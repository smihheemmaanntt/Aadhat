Imports Microsoft.Win32

Public Class Splash

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'ProgressBar1.Visible = True
        ProgressBar1.Maximum = 100
        ProgressBar1.Value += 10
        If ProgressBar1.Value <= 20 Then
            Label4.Text = "Loading System..."
        ElseIf ProgressBar1.Value <= 40 Then
            Label4.Text = "Checking Companies..."
        ElseIf ProgressBar1.Value <= 60 Then
            Label4.Text = "Checking Database..."
        ElseIf ProgressBar1.Value <= 80 Then
            Label4.Text = "Just Wait.Almost Done"
        ElseIf ProgressBar1.Value <= 100 Then
            Label4.Text = "Welcome to Aadhat..."
            If ProgressBar1.Value = 100 Then
                Me.Dispose() : ShowCompanies.Show()
                Timer1.Dispose()
            End If
        End If

    End Sub

    Private Sub Splash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ProgressBar1.Visible = False
        Label4.TextAlign = ContentAlignment.MiddleCenter
        ChangeDate() : ChangeDate()
        'Dim conn As SQLite.SQLiteConnection = New SQLite.SQLiteConnection("Data Source=" & Application.StartupPath & "\Dbp.db;Version=3;New=True;Compress=True;synchronous=ON;")
        'conn.Open()
        'conn.ChangePassword("Changepassword")
        'conn.Close()
    End Sub
    Private Sub ChangeDate()
        Dim reg As RegistryKey
        reg = Registry.CurrentUser.OpenSubKey("Control Panel\International", True)
        If Convert.ToString(reg).Trim <> String.Empty Then
            reg.SetValue("sShortDate", "dd-MMM-yy")
            reg.SetValue("sLongDate", "dd-MMM-yy")
        End If
    End Sub
    'Private Sub CreateSqlLite()
    '    Dim Odbf As New SampleSqlLite.cSQliteFunction
    '    Try
    '        Dim sQDatabaseFileName As String = Path.Combine(Application.StartupPath, "dbp.db")
    '        If Not File.Exists(sQDatabaseFileName) Then System.Data.SQLite.SQLiteConnection.CreateFile(sQDatabaseFileName)
    '        oDbf.CreateConnection(sQDatabaseFileName)
    '        If oDbf.DatabaseConnection.State = ConnectionState.Closed Then oDbf.DatabaseConnection.Open()

    '        If oDbf.DatabaseConnection.GetSchema("Tables").[Select]("Table_Name = 'Ledger'").Length = 0 Then
    '            oDbf.deleteData("CREATE TABLE IF NOT EXISTS [Ledger] ( [LedgerID] integer PRIMARY KEY ASC AUTOINCREMENT" & ",[Description] varchar(255), [Time] datetime,Balance int default 0 );")
    '        End If

    '        If oDbf.DatabaseConnection.State = ConnectionState.Open Then oDbf.DatabaseConnection.Close()
    '        oDbf.oConnection = New System.Data.SQLite.SQLiteConnection(oDbf.DatabaseConnectionString)
    '        Dim nInsertID As Integer = CInt(oDbf.getScalar("insert into Ledger (Description,Time,Balance) values ('test data','" & DateTime.Now & "',500); Select last_insert_rowid();"))
    '        Dim oDsQSpyName As DataSet = oDbf.SelectData("Select LedgerID,Description,Cast(Time as varchar) as Time,Balance from Ledger")
    '        oDbf.deleteData("delete from Ledger where LedgerID = 1")
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub
End Class
