Imports System.Net
Imports Newtonsoft.Json
Imports System.Collections.Specialized
Imports System.Text
Imports System.IO

Public Class HttpService
    Private Function PostSyncJson(Of T As Class)(ByVal endpoint As String, ByVal json As String, ByVal authToken As String) As T
        Try
            Using client As New WebClientWithTimeout()
                client.Timeout = 120000
                client.Encoding = Encoding.UTF8
                client.BaseAddress = baseAddress
                client.Headers(HttpRequestHeader.ContentType) = "application/json; charset=utf-8"
                If endpoint <> "Master/SaveCompany" AndAlso String.IsNullOrEmpty(authToken) Then
                    Throw New ApplicationException(endpoint & ": Login token is missing. Please sign in again.")
                End If
                If Not String.IsNullOrEmpty(authToken) Then
                    client.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
                End If
                Dim response As String = client.UploadString(endpoint, "POST", json)
                ValidateSyncResponse(endpoint, response)
                Dim result As T = JsonConvert.DeserializeObject(Of T)(response)
                If result Is Nothing Then Throw New ApplicationException(endpoint & ": Server returned an empty result.")
                Return result
            End Using
        Catch ex As WebException
            Throw BuildSyncException(endpoint, ex)
        Catch ex As Newtonsoft.Json.JsonReaderException
            Throw New ApplicationException(endpoint & ": Server returned an invalid JSON response.", ex)
        Catch ex As Newtonsoft.Json.JsonSerializationException
            Throw New ApplicationException(endpoint & ": Server response does not match the expected format.", ex)
        End Try
    End Function

    Private Sub ValidateSyncResponse(ByVal endpoint As String, ByVal response As String)
        Dim payload As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(response)
        For Each field As Newtonsoft.Json.Linq.JProperty In payload.Properties()
            Select Case field.Name.ToLowerInvariant()
                Case "status", "success", "isvalid", "isvaliduser"
                    Dim accepted As Boolean
                    If Boolean.TryParse(field.Value.ToString(), accepted) AndAlso Not accepted Then
                        Dim detail As String = "Server rejected the request."
                        For Each messageField As Newtonsoft.Json.Linq.JProperty In payload.Properties()
                            If String.Equals(messageField.Name, "message", StringComparison.OrdinalIgnoreCase) Then
                                detail = messageField.Value.ToString()
                            End If
                        Next
                        If IsBlankTableResponse(endpoint, detail) Then Return
                        Throw New ApplicationException(endpoint & ": " & detail)
                    End If
            End Select
        Next
    End Sub


    Private Function IsBlankTableResponse(ByVal endpoint As String, ByVal detail As String) As Boolean
        If String.IsNullOrEmpty(detail) Then Return False
        If endpoint Is Nothing Then endpoint = ""
        Dim endpointName As String = endpoint.ToLowerInvariant()
        Dim message As String = detail.ToLowerInvariant()
        If Not endpointName.Contains("save") Then Return False
        Return message.Contains("no ") AndAlso message.Contains(" found")
    End Function

    Private Sub MarkSkippedSyncResponse(ByVal resp As Response, ByVal message As String)
        If resp Is Nothing Then Exit Sub
        resp.IsValid = True
        resp.Status = True
        resp.Message = message
    End Sub
    Private Function BuildSyncException(ByVal endpoint As String, ByVal ex As WebException) As Exception
        Dim detail As String = ex.Message
        If ex.Response IsNot Nothing Then
            Try
                Using serverResponse As WebResponse = ex.Response
                    Using reader As New StreamReader(serverResponse.GetResponseStream(), Encoding.UTF8)
                        Dim responseText As String = reader.ReadToEnd()
                        If Not String.IsNullOrEmpty(responseText) Then
                            detail &= Environment.NewLine & responseText.Substring(0, Math.Min(responseText.Length, 4000))
                        End If
                    End Using
                End Using
            Catch
                ' Preserve the original HTTP error if the response body cannot be read.
            End Try
        End If
        Return New ApplicationException(endpoint & ": " & detail, ex)
    End Function

    Dim ClsCommon As CommonClass = New CommonClass()
    Const baseAddress As String = "http://147.93.107.113/api/"
    'Const baseAddress As String = "http://103.199.214.72:8080/api/"

    'Send company Must call at first everytime start sync(if new company)
    'Always call this function at first if new or old company, otherwise user will get un autherize response

    Public Function SendCompany(ByVal rqst As CompanyRequest) As CompanyResponse
        Return PostSyncJson(Of CompanyResponse)("Master/SaveCompany", JsonConvert.SerializeObject(rqst), "")
    End Function

    'Need to call this Authenticate first everytime start calling api to get the valid auth token against orgid and pwd

    Public Function Authenticate(organizationId As String, pwd As String) As LoginResponse
        Dim resp As LoginResponse = New LoginResponse()
        Try
            Using webClient As WebClient = New WebClient()
                webClient.Encoding = Encoding.UTF8
                webClient.BaseAddress = baseAddress
                Dim url = "User/LoginUser"
                'webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
                webClient.Headers(HttpRequestHeader.ContentType) = "application/x-www-form-urlencoded"
                Dim formData As New NameValueCollection()
                formData.Add("orgId", organizationId)
                formData.Add("password", pwd)
                formData.Add("deviceId", ClsCommon.GetMacAddress())
                formData.Add("deviceType", "windows")
                formData.Add("firebaseToken", "none")
                Dim response As String = Encoding.UTF8.GetString(webClient.UploadValues(url, formData))
                ValidateSyncResponse("User/LoginUser", response)
                resp = JsonConvert.DeserializeObject(Of LoginResponse)(response)
                If resp Is Nothing OrElse String.IsNullOrEmpty(resp.Token) Then Throw New ApplicationException("User/LoginUser: Server did not return a login token.")
                Return resp
            End Using
        Catch ex As WebException
            Throw BuildSyncException("User/LoginUser", ex)
        End Try
    End Function

    'Public Function SendAccountData(ByVal rqst As AddAccountRequest, Optional ByVal authToken As String = "") As SaveAccountResponse
    '    Dim resp As SaveAccountResponse = New SaveAccountResponse()
    '    Try
    '        Using webClient As WebClient = New WebClient()
    '            webClient.BaseAddress = baseAddress
    '            Dim url = "Master/SaveAccounts"
    '            'webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
    '            webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
    '            webClient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
    '            Dim AccountsData As String = JsonConvert.SerializeObject(rqst)
    '            Dim response = webClient.UploadString(url, AccountsData)
    '            resp = JsonConvert.DeserializeObject(Of SaveAccountResponse)(response)
    '            Return resp
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '        Mobile_App.btnCustom.Visible = True
    '    End Try
    'End Function
    Public Function SendAccountData(ByVal rqst As AddAccountRequest, Optional ByVal authToken As String = "") As SaveAccountResponse
        If rqst Is Nothing OrElse rqst.Accounts Is Nothing OrElse rqst.Accounts.Count = 0 Then
            Dim resp As New SaveAccountResponse()
            MarkSkippedSyncResponse(resp, "No accounts to sync.")
            Return resp
        End If
        Return PostSyncJson(Of SaveAccountResponse)("Master/SaveAccounts", JsonConvert.SerializeObject(rqst), authToken)
    End Function
    Public Function SendLedgerData(ByVal rqst As LedgerRequest, Optional ByVal authToken As String = "") As SaveLedgerResponse
        If rqst Is Nothing OrElse rqst.Ledgers Is Nothing OrElse rqst.Ledgers.Count = 0 Then
            Dim resp As New SaveLedgerResponse()
            MarkSkippedSyncResponse(resp, "No ledgers to sync.")
            Return resp
        End If
        Return PostSyncJson(Of SaveLedgerResponse)("Master/SaveLedgers", JsonConvert.SerializeObject(rqst), authToken)
    End Function

    'Public Function SendLedgerData(ByVal rqst As LedgerRequest) As SaveLedgerResponse
    '    Application.DoEvents()

    '    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
    '    Dim resp As SaveLedgerResponse = New SaveLedgerResponse()
    '    Try
    '        Using webClient As WebClient = New WebClient()
    '            webClient.BaseAddress = "http://103.199.214.72:8080/api/"
    '            'webClient.BaseAddress = "http://api.smicloud.in/api/"
    '            Dim url = "Master/SaveLedgers"
    '            'webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
    '            webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
    '            Dim data As String = JsonConvert.SerializeObject(rqst)
    '            Dim response = webClient.UploadString(url, data)
    '            resp = JsonConvert.DeserializeObject(Of SaveLedgerResponse)(response)
    '            Return resp
    '        End Using
    '    Catch ex As Exception
    '        Mobile_App.btnCustom.Visible = True
    '        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
    '        Throw ex
    '    End Try
    'End Function

    Public Function SendAccountGroup(ByVal rqst As AddAccountGroupRequest, Optional ByVal authToken As String = "") As AccountGroupResponse
        If rqst Is Nothing OrElse rqst.AccountGroups Is Nothing OrElse rqst.AccountGroups.Count = 0 Then
            Dim resp As New AccountGroupResponse()
            MarkSkippedSyncResponse(resp, "No account groups to sync.")
            Return resp
        End If
        Return PostSyncJson(Of AccountGroupResponse)("Master/SaveAccountGroups", JsonConvert.SerializeObject(rqst), authToken)
    End Function

    Public Function sendcratemarka(ByVal rqst As SaveCrateMarkaRequest, Optional ByVal authToken As String = "") As SaveCrateMarkaResponse
        If rqst Is Nothing OrElse rqst.CrateMarkas Is Nothing OrElse rqst.CrateMarkas.Count = 0 Then
            Dim resp As New SaveCrateMarkaResponse()
            MarkSkippedSyncResponse(resp, "No crate markas to sync.")
            Return resp
        End If
        Return PostSyncJson(Of SaveCrateMarkaResponse)("master/SaveCrateMarkas", JsonConvert.SerializeObject(rqst), authToken)
    End Function

    Public Function sendcrateVoucher(ByVal rqst As CrateVoucherRequest, Optional ByVal authToken As String = "") As SaveCrateVoucherResponse
        If rqst Is Nothing OrElse rqst.CrateVouchers Is Nothing OrElse rqst.CrateVouchers.Count = 0 Then
            Dim resp As New SaveCrateVoucherResponse()
            MarkSkippedSyncResponse(resp, "No crate vouchers to sync.")
            Return resp
        End If
        Return PostSyncJson(Of SaveCrateVoucherResponse)("master/SaveCrateVouchers", JsonConvert.SerializeObject(rqst), authToken)
    End Function

    Public Function UpdateLastDataSyncDateTime(ByVal authToken As String) As UpdateLastDataSyncDateTimeResponse
        Return PostSyncJson(Of UpdateLastDataSyncDateTimeResponse)("Master/UpdateLastDataSyncDateTime", String.Empty, authToken)
    End Function

    Public Function GetCrateVouchers(ByVal authToken As String) As List(Of CrateVoucher)
        Dim crateVouchers As New List(Of CrateVoucher)()
        Try
            Using webClient As New WebClient()
                webClient.Encoding = Encoding.UTF8
                webClient.BaseAddress = baseAddress
                Dim url = "Master/GetCrateVouchers"
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json; charset=utf-8"
                webClient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
                Dim response As String = webClient.DownloadString(url)
                ' API Response को Deseralize करें
                Dim apiResponse As ApiResponse = JsonConvert.DeserializeObject(Of ApiResponse)(response)
                If apiResponse IsNot Nothing AndAlso apiResponse.status Then
                    crateVouchers = apiResponse.crateVouchers
                End If
            End Using
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
        Return crateVouchers
    End Function

    Public Function GetVouchers(ByVal authToken As String) As List(Of Voucher)
        Dim vouchers As New List(Of Voucher)()
        Try
            Using webClient As New WebClient()
                webClient.Encoding = Encoding.UTF8
                webClient.BaseAddress = baseAddress
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json; charset=utf-8"
                webClient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken

                Dim response As String = webClient.DownloadString("Master/GetVouchers")
                Dim apiResponse As VoucherResponse = JsonConvert.DeserializeObject(Of VoucherResponse)(response)

                If apiResponse IsNot Nothing AndAlso apiResponse.status Then
                    vouchers = apiResponse.vouchers
                End If
            End Using
        Catch ex As Exception
            MsgBox("Error: " & ex.Message)
        End Try
        Return vouchers
    End Function

    Public Function DeleteCrateVoucher(ByVal authToken As String, ByVal voucherId As Integer) As CrateCancelResponse
        Try
            Using webclient As WebClient = New WebClient()
                webClient.Encoding = Encoding.UTF8
                webclient.BaseAddress = baseAddress
                Dim url = "master/DeleteCrateVoucher?Id in(" & voucherId & ")"
                'webclient.headers.add("user-agent", "mozilla/4.0 (compatible; msie 6.0; windows nt 5.2; .net clr 1.0.3705;)")
                webclient.Headers(HttpRequestHeader.ContentType) = "application/json; charset=utf-8"
                webclient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
                Dim data As String = JsonConvert.SerializeObject(rqst)
                Dim response = webclient.UploadString(url, data)
                resp = JsonConvert.DeserializeObject(Of VoucherCancelResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Throw ex
            Mobile_App.btnCustom.Visible = True
        End Try
    End Function

    Public Function DeleteVouchers(ByVal authToken As String, ByVal voucherIds As List(Of Integer)) As Boolean
        Try
            Using webclient As New WebClient()
                webClient.Encoding = Encoding.UTF8
                webclient.BaseAddress = "http://147.93.107.113/api/"
                Dim url As String = "Master/DeleteVouchers"

                webclient.Headers(HttpRequestHeader.ContentType) = "application/json; charset=utf-8"
                webclient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken

                ' Create JSON request body
                Dim requestData As String = JsonConvert.SerializeObject(voucherIds)

                ' Send POST request
                Dim response As String = webclient.UploadString(url, "POST", requestData)
                ' Deserialize response to Boolean (assuming API returns true/false)
                Dim result As Boolean = JsonConvert.DeserializeObject(Of Boolean)(response)
                Return result
            End Using
        Catch ex As Exception
            ' Handle exception and return false
            Return False
        End Try
    End Function

    Public Function DeleteCrateVouchers(ByVal authToken As String, ByVal voucherIds As List(Of Integer)) As Boolean
        Try
            Using webclient As New WebClient()
                webClient.Encoding = Encoding.UTF8
                webclient.BaseAddress = "http://147.93.107.113/api/"
                Dim url As String = "Master/DeleteCrateVouchers"

                webclient.Headers(HttpRequestHeader.ContentType) = "application/json; charset=utf-8"
                webclient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken

                ' Create JSON request body
                Dim requestData As String = JsonConvert.SerializeObject(voucherIds)

                ' Send POST request
                Dim response As String = webclient.UploadString(url, "POST", requestData)
                ' Deserialize response to Boolean (assuming API returns true/false)
                Dim result As Boolean = JsonConvert.DeserializeObject(Of Boolean)(response)
                Return result
            End Using
        Catch ex As Exception
            ' Handle exception and return false
            Return False
        End Try
    End Function


End Class


' Crate Voucher Model
Public Class CrateVoucher
    Public Property id As Integer
    Public Property slipNo As Integer
    Public Property entryDate As DateTime
    Public Property accountId As Integer
    Public Property accountName As String
    Public Property crateType As String
    Public Property crateId As Integer
    Public Property crateName As String
    Public Property qty As Integer
    Public Property remark As String
    Public Property rate As Decimal
    Public Property amount As Decimal
    Public Property cashPaid As Boolean
    Public Property orgID As Integer
    Public Property isCanceled As Boolean
    Public Property serverTag As Integer
End Class

' API Response Model
Public Class ApiResponse
    Public Property crateVouchers As List(Of CrateVoucher)
    Public Property isValid As Boolean
    Public Property status As Boolean
    Public Property message As String
    Public Property statusCode As Integer
End Class

' Voucher Model
Public Class Voucher
    Public Property id As Integer
    Public Property entryDate As DateTime
    Public Property transType As String
    Public Property mode As String
    Public Property accountId As Integer
    Public Property accountName As String
    Public Property receiptNo As Integer
    Public Property amount As Decimal
    Public Property discount As Decimal
    Public Property total As Decimal
    Public Property remark As String
    Public Property orgID As Integer
    Public Property isCanceled As Boolean
End Class

' Voucher API Response Model
Public Class VoucherResponse
    Public Property vouchers As List(Of Voucher)
    Public Property isValid As Boolean
    Public Property status As Boolean
    Public Property message As String
    Public Property statusCode As Integer
End Class

Public Class CrateCancelResponse
    Public Property CrateCancel As List(Of CrateCancelResponse) ' ✅ Fixed List Type
    Public Property isValid As Boolean
    Public Property status As Boolean
    Public Property message As String
    Public Property statusCode As Integer
End Class

Public Class VoucherCancelResponse
    Public Property CrateCancel As List(Of VoucherCancelResponse) ' ✅ Fixed List Type
    Public Property isValid As Boolean
    Public Property status As Boolean
    Public Property message As String
    Public Property statusCode As Integer
End Class


' Define the response class based on the expected API response
Public Class UpdateLastDataSyncDateTimeResponse
    Public Property Success As Boolean
    Public Property Message As String
    ' Add other properties as per the response
End Class

Public Class WebClientWithTimeout
    Inherits WebClient
    Public Property Timeout As Integer

    Public Sub New()
        Timeout = 60000 ' Default to 60 seconds
    End Sub

    Protected Overrides Function GetWebRequest(ByVal address As Uri) As WebRequest
        Dim request As WebRequest = MyBase.GetWebRequest(address)
        If request IsNot Nothing Then
            request.Timeout = Timeout
        End If
        Return request
    End Function
End Class



