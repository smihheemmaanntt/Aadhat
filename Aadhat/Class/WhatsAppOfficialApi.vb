Imports System
Imports System.Data
Imports System.Collections.Generic
Imports Newtonsoft.Json.Linq

Public Class WhatsAppBusinessInfo
    Public VendorUid As String
    Public WabaId As String
    Public PhoneNumberId As String
    Public DisplayPhoneNumber As String
    Public VerifiedName As String
    Public QualityRating As String
    Public HealthStatus As String
    Public VendorExpiryDate As String
    Public RawResponse As String

    Public Function ToDisplayText() As String
        Dim text As String = "Status: Connected" & vbCrLf
        If DisplayPhoneNumber <> "" Then text &= "WhatsApp Number: " & DisplayPhoneNumber & vbCrLf
        If VerifiedName <> "" Then text &= "Business Name: " & VerifiedName & vbCrLf
        If QualityRating <> "" Then text &= "Quality Rating: " & QualityRating & vbCrLf
        If HealthStatus <> "" Then text &= "Health Status: " & HealthStatus & vbCrLf
        If VendorExpiryDate <> "" Then text &= "Vendor Expiry: " & VendorExpiryDate & vbCrLf
        If WabaId <> "" Then text &= "WABA ID: " & WabaId & vbCrLf
        If PhoneNumberId <> "" Then text &= "Phone Number ID: " & PhoneNumberId & vbCrLf
        If VendorUid <> "" Then text &= "Vendor ID: " & VendorUid
        Return text.Trim()
    End Function
End Class

