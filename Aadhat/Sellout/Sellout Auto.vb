Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Threading

Public Class Sellout_Auto
    Dim sql As String = String.Empty
    Dim Vno As Integer
    Dim VchId As Integer
    Dim count As Integer = 0
    Dim CalcType As String = String.Empty
    Dim PurchaseNugs As Integer = 0
    Dim SoldNugs As Integer = 0
    Dim TotalNugs As Integer = 0
    Dim remark As String = String.Empty
    Dim remarkHindi As String = String.Empty
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 1 : Dim Offset As Integer = 0
    Dim ServerTag As Integer : Dim ItemPer As String
    Dim crateRate As String = String.Empty : Dim trackStock As String
    Dim CalculationMethod As String : Dim CUT As Decimal = 0.0
    Dim whatsappSender As New WhatsAppSender()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        clsFun.DoubleBuffered(Dg2, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub
    Private Sub txtGrossWt_GotFocus(sender As Object, e As EventArgs) Handles txtGrossWt.GotFocus
        txtGrossWt.SelectAll()
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 50
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "Date" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "AccountName" : .Columns(3).Width = 159
            .Columns(4).Name = "BillingType" : .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo" : .Columns(5).Width = 59
            .Columns(6).Name = "itemName" : .Columns(6).Width = 69
            .Columns(7).Name = "LotNo" : .Columns(7).Width = 76
            .Columns(8).Name = "Nug" : .Columns(8).Width = 90
            .Columns(9).Name = "Weight" : .Columns(9).Width = 86
            .Columns(10).Name = "Rate" : .Columns(10).Width = 90
            .Columns(11).Name = "per" : .Columns(11).Width = 50
            .Columns(12).Name = "Amount" : .Columns(12).Width = 95
            .Columns(13).Name = "ChargeName" : .Columns(13).Width = 159
            .Columns(14).Name = "onValue" : .Columns(14).Width = 159
            .Columns(15).Name = "@" : .Columns(15).Width = 59
            .Columns(16).Name = "=/-" : .Columns(16).Width = 59
            .Columns(17).Name = "ChargeAmount" : .Columns(17).Width = 69
            .Columns(18).Name = "TotalNug" : .Columns(18).Width = 76
            .Columns(19).Name = "TotalWeight" : .Columns(19).Width = 90
            .Columns(20).Name = "TotalBasicAmount" : .Columns(20).Width = 86
            .Columns(21).Name = "RoundOff" : .Columns(21).Width = 90
            .Columns(22).Name = "TotalAmount" : .Columns(22).Width = 90
            .Columns(23).Name = "OtherItemName" : .Columns(23).Width = 95
            .Columns(24).Name = "OtherAccountName" : .Columns(24).Width = 159
            .Columns(25).Name = "AmountInWords" : .Columns(25).Width = 159
            .Columns(26).Name = "PaidCharges" : .Columns(24).Width = 159
            .Columns(27).Name = "TotalCharges" : .Columns(25).Width = 159
            .Columns(28).Name = "Address" : .Columns(25).Width = 159
            .Columns(29).Name = "City" : .Columns(25).Width = 159
            .Columns(30).Name = "State" : .Columns(25).Width = 159
            .Columns(31).Name = "Area" : .Columns(25).Width = 159
            .Columns(32).Name = "Mobile1" : .Columns(25).Width = 159
            .Columns(33).Name = "Mobile2" : .Columns(25).Width = 159
            .Columns(34).Name = "FarmerName" : .Columns(34).Width = 159
            .Columns(35).Name = "FarmerAccount" : .Columns(35).Width = 159
            .Columns(35).Name = "PrintOtherCharge" : .Columns(35).Width = 159
            '''''New Option
            .Columns(36).Name = "itemName" : .Columns(36).Width = 69
            .Columns(37).Name = "LotNo" : .Columns(37).Width = 76
            .Columns(38).Name = "Nug" : .Columns(38).Width = 90
            .Columns(39).Name = "Weight" : .Columns(39).Width = 86
            .Columns(40).Name = "Rate" : .Columns(40).Width = 90
            .Columns(41).Name = "per" : .Columns(41).Width = 50
            .Columns(42).Name = "Amount" : .Columns(42).Width = 95
            .Columns(43).Name = "ChargeName" : .Columns(43).Width = 159
            .Columns(44).Name = "onValue" : .Columns(44).Width = 159
            .Columns(45).Name = "@" : .Columns(45).Width = 59
            .Columns(46).Name = "=/-" : .Columns(46).Width = 59
            .Columns(47).Name = "ChargeAmount" : .Columns(47).Width = 69
            .Columns(48).Name = "HindiItem" : .Columns(48).Width = 69
            .Columns(49).Name = "GrossWt" : .Columns(49).Width = 69
        End With
    End Sub
 
    Private Sub PrintOnly()
        TempRowColumn()
        Dim i, j As Integer
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        tmpgrid.Rows.Clear()
        If dg1.Rows.Count = 0 Then Exit Sub
        LineMargin = clsFun.ExecScalarInt("Select Margin From Controls")
        Dim margin As String = String.Empty
        If LineMargin = 0 Then margin = vbCrLf
        If LineMargin = 1 Then margin = vbCrLf & vbCrLf
        If LineMargin = 2 Then margin = vbCrLf & vbCrLf
        If LineMargin = 3 Then margin = vbCrLf & vbCrLf & vbCrLf
        If LineMargin = 4 Then margin = vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If LineMargin = 5 Then margin = vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If dg1.Rows.Count > 0 Then
            For i = 0 To dg1.Rows.Count - 1
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = mskEntryDate.Text
                    .Cells(2).Value = .Cells(2).Value & txtVoucherNo.Text
                    .Cells(3).Value = txtAccount.Text
                    .Cells(5).Value = .Cells(5).Value & txtVoucherNo.Text
                    .Cells(6).Value = .Cells(6).Value & dg1.Rows(i).Cells(0).Value
                    .Cells(7).Value = .Cells(7).Value & dg1.Rows(i).Cells(1).Value
                    .Cells(8).Value = .Cells(8).Value & dg1.Rows(i).Cells(2).Value
                    .Cells(9).Value = .Cells(9).Value & dg1.Rows(i).Cells(3).Value
                    .Cells(10).Value = .Cells(10).Value & dg1.Rows(i).Cells(4).Value
                    .Cells(11).Value = .Cells(11).Value & dg1.Rows(i).Cells(5).Value
                    .Cells(12).Value = .Cells(12).Value & dg1.Rows(i).Cells(6).Value
                    .Cells(18).Value = .Cells(18).Value & txtTotalNug.Text
                    .Cells(19).Value = .Cells(19).Value & txttotalWeight.Text
                    .Cells(20).Value = .Cells(20).Value & txtbasicTotal.Text
                    .Cells(21).Value = .Cells(21).Value & txttotalNetAmount.Text
                    .Cells(22).Value = .Cells(22).Value & txttotalCharges.Text
                    .Cells(23).Value = clsFun.ExecScalarStr("Select OtherName From Items Where Id='" & Val(dg1.Rows(i).Cells(7).Value) & "'")
                    .Cells(24).Value = clsFun.ExecScalarStr("Select OtherName From Accounts Where Id='" & Val(txtAccountID.Text) & "'")
                    .Cells(25).Value = lblInword.Text
                    .Cells(26).Value = .Cells(26).Value & txtPaidCharges.Text
                    .Cells(27).Value = .Cells(27).Value & Val(txtPaidCharges.Text) + Val(txttotalCharges.Text)
                    .Cells(28).Value = clsFun.ExecScalarStr("Select Address From Accounts Where Id='" & Val(txtAccountID.Text) & "'")
                    .Cells(29).Value = clsFun.ExecScalarStr("Select City From Accounts Where Id='" & Val(txtAccountID.Text) & "'")
                    .Cells(30).Value = clsFun.ExecScalarStr("Select State From Accounts Where Id='" & Val(txtAccountID.Text) & "'")
                    .Cells(31).Value = clsFun.ExecScalarStr("Select Area From Accounts Where Id='" & Val(txtAccountID.Text) & "'")
                    .Cells(32).Value = clsFun.ExecScalarStr("Select Mobile1 From Accounts Where Id='" & Val(txtAccountID.Text) & "'")
                    .Cells(33).Value = clsFun.ExecScalarStr("Select Mobile2 From Accounts Where Id='" & Val(txtAccountID.Text) & "'")
                    .Cells(34).Value = lblFarmerName.Text
                    .Cells(35).Value = lblFarmerName.Text & "-(" & txtAccount.Text & ")"
                    For k = 0 To dg1.Rows.Count - 1
                        If dg1.Rows.Count > 0 Then
                            .Cells(36).Value = .Cells(36).Value & dg1.Rows(k).Cells(0).Value & vbCrLf
                            .Cells(37).Value = .Cells(37).Value & dg1.Rows(k).Cells(1).Value & vbCrLf
                            .Cells(38).Value = .Cells(38).Value & dg1.Rows(k).Cells(2).Value & vbCrLf
                            .Cells(39).Value = .Cells(39).Value & Format(Val(dg1.Rows(k).Cells(3).Value), "0.00") & vbCrLf
                            .Cells(40).Value = .Cells(40).Value & Format(Val(dg1.Rows(k).Cells(4).Value), "0.00") & vbCrLf
                            .Cells(41).Value = .Cells(41).Value & dg1.Rows(k).Cells(5).Value & vbCrLf
                            .Cells(42).Value = .Cells(42).Value & Format(Val(dg1.Rows(k).Cells(6).Value), "0.00") & vbCrLf
                            .Cells(48).Value = .Cells(48).Value & clsFun.ExecScalarStr("Select OtherName From Items Where Id='" & Val(dg1.Rows(k).Cells(7).Value) & "'") & vbCrLf
                            .Cells(49).Value = .Cells(49).Value & Format(Val(dg1.Rows(k).Cells(10).Value), "0.00") & vbCrLf
                        End If
                    Next
                    If dg3.Rows.Count > 0 Then
                        For j = 0 To dg3.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dg3.Rows(j).Cells(0).Value & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & dg3.Rows(j).Cells(1).Value & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dg3.Rows(j).Cells(2).Value & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dg3.Rows(j).Cells(3).Value & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & dg3.Rows(j).Cells(4).Value & vbCrLf
                            .Cells(43).Value = .Cells(43).Value & dg3.Rows(j).Cells(0).Value & margin
                            .Cells(44).Value = .Cells(44).Value & Format(Val(dg3.Rows(j).Cells(1).Value), "0.00") & margin
                            .Cells(45).Value = .Cells(45).Value & dg3.Rows(j).Cells(2).Value & margin
                            .Cells(46).Value = .Cells(46).Value & dg3.Rows(j).Cells(3).Value & margin
                            .Cells(47).Value = .Cells(47).Value & Format(Val(dg3.Rows(j).Cells(4).Value), "0.00") & margin
                        Next
                    End If
                    If Dg2.Rows.Count > 0 Then
                        For j = 0 To Dg2.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & Dg2.Rows(j).Cells(0).Value & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Dg2.Rows(j).Cells(1).Value & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & Dg2.Rows(j).Cells(2).Value & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & Dg2.Rows(j).Cells(3).Value & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & Dg2.Rows(j).Cells(4).Value & vbCrLf
                            .Cells(43).Value = .Cells(43).Value & Dg2.Rows(j).Cells(0).Value & margin
                            .Cells(44).Value = .Cells(44).Value & Format(Val(Dg2.Rows(j).Cells(1).Value), "0.00") & margin
                            .Cells(45).Value = .Cells(45).Value & Dg2.Rows(j).Cells(2).Value & margin
                            .Cells(46).Value = .Cells(46).Value & Dg2.Rows(j).Cells(3).Value & margin
                            .Cells(47).Value = .Cells(47).Value & Format(Val(Dg2.Rows(j).Cells(4).Value), "0.00") & margin
                        Next
                    End If
                    If dg3.Rows.Count = 0 AndAlso Dg2.Rows.Count = 0 Then
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
    End Sub

    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub Sellout_Auto_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False : Exit Sub
            If dgVehicleNo.Visible = True Then dgVehicleNo.Visible = False : Exit Sub
            If dgCharges.Visible = True Then dgCharges.Visible = False : Exit Sub
            If dgItemSearch.Visible = True Then dgItemSearch.Visible = False : Exit Sub
            If pnlPaidCharges.Visible = True Then pnlPaidCharges.Visible = False : Exit Sub
            If pnlWhatsapp.Visible = True Then
                cleartxt() : cleartxtCharges() : FootertextClear()
                dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
                pnlWhatsapp.Visible = False : txtid.Clear() : Exit Sub
            End If

            If dg1.Rows.Count > 0 Then
                If MessageBox.Show("Are you Sure Want to Exit Sellout ???", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Me.Close() : Exit Sub
                End If
            Else
                Me.Close() : Exit Sub
            End If
        End If
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub Sellout_Auto_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : RadioPrint1.Checked = True
        Cbper.SelectedIndex = 0 : cbBillingType.SelectedIndex = 0
        BtnDelete.Enabled = False : rowColums() : FootertextClear()
        Cbper.Text = clsFun.ExecScalarStr("Select per From Controls")
    End Sub
    Private Sub VNumber()
        Vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        txtVoucherNo.Text = Vno + 1
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 11
        dg1.Columns(0).Name = "Item Name" : dg1.Columns(0).Width = 383 : dg1.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(1).Name = "Lot No" : dg1.Columns(1).Width = 199 ' : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Nug" : dg1.Columns(2).Width = 114 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).Name = "Weight" : dg1.Columns(3).Width = 114 : dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).Name = "Rate" : dg1.Columns(4).Width = 114 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).Name = "per" : dg1.Columns(5).Width = 100 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).Name = "Amount" : dg1.Columns(6).Width = 143 : dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).Name = "ItemID" : dg1.Columns(7).Visible = False : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).Name = "ID" : dg1.Columns(8).Visible = False
        dg1.Columns(9).Name = "AddWeight" : dg1.Columns(9).Visible = False
        dg1.Columns(10).Name = "GrossWt" : dg1.Columns(10).Visible = False
        Dg2.ColumnCount = 7
        Dg2.Columns(0).Name = "Charge Name" : Dg2.Columns(0).Width = 259 : Dg2.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : Dg2.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Dg2.Columns(1).Name = "On Value" : Dg2.Columns(1).Width = 113 : Dg2.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : Dg2.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(2).Name = "Cal" : Dg2.Columns(2).Width = 114 : Dg2.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : Dg2.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(3).Name = "+/-" : Dg2.Columns(3).Width = 114 : Dg2.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter : Dg2.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dg2.Columns(4).Name = "Amount" : Dg2.Columns(4).Width = 114 : Dg2.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : Dg2.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(5).Name = "ChargeID" : Dg2.Columns(5).Visible = False : Dg2.Columns(6).Name = "CostOn" : Dg2.Columns(6).Visible = False

        dg3.ColumnCount = 7
        dg3.Columns(0).Name = "Charge Name" : dg3.Columns(0).Width = 259 : dg3.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        dg3.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg3.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg3.Columns(1).Name = "On Value" : dg3.Columns(1).Width = 113 : dg3.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg3.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg3.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg3.Columns(2).Name = "Cal" : dg3.Columns(2).Width = 114 : dg3.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        dg3.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg3.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg3.Columns(3).Name = "+/-" : dg3.Columns(3).Width = 114 : dg3.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg3.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter : dg3.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg3.Columns(4).Name = "Amount" : dg3.Columns(4).Width = 114 : dg3.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg3.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg3.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg3.Columns(5).Name = "ChargeID" : dg3.Columns(5).Visible = False : dg3.Columns(6).Name = "CostOn" : dg3.Columns(6).Visible = False

    End Sub

    Private Sub ItemFill()
        Dim itemID As Integer = Val(txtItemID.Text)
        If itemID = 0 Then Exit Sub ' Prevent unnecessary queries if ItemID is invalid
        ' Use a single query to fetch all required fields
        Dim query As String = "SELECT CommisionPer, UserChargesPer, Tare, Labour, RDFPer, MaintainCrate, WeightPerNug,TrackStock,RateAs,CutPerNug FROM Items WHERE ID = " & itemID
        Dim dt As DataTable = clsFun.ExecDataTable(query) ' Assume ExecDataTable returns a DataTable
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)
            CUT = row("CutPerNug").ToString()
            If ItemPer = "ItemWise" Then CbPer.Text = row("RateAs").ToString()
            trackStock = row("TrackStock").ToString()
        End If
        'AccountComm()
    End Sub

    Sub calc()
        txtTotalNug.Text = Format(0, "0.00") : txttotalWeight.Text = Format(0, "0.00")
        txtbasicTotal.Text = Format(0, "0.00") : txttotalCharges.Text = Format(0, "0.00")
        txtPaidCharges.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNug.Text = Format(Val(txtTotalNug.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txttotalWeight.Text = Format(Val(txttotalWeight.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtbasicTotal.Text = Format(Val(txtbasicTotal.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")

        Next
        For i = 0 To Dg2.Rows.Count - 1
            Dim CalcType As String = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
            Dim PlusMinus As String = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
            If Dg2.Rows(i).Cells(3).Value = "-" Then
                txttotalCharges.Text = Format(Val(txttotalCharges.Text) - Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            Else
                txttotalCharges.Text = Format(Val(txttotalCharges.Text) + Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            End If
        Next
        For i = 0 To dg3.Rows.Count - 1
            If dg3.Rows(i).Cells(3).Value = "-" Then
                txtPaidCharges.Text = Format(Val(txtPaidCharges.Text) - Val(dg3.Rows(i).Cells(4).Value), "0.00")
            Else
                txtPaidCharges.Text = Format(Val(txtPaidCharges.Text) + Val(dg3.Rows(i).Cells(4).Value), "0.00")
            End If
        Next
        txttotalNetAmount.Text = Format(Val(txtbasicTotal.Text) + Val(txttotalCharges.Text) + Val(txtPaidCharges.Text), "0.00")
        Dim tmpamount As Double = CDbl(Val(txttotalNetAmount.Text))
        txttotalNetAmount.Text = Math.Round(Val(tmpamount), 0)
        txtroundoff.Text = Format(Val(txttotalNetAmount.Text) - Val(tmpamount), "0.00")
        txttotalNetAmount.Text = Format(Val(txttotalNetAmount.Text), "0.00")
        Try
            If Val(txttotalNetAmount.Text) > 0 Then
                lblInword.Text = AmtInWord(Math.Abs(Val(txttotalNetAmount.Text)))
            Else
                lblInword.Text = "(Minus)" & AmtInWord(Math.Abs(Val(txttotalNetAmount.Text)))
            End If


        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
    End Sub
    Private Sub FillCharges()
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        If Val(txtCalculatePer.Text) = 0 Then
            txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        End If
        If CalcType = "Aboslute" Then
            txtOnValue.TabStop = False
            txtCalculatePer.TabStop = False
            txtchargesAmount.Focus()
        ElseIf CalcType = "Weight" Then
            txtOnValue.Text = txttotalWeight.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Percentage" Then
            txtOnValue.Text = txtbasicTotal.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Nug" Then
            txtOnValue.Text = txtTotalNug.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Crate" Then
            txtOnValue.Text = txtTotalNug.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        End If
    End Sub

    Private Sub PrintRecord()
        Dim FastQuery As String = String.Empty
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & txtVoucherNo.Text & "','" & lblPurchaseDate.Text & "','" & txtVehicle.Text & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," & _
                                "'" & lblInword.Text & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & .Cells(28).Value & "'," & _
                                "'" & .Cells(29).Value & "','" & .Cells(30).Value & "','" & .Cells(31).Value & "','" & .Cells(32).Value & "', " & _
                                "'" & .Cells(33).Value & "','" & .Cells(34).Value & "','" & .Cells(35).Value & "','" & .Cells(36).Value & "', " & _
                                "'" & .Cells(37).Value & "','" & .Cells(38).Value & "','" & .Cells(39).Value & "','" & .Cells(40).Value & "'," & _
                                "'" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "','" & .Cells(44).Value & "'," & _
                                "'" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "','" & .Cells(49).Value & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into Printing(D1, D2,M1,M2,M3,M4, P1,P2, P3, P4, P5, P6,P7,P8,P9, " & _
                    " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20,P21,P22,P23,P24,T1,T2,T3,T4,T5,T6,T7,T8,P25,P26,P27,P28,P29,P30,P31,P32,P33,P34,P35,P36,P37,P38)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub


    Private Sub txtLotNo_GotFocus(sender As Object, e As EventArgs) Handles txtLotNo.GotFocus
        txtLotNo.SelectionStart = 0 : txtLotNo.SelectionLength = Len(txtLotNo.Text)
        'If txtItem.Text.Trim() <> "" Then
        '    dgItemSearch.Visible = True
        '    retriveItems(" Where upper(ItemName) Like upper('%" & txtItem.Text.Trim() & "%')")
        'Else
        '    ' dgItemSearch.Visible = True
        '    retriveItems()
        'End If
        If dgItemSearch.SelectedRows.Count = 0 Then txtItem.Focus() : Exit Sub
        If txtItem.Text = "" Then txtItem.Focus() : Exit Sub
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        dgItemSearch.Visible = False


        'If txtItem.Text.ToUpper <> dgItemSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtItem.Focus() : Exit Sub
        'If txtItem.Text = "" Then txtItem.Focus() : Exit Sub
        'If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
        'If txtItem.Text.ToUpper = dgItemSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then

        'Else
        '    txtItem.Focus()
        'End If
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        AccountRowColumns()
        txtVehicle.SelectionStart = 0 : txtVehicle.SelectionLength = Len(txtVehicle.Text)
    End Sub

    Private Sub cbBillingType_GotFocus(sender As Object, e As EventArgs) Handles cbBillingType.GotFocus
        'If txtVehicle.Text.Trim() <> "" Then
        '    'dgCharges.Visible = True
        '    RetrvieVehicle(" And upper(VehicleNo) Like upper('" & txtVehicle.Text.Trim() & "%')")
        'Else
        '    RetrvieVehicle()
        'End If
        If dgVehicleNo.SelectedRows.Count = 0 Then Exit Sub
        txtVehicleID.Text = Val(dgVehicleNo.SelectedRows(0).Cells(0).Value)
        txtVehicle.Text = dgVehicleNo.SelectedRows(0).Cells(1).Value
        dgVehicleNo.Visible = False
        ' If txtVehicleID.Text = "" Then txtVehicle.Focus() : Exit Sub

        'If dgVehicleNo.RowCount = 0 Then txtVehicle.Focus() : Exit Sub
        'If txtVehicle.Text.ToUpper <> dgVehicleNo.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtVehicle.Focus() : Exit Sub
        'If txtVehicle.Text.ToUpper = dgVehicleNo.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then


        'Else
        '    txtVehicle.Focus()
        'End If
    End Sub

    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        txtNug.SelectionStart = 0 : txtNug.SelectionLength = Len(txtNug.Text)
    End Sub

    Private Sub txtKg_GotFocus(sender As Object, e As EventArgs) Handles txtKg.GotFocus
        txtKg.SelectionStart = 0 : txtKg.SelectionLength = Len(txtKg.Text)
    End Sub

    Private Sub txtRate_GotFocus(sender As Object, e As EventArgs) Handles txtRate.GotFocus
        txtRate.SelectionStart = 0 : txtRate.SelectionLength = Len(txtRate.Text)
    End Sub

    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus, txtAccount.GotFocus,
    txtVoucherNo.GotFocus, txtVehicle.GotFocus, txtLotNo.GotFocus, txtNug.GotFocus, txtKg.GotFocus, txtRate.GotFocus, txtTotAmount.GotFocus
        If txtNug.Focused Or txtLotNo.Focused Then ItemFill()
        If txtKg.Focused Then txtKg.SelectAll() : pnlGrossWeight.Visible = False
        If mskEntryDate.Focused Then mskEntryDate.BackColor = Color.LightGray : mskEntryDate.SelectAll() : Exit Sub
        If txtItem.Focused Then
            ItemRowColumns()
            If txtItem.Text.Trim() <> "" Then
                dgItemSearch.Visible = True
                retriveItems(" Where upper(ItemName) Like upper('%" & txtItem.Text.Trim() & "%')")
            Else
                dgItemSearch.Visible = True
                retriveItems()
            End If
        End If
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        'tb.BackColor = Color.GhostWhite
        tb.SelectAll()
    End Sub


    Private Sub TxtItem_LostFocus(sender As Object, e As EventArgs) Handles txtItem.LostFocus, txtAccount.LostFocus,
    txtVoucherNo.LostFocus, txtVehicle.LostFocus, txtLotNo.LostFocus, txtNug.LostFocus, txtKg.LostFocus, txtRate.LostFocus, txtTotAmount.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.GhostWhite
    End Sub

    Private Sub txtGrossWt_KeyUp(sender As Object, e As KeyEventArgs) Handles txtGrossWt.KeyUp
        If Val(txtGrossWt.Text) = 0 Then Exit Sub
        txtKg.Text = Format(Val(txtGrossWt.Text) - (Val(CUT) * Val(txtNug.Text)), "0.00")
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtItem.KeyDown, txtAccount.KeyDown, txtGrossWt.KeyDown,
        txtVoucherNo.KeyDown, cbBillingType.KeyDown, txtVehicle.KeyDown, txtLotNo.KeyDown, txtNug.KeyDown, txtKg.KeyDown, txtRate.KeyDown, Cbper.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtNug.Focused AndAlso Val(CUT) > 0 Then
                pnlGrossWeight.Visible = True : txtGrossWt.Focus()
                pnlGrossWeight.BringToFront()
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        If e.KeyCode = Keys.Down Then
            If DgAccountSearch.Visible = False Or dgItemSearch.Visible = False Or dgVehicleNo.Visible = True Then
                Exit Sub
            End If
            ' Agar focus combobox par hai, to dropdown kholna
            If TypeOf sender Is ComboBox Then
                Dim comboBox As ComboBox = DirectCast(sender, ComboBox)
                If Not comboBox.DroppedDown Then
                    comboBox.DroppedDown = True
                    e.SuppressKeyPress = True
                End If
            Else
                ' DataGridView par focus karna
                If dg1.RowCount = 0 Then Exit Sub
                dg1.Focus()
                dg1.CurrentCell = dg1.Rows(0).Cells(1) ' Pehli row ke pehle column par focus
                dg1.Rows(0).Selected = True
            End If
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
        If txtItem.Focused Then
            If e.KeyCode = Keys.F3 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
            End If
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
            If e.KeyCode = Keys.Down Then
                If dgItemSearch.Visible = True Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If Cbper.Focused = True Then
            If e.KeyCode = Keys.Down Then Exit Sub
        End If


        If DgAccountSearch.Visible = False Or dgItemSearch.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
    End Sub
    Private Sub SpeedCalculation()
        If Cbper.SelectedIndex = 0 Then
            txtTotAmount.Text = Format(Val(txtNug.Text) * Val(txtRate.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtTotAmount.Text = Format(Val(txtKg.Text) * Val(txtRate.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 5 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 3 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 10 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 4 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 20 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 5 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 40 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 6 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 41 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 7 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 50 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 8 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 51 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 9 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 51.7 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 10 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 52.3 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 11 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 53 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 12 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 80 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 13 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) / 100 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 14 Then
            txtTotAmount.Text = Format(Val(txtRate.Text) * Val(txtNug.Text), "0.00")
        End If
        txtTotAmount.Text = Format(Math.Round(Val(txtTotAmount.Text), 2), "0.00")
    End Sub

    Private Sub txtTotAmount_GotFocus(sender As Object, e As EventArgs) Handles txtTotAmount.GotFocus
        txtTotAmount.SelectionStart = 0 : txtTotAmount.SelectionLength = Len(txtTotAmount.Text)
    End Sub

    Private Sub txtTotAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotAmount.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(0).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(1).Value = txtLotNo.Text
                dg1.SelectedRows(0).Cells(2).Value = Val(txtNug.Text)
                dg1.SelectedRows(0).Cells(3).Value = Val(txtKg.Text)
                dg1.SelectedRows(0).Cells(4).Value = Val(txtRate.Text)
                dg1.SelectedRows(0).Cells(5).Value = Cbper.Text
                dg1.SelectedRows(0).Cells(6).Value = txtTotAmount.Text
                dg1.SelectedRows(0).Cells(7).Value = Val(txtItemID.Text)
                dg1.SelectedRows(0).Cells(8).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(10).Value = Val(txtGrossWt.Text)
                txtItem.Focus()
                cleartxt()
                calc()
                dg1.ClearSelection()

            Else
                dg1.Rows.Add(txtItem.Text, txtLotNo.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtKg.Text), "0.00"),
                             Format(Val(txtRate.Text), "0.00"), Cbper.Text, Format(Val(txtTotAmount.Text), "0.00"),
                             Val(txtItemID.Text), "", Val(txtGrossWt.Text))
                cleartxt() : calc()
                txtItem.Focus()
                dg1.ClearSelection()
            End If
        End If
    End Sub
    Private Sub cleartxt()
        '  txtLotNo.Text = "" : txtNug.Text = ""
        txtKg.Text = "" : txtTotAmount.Text = "" : txtGrossWt.Text = ""
    End Sub
    Private Sub FootertextClear()
        '  txtid.Text = ""
        BtnSave.Text = "&Save" : txtTotalNug.Text = ""
        txtbasicTotal.Text = "" : txttotalWeight.Text = ""
        txttotalNetAmount.Text = "" : txttotalCharges.Text = ""
        txtroundoff.Text = "" : VNumber()
        txtVehicle.Text = "" : txtRate.Text = ""
        txtPaidCharges.Text = "" : pnlPaidCharges.Visible = False
        btnPnlShow.Visible = False : dg1.Rows.Clear()
        Dg2.Rows.Clear() : mskEntryDate.Focus() : mskEntryDate.SelectAll()
    End Sub
    Private Sub cleartxtCharges()
        txtOnValue.Text = ""
        txtCalculatePer.Text = ""
        txtPlusMinus.Text = ""
        txtchargesAmount.Text = ""
    End Sub
    Private Sub ChargesCalculation()
        ' If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
        If CalcType = "Percentage" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * CDbl(txtCalculatePer.Text) / 100, "0.00")
        ElseIf CalcType = "Nug" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Weight" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Crate" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * CDbl(txtCalculatePer.Text), "0.00")
        End If
        'If String.IsNullOrEmpty(txtbasicTotal.Text) OrElse String.IsNullOrEmpty(txttotalCharges.Text) Then Exit Sub
        'txttotalNetAmount.Text = Format(CDbl(txtbasicTotal.Text) + CDbl(txttotalCharges.Text), "#######.00")
    End Sub
    Private Sub Cbper_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbper.SelectedIndexChanged
        SpeedCalculation()
    End Sub

    Private Sub txtOnValue_GotFocus(sender As Object, e As EventArgs) Handles txtOnValue.GotFocus
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        'If txtCharges.Text.Trim() <> "" Then
        '    RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
        'Else
        '    RetriveCharges()
        'End If
        '   FillCharges()
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        dgCharges.Visible = False : FillCharges()
    End Sub

    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtKg.KeyPress,
        txtRate.KeyPress, txtTotAmount.KeyPress, txttotalCharges.KeyPress, txtOnValue.KeyPress, txtCalculatePer.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub



    Private Sub txtKg_Leave(sender As Object, e As EventArgs) Handles txtKg.Leave
        If txtKg.Text = "" Then txtKg.Text = Val(0)
    End Sub

    Private Sub txtRate_Leave(sender As Object, e As EventArgs) Handles txtRate.Leave
        If txtRate.Text = "" Then txtRate.Text = Val(0)
    End Sub

    Private Sub txtchargesAmount_GotFocus(sender As Object, e As EventArgs) Handles txtchargesAmount.GotFocus
        dgItemSearch.Visible = False : DgAccountSearch.Visible = False
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If dgCharges.SelectedRows.Count = 0 Then txtCharges.Focus() : Exit Sub
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        dgCharges.Visible = False : FillCharges()
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        txtCharges.SelectionLength = Len(txtCharges.Text)
    End Sub

    Private Sub txtTotAmount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtTotAmount.KeyUp
        'If Cbper.SelectedIndex = 0 Then
        '    txtRate.Text = Val(txtTotAmount.Text) / Format(Val(txtNug.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 1 Then
        '    txtRate.Text = Val(txtTotAmount.Text) / Format(Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 2 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 5 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 3 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 10 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 4 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 20 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 5 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 40 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 6 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 41 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 7 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 50 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 8 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 51 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 9 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 51.7 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 10 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 52.3 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 11 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 53 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 12 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 80 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 13 Then
        '    txtRate.Text = Format(Val(txtTotAmount.Text) * 100 / Val(txtKg.Text), "0.00")
        'ElseIf Cbper.SelectedIndex = 14 Then
        '    txtRate.Text = Format(Val(txtNug.Text) * Val(txtTotAmount.Text), "0.00")
        'End If
        'txtRate.Text = Format(Math.Round(Val(txtRate.Text), 2), "0.00")
        'If txtRate.Text = "NAN" Then txtRate.Text = ""
    End Sub
    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave, Cbper.Leave,
        txtKg.Leave, txtRate.Leave, txtTotAmount.Leave, txtchargesAmount.Leave,
        txttotalNetAmount.Leave, txtbasicTotal.Leave, txtCalculatePer.Leave,
       txtOnValue.Leave, txttotalCharges.Leave
        ChargesCalculation()
        SpeedCalculation()
        Try
            If Val(txttotalNetAmount.Text) > 0 Then
                lblInword.Text = AmtInWord(Math.Abs(Val(txttotalNetAmount.Text)))
            Else
                lblInword.Text = "(Minus)" & AmtInWord(Math.Abs(Val(txttotalNetAmount.Text)))
            End If


        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
    End Sub
    Private Sub Save()
        PurchaseNugs = clsFun.ExecScalarInt("Select sum(Nug) From Purchase Where VoucherID='" & Val(txtVehicleID.Text) & "' ")
        SoldNugs = clsFun.ExecScalarInt("Select sum(Nug) From Transaction2 Where PurchaseID='" & Val(txtVehicleID.Text) & "'")
        'If Val(PurchaseNugs) <> Val(SoldNugs) Then
        '    MsgBox("Please Make Sure You have Sold all Items / Nugs.", MsgBoxStyle.Critical, "Stock Not Sold.")
        '    Exit Sub
        'End If
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        If dg1.Rows.Count = 0 Then
            Exit Sub
            MsgBox("There is no record to Save.", vbOKOnly, "Empty Record")
        End If
        dg1.ClearSelection()
        Dim cmd As SQLite.SQLiteCommand
        sql = "insert into Vouchers(TransType,BillNo,VehicleNo, Entrydate, " _
                                    & "SallerID, Sallername, Nug, kg,BasicAmount, TotalAmount,TotalCharges,BillingType)" _
                                    & "values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10,@11,@12)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@3", txtVehicle.Text)
            cmd.Parameters.AddWithValue("@4", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@5", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@6", txtAccount.Text)
            cmd.Parameters.AddWithValue("@7", txtTotalNug.Text)
            cmd.Parameters.AddWithValue("@8", txttotalWeight.Text)
            cmd.Parameters.AddWithValue("@9", txtbasicTotal.Text)
            cmd.Parameters.AddWithValue("@10", txttotalNetAmount.Text)
            cmd.Parameters.AddWithValue("@11", txttotalCharges.Text)
            cmd.Parameters.AddWithValue("@12", cbBillingType.Text)
            If cmd.ExecuteNonQuery() > 0 Then
            End If
            clsFun.CloseConnection()
            txtid.Text = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
            dg1Record() : dg2Record() : Servertag = 1
            LedgerInsert() : ChargeInsert()
            ServerLedgerInsert() : ServerChargeInsert()
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            txtid.Text = Val(VchId)
            '            retrive2()

        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Sub LedgerInsert()
        Dim fastQuery As String = String.Empty
        Dim dt As DateTime
        Dim tmpamount As Decimal = Val(txtbasicTotal.Text)
        Dim tmpamount2 As Decimal = Val(txtbasicTotal.Text)
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")

        Dim SellOutCharges As String = clsFun.ExecScalarStr("Select ChargeEffect From Controls")
        If SellOutCharges = "Yes" Then
            For Each row As DataGridViewRow In dg3.Rows
                With row
                    If .Cells("Charge Name").Value <> "" Then
                        If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                            If .Cells(3).Value = "+" Then
                                tmpamount = tmpamount + Val(.Cells(4).Value)
                            Else
                                tmpamount = tmpamount - Val(.Cells(4).Value)
                            End If
                        ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(6).Value) & "'") = "Our Cost" Then ''our coast
                            If .Cells(3).Value = "+" Then
                                tmpamount2 = Math.Round(Val(tmpamount2) + Val(.Cells(4).Value))
                            Else
                                tmpamount2 = Math.Round(Val(tmpamount2) - Val(.Cells(4).Value))
                            End If
                        End If
                    End If
                End With
            Next
        End If

        ''Caluclate  net amt
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim TxtID.text As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Our Cost" Then ''our coast
                        If .Cells(3).Value = "+" Then
                            tmpamount2 = Math.Round(Val(tmpamount2) + Val(.Cells(4).Value))
                        Else
                            tmpamount2 = Math.Round(Val(tmpamount2) - Val(.Cells(4).Value))
                        End If
                    End If
                End If
            End With
        Next
        Dim SelloutRemark As String = clsFun.ExecScalarStr("Select SelloutRemark From Controls")
        If SelloutRemark = "Full" Then
            RemarkNaration()
            remark = "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No.: " & txtVehicle.Text & ",(Total Nugs: " & txtTotalNug.Text & ")" & vbCrLf & remark
            remarkHindi = "वाउचर नं.: " & txtVoucherNo.Text & ",गाडी संख्या: " & txtVehicle.Text & ",(कुल नग: " & txtTotalNug.Text & ")" & vbCrLf & remarkHindi
        ElseIf SelloutRemark = "Short" Then
            remark = "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text
            remarkHindi = "वाउचर नं.: " & txtVoucherNo.Text
        Else
            RemarkNaration()
        End If
        ' RemarkNaration()
        If Val(txtbasicTotal.Text) > 0 Then ''Manual Beejak Account Fixed
            '      clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 46, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46"), Val(tmpamount2), "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 46 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Val(tmpamount2) & "','D','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
        End If
        If Val(txttotalNetAmount.Text) > 0 Then ''Account 
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "','C','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"

        Else
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ", Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "','D','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) < 0 Then
                '   clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtroundoff.Text)), "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(42) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','C','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            Else
                'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Val(txtroundoff.Text), "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(42) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','D','" & remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            End If
        End If
        Dim isDiff As String = clsFun.ExecScalarStr("Select sendDiff From Controls")
        If isDiff = "Yes" Then
            Dim CustomersBasic As String = Val(clsFun.ExecScalarStr("Select Sum(Amount) From Transaction2 Where PurchaseID=" & Val(txtVehicleID.Text) & ""))
            Dim diff As Decimal = Val(CustomersBasic) - Val(txtbasicTotal.Text)
            If diff <> 0 Then
                If Val(diff) < 0 Then
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 56, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56"), Math.Abs(Val(diff)), "D", "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text)
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 38, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38"), Math.Abs(Val(diff)), "C", "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff)
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(56) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "','" & Math.Abs(diff) & "','D','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & remarkHindi & "'"
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(38) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "','" & Math.Abs(diff) & "','C','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & remarkHindi & "'"

                Else
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 56, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56"), Val(diff), "C", "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text)
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 38, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38"), Math.Abs(Val(diff)), "D", "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff)
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(56) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "','" & Math.Abs(diff) & "','C','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & remarkHindi & "'"
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(38) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "','" & Math.Abs(diff) & "','D','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & remarkHindi & "'"
                End If
            End If
        End If
        clsFun.FastLedger(fastQuery)
        clsFun.CloseConnection()
    End Sub
    Sub ServerLedgerInsert()
        If Val(OrgID) = 0 Then Exit Sub
        Dim fastQuery As String = String.Empty
        Dim dt As DateTime
        Dim tmpamount As Decimal = Val(txtbasicTotal.Text)
        Dim tmpamount2 As Decimal = Val(txtbasicTotal.Text)
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")

        Dim SellOutCharges As String = clsFun.ExecScalarStr("Select ChargeEffect From Controls")
        If SellOutCharges = "Yes" Then
            For Each row As DataGridViewRow In dg3.Rows
                With row
                    If .Cells("Charge Name").Value <> "" Then
                        If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                            If .Cells(3).Value = "+" Then
                                tmpamount = tmpamount + Val(.Cells(4).Value)
                            Else
                                tmpamount = tmpamount - Val(.Cells(4).Value)
                            End If
                        ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(6).Value) & "'") = "Our Cost" Then ''our coast
                            If .Cells(3).Value = "+" Then
                                tmpamount2 = Math.Round(Val(tmpamount2) + Val(.Cells(4).Value))
                            Else
                                tmpamount2 = Math.Round(Val(tmpamount2) - Val(.Cells(4).Value))
                            End If
                        End If
                    End If
                End With
            Next
        End If

        ''Caluclate  net amt
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim TxtID.text As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Our Cost" Then ''our coast
                        If .Cells(3).Value = "+" Then
                            tmpamount2 = Math.Round(Val(tmpamount2) + Val(.Cells(4).Value))
                        Else
                            tmpamount2 = Math.Round(Val(tmpamount2) - Val(.Cells(4).Value))
                        End If
                    End If
                End If
            End With
        Next
        ' RemarkNaration()
        Dim Remark As String = "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text
        Dim RemarkHindi As String = "वाउचर नं.: " & txtVoucherNo.Text
        If Val(txtbasicTotal.Text) > 0 Then ''Manual Beejak Account Fixed
            '      clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 46, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46"), Val(tmpamount2), "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 46 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Val(tmpamount2) & "','D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txttotalNetAmount.Text) > 0 Then ''Account 
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "','C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"

        Else
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ", Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "','D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) < 0 Then
                '   clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtroundoff.Text)), "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(42) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            Else
                'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Val(txtroundoff.Text), "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(42) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            End If
        End If
        Dim isDiff As String = clsFun.ExecScalarStr("Select sendDiff From Controls")
        If isDiff = "Yes" Then
            Dim CustomersBasic As String = Val(clsFun.ExecScalarStr("Select Sum(Amount) From Transaction2 Where PurchaseID=" & Val(txtVehicleID.Text) & ""))
            Dim diff As Decimal = Val(CustomersBasic) - Val(txtbasicTotal.Text)
            If diff <> 0 Then
                If Val(diff) < 0 Then
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 56, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56"), Math.Abs(Val(diff)), "D", "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text)
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 38, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38"), Math.Abs(Val(diff)), "C", "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff)
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(56) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "','" & Math.Abs(diff) & "','D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(38) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "','" & Math.Abs(diff) & "','C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & RemarkHindi & "'"

                Else
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 56, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56"), Val(diff), "C", "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text)
                    'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 38, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38"), Math.Abs(Val(diff)), "D", "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff)
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(56) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "','" & Math.Abs(diff) & "','C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(38) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "','" & Math.Abs(diff) & "','D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txttotalNetAmount.Text & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                End If
            End If
        End If
        ClsFunserver.FastLedger(fastQuery)
    End Sub
    Private Sub RemarkNaration()
        remark = String.Empty
        remarkHindi = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                remark = remark & .Cells(0).Value & " Lot No. : " & .Cells(1).Value & ", Nug. : " & Format(Val(.Cells(2).Value), "0.00") & ",Weight : " & Format(Val(.Cells(3).Value), "0.00") & ",Rate : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & vbCrLf
                Dim othername As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID='" & Val(.Cells(7).Value) & "' ")
                remarkHindi = remarkHindi & othername & " Lot No. : " & .Cells(1).Value & ", नग : " & Format(Val(.Cells(2).Value), "0.00") & ",वजन : " & Format(Val(.Cells(3).Value), "0.00") & ",भाव : " & Format(Val(.Cells(4).Value), "0.00") & "/- " & .Cells(5).Value & vbCrLf
            End With
        Next
    End Sub
    Sub dg1Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtid.Text) & ",'" & .Cells(0).Value & "','" & .Cells(1).Value & "','" & .Cells(2).Value & "'," &
                                            " '" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "', " &
                                            "'" & .Cells(7).Value & "'," & Val(txtVehicleID.Text) & ",'" & Val(.Cells(10).Value) & "'"
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            Sql = "insert into Transaction1(VoucherID, ItemName,Lot, Nug, Weight, Rate, Per,Amount,ItemID,PurchaseID,GrossWeight) " & FastQuery & ""
            clsFun.ExecNonQuery(Sql)
        Catch ex As Exception

        End Try
        clsFun.CloseConnection()
    End Sub
    Sub ChargeInsert()
        Dim FastQuery As String = String.Empty
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim Remark As String = "(" & txtAccount.Text & ") Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text
        Dim RemarkHindi As String = " (" & txtAccount.Text & ") वाउचर नं.: " & txtVoucherNo.Text & ",गाडी नं.: " & txtVehicle.Text & ",नग: " & txtTotalNug.Text
        Dim SellOutCharges As String = clsFun.ExecScalarStr("Select ChargeEffect From Controls")
        If SellOutCharges = "Yes" Then
            For Each row As DataGridViewRow In dg3.Rows
                With row
                    If .Cells("Charge Name").Value <> "" Then
                        Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & Val(.Cells(5).Value) & "")
                        ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & Val(.Cells(5).Value) & "")
                        Dim AccName As String = ssql
                        If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                            If .Cells(3).Value = "+" Then
                                '    clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            Else
                                '                                clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            End If
                        ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then
                            If .Cells(3).Value = "+" Then
                                '       clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            Else
                                'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            End If
                        End If
                    End If
                End With
            Next
        End If
        If FastQuery <> String.Empty Then
            clsFun.FastLedger(FastQuery)
        End If
        FastQuery = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & Val(.Cells(5).Value) & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            '                            clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
        clsFun.CloseConnection()
    End Sub

    Sub ServerChargeInsert()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim Remark As String = "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text
        Dim RemarkHindi As String = "वाउचर नं.: " & txtVoucherNo.Text
        Dim SellOutCharges As String = clsFun.ExecScalarStr("Select ChargeEffect From Controls")
        If SellOutCharges = "Yes" Then
            For Each row As DataGridViewRow In dg3.Rows
                With row
                    If .Cells("Charge Name").Value <> "" Then
                        Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & Val(.Cells(5).Value) & "")
                        ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & Val(.Cells(5).Value) & "")
                        Dim AccName As String = ssql
                        If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                            If .Cells(3).Value = "+" Then
                                '    clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            Else
                                '                                clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            End If
                        ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then
                            If .Cells(3).Value = "+" Then
                                '       clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            Else
                                'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                            End If
                        End If
                    End If
                End With
            Next
        End If
        If FastQuery <> String.Empty Then
            ClsFunserver.FastLedger(FastQuery)
        End If
        FastQuery = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & Val(.Cells(5).Value) & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & Val(.Cells(5).Value) & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            '                            clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.: " & txtVoucherNo.Text & ",Vehicle No : " & txtVehicle.Text & ",Nugs: " & txtTotalNug.Text, txtAccount.Text, "वाउचर नं.: " & txtVoucherNo.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(Servertag) & ", " & Val(OrgID) & ",'" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)

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
    Private Sub UpdateRecord()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim count As Integer = 0
        dg1.ClearSelection()
        ' Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicle.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  SallerID='" & txtAccountID.Text & "', Sallername='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtbasicTotal.Text & "', TotalAmount='" & txttotalNetAmount.Text & "',TotalCharges='" & txttotalCharges.Text & "'," _
                                 & " BillingType='" & cbBillingType.Text & "' where ID =" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
            End If
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from Transaction1 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
            End If
            ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
            dg1Record() : dg2Record() : Servertag = 1
            LedgerInsert() : ChargeInsert()
            ServerLedgerInsert() : ServerChargeInsert()
            BtnSave.Text = "&Save"
            BtnDelete.Enabled = False
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Public Sub MultiUpdate()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim count As Integer = 0
        dg1.ClearSelection()
        '  Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicle.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  SallerID='" & Val(txtAccountID.Text) & "', Sallername='" & txtAccount.Text & "', Nug='" & Val(txtTotalNug.Text) & "', kg='" & Val(txttotalWeight.Text) & "'," _
                                & " BasicAmount='" & txtbasicTotal.Text & "', TotalAmount='" & Val(txttotalNetAmount.Text) & "',TotalCharges='" & Val(txttotalCharges.Text) & "'," _
                                 & " BillingType='" & cbBillingType.Text & "' where ID =" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
            End If
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from Transaction1 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
            End If
            ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
            dg1Record() : dg2Record() : Servertag = 1
            LedgerInsert() : ChargeInsert()
            ServerLedgerInsert() : ServerChargeInsert()
            BtnSave.Text = "&save"
            BtnDelete.Enabled = False
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub Delete()
        Dim RemoveSellout As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
        If RemoveSellout <> "Y" Then MsgBox("You Don't Have Rights to Delete Sellout Bill... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Try
            If MessageBox.Show("Are you Sure want to Delete Auto Sellout...??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & ";DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from TransAction1 WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & "") > 0 Then
                    ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                    Servertag = 0 : ServerLedgerInsert() : ServerChargeInsert()
                    MsgBox("Auto Sellout Successfully deleted", MsgBoxStyle.Information, "Deleted")
                    cleartxt() : cleartxtCharges() : FootertextClear()
                    dg1.Rows.Clear() : Dg2.Rows.Clear()
                    BtnDelete.Enabled = False
                    BtnSave.Text = "&Save"
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim Sql As String = String.Empty
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnDelete.Enabled = True
        btnPrint.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        If cbBillingType.SelectedIndex = 0 Then
            Sql = "Select * from Transaction2 where PurchaseID=" & Val(id) & " order by SRate Desc"
        ElseIf cbBillingType.SelectedIndex = 1 Then
            Sql = "Select ItemID, ItemName,lot,sum(nug) as nug,Round(sum(GrossWeight),2) as GrossWeight ,sum(weight) as weight,(sum(sallerAmt) / sum(weight)) as Rate, avg(SRate) as SRate,Per,sum(SallerAmt)  As SallerAmt from Transaction2 where PurchaseID=" & id & " Group by ItemName,Lot,Per order by SRate Desc"
        ElseIf cbBillingType.SelectedIndex = 2 Then
            Sql = "Select * from Transaction2 where PurchaseID=" & Val(id) & " order by SRate Desc"
        ElseIf cbBillingType.SelectedIndex = 3 Then
            Sql = "Select ItemID, ItemName,Lot,Round(sum(Nug),2) as nug,Round(sum(GrossWeight),2) as GrossWeight,Round(sum(Weight),2) as weight,Round(SRate,2) as SRate,Per,Round(sum(SallerAmt),2) as Salleramt from Transaction2 where PurchaseID=" & id & " Group By ItemId,Lot,SRate,Per order by ItemName asc,SRate Desc"
        ElseIf cbBillingType.SelectedIndex = 4 Then
            Sql = "Select ItemID, ItemName,Lot,Round(sum(Nug),2) as nug,Round(sum(GrossWeight),2) as GrossWeight,Round(sum(Weight),2) as weight,Round(SRate,2) as SRate,Per,Round(sum(SallerAmt),2) as Salleramt from Transaction2 where PurchaseID=" & id & " Group By ItemId,Lot,SRate,Per order by SRate Desc"
        End If
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & id
        clsFun.con.Open()
        Dim ad1 As New SQLite.SQLiteDataAdapter(Sql, clsFun.GetConnection)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds As New DataSet
        ' ad.Fill(ds, "a")
        ad1.Fill(ds, "b")
        ad2.Fill(ds, "C")
        If ds.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells("Item Name").Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells("Lot No").Value = ds.Tables("b").Rows(i)("Lot").ToString()
                .Rows(i).Cells("Nug").Value = Format(Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells("Weight").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                If ds.Tables("b").Rows(i)("Per").ToString() = "Kg" Then
                    .Rows(i).Cells("Rate").Value = Format(Val(ds.Tables("b").Rows(i)("sallerAmt").ToString()) / Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                ElseIf ds.Tables("b").Rows(i)("Per").ToString() = "Nug" Then
                    .Rows(i).Cells("Rate").Value = Format(Val(ds.Tables("b").Rows(i)("sallerAmt").ToString()) / Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                Else
                    .Rows(i).Cells("Rate").Value = Format(Val(ds.Tables("b").Rows(i)("SRate").ToString()), "0.00")
                End If
                .Rows(i).Cells("per").Value = ds.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("b").Rows(i)("SallerAmt").ToString()), "0.00")
                .Rows(i).Cells("ItemID").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells("GrossWt").Value = ds.Tables("b").Rows(i)("GrossWeight").ToString()
            Next
        End With
        If ds.Tables("c").Rows.Count > 0 Then
            dg3.Rows.Clear()
            With dg3
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
            btnPnlShow.Visible = True
            'txtItemID.Text = IID
        End If
        dg1.ClearSelection()
        Dg2.ClearSelection()
        dg3.ClearSelection()
        calc()
        Dim ssqlll As String
        ssqlll = clsFun.ExecScalarStr("Select EntryDate from Purchase where VoucherID=" & Val(txtVehicleID.Text) & "")
        lblFarmerName.Text = clsFun.ExecScalarStr("Select AccountName from Vouchers where ID=" & Val(txtVehicleID.Text) & "")
        lbl.Visible = True : lblPurchaseDate.Visible = True
        lblPurchaseDate.Text = CDate(ssqlll).ToString("dd-MM-yyyy")
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub


    Private Sub txtCharges_LostFocus(sender As Object, e As EventArgs) Handles txtCharges.LostFocus, txtOnValue.LostFocus, txtCalculatePer.LostFocus, txtPlusMinus.LostFocus, txtchargesAmount.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.GhostWhite
    End Sub

    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs) Handles txtCharges.GotFocus, txtOnValue.GotFocus, txtCalculatePer.GotFocus, txtPlusMinus.GotFocus, txtchargesAmount.GotFocus
        If txtCharges.Focused Then
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
        End If
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub


    Private Sub txtCharges_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyDown, txtOnValue.KeyDown, txtCalculatePer.KeyDown, txtPlusMinus.KeyDown
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
            End If
            If e.KeyCode = Keys.F1 Then
                If Val(dgCharges.SelectedRows(0).Cells(0).Value) = 0 Then Exit Sub
                ChargesForm.MdiParent = MainScreenForm
                ChargesForm.Show()
                ChargesForm.FillContros(Val(dgCharges.SelectedRows(0).Cells(0).Value))
                If Not ChargesForm Is Nothing Then
                    ChargesForm.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.Down Then dgCharges.Focus()
        End If
        If e.KeyCode = Keys.Down Then
            If dgCharges.Visible = True Then dgCharges.Focus() : Exit Sub
            Dg2.Rows(0).Selected = True : Dg2.Focus()
        End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
            txtLotNo.Text = dg1.SelectedRows(0).Cells(1).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
            txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
            txtTotAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
            txtTotAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
            txtGrossWt.Text = Val(dg1.SelectedRows(0).Cells(10).Value)
            '  lblCrate.Text = dg1.SelectedRows(0).Cells(9).Value
            txtItem.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Up Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Index) = 0 Then txtItem.Focus()
            dg1.ClearSelection()
        End If
        If e.KeyCode = Keys.Down Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Index) = Val(dg1.Rows.Count - 1) Then dg1.Rows(0).Selected = True
        End If
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
                calc()
                'ClearDetails()
            End If
        End If
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
        txtLotNo.Text = dg1.SelectedRows(0).Cells(1).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
        txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
        txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
        Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
        txtTotAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
        txtGrossWt.Text = Val(dg1.SelectedRows(0).Cells(10).Value)
        'txtItem.Focus()
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
                calc() : txtCharges.Focus() : Dg2.ClearSelection() : cleartxtCharges()
            Else
                Dg2.Rows.Add(txtCharges.Text, txtOnValue.Text, txtCalculatePer.Text, txtPlusMinus.Text, txtchargesAmount.Text, txtChargeID.Text)
                calc() : cleartxtCharges() : txtCharges.Focus() : Dg2.ClearSelection()
            End If
        End If
    End Sub

    Private Sub Dg2_KeyDown(sender As Object, e As KeyEventArgs) Handles Dg2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            txtCharges.Text = Dg2.SelectedRows(0).Cells(0).Value
            txtOnValue.Text = Dg2.SelectedRows(0).Cells(1).Value
            txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(2).Value
            txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(3).Value
            txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(4).Value
            txtChargeID.Text = Dg2.SelectedRows(0).Cells(5).Value
            txtCharges.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
                calc() : cleartxtCharges()
                Dg2.ClearSelection() : txtCharges.Focus()
                'ClearDetails()
            End If
        End If
        If e.KeyCode = Keys.Up Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            If Val(Dg2.SelectedRows(0).Index) = 0 Then txtItem.Focus()
            Dg2.ClearSelection()
        End If
        If e.KeyCode = Keys.Down Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            If Val(Dg2.SelectedRows(0).Index) = Val(Dg2.Rows.Count - 1) Then Dg2.Rows(0).Selected = True
        End If
        If e.KeyCode = Keys.Back Then txtCharges.Focus()
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
        txtChargeID.Text = Dg2.SelectedRows(0).Cells(5).Value
        'txtCharges.Focus()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        TempRowColumn()
        mskEntryDate.Focus()
        If pnlWhatsapp.Visible = True Then MsgBox("Print Pop-Up Showing... Please Close it", vbOKOnly, "Print Pop Up") : Exit Sub
        If dg1.RowCount = 0 Then MsgBox("There is No record to Save / Update...", vbOKOnly, "Empty") : Exit Sub
        If Val(txtVehicleID.Text) = 0 Then MsgBox("Please Check Vehicle Number...", MsgBoxStyle.Critical, "Invallid Vehicle") : Exit Sub
        If BtnSave.Text = "&Save" Then
            Dim AddSellout As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
            If AddSellout <> "Y" Then MsgBox("You Don't Have Rights to Save Sellout... " & vbNewLine & " Please Contact to Admin..Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            PurchaseNugs = clsFun.ExecScalarInt("Select sum(Nug) From Purchase Where VoucherID='" & Val(txtVehicleID.Text) & "' ")
            SoldNugs = clsFun.ExecScalarInt("Select sum(Nug) From Transaction2 Where PurchaseID='" & Val(txtVehicleID.Text) & "'")
            If Val(PurchaseNugs) <> Val(SoldNugs) Then
                If MessageBox.Show("Stock Balances Are Due to Sale In this Vehicle, " & vbNewLine & "Are Your Sure Want to Save this ???", "Save Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Save() : txtid.Clear()
                Else
                    Exit Sub
                End If
                'Exit Sub
            Else
                Save() : txtid.Clear()
            End If
        Else
            Dim ModifySellout As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
            If ModifySellout <> "Y" Then MsgBox("You Don't Have Rights to Modify Sellout... " & vbNewLine & " Please Contact to Admin..Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            UpdateRecord() : txtid.Clear()
        End If
        Dim res = MessageBox.Show("Do you want to Print Bill", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If res = Windows.Forms.DialogResult.Yes Then
            btnPrint.Enabled = True : btnPrint.PerformClick()
            txtid.Clear()
            Exit Sub
        Else
        End If
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub
    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        Me.Refresh()
        ' clsFun.FillDropDownList(CbAccountName, "Select * From Accounts where groupid in(11,17)", "AccountName", "Id", "")
        '  clsFun.FillDropDownList(cbitem, "Select * From Items", "ItemName", "Id", "")
        '  clsFun.FillDropDownList(CbCharges, "Select * From Charges", "ChargeName", "Id", "")
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If dg1.RowCount = 0 Then MsgBox("There is no record to Print...", vbOKOnly, "Empty") : Exit Sub
        Dim PrintSellout As String = clsFun.ExecScalarStr("SELECT Print FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
        If PrintSellout <> "Y" Then MsgBox("You Don't Have Rights to Print Purchase... " & vbNewLine & " Please Contact to Admin..Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        txtWhatsappNo.Text = clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
        pnlWhatsapp.Visible = True : pnlWhatsapp.BringToFront() : txtWhatsappNo.Focus()
        If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
            cbType.SelectedIndex = 0
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
        Else
            cbType.SelectedIndex = 1
        End If

    End Sub
    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=33 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
    End Sub

    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        ' CustomerFill()
        DgAccountSearch.Visible = False
        txtVehicle.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            'CreateAccount.txtGroup.Text = Val(clsFun.ExecScalarStr(" Select GroupName FROM AccountGroup WHERE ID =33"))
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            '    CustomerFill()
            DgAccountSearch.Visible = False
            txtVehicle.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 2
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 385
        '        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 160
        DgAccountSearch.BringToFront() : DgAccountSearch.Visible = True
        retriveAccounts()
    End Sub

    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress, txtItem.KeyPress, txtCharges.KeyPress, txtLotNo.KeyPress
        If txtAccount.Focused = True Then AccountRowColumns()
        If txtItem.Focused = True Then dgItemSearch.Visible = True
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" and upper(SallerName) Like upper('" & txtAccount.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
            Exit Sub
        End If
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select SallerID,SallerName from Transaction2 Where TransType in ('Stock Sale','On Sale','Standard Sale') and SallerID Not in (28)  " & condtion & "Group By SallerID  order by SallerName limit 20 ")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("SallerID").ToString()
                        .Cells(1).Value = clsFun.ExecScalarStr("Select AccountName From Accounts where ID='" & Val(dt.Rows(i)("SallerID").ToString()) & "'")
                        ' .Cells(2).Value = dt.Rows(i)("City").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub VehicleColums()
        dgVehicleNo.ColumnCount = 2
        dgVehicleNo.Columns(0).Name = "Lot" : dgVehicleNo.Columns(0).Visible = False
        dgVehicleNo.Columns(1).Name = "Lot" : dgVehicleNo.Columns(1).Width = 200
        RetrvieVehicle() : dgVehicleNo.Visible = True
    End Sub
    Private Sub RetrvieVehicle(Optional ByVal condtion As String = "")
        dgVehicleNo.Rows.Clear()
        Dim dt As New DataTable
        Dim sql As String = String.Empty
        If BtnSave.Text = "&Save" Then
            sql = "Select VoucherID,VehicleNo,Nug,EntryDate,ifnull((Select PurchaseID From Transaction1 Where PurchaseID=Purchase.VoucherID),0) as SaleVoucherID " &
     "From Purchase where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and  AccountID=" & Val(txtAccountID.Text) & " and StockHolderID<>28  " & condtion & "   Group By VoucherID having SaleVoucherID=0;"
        Else
            sql = "Select VoucherID,VehicleNo,Nug,EntryDate,ifnull((Select PurchaseID From Transaction1 Where PurchaseID=Purchase.VoucherID),0) as SaleVoucherID " &
                "From Purchase where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID=" & Val(txtAccountID.Text) & " and StockHolderID<>28 and VoucherID=" & Val(txtVehicleID.Text) & " " & condtion & "   Group By VoucherID"
        End If
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dgVehicleNo.Rows.Add()
                    With dgVehicleNo.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = dt.Rows(i)("VehicleNo").ToString()
                    End With
                Next
            End If

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        'Dim PurchaseLot As String = ""
        'Dim StockLot As String = ""
        'dgVehicleNo.Rows.Clear()
        'Dim dt As New DataTable
        'Dim tmpval As Integer = 0
        'dt = clsFun.ExecDataTable("Select VoucherID,VehicleNo,Nug,EntryDate From Purchase where AccountID=" & Val(txtAccountID.Text) & " " & condtion & " Group By VoucherID ")
        'Try
        '    If dt.Rows.Count > 0 Then
        '        dgVehicleNo.Rows.Clear()
        '        For i = 0 To dt.Rows.Count - 1
        '            If BtnSave.Text = "&Save" Then
        '                PurchaseLot = clsFun.ExecScalarStr("Select VoucherID From Purchase Where AccountID=" & Val(txtAccountID.Text) & " and VehicleNo='" & dt.Rows(i)("VehicleNo").ToString() & "' Group By VoucherID ")
        '                StockLot = clsFun.ExecScalarStr("Select PurchaseID From Transaction1 Where PurchaseID='" & dt.Rows(i)("VoucherID").ToString() & "' Group By VoucherID")
        '                RestLot = Val(PurchaseLot) - Val(StockLot)
        '                If RestLot > 0 Then
        '                    dgVehicleNo.Rows.Add()

        '                    With dgVehicleNo.Rows(tmpval)
        '                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
        '                        .Cells(1).Value = dt.Rows(i)("VehicleNo").ToString()
        '                        tmpval = tmpval + 1
        '                    End With
        '                End If
        '            Else
        '                dgVehicleNo.Rows.Add()

        '                With dgVehicleNo.Rows(i)
        '                         .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
        '                    .Cells(1).Value = dt.Rows(i)("VehicleNo").ToString()
        '                    '  tmpval = tmpval + 1
        '                End With
        '            End If

        '        Next
        '    End If
        '    dt.Dispose()
        'Catch ex As Exception
        '    MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        'End Try
    End Sub
    Private Sub ItemRowColumns()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 200
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 185
        ' retriveItems()
    End Sub
    Private Sub retriveItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Items " & condtion & " order by ItemName")
        Try
            If dt.Rows.Count > 0 Then
                dgItemSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgItemSearch.Rows.Add()
                    With dgItemSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(2).Value = dt.Rows(i)("OtherName").ToString()
                    End With
                Next

            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub ChargesRowColums()
        dgCharges.ColumnCount = 3
        dgCharges.Columns(0).Name = "ID" : dgCharges.Columns(0).Visible = False
        dgCharges.Columns(1).Name = "Item Name" : dgCharges.Columns(1).Width = 130
        dgCharges.Columns(2).Name = "ApplyType" : dgCharges.Columns(2).Width = 130
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
    Private Sub txtItem_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItem.KeyDown
        If e.KeyCode = Keys.Down Then dgItemSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
        End If
    End Sub
    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        ItemRowColumns()
        If txtItem.Text.Trim() <> "" Then
            dgItemSearch.Visible = True
            retriveItems(" Where upper(ItemName) Like upper('%" & txtItem.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
    End Sub

    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
        ItemRowColumns()
        dgItemSearch.Visible = True
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        'itemfill()
        dgItemSearch.Visible = False
        txtLotNo.Focus()
    End Sub
    'Private Sub itemfill()
    '    If btnSave.Text = "&Save" Then
    '        txtComPer.Text = clsFun.ExecScalarStr(" Select CommisionPer FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        txtMPer.Text = clsFun.ExecScalarStr(" Select UserChargesPer FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        txtTare.Text = clsFun.ExecScalarStr(" Select tare FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        txtLabour.Text = clsFun.ExecScalarStr(" Select Labour FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        txtRdfPer.Text = clsFun.ExecScalarStr(" Select rdfper FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        txtKg.Text = clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        txtOtherItemName.Text = clsFun.ExecScalarStr(" Select OtherName FROM Items WHERE ID='" & txtItemID.Text & "'")
    '    Else
    '        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & txtItemID.Text & "'")
    '        txtOtherItemName.Text = clsFun.ExecScalarStr(" Select OtherName FROM Items WHERE ID='" & txtItemID.Text & "'")
    '    End If
    'End Sub
    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtItem.Clear()
            txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            ' itemfill()
            dgItemSearch.Visible = False
            e.SuppressKeyPress = True
            txtLotNo.Focus()
        End If
        If e.KeyCode = Keys.Back Then
            txtItem.Focus()
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
            txtCharges.Clear() : txtChargeID.Clear()
            txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
            txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
            dgCharges.Visible = False
            txtOnValue.Focus() : FillCharges()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtCharges.Focus()
    End Sub
    Private Sub txtCharges_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyUp
        ChargesRowColums()
        If txtCharges.Text.Trim() <> "" Then
            'dgCharges.Visible = True
            RetriveCharges(" Where upper(ChargeName) Like upper('%" & txtCharges.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgCharges.Visible = False
    End Sub

    Private Sub txtCharges_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCharges.KeyPress
        ChargesRowColums()
        dgCharges.Visible = True
    End Sub
    Private Sub cbBillingType_Leave(sender As Object, e As EventArgs) Handles cbBillingType.Leave
        'If txtVehicleID.Text = "" Then txtVehicle.Focus() : Exit Sub
        If dgVehicleNo.SelectedRows.Count = 0 Then Exit Sub
        If dgVehicleNo.RowCount = 0 Then txtVehicle.Focus() : Exit Sub
        If txtVehicle.Text.ToUpper <> dgVehicleNo.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtVehicle.Focus() : Exit Sub
        If txtVehicle.Text.ToUpper = dgVehicleNo.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtVehicleID.Text = dgVehicleNo.SelectedRows(0).Cells(0).Value
            txtVehicle.Text = dgVehicleNo.SelectedRows(0).Cells(1).Value
            dgVehicleNo.Visible = False
        Else
            txtVehicle.Focus()
        End If
        'If cbBillingType.SelectedIndex = 0 Then
        'PurchaseNugs = clsFun.ExecScalarInt("Select sum(Nug) From Purchase Where VoucherID='" & Val(txtVehicleID.Text) & "' ")
        'SoldNugs = clsFun.ExecScalarInt("Select sum(Nug) From Transaction2 Where PurchaseID='" & Val(txtVehicleID.Text) & "'")
        'If Val(PurchaseNugs) <> Val(SoldNugs) Then
        '    BtnSave.Visible = False
        '    'Exit Sub
        'Else
        '    BtnSave.Visible = True
        'End If
        'Else
        '
        'End If
        FillControls(Val(txtVehicleID.Text))
    End Sub
    Public Sub FillFromData(ByVal id As Integer)
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()

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
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("SallerName").ToString()
            txtVehicle.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txttotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            cbBillingType.Text = ds.Tables("a").Rows(0)("BillingType").ToString()
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
        End If
        '   clsFun.FillDropDownList(CbVehicleNo, "Select * From Purchase Where AccountID='" & Val(txtAccountID.Text) & "' ", "VehicleNo", "VoucherID", "")
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
                txtVehicleID.Text = ds.Tables("b").Rows(i)("PurchaseID").ToString()
                .Rows(i).Cells("GrossWt").Value = ds.Tables("b").Rows(i)("GrossWeight").ToString()
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
                    .Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With

        End If

        Ssqll = "Select * from ChargesTrans where VoucherID=" & Val(txtVehicleID.Text)
        Dim ad3 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds1 As New DataSet
        ad3.Fill(ds1, "c")
        If ds1.Tables("c").Rows.Count > 0 Then
            dg3.Rows.Clear()
            With dg3
                Dim i As Integer = 0
                For i = 0 To ds1.Tables("c").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds1.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On Value").Value = Format(Val(ds1.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds1.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds1.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds1.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds1.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
            btnPnlShow.Visible = True
        End If
        calc()
        dg1.ClearSelection()
        Dg2.ClearSelection()
        dg3.ClearSelection()
        Dim ssqlll As String
        ssqlll = clsFun.ExecScalarStr("Select entrydate from purchase where voucherid=" & Val(txtVehicleID.Text) & "")
        lblFarmerName.Text = clsFun.ExecScalarStr("Select AccountName from Vouchers where ID=" & Val(txtVehicleID.Text) & "")
        lbl.Visible = True : lblPurchaseDate.Visible = True
        lblPurchaseDate.Text = CDate(ssqlll).ToString("dd-MM-yyyy")
    End Sub

    Public Sub FillWithNevigation()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
        If BtnSave.Text = "&Save" And dg1.RowCount > 0 Then MsgBox("Save Transaction First...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnDelete.Enabled = True
        btnPrint.Enabled = True
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Auto Beejak'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers  WHERE transtype = 'Auto Beejak'   Order By ID LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString() + ""
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("SallerName").ToString()
            txtVehicle.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txttotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            cbBillingType.Text = ds.Tables("a").Rows(0)("BillingType").ToString()
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
        End If
        dg1.Rows.Clear()
        Dim sql As String = "Select * from Transaction1 where VoucherID=" & Val(txtid.Text)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        ad1.Fill(ds, "b")

        If ds.Tables("b").Rows.Count > 0 Then
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
                    txtVehicleID.Text = ds.Tables("b").Rows(i)("PurchaseID").ToString()
                Next
            End With
        End If
        Dg2.Rows.Clear()
        'txtItemID.Text = IID
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & Val(txtid.Text)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        ad2.Fill(ds, "c")
        If ds.Tables("c").Rows.Count > 0 Then
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If

        Ssqll = "Select * from ChargesTrans where VoucherID=" & Val(txtVehicleID.Text)
        Dim ad3 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds1 As New DataSet
        ad3.Fill(ds1, "c")
        If ds1.Tables("c").Rows.Count > 0 Then
            dg3.Rows.Clear()
            With dg3
                Dim i As Integer = 0
                For i = 0 To ds1.Tables("c").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds1.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On Value").Value = Format(Val(ds1.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds1.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds1.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds1.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds1.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
            btnPnlShow.Visible = True
        End If
        pnlPaidCharges.Visible = False
        calc()
        dg1.ClearSelection()
        Dg2.ClearSelection()
        dg3.ClearSelection()
        Dim ssqlll As String
        ssqlll = clsFun.ExecScalarStr("Select entrydate from purchase where voucherid=" & Val(txtVehicleID.Text) & "")
        lblFarmerName.Text = clsFun.ExecScalarStr("Select AccountName from Vouchers where ID=" & Val(txtVehicleID.Text) & "")
        If ssqlll = "" Then Exit Sub
        lbl.Visible = True : lblPurchaseDate.Visible = True
        lblPurchaseDate.Text = CDate(ssqlll).ToString("dd-MM-yyyy")
    End Sub
    Private Sub txtVehicle_GotFocus(sender As Object, e As EventArgs) Handles txtVehicle.GotFocus

        If txtAccount.Text.Trim() <> "" Then
            AccountRowColumns()
            retriveAccounts(" and upper(SallerName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            AccountRowColumns()
            retriveAccounts()
        End If
        If DgAccountSearch.SelectedRows.Count=0 then Exit Sub
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        txtVehicle.SelectionStart = 0 : txtVehicle.SelectionLength = Len(txtVehicle.Text)
        VehicleColums() : dgVehicleNo.BringToFront()
        'If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        'If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        'If txtAccount.Text.ToUpper <> DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtAccount.Focus() : Exit Sub
        'If txtAccount.Text.ToUpper = DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
        '    DgAccountSearch.Visible = False
        'Else
        '    txtAccount.Focus()
        'End If
    End Sub

    Private Sub dgVehicleNo_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgVehicleNo.CellClick
        If dgVehicleNo.SelectedRows.Count = 0 Then Exit Sub
        txtVehicle.Clear() : txtVehicleID.Clear()
        txtVehicleID.Text = dgVehicleNo.SelectedRows(0).Cells(0).Value
        txtVehicle.Text = dgVehicleNo.SelectedRows(0).Cells(1).Value
        dgVehicleNo.Visible = False : cbBillingType.Focus()
    End Sub

    Private Sub dgVehicleNo_KeyDown(sender As Object, e As KeyEventArgs) Handles dgVehicleNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dgVehicleNo.SelectedRows.Count = 0 Then Exit Sub
            txtVehicle.Clear() : txtVehicleID.Clear()
            txtVehicleID.Text = dgVehicleNo.SelectedRows(0).Cells(0).Value
            txtVehicle.Text = dgVehicleNo.SelectedRows(0).Cells(1).Value
            dgVehicleNo.Visible = False : cbBillingType.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtVehicle.Focus()
    End Sub

    Private Sub txtVehicle_KeyDown(sender As Object, e As KeyEventArgs) Handles txtVehicle.KeyDown
        If e.KeyCode = Keys.Down Then dgVehicleNo.Focus()
    End Sub

    Private Sub txtVehicle_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtVehicle.KeyPress
        dgVehicleNo.Visible = True
    End Sub

    Private Sub txtVehicle_KeyUp(sender As Object, e As KeyEventArgs) Handles txtVehicle.KeyUp
        VehicleColums()
        If txtVehicle.Text.Trim() <> "" Then
            'dgCharges.Visible = True
            RetrvieVehicle(" And upper(VehicleNo) Like upper('" & txtVehicle.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgCharges.Visible = False
    End Sub

    Private Sub mskEntryDate_Validating1(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
        Dim BackDateEntry As String = clsFun.ExecScalarStr("SELECT DontAllowBack FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Other'")
        If BackDateEntry <> "N" Then
            If mskEntryDate.Text < Date.Today.ToString("dd-MM-yyyy") Then MsgBox("Can't Create Bill Back Date...", MsgBoxStyle.Critical, "Denied Back Date Entries...") : mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy") : Exit Sub
        End If
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        FillWithNevigation()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If Offset = 0 Then
            Offset = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Auto Beejak'  Order By ID ")
        End If
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
        Else
            Offset = 0
        End If

        FillWithNevigation()
    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Auto Beejak'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        Offset = (TotalPages - 1) * RowCount
        FillWithNevigation()
    End Sub

    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub

    Private Sub btnPnlClose_Click(sender As Object, e As EventArgs) Handles btnPnlClose.Click
        pnlPaidCharges.Visible = False : txtCharges.Focus() : btnPnlShow.Visible = True
    End Sub

    Private Sub btnPnlShow_Click(sender As Object, e As EventArgs) Handles btnPnlShow.Click
        pnlPaidCharges.Visible = True : btnPnlShow.Visible = False
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
        PrintOnly() : PrintRecord()
        Pdf_Genrate.ExportReport("\AutoBeejak.rpt")
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        pnlWhatsapp.Visible = False
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
    End Sub
    Private Sub UsingWhatsappAPI()
        If txtWhatsappNo.Text <= "" Then lblStatus.Visible = False : Exit Sub
        lblStatus.Visible = False
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
        PrintOnly() : PrintRecord()
        'Pdf_Genrate.ExportReport("\AutoBeejak.rpt")
        If RadioPrint1.Checked = True Then
            Pdf_Genrate.ExportReport("\AutoBeejak.rpt")
        Else
            Pdf_Genrate.ExportReport("\AutoBeejak2.rpt")
        End If
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Whatsapp\Pdfs\" & GlobalData.PdfName)
        ' UploadFile(FilePath)
        whatsappSender.SendWhatsAppFile("91" & txtWhatsappNo.Text, "Sended By: Aadhat Software" & vbCrLf & "www.softmanagementindia.in", FilePath)
        lblStatus.Text = "PDF Sent " & whatsappSender.APIResposne
        lblStatus.Visible = True
        sql = "insert into waReport(EntryDate,AccountName,WhatsAppNo,Type,Status) SELECT '" & Date.Today.ToString("yyyy-MM-dd") & "','" & txtAccount.Text.Replace("/", "") & "','" & txtWhatsappNo.Text & "','SellOut Auto','" & lblStatus.Text & "'"
        clsFun.ExecNonQuery(sql)
        pnlWhatsapp.Visible = False : mskEntryDate.Focus()
    End Sub
    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
            'MsgBox("A background task is running. you can Use your Task", MsgBoxStyle.Information, "Background Task")
        Else
            MsgBox("A background task is already running.", MsgBoxStyle.Information, "Background Task")
        End If
    End Sub

    Private Sub bgWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        isBackgroundWorkerRunning = True
        Dim action As Action = CType(e.Argument, Action)
        action.Invoke()
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        isBackgroundWorkerRunning = False
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
        pnlWhatsapp.Visible = False : mskEntryDate.Focus()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(txtAccountID.Text) & "'") = "" And txtWhatsappNo.Text <> "" Then
            clsFun.ExecScalarStr("Update Accounts set Mobile1='" & txtWhatsappNo.Text & "' Where ID='" & Val(txtAccountID.Text) & "'")
        Else
            If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(txtAccountID.Text) & "'") <> txtWhatsappNo.Text Then
                If MessageBox.Show("Are you Sure to Change Mobile No In PhoneBook", "Change Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    clsFun.ExecScalarStr("Update Accounts set Mobile1='" & txtWhatsappNo.Text & "' Where ID='" & Val(txtAccountID.Text) & "'")
                End If
            End If
        End If
        If cbType.SelectedIndex = 0 Then
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
        Else
            StartBackgroundTask(AddressOf WahSoft)
        End If
    End Sub
    Private Sub WahSoft()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        ' Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        WABA.ExecNonQuery("Delete from SendingData")

        'pnlWahtsappNo.Visible = True
        'txtWhatsappNo.Focus()
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        PrintOnly() : PrintRecord()
      Pdf_Genrate.ExportReport("\AutoBeejak.rpt")
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & whatsappSender.FilePath & "')"
        If WABA.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            WABA.ExecScalarStr(sql)
        End If
        Dim p() As Process
        p = Process.GetProcessesByName("WahSoft")
        If p.Count = 0 Then
            Dim StartWhatsapp As New System.Diagnostics.Process
            StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\WahSoft\WahSoft.exe"
            StartWhatsapp.Start()
        End If
    End Sub

    Private Sub WhatsAppDesktop()
        If txtWhatsappNo.Text.Trim = "" Then Exit Sub
        Cursor.Current = Cursors.WaitCursor
        Dim GAP1 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP1 From API")) & "000"
        Dim GAP2 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP2 From API")) & "000"
        Dim whatsappURL As String = String.Empty
        Dim sourceFilePath As String = String.Empty
        Try
            whatsappURL = "whatsapp://send?"
            Dim psi As New ProcessStartInfo(whatsappURL)
            Dim process As Process = process.Start(psi)
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try
        If Directory.Exists(Application.StartupPath & "\Whatsapp\Pdfs") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Whatsapp\Pdfs")
        End If
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        PrintOnly() : PrintRecord()
        Pdf_Genrate.ExportReport("\AutoBeejak.rpt")
        sourceFilePath = GlobalData.PdfPath
        whatsappURL = "whatsapp://send?phone=91" & txtWhatsappNo.Text.Trim & ""
        Dim psi1 As New ProcessStartInfo(whatsappURL)
        psi1.UseShellExecute = True
        psi1.WindowStyle = ProcessWindowStyle.Normal
        Dim process1 As Process = Process.Start(psi1)
        psi1.WindowStyle = ProcessWindowStyle.Minimized
        Thread.Sleep(GAP1)
        SendKeys.SendWait("^(+f)")
        SendKeys.SendWait("{ESCAPE}")
        Clipboard.SetData(DataFormats.FileDrop, {sourceFilePath})
        ' If i = 0 Then Thread.Sleep(1000)
        SendKeys.SendWait("^(v)")
        Thread.Sleep(GAP2)
        SendKeys.SendWait("{ENTER}")
        Thread.Sleep(GAP2)
        Dim processName As String = "WhatsApp"
        Dim proc As New ProcessStartInfo(processName)
        Dim processes() As Process = Process.GetProcessesByName(processName)
        If processes.Length > 0 Then
            ' Close each instance of the process
            For Each p As Process In processes
                Thread.Sleep(GAP2)
                p.Kill() : pnlWhatsapp.Visible = False : mskEntryDate.Focus() : mskEntryDate.SelectAll()
                'proc.WindowStyle = ProcessWindowStyle.Minimized
            Next
            MsgBox("Sellout Auto Send to " & txtAccount.Text & " via WhatsApp Successful", MsgBoxStyle.Information, "SellOut Auto Sent")

        Else
            ' The process was not found
            Console.WriteLine("Process not found.")
        End If
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PrintOnly() : PrintRecord()
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
        pnlWhatsapp.Visible = False : txtAccount.Focus()
        If RadioPrint1.Checked = True Then
            Report_Viewer.printReport("\AutoBeejak.rpt")
        Else
            Report_Viewer.printReport("\AutoBeejak2.rpt")
        End If
        'Report_Viewer.printReport("\AutoBeejak.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        Report_Viewer.BringToFront()


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PrintOnly() : PrintRecord()
        pnlWhatsapp.Visible = False
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
        If RadioPrint1.Checked = True Then
            Report_Viewer.printReport("\AutoBeejak.rpt")
        Else
            Report_Viewer.printReport("\AutoBeejak2.rpt")
        End If
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If

    End Sub


    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
    Private Sub ExpSettings()
        If BtnSave.Text <> "&Save" Then Exit Sub
        Dim dt As New DataTable
        Dim sql As String = "Select * From ExpControl Where ApplyOn='Sellout Auto'"
        dt = clsFun.ExecDataTable(sql)
        If dt.Rows.Count = 0 Then Exit Sub
        Dg2.Rows.Clear()
        Try
            For i = 0 To dt.Rows.Count - 1
                Dg2.Rows.Add()
                With Dg2.Rows(i)
                    Dim CalcType As String = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & dt.Rows(i)("ChargesID").ToString() & "'")
                    Dim PlusMinus As String = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & dt.Rows(i)("ChargesID").ToString() & "'")
                    .Cells(5).Value = Val(dt.Rows(i)("ChargesID").ToString())
                    .Cells(0).Value = dt.Rows(i)("ChargesName").ToString()
                    .Cells(3).Value = PlusMinus
                    If CalcType = "Aboslute" Then
                        .Cells(4).Value = dt.Rows(i)("FixAs").ToString()
                    ElseIf CalcType = "Percentage" Then
                        .Cells(2).Value = dt.Rows(i)("FixAs").ToString()
                    ElseIf CalcType = "Nug" Then
                        .Cells(2).Value = dt.Rows(i)("FixAs").ToString()
                    ElseIf CalcType = "Weight" Then
                        .Cells(2).Value = dt.Rows(i)("FixAs").ToString()
                    End If
                End With
            Next
        Catch ex As Exception

        End Try
        Dg2.ClearSelection()
    End Sub
    Private Sub mskEntryDate_Leave(sender As Object, e As EventArgs) Handles mskEntryDate.Leave
        ExpSettings()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub txtchargesAmount_TextChanged(sender As Object, e As EventArgs) Handles txtchargesAmount.TextChanged

    End Sub

    Private Sub txtCalculatePer_TextChanged(sender As Object, e As EventArgs) Handles txtCalculatePer.TextChanged

    End Sub

    Private Sub txtVehicle_TextChanged(sender As Object, e As EventArgs) Handles txtVehicle.TextChanged


    End Sub

    Private Sub txtTotAmount_TextChanged(sender As Object, e As EventArgs) Handles txtTotAmount.TextChanged

    End Sub

    Private Sub txtCharges_TextChanged(sender As Object, e As EventArgs) Handles txtCharges.TextChanged

    End Sub
End Class