<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Bank_Entry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Bank_Entry))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblChequeNo = New System.Windows.Forms.Label()
        Me.lblChequeDate = New System.Windows.Forms.Label()
        Me.TxtChequeNo = New System.Windows.Forms.MaskedTextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.CbEntryType = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtRemark = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.MskEntryDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txtAccountID = New System.Windows.Forms.TextBox()
        Me.txtModeID = New System.Windows.Forms.TextBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.txtExpenses = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.mskChequeDate = New System.Windows.Forms.MaskedTextBox()
        Me.dgMode = New System.Windows.Forms.DataGridView()
        Me.txtMode = New System.Windows.Forms.TextBox()
        Me.DgAccountSearch = New System.Windows.Forms.DataGridView()
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.txtBillNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtTotTotal = New System.Windows.Forms.TextBox()
        Me.txtTotDisc = New System.Windows.Forms.TextBox()
        Me.txttotNet = New System.Windows.Forms.TextBox()
        Me.Panel20 = New System.Windows.Forms.Panel()
        Me.Panel21 = New System.Windows.Forms.Panel()
        Me.Panel22 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.dtp1 = New System.Windows.Forms.DateTimePicker()
        Me.lblCapAcBal = New System.Windows.Forms.Label()
        Me.lblAcBal = New System.Windows.Forms.Label()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgMode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DgAccountSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblChequeNo
        '
        Me.lblChequeNo.AutoSize = True
        Me.lblChequeNo.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblChequeNo.ForeColor = System.Drawing.Color.Black
        Me.lblChequeNo.Location = New System.Drawing.Point(10, 243)
        Me.lblChequeNo.Name = "lblChequeNo"
        Me.lblChequeNo.Size = New System.Drawing.Size(95, 19)
        Me.lblChequeNo.TabIndex = 34
        Me.lblChequeNo.Text = "Cheque No.  :"
        '
        'lblChequeDate
        '
        Me.lblChequeDate.AutoSize = True
        Me.lblChequeDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblChequeDate.ForeColor = System.Drawing.Color.Black
        Me.lblChequeDate.Location = New System.Drawing.Point(324, 245)
        Me.lblChequeDate.Name = "lblChequeDate"
        Me.lblChequeDate.Size = New System.Drawing.Size(96, 19)
        Me.lblChequeDate.TabIndex = 32
        Me.lblChequeDate.Text = "Cheque Date :"
        '
        'TxtChequeNo
        '
        Me.TxtChequeNo.BackColor = System.Drawing.Color.GhostWhite
        Me.TxtChequeNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtChequeNo.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TxtChequeNo.ForeColor = System.Drawing.Color.Black
        Me.TxtChequeNo.Location = New System.Drawing.Point(111, 241)
        Me.TxtChequeNo.Name = "TxtChequeNo"
        Me.TxtChequeNo.Size = New System.Drawing.Size(208, 26)
        Me.TxtChequeNo.TabIndex = 10
        '
        'txtAmount
        '
        Me.txtAmount.BackColor = System.Drawing.Color.GhostWhite
        Me.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAmount.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtAmount.ForeColor = System.Drawing.Color.Black
        Me.txtAmount.Location = New System.Drawing.Point(91, 199)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.Size = New System.Drawing.Size(229, 26)
        Me.txtAmount.TabIndex = 7
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.Black
        Me.Label16.Location = New System.Drawing.Point(10, 201)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(64, 19)
        Me.Label16.TabIndex = 26
        Me.Label16.Text = "Amount :"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(326, 150)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(59, 19)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "Legder :"
        '
        'CbEntryType
        '
        Me.CbEntryType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbEntryType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbEntryType.BackColor = System.Drawing.Color.GhostWhite
        Me.CbEntryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbEntryType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbEntryType.Font = New System.Drawing.Font("Times New Roman", 10.0!)
        Me.CbEntryType.ForeColor = System.Drawing.Color.Black
        Me.CbEntryType.FormattingEnabled = True
        Me.CbEntryType.Items.AddRange(New Object() {"Bank Expenses", "Cheque / Draft / RTGS Recieved", "Cheque Issued", "Deposit Cash Into Bank", "Online Transfer", "WithDraw Cash From Bank"})
        Me.CbEntryType.Location = New System.Drawing.Point(72, 151)
        Me.CbEntryType.Name = "CbEntryType"
        Me.CbEntryType.Size = New System.Drawing.Size(248, 23)
        Me.CbEntryType.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(10, 152)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 19)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Entry :"
        '
        'txtRemark
        '
        Me.txtRemark.BackColor = System.Drawing.Color.GhostWhite
        Me.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRemark.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtRemark.ForeColor = System.Drawing.Color.Black
        Me.txtRemark.Location = New System.Drawing.Point(748, 108)
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(198, 26)
        Me.txtRemark.TabIndex = 4
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(326, 106)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(48, 19)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Bank :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(10, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(45, 19)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Date :"
        '
        'MskEntryDate
        '
        Me.MskEntryDate.BackColor = System.Drawing.Color.GhostWhite
        Me.MskEntryDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MskEntryDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.MskEntryDate.ForeColor = System.Drawing.Color.Black
        Me.MskEntryDate.Location = New System.Drawing.Point(72, 106)
        Me.MskEntryDate.Mask = "00-00-0000"
        Me.MskEntryDate.Name = "MskEntryDate"
        Me.MskEntryDate.Size = New System.Drawing.Size(86, 26)
        Me.MskEntryDate.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(477, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(253, 48)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "BANK ENTRY"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.Black
        Me.Label13.Location = New System.Drawing.Point(665, 106)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(64, 19)
        Me.Label13.TabIndex = 18
        Me.Label13.Text = "Remark :"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(1143, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 49)
        Me.btnClose.TabIndex = 91118
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'txtAccountID
        '
        Me.txtAccountID.BackColor = System.Drawing.Color.AliceBlue
        Me.txtAccountID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAccountID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountID.ForeColor = System.Drawing.Color.Teal
        Me.txtAccountID.Location = New System.Drawing.Point(139, 12)
        Me.txtAccountID.Name = "txtAccountID"
        Me.txtAccountID.Size = New System.Drawing.Size(48, 26)
        Me.txtAccountID.TabIndex = 91116
        Me.txtAccountID.TabStop = False
        Me.txtAccountID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtAccountID.Visible = False
        '
        'txtModeID
        '
        Me.txtModeID.BackColor = System.Drawing.Color.AliceBlue
        Me.txtModeID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtModeID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtModeID.ForeColor = System.Drawing.Color.Teal
        Me.txtModeID.Location = New System.Drawing.Point(85, 12)
        Me.txtModeID.Name = "txtModeID"
        Me.txtModeID.Size = New System.Drawing.Size(48, 26)
        Me.txtModeID.TabIndex = 91115
        Me.txtModeID.TabStop = False
        Me.txtModeID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtModeID.Visible = False
        '
        'txtID
        '
        Me.txtID.BackColor = System.Drawing.Color.AliceBlue
        Me.txtID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtID.ForeColor = System.Drawing.Color.Teal
        Me.txtID.Location = New System.Drawing.Point(5, 13)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(70, 26)
        Me.txtID.TabIndex = 40
        Me.txtID.Visible = False
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dg1.ColumnHeadersHeight = 28
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkTurquoise
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle2
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Gray
        Me.dg1.Location = New System.Drawing.Point(12, 280)
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dg1.RowHeadersVisible = False
        Me.dg1.RowHeadersWidth = 42
        Me.dg1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1172, 336)
        Me.dg1.TabIndex = 39
        Me.dg1.TabStop = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnSave.Image = Global.Aadhat.My.Resources.Resources.Save
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(841, 227)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(106, 39)
        Me.btnSave.TabIndex = 12
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'txtExpenses
        '
        Me.txtExpenses.BackColor = System.Drawing.Color.GhostWhite
        Me.txtExpenses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtExpenses.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtExpenses.ForeColor = System.Drawing.Color.Black
        Me.txtExpenses.Location = New System.Drawing.Point(457, 199)
        Me.txtExpenses.Name = "txtExpenses"
        Me.txtExpenses.Size = New System.Drawing.Size(198, 26)
        Me.txtExpenses.TabIndex = 8
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.Black
        Me.Label22.Location = New System.Drawing.Point(435, 201)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(12, 19)
        Me.Label22.TabIndex = 29
        Me.Label22.Text = ":"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label23.ForeColor = System.Drawing.Color.Black
        Me.Label23.Location = New System.Drawing.Point(326, 203)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(66, 19)
        Me.Label23.TabIndex = 28
        Me.Label23.Text = "Expenses"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.Black
        Me.Label24.Location = New System.Drawing.Point(716, 203)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(12, 19)
        Me.Label24.TabIndex = 31
        Me.Label24.Text = ":"
        '
        'txtTotal
        '
        Me.txtTotal.BackColor = System.Drawing.Color.GhostWhite
        Me.txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotal.ForeColor = System.Drawing.Color.Black
        Me.txtTotal.Location = New System.Drawing.Point(749, 199)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.Size = New System.Drawing.Size(198, 26)
        Me.txtTotal.TabIndex = 9
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label26.ForeColor = System.Drawing.Color.Black
        Me.Label26.Location = New System.Drawing.Point(657, 202)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(39, 19)
        Me.Label26.TabIndex = 30
        Me.Label26.Text = "Total"
        '
        'mskChequeDate
        '
        Me.mskChequeDate.BackColor = System.Drawing.Color.GhostWhite
        Me.mskChequeDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskChequeDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.mskChequeDate.ForeColor = System.Drawing.Color.Black
        Me.mskChequeDate.Location = New System.Drawing.Point(457, 241)
        Me.mskChequeDate.Mask = "00-00-0000"
        Me.mskChequeDate.Name = "mskChequeDate"
        Me.mskChequeDate.Size = New System.Drawing.Size(99, 26)
        Me.mskChequeDate.TabIndex = 11
        '
        'dgMode
        '
        Me.dgMode.AllowUserToAddRows = False
        Me.dgMode.AllowUserToDeleteRows = False
        Me.dgMode.AllowUserToResizeRows = False
        Me.dgMode.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dgMode.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.Teal
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Teal
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgMode.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgMode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgMode.ColumnHeadersVisible = False
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DarkTurquoise
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgMode.DefaultCellStyle = DataGridViewCellStyle5
        Me.dgMode.EnableHeadersVisualStyles = False
        Me.dgMode.GridColor = System.Drawing.Color.Gray
        Me.dgMode.Location = New System.Drawing.Point(388, 132)
        Me.dgMode.MultiSelect = False
        Me.dgMode.Name = "dgMode"
        Me.dgMode.ReadOnly = True
        Me.dgMode.RowHeadersVisible = False
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        Me.dgMode.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.dgMode.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.dgMode.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgMode.Size = New System.Drawing.Size(369, 135)
        Me.dgMode.TabIndex = 40121
        Me.dgMode.TabStop = False
        Me.dgMode.Visible = False
        '
        'txtMode
        '
        Me.txtMode.BackColor = System.Drawing.Color.GhostWhite
        Me.txtMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMode.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtMode.ForeColor = System.Drawing.Color.Black
        Me.txtMode.Location = New System.Drawing.Point(388, 107)
        Me.txtMode.Name = "txtMode"
        Me.txtMode.Size = New System.Drawing.Size(267, 26)
        Me.txtMode.TabIndex = 3
        '
        'DgAccountSearch
        '
        Me.DgAccountSearch.AllowUserToAddRows = False
        Me.DgAccountSearch.AllowUserToDeleteRows = False
        Me.DgAccountSearch.AllowUserToResizeRows = False
        Me.DgAccountSearch.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.DgAccountSearch.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Teal
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DgAccountSearch.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.DgAccountSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgAccountSearch.ColumnHeadersVisible = False
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.DarkTurquoise
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DgAccountSearch.DefaultCellStyle = DataGridViewCellStyle8
        Me.DgAccountSearch.EnableHeadersVisualStyles = False
        Me.DgAccountSearch.GridColor = System.Drawing.Color.Gray
        Me.DgAccountSearch.Location = New System.Drawing.Point(388, 173)
        Me.DgAccountSearch.MultiSelect = False
        Me.DgAccountSearch.Name = "DgAccountSearch"
        Me.DgAccountSearch.ReadOnly = True
        Me.DgAccountSearch.RowHeadersVisible = False
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black
        Me.DgAccountSearch.RowsDefaultCellStyle = DataGridViewCellStyle9
        Me.DgAccountSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.DgAccountSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DgAccountSearch.Size = New System.Drawing.Size(559, 202)
        Me.DgAccountSearch.TabIndex = 40124
        Me.DgAccountSearch.TabStop = False
        Me.DgAccountSearch.Visible = False
        '
        'txtAccount
        '
        Me.txtAccount.BackColor = System.Drawing.Color.GhostWhite
        Me.txtAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccount.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtAccount.ForeColor = System.Drawing.Color.Black
        Me.txtAccount.Location = New System.Drawing.Point(388, 148)
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.Size = New System.Drawing.Size(559, 26)
        Me.txtAccount.TabIndex = 6
        '
        'txtBillNo
        '
        Me.txtBillNo.BackColor = System.Drawing.Color.GhostWhite
        Me.txtBillNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBillNo.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtBillNo.ForeColor = System.Drawing.Color.Black
        Me.txtBillNo.Location = New System.Drawing.Point(222, 107)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.Size = New System.Drawing.Size(98, 26)
        Me.txtBillNo.TabIndex = 2
        Me.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(178, 107)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 19)
        Me.Label3.TabIndex = 40126
        Me.Label3.Text = "No. :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(902, 619)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(97, 19)
        Me.Label2.TabIndex = 40134
        Me.Label2.Text = "Total Amount :"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Black
        Me.Label10.Location = New System.Drawing.Point(641, 619)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 19)
        Me.Label10.TabIndex = 40133
        Me.Label10.Text = "Discount :"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(336, 619)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 19)
        Me.Label6.TabIndex = 40132
        Me.Label6.Text = "Basic Amount :"
        '
        'txtTotTotal
        '
        Me.txtTotTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotTotal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotTotal.ForeColor = System.Drawing.Color.Red
        Me.txtTotTotal.Location = New System.Drawing.Point(1020, 621)
        Me.txtTotTotal.Name = "txtTotTotal"
        Me.txtTotTotal.Size = New System.Drawing.Size(164, 19)
        Me.txtTotTotal.TabIndex = 40131
        Me.txtTotTotal.TabStop = False
        Me.txtTotTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotDisc
        '
        Me.txtTotDisc.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotDisc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotDisc.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotDisc.ForeColor = System.Drawing.Color.Red
        Me.txtTotDisc.Location = New System.Drawing.Point(733, 621)
        Me.txtTotDisc.Name = "txtTotDisc"
        Me.txtTotDisc.Size = New System.Drawing.Size(159, 19)
        Me.txtTotDisc.TabIndex = 40130
        Me.txtTotDisc.TabStop = False
        Me.txtTotDisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txttotNet
        '
        Me.txttotNet.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txttotNet.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txttotNet.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txttotNet.ForeColor = System.Drawing.Color.Red
        Me.txttotNet.Location = New System.Drawing.Point(468, 621)
        Me.txttotNet.Name = "txttotNet"
        Me.txttotNet.Size = New System.Drawing.Size(100, 19)
        Me.txttotNet.TabIndex = 40129
        Me.txttotNet.TabStop = False
        Me.txttotNet.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel20
        '
        Me.Panel20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel20.Location = New System.Drawing.Point(342, 640)
        Me.Panel20.Name = "Panel20"
        Me.Panel20.Size = New System.Drawing.Size(226, 1)
        Me.Panel20.TabIndex = 40249
        '
        'Panel21
        '
        Me.Panel21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel21.Location = New System.Drawing.Point(647, 640)
        Me.Panel21.Name = "Panel21"
        Me.Panel21.Size = New System.Drawing.Size(249, 1)
        Me.Panel21.TabIndex = 40250
        '
        'Panel22
        '
        Me.Panel22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel22.Location = New System.Drawing.Point(906, 640)
        Me.Panel22.Name = "Panel22"
        Me.Panel22.Size = New System.Drawing.Size(235, 1)
        Me.Panel22.TabIndex = 40251
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(993, 108)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(167, 158)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 91137
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(422, 9)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 91138
        Me.PictureBox2.TabStop = False
        '
        'BtnDelete
        '
        Me.BtnDelete.BackColor = System.Drawing.Color.DarkRed
        Me.BtnDelete.FlatAppearance.BorderSize = 0
        Me.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDelete.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BtnDelete.ForeColor = System.Drawing.Color.Green
        Me.BtnDelete.Image = CType(resources.GetObject("BtnDelete.Image"), System.Drawing.Image)
        Me.BtnDelete.Location = New System.Drawing.Point(14, 619)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(38, 32)
        Me.BtnDelete.TabIndex = 91253
        Me.BtnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnDelete.UseVisualStyleBackColor = False
        Me.BtnDelete.Visible = False
        '
        'dtp1
        '
        Me.dtp1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp1.Location = New System.Drawing.Point(72, 106)
        Me.dtp1.Name = "dtp1"
        Me.dtp1.Size = New System.Drawing.Size(100, 26)
        Me.dtp1.TabIndex = 91259
        '
        'lblCapAcBal
        '
        Me.lblCapAcBal.AutoSize = True
        Me.lblCapAcBal.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblCapAcBal.ForeColor = System.Drawing.Color.Navy
        Me.lblCapAcBal.Location = New System.Drawing.Point(392, 87)
        Me.lblCapAcBal.Name = "lblCapAcBal"
        Me.lblCapAcBal.Size = New System.Drawing.Size(28, 17)
        Me.lblCapAcBal.TabIndex = 91260
        Me.lblCapAcBal.Text = "Bal"
        Me.lblCapAcBal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblCapAcBal.Visible = False
        '
        'lblAcBal
        '
        Me.lblAcBal.AutoSize = True
        Me.lblAcBal.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblAcBal.ForeColor = System.Drawing.Color.Navy
        Me.lblAcBal.Location = New System.Drawing.Point(682, 78)
        Me.lblAcBal.Name = "lblAcBal"
        Me.lblAcBal.Size = New System.Drawing.Size(28, 17)
        Me.lblAcBal.TabIndex = 91261
        Me.lblAcBal.Text = "Bal"
        Me.lblAcBal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblAcBal.Visible = False
        '
        'Bank_Entry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.lblAcBal)
        Me.Controls.Add(Me.lblCapAcBal)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Panel22)
        Me.Controls.Add(Me.txtAccountID)
        Me.Controls.Add(Me.Panel21)
        Me.Controls.Add(Me.txtModeID)
        Me.Controls.Add(Me.Panel20)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtTotTotal)
        Me.Controls.Add(Me.txtTotDisc)
        Me.Controls.Add(Me.txttotNet)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtBillNo)
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.txtMode)
        Me.Controls.Add(Me.mskChequeDate)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.txtExpenses)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.lblChequeNo)
        Me.Controls.Add(Me.lblChequeDate)
        Me.Controls.Add(Me.TxtChequeNo)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.CbEntryType)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtRemark)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.MskEntryDate)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.dgMode)
        Me.Controls.Add(Me.DgAccountSearch)
        Me.Controls.Add(Me.dtp1)
        Me.KeyPreview = True
        Me.Name = "Bank_Entry"
        Me.Text = "Bank Entry"
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgMode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DgAccountSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblChequeNo As System.Windows.Forms.Label
    Friend WithEvents lblChequeDate As System.Windows.Forms.Label
    Friend WithEvents TxtChequeNo As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtAmount As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents CbEntryType As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents MskEntryDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents txtExpenses As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents mskChequeDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents dgMode As System.Windows.Forms.DataGridView
    Friend WithEvents txtMode As System.Windows.Forms.TextBox
    Friend WithEvents DgAccountSearch As System.Windows.Forms.DataGridView
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
    Friend WithEvents txtAccountID As System.Windows.Forms.TextBox
    Friend WithEvents txtModeID As System.Windows.Forms.TextBox
    Friend WithEvents txtBillNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtTotTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtTotDisc As System.Windows.Forms.TextBox
    Friend WithEvents txttotNet As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Panel20 As System.Windows.Forms.Panel
    Friend WithEvents Panel21 As System.Windows.Forms.Panel
    Friend WithEvents Panel22 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents dtp1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblCapAcBal As System.Windows.Forms.Label
    Friend WithEvents lblAcBal As System.Windows.Forms.Label
End Class
