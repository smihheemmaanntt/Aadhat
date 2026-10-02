Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms
Imports Newtonsoft.Json

Public Class HotKeyLayout
    Public Order As New List(Of String)()
    Public Hidden As New List(Of String)()
    Public MenuOrders As New Dictionary(Of String, List(Of String))()
    Public HiddenMenus As New List(Of String)()
    ' Version 2 discards menu visibility captured before the Hot Keys editor
    ' could distinguish application-controlled visibility from user changes.
    Public MenuLayoutVersion As Integer = 0
End Class

Public NotInheritable Class HotKeySettingsManager
    Private Shared ReadOnly defaultOrders As New Dictionary(Of Panel, HotKeyLayout)()
    Private Shared ReadOnly exitHooks As New Dictionary(Of Panel, Boolean)()
    Private Shared Function ConfigPath() As String
        Return Path.Combine(Application.StartupPath, "Hot Key.json")
    End Function
    Public Shared Function Load(ByVal panel As Panel) As HotKeyLayout
        Dim layout As HotKeyLayout = Nothing
        Try
            If File.Exists(ConfigPath()) Then layout = JsonConvert.DeserializeObject(Of HotKeyLayout)(File.ReadAllText(ConfigPath()))
        Catch
        End Try
        If layout Is Nothing Then layout = DefaultLayout(panel)
        If layout.Order Is Nothing Then layout.Order = New List(Of String)()
        If layout.Hidden Is Nothing Then layout.Hidden = New List(Of String)()
        If layout.MenuOrders Is Nothing Then layout.MenuOrders = New Dictionary(Of String, List(Of String))()
        If layout.HiddenMenus Is Nothing Then layout.HiddenMenus = New List(Of String)()

        ' Older profiles treated every currently invisible menu as a user-hidden
        ' menu.  That also included menus hidden temporarily by application
        ' logic, so it could make the entire Tools menu disappear on restart.
        ' Keep the user's right-panel arrangement, but reset only that stale
        ' menu profile once.
        If layout.MenuLayoutVersion < 2 Then
            layout.MenuOrders.Clear()
            layout.HiddenMenus.Clear()
            layout.MenuLayoutVersion = 2
            Try
                File.WriteAllText(ConfigPath(), JsonConvert.SerializeObject(layout, Formatting.Indented))
            Catch
            End Try
        End If
        Return layout
    End Function
    Public Shared Sub LoadAndApply(ByVal panel As Panel)
        RememberDefault(panel)
        HookExitButton(panel)
        Apply(panel, Load(panel))
    End Sub
    Public Shared Sub ApplyMenus(ByVal menu As MenuStrip, ByVal layout As HotKeyLayout)
        ApplyMenuItems(menu.Items, "__root__", layout)
    End Sub
    Public Shared Sub SaveMenus(ByVal menu As MenuStrip, ByVal layout As HotKeyLayout)
        layout.MenuOrders.Clear()
        CaptureMenuItems(menu.Items, "__root__", layout)
        layout.MenuLayoutVersion = 2
        File.WriteAllText(ConfigPath(), JsonConvert.SerializeObject(layout, Formatting.Indented))
    End Sub
    Private Shared Sub CaptureMenuItems(ByVal items As ToolStripItemCollection, ByVal parentKey As String, ByVal layout As HotKeyLayout)
        Dim order As New List(Of String)()
        For Each item As ToolStripItem In items
            If String.IsNullOrEmpty(item.Name) Then Continue For
            order.Add(item.Name)
            Dim menuItem As ToolStripMenuItem = TryCast(item, ToolStripMenuItem)
            If menuItem IsNot Nothing Then CaptureMenuItems(menuItem.DropDownItems, item.Name, layout)
        Next
        layout.MenuOrders(parentKey) = order
    End Sub
    Private Shared Sub ApplyMenuItems(ByVal items As ToolStripItemCollection, ByVal parentKey As String, ByVal layout As HotKeyLayout)
        Dim current As New Dictionary(Of String, ToolStripItem)()
        For Each item As ToolStripItem In items : If Not String.IsNullOrEmpty(item.Name) Then current(item.Name) = item
        Next
        Dim arranged As New List(Of ToolStripItem)()
        If layout.MenuOrders IsNot Nothing AndAlso layout.MenuOrders.ContainsKey(parentKey) Then
            For Each name As String In layout.MenuOrders(parentKey) : If current.ContainsKey(name) Then arranged.Add(current(name))
            Next
        End If
        For Each item As ToolStripItem In items : If Not arranged.Contains(item) Then arranged.Add(item)
        Next
        items.Clear() : For Each item As ToolStripItem In arranged
            items.Add(item)
            ' Do not force menus visible just because they are not in the
            ' profile: the application may be hiding them for user rights.
            If layout.HiddenMenus.Contains(item.Name) Then item.Visible = False
        Next
        For Each item As ToolStripItem In arranged
            Dim menuItem As ToolStripMenuItem = TryCast(item, ToolStripMenuItem)
            If menuItem IsNot Nothing Then ApplyMenuItems(menuItem.DropDownItems, item.Name, layout)
        Next
    End Sub
    Public Shared Sub SaveAndApply(ByVal panel As Panel, ByVal layout As HotKeyLayout)
        File.WriteAllText(ConfigPath(), JsonConvert.SerializeObject(layout, Formatting.Indented)) : Apply(panel, layout)
    End Sub
    Public Shared Sub RestoreDefault(ByVal panel As Panel)
        If File.Exists(ConfigPath()) Then File.Delete(ConfigPath())
        Apply(panel, DefaultLayout(panel))
    End Sub
    Public Shared Function DefaultLayout(ByVal panel As Panel) As HotKeyLayout
        If defaultOrders.ContainsKey(panel) Then
            Return New HotKeyLayout With {.Order = New List(Of String)(defaultOrders(panel).Order), .Hidden = New List(Of String)()}
        End If
        Dim result As New HotKeyLayout() : Dim buttons As New List(Of Button)()
        For Each control As Control In panel.Controls
            Dim button As Button = TryCast(control, Button)
            If button IsNot Nothing AndAlso button.Name <> "Button23" Then buttons.Add(button)
        Next
        buttons.Sort(Function(a As Button, b As Button) a.Top.CompareTo(b.Top))
        For Each button As Button In buttons : result.Order.Add(button.Name) : Next
        Return result
    End Function
    Private Shared Sub RememberDefault(ByVal panel As Panel)
        If defaultOrders.ContainsKey(panel) Then Exit Sub
        Dim current As HotKeyLayout = DefaultLayout(panel)
        defaultOrders(panel) = New HotKeyLayout With {.Order = New List(Of String)(current.Order), .Hidden = New List(Of String)()}
    End Sub
    Public Shared Sub Apply(ByVal panel As Panel, ByVal layout As HotKeyLayout)
        Dim buttons As New Dictionary(Of String, Button)()
        For Each control As Control In panel.Controls
            Dim button As Button = TryCast(control, Button)
            If button IsNot Nothing AndAlso button.Name <> "Button23" Then buttons(button.Name) = button
        Next
        Dim order As New List(Of String)()
        For Each name As String In layout.Order : If buttons.ContainsKey(name) AndAlso Not order.Contains(name) Then order.Add(name)
        Next
        For Each name As String In DefaultLayout(panel).Order : If Not order.Contains(name) Then order.Add(name)
        Next
        ' The logo label can be resized by the responsive layout.  Start the
        ' first shortcut from its real bottom edge rather than a fixed value,
        ' so no blank strip appears between "Aadhat" and Account.
        Dim logoLabel As Control = panel.Controls("Label1")
        Dim top As Integer = If(logoLabel Is Nothing, 0, logoLabel.Bottom)
        panel.SuspendLayout()
        Try
            For Each name As String In order
                Dim button As Button = buttons(name) : button.Visible = Not layout.Hidden.Contains(name)
                If button.Visible Then button.Location = New Drawing.Point(0, top) : top += button.Height
            Next
        Finally
            panel.ResumeLayout(True)
        End Try
        KeepExitAfterShortcuts(panel)
    End Sub

    ' Exit is not a configurable shortcut. Keep it immediately after the last
    ' visible shortcut, so hiding entries never leaves a large empty gap.
    Private Shared Sub HookExitButton(ByVal panel As Panel)
        If exitHooks.ContainsKey(panel) Then Return
        exitHooks(panel) = True
        AddHandler panel.Resize, AddressOf RightPanelResized
    End Sub

    Private Shared Sub RightPanelResized(ByVal sender As Object, ByVal e As EventArgs)
        Dim panel As Panel = TryCast(sender, Panel)
        If panel Is Nothing OrElse Not panel.IsHandleCreated Then Return
        panel.BeginInvoke(New MethodInvoker(Sub() KeepExitAfterShortcuts(panel)))
    End Sub

    Private Shared Sub KeepExitAfterShortcuts(ByVal panel As Panel)
        Dim exitButton As Button = TryCast(panel.Controls("Button23"), Button)
        If exitButton Is Nothing Then Return
        Dim nextTop As Integer = 0
        For Each control As Control In panel.Controls
            Dim button As Button = TryCast(control, Button)
            If button IsNot Nothing AndAlso button IsNot exitButton AndAlso button.Visible Then
                nextTop = Math.Max(nextTop, button.Bottom)
            End If
        Next
        exitButton.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        exitButton.SetBounds(0, nextTop, panel.ClientSize.Width, exitButton.Height)
    End Sub
End Class
