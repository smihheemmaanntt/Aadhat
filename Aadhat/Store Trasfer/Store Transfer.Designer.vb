<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Store_Transfer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Store_Transfer))
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.mskEntryDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.dtp1 = New System.Windows.Forms.DateTimePicker()
        Me.cbAccountName = New System.Windows.Forms.ComboBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.CbStoreFrom = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtVoucherNo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CbStoreto = New System.Windows.Forms.ComboBox()
        Me.cbitemName = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CbLot = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNug = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.txtRemark = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtid = New System.Windows.Forms.TextBox()
        Me.lblItemBalance = New System.Windows.Forms.Label()
        Me.lblLot = New System.Windows.Forms.Label()
        Me.pnlInvoiceID = New System.Windows.Forms.Panel()
        Me.Label56 = New System.Windows.Forms.Label()
        Me.txtInvoiceID = New System.Windows.Forms.TextBox()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlInvoiceID.SuspendLayout()
        Me.SuspendLayout()
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
        Me.btnClose.Size = New System.Drawing.Size(53, 47)
        Me.btnClose.TabIndex = 91121
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'mskEntryDate
        '
        Me.mskEntryDate.BackColor = System.Drawing.Color.GhostWhite
        Me.mskEntryDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskEntryDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.mskEntryDate.ForeColor = System.Drawing.Color.Black
        Me.mskEntryDate.Location = New System.Drawing.Point(8, 139)
        Me.mskEntryDate.Mask = "00-00-0000"
        Me.mskEntryDate.Name = "mskEntryDate"
        Me.mskEntryDate.Size = New System.Drawing.Size(84, 26)
        Me.mskEntryDate.TabIndex = 0
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.Black
        Me.Label25.Location = New System.Drawing.Point(19, 114)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(38, 19)
        Me.Label25.TabIndex = 91255
        Me.Label25.Text = "Date"
        '
        'dtp1
        '
        Me.dtp1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp1.Location = New System.Drawing.Point(14, 139)
        Me.dtp1.Name = "dtp1"
        Me.dtp1.Size = New System.Drawing.Size(96, 26)
        Me.dtp1.TabIndex = 91256
        '
        'cbAccountName
        '
        Me.cbAccountName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbAccountName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbAccountName.BackColor = System.Drawing.SystemColors.Control
        Me.cbAccountName.DropDownHeight = 100
        Me.cbAccountName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAccountName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbAccountName.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cbAccountName.ForeColor = System.Drawing.Color.Navy
        Me.cbAccountName.FormattingEnabled = True
        Me.cbAccountName.IntegralHeight = False
        Me.cbAccountName.Location = New System.Drawing.Point(280, 138)
        Me.cbAccountName.Name = "cbAccountName"
        Me.cbAccountName.Size = New System.Drawing.Size(453, 25)
        Me.cbAccountName.TabIndex = 2
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label42.ForeColor = System.Drawing.Color.Black
        Me.Label42.Location = New System.Drawing.Point(285, 111)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(373, 27)
        Me.Label42.TabIndex = 91258
        Me.Label42.Text = "Account / Supplier / Party  Name"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CbStoreFrom
        '
        Me.CbStoreFrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbStoreFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbStoreFrom.BackColor = System.Drawing.SystemColors.Control
        Me.CbStoreFrom.DropDownHeight = 100
        Me.CbStoreFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbStoreFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbStoreFrom.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CbStoreFrom.ForeColor = System.Drawing.Color.Navy
        Me.CbStoreFrom.FormattingEnabled = True
        Me.CbStoreFrom.IntegralHeight = False
        Me.CbStoreFrom.Location = New System.Drawing.Point(733, 138)
        Me.CbStoreFrom.Name = "CbStoreFrom"
        Me.CbStoreFrom.Size = New System.Drawing.Size(271, 25)
        Me.CbStoreFrom.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(733, 110)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(271, 27)
        Me.Label1.TabIndex = 91260
        Me.Label1.Text = "Store From "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(101, 115)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 19)
        Me.Label2.TabIndex = 91262
        Me.Label2.Text = "Voucher No."
        '
        'txtVoucherNo
        '
        Me.txtVoucherNo.BackColor = System.Drawing.Color.GhostWhite
        Me.txtVoucherNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtVoucherNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtVoucherNo.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtVoucherNo.ForeColor = System.Drawing.Color.Black
        Me.txtVoucherNo.Location = New System.Drawing.Point(109, 139)
        Me.txtVoucherNo.Name = "txtVoucherNo"
        Me.txtVoucherNo.Size = New System.Drawing.Size(170, 26)
        Me.txtVoucherNo.TabIndex = 1
        Me.txtVoucherNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(1001, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(180, 27)
        Me.Label3.TabIndex = 91264
        Me.Label3.Text = "Store to"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CbStoreto
        '
        Me.CbStoreto.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbStoreto.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbStoreto.BackColor = System.Drawing.SystemColors.Control
        Me.CbStoreto.DropDownHeight = 100
        Me.CbStoreto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbStoreto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbStoreto.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CbStoreto.ForeColor = System.Drawing.Color.Navy
        Me.CbStoreto.FormattingEnabled = True
        Me.CbStoreto.IntegralHeight = False
        Me.CbStoreto.Location = New System.Drawing.Point(1003, 138)
        Me.CbStoreto.Name = "CbStoreto"
        Me.CbStoreto.Size = New System.Drawing.Size(178, 25)
        Me.CbStoreto.TabIndex = 4
        '
        'cbitemName
        '
        Me.cbitemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbitemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbitemName.BackColor = System.Drawing.SystemColors.Control
        Me.cbitemName.DropDownHeight = 100
        Me.cbitemName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbitemName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbitemName.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cbitemName.ForeColor = System.Drawing.Color.Navy
        Me.cbitemName.FormattingEnabled = True
        Me.cbitemName.IntegralHeight = False
        Me.cbitemName.Location = New System.Drawing.Point(10, 198)
        Me.cbitemName.Name = "cbitemName"
        Me.cbitemName.Size = New System.Drawing.Size(494, 25)
        Me.cbitemName.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(8, 169)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(271, 27)
        Me.Label4.TabIndex = 91266
        Me.Label4.Text = "Item Name"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CbLot
        '
        Me.CbLot.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbLot.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbLot.BackColor = System.Drawing.SystemColors.Control
        Me.CbLot.DropDownHeight = 100
        Me.CbLot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbLot.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbLot.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CbLot.ForeColor = System.Drawing.Color.Navy
        Me.CbLot.FormattingEnabled = True
        Me.CbLot.IntegralHeight = False
        Me.CbLot.Location = New System.Drawing.Point(504, 197)
        Me.CbLot.Name = "CbLot"
        Me.CbLot.Size = New System.Drawing.Size(500, 25)
        Me.CbLot.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(500, 169)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(150, 27)
        Me.Label5.TabIndex = 91268
        Me.Label5.Text = "Lot No"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtNug
        '
        Me.txtNug.BackColor = System.Drawing.Color.GhostWhite
        Me.txtNug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNug.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNug.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtNug.ForeColor = System.Drawing.Color.Black
        Me.txtNug.Location = New System.Drawing.Point(1003, 198)
        Me.txtNug.Name = "txtNug"
        Me.txtNug.Size = New System.Drawing.Size(178, 26)
        Me.txtNug.TabIndex = 7
        Me.txtNug.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(1001, 169)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(150, 27)
        Me.Label6.TabIndex = 91271
        Me.Label6.Text = "Nug"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightGray
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Maroon
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle5
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Gray
        Me.dg1.Location = New System.Drawing.Point(9, 224)
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersVisible = False
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1172, 334)
        Me.dg1.TabIndex = 91272
        '
        'txtRemark
        '
        Me.txtRemark.BackColor = System.Drawing.Color.GhostWhite
        Me.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtRemark.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRemark.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtRemark.ForeColor = System.Drawing.Color.Black
        Me.txtRemark.Location = New System.Drawing.Point(9, 590)
        Me.txtRemark.Name = "txtRemark"
        Me.txtRemark.Size = New System.Drawing.Size(575, 26)
        Me.txtRemark.TabIndex = 91273
        Me.txtRemark.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(5, 560)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(150, 27)
        Me.Label7.TabIndex = 91274
        Me.Label7.Text = "Remark"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.ForestGreen
        Me.btnPrint.FlatAppearance.BorderSize = 0
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(979, 67)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 38)
        Me.btnPrint.TabIndex = 91277
        Me.btnPrint.TabStop = False
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'BtnDelete
        '
        Me.BtnDelete.BackColor = System.Drawing.Color.DarkRed
        Me.BtnDelete.FlatAppearance.BorderSize = 0
        Me.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDelete.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BtnDelete.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnDelete.Image = CType(resources.GetObject("BtnDelete.Image"), System.Drawing.Image)
        Me.BtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnDelete.Location = New System.Drawing.Point(879, 67)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(100, 38)
        Me.BtnDelete.TabIndex = 91276
        Me.BtnDelete.TabStop = False
        Me.BtnDelete.Text = "&Delete"
        Me.BtnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnDelete.UseVisualStyleBackColor = False
        '
        'BtnSave
        '
        Me.BtnSave.BackColor = System.Drawing.Color.DarkTurquoise
        Me.BtnSave.FlatAppearance.BorderSize = 0
        Me.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSave.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BtnSave.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnSave.Image = Global.Aadhat.My.Resources.Resources.Save
        Me.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnSave.Location = New System.Drawing.Point(1079, 67)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(105, 38)
        Me.BtnSave.TabIndex = 91275
        Me.BtnSave.TabStop = False
        Me.BtnSave.Text = "&Save"
        Me.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnSave.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(402, 3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(319, 48)
        Me.Label8.TabIndex = 91278
        Me.Label8.Text = "STORE TRANFER"
        '
        'txtid
        '
        Me.txtid.BackColor = System.Drawing.Color.AliceBlue
        Me.txtid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtid.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtid.ForeColor = System.Drawing.Color.Teal
        Me.txtid.Location = New System.Drawing.Point(55, 12)
        Me.txtid.Name = "txtid"
        Me.txtid.Size = New System.Drawing.Size(53, 26)
        Me.txtid.TabIndex = 91279
        Me.txtid.TabStop = False
        Me.txtid.Visible = False
        '
        'lblItemBalance
        '
        Me.lblItemBalance.AutoSize = True
        Me.lblItemBalance.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblItemBalance.ForeColor = System.Drawing.Color.Navy
        Me.lblItemBalance.Location = New System.Drawing.Point(105, 176)
        Me.lblItemBalance.Name = "lblItemBalance"
        Me.lblItemBalance.Size = New System.Drawing.Size(36, 15)
        Me.lblItemBalance.TabIndex = 91280
        Me.lblItemBalance.Text = "Bal. :"
        Me.lblItemBalance.Visible = False
        '
        'lblLot
        '
        Me.lblLot.AutoSize = True
        Me.lblLot.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblLot.ForeColor = System.Drawing.Color.Navy
        Me.lblLot.Location = New System.Drawing.Point(571, 176)
        Me.lblLot.Name = "lblLot"
        Me.lblLot.Size = New System.Drawing.Size(35, 15)
        Me.lblLot.TabIndex = 91281
        Me.lblLot.Text = "Lot. :"
        Me.lblLot.Visible = False
        '
        'pnlInvoiceID
        '
        Me.pnlInvoiceID.Controls.Add(Me.Label56)
        Me.pnlInvoiceID.Controls.Add(Me.txtInvoiceID)
        Me.pnlInvoiceID.Location = New System.Drawing.Point(109, 165)
        Me.pnlInvoiceID.Name = "pnlInvoiceID"
        Me.pnlInvoiceID.Size = New System.Drawing.Size(170, 74)
        Me.pnlInvoiceID.TabIndex = 91282
        Me.pnlInvoiceID.Visible = False
        '
        'Label56
        '
        Me.Label56.AutoSize = True
        Me.Label56.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label56.ForeColor = System.Drawing.Color.Black
        Me.Label56.Location = New System.Drawing.Point(30, 13)
        Me.Label56.Name = "Label56"
        Me.Label56.Size = New System.Drawing.Size(73, 19)
        Me.Label56.TabIndex = 40073
        Me.Label56.Text = "Invoice ID"
        '
        'txtInvoiceID
        '
        Me.txtInvoiceID.BackColor = System.Drawing.Color.GhostWhite
        Me.txtInvoiceID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtInvoiceID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtInvoiceID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtInvoiceID.ForeColor = System.Drawing.Color.Black
        Me.txtInvoiceID.Location = New System.Drawing.Point(2, 37)
        Me.txtInvoiceID.Name = "txtInvoiceID"
        Me.txtInvoiceID.Size = New System.Drawing.Size(164, 26)
        Me.txtInvoiceID.TabIndex = 2
        Me.txtInvoiceID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Store_Transfer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.pnlInvoiceID)
        Me.Controls.Add(Me.lblLot)
        Me.Controls.Add(Me.lblItemBalance)
        Me.Controls.Add(Me.txtid)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtRemark)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtNug)
        Me.Controls.Add(Me.CbLot)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cbitemName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.CbStoreto)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtVoucherNo)
        Me.Controls.Add(Me.CbStoreFrom)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbAccountName)
        Me.Controls.Add(Me.Label42)
        Me.Controls.Add(Me.mskEntryDate)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.dtp1)
        Me.Controls.Add(Me.btnClose)
        Me.Name = "Store_Transfer"
        Me.Text = "Store Transfer"
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlInvoiceID.ResumeLayout(False)
        Me.pnlInvoiceID.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents mskEntryDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents dtp1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents cbAccountName As System.Windows.Forms.ComboBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents CbStoreFrom As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtVoucherNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CbStoreto As System.Windows.Forms.ComboBox
    Friend WithEvents cbitemName As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CbLot As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtNug As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents txtRemark As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtid As System.Windows.Forms.TextBox
    Friend WithEvents lblItemBalance As System.Windows.Forms.Label
    Friend WithEvents lblLot As System.Windows.Forms.Label
    Friend WithEvents pnlInvoiceID As System.Windows.Forms.Panel
    Friend WithEvents Label56 As System.Windows.Forms.Label
    Friend WithEvents txtInvoiceID As System.Windows.Forms.TextBox
End Class
