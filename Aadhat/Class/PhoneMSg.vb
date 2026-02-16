Imports System.Net
Imports System.Text
Imports System.IO

Module PhoneMSg

    ' 🔗 BASE URL
    Private BASE_URL As String = "http://msgz.in"
    ' 🔐 UPLOAD SECURITY KEY
    Private UPLOAD_KEY As String = "SOFTMANAGE@2026"
    ' ==========================================
    ' 📤 PDF UPLOAD FUNCTION (LOCAL → SERVER)
    ' ==========================================
 Public Function UploadPDF_Local(
           ByVal filePath As String
       ) As String
        Try
            If Not File.Exists(filePath) Then
                Return "File Not Found"
            End If
            Dim url As String =
                BASE_URL & "/upload.php"
            Dim client As New WebClient()
            client.Headers.Add(
                "X-Upload-Key",
                UPLOAD_KEY
            )
            Dim resBytes As Byte() =
                client.UploadFile(url, "POST", filePath)
            Dim res As String =
                Encoding.UTF8.GetString(resBytes)
            If res.Contains("""status"":true") Then
                Dim s As Integer =
                    res.IndexOf("short_url")
                Dim u1 As Integer =
                    res.IndexOf("http", s)
                Dim u2 As Integer =
                    res.IndexOf("""", u1)
                Return res.Substring(u1, u2 - u1)
            Else
                Return "UPLOAD FAIL"
            End If
        Catch ex As Exception
            Return ex.Message
        End Try

    End Function
    ' ==========================================
    ' 📩 SEND MESSAGE FUNCTION
    ' ==========================================
    Public Function SendPhoneMsg( _
        ByVal device_id As String, _
        ByVal to_numbers As String, _
        ByVal subscriber_id As String, _
        ByVal body As String _
    ) As String

        Try
            Dim token As String = ClsFunPrimary.ExecScalarStr("Select Msg_Access_Token From API")
            Dim url As String = BASE_URL & "/api/sent/compose"
            Dim jsonBody As String = "{" & _
                """device_id"":""" & device_id & """," & _
                """to_numbers"":""" & to_numbers & """," & _
                """subscriber_id"":""" & subscriber_id & """," & _
                """body"":""" & body.Replace("""", "\""") & """" & _
            "}"
            Dim request As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            request.Method = "POST"
            request.ContentType = "application/json"
            request.Headers.Add("Authorization", "Bearer " & token)
            ServicePointManager.ServerCertificateValidationCallback = Function() True
            Dim byteArray As Byte() = Encoding.UTF8.GetBytes(jsonBody)
            request.ContentLength = byteArray.Length
            Dim stream As Stream = request.GetRequestStream()
            stream.Write(byteArray, 0, byteArray.Length)
            stream.Close()
            Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim reader As New StreamReader(response.GetResponseStream())
            Dim result As String = reader.ReadToEnd()
            If result.Contains("Message queued successfully") Then
                Return "SUCCESS"
            Else
                Return result
            End If
        Catch ex As Exception
            Return ex.Message
        End Try
    End Function
End Module
