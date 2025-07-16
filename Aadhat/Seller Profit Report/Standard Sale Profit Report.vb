Public Class Standard_Sale_Profit_Report

    Private Sub Scrip_Profit_Report_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Scrip_Profit_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from Purchase ")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from Purchase ")
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
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text) : MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
        rowColums()
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
        e.SuppressKeyPress = True
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 16
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 80
        dg1.Columns(2).Name = "V.No." : dg1.Columns(2).Width = 60
        dg1.Columns(3).Name = "Vehicle" : dg1.Columns(3).Width = 110
        dg1.Columns(4).Name = "Seller Name" : dg1.Columns(4).Width = 120
        dg1.Columns(5).Name = "Type" : dg1.Columns(5).Width = 90
        dg1.Columns(6).Name = "PNug" : dg1.Columns(6).Width = 50
        dg1.Columns(7).Name = "SNug" : dg1.Columns(7).Width = 50
        dg1.Columns(8).Name = "BNug" : dg1.Columns(8).Width = 50
        dg1.Columns(9).Name = "PWeight" : dg1.Columns(9).Width = 70
        dg1.Columns(10).Name = "SWeight" : dg1.Columns(10).Width = 70
        dg1.Columns(11).Name = "SAmt" : dg1.Columns(11).Width = 80
        dg1.Columns(12).Name = "PAmt" : dg1.Columns(12).Width = 80
        dg1.Columns(13).Name = "P & L" : dg1.Columns(13).Width = 90
        dg1.Columns(14).Name = "Comm" : dg1.Columns(14).Width = 90
        dg1.Columns(15).Name = "P & L" : dg1.Columns(15).Width = 90
    End Sub
    Sub calc()
        txtTotalNug.Text = Format(0, "0.00") : txtTotalWeight.Text = Format(0, "0.00")
        txtTotSale.Text = Format(0, "0.00") : txtPurchase.Text = Format(0, "0.00")
        txtPNL.Text = Format(0, "0.00") : txtCommission.Text = Format(0, "0.00")
        txtActualPNL.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNug.Text = Format(Val(txtTotalNug.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotalWeight.Text = Format(Val(txtTotalWeight.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotSale.Text = Format(Val(txtTotSale.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            txtPurchase.Text = Format(Val(txtPurchase.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            txtPNL.Text = Format(Val(txtPNL.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            txtCommission.Text = Format(Val(txtCommission.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
            txtActualPNL.Text = Format(Val(txtActualPNL.Text) + Val(dg1.Rows(i).Cells(15).Value), "0.00")
        Next
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()

    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim i As Integer : Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                dg1.ClearSelection()
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                    .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                    .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                    .Cells(6).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Purchase where voucherID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    .Cells(7).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    .Cells(8).Value = Format(Val(Val(.Cells(6).Value) - Val(.Cells(7).Value)), "0.00")
                    .Cells(9).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Weight) from Purchase where voucherID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    .Cells(10).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Weight) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If clsFun.ExecScalarStr(" Select TransType from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "") = "Standard Sale" Then
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select SallerAmt from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    Else
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        Dim TotCharges As String = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Vouchers where ID = " & dt.Rows(i)("VoucherID").ToString() & "")) * Val(txtCal.Text), "0.00")
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(BasicAmount) from Vouchers where ID = " & dt.Rows(i)("VoucherID").ToString() & "") + Val(TotCharges)), "0.00")
                    Else
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(amount) from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If
                    .Cells(14).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from ChargesTrans where  ChargesID=7 and VoucherID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    .Cells(15).Value = Format(Val(Val(.Cells(14).Value) + Val(.Cells(13).Value)), "0.00")
                End With
            Next i
        End If
        dg1.ClearSelection() : calc()
    End Sub
    Private Sub RetriveChargeAlso()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Group by VoucherID"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                dg1.ClearSelection()
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                    .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                    .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                    .Cells(6).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Purchase where voucherID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    .Cells(7).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    .Cells(8).Value = Format(Val(Val(.Cells(6).Value) - Val(.Cells(7).Value)), "0.00")
                    .Cells(9).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Weight) from Purchase where voucherID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    .Cells(10).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Weight) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If clsFun.ExecScalarStr(" Select TransType from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "") = "Standard Sale" Then
                        Dim AddCharges As String = Format(Val(clsFun.ExecScalarStr(" Select SallerAmt from Charges where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select SallerAmt from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")

                    Else
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    Else
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(amount) from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If

                    '.Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    '.Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                    '.Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    '.Cells(5).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                    '.Cells(2).Value = Math.Abs(Val(tmpamt)) & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts  WHERE id = " & dt.Rows(i)("Id").ToString() & "")
                End With
            Next i
        End If
        dg1.ClearSelection()
        'calc()
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            Purchase.MdiParent = MainScreenForm
            Purchase.Show()
            Purchase.FillControls(tmpID)
            If Not Standard_Sale Is Nothing Then
                Purchase.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Purchase.MdiParent = MainScreenForm
        Purchase.Show()
        Purchase.FillControls(tmpID)
        If Not Purchase Is Nothing Then
            Purchase.BringToFront()
        End If
    End Sub
    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        If dg1.Rows.Count = 0 Then MsgBox("There is No Record to Print...", MsgBoxStyle.Critical, "No Record") : Exit Sub
        PrintRecord()
        Report_Viewer.printReport("\Reports\SellerProfitReport.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Registers_Viewer Is Nothing Then
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
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        pnlWait.Visible = True
        For Each row As DataGridViewRow In dg1.Rows
            Application.DoEvents()
            pb1.Minimum = 0 : pb1.Maximum = dg1.Rows.Count
            With row
                pb1.Value = Val(row.Index)
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20,P21,P22) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                    "'" & .Cells(5).Value & "'," & Format(Val(.Cells(6).Value), "0.00") & ",'" & Format(Val(.Cells(7).Value), "0.00") & "'," & _
                    "'" & .Cells(8).Value & "'," & Format(Val(.Cells(9).Value), "0.00") & ",'" & Format(Val(.Cells(10).Value), "0.00") & "'," & _
                    "'" & .Cells(11).Value & "'," & Format(Val(.Cells(12).Value), "0.00") & ",'" & Format(Val(.Cells(13).Value), "0.00") & "'," & _
                    "'" & .Cells(14).Value & "'," & Format(Val(.Cells(15).Value), "0.00") & ",'" & Format(Val(txtTotalNug.Text), "0.00") & "'," & _
                     " " & Format(Val(txtTotalWeight.Text), "0.00") & "," & Format(Val(txtTotSale.Text), "0.00") & ",'" & Format(Val(txtPurchase.Text), "0.00") & "'," & _
                    "'" & Format(Val(txtPNL.Text), "0.00") & "'," & Format(Val(txtCommission.Text), "0.00") & ",'" & Format(Val(txtActualPNL.Text), "0.00") & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    pnlWait.Visible = False
                    ClsFunPrimary.CloseConnection()
                End Try
                pnlWait.Visible = False
            End With
        Next
    End Sub
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        PrintRecord()
        Report_Viewer.printReport("\Reports\BOSProfitablity.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        If dg1.RowCount = 0 Then Exit Sub
        If txtSearch.Text.Trim() <> "" Then
            Retrive(" And upper(accountname) Like upper('" & txtSearch.Text.Trim() & "%')")
        Else
            Retrive()
        End If
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class