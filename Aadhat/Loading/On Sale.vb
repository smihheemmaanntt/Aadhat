Imports System.IO

Public Class On_Sale
    Dim vno As Integer : Dim VchId As Integer
    Dim sql As String = String.Empty : Dim count As Integer = 0
    Dim MaxID As String = String.Empty : Dim itemBal As String = String.Empty
    Dim LotBal As String = String.Empty : Dim RestBal As String = String.Empty
    Dim UpdateTmp As Integer = 0 : Dim tmpID As String = String.Empty
    Dim bal As Decimal = 0.0 : Dim curindex As Integer = 0
    Dim CalcType As String = String.Empty : Dim ServerTag As Integer
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 1 : Dim Offset As Integer = 0
    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.BackColor = Color.LightGray
        mskEntryDate.SelectAll()
    End Sub

    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub Stock_Transfer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If dg1.RowCount = 0 Then
                Me.Close()
            ElseIf pnlMarka.Visible = True Then
                pnlMarka.Visible = False
            Else
                Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "AADHAT")
                If msgRslt = MsgBoxResult.Yes Then
                    Me.Close()
                ElseIf msgRslt = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub Stock_Transfer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        CbPer.SelectedIndex = 0
        ' pnlMarka.Visible = False
        RowColumns() : dg2RownColums()
        PurchaseRowColums() : VNumber()
        BtnDelete.Enabled = False
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
    End Sub
    Private Sub PurchaseRowColums()
        dgPurchase.ColumnCount = 5
        dgPurchase.Columns(0).Name = "ID" : dgPurchase.Columns(0).Visible = False
        dgPurchase.Columns(1).Name = "Entry Date" : dgPurchase.Columns(1).Width = 100
        dgPurchase.Columns(2).Name = "Account Name" : dgPurchase.Columns(2).Width = 150
        dgPurchase.Columns(3).Name = "Type" : dgPurchase.Columns(3).Width = 100
        dgPurchase.Columns(4).Name = "Rest Nug" : dgPurchase.Columns(4).Width = 100
        dgPurchase.Visible = True : dgPurchase.BringToFront() ': retrivePurchaseType()
    End Sub

    Private Sub retrivePurchase(Optional ByVal condtion As String = "")
        dgPurchase.Rows.Clear()
        Dim sql As String = String.Empty
        sql = "Select VoucherID,EntryDate,StockHolderID,StockHolderName,AccountID ,AccountName ,PurchaseTypeName,ifnull(sum(Nug),0) as PurchaseNug,(ifnull(sum(Nug),0)-(Select ifnull(sum(Nug),0)  from Transaction2 " & _
                " where Transtype in('Stock Sale','Standard Sale', 'On Sale') and sallerID=Purchase.StockHolderID and PurchaseID=Purchase.VoucherID)) as RestNug  FROM Purchase " & _
                " " & condtion & " group by VoucherID,StockHolderID Having RestNug=PurchaseNug Limit 5;"
    
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dgPurchase.Rows.Add()
                    With dgPurchase.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(3).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                        .Cells(4).Value = dt.Rows(i)("RestNug").ToString()
                    End With
                Next
            End If
        Catch ex As Exception

        End Try
        dgPurchase.ClearSelection()
    End Sub



    Private Sub dg2RownColums()
        Dg2.ColumnCount = 7
        Dg2.Columns(0).Name = "Charge Name" : Dg2.Columns(0).Width = 273 : Dg2.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable : Dg2.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        Dg2.Columns(1).Name = "On" : Dg2.Columns(1).Width = 129 : Dg2.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable : Dg2.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(2).Name = "Cal" : Dg2.Columns(2).Width = 99 : Dg2.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : Dg2.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(3).Name = "+/-" : Dg2.Columns(3).Width = 98 : Dg2.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable : Dg2.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dg2.Columns(4).Name = "Amount" : Dg2.Columns(4).Width = 149 : Dg2.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : Dg2.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(5).Name = "ChargeID" : Dg2.Columns(5).Visible = False : Dg2.Columns(6).Name = "ID" : Dg2.Columns(6).Visible = False
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 31
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
            .Columns(29).Name = "OtherAccountName" : .Columns(29).Width = 159
            .Columns(30).Name = "OtherSallerName" : .Columns(30).Width = 159
        End With
    End Sub
    Private Sub RowColumns()
        dg1.ColumnCount = 17
        dg1.Columns(0).Name = "SellerID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Seller Name" : dg1.Columns(1).Width = 290
        dg1.Columns(2).Name = "StoreID" : dg1.Columns(2).Visible = False
        dg1.Columns(3).Name = "Store Name" : dg1.Columns(3).Width = 138
        dg1.Columns(4).Name = "ItemID" : dg1.Columns(4).Visible = False
        dg1.Columns(5).Name = "ItemName" : dg1.Columns(5).Width = 180 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).Name = "LotNo" : dg1.Columns(6).Width = 118 : dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(7).Name = "Nug" : dg1.Columns(7).Width = 80 : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).Name = "Weight" : dg1.Columns(8).Width = 80 : dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Rate" : dg1.Columns(9).Width = 80 : dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "Per" : dg1.Columns(10).Width = 74 : dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(11).Name = "Total" : dg1.Columns(11).Width = 130 : dg1.Columns(11).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(12).Name = "PurachseID" : dg1.Columns(12).Visible = False
        dg1.Columns(13).Name = "MaintainCrate" : dg1.Columns(13).Visible = False
        dg1.Columns(14).Name = "CrateID" : dg1.Columns(14).Visible = False
        dg1.Columns(15).Name = "CrateName" : dg1.Columns(15).Visible = False
        dg1.Columns(16).Name = "CrateQty" : dg1.Columns(16).Visible = False
    End Sub
    Private Sub Calculation()
        If CbPer.SelectedIndex = 0 Then
            If String.IsNullOrEmpty(txtNug.Text) OrElse String.IsNullOrEmpty(txtRate.Text) Then Exit Sub
            txtAmount.Text = Format(CDbl(txtNug.Text) * CDbl(txtRate.Text), "0.00")
        ElseIf CbPer.SelectedIndex = 1 Then
            If String.IsNullOrEmpty(txtWeight.Text) OrElse String.IsNullOrEmpty(txtRate.Text) Then Exit Sub
            txtAmount.Text = Format(CDbl(txtWeight.Text) * CDbl(txtRate.Text), "0.00")
        ElseIf CbPer.SelectedIndex = 2 Then
            If String.IsNullOrEmpty(txtWeight.Text) OrElse String.IsNullOrEmpty(txtRate.Text) Then Exit Sub
            txtAmount.Text = Format(CDbl(txtRate.Text) / 40 * CDbl(txtWeight.Text), "0.00")
        End If
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
    Private Sub txtclear()
        cbCrateMarka.SelectedValue = 0
        txtNug.Clear() : txtWeight.Clear()
        txtRate.Clear() : txtAmount.Clear()
        txtPurchaseType.Focus() : calc() : dg1.ClearSelection()
        ItemBalance() : LotBalance()
    End Sub
    Private Sub txtclearall()
        VNumber() : BtnSave.Text = "&Save"
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        txtAccount.Clear() : txtitem.Clear()
        txtPurchaseType.Clear() : txtLot.Clear()
        txtStoreName.Clear()
        txtclear() : ChargesClear() : calc()
        dgPurchaseType.Visible = False
        DgAccountSearch.Visible = False
        dgItemSearch.Visible = False
        dgCharges.Visible = False
        dgLot.Visible = False
        mskEntryDate.Focus()

    End Sub
    Sub retrivePrint()
        'MsgBox(txtid.Text)
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        Dim sql As String = String.Empty
        sql = "SELECT * FROM Vouchers AS V INNER JOIN Transaction2 AS T ON T.VoucherID = V.ID Where V.ID=" & Val(txtid.Text) & ""
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
                    .Cells(4).Value = dt.Rows(i)("SallerName1").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
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
                    .Cells(25).Value = .Cells(25).Value & clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("SallerID1").ToString()) & "")
                    .Cells(26).Value = Format(Val(.Cells(26).Value & dt.Rows(i)("RoundOff").ToString()), "0.00")
                    .Cells(29).Value = .Cells(29).Value & dt.Rows(i)("Cratemarka").ToString()
                    .Cells(30).Value = .Cells(30).Value & dt.Rows(i)("CrateQty1").ToString()
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

    Private Sub PurchaseTypeColumns()
        dgPurchaseType.ColumnCount = 3
        dgPurchaseType.Columns(0).Name = "ID" : dgPurchaseType.Columns(0).Visible = False
        dgPurchaseType.Columns(1).Name = "Account Name" : dgPurchaseType.Columns(1).Width = 300
        dgPurchaseType.Columns(2).Name = "City" : dgPurchaseType.Columns(2).Width = 94
        dgPurchaseType.Visible = True : dgPurchaseType.BringToFront()
    End Sub
    Private Sub retrivePurchaseType(Optional ByVal condtion As String = "")
        dgPurchaseType.Rows.Clear()
        Dim sql As String = String.Empty
        Dim dt As New DataTable
        If BtnSave.Text = "&Save" Then
            sql = "Select StockHolderID ,StockHolderName ,PurchaseTypeName,(ifnull(sum(Nug),0)-(Select ifnull(sum(Nug),0)  from Transaction2 " & _
                " where Transtype in('Stock Sale','Standard Sale', 'On Sale') and sallerID=Purchase.StockHolderID )) as RestNug  FROM Purchase " & _
                " " & condtion & " group by StockHolderID Having RestNug<>0;"
        Else
            sql = "Select StockHolderID ,StockHolderName ,PurchaseTypeName,(ifnull(sum(Nug),0)-(Select ifnull(sum(Nug),0)  from Transaction2 " & _
                       " where Transtype in('Stock Sale','Standard Sale', 'On Sale') and sallerID=Purchase.StockHolderID)) " & _
                       " +(Select ifnull(sum(Nug),0)  from Transaction2 where Transtype in('Stock Sale','Standard Sale', 'On Sale') and sallerID=Purchase.StockHolderID and VoucherID=" & Val(txtid.Text) & ") as RestNug  FROM Purchase " & _
                       " " & condtion & " group by StockHolderID Having RestNug<>0;"
        End If
        ' sql = "Select StockHolderID,StockholderName,PurchaseTypeName From Purchase Group By StockHolderID;"
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                dgPurchaseType.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgPurchaseType.Rows.Add()
                    With dgPurchaseType.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("StockHolderID").ToString()
                        .Cells(1).Value = dt.Rows(i)("StockHolderName").ToString()
                        .Cells(2).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub txtaccountName_GotFocus(sender As Object, e As EventArgs) Handles txtPurchaseType.GotFocus
        dgPurchaseType.Visible = True : dgItemSearch.Visible = False : dgLot.Visible = False
        dgStore.Visible = False : DgAccountSearch.Visible = False : dgCharges.Visible = False
        dgItemSearch.Visible = False : dgLot.Visible = False : dgPurchaseType.Visible = False
        dgStore.Visible = False : DgAccountSearch.Visible = False : dgCharges.Visible = False
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then txtAccount.Focus() : Exit Sub
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        txtPurchaseType.SelectionStart = 0 : txtPurchaseType.SelectionLength = Len(txtPurchaseType.Text)
        If dgPurchaseType.ColumnCount = 0 Then PurchaseTypeColumns()
        If txtPurchaseType.Text.Trim() <> "" Then
            retrivePurchaseType(" Where upper(StockHolderName) Like upper('" & txtPurchaseType.Text.Trim() & "%')")
        Else
            retrivePurchaseType()
        End If

    End Sub
    Private Sub txtaccountName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPurchaseType.KeyPress
        dgPurchaseType.BringToFront()
        dgPurchaseType.Visible = True
        dgItemSearch.Visible = False
        dgStore.Visible = False
        DgAccountSearch.Visible = False
    End Sub

    Private Sub txtaccountName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPurchaseType.KeyUp
        'PurchaseTypeColumns()
        If dgPurchaseType.RowCount = 0 Then PurchaseTypeColumns()
        If txtPurchaseType.Text.Trim() <> "" Then
            retrivePurchaseType(" Where upper(StockHolderName) Like upper('" & txtPurchaseType.Text.Trim() & "%')")
        Else
            retrivePurchaseType()
        End If
        If e.KeyCode = Keys.Escape Then
            If dgPurchaseType.Visible = True Then dgPurchaseType.Visible = False
            Exit Sub
        End If
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 205
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 150
        DgAccountSearch.Visible = True : DgAccountSearch.BringToFront()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16,17)  or UnderGroupID in (11,16,17))" & condtion & " order by AccountName Limit 11")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("City").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
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
    Private Sub StockInItemRowColums()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 160
        dgItemSearch.Columns(2).Name = "Nug" : dgItemSearch.Columns(2).Width = 130
        StockINretriveItems() : dgItemSearch.Visible = True : dgItemSearch.BringToFront()
    End Sub
    Private Sub StockINretriveItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable : dgItemSearch.Rows.Clear()
        Dim sql As String = String.Empty
        If BtnSave.Text = "&Save" Then

            sql = "Select ItemID,ItemName," &
        "(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  and ItemID = Purchase.ItemID and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=" & Val(txtStorageID.Text) & " and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' )) as RestNug" &
        " from Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and StorageID=" & Val(txtStorageID.Text) & " and StockHolderID=" & Val(txtPurchaseTypeID.Text) & "  " & condtion & " Group By ItemID having RestNug>0  order by ItemName"
        Else
            sql = "Select ID,VoucherID,ItemID,ItemName,StockHolderID,ifnull(sum(Nug),0) as PurchaseNug," &
           "(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  and ItemID = Purchase.ItemID and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out')) as SoldNug," &
           "(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  and ItemID = Purchase.ItemID and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=" & Val(txtStorageID.Text) & " and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' )" &
           "+(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID =Purchase.ItemID  and VoucherID=" & Val(txtid.Text) & " and TransType in('Stock Sale','On Sale','Store Out') and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' )) as RestNug" &
           " from Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and StorageID=" & Val(txtStorageID.Text) & " and StockHolderID=" & Val(txtPurchaseTypeID.Text) & "  " & condtion & " Group By ItemID having RestNug<>0 order by ItemName"
        End If
        dt = clsFun.ExecDataTable(sql)
        Try
            Dim lastval As Integer = 0
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1

                    ' Application.DoEvents()
                    ItemBalCheck = 0 : SelectedItemBal = 0
                    For j As Integer = 0 To dg1.RowCount - 1
                        If Val(dg1.Rows(j).Cells(4).Value) = Val(txtItemID.Text) Then
                            ItemBalCheck = Val(ItemBalCheck) + Val(dg1.Rows(j).Cells(7).Value)
                        End If
                    Next j
                    If dg1.SelectedRows.Count <> 0 Then
                        If Val(dg1.SelectedRows(0).Cells(4).Value) = Val(txtItemID.Text) Then
                            SelectedItemBal = Val(SelectedItemBal) + Val(dg1.SelectedRows(0).Cells(7).Value)
                        End If
                    End If
                    ItemBalCheck = Val(Val(dt.Rows(i)("RestNug").ToString()) + Val(SelectedItemBal)) - Val(ItemBalCheck)
                    dgItemSearch.Rows.Add()
                    With dgItemSearch.Rows(lastval)
                        .Cells(0).Value = dt.Rows(i)("ItemID").ToString()
                        .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(2).Value = Val(ItemBalCheck)
                        lastval = lastval + 1
                    End With
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub StoreColums()
        dgStore.ColumnCount = 2
        dgStore.Columns(0).Name = "ID" : dgStore.Columns(0).Visible = False
        dgStore.Columns(1).Name = "Mode Name" : dgStore.Columns(1).Width = 230
        RetriveMode() : dgStore.Visible = True : dgStore.BringToFront()
    End Sub
    Private Sub LotCoulmns()
        dgLot.ColumnCount = 6
        dgLot.Columns(0).Name = "LotID" : dgLot.Columns(0).Visible = False
        dgLot.Columns(1).Name = "Lot" : dgLot.Columns(1).Width = 100
        dgLot.Columns(2).Name = "Vehicle No." : dgLot.Columns(2).Width = 120
        dgLot.Columns(3).Name = "Date" : dgLot.Columns(3).Width = 80
        dgLot.Columns(4).Name = "Account Name" : dgLot.Columns(4).Width = 200
        dgLot.Columns(5).Name = "Balance" : dgLot.Columns(5).Width = 100
        dgLot.Visible = True : RetriveLot()
    End Sub
    Private Sub RetriveLot(Optional ByVal condtion As String = "")
        dgLot.Rows.Clear()
        Dim LotBalCheck As String = 0
        Dim SelectedLotBal As String = 0
        Dim sql As String = String.Empty
        If BtnSave.Text = "&Save" Then
            sql = "Select VoucherID, LotNo,VehicleNo,EntryDate,AccountName," &
                           "(nug-(Select ifnull(sum(nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out') and sallerID=Purchase.StockHolderID  and ItemID=Purchase.ItemID " &
                           "and Lot=Purchase.LotNo and PurchaseID=Purchase.VoucherID and StorageID=" & Val(txtStorageID.Text) & ")) as RestNug From Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and StorageID=" & Val(txtStorageID.Text) & " and StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and ItemID=" & Val(txtItemID.Text) & "  and RestNug > 0  " & condtion & ""
        Else
            sql = "Select VoucherID, LotNo,VehicleNo,EntryDate,AccountName," &
                        "(nug-(Select ifnull(sum(nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out') and sallerID=Purchase.StockHolderID  and ItemID=Purchase.ItemID " &
                        "and Lot=Purchase.LotNo and PurchaseID=Purchase.VoucherID))+(Select ifnull(sum(nug),0) from Transaction2 where Transtype in('Stock Sale','Standard Sale', 'On Sale','Store Out')and sallerID=Purchase.StockHolderID  and ItemID=Purchase.ItemID " &
                        "and Lot=Purchase.LotNo and PurchaseID=Purchase.VoucherID and VoucherID=" & Val(txtid.Text) & " and StorageID=" & Val(txtStorageID.Text) & ")   as RestNug From Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and  StorageID=" & Val(txtStorageID.Text) & " and StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and ItemID=" & Val(txtItemID.Text) & "  and RestNug >0    " & condtion & ""

        End If

        dt = clsFun.ExecDataTable(sql)
        Try
            Dim lastval As Integer = 0
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    ' Application.DoEvents()
                    LotBalCheck = 0 : SelectedLotBal = 0
                    For j As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(j).Cells(6).Value = dt.Rows(i)("LotNo").ToString() AndAlso Val(dg1.Rows(j).Cells(4).Value) = Val(txtItemID.Text) AndAlso Val(dg1.Rows(j).Cells(12).Value) = Val(dt.Rows(i)("VoucherID").ToString()) Then
                            LotBalCheck = Val(LotBalCheck) + Val(dg1.Rows(j).Cells(7).Value)
                        End If
                    Next j
                    If dg1.SelectedRows.Count <> 0 Then
                        If dg1.SelectedRows(0).Cells(6).Value = dt.Rows(i)("LotNo").ToString() AndAlso Val(dg1.SelectedRows(0).Cells(4).Value) = Val(txtItemID.Text) AndAlso Val(dg1.SelectedRows(0).Cells(12).Value) = Val(dt.Rows(i)("VoucherID").ToString()) Then
                            SelectedLotBal = Val(SelectedLotBal) + Val(dg1.SelectedRows(0).Cells(7).Value)
                        End If
                    End If
                    LotBalCheck = Val(Val(dt.Rows(i)("RestNug").ToString()) + Val(SelectedLotBal)) - Val(LotBalCheck)
                    If Val(LotBalCheck) > 0 Then
                        dgLot.Rows.Add()
                        With dgLot.Rows(lastval)
                            .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                            .Cells(1).Value = dt.Rows(i)("LotNo").ToString()
                            .Cells(2).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(3).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                            .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                            .Cells(5).Value = Val(LotBalCheck)
                            lastval = lastval + 1
                        End With
                    End If
                Next
            End If
        Catch ex As Exception

        End Try

    End Sub

    'Private Sub RetriveLot1(Optional ByVal condtion As String = "")
    '    dgLot.Rows.Clear()
    '    Dim PurchaseLot As String = ""
    '    Dim StockLot As String = ""
    '    Dim RestLot As String = ""
    '    Dim dt As New DataTable
    '    Dim tmpval As Integer = 0
    '    If BtnSave.Text = "&Save" Then
    '        sql = "Select VoucherID, LotNo,VehicleNo,EntryDate,AccountName," & _
    '                       "(nug-(Select ifnull(sum(nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out') and sallerID=Purchase.StockHolderID  and ItemID=Purchase.ItemID " & _
    '                       "and Lot=Purchase.LotNo and PurchaseID=Purchase.VoucherID)) as RestNug From Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and StorageID=" & Val(txtStorageID.Text) & " and StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and ItemID=" & Val(txtItemID.Text) & "  and RestNug > 0  " & condtion & ""
    '    Else
    '        sql = "Select VoucherID, LotNo,VehicleNo,EntryDate,AccountName," & _
    '                    "(nug-(Select ifnull(sum(nug),0) from Transaction2 where Transtype in ('Stock Sale','On Sale','Standard Sale','Store Out') and sallerID=Purchase.StockHolderID  and ItemID=Purchase.ItemID " & _
    '                    "and Lot=Purchase.LotNo and PurchaseID=Purchase.VoucherID))+(Select ifnull(sum(nug),0) from Transaction2 where Transtype in('Stock Sale','Standard Sale','On Sale','Store Out')and sallerID=Purchase.StockHolderID  and ItemID=Purchase.ItemID " & _
    '                    "and Lot=Purchase.LotNo and PurchaseID=Purchase.VoucherID and VoucherID=" & Val(txtid.Text) & ")   as RestNug From Purchase where EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and  StorageID=" & Val(txtStorageID.Text) & " and StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and ItemID=" & Val(txtItemID.Text) & "  and RestNug >0    " & condtion & ""

    '    End If

    '    dt = clsFun.ExecDataTable(sql)

    '    'dt = clsFun.ExecDataTable("Select VoucherID, LotNo,VehicleNo From Purchase where StorageID=" & txtStorageID.Text & " and StockHolderID=" & txtPurchaseTypeID.Text & " and ItemID=" & txtItemID.Text & " " & condtion & " ")
    '    Try
    '        If dt.Rows.Count > 0 Then
    '            dgLot.Rows.Clear()
    '            For i = 0 To dt.Rows.Count - 1
    '                If BtnSave.Text = "&Save" Then
    '                    PurchaseLot = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and LotNo='" & dt.Rows(i)("LotNo").ToString() & "'")
    '                    StockLot = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and Lot='" & dt.Rows(i)("LotNo").ToString() & "'and TransType in ('Stock Sale','On Sale','Standard Sale')")
    '                    RestLot = Val(PurchaseLot) - Val(StockLot)
    '                    If RestLot > 0 Then
    '                        dgLot.Rows.Add()

    '                        With dgLot.Rows(tmpval)
    '                            .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
    '                            .Cells(1).Value = dt.Rows(i)("LotNo").ToString()
    '                            .Cells(2).Value = dt.Rows(i)("VehicleNo").ToString()
    '                            tmpval = tmpval + 1
    '                        End With
    '                    End If
    '                Else
    '                    dgLot.Rows.Add()

    '                    With dgLot.Rows(i)
    '                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
    '                        .Cells(1).Value = dt.Rows(i)("LotNo").ToString()
    '                        .Cells(2).Value = dt.Rows(i)("VehicleNo").ToString()
    '                        '  tmpval = tmpval + 1
    '                    End With
    '                End If

    '            Next
    '        End If
    '        dt.Dispose()
    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
    '    End Try
    'End Sub
    Private Sub txtLot_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLot.KeyPress
        dgLot.BringToFront()
        LotCoulmns()
        dgLot.Visible = True
    End Sub
    Private Sub txtLot_KeyUp(sender As Object, e As KeyEventArgs) Handles txtLot.KeyUp
        If dgLot.RowCount = 0 Then Exit Sub
        If txtLot.Text.Trim() <> "" Then
            RetriveLot(" And upper(LotNo) like upper('" & txtLot.Text.Trim() & "%')")
        End If
    End Sub
    Private Sub dgLot_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgLot.CellClick
        txtLot.Clear()
        txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
        '  txtlotid.Text = dgLot.SelectedRows(0).Cells(0).Value
        dgLot.Visible = False
        txtNug.Focus()
    End Sub

    Private Sub RetriveMode(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        'dt = clsFun.ExecDataTable("Select  StorageID,StorageName From Purchase where StockHolderID=" & txtPurchaseTypeID.Text & " " & condtion & "  group by  StorageID,StorageName ")
        If BtnSave.Text = "&Save" Then
            sql = "Select  StorageID,StorageName,(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  " & _
            " and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=Purchase.StorageID  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as RestNug " & _
            "From Purchase where StockHolderID=" & Val(txtPurchaseTypeID.Text) & "  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & "  group by  StorageID,StorageName having RestNug<>0 order by StorageName "
        Else
            sql = "Select  StorageID,StorageName,(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  " & _
            " and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=Purchase.StorageID  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
            "+(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  " & _
            " and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=Purchase.StorageID  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') as RestNug " & _
            "From Purchase where StockHolderID=" & Val(txtPurchaseTypeID.Text) & "  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & "  group by  StorageID,StorageName having RestNug<>0 order by StorageName"
        End If
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                dgStore.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgStore.Rows.Add()
                    With dgStore.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("StorageID").ToString()
                        .Cells(1).Value = dt.Rows(i)("StorageName").ToString()
                    End With
                Next

            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub txtLot_GotFocus(sender As Object, e As EventArgs) Handles txtLot.GotFocus
        dgPurchaseType.Visible = False : dgItemSearch.Visible = False : dgLot.Visible = True
        dgStore.Visible = False : DgAccountSearch.Visible = False : dgCharges.Visible = False
        If dgItemSearch.ColumnCount = 0 Then StockInItemRowColums()
        If dgItemSearch.RowCount = 0 Then StockINretriveItems()
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtitem.Focus() : Exit Sub
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtitem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        ItemBalance()
        If bal = 0 Then txtitem.Focus() : MsgBox("Item Balance is Zero...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        dgItemSearch.Visible = False
        If dgLot.ColumnCount = 0 Then LotCoulmns()
        If dgLot.RowCount = 0 Then RetriveLot()
        If txtLot.Text.Trim() <> "" Then
            RetriveLot(" And upper(LotNo) like upper('" & txtLot.Text.Trim() & "%')")
        Else
            txtPurchaseID.Clear()
            RetriveLot()
        End If
        txtLot.SelectionStart = 0 : txtLot.SelectionLength = Len(txtLot.Text)
    End Sub

    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        If dgLot.ColumnCount = 0 Then LotCoulmns()
        If dgLot.RowCount = 0 Then RetriveLot()
        If dgLot.SelectedRows.Count = 0 Then dgLot.Visible = True : txtLot.Focus() : Exit Sub
        txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
        txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
        dgLot.Visible = False
        LotBalance() : txtNug.Focus()
        txtNug.SelectionStart = 0 : txtNug.SelectionLength = Len(txtNug.Text)
        LotBalance()
        If LotBal = 0 Then txtLot.Focus() : MsgBox("Item Balance is Zero...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        dgItemSearch.Visible = False
        txtNug.SelectionStart = 0 : txtNug.SelectionLength = Len(txtNug.Text)
        If lblCrate.Text = "Y" Then
            pnlMarka.Visible = True : pnlMarka.BringToFront()
        Else
            cbCrateMarka.SelectedIndex = -1
            pnlMarka.Visible = False
            txtCrateQty.Text = 0
        End If
    End Sub
    Private Sub AcBal()
        'Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtAccountID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = Math.Abs(Val(opbal)) & " Cr"
        Else
            opbal = Math.Abs(Val(opbal)) & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Math.Abs(Val(opbal)) & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Math.Abs(Val(tmpamt)) & " Cr"
            Else
                opbal = Math.Abs(Val(tmpamt)) & " Dr"
            End If
        End If
        lblAcBal.Visible = True
        lblAcBal.Text = "Bal : " & opbal
    End Sub
    Private Sub txtStoreName_GotFocus(sender As Object, e As EventArgs) Handles txtStoreName.GotFocus
        dgPurchaseType.Visible = False : dgItemSearch.Visible = False : dgLot.Visible = False
        dgStore.Visible = True : DgAccountSearch.Visible = False : dgCharges.Visible = False
        If dgPurchaseType.ColumnCount = 0 Then PurchaseTypeColumns()
        If dgPurchaseType.Rows.Count = 0 Then retrivePurchaseType()
        If dgPurchaseType.SelectedRows.Count = 0 Then txtPurchaseType.Focus() : Exit Sub
        txtPurchaseTypeID.Text = dgPurchaseType.SelectedRows(0).Cells(0).Value
        txtPurchaseType.Text = dgPurchaseType.SelectedRows(0).Cells(1).Value
        StoreColums() : AcBal()
        dgPurchaseType.Visible = False
    End Sub

    Private Sub txtitem_GotFocus(sender As Object, e As EventArgs) Handles txtitem.GotFocus
        dgPurchaseType.Visible = False : dgItemSearch.Visible = True : dgLot.Visible = False
        dgStore.Visible = False : DgAccountSearch.Visible = False : dgCharges.Visible = False
        If dgStore.ColumnCount = 0 Then StoreColums()
        If dgStore.RowCount = 0 Then RetriveMode()
        If dgStore.SelectedRows.Count = 0 Then dgStore.Visible = True : txtStoreName.Focus() : Exit Sub
        txtStorageID.Text = Val(dgStore.SelectedRows(0).Cells(0).Value)
        txtStoreName.Text = dgStore.SelectedRows(0).Cells(1).Value
        dgStore.Visible = False ': AcBal()
        If dgItemSearch.ColumnCount = 0 Then StockInItemRowColums()
        If dgItemSearch.RowCount = 0 Then StockINretriveItems()
        If txtitem.Text.Trim() <> "" Then
            StockINretriveItems(" And upper(ItemName) Like upper('" & txtitem.Text.Trim() & "%')")
        Else
            txtItemID.Clear()
            StockINretriveItems()
        End If
        txtitem.SelectionStart = 0 : txtitem.SelectionLength = Len(txtitem.Text)

    End Sub

    Private Sub txtVehicleNo_GotFocus(sender As Object, e As EventArgs) Handles txtVehicleNo.GotFocus
        '   retrivePurchase()
    End Sub
    Private Sub txtVehicleNo_Leave(sender As Object, e As EventArgs) Handles txtVehicleNo.Leave
        '  If dg1.Rows.Count = 0 And dgPurchase.Rows.Count <> 0 Then pnlPurchaseList.Visible = True : txtPurchaseSearch.Focus() Else pnlPurchaseList.Visible = False
    End Sub

    Private Sub txtWeight_GotFocus(sender As Object, e As EventArgs) Handles txtWeight.GotFocus
        pnlMarka.Visible = False
    End Sub
    Private Sub txtVoucherNo_LostFocus(sender As Object, e As EventArgs) Handles txtVoucherNo.LostFocus, txtVehicleNo.LostFocus, txtAccount.LostFocus, txtPurchaseType.LostFocus,
    txtStoreName.LostFocus, txtitem.LostFocus, txtLot.LostFocus, txtNug.LostFocus, txtWeight.LostFocus, txtRate.LostFocus, txtDriverName.LostFocus, txtMobile.LostFocus, txtRemark.LostFocus, txtCrateQty.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        If tb.Text Is mskEntryDate Then mskEntryDate.SelectAll() : mskEntryDate.BackColor = Color.LightGray : Exit Sub
        tb.BackColor = Color.GhostWhite
        tb.SelectAll()
    End Sub

    Private Sub txtvoucherNo_GotFocus(sender As Object, e As EventArgs) Handles txtVoucherNo.GotFocus, txtVehicleNo.GotFocus, txtAccount.GotFocus, txtPurchaseType.GotFocus,
    txtStoreName.GotFocus, txtitem.GotFocus, txtLot.GotFocus, txtNug.GotFocus, txtWeight.GotFocus, txtRate.GotFocus, txtDriverName.GotFocus, txtMobile.GotFocus, txtRemark.GotFocus, txtCrateQty.GotFocus
        Dim tb As TextBox = CType(sender, TextBox)
        '   If tb.Text Is mskEntryDate Then mskEntryDate.BackColor = Color.GhostWhite : Exit Sub
        tb.BackColor = Color.LightGray
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown, txtVehicleNo.KeyDown, txtAccount.KeyDown, txtPurchaseType.KeyDown,
        txtStoreName.KeyDown, txtitem.KeyDown, txtLot.KeyDown, txtNug.KeyDown, txtWeight.KeyDown, txtRate.KeyDown, CbPer.KeyDown, txtDriverName.KeyDown, txtMobile.KeyDown, txtRemark.KeyDown,
        cbCrateMarka.KeyDown, txtCrateQty.KeyDown
        If txtVoucherNo.Focused Then
            If e.KeyCode = Keys.F3 Then
                If pnlInvoiceID.Visible = True Then
                    pnlInvoiceID.Visible = False : txtVoucherNo.Focus()

                Else
                    pnlInvoiceID.Visible = True : txtInvoiceID.Focus()
                End If

            End If
        End If


        If txtAccount.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If e.KeyCode = Keys.PageDown Then
            txtCharges.Focus()
        End If
        If pnlSendingDetails.Focused Then
            If e.KeyCode = Keys.Escape Then
                pnlSendingDetails.Visible = False
                txtVehicleNo.Focus()
            End If
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then
                DgAccountSearch.Focus()
            End If
        End If
        If txtVoucherNo.Focused Then
            If e.KeyCode = Keys.F4 Then
                pnlInvoiceID.Visible = True
            End If
        End If
        If txtVehicleNo.Focused Then
            If e.KeyCode = Keys.F4 Then
                If pnlSendingDetails.Visible = False Then
                    pnlSendingDetails.Visible = True
                    txtDriverName.Focus()
                Else
                    pnlSendingDetails.Visible = False
                    txtVehicleNo.Focus()
                End If
            End If
        End If
        If DgAccountSearch.Visible = False And dgItemSearch.Visible = False And dgPurchaseType.Visible = False And dgStore.Visible = False And dgLot.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or CbPer.Focused = True Or cbCrateMarka.Focused = True Or txtCrateQty.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True

        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus()
        End Select
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        End If
        If txtPurchaseType.Focused Then
            If e.KeyCode = Keys.Down Then dgPurchaseType.Focus()
        End If
        If txtStoreName.Focused Then
            If e.KeyCode = Keys.Down Then dgStore.Focus()
        End If
        If txtitem.Focused Then
            If e.KeyCode = Keys.Down Then dgItemSearch.Focus()
        End If
        If txtLot.Focused Then
            If e.KeyCode = Keys.Down Then dgLot.Focus()
        End If
        If txtLot.Focused Then
            If e.KeyCode = Keys.Down Then dgLot.Focus()
        End If
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        ' CustomerFill()
        PurchaseTypeColumns()
        DgAccountSearch.Visible = False
        If pnlPurchaseList.Visible = True Then txtPurchaseSearch.Focus() Else txtPurchaseType.Focus()
    End Sub

    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            ' CustomerFill()
            PurchaseTypeColumns()
            DgAccountSearch.Visible = False
            If pnlPurchaseList.Visible = True Then txtPurchaseSearch.Focus() Else txtPurchaseType.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub dgPurchaseType_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgPurchaseType.CellClick
        txtPurchaseType.Clear()
        txtPurchaseTypeID.Clear()
        txtPurchaseTypeID.Text = dgPurchaseType.SelectedRows(0).Cells(0).Value
        txtPurchaseType.Text = dgPurchaseType.SelectedRows(0).Cells(1).Value
        dgPurchaseType.Visible = False
        StoreColums()
        txtStoreName.Focus()
    End Sub
    Private Sub dgPurchaseType_KeyDown(sender As Object, e As KeyEventArgs) Handles dgPurchaseType.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtPurchaseType.Clear()
            txtPurchaseTypeID.Clear()
            txtPurchaseTypeID.Text = dgPurchaseType.SelectedRows(0).Cells(0).Value
            txtPurchaseType.Text = dgPurchaseType.SelectedRows(0).Cells(1).Value
            StoreColums()
            dgPurchaseType.Visible = False
            txtStoreName.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtPurchaseType.Focus()
    End Sub
    Private Sub txtStoreName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtStoreName.KeyPress
        dgStore.BringToFront()
        StoreColums()
        dgStore.Visible = True
    End Sub
    Private Sub txtStoreName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtStoreName.KeyUp
        'StoreColums()
        If txtStoreName.Text.Trim() <> "" Then
            '   dgStore.Visible = True
            RetriveMode(" and upper(StorageName) Like upper('%" & txtStoreName.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then
            If dgStore.Visible = False Then dgStore.Visible = False
        End If
    End Sub
    Private Sub dgMode_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgStore.CellClick
        txtStoreName.Clear() : txtStorageID.Clear()
        txtStorageID.Text = dgStore.SelectedRows(0).Cells(0).Value
        txtStoreName.Text = dgStore.SelectedRows(0).Cells(1).Value
        dgStore.Visible = False
        txtitem.Focus()
    End Sub

    Private Sub dgMode_KeyDown(sender As Object, e As KeyEventArgs) Handles dgStore.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtStoreName.Clear() : txtStorageID.Clear()
            txtStorageID.Text = dgStore.SelectedRows(0).Cells(0).Value
            txtStoreName.Text = dgStore.SelectedRows(0).Cells(1).Value
            dgStore.Visible = False
            txtitem.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtStoreName.Focus()
    End Sub
    Private Sub txtItemStockIn_KeyUp(sender As Object, e As KeyEventArgs) Handles txtitem.KeyUp
        If dgItemSearch.Rows.Count = 0 Then Exit Sub
        If txtitem.Text.Trim() <> "" Then
            dgItemSearch.Visible = True
            StockINretriveItems(" And upper(ItemName) Like upper('" & txtitem.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then
            If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        End If
    End Sub

    Private Sub txtItemStockIn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtitem.KeyPress
        StockInItemRowColums()
        dgItemSearch.Visible = True
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtitem.Clear()
        txtItemID.Clear()
        txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
        txtitem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        txtitem.TextAlign = HorizontalAlignment.Left
        dgItemSearch.Visible = False : txtLot.Focus()
        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    End Sub
    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
            txtitem.Clear()
            txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
            txtitem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            txtitem.TextAlign = HorizontalAlignment.Left
            lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
            ' ItemFill()
            '  clsFun.FillDropDownList(cbLot, "Select  LotNo From Purchase where ItemID=" & txtItemID.Text & " and StockHolderId=" & txtAccountID.Text & " group by  LotNo", "LotNo", "", "")
            dgItemSearch.Visible = False
            e.SuppressKeyPress = True
            txtLot.Focus()
        End If
        If e.KeyCode = Keys.Back Then txtitem.Focus()
    End Sub
    Private Sub ItemBalance()
        lblItemBalance.Visible = True
        Dim PurchaseBal As String = "" : Dim StockBal As String = ""
        Dim RestBal As String = "" : Dim tmpbal As String = ""
        Dim dt As New DataTable
        PurchaseBal = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        StockBal = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and TransType  in ('Stock Sale','On Sale','Standard Sale','Store Out') and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        RestBal = Val(PurchaseBal) - Val(StockBal)
        If BtnSave.Text = "&Save" Then
            If dg1.SelectedRows.Count = 0 Then
                If Val(StockBal) = 0 Then ' if no record inserted
                    If dg1.RowCount = 0 Then ' if no rows addred
                        bal = (RestBal)
                    Else 'if rows count
                        For i As Integer = 0 To dg1.RowCount - 1
                            If Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
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
                            If dg1.Rows(i).Cells(4).Value = Val(txtItemID.Text) Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                            End If
                        Next i
                        bal = Val(RestBal) - Val(tmpbal)
                    End If
                End If
            Else
                If Val(StockBal) = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpbal = Val(PurchaseBal) - Val(tmpbal)
                    tmpbal = Val(tmpbal) + Val(dg1.SelectedRows(0).Cells(7).Value)
                    bal = Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpbal = Val(RestBal) - Val(tmpbal)
                    tmpbal = (tmpbal) + Val(dg1.SelectedRows(0).Cells(7).Value)
                    bal = Val(tmpbal)
                End If
            End If
        Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
            If dg1.RowCount = 0 Then ' if no rows addred
                bal = (RestBal)
            Else 'if rows count
                UpdateTmp = clsFun.ExecScalarInt("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " AND VoucherID not in ('" & Val(txtid.Text) & "') and TransType  in ('Stock Sale','On Sale','Standard Sale','Store Out') and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                If dg1.SelectedRows.Count = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpbal = Val(UpdateTmp) + Val(tmpbal)
                    bal = Val(PurchaseBal) - Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpbal = Val(UpdateTmp) + Val(tmpbal)
                    If Val(dg1.SelectedRows(0).Cells(4).Value) = Val(txtItemID.Text) Then
                        '- Val(dg1.SelectedRows(0).Cells(5).Value)
                        tmpbal = Val(tmpbal) - Val(dg1.SelectedRows(0).Cells(7).Value)
                    End If
                    ' If (StockBal) = 0 Then

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
    Private Sub ItemBalance1()
        lblItemBalance.Visible = True
        Dim PurchaseBal As String = "" : Dim StockBal As String = ""
        Dim RestBal As String = "" : Dim tmpbal As String = ""
        Dim dt As New DataTable
        PurchaseBal = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & "")
        StockBal = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and TransType  in ('Stock Sale','On Sale','Standard Sale')")
        RestBal = Val(PurchaseBal) - Val(StockBal)
        If BtnSave.Text = "&Save" Then
            If dg1.SelectedRows.Count = 0 Then
                If Val(StockBal) = 0 Then ' if no record inserted
                    If dg1.RowCount = 0 Then ' if no rows addred
                        bal = (RestBal)
                    Else 'if rows count
                        For i As Integer = 0 To dg1.RowCount - 1
                            If dg1.Rows(i).Cells(4).Value = txtItemID.Text Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
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
                            If dg1.Rows(i).Cells(4).Value = txtItemID.Text Then
                                tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                            End If
                        Next i
                        bal = Val(RestBal) - Val(tmpbal)
                    End If
                End If
            Else
                If Val(StockBal) = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(4).Value = txtItemID.Text Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpbal = Val(PurchaseBal) - Val(tmpbal)
                    tmpbal = Val(tmpbal) + Val(dg1.SelectedRows(0).Cells(7).Value)
                    bal = Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(0).Value = txtItemID.Text Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpbal = Val(RestBal) - Val(tmpbal)
                    tmpbal = (tmpbal) + Val(dg1.SelectedRows(0).Cells(7).Value)
                    bal = Val(tmpbal)
                End If
            End If
        Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
            If dg1.RowCount = 0 Then ' if no rows addred
                bal = (RestBal)
            Else 'if rows count
                UpdateTmp = clsFun.ExecScalarInt("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " AND VoucherID not in ('" & Val(txtid.Text) & "') and TransType  in ('Stock Sale','On Sale','Standard Sale')")
                If dg1.SelectedRows.Count = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(4).Value = Val(txtItemID.Text) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpbal = Val(UpdateTmp) + Val(tmpbal)
                    bal = Val(PurchaseBal) - Val(tmpbal)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(4).Value = Val(txtItemID.Text) Then
                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    ' If (StockBal) = 0 Then
                    tmpbal = Val(UpdateTmp) + Val(tmpbal) '- Val(dg1.SelectedRows(0).Cells(5).Value)
                    tmpbal = Val(tmpbal) - Val(dg1.SelectedRows(0).Cells(7).Value)
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
    Private Sub LotBalance()
        lblLot.Visible = True
        Dim PurchaseLot As String = ""
        Dim StockLot As String = ""
        Dim RestLot As String = ""
        Dim tmpLot As String = ""
        Dim UpdatetmpLot As String = ""
        Dim tmpbal As String = ""
        PurchaseLot = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and LotNo='" & txtLot.Text & "' and VoucherID= '" & (txtPurchaseID.Text) & "' ")
        '        PurchaseLot = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and LotNo='" & txtLot.Text & "' and VoucherID='" & txtPurchaseID.Text & "'")
        StockLot = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and Lot='" & txtLot.Text & "' and PurchaseID= '" & (txtPurchaseID.Text) & "'  and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out')")
        RestLot = Val(PurchaseLot) - Val(StockLot)
        If BtnSave.Text = "&Save" Then
            If dg1.SelectedRows.Count = 0 Then
                If Val(StockLot) = 0 Then ' if no record inserted
                    If dg1.RowCount = 0 Then ' if no rows addred
                        LotBal = (StockLot)
                    Else 'if rows count
                        For i As Integer = 0 To dg1.RowCount - 1
                            If dg1.Rows(i).Cells(6).Value = txtLot.Text And Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(12).Value) = Val(txtPurchaseID.Text) Then
                                tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(7).Value)
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
                            If dg1.Rows(i).Cells(6).Value = txtLot.Text And Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(12).Value) = Val(txtPurchaseID.Text) Then
                                tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(7).Value)
                            End If
                        Next i
                        LotBal = Val(RestLot) - Val(tmpLot)
                    End If
                End If
            Else
                If Val(StockLot) = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(6).Value = txtLot.Text And Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(12).Value) = Val(txtPurchaseID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpLot = Val(PurchaseLot) - Val(tmpLot)
                    tmpLot = Val(tmpLot) + Val(dg1.SelectedRows(0).Cells(7).Value)
                    LotBal = Val(tmpLot)
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(6).Value = txtLot.Text And Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(12).Value) = Val(txtPurchaseID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    tmpLot = Val(RestLot) - Val(tmpLot)
                    tmpLot = (tmpLot) + Val(dg1.SelectedRows(0).Cells(7).Value)
                    LotBal = Val(tmpLot)
                End If
            End If
        Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
            If dg1.RowCount = 0 Then ' if no rows addred
                LotBal = (RestLot)
            Else 'if rows count
                UpdatetmpLot = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " " &
                                                    "AND VoucherID  in ('" & Val(txtid.Text) & "') and Lot='" & txtLot.Text & "' and PurchaseID='" & Val(txtPurchaseID.Text) & "' and StorageID=" & Val(txtStorageID.Text) & " " &
                                                    " and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out')")
                Dim UpdatedLot As String = Val(clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where  ItemID = " & Val(txtItemID.Text) & " and Lot='" & txtLot.Text & "' " &
                                                                    "and VoucherID<>'" & Val(txtid.Text) & "' and PurchaseID='" & Val(txtPurchaseID.Text) & "' "))
                If dg1.SelectedRows.Count = 0 Then
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(6).Value = txtLot.Text And Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(12).Value) = Val(txtPurchaseID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(7).Value)
                        End If
                    Next i
                    LotBal = Val(PurchaseLot) - Val(Val(tmpLot) + Val(UpdatedLot))
                Else
                    For i As Integer = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(6).Value = txtLot.Text And Val(dg1.Rows(i).Cells(4).Value) = Val(txtItemID.Text) And Val(dg1.Rows(i).Cells(12).Value) = Val(txtPurchaseID.Text) Then
                            tmpLot = Val(tmpLot) + Val(dg1.Rows(i).Cells(7).Value)
                            'Else
                            '   MsgBox("Please Choose Selected Lot Only", MsgBoxStyle.Critical, "Check Lot") : txtLot.Focus() : Exit Sub
                        End If
                    Next i
                    ' If (StockBal) = 0 Then
                    'tmpLot = Val(UpdatetmpLot) + Val(Val(tmpLot) - Val(dg1.SelectedRows(0).Cells(5).Value))
                    If dg1.SelectedRows(0).Cells(6).Value = txtLot.Text And dg1.SelectedRows(0).Cells(4).Value = txtItemID.Text And Val(dg1.SelectedRows(0).Cells(12).Value) = Val(txtPurchaseID.Text) Then
                        tmpLot = Val(Val(tmpLot) - Val(dg1.SelectedRows(0).Cells(7).Value))
                        LotBal = Val(PurchaseLot) - Val(Val(tmpLot) + Val(UpdatedLot))
                    Else
                        LotBal = RestLot
                    End If


                    ' tmpLot = Val(tmpLot) - dg1.SelectedRows(0).Cells(5).Value
                    'LotBal = Val(PurchaseLot) - Val(tmpLot)
                End If
            End If
        End If
        If dg1.SelectedRows.Count = 0 Then
            lblLot.Text = "Lot Balance : " & Val(LotBal)
        Else
            lblLot.Text = "Lot Balance : " & Val(LotBal) & " (Selected Nugs Not Counting)"
        End If
    End Sub
    Private Sub txtAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAmount.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(0).Value = txtPurchaseTypeID.Text
                dg1.SelectedRows(0).Cells(1).Value = txtPurchaseType.Text
                dg1.SelectedRows(0).Cells(2).Value = txtStorageID.Text
                dg1.SelectedRows(0).Cells(3).Value = txtStoreName.Text
                dg1.SelectedRows(0).Cells(4).Value = Val(txtItemID.Text)
                dg1.SelectedRows(0).Cells(5).Value = txtitem.Text
                dg1.SelectedRows(0).Cells(6).Value = txtLot.Text
                dg1.SelectedRows(0).Cells(7).Value = Format(Val(txtNug.Text), "0.00")
                dg1.SelectedRows(0).Cells(8).Value = Format(Val(txtWeight.Text), "0.00")
                dg1.SelectedRows(0).Cells(9).Value = Format(Val(txtRate.Text), "0.00")
                dg1.SelectedRows(0).Cells(10).Value = CbPer.Text
                dg1.SelectedRows(0).Cells(11).Value = Format(Val(txtAmount.Text), "0.00")
                dg1.SelectedRows(0).Cells(12).Value = Val(txtPurchaseID.Text)
                dg1.SelectedRows(0).Cells(13).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(14).Value = Val(cbCrateMarka.SelectedValue)
                dg1.SelectedRows(0).Cells(15).Value = cbCrateMarka.Text
                dg1.SelectedRows(0).Cells(16).Value = Val(txtCrateQty.Text)
                txtclear()

            Else
                dg1.Rows.Add(Val(txtPurchaseTypeID.Text), txtPurchaseType.Text, txtStorageID.Text, txtStoreName.Text, txtItemID.Text, txtitem.Text,
                             txtLot.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtWeight.Text), "0.00"), Format(Val(txtRate.Text), "0.00"),
                             CbPer.Text, Format(Val(txtAmount.Text), "0.00"), txtPurchaseID.Text, lblCrate.Text, Val(cbCrateMarka.SelectedValue),
                             cbCrateMarka.Text, Val(txtCrateQty.Text))
                txtclear()

            End If
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub dg2Record()
        Dim sql As String = String.Empty
        Dim FastQuery As String = String.Empty

        For Each row As DataGridViewRow In Dg2.Rows
            Application.DoEvents()
            With row
                If .Cells("Charge Name").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtid.Text) & "," & _
                        "'" & .Cells("ChargeID").Value & "','" & .Cells("Charge Name").Value & "','" & .Cells("On").Value & "'," & _
                        "'" & .Cells("Cal").Value & "','" & .Cells("+/-").Value & "','" & .Cells("Amount").Value & "'"

                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into ChargesTrans(VoucherID, ChargesID, ChargeName, OnValue, Calculate, ChargeType, Amount) " & FastQuery & ""
            clsFun.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub Save()
        'VNumber()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        dg1.ClearSelection()
        Dim cmd As SQLite.SQLiteCommand
        sql = "insert into Vouchers(TransType,BillNo, Entrydate, " _
                                    & "AccountID,AccountName, Nug, kg,BasicAmount,InvoiceID,VehicleNo,TotalCharges,RoundOff,TotalAmount,T1,T2,T3)" _
                                    & "values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10,@11,@12,@13,@14,@15,@16)"
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
            cmd.Parameters.AddWithValue("@10", txtVehicleNo.Text)
            cmd.Parameters.AddWithValue("@11", Val(txtTotCharges.Text))
            cmd.Parameters.AddWithValue("@12", Val(txtRoundOff.Text))
            cmd.Parameters.AddWithValue("@13", Val(txtTotTotal.Text))
            cmd.Parameters.AddWithValue("@14", txtDriverName.Text)
            cmd.Parameters.AddWithValue("@15", txtMobile.Text)
            cmd.Parameters.AddWithValue("@16", txtRemark.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                clsFun.CloseConnection()
            End If

            txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
            ServerTag = 1
            Dg1Record() : dg2Record() : CrateLedger() : ServerCrateLedger()
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
            txtclearall()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub CrateLedger()
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                ' If Val(.Cells(33).Value) > 0 Then
                If Val(.Cells(16).Value) > 0 Then ''Party Account
                    '     clsFun.CrateLedger(0, VchId, clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, SqliteEntryDate, Me.Text, Val(txtAccountID.Text), txtAccount.Text, "Crate Out", Val(.Cells(14).Value), .Cells(15).Value, .Cells(16).Value, "", "", "", "")
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(.Cells(14).Value) & ",'" & .Cells(15).Value & "','" & .Cells(16).Value & "', '','','',''"
                End If
                ' End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastCrateLedger(FastQuery)
    End Sub

    Private Sub ServerCrateLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                ' If Val(.Cells(33).Value) > 0 Then
                If Val(.Cells(16).Value) > 0 Then ''Party Account
                    '     clsFun.CrateLedger(0, VchId, clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, SqliteEntryDate, Me.Text, Val(txtAccountID.Text), txtAccount.Text, "Crate Out", Val(.Cells(14).Value), .Cells(15).Value, .Cells(16).Value, "", "", "", "")
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(.Cells(14).Value) & ",'" & .Cells(15).Value & "','" & .Cells(16).Value & "', '','','',''," & Val(ServerTag) & "," & Val(OrgID) & ""
                End If
                ' End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(FastQuery)
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
        Dim dt As DateTime
        Dim tmpamount As Decimal = Val(txtTotbasic.Text)
        Dim tmpamount2 As Decimal = Val(txtTotbasic.Text)
        Dim remarkHindi As String = String.Empty
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        ''Caluclate  net amt
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells(0).Value <> "" Then
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
        RemarkNaration()
        ' If Val(txtTotbasic.Text) > 0 Then ''Manual Beejak Account Fixed
        clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 29, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29"), Val(tmpamount2), "C", "On Sale" & txtVoucherNo.Text, txtAccount.Text, remarkHindi)
        'End If
        If Val(txtTotTotal.Text) > 0 Then ''Account 
            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtRoundOff.Text)), "D", "On Sale" & txtVoucherNo.Text, txtAccount.Text, remarkHindi)
        Else
            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtRoundOff.Text)), "C", "On Sale" & txtVoucherNo.Text, txtAccount.Text, remarkHindi)
        End If
        'If Val(txtRoundOff.Text) <> 0 Then ''Account 
        If Val(txtRoundOff.Text) < 0 Then
            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtRoundOff.Text)), "D", "On Sale" & txtVoucherNo.Text, txtAccount.Text, remarkHindi)
        Else
            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtRoundOff.Text)), "C", "On Sale" & txtVoucherNo.Text, txtAccount.Text, remarkHindi)
        End If
        'End If
        clsFun.CloseConnection()
    End Sub
    Private Sub InsertCharges()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim CostON As String = clsFun.ExecScalarStr(" Select CostOn FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ChargeName='" & .Cells("Charge Name").Value & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D")
                        Else
                            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C")
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ChargeName='" & .Cells("Charge Name").Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C")
                        Else
                            clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D")
                        End If
                    End If
                End If
            End With
        Next
        clsFun.CloseConnection()
    End Sub
    Private Sub UpdateCrate()
        Dim fastQuery As String = String.Empty
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from CrateVoucher where VoucherID='" & Val(txtid.Text) & "'")
        Try
            If dt.Rows.Count > 0 Then
                ServerTag = 0
                For i = 0 To dt.Rows.Count - 1
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & dt.Rows(i)("SlipNo").ToString() & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(dt.Rows(i)("AccountID").ToString()) & ",'" & dt.Rows(i)("AccountName").ToString() & "','Crate Out','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "'," & Val(dt.Rows(i)("Qty").ToString()) & ",'', '" & Val(dt.Rows(i)("Rate").ToString()) & "','" & Val(dt.Rows(i)("Amount").ToString()) & "',''," & Val(ServerTag) & ", " & Val(OrgID) & ""
                Next
            End If
            If fastQuery = String.Empty Then Exit Sub
            ClsFunserver.FastCrateLedger(fastQuery)
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub UpdateRecord()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        dg1.ClearSelection()
        ' Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "', Entrydate='" & SqliteEntryDate & "',SallerID='" & Val(txtPurchaseID.Text) & "', " &
              "SallerName='" & txtPurchaseType.Text & "',AccountID='" & Val(txtAccountID.Text) & "',AccountName='" & txtAccount.Text & "', Nug='" & Val(txtTotNug.Text) & "', kg='" & Val(txtTotweight.Text) & "' ," &
              "BasicAmount='" & Val(txtTotbasic.Text) & "',RoundOff='" & Val(txtRoundOff.Text) & "',TotalCharges='" & Val(txtTotCharges.Text) & "',TotalAmount='" & Val(txtTotTotal.Text) & "', " &
              "InvoiceID='" & Val(txtInvoiceID.Text) & "',VehicleNo='" & txtVehicleNo.Text & "',T1='" & txtDriverName.Text & "',T2='" & txtMobile.Text & "',T3='" & txtRemark.Text & "' Where ID='" & Val(txtid.Text) & "'"
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
            End If
            ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                               " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
            UpdateCrate()
            clsFun.ExecNonQuery("DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from Chargestrans WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                "DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ";")
            ServerTag = 1
            Dg1Record() : dg2Record() : CrateLedger() : ServerCrateLedger()
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
            txtclearall()
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
        'btnPrint.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & id
        Dim sql As String = "Select * from Transaction2 where VoucherID=" & id
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds, ds1, ds2 As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds1, "b")
        ad2.Fill(ds2, "c")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = id
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("AccountID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txtTotweight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtTotTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtDriverName.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtMobile.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtRemark.Text = ds.Tables("a").Rows(0)("T3").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
        End If

        If ds1.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds1.Tables("b").Rows.Count - 1
                .Rows.Add()
                dg1.Rows(i).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(12).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Rows(i).Cells(0).Value = ds1.Tables("b").Rows(i)("SallerID").ToString()
                .Rows(i).Cells(1).Value = ds1.Tables("b").Rows(i)("SallerName").ToString()
                .Rows(i).Cells(2).Value = ds1.Tables("b").Rows(i)("StorageID").ToString()
                .Rows(i).Cells(3).Value = ds1.Tables("b").Rows(i)("StorageName").ToString()
                .Rows(i).Cells(4).Value = ds1.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells(5).Value = ds1.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells(6).Value = ds1.Tables("b").Rows(i)("Lot").ToString()
                .Rows(i).Cells(7).Value = Format(Val(ds1.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells(8).Value = Format(Val(ds1.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells(9).Value = Format(Val(ds1.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells(10).Value = ds1.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells(11).Value = Format(Val(ds1.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells(12).Value = Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString())
                .Rows(i).Cells(13).Value = ds1.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells(14).Value = Val(ds1.Tables("b").Rows(i)("CrateID").ToString())
                .Rows(i).Cells(15).Value = ds1.Tables("b").Rows(i)("CrateMarka").ToString()
                .Rows(i).Cells(16).Value = Val(ds1.Tables("b").Rows(i)("CrateQty").ToString())
            Next
        End With
        If ds2.Tables("c").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                If ds2.Tables("C").Rows.Count > 5 Then Dg2.Columns(4).Width = 129 Else Dg2.Columns(4).Width = 149
                For i = 0 To ds2.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    Dg2.Rows(i).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    Dg2.Rows(i).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    Dg2.Rows(i).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Dg2.Rows(i).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Rows(i).Cells("Charge Name").Value = ds2.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On").Value = Format(Val(ds2.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds2.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds2.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds2.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds2.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        dg1.ClearSelection() : Dg2.ClearSelection() : calc()
    End Sub

    Public Sub FillWithNevigation()
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnDelete.Enabled = True
        'btnPrint.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()

        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'On Sale' Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from  Vouchers WHERE transtype = 'On Sale' Order By ID LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString() + ""
        ' sSql = "Select * from Vouchers where id=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds, ds1, ds2 As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            '  txtid.Text = id
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("AccountID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txtTotweight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtTotTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtDriverName.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtMobile.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtRemark.Text = ds.Tables("a").Rows(0)("T3").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
            txtid.Text = Val(ds.Tables("a").Rows(0)("ID").ToString())
        End If
        Dim sql As String = "Select * from Transaction2 where VoucherID=" & Val(txtid.Text)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        ad1.Fill(ds1, "b")
        If ds1.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds1.Tables("b").Rows.Count - 1
                .Rows.Add()
                dg1.Rows(i).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                dg1.Rows(i).Cells(12).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Rows(i).Cells(0).Value = ds1.Tables("b").Rows(i)("SallerID").ToString()
                .Rows(i).Cells(1).Value = ds1.Tables("b").Rows(i)("SallerName").ToString()
                .Rows(i).Cells(2).Value = ds1.Tables("b").Rows(i)("StorageID").ToString()
                .Rows(i).Cells(3).Value = ds1.Tables("b").Rows(i)("StorageName").ToString()
                .Rows(i).Cells(4).Value = ds1.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells(5).Value = ds1.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells(6).Value = ds1.Tables("b").Rows(i)("Lot").ToString()
                .Rows(i).Cells(7).Value = Format(Val(ds1.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells(8).Value = Format(Val(ds1.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells(9).Value = Format(Val(ds1.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells(10).Value = ds1.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells(11).Value = Format(Val(ds1.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells(12).Value = Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString())
                .Rows(i).Cells(13).Value = ds1.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells(14).Value = Val(ds1.Tables("b").Rows(i)("CrateID").ToString())
                .Rows(i).Cells(15).Value = ds1.Tables("b").Rows(i)("CrateMarka").ToString()
                .Rows(i).Cells(16).Value = Val(ds1.Tables("b").Rows(i)("CrateQty").ToString())
            Next
        End With
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & Val(txtid.Text)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        ad2.Fill(ds2, "c")
        If ds2.Tables("c").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                If ds2.Tables("C").Rows.Count > 5 Then Dg2.Columns(4).Width = 129 Else Dg2.Columns(4).Width = 149
                For i = 0 To ds2.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    Dg2.Rows(i).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    Dg2.Rows(i).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    Dg2.Rows(i).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Dg2.Rows(i).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Rows(i).Cells("Charge Name").Value = ds2.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On").Value = Format(Val(ds2.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds2.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds2.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds2.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds2.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        dg1.ClearSelection() : Dg2.ClearSelection() : calc()
    End Sub
    Private Sub Dg1Record()
        Dim sql As String = String.Empty
        Dim FastQuery As String = String.Empty
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                Dim typeac As String = IIf(Val(.Cells(0).Value) = Val(28), "Purchase", "Stock in")
                If .Cells(1).Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & txtVoucherNo.Text & "'," & Val(txtid.Text) & ", '" & Me.Text & "'," &
                             "'" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "'," & Val(.Cells(0).Value) & "," &
                             "'" & .Cells(1).Value & "','" & Val(.Cells(2).Value) & "', " &
                             " '" & .Cells(3).Value & "','" & Val(.Cells(4).Value) & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "'," &
                             " '" & Val(.Cells(7).Value) & "','" & Val(.Cells(8).Value) & "','" & Val(.Cells(9).Value) & "','" & Val(.Cells(9).Value) & "'," &
                             "'" & .Cells(10).Value & "','" & Val(.Cells(11).Value) & "','" & Val(.Cells(11).Value) & "','" & Val(.Cells(12).Value) & "','" & typeac & "', " &
                            " '" & .Cells(13).Value & "','" & Val(.Cells(14).Value) & "','" & .Cells(15).Value & "','" & Val(.Cells(16).Value) & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into Transaction2(EntryDate,BillNo,VoucherID,TransType,AccountID,AccountName,SallerID,SallerName,StorageID,StorageName,ItemID,ItemName,Lot,Nug,Weight, " _
                   & " Rate,SRate, Per,Amount,SallerAmt,PurchaseID,PurchaseTypename,MaintainCrate,CrateID,Cratemarka,CrateQty) " & FastQuery & ""
            clsFun.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        clsFun.CloseConnection()
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotbasic.Text = Format(0, "0.00") : txtTotTotal.Text = Format(0, "0.00")
        txtRoundOff.Text = Format(0, "0.00") : txtTotCharges.Text = Format(0, "0.00")
        Dim i As Integer
        If Dg2.RowCount >= 5 Then Dg2.Columns(4).Width = 129 Else Dg2.Columns(4).Width = 149
        For i = 0 To dg1.Rows.Count - 1
            dg1.Rows(i).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            dg1.Rows(i).Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(12).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotbasic.Text = Format(Val(txtTotbasic.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
        Next
        txtTotCharges.Text = Format(0, "0.00")
        For i = 0 To Dg2.Rows.Count - 1
            Dg2.Rows(i).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            Dg2.Rows(i).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            Dg2.Rows(i).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Dg2.Rows(i).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight

            If Dg2.Rows(i).Cells(3).Value = "-" Then
                txtTotCharges.Text = Format(Val(txtTotCharges.Text) - Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            Else
                txtTotCharges.Text = Format(Val(txtTotCharges.Text) + Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            End If
        Next
        txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        Dim tmpamount As Double = CDbl(Val(txtTotTotal.Text))
        txtTotTotal.Text = Math.Round(Val(tmpamount), 0)
        txtRoundOff.Text = Format(Val(txtTotTotal.Text) - Val(tmpamount), "0.00")
        txtTotTotal.Text = Format(Val(txtTotTotal.Text), "0.00")
    End Sub


    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave, txtWeight.Leave,
        txtAmount.Leave, txtRate.Leave, CbPer.Leave, CbPer.SelectedIndexChanged
        Calculation()
    End Sub

    Private Sub txtCustomer_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub txtaccountName_TextChanged(sender As Object, e As EventArgs) Handles txtPurchaseType.TextChanged

    End Sub

    Private Sub txtStoreName_TextChanged(sender As Object, e As EventArgs) Handles txtStoreName.TextChanged

    End Sub

    Private Sub dgLot_KeyDown(sender As Object, e As KeyEventArgs) Handles dgLot.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtLot.Clear()
            txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
            '  txtAccountID.Text = dgLot.SelectedRows(0).Cells(0).Value
            dgLot.Visible = False
            txtNug.Focus()
        End If
        If e.KeyCode = Keys.Back Then txtLot.Focus()
    End Sub

    Private Sub txtitem_TextChanged(sender As Object, e As EventArgs) Handles txtitem.TextChanged

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtPurchaseTypeID.Text = dg1.SelectedRows(0).Cells(0).Value
        txtPurchaseType.Text = dg1.SelectedRows(0).Cells(1).Value
        txtStorageID.Text = dg1.SelectedRows(0).Cells(2).Value
        txtStoreName.Text = dg1.SelectedRows(0).Cells(3).Value
        txtItemID.Text = Val(dg1.SelectedRows(0).Cells(4).Value)
        txtitem.Text = dg1.SelectedRows(0).Cells(5).Value
        txtLot.Text = dg1.SelectedRows(0).Cells(6).Value
        txtNug.Text = Format(Val(dg1.SelectedRows(0).Cells(7).Value), "0.00")
        txtWeight.Text = Format(Val(dg1.SelectedRows(0).Cells(8).Value), "0.00")
        txtRate.Text = Format(Val(dg1.SelectedRows(0).Cells(9).Value), "0.00")
        CbPer.Text = dg1.SelectedRows(0).Cells(10).Value
        txtAmount.Text = Format(Val(dg1.SelectedRows(0).Cells(11).Value), "0.00")
        txtPurchaseID.Text = Val(dg1.SelectedRows(0).Cells(12).Value)
        lblCrate.Text = dg1.SelectedRows(0).Cells(13).Value
        cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(14).Value)
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(15).Value
        txtCrateQty.Text = Val(dg1.SelectedRows(0).Cells(16).Value)
        '   txtPurchaseType.Focus()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            txtPurchaseTypeID.Text = dg1.SelectedRows(0).Cells(0).Value
            txtPurchaseType.Text = dg1.SelectedRows(0).Cells(1).Value
            txtStorageID.Text = dg1.SelectedRows(0).Cells(2).Value
            txtStoreName.Text = dg1.SelectedRows(0).Cells(3).Value
            txtItemID.Text = Val(dg1.SelectedRows(0).Cells(4).Value)
            txtitem.Text = dg1.SelectedRows(0).Cells(5).Value
            txtLot.Text = dg1.SelectedRows(0).Cells(6).Value
            txtNug.Text = Format(Val(dg1.SelectedRows(0).Cells(7).Value), "0.00")
            txtWeight.Text = Format(Val(dg1.SelectedRows(0).Cells(8).Value), "0.00")
            txtRate.Text = Format(Val(dg1.SelectedRows(0).Cells(9).Value), "0.00")
            CbPer.Text = dg1.SelectedRows(0).Cells(10).Value
            txtAmount.Text = Format(Val(dg1.SelectedRows(0).Cells(11).Value), "0.00")
            txtPurchaseID.Text = Val(dg1.SelectedRows(0).Cells(12).Value)
            cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(14).Value)
            cbCrateMarka.Text = dg1.SelectedRows(0).Cells(15).Value
            txtCrateQty.Text = Val(dg1.SelectedRows(0).Cells(16).Value)
            txtPurchaseType.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to Remove Item", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
            End If
        End If
    End Sub

    Private Sub ChargesRowColums()
        dgCharges.ColumnCount = 3
        dgCharges.Columns(0).Name = "ID" : dgCharges.Columns(0).Visible = False
        dgCharges.Columns(1).Name = "Item Name" : dgCharges.Columns(1).Width = 130
        dgCharges.Columns(2).Name = "ApplyType" : dgCharges.Columns(2).Width = 142
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
    Private Sub ChargesCalculation()
        ' If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
        If CalcType = "Percentage" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text) / 100, "0.00")
        ElseIf CalcType = "Nug" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Weight" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Aboslute" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "0.00")
        End If
        'If String.IsNullOrEmpty(txtbasicTotal.Text) OrElse String.IsNullOrEmpty(txttotalCharges.Text) Then Exit Sub
        'txtsallerTotal.Text = Format(CDbl(txtSallerBasicTotal.Text) + CDbl(txtSallerCharges.Text), "0.00")
    End Sub
    Private Sub FillCharges()
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        If CalcType = "Aboslute" Then
            txtOnValue.TabStop = False
            txtOnValue.Text = ""
            txtCalculatePer.Text = ""
            txtCalculatePer.TabStop = False
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
        ElseIf CalcType = "Crate" Then
            txtOnValue.Text = Val(txtTotNug.Text)
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
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

    Private Sub txtCharges_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCharges.KeyPress
        ChargesRowColums()
        dgCharges.Visible = True
    End Sub
    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs) Handles txtCharges.GotFocus
        dgPurchaseType.Visible = False : dgItemSearch.Visible = False : dgLot.Visible = False
        dgStore.Visible = False : DgAccountSearch.Visible = False : dgCharges.Visible = True
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If txtCharges.Text.Trim() <> "" Then
            'dgCharges.Visible = True
            RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
        Else
            RetriveCharges()
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
    Private Sub Dg2_KeyDown(sender As Object, e As KeyEventArgs) Handles Dg2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            txtCharges.Text = Dg2.SelectedRows(0).Cells(0).Value
            txtOnValue.Text = Dg2.SelectedRows(0).Cells(1).Value
            txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(2).Value
            txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(3).Value
            txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(4).Value
            txtChargeID.Text = Dg2.SelectedRows(0).Cells(4).Value
            txtCharges.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
            End If
        End If
        calc()
        If String.IsNullOrEmpty(txtTotbasic.Text) OrElse String.IsNullOrEmpty(txtTotCharges.Text) Then Exit Sub
        txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
    End Sub
    Private Sub CbCharges_KeyDown(sender As Object, e As KeyEventArgs) Handles txtOnValue.KeyDown, txtCalculatePer.KeyDown, txtPlusMinus.KeyDown, txtCharges.KeyDown

        If dgCharges.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or CbPer.Focused = True Or cbCrateMarka.Focused = True Or txtCrateQty.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If

        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
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
                If e.KeyCode = Keys.F1 Then
                    If dgCharges.SelectedRows.Count = 0 Then Exit Sub
                    ChargesForm.MdiParent = MainScreenForm
                    Dim tmpMarkaID As String = dgCharges.SelectedRows(0).Cells(0).Value
                    ChargesForm.Show()
                    ChargesForm.FillContros(tmpMarkaID)
                    If Not ChargesForm Is Nothing Then
                        ChargesForm.BringToFront()
                    End If
                End If
            End If
            If e.KeyCode = Keys.Down Then dgCharges.Focus()
        End If
    End Sub

    Private Sub Dg2_MouseClick(sender As Object, e As MouseEventArgs) Handles Dg2.MouseClick
        Dg2.ClearSelection()
    End Sub
    Private Sub Dg2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Dg2.MouseDoubleClick
        If Dg2.SelectedRows.Count = 0 Then Exit Sub
        txtCharges.Text = Dg2.SelectedRows(0).Cells(0).Value
        txtOnValue.Text = Dg2.SelectedRows(0).Cells(1).Value
        txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(2).Value
        txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(3).Value
        txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(4).Value
        txtChargeID.Text = Dg2.SelectedRows(0).Cells(4).Value
        'txtCharges.Focus()
    End Sub
    Private Sub ChargesClear()
        txtCharges.Clear() : txtChargeID.Clear()
        txtOnValue.Clear() : txtCalculatePer.Clear()
        txtPlusMinus.Clear() : txtchargesAmount.Clear()
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
    Private Sub txtchargesAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtchargesAmount.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dg2.SelectedRows.Count = 1 Then
                Dg2.SelectedRows(0).Cells(0).Value = txtCharges.Text
                Dg2.SelectedRows(0).Cells(1).Value = txtOnValue.Text
                Dg2.SelectedRows(0).Cells(2).Value = txtCalculatePer.Text
                Dg2.SelectedRows(0).Cells(3).Value = txtPlusMinus.Text
                Dg2.SelectedRows(0).Cells(4).Value = txtchargesAmount.Text
                Dg2.SelectedRows(0).Cells(5).Value = txtChargeID.Text
                txtCharges.Focus()
                Dg2.ClearSelection()
                calc()
                ChargesClear() 'cleartxtCharges()
            Else
                Dg2.Rows.Add(txtCharges.Text, txtOnValue.Text, txtCalculatePer.Text, txtPlusMinus.Text, txtchargesAmount.Text, txtChargeID.Text)
                ' cleartxtCharges()
                txtCharges.Focus()
                Dg2.ClearSelection()
                calc()
                ChargesClear() '
            End If
        End If
    End Sub

    Private Sub txtchargesAmount_TextChanged(sender As Object, e As EventArgs) Handles txtchargesAmount.TextChanged

    End Sub

    Private Sub txtOnValue_TextChanged(sender As Object, e As EventArgs) Handles txtOnValue.TextChanged, txtchargesAmount.TextChanged, txtCalculatePer.TextChanged
        ChargesCalculation()
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
        End If
        ChargesClear() : dg1.Rows.Clear() : Dg2.Rows.Clear() : BtnDelete.Enabled = False
    End Sub

    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(3).Value <> "" Then
                    sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " &
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                        " P21,P22,P23,P24,P25,P27,P28)" &
                        "  values('" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "', " &
                                "'" & .Cells(25).Value & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & lblInword.Text & "','" & .Cells(29).Value & "','" & .Cells(30).Value & "')"
                    Try
                        cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
                        ClsFunPrimary.ExecNonQuery(sql)
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        ClsFunPrimary.CloseConnection()
                    End Try
                End If
            End With
        Next
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If txtid.Text = "" Then
            If dg1.RowCount = 0 Then MsgBox("No Record to Save Bill...", MsgBoxStyle.Critical, "No Record") : Exit Sub
            MsgBox("If you want to Print. Save First Record.", vbOKOnly, "Save First")
            Dim res = MessageBox.Show("Do you want to Save Bill", "Save First?", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = Windows.Forms.DialogResult.Yes Then
                BtnSave.PerformClick()
            End If
        Else
            If Val(txtid.Text) > 0 Then retrivePrint()
            pnlWhatsapp.Visible = True : txtWhatsappNo.Focus() : Exit Sub
        End If
        txtid.Text = ""
    End Sub

    Private Sub txtInvoiceID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceID.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlInvoiceID.Visible = False
            txtVehicleNo.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub



    Private Sub delete()
        ButtonControl()
        If clsFun.ExecScalarInt("Select count(*) from Transaction1 where PurchaseID=" & Val(txtid.Text) & "") <> 0 Then
            MsgBox("Receipts Already Used in Transactions", vbOKOnly, "Access Denied")
            ButtonControl()
            Exit Sub
        End If
        If MessageBox.Show(" are you Sure want to Delete On Sale Entry... ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "; " &
                                   "DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "; " &
                                   "DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
                ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                   "Delete From  CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 0 : ServerCrateLedger()
                MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                txtclearall()
            End If
        End If
        ButtonControl() : BtnDelete.Enabled = False
    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        delete()
    End Sub



    Private Sub pnlPurchaseList_Paint(sender As Object, e As PaintEventArgs) Handles pnlPurchaseList.Paint

    End Sub
    Private Sub dgPurchase_KeyDown(sender As Object, e As KeyEventArgs) Handles dgPurchase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dgPurchase.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dgPurchase.SelectedRows(0).Cells(0).Value
            FillPurchase(tmpID) : txtPurchaseType.Focus() : pnlPurchaseList.Visible = False
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtPurchaseSearch.Focus()
    End Sub
    Public Sub FillPurchase(ByVal id As Integer)
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & id
        Dim sql As String = "Select * from Purchase where VoucherID=" & id
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds, "b")
        ad2.Fill(ds, "c")
        If ds.Tables("a").Rows.Count > 0 Then
            txtTotNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txtTotweight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtTotbasic.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txtTotCharges.Text = Format(Val(ds.Tables("a").Rows(0)("DiscountAmount").ToString()), "0.00")
            txtTotTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
        End If
        '  If ds.Tables("b").Rows.Count > 0 Then dgStockIn.Rows.Clear()
        If ds.Tables("b").Rows.Count > 0 Then dgPurchase.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells(0).Value = ds.Tables("b").Rows(0)("StockHolderID").ToString()
                .Rows(i).Cells(1).Value = ds.Tables("b").Rows(0)("StockHolderName").ToString()
                .Rows(i).Cells(2).Value = ds.Tables("b").Rows(i)("StorageID").ToString()
                .Rows(i).Cells(3).Value = ds.Tables("b").Rows(i)("StorageName").ToString()
                .Rows(i).Cells(4).Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells(5).Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells(6).Value = ds.Tables("b").Rows(i)("LotNo").ToString()
                .Rows(i).Cells(7).Value = Format(Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells(8).Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells(9).Value = Format(Val(ds.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells(10).Value = ds.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells(11).Value = Format(Val(ds.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells(12).Value = ds.Tables("a").Rows(0)("ID").ToString()
            Next
        End With
        If ds.Tables("c").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    Dg2.Rows(i).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    Dg2.Rows(i).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    Dg2.Rows(i).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Dg2.Rows(i).Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        dg1.ClearSelection() : Dg2.ClearSelection() : calc()
    End Sub

    Private Sub dgPurchase_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dgPurchase.Click
        If dgPurchase.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dgPurchase.SelectedRows(0).Cells(0).Value
        FillPurchase(tmpID) : txtPurchaseType.Focus() : pnlPurchaseList.Visible = False
    End Sub

    Private Sub txtPurchaseSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPurchaseSearch.KeyDown
        If e.KeyCode = Keys.Enter Then txtPurchaseType.Focus() : pnlPurchaseList.Visible = False
        If e.KeyCode = Keys.Down Then
            dgPurchase.Focus()
            If dgPurchase.Rows.Count = 0 Then Exit Sub
            '  dgPurchase.SelectedRows(0).Cells(0).Value
            dgPurchase.SelectionMode = DataGridViewSelectionMode.FullRowSelect

            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs) Handles txtAmount.TextChanged
        If CbPer.SelectedIndex = 0 Then
            txtRate.Text = Format(Val(txtAmount.Text) / Val(txtNug.Text), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        ElseIf CbPer.SelectedIndex = 1 Then
            txtRate.Text = Format(Val(txtAmount.Text) / Val(txtWeight.Text), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        ElseIf CbPer.SelectedIndex = 2 Then
            txtRate.Text = Format(Val(txtAmount.Text) / 40 * Val(txtWeight.Text), "0.00")
            txtTotTotal.Text = Format(Val(txtTotbasic.Text) + Val(txtTotCharges.Text), "0.00")
        End If
        If txtRate.Text = "NAN" Then txtRate.Text = ""
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PrintRecord()
        Report_Viewer.printReport("\OnSale.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
        pnlWhatsapp.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PrintRecord()
        Report_Viewer.printReport("\OnSale.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
        pnlWhatsapp.Visible = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
        If System.IO.File.Exists(WhatsappFile) = False Then
            MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
            Exit Sub
        End If
        Dim p() As Process
        p = Process.GetProcessesByName("Easy Whatsapp")
        If p.Count = 0 Then
            Dim StartWhatsapp As New System.Diagnostics.Process
            StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            StartWhatsapp.Start()
        End If
        pnlWhatsapp.Visible = True : txtWhatsappNo.Focus()
        If txtWhatsappNo.Text <> "" Then
            SendWhatsappData()
        Else
            MsgBox("Please Enter Valid Whatsapp Contact", MsgBoxStyle.Critical, "Invalid Contact") : txtWhatsappNo.Focus() : Exit Sub
        End If
        pnlWhatsapp.Visible = False
    End Sub
    Private Sub SendWhatsappData()
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")

        'pnlWahtsappNo.Visible = True
        'txtWhatsappNo.Focus()
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        retrivePrint()
        PrintRecord()
        Pdf_Genrate.ExportReport("\OnSale.rpt")
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        pnlWhatsapp.Visible = False
        ChargesClear() : dg1.Rows.Clear() : Dg2.Rows.Clear()
    End Sub

    Private Sub CbPer_LostFocus(sender As Object, e As EventArgs) Handles CbPer.LostFocus
        If Val(txtNug.Text) > Val(LotBal) Then
            MsgBox("Not Enough Nugs..." & vbNewLine & " Please Choose Another Item / Lot ", MsgBoxStyle.Critical, "Zero")
            txtNug.Text = 0 : txtLot.Focus() : Exit Sub
        End If
        If lblCrate.Text = "N" Then pnlMarka.Visible = False : Exit Sub
        txtNug.SelectionStart = 0
        If txtNug.Text = "" Then txtNug.Text = Val(0)
        '  txtCrateQty.Text = txtNug.Text
    End Sub

    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub


    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        FillWithNevigation()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Offset -= RowCount
        If Offset <= 0 Then
            Offset = 0
        End If
        FillWithNevigation()
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim currentPage As Integer = (Offset / RowCount) + 1
        If currentPage <> TotalPages Then
            Offset += RowCount
        End If
        FillWithNevigation()
    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        Offset = (TotalPages - 1) * RowCount
        FillWithNevigation()
    End Sub

    Private Sub txtTotbasic_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotbasic.KeyDown

    End Sub

    Private Sub txtTotTotal_Leave(sender As Object, e As EventArgs) Handles txtTotTotal.Leave

    End Sub
End Class