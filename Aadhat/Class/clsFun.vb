Imports System.Data.SQLite
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports Microsoft.Win32

Public Class clsFun
    Public Shared ConStr As String = "Data Source=|DataDirectory|" & GlobalData.ConnectionPath & ";Version=3;New=True;Compress=True;synchronous=ON;cache_size = 100000;Page Size=2048;PRAGMA mmap_size = 268435456;"
    Public Shared con As New SQLiteConnection

    Public Sub New()
        ConStr = "Data Source=|DataDirectory|" & GlobalData.ConnectionPath & ";Version=3;New=True;Compress=True;synchronous=ON;cache_size = 100000;Page Size=2048;PRAGMA mmap_size = 268435456;"
    End Sub

    Public Function GenerateGUID() As String
        Dim guid As Guid = guid.NewGuid()
        Return guid.ToString()
    End Function


    Public Shared Function GetConnection() As SQLiteConnection
        If (GlobalData.ConnectionPath).ToUpper = ("Data").ToUpper Then
            Dim cs As String = "Data Source=|DataDirectory|" & GlobalData.ConnectionPath & ";Version=3;New=True;Compress=True;synchronous=ON;cache_size =100000;Page Size=2048;PRAGMA mmap_size = 268435456;"
            con = New SQLiteConnection(cs)
            con.Open()
        Else
            Dim cs As String = "Data Source= " & GlobalData.ConnectionPath & ";Version=3;New=True;Compress=True;synchronous=ON;cache_size = 100000;Page Size=2048;PRAGMA mmap_size = 268435456;"
            con = New SQLiteConnection(cs)
            con.Open()
        End If
        Return con
    End Function

    Public Shared Sub CloseConnection()
        con.Close()
    End Sub
    Public Shared Sub DoubleBuffered(ByVal dgv As DataGridView, ByVal setting As Boolean)
        Dim dgvType As Type = dgv.GetType()
        Dim pi As System.Reflection.PropertyInfo = dgvType.GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
        '  Dim pi As PropertyInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(dgv, setting, Nothing)
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
        Dim ad As New SQLiteDataAdapter(cmdText, GetConnection())
        Dim dt As DataTable = New DataTable()
        ad.Fill(dt)
        ad.Dispose()
        Return dt
    End Function
    Public Shared Function ExecDataSet(ByVal cmdText As String, Optional ByVal tmptblName As String = "a") As DataSet
        Dim ad As New SQLiteDataAdapter(cmdText, GetConnection())
        Dim ds As DataSet = New DataSet()
        ad.Fill(ds, tmptblName)
        ad.Dispose()
        Return ds
    End Function
    Public Shared Function adExecDataTable(ByVal cmdText As String, ByVal parameters As Dictionary(Of String, Object)) As DataTable
        Using connection As New SQLiteConnection(GetConnection())
            Using command As New SQLiteCommand(cmdText, connection)
                If parameters IsNot Nothing Then
                    For Each param In parameters
                        command.Parameters.AddWithValue(param.Key, param.Value)
                    Next
                End If

                Using adapter As New SQLiteDataAdapter(command)
                    Dim dt As DataTable = New DataTable()
                    adapter.Fill(dt)
                    Return dt
                End Using
            End Using
        End Using
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
        Dim Con As SQLiteConnection = New SQLiteConnection(GetConnection())
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
    'Public Shared Function CheckIfViewColumnExists(ByVal ViewName As String, ByVal columnName As String) As Boolean
    '    Using conn = New SQLiteConnection(GetConnection())
    '        '  conn.Open()
    '        Dim cmd = conn.CreateCommand()
    '        cmd.CommandText = String.Format("SELECT sql FROM sqlite_master WHERE type = 'view' AND name = '" & ViewName & "'")
    '        Dim reader = cmd.ExecuteReader()
    '        Dim nameIndex As Integer = reader.GetOrdinal("Name")
    '        While reader.Read()
    '            If reader.GetString(nameIndex).Equals(columnName) Then
    '                conn.close()
    '                Return True
    '            End If
    '        End While
    '        conn.Close()
    '    End Using
    '    Return False
    'End Function
    Public Shared Function CheckIfViewColumnExists(ByVal ViewName As String, ByVal columnName As String) As Boolean
        Dim viewExists As String = String.Empty
        Using conn As New SQLiteConnection(GetConnection())
            'conn.Open()
            Dim cmd = conn.CreateCommand()
            cmd.CommandText = String.Format("SELECT sql FROM sqlite_master WHERE type = 'view' AND name = '" & ViewName & "'")
            cmd.Parameters.AddWithValue("@viewName", ViewName)
            Dim viewSql = cmd.ExecuteScalar().ToString()
            If Not String.IsNullOrEmpty(viewSql) Then
                Dim regex = New Regex("\[(\w+)\]")
                Dim matches = regex.Matches(viewSql)
                For Each match As Match In matches
                    If match.Groups(1).Value.Equals(columnName) Then
                        conn.Close()
                        Return True
                    End If
                Next
            End If
            conn.Close()
        End Using
        Return False
    End Function
    Public Shared Function CheckIfColumnExists(ByVal tableName As String, ByVal columnName As String) As Boolean
        Using conn = New SQLiteConnection(GetConnection())
            '  conn.Open()
            Dim cmd = conn.CreateCommand()
            cmd.CommandText = String.Format("PRAGMA table_info({0})", tableName)
            Dim reader = cmd.ExecuteReader()
            Dim nameIndex As Integer = reader.GetOrdinal("Name")
            While reader.Read()
                If reader.GetString(nameIndex).Equals(columnName) Then
                    conn.close()
                    Return True
                End If
            End While
            conn.Close()
        End Using
        Return False
    End Function
    Public Shared Function ExecScalarStr(ByVal cmdText As String) As String
        Dim Result As String = String.Empty
        Dim Con As SQLiteConnection = GetConnection()
        'Con.Open()
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
            If Val IsNot Nothing Then ' Added null check here
                Result = Val.ToString()
            End If
        Catch ex As Exception
            Result = String.Empty
        End Try
        Return Result
    End Function

    Shared el As New Aadhat.ErrorLogger


    ''Insert data into Ledger
    Public Shared Function Ledger(ByVal Opt As Integer, ByVal vchid As Integer, ByVal entryDt As String, ByVal TransType As String, ByVal Acid As Integer, _
                                  ByVal AcName As String, ByVal Amt As Double, ByVal Dc As String, Optional ByVal Remarks As String = "", _
                                  Optional ByVal Narration As String = "", Optional ByVal RemarkHindi As String = "") As Integer
        Ledger = 0
        Dim sSql As String = String.Empty
        If Opt = 0 Then
            sSql = "Insert into Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi)" & _
                " values(" & vchid & ",'" & entryDt & "','" & TransType & "'," & Acid & ",'" & AcName & "'," & Amt & ",'" & Dc & "','" & Remarks & "', " & _
                "'" & Narration & "','" & RemarkHindi & "');"
            Ledger = ExecNonQuery(sSql)
            el.WriteToErrorLog(sSql, Constants.compname, "Ledger Record")
            'ElseIf Opt = 1 Then
            '    sSql = "Update Ledger SET EntryDate='" & entryDt & "',TransType='" & TransType & "',AccountID='" & Acid & "',AccountName='" & AcName & "',Amount=" & Amt & ",DC='" & Dc & "',Remark='" & Remarks & "' where VourchersID=" & vchid & ""
            '    Ledger = ExecNonQuery(sSql)
        End If
        Return Ledger
    End Function
    Public Shared Function FastLedger(FastQuery As String) As Integer
        Dim sSql As String = String.Empty

        sSql = "Insert into Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi) " & FastQuery & ";"
        FastQuery = ExecNonQuery(sSql, True)
        ' el.WriteToErrorLog(sSql, Constants.compname, "Ledger Record")
        Return FastLedger
    End Function
    Public Shared Function FastReceipt(FastQuery As String) As Integer
        Dim sSql As String = String.Empty
        sSql = "Insert into Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi,PartyID,PartyName) " & FastQuery & ";"
        FastQuery = ExecNonQuery(sSql)
        'el.WriteToErrorLog(sSql, Constants.compname, "Ledger Record")
        Return FastReceipt
    End Function

    'Public Shared Function LedgerMulti(ByVal Opt As Integer, ByVal vchid As Integer, ByVal entryDt As String, ByVal TransType As String, ByVal Acid As Integer, _
    '                           ByVal AcName As String, ByVal Amt As Double, ByVal Dc As String, Optional ByVal Remarks As String = "", _
    '                           Optional ByVal Narration As String = "", Optional ByVal RemarkHindi As String = "") As Integer
    '    LedgerMulti = 0
    '    Dim sSql As String = String.Empty
    '    If Opt = 0 Then
    '        sSql = Saql & "Insert into Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi)" & _
    '            " values(" & vchid & ",'" & entryDt & "','" & TransType & "'," & Acid & ",'" & AcName & "'," & Amt & ",'" & Dc & "','" & Remarks & "', " & _
    '            "'" & Narration & "','" & RemarkHindi & "');"
    '        LedgerMulti = ExecNonQuery(sSql)
    '        el.WriteToErrorLog(sSql, Constants.compname, "Ledger Record")
    '        'ElseIf Opt = 1 Then
    '        '    sSql = "Update Ledger SET EntryDate='" & entryDt & "',TransType='" & TransType & "',AccountID='" & Acid & "',AccountName='" & AcName & "',Amount=" & Amt & ",DC='" & Dc & "',Remark='" & Remarks & "' where VourchersID=" & vchid & ""
    '        '    Ledger = ExecNonQuery(sSql)
    '    End If
    '    Return LedgerMulti
    'End Function
    Public Shared Function CrateLedger(ByVal Opt As Integer, ByVal vchid As String, ByVal Slip As Integer, ByVal entryDt As String, ByVal TransType As String, ByVal Acid As Integer, ByVal AcName As String, ByVal CrateType As String, ByVal CrateID As Integer, ByVal MarkaName As String, ByVal qty As Integer, ByVal Remark As String, ByVal Rate As String, ByVal Amount As String, ByVal CashPaid As String) As Integer
        CrateLedger = 0
        Dim sSql As String = String.Empty
        If Opt = 0 Then
            sSql = "Insert into CrateVoucher(VoucherID,SlipNo, EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName, Qty,Remark,Rate,Amount,CashPaid)" & _
                " values(" & vchid & "," & Slip & ",'" & entryDt & "','" & TransType & "'," & Acid & ",'" & AcName & "','" & CrateType & "'," & CrateID & ",'" & MarkaName & "'," & qty & ",'" & Remark & "','" & Rate & "','" & Amount & "','" & CashPaid & "')"
            CrateLedger = ExecNonQuery(sSql)
            el.WriteToErrorLog(sSql, Constants.compname, "Crate Record")
            'ElseIf Opt = 1 Then
            '    sSql = "Update Ledger SET EntryDate='" & entryDt & "',TransType='" & TransType & "',AccountID='" & Acid & "',AccountName='" & AcName & "',Amount=" & Amt & ",DC='" & Dc & "',Remark='" & Remarks & "' where VourchersID=" & vchid & ""
            '    Ledger = ExecNonQuery(sSql)
        End If

        Return CrateLedger
    End Function

    Public Shared Function FastCrateLedger(FastQuery) As Integer
        Dim sSql As String = String.Empty
        sSql = "Insert into CrateVoucher(VoucherID,SlipNo, EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName, Qty,Remark,Rate,Amount,CashPaid)" & FastQuery & ""
        FastCrateLedger = ExecNonQuery(sSql, True)
        ' el.WriteToErrorLog(sSql, Constants.compname, "Crate Record")
        Return FastCrateLedger
    End Function

    Public Shared Function GetAutoNo(ByVal TblName As String, ByVal ColName As String, Optional Prefix As String = "", Optional Suffix As String = "") As String
        GetAutoNo = Prefix & clsFun.ExecScalarInt("Select " & ColName & " from " & TblName & "") & Suffix
    End Function


    Public Shared Function convdate(ByVal Date1 As String)
        Dim YearStart As Date = clsFun.ExecScalarStr("Select YearStart From Company")
        Dim YearEnd As Date = clsFun.ExecScalarStr("Select YearEnd From Company")
        Dim yearEndDate As New DateTime(YearStart.Year, 12, 31)
        Dim yearStartDate As New DateTime(YearStart.Year, 1, 1)
        'Check If Day is entered.
        If Len(Date1) <> 10 Then
            If Len(Date1.Substring(0, 2).Trim) = 1 Then Date1 = "0" & Date1.Substring(0, 2).Trim & "-0" & Now.Month
            If Date1.Substring(0, 2).Trim <> "" Then
                'Check if Month is entered.
                If Len(Date1.Substring(3, 2).Trim) = 0 Then Date1 = Date1.Substring(0, 2).Trim & "-0" & Now.Month
                If Len(Date1.Substring(3, 2).Trim) <> 2 Then Date1 = Date1.Substring(0, 2).Trim & "-0" & Date1.Substring(3, 2).Trim

                If Date1.Substring(3, 2).Trim <> "" Then
                    'Check if year is not entered.
                    If Date1.Length = 6 And IsDate(Date1.Substring(0, 2).Trim & "-" & Date1.Substring(3, 2).Trim & "-" & Now.Year) Then
                        'Check if Date and month entered are valid.

                        If Date1.Substring(3, 2).Trim >= YearStart.Month And Date1.Substring(3, 2).Trim <= yearEndDate.Month Then
                            Date1 = Date1 & YearStart.Year
                            ' MsgBox("Yes")
                        Else
                            If Date1.Substring(0, 2).Trim = "29" And Date1.Substring(3, 2).Trim = "02" Then
                                If Val(YearEnd.Year) Mod 100 = 0 Then
                                    If Val(YearEnd.Year) Mod 400 = 0 Then
                                        Date1 = Date1 & YearEnd.Year
                                    Else
                                        Date1 = (Date1.Substring(0, 2).Trim - 1) & "-" & Date1.Substring(3, 2).Trim & "-" & YearEnd.Year
                                        'Date1 = Date1 & YearEnd.Year)
                                    End If
                                Else
                                    If Val(YearEnd.Year) Mod 4 = 0 Then
                                        Date1 = Date1 & YearEnd.Year
                                    Else
                                        Date1 = (Date1.Substring(0, 2).Trim - 1) & "-" & Date1.Substring(3, 2).Trim & "-" & YearEnd.Year
                                        'Date1 = Date1 & YearEnd.Year)
                                    End If

                                End If
                            Else
                                Date1 = Date1 & YearEnd.Year

                            End If

                        End If
                    Else
                        If Date1.Substring(3, 2).Trim >= YearStart.Month And Date1.Substring(3, 2).Trim <= yearEndDate.Month Then
                            Date1 = Date1 & "-" & YearStart.Year
                            ' MsgBox("Yes")
                        Else
                            Date1 = Date1 & "-" & YearEnd.Year
                        End If
                    End If
                Else
                    If YearStart.Month <= Now.Month Then
                        'Check if Date entered will be valid for the current date that we want to create?
                        If IsDate(Date1.Substring(0, 2).Trim & "-" & Now.Month & "-" & YearStart.Year) Then
                            'Check if month is two digit?
                            If Now.Month.ToString.Length < 2 Then
                                Date1 = Date1.Substring(0, 2).Trim & "-0" & Now.Month & "-" & YearStart.Year
                            Else
                                Date1 = Date1.Substring(0, 2).Trim & "-" & Now.Month & "-" & YearStart.Year
                            End If
                        Else
                            Dim thisMonth As New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                            'last day of the month
                            Date1 = thisMonth.AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy").ToString
                        End If
                    Else
                        If IsDate(Date1.Substring(0, 2).Trim & "-" & Now.Month & "-" & YearEnd.Year) Then
                            'Check if month is two digit?
                            If Now.Month.ToString.Length < 2 Then
                                Date1 = Date1.Substring(0, 2).Trim & "-0" & Now.Month & "-" & YearEnd.Year
                            Else
                                Date1 = Date1.Substring(0, 2).Trim & "-" & Now.Month & "-" & YearEnd.Year
                            End If
                        Else
                            Dim thisMonth As New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                            'last day of the month
                            Date1 = thisMonth.AddMonths(1).AddDays(-1).ToString("dd-MM-yyyy").ToString
                        End If

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
        Try
            Date1 = CDate(Date1).ToString("dd-MM-yyyy")
        Catch ex As Exception
            Date1 = CDate(DateTime.Now).ToString("dd-MM-yyyy")
        End Try

        If Date1 < YearStart Then Date1 = YearStart.ToString("dd-MM-yyyy")
        If Date1 > YearEnd Then Date1 = YearEnd.ToString("dd-MM-yyyy")

        'FbConnection.ClearAllPools()
        convdate = Date1
    End Function

    Private Shared Function ChangeDate() As Boolean
        Dim reg As RegistryKey
        reg = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)
        If Convert.ToString(reg).Trim <> String.Empty Then
            reg.SetValue("sShortDate", "dd-MMM-yy")
            reg.SetValue("sLongDate", "dd-MMM-yy")
        End If
        Return True
    End Function


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
        Dim count As Integer = clsFun.ExecScalarInt("Select OpenTime from Licence")

        If count > 30 Then
            CheckLicence = True
        Else

            If count = 0 Then
                clsFun.ExecNonQuery("Insert into Licence(OpenTime,InstallDate) values(" & count + 1 & ",'" & clsFun.GetServerDate & "')")
                CheckLicence = False
                MainScreenForm.lblARC.Text = "Evaluation Version"
            Else
                Dim instDate As Date = clsFun.ExecScalarStr("Select InstallDate from Licence")
                If clsFun.ExecScalarInt("Select count(*) from ledger") > 30 Then
                    Dim Expdate As Date = clsFun.ExecScalarStr("Select MAx(entrydate) from ledger limit 1")
                    Dim difference As TimeSpan = Expdate.Subtract(instDate)
                    If difference.TotalDays > 7 Then
                        CheckLicence = True
                    End If
                Else
                    Dim compdate As Date = clsFun.GetServerDate
                    Dim dif As TimeSpan = compdate.Subtract(instDate)
                    If dif.TotalDays > 7 Then
                        CheckLicence = True
                    Else
                        CheckLicence = False
                        MainScreenForm.lblARC.Text = "Evaluation Version"
                    End If
                End If
                clsFun.ExecNonQuery("Update Licence set OpenTime =OpenTime+1")
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
    Public Shared Function CheckFinDate(ByVal StartDate As Date, ByVal Enddate As Date) As Boolean
        If StartDate < FinYearStart Then
            CheckFinDate = False
        ElseIf StartDate > FinYearEnd Then
            CheckFinDate = False
        ElseIf Enddate < FinYearStart Then
            CheckFinDate = False
        ElseIf Enddate > FinYearEnd Then
            CheckFinDate = False
        Else
            CheckFinDate = True
        End If
    End Function

End Class


