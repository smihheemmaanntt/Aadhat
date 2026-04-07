<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WhatsApp_API
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WhatsApp_API))
        Me.TxtInstanceID = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnReconnect = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbMethod = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cbmsgType = New System.Windows.Forms.ComboBox()
        Me.cbLanguage = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GbWhatsappAPI = New System.Windows.Forms.GroupBox()
        Me.txtAccessToken = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cbDefaultSim = New System.Windows.Forms.ComboBox()
        Me.txtMsgAccess = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GbWhatsappAPI.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtInstanceID
        '
        Me.TxtInstanceID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtInstanceID.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtInstanceID.Location = New System.Drawing.Point(135, 31)
        Me.TxtInstanceID.Name = "TxtInstanceID"
        Me.TxtInstanceID.Size = New System.Drawing.Size(460, 29)
        Me.TxtInstanceID.TabIndex = 0
        Me.TxtInstanceID.TabStop = False
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
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(384, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(403, 46)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "WhatsApp Configration"
        '
        'btnReconnect
        '
        Me.btnReconnect.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnReconnect.FlatAppearance.BorderSize = 0
        Me.btnReconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReconnect.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReconnect.ForeColor = System.Drawing.Color.White
        Me.btnReconnect.Location = New System.Drawing.Point(327, 101)
        Me.btnReconnect.Name = "btnReconnect"
        Me.btnReconnect.Size = New System.Drawing.Size(268, 37)
        Me.btnReconnect.TabIndex = 12
        Me.btnReconnect.Text = "Whatsapp Connect With Facebook"
        Me.btnReconnect.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(43, 95)
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
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.cbmsgType)
        Me.GroupBox1.Controls.Add(Me.cbLanguage)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 129)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(601, 136)
        Me.GroupBox1.TabIndex = 91124
        Me.GroupBox1.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
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
        Me.cbMethod.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbMethod.ForeColor = System.Drawing.Color.Black
        Me.cbMethod.FormattingEnabled = True
        Me.cbMethod.IntegralHeight = False
        Me.cbMethod.Items.AddRange(New Object() {"Easy WhatsApp", "WhatsApp Official API", "From Mobile"})
        Me.cbMethod.Location = New System.Drawing.Point(28, 50)
        Me.cbMethod.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbMethod.Name = "cbMethod"
        Me.cbMethod.Size = New System.Drawing.Size(201, 27)
        Me.cbMethod.TabIndex = 91196
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(409, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(108, 19)
        Me.Label7.TabIndex = 91194
        Me.Label7.Text = "Pdf Type + Msg"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(239, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 19)
        Me.Label6.TabIndex = 91125
        Me.Label6.Text = "Pdf Langunage"
        '
        'cbmsgType
        '
        Me.cbmsgType.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.cbmsgType.DropDownHeight = 100
        Me.cbmsgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbmsgType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbmsgType.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbmsgType.ForeColor = System.Drawing.Color.Black
        Me.cbmsgType.FormattingEnabled = True
        Me.cbmsgType.IntegralHeight = False
        Me.cbmsgType.Items.AddRange(New Object() {"Pdf + Msg", "Pdf Only", "Msg Only"})
        Me.cbmsgType.Location = New System.Drawing.Point(411, 50)
        Me.cbmsgType.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbmsgType.Name = "cbmsgType"
        Me.cbmsgType.Size = New System.Drawing.Size(180, 27)
        Me.cbmsgType.TabIndex = 91193
        '
        'cbLanguage
        '
        Me.cbLanguage.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.cbLanguage.DropDownHeight = 100
        Me.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbLanguage.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbLanguage.ForeColor = System.Drawing.Color.Black
        Me.cbLanguage.FormattingEnabled = True
        Me.cbLanguage.IntegralHeight = False
        Me.cbLanguage.Items.AddRange(New Object() {"English", "Regional"})
        Me.cbLanguage.Location = New System.Drawing.Point(237, 50)
        Me.cbLanguage.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbLanguage.Name = "cbLanguage"
        Me.cbLanguage.Size = New System.Drawing.Size(166, 27)
        Me.cbLanguage.TabIndex = 91192
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.DarkTurquoise
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Button1.ForeColor = System.Drawing.Color.GhostWhite
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.Location = New System.Drawing.Point(450, 93)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(141, 30)
        Me.Button1.TabIndex = 91229
        Me.Button1.Text = "Save &Default"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.SystemColors.Control
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(1142, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 47)
        Me.btnClose.TabIndex = 91122
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'GbWhatsappAPI
        '
        Me.GbWhatsappAPI.Controls.Add(Me.txtAccessToken)
        Me.GbWhatsappAPI.Controls.Add(Me.Label2)
        Me.GbWhatsappAPI.Controls.Add(Me.TxtInstanceID)
        Me.GbWhatsappAPI.Controls.Add(Me.Label1)
        Me.GbWhatsappAPI.Controls.Add(Me.btnReconnect)
        Me.GbWhatsappAPI.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GbWhatsappAPI.ForeColor = System.Drawing.Color.Green
        Me.GbWhatsappAPI.Location = New System.Drawing.Point(12, 271)
        Me.GbWhatsappAPI.Name = "GbWhatsappAPI"
        Me.GbWhatsappAPI.Size = New System.Drawing.Size(601, 370)
        Me.GbWhatsappAPI.TabIndex = 91231
        Me.GbWhatsappAPI.TabStop = False
        Me.GbWhatsappAPI.Text = "WhatsApp API official (Paid) "
        Me.GbWhatsappAPI.Visible = False
        '
        'txtAccessToken
        '
        Me.txtAccessToken.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccessToken.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccessToken.Location = New System.Drawing.Point(135, 66)
        Me.txtAccessToken.Name = "txtAccessToken"
        Me.txtAccessToken.Size = New System.Drawing.Size(460, 29)
        Me.txtAccessToken.TabIndex = 13
        Me.txtAccessToken.TabStop = False
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
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.cbDefaultSim)
        Me.GroupBox2.Controls.Add(Me.txtMsgAccess)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.ForeColor = System.Drawing.Color.Green
        Me.GroupBox2.Location = New System.Drawing.Point(619, 129)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(565, 97)
        Me.GroupBox2.TabIndex = 91232
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Send From Mobile Messages"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label8.Location = New System.Drawing.Point(26, 67)
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
        Me.cbDefaultSim.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbDefaultSim.ForeColor = System.Drawing.Color.Black
        Me.cbDefaultSim.FormattingEnabled = True
        Me.cbDefaultSim.IntegralHeight = False
        Me.cbDefaultSim.Items.AddRange(New Object() {"SIM 1", "SIM 2"})
        Me.cbDefaultSim.Location = New System.Drawing.Point(136, 63)
        Me.cbDefaultSim.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.cbDefaultSim.Name = "cbDefaultSim"
        Me.cbDefaultSim.Size = New System.Drawing.Size(180, 27)
        Me.cbDefaultSim.TabIndex = 91230
        '
        'txtMsgAccess
        '
        Me.txtMsgAccess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMsgAccess.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMsgAccess.Location = New System.Drawing.Point(135, 25)
        Me.txtMsgAccess.Name = "txtMsgAccess"
        Me.txtMsgAccess.Size = New System.Drawing.Size(424, 29)
        Me.txtMsgAccess.TabIndex = 13
        Me.txtMsgAccess.TabStop = False
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
        Me.Text = "WhatsApp_API"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GbWhatsappAPI.ResumeLayout(False)
        Me.GbWhatsappAPI.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtInstanceID As TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnReconnect As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbmsgType As System.Windows.Forms.ComboBox
    Friend WithEvents cbLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbMethod As System.Windows.Forms.ComboBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents GbWhatsappAPI As System.Windows.Forms.GroupBox
    Friend WithEvents txtAccessToken As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMsgAccess As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cbDefaultSim As System.Windows.Forms.ComboBox
End Class
