<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CloudBackupConfiguration
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer
    Friend WithEvents chkGoogle As CheckBox
    Friend WithEvents txtGoogleEmail As TextBox
    Friend WithEvents btnGoogleConnect As Button
    Friend WithEvents btnEnableDriveApi As Button
    Friend WithEvents btnImportGoogleJson As Button
    Friend WithEvents btnGoogleConnectAccount As Button
    Friend WithEvents btnOneDriveLogin As Button
    Friend WithEvents btnUserManual As Button
    Friend WithEvents lblGoogleStatus As Label
    Friend WithEvents chkOneDrive As CheckBox
    Friend WithEvents txtOneDrivePath As TextBox
    Friend WithEvents btnDetectOneDrive As Button
    Friend WithEvents btnBrowseOneDrive As Button
    Friend WithEvents cbSchedule As ComboBox
    Friend WithEvents chkSyncOnClose As CheckBox
    Friend WithEvents lblLastGoogle As Label
    Friend WithEvents lblLastOneDrive As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnClose As Button
    Friend WithEvents lblStatus As Label
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents lblTitle, lblIntro, lblGoogleStep, lblGoogleHelp, lblEmail As Label
    Friend WithEvents lblDriveStep, lblDriveHelp, lblOneDrivePath, lblOptionsStep As Label
    Friend WithEvents lblLastBackups, lblSchedule As Label

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.chkGoogle = New System.Windows.Forms.CheckBox()
        Me.txtGoogleEmail = New System.Windows.Forms.TextBox()
        Me.btnGoogleConnect = New System.Windows.Forms.Button()
        Me.btnEnableDriveApi = New System.Windows.Forms.Button()
        Me.btnImportGoogleJson = New System.Windows.Forms.Button()
        Me.btnGoogleConnectAccount = New System.Windows.Forms.Button()
        Me.btnOneDriveLogin = New System.Windows.Forms.Button()
        Me.btnUserManual = New System.Windows.Forms.Button()
        Me.lblGoogleStatus = New System.Windows.Forms.Label()
        Me.chkOneDrive = New System.Windows.Forms.CheckBox()
        Me.txtOneDrivePath = New System.Windows.Forms.TextBox()
        Me.btnDetectOneDrive = New System.Windows.Forms.Button()
        Me.btnBrowseOneDrive = New System.Windows.Forms.Button()
        Me.cbSchedule = New System.Windows.Forms.ComboBox()
        Me.chkSyncOnClose = New System.Windows.Forms.CheckBox()
        Me.lblLastGoogle = New System.Windows.Forms.Label()
        Me.lblLastOneDrive = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblIntro = New System.Windows.Forms.Label()
        Me.lblGoogleStep = New System.Windows.Forms.Label()
        Me.lblGoogleHelp = New System.Windows.Forms.Label()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.lblDriveStep = New System.Windows.Forms.Label()
        Me.lblDriveHelp = New System.Windows.Forms.Label()
        Me.lblOneDrivePath = New System.Windows.Forms.Label()
        Me.lblOptionsStep = New System.Windows.Forms.Label()
        Me.lblLastBackups = New System.Windows.Forms.Label()
        Me.lblSchedule = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'chkGoogle
        '
        Me.chkGoogle.AutoSize = True
        Me.chkGoogle.BackColor = Me.BackColor
        Me.chkGoogle.Location = New System.Drawing.Point(30, 220)
        Me.chkGoogle.Name = "chkGoogle"
        Me.chkGoogle.Size = New System.Drawing.Size(227, 25)
        Me.chkGoogle.TabIndex = 7
        Me.chkGoogle.Text = "Enable Google Drive backup"
        Me.chkGoogle.UseVisualStyleBackColor = False
        '
        'txtGoogleEmail
        '
        Me.txtGoogleEmail.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.txtGoogleEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGoogleEmail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.txtGoogleEmail.Location = New System.Drawing.Point(30, 274)
        Me.txtGoogleEmail.Name = "txtGoogleEmail"
        Me.txtGoogleEmail.Size = New System.Drawing.Size(355, 29)
        Me.txtGoogleEmail.TabIndex = 9
        '
        'btnGoogleConnect
        '
        Me.btnGoogleConnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(66, Byte), Integer), CType(CType(133, Byte), Integer), CType(CType(244, Byte), Integer))
        Me.btnGoogleConnect.FlatAppearance.BorderSize = 0
        Me.btnGoogleConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGoogleConnect.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGoogleConnect.ForeColor = System.Drawing.Color.White
        Me.btnGoogleConnect.Location = New System.Drawing.Point(30, 179)
        Me.btnGoogleConnect.Name = "btnGoogleConnect"
        Me.btnGoogleConnect.Size = New System.Drawing.Size(185, 34)
        Me.btnGoogleConnect.TabIndex = 5
        Me.btnGoogleConnect.Text = "1. Open OAuth Setup"
        Me.btnGoogleConnect.UseVisualStyleBackColor = False
        '
        'btnEnableDriveApi
        '
        Me.btnEnableDriveApi.BackColor = System.Drawing.Color.FromArgb(CType(CType(52, Byte), Integer), CType(CType(112, Byte), Integer), CType(CType(188, Byte), Integer))
        Me.btnEnableDriveApi.FlatAppearance.BorderSize = 0
        Me.btnEnableDriveApi.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEnableDriveApi.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnEnableDriveApi.ForeColor = System.Drawing.Color.White
        Me.btnEnableDriveApi.Location = New System.Drawing.Point(225, 179)
        Me.btnEnableDriveApi.Name = "btnEnableDriveApi"
        Me.btnEnableDriveApi.Size = New System.Drawing.Size(155, 34)
        Me.btnEnableDriveApi.TabIndex = 6
        Me.btnEnableDriveApi.Text = "2. Enable Drive API"
        Me.btnEnableDriveApi.UseVisualStyleBackColor = False
        '
        'btnImportGoogleJson
        '
        Me.btnImportGoogleJson.BackColor = System.Drawing.Color.FromArgb(CType(CType(111, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(160, Byte), Integer))
        Me.btnImportGoogleJson.FlatAppearance.BorderSize = 0
        Me.btnImportGoogleJson.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImportGoogleJson.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnImportGoogleJson.ForeColor = System.Drawing.Color.White
        Me.btnImportGoogleJson.Location = New System.Drawing.Point(405, 270)
        Me.btnImportGoogleJson.Name = "btnImportGoogleJson"
        Me.btnImportGoogleJson.Size = New System.Drawing.Size(160, 35)
        Me.btnImportGoogleJson.TabIndex = 10
        Me.btnImportGoogleJson.Text = "3. Import OAuth JSON"
        Me.btnImportGoogleJson.UseVisualStyleBackColor = False
        '
        'btnGoogleConnectAccount
        '
        Me.btnGoogleConnectAccount.BackColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(125, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btnGoogleConnectAccount.FlatAppearance.BorderSize = 0
        Me.btnGoogleConnectAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGoogleConnectAccount.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGoogleConnectAccount.ForeColor = System.Drawing.Color.White
        Me.btnGoogleConnectAccount.Location = New System.Drawing.Point(577, 270)
        Me.btnGoogleConnectAccount.Name = "btnGoogleConnectAccount"
        Me.btnGoogleConnectAccount.Size = New System.Drawing.Size(190, 35)
        Me.btnGoogleConnectAccount.TabIndex = 11
        Me.btnGoogleConnectAccount.Text = "4. Connect Google account"
        Me.btnGoogleConnectAccount.UseVisualStyleBackColor = False
        '
        'btnOneDriveLogin
        '
        Me.btnOneDriveLogin.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(212, Byte), Integer))
        Me.btnOneDriveLogin.FlatAppearance.BorderSize = 0
        Me.btnOneDriveLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOneDriveLogin.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnOneDriveLogin.ForeColor = System.Drawing.Color.White
        Me.btnOneDriveLogin.Location = New System.Drawing.Point(242, 429)
        Me.btnOneDriveLogin.Name = "btnOneDriveLogin"
        Me.btnOneDriveLogin.Size = New System.Drawing.Size(175, 32)
        Me.btnOneDriveLogin.TabIndex = 16
        Me.btnOneDriveLogin.Text = "Sign in to OneDrive"
        Me.btnOneDriveLogin.UseVisualStyleBackColor = False
        '
        'btnUserManual
        '
        Me.btnUserManual.BackColor = System.Drawing.Color.FromArgb(CType(CType(91, Byte), Integer), CType(CType(103, Byte), Integer), CType(CType(120, Byte), Integer))
        Me.btnUserManual.FlatAppearance.BorderSize = 0
        Me.btnUserManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUserManual.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnUserManual.ForeColor = System.Drawing.Color.White
        Me.btnUserManual.Location = New System.Drawing.Point(218, 681)
        Me.btnUserManual.Name = "btnUserManual"
        Me.btnUserManual.Size = New System.Drawing.Size(145, 38)
        Me.btnUserManual.TabIndex = 29
        Me.btnUserManual.Text = "Help / Steps"
        Me.btnUserManual.UseVisualStyleBackColor = False
        '
        'lblGoogleStatus
        '
        Me.lblGoogleStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(246, Byte), Integer))
        Me.lblGoogleStatus.Location = New System.Drawing.Point(30, 311)
        Me.lblGoogleStatus.Name = "lblGoogleStatus"
        Me.lblGoogleStatus.Padding = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.lblGoogleStatus.Size = New System.Drawing.Size(737, 25)
        Me.lblGoogleStatus.TabIndex = 12
        Me.lblGoogleStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkOneDrive
        '
        Me.chkOneDrive.AutoSize = True
        Me.chkOneDrive.BackColor = Me.BackColor
        Me.chkOneDrive.Location = New System.Drawing.Point(30, 432)
        Me.chkOneDrive.Name = "chkOneDrive"
        Me.chkOneDrive.Size = New System.Drawing.Size(202, 25)
        Me.chkOneDrive.TabIndex = 15
        Me.chkOneDrive.Text = "Enable OneDrive backup"
        Me.chkOneDrive.UseVisualStyleBackColor = False
        '
        'txtOneDrivePath
        '
        Me.txtOneDrivePath.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.txtOneDrivePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtOneDrivePath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(60, Byte), Integer))
        Me.txtOneDrivePath.Location = New System.Drawing.Point(30, 486)
        Me.txtOneDrivePath.Name = "txtOneDrivePath"
        Me.txtOneDrivePath.Size = New System.Drawing.Size(430, 29)
        Me.txtOneDrivePath.TabIndex = 18
        '
        'btnDetectOneDrive
        '
        Me.btnDetectOneDrive.BackColor = System.Drawing.Color.FromArgb(CType(CType(42, Byte), Integer), CType(CType(132, Byte), Integer), CType(CType(78, Byte), Integer))
        Me.btnDetectOneDrive.FlatAppearance.BorderSize = 0
        Me.btnDetectOneDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDetectOneDrive.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnDetectOneDrive.ForeColor = System.Drawing.Color.White
        Me.btnDetectOneDrive.Location = New System.Drawing.Point(466, 482)
        Me.btnDetectOneDrive.Name = "btnDetectOneDrive"
        Me.btnDetectOneDrive.Size = New System.Drawing.Size(145, 35)
        Me.btnDetectOneDrive.TabIndex = 19
        Me.btnDetectOneDrive.Text = "Detect OneDrive"
        Me.btnDetectOneDrive.UseVisualStyleBackColor = False
        '
        'btnBrowseOneDrive
        '
        Me.btnBrowseOneDrive.BackColor = System.Drawing.Color.FromArgb(CType(CType(76, Byte), Integer), CType(CType(106, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.btnBrowseOneDrive.FlatAppearance.BorderSize = 0
        Me.btnBrowseOneDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBrowseOneDrive.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnBrowseOneDrive.ForeColor = System.Drawing.Color.White
        Me.btnBrowseOneDrive.Location = New System.Drawing.Point(632, 483)
        Me.btnBrowseOneDrive.Name = "btnBrowseOneDrive"
        Me.btnBrowseOneDrive.Size = New System.Drawing.Size(135, 34)
        Me.btnBrowseOneDrive.TabIndex = 20
        Me.btnBrowseOneDrive.Text = "Browse..."
        Me.btnBrowseOneDrive.UseVisualStyleBackColor = False
        '
        'cbSchedule
        '
        Me.cbSchedule.BackColor = System.Drawing.Color.White
        Me.cbSchedule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSchedule.Items.AddRange(New Object() {"Daily", "Weekly"})
        Me.cbSchedule.Location = New System.Drawing.Point(155, 598)
        Me.cbSchedule.Name = "cbSchedule"
        Me.cbSchedule.Size = New System.Drawing.Size(130, 29)
        Me.cbSchedule.TabIndex = 23
        '
        'chkSyncOnClose
        '
        Me.chkSyncOnClose.AutoSize = True
        Me.chkSyncOnClose.BackColor = Me.BackColor
        Me.chkSyncOnClose.Location = New System.Drawing.Point(310, 600)
        Me.chkSyncOnClose.Name = "chkSyncOnClose"
        Me.chkSyncOnClose.Size = New System.Drawing.Size(372, 25)
        Me.chkSyncOnClose.TabIndex = 24
        Me.chkSyncOnClose.Text = "Sync due backups when Backup On Server closes"
        Me.chkSyncOnClose.UseVisualStyleBackColor = False
        '
        'lblLastGoogle
        '
        Me.lblLastGoogle.Location = New System.Drawing.Point(30, 653)
        Me.lblLastGoogle.Name = "lblLastGoogle"
        Me.lblLastGoogle.Size = New System.Drawing.Size(360, 20)
        Me.lblLastGoogle.TabIndex = 26
        Me.lblLastGoogle.Text = "Google Drive: Never"
        '
        'lblLastOneDrive
        '
        Me.lblLastOneDrive.Location = New System.Drawing.Point(405, 654)
        Me.lblLastOneDrive.Name = "lblLastOneDrive"
        Me.lblLastOneDrive.Size = New System.Drawing.Size(360, 20)
        Me.lblLastOneDrive.TabIndex = 27
        Me.lblLastOneDrive.Text = "OneDrive: Never"
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(145, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.Location = New System.Drawing.Point(30, 681)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(175, 38)
        Me.btnSave.TabIndex = 28
        Me.btnSave.Text = "Save Configuration"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(190, Byte), Integer), CType(CType(55, Byte), Integer), CType(CType(55, Byte), Integer))
        Me.btnClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.ForeColor = System.Drawing.Color.White
        Me.btnClose.Location = New System.Drawing.Point(750, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(38, 32)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "X"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblStatus
        '
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(121, Byte), Integer))
        Me.lblStatus.Location = New System.Drawing.Point(380, 681)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(350, 38)
        Me.lblStatus.TabIndex = 30
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(121, Byte), Integer))
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 15.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(0, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.lblTitle.Size = New System.Drawing.Size(800, 48)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Cloud Backup Setup"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblIntro
        '
        Me.lblIntro.ForeColor = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lblIntro.Location = New System.Drawing.Point(22, 57)
        Me.lblIntro.Name = "lblIntro"
        Me.lblIntro.Size = New System.Drawing.Size(755, 30)
        Me.lblIntro.TabIndex = 2
        Me.lblIntro.Text = "Connect Google Drive or OneDrive, then save your backup schedule. You can use eit" & _
    "her or both services."
        '
        'lblGoogleStep
        '
        Me.lblGoogleStep.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.lblGoogleStep.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
        Me.lblGoogleStep.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(78, Byte), Integer), CType(CType(121, Byte), Integer))
        Me.lblGoogleStep.Location = New System.Drawing.Point(22, 96)
        Me.lblGoogleStep.Name = "lblGoogleStep"
        Me.lblGoogleStep.Padding = New System.Windows.Forms.Padding(8, 3, 0, 0)
        Me.lblGoogleStep.Size = New System.Drawing.Size(755, 27)
        Me.lblGoogleStep.TabIndex = 3
        Me.lblGoogleStep.Text = "STEP 1  |  Google Drive setup (one-time setup)"
        '
        'lblGoogleHelp
        '
        Me.lblGoogleHelp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lblGoogleHelp.Location = New System.Drawing.Point(30, 130)
        Me.lblGoogleHelp.Name = "lblGoogleHelp"
        Me.lblGoogleHelp.Size = New System.Drawing.Size(740, 42)
        Me.lblGoogleHelp.TabIndex = 4
        Me.lblGoogleHelp.Text = "Create a Desktop OAuth client, enable Drive API, and add your backup Gmail under " & _
    "Audience > Test users."
        '
        'lblEmail
        '
        Me.lblEmail.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblEmail.ForeColor = System.Drawing.Color.FromArgb(CType(CType(55, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(80, Byte), Integer))
        Me.lblEmail.Location = New System.Drawing.Point(30, 251)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(370, 21)
        Me.lblEmail.TabIndex = 8
        Me.lblEmail.Text = "Backup Gmail account to authorize"
        '
        'lblDriveStep
        '
        Me.lblDriveStep.BackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(239, Byte), Integer), CType(CType(218, Byte), Integer))
        Me.lblDriveStep.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
        Me.lblDriveStep.ForeColor = System.Drawing.Color.FromArgb(CType(CType(48, Byte), Integer), CType(CType(92, Byte), Integer), CType(CType(44, Byte), Integer))
        Me.lblDriveStep.Location = New System.Drawing.Point(22, 370)
        Me.lblDriveStep.Name = "lblDriveStep"
        Me.lblDriveStep.Padding = New System.Windows.Forms.Padding(8, 3, 0, 0)
        Me.lblDriveStep.Size = New System.Drawing.Size(755, 27)
        Me.lblDriveStep.TabIndex = 13
        Me.lblDriveStep.Text = "STEP 2  |  OneDrive (optional)"
        '
        'lblDriveHelp
        '
        Me.lblDriveHelp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lblDriveHelp.Location = New System.Drawing.Point(30, 403)
        Me.lblDriveHelp.Name = "lblDriveHelp"
        Me.lblDriveHelp.Size = New System.Drawing.Size(735, 24)
        Me.lblDriveHelp.TabIndex = 14
        Me.lblDriveHelp.Text = "Sign in to the OneDrive desktop app. Detect the sync folder or choose it with Bro" & _
    "wse."
        '
        'lblOneDrivePath
        '
        Me.lblOneDrivePath.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblOneDrivePath.Location = New System.Drawing.Point(30, 464)
        Me.lblOneDrivePath.Name = "lblOneDrivePath"
        Me.lblOneDrivePath.Size = New System.Drawing.Size(430, 20)
        Me.lblOneDrivePath.TabIndex = 17
        Me.lblOneDrivePath.Text = "OneDrive sync folder path (Detect or Browse)"
        '
        'lblOptionsStep
        '
        Me.lblOptionsStep.BackColor = System.Drawing.Color.FromArgb(CType(CType(252, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(210, Byte), Integer))
        Me.lblOptionsStep.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold)
        Me.lblOptionsStep.ForeColor = System.Drawing.Color.FromArgb(CType(CType(133, Byte), Integer), CType(CType(83, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.lblOptionsStep.Location = New System.Drawing.Point(22, 564)
        Me.lblOptionsStep.Name = "lblOptionsStep"
        Me.lblOptionsStep.Padding = New System.Windows.Forms.Padding(8, 3, 0, 0)
        Me.lblOptionsStep.Size = New System.Drawing.Size(755, 27)
        Me.lblOptionsStep.TabIndex = 21
        Me.lblOptionsStep.Text = "STEP 3  |  Schedule and save"
        '
        'lblLastBackups
        '
        Me.lblLastBackups.ForeColor = System.Drawing.Color.FromArgb(CType(CType(65, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(90, Byte), Integer))
        Me.lblLastBackups.Location = New System.Drawing.Point(30, 630)
        Me.lblLastBackups.Name = "lblLastBackups"
        Me.lblLastBackups.Size = New System.Drawing.Size(735, 20)
        Me.lblLastBackups.TabIndex = 25
        Me.lblLastBackups.Text = "Last successful uploads:"
        '
        'lblSchedule
        '
        Me.lblSchedule.Location = New System.Drawing.Point(30, 599)
        Me.lblSchedule.Name = "lblSchedule"
        Me.lblSchedule.Size = New System.Drawing.Size(120, 26)
        Me.lblSchedule.TabIndex = 22
        Me.lblSchedule.Text = "Backup schedule"
        Me.lblSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CloudBackupConfiguration
        '
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(800, 726)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblIntro)
        Me.Controls.Add(Me.lblGoogleStep)
        Me.Controls.Add(Me.lblGoogleHelp)
        Me.Controls.Add(Me.btnGoogleConnect)
        Me.Controls.Add(Me.btnEnableDriveApi)
        Me.Controls.Add(Me.chkGoogle)
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtGoogleEmail)
        Me.Controls.Add(Me.btnImportGoogleJson)
        Me.Controls.Add(Me.btnGoogleConnectAccount)
        Me.Controls.Add(Me.lblGoogleStatus)
        Me.Controls.Add(Me.lblDriveStep)
        Me.Controls.Add(Me.lblDriveHelp)
        Me.Controls.Add(Me.chkOneDrive)
        Me.Controls.Add(Me.btnOneDriveLogin)
        Me.Controls.Add(Me.lblOneDrivePath)
        Me.Controls.Add(Me.txtOneDrivePath)
        Me.Controls.Add(Me.btnDetectOneDrive)
        Me.Controls.Add(Me.btnBrowseOneDrive)
        Me.Controls.Add(Me.lblOptionsStep)
        Me.Controls.Add(Me.lblSchedule)
        Me.Controls.Add(Me.cbSchedule)
        Me.Controls.Add(Me.chkSyncOnClose)
        Me.Controls.Add(Me.lblLastBackups)
        Me.Controls.Add(Me.lblLastGoogle)
        Me.Controls.Add(Me.lblLastOneDrive)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnUserManual)
        Me.Controls.Add(Me.lblStatus)
        Me.btnClose.BringToFront()
        Me.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "CloudBackupConfiguration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Cloud Backup Setup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class
