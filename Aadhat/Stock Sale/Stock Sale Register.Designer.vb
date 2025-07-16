<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Stock_Sale_Register
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Stock_Sale_Register))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BtnPrint = New System.Windows.Forms.Button()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.MsktoDate = New System.Windows.Forms.MaskedTextBox()
        Me.mskFromDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txtSearchPrimary = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtGrandTotal = New System.Windows.Forms.TextBox()
        Me.txtTotCharge = New System.Windows.Forms.TextBox()
        Me.txtTotBasic = New System.Windows.Forms.TextBox()
        Me.txtTotweight = New System.Windows.Forms.TextBox()
        Me.txtTotNug = New System.Windows.Forms.TextBox()
        Me.CbSearchPrimary = New System.Windows.Forms.ComboBox()
        Me.cbSearchSecondary = New System.Windows.Forms.ComboBox()
        Me.txtSearchSecondary = New System.Windows.Forms.TextBox()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTotROff = New System.Windows.Forms.TextBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.dtp2 = New System.Windows.Forms.DateTimePicker()
        Me.dtp1 = New System.Windows.Forms.DateTimePicker()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.lblvoucherCount = New System.Windows.Forms.Label()
        Me.lblbillCount = New System.Windows.Forms.Label()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnPrint
        '
        Me.BtnPrint.BackColor = System.Drawing.Color.MediumAquamarine
        Me.BtnPrint.FlatAppearance.BorderSize = 0
        Me.BtnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPrint.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.BtnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnPrint.Image = CType(resources.GetObject("BtnPrint.Image"), System.Drawing.Image)
        Me.BtnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnPrint.Location = New System.Drawing.Point(360, 116)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(96, 25)
        Me.BtnPrint.TabIndex = 6
        Me.BtnPrint.Text = "&Print"
        Me.BtnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnPrint.UseVisualStyleBackColor = False
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.DarkTurquoise
        Me.btnShow.FlatAppearance.BorderSize = 0
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.btnShow.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnShow.Image = CType(resources.GetObject("btnShow.Image"), System.Drawing.Image)
        Me.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShow.Location = New System.Drawing.Point(262, 116)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(98, 25)
        Me.btnShow.TabIndex = 3
        Me.btnShow.TabStop = False
        Me.btnShow.Text = "&Show"
        Me.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'MsktoDate
        '
        Me.MsktoDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MsktoDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.MsktoDate.Location = New System.Drawing.Point(140, 115)
        Me.MsktoDate.Mask = "00-00-0000"
        Me.MsktoDate.Name = "MsktoDate"
        Me.MsktoDate.Size = New System.Drawing.Size(106, 26)
        Me.MsktoDate.TabIndex = 2
        Me.MsktoDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'mskFromDate
        '
        Me.mskFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskFromDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.mskFromDate.Location = New System.Drawing.Point(12, 115)
        Me.mskFromDate.Mask = "00-00-0000"
        Me.mskFromDate.Name = "mskFromDate"
        Me.mskFromDate.Size = New System.Drawing.Size(113, 26)
        Me.mskFromDate.TabIndex = 1
        Me.mskFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(392, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(430, 48)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "STOCK SALE REGISTER"
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
        Me.btnClose.TabIndex = 91115
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'txtSearchPrimary
        '
        Me.txtSearchPrimary.BackColor = System.Drawing.Color.GhostWhite
        Me.txtSearchPrimary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearchPrimary.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtSearchPrimary.ForeColor = System.Drawing.Color.Black
        Me.txtSearchPrimary.Location = New System.Drawing.Point(632, 115)
        Me.txtSearchPrimary.Name = "txtSearchPrimary"
        Me.txtSearchPrimary.Size = New System.Drawing.Size(190, 26)
        Me.txtSearchPrimary.TabIndex = 40070
        Me.txtSearchPrimary.TabStop = False
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.CadetBlue
        Me.Label10.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label10.Location = New System.Drawing.Point(12, 95)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(129, 21)
        Me.Label10.TabIndex = 40072
        Me.Label10.Text = "Date From"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.CadetBlue
        Me.Label11.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label11.Location = New System.Drawing.Point(140, 95)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(122, 21)
        Me.Label11.TabIndex = 40073
        Me.Label11.Text = "Date to"
        '
        'Label22
        '
        Me.Label22.AllowDrop = True
        Me.Label22.BackColor = System.Drawing.Color.CadetBlue
        Me.Label22.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label22.Location = New System.Drawing.Point(987, 604)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(101, 26)
        Me.Label22.TabIndex = 40191
        Me.Label22.Text = "Total Amount : "
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label21
        '
        Me.Label21.AllowDrop = True
        Me.Label21.BackColor = System.Drawing.Color.CadetBlue
        Me.Label21.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label21.Location = New System.Drawing.Point(699, 604)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(66, 26)
        Me.Label21.TabIndex = 40190
        Me.Label21.Text = "Charges :"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label20
        '
        Me.Label20.AllowDrop = True
        Me.Label20.BackColor = System.Drawing.Color.CadetBlue
        Me.Label20.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label20.Location = New System.Drawing.Point(553, 604)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(49, 26)
        Me.Label20.TabIndex = 40189
        Me.Label20.Text = "Basic :"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label19
        '
        Me.Label19.AllowDrop = True
        Me.Label19.BackColor = System.Drawing.Color.CadetBlue
        Me.Label19.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label19.Location = New System.Drawing.Point(406, 604)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(58, 26)
        Me.Label19.TabIndex = 40188
        Me.Label19.Text = "Weight :"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AllowDrop = True
        Me.Label2.BackColor = System.Drawing.Color.CadetBlue
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label2.Location = New System.Drawing.Point(286, 604)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 26)
        Me.Label2.TabIndex = 40187
        Me.Label2.Text = "Nugs :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TxtGrandTotal
        '
        Me.TxtGrandTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.TxtGrandTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtGrandTotal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TxtGrandTotal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TxtGrandTotal.ForeColor = System.Drawing.Color.Red
        Me.TxtGrandTotal.Location = New System.Drawing.Point(1087, 604)
        Me.TxtGrandTotal.Name = "TxtGrandTotal"
        Me.TxtGrandTotal.ReadOnly = True
        Me.TxtGrandTotal.Size = New System.Drawing.Size(97, 26)
        Me.TxtGrandTotal.TabIndex = 40186
        Me.TxtGrandTotal.TabStop = False
        Me.TxtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotCharge
        '
        Me.txtTotCharge.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotCharge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotCharge.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotCharge.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotCharge.ForeColor = System.Drawing.Color.Red
        Me.txtTotCharge.Location = New System.Drawing.Point(765, 604)
        Me.txtTotCharge.Name = "txtTotCharge"
        Me.txtTotCharge.ReadOnly = True
        Me.txtTotCharge.Size = New System.Drawing.Size(84, 26)
        Me.txtTotCharge.TabIndex = 40185
        Me.txtTotCharge.TabStop = False
        Me.txtTotCharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotBasic
        '
        Me.txtTotBasic.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotBasic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotBasic.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotBasic.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotBasic.ForeColor = System.Drawing.Color.Red
        Me.txtTotBasic.Location = New System.Drawing.Point(602, 604)
        Me.txtTotBasic.Name = "txtTotBasic"
        Me.txtTotBasic.ReadOnly = True
        Me.txtTotBasic.Size = New System.Drawing.Size(97, 26)
        Me.txtTotBasic.TabIndex = 40184
        Me.txtTotBasic.TabStop = False
        Me.txtTotBasic.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotweight
        '
        Me.txtTotweight.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotweight.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotweight.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotweight.ForeColor = System.Drawing.Color.Red
        Me.txtTotweight.Location = New System.Drawing.Point(464, 604)
        Me.txtTotweight.Name = "txtTotweight"
        Me.txtTotweight.ReadOnly = True
        Me.txtTotweight.Size = New System.Drawing.Size(89, 26)
        Me.txtTotweight.TabIndex = 40183
        Me.txtTotweight.TabStop = False
        Me.txtTotweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtTotNug
        '
        Me.txtTotNug.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotNug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotNug.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotNug.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotNug.ForeColor = System.Drawing.Color.Red
        Me.txtTotNug.Location = New System.Drawing.Point(334, 604)
        Me.txtTotNug.Name = "txtTotNug"
        Me.txtTotNug.ReadOnly = True
        Me.txtTotNug.Size = New System.Drawing.Size(73, 26)
        Me.txtTotNug.TabIndex = 40182
        Me.txtTotNug.TabStop = False
        Me.txtTotNug.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CbSearchPrimary
        '
        Me.CbSearchPrimary.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbSearchPrimary.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbSearchPrimary.BackColor = System.Drawing.Color.GhostWhite
        Me.CbSearchPrimary.DropDownHeight = 100
        Me.CbSearchPrimary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbSearchPrimary.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbSearchPrimary.Font = New System.Drawing.Font("Times New Roman", 10.0!)
        Me.CbSearchPrimary.ForeColor = System.Drawing.Color.Black
        Me.CbSearchPrimary.FormattingEnabled = True
        Me.CbSearchPrimary.IntegralHeight = False
        Me.CbSearchPrimary.Items.AddRange(New Object() {"Seller Name", "Customer Name", "Item Name", "Lot No.", "Voucher No.", "Vehicle No.", "Nug", "Weight", "rate", "Basic", "Charges", "Total"})
        Me.CbSearchPrimary.Location = New System.Drawing.Point(456, 117)
        Me.CbSearchPrimary.Name = "CbSearchPrimary"
        Me.CbSearchPrimary.Size = New System.Drawing.Size(176, 23)
        Me.CbSearchPrimary.TabIndex = 40293
        '
        'cbSearchSecondary
        '
        Me.cbSearchSecondary.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbSearchSecondary.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbSearchSecondary.BackColor = System.Drawing.Color.GhostWhite
        Me.cbSearchSecondary.DropDownHeight = 100
        Me.cbSearchSecondary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSearchSecondary.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbSearchSecondary.Font = New System.Drawing.Font("Times New Roman", 10.0!)
        Me.cbSearchSecondary.ForeColor = System.Drawing.Color.Black
        Me.cbSearchSecondary.FormattingEnabled = True
        Me.cbSearchSecondary.IntegralHeight = False
        Me.cbSearchSecondary.Items.AddRange(New Object() {"Nug", "Weight", "rate", "Basic", "Charges", "Total", "Seller Name", "Customer Name", "Item Name", "Lot No.", "Voucher No.", "Vehicle No."})
        Me.cbSearchSecondary.Location = New System.Drawing.Point(822, 117)
        Me.cbSearchSecondary.Name = "cbSearchSecondary"
        Me.cbSearchSecondary.Size = New System.Drawing.Size(183, 23)
        Me.cbSearchSecondary.TabIndex = 40296
        '
        'txtSearchSecondary
        '
        Me.txtSearchSecondary.BackColor = System.Drawing.Color.GhostWhite
        Me.txtSearchSecondary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearchSecondary.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtSearchSecondary.ForeColor = System.Drawing.Color.Black
        Me.txtSearchSecondary.Location = New System.Drawing.Point(1005, 115)
        Me.txtSearchSecondary.Name = "txtSearchSecondary"
        Me.txtSearchSecondary.Size = New System.Drawing.Size(179, 26)
        Me.txtSearchSecondary.TabIndex = 40294
        Me.txtSearchSecondary.TabStop = False
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dg1.ColumnHeadersHeight = 28
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Maroon
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle2
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Gray
        Me.dg1.Location = New System.Drawing.Point(12, 140)
        Me.dg1.MultiSelect = False
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
        Me.dg1.Size = New System.Drawing.Size(1172, 465)
        Me.dg1.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AllowDrop = True
        Me.Label3.BackColor = System.Drawing.Color.CadetBlue
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label3.Location = New System.Drawing.Point(849, 604)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 26)
        Me.Label3.TabIndex = 40292
        Me.Label3.Text = "R. Off :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTotROff
        '
        Me.txtTotROff.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotROff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotROff.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotROff.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotROff.ForeColor = System.Drawing.Color.Red
        Me.txtTotROff.Location = New System.Drawing.Point(903, 604)
        Me.txtTotROff.Name = "txtTotROff"
        Me.txtTotROff.ReadOnly = True
        Me.txtTotROff.Size = New System.Drawing.Size(84, 26)
        Me.txtTotROff.TabIndex = 40291
        Me.txtTotROff.TabStop = False
        Me.txtTotROff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(342, 9)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 91116
        Me.PictureBox4.TabStop = False
        '
        'dtp2
        '
        Me.dtp2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp2.Location = New System.Drawing.Point(153, 115)
        Me.dtp2.Name = "dtp2"
        Me.dtp2.Size = New System.Drawing.Size(109, 26)
        Me.dtp2.TabIndex = 91139
        '
        'dtp1
        '
        Me.dtp1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp1.Location = New System.Drawing.Point(25, 115)
        Me.dtp1.Name = "dtp1"
        Me.dtp1.Size = New System.Drawing.Size(116, 26)
        Me.dtp1.TabIndex = 91142
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.CadetBlue
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label4.Location = New System.Drawing.Point(453, 95)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(178, 21)
        Me.Label4.TabIndex = 91143
        Me.Label4.Text = "Select I"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.CadetBlue
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label5.Location = New System.Drawing.Point(628, 95)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(194, 21)
        Me.Label5.TabIndex = 91144
        Me.Label5.Text = "Search I"
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.CadetBlue
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label6.Location = New System.Drawing.Point(1000, 95)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(184, 21)
        Me.Label6.TabIndex = 91146
        Me.Label6.Text = "Search II"
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.Color.CadetBlue
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label7.Location = New System.Drawing.Point(822, 95)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(178, 21)
        Me.Label7.TabIndex = 91145
        Me.Label7.Text = "Select II"
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.Color.CadetBlue
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label8.Location = New System.Drawing.Point(258, 95)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(198, 21)
        Me.Label8.TabIndex = 91147
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.ForeColor = System.Drawing.Color.Black
        Me.CheckBox1.Location = New System.Drawing.Point(16, 607)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(89, 30)
        Me.CheckBox1.TabIndex = 91148
        Me.CheckBox1.Text = "&Marge Same " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Rate Entries"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'lblvoucherCount
        '
        Me.lblvoucherCount.AutoSize = True
        Me.lblvoucherCount.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblvoucherCount.ForeColor = System.Drawing.Color.Navy
        Me.lblvoucherCount.Location = New System.Drawing.Point(109, 623)
        Me.lblvoucherCount.Name = "lblvoucherCount"
        Me.lblvoucherCount.Size = New System.Drawing.Size(14, 15)
        Me.lblvoucherCount.TabIndex = 91224
        Me.lblvoucherCount.Text = "0"
        Me.lblvoucherCount.Visible = False
        '
        'lblbillCount
        '
        Me.lblbillCount.AutoSize = True
        Me.lblbillCount.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblbillCount.ForeColor = System.Drawing.Color.Navy
        Me.lblbillCount.Location = New System.Drawing.Point(109, 607)
        Me.lblbillCount.Name = "lblbillCount"
        Me.lblbillCount.Size = New System.Drawing.Size(14, 15)
        Me.lblbillCount.TabIndex = 91223
        Me.lblbillCount.Text = "0"
        Me.lblbillCount.Visible = False
        '
        'Stock_Sale_Register
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.lblvoucherCount)
        Me.Controls.Add(Me.lblbillCount)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.mskFromDate)
        Me.Controls.Add(Me.dtp1)
        Me.Controls.Add(Me.MsktoDate)
        Me.Controls.Add(Me.dtp2)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtTotROff)
        Me.Controls.Add(Me.cbSearchSecondary)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtSearchSecondary)
        Me.Controls.Add(Me.CbSearchPrimary)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtGrandTotal)
        Me.Controls.Add(Me.txtTotCharge)
        Me.Controls.Add(Me.txtTotBasic)
        Me.Controls.Add(Me.txtTotweight)
        Me.Controls.Add(Me.txtTotNug)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.txtSearchPrimary)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.BtnPrint)
        Me.Name = "Stock_Sale_Register"
        Me.Text = "Stock Sale"
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnPrint As System.Windows.Forms.Button
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents MsktoDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mskFromDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSearchPrimary As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtGrandTotal As System.Windows.Forms.TextBox
    Friend WithEvents txtTotCharge As System.Windows.Forms.TextBox
    Friend WithEvents txtTotBasic As System.Windows.Forms.TextBox
    Friend WithEvents txtTotweight As System.Windows.Forms.TextBox
    Friend WithEvents txtTotNug As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents CbSearchPrimary As System.Windows.Forms.ComboBox
    Friend WithEvents cbSearchSecondary As System.Windows.Forms.ComboBox
    Friend WithEvents txtSearchSecondary As System.Windows.Forms.TextBox
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtTotROff As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents dtp2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents lblvoucherCount As System.Windows.Forms.Label
    Friend WithEvents lblbillCount As System.Windows.Forms.Label
End Class
