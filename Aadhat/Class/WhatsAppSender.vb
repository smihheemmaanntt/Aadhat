Imports System.IO
Imports System.Net
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Class WhatsAppSender
    Public FilePath As String
    Public hostedFilePath As String
    Public access_token As String = ""
    Public instance_id As String = ""
    Public APIResposne As String

    Public Sub SendWhatsAppMessage(ByVal phoneNumber As String, ByVal message As String)
        Dim url As String = "https://wahsoft.in/public/api/" & instance_id.Trim & "/contact/send-template-message?token=" & access_token
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

    Public Function SendOfficialSmartMessage(ByVal phoneNumber As String, ByVal message As String, ByVal pdfFilePath As String) As Boolean
        Try
            APIResposne = ""

            If pdfFilePath.Trim() <> "" Then
                FilePath = pdfFilePath
                SendWhatsAppFile(phoneNumber, message, pdfFilePath)
            Else
                SendWhatsAppMessage(phoneNumber, message)
            End If

            Dim result As String = If(APIResposne, "").Trim().ToLower()
            Return result.Contains("successful") OrElse result.Contains("success")
        Catch ex As Exception
            APIResposne = ex.Message
            Return False
        End Try
    End Function


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
   
End Class
