Public Class CashBookPaymentDayWise
    Private opbal As Decimal = 0.0
    Private Sub Cash_Bank_Book_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles MskEntryDate.KeyDown, cbAccountName.KeyDown
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
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles MskEntryDate.GotFocus, MskEntryDate.Click
        MskEntryDate.SelectionStart = 0 : MskEntryDate.SelectionLength = Len(MskEntryDate.Text)
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MskEntryDate.Validating
        MskEntryDate.Text = clsFun.convdate(MskEntryDate.Text)
    End Sub

    Private Sub MsktoDate_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then btnShow.Focus()
    End Sub

    Private Sub Cash_Bank_Book_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        clsFun.FillDropDownList(cbAccountName, "Select * From Accounts where groupid in(11,12)", "AccountName", "Id", "")
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        MskEntryDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyyy"))
        MskEntryDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyyy"))
        rowColumsLibilities() : rowColumsAssests() : rowColums() : cbAccountName.Focus() : Me.KeyPreview = True
    End Sub
    Private Sub rowColumsLibilities()
        DgLibilities.ColumnCount = 4
        DgLibilities.Columns(0).Name = "ID"
        DgLibilities.Columns(0).Visible = False
        DgLibilities.Columns(1).Name = "Date"
        DgLibilities.Columns(1).Width = 300
        DgLibilities.Columns(2).Name = "TransType"
        DgLibilities.Columns(2).Width = 300
        DgLibilities.Columns(3).Name = "Amount"
        DgLibilities.Columns(3).Width = 200
    End Sub
    Private Sub rowColumsAssests()
        dgAssests.ColumnCount = 4
        dgAssests.Columns(0).Name = "ID"
        dgAssests.Columns(0).Visible = False
        dgAssests.Columns(1).Name = "Date"
        dgAssests.Columns(1).Width = 300
        dgAssests.Columns(2).Name = "TransType"
        dgAssests.Columns(2).Width = 300
        dgAssests.Columns(3).Name = "Amount"
        dgAssests.Columns(3).Width = 200
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        MskEntryDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        MskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        MskEntryDate.Text = clsFun.convdate(MskEntryDate.Text)
    End Sub

    Private Sub cbAccountName_Leave(sender As Object, e As EventArgs) Handles cbAccountName.Leave
        If clsFun.ExecScalarInt("Select count(*)from Accounts") = 0 Then
            Exit Sub
        End If
        If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & cbAccountName.Text & "'") = 0 Then
            MsgBox("Account Name Not Found in Database...", vbOKOnly, "Access Denied")
            cbAccountName.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 8
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Disc." : dg1.Columns(1).Width = 360
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Visible = False
        dg1.Columns(3).Name = "Receipt" : dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "||" : dg1.Columns(4).Width = 50
        dg1.Columns(5).Name = "Disc." : dg1.Columns(5).Width = 360
        dg1.Columns(6).Name = "Type" : dg1.Columns(6).Visible = False
        dg1.Columns(7).Name = "Payment" : dg1.Columns(7).Width = 200
    End Sub
    Private Sub RetriveTemp()
        DgLibilities.Rows.Clear() : dgAssests.Rows.Clear() : lastval = 0
        Dim dt As New DataTable : Dim dt1 As New DataTable : Dim dt2 As New DataTable
        Dim Sql As String = String.Empty
        Sql = "Select sum(Amount) as Total From Ledger Where EntryDate='" & CDate(Me.MskEntryDate.Text).ToString("yyyy-MM-dd") & "'  and TransType='Receipt' and DC='D' And PartyID in(Select ID From Accounts Where GroupID=32)" & _
               " and AccountID in (" & Val(cbAccountName.SelectedValue) & ",17);"
        dt = clsFun.ExecDataTable(Sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = ""
                .Cells(1).Value = "Receipt (Customers)"
                .Cells(2).Value = Val(dt.Rows(i)("Total").ToString()) + Val(clsFun.ExecScalarStr("Select Sum(Amount) as Total From Ledger Where EntryDate='" & CDate(MskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID=17 and DC='D' and TransType='Receipt' and PartyID=" & Val(cbAccountName.SelectedValue) & ";"))
                lastval = lastval + 1
            End With
        Next
        Sql = String.Empty
        Sql = "Select TransType,sum(Amount) as Total From Ledger Where EntryDate='" & CDate(Me.MskEntryDate.Text).ToString("yyyy-MM-dd") & "'  and TransType<>'Receipt' and DC='D' and AccountID=" & Val(cbAccountName.SelectedValue) & " Group by TransType;"
        dt = clsFun.ExecDataTable(Sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = ""
                .Cells(1).Value = dt.Rows(i)("TransType").ToString()
                .Cells(2).Value = dt.Rows(i)("Total").ToString()
                lastval = lastval + 1
            End With
        Next
        Sql = String.Empty
        Sql = "Select PartyName,Amount,TransType as Total From Ledger Where EntryDate='" & CDate(Me.MskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType in ('Receipt','Group Receipt') and DC='D' " & _
            " and PartyID in(Select ID From Accounts Where GroupID<>32)  and AccountID=" & Val(cbAccountName.SelectedValue) & ";"
        dt = clsFun.ExecDataTable(Sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = ""
                .Cells(1).Value = dt.Rows(i)("PartyName").ToString()
                .Cells(2).Value = dt.Rows(i)("Amount").ToString()
                lastval = lastval + 1
            End With
        Next
        Sql = String.Empty : lastval = 0
        Sql = "Select PartyName,TransType,Amount From Ledger Where DC='C' and TransType in ('Payment','Cash Deposit','Group Payment') and EntryDate='" & CDate(Me.MskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID=" & Val(cbAccountName.SelectedValue) & ""
        dt = clsFun.ExecDataTable(Sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            dgAssests.Rows.Add()
            With dgAssests.Rows(lastval)
                .Cells(0).Value = ""
                .Cells(1).Value = dt.Rows(i)("PartyName").ToString()
                .Cells(2).Value = dt.Rows(i)("Amount").ToString()
                lastval = lastval + 1
            End With
        Next


    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        pbWait.Visible = True
        RetriveTemp() : Retrive()
        ' If ckDiscountAlso.Checked = True Then RetriveNew() Else RetriveOld()
        pbWait.Visible = False
        ' Retrive()
        BtnPrint.Focus()
    End Sub
    Private Sub Retrive()
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim Sql As String : Dim lastval As Integer = 0
        Dim Drtotal As Decimal = 0 : Dim CrTotal As Decimal = 0
        Sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
           "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(MskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
           "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(MskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
           " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(MskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
           " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(MskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(cbAccountName.SelectedValue) & "  Order by upper(AccountName) ;"
        dt = clsFun.ExecDataTable(Sql)
        If dt.Rows.Count > 20 Then dg1.Columns(5).Width = 355 Else dg1.Columns(5).Width = 360
        dg1.Rows.Clear()
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                dg1.Rows.Add()
                If Val(dt.Rows(i)("RestBal").ToString()) > 0 Then
                    dg1.Rows(lastval).Cells(1).Value = "Opening Balance"
                    dg1.Rows(lastval).Cells(3).Value = Format(Val(dt.Rows(i)("RestBal").ToString()), "0.00")
                    dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    Drtotal = Val(Drtotal) + Val(dt.Rows(i)("RestBal").ToString())
                Else
                    dg1.Rows(lastval).Cells(5).Value = "Opening Balance"
                    dg1.Rows(lastval).Cells(7).Value = Format(Val(dt.Rows(i)("RestBal").ToString()), "0.00")
                    dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    CrTotal = Val(CrTotal) + Val(dt.Rows(i)("RestBal").ToString())
                End If
                dg1.Rows(lastval).Cells(4).Value = "|"
                lastval = lastval + 1
            Next
        End If
        For j As Integer = 0 To IIf(DgLibilities.Rows.Count > dgAssests.Rows.Count, DgLibilities.Rows.Count, dgAssests.Rows.Count) - 1
            dg1.Rows.Add()
            If DgLibilities.Rows.Count > j Then
                dg1.Rows(lastval).Cells(1).Value = DgLibilities.Rows(j).Cells(1).Value
                dg1.Rows(lastval).Cells(3).Value = Format(Val(DgLibilities.Rows(j).Cells(2).Value), "0.00")
                dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                Drtotal = Val(Drtotal) + Val(DgLibilities.Rows(j).Cells(2).Value)
            End If
            If dgAssests.Rows.Count > j Then
                dg1.Rows(lastval).Cells(5).Value = dgAssests.Rows(j).Cells(1).Value
                dg1.Rows(lastval).Cells(7).Value = Format(Val(dgAssests.Rows(j).Cells(2).Value), "0.00")
                dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                CrTotal = Val(CrTotal) + Val(dgAssests.Rows(j).Cells(2).Value)
            End If
            dg1.Rows(lastval).Cells(4).Value = "|"
            lastval = lastval + 1
        Next
        Sql = "Select Sum(Amount) as Total From Ledger Where EntryDate='" & CDate(MskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID=17 and DC='D' and TransType='Receipt' and PartyID=" & Val(cbAccountName.SelectedValue) & ";"
        dt = clsFun.ExecDataTable(Sql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If Val(dt.Rows(i)("Total").ToString()) > 0 Then
                    dg1.Rows.Add()
                    dg1.Rows(lastval).Cells(5).Value = "Discount (Receipts)"
                    dg1.Rows(lastval).Cells(7).Value = Format(Val(dt.Rows(i)("Total").ToString()), "0.00")
                    dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    CrTotal = Val(CrTotal) + Val(dt.Rows(i)("Total").ToString())
                    dg1.Rows(lastval).Cells(4).Value = "|"
                    lastval = lastval + 1
                End If
            Next
        End If
        If Val(Drtotal) > Val(CrTotal) Then
            dg1.Rows.Add()
            dg1.Rows(lastval).Cells(5).Value = "Closing Balance"
            dg1.Rows(lastval).Cells(7).Value = Format(Val(Val(Drtotal) - Val(CrTotal)), "0.00")
            dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            CrTotal = Val(CrTotal) + Val(dg1.Rows(lastval).Cells(7).Value)
            dg1.Rows(lastval).Cells(4).Value = "|"
            lastval = lastval + 1
        Else
            dg1.Rows.Add()
            dg1.Rows(lastval).Cells(1).Value = "Closing Balance"
            dg1.Rows(lastval).Cells(3).Value = Format(Val(Val(CrTotal) - Val(Drtotal)), "0.00")
            dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            Drtotal = Val(Drtotal) + Val(dg1.Rows(lastval).Cells(3).Value)
            dg1.Rows(lastval).Cells(4).Value = "|"
            lastval = lastval + 1
        End If

        dg1.Rows.Add()
        dg1.Rows(lastval).Cells(1).Value = "Grand Total"
        dg1.Rows(lastval).Cells(3).Value = Format(Val(Drtotal), "0.00")
        dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Rows(lastval).Cells(5).Value = "Grand Total"
        dg1.Rows(lastval).Cells(7).Value = Format(Val(CrTotal), "0.00")
        dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Rows(lastval).Cells(4).Value = "|"
        dg1.ClearSelection()
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub PrintRecord()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            Application.DoEvents()
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With dg1.Rows(LastRecord)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & MskEntryDate.Text & "'," & _
                    "'" & MskEntryDate.Text & "','" & cbAccountName.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "'"
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            ' LastRecord = LastCount
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,M1, P1, P2,P3, P4, P5, P6,P7) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        Next
    End Sub
    Private Sub PrintRecord1()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,M1, P1, P2,P3, P4, P5, P6,P7) values('" & MskEntryDate.Text & "'," & _
                    "'" & MskEntryDate.Text & "','" & cbAccountName.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "');"
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        '  clsFun.changeCompany()
        PrintRecord()
        Report_Viewer.printReport("\Cashbook.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub MskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MskEntryDate.MaskInputRejected

    End Sub
End Class