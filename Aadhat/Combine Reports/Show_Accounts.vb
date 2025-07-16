Imports System.Data.SQLite
Public Class Show_Accounts

    Private Sub Show_Accounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mskFromDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyy"))
        MsktoDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyy"))
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 130
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Description" : dg1.Columns(4).Width = 545
        dg1.Columns(5).Name = "Debit" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Credit" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Balance" : dg1.Columns(7).Width = 150
        dg1.Columns(8).Name = "HindiName" : dg1.Columns(8).Visible = False
        dg1.Columns(9).Name = "HindiItem" : dg1.Columns(9).Visible = False
    End Sub
    Public Sub retriveAccounts(Optional ByVal condtion As String = "")
        DgAccountSearch.Rows.Clear()
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        ' Define a DataTable object to hold the merged data
        Dim dataTable As New DataTable()

        ' Loop through the rows in the DataGridView control and merge the data from each checked database into the DataTable
        For Each row As DataGridViewRow In Combine_Reports.dg1.Rows
            ' Check if the checkbox in the current row is checked
            If CBool(row.Cells(0).Value) Then
                ' Get the database path from the specified cell in the current row
                Dim databasePath As String = row.Cells(8).Value.ToString()

                ' Build the connection string using the database path
                Dim connectionString As String = String.Format("Data Source={0}", databasePath)

                ' Open a connection to the database, retrieve the data, and merge it into the DataTable
                Using connection As New SQLiteConnection(connectionString)
                    connection.Open()
                    Using command As New SQLiteCommand("Select Ac.ID as ID,Ac.AccountName as AccountName,GroupName,grp.ID as GroupID from Accounts ac inner join AccountGroup grp on ac.GroupID=grp.ID " & condtion & " Group by AccountName Limit 20", connection)
                        Using adapter As New SQLiteDataAdapter(command)
                            Dim tempDataTable As New DataTable()
                            adapter.Fill(tempDataTable)
                            If dataTable.Rows.Count = 0 Then
                                dataTable = tempDataTable
                            Else
                                ' dataTable.Merge(tempDataTable)
                                For Each group As DataRow In tempDataTable.Rows
                                    Dim existingGroup As DataRow() = dataTable.Select("AccountName = '" & group("AccountName").ToString() & "'")
                                    If existingGroup.Length > 0 Then
                                        ' Update the existing group with the new values
                                        'existingGroup(0)("Opbal") = CDec(existingGroup(0)("Opbal")) + CDec(group("Opbal"))
                                    Else
                                        ' Add the new group to the DataTable
                                        dataTable.Rows.Add(group.ItemArray)
                                    End If
                                Next
                            End If
                        End Using
                    End Using
                End Using
            End If
        Next

        For i = 0 To dataTable.Rows.Count - 1
            DgAccountSearch.Rows.Add()
            With DgAccountSearch.Rows(i)
                .Cells(0).Value = dataTable.Rows(i)("id").ToString()
                .Cells(1).Value = dataTable.Rows(i)("AccountName").ToString()
            End With
        Next
        lblTotalAccounts.Text = "Total Accounts : " & Val(dg1.Rows.Count)
    End Sub
    Private Sub Retrive()

        ' Application.DoEvents()
        dg1.Rows.Clear()
        txtOpBal.Text = ""
        Dim ssql As String = String.Empty
        Dim dt As New DataTable : Dim dr As Decimal = 0
        Dim cr As Decimal = 0 : Dim tot As Decimal = 0
        Dim opbal As String = "" : Dim tmpamtdr As String = ""
        Dim tmpamtcr As String = ""
        opbal = clsFun.ExecScalarStr(" Select Round(OpBal,2) FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        ssql = "Select VourchersID,Entrydate, TransType,AccountName,Remark,RemarkHindi,Narration,round(Amount,2) as Dr,'0' as Cr from Ledger where DC ='D' " & IIf(txtAccountID.Text > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    union all" &
            " Select VourchersID,Entrydate,  TransType,AccountName,Remark,RemarkHindi,Narration,'0' as Dr,round(Amount,2) as Cr  from Ledger where Dc='C' " & IIf(txtAccountID.Text > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    "
        tmpamtdr = clsFun.ExecScalarStr("Select round(sum(Amount),2) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        tmpamtcr = clsFun.ExecScalarStr("Select round(sum(Amount),2) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If

        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = -Val(opbal)
        End If

        'If Val(opbal) < Val(tmpamt) Then
        '    tmpamt = Math.Abs(Val(tmpamt)) + Math.Abs(Val(opbal))
        'Else
        '    tmpamt = Val(opbal) - Math.Abs(Val(tmpamt))
        'End If
        dt = clsFun.ExecDataTable(ssql)
        Dim dvData As DataView = New DataView(dt)
        'dvData.RowFilter = "EntryDate Between '" & mskFromDate.Text & "' And '" & MsktoDate.Text & "'"
        dvData.Sort = " [EntryDate],VourchersID asc"
        dt = dvData.ToTable
        dg1.Rows.Clear()

        opbal = tmpamt
        'If Val(tmpamt) > 0 Then
        '    opbal = Val(tmpamt)
        'Else
        '    opbal = Val(opbal) + Val(tmpamt)
        'End If

        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from LEdger where  accountID=" & Val(txtAccountID.Text) & " and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        If cnt = 0 Then
            txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & txtAccountID.Text & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Cr"
            Else
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Dr"
            End If
            '  txtOpBal.Text = IIf(Val(opbal) > 0, Val(opbal) & " Dr", Math.Abs(Val(opbal)) & " Cr")
        End If
        ''opbal = Val(opbal) + Val(tmpamt)
        '  
        If Val(txtOpBal.Text) > 0 Then
            drcr = txtOpBal.Text.Substring(txtOpBal.Text.Length - 2)
        End If
        ' opbal = Math.Abs(val(opbal))
        Try
            If dt.Rows.Count > 0 Then
                Application.DoEvents()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(5).Value = IIf(Format(Val(dt.Rows(i)("Dr").ToString()), "0.00") = 0, "", Format(Val(dt.Rows(i)("Dr").ToString()), "0.00"))
                        .Cells(6).Value = IIf(Format(Val(dt.Rows(i)("Cr").ToString()), "0.00") = 0, "", Format(Val(dt.Rows(i)("Cr").ToString()), "0.00"))
                        .Cells(8).Value = clsFun.ExecScalarStr(" Select OtherName FROM Accounts WHERE ID= " & txtAccountID.Text & "")
                        ssql = "(Select * FROM Transaction2 AS T INNER JOIN Items AS I ON T.ItemID = I.ID)"
                        .Cells(9).Value = dt.Rows(i)("RemarkHindi").ToString()
                        'If dt.Rows(i)("TransType").ToString() = "Speed Sale" Then
                        '    'ssql = "Select OtherName ||' = नग  : '||(nug)||', वजन : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount as HindiRemark  From " & ssql & "  where AccountID=" & txtAccountID.Text & " and TransType='" & dt.Rows(i)("TransType").ToString() & "' and EntryDate= '" & Format(dt.Rows(i)("EntryDate"), "yyyy-MM-dd") & "'"
                        '    .Cells(9).Value = clsFun.ExecScalarStr("Select OtherName ||' = नग  : '||(nug)||', वजन : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount From " & ssql & " where VoucherID= " & dt.Rows(i)("VourchersID").ToString() & "")
                        'Else
                        '    .Cells(9).Value = dt.Rows(i)("RemarkHindi").ToString()
                        'End If
                        If i = 0 Then
                            If Val(.Cells(5).Value) > 0 Then
                                If drcr = "Dr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(5).Value)), "0.00")
                                Else
                                    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'If Val(.cells(5).value) > Val(opbal) Then
                                    '    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'Else
                                    '    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'End If
                                End If
                            Else
                                If drcr = "Cr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(6).Value)), "0.00")
                                Else
                                    If Val(.Cells(6).Value) > Val(opbal) Then
                                        tot = Format(Val(Val(.Cells(6).Value) - Val(opbal)), "0.00")
                                    Else
                                        tot = Format(Val(Val(opbal) - Val(.Cells(6).Value)), "0.00")
                                    End If
                                End If
                                If drcr = "Dr" And Val(opbal) > Val(.Cells(6).Value) Then
                                    tot = Math.Round(Val(tot), 2)
                                Else
                                    tot = -Val(tot)
                                End If
                            End If
                        Else
                            tot = tot + IIf(Val(.Cells(5).Value) > 0, Val(.Cells(5).Value), -Val(.Cells(6).Value))
                        End If
                        .Cells(7).Value = IIf(tot > 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(tot), "0.00") & " Cr")
                        dr = dr + Val(.Cells(5).Value)
                        cr = cr + Val(.Cells(6).Value)
                    End With
                Next
            Else
                tot = Format(Val(opbal), "0.00")

            End If
            If drcr = "Dr" Then
                dr = Format(Val(dr) + Math.Abs(Val(opbal)), "0.00")
            Else
                cr = Format(Val(cr) + Math.Abs(Val(opbal)), "0.00")
            End If
            txtDramt.Text = dr.ToString() : txtcrAmt.Text = cr.ToString()
            If dt.Rows.Count = 0 Then
                txtBalAmt.Text = txtOpBal.Text
            Else
                txtBalAmt.Text = IIf(Format(Val(tot), "0.00") >= 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(Val(tot)), "0.00") & " Cr")
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
    End Sub
    'Public Sub ShowAccounts()
    '    Dim FastQuery As String = String.Empty
    '    Dim i As Integer = 0
    '    ' Create a connection to the main database
    '    Dim connMain As New SQLiteConnection(clsFun.GetConnection())

    '    ' Open the connection to the main database

    '    For Each row As DataGridViewRow In Combine_Reports.dg1.Rows

    '        If CBool(row.Cells(0).Value) Then
    '                Dim databasePath As String = row.Cells(8).Value.ToString()
    '                i = i + 1
    '                Dim attachCmd As New SQLiteCommand("ATTACH DATABASE '" & row.Cells(8).Value.ToString() & "' AS db" & i & ";", connMain)
    '                Dim AttachDb As String = "db" & i
    '                attachCmd.ExecuteNonQuery()
    '                FastQuery = "Select ID ,AccountName,GroupName from (" & FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & _
    '                            " ID,AccountName,GroupName from " & AttachDb & ".Account_AcGrp "
    '        End If
    '    Next
    '    FastQuery = FastQuery & ") Group by AccountName"
    '    '' Attach the first database
    '    'Dim attachCmd1 As New SQLiteCommand("ATTACH DATABASE 'database1.db' AS db1", connMain)
    '    'attachCmd1.ExecuteNonQuery()

    '    '' Attach the second database
    '    'Dim attachCmd2 As New SQLiteCommand("ATTACH DATABASE 'database2.db' AS db2", connMain)
    '    'attachCmd2.ExecuteNonQuery()

    '    '' Attach the third database
    '    'Dim attachCmd3 As New SQLiteCommand("ATTACH DATABASE 'database3.db' AS db3", connMain)
    '    'attachCmd3.ExecuteNonQuery()

    '    ' Execute the query on the main database
    '    Dim sqlQuery As String = FastQuery
    '    Dim cmd As New SQLiteCommand(sqlQuery, connMain)
    '    Dim adapter As New SQLiteDataAdapter(cmd)
    '    Dim dataTable As New DataTable()
    '    adapter.Fill(dataTable)
    '    'Dim commitCmd As New SQLiteCommand("COMMIT", connMain)
    '    'commitCmd.ExecuteNonQuery()
    '    'i = 0
    '    For Each row As DataGridViewRow In Combine_Reports.dg1.Rows
    '        If CBool(row.Cells(0).Value) Then
    '            i = i + 1
    '            Dim databasePath As String = row.Cells(8).Value.ToString()
    '            Dim detachCmd As New SQLiteCommand("DETACH DATABASE db" & i & "; ", connMain)
    '            detachCmd.ExecuteNonQuery()
    '        End If
    '    Next
    '    rowColums()
    '    For i = 0 To dataTable.Rows.Count - 1
    '        dg1.Rows.Add()
    '        With dg1.Rows(i)
    '            .Cells(0).Value = dataTable.Rows(i)("id").ToString()
    '            .Cells(1).Value = dataTable.Rows(i)("AccountName").ToString()
    '            .Cells(2).Value = dataTable.Rows(i)("GroupName").ToString()

    '        End With
    '    Next
    '    '' Detach the databases
    '    'Dim detachCmd1 As New SQLiteCommand("DETACH DATABASE db1", connMain)
    '    'detachCmd1.ExecuteNonQuery()
    '    'Dim detachCmd2 As New SQLiteCommand("DETACH DATABASE db2", connMain)
    '    'detachCmd2.ExecuteNonQuery()
    '    'Dim detachCmd3 As New SQLiteCommand("DETACH DATABASE db3", connMain)
    '    'detachCmd3.ExecuteNonQuery()

    '    ' Close the connection to the main database
    '    connMain.Close() : lblTotalAccounts.Text = "Total Accounts : " & Val(dg1.Rows.Count)
    'End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 2
        DgAccountSearch.Columns(0).Name = "ID"
        DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name"
        DgAccountSearch.Columns(1).Width = 445

        'FillWithNevigation() '  retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub

    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown, mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then
                DgAccountSearch.Rows(0).Selected = True : DgAccountSearch.Focus()
            End If
        End If

    End Sub

    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.Visible = True
        DgAccountSearch.BringToFront()
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        If DgAccountSearch.Visible = False Then mskFromDate.SelectAll() : Exit Sub
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : mskFromDate.SelectAll()
    End Sub

    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            mskFromDate.Focus() : mskFromDate.SelectAll()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
        If e.KeyCode = Keys.Up Then
            If DgAccountSearch.CurrentRow.Index = 0 Then
                txtAccount.Focus()
            End If
        End If
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()
    End Sub
End Class