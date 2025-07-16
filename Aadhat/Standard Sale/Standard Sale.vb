Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text

Public Class Standard_Sale
    Dim vno As Integer : Dim VchId As Integer
    Dim sql As String = String.Empty
    Dim count As Integer = 0
    Dim MaxID As String = String.Empty
    Dim CalcType As String = String.Empty
    Dim ApplyON As String = String.Empty
    Dim LotBal As String = String.Empty
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 1 : Dim Offset As Integer = 0
    Dim remark2 As String : Dim remarkHindi As String
    Dim ServerTag As Integer
    Dim APIResposne As String
    Dim FilePath As String : Dim hostedFilePath As String
    Dim access_token As String = "6687c047a58e1"
    Dim instance_id As String = ClsFunPrimary.ExecScalarStr("Select InstanceID From API")
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False
    Dim whatsappSender As New WhatsAppSender() : Dim stopBasic As String = String.Empty
    Dim trackStock As String

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        clsFun.DoubleBuffered(Dg2, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If isBackgroundWorkerRunning Then
            Me.Hide() : Me.Top = 0 : Me.Left = 0
        Else
            If MessageBox.Show("Are You Sure want to Exit Standard Sale ??", "Exit Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close()
            End If
        End If
    End Sub

    Private Sub Standard_Sale_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        If isBackgroundWorkerRunning Then
            e.Cancel = True
            Me.Hide() : Me.Top = 0 : Me.Left = 0
            ' MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub Standard_Sale_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False : txtAccount.Focus() : Exit Sub
            If dgItemSearch.Visible = True Then dgItemSearch.Visible = False : txtItem.Focus() : Exit Sub
            If dgCharges.Visible = True Then dgCharges.Visible = False : txtCharges.Focus() : Exit Sub
            If pnlWhatsapp.Visible = True Then pnlWhatsapp.Visible = False : mskEntryDate.Focus() : Exit Sub
            If isBackgroundWorkerRunning Then
                Me.Hide() : Me.Top = 0 : Me.Left = 0
            Else
                If MessageBox.Show("Are You Sure want to Exit Standard Sale ??", "Exit Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Me.Close() : e.SuppressKeyPress = True
                End If
            End If
        End If
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub AcBal()
        ' Dim i, j As Integer
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
            opbal = Math.Round(Math.Abs(Val(opbal)), 2) & " Cr"
        Else
            opbal = Math.Round(Math.Abs(Val(opbal)), 2) & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Math.Round(Math.Abs(Val(opbal)), 2) & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " Cr"
            Else
                opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " Dr"
            End If
        End If
        lblAcBal.Visible = True
        lblAcBal.Text = "Account Balance : " & opbal
    End Sub
    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub Standard_Sale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33,11) order by AccountName ", "AccountName", "ID", "--N./A.--")
        Cbper.SelectedIndex = 0 : clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        mskEntryDate.Focus() : Me.KeyPreview = True
        pnlMarka.Visible = False : BtnDelete.Enabled = False
        VNumber() : rowColums() : FillstdSale()
    End Sub
    Public Sub FillstdSale()
        Dim dt As DataTable
        Dim ssql As String = "Select * from Controls "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("StdCommission").ToString().Trim() = "Percentage" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = False
            ElseIf dt.Rows(0)("StdCommission").ToString().Trim() = "Amount" Then
                txtComAmt.TabStop = True : txtComPer.TabStop = False
            ElseIf dt.Rows(0)("StdCommission").ToString().Trim() = "Both" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = True
            Else
                txtComPer.TabStop = False : txtComAmt.TabStop = False
            End If
            If dt.Rows(0)("StdMandiTax").ToString().Trim() = "Percentage" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = False
            ElseIf dt.Rows(0)("StdMandiTax").ToString().Trim() = "Amount" Then
                txtMAmt.TabStop = True : txtMPer.TabStop = False
            ElseIf dt.Rows(0)("StdMandiTax").ToString().Trim() = "Both" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = True
            Else
                txtMPer.TabStop = False : txtMAmt.TabStop = False
            End If
            If dt.Rows(0)("StdRDF").ToString().Trim() = "Percentage" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = False
            ElseIf dt.Rows(0)("StdRDF").ToString().Trim() = "Amount" Then
                txtRdfAmt.TabStop = True : txtRdfPer.TabStop = False
            ElseIf dt.Rows(0)("StdRDF").ToString().Trim() = "Both" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = True
            Else
                txtRdfPer.TabStop = False : txtRdfAmt.TabStop = False
            End If
            If dt.Rows(0)("StdTare").ToString().Trim() = "Nug" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = False
            ElseIf dt.Rows(0)("StdTare").ToString().Trim() = "Amount" Then
                txtTareAmt.TabStop = True : txtTare.TabStop = False
            ElseIf dt.Rows(0)("StdTare").ToString().Trim() = "Both" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = True
            Else
                txtTare.TabStop = False : txtTareAmt.TabStop = False
            End If
            If dt.Rows(0)("StdLabour").ToString().Trim() = "Nug" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = False
            ElseIf dt.Rows(0)("StdLabour").ToString().Trim() = "Amount" Then
                txtLaboutAmt.TabStop = True : txtLabour.TabStop = False
            ElseIf dt.Rows(0)("StdLabour").ToString().Trim() = "Both" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = True
            Else
                txtLabour.TabStop = False : txtLaboutAmt.TabStop = False
            End If
            Cbper.Text = dt.Rows(0)("Per").ToString().Trim()
            If dt.Rows(0)("STDNoLot").ToString().Trim() = "Yes" Then
                txtLot.TabStop = False : txtVehicleNo.TabStop = False
            Else
                txtLot.TabStop = True : txtVehicleNo.TabStop = True
            End If
            If dt.Rows(0)("StopBasic").ToString().Trim() = "Yes" Then txtBasicAmount.TabStop = True : txtBasicAmount.ReadOnly = False Else txtBasicAmount.TabStop = False : txtBasicAmount.ReadOnly = True
        End If
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 76
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "EntryDate" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "SallerName" : .Columns(3).Width = 59
            .Columns(4).Name = "BillingType" : .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo" : .Columns(5).Width = 159
            .Columns(6).Name = "itemName" : .Columns(6).Width = 69
            .Columns(7).Name = "Cut" : .Columns(7).Width = 76
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
            .Columns(26).Name = "CommPer" : .Columns(26).Width = 90
            .Columns(27).Name = "CommAmt" : .Columns(27).Width = 86
            .Columns(28).Name = "UCPer" : .Columns(28).Width = 90
            .Columns(29).Name = "UcAmt" : .Columns(29).Width = 90
            .Columns(30).Name = "RdfPer" : .Columns(30).Width = 95
            .Columns(31).Name = "RdfAmt" : .Columns(31).Width = 159
            .Columns(32).Name = "Bardanaper" : .Columns(32).Width = 159
            .Columns(33).Name = "BardanaAmt" : .Columns(33).Width = 90
            .Columns(34).Name = "labourPer" : .Columns(34).Width = 90
            .Columns(35).Name = "LabourAmt" : .Columns(35).Width = 95
            .Columns(36).Name = "TotalCharges" : .Columns(36).Width = 159
            .Columns(37).Name = "GrossAmount" : .Columns(37).Width = 159
            .Columns(38).Name = "TotalNugs" : .Columns(38).Width = 159
            .Columns(39).Name = "TotalNugs" : .Columns(39).Width = 159
            .Columns(40).Name = "TotalWeight" : .Columns(40).Width = 159
            .Columns(41).Name = "TotalCommission" : .Columns(41).Width = 159
            .Columns(42).Name = "TotalMarketFees" : .Columns(42).Width = 159
            .Columns(43).Name = "TotalRDF" : .Columns(43).Width = 159
            .Columns(44).Name = "TotalTare" : .Columns(44).Width = 159
            .Columns(45).Name = "TotalLabour" : .Columns(45).Width = 159
            .Columns(46).Name = "Driver Name" : .Columns(46).Width = 159
            .Columns(47).Name = "Mobile" : .Columns(47).Width = 159
            .Columns(48).Name = "Remark" : .Columns(48).Width = 159
            .Columns(49).Name = "GrNo" : .Columns(47).Width = 159
            .Columns(50).Name = "GSTN" : .Columns(48).Width = 159
            .Columns(51).Name = "Cust Mobile" : .Columns(51).Width = 159
            .Columns(52).Name = "Broker Name" : .Columns(52).Width = 159
            .Columns(53).Name = "Broker Mobile" : .Columns(53).Width = 159
            .Columns(54).Name = "TransPort" : .Columns(54).Width = 159
            .Columns(55).Name = "GRNo" : .Columns(55).Width = 159
            .Columns(56).Name = "CrateMarka" : .Columns(56).Width = 159
            .Columns(57).Name = "CrateQty" : .Columns(57).Width = 159
            .Columns(58).Name = "OpeningBal" : .Columns(58).Width = 159
            .Columns(59).Name = "Closingbal" : .Columns(59).Width = 159
            .Columns(60).Name = "TotalCrate" : .Columns(60).Width = 159
            .Columns(61).Name = "CrateDetails" : .Columns(61).Width = 159
            .Columns(62).Name = "ItemTotal" : .Columns(62).Width = 159
            .Columns(63).Name = "PrintCharges" : .Columns(63).Width = 159
            .Columns(64).Name = "Address" : .Columns(64).Width = 159
            .Columns(65).Name = "City" : .Columns(65).Width = 159
            .Columns(66).Name = "State" : .Columns(66).Width = 159
            .Columns(67).Name = "Mobile1" : .Columns(67).Width = 159
            .Columns(68).Name = "Mobile2" : .Columns(68).Width = 159
            .Columns(69).Name = "ItemTotal" : .Columns(69).Width = 159
            .Columns(70).Name = "PrintCharges" : .Columns(70).Width = 15
            .Columns(71).Name = "ReceiptAmt" : .Columns(71).Width = 15
            .Columns(72).Name = "lastRecp" : .Columns(72).Width = 15
            .Columns(73).Name = "lastCrate" : .Columns(73).Width = 15
            .Columns(74).Name = "lastRecp" : .Columns(74).Width = 15
            .Columns(75).Name = "lastCrate" : .Columns(75).Width = 15
        End With
    End Sub
    Sub retrive2()
        TempRowColumn()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        dt = clsFun.ExecDataTable("Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo,Vouchers.AccountID, Vouchers.AccountName,Accounts.Address,Accounts.City,Accounts.State,Accounts.Mobile1,Accounts.Mobile2,  Vouchers.VehicleNo," _
                                  & "Transaction2.ItemName, Transaction2.Lot, Transaction2.Nug as TransNug, Transaction2.Weight, Transaction2.Rate," _
                                  & "Transaction2.Per, Transaction2.Amount,Transaction2.CommPer,Transaction2.CommAmt,Transaction2.MPer,Transaction2.MAmt," _
                                  & "Transaction2.RdfPer,Transaction2.RdfAmt,Transaction2.Tare,Transaction2.TareAmt,Transaction2.labour,Transaction2.LabourAmt," _
                                  & "Transaction2.TotalAmount as TotAmt,Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, Vouchers.DiscountAmount, Vouchers.TotalCharges, vouchers.SubTotal, " _
                                  & "Vouchers.RoundOff,Vouchers.T1,Vouchers.T2,Vouchers.T3,Vouchers.T4,Vouchers.T5,Vouchers.T6,Vouchers.T7,Vouchers.T8,Vouchers.T9,Vouchers.T10, " _
                                  & "Items.OtherName, Accounts.OtherName as AccountOtherName,Transaction2.Cratemarka as CrateMarka, Transaction2.CrateQty as CrateQty FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)" _
                                  & "INNER JOIN Items ON Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.AccountID = Accounts.ID  Where (Vouchers.ID=" & Val(txtid.Text) & ")")

        ''''''''''''''''''''' Opening Balance'''''''''''''''''''''''''''''''''''
        opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(txtAccountID.Text) & " Order by upper(AccountName) ;"))

        ''''''''''''''''''''closing balance'''''''''''''''''''''''''

        ClBal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(txtAccountID.Text) & " Order by upper(AccountName) ;"))

        TodaysCredit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate = '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'"))
        todaysDebit = Val(clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate = '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'"))
        If ClBal < 0 Then
            lastbal = Format(Math.Abs(Val(Val(ClBal + todaysDebit) - TodaysCredit)), "0.00") & " Cr"
        Else
            lastbal = Format(Math.Abs(Val(Val(ClBal - todaysDebit))), "0.00") & " Dr"
        End If

        acID = (txtAccountID.Text)
        ''''''Total Crates Show''''''
        Dim U As Integer = 0
        Dim cratebal As String = String.Empty
        Dim CrateQty As String = String.Empty
        Dim CrateName As String = String.Empty
        Dim CQty As String = String.Empty
        Dim SingleCrate As String = String.Empty
        Dim dtcrate As New DataTable
        dtcrate = clsFun.ExecDataTable("Select CrateName,CrateName ||':'||" & _
        " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') -" & _
        " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as Reciveable," & _
                    " ((Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID = ACG.ID and CV.CrateID = CrateID and CrateType='Crate Out' and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') -" & _
        " (Select ifnull(Sum(Qty),0) from CrateVoucher Where AccountID =  ACG.ID and CV.CrateID = CrateID and CrateType='Crate In' and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as DueCrates " & _
        " FROM CrateVoucher as CV INNER JOIN Account_AcGrp AS ACG ON CV.AccountID = ACG.ID Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(txtAccountID.Text) & "' Group by AccountID,CrateID Having DueCrates<>0 order by upper(ACG.AccountName);")
        Try
            If dtcrate.Rows.Count > 0 Then
                For U = 0 To dtcrate.Rows.Count - 1
                    If Application.OpenForms().OfType(Of Standard_Sale).Any = False Then Exit Sub
                    cratebal = dtcrate.Rows(U)("Reciveable").ToString()
                    CrateQty = CrateQty & ", " & cratebal
                    CrateName = CrateName & dtcrate.Rows(U)("CrateName").ToString() & vbCrLf
                    CQty = CQty & dtcrate.Rows(U)("DueCrates").ToString() & vbCrLf
                    SingleCrate = Val(SingleCrate) + Val(dtcrate.Rows(U)("DueCrates").ToString())
                Next
                CrateQty = CrateQty.Trim().TrimStart(",")
            End If
        Catch ex As Exception

        End Try
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1

                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Lot").ToString()
                    .Cells(8).Value = .Cells(8).Value & Val(dt.Rows(i)("TransNug").ToString())
                    .Cells(9).Value = .Cells(9).Value & Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = .Cells(10).Value & Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = .Cells(12).Value & Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(18).Value = .Cells(18).Value & Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = .Cells(19).Value & Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = .Cells(20).Value & Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = .Cells(21).Value & Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = .Cells(22).Value & Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("AccountOtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & Format(Val(dt.Rows(i)("CommPer").ToString()), "0.00") & vbCrLf
                    .Cells(27).Value = .Cells(27).Value & Format(Val(dt.Rows(i)("CommAmt").ToString()), "0.00") & vbCrLf
                    .Cells(28).Value = .Cells(28).Value & Format(Val(dt.Rows(i)("Mper").ToString()), "0.00") & vbCrLf
                    .Cells(29).Value = .Cells(29).Value & Format(Val(dt.Rows(i)("Mamt").ToString()), "0.00") & vbCrLf
                    .Cells(30).Value = .Cells(30).Value & Format(Val(dt.Rows(i)("RdfPer").ToString()), "0.00") & vbCrLf
                    .Cells(31).Value = .Cells(31).Value & Format(Val(dt.Rows(i)("rdfAmt").ToString()), "0.00") & vbCrLf
                    .Cells(32).Value = .Cells(32).Value & Format(Val(dt.Rows(i)("Tare").ToString()), "0.00") & vbCrLf
                    .Cells(33).Value = .Cells(33).Value & Format(Val(dt.Rows(i)("tareamt").ToString()), "0.00") & vbCrLf
                    .Cells(34).Value = .Cells(34).Value & Format(Val(dt.Rows(i)("Labour").ToString()), "0.00") & vbCrLf
                    .Cells(35).Value = .Cells(35).Value & Format(Val(dt.Rows(i)("Labouramt").ToString()), "0.00") & vbCrLf
                    .Cells(36).Value = .Cells(36).Value & Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                    .Cells(37).Value = .Cells(37).Value & Format(Val(dt.Rows(i)("SubTotal").ToString()), "0.00")
                    .Cells(38).Value = .Cells(38).Value & Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                    .Cells(39).Value = Val(dt.Compute("Sum(TransNug)", ""))
                    .Cells(40).Value = Format(Val(dt.Compute("Sum(Weight)", "")), "0.00")
                    .Cells(41).Value = Format(Val(dt.Compute("Sum(CommAmt)", "")), "0.00")
                    .Cells(42).Value = Format(Val(dt.Compute("Sum(Mamt)", "")), "0.00")
                    .Cells(43).Value = Format(Val(dt.Compute("Sum(rdfAmt)", "")), "0.00")
                    .Cells(44).Value = Format(Val(dt.Compute("Sum(tareamt)", "")), "0.00")
                    .Cells(45).Value = Format(Val(dt.Compute("Sum(Labouramt)", "")), "0.00")
                    .Cells(46).Value = dt.Rows(i)("T1").ToString()
                    .Cells(47).Value = dt.Rows(i)("T2").ToString()
                    .Cells(48).Value = dt.Rows(i)("T3").ToString()
                    .Cells(49).Value = dt.Rows(i)("T4").ToString()
                    .Cells(50).Value = dt.Rows(i)("T5").ToString()
                    .Cells(51).Value = dt.Rows(i)("T6").ToString()
                    .Cells(52).Value = dt.Rows(i)("T7").ToString()
                    .Cells(53).Value = dt.Rows(i)("T8").ToString()
                    .Cells(54).Value = dt.Rows(i)("T9").ToString()
                    .Cells(55).Value = dt.Rows(i)("T10").ToString()
                    .Cells(56).Value = dt.Rows(i)("CrateMarka").ToString()
                    .Cells(57).Value = dt.Rows(i)("CrateQty").ToString()
                    .Cells(58).Value = If(Val(opbal) >= 0, Format(Math.Abs(Val(opbal)), "0.00") & " Dr", Format(Math.Abs(Val(opbal)), "0.00") & " Cr")
                    .Cells(59).Value = If(Val(ClBal) >= 0, Format(Math.Abs(Val(ClBal)), "0.00") & " Dr", Format(Math.Abs(Val(ClBal)), "0.00") & " Cr")
                    .Cells(60).Value = SingleCrate
                    .Cells(61).Value = CrateQty
                    .Cells(62).Value = Format(Val(dt.Rows(i)("TotAmt").ToString()), "0.00")
                    .Cells(64).Value = dt.Rows(i)("Address").ToString()
                    .Cells(65).Value = dt.Rows(i)("City").ToString()
                    .Cells(66).Value = dt.Rows(i)("State").ToString()
                    .Cells(67).Value = dt.Rows(i)("Mobile1").ToString()
                    .Cells(68).Value = dt.Rows(i)("Mobile2").ToString()
                    .Cells(71).Value = RectAmt 'dt.Rows(i)("Mobile2").ToString()
                    .Cells(72).Value = clsFun.ExecScalarStr("Select  ('Last Receipt Rs. : '|| Sum(Vouchers.TotalAmount)  || ' Disc : '|| Sum(Vouchers.DiscountAmount) ||  ' On : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(3) & " Group by EntryDate ORDER BY Vouchers.Entrydate DESC limit 1 ;")
                    .Cells(73).Value = clsFun.ExecScalarStr("Select 'Total Amount Rec. Rs. :'||Sum(Amount) ||' of Todays Crate Rec. :'||Sum(Qty) as CrateRec  From CrateVoucher Where EntryDate ='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "' and CrateType='Crate In' And Amount<>0 and AccountID=" & dt.Rows(i)(3) & "")
                    .Cells(74).Value = clsFun.ExecScalarStr("Select  ('अंतिम जमा राशि:'|| Sum(Vouchers.TotalAmount)  || ' छूट : '|| Sum(Vouchers.DiscountAmount) ||  ' दिनांक : '|| strftime('%d-%m-%Y', vouchers.Entrydate)) as lastReceipt,AccountID,TransType FROM Vouchers where TransType='Receipt' and Accountid=" & dt.Rows(i)(3) & " Group by EntryDate ORDER BY Vouchers.Entrydate DESC limit 1 ;")
                    .Cells(75).Value = clsFun.ExecScalarStr("Select 'आज केरेट जमा राशि:'||Sum(Amount) ||' आज जमा केरेट :'||Sum(Qty) as CrateRec  From CrateVoucher Where EntryDate ='" & CDate(dt.Rows(i)("EntryDate")).ToString("yyyy-MM-dd") & "' and CrateType='Crate In' And Amount<>0 and AccountID=" & dt.Rows(i)(3) & "")
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Amount").ToString() & vbCrLf
                            .Cells(63).Value = .Cells(63).Value & clsFun.ExecScalarStr("Select PrintName From Charges Where ID=" & Val(dt1.Rows(j)("ChargesID").ToString()) & "") & vbCrLf
                        Next
                    Else
                        .Cells(13).Value = ""
                        .Cells(14).Value = ""
                        .Cells(15).Value = ""
                        .Cells(16).Value = ""
                        .Cells(17).Value = ""
                        .Cells(63).Value = ""
                    End If
                    'dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    'If dt1.Rows.Count > 0 Then
                    '    For j = 0 To dt1.Rows.Count - 1
                    '        .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                    '        .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                    '        .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                    '        .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                    '        .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Amount").ToString() & vbCrLf
                    '    Next
                    'Else
                    '    .Cells(13).Value = ""
                    '    .Cells(14).Value = ""
                    '    .Cells(15).Value = ""
                    '    .Cells(16).Value = ""
                    '    .Cells(17).Value = ""
                    'End If
                End With
                '  End If
            Next
        End If
        dt.Clear()
        dt1.Clear()
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.SendToBack()
        DgAccountSearch.Visible = False
        txtItem.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32", "GroupName", "ID", "")
            CreateAccount.BringToFront()
        End If
        If e.KeyCode = Keys.F1 Then
            If DgAccountSearch.SelectedRows.Count = 0 Then AccountRowColumns()
            If txtAccount.Text.Trim() <> "" Then
                retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
            End If
            Dim AccountID As Integer = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show() : CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            CreateAccount.FillContros(AccountID)
            CreateAccount.BringToFront()
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            '    CustomerFill()
            DgAccountSearch.Visible = False
            txtItem.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 320
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 160
        DgAccountSearch.BringToFront() : DgAccountSearch.Visible = True
        retriveAccounts()
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        ' If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        If dgCharges.Visible = True Then dgCharges.Visible = False
    End Sub

    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress, txtItem.KeyPress, txtCharges.KeyPress, txtLot.KeyPress
        If txtAccount.Focused = True Then AccountRowColumns()
        If txtItem.Focused = True Then dgItemSearch.Visible = True
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
            Exit Sub
        End If
    End Sub

    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))" & condtion & " order by AccountName Limit 20")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
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
    Private Sub save()
        If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        If dgCharges.Visible = True Then dgCharges.Visible = False
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        dg1.ClearSelection()
        Dim cmd As SQLite.SQLiteCommand
        sql = "Insert Into Vouchers(Transtype, EntryDate,BillNo,AccountID, AccountName,VehicleNo,Nug,Kg,BasicAmount, " &
            "DiscountAmount,TotalAmount,TotalCharges,Subtotal,T1,T2,T3,RoundOff,T4,T5,T6,T7,T8,T9,T10,InvoiceID) Values  " &
            "(@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@3", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@4", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@5", txtAccount.Text)
            cmd.Parameters.AddWithValue("@6", txtVehicleNo.Text)
            cmd.Parameters.AddWithValue("@7", txtTotalNug.Text)
            cmd.Parameters.AddWithValue("@8", txttotalWeight.Text)
            cmd.Parameters.AddWithValue("@9", txtbasicTotal.Text)
            cmd.Parameters.AddWithValue("@10", txttotalCharges.Text)
            cmd.Parameters.AddWithValue("@11", txtTotalNetAmount.Text)
            cmd.Parameters.AddWithValue("@12", txtotherCharges.Text)
            cmd.Parameters.AddWithValue("@13", txtTotGross.Text)
            cmd.Parameters.AddWithValue("@14", txtDriverName.Text)
            cmd.Parameters.AddWithValue("@15", txtDriverMobile.Text)
            cmd.Parameters.AddWithValue("@16", txtRemark.Text)
            cmd.Parameters.AddWithValue("@17", txtTotRoundOff.Text)
            cmd.Parameters.AddWithValue("@18", txtStateName.Text)
            cmd.Parameters.AddWithValue("@19", txtGSTN.Text)
            cmd.Parameters.AddWithValue("@20", txtCustMobile.Text)
            cmd.Parameters.AddWithValue("@21", txtBrokerName.Text)
            cmd.Parameters.AddWithValue("@22", txtBrokerMob.Text)
            cmd.Parameters.AddWithValue("@23", txtTransPort.Text)
            cmd.Parameters.AddWithValue("@24", txtGrNo.Text)
            cmd.Parameters.AddWithValue("@25", txtInvoiceID.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                clsFun.CloseConnection()
            End If
            txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
            ServerTag = 1
            Dg1Record() : dg2Record() : insertLedger() : InsertCharges() : CrateLedger()
            ServerinsertLedger() : ServerInsertCharges() : ServerCrateLedger()
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Dim sallerAmt As Decimal = 0
    Private Sub Dg1Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        ' Dim cmd As SQLite.SQLiteCommand
        Dim SqliteEntryDate As String
        SqliteEntryDate = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sallerAmt = Val(txtTotalNetAmount.Text)
                If .Cells("Item Name").Value <> "" Then
                    Dim SellerID As Integer = clsFun.ExecScalarInt("Select StockHolderID From Purchase Where VoucherID='" & Val(.Cells(26).Value) & "'")
                    Dim SellerName As String = clsFun.ExecScalarStr("Select StockHolderName From Purchase Where VoucherID='" & Val(.Cells(26).Value) & "'")
                    Dim StorageID As Integer = clsFun.ExecScalarInt("Select StorageID From Vouchers Where ID='" & Val(.Cells(26).Value) & "'")
                    Dim StorageName As String = clsFun.ExecScalarStr("Select StorageName From Vouchers Where ID='" & Val(.Cells(26).Value) & "'")
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd") & "'," & Val(txtid.Text) & ", '" & Me.Text & "'," &
                             "'" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "'," & Val(.Cells("ItemID").Value) & "," &
                             "'" & .Cells("Item Name").Value & "','" & .Cells("Lot No").Value & "', " &
                             " '" & .Cells("Nug").Value & "','" & Val(.Cells("Weight").Value) & "','" & Val(.Cells("Rate").Value) & "'," &
                             "'" & Val(.Cells("Rate").Value) & "', '" & .Cells("per").Value & "','" & Val(.Cells("Amount").Value) & "'," &
                             "'" & Val(.Cells("TotalCharges").Value) & "'," &
                             " '" & .Cells("Total").Value & "', '" & Val(.Cells("Amount").Value) & "','" & Val(.Cells("CommPer").Value) & "'," &
                             " '" & .Cells("CommAmt").Value & "','" & Val(.Cells("MPer").Value) & "','" & Val(.Cells("MAmt").Value) & "'," &
                             " '" & .Cells("RdfPer").Value & "','" & Val(.Cells("Rdfamt").Value) & "','" & Val(.Cells("TarePer").Value) & "'," &
                             " '" & .Cells("TareAmt").Value & "','" & Val(.Cells("LabourPer").Value) & "','" & Val(.Cells("LabourAmt").Value) & "'," &
                             " '" & .Cells("CrateID").Value & "','" & .Cells("CrateName").Value & "','" & Val(.Cells("CrateQty").Value) & "'," &
                             "'" & .Cells("CrateY/N").Value & "','" & Val(.Cells(24).Value) & "','" & .Cells(25).Value & "','" & Val(.Cells(26).Value) & "', " &
                             "'" & Val(SellerID) & "','" & SellerName & "','" & Val(StorageID) & "','" & StorageName & "','" & Val(.Cells(27).Value) & "',0"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            Sql = "insert into Transaction2(EntryDate,VoucherID,TransType,AccountID,AccountName,ItemID,ItemName,Lot,Nug,Weight, " _
            & " Rate,SRate, Per,Amount,Charges,TotalAmount,SallerAmt,CommPer,CommAmt,MPer,MAmt,RdfPer,RdfAmt," _
            & "Tare,TareAmt,Labour, LabourAmt,CrateID,Cratemarka,CrateQty, MaintainCrate,CrateAccountID,CrateAccountName,PurchaseID,SallerID,SallerName,StorageID,StorageName,RoundOff,Cut) " & FastQuery & ""
            clsFun.ExecNonQuery(Sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub insertLedger()
        Dim FastQuery As String = String.Empty
        remark2 = String.Empty : remarkHindi = String.Empty
        Dim MandiTax As Decimal = 0.0 : Dim CommAmt As Decimal = 0.0
        Dim RDFAmt As Decimal = 0.0 : Dim TareAmt As Decimal = 0.0
        Dim Labouramt As Decimal = 0.0 : Dim Roff As Decimal = 0.0
        Dim Remark As Decimal = 0.0
        ' Cutomer Account
        For Each row As DataGridViewRow In dg1.Rows
            ' Application.DoEvents()
            With row
                If .Cells(0).Value <> "" Then
                    remark2 = remark2 & .Cells(0).Value & " Nug : " & .Cells(2).Value & " Weight : " & .Cells(3).Value & " On : " & .Cells(4).Value & " Per  /- " & .Cells(5).Value & ", Charges : " & .Cells(23).Value & vbCrLf
                    remarkHindi = remarkHindi & clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & (.Cells(8).Value) & "") & ", नग : " & .Cells(2).Value & " वजन : " & .Cells(3).Value & " भाव : " & .Cells(4).Value & "  /- " & .Cells(5).Value & ", ख़र्चे : " & .Cells(23).Value & vbCrLf
                    CommAmt = Val(CommAmt) + Val(.Cells(10).Value)
                    MandiTax = Val(MandiTax) + Val(.Cells(12).Value)
                    RDFAmt = Val(RDFAmt) + Val(.Cells(14).Value)
                    TareAmt = Val(TareAmt) + Val(.Cells(16).Value)
                    Labouramt = Val(Labouramt) + Val(.Cells(18).Value)
                    Roff = Val(Roff) + Val(.Cells(27).Value)
                End If
            End With
        Next

        If Val(txtAccountID.Text) > 0 Then ''Party Account
            '  clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Val(txtTotalNetAmount.Text), "D", remark2, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & Val(txtTotalNetAmount.Text) & "','D','" & remark2 & "','','" & remarkHindi & "'"
        End If
        ''Maal Khata Account
        If Val(txtbasicTotal.Text) > 0 Then ''Maal Khata Account
            '  clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 29, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29"), Val(txtbasicTotal.Text), "C", remark2, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(29) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "','" & Val(txtbasicTotal.Text) & "','C','" & remark2 & "','','" & remarkHindi & "'"
        End If

        If Val(CommAmt) > 0 Then ''Commission Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(10) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "','" & Val(CommAmt) & "','C','" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(MandiTax) > 0 Then ''Mandi Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(30) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "','" & Val(MandiTax) & "','C','" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(RDFAmt) > 0 Then ''rdf Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(39) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "','" & Val(RDFAmt) & "','C','" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(TareAmt) > 0 Then ''Bardana Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(4) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "','" & Val(TareAmt) & "','C','" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(Labouramt) > 0 Then ''Labour Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(23) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "','" & Val(Labouramt) & "','C','" & remark2 & "','','" & remarkHindi & "'"
        End If
        Roff = Val(Roff) + Val(txtTotRoundOff.Text)
        If Val(Roff) <> 0 Then ''Account 
            If Val(Roff) < 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Math.Abs(Val(Roff)) & ",'D' ,'" & Remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Val(Roff) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub
    Private Sub ServerinsertLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        remark2 = String.Empty : remarkHindi = String.Empty
        Dim MandiTax As Decimal = 0.0 : Dim CommAmt As Decimal = 0.0
        Dim RDFAmt As Decimal = 0.0 : Dim TareAmt As Decimal = 0.0
        Dim Labouramt As Decimal = 0.0 : Dim Roff As Decimal = 0.0
        Dim Remark As String = String.Empty
        ' Cutomer Account
        For Each row As DataGridViewRow In dg1.Rows
            ' Application.DoEvents()
            With row
                If .Cells(0).Value <> "" Then
                    remark2 = remark2 & .Cells(0).Value & " Nug : " & .Cells(2).Value & " Weight : " & .Cells(3).Value & " On : " & .Cells(4).Value & " Per  /- " & .Cells(5).Value & ", Charges : " & .Cells(23).Value & vbCrLf
                    remarkHindi = remarkHindi & clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & (.Cells(8).Value) & "") & ", नग : " & .Cells(2).Value & " वजन : " & .Cells(3).Value & " भाव : " & .Cells(4).Value & "  /- " & .Cells(5).Value & ", ख़र्चे : " & .Cells(23).Value & vbCrLf
                    CommAmt = Val(CommAmt) + Val(.Cells(10).Value)
                    MandiTax = Val(MandiTax) + Val(.Cells(12).Value)
                    RDFAmt = Val(RDFAmt) + Val(.Cells(14).Value)
                    TareAmt = Val(TareAmt) + Val(.Cells(16).Value)
                    Labouramt = Val(Labouramt) + Val(.Cells(18).Value)
                End If
            End With
        Next

        If Val(txtAccountID.Text) > 0 Then ''Party Account
            '  clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Val(txtTotalNetAmount.Text), "D", remark2, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & Val(txtTotalNetAmount.Text) & "','D'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & remark2 & "','','" & remarkHindi & "'"
        End If
        ''Maal Khata Account
        If Val(txtbasicTotal.Text) > 0 Then ''Maal Khata Account
            '  clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 29, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29"), Val(txtbasicTotal.Text), "C", remark2, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(29) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "','" & Val(txtbasicTotal.Text) & "','C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(CommAmt) > 0 Then ''Commission Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(10) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "','" & Val(CommAmt) & "','C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(MandiTax) > 0 Then ''Mandi Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(30) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "','" & Val(MandiTax) & "','C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(RDFAmt) > 0 Then ''rdf Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(39) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "','" & Val(RDFAmt) & "','C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(TareAmt) > 0 Then ''Bardana Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(4) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "','" & Val(TareAmt) & "','C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(Labouramt) > 0 Then ''Labour Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & Val(23) & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "','" & Val(Labouramt) & "','C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & remark2 & "','','" & remarkHindi & "'"
        End If
        If Val(Roff) <> 0 Then ''Account 
            If Val(Roff) < 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Math.Abs(Val(Roff)) & ",'D' ,'" & Remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Val(Roff) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & remarkHindi & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub
    Private Sub CrateLedger()
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If Val(.Cells(24).Value) <> 0 Then
                    If Val(.Cells(22).Value) > 0 Then ''Party Account
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(24).Value) & ",'" & .Cells(25).Value & "','Crate Out'," & Val(.Cells(21).Value) & ",'" & .Cells(20).Value & "','" & .Cells(22).Value & "', '','','',''"
                    End If
                Else
                    If Val(.Cells(22).Value) > 0 Then ''Party Account
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(.Cells(21).Value) & ",'" & .Cells(20).Value & "','" & .Cells(22).Value & "', '','','',''"
                    End If
                End If

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
                If Val(.Cells(22).Value) > 0 Then ''Party Account
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(.Cells(21).Value) & ",'" & .Cells(20).Value & "','" & .Cells(22).Value & "', '','','',''," & Val(ServerTag) & "," & Val(OrgID) & ""
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(FastQuery)
    End Sub
    Private Sub dg2Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        '   Dim cmd As SQLite.SQLiteCommand
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & Val(txtid.Text) & "'," &
                        "'" & .Cells("ChargeID").Value & "','" & .Cells("Charge Name").Value & "','" & .Cells("On Value").Value & "'," &
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
    Private Sub InsertCharges()
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    Dim ssql As String
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
                    Dim AccName As String = ssql
                    If .Cells(3).Value = "+" Then
                        '     clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text, .Cells(0).Value)
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','Voucher No:" & txtVoucherNo.Text & "','" & .Cells(4).Value & "','Voucher No:" & txtVoucherNo.Text & "'"
                    Else
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','Voucher No:" & txtVoucherNo.Text & "','" & .Cells(4).Value & "','Voucher No:" & txtVoucherNo.Text & "'"
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub

    Private Sub ServerInsertCharges()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    Dim ssql As String
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
                    Dim AccName As String = ssql
                    If .Cells(3).Value = "+" Then
                        '     clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, AcID, AccName, .Cells(4).Value, "C", Me.Text & ", Voucher No:" & txtVoucherNo.Text, .Cells(0).Value)
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'Voucher No:" & txtVoucherNo.Text & "','" & .Cells(4).Value & "','Voucher No:" & txtVoucherNo.Text & "'"
                    Else
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ",'Voucher No:" & txtVoucherNo.Text & "','" & .Cells(4).Value & "','Voucher No:" & txtVoucherNo.Text & "'"
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub


    Public Sub FillControls(ByVal id As Integer)
        'fill stock Record
        clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnSave.BackColor = Color.Coral
        BtnSave.Image = My.Resources.Edit
        BtnDelete.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & id
        Dim sql As String = "Select * from Transaction2 where VoucherID=" & id
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
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("AccountID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("DiscountAmount").ToString()), "0.00")
            txtTotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("billNo").ToString()
            txtotherCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txtTotRoundOff.Text = Format(Val(ds.Tables("a").Rows(0)("Roundoff").ToString()), "0.00")
            txtTotGross.Text = Format(Val(ds.Tables("a").Rows(0)("SubTotal").ToString()), "0.00")
            txtDriverName.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtDriverMobile.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtRemark.Text = ds.Tables("a").Rows(0)("T3").ToString()
            txtStateName.Text = ds.Tables("a").Rows(0)("T4").ToString()
            txtGSTN.Text = ds.Tables("a").Rows(0)("T5").ToString()
            txtCustMobile.Text = ds.Tables("a").Rows(0)("T6").ToString()
            txtBrokerName.Text = ds.Tables("a").Rows(0)("T7").ToString()
            txtBrokerMob.Text = ds.Tables("a").Rows(0)("T8").ToString()
            txtTransPort.Text = ds.Tables("a").Rows(0)("T9").ToString()
            txtGrNo.Text = ds.Tables("a").Rows(0)("T10").ToString()
            txtInvoiceID.Text = Val(ds.Tables("a").Rows(0)("InvoiceID").ToString())
        End If
        If ds.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells("Item Name").Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells("Lot No").Value = ds.Tables("b").Rows(i)("Lot").ToString()
                .Rows(i).Cells("Nug").Value = Val(ds.Tables("b").Rows(i)("Nug").ToString())
                .Rows(i).Cells("Weight").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells("Rate").Value = Format(Val(ds.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells("per").Value = ds.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells("ItemID").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells("Total").Value = Format(Val(ds.Tables("b").Rows(i)("TotalAmount").ToString()), "0.00")
                .Rows(i).Cells("CommPer").Value = Format(Val(ds.Tables("b").Rows(i)("CommPer").ToString()), "0.00")
                .Rows(i).Cells("CommAmt").Value = Format(Val(ds.Tables("b").Rows(i)("CommAmt").ToString()), "0.00")
                .Rows(i).Cells("Mper").Value = Format(Val(ds.Tables("b").Rows(i)("MPer").ToString()), "0.00")
                .Rows(i).Cells("MAmt").Value = Format(Val(ds.Tables("b").Rows(i)("Mamt").ToString()), "0.00")
                .Rows(i).Cells("RDFPer").Value = Format(Val(ds.Tables("b").Rows(i)("RDFPer").ToString()), "0.00")
                .Rows(i).Cells("RDFAmt").Value = Format(Val(ds.Tables("b").Rows(i)("RDFAmt").ToString()), "0.00")
                .Rows(i).Cells("TarePer").Value = Format(Val(ds.Tables("b").Rows(i)("Tare").ToString()), "0.00")
                .Rows(i).Cells("TareAmt").Value = Format(Val(ds.Tables("b").Rows(i)("TareAmt").ToString()), "0.00")
                .Rows(i).Cells("LabourPer").Value = Format(Val(ds.Tables("b").Rows(i)("Labour").ToString()), "0.00")
                .Rows(i).Cells("LabourAmt").Value = Format(Val(ds.Tables("b").Rows(i)("LabourAmt").ToString()), "0.00")
                .Rows(i).Cells("CrateID").Value = Val(ds.Tables("b").Rows(i)("CrateID").ToString())
                .Rows(i).Cells("CrateName").Value = ds.Tables("b").Rows(i)("Cratemarka").ToString()
                .Rows(i).Cells("CrateQty").Value = ds.Tables("b").Rows(i)("CrateQty").ToString()
                .Rows(i).Cells("CrateY/N").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells(24).Value = Val(ds.Tables("b").Rows(i)("CrateAccountID").ToString())
                .Rows(i).Cells(25).Value = ds.Tables("b").Rows(i)("CrateAccountName").ToString()
                .Rows(i).Cells(26).Value = Val(ds.Tables("b").Rows(i)("PurchaseID").ToString())
                .Rows(i).Cells(27).Value = Val(ds.Tables("b").Rows(i)("RoundOff").ToString())
                .Rows(i).Cells(28).Value = i + 1
                Dim tmpamt As Double = Val(ds.Tables("b").Rows(i)("CommAmt").ToString()) + Val(ds.Tables("b").Rows(i)("Mamt").ToString()) + Val(ds.Tables("b").Rows(i)("RDFAmt").ToString()) + Val(ds.Tables("b").Rows(i)("TareAmt").ToString()) + Val(ds.Tables("b").Rows(i)("LabourAmt").ToString())
                .Rows(i).Cells("TotalCharges").Value = Format(tmpamt, "0.00")
                Dim SellerID As Integer = Val(clsFun.ExecScalarInt("Select StockHolderID From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                Dim SellerName As String = clsFun.ExecScalarStr("Select StockHolderName From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                Dim StorageID As Integer = Val(clsFun.ExecScalarInt("Select StorageID From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                Dim StorageName As String = clsFun.ExecScalarStr("Select StorageName From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                Dim typeac As String = IIf(SellerID = 28, "Purchase", "Stock in")
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & Val(i + 1) & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," &
                            "'" & typeac & "'," & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "," & Val(SellerID) & ",'" & SellerName & "', " &
                            "'" & Val(StorageID) & "','" & StorageName & "','" & Val(ds.Tables("b").Rows(i)("ItemID").ToString()) & "','" & ds.Tables("b").Rows(i)("ItemName").ToString() & "'," &
                            "'" & Val(0) & "', '" & ds.Tables("b").Rows(i)("Lot").ToString() & "','" & Val(ds.Tables("b").Rows(i)("Nug").ToString()) & "','" & Val(ds.Tables("b").Rows(i)("Weight").ToString()) & "', " &
                            "'" & ds.Tables("b").Rows(i)("Per").ToString() & "'," & Val(txtid.Text) & ""
            Next
        End With
        Try
            sql = "insert into Stock(ID,ENTRYDATE,TRANSTYPE,PURCHASETYPENAME,PurchaseID,SELLERID,SELLERNAME,STORAGEID,StorageName," _
                           & " ITEMID,ITEMNAME,CUT,LOT,NUG,WEIGHT,PER,TransID) " & FastQuery & ""
            If clsFun.ExecNonQuery(sql) > 0 Then

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try

        If ds.Tables("c").Rows.Count > 0 Then
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    If Val(ds.Tables("c").Rows(i)("OnValue").ToString()) > 0 Then
                        .Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    End If
                    If Val(ds.Tables("c").Rows(i)("Calculate").ToString()) > 0 Then
                        .Rows(i).Cells("cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    End If
                    '.Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        calc()
        dg1.ClearSelection()
        Dg2.ClearSelection()
    End Sub
    Public Sub FillWithNevigation()
        If BtnSave.Text = "&Save" And dg1.RowCount > 0 Then MsgBox("Save Transaction First...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Standard Sale'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers  WHERE transtype = 'Standard Sale'   Order By ID LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString() + ""

        'sSql = "Select * from Vouchers where id=" & id

        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")

        If ds.Tables("a").Rows.Count > 0 Then
            BtnSave.Text = "&Update"
            BtnSave.BackColor = Color.Coral
            BtnSave.Image = My.Resources.Edit
            BtnDelete.Enabled = True
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("AccountID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Val(ds.Tables("a").Rows(0)("Nug").ToString())
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("DiscountAmount").ToString()), "0.00")
            txtTotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("billNo").ToString()
            txtotherCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txtTotRoundOff.Text = Format(Val(ds.Tables("a").Rows(0)("Roundoff").ToString()), "0.00")
            txtTotGross.Text = Format(Val(ds.Tables("a").Rows(0)("SubTotal").ToString()), "0.00")
            txtDriverName.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtDriverMobile.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtRemark.Text = ds.Tables("a").Rows(0)("T3").ToString()
            txtStateName.Text = ds.Tables("a").Rows(0)("T4").ToString()
            txtGSTN.Text = ds.Tables("a").Rows(0)("T5").ToString()
            txtCustMobile.Text = ds.Tables("a").Rows(0)("T6").ToString()
            txtBrokerName.Text = ds.Tables("a").Rows(0)("T7").ToString()
            txtBrokerMob.Text = ds.Tables("a").Rows(0)("T8").ToString()
            txtTransPort.Text = ds.Tables("a").Rows(0)("T9").ToString()
            txtGrNo.Text = ds.Tables("a").Rows(0)("T10").ToString()
            txtInvoiceID.Text = Val(ds.Tables("a").Rows(0)("InvoiceID").ToString())
        End If
        dg1.Rows.Clear()
        Dim sql As String = "Select * from Transaction2 where VoucherID=" & Val(txtid.Text)
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
                .Rows(i).Cells("Total").Value = Format(Val(ds.Tables("b").Rows(i)("TotalAmount").ToString()), "0.00")
                .Rows(i).Cells("CommPer").Value = Format(Val(ds.Tables("b").Rows(i)("CommPer").ToString()), "0.00")
                .Rows(i).Cells("CommAmt").Value = Format(Val(ds.Tables("b").Rows(i)("CommAmt").ToString()), "0.00")
                .Rows(i).Cells("Mper").Value = Format(Val(ds.Tables("b").Rows(i)("MPer").ToString()), "0.00")
                .Rows(i).Cells("MAmt").Value = Format(Val(ds.Tables("b").Rows(i)("Mamt").ToString()), "0.00")
                .Rows(i).Cells("RDFPer").Value = Format(Val(ds.Tables("b").Rows(i)("RDFPer").ToString()), "0.00")
                .Rows(i).Cells("RDFAmt").Value = Format(Val(ds.Tables("b").Rows(i)("RDFAmt").ToString()), "0.00")
                .Rows(i).Cells("TarePer").Value = Format(Val(ds.Tables("b").Rows(i)("Tare").ToString()), "0.00")
                .Rows(i).Cells("TareAmt").Value = Format(Val(ds.Tables("b").Rows(i)("TareAmt").ToString()), "0.00")
                .Rows(i).Cells("LabourPer").Value = Format(Val(ds.Tables("b").Rows(i)("Labour").ToString()), "0.00")
                .Rows(i).Cells("LabourAmt").Value = Format(Val(ds.Tables("b").Rows(i)("LabourAmt").ToString()), "0.00")
                .Rows(i).Cells("CrateID").Value = Val(ds.Tables("b").Rows(i)("CrateID").ToString())
                .Rows(i).Cells("CrateName").Value = ds.Tables("b").Rows(i)("Cratemarka").ToString()
                .Rows(i).Cells("CrateQty").Value = ds.Tables("b").Rows(i)("CrateQty").ToString()
                .Rows(i).Cells("CrateY/N").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells(24).Value = Val(ds.Tables("b").Rows(i)("CrateAccountID").ToString())
                .Rows(i).Cells(25).Value = ds.Tables("b").Rows(i)("CrateAccountName").ToString()
                .Rows(i).Cells(26).Value = Val(ds.Tables("b").Rows(i)("PurchaseID").ToString())
                .Rows(i).Cells(27).Value = Val(ds.Tables("b").Rows(i)("RoundOff").ToString())
                .Rows(i).Cells(28).Value = i + 1
                Dim tmpamt As Double = Val(ds.Tables("b").Rows(i)("CommAmt").ToString()) + Val(ds.Tables("b").Rows(i)("Mamt").ToString()) + Val(ds.Tables("b").Rows(i)("RDFAmt").ToString()) + Val(ds.Tables("b").Rows(i)("TareAmt").ToString()) + Val(ds.Tables("b").Rows(i)("LabourAmt").ToString())
                .Rows(i).Cells("TotalCharges").Value = Format(tmpamt, "0.00")
                .Rows(i).Cells("TotalCharges").Value = Format(tmpamt, "0.00")
                Dim SellerID As Integer = Val(clsFun.ExecScalarInt("Select StockHolderID From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                Dim SellerName As String = clsFun.ExecScalarStr("Select StockHolderName From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                Dim StorageID As Integer = Val(clsFun.ExecScalarInt("Select StorageID From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                Dim StorageName As String = clsFun.ExecScalarStr("Select StorageName From Purchase Where VoucherID='" & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                Dim typeac As String = IIf(SellerID = 28, "Purchase", "Stock in")
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & Val(i + 1) & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," &
                            "'" & typeac & "'," & Val(ds.Tables("b").Rows(i)("PurchaseID").ToString()) & "," & Val(SellerID) & ",'" & SellerName & "', " &
                            "'" & Val(StorageID) & "','" & StorageName & "','" & Val(ds.Tables("b").Rows(i)("ItemID").ToString()) & "','" & ds.Tables("b").Rows(i)("ItemName").ToString() & "'," &
                            "'" & Val(0) & "', '" & ds.Tables("b").Rows(i)("Lot").ToString() & "','" & Val(ds.Tables("b").Rows(i)("Nug").ToString()) & "','" & Val(ds.Tables("b").Rows(i)("Weight").ToString()) & "', " &
                            "'" & ds.Tables("b").Rows(i)("Per").ToString() & "'," & Val(txtid.Text) & ""
            Next
        End With
        Try
            sql = "insert into Stock(ID,ENTRYDATE,TRANSTYPE,PURCHASETYPENAME,PurchaseID,SELLERID,SELLERNAME,STORAGEID,StorageName," _
                           & " ITEMID,ITEMNAME,CUT,LOT,NUG,WEIGHT,PER,TransID) " & FastQuery & ""
            If clsFun.ExecNonQuery(sql) > 0 Then

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        Dg2.Rows.Clear()
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & Val(txtid.Text)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        ad2.Fill(ds, "c")
        If ds.Tables("c").Rows.Count > 0 Then
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    If Val(ds.Tables("c").Rows(i)("OnValue").ToString()) > 0 Then
                        .Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    End If
                    If Val(ds.Tables("c").Rows(i)("Calculate").ToString()) > 0 Then
                        .Rows(i).Cells("cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    End If
                    ' .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    '.Rows(i).Cells("On Value").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        calc()
        dg1.ClearSelection()
        Dg2.ClearSelection()
    End Sub
    Private Sub UpdateRecord()
        If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        If dgCharges.Visible = True Then dgCharges.Visible = False
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        dg1.ClearSelection()
        ' Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicleNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  AccountID='" & Val(txtAccountID.Text) & "', AccountName='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtbasicTotal.Text & "', TotalAmount='" & txtTotalNetAmount.Text & "',TotalCharges='" & txtotherCharges.Text & "',DiscountAmount='" & txttotalCharges.Text & "'," _
                                & " Subtotal= '" & txtTotGross.Text & "',T1= '" & txtDriverName.Text & "',T2= '" & txtDriverMobile.Text & "',T3= '" & txtRemark.Text & "',T4= '" & txtStateName.Text & "', " _
                                & " T5= '" & txtGSTN.Text & "',T6= '" & txtCustMobile.Text & "',T7= '" & txtBrokerName.Text & "',T8= '" & txtBrokerMob.Text & "',T9= '" & txtTransPort.Text & "', " _
                                & " T10= '" & txtGrNo.Text & "',RoundOff= '" & txtTotRoundOff.Text & "' , InVoiceID='" & Val(txtInvoiceID.Text) & "' where ID =" & Val(txtid.Text) & ""

        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
            End If
            ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
            UpdateCrate()
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
            End If
            ServerTag = 1
            Dg1Record() : dg2Record() : insertLedger() : InsertCharges() : CrateLedger()
            ServerinsertLedger() : ServerInsertCharges() : ServerCrateLedger()
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub


    Public Sub UpdateMulti()
        If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        If dgCharges.Visible = True Then dgCharges.Visible = False
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        dg1.ClearSelection()
        ' Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicleNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  AccountID='" & Val(txtAccountID.Text) & "', AccountName='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtbasicTotal.Text & "', TotalAmount='" & txtTotalNetAmount.Text & "',TotalCharges='" & txtotherCharges.Text & "',DiscountAmount='" & txttotalCharges.Text & "'," _
                                & " Subtotal= '" & txtTotGross.Text & "',T1= '" & txtDriverName.Text & "',T2= '" & txtDriverMobile.Text & "',T3= '" & txtRemark.Text & "',T4= '" & txtStateName.Text & "', " _
                                & " T5= '" & txtGSTN.Text & "',T6= '" & txtCustMobile.Text & "',T7= '" & txtBrokerName.Text & "',T8= '" & txtBrokerMob.Text & "',T9= '" & txtTransPort.Text & "', " _
                                & " T10= '" & txtGrNo.Text & "',RoundOff= '" & txtTotRoundOff.Text & "' , InVoiceID='" & Val(txtInvoiceID.Text) & "' where ID =" & Val(txtid.Text) & ""

        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
            End If
            ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
            UpdateCrate()
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
            End If
            ServerTag = 1
            Dg1Record() : dg2Record() : insertLedger() : InsertCharges() : CrateLedger()
            ServerinsertLedger() : ServerInsertCharges() : ServerCrateLedger()
            ' MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
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

    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub

    Private Sub delete()
        ButtonControl()
        If MessageBox.Show("Are you Sure want to Delete Sale Voucher ?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & txtid.Text & "; " &
                                   "DELETE from Vouchers WHERE ID=" & txtid.Text & "; " &
                                   "DELETE from Transaction2 WHERE VoucherID=" & txtid.Text & "; " &
                                   "DELETE from ChargesTrans WHERE VoucherID=" & txtid.Text & "; " &
                                   "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & txtid.Text & "") > 0 Then
                ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                           "Delete From  CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 0 : ServerinsertLedger() : ServerInsertCharges() : ServerCrateLedger()
                MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Delete")
            End If
            cleartxt()
            cleartxtCharges()
            FootertextClear()
            dg1.Rows.Clear()
            Dg2.Rows.Clear()
            BtnSave.Text = "&Save"
        End If
        MainScreenPicture.lblStdAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Standard Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        MainScreenPicture.lblStdTotal.Text = Val(clsFun.ExecScalarStr("Select Count(*) from Vouchers Where TransType='Standard Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        MainScreenPicture.retrive() : If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = True Then If Standard_Sale_Register.ckShowItems.Checked = False Then Standard_Sale_Register.retrive() Else Standard_Sale_Register.retrive1()
        If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
        ButtonControl()
    End Sub

    'Private Sub itemfill()
    '    If dg1.SelectedRows.Count = 0 Then
    '        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '        txtComPer.Text = clsFun.ExecScalarStr(" Select CommisionPer FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '        txtMPer.Text = clsFun.ExecScalarStr(" Select UserChargesPer FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '        txtTare.Text = clsFun.ExecScalarStr(" Select tare FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '        txtLabour.Text = clsFun.ExecScalarStr(" Select Labour FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '        txtRdfPer.Text = clsFun.ExecScalarStr(" Select rdfper FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '    End If
    '    If dg1.SelectedRows.Count = 0 Then txtKg.Text = clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '    Dim Rate As String = clsFun.ExecScalarStr("SELECT Rate FROM PartyRates WHERE AccountID = '" & Val(txtAccountID.Text) & "' AND ItemID = '" & Val(txtItemID.Text) & "'")
    '    If Val(Rate) <> 0 Then txtRate.Text = Format(Val(Rate), "0.00")
    '    AccountComm()
    'End Sub
    Private Sub ItemFill()
        Dim itemID As Integer = Val(txtItemID.Text)
        If itemID = 0 Then Exit Sub ' Prevent unnecessary queries if ItemID is invalid
        ' Use a single query to fetch all required fields
        Dim query As String = "SELECT CommisionPer, UserChargesPer, Tare, Labour, RDFPer, MaintainCrate, WeightPerNug,TrackStock,RateAs FROM Items WHERE ID = " & itemID
        Dim dt As DataTable = clsFun.ExecDataTable(query) ' Assume ExecDataTable returns a DataTable
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)
            txtComPer.Text = row("CommisionPer").ToString()
            txtMPer.Text = row("UserChargesPer").ToString()
            txtTare.Text = row("Tare").ToString()
            txtLabour.Text = row("Labour").ToString()
            txtRdfPer.Text = row("RDFPer").ToString()
            lblCrate.Text = row("MaintainCrate").ToString()
            CbPer.Text = row("RateAs").ToString()
            trackStock = row("TrackStock").ToString()
        End If
        Dim Rate As String = clsFun.ExecScalarStr("SELECT Rate FROM PartyRates WHERE AccountID = '" & Val(txtAccountID.Text) & "' AND ItemID = '" & Val(txtItemID.Text) & "'")
        If Val(Rate) <> 0 Then txtRate.Text = Format(Val(Rate), "0.00")
        AccountComm()
    End Sub
    'Private Sub ItemFill()
    '    Dim itemID As Integer = Val(txtItemID.Text)
    '    If itemID = 0 Then Exit Sub ' Prevent unnecessary queries if ItemID is invalid
    '    ' Use a single query to fetch all required fields
    '    Dim query As String = "SELECT CommisionPer, UserChargesPer, Tare, Labour, RDFPer, MaintainCrate, WeightPerNug,TrackStock FROM Items WHERE ID = " & itemID
    '    Dim dt As DataTable = clsFun.ExecDataTable(query) ' Assume ExecDataTable returns a DataTable
    '    If dt.Rows.Count > 0 Then
    '        Dim row As DataRow = dt.Rows(0)
    '        txtComPer.Text = row("CommisionPer").ToString()
    '        txtMPer.Text = row("UserChargesPer").ToString()
    '        txtTare.Text = row("Tare").ToString()
    '        txtLabour.Text = row("Labour").ToString()
    '        txtRdfPer.Text = row("RDFPer").ToString()
    '        lblCrate.Text = row("MaintainCrate").ToString()
    '        trackStock = row("TrackStock").ToString()
    '    End If
    '    AccountComm()
    'End Sub
    Private Sub AccountComm()
        If BtnSave.Text = "&Save" Then
            Dim acccomm As Decimal = 0.0 : Dim Mper As Decimal = 0.0
            Dim RdfPer As Decimal = 0.0 : Dim TarePer As Decimal = 0.0
            Dim LabourPer As Decimal = 0.0
            acccomm = Val(clsFun.ExecScalarStr("Select CommPer From Accounts Where ID= '" & Val(txtAccountID.Text) & "'"))
            Mper = Val(clsFun.ExecScalarStr("Select Mper From Accounts Where ID= '" & Val(txtAccountID.Text) & "'"))
            RdfPer = Val(clsFun.ExecScalarStr("Select RdfPer From Accounts Where ID= '" & Val(txtAccountID.Text) & "'"))
            TarePer = Val(clsFun.ExecScalarStr("Select TarePer From Accounts Where ID= '" & Val(txtAccountID.Text) & "'"))
            LabourPer = Val(clsFun.ExecScalarStr("Select LabourPer From Accounts Where ID= '" & Val(txtAccountID.Text) & "'"))
            If acccomm > 0 And Val(txtComPer.Text) <> 0 Then txtComPer.Text = acccomm
            If Mper > 0 And Val(txtMPer.Text) <> 0 Then txtMPer.Text = Mper
            If RdfPer > 0 And Val(txtRdfPer.Text) <> 0 Then txtRdfPer.Text = RdfPer
            If TarePer > 0 And Val(txtTare.Text) <> 0 Then txtTare.Text = TarePer
            If LabourPer > 0 And Val(txtLabour.Text) <> 0 Then txtLabour.Text = LabourPer
        End If

    End Sub
    Private Sub txtInvoiceID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceID.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlInvoiceID.Visible = False
            txtVoucherNo.Focus()
        End If
    End Sub
    Private Sub VNumber()
        'vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        'txtVoucherNo.Text = vno + 1
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
    Private Sub rowColums()
        dg1.ColumnCount = 29
        dg1.Columns(0).Name = "Item Name" : dg1.Columns(0).Width = 371
        dg1.Columns(1).Name = "Lot No" : dg1.Columns(1).Width = 119
        dg1.Columns(2).Name = "Nug" : dg1.Columns(2).Width = 119
        dg1.Columns(3).Name = "Weight" : dg1.Columns(3).Width = 119
        dg1.Columns(4).Name = "Rate" : dg1.Columns(4).Width = 119
        dg1.Columns(5).Name = "per" : dg1.Columns(5).Width = 87
        dg1.Columns(6).Name = "Amount" : dg1.Columns(6).Width = 120
        dg1.Columns(7).Name = "Total" : dg1.Columns(7).Width = 120
        dg1.Columns(8).Name = "ItemID" : dg1.Columns(8).Width = 50
        dg1.Columns(9).Name = "CommPer" : dg1.Columns(9).Width = 50
        dg1.Columns(10).Name = "CommAmt" : dg1.Columns(10).Width = 50
        dg1.Columns(11).Name = "MPer" : dg1.Columns(11).Width = 50
        dg1.Columns(12).Name = "MAmt" : dg1.Columns(12).Width = 50
        dg1.Columns(13).Name = "RDFPer" : dg1.Columns(13).Width = 50
        dg1.Columns(14).Name = "RdfAmt" : dg1.Columns(14).Width = 50
        dg1.Columns(15).Name = "TarePer" : dg1.Columns(15).Width = 50
        dg1.Columns(16).Name = "TareAmt" : dg1.Columns(16).Width = 50
        dg1.Columns(17).Name = "LabourPer" : dg1.Columns(17).Width = 50
        dg1.Columns(18).Name = "LabourAmt" : dg1.Columns(18).Width = 50
        dg1.Columns(19).Name = "CrateY/N" : dg1.Columns(19).Width = 50
        dg1.Columns(20).Name = "CrateName" : dg1.Columns(20).Width = 50
        dg1.Columns(21).Name = "CrateID" : dg1.Columns(21).Width = 50
        dg1.Columns(22).Name = "CrateQty" : dg1.Columns(22).Width = 50
        dg1.Columns(23).Name = "TotalCharges" : dg1.Columns(23).Width = 50
        dg1.Columns(24).Name = "CrateAccountID" : dg1.Columns(24).Width = 50
        dg1.Columns(25).Name = "CrateAccountName" : dg1.Columns(25).Width = 50
        dg1.Columns(26).Name = "PurchaseID" : dg1.Columns(26).Width = 50
        dg1.Columns(27).Name = "Roundoff" : dg1.Columns(27).Width = 50
        dg1.Columns(28).Name = "RecordID" : dg1.Columns(28).Width = 50
        Dg2.ColumnCount = 7
        Dg2.Columns(0).Name = "Charge Name" : Dg2.Columns(0).Width = 259
        Dg2.Columns(1).Name = "On Value" : Dg2.Columns(1).Width = 113
        Dg2.Columns(2).Name = "Cal" : Dg2.Columns(2).Width = 114
        Dg2.Columns(3).Name = "+/-" : Dg2.Columns(3).Width = 114
        Dg2.Columns(4).Name = "Amount" : Dg2.Columns(4).Width = 114
        Dg2.Columns(5).Name = "ChargeID" : Dg2.Columns(5).Width = 110
        Dg2.Columns(6).Name = "ID" : Dg2.Columns(6).Visible = False
    End Sub
    Sub calc()
        txtTotalNug.Text = Format(0, "0.00") : txtbasicTotal.Text = Format(0, "0.00")
        txtTotGross.Text = Format(0, "0.00") : txttotalWeight.Text = Format(0, "0.00")
        txttotalCharges.Text = Format(0, "0.00") : txtTotGross.Text = Format(0, "0.00")
        lbltotCom.Text = Format(0, "0.00") : lblTotMandi.Text = Format(0, "0.00")
        lblTotRdf.Text = Format(0, "0.00") : lblTotLabour.Text = Format(0, "0.00")
        lblTotBardana.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1  ''''For Items Dg1
            txtTotalNug.Text = Format(Val(txtTotalNug.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txttotalWeight.Text = Format(Val(txttotalWeight.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtbasicTotal.Text = Format(Val(txtbasicTotal.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txttotalCharges.Text = Format(Val(txttotalCharges.Text) + Val(dg1.Rows(i).Cells(23).Value), "0.00")
            txtTotGross.Text = Format(Val(txtTotGross.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            lbltotCom.Text = Format(Val(lbltotCom.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            lblTotMandi.Text = Format(Val(lblTotMandi.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            lblTotRdf.Text = Format(Val(lblTotRdf.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
            lblTotBardana.Text = Format(Val(lblTotBardana.Text) + Val(dg1.Rows(i).Cells(16).Value), "0.00")
            lblTotLabour.Text = Format(Val(lblTotLabour.Text) + Val(dg1.Rows(i).Cells(18).Value), "0.00")
        Next
        txtotherCharges.Text = Format(0, "0.00")
        For i = 0 To Dg2.Rows.Count - 1
            If Dg2.Rows(i).Cells(3).Value = "-" Then
                txtotherCharges.Text = Format(Val(txtotherCharges.Text) - Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            Else
                txtotherCharges.Text = Format(Val(txtotherCharges.Text) + Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            End If
        Next
        'For i = 0 To Dg2.Rows.Count - 1
        '    Dim CalcType As String = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
        '    Dim PlusMinus As String = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
        '    If CalcType = "Aboslute" Then
        '        Dg2.Rows(i).Cells(4).Value = Dg2.Rows(i).Cells(4).Value
        '    ElseIf CalcType = "Percentage" Then
        '        '        Dg2.Rows(i).Cells(1).Value = Format(Val(txtbasicTotal.Text), "0.00")
        '        Dg2.Rows(i).Cells(4).Value = Format(Val(Val(Dg2.Rows(i).Cells(1).Value) * Val(Dg2.Rows(i).Cells(2).Value) / 100), "0.00")
        '    ElseIf CalcType = "Nug" Then
        '        '       Dg2.Rows(i).Cells(1).Value = Format(Val(txtTotalNug.Text), "0.00")
        '        Dg2.Rows(i).Cells(4).Value = Format(Val(Dg2.Rows(i).Cells(1).Value) * Val(Dg2.Rows(i).Cells(2).Value), "0.00")
        '    ElseIf CalcType = "Weight" Then
        '        '      Dg2.Rows(i).Cells(1).Value = Format(Val(txttotalWeight.Text), "0.00")
        '        Dg2.Rows(i).Cells(4).Value = Format(Val(Dg2.Rows(i).Cells(1).Value) * Val(Dg2.Rows(i).Cells(2).Value), "0.00")
        '    End If
        '    If Dg2.Rows(i).Cells(3).Value = "-" Then
        '        txtotherCharges.Text = Format(Val(txtotherCharges.Text) - Val(Dg2.Rows(i).Cells(4).Value), "0.00")
        '    Else
        '        txtotherCharges.Text = Format(Val(txtotherCharges.Text) + Val(Dg2.Rows(i).Cells(4).Value), "0.00")
        '    End If
        'Next
        txtTotalNetAmount.Text = Format(Val(txtTotGross.Text) + Val(txtotherCharges.Text), "0.00")
        Dim tmpCustAmount As Double = Val(Val(txtTotGross.Text) + Val(txtotherCharges.Text))
        txtTotalNetAmount.Text = Math.Round(Val(tmpCustAmount), 0)
        txtTotRoundOff.Text = Format(Val(txtTotalNetAmount.Text) - Val(tmpCustAmount), "0.00")
        txtTotalNetAmount.Text = Format(Val(txtTotalNetAmount.Text), "0.00")
    End Sub
    Private Sub cbitem_GotFocus(sender As Object, e As EventArgs)
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper <> DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper = DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            itemfill() : AcBal()
            DgAccountSearch.Visible = False
        Else
            txtAccount.Focus()
        End If
    End Sub
    Private Sub txtLot_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLot.KeyPress
        dgLot.BringToFront() : dgLot.Visible = True
    End Sub
    Private Sub txtLot_KeyUp(sender As Object, e As KeyEventArgs) Handles txtLot.KeyUp
        If txtLot.Text.Trim() <> "" Then
            RetriveLot(" And upper(LotNo) like upper('%" & txtLot.Text.Trim() & "%')")
            SelectMatchingRow(txtLot.Text.Trim())
        Else
            RetriveLot()
        End If
    End Sub
    Private Sub SelectMatchingRow(ByVal searchText As String)
        For Each row As DataGridViewRow In dgLot.Rows
            If row.Cells(1).Value IsNot Nothing AndAlso row.Cells(1).Value.ToString().ToUpper().Contains(searchText.ToUpper()) Then
                row.Selected = True
                dgLot.FirstDisplayedScrollingRowIndex = row.Index
                Exit Sub
            End If
        Next
    End Sub


    Private Sub txtLotNo_GotFocus(sender As Object, e As EventArgs) Handles txtLot.GotFocus
        dgLot.Visible = True
        If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
        If dgItemSearch.Rows.Count = 0 Then retriveItems()
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus() : Exit Sub
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        dgItemSearch.Visible = False : itemfill()
        If dgLot.ColumnCount = 0 Then LotCoulmns()
        If dgLot.RowCount = 0 Then RetriveLot()
        If txtLot.Text.Trim() <> "" Then
            RetriveLot(" And upper(LotNo) like upper('%" & txtLot.Text.Trim() & "%')")
        Else
            RetriveLot()
        End If
        txtLot.SelectionStart = 0 : txtLot.SelectionLength = Len(txtLot.Text)
    End Sub

    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus
        dgLot.Visible = False
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
        If dgItemSearch.RowCount = 0 Then retriveAccounts()
        If txtItem.Text.Trim() <> "" Then
            retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            retriveItems()
        End If
        retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : AcBal()
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True
        txtItem.SelectionStart = 0 : txtItem.SelectionLength = Len(txtItem.Text)
    End Sub
    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear() : txtItemID.Clear()
        txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        itemfill() : dgItemSearch.Visible = False
        If txtLot.TabStop = True Then txtLot.Focus() Else txtNug.Focus()
        Dim CutStop As String = String.Empty
    End Sub
    Private Sub ItemRowColumns()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 186
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 186
        retriveItems()
    End Sub

    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        ItemRowColumns()
        dgItemSearch.Visible = True
        If txtItem.Text.Trim() <> "" Then
            dgItemSearch.Visible = True
            retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
    End Sub
    Private Sub retriveItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable

        dt = clsFun.ExecDataTable("Select * from Items " & condtion & " order by ItemName Limit 10")
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
    Private Sub LotBalance()
        Dim sql As String = String.Empty
        'If dg1.SelectedRows.Count > 0 AndAlso dg1.Rows.Count > 0 AndAlso dg1.SelectedRows(0).Cells.Count > 36 Then
        '    excludeCondition = "AND STOCK.ID <> " & Val(dg1.SelectedRows(0).Cells(36).Value)
        '    If Val(txtid.Text) <> 0 Then excludeID = "and t.VoucherID <>" & Val(txtid.Text)
        'End If
        If Val(txtid.Text) <> 0 Then excludeID = "S.PURCHASEID <> P.VOUCHERID" Else excludeID = "S.PURCHASEID = P.VOUCHERID"
        If trackStock = "Nug" Then
            sql = "SELECT " & _
                  " COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
                  " AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
                  " + COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug " & _
                  " FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.ItemID = " & Val(txtItemID.Text) & " " & _
                  " and P.VoucherID=" & Val(txtPurchaseID.Text) & "  AND  p.LotNo='" & txtLot.Text & "'  " & condtion & " " & _
                  " GROUP BY P.VOUCHERID, P.ENTRYDATE, P.ItemID, P.ItemName, P.StockHolderName, P.LOTNO, P.VEHICLENO " & _
                  " HAVING RestNug > 0 ORDER BY P.EntryDate;"
            LotBal = Format(Val(clsFun.ExecScalarStr(sql)), "0")
            If dg1.SelectedRows.Count <> 0 Then
                If dg1.SelectedRows(0).Cells(1).Value.trim = txtLot.Text.Trim AndAlso Val(dg1.SelectedRows(0).Cells(8).Value) = Val(txtItemID.Text) AndAlso Val(dg1.SelectedRows(0).Cells(26).Value) = Val(txtPurchaseID.Text.Trim) Then
                    SelectedLotBal = Val(SelectedLotBal) + Val(dg1.SelectedRows(0).Cells(2).Value)
                End If
            End If
            LotBal = Val(LotBal) + Val(SelectedLotBal)
            lblLot.Text = "Lot Bal. (Nug): " & Format(Val(LotBal), "0.00")
            lblLot.Visible = True
        Else
            sql = "SELECT  " & _
                  " COALESCE(SUM(P.Weight), 0) - (COALESCE((SELECT SUM(T.Weight) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
                  " AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
                  " + COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestWeight " & _
                  " FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.ItemID = " & Val(txtItemID.Text) & " " & _
                  " and P.VoucherID=" & Val(txtPurchaseID.Text) & "  AND  p.LotNo='" & txtLot.Text & "'  " & condtion & " " & _
                  " GROUP BY P.VOUCHERID, P.ENTRYDATE, P.ItemID, P.ItemName, P.StockHolderName, P.LOTNO, P.VEHICLENO " & _
                  " HAVING  RestWeight > 0 ORDER BY P.EntryDate;"
            LotBal = Format(Val(clsFun.ExecScalarStr(sql)), "0")
            If dg1.SelectedRows.Count <> 0 Then
                If dg1.SelectedRows(0).Cells(1).Value.trim = txtLot.Text.Trim AndAlso Val(dg1.SelectedRows(0).Cells(8).Value) = Val(txtItemID.Text) AndAlso Val(dg1.SelectedRows(0).Cells(26).Value) = Val(txtPurchaseID.Text.Trim) Then
                    SelectedLotWeight = Val(SelectedLotWeight) + Val(dg1.SelectedRows(0).Cells(3).Value)
                End If
            End If
            LotBal = Val(LotBal) + Val(SelectedLotWeight)
            lblLot.Text = "Lot Bal. (Weight): " & Format(Val(LotBal), "0.00")
            lblLot.Visible = True
        End If
    End Sub
    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtItem.Clear()
            txtItemID.Clear()
            txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            itemfill()
            dgItemSearch.Visible = False
            If txtLot.TabStop = True Then txtLot.Focus() Else txtNug.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.F3 Then
            Item_form.MdiParent = MainScreenForm
            Item_form.Show() : Item_form.Opener = Me
            Item_form.OpenedFromItems = True
            Item_form.BringToFront()
        End If
        If e.KeyCode = Keys.F1 Then
            Dim ItemID As String = dgItemSearch.SelectedRows(0).Cells(0).Value
            Item_form.MdiParent = MainScreenForm
            Item_form.Show() : Item_form.Opener = Me
            Item_form.OpenedFromItems = True
            Item_form.FillContros(ItemID)
            Item_form.BringToFront()
        End If
    End Sub
    Private Sub LotCoulmns()
        dgLot.ColumnCount = 7
        dgLot.Columns(0).Name = "LotID" : dgLot.Columns(0).Visible = False
        dgLot.Columns(1).Name = "Lot" : dgLot.Columns(1).Width = 100
        dgLot.Columns(2).Name = "Vehicle No." : dgLot.Columns(2).Width = 120
        dgLot.Columns(3).Name = "Date" : dgLot.Columns(3).Width = 100
        dgLot.Columns(4).Name = "Account Name" : dgLot.Columns(4).Width = 200
        dgLot.Columns(5).Name = "Nug" : dgLot.Columns(5).Width = 80
        dgLot.Columns(6).Name = "Weight" : dgLot.Columns(6).Width = 80
        dgLot.Visible = True
    End Sub

    Private Sub BalanceRecord()
        If dg1.SelectedRows.Count = 1 Then clsFun.ExecScalarStr("Delete From Stock Where ID='" & Val(dg1.SelectedRows(0).Cells(28).Value) & "' ")
        Dim FastQuery As String = String.Empty
        Dim RecordCount As Integer = If(dg1.SelectedRows.Count = 1, Val(dg1.SelectedRows(0).Cells(28).Value), (dg1.Rows.Count) + 1)
        Dim Sql As String = String.Empty
        Dim SellerID As Integer = Val(clsFun.ExecScalarInt("Select StockHolderID From Purchase Where VoucherID='" & Val(txtPurchaseID.Text) & "'"))
        Dim SellerName As String = clsFun.ExecScalarStr("Select StockHolderName From Purchase Where VoucherID='" & Val(txtPurchaseID.Text) & "'")
        Dim StorageID As Integer = Val(clsFun.ExecScalarInt("Select StorageID From Purchase Where VoucherID='" & Val(txtPurchaseID.Text) & "'"))
        Dim StorageName As String = clsFun.ExecScalarStr("Select StorageName From Purchase Where VoucherID='" & Val(txtPurchaseID.Text) & "'")
        Dim typeac As String = IIf(SellerID = 28, "Purchase", "Stock in")
        FastQuery = " SELECT '" & Val(RecordCount) & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," &
            "'" & typeac & "'," & Val(txtPurchaseID.Text) & "," & Val(SellerID) & ",'" & SellerName & "', " &
            "'" & Val(StorageID) & "','" & StorageName & "','" & Val(txtItemID.Text) & "','" & txtItem.Text & "'," &
            "'" & Val(0) & "', '" & txtLot.Text & "','" & Val(txtNug.Text) & "','" & Val(txtKg.Text) & "', " &
            "'" & Cbper.Text & "'," & Val(txtid.Text) & ""
        Try
            Sql = "insert into Stock(ID,ENTRYDATE,TRANSTYPE,PURCHASETYPENAME,PurchaseID,SELLERID,SELLERNAME,STORAGEID,StorageName," _
                           & " ITEMID,ITEMNAME,CUT,LOT,NUG,WEIGHT,PER,TransID) " & FastQuery & ""
            If clsFun.ExecNonQuery(Sql) > 0 Then

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        clsFun.CloseConnection()
    End Sub

    Private Sub RetriveLot(Optional ByVal condtion As String = "")
        dgLot.Rows.Clear()
        Dim lastval As Integer = 0
        Dim sql As String = String.Empty
        Dim dt As DataTable
        Dim Calculate As String = IIf(trackStock = "Nug", "RestNug", "RestWeight")

        ' SQL Query
        sql = "SELECT P.VOUCHERID, P.ENTRYDATE, P.ItemID, P.ItemName, P.StockHolderName, P.LOTNO, P.VEHICLENO, " & _
              "COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
              "AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
              "+ COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug, " & _
              "COALESCE(SUM(P.Weight), 0) - (COALESCE((SELECT SUM(T.Weight) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
              "AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
              "+ COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestWeight " & _
              "FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.ItemID = " & Val(txtItemID.Text) & " " & condtion & " " & _
              "GROUP BY P.VOUCHERID, P.ENTRYDATE, P.ItemID, P.ItemName, P.StockHolderName, P.LOTNO, P.VEHICLENO " & _
              "HAVING " & Calculate & " >= 0 ORDER BY P.EntryDate;"

        ' Add "N/A" Row
        dgLot.Rows.Add()
        With dgLot.Rows(lastval)
            .Cells(0).Value = 0
            .Cells(1).Value = "N/A"
            .Cells(2).Value = "N/A"
            .Cells(3).Value = "N/A"
            .Cells(4).Value = "N/A"
            .Cells(5).Value = "N/A"
            .Cells(6).Value = "N/A"
        End With
        lastval += 1

        ' Execute SQL and Process Rows
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Dim LotBalCheck As Double = Val(dt.Rows(i)("RestNug").ToString())
                    Dim LotWeightCheck As Double = Val(dt.Rows(i)("RestWeight").ToString())
                    Dim addRecord As Boolean = False

                    ' Check Selected Rows in dg1
                    If dg1.SelectedRows.Count > 0 Then
                        If dg1.SelectedRows(0).Cells(1).Value.ToString() = dt.Rows(i)("LotNo").ToString() AndAlso _
                           Val(dg1.SelectedRows(0).Cells(8).Value) = Val(txtItemID.Text) AndAlso _
                           Val(dg1.SelectedRows(0).Cells(26).Value) = Val(dt.Rows(i)("VoucherID").ToString()) Then

                            LotBalCheck += Val(dg1.SelectedRows(0).Cells(2).Value)
                            LotWeightCheck += Val(dg1.SelectedRows(0).Cells(3).Value)
                            addRecord = True
                        End If
                    End If

                    ' Add Record if Balance or Selected Row is Valid
                    If LotBalCheck > 0 Or LotWeightCheck > 0 Or addRecord Then
                        dgLot.Rows.Add()
                        With dgLot.Rows(lastval)
                            .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                            .Cells(1).Value = dt.Rows(i)("LotNo").ToString()
                            .Cells(2).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(3).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                            .Cells(4).Value = dt.Rows(i)("StockHolderName").ToString()
                            .Cells(5).Value = Format(LotBalCheck, "0")
                            .Cells(6).Value = Format(LotWeightCheck, "0.00")
                        End With
                        lastval += 1
                    End If
                Next
            End If
        Catch ex As Exception
            ' Handle exception
        End Try
    End Sub

    'Private Sub RetriveLot(Optional ByVal condtion As String = "")
    '    dgLot.Rows.Clear()
    '    Dim lastval As Integer = 0
    '    Dim sql As String = String.Empty
    '    Dim excludeCondition As String=String.Empty
    '    Dim excludeID As String = String.Empty
    '    Dim dt As DataTable
    '    Dim Calculate As String = IIf(trackStock = "Nug", "RestNug", "RestWeight")
    '    sql = "SELECT P.VOUCHERID, P.ENTRYDATE, P.ItemID, P.ItemName, P.StockHolderName, P.LOTNO, P.VEHICLENO, " & _
    '          "COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
    '          "AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
    '          "+ COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug, " & _
    '          "COALESCE(SUM(P.Weight), 0) - (COALESCE((SELECT SUM(T.Weight) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
    '          "AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
    '          "+ COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestWeight " & _
    '          "FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.ItemID = " & Val(txtItemID.Text) & " " & condtion & " " & _
    '          "GROUP BY P.VOUCHERID, P.ENTRYDATE, P.ItemID, P.ItemName, P.StockHolderName, P.LOTNO, P.VEHICLENO " & _
    '          "HAVING " & Calculate & " > 0 ORDER BY P.EntryDate;"

    '    dt = clsFun.ExecDataTable(sql)
    '    dgLot.Rows.Add()
    '    With dgLot.Rows(lastval)
    '        .Cells(0).Value = 0
    '        .Cells(1).Value = "N/A"
    '        .Cells(2).Value = "N/A"
    '        .Cells(3).Value = "N/A"
    '        .Cells(4).Value = "N/A"
    '        .Cells(5).Value = "N/A"
    '        .Cells(6).Value = "N/A"
    '        'lastval = lastval + 1
    '    End With

    '    Try
    '        lastval = 1
    '        If dt.Rows.Count > 0 Then
    '            For i = 0 To dt.Rows.Count - 1
    '                ' Application.DoEvents()
    '                LotBalCheck = 0 : SelectedLotBal = 0
    '                LotWeightCheck = 0 : SelectedLotWeight = 0
    '                If dg1.SelectedRows.Count <> 0 Then
    '                    If dg1.SelectedRows(0).Cells(1).Value = dt.Rows(i)("LotNo").ToString() AndAlso Val(dg1.SelectedRows(0).Cells(8).Value) = Val(txtItemID.Text) AndAlso Val(dg1.SelectedRows(0).Cells(26).Value) = Val(dt.Rows(i)("VoucherID").ToString()) Then
    '                        SelectedLotBal = Val(SelectedLotBal) + Val(dg1.SelectedRows(0).Cells(2).Value)
    '                        SelectedLotWeight = Val(SelectedLotWeight) + Val(dg1.SelectedRows(0).Cells(3).Value)
    '                    End If
    '                End If
    '                LotBalCheck = Val(Val(dt.Rows(i)("RestNug").ToString())) + Val(SelectedLotBal)
    '                LotWeightCheck = Val(dt.Rows(i)("RestWeight").ToString()) + Val(SelectedLotWeight)
    '                If If(trackStock = "Nug", Val(LotBalCheck), Val(LotWeightCheck)) > 0 Then
    '                    dgLot.Rows.Add()
    '                    With dgLot.Rows(lastval)
    '                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
    '                        .Cells(1).Value = dt.Rows(i)("LotNo").ToString()
    '                        .Cells(2).Value = dt.Rows(i)("VehicleNo").ToString()
    '                        .Cells(3).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
    '                        .Cells(4).Value = dt.Rows(i)("StockHolderName").ToString()
    '                        .Cells(5).Value = Val(LotBalCheck)
    '                        .Cells(6).Value = Format(Val(LotWeightCheck), "0.00")
    '                        lastval = lastval + 1
    '                    End With
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
    Private Sub txtRate_GotFocus(sender As Object, e As EventArgs) Handles txtRate.GotFocus
        txtRate.SelectionStart = 0 : txtRate.SelectionLength = Len(txtRate.Text)
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.BackColor = Color.LightGray
        mskEntryDate.SelectAll()
    End Sub

    Private Sub txtVoucherNo_GotFocus(sender As Object, e As EventArgs) Handles txtVoucherNo.GotFocus, txtVehicleNo.GotFocus,
        txtLot.GotFocus, txtKg.GotFocus, txtRate.GotFocus, txtAccount.GotFocus, txtItem.GotFocus, txtDriverName.GotFocus,
        txtDriverMobile.GotFocus, txtRemark.GotFocus, txtStateName.GotFocus, txtGSTN.GotFocus, txtCustMobile.GotFocus, txtBrokerName.GotFocus, txtBrokerMob.GotFocus,
        txtTransPort.GotFocus, txtGrNo.GotFocus, txtComPer.GotFocus, txtLaboutAmt.GotFocus, txtComAmt.GotFocus, txtMPer.GotFocus, txtMAmt.GotFocus,
        txtRdfPer.GotFocus, txtRdfAmt.GotFocus, txtTare.GotFocus, txtTareAmt.GotFocus, txtLabour.GotFocus, txtTotal.GotFocus
        If txtNug.Focused AndAlso lblCrate.Text = "Y" Then pnlMarka.Visible = True Else pnlMarka.Visible = False
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub

    Private Sub txtVoucherNo_LostFocus(sender As Object, e As EventArgs) Handles txtVoucherNo.LostFocus, txtVehicleNo.LostFocus,
    txtLot.LostFocus, txtKg.LostFocus, txtRate.LostFocus, txtAccount.LostFocus, txtItem.LostFocus, txtDriverName.LostFocus,
    txtDriverMobile.LostFocus, txtRemark.LostFocus, txtStateName.LostFocus, txtGSTN.LostFocus, txtCustMobile.LostFocus, txtBrokerName.LostFocus, txtBrokerMob.LostFocus,
    txtTransPort.LostFocus, txtGrNo.LostFocus, txtComPer.LostFocus, txtLaboutAmt.LostFocus, txtComAmt.LostFocus, txtMPer.LostFocus, txtMAmt.LostFocus, txtRdfPer.LostFocus,
    txtRdfAmt.LostFocus, txtTare.LostFocus, txtTareAmt.LostFocus, txtLabour.LostFocus, txtTotal.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        If tb Is txtComPer Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtComAmt Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtMPer Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtMAmt Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtRdfPer Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtRdfAmt Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtLabour Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtLaboutAmt Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtTare Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        ElseIf tb Is txtTareAmt Then
            tb.BackColor = Color.FromArgb(247, 220, 111)
        Else
            tb.BackColor = Color.GhostWhite
        End If
    End Sub

    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32", "GroupName", "ID", "")
            CreateAccount.BringToFront()
        End If

        If e.KeyCode = Keys.F1 Then
            If DgAccountSearch.SelectedRows.Count = 0 Then AccountRowColumns()
            If txtAccount.Text.Trim() <> "" Then
                retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
            End If
            Dim AccountID As Integer = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show() : CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            CreateAccount.FillContros(AccountID)
            CreateAccount.BringToFront()
        End If
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
            If dgItemSearch.SelectedRows.Count = 0 Then ItemRowColumns()
            If txtItem.Text.Trim() <> "" Then
                dgItemSearch.Visible = True
                retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
            End If
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

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown, txtVehicleNo.KeyDown,
          txtLot.KeyDown, txtKg.KeyDown, txtRate.KeyDown, Cbper.KeyDown, txtAccount.KeyDown, txtItem.KeyDown, txtDriverName.KeyDown,
          txtDriverMobile.KeyDown, txtRemark.KeyDown, txtStateName.KeyDown, txtGSTN.KeyDown, txtCustMobile.KeyDown, txtBrokerName.KeyDown, txtBrokerMob.KeyDown,
        txtTransPort.KeyDown, txtGrNo.KeyDown, txtComPer.KeyDown, txtLaboutAmt.KeyDown,
        txtComAmt.KeyDown, txtMPer.KeyDown, txtMAmt.KeyDown, txtRdfPer.KeyDown, txtRdfAmt.KeyDown, txtTare.KeyDown, txtTareAmt.KeyDown, txtLabour.KeyDown
        If txtVoucherNo.Focused Then
            If e.KeyCode = Keys.F2 Then
                pnlInvoiceID.Visible = False
                txtInvoiceID.Focus()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If txtLot.Focused Then
            If e.KeyCode = Keys.Down Then dgLot.Focus() : Exit Sub
        End If
        If DgAccountSearch.Visible = False And dgItemSearch.Visible = False And dgLot.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or Cbper.Focused = True Or cbCrateMarka.Focused = True Or cbAccountName.Focused = True Or txtCrateQty.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
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
        Select Case e.KeyCode
            Case Keys.PageUp
                e.Handled = True
                mskEntryDate.Focus()
        End Select
        If txtLot.Focused Then
            If e.KeyCode = Keys.Down Then
                dgLot.Focus()
            End If
        End If
        If txtVehicleNo.Focused Then
            If e.KeyCode = Keys.F3 Then
                If pnlSendingDetails.Visible = False Then
                    pnlSendingDetails.Visible = True
                    pnlSendingDetails.BringToFront()
                    txtCustMobile.Focus()
                Else
                    pnlSendingDetails.Visible = False
                    txtAccount.Focus()
                End If
            End If
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
            If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = Val(txtAccountID.Text)
            If e.KeyCode = Keys.F1 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                CreateAccount.FillContros(tmpID)
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.F3 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
            End If
            Dim tmpID As String = Val(txtItemID.Text)
            If e.KeyCode = Keys.F1 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                Item_form.FillContros(tmpID)
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
            End If
        End If
    End Sub
    Private Sub SpeedCalculation()
        If Cbper.SelectedIndex = 0 Then
            txtBasicAmount.Text = Format(Val(txtNug.Text) * Val(txtRate.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtBasicAmount.Text = Format(Val(txtKg.Text) * Val(txtRate.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 5 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 3 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 10 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 4 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 20 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 5 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 40 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 6 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 41 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 7 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 50 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 8 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 51 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 9 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 51.7 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 10 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 52.2 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 11 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 52.3 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 12 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 52.5 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 13 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 53 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 14 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 80 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 15 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) / 100 * Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 16 Then
            txtBasicAmount.Text = Format(Val(txtRate.Text) * Val(txtNug.Text), "0.00")
        End If
        txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
        txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
        txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
        txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
        txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
        lblTotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
        txtTotal.Text = Val(lblTotCharges.Text) + Val(txtBasicAmount.Text)
        Dim tmpCustAmount As Double = Val(txtTotal.Text)
        txtTotal.Text = Math.Round(Val(tmpCustAmount), 0)
        lblRoundOff.Text = Math.Round(Val(txtTotal.Text) - Val(tmpCustAmount), 2)
        txtTotal.Text = Format(Val(txtTotal.Text), "0.00")
    End Sub

    Private Sub txtOnValue_GotFocus(sender As Object, e As EventArgs) Handles txtOnValue.GotFocus
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        dgCharges.Visible = False : FillCharges()
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
        txtCharges.SelectAll()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Up Then
            If dg1.SelectedRows.Count > 0 AndAlso dg1.SelectedRows(0).Index = 0 Then
                ' If the first row is selected, focus on txtItem
                txtItem.Focus()
            End If
            ' Clear the selection in the DataGridView
            dg1.ClearSelection()
        End If

        If e.KeyCode = Keys.Down Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub

            Dim currentIndex As Integer = dg1.SelectedRows(0).Index
            Dim lastIndex As Integer = dg1.Rows.Count - 1

            If currentIndex = lastIndex Then
                ' If last row is selected, select the first row
                dg1.Rows(0).Selected = True
            Else
                ' Select the next row
                dg1.Rows(currentIndex + 1).Selected = True
            End If
        End If

        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
            txtLot.Text = dg1.SelectedRows(0).Cells(1).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
            txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
            txtBasicAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtTotal.Text = dg1.SelectedRows(0).Cells(7).Value
            '  cbitem.SelectedValue = dg1.SelectedRows(0).Cells(8).Value
            txtComPer.Text = dg1.SelectedRows(0).Cells(9).Value
            txtComAmt.Text = dg1.SelectedRows(0).Cells(10).Value
            txtMPer.Text = dg1.SelectedRows(0).Cells(11).Value
            txtMAmt.Text = dg1.SelectedRows(0).Cells(12).Value
            txtRdfPer.Text = dg1.SelectedRows(0).Cells(13).Value
            txtRdfAmt.Text = dg1.SelectedRows(0).Cells(14).Value
            txtTare.Text = dg1.SelectedRows(0).Cells(15).Value
            txtTareAmt.Text = dg1.SelectedRows(0).Cells(16).Value
            txtLabour.Text = dg1.SelectedRows(0).Cells(17).Value
            txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(18).Value
            lblCrate.Text = dg1.SelectedRows(0).Cells(19).Value
            cbCrateMarka.Text = dg1.SelectedRows(0).Cells(20).Value
            cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(21).Value)
            txtCrateQty.Text = dg1.SelectedRows(0).Cells(22).Value
            lblTotCharges.Text = dg1.SelectedRows(0).Cells(23).Value
            cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(24).Value)
            cbAccountName.Text = dg1.SelectedRows(0).Cells(25).Value
            txtPurchaseID.Text = Val(dg1.SelectedRows(0).Cells(26).Value)
            lblRoundOff.Text = Val(dg1.SelectedRows(0).Cells(27).Value)
            txtItem.Focus() : e.SuppressKeyPress = True
            '    dg1.ClearSelection()
        End If
        If e.KeyCode = Keys.Delete AndAlso dg1.SelectedRows.Count <> 0 Then
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If dg1.SelectedRows.Count = 0 Then Exit Sub
                clsFun.ExecNonQuery("Delete From Stock Where ID='" & Val(dg1.SelectedRows(0).Cells(28).Value) & "'")
                dg1.Rows.Remove(dg1.SelectedRows(0))

                calc()
                'ClearDetails()
            End If
        End If
        '  e.SuppressKeyPress = True
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
        txtLot.Text = dg1.SelectedRows(0).Cells(1).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
        txtKg.Text = dg1.SelectedRows(0).Cells(3).Value
        txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
        Cbper.Text = dg1.SelectedRows(0).Cells(5).Value
        txtBasicAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        txtTotal.Text = dg1.SelectedRows(0).Cells(7).Value
        txtItemID.Text = dg1.SelectedRows(0).Cells(8).Value
        txtComPer.Text = dg1.SelectedRows(0).Cells(9).Value
        txtComAmt.Text = dg1.SelectedRows(0).Cells(10).Value
        txtMPer.Text = dg1.SelectedRows(0).Cells(11).Value
        txtMAmt.Text = dg1.SelectedRows(0).Cells(12).Value
        txtRdfPer.Text = dg1.SelectedRows(0).Cells(13).Value
        txtRdfAmt.Text = dg1.SelectedRows(0).Cells(14).Value
        txtTare.Text = dg1.SelectedRows(0).Cells(15).Value
        txtTareAmt.Text = dg1.SelectedRows(0).Cells(16).Value
        txtLabour.Text = dg1.SelectedRows(0).Cells(17).Value
        txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(18).Value
        lblCrate.Text = dg1.SelectedRows(0).Cells(19).Value
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(20).Value
        cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(21).Value)
        txtCrateQty.Text = dg1.SelectedRows(0).Cells(22).Value
        lblTotCharges.Text = dg1.SelectedRows(0).Cells(23).Value
        cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(24).Value)
        cbAccountName.Text = dg1.SelectedRows(0).Cells(25).Value
        txtPurchaseID.Text = Val(dg1.SelectedRows(0).Cells(26).Value)
        ' txtItem.Focus()
        '  dg1.ClearSelection()
    End Sub
    Private Sub txtchargesAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtchargesAmount.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dg2.SelectedRows.Count = 1 Then
                Dg2.SelectedRows(0).Cells(0).Value = txtCharges.Text
                Dg2.SelectedRows(0).Cells(1).Value = IIf(Val(txtOnValue.Text) = 0, "", Val(txtOnValue.Text))
                Dg2.SelectedRows(0).Cells(2).Value = IIf(Val(txtCalculatePer.Text) = 0, "", Val(txtCalculatePer.Text))
                Dg2.SelectedRows(0).Cells(3).Value = txtPlusMinus.Text
                Dg2.SelectedRows(0).Cells(4).Value = txtchargesAmount.Text
                Dg2.SelectedRows(0).Cells(5).Value = txtChargeID.Text
                calc()
                txtCharges.Focus()
                Dg2.ClearSelection()
                cleartxtCharges()
            Else
                Dg2.Rows.Add(txtCharges.Text, IIf(Val(txtOnValue.Text) = 0, "", Val(txtOnValue.Text)), IIf(Val(txtCalculatePer.Text) = 0, "", Val(txtCalculatePer.Text)), txtPlusMinus.Text, txtchargesAmount.Text, txtChargeID.Text)
                calc()
                cleartxtCharges()
                txtCharges.Focus()
                Dg2.ClearSelection()
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
            If MessageBox.Show("Are you Sure to Remove Charge", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
                calc() : e.SuppressKeyPress = True
                Dg2.ClearSelection() : txtCharges.Focus()
                'ClearDetails()
            End If
        End If
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
        If dg1.Rows.Count = 0 Then MsgBox("There is No Items to Save/Update Record... Add Items First", MsgBoxStyle.Critical, "No Item") : Exit Sub
        mskEntryDate.Focus()
        ButtonControl()
        If BtnSave.Text = "&Save" Then
            save()
        Else
            UpdateRecord()
        End If
        ButtonControl()
        If clsFun.ExecScalarStr("Select AskMannual from Controls ") = "Yes" Then
            Dim res = MessageBox.Show("Do you want to Print...", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = Windows.Forms.DialogResult.Yes Then
                res = MessageBox.Show("Do you want to Bill or Bill of Supply...", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If res = Windows.Forms.DialogResult.Yes Then
                    btnPrint.Enabled = True
                    btnPrint.PerformClick()
                    Exit Sub
                Else
                    btnPrintBOS.Enabled = True
                    btnPrintBOS.PerformClick()
                    Exit Sub
                End If
            Else
                FootertextClear()
            End If
        Else
            FootertextClear()
        End If
        MainScreenPicture.lblStdAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Standard Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        MainScreenPicture.lblStdTotal.Text = Val(clsFun.ExecScalarStr("Select Count(*) from Vouchers Where TransType='Standard Sale' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        MainScreenPicture.retrive() : If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = True Then If Standard_Sale_Register.ckShowItems.Checked = False Then Standard_Sale_Register.retrive() Else Standard_Sale_Register.retrive1()
        If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        delete()
    End Sub
    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
    End Sub
    Private Sub cleartxt()
        txtNug.Text = "" : txtBasicAmount.Text = ""
        txtTotal.Text = "" : lblTotCharges.Text = ""
        txtItem.Focus()
    End Sub
    Private Sub FootertextClear()
        Try
            lblInword.Text = AmtInWord(txtTotalNetAmount.Text)
        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
        txtAccount.Clear() : txtAccountID.Clear()
        cleartxtCharges() : VNumber()
        txtTotalNug.Text = "" : txtbasicTotal.Text = ""
        txttotalWeight.Text = "" : txtTotalNetAmount.Text = ""
        txttotalCharges.Text = "" : txtotherCharges.Text = ""
        txtVehicleNo.Text = "" : txtTotGross.Text = "" : txtRate.Text = ""
        BtnSave.Text = "&Save" : BtnSave.BackColor = Color.DarkTurquoise
        BtnSave.Image = My.Resources.Save : BtnDelete.Enabled = False
        lbltotCom.Text = "0.00" : lblTotLabour.Text = "0.00"
        lblTotRdf.Text = "0.00" : lblTotBardana.Text = "0.00"
        lblTotMandi.Text = "0.00"
        txtLot.Text = "" : txtNug.Text = ""
        txtKg.Text = "" : txtBasicAmount.Text = ""
        txtTotal.Text = "" : lblTotCharges.Text = ""
        txtTotRoundOff.Text = "" : txtid.Clear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
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
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text) / 100, "0.00")
        ElseIf CalcType = "Nug" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Weight" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Aboslute" Then
            If String.IsNullOrEmpty(txtOnValue.Text) OrElse String.IsNullOrEmpty(txtCalculatePer.Text) Then Exit Sub
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text), "0.00")
        End If
    End Sub
    Private Sub Cbper_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbper.SelectedIndexChanged
        SpeedCalculation()
    End Sub
    Private Sub dgLot_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgLot.CellClick
        txtLot.Clear()
        txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
        txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
        lblRate.Text = "Purchase Rate : " & Val(dgLot.SelectedRows(0).Cells(6).Value)
        dgLot.Visible = False : LotBalance()
        txtNug.Focus()
    End Sub

    Private Sub dgLot_KeyDown(sender As Object, e As KeyEventArgs) Handles dgLot.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtLot.Clear()
            txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
            txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
            lblRate.Text = "Purchase Rate :" & Val(dgLot.SelectedRows(0).Cells(6).Value)
            dgLot.Visible = False : LotBalance()
            txtNug.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtLot.Focus()
    End Sub
    Private Sub txtBasicAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBasicAmount.KeyDown
        If e.KeyCode = Keys.Enter Then txtTotal.Focus()
    End Sub
    Public Sub RefreshAccounts()
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33) order by AccountName ", "AccountName", "ID", "--N./A.--")
    End Sub
    Private Sub cbAccountName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbAccountName.KeyDown
        If e.KeyCode = Keys.Enter Then cbCrateMarka.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            CreateAccount.TextBoxSender = "cbAccountName"
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.Opener = Me
            CreateAccount.OpenedFromItems = True
            CreateAccount.TextBoxSender = "cbAccountName"
            CreateAccount.FillContros(Val(cbAccountName.SelectedValue))
            ' clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
    End Sub

    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        If txtLot.TabStop = False Then
            If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
            If dgItemSearch.Rows.Count = 0 Then retriveItems()
            If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus() : Exit Sub
            txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            dgItemSearch.Visible = False : itemfill()
        End If
        '  If txtLot.Text = "" Then txtPurchaseID.Text = 0
        If dgLot.SelectedRows.Count = 1 Then
            If Val(dgLot.SelectedRows(0).Cells(0).Value) <> Val(txtPurchaseID.Text) Then txtPurchaseID.Text = ""
            txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
            txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
            If Val(txtPurchaseID.Text) <= 0 Then lblLot.Visible = False
            lblRate.Text = "Purchase Rate : " & Val(dgLot.SelectedRows(0).Cells(6).Value)
            LotBalance() : dgLot.Visible = False
        End If

        If lblCrate.Text = "Y" Then
            If txtAccountID.Text = 7 Then
                cbAccountName.Enabled = True : If dg1.SelectedRows.Count = 0 Then cbAccountName.SelectedIndex = 0
            Else
                cbAccountName.Enabled = False : cbAccountName.SelectedIndex = -1
            End If
            pnlMarka.Visible = True
            pnlMarka.BringToFront()
        End If
        txtNug.SelectAll()
    End Sub

    Private Sub txtNug_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNug.KeyDown
        If lblCrate.Text = "Y" Then
            If e.KeyCode = Keys.Enter Then
                If Val(txtAccountID.Text) = 7 Then
                    cbAccountName.Focus()
                Else
                    cbCrateMarka.Focus()
                End If
            End If

        Else
            If e.KeyCode = Keys.Enter Then txtKg.Focus()
        End If

    End Sub
    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtKg.KeyPress,
        txtRate.KeyPress, txtBasicAmount.KeyPress, txttotalCharges.KeyPress, txtOnValue.KeyPress, txtCalculatePer.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave
        txtNug.SelectionLength = 0
        If lblCrate.Text = "Y" Then
            txtCrateQty.Text = txtNug.Text
            ' cbCrateMarka.SelectedIndex = 0
        Else
            txtCrateQty.Text = Val(0)
            cbCrateMarka.SelectedValue = Val(0)
            cbCrateMarka.Text = ""
        End If
        If Val(txtPurchaseID.Text) > 0 Then

            If trackStock <> "Nug" Then Exit Sub
            If Val(txtNug.Text) > Val(LotBal) Then
                MsgBox("Not Enough Nugs. Please Choose Another Item / Lot ", MsgBoxStyle.Critical, "Zero")
                txtNug.Text = 0 : txtNug.Focus() : Exit Sub
            End If
        End If

    End Sub

    Private Sub txtBasicAmount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtBasicAmount.KeyUp
        If Cbper.SelectedIndex = 0 Then
            txtRate.Text = Val(txtBasicAmount.Text) / Format(Val(txtNug.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtRate.Text = Val(txtBasicAmount.Text) / Format(Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 5 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 3 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 10 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 4 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 20 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 5 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 40 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 6 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 41 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 7 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 50 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 8 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 51 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 9 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 51.7 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 10 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 52.3 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 11 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 53 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 12 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 80 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 13 Then
            txtRate.Text = Format(Val(txtBasicAmount.Text) * 100 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 14 Then
            txtRate.Text = Format(Val(txtNug.Text) * Val(txtBasicAmount.Text), "0.00")
        End If
        'txtSallerAmout.Text = txtBasicAmount.Text
        'txtRate.Text = Format(Math.Round(Val(txtRate.Text), 2), "0.00")
        'txtSallerRate.Text = txtRate.Text
        If txtRate.Text = "NAN" Then txtRate.Text = "" : Exit Sub
        txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
        txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
        If Val(txtRdfPer.Text) > 0 Then
            txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtBasicAmount.Text) / 100, "0.00")
        Else
            txtRdfAmt.Text = Format(Val(txtRdfAmt.Text), "0.00")
        End If
        If clsFun.ExecScalarStr("Select ApplyCommWeight From Controls") = "Yes" Then
            txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtKg.Text), "0.00")
        Else
            Dim crateRate As String = clsFun.ExecScalarStr("Select CrateBardana From Controls")
            If crateRate = "Yes" And lblCrate.Text = "Y" Then txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtCrateQty.Text), "0.00") Else txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
        End If
        txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
        lblTotCharges.Text = Format(Val(Val(txtComAmt.Text)) + Val(Val(txtMAmt.Text)) + Val(Val(txtRdfAmt.Text)) + Val(Val(txtTareAmt.Text)) + Val(Val(txtLaboutAmt.Text)), "0.00")
        txtTotal.Text = Format(Val(Val(lblTotCharges.Text)) + Val(Val(txtBasicAmount.Text)), 0)
    End Sub


    Private Sub txtNug_KeyUp(sender As Object, e As EventArgs) Handles txtNug.Leave, Cbper.Leave,
        txtKg.Leave, txtRate.Leave, txtBasicAmount.Leave, txtchargesAmount.Leave,
        txtTotalNetAmount.Leave, txtbasicTotal.Leave, txtCalculatePer.Leave, txtPlusMinus.Leave,
       txtOnValue.Leave, txttotalCharges.Leave, txtComPer.Leave, txtMAmt.Leave, txtRdfPer.Leave, txtTare.Leave, txtLabour.Leave
        ChargesCalculation() : SpeedCalculation()
        If txtKg.Focused Then
            If Val(txtPurchaseID.Text) > 0 Then
                If trackStock <> "Weight" Then Exit Sub
                If Val(txtKg.Text) > Val(LotBal) Then
                    MsgBox("Not Enough Weight. Please Choose Another Item / Lot ", MsgBoxStyle.Critical, "Zero")
                    txtKg.Text = 0 : txtKg.Focus() : Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub txtTotal_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotal.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Cbper.SelectedIndex = 0 Then
                If Val(txtNug.Text) = 0 Then MsgBox("please fill Nug...", MsgBoxStyle.Critical, "Access Denied") : txtNug.Focus() : Exit Sub
            ElseIf Cbper.SelectedIndex = 14 Then
                If lblCrate.Text = "Y" Then
                    pnlMarka.Visible = True : pnlMarka.BringToFront()
                    If Val(txtCrateQty.Text) = 0 Then MsgBox("please fill Crate Qty...", MsgBoxStyle.Critical, "Access Denied") : txtCrateQty.Focus() : Exit Sub
                End If
            Else
                If Val(txtKg.Text) = 0 Then MsgBox("please fill Weight...", MsgBoxStyle.Critical, "Access Denied") : txtKg.Focus() : Exit Sub
            End If
            If Val(txtPurchaseID.Text) <> 0 Then
                Dim SellerID As Integer = Val(clsFun.ExecScalarInt("Select StockHolderID From Purchase Where VoucherID='" & Val(txtPurchaseID.Text) & "'"))
                Dim StorageID As Integer = Val(clsFun.ExecScalarInt("Select StorageID From Purchase Where VoucherID='" & Val(txtPurchaseID.Text) & "'"))
                '  If Val(ReserveBal) < Val(txtNug.Text) Then MsgBox("Lot Reserved by Other User ", MsgBoxStyle.Critical, "Access Denied") : LotBalance() : Exit Sub
                If trackStock = "Nug" Then
                    If Val(LotBal) < Val(txtNug.Text) Then
                        MsgBox("Not Enough Nug", MsgBoxStyle.Critical, "Access Denied") : txtLot.Focus() : Exit Sub
                    End If
                Else
                    If Val(LotBal) < Val(txtKg.Text) Then
                        MsgBox("Not Enough Weight", MsgBoxStyle.Critical, "Access Denied") : txtLot.Focus() : Exit Sub
                    End If
                End If
                BalanceRecord()
            End If
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(0).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(1).Value = txtLot.Text
                dg1.SelectedRows(0).Cells(2).Value = Format(Val(txtNug.Text), "0.00")
                dg1.SelectedRows(0).Cells(3).Value = Format(Val(txtKg.Text), "0.00")
                dg1.SelectedRows(0).Cells(4).Value = Format(Val(txtRate.Text), "0.00")
                dg1.SelectedRows(0).Cells(5).Value = Cbper.Text
                dg1.SelectedRows(0).Cells(6).Value = Format(Val(txtBasicAmount.Text), "0.00")
                dg1.SelectedRows(0).Cells(7).Value = Format(Val(txtTotal.Text), "0.00")
                dg1.SelectedRows(0).Cells(8).Value = Val(txtItemID.Text)
                dg1.SelectedRows(0).Cells(9).Value = Format(Val(txtComPer.Text), "0.00")
                dg1.SelectedRows(0).Cells(10).Value = Format(Val(txtComAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(11).Value = Format(Val(txtMPer.Text), "0.00")
                dg1.SelectedRows(0).Cells(12).Value = Format(Val(txtMAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(13).Value = Format(Val(txtRdfPer.Text), "0.00")
                dg1.SelectedRows(0).Cells(14).Value = Format(Val(txtRdfAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(15).Value = Format(Val(txtTare.Text), "0.00")
                dg1.SelectedRows(0).Cells(16).Value = Format(Val(txtTareAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(17).Value = Format(Val(txtLabour.Text), "0.00")
                dg1.SelectedRows(0).Cells(18).Value = Format(Val(txtLaboutAmt.Text), "0.00")
                dg1.SelectedRows(0).Cells(19).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(20).Value = cbCrateMarka.Text
                dg1.SelectedRows(0).Cells(21).Value = Val(cbCrateMarka.SelectedValue)
                dg1.SelectedRows(0).Cells(22).Value = Val(txtCrateQty.Text)
                dg1.SelectedRows(0).Cells(23).Value = Format(Val(lblTotCharges.Text), "0.00")
                dg1.SelectedRows(0).Cells(24).Value = Val(cbAccountName.SelectedValue)
                dg1.SelectedRows(0).Cells(25).Value = cbAccountName.Text
                dg1.SelectedRows(0).Cells(26).Value = Val(txtPurchaseID.Text)
                dg1.SelectedRows(0).Cells(27).Value = Val(lblRoundOff.Text)
                dg1.SelectedRows(0).Cells(28).Value = Val(dg1.SelectedRows(0).Cells(28).Value)
                calc() : cleartxt() : dg1.ClearSelection()
            Else
                If lblCrate.Text = "Y" Then
                    dg1.Rows.Add(txtItem.Text, txtLot.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtKg.Text), "0.00"), Format(Val(txtRate.Text), "0.00"), Cbper.Text,
                          Format(Val(txtBasicAmount.Text), "0.00"), Format(Val(txtTotal.Text), "0.00"), Val(txtItemID.Text), Format(Val(txtComPer.Text), "0.00"),
                           Format(Val(txtComAmt.Text), "0.00"), Format(Val(txtMPer.Text), "0.00"), Format(Val(txtMAmt.Text), "0.00"), Format(Val(txtRdfPer.Text), "0.00"), Format(Val(txtRdfAmt.Text), "0.00"),
                           Format(Val(txtTare.Text), "0.00"), Format(Val(txtTareAmt.Text), "0.00"), Format(Val(txtLabour.Text), "0.00"), Format(Val(txtLaboutAmt.Text), "0.00"), lblCrate.Text,
                           cbCrateMarka.Text, Val(cbCrateMarka.SelectedValue), Val(txtCrateQty.Text), Format(Val(lblTotCharges.Text), "0.00"), Val(cbAccountName.SelectedValue), cbAccountName.Text, Val(txtPurchaseID.Text), Val(lblRoundOff.Text), (dg1.Rows.Count) + 1)
                Else
                    dg1.Rows.Add(txtItem.Text, txtLot.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtKg.Text), "0.00"), Format(Val(txtRate.Text), "0.00"), Cbper.Text,
                          Format(Val(txtBasicAmount.Text), "0.00"), Format(Val(txtTotal.Text), "0.00"), Val(txtItemID.Text), Format(Val(txtComPer.Text), "0.00"),
                           Format(Val(txtComAmt.Text), "0.00"), Format(Val(txtMPer.Text), "0.00"), Format(Val(txtMAmt.Text), "0.00"), Format(Val(txtRdfPer.Text), "0.00"), Format(Val(txtRdfAmt.Text), "0.00"),
                           Format(Val(txtTare.Text), "0.00"), Format(Val(txtTareAmt.Text), "0.00"), Format(Val(txtLabour.Text), "0.00"), Format(Val(txtLaboutAmt.Text), "0.00"), lblCrate.Text,
                           cbCrateMarka.Text, Val(cbCrateMarka.SelectedValue), Val(txtCrateQty.Text), Format(Val(lblTotCharges.Text), "0.00"), Val(0), "", Val(txtPurchaseID.Text), Val(lblRoundOff.Text), (dg1.Rows.Count) + 1)
                End If
                calc() : cleartxt() : dg1.ClearSelection()
            End If
        End If

    End Sub

    Private Sub txtCrateQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCrateQty.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlMarka.Visible = False
            txtKg.Focus()
            pnlMarka.SendToBack()
        End If
    End Sub
    Private Sub txtCrateQty_Leave(sender As Object, e As EventArgs) Handles txtCrateQty.Leave
        pnlMarka.Visible = False
        pnlMarka.SendToBack()
    End Sub
    Private Sub cbCrateMarka_KeyDown(sender As Object, e As KeyEventArgs) Handles cbCrateMarka.KeyDown
        If e.KeyCode = Keys.F3 Then
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            If Not CrateForm Is Nothing Then
                CrateForm.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then txtCrateQty.Focus()
    End Sub

    Private Sub cbCrateMarka_Leave(sender As Object, e As EventArgs) Handles cbCrateMarka.Leave
        If clsFun.ExecScalarInt("Select count(*)from CrateMarka") = 0 Then
            Exit Sub
        End If
        If clsFun.ExecScalarInt("Select count(*)from CrateMarka where MarkaName='" & cbCrateMarka.Text & "'") = 0 Then
            MsgBox("Crate Not Found in Database...", vbOKOnly, "Access Denied")
            cbCrateMarka.Focus()
            Exit Sub
        End If
        Dim crateRate As String = clsFun.ExecScalarStr("Select CrateBardana From Controls")
        If crateRate = "Yes" Then txtTare.Text = clsFun.ExecScalarStr("Select Rate From CrateMarka Where ID='" & Val(cbCrateMarka.SelectedValue) & "'")
    End Sub
    Private Sub PrintRecord()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In tmpgrid.Rows
            With row
                If .Cells(6).Value <> "" Then
                    Dim amtInWords As String = String.Empty
                    Try
                        amtInWords = AmtInWord(.Cells(21).Value)
                    Catch ex As Exception
                        amtInWords = ex.ToString
                    End Try
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," & _
                                "'" & amtInWords & "','" & .Cells(36).Value & "','" & .Cells(37).Value & "','" & .Cells(38).Value & "'," & _
                                "'" & .Cells(39).Value & "','" & .Cells(40).Value & "','" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "'," & _
                                "'" & .Cells(44).Value & "','" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "'," & _
                                "'" & .Cells(49).Value & "','" & .Cells(50).Value & "','" & .Cells(51).Value & "','" & .Cells(52).Value & "','" & .Cells(53).Value & "'," & _
                                "'" & .Cells(54).Value & "','" & .Cells(55).Value & "','" & .Cells(56).Value & "','" & .Cells(57).Value & "','" & .Cells(58).Value & "'," & _
                                "'" & .Cells(59).Value & "','" & .Cells(60).Value & "','" & .Cells(61).Value & "','" & .Cells(62).Value & "','" & .Cells(63).Value & "'," & _
                                "'" & .Cells(64).Value & "','" & .Cells(65).Value & "','" & .Cells(66).Value & "','" & .Cells(67).Value & "','" & .Cells(68).Value & "', " & _
                                "'" & .Cells(69).Value & "','" & .Cells(70).Value & "','" & .Cells(71).Value & "','" & .Cells(72).Value & "','" & .Cells(73).Value & "', " & _
                                "'" & .Cells(74).Value & "','" & .Cells(75).Value & "'," & _
                                "'" & .Cells(27).Value & "','" & .Cells(29).Value & "','" & .Cells(31).Value & "','" & .Cells(33).Value & "','" & .Cells(35).Value & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " & _
                         " P21,P22,P23,P24,P25,P26,P27,P28,P29,P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42,P43,P44,P45, " & _
                         "P46,P47,P48,P49,P50,P51,P52,P53,P54,P55,P56,P57,P58,P59,P60,P61,P62,P63,P64,P65,P66,P67)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
        ' clsFun.ExecNonQuery(sql)
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        pnlWhatsapp.Visible = True : pnlWhatsapp.BringToFront() : txtWhatsappNo.Focus() : RadioPrint1.Checked = True
        txtWhatsappNo.Text = clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
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
    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
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
    Private Sub FillCharges()
        ApplyON = clsFun.ExecScalarStr(" Select ApplyON FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        If CalcType = "Aboslute" Then
            txtOnValue.Clear()
            txtCalculatePer.Clear()
            txtOnValue.TabStop = False
            txtCalculatePer.TabStop = False
            txtchargesAmount.Focus()
        ElseIf CalcType = "Weight" Then
            txtOnValue.Text = txttotalWeight.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Percentage" Then
            If ApplyON = "Total Amount" Then
                txtOnValue.Text = Format(Val(txtTotal.Text), "0.00")
            ElseIf ApplyON = "Item Total" Then
                txtOnValue.Text = Format(Val(txtTotGross.Text), "0.00")
            Else
                txtOnValue.Text = Format(Val(txtbasicTotal.Text), "0.00")
            End If
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Nug" Then
            txtOnValue.Text = txtTotalNug.Text
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
            dgCharges.Visible = False : txtOnValue.Focus()
            FillCharges() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtCharges.Focus()
    End Sub

    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs) Handles txtCharges.GotFocus
        If DgAccountSearch.Visible = True Then DgAccountSearch.Visible = False
        If dgItemSearch.Visible = True Then dgItemSearch.Visible = False
        ' If dgCharges.Visible = True Then dgCharges.Visible = False
    End Sub
    Private Sub txtCharges_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyUp
        ChargesRowColums()
        If txtCharges.Text.Trim() <> "" Then
            'dgCharges.Visible = True
            RetriveCharges(" Where upper(ChargeName) Like upper('%" & txtCharges.Text.Trim() & "%')")
            ' FillCharges()
        End If
        If e.KeyCode = Keys.Escape Then dgCharges.Visible = False
    End Sub

    Private Sub txtCharges_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCharges.KeyPress
        ChargesRowColums()
        dgCharges.Visible = True
    End Sub


    Private Sub CbAccountName_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub txtItem_TextChanged(sender As Object, e As EventArgs) Handles txtItem.TextChanged

    End Sub

    Private Sub lblAcBal_Click(sender As Object, e As EventArgs) Handles lblAcBal.Click

    End Sub

    Private Sub txtCharges_TextChanged(sender As Object, e As EventArgs) Handles txtCharges.TextChanged

    End Sub

    'Private Sub txtCharges_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyDown
    '    If e.KeyCode = Keys.Down Then dgCharges.Focus()
    'End Sub
    Private Sub CbCharges_KeyDown(sender As Object, e As KeyEventArgs) Handles txtOnValue.KeyDown, txtCalculatePer.KeyDown, txtPlusMinus.KeyDown, txtCharges.KeyDown
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
            Case Keys.PageUp
                e.Handled = True
                dgCharges.Visible = False
                txtItem.Focus()
        End Select
        If txtCharges.Focused Then
            If e.KeyCode = Keys.F3 Then
                ChargesForm.MdiParent = MainScreenForm
                ChargesForm.Show()
                ChargesForm.BringToFront()
            End If
            If e.KeyCode = Keys.F1 Then
                If dgCharges.SelectedRows.Count = 0 Then Exit Sub
                ChargesForm.MdiParent = MainScreenForm
                Dim tmpMarkaID As String = dgCharges.SelectedRows(0).Cells(0).Value
                ChargesForm.Show()
                ChargesForm.FillContros(tmpMarkaID)
                ChargesForm.BringToFront()
            End If
            If e.KeyCode = Keys.Down Then dgCharges.Focus()
        End If
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


    Private Sub txtVehicleNo_TextChanged(sender As Object, e As EventArgs) Handles txtVehicleNo.TextChanged

    End Sub

    Private Sub txtLotNo_TextChanged(sender As Object, e As EventArgs) Handles txtLot.TextChanged

    End Sub


    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        FillWithNevigation()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If Offset = 0 Then
            Offset = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Standard Sale'  Order By ID ")
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
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Standard Sale'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        Offset = (TotalPages - 1) * RowCount
        FillWithNevigation()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnPrintBOS.Click
        pnlWhatsapp.Visible = True : pnlWhatsapp.BringToFront() : txtWhatsappNo.Focus() : radioBOS.Checked = True
        If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Whatsapp API" Then
            cbType.SelectedIndex = 0 : Exit Sub
        Else
            cbType.SelectedIndex = 1
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
        End If
    End Sub

    Private Sub txtMobile_TextChanged(sender As Object, e As EventArgs) Handles txtDriverMobile.TextChanged

    End Sub

    Private Sub txtRemark_Leave(sender As Object, e As EventArgs) Handles txtRemark.Leave
        pnlSendingDetails.Visible = False : txtAccount.Focus()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtBrokerName.TextChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TempRowColumn() : retrive2()
        ClsFunPrimary.CloseConnection()
        '    ClsFunPrimary.changeCompany()
        PrintRecord() : FootertextClear()
        If radioBOS.Checked = True Then
            Report_Viewer.printReport("\BillofSupply.rpt")
        ElseIf RadioPrint1.Checked = True Then
            Report_Viewer.printReport("\StandardSale.rpt")
        ElseIf RadioPrint2.Checked = True Then
            Report_Viewer.printReport("\StandardSale2.rpt")
        End If
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        TempRowColumn() : retrive2()
        ClsFunPrimary.CloseConnection()
        '    ClsFunPrimary.changeCompany()
        PrintRecord() : FootertextClear()
        If radioBOS.Checked = True Then
            Report_Viewer.printReport("\BillofSupply.rpt")
        ElseIf RadioPrint1.Checked = True Then
            Report_Viewer.printReport("\StandardSale.rpt")
        ElseIf RadioPrint2.Checked = True Then
            Report_Viewer.printReport("\StandardSale2.rpt")
        End If
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If

    End Sub
    Private Sub SendWhatsAppFile(ByVal phoneNumber As String, ByVal message As String, ByVal pdfFilePath As String)
        'UplaodFile()
        pdfFilePath = FilePath
        ' Dim base64Pdf As String = ConvertPdfToBase64(pdfFilePath)
        ' Dim url As String = "https://aadhat.cloud/send?phone=" & phoneNumber & "&text=" & Uri.EscapeDataString(message) & "&file=" & Uri.EscapeDataString(base64Pdf)
        Dim url As String = "http://aadhat.cloud/api/send?number=" & phoneNumber & "&type=text&message=" & Uri.EscapeDataString(message) & "&media_url=" & Uri.EscapeDataString(pdfFilePath) & "&instance_id=" & instance_id & "&access_token=" & access_token & ""
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        request.Method = "GET"

        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim responseStream As Stream = response.GetResponseStream()
        Dim reader As New StreamReader(responseStream, Encoding.GetEncoding("utf-8"))
        Dim responseString As String = reader.ReadToEnd()
        Dim responseJson As JObject = JObject.Parse(responseString)
        Dim status As String = responseJson("status").ToString()
        Dim msg As String = String.Empty
        If responseJson("message") IsNot Nothing Then
            msg = responseJson("message").ToString()
        End If
        '   Dim msg As String = responseJson("message").ToString()
        '   Dim hostedFilePath As String = UploadFileAndGetPath(Filepath)
        ' Check the status value and handle accordingly
        If status = "success" Or msg IsNot Nothing Then
            APIResposne = "Successful"
        Else
            APIResposne = "Unsuccessful"
        End If
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
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        retrive2() : PrintRecord()
        If radioBOS.Checked = True Then
            Pdf_Genrate.ExportReport("\BillofSupply.rpt")
        Else
            Pdf_Genrate.ExportReport("\StandardSale.rpt")
        End If
        Pdf_Genrate.ExportReport("\Formats\SuperSaleBeejak.rpt")
        WhatsAppSender.FilePath = WhatsAppSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
        SendWhatsAppFile("91" & txtWhatsappNo.Text, "Sended By: Aadhat Software" & vbCrLf & "www.softmanagementindia.in", FilePath)
        lblStatus.Text = "PDF Sent " & APIResposne
        lblStatus.Visible = True
        sql = "insert into waReport(EntryDate,AccountName,WhatsAppNo,Type,Status) SELECT '" & Date.Today.ToString("yyyy-MM-dd") & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','Standard Sale','" & lblStatus.Text & "'"
        clsFun.ExecNonQuery(sql)
    End Sub

    Private Sub WahSoft()
        If txtWhatsappNo.Text <= "" Then lblStatus.Visible = False : Exit Sub
        lblStatus.Visible = False
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.pdf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        WABA.ExecNonQuery("Delete from SendingData")
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        retrive2() : PrintRecord()
        If radioBOS.Checked = True Then
            Pdf_Genrate.ExportReport("\BillofSupply.rpt")
        Else
            Pdf_Genrate.ExportReport("\StandardSale.rpt")
        End If
        ' Pdf_Genrate.ExportReport("\Formats\SuperSaleBeejak.rpt")
        WhatsAppSender.FilePath = WhatsAppSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
               "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & whatsappSender.FilePath & "')"
        If WABA.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            WABA.ExecScalarStr(sql)
        End If
        Dim WhatsappFile As String = Application.StartupPath & "\WahSoft\WahSoft.exe"
        If System.IO.File.Exists(WhatsappFile) = False Then
            MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
            Exit Sub
        End If
        Dim p() As Process
        p = Process.GetProcessesByName("WahSoft")
        If p.Count = 0 Then
            Dim StartWhatsapp As New System.Diagnostics.Process
            StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\WahSoft\WahSoft.exe"
            StartWhatsapp.Start()
        End If
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
                MsgBox("Please Enter Valid Whatsapp Contact", MsgBoxStyle.Critical, "Invalid Contact") : txtWhatsappNo.Focus()
            End If
        Else
            StartBackgroundTask(AddressOf WahSoft)
        End If
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
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        retrive2() : PrintRecord()
        'If radioBOS.Checked = True Then
        '    Pdf_Genrate.ExportReport("\BillofSupply.rpt")
        'Else
        '    Pdf_Genrate.ExportReport("\Formats\StandardSale.rpt")
        'End If
        If radioBOS.Checked = True Then
            Pdf_Genrate.ExportReport("\BillofSupply.rpt")
        ElseIf RadioPrint1.Checked = True Then
            Pdf_Genrate.ExportReport("\Formats\StandardSale.rpt")
        ElseIf RadioPrint2.Checked = True Then
            Pdf_Genrate.ExportReport("\Formats\StandardSale2.rpt")
        End If
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        FootertextClear()
    End Sub




    Private Sub Dg2_MouseClick(sender As Object, e As MouseEventArgs) Handles Dg2.MouseClick
        Dg2.ClearSelection()
    End Sub

    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub
    Private Sub ExpSettings()
        If BtnSave.Text <> "&Save" Then Exit Sub
        Dg2.Rows.Clear() : Dim dt As New DataTable
        Dim sql As String = "Select * From ExpControl Where ApplyOn='Standard Sale'"
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

    Private Sub txtAccountID_TextChanged(sender As Object, e As EventArgs) Handles txtAccountID.TextChanged
        If (txtLot.TabStop = False Or Val(txtAccountID.Text) = 0) AndAlso BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarInt("Select Count(*) From Vouchers Where EntryDate ='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID=" & Val(txtAccountID.Text) & "  and TransType='" & Me.Text & "'") Then
                If MessageBox.Show("Do you want Entry On Last Today's Exist Bill?", "Bill Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    txtid.Text = clsFun.ExecScalarInt("Select ID From Vouchers Where EntryDate ='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID=" & Val(txtAccountID.Text) & " and TransType= '" & Me.Text & "'")
                    FillControls(txtid.Text) : txtItem.Focus() : Exit Sub
                End If
            End If
        End If
    End Sub
End Class