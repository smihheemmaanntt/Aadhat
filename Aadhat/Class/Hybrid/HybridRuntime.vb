Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Windows.Forms

Public Enum HybridCompanyMode
    LocalOnly = 0
    OnlineOnly = 1
    SyncingToOnline = 2
    SyncingToLocal = 3
    ReadOnlyArchive = 4
End Enum

Public Class HybridSettings
    Public Property Enabled As Boolean
    Public Property ApiBaseUrl As String
    Public Property ApiToken As String
    Public Property AuthorizationKey As String
    Public Property DeviceToken As String
    Public Property LastCompanyCode As String

    Public Shared Function ConfigPath() As String
        Return Path.Combine(Application.StartupPath, "hybrid.json")
    End Function

    Public Shared Function Load() As HybridSettings
        Dim settings As New HybridSettings()
        settings.Enabled = False
        settings.ApiBaseUrl = ""
        settings.ApiToken = ""
        settings.AuthorizationKey = ""
        settings.DeviceToken = ""
        settings.LastCompanyCode = ""

        Dim filePath As String = ConfigPath()
        If Not File.Exists(filePath) Then Return settings

        Try
            Dim serializer As New JavaScriptSerializer()
            Dim json As String = File.ReadAllText(filePath)
            Dim loaded As HybridSettings = serializer.Deserialize(Of HybridSettings)(json)
            If loaded IsNot Nothing Then Return loaded
        Catch ex As Exception
        End Try

        Return settings
    End Function

    Public Sub Save()
        Dim serializer As New JavaScriptSerializer()
        Dim json As String = serializer.Serialize(Me)
        File.WriteAllText(ConfigPath(), json)
    End Sub
End Class

Public Class HybridCompanyStatus
    Public Property Success As Boolean
    Public Property Message As String
    Public Property CompanyCode As String
    Public Property company_code As String
    Public Property CompanyName As String
    Public Property company_name As String
    Public Property Mode As String
    Public Property LocalDbPath As String
    Public Property local_db_path As String
    Public Property ServerTime As String
    Public Property server_time As String

    Public Sub ApplyJsonAliases()
        If CompanyCode = "" Then CompanyCode = If(company_code, "")
        If CompanyName = "" Then CompanyName = If(company_name, "")
        If LocalDbPath = "" Then LocalDbPath = If(local_db_path, "")
        If ServerTime = "" Then ServerTime = If(server_time, "")
    End Sub

    Public Function ParsedMode() As HybridCompanyMode
        Select Case (If(Mode, "")).Trim().ToUpperInvariant()
            Case "ONLINEONLY"
                Return HybridCompanyMode.OnlineOnly
            Case "SYNCINGTOONLINE"
                Return HybridCompanyMode.SyncingToOnline
            Case "SYNCINGTOLOCAL"
                Return HybridCompanyMode.SyncingToLocal
            Case "READONLYARCHIVE"
                Return HybridCompanyMode.ReadOnlyArchive
            Case Else
                Return HybridCompanyMode.LocalOnly
        End Select
    End Function
End Class

Public Class HybridRuntime
    Private Shared _settings As HybridSettings = HybridSettings.Load()
    Private Shared _companyStatus As HybridCompanyStatus = Nothing

    Public Shared ReadOnly Property Settings As HybridSettings
        Get
            Return _settings
        End Get
    End Property

    Public Shared ReadOnly Property IsEnabled As Boolean
        Get
            Return _settings IsNot Nothing AndAlso _settings.Enabled AndAlso _settings.ApiBaseUrl.Trim() <> ""
        End Get
    End Property

    Public Shared ReadOnly Property CurrentCompanyStatus As HybridCompanyStatus
        Get
            Return _companyStatus
        End Get
    End Property

    Public Shared Sub ReloadSettings()
        _settings = HybridSettings.Load()
        _companyStatus = Nothing
    End Sub

    Public Shared Function CheckCompanyStatus(ByVal companyCode As String, ByVal userName As String) As HybridCompanyStatus
        If Not IsEnabled Then
            Dim localStatus As New HybridCompanyStatus()
            localStatus.Success = True
            localStatus.Message = "Hybrid disabled. Local SQLite mode active."
            localStatus.CompanyCode = companyCode
            localStatus.CompanyName = compname
            localStatus.Mode = "LocalOnly"
            localStatus.LocalDbPath = GlobalData.ConnectionPath
            localStatus.ServerTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            _companyStatus = localStatus
            Return localStatus
        End If

        Dim client As New HybridApiClient(_settings)
        _companyStatus = client.GetCompanyStatus(companyCode, userName)
        Return _companyStatus
    End Function
End Class

Public Class HybridApiClient
    Private ReadOnly _settings As HybridSettings

    Public Sub New(ByVal settings As HybridSettings)
        _settings = settings
    End Sub

    Public Function GetCompanyStatus(ByVal companyCode As String, ByVal userName As String) As HybridCompanyStatus
        Dim data As New Dictionary(Of String, Object)()
        data("company_code") = companyCode
        data("user_name") = userName
        data("pc_name") = Environment.MachineName
        data("device_id") = GetDeviceId()
        data("authorization_key") = If(_settings.AuthorizationKey, "")
        data("device_token") = If(_settings.DeviceToken, "")
        Dim result As HybridCompanyStatus = Post(Of HybridCompanyStatus)("company/status", data)
        result.ApplyJsonAliases()
        Return result
    End Function

    Public Function ActivateDevice(ByVal authorizationKey As String) As HybridDeviceActivationResponse
        Dim data As New Dictionary(Of String, Object)()
        data("authorization_key") = authorizationKey
        data("device_id") = GetDeviceId()
        data("pc_name") = Environment.MachineName
        Dim result As HybridDeviceActivationResponse = Post(Of HybridDeviceActivationResponse)("device/activate", data)
        result.ApplyJsonAliases()
        Return result
    End Function

    Public Function Health() As Dictionary(Of String, Object)
        Return GetJson("health")
    End Function

    Private Function GetDeviceId() As String
        Dim boardId As String = AccentStorageHelper.GetMotherboardID()
        If boardId IsNot Nothing AndAlso boardId.Trim() <> "" Then Return boardId.Trim()
        Return Environment.MachineName.Trim().ToUpperInvariant()
    End Function

    Private Function GetJson(ByVal endpoint As String) As Dictionary(Of String, Object)
        Dim response As String = Send(endpoint, "GET", Nothing)
        Dim serializer As New JavaScriptSerializer()
        Return serializer.Deserialize(Of Dictionary(Of String, Object))(response)
    End Function

    Private Function Post(Of T)(ByVal endpoint As String, ByVal data As Object) As T
        Dim serializer As New JavaScriptSerializer()
        Dim payload As String = serializer.Serialize(data)
        Dim response As String = Send(endpoint, "POST", payload)
        Return serializer.Deserialize(Of T)(response)
    End Function

    Private Function Send(ByVal endpoint As String, ByVal method As String, ByVal payload As String) As String
        Dim baseUrl As String = _settings.ApiBaseUrl.Trim().TrimEnd("/"c)
        Dim url As String = baseUrl & "/api/" & endpoint.TrimStart("/"c)
        Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        request.Method = method
        request.ContentType = "application/json"
        request.Accept = "application/json"
        request.Timeout = 15000

        If _settings.ApiToken IsNot Nothing AndAlso _settings.ApiToken.Trim() <> "" Then
            request.Headers("Authorization") = "Bearer " & _settings.ApiToken.Trim()
        End If

        If payload IsNot Nothing Then
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(payload)
            request.ContentLength = bytes.Length
            Using stream = request.GetRequestStream()
                stream.Write(bytes, 0, bytes.Length)
            End Using
        End If

        Try
            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using stream As Stream = response.GetResponseStream()
                    Using reader As New StreamReader(stream)
                        Return reader.ReadToEnd()
                    End Using
                End Using
            End Using
        Catch ex As WebException
            Dim responseText As String = ""
            If ex.Response IsNot Nothing Then
                Using stream As Stream = ex.Response.GetResponseStream()
                    Using reader As New StreamReader(stream)
                        responseText = reader.ReadToEnd()
                    End Using
                End Using
            End If
            If responseText.Trim() <> "" Then
                Throw New ApplicationException(ExtractApiMessage(responseText), ex)
            End If
            Throw
        End Try
    End Function

    Private Function ExtractApiMessage(ByVal responseText As String) As String
        Try
            Dim serializer As New JavaScriptSerializer()
            Dim data As Dictionary(Of String, Object) = serializer.Deserialize(Of Dictionary(Of String, Object))(responseText)
            If data IsNot Nothing AndAlso data.ContainsKey("message") Then
                Return Convert.ToString(data("message"))
            End If
        Catch ex As Exception
        End Try
        Return responseText
    End Function
End Class

Public Class HybridDeviceActivationResponse
    Public Property Success As Boolean
    Public Property Message As String
    Public Property DeviceToken As String
    Public Property device_token As String
    Public Property MaxDevices As Integer
    Public Property max_devices As Integer
    Public Property ActiveDevices As Integer
    Public Property active_devices As Integer
    Public Property ServerTime As String
    Public Property server_time As String

    Public Sub ApplyJsonAliases()
        If DeviceToken = "" Then DeviceToken = If(device_token, "")
        If MaxDevices = 0 Then MaxDevices = max_devices
        If ActiveDevices = 0 Then ActiveDevices = active_devices
        If ServerTime = "" Then ServerTime = If(server_time, "")
    End Sub
End Class
