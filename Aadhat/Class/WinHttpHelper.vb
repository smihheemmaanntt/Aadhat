Imports System.Text
Imports Newtonsoft.Json

Public Class WinHttpHelper

    Private Const SecureProtocolsOption As Integer = 9
    Private Const SecureProtocolTls1 As Integer = 128
    Private Const SecureProtocolTls11 As Integer = 512
    Private Const SecureProtocolTls12 As Integer = 2048

    Private Shared Sub ApplySecureProtocolFallback(ByVal http As Object)
        Try
            http.Option(SecureProtocolsOption) =
                SecureProtocolTls1 Or SecureProtocolTls11 Or SecureProtocolTls12
        Catch
            Try
                http.Option(SecureProtocolsOption) = SecureProtocolTls1
            Catch
            End Try
        End Try
    End Sub

    Public Shared Function PostJson(
        ByVal url As String,
        ByVal obj As Object
    ) As String

        Try

            Dim json As String =
                JsonConvert.SerializeObject(obj)

            Dim http As Object

            http = CreateObject(
                "WinHttp.WinHttpRequest.5.1"
            )

            ' SSL ignore optional
            http.Option(4) = 13056
            ApplySecureProtocolFallback(http)

            http.Open("POST", url, False)

            ' Keep API calls responsive on slow mobile/office networks.
            ' Values are milliseconds: resolve, connect, send, receive.
            http.SetTimeouts(10000, 15000, 60000, 60000)
            http.SetRequestHeader("User-Agent", "Aadhat/1.0")
            http.SetRequestHeader("Accept", "application/json")

            http.SetRequestHeader(
                "Content-Type",
                "application/json; charset=utf-8"
            )

            http.Send(json)

            Return http.ResponseText

        Catch ex As Exception

            Return "{""status"":""error"",""message"":""" &
                   ex.Message.Replace("""", "'") &
                   """}"

        End Try

    End Function

    Public Shared Function GetData(
        ByVal url As String
    ) As String

        Try

            Dim http As Object

            http = CreateObject(
                "WinHttp.WinHttpRequest.5.1"
            )

            http.Option(4) = 13056
            ApplySecureProtocolFallback(http)

            http.Open("GET", url, False)

            http.SetTimeouts(10000, 15000, 30000, 30000)
            http.SetRequestHeader("User-Agent", "Aadhat/1.0")
            http.SetRequestHeader("Accept", "application/json")

            http.Send()

            Return http.ResponseText

        Catch ex As Exception

            Return ""

        End Try

    End Function

    Public Shared Function DownloadFile(
        ByVal url As String,
        ByVal filePath As String
    ) As Boolean

        Try
            Dim http As Object

            http = CreateObject(
                "WinHttp.WinHttpRequest.5.1"
            )

            http.Option(4) = 13056
            ApplySecureProtocolFallback(http)
            http.Open("GET", url, False)
            http.SetTimeouts(10000, 15000, 60000, 60000)
            http.SetRequestHeader("User-Agent", "Aadhat/1.0")
            http.Send()

            If CInt(http.Status) < 200 OrElse CInt(http.Status) >= 300 Then Return False

            Dim stream As Object
            stream = CreateObject("ADODB.Stream")
            stream.Type = 1
            stream.Open()
            stream.Write(http.ResponseBody)
            stream.SaveToFile(filePath, 2)
            stream.Close()

            Return True

        Catch
            Return False
        End Try

    End Function

End Class
