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

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub retrive1(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
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
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
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
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
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
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
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
        If RadioOldMethod.Checked = True Then
            If ckExpAlso.Checked = True Then
                RetriveChargeAlso1()
            Else
                retrive1()
            End If

            Exit Sub
        End If
        If RadioOldMethod2.Checked = True Then
            If ckExpAlso.Checked = True Then
                RetriveChargeAlso2()
            Else
                retrive2()
            End If
            Exit Sub
        End If
        If ckExpAlso.Checked = True Then
            RetriveChargeAlso()
        Else
            retrive()
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
    '    ssql = "Select * From Purchase  Where EntryDate between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " Group by VoucherID"
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
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
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
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
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
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
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
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
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

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub

    Private Sub txtSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtSearch.Text.Trim() <> "" Then
                'If RadioOldMethod.Checked = True AndAlso ckExpAlso.Checked = True Then RetriveChargeAlso1("And AccountName Like '" & txtSearch.Text.Trim() & "%'") : Exit Sub
                'If RadioOldMethod.Checked = False AndAlso ckExpAlso.Checked = False Then retrive1("And AccountName Like '" & txtSearch.Text.Trim() & "%'") : Exit Sub
                'If RadioOldMethod2.Checked = True AndAlso ckExpAlso.Checked = True Then RetriveChargeAlso2("And AccountName Like '" & txtSearch.Text.Trim() & "%'") : Exit Sub
                'If RadioOldMethod2.Checked = False AndAlso ckExpAlso.Checked = False Then retrive2("And AccountName Like '" & txtSearch.Text.Trim() & "%'") : Exit Sub
                If ckExpAlso.Checked = True Then
                    RetriveChargeAlso("And AccountName Like '" & txtSearch.Text.Trim() & "%'")
                End If

                If ckExpAlso.Checked = False Then
                    retrive("And AccountName Like '" & txtSearch.Text.Trim() & "%'")
                End If

            End If
            If txtSearch.Text.Trim() = "" Then Exit Sub
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
End Class