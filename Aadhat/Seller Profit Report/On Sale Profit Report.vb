Public Class On_Sale_Profit_Report

    Private Sub Scrip_Profit_Report_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Scrip_Profit_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from Transaction2 Where TransType='On Sale' ")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from Transaction2 Where TransType='On Sale' ")
        If mindate <> "" Then mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy") Else mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        If maxdate <> "" Then MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy") Else MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text) : MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
        rowColums() : ckExpAlso.Checked = True
    End Sub


    Private Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 80
        dg1.Columns(2).Name = "Challan No." : dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Vehicle No" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Party Name" : dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "Sent Nug" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Our Cost" : dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "Net Cost" : dg1.Columns(7).Width = 150
        dg1.Columns(8).Name = "P & L" : dg1.Columns(8).Width = 150
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If ckExpAlso.Checked = True Then
            RetriveExpAlso()
        Else
            Retrive()
        End If

    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
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

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub Retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim i As Integer : Dim count As Integer = 0
        ssql = "Select * From Vouchers  Where EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'" & condtion & " and TransType='On Sale'"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                dg1.ClearSelection()
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
                    .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                    .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Transaction2 where TransType='On Sale' and voucherID = " & Val(dt.Rows(i)("ID").ToString() & ""))), "0.00")
                    .Cells(6).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where  VoucherID = " & Val(dt.Rows(i)("ID").ToString() & ""))), "0.00")
                    Dim NetAmt As Decimal = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) from Vouchers  where ItemID = " & Val(dt.Rows(i)("ID").ToString()) & "")), "0.00")
                    .Cells(7).Value = IIf(NetAmt > 0, NetAmt, "Waiting")
                    .Cells(8).Value = IIf(NetAmt > 0, Format(Val(.Cells(7).Value) - Val(.Cells(6).Value), "0.00"), "Not Recived")
                End With
            Next i
        End If
        dg1.ClearSelection() : calc()
    End Sub

    Private Sub RetriveExpAlso(Optional ByVal condtion As String = "")
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim i As Integer : Dim count As Integer = 0
        ssql = "Select * From Vouchers  Where EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & " '" & condtion & " and TransType='On Sale'"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                dg1.ClearSelection()
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
                    .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                    .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Transaction2 where TransType='On Sale' and voucherID = " & Val(dt.Rows(i)("ID").ToString() & ""))), "0.00")
                    Dim TotPurchaseCharges As String = dt.Rows(i)("TotalCharges").ToString() ' Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from ChargesTrans Where    VoucherID = " & Val(dt.Rows(i)("ID").ToString() & ""))), "0.00")
                    .Cells(6).Value = Val(dt.Rows(i)("BasicAmount").ToString()) + Val(dt.Rows(i)("TotalCharges").ToString()) 'Format(Val(clsFun.ExecScalarStr(" Select BasicAmount from Vouchers Where TransType='On Sale' and ID = " & Val(dt.Rows(i)("ID").ToString()) & "")) + TotPurchaseCharges, "0.00")
                    Dim RecID As Integer = Val(clsFun.ExecScalarStr("Select ID from Vouchers where   ItemID = " & Val(dt.Rows(i)("ID").ToString()) & ""))
                    Dim NetAmt As Decimal = Format(Val(clsFun.ExecScalarStr("Select TotalAmount from Vouchers where   ID = " & RecID & "")), "0.00")
                    .Cells(7).Value = IIf(NetAmt > 0, NetAmt, "Waiting")
                    .Cells(8).Value = IIf(NetAmt > 0, Format(Val(.Cells(7).Value) - Val(.Cells(6).Value), "0.00"), "Not Recived")
                End With
            Next i
        End If
        dg1.ClearSelection() : calc()
    End Sub
    Sub calc()
        txtSentQty.Text = Format(0, "0.00") : txtOurCost.Text = Format(0, "0.00")
        txtNetCost.Text = Format(0, "0.00") : txtPNL.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtSentQty.Text = Format(Val(txtSentQty.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtOurCost.Text = Format(Val(txtOurCost.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtNetCost.Text = Format(Val(txtNetCost.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtPNL.Text = Format(Val(txtPNL.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
        Next
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
            On_Sale.MdiParent = MainScreenForm
            On_Sale.Show()
            On_Sale.FillControl(tmpID)
            If Not On_Sale Is Nothing Then
                On_Sale.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
        On_Sale.MdiParent = MainScreenForm
        On_Sale.Show()
        On_Sale.FillControl(tmpID)
        If Not On_Sale Is Nothing Then
            On_Sale.BringToFront()
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dg1.Rows.Count = 0 Then MsgBox("There is No Record to Print...", MsgBoxStyle.Critical, "No Record") : Exit Sub
        PrintRecord()
        Report_Viewer.printReport("\Reports\OnSaleProfitReport.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Registers_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9, P10,P11,P12) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                    "'" & .Cells(5).Value & "','" & Format(Val(.Cells(6).Value), "0.00") & "','" & Format(Val(.Cells(7).Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells(8).Value), "0.00") & "','" & Format(Val(txtSentQty.Text), "0.00") & "', " & _
                    "'" & Format(Val(txtOurCost.Text), "0.00") & "','" & Format(Val(txtNetCost.Text), "0.00") & "','" & Format(Val(txtPNL.Text), "0.00") & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
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

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtSearch.Text.Trim() <> "" Then
                If ckExpAlso.Checked = True Then RetriveExpAlso(" And AccountName Like '" & txtSearch.Text.Trim() & "%' ")
                If ckExpAlso.Checked = False Then Retrive(" And AccountName Like '" & txtSearch.Text.Trim() & "%' ")
            End If
            If txtSearch.Text.Trim() = "" Then
                If ckExpAlso.Checked = True Then RetriveExpAlso() Else Retrive()
            End If
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class