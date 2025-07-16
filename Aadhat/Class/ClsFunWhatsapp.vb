Imports System.Data.SQLite
Imports System.Globalization

Public Class ClsFunWhatsapp
    Public Shared ConString As String = "Data Source=" & Application.StartupPath & "\Whatsapp\Data\Whatsapp.db;Version=3;New=True;Compress=True;synchronous=ON;"
    'Public Shared ConString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\Data\db.accdb;Persist Security Info=true;password=smi3933"
    'Shared con As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source =" & Application.StartupPath & "\Data\db.accdb")
    'Shared con As New OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings("Con").ConnectionString)
    Public Shared con As New SQLite.SQLiteConnection
    Public Shared Function GetConnection() As SQLiteConnection
        Dim cs As String = ConString
        con = New SQLiteConnection(cs)
        con.Open()
        Return con
    End Function
    'Public Function GetNewConnection() As SQLiteConnection
    '    Dim conn As SQLiteConnection
    '    conn = New SQLiteConnection("Data Source=|DataDirectory|\Data" & Data & ";Version=3;New=True;Compress=True;synchronous=ON;")
    '    Return conn
    'End Function
    Public Shared Sub CloseConnection()
        con.Close()
    End Sub
    Public Function GetMDYFrmCustDt(ByVal CustDate As String) As String
        Dim tmp As Date = DateTime.ParseExact(CustDate, "dd-MM-yyyy", DateTimeFormatInfo.CurrentInfo)
        GetMDYFrmCustDt = GetMDYFrmSysDt(tmp)
    End Function
    Public Function GetMDYFrmSysDt(ByVal SysDate As Date) As String
        Try
            GetMDYFrmSysDt = SysDate.ToString("MM-dd-yyyy")
        Catch ex As Exception
            GetMDYFrmSysDt = SysDate.ToString("MM-dd-yyyy")
        End Try
    End Function
    Public Shared Function ExecDataTable(ByVal cmdText As String) As DataTable
        Dim ad As New SQLiteDataAdapter(cmdText, ConString)
        Dim dt As DataTable = New DataTable()
        ad.Fill(dt)
        ad.Dispose()
        Return dt
    End Function
    Public Shared Function ExecDataSet(ByVal cmdText As String, Optional ByVal tmptblName As String = "a") As DataSet

        Dim ad As New SQLiteDataAdapter(cmdText, ConString)
        Dim ds As DataSet = New DataSet()
        ad.Fill(ds, tmptblName)
        ad.Dispose()
        Return ds
    End Function
    Public Shared Function DateValidate(ByVal Date1 As String)
        'Check If Day is entered.
        If Len(Date1) <> 10 Then
            If Date1.Substring(0, 2).Trim <> "" Then
                'Check if Month is entered.
                If Date1.Substring(3, 2).Trim <> "" Then
                    'Check if year is not entered.
                    If Date1.Length = 6 And IsDate(Date1.Substring(0, 2).Trim & "-" & Date1.Substring(3, 2).Trim & "-" & Now.Year) Then
                        'Check if Date and month entered are valid.
                        If IsDate(Date1 & Now.Year) Then
                            Date1 = Date1 & Now.Year
                        End If
                    Else
                        Dim thisMonth As New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                        'last day of the month
                        Date1 = thisMonth.AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy").ToString
                    End If
                Else
                    'Check if Date entered will be valid for the current date that we want to create?
                    If IsDate(Date1.Substring(0, 2).Trim & "-" & Now.Month & "-" & Now.Year) Then
                        'Check if month is two digit?
                        If Now.Month.ToString.Length < 2 Then
                            Date1 = Date1.Substring(0, 2).Trim & "-0" & Now.Month & "-" & Now.Year
                        Else
                            Date1 = Date1.Substring(0, 2).Trim & "-" & Now.Month & "-" & Now.Year
                        End If
                    Else
                        Dim thisMonth As New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                        'last day of the month
                        Date1 = thisMonth.AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy").ToString
                    End If
                End If
            Else
                Dim TodaysDay As String = Now.Day
                Dim TodaysMonth As String = Now.Month
                'Check if Current day is two digit?
                If Now.Day < 10 Then
                    TodaysDay = "0" & Now.Day
                ElseIf Now.Day > 31 Then
                    TodaysDay = Now.Day
                End If
                'Check if Current Month is two digit?
                If Now.Month < 10 Then
                    TodaysMonth = "0" & Now.Month
                ElseIf Now.Month > 12 Then
                    TodaysMonth = "0" & Now.Month
                End If
                Date1 = TodaysDay & "-" & TodaysMonth & "-" & Now.Year
            End If
        End If
        DateValidate = Date1
    End Function
    Public Shared Function ExecNonQuery(ByVal cmdText As String, Optional ByVal withTran As Boolean = False) As Integer
        Dim cmd As SQLiteCommand = Nothing
        Dim nUpdatedRows As Integer = 0
        Dim Trans As SQLiteTransaction = Nothing
        Dim Con As SQLiteConnection = New SQLiteConnection(ConString)
        Con.Open()
        If withTran = False Then
            cmd = New SQLiteCommand(cmdText, Con)
            nUpdatedRows = cmd.ExecuteNonQuery()
        Else
            Trans = Con.BeginTransaction()
            cmd = New SQLiteCommand(cmdText, Con, Trans)
            cmd.CommandTimeout = 7000
            nUpdatedRows = cmd.ExecuteNonQuery()
            Trans.Commit()
            Trans.Dispose()
        End If
        cmd.Dispose()
        Con.Dispose()
        Return nUpdatedRows
    End Function

    Public Shared Function ExecScalarStr(ByVal cmdText As String) As String
        Dim Result As String = String.Empty
        Dim Con As SQLiteConnection = New SQLiteConnection(ConString)
        Con.Open()
        Dim cmd As SQLiteCommand = New SQLiteCommand(cmdText, Con)
        Dim obj As Object = cmd.ExecuteScalar()
        cmd.Dispose()
        Con.Dispose()
        Return ToStr(obj)
    End Function

    Public Shared Function ExecScalarInt(ByVal cmdText As String) As Integer
        Return ToInt(ExecScalarStr(cmdText))
    End Function

    Public Shared Function ExecScalarDec(ByVal cmdText As String) As Decimal
        Return Convert.ToDecimal(ExecScalarStr(cmdText))
    End Function
    Public Shared Function ToInt(ByVal Val As Object) As Integer
        Dim Result As Integer = 0
        Try
            Result = Convert.ToInt32(Convert.ToDecimal(Val))
        Catch ex As Exception
            Result = 0
        End Try
        Return Result
    End Function

    Public Shared Function GetServerDate() As String
        Dim Result As String = String.Empty
        ''Result = ExecScalarStr("Select CONVERT(varchar, DATEADD(MINUTE, 693, GETDATE()), 103)")
        Result = ExecScalarStr("Select date('now')")
        '' Result = Result.Replace("-", "/")
        Return Result
    End Function

    Public Shared Function GetServerDateTime() As Date
        Dim Result As String = String.Empty
        Result = ExecScalarStr("Select EntryDate()")
        '' Result = Result.Replace("-", "/")
        Return Convert.ToDateTime(Result)
    End Function
    Public Shared Sub FillDropDownList(ByRef ddl As ComboBox, ByVal cmdText As String, ByVal sTextField As String, ByVal sValueField As String, ByVal sDefaultValue As String)
        Dim dt As DataTable = New DataTable()
        ' ddl.Items.Clear()
        dt = ExecDataTable(cmdText)
        If sDefaultValue.Trim <> "" Then
            Dim row As DataRow = dt.NewRow()
            row(0) = 0
            row(1) = sDefaultValue
            dt.Rows.InsertAt(row, 0)
        End If
        ddl.DataSource = dt
        ddl.DisplayMember = sTextField
        ddl.ValueMember = sValueField
        If ddl.Items.Count > 0 Then
            ddl.SelectedIndex = 0
        End If
    End Sub
    Public Shared Function ToStr(ByVal Val As Object) As String
        Dim Result As String = String.Empty
        Try
            Result = Val.ToString()
        Catch ex As Exception
            Result = String.Empty
        End Try
        Return Result
    End Function
    ''Insert data into Ledger
    Public Shared Function Ledger(ByVal Opt As Integer, ByVal vchid As Integer, ByVal entryDt As String, ByVal TransType As String, ByVal Acid As Integer, ByVal AcName As String, ByVal Amt As Double, ByVal Dc As String, Optional ByVal Remarks As String = "", Optional ByVal Narration As String = "") As Integer
        Ledger = 0
        Dim sSql As String = String.Empty
        If Opt = 0 Then
            sSql = "Insert into Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration)" & _
                " values(" & vchid & ",'" & entryDt & "','" & TransType & "'," & Acid & ",'" & AcName & "'," & Amt & ",'" & Dc & "','" & Remarks & "','" & Narration & "')"
            Ledger = ExecNonQuery(sSql)
            'ElseIf Opt = 1 Then
            '    sSql = "Update Ledger SET EntryDate='" & entryDt & "',TransType='" & TransType & "',AccountID='" & Acid & "',AccountName='" & AcName & "',Amount=" & Amt & ",DC='" & Dc & "',Remark='" & Remarks & "' where VourchersID=" & vchid & ""
            '    Ledger = ExecNonQuery(sSql)
        End If
        Return Ledger
    End Function
    Public Shared Function CrateLedger(ByVal Opt As Integer, ByVal vchid As String, ByVal Slip As Integer, ByVal entryDt As String, ByVal TransType As String, ByVal Acid As Integer, ByVal AcName As String, ByVal CrateType As String, ByVal CrateID As Integer, ByVal MarkaName As String, ByVal qty As Integer, ByVal Remark As String) As Integer
        CrateLedger = 0
        Dim sSql As String = String.Empty
        If Opt = 0 Then
            sSql = "Insert into CrateVoucher(VoucherID,SlipNo, EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName, Qty,Remark)" & _
                " values(" & vchid & "," & Slip & ",'" & entryDt & "','" & TransType & "'," & Acid & ",'" & AcName & "','" & CrateType & "'," & CrateID & ",'" & MarkaName & "'," & qty & ",'" & Remark & "')"
            CrateLedger = ExecNonQuery(sSql)
            'ElseIf Opt = 1 Then
            '    sSql = "Update Ledger SET EntryDate='" & entryDt & "',TransType='" & TransType & "',AccountID='" & Acid & "',AccountName='" & AcName & "',Amount=" & Amt & ",DC='" & Dc & "',Remark='" & Remarks & "' where VourchersID=" & vchid & ""
            '    Ledger = ExecNonQuery(sSql)
        End If

        Return CrateLedger
    End Function
    'Public Shared Function changeCompany()
    '    'ConStringucting connection string from the inputs
    '    Try
    '        Dim Con As New StringBuilder("Data Source=|DataDirectory|\dbp.db;Version=3;New=True;Compress=True;synchronous=ON;")

    '        'Con.Append("Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\")
    '        'Con.Append("dbp.mdb;Jet OLEDB:Database Password=")

    '        Dim strCon As String = Con.ToString()

    '        updateConfigFile(strCon)
    '        'Create new sql connection
    '        Dim Db As New SQLiteConnection()
    '        'to refresh connection string each time else it will use             previous connection string
    '        ConfigurationManager.RefreshSection("connectionStrings")
    '        Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
    '        ClsFunPrimary.ConString = Db.ConnectionString
    '    Catch E As Exception
    '        MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
    '    End Try

    'End Function

    'Public Shared Function xmldata()
    '    'ConStringucting connection string from the inputs
    '    Try
    '        Dim Con As New StringBuilder("Data Source=|DataDirectory|\Product.Xml")

    '        'Con.Append("Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\")
    '        'Con.Append("dbp.mdb;Jet OLEDB:Database Password=")

    '        Dim strCon As String = Con.ToString()

    '        updateConfigFile(strCon)
    '        'Create new sql connection
    '        Dim Db As New SQLiteConnection()
    '        'to refresh connection string each time else it will use             previous connection string
    '        ConfigurationManager.RefreshSection("connectionStrings")
    '        Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
    '        ClsFunPrimary.ConString = Db.ConnectionString
    '    Catch E As Exception
    '        MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
    '    End Try

    'End Function
    'Public Shared Function updateConfigFile(con As String)
    '    'updating config file
    '    Dim XmlDoc As New XmlDocument()
    '    'Loading the Config file
    '    XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
    '    For Each xElement As XmlElement In XmlDoc.DocumentElement
    '        If xElement.Name = "connectionStrings" Then
    '            'setting the coonection string
    '            xElement.FirstChild.Attributes(1).Value = con
    '        End If
    '    Next
    '    'writing the connection string in config file
    '    XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
    'End Function
    'Public Shared Function ChangePath(ByVal Data As String)
    '    Try
    '        'ConStringucting connection string from the inputs
    '        'Dim Con As New StringBuilder("Provider=")
    '        'Con.Append("Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\")
    '        'Con.Append(Data & ";Jet OLEDB:Database Password=smi3933")
    '        Dim Con As New StringBuilder("Data Source=|DataDirectory|\" & Data & ";Version=3;New=True;Compress=True;synchronous=ON;")
    '        'Dim Con As New StringBuilder("Data Source=|DataDirectory|\" & Data & ";Version=3;Password=smi3933;")
    '        'Con.Append("Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\")
    '        'Con.Append(Data & ";Jet OLEDB:Database Password=smi3933")

    '        Dim strCon As String = Con.ToString()

    '        updateConfigFile(strCon)
    '        'Create new sql connection
    '        Dim Db As New SQLiteConnection()
    '        'to refresh connection string each time else it will use             previous connection string
    '        ConfigurationManager.RefreshSection("connectionStrings")
    '        Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
    '        ClsFunPrimary.ConString = Db.ConnectionString
    '    Catch E As Exception
    '        MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
    '    End Try
    'End Function


    Public Shared Function GetFirstDateOfMonth(ByVal iMonth As Integer)
        Dim dtFrom As DateTime = New DateTime(DateTime.Now.Year, iMonth, 1)
        dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1))
        Return dtFrom
    End Function
    Public Shared Function GetLastDateOfMonth(ByVal iMonth As Integer)
        Dim dtTo As New DateTime(DateTime.Now.Year, iMonth, 1)
        dtTo = dtTo.AddMonths(1)
        dtTo = dtTo.AddDays(-(dtTo.Day))
        Return dtTo
    End Function
    Public Shared Function SelectTextbox(ByVal txt As TextBox) As TextBox
        txt.SelectionStart = 0
        txt.SelectionLength = txt.Text.Length
        Return txt
    End Function
    Public Shared Function SelectmskTextbox(ByVal txt As MaskedTextBox) As MaskedTextBox
        txt.SelectionStart = 0
        txt.SelectionLength = txt.Text.Length
        Return txt
    End Function
    Public Shared Function ChangeDateFormat(ByRef dt As DateTime) As Object
        ChangeDateFormat = dt.ToString("yyyy-MM-dd")
    End Function

    Public Shared Function CheckLicence() As Boolean
        CheckLicence = False
        Dim count As Integer = ClsFunPrimary.ExecScalarInt("Select OpenTime from Licence")

        If count > 30 Then
            CheckLicence = True
        Else

            If count = 0 Then
                ClsFunPrimary.ExecNonQuery("Insert into Licence(OpenTime,InstallDate) values(" & count + 1 & ",'" & ClsFunPrimary.GetServerDate & "')")
                CheckLicence = False
            Else
                Dim instDate As Date = ClsFunPrimary.ExecScalarStr("Select InstallDate from Licence")
                If ClsFunPrimary.ExecScalarInt("Select count(*) from ledger") > 30 Then
                    Dim Expdate As Date = ClsFunPrimary.ExecScalarStr("Select MAx(entrydate) from ledger")
                    Dim difference As TimeSpan = Expdate.Subtract(instDate)
                    If difference.TotalDays > 7 Then
                        CheckLicence = True
                    End If
                Else
                    Dim compdate As Date = ClsFunPrimary.GetServerDate
                    Dim dif As TimeSpan = compdate.Subtract(instDate)
                    If dif.TotalDays > 7 Then
                        CheckLicence = True
                    Else
                        CheckLicence = False
                    End If
                End If
                ClsFunPrimary.ExecNonQuery("Update Licence set OpenTime =OpenTime+1")
            End If
        End If
    End Function
    Public Shared Function CalcChargesScriptForm(ByRef Amount As Decimal, ByRef ChargesId As Integer, ByRef ChargesAmt As Decimal) As String()
        ''   CalcChargesScriptForm = 0.0
        Dim dt As DataTable = ExecDataTable("Select ID,ChargesType,AccountID,AccountName,CostOn,RoundOff FROM Charges where Id=" & ChargesId & "")
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("CostOn").ToString() = "Party Cost" Then
                If dt.Rows(0)("ChargesType").ToString() = "+" Then
                    Amount = Amount + ChargesAmt
                Else
                    Amount = Amount - ChargesAmt
                End If
            Else
                If dt.Rows(0)("ChargesType").ToString() = "+" Then
                    Amount = Amount + ChargesAmt
                Else
                    Amount = Amount - ChargesAmt
                End If
            End If
        End If
        Return New String() {Amount, dt.Rows(0)("CostOn").ToString(), ChargesAmt, dt.Rows(0)("ChargesType").ToString(), dt.Rows(0)("AccountID").ToString(), dt.Rows(0)("AccountName").ToString()}

    End Function

End Class
