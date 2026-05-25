<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ComplaintRegisterForm
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.rootPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblCustomerCode = New System.Windows.Forms.Label()
        Me.lblCustomerCodeValue = New System.Windows.Forms.Label()
        Me.lblFirmName = New System.Windows.Forms.Label()
        Me.lblFirmNameValue = New System.Windows.Forms.Label()
        Me.lblMobile = New System.Windows.Forms.Label()
        Me.txtMobile = New System.Windows.Forms.TextBox()
        Me.lblSubject = New System.Windows.Forms.Label()
        Me.cboSubject = New System.Windows.Forms.ComboBox()
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.buttonPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.rootPanel.SuspendLayout()
        Me.buttonPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'rootPanel
        '
        Me.rootPanel.ColumnCount = 2
        Me.rootPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130.0!))
        Me.rootPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.rootPanel.Controls.Add(Me.lblTitle, 0, 0)
        Me.rootPanel.Controls.Add(Me.lblCustomerCode, 0, 1)
        Me.rootPanel.Controls.Add(Me.lblCustomerCodeValue, 1, 1)
        Me.rootPanel.Controls.Add(Me.lblFirmName, 0, 2)
        Me.rootPanel.Controls.Add(Me.lblFirmNameValue, 1, 2)
        Me.rootPanel.Controls.Add(Me.lblMobile, 0, 3)
        Me.rootPanel.Controls.Add(Me.txtMobile, 1, 3)
        Me.rootPanel.Controls.Add(Me.lblSubject, 0, 4)
        Me.rootPanel.Controls.Add(Me.cboSubject, 1, 4)
        Me.rootPanel.Controls.Add(Me.lblDescription, 0, 5)
        Me.rootPanel.Controls.Add(Me.txtDescription, 1, 5)
        Me.rootPanel.Controls.Add(Me.lblStatus, 1, 6)
        Me.rootPanel.Controls.Add(Me.buttonPanel, 1, 7)
        Me.rootPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rootPanel.Location = New System.Drawing.Point(0, 0)
        Me.rootPanel.Name = "rootPanel"
        Me.rootPanel.Padding = New System.Windows.Forms.Padding(18)
        Me.rootPanel.RowCount = 9
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.rootPanel.Size = New System.Drawing.Size(544, 481)
        Me.rootPanel.TabIndex = 0
        '
        'lblTitle
        '
        Me.rootPanel.SetColumnSpan(Me.lblTitle, 2)
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 13.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(71, Byte), Integer))
        Me.lblTitle.Location = New System.Drawing.Point(21, 18)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(502, 34)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Register Complaint"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCustomerCode
        '
        Me.lblCustomerCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCustomerCode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.lblCustomerCode.Location = New System.Drawing.Point(21, 52)
        Me.lblCustomerCode.Name = "lblCustomerCode"
        Me.lblCustomerCode.Size = New System.Drawing.Size(124, 44)
        Me.lblCustomerCode.TabIndex = 1
        Me.lblCustomerCode.Text = "Customer Code"
        Me.lblCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCustomerCodeValue
        '
        Me.lblCustomerCodeValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lblCustomerCodeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCustomerCodeValue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCustomerCodeValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustomerCodeValue.Location = New System.Drawing.Point(151, 55)
        Me.lblCustomerCodeValue.Margin = New System.Windows.Forms.Padding(3)
        Me.lblCustomerCodeValue.Name = "lblCustomerCodeValue"
        Me.lblCustomerCodeValue.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblCustomerCodeValue.Size = New System.Drawing.Size(372, 38)
        Me.lblCustomerCodeValue.TabIndex = 0
        Me.lblCustomerCodeValue.Text = "-"
        Me.lblCustomerCodeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFirmName
        '
        Me.lblFirmName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFirmName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.lblFirmName.Location = New System.Drawing.Point(21, 96)
        Me.lblFirmName.Name = "lblFirmName"
        Me.lblFirmName.Size = New System.Drawing.Size(124, 44)
        Me.lblFirmName.TabIndex = 2
        Me.lblFirmName.Text = "Firm Name"
        Me.lblFirmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFirmNameValue
        '
        Me.lblFirmNameValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lblFirmNameValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblFirmNameValue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFirmNameValue.Location = New System.Drawing.Point(151, 99)
        Me.lblFirmNameValue.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFirmNameValue.Name = "lblFirmNameValue"
        Me.lblFirmNameValue.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblFirmNameValue.Size = New System.Drawing.Size(372, 38)
        Me.lblFirmNameValue.TabIndex = 1
        Me.lblFirmNameValue.Text = "-"
        Me.lblFirmNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMobile
        '
        Me.lblMobile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMobile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.lblMobile.Location = New System.Drawing.Point(21, 140)
        Me.lblMobile.Name = "lblMobile"
        Me.lblMobile.Size = New System.Drawing.Size(124, 44)
        Me.lblMobile.TabIndex = 3
        Me.lblMobile.Text = "Mobile Number"
        Me.lblMobile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMobile
        '
        Me.txtMobile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMobile.Location = New System.Drawing.Point(151, 143)
        Me.txtMobile.Name = "txtMobile"
        Me.txtMobile.Size = New System.Drawing.Size(372, 23)
        Me.txtMobile.TabIndex = 2
        '
        'lblSubject
        '
        Me.lblSubject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblSubject.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.lblSubject.Location = New System.Drawing.Point(21, 184)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(124, 44)
        Me.lblSubject.TabIndex = 4
        Me.lblSubject.Text = "Subject"
        Me.lblSubject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cboSubject
        '
        Me.cboSubject.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubject.FormattingEnabled = True
        Me.cboSubject.Items.AddRange(New Object() {"01 - Installation Pending", "02 - Activation Key Not Working", "03 - Mobile App Not Syncing", "04 - WhatsApp Integration Not Working", "05 - Error During Initial Setup", "06 - User Login Failed", "07 - Forgot Password Issue", "08 - Device Binding Issue", "09 - OTP Not Received", "10 - Invoice Format Issue", "11 - GST Report Incorrect", "12 - E-Invoice Not Generating", "13 - HSN/SAC Code Problem", "14 - Tax Calculation Wrong", "15 - Ledger Not Matching", "16 - Outstanding Report Blank", "17 - Trial Balance Not Opening", "18 - GSTR-1/GSTR-3B Error", "19 - Data Not Saving", "20 - Cloud Backup Failed", "21 - Data Sync Slow", "22 - Auto Backup Not Working", "23 - App Crashing", "24 - Data Not Showing in App", "25 - Push Notification Not Received", "26 - Customer Ledger Not Visible", "27 - WhatsApp Not Sending Invoice", "28 - Message Template Not Working", "29 - QR Code Scan Not Responding", "30 - Latest Update Not Installed", "31 - AMC Reminder Not Showing", "32 - New Feature Request", "33 - Software Slow", "34 - Request for Training", "35 - Request for Demo", "36 - Complaint Regarding Support", "37 - Feedback / Suggestion", "38 - Printing Issue", "39 - Export Not Working", "40 - Other"})
        Me.cboSubject.Location = New System.Drawing.Point(151, 187)
        Me.cboSubject.Name = "cboSubject"
        Me.cboSubject.Size = New System.Drawing.Size(372, 23)
        Me.cboSubject.TabIndex = 3
        '
        'lblDescription
        '
        Me.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblDescription.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
        Me.lblDescription.Location = New System.Drawing.Point(21, 228)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(124, 127)
        Me.lblDescription.TabIndex = 5
        Me.lblDescription.Text = "Description"
        Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtDescription
        '
        Me.txtDescription.AcceptsReturn = True
        Me.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDescription.Location = New System.Drawing.Point(151, 231)
        Me.txtDescription.MaxLength = 2000
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(372, 121)
        Me.txtDescription.TabIndex = 4
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblStatus.Location = New System.Drawing.Point(151, 355)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(372, 36)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'buttonPanel
        '
        Me.buttonPanel.Controls.Add(Me.btnSubmit)
        Me.buttonPanel.Controls.Add(Me.btnClose)
        Me.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.buttonPanel.Location = New System.Drawing.Point(151, 394)
        Me.buttonPanel.Name = "buttonPanel"
        Me.buttonPanel.Size = New System.Drawing.Size(372, 42)
        Me.buttonPanel.TabIndex = 7
        '
        'btnSubmit
        '
        Me.btnSubmit.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
        Me.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSubmit.ForeColor = System.Drawing.Color.White
        Me.btnSubmit.Location = New System.Drawing.Point(224, 3)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(145, 32)
        Me.btnSubmit.TabIndex = 0
        Me.btnSubmit.Text = "Submit Complaint"
        Me.btnSubmit.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(128, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(90, 32)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'ComplaintRegisterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(544, 481)
        Me.Controls.Add(Me.rootPanel)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MinimumSize = New System.Drawing.Size(520, 480)
        Me.Name = "ComplaintRegisterForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Register Complaint"
        Me.rootPanel.ResumeLayout(False)
        Me.rootPanel.PerformLayout()
        Me.buttonPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents rootPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblCustomerCode As System.Windows.Forms.Label
    Friend WithEvents lblCustomerCodeValue As System.Windows.Forms.Label
    Friend WithEvents lblFirmName As System.Windows.Forms.Label
    Friend WithEvents lblFirmNameValue As System.Windows.Forms.Label
    Friend WithEvents lblMobile As System.Windows.Forms.Label
    Friend WithEvents txtMobile As System.Windows.Forms.TextBox
    Friend WithEvents lblSubject As System.Windows.Forms.Label
    Friend WithEvents cboSubject As System.Windows.Forms.ComboBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents buttonPanel As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnSubmit As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
