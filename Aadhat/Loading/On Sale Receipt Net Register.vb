Public Class On_Sale_Receipt_Net_Register

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub On_Sale_Receipt_Net_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
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
        mskFromDate.Focus()
        rowColums()
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 11
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 80
        dg1.Columns(2).Name = "V.No." : dg1.Columns(2).Width = 60
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 300
        dg1.Columns(4).Name = "Challan" : dg1.Columns(4).Width = 60
        dg1.Columns(5).Name = "Amount" : dg1.Columns(5).Width = 100 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).Name = "Freight" : dg1.Columns(6).Width = 100 : dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).Name = "Labour" : dg1.Columns(7).Width = 120 : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).Name = "Oth. Charges" : dg1.Columns(8).Width = 120 : dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Total Exp." : dg1.Columns(9).Width = 100 : dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "Total" : dg1.Columns(10).Width = 130 : dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear() : Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='Net Receipt' " & condtion & "  order by  InvoiceID,EntryDate ")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    If dt.Rows.Count > 20 Then dg1.Columns(10).Width = 110 Else dg1.Columns(10).Width = 130
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        dg1.Rows(i).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        dg1.Rows(i).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        dg1.Rows(i).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        dg1.Rows(i).Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        dg1.Rows(i).Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        dg1.Rows(i).Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .cells(0).value = dt.Rows(i)("ID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("T2").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("N1").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("N2").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("N3").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Sub calc()
        txtAmount.Text = Format(0, "0.00") : txtTotFreight.Text = Format(0, "0.00")
        txtTotLabour.Text = Format(0, "0.00") : txtTotOthCharge.Text = Format(0, "0.00")
        txtTotExp.Text = Format(0, "0.00") : TxtGrandTotal.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtAmount.Text = Format(Val(txtAmount.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotFreight.Text = Format(Val(txtTotFreight.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotLabour.Text = Format(Val(txtTotLabour.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotOthCharge.Text = Format(Val(txtTotOthCharge.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotExp.Text = Format(Val(txtTotExp.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
        Next
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            OnSaleReceipt_Net.MdiParent = MainScreenForm
            OnSaleReceipt_Net.Show()
            OnSaleReceipt_Net.FillControls(Val(dg1.SelectedRows(0).Cells(0).Value))
            OnSaleReceipt_Net.BringToFront()
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        OnSaleReceipt_Net.MdiParent = MainScreenForm
        OnSaleReceipt_Net.Show()
        OnSaleReceipt_Net.FillControls(Val(dg1.SelectedRows(0).Cells(0).Value))
        OnSaleReceipt_Net.BringToFront()
    End Sub

End Class