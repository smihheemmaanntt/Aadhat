<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Total_sale_Report
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Total_sale_Report))
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblRounfOff = New System.Windows.Forms.Label()
        Me.lblPageNumber = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.btnPrintNew = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.radioCurrent = New System.Windows.Forms.RadioButton()
        Me.radioAll = New System.Windows.Forms.RadioButton()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel29 = New System.Windows.Forms.Panel()
        Me.Panel28 = New System.Windows.Forms.Panel()
        Me.Panel27 = New System.Windows.Forms.Panel()
        Me.Panel26 = New System.Windows.Forms.Panel()
        Me.Panel25 = New System.Windows.Forms.Panel()
        Me.txtTotalRoff = New System.Windows.Forms.TextBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.TxtGrandTotal = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtTotCharge = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtTotBasic = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtTotweight = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtTotNug = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblTotalRecordCount = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.lblCharges = New System.Windows.Forms.Label()
        Me.lblBasic = New System.Windows.Forms.Label()
        Me.lblTotalWeight = New System.Windows.Forms.Label()
        Me.lbltotNug = New System.Windows.Forms.Label()
        Me.pnlprint = New System.Windows.Forms.Panel()
        Me.btnFirst = New System.Windows.Forms.Button()
        Me.btnLast = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrevious = New System.Windows.Forms.Button()
        Me.lblTotalRecord = New System.Windows.Forms.Label()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.lblRecordCount = New System.Windows.Forms.Label()
        Me.txtThirdSearch = New System.Windows.Forms.TextBox()
        Me.txtSecondarySearch = New System.Windows.Forms.TextBox()
        Me.txtPrimarySearch = New System.Windows.Forms.TextBox()
        Me.BtnPrint = New System.Windows.Forms.Button()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.MsktoDate = New System.Windows.Forms.MaskedTextBox()
        Me.mskFromDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.dtp2 = New System.Windows.Forms.DateTimePicker()
        Me.dtp1 = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlprint.SuspendLayout()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(1018, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(118, 21)
        Me.Label6.TabIndex = 91192
        Me.Label6.Text = "Total Amount"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(833, 64)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(99, 21)
        Me.Label5.TabIndex = 91191
        Me.Label5.Text = "Item Name"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(541, 64)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(132, 21)
        Me.Label11.TabIndex = 91190
        Me.Label11.Text = "Account Name"
        '
        'lblRounfOff
        '
        Me.lblRounfOff.AutoSize = True
        Me.lblRounfOff.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblRounfOff.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblRounfOff.Location = New System.Drawing.Point(627, 631)
        Me.lblRounfOff.Name = "lblRounfOff"
        Me.lblRounfOff.Size = New System.Drawing.Size(14, 16)
        Me.lblRounfOff.TabIndex = 91187
        Me.lblRounfOff.Text = "0"
        Me.lblRounfOff.Visible = False
        '
        'lblPageNumber
        '
        Me.lblPageNumber.AutoSize = True
        Me.lblPageNumber.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblPageNumber.ForeColor = System.Drawing.Color.Coral
        Me.lblPageNumber.Location = New System.Drawing.Point(866, 616)
        Me.lblPageNumber.Name = "lblPageNumber"
        Me.lblPageNumber.Size = New System.Drawing.Size(15, 16)
        Me.lblPageNumber.TabIndex = 91186
        Me.lblPageNumber.Text = "0"
        Me.lblPageNumber.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(354, 5)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 91185
        Me.PictureBox1.TabStop = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.BackColor = System.Drawing.Color.GhostWhite
        Me.ProgressBar1.Location = New System.Drawing.Point(4, 125)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(248, 23)
        Me.ProgressBar1.TabIndex = 40280
        Me.ProgressBar1.Visible = False
        '
        'btnPrintNew
        '
        Me.btnPrintNew.BackColor = System.Drawing.Color.ForestGreen
        Me.btnPrintNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrintNew.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnPrintNew.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnPrintNew.Image = CType(resources.GetObject("btnPrintNew.Image"), System.Drawing.Image)
        Me.btnPrintNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrintNew.Location = New System.Drawing.Point(257, 122)
        Me.btnPrintNew.Name = "btnPrintNew"
        Me.btnPrintNew.Size = New System.Drawing.Size(92, 30)
        Me.btnPrintNew.TabIndex = 2
        Me.btnPrintNew.Text = "&Print"
        Me.btnPrintNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrintNew.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Coral
        Me.Label3.Location = New System.Drawing.Point(148, 69)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(15, 16)
        Me.Label3.TabIndex = 40279
        Me.Label3.Text = "0"
        Me.Label3.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Coral
        Me.Label2.Location = New System.Drawing.Point(148, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(15, 16)
        Me.Label2.TabIndex = 40278
        Me.Label2.Text = "0"
        Me.Label2.Visible = False
        '
        'radioCurrent
        '
        Me.radioCurrent.AutoSize = True
        Me.radioCurrent.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.radioCurrent.Location = New System.Drawing.Point(17, 40)
        Me.radioCurrent.Name = "radioCurrent"
        Me.radioCurrent.Size = New System.Drawing.Size(112, 21)
        Me.radioCurrent.TabIndex = 0
        Me.radioCurrent.TabStop = True
        Me.radioCurrent.Text = "Current Page"
        Me.radioCurrent.UseVisualStyleBackColor = True
        '
        'radioAll
        '
        Me.radioAll.AutoSize = True
        Me.radioAll.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.radioAll.Location = New System.Drawing.Point(17, 67)
        Me.radioAll.Name = "radioAll"
        Me.radioAll.Size = New System.Drawing.Size(84, 21)
        Me.radioAll.TabIndex = 1
        Me.radioAll.TabStop = True
        Me.radioAll.Text = "All Pages"
        Me.radioAll.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(1142, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 47)
        Me.btnClose.TabIndex = 91166
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Location = New System.Drawing.Point(818, 612)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(164, 1)
        Me.Panel2.TabIndex = 91184
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(410, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(394, 48)
        Me.Label1.TabIndex = 91143
        Me.Label1.Text = "TOTAL SALE REPORT"
        '
        'Panel29
        '
        Me.Panel29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel29.Location = New System.Drawing.Point(995, 613)
        Me.Panel29.Name = "Panel29"
        Me.Panel29.Size = New System.Drawing.Size(189, 1)
        Me.Panel29.TabIndex = 91182
        '
        'Panel28
        '
        Me.Panel28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel28.Location = New System.Drawing.Point(648, 613)
        Me.Panel28.Name = "Panel28"
        Me.Panel28.Size = New System.Drawing.Size(155, 1)
        Me.Panel28.TabIndex = 91183
        '
        'Panel27
        '
        Me.Panel27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel27.Location = New System.Drawing.Point(432, 612)
        Me.Panel27.Name = "Panel27"
        Me.Panel27.Size = New System.Drawing.Size(192, 1)
        Me.Panel27.TabIndex = 91181
        '
        'Panel26
        '
        Me.Panel26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel26.Location = New System.Drawing.Point(250, 612)
        Me.Panel26.Name = "Panel26"
        Me.Panel26.Size = New System.Drawing.Size(169, 1)
        Me.Panel26.TabIndex = 91180
        '
        'Panel25
        '
        Me.Panel25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel25.Location = New System.Drawing.Point(86, 612)
        Me.Panel25.Name = "Panel25"
        Me.Panel25.Size = New System.Drawing.Size(154, 1)
        Me.Panel25.TabIndex = 91179
        '
        'txtTotalRoff
        '
        Me.txtTotalRoff.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotalRoff.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotalRoff.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotalRoff.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotalRoff.ForeColor = System.Drawing.Color.Red
        Me.txtTotalRoff.Location = New System.Drawing.Point(887, 586)
        Me.txtTotalRoff.Name = "txtTotalRoff"
        Me.txtTotalRoff.Size = New System.Drawing.Size(96, 20)
        Me.txtTotalRoff.TabIndex = 91178
        Me.txtTotalRoff.TabStop = False
        Me.txtTotalRoff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label37
        '
        Me.Label37.AllowDrop = True
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label37.ForeColor = System.Drawing.Color.Black
        Me.Label37.Location = New System.Drawing.Point(818, 586)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(69, 21)
        Me.Label37.TabIndex = 91177
        Me.Label37.Text = "Round :"
        '
        'TxtGrandTotal
        '
        Me.TxtGrandTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.TxtGrandTotal.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtGrandTotal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TxtGrandTotal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TxtGrandTotal.ForeColor = System.Drawing.Color.Red
        Me.TxtGrandTotal.Location = New System.Drawing.Point(1048, 587)
        Me.TxtGrandTotal.Name = "TxtGrandTotal"
        Me.TxtGrandTotal.ReadOnly = True
        Me.TxtGrandTotal.Size = New System.Drawing.Size(135, 20)
        Me.TxtGrandTotal.TabIndex = 91176
        Me.TxtGrandTotal.TabStop = False
        Me.TxtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label22
        '
        Me.Label22.AllowDrop = True
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.Black
        Me.Label22.Location = New System.Drawing.Point(989, 587)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(61, 21)
        Me.Label22.TabIndex = 91173
        Me.Label22.Text = "Total : "
        '
        'txtTotCharge
        '
        Me.txtTotCharge.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotCharge.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotCharge.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotCharge.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotCharge.ForeColor = System.Drawing.Color.Red
        Me.txtTotCharge.Location = New System.Drawing.Point(725, 587)
        Me.txtTotCharge.Name = "txtTotCharge"
        Me.txtTotCharge.Size = New System.Drawing.Size(79, 20)
        Me.txtTotCharge.TabIndex = 91175
        Me.txtTotCharge.TabStop = False
        Me.txtTotCharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label21
        '
        Me.Label21.AllowDrop = True
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label21.ForeColor = System.Drawing.Color.Black
        Me.Label21.Location = New System.Drawing.Point(640, 587)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(84, 21)
        Me.Label21.TabIndex = 91171
        Me.Label21.Text = "Charges :"
        '
        'txtTotBasic
        '
        Me.txtTotBasic.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotBasic.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotBasic.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotBasic.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotBasic.ForeColor = System.Drawing.Color.Red
        Me.txtTotBasic.Location = New System.Drawing.Point(488, 586)
        Me.txtTotBasic.Name = "txtTotBasic"
        Me.txtTotBasic.Size = New System.Drawing.Size(135, 20)
        Me.txtTotBasic.TabIndex = 91174
        Me.txtTotBasic.TabStop = False
        Me.txtTotBasic.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label20
        '
        Me.Label20.AllowDrop = True
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label20.ForeColor = System.Drawing.Color.Black
        Me.Label20.Location = New System.Drawing.Point(429, 586)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(57, 21)
        Me.Label20.TabIndex = 91169
        Me.Label20.Text = "Basic :"
        '
        'txtTotweight
        '
        Me.txtTotweight.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotweight.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotweight.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotweight.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotweight.ForeColor = System.Drawing.Color.Red
        Me.txtTotweight.Location = New System.Drawing.Point(320, 586)
        Me.txtTotweight.Name = "txtTotweight"
        Me.txtTotweight.Size = New System.Drawing.Size(100, 20)
        Me.txtTotweight.TabIndex = 91172
        Me.txtTotweight.TabStop = False
        Me.txtTotweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label19
        '
        Me.Label19.AllowDrop = True
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label19.ForeColor = System.Drawing.Color.Black
        Me.Label19.Location = New System.Drawing.Point(247, 585)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(74, 21)
        Me.Label19.TabIndex = 91168
        Me.Label19.Text = "Weight :"
        '
        'txtTotNug
        '
        Me.txtTotNug.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotNug.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtTotNug.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotNug.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotNug.ForeColor = System.Drawing.Color.Red
        Me.txtTotNug.Location = New System.Drawing.Point(141, 586)
        Me.txtTotNug.Name = "txtTotNug"
        Me.txtTotNug.ReadOnly = True
        Me.txtTotNug.Size = New System.Drawing.Size(100, 20)
        Me.txtTotNug.TabIndex = 91170
        Me.txtTotNug.TabStop = False
        Me.txtTotNug.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AllowDrop = True
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(83, 586)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 21)
        Me.Label4.TabIndex = 91167
        Me.Label4.Text = "Nugs :"
        '
        'lblTotalRecordCount
        '
        Me.lblTotalRecordCount.AutoSize = True
        Me.lblTotalRecordCount.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTotalRecordCount.ForeColor = System.Drawing.Color.Coral
        Me.lblTotalRecordCount.Location = New System.Drawing.Point(8, 612)
        Me.lblTotalRecordCount.Name = "lblTotalRecordCount"
        Me.lblTotalRecordCount.Size = New System.Drawing.Size(15, 16)
        Me.lblTotalRecordCount.TabIndex = 91165
        Me.lblTotalRecordCount.Text = "0"
        Me.lblTotalRecordCount.Visible = False
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotal.Location = New System.Drawing.Point(752, 631)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(14, 16)
        Me.lblTotal.TabIndex = 91164
        Me.lblTotal.Text = "0"
        Me.lblTotal.Visible = False
        '
        'lblCharges
        '
        Me.lblCharges.AutoSize = True
        Me.lblCharges.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblCharges.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblCharges.Location = New System.Drawing.Point(523, 631)
        Me.lblCharges.Name = "lblCharges"
        Me.lblCharges.Size = New System.Drawing.Size(14, 16)
        Me.lblCharges.TabIndex = 91163
        Me.lblCharges.Text = "0"
        Me.lblCharges.Visible = False
        '
        'lblBasic
        '
        Me.lblBasic.AutoSize = True
        Me.lblBasic.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblBasic.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblBasic.Location = New System.Drawing.Point(404, 631)
        Me.lblBasic.Name = "lblBasic"
        Me.lblBasic.Size = New System.Drawing.Size(14, 16)
        Me.lblBasic.TabIndex = 91162
        Me.lblBasic.Text = "0"
        Me.lblBasic.Visible = False
        '
        'lblTotalWeight
        '
        Me.lblTotalWeight.AutoSize = True
        Me.lblTotalWeight.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTotalWeight.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTotalWeight.Location = New System.Drawing.Point(271, 631)
        Me.lblTotalWeight.Name = "lblTotalWeight"
        Me.lblTotalWeight.Size = New System.Drawing.Size(14, 16)
        Me.lblTotalWeight.TabIndex = 91161
        Me.lblTotalWeight.Text = "0"
        Me.lblTotalWeight.Visible = False
        '
        'lbltotNug
        '
        Me.lbltotNug.AutoSize = True
        Me.lbltotNug.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lbltotNug.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lbltotNug.Location = New System.Drawing.Point(140, 631)
        Me.lbltotNug.Name = "lbltotNug"
        Me.lbltotNug.Size = New System.Drawing.Size(14, 16)
        Me.lbltotNug.TabIndex = 91160
        Me.lbltotNug.Text = "0"
        Me.lbltotNug.Visible = False
        '
        'pnlprint
        '
        Me.pnlprint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlprint.Controls.Add(Me.ProgressBar1)
        Me.pnlprint.Controls.Add(Me.btnPrintNew)
        Me.pnlprint.Controls.Add(Me.Label3)
        Me.pnlprint.Controls.Add(Me.Label2)
        Me.pnlprint.Controls.Add(Me.radioCurrent)
        Me.pnlprint.Controls.Add(Me.radioAll)
        Me.pnlprint.Location = New System.Drawing.Point(251, 114)
        Me.pnlprint.Name = "pnlprint"
        Me.pnlprint.Size = New System.Drawing.Size(354, 170)
        Me.pnlprint.TabIndex = 91159
        Me.pnlprint.Visible = False
        '
        'btnFirst
        '
        Me.btnFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFirst.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnFirst.ForeColor = System.Drawing.Color.Navy
        Me.btnFirst.Location = New System.Drawing.Point(992, 623)
        Me.btnFirst.Name = "btnFirst"
        Me.btnFirst.Size = New System.Drawing.Size(44, 23)
        Me.btnFirst.TabIndex = 91158
        Me.btnFirst.Text = "|<"
        Me.btnFirst.UseVisualStyleBackColor = True
        '
        'btnLast
        '
        Me.btnLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLast.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnLast.ForeColor = System.Drawing.Color.Navy
        Me.btnLast.Location = New System.Drawing.Point(1142, 623)
        Me.btnLast.Name = "btnLast"
        Me.btnLast.Size = New System.Drawing.Size(44, 23)
        Me.btnLast.TabIndex = 91157
        Me.btnLast.Text = ">|"
        Me.btnLast.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnNext.ForeColor = System.Drawing.Color.Navy
        Me.btnNext.Location = New System.Drawing.Point(1092, 623)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(44, 23)
        Me.btnNext.TabIndex = 91156
        Me.btnNext.Text = ">>"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrevious
        '
        Me.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrevious.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnPrevious.ForeColor = System.Drawing.Color.Navy
        Me.btnPrevious.Location = New System.Drawing.Point(1042, 623)
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(44, 23)
        Me.btnPrevious.TabIndex = 91155
        Me.btnPrevious.Text = "<<"
        Me.btnPrevious.UseVisualStyleBackColor = True
        '
        'lblTotalRecord
        '
        Me.lblTotalRecord.AutoSize = True
        Me.lblTotalRecord.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTotalRecord.ForeColor = System.Drawing.Color.Coral
        Me.lblTotalRecord.Location = New System.Drawing.Point(866, 631)
        Me.lblTotalRecord.Name = "lblTotalRecord"
        Me.lblTotalRecord.Size = New System.Drawing.Size(15, 16)
        Me.lblTotalRecord.TabIndex = 91154
        Me.lblTotalRecord.Text = "0"
        Me.lblTotalRecord.Visible = False
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Times New Roman", 10.0!)
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle10
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Tan
        Me.dg1.Location = New System.Drawing.Point(5, 114)
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersDefaultCellStyle = DataGridViewCellStyle11
        Me.dg1.RowHeadersVisible = False
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1172, 467)
        Me.dg1.TabIndex = 91153
        Me.dg1.TabStop = False
        '
        'lblRecordCount
        '
        Me.lblRecordCount.AutoSize = True
        Me.lblRecordCount.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblRecordCount.ForeColor = System.Drawing.Color.Coral
        Me.lblRecordCount.Location = New System.Drawing.Point(8, 631)
        Me.lblRecordCount.Name = "lblRecordCount"
        Me.lblRecordCount.Size = New System.Drawing.Size(15, 16)
        Me.lblRecordCount.TabIndex = 91150
        Me.lblRecordCount.Text = "0"
        Me.lblRecordCount.Visible = False
        '
        'txtThirdSearch
        '
        Me.txtThirdSearch.BackColor = System.Drawing.Color.GhostWhite
        Me.txtThirdSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtThirdSearch.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtThirdSearch.ForeColor = System.Drawing.Color.Gray
        Me.txtThirdSearch.Location = New System.Drawing.Point(1022, 88)
        Me.txtThirdSearch.Name = "txtThirdSearch"
        Me.txtThirdSearch.Size = New System.Drawing.Size(155, 27)
        Me.txtThirdSearch.TabIndex = 91149
        Me.txtThirdSearch.TabStop = False
        '
        'txtSecondarySearch
        '
        Me.txtSecondarySearch.BackColor = System.Drawing.Color.GhostWhite
        Me.txtSecondarySearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSecondarySearch.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtSecondarySearch.ForeColor = System.Drawing.Color.Gray
        Me.txtSecondarySearch.Location = New System.Drawing.Point(837, 88)
        Me.txtSecondarySearch.Name = "txtSecondarySearch"
        Me.txtSecondarySearch.Size = New System.Drawing.Size(186, 27)
        Me.txtSecondarySearch.TabIndex = 91148
        Me.txtSecondarySearch.TabStop = False
        '
        'txtPrimarySearch
        '
        Me.txtPrimarySearch.BackColor = System.Drawing.Color.GhostWhite
        Me.txtPrimarySearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPrimarySearch.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtPrimarySearch.ForeColor = System.Drawing.Color.Gray
        Me.txtPrimarySearch.Location = New System.Drawing.Point(545, 88)
        Me.txtPrimarySearch.Name = "txtPrimarySearch"
        Me.txtPrimarySearch.Size = New System.Drawing.Size(293, 27)
        Me.txtPrimarySearch.TabIndex = 91147
        Me.txtPrimarySearch.TabStop = False
        '
        'BtnPrint
        '
        Me.BtnPrint.BackColor = System.Drawing.Color.Sienna
        Me.BtnPrint.FlatAppearance.BorderSize = 0
        Me.BtnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPrint.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BtnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnPrint.Image = CType(resources.GetObject("BtnPrint.Image"), System.Drawing.Image)
        Me.BtnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnPrint.Location = New System.Drawing.Point(437, 88)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(108, 27)
        Me.BtnPrint.TabIndex = 91146
        Me.BtnPrint.Text = "&Print"
        Me.BtnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnPrint.UseVisualStyleBackColor = False
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.DarkTurquoise
        Me.btnShow.FlatAppearance.BorderSize = 0
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnShow.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnShow.Image = CType(resources.GetObject("btnShow.Image"), System.Drawing.Image)
        Me.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShow.Location = New System.Drawing.Point(327, 88)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(110, 27)
        Me.btnShow.TabIndex = 91145
        Me.btnShow.Text = "&Show"
        Me.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'MsktoDate
        '
        Me.MsktoDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.MsktoDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MsktoDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.MsktoDate.Location = New System.Drawing.Point(218, 88)
        Me.MsktoDate.Mask = "00-00-0000"
        Me.MsktoDate.Name = "MsktoDate"
        Me.MsktoDate.Size = New System.Drawing.Size(92, 27)
        Me.MsktoDate.TabIndex = 91144
        '
        'mskFromDate
        '
        Me.mskFromDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.mskFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskFromDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.mskFromDate.Location = New System.Drawing.Point(69, 88)
        Me.mskFromDate.Mask = "00-00-0000"
        Me.mskFromDate.Name = "mskFromDate"
        Me.mskFromDate.Size = New System.Drawing.Size(92, 27)
        Me.mskFromDate.TabIndex = 91142
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label10.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label10.Location = New System.Drawing.Point(177, 88)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(42, 27)
        Me.Label10.TabIndex = 91152
        Me.Label10.Text = "To :"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dtp2
        '
        Me.dtp2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp2.Location = New System.Drawing.Point(218, 88)
        Me.dtp2.Name = "dtp2"
        Me.dtp2.Size = New System.Drawing.Size(109, 27)
        Me.dtp2.TabIndex = 91188
        '
        'dtp1
        '
        Me.dtp1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp1.Location = New System.Drawing.Point(69, 88)
        Me.dtp1.Name = "dtp1"
        Me.dtp1.Size = New System.Drawing.Size(109, 27)
        Me.dtp1.TabIndex = 91189
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label9.Location = New System.Drawing.Point(5, 88)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(65, 27)
        Me.Label9.TabIndex = 91151
        Me.Label9.Text = "From : "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Total_sale_Report
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.lblRounfOff)
        Me.Controls.Add(Me.lblPageNumber)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel29)
        Me.Controls.Add(Me.Panel28)
        Me.Controls.Add(Me.Panel27)
        Me.Controls.Add(Me.Panel26)
        Me.Controls.Add(Me.Panel25)
        Me.Controls.Add(Me.txtTotalRoff)
        Me.Controls.Add(Me.Label37)
        Me.Controls.Add(Me.TxtGrandTotal)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.txtTotCharge)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.txtTotBasic)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.txtTotweight)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.txtTotNug)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblTotalRecordCount)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblCharges)
        Me.Controls.Add(Me.lblBasic)
        Me.Controls.Add(Me.lblTotalWeight)
        Me.Controls.Add(Me.lbltotNug)
        Me.Controls.Add(Me.pnlprint)
        Me.Controls.Add(Me.btnFirst)
        Me.Controls.Add(Me.btnLast)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnPrevious)
        Me.Controls.Add(Me.lblTotalRecord)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.lblRecordCount)
        Me.Controls.Add(Me.txtThirdSearch)
        Me.Controls.Add(Me.txtSecondarySearch)
        Me.Controls.Add(Me.txtPrimarySearch)
        Me.Controls.Add(Me.BtnPrint)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.MsktoDate)
        Me.Controls.Add(Me.mskFromDate)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.dtp2)
        Me.Controls.Add(Me.dtp1)
        Me.Controls.Add(Me.Label9)
        Me.Name = "Total_sale_Report"
        Me.Text = "Total_sale_Report"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlprint.ResumeLayout(False)
        Me.pnlprint.PerformLayout()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblRounfOff As System.Windows.Forms.Label
    Friend WithEvents lblPageNumber As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents btnPrintNew As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents radioCurrent As System.Windows.Forms.RadioButton
    Friend WithEvents radioAll As System.Windows.Forms.RadioButton
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel29 As System.Windows.Forms.Panel
    Friend WithEvents Panel28 As System.Windows.Forms.Panel
    Friend WithEvents Panel27 As System.Windows.Forms.Panel
    Friend WithEvents Panel26 As System.Windows.Forms.Panel
    Friend WithEvents Panel25 As System.Windows.Forms.Panel
    Friend WithEvents txtTotalRoff As System.Windows.Forms.TextBox
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents TxtGrandTotal As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtTotCharge As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtTotBasic As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtTotweight As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtTotNug As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblTotalRecordCount As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblCharges As System.Windows.Forms.Label
    Friend WithEvents lblBasic As System.Windows.Forms.Label
    Friend WithEvents lblTotalWeight As System.Windows.Forms.Label
    Friend WithEvents lbltotNug As System.Windows.Forms.Label
    Friend WithEvents pnlprint As System.Windows.Forms.Panel
    Friend WithEvents btnFirst As System.Windows.Forms.Button
    Friend WithEvents btnLast As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrevious As System.Windows.Forms.Button
    Friend WithEvents lblTotalRecord As System.Windows.Forms.Label
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents lblRecordCount As System.Windows.Forms.Label
    Friend WithEvents txtThirdSearch As System.Windows.Forms.TextBox
    Friend WithEvents txtSecondarySearch As System.Windows.Forms.TextBox
    Friend WithEvents txtPrimarySearch As System.Windows.Forms.TextBox
    Friend WithEvents BtnPrint As System.Windows.Forms.Button
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents MsktoDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mskFromDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents dtp2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
