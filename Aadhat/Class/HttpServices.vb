Imports System.Net
Imports Newtonsoft.Json
Imports System.Collections.Specialized
Imports System.Text
Imports System.IO

Public Class HttpService
    Dim ClsCommon As CommonClass = New CommonClass()
    Const baseAddress As String = "http://147.93.107.113/api/"
    'Const baseAddress As String = "http://103.199.214.72:8080/api/"

    'Send company Must call at first everytime start sync(if new company)
    'Always call this function at first if new or old company, otherwise user will get un autherize response

    Public Function SendCompany(ByVal rqst As CompanyRequest) As CompanyResponse
        Dim resp As CompanyResponse = New CompanyResponse()
        Try
            Using webClient As WebClient = New WebClient()
                webClient.BaseAddress = baseAddress
                Dim url = "Master/SaveCompany"
                'webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
                Dim data As String = JsonConvert.SerializeObject(rqst)
                Dim response = webClient.UploadString(url, data)
                resp = JsonConvert.DeserializeObject(Of CompanyResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Throw ex
            Mobile_App.btnCustom.Visible = True
        End Try
    End Function

    'Need to call this Authenticate first everytime start calling api to get the valid auth token against orgid and pwd

    Public Function Authenticate(organizationId As String, pwd As String) As LoginResponse
        Dim resp As LoginResponse = New LoginResponse()
        Try
            Using webClient As WebClient = New WebClient()
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
                resp = JsonConvert.DeserializeObject(Of LoginResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Throw ex
            Mobile_App.btnCustom.Visible = True
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
        Dim resp As New SaveAccountResponse()
        Try
            Using webClient As New WebClient()
                webClient.BaseAddress = baseAddress
                Dim url As String = webClient.BaseAddress & "Master/SaveAccounts"

                ' Add necessary headers
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
                webClient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken

                ' Serialize object with proper formatting
                Dim AccountsData As String = JsonConvert.SerializeObject(rqst, Formatting.Indented)

                ' Debugging: Print JSON before sending (check for null/invalid values)
                Debug.WriteLine("Request JSON: " & AccountsData)

                ' Convert to bytes to avoid encoding issues
                Dim requestData As Byte() = Encoding.UTF8.GetBytes(AccountsData)

                ' Upload data using POST method
                Dim responseBytes As Byte() = webClient.UploadData(url, "POST", requestData)
                Dim responseString As String = Encoding.UTF8.GetString(responseBytes)

                ' Debugging: Print API response
                Debug.WriteLine("Response JSON: " & responseString)

                ' Deserialize response
                resp = JsonConvert.DeserializeObject(Of SaveAccountResponse)(responseString)
            End Using
        Catch webEx As WebException
            ' Capture detailed error response from the server
            Dim errorResponse As String = New StreamReader(webEx.Response.GetResponseStream()).ReadToEnd()
            Debug.WriteLine("Error Response: " & errorResponse)

            ' Log error message
            Debug.WriteLine("WebException: " & webEx.Message)

            ' Show UI button only if needed
            Mobile_App.btnCustom.Visible = True
        Catch ex As Exception
            Debug.WriteLine("General Exception: " & ex.Message)
        End Try

        Return resp
    End Function
    Public Function SendLedgerData(ByVal rqst As LedgerRequest, Optional ByVal authToken As String = "") As SaveLedgerResponse
        Application.DoEvents()

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
        Dim resp As SaveLedgerResponse = New SaveLedgerResponse()
        Try
            Using webClient As WebClientWithTimeout = New WebClientWithTimeout()
                webClient.Timeout = 120000 ' Set the timeout to 120 seconds (adjust as needed)
                webClient.BaseAddress = baseAddress
                Dim url = "Master/SaveLedgers"
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
                webClient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
                Dim data As String = JsonConvert.SerializeObject(rqst)
                Dim response = webClient.UploadString(url, data)
                resp = JsonConvert.DeserializeObject(Of SaveLedgerResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Mobile_App.btnCustom.Visible = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            Throw ex
        End Try
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
        Dim resp As AccountGroupResponse = New AccountGroupResponse()
        Try
            Using webClient As WebClient = New WebClient()
                webClient.BaseAddress = baseAddress
                Dim url = "Master/SaveAccountGroups"
                'webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)")
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
                webClient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
                Dim data As String = JsonConvert.SerializeObject(rqst)
                Dim response = webClient.UploadString(url, data)
                resp = JsonConvert.DeserializeObject(Of AccountGroupResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Throw ex
            Mobile_App.btnCustom.Visible = True '
        End Try
    End Function

    Public Function sendcratemarka(ByVal rqst As SaveCrateMarkaRequest, Optional ByVal authToken As String = "") As SaveCrateMarkaResponse
        Dim resp As SaveCrateMarkaResponse = New SaveCrateMarkaResponse()
        Try
            Using webclient As WebClient = New WebClient()
                webclient.BaseAddress = baseAddress
                Dim url = "master/SaveCrateMarkas"
                'webclient.headers.add("user-agent", "mozilla/4.0 (compatible; msie 6.0; windows nt 5.2; .net clr 1.0.3705;)")
                webclient.Headers(HttpRequestHeader.ContentType) = "application/json"
                webclient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
                Dim data As String = JsonConvert.SerializeObject(rqst)
                Dim response = webclient.UploadString(url, data)
                resp = JsonConvert.DeserializeObject(Of SaveCrateMarkaResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Throw ex

        End Try
    End Function

    Public Function sendcrateVoucher(ByVal rqst As CrateVoucherRequest, Optional ByVal authToken As String = "") As SaveCrateVoucherResponse
        Dim resp As SaveCrateVoucherResponse = New SaveCrateVoucherResponse()
        Try
            Using webclient As WebClient = New WebClient()
                webclient.BaseAddress = baseAddress
                Dim url = "master/SaveCrateVouchers"
                'webclient.headers.add("user-agent", "mozilla/4.0 (compatible; msie 6.0; windows nt 5.2; .net clr 1.0.3705;)")
                webclient.Headers(HttpRequestHeader.ContentType) = "application/json"
                webclient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken
                Dim data As String = JsonConvert.SerializeObject(rqst)
                Dim response = webclient.UploadString(url, data)
                resp = JsonConvert.DeserializeObject(Of SaveCrateVoucherResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Throw ex
            Mobile_App.btnCustom.Visible = True
        End Try
    End Function

    Public Function UpdateLastDataSyncDateTime(ByVal authToken As String) As UpdateLastDataSyncDateTimeResponse
        Dim resp As UpdateLastDataSyncDateTimeResponse = New UpdateLastDataSyncDateTimeResponse()
        Try
            Using webClient As WebClient = New WebClient()
                webClient.BaseAddress = baseAddress
                Dim url = "Master/UpdateLastDataSyncDateTime"
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
                webClient.Headers(HttpRequestHeader.Authorization) = "Bearer " & authToken

                ' Sending an empty POST request
                Dim response = webClient.UploadString(url, "POST", String.Empty)
                resp = JsonConvert.DeserializeObject(Of UpdateLastDataSyncDateTimeResponse)(response)
                Return resp
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetCrateVouchers(ByVal authToken As String) As List(Of CrateVoucher)
        Dim crateVouchers As New List(Of CrateVoucher)()
        Try
            Using webClient As New WebClient()
                webClient.BaseAddress = baseAddress
                Dim url = "Master/GetCrateVouchers"
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
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
                webClient.BaseAddress = baseAddress
                webClient.Headers(HttpRequestHeader.ContentType) = "application/json"
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
                webclient.BaseAddress = baseAddress
                Dim url = "master/DeleteCrateVoucher?Id in(" & voucherId & ")"
                'webclient.headers.add("user-agent", "mozilla/4.0 (compatible; msie 6.0; windows nt 5.2; .net clr 1.0.3705;)")
                webclient.Headers(HttpRequestHeader.ContentType) = "application/json"
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
                webclient.BaseAddress = "http://147.93.107.113/api/"
                Dim url As String = "Master/DeleteVouchers"

                webclient.Headers(HttpRequestHeader.ContentType) = "application/json"
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
                webclient.BaseAddress = "http://147.93.107.113/api/"
                Dim url As String = "Master/DeleteCrateVouchers"

                webclient.Headers(HttpRequestHeader.ContentType) = "application/json"
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


