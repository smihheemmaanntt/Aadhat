Public Class Scrip_Profit_Report

    Private Sub Scrip_Profit_Report_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Scrip_Profit_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : radioboth.Checked = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Min(EntryDate) as entrydate from Purchase ")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from Purchase ")
        If mindate <> "" Then
            txtFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            txtFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            txttoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            txttoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        txtFromDate.Text = SmartDate(txtFromDate.Text) : txttoDate.Text = SmartDate(txttoDate.Text, True, 2)
        rowColums() : ckExpAlso.Checked = True
    End Sub


    Private Sub rowColums()
        dg1.ColumnCount = 15
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "V.No." : dg1.Columns(2).Width = 60
        dg1.Columns(3).Name = "Vehicle" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Seller Name" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Type" : dg1.Columns(5).Width = 90
        dg1.Columns(6).Name = "PNug" : dg1.Columns(6).Width = 70
        dg1.Columns(7).Name = "SNug" : dg1.Columns(7).Width = 70
        dg1.Columns(8).Name = "BNug" : dg1.Columns(8).Width = 70
        dg1.Columns(9).Name = "PWeight" : dg1.Columns(9).Width = 70
        dg1.Columns(10).Name = "SWeight" : dg1.Columns(10).Width = 70
        dg1.Columns(11).Name = "SAmt" : dg1.Columns(11).Width = 80
        dg1.Columns(12).Name = "PAmt" : dg1.Columns(12).Width = 80
        dg1.Columns(13).Name = "P & L" : dg1.Columns(13).Width = 80
        dg1.Columns(14).Name = "BWeight" : dg1.Columns(14).Visible = False
    End Sub

    Private Function DbText(ByVal row As DataRow, ByVal columnName As String) As String
        If row Is Nothing OrElse row.Table.Columns.Contains(columnName) = False OrElse IsDBNull(row(columnName)) Then Return ""
        Return row(columnName).ToString()
    End Function

    Private Function DbVal(ByVal row As DataRow, ByVal columnName As String) As Double
        Return Val(DbText(row, columnName))
    End Function

    Private Function Format2(ByVal value As Double) As String
        Return Format(value, "0.00")
    End Function

    Private Function FirstCsvNumber(ByVal value As String) As Integer
        Return CInt(Val(value))
    End Function

    Private Function CsvToList(ByVal value As String) As System.Collections.Generic.List(Of Integer)
        Dim result As New System.Collections.Generic.List(Of Integer)
        If value Is Nothing OrElse value.Trim() = "" Then Return result

        For Each part As String In value.Split(","c)
            Dim id As Integer = CInt(Val(part))
            If id > 0 AndAlso result.Contains(id) = False Then result.Add(id)
        Next

        Return result
    End Function

    Private Function IdListFromValues(ByVal values As System.Collections.Generic.IEnumerable(Of Integer)) As String
        Dim ids As New System.Collections.Generic.List(Of String)
        For Each id As Integer In values
            If id > 0 AndAlso ids.Contains(id.ToString()) = False Then ids.Add(id.ToString())
        Next

        If ids.Count = 0 Then Return "0"
        Return String.Join(",", ids.ToArray())
    End Function

    Private Function IdListFromRows(ByVal dt As DataTable, ByVal columnName As String) As String
        Dim ids As New System.Collections.Generic.List(Of Integer)
        For Each row As DataRow In dt.Rows
            Dim id As Integer = CInt(Val(DbText(row, columnName)))
            If id > 0 Then ids.Add(id)
        Next

        Return IdListFromValues(ids)
    End Function

    Private Function LoadRowsByKey(ByVal sql As String, ByVal keyColumn As String) As System.Collections.Generic.Dictionary(Of Integer, DataRow)
        Dim result As New System.Collections.Generic.Dictionary(Of Integer, DataRow)
        Dim data As DataTable = clsFun.ExecDataTable(sql)

        For Each row As DataRow In data.Rows
            Dim key As Integer = CInt(Val(DbText(row, keyColumn)))
            If key > 0 AndAlso result.ContainsKey(key) = False Then result.Add(key, row)
        Next

        Return result
    End Function

    Private Function FindRow(ByVal rowsByKey As System.Collections.Generic.Dictionary(Of Integer, DataRow), ByVal key As Integer) As DataRow
        If rowsByKey IsNot Nothing AndAlso rowsByKey.ContainsKey(key) Then Return rowsByKey(key)
        Return Nothing
    End Function

    Private Function SumByOnSaleIds(ByVal amountByOnSaleId As System.Collections.Generic.Dictionary(Of Integer, DataRow), ByVal onSaleIds As String, ByVal columnName As String) As Double
        Dim total As Double = 0
        For Each onSaleId As Integer In CsvToList(onSaleIds)
            total += DbVal(FindRow(amountByOnSaleId, onSaleId), columnName)
        Next
        Return total
    End Function

    Private Function SumReceiptBasic(ByVal receiptBasicByPurchaseId As System.Collections.Generic.Dictionary(Of Integer, DataRow), ByVal purchaseIds As String) As Double
        Dim total As Double = 0
        For Each purchaseId As Integer In CsvToList(purchaseIds)
            total += DbVal(FindRow(receiptBasicByPurchaseId, purchaseId), "BasicAmountSum")
        Next
        Return total
    End Function

    Private Sub LoadProfitReport(Optional ByVal condtion As String = "", Optional ByVal includeCharges As Boolean = False, Optional ByVal oldMode As Integer = 0)
        dg1.Rows.Clear()

        Dim fromDate As String = CDate(txtFromDate.Text).ToString("yyyy-MM-dd")
        Dim toDate As String = CDate(txttoDate.Text).ToString("yyyy-MM-dd")
        ssql = "Select * From Purchase Where EntryDate between '" & fromDate & "' and '" & toDate & "' " & condtion & " Group by VoucherID"

        Dim dt As DataTable = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count = 0 Then
            calc() : lblCount.Text = "# :" & Val(dg1.RowCount)
            Exit Sub
        End If

        Dim voucherIds As String = IdListFromRows(dt, "VoucherID")
        Dim purchaseAgg As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey("Select VoucherID, Sum(Nug) as PNug, Sum(Weight) as PWeight From Purchase Where VoucherID in (" & voucherIds & ") Group by VoucherID", "VoucherID")
        Dim transAgg As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey(
            "Select T.PurchaseID, Sum(T.Nug) as SNug, Sum(T.Weight) as SWeight, Sum(T.Amount) as AmountSum, Sum(T.TotalAmount) as TotalAmountSum, " &
            "Sum(Case When T.TransType Not In ('On Sale') Then T.Amount Else 0 End) as NonOnSaleAmount, " &
            "Sum(Case When T.TransType Not In ('Standard Sale','On Sale') Then T.Amount Else 0 End) as NonStandardOnSaleAmount, " &
            "GROUP_CONCAT(Case When T.TransType='On Sale' Then T.VoucherID End) as OnSaleVoucherIDs, " &
            "(Select TransType From Transaction2 T2 Where T2.PurchaseID=T.PurchaseID Limit 1) as FirstTransType, " &
            "(Select SallerAmt From Transaction2 T2 Where T2.PurchaseID=T.PurchaseID Limit 1) as FirstSallerAmt " &
            "From Transaction2 T Where T.PurchaseID in (" & voucherIds & ") Group by T.PurchaseID", "PurchaseID")

        Dim lookupPurchaseIds As New System.Collections.Generic.List(Of Integer)
        Dim onSalePurchaseIds As New System.Collections.Generic.List(Of Integer)
        For Each idText As String In voucherIds.Split(","c)
            Dim id As Integer = CInt(Val(idText))
            If id > 0 Then lookupPurchaseIds.Add(id)
        Next
        For Each row As DataRow In transAgg.Values
            For Each id As Integer In CsvToList(DbText(row, "OnSaleVoucherIDs"))
                If lookupPurchaseIds.Contains(id) = False Then lookupPurchaseIds.Add(id)
                If onSalePurchaseIds.Contains(id) = False Then onSalePurchaseIds.Add(id)
            Next
        Next

        Dim transaction1LookupIds As String = IdListFromValues(lookupPurchaseIds)
        Dim transaction1ByPurchase As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey("Select PurchaseID, GROUP_CONCAT(OnSaleID) as OnSaleIDs, (Select VoucherID From Transaction1 T1B Where T1B.PurchaseID=T1.PurchaseID Limit 1) as FirstVoucherID From Transaction1 T1 Where PurchaseID in (" & transaction1LookupIds & ") Group by PurchaseID", "PurchaseID")

        Dim ledgerVoucherIds As New System.Collections.Generic.List(Of Integer)
        Dim voucherLookupIds As New System.Collections.Generic.List(Of Integer)
        Dim onSaleIds As New System.Collections.Generic.List(Of Integer)
        For Each row As DataRow In dt.Rows
            Dim voucherId As Integer = CInt(Val(DbText(row, "VoucherID")))
            If voucherId > 0 Then
                voucherLookupIds.Add(voucherId)
                ledgerVoucherIds.Add(voucherId)
            End If

            Dim chargeVoucherId As Integer = FirstCsvNumber(DbText(FindRow(transaction1ByPurchase, voucherId), "FirstVoucherID"))
            If chargeVoucherId > 0 Then
                voucherLookupIds.Add(chargeVoucherId)
                ledgerVoucherIds.Add(chargeVoucherId)
            End If
        Next
        For Each row As DataRow In transaction1ByPurchase.Values
            For Each onSaleId As Integer In CsvToList(DbText(row, "OnSaleIDs"))
                If onSaleIds.Contains(onSaleId) = False Then onSaleIds.Add(onSaleId)
            Next
        Next

        Dim ledgerIds As String = IdListFromValues(ledgerVoucherIds)
        Dim voucherLookupList As String = IdListFromValues(voucherLookupIds)
        Dim onSaleIdList As String = IdListFromValues(onSaleIds)
        Dim onSalePurchaseIdList As String = IdListFromValues(onSalePurchaseIds)
        Dim ledger28 As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey("Select VourchersID, Amount From Ledger Where AccountID=28 and VourchersID in (" & ledgerIds & ") Group by VourchersID", "VourchersID")
        Dim ledger46 As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey("Select VourchersID, Amount From Ledger Where AccountID=46 and VourchersID in (" & ledgerIds & ") Group by VourchersID", "VourchersID")
        Dim vouchersById As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey("Select ID, Sum(TotalAmount) as TotalAmountSum From Vouchers Where ID in (" & voucherLookupList & ") Group by ID", "ID")
        Dim amountByOnSaleId As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey("Select OnSaleID, Sum(Amount) as AmountSum From Transaction1 Where OnSaleID in (" & onSaleIdList & ") Group by OnSaleID", "OnSaleID")
        Dim receiptBasicByPurchaseId As System.Collections.Generic.Dictionary(Of Integer, DataRow) = LoadRowsByKey("Select T1.PurchaseID, Sum(BasicAmount) as BasicAmountSum From Transaction1 AS T1 INNER JOIN Vouchers AS V ON T1.VoucherID = V.ID Where V.TransType=('On Sale Receipt') and T1.PurchaseID in (" & onSalePurchaseIdList & ") Group by T1.PurchaseID", "PurchaseID")

        dg1.SuspendLayout()
        Try
            For Each row As DataRow In dt.Rows
                Dim voucherId As Integer = CInt(Val(DbText(row, "VoucherID")))
                Dim purchaseRow As DataRow = FindRow(purchaseAgg, voucherId)
                Dim transRow As DataRow = FindRow(transAgg, voucherId)
                Dim trans1Row As DataRow = FindRow(transaction1ByPurchase, voucherId)

                Dim pNug As Double = DbVal(purchaseRow, "PNug")
                Dim sNug As Double = DbVal(transRow, "SNug")
                Dim pWeight As Double = DbVal(purchaseRow, "PWeight")
                Dim sWeight As Double = DbVal(transRow, "SWeight")
                Dim saleAmount As Double = 0
                Dim purchaseAmount As Double = 0
                Dim transType As String = DbText(transRow, "FirstTransType")
                Dim onSaleVoucherIds As String = DbText(transRow, "OnSaleVoucherIDs")

                If oldMode = 1 Then
                    If transType = "Standard Sale" Then
                        saleAmount = DbVal(transRow, "FirstSallerAmt")
                    Else
                        saleAmount = DbVal(transRow, "AmountSum")
                    End If
                ElseIf oldMode = 2 Then
                    If transType = "Standard Sale" Then
                        saleAmount = DbVal(transRow, "FirstSallerAmt")
                    ElseIf includeCharges Then
                        saleAmount = DbVal(transRow, "TotalAmountSum")
                    Else
                        saleAmount = DbVal(transRow, "AmountSum")
                    End If
                Else
                    If radioboth.Checked = True Then
                        If includeCharges Then
                            saleAmount = DbVal(transRow, "AmountSum")
                            saleAmount += SumReceiptBasic(receiptBasicByPurchaseId, onSaleVoucherIds)
                        Else
                            saleAmount = DbVal(transRow, "NonOnSaleAmount")
                            Dim onSaleIdRow As DataRow = FindRow(transaction1ByPurchase, FirstCsvNumber(onSaleVoucherIds))
                            Dim onSaleIdsForPurchase As String = DbText(onSaleIdRow, "OnSaleIDs")
                            If FirstCsvNumber(onSaleIdsForPurchase) = voucherId Then
                                saleAmount += SumByOnSaleIds(amountByOnSaleId, onSaleIdsForPurchase, "AmountSum")
                            End If
                        End If
                    ElseIf radioOnSale.Checked = True Then
                        If includeCharges Then
                            saleAmount = SumReceiptBasic(receiptBasicByPurchaseId, onSaleVoucherIds)
                        Else
                            Dim onSaleIdRow As DataRow = FindRow(transaction1ByPurchase, FirstCsvNumber(onSaleVoucherIds))
                            Dim onSaleIdsForPurchase As String = DbText(onSaleIdRow, "OnSaleIDs")
                            If FirstCsvNumber(onSaleIdsForPurchase) = voucherId Then
                                saleAmount += SumByOnSaleIds(amountByOnSaleId, onSaleIdsForPurchase, "AmountSum")
                            End If
                        End If
                    ElseIf radioSale.Checked = True Then
                        If includeCharges Then
                            saleAmount = DbVal(transRow, "AmountSum")
                        Else
                            saleAmount = DbVal(transRow, "NonStandardOnSaleAmount")
                        End If
                    End If
                End If

                If DbText(row, "PurchaseTypeName") = "Purchase" Then
                    If includeCharges Then
                        purchaseAmount = DbVal(FindRow(vouchersById, voucherId), "TotalAmountSum")
                    Else
                        purchaseAmount = DbVal(FindRow(ledger28, voucherId), "Amount")
                    End If
                Else
                    Dim chargeVoucherId As Integer = FirstCsvNumber(DbText(trans1Row, "FirstVoucherID"))
                    If includeCharges Then
                        purchaseAmount = DbVal(FindRow(vouchersById, chargeVoucherId), "TotalAmountSum")
                    Else
                        purchaseAmount = DbVal(FindRow(ledger46, chargeVoucherId), "Amount")
                    End If
                End If

                Dim pnl As Object
                If oldMode = 0 AndAlso includeCharges = False Then
                    pnl = Format2(saleAmount - purchaseAmount)
                ElseIf includeCharges Then
                    If pNug = sNug Or pWeight = sWeight Then
                        pnl = Format2(saleAmount - purchaseAmount)
                    Else
                        pnl = "Not Sold"
                    End If
                Else
                    If pNug = sNug Then
                        pnl = Format2(saleAmount - purchaseAmount)
                    Else
                        pnl = "Not Sold"
                    End If
                End If

                dg1.Rows.Add(New Object() {
                    voucherId,
                    CDate(row("EntryDate")).ToString("dd-MM-yyyy"),
                    DbText(row, "BillNo"),
                    DbText(row, "VehicleNo"),
                    DbText(row, "AccountName"),
                    DbText(row, "PurchaseTypeName"),
                    Format2(pNug),
                    Format2(sNug),
                    Format2(pNug - sNug),
                    Format2(pWeight),
                    Format2(sWeight),
                    Format2(saleAmount),
                    Format2(purchaseAmount),
                    pnl,
                    Format2(pWeight - sWeight)})
            Next
        Finally
            dg1.ResumeLayout()
        End Try

        dg1.ClearSelection()
        calc() : lblCount.Text = "# :" & Val(dg1.RowCount)
    End Sub

    Private Function SqlLikeText(ByVal value As String) As String
        Return value.Trim().Replace("'", "''")
    End Function

    Private Function BuildFilterCondition() As String
        Dim condition As String = ""

        If txtSearch.Text.Trim() <> "" Then
            condition &= " And AccountName Like '" & SqlLikeText(txtSearch.Text) & "%'"
        End If

        If txtType.Text.Trim() <> "" Then
            condition &= " And PurchaseTypeName Like '" & SqlLikeText(txtType.Text) & "%'"
        End If

        Return condition
    End Function

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub retrive1(Optional ByVal condtion As String = "")
        LoadProfitReport(condtion, False, 1)
        Exit Sub
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
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
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        '.Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(BasicAmount) from Vouchers where ID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(dt.Rows(i)("VoucherID").ToString()) & " and AccountID=28")), "0.00")
                    Else
                        Dim ChargesID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & ""))
                        ' Dim TotalAmount As String = Val(clsFun.ExecScalarStr(" Select sum(BasicAmount) from Vouchers where ID = " & ChargesID & ""))
                        ' .Cells(12).Value = Format(Val(TotalAmount), "0.00")
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(ChargesID) & " and AccountID=46")), "0.00")

                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If

                End With
            Next i
        End If
        dg1.ClearSelection()
        calc()
    End Sub

    Private Sub RetriveChargeAlso1(Optional ByVal condtion As String = "")
        LoadProfitReport(condtion, True, 1)
        Exit Sub
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
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
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & Val(dt.Rows(i)("VoucherID").ToString()) & "")), "0.00")
                    Else
                        Dim ChargesID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & ""))
                        Dim TotalAmount As String = Val(clsFun.ExecScalarStr(" Select Sum(TotalAmount) from Vouchers where ID = " & ChargesID & ""))
                        .Cells(12).Value = Format(Val(TotalAmount), "0.00")
                        'Dim PurchaseID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction2 where PurchaseID = " & Val(dt.Rows(i)("VoucherID").ToString()) & ""))
                        'Dim TransID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & Val(dt.Rows(i)("VoucherID").ToString()) & ""))
                        '.Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & Val(TransID) & "")), "0.00")
                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Or Val(.Cells(9).Value) = Val(.Cells(10).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If
                    .Cells(14).Value = Format(Val(Val(.Cells(9).Value) - Val(.Cells(10).Value)), "0.00")
                    '.Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    '.Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                    '.Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    '.Cells(5).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                    '.Cells(2).Value = Math.Abs(Val(tmpamt)) & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts  WHERE id = " & dt.Rows(i)("Id").ToString() & "")
                End With
            Next i
        End If
        dg1.ClearSelection()
        calc()
    End Sub



    Private Sub retrive2(Optional ByVal condtion As String = "")
        LoadProfitReport(condtion, False, 2)
        Exit Sub
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
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
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        '.Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(BasicAmount) from Vouchers where ID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(dt.Rows(i)("VoucherID").ToString()) & " and AccountID=28")), "0.00")
                    Else
                        Dim ChargesID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & ""))
                        ' Dim TotalAmount As String = Val(clsFun.ExecScalarStr(" Select sum(BasicAmount) from Vouchers where ID = " & ChargesID & ""))
                        ' .Cells(12).Value = Format(Val(TotalAmount), "0.00")
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(ChargesID) & " and AccountID=46")), "0.00")
                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If

                End With
            Next i
        End If
        dg1.ClearSelection()
        calc()
    End Sub

    Private Sub RetriveChargeAlso2(Optional ByVal condtion As String = "")
        LoadProfitReport(condtion, True, 2)
        Exit Sub
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
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
                        .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TOtalAmount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & Val(dt.Rows(i)("VoucherID").ToString()) & "")), "0.00")
                    Else
                        Dim ChargesID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & ""))
                        Dim TotalAmount As String = Val(clsFun.ExecScalarStr(" Select Sum(TotalAmount) from Vouchers where ID = " & ChargesID & ""))
                        .Cells(12).Value = Format(Val(TotalAmount), "0.00")
                        'Dim PurchaseID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction2 where PurchaseID = " & Val(dt.Rows(i)("VoucherID").ToString()) & ""))
                        'Dim TransID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & Val(dt.Rows(i)("VoucherID").ToString()) & ""))
                        '.Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & Val(TransID) & "")), "0.00")
                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Or Val(.Cells(9).Value) = Val(.Cells(10).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If
                    .Cells(14).Value = Format(Val(Val(.Cells(9).Value) - Val(.Cells(10).Value)), "0.00")
                End With
            Next i
        End If
        dg1.ClearSelection()
        calc()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Dim filterCondition As String = BuildFilterCondition()

        If RadioOldMethod.Checked = True Then
            If ckExpAlso.Checked = True Then
                RetriveChargeAlso1(filterCondition)
            Else
                retrive1(filterCondition)
            End If

            Exit Sub
        End If
        If RadioOldMethod2.Checked = True Then
            If ckExpAlso.Checked = True Then
                RetriveChargeAlso2(filterCondition)
            Else
                retrive2(filterCondition)
            End If
            Exit Sub
        End If
        If ckExpAlso.Checked = True Then
            RetriveChargeAlso(filterCondition)
        Else
            retrive(filterCondition)
        End If
        '  calc()
    End Sub

    'Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
    '    Dim includeCharges As Boolean = ckExpAlso.Checked

    '    If RadioOldMethod.Checked Then
    '        retriveMerged("", includeCharges)
    '    ElseIf RadioOldMethod2.Checked Then
    '        retriveMerged("", includeCharges)
    '    Else
    '        retriveMerged("", includeCharges)
    '    End If
    'End Sub


    'Private Sub RetriveMerged(Optional ByVal condtion As String = "", Optional ByVal isChargeAlso As Boolean = False)
    '    dg1.Rows.Clear()
    '    Dim dt As New DataTable
    '    Dim i As Integer
    '    Dim count As Integer = 0
    '    ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
    '    dt = clsFun.ExecDataTable(ssql)

    '    If dt.Rows.Count > 0 Then
    '        For i = 0 To dt.Rows.Count - 1
    '            dg1.ClearSelection()
    '            dg1.Rows.Add()
    '            With dg1.Rows(i)
    '                .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
    '                .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
    '                .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
    '                .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
    '                .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
    '                .Cells(5).Value = dt.Rows(i)("PurchaseTypeName").ToString()
    '                .Cells(6).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Purchase where voucherID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
    '                .Cells(7).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(nug) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
    '                .Cells(8).Value = Format(Val(.Cells(6).Value) - Val(.Cells(7).Value), "0.00")
    '                .Cells(9).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Weight) from Purchase where voucherID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
    '                .Cells(10).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Weight) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")

    '                ' Determine TransType logic
    '                Dim transType As String = clsFun.ExecScalarStr(" Select TransType from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")
    '                If transType = "Standard Sale" Then
    '                    .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select SallerAmt from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
    '                Else
    '                    .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
    '                End If

    '                ' Handling .Cells(12) based on 'isChargeAlso' flag
    '                If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
    '                    If isChargeAlso Then
    '                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & Val(dt.Rows(i)("VoucherID").ToString()) & "")), "0.00")
    '                    Else
    '                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(dt.Rows(i)("VoucherID").ToString()) & " and AccountID=28")), "0.00")
    '                    End If
    '                Else
    '                    Dim ChargesID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & ""))
    '                    .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(ChargesID) & " and AccountID=46")), "0.00")
    '                End If

    '                ' Check final selling logic
    '                If Val(.Cells(6).Value) = Val(.Cells(7).Value) Then
    '                    .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
    '                Else
    '                    .Cells(13).Value = "Not Sold"
    '                End If
    '            End With
    '        Next i
    '    End If
    '    dg1.ClearSelection()
    '    calc() ' If needed
    'End Sub

    Sub calc()
        txtSentQty.Text = Format(0, "0.00") : txtOurCost.Text = Format(0, "0.00")
        txtNetCost.Text = Format(0, "0.00") : txtPNL.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtSentQty.Text = Format(Val(txtSentQty.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtOurCost.Text = Format(Val(txtOurCost.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            txtNetCost.Text = Format(Val(txtNetCost.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            txtPNL.Text = Format(Val(txtPNL.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
        Next
    End Sub
    Private Sub RetriveExpAlso()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' Group by VoucherID"
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
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    Else
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(amount) from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Or Val(.Cells(9).Value) = Val(.Cells(10).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If
                    .Cells(14).Value = Format(Val(Val(.Cells(9).Value) - Val(.Cells(10).Value)), "0.00")
                End With
            Next i
        End If
        dg1.ClearSelection()
        calc()
    End Sub
    Private Sub txtFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtFromDate.Validating
        txtFromDate.Text = SmartDate(txtFromDate.Text)
    End Sub

    Private Sub txttoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txttoDate.Validating
        txttoDate.Text = SmartDate(txttoDate.Text, True, 2)
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        LoadProfitReport(condtion, False, 0)
        Exit Sub
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
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
                    Dim transType As String = clsFun.ExecScalarStr("SELECT TransType FROM Transaction2 WHERE PurchaseID = " & dt.Rows(i)("VoucherID").ToString())
                    If radioboth.Checked = True Then
                        If transType <> "On Sale" Then
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount)  from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & " and TransType Not In('On Sale')")), "0.00")

                        Else
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & " and TransType Not In('On Sale') ")), "0.00")
                        End If
                        Dim Purchaseid As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(VoucherID) AS PurchaseIDs FROM Transaction2 Where TransType=('On Sale') AND PurchaseID=" & Val(dt.Rows(i)("VoucherID").ToString()) & "")
                        Dim OnSaleID As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(OnSaleID) AS PurchaseIDs FROM Transaction1 Where  PurchaseID=" & Val(Purchaseid) & "")
                        If Val(OnSaleID) = Val(dt.Rows(i)("VoucherID").ToString()) Then
                            Dim OnSaleAmt As String = Val(clsFun.ExecScalarStr(" Select Sum(Amount) from Transaction1  Where ONSaleID in (" & OnSaleID & ")"))
                            .Cells(11).Value = Format(Val(.Cells(11).Value) + Val(OnSaleAmt), "0.00")
                        End If
                    ElseIf radioOnSale.Checked = True Then
                        Dim Purchaseid As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(VoucherID) AS PurchaseIDs FROM Transaction2 Where TransType=('On Sale') AND PurchaseID=" & Val(dt.Rows(i)("VoucherID").ToString()) & "")
                        Dim OnSaleID As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(OnSaleID) AS PurchaseIDs FROM Transaction1 Where  PurchaseID=" & Val(Purchaseid) & "")
                        If Val(OnSaleID) = Val(dt.Rows(i)("VoucherID").ToString()) Then
                            Dim OnSaleAmt As String = Val(clsFun.ExecScalarStr(" Select Sum(Amount) from Transaction1  Where ONSaleID in (" & OnSaleID & ")"))
                            .Cells(11).Value = Format(Val(.Cells(11).Value) + Val(OnSaleAmt), "0.00")
                        End If
                    ElseIf radioSale.Checked = True Then
                        If transType <> "Standard Sale" Or transType <> "On Sale" Then
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount)  from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & " and TransType Not In('Standard Sale','On Sale')")), "0.00")

                        Else
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                        End If
                    End If
                    'If clsFun.ExecScalarStr(" Select TransType from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "") = "Standard Sale" Then
                    '    .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select SallerAmt from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    'Else
                    '    .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    'End If
                    'Dim Purchaseid As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(VoucherID) AS PurchaseIDs FROM Transaction2 Where TransType=('On Sale'); AND PurchaseID=" & dt.Rows(i)("VoucherID").ToString() & "")
                    'Dim OnSaleAmt As String = Val(clsFun.ExecScalarStr(" Select Sum(V.TotalAmount) from Transaction1  AS T1 INNER JOIN Vouchers AS V ON T1.VoucherID = V.ID  Where V.TransType=('On Sale Receipt') and PurchaseID in (" & Purchaseid & ")"))
                    '.Cells(11).Value = Val(.Cells(11).Value) + Val(OnSaleAmt)

                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        '.Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(BasicAmount) from Vouchers where ID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(dt.Rows(i)("VoucherID").ToString()) & " and AccountID=28")), "0.00")
                    Else
                        Dim ChargesID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & ""))
                        ' Dim TotalAmount As String = Val(clsFun.ExecScalarStr(" Select sum(BasicAmount) from Vouchers where ID = " & ChargesID & ""))
                        ' .Cells(12).Value = Format(Val(TotalAmount), "0.00")
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select Amount from Ledger where VourchersID = " & Val(ChargesID) & " and AccountID=46")), "0.00")

                    End If
                    '   If Val(.Cells(6).Value) = Val(.Cells(7).Value) Or Val(.Cells(9).Value) = Val(.Cells(10).Value) Then
                    .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    'Else
                    '.Cells(13).Value = "Not Sold"
                    'End If
                    .Cells(14).Value = Format(Val(Val(.Cells(9).Value) - Val(.Cells(10).Value)), "0.00")
                End With
            Next i
        End If
        dg1.ClearSelection()
        calc() : lblCount.Text = "# :" & Val(dg1.RowCount)
    End Sub
    Private Sub RetriveChargeAlso(Optional ByVal condtion As String = "")
        LoadProfitReport(condtion, True, 0)
        Exit Sub
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select * From Purchase  Where EntryDate between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
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
                    If radioboth.Checked = True Then
                        If clsFun.ExecScalarStr(" Select TransType from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "") = "Standard Sale" Then
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount)  from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")

                        Else
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                        End If
                        Dim Purchaseid As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(VoucherID) AS PurchaseIDs FROM Transaction2 Where TransType=('On Sale') AND PurchaseID=" & dt.Rows(i)("VoucherID").ToString() & "")
                        Dim OnSaleAmt As String = Val(clsFun.ExecScalarStr(" Select Sum(BasicAmount) from Transaction1  AS T1 INNER JOIN Vouchers AS V ON T1.VoucherID = V.ID  Where V.TransType=('On Sale Receipt') and PurchaseID in (" & Purchaseid & ")"))
                        .Cells(11).Value = Val(.Cells(11).Value) + Val(OnSaleAmt)

                    ElseIf radioOnSale.Checked = True Then
                        Dim Purchaseid As String = clsFun.ExecScalarStr("SELECT GROUP_CONCAT(VoucherID) AS PurchaseIDs FROM Transaction2 Where TransType=('On Sale') AND PurchaseID=" & dt.Rows(i)("VoucherID").ToString() & "")
                        Dim OnSaleAmt As String = Val(clsFun.ExecScalarStr(" Select Sum(BasicAmount) from Transaction1  AS T1 INNER JOIN Vouchers AS V ON T1.VoucherID = V.ID  Where V.TransType=('On Sale Receipt') and PurchaseID in (" & Purchaseid & ")"))
                        .Cells(11).Value = Val(.Cells(11).Value) + Val(OnSaleAmt)

                    ElseIf radioSale.Checked = True Then
                        If clsFun.ExecScalarStr(" Select TransType from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "") = "Standard Sale" Then
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount)  from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")

                        Else
                            .Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(Amount) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                        End If

                    End If
                    '.Cells(11).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(SallerAmt) from Transaction2 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & "")), "0.00")
                    If dt.Rows(i)("PurchaseTypeName").ToString() = "Purchase" Then
                        .Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & Val(dt.Rows(i)("VoucherID").ToString()) & "")), "0.00")
                    Else
                        Dim ChargesID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & dt.Rows(i)("VoucherID").ToString() & ""))
                        Dim TotalAmount As String = Val(clsFun.ExecScalarStr(" Select Sum(TotalAmount) from Vouchers where ID = " & ChargesID & ""))
                        .Cells(12).Value = Format(Val(TotalAmount), "0.00")
                        'Dim PurchaseID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction2 where PurchaseID = " & Val(dt.Rows(i)("VoucherID").ToString()) & ""))
                        'Dim TransID As Integer = Val(clsFun.ExecScalarStr(" Select VoucherID from Transaction1 where PurchaseID = " & Val(dt.Rows(i)("VoucherID").ToString()) & ""))
                        '.Cells(12).Value = Format(Val(clsFun.ExecScalarStr(" Select sum(TotalAmount) from Vouchers where ID = " & Val(TransID) & "")), "0.00")
                    End If
                    If Val(.Cells(6).Value) = Val(.Cells(7).Value) Or Val(.Cells(9).Value) = Val(.Cells(10).Value) Then
                        .Cells(13).Value = Format(Val(Val(.Cells(11).Value) - Val(.Cells(12).Value)), "0.00")
                    Else
                        .Cells(13).Value = "Not Sold"
                    End If
                    .Cells(14).Value = Format(Val(Val(.Cells(9).Value) - Val(.Cells(10).Value)), "0.00")
                    '.Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    '.Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                    '.Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    '.Cells(5).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                    '.Cells(2).Value = Math.Abs(Val(tmpamt)) & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts  WHERE id = " & dt.Rows(i)("Id").ToString() & "")
                End With
            Next i
        End If
        dg1.ClearSelection()
        calc()
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
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dg1.Rows.Count = 0 Then MsgBox("There is No Record to Print...", MsgBoxStyle.Critical, "No Record") : Exit Sub
        PrintRecord()
        Report_Viewer.printReport("\Reports\SellerProfitReport.rpt")
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
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18) values('" & txtFromDate.Text & "','" & txttoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                    "'" & .Cells(5).Value & "','" & Format(Val(.Cells(6).Value), "0.00") & "','" & Format(Val(.Cells(7).Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells(8).Value), "0.00") & "'," & Format(Val(.Cells(9).Value), "0.00") & ",'" & Format(Val(.Cells(10).Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells(11).Value), "0.00") & "', " & Format(Val(.Cells(12).Value), "0.00") & ",'" & .Cells(13).Value & "','" & txtSentQty.Text & "', " & _
                    "'" & txtOurCost.Text & "','" & txtNetCost.Text & "','" & txtPNL.Text & "','" & .Cells(14).Value & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub
    Private Sub txtFromDate_GotFocus(sender As Object, e As EventArgs) Handles txtFromDate.GotFocus, txtFromDate.Click
        txtFromDate.SelectAll()
    End Sub
    Private Sub txttoDate_GotFocus(sender As Object, e As EventArgs) Handles txttoDate.GotFocus, txttoDate.Click
        txttoDate.SelectAll()
    End Sub
    Private Sub txtFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFromDate.KeyDown, txttoDate.KeyDown
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
    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        txttoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        txttoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        txttoDate.Text = smartDate(txttoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        txtFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        txtFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        txtFromDate.Text = smartDate(txtFromDate.Text)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub txtFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs)

    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp, txtType.KeyUp
        If e.KeyCode = Keys.Enter Then
            btnShow.PerformClick()
        End If
    End Sub

    'Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
    '    If e.KeyCode = Keys.Enter Then
    '        Dim searchCondition As String = ""

    '        If txtSearch.Text.Trim() <> "" Then
    '            searchCondition = "And AccountName Like '" & txtSearch.Text.Trim() & "%'"
    '        End If

    '        Dim methodType As String
    '        Dim includeCharges As Boolean = ckExpAlso.Checked

    '        ' Determine which retrieval method to use
    '        If RadioOldMethod.Checked Then
    '            methodType = "OldMethod1"
    '        ElseIf RadioOldMethod2.Checked Then
    '            methodType = "OldMethod2"
    '        Else
    '            methodType = "Default"
    '        End If

    '        ' Call MergedRetrive with appropriate parameters
    '        RetriveMerged(searchCondition, includeCharges)
    '    End If
    'End Sub


    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

    End Sub

    Private Sub RadioOldMethod_CheckedChanged(sender As Object, e As EventArgs) Handles RadioOldMethod.CheckedChanged

    End Sub

    Private Sub txtFromDate_TextChanged(sender As Object, e As EventArgs) Handles txtFromDate.TextChanged

    End Sub
End Class
