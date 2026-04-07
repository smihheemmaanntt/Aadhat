Imports System.Net
Imports System.Text
Imports System.IO

Module PhoneMSg

    Private BASE_URL As String = "http://msgz.in"
    Private UPLOAD_KEY As String = "SOFTMANAGE@2026"

    ' ==========================================
    ' COMMON POST JSON FUNCTION
    ' ==========================================
    Private Function PostJSON(ByVal url As String,
                              ByVal json As String,
                              ByVal token As String) As String

        Try
            Dim req As HttpWebRequest =
                CType(WebRequest.Create(url), HttpWebRequest)

            req.Method = "POST"
            req.ContentType = "application/json"

            If token <> "" Then
                req.Headers.Add("Authorization", "Bearer " & token)
            End If

            Dim bytes() As Byte =
                Encoding.UTF8.GetBytes(json)

            req.ContentLength = bytes.Length

            Using stream As Stream = req.GetRequestStream()
                stream.Write(bytes, 0, bytes.Length)
            End Using

            Dim response As HttpWebResponse =
                CType(req.GetResponse(), HttpWebResponse)

            Using respStream As Stream =
                response.GetResponseStream()

                Using reader As New StreamReader(respStream)
                    Return reader.ReadToEnd()
                End Using

            End Using

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function


    ' ==========================================
    ' GET DEVICE ID FROM API
    ' ==========================================
    Public Function GetDeviceID() As String

        Try
            Dim token As String =
                ClsFunPrimary.ExecScalarStr(
                    "Select Msg_Access_Token From API"
                )

            Dim url As String =
                BASE_URL & "/api/get/devices"

            Dim req As HttpWebRequest =
                CType(WebRequest.Create(url), HttpWebRequest)

            req.Method = "GET"
            req.ContentType = "application/json"
            req.Headers.Add("Authorization", "Bearer " & token)

            Dim response As HttpWebResponse =
                CType(req.GetResponse(), HttpWebResponse)

            Using respStream As Stream =
                response.GetResponseStream()

                Using reader As New StreamReader(respStream)

                    Dim json As String =
                        reader.ReadToEnd()

                    ' ===== FIND FIRST "id": NUMBER =====
                    Dim key As String = """id"":"

                    Dim i As Integer =
                        json.IndexOf(key)

                    If i = -1 Then
                        Return "DEVICE ID NOT FOUND"
                    End If

                    Dim startPos As Integer =
                        i + key.Length

                    Dim endPos As Integer =
                        json.IndexOf(",", startPos)

                    If endPos = -1 Then
                        endPos =
                            json.IndexOf("}", startPos)
                    End If

                    Dim deviceID As String =
                        json.Substring(startPos,
                                       endPos - startPos).Trim()

                    Return deviceID

                End Using

            End Using

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function


    ' ==========================================
    ' SEND MESSAGE (AUTO DEVICE ID)
    ' ==========================================
    Public Function SendPhoneMsg(
        ByVal to_numbers As String,
        ByVal subscriber_id As String,
        ByVal body As String
    ) As String

        Try

            ' ===== AUTO DEVICE FETCH =====
            Dim device_id As String = GetDeviceID()

            If device_id = "" Or
               device_id.Contains("NOT") Then

                Return device_id
            End If

            Dim token As String =
                ClsFunPrimary.ExecScalarStr(
                    "Select Msg_Access_Token From API"
                )

            ' Escape quotes
            body = body.Replace("""", "\""") _
                       .Replace(vbCrLf, "\n")

            Dim json As String =
                "{""device_id"":""" & device_id &
                """,""to_numbers"":""" & to_numbers &
                """,""subscriber_id"":""" & subscriber_id &
                """,""body"":""" & body & """}"

            Dim result As String =
                PostJSON(BASE_URL &
                         "/api/sent/compose",
                         json,
                         token)

            If result.Contains("Message queued successfully") Then
                Return "SUCCESS"
            Else
                Return result
            End If

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function


    ' ==========================================
    ' PDF UPLOAD (OPTIONAL)
    ' ==========================================
    Public Function UploadPDF_Local(
        ByVal filePath As String
    ) As String

        Try
            If Not File.Exists(filePath) Then
                Return "File Not Found"
            End If

            Dim wc As New WebClient()
            wc.Headers.Add("X-Upload-Key", UPLOAD_KEY)

            Dim responseBytes() As Byte =
                wc.UploadFile(BASE_URL &
                              "/upload.php",
                              "POST",
                              filePath)

            Dim response As String =
                Encoding.UTF8.GetString(responseBytes)

            If response.Contains("""status"":true") Then

                Dim s As Integer =
                    response.IndexOf("short_url")

                If s > -1 Then

                    Dim u1 As Integer =
                        response.IndexOf("http", s)

                    Dim u2 As Integer =
                        response.IndexOf("""", u1)

                    Return response.Substring(u1,
                                              u2 - u1)
                End If

                Return "SHORT URL NOT FOUND"
            End If

            Return "UPLOAD FAIL"

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

End Module