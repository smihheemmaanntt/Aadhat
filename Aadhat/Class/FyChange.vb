Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Xml

Public Class FyChange
    Dim data As String = String.Empty
    Public Shared Sub CreateDb(ByVal dbname As String, ByVal compid As Integer)
        Dim CopyDestPath As String = String.Empty
        Dim tmpDataPath As String = String.Empty
        Dim i As Integer = 0
        CopyDestPath = "Data\Data"
        tmpDataPath = "Data"
        For i = 1 To 50
            If Directory.Exists(Application.StartupPath & "\" & CopyDestPath & i) = False Then
                CompData = CopyDestPath & i & "\"
                tmpDataPath = tmpDataPath & i
                tmpDataPath = tmpDataPath & "\Data.db"
                Directory.CreateDirectory(Application.StartupPath & "\" & CopyDestPath & i)
                CopyDestPath = (Application.StartupPath & "\" & CopyDestPath & i).ToString()
                Exit For
            End If
        Next

        Dim ssql As String = ""
        ssql = "INSERT INTO Company (CompanyName,PrintOtherName,Address,PrintOtheraddress,City,PrintOtherCity,State,PrintOtherState,MobileNo1,MobileNo2,PhoneNo,FaxNo,EmailID,Website,GSTN,DealsIN,RegistrationNo,PanNo," & _
            " Marka,Other,Logo,YearStart,Yearend,CompData,tag) " & _
            " Select CompanyName,PrintOtherName,Address,PrintOtheraddress,City,PrintOtherCity,State,PrintOtherState,MobileNo1,MobileNo2,PhoneNo,FaxNo,EmailID,Website,GSTN,DealsIN,RegistrationNo,PanNo, " & _
            " Marka,Other,Logo,date(YearStart,'+1 year'),date(Yearend,'+1 year') ,'" & tmpDataPath & "',tag FROM Company WHERE ID = " & compid & ""
        clsFun.ExecNonQuery(ssql)
        CopyDirectory(Application.StartupPath & "\Data\" & dbname, CopyDestPath)
        Dim data As String = String.Empty
        '' changeCompany(tmpDataPath)
        changeCompany("Data\" & tmpDataPath)
        AcBal()
    End Sub
    Private Shared Sub CopyDirectory(ByVal sourcePath As String, ByVal destPath As String)
        Dim CompData As String = String.Empty
        'If Not Directory.Exists(destPath) Then
        '    Directory.CreateDirectory(destPath)
        'End If
        For Each file__1 As String In Directory.GetFiles(Path.GetDirectoryName(sourcePath))
            Dim dest As String = Path.Combine(destPath, Path.GetFileName(file__1))
            File.Copy(file__1, dest)
            CompData = CompData & file__1
        Next

        For Each folder As String In Directory.GetDirectories(Path.GetDirectoryName(sourcePath))
            Dim dest As String = Path.Combine(destPath, Path.GetFileName(folder))
            CopyDirectory(folder, dest)
        Next

        ''Conection change
    End Sub

    Private Shared Sub AcBal()
        ' Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Dim opbal As String = ""
        Dim ClBal As String = ""
        ''  clsFun.ExecNonQuery("Update Accounts set OpBal=0")
        Dim ssql As String = ""
        ''
        '' clsFun.GetConnection()
        dt = clsFun.ExecDataTable("Select OpBal,id,dc FROM Accounts")
        For i = 0 To dt.Rows.Count - 1
            opbal = dt.Rows(i)("opbal").ToString()
            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("Id").ToString()) & "")
            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("Id").ToString()) & "")
            ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(i)("Id").ToString()) & "")
            If drcr = "Dr" Then
                tmpamtdr = Val(opbal) + Val(tmpamtdr)
            Else
                tmpamtcr = Val(opbal) + Val(tmpamtcr)
            End If
            Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
            If drcr = "Cr" Then
                opbal = Math.Abs(Val(opbal)) & " Cr"
            Else
                opbal = Math.Abs(Val(opbal)) & " Dr"
            End If
            Dim cntbal As Integer = 0
            cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(dt.Rows(i)("Id").ToString()) & "")
            If cntbal = 0 Then

                opbal = Math.Abs(Val(opbal)) & "," & dt.Rows(0)("Dc").ToString()
                ssql = ssql & "Update Accounts set opbal=" & opbal.Split(",")(0) & " where id=" & dt.Rows(i)("id").ToString() & ";"
                ''clsFun.ExecNonQuery("Update Accounts set obbal=" & opbal & " where id=" & dt.Rows(i)("id").ToString() & "")
            Else
                If Val(tmpamtcr) > Val(tmpamtdr) Then
                    opbal = Math.Abs(Val(tmpamt)) & ",Cr"

                Else
                    opbal = Math.Abs(Val(tmpamt)) & ",Dr"
                End If
                ssql = ssql & "Update Accounts set opbal=" & opbal.Split(",")(0) & " ,DC='" & opbal.Split(",")(1) & "' where id=" & dt.Rows(i)("id").ToString() & ";"
                '' clsFun.ExecNonQuery("Update Accounts set obbal=" & opbal.Split(",")(0) & " ,DC='" & opbal.Split(",")(0) & "' where id=" & dt.Rows(i)("id").ToString() & "")
            End If
        Next
        Dim a As Integer = clsFun.ExecNonQuery(ssql, True)

        If a > 0 Then
            ssql = ""
            ssql = "delete from Vouchers; delete from sqlite_sequence where name='Vouchers';delete from Ledger; delete from sqlite_sequence where name='Ledger';delete from Transaction1; delete from sqlite_sequence where name='Transaction1';delete from Transaction2; delete from sqlite_sequence where name='Transaction2';delete from Purchase; delete from sqlite_sequence where name='Purchase';delete from ChargesTrans;delete from sqlite_sequence where name='ChargesTrans'"
            Dim a1 As Integer = clsFun.ExecNonQuery(ssql, True)

            If a1 > 0 Then MsgBox("Financial Year Chaned Successfully...")
            Dim data As String = String.Empty
            'If data <> "" Then
            '    isCompanyOpen = True
            '    clsFun.ChangePath("Data\" & data)
            'End If
            '  clsFun.changeCompany()
            '  Me.Dispose()
            '   CompanyList.retrive()
        End If
    End Sub
    Private Shared Sub updateConfigFile(con As String)
        Dim XmlDoc As New XmlDocument()
        XmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
        For Each xElement As XmlElement In XmlDoc.DocumentElement
            If xElement.Name = "connectionStrings" Then
                xElement.FirstChild.Attributes(1).Value = con
            End If
        Next
        XmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
    End Sub
    Private Shared Sub changeCompany(ByVal Data As String)
        Try
            Dim Con As New StringBuilder("")
            Con.Append("Data Source=|DataDirectory|\")
            Con.Append(Data & ";Version=3;New=True;Compress=True;synchronous=ON;")
            'Con.Append(Data & ";Version=3;Password=smi3933;")
            Dim strCon As String = Con.ToString()
            updateConfigFile(strCon)
            Dim Db As New SQLite.SQLiteConnection()
            ConfigurationManager.RefreshSection("connectionStrings")
            Db.ConnectionString = ConfigurationManager.ConnectionStrings("Con").ConnectionString
            clsFun.ConStr = Db.ConnectionString
        Catch E As Exception
            MessageBox.Show(ConfigurationManager.ConnectionStrings("con").ToString() + ".This is invalid connection", "Incorrect server/Database")
        End Try
    End Sub
End Class
