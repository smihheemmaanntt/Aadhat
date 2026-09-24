Imports System.Drawing
Imports System.Web.Script.Serialization
Imports System.Windows.Forms

Public Class HybridSettingsForm
    Inherits Form

    Private txtSetupCode As TextBox
    Private chkEnabled As CheckBox
    Private txtApiBaseUrl As TextBox
    Private txtApiToken As TextBox
    Private txtAuthorizationKey As TextBox
    Private txtDeviceToken As TextBox
    Private lblStatus As Label
    Private lblConfigPath As Label
    Private grpAdvanced As GroupBox

    Public Sub New()
        Me.Text = "Hybrid Multiuser Setup"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Width = 680
        Me.Height = 690
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        BuildUi()
        LoadSettings()
    End Sub

    Private Sub BuildUi()
        Dim root As New TableLayoutPanel()
        root.Dock = DockStyle.Fill
        root.Padding = New Padding(18)
        root.ColumnCount = 1
        root.RowCount = 7
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 72))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 36))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 150))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 48))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 66))
        root.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 54))

        Dim title As New Label()
        title.Text = "Paste setup code and start Hybrid on this PC" & vbCrLf & "API URL, token, authorization key and device token will be handled automatically."
        title.Dock = DockStyle.Fill
        title.Font = New Font(Me.Font.FontFamily, 11.0F, FontStyle.Bold)
        title.ForeColor = Color.FromArgb(30, 41, 59)

        Dim hint As New Label()
        hint.Text = "Get the setup code from Web Admin > Setup > Generate Setup Code."
        hint.Dock = DockStyle.Fill
        hint.ForeColor = Color.DimGray

        txtSetupCode = New TextBox()
        txtSetupCode.Multiline = True
        txtSetupCode.ScrollBars = ScrollBars.Vertical
        txtSetupCode.Dock = DockStyle.Fill
        txtSetupCode.Font = New Font("Consolas", 9.0F)

        chkEnabled = New CheckBox()
        chkEnabled.Text = "Hybrid Mode will be enabled after successful activation"
        chkEnabled.AutoSize = True
        chkEnabled.Enabled = False

        txtApiBaseUrl = New TextBox()
        txtApiToken = New TextBox()
        txtAuthorizationKey = New TextBox()
        txtDeviceToken = New TextBox()
        txtApiToken.UseSystemPasswordChar = True
        txtAuthorizationKey.UseSystemPasswordChar = True
        txtDeviceToken.UseSystemPasswordChar = True

        lblStatus = New Label()
        lblStatus.AutoSize = False
        lblStatus.Dock = DockStyle.Fill
        lblStatus.Padding = New Padding(10)
        lblStatus.BackColor = Color.FromArgb(248, 250, 252)
        lblStatus.ForeColor = Color.DimGray

        lblConfigPath = New Label()
        lblConfigPath.AutoSize = False
        lblConfigPath.Dock = DockStyle.Fill
        lblConfigPath.ForeColor = Color.DimGray

        grpAdvanced = New GroupBox()
        grpAdvanced.Text = "Advanced connection details"
        grpAdvanced.Dock = DockStyle.Fill
        grpAdvanced.ForeColor = Color.FromArgb(51, 65, 85)

        Dim advancedPanel As New TableLayoutPanel()
        advancedPanel.Dock = DockStyle.Fill
        advancedPanel.Padding = New Padding(10)
        advancedPanel.ColumnCount = 2
        advancedPanel.RowCount = 5
        advancedPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 135))
        advancedPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))

        AddRow(advancedPanel, 0, "API URL", txtApiBaseUrl)
        AddRow(advancedPanel, 1, "API Token", txtApiToken)
        AddRow(advancedPanel, 2, "Authorization Key", txtAuthorizationKey)
        AddRow(advancedPanel, 3, "Device Token", txtDeviceToken)
        AddRow(advancedPanel, 4, "Config File", lblConfigPath, 50)
        grpAdvanced.Controls.Add(advancedPanel)

        Dim btnPanel As New FlowLayoutPanel()
        btnPanel.FlowDirection = FlowDirection.RightToLeft
        btnPanel.Dock = DockStyle.Fill

        Dim btnSave As New Button()
        btnSave.Text = "Save"
        btnSave.Width = 95
        AddHandler btnSave.Click, AddressOf Save_Click

        Dim btnHealth As New Button()
        btnHealth.Text = "Test Connection"
        btnHealth.Width = 125
        AddHandler btnHealth.Click, AddressOf Health_Click

        Dim btnActivate As New Button()
        btnActivate.Text = "Activate PC"
        btnActivate.Width = 105
        AddHandler btnActivate.Click, AddressOf Activate_Click

        Dim btnImportActivate As New Button()
        btnImportActivate.Text = "Start Hybrid on This PC"
        btnImportActivate.Width = 175
        AddHandler btnImportActivate.Click, AddressOf ImportActivate_Click

        Dim btnDisable As New Button()
        btnDisable.Text = "Turn Off Hybrid"
        btnDisable.Width = 125
        AddHandler btnDisable.Click, AddressOf Disable_Click

        btnPanel.Controls.Add(btnSave)
        btnPanel.Controls.Add(btnDisable)
        btnPanel.Controls.Add(btnImportActivate)
        btnPanel.Controls.Add(btnHealth)

        root.Controls.Add(title, 0, 0)
        root.Controls.Add(hint, 0, 1)
        root.Controls.Add(txtSetupCode, 0, 2)
        root.Controls.Add(chkEnabled, 0, 3)
        root.Controls.Add(lblStatus, 0, 4)
        root.Controls.Add(grpAdvanced, 0, 5)
        root.Controls.Add(btnPanel, 0, 6)

        Me.Controls.Add(root)
    End Sub

    Private Sub AddRow(ByVal panel As TableLayoutPanel, ByVal row As Integer, ByVal caption As String, ByVal control As Control, Optional ByVal rowHeight As Integer = 42)
        Dim lbl As New Label()
        lbl.Text = caption
        lbl.Dock = DockStyle.Fill
        lbl.TextAlign = ContentAlignment.MiddleLeft
        control.Dock = DockStyle.Fill
        panel.RowStyles.Add(New RowStyle(SizeType.Absolute, rowHeight))
        panel.Controls.Add(lbl, 0, row)
        panel.Controls.Add(control, 1, row)
    End Sub

    Private Sub LoadSettings()
        HybridRuntime.ReloadSettings()
        Dim settings As HybridSettings = HybridRuntime.Settings
        chkEnabled.Checked = settings.Enabled
        txtApiBaseUrl.Text = settings.ApiBaseUrl
        txtApiToken.Text = settings.ApiToken
        txtAuthorizationKey.Text = settings.AuthorizationKey
        txtDeviceToken.Text = settings.DeviceToken
        lblStatus.Text = "Single-user mode stays unchanged when hybrid is disabled."
        lblConfigPath.Text = HybridSettings.ConfigPath()
    End Sub

    Private Function BuildSettingsFromUi() As HybridSettings
        Dim settings As New HybridSettings()
        settings.Enabled = chkEnabled.Checked
        settings.ApiBaseUrl = txtApiBaseUrl.Text.Trim()
        settings.ApiToken = txtApiToken.Text.Trim()
        settings.AuthorizationKey = txtAuthorizationKey.Text.Trim()
        settings.DeviceToken = txtDeviceToken.Text.Trim()
        settings.LastCompanyCode = HybridRuntime.Settings.LastCompanyCode
        Return settings
    End Function

    Private Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim settings As HybridSettings = BuildSettingsFromUi()
        If settings.Enabled Then
            If settings.ApiBaseUrl = "" OrElse settings.ApiToken = "" OrElse settings.AuthorizationKey = "" Then
                lblStatus.Text = "API URL, API Token, and Authorization Key are required when hybrid mode is enabled."
                Exit Sub
            End If
        End If
        settings.Save()
        HybridRuntime.ReloadSettings()
        lblStatus.Text = "Settings saved."
    End Sub

    Private Sub Disable_Click(ByVal sender As Object, ByVal e As EventArgs)
        chkEnabled.Checked = False
        Dim settings As HybridSettings = BuildSettingsFromUi()
        settings.Enabled = False
        settings.Save()
        HybridRuntime.ReloadSettings()
        lblStatus.Text = "Hybrid mode disabled. The app will use local SQLite mode."
    End Sub

    Private Sub Health_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If txtSetupCode.Text.Trim() <> "" AndAlso txtApiBaseUrl.Text.Trim() = "" Then
                ApplySetupCode()
            End If
            Dim settings As HybridSettings = BuildSettingsFromUi()
            If settings.ApiBaseUrl = "" Then
                lblStatus.Text = "Paste the setup code first, then click Test Connection."
                Exit Sub
            End If
            Dim client As New HybridApiClient(settings)
            Dim result As Dictionary(Of String, Object) = client.Health()
            lblStatus.Text = "Connection OK. " & Convert.ToString(result("message"))
        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub Activate_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim settings As HybridSettings = BuildSettingsFromUi()
            If settings.ApiBaseUrl = "" OrElse settings.ApiToken = "" OrElse settings.AuthorizationKey = "" Then
                lblStatus.Text = "API URL, API Token, and Authorization Key are required before activation."
                Exit Sub
            End If
            Dim client As New HybridApiClient(settings)
            Dim result As HybridDeviceActivationResponse = client.ActivateDevice(settings.AuthorizationKey)
            If result IsNot Nothing AndAlso result.Success AndAlso result.DeviceToken <> "" Then
                txtDeviceToken.Text = result.DeviceToken
                settings.DeviceToken = result.DeviceToken
                settings.Save()
                HybridRuntime.ReloadSettings()
            End If
            lblStatus.Text = If(result Is Nothing, "Activation failed.", result.Message)
        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub ImportActivate_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ApplySetupCode()
            chkEnabled.Checked = True
            Activate_Click(sender, e)
            If txtDeviceToken.Text.Trim() <> "" Then
                Dim settings As HybridSettings = BuildSettingsFromUi()
                settings.Enabled = True
                settings.Save()
                HybridRuntime.ReloadSettings()
                lblStatus.Text = "Done. This PC is activated for Hybrid multiuser mode. Restart Aadhat and login normally."
            End If
        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub ApplySetupCode()
        Dim setupCode As String = txtSetupCode.Text.Trim()
        If setupCode = "" Then
            Throw New ApplicationException("Paste the Desktop Setup Code first.")
        End If

        Dim serializer As New JavaScriptSerializer()
        Dim data As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(setupCode)
        If data Is Nothing Then
            Throw New ApplicationException("Invalid setup code.")
        End If

        txtApiBaseUrl.Text = SetupValue(data, "ApiBaseUrl", "api_base_url")
        txtApiToken.Text = SetupValue(data, "ApiToken", "api_token")
        txtAuthorizationKey.Text = SetupValue(data, "AuthorizationKey", "authorization_key")
        txtDeviceToken.Text = ""

        If txtApiBaseUrl.Text.Trim() = "" OrElse txtApiToken.Text.Trim() = "" OrElse txtAuthorizationKey.Text.Trim() = "" Then
            Throw New ApplicationException("Setup code must include API URL, API Token, and Authorization Key.")
        End If
    End Sub

    Private Function SetupValue(ByVal data As Dictionary(Of String, Object), ByVal pascalName As String, ByVal snakeName As String) As String
        If data.ContainsKey(pascalName) AndAlso data(pascalName) IsNot Nothing Then Return Convert.ToString(data(pascalName)).Trim()
        If data.ContainsKey(snakeName) AndAlso data(snakeName) IsNot Nothing Then Return Convert.ToString(data(snakeName)).Trim()
        Return ""
    End Function
End Class
