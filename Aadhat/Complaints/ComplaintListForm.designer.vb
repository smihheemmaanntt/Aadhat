<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ComplaintListForm
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
        Me.infoPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.lblCustomerCode = New System.Windows.Forms.Label()
        Me.lblCustomerCodeValue = New System.Windows.Forms.Label()
        Me.lblFirmName = New System.Windows.Forms.Label()
        Me.lblFirmNameValue = New System.Windows.Forms.Label()
        Me.lblMobile = New System.Windows.Forms.Label()
        Me.txtMobile = New System.Windows.Forms.TextBox()
        Me.buttonPanel = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.dgvComplaints = New System.Windows.Forms.DataGridView()
        Me.colCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSubject = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colCreatedAt = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFeedback = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFeedbackDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtDetails = New System.Windows.Forms.TextBox()
        Me.rootPanel.SuspendLayout()
        Me.infoPanel.SuspendLayout()
        Me.buttonPanel.SuspendLayout()
        CType(Me.dgvComplaints, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'rootPanel
        '
        Me.rootPanel.ColumnCount = 1
        Me.rootPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.rootPanel.Controls.Add(Me.lblTitle, 0, 0)
        Me.rootPanel.Controls.Add(Me.infoPanel, 0, 1)
        Me.rootPanel.Controls.Add(Me.lblStatus, 0, 2)
        Me.rootPanel.Controls.Add(Me.dgvComplaints, 0, 3)
        Me.rootPanel.Controls.Add(Me.txtDetails, 0, 4)
        Me.rootPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rootPanel.Location = New System.Drawing.Point(0, 0)
        Me.rootPanel.Name = "rootPanel"
        Me.rootPanel.Padding = New System.Windows.Forms.Padding(14)
        Me.rootPanel.RowCount = 5
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.0!))
        Me.rootPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.0!))
        Me.rootPanel.Size = New System.Drawing.Size(930, 561)
        Me.rootPanel.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 13.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer), CType(CType(71, Byte), Integer))
        Me.lblTitle.Location = New System.Drawing.Point(17, 14)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(896, 34)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Complaint Status / Feedback"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'infoPanel
        '
        Me.infoPanel.ColumnCount = 4
        Me.infoPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115.0!))
        Me.infoPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.infoPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 95.0!))
        Me.infoPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.infoPanel.Controls.Add(Me.lblCustomerCode, 0, 0)
        Me.infoPanel.Controls.Add(Me.lblCustomerCodeValue, 1, 0)
        Me.infoPanel.Controls.Add(Me.lblFirmName, 2, 0)
        Me.infoPanel.Controls.Add(Me.lblFirmNameValue, 3, 0)
        Me.infoPanel.Controls.Add(Me.lblMobile, 0, 1)
        Me.infoPanel.Controls.Add(Me.txtMobile, 1, 1)
        Me.infoPanel.Controls.Add(Me.buttonPanel, 3, 1)
        Me.infoPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.infoPanel.Location = New System.Drawing.Point(17, 51)
        Me.infoPanel.Name = "infoPanel"
        Me.infoPanel.RowCount = 2
        Me.infoPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.infoPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.infoPanel.Size = New System.Drawing.Size(896, 84)
        Me.infoPanel.TabIndex = 1
        '
        'lblCustomerCode
        '
        Me.lblCustomerCode.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCustomerCode.Location = New System.Drawing.Point(3, 0)
        Me.lblCustomerCode.Name = "lblCustomerCode"
        Me.lblCustomerCode.Size = New System.Drawing.Size(109, 42)
        Me.lblCustomerCode.TabIndex = 0
        Me.lblCustomerCode.Text = "Code"
        Me.lblCustomerCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblCustomerCodeValue
        '
        Me.lblCustomerCodeValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lblCustomerCodeValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCustomerCodeValue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCustomerCodeValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblCustomerCodeValue.Location = New System.Drawing.Point(118, 3)
        Me.lblCustomerCodeValue.Margin = New System.Windows.Forms.Padding(3)
        Me.lblCustomerCodeValue.Name = "lblCustomerCodeValue"
        Me.lblCustomerCodeValue.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblCustomerCodeValue.Size = New System.Drawing.Size(337, 36)
        Me.lblCustomerCodeValue.TabIndex = 0
        Me.lblCustomerCodeValue.Text = "-"
        Me.lblCustomerCodeValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFirmName
        '
        Me.lblFirmName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFirmName.Location = New System.Drawing.Point(461, 0)
        Me.lblFirmName.Name = "lblFirmName"
        Me.lblFirmName.Size = New System.Drawing.Size(89, 42)
        Me.lblFirmName.TabIndex = 1
        Me.lblFirmName.Text = "Firm"
        Me.lblFirmName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblFirmNameValue
        '
        Me.lblFirmNameValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.lblFirmNameValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblFirmNameValue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFirmNameValue.Location = New System.Drawing.Point(556, 3)
        Me.lblFirmNameValue.Margin = New System.Windows.Forms.Padding(3)
        Me.lblFirmNameValue.Name = "lblFirmNameValue"
        Me.lblFirmNameValue.Padding = New System.Windows.Forms.Padding(8, 0, 0, 0)
        Me.lblFirmNameValue.Size = New System.Drawing.Size(337, 36)
        Me.lblFirmNameValue.TabIndex = 1
        Me.lblFirmNameValue.Text = "-"
        Me.lblFirmNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblMobile
        '
        Me.lblMobile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblMobile.Location = New System.Drawing.Point(3, 42)
        Me.lblMobile.Name = "lblMobile"
        Me.lblMobile.Size = New System.Drawing.Size(109, 42)
        Me.lblMobile.TabIndex = 2
        Me.lblMobile.Text = "Mobile"
        Me.lblMobile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMobile
        '
        Me.txtMobile.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMobile.Location = New System.Drawing.Point(118, 45)
        Me.txtMobile.Name = "txtMobile"
        Me.txtMobile.Size = New System.Drawing.Size(337, 23)
        Me.txtMobile.TabIndex = 2
        '
        'buttonPanel
        '
        Me.buttonPanel.Controls.Add(Me.btnRefresh)
        Me.buttonPanel.Controls.Add(Me.btnClose)
        Me.buttonPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.buttonPanel.Location = New System.Drawing.Point(556, 45)
        Me.buttonPanel.Name = "buttonPanel"
        Me.buttonPanel.Size = New System.Drawing.Size(337, 36)
        Me.buttonPanel.TabIndex = 3
        '
        'btnRefresh
        '
        Me.btnRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
        Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.Location = New System.Drawing.Point(226, 3)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(108, 30)
        Me.btnRefresh.TabIndex = 0
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = False
        '
        'btnClose
        '
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Location = New System.Drawing.Point(145, 3)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 30)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
        Me.lblStatus.Location = New System.Drawing.Point(17, 138)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(896, 28)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'dgvComplaints
        '
        Me.dgvComplaints.AllowUserToAddRows = False
        Me.dgvComplaints.AllowUserToDeleteRows = False
        Me.dgvComplaints.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvComplaints.BackgroundColor = System.Drawing.Color.White
        Me.dgvComplaints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvComplaints.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colCode, Me.colStatus, Me.colSubject, Me.colCreatedAt, Me.colFeedback, Me.colFeedbackDate})
        Me.dgvComplaints.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvComplaints.Location = New System.Drawing.Point(17, 169)
        Me.dgvComplaints.MultiSelect = False
        Me.dgvComplaints.Name = "dgvComplaints"
        Me.dgvComplaints.ReadOnly = True
        Me.dgvComplaints.RowHeadersVisible = False
        Me.dgvComplaints.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvComplaints.Size = New System.Drawing.Size(896, 199)
        Me.dgvComplaints.TabIndex = 3
        '
        'colCode
        '
        Me.colCode.HeaderText = "Complaint No"
        Me.colCode.Name = "colCode"
        Me.colCode.ReadOnly = True
        '
        'colStatus
        '
        Me.colStatus.HeaderText = "Status"
        Me.colStatus.Name = "colStatus"
        Me.colStatus.ReadOnly = True
        '
        'colSubject
        '
        Me.colSubject.HeaderText = "Subject"
        Me.colSubject.Name = "colSubject"
        Me.colSubject.ReadOnly = True
        '
        'colCreatedAt
        '
        Me.colCreatedAt.HeaderText = "Date"
        Me.colCreatedAt.Name = "colCreatedAt"
        Me.colCreatedAt.ReadOnly = True
        '
        'colFeedback
        '
        Me.colFeedback.HeaderText = "Latest Feedback"
        Me.colFeedback.Name = "colFeedback"
        Me.colFeedback.ReadOnly = True
        '
        'colFeedbackDate
        '
        Me.colFeedbackDate.HeaderText = "Feedback Date"
        Me.colFeedbackDate.Name = "colFeedbackDate"
        Me.colFeedbackDate.ReadOnly = True
        '
        'txtDetails
        '
        Me.txtDetails.BackColor = System.Drawing.Color.White
        Me.txtDetails.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDetails.Location = New System.Drawing.Point(17, 374)
        Me.txtDetails.Multiline = True
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ReadOnly = True
        Me.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDetails.Size = New System.Drawing.Size(896, 170)
        Me.txtDetails.TabIndex = 4
        '
        'ComplaintListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(930, 561)
        Me.Controls.Add(Me.rootPanel)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MinimumSize = New System.Drawing.Size(780, 520)
        Me.Name = "ComplaintListForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Complaint Status"
        Me.rootPanel.ResumeLayout(False)
        Me.rootPanel.PerformLayout()
        Me.infoPanel.ResumeLayout(False)
        Me.infoPanel.PerformLayout()
        Me.buttonPanel.ResumeLayout(False)
        CType(Me.dgvComplaints, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents rootPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents infoPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblCustomerCode As System.Windows.Forms.Label
    Friend WithEvents lblCustomerCodeValue As System.Windows.Forms.Label
    Friend WithEvents lblFirmName As System.Windows.Forms.Label
    Friend WithEvents lblFirmNameValue As System.Windows.Forms.Label
    Friend WithEvents lblMobile As System.Windows.Forms.Label
    Friend WithEvents txtMobile As System.Windows.Forms.TextBox
    Friend WithEvents buttonPanel As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents dgvComplaints As System.Windows.Forms.DataGridView
    Friend WithEvents colCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSubject As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCreatedAt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFeedback As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFeedbackDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtDetails As System.Windows.Forms.TextBox
End Class
