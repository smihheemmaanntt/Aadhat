<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OtherNameTranslator
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
        Me.pnlTop = New System.Windows.Forms.Panel()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblLanguage = New System.Windows.Forms.Label()
        Me.cbLanguage = New System.Windows.Forms.ComboBox()
        Me.lblLanguageCode = New System.Windows.Forms.Label()
        Me.txtLanguageCode = New System.Windows.Forms.TextBox()
        Me.lblRecordType = New System.Windows.Forms.Label()
        Me.cbRecordType = New System.Windows.Forms.ComboBox()
        Me.chkOnlyBlank = New System.Windows.Forms.CheckBox()
        Me.chkNameStyle = New System.Windows.Forms.CheckBox()
        Me.chkUpdatePrimaryAccountName = New System.Windows.Forms.CheckBox()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.btnTranslateSelected = New System.Windows.Forms.Button()
        Me.btnUpdateTypedNames = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.chkSelectAll = New System.Windows.Forms.CheckBox()
        Me.lblSummary = New System.Windows.Forms.Label()
        Me.pbProgress = New System.Windows.Forms.ProgressBar()
        Me.dgNames = New System.Windows.Forms.DataGridView()
        Me.colSelect = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colRecordType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSourceName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colOldOtherName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colNewOtherName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colPrimary = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlTop.SuspendLayout()
        CType(Me.dgNames, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTop.Controls.Add(Me.btnCancel)
        Me.pnlTop.Controls.Add(Me.btnUpdateTypedNames)
        Me.pnlTop.Controls.Add(Me.btnTranslateSelected)
        Me.pnlTop.Controls.Add(Me.btnLoad)
        Me.pnlTop.Controls.Add(Me.chkUpdatePrimaryAccountName)
        Me.pnlTop.Controls.Add(Me.chkNameStyle)
        Me.pnlTop.Controls.Add(Me.chkOnlyBlank)
        Me.pnlTop.Controls.Add(Me.cbRecordType)
        Me.pnlTop.Controls.Add(Me.lblRecordType)
        Me.pnlTop.Controls.Add(Me.txtLanguageCode)
        Me.pnlTop.Controls.Add(Me.lblLanguageCode)
        Me.pnlTop.Controls.Add(Me.cbLanguage)
        Me.pnlTop.Controls.Add(Me.lblLanguage)
        Me.pnlTop.Controls.Add(Me.lblTitle)
        Me.pnlTop.Controls.Add(Me.btnClose)
        Me.pnlTop.Location = New System.Drawing.Point(12, 12)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(1162, 116)
        Me.pnlTop.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 18.0!, System.Drawing.FontStyle.Bold)
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Location = New System.Drawing.Point(1110, 1)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(47, 42)
        Me.btnClose.TabIndex = 91115
        Me.btnClose.TabStop = False
        Me.btnClose.Text = "X"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Times New Roman", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.Maroon
        Me.lblTitle.Location = New System.Drawing.Point(16, 12)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(519, 25)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Accounts and Items OtherName Google Translator"
        '
        'lblLanguage
        '
        Me.lblLanguage.AutoSize = True
        Me.lblLanguage.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblLanguage.Location = New System.Drawing.Point(18, 56)
        Me.lblLanguage.Name = "lblLanguage"
        Me.lblLanguage.Size = New System.Drawing.Size(126, 19)
        Me.lblLanguage.TabIndex = 1
        Me.lblLanguage.Text = "Target Language"
        '
        'cbLanguage
        '
        Me.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLanguage.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cbLanguage.FormattingEnabled = True
        Me.cbLanguage.Location = New System.Drawing.Point(151, 52)
        Me.cbLanguage.Name = "cbLanguage"
        Me.cbLanguage.Size = New System.Drawing.Size(160, 27)
        Me.cbLanguage.TabIndex = 1
        '
        'lblLanguageCode
        '
        Me.lblLanguageCode.AutoSize = True
        Me.lblLanguageCode.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblLanguageCode.Location = New System.Drawing.Point(326, 56)
        Me.lblLanguageCode.Name = "lblLanguageCode"
        Me.lblLanguageCode.Size = New System.Drawing.Size(93, 19)
        Me.lblLanguageCode.TabIndex = 3
        Me.lblLanguageCode.Text = "Google Code"
        '
        'txtLanguageCode
        '
        Me.txtLanguageCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLanguageCode.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.txtLanguageCode.Location = New System.Drawing.Point(423, 53)
        Me.txtLanguageCode.Name = "txtLanguageCode"
        Me.txtLanguageCode.Size = New System.Drawing.Size(55, 26)
        Me.txtLanguageCode.TabIndex = 2
        '
        'lblRecordType
        '
        Me.lblRecordType.AutoSize = True
        Me.lblRecordType.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblRecordType.Location = New System.Drawing.Point(492, 56)
        Me.lblRecordType.Name = "lblRecordType"
        Me.lblRecordType.Size = New System.Drawing.Size(89, 19)
        Me.lblRecordType.TabIndex = 4
        Me.lblRecordType.Text = "Record Type"
        '
        'cbRecordType
        '
        Me.cbRecordType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbRecordType.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cbRecordType.FormattingEnabled = True
        Me.cbRecordType.Items.AddRange(New Object() {"Both", "Accounts", "Items"})
        Me.cbRecordType.Location = New System.Drawing.Point(588, 52)
        Me.cbRecordType.Name = "cbRecordType"
        Me.cbRecordType.Size = New System.Drawing.Size(100, 27)
        Me.cbRecordType.TabIndex = 3
        '
        'chkOnlyBlank
        '
        Me.chkOnlyBlank.AutoSize = True
        Me.chkOnlyBlank.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chkOnlyBlank.Location = New System.Drawing.Point(700, 55)
        Me.chkOnlyBlank.Name = "chkOnlyBlank"
        Me.chkOnlyBlank.Size = New System.Drawing.Size(158, 21)
        Me.chkOnlyBlank.TabIndex = 4
        Me.chkOnlyBlank.Text = "Only Blank OtherName"
        Me.chkOnlyBlank.UseVisualStyleBackColor = True
        '
        'chkNameStyle
        '
        Me.chkNameStyle.AutoSize = True
        Me.chkNameStyle.Checked = True
        Me.chkNameStyle.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkNameStyle.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chkNameStyle.Location = New System.Drawing.Point(864, 55)
        Me.chkNameStyle.Name = "chkNameStyle"
        Me.chkNameStyle.Size = New System.Drawing.Size(95, 21)
        Me.chkNameStyle.TabIndex = 5
        Me.chkNameStyle.Text = "Name Style"
        Me.chkNameStyle.UseVisualStyleBackColor = True
        '
        'chkUpdatePrimaryAccountName
        '
        Me.chkUpdatePrimaryAccountName.AutoSize = True
        Me.chkUpdatePrimaryAccountName.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chkUpdatePrimaryAccountName.Location = New System.Drawing.Point(966, 55)
        Me.chkUpdatePrimaryAccountName.Name = "chkUpdatePrimaryAccountName"
        Me.chkUpdatePrimaryAccountName.Size = New System.Drawing.Size(168, 21)
        Me.chkUpdatePrimaryAccountName.TabIndex = 6
        Me.chkUpdatePrimaryAccountName.Text = "Include Primary Accounts"
        Me.chkUpdatePrimaryAccountName.UseVisualStyleBackColor = True
        '
        'btnLoad
        '
        Me.btnLoad.BackColor = System.Drawing.Color.SteelBlue
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnLoad.ForeColor = System.Drawing.Color.White
        Me.btnLoad.Location = New System.Drawing.Point(21, 82)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(128, 29)
        Me.btnLoad.TabIndex = 8
        Me.btnLoad.Text = "Load Names"
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'btnTranslateSelected
        '
        Me.btnTranslateSelected.BackColor = System.Drawing.Color.Teal
        Me.btnTranslateSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTranslateSelected.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnTranslateSelected.ForeColor = System.Drawing.Color.White
        Me.btnTranslateSelected.Location = New System.Drawing.Point(161, 82)
        Me.btnTranslateSelected.Name = "btnTranslateSelected"
        Me.btnTranslateSelected.Size = New System.Drawing.Size(176, 29)
        Me.btnTranslateSelected.TabIndex = 9
        Me.btnTranslateSelected.Text = "Preview Selected"
        Me.btnTranslateSelected.UseVisualStyleBackColor = False
        '
        'btnUpdateTypedNames
        '
        Me.btnUpdateTypedNames.BackColor = System.Drawing.Color.DarkGoldenrod
        Me.btnUpdateTypedNames.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdateTypedNames.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnUpdateTypedNames.ForeColor = System.Drawing.Color.White
        Me.btnUpdateTypedNames.Location = New System.Drawing.Point(402, 82)
        Me.btnUpdateTypedNames.Name = "btnUpdateTypedNames"
        Me.btnUpdateTypedNames.Size = New System.Drawing.Size(176, 29)
        Me.btnUpdateTypedNames.TabIndex = 10
        Me.btnUpdateTypedNames.Text = "Save Changes"
        Me.btnUpdateTypedNames.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Maroon
        Me.btnCancel.Enabled = False
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold)
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.Location = New System.Drawing.Point(590, 82)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(92, 29)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'chkSelectAll
        '
        Me.chkSelectAll.AutoSize = True
        Me.chkSelectAll.Font = New System.Drawing.Font("Times New Roman", 11.25!, System.Drawing.FontStyle.Bold)
        Me.chkSelectAll.Location = New System.Drawing.Point(20, 138)
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
        Me.lblSummary.Location = New System.Drawing.Point(126, 140)
        Me.lblSummary.Name = "lblSummary"
        Me.lblSummary.Size = New System.Drawing.Size(173, 17)
        Me.lblSummary.TabIndex = 2
        Me.lblSummary.Text = "Records: 0    Selected: 0"
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(876, 137)
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(298, 22)
        Me.pbProgress.TabIndex = 3
        '
        'dgNames
        '
        Me.dgNames.AllowUserToAddRows = False
        Me.dgNames.AllowUserToDeleteRows = False
        Me.dgNames.BackgroundColor = System.Drawing.Color.White
        Me.dgNames.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgNames.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgNames.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSelect, Me.colRecordType, Me.colID, Me.colSourceName, Me.colOldOtherName, Me.colNewOtherName, Me.colPrimary, Me.colTag, Me.colStatus})
        Me.dgNames.Location = New System.Drawing.Point(12, 166)
        Me.dgNames.MultiSelect = False
        Me.dgNames.Name = "dgNames"
        Me.dgNames.RowHeadersVisible = False
        Me.dgNames.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgNames.Size = New System.Drawing.Size(1162, 475)
        Me.dgNames.TabIndex = 4
        '
        'colSelect
        '
        Me.colSelect.HeaderText = "Tick"
        Me.colSelect.Name = "colSelect"
        Me.colSelect.Width = 42
        '
        'colRecordType
        '
        Me.colRecordType.HeaderText = "Type"
        Me.colRecordType.Name = "colRecordType"
        Me.colRecordType.ReadOnly = True
        Me.colRecordType.Width = 75
        '
        'colID
        '
        Me.colID.HeaderText = "ID"
        Me.colID.Name = "colID"
        Me.colID.ReadOnly = True
        Me.colID.Width = 55
        '
        'colSourceName
        '
        Me.colSourceName.HeaderText = "Account / Item Name"
        Me.colSourceName.Name = "colSourceName"
        Me.colSourceName.ReadOnly = True
        Me.colSourceName.Width = 220
        '
        'colOldOtherName
        '
        Me.colOldOtherName.HeaderText = "Current OtherName"
        Me.colOldOtherName.Name = "colOldOtherName"
        Me.colOldOtherName.ReadOnly = True
        Me.colOldOtherName.Width = 220
        '
        'colNewOtherName
        '
        Me.colNewOtherName.HeaderText = "Translated OtherName"
        Me.colNewOtherName.Name = "colNewOtherName"
        Me.colNewOtherName.Width = 245
        '
        'colPrimary
        '
        Me.colPrimary.HeaderText = "Primary"
        Me.colPrimary.Name = "colPrimary"
        Me.colPrimary.ReadOnly = True
        Me.colPrimary.Width = 70
        '
        'colTag
        '
        Me.colTag.HeaderText = "Tag"
        Me.colTag.Name = "colTag"
        Me.colTag.ReadOnly = True
        Me.colTag.Visible = False
        Me.colTag.Width = 45
        '
        'colStatus
        '
        Me.colStatus.HeaderText = "Status"
        Me.colStatus.Name = "colStatus"
        Me.colStatus.ReadOnly = True
        Me.colStatus.Width = 220
        '
        'OtherNameTranslator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.dgNames)
        Me.Controls.Add(Me.pbProgress)
        Me.Controls.Add(Me.lblSummary)
        Me.Controls.Add(Me.chkSelectAll)
        Me.Controls.Add(Me.pnlTop)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OtherNameTranslator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Other Name Translator"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.dgNames, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblLanguage As System.Windows.Forms.Label
    Friend WithEvents cbLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents lblLanguageCode As System.Windows.Forms.Label
    Friend WithEvents txtLanguageCode As System.Windows.Forms.TextBox
    Friend WithEvents lblRecordType As System.Windows.Forms.Label
    Friend WithEvents cbRecordType As System.Windows.Forms.ComboBox
    Friend WithEvents chkOnlyBlank As System.Windows.Forms.CheckBox
    Friend WithEvents chkNameStyle As System.Windows.Forms.CheckBox
    Friend WithEvents chkUpdatePrimaryAccountName As System.Windows.Forms.CheckBox
    Friend WithEvents btnLoad As System.Windows.Forms.Button
    Friend WithEvents btnTranslateSelected As System.Windows.Forms.Button
    Friend WithEvents btnUpdateTypedNames As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents chkSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents lblSummary As System.Windows.Forms.Label
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents dgNames As System.Windows.Forms.DataGridView
    Friend WithEvents colSelect As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colRecordType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSourceName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colOldOtherName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colNewOtherName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPrimary As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTag As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colStatus As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