Public Module WhatsAppOfficialApi
    'Public Const BaseUrl As String = "http://localhost/Aadhat_offical/wahsoft.in/public/api/"
    ' For server upload, keep the production BaseUrl enabled.
    Public Const BaseUrl As String = "https://wahsoft.in/public/api/"
    Public Function GetBusinessInfo(ByVal vendorUid As String, ByVal accessToken As String, ByRef info As WhatsAppBusinessInfo, ByRef errorMessage As String) As Boolean
        Try
            vendorUid = SafeTrim(vendorUid)
            accessToken = SafeTrim(accessToken)
            If vendorUid = "" Then
                errorMessage = "Vendor ID is blank."
                Return False
            End If
            If accessToken = "" Then
                errorMessage = "Access Token is blank."
                Return False
            End If

            Dim url As String = BaseUrl & vendorUid & "/whatsapp-business-info?token=" & Uri.EscapeDataString(accessToken)
            Dim responseString As String = WinHttpHelper.GetData(url)
            If SafeTrim(responseString) = "" Then
                errorMessage = "No response received from Official API server."
                Return False
            End If

            If IsJsonResponse(responseString) = False Then
                errorMessage = BuildNonJsonError(responseString)
                Return False
            End If

            Dim responseJson As JObject = JObject.Parse(responseString)
            If responseJson("result") Is Nothing OrElse responseJson("result").ToString().ToLower() <> "success" Then
                If responseJson("message") IsNot Nothing Then
                    errorMessage = responseJson("message").ToString()
                Else
                    errorMessage = responseString
                End If
                Return False
            End If

            info = ParseBusinessInfo(responseJson, vendorUid, responseString)
            Return True

        Catch ex As Exception
            errorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetServerTemplates(ByVal vendorUid As String, ByVal accessToken As String, ByVal syncMeta As Boolean, ByRef responseString As String, ByRef errorMessage As String) As Boolean
        Try
            vendorUid = SafeTrim(vendorUid)
            accessToken = SafeTrim(accessToken)
            If vendorUid = "" Or accessToken = "" Then
                errorMessage = "Vendor ID / Token missing"
                Return False
            End If
            Dim url As String = BaseUrl & vendorUid & "/whatsapp/templates?token=" & Uri.EscapeDataString(accessToken)
            If syncMeta Then url &= "&sync=1"
            url &= "&status=all"
            responseString = WinHttpHelper.GetData(url)
            If SafeTrim(responseString) = "" Then
                errorMessage = "No response received from Templates API server."
                Return False
            End If
            If IsJsonResponse(responseString) = False Then
                errorMessage = BuildNonJsonError(responseString)
                Return False
            End If
            Dim responseJson As JObject = JObject.Parse(responseString)
            If responseJson("result") IsNot Nothing AndAlso responseJson("result").ToString().ToLower() = "success" Then Return True
            If responseJson("message") IsNot Nothing Then errorMessage = responseJson("message").ToString() Else errorMessage = responseString
            Return False
        Catch ex As Exception
            errorMessage = ex.Message
            Return False
        End Try
    End Function

    Public Function GetMessageLogs(ByVal vendorUid As String, ByVal accessToken As String, ByVal fromDate As String, ByVal toDate As String, ByVal searchText As String, ByVal statusFilter As String, ByRef logsTable As DataTable, ByRef errorMessage As String) As Boolean
        Try
            logsTable = CreateMessageLogsTable()
            vendorUid = SafeTrim(vendorUid)
            accessToken = SafeTrim(accessToken)
            If vendorUid = "" Or accessToken = "" Then
                errorMessage = "Vendor ID / Token missing"
                Return False
            End If

            Dim url As String = BaseUrl & vendorUid & "/whatsapp/message-log?limit=200&token=" & Uri.EscapeDataString(accessToken)
            If SafeTrim(fromDate) <> "" Then url &= "&from_date=" & Uri.EscapeDataString(SafeTrim(fromDate))
            If SafeTrim(toDate) <> "" Then url &= "&to_date=" & Uri.EscapeDataString(SafeTrim(toDate))
            If SafeTrim(searchText) <> "" Then url &= "&search=" & Uri.EscapeDataString(SafeTrim(searchText))
            If SafeTrim(statusFilter) <> "" AndAlso SafeTrim(statusFilter).ToLower() <> "all" Then url &= "&status=" & Uri.EscapeDataString(SafeTrim(statusFilter))
            Dim responseString As String = WinHttpHelper.GetData(url)
            If SafeTrim(responseString) = "" Then
                errorMessage = "No response received from Message Log API server."
                Return False
            End If
            If IsJsonResponse(responseString) = False Then
                errorMessage = BuildNonJsonError(responseString)
                Return False
            End If

            Dim responseJson As JObject = JObject.Parse(responseString)
            If responseJson("result") Is Nothing OrElse responseJson("result").ToString().ToLower() <> "success" Then
                If responseJson("message") IsNot Nothing Then errorMessage = responseJson("message").ToString() Else errorMessage = responseString
                Return False
            End If

            Dim logsToken As JToken = Nothing
            If responseJson("data") IsNot Nothing Then logsToken = responseJson("data")("logs")
            If logsToken Is Nothing OrElse logsToken.Type <> JTokenType.Array Then Return True

            For Each logItem As JToken In CType(logsToken, JArray)
                If logItem Is Nothing OrElse logItem.Type <> JTokenType.Object Then Continue For
                Dim logObject As JObject = CType(logItem, JObject)
                logsTable.Rows.Add(
                    ReadString(logObject, "message_at_formatted"),
                    ReadString(logObject, "to_phone_number"),
                    ReadString(logObject, "template_name"),
                    ReadString(logObject, "template_language"),
                    ReadString(logObject, "status"),
                    ReadString(logObject, "response"),
                    ReadString(logObject, "approx_charge"),
                    ReadString(logObject, "wamid"),
                    ReadString(logObject, "message")
                )
            Next

            Return True
        Catch ex As Exception
            errorMessage = ex.Message
            Return False
        End Try
    End Function

    Private Function CreateMessageLogsTable() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("MessageTime")
        dt.Columns.Add("MobileNo")
        dt.Columns.Add("Template")
        dt.Columns.Add("Language")
        dt.Columns.Add("Status")
        dt.Columns.Add("Response")
        dt.Columns.Add("ApproxCharge")
        dt.Columns.Add("WAMID")
        dt.Columns.Add("Message")
        Return dt
    End Function

    Public Function UploadTemplateSample(ByVal vendorUid As String, ByVal accessToken As String, ByVal headerType As String, ByVal filePath As String, ByRef uploadedMediaFileName As String, ByRef apiResponse As String) As Boolean
        Try
            If SafeTrim(filePath) = "" OrElse System.IO.File.Exists(filePath) = False Then
                apiResponse = "Please select valid sample file."
                Return False
            End If
            Dim url As String = BaseUrl & SafeTrim(vendorUid) & "/whatsapp/templates/upload-sample?token=" & Uri.EscapeDataString(SafeTrim(accessToken))
            Dim payload As New JObject()
            payload("header_type") = New JValue(SafeTrim(headerType).ToLower())
            payload("file_name") = New JValue(System.IO.Path.GetFileName(filePath))
            payload("file_base64") = New JValue(Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath)))
            Dim responseString As String = WinHttpHelper.PostJson(url, payload)
            apiResponse = FormatDisplayMessage(responseString)
            If SafeTrim(responseString) = "" OrElse IsJsonResponse(responseString) = False Then Return False
            Dim responseJson As JObject = JObject.Parse(responseString)
            If responseJson("result") IsNot Nothing AndAlso responseJson("result").ToString().ToLower() = "success" Then
                If responseJson("data") IsNot Nothing AndAlso responseJson("data")("uploaded_media_file_name") IsNot Nothing Then
                    uploadedMediaFileName = responseJson("data")("uploaded_media_file_name").ToString()
                End If
                Return uploadedMediaFileName <> ""
            End If
            Return False
        Catch ex As Exception
            apiResponse = ex.Message
            Return False
        End Try
    End Function

    Public Function SubmitTemplate(ByVal vendorUid As String, ByVal accessToken As String, ByVal templateName As String, ByVal languageCode As String, ByVal category As String, ByVal headerType As String, ByVal bodyText As String, ByVal footerText As String, ByVal examples As String, ByVal uploadedMediaFileName As String, ByRef apiResponse As String, Optional ByVal buttonsJson As String = "") As Boolean
        Try
            Dim url As String = BaseUrl & SafeTrim(vendorUid) & "/whatsapp/templates/submit?token=" & Uri.EscapeDataString(SafeTrim(accessToken))
            Dim payload As New JObject()
            payload("template_name") = New JValue(SafeTrim(templateName).ToLower())
            payload("language_code") = New JValue(SafeTrim(languageCode))
            payload("category") = New JValue(If(SafeTrim(category) = "", "UTILITY", SafeTrim(category).ToUpper()))
            Dim normalizedHeaderType As String = SafeTrim(headerType).ToLower()
            If normalizedHeaderType = "none" OrElse normalizedHeaderType = "text" Then normalizedHeaderType = ""
            payload("header_type") = New JValue(normalizedHeaderType)
            payload("body") = New JValue(SafeTrim(bodyText))
            payload("footer") = New JValue(SafeTrim(footerText))
            payload("examples") = New JValue(SafeTrim(examples))
            If SafeTrim(uploadedMediaFileName) <> "" Then payload("uploaded_media_file_name") = New JValue(SafeTrim(uploadedMediaFileName))
            If SafeTrim(buttonsJson) <> "" Then
                Try
                    payload("message_buttons") = JArray.Parse(buttonsJson)
                Catch ex As Exception
                End Try
            End If
            Dim responseString As String = WinHttpHelper.PostJson(url, payload)
            apiResponse = FormatDisplayMessage(responseString)
            If SafeTrim(responseString) = "" OrElse IsJsonResponse(responseString) = False Then Return False
            Dim responseJson As JObject = JObject.Parse(responseString)
            Return responseJson("result") IsNot Nothing AndAlso responseJson("result").ToString().ToLower() = "success"
        Catch ex As Exception
            apiResponse = ex.Message
            Return False
        End Try
    End Function

    Public Function DeleteTemplate(ByVal vendorUid As String, ByVal accessToken As String, ByVal templateName As String, ByVal languageCode As String, ByRef apiResponse As String) As Boolean
        Try
            Dim url As String = BaseUrl & SafeTrim(vendorUid) & "/whatsapp/templates/delete?token=" & Uri.EscapeDataString(SafeTrim(accessToken))
            Dim payload As New JObject()
            payload("template_name") = New JValue(SafeTrim(templateName).ToLower())
            payload("language_code") = New JValue(SafeTrim(languageCode))
            Dim responseString As String = WinHttpHelper.PostJson(url, payload)
            apiResponse = FormatDisplayMessage(responseString)
            If SafeTrim(responseString) = "" OrElse IsJsonResponse(responseString) = False Then Return False
            Dim responseJson As JObject = JObject.Parse(responseString)
            Return responseJson("result") IsNot Nothing AndAlso responseJson("result").ToString().ToLower() = "success"
        Catch ex As Exception
            apiResponse = ex.Message
            Return False
        End Try
    End Function
    Public Function SendSmartMessage(ByVal vendorUid As String, ByVal accessToken As String, ByVal phoneNumber As String, ByVal messageBody As String, ByVal pdfUrl As String, ByVal templateName As String, ByVal templateLanguage As String, ByVal field1 As String, ByVal field2 As String, ByVal field3 As String, ByVal field4 As String, ByVal field5 As String, ByRef apiResponse As String, Optional ByVal documentName As String = "Bill.pdf", Optional ByVal bodyParamCount As Integer = -1) As Boolean
        Dim fields As New List(Of String)()
        fields.Add(field1)
        fields.Add(field2)
        fields.Add(field3)
        fields.Add(field4)
        fields.Add(field5)
        Return SendSmartMessage(vendorUid, accessToken, phoneNumber, messageBody, pdfUrl, templateName, templateLanguage, fields, apiResponse, documentName, bodyParamCount)
    End Function

    Public Function CleanDocumentName(ByVal documentName As String, Optional ByVal fallbackName As String = "Bill.pdf") As String
        Dim name As String = SafeTrim(documentName)
        Dim fallback As String = SafeTrim(fallbackName)
        If fallback = "" Then fallback = "Bill.pdf"
        If name = "" Then name = fallback
        name = name.Replace(vbCr, " ").Replace(vbLf, " ").Replace(vbTab, " ")
        name = System.Text.RegularExpressions.Regex.Replace(name, "[\\/:*?""<>|]", "")
        name = System.Text.RegularExpressions.Regex.Replace(name, "\s+", " ").Trim()
        If name = "" Then name = fallback
        If name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) = False Then name &= ".pdf"
        Return name
    End Function

    Public Function SendSmartMessage(ByVal vendorUid As String, ByVal accessToken As String, ByVal phoneNumber As String, ByVal messageBody As String, ByVal pdfUrl As String, ByVal templateName As String, ByVal templateLanguage As String, ByVal fields As List(Of String), ByRef apiResponse As String, Optional ByVal documentName As String = "Bill.pdf", Optional ByVal bodyParamCount As Integer = -1) As Boolean
        Try
            vendorUid = SafeTrim(vendorUid)
            accessToken = SafeTrim(accessToken)
            If vendorUid = "" Or accessToken = "" Then
                apiResponse = "Official API Vendor ID / Token missing"
                Return False
            End If

            If fields Is Nothing Then fields = New List(Of String)()
            Dim sendCount As Integer = fields.Count
            If bodyParamCount >= 0 Then sendCount = bodyParamCount
            If sendCount < 0 Then sendCount = 0

            Dim url As String = BaseUrl & vendorUid & "/contact/send-smart-message?token=" & Uri.EscapeDataString(accessToken)
            Dim payload As New JObject()
            payload("phone_number") = New JValue(NormalizeMobile(phoneNumber))
            payload("message_body") = New JValue(BuildRequiredMessageBody(messageBody, fields, templateName))
            payload("template_name") = New JValue(templateName)
            payload("template_language") = New JValue(NormalizeTemplateLanguage(templateLanguage))
            For i As Integer = 1 To sendCount
                Dim fieldValue As String = ""
                If fields.Count >= i Then fieldValue = fields(i - 1)
                If SafeTrim(fieldValue) = "" Then fieldValue = "-"
                payload("field_" & i.ToString()) = New JValue(fieldValue)
            Next
            payload("body_param_count") = New JValue(sendCount)

            If SafeTrim(pdfUrl) <> "" Then
                If IsTemplateSamplePdfUrl(pdfUrl) Then
                    apiResponse = "Sample PDF is only for template approval. Please generate/upload the real Aadhat PDF before sending."
                    WriteSendLog(vendorUid, payload, "", apiResponse)
                    Return False
                End If
                payload("header_document") = New JValue(SafeTrim(pdfUrl))
                payload("header_document_name") = New JValue(CleanDocumentName(documentName, "Bill.pdf"))
            End If

            Dim responseString As String = WinHttpHelper.PostJson(url, payload)
            If SafeTrim(responseString) = "" Then
                apiResponse = "No response from Official API"
                WriteSendLog(vendorUid, payload, responseString, apiResponse)
                Return False
            End If

            If IsJsonResponse(responseString) = False Then
                apiResponse = BuildNonJsonError(responseString)
                WriteSendLog(vendorUid, payload, responseString, apiResponse)
                Return False
            End If

            Dim responseJson As JObject = JObject.Parse(responseString)
            If responseJson("result") IsNot Nothing AndAlso responseJson("result").ToString().ToLower() = "success" Then
                apiResponse = "Successful"
                If responseJson("message") IsNot Nothing Then
                    apiResponse &= " - " & responseJson("message").ToString()
                End If
                Dim dataToken As JToken = responseJson("data")
                If dataToken IsNot Nothing AndAlso dataToken.Type = JTokenType.Object Then
                    Dim dataObject As JObject = CType(dataToken, JObject)
                    If dataObject("message_mode") IsNot Nothing Then
                        apiResponse &= " (" & dataObject("message_mode").ToString() & ")"
                    End If
                    If dataObject("status") IsNot Nothing Then
                        apiResponse &= " Status: " & dataObject("status").ToString()
                    End If
                End If
                WriteSendLog(vendorUid, payload, responseString, apiResponse)
                Return True
            End If

            apiResponse = ExtractApiErrorMessage(responseJson, responseString)
            apiResponse &= " | Template=" & templateName & ", Lang=" & NormalizeTemplateLanguage(templateLanguage)
            WriteSendLog(vendorUid, payload, responseString, apiResponse)
            Return False

        Catch ex As Exception
            apiResponse = ex.Message
            WriteSendLog(vendorUid, Nothing, ex.ToString(), apiResponse)
            Return False
        End Try
    End Function

    Private Function BuildRequiredMessageBody(ByVal messageBody As String, ByVal fields As List(Of String), ByVal templateName As String) As String
        Dim body As String = SafeTrim(messageBody)
        If body <> "" Then Return body
        If fields IsNot Nothing Then
            For Each fieldValue As String In fields
                If SafeTrim(fieldValue) <> "" Then Return SafeTrim(fieldValue)
            Next
        End If
        If SafeTrim(templateName) <> "" Then Return SafeTrim(templateName)
        Return "Bill"
    End Function

    Private Function IsTemplateSamplePdfUrl(ByVal pdfUrl As String) As Boolean
        Dim value As String = SafeTrim(pdfUrl).ToLower()
        Return value.Contains("pdfobject.com/pdf/sample.pdf")
    End Function

    Private Sub WriteSendLog(ByVal vendorUid As String, ByVal payload As JObject, ByVal rawResponse As String, ByVal parsedResponse As String)
        Try
            Dim logDir As String = ResolveOfficialApiLogDirectory()
            If logDir = "" Then Exit Sub

            Dim logFile As String = System.IO.Path.Combine(logDir, "print_bill_official_api_" & DateTime.Now.ToString("yyyyMMdd") & ".txt")
            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine("============================================================")
            sb.AppendLine("Time: " & DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"))
            sb.AppendLine("VendorUid: " & SecureCredentialStore.Mask(SafeTrim(vendorUid)))
            sb.AppendLine("Endpoint: " & BaseUrl & SecureCredentialStore.Mask(SafeTrim(vendorUid)) & "/contact/send-smart-message")
            If payload IsNot Nothing Then
                sb.AppendLine("Payload:")
                sb.AppendLine(payload.ToString(Newtonsoft.Json.Formatting.None))
            End If
            sb.AppendLine("Parsed Response:")
            sb.AppendLine(SafeTrim(parsedResponse))
            sb.AppendLine("Raw Response:")
            sb.AppendLine(SafeTrim(rawResponse))
            sb.AppendLine()
            System.IO.File.AppendAllText(logFile, sb.ToString())
        Catch ex As Exception
        End Try
    End Sub

    Private Function ResolveOfficialApiLogDirectory() As String
        Dim possibleDirs As New List(Of String)()

        Try
            possibleDirs.Add(System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "OfficialApiLogs"))
        Catch ex As Exception
        End Try

        Try
            Dim localAppData As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            If SafeTrim(localAppData) <> "" Then
                possibleDirs.Add(System.IO.Path.Combine(System.IO.Path.Combine(localAppData, "Aadhat"), "OfficialApiLogs"))
            End If
        Catch ex As Exception
        End Try

        Try
            Dim appData As String = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            If SafeTrim(appData) <> "" Then
                possibleDirs.Add(System.IO.Path.Combine(System.IO.Path.Combine(appData, "Aadhat"), "OfficialApiLogs"))
            End If
        Catch ex As Exception
        End Try

        For Each dirPath As String In possibleDirs
            Try
                If SafeTrim(dirPath) = "" Then Continue For
                If System.IO.Directory.Exists(dirPath) = False Then
                    System.IO.Directory.CreateDirectory(dirPath)
                End If

                Dim testFile As String = System.IO.Path.Combine(dirPath, "write_test.tmp")
                System.IO.File.WriteAllText(testFile, "ok")
                If System.IO.File.Exists(testFile) Then
                    System.IO.File.Delete(testFile)
                End If
                Return dirPath
            Catch ex As Exception
            End Try
        Next

        Return ""
    End Function

    Private Function ExtractApiErrorMessage(ByVal responseJson As JObject, ByVal rawResponse As String) As String
        Dim parts As New List(Of String)()

        If responseJson("message") IsNot Nothing Then parts.Add(responseJson("message").ToString())
        If responseJson("error") IsNot Nothing Then parts.Add(responseJson("error").ToString())
        If responseJson("errors") IsNot Nothing Then parts.Add("Errors: " & responseJson("errors").ToString(Newtonsoft.Json.Formatting.None))
        If responseJson("data") IsNot Nothing Then parts.Add("Data: " & responseJson("data").ToString(Newtonsoft.Json.Formatting.None))

        Dim messageText As String = If(parts.Count = 0, rawResponse, String.Join(" | ", parts.ToArray()))
        If messageText.Contains("#131005") OrElse messageText.ToLower().Contains("access denied") Then
            Return "Wahsoft API token is accepted, but WhatsApp Cloud send permission was denied. Please reconnect WhatsApp Cloud API setup in the Wahsoft panel, then sync templates again and send."
        End If
        If messageText.ToLower().Contains("you can not send message to your whatsapp api number") Then
            Return "You cannot send an Official API message to the same WhatsApp number that is connected as your business API number. Please send to a different customer number."
        End If
        Return messageText
    End Function

    Public Function FormatDisplayMessage(ByVal responseText As String) As String
        responseText = SafeTrim(responseText)
        If responseText = "" Then Return "No response message."
        If IsJsonResponse(responseText) = False Then Return NormalizeOfficialApiDisplayMessage(responseText)

        Try
            Dim responseJson As JObject = JObject.Parse(responseText)
            If responseJson("message") IsNot Nothing Then
                Return NormalizeOfficialApiDisplayMessage(responseJson("message").ToString())
            End If
            If responseJson("error") IsNot Nothing Then
                Return NormalizeOfficialApiDisplayMessage(responseJson("error").ToString())
            End If
        Catch ex As Exception
        End Try

        Return NormalizeOfficialApiDisplayMessage(responseText)
    End Function

    Public Function GetSendGridStatus(ByVal apiResponse As String, ByVal templateCode As String) As String
        Dim responseText As String = SafeTrim(apiResponse).ToLower()
        Dim codeText As String = SafeTrim(templateCode)

        If responseText.Contains("status: delivered") OrElse responseText.Contains("""status"":""delivered""") Then
            Return "Delivered via Official API (" & codeText & ")"
        End If

        If responseText.Contains("status: read") OrElse responseText.Contains("""status"":""read""") Then
            Return "Read on Official API (" & codeText & ")"
        End If

        If responseText.Contains("status: accepted") OrElse
           responseText.Contains("message processed") OrElse
           responseText.Contains("quality assessment") Then
            Return "Accepted by Official API (" & codeText & ")"
        End If

        Return "Sent via Official API (" & codeText & ")"
    End Function

    Private Function NormalizeOfficialApiDisplayMessage(ByVal messageText As String) As String
        messageText = SafeTrim(messageText)
        If messageText = "" Then Return "No response message."

        messageText = messageText.Replace("Link open nahi ho pa raha hai.", "The link could not be opened.")
        messageText = messageText.Replace("Template nahi mila", "Template not found")
        messageText = messageText.Replace("template nahi mila", "template not found")
        messageText = messageText.Replace("Message nahi gaya", "Message was not sent")
        messageText = messageText.Replace("message nahi gaya", "message was not sent")

        If messageText.Contains("#131005") OrElse messageText.ToLower().Contains("access denied") Then
            Return "Wahsoft API token is accepted, but WhatsApp Cloud send permission was denied. Please reconnect WhatsApp Cloud API setup in the Wahsoft panel, then sync templates again and send."
        End If

        If messageText.ToLower().Contains("you can not send message to your whatsapp api number") Then
            Return "You cannot send an Official API message to the same WhatsApp number that is connected as your business API number. Please send to a different customer number."
        End If

        Return messageText
    End Function

    Public Function NormalizeMobile(ByVal phoneNumber As String) As String
        Dim mobile As String = ""
        If phoneNumber Is Nothing Then Return mobile
        For Each ch As Char In phoneNumber
            If Char.IsDigit(ch) Then mobile &= ch
        Next
        If mobile.Length = 10 Then mobile = "91" & mobile
        Return mobile
    End Function

    Private Function NormalizeTemplateLanguage(ByVal languageCode As String) As String
        Dim value As String = SafeTrim(languageCode).ToLower()
        If value = "" Then Return "en_US"
        If value = "en" Then Return "en_US"
        Return SafeTrim(languageCode)
    End Function

    Private Function ParseBusinessInfo(ByVal responseJson As JObject, ByVal vendorUid As String, ByVal rawResponse As String) As WhatsAppBusinessInfo
        Dim info As New WhatsAppBusinessInfo()
        info.VendorUid = vendorUid
        info.RawResponse = rawResponse

        Dim dataToken As JToken = responseJson("data")
        If dataToken Is Nothing OrElse dataToken.Type <> JTokenType.Object Then Return info
        Dim data As JObject = CType(dataToken, JObject)

        info.WabaId = ReadString(data, "whatsapp_business_account_id")
        info.PhoneNumberId = ReadString(data, "current_phone_number_id")
        info.HealthStatus = ReadString(data, "health_status")
        info.VendorExpiryDate = ReadString(data, "vendor_expiry_date_formatted")

        Dim phoneToken As JToken = data("current_phone_number")
        If phoneToken IsNot Nothing AndAlso phoneToken.Type = JTokenType.Object Then
            Dim phone As JObject = CType(phoneToken, JObject)
            If info.PhoneNumberId = "" Then info.PhoneNumberId = ReadString(phone, "id")
            info.DisplayPhoneNumber = ReadString(phone, "display_phone_number")
            info.VerifiedName = ReadString(phone, "verified_name")
            info.QualityRating = ReadString(phone, "quality_rating")
        End If

        Return info
    End Function

    Private Function ReadString(ByVal obj As JObject, ByVal key As String) As String
        If obj Is Nothing Then Return ""
        If obj(key) Is Nothing Then Return ""
        Return obj(key).ToString()
    End Function

    Private Function IsJsonResponse(ByVal responseString As String) As Boolean
        Dim responseText As String = SafeTrim(responseString)
        Return responseText.StartsWith("{")
    End Function

    Private Function BuildNonJsonError(ByVal responseString As String) As String
        Dim responseText As String = SafeTrim(responseString)
        If responseText = "" Then Return "Blank response received from Official API server."
        If responseText.Length > 180 Then responseText = responseText.Substring(0, 180)
        responseText = responseText.Replace(vbCr, " ").Replace(vbLf, " ").Trim()
        Return "Official API returned HTML/text instead of JSON. Check Base URL, Apache route, or token. Response: " & responseText
    End Function

    Private Function SafeTrim(ByVal value As String) As String
        If value Is Nothing Then Return ""
        Return value.Trim()
    End Function
