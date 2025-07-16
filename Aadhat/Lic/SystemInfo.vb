Public Class SystemInfo
    Dim ClsCommon As CommonClass = New CommonClass()

    Private Sub SystemInfo_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub SystemInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 130 : Me.Left = 84
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.GhostWhite : Me.KeyPreview = True


        txtMacAddress.Text = ClsCommon.GetMacAddress()
        ' MsgBox("A")
        txtHostName.Text = ClsCommon.GetHostName()
        'MsgBox("B")
        ' txtIPAddress.Text = ClsCommon.GetIPAddress()
        'MsgBox("C")
        txtOSName.Text = ClsCommon.GetOSName()
        'MsgBox("D")
        txtInternetCheck.Text = ClsCommon.IsInternetConnect()
        'MsgBox("E")
        txtMotherBoard.Text = ClsCommon.MotherboardSerialNumber()
        'MsgBox("F")
        txtHardDiskSerial.Text = ClsCommon.HardDiskSerialNumber()
        'MsgBox("G")
    End Sub
End Class