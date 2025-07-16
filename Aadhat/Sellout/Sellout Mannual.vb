Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Threading

Public Class Sellout_Mannual
    Dim sql As String = String.Empty
    Dim Vno As Integer : Dim VchId As Integer
    Dim count As Integer = 0
    Dim CalcType As String = String.Empty
    Dim opbal As String = "" : Dim ClBal As String = ""
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 1 : Dim Offset As Integer = 0
    Dim remark As String = String.Empty
    Dim remarkHindi As String = String.Empty
    Dim ServerTag As Integer : Dim LineMargin As Integer
    Dim whatsappSender As New WhatsAppSender()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.BackColor = Color.LightGray
        mskEntryDate.SelectAll()
    End Sub

    Private Sub mskEntryDate_Leave(sender As Object, e As EventArgs) Handles mskEntryDate.Leave
        ExpSettings()
    End Sub
    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 52
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
            .Columns(25).Name = "T1" : .Columns(25).Width = 159
            .Columns(26).Name = "T2" : .Columns(26).Width = 159
            .Columns(27).Name = "T3" : .Columns(27).Width = 159
            .Columns(28).Name = "T4" : .Columns(28).Width = 159
            .Columns(29).Name = "T5" : .Columns(29).Width = 159
            .Columns(30).Name = "T6" : .Columns(30).Width = 159
            .Columns(31).Name = "T7" : .Columns(31).Width = 159
            .Columns(32).Name = "T8" : .Columns(32).Width = 159
            .Columns(33).Name = "T9" : .Columns(33).Width = 159
            .Columns(34).Name = "T10" : .Columns(34).Width = 159
            .Columns(35).Name = "P21" : .Columns(35).Width = 159
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
            .Columns(49).Name = "AddWeight" : .Columns(49).Width = 69
            .Columns(50).Name = "AddWeightGroup" : .Columns(50).Width = 69
            .Columns(51).Name = "PaidAmount" : .Columns(51).Width = 95

        End With
    End Sub
    Sub retrive2()
        Dim i, j As Integer
        Dim dt As New DataTable : Dim dt1 As New DataTable
        Dim dt2 As New DataTable : Dim cnt As Integer = -1
        tmpgrid.Rows.Clear() : Dim sql As String = String.Empty
        LineMargin = clsFun.ExecScalarInt("Select Margin From Controls")
        Dim margin As String = String.Empty
        If LineMargin = 0 Then margin = vbCrLf
        If LineMargin = 1 Then margin = vbCrLf & vbCrLf
        If LineMargin = 2 Then margin = vbCrLf & vbCrLf
        If LineMargin = 3 Then margin = vbCrLf & vbCrLf & vbCrLf
        If LineMargin = 4 Then margin = vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If LineMargin = 5 Then margin = vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        '  dt = clsFun.ExecDataTable("Select * From ScripPrint Where (ID=" & txtid.Text & ")")
        sql = " Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.SallerName, Vouchers.VehicleNo, " _
                                & " Transaction1.ItemName, Transaction1.Lot, Transaction1.Nug as TransNug, Transaction1.Weight, Transaction1.Rate," _
                                & " Transaction1.Per, Transaction1.Amount,Transaction1.AddWeight, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, " _
                                & " Vouchers.T1, Vouchers.T2, Vouchers.T3, Vouchers.T4, Vouchers.T5, Vouchers.T6,Vouchers.T7, Vouchers.T8, Vouchers.T9, Vouchers.T10,Vouchers.T11, " _
                                & " Vouchers.TotalCharges, Items.OtherName, Accounts.OtherName FROM ((Vouchers INNER JOIN Transaction1 ON Vouchers.ID = Transaction1.VoucherID) " _
                                & "    INNER JOIN Items ON Transaction1.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID Where (Vouchers.ID=" & txtid.Text & ")"
        dt = clsFun.ExecDataTable(sql)
        dt2 = clsFun.ExecDataTable(sql)
        tmpgrid.Rows.Clear()
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("SallerName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Lot").ToString()
                    .Cells(8).Value = Format(Val(.Cells(8).Value & dt.Rows(i)("TransNug").ToString()), "0.00")
                    .Cells(9).Value = Format(Val(.Cells(9).Value & dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = Format(Val(.Cells(10).Value & dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = Format(Val(.Cells(12).Value & dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(18).Value = Format(Val(.Cells(18).Value & dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = Format(Val(.Cells(19).Value & dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = Format(Val(.Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = Format(Val(.Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = Format(Val(.Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("OtherName1").ToString()
                    .Cells(50).Value = .Cells(50).Value & dt.Rows(i)("AddWeight").ToString()
                    .Cells(51).Value = clsFun.ExecScalarStr("Select Sum(Amount) From Ledger Where AccountID=" & Val(txtAccountID.Text) & " and DC='D' and EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' ")
                    For k = 0 To dt2.Rows.Count - 1
                        If dt2.Rows.Count > 0 Then
                            .Cells(36).Value = .Cells(36).Value & dt2.Rows(k)("ItemName").ToString() & vbCrLf
                            .Cells(37).Value = .Cells(37).Value & dt2.Rows(k)("Lot").ToString() & vbCrLf
                            .Cells(38).Value = .Cells(38).Value & Format(Val(dt2.Rows(k)("TransNug").ToString())) & vbCrLf
                            .Cells(39).Value = .Cells(39).Value & Format(Val(dt2.Rows(k)("Weight").ToString()), "0.00") & vbCrLf
                            .Cells(40).Value = .Cells(40).Value & Format(Val(dt2.Rows(k)("Rate").ToString()), "0.00") & vbCrLf
                            .Cells(41).Value = .Cells(41).Value & dt2.Rows(k)("Per").ToString() & vbCrLf
                            .Cells(42).Value = .Cells(42).Value & Format(Val(dt2.Rows(k)("Amount").ToString()), "0.00") & vbCrLf
                            .Cells(48).Value = .Cells(48).Value & dt2.Rows(k)("OtherName").ToString() & vbCrLf
                            '.Cells(50).Value = .Cells(50).Value & dt2.Rows(k)("AddWeight").ToString() & vbCrLf
                        End If
                    Next


                    .Cells(25).Value = .Cells(25).Value & dt.Rows(i)("T1").ToString()
                    .Cells(26).Value = .Cells(26).Value & dt.Rows(i)("T2").ToString()
                    .Cells(27).Value = .Cells(27).Value & dt.Rows(i)("T3").ToString()
                    .Cells(28).Value = .Cells(28).Value & dt.Rows(i)("T4").ToString()
                    .Cells(29).Value = .Cells(29).Value & dt.Rows(i)("T5").ToString()
                    .Cells(30).Value = .Cells(30).Value & dt.Rows(i)("T6").ToString()
                    .Cells(31).Value = .Cells(31).Value & dt.Rows(i)("T7").ToString()
                    .Cells(32).Value = .Cells(32).Value & dt.Rows(i)("T8").ToString()
                    .Cells(33).Value = .Cells(33).Value & dt.Rows(i)("T9").ToString()
                    .Cells(34).Value = .Cells(34).Value & dt.Rows(i)("T10").ToString()
                    .Cells(35).Value = .Cells(35).Value & dt.Rows(i)("T11").ToString()

                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    '  Dim ManageSpace As String = String.Empty
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("OnValue").ToString()), "0.00") & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                            .Cells(43).Value = .Cells(43).Value & dt1.Rows(j)("ChargeName").ToString() & margin
                            .Cells(44).Value = .Cells(44).Value & Format(Val(dt1.Rows(j)("OnValue").ToString()), "0.00") & margin
                            .Cells(45).Value = .Cells(45).Value & dt1.Rows(j)("Calculate").ToString() & margin
                            .Cells(46).Value = .Cells(46).Value & dt1.Rows(j)("ChargeType").ToString() & margin
                            .Cells(47).Value = .Cells(47).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & margin
                            .Cells(49).Value = .Cells(49).Value & clsFun.ExecScalarStr("Select PrintName From Charges Where ID ='" & Val(dt1.Rows(j)("ChargesID").ToString()) & "'") & margin
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

    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub

    Private Sub Sellout_Mannual_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isBackgroundWorkerRunning Then
            e.Cancel = True
            Me.Hide()
            ' MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Top = 0 : Me.Left = 0
        End If
    End Sub

    Private Sub Sellout_Mannual_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If isBackgroundWorkerRunning Then
                Me.Hide() : Me.Top = 0 : Me.Left = 0
            Else
                Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "Aadhat")
                If msgRslt = MsgBoxResult.Yes Then
                    Me.Close() : Exit Sub
                ElseIf msgRslt = MsgBoxResult.No Then
                End If
            End If
        End If
    End Sub

    Private Sub scripForm_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then
                txtAccount_KeyUp(sender, e)
                'DgAccountSearch.Visible = False
                Exit Sub
            ElseIf dgItemSearch.Visible = True Then
                dgItemSearch.Visible = False : txtItem.Focus()
                Exit Sub
            ElseIf dgCharges.Visible = True Then
                dgCharges.Visible = False : txtCharges.Focus()
                Exit Sub
            ElseIf pnlSendingDetails.Visible = True Then
                pnlSendingDetails.Visible = False : txtVehicleNo.Focus()
                Exit Sub
            Else
                If dg1.RowCount = 0 Then
                    Me.Close()
                Else
                    Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "Aadhat")
                    If msgRslt = MsgBoxResult.Yes Then
                        Me.Close() : Exit Sub
                    ElseIf msgRslt = MsgBoxResult.No Then
                    End If
                End If
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

    Private Sub scripForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33) order by AccountName ", "AccountName", "ID", "--N./A.--")
        cbCrateTrans.SelectedIndex = 0
        'clsFun.FillDropDownList(CbAccountName, "Select ID, AccountName From Account_AcGrp where (Groupid in(11,17)  or UnderGroupID in (11,17))", "AccountName", "Id", "")
        'clsFun.FillDropDownList(cbitem, "Select * From Items", "ItemName", "Id", "")
        'clsFun.FillDropDownList(CbCharges, "Select * From Charges", "ChargeName", "Id", "")
        Me.KeyPreview = True : Cbper.SelectedIndex = 0
        BtnDelete.Enabled = False : btnPrint.Enabled = False
        rowColums() : FootertextClear() : ExpSettings()
        RadioPrint1.Checked = True
        Cbper.Text = clsFun.ExecScalarStr("Select per From Controls")
    End Sub

    Private Sub AcBal()
        Dim Sql As String = String.Empty
        Sql = "Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
   "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
   " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
   " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where ID=" & Val(txtAccountID.Text) & " Order by upper(AccountName) ;"
        Dim Bal As String = clsFun.ExecScalarStr(Sql)
        If Val(Bal) >= 0 Then
            lblAcBal.Visible = True
            opbal = Format(Math.Abs(Val(Bal)), "0.00") & " Dr"
            lblAcBal.Text = "Bal : " & Format(Math.Abs(Val(Bal)), "0.00") & " Dr"
        Else
            lblAcBal.Visible = True
            opbal = Format(Math.Abs(Val(Bal)), "0.00") & " Cr"
            lblAcBal.Text = "Bal : " & Format(Math.Abs(Val(Bal)), "0.00") & " Cr"
        End If
    End Sub

    Private Sub ExpSettings()
        If BtnSave.Text <> "&Save" Then Exit Sub
        Dg2.Rows.Clear()
        Dim dt As New DataTable
        Dim sql As String = "Select * From ExpControl Where ApplyOn='Sellout Mannual'"
        dt = clsFun.ExecDataTable(sql)
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

    Private Sub ClosingBal()
            Dim Sql As String = String.Empty
        Sql = "Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
   "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
   " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
   " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where ID=" & Val(txtAccountID.Text) & " Order by upper(AccountName) ;"
        Dim Bal As String = clsFun.ExecScalarStr(Sql)
        If Val(Bal) >= 0 Then
            lblAcBal.Visible = True
            opbal = Format(Math.Abs(Val(Bal)), "0.00") & " Dr"
        Else
            lblAcBal.Visible = True
            opbal = Format(Math.Abs(Val(Bal)), "0.00") & " Cr"
        End If
        Sql = "Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
              "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
              " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
              " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where ID=" & Val(txtAccountID.Text) & " Order by upper(AccountName) ;"
        Bal = clsFun.ExecScalarStr(Sql)
        If Val(Bal) >= 0 Then
            lblAcBal.Visible = True
            ClBal = Format(Math.Abs(Val(Bal)), "0.00") & " Dr"
        Else
            lblAcBal.Visible = True
            ClBal = Format(Math.Abs(Val(Bal)), "0.00") & " Cr"
        End If

    End Sub

    Private Sub VNumber()
        If Vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
            Vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = Vno + 1
            txtInvoiceID.Text = Vno + 1
        Else
            Vno = clsFun.ExecScalarInt("Select Max(InvoiceID)  AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            ' Vno = clsFun.ExecScalarInt("SELECT InvoiceID AS NumberOfProducts FROM Vouchers WHERE id=(SELECT max(id) FROM Vouchers Where TransType='" & Me.Text & "')")
            txtVoucherNo.Text = Vno + 1
            txtInvoiceID.Text = Vno + 1
        End If
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 17
        dg1.Columns(0).Name = "Item Name" : dg1.Columns(0).Width = 399
        dg1.Columns(1).Name = "Lot No" : dg1.Columns(1).Width = 199
        dg1.Columns(2).Name = "Nug" : dg1.Columns(2).Width = 114
        dg1.Columns(3).Name = "Weight" : dg1.Columns(3).Width = 114
        dg1.Columns(4).Name = "Rate" : dg1.Columns(4).Width = 114
        dg1.Columns(5).Name = "Per" : dg1.Columns(5).Width = 89
        dg1.Columns(6).Name = "Amount" : dg1.Columns(6).Width = 142
        dg1.Columns(7).Name = "ItemID" : dg1.Columns(7).Width = 142
        dg1.Columns(8).Name = "ID" : dg1.Columns(8).Visible = False
        dg1.Columns(9).Name = "CrateAccountID" : dg1.Columns(9).Width = 142
        dg1.Columns(10).Name = "CrateAccountName" : dg1.Columns(10).Width = 142
        dg1.Columns(11).Name = "CrateID" : dg1.Columns(11).Width = 142
        dg1.Columns(12).Name = "CrateName" : dg1.Columns(12).Width = 142
        dg1.Columns(13).Name = "CrateQty" : dg1.Columns(13).Width = 142
        dg1.Columns(14).Name = "CrateTransType" : dg1.Columns(14).Width = 142
        dg1.Columns(15).Name = "crateYN" : dg1.Columns(15).Width = 142
        dg1.Columns(16).Name = "AddWeight" : dg1.Columns(16).Width = 142
        dg1.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable

        Dg2.ColumnCount = 7
        Dg2.Columns(0).Name = "Charge Name"
        Dg2.Columns(0).Width = 259
        Dg2.Columns(1).Name = "On Value"
        Dg2.Columns(1).Width = 113
        Dg2.Columns(2).Name = "Cal"
        Dg2.Columns(2).Width = 114
        Dg2.Columns(3).Name = "+/-"
        Dg2.Columns(3).Width = 114
        Dg2.Columns(4).Name = "Amount"
        Dg2.Columns(4).Width = 114
        Dg2.Columns(5).Name = "ChargeID"
        Dg2.Columns(5).Width = 110
        Dg2.Columns(6).Name = "CostOn"
        Dg2.Columns(6).Visible = False
    End Sub
    Sub calc()
        txtTotalNug.Text = Format(Val(0), "0.00") : txttotalWeight.Text = Format(Val(0), "0.00")
        txtbasicTotal.Text = Format(Val(0), "0.00") : txttotalCharges.Text = Format(Val(0), "0.00")
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
        'For i = 0 To Dg2.Rows.Count - 1
        '    Dim CalcType As String = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
        '    Dim PlusMinus As String = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
        '    If CalcType = "Aboslute" Then
        '        Dg2.Rows(i).Cells(4).Value = Dg2.Rows(i).Cells(4).Value
        '    ElseIf CalcType = "Percentage" Then
        '        '    Dg2.Rows(i).Cells(1).Value = Format(Val(txtbasicTotal.Text), "0.00")
        '        Dg2.Rows(i).Cells(4).Value = Format(Val(Val(Dg2.Rows(i).Cells(1).Value) * Val(Dg2.Rows(i).Cells(2).Value) / 100), "0.00")
        '    ElseIf CalcType = "Nug" Then
        '        '   Dg2.Rows(i).Cells(1).Value = Format(Val(txtTotalNug.Text), "0.00")
        '        Dg2.Rows(i).Cells(4).Value = Format(Val(Dg2.Rows(i).Cells(1).Value) * Val(Dg2.Rows(i).Cells(2).Value), "0.00")
        '    ElseIf CalcType = "Weight" Then
        '        '  Dg2.Rows(i).Cells(1).Value = Format(Val(txttotalWeight.Text), "0.00")
        '        Dg2.Rows(i).Cells(4).Value = Format(Val(Dg2.Rows(i).Cells(1).Value) * Val(Dg2.Rows(i).Cells(2).Value), "0.00")
        '    End If
        '    If Dg2.Rows(i).Cells(3).Value = "-" Then
        '        txttotalCharges.Text = Format(Val(txttotalCharges.Text) - Val(Dg2.Rows(i).Cells(4).Value), "0.00")
        '    Else
        '        txttotalCharges.Text = Format(Val(txttotalCharges.Text) + Val(Dg2.Rows(i).Cells(4).Value), "0.00")
        '    End If
        'Next
        txttotalNetAmount.Text = Format(Val(Val(txtbasicTotal.Text)) + Val(Val(txttotalCharges.Text)), "0.00")
        Dim tmpamount As Double = Val(txttotalNetAmount.Text)
        txttotalNetAmount.Text = Math.Round(Val(tmpamount), 0)
        txtroundoff.Text = Format(Val(txttotalNetAmount.Text) - Val(tmpamount), "0.00")
        txttotalNetAmount.Text = Format(Val(txttotalNetAmount.Text), "0.00")
    End Sub
    'Private Sub CbCustomer_Leave(sender As Object, e As EventArgs)
    '    If clsFun.ExecScalarInt("Select count(*)from Accounts") = 0 Then
    '        Exit Sub
    '    End If
    '    If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & CbAccountName.Text & "'") = 0 Then
    '        MsgBox("Account Not Found in Database...", vbOkOnly, "Access Denied")
    '        CbAccountName.Focus()
    '        Exit Sub
    '    End If
    'End Sub
    'Private Sub fillCharges(ByVal id As Integer)
    '    Dim dt As New DataTable
    '    dt = clsFun.ExecDataTable("Select * from Charges where Id=" & id & "")
    '    If dt.Rows.Count > 0 Then
    '        txtCharges.Text = dt.Rows(0)("ChargeName").ToString()
    '        txtCalculatePer.Text = dt.Rows(0)("Calculate").ToString()
    '        txtPlusMinus.Text = dt.Rows(0)("ChargesType").ToString()
    '        lblClcType.Text = dt.Rows(0)("ApplyType").ToString()
    '        txtChargeID.Text = dt.Rows(0)("id").ToString()
    '        If dt.Rows(0)("ApplyType").ToString() = "Nug" Then
    '            txtOnValue.Text = txtTotalNug.Text
    '            txtOnValue.TabStop = True
    '            txtCalculatePer.TabStop = True
    '        ElseIf dt.Rows(0)("ApplyType").ToString() = "Percentage" Then
    '            txtOnValue.Text = txtbasicTotal.Text
    '            txtOnValue.TabStop = True
    '            txtCalculatePer.TabStop = True
    '        ElseIf dt.Rows(0)("ApplyType").ToString() = "Weight" Then
    '            txtOnValue.Text = txttotalWeight.Text
    '            txtOnValue.TabStop = True
    '            txtCalculatePer.TabStop = True
    '        ElseIf dt.Rows(0)("ApplyType").ToString() = "Aboslute" Then
    '            txtOnValue.TabStop = False
    '            txtCalculatePer.TabStop = False
    '            txtOnValue.Text = ""
    '            txtCalculatePer.Text = ""
    '        End If
    '    End If
    'End Sub
    Private Sub FillCharges()
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        If CalcType = "Aboslute" Then
            txtOnValue.BackColor = Color.GhostWhite
            txtOnValue.TabStop = False
            txtCalculatePer.TabStop = False
            txtOnValue.Text = ""
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
        End If
    End Sub

    Private Sub PrintRecord()
        Dim FastQuery As String = String.Empty
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," &
                                "'" & lblInword.Text & "','" & txtroundoff.Text & "','" & opbal & "','" & ClBal & "'," &
                                "'" & .Cells(25).Value & "','" & .Cells(26).Value & "','" & .Cells(27).Value & "','" & .Cells(28).Value & "', " &
                                "'" & .Cells(29).Value & "','" & .Cells(30).Value & "','" & .Cells(31).Value & "','" & .Cells(32).Value & "'," &
                                "'" & .Cells(33).Value & "','" & .Cells(34).Value & "','" & .Cells(35).Value & "','" & .Cells(36).Value & "', " & _
                                "'" & .Cells(37).Value & "','" & .Cells(38).Value & "','" & .Cells(39).Value & "','" & .Cells(40).Value & "'," & _
                                "'" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "','" & .Cells(44).Value & "'," & _
                                "'" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "', " & _
                                "'" & .Cells(49).Value & "','" & .Cells(50).Value & "','" & .Cells(51).Value & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = sql & "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " &
                       " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, P21,P22,P23,P24,P25,P26,P27,P28,P29, " &
                       " P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42,P43,P34,P45,P46,P47,P48,P49,P50,P51,P52)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try

    End Sub
    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : AcBal()

        If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
        If dgItemSearch.RowCount = 0 Then retriveAccounts()
        If txtItem.Text.Trim() <> "" Then
            retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            retriveItems()
        End If
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True
        txtItem.SelectionStart = 0 : txtItem.SelectionLength = Len(txtItem.Text)

        txtItem.Focus()
    End Sub

    Private Sub txtLotNo_GotFocus(sender As Object, e As EventArgs) Handles txtLotNo.GotFocus
        If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
        If dgItemSearch.Rows.Count = 0 Then retriveItems()
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus() : Exit Sub
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        dgItemSearch.Visible = False
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        dgCharges.Visible = False : dgItemSearch.Visible = False
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        txtAccount.SelectAll()
    End Sub

    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        txtNug.SelectionStart = 0 : txtNug.SelectionLength = Len(txtNug.Text)
        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
        If lblCrate.Text = "Y" Then
            If txtAccountID.Text = 7 Then
                cbAccountName.Enabled = True
                cbAccountName.SelectedIndex = 0
                txtCrateQty.Text = txtNug.Text
            Else
                cbAccountName.Enabled = False
                cbAccountName.SelectedIndex = -1
                txtCrateQty.Text = Val(txtNug.Text)
            End If
            pnlMarka.Visible = True
            txtCrateQty.Text = Val(txtNug.Text)
        Else
            pnlMarka.Visible = False
            txtCrateQty.Clear() : cbCrateMarka.Text = ""
            cbCrateMarka.SelectedValue = 0
            txtCrateQty.Text = 0
            cbCrateTrans.Text = ""
            'pnlMarka.BringToFront()
        End If
    End Sub
    Private Sub txtKg_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus, txtAccount.GotFocus,
      txtVoucherNo.GotFocus, txtVehicleNo.GotFocus, txtLotNo.GotFocus, txtNug.GotFocus, txtKg.GotFocus, txtRate.GotFocus,
     txtCrateQty.GotFocus, txtCustName.GotFocus, txtCustMobile.GotFocus, txtPaymentTerm.GotFocus, txtBrokerName.GotFocus, txtBrokerMob.GotFocus,
      txtTransPort.GotFocus, txtDriverName.GotFocus, txtGrNo.GotFocus, txtDriverMobile.GotFocus, txtRemark.GotFocus, txtBuiltyNo.GotFocus
        If txtKg.Focused = True Then
            If txtAddWeight.Text = "" Then lblAddWeight.Text = "" Else lblAddWeight.Text = txtAddWeight.Text
            lblAddWeight.Visible = True
        End If
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtItem.KeyDown, txtAccount.KeyDown,
        txtVoucherNo.KeyDown, txtVehicleNo.KeyDown, txtLotNo.KeyDown, txtNug.KeyDown, txtKg.KeyDown, txtRate.KeyDown, Cbper.KeyDown,
        cbAccountName.KeyDown, cbCrateMarka.KeyDown, txtCrateQty.KeyDown, txtCustName.KeyDown, txtCustMobile.KeyDown, txtPaymentTerm.KeyDown,
        txtBrokerName.KeyDown, txtBrokerMob.KeyDown, txtTransPort.KeyDown, txtDriverName.KeyDown, txtGrNo.KeyDown, txtDriverMobile.KeyDown, txtRemark.KeyDown, txtBuiltyNo.KeyDown
        If txtKg.Focused Then
            If e.KeyCode = Keys.F3 Then
                pnlAddWeight.Visible = True
                txtAddWeight.Focus()
            End If
        End If

        If txtVoucherNo.Focused Then
            If e.KeyCode = Keys.F2 Then
                pnlInvoiceID.Visible = True
                txtInvoiceID.Focus()
                e.SuppressKeyPress = True
            End If
        End If
        If txtRemark.Focused Then
            If e.KeyCode = Keys.Enter Then
                pnlSendingDetails.Visible = False
                pnlSendingDetails.BringToFront()
                txtAccount.Focus() : Exit Sub
            End If
        End If
        If txtVehicleNo.Focused Then
            If e.KeyCode = Keys.F3 Then
                pnlSendingDetails.Visible = True
                txtCustName.Focus()
                e.SuppressKeyPress = True
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            'SendKeys.Send("{TAB}")
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
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
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.Down Then
                If dgItemSearch.Visible = True Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If dgItemSearch.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or Cbper.Focused = True Or cbCrateMarka.Focused = True Or cbAccountName.Focused = True Or txtCrateQty.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
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
    Private Sub SpeedCalculation()
        If Cbper.SelectedIndex = 0 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtNug.Text) * Val(txtRate.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtKg.Text) * Val(txtRate.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 5 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 3 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 10 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 4 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 20 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 5 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 40 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 6 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 41 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 7 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 50 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 8 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 51 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 9 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 51.7 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 10 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 52.3 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 11 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 53 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 12 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 80 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 13 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) / 100 * Val(txtKg.Text), 2), "0.00")
        ElseIf Cbper.SelectedIndex = 14 Then
            txtTotAmount.Text = Format(Math.Round(Val(txtRate.Text) * Val(txtNug.Text), 2), "0.00")
        End If
        txtTotAmount.Text = Math.Round(Val(txtTotAmount.Text), 0, MidpointRounding.AwayFromZero)
        ' txtTotAmount.Text = Format(Math.Round(Val(txtTotAmount.Text), 0), "0.00")
        txttotalNetAmount.Text = Format(Val(txtbasicTotal.Text) + Val(txttotalCharges.Text), "0.00")
    End Sub

    Private Sub txtTotAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotAmount.KeyDown
        If Val(txtItemID.Text) = 0 Then txtItem.Focus() : Exit Sub
        If e.KeyCode = Keys.Enter Then
            If Val(txtNug.Text) = 0 And Val(txtKg.Text) = 0 Then
                MsgBox("please fill Nug or Kg", MsgBoxStyle.Critical, "Access Denied")
                txtNug.Focus()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(0).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(1).Value = txtLotNo.Text
                dg1.SelectedRows(0).Cells(2).Value = Format(Val(txtNug.Text), "0.00")
                dg1.SelectedRows(0).Cells(3).Value = Format(Val(txtKg.Text), "0.00")
                dg1.SelectedRows(0).Cells(4).Value = Format(Val(txtRate.Text), "0.00")
                dg1.SelectedRows(0).Cells(5).Value = Cbper.Text
                dg1.SelectedRows(0).Cells(6).Value = Format(Val(txtTotAmount.Text), "0.00")
                dg1.SelectedRows(0).Cells(7).Value = txtItemID.Text
                dg1.SelectedRows(0).Cells(9).Value = Val(cbAccountName.SelectedValue)
                dg1.SelectedRows(0).Cells(10).Value = cbAccountName.Text
                dg1.SelectedRows(0).Cells(11).Value = Val(cbCrateMarka.SelectedValue)
                dg1.SelectedRows(0).Cells(12).Value = cbCrateMarka.Text
                dg1.SelectedRows(0).Cells(13).Value = txtCrateQty.Text
                dg1.SelectedRows(0).Cells(14).Value = cbCrateTrans.Text
                dg1.SelectedRows(0).Cells(15).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(16).Value = txtAddWeight.Text.Trim

                txtItem.Focus()
                calc()
                dg1.ClearSelection()
                cleartxt()
            Else
                dg1.Rows.Add(txtItem.Text, txtLotNo.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtKg.Text), "0.00"), Format(Val(txtRate.Text), "0.00"),
                             Cbper.Text, Format(Val(txtTotAmount.Text), "0.00"), Val(txtItemID.Text), "", Val(cbAccountName.SelectedValue), cbAccountName.Text,
                             Val(cbCrateMarka.SelectedValue), cbCrateMarka.Text, txtCrateQty.Text, cbCrateTrans.Text, lblCrate.Text, txtAddWeight.Text.Trim)
                calc()
                cleartxt()
                txtItem.Focus()
                dg1.ClearSelection()
            End If
        End If
    End Sub
    Private Sub cleartxt()
        txtLotNo.Text = "" : txtNug.Text = ""
        txtKg.Text = "" : txtTotAmount.Text = ""
        txtAddWeight.Text = "" : lblAddWeight.Visible = False
    End Sub
    Private Sub FootertextClear()
        txtTotalNug.Text = "" : txtbasicTotal.Text = ""
        txttotalWeight.Text = "" : txttotalNetAmount.Text = ""
        txttotalCharges.Text = "" : txtroundoff.Text = ""
        txtRate.Text = "" : VNumber()
        txtCustName.Text = "" : txtCustMobile.Text = ""
        txtPaymentTerm.Text = "" : txtBrokerName.Text = ""
        txtBrokerMob.Text = "" : txtTransPort.Text = ""
        txtDriverName.Text = "" : txtGrNo.Text = ""
        txtDriverMobile.Text = "" : txtRemark.Text = ""
        txtCustName.Text = "" : dg1.Rows.Clear()
        pnlWhatsapp.Visible = False : BtnSave.Text = "&Save"
        BtnDelete.Enabled = False
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
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text) / 100), "0.00")
        ElseIf CalcType = "Nug" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text)), "0.00")
        ElseIf CalcType = "Weight" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text)), "0.00")
        ElseIf CalcType = "Aboslute" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(CDbl(txtOnValue.Text) * CDbl(txtCalculatePer.Text)), "0.00")
        End If
    End Sub
    Private Sub Cbper_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbper.SelectedIndexChanged
        SpeedCalculation()
    End Sub

    Private Sub txtOnValue_GotFocus(sender As Object, e As EventArgs) Handles txtOnValue.GotFocus
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        dgCharges.Visible = False : FillCharges()
    End Sub

    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtKg.KeyPress,
        txtRate.KeyPress, txtTotAmount.KeyPress, txttotalCharges.KeyPress, txtOnValue.KeyPress, txtCalculatePer.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave
        If txtNug.Text = "" Then txtNug.Text = Val(0)
    End Sub

    Private Sub txtKg_Leave(sender As Object, e As EventArgs) Handles txtKg.Leave
        If txtKg.Text = "" Then txtKg.Text = Val(0)
    End Sub

    Private Sub txtRate_Leave(sender As Object, e As EventArgs) Handles txtRate.Leave
        If txtRate.Text = "" Then txtRate.Text = Val(0)
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
        txtchargesAmount.BackColor = Color.LightGray
    End Sub

    Private Sub txtNug_KeyUp(sender As Object, e As EventArgs) Handles txtNug.KeyUp, Cbper.KeyUp,
        txtKg.KeyUp, txtRate.KeyUp, txtchargesAmount.KeyUp,
        txtbasicTotal.KeyUp, txtCalculatePer.KeyUp,
       txtOnValue.KeyUp, txttotalCharges.KeyUp
        ChargesCalculation() : SpeedCalculation()
        txttotalNetAmount.Text = Format(Val(Val(txtbasicTotal.Text)) + Val(Val(txttotalCharges.Text)), "0.00")
        Dim tmpamount As Double = Val(txttotalNetAmount.Text)
        txttotalNetAmount.Text = Math.Round(Val(tmpamount), 0, MidpointRounding.AwayFromZero)
        'txttotalNetAmount.Text = Math.Round(Val(tmpamount), 0)
        txtroundoff.Text = Format(Val(txttotalNetAmount.Text) - Val(tmpamount), "0.00")
        txttotalNetAmount.Text = Format(Val(txttotalNetAmount.Text), "0.00")
    End Sub
    Private Sub Save()
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        If dg1.Rows.Count = 0 Then Exit Sub : MsgBox("There is no record to Save.", vbOKOnly, "Empty Record")
        dg1.ClearSelection()
        Dim cmd As SQLite.SQLiteCommand
        sql = "insert into Vouchers(TransType,BillNo,VehicleNo, Entrydate, " _
                                    & "SallerID, Sallername, Nug, kg,BasicAmount, TotalAmount,TotalCharges,InvoiceiD,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11)" _
                                    & "values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10,@11,@12,@13, @14, @15, @16, @17, @18, @19, @20, @21, @22, @23)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@3", txtVehicleNo.Text)
            cmd.Parameters.AddWithValue("@4", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@5", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@6", txtAccount.Text)
            cmd.Parameters.AddWithValue("@7", txtTotalNug.Text)
            cmd.Parameters.AddWithValue("@8", txttotalWeight.Text)
            cmd.Parameters.AddWithValue("@9", txtbasicTotal.Text)
            cmd.Parameters.AddWithValue("@10", txttotalNetAmount.Text)
            cmd.Parameters.AddWithValue("@11", txttotalCharges.Text)
            cmd.Parameters.AddWithValue("@12", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@13", txtCustName.Text)
            cmd.Parameters.AddWithValue("@14", txtCustMobile.Text)
            cmd.Parameters.AddWithValue("@15", txtPaymentTerm.Text)
            cmd.Parameters.AddWithValue("@16", txtBrokerName.Text)
            cmd.Parameters.AddWithValue("@17", txtBrokerMob.Text)
            cmd.Parameters.AddWithValue("@18", txtTransPort.Text)
            cmd.Parameters.AddWithValue("@19", txtDriverName.Text)
            cmd.Parameters.AddWithValue("@20", txtGrNo.Text)
            cmd.Parameters.AddWithValue("@21", txtDriverMobile.Text)
            cmd.Parameters.AddWithValue("@22", txtRemark.Text)
            cmd.Parameters.AddWithValue("@23", txtBuiltyNo.Text)
            If cmd.ExecuteNonQuery() > 0 Then
            End If
            clsFun.CloseConnection()
            txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
            dg1Record() : dg2Record() : ServerTag = 1
            SellerLedger() : ChargesLedger() : CrateLedger()
            ServerSellerLedger() : ServerChargesLedger() : ServerCrateLedger()
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            retrive2()
            mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub RemarkNaration()
        remark = String.Empty
        remarkHindi = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                remark = remark & .Cells("Item Name").Value & " Lot No. : " & .Cells("Lot No").Value & ", Nug. : " & Format(Val(.Cells("Nug").Value), "0.00") & ",Weight : " & Format(Val(.Cells("Weight").Value), "0.00") & ",Rate : " & Format(Val(.Cells("Rate").Value), "0.00") & "/- " & .Cells("Per").Value & " =" & Format(Val(.Cells("Amount").Value), "0.00") & "" & vbCrLf
                Dim othername As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID='" & Val(.Cells("ItemID").Value) & "' ")
                remarkHindi = remarkHindi & othername & " Lot No. : " & .Cells("Lot No").Value & ", नग : " & Format(Val(.Cells("Nug").Value), "0.00") & ",वजन : " & Format(Val(.Cells("Weight").Value), "0.00") & ",भाव : " & Format(Val(.Cells("Rate").Value), "0.00") & "/- " & .Cells("Per").Value & "=" & Format(Val(.Cells("Amount").Value), "0.00") & "" & vbCrLf
            End With
        Next
    End Sub
    Sub SellerLedger()
        Dim FastQuery As String = String.Empty
        Dim tmpamount As Decimal = Val(txtbasicTotal.Text)
        Dim tmpamount2 As Decimal = Val(txtbasicTotal.Text)
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID=" & Val(.Cells(5).Value) & "") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID=" & Val(.Cells(5).Value) & "") = "Our Cost" Then ''our coast
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
        If Val(txtbasicTotal.Text) > 0 Then ''Manual Beejak Account Fixed
            '   clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 46, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46"), Val(tmpamount2), "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 46 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Val(tmpamount2) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text.Trim & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text.Trim, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
        Else
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 46, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46"), Math.Abs(Val(tmpamount2)), "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 46 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Math.Abs(Val(tmpamount2)) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text.Trim, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
        End If
        If Val(txttotalNetAmount.Text) > 0 Then ''Account 
            '        clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text.Trim & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text.Trim, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

        Else
            '            clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text.Trim & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text.Trim, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) < 0 Then
                '               clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtroundoff.Text)), "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text.Trim & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
            Else
                '          clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Val(txtroundoff.Text), "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & remark, txtAccount.Text, remarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text.Trim & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text.Trim, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

            End If
        End If
        If FastQuery = "" Then Exit Sub
        clsFun.FastLedger(FastQuery)

    End Sub

    Sub ServerSellerLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim tmpamount As Decimal = Val(txtbasicTotal.Text)
        Dim tmpamount2 As Decimal = Val(txtbasicTotal.Text)
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID=" & Val(.Cells(5).Value) & "") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID=" & Val(.Cells(5).Value) & "") = "Our Cost" Then ''our coast
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
        If Val(txtbasicTotal.Text) > 0 Then ''Manual Beejak Account Fixed
            '   clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 46, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46"), Val(tmpamount2), "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 46 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Val(tmpamount2) & "', 'D'," & Val(ServerTag) & ", " & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
        Else
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 46, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46"), Math.Abs(Val(tmpamount2)), "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 46 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Math.Abs(Val(tmpamount2)) & "', 'C'," & Val(ServerTag) & ", " & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
        End If
        If Val(txttotalNetAmount.Text) > 0 Then ''Account 
            '        clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'C'," & Val(ServerTag) & ", " & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

        Else
            '            clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'D'," & Val(ServerTag) & ", " & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) < 0 Then
                '               clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtroundoff.Text)), "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark, txtAccount.Text, remarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'C'," & Val(ServerTag) & ", " & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
            Else
                '          clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Val(txtroundoff.Text), "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & remark, txtAccount.Text, remarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'D'," & Val(ServerTag) & ", " & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

            End If
        End If
        If FastQuery = "" Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub

    Sub dg1Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells("Item Name").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtid.Text) & "," &
                             "'" & .Cells("Item Name").Value & "','" & .Cells("Lot No").Value & "'," & Val(.Cells("Nug").Value) & "," &
                             "" & Val(.Cells("Weight").Value) & "," & Val(.Cells("rate").Value) & ", " &
                              "'" & .Cells("Per").Value & "','" & Val(.Cells("Amount").Value) & "','" & Val(.Cells("ItemID").Value) & "', " &
                              "'" & Val(.Cells("CrateAccountID").Value) & "','" & .Cells("CrateAccountName").Value & "', " &
                              "'" & Val(.Cells("CrateID").Value) & "','" & .Cells("CrateName").Value & "','" & Val(.Cells("CrateQty").Value) & "', " &
                              "'" & .Cells("CrateTransType").Value & "','" & .Cells("CrateYN").Value & "','" & .Cells("AddWeight").Value & "'"
                End If
            End With
        Next
        Try
            Sql = "insert into Transaction1(VoucherID, ItemName,Lot, Nug, Weight, Rate, Per,Amount,ItemID,CrateAccountID,CrateAccountName,CrateID, " &
     "CrateMarka,CrateQty,CrateTransType,MaintainCrate,AddWeight) " & FastQuery & ""
            If FastQuery = String.Empty Then Exit Sub
            If clsFun.ExecNonQuery(Sql) > 0 Then count = +1
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        clsFun.CloseConnection()
    End Sub
    Sub ChargesLedger()
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim CostON As String = clsFun.ExecScalarStr(" Select CostOn FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            '         clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"
                        Else
                            '        clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"

                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"

                        Else
                            '    clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"

                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = "" Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub

    Sub ServerChargesLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim CostON As String = clsFun.ExecScalarStr(" Select CostOn FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            '         clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"
                        Else
                            '        clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"

                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(5).Value & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"

                        Else
                            '    clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "D", "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ""))
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "','" & AccName & "','" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, "") & "'"

                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = "" Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub

    Private Sub dg2Record()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty

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
            If FastQuery = "" Then Exit Sub
            sql = "insert into ChargesTrans(VoucherID, ChargesID, ChargeName, OnValue, Calculate, ChargeType, Amount) " & FastQuery & ""
            If clsFun.ExecNonQuery(sql) > 0 Then count = +1
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
        'Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicleNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  SallerID='" & txtAccountID.Text & "', Sallername='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtbasicTotal.Text & "', TotalAmount='" & txttotalNetAmount.Text & "',TotalCharges='" & txttotalCharges.Text & "' ," _
                                & " InvoiceID='" & Val(txtInvoiceID.Text) & "',T1='" & txtCustName.Text & "',T2='" & txtCustMobile.Text & "',T3='" & txtPaymentTerm.Text & "', " _
                                & " T4='" & txtBrokerName.Text & "',T5='" & txtBrokerMob.Text & "',T6='" & txtTransPort.Text & "', T7='" & txtDriverName.Text & "', " _
                                & "T8='" & txtGrNo.Text & "',T9='" & txtDriverMobile.Text & "',T10='" & txtRemark.Text & "',T11='" & txtBuiltyNo.Text & "' where ID =" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then

                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 1 : UpdateCrate()
                If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from Transaction1 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                       "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ";") > 0 Then

                End If
                dg1Record() : dg2Record() : ServerTag = 1
                SellerLedger() : ChargesLedger() : CrateLedger()
                ServerSellerLedger() : ServerChargesLedger() : ServerCrateLedger()
            End If
            BtnSave.Text = "&Save" : BtnDelete.Enabled = False
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            clsFun.CloseConnection()
            retrive2()
            mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
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
    Private Sub ServerCrateLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells(15).Value = "Y" Then ''Party Account
                    If .Cells(14).Value <> "" Then
                        If Val(txtAccountID.Text) = 7 Then
                            If Val(cbAccountName.SelectedValue) > 0 Then
                                '         clsFun.CrateLedger(0, Val(txtid.Text), Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, cbAccountName.SelectedValue, cbAccountName.Text, .Cells(14).Value, Val(.Cells(11).Value), .Cells(12).Value, Val(.Cells(13).Value), "", "", "", "")
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(cbAccountName.SelectedValue) & ",'" & cbAccountName.Text & "','" & .Cells(14).Value & "'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '','','',''," & Val(ServerTag) & ", " & Val(OrgID) & ""

                            End If
                        Else
                            '                            clsFun.CrateLedger(0, Val(txtid.Text), Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, .Cells(14).Value, Val(.Cells(11).Value), .Cells(12).Value, Val(.Cells(13).Value), "", "", "", "")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & .Cells(14).Value & "'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '','','',''," & Val(ServerTag) & ", " & Val(OrgID) & ""
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(FastQuery)
    End Sub
    Private Sub CrateLedger()
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells(15).Value = "Y" Then ''Party Account
                    If .Cells(14).Value <> "" Then
                        If Val(txtAccountID.Text) = 7 Then
                            If Val(cbAccountName.SelectedValue) > 0 Then
                                '         clsFun.CrateLedger(0, Val(txtid.Text), Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, cbAccountName.SelectedValue, cbAccountName.Text, .Cells(14).Value, Val(.Cells(11).Value), .Cells(12).Value, Val(.Cells(13).Value), "", "", "", "")
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(cbAccountName.SelectedValue) & ",'" & cbAccountName.Text & "','" & .Cells(14).Value & "'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '','','',''"

                            End If
                        Else
                            '                            clsFun.CrateLedger(0, Val(txtid.Text), Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, .Cells(14).Value, Val(.Cells(11).Value), .Cells(12).Value, Val(.Cells(13).Value), "", "", "", "")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & .Cells(14).Value & "'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '','','',''"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastCrateLedger(FastQuery)
    End Sub

    Public Sub UpdateMulti()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim count As Integer = 0
        dg1.ClearSelection()
        ' Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicleNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  SallerID='" & txtAccountID.Text & "', Sallername='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtbasicTotal.Text & "', TotalAmount='" & txttotalNetAmount.Text & "',TotalCharges='" & txttotalCharges.Text & "' ," _
                                & "InvoiceID='" & Val(txtInvoiceID.Text) & "' where ID =" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 1 : UpdateCrate()
                If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from Transaction1 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                  "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
                End If
                VchId = Val(txtid.Text)
                dg1Record() : dg2Record() : ServerTag = 1
                SellerLedger() : ChargesLedger() : CrateLedger()
            End If
            BtnSave.Text = "&Save"
            BtnDelete.Enabled = False
            cleartxt() : cleartxtCharges()
            FootertextClear() : dg1.Rows.Clear()
            Dg2.Rows.Clear()
            mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub Delete()
        Dim RemoveSellout As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
        If RemoveSellout <> "Y" Then MsgBox("You Don't Have Rights to Delete Sellout Bill... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Try
            If MessageBox.Show("Are you Sure want to Delete Mannual Sellout...??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & ";DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from Transaction1 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                       "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
                    ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                           "Delete From  CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                    ServerTag = 0 : ServerSellerLedger() : ServerChargesLedger() : ServerCrateLedger()
                    MsgBox("Mannual Sellout Successfully deleted", MsgBoxStyle.Information, "Deleted")
                End If
                cleartxt() : cleartxtCharges() : FootertextClear()
                dg1.Rows.Clear() : Dg2.Rows.Clear() : BtnDelete.Enabled = False
                BtnSave.Text = "&Save" : mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub FillContros(ByVal id As Integer)
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
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("SallerName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txttotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtCustName.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtCustMobile.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtPaymentTerm.Text = ds.Tables("a").Rows(0)("T3").ToString()
            txtBrokerName.Text = ds.Tables("a").Rows(0)("T4").ToString()
            txtBrokerMob.Text = ds.Tables("a").Rows(0)("T5").ToString()
            txtTransPort.Text = ds.Tables("a").Rows(0)("T6").ToString()
            txtDriverName.Text = ds.Tables("a").Rows(0)("T7").ToString()
            txtGrNo.Text = ds.Tables("a").Rows(0)("T8").ToString()
            txtDriverMobile.Text = ds.Tables("a").Rows(0)("T9").ToString()
            txtRemark.Text = ds.Tables("a").Rows(0)("T10").ToString()
            txtBuiltyNo.Text = ds.Tables("a").Rows(0)("T11").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
            txtid.Text = Val(ds.Tables("a").Rows(0)("ID").ToString())
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
                .Rows(i).Cells("CrateAccountID").Value = Val(ds.Tables("b").Rows(i)("CrateAccountID").ToString())
                .Rows(i).Cells("CrateAccountName").Value = ds.Tables("b").Rows(i)("CrateAccountName").ToString()
                .Rows(i).Cells("CrateID").Value = Val(ds.Tables("b").Rows(i)("CrateID").ToString())
                .Rows(i).Cells("CrateName").Value = ds.Tables("b").Rows(i)("CrateMarka").ToString()
                .Rows(i).Cells("CrateQty").Value = ds.Tables("b").Rows(i)("CrateQty").ToString()
                .Rows(i).Cells("CrateTransType").Value = ds.Tables("b").Rows(i)("CrateTransType").ToString()
                .Rows(i).Cells("CrateYN").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells("AddWeight").Value = ds.Tables("b").Rows(i)("AddWeight").ToString()
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
    Public Sub FillWithNevigation()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        If BtnSave.Text = "&Save" And dg1.RowCount > 0 Then MsgBox("Save Transaction First...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnDelete.Enabled = True
        btnPrint.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Beejak' Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from  Vouchers WHERE transtype = 'Beejak' Order By ID LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString() + ""
        'sSql = "Select * from Vouchers where id=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            ' txtid.Text = id
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("SallerName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txttotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtCustName.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtCustMobile.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtPaymentTerm.Text = ds.Tables("a").Rows(0)("T3").ToString()
            txtBrokerName.Text = ds.Tables("a").Rows(0)("T4").ToString()
            txtBrokerMob.Text = ds.Tables("a").Rows(0)("T5").ToString()
            txtTransPort.Text = ds.Tables("a").Rows(0)("T6").ToString()
            txtDriverName.Text = ds.Tables("a").Rows(0)("T7").ToString()
            txtGrNo.Text = ds.Tables("a").Rows(0)("T8").ToString()
            txtDriverMobile.Text = ds.Tables("a").Rows(0)("T9").ToString()
            txtRemark.Text = ds.Tables("a").Rows(0)("T10").ToString()
            txtBuiltyNo.Text = ds.Tables("a").Rows(0)("T11").ToString()
            txtid.Text = Val(ds.Tables("a").Rows(0)("ID").ToString())
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
        End If
        Dim sql As String = "Select * from Transaction1 where VoucherID=" & Val(txtid.Text)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        ad1.Fill(ds, "b")
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
                .Rows(i).Cells("CrateAccountID").Value = Val(ds.Tables("b").Rows(i)("CrateAccountID").ToString())
                .Rows(i).Cells("CrateAccountName").Value = ds.Tables("b").Rows(i)("CrateAccountName").ToString()
                .Rows(i).Cells("CrateID").Value = Val(ds.Tables("b").Rows(i)("CrateID").ToString())
                .Rows(i).Cells("CrateName").Value = ds.Tables("b").Rows(i)("CrateMarka").ToString()
                .Rows(i).Cells("CrateQty").Value = ds.Tables("b").Rows(i)("CrateQty").ToString()
                .Rows(i).Cells("CrateTransType").Value = ds.Tables("b").Rows(i)("CrateTransType").ToString()
                .Rows(i).Cells("CrateYN").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells("AddWeight").Value = ds.Tables("b").Rows(i)("AddWeight").ToString()
            Next
        End With
        'txtItemID.Text = IID
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & Val(txtid.Text)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        ad2.Fill(ds, "C")
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
        dg1.ClearSelection() : Dg2.ClearSelection() : calc()
    End Sub

    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs) Handles txtCharges.GotFocus, txtOnValue.GotFocus, txtCalculatePer.GotFocus, txtPlusMinus.GotFocus
        If txtCharges.Focused = True Then
            dgItemSearch.Visible = False : DgAccountSearch.Visible = False
            If dgCharges.ColumnCount = 0 Then ChargesRowColums()
            If dgCharges.RowCount = 0 Then RetriveCharges()
            If txtCharges.Text.Trim() <> "" Then
                RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
            Else
                RetriveCharges()
            End If
            If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        End If
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub
    Private Sub txtCharges_LostFocus(sender As Object, e As EventArgs) Handles txtCharges.LostFocus, txtOnValue.LostFocus, txtCalculatePer.LostFocus, txtPlusMinus.LostFocus, txtchargesAmount.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.GhostWhite
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
            If e.KeyCode = Keys.Down Then
                If dgCharges.Visible = True Then dgCharges.Focus() : Exit Sub
                Dg2.Rows(0).Selected = True : Dg2.Focus()
            End If
        End If
        If dgItemSearch.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If Dg2.Rows.Count = 0 Then Exit Sub
                Dg2.Rows(0).Selected = True : Dg2.Focus()
            End If
        End If
        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
                calc()
                'ClearDetails()
            End If
        End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If dg1.RowCount = 0 Then Exit Sub : If dg1.SelectedRows.Count = 0 Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
            txtLotNo.Text = dg1.SelectedRows(0).Cells(1).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
            txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
            txtTotAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
            cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(9).Value)
            cbAccountName.Text = dg1.SelectedRows(0).Cells(10).Value
            cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(11).Value)
            cbCrateMarka.Text = dg1.SelectedRows(0).Cells(12).Value
            txtCrateQty.Text = Val(dg1.SelectedRows(0).Cells(13).Value)
            cbCrateTrans.Text = dg1.SelectedRows(0).Cells(14).Value
            lblCrate.Text = dg1.SelectedRows(0).Cells(15).Value
            txtAddWeight.Text = dg1.SelectedRows(0).Cells(16).Value
            txtItem.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Up Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Index) = 0 Then txtItem.Focus()
            dg1.ClearSelection()
        End If
        If e.KeyCode = Keys.Down Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub

            If Val(dg1.SelectedRows(0).Index) = Val(dg1.Rows.Count - 1) Then dg1.ClearSelection() : dg1.Rows(0).Selected = True
        End If
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0)) : calc()
            End If
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
        txtLotNo.Text = dg1.SelectedRows(0).Cells(1).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
        txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
        txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
        Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
        txtTotAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
        cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(9).Value)
        cbAccountName.Text = dg1.SelectedRows(0).Cells(10).Value
        cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(11).Value)
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(12).Value
        txtCrateQty.Text = Val(dg1.SelectedRows(0).Cells(13).Value)
        cbCrateTrans.Text = dg1.SelectedRows(0).Cells(14).Value
        lblCrate.Text = dg1.SelectedRows(0).Cells(15).Value
        'txtItem.Focus()
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

    Private Sub Dg2_KeyDown(sender As Object, e As KeyEventArgs) Handles Dg2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dg2.RowCount = 0 Then Exit Sub
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            txtCharges.Text = Dg2.SelectedRows(0).Cells(0).Value
            txtOnValue.Text = Dg2.SelectedRows(0).Cells(1).Value
            txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(2).Value
            txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(3).Value
            txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(4).Value
            txtChargeID.Text = Dg2.SelectedRows(0).Cells(5).Value
            txtCharges.Focus() : e.SuppressKeyPress = True
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

        If e.KeyCode = Keys.Delete Then
            If MessageBox.Show("Are you Sure to delete Charge", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
                calc() : Dg2.ClearSelection() : txtCharges.Focus()
                'ClearDetails()
            End If
        End If
        If e.KeyCode = Keys.Back Then txtCharges.Focus()
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
        'txtItem.Focus()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        TempRowColumn()
        mskEntryDate.Focus()
        If dg1.RowCount = 0 Then MsgBox("There is no record to Save / Update...", vbOKOnly, "Empty") : Exit Sub
        If BtnSave.Text = "&Save" Then
            Dim AddSellout As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
            If AddSellout <> "Y" Then MsgBox("You Don't Have Rights to Save Sellout... " & vbNewLine & " Please Contact to Admin..Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            Save()
        Else
            Dim ModifySellout As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
            If ModifySellout <> "Y" Then MsgBox("You Don't Have Rights to Modify Sellout... " & vbNewLine & " Please Contact to Admin..Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            UpdateRecord()
        End If

        Dim res = MessageBox.Show("Do you want to Print Seller Bill", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If res = Windows.Forms.DialogResult.Yes Then
            btnPrint.Enabled = True
            btnPrint.PerformClick()
            RadioPrint1.Checked = True
            Exit Sub
        Else
            txtid.Clear()
        End If
        cleartxt() : cleartxtCharges()
        FootertextClear() : dg1.Rows.Clear()
        Dg2.Rows.Clear()

    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If dg1.RowCount = 0 Then MsgBox("There is no record to Print...", vbOKOnly, "Empty") : Exit Sub
        Dim PrintSellout As String = clsFun.ExecScalarStr("SELECT Print FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sellout'")
        If PrintSellout <> "Y" Then MsgBox("You Don't Have Rights to Print Purchase... " & vbNewLine & " Please Contact to Admin..Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        txtWhatsappNo.Text = clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
        TempRowColumn() : ClosingBal() : retrive2()
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
    Private Sub WahSoft()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        WABA.ExecNonQuery("Delete from SendingData")

        'pnlWahtsappNo.Visible = True
        'txtWhatsappNo.Focus()
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        PrintRecord()
        If RadioPrint1.Checked = True Then
            Pdf_Genrate.ExportReport("\Formats\MannualBeejak.rpt")
        Else
            Pdf_Genrate.ExportReport("\Formats\MannualBeejak2.rpt")
        End If
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & whatsappSender.FilePath & "')"
        If WABA.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            WABA.ExecScalarStr(sql)
            'lblStatus.Visible = True
            'lblStatus.Text = "Data Send to wahsoft Successfully..."
        End If
        Dim p() As Process
        p = Process.GetProcessesByName("WahSoft")
        If p.Count = 0 Then
            Dim StartWhatsapp As New System.Diagnostics.Process
            StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\WahSoft\WahSoft.exe"
            StartWhatsapp.Start()
        End If
    End Sub


    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=33", "GroupName", "ID", "")
            CreateAccount.BringToFront()
        End If
        If e.KeyCode = Keys.F1 Then
            Dim AccountID As String = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show() : CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            CreateAccount.FillContros(AccountID)
            CreateAccount.BringToFront()
        End If
    End Sub

    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear() : txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        AcBal() : DgAccountSearch.Visible = False
        txtItem.Focus()
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
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            AcBal()
            DgAccountSearch.Visible = False
            txtItem.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 224
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 160
        DgAccountSearch.Visible = True : DgAccountSearch.BringToFront()
        retriveAccounts()
    End Sub

    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress, txtItem.KeyPress, txtCharges.KeyPress, txtLotNo.KeyPress
        If txtAccount.Focused = True Then DgAccountSearch.BringToFront()
        If txtItem.Focused = True Then dgItemSearch.Visible = True
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp

        If DgAccountSearch.RowCount = 0 Then Exit Sub
        DgAccountSearch.Visible = True
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
            Exit Sub
        End If
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        If ckShowCustomer.Checked = True Then
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,17,16)  or UnderGroupID in (11,17,16))" & condtion & " order by AccountName")
        Else
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,17)  or UnderGroupID in (11,17))" & condtion & " order by AccountName")
        End If

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
    Private Sub ItemRowColumns()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 200
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 200
        dgItemSearch.Visible = True : dgItemSearch.BringToFront()
        retriveItems()
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
    Private Sub txtItem_KeyDown(sender As Object, e As KeyEventArgs) Handles txtItem.KeyDown
        If e.KeyCode = Keys.Down Then dgItemSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            Item_form.Opener = Me
            Item_form.OpenedFromItems = True
            Item_form.BringToFront()
            Dim CutStop As String = String.Empty
        End If
        If e.KeyCode = Keys.F1 Then
            If Val(dgItemSearch.SelectedRows(0).Cells(0).Value) = 0 Then Exit Sub
            Item_form.MdiParent = MainScreenForm
            Item_form.FillContros(Val(dgItemSearch.SelectedRows(0).Cells(0).Value))
            Item_form.Show()
            Item_form.BringToFront()
            Item_form.Opener = Me
            Item_form.OpenedFromItems = True
            Dim CutStop As String = String.Empty
        End If
    End Sub


    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        ItemRowColumns()
        If txtItem.Text.Trim() <> "" Then
            dgItemSearch.Visible = True
            retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
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
            txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
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
        txtCharges.Clear() : txtChargeID.Clear()
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        dgCharges.Visible = False
        txtOnValue.Focus() : FillCharges()
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
    End Sub
    Private Sub txtCharges_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyUp
        ChargesRowColums()
        If txtCharges.Text.Trim() <> "" Then
            RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgCharges.Visible = False
    End Sub

    Private Sub txtCharges_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCharges.KeyPress
        ChargesRowColums()
        dgCharges.Visible = True
    End Sub

    Private Sub txtVoucherNo_Leave(sender As Object, e As EventArgs) Handles txtVoucherNo.Leave
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarStr("Select count(*) from  Vouchers Where TransType='" & Me.Text & "' AND billNo='" & txtVoucherNo.Text & "'") = 1 Then
                MsgBox("Voucher Number Already Exists...", vbOKOnly, "Access Denied")
                txtVoucherNo.Focus() : Exit Sub
            End If
        Else
            Exit Sub
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If txtid.Text = "" Then
            MsgBox("If you want to Print. Save First Record.", vbOKOnly, "Save First")
            Dim res = MessageBox.Show("Do you want to Save Bill", "Save First?", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = Windows.Forms.DialogResult.Yes Then
                BtnSave.PerformClick()
            Else
            End If
        ElseIf dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            'clsFun.CloseConnection()
            'clsFun.changeCompany()
            PrintRecord()
            If RadioPrint1.Checked = True Then
                Report_Viewer.printReport("\MannualBeejak.rpt")
            Else
                Report_Viewer.printReport("\MannualBeejak2.rpt")
            End If

            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
        pnlWhatsapp.Visible = False : mskEntryDate.Focus()
        cleartxt() : cleartxtCharges()
        FootertextClear() : dg1.Rows.Clear()
        Dg2.Rows.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtid.Text = "" Then
            MsgBox("If you want to Print. Save First Record.", vbOKOnly, "Save First")
            Dim res = MessageBox.Show("Do you want to Save Bill", "Save First?", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = Windows.Forms.DialogResult.Yes Then
                BtnSave.PerformClick()
            End If
        ElseIf dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If RadioPrint1.Checked = True Then
                Report_Viewer.printReport("\MannualBeejak.rpt")
            Else
                Report_Viewer.printReport("\MannualBeejak2.rpt")
            End If
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            Report_Viewer.BringToFront()
        End If
        pnlWhatsapp.Visible = False : mskEntryDate.Focus()
        cleartxt() : cleartxtCharges()
        FootertextClear() : dg1.Rows.Clear()
        Dg2.Rows.Clear() : Report_Viewer.btnPrint.Focus()
    End Sub

    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
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
        pnlWhatsapp.Visible = False : mskEntryDate.Focus()
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
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
                SendWhatsappData() : FootertextClear()
                'StartBackgroundTask(AddressOf SendWhatsappData) : FootertextClear()
            Else
                MsgBox("Please Enter Valid Whatsapp Contact", MsgBoxStyle.Critical, "Invalid Contact") : txtWhatsappNo.Focus() : Exit Sub
            End If
            pnlWhatsapp.Visible = False : mskEntryDate.Focus()
            cleartxt() : cleartxtCharges()
            FootertextClear() : dg1.Rows.Clear()
            Dg2.Rows.Clear() : VNumber()
        Else
            StartBackgroundTask(AddressOf WahSoft) : FootertextClear()
        End If
    End Sub
    Private Sub SendWhatsappData()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ' Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")

        'pnlWahtsappNo.Visible = True
        'txtWhatsappNo.Focus()
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        PrintRecord()
        If RadioPrint1.Checked = True Then
            Pdf_Genrate.ExportReport("\Formats\MannualBeejak.rpt")
        Else
            Pdf_Genrate.ExportReport("\Formats\MannualBeejak2.rpt")
        End If
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            'lblStatus.Visible = True
            'lblStatus.Text = "Data Send to Easy Whatsapp Successfully..."
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
    End Sub

    Private Sub txtInvoiceID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceID.KeyDown
        If e.KeyCode = Keys.Enter Then pnlInvoiceID.Visible = False : txtVehicleNo.Focus()
    End Sub

    Private Sub txtTotAmount_keyUp(sender As Object, e As EventArgs) Handles txtTotAmount.KeyUp
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


    Private Sub cbCrateTrans_KeyDown(sender As Object, e As KeyEventArgs) Handles cbCrateTrans.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlMarka.Visible = False
            txtKg.Focus()
            e.SuppressKeyPress = True
            Exit Sub
        End If
    End Sub

    Private Sub txttotalNetAmount_TextChanged(sender As Object, e As EventArgs) Handles txttotalNetAmount.TextChanged
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

    Private Sub txtKg_LostFocus(sender As Object, e As EventArgs) Handles txtItem.LostFocus, txtAccount.LostFocus,
  txtVoucherNo.LostFocus, txtVehicleNo.LostFocus, txtLotNo.LostFocus, txtNug.LostFocus, txtKg.LostFocus, txtRate.LostFocus,
 txtCrateQty.LostFocus, txtCustName.LostFocus, txtCustMobile.LostFocus, txtPaymentTerm.LostFocus, txtBrokerName.LostFocus, txtBrokerMob.LostFocus,
  txtTransPort.LostFocus, txtDriverName.LostFocus, txtGrNo.LostFocus, txtDriverMobile.LostFocus, txtRemark.LostFocus, txtBuiltyNo.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.GhostWhite
        ' tb.SelectAll()
    End Sub

    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub

    Private Sub txtAddWeight_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAddWeight.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtAddWeight.Text = "" Then pnlAddWeight.Visible = False : txtKg.Focus() : Exit Sub
            Dim code As String = ""
            Dim a() As String = Split(txtAddWeight.Text, "+")
            If a.Length >= 1 Then
                For i = 0 To a.Length - 1
                    code = Val(code) + Val(a(i).ToString)
                Next
            End If
            txtKg.Text = code
            txtKg.Focus()
            pnlAddWeight.Visible = False
        End If
    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub txtCharges_TextChanged(sender As Object, e As EventArgs) Handles txtCharges.TextChanged

    End Sub

    Private Sub txtNug_TextChanged(sender As Object, e As EventArgs) Handles txtNug.TextChanged

    End Sub
End Class