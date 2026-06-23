<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WhatsApp_API
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WhatsApp_API))
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbMethod = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbLanguage = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cbmsgType = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lnkMsgzLogin = New System.Windows.Forms.LinkLabel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtMsgAccess = New System.Windows.Forms.TextBox()
        Me.btnVerifyMsgAccess = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbDefaultSim = New System.Windows.Forms.ComboBox()
        Me.GbWhatsappAPI = New System.Windows.Forms.GroupBox()
        Me.lnkOfficialLogin = New System.Windows.Forms.LinkLabel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtInstanceID = New System.Windows.Forms.TextBox()
        Me.btnCopyVendorId = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtAccessToken = New System.Windows.Forms.TextBox()
        Me.btnCopyAccessToken = New System.Windows.Forms.Button()
        Me.btnReconnect = New System.Windows.Forms.Button()
        Me.btnSyncTemplates = New System.Windows.Forms.Button()
        Me.lblTestMobile = New System.Windows.Forms.Label()
        Me.txtTestMobile = New System.Windows.Forms.TextBox()
        Me.btnSendTest = New System.Windows.Forms.Button()
        Me.btnTemplateEditor = New System.Windows.Forms.Button()
        Me.btnMessageLogs = New System.Windows.Forms.Button()
        Me.btnClearOfficialApi = New System.Windows.Forms.Button()
        Me.lblBusinessInfo = New System.Windows.Forms.Label()
        Me.lblTemplates = New System.Windows.Forms.Label()
        Me.tabTemplates = New System.Windows.Forms.TabControl()
        Me.tabLocalTemplates = New System.Windows.Forms.TabPage()
        Me.tabApprovedTemplates = New System.Windows.Forms.TabPage()
        Me.tabPendingTemplates = New System.Windows.Forms.TabPage()
        Me.tabRejectedTemplates = New System.Windows.Forms.TabPage()
        Me.dgvTemplates = New System.Windows.Forms.DataGridView()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GbWhatsappAPI.SuspendLayout()
        Me.tabTemplates.SuspendLayout()
        CType(Me.dgvTemplates, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 30.0!)
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(384, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(403, 46)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "WhatsApp Configration"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.SystemColors.Control
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Location = New System.Drawing.Point(1123, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(72, 47)
        Me.btnClose.TabIndex = 91122
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "CLOSE"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 20.25!)
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(34, 57)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(273, 31)
        Me.Label5.TabIndex = 91123
        Me.Label5.Text = "Default Sending Setting"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.cbMethod)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cbLanguage)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.cbmsgType)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 91)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(601, 136)
        Me.GroupBox1.TabIndex = 91124
        Me.GroupBox1.TabStop = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.DarkTurquoise
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Button1.ForeColor = System.Drawing.Color.GhostWhite
        Me.Button1.Location = New System.Drawing.Point(450, 93)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(141, 30)
        Me.Button1.TabIndex = 91229
        Me.Button1.Text = "Save &Default"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.Label10.Location = New System.Drawing.Point(30, 24)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(109, 19)
        Me.Label10.TabIndex = 91195
        Me.Label10.Text = "Sending Method"
        '
        'cbMethod
        '
        Me.cbMethod.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.cbMethod.DropDownHeight = 100
        Me.cbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbMethod.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cbMethod.FormattingEnabled = True
        Me.cbMethod.IntegralHeight = False
        Me.cbMethod.Items.AddRange(New Object() {"Easy WhatsApp", "WhatsApp Official API", "From Mobile"})
        Me.cbMethod.Location = New System.Drawing.Point(28, 50)
        Me.cbMethod.Name = "cbMethod"
        Me.cbMethod.Size = New System.Drawing.Size(201, 27)
        Me.cbMethod.TabIndex = 91196
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.Label6.Location = New System.Drawing.Point(239, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 19)
        Me.Label6.TabIndex = 91125
        Me.Label6.Text = "Pdf Langunage"
        '
        'cbLanguage
        '
        Me.cbLanguage.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.cbLanguage.DropDownHeight = 100
        Me.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbLanguage.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cbLanguage.FormattingEnabled = True
        Me.cbLanguage.IntegralHeight = False
        Me.cbLanguage.Items.AddRange(New Object() {"English", "Regional"})
        Me.cbLanguage.Location = New System.Drawing.Point(237, 50)
        Me.cbLanguage.Name = "cbLanguage"
        Me.cbLanguage.Size = New System.Drawing.Size(166, 27)
        Me.cbLanguage.TabIndex = 91192
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.Label7.Location = New System.Drawing.Point(409, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 19)
        Me.Label7.TabIndex = 91194
        Me.Label7.Text = "Pdf Type + Msg"
        '
        'cbmsgType
        '
        Me.cbmsgType.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.cbmsgType.DropDownHeight = 100
        Me.cbmsgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbmsgType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbmsgType.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cbmsgType.FormattingEnabled = True
        Me.cbmsgType.IntegralHeight = False
        Me.cbmsgType.Items.AddRange(New Object() {"Pdf + Msg", "Pdf Only", "Msg Only"})
        Me.cbmsgType.Location = New System.Drawing.Point(411, 50)
        Me.cbmsgType.Name = "cbmsgType"
        Me.cbmsgType.Size = New System.Drawing.Size(180, 27)
        Me.cbmsgType.TabIndex = 91193
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.PictureBox1)
        Me.GroupBox2.Controls.Add(Me.lnkMsgzLogin)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtMsgAccess)
        Me.GroupBox2.Controls.Add(Me.btnVerifyMsgAccess)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.cbDefaultSim)
        Me.GroupBox2.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.GroupBox2.ForeColor = System.Drawing.Color.Green
        Me.GroupBox2.Location = New System.Drawing.Point(619, 91)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(565, 174)
        Me.GroupBox2.TabIndex = 91232
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Send From Mobile Messages"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(116, 136)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(427, 19)
        Me.Label9.TabIndex = 91249
        Me.Label9.Text = "Scan QR to Download msgz Apk File From your Android Phone"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(6, 59)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(104, 109)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 91251
        Me.PictureBox1.TabStop = False
        '
        'lnkMsgzLogin
        '
        Me.lnkMsgzLogin.ActiveLinkColor = System.Drawing.Color.Red
        Me.lnkMsgzLogin.AutoSize = True
        Me.lnkMsgzLogin.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lnkMsgzLogin.LinkColor = System.Drawing.Color.Navy
        Me.lnkMsgzLogin.Location = New System.Drawing.Point(414, 0)
        Me.lnkMsgzLogin.Name = "lnkMsgzLogin"
        Me.lnkMsgzLogin.Size = New System.Drawing.Size(131, 17)
        Me.lnkMsgzLogin.TabIndex = 91250
        Me.lnkMsgzLogin.TabStop = True
        Me.lnkMsgzLogin.Text = "Open msgz.in Login"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label4.Location = New System.Drawing.Point(7, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(126, 21)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Access Token :"
        '
        'txtMsgAccess
        '
        Me.txtMsgAccess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMsgAccess.Font = New System.Drawing.Font("Times New Roman", 14.25!)
        Me.txtMsgAccess.Location = New System.Drawing.Point(135, 25)
        Me.txtMsgAccess.Name = "txtMsgAccess"
        Me.txtMsgAccess.Size = New System.Drawing.Size(424, 29)
        Me.txtMsgAccess.TabIndex = 13
        Me.txtMsgAccess.TabStop = False
        '
        'btnVerifyMsgAccess
        '
        Me.btnVerifyMsgAccess.BackColor = System.Drawing.Color.Teal
        Me.btnVerifyMsgAccess.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnVerifyMsgAccess.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerifyMsgAccess.ForeColor = System.Drawing.Color.White
        Me.btnVerifyMsgAccess.Location = New System.Drawing.Point(460, 59)
        Me.btnVerifyMsgAccess.Name = "btnVerifyMsgAccess"
        Me.btnVerifyMsgAccess.Size = New System.Drawing.Size(100, 29)
        Me.btnVerifyMsgAccess.TabIndex = 91233
        Me.btnVerifyMsgAccess.Text = "Verify"
        Me.btnVerifyMsgAccess.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label8.Location = New System.Drawing.Point(130, 67)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(106, 21)
        Me.Label8.TabIndex = 91231
        Me.Label8.Text = "Default Sim :"
        '
        'cbDefaultSim
        '
        Me.cbDefaultSim.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.cbDefaultSim.DropDownHeight = 100
        Me.cbDefaultSim.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDefaultSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbDefaultSim.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cbDefaultSim.FormattingEnabled = True
        Me.cbDefaultSim.IntegralHeight = False
        Me.cbDefaultSim.Location = New System.Drawing.Point(242, 63)
        Me.cbDefaultSim.Name = "cbDefaultSim"
        Me.cbDefaultSim.Size = New System.Drawing.Size(212, 27)
        Me.cbDefaultSim.TabIndex = 91230
        '
        'GbWhatsappAPI
        '
        Me.GbWhatsappAPI.Controls.Add(Me.lnkOfficialLogin)
        Me.GbWhatsappAPI.Controls.Add(Me.Label1)
        Me.GbWhatsappAPI.Controls.Add(Me.TxtInstanceID)
        Me.GbWhatsappAPI.Controls.Add(Me.btnCopyVendorId)
        Me.GbWhatsappAPI.Controls.Add(Me.Label2)
        Me.GbWhatsappAPI.Controls.Add(Me.txtAccessToken)
        Me.GbWhatsappAPI.Controls.Add(Me.btnCopyAccessToken)
        Me.GbWhatsappAPI.Controls.Add(Me.btnReconnect)
        Me.GbWhatsappAPI.Controls.Add(Me.btnSyncTemplates)
        Me.GbWhatsappAPI.Controls.Add(Me.lblTestMobile)
        Me.GbWhatsappAPI.Controls.Add(Me.txtTestMobile)
        Me.GbWhatsappAPI.Controls.Add(Me.btnSendTest)
        Me.GbWhatsappAPI.Controls.Add(Me.btnTemplateEditor)
        Me.GbWhatsappAPI.Controls.Add(Me.btnMessageLogs)
        Me.GbWhatsappAPI.Controls.Add(Me.btnClearOfficialApi)
        Me.GbWhatsappAPI.Controls.Add(Me.lblBusinessInfo)
        Me.GbWhatsappAPI.Controls.Add(Me.lblTemplates)
        Me.GbWhatsappAPI.Controls.Add(Me.tabTemplates)
        Me.GbWhatsappAPI.Controls.Add(Me.dgvTemplates)
        Me.GbWhatsappAPI.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.GbWhatsappAPI.ForeColor = System.Drawing.Color.Green
        Me.GbWhatsappAPI.Location = New System.Drawing.Point(12, 271)
        Me.GbWhatsappAPI.Name = "GbWhatsappAPI"
        Me.GbWhatsappAPI.Size = New System.Drawing.Size(1172, 370)
        Me.GbWhatsappAPI.TabIndex = 91231
        Me.GbWhatsappAPI.TabStop = False
        Me.GbWhatsappAPI.Text = "WhatsApp API official (Paid) "
        '
        'lnkOfficialLogin
        '
        Me.lnkOfficialLogin.ActiveLinkColor = System.Drawing.Color.Red
        Me.lnkOfficialLogin.AutoSize = True
        Me.lnkOfficialLogin.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lnkOfficialLogin.LinkColor = System.Drawing.Color.Navy
        Me.lnkOfficialLogin.Location = New System.Drawing.Point(461, 12)
        Me.lnkOfficialLogin.Name = "lnkOfficialLogin"
        Me.lnkOfficialLogin.Size = New System.Drawing.Size(130, 17)
        Me.lnkOfficialLogin.TabIndex = 91248
        Me.lnkOfficialLogin.TabStop = True
        Me.lnkOfficialLogin.Text = "Open Official Login"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label1.Location = New System.Drawing.Point(32, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 21)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Vendor ID :"
        '
        'TxtInstanceID
        '
        Me.TxtInstanceID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtInstanceID.Font = New System.Drawing.Font("Times New Roman", 14.25!)
        Me.TxtInstanceID.Location = New System.Drawing.Point(135, 31)
        Me.TxtInstanceID.Name = "TxtInstanceID"
        Me.TxtInstanceID.Size = New System.Drawing.Size(386, 29)
        Me.TxtInstanceID.TabIndex = 0
        Me.TxtInstanceID.TabStop = False
        '
        'btnCopyVendorId
        '
        Me.btnCopyVendorId.BackColor = System.Drawing.Color.SteelBlue
        Me.btnCopyVendorId.FlatAppearance.BorderSize = 0
        Me.btnCopyVendorId.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyVendorId.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnCopyVendorId.ForeColor = System.Drawing.Color.White
        Me.btnCopyVendorId.Location = New System.Drawing.Point(526, 31)
        Me.btnCopyVendorId.Name = "btnCopyVendorId"
        Me.btnCopyVendorId.Size = New System.Drawing.Size(69, 29)
        Me.btnCopyVendorId.TabIndex = 91252
        Me.btnCopyVendorId.Text = "Copy"
        Me.btnCopyVendorId.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label2.Location = New System.Drawing.Point(7, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 21)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Access Token :"
        '
        'txtAccessToken
        '
        Me.txtAccessToken.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccessToken.Font = New System.Drawing.Font("Times New Roman", 14.25!)
        Me.txtAccessToken.Location = New System.Drawing.Point(135, 66)
        Me.txtAccessToken.Name = "txtAccessToken"
        Me.txtAccessToken.Size = New System.Drawing.Size(386, 29)
        Me.txtAccessToken.TabIndex = 13
        Me.txtAccessToken.TabStop = False
        Me.txtAccessToken.UseSystemPasswordChar = True
        '
        'btnCopyAccessToken
        '
        Me.btnCopyAccessToken.BackColor = System.Drawing.Color.SteelBlue
        Me.btnCopyAccessToken.FlatAppearance.BorderSize = 0
        Me.btnCopyAccessToken.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyAccessToken.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnCopyAccessToken.ForeColor = System.Drawing.Color.White
        Me.btnCopyAccessToken.Location = New System.Drawing.Point(526, 66)
        Me.btnCopyAccessToken.Name = "btnCopyAccessToken"
        Me.btnCopyAccessToken.Size = New System.Drawing.Size(69, 29)
        Me.btnCopyAccessToken.TabIndex = 91253
        Me.btnCopyAccessToken.Text = "Copy"
        Me.btnCopyAccessToken.UseVisualStyleBackColor = False
        '
        'btnReconnect
        '
        Me.btnReconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnReconnect.FlatAppearance.BorderSize = 0
        Me.btnReconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReconnect.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.btnReconnect.ForeColor = System.Drawing.Color.White
        Me.btnReconnect.Location = New System.Drawing.Point(327, 101)
        Me.btnReconnect.Name = "btnReconnect"
        Me.btnReconnect.Size = New System.Drawing.Size(268, 37)
        Me.btnReconnect.TabIndex = 12
        Me.btnReconnect.Text = "Whatsapp Connect With Facebook"
        Me.btnReconnect.UseVisualStyleBackColor = False
        '
        'btnSyncTemplates
        '
        Me.btnSyncTemplates.BackColor = System.Drawing.Color.DarkCyan
        Me.btnSyncTemplates.FlatAppearance.BorderSize = 0
        Me.btnSyncTemplates.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSyncTemplates.Font = New System.Drawing.Font("Times New Roman", 11.0!)
        Me.btnSyncTemplates.ForeColor = System.Drawing.Color.White
        Me.btnSyncTemplates.Location = New System.Drawing.Point(1045, 10)
        Me.btnSyncTemplates.Name = "btnSyncTemplates"
        Me.btnSyncTemplates.Size = New System.Drawing.Size(110, 31)
        Me.btnSyncTemplates.TabIndex = 91243
        Me.btnSyncTemplates.Text = "Sync Templates"
        Me.btnSyncTemplates.UseVisualStyleBackColor = False
        '
        'lblTestMobile
        '
        Me.lblTestMobile.AutoSize = True
        Me.lblTestMobile.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.lblTestMobile.ForeColor = System.Drawing.Color.Navy
        Me.lblTestMobile.Location = New System.Drawing.Point(17, 108)
        Me.lblTestMobile.Name = "lblTestMobile"
        Me.lblTestMobile.Size = New System.Drawing.Size(112, 19)
        Me.lblTestMobile.TabIndex = 91241
        Me.lblTestMobile.Text = "Test Mobile No :"
        '
        'txtTestMobile
        '
        Me.txtTestMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTestMobile.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.txtTestMobile.Location = New System.Drawing.Point(135, 101)
        Me.txtTestMobile.Name = "txtTestMobile"
        Me.txtTestMobile.Size = New System.Drawing.Size(185, 26)
        Me.txtTestMobile.TabIndex = 91242
        '
        'btnSendTest
        '
        Me.btnSendTest.BackColor = System.Drawing.Color.SeaGreen
        Me.btnSendTest.FlatAppearance.BorderSize = 0
        Me.btnSendTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSendTest.Font = New System.Drawing.Font("Times New Roman", 11.0!)
        Me.btnSendTest.ForeColor = System.Drawing.Color.White
        Me.btnSendTest.Location = New System.Drawing.Point(327, 142)
        Me.btnSendTest.Name = "btnSendTest"
        Me.btnSendTest.Size = New System.Drawing.Size(268, 31)
        Me.btnSendTest.TabIndex = 91244
        Me.btnSendTest.Text = "Send Test Message"
        Me.btnSendTest.UseVisualStyleBackColor = False
        '
        'btnTemplateEditor
        '
        Me.btnTemplateEditor.BackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnTemplateEditor.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnTemplateEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTemplateEditor.Font = New System.Drawing.Font("Times New Roman", 11.0!)
        Me.btnTemplateEditor.ForeColor = System.Drawing.Color.White
        Me.btnTemplateEditor.Location = New System.Drawing.Point(932, 10)
        Me.btnTemplateEditor.Name = "btnTemplateEditor"
        Me.btnTemplateEditor.Size = New System.Drawing.Size(112, 31)
        Me.btnTemplateEditor.TabIndex = 91247
        Me.btnTemplateEditor.Text = "Template Editor"
        Me.btnTemplateEditor.UseVisualStyleBackColor = False
        '
        'btnMessageLogs
        '
        Me.btnMessageLogs.BackColor = System.Drawing.Color.SteelBlue
        Me.btnMessageLogs.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnMessageLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMessageLogs.Font = New System.Drawing.Font("Times New Roman", 11.0!)
        Me.btnMessageLogs.ForeColor = System.Drawing.Color.White
        Me.btnMessageLogs.Location = New System.Drawing.Point(176, 141)
        Me.btnMessageLogs.Name = "btnMessageLogs"
        Me.btnMessageLogs.Size = New System.Drawing.Size(126, 31)
        Me.btnMessageLogs.TabIndex = 91252
        Me.btnMessageLogs.Text = "Message Logs"
        Me.btnMessageLogs.UseVisualStyleBackColor = False
        '
        'btnClearOfficialApi
        '
        Me.btnClearOfficialApi.BackColor = System.Drawing.Color.Firebrick
        Me.btnClearOfficialApi.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnClearOfficialApi.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClearOfficialApi.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnClearOfficialApi.ForeColor = System.Drawing.Color.White
        Me.btnClearOfficialApi.Location = New System.Drawing.Point(11, 141)
        Me.btnClearOfficialApi.Name = "btnClearOfficialApi"
        Me.btnClearOfficialApi.Size = New System.Drawing.Size(158, 31)
        Me.btnClearOfficialApi.TabIndex = 91254
        Me.btnClearOfficialApi.Text = "Reset Official API"
        Me.btnClearOfficialApi.UseVisualStyleBackColor = False
        '
        'lblBusinessInfo
        '
        Me.lblBusinessInfo.BackColor = System.Drawing.Color.AliceBlue
        Me.lblBusinessInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblBusinessInfo.Font = New System.Drawing.Font("Times New Roman", 11.0!)
        Me.lblBusinessInfo.ForeColor = System.Drawing.Color.Navy
        Me.lblBusinessInfo.Location = New System.Drawing.Point(11, 185)
        Me.lblBusinessInfo.Name = "lblBusinessInfo"
        Me.lblBusinessInfo.Padding = New System.Windows.Forms.Padding(8)
        Me.lblBusinessInfo.Size = New System.Drawing.Size(584, 165)
        Me.lblBusinessInfo.TabIndex = 91240
        Me.lblBusinessInfo.Text = "Status: Not connected"
        '
        'lblTemplates
        '
        Me.lblTemplates.AutoSize = True
        Me.lblTemplates.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTemplates.ForeColor = System.Drawing.Color.Navy
        Me.lblTemplates.Location = New System.Drawing.Point(620, -2)
        Me.lblTemplates.Name = "lblTemplates"
        Me.lblTemplates.Size = New System.Drawing.Size(154, 19)
        Me.lblTemplates.TabIndex = 91245
        Me.lblTemplates.Text = "Predefined Templates"
        '
        'tabTemplates
        '
        Me.tabTemplates.Controls.Add(Me.tabLocalTemplates)
        Me.tabTemplates.Controls.Add(Me.tabApprovedTemplates)
        Me.tabTemplates.Controls.Add(Me.tabPendingTemplates)
        Me.tabTemplates.Controls.Add(Me.tabRejectedTemplates)
        Me.tabTemplates.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.tabTemplates.Location = New System.Drawing.Point(607, 17)
        Me.tabTemplates.Name = "tabTemplates"
        Me.tabTemplates.SelectedIndex = 0
        Me.tabTemplates.Size = New System.Drawing.Size(548, 28)
        Me.tabTemplates.TabIndex = 91253
        '
        'tabLocalTemplates
        '
        Me.tabLocalTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabLocalTemplates.Name = "tabLocalTemplates"
        Me.tabLocalTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLocalTemplates.Size = New System.Drawing.Size(540, 0)
        Me.tabLocalTemplates.TabIndex = 0
        Me.tabLocalTemplates.Text = "Local"
        Me.tabLocalTemplates.UseVisualStyleBackColor = True
        '
        'tabApprovedTemplates
        '
        Me.tabApprovedTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabApprovedTemplates.Name = "tabApprovedTemplates"
        Me.tabApprovedTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabApprovedTemplates.Size = New System.Drawing.Size(540, 0)
        Me.tabApprovedTemplates.TabIndex = 1
        Me.tabApprovedTemplates.Text = "Approved"
        Me.tabApprovedTemplates.UseVisualStyleBackColor = True
        '
        'tabPendingTemplates
        '
        Me.tabPendingTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabPendingTemplates.Name = "tabPendingTemplates"
        Me.tabPendingTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPendingTemplates.Size = New System.Drawing.Size(540, 0)
        Me.tabPendingTemplates.TabIndex = 2
        Me.tabPendingTemplates.Text = "Pending"
        Me.tabPendingTemplates.UseVisualStyleBackColor = True
        '
        'tabRejectedTemplates
        '
        Me.tabRejectedTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabRejectedTemplates.Name = "tabRejectedTemplates"
        Me.tabRejectedTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabRejectedTemplates.Size = New System.Drawing.Size(540, 0)
        Me.tabRejectedTemplates.TabIndex = 3
        Me.tabRejectedTemplates.Text = "Rejected"
        Me.tabRejectedTemplates.UseVisualStyleBackColor = True
        '
        'dgvTemplates
        '
        Me.dgvTemplates.AllowUserToAddRows = False
        Me.dgvTemplates.AllowUserToDeleteRows = False
        Me.dgvTemplates.AllowUserToResizeRows = False
        Me.dgvTemplates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTemplates.BackgroundColor = System.Drawing.Color.White
        Me.dgvTemplates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTemplates.Location = New System.Drawing.Point(607, 42)
        Me.dgvTemplates.MultiSelect = False
        Me.dgvTemplates.Name = "dgvTemplates"
        Me.dgvTemplates.ReadOnly = True
        Me.dgvTemplates.RowHeadersVisible = False
        Me.dgvTemplates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTemplates.Size = New System.Drawing.Size(548, 308)
        Me.dgvTemplates.TabIndex = 91246
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'WhatsApp_API
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GbWhatsappAPI)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "WhatsApp_API"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "WhatsApp_API"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GbWhatsappAPI.ResumeLayout(False)
        Me.GbWhatsappAPI.PerformLayout()
        Me.tabTemplates.ResumeLayout(False)
        CType(Me.dgvTemplates, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbMethod As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbmsgType As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lnkMsgzLogin As System.Windows.Forms.LinkLabel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtMsgAccess As System.Windows.Forms.TextBox
    Friend WithEvents btnVerifyMsgAccess As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbDefaultSim As System.Windows.Forms.ComboBox
    Friend WithEvents GbWhatsappAPI As System.Windows.Forms.GroupBox
    Friend WithEvents lnkOfficialLogin As System.Windows.Forms.LinkLabel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtInstanceID As System.Windows.Forms.TextBox
    Friend WithEvents btnCopyVendorId As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtAccessToken As System.Windows.Forms.TextBox
    Friend WithEvents btnCopyAccessToken As System.Windows.Forms.Button
    Friend WithEvents btnReconnect As System.Windows.Forms.Button
    Friend WithEvents btnSyncTemplates As System.Windows.Forms.Button
    Friend WithEvents lblTestMobile As System.Windows.Forms.Label
    Friend WithEvents txtTestMobile As System.Windows.Forms.TextBox
    Friend WithEvents btnSendTest As System.Windows.Forms.Button
    Friend WithEvents btnTemplateEditor As System.Windows.Forms.Button
    Friend WithEvents btnMessageLogs As System.Windows.Forms.Button
    Friend WithEvents btnClearOfficialApi As System.Windows.Forms.Button
    Friend WithEvents lblBusinessInfo As System.Windows.Forms.Label
    Friend WithEvents lblTemplates As System.Windows.Forms.Label
    Friend WithEvents tabTemplates As System.Windows.Forms.TabControl
    Friend WithEvents tabLocalTemplates As System.Windows.Forms.TabPage
    Friend WithEvents tabApprovedTemplates As System.Windows.Forms.TabPage
    Friend WithEvents tabPendingTemplates As System.Windows.Forms.TabPage
    Friend WithEvents tabRejectedTemplates As System.Windows.Forms.TabPage
    Friend WithEvents dgvTemplates As System.Windows.Forms.DataGridView
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class

