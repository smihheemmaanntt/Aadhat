<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CrateCrateAccountCumMarkaLedger
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CrateCrateAccountCumMarkaLedger))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.MsktoDate = New System.Windows.Forms.MaskedTextBox()
        Me.mskFromDate = New System.Windows.Forms.MaskedTextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtBalAmt = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtcrAmt = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtDramt = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtOpBal = New System.Windows.Forms.TextBox()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.BtnPrint = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.txtAccountID = New System.Windows.Forms.TextBox()
        Me.DgAccountSearch = New System.Windows.Forms.DataGridView()
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.CbPer = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.dtp1 = New System.Windows.Forms.DateTimePicker()
        Me.dtp2 = New System.Windows.Forms.DateTimePicker()
        Me.lblCrateDetails = New System.Windows.Forms.Label()
        Me.lblCrate = New System.Windows.Forms.Label()
        Me.pnlWahtsappNo = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cbType = New System.Windows.Forms.ComboBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtWhatsappNo = New System.Windows.Forms.TextBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.CkHideOpBal = New System.Windows.Forms.CheckBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        CType(Me.DgAccountSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlWahtsappNo.SuspendLayout()
        Me.SuspendLayout()
        '
        'MsktoDate
        '
        Me.MsktoDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.MsktoDate.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.MsktoDate.Location = New System.Drawing.Point(648, 130)
        Me.MsktoDate.Mask = "00-00-0000"
        Me.MsktoDate.Name = "MsktoDate"
        Me.MsktoDate.Size = New System.Drawing.Size(122, 29)
        Me.MsktoDate.TabIndex = 2
        Me.MsktoDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'mskFromDate
        '
        Me.mskFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskFromDate.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.mskFromDate.Location = New System.Drawing.Point(510, 130)
        Me.mskFromDate.Mask = "00-00-0000"
        Me.mskFromDate.Name = "mskFromDate"
        Me.mskFromDate.Size = New System.Drawing.Size(122, 29)
        Me.mskFromDate.TabIndex = 1
        Me.mskFromDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(974, 618)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 19)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "Balance :"
        '
        'txtBalAmt
        '
        Me.txtBalAmt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtBalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBalAmt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBalAmt.ForeColor = System.Drawing.Color.Red
        Me.txtBalAmt.Location = New System.Drawing.Point(1061, 615)
        Me.txtBalAmt.Name = "txtBalAmt"
        Me.txtBalAmt.ReadOnly = True
        Me.txtBalAmt.Size = New System.Drawing.Size(123, 26)
        Me.txtBalAmt.TabIndex = 35
        Me.txtBalAmt.TabStop = False
        Me.txtBalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(746, 618)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 19)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Crate Out :"
        '
        'txtcrAmt
        '
        Me.txtcrAmt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtcrAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtcrAmt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtcrAmt.ForeColor = System.Drawing.Color.Red
        Me.txtcrAmt.Location = New System.Drawing.Point(849, 615)
        Me.txtcrAmt.Name = "txtcrAmt"
        Me.txtcrAmt.ReadOnly = True
        Me.txtcrAmt.Size = New System.Drawing.Size(123, 26)
        Me.txtcrAmt.TabIndex = 34
        Me.txtcrAmt.TabStop = False
        Me.txtcrAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(532, 618)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 19)
        Me.Label7.TabIndex = 36
        Me.Label7.Text = "Crate In :"
        '
        'txtDramt
        '
        Me.txtDramt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtDramt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDramt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDramt.ForeColor = System.Drawing.Color.Red
        Me.txtDramt.Location = New System.Drawing.Point(621, 615)
        Me.txtDramt.Name = "txtDramt"
        Me.txtDramt.ReadOnly = True
        Me.txtDramt.Size = New System.Drawing.Size(123, 26)
        Me.txtDramt.TabIndex = 33
        Me.txtDramt.TabStop = False
        Me.txtDramt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.CadetBlue
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label6.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label6.Location = New System.Drawing.Point(787, 159)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(214, 27)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "Opening Balance :"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtOpBal
        '
        Me.txtOpBal.BackColor = System.Drawing.Color.GhostWhite
        Me.txtOpBal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtOpBal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtOpBal.ForeColor = System.Drawing.Color.Navy
        Me.txtOpBal.Location = New System.Drawing.Point(1001, 160)
        Me.txtOpBal.Name = "txtOpBal"
        Me.txtOpBal.ReadOnly = True
        Me.txtOpBal.Size = New System.Drawing.Size(183, 26)
        Me.txtOpBal.TabIndex = 30
        Me.txtOpBal.TabStop = False
        Me.txtOpBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.DarkTurquoise
        Me.btnShow.FlatAppearance.BorderSize = 0
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.btnShow.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnShow.Image = Global.Aadhat.My.Resources.Resources.icons8_event_accepted_24px
        Me.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShow.Location = New System.Drawing.Point(1001, 130)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(89, 30)
        Me.btnShow.TabIndex = 4
        Me.btnShow.Text = "&Show"
        Me.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.CadetBlue
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label2.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label2.Location = New System.Drawing.Point(648, 109)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 21)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "To Date"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.CadetBlue
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label1.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label1.Location = New System.Drawing.Point(511, 109)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(138, 21)
        Me.Label1.TabIndex = 28
        Me.Label1.Text = "From Date"
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.CadetBlue
        Me.Label42.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label42.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label42.Location = New System.Drawing.Point(12, 109)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(499, 21)
        Me.Label42.TabIndex = 27
        Me.Label42.Text = "Account Name"
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ForeColor = System.Drawing.Color.Black
        Me.Label41.Location = New System.Drawing.Point(366, 9)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(482, 48)
        Me.Label41.TabIndex = 20
        Me.Label41.Text = "CRATE ACCOUNT LEDGER"
        '
        'BtnPrint
        '
        Me.BtnPrint.BackColor = System.Drawing.Color.Coral
        Me.BtnPrint.FlatAppearance.BorderSize = 0
        Me.BtnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPrint.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.BtnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnPrint.Image = Global.Aadhat.My.Resources.Resources.icons8_printer_24px
        Me.BtnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnPrint.Location = New System.Drawing.Point(1090, 130)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(93, 30)
        Me.BtnPrint.TabIndex = 5
        Me.BtnPrint.TabStop = False
        Me.BtnPrint.Text = "&Print"
        Me.BtnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnPrint.UseVisualStyleBackColor = False
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
        'txtAccountID
        '
        Me.txtAccountID.BackColor = System.Drawing.Color.AliceBlue
        Me.txtAccountID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccountID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAccountID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountID.ForeColor = System.Drawing.Color.Teal
        Me.txtAccountID.Location = New System.Drawing.Point(12, 9)
        Me.txtAccountID.Name = "txtAccountID"
        Me.txtAccountID.Size = New System.Drawing.Size(72, 26)
        Me.txtAccountID.TabIndex = 91118
        Me.txtAccountID.TabStop = False
        Me.txtAccountID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtAccountID.Visible = False
        '
        'DgAccountSearch
        '
        Me.DgAccountSearch.AllowUserToAddRows = False
        Me.DgAccountSearch.AllowUserToDeleteRows = False
        Me.DgAccountSearch.AllowUserToResizeRows = False
        Me.DgAccountSearch.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.DgAccountSearch.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DgAccountSearch.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DgAccountSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DgAccountSearch.ColumnHeadersVisible = False
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkTurquoise
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DgAccountSearch.DefaultCellStyle = DataGridViewCellStyle2
        Me.DgAccountSearch.EnableHeadersVisualStyles = False
        Me.DgAccountSearch.GridColor = System.Drawing.Color.GhostWhite
        Me.DgAccountSearch.Location = New System.Drawing.Point(12, 158)
        Me.DgAccountSearch.MultiSelect = False
        Me.DgAccountSearch.Name = "DgAccountSearch"
        Me.DgAccountSearch.ReadOnly = True
        Me.DgAccountSearch.RowHeadersVisible = False
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.DgAccountSearch.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.DgAccountSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.DgAccountSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DgAccountSearch.Size = New System.Drawing.Size(499, 202)
        Me.DgAccountSearch.TabIndex = 40124
        Me.DgAccountSearch.TabStop = False
        Me.DgAccountSearch.Visible = False
        '
        'txtAccount
        '
        Me.txtAccount.BackColor = System.Drawing.Color.GhostWhite
        Me.txtAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccount.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.txtAccount.ForeColor = System.Drawing.Color.Black
        Me.txtAccount.Location = New System.Drawing.Point(12, 130)
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.Size = New System.Drawing.Size(499, 29)
        Me.txtAccount.TabIndex = 0
        Me.txtAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CbPer
        '
        Me.CbPer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.CbPer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.CbPer.BackColor = System.Drawing.Color.GhostWhite
        Me.CbPer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbPer.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CbPer.Font = New System.Drawing.Font("Times New Roman", 11.75!)
        Me.CbPer.FormattingEnabled = True
        Me.CbPer.Items.AddRange(New Object() {"--All--"})
        Me.CbPer.Location = New System.Drawing.Point(787, 131)
        Me.CbPer.Name = "CbPer"
        Me.CbPer.Size = New System.Drawing.Size(213, 27)
        Me.CbPer.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.CadetBlue
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label3.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label3.Location = New System.Drawing.Point(786, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(397, 21)
        Me.Label3.TabIndex = 40126
        Me.Label3.Text = "Marka Name"
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dg1.ColumnHeadersHeight = 28
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Maroon
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle5
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Crimson
        Me.dg1.Location = New System.Drawing.Point(12, 185)
        Me.dg1.MultiSelect = False
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dg1.RowHeadersVisible = False
        Me.dg1.RowHeadersWidth = 45
        Me.dg1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1172, 422)
        Me.dg1.TabIndex = 91122
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(310, 9)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 91249
        Me.PictureBox4.TabStop = False
        '
        'dtp1
        '
        Me.dtp1.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.dtp1.Location = New System.Drawing.Point(539, 130)
        Me.dtp1.Name = "dtp1"
        Me.dtp1.Size = New System.Drawing.Size(110, 29)
        Me.dtp1.TabIndex = 91255
        '
        'dtp2
        '
        Me.dtp2.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.dtp2.Location = New System.Drawing.Point(662, 130)
        Me.dtp2.Name = "dtp2"
        Me.dtp2.Size = New System.Drawing.Size(124, 29)
        Me.dtp2.TabIndex = 91254
        '
        'lblCrateDetails
        '
        Me.lblCrateDetails.ForeColor = System.Drawing.Color.Black
        Me.lblCrateDetails.Location = New System.Drawing.Point(10, 631)
        Me.lblCrateDetails.Name = "lblCrateDetails"
        Me.lblCrateDetails.Size = New System.Drawing.Size(512, 17)
        Me.lblCrateDetails.TabIndex = 91274
        Me.lblCrateDetails.Text = "0"
        Me.lblCrateDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblCrateDetails.Visible = False
        '
        'lblCrate
        '
        Me.lblCrate.AutoSize = True
        Me.lblCrate.ForeColor = System.Drawing.Color.Black
        Me.lblCrate.Location = New System.Drawing.Point(10, 614)
        Me.lblCrate.Name = "lblCrate"
        Me.lblCrate.Size = New System.Drawing.Size(13, 13)
        Me.lblCrate.TabIndex = 91273
        Me.lblCrate.Text = "0"
        Me.lblCrate.Visible = False
        '
        'pnlWahtsappNo
        '
        Me.pnlWahtsappNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.pnlWahtsappNo.Controls.Add(Me.Label4)
        Me.pnlWahtsappNo.Controls.Add(Me.cbType)
        Me.pnlWahtsappNo.Controls.Add(Me.Button2)
        Me.pnlWahtsappNo.Controls.Add(Me.Label5)
        Me.pnlWahtsappNo.Controls.Add(Me.txtWhatsappNo)
        Me.pnlWahtsappNo.Location = New System.Drawing.Point(495, 186)
        Me.pnlWahtsappNo.Name = "pnlWahtsappNo"
        Me.pnlWahtsappNo.Size = New System.Drawing.Size(305, 94)
        Me.pnlWahtsappNo.TabIndex = 91275
        Me.pnlWahtsappNo.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(62, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 21)
        Me.Label4.TabIndex = 91238
        Me.Label4.Text = "Send Via :"
        '
        'cbType
        '
        Me.cbType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbType.BackColor = System.Drawing.Color.GhostWhite
        Me.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbType.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.cbType.ForeColor = System.Drawing.Color.Black
        Me.cbType.FormattingEnabled = True
        Me.cbType.Items.AddRange(New Object() {"Easy WhatsApp"})
        Me.cbType.Location = New System.Drawing.Point(152, 9)
        Me.cbType.Name = "cbType"
        Me.cbType.Size = New System.Drawing.Size(146, 23)
        Me.cbType.TabIndex = 91237
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.DarkTurquoise
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.GhostWhite
        Me.Button2.Image = Global.Aadhat.My.Resources.Resources.icons8_event_accepted_24px
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(184, 35)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(114, 51)
        Me.Button2.TabIndex = 91228
        Me.Button2.Text = "&Send Crates"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.CadetBlue
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label5.Location = New System.Drawing.Point(3, 35)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(185, 24)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Enter Whatsapp No. "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtWhatsappNo
        '
        Me.txtWhatsappNo.BackColor = System.Drawing.Color.GhostWhite
        Me.txtWhatsappNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtWhatsappNo.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtWhatsappNo.ForeColor = System.Drawing.Color.Black
        Me.txtWhatsappNo.Location = New System.Drawing.Point(3, 59)
        Me.txtWhatsappNo.Name = "txtWhatsappNo"
        Me.txtWhatsappNo.Size = New System.Drawing.Size(185, 26)
        Me.txtWhatsappNo.TabIndex = 3
        Me.txtWhatsappNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(191, Byte), Integer), CType(CType(165, Byte), Integer))
        Me.Button5.FlatAppearance.BorderSize = 0
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Button5.ForeColor = System.Drawing.Color.GhostWhite
        Me.Button5.Image = CType(resources.GetObject("Button5.Image"), System.Drawing.Image)
        Me.Button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button5.Location = New System.Drawing.Point(610, 159)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(177, 30)
        Me.Button5.TabIndex = 91229
        Me.Button5.Text = "Send Whatsapp"
        Me.Button5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button5.UseVisualStyleBackColor = False
        '
        'CkHideOpBal
        '
        Me.CkHideOpBal.AutoSize = True
        Me.CkHideOpBal.BackColor = System.Drawing.Color.CadetBlue
        Me.CkHideOpBal.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.CkHideOpBal.ForeColor = System.Drawing.Color.GhostWhite
        Me.CkHideOpBal.Location = New System.Drawing.Point(1001, 111)
        Me.CkHideOpBal.Name = "CkHideOpBal"
        Me.CkHideOpBal.Size = New System.Drawing.Size(169, 21)
        Me.CkHideOpBal.TabIndex = 91285
        Me.CkHideOpBal.Text = "&Hide Opening Balance"
        Me.CkHideOpBal.UseVisualStyleBackColor = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Green
        Me.lblStatus.Location = New System.Drawing.Point(926, 87)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(56, 21)
        Me.lblStatus.TabIndex = 91286
        Me.lblStatus.Text = "Status"
        Me.lblStatus.Visible = False
        '
        'CrateCrateAccountCumMarkaLedger
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.pnlWahtsappNo)
        Me.Controls.Add(Me.lblCrateDetails)
        Me.Controls.Add(Me.lblCrate)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtAccountID)
        Me.Controls.Add(Me.Label41)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtBalAmt)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtcrAmt)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtDramt)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.CkHideOpBal)
        Me.Controls.Add(Me.DgAccountSearch)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.MsktoDate)
        Me.Controls.Add(Me.mskFromDate)
        Me.Controls.Add(Me.txtOpBal)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label42)
        Me.Controls.Add(Me.BtnPrint)
        Me.Controls.Add(Me.dtp2)
        Me.Controls.Add(Me.dtp1)
        Me.Controls.Add(Me.CbPer)
        Me.Controls.Add(Me.Button5)
        Me.Name = "CrateCrateAccountCumMarkaLedger"
        Me.Text = "CrateLedger"
        CType(Me.DgAccountSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlWahtsappNo.ResumeLayout(False)
        Me.pnlWahtsappNo.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MsktoDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mskFromDate As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtBalAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtcrAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtDramt As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtOpBal As System.Windows.Forms.TextBox
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents BtnPrint As System.Windows.Forms.Button
    Friend WithEvents DgAccountSearch As System.Windows.Forms.DataGridView
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
    Friend WithEvents txtAccountID As System.Windows.Forms.TextBox
    Friend WithEvents CbPer As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents dtp1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblCrateDetails As System.Windows.Forms.Label
    Friend WithEvents lblCrate As System.Windows.Forms.Label
    Friend WithEvents pnlWahtsappNo As System.Windows.Forms.Panel
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtWhatsappNo As System.Windows.Forms.TextBox
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents CkHideOpBal As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbType As System.Windows.Forms.ComboBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
End Class
