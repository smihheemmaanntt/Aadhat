<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HotKeys
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then components.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lstAvailable = New ListBox()
        Me.lstPanel = New ListBox()
        Me.btnAdd = New Button()
        Me.btnRemove = New Button()
        Me.btnUp = New Button()
        Me.btnDown = New Button()
        Me.btnSave = New Button()
        Me.btnDefault = New Button()
        Me.lblStatus = New Label()
        Me.tvMenus = New TreeView()
        Me.btnMenuUp = New Button()
        Me.btnMenuDown = New Button()
        Me.btnMenuHideShow = New Button()
        Me.btnMenuSave = New Button()
        Me.lblMenuStatus = New Label()
        Me.btnClose = New Button()

        Me.lblTitle = New Label()
        Me.lblIntro = New Label()
        Me.lblShortcutsHeader = New Label()
        Me.lblMenusHeader = New Label()
        Me.lblActive = New Label()
        Me.lblAvailable = New Label()
        Me.lblMenuHelp = New Label()

        Me.SuspendLayout()
        Me.ClientSize = New Drawing.Size(1160, 650)
        Me.AutoScaleDimensions = New Drawing.SizeF(8.0!, 17.0!)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = Drawing.Color.FromArgb(244, 247, 252)
        Me.Font = New Drawing.Font("Segoe UI", 9.5!)
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.KeyPreview = True
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Name = "HotKeys"
        Me.Text = "Hot Keys and Menu Layout"

        Me.lblTitle.Location = New Drawing.Point(0, 0) : Me.lblTitle.Size = New Drawing.Size(1160, 50)
        Me.lblTitle.BackColor = Drawing.Color.FromArgb(31, 78, 121) : Me.lblTitle.ForeColor = Drawing.Color.White
        Me.lblTitle.Font = New Drawing.Font("Segoe UI", 15.0!, Drawing.FontStyle.Bold)
        Me.lblTitle.TextAlign = Drawing.ContentAlignment.MiddleLeft : Me.lblTitle.Padding = New Padding(20, 0, 0, 0)
        Me.lblTitle.Text = "Hot Keys and Menu Layout"
        Me.lblIntro.Location = New Drawing.Point(24, 60) : Me.lblIntro.Size = New Drawing.Size(1100, 26)
        Me.lblIntro.ForeColor = Drawing.Color.FromArgb(65, 75, 90)
        Me.lblIntro.Text = "Customize the right-side shortcut buttons and main menu order. Original button actions remain unchanged."

        Me.lblShortcutsHeader.Location = New Drawing.Point(20, 98) : Me.lblShortcutsHeader.Size = New Drawing.Size(660, 30)
        Me.lblShortcutsHeader.BackColor = Drawing.Color.FromArgb(221, 235, 247) : Me.lblShortcutsHeader.ForeColor = Drawing.Color.FromArgb(31, 78, 121)
        Me.lblShortcutsHeader.Font = New Drawing.Font("Segoe UI", 10.5!, Drawing.FontStyle.Bold)
        Me.lblShortcutsHeader.Padding = New Padding(8, 4, 0, 0) : Me.lblShortcutsHeader.Text = "RIGHT PANEL SHORTCUTS"
        Me.lblMenusHeader.Location = New Drawing.Point(700, 98) : Me.lblMenusHeader.Size = New Drawing.Size(440, 30)
        Me.lblMenusHeader.BackColor = Drawing.Color.FromArgb(232, 225, 244) : Me.lblMenusHeader.ForeColor = Drawing.Color.FromArgb(93, 65, 132)
        Me.lblMenusHeader.Font = New Drawing.Font("Segoe UI", 10.5!, Drawing.FontStyle.Bold)
        Me.lblMenusHeader.Padding = New Padding(8, 4, 0, 0) : Me.lblMenusHeader.Text = "MAIN MENUS AND SUBMENUS"

        Me.lblActive.Location = New Drawing.Point(25, 137) : Me.lblActive.Size = New Drawing.Size(260, 21)
        Me.lblActive.ForeColor = Drawing.Color.FromArgb(55, 65, 80) : Me.lblActive.Font = New Drawing.Font("Segoe UI", 9.5!, Drawing.FontStyle.Bold)
        Me.lblActive.Text = "Visible right panel buttons"
        Me.lblAvailable.Location = New Drawing.Point(405, 137) : Me.lblAvailable.Size = New Drawing.Size(270, 21)
        Me.lblAvailable.ForeColor = Drawing.Color.FromArgb(55, 65, 80) : Me.lblAvailable.Font = New Drawing.Font("Segoe UI", 9.5!, Drawing.FontStyle.Bold)
        Me.lblAvailable.Text = "Hidden / available buttons"

        Me.lstPanel.Location = New Drawing.Point(25, 162) : Me.lstPanel.Size = New Drawing.Size(260, 230)
        Me.lstPanel.Name = "lstPanel" : Me.lstPanel.Font = New Drawing.Font("Segoe UI", 9.5!)
        Me.lstPanel.BackColor = Drawing.Color.White : Me.lstPanel.BorderStyle = BorderStyle.FixedSingle
        Me.lstAvailable.Location = New Drawing.Point(405, 162) : Me.lstAvailable.Size = New Drawing.Size(270, 230)
        Me.lstAvailable.Name = "lstAvailable" : Me.lstAvailable.Font = New Drawing.Font("Segoe UI", 9.5!)
        Me.lstAvailable.BackColor = Drawing.Color.White : Me.lstAvailable.BorderStyle = BorderStyle.FixedSingle

        Me.btnAdd.Location = New Drawing.Point(300, 215) : Me.btnAdd.Size = New Drawing.Size(90, 36)
        Me.btnAdd.Name = "btnAdd" : Me.btnAdd.Text = "< Add"
        Me.btnRemove.Location = New Drawing.Point(300, 260) : Me.btnRemove.Size = New Drawing.Size(90, 36)
        Me.btnRemove.Name = "btnRemove" : Me.btnRemove.Text = "Remove >"
        Me.btnUp.Location = New Drawing.Point(25, 405) : Me.btnUp.Size = New Drawing.Size(120, 34)
        Me.btnUp.Name = "btnUp" : Me.btnUp.Text = "Move Up"
        Me.btnDown.Location = New Drawing.Point(155, 405) : Me.btnDown.Size = New Drawing.Size(120, 34)
        Me.btnDown.Name = "btnDown" : Me.btnDown.Text = "Move Down"
        Me.btnSave.Location = New Drawing.Point(25, 452) : Me.btnSave.Size = New Drawing.Size(150, 38)
        Me.btnSave.Name = "btnSave" : Me.btnSave.Text = "Save Layout"
        Me.btnDefault.Location = New Drawing.Point(190, 452) : Me.btnDefault.Size = New Drawing.Size(150, 38)
        Me.btnDefault.Name = "btnDefault" : Me.btnDefault.Text = "Restore Default"
        Me.lblStatus.Location = New Drawing.Point(25, 505) : Me.lblStatus.Size = New Drawing.Size(650, 48)
        Me.lblStatus.Name = "lblStatus" : Me.lblStatus.ForeColor = Drawing.Color.FromArgb(31, 100, 75)
        Me.lblStatus.BackColor = Drawing.Color.FromArgb(232, 244, 237) : Me.lblStatus.Padding = New Padding(8, 5, 5, 5)
        Me.lblStatus.Text = "Select a button to move, hide, or restore it."

        Me.lblMenuHelp.Location = New Drawing.Point(705, 137) : Me.lblMenuHelp.Size = New Drawing.Size(430, 21)
        Me.lblMenuHelp.ForeColor = Drawing.Color.FromArgb(65, 75, 90)
        Me.lblMenuHelp.Text = "Select a menu item, then use the controls below."
        Me.tvMenus.Location = New Drawing.Point(705, 162) : Me.tvMenus.Size = New Drawing.Size(430, 345)
        Me.tvMenus.Name = "tvMenus" : Me.tvMenus.Font = New Drawing.Font("Segoe UI", 9.5!)
        Me.tvMenus.BackColor = Drawing.Color.White : Me.tvMenus.BorderStyle = BorderStyle.FixedSingle
        Me.tvMenus.ShowLines = True : Me.tvMenus.ShowPlusMinus = True : Me.tvMenus.HideSelection = False
        Me.btnMenuUp.Location = New Drawing.Point(705, 520) : Me.btnMenuUp.Size = New Drawing.Size(105, 34)
        Me.btnMenuUp.Name = "btnMenuUp" : Me.btnMenuUp.Text = "Menu Up"
        Me.btnMenuDown.Location = New Drawing.Point(820, 520) : Me.btnMenuDown.Size = New Drawing.Size(105, 34)
        Me.btnMenuDown.Name = "btnMenuDown" : Me.btnMenuDown.Text = "Menu Down"
        Me.btnMenuHideShow.Location = New Drawing.Point(935, 520) : Me.btnMenuHideShow.Size = New Drawing.Size(130, 34)
        Me.btnMenuHideShow.Name = "btnMenuHideShow" : Me.btnMenuHideShow.Text = "Hide / Show"
        Me.btnMenuSave.Location = New Drawing.Point(705, 568) : Me.btnMenuSave.Size = New Drawing.Size(145, 38)
        Me.btnMenuSave.Name = "btnMenuSave" : Me.btnMenuSave.Text = "Save Menus"
        Me.lblMenuStatus.Location = New Drawing.Point(865, 568) : Me.lblMenuStatus.Size = New Drawing.Size(270, 40)
        Me.lblMenuStatus.Name = "lblMenuStatus" : Me.lblMenuStatus.ForeColor = Drawing.Color.FromArgb(31, 100, 75)
        Me.lblMenuStatus.BackColor = Drawing.Color.FromArgb(239, 235, 247) : Me.lblMenuStatus.Padding = New Padding(6, 4, 4, 4)
        Me.lblMenuStatus.Text = "Select a menu or submenu to manage it."

        Me.btnClose.Location = New Drawing.Point(1110, 8) : Me.btnClose.Size = New Drawing.Size(40, 34)
        Me.btnClose.Name = "btnClose" : Me.btnClose.Text = "X"
        Me.btnClose.FlatStyle = FlatStyle.Flat : Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.BackColor = Drawing.Color.FromArgb(190, 55, 55) : Me.btnClose.ForeColor = Drawing.Color.White
        Me.btnClose.Font = New Drawing.Font("Segoe UI", 11.0!, Drawing.FontStyle.Bold) : Me.btnClose.Cursor = Cursors.Hand

        Me.btnAdd.BackColor = Drawing.Color.FromArgb(15, 125, 90) : Me.btnAdd.ForeColor = Drawing.Color.White : Me.btnAdd.FlatStyle = FlatStyle.Flat : Me.btnAdd.FlatAppearance.BorderSize = 0 : Me.btnAdd.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnRemove.BackColor = Drawing.Color.FromArgb(190, 105, 35) : Me.btnRemove.ForeColor = Drawing.Color.White : Me.btnRemove.FlatStyle = FlatStyle.Flat : Me.btnRemove.FlatAppearance.BorderSize = 0 : Me.btnRemove.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnUp.BackColor = Drawing.Color.FromArgb(66, 133, 244) : Me.btnUp.ForeColor = Drawing.Color.White : Me.btnUp.FlatStyle = FlatStyle.Flat : Me.btnUp.FlatAppearance.BorderSize = 0 : Me.btnUp.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnDown.BackColor = Drawing.Color.FromArgb(66, 133, 244) : Me.btnDown.ForeColor = Drawing.Color.White : Me.btnDown.FlatStyle = FlatStyle.Flat : Me.btnDown.FlatAppearance.BorderSize = 0 : Me.btnDown.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnSave.BackColor = Drawing.Color.FromArgb(32, 145, 90) : Me.btnSave.ForeColor = Drawing.Color.White : Me.btnSave.FlatStyle = FlatStyle.Flat : Me.btnSave.FlatAppearance.BorderSize = 0 : Me.btnSave.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnDefault.BackColor = Drawing.Color.FromArgb(91, 103, 120) : Me.btnDefault.ForeColor = Drawing.Color.White : Me.btnDefault.FlatStyle = FlatStyle.Flat : Me.btnDefault.FlatAppearance.BorderSize = 0 : Me.btnDefault.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnMenuUp.BackColor = Drawing.Color.FromArgb(111, 78, 160) : Me.btnMenuUp.ForeColor = Drawing.Color.White : Me.btnMenuUp.FlatStyle = FlatStyle.Flat : Me.btnMenuUp.FlatAppearance.BorderSize = 0 : Me.btnMenuUp.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnMenuDown.BackColor = Drawing.Color.FromArgb(111, 78, 160) : Me.btnMenuDown.ForeColor = Drawing.Color.White : Me.btnMenuDown.FlatStyle = FlatStyle.Flat : Me.btnMenuDown.FlatAppearance.BorderSize = 0 : Me.btnMenuDown.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnMenuHideShow.BackColor = Drawing.Color.FromArgb(76, 106, 130) : Me.btnMenuHideShow.ForeColor = Drawing.Color.White : Me.btnMenuHideShow.FlatStyle = FlatStyle.Flat : Me.btnMenuHideShow.FlatAppearance.BorderSize = 0 : Me.btnMenuHideShow.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)
        Me.btnMenuSave.BackColor = Drawing.Color.FromArgb(32, 145, 90) : Me.btnMenuSave.ForeColor = Drawing.Color.White : Me.btnMenuSave.FlatStyle = FlatStyle.Flat : Me.btnMenuSave.FlatAppearance.BorderSize = 0 : Me.btnMenuSave.Font = New Drawing.Font("Segoe UI", 9.0!, Drawing.FontStyle.Bold)

        Me.Controls.AddRange(New Control() {Me.lblTitle, Me.btnClose, Me.lblIntro, Me.lblShortcutsHeader, Me.lblMenusHeader, Me.lblActive, Me.lblAvailable, Me.lstAvailable, Me.lstPanel, Me.btnAdd, Me.btnRemove, Me.btnUp, Me.btnDown, Me.btnSave, Me.btnDefault, Me.lblStatus, Me.lblMenuHelp, Me.tvMenus, Me.btnMenuUp, Me.btnMenuDown, Me.btnMenuHideShow, Me.btnMenuSave, Me.lblMenuStatus})
        Me.btnClose.BringToFront()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents lstAvailable, lstPanel As ListBox
    Friend WithEvents btnAdd, btnRemove, btnUp, btnDown, btnSave, btnDefault As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents tvMenus As TreeView
    Friend WithEvents btnMenuUp, btnMenuDown, btnMenuHideShow, btnMenuSave As Button
    Friend WithEvents lblMenuStatus As Label
    Friend WithEvents btnClose As Button
    Friend WithEvents lblTitle, lblIntro, lblShortcutsHeader, lblMenusHeader As Label
    Friend WithEvents lblActive, lblAvailable, lblMenuHelp As Label
End Class
