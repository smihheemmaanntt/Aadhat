Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Public Class ApplyLicenseKey
    Dim el As New Aadhat.ErrorLogger

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")> Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Dim ClsCommon As CommonClass = New CommonClass()
        Dim dtLicenseInfo As DataTable = New DataTable()
        Dim dtLicenseInfoValidate As DataTable = New DataTable()
        'Dim objDBHelper As DBHelper = New DBHelper()
        Dim encryptedString As String = String.Empty
        Dim tempMac As String = String.Empty
        Dim OSName As String = String.Empty
        Dim hostName As String = String.Empty
        Dim myIP As String = String.Empty
        Dim MotherBoardID As String = String.Empty
        Dim HardDiskID As String = String.Empty
        lblError.Text = ""

        Try
            If ClsCommon.IsInternetConnect() Then
                hostName = ClsCommon.GetHostName()
                '   myIP = ClsCommon.GetIPAddress()
                tempMac = ClsCommon.GetMacAddress()
                OSName = ClsCommon.GetOSName()
                MotherBoardID = ClsCommon.MotherboardSerialNumber()
                HardDiskID = ClsCommon.HardDiskSerialNumber()
                Dim strSql As String = "Select * from LicenseKeyDetails where LicenseKey='" + txtLicKey.Text + "'"
                Dim dtb As New DataTable
                ' Using cnn As New SqlConnection("server=sql5042.site4now.net;database=DB_A4AB62_LicenseKey;uid=DB_A4AB62_LicenseKey_admin;pwd=meadmin81; Integrated Security=True;MultipleActiveResultSets=True")
                Using cnn As New SqlConnection(Convert.ToString(ClsCommon.GetConnectionstr()))
                    cnn.Open()
                    Using dad As New SqlDataAdapter(strSql, cnn)
                        dad.Fill(dtb)
                    End Using
                    cnn.Close()
                End Using
                If dtb.Rows.Count > 0 Then
                    For Each dr As DataRow In dtb.Rows
                        If dr.Item("IsActive").ToString.Contains("True") Then
                            lblError.Text = "Key Already used..."
                            el.WriteToErrorLog("Key Already used...", Constants.compname, "Apply License Key")
                            Exit For
                        Else
                            Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
                            If System.IO.File.Exists(fileName) = True Then
                                System.GC.Collect()
                                System.GC.WaitForPendingFinalizers()
                                System.IO.File.Delete(fileName)
                            End If
                            Dim fs As FileStream = File.Create(fileName)
                            For Each drrr As DataRow In dtb.Rows
                                Dim strKeyValue As String = "Key |" & txtLicKey.Text & "$" & "Macadress|" + tempMac & "$" & "HostName|" + hostName & "$" & "IPAddress|" + myIP & "$" & "OSName|" + OSName & "$" & "DaysRemaining|" + drrr.Item("NoofdaysExpiration").ToString & "$" & "LicenseGenDate|" + DateTime.Now.ToString("dd/MM/yyyy") & "$" & "MotherBoardID|" + MotherBoardID & "$" & "HardDiskID|" + HardDiskID
                                encryptedString = ClsCommon.EnryptString(strKeyValue)
                                '                              MsgBox("a")
                                Dim myconnection As SqlConnection
                                Dim mycommand As SqlCommand
                                myconnection = New SqlConnection(ClsCommon.GetConnectionstr().ToString())
                                '                             MsgBox("b")
                                '  myconnection = New SqlConnection("server=sql5042.site4now.net;database=DB_A4AB62_LicenseKey;uid=DB_A4AB62_LicenseKey_admin;pwd=meadmin81;Integrated Security=True;MultipleActiveResultSets=True")
                                myconnection.Open()
                                ' mycommand = New SqlCommand("update LicenseKeyDetails set LicenseKeyValue='" + encryptedString + "', IsActive=1,IPaddress='" + myIP + "',MacAddress='" + tempMac + "',HostName='" + hostName + "',OsName='" + OSName + "',MotherBoardID='" + MotherBoardID + "',HardDiskID='" + HardDiskID + "',ApplicationName='Aadhat',creationdate='" + DateTime.Now.ToString("YYYY-MM-DD HH:MM:SS AM/PM") + "' where LicenseKey='" + txtLicKey.Text + "'", myconnection)
                                mycommand = New SqlCommand("update LicenseKeyDetails set LicenseKeyValue='" + encryptedString + "', IsActive=1,IPaddress='" + myIP + "',MacAddress='" + tempMac + "',HostName='" + hostName + "',OsName='" + OSName + "',MotherBoardID='" + MotherBoardID + "',HardDiskID='" + HardDiskID + "',ApplicationName='Aadhat',creationdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where LicenseKey='" + txtLicKey.Text + "'", myconnection)
                                '   MsgBox("update LicenseKeyDetails set LicenseKeyValue='" + encryptedString + "', IsActive=1,IPaddress='" + myIP + "',MacAddress='" + tempMac + "',HostName='" + hostName + "',OsName='" + OSName + "',MotherBoardID='" + MotherBoardID + "',HardDiskID='" + HardDiskID + "',ApplicationName='Aadhat',creationdate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' where LicenseKey='" + txtLicKey.Text + "'")
                                mycommand.ExecuteNonQuery()
                                '                               MsgBox("C")
                                myconnection.Close()
                                Dim title As Byte() = New UTF8Encoding(True).GetBytes(encryptedString)
                                fs.Write(title, 0, title.Length)
                                fs.Close()
                                MsgBox("License Key Applied Sucessfully")
                                el.WriteToErrorLog("License Key Applied Sucessfully", Constants.compname, "Apply License Key")
                                ClsCommon.UpdateCutomerInfo("ADD")
                                Dim FILE_NAME As String = Application.StartupPath & "\Lic Key.txt"
                                Dim objWriter As New System.IO.StreamWriter(FILE_NAME)
                                objWriter.Write(Date.Today.ToString("dd-MM-yyyy") & " : " & txtLicKey.Text)
                                objWriter.Close()
                                Login.Show() : Login.BringToFront()
                                Login.BtnLogin.PerformClick()
                                Me.Dispose()
                                If Not File.Exists(fileName) Then
                                    MainScreenForm.RegistrationToolStripMenuItem.Visible = True
                                End If
                            Next
                            Exit For
                        End If
                    Next
                Else
                    lblError.Text = "Invalid Key"
                    el.WriteToErrorLog("Invalid Key", Constants.compname, "Apply License Key")
                End If
            Else
                ' lblError.Text = "Please check internet connectivity to register key"
                lblError.Text = "Please check internet connectivity to register key"
                el.WriteToErrorLog("Please check internet connectivity to register key", Constants.compname, "Apply License Key")
            End If
        Catch
            lblError.Text = "Something Went Wrong.."
            el.WriteToErrorLog("Something Went Wrong..", Constants.compname, "Apply License Key")
            '  lblError.Text = "Something went wrong.."
        End Try

    End Sub

    Private Sub ApplyLicenseKey_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close() : Login.Focus() : Exit Sub
    End Sub
    Private Sub ApplyLicenseKey_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 130 : Me.Left = 84
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.GhostWhite : Me.KeyPreview = True
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        SystemInfo.MdiParent = ShowCompanies
        SystemInfo.Show()
        SystemInfo.BringToFront()
    End Sub
End Class