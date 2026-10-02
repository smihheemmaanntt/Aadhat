Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

' Portable WinForms responsive scaler.  Copy this file into another project
' and call GenericResponsiveFormScaler.Bind(Me) in each form's Load event.
' It has no AccoBook, MDI, database, or third-party dependencies.
Public NotInheritable Class GenericResponsiveFormScaler
    Implements IDisposable

    Private NotInheritable Class ControlState
        Public Bounds As Rectangle
        Public Font As Font
        Public GridColumnWidths As List(Of Integer)
        Public GridHeaderHeight As Integer
        Public GridRowHeight As Integer
        Public GridHeaderFont As Font
        Public GridCellFont As Font
        Public ComboItemHeight As Integer
        Public PreserveAutoSizeRightEdge As Boolean
    End Class

    Private ReadOnly targetForm As Form
    Private ReadOnly states As New Dictionary(Of Control, ControlState)()
    Private Shared ReadOnly activeScalers As New Dictionary(Of Form, GenericResponsiveFormScaler)()
    Private Shared ReadOnly mdiParentLayouts As New Dictionary(Of Form, MdiParentSidebarLayout)()
    Private baseClientSize As Size
    Private isCaptured As Boolean
    Private isApplying As Boolean
    Private isConfiguringMdiBounds As Boolean
    Private disposed As Boolean
    Private isFullWorkspaceForm As Boolean
    Private lastAppliedClientSize As Size = Size.Empty
    Private hookedMdiParent As Form
    Private hookedMdiClient As MdiClient

    Private Const WM_SETREDRAW As Integer = &HB
    Private Const WorkspaceContentInset As Integer = 14
    Private Const CreateAccountControlScaleLimit As Double = 1.4R

    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer,
                                        ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    Private Sub New(ByVal form As Form)
        targetForm = form
        AddHandler targetForm.HandleCreated, AddressOf OnHandleCreated
        AddHandler targetForm.Load, AddressOf OnFormLoad
        AddHandler targetForm.Shown, AddressOf OnFormShown
        AddHandler targetForm.Resize, AddressOf OnFormResize
        AddHandler targetForm.ParentChanged, AddressOf OnParentChanged
        AddHandler targetForm.FormClosed, AddressOf OnFormClosed

        ' Bind(Me) is normally called from a form's Load handler.  At that
        ' point the Load event has already begun, so activate immediately.
        If targetForm.IsHandleCreated Then InitializeScaler()
    End Sub

    Public Shared Function Bind(ByVal form As Form) As GenericResponsiveFormScaler
        If form Is Nothing Then Throw New ArgumentNullException("form")

        ' These workspace/overlay forms are borderless at runtime, but their
        ' designer definitions remain sizable. Normalize the non-client frame
        ' before the first display so Windows cannot paint a temporary title
        ' bar while the responsive layout is being applied.
        If IsBorderlessResponsiveForm(form) AndAlso form.FormBorderStyle <> FormBorderStyle.None Then
            form.FormBorderStyle = FormBorderStyle.None
        End If

        SyncLock activeScalers
            Dim existing As GenericResponsiveFormScaler = Nothing
            If activeScalers.TryGetValue(form, existing) Then Return existing

            Dim scaler As New GenericResponsiveFormScaler(form)
            activeScalers(form) = scaler
            Return scaler
        End SyncLock
    End Function

    Private Shared Function IsBorderlessResponsiveForm(ByVal form As Form) As Boolean
        If form Is Nothing Then Return False
        Select Case form.Name
            Case "MainScreenPicture", "Login", "CompanyList"
                Return True
            Case Else
                Return False
        End Select
    End Function

    ' Lets an overlay capture its designer dimensions before it is moved to
    ' its final target rectangle and before the first visible paint.
    Public Sub PrepareForFirstShow()
        If disposed OrElse targetForm.IsDisposed Then Return
        Capture()
        EnableDoubleBuffer(targetForm)
    End Sub

    ' This path is intentionally parent-only: it lays out a docked shortcut
    ' sidebar and never reads, resizes, or binds any MDI child form.
    Public Shared Sub BindMdiParent(ByVal parentForm As Form, ByVal rightDockPanel As Panel)
        If parentForm Is Nothing OrElse rightDockPanel Is Nothing Then Return
        SyncLock mdiParentLayouts
            If mdiParentLayouts.ContainsKey(parentForm) Then Return
            mdiParentLayouts(parentForm) = New MdiParentSidebarLayout(parentForm, rightDockPanel)
        End SyncLock
    End Sub

    Private NotInheritable Class MdiParentSidebarLayout
        Private NotInheritable Class SidebarControlState
            Public Bounds As Rectangle
            Public Font As Font
        End Class

        Private ReadOnly parentForm As Form
        Private ReadOnly sidebar As Panel
        Private ReadOnly states As New Dictionary(Of Control, SidebarControlState)()
        Private contentWidth As Integer
        Private contentHeight As Integer
        Private applying As Boolean

        Public Sub New(ByVal owner As Form, ByVal panel As Panel)
            parentForm = owner
            sidebar = panel
            Capture()
            EnableDoubleBuffer(parentForm)
            EnableDoubleBuffer(sidebar)
            AddHandler parentForm.Resize, AddressOf ApplyLayout
            AddHandler sidebar.Resize, AddressOf ApplyLayout
            AddHandler parentForm.FormClosed, AddressOf ParentClosed
            ApplyLayout(Nothing, EventArgs.Empty)
        End Sub

        Private Sub Capture()
            For Each child As Control In sidebar.Controls
                states(child) = New SidebarControlState With {.Bounds = child.Bounds, .Font = child.Font}
                ' The shortcuts repaint on hover.  Buffer each direct child so
                ' colour changes never expose an intermediate background frame.
                EnableDoubleBuffer(child)
                contentWidth = Math.Max(contentWidth, child.Right)
                contentHeight = Math.Max(contentHeight, child.Bottom)
            Next
            contentWidth = Math.Max(1, contentWidth)
            contentHeight = Math.Max(1, contentHeight)
        End Sub

        Private Sub ApplyLayout(ByVal sender As Object, ByVal e As EventArgs)
            If applying OrElse sidebar.IsDisposed OrElse sidebar.ClientSize.Width < 1 OrElse sidebar.ClientSize.Height < 1 Then Return
            applying = True
            Try
                Dim scaleX As Double = CDbl(sidebar.ClientSize.Width) / contentWidth
                Dim scaleY As Double = CDbl(sidebar.ClientSize.Height) / contentHeight
                Dim fontScale As Double = Math.Min(scaleX, scaleY)
                sidebar.SuspendLayout()
                For Each entry As KeyValuePair(Of Control, SidebarControlState) In states
                    Dim child As Control = entry.Key
                    Dim state As SidebarControlState = entry.Value
                    If child Is Nothing OrElse child.IsDisposed Then Continue For
                    Dim scaledBounds As New Rectangle(
                        CInt(Math.Round(state.Bounds.Left * scaleX)),
                        CInt(Math.Round(state.Bounds.Top * scaleY)),
                        Math.Max(1, CInt(Math.Round(state.Bounds.Width * scaleX))),
                        Math.Max(1, CInt(Math.Round(state.Bounds.Height * scaleY))))
                    ' Preserve the exact layout but avoid redundant layout
                    ' and paint work when a sidebar control is already placed.
                    If child.Bounds <> scaledBounds Then child.Bounds = scaledBounds
                    If state.Font IsNot Nothing Then
                        Dim size As Single = Math.Max(6.5F, CSng(Math.Round(state.Font.Size * fontScale, 1)))
                        If Math.Abs(child.Font.Size - size) > 0.05F Then child.Font = New Font(state.Font.FontFamily, size, state.Font.Style)
                    End If
                Next
            Finally
                sidebar.ResumeLayout(True)
                applying = False
            End Try
        End Sub

        Private Sub ParentClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
            RemoveHandler parentForm.Resize, AddressOf ApplyLayout
            RemoveHandler sidebar.Resize, AddressOf ApplyLayout
            SyncLock mdiParentLayouts
                mdiParentLayouts.Remove(parentForm)
            End SyncLock
        End Sub
    End Class

    Private Sub OnHandleCreated(sender As Object, e As EventArgs)
        InitializeScaler()
    End Sub

    Private Sub OnFormLoad(sender As Object, e As EventArgs)
        InitializeScaler()
    End Sub

    Private Sub OnFormShown(sender As Object, e As EventArgs)
        ConfigureMdiChildBounds()
        ' Only MDI children can receive their final MDI-client bounds after
        ' Load. Standalone forms are already laid out during Load, so do not
        ' run a second visible full-tree layout/repaint for them.
        Dim finalLayoutPending As Boolean = targetForm.MdiParent IsNot Nothing AndAlso
                                            targetForm.ClientSize <> lastAppliedClientSize
        If finalLayoutPending OrElse CaptureRuntimeGridMetrics() Then Apply(True)
    End Sub

    Private Sub InitializeScaler()
        Capture()
        EnableDoubleBuffer(targetForm)
        HookMdiParent()
        ConfigureMdiChildBounds()
        Apply()
    End Sub

    Private Sub OnFormResize(sender As Object, e As EventArgs)
        If isConfiguringMdiBounds Then Return
        Apply()
    End Sub

    Private Sub OnParentChanged(sender As Object, e As EventArgs)
        HookMdiParent()
        ConfigureMdiChildBounds()
        Apply()
    End Sub

    Private Sub HookMdiParent()
        Dim mdiParent As Form = targetForm.MdiParent
        If mdiParent Is hookedMdiParent Then Return

        If hookedMdiClient IsNot Nothing Then RemoveHandler hookedMdiClient.Resize, AddressOf OnMdiClientResize
        hookedMdiParent = mdiParent
        hookedMdiClient = Nothing

        If hookedMdiParent Is Nothing Then Return
        For Each child As Control In hookedMdiParent.Controls
            Dim mdi = TryCast(child, MdiClient)
            If mdi IsNot Nothing Then
                hookedMdiClient = mdi
                EnableDoubleBuffer(mdi)
                AddHandler hookedMdiClient.Resize, AddressOf OnMdiClientResize
                Exit For
            End If
        Next
    End Sub

    Private Sub OnMdiClientResize(sender As Object, e As EventArgs)
        ConfigureMdiChildBounds()
        Apply()
    End Sub

    Private Sub ConfigureMdiChildBounds()
        If isConfiguringMdiBounds OrElse Not isCaptured OrElse Not isFullWorkspaceForm OrElse
           hookedMdiClient Is Nothing OrElse hookedMdiClient.ClientSize.Width < 50 OrElse hookedMdiClient.ClientSize.Height < 50 Then Return

        isConfiguringMdiBounds = True
        Try
            If targetForm.WindowState <> FormWindowState.Normal Then targetForm.WindowState = FormWindowState.Normal
            If targetForm.Left <> 0 OrElse targetForm.Top <> 0 OrElse
               targetForm.Width <> hookedMdiClient.ClientSize.Width OrElse targetForm.Height <> hookedMdiClient.ClientSize.Height Then
                targetForm.StartPosition = FormStartPosition.Manual
                targetForm.SetBounds(0, 0, hookedMdiClient.ClientSize.Width, hookedMdiClient.ClientSize.Height)
            End If
        Finally
            isConfiguringMdiBounds = False
        End Try
    End Sub

    Private Shared Sub EnableDoubleBuffer(ByVal control As Control)
        If control Is Nothing Then Return
        Try
            Dim propertyInfo = GetType(Control).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            If propertyInfo IsNot Nothing Then propertyInfo.SetValue(control, True, Nothing)
        Catch
        End Try
    End Sub

    Private Sub OnFormClosed(sender As Object, e As FormClosedEventArgs)
        Dispose()
    End Sub

    Private Sub Capture()
        If isCaptured OrElse targetForm.IsDisposed Then Return
        baseClientSize = targetForm.ClientSize
        If baseClientSize.Width < 1 OrElse baseClientSize.Height < 1 Then Return
        ' Overlay dialogs (for example Login over CompanyList.dg1) can become
        ' large on high-resolution displays, but must remain inside their own
        ' target rectangle rather than being promoted to the entire MDI client.
        isFullWorkspaceForm = baseClientSize.Width >= 950 AndAlso baseClientSize.Height >= 500 AndAlso Not IsResponsiveOverlay()
        If isFullWorkspaceForm Then
            ' Some legacy full-screen forms have background/logo controls that
            ' intentionally extend past ClientSize.  Treat those extents as the
            ' design canvas so the bottom edge is not scaled past the MDI area.
            Dim designWidth As Integer = baseClientSize.Width
            Dim designHeight As Integer = baseClientSize.Height
            For Each child As Control In targetForm.Controls
                If Not TypeOf child Is MdiClient Then
                    designWidth = Math.Max(designWidth, child.Right)
                    designHeight = Math.Max(designHeight, child.Bottom)
                End If
            Next
            baseClientSize = New Size(designWidth, designHeight)
        End If
        CaptureChildren(targetForm)
        isCaptured = True
    End Sub

    Private Function IsResponsiveOverlay() As Boolean
        Dim marker As String = TryCast(targetForm.Tag, String)
        Return String.Equals(If(marker, "").Trim(), "ResponsiveOverlay", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function IsCreateAccount() As Boolean
        Return String.Equals(targetForm.Name, "CreateAccount", StringComparison.OrdinalIgnoreCase)
    End Function

    ' Keep the dashboard's first and last sale-card margins symmetrical.  The
    ' Purchase panel is deliberately wider than the middle cards, so shifting
    ' it would make it overlap them; trim only its excess right-side width.
    Private Sub ApplyRightEdgeSafetySpacing()
        If String.Equals(targetForm.Name, "MainScreenPicture", StringComparison.OrdinalIgnoreCase) Then
            Dim firstCard As Control = targetForm.Controls("Panel2")
            Dim purchaseCard As Control = targetForm.Controls("Panel1")
            If firstCard IsNot Nothing AndAlso purchaseCard IsNot Nothing Then
                Dim desiredRight As Integer = targetForm.ClientSize.Width - firstCard.Left
                If purchaseCard.Right > desiredRight Then
                    purchaseCard.Width = Math.Max(1, purchaseCard.Width - (purchaseCard.Right - desiredRight))
                End If
            End If
        End If

        ' Legacy entry forms place CLOSE on the outer canvas edge.  Leave a
        ' small visual buffer so it cannot look clipped by the shortcut bar.
        For Each child As Control In targetForm.Controls
            If String.Equals(child.Name, "btnClose", StringComparison.OrdinalIgnoreCase) AndAlso
               child.Right > targetForm.ClientSize.Width - WorkspaceContentInset Then
                child.Left = Math.Max(0, targetForm.ClientSize.Width - WorkspaceContentInset - child.Width)
            End If
        Next
    End Sub

    Private Sub CaptureChildren(ByVal parent As Control)
        EnableDoubleBuffer(parent)
        For Each child As Control In parent.Controls
            EnableDoubleBuffer(child)
            Dim state As New ControlState With {
                .Bounds = child.Bounds,
                .Font = child.Font
            }
            ' Some legacy captions are AutoSize labels positioned so their
            ' right edge lines up immediately before a value field.  Changing
            ' the font later makes AutoSize recalculate its width and moves
            ' that edge unless it is restored after scaling.
            Dim label = TryCast(child, Label)
            If label IsNot Nothing AndAlso label.AutoSize Then
                For Each sibling As Control In parent.Controls
                    If sibling Is child Then Continue For
                    If Not (TypeOf sibling Is TextBox OrElse TypeOf sibling Is ComboBox OrElse TypeOf sibling Is MaskedTextBox) Then Continue For
                    Dim gap As Integer = sibling.Left - child.Right
                    Dim verticallyAligned As Boolean = Math.Abs((sibling.Top + sibling.Height \ 2) - (child.Top + child.Height \ 2)) <= 12
                    If gap >= 0 AndAlso gap <= 24 AndAlso verticallyAligned Then
                        state.PreserveAutoSizeRightEdge = True
                        Exit For
                    End If
                Next
            End If
            Dim grid = TryCast(child, DataGridView)
            Dim combo = TryCast(child, ComboBox)
            If combo IsNot Nothing Then
                state.ComboItemHeight = combo.ItemHeight
            End If
            If grid IsNot Nothing Then
                state.GridColumnWidths = New List(Of Integer)()
                For Each column As DataGridViewColumn In grid.Columns
                    state.GridColumnWidths.Add(column.Width)
                Next
                state.GridHeaderHeight = grid.ColumnHeadersHeight
                state.GridRowHeight = grid.RowTemplate.Height
                state.GridHeaderFont = grid.ColumnHeadersDefaultCellStyle.Font
                state.GridCellFont = grid.DefaultCellStyle.Font
            End If
            states(child) = state
            If child.HasChildren Then CaptureChildren(child)
        Next
    End Sub

    Public Sub Apply(Optional ByVal force As Boolean = False)
        If disposed OrElse isApplying OrElse Not isCaptured OrElse targetForm.IsDisposed Then Return
        If targetForm.ClientSize.Width < 1 OrElse targetForm.ClientSize.Height < 1 Then Return
        If Not force AndAlso targetForm.ClientSize = lastAppliedClientSize Then Return

        Dim scaleX As Double = CDbl(targetForm.ClientSize.Width) / baseClientSize.Width
        Dim scaleY As Double = CDbl(targetForm.ClientSize.Height) / baseClientSize.Height
        ' Preserve the legacy designer layout's aspect ratio.  Applying a
        ' separate X/Y scale stretches controls vertically on wide displays
        ' (for example 1920x1080), making the lower section look pulled down.
        Dim layoutScale As Double = Math.Min(scaleX, scaleY)
        If isFullWorkspaceForm Then
            ' Legacy dashboard/form controls are often designed right on the
            ' canvas edge.  Reserve a small viewport inset so a scaled card or
            ' bottom control never visually touches the MDI edge.
            Dim insetScaleX As Double = CDbl(Math.Max(1, targetForm.ClientSize.Width - WorkspaceContentInset)) / baseClientSize.Width
            Dim insetScaleY As Double = CDbl(Math.Max(1, targetForm.ClientSize.Height - WorkspaceContentInset)) / baseClientSize.Height
            layoutScale = Math.Min(layoutScale, Math.Min(insetScaleX, insetScaleY))
        End If
        If IsCreateAccount() Then
            layoutScale = Math.Min(layoutScale, CreateAccountControlScaleLimit)
        End If
        Dim fontScale As Double = layoutScale
        ' Keep X and Y on the same scale so controls retain their designer
        ' spacing rather than drifting toward the right edge.
        Dim horizontalControlScale As Double = layoutScale
        isApplying = True
        Try
            SetRedraw(False)
            targetForm.SuspendLayout()
            For Each entry In states
                Dim control As Control = entry.Key
                Dim state As ControlState = entry.Value
                If control Is Nothing OrElse control.IsDisposed Then Continue For

                Dim scaledBounds As New Rectangle(
                    CInt(Math.Round(state.Bounds.Left * horizontalControlScale)),
                    CInt(Math.Round(state.Bounds.Top * layoutScale)),
                    Math.Max(1, CInt(Math.Round(state.Bounds.Width * horizontalControlScale))),
                    Math.Max(1, CInt(Math.Round(state.Bounds.Height * layoutScale))))
                ' Avoid firing layout/paint work for controls already at their
                ' final responsive bounds. This preserves the exact geometry
                ' while reducing lag on lower-powered PCs.
                If control.Bounds <> scaledBounds Then control.Bounds = scaledBounds

                ' A PictureBox in Normal mode keeps the bitmap at its original
                ' pixel size even though the responsive pass has enlarged the
                ' control.  For MDI children, use Zoom so a manual resize of
                ' the MDI parent scales the image too, without distortion.
                Dim picture = TryCast(control, PictureBox)
                If targetForm.MdiParent IsNot Nothing AndAlso picture IsNot Nothing AndAlso
                   picture.Image IsNot Nothing AndAlso picture.SizeMode = PictureBoxSizeMode.Normal Then
                    picture.SizeMode = PictureBoxSizeMode.Zoom
                End If

                If state.Font IsNot Nothing Then
                    Dim scaledSize As Single = Math.Max(6.5F, CSng(Math.Round(state.Font.Size * fontScale, 1)))
                    If Math.Abs(control.Font.Size - scaledSize) > 0.05F OrElse
                       control.Font.FontFamily.Name <> state.Font.FontFamily.Name OrElse
                       control.Font.Style <> state.Font.Style Then
                        control.Font = New Font(state.Font.FontFamily, scaledSize, state.Font.Style)
                    End If
                End If

                ' AutoSize recalculates the label's width after its font is
                ' changed.  For captions paired to a value field, retain the
                ' designer's right edge instead of leaving the caption shifted
                ' left or right at runtime.
                If state.PreserveAutoSizeRightEdge AndAlso TypeOf control Is Label AndAlso DirectCast(control, Label).AutoSize Then
                    control.Left = CInt(Math.Round(state.Bounds.Right * horizontalControlScale)) - control.Width
                End If

                ' A ComboBox ignores most explicit Height assignments and
                ' derives its visible edit area from ItemHeight.  Scale that
                ' value too, otherwise its width grows while its height stays
                ' at the original designer value.
                Dim combo = TryCast(control, ComboBox)
                If combo IsNot Nothing AndAlso state.ComboItemHeight > 0 Then
                    combo.ItemHeight = Math.Max(13, CInt(Math.Round(state.ComboItemHeight * fontScale)))
                End If

                ApplyGridScale(TryCast(control, DataGridView), state, horizontalControlScale, layoutScale)
            Next
            ApplyRightEdgeSafetySpacing()
        Finally
            targetForm.ResumeLayout(True)
            SetRedraw(True)
            ' WM_SETREDRAW suppresses the invalidations raised by child controls
            ' while their bounds are updated.  Repaint the complete child tree
            ' once after the final layout; otherwise labels/pictures can remain
            ' blank until a mouse hover happens to invalidate them.
            targetForm.Invalidate(True)
            ' Do not force a synchronous paint here.  On an MDI child that can
            ' expose one transient frame during Show; the queued invalidation
            ' draws the same completed layout in the next normal paint pass.
            isApplying = False
            lastAppliedClientSize = targetForm.ClientSize
        End Try
    End Sub

    ' Apply can update many legacy controls in one pass.  Hold painting until
    ' all existing responsive bounds are in place, preventing intermediate
    ' designer-size frames from being shown.  This does not alter sizing.
    Private Sub SetRedraw(ByVal enabled As Boolean)
        If Not targetForm.IsHandleCreated OrElse targetForm.IsDisposed Then Return
        Try
            SendMessage(targetForm.Handle, WM_SETREDRAW, If(enabled, New IntPtr(1), IntPtr.Zero), IntPtr.Zero)
        Catch
        End Try
    End Sub

    Private Shared Sub ApplyGridScale(ByVal grid As DataGridView, ByVal state As ControlState,
                                      ByVal scaleX As Double, ByVal scaleY As Double)
        If grid Is Nothing OrElse state.GridColumnWidths Is Nothing Then Return
        While state.GridColumnWidths.Count < grid.Columns.Count
            state.GridColumnWidths.Add(grid.Columns(state.GridColumnWidths.Count).Width)
        End While
        For index As Integer = 0 To Math.Min(grid.Columns.Count, state.GridColumnWidths.Count) - 1
            grid.Columns(index).Width = Math.Max(grid.Columns(index).MinimumWidth,
                CInt(Math.Round(state.GridColumnWidths(index) * scaleX)))
        Next
        FitVisibleGridColumnsToViewport(grid)
        If state.GridHeaderHeight > 0 Then
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            grid.ColumnHeadersHeight = Math.Max(26, CInt(Math.Round(state.GridHeaderHeight * scaleY)))
        End If
        If state.GridHeaderFont IsNot Nothing Then
            Dim headerSize As Single = Math.Max(6.5F, CSng(Math.Round(state.GridHeaderFont.Size * Math.Min(scaleX, scaleY), 1)))
            grid.ColumnHeadersDefaultCellStyle.Font = New Font(state.GridHeaderFont.FontFamily, headerSize, state.GridHeaderFont.Style)
        End If
        If state.GridCellFont IsNot Nothing Then
            Dim cellSize As Single = Math.Max(6.5F, CSng(Math.Round(state.GridCellFont.Size * Math.Min(scaleX, scaleY), 1)))
            grid.DefaultCellStyle.Font = New Font(state.GridCellFont.FontFamily, cellSize, state.GridCellFont.Style)
        End If
        If state.GridRowHeight > 0 Then
            Dim rowHeight = Math.Max(18, CInt(Math.Round(state.GridRowHeight * scaleY)))
            grid.RowTemplate.Height = rowHeight
            For Each row As DataGridViewRow In grid.Rows
                row.Height = rowHeight
            Next
        End If
    End Sub

    Private Function CaptureRuntimeGridMetrics() As Boolean
        Dim changed As Boolean = False
        For Each entry As KeyValuePair(Of Control, ControlState) In states
            Dim grid As DataGridView = TryCast(entry.Key, DataGridView)
            Dim state As ControlState = entry.Value
            If grid Is Nothing OrElse grid.IsDisposed OrElse state.GridColumnWidths Is Nothing Then Continue For

            While state.GridColumnWidths.Count < grid.Columns.Count
                state.GridColumnWidths.Add(grid.Columns(state.GridColumnWidths.Count).Width)
                changed = True
            End While
            If state.GridHeaderHeight <= 0 Then
                state.GridHeaderHeight = grid.ColumnHeadersHeight
                changed = True
            End If
            If state.GridRowHeight <= 0 Then
                state.GridRowHeight = grid.RowTemplate.Height
                changed = True
            End If
            If state.GridHeaderFont Is Nothing Then
                state.GridHeaderFont = grid.ColumnHeadersDefaultCellStyle.Font
                changed = True
            End If
            If state.GridCellFont Is Nothing Then
                state.GridCellFont = grid.DefaultCellStyle.Font
                changed = True
            End If
        Next
        Return changed
    End Function

    Private Shared Sub FitVisibleGridColumnsToViewport(ByVal grid As DataGridView)
        If grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill Then Return

        Dim visibleColumns As New List(Of DataGridViewColumn)()
        Dim totalWidth As Integer = 0
        For Each column As DataGridViewColumn In grid.Columns
            If column.Visible Then
                visibleColumns.Add(column)
                totalWidth += column.Width
            End If
        Next
        If visibleColumns.Count = 0 Then Return

        Dim availableWidth As Integer = grid.ClientSize.Width
        If grid.RowHeadersVisible Then availableWidth -= grid.RowHeadersWidth
        availableWidth -= 2
        If availableWidth <= totalWidth Then Return

        Dim remainingWidth As Integer = availableWidth
        For index As Integer = 0 To visibleColumns.Count - 1
            Dim column As DataGridViewColumn = visibleColumns(index)
            Dim targetWidth As Integer
            If index = visibleColumns.Count - 1 Then
                targetWidth = Math.Max(column.MinimumWidth, remainingWidth)
            Else
                targetWidth = Math.Max(column.MinimumWidth, CInt(Math.Round(CDbl(column.Width) * availableWidth / totalWidth)))
                remainingWidth -= targetWidth
            End If
            column.Width = targetWidth
        Next
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If disposed Then Return
        RemoveHandler targetForm.Load, AddressOf OnFormLoad
        RemoveHandler targetForm.HandleCreated, AddressOf OnHandleCreated
        RemoveHandler targetForm.Shown, AddressOf OnFormShown
        RemoveHandler targetForm.Resize, AddressOf OnFormResize
        RemoveHandler targetForm.ParentChanged, AddressOf OnParentChanged
        RemoveHandler targetForm.FormClosed, AddressOf OnFormClosed
        If hookedMdiClient IsNot Nothing Then RemoveHandler hookedMdiClient.Resize, AddressOf OnMdiClientResize
        states.Clear()
        SyncLock activeScalers
            activeScalers.Remove(targetForm)
        End SyncLock
        disposed = True
    End Sub
End Class

' Keeps a small non-modal entry form above its MDI/background form without
' disabling that background form.  Enable it only for utility dialogs that
' should stay in front after an accidental click behind them.
Public NotInheritable Class NonModalFormFrontGuard
    Private Shared ReadOnly Guards As New Dictionary(Of Form, NonModalFormFrontGuard)()
    Private ReadOnly guardedForm As Form
    Private disposed As Boolean

    Private Sub New(ByVal form As Form)
        guardedForm = form
        AddHandler guardedForm.Deactivate, AddressOf OnFormDeactivate
        AddHandler guardedForm.FormClosed, AddressOf OnFormClosed
    End Sub

    Public Shared Sub Enable(ByVal form As Form)
        If form Is Nothing Then Return
        SyncLock Guards
            If Guards.ContainsKey(form) Then Return
            Guards(form) = New NonModalFormFrontGuard(form)
        End SyncLock
    End Sub

    Public Shared Sub Disable(ByVal form As Form)
        If form Is Nothing Then Return
        Dim guard As NonModalFormFrontGuard = Nothing
        SyncLock Guards
            If Guards.TryGetValue(form, guard) Then Guards.Remove(form)
        End SyncLock
        If guard IsNot Nothing Then guard.Dispose()
    End Sub

    Private Sub OnFormDeactivate(sender As Object, e As EventArgs)
        If disposed OrElse guardedForm.IsDisposed OrElse Not guardedForm.Visible Then Return
        Try
            ' Let Windows complete the click behind the form first; then restore
            ' only this form's z-order without changing any other form's state.
            guardedForm.BeginInvoke(New MethodInvoker(Sub()
                                                           If disposed OrElse guardedForm.IsDisposed OrElse Not guardedForm.Visible Then Return
                                                           guardedForm.BringToFront()
                                                       End Sub))
        Catch
            ' The form may have been disposed before the queued callback runs.
        End Try
    End Sub

    Private Sub OnFormClosed(sender As Object, e As FormClosedEventArgs)
        Disable(guardedForm)
    End Sub

    Private Sub Dispose()
        If disposed Then Return
        RemoveHandler guardedForm.Deactivate, AddressOf OnFormDeactivate
        RemoveHandler guardedForm.FormClosed, AddressOf OnFormClosed
        SyncLock Guards
            Guards.Remove(guardedForm)
        End SyncLock
        disposed = True
    End Sub
End Class
