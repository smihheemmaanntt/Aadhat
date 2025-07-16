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
        PictureBox1.Image = My.Resources._124034
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.Top = 0 : Me.Left = 0 : Me.KeyPreview = True
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        cbLanguage.SelectedIndex = 0 : cbmsgType.SelectedIndex = 0 : cbMethod.SelectedIndex = 0
        rowColums() : FillControl()
        If ClsCommon.IsInternetConnect() = False Then Timer1.Stop() : Exit Sub
        ' If TxtInstanceID.Text.Trim <> "" Then ScanQRCode()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 70
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "Name" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Nos." : dg1.Columns(4).Width = 80
        dg1.Columns(5).Name = "Status" : dg1.Columns(5).Width = 200
    End Sub
    Private Sub ScanQRCode()
        Dim qrCodeUrl As String = "http://smicloud.in/api/get_qrcode?instance_id=" & TxtInstanceID.Text & "&access_token=" & access_token
        Dim qrCodeBase64 As String = GetQRCodeBase64(qrCodeUrl)
        ' Convert the Base64 string to an image and display it in a PictureBox
        If qrCodeBase64 <> "" Then
            Dim qrCodeImage As Image = Base64ToImage(qrCodeBase64)
            PictureBox1.Image = qrCodeImage
            PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        End If
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
    Private Function GetQRCodeBase64(url As String) As String
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Timer1.Stop() : Exit Function
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim stream As Stream = response.GetResponseStream()
        ' Convert the response stream to a byte array
        Dim buffer As Byte() = New Byte(1023) {}
        Dim bytesRead As Integer = 0
        Dim memoryStream As New MemoryStream()
        While (InlineAssignHelper(bytesRead, stream.Read(buffer, 0, buffer.Length))) > 0
            memoryStream.Write(buffer, 0, bytesRead)
        End While
        ' Convert the byte array to Base64 string
        Dim responseString = Encoding.ASCII.GetString(memoryStream.ToArray())
        Dim responseJson = JsonConvert.DeserializeObject(responseString)
        Dim status = responseJson("status")
        If (status <> "error") Then
            Dim qrCodeBase64 As String = responseJson("base64").ToString().Replace("data:image/png;base64,", "")
            stream.Close() : response.Close() : Timer1.Start()
            lblStatus.Visible = False : Return qrCodeBase64
        Else
            If responseJson("message") = "Instance ID Invalidated" Then
                lblStatus.Visible = True : lblStatus.Text = "Disconnected"
                lblStatus.BackColor = Color.Red
                Exit Function
            End If
            If responseJson("message") = "Instance ID has been used" Then
                lblStatus.Visible = True : lblStatus.Text = "Connected"
                lblStatus.BackColor = Color.Green
                QRCodeStatus = "Instance ID has been used" : PictureBox1.Image = My.Resources._124034
                btnReconnect.Text = "Re-Connect" : btnReconnect.BackColor = Color.Green : Exit Function
            End If
            MsgBox(responseJson("message"))
            Return ""
        End If
        ' Clean up resources
    End Function


    Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
        target = value
        Return value
    End Function
    Private Function Base64ToImage(base64String As String) As Image
        ' Convert Base64 string to byte array
        Dim imageConverter As New ImageConverter()
        Dim imageBytes As Byte() = Convert.FromBase64String(base64String)
        Try
            ' Create a MemoryStream from the byte array
            Dim memoryStream As New MemoryStream(imageBytes)
            Dim image As Image = DirectCast(imageConverter.ConvertFrom(imageBytes), Image)
            Return image
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ' Method to extract the instance ID from the API response JSON
    Private Function GetInstanceID(apiResponse As String) As String
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Function
        Dim json As JObject = JObject.Parse(apiResponse)
        Dim instanceID As String = json("instance_id").ToString()
        Return instanceID
    End Function

    Private Sub btnGetIntanceID_Click(sender As Object, e As EventArgs) Handles btnGetIntanceID.Click
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Sub
        Dim apiResponse As String = SendAPIRequest()
        Dim instanceID As String = GetInstanceID(apiResponse)
        TxtInstanceID.Text = instanceID
        Dim sql As String = String.Empty
        Sql = "Delete From API;Insert Into API(InstanceID,SendingMethod,LanguageType,SendingType) SELECT " & _
             "'" & TxtInstanceID.Text & "','" & cbMethod.Text & "','" & cbLanguage.Text & "','" & cbmsgType.Text & "'"
        If ClsFunPrimary.ExecNonQuery(sql) > 0 Then FillControl()
        ScanQRCode()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectAll()
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectAll()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub btnReconnect_Click(sender As Object, e As EventArgs) Handles btnReconnect.Click
        If ClsCommon.IsInternetConnect() = False Then MsgBox("Check Internet Connection", MsgBoxStyle.Critical, "No Internet Connection") : Exit Sub
        '  If btnReconnect.Text = "Re-Connect" Then
        Reconnect()
        ' End If
    End Sub
    Private Sub Reconnect()
        ' Make the API request
        Dim url As String = "http://smicloud.in/api/reset_instance?instance_id=" & TxtInstanceID.Text & "&access_token=" & access_token
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

        ' Read the response
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim responseJson As String = reader.ReadToEnd()
        reader.Close()

        ' Deserialize the response JSON
        Dim responseData As JObject = JObject.Parse(responseJson)

        ' Check the status
        If responseData("status").ToString().ToLower() = "success" Then
            Dim apiResponse As String = SendAPIRequest()
            Dim instanceID As String = GetInstanceID(apiResponse)
            TxtInstanceID.Text = instanceID
            ClsFunPrimary.ExecScalarStr("Delete From API;Insert Into API(InstanceID) Select '" & TxtInstanceID.Text & "'")
            ScanQRCode()
        Else
            MsgBox("Unable to Reconnect")
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Static counter As Integer = 0
        counter += 1
        If counter Mod 10 = 0 Then
            Dim qrCodeUrl As String = "http://smicloud.in/api/get_qrcode?instance_id=" & TxtInstanceID.Text & "&access_token=" & access_token
            GetQRCodeBase64(qrCodeUrl)
        End If

        If QRCodeStatus = "Instance ID has been used" Then
            Timer1.Stop()
            lblStatus.Visible = True
            lblStatus.Text = "Connected"
            lblStatus.BackColor = Color.Green
            QRCodeStatus = "Instance ID has been used"
            PictureBox1.Image = My.Resources._124034
            btnReconnect.Text = "Re-Connect"
            btnReconnect.BackColor = Color.Green
        End If
        'If QRCodeStatus = "Instance ID has been used" Then
        '    Timer1.Stop()
        '    lblStatus.Visible = True : lblStatus.Text = "Connected"
        '    lblStatus.BackColor = Color.Green
        '    QRCodeStatus = "Instance ID has been used" : PictureBox1.Image = My.Resources._124034
        '    btnReconnect.Text = "Re-Connect" : btnReconnect.BackColor = Color.Green
        'Else
        '    ScanQRCode()
        'End If
    End Sub

    Private Sub cbMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbMethod.SelectedIndexChanged
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SaveDefault()
    End Sub
    Private Sub SaveDefault()
        Dim Sql As String = String.Empty
        Sql = "Delete From API;Insert Into API(InstanceID,SendingMethod,LanguageType,SendingType) SELECT " & _
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

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub
    Private Sub retrive()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from WaReport Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Order by EntryDate")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("Type").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("WhatsAppNo").ToString()
                        .Cells(5).Value = dt.Rows(i)("Status").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
    End Sub
End Class