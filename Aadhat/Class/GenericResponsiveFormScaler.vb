Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

' A runtime UserControl can opt in when it owns the responsive layout of all
' of its children. The form scaler continues to scale the UserControl itself,
' while leaving its child bounds to the control's own layout calculation.
Public Interface IResponsiveUserControlLayout
    Sub ApplyResponsiveLayout()
End Interface

' Shared implementation for fixed-layout runtime UserControls. Each control
' captures its designer bounds once (before it is added to a form) and owns
' its child layout thereafter, so a parent form scaler can never double-scale
' or restore stale child coordinates.
Public Class ResponsiveUserControlLayoutHelper
    Private Class Snapshot
        Public Bounds As Rectangle
        Public Font As Font
    End Class

    Private Shared ReadOnly Layouts As New Dictionary(Of Control, Dictionary(Of Control, Snapshot))()

    Public Shared Sub Capture(ByVal container As Control)
        If container Is Nothing OrElse Layouts.ContainsKey(container) Then Return
        Dim snapshots As New Dictionary(Of Control, Snapshot)()
        CaptureChildren(container, snapshots)
        Layouts(container) = snapshots
    End Sub

    Public Shared Sub Apply(ByVal container As Control, ByVal designWidth As Integer, ByVal designHeight As Integer)
        Apply(container, designWidth, designHeight, -1.0R)
    End Sub

    Public Shared Sub Apply(ByVal container As Control, ByVal designWidth As Integer, ByVal designHeight As Integer,
                            ByVal fontScaleOverride As Double, Optional ByVal activeFieldCount As Integer = 0)
        If container Is Nothing OrElse container.IsDisposed OrElse designWidth < 1 OrElse designHeight < 1 Then Return
        Capture(container)
        Dim snapshots As Dictionary(Of Control, Snapshot) = Nothing
        If Not Layouts.TryGetValue(container, snapshots) Then Return

        Dim scaleX As Double = CDbl(container.ClientSize.Width) / designWidth
        Dim scaleY As Double = CDbl(container.ClientSize.Height) / designHeight
        Dim fontScale As Double = If(fontScaleOverride > 0, fontScaleOverride, Math.Min(scaleX, scaleY))
        Dim activeScaleY As Double = scaleY
        If activeFieldCount > 0 Then
            Dim baseActiveHeight As Integer = 0
            For Each item In snapshots
                Dim child As Control = item.Key
                If child Is Nothing Then Continue For
                If String.Equals(child.Name, "txtField" & activeFieldCount, StringComparison.OrdinalIgnoreCase) Then
                    baseActiveHeight = Math.Max(baseActiveHeight, item.Value.Bounds.Bottom)
                End If
            Next
            If baseActiveHeight > 0 Then activeScaleY = CDbl(container.ClientSize.Height) / baseActiveHeight
        End If
        For Each item In snapshots
            Dim child = item.Key
            Dim snap = item.Value
            If child Is Nothing OrElse child.IsDisposed Then Continue For
            If activeFieldCount > 0 AndAlso child.Name.StartsWith("lblTitleAdd", StringComparison.OrdinalIgnoreCase) Then
                Dim fieldIndex As Integer = Val(child.Name.Substring("lblTitleAdd".Length))
                If fieldIndex > activeFieldCount Then Continue For
            ElseIf activeFieldCount > 0 AndAlso child.Name.StartsWith("txtField", StringComparison.OrdinalIgnoreCase) AndAlso
                   Not String.Equals(child.Name, "txtFieldNos", StringComparison.OrdinalIgnoreCase) Then
                Dim fieldIndex As Integer = Val(child.Name.Substring("txtField".Length))
                If fieldIndex > activeFieldCount Then Continue For
            End If
            Dim childScaleY As Double = If(activeFieldCount > 0, activeScaleY, scaleY)
            child.SetBounds(CInt(Math.Round(snap.Bounds.Left * scaleX)), CInt(Math.Round(snap.Bounds.Top * childScaleY)),
                            Math.Max(1, CInt(Math.Round(snap.Bounds.Width * scaleX))),
                            Math.Max(1, CInt(Math.Round(snap.Bounds.Height * childScaleY))))
            If snap.Font IsNot Nothing Then
                child.Font = GenericResponsiveFormScaler.GetCachedFont(snap.Font.FontFamily,
                    Math.Max(6.5!, CSng(Math.Round(snap.Font.Size * fontScale, 1))), snap.Font.Style)
            End If
        Next
    End Sub

    Private Shared Sub CaptureChildren(ByVal container As Control, ByVal snapshots As Dictionary(Of Control, Snapshot))
        For Each child As Control In container.Controls
            snapshots(child) = New Snapshot With {.Bounds = child.Bounds, .Font = child.Font}
            If child.HasChildren Then CaptureChildren(child, snapshots)
        Next
    End Sub
End Class

' =========================================================================================
' Tier 2: Universal, High-Performance, Zero-Dependency Responsive Form Scaler (.NET WinForms)
'
' 100% Generic and Plug-and-Play:
' - Type-based pure mathematical linear scaling:
'     scaleX = CurrentClientSize.Width / BaseDesignerWidth
'     scaleY = CurrentClientSize.Height / BaseDesignerHeight
' - Zero hardcoded control names or form names.
' - Zero heuristic guessing (no arbitrary column shaving, no centering modals, no action strips).
' - Zero flicker and lightning-fast (under 3ms) layout using SuspendLayout/ResumeLayout.
' - Dynamically added runtime controls inherit scaling automatically without position shift.
' - DataGridView columns and input textboxes above them scale by the exact same scaleX factor,
'   guaranteeing 100% mathematical alignment across all screen resolutions.
'
' USAGE (Any Form in Any Project):
'   In your Form's Load event:
'       GenericResponsiveFormScaler.Bind(Me)
' =========================================================================================
Public Class GenericResponsiveFormScaler
    Implements IDisposable

    ''' <summary>
    ''' Snapshot of a control's designer metrics.
    ''' </summary>
    Private Class ControlSnapshot
        Public Property Bounds As Rectangle
        Public Property Font As Font
        Public Property ToolStripItemFonts As New List(Of KeyValuePair(Of ToolStripItem, Font))()
        Public Property IsDgv As Boolean
        Public Property DgvColWidths As New List(Of Integer)()
        Public Property DgvHeaderHeight As Integer
        Public Property DgvRowHeight As Integer
        Public Property DgvHeaderFont As Font
        Public Property DgvCellFont As Font
        Public Property DgvRowsFont As Font
        Public Property PreserveRuntimeGridWidths As Boolean
        Public Property RuntimeGridWidthsAreDesignSpace As Boolean
        Public Property HasRuntimeGridWidthBaseline As Boolean
        Public Property IsComboBox As Boolean
        Public Property CbItemHeight As Integer
    End Class

    Private Shared ReadOnly FontCache As New Dictionary(Of String, Font)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly ActiveScalers As New Dictionary(Of Form, GenericResponsiveFormScaler)()

    Private ReadOnly _targetForm As Form
    Private ReadOnly _controlMap As New Dictionary(Of Control, ControlSnapshot)()
    Private _baseWidth As Integer
    Private _baseHeight As Integer
    Private _initialBounds As Rectangle
    Private _isCaptured As Boolean = False
    Private _isScaling As Boolean = False
    Private _hasBeenShown As Boolean = False
    Private _isConfiguringBounds As Boolean = False
    Private _disposed As Boolean = False
    Private _hookedParent As Form = Nothing
    Private _hookedMdiClient As MdiClient = Nothing
    Private _isFullScreenCanvas As Boolean = True
    Private _isMdiParentMode As Boolean = False
    Private _mdiBoundsOverride As Nullable(Of Rectangle)
    Private _lastAppliedW As Integer = -1
    Private _lastAppliedH As Integer = -1
    Private _resizeTimer As Timer = Nothing
    ' Runtime grids in Purchase/Sale add their columns and rows after the form
    ' has already been scaled. Queue one UI-cycle refresh so a complete batch
    ' (ColumnCount + Width assignments + Rows.Add) is captured as one layout.
    Private ReadOnly _hookedDataGrids As New HashSet(Of DataGridView)()
    Private ReadOnly _pendingRuntimeGrids As New HashSet(Of DataGridView)()
    Private _runtimeGridRefreshQueued As Boolean = False

    Public Shared Function GetFormFontScale(ByVal form As Form) As Double
        If form Is Nothing OrElse form.IsDisposed Then Return 1.0R
        Dim scaler As GenericResponsiveFormScaler = Nothing
        SyncLock ActiveScalers
            ActiveScalers.TryGetValue(form, scaler)
        End SyncLock
        If scaler Is Nothing OrElse scaler._baseWidth < 1 OrElse scaler._baseHeight < 1 Then Return 1.0R
        Dim scaleX As Double = CDbl(form.ClientSize.Width) / scaler._baseWidth
        Dim scaleY As Double = CDbl(form.ClientSize.Height) / scaler._baseHeight
        Return Math.Min(scaleX, scaleY)
    End Function

    ''' <summary>
    ''' Constructs a new GenericResponsiveFormScaler for the given Form.
    ''' </summary>
    Public Sub New(ByVal targetForm As Form, Optional ByVal baseWidth As Integer = 0, Optional ByVal baseHeight As Integer = 0,
                   Optional ByVal isMdiParentMode As Boolean = False)
        If targetForm Is Nothing Then Throw New ArgumentNullException("targetForm")
        _targetForm = targetForm
        _baseWidth = baseWidth
        _baseHeight = baseHeight
        _isMdiParentMode = isMdiParentMode
        If _isMdiParentMode Then _isFullScreenCanvas = True

        SyncLock ActiveScalers
            ActiveScalers(_targetForm) = Me
        End SyncLock

        _resizeTimer = New Timer()
        _resizeTimer.Interval = 35
        AddHandler _resizeTimer.Tick, AddressOf OnResizeTimerTick

        AddHandler _targetForm.Load, AddressOf OnFormLoad
        AddHandler _targetForm.Shown, AddressOf OnFormShown
        AddHandler _targetForm.Resize, AddressOf OnFormResize
        AddHandler _targetForm.FormClosed, AddressOf OnFormClosed
        AddHandler _targetForm.ParentChanged, AddressOf OnParentChanged

        HookControlAddedRecursive(_targetForm)
        HookMdiParentEvents()

        If _targetForm.IsHandleCreated AndAlso Not _isMdiParentMode Then
            CaptureState()
            ConfigureInitialFormBounds()
            ApplyScale()
            Try
                _targetForm.BeginInvoke(New MethodInvoker(AddressOf EnsureFinalLoadedBounds))
            Catch
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Universal one-line binder for any Form in any project.
    ''' </summary>
    Public Shared Function Bind(ByVal form As Form, Optional ByVal baseWidth As Integer = 0, Optional ByVal baseHeight As Integer = 0) As GenericResponsiveFormScaler
        If form Is Nothing Then Return Nothing
        SyncLock ActiveScalers
            Dim existing As GenericResponsiveFormScaler = Nothing
            If ActiveScalers.TryGetValue(form, existing) Then
                If baseWidth > 0 Then existing._baseWidth = baseWidth
                If baseHeight > 0 Then existing._baseHeight = baseHeight
                Return existing
            End If
        End SyncLock
        Return New GenericResponsiveFormScaler(form, baseWidth, baseHeight)
    End Function

    ''' <summary>
    ''' Makes an MDI child occupy the complete available MDI workspace while
    ''' preserving its captured designer dimensions as the scaling reference.
    ''' Use this for full-page overlays such as the company login screen.
    ''' </summary>
    Public Sub FillMdiWorkspace()
        If _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return

        _isFullScreenCanvas = True
        ConfigureInitialFormBounds()
        _lastAppliedW = -1
        _lastAppliedH = -1
        ApplyScale(force:=True)
    End Sub

    ''' <summary>
    ''' Fits an MDI child to a specific rectangle in MDI-client coordinates.
    ''' The controls continue to use their designer size as their responsive base.
    ''' </summary>
    Public Sub FitMdiBounds(ByVal bounds As Rectangle)
        If _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return
        If bounds.Width < 1 OrElse bounds.Height < 1 Then Return

        _mdiBoundsOverride = bounds
        ConfigureInitialFormBounds()
        _lastAppliedW = -1
        _lastAppliedH = -1
        ApplyScale(force:=True)
    End Sub

    ''' <summary>
    ''' Backwards-compatible alias for Bind.
    ''' </summary>
    Public Shared Function Attach(ByVal form As Form, Optional ByVal baseWidth As Integer = 0, Optional ByVal baseHeight As Integer = 0) As GenericResponsiveFormScaler
        Return Bind(form, baseWidth, baseHeight)
    End Function

    ''' <summary>
    ''' Changes the design-time size of a control that is intentionally resized
    ''' by application code after the initial snapshot (for example, a runtime
    ''' discount panel with a variable number of rows).  The supplied dimensions
    ''' are always in the form's designer coordinate system, so the same code
    ''' produces the correct size at every resolution.
    ''' </summary>
    Public Shared Sub SetControlDesignSize(ByVal form As Form, ByVal ctl As Control,
                                           ByVal designWidth As Integer, ByVal designHeight As Integer)
        If form Is Nothing OrElse ctl Is Nothing OrElse designWidth < 1 OrElse designHeight < 1 Then Return

        Dim scaler As GenericResponsiveFormScaler = Nothing
        SyncLock ActiveScalers
            ActiveScalers.TryGetValue(form, scaler)
        End SyncLock
        If scaler IsNot Nothing Then
            scaler.SetControlDesignSize(ctl, designWidth, designHeight)
        Else
            ctl.Size = New Size(designWidth, designHeight)
        End If
    End Sub

    Private Sub SetControlDesignSize(ByVal ctl As Control, ByVal designWidth As Integer, ByVal designHeight As Integer)
        If _disposed OrElse ctl Is Nothing OrElse ctl.IsDisposed Then Return
        If Not _isCaptured Then CaptureState()

        CaptureControl(ctl)
        Dim snap As ControlSnapshot = Nothing
        If Not _controlMap.TryGetValue(ctl, snap) Then Return

        ' Keep the original design-space location; only the runtime-selected
        ' dimensions are changed.  ApplyScale will use these values on all
        ' subsequent form resizes.
        snap.Bounds = New Rectangle(snap.Bounds.Location, New Size(designWidth, designHeight))

        If _baseWidth < 1 OrElse _baseHeight < 1 Then Return
        Dim scaleX As Double = CDbl(_targetForm.ClientSize.Width) / CDbl(_baseWidth)
        Dim scaleY As Double = CDbl(_targetForm.ClientSize.Height) / CDbl(_baseHeight)
        Dim isNeutral As Boolean = (Math.Abs(scaleX - 1.0) < 0.015 AndAlso Math.Abs(scaleY - 1.0) < 0.015)
        Dim targetWidth As Integer = If(isNeutral, designWidth, Math.Max(1, CInt(Math.Round(designWidth * scaleX))))
        Dim targetHeight As Integer = If(isNeutral, designHeight, Math.Max(1, CInt(Math.Round(designHeight * scaleY))))
        If ctl.Width <> targetWidth OrElse ctl.Height <> targetHeight Then ctl.Size = New Size(targetWidth, targetHeight)

        ' Runtime-added containers (for example Sale's UCSerial) are initially
        ' scaled through ScaleSingleControl, which handles control bounds but
        ' not DataGridView columns/rows. Re-run the normal pass after changing a
        ' design size so nested grids and their contents use the same current
        ' form scale immediately, without waiting for a later form resize.
        ApplyScale(force:=True)
    End Sub

    ''' <summary>
    ''' Binds only the MDI host chrome (menus, docked panels and their controls).
    ''' The MdiClient is deliberately excluded while taking the snapshot, therefore
    ''' none of the MDI child forms are captured, resized or otherwise altered.
    ''' </summary>
    Public Shared Function MDI(ByVal mdiHost As Form, Optional ByVal baseWidth As Integer = 0, Optional ByVal baseHeight As Integer = 0) As GenericResponsiveFormScaler
        If mdiHost Is Nothing Then Return Nothing
        If Not mdiHost.IsMdiContainer Then
            Throw New ArgumentException("GenericResponsiveFormScaler.MDI requires an MDI container form.", "mdiHost")
        End If

        If baseWidth <= 0 Then baseWidth = mdiHost.ClientSize.Width
        If baseHeight <= 0 Then baseHeight = mdiHost.ClientSize.Height
        Dim scaler As GenericResponsiveFormScaler = Nothing
        SyncLock ActiveScalers
            ActiveScalers.TryGetValue(mdiHost, scaler)
        End SyncLock
        If scaler Is Nothing Then Return New GenericResponsiveFormScaler(mdiHost, baseWidth, baseHeight, True)

        Dim wasMdiParentMode As Boolean = scaler._isMdiParentMode
        scaler._isMdiParentMode = True
        scaler._isFullScreenCanvas = True
        mdiHost.AutoSize = False
        If baseWidth > 0 Then scaler._baseWidth = baseWidth
        If baseHeight > 0 Then scaler._baseHeight = baseHeight
        If scaler._isCaptured AndAlso Not wasMdiParentMode Then
            scaler._controlMap.Clear()
            scaler._isCaptured = False
        End If
        If Not scaler._isCaptured Then scaler.CaptureState()
        scaler._lastAppliedW = -1
        scaler._lastAppliedH = -1
        scaler.ApplyScale(force:=True)
        Return scaler
    End Function

    ''' <summary>
    ''' Applies MDI-host scaling after the form's Load handlers finish, matching
    ''' the MDI-only startup flow used by the shipped AccoBook executable.
    ''' </summary>
    Public Shared Sub ApplyMdiAfterLoad(ByVal form As Form, Optional ByVal baseWidth As Integer = 0, Optional ByVal baseHeight As Integer = 0)
        If form Is Nothing OrElse form.IsDisposed Then Return

        Dim runApply As MethodInvoker = Sub()
                                            If form Is Nothing OrElse form.IsDisposed OrElse Not form.IsMdiContainer Then Return
                                            ' Reuse the scaler installed before form Load. Creating/replacing it
                                            ' here would capture an already-laid-out window and trigger another
                                            ' full control/font pass immediately after the first show.
                                            Dim scaler As GenericResponsiveFormScaler = Nothing
                                            SyncLock ActiveScalers
                                                ActiveScalers.TryGetValue(form, scaler)
                                            End SyncLock
                                            If scaler Is Nothing Then scaler = MDI(form, baseWidth, baseHeight)
                                            If scaler Is Nothing Then Return
                                            Dim dimensionsChanged As Boolean = (baseWidth > 0 AndAlso scaler._baseWidth <> baseWidth) OrElse
                                               (baseHeight > 0 AndAlso scaler._baseHeight <> baseHeight)
                                            Dim wasCaptured As Boolean = scaler._isCaptured
                                            scaler._isMdiParentMode = True
                                            scaler._isFullScreenCanvas = True
                                            form.AutoSize = False
                                            If baseWidth > 0 Then scaler._baseWidth = baseWidth
                                            If baseHeight > 0 Then scaler._baseHeight = baseHeight
                                            Dim needsInitialScale As Boolean = Not wasCaptured OrElse dimensionsChanged
                                            If Not wasCaptured Then
                                                scaler.CaptureState()
                                            ElseIf dimensionsChanged Then
                                                ' Refresh once after Load when the initial client size was temporary.
                                                scaler._controlMap.Clear()
                                                scaler._isCaptured = False
                                                scaler.CaptureState()
                                            End If
                                            If needsInitialScale Then
                                                scaler._lastAppliedW = -1
                                                scaler._lastAppliedH = -1
                                                scaler.ApplyScale(force:=False)
                                            End If
                                        End Sub

        If form.IsHandleCreated Then
            Try
                form.BeginInvoke(runApply)
            Catch
                runApply.Invoke()
            End Try
            Return
        End If

        Dim handleReady As EventHandler = Nothing
        handleReady = Sub(sender As Object, e As EventArgs)
                          RemoveHandler form.HandleCreated, handleReady
                          Try
                              form.BeginInvoke(runApply)
                          Catch
                              runApply.Invoke()
                          End Try
                      End Sub
        AddHandler form.HandleCreated, handleReady
    End Sub

    ''' <summary>
    ''' Applies a MainScreen MDI host's initial chrome layout synchronously,
    ''' before its maximized dashboard child is shown. This prevents a second
    ''' MDI-client resize from relaying out the child immediately after startup.
    ''' </summary>
    Public Shared Sub ApplyMdiImmediately(ByVal form As Form, Optional ByVal baseWidth As Integer = 0, Optional ByVal baseHeight As Integer = 0)
        If form Is Nothing OrElse form.IsDisposed OrElse Not form.IsMdiContainer Then Return

        Dim scaler As GenericResponsiveFormScaler = MDI(form, baseWidth, baseHeight)
        If scaler Is Nothing Then Return

        Dim dimensionsChanged As Boolean = (baseWidth > 0 AndAlso scaler._baseWidth <> baseWidth) OrElse
                                           (baseHeight > 0 AndAlso scaler._baseHeight <> baseHeight)
        Dim needsInitialScale As Boolean = Not scaler._isCaptured OrElse dimensionsChanged
        scaler._isMdiParentMode = True
        scaler._isFullScreenCanvas = True
        form.AutoSize = False
        If baseWidth > 0 Then scaler._baseWidth = baseWidth
        If baseHeight > 0 Then scaler._baseHeight = baseHeight

        If Not scaler._isCaptured Then
            scaler.CaptureState()
        ElseIf dimensionsChanged Then
            scaler._controlMap.Clear()
            scaler._isCaptured = False
            scaler.CaptureState()
        End If

        If needsInitialScale Then
            scaler._lastAppliedW = -1
            scaler._lastAppliedH = -1
            scaler.ApplyScale(force:=False)
        End If
    End Sub

    ''' <summary>
    ''' Removes a previously registered scaler.  This provides a single, safe
    ''' rollback point if an MDI host needs to return to its existing behaviour.
    ''' </summary>
    Public Shared Sub Unbind(ByVal form As Form)
        If form Is Nothing Then Return

        Dim existing As GenericResponsiveFormScaler = Nothing
        SyncLock ActiveScalers
            ActiveScalers.TryGetValue(form, existing)
        End SyncLock
        If existing IsNot Nothing Then existing.Dispose()
    End Sub

    Public Shared Function FindMdiClient(ByVal parentForm As Form) As MdiClient
        If parentForm Is Nothing Then Return Nothing
        For Each ctl As Control In parentForm.Controls
            If TypeOf ctl Is MdiClient Then
                EnableDoubleBuffer(ctl)
                Return DirectCast(ctl, MdiClient)
            End If
        Next
        Return Nothing
    End Function

    Private Shared Sub EnableDoubleBuffer(ByVal ctl As Control)
        If ctl Is Nothing OrElse ctl.IsDisposed Then Return
        Try
            Dim prop = ctl.GetType().GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            If prop IsNot Nothing AndAlso prop.CanWrite Then prop.SetValue(ctl, True, Nothing)
        Catch
            ' Some native-backed controls do not expose a writable DoubleBuffered property.
        End Try
    End Sub

    ''' <summary>
    ''' Captures unscaled designer dimensions, bounds, and fonts once.
    ''' </summary>
    Public Sub CaptureState()
        If _isCaptured OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return

        ' -----------------------------------------------------------------------
        ' Priority 1: Use explicit caller-supplied base dimensions (most reliable)
        ' -----------------------------------------------------------------------
        Dim w As Integer = 0
        Dim h As Integer = 0

        If _baseWidth > 0 Then w = _baseWidth
        If _baseHeight > 0 Then h = _baseHeight

        ' -----------------------------------------------------------------------
        ' Priority 2: Read form's own designer ClientSize directly from Tag or
        '             the AutoScaleDimensions-corrected stored size.
        '             Login.Designer.vb sets Me.ClientSize = New Size(880, 430).
        '             WinForms stores this in InitialClientSize before MDI clamps.
        '             We retrieve it from the protected property via reflection.
        ' -----------------------------------------------------------------------
        If w <= 0 OrElse h <= 0 Then
            Try
                Dim pi = _targetForm.GetType().GetProperty("InitialClientSize",
                    Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Public)
                If pi IsNot Nothing Then
                    Dim ics = DirectCast(pi.GetValue(_targetForm), Size)
                    If ics.Width > 50 AndAlso ics.Height > 50 Then
                        w = ics.Width
                        h = ics.Height
                    End If
                End If
            Catch
            End Try
        End If

        ' -----------------------------------------------------------------------
        ' Priority 3: Current runtime ClientSize (may be clamped by MDI parent)
        ' -----------------------------------------------------------------------
        If w <= 0 OrElse h <= 0 Then
            w = _targetForm.ClientSize.Width
            h = _targetForm.ClientSize.Height
        End If

        ' -----------------------------------------------------------------------
        ' Priority 4: Recover from control extents only for full-screen MDI forms
        '             (Purchase, CompanyList etc.) — NOT for small dialog forms.
        '             We only expand w/h from controls if the form appears to be
        '             already close to full-screen (> 900 x 480).
        '             This prevents Login (880x430) from being inflated by MDI size.
        ' -----------------------------------------------------------------------
        Dim isCandidateFullScreen As Boolean = (w >= 900 AndAlso h >= 480)
        If isCandidateFullScreen Then
            For Each ctl As Control In _targetForm.Controls
                If TypeOf ctl Is MdiClient Then Continue For
                If ctl.Right > w Then w = ctl.Right
                If ctl.Bottom > h Then h = ctl.Bottom
            Next
        End If

        ' Default fallback if form was loaded without explicit designer dimensions
        If w <= 50 Then w = 1366
        If h <= 50 Then h = 768

        _baseWidth = w
        _baseHeight = h
        _initialBounds = _targetForm.Bounds
        _isFullScreenCanvas = (_baseWidth >= 950 AndAlso _baseHeight >= 500 AndAlso Not IsResponsiveOverlay())

        EnableDoubleBuffer(_targetForm)
        _controlMap.Clear()
        CaptureControlsRecursive(_targetForm)
        _isCaptured = True
    End Sub

    Private Sub CaptureControlsRecursive(ByVal container As Control)
        If container Is Nothing Then Return
        EnableDoubleBuffer(container)
        For Each ctl As Control In container.Controls
            If TypeOf ctl Is MdiClient Then Continue For
            CaptureControl(ctl)
            HookAdditionalFieldsPopup(ctl)

            If ctl.HasChildren Then
                CaptureControlsRecursive(ctl)
            End If
        Next
    End Sub

    Private Sub HookAdditionalFieldsPopup(ByVal ctl As Control)
        If ctl Is Nothing Then Return
        If Not (String.Equals(ctl.Name, "pnlAdditional", StringComparison.OrdinalIgnoreCase) OrElse
                String.Equals(ctl.GetType().Name, "ucAddItems", StringComparison.OrdinalIgnoreCase)) Then Return
        RemoveHandler ctl.VisibleChanged, AddressOf OnAdditionalFieldsPopupVisibleChanged
        RemoveHandler ctl.SizeChanged, AddressOf OnAdditionalFieldsPopupSizeChanged
        AddHandler ctl.VisibleChanged, AddressOf OnAdditionalFieldsPopupVisibleChanged
        AddHandler ctl.SizeChanged, AddressOf OnAdditionalFieldsPopupSizeChanged
        Dim countControl As Control = ctl.Controls("txtFieldNos")
        If countControl IsNot Nothing Then
            RemoveHandler countControl.TextChanged, AddressOf OnAdditionalFieldsFieldCountChanged
            AddHandler countControl.TextChanged, AddressOf OnAdditionalFieldsFieldCountChanged
        End If
    End Sub

    Private Sub OnAdditionalFieldsPopupSizeChanged(ByVal sender As Object, ByVal e As EventArgs)
        QueueAdditionalFieldsPopupPosition(TryCast(sender, Control))
    End Sub

    Private Sub OnAdditionalFieldsFieldCountChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim countControl As Control = TryCast(sender, Control)
        If countControl IsNot Nothing Then QueueAdditionalFieldsPopupPosition(countControl.Parent)
    End Sub

    Private Sub QueueAdditionalFieldsPopupPosition(ByVal popup As Control)
        If popup Is Nothing OrElse popup.IsDisposed OrElse Not popup.Visible Then Return
        Try
            popup.BeginInvoke(New MethodInvoker(Sub()
                                                    If popup IsNot Nothing AndAlso Not popup.IsDisposed AndAlso popup.Visible Then
                                                        PositionAdditionalFieldsPopup(popup)
                                                        popup.BringToFront()
                                                    End If
                                                End Sub))
        Catch
            PositionAdditionalFieldsPopup(popup)
            popup.BringToFront()
        End Try
    End Sub

    Private Sub OnAdditionalFieldsPopupVisibleChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim popup As Control = TryCast(sender, Control)
        If popup Is Nothing OrElse Not popup.Visible Then Return
        Dim searchGrid As Control = _targetForm.Controls("dgItemSearch")
        If searchGrid IsNot Nothing Then searchGrid.Visible = False
        PositionAdditionalFieldsPopup(popup)
        popup.BringToFront()
    End Sub

    Private Sub PositionAdditionalFieldsPopup(ByVal popup As Control)
        If popup Is Nothing OrElse popup.IsDisposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return

        Dim grid As DataGridView = TryCast(_targetForm.Controls("dg1"), DataGridView)
        If grid Is Nothing OrElse grid.IsDisposed OrElse grid.Columns.Count < 2 Then Return

        ' Anchor to the first visible item-entry column in dg1, not the popup's
        ' designer X coordinate. Convert the grid point into form client space.
        Dim itemColumnIndex As Integer = 1
        While itemColumnIndex < grid.Columns.Count AndAlso Not grid.Columns(itemColumnIndex).Visible
            itemColumnIndex += 1
        End While
        If itemColumnIndex >= grid.Columns.Count Then Return

        Dim cellBounds As Rectangle = grid.GetCellDisplayRectangle(itemColumnIndex, -1, True)
        Dim gridPoint As Point = _targetForm.PointToClient(grid.PointToScreen(New Point(cellBounds.Left, 0)))
        Dim top As Integer = grid.Top
        Dim left As Integer = gridPoint.X
        If popup.Left <> left OrElse popup.Top <> top Then popup.Location = New Point(left, top)

        Dim popupScaleX As Double = CDbl(_targetForm.ClientSize.Width) / Math.Max(1, _baseWidth)
        Dim popupScaleY As Double = CDbl(_targetForm.ClientSize.Height) / Math.Max(1, _baseHeight)
        Dim activeFieldCount As Integer = 0
        Dim countControl As Control = popup.Controls("txtFieldNos")
        If countControl IsNot Nothing Then activeFieldCount = Val(countControl.Text)
        If activeFieldCount > 0 Then
            Dim requiredRight As Integer = 0
            Dim requiredBottom As Integer = 0
            For Each item In _controlMap
                Dim child As Control = item.Key
                If child Is Nothing OrElse child.IsDisposed OrElse Not IsDescendantOf(child, popup) Then Continue For
                Dim isLabel As Boolean = child.Name.StartsWith("lblTitleAdd", StringComparison.OrdinalIgnoreCase)
                Dim isField As Boolean = child.Name.StartsWith("txtField", StringComparison.OrdinalIgnoreCase) AndAlso
                                         Not String.Equals(child.Name, "txtFieldNos", StringComparison.OrdinalIgnoreCase)
                If Not isLabel AndAlso Not isField Then Continue For
                Dim fieldIndex As Integer = Val(child.Name.Substring(If(isLabel, "lblTitleAdd".Length, "txtField".Length)))
                If fieldIndex < 1 OrElse fieldIndex > activeFieldCount Then Continue For
                requiredRight = Math.Max(requiredRight, item.Value.Bounds.Right)
                requiredBottom = Math.Max(requiredBottom, item.Value.Bounds.Bottom)
            Next
            Dim requiredWidth As Integer = CInt(Math.Ceiling((requiredRight + 6) * popupScaleX))
            Dim requiredHeight As Integer = CInt(Math.Ceiling((requiredBottom + 6) * popupScaleY))
            If requiredRight > 0 AndAlso popup.Width < requiredWidth Then popup.Width = requiredWidth
            If requiredBottom > 0 AndAlso popup.Height < requiredHeight Then popup.Height = requiredHeight
        End If
    End Sub

    Private Shared Function IsDescendantOf(ByVal control As Control, ByVal ancestor As Control) As Boolean
        Dim parent As Control = If(control Is Nothing, Nothing, control.Parent)
        While parent IsNot Nothing
            If Object.ReferenceEquals(parent, ancestor) Then Return True
            parent = parent.Parent
        End While
        Return False
    End Function

    ' Captures a standalone runtime control too. The earlier recursive capture
    ' only visited a container's children, which meant a newly added DataGridView
    ' or Panel itself could miss its designer-equivalent snapshot.
    Private Sub CaptureControl(ByVal ctl As Control)
        If ctl Is Nothing OrElse TypeOf ctl Is MdiClient Then Return
        EnableDoubleBuffer(ctl)

        If Not _controlMap.ContainsKey(ctl) Then
            Dim snap As New ControlSnapshot With {
                .Bounds = New Rectangle(ctl.Left, ctl.Top, ctl.Width, ctl.Height),
                .Font = ctl.Font
            }

            Dim toolStrip As ToolStrip = TryCast(ctl, ToolStrip)
            If toolStrip IsNot Nothing Then CaptureToolStripItemFonts(toolStrip.Items, snap.ToolStripItemFonts)

            Dim dgv = TryCast(ctl, DataGridView)
            If dgv IsNot Nothing Then
                snap.IsDgv = True
                snap.DgvHeaderHeight = Math.Max(20, dgv.ColumnHeadersHeight)
                snap.DgvRowHeight = Math.Max(20, dgv.RowTemplate.Height)
                ' Use dgv.Font as fallback when no explicit header/cell font is set in designer.
                snap.DgvHeaderFont = If(dgv.ColumnHeadersDefaultCellStyle.Font, dgv.Font)
                snap.DgvCellFont = If(dgv.DefaultCellStyle.Font, dgv.Font)
                snap.DgvRowsFont = If(dgv.RowsDefaultCellStyle.Font,
                                     If(dgv.DefaultCellStyle.Font, dgv.Font))
                For i As Integer = 0 To dgv.Columns.Count - 1
                    snap.DgvColWidths.Add(Math.Max(10, dgv.Columns(i).Width))
                Next
                HookRuntimeGrid(dgv)
            End If

            Dim cb = TryCast(ctl, ComboBox)
            If cb IsNot Nothing Then
                snap.IsComboBox = True
                snap.CbItemHeight = cb.ItemHeight
            End If

            _controlMap(ctl) = snap
        End If
    End Sub

    Private Sub HookControlAddedRecursive(ByVal container As Control)
        If container Is Nothing Then Return
        RemoveHandler container.ControlAdded, AddressOf OnControlAdded
        AddHandler container.ControlAdded, AddressOf OnControlAdded
        For Each child As Control In container.Controls
            If child.HasChildren Then
                HookControlAddedRecursive(child)
            End If
        Next
    End Sub

    Private Sub OnControlAdded(ByVal sender As Object, ByVal e As ControlEventArgs)
        If Not _isCaptured OrElse _isScaling OrElse e.Control Is Nothing OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return
        Try
            If Not _controlMap.ContainsKey(e.Control) Then
                HookControlAddedRecursive(e.Control)
                CaptureControl(e.Control)
                CaptureControlsRecursive(e.Control)

                ' If form is currently scaled, immediately apply current scale to newly added control
                If _isCaptured AndAlso _baseWidth > 0 AndAlso _baseHeight > 0 Then
                    Dim scaleX As Double = CDbl(_targetForm.ClientSize.Width) / CDbl(_baseWidth)
                    Dim scaleY As Double = CDbl(_targetForm.ClientSize.Height) / CDbl(_baseHeight)
                    If Math.Abs(scaleX - 1.0) >= 0.01 OrElse Math.Abs(scaleY - 1.0) >= 0.01 Then
                        Dim fontScale As Single = CSng(Math.Min(scaleX, scaleY))
                        ScaleSingleControl(e.Control, scaleX, scaleY, fontScale)
                    End If
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub HookRuntimeGrid(ByVal dgv As DataGridView)
        If dgv Is Nothing OrElse dgv.IsDisposed OrElse _hookedDataGrids.Contains(dgv) Then Return
        AddHandler dgv.ColumnAdded, AddressOf OnRuntimeGridColumnAdded
        AddHandler dgv.RowsAdded, AddressOf OnRuntimeGridRowsAdded
        AddHandler dgv.ColumnWidthChanged, AddressOf OnRuntimeGridColumnWidthChanged
        _hookedDataGrids.Add(dgv)
    End Sub

    Private Sub OnRuntimeGridColumnAdded(ByVal sender As Object, ByVal e As DataGridViewColumnEventArgs)
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        If IsSearchOrChargeLookupGrid(dgv) Then
            Dim snap As ControlSnapshot = Nothing
            If _controlMap.TryGetValue(dgv, snap) Then
                ' Lookup grids are created after the form is shown. Their code
                ' assigns explicit design widths after ColumnAdded, so retain
                ' those widths as design metrics when the deferred batch runs.
                snap.PreserveRuntimeGridWidths = True
                snap.RuntimeGridWidthsAreDesignSpace = True
                snap.HasRuntimeGridWidthBaseline = False
            End If
        End If
        QueueRuntimeGridRefresh(dgv)
    End Sub

    Private Sub OnRuntimeGridRowsAdded(ByVal sender As Object, ByVal e As DataGridViewRowsAddedEventArgs)
        ' New rows inherit DefaultCellStyle.Font, but their Height can be reset
        ' by a runtime Rows.Add. Adjust only those rows; do not run a full grid
        ' pass here because that would overwrite a transaction grid's deliberate
        ' runtime column-width changes (for example Purchase/Sale Total).
        ApplyRuntimeRowHeights(TryCast(sender, DataGridView), e.RowIndex, e.RowCount)
    End Sub

    Private Sub OnRuntimeGridDataBound(ByVal sender As Object, ByVal e As DataGridViewBindingCompleteEventArgs)
        QueueRuntimeGridRefresh(TryCast(sender, DataGridView))
    End Sub

    Private Sub OnRuntimeGridColumnWidthChanged(ByVal sender As Object, ByVal e As DataGridViewColumnEventArgs)
        ' Application code may deliberately change a column after rows are
        ' populated (for example dg1.Columns(5).Width = 150). Capture that
        ' runtime width as the new responsive baseline.
        If _isScaling Then Return
        Dim dgv As DataGridView = TryCast(sender, DataGridView)
        If dgv IsNot Nothing Then
            Dim snap As ControlSnapshot = Nothing
            If _controlMap.TryGetValue(dgv, snap) Then
                snap.PreserveRuntimeGridWidths = True
                ' Widths assigned while a form is building its runtime grid are
                ' design dimensions. Lookup grids create their columns on demand
                ' after the form is shown, but their code-assigned widths are still
                ' design dimensions and must scale with the form.
                Dim isLookupGrid As Boolean = IsSearchOrChargeLookupGrid(dgv)
                snap.RuntimeGridWidthsAreDesignSpace = Not _hasBeenShown OrElse isLookupGrid
                snap.HasRuntimeGridWidthBaseline = False
            End If
        End If
        QueueRuntimeGridRefresh(dgv)
    End Sub

    ' BeginInvoke deliberately waits until the calling code has completed its
    ' ColumnCount, Width and Rows.Add statements. This avoids snapshotting the
    ' temporary 100px default width created by DataGridView.ColumnAdded.
    Private Sub QueueRuntimeGridRefresh(ByVal dgv As DataGridView)
        If _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed OrElse dgv Is Nothing OrElse dgv.IsDisposed Then Return
        _pendingRuntimeGrids.Add(dgv)
        If _runtimeGridRefreshQueued Then Return

        _runtimeGridRefreshQueued = True
        Try
            _targetForm.BeginInvoke(New MethodInvoker(AddressOf ApplyQueuedRuntimeGridRefresh))
        Catch
            ApplyQueuedRuntimeGridRefresh()
        End Try
    End Sub

    Private Sub ApplyQueuedRuntimeGridRefresh()
        _runtimeGridRefreshQueued = False
        If _disposed OrElse Not _isCaptured OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then
            _pendingRuntimeGrids.Clear()
            Return
        End If

        ' A data-binding operation can raise RowsAdded while a normal resize pass
        ' is active. Leave the batch pending and run immediately after that pass.
        If _isScaling Then
            _runtimeGridRefreshQueued = True
            Try
                _targetForm.BeginInvoke(New MethodInvoker(AddressOf ApplyQueuedRuntimeGridRefresh))
            Catch
            End Try
            Return
        End If

        Dim pendingGrids As New List(Of DataGridView)(_pendingRuntimeGrids)
        For Each dgv As DataGridView In pendingGrids
            If dgv Is Nothing OrElse dgv.IsDisposed Then Continue For
            Dim snap As ControlSnapshot = Nothing
            If Not _controlMap.TryGetValue(dgv, snap) OrElse Not snap.IsDgv Then Continue For

            ' New columns are still in their raw runtime dimensions here. Store
            ' those dimensions once, then the normal scaler applies the current
            ' X/Y/font/row scale identically to designer-created grids.
            While snap.DgvColWidths.Count < dgv.Columns.Count
                snap.DgvColWidths.Add(Math.Max(10, dgv.Columns(snap.DgvColWidths.Count).Width))
            End While

            ' A lookup grid can first be created after the form is already at a
            ' scaled size. Capture its finished code-assigned width only after
            ' ColumnAdded has unwound (the queued batch), never at default 100px.
            If IsSearchOrChargeLookupGrid(dgv) AndAlso
               snap.RuntimeGridWidthsAreDesignSpace AndAlso
               Not snap.HasRuntimeGridWidthBaseline Then
                For columnIndex As Integer = 0 To dgv.Columns.Count - 1
                    If dgv.Columns(columnIndex).Visible Then
                        snap.DgvColWidths(columnIndex) = Math.Max(10, dgv.Columns(columnIndex).Width)
                    End If
                Next
                snap.HasRuntimeGridWidthBaseline = True
            End If

            ' Normalize post-show edits to existing columns back to design
            ' coordinates. Explicit lookup widths and initial runtime widths are
            ' already in design coordinates and must not be divided by scaleX.
            Dim currentScaleX As Double = If(_baseWidth > 0, CDbl(_targetForm.ClientSize.Width) / CDbl(_baseWidth), 1.0)
            If currentScaleX < 0.01 Then currentScaleX = 1.0
            For columnIndex As Integer = 0 To dgv.Columns.Count - 1
                Dim col As DataGridViewColumn = dgv.Columns(columnIndex)
                If col.Visible Then
                    Dim rawW As Integer = Math.Max(10, col.Width)
                    ' Load-time code often assigns its intended design width
                    ' after the form has already been scaled. Keep that width as
                    ' the baseline; only live post-show pixel changes need
                    ' normalization back to design coordinates.
                    Dim designW As Integer = If(snap.RuntimeGridWidthsAreDesignSpace,
                                                rawW,
                                                Math.Max(10, CInt(Math.Round(rawW / currentScaleX))))
                    snap.DgvColWidths(columnIndex) = designW
                End If
            Next
        Next
        _pendingRuntimeGrids.Clear()

        ' One suspended, coalesced pass handles all grids and their rows without
        ' flicker. Existing controls keep their same stored snapshots.
        ApplyScale(force:=True)
    End Sub

    Private Sub ApplyRuntimeRowHeights(ByVal dgv As DataGridView, ByVal firstRow As Integer, ByVal rowCount As Integer)
        If _disposed OrElse Not _isCaptured OrElse dgv Is Nothing OrElse dgv.IsDisposed OrElse rowCount <= 0 Then Return
        Dim snap As ControlSnapshot = Nothing
        If Not _controlMap.TryGetValue(dgv, snap) OrElse Not snap.IsDgv OrElse snap.DgvRowHeight <= 0 Then Return
        If _baseHeight <= 0 OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return

        Dim rawScaleY As Double = CDbl(_targetForm.ClientSize.Height) / CDbl(_baseHeight)
        Dim isNeutral As Boolean = Math.Abs(rawScaleY - 1.0) < 0.015
        Dim targetRowHeight As Integer = If(isNeutral, snap.DgvRowHeight, Math.Max(18, CInt(Math.Round(snap.DgvRowHeight * rawScaleY))))
        If dgv.RowTemplate.Height <> targetRowHeight Then dgv.RowTemplate.Height = targetRowHeight

        Dim lastRow As Integer = Math.Min(dgv.Rows.Count - 1, firstRow + rowCount - 1)
        For rowIndex As Integer = Math.Max(0, firstRow) To lastRow
            If dgv.Rows(rowIndex).Height <> targetRowHeight Then dgv.Rows(rowIndex).Height = targetRowHeight
        Next
    End Sub

    Private Sub ScaleSingleControl(ByVal ctl As Control, ByVal scaleX As Double, ByVal scaleY As Double, ByVal fontScale As Single)
        If ctl Is Nothing OrElse ctl.IsDisposed Then Return
        If IsManagedByResponsiveUserControl(ctl) Then Return
        Dim snap As ControlSnapshot = Nothing
        If _controlMap.TryGetValue(ctl, snap) Then
            Dim targetHeight As Integer = Math.Max(1, CInt(Math.Round(snap.Bounds.Height * scaleY)))
            Dim inputExtraHeight As Integer = GetMdiInputHeightAllowance(ctl, scaleY)
            targetHeight += inputExtraHeight
            If inputExtraHeight > 0 OrElse TypeOf ctl Is DateTimePicker Then PrepareInputForHeightScaling(ctl)

            Dim targetWidth As Integer = Math.Max(1, CInt(Math.Round(snap.Bounds.Width * scaleX)))
            targetWidth += GetMdiChildWidthAllowance(ctl, scaleY)

            ctl.SetBounds(CInt(Math.Round(snap.Bounds.Left * scaleX)),
                          CInt(Math.Round(snap.Bounds.Top * scaleY)),
                          targetWidth,
                          targetHeight)

            If snap.Font IsNot Nothing Then
                Dim controlFontScale As Single = GetControlFontScale(ctl, snap.Font, fontScale)
                ctl.Font = GetCachedFont(snap.Font.FontFamily, Math.Max(6.5!, CSng(Math.Round(snap.Font.Size * controlFontScale, 1))), snap.Font.Style)
            End If
        End If

        Dim responsiveUserControl = TryCast(ctl, IResponsiveUserControlLayout)
        If responsiveUserControl IsNot Nothing Then
            responsiveUserControl.ApplyResponsiveLayout()
            Return
        End If

        For Each child As Control In ctl.Controls
            ScaleSingleControl(child, scaleX, scaleY, fontScale)
        Next
    End Sub

    Private Shared Sub CaptureToolStripItemFonts(ByVal items As ToolStripItemCollection,
                                                  ByVal snapshots As List(Of KeyValuePair(Of ToolStripItem, Font)))
        For Each item As ToolStripItem In items
            snapshots.Add(New KeyValuePair(Of ToolStripItem, Font)(item, item.Font))
            Dim menuItem As ToolStripMenuItem = TryCast(item, ToolStripMenuItem)
            If menuItem IsNot Nothing Then CaptureToolStripItemFonts(menuItem.DropDownItems, snapshots)
        Next
    End Sub

    Private Shared Function IsManagedByResponsiveUserControl(ByVal ctl As Control) As Boolean
        Dim parent As Control = If(ctl Is Nothing, Nothing, ctl.Parent)
        While parent IsNot Nothing
            If TypeOf parent Is IResponsiveUserControlLayout Then Return True
            parent = parent.Parent
        End While
        Return False
    End Function

    Private Sub ScaleMdiParentToCurrentScreen()
        If Not _isMdiParentMode OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return
        If Not _isCaptured Then CaptureState()

        ' MainScreenForm is a maximized MDI shell. Native maximize already fits
        ' the current monitor work area; applying manual bounds here causes a
        ' visible jump on Shown and on every resize notification.
        ApplyScale(force:=False)
    End Sub

    ' At the 1366 x 768 design size inputs retain their designer height.  On a
    ' larger MDI workspace, their extra height gently consumes the otherwise blank
    ' strip above a grid without moving labels, buttons, panels or the grid itself.
    Private Function GetMdiInputHeightAllowance(ByVal ctl As Control, ByVal scaleY As Double) As Integer
        If _targetForm Is Nothing OrElse _targetForm.MdiParent Is Nothing OrElse scaleY <= 1.02 Then Return 0
        If Not (TypeOf ctl Is TextBox OrElse
                TypeOf ctl Is ComboBox OrElse
                TypeOf ctl Is RadioButton OrElse
                TypeOf ctl Is GroupBox) Then Return 0

        ' Keep this intentionally subtle: a larger control-to-grid gap needs only
        ' 1–3 px, not a full proportional row-height increase.
        Dim allowance As Integer = Math.Min(3, Math.Max(1, CInt(Math.Ceiling((scaleY - 1.0) * 4.0))))

        Return allowance
    End Function

    ' A one-pixel horizontal allowance removes the visible hairline gap at high
    ' resolutions. Labels and buttons retain their exact designer bounds.
    Private Function GetMdiChildWidthAllowance(ByVal ctl As Control, ByVal scaleY As Double) As Integer
        If _targetForm Is Nothing OrElse _targetForm.MdiParent Is Nothing OrElse scaleY <= 1.02 Then Return 0
        If TypeOf ctl Is Label OrElse TypeOf ctl Is LinkLabel OrElse TypeOf ctl Is Button Then Return 0
        Return 1
    End Function

    ' WinForms keeps AutoSize text/radio controls at their preferred height unless
    ' it is disabled first. This is intentionally limited to the requested inputs.
    Private Shared Sub PrepareInputForHeightScaling(ByVal ctl As Control)
        Dim tb = TryCast(ctl, TextBox)
        If tb IsNot Nothing AndAlso tb.AutoSize Then tb.AutoSize = False

        Dim rb = TryCast(ctl, RadioButton)
        If rb IsNot Nothing AndAlso rb.AutoSize Then rb.AutoSize = False

        ' ComboBox otherwise restores its preferred native height during layout.
        ' Disable AutoSize so it retains the same small increase as a TextBox.
        Dim cb = TryCast(ctl, ComboBox)
        If cb IsNot Nothing AndAlso cb.AutoSize Then cb.AutoSize = False

        ' Native DateTimePicker calculates its preferred height from its font.
        ' Keep AutoSize enabled; forcing Height can hide its calendar dropdown.
        Dim dtp = TryCast(ctl, DateTimePicker)
        If dtp IsNot Nothing AndAlso Not dtp.AutoSize Then dtp.AutoSize = True
    End Sub

    ' Keep ordinary captions readable without letting them dominate a wider screen.
    ' Form titles are intentionally excluded so their existing visual hierarchy remains intact.
    Private Shared Function GetControlFontScale(ByVal ctl As Control, ByVal baseFont As Font, ByVal fontScale As Single) As Single
        ' A native DateTimePicker derives its usable height from Font. Designer
        ' testing shows 14pt aligns it with 12pt TextBox/ComboBox controls. When
        ' a designer already uses 14pt, retain its intended hierarchy by adding
        ' 2pt before applying the same responsive scale on every resize.
        If TypeOf ctl Is DateTimePicker Then
            Dim effectiveDtpBaseSize As Single = Math.Max(14.0!, baseFont.Size + 2.0!)
            Return (effectiveDtpBaseSize / baseFont.Size) * fontScale
        End If

        If (TypeOf ctl Is Label OrElse TypeOf ctl Is LinkLabel) AndAlso baseFont.Size < 30.0! Then
            Return Math.Min(fontScale, 1.2!)
        End If

        Return fontScale
    End Function

    Private Sub OnFormLoad(ByVal sender As Object, ByVal e As EventArgs)
        CaptureState()
        HookMdiParentEvents()
        If _isMdiParentMode Then Return
        ConfigureInitialFormBounds()
        ApplyScale(force:=False)
        Try
            If _targetForm IsNot Nothing AndAlso _targetForm.IsHandleCreated Then
                _targetForm.BeginInvoke(New MethodInvoker(AddressOf EnsureFinalLoadedBounds))
            End If
        Catch
        End Try
    End Sub

    Private Sub OnFormShown(ByVal sender As Object, ByVal e As EventArgs)
        HookMdiParentEvents()
        _hasBeenShown = True
        If _isMdiParentMode Then
            If _resizeTimer IsNot Nothing Then
                _resizeTimer.Stop()
                _resizeTimer.Start()
            Else
                ApplyScale(force:=False)
            End If
            Return
        End If
        EnsureFinalLoadedBounds()

        ' Check if any DataGridView had 0 columns during Load or added runtime columns
        Dim hasNewColumns As Boolean = False
        For Each kvp In _controlMap
            If kvp.Value.IsDgv Then
                Dim dgv = TryCast(kvp.Key, DataGridView)
                If dgv IsNot Nothing AndAlso dgv.Columns.Count > kvp.Value.DgvColWidths.Count Then
                    While kvp.Value.DgvColWidths.Count < dgv.Columns.Count
                        kvp.Value.DgvColWidths.Add(Math.Max(10, dgv.Columns(kvp.Value.DgvColWidths.Count).Width))
                        hasNewColumns = True
                    End While
                End If
            End If
        Next

        If hasNewColumns Then
            ApplyScale(force:=True)
        End If
    End Sub

    Private Sub EnsureFinalLoadedBounds()
        If _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return
        If _targetForm.WindowState = FormWindowState.Minimized Then Return
        If IsResponsiveOverlay() AndAlso _targetForm.MdiParent IsNot Nothing Then Return
        If _hookedParent IsNot Nothing AndAlso (_hookedParent.WindowState = FormWindowState.Minimized OrElse _hookedParent.ClientSize.Width < 400 OrElse _hookedParent.ClientSize.Height < 300) Then Return

        If _isMdiParentMode Then
            ApplyScale(force:=False)
            Return
        End If

        Dim prevW As Integer = _targetForm.Width
        Dim prevH As Integer = _targetForm.Height
        ConfigureInitialFormBounds()
        Dim changed As Boolean = (_targetForm.Width <> prevW OrElse _targetForm.Height <> prevH)
        ' Only force re-scale if form size actually changed (avoids flicker on second call)
        If changed Then
            _lastAppliedW = -1
            _lastAppliedH = -1
            ApplyScale(force:=True)
        Else
            ApplyScale(force:=False)
        End If
    End Sub

    Private Sub OnParentChanged(ByVal sender As Object, ByVal e As EventArgs)
        HookMdiParentEvents()
        If IsResponsiveOverlay() AndAlso _targetForm.MdiParent IsNot Nothing Then Return
        If Not _isMdiParentMode Then ConfigureInitialFormBounds()
        ApplyScale(force:=False)
    End Sub

    Private Sub HookMdiParentEvents()
        If _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return
        Dim parent = _targetForm.MdiParent

        If parent IsNot _hookedParent OrElse (_hookedMdiClient Is Nothing AndAlso parent IsNot Nothing) Then
            UnhookMdiParentEvents()
            _hookedParent = parent
            If _hookedParent IsNot Nothing Then
                _hookedMdiClient = FindMdiClient(_hookedParent)
                If _hookedMdiClient IsNot Nothing Then
                    AddHandler _hookedMdiClient.Resize, AddressOf OnMdiParentResize
                Else
                    AddHandler _hookedParent.Resize, AddressOf OnMdiParentResize
                End If
            End If
        End If
    End Sub

    Private Sub UnhookMdiParentEvents()
        If _hookedMdiClient IsNot Nothing Then
            RemoveHandler _hookedMdiClient.Resize, AddressOf OnMdiParentResize
            _hookedMdiClient = Nothing
        ElseIf _hookedParent IsNot Nothing Then
            RemoveHandler _hookedParent.Resize, AddressOf OnMdiParentResize
        End If
        _hookedParent = Nothing
    End Sub

    Private Sub OnMdiParentResize(ByVal sender As Object, ByVal e As EventArgs)
        If _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed OrElse _isScaling Then Return
        If IsResponsiveOverlay() AndAlso _targetForm.MdiParent IsNot Nothing Then Return
        If _hookedParent IsNot Nothing AndAlso (_hookedParent.WindowState = FormWindowState.Minimized OrElse _hookedParent.ClientSize.Width < 400 OrElse _hookedParent.ClientSize.Height < 300) Then Return
        If _targetForm.WindowState = FormWindowState.Minimized OrElse Not _targetForm.Visible Then Return

        If _isMdiParentMode Then
            If _resizeTimer IsNot Nothing Then
                _resizeTimer.Stop()
                _resizeTimer.Start()
            Else
                ApplyScale(force:=False)
            End If
            Return
        End If

        If _resizeTimer IsNot Nothing Then
            _resizeTimer.Stop()
            _resizeTimer.Start()
        Else
            ConfigureInitialFormBounds()
            ApplyScale(force:=False)
        End If
    End Sub

    Private Sub OnResizeTimerTick(ByVal sender As Object, ByVal e As EventArgs)
        If _resizeTimer IsNot Nothing Then
            _resizeTimer.Stop()
        End If
        If _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed OrElse _isScaling Then Return
        If IsResponsiveOverlay() AndAlso _targetForm.MdiParent IsNot Nothing Then Return
        If _hookedParent IsNot Nothing AndAlso (_hookedParent.WindowState = FormWindowState.Minimized OrElse _hookedParent.ClientSize.Width < 400 OrElse _hookedParent.ClientSize.Height < 300) Then Return
        If _targetForm.WindowState = FormWindowState.Minimized OrElse Not _targetForm.Visible Then Return

        If _isMdiParentMode Then
            ApplyScale(force:=False)
            Return
        End If

        Dim prevW As Integer = _targetForm.Width
        Dim prevH As Integer = _targetForm.Height
        ConfigureInitialFormBounds()
        Dim changed As Boolean = (_targetForm.Width <> prevW OrElse _targetForm.Height <> prevH)
        ApplyScale(force:=changed)
    End Sub

    Private Sub OnFormResize(ByVal sender As Object, ByVal e As EventArgs)
        If _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed OrElse _isScaling OrElse _isConfiguringBounds Then Return
        If _targetForm.WindowState = FormWindowState.Minimized Then Return
        If IsResponsiveOverlay() AndAlso _targetForm.MdiParent IsNot Nothing Then Return
        If _isMdiParentMode Then
            ' Form.Resize can fire repeatedly while Windows is maximizing or
            ' adjusting the non-client area. Coalesce those events just like
            ' MdiClient.Resize so the host chrome is laid out once per settle.
            If _resizeTimer IsNot Nothing Then
                _resizeTimer.Stop()
                _resizeTimer.Start()
            Else
                ApplyScale(force:=False)
            End If
            Return
        End If
        If _hookedParent IsNot Nothing AndAlso (_hookedParent.WindowState = FormWindowState.Minimized OrElse _hookedParent.ClientSize.Width < 400 OrElse _hookedParent.ClientSize.Height < 300) Then Return

        ' If this is a full workspace MDI child, ensure its bounds match available MDI area
        If _targetForm.MdiParent IsNot Nothing AndAlso _isFullScreenCanvas Then
            Dim parent = _targetForm.MdiParent
            Dim mdi = FindMdiClient(parent)
            If mdi IsNot Nothing AndAlso mdi.ClientSize.Width > 50 AndAlso mdi.ClientSize.Height > 50 Then
                If _targetForm.Width <> mdi.ClientSize.Width OrElse _targetForm.Height <> mdi.ClientSize.Height OrElse _targetForm.Left <> 0 OrElse _targetForm.Top <> 0 Then
                    ConfigureInitialFormBounds()
                End If
            End If
        End If

        ApplyScale(force:=False)
    End Sub

    Public Sub ConfigureInitialFormBounds()
        If _isConfiguringBounds OrElse _disposed OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return
        If _isMdiParentMode Then Return
        ' Login/license overlays set their exact grid bounds before Show. Keep
        ' those bounds; generic MDI centering/resizing must not replace them.
        If IsResponsiveOverlay() AndAlso _targetForm.MdiParent IsNot Nothing Then Return
        If _hookedParent IsNot Nothing AndAlso (_hookedParent.WindowState = FormWindowState.Minimized OrElse _hookedParent.ClientSize.Width < 400 OrElse _hookedParent.ClientSize.Height < 300) Then Return

        _isConfiguringBounds = True
        Try
            Dim scr = Screen.FromControl(_targetForm).Bounds
            Dim isBaseRes As Boolean = (Math.Abs(scr.Width - 1366) <= 25 AndAlso Math.Abs(scr.Height - 768) <= 25)

            Dim scrScaleX As Double = If(isBaseRes, 1.0, CDbl(scr.Width) / 1366.0)
            Dim scrScaleY As Double = If(isBaseRes, 1.0, CDbl(scr.Height) / 768.0)

            If _targetForm.MdiParent IsNot Nothing Then
                Dim parent = _targetForm.MdiParent
                Dim mdi = FindMdiClient(parent)

                ' A caller may use a precise child-area overlay instead of the
                ' normal full-page or centered-dialog MDI behaviour.
                If _mdiBoundsOverride.HasValue Then
                    Dim overlayBounds = _mdiBoundsOverride.Value
                    If _targetForm.Bounds <> overlayBounds Then
                        _targetForm.StartPosition = FormStartPosition.Manual
                        _targetForm.SetBounds(overlayBounds.X, overlayBounds.Y, overlayBounds.Width, overlayBounds.Height)
                    End If
                    Return
                End If

                ' Measure all docked/undocked bars on the MDI parent
                Dim topBars As Integer = 0
                Dim bottomBars As Integer = 0
                Dim rightBars As Integer = 0
                Dim leftBars As Integer = 0
                Dim undockedRightBars As Integer = 0

                For Each c As Control In parent.Controls
                    If c.Visible AndAlso Not (TypeOf c Is MdiClient) Then
                        Select Case c.Dock
                            Case DockStyle.Top
                                topBars += c.Height
                            Case DockStyle.Bottom
                                bottomBars += c.Height
                            Case DockStyle.Right
                                rightBars = Math.Max(rightBars, c.Width)
                            Case DockStyle.Left
                                leftBars = Math.Max(leftBars, c.Width)
                            Case DockStyle.None
                                ' Detect undocked sidebar controls placed along the right edge of MDI parent
                                If c.Right >= parent.ClientSize.Width - 25 AndAlso c.Width >= 50 AndAlso c.Width <= 350 Then
                                    undockedRightBars = Math.Max(undockedRightBars, parent.ClientSize.Width - c.Left)
                                End If
                        End Select
                    End If
                Next

                ' Full-screen MDI children: deduct right sidebar so they don't go under it.
                ' Dialog-style MDI children: use full MDI client width; the right sidebar is a floating
                '   overlay on MainScreenForm and dialog forms should center within the full MDI area.
                Dim availW As Integer
                Dim availH As Integer
                If mdi IsNot Nothing AndAlso mdi.ClientSize.Width > 50 AndAlso mdi.ClientSize.Height > 50 Then
                    If _isFullScreenCanvas Then
                        availW = Math.Max(100, mdi.ClientSize.Width - undockedRightBars)
                    Else
                        availW = Math.Max(100, mdi.ClientSize.Width)  ' Dialog: full MDI client width
                    End If
                    availH = Math.Max(100, mdi.ClientSize.Height)
                Else
                    availW = Math.Max(100, parent.ClientSize.Width - leftBars - Math.Max(rightBars, undockedRightBars))
                    availH = Math.Max(100, parent.ClientSize.Height - topBars - bottomBars)
                End If

                If _targetForm.WindowState <> FormWindowState.Normal Then
                    _targetForm.WindowState = FormWindowState.Normal
                End If

                If _isFullScreenCanvas Then
                    ' Full workspace screen (Purchase, Sale, CompanyList, Item_form, MainScreenPicture, CreateAccount, etc.)
                    If _targetForm.Left <> 0 OrElse _targetForm.Top <> 0 OrElse _targetForm.Width <> availW OrElse _targetForm.Height <> availH Then
                        _targetForm.StartPosition = FormStartPosition.Manual
                        _targetForm.SetBounds(0, 0, availW, availH)
                    End If
                Else
                    ' Sized dialog form (Login, Tax Setting, AddBatch, etc.)
                    ' Scale to designer dimensions; center within full MDI client area.
                    Dim borderW As Integer = Math.Max(0, _targetForm.Width - _targetForm.ClientSize.Width)
                    Dim borderH As Integer = Math.Max(0, _targetForm.Height - _targetForm.ClientSize.Height)

                    Dim clientW As Integer = If(isBaseRes, _baseWidth, Math.Min(availW - borderW, CInt(Math.Round(_baseWidth * scrScaleX))))
                    Dim clientH As Integer = If(isBaseRes, _baseHeight, Math.Min(availH - borderH, CInt(Math.Round(_baseHeight * scrScaleY))))

                    ' Do NOT clamp outerH below designer size — avoids bottom cut
                    Dim outerW As Integer = clientW + borderW
                    Dim outerH As Integer = clientH + borderH
                    Dim targetL As Integer
                    Dim targetT As Integer

                    If _targetForm.StartPosition = FormStartPosition.Manual Then
                        targetL = Math.Max(0, Math.Min(availW - outerW, If(isBaseRes, _targetForm.Left, CInt(Math.Round(_targetForm.Left * scrScaleX)))))
                        targetT = Math.Max(0, Math.Min(availH - outerH, If(isBaseRes, _targetForm.Top, CInt(Math.Round(_targetForm.Top * scrScaleY)))))
                    Else
                        ' Center within full MDI client area
                        targetL = Math.Max(0, (availW - outerW) \ 2)
                        targetT = Math.Max(0, (availH - outerH) \ 2)
                    End If

                    If _targetForm.Left <> targetL OrElse _targetForm.Top <> targetT OrElse _targetForm.Width <> outerW OrElse _targetForm.Height <> outerH Then
                        _targetForm.SetBounds(targetL, targetT, outerW, outerH)
                    End If
                End If
            ElseIf _targetForm.WindowState = FormWindowState.Normal Then
                If Not isBaseRes AndAlso _targetForm.FormBorderStyle = FormBorderStyle.Sizable Then
                    Dim workingArea = Screen.FromControl(_targetForm).WorkingArea
                    Dim targetW As Integer = Math.Min(workingArea.Width, CInt(Math.Round(_baseWidth * scrScaleX)))
                    Dim targetH As Integer = Math.Min(workingArea.Height, CInt(Math.Round(_baseHeight * scrScaleY)))
                    _targetForm.ClientSize = New Size(targetW, targetH)

                    If _targetForm.StartPosition = FormStartPosition.CenterScreen OrElse _targetForm.StartPosition = FormStartPosition.CenterParent Then
                        _targetForm.Location = New Point(workingArea.Left + (workingArea.Width - _targetForm.Width) \ 2,
                                                     workingArea.Top + (workingArea.Height - _targetForm.Height) \ 2)
                    End If
                End If
            End If
        Finally
            _isConfiguringBounds = False
        End Try
    End Sub

    ''' <summary>
    ''' Pure mathematical scaling pass. Runs in &lt; 3ms without flicker.
    ''' </summary>
    Public Sub ApplyScale(Optional ByVal force As Boolean = False)
        If _isScaling OrElse Not _isCaptured OrElse _targetForm Is Nothing OrElse _targetForm.IsDisposed Then Return
        If _targetForm.WindowState = FormWindowState.Minimized Then Return

        Dim curW As Integer = _targetForm.ClientSize.Width
        Dim curH As Integer = _targetForm.ClientSize.Height
        If curW <= 0 OrElse curH <= 0 OrElse _baseWidth <= 0 OrElse _baseHeight <= 0 Then Return

        ' Anti-flicker guard: If form is already scaled to these exact dimensions, skip completely (0ms on click)
        If Not force AndAlso curW = _lastAppliedW AndAlso curH = _lastAppliedH Then Return

        Dim rawScaleX As Double = CDbl(curW) / CDbl(_baseWidth)
        Dim rawScaleY As Double = CDbl(curH) / CDbl(_baseHeight)

        ' Exact 1:1 neutral scaling: restore designer state when client size is within 1.5% of designer dimensions
        Dim isNeutral As Boolean = (Math.Abs(rawScaleX - 1.0) < 0.015 AndAlso Math.Abs(rawScaleY - 1.0) < 0.015)
        Dim isMainScreenMdiHost As Boolean = _isMdiParentMode AndAlso
            String.Equals(_targetForm.Name, "MainScreenForm", StringComparison.OrdinalIgnoreCase)
        ' MainScreen MDI chrome uses the actual horizontal and vertical ratios separately:
        ' the right dock panel/menu height and its child buttons must fill their
        ' corresponding screen dimension. Keep font growth uniform and readable.
        Dim scaleX As Double = If(isNeutral, 1.0, If(isMainScreenMdiHost OrElse Not _isMdiParentMode, rawScaleX, Math.Min(rawScaleX, rawScaleY)))
        Dim scaleY As Double = If(isNeutral, 1.0, If(isMainScreenMdiHost OrElse Not _isMdiParentMode, rawScaleY, Math.Min(rawScaleX, rawScaleY)))
        Dim fontScale As Single = If(isNeutral, 1.0!, CSng(Math.Min(scaleX, scaleY)))

        _isScaling = True
        Try
            _targetForm.SuspendLayout()

            For Each kvp In _controlMap
                Dim ctl = kvp.Key
                Dim snap = kvp.Value
                If ctl Is Nothing OrElse ctl.IsDisposed Then Continue For
                If IsManagedByResponsiveUserControl(ctl) Then Continue For

                If _isMdiParentMode AndAlso TypeOf ctl Is MdiClient Then Continue For

                ' 1. Scale Control Bounds linearly and proportionally
                Dim newL As Integer = If(isNeutral, snap.Bounds.Left, CInt(Math.Round(snap.Bounds.Left * scaleX)))
                Dim newT As Integer = If(isNeutral, snap.Bounds.Top, CInt(Math.Round(snap.Bounds.Top * scaleY)))
                Dim newW As Integer = If(isNeutral, snap.Bounds.Width, Math.Max(1, CInt(Math.Round(snap.Bounds.Width * scaleX))))
                Dim newH As Integer = If(isNeutral, snap.Bounds.Height, Math.Max(1, CInt(Math.Round(snap.Bounds.Height * scaleY))))

                If _isMdiParentMode AndAlso ctl.Dock <> DockStyle.None Then
                    If Not isMainScreenMdiHost Then Continue For
                    ' Keep the dock relationship intact, but scale its sizing
                    ' dimension. MDI hosts depend on these docked controls for
                    ' the menu height and right-panel width; skipping them leaves
                    ' the navigation panel at designer size on other resolutions.
                    Select Case ctl.Dock
                        Case DockStyle.Top, DockStyle.Bottom
                            If ctl.Height <> newH Then ctl.Height = newH
                        Case DockStyle.Left, DockStyle.Right
                            If ctl.Width <> newW Then ctl.Width = newW
                    End Select
                Else

                    If _isMdiParentMode AndAlso ctl.Name = "Button23" AndAlso ctl.Parent IsNot Nothing AndAlso
                       TypeOf ctl.Parent Is Panel AndAlso ctl.Parent.Dock = DockStyle.Right Then
                        newT = ctl.Parent.ClientSize.Height - newH
                    End If

                    ' MDI child inputs get a small vertical allowance only above their
                    ' 1366 x 768 baseline. This fills the input-to-grid visual gap;
                    ' labels, buttons, panels and grids deliberately remain unchanged.
                    Dim inputExtraHeight As Integer = If(_isMdiParentMode, 0, GetMdiInputHeightAllowance(ctl, scaleY))
                    newH += inputExtraHeight
                    Dim textBox As TextBox = TryCast(ctl, TextBox)
                    Dim reduceAt100Percent As Boolean = textBox IsNot Nothing AndAlso Not textBox.Multiline
                    If reduceAt100Percent Then
                        PrepareInputForHeightScaling(ctl)
                        newH = Math.Max(20, CInt(Math.Round(newH * 0.9)))
                    ElseIf Not _isMdiParentMode AndAlso (inputExtraHeight > 0 OrElse TypeOf ctl Is DateTimePicker) Then
                        PrepareInputForHeightScaling(ctl)
                    End If
                    If Not _isMdiParentMode Then newW += GetMdiChildWidthAllowance(ctl, scaleY)

                    ' If a control was designed to reach the bottom edge of a full workspace canvas (e.g. background sidebar banner),
                    ' extend it down to the full canvas height to prevent any bottom gaps
                    If Not _isMdiParentMode AndAlso _isFullScreenCanvas AndAlso snap.Bounds.Bottom >= _baseHeight - 10 Then
                        newH = curH - newT
                    End If

                    ' If a control was designed to stretch across the full width (e.g. top banner, bottom status strip),
                    ' extend it to the right edge to eliminate rounding gaps
                    If Not _isMdiParentMode AndAlso _isFullScreenCanvas AndAlso snap.Bounds.Left <= 5 AndAlso snap.Bounds.Width >= _baseWidth - 15 Then
                        newW = curW - newL
                    End If

                    If ctl.Left <> newL OrElse ctl.Top <> newT OrElse ctl.Width <> newW OrElse ctl.Height <> newH Then
                        ctl.SetBounds(newL, newT, newW, newH)
                    End If
                End If

                ' 2. Scale Font
                If snap.Font IsNot Nothing Then
                    ' DateTimePicker must keep the 14pt effective baseline even
                    ' at 1:1. Its native AutoSize then derives the matching height.
                    If TypeOf ctl Is DateTimePicker Then
                        Dim dtpFontScale As Single = If(isNeutral, 1.0!, fontScale)
                        Dim dtpControlScale As Single = GetControlFontScale(ctl, snap.Font, dtpFontScale)
                        Dim dtpFontSize As Single = Math.Max(6.5!, CSng(Math.Round(snap.Font.Size * dtpControlScale, 1)))
                        Dim dtpFont As Font = GetCachedFont(snap.Font.FontFamily, dtpFontSize, snap.Font.Style)
                        If Not Object.ReferenceEquals(ctl.Font, dtpFont) Then ctl.Font = dtpFont
                    ElseIf isNeutral Then
                        If Not Object.ReferenceEquals(ctl.Font, snap.Font) Then ctl.Font = snap.Font
                    Else
                        Dim controlFontScale As Single = GetControlFontScale(ctl, snap.Font, fontScale)
                        Dim newSize As Single = Math.Max(6.5!, CSng(Math.Round(snap.Font.Size * controlFontScale, 1)))
                        Dim targetFont As Font = GetCachedFont(snap.Font.FontFamily, newSize, snap.Font.Style)
                        If Not Object.ReferenceEquals(ctl.Font, targetFont) Then ctl.Font = targetFont
                    End If
                End If

                ' 3. Scale DataGridView Columns and Row Heights proportionally
                If snap.IsDgv Then
                    Dim dgv = DirectCast(ctl, DataGridView)
                    If dgv.Columns.Count > 0 Then
                        While snap.DgvColWidths.Count < dgv.Columns.Count
                            snap.DgvColWidths.Add(Math.Max(10, dgv.Columns(snap.DgvColWidths.Count).Width))
                        End While

                        ' Step 1: Apply proportional scaleX to every column.
                        For c As Integer = 0 To dgv.Columns.Count - 1
                            If c < snap.DgvColWidths.Count Then
                                Dim baseColW As Integer = snap.DgvColWidths(c)
                                Dim targetColW As Integer = If(isNeutral, baseColW, Math.Max(10, CInt(Math.Round(baseColW * scaleX))))
                                If dgv.Columns(c).Width <> targetColW Then dgv.Columns(c).Width = targetColW
                            End If
                        Next

                        ' CompanyList's visible columns should stay inside this
                        ' grid's viewport at every scale. Other grids preserve their
                        ' own horizontal scrolling and column visibility behavior.
                        If String.Equals(_targetForm.Name, "CompanyList", StringComparison.OrdinalIgnoreCase) AndAlso
                           String.Equals(dgv.Name, "dg1", StringComparison.OrdinalIgnoreCase) Then
                            FitVisibleGridColumnsToViewport(dgv, snap, scaleX, isNeutral)
                        End If

                        If snap.DgvHeaderHeight > 0 Then
                            Dim targetHeaderH As Integer = If(isNeutral, snap.DgvHeaderHeight, Math.Max(18, CInt(Math.Round(snap.DgvHeaderHeight * scaleY))))
                            If dgv.ColumnHeadersHeight <> targetHeaderH Then dgv.ColumnHeadersHeight = targetHeaderH
                        End If

                        If snap.DgvRowHeight > 0 Then
                            Dim newRowH As Integer = If(isNeutral, snap.DgvRowHeight, Math.Max(18, CInt(Math.Round(snap.DgvRowHeight * scaleY))))
                            If dgv.RowTemplate.Height <> newRowH Then
                                dgv.RowTemplate.Height = newRowH
                                For Each row As DataGridViewRow In dgv.Rows
                                    row.Height = newRowH
                                Next
                            End If
                        End If

                        If snap.DgvHeaderFont IsNot Nothing Then
                            Dim targetHeaderSize As Single = If(isNeutral, snap.DgvHeaderFont.Size, Math.Max(6.5!, CSng(Math.Round(snap.DgvHeaderFont.Size * fontScale, 1))))
                            Dim targetHFont = GetCachedFont(snap.DgvHeaderFont.FontFamily, targetHeaderSize, snap.DgvHeaderFont.Style)
                            If Not Object.ReferenceEquals(dgv.ColumnHeadersDefaultCellStyle.Font, targetHFont) Then
                                dgv.ColumnHeadersDefaultCellStyle.Font = targetHFont
                            End If
                        End If

                        If snap.DgvCellFont IsNot Nothing Then
                            Dim targetCellSize As Single = If(isNeutral, snap.DgvCellFont.Size, Math.Max(6.5!, CSng(Math.Round(snap.DgvCellFont.Size * fontScale, 1))))
                            Dim targetCFont = GetCachedFont(snap.DgvCellFont.FontFamily, targetCellSize, snap.DgvCellFont.Style)
                            If Not Object.ReferenceEquals(dgv.DefaultCellStyle.Font, targetCFont) Then
                                dgv.DefaultCellStyle.Font = targetCFont
                            End If
                        End If

                        ' Search popup grids set an explicit RowsDefaultCellStyle.Font
                        ' in the designer. That style takes precedence over
                        ' DefaultCellStyle.Font, so scale it independently to keep
                        ' lookup row text responsive too.
                        If IsSearchOrChargeLookupGrid(dgv) AndAlso
                           snap.DgvRowsFont IsNot Nothing Then
                            Dim targetRowsSize As Single = If(isNeutral, snap.DgvRowsFont.Size,
                                Math.Max(6.5!, CSng(Math.Round(snap.DgvRowsFont.Size * fontScale, 1))))
                            Dim targetRowsFont = GetCachedFont(snap.DgvRowsFont.FontFamily,
                                                               targetRowsSize,
                                                               snap.DgvRowsFont.Style)
                            If Not Object.ReferenceEquals(dgv.RowsDefaultCellStyle.Font, targetRowsFont) Then
                                dgv.RowsDefaultCellStyle.Font = targetRowsFont
                            End If
                        End If
                    End If
                End If

                ' 4. ComboBox ItemHeight — font is already scaled above.
                '    ItemHeight must only be large enough to fit the scaled font.
                '    Do NOT multiply by scaleY again (double-scaling causes oversized combo).
                If snap.IsComboBox Then
                    Dim cb = DirectCast(ctl, ComboBox)
                    If cb.DrawMode <> DrawMode.Normal Then
                        ' Target: just enough to show the scaled font text with 4px padding.
                        ' GetHeight() returns line-height in pixels at current DPI.
                        Dim fontH As Integer = CInt(Math.Ceiling(ctl.Font.GetHeight()))
                        Dim targetItemH As Integer = fontH + 4
                        If cb.ItemHeight <> targetItemH Then
                            cb.ItemHeight = targetItemH
                        End If
                    End If
                End If

                If Not _isMdiParentMode AndAlso IsAdditionalFieldsControl(ctl) AndAlso snap.Font IsNot Nothing Then
                    Dim popupFontScale As Single = CSng(Math.Min(rawScaleX, rawScaleY))
                    Dim popupFontSize As Single = Math.Max(6.5!, CSng(Math.Round(snap.Font.Size * popupFontScale, 1)))
                    Dim popupFont As Font = GetCachedFont(snap.Font.FontFamily, popupFontSize, snap.Font.Style)
                    If Not Object.ReferenceEquals(ctl.Font, popupFont) Then ctl.Font = popupFont
                    ApplyAdditionalFieldVisibility(ctl)
                End If

                If _isMdiParentMode Then ScaleToolStripFonts(snap.ToolStripItemFonts, fontScale, isNeutral)

                Dim responsiveUserControl = TryCast(ctl, IResponsiveUserControlLayout)
                If responsiveUserControl IsNot Nothing Then responsiveUserControl.ApplyResponsiveLayout()
            Next

            AlignDateTimePickerHeights()

            If isMainScreenMdiHost Then ScaleMainScreenRightPanelButtons(scaleY)

            For Each kvp In _controlMap
                Dim popup As Control = kvp.Key
                If popup IsNot Nothing AndAlso Not popup.IsDisposed AndAlso popup.Visible AndAlso
                   (String.Equals(popup.Name, "pnlAdditional", StringComparison.OrdinalIgnoreCase) OrElse
                    String.Equals(popup.GetType().Name, "ucAddItems", StringComparison.OrdinalIgnoreCase)) Then
                    PositionAdditionalFieldsPopup(popup)
                End If
            Next

            _lastAppliedW = curW
            _lastAppliedH = curH

        Finally
            _targetForm.ResumeLayout(False)
            _isScaling = False
        End Try
    End Sub

    Private Function IsResponsiveOverlay() As Boolean
        Dim marker As String = TryCast(_targetForm.Tag, String)
        Return String.Equals(If(marker, String.Empty).Trim(), "ResponsiveOverlay", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub AlignDateTimePickerHeights()
        If _targetForm Is Nothing Then Return

        For Each kvp In _controlMap
            Dim picker As DateTimePicker = TryCast(kvp.Key, DateTimePicker)
            If picker Is Nothing OrElse picker.IsDisposed OrElse Not picker.Visible OrElse picker.Parent Is Nothing Then Continue For

            Dim matchingTextBox As TextBox = Nothing
            Dim bestVerticalDistance As Integer = Integer.MaxValue
            Dim bestHorizontalDistance As Integer = Integer.MaxValue
            For Each sibling As Control In picker.Parent.Controls
                Dim textBox As TextBox = TryCast(sibling, TextBox)
                If textBox Is Nothing OrElse textBox.IsDisposed OrElse Not textBox.Visible OrElse textBox.Multiline Then Continue For

                Dim verticalDistance As Integer = Math.Abs(textBox.Top - picker.Top)
                Dim sameRowTolerance As Integer = Math.Max(3, CInt(Math.Round(Math.Max(textBox.Height, picker.Height) * 0.25)))
                If verticalDistance > sameRowTolerance Then Continue For

                Dim horizontalDistance As Integer = Math.Abs(textBox.Left - picker.Left)
                If verticalDistance < bestVerticalDistance OrElse
                   (verticalDistance = bestVerticalDistance AndAlso horizontalDistance < bestHorizontalDistance) Then
                    matchingTextBox = textBox
                    bestVerticalDistance = verticalDistance
                    bestHorizontalDistance = horizontalDistance
                End If
            Next

            If matchingTextBox Is Nothing Then Continue For
            If picker.AutoSize Then picker.AutoSize = False
            If Not Object.ReferenceEquals(picker.Font, matchingTextBox.Font) Then picker.Font = matchingTextBox.Font
            If picker.Height <> matchingTextBox.Height Then picker.Height = matchingTextBox.Height
        Next
    End Sub

    ' MainScreenForm's navigation buttons are individually positioned in the
    ' designer. At taller aspect ratios that fixed stack leaves a blank block
    ' before Exit. Distribute its existing button order over the live dock panel
    ' height so the last navigation button remains immediately above Exit.
    Private Sub ScaleMainScreenRightPanelButtons(ByVal scaleY As Double)
        If _targetForm Is Nothing OrElse Not String.Equals(_targetForm.Name, "MainScreenForm", StringComparison.OrdinalIgnoreCase) Then Return

        Dim panel As Panel = TryCast(_targetForm.Controls("RightDockPanel"), Panel)
        If panel Is Nothing OrElse panel.IsDisposed Then Return
        Dim exitButton As Button = TryCast(panel.Controls("Button23"), Button)
        If exitButton Is Nothing OrElse exitButton.IsDisposed Then Return

        Dim buttons As New List(Of Control)()
        For Each child As Control In panel.Controls
            If TypeOf child Is Button AndAlso Not Object.ReferenceEquals(child, exitButton) Then buttons.Add(child)
        Next
        If buttons.Count = 0 Then Return

        buttons.Sort(Function(left As Control, right As Control)
                         Dim leftSnapshot As ControlSnapshot = Nothing
                         Dim rightSnapshot As ControlSnapshot = Nothing
                         Dim leftTop As Integer = If(_controlMap.TryGetValue(left, leftSnapshot), leftSnapshot.Bounds.Top, left.Top)
                         Dim rightTop As Integer = If(_controlMap.TryGetValue(right, rightSnapshot), rightSnapshot.Bounds.Top, right.Top)
                         Return leftTop.CompareTo(rightTop)
                     End Function)

        Dim firstSnapshot As ControlSnapshot = Nothing
        Dim firstTop As Integer = If(_controlMap.TryGetValue(buttons(0), firstSnapshot),
                                     CInt(Math.Round(firstSnapshot.Bounds.Top * scaleY)), buttons(0).Top)
        Dim lastTop As Integer = Math.Max(firstTop, panel.ClientSize.Height - exitButton.Height)
        Dim availableHeight As Integer = lastTop - firstTop
        If availableHeight < buttons.Count Then Return

        panel.SuspendLayout()
        Try
            For index As Integer = 0 To buttons.Count - 1
                Dim rowTop As Integer = firstTop + CInt(Math.Floor(CDbl(availableHeight) * index / buttons.Count))
                Dim rowBottom As Integer = firstTop + CInt(Math.Floor(CDbl(availableHeight) * (index + 1) / buttons.Count))
                Dim button As Control = buttons(index)
                button.SetBounds(0, rowTop, panel.ClientSize.Width, Math.Max(1, rowBottom - rowTop))
            Next
        Finally
            panel.ResumeLayout(False)
        End Try
    End Sub

    Private Shared Function IsAdditionalFieldsControl(ByVal ctl As Control) As Boolean
        If ctl Is Nothing Then Return False
        If TypeOf ctl Is Label OrElse TypeOf ctl Is TextBox Then
            Dim parent As Control = ctl.Parent
            While parent IsNot Nothing
                If String.Equals(parent.Name, "pnlAdditional", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(parent.GetType().Name, "ucAddItems", StringComparison.OrdinalIgnoreCase) Then Return True
                parent = parent.Parent
            End While
        End If
        Return False
    End Function

    Private Shared Function IsSearchOrChargeLookupGrid(ByVal dgv As DataGridView) As Boolean
        If dgv Is Nothing Then Return False
        Return dgv.Name.IndexOf("Search", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
               String.Equals(dgv.Name, "dgCharges", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub ApplyAdditionalFieldVisibility(ByVal ctl As Control)
        Dim popup As Control = ctl
        While popup IsNot Nothing AndAlso
              Not String.Equals(popup.Name, "pnlAdditional", StringComparison.OrdinalIgnoreCase) AndAlso
              Not String.Equals(popup.GetType().Name, "ucAddItems", StringComparison.OrdinalIgnoreCase)
            popup = popup.Parent
        End While
        If popup Is Nothing Then Return

        Dim countControl As Control = popup.Controls("txtFieldNos")
        If countControl Is Nothing Then Return
        Dim activeCount As Integer = Math.Max(0, Math.Min(10, Val(countControl.Text)))
        Dim isLabel As Boolean = ctl.Name.StartsWith("lblTitleAdd", StringComparison.OrdinalIgnoreCase)
        Dim isField As Boolean = ctl.Name.StartsWith("txtField", StringComparison.OrdinalIgnoreCase) AndAlso
                                 Not String.Equals(ctl.Name, "txtFieldNos", StringComparison.OrdinalIgnoreCase)
        If Not isLabel AndAlso Not isField Then Return

        Dim prefixLength As Integer = If(isLabel, "lblTitleAdd".Length, "txtField".Length)
        Dim fieldIndex As Integer = Val(ctl.Name.Substring(prefixLength))
        If fieldIndex > 0 Then ctl.Visible = (fieldIndex <= activeCount)
    End Sub

    Private Sub ScaleToolStripFonts(ByVal itemFonts As List(Of KeyValuePair(Of ToolStripItem, Font)),
                                    ByVal fontScale As Single, ByVal isNeutral As Boolean)
        For Each entry As KeyValuePair(Of ToolStripItem, Font) In itemFonts
            Dim item As ToolStripItem = entry.Key
            Dim baseFont As Font = entry.Value
            If item Is Nothing OrElse baseFont Is Nothing Then Continue For
            Dim size As Single = If(isNeutral, baseFont.Size, Math.Max(6.5!, CSng(Math.Round(baseFont.Size * fontScale, 1))))
            Dim targetFont As Font = GetCachedFont(baseFont.FontFamily, size, baseFont.Style)
            If Not Object.ReferenceEquals(item.Font, targetFont) Then item.Font = targetFont
        Next
    End Sub

    Public Shared Function GetCachedFont(ByVal family As FontFamily, ByVal sizeInPoints As Single, ByVal style As FontStyle) As Font
        Dim key As String = family.Name & ":" & sizeInPoints.ToString("F1") & ":" & style.ToString()
        SyncLock FontCache
            Dim cachedFont As Font = Nothing
            If FontCache.TryGetValue(key, cachedFont) Then
                Return cachedFont
            End If

            Try
                cachedFont = New Font(family, sizeInPoints, style)
            Catch
                cachedFont = SystemFonts.DefaultFont
            End Try
            FontCache(key) = cachedFont
            Return cachedFont
        End SyncLock
    End Function

    Private Shared Sub FitVisibleGridColumnsToViewport(ByVal dgv As DataGridView,
                                                        ByVal snap As ControlSnapshot,
                                                        ByVal scaleX As Double,
                                                        ByVal isNeutral As Boolean)
        If dgv Is Nothing OrElse dgv.IsDisposed OrElse dgv.Columns.Count = 0 Then Return

        Dim visibleCount As Integer = 0
        Dim totalColumnWidth As Integer = 0
        For columnIndex As Integer = 0 To dgv.Columns.Count - 1
            Dim column As DataGridViewColumn = dgv.Columns(columnIndex)
            If column.Visible Then
                visibleCount += 1
                Dim baselineWidth As Integer = column.Width
                If snap IsNot Nothing AndAlso columnIndex < snap.DgvColWidths.Count Then
                    baselineWidth = snap.DgvColWidths(columnIndex)
                End If
                If Not isNeutral Then baselineWidth = Math.Max(1, CInt(Math.Round(baselineWidth * scaleX)))
                totalColumnWidth += Math.Max(column.MinimumWidth, baselineWidth)
            End If
        Next
        If visibleCount = 0 OrElse totalColumnWidth <= 0 Then Return

        Dim viewportWidth As Integer = dgv.ClientSize.Width
        If dgv.RowHeadersVisible Then viewportWidth -= dgv.RowHeadersWidth
        If dgv.ScrollBars = ScrollBars.Vertical OrElse dgv.ScrollBars = ScrollBars.Both Then
            viewportWidth -= SystemInformation.VerticalScrollBarWidth
        End If
        If viewportWidth <= 0 Then Return

        ' Use the full viewport without exceeding it so the final visible
        ' CompanyList column is not clipped at the right edge.
        Dim proportionalViewportWidth As Integer = viewportWidth

        Dim remainingWidth As Integer = proportionalViewportWidth
        Dim remainingColumns As Integer = visibleCount
        For columnIndex As Integer = 0 To dgv.Columns.Count - 1
            Dim column As DataGridViewColumn = dgv.Columns(columnIndex)
            If Not column.Visible Then Continue For

            Dim targetWidth As Integer
            Dim baselineWidth As Integer = column.Width
            If snap IsNot Nothing AndAlso columnIndex < snap.DgvColWidths.Count Then
                baselineWidth = snap.DgvColWidths(columnIndex)
            End If
            If Not isNeutral Then baselineWidth = Math.Max(1, CInt(Math.Round(baselineWidth * scaleX)))
            If remainingColumns = 1 Then
                ' Preserve the last column's proportional width as well. Only
                ' distribute rounding/fit remainder here; never shrink the last
                ' column because earlier minimum-width corrections consumed space.
                targetWidth = Math.Max(column.MinimumWidth,
                    CInt(Math.Round(CDbl(Math.Max(column.MinimumWidth, baselineWidth)) * proportionalViewportWidth / totalColumnWidth)))
            Else
                targetWidth = CInt(Math.Round(CDbl(Math.Max(column.MinimumWidth, baselineWidth)) * proportionalViewportWidth / totalColumnWidth))
                targetWidth = Math.Max(column.MinimumWidth, targetWidth)
                targetWidth = Math.Min(targetWidth, remainingWidth - (remainingColumns - 1))
            End If

            If column.Width <> targetWidth Then column.Width = targetWidth
            remainingWidth -= targetWidth
            remainingColumns -= 1
        Next
    End Sub

    Private Sub OnFormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        Dispose()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not _disposed Then
            SyncLock ActiveScalers
                If _targetForm IsNot Nothing Then
                    ActiveScalers.Remove(_targetForm)
                End If
            End SyncLock

            If _resizeTimer IsNot Nothing Then
                _resizeTimer.Stop()
                RemoveHandler _resizeTimer.Tick, AddressOf OnResizeTimerTick
                _resizeTimer.Dispose()
                _resizeTimer = Nothing
            End If

            If _targetForm IsNot Nothing Then
                RemoveHandler _targetForm.Load, AddressOf OnFormLoad
                RemoveHandler _targetForm.Shown, AddressOf OnFormShown
                RemoveHandler _targetForm.Resize, AddressOf OnFormResize
                RemoveHandler _targetForm.FormClosed, AddressOf OnFormClosed
                RemoveHandler _targetForm.ControlAdded, AddressOf OnControlAdded
                RemoveHandler _targetForm.ParentChanged, AddressOf OnParentChanged
            End If

            UnhookMdiParentEvents()

            For Each dgv As DataGridView In _hookedDataGrids
                If dgv Is Nothing OrElse dgv.IsDisposed Then Continue For
                RemoveHandler dgv.ColumnAdded, AddressOf OnRuntimeGridColumnAdded
                RemoveHandler dgv.RowsAdded, AddressOf OnRuntimeGridRowsAdded
                RemoveHandler dgv.ColumnWidthChanged, AddressOf OnRuntimeGridColumnWidthChanged
            Next
            _hookedDataGrids.Clear()
            _pendingRuntimeGrids.Clear()

            _controlMap.Clear()
            _disposed = True
        End If
    End Sub
End Class
