Imports System.IO
Imports System.Net
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class WhatsAppSender
    Public FilePath As String
    Public hostedFilePath As String
    Public access_token As String = "6687c047a58e1"
    Public instance_id As String = ClsFunPrimary.ExecScalarStr("Select InstanceID From API")
    Public APIResposne As String

    Public Sub SendWhatsAppMessage(ByVal phoneNumber As String, ByVal message As String)
        Dim url As String = "http://smicloud.in/api/send?number=" & phoneNumber & "&type=text&message=" & Uri.EscapeDataString(message) & "&instance_id=" & instance_id & "&access_token=" & access_token
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        request.Method = "GET"
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim responseStream As Stream = response.GetResponseStream()
        Dim reader As New StreamReader(responseStream, Encoding.GetEncoding("utf-8"))
        Dim responseString As String = reader.ReadToEnd()
        ' Parse the JSON response
        Dim responseJson As JObject = JObject.Parse(responseString)
        ' Extract the "status" value
        Dim status As String = responseJson("status").ToString()
        Dim msg As String = responseJson("message").ToString()
        If status = "success" AndAlso msg IsNot Nothing Then
            APIResposne = "Successful"
        Else
            APIResposne = "Unsuccessful"
        End If
    End Sub

    Public Sub SendWhatsAppFile(ByVal phoneNumber As String, ByVal message As String, ByVal pdfFilePath As String)
        'UplaodFile()
        pdfFilePath = FilePath
        ' Dim base64Pdf As String = ConvertPdfToBase64(pdfFilePath)
        ' Dim url As String = "http://aadhat.cloud/send?phone=" & phoneNumber & "&text=" & Uri.EscapeDataString(message) & "&file=" & Uri.EscapeDataString(base64Pdf)
        Dim url As String = "http://smicloud.in/api/send?number=" & phoneNumber & "&type=text&message=" & Uri.EscapeDataString(message) & "&media_url=" & Uri.EscapeDataString(pdfFilePath) & "&instance_id=" & instance_id & "&access_token=" & access_token & ""
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        request.Method = "GET"
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim responseStream As Stream = response.GetResponseStream()
        Dim reader As New StreamReader(responseStream, Encoding.GetEncoding("utf-8"))
        Dim responseString As String = reader.ReadToEnd()
        Dim responseJson As JObject = JObject.Parse(responseString)
        Dim status As String = responseJson("status").ToString()
        Dim msg As String = responseJson("message").ToString()
        If status = "success" AndAlso msg IsNot Nothing Then
            APIResposne = "Successful"
        Else
            APIResposne = "Unsuccessful"
        End If
    End Sub
    'Public Shared Function UploadFile(filePath As String) As String
    '    Dim apiUrl As String = "http://free.keep.sh/upload" ' URL for file upload
    '    Dim client As New WebClient()

    '    Try
    '        ' Prepare the file for upload
    '        Dim fileData As Byte() = File.ReadAllBytes(filePath)

    '        ' Upload the file as a multipart form data
    '        Dim responseBytes As Byte() = client.UploadData(apiUrl, "POST", fileData)

    '        ' Convert the response to a string
    '        Dim responseString As String = System.Text.Encoding.UTF8.GetString(responseBytes)

    '        ' Parse the response to get the URL
    '        Dim responseObject As JObject = JObject.Parse(responseString)
    '        Dim hostedFilePath As String = responseObject("url").ToString()

    '        Return hostedFilePath
    '    Catch ex As Exception
    '        ' Handle any exceptions here
    '        Throw ex
    '    End Try
    'End Function


    'Public Shared Function UploadFile(filePath As String) As String
    '    Dim url As String = "http://temp.sh/"
    '    Dim client As New WebClient()
    '    Try
    '        client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36")
    '        Dim responseBytes As Byte() = client.UploadFile(url, filePath)
    '        Dim responseString As String = System.Text.Encoding.UTF8.GetString(responseBytes)
    '        Dim responseObject As JObject = JObject.Parse(responseString)
    '        Dim hostedFilePath As String = responseObject("data")("url").ToString()
    '        ' hostedFilePath = hostedFilePath.Replace("http://temp.sh/", "http://temp.sh/dl/")
    '        Return hostedFilePath
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function




    Public Shared Function UploadFile(filePath As String) As String
        ' Define the URL for the file upload
        Dim url As String = "https://tmpfiles.org/api/v1/upload"

        ' Create a new instance of WebClient
        Using client As New WebClient()
            ' Set the appropriate header for the binary file upload
            client.Headers.Add("Content-Type", "binary/octet-stream")

            ' Upload the file and get the response as a byte array
            Dim responseBytes As Byte() = client.UploadFile(url, filePath)

            ' Convert the response bytes to a string
            Dim responseString As String = System.Text.Encoding.UTF8.GetString(responseBytes)

            ' Parse the response string to extract the URL
            Dim responseObject As JObject = JObject.Parse(responseString)
            Dim originalUrl As String = responseObject("data")("url").ToString()

            ' Modify the URL for direct download
            Dim hostedFilePath As String = originalUrl.Replace("https://tmpfiles.org/", "https://tmpfiles.org/dl/")

            ' Return the modified URL
            Return hostedFilePath
        End Using
    End Function

    'Public Shared Function UploadFile(filePath As String) As String
    '    Dim url As String = "http://tmpfiles.org/api/v1/upload"
    '    Dim client As New WebClient()

    '    Try
    '        Dim responseBytes As Byte() = client.UploadFile(url, filePath)
    '        Dim responseString As String = System.Text.Encoding.UTF8.GetString(responseBytes)
    '        Dim responseObject As JObject = JObject.Parse(responseString)
    '        Dim originalUrl As String = responseObject("data")("url").ToString()

    '        ' Modify the URL path by replacing the initial part with "/dl/"
    '        Dim modifiedUrl As String = originalUrl.Replace("http://tmpfiles.org/", "http://tmpfiles.org/dl/")
    '        Dim hostedFilePath As String = modifiedUrl

    '        Return hostedFilePath
    '    Catch ex As Exception
    '        ' Handle exceptions here (e.g., log or display an error message)
    '        Return "Error: " & ex.Message
    '    End Try
    'End Function


    'Public Shared Function UploadFile(filePath As String) As String
    '    Dim url As String = "http://file.pizza/"
    '    Dim client As New WebClient()
    '    Try
    '         Set the user-agent header to mimic a web browser request
    '        client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36")

    '         Upload the file to the service
    '        Dim responseBytes As Byte() = client.UploadFile(url, filePath)

    '         Parse the response JSON
    '        Dim responseString As String = System.Text.Encoding.UTF8.GetString(responseBytes)
    '        Dim responseObject As JObject = JObject.Parse(responseString)

    '         Extract the URL of the uploaded file
    '        Dim hostedFilePath As String = responseObject("url").ToString()

    '        Return hostedFilePath
    '    Catch ex As Exception
    '         Handle any exceptions here
    '        Throw ex
    '    End Try
    'End Function

End Class
