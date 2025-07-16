Public Class Seller_Day_Book

    Private Sub Trail_Balance_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Trail_Balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Dim MinDate As String : Dim MaxDate As String
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MinDate = CDate(clsFun.ExecScalarStr("Select Max (EntryDate) From Transaction2")).ToString("dd-MM-yyyy")
        MaxDate = CDate(clsFun.ExecScalarStr("Select Max (EntryDate) From Transaction2")).ToString("dd-MM-yyyy")
        mskFromDate.Text = If(MinDate = "", Date.Today.ToString("dd-MM-yyyy"), MinDate)
        MsktoDate.Text = If(MaxDate = "", Date.Today.ToString("dd-MM-yyyy"), MaxDate)
        Me.KeyPreview = True : rowColums()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs)
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

    Private Sub rowColums()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Particulars"
        dg1.Columns(1).Width = 520
        dg1.Columns(2).Name = "Credit"
        dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "Particulars"
        dg1.Columns(3).Width = 450
        dg1.Columns(4).Name = "Debit"
        dg1.Columns(4).Width = 100
        rowColumsLibilities() : rowColumsAssests()
    End Sub
    Private Sub rowColumsLibilities()
        DgLibilities.ColumnCount = 2
        DgLibilities.Columns(0).Name = "Particulars"
        DgLibilities.Columns(0).Width = 300
        DgLibilities.Columns(1).Name = "Amount(Cr)"
        DgLibilities.Columns(1).Width = 100
    End Sub
    Private Sub rowColumsAssests()
        dgAssests.ColumnCount = 2
        dgAssests.Columns(0).Name = "Particulars"
        dgAssests.Columns(0).Width = 300
        dgAssests.Columns(1).Name = "Amount(Cr)"
        dgAssests.Columns(1).Width = 100
    End Sub
    Private Sub CreditSide()
        DgLibilities.Rows.Clear()
        dgAssests.Rows.Clear()
        dg1.Rows.Clear()
        Dim AddCr As Decimal = 0
        Dim lastval As Integer = 0
        Dim OpeningCash As Decimal
        Dim Drbal As Decimal = Val(clsFun.ExecScalarStr("Select Sum(Amount) From Ledger Where AccountID=7 and DC='D' and EntryDate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
        Dim CrBal As Decimal = Val(clsFun.ExecScalarStr("Select Sum(Amount) From Ledger Where AccountID=7 and DC='C' and EntryDate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'"))
        OpeningCash = Val(Drbal) - Val(CrBal)
        DgLibilities.Rows.Add()
        With DgLibilities.Rows(lastval)
            .Cells(0).Value = "Opening Cash "
            .Cells(1).Value = Format(Val(OpeningCash))
            lastval = lastval + 1
            AddCr = Val(AddCr) + Val(.Cells(1).Value)
        End With
        Dim dt As New DataTable
        Dim sql As String
        sql = "Select T2.SallerName ||' (Vehicle No.: ' || VehicleNo || ') Sold Nugs : ' || Sum(T2.Nug) as Particulars,Sum(T2.Amount) as Amount From   Transaction2 " & _
            "as T2  Inner Join Vouchers V on T2.PurchaseID=V.ID Where T2.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group by V.VehicleNo Order by T2.SallerName"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = dt.Rows(i)("Particulars").ToString()
                .Cells(1).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next
        sql = ""
        sql = "Select '(Other Sale) ' ||' Sold Nugs : ' || Sum(Nug) as Particulars,Sum(Amount) as Amount From   Transaction2 " & _
            "Where TransType Not In('Stock Sale','Standard Sale') and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = dt.Rows(i)("Particulars").ToString()
                .Cells(1).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next

        DgLibilities.Rows.Add()
        DgLibilities.Rows.Add()
        lastval = lastval + 2
        sql = ""
        sql = "Select Sum(CommAmt) as CommAmt From Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = "Aadhat/Commission "
                .Cells(1).Value = Format(Val(dt.Rows(i)("CommAmt").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next
        sql = ""
        sql = "Select Sum(MAmt) as MandiTax From Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = "Mandi Tax "
                .Cells(1).Value = Format(Val(dt.Rows(i)("MandiTax").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next
        sql = ""
        sql = "Select Sum(RdfAmt) as RdfAmt From Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = "Rdf Amount "
                .Cells(1).Value = Format(Val(dt.Rows(i)("RdfAmt").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next

        sql = ""
        sql = "Select Sum(TareAmt) as TareAmt From Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = "Bardana "
                .Cells(1).Value = Format(Val(dt.Rows(i)("TareAmt").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next

        sql = ""
        sql = "Select Sum(LabourAmt) as LabourAmt From Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = "Labour "
                .Cells(1).Value = Format(Val(dt.Rows(i)("LabourAmt").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next

        sql = ""
        sql = "Select Sum(RoundOff) as RoundOff From Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = "Round Off "
                .Cells(1).Value = Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next
        Dim TotalSale As Decimal = 0
        DgLibilities.Rows.Add()
        lastval = lastval + 1
        DgLibilities.Rows.Add()
        With DgLibilities.Rows(lastval)
            .Cells(0).Value = "(Total Sale Amount) : " & Format(Val(AddCr) - Val(OpeningCash), "0.00")
            TotalSale = Format(Val(AddCr) - Val(OpeningCash), "0.00")
            lastval = lastval + 1
        End With
        sql = ""
        sql = "Select AccountName, Sum(Amount) as Amount From Ledger Where AccountID=7 and TransType In ('Receipt', 'Group Receipt') and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group by AccountID"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            DgLibilities.Rows.Add()
            With DgLibilities.Rows(lastval)
                .Cells(0).Value = " Total Receipts"
                .Cells(1).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                AddCr = Val(AddCr) + Val(.Cells(1).Value) + Val(TotalSale)
                lastval = lastval + 1
            End With
        Next


        lastval = 0
        Dim AddDr As Decimal = 0
        sql = ""
        sql = "Select AccountName, Sum(Amount) as Amount From Ledger Where TransType In ('Payment', 'Group Payment') and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group by AccountID"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            dgAssests.Rows.Add()
            With dgAssests.Rows(lastval)
                .Cells(0).Value = dt.Rows(i)("AccountName").ToString()
                .Cells(1).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                AddDr = Val(AddDr) + Val(.Cells(1).Value)
                lastval = lastval + 1
            End With
        Next
        dgAssests.Rows.Add()
        With dgAssests.Rows(lastval)
            .Cells(0).Value = "Ugahi (Total Sale Amount) "
            .Cells(1).Value = Format(Val(TotalSale), "0.00")
            ' AddDr = Format(Val(AddDr) + Val(TotalSale), "0.00")
        End With
        lastval = 0
        Dim DrTotal As Decimal = 0
        Dim CrTotal As Decimal = 0
        For i As Integer = 0 To IIf(DgLibilities.Rows.Count > dgAssests.Rows.Count, DgLibilities.Rows.Count, dgAssests.Rows.Count) - 1
            dg1.Rows.Add()
            If DgLibilities.Rows.Count > i Then
                dg1.Rows(i).Cells(1).Value = DgLibilities.Rows(i).Cells(0).Value
                dg1.Rows(i).Cells(2).Value = DgLibilities.Rows(i).Cells(1).Value
                CrTotal = Val(CrTotal) + Val(DgLibilities.Rows(i).Cells(1).Value)
            End If
            If dgAssests.Rows.Count > i Then
                dg1.Rows(i).Cells(3).Value = dgAssests.Rows(i).Cells(0).Value
                dg1.Rows(i).Cells(4).Value = dgAssests.Rows(i).Cells(1).Value
                DrTotal = Val(DrTotal) + Val(dgAssests.Rows(i).Cells(1).Value)
            End If
            lastval = lastval + 1
        Next
        dg1.Rows.Add()
        With dg1.Rows(lastval)
            .Cells(3).Value = "---------------"
            .Cells(4).Value = "---------------"
            lastval = lastval + 1
        End With
        dg1.Rows.Add()
        With dg1.Rows(lastval)
            .Cells(3).Value = ""
            .Cells(4).Value = Format(Val(DrTotal), "0.00")
            lastval = lastval + 1
        End With
        Dim ClosingBal As Decimal = Format(Val(CrTotal) - Val(DrTotal), "0.00")
        dg1.Rows.Add()
        With dg1.Rows(lastval)
            If ClosingBal < 0 Then
                .Cells(1).Value = "Closing Bal (Cash)"
                .Cells(2).Value = Format(Val(ClosingBal), "0.00")
                CrTotal = Val(CrTotal) + Val(.Cells(2).Value)
            Else
                .Cells(3).Value = "Closing Bal (Cash)"
                .Cells(4).Value = Format(Val(ClosingBal), "0.00")
                DrTotal = Val(DrTotal) + Val(.Cells(4).Value)
            End If
            lastval = lastval + 1
        End With

        dg1.Rows.Add()

        With dg1.Rows(lastval)
            .Cells(1).Value = "Grand Total"
            .Cells(2).Value = Format(Val(CrTotal), "0.00")
            .Cells(3).Value = "Grand Total"
            .Cells(4).Value = Format(Val(DrTotal), "0.00")
            lastval = lastval + 1
        End With
        dg1.ClearSelection()

    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        CreditSide()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & IIf(.Cells(2).Value = "", "", Format(Val(.Cells(2).Value), "0.00")) & "', " & _
                    "'" & .Cells(3).Value & "','" & IIf(.Cells(4).Value = "", "", Format(Val(.Cells(4).Value), "0.00")) & "');"
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
        Report_Viewer.printReport("\Reports\SallerReport.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
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