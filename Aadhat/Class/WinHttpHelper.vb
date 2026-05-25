Imports System.Text
Imports Newtonsoft.Json

Public Class WinHttpHelper

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

            http.Open("POST", url, False)

            http.SetRequestHeader(
                "Content-Type",
                "application/json"
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

            http.Open("GET", url, False)

            http.Send()

            Return http.ResponseText

        Catch ex As Exception

            Return ""

        End Try

    End Function

End Class