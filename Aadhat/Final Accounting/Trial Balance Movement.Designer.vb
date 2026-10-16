<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Trial_Balance_Movement
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblDate = New System.Windows.Forms.Label()
        Me.txtEntryDate = New System.Windows.Forms.TextBox()
        Me.lblGroup = New System.Windows.Forms.Label()
        Me.cmbGroup = New System.Windows.Forms.ComboBox()
        Me.lblAccountCount = New System.Windows.Forms.Label()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlTotals = New System.Windows.Forms.Panel()
        Me.lblOpeningDr = New System.Windows.Forms.Label()
        Me.openingDrTotal = New System.Windows.Forms.TextBox()
        Me.lblOpeningCr = New System.Windows.Forms.Label()
        Me.openingCrTotal = New System.Windows.Forms.TextBox()
        Me.lblDebitTotal = New System.Windows.Forms.Label()
        Me.debitTotalBox = New System.Windows.Forms.TextBox()
        Me.lblCreditTotal = New System.Windows.Forms.Label()
        Me.creditTotalBox = New System.Windows.Forms.TextBox()
        Me.lblClosingDr = New System.Windows.Forms.Label()
        Me.closingDrTotal = New System.Windows.Forms.TextBox()
        Me.lblClosingCr = New System.Windows.Forms.Label()
        Me.closingCrTotal = New System.Windows.Forms.TextBox()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTotals.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Times New Roman", 24.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.Location = New System.Drawing.Point(287, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(523, 48)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "TRIAL BALANCE - MOVEMENT"
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.lblDate.Location = New System.Drawing.Point(18, 67)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(38, 19)
        Me.lblDate.TabIndex = 1
        Me.lblDate.Text = "Date"
        '
        'txtEntryDate
        '
        Me.txtEntryDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEntryDate.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.txtEntryDate.Location = New System.Drawing.Point(61, 64)
        Me.txtEntryDate.Name = "txtEntryDate"
        Me.txtEntryDate.Size = New System.Drawing.Size(120, 26)
        Me.txtEntryDate.TabIndex = 2
        '
        'lblGroup
        '
        Me.lblGroup.AutoSize = True
        Me.lblGroup.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.lblGroup.Location = New System.Drawing.Point(184, 68)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.Size = New System.Drawing.Size(48, 19)
        Me.lblGroup.TabIndex = 3
        Me.lblGroup.Text = "Group"
        '
        'cmbGroup
        '
        Me.cmbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbGroup.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cmbGroup.Location = New System.Drawing.Point(232, 64)
        Me.cmbGroup.Name = "cmbGroup"
        Me.cmbGroup.Size = New System.Drawing.Size(250, 27)
        Me.cmbGroup.TabIndex = 4
        '
        'lblAccountCount
        '
        Me.lblAccountCount.AutoSize = True
        Me.lblAccountCount.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblAccountCount.ForeColor = System.Drawing.Color.Black
        Me.lblAccountCount.Location = New System.Drawing.Point(675, 68)
        Me.lblAccountCount.Name = "lblAccountCount"
        Me.lblAccountCount.Size = New System.Drawing.Size(96, 19)
        Me.lblAccountCount.TabIndex = 8
        Me.lblAccountCount.Text = "Accounts: 0"
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.DarkTurquoise
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnShow.ForeColor = System.Drawing.Color.White
        Me.btnShow.Location = New System.Drawing.Point(485, 64)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(95, 27)
        Me.btnShow.TabIndex = 5
        Me.btnShow.Text = "&Show"
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.MediumAquamarine
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnPrint.Location = New System.Drawing.Point(580, 64)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(88, 27)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.Location = New System.Drawing.Point(1111, 10)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(62, 35)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dg1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dg1.ColumnHeadersHeight = 36
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dg1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DarkTurquoise
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle4
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Crimson
        Me.dg1.Location = New System.Drawing.Point(16, 108)
        Me.dg1.MultiSelect = False
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersVisible = False
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1157, 460)
        Me.dg1.TabIndex = 7
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn1.FillWeight = 220.0!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Name"
        Me.DataGridViewTextBoxColumn1.MinimumWidth = 200
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn2.FillWeight = 115.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Opening Balance"
        Me.DataGridViewTextBoxColumn2.MinimumWidth = 155
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn3.HeaderText = "Debit"
        Me.DataGridViewTextBoxColumn3.MinimumWidth = 135
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn4.HeaderText = "Credit"
        Me.DataGridViewTextBoxColumn4.MinimumWidth = 135
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn5.FillWeight = 120.0!
        Me.DataGridViewTextBoxColumn5.HeaderText = "Closing Balance"
        Me.DataGridViewTextBoxColumn5.MinimumWidth = 175
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        '
        'pnlTotals
        '
        Me.pnlTotals.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTotals.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.pnlTotals.Controls.Add(Me.lblOpeningDr)
        Me.pnlTotals.Controls.Add(Me.openingDrTotal)
        Me.pnlTotals.Controls.Add(Me.lblOpeningCr)
        Me.pnlTotals.Controls.Add(Me.openingCrTotal)
        Me.pnlTotals.Controls.Add(Me.lblDebitTotal)
        Me.pnlTotals.Controls.Add(Me.debitTotalBox)
        Me.pnlTotals.Controls.Add(Me.lblCreditTotal)
        Me.pnlTotals.Controls.Add(Me.creditTotalBox)
        Me.pnlTotals.Controls.Add(Me.lblClosingDr)
        Me.pnlTotals.Controls.Add(Me.closingDrTotal)
        Me.pnlTotals.Controls.Add(Me.lblClosingCr)
        Me.pnlTotals.Controls.Add(Me.closingCrTotal)
        Me.pnlTotals.ForeColor = System.Drawing.Color.Black
        Me.pnlTotals.Location = New System.Drawing.Point(16, 580)
        Me.pnlTotals.Name = "pnlTotals"
        Me.pnlTotals.Size = New System.Drawing.Size(1160, 52)
        Me.pnlTotals.TabIndex = 8
        '
        'lblOpeningDr
        '
        Me.lblOpeningDr.AutoSize = True
        Me.lblOpeningDr.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblOpeningDr.ForeColor = System.Drawing.Color.Black
        Me.lblOpeningDr.Location = New System.Drawing.Point(14, 2)
        Me.lblOpeningDr.Name = "lblOpeningDr"
        Me.lblOpeningDr.Size = New System.Drawing.Size(80, 17)
        Me.lblOpeningDr.TabIndex = 0
        Me.lblOpeningDr.Text = "Opening Dr"
        '
        'openingDrTotal
        '
        Me.openingDrTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.openingDrTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.openingDrTotal.Location = New System.Drawing.Point(14, 22)
        Me.openingDrTotal.Name = "openingDrTotal"
        Me.openingDrTotal.ReadOnly = True
        Me.openingDrTotal.Size = New System.Drawing.Size(145, 26)
        Me.openingDrTotal.TabIndex = 1
        '
        'lblOpeningCr
        '
        Me.lblOpeningCr.AutoSize = True
        Me.lblOpeningCr.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblOpeningCr.ForeColor = System.Drawing.Color.Black
        Me.lblOpeningCr.Location = New System.Drawing.Point(169, 2)
        Me.lblOpeningCr.Name = "lblOpeningCr"
        Me.lblOpeningCr.Size = New System.Drawing.Size(80, 17)
        Me.lblOpeningCr.TabIndex = 2
        Me.lblOpeningCr.Text = "Opening Cr"
        '
        'openingCrTotal
        '
        Me.openingCrTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.openingCrTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.openingCrTotal.Location = New System.Drawing.Point(169, 22)
        Me.openingCrTotal.Name = "openingCrTotal"
        Me.openingCrTotal.ReadOnly = True
        Me.openingCrTotal.Size = New System.Drawing.Size(145, 26)
        Me.openingCrTotal.TabIndex = 3
        '
        'lblDebitTotal
        '
        Me.lblDebitTotal.AutoSize = True
        Me.lblDebitTotal.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblDebitTotal.ForeColor = System.Drawing.Color.Black
        Me.lblDebitTotal.Location = New System.Drawing.Point(323, 2)
        Me.lblDebitTotal.Name = "lblDebitTotal"
        Me.lblDebitTotal.Size = New System.Drawing.Size(75, 17)
        Me.lblDebitTotal.TabIndex = 4
        Me.lblDebitTotal.Text = "Debit Total"
        '
        'debitTotalBox
        '
        Me.debitTotalBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.debitTotalBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.debitTotalBox.Location = New System.Drawing.Point(324, 22)
        Me.debitTotalBox.Name = "debitTotalBox"
        Me.debitTotalBox.ReadOnly = True
        Me.debitTotalBox.Size = New System.Drawing.Size(145, 26)
        Me.debitTotalBox.TabIndex = 5
        '
        'lblCreditTotal
        '
        Me.lblCreditTotal.AutoSize = True
        Me.lblCreditTotal.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblCreditTotal.ForeColor = System.Drawing.Color.Black
        Me.lblCreditTotal.Location = New System.Drawing.Point(477, 2)
        Me.lblCreditTotal.Name = "lblCreditTotal"
        Me.lblCreditTotal.Size = New System.Drawing.Size(81, 17)
        Me.lblCreditTotal.TabIndex = 6
        Me.lblCreditTotal.Text = "Credit Total"
        '
        'creditTotalBox
        '
        Me.creditTotalBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.creditTotalBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.creditTotalBox.Location = New System.Drawing.Point(479, 22)
        Me.creditTotalBox.Name = "creditTotalBox"
        Me.creditTotalBox.ReadOnly = True
        Me.creditTotalBox.Size = New System.Drawing.Size(145, 26)
        Me.creditTotalBox.TabIndex = 7
        '
        'lblClosingDr
        '
        Me.lblClosingDr.AutoSize = True
        Me.lblClosingDr.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblClosingDr.ForeColor = System.Drawing.Color.Black
        Me.lblClosingDr.Location = New System.Drawing.Point(632, 2)
        Me.lblClosingDr.Name = "lblClosingDr"
        Me.lblClosingDr.Size = New System.Drawing.Size(74, 17)
        Me.lblClosingDr.TabIndex = 8
        Me.lblClosingDr.Text = "Closing Dr"
        '
        'closingDrTotal
        '
        Me.closingDrTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.closingDrTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.closingDrTotal.Location = New System.Drawing.Point(634, 22)
        Me.closingDrTotal.Name = "closingDrTotal"
        Me.closingDrTotal.ReadOnly = True
        Me.closingDrTotal.Size = New System.Drawing.Size(145, 26)
        Me.closingDrTotal.TabIndex = 9
        '
        'lblClosingCr
        '
        Me.lblClosingCr.AutoSize = True
        Me.lblClosingCr.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblClosingCr.ForeColor = System.Drawing.Color.Black
        Me.lblClosingCr.Location = New System.Drawing.Point(787, 2)
        Me.lblClosingCr.Name = "lblClosingCr"
        Me.lblClosingCr.Size = New System.Drawing.Size(74, 17)
        Me.lblClosingCr.TabIndex = 10
        Me.lblClosingCr.Text = "Closing Cr"
        '
        'closingCrTotal
        '
        Me.closingCrTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.closingCrTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.closingCrTotal.Location = New System.Drawing.Point(789, 22)
        Me.closingCrTotal.Name = "closingCrTotal"
        Me.closingCrTotal.ReadOnly = True
        Me.closingCrTotal.Size = New System.Drawing.Size(145, 26)
        Me.closingCrTotal.TabIndex = 11
        '
        'Trial_Balance_Movement
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1188, 644)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.pnlTotals)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.lblAccountCount)
        Me.Controls.Add(Me.cmbGroup)
        Me.Controls.Add(Me.lblGroup)
        Me.Controls.Add(Me.txtEntryDate)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "Trial_Balance_Movement"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Trial Balance - Movement"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTotals.ResumeLayout(False)
        Me.pnlTotals.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents txtEntryDate As System.Windows.Forms.TextBox
    Friend WithEvents lblGroup As System.Windows.Forms.Label
    Friend WithEvents cmbGroup As System.Windows.Forms.ComboBox
    Friend WithEvents lblAccountCount As System.Windows.Forms.Label
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pnlTotals As System.Windows.Forms.Panel
    Friend WithEvents lblOpeningDr As System.Windows.Forms.Label
    Friend WithEvents openingDrTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblOpeningCr As System.Windows.Forms.Label
    Friend WithEvents openingCrTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblDebitTotal As System.Windows.Forms.Label
    Friend WithEvents debitTotalBox As System.Windows.Forms.TextBox
    Friend WithEvents lblCreditTotal As System.Windows.Forms.Label
    Friend WithEvents creditTotalBox As System.Windows.Forms.TextBox
    Friend WithEvents lblClosingDr As System.Windows.Forms.Label
    Friend WithEvents closingDrTotal As System.Windows.Forms.TextBox
    Friend WithEvents lblClosingCr As System.Windows.Forms.Label
    Friend WithEvents closingCrTotal As System.Windows.Forms.TextBox
End Class
