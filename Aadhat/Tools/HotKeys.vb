Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class HotKeys
    Private layout As HotKeyLayout
    Private mainWindow As MainScreenForm

    Public Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Private Class ShortcutListItem
        Public ButtonRef As Button
        Public Sub New(ByVal button As Button)
            ButtonRef = button
        End Sub
        Public Overrides Function ToString() As String
            Return ButtonRef.Text
        End Function
    End Class
    Private Sub HotKeys_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        If LicenseManager.UsageMode = LicenseUsageMode.Designtime OrElse
           (Me.Site IsNot Nothing AndAlso Me.Site.DesignMode) Then Return

        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mainWindow = TryCast(Me.MdiParent, MainScreenForm)
        If mainWindow Is Nothing OrElse mainWindow.RightDockPanel Is Nothing OrElse mainWindow.MenuStrip1 Is Nothing Then Return
        layout = HotKeySettingsManager.Load(mainWindow.RightDockPanel)
        RefreshLists()
        BuildMenuTree()
    End Sub

    Private Sub HotKeys_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            e.Handled = True
            e.SuppressKeyPress = True
            Me.Close()
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub BuildMenuTree()
        If mainWindow Is Nothing OrElse mainWindow.MenuStrip1 Is Nothing OrElse tvMenus Is Nothing Then Return
        tvMenus.BeginUpdate()
        Try
            tvMenus.Nodes.Clear()
            For Each item As ToolStripItem In mainWindow.MenuStrip1.Items
                AddMenuNode(tvMenus.Nodes, item)
            Next
        Finally
            tvMenus.EndUpdate()
        End Try
    End Sub

    Private Function SelectedMenuItem() As ToolStripItem
        Dim node As TreeNode = tvMenus.SelectedNode
        If node Is Nothing Then Return Nothing
        Return TryCast(node.Tag, ToolStripItem)
    End Function

    Private Function SelectedMenuCollection() As ToolStripItemCollection
        If mainWindow Is Nothing OrElse mainWindow.MenuStrip1 Is Nothing Then Return Nothing
        Dim node As TreeNode = tvMenus.SelectedNode
        If node Is Nothing OrElse node.Parent Is Nothing Then Return mainWindow.MenuStrip1.Items
        Dim parentItem As ToolStripMenuItem = TryCast(node.Parent.Tag, ToolStripMenuItem)
        If parentItem Is Nothing Then Return Nothing
        Return parentItem.DropDownItems
    End Function

    Private Sub btnMenuUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMenuUp.Click
        MoveMenu(-1)
    End Sub
    Private Sub btnMenuDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMenuDown.Click
        MoveMenu(1)
    End Sub
    Private Sub MoveMenu(ByVal direction As Integer)
        Dim item As ToolStripItem = SelectedMenuItem() : Dim items As ToolStripItemCollection = SelectedMenuCollection()
        If item Is Nothing OrElse items Is Nothing Then Exit Sub
        Dim index As Integer = items.IndexOf(item) : Dim destination As Integer = index + direction
        If destination < 0 OrElse destination >= items.Count Then Exit Sub
        items.Remove(item) : items.Insert(destination, item) : BuildMenuTree()
    End Sub
    Private Sub btnMenuHideShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMenuHideShow.Click
        Dim item As ToolStripItem = SelectedMenuItem() : If item Is Nothing Then Exit Sub
        item.Visible = Not item.Visible
        If item.Visible Then
            layout.HiddenMenus.Remove(item.Name)
        ElseIf Not layout.HiddenMenus.Contains(item.Name) Then
            layout.HiddenMenus.Add(item.Name)
        End If
        BuildMenuTree()
    End Sub
    Private Sub btnMenuSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMenuSave.Click
        If mainWindow Is Nothing OrElse mainWindow.MenuStrip1 Is Nothing OrElse layout Is Nothing Then Return
        HotKeySettingsManager.SaveMenus(mainWindow.MenuStrip1, layout) : lblMenuStatus.Text = "Menu layout saved in Hot Key.json."
    End Sub

    Private Sub AddMenuNode(ByVal nodes As TreeNodeCollection, ByVal item As ToolStripItem)
        If String.IsNullOrEmpty(item.Text) Then Exit Sub
        Dim node As TreeNode = nodes.Add(item.Text.Replace("&", ""))
        node.Tag = item
        If Not item.Visible Then node.Text &= "  (Hidden)" : node.ForeColor = Drawing.Color.Gray
        Dim menuItem As ToolStripMenuItem = TryCast(item, ToolStripMenuItem)
        If menuItem Is Nothing Then Exit Sub
        For Each child As ToolStripItem In menuItem.DropDownItems
            AddMenuNode(node.Nodes, child)
        Next
    End Sub
    Private Sub RefreshLists()
        If lstAvailable Is Nothing OrElse lstPanel Is Nothing OrElse layout Is Nothing Then Return
        lstAvailable.Items.Clear() : lstPanel.Items.Clear()
        For Each name As String In layout.Order
            Dim button As Button = TryCast(mainWindow.RightDockPanel.Controls(name), Button)
            If button Is Nothing Then Continue For
            Dim item As New ShortcutListItem(button)
            If layout.Hidden.Contains(name) Then lstAvailable.Items.Add(item) Else lstPanel.Items.Add(item)
        Next
    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Dim selected As ShortcutListItem = TryCast(lstAvailable.SelectedItem, ShortcutListItem) : If selected Is Nothing Then Exit Sub
        Dim button As Button = selected.ButtonRef
        layout.Hidden.Remove(button.Name) : RefreshLists()
    End Sub
    Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
        Dim selected As ShortcutListItem = TryCast(lstPanel.SelectedItem, ShortcutListItem) : If selected Is Nothing Then Exit Sub
        Dim button As Button = selected.ButtonRef
        If Not layout.Hidden.Contains(button.Name) Then layout.Hidden.Add(button.Name) : RefreshLists()
    End Sub
    Private Sub btnUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUp.Click
        MoveSelected(-1)
    End Sub
    Private Sub btnDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDown.Click
        MoveSelected(1)
    End Sub
    Private Sub MoveSelected(ByVal direction As Integer)
        Dim selected As ShortcutListItem = TryCast(lstPanel.SelectedItem, ShortcutListItem) : If selected Is Nothing Then Exit Sub
        Dim button As Button = selected.ButtonRef
        Dim index As Integer = layout.Order.IndexOf(button.Name) : Dim destination As Integer = index + direction
        If destination < 0 OrElse destination >= layout.Order.Count Then Exit Sub
        layout.Order.RemoveAt(index) : layout.Order.Insert(destination, button.Name) : RefreshLists()
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If mainWindow Is Nothing OrElse mainWindow.RightDockPanel Is Nothing OrElse layout Is Nothing Then Return
        HotKeySettingsManager.SaveAndApply(mainWindow.RightDockPanel, layout) : lblStatus.Text = "Saved: buttons moved and working actions retained."
    End Sub
    Private Sub btnDefault_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDefault.Click
        If mainWindow Is Nothing OrElse mainWindow.RightDockPanel Is Nothing Then Return
        HotKeySettingsManager.RestoreDefault(mainWindow.RightDockPanel) : layout = HotKeySettingsManager.DefaultLayout(mainWindow.RightDockPanel) : RefreshLists() : lblStatus.Text = "Default right panel restored."
    End Sub
End Class
