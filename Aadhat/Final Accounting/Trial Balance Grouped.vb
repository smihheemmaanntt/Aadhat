Public Class Trial_Balance_Grouped

    Private Sub Trail_Balance_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Trail_Balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.KeyPreview = True
        rowColums()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub

    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Head Name"
        dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Group Head /Account Name"
        dg1.Columns(2).Width = 600
        dg1.Columns(3).Name = "Debit"
        dg1.Columns(3).Width = 250
        dg1.Columns(4).Name = "Credit"
        dg1.Columns(4).Width = 250
        ' retrive()
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim i As Integer
        Dim drtot As Decimal = 0.0
        Dim crtot As Decimal = 0.0
        Dim DebitTotal As Decimal = 0.0
        Dim CreditTotal As Decimal = 0.0

        Dim lastval As Integer = 0
        Dim sql As String = String.Empty
        Dim TransType = String.Empty
        sql = "Select ID,Accountname,Area,Opbal,DC,GroupID,OtherName,Mobile1,(Select GroupName From AccountGroup Where ID=Accounts.GroupID) as GroupName,  " &
        "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
        "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
        " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
        " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0  Order by GroupName,AccountName ;"
        dt = clsFun.ExecDataTable(sql)
        pnlWait.Visible = True
        dg1.Rows.Clear()
        For i = 0 To dt.Rows.Count - 1
            DebitTotal = 0.0
            CreditTotal = 0.0
            Application.DoEvents()
            dg1.ClearSelection()
            pb1.Minimum = 0
            pb1.Maximum = dt.Rows.Count - 1
            pb1.Value = i
            ssql = "Select ID,GroupID,(Select GroupName From AccountGroup Where ID=Accounts.GroupID) as GroupName,  " & _
    "Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
    " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  GroupBal from Accounts " & _
    " Where GroupID=" & Val(dt.Rows(i)("GroupID").ToString()) & ";"
            dt1 = clsFun.ExecDataTable(ssql)
            If TransType <> dt1.Rows(0)("GroupName").ToString() Or TransType = "" Then
                If Application.OpenForms().OfType(Of Trial_Balance_Grouped).Any = False Then Exit Sub
                dg1.Rows.Add()
                With dg1.Rows(lastval)
                    .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    dg1.Rows(lastval).Cells(1).Style.BackColor = Color.Tan
                    dg1.Rows(lastval).Cells(2).Style.BackColor = Color.Tan
                    dg1.Rows(lastval).Cells(3).Style.BackColor = Color.Tan
                    dg1.Rows(lastval).Cells(4).Style.BackColor = Color.Tan
                    dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.GhostWhite
                    dg1.Rows(lastval).Cells(2).Style.ForeColor = Color.GhostWhite
                    dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.GhostWhite
                    dg1.Rows(lastval).Cells(4).Style.ForeColor = Color.GhostWhite
                    .Cells(0).Value = dt1.Rows(0)("GroupID").ToString()
                    .Cells(2).Value = dt1.Rows(0)("GroupName").ToString()
                    .Cells(2).Style.Font = New Font("Times New Roman", 14, FontStyle.Bold)
                    If Val(dt1.Rows(0)("GroupBal").ToString()) > 0 Then
                        .Cells(3).Value = Format(Math.Abs(Val(dt1.Rows(0)("GroupBal").ToString())), "0.00")
                        .Cells(3).Style.Font = New Font("Times New Roman", 14, FontStyle.Bold)
                        '   drtot = drtot + .Cells(3).Value
                    Else
                        .Cells(4).Value = Format(Math.Abs(Val(dt1.Rows(0)("GroupBal").ToString())), "0.00")
                        .Cells(4).Style.Font = New Font("Times New Roman", 14, FontStyle.Bold)
                        '  crtot = crtot + .Cells(4).Value
                    End If
                    lastval = lastval + 1
                End With
            End If
            If Application.OpenForms().OfType(Of Trial_Balance_Grouped).Any = False Then Exit Sub
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                If TransType = "" Then
                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
                    .Cells(2).Value = "     " & dt.Rows(i)("AccountName").ToString()
                ElseIf TransType = dt1.Rows(0)("GroupName").ToString() Then
                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
                    .Cells(2).Value = "     " & dt.Rows(i)("AccountName").ToString()
                Else
                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
                    .Cells(2).Value = "     " & dt.Rows(i)("AccountName").ToString()
                End If

                If Val(dt.Rows(i)("Restbal").ToString()) > 0 Then
                    .Cells(3).Value = Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00")
                    drtot = drtot + .Cells(3).Value
                    DebitTotal = DebitTotal + .Cells(3).Value
                Else
                    .Cells(4).Value = Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00")
                    crtot = crtot + .Cells(4).Value
                    CreditTotal = CreditTotal + .Cells(4).Value
                End If
                TransType = dt1.Rows(0)("GroupName").ToString()
                lastval = lastval + 1
            End With
            'dg1.Rows(i).Cells(3).Value = DebitTotal
            'dg1.Rows(i).Cells(4).Value = CreditTotal
        Next
        If drtot > crtot Then
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                dg1.Rows(lastval).Cells(1).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(2).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(3).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(4).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(2).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(4).Style.ForeColor = Color.GhostWhite
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(2).Value = "Diffrence in Opening Balance"
                .Cells(2).Style.Font = New Font("Times New Roman", 14, FontStyle.Bold)
                .Cells(4).Style.Font = New Font("Times New Roman", 14, FontStyle.Bold)
                .Cells(4).Value = Format(Math.Abs(Val(drtot - crtot)), "0.00")
                crtot = crtot + .Cells(4).Value

            End With
        ElseIf drtot < crtot Then
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                dg1.Rows(lastval).Cells(1).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(2).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(3).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(4).Style.BackColor = Color.Tan
                dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(2).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.GhostWhite
                dg1.Rows(lastval).Cells(4).Style.ForeColor = Color.GhostWhite
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(2).Value = "Diffrence in Opening Balance"
                .Cells(2).Style.Font = New Font("Times New Roman", 14, FontStyle.Bold)
                .Cells(3).Style.Font = New Font("Times New Roman", 14, FontStyle.Bold)
                .Cells(3).Value = Format(Math.Abs(Val(crtot - drtot)), "0.00")
                drtot = drtot + .Cells(3).Value
            End With
        End If
        txtDramt.Text = Format(Val(drtot), "0.00")
        txtcrAmt.Text = Format(Val(crtot), "0.00")
        'If dg1.RowCount = 0 Then calc() : Exit Sub
        'If crtot > drtot Then
        '    dg1.Rows.Add()
        '    dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
        '    dg1.Rows(tmpval).Cells(3).Value = Format(Val(crtot - drtot), "0.00")
        'ElseIf crtot < drtot Then
        '    dg1.Rows.Add()
        '    dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
        '    dg1.Rows(tmpval).Cells(4).Value = Format(Val(drtot - crtot), "0.00")
        'End If
        dg1.ClearSelection()
        pnlWait.Visible = False
    End Sub
    Private Sub calc()
        txtDramt.Text = Format(0, "0.00") : txtcrAmt.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtDramt.Text = Format(Val(txtDramt.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtcrAmt.Text = Format(Val(txtcrAmt.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
        Next
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,P1, P2,P3, P4,P5) values('" & mskEntryDate.Text & "'," & _
                    "'" & .Cells(2).Value & "','" & IIf(.Cells(3).Value = "", "", Format(Val(.Cells(3).Value), "0.00")) & "', " & _
                    "'" & IIf(.Cells(4).Value = "", "", Format(Val(.Cells(4).Value), "0.00")) & "','" & Format(Val(txtDramt.Text), "0.00") & "','" & Format(Val(txtcrAmt.Text), "0.00") & "');"
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
        Report_Viewer.printReport("\Reports\TrailBalanceGroup.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub
End Class