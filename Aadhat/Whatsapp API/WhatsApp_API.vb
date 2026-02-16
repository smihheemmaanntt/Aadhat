Imports System.Net
Imports System.IO
Imports System.Text
Imports Newtonsoft.Json.Linq
Imports System.Diagnostics
Imports Newtonsoft.Json
Public Class WhatsApp_API
    Dim ClsCommon As CommonClass = New CommonClass()
    ' Dim instance_id As String = "648E8D4BC315A"
    Dim access_token As String = "6687c047a58e1"
    Private WithEvents timer As Timer
    Dim QRCodeStatus As String
    Private Sub WhatsApp_API_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub WhatsApp_API_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0 : Me.KeyPreview = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        cbLanguage.SelectedIndex = 0 : cbmsgType.SelectedIndex = 0 : cbMethod.SelectedIndex = 0
        FillControl()
        If ClsCommon.IsInternetConnect() = False Then Timer1.Stop() : Exit Sub
        ' If TxtInstanceID.Text.Trim <> "" Then ScanQRCode()
    End Sub
 
    Private Function SendAPIRequest() As String
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Function
        Dim apiUrl As String = "http://smicloud.in/api/create_instance?&access_token=" & access_token
        Dim request As HttpWebRequest = CType(WebRequest.Create(apiUrl), HttpWebRequest)
        Dim response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

        Using reader As New StreamReader(response.GetResponseStream())
            Return reader.ReadToEnd()
        End Using
    End Function
  

    Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function


    ' Method to extract the instance ID from the API response JSON
    Private Function GetInstanceID(apiResponse As String) As String
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Function
        Dim json As JObject = JObject.Parse(apiResponse)
        Dim instanceID As String = json("instance_id").ToString()
        Return instanceID
    End Function

    Private Sub btnGetIntanceID_Click(sender As Object, e As EventArgs)
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Sub
        Dim apiResponse As String = SendAPIRequest()
        Dim instanceID As String = GetInstanceID(apiResponse)
        TxtInstanceID.Text = instanceID
        Dim sql As String = String.Empty
        sql = "Delete From API;Insert Into API(InstanceID,SendingMethod,LanguageType,SendingType) SELECT " & _
             "'" & TxtInstanceID.Text & "','" & cbMethod.Text & "','" & cbLanguage.Text & "','" & cbmsgType.Text & "'"
        If ClsFunPrimary.ExecNonQuery(sql) > 0 Then FillControl()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub btnReconnect_Click(sender As Object, e As EventArgs) Handles btnReconnect.Click
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Sub
        SaveDefault()
        '  If btnReconnect.Text = "Re-Connect" Then
        ' End If
    End Sub
 

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveDefault()
    End Sub
    Private Sub SaveDefault()
        Dim Sql As String = String.Empty
        Sql = "Delete From API;Insert Into API(InstanceID,AccessTken,SendingMethod,LanguageType,SendingType) SELECT " & _
            "'" & TxtInstanceID.Text & "','" & cbMethod.Text & "','" & cbLanguage.Text & "','" & cbmsgType.Text & "'"
        If ClsFunPrimary.ExecNonQuery(Sql) > 0 Then MsgBox("Sending Settings Updated For All Companies", MsgBoxStyle.Information, "Updated")
        FillControl()
    End Sub
    Public Sub FillControl()
        Dim Sql As String = "Select * From API"
        Dim dt As New DataTable
        dt = ClsFunPrimary.ExecDataTable(Sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    TxtInstanceID.Text = dt.Rows(i)("InstanceID").ToString()
                    TxtInstanceID.Text = dt.Rows(i)("AccessToken").ToString()
                    cbMethod.Text = dt.Rows(i)("SendingMethod").ToString()
                    cbLanguage.Text = dt.Rows(i)("LanguageType").ToString()
                    cbmsgType.Text = dt.Rows(i)("SendingType").ToString()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        'clsFun.CloseConnection()
    End Sub

 
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        UploadPDF_Local("D:\1\Aadhat\Aadhat\bin\x86\Debug\Pdfs\SONU BHAI-24-10-2025.pdf")
    End Sub
End Class