Public Class Supplier_VS_Item_Summary

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub Market_Tax_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        mskFromDate.SelectAll()
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Market_Tax_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from transaction2 ")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from transaction2")
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
    Private Sub rowColums()
        dg1.ColumnCount = 12
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "Voucher No." : dg1.Columns(2).Width = 60
        dg1.Columns(3).Name = "Vehicle" : dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Seller" : dg1.Columns(4).Width = 300
        dg1.Columns(5).Name = "Item" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Lot" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Nug" : dg1.Columns(7).Width = 100
        dg1.Columns(8).Name = "Kg" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "Rate" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "Per" : dg1.Columns(10).Width = 50
        dg1.Columns(11).Name = "Basic" : dg1.Columns(11).Width = 100
        'dg1.Columns(12).Name = "Charges" : dg1.Columns(12).Width = 80
        'dg1.Columns(13).Name = "R.Off" : dg1.Columns(13).Width = 60
        'dg1.Columns(14).Name = "Total" : dg1.Columns(14).Width = 100
    End Sub
   Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") ': txtTotCharge.Text = Format(0, "0.00")
        'TxtGrandTotal.Text = Format(0, "0.00") : txtTotROff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            'txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            'txtTotROff.Text = Format(Val(txtTotROff.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            'TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
        Next
    End Sub
    Private Sub retrive(Optional ByVal Primary As String = "", Optional ByVal Secondary As String = "")
        dg1.Rows.Clear()
        Dim sql As String = String.Empty
        Dim dt As New DataTable
        ''dt = clsFun.ExecDataTable("Select * FROM Stock_Sale_Report Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'  " & Primary & "" & Secondary & "  order by EntryDate,BillNo,Voucherid ")
        'sql = "Select VoucherID,EntryDate,BillNo,VehicleNo,SallerName,ItemName,Lot,Sum(Nug) as Nug,Sum(Weight) as Weight,Round(Avg(Rate),2) as Rate,Per,sum(Amount) as Amount," &
        '    "Round(sum(Charges),2) as Charges,sum(TotalAmount) as TotalAmount FROM Stock_Sale_Report  Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'" &
        '    " " & Primary & "" & Secondary & "  Group By EntryDate,SallerName,Lot,ItemName,Per  Order by EntryDate,BillNo,Voucherid"
        sql = "SELECT T.VoucherID, T.EntryDate, T.BillNo, V.VehicleNo, V.SallerName, T.ItemName, T.Lot, " &
      "SUM(T.Nug) AS Nug, SUM(T.Weight) AS Weight, " &
      "avg(T.SRate) AS Rate, T.Per, SUM(T.Amount) AS Amount, " &
      "ROUND(SUM(T.Charges), 2) AS Charges, SUM(T.TotalAmount) AS TotalAmount " &
      "FROM Transaction2 T " &
      "INNER JOIN Vouchers V ON V.ID = T.VoucherID " &
      "WHERE T.EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " &
      Primary & " " & Secondary & " " &
      "GROUP BY T.EntryDate, T.SallerName, T.Lot, T.ItemName,T.SRate, T.Per " &
      "ORDER BY T.EntryDate, T.BillNo, T.VoucherID"

        dt = clsFun.ExecDataTable(sql)
        Dim vchid As Integer = 0
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        If i = 0 Then
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("SallerName").ToString()
                        ElseIf vchid = dt.Rows(i)("Voucherid").ToString() Then
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                            .Cells(4).Value = ""
                        Else
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("SallerName").ToString()
                        End If
                        .Cells(5).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(6).Value = dt.Rows(i)("Lot").ToString()
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(10).Value = dt.Rows(i)("Per").ToString()
                        .Cells(11).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        '.Cells(12).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        '.Cells(13).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        '.Cells(14).Value = Format(Val(.Cells(14).Value) - Val(Val(.Cells(12).Value) + Val(.Cells(11).Value)), "0.00")
                    End With
                    vchid = dt.Rows(i)("Voucherid").ToString()
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

    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                sql = sql & "insert into Printing(D1,D2,P1,P2, P3, P4,P5, P6, P7, P8,P9,P10,P11,P12,P13,P14) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "'," & _
                    "'" & Val(.Cells(6).Value) & "','" & Val(.Cells(7).Value) & "','" & Val(.Cells(8).Value) & "','" & Val(.Cells(9).Value) & "','" & Val(.Cells(10).Value) & "'," & _
                    "'" & Val(.Cells(11).Value) & "','" & Val(txtTotNug.Text) & "','" & Val(txtTotweight.Text) & "','" & Val(txtTotBasic.Text) & "');"
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
        Report_Viewer.printReport("\Reports\SupplierVsItemSummary.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
    End Sub

    Private Sub txtSearchPrimary_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchPrimary.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtSearchPrimary.Text.Trim() <> "" Then
                Primary = "And V.SallerName Like '" & txtSearchPrimary.Text.Trim() & "%'"
                retrive(Primary, Secondary)
            Else
                retrive()
            End If
        End If
    End Sub

    Private Sub txtSearchSecondary_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchSecondary.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtSearchSecondary.Text.Trim() <> "" Then
                Secondary = "And T.txtItem Like '" & txtSearchSecondary.Text.Trim() & "%'"
                retrive(Primary, Secondary)
            Else
                retrive()
            End If
        End If
    End Sub

    Private Sub txtSearchPrimary_TextChanged(sender As Object, e As EventArgs) Handles txtSearchPrimary.TextChanged

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class