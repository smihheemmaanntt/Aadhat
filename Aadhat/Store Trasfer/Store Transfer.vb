
Public Class Store_Transfer
    Dim LotBal As String = String.Empty
    Private Sub Store_Transfer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Store_Transfer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Top = 0 : Me.Left = 0
        If BtnSave.Text = "&Save" Then
            clsFun.FillDropDownList(cbAccountName, "Select StockHolderID ,StockHolderName ,(ifnull(sum(Nug),0)-(Select ifnull(sum(Nug),0)  from Transaction2 " &
               " where Transtype in('Stock Sale','Standard Sale', 'On Sale','Store Out') and sallerID=Purchase.StockHolderID )) as RestNug  FROM Purchase " &
               "  group by StockHolderID Having RestNug<>0;", "StockHolderName", "StockHolderID", "")
        Else
            clsFun.FillDropDownList(cbAccountName, "Select StockHolderID ,StockHolderName ,(ifnull(sum(Nug),0)-(Select ifnull(sum(Nug),0)  from Transaction2 " &
                       " where Transtype in('Stock Sale','Standard Sale', 'On Sale','Store Out') and sallerID=Purchase.StockHolderID)) " &
                       " + (Select ifnull(sum(Nug),0)  from Transaction2 where Transtype in('Stock Sale','Standard Sale', 'On Sale','Store Out') and sallerID=Purchase.StockHolderID and VoucherID=" & Val(txtid.Text) & ") as RestNug  FROM Purchase " &
                       "  group by StockHolderID Having RestNug<>0;", "StockHolderName", "StockHolderID", "")
        End If
        If BtnSave.Text = "&Save" Then
            clsFun.FillDropDownList(CbStoreFrom, "Select  StorageID,StorageName From Purchase where StockHolderID=" & Val(cbAccountName.SelectedValue) & "  group by  StorageID,StorageName ", "StorageName", "StorageID", "")
        Else
            clsFun.FillDropDownList(CbStoreFrom, "Select  StorageID,StorageName From Purchase where StockHolderID=" & Val(cbAccountName.SelectedValue) & "  group by  StorageID,StorageName ", "StorageName", "StorageID", "")
        End If
        Me.BackColor = Color.FromArgb(247, 220, 111) : mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        clsFun.FillDropDownList(CbLot, "Select VoucherID,LotNo,(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID='" & Val(cbAccountName.SelectedValue) & "'  and ItemID = '" & Val(cbitemName.SelectedValue) & "' and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' )) as RestNug" &
                                  " from Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and StockHolderID=" & Val(cbAccountName.SelectedValue) & " and ItemID=" & Val(cbitemName.SelectedValue) & " Group By VoucherID having RestNug>0  order by LotNo ", "LotNo", "VoucherID", "")

        Me.KeyPreview = True : BtnDelete.Enabled = False : RowColumns() : VNumber()
    End Sub
    Private Sub VNumber()
        Dim Vno As Integer = 0
        If Vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
            Vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = Vno + 1
            txtInvoiceID.Text = Vno + 1
        Else
            Vno = Val(clsFun.ExecScalarStr("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'"))
            txtVoucherNo.Text = Vno + 1
            txtInvoiceID.Text = Vno + 1
        End If
    End Sub
    Private Sub RowColumns()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ItemID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Item Name" : dg1.Columns(1).Width = 493
        dg1.Columns(2).Name = "LotID" : dg1.Columns(2).Visible = False
        dg1.Columns(3).Name = "Lot No" : dg1.Columns(3).Width = 499
        dg1.Columns(4).Name = "Nugs" : dg1.Columns(4).Width = 178
    End Sub

    Private Sub cbitemName_GotFocus(sender As Object, e As EventArgs) Handles cbitemName.GotFocus
        'clsFun.FillDropDownList(cbitemName, "Select  ItemID,itemName From Purchase Where StockHolderID = '" & Val(cbAccountName.SelectedValue) & "' Group by ItemID ", "ItemName", "ItemID", "")
        Dim sql As String = String.Empty
        clsFun.FillDropDownList(cbitemName, "Select ItemID,ItemName,(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  and ItemID = Purchase.ItemID and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' )) as RestNug" &
                                           " from Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and StockHolderID=" & Val(cbAccountName.SelectedValue) & "  Group By ItemID having RestNug>0  order by ItemName ", "ItemName", "ItemID", "")
        If CbStoreto.SelectedValue = 0 Then CbStoreto.Focus() : Exit Sub
        ItemBalance()
    End Sub
    Private Sub ItemBalance()
        lblItemBalance.Visible = True
        Dim PurchaseBal As String = "" : Dim StockBal As String = ""
        Dim RestBal As String = "" : Dim tmpbal As String = ""
        Dim dt As New DataTable
        PurchaseBal = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(cbAccountName.SelectedValue) & " and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and ItemID = " & Val(cbitemName.SelectedValue) & " and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        StockBal = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(cbAccountName.SelectedValue) & " and StorageID=" & Val(CbStoreFrom.SelectedValue) & "  and ItemID = " & Val(cbitemName.SelectedValue) & " and TransType  in ('Stock Sale','On Sale','Standard Sale','Store Out') and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        RestBal = Val(PurchaseBal) - Val(StockBal)
        If BtnSave.Text = "&Save" Then
            If dg1.SelectedRows.Count = 0 Then
                If Val(StockBal) = 0 Then ' if no record inserted
                    If dg1.RowCount = 0 Then ' if no rows addred
                        bal = (RestBal)
                    Else 'if rows count
                        For i As Integer = 0 To dg1.RowCount - 1
                            If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(4).Value)
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
                            If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(4).Value)
                            End If
                        Next i
                        bal = Val(RestBal) - Val(tmpbal)
                    End If
                End If
            Else
                If Val(StockBal) = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(4).Value)
                        End If
                    Next i
                    tmpbal = Val(PurchaseBal) - Val(tmpbal)
                    tmpbal = Val(tmpbal) + Val(dg1.SelectedRows(0).Cells(4).Value)
                    bal = Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(4).Value)
                        End If
                    Next i
                    tmpbal = Val(RestBal) - Val(tmpbal)
                    tmpbal = (tmpbal) + Val(dg1.SelectedRows(0).Cells(4).Value)
                    bal = Val(tmpbal)
                End If
            End If
        Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
            If dg1.RowCount = 0 Then ' if no rows addred
                bal = (RestBal)
            Else 'if rows count
                UpdateTmp = clsFun.ExecScalarInt("Select sum(Nug) From Transaction2 Where SallerID=" & Val(cbAccountName.SelectedValue) & "  and ItemID = " & Val(cbitemName.SelectedValue) & " and StorageID=" & Val(CbStoreFrom.SelectedValue) & " AND VoucherID not in ('" & Val(txtid.Text) & "') and TransType  in ('Stock Sale','On Sale','Standard Sale','Store Out') and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                If dg1.SelectedRows.Count = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(4).Value)
                        End If
                    Next i
                    tmpbal = Val(UpdateTmp) + Val(tmpbal)
                    bal = Val(PurchaseBal) - Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(4).Value)
                        End If
                    Next i
                    tmpbal = Val(UpdateTmp) + Val(tmpbal)
                    If dg1.SelectedRows(0).Cells(0).Value = Val(cbitemName.SelectedValue) Then
                        '- Val(dg1.SelectedRows(0).Cells(5).Value)
                        tmpbal = Val(tmpbal) - Val(dg1.SelectedRows(0).Cells(4).Value)
                    End If
                    ' If (StockBal) = 0 Then

                    bal = Val(PurchaseBal) - Val(tmpbal)
                End If
            End If
        End If
        lblItemBalance.Text = "Bal. : " & Val(bal)
    End Sub
    Private Sub CbLot_GotFocus(sender As Object, e As EventArgs) Handles CbLot.GotFocus
        clsFun.FillDropDownList(CbLot, "Select VoucherID,LotNo,(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID='" & Val(cbAccountName.SelectedValue) & "'  and ItemID = '" & Val(cbitemName.SelectedValue) & "' and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' )) as RestNug" &
                                   " from Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and StockHolderID=" & Val(cbAccountName.SelectedValue) & " and ItemID=" & Val(cbitemName.SelectedValue) & " Group By VoucherID having RestNug>0  order by LotNo ", "LotNo", "VoucherID", "")
        If cbitemName.SelectedValue = 0 Then CbStoreto.Focus() : Exit Sub
        LotBalance()
    End Sub
    Private Sub LotBalance()
        lblLot.Visible = True
        Dim PurchaseLot As String = ""
        Dim StockLot As String = ""
        Dim RestLot As String = ""
        Dim tmpLot As String = ""
        Dim UpdatetmpLot As String = ""
        Dim tmpbal As String = ""
        Dim i As Integer = 0
        PurchaseLot = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(cbAccountName.SelectedValue) & " and StorageID=" & Val(CbStoreFrom.SelectedValue) & " and ItemID = " & Val(cbitemName.SelectedValue) & " and LotNo='" & CbLot.Text & "' and VoucherID= '" & Val(CbLot.SelectedValue) & "' ")
        '        PurchaseLot = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and LotNo='" & txtLot.Text & "' and VoucherID='" & txtPurchaseID.Text & "'")
        StockLot = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(cbAccountName.SelectedValue) & " and StorageID=" & Val(CbStoreFrom.SelectedValue) & "  and ItemID = " & Val(cbitemName.SelectedValue) & " and Lot='" & CbLot.Text & "' and PurchaseID= '" & Val(CbLot.SelectedValue) & "'  and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out')")
        RestLot = Val(PurchaseLot) - Val(StockLot)
        If BtnSave.Text = "&Save" Then
            If dg1.SelectedRows.Count = 0 Then
                If Val(StockLot) = 0 Then ' if no record inserted
                    If dg1.RowCount = 0 Then ' if no rows addred
                        LotBal = (StockLot)
                    Else 'if rows count
                        For i = 0 To dg1.RowCount - 1
                            If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) And dg1.Rows(i).Cells(3).Value = CbLot.Text Then
                                tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(4).Value)
                            End If
                        Next i
                        tmpLot = (tmpLot)
                    End If
                    LotBal = Val(PurchaseLot) - Val(tmpLot)
                Else
                    If dg1.RowCount = 0 Then ' if any Record Inserted in Database but Row not Added
                        LotBal = (RestLot)
                    Else
                        For i = 0 To dg1.RowCount - 1 'if any Record Inserted in Database and Row Added
                            If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) And dg1.Rows(i).Cells(3).Value = CbLot.Text Then
                                tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(4).Value)
                            End If
                        Next i
                        LotBal = Val(RestLot) - Val(tmpLot)
                    End If
                End If
            Else
                If Val(StockLot) = 0 Then
                    For i = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) And dg1.Rows(i).Cells(3).Value = CbLot.Text Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(4).Value)
                        End If
                    Next i
                    tmpLot = Val(PurchaseLot) - Val(tmpLot)
                    tmpLot = Val(tmpLot) + Val(dg1.SelectedRows(0).Cells(4).Value)
                    LotBal = Val(tmpLot)
                Else
                    For i = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) And dg1.Rows(i).Cells(3).Value = CbLot.Text Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(4).Value)
                        End If
                    Next i
                    tmpLot = Val(RestLot) - Val(tmpLot)
                    tmpLot = (tmpLot) + Val(dg1.SelectedRows(0).Cells(4).Value)
                    LotBal = Val(tmpLot)
                End If
            End If
        Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
            If dg1.RowCount = 0 Then ' if no rows addred
                LotBal = (RestLot)
            Else 'if rows count
                UpdatetmpLot = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(cbAccountName.SelectedValue) & "  and ItemID = " & Val(cbitemName.SelectedValue) & " " & _
                                                    "AND VoucherID  in ('" & Val(txtid.Text) & "')and Lot='" & CbLot.Text & "' and PurchaseID='" & Val(CbLot.SelectedValue) & "' and StorageID=" & Val(CbStoreFrom.SelectedValue) & " " & _
                                                    " and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out')")
                If dg1.SelectedRows.Count = 0 Then
                    For i = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) And dg1.Rows(i).Cells(3).Value = CbLot.Text Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(4).Value)
                        End If
                    Next i
                    tmpbal = Val(tmpLot)

                    LotBal = Val(PurchaseLot) - Val(tmpbal)
                Else
                    For i = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = Val(cbitemName.SelectedValue) And dg1.Rows(i).Cells(3).Value = CbLot.Text Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(4).Value)
                            'Else
                            '   MsgBox("Please Choose Selected Lot Only", MsgBoxStyle.Critical, "Check Lot") : txtLot.Focus() : Exit Sub
                        End If
                    Next i
                    ' If (StockBal) = 0 Then
                    'tmpLot = Val(UpdatetmpLot) + Val(Val(tmpLot) - Val(dg1.SelectedRows(0).Cells(5).Value))
                    If dg1.SelectedRows(0).Cells(0).Value = Val(cbitemName.SelectedValue) And dg1.SelectedRows(0).Cells(3).Value = CbLot.Text Then
                        tmpLot = Val(Val(tmpLot) - Val(dg1.SelectedRows(0).Cells(4).Value))
                    End If


                    ' tmpLot = Val(tmpLot) - dg1.SelectedRows(0).Cells(5).Value
                    LotBal = Val(PurchaseLot) - Val(tmpLot)
                End If
            End If
        End If
        lblLot.Text = "Bal. : " & Val(LotBal)
    End Sub
    Private Sub CbStoreto_GotFocus(sender As Object, e As EventArgs) Handles CbStoreto.GotFocus
        clsFun.FillDropDownList(CbStoreto, "Select  ID,StorageName From Storage where ID <>'" & Val(CbStoreFrom.SelectedValue) & "'  ", "StorageName", "ID", "")

        If CbStoreto.SelectedValue = 0 Then
            Store.MdiParent = MainScreenForm
            Store.Show() : Store.BringToFront()
            Exit Sub
        End If
    End Sub

    Private Sub CbStoreFrom_GotFocus(sender As Object, e As EventArgs) Handles CbStoreFrom.GotFocus
        If BtnSave.Text = "&Save" Then
            clsFun.FillDropDownList(CbStoreFrom, "Select  StorageID,StorageName From Purchase where StockHolderID=" & Val(cbAccountName.SelectedValue) & " group by  StorageID,StorageName ", "StorageName", "StorageID", "")
        Else
            clsFun.FillDropDownList(CbStoreFrom, "Select  StorageID,StorageName From Purchase where StockHolderID=" & Val(cbAccountName.SelectedValue) & "   group by  StorageID,StorageName ", "StorageName", "StorageID", "")
        End If
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown,
        cbAccountName.KeyDown, CbStoreFrom.KeyDown, CbStoreto.KeyDown, CbLot.KeyDown, cbitemName.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If CbStoreto.Focused Then
            If e.KeyCode = Keys.F3 Then
                Store.MdiParent = MainScreenForm
                Store.Show() : Store.BringToFront()
            End If
        End If
    End Sub

    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        If CbLot.SelectedValue = 0 Then CbStoreto.Focus() : Exit Sub
    End Sub

    Private Sub CbStoreto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbStoreto.SelectedIndexChanged

    End Sub

    Private Sub cbitemName_Leave(sender As Object, e As EventArgs) Handles cbitemName.Leave
        ItemBalance()
    End Sub

    Private Sub CbLot_Leave(sender As Object, e As EventArgs) Handles CbLot.Leave
        LotBalance()
    End Sub
    Private Sub txtNug_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNug.KeyDown


        If e.KeyCode = Keys.Enter Then
            If Val(txtNug.Text) = 0 Then MsgBox("Please fill Nug / Weight", MsgBoxStyle.Critical, "Empty") : txtNug.Focus() : Exit Sub
            If Val(LotBal) < Val(txtNug.Text) Then
                MsgBox("Not Enough Lot to Trnasfer", MsgBoxStyle.Critical, "Check Lot") : txtNug.Focus() : Exit Sub
            End If
            If e.KeyCode = Keys.Enter Then
                If dg1.SelectedRows.Count = 1 Then
                    dg1.SelectedRows(0).Cells(0).Value = Val(cbitemName.SelectedValue)
                    dg1.SelectedRows(0).Cells(1).Value = cbitemName.Text
                    dg1.SelectedRows(0).Cells(2).Value = (CbLot.SelectedValue)
                    dg1.SelectedRows(0).Cells(3).Value = CbLot.Text
                    dg1.SelectedRows(0).Cells(4).Value = Val(txtNug.Text)
                    cbitemName.Focus()
                Else
                    If lblLot.Text <> "" Then
                        dg1.Rows.Add(Val(cbitemName.SelectedValue), cbitemName.Text, (CbLot.SelectedValue), CbLot.Text, Val(txtNug.Text))
                        cbitemName.Focus()
                    End If
                End If
                ItemBalance() : LotBalance() : txtNug.Clear()
                lblItemBalance.Visible = False : lblLot.Visible = False
                dg1.ClearSelection()
            End If
        End If
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            cbitemName.SelectedValue = Val(dg1.SelectedRows(0).Cells(0).Value)
            cbitemName.Text = dg1.SelectedRows(0).Cells(1).Value
            CbLot.SelectedValue = Val(dg1.SelectedRows(0).Cells(2).Value)
            CbLot.Text = dg1.SelectedRows(0).Cells(3).Value
            txtNug.Text = Val(dg1.SelectedRows(0).Cells(4).Value)
            cbitemName.Focus() : e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        cbitemName.SelectedValue = Val(dg1.SelectedRows(0).Cells(0).Value)
        cbitemName.Text = dg1.SelectedRows(0).Cells(1).Value
        CbLot.SelectedValue = Val(dg1.SelectedRows(0).Cells(2).Value)
        CbLot.Text = dg1.SelectedRows(0).Cells(3).Value
        txtNug.Text = Val(dg1.SelectedRows(0).Cells(4).Value)
        cbitemName.Focus()
    End Sub
    Private Sub Save()
        Dim sql As String = "insert into Vouchers (EntryDate,AccountID,AccountName,StorageID,StorageName,T1,T2,Remark,InvoiceID,TransType) values (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", CDate(mskEntryDate.Text).ToString("yyy-MM-dd"))
            cmd.Parameters.AddWithValue("@2", Val(cbAccountName.SelectedValue))
            cmd.Parameters.AddWithValue("@3", cbAccountName.Text)
            cmd.Parameters.AddWithValue("@4", Val(CbStoreFrom.SelectedValue))
            cmd.Parameters.AddWithValue("@5", CbStoreFrom.Text)
            cmd.Parameters.AddWithValue("@6", Val(CbStoreto.SelectedValue))
            cmd.Parameters.AddWithValue("@7", CbStoreto.Text)
            cmd.Parameters.AddWithValue("@8", txtRemark.Text)
            cmd.Parameters.AddWithValue("@9", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@10", Me.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
                Dg1Record()
                MsgBox("Store Transfered Successfully.", vbInformation + vbOKOnly, "Transfered")
                dg1.Rows.Clear() : VNumber()
            End If
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub Dg1Record()
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                Dim ssql As String = "insert into Purchase(VoucherID,EntryDate, TransType,PurchaseTypeName,AccountID, AccountName,StockHolderID,StockHolderName, StorageID, StorageName,ItemID,ItemName,StockID,LotNo,Nug) " &
                                             " VALUES(" & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyy-MM-dd") & "','Store In','Store In','" & Val(cbAccountName.SelectedValue) & "', " &
                                             "'" & cbAccountName.Text & "', '" & Val(cbAccountName.SelectedValue) & "','" & cbAccountName.Text & "', " &
                                             "'" & Val(CbStoreto.SelectedValue) & "', '" & CbStoreto.Text & "','" & Val(.Cells(0).Value) & "', '" & .Cells(1).Value & "', " &
                                             "'" & Val(.Cells(2).Value) & "', '" & .Cells(3).Value & "', '" & .Cells(4).Value & "');"
                ssql = ssql & "insert into Transaction2(VoucherID,EntryDate, TransType ,SallerID, SallerName, StorageID, StorageName,ItemID,ItemName,PurchaseID,Lot,Nug) " &
                             " VALUES(" & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyy-MM-dd") & "','Store Out','" & Val(cbAccountName.SelectedValue) & "','" & cbAccountName.Text & "', " &
                             "'" & Val(CbStoreFrom.SelectedValue) & "', '" & CbStoreFrom.Text & "','" & Val(.Cells(0).Value) & "', '" & .Cells(1).Value & "', " &
                             "'" & Val(.Cells(2).Value) & "', '" & .Cells(3).Value & "', '" & .Cells(4).Value & "');"
                cmd = New SQLite.SQLiteCommand(ssql, clsFun.GetConnection())
            End With
            If cmd.ExecuteNonQuery() > 0 Then
                ' MsgBox("Record Inerted")
            End If
        Next
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If BtnSave.Text = "&Save" Then
            Save()
        Else
            UpdateRecord()
        End If
    End Sub
    Private Sub UpdateRecord()
        Dim Sql As String = String.Empty
        'Sql = "insert into Vouchers (EntryDate,AccountID,AccountName,StorageID,StorageName,T1,T2,Remark,InvoiceID,TransType) values (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10)"
        Sql = "Update Vouchers Set EntryDate='" & CDate(mskEntryDate.Text).ToString("yyy-MM-dd") & "', AccountID='" & Val(cbAccountName.SelectedValue) & "', " &
              "AccountName='" & cbAccountName.Text & "',StorageID='" & Val(CbStoreFrom.SelectedValue) & "', StorageName='" & CbStoreFrom.Text & "', " &
              "T1='" & Val(CbStoreto.SelectedValue) & "', T2='" & CbStoreto.Text & "' Where ID='" & Val(txtid.Text) & "'"
        cmd = New SQLite.SQLiteCommand(Sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(Sql) > 0 Then
                clsFun.ExecScalarStr("Delete From Purchase Where VoucherID='" & Val(txtid.Text) & "'; " &
                                     "Delete From Transaction2 Where VoucherID='" & Val(txtid.Text) & "';")
                Dg1Record()
                MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        'clsFun.FillDropDownList(CbStoreto, "Select  ID,StorageName From Storage where ID <>'" & Val(CbStoreFrom.SelectedValue) & "'  ", "StorageName", "ID", "")
        Dim ssql As String = String.Empty
        Dim primary As String = String.Empty
        BtnDelete.Enabled = True
        Dim dt As New DataTable
        BtnSave.BackColor = Color.Coral
        BtnSave.Image = My.Resources.icons8_edit_48px
        BtnSave.Text = "&Update"
        ssql = "Select * from Vouchers where id=" & Val(id)
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            mskEntryDate.Text = Format(dt.Rows(0)("EntryDate"), "dd-MM-yyyy")
            cbAccountName.SelectedValue = Val(dt.Rows(0)("AccountID").ToString())
            cbAccountName.Text = dt.Rows(0)("AccountName").ToString()
            CbStoreFrom.SelectedValue = Val(dt.Rows(0)("StorageID").ToString())
            CbStoreFrom.Text = dt.Rows(0)("StorageName").ToString()
            CbStoreto.SelectedValue = Val(dt.Rows(0)("T1").ToString())
            CbStoreto.Text = dt.Rows(0)("T2").ToString()
            txtRemark.Text = dt.Rows(0)("Remark").ToString()
            txtInvoiceID.Text = dt.Rows(0)("InvoiceID").ToString()
            txtid.Text = dt.Rows(0)("ID").ToString()

        End If
        Dim sql As String = "Select * from Purchase Where VoucherID=" & Val(txtid.Text)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad1.Fill(ds, "b")
        If ds.Tables("b").Rows.Count > 0 Then
            dg1.Rows.Clear()
            'With dg1
            ' If ds.Tables("b").Rows.Count > 9 Then dg1.Columns(10).Width = 109 Else dg1.Columns(10).Width = 129
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells(0).Value = Val(ds.Tables("b").Rows(i)("ItemID").ToString())
                    .Cells(1).Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                    .Cells(2).Value = Val(ds.Tables("b").Rows(i)("StockID").ToString())
                    .Cells(3).Value = ds.Tables("b").Rows(i)("LotNo").ToString()
                    .Cells(4).Value = ds.Tables("b").Rows(i)("Nug").ToString()
                End With
            Next

        End If
        dg1.ClearSelection()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub
    Private Sub Delete()
        If clsFun.ExecScalarInt("Select count(*) from Transaction2 where VoucherID=" & Val(txtid.Text) & "") <> 0 Then
            MsgBox("Stock Transfer Entry Already Used in Transactions", vbOKOnly, "Access Denied")
            Exit Sub
        End If
        If MessageBox.Show("Are you Sure want to Delete Store Transfer Entry : " & txtVoucherNo.Text & " ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
            If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "; " & _
                                   "DELETE from Purchase WHERE ID=" & Val(txtid.Text) & "; " & _
                                   "DELETE from Transaction2 WHERE ID=" & Val(txtid.Text) & ";") > 0 Then
                MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                VNumber()
            End If
        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtNug_TextChanged(sender As Object, e As EventArgs) Handles txtNug.TextChanged

    End Sub
End Class