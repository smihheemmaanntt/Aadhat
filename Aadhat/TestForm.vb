Imports System.Data.SQLite
Public Class TestForm
    Private headerCheckBox As New CheckBox
    Dim connectionString1 As String = "Data Source=" & GlobalData.PrvPath & ";Version=3;"
    Dim connectionString2 As String = "Data Source=" & GlobalData.ConnectionPath & ";Version=3;"

    Private Sub TestForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : ROwColums()
    End Sub
    Private Sub ROwColums()
        dg1.ColumnCount = 4
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        headerCheckBox.Location = New Point(headerCellLocation.X + 8, headerCellLocation.Y + 2)
        headerCheckBox.BackColor = Color.GhostWhite
        headerCheckBox.Size = New Size(18, 18)
        AddHandler headerCheckBox.Click, AddressOf HeaderCheckBox_Clicked
        dg1.Controls.Add(headerCheckBox)
        Dim checkBoxColumn As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = ""
        checkBoxColumn.Width = 30
        checkBoxColumn.Name = "checkBoxColumn"
        dg1.Columns.Insert(0, checkBoxColumn)
        AddHandler dg1.CellContentClick, AddressOf dg1_CellClick
        dg1.Columns(1).Name = "AccountID" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "Account Name" : dg1.Columns(2).Width = 270
        dg1.Columns(3).Name = "Group Name" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Cl bal" : dg1.Columns(4).Width = 100
        dg2.ColumnCount = 4
        dg2.Columns(0).Name = "AccountID" : dg2.Columns(0).Width = 100
        dg2.Columns(1).Name = "Account Name" : dg2.Columns(1).Width = 270
        dg2.Columns(2).Name = "Group Name" : dg2.Columns(2).Width = 100
        dg2.Columns(3).Name = "Cl bal" : dg2.Columns(3).Width = 100
    End Sub
    Private Sub HeaderCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        'Necessary to end the edit mode of the Cell.
        dg1.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In dg1.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = headerCheckBox.Checked
        Next
    End Sub
    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        'Check to ensure that the row CheckBox is clicked.
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In dg1.Rows
                If Convert.ToBoolean(row.Cells("checkBoxColumn").EditedFormattedValue) = False Then
                    isChecked = False
                    Exit For
                End If
            Next
            headerCheckBox.Checked = isChecked
        End If
    End Sub

    Private Sub dg1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dg1.CurrentCellDirtyStateChanged
        If dg1.CurrentCell IsNot Nothing AndAlso dg1.CurrentCell.OwningColumn.Name = "checkBoxColumn" Then
            dg1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub LoadData()
        If connectionString1.Trim = "Data Source=;Version=3;" Then Exit Sub
        If connectionString2.Trim = "Data Source=;Version=3;" Then MsgBox("No Other Fincial Year Found,Please Link it", MsgBoxStyle.Critical, "No Database") : Exit Sub
        Dim accounts1 As DataTable = GetAccounts(connectionString1)
        Dim accounts2 As DataTable = GetAccounts(connectionString2)
        For Each account1 As DataRow In accounts1.Rows
            Dim found As Boolean = False
            For Each account2 As DataRow In accounts2.Rows
                If account1("GUID").ToString() = account2("GUID").ToString() AndAlso account1("AccountName").ToString().ToUpper = account2("AccountName").ToString().ToUpper Then
                    found = True
                    Exit For
                End If
            Next

            If Not found Then
                dg1.Rows.Add(0, account1("ID"), account1("AccountName"), account1("GroupName"), account1("Opbal"))
            End If
        Next

        For Each account2 As DataRow In accounts2.Rows
            Dim found As Boolean = False
            For Each account1 As DataRow In accounts1.Rows
                If account1("GUID").ToString() = account2("GUID").ToString() AndAlso account1("AccountName").ToString().ToUpper = account2("AccountName").ToString().ToUpper Then
                    found = True
                    Exit For
                End If
            Next

            If Not found Then
                dg2.Rows.Add(account2("ID"), account2("AccountName"), account2("GroupName"), account2("Opbal"))
            End If
        Next
    End Sub
    Private Function GetAccounts(connectionString As String) As DataTable
        Dim dt As New DataTable()
        Using connection As New SQLiteConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT * FROM Account_AcGrp"
            Using adapter As New SQLiteDataAdapter(query, connection)
                adapter.Fill(dt)
            End Using
        End Using
        Return dt
    End Function
    Private Function GetAccounts2(connectionString As String) As DataTable
        Dim dt As New DataTable()
        Using connection As New SQLiteConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT * FROM Accounts"
            Using adapter As New SQLiteDataAdapter(query, connection)
                adapter.Fill(dt)
            End Using
        End Using
        Return dt
    End Function
    Private Sub InsertGUIDs(connectionString1 As String, connectionString2 As String)
        Dim dt1 As DataTable = GetAccounts(connectionString1)
        Dim dt2 As DataTable = GetAccounts(connectionString2)
        Using connection1 As New SQLiteConnection(connectionString1), connection2 As New SQLiteConnection(connectionString2)
            connection1.Open()
            connection2.Open()
            For Each row1 As DataRow In dt1.Rows
                Dim id As Integer = Convert.ToInt32(row1("ID"))
                Dim accountName As String = row1("AccountName").ToString().ToUpper
                If id = "57" Then MsgBox("a")
                ' Check if the same record exists in the second database
                Dim duplicateFound As Boolean = False
                For Each row2 As DataRow In dt2.Rows
                    If id = Convert.ToInt32(row2("ID")) AndAlso accountName.ToUpper = row2("AccountName").ToString().ToUpper Then
                        duplicateFound = True
                        Exit For
                    End If
                Next
                ' Insert the GUID into both databases if the record exists
                If duplicateFound Then
                    Dim guid As Guid = guid.NewGuid()
                    InsertGUID(connection1, id, accountName, guid)
                    InsertGUID(connection2, id, accountName, guid)
                End If
            Next
        End Using
    End Sub

    Private Sub InsertGUID(connection As SQLiteConnection, id As Integer, accountName As String, guid As Guid)
        Dim commandText As String = "UPDATE Accounts SET GUID = @GUID WHERE ID = @ID AND Upper(AccountName) = @AccountName"
        Using command As New SQLiteCommand(commandText, connection)
            command.Parameters.AddWithValue("@GUID", guid.ToString())
            command.Parameters.AddWithValue("@ID", id)
            command.Parameters.AddWithValue("@AccountName", accountName)
            command.ExecuteNonQuery()
        End Using
    End Sub


    Private Sub UpdateGUIDs(connectionString1 As String, connectionString2 As String)
        Dim dt1 As DataTable = GetAccounts(connectionString1)
        Dim dt2 As DataTable = GetAccounts(connectionString2)

        Using connection1 As New SQLiteConnection(connectionString1), connection2 As New SQLiteConnection(connectionString2)
            connection1.Open()
            connection2.Open()

            For Each row1 As DataRow In dt1.Rows
                Dim id As Integer = Convert.ToInt32(row1("ID"))
                Dim accountName As String = row1("AccountName").ToString().ToUpper()
                Dim guid As String = row1("GUID").ToString()
                ' Check if the same record exists in the second database
                Dim duplicateFound As Boolean = False
                For Each row2 As DataRow In dt2.Rows
                    If id = Convert.ToInt32(row2("ID")) AndAlso accountName.ToUpper() = row2("AccountName").ToString().ToUpper() Then
                        duplicateFound = True
                        Exit For
                    End If
                Next

                ' Update GUID in the second database if the record exists
                If duplicateFound Then
                    UpdateGUID(connection2, id, guid.ToString)
                End If
            Next
        End Using
    End Sub

    Private Sub UpdateGUID(connection As SQLiteConnection, id As Integer, guid As String)
        Dim accountName As String = String.Empty
        ' Retrieve the AccountName
        Dim selectQuery As String = "SELECT AccountName FROM Accounts WHERE ID = @id"
        Using selectCommand As New SQLiteCommand(selectQuery, connection)
            selectCommand.Parameters.AddWithValue("@id", id)
            Using reader As SQLiteDataReader = selectCommand.ExecuteReader()
                If reader.Read() Then
                    accountName = reader("AccountName").ToString()
                End If
            End Using
        End Using
        Dim query As String = "UPDATE Accounts SET GUID = @guid WHERE ID = @id and AccountName='" & accountName & "'"
        Using command As New SQLiteCommand(query, connection)
            command.Parameters.AddWithValue("@guid", guid.ToString())
            command.Parameters.AddWithValue("@id", id)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        dg1.Rows.Clear() : dg2.Rows.Clear()
        LoadData()
    End Sub

    Private Sub GUID_Click(sender As Object, e As EventArgs) Handles GUID.Click
        Dim connectionString1 As String = "Data Source=" & GlobalData.PrvPath & ";Version=3;"
        Dim connectionString2 As String = "Data Source=" & GlobalData.ConnectionPath & ";Version=3;"
        UpdateGUIDs(connectionString1, connectionString2)
        ' InsertGUIDs(connectionString1, connectionString2)
    End Sub

    Private Sub InsertMissingAccounts(ByVal connectionString1 As String, ByVal connectionString2 As String)
        Dim dt1 As DataTable = GetAccounts2(connectionString1)
        Dim dt2 As DataTable = GetAccounts2(connectionString2)
        Using connection2 As New SQLiteConnection(connectionString2)
            connection2.Open()
            'For Each row1 As DataRow In dt1.Rows
            '    Dim id As Integer = Convert.ToInt32(row1("ID"))
            '    Dim accountName As String = row1("AccountName").ToString()
            '    ' Check if the account exists in the second database
            '    Dim accountExists As Boolean = dt2.AsEnumerable().Any(Function(row2) Convert.ToInt32(row2("ID")) = id AndAlso row2("AccountName").ToString() = accountName)
            '    ' If the account does not exist in the second database, insert it with all fields
            '    If Not accountExists Then
            '        InsertAccount(connection2, row1)
            '    End If
            'Next
            For Each row As DataGridViewRow In dg1.Rows
                ' Check if the row is checked and selected
                Dim checkBox As DataGridViewCheckBoxCell
                CheckBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
                If CheckBox.Value = True Then
                    Dim id As Integer = Convert.ToInt32(row.Cells("AccountID").Value)
                    ' Find the corresponding DataRow in dt1 based on ID
                    Dim matchingRow As DataRow = dt1.AsEnumerable().FirstOrDefault(Function(r) Convert.ToInt32(r("ID")) = id)
                    If matchingRow IsNot Nothing Then
                        ' Insert the matching DataRow into the second database
                        InsertAccount(connection2, matchingRow)
                    End If
                End If
            Next
        End Using
    End Sub

    'Private Sub InsertAccount(ByVal connection As SQLiteConnection, ByVal row As DataRow)
    '      ' Prepare the insert command
    '      Dim command As New SQLiteCommand()
    '      command.Connection = connection
    '      Dim columns As New List(Of String)()
    '      Dim values As New List(Of String)()
    '      Dim parameters As New List(Of SQLiteParameter)()

    '      ' Add columns and parameters
    '      For Each column As DataColumn In row.Table.Columns
    '          Dim columnName As String = column.ColumnName
    '          If columnName <> "ID" Then
    '              If columnName = "Limit" Then
    '                  columnName = "[Limit]"
    '              End If
    '              columns.Add(columnName)
    '              values.Add("@" & column.ColumnName)
    '              parameters.Add(New SQLiteParameter("@" & column.ColumnName, row(column)))
    '          End If
    '      Next

    '      '' Add GUID column and value
    '      'columns.Add("GUID")
    '      'values.Add("@GUID")
    '      'parameters.Add(New SQLiteParameter("@GUID", Guid.NewGuid().ToString()))

    '      ' Create the SQL insert command dynamically
    '      Dim sql As String = String.Format("INSERT INTO Accounts ({0}) VALUES ({1})", String.Join(", ", columns.ToArray()), String.Join(", ", values.ToArray()))
    '      command.CommandText = sql

    '      ' Add parameters to the command
    '      command.Parameters.AddRange(parameters.ToArray())

    '      ' Execute the insert command
    '      command.ExecuteNonQuery()
    '  End Sub
    Private Sub InsertAccount(ByVal connection As SQLiteConnection, ByVal row As DataRow)
        ' Prepare the insert command
        Dim command As New SQLiteCommand()
        command.Connection = connection
        Dim columns As New List(Of String)()
        Dim values As New List(Of String)()
        Dim parameters As New List(Of SQLiteParameter)()

        ' Calculate the Restbal value
        Dim accountID As Integer = Convert.ToInt32(row("ID"))
        Dim entryDate As String = CDate(DateTime.Now).ToString("yyyy-MM-dd")
        Dim sql As String = "Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <= @entryDate) " &
            "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <= @entryDate)) " &
            "else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <= @entryDate) " &
            "+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <= @entryDate)) end),2) as Restbal " &
            "from Accounts Where ID=@accountID Order by upper(AccountName);"
        Dim restbal As Decimal = 0
        Dim oldconnections As New SQLiteConnection(connectionString1)
        oldconnections.Open()
        Using balanceCommand As New SQLiteCommand(sql, oldconnections)
            balanceCommand.Parameters.AddWithValue("@entryDate", entryDate)
            balanceCommand.Parameters.AddWithValue("@accountID", accountID)
            restbal = Convert.ToDecimal(balanceCommand.ExecuteScalar())
        End Using

        ' Set the OpBal and DC fields based on the Restbal value
        row("OpBal") = restbal
        row("DC") = If(restbal < 0, "Cr", "Dr")

        ' Add columns and parameters
        For Each column As DataColumn In row.Table.Columns
            Dim columnName As String = column.ColumnName
            If columnName <> "ID" Then
                If columnName = "Limit" Then
                    columnName = "[Limit]"
                End If
                columns.Add(columnName)
                values.Add("@" & column.ColumnName)
                parameters.Add(New SQLiteParameter("@" & column.ColumnName, row(column)))
            End If
        Next

        ' Create the SQL insert command dynamically
        Dim sqlInsert As String = String.Format("INSERT INTO Accounts ({0}) VALUES ({1})", String.Join(", ", columns.ToArray()), String.Join(", ", values.ToArray()))
        command.CommandText = sqlInsert

        ' Add parameters to the command
        command.Parameters.AddRange(parameters.ToArray())

        ' Execute the insert command
        command.ExecuteNonQuery()
    End Sub
    Private Sub InsertAccountWithGUID(ByVal connectionString As String, ByVal id As Integer, ByVal accountName As String, ByVal guid As Guid)
        Using connection As New SQLiteConnection(connectionString)
            connection.Open()
            Dim commandText As String = "INSERT INTO Accounts ( AccountName, GUID) VALUES ( @AccountName, @GUID)"
            Using command As New SQLiteCommand(commandText, connection)
                command.Parameters.AddWithValue("@AccountName", accountName)
                command.Parameters.AddWithValue("@GUID", guid.ToString())
                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        InsertMissingAccounts(connectionString1, connectionString2)
    End Sub
End Class