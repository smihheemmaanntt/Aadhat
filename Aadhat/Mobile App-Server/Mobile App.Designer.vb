<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Mobile_App
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Mobile_App))
        Me.txtCompanyID = New System.Windows.Forms.TextBox()
        Me.BtnIDGenrate = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CkShowPassword = New System.Windows.Forms.CheckBox()
        Me.dataProgress = New System.Windows.Forms.ProgressBar()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.btnCustom = New System.Windows.Forms.Button()
        Me.lblLastUpdate = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ckFullSync = New System.Windows.Forms.CheckBox()
        Me.lblLink = New System.Windows.Forms.LinkLabel()
        Me.pnlLock = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtAuthPathPassword = New System.Windows.Forms.TextBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLock.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtCompanyID
        '
        Me.txtCompanyID.BackColor = System.Drawing.Color.GhostWhite
        Me.txtCompanyID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCompanyID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtCompanyID.ForeColor = System.Drawing.Color.Teal
        Me.txtCompanyID.Location = New System.Drawing.Point(232, 156)
        Me.txtCompanyID.Name = "txtCompanyID"
        Me.txtCompanyID.ReadOnly = True
        Me.txtCompanyID.Size = New System.Drawing.Size(200, 26)
        Me.txtCompanyID.TabIndex = 1
        '
        'BtnIDGenrate
        '
        Me.BtnIDGenrate.BackColor = System.Drawing.Color.DarkTurquoise
        Me.BtnIDGenrate.FlatAppearance.BorderSize = 0
        Me.BtnIDGenrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnIDGenrate.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BtnIDGenrate.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnIDGenrate.Location = New System.Drawing.Point(227, 25)
        Me.BtnIDGenrate.Name = "BtnIDGenrate"
        Me.BtnIDGenrate.Size = New System.Drawing.Size(208, 44)
        Me.BtnIDGenrate.TabIndex = 0
        Me.BtnIDGenrate.Text = "&Genrate Organization ID"
        Me.BtnIDGenrate.UseVisualStyleBackColor = False
        '
        'txtPassword
        '
        Me.txtPassword.BackColor = System.Drawing.Color.GhostWhite
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtPassword.ForeColor = System.Drawing.Color.Teal
        Me.txtPassword.Location = New System.Drawing.Point(233, 210)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(199, 26)
        Me.txtPassword.TabIndex = 2
        Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.GhostWhite
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Teal
        Me.Label6.Location = New System.Drawing.Point(263, 133)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 15)
        Me.Label6.TabIndex = 124
        Me.Label6.Text = "Your Organisation"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.GhostWhite
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Teal
        Me.Label1.Location = New System.Drawing.Point(273, 190)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(87, 15)
        Me.Label1.TabIndex = 125
        Me.Label1.Text = "Your Password"
        '
        'CkShowPassword
        '
        Me.CkShowPassword.AutoSize = True
        Me.CkShowPassword.BackColor = System.Drawing.Color.GhostWhite
        Me.CkShowPassword.Font = New System.Drawing.Font("Times New Roman", 8.0!)
        Me.CkShowPassword.ForeColor = System.Drawing.Color.Red
        Me.CkShowPassword.Location = New System.Drawing.Point(321, 243)
        Me.CkShowPassword.Name = "CkShowPassword"
        Me.CkShowPassword.Size = New System.Drawing.Size(95, 18)
        Me.CkShowPassword.TabIndex = 3
        Me.CkShowPassword.Text = "&Show Password"
        Me.CkShowPassword.UseVisualStyleBackColor = False
        '
        'dataProgress
        '
        Me.dataProgress.BackColor = System.Drawing.Color.Red
        Me.dataProgress.Location = New System.Drawing.Point(231, 333)
        Me.dataProgress.Name = "dataProgress"
        Me.dataProgress.Size = New System.Drawing.Size(200, 23)
        Me.dataProgress.TabIndex = 129
        Me.dataProgress.Visible = False
        '
        'txtID
        '
        Me.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtID.Location = New System.Drawing.Point(377, 310)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(54, 20)
        Me.txtID.TabIndex = 128
        Me.txtID.TabStop = False
        Me.txtID.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.GhostWhite
        Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(470, 500)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 130
        Me.PictureBox1.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.GhostWhite
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(13, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(177, 28)
        Me.Label2.TabIndex = 131
        Me.Label2.Text = "Step 1 : If you are 1st Time Click on" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " "" Genrate Organization ID ""."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.GhostWhite
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(12, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 14)
        Me.Label3.TabIndex = 132
        Me.Label3.Text = "Step 2 : Choose Your Password."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.GhostWhite
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(12, 110)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(159, 42)
        Me.Label4.TabIndex = 133
        Me.Label4.Text = "Step 3 : Click  "" Synchronization" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " On Server "". Be patience." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "It Will Take Some " & _
    "Time."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.BackColor = System.Drawing.Color.GhostWhite
        Me.lblProgress.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblProgress.ForeColor = System.Drawing.Color.Teal
        Me.lblProgress.Location = New System.Drawing.Point(229, 313)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(63, 15)
        Me.lblProgress.TabIndex = 134
        Me.lblProgress.Text = "Progress..."
        '
        'btnCustom
        '
        Me.btnCustom.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnCustom.FlatAppearance.BorderSize = 0
        Me.btnCustom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCustom.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnCustom.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnCustom.Location = New System.Drawing.Point(227, 384)
        Me.btnCustom.Name = "btnCustom"
        Me.btnCustom.Size = New System.Drawing.Size(208, 35)
        Me.btnCustom.TabIndex = 135
        Me.btnCustom.Text = "Synchronization"
        Me.btnCustom.UseVisualStyleBackColor = False
        '
        'lblLastUpdate
        '
        Me.lblLastUpdate.AutoSize = True
        Me.lblLastUpdate.BackColor = System.Drawing.Color.GhostWhite
        Me.lblLastUpdate.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblLastUpdate.ForeColor = System.Drawing.Color.Navy
        Me.lblLastUpdate.Location = New System.Drawing.Point(240, 99)
        Me.lblLastUpdate.Name = "lblLastUpdate"
        Me.lblLastUpdate.Size = New System.Drawing.Size(0, 15)
        Me.lblLastUpdate.TabIndex = 136
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.GhostWhite
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Navy
        Me.Label5.Location = New System.Drawing.Point(230, 83)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(103, 15)
        Me.Label5.TabIndex = 137
        Me.Label5.Text = "Last Updated On :"
        '
        'ckFullSync
        '
        Me.ckFullSync.AutoSize = True
        Me.ckFullSync.BackColor = System.Drawing.Color.GhostWhite
        Me.ckFullSync.Font = New System.Drawing.Font("Times New Roman", 8.0!)
        Me.ckFullSync.ForeColor = System.Drawing.Color.Red
        Me.ckFullSync.Location = New System.Drawing.Point(232, 362)
        Me.ckFullSync.Name = "ckFullSync"
        Me.ckFullSync.Size = New System.Drawing.Size(68, 18)
        Me.ckFullSync.TabIndex = 138
        Me.ckFullSync.Text = "&Full Sync"
        Me.ckFullSync.UseVisualStyleBackColor = False
        '
        'lblLink
        '
        Me.lblLink.AutoSize = True
        Me.lblLink.Location = New System.Drawing.Point(233, 275)
        Me.lblLink.Name = "lblLink"
        Me.lblLink.Size = New System.Drawing.Size(75, 13)
        Me.lblLink.TabIndex = 139
        Me.lblLink.TabStop = True
        Me.lblLink.Text = "Server : Office"
        '
        'pnlLock
        '
        Me.pnlLock.Controls.Add(Me.Label9)
        Me.pnlLock.Controls.Add(Me.Label8)
        Me.pnlLock.Controls.Add(Me.Label7)
        Me.pnlLock.Controls.Add(Me.TxtAuthPathPassword)
        Me.pnlLock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlLock.Location = New System.Drawing.Point(0, 0)
        Me.pnlLock.Name = "pnlLock"
        Me.pnlLock.Size = New System.Drawing.Size(470, 500)
        Me.pnlLock.TabIndex = 140
        Me.pnlLock.Visible = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Navy
        Me.Label9.Location = New System.Drawing.Point(56, 110)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(320, 38)
        Me.Label9.TabIndex = 3
        Me.Label9.Text = "Use Password For Try Mobile App in Demo Mode" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Contact to Software Support Officer" & _
    " for Password"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(94, 72)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(250, 19)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Mobile App Locked In Demo Mode !!!"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Teal
        Me.Label7.Location = New System.Drawing.Point(45, 194)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(146, 19)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Authorised Password :"
        '
        'TxtAuthPathPassword
        '
        Me.TxtAuthPathPassword.BackColor = System.Drawing.Color.GhostWhite
        Me.TxtAuthPathPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtAuthPathPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TxtAuthPathPassword.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TxtAuthPathPassword.ForeColor = System.Drawing.Color.Black
        Me.TxtAuthPathPassword.Location = New System.Drawing.Point(197, 191)
        Me.TxtAuthPathPassword.Name = "TxtAuthPathPassword"
        Me.TxtAuthPathPassword.Size = New System.Drawing.Size(154, 26)
        Me.TxtAuthPathPassword.TabIndex = 0
        Me.TxtAuthPathPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Mobile_App
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(470, 500)
        Me.Controls.Add(Me.lblLink)
        Me.Controls.Add(Me.ckFullSync)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblLastUpdate)
        Me.Controls.Add(Me.btnCustom)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dataProgress)
        Me.Controls.Add(Me.CkShowPassword)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.BtnIDGenrate)
        Me.Controls.Add(Me.txtCompanyID)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.pnlLock)
        Me.Name = "Mobile_App"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Mobile_App"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLock.ResumeLayout(False)
        Me.pnlLock.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtCompanyID As System.Windows.Forms.TextBox
    Friend WithEvents BtnIDGenrate As System.Windows.Forms.Button
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CkShowPassword As System.Windows.Forms.CheckBox
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Private WithEvents dataProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents btnCustom As System.Windows.Forms.Button
    Friend WithEvents lblLastUpdate As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ckFullSync As System.Windows.Forms.CheckBox
    Friend WithEvents lblLink As System.Windows.Forms.LinkLabel
    Friend WithEvents pnlLock As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TxtAuthPathPassword As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
End Class
