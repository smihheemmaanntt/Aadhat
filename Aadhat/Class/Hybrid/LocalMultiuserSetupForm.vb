Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Public Class LocalMultiuserSetupForm
    Inherits Form

    Private lblLicense As Label
    Private lblCurrent As Label
    Private txtAdminFolder As TextBox
    Private txtClientDataFile As TextBox
    Private txtInstructions As TextBox
    Private lblStatus As Label

    Public Sub New()
        Me.Text = "Local Multiuser Setup"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Width = 760
        Me.Height = 640
        Me.MinimumSize = New Size(720, 600)
        BuildUi()
        LoadInfo()
    End Sub

    Private Sub BuildUi()
        Dim root As New TableLayoutPanel()
        root.Dock = DockStyle.Fill
        root.Padding = New Padding(18)
        root.ColumnCount = 1
        root.RowCount = 7
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 70))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 70))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 128))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 128))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 58))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 48))

        Dim title As New Label()
        title.Text = "Local Multiuser Setup" & vbCrLf & "Owner can make this PC the Admin PC, or connect this PC as a Client PC. Permission comes from the installed license."
        title.Dock = DockStyle.Fill
        title.Font = New Font(Me.Font.FontFamily, 11.0F, FontStyle.Bold)
        title.ForeColor = Color.FromArgb(30, 41, 59)

        Dim infoPanel As New TableLayoutPanel()
        infoPanel.Dock = DockStyle.Fill
        infoPanel.ColumnCount = 2
        infoPanel.RowCount = 1
        infoPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        infoPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))

        lblLicense = CardLabel()
        lblCurrent = CardLabel()
        infoPanel.Controls.Add(lblLicense, 0, 0)
        infoPanel.Controls.Add(lblCurrent, 1, 0)

        Dim adminPanel As GroupBox = BuildAdminPanel()
        Dim clientPanel As GroupBox = BuildClientPanel()

        txtInstructions = New TextBox()
        txtInstructions.Multiline = True
        txtInstructions.ScrollBars = ScrollBars.Vertical
        txtInstructions.ReadOnly = True
        txtInstructions.Dock = DockStyle.Fill
        txtInstructions.Font = New Font("Consolas", 9.0F)

        lblStatus = New Label()
        lblStatus.Dock = DockStyle.Fill
        lblStatus.Padding = New Padding(10)
        lblStatus.BackColor = Color.FromArgb(248, 250, 252)
        lblStatus.ForeColor = Color.FromArgb(71, 85, 105)

        Dim buttons As New FlowLayoutPanel()
        buttons.FlowDirection = FlowDirection.RightToLeft
        buttons.Dock = DockStyle.Fill

        Dim btnClose As New Button()
        btnClose.Text = "Close"
        btnClose.Width = 100
        AddHandler btnClose.Click, AddressOf Close_Click

        Dim btnRefresh As New Button()
        btnRefresh.Text = "Refresh"
        btnRefresh.Width = 100
        AddHandler btnRefresh.Click, AddressOf Refresh_Click

        Dim btnOff As New Button()
        btnOff.Text = "Turn Off Multiuser"
        btnOff.Width = 145
        AddHandler btnOff.Click, AddressOf TurnOff_Click

        buttons.Controls.Add(btnClose)
        buttons.Controls.Add(btnRefresh)
        buttons.Controls.Add(btnOff)

        root.Controls.Add(title, 0, 0)
        root.Controls.Add(infoPanel, 0, 1)
        root.Controls.Add(adminPanel, 0, 2)
        root.Controls.Add(clientPanel, 0, 3)
        root.Controls.Add(txtInstructions, 0, 4)
        root.Controls.Add(lblStatus, 0, 5)
        root.Controls.Add(buttons, 0, 6)

        Me.Controls.Add(root)
    End Sub

    Private Function BuildAdminPanel() As GroupBox
        Dim box As New GroupBox()
        box.Text = "Step 1: Make this PC Admin / Server PC"
        box.Dock = DockStyle.Fill

        Dim panel As New TableLayoutPanel()
        panel.Dock = DockStyle.Fill
        panel.Padding = New Padding(10)
        panel.ColumnCount = 3
        panel.RowCount = 2
        panel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 115))
        panel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        panel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 155))

        Dim lbl As New Label()
        lbl.Text = "Data Folder"
        lbl.Dock = DockStyle.Fill
        lbl.TextAlign = ContentAlignment.MiddleLeft

        txtAdminFolder = New TextBox()
        txtAdminFolder.Dock = DockStyle.Fill

        Dim btnBrowse As New Button()
        btnBrowse.Text = "Choose Folder"
        btnBrowse.Dock = DockStyle.Fill
        AddHandler btnBrowse.Click, AddressOf ChooseAdminFolder_Click

        Dim btnUseCurrent As New Button()
        btnUseCurrent.Text = "Use Current Company Folder"
        btnUseCurrent.Dock = DockStyle.Fill
        AddHandler btnUseCurrent.Click, AddressOf UseCurrentCompany_Click

        Dim btnMakeAdmin As New Button()
        btnMakeAdmin.Text = "Make This PC Admin"
        btnMakeAdmin.Dock = DockStyle.Fill
        AddHandler btnMakeAdmin.Click, AddressOf MakeAdmin_Click

        panel.Controls.Add(lbl, 0, 0)
        panel.Controls.Add(txtAdminFolder, 1, 0)
        panel.Controls.Add(btnBrowse, 2, 0)
        panel.Controls.Add(btnUseCurrent, 1, 1)
        panel.Controls.Add(btnMakeAdmin, 2, 1)
        box.Controls.Add(panel)
        Return box
    End Function

    Private Function BuildClientPanel() As GroupBox
        Dim box As New GroupBox()
        box.Text = "Step 2: Connect this PC as Client PC"
        box.Dock = DockStyle.Fill

        Dim panel As New TableLayoutPanel()
        panel.Dock = DockStyle.Fill
        panel.Padding = New Padding(10)
        panel.ColumnCount = 3
        panel.RowCount = 2
        panel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 115))
        panel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
        panel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 155))

        Dim lbl As New Label()
        lbl.Text = "Shared Data.db"
        lbl.Dock = DockStyle.Fill
        lbl.TextAlign = ContentAlignment.MiddleLeft

        txtClientDataFile = New TextBox()
        txtClientDataFile.Dock = DockStyle.Fill

        Dim btnBrowse As New Button()
        btnBrowse.Text = "Select Data.db"
        btnBrowse.Dock = DockStyle.Fill
        AddHandler btnBrowse.Click, AddressOf ChooseClientDb_Click

        Dim help As New Label()
        help.Text = "Select Data.db from Admin PC shared folder, e.g. \\ADMIN-PC\AadhatData\Data.db"
        help.Dock = DockStyle.Fill
        help.TextAlign = ContentAlignment.MiddleLeft
        help.ForeColor = Color.DimGray

        Dim btnConnect As New Button()
        btnConnect.Text = "Connect as Client"
        btnConnect.Dock = DockStyle.Fill
        AddHandler btnConnect.Click, AddressOf ConnectClient_Click

        panel.Controls.Add(lbl, 0, 0)
        panel.Controls.Add(txtClientDataFile, 1, 0)
        panel.Controls.Add(btnBrowse, 2, 0)
        panel.Controls.Add(help, 1, 1)
        panel.Controls.Add(btnConnect, 2, 1)
        box.Controls.Add(panel)
        Return box
    End Function

    Private Function CardLabel() As Label
        Dim lbl As New Label()
        lbl.Dock = DockStyle.Fill
        lbl.Padding = New Padding(10)
        lbl.BackColor = Color.FromArgb(248, 250, 252)
        lbl.BorderStyle = BorderStyle.FixedSingle
        lbl.ForeColor = Color.FromArgb(30, 41, 59)
        Return lbl
    End Function

    Private Sub LoadInfo()
        Dim summary As LocalMultiuserLicenseSummary = LocalMultiuserRuntime.LicenseSummary()
        lblLicense.Text = "License" & vbCrLf & _
            "Customer: " & If(summary.CustomerCode = "", "Not available", summary.CustomerCode) & vbCrLf & _
            "LAN: " & If(summary.LanAllowed, "Yes", "No") & " (" & summary.RegisteredPcCount.ToString() & "/" & summary.AllowedPcCount.ToString() & ")"

        Dim settings As LocalMultiuserSettings = LocalMultiuserSettings.Load()
        lblCurrent.Text = "This PC" & vbCrLf & _
            "Name: " & Environment.MachineName & vbCrLf & _
            "Role: " & If(settings.Enabled, settings.Role, "Single User")

        txtAdminFolder.Text = LocalMultiuserRuntime.CurrentCompanyDataFolder()
        txtInstructions.Text = LocalMultiuserRuntime.BuildOwnerInstructions(txtAdminFolder.Text)
        lblStatus.Text = summary.Message
    End Sub

    Private Sub UseCurrentCompany_Click(ByVal sender As Object, ByVal e As EventArgs)
        txtAdminFolder.Text = LocalMultiuserRuntime.CurrentCompanyDataFolder()
        txtInstructions.Text = LocalMultiuserRuntime.BuildOwnerInstructions(txtAdminFolder.Text)
    End Sub

    Private Sub ChooseAdminFolder_Click(ByVal sender As Object, ByVal e As EventArgs)
        Using dlg As New FolderBrowserDialog()
            dlg.Description = "Select the folder that contains the company Data.db on Admin PC."
            If Directory.Exists(txtAdminFolder.Text) Then dlg.SelectedPath = txtAdminFolder.Text
            If dlg.ShowDialog(Me) = DialogResult.OK Then
                txtAdminFolder.Text = dlg.SelectedPath
                txtInstructions.Text = LocalMultiuserRuntime.BuildOwnerInstructions(txtAdminFolder.Text)
            End If
        End Using
    End Sub

    Private Sub ChooseClientDb_Click(ByVal sender As Object, ByVal e As EventArgs)
        Using dlg As New OpenFileDialog()
            dlg.Title = "Select shared Data.db from Admin PC"
            dlg.Filter = "Aadhat Database (Data.db)|Data.db|SQLite Database (*.db)|*.db|All Files (*.*)|*.*"
            If dlg.ShowDialog(Me) = DialogResult.OK Then
                txtClientDataFile.Text = dlg.FileName
            End If
        End Using
    End Sub

    Private Sub MakeAdmin_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            LocalMultiuserRuntime.SaveAdminSetup(txtAdminFolder.Text.Trim())
            txtInstructions.Text = LocalMultiuserRuntime.BuildOwnerInstructions(txtAdminFolder.Text.Trim())
            lblStatus.Text = "Done. This PC is marked as Admin PC. Share this data folder in Windows, then connect client PCs to the shared Data.db."
            LoadInfo()
        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub ConnectClient_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            LocalMultiuserRuntime.SaveClientSetup(txtClientDataFile.Text.Trim())
            lblStatus.Text = "Done. This PC is connected as Client PC. Restart Aadhat and open the company from the shared path."
            LoadInfo()
        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub TurnOff_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            LocalMultiuserRuntime.TurnOff()
            lblStatus.Text = "Multiuser mode is off. Existing single-user data is unchanged."
            LoadInfo()
        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub Close_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close()
    End Sub

    Private Sub Refresh_Click(ByVal sender As Object, ByVal e As EventArgs)
        LoadInfo()
    End Sub
End Class