End Module

Public Module WhatsAppOfficialSendHelper
    Public Class OfficialTemplateComboItem
        Public TemplateCode As String = ""
        Public TemplateName As String = ""
        Public LanguageCode As String = ""

        Public Overrides Function ToString() As String
            If TemplateCode.Trim() = "" Then Return TemplateName
            Return TemplateName & " (" & TemplateCode & " / " & LanguageCode & ")"
        End Function
    End Class

    Public Sub LoadOfficialTemplateCombo(ByVal combo As System.Windows.Forms.ComboBox, ByVal label As System.Windows.Forms.Label, ByVal localType As String, ByVal preferredLanguage As String, Optional ByVal documentOnly As Boolean = True)
        If combo Is Nothing Then Exit Sub
        combo.Items.Clear()
        Dim languageCode As String = NormalizePreferredLanguage(preferredLanguage)
        AddOfficialTemplateComboRows(combo, localType, languageCode, documentOnly)
        If combo.Items.Count = 0 Then
            combo.Items.Add(New OfficialTemplateComboItem With {.TemplateName = "No approved " & localType.Replace("_", " ") & " template", .TemplateCode = "", .LanguageCode = languageCode})
        End If
        combo.SelectedIndex = 0
        combo.Visible = True
        If label IsNot Nothing Then label.Visible = True
    End Sub

    Public Function SelectedTemplateCode(ByVal combo As System.Windows.Forms.ComboBox) As String
        If combo Is Nothing Then Return ""
        Dim selectedItem As OfficialTemplateComboItem = TryCast(combo.SelectedItem, OfficialTemplateComboItem)
        If selectedItem Is Nothing Then Return ""
        Return selectedItem.TemplateCode
    End Function

    Public Sub SetTemplateComboVisible(ByVal combo As System.Windows.Forms.ComboBox, ByVal label As System.Windows.Forms.Label, ByVal visible As Boolean)
        If combo IsNot Nothing Then combo.Visible = visible
        If label IsNot Nothing Then label.Visible = visible
    End Sub

    Private Sub AddOfficialTemplateComboRows(ByVal combo As System.Windows.Forms.ComboBox, ByVal localType As String, ByVal languageCode As String, ByVal documentOnly As Boolean)
        For Each lookupType As String In TemplateLookupTypes(localType)
            Dim dt As DataTable = If(documentOnly, WhatsAppOfficialDb.GetApprovedDocumentTemplates(lookupType, languageCode), WhatsAppOfficialDb.GetApprovedTemplates(lookupType, languageCode))
            If dt Is Nothing Then Continue For
            For Each row As DataRow In dt.Rows
                Dim templateCode As String = row("TemplateCode").ToString()
                If TemplateComboContains(combo, templateCode) Then Continue For
                combo.Items.Add(New OfficialTemplateComboItem With {
                    .TemplateCode = templateCode,
                    .TemplateName = If(row.Table.Columns.Contains("TemplateName"), row("TemplateName").ToString(), templateCode),
                    .LanguageCode = row("LanguageCode").ToString()
                })
            Next
        Next
    End Sub

    Private Function TemplateComboContains(ByVal combo As System.Windows.Forms.ComboBox, ByVal templateCode As String) As Boolean
        For Each item As Object In combo.Items
            Dim templateItem As OfficialTemplateComboItem = TryCast(item, OfficialTemplateComboItem)
            If templateItem IsNot Nothing AndAlso templateItem.TemplateCode.Trim().Equals(templateCode.Trim(), StringComparison.OrdinalIgnoreCase) Then Return True
        Next
        Return False
    End Function

    Public Function SendAadhatDocument(ByVal localType As String, ByVal moduleName As String, ByVal mobileNo As String, ByVal accountName As String, ByVal entryDate As String, ByVal amount As String, ByVal pdfUrl As String, ByVal documentName As String, ByRef apiResponse As String, Optional ByVal preferredLanguage As String = "", Optional ByVal extraValue As String = "") As Boolean
        Return SendAadhatDocument(localType, moduleName, mobileNo, accountName, entryDate, amount, pdfUrl, documentName, apiResponse, preferredLanguage, extraValue, "")
    End Function

    Public Function SendAadhatDocument(ByVal localType As String, ByVal moduleName As String, ByVal mobileNo As String, ByVal accountName As String, ByVal entryDate As String, ByVal amount As String, ByVal pdfUrl As String, ByVal documentName As String, ByRef apiResponse As String, ByVal preferredLanguage As String, ByVal extraValue As String, ByVal selectedTemplateCode As String) As Boolean
        If AccentStorageHelper.IsOfficialApiAllowed() = False Then
            apiResponse = "Official API is not available in Trial Mode or inactive license."
            Return False
        End If
        WhatsAppOfficialDb.EnsureDatabase()
        Dim vendorUid As String = WhatsAppOfficialDb.GetSetting("VendorUid").Trim()
        Dim accessToken As String = WhatsAppOfficialDb.GetSetting("AccessToken").Trim()
        If vendorUid = "" OrElse accessToken = "" Then
            apiResponse = "Official API Vendor ID / Access Token is missing. Please validate and save WhatsApp API Configuration."
            Return False
        End If

        Dim mobile As String = WhatsAppOfficialApi.NormalizeMobile(mobileNo)
        If mobile = "" OrElse mobile.Length < 12 Then
            apiResponse = "Invalid mobile number"
            Return False
        End If

        If pdfUrl.Trim() <> "" AndAlso pdfUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase) = False Then
            apiResponse = "PDF upload failed: " & pdfUrl
            Return False
        End If

        Dim languageCode As String = NormalizePreferredLanguage(preferredLanguage)
        Dim templateRow As DataRow = FindApprovedTemplateRow(localType, languageCode, selectedTemplateCode)
        If templateRow Is Nothing Then
            apiResponse = "No approved " & localType.Replace("_", " ") & " template found. Please approve/sync a document template first."
            Return False
        End If

        Dim templateCode As String = templateRow("TemplateCode").ToString()
        Dim templateLanguage As String = templateRow("LanguageCode").ToString()
        Dim templateBody As String = If(templateRow.Table.Columns.Contains("BodyText"), templateRow("BodyText").ToString(), "")
        Dim parameterFields As String = If(templateRow.Table.Columns.Contains("ParameterFields"), templateRow("ParameterFields").ToString(), "")
        Dim templateParamCount As Integer = Val(templateRow("ParameterCount").ToString())
        Dim sendCount As Integer = MaxTemplateParameterIndex(templateBody)
        If sendCount <= 0 Then sendCount = templateParamCount
        If sendCount <= 0 Then sendCount = 1

        Dim fields As List(Of String) = BuildFields(localType, parameterFields, accountName, entryDate, amount, pdfUrl, extraValue, mobile)
        While fields.Count < sendCount
            fields.Add("-")
        End While
        If fields.Count > sendCount Then fields = fields.GetRange(0, sendCount)

        Dim serviceMessage As String = ExpandTemplateBody(templateBody, fields)
        If serviceMessage.Trim() = "" Then serviceMessage = accountName
        Return WhatsAppOfficialApi.SendSmartMessage(vendorUid, accessToken, mobile, serviceMessage, pdfUrl, templateCode, templateLanguage, fields, apiResponse, If(documentName.Trim() = "", DefaultDocumentName(localType), documentName), sendCount)
    End Function

    Public Function SendAadhatText(ByVal localType As String, ByVal moduleName As String, ByVal mobileNo As String, ByVal accountName As String, ByVal entryDate As String, ByVal amount As String, ByVal messageText As String, ByRef apiResponse As String, Optional ByVal preferredLanguage As String = "", Optional ByVal extraValue As String = "") As Boolean
        Return SendAadhatText(localType, moduleName, mobileNo, accountName, entryDate, amount, messageText, apiResponse, preferredLanguage, extraValue, "")
    End Function

    Public Function SendAadhatText(ByVal localType As String, ByVal moduleName As String, ByVal mobileNo As String, ByVal accountName As String, ByVal entryDate As String, ByVal amount As String, ByVal messageText As String, ByRef apiResponse As String, ByVal preferredLanguage As String, ByVal extraValue As String, ByVal selectedTemplateCode As String) As Boolean
        If AccentStorageHelper.IsOfficialApiAllowed() = False Then
            apiResponse = "Official API is not available in Trial Mode or inactive license."
            Return False
        End If
        WhatsAppOfficialDb.EnsureDatabase()
        Dim vendorUid As String = WhatsAppOfficialDb.GetSetting("VendorUid").Trim()
        Dim accessToken As String = WhatsAppOfficialDb.GetSetting("AccessToken").Trim()
        If vendorUid = "" OrElse accessToken = "" Then
            apiResponse = "Official API Vendor ID / Access Token is missing. Please validate and save WhatsApp API Configuration."
            Return False
        End If

        Dim mobile As String = WhatsAppOfficialApi.NormalizeMobile(mobileNo)
        If mobile = "" OrElse mobile.Length < 12 Then
            apiResponse = "Invalid mobile number"
            Return False
        End If

        Dim languageCode As String = NormalizePreferredLanguage(preferredLanguage)
        Dim templateRow As DataRow = FindApprovedAnyTemplateRow(localType, languageCode, selectedTemplateCode)
        If templateRow Is Nothing Then
            apiResponse = "No approved " & localType.Replace("_", " ") & " template found. Please approve/sync a template first."
            Return False
        End If

        Dim templateBody As String = If(templateRow.Table.Columns.Contains("BodyText"), templateRow("BodyText").ToString(), "")
        Dim parameterFields As String = If(templateRow.Table.Columns.Contains("ParameterFields"), templateRow("ParameterFields").ToString(), "")
        Dim sendCount As Integer = MaxTemplateParameterIndex(templateBody)
        If sendCount <= 0 Then sendCount = Val(templateRow("ParameterCount").ToString())
        If sendCount <= 0 Then sendCount = 1

        Dim fields As List(Of String) = BuildFields(localType, parameterFields, accountName, entryDate, amount, "", extraValue, mobile)
        While fields.Count < sendCount
            fields.Add("-")
        End While
        If fields.Count > sendCount Then fields = fields.GetRange(0, sendCount)

        Dim serviceMessage As String = If(messageText.Trim() <> "", messageText, ExpandTemplateBody(templateBody, fields))
        Return WhatsAppOfficialApi.SendSmartMessage(vendorUid, accessToken, mobile, serviceMessage, "", templateRow("TemplateCode").ToString(), templateRow("LanguageCode").ToString(), fields, apiResponse, "", sendCount)
    End Function

    Private Function FindApprovedTemplateRow(ByVal localType As String, ByVal languageCode As String, Optional ByVal selectedTemplateCode As String = "") As DataRow
        For Each lookupType As String In TemplateLookupTypes(localType)
            Dim dt As DataTable = WhatsAppOfficialDb.GetApprovedDocumentTemplates(lookupType, languageCode)
            Dim selectedRow As DataRow = FindTemplateCodeRow(dt, selectedTemplateCode)
            If selectedRow IsNot Nothing Then Return selectedRow
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
        Next
        Return Nothing
    End Function

    Private Function FindApprovedAnyTemplateRow(ByVal localType As String, ByVal languageCode As String, Optional ByVal selectedTemplateCode As String = "") As DataRow
        For Each lookupType As String In TemplateLookupTypes(localType)
            Dim dt As DataTable = WhatsAppOfficialDb.GetApprovedTemplates(lookupType, languageCode)
            Dim selectedRow As DataRow = FindTemplateCodeRow(dt, selectedTemplateCode)
            If selectedRow IsNot Nothing Then Return selectedRow
            If dt.Rows.Count > 0 Then Return dt.Rows(0)
        Next
        Return Nothing
    End Function

    Private Function FindTemplateCodeRow(ByVal dt As DataTable, ByVal selectedTemplateCode As String) As DataRow
        selectedTemplateCode = If(selectedTemplateCode, "").Trim().ToLower()
        If selectedTemplateCode = "" OrElse dt Is Nothing OrElse dt.Columns.Contains("TemplateCode") = False Then Return Nothing
        For Each row As DataRow In dt.Rows
            If row("TemplateCode").ToString().Trim().ToLower() = selectedTemplateCode Then Return row
        Next
        Return Nothing
    End Function

    Private Function TemplateLookupTypes(ByVal localType As String) As List(Of String)
        Dim result As New List(Of String)()
        Dim value As String = If(localType, "").Trim().ToLower()
        If value <> "" Then result.Add(value)
        Select Case value
            Case "purchase_register"
                If result.Contains("purchase") = False Then result.Add("purchase")
            Case "standard_sale_register", "super_sale_register", "loose_sale"
                If result.Contains("standard_sale") = False Then result.Add("standard_sale")
            Case "settle_ledger", "sub_ledger", "crate_ledger"
                If result.Contains("ledger") = False Then result.Add("ledger")
        End Select
        Return result
    End Function

    Private Function NormalizePreferredLanguage(ByVal preferredLanguage As String) As String
        Dim value As String = If(preferredLanguage, "").Trim().ToLower()
        If value.Contains("regional") OrElse value.Contains("hindi") OrElse value = "hi" OrElse value.StartsWith("gu") OrElse value.Contains("gujarati") OrElse value.Contains("gujrati") Then Return "hi"
        If value = "en" OrElse value.StartsWith("en_") OrElse value.Contains("english") Then Return "en"
        If value <> "" Then Return "hi"
        value = WhatsAppOfficialDb.GetSetting("LanguageType").Trim().ToLower()
        If value.Contains("regional") OrElse value.Contains("hindi") OrElse value = "hi" OrElse value.StartsWith("gu") OrElse value.Contains("gujarati") OrElse value.Contains("gujrati") Then Return "hi"
        Return "en"
    End Function

    Private Function BuildFields(ByVal localType As String, ByVal parameterFields As String, ByVal accountName As String, ByVal entryDate As String, ByVal amount As String, ByVal pdfUrl As String, ByVal extraValue As String, ByVal customerMobileNo As String) As List(Of String)
        If parameterFields.Trim() = "" Then parameterFields = DefaultFields(localType)
        Dim result As New List(Of String)()
        For Each rawField As String In parameterFields.Split(","c)
            Dim fieldName As String = NormalizeParameterFieldKey(rawField)
            Select Case fieldName
                Case "company_name", "firm_name"
                    result.Add(GetCompanyName())
                Case "company_other_name", "firm_other_name"
                    result.Add(GetCompanyOtherName())
                Case "company_hindi_name", "firm_hindi_name"
                    result.Add(GetCompanyOtherName())
                Case "account_name", "customer_name", "customer_account_name", "party_name"
                    result.Add(accountName)
                Case "account_other_name", "account_hindi_name", "party_other_name", "party_hindi_name", "customer_other_name", "customer_hindi_name"
                    result.Add(GetAccountOtherName(accountName))
                Case "item_name"
                    result.Add(GetItemNames(localType, accountName, entryDate))
                Case "item_other_name", "item_hindi_name"
                    result.Add(GetItemOtherNames(localType, accountName, entryDate))
                Case "customer_mobile_no", "mobile_no"
                    result.Add(customerMobileNo)
                Case "customer_city", "city"
                    result.Add("")
                Case "entry_date", "bill_date", "receipt_date", "payment_date", "purchase_date", "balance_date", "from_date"
                    result.Add(entryDate)
                Case "to_date"
                    result.Add(If(extraValue.Trim() <> "", extraValue, entryDate))
                Case "bill_total", "amount", "payment_amount", "receipt_amount", "balance_amount", "crate_qty", "nug"
                    result.Add(amount)
                Case "opening_balance"
                    result.Add(GetAccountBalance(accountName, entryDate, False))
                Case "closing_balance"
                    result.Add(GetAccountBalance(accountName, entryDate, True))
                Case "pdf_link"
                    result.Add(pdfUrl)
                Case "custom_message", "message_text", "extra_message", "own_message"
                    result.Add(extraValue)
                Case Else
                    result.Add(If(extraValue.Trim() <> "", extraValue, amount))
            End Select
        Next
        If result.Count = 0 Then
            result.Add(GetCompanyName())
            result.Add(accountName)
            result.Add(entryDate)
            result.Add(amount)
        End If
        Return result
    End Function

    Private Function NormalizeParameterFieldKey(ByVal fieldKey As String) As String
        Dim key As String = If(fieldKey, "").Trim().ToLower().Replace(" ", "_").Replace("/", "_")
        key = System.Text.RegularExpressions.Regex.Replace(key, "_+", "_")
        Select Case key
            Case "firm_name"
                Return "company_name"
            Case "firm_hindi_name", "hindi_company_name", "company_hindi_name", "firm_other_name"
                Return "company_other_name"
            Case "customer_name", "customer_account_name", "party_name"
                Return "account_name"
            Case "party_other_name", "customer_other_name"
                Return "account_other_name"
            Case "party_hindi_name", "customer_hindi_name", "account_hindi_name"
                Return "account_other_name"
            Case "item_hindi_name"
                Return "item_other_name"
            Case "mobile_no", "mobile", "whatsapp_no", "customer_mobile", "account_mobile"
                Return "customer_mobile_no"
            Case "city", "account_city"
                Return "customer_city"
            Case "total_amount", "sale_total"
                Return "bill_total"
            Case "receipt_amount", "payment_amount", "balance_amount"
                Return "amount"
            Case "message_text", "extra_message", "own_message"
                Return "custom_message"
        End Select
        Return key
    End Function

    Private Function DefaultFields(ByVal localType As String) As String
        Select Case localType.Trim().ToLower()
            Case "ledger", "settle_ledger", "sub_ledger", "crate_ledger"
                Return "company_name,account_name,from_date,to_date,company_other_name,account_other_name,opening_balance,closing_balance"
            Case "crate_in", "crate_out"
                Return "company_name,account_name,entry_date,crate_qty"
            Case Else
                Return "company_name,account_name,bill_date,bill_total,company_other_name,account_other_name,item_other_name,opening_balance,closing_balance"
        End Select
    End Function

    Private Function GetCompanyName() As String
        Dim name As String = clsFun.ExecScalarStr("Select CompanyName From Company Limit 1")
        If name.Trim() = "" Then name = "Aadhat"
        Return name
    End Function

    Private Function GetCompanyOtherName() As String
        Dim name As String = clsFun.ExecScalarStr("Select PrintOtherName From Company Limit 1")
        If name.Trim() = "" Then name = compnameHindi
        If name.Trim() = "" Then name = GetCompanyName()
        Return name
    End Function

    Private Function SqlText(ByVal value As String) As String
        Return If(value, "").Replace("'", "''")
    End Function

    Private Function GetAccountIdByName(ByVal accountName As String) As Integer
        accountName = If(accountName, "").Trim()
        If accountName = "" Then Return 0
        Return Val(clsFun.ExecScalarStr("Select ID From Accounts Where AccountName='" & SqlText(accountName) & "' Or OtherName='" & SqlText(accountName) & "' Limit 1"))
    End Function

    Private Function GetAccountOtherName(ByVal accountName As String) As String
        Dim accountId As Integer = GetAccountIdByName(accountName)
        Dim otherName As String = ""
        If accountId > 0 Then otherName = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & accountId)
        If otherName.Trim() = "" Then otherName = accountName
        Return otherName
    End Function

    Private Function GetItemOtherNames(ByVal localType As String, ByVal accountName As String, ByVal entryDate As String) As String
        Try
            Dim accountId As Integer = GetAccountIdByName(accountName)
            Dim sqlDate As String = CDate(entryDate).ToString("yyyy-MM-dd")
            Dim accountFilter As String = ""
            If accountId > 0 Then
                accountFilter = " And t.AccountID=" & accountId
            ElseIf If(accountName, "").Trim() <> "" Then
                accountFilter = " And t.AccountName='" & SqlText(accountName) & "'"
            End If
            If accountFilter = "" Then Return ""
            Return clsFun.ExecScalarStr("Select group_concat(Distinct i.OtherName) From Transaction2 t Inner Join Items i On t.ItemID=i.ID Where IfNull(i.OtherName,'')<>'' And t.EntryDate='" & sqlDate & "'" & accountFilter)
        Catch
            Return ""
        End Try
    End Function

    Private Function GetItemNames(ByVal localType As String, ByVal accountName As String, ByVal entryDate As String) As String
        Try
            Dim accountId As Integer = GetAccountIdByName(accountName)
            Dim sqlDate As String = CDate(entryDate).ToString("yyyy-MM-dd")
            Dim accountFilter As String = ""
            If accountId > 0 Then
                accountFilter = " And t.AccountID=" & accountId
            ElseIf If(accountName, "").Trim() <> "" Then
                accountFilter = " And t.AccountName='" & SqlText(accountName) & "'"
            End If
            If accountFilter = "" Then Return ""
            Return clsFun.ExecScalarStr("Select group_concat(Distinct i.ItemName) From Transaction2 t Inner Join Items i On t.ItemID=i.ID Where IfNull(i.ItemName,'')<>'' And t.EntryDate='" & sqlDate & "'" & accountFilter)
        Catch
            Return ""
        End Try
    End Function

    Private Function GetAccountBalance(ByVal accountName As String, ByVal entryDate As String, ByVal includeEntryDate As Boolean) As String
        Try
            Dim accountId As Integer = GetAccountIdByName(accountName)
            If accountId <= 0 Then Return ""
            Dim sqlDate As String = CDate(entryDate).ToString("yyyy-MM-dd")
            Dim op As String = If(includeEntryDate, "<=", "<")
            Dim sql As String = "Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate " & op & "'" & sqlDate & "')" & _
                            "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate " & op & "'" & sqlDate & "')) " & _
                            " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate " & op & "'" & sqlDate & "')" & _
                            " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate " & op & "'" & sqlDate & "'))  end),2) as Restbal from Accounts Where ID=" & accountId
            Dim balance As Decimal = Val(clsFun.ExecScalarStr(sql))
            If balance >= 0 Then Return Format(Math.Abs(balance), "0.00") & " Dr"
            Return Format(Math.Abs(balance), "0.00") & " Cr"
        Catch
            Return ""
        End Try
    End Function

    Private Function MaxTemplateParameterIndex(ByVal bodyText As String) As Integer
        Dim maxIndex As Integer = 0
        If bodyText Is Nothing Then Return maxIndex
        Dim matches As System.Text.RegularExpressions.MatchCollection = System.Text.RegularExpressions.Regex.Matches(bodyText, "\{\{(\d+)\}\}")
        For Each m As System.Text.RegularExpressions.Match In matches
            maxIndex = Math.Max(maxIndex, Val(m.Groups(1).Value))
        Next
        Return maxIndex
    End Function

    Private Function ExpandTemplateBody(ByVal bodyText As String, ByVal fields As List(Of String)) As String
        Dim text As String = If(bodyText, "")
        For i As Integer = 1 To fields.Count
            text = text.Replace("{{" & i.ToString() & "}}", fields(i - 1))
        Next
        Return text
    End Function

    Public Function DefaultDocumentName(ByVal localType As String) As String
        Select Case localType.Trim().ToLower()
            Case "ledger", "settle_ledger", "sub_ledger", "crate_ledger"
                Return "Ledger.pdf"
            Case "receipt"
                Return "Receipt.pdf"
            Case "payment"
                Return "Payment.pdf"
            Case "purchase", "purchase_register"
                Return "Purchase.pdf"
            Case "crate_in", "crate_out"
                Return "Crate.pdf"
            Case Else
                Return "Bill.pdf"
        End Select
    End Function
End Module
