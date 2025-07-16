<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Change_Financial_Year
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Change_Financial_Year))
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.mskCurrFromDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.MskCurrToDate = New System.Windows.Forms.MaskedTextBox()
        Me.txtCurrentPath = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.mskNewtoDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.mskNewFromDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblgroup = New System.Windows.Forms.Label()
        Me.txtAccountID = New System.Windows.Forms.TextBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.lblstatus2 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnChangeYear = New System.Windows.Forms.Button()
        Me.lblAcBalance = New System.Windows.Forms.Label()
        Me.lblCrateBalance = New System.Windows.Forms.Label()
        Me.lblStockBalance = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.RadioAccounts = New System.Windows.Forms.RadioButton()
        Me.RadioCrates = New System.Windows.Forms.RadioButton()
        Me.RadioStock = New System.Windows.Forms.RadioButton()
        Me.RadioAll = New System.Windows.Forms.RadioButton()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtAccount
        '
        Me.txtAccount.BackColor = System.Drawing.Color.GhostWhite
        Me.txtAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccount.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtAccount.ForeColor = System.Drawing.Color.Black
        Me.txtAccount.Location = New System.Drawing.Point(76, 119)
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.ReadOnly = True
        Me.txtAccount.Size = New System.Drawing.Size(384, 26)
        Me.txtAccount.TabIndex = 40114
        Me.txtAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(156, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(212, 19)
        Me.Label4.TabIndex = 40115
        Me.Label4.Text = "Select Firm / Company From List"
        '
        'mskCurrFromDate
        '
        Me.mskCurrFromDate.BackColor = System.Drawing.Color.GhostWhite
        Me.mskCurrFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskCurrFromDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.mskCurrFromDate.ForeColor = System.Drawing.Color.Black
        Me.mskCurrFromDate.Location = New System.Drawing.Point(535, 119)
        Me.mskCurrFromDate.Mask = "00-00-0000"
        Me.mskCurrFromDate.Name = "mskCurrFromDate"
        Me.mskCurrFromDate.ReadOnly = True
        Me.mskCurrFromDate.Size = New System.Drawing.Size(84, 26)
        Me.mskCurrFromDate.TabIndex = 40117
        Me.mskCurrFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.Black
        Me.Label25.Location = New System.Drawing.Point(513, 97)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(143, 19)
        Me.Label25.TabIndex = 40118
        Me.Label25.Text = "Current Financial Year"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(466, 122)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 19)
        Me.Label1.TabIndex = 40119
        Me.Label1.Text = "From :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(627, 122)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(28, 19)
        Me.Label2.TabIndex = 40120
        Me.Label2.Text = "to :"
        '
        'MskCurrToDate
        '
        Me.MskCurrToDate.BackColor = System.Drawing.Color.GhostWhite
        Me.MskCurrToDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MskCurrToDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.MskCurrToDate.ForeColor = System.Drawing.Color.Black
        Me.MskCurrToDate.Location = New System.Drawing.Point(668, 120)
        Me.MskCurrToDate.Mask = "00-00-0000"
        Me.MskCurrToDate.Name = "MskCurrToDate"
        Me.MskCurrToDate.ReadOnly = True
        Me.MskCurrToDate.Size = New System.Drawing.Size(84, 26)
        Me.MskCurrToDate.TabIndex = 40121
        Me.MskCurrToDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtCurrentPath
        '
        Me.txtCurrentPath.BackColor = System.Drawing.Color.GhostWhite
        Me.txtCurrentPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCurrentPath.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtCurrentPath.ForeColor = System.Drawing.Color.Black
        Me.txtCurrentPath.Location = New System.Drawing.Point(76, 167)
        Me.txtCurrentPath.Name = "txtCurrentPath"
        Me.txtCurrentPath.ReadOnly = True
        Me.txtCurrentPath.Size = New System.Drawing.Size(384, 26)
        Me.txtCurrentPath.TabIndex = 40122
        Me.txtCurrentPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(173, 148)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(151, 19)
        Me.Label3.TabIndex = 40123
        Me.Label3.Text = "Current Database Path "
        '
        'mskNewtoDate
        '
        Me.mskNewtoDate.BackColor = System.Drawing.Color.GhostWhite
        Me.mskNewtoDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskNewtoDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.mskNewtoDate.ForeColor = System.Drawing.Color.DarkRed
        Me.mskNewtoDate.Location = New System.Drawing.Point(668, 170)
        Me.mskNewtoDate.Mask = "00-00-0000"
        Me.mskNewtoDate.Name = "mskNewtoDate"
        Me.mskNewtoDate.Size = New System.Drawing.Size(84, 26)
        Me.mskNewtoDate.TabIndex = 40128
        Me.mskNewtoDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(627, 171)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 19)
        Me.Label5.TabIndex = 40127
        Me.Label5.Text = "to :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(467, 171)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 19)
        Me.Label6.TabIndex = 40126
        Me.Label6.Text = "From :"
        '
        'mskNewFromDate
        '
        Me.mskNewFromDate.BackColor = System.Drawing.Color.GhostWhite
        Me.mskNewFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskNewFromDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.mskNewFromDate.ForeColor = System.Drawing.Color.DarkRed
        Me.mskNewFromDate.Location = New System.Drawing.Point(529, 171)
        Me.mskNewFromDate.Mask = "00-00-0000"
        Me.mskNewFromDate.Name = "mskNewFromDate"
        Me.mskNewFromDate.Size = New System.Drawing.Size(92, 26)
        Me.mskNewFromDate.TabIndex = 40124
        Me.mskNewFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(525, 146)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(127, 19)
        Me.Label7.TabIndex = 40125
        Me.Label7.Text = "New Financial Year"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.DarkKhaki
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Right
        Me.Panel4.Location = New System.Drawing.Point(854, 27)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(10, 354)
        Me.Panel4.TabIndex = 40201
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.DarkKhaki
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel3.Location = New System.Drawing.Point(0, 27)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(10, 354)
        Me.Panel3.TabIndex = 40200
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.DarkKhaki
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 381)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(864, 10)
        Me.Panel2.TabIndex = 40199
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.DarkKhaki
        Me.Panel1.Controls.Add(Me.btnClose)
        Me.Panel1.Controls.Add(Me.lblgroup)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(864, 27)
        Me.Panel1.TabIndex = 40198
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.DarkKhaki
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Location = New System.Drawing.Point(830, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(34, 25)
        Me.btnClose.TabIndex = 40030
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "X"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblgroup
        '
        Me.lblgroup.AutoSize = True
        Me.lblgroup.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblgroup.ForeColor = System.Drawing.Color.GhostWhite
        Me.lblgroup.Location = New System.Drawing.Point(4, 5)
        Me.lblgroup.Name = "lblgroup"
        Me.lblgroup.Size = New System.Drawing.Size(136, 19)
        Me.lblgroup.TabIndex = 1
        Me.lblgroup.Text = "Change Finacial Year"
        '
        'txtAccountID
        '
        Me.txtAccountID.BackColor = System.Drawing.Color.AliceBlue
        Me.txtAccountID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccountID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAccountID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountID.ForeColor = System.Drawing.Color.Teal
        Me.txtAccountID.Location = New System.Drawing.Point(436, 90)
        Me.txtAccountID.Name = "txtAccountID"
        Me.txtAccountID.Size = New System.Drawing.Size(48, 26)
        Me.txtAccountID.TabIndex = 40203
        Me.txtAccountID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtAccountID.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(76, 223)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(435, 23)
        Me.ProgressBar1.TabIndex = 40205
        Me.ProgressBar1.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Teal
        Me.lblStatus.Location = New System.Drawing.Point(72, 249)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(34, 19)
        Me.lblStatus.TabIndex = 40206
        Me.lblStatus.Text = "0 %"
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerReportsProgress = True
        Me.BackgroundWorker1.WorkerSupportsCancellation = True
        '
        'lblstatus2
        '
        Me.lblstatus2.AutoSize = True
        Me.lblstatus2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblstatus2.ForeColor = System.Drawing.Color.Red
        Me.lblstatus2.Location = New System.Drawing.Point(70, 353)
        Me.lblstatus2.Name = "lblstatus2"
        Me.lblstatus2.Size = New System.Drawing.Size(372, 19)
        Me.lblstatus2.TabIndex = 40207
        Me.lblstatus2.Text = "4. Please check Crate Balances Before Change Fincial Year."
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(70, 331)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(397, 19)
        Me.Label8.TabIndex = 40208
        Me.Label8.Text = "3. Please Check Ledger Balances From Previous Financial Year."
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(69, 267)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(66, 19)
        Me.Label9.TabIndex = 40209
        Me.Label9.Text = "Warning :"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(69, 287)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(227, 19)
        Me.Label10.TabIndex = 40210
        Me.Label10.Text = "1. Please Take Backup of database."
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Red
        Me.Label11.Location = New System.Drawing.Point(70, 308)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(368, 19)
        Me.Label11.TabIndex = 40211
        Me.Label11.Text = "2. Change  Financial Year In Monitoring of Support Person."
        '
        'btnChangeYear
        '
        Me.btnChangeYear.BackColor = System.Drawing.Color.MediumAquamarine
        Me.btnChangeYear.FlatAppearance.BorderSize = 0
        Me.btnChangeYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnChangeYear.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.btnChangeYear.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnChangeYear.Image = CType(resources.GetObject("btnChangeYear.Image"), System.Drawing.Image)
        Me.btnChangeYear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeYear.Location = New System.Drawing.Point(529, 208)
        Me.btnChangeYear.Name = "btnChangeYear"
        Me.btnChangeYear.Size = New System.Drawing.Size(223, 49)
        Me.btnChangeYear.TabIndex = 40129
        Me.btnChangeYear.Text = "Change Financial Year"
        Me.btnChangeYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnChangeYear.UseVisualStyleBackColor = False
        '
        'lblAcBalance
        '
        Me.lblAcBalance.AutoSize = True
        Me.lblAcBalance.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblAcBalance.ForeColor = System.Drawing.Color.Navy
        Me.lblAcBalance.Location = New System.Drawing.Point(131, 54)
        Me.lblAcBalance.Name = "lblAcBalance"
        Me.lblAcBalance.Size = New System.Drawing.Size(121, 19)
        Me.lblAcBalance.TabIndex = 40245
        Me.lblAcBalance.Text = "Account Balance"
        '
        'lblCrateBalance
        '
        Me.lblCrateBalance.AutoSize = True
        Me.lblCrateBalance.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblCrateBalance.ForeColor = System.Drawing.Color.Navy
        Me.lblCrateBalance.Location = New System.Drawing.Point(350, 55)
        Me.lblCrateBalance.Name = "lblCrateBalance"
        Me.lblCrateBalance.Size = New System.Drawing.Size(105, 19)
        Me.lblCrateBalance.TabIndex = 40246
        Me.lblCrateBalance.Text = "Crate Balance"
        '
        'lblStockBalance
        '
        Me.lblStockBalance.AutoSize = True
        Me.lblStockBalance.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblStockBalance.ForeColor = System.Drawing.Color.Navy
        Me.lblStockBalance.Location = New System.Drawing.Point(563, 54)
        Me.lblStockBalance.Name = "lblStockBalance"
        Me.lblStockBalance.Size = New System.Drawing.Size(105, 19)
        Me.lblStockBalance.TabIndex = 40247
        Me.lblStockBalance.Text = "Stock Balance"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(80, 45)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 40248
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(301, 45)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 40249
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(504, 44)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(40, 40)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox3.TabIndex = 40250
        Me.PictureBox3.TabStop = False
        '
        'RadioAccounts
        '
        Me.RadioAccounts.AutoSize = True
        Me.RadioAccounts.Location = New System.Drawing.Point(80, 200)
        Me.RadioAccounts.Name = "RadioAccounts"
        Me.RadioAccounts.Size = New System.Drawing.Size(94, 17)
        Me.RadioAccounts.TabIndex = 40251
        Me.RadioAccounts.TabStop = True
        Me.RadioAccounts.Text = "Accounts Only"
        Me.RadioAccounts.UseVisualStyleBackColor = True
        Me.RadioAccounts.Visible = False
        '
        'RadioCrates
        '
        Me.RadioCrates.AutoSize = True
        Me.RadioCrates.Location = New System.Drawing.Point(180, 200)
        Me.RadioCrates.Name = "RadioCrates"
        Me.RadioCrates.Size = New System.Drawing.Size(79, 17)
        Me.RadioCrates.TabIndex = 40252
        Me.RadioCrates.TabStop = True
        Me.RadioCrates.Text = "Crates Only"
        Me.RadioCrates.UseVisualStyleBackColor = True
        Me.RadioCrates.Visible = False
        '
        'RadioStock
        '
        Me.RadioStock.AutoSize = True
        Me.RadioStock.Location = New System.Drawing.Point(265, 200)
        Me.RadioStock.Name = "RadioStock"
        Me.RadioStock.Size = New System.Drawing.Size(77, 17)
        Me.RadioStock.TabIndex = 40253
        Me.RadioStock.TabStop = True
        Me.RadioStock.Text = "Stock Only"
        Me.RadioStock.UseVisualStyleBackColor = True
        Me.RadioStock.Visible = False
        '
        'RadioAll
        '
        Me.RadioAll.AutoSize = True
        Me.RadioAll.Location = New System.Drawing.Point(348, 200)
        Me.RadioAll.Name = "RadioAll"
        Me.RadioAll.Size = New System.Drawing.Size(160, 17)
        Me.RadioAll.TabIndex = 40254
        Me.RadioAll.TabStop = True
        Me.RadioAll.Text = "All (Accounts+Crates+Stock)"
        Me.RadioAll.UseVisualStyleBackColor = True
        Me.RadioAll.Visible = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Times New Roman", 12.0!, CType(((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic) _
                Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Navy
        Me.Label12.Location = New System.Drawing.Point(490, 312)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(342, 38)
        Me.Label12.TabIndex = 40255
        Me.Label12.Text = "Note :" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "On Sale Entires Will Not Transfer Automettically."
        '
        'Change_Financial_Year
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(864, 391)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.RadioAll)
        Me.Controls.Add(Me.RadioStock)
        Me.Controls.Add(Me.RadioCrates)
        Me.Controls.Add(Me.RadioAccounts)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblStockBalance)
        Me.Controls.Add(Me.lblCrateBalance)
        Me.Controls.Add(Me.lblAcBalance)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.lblstatus2)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.txtAccountID)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnChangeYear)
        Me.Controls.Add(Me.mskNewtoDate)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.mskNewFromDate)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtCurrentPath)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.MskCurrToDate)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.mskCurrFromDate)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.Label4)
        Me.Name = "Change_Financial_Year"
        Me.Text = "Change_Financial_Year"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents mskCurrFromDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents MskCurrToDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtCurrentPath As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents mskNewtoDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents mskNewFromDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnChangeYear As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblgroup As System.Windows.Forms.Label
    Friend WithEvents txtAccountID As System.Windows.Forms.TextBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblstatus2 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblAcBalance As System.Windows.Forms.Label
    Friend WithEvents lblCrateBalance As System.Windows.Forms.Label
    Friend WithEvents lblStockBalance As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents RadioAccounts As System.Windows.Forms.RadioButton
    Friend WithEvents RadioCrates As System.Windows.Forms.RadioButton
    Friend WithEvents RadioStock As System.Windows.Forms.RadioButton
    Friend WithEvents RadioAll As System.Windows.Forms.RadioButton
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class
