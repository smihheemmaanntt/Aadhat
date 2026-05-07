<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Account_cum_Marka_Outstanding
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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Account_cum_Marka_Outstanding))
        Me.txtAccountID = New System.Windows.Forms.TextBox()
        Me.DgAccountSearch = New System.Windows.Forms.DataGridView()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.txtAccount = New System.Windows.Forms.TextBox()
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
        Me.BtnPrint = New System.Windows.Forms.Button()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblCrateDetails = New System.Windows.Forms.Label()
        Me.lblCrate = New System.Windows.Forms.Label()
        Me.txttoDate = New System.Windows.Forms.TextBox()
        Me.txtFromDate = New System.Windows.Forms.TextBox()
        Me.dtp2 = New System.Windows.Forms.DateTimePicker()
        Me.dtp1 = New System.Windows.Forms.DateTimePicker()
        CType(Me.DgAccountSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtAccountID
        '
        Me.txtAccountID.BackColor = System.Drawing.Color.AliceBlue
        Me.txtAccountID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAccountID.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccountID.ForeColor = System.Drawing.Color.Teal
        Me.txtAccountID.Location = New System.Drawing.Point(6, 15)
        Me.txtAccountID.Name = "txtAccountID"
        Me.txtAccountID.Size = New System.Drawing.Size(48, 26)
        Me.txtAccountID.TabIndex = 91268
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
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.Teal
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
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.DarkSlateGray
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DgAccountSearch.DefaultCellStyle = DataGridViewCellStyle8
        Me.DgAccountSearch.GridColor = System.Drawing.Color.Gray
        Me.DgAccountSearch.Location = New System.Drawing.Point(158, 150)
        Me.DgAccountSearch.MultiSelect = False
        Me.DgAccountSearch.Name = "DgAccountSearch"
        Me.DgAccountSearch.ReadOnly = True
        Me.DgAccountSearch.RowHeadersVisible = False
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black
        Me.DgAccountSearch.RowsDefaultCellStyle = DataGridViewCellStyle9
        Me.DgAccountSearch.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.DgAccountSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DgAccountSearch.Size = New System.Drawing.Size(440, 202)
        Me.DgAccountSearch.TabIndex = 91267
        Me.DgAccountSearch.TabStop = False
        Me.DgAccountSearch.Visible = False
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ForeColor = System.Drawing.Color.Black
        Me.Label41.Location = New System.Drawing.Point(374, 15)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(482, 48)
        Me.Label41.TabIndex = 91255
        Me.Label41.Text = "CRATE ACCOUNT LEDGER"
        '
        'txtAccount
        '
        Me.txtAccount.BackColor = System.Drawing.Color.GhostWhite
        Me.txtAccount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAccount.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.txtAccount.ForeColor = System.Drawing.Color.Teal
        Me.txtAccount.Location = New System.Drawing.Point(158, 122)
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.Size = New System.Drawing.Size(440, 29)
        Me.txtAccount.TabIndex = 0
        Me.txtAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(967, 600)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 19)
        Me.Label9.TabIndex = 91266
        Me.Label9.Text = "Balance :"
        '
        'txtBalAmt
        '
        Me.txtBalAmt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtBalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBalAmt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtBalAmt.ForeColor = System.Drawing.Color.Navy
        Me.txtBalAmt.Location = New System.Drawing.Point(1055, 597)
        Me.txtBalAmt.Name = "txtBalAmt"
        Me.txtBalAmt.ReadOnly = True
        Me.txtBalAmt.Size = New System.Drawing.Size(123, 26)
        Me.txtBalAmt.TabIndex = 91263
        Me.txtBalAmt.TabStop = False
        Me.txtBalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(733, 601)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 19)
        Me.Label8.TabIndex = 91265
        Me.Label8.Text = "Crate Out :"
        '
        'txtcrAmt
        '
        Me.txtcrAmt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtcrAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtcrAmt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtcrAmt.ForeColor = System.Drawing.Color.Navy
        Me.txtcrAmt.Location = New System.Drawing.Point(838, 597)
        Me.txtcrAmt.Name = "txtcrAmt"
        Me.txtcrAmt.ReadOnly = True
        Me.txtcrAmt.Size = New System.Drawing.Size(123, 26)
        Me.txtcrAmt.TabIndex = 91262
        Me.txtcrAmt.TabStop = False
        Me.txtcrAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(515, 600)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 19)
        Me.Label7.TabIndex = 91264
        Me.Label7.Text = "Crate In :"
        '
        'txtDramt
        '
        Me.txtDramt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtDramt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDramt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtDramt.ForeColor = System.Drawing.Color.Navy
        Me.txtDramt.Location = New System.Drawing.Point(604, 597)
        Me.txtDramt.Name = "txtDramt"
        Me.txtDramt.ReadOnly = True
        Me.txtDramt.Size = New System.Drawing.Size(123, 26)
        Me.txtDramt.TabIndex = 91261
        Me.txtDramt.TabStop = False
        Me.txtDramt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(904, 154)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(119, 19)
        Me.Label6.TabIndex = 91260
        Me.Label6.Text = "Opening Balance :"
        '
        'txtOpBal
        '
        Me.txtOpBal.BackColor = System.Drawing.Color.GhostWhite
        Me.txtOpBal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtOpBal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtOpBal.ForeColor = System.Drawing.Color.Navy
        Me.txtOpBal.Location = New System.Drawing.Point(1026, 151)
        Me.txtOpBal.Name = "txtOpBal"
        Me.txtOpBal.ReadOnly = True
        Me.txtOpBal.Size = New System.Drawing.Size(152, 26)
        Me.txtOpBal.TabIndex = 91259
        Me.txtOpBal.TabStop = False
        Me.txtOpBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.Coral
        Me.btnShow.FlatAppearance.BorderSize = 0
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.btnShow.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnShow.Location = New System.Drawing.Point(1026, 122)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(76, 29)
        Me.btnShow.TabIndex = 3
        Me.btnShow.Text = "&Show"
        Me.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label2.Location = New System.Drawing.Point(825, 122)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(81, 28)
        Me.Label2.TabIndex = 91258
        Me.Label2.Text = "To Date :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label1.Location = New System.Drawing.Point(598, 122)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 29)
        Me.Label1.TabIndex = 91257
        Me.Label1.Text = "From Date :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label42.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label42.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label42.Location = New System.Drawing.Point(6, 122)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(153, 28)
        Me.Label42.TabIndex = 91256
        Me.Label42.Text = "Account Name :"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnPrint
        '
        Me.BtnPrint.BackColor = System.Drawing.Color.Tan
        Me.BtnPrint.FlatAppearance.BorderSize = 0
        Me.BtnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPrint.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.BtnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnPrint.Location = New System.Drawing.Point(1102, 122)
        Me.BtnPrint.Name = "BtnPrint"
        Me.BtnPrint.Size = New System.Drawing.Size(76, 29)
        Me.BtnPrint.TabIndex = 4
        Me.BtnPrint.TabStop = False
        Me.BtnPrint.Text = "&Print"
        Me.BtnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnPrint.UseVisualStyleBackColor = False
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Times New Roman", 10.0!)
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle10
        Me.dg1.ColumnHeadersHeight = 28
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.CadetBlue
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle11
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Crimson
        Me.dg1.Location = New System.Drawing.Point(6, 176)
        Me.dg1.MultiSelect = False
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dg1.RowHeadersVisible = False
        Me.dg1.RowHeadersWidth = 45
        Me.dg1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1172, 422)
        Me.dg1.TabIndex = 91269
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(318, 18)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox4.TabIndex = 91270
        Me.PictureBox4.TabStop = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(1137, 6)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 47)
        Me.btnClose.TabIndex = 91254
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblCrateDetails
        '
        Me.lblCrateDetails.ForeColor = System.Drawing.Color.Black
        Me.lblCrateDetails.Location = New System.Drawing.Point(7, 601)
        Me.lblCrateDetails.Name = "lblCrateDetails"
        Me.lblCrateDetails.Size = New System.Drawing.Size(512, 17)
        Me.lblCrateDetails.TabIndex = 91272
        Me.lblCrateDetails.Text = "0"
        Me.lblCrateDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblCrateDetails.Visible = False
        '
        'lblCrate
        '
        Me.lblCrate.AutoSize = True
        Me.lblCrate.ForeColor = System.Drawing.Color.Black
        Me.lblCrate.Location = New System.Drawing.Point(8, 616)
        Me.lblCrate.Name = "lblCrate"
        Me.lblCrate.Size = New System.Drawing.Size(13, 13)
        Me.lblCrate.TabIndex = 91271
        Me.lblCrate.Text = "0"
        Me.lblCrate.Visible = False
        '
        'txttoDate
        '
        Me.txttoDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txttoDate.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.txttoDate.Location = New System.Drawing.Point(900, 122)
        Me.txttoDate.Name = "txttoDate"
        Me.txttoDate.Size = New System.Drawing.Size(114, 29)
        Me.txttoDate.TabIndex = 2
        '
        'txtFromDate
        '
        Me.txtFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFromDate.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.txtFromDate.Location = New System.Drawing.Point(703, 122)
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.Size = New System.Drawing.Size(113, 29)
        Me.txtFromDate.TabIndex = 1
        '
        'dtp2
        '
        Me.dtp2.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.dtp2.Location = New System.Drawing.Point(901, 122)
        Me.dtp2.Name = "dtp2"
        Me.dtp2.Size = New System.Drawing.Size(128, 29)
        Me.dtp2.TabIndex = 91290
        '
        'dtp1
        '
        Me.dtp1.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.dtp1.Location = New System.Drawing.Point(708, 122)
        Me.dtp1.Name = "dtp1"
        Me.dtp1.Size = New System.Drawing.Size(122, 29)
        Me.dtp1.TabIndex = 91288
        '
        'Account_cum_Marka_Outstanding
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.txttoDate)
        Me.Controls.Add(Me.txtFromDate)
        Me.Controls.Add(Me.dtp2)
        Me.Controls.Add(Me.dtp1)
        Me.Controls.Add(Me.lblCrateDetails)
        Me.Controls.Add(Me.lblCrate)
        Me.Controls.Add(Me.PictureBox4)
        Me.Controls.Add(Me.txtAccountID)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.DgAccountSearch)
        Me.Controls.Add(Me.Label41)
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtBalAmt)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtcrAmt)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtDramt)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtOpBal)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label42)
        Me.Controls.Add(Me.BtnPrint)
        Me.Name = "Account_cum_Marka_Outstanding"
        Me.Text = "Account_cum_Marka_Outstanding"
        CType(Me.DgAccountSearch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents txtAccountID As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents DgAccountSearch As System.Windows.Forms.DataGridView
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents txtAccount As System.Windows.Forms.TextBox
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
    Friend WithEvents BtnPrint As System.Windows.Forms.Button
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents lblCrateDetails As System.Windows.Forms.Label
    Friend WithEvents lblCrate As System.Windows.Forms.Label
    Friend WithEvents txttoDate As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
    Friend WithEvents dtp2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp1 As System.Windows.Forms.DateTimePicker
End Class
