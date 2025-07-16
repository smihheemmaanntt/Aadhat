Public Class On_Sale_Receipt
    Dim vno As Integer : Dim VchId As Integer
    Dim sql As String = String.Empty : Dim count As Integer = 0
    Dim MaxID As String = String.Empty : Dim itemBal As String = String.Empty
    Dim LotBal As String = String.Empty : Dim RestBal As String = String.Empty
    Dim UpdateTmp As String : Dim tmpID As String = String.Empty
    Dim bal As Decimal = 0.0 : Dim curindex As Integer = 0
    Dim CalcType As String = String.Empty : Dim ServerTag As Integer
    Private Sub On_Sale_Receipt_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub On_Sale_Receipt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Cbper.SelectedIndex = 0 : rowColums() : VNumber()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "Item Name" : dg1.Columns(0).Width = 384
        dg1.Columns(1).Name = "Lot No" : dg1.Columns(1).Width = 199
        dg1.Columns(2).Name = "Nug" : dg1.Columns(2).Width = 114
        dg1.Columns(3).Name = "Weight" : dg1.Columns(3).Width = 114
        dg1.Columns(4).Name = "Rate" : dg1.Columns(4).Width = 114
        dg1.Columns(5).Name = "per" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Amount" : dg1.Columns(6).Width = 143
        dg1.Columns(7).Name = "ItemID" : dg1.Columns(7).Width = 142
        dg1.Columns(8).Name = "ID" : dg1.Columns(8).Width = 142
        dg1.Columns(9).Name = "OnSaleID" : dg1.Columns(9).Width = 142
        Dg2.ColumnCount = 7
        Dg2.Columns(0).Name = "Charge Name" : Dg2.Columns(0).Width = 273
        Dg2.Columns(1).Name = "On Value" : Dg2.Columns(1).Width = 127
        Dg2.Columns(2).Name = "Cal" : Dg2.Columns(2).Width = 100
        Dg2.Columns(3).Name = "+/-" : Dg2.Columns(3).Width = 100
        Dg2.Columns(4).Name = "Amount" : Dg2.Columns(4).Width = 150
        Dg2.Columns(5).Name = "ChargeID" : Dg2.Columns(5).Width = 110
        Dg2.Columns(6).Name = "CostOn" : Dg2.Columns(6).Visible = False
    End Sub

    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus
        If dgChallanNo.RowCount = 0 Then txtChallanNo.Focus() : Exit Sub
        If dgChallanNo.SelectedRows.Count = 0 Then txtChallanNo.Focus() : Exit Sub
        txtChallanNo.Text = dgChallanNo.SelectedRows(0).Cells(1).Value
        txtChallanID.Text = dgChallanNo.SelectedRows(0).Cells(0).Value
        dgChallanNo.Visible = False
        txtItem.Focus()

        If dgItemSearch.ColumnCount = 0 Then StockInItemRowColums()
        If dgItemSearch.RowCount = 0 Then StockINretriveItems()
        If txtItem.Text.Trim() <> "" Then
            StockINretriveItems(" And upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            txtItemID.Clear()
            StockINretriveItems()
        End If
        dgItemSearch.Visible = True : dgItemSearch.BringToFront()
        txtItem.SelectionStart = 0 : txtItem.SelectionLength = Len(txtItem.Text)
    End Sub

    Private Sub txtLotNo_GotFocus(sender As Object, e As EventArgs) Handles txtLot.GotFocus
        If dgItemSearch.ColumnCount = 0 Then StockInItemRowColums()
        If dgItemSearch.RowCount = 0 Then StockINretriveItems()
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus() : Exit Sub
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        ItemBalance()
        dgItemSearch.Visible = False
        If dgLot.ColumnCount = 0 Then LotCoulmns()
        If dgLot.RowCount = 0 Then RetriveLot()
        If txtLot.Text.Trim() <> "" Then
            RetriveLot(" And upper(LotNo) like upper('" & txtLot.Text.Trim() & "%')")
        Else
            RetriveLot()
            ' If dgLot.RowCount = 0 Then RetriveLot()
        End If
        txtLot.SelectionStart = 0 : txtLot.SelectionLength = Len(txtLot.Text)
    End Sub
    Private Sub SpeedCalculation()
        If CbPer.SelectedIndex = 0 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtNug.Text) * Val(txtRate.Text), 0), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtKg.Text) * Val(txtRate.Text), 0), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 40 * Val(txtKg.Text), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        End If
    End Sub

    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        If dgLot.ColumnCount = 0 Then LotCoulmns()
        If dgLot.RowCount = 0 Then RetriveLot()
        If dgLot.SelectedRows.Count = 0 Then dgLot.Visible = True : txtLot.Focus() : Exit Sub
        '  txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
        txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
        txtNug.Text = Format(Val(dgLot.SelectedRows(0).Cells(5).Value), "0.00")
        txtKg.Text = Format(Val(dgLot.SelectedRows(0).Cells(6).Value), "0.00")
        txtOnSaleID.Text = Val(dgLot.SelectedRows(0).Cells(7).Value)
        dgLot.Visible = False
        LotBalance()
    End Sub
    Private Sub txtNug_TextChanged(sender As Object, e As EventArgs) Handles txtNug.Leave, Cbper.Leave,
    txtKg.Leave, txtRate.Leave, txtTotbasic.Leave
        SpeedCalculation()

    End Sub
    Private Sub txtOnValue_TextChanged(sender As Object, e As EventArgs) Handles txtOnValue.TextChanged, txtchargesAmount.TextChanged, txtCalculatePer.TextChanged
        ChargesCalculation()
    End Sub
    Private Sub Cbper_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbper.SelectedIndexChanged
        SpeedCalculation()
    End Sub
    Private Sub ChargesCalculation()
        ' If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
        If calctype = "Percentage" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text) / 100), "0.00")
        ElseIf calctype = "Nug" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text)), "0.00")
        ElseIf calctype = "Weight" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text)), "0.00")
        ElseIf calctype = "Aboslute" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text)), "0.00")
        End If
    End Sub
    Private Sub txtOnValue_GotFocus(sender As Object, e As EventArgs) Handles txtOnValue.GotFocus
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        dgCharges.Visible = False : FillCharges()
    End Sub
    Private Sub FillCharges()
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        If CalcType = "Aboslute" Then
            txtOnValue.TabStop = False
            txtCalculatePer.TabStop = False
            txtOnValue.Text = ""
            txtchargesAmount.Focus()
        ElseIf CalcType = "Weight" Then
            txtOnValue.Text = txtTotweight.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Percentage" Then
            txtOnValue.Text = txtTotbasic.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Nug" Then
            txtOnValue.Text = txtTotNug.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        End If
    End Sub
    Private Sub ChargesRowColums()
        dgCharges.ColumnCount = 3
        dgCharges.Columns(0).Name = "ID" : dgCharges.Columns(0).Visible = False
        dgCharges.Columns(1).Name = "Item Name" : dgCharges.Columns(1).Width = 130
        dgCharges.Columns(2).Name = "ApplyType" : dgCharges.Columns(2).Width = 130
        dgCharges.Visible = True : dgItemSearch.BringToFront()
        RetriveCharges()
    End Sub
    Private Sub RetriveCharges(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Charges " & condtion & " order by ChargeName")
        Try
            If dt.Rows.Count > 0 Then
                dgCharges.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgCharges.Rows.Add()
                    With dgCharges.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("ChargeName").ToString()
                        .Cells(2).Value = dt.Rows(i)("ApplyType").ToString()
                    End With
                Next

            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtKg.KeyPress,
        txtRate.KeyPress, txtTotAmount.KeyPress, txtTotCharges.KeyPress, txtOnValue.KeyPress, txtCalculatePer.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave
        If txtNug.Text = "" Then txtNug.Text = Val(0)
        If txtNug.Text > Val(LotBal) Then
            MsgBox("Not Enough Nugs. Please Choose Another Item / Lot ", MsgBoxStyle.Critical, "Zero")
            txtNug.Text = 0 : txtLot.Focus() : Exit Sub
        End If
    End Sub


    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs) Handles txtCharges.GotFocus, txtCharges.Click
        dgItemSearch.Visible = False : DgAccountSearch.Visible = False
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If txtCharges.Text.Trim() <> "" Then
            RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
        Else
            RetriveCharges()
        End If
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        txtCharges.SelectionLength = Len(txtCharges.Text)
    End Sub

    Private Sub txtTotAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotAmount.KeyDown
        If Val(txtItemID.Text) = 0 Then txtItem.Focus() : Exit Sub
        If e.KeyCode = Keys.Enter Then
            If Val(txtNug.Text) = 0 And Val(txtKg.Text) = 0 Then
                MsgBox("Please Fill Nug or Weight...", MsgBoxStyle.Critical, "Empty Qty / Nug / Weight")
                txtNug.Focus()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(0).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(1).Value = txtLot.Text
                dg1.SelectedRows(0).Cells(2).Value = Format(Val(txtNug.Text), "0.00")
                dg1.SelectedRows(0).Cells(3).Value = Format(Val(txtKg.Text), "0.00")
                dg1.SelectedRows(0).Cells(4).Value = Format(Val(txtRate.Text), "0.00")
                dg1.SelectedRows(0).Cells(5).Value = Cbper.Text
                dg1.SelectedRows(0).Cells(6).Value = Format(Val(txtTotAmount.Text), "0.00")
                dg1.SelectedRows(0).Cells(7).Value = txtItemID.Text
                dg1.SelectedRows(0).Cells(9).Value = (txtOnSaleID.Text)
                txtItem.Focus()
                calc()
                dg1.ClearSelection()
                cleartxt()
            Else
                dg1.Rows.Add(txtItem.Text, txtLot.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtKg.Text), "0.00"), Format(Val(txtRate.Text), "0.00"), Cbper.Text, Format(Val(txtTotAmount.Text), "0.00"), txtItemID.Text, 0, Val(txtOnSaleID.Text))
                calc()
                cleartxt()
                txtItem.Focus()
                dg1.ClearSelection()
            End If
        End If
    End Sub
    Sub calc()
        txtTotNug.Text = Format(Val(0), "0.00") : txtTotweight.Text = Format(Val(0), "0.00")
        txtTotbasic.Text = Format(Val(0), "0.00") : txtTotCharges.Text = Format(Val(0), "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtTotbasic.Text = Format(Val(txtTotbasic.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
        Next
        For i = 0 To Dg2.Rows.Count - 1
            If Dg2.Rows(i).Cells(3).Value = "-" Then
                txtTotCharges.Text = Format(Val(txtTotCharges.Text) - Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            Else
                txtTotCharges.Text = Format(Val(txtTotCharges.Text) + Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            End If
        Next
        txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        Dim tmpamount As Double = Val(txtTotTotal.Text)
        txtTotTotal.Text = Math.Round(Val(tmpamount), 0)
        txtRoundOff.Text = Format(Val(txtTotTotal.Text) - Val(tmpamount), "0.00")
        txtTotTotal.Text = Format(Val(txtTotTotal.Text), "0.00")
        Try
            lblInword.Text = AmtInWord(txtTotTotal.Text)
        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
    End Sub
    Private Sub VNumber()
        If vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
            vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        Else
            vno = clsFun.ExecScalarInt("Select Max(InvoiceID)  AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        End If
    End Sub
    Private Sub cleartxt()
        txtLot.Text = ""
        txtNug.Text = ""
        txtKg.Text = ""
        txtTotAmount.Text = ""
        txtOnSaleID.Text = ""
    End Sub
    Private Sub FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        cleartxt() : cleartxtCharges()
        txtTotNug.Text = ""
        txtTotbasic.Text = ""
        txtTotweight.Text = ""
        txtTotTotal.Text = ""
        txtTotCharges.Text = ""
        txtRoundOff.Text = ""
        VNumber()
        ' txtVehicleNo.Text = ""
        txtRate.Text = ""
        mskEntryDate.Focus()
        BtnSave.Text = "&Save"
    End Sub
    Private Sub cleartxtCharges()
        txtOnValue.Text = ""
        txtCalculatePer.Text = ""
        txtPlusMinus.Text = ""
        txtchargesAmount.Text = ""
    End Sub
    Private Sub txtchargesAmount_GotFocus(sender As Object, e As EventArgs) Handles txtchargesAmount.GotFocus
        If txtOnValue.TabStop = False Then
            If dgCharges.ColumnCount = 0 Then ChargesRowColums()
            If dgCharges.RowCount = 0 Then RetriveCharges()
            If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
            txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
            txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
            dgCharges.Visible = False : FillCharges()
        End If
    End Sub
    Private Sub txtCharges_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyDown, txtOnValue.KeyDown, txtCalculatePer.KeyDown, txtPlusMinus.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus()
        End Select
        If txtCharges.Focused Then
            If e.KeyCode = Keys.F3 Then
                ChargesForm.MdiParent = MainScreenForm
                ChargesForm.Show()
                If Not ChargesForm Is Nothing Then
                    ChargesForm.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.F1 Then
                If dgCharges.SelectedRows.Count = 0 Then Exit Sub
                Dim tmpMarkaID As String = dgCharges.SelectedRows(0).Cells(0).Value
                ChargesForm.MdiParent = MainScreenForm
                ChargesForm.Show()
                ChargesForm.FillContros(tmpMarkaID)
                If Not ChargesForm Is Nothing Then
                    ChargesForm.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.Down Then dgCharges.Focus()
        End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If dg1.RowCount = 0 Then Exit Sub : If dg1.SelectedRows.Count = 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
            txtLot.Text = dg1.SelectedRows(0).Cells(1).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
            txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
            txtTotAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
            txtItem.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
                calc()
                'ClearDetails()
            End If
        End If
    End Sub
    Private Sub Dg2_KeyDown(sender As Object, e As KeyEventArgs) Handles Dg2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dg2.RowCount = 0 Then Exit Sub : If Dg2.SelectedRows.Count = 0 Then Exit Sub
            txtCharges.Text = Dg2.SelectedRows(0).Cells(0).Value
            txtOnValue.Text = Dg2.SelectedRows(0).Cells(1).Value
            txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(2).Value
            txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(3).Value
            txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(4).Value
            txtChargeID.Text = Dg2.SelectedRows(0).Cells(5).Value
            txtCharges.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are you Sure to delete Charge", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
                calc()
                'ClearDetails()
            End If
        End If
    End Sub

    Private Sub Dg2_MouseClick(sender As Object, e As MouseEventArgs) Handles Dg2.MouseClick
        Dg2.ClearSelection()
    End Sub

    Private Sub Dg2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Dg2.MouseDoubleClick
        If Dg2.RowCount = 0 Then Exit Sub : If Dg2.SelectedRows.Count = 0 Then Exit Sub
        txtCharges.Text = Dg2.SelectedRows(0).Cells(0).Value
        txtOnValue.Text = Dg2.SelectedRows(0).Cells(1).Value
        txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(2).Value
        txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(3).Value
        txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(4).Value
        txtChargeID.Text = Dg2.SelectedRows(0).Cells(5).Value
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then Exit Sub : If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
        txtLot.Text = dg1.SelectedRows(0).Cells(1).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
        txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
        txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
        Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
        txtTotAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
    End Sub
    Private Sub txtchargesAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtchargesAmount.KeyDown
        If dg1.RowCount = 0 Then Exit Sub : If dg1.SelectedRows.Count = 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            If Dg2.SelectedRows.Count = 1 Then
                Dg2.SelectedRows(0).Cells(0).Value = txtCharges.Text
                Dg2.SelectedRows(0).Cells(1).Value = txtOnValue.Text
                Dg2.SelectedRows(0).Cells(2).Value = txtCalculatePer.Text
                Dg2.SelectedRows(0).Cells(3).Value = txtPlusMinus.Text
                Dg2.SelectedRows(0).Cells(4).Value = txtchargesAmount.Text
                Dg2.SelectedRows(0).Cells(5).Value = txtChargeID.Text
                calc()
                txtCharges.Focus()
                Dg2.ClearSelection()
                cleartxtCharges()
            Else
                Dg2.Rows.Add(txtCharges.Text, txtOnValue.Text, txtCalculatePer.Text, txtPlusMinus.Text, txtchargesAmount.Text, txtChargeID.Text)
                calc()
                cleartxtCharges()
                txtCharges.Focus()
                Dg2.ClearSelection()
            End If
        End If
    End Sub
    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtItem.KeyDown, txtAccount.KeyDown,
      txtVoucherNo.KeyDown, txtLot.KeyDown, txtNug.KeyDown, txtKg.KeyDown, txtRate.KeyDown, Cbper.KeyDown, txtChallanNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus()
        End Select
        Select Case e.KeyCode
            Case Keys.PageDown
                e.Handled = True
                txtCharges.Focus()
        End Select
        If txtLot.Focused Then
            If e.KeyCode = Keys.Down Then dgLot.Focus()
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        End If
        If txtChallanNo.Focused Then
            If e.KeyCode = Keys.Down Then dgChallanNo.Focus()
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.F3 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.Down Then dgItemSearch.Focus()
            If e.KeyCode = Keys.F1 Then
                If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
                Dim tmpID As String = dgItemSearch.SelectedRows(0).Cells(0).Value
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                Item_form.FillContros(tmpID)
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
            End If
        End If
    End Sub

    Private Sub txtCustomer_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub txtCustomer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.BringToFront()
        AccountRowColumns()
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtCustomer_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub

    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 180
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 150
        DgAccountSearch.Visible = True : DgAccountSearch.BringToFront()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select AccountID,AccountName from Transaction2  where Transtype='On Sale' " & condtion & " Group by AccountID order by AccountName")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        '   .Cells(2).Value = dt.Rows(i)("City").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub ChallanColums()
        dgChallanNo.ColumnCount = 3
        dgChallanNo.Columns(0).Name = "ID" : dgChallanNo.Columns(0).Visible = False
        dgChallanNo.Columns(1).Name = "Account Name" : dgChallanNo.Columns(1).Width = 100
        dgChallanNo.Columns(2).Name = "City" : dgChallanNo.Columns(2).Width = 100
        dgChallanNo.Visible = True : dgChallanNo.BringToFront()
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        ' CustomerFill()
        ChallanColums()
        DgAccountSearch.Visible = False
        txtChallanNo.Focus()
    End Sub

    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            ' CustomerFill()
            ChallanColums()
            DgAccountSearch.Visible = False
            txtChallanNo.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()

    End Sub
    Private Sub txtaccountName_GotFocus(sender As Object, e As EventArgs) Handles txtChallanNo.GotFocus
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then txtAccount.Focus() : Exit Sub
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        txtChallanNo.SelectionStart = 0 : txtChallanNo.SelectionLength = Len(txtChallanNo.Text)
        If dgChallanNo.ColumnCount = 0 Then ChallanColums()
        If txtChallanNo.Text.Trim() <> "" Then
            RetriveChallanNo(" and upper(BillNo) Like upper('" & txtChallanNo.Text.Trim() & "%')")
        Else
            RetriveChallanNo()
        End If
    End Sub
    Private Sub dgCharges_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgCharges.CellClick
        txtCharges.Clear()
        txtChargeID.Clear()
        txtChargeID.Text = dgCharges.SelectedRows(0).Cells(0).Value
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        dgCharges.Visible = False
        txtOnValue.Focus()
        FillCharges()

    End Sub

    Private Sub dgCharges_KeyDown(sender As Object, e As KeyEventArgs) Handles dgCharges.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtCharges.Clear()
            txtChargeID.Clear()
            txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
            txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
            dgCharges.Visible = False
            txtOnValue.Focus()
            FillCharges()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtCharges.Focus()
        If e.KeyCode = Keys.F3 Then
            ChargesForm.MdiParent = MainScreenForm
            ChargesForm.Show()
            If Not ChargesForm Is Nothing Then
                ChargesForm.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Back Then txtCharges.Focus()
    End Sub
    Private Sub RetriveChallanNo(Optional ByVal condtion As String = "")
        dgChallanNo.Rows.Clear()
        Dim dt As New DataTable
        Dim sql As String = String.Empty
        If BtnSave.Text = "&Save" Then
            sql = "Select VoucherID,EntryDate,BillNo,Nug-(Select ifnull(sum(Nug),0) From Transaction1 Where PurchaseID=Transaction2.VoucherID) as RestNug From Transaction2 Where AccountID='" & Val(txtAccountID.Text) & "' and TransType in('On Sale') and RestNug > 0" & condtion & "   Group by VoucherID"
        Else
            sql = "Select VoucherID,EntryDate,BillNo,(Nug-(Select  ifnull(sum(Nug),0) From Transaction1 Where PurchaseID=Transaction2.VoucherID)+(Select sum(Nug) From Transaction1 Where PurchaseID=Transaction2.VoucherID and VoucherID=" & Val(txtid.Text) & ")) as RestNug From Transaction2 Where AccountID='" & Val(txtAccountID.Text) & "' and TransType in('On Sale') and RestNug > 0" & condtion & "   Group by VoucherID"
        End If
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                dgChallanNo.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgChallanNo.Rows.Add()
                    With dgChallanNo.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(2).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub dgChallanNo_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgChallanNo.CellClick
        txtChallanNo.Clear() : txtChallanID.Clear()
        txtChallanID.Text = Val(dgChallanNo.SelectedRows(0).Cells(0).Value)
        txtChallanNo.Text = dgChallanNo.SelectedRows(0).Cells(1).Value
        dgChallanNo.Visible = False
        ChallanColums() : txtItem.Focus()
    End Sub
    Private Sub dgChallanNo_KeyDown(sender As Object, e As KeyEventArgs) Handles dgChallanNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtChallanNo.Clear() : txtChallanID.Clear()
            txtChallanID.Text = Val(dgChallanNo.SelectedRows(0).Cells(0).Value)
            txtChallanNo.Text = dgChallanNo.SelectedRows(0).Cells(1).Value
            dgChallanNo.Visible = False
            ChallanColums() : txtItem.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtChallanNo.Focus()
    End Sub

    Private Sub txtChallanNo_KeyUp(sender As Object, e As KeyEventArgs) Handles txtChallanNo.KeyUp
        If txtChallanNo.Text.Trim() <> "" Then
            RetriveChallanNo(" and upper(BillNo) Like upper('" & txtChallanNo.Text.Trim() & "%')")
        Else
            RetriveChallanNo()
        End If
    End Sub
    Private Sub ItemBalance()
        lblItemBalance.Visible = True
        Dim PurchaseBal As String = "" : Dim StockBal As String = ""
        Dim RestBal As String = "" : Dim tmpbal As String = ""
        Dim dt As New DataTable
        PurchaseBal = clsFun.ExecScalarStr("Select  Sum(Nug) From Transaction2 Where AccountID='" & Val(txtAccountID.Text) & "' and VoucherID='" & Val(txtChallanID.Text) & "'  and ItemID='" & Val(txtItemID.Text) & "'  Group by ItemID Order by ItemName")
        StockBal = clsFun.ExecScalarStr("Select  Sum(Nug) From Transaction1 Where PurchaseID='" & Val(txtChallanID.Text) & "'  and ItemID='" & Val(txtItemID.Text) & "'  Group by ItemID Order by ItemName")
        RestBal = Val(PurchaseBal) - Val(StockBal)
        If BtnSave.Text = "&Save" Then
            If dg1.SelectedRows.Count = 0 Then
                If Val(StockBal) = 0 Then ' if no record inserted
                    If dg1.RowCount = 0 Then ' if no rows addred
                        bal = (RestBal)
                    Else 'if rows count
                        For i As Integer = 0 To dg1.RowCount - 1
                            If dg1.Rows(i).Cells(7).Value = txtItemID.Text Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(2).Value)
                            End If
                        Next i
                        tmpbal = (tmpbal)
                    End If
                    bal = Val(PurchaseBal) - Val(tmpbal)
                Else
                    If dg1.RowCount = 0 Then ' if any Record Inserted in Database but Row not Added
                        bal = (RestBal)
                    Else
                        For i As Integer = 0 To dg1.RowCount - 1 'if any Record Inserted in Database and Row Added
                            If dg1.Rows(i).Cells(7).Value = txtItemID.Text Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(2).Value)
                            End If
                        Next i
                        bal = Val(RestBal) - Val(tmpbal)
                    End If
                End If
            Else
                If Val(StockBal) = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(7).Value = txtItemID.Text Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    tmpbal = Val(PurchaseBal) - Val(tmpbal)
                    tmpbal = Val(tmpbal) + Val(dg1.SelectedRows(0).Cells(2).Value)
                    bal = Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(7).Value = txtItemID.Text Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    tmpbal = Val(RestBal) - Val(tmpbal)
                    tmpbal = (tmpbal) + Val(dg1.SelectedRows(0).Cells(2).Value)
                    bal = Val(tmpbal)
                End If
            End If
        Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
            If dg1.RowCount = 0 Then ' if no rows addred
                bal = (RestBal)
            Else 'if rows count
                UpdateTmp = clsFun.ExecScalarStr("Select  Sum(Nug) From Transaction1 Where PurchaseID='" & Val(txtChallanID.Text) & "'  and ItemID='" & Val(txtItemID.Text) & "' AND VoucherID not in ('" & Val(txtid.Text) & "') Group by ItemID Order by ItemName")
                If dg1.SelectedRows.Count = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(7).Value = txtItemID.Text Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    tmpbal = Val(UpdateTmp) + Val(tmpbal)
                    bal = Val(PurchaseBal) - Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(7).Value = txtItemID.Text Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    ' If (StockBal) = 0 Then
                    tmpbal = Val(UpdateTmp) + Val(tmpbal) '- Val(dg1.SelectedRows(0).Cells(5).Value)
                    tmpbal = Val(tmpbal) - Val(dg1.SelectedRows(0).Cells(2).Value)
                    bal = Val(PurchaseBal) - Val(tmpbal)
                End If
            End If
        End If
        If dg1.SelectedRows.Count = 0 Then
            lblItemBalance.Text = "Item Balance : " & Val(bal)
        Else
            lblItemBalance.Text = "Item Balance : " & Val(bal) & " (Selected Nugs Not Counting)"
        End If
    End Sub

    'Private Sub LotBalance()
    '    lblLot.Visible = True
    '    Dim PurchaseLot As String = ""
    '    Dim StockLot As String = ""
    '    Dim RestLot As String = ""
    '    Dim tmpLot As String = ""
    '    Dim UpdatetmpLot As String = ""
    '    Dim tmpbal As String = ""
    '    PurchaseLot = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and LotNo='" & txtLot.Text & "' and VoucherID= '" & (txtPurchaseID.Text) & "' ")
    '    '        PurchaseLot = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and LotNo='" & txtLot.Text & "' and VoucherID='" & txtPurchaseID.Text & "'")
    '    StockLot = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and Lot='" & txtLot.Text & "' and PurchaseID= '" & (txtPurchaseID.Text) & "'  and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out')")
    '    RestLot = Val(PurchaseLot) - Val(StockLot)
    '    If BtnSave.Text = "&Save" Then
    '        If dg1.SelectedRows.Count = 0 Then
    '            If Val(StockLot) = 0 Then ' if no record inserted
    '                If dg1.RowCount = 0 Then ' if no rows addred
    '                    LotBal = (StockLot)
    '                Else 'if rows count
    '                    For i As Integer = 0 To dg1.RowCount - 1
    '                        If dg1.Rows(i).Cells(4).Value = txtLot.Text And Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(30).Value) = Val(txtPurchaseID.Text) Then
    '                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(5).Value)
    '                        End If
    '                    Next i
    '                    tmpLot = (tmpLot)
    '                End If
    '                LotBal = Val(PurchaseLot) - Val(tmpLot)
    '            Else
    '                If dg1.RowCount = 0 Then ' if any Record Inserted in Database but Row not Added
    '                    LotBal = (RestLot)
    '                Else
    '                    For i As Integer = 0 To dg1.RowCount - 1 'if any Record Inserted in Database and Row Added
    '                        If dg1.Rows(i).Cells(4).Value = txtLot.Text And Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(30).Value) = Val(txtPurchaseID.Text) Then
    '                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(5).Value)
    '                        End If
    '                    Next i
    '                    LotBal = Val(RestLot) - Val(tmpLot)
    '                End If
    '            End If
    '        Else
    '            If Val(StockLot) = 0 Then
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If dg1.Rows(i).Cells(4).Value = txtLot.Text And Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(30).Value) = Val(txtPurchaseID.Text) Then
    '                        tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(5).Value)
    '                    End If
    '                Next i
    '                tmpLot = Val(PurchaseLot) - Val(tmpLot)
    '                tmpLot = Val(tmpLot) + Val(dg1.SelectedRows(0).Cells(5).Value)
    '                LotBal = Val(tmpLot)
    '            Else
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If dg1.Rows(i).Cells(4).Value = txtLot.Text And Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(30).Value) = Val(txtPurchaseID.Text) Then
    '                        tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(5).Value)
    '                    End If
    '                Next i
    '                tmpLot = Val(RestLot) - Val(tmpLot)
    '                tmpLot = (tmpLot) + Val(dg1.SelectedRows(0).Cells(5).Value)
    '                LotBal = Val(tmpLot)
    '            End If
    '        End If
    '    Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
    '        If dg1.RowCount = 0 Then ' if no rows addred
    '            LotBal = (RestLot)
    '        Else 'if rows count
    '            UpdatetmpLot = clsFun.ExecScalarStr("Select  Sum(Nug) From Transaction1 Where ItemID='" & Val(txtItemID.Text) & "' and Lot='" & txtLot.Text & "' and PurchaseID='" & Val(txtChallanID.Text) & "' AND VoucherID not in ('" & Val(txtid.Text) & "') Group by ItemID Order by ItemName")
    '            '        
    '            UpdatetmpLot = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " " &
    '                                                "AND VoucherID  in ('" & Val(txtid.Text) & "') and Lot='" & txtLot.Text & "' and PurchaseID='" & Val(txtPurchaseID.Text) & "' and StorageID=" & Val(txtStorageID.Text) & " " &
    '                                                " and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out')")
    '            Dim UpdatedLot As String = Val(clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where  ItemID = " & Val(txtItemID.Text) & " and Lot='" & txtLot.Text & "' " &
    '                                                                "and VoucherID<>'" & Val(txtid.Text) & "' and PurchaseID='" & Val(txtPurchaseID.Text) & "' "))
    '            If dg1.SelectedRows.Count = 0 Then
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If dg1.Rows(i).Cells(4).Value = txtLot.Text And Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(30).Value) = Val(txtPurchaseID.Text) Then
    '                        tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(5).Value)
    '                    End If
    '                Next i
    '                LotBal = Val(PurchaseLot) - Val(Val(tmpLot) + Val(UpdatedLot))
    '            Else
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If dg1.Rows(i).Cells(4).Value = txtLot.Text And dg1.Rows(i).Cells(0).Value = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(30).Value) = Val(txtPurchaseID.Text) Then
    '                        tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(5).Value)
    '                        'Else
    '                        '   MsgBox("Please Choose Selected Lot Only", MsgBoxStyle.Critical, "Check Lot") : txtLot.Focus() : Exit Sub
    '                    End If
    '                Next i
    '                ' If (StockBal) = 0 Then
    '                'tmpLot = Val(UpdatetmpLot) + Val(Val(tmpLot) - Val(dg1.SelectedRows(0).Cells(5).Value))
    '                If dg1.SelectedRows(0).Cells(4).Value = txtLot.Text And dg1.SelectedRows(0).Cells(0).Value = txtItemID.Text And Val(dg1.SelectedRows(0).Cells(30).Value) = Val(txtPurchaseID.Text) Then
    '                    tmpLot = Val(Val(tmpLot) - Val(dg1.SelectedRows(0).Cells(5).Value))
    '                    LotBal = Val(PurchaseLot) - Val(Val(tmpLot) + Val(UpdatedLot))
    '                Else
    '                    LotBal = RestLot
    '                End If


    '                ' tmpLot = Val(tmpLot) - dg1.SelectedRows(0).Cells(5).Value
    '                'LotBal = Val(PurchaseLot) - Val(tmpLot)
    '            End If
    '        End If
    '    End If
    '    If dg1.SelectedRows.Count = 0 Then
    '        lblLot.Text = "Lot Balance : " & Val(LotBal)
    '    Else
    '        lblLot.Text = "Lot Balance : " & Val(LotBal) & " (Selected Nugs Not Counting)"
    '    End If
    'End Sub


    Private Sub LotBalance()
        lblLot.Visible = True
        Dim PurchaseLot As String = ""
        Dim StockLot As String = ""
        Dim RestLot As String = ""
        Dim tmpLot As String = ""
        Dim UpdatetmpLot As String = ""
        Dim tmpbal As String = ""
        PurchaseLot = clsFun.ExecScalarStr("Select  Sum(Nug) From Transaction2 Where AccountID='" & Val(txtAccountID.Text) & "' and VoucherID='" & Val(txtChallanID.Text) & "' and ItemID='" & txtItemID.Text & "'  and Lot='" & txtLot.Text & "' Group by ItemID Order by ItemName")
        StockLot = clsFun.ExecScalarStr("Select  Sum(Nug) From Transaction1 Where PurchaseID='" & Val(txtChallanID.Text) & "'and ItemID='" & txtItemID.Text & "'  and Lot='" & txtLot.Text & "' Group by ItemID Order by ItemName")
        RestLot = Val(PurchaseLot) - Val(StockLot)
        If BtnSave.Text = "&Save" Then
            If dg1.SelectedRows.Count = 0 Then
                If Val(StockLot) = 0 Then ' if no record inserted
                    If dg1.RowCount = 0 Then ' if no rows addred
                        LotBal = (StockLot)
                    Else 'if rows count
                        For i As Integer = 0 To dg1.RowCount - 1
                            If dg1.Rows(i).Cells(1).Value = txtLot.Text AndAlso dg1.Rows(i).Cells(8).Value = Val(txtItemID.Text) Then
                                tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(2).Value)
                            End If
                        Next i
                        tmpLot = (tmpLot)
                    End If
                    LotBal = Val(PurchaseLot) - Val(tmpLot)
                Else
                    If dg1.RowCount = 0 Then ' if any Record Inserted in Database but Row not Added
                        LotBal = (RestLot)
                    Else
                        For i As Integer = 0 To dg1.RowCount - 1 'if any Record Inserted in Database and Row Added
                            If dg1.Rows(i).Cells(1).Value = txtLot.Text AndAlso dg1.Rows(i).Cells(8).Value = Val(txtItemID.Text) Then
                                tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(2).Value)
                            End If
                        Next i
                        LotBal = Val(RestLot) - Val(tmpLot)
                    End If
                End If
            Else
                If Val(StockLot) = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(1).Value = txtLot.Text AndAlso dg1.Rows(i).Cells(8).Value = Val(txtItemID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    tmpLot = Val(PurchaseLot) - Val(tmpLot)
                    tmpLot = Val(tmpLot) + Val(dg1.SelectedRows(0).Cells(2).Value)
                    LotBal = Val(tmpLot)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(1).Value = txtLot.Text AndAlso dg1.Rows(i).Cells(8).Value = Val(txtItemID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    tmpLot = Val(RestLot) - Val(tmpLot)
                    tmpLot = (tmpLot) + Val(dg1.SelectedRows(0).Cells(5).Value)
                    LotBal = Val(tmpLot)
                End If
            End If
        Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
            If dg1.RowCount = 0 Then ' if no rows addred
                LotBal = (RestLot)
            Else 'if rows count
                UpdatetmpLot = clsFun.ExecScalarStr("Select  Sum(Nug) From Transaction1 Where ItemID='" & Val(txtItemID.Text) & "' and Lot='" & txtLot.Text & "' and PurchaseID='" & Val(txtChallanID.Text) & "' AND VoucherID not in ('" & Val(txtid.Text) & "') Group by ItemID Order by ItemName")
                If dg1.SelectedRows.Count = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(1).Value = txtLot.Text AndAlso dg1.Rows(i).Cells(8).Value = Val(txtItemID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    tmpbal = Val(UpdatetmpLot) + Val(tmpLot)
                    LotBal = Val(PurchaseLot) - Val(tmpLot)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(1).Value = txtLot.Text AndAlso dg1.Rows(i).Cells(8).Value = Val(txtItemID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(2).Value)
                        End If
                    Next i
                    ' If (StockBal) = 0 Then
                    tmpLot = Val(UpdatetmpLot) + Val(tmpLot) '- Val(dg1.SelectedRows(0).Cells(5).Value)
                    tmpLot = Val(tmpLot) - Val(dg1.SelectedRows(0).Cells(2).Value)
                    LotBal = Val(PurchaseLot) - Val(tmpLot)
                End If
            End If
        End If
        If dg1.SelectedRows.Count = 0 Then
            lblLot.Text = "Lot Balance : " & Val(LotBal)
        Else
            lblLot.Text = "Lot Balance : " & Val(LotBal) & " (Selected Nugs Not Counting)"
        End If
    End Sub
    Private Sub StockInItemRowColums()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 200
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 200
        StockINretriveItems()
    End Sub
    Private Sub StockINretriveItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dgItemSearch.Rows.Clear()
        Dim sql As String = String.Empty
        If BtnSave.Text = "&Save" Then
            sql = "Select  ItemID,ItemName From Transaction2 Where AccountID='" & Val(txtAccountID.Text) & "' and VoucherID='" & Val(txtChallanID.Text) & "' " & condtion & " Group by ItemID Order by AccountName"
        Else
            sql = "Select  ItemID,ItemName,(ifnull(Nug,0)-(Select ifnull(sum(Nug),0) From Transaction1 Where PurchaseID=Transaction2.VoucherID)+(Select ifnull(sum(Nug),0) From Transaction1 Where PurchaseID=Transaction2.VoucherID and VoucherID=" & Val(txtid.Text) & ")) as RestNug From Transaction2 Where RestNug<> 0 and AccountID='" & Val(txtAccountID.Text) & "' and VoucherID='" & Val(txtChallanID.Text) & "' " & condtion & " Group by ItemID Order by AccountName"
        End If

        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dgItemSearch.Rows.Add()
                    With dgItemSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("ItemID").ToString()
                        .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                    End With
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub LotCoulmns()
        dgLot.ColumnCount = 8
        dgLot.Columns(0).Name = "LotID" : dgLot.Columns(0).Visible = False
        dgLot.Columns(1).Name = "Lot" : dgLot.Columns(1).Width = 100
        dgLot.Columns(2).Name = "Vehicle No." : dgLot.Columns(2).Width = 115
        dgLot.Columns(3).Name = "Date" : dgLot.Columns(3).Width = 100
        dgLot.Columns(4).Name = "Account Name" : dgLot.Columns(4).Width = 170
        dgLot.Columns(5).Name = "Nug" : dgLot.Columns(5).Width = 50
        dgLot.Columns(6).Name = "Weight" : dgLot.Columns(6).Width = 50
        dgLot.Columns(7).Name = "PurchaseID" : dgLot.Columns(7).Visible = False
        dgLot.Visible = True
    End Sub
    Private Sub RetriveLot(Optional ByVal condtion As String = "")
        dgLot.Rows.Clear()
        Dim sql As String = String.Empty
        sql = "Select VoucherID,Lot,BillNo,AccountName,EntryDate,PurchaseID,Sum(Nug) as nug,Sum(Weight) as Weight  From Transaction2 Where AccountID='" & Val(txtAccountID.Text) & "' and VoucherID='" & Val(txtChallanID.Text) & "' and ItemID='" & Val(txtItemID.Text) & "' and TransType='On Sale' Group by VoucherID,Lot,ItemID Order by AccountName"
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    ' Application.DoEvents()
                    dgLot.Rows.Add()
                    With dgLot.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = dt.Rows(i)("Lot").ToString()
                        .Cells(2).Value = clsFun.ExecScalarStr("Select VehicleNo From Vouchers Where ID='" & Val(.Cells(0).Value) & "'")
                        .Cells(3).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                        .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Val(dt.Rows(i)("Nug").ToString())
                        .Cells(6).Value = Val(dt.Rows(i)("Weight").ToString())
                        .Cells(7).Value = Val(dt.Rows(i)("PurchaseID").ToString())
                    End With
                Next
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub txtItemStockIn_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        ' StockInItemRowColums()
        If txtItem.Text.Trim() <> "" Then

            StockINretriveItems(" And upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            txtItemID.Clear()
            StockINretriveItems()
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
    End Sub

    Private Sub txtItemStockIn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
        dgItemSearch.Visible = True
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        dgItemSearch.Visible = False
        txtLot.Focus()
    End Sub
    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
            txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            dgItemSearch.Visible = False : e.SuppressKeyPress = True
            txtLot.Focus()
        End If
        If e.KeyCode = Keys.Back Then txtItem.Focus()
    End Sub
    Private Sub txtLot_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLot.KeyPress
        dgLot.BringToFront() : dgLot.Visible = True
    End Sub

    Private Sub dgLot_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgLot.CellClick
        txtLot.Clear()
        txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
        txtNug.Text = Format(Val(dgLot.SelectedRows(0).Cells(5).Value), "0.00")
        txtKg.Text = Format(Val(dgLot.SelectedRows(0).Cells(6).Value), "0.00")
        txtOnSaleID.Text = Val(dgLot.SelectedRows(0).Cells(7).Value)
        dgLot.Visible = False
        txtNug.Focus()
    End Sub

    Private Sub dgLot_KeyDown(sender As Object, e As KeyEventArgs) Handles dgLot.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtLot.Clear()
            txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
            txtNug.Text = Format(Val(dgLot.SelectedRows(0).Cells(5).Value), "0.00")
            txtKg.Text = Format(Val(dgLot.SelectedRows(0).Cells(6).Value), "0.00")
            dgLot.Visible = False
            txtNug.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtLot.Focus()
    End Sub

    Private Sub txtChallanNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtChallanNo.KeyPress
        dgChallanNo.Visible = True : dgChallanNo.BringToFront()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If dg1.Rows.Count = 0 Then MsgBox("There is No Items to Save/Update Record... Add Items First", MsgBoxStyle.Critical, "No Item") : Exit Sub
        ButtonControl()
        If BtnSave.Text = "&Save" Then
            Save()
        Else
            UpdateRecord()
        End If
        ButtonControl()
        Dim res = MessageBox.Show("Do you want to Print On Sale Bill...", "Print Invoice...", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If res = Windows.Forms.DialogResult.Yes Then
            btnPrint.Enabled = True
            btnPrint.PerformClick()
            Exit Sub
        Else
            FootertextClear()
        End If

    End Sub
    Private Sub RemarkNaration()
        remark = String.Empty
        remarkHindi = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                remark = remark & .Cells(0).Value & " Lot No. : " & .Cells(1).Value & ", Nug. : " & Format(Val(.Cells(2).Value), "0.00") & ",Weight : " & Format(Val(.Cells(3).Value), "0.00") & ",Rate : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & "=" & Format(Val(.Cells(6).Value), "0.00") & "" & vbCrLf
                Dim othername As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID='" & Val(.Cells(7).Value) & "' ")
                remarkHindi = remarkHindi & othername & " Lot No. : " & .Cells(1).Value & ", नग : " & Format(Val(.Cells(2).Value), "0.00") & ",वजन : " & Format(Val(.Cells(3).Value), "0.00") & ",भाव : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & "=" & Format(Val(.Cells(6).Value), "0.00") & "" & vbCrLf
            End With
        Next
    End Sub
    Sub LedgerInsert()
        Dim FastQuery As String = String.Empty
        Dim dt As DateTime
        Dim tmpamount As Decimal = Val(txtTotbasic.Text)
        Dim tmpamount2 As Decimal = Val(txtTotbasic.Text)
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        ''Caluclate  net amt
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then ''our coast
                        If .Cells(3).Value = "+" Then
                            tmpamount2 = Math.Round(Val(tmpamount2) + Val(.Cells(4).Value))
                        Else
                            tmpamount2 = Math.Round(Val(tmpamount2) - Val(.Cells(4).Value))
                        End If
                    End If
                End If
            End With
        Next
        remark = String.Empty
        remarkHindi = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                remark = remark & .Cells(0).Value & " Lot No. : " & .Cells(1).Value & ", Nug. : " & Format(Val(.Cells(2).Value), "0.00") & ",Weight : " & Format(Val(.Cells(3).Value), "0.00") & ",Rate : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & "=" & Format(Val(.Cells(6).Value), "0.00") & "" & vbCrLf
                Dim othername As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID='" & Val(.Cells(7).Value) & "' ")
                remarkHindi = remarkHindi & othername & " Lot No. : " & .Cells(1).Value & ", नग : " & Format(Val(.Cells(2).Value), "0.00") & ",वजन : " & Format(Val(.Cells(3).Value), "0.00") & ",भाव : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & "=" & Format(Val(.Cells(6).Value), "0.00") & "" & vbCrLf
            End With
        Next
        '  RemarkNaration()
        If Val(txtTotbasic.Text) > 0 Then ''Manual Beejak Account Fixed
            '    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 24, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=24"), Val(tmpamount2), "C", remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(24) & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=24") & "','" & Val(tmpamount2) & "', 'C','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
        End If
        If Val(txtTotTotal.Text) > 0 Then ''Account 
            'clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtRoundOff.Text)), "D", remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ", '" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtRoundOff.Text)) & "', 'D','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
        End If
        If Val(txtRoundOff.Text) <> 0 Then ''Account 
            If Val(txtRoundOff.Text) < 0 Then
                '        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtRoundOff.Text)), "C", remark, txtAccount.Text, remarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Val(txtRoundOff.Text) & "','C','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Val(txtRoundOff.Text) & "','D','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)

    End Sub
    Sub ServerLedgerInsert()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim dt As DateTime
        Dim tmpamount As Decimal = Val(txtTotbasic.Text)
        Dim tmpamount2 As Decimal = Val(txtTotbasic.Text)
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        ''Caluclate  net amt
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then ''our coast
                        If .Cells(3).Value = "+" Then
                            tmpamount2 = Math.Round(Val(tmpamount2) + Val(.Cells(4).Value))
                        Else
                            tmpamount2 = Math.Round(Val(tmpamount2) - Val(.Cells(4).Value))
                        End If
                    End If
                End If
            End With
        Next
        'RemarkNaration()

        Dim remark As String = String.Empty
        Dim remarkHindi As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                remark = remark & .Cells(0).Value & " Lot No. : " & .Cells(1).Value & ", Nug. : " & Format(Val(.Cells(2).Value), "0.00") & ",Weight : " & Format(Val(.Cells(3).Value), "0.00") & ",Rate : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & "=" & Format(Val(.Cells(6).Value), "0.00") & "" & vbCrLf
                Dim othername As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID='" & Val(.Cells(7).Value) & "' ")
                remarkHindi = remarkHindi & othername & " Lot No. : " & .Cells(1).Value & ", नग : " & Format(Val(.Cells(2).Value), "0.00") & ",वजन : " & Format(Val(.Cells(3).Value), "0.00") & ",भाव : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & "=" & Format(Val(.Cells(6).Value), "0.00") & "" & vbCrLf
            End With
        Next
        If Val(txtTotbasic.Text) > 0 Then ''Manual Beejak Account Fixed
            '    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 24, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=24"), Val(tmpamount2), "C", remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(24) & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=24") & "','" & Val(tmpamount2) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
        End If
        If Val(txtTotTotal.Text) > 0 Then ''Account 
            'clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtRoundOff.Text)), "D", remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ", '" & txtAccountID.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtRoundOff.Text)) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
        End If
        If Val(txtRoundOff.Text) <> 0 Then ''Account 
            If Val(txtRoundOff.Text) < 0 Then
                '        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtRoundOff.Text)), "C", remark, txtAccount.Text, remarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Val(txtRoundOff.Text) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Val(txtRoundOff.Text) & "','D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)

    End Sub
    Sub ChargeInsert()
        Dim Remark As String = String.Empty
        Dim RemarkHindi As String = String.Empty
        Dim FastQuery As String = String.Empty
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim CostON As String = clsFun.ExecScalarStr(" Select CostOn FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & Val(.Cells(5).Value) & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            '    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            '                            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)

    End Sub
    Sub ServerChargeInsert()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim CostON As String = clsFun.ExecScalarStr(" Select CostOn FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim RemarkHindi As String = String.Empty
                    Dim Remark As String = String.Empty
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & Val(.Cells(5).Value) & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            '    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            '                            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub

    Private Sub UpdateRecord()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        dg1.ClearSelection()
        Dim sql As String = String.Empty
        ' Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "', Entrydate='" & SqliteEntryDate & "', " &
              "AccountID='" & Val(txtAccountID.Text) & "',AccountName='" & txtAccount.Text & "', Nug='" & Val(txtTotNug.Text) & "', kg='" & Val(txtTotweight.Text) & "' ," &
              "BasicAmount='" & Val(txtTotbasic.Text) & "',RoundOff='" & Val(txtRoundOff.Text) & "',TotalCharges='" & Val(txtTotCharges.Text) & "',TotalAmount='" & Val(txtTotTotal.Text) & "', " &
              "InvoiceID='" & Val(txtInvoiceID.Text) & "' Where ID='" & Val(txtid.Text) & "'"
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
            End If
            clsFun.ExecNonQuery("DELETE from Transaction1 WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from Chargestrans WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                "DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "")
            ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                               " DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "")
            dg1Record() : dg2Record() : LedgerInsert() : ChargeInsert()
            ServerTag = 1 : ServerLedgerInsert() : ServerLedgerInsert()
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
            FootertextClear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try

    End Sub
    Private Sub Save()
        'VNumber()
        Dim sql As String = String.Empty
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        dg1.ClearSelection()
        Dim cmd As SQLite.SQLiteCommand
        sql = "insert into Vouchers(TransType,BillNo, Entrydate, " _
                                    & "AccountID,AccountName, Nug, kg,BasicAmount,InvoiceID,ItemID,ItemName,TotalCharges,RoundOff,TotalAmount)" _
                                    & "values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10,@11,@12,@13,@14)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@3", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@4", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@5", txtAccount.Text)
            cmd.Parameters.AddWithValue("@6", Val(txtTotNug.Text))
            cmd.Parameters.AddWithValue("@7", Val(txtTotweight.Text))
            cmd.Parameters.AddWithValue("@8", Val(txtTotbasic.Text))
            cmd.Parameters.AddWithValue("@9", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@10", Val(txtChallanID.Text))
            cmd.Parameters.AddWithValue("@11", txtChallanNo.Text)
            cmd.Parameters.AddWithValue("@12", Val(txtTotCharges.Text))
            cmd.Parameters.AddWithValue("@13", Val(txtRoundOff.Text))
            cmd.Parameters.AddWithValue("@14", Val(txtTotTotal.Text))
            If cmd.ExecuteNonQuery() > 0 Then
                clsFun.CloseConnection()
            End If
            txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
            dg1Record() : dg2Record() : LedgerInsert() : ChargeInsert()
            ServerTag = 1 : ServerLedgerInsert() : ServerLedgerInsert()
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
            FootertextClear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Sub dg1Record1()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtid.Text) & ",'" & .Cells(0).Value & "','" & .Cells(1).Value & "','" & .Cells(2).Value & "'," &
                            "'" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "', " &
                            "'" & .Cells(7).Value & "'," & Val(txtChallanID.Text) & ""
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        Sql = "insert into Transaction1(VoucherID, ItemName,Lot, Nug, Weight, Rate,  Per,Amount,ItemID,PurchaseID) " & FastQuery & ""
        clsFun.ExecNonQuery(Sql)
    End Sub

    Sub dg1Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtid.Text) & ",'" & .Cells(0).Value & "','" & .Cells(1).Value & "','" & .Cells(2).Value & "'," &
                            "'" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "', " &
                            "'" & .Cells(7).Value & "'," & Val(txtChallanID.Text) & ", '" & Val(.Cells(9).Value) & "'"
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        Sql = "insert into Transaction1(VoucherID, ItemName,Lot, Nug, Weight, Rate,  Per,Amount,ItemID,PurchaseID,OnSaleID) " & FastQuery & ""
        clsFun.ExecNonQuery(Sql)
    End Sub

    Private Sub dg2Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtid.Text) & "," &
                        "'" & Val(.Cells("ChargeID").Value) & "','" & .Cells("Charge Name").Value & "','" & .Cells("On Value").Value & "'," &
                        "'" & .Cells("Cal").Value & "','" & .Cells("+/-").Value & "','" & .Cells("Amount").Value & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            Sql = "insert into ChargesTrans(VoucherID, ChargesID, ChargeName, OnValue, Calculate, ChargeType, Amount) " & FastQuery & ""
            clsFun.ExecNonQuery(Sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Public Sub FillControl(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnDelete.Enabled = True
        btnPrint.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & id
        Dim sql As String = "Select * from Transaction1 where VoucherID=" & id
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds, "b")
        ad2.Fill(ds, "C")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = id
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("AccountID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtChallanID.Text = ds.Tables("a").Rows(0)("ItemID").ToString()
            txtChallanNo.Text = ds.Tables("a").Rows(0)("ItemName").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txtTotweight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtTotbasic.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txtTotCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txtTotTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtRoundOff.Text = Format(Val(ds.Tables("a").Rows(0)("RoundOff").ToString()), "0.00")
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
        End If

        If ds.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells("Item Name").Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells("Lot No").Value = ds.Tables("b").Rows(i)("Lot").ToString()
                .Rows(i).Cells("Nug").Value = Format(Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells("Weight").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells("Rate").Value = Format(Val(ds.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells("per").Value = ds.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells("ItemID").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells("OnSaleID").Value = ds.Tables("b").Rows(i)("OnSaleID").ToString()
            Next
        End With
        'txtItemID.Text = IID

        If ds.Tables("c").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    If Val(ds.Tables("c").Rows(i)("OnValue").ToString()) > 0 Then
                        .Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    End If
                    .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
            'txtItemID.Text = IID
        End If
        dg1.ClearSelection()
        Dg2.ClearSelection()
        calc()
    End Sub

    Private Sub txtCharges_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyUp
        ChargesRowColums()
        If txtCharges.Text.Trim() <> "" Then
            'dgCharges.Visible = True
            RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then
            If dgCharges.Visible = True Then dgCharges.Visible = False
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Val(txtid.Text) > 0 Then
            retrivePrint()
        End If
        If txtid.Text = "" Then
            MsgBox("If you want to Print. Save First Record.", vbOKOnly, "Save First")
            Dim res = MessageBox.Show("Do you want to Save Bill", "Save First?", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = Windows.Forms.DialogResult.Yes Then
                BtnSave.PerformClick()
            End If
        Else
            PrintRecord()
            Report_Viewer.printReport("\OnSale.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
        txtid.Text = ""
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 29
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "EntryDate" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "SallerName" : .Columns(3).Width = 59
            .Columns(4).Name = "AccountName" : .Columns(4).Width = 59
            .Columns(5).Name = "BillingType" : .Columns(5).Width = 59
            .Columns(6).Name = "VehicleNo" : .Columns(6).Width = 159
            .Columns(7).Name = "itemName" : .Columns(7).Width = 69
            .Columns(8).Name = "Cut" : .Columns(8).Width = 76
            .Columns(9).Name = "Nug" : .Columns(9).Width = 90
            .Columns(10).Name = "Weight" : .Columns(10).Width = 86
            .Columns(11).Name = "SRate" : .Columns(11).Width = 90
            .Columns(12).Name = "per" : .Columns(12).Width = 50
            .Columns(13).Name = "SallerAmount" : .Columns(13).Width = 95
            .Columns(14).Name = "ChargeName" : .Columns(14).Width = 159
            .Columns(15).Name = "onValue" : .Columns(15).Width = 159
            .Columns(16).Name = "@" : .Columns(16).Width = 59
            .Columns(17).Name = "=/-" : .Columns(17).Width = 59
            .Columns(18).Name = "ChargeAmount" : .Columns(18).Width = 69
            .Columns(19).Name = "TotalNug" : .Columns(19).Width = 76
            .Columns(20).Name = "TotalWeight" : .Columns(20).Width = 90
            .Columns(21).Name = "TotalBasicAmount" : .Columns(21).Width = 86
            .Columns(22).Name = "RoundOff" : .Columns(22).Width = 90
            .Columns(23).Name = "TotalAmount" : .Columns(23).Width = 90
            .Columns(24).Name = "OtherItemName" : .Columns(24).Width = 95
            .Columns(25).Name = "OtherAccountName" : .Columns(25).Width = 159
            .Columns(26).Name = "OtherSallerName" : .Columns(26).Width = 159
            .Columns(27).Name = "RoudOff" : .Columns(27).Width = 159
            .Columns(28).Name = "AmountInWords" : .Columns(28).Width = 159
        End With
    End Sub
    Sub retrivePrint()
        'MsgBox(txtid.Text)
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        Dim sql As String = String.Empty
        sql = "SELECT * FROM Vouchers AS V INNER JOIN Transaction1 AS T ON T.VoucherID = V.ID Where V.ID=" & Val(txtid.Text) & ""
        dt = clsFun.ExecDataTable(sql)
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                TempRowColumn()
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                    ' .Cells(4).Value = dt.Rows(i)("SallerName1").ToString()
                    '.Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName1").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Lot").ToString()
                    .Cells(8).Value = Format(Val(.Cells(8).Value & dt.Rows(i)("Nug1").ToString()), "0.00")
                    .Cells(9).Value = Format(Val(.Cells(9).Value & dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = Format(Val(.Cells(10).Value & dt.Rows(i)("Rate1").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per1").ToString()
                    .Cells(12).Value = Format(Val(.Cells(12).Value & dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(18).Value = Format(Val(.Cells(18).Value & dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = Format(Val(.Cells(19).Value & dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = Format(Val(.Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = Format(Val(.Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = Format(Val(.Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(i)("ItemID1").ToString()) & "")
                    .Cells(24).Value = .Cells(24).Value & clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("AccountID").ToString()) & "")
                    '  .Cells(25).Value = .Cells(25).Value & clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("SallerID1").ToString()) & "")
                    .Cells(26).Value = Format(Val(.Cells(26).Value & dt.Rows(i)("RoundOff").ToString()), "0.00")
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & Val(dt.Rows(i)("ID").ToString()) & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("OnValue").ToString()), "0.00") & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & Format(Val(dt1.Rows(j)("Calculate").ToString()), "0.00") & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                        Next
                    Else
                        .Cells(13).Value = ""
                        .Cells(14).Value = ""
                        .Cells(15).Value = ""
                        .Cells(16).Value = ""
                        .Cells(17).Value = ""
                    End If

                End With
                '  End If
            Next
        End If
        dt.Clear()
        dt1.Clear()
    End Sub
    Private Sub PrintRecord()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(3).Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                                "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                   "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                                   "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                   "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                                   "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "', " &
                                   "'" & .Cells(25).Value & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & lblInword.Text & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            Sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                    " P21,P22,P23,P24,P25)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(Sql) : FootertextClear()
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub delete()
        ButtonControl()
        'If clsFun.ExecScalarInt("Select count(*) from Transaction1 where PurchaseID=" & Val(txtid.Text) & "") <> 0 Then
        '    MsgBox("Account Already Used in Transactions", vbOkOnly, "Access Denied")
        '    Exit Sub
        'End If
        If MessageBox.Show(" are you Sure want to Delete On Sale Entry... ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "; " & _
                                   "DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "; " & _
                                   "DELETE from Transaction1 WHERE VoucherID=" & Val(txtid.Text) & "; " & _
                                   "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & "; " & _
                                   "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " & _
                           " DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "")
                ServerTag = 0 : ServerLedgerInsert() : ServerChargeInsert()
                MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                FootertextClear()
            End If
        End If
        ButtonControl()
    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        delete()
    End Sub

    Private Sub txtTotAmount_TextChanged(sender As Object, e As EventArgs) Handles txtTotAmount.TextChanged
        If Cbper.SelectedIndex = 0 Then
            txtRate.Text = Format(Val(txtTotAmount.Text) / Val(txtNug.Text), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtRate.Text = Format(Val(txtTotAmount.Text) / Val(txtKg.Text), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 40 * Val(txtKg.Text), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        End If
        If txtRate.Text = "NAN" Then txtRate.Text = ""
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub tmpgrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles tmpgrid.CellContentClick

    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub DgAccountSearch_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellContentClick

    End Sub

    Private Sub txtLot_TextChanged(sender As Object, e As EventArgs) Handles txtLot.TextChanged

    End Sub

    Private Sub dgLot_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgLot.CellContentClick

    End Sub
End Class

