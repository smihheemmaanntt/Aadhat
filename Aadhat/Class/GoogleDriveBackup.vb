Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Imports System.Security.Cryptography
Imports System.Diagnostics
Imports System.Net.Sockets

'Minimal Google Drive OAuth and upload implementation for the legacy .NET 3.5 client.
Public NotInheritable Class GoogleDriveBackup
    Private Sub New()
    End Sub

    Private Class OAuthData
        Public ClientId As String
        Public ClientSecret As String
        Public RefreshToken As String
    End Class

    Private Shared ReadOnly Entropy As Byte() = Encoding.UTF8.GetBytes("Aadhat.GoogleDriveBackup.v1")
    Private Shared ReadOnly Serializer As New JavaScriptSerializer()
    Private Shared ReadOnly CredentialPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aadhat\GoogleDriveOAuth.dat")

    Private Shared Function ReadCredentials() As OAuthData
        If Not File.Exists(CredentialPath) Then Return Nothing
        Dim encrypted As Byte() = Convert.FromBase64String(File.ReadAllText(CredentialPath, Encoding.UTF8))
        Dim clear As Byte() = ProtectedData.Unprotect(encrypted, Entropy, DataProtectionScope.CurrentUser)
        Dim values As Dictionary(Of String, Object) = Serializer.Deserialize(Of Dictionary(Of String, Object))(Encoding.UTF8.GetString(clear))
        Dim result As New OAuthData()
        If values.ContainsKey("ClientId") Then result.ClientId = Convert.ToString(values("ClientId"))
        If values.ContainsKey("ClientSecret") Then result.ClientSecret = Convert.ToString(values("ClientSecret"))
        If values.ContainsKey("RefreshToken") Then result.RefreshToken = Convert.ToString(values("RefreshToken"))
        Return result
    End Function

    Private Shared Sub SaveCredentials(ByVal data As OAuthData)
        Dim values As New Dictionary(Of String, Object)()
        values("ClientId") = data.ClientId : values("ClientSecret") = data.ClientSecret : values("RefreshToken") = data.RefreshToken
        Directory.CreateDirectory(Path.GetDirectoryName(CredentialPath))
        Dim clear As Byte() = Encoding.UTF8.GetBytes(Serializer.Serialize(values))
        File.WriteAllText(CredentialPath, Convert.ToBase64String(ProtectedData.Protect(clear, Entropy, DataProtectionScope.CurrentUser)), Encoding.UTF8)
    End Sub

    Public Shared Function IsConnected() As Boolean
        Try
            Dim data As OAuthData = ReadCredentials()
            Return data IsNot Nothing AndAlso Not String.IsNullOrEmpty(data.RefreshToken)
        Catch
            Return False
        End Try
    End Function

    Public Shared Sub ImportClientCredentials(ByVal jsonPath As String)
        If Not File.Exists(jsonPath) Then Throw New FileNotFoundException("OAuth JSON file not found.")
        Dim root As Dictionary(Of String, Object) = Serializer.Deserialize(Of Dictionary(Of String, Object))(File.ReadAllText(jsonPath, Encoding.UTF8))
        Dim section As Object = Nothing
        If Not root.TryGetValue("installed", section) Then
            If Not root.TryGetValue("web", section) Then Throw New InvalidDataException("This is not a Google OAuth client JSON. Download the Desktop app client JSON from Google Cloud Console.")
        End If
        Dim client As Dictionary(Of String, Object) = DirectCast(section, Dictionary(Of String, Object))
        Dim id As String = Convert.ToString(client("client_id"))
        Dim secret As String = Convert.ToString(client("client_secret"))
        If String.IsNullOrEmpty(id) OrElse String.IsNullOrEmpty(secret) Then Throw New InvalidDataException("OAuth JSON is missing the client ID or client secret.")
        Dim old As OAuthData = ReadCredentials()
        SaveCredentials(New OAuthData With {.ClientId = id, .ClientSecret = secret, .RefreshToken = If(old Is Nothing, "", old.RefreshToken)})
    End Sub

    Public Shared Sub ConnectAccount(ByVal email As String)
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) Or SecurityProtocolType.Tls
        Dim data As OAuthData = ReadCredentials()
        If data Is Nothing OrElse String.IsNullOrEmpty(data.ClientId) Then Throw New InvalidOperationException("First click Import JSON and select the Desktop OAuth client JSON file.")
        Dim port As Integer
        Dim probe As New TcpListener(System.Net.IPAddress.Loopback, 0)
        probe.Start()
        port = DirectCast(probe.LocalEndpoint, System.Net.IPEndPoint).Port
        probe.Stop()
        Dim redirect As String = "http://127.0.0.1:" & port.ToString() & "/"
        Dim state As String = Guid.NewGuid().ToString("N")
        Dim authUrl As String = "https://accounts.google.com/o/oauth2/v2/auth?client_id=" & Uri.EscapeDataString(data.ClientId) & _
            "&redirect_uri=" & Uri.EscapeDataString(redirect) & "&response_type=code&scope=" & Uri.EscapeDataString("https://www.googleapis.com/auth/drive.file") & _
            "&access_type=offline&prompt=consent&state=" & state
        If Not String.IsNullOrEmpty(email) Then authUrl &= "&login_hint=" & Uri.EscapeDataString(email)
        Dim listener As New TcpListener(System.Net.IPAddress.Loopback, port)
        listener.Start()
        Try
            Process.Start(authUrl)
            Using client As TcpClient = listener.AcceptTcpClient()
                Dim network As NetworkStream = client.GetStream()
                Dim reader As New StreamReader(network, Encoding.ASCII)
                Dim requestLine As String = reader.ReadLine()
                Dim headersDone As Boolean = False
                While Not headersDone
                    Dim header As String = reader.ReadLine()
                    If String.IsNullOrEmpty(header) Then headersDone = True
                End While
                Dim callback As New Uri("http://127.0.0.1" & requestLine.Split(" "c)(1))
                Dim returnedState As String = QueryValue(callback.Query, "state")
                Dim code As String = QueryValue(callback.Query, "code")
                Dim html As Byte() = Encoding.UTF8.GetBytes("<html><body>Google Drive connected. You can return to Aadhat.</body></html>")
                Dim headerBytes As Byte() = Encoding.ASCII.GetBytes("HTTP/1.1 200 OK" & vbCrLf & "Content-Type: text/html; charset=utf-8" & vbCrLf & "Content-Length: " & html.Length.ToString() & vbCrLf & "Connection: close" & vbCrLf & vbCrLf)
                network.Write(headerBytes, 0, headerBytes.Length) : network.Write(html, 0, html.Length) : network.Flush()
                network.Close()
                If returnedState <> state Then Throw New InvalidOperationException("Google authorization state check failed. Please try Connect Google again.")
                If String.IsNullOrEmpty(code) Then Throw New InvalidOperationException("Google did not return an authorization code. Check that the account is allowed in Google OAuth test users.")
                Dim tokenResponse As Dictionary(Of String, Object) = PostForm("https://oauth2.googleapis.com/token", "code=" & Uri.EscapeDataString(code) & _
                    "&client_id=" & Uri.EscapeDataString(data.ClientId) & "&client_secret=" & Uri.EscapeDataString(data.ClientSecret) & _
                    "&redirect_uri=" & Uri.EscapeDataString(redirect) & "&grant_type=authorization_code")
                If tokenResponse.ContainsKey("refresh_token") Then data.RefreshToken = Convert.ToString(tokenResponse("refresh_token"))
                If String.IsNullOrEmpty(data.RefreshToken) Then Throw New InvalidOperationException("Google did not issue a refresh token. Revoke Aadhat access in Google Account permissions and connect again.")
                SaveCredentials(data)
            End Using
        Finally
            listener.Stop()
        End Try
    End Sub

    Private Shared Function QueryValue(ByVal query As String, ByVal key As String) As String
        If query.StartsWith("?") Then query = query.Substring(1)
        For Each part As String In query.Split("&"c)
            Dim pair As String() = part.Split(New Char() {"="c}, 2)
            If pair.Length = 2 AndAlso String.Equals(Uri.UnescapeDataString(pair(0).Replace("+", " ")), key, StringComparison.Ordinal) Then Return Uri.UnescapeDataString(pair(1).Replace("+", " "))
        Next
        Return ""
    End Function

    Public Shared Function UploadFolder(ByVal sourceFolder As String, ByVal progress As CloudBackupProgress) As Boolean
        Dim data As OAuthData = ReadCredentials()
        If data Is Nothing OrElse String.IsNullOrEmpty(data.RefreshToken) Then Throw New InvalidOperationException("Google Drive is not connected. Open Cloud Backup Configuration and click Connect Google.")
        If Not Directory.Exists(sourceFolder) Then Throw New DirectoryNotFoundException("Local BackupGoogle folder was not found.")
        Dim accessToken As String = GetAccessToken(data)
        Dim rootFolderId As String = GetOrCreateDriveFolder(accessToken, CloudBackupManager.CloudBackupRootFolderName, "")
        Dim dailyFolderId As String = GetOrCreateDriveFolder(accessToken, CloudBackupManager.CloudBackupDailyFolderName, rootFolderId)
        Dim driveFolderId As String = GetOrCreateDriveFolder(accessToken, Date.Now.ToString("yyyyMMdd"), dailyFolderId)
        Dim files As String() = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories)
        Dim valid As New List(Of String)()
        For Each file As String In files
            If Not String.Equals(Path.GetExtension(file), ".tmp", StringComparison.OrdinalIgnoreCase) Then valid.Add(file)
        Next
        If valid.Count = 0 Then Throw New InvalidOperationException("There are no completed backup files to upload from BackupGoogle.")
        For i As Integer = 0 To valid.Count - 1
            UploadFile(valid(i), sourceFolder, accessToken, driveFolderId)
            If progress IsNot Nothing Then progress(CInt((i + 1) * 100 / Math.Max(1, valid.Count)), "Uploading to Google Drive > " & CloudBackupManager.CloudBackupRootFolderName & " > " & CloudBackupManager.CloudBackupDailyFolderName & " > " & Date.Now.ToString("yyyyMMdd") & ": " & Path.GetFileName(valid(i)))
        Next
        CloudBackupManager.MarkGoogleBackupComplete()
        If progress IsNot Nothing Then progress(100, "Upload complete: Google Drive > " & CloudBackupManager.CloudBackupRootFolderName & " > " & CloudBackupManager.CloudBackupDailyFolderName & " > " & Date.Now.ToString("yyyyMMdd"))
        Return True
    End Function

    Private Shared Function GetAccessToken(ByVal data As OAuthData) As String
        Dim response As Dictionary(Of String, Object) = PostForm("https://oauth2.googleapis.com/token", "client_id=" & Uri.EscapeDataString(data.ClientId) & _
            "&client_secret=" & Uri.EscapeDataString(data.ClientSecret) & "&refresh_token=" & Uri.EscapeDataString(data.RefreshToken) & "&grant_type=refresh_token")
        If Not response.ContainsKey("access_token") Then Throw New InvalidOperationException("Google token refresh failed. Reconnect the Google account.")
        Return Convert.ToString(response("access_token"))
    End Function

    Private Shared Function PostForm(ByVal url As String, ByVal form As String) As Dictionary(Of String, Object)
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) Or SecurityProtocolType.Tls
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        request.Method = "POST" : request.ContentType = "application/x-www-form-urlencoded" : request.Timeout = 60000
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(form) : request.ContentLength = bytes.Length
        Using stream As Stream = request.GetRequestStream() : stream.Write(bytes, 0, bytes.Length) : End Using
        Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            Return Serializer.Deserialize(Of Dictionary(Of String, Object))(New StreamReader(response.GetResponseStream()).ReadToEnd())
        End Using
    End Function

    Private Shared Function GetOrCreateDriveFolder(ByVal token As String, ByVal folderName As String, ByVal parentId As String) As String
        Dim escapedName As String = folderName.Replace("\", "\\").Replace("'", "\'")
        Dim query As String = "name='" & escapedName & "' and mimeType='application/vnd.google-apps.folder' and trashed=false"
        If Not String.IsNullOrEmpty(parentId) Then query &= " and '" & parentId.Replace("'", "\\'") & "' in parents"
        Dim listUrl As String = "https://www.googleapis.com/drive/v3/files?q=" & Uri.EscapeDataString(query) & "&pageSize=100&fields=files(id,name,mimeType)"
        Dim existing As Dictionary(Of String, Object) = GetJson(listUrl, token)
        If existing.ContainsKey("files") Then
            Dim found As System.Collections.ArrayList = DirectCast(existing("files"), System.Collections.ArrayList)
            If found.Count > 0 Then
                Dim first As Dictionary(Of String, Object) = DirectCast(found(0), Dictionary(Of String, Object))
                Return Convert.ToString(first("id"))
            End If
        End If

        Dim metadata As New Dictionary(Of String, Object)()
        metadata("name") = folderName
        metadata("mimeType") = "application/vnd.google-apps.folder"
        If Not String.IsNullOrEmpty(parentId) Then metadata("parents") = New String() {parentId}
        Dim created As Dictionary(Of String, Object) = PostJson("https://www.googleapis.com/drive/v3/files?fields=id", token, metadata)
        If Not created.ContainsKey("id") Then Throw New InvalidOperationException("Google Drive did not return the backup folder ID.")
        Return Convert.ToString(created("id"))
    End Function

    Private Shared Function GetJson(ByVal url As String, ByVal token As String) As Dictionary(Of String, Object)
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        request.Method = "GET" : request.Headers.Add("Authorization", "Bearer " & token) : request.Timeout = 60000
        Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            Return Serializer.Deserialize(Of Dictionary(Of String, Object))(New StreamReader(response.GetResponseStream()).ReadToEnd())
        End Using
    End Function

    Private Shared Function PostJson(ByVal url As String, ByVal token As String, ByVal values As Dictionary(Of String, Object)) As Dictionary(Of String, Object)
        Dim body As Byte() = Encoding.UTF8.GetBytes(Serializer.Serialize(values))
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        request.Method = "POST" : request.ContentType = "application/json; charset=UTF-8" : request.Headers.Add("Authorization", "Bearer " & token)
        request.Timeout = 60000 : request.ContentLength = body.Length
        Using output As Stream = request.GetRequestStream() : output.Write(body, 0, body.Length) : End Using
        Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            Return Serializer.Deserialize(Of Dictionary(Of String, Object))(New StreamReader(response.GetResponseStream()).ReadToEnd())
        End Using
    End Function

    Private Shared Sub UploadFile(ByVal file As String, ByVal sourceFolder As String, ByVal token As String, ByVal driveFolderId As String)
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) Or SecurityProtocolType.Tls
        Dim boundary As String = "aadhat_" & Guid.NewGuid().ToString("N")
        Dim relative As String = file.Substring(sourceFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace("\", "/")
        Dim existingId As String = FindDriveFile(token, relative, driveFolderId)
        Dim metadataValues As New Dictionary(Of String, Object)()
        metadataValues("name") = relative : metadataValues("mimeType") = "application/octet-stream"
        If String.IsNullOrEmpty(existingId) Then metadataValues("parents") = New String() {driveFolderId}
        Dim metadata As String = Serializer.Serialize(metadataValues)
        Dim prefix As Byte() = Encoding.UTF8.GetBytes("--" & boundary & vbCrLf & "Content-Type: application/json; charset=UTF-8" & vbCrLf & vbCrLf & metadata & vbCrLf & _
            "--" & boundary & vbCrLf & "Content-Type: application/octet-stream" & vbCrLf & vbCrLf)
        Dim suffix As Byte() = Encoding.UTF8.GetBytes(vbCrLf & "--" & boundary & "--")
        Dim uploadUrl As String = "https://www.googleapis.com/upload/drive/v3/files"
        If String.IsNullOrEmpty(existingId) Then
            uploadUrl &= "?uploadType=multipart&fields=id"
        Else
            uploadUrl &= "/" & Uri.EscapeDataString(existingId) & "?uploadType=multipart&fields=id"
        End If
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(uploadUrl), HttpWebRequest)
        request.Method = If(String.IsNullOrEmpty(existingId), "POST", "PATCH") : request.ContentType = "multipart/related; boundary=" & boundary : request.Headers.Add("Authorization", "Bearer " & token)
        request.Timeout = 300000 : request.ReadWriteTimeout = 300000
        Dim info As New FileInfo(file) : request.ContentLength = prefix.Length + info.Length + suffix.Length
        Using output As Stream = request.GetRequestStream()
            output.Write(prefix, 0, prefix.Length)
            Using input As New FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read)
                Dim buffer(65535) As Byte : Dim count As Integer
                Do
                    count = input.Read(buffer, 0, buffer.Length) : If count > 0 Then output.Write(buffer, 0, count)
                Loop While count > 0
            End Using
            output.Write(suffix, 0, suffix.Length)
        End Using
        Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            If CInt(response.StatusCode) < 200 OrElse CInt(response.StatusCode) >= 300 Then Throw New InvalidOperationException("Google Drive rejected file upload: " & response.StatusDescription)
        End Using
    End Sub

    Private Shared Function FindDriveFile(ByVal token As String, ByVal fileName As String, ByVal parentId As String) As String
        Dim escapedName As String = fileName.Replace("\", "\\").Replace("'", "\'")
        Dim query As String = "name='" & escapedName & "' and '" & parentId.Replace("'", "\\'") & "' in parents and trashed=false"
        Dim result As Dictionary(Of String, Object) = GetJson("https://www.googleapis.com/drive/v3/files?q=" & Uri.EscapeDataString(query) & "&pageSize=1&fields=files(id)", token)
        If Not result.ContainsKey("files") Then Return ""
        Dim found As System.Collections.ArrayList = DirectCast(result("files"), System.Collections.ArrayList)
        If found.Count = 0 Then Return ""
        Return Convert.ToString(DirectCast(found(0), Dictionary(Of String, Object))("id"))
    End Function
End Class
