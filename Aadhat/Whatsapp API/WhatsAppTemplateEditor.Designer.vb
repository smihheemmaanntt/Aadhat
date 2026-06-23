<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WhatsAppTemplateEditor
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.dgvTemplates = New System.Windows.Forms.DataGridView()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblCode = New System.Windows.Forms.Label()
        Me.txtTemplateCode = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.txtTemplateTitle = New System.Windows.Forms.TextBox()
        Me.lblLang = New System.Windows.Forms.Label()
        Me.cbLanguage = New System.Windows.Forms.ComboBox()
        Me.lblCategory = New System.Windows.Forms.Label()
        Me.cbCategory = New System.Windows.Forms.ComboBox()
        Me.lblHeader = New System.Windows.Forms.Label()
        Me.cbHeaderType = New System.Windows.Forms.ComboBox()
        Me.lblType = New System.Windows.Forms.Label()
        Me.txtTemplateType = New System.Windows.Forms.ComboBox()
        Me.lblFormat = New System.Windows.Forms.Label()
        Me.cbTemplateFormat = New System.Windows.Forms.ComboBox()
        Me.lblMedia = New System.Windows.Forms.Label()
        Me.txtMediaFile = New System.Windows.Forms.TextBox()
        Me.btnSelectMedia = New System.Windows.Forms.Button()
        Me.btnUploadMedia = New System.Windows.Forms.Button()
        Me.lblBody = New System.Windows.Forms.Label()
        Me.txtBody = New System.Windows.Forms.TextBox()
        Me.lblFooter = New System.Windows.Forms.Label()
        Me.txtFooter = New System.Windows.Forms.TextBox()
        Me.lblExamples = New System.Windows.Forms.Label()
        Me.txtExamples = New System.Windows.Forms.TextBox()
        Me.pnlSampleValues = New System.Windows.Forms.FlowLayoutPanel()
        Me.chkQuickReplies = New System.Windows.Forms.CheckBox()
        Me.lblButton1 = New System.Windows.Forms.Label()
        Me.txtButton1 = New System.Windows.Forms.TextBox()
        Me.lblButton2 = New System.Windows.Forms.Label()
        Me.txtButton2 = New System.Windows.Forms.TextBox()
        Me.lblParameterField = New System.Windows.Forms.Label()
        Me.cbParameterField = New System.Windows.Forms.ListBox()
        Me.btnP1 = New System.Windows.Forms.Button()
        Me.btnP2 = New System.Windows.Forms.Button()
        Me.btnP3 = New System.Windows.Forms.Button()
        Me.btnP4 = New System.Windows.Forms.Button()
        Me.btnP5 = New System.Windows.Forms.Button()
        Me.btnP6 = New System.Windows.Forms.Button()
        Me.btnSaveLocal = New System.Windows.Forms.Button()
        Me.btnSubmitMeta = New System.Windows.Forms.Button()
        Me.btnDeleteMeta = New System.Windows.Forms.Button()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnNewTemplate = New System.Windows.Forms.Button()
        Me.btnSubmitSelectedLocal = New System.Windows.Forms.Button()
        Me.chkSelectAllLocal = New System.Windows.Forms.CheckBox()
        Me.tabTemplates = New System.Windows.Forms.TabControl()
        Me.tabLocalTemplates = New System.Windows.Forms.TabPage()
        Me.tabApprovedTemplates = New System.Windows.Forms.TabPage()
        Me.tabPendingTemplates = New System.Windows.Forms.TabPage()
        Me.tabRejectedTemplates = New System.Windows.Forms.TabPage()
        Me.lblStatus = New System.Windows.Forms.Label()
        CType(Me.dgvTemplates, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlSampleValues.SuspendLayout()
        Me.tabTemplates.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvTemplates
        '
        Me.dgvTemplates.AllowUserToAddRows = False
        Me.dgvTemplates.AllowUserToDeleteRows = False
        Me.dgvTemplates.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTemplates.BackgroundColor = System.Drawing.Color.White
        Me.dgvTemplates.EnableHeadersVisualStyles = False
        Me.dgvTemplates.GridColor = System.Drawing.Color.Silver
        Me.dgvTemplates.Location = New System.Drawing.Point(12, 104)
        Me.dgvTemplates.MultiSelect = False
        Me.dgvTemplates.Name = "dgvTemplates"
        Me.dgvTemplates.RowHeadersVisible = False
        Me.dgvTemplates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTemplates.Size = New System.Drawing.Size(520, 463)
        Me.dgvTemplates.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Times New Roman", 20.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.Navy
        Me.lblTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(338, 31)
        Me.lblTitle.TabIndex = 1
        Me.lblTitle.Text = "WhatsApp Template Editor"
        '
        'lblCode
        '
        Me.lblCode.AutoSize = True
        Me.lblCode.Location = New System.Drawing.Point(552, 23)
        Me.lblCode.Name = "lblCode"
        Me.lblCode.Size = New System.Drawing.Size(187, 19)
        Me.lblCode.TabIndex = 2
        Me.lblCode.Text = "Template Code / Meta Name"
        '
        'txtTemplateCode
        '
        Me.txtTemplateCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTemplateCode.Location = New System.Drawing.Point(552, 46)
        Me.txtTemplateCode.Name = "txtTemplateCode"
        Me.txtTemplateCode.Size = New System.Drawing.Size(250, 26)
        Me.txtTemplateCode.TabIndex = 3
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(820, 23)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(34, 19)
        Me.lblName.TabIndex = 4
        Me.lblName.Text = "Title"
        '
        'txtTemplateTitle
        '
        Me.txtTemplateTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTemplateTitle.Location = New System.Drawing.Point(820, 46)
        Me.txtTemplateTitle.Name = "txtTemplateTitle"
        Me.txtTemplateTitle.Size = New System.Drawing.Size(250, 26)
        Me.txtTemplateTitle.TabIndex = 5
        '
        'lblLang
        '
        Me.lblLang.AutoSize = True
        Me.lblLang.Location = New System.Drawing.Point(552, 75)
        Me.lblLang.Name = "lblLang"
        Me.lblLang.Size = New System.Drawing.Size(67, 19)
        Me.lblLang.TabIndex = 6
        Me.lblLang.Text = "Language"
        '
        'cbLanguage
        '
        Me.cbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLanguage.Items.AddRange(New Object() {"en_US", "en", "hi"})
        Me.cbLanguage.Location = New System.Drawing.Point(552, 98)
        Me.cbLanguage.Name = "cbLanguage"
        Me.cbLanguage.Size = New System.Drawing.Size(150, 27)
        Me.cbLanguage.TabIndex = 7
        '
        'lblCategory
        '
        Me.lblCategory.AutoSize = True
        Me.lblCategory.Location = New System.Drawing.Point(718, 75)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(65, 19)
        Me.lblCategory.TabIndex = 8
        Me.lblCategory.Text = "Category"
        '
        'cbCategory
        '
        Me.cbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCategory.Items.AddRange(New Object() {"UTILITY", "MARKETING", "AUTHENTICATION"})
        Me.cbCategory.Location = New System.Drawing.Point(718, 98)
        Me.cbCategory.Name = "cbCategory"
        Me.cbCategory.Size = New System.Drawing.Size(160, 27)
        Me.cbCategory.TabIndex = 9
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Location = New System.Drawing.Point(894, 75)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(54, 19)
        Me.lblHeader.TabIndex = 10
        Me.lblHeader.Text = "Header"
        '
        'cbHeaderType
        '
        Me.cbHeaderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbHeaderType.Items.AddRange(New Object() {"none", "text", "document", "image", "video"})
        Me.cbHeaderType.Location = New System.Drawing.Point(894, 98)
        Me.cbHeaderType.Name = "cbHeaderType"
        Me.cbHeaderType.Size = New System.Drawing.Size(176, 27)
        Me.cbHeaderType.TabIndex = 11
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(552, 176)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(77, 19)
        Me.lblType.TabIndex = 12
        Me.lblType.Text = "Local Type"
        '
        'txtTemplateType
        '
        Me.txtTemplateType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.txtTemplateType.FormattingEnabled = True
        Me.txtTemplateType.Location = New System.Drawing.Point(552, 199)
        Me.txtTemplateType.Name = "txtTemplateType"
        Me.txtTemplateType.Size = New System.Drawing.Size(175, 27)
        Me.txtTemplateType.TabIndex = 13
        '
        'lblFormat
        '
        Me.lblFormat.AutoSize = True
        Me.lblFormat.Location = New System.Drawing.Point(742, 176)
        Me.lblFormat.Name = "lblFormat"
        Me.lblFormat.Size = New System.Drawing.Size(53, 19)
        Me.lblFormat.TabIndex = 37
        Me.lblFormat.Text = "Format"
        '
        'cbTemplateFormat
        '
        Me.cbTemplateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbTemplateFormat.Items.AddRange(New Object() {"English", "Regional"})
        Me.cbTemplateFormat.Location = New System.Drawing.Point(742, 199)
        Me.cbTemplateFormat.Name = "cbTemplateFormat"
        Me.cbTemplateFormat.Size = New System.Drawing.Size(104, 27)
        Me.cbTemplateFormat.TabIndex = 14
        '
        'lblMedia
        '
        Me.lblMedia.AutoSize = True
        Me.lblMedia.Location = New System.Drawing.Point(554, 126)
        Me.lblMedia.Name = "lblMedia"
        Me.lblMedia.Size = New System.Drawing.Size(97, 19)
        Me.lblMedia.TabIndex = 14
        Me.lblMedia.Text = "Sample Media"
        '
        'txtMediaFile
        '
        Me.txtMediaFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMediaFile.Location = New System.Drawing.Point(554, 148)
        Me.txtMediaFile.Name = "txtMediaFile"
        Me.txtMediaFile.ReadOnly = True
        Me.txtMediaFile.Size = New System.Drawing.Size(393, 26)
        Me.txtMediaFile.TabIndex = 15
        '
        'btnSelectMedia
        '
        Me.btnSelectMedia.BackColor = System.Drawing.Color.SteelBlue
        Me.btnSelectMedia.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSelectMedia.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSelectMedia.ForeColor = System.Drawing.Color.White
        Me.btnSelectMedia.Location = New System.Drawing.Point(953, 145)
        Me.btnSelectMedia.Name = "btnSelectMedia"
        Me.btnSelectMedia.Size = New System.Drawing.Size(115, 30)
        Me.btnSelectMedia.TabIndex = 16
        Me.btnSelectMedia.Text = "Select File"
        Me.btnSelectMedia.UseVisualStyleBackColor = False
        '
        'btnUploadMedia
        '
        Me.btnUploadMedia.BackColor = System.Drawing.Color.SeaGreen
        Me.btnUploadMedia.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnUploadMedia.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUploadMedia.ForeColor = System.Drawing.Color.White
        Me.btnUploadMedia.Location = New System.Drawing.Point(405, 9)
        Me.btnUploadMedia.Name = "btnUploadMedia"
        Me.btnUploadMedia.Size = New System.Drawing.Size(129, 30)
        Me.btnUploadMedia.TabIndex = 17
        Me.btnUploadMedia.Text = "Upload Sample"
        Me.btnUploadMedia.UseVisualStyleBackColor = False
        Me.btnUploadMedia.Visible = False
        '
        'lblBody
        '
        Me.lblBody.AutoSize = True
        Me.lblBody.Location = New System.Drawing.Point(548, 229)
        Me.lblBody.Name = "lblBody"
        Me.lblBody.Size = New System.Drawing.Size(100, 19)
        Me.lblBody.TabIndex = 18
        Me.lblBody.Text = "Message Body"
        '
        'txtBody
        '
        Me.txtBody.AcceptsReturn = True
        Me.txtBody.AcceptsTab = True
        Me.txtBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBody.Location = New System.Drawing.Point(552, 251)
        Me.txtBody.Multiline = True
        Me.txtBody.Name = "txtBody"
        Me.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBody.Size = New System.Drawing.Size(300, 163)
        Me.txtBody.TabIndex = 19
        '
        'lblFooter
        '
        Me.lblFooter.AutoSize = True
        Me.lblFooter.Location = New System.Drawing.Point(554, 420)
        Me.lblFooter.Name = "lblFooter"
        Me.lblFooter.Size = New System.Drawing.Size(57, 19)
        Me.lblFooter.TabIndex = 20
        Me.lblFooter.Text = "Footer :"
        '
        'txtFooter
        '
        Me.txtFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFooter.Location = New System.Drawing.Point(633, 416)
        Me.txtFooter.Name = "txtFooter"
        Me.txtFooter.Size = New System.Drawing.Size(437, 26)
        Me.txtFooter.TabIndex = 21
        '
        'lblExamples
        '
        Me.lblExamples.AutoSize = True
        Me.lblExamples.Location = New System.Drawing.Point(552, 471)
        Me.lblExamples.Name = "lblExamples"
        Me.lblExamples.Size = New System.Drawing.Size(97, 19)
        Me.lblExamples.TabIndex = 22
        Me.lblExamples.Text = "Sample Values"
        '
        'txtExamples
        '
        Me.txtExamples.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtExamples.Location = New System.Drawing.Point(3, 3)
        Me.txtExamples.Multiline = True
        Me.txtExamples.Name = "txtExamples"
        Me.txtExamples.Size = New System.Drawing.Size(518, 74)
        Me.txtExamples.TabIndex = 23
        '
        'pnlSampleValues
        '
        Me.pnlSampleValues.AutoScroll = True
        Me.pnlSampleValues.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSampleValues.Controls.Add(Me.txtExamples)
        Me.pnlSampleValues.Location = New System.Drawing.Point(552, 493)
        Me.pnlSampleValues.Name = "pnlSampleValues"
        Me.pnlSampleValues.Size = New System.Drawing.Size(518, 74)
        Me.pnlSampleValues.TabIndex = 35
        '
        'chkQuickReplies
        '
        Me.chkQuickReplies.AutoSize = True
        Me.chkQuickReplies.Checked = True
        Me.chkQuickReplies.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkQuickReplies.Location = New System.Drawing.Point(552, 445)
        Me.chkQuickReplies.Name = "chkQuickReplies"
        Me.chkQuickReplies.Size = New System.Drawing.Size(154, 23)
        Me.chkQuickReplies.TabIndex = 91270
        Me.chkQuickReplies.Text = "Quick Reply Buttons"
        Me.chkQuickReplies.UseVisualStyleBackColor = True
        '
        'lblButton1
        '
        Me.lblButton1.AutoSize = True
        Me.lblButton1.Location = New System.Drawing.Point(704, 446)
        Me.lblButton1.Name = "lblButton1"
        Me.lblButton1.Size = New System.Drawing.Size(27, 19)
        Me.lblButton1.TabIndex = 91271
        Me.lblButton1.Text = "B1"
        '
        'txtButton1
        '
        Me.txtButton1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtButton1.Location = New System.Drawing.Point(732, 443)
        Me.txtButton1.Name = "txtButton1"
        Me.txtButton1.Size = New System.Drawing.Size(175, 26)
        Me.txtButton1.TabIndex = 91272
        '
        'lblButton2
        '
        Me.lblButton2.AutoSize = True
        Me.lblButton2.Location = New System.Drawing.Point(914, 446)
        Me.lblButton2.Name = "lblButton2"
        Me.lblButton2.Size = New System.Drawing.Size(27, 19)
        Me.lblButton2.TabIndex = 91273
        Me.lblButton2.Text = "B2"
        '
        'txtButton2
        '
        Me.txtButton2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtButton2.Location = New System.Drawing.Point(942, 443)
        Me.txtButton2.Name = "txtButton2"
        Me.txtButton2.Size = New System.Drawing.Size(126, 26)
        Me.txtButton2.TabIndex = 91274
        '
        'lblParameterField
        '
        Me.lblParameterField.AutoSize = True
        Me.lblParameterField.Location = New System.Drawing.Point(860, 178)
        Me.lblParameterField.Name = "lblParameterField"
        Me.lblParameterField.Size = New System.Drawing.Size(105, 19)
        Me.lblParameterField.TabIndex = 38
        Me.lblParameterField.Text = "Parameter Field"
        '
        'cbParameterField
        '
        Me.cbParameterField.FormattingEnabled = True
        Me.cbParameterField.ItemHeight = 19
        Me.cbParameterField.Location = New System.Drawing.Point(858, 199)
        Me.cbParameterField.Name = "cbParameterField"
        Me.cbParameterField.Size = New System.Drawing.Size(210, 175)
        Me.cbParameterField.TabIndex = 24
        '
        'btnP1
        '
        Me.btnP1.BackColor = System.Drawing.Color.SteelBlue
        Me.btnP1.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnP1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnP1.ForeColor = System.Drawing.Color.White
        Me.btnP1.Location = New System.Drawing.Point(909, 380)
        Me.btnP1.Name = "btnP1"
        Me.btnP1.Size = New System.Drawing.Size(160, 30)
        Me.btnP1.TabIndex = 24
        Me.btnP1.Text = "+ Add Variable"
        Me.btnP1.UseVisualStyleBackColor = False
        '
        'btnP2
        '
        Me.btnP2.BackColor = System.Drawing.Color.SteelBlue
        Me.btnP2.Enabled = False
        Me.btnP2.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnP2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnP2.ForeColor = System.Drawing.Color.White
        Me.btnP2.Location = New System.Drawing.Point(633, 522)
        Me.btnP2.Name = "btnP2"
        Me.btnP2.Size = New System.Drawing.Size(75, 30)
        Me.btnP2.TabIndex = 25
        Me.btnP2.Text = "{{2}}"
        Me.btnP2.UseVisualStyleBackColor = False
        Me.btnP2.Visible = False
        '
        'btnP3
        '
        Me.btnP3.BackColor = System.Drawing.Color.SteelBlue
        Me.btnP3.Enabled = False
        Me.btnP3.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnP3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnP3.ForeColor = System.Drawing.Color.White
        Me.btnP3.Location = New System.Drawing.Point(714, 522)
        Me.btnP3.Name = "btnP3"
        Me.btnP3.Size = New System.Drawing.Size(75, 30)
        Me.btnP3.TabIndex = 26
        Me.btnP3.Text = "{{3}}"
        Me.btnP3.UseVisualStyleBackColor = False
        Me.btnP3.Visible = False
        '
        'btnP4
        '
        Me.btnP4.BackColor = System.Drawing.Color.SteelBlue
        Me.btnP4.Enabled = False
        Me.btnP4.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnP4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnP4.ForeColor = System.Drawing.Color.White
        Me.btnP4.Location = New System.Drawing.Point(795, 522)
        Me.btnP4.Name = "btnP4"
        Me.btnP4.Size = New System.Drawing.Size(75, 30)
        Me.btnP4.TabIndex = 27
        Me.btnP4.Text = "{{4}}"
        Me.btnP4.UseVisualStyleBackColor = False
        Me.btnP4.Visible = False
        '
        'btnP5
        '
        Me.btnP5.BackColor = System.Drawing.Color.SteelBlue
        Me.btnP5.Enabled = False
        Me.btnP5.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnP5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnP5.ForeColor = System.Drawing.Color.White
        Me.btnP5.Location = New System.Drawing.Point(876, 522)
        Me.btnP5.Name = "btnP5"
        Me.btnP5.Size = New System.Drawing.Size(75, 30)
        Me.btnP5.TabIndex = 28
        Me.btnP5.Text = "{{5}}"
        Me.btnP5.UseVisualStyleBackColor = False
        Me.btnP5.Visible = False
        '
        'btnP6
        '
        Me.btnP6.BackColor = System.Drawing.Color.SteelBlue
        Me.btnP6.Enabled = False
        Me.btnP6.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnP6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnP6.ForeColor = System.Drawing.Color.White
        Me.btnP6.Location = New System.Drawing.Point(957, 522)
        Me.btnP6.Name = "btnP6"
        Me.btnP6.Size = New System.Drawing.Size(75, 30)
        Me.btnP6.TabIndex = 29
        Me.btnP6.Text = "{{6}}"
        Me.btnP6.UseVisualStyleBackColor = False
        Me.btnP6.Visible = False
        '
        'btnSaveLocal
        '
        Me.btnSaveLocal.BackColor = System.Drawing.Color.DarkCyan
        Me.btnSaveLocal.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSaveLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveLocal.ForeColor = System.Drawing.Color.White
        Me.btnSaveLocal.Location = New System.Drawing.Point(552, 583)
        Me.btnSaveLocal.Name = "btnSaveLocal"
        Me.btnSaveLocal.Size = New System.Drawing.Size(130, 36)
        Me.btnSaveLocal.TabIndex = 30
        Me.btnSaveLocal.Text = "Save Local"
        Me.btnSaveLocal.UseVisualStyleBackColor = False
        '
        'btnSubmitMeta
        '
        Me.btnSubmitMeta.BackColor = System.Drawing.Color.SeaGreen
        Me.btnSubmitMeta.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSubmitMeta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSubmitMeta.ForeColor = System.Drawing.Color.White
        Me.btnSubmitMeta.Location = New System.Drawing.Point(688, 583)
        Me.btnSubmitMeta.Name = "btnSubmitMeta"
        Me.btnSubmitMeta.Size = New System.Drawing.Size(170, 36)
        Me.btnSubmitMeta.TabIndex = 31
        Me.btnSubmitMeta.Text = "Submit / Update Meta"
        Me.btnSubmitMeta.UseVisualStyleBackColor = False
        '
        'btnDeleteMeta
        '
        Me.btnDeleteMeta.BackColor = System.Drawing.Color.Firebrick
        Me.btnDeleteMeta.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnDeleteMeta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeleteMeta.ForeColor = System.Drawing.Color.White
        Me.btnDeleteMeta.Location = New System.Drawing.Point(864, 583)
        Me.btnDeleteMeta.Name = "btnDeleteMeta"
        Me.btnDeleteMeta.Size = New System.Drawing.Size(120, 36)
        Me.btnDeleteMeta.TabIndex = 32
        Me.btnDeleteMeta.Text = "Delete Meta"
        Me.btnDeleteMeta.UseVisualStyleBackColor = False
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.SteelBlue
        Me.btnRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(990, 583)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(80, 36)
        Me.btnRefresh.TabIndex = 33
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnNewTemplate
        '
        Me.btnNewTemplate.BackColor = System.Drawing.Color.DarkSlateBlue
        Me.btnNewTemplate.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnNewTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNewTemplate.ForeColor = System.Drawing.Color.White
        Me.btnNewTemplate.Location = New System.Drawing.Point(900, 9)
        Me.btnNewTemplate.Name = "btnNewTemplate"
        Me.btnNewTemplate.Size = New System.Drawing.Size(170, 32)
        Me.btnNewTemplate.TabIndex = 36
        Me.btnNewTemplate.Text = "+ New Template"
        Me.btnNewTemplate.UseVisualStyleBackColor = False
        '
        'btnSubmitSelectedLocal
        '
        Me.btnSubmitSelectedLocal.BackColor = System.Drawing.Color.SeaGreen
        Me.btnSubmitSelectedLocal.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.btnSubmitSelectedLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSubmitSelectedLocal.ForeColor = System.Drawing.Color.White
        Me.btnSubmitSelectedLocal.Location = New System.Drawing.Point(315, 59)
        Me.btnSubmitSelectedLocal.Name = "btnSubmitSelectedLocal"
        Me.btnSubmitSelectedLocal.Size = New System.Drawing.Size(216, 32)
        Me.btnSubmitSelectedLocal.TabIndex = 91275
        Me.btnSubmitSelectedLocal.Text = "Submit Selected Local"
        Me.btnSubmitSelectedLocal.UseVisualStyleBackColor = False
        '
        'chkSelectAllLocal
        '
        Me.chkSelectAllLocal.AutoSize = True
        Me.chkSelectAllLocal.ForeColor = System.Drawing.Color.Navy
        Me.chkSelectAllLocal.Location = New System.Drawing.Point(16, 80)
        Me.chkSelectAllLocal.Name = "chkSelectAllLocal"
        Me.chkSelectAllLocal.Size = New System.Drawing.Size(85, 23)
        Me.chkSelectAllLocal.TabIndex = 91277
        Me.chkSelectAllLocal.Text = "Select All"
        Me.chkSelectAllLocal.UseVisualStyleBackColor = True
        '
        'tabTemplates
        '
        Me.tabTemplates.Controls.Add(Me.tabLocalTemplates)
        Me.tabTemplates.Controls.Add(Me.tabApprovedTemplates)
        Me.tabTemplates.Controls.Add(Me.tabPendingTemplates)
        Me.tabTemplates.Controls.Add(Me.tabRejectedTemplates)
        Me.tabTemplates.Font = New System.Drawing.Font("Times New Roman", 10.0!, System.Drawing.FontStyle.Bold)
        Me.tabTemplates.Location = New System.Drawing.Point(12, 48)
        Me.tabTemplates.Name = "tabTemplates"
        Me.tabTemplates.SelectedIndex = 0
        Me.tabTemplates.Size = New System.Drawing.Size(520, 28)
        Me.tabTemplates.TabIndex = 91276
        '
        'tabLocalTemplates
        '
        Me.tabLocalTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabLocalTemplates.Name = "tabLocalTemplates"
        Me.tabLocalTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabLocalTemplates.Size = New System.Drawing.Size(512, 0)
        Me.tabLocalTemplates.TabIndex = 0
        Me.tabLocalTemplates.Text = "Local"
        Me.tabLocalTemplates.UseVisualStyleBackColor = True
        '
        'tabApprovedTemplates
        '
        Me.tabApprovedTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabApprovedTemplates.Name = "tabApprovedTemplates"
        Me.tabApprovedTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabApprovedTemplates.Size = New System.Drawing.Size(512, 0)
        Me.tabApprovedTemplates.TabIndex = 1
        Me.tabApprovedTemplates.Text = "Approved"
        Me.tabApprovedTemplates.UseVisualStyleBackColor = True
        '
        'tabPendingTemplates
        '
        Me.tabPendingTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabPendingTemplates.Name = "tabPendingTemplates"
        Me.tabPendingTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPendingTemplates.Size = New System.Drawing.Size(512, 0)
        Me.tabPendingTemplates.TabIndex = 2
        Me.tabPendingTemplates.Text = "Pending"
        Me.tabPendingTemplates.UseVisualStyleBackColor = True
        '
        'tabRejectedTemplates
        '
        Me.tabRejectedTemplates.Location = New System.Drawing.Point(4, 24)
        Me.tabRejectedTemplates.Name = "tabRejectedTemplates"
        Me.tabRejectedTemplates.Padding = New System.Windows.Forms.Padding(3)
        Me.tabRejectedTemplates.Size = New System.Drawing.Size(512, 0)
        Me.tabRejectedTemplates.TabIndex = 3
        Me.tabRejectedTemplates.Text = "Rejected"
        Me.tabRejectedTemplates.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblStatus.ForeColor = System.Drawing.Color.Navy
        Me.lblStatus.Location = New System.Drawing.Point(12, 585)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(520, 32)
        Me.lblStatus.TabIndex = 34
        Me.lblStatus.Text = "Ready"
        '
        'WhatsAppTemplateEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 19.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1088, 635)
        Me.Controls.Add(Me.btnSubmitSelectedLocal)
        Me.Controls.Add(Me.dgvTemplates)
        Me.Controls.Add(Me.tabTemplates)
        Me.Controls.Add(Me.chkSelectAllLocal)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnNewTemplate)
        Me.Controls.Add(Me.lblCode)
        Me.Controls.Add(Me.txtTemplateCode)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtTemplateTitle)
        Me.Controls.Add(Me.lblLang)
        Me.Controls.Add(Me.cbLanguage)
        Me.Controls.Add(Me.lblCategory)
        Me.Controls.Add(Me.cbCategory)
        Me.Controls.Add(Me.lblHeader)
        Me.Controls.Add(Me.cbHeaderType)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.txtTemplateType)
        Me.Controls.Add(Me.lblFormat)
        Me.Controls.Add(Me.cbTemplateFormat)
        Me.Controls.Add(Me.lblMedia)
        Me.Controls.Add(Me.txtMediaFile)
        Me.Controls.Add(Me.btnSelectMedia)
        Me.Controls.Add(Me.btnUploadMedia)
        Me.Controls.Add(Me.lblBody)
        Me.Controls.Add(Me.txtBody)
        Me.Controls.Add(Me.lblFooter)
        Me.Controls.Add(Me.txtFooter)
        Me.Controls.Add(Me.chkQuickReplies)
        Me.Controls.Add(Me.lblButton1)
        Me.Controls.Add(Me.txtButton1)
        Me.Controls.Add(Me.lblButton2)
        Me.Controls.Add(Me.txtButton2)
        Me.Controls.Add(Me.lblExamples)
        Me.Controls.Add(Me.pnlSampleValues)
        Me.Controls.Add(Me.lblParameterField)
        Me.Controls.Add(Me.cbParameterField)
        Me.Controls.Add(Me.btnP1)
        Me.Controls.Add(Me.btnP2)
        Me.Controls.Add(Me.btnP3)
        Me.Controls.Add(Me.btnP4)
        Me.Controls.Add(Me.btnP5)
        Me.Controls.Add(Me.btnP6)
        Me.Controls.Add(Me.btnSaveLocal)
        Me.Controls.Add(Me.btnSubmitMeta)
        Me.Controls.Add(Me.btnDeleteMeta)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.lblStatus)
        Me.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.Name = "WhatsAppTemplateEditor"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "WhatsApp Template Editor"
        CType(Me.dgvTemplates, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlSampleValues.ResumeLayout(False)
        Me.pnlSampleValues.PerformLayout()
        Me.tabTemplates.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvTemplates As System.Windows.Forms.DataGridView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblCode As System.Windows.Forms.Label
    Friend WithEvents txtTemplateCode As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents txtTemplateTitle As System.Windows.Forms.TextBox
    Friend WithEvents lblLang As System.Windows.Forms.Label
    Friend WithEvents cbLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents cbCategory As System.Windows.Forms.ComboBox
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents cbHeaderType As System.Windows.Forms.ComboBox
    Friend WithEvents lblType As System.Windows.Forms.Label
    Friend WithEvents txtTemplateType As System.Windows.Forms.ComboBox
    Friend WithEvents lblFormat As System.Windows.Forms.Label
    Friend WithEvents cbTemplateFormat As System.Windows.Forms.ComboBox
    Friend WithEvents lblMedia As System.Windows.Forms.Label
    Friend WithEvents txtMediaFile As System.Windows.Forms.TextBox
    Friend WithEvents btnSelectMedia As System.Windows.Forms.Button
    Friend WithEvents btnUploadMedia As System.Windows.Forms.Button
    Friend WithEvents lblBody As System.Windows.Forms.Label
    Friend WithEvents txtBody As System.Windows.Forms.TextBox
    Friend WithEvents lblFooter As System.Windows.Forms.Label
    Friend WithEvents txtFooter As System.Windows.Forms.TextBox
    Friend WithEvents chkQuickReplies As System.Windows.Forms.CheckBox
    Friend WithEvents lblButton1 As System.Windows.Forms.Label
    Friend WithEvents txtButton1 As System.Windows.Forms.TextBox
    Friend WithEvents lblButton2 As System.Windows.Forms.Label
    Friend WithEvents txtButton2 As System.Windows.Forms.TextBox
    Friend WithEvents lblExamples As System.Windows.Forms.Label
    Friend WithEvents txtExamples As System.Windows.Forms.TextBox
    Friend WithEvents pnlSampleValues As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents lblParameterField As System.Windows.Forms.Label
    Friend WithEvents cbParameterField As System.Windows.Forms.ListBox
    Friend WithEvents btnP1 As System.Windows.Forms.Button
    Friend WithEvents btnP2 As System.Windows.Forms.Button
    Friend WithEvents btnP3 As System.Windows.Forms.Button
    Friend WithEvents btnP4 As System.Windows.Forms.Button
    Friend WithEvents btnP5 As System.Windows.Forms.Button
    Friend WithEvents btnP6 As System.Windows.Forms.Button
    Friend WithEvents btnSaveLocal As System.Windows.Forms.Button
    Friend WithEvents btnSubmitMeta As System.Windows.Forms.Button
    Friend WithEvents btnDeleteMeta As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnNewTemplate As System.Windows.Forms.Button
    Friend WithEvents btnSubmitSelectedLocal As System.Windows.Forms.Button
    Friend WithEvents chkSelectAllLocal As System.Windows.Forms.CheckBox
    Friend WithEvents tabTemplates As System.Windows.Forms.TabControl
    Friend WithEvents tabLocalTemplates As System.Windows.Forms.TabPage
    Friend WithEvents tabApprovedTemplates As System.Windows.Forms.TabPage
    Friend WithEvents tabPendingTemplates As System.Windows.Forms.TabPage
    Friend WithEvents tabRejectedTemplates As System.Windows.Forms.TabPage
    Friend WithEvents lblStatus As System.Windows.Forms.Label
End Class










