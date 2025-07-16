Imports System.IO
Public Class LicenceInfo
    Dim ClsCommon As CommonClass = New CommonClass()

    Private Sub SystemInfo_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub SystemInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 130 : Me.Left = 84
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.GhostWhite : Me.KeyPreview = True
        Dim str As String
        If File.Exists(Application.StartupPath & "\Accent.Dll") = True Then
            pnlDemo.Visible = False
            Dim reader As TextReader = New StreamReader(Application.StartupPath & "\Accent.Dll")
            str = ClsCommon.DecodeBase64(reader.ReadToEnd())
            reader.Close()
            Dim count As Integer
            strArr = str.Split("$")
            For count = 0 To strArr.Length - 1
                ' MsgBox(strArr(count))
                txtLicenseKey.Text = strArr(0)
                txtMacAddress.Text = strArr(1)
                txtHostName.Text = strArr(2)
                txtIPAddress.Text = strArr(3)
                txtOSName.Text = strArr(4)
                txtDueDays.Text = strArr(5)
                'Dim CheckDemo As String = CheckDays.Split("|")
                'If Val(CheckDemo(2)) >= 30 Then
                '    pnlDemo.Visible = True
                'Else
                '    txtDueDays.Text = strArr(5)
                'End If
                txtActivationDays.Text = strArr(6)
                txtMotherBoard.Text = strArr(7)
                txtHardDiskSerial.Text = strArr(8)

            Next
        Else
            pnlDemo.Visible = True : Exit Sub
        End If


        'txtMacAddress.Text = ClsCommon.GetMacAddress()
        'txtHostName.Text = ClsCommon.GetHostName()
        'txtIPAddress.Text = ClsCommon.GetIPAddress()
        'txtOSName.Text = ClsCommon.GetOSName()
        'txtInternetCheck.Text = ClsCommon.IsInternetConnect()
        'txtMotherBoard.Text = ClsCommon.MotherboardSerialNumber()
        'txtHardDiskSerial.Text = ClsCommon.HardDiskSerialNumber()
    End Sub
End Class