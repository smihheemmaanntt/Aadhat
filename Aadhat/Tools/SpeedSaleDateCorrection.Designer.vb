<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpeedSaleDateCorrection
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SpeedSaleDateCorrection))
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.lblHint = New System.Windows.Forms.Label()
        Me.txtSure = New System.Windows.Forms.TextBox()
        Me.lblSure = New System.Windows.Forms.Label()
        Me.btnCorrectDate = New System.Windows.Forms.Button()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.txtCorrectDate = New System.Windows.Forms.TextBox()
        Me.lblCorrectDate = New System.Windows.Forms.Label()
        Me.txtEntryDate = New System.Windows.Forms.TextBox()
        Me.lblEntryDate = New System.Windows.Forms.Label()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.lblSummary = New System.Windows.Forms.Label()
        Me.dgEntries = New System.Windows.Forms.DataGridView()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.pnlTop.SuspendLayout()
        CType(Me.dgEntries, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTop.Controls.Add(Me.btnClose)
        Me.pnlTop.Controls.Add(Me.lblHint)
        Me.pnlTop.Controls.Add(Me.txtSure)
        Me.pnlTop.Controls.Add(Me.lblSure)
        Me.pnlTop.Controls.Add(Me.btnCorrectDate)
        Me.pnlTop.Controls.Add(Me.btnShow)
        Me.pnlTop.Controls.Add(Me.txtCorrectDate)
        Me.pnlTop.Controls.Add(Me.lblCorrectDate)
        Me.pnlTop.Controls.Add(Me.txtEntryDate)
        Me.pnlTop.Controls.Add(Me.lblEntryDate)
        Me.pnlTop.Location = New System.Drawing.Point(12, 12)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1162, 105)
        Me.pnlTop.TabIndex = 0
        '
        'lblHint
        '
        Me.lblHint.AutoSize = True
        Me.lblHint.Font = New System.Drawing.Font("Times New Roman", 10.5!, System.Drawing.FontStyle.Bold)
        Me.lblHint.ForeColor = System.Drawing.Color.Maroon
        Me.lblHint.Location = New System.Drawing.Point(18, 66)
        Me.lblHint.Name = "lblHint"
        Me.lblHint.Size = New System.Drawing.Size(686, 17)
        Me.lblHint.TabIndex = 8
        Me.lblHint.Text = "Show only those Speed Sale entries where EntryTime Date matches but Entry Date wa" & _
    "s saved on another date."
        '
        'txtSure
        '
        Me.txtSure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSure.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtSure.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.txtSure.Location = New System.Drawing.Point(440, 20)
        Me.txtSure.Name = "txtSure"
        Me.txtSure.Size = New System.Drawing.Size(101, 26)
        Me.txtSure.TabIndex = 5
        Me.txtSure.Visible = False
        '
        'lblSure
        '
        Me.lblSure.AutoSize = True
        Me.lblSure.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblSure.ForeColor = System.Drawing.Color.Maroon
        Me.lblSure.Location = New System.Drawing.Point(387, 24)
        Me.lblSure.Name = "lblSure"
        Me.lblSure.Size = New System.Drawing.Size(51, 19)
        Me.lblSure.TabIndex = 4
        Me.lblSure.Text = "SURE"
        Me.lblSure.Visible = False
        '
        'btnCorrectDate
        '
        Me.btnCorrectDate.BackColor = System.Drawing.Color.Teal
        Me.btnCorrectDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCorrectDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnCorrectDate.ForeColor = System.Drawing.Color.White
        Me.btnCorrectDate.Location = New System.Drawing.Point(873, 16)
        Me.btnCorrectDate.Name = "btnCorrectDate"
        Me.btnCorrectDate.Size = New System.Drawing.Size(218, 34)
        Me.btnCorrectDate.TabIndex = 6
        Me.btnCorrectDate.Text = "Correct Selected Speed Sale"
        Me.btnCorrectDate.UseVisualStyleBackColor = False
        Me.btnCorrectDate.Visible = False
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.SteelBlue
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnShow.ForeColor = System.Drawing.Color.White
        Me.btnShow.Location = New System.Drawing.Point(263, 16)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(118, 34)
        Me.btnShow.TabIndex = 3
        Me.btnShow.Text = "Show Entries"
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'txtCorrectDate
        '
        Me.txtCorrectDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCorrectDate.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.txtCorrectDate.Location = New System.Drawing.Point(714, 20)
        Me.txtCorrectDate.Name = "txtCorrectDate"
        Me.txtCorrectDate.Size = New System.Drawing.Size(135, 26)
        Me.txtCorrectDate.TabIndex = 2
        Me.txtCorrectDate.Visible = False
        '
        'lblCorrectDate
        '
        Me.lblCorrectDate.AutoSize = True
        Me.lblCorrectDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblCorrectDate.Location = New System.Drawing.Point(550, 24)
        Me.lblCorrectDate.Name = "lblCorrectDate"
        Me.lblCorrectDate.Size = New System.Drawing.Size(138, 19)
        Me.lblCorrectDate.TabIndex = 2
        Me.lblCorrectDate.Text = "Correct Entry Date"
        Me.lblCorrectDate.Visible = False
        '
        'txtEntryDate
        '
        Me.txtEntryDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEntryDate.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.txtEntryDate.Location = New System.Drawing.Point(140, 20)
        Me.txtEntryDate.Name = "txtEntryDate"
        Me.txtEntryDate.Size = New System.Drawing.Size(120, 26)
        Me.txtEntryDate.TabIndex = 1
        '
        'lblEntryDate
        '
        Me.lblEntryDate.AutoSize = True
        Me.lblEntryDate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblEntryDate.Location = New System.Drawing.Point(17, 22)
        Me.lblEntryDate.Name = "lblEntryDate"
        Me.lblEntryDate.Size = New System.Drawing.Size(117, 19)
        Me.lblEntryDate.TabIndex = 0
        Me.lblEntryDate.Text = "EntryTime Date"
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chkSelectAll.Location = New System.Drawing.Point(20, 129)
        Me.chkSelectAll.Name = "chkSelectAll"
        Me.chkSelectAll.Size = New System.Drawing.Size(89, 21)
        Me.chkSelectAll.TabIndex = 1
        Me.chkSelectAll.Text = "Select All"
        Me.chkSelectAll.UseVisualStyleBackColor = True
        '
        'lblSummary
        '
        Me.lblSummary.AutoSize = True
        Me.lblSummary.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Bold)
        Me.lblSummary.ForeColor = System.Drawing.Color.Navy
        Me.lblSummary.Location = New System.Drawing.Point(126, 131)
        Me.lblSummary.Name = "lblSummary"
        Me.lblSummary.Size = New System.Drawing.Size(187, 17)
        Me.lblSummary.TabIndex = 2
        Me.lblSummary.Text = "No mismatch entries found."
        '
        'dgEntries
        '
        Me.dgEntries.AllowUserToAddRows = False
        Me.dgEntries.AllowUserToDeleteRows = False
        Me.dgEntries.BackgroundColor = System.Drawing.Color.White
        Me.dgEntries.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgEntries.Location = New System.Drawing.Point(12, 157)
        Me.dgEntries.MultiSelect = False
        Me.dgEntries.Name = "dgEntries"
        Me.dgEntries.RowHeadersVisible = False
        Me.dgEntries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgEntries.Size = New System.Drawing.Size(1162, 469)
        Me.dgEntries.TabIndex = 3
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(1104, -1)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 47)
        Me.btnClose.TabIndex = 91115
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'SpeedSaleDateCorrection
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.dgEntries)
        Me.Controls.Add(Me.lblSummary)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.pnlTop)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SpeedSaleDateCorrection"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Speed Sale Date Correction"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.dgEntries, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents txtEntryDate As System.Windows.Forms.TextBox
    Friend WithEvents lblEntryDate As System.Windows.Forms.Label
    Friend WithEvents txtCorrectDate As System.Windows.Forms.TextBox
    Friend WithEvents lblCorrectDate As System.Windows.Forms.Label
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents txtSure As System.Windows.Forms.TextBox
    Friend WithEvents lblSure As System.Windows.Forms.Label
    Friend WithEvents btnCorrectDate As System.Windows.Forms.Button
    Friend WithEvents lblHint As System.Windows.Forms.Label
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents lblSummary As System.Windows.Forms.Label
    Friend WithEvents dgEntries As System.Windows.Forms.DataGridView
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
