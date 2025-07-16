Public Class Journal_Register

    Private Sub Journal_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub Journal_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : Dim mindate, maxdate As String
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        rowColums()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date"
        dg1.Columns(1).Width = 95
        dg1.Columns(2).Name = "Type"
        dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "Account Name"
        dg1.Columns(3).Width = 275
        dg1.Columns(4).Name = "Debit"
        dg1.Columns(4).Width = 120
        dg1.Columns(5).Name = "Credit"
        dg1.Columns(5).Width = 120
        dg1.Columns(6).Name = "Remark"
        dg1.Columns(6).Width = 450
    End Sub
    Private Sub retrive()
        Dim ssql As String
        Dim dt As New DataTable
        ssql = "Select VourchersID, EntryDate,TransType,AccountName,Amount as Dr,'0' as Cr,Remark from Ledger where DC ='D' and transtype='" & Me.Text & "' and  EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'" & _
         " UNION ALL Select VourchersID, EntryDate,TransType,AccountName,'0' as Dr,Amount as Cr,Remark  from Ledger where Dc='C' and transtype='" & Me.Text & "' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' order by VourchersID "
        dt = clsFun.ExecDataTable(ssql)
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = Format(Val(dt.Rows(i)("Dr").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Cr").ToString()), "0.00")
                        .Cells(6).Value = dt.Rows(i)("Remark").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub
    Private Sub calc()
        txtDebitBal.Text = Format(0, "0.00") : txtCreditBal.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtDebitBal.Text = Format(Val(txtDebitBal.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtCreditBal.Text = Format(Val(txtCreditBal.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
        Next
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                Exit Sub
            End If
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            JournalEntry.MdiParent = MainScreenForm
            JournalEntry.Show()
            JournalEntry.FillContros(tmpID)
            If Not JournalEntry Is Nothing Then
                Sellout_Mannual.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then
            Exit Sub
        End If
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        JournalEntry.MdiParent = MainScreenForm
        JournalEntry.Show()
        JournalEntry.FillContros(tmpID)
        If Not JournalEntry Is Nothing Then
            Sellout_Mannual.BringToFront()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtCreditBal_TextChanged(sender As Object, e As EventArgs) Handles txtCreditBal.TextChanged

    End Sub

    Private Sub txtDebitBal_TextChanged(sender As Object, e As EventArgs) Handles txtDebitBal.TextChanged

    End Sub
    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & Format(Val(.Cells(4).Value), "0.00") & "','" & Format(Val(.Cells(5).Value), "0.00") & "','" & .Cells(6).Value & "'," & _
                    " '" & Format(Val(txtDebitBal.Text), "0.00") & "','" & Format(Val(txtCreditBal.Text), "0.00") & "');"

            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
            'clsFun.ExecNonQuery("COMMIT;")
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\JournalEntryRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
End Class