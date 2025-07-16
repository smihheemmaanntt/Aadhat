Imports System.Data.SqlClient
Imports System.IO
Imports System.Management
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Threading

Public Class CommonClass
    Public Function LicenseCheck(ByVal fileName As String) As Boolean
        Dim ClsCommon As CommonClass = New CommonClass()
        'If ClsCommon.IsInternetConnect() = False Then Exit Function
        Dim decryptedString As String = String.Empty
        Dim DaysRemain As String = String.Empty
        Dim LicenseGenerate As String = String.Empty
        Dim Daysdiff As String = String.Empty
        Dim LictempMac As String = String.Empty
        Dim LicOSName As String = String.Empty
        Dim LichostName As String = String.Empty
        Dim LicmyIP As String = String.Empty
        Dim LicMotherBoardID As String = String.Empty
        Dim LicHardDiskID As String = String.Empty

        Using sr As StreamReader = File.OpenText(fileName)
            Dim s As String = ""
            Dim formats As String() = {"dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "dd-MM-yyyy"} ' Add more formats as needed
            Dim LicenseGenerateDate As DateTime

            Do
                s = sr.ReadLine()
                If s Is Nothing Then Exit Do
                decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(s))
                Dim LicenseValueList As String() = decryptedString.Split("$"c)
                DaysRemain = LicenseValueList(5).ToString().Split("|"c)(1)
                LicenseGenerate = LicenseValueList(6).ToString().Split("|"c)(1)
                ' Try to parse LicenseGenerate into a DateTime using known formats
                If DateTime.TryParseExact(LicenseGenerate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, LicenseGenerateDate) Then
                    Daysdiff = (DateTime.Now.Date - LicenseGenerateDate.Date).TotalDays.ToString()
                    Dim remain As Integer = Val(DaysRemain) - Val(Daysdiff)
                    MainScreenForm.lblARC.Text = "ARC Expiry in Next " & remain & " Days"
                    If remain <= 10 Then
                        ' MsgBox("ARC Expiry in Next " & remain & " Days", MsgBoxStyle.Critical, "ARC Expiry")
                        MainScreenForm.lblARC.ForeColor = Color.Red
                        MainScreenForm.lblARC.Visible = True
                        MainScreenForm.blinkTimer.Interval = 500
                        MainScreenForm.blinkTimer.Start()
                    Else
                        MainScreenForm.blinkTimer.Stop()
                        MainScreenForm.lblARC.Visible = True
                        MainScreenForm.lblARC.ForeColor = Color.White
                    End If
                Else
                    MessageBox.Show("Invalid date format in LicenseGenerate: " & LicenseGenerate)
                End If

                LictempMac = LicenseValueList(1).ToString().Split("|"c)(1)
                LicOSName = LicenseValueList(4).ToString().Split("|"c)(1)
                LichostName = LicenseValueList(2).ToString().Split("|"c)(1)
                LicmyIP = LicenseValueList(3).ToString().Split("|"c)(1)
                LicMotherBoardID = LicenseValueList(7).ToString().Split("|"c)(1)
                LicHardDiskID = LicenseValueList(8).ToString().Split("|"c)(1)

            Loop
        End Using

        If String.IsNullOrEmpty(DaysRemain) Or String.IsNullOrEmpty(LicenseGenerate) Then
            MsgBox("License is Invalid for this machine", MsgBoxStyle.Critical, "Invalid License")
            Return False
        End If

        If Convert.ToInt32(Daysdiff) > Convert.ToInt32(DaysRemain) Then
            MsgBox("Your License is expired", MsgBoxStyle.Critical, "Expired")
            ApplyLicenseKey.MdiParent = ShowCompanies
            ApplyLicenseKey.Show()
            ApplyLicenseKey.BringToFront()
            Return False
        Else
            Dim tempMac As String = String.Empty
            Dim OSName As String = String.Empty
            Dim hostName As String = String.Empty
            Dim myIP As String = String.Empty
            Dim MotherBoardID As String = String.Empty
            Dim HardDiskID As String = String.Empty
            hostName = ClsCommon.GetHostName()
            ' myIP = ClsCommon.GetIPAddress()
            tempMac = ClsCommon.GetMacAddress()
            OSName = ClsCommon.GetOSName()
            MotherBoardID = ClsCommon.MotherboardSerialNumber()
            HardDiskID = ClsCommon.HardDiskSerialNumber()
            'If (hostName = LichostName And myIP = LicmyIP And tempMac = LictempMac And OSName = LicOSName) Then
            ' If (MotherBoardID = LicMotherBoardID And HardDiskID = LicHardDiskID) Then
            If (MotherBoardID = LicMotherBoardID) Then
                Return True
            Else
                MsgBox("License is Invalid for this machine", MsgBoxStyle.Critical, "Invalid License")
                Return False
            End If
        End If
    End Function

    Public Function GetMacAddress() As String
        Dim qstring As String = "Select * FROM Win32_NetworkAdapterConfiguration where IPEnabled = true"
        For Each mo As System.Management.ManagementObject In New System.Management.ManagementObjectSearcher(qstring).Get()
            Dim macaddress As String = mo("MacAddress")
            If Not macaddress Is Nothing Then
                Return macaddress
            End If
        Next
        Return ""
    End Function

    Public Function GetHostName() As String
        Dim hostName As String = Dns.GetHostName()
        Return hostName
    End Function

    'Public Function GetIPAddress() As String
    '    Dim hostName As String = Dns.GetHostName()
    '    Dim myIP As String = Dns.GetHostByName(hostName).AddressList(0).ToString()
    '    Return myIP
    'End Function

    Public Function GetOSName() As String
        Dim Full_Os_Name As String = My.Computer.Info.OSFullName
        Return Full_Os_Name
    End Function


    'Public Function IsInternetConnect() As Boolean
    '    If Not My.Computer.Network.IsAvailable Then
    '        Return False
    '    End If

    '    Dim pingSuccess As Boolean = False
    '    Dim waitHandle As New ManualResetEvent(False)

    '    Try
    '        ' Run both pings in parallel using ThreadPool
    '        ThreadPool.QueueUserWorkItem(AddressOf CheckPingGoogle, New Object() {waitHandle, pingSuccess})
    '        ThreadPool.QueueUserWorkItem(AddressOf CheckPingServer, New Object() {waitHandle, pingSuccess})

    '        ' Wait for any one of the pings to succeed (maximum 3 seconds to avoid hanging)
    '        waitHandle.WaitOne(3000)

    '        ' Return true if either of the two checks succeeded
    '        Return pingSuccess
    '    Catch ex As Exception
    '        ' Log the error if needed
    '    End Try

    '    Return False
    'End Function

    'Private Sub CheckPingGoogle(state As Object)
    '    Dim parameters = DirectCast(state, Object())
    '    Dim waitHandle As ManualResetEvent = DirectCast(parameters(0), ManualResetEvent)
    '    Dim pingSuccess As Boolean = DirectCast(parameters(1), Boolean)
    '    CheckPing("8.8.8.8", waitHandle, pingSuccess)
    'End Sub

    'Private Sub CheckPingServer(state As Object)
    '    Dim parameters = DirectCast(state, Object())
    '    Dim waitHandle As ManualResetEvent = DirectCast(parameters(0), ManualResetEvent)
    '    Dim pingSuccess As Boolean = DirectCast(parameters(1), Boolean)
    '    CheckPing("103.199.214.72", waitHandle, pingSuccess)
    'End Sub

    'Private Sub CheckPing(host As String, waitHandle As ManualResetEvent, ByRef pingSuccess As Boolean)
    '    Try
    '        Using ping As New Ping()
    '            ' Ping with a short timeout (1000 ms)
    '            Dim pingReply = ping.Send(host, 1000)
    '            If pingReply.Status = IPStatus.Success Then
    '                pingSuccess = True
    '                waitHandle.Set() ' Signal the main thread that a ping succeeded
    '            End If
    '        End Using
    '    Catch ex As Exception
    '        ' Log the error if needed
    '    End Try
    'End Sub



    Public Function IsInternetConnect() As Boolean
        If My.Computer.Network.IsAvailable Then
            Try
                Using ping As New Ping()
                    Dim pingReply1 = ping.Send("8.8.8.8")
                    ' Dim pingReply2 = ping.Send("103.199.214.72")
                    If (pingReply1.Status = IPStatus.Success) Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            Catch ex As Exception
                ' Log the exception if needed
            End Try
        End If
        Return False
    End Function



    Public Function EnryptString(ByVal strEncrypted As String) As String
        Dim b As Byte() = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted)
        Dim encrypted As String = Convert.ToBase64String(b)
        Return encrypted
    End Function

    Public Function DecodeBase64(input As String) As String
        Return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(input))
    End Function

    'Public Function MotherboardSerialNumber() As String
    '    Dim value As String = ""
    '    Dim baseBoard As ManagementClass = New ManagementClass("Win32_BaseBoard")
    '    '  MsgBox("BASEBOARD Error 1")
    '    Dim board As ManagementObjectCollection = baseBoard.GetInstances()
    '    ' MsgBox("BASEBOARD Error 2")
    '    If board.Count > 0 Then
    '        value = board(0)("SerialNumber")
    '        'MsgBox(value)
    '        If value.Length > 0 Then value = value.Substring(2)
    '    End If
    '    Return value
    'End Function

    'Public Function MotherboardSerialNumber() As String
    '    Dim q As New SelectQuery("Win32_bios")
    '    Dim search As New ManagementObjectSearcher(q)
    '    Dim info As New ManagementObject
    '    Dim value As String = ""
    '    For Each info In search.Get
    '        value = info("serialnumber").ToString
    '        '  MessageBox.Show("Serial Number: " & info("serialnumber").ToString & vbNewLine & vbNewLine & "Bios Version: " & info("version").ToString)
    '    Next
    '    Return value
    '    'Dim baseBoard As ManagementClass = New ManagementClass("Win32_BaseBoard")
    '    ''  MsgBox("BASEBOARD Error 1")
    '    'Dim board As ManagementObjectCollection = baseBoard.GetInstances()
    '    '' MsgBox("BASEBOARD Error 2")
    '    'If board.Count > 0 Then
    '    '    value = board(0)("SerialNumber")
    '    '    'MsgBox(value)
    '    '    If value.Length > 0 Then value = value.Substring(2)
    '    'End If
    '    'Return value
    'End Function


    Public Function MotherboardSerialNumber() As String
        Dim value As String = ""
        Dim baseBoard As ManagementClass = New ManagementClass("Win32_BaseBoard")
        Dim board As ManagementObjectCollection = baseBoard.GetInstances()

        If board.Count > 0 Then
            '  MsgBox("a")
            Dim serialNumber As String = board(0)("SerialNumber").ToString()
            '    MsgBox(serialNumber)
            If serialNumber.Length >= 3 Then
                '    MsgBox("b")
                value = serialNumber.Substring(2)
            End If
        End If

        Return value
    End Function

    Public Function HardDiskSerialNumber() As String
        Dim HDD_Serial As String = String.Empty
        'MsgBox("HDD Error")
        Dim hdd As New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")
        'MsgBox("HDD Error 2")
        For Each hd In hdd.Get
            HDD_Serial = hd("SerialNumber")
        Next
        Return HDD_Serial
    End Function
    Public Function GetServerIP() As String
        Dim Url As String = "https://softmanagementindia.in/api/ServerIP.txt"
        Dim wc As New WebClient()
        Dim stream As Stream = wc.OpenRead(Url)
        Dim reader As New StreamReader(stream)
        Dim ServerIP As String = reader.ReadToEnd()
        reader.Close()
        stream.Close()
        Return ServerIP
    End Function
    Public Function UpdateCutomerInfo(ByVal Operation As String) As String
        Dim dts As New DataTable
        Dim decryptedString As String = String.Empty
        Dim CustID As Integer
        Dim LicenseKey As String = String.Empty
        Dim conn As SqlConnection
        Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
        ' clsFun.changeCompany()
        dts = clsFun.ExecDataTable("Select * From Company Where ID = " & Val(sCompCode) & "")
        Try
            If dts.Rows.Count > 0 Then
                CustID = Val(dts.Rows(0)("ID").ToString())
                LicenseKey = GetLicenseKey()
                conn = GetConnectionstring()
                conn.Open()
                Dim cmd As New SqlCommand("InsertingUpdating_customer", conn)
                Try
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@LicenseKey", SqlDbType.VarChar)
                    cmd.Parameters("@LicenseKey").Value = LicenseKey
                    cmd.Parameters.Add("@Operation", SqlDbType.VarChar)
                    cmd.Parameters("@Operation").Value = Operation
                    cmd.Parameters.Add("@CompanyID", SqlDbType.VarChar)
                    cmd.Parameters("@CompanyID").Value = CustID
                    cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar)
                    cmd.Parameters("@CompanyName").Value = dts.Rows(0)("CompanyName").ToString()
                    cmd.Parameters.Add("@Address", SqlDbType.VarChar)
                    cmd.Parameters("@Address").Value = dts.Rows(0)("Address").ToString()
                    cmd.Parameters.Add("@City", SqlDbType.VarChar)
                    cmd.Parameters("@City").Value = dts.Rows(0)("City").ToString()
                    cmd.Parameters.Add("@State", SqlDbType.VarChar)
                    cmd.Parameters("@State").Value = dts.Rows(0)("State").ToString()
                    cmd.Parameters.Add("@StateCode", SqlDbType.VarChar)
                    cmd.Parameters("@StateCode").Value = 0
                    cmd.Parameters.Add("@MobileNo1", SqlDbType.VarChar)
                    cmd.Parameters("@MobileNo1").Value = dts.Rows(0)("MobileNo1").ToString()
                    cmd.Parameters.Add("@MobileNo2", SqlDbType.VarChar)
                    cmd.Parameters("@MobileNo2").Value = dts.Rows(0)("MobileNo2").ToString()
                    cmd.Parameters.Add("@PhoneNo", SqlDbType.VarChar)
                    cmd.Parameters("@PhoneNo").Value = dts.Rows(0)("PhoneNo").ToString()
                    cmd.Parameters.Add("@FaxNo", SqlDbType.VarChar)
                    cmd.Parameters("@FaxNo").Value = dts.Rows(0)("FaxNo").ToString()
                    cmd.Parameters.Add("@EmailID", SqlDbType.VarChar)
                    cmd.Parameters("@EmailID").Value = dts.Rows(0)("EmailID").ToString()
                    cmd.Parameters.Add("@Website", SqlDbType.VarChar)
                    cmd.Parameters("@Website").Value = dts.Rows(0)("Website").ToString()
                    cmd.Parameters.Add("@GSTN", SqlDbType.VarChar)
                    cmd.Parameters("@GSTN").Value = dts.Rows(0)("GSTN").ToString()
                    cmd.Parameters.Add("@RegistrationNo", SqlDbType.VarChar)
                    cmd.Parameters("@RegistrationNo").Value = dts.Rows(0)("RegistrationNo").ToString()
                    cmd.Parameters.Add("@PanNo", SqlDbType.VarChar)
                    cmd.Parameters("@PanNo").Value = dts.Rows(0)("PanNo").ToString()
                    cmd.Parameters.Add("@YearStart", SqlDbType.VarChar)
                    cmd.Parameters("@YearStart").Value = dts.Rows(0)("YearStart").ToString()
                    cmd.Parameters.Add("@Yearend", SqlDbType.VarChar)
                    cmd.Parameters("@Yearend").Value = dts.Rows(0)("Yearend").ToString()
                    cmd.Parameters.Add("@tag", SqlDbType.VarChar)
                    cmd.Parameters("@tag").Value = dts.Rows(0)("tag").ToString()
                    cmd.Parameters.Add("@ZipNo", SqlDbType.VarChar)
                    cmd.Parameters("@ZipNo").Value = 0
                    cmd.ExecuteNonQuery()
                Finally
                    If cmd IsNot Nothing Then cmd.Dispose()
                    If conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed Then conn.Close()
                End Try
            End If
            dts.Dispose()

        Catch ex As Exception
            MsgBox("Something went wrong", MsgBoxStyle.Critical, "Access Denied")
        End Try
        Return ""
    End Function

    Public Function IsLicenseBlocked() As Boolean
        Try
            If My.Computer.Network.IsAvailable = False Then
                Dim dt As New DataTable : ClsFunPrimary.changeCompany()
                dt = clsFun.ExecDataTable("Select IsAssign from accent")
                Try
                    If dt.Rows.Count > 0 Then
                        If Convert.ToBoolean(dt.Rows(0)("IsAssign").ToString()) Then
                            Return True
                        Else
                            Return False
                        End If
                    End If
                Catch
                End Try
                Return False
                Exit Function
            End If
            If IsInternetConnect() = True Then
                Try
                    Dim strSql As String = "Select IsBlocked from LicenseKeyDetails where LicenseKey='" + GetLicenseKey() + "'"
                    Dim dtb As New DataTable
                    Using cnn As New SqlConnection(Convert.ToString(GetConnectionstr()))
                        cnn.Open()
                        Using dad As New SqlDataAdapter(strSql, cnn)
                            dad.Fill(dtb)
                        End Using
                        cnn.Close()
                    End Using
                    If dtb.Rows.Count > 0 Then
                        'clsFun.changeCompany()
                        For Each dr As DataRow In dtb.Rows
                            If dr.Item("IsBlocked").ToString.Contains("True") Then
                                Dim sql As String = "Update accent SET IsAssign=1"
                                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                                ClsFunPrimary.ExecNonQuery(sql)
                                Return True
                            Else
                                Dim sql As String = "Update accent SET IsAssign=0"
                                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                                ClsFunPrimary.ExecNonQuery(sql)
                                Return False
                            End If
                        Next
                    End If
                Catch
                End Try
            End If
            Return False
        Catch
            Dim dt As New DataTable
            dt = ClsFunPrimary.ExecDataTable("Select IsAssign from accent")
            Try
                If dt.Rows.Count > 0 Then
                    If Convert.ToBoolean(dt.Rows(0)("IsAssign").ToString()) Then
                        Return True
                    Else
                        Return False
                    End If

                End If
            Catch
            End Try
            Return False
        End Try
        Return False
    End Function

    Public Function GetLicenseKey() As String
        Dim LicenseKey As String = String.Empty
        Dim decryptedString As String = String.Empty
        Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
        Using sr As StreamReader = File.OpenText(fileName)
            Dim s As String = ""

            Do
                s = sr.ReadLine()
                If s Is Nothing Then Exit Do
                decryptedString = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(s))
                Dim LicenseValueList As String() = decryptedString.Split("$"c)
                LicenseKey = LicenseValueList(0).ToString().Split("|"c)(1)
            Loop
        End Using
        Return LicenseKey
    End Function

    Public Function GetConnectionstr() As String
        Dim conn As String
        conn = "Data Source=103.199.214.72;Initial Catalog=smicloud_smilic;Persist Security Info=True;User ID=smiadmin;Password=admin@123"
        Return conn
    End Function

    Public Function GetConnectionstring() As SqlConnection
        Dim conn As SqlConnection
        conn = New SqlConnection("Data Source=103.199.214.72;Initial Catalog=smicloud_smilic;Persist Security Info=True;User ID=smiadmin;Password=admin@123")
        Return conn
    End Function

    Public Function GetNewConnection() As SqlConnection
        Dim conn As SqlConnection
        conn = New SqlConnection("Data Source=103.199.214.72;Initial Catalog=smicloud_Aadhat;Persist Security Info=True;User ID=smiadmin;Password=admin@123")
        Return conn
    End Function

    Public Function GenrateID()
        Dim conn As SqlConnection
        conn = GetConnectionstring()
        Dim rnd As New Random()
        Mobile_App.txtCompanyID.Text = (rnd.Next(100000, 999999))
        If clsFun.ExecScalarInt("Select OrganizationID From Company  WHERE OrganizationID='" & Mobile_App.txtCompanyID.Text & "'") > 1 Then
            Mobile_App.txtCompanyID.Text = (rnd.Next(100000, 999999))
        End If
        Return Mobile_App.txtCompanyID.Text
    End Function

    Public Function UpdateCompanyInfo(ByVal Operation As String) As String
        Dim dts As New DataTable
        Dim decryptedString As String = String.Empty
        Dim CustID As Integer
        Dim LicenseKey As String = String.Empty
        Dim conn As SqlConnection
        Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
        '  clsFun.ChangePath(Data)
        dts = clsFun.ExecDataTable("Select * From Company Where OrganizationID = " & Val(Mobile_App.txtCompanyID.Text) & "")
        Try
            If dts.Rows.Count > 0 Then
                CustID = Val(dts.Rows(0)("ID").ToString())
                LicenseKey = GetLicenseKey()
                conn = GetNewConnection()
                conn.Open()
                Dim cmd As New SqlCommand("InsertingUpdating_customer", conn)
                Try
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@LicenseKey", SqlDbType.VarChar)
                    cmd.Parameters("@LicenseKey").Value = LicenseKey
                    cmd.Parameters.Add("@Operation", SqlDbType.VarChar)
                    cmd.Parameters("@Operation").Value = Operation
                    cmd.Parameters.Add("@CompanyID", SqlDbType.VarChar)
                    cmd.Parameters("@CompanyID").Value = CustID
                    cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar)
                    cmd.Parameters("@CompanyName").Value = dts.Rows(0)("CompanyName").ToString()
                    cmd.Parameters.Add("@Address", SqlDbType.VarChar)
                    cmd.Parameters("@Address").Value = dts.Rows(0)("Address").ToString()
                    cmd.Parameters.Add("@City", SqlDbType.VarChar)
                    cmd.Parameters("@City").Value = dts.Rows(0)("City").ToString()
                    cmd.Parameters.Add("@State", SqlDbType.VarChar)
                    cmd.Parameters("@State").Value = dts.Rows(0)("State").ToString()
                    cmd.Parameters.Add("@StateCode", SqlDbType.VarChar)
                    cmd.Parameters("@StateCode").Value = 0
                    cmd.Parameters.Add("@MobileNo1", SqlDbType.VarChar)
                    cmd.Parameters("@MobileNo1").Value = dts.Rows(0)("MobileNo1").ToString()
                    cmd.Parameters.Add("@MobileNo2", SqlDbType.VarChar)
                    cmd.Parameters("@MobileNo2").Value = dts.Rows(0)("MobileNo2").ToString()
                    cmd.Parameters.Add("@PhoneNo", SqlDbType.VarChar)
                    cmd.Parameters("@PhoneNo").Value = dts.Rows(0)("PhoneNo").ToString()
                    cmd.Parameters.Add("@FaxNo", SqlDbType.VarChar)
                    cmd.Parameters("@FaxNo").Value = dts.Rows(0)("FaxNo").ToString()
                    cmd.Parameters.Add("@EmailID", SqlDbType.VarChar)
                    cmd.Parameters("@EmailID").Value = dts.Rows(0)("EmailID").ToString()
                    cmd.Parameters.Add("@Website", SqlDbType.VarChar)
                    cmd.Parameters("@Website").Value = dts.Rows(0)("Website").ToString()
                    cmd.Parameters.Add("@GSTN", SqlDbType.VarChar)
                    cmd.Parameters("@GSTN").Value = dts.Rows(0)("GSTN").ToString()
                    cmd.Parameters.Add("@RegistrationNo", SqlDbType.VarChar)
                    cmd.Parameters("@RegistrationNo").Value = dts.Rows(0)("RegistrationNo").ToString()
                    cmd.Parameters.Add("@PanNo", SqlDbType.VarChar)
                    cmd.Parameters("@PanNo").Value = dts.Rows(0)("PanNo").ToString()
                    cmd.Parameters.Add("@YearStart", SqlDbType.VarChar)
                    cmd.Parameters("@YearStart").Value = dts.Rows(0)("YearStart").ToString()
                    cmd.Parameters.Add("@Yearend", SqlDbType.VarChar)
                    cmd.Parameters("@Yearend").Value = dts.Rows(0)("Yearend").ToString()
                    cmd.Parameters.Add("@tag", SqlDbType.VarChar)
                    cmd.Parameters("@tag").Value = dts.Rows(0)("tag").ToString()
                    cmd.Parameters.Add("@ZipNo", SqlDbType.VarChar)
                    cmd.Parameters("@ZipNo").Value = 0
                    cmd.Parameters.Add("@OrganisationID", SqlDbType.Int)
                    cmd.Parameters("@OrganisationID").Value = dts.Rows(0)("OrganisationID").ToString()
                    cmd.ExecuteNonQuery()
                Finally
                    If cmd IsNot Nothing Then cmd.Dispose()
                    If conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed Then conn.Close()
                End Try
            End If
            dts.Dispose()

        Catch ex As Exception
            MsgBox("Something went wrong", MsgBoxStyle.Critical, "Access Denied")
        End Try

        Return ""
    End Function
End Class
