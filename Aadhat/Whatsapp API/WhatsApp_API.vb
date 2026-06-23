Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Drawing
Imports System.Windows.Forms
Imports Newtonsoft.Json.Linq
Imports System.Diagnostics
Imports Newtonsoft.Json
Imports System.ComponentModel
Imports System.Collections.Generic
Public Class WhatsApp_API
    Dim ClsCommon As CommonClass
    ' Dim instance_id As String = "648E8D4BC315A"
    Dim access_token As String = "6687c047a58e1"
    Private Const MsgzLoginUrl As String = "https://msgz.in/login"
    Private Const OfficialWhatsAppLoginUrl As String = "https://wahsoft.in/public/auth/login"
    Private WithEvents timer As Timer
    Dim QRCodeStatus As String
    Private officialApiValidated As Boolean = False
    Private validatedOfficialVendorUid As String = ""
    Private Sub WhatsApp_API_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Function IsDesignerMode() As Boolean
        Return LicenseManager.UsageMode = LicenseUsageMode.Designtime OrElse Me.DesignMode
    End Function

    Private Sub EnsureCommon()
        If ClsCommon Is Nothing Then ClsCommon = New CommonClass()
    End Sub

    Private Sub OpenWebLink(ByVal url As String)
        Try
            Process.Start(url)
        Catch ex As Exception
            MsgBox("The link could not be opened." & vbCrLf & url & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Open Link")
        End Try
    End Sub

    Private Sub lnkMsgzLogin_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lnkMsgzLogin.LinkClicked
        OpenWebLink(MsgzLoginUrl)
    End Sub

    Private Sub lnkOfficialLogin_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lnkOfficialLogin.LinkClicked
        OpenWebLink(OfficialWhatsAppLoginUrl)
    End Sub

    Private Sub CopyTextBoxValue(ByVal sourceTextBox As TextBox)
        If sourceTextBox Is Nothing Then Exit Sub
        sourceTextBox.Focus()
        sourceTextBox.SelectAll()
        If sourceTextBox.Text.Trim() <> "" Then Clipboard.SetText(sourceTextBox.Text.Trim())
    End Sub

    Private Sub btnCopyVendorId_Click(sender As Object, e As EventArgs) Handles btnCopyVendorId.Click
        CopyTextBoxValue(TxtInstanceID)
    End Sub

    Private Sub btnCopyAccessToken_Click(sender As Object, e As EventArgs) Handles btnCopyAccessToken.Click
        CopyTextBoxValue(txtAccessToken)
    End Sub

    Private Sub WhatsApp_API_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If IsDesignerMode() Then Exit Sub
        EnsureCommon()
        Me.Top = 0 : Me.Left = 0 : Me.KeyPreview = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        cbLanguage.SelectedIndex = 0 : cbmsgType.SelectedIndex = 0 : cbMethod.SelectedIndex = 0
        ApplyTrialModeOfficialApiVisibility()
        cbDefaultSim.Items.Clear()
        cbDefaultSim.Text = ""
        EnsureBusinessInfoLabel()
        WhatsAppOfficialDb.EnsureDatabase()
        MigrateLegacyApiCredentials()
        If btnMessageLogs IsNot Nothing Then AddHandler btnMessageLogs.Click, AddressOf btnMessageLogs_Click
        FillControl()
        UpdateOfficialApiPanel()
        SilentValidateOfficialApiOnLoad()
        If ClsCommon.IsInternetConnect() = False Then Timer1.Stop() : Exit Sub
        ' If TxtInstanceID.Text.Trim <> "" Then ScanQRCode()
    End Sub

    Private Function IsTrialMode() As Boolean
        Return AccentStorageHelper.IsTrialMode()
    End Function

    Private Sub ApplyTrialModeOfficialApiVisibility()
        If cbMethod IsNot Nothing AndAlso IsTrialMode() Then
            If cbMethod.Items.Contains("WhatsApp Official API") Then cbMethod.Items.Remove("WhatsApp Official API")
            If cbMethod.Text.Trim() = "WhatsApp Official API" Then cbMethod.SelectedIndex = 0
        ElseIf cbMethod IsNot Nothing AndAlso cbMethod.Items.Contains("WhatsApp Official API") = False Then
            cbMethod.Items.Insert(Math.Min(1, cbMethod.Items.Count), "WhatsApp Official API")
        End If
    End Sub

    Private Function SendAPIRequest() As String
        EnsureCommon()
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Return ""
        Dim info As WhatsAppBusinessInfo = Nothing
        Dim errorMessage As String = ""
        If WhatsAppOfficialApi.GetBusinessInfo(TxtInstanceID.Text.Trim, txtAccessToken.Text.Trim, info, errorMessage) Then
            Return info.RawResponse
        End If
        Return ""
    End Function


    Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function

    Private Function SqlText(ByVal value As String) As String
        If value Is Nothing Then Return ""
        Return value.Trim().Replace("'", "''")
    End Function

    Private Function DecryptCredentialForUi(ByVal value As String, ByVal fieldTitle As String) As String
        Dim plainText As String = ""
        Dim errorMessage As String = ""
        If SecureCredentialStore.TryUnprotect(value, plainText, errorMessage) Then Return plainText
        MsgBox(errorMessage & vbCrLf & fieldTitle & " cleared.", MsgBoxStyle.Critical, "Official API")
        Return ""
    End Function

    Private Sub MigrateLegacyApiCredentials()
        Try
            Dim dt As DataTable = ClsFunPrimary.ExecDataTable("Select * From API")
            If dt.Rows.Count = 0 Then Exit Sub
            Dim updates As New List(Of String)()
            Dim row As DataRow = dt.Rows(0)
            If dt.Columns.Contains("InstanceID") Then
                Dim value As String = row("InstanceID").ToString()
                If value.Trim() <> "" AndAlso SecureCredentialStore.IsProtected(value) = False Then updates.Add("InstanceID='" & SqlText(SecureCredentialStore.Protect(value)) & "'")
            End If
            If dt.Columns.Contains("AccessToken") Then
                Dim value As String = row("AccessToken").ToString()
                If value.Trim() <> "" AndAlso SecureCredentialStore.IsProtected(value) = False Then updates.Add("AccessToken='" & SqlText(SecureCredentialStore.Protect(value)) & "'")
            End If
            If updates.Count > 0 Then ClsFunPrimary.ExecNonQuery("Update API Set " & String.Join(",", updates.ToArray()))
            dt.Dispose()
        Catch ex As Exception
        End Try
    End Sub


    ' Method to extract the instance ID from the API response JSON
    Private Function GetInstanceID(apiResponse As String) As String
        If apiResponse.Trim = "" Then Return ""
        Dim json As JObject = JObject.Parse(apiResponse)
        If json("result") IsNot Nothing AndAlso json("result").ToString().ToLower() = "success" Then
            Return TxtInstanceID.Text.Trim
        End If
        Return ""
    End Function

    Private Sub btnGetIntanceID_Click(sender As Object, e As EventArgs)
        EnsureCommon()
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Sub
        Dim apiResponse As String = SendAPIRequest()
        Dim instanceID As String = GetInstanceID(apiResponse)
        TxtInstanceID.Text = instanceID
        Dim sql As String = String.Empty
        sql = "Delete From API;Insert Into API(InstanceID,SendingMethod,LanguageType,SendingType) SELECT " & _
             "'" & SqlText(SecureCredentialStore.Protect(TxtInstanceID.Text)) & "','" & SqlText(cbMethod.Text) & "','" & SqlText(cbLanguage.Text) & "','" & SqlText(cbmsgType.Text) & "'"
        If ClsFunPrimary.ExecNonQuery(sql) > 0 Then FillControl()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub btnReconnect_Click(sender As Object, e As EventArgs) Handles btnReconnect.Click
        EnsureCommon()
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Sub
        If cbMethod.Text.Trim = "WhatsApp Official API" Then
            If IsTrialMode() Then
                MsgBox("Official API is not available in Trial Mode.", MsgBoxStyle.Information, "Official API")
                Exit Sub
            End If
            If ValidateOfficialApi(True) = False Then Exit Sub
        End If
        SaveDefault(False)
        '  If btnReconnect.Text = "Re-Connect" Then
        ' End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If cbMethod.Text.Trim = "WhatsApp Official API" Then
            If IsTrialMode() Then
                MsgBox("Official API is not available in Trial Mode.", MsgBoxStyle.Information, "Official API")
                Exit Sub
            End If
            If ValidateOfficialApi(False) = False Then Exit Sub
        End If
        SaveDefault()
    End Sub

    Private Sub btnVerifyMsgAccess_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVerifyMsgAccess.Click
        VerifyAndLoadSims()
    End Sub

    Private Sub btnClearOfficialApi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearOfficialApi.Click
        Dim confirm As DialogResult = MessageBox.Show("This will clear saved Official API Vendor ID and Access Token from this company. Continue?", "Clear Official API", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If confirm <> DialogResult.Yes Then Exit Sub

        TxtInstanceID.Clear()
        txtAccessToken.Clear()
        txtTestMobile.Clear()
        officialApiValidated = False
        validatedOfficialVendorUid = ""

        Try
            WhatsAppOfficialDb.ClearOfficialApiSettings()
        Catch ex As Exception
        End Try

        Try
            ClsFunPrimary.ExecNonQuery("Update API Set InstanceID='', AccessToken=''")
        Catch ex As Exception
        End Try

        ClearOfficialTemplateList()
        UpdateOfficialApiPanel()
        MsgBox("Official API credentials cleared.", MsgBoxStyle.Information, "Official API")
    End Sub

    Private Sub VerifyAndLoadSims()
        If txtMsgAccess.Text.Trim() = "" Then
            MsgBox("Please enter access token first.", MsgBoxStyle.Critical, "Mobile API")
            txtMsgAccess.Focus()
            Exit Sub
        End If

        Try
            If LoadDefaultSimCombo(True) = False Then Exit Sub
            cbDefaultSim.DroppedDown = True
        Catch ex As Exception
            MsgBox(WhatsAppOfficialApi.FormatDisplayMessage(ex.Message), MsgBoxStyle.Critical, "Mobile API")
        End Try
    End Sub

    Private Function LoadDefaultSimCombo(Optional ByVal showMessage As Boolean = False) As Boolean
        cbDefaultSim.Items.Clear()
        cbDefaultSim.Text = ""

        Dim simItems As List(Of String) = PhoneMSg.GetSimDisplayList(txtMsgAccess.Text.Trim(), False)
        If simItems Is Nothing OrElse simItems.Count = 0 Then
            If showMessage Then MsgBox("No device/SIM data found for this token.", MsgBoxStyle.Exclamation, "Mobile API")
            Return False
        End If

        For Each item In simItems
            cbDefaultSim.Items.Add(item)
        Next

        Dim savedDefaultSim As String = WhatsAppOfficialDb.GetSetting("DefaultSim")
        If savedDefaultSim.Trim() = "" Then savedDefaultSim = ClsFunPrimary.ExecScalarStr("Select DefaultSim From API")
        Dim selectedIndex As Integer = PhoneMSg.FindSimIndexBySubscriberId(savedDefaultSim, cbDefaultSim.Items)
        If selectedIndex >= 0 Then
            cbDefaultSim.SelectedIndex = selectedIndex
        ElseIf cbDefaultSim.Items.Count > 0 Then
            cbDefaultSim.SelectedIndex = 0
        End If

        Return cbDefaultSim.SelectedIndex >= 0
    End Function

    Private Sub cbMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbMethod.SelectedIndexChanged
        If IsTrialMode() AndAlso cbMethod.Text.Trim = "WhatsApp Official API" Then
            cbMethod.SelectedIndex = 0
            Exit Sub
        End If
        officialApiValidated = False
        validatedOfficialVendorUid = ""
        UpdateOfficialApiPanel()
    End Sub

    Private Sub OfficialCredential_TextChanged(sender As Object, e As EventArgs) Handles TxtInstanceID.TextChanged, txtAccessToken.TextChanged
        officialApiValidated = False
        validatedOfficialVendorUid = ""
        If lblBusinessInfo IsNot Nothing AndAlso Me.Visible Then
            lblBusinessInfo.ForeColor = Color.Maroon
            lblBusinessInfo.BackColor = Color.MistyRose
            lblBusinessInfo.Text = "Status: Not connected" & vbCrLf & "Official API credentials changed. Please validate again."
        End If
        ClearOfficialTemplateList()
        UpdateOfficialTemplateActions()
    End Sub

    Private Sub UpdateOfficialApiPanel()
        If cbMethod Is Nothing OrElse GbWhatsappAPI Is Nothing OrElse btnReconnect Is Nothing Then Exit Sub
        ApplyTrialModeOfficialApiVisibility()
        Dim isOfficial As Boolean = (cbMethod.Text.Trim = "WhatsApp Official API")
        If IsTrialMode() Then isOfficial = False
        GbWhatsappAPI.Visible = isOfficial
        If isOfficial Then
            btnReconnect.Text = "Validate Official API"
        Else
            btnReconnect.Text = "Whatsapp Connect With Facebook"
        End If
        UpdateOfficialTemplateActions()
    End Sub

    Private Sub UpdateOfficialTemplateActions()
        If IsDesignerMode() Then Exit Sub
        Dim isOfficial As Boolean = (cbMethod.Text.Trim = "WhatsApp Official API")
        If IsTrialMode() Then isOfficial = False
        Dim showActions As Boolean = (cbMethod.Text.Trim = "WhatsApp Official API" AndAlso officialApiValidated)
        If IsTrialMode() Then showActions = False
        If btnClearOfficialApi IsNot Nothing Then btnClearOfficialApi.Visible = isOfficial
        If btnSyncTemplates IsNot Nothing Then btnSyncTemplates.Visible = showActions
        If btnTemplateEditor IsNot Nothing Then btnTemplateEditor.Visible = showActions
        If btnMessageLogs IsNot Nothing Then btnMessageLogs.Visible = showActions
        If btnSendTest IsNot Nothing Then btnSendTest.Visible = showActions
        If txtTestMobile IsNot Nothing Then txtTestMobile.Visible = showActions
        If lblTestMobile IsNot Nothing Then lblTestMobile.Visible = showActions
        Dim showTemplateList As Boolean = showActions
        If dgvTemplates IsNot Nothing Then dgvTemplates.Visible = showTemplateList
        If lblTemplates IsNot Nothing Then lblTemplates.Visible = showTemplateList
        If tabTemplates IsNot Nothing Then tabTemplates.Visible = showTemplateList
    End Sub

    Private Sub ClearOfficialTemplateList()
        If dgvTemplates IsNot Nothing Then dgvTemplates.DataSource = Nothing
        If lblTemplates IsNot Nothing Then lblTemplates.Text = "Validate Official API to load templates"
        If tabLocalTemplates IsNot Nothing Then tabLocalTemplates.Text = "Local (0)"
        If tabApprovedTemplates IsNot Nothing Then tabApprovedTemplates.Text = "Approved (0)"
        If tabPendingTemplates IsNot Nothing Then tabPendingTemplates.Text = "Pending (0)"
        If tabRejectedTemplates IsNot Nothing Then tabRejectedTemplates.Text = "Rejected (0)"
    End Sub

    Private Sub SilentValidateOfficialApiOnLoad()
        If cbMethod.Text.Trim <> "WhatsApp Official API" Then
            UpdateOfficialTemplateActions()
            Exit Sub
        End If
        If TxtInstanceID.Text.Trim = "" OrElse txtAccessToken.Text.Trim = "" Then
            officialApiValidated = False
            validatedOfficialVendorUid = ""
            ClearOfficialTemplateList()
            UpdateOfficialTemplateActions()
            Exit Sub
        End If
        ValidateOfficialApi(False, True)
    End Sub

    Private Function ValidateOfficialApi(ByVal showSuccess As Boolean, Optional ByVal silent As Boolean = False) As Boolean
        Try
            If IsTrialMode() Then
                If silent = False Then MsgBox("Official API is not available in Trial Mode.", MsgBoxStyle.Information, "Official API")
                officialApiValidated = False
                validatedOfficialVendorUid = ""
                ClearOfficialTemplateList()
                UpdateOfficialTemplateActions()
                Return False
            End If
            If TxtInstanceID.Text.Trim = "" Then
                If silent = False Then
                    MsgBox("Please enter Vendor ID.", MsgBoxStyle.Critical, "Official API")
                    TxtInstanceID.Focus()
                End If
                Return False
            End If
            If txtAccessToken.Text.Trim = "" Then
                If silent = False Then
                    MsgBox("Please enter API Access Token.", MsgBoxStyle.Critical, "Official API")
                    txtAccessToken.Focus()
                End If
                Return False
            End If

            Dim info As WhatsAppBusinessInfo = Nothing
            Dim errorMessage As String = ""
            If WhatsAppOfficialApi.GetBusinessInfo(TxtInstanceID.Text.Trim, txtAccessToken.Text.Trim, info, errorMessage) Then
                officialApiValidated = True
                validatedOfficialVendorUid = TxtInstanceID.Text.Trim
                ShowBusinessInfo(info)
                If silent = False Then SaveDefault(False)
                LoadTemplatesGrid()
                UpdateOfficialTemplateActions()
                If showSuccess Then
                    MsgBox("Official API connected successfully.", MsgBoxStyle.Information, "Official API")
                End If
                Return True
            End If

            officialApiValidated = False
            validatedOfficialVendorUid = ""
            ClearOfficialTemplateList()
            UpdateOfficialTemplateActions()
            ShowBusinessError(errorMessage)
            If silent = False Then MsgBox("Official API validation failed." & vbCrLf & errorMessage, MsgBoxStyle.Critical, "Official API")
            Return False

        Catch ex As Exception
            officialApiValidated = False
            validatedOfficialVendorUid = ""
            ClearOfficialTemplateList()
            UpdateOfficialTemplateActions()
            ShowBusinessError(ex.Message)
            If silent = False Then MsgBox("Official API validation error." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Official API")
            Return False
        End Try
    End Function

    Private Sub EnsureBusinessInfoLabel()
        If IsDesignerMode() OrElse GbWhatsappAPI Is Nothing Then Exit Sub
        If lblBusinessInfo Is Nothing Then Exit Sub
        If lblBusinessInfo.Text.Trim = "" Then
            lblBusinessInfo.Text = "Status: Not connected" & vbCrLf & "Enter Vendor ID and Access Token, then click Validate Official API."
        End If
    End Sub
    Private Sub LoadTemplatesFromDbOrServer()
        If IsDesignerMode() Then Exit Sub
        LoadTemplatesGrid()
    End Sub


    Private Sub LoadTemplatesGrid()
        If IsDesignerMode() Then Exit Sub
        If dgvTemplates Is Nothing Then Exit Sub
        If officialApiValidated = False Then
            ClearOfficialTemplateList()
            UpdateOfficialTemplateActions()
            Exit Sub
        End If
        Try
            WhatsAppOfficialDb.EnsureTemplateCacheForVendor(TxtInstanceID.Text.Trim)
            Dim dt As DataTable = WhatsAppOfficialDb.GetTemplatesForDisplay()
            dt = FilterTemplatesForSelectedTab(dt)
            AddSerialNumbers(dt)
            If lblTemplates IsNot Nothing Then lblTemplates.Text = GetTemplateGridTitle(dt.Rows.Count)
            dgvTemplates.DataSource = dt
            If dgvTemplates.Columns.Contains("SNo") Then dgvTemplates.Columns("SNo").HeaderText = "SNo."
            If dgvTemplates.Columns.Contains("TemplateCode") Then dgvTemplates.Columns("TemplateCode").HeaderText = "Code"
            If dgvTemplates.Columns.Contains("TemplateName") Then dgvTemplates.Columns("TemplateName").HeaderText = "Template"
            If dgvTemplates.Columns.Contains("LanguageCode") Then dgvTemplates.Columns("LanguageCode").HeaderText = "Lang"
            If dgvTemplates.Columns.Contains("TemplateType") Then dgvTemplates.Columns("TemplateType").Visible = False
            If dgvTemplates.Columns.Contains("LocalTypeName") Then dgvTemplates.Columns("LocalTypeName").HeaderText = "Local Type"
            If dgvTemplates.Columns.Contains("Status") Then dgvTemplates.Columns("Status").HeaderText = "Meta Status"
            If dgvTemplates.Columns.Contains("FileSupport") Then dgvTemplates.Columns("FileSupport").HeaderText = "File"
            If dgvTemplates.Columns.Contains("ParameterCount") Then dgvTemplates.Columns("ParameterCount").HeaderText = "Params"
            If dgvTemplates.Columns.Contains("Description") Then dgvTemplates.Columns("Description").Visible = False
            If dgvTemplates.Columns.Contains("BodyText") Then dgvTemplates.Columns("BodyText").Visible = False
            If dgvTemplates.Columns.Contains("FooterText") Then dgvTemplates.Columns("FooterText").Visible = False
            If dgvTemplates.Columns.Contains("Examples") Then dgvTemplates.Columns("Examples").Visible = False
            If dgvTemplates.Columns.Contains("Category") Then dgvTemplates.Columns("Category").Visible = False
            If dgvTemplates.Columns.Contains("ButtonsJson") Then dgvTemplates.Columns("ButtonsJson").Visible = False
            If dgvTemplates.Columns.Contains("TemplateName") Then dgvTemplates.Columns("TemplateName").FillWeight = 135
            If dgvTemplates.Columns.Contains("Status") Then dgvTemplates.Columns("Status").FillWeight = 90
            ApplyTemplatesGridStyle()
        Catch ex As Exception
            If lblTemplates IsNot Nothing Then lblTemplates.Text = "Predefined Templates - " & ex.Message
        End Try
    End Sub

    Private Sub AddSerialNumbers(ByVal dt As DataTable)
        If dt Is Nothing Then Exit Sub
        If dt.Columns.Contains("SNo") = False Then dt.Columns.Add("SNo", GetType(Integer))
        For i As Integer = 0 To dt.Rows.Count - 1
            dt.Rows(i)("SNo") = i + 1
        Next
    End Sub

    Private Function FilterTemplatesForSelectedTab(ByVal source As DataTable) As DataTable
        If source Is Nothing Then Return New DataTable()
        If tabTemplates Is Nothing OrElse source.Columns.Contains("Status") = False Then Return source

        Dim filtered As DataTable = source.Clone()
        Dim wanted As String = "LOCAL"
        If tabTemplates.SelectedTab Is tabApprovedTemplates Then wanted = "APPROVED"
        If tabTemplates.SelectedTab Is tabPendingTemplates Then wanted = "PENDING"
        If tabTemplates.SelectedTab Is tabRejectedTemplates Then wanted = "REJECT"

        For Each row As DataRow In source.Rows
            Dim statusText As String = row("Status").ToString().ToUpper()
            Dim includeRow As Boolean = False
            If wanted = "LOCAL" Then
                includeRow = (statusText = "LOCAL" OrElse statusText.Trim() = "")
            ElseIf wanted = "APPROVED" Then
                includeRow = statusText.Contains("APPROVED")
            ElseIf wanted = "PENDING" Then
                includeRow = (statusText.Contains("PENDING") OrElse statusText.Contains("APPEAL"))
            ElseIf wanted = "REJECT" Then
                includeRow = (statusText.Contains("REJECT") OrElse statusText.Contains("FAILED"))
            End If
            If includeRow Then filtered.ImportRow(row)
        Next

        UpdateTemplateTabCounts(source)
        Return filtered
    End Function

    Private Sub UpdateTemplateTabCounts(ByVal source As DataTable)
        If tabTemplates Is Nothing OrElse source Is Nothing OrElse source.Columns.Contains("Status") = False Then Exit Sub
        Dim localCount As Integer = 0
        Dim approvedCount As Integer = 0
        Dim pendingCount As Integer = 0
        Dim rejectedCount As Integer = 0

        For Each row As DataRow In source.Rows
            Dim statusText As String = row("Status").ToString().ToUpper()
            If statusText = "LOCAL" OrElse statusText.Trim() = "" Then localCount += 1
            If statusText.Contains("APPROVED") Then approvedCount += 1
            If statusText.Contains("PENDING") OrElse statusText.Contains("APPEAL") Then pendingCount += 1
            If statusText.Contains("REJECT") OrElse statusText.Contains("FAILED") Then rejectedCount += 1
        Next

        If tabLocalTemplates IsNot Nothing Then tabLocalTemplates.Text = "Local (" & localCount & ")"
        If tabApprovedTemplates IsNot Nothing Then tabApprovedTemplates.Text = "Approved (" & approvedCount & ")"
        If tabPendingTemplates IsNot Nothing Then tabPendingTemplates.Text = "Pending (" & pendingCount & ")"
        If tabRejectedTemplates IsNot Nothing Then tabRejectedTemplates.Text = "Rejected (" & rejectedCount & ")"
    End Sub

    Private Function GetTemplateGridTitle(ByVal rowCount As Integer) As String
        If tabTemplates Is Nothing OrElse tabTemplates.SelectedTab Is Nothing Then Return "Predefined Templates"
        If tabTemplates.SelectedTab Is tabLocalTemplates Then Return "Local Predefined Templates - " & rowCount.ToString()
        If tabTemplates.SelectedTab Is tabApprovedTemplates Then Return "Meta Approved Templates - " & rowCount.ToString()
        If tabTemplates.SelectedTab Is tabPendingTemplates Then Return "Meta Pending Templates - " & rowCount.ToString()
        If tabTemplates.SelectedTab Is tabRejectedTemplates Then Return "Meta Rejected Templates - " & rowCount.ToString()
        Return "Predefined Templates - " & rowCount.ToString()
    End Function

    Private Sub tabTemplates_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tabTemplates.SelectedIndexChanged
        LoadTemplatesGrid()
    End Sub

    Private Sub LoadMessageLogsGrid()
        If IsDesignerMode() Then Exit Sub
        If dgvTemplates Is Nothing Then Exit Sub
        If ValidateOfficialApi(False) = False Then Exit Sub

        Try
            SaveDefault(False)
        Catch ex As Exception
        End Try

        Dim dt As DataTable = Nothing
        Dim errorMessage As String = ""
        If WhatsAppOfficialApi.GetMessageLogs(TxtInstanceID.Text.Trim, txtAccessToken.Text.Trim, "", "", "", "all", dt, errorMessage) = False Then
            MsgBox("Message logs could not be loaded." & vbCrLf & WhatsAppOfficialApi.FormatDisplayMessage(errorMessage), MsgBoxStyle.Critical, "Official API")
            Exit Sub
        End If

        dgvTemplates.DataSource = dt
        If lblTemplates IsNot Nothing Then lblTemplates.Text = "Official API Message Logs"
        If dgvTemplates.Columns.Contains("MessageTime") Then dgvTemplates.Columns("MessageTime").HeaderText = "Time"
        If dgvTemplates.Columns.Contains("MobileNo") Then dgvTemplates.Columns("MobileNo").HeaderText = "Mobile No"
        If dgvTemplates.Columns.Contains("Template") Then dgvTemplates.Columns("Template").HeaderText = "Template"
        If dgvTemplates.Columns.Contains("Language") Then dgvTemplates.Columns("Language").HeaderText = "Lang"
        If dgvTemplates.Columns.Contains("WAMID") Then dgvTemplates.Columns("WAMID").HeaderText = "WAMID"
        If dgvTemplates.Columns.Contains("Message") Then dgvTemplates.Columns("Message").Visible = False
        If dgvTemplates.Columns.Contains("MobileNo") Then dgvTemplates.Columns("MobileNo").FillWeight = 90
        If dgvTemplates.Columns.Contains("Template") Then dgvTemplates.Columns("Template").FillWeight = 100
        If dgvTemplates.Columns.Contains("Status") Then dgvTemplates.Columns("Status").FillWeight = 90
        If dgvTemplates.Columns.Contains("WAMID") Then dgvTemplates.Columns("WAMID").FillWeight = 140
        ApplyTemplatesGridStyle()
    End Sub

    Private Sub ApplyTemplatesGridStyle()
        If dgvTemplates Is Nothing Then Exit Sub
        dgvTemplates.BorderStyle = BorderStyle.FixedSingle
        dgvTemplates.CellBorderStyle = DataGridViewCellBorderStyle.Single
        dgvTemplates.GridColor = Color.Silver
        dgvTemplates.EnableHeadersVisualStyles = False
        dgvTemplates.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
        dgvTemplates.ColumnHeadersDefaultCellStyle.ForeColor = Color.Navy
        dgvTemplates.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 240, 248)
        dgvTemplates.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 105, 160)
        dgvTemplates.DefaultCellStyle.SelectionForeColor = Color.White
        dgvTemplates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        If dgvTemplates.Columns.Contains("SNo") Then dgvTemplates.Columns("SNo").Width = 42
        If dgvTemplates.Columns.Contains("TemplateCode") Then dgvTemplates.Columns("TemplateCode").Width = 70
        If dgvTemplates.Columns.Contains("TemplateName") Then dgvTemplates.Columns("TemplateName").Width = 108
        If dgvTemplates.Columns.Contains("LanguageCode") Then dgvTemplates.Columns("LanguageCode").Width = 45
        If dgvTemplates.Columns.Contains("LocalTypeName") Then dgvTemplates.Columns("LocalTypeName").Width = 92
        If dgvTemplates.Columns.Contains("ParameterCount") Then dgvTemplates.Columns("ParameterCount").Width = 55
        If dgvTemplates.Columns.Contains("HeaderType") Then dgvTemplates.Columns("HeaderType").Width = 65
        If dgvTemplates.Columns.Contains("Status") Then dgvTemplates.Columns("Status").Width = 78
        If dgvTemplates.Columns.Contains("FileSupport") Then dgvTemplates.Columns("FileSupport").Width = 50
        If dgvTemplates.Columns.Contains("ButtonsJson") Then dgvTemplates.Columns("ButtonsJson").Width = 80

        If dgvTemplates.Columns.Contains("Status") = False Then Exit Sub
        For Each row As DataGridViewRow In dgvTemplates.Rows
            If row.IsNewRow Then Continue For
            Dim statusText As String = ""
            If row.Cells("Status").Value IsNot Nothing Then statusText = row.Cells("Status").Value.ToString().ToUpper()

            If statusText.Contains("APPROVED") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(224, 245, 232)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 100, 45)
            ElseIf statusText.Contains("PENDING") OrElse statusText.Contains("APPEAL") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 215)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 85, 0)
            ElseIf statusText.Contains("REJECT") OrElse statusText.Contains("FAILED") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230)
                row.DefaultCellStyle.ForeColor = Color.Maroon
            Else
                row.DefaultCellStyle.BackColor = Color.FromArgb(242, 246, 252)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(45, 60, 85)
            End If
        Next
    End Sub
    Private Sub ShowBusinessInfo(ByVal info As WhatsAppBusinessInfo)
        EnsureBusinessInfoLabel()
        If lblBusinessInfo Is Nothing Then Exit Sub
        If info Is Nothing Then Exit Sub
        lblBusinessInfo.ForeColor = Color.Navy
        lblBusinessInfo.BackColor = Color.AliceBlue
        lblBusinessInfo.Text = info.ToDisplayText()
    End Sub

    Private Sub ShowBusinessError(ByVal errorMessage As String)
        EnsureBusinessInfoLabel()
        If lblBusinessInfo Is Nothing Then Exit Sub
        lblBusinessInfo.ForeColor = Color.Maroon
        lblBusinessInfo.BackColor = Color.MistyRose
        lblBusinessInfo.Text = "Status: Not connected" & vbCrLf & WhatsAppOfficialApi.FormatDisplayMessage(errorMessage)
    End Sub

    Private Sub btnSyncTemplates_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSyncTemplates.Click
        If ValidateOfficialApi(False) = False Then Exit Sub
        SaveDefault(False)
        Dim responseMessage As String = ""
        If WhatsAppOfficialDb.SyncMetaTemplatesFromApi(TxtInstanceID.Text.Trim, txtAccessToken.Text.Trim, responseMessage) Then
            WhatsAppOfficialDb.MarkTemplatesVendor(TxtInstanceID.Text.Trim)
            LoadTemplatesGrid()
            MsgBox(responseMessage, MsgBoxStyle.Information, "Official API")
        Else
            MsgBox(responseMessage, MsgBoxStyle.Critical, "Official API")
        End If
    End Sub

    Private Sub btnMessageLogs_Click(ByVal sender As Object, ByVal e As EventArgs)
        If ValidateOfficialApi(False) = False Then Exit Sub
        SaveDefault(False)
        Dim frm As New WhatsAppOfficialMessageLog()
        frm.VendorUid = TxtInstanceID.Text.Trim
        frm.AccessToken = txtAccessToken.Text.Trim
        frm.StartPosition = FormStartPosition.CenterScreen
        frm.ShowDialog()
    End Sub

    Private Sub btnSendTest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendTest.Click
        If txtTestMobile Is Nothing OrElse txtTestMobile.Text.Trim = "" Then
            MsgBox("Please enter test mobile number.", MsgBoxStyle.Critical, "Official API")
            txtTestMobile.Focus()
            Exit Sub
        End If
        If ValidateOfficialApi(False) = False Then Exit Sub
        SaveDefault(False)

        Dim senderApi As New WhatsAppSender()
        Dim sampleMessage As String = "Official WhatsApp API test message. Connection successful."
        If senderApi.SendOfficialSmartMessage(txtTestMobile.Text.Trim, sampleMessage, "") Then
            MsgBox("Test message sent successfully." & vbCrLf & senderApi.APIResposne, MsgBoxStyle.Information, "Official API")
        Else
            MsgBox("Test message failed." & vbCrLf & senderApi.APIResposne, MsgBoxStyle.Critical, "Official API")
        End If
    End Sub


    Private Sub btnTemplateEditor_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTemplateEditor.Click
        If officialApiValidated = False OrElse validatedOfficialVendorUid <> TxtInstanceID.Text.Trim Then
            If ValidateOfficialApi(False) = False Then Exit Sub
        End If
        Try
            SaveDefault(False)
        Catch ex As Exception
        End Try

        Dim frm As New WhatsAppTemplateEditor()
        frm.VendorUid = TxtInstanceID.Text.Trim
        frm.AccessToken = txtAccessToken.Text.Trim
        frm.StartPosition = FormStartPosition.CenterScreen
        AddHandler frm.FormClosed, AddressOf TemplateEditorClosed
        frm.Show()
        frm.Activate()
        frm.BringToFront()
    End Sub

    Private Sub TemplateEditorClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        LoadTemplatesGrid()
    End Sub

    Private Sub SaveDefault(Optional ByVal showMessage As Boolean = True)
        If IsTrialMode() AndAlso cbMethod.Text.Trim = "WhatsApp Official API" Then
            MsgBox("Official API is not available in Trial Mode.", MsgBoxStyle.Information, "Official API")
            cbMethod.SelectedIndex = 0
            Exit Sub
        End If
        Dim simValue As String = ""
        If txtMsgAccess.Text.Trim() <> "" AndAlso cbDefaultSim.SelectedIndex < 0 Then
            MsgBox("Verify token first and choose SIM.", MsgBoxStyle.Critical, "Mobile API")
            cbDefaultSim.Focus()
            Exit Sub
        End If
        If cbDefaultSim.SelectedIndex >= 0 Then
            simValue = PhoneMSg.ExtractSubscriberId(cbDefaultSim.Text).ToString()
        End If

        WhatsAppOfficialDb.SaveApiSettings(TxtInstanceID.Text, txtAccessToken.Text, cbMethod.Text.Trim, cbLanguage.Text.Trim, cbmsgType.Text.Trim, txtMsgAccess.Text.Trim, simValue)
        Dim Sql As String = String.Empty
        Sql = "Delete From API;Insert Into API(InstanceID,AccessToken,SendingMethod,LanguageType,SendingType,msg_Access_Token,defaultSim) SELECT " & _
            "'" & SqlText(SecureCredentialStore.Protect(TxtInstanceID.Text)) & "','" & SqlText(SecureCredentialStore.Protect(txtAccessToken.Text)) & "','" & SqlText(cbMethod.Text.Trim) & "','" & SqlText(cbLanguage.Text.Trim) & "','" & SqlText(cbmsgType.Text.Trim) & "','" & SqlText(txtMsgAccess.Text.Trim) & "','" & SqlText(simValue) & "'"
        If ClsFunPrimary.ExecNonQuery(Sql) > 0 AndAlso showMessage Then MsgBox("Sending Settings Updated For All Companies", MsgBoxStyle.Information, "Updated")
        If showMessage Then FillControl()
    End Sub
    Public Sub FillControl()
        ApplyTrialModeOfficialApiVisibility()
        Dim Sql As String = "Select * From API"
        Dim dt As New DataTable
        Dim loadedFromApiSettings As Boolean = False
        dt = WhatsAppOfficialDb.GetApiSettings()
        If dt.Rows.Count > 0 Then
            loadedFromApiSettings = True
        Else
            dt = ClsFunPrimary.ExecDataTable(Sql)
        End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    If loadedFromApiSettings AndAlso dt.Columns.Contains("VendorUid") Then
                        TxtInstanceID.Text = dt.Rows(i)("VendorUid").ToString()
                    ElseIf dt.Columns.Contains("InstanceID") Then
                        TxtInstanceID.Text = DecryptCredentialForUi(dt.Rows(i)("InstanceID").ToString(), "Vendor ID")
                    Else
                        TxtInstanceID.Text = ""
                    End If

                    If loadedFromApiSettings AndAlso dt.Columns.Contains("AccessToken") Then
                        txtAccessToken.Text = dt.Rows(i)("AccessToken").ToString()
                    ElseIf dt.Columns.Contains("AccessToken") Then
                        txtAccessToken.Text = DecryptCredentialForUi(dt.Rows(i)("AccessToken").ToString(), "Access Token")
                    Else
                        txtAccessToken.Text = ""
                    End If
                    Dim sendingMethod As String = dt.Rows(i)("SendingMethod").ToString()
                    If IsTrialMode() AndAlso sendingMethod = "WhatsApp Official API" Then sendingMethod = "Easy WhatsApp"
                    cbMethod.Text = sendingMethod
                    cbLanguage.Text = dt.Rows(i)("LanguageType").ToString()
                    cbmsgType.Text = dt.Rows(i)("SendingType").ToString()
                    If dt.Columns.Contains("MsgAccessToken") Then
                        txtMsgAccess.Text = dt.Rows(i)("MsgAccessToken").ToString()
                    Else
                        txtMsgAccess.Text = dt.Rows(i)("msg_Access_Token").ToString()
                    End If
                    Dim Defaultsim As String = dt.Rows(i)("DefaultSim").ToString()
                    If LoadDefaultSimCombo(False) = False AndAlso Defaultsim.Trim() <> "" Then
                        cbDefaultSim.Items.Add(PhoneMSg.GetSimDisplayText(Defaultsim.Trim()))
                        cbDefaultSim.SelectedIndex = 0
                    End If
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        UpdateOfficialApiPanel()
        'clsFun.CloseConnection()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs)
        SaveDefault()
    End Sub
End Class



