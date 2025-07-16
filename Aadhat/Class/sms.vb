Imports System.IO
Imports System.Net

Public Class Sms


    Protected Shared Function SendSMSToNum(url As String) As String

        Dim objURI As Uri = New Uri(url)
        Dim objWebRequest As WebRequest = WebRequest.Create(objURI)
        Dim objWebResponse As WebResponse = objWebRequest.GetResponse()
        Dim objStream As Stream = objWebResponse.GetResponseStream()
        Dim objStreamReader As StreamReader = New StreamReader(objStream)
        Dim strHTML As String = objStreamReader.ReadToEnd
        SendSMSToNum = strHTML

    End Function

    Public Shared Function SendSms(ByVal SendMessageNo As String) As String
        Dim strGatewayResponse As String = "Message not send"
        Try
            Dim dt As System.Data.DataTable = clsFun.ExecDataTable("Select * from SMSAPI")
            If dt.Rows.Count > 0 Then
                Dim tempUrl As String
                Dim part1 As String = dt.Rows(0)("part1").ToString()
                Dim part2 As String = dt.Rows(0)("part2").ToString()
                Dim part3 As String = dt.Rows(0)("part3").ToString()
                Dim defaultsms As String = dt.Rows(0)("DefaultTemplate").ToString()
                tempUrl = part1 & SendMessageNo & part2 & defaultsms & part3

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls


                strGatewayResponse = SendSMSToNum(tempUrl)

                If strGatewayResponse.Contains("3001") Or strGatewayResponse.Contains("true") Then
                    strGatewayResponse = "Message Submitted"
                Else
                    strGatewayResponse = "Message not send" & strGatewayResponse
                End If

            End If
        Catch ex As Exception
            strGatewayResponse = ex.Message & "Message not send"
        End Try



        Return strGatewayResponse
    End Function
End Class
