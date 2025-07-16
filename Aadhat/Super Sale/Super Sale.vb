Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text

Public Class Super_Sale

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        clsFun.DoubleBuffered(Dg2, True)
    End Sub

    Dim vno As Integer : Dim VchId As Integer
    Dim VchId2 As Integer : Dim sql As String = String.Empty
    Dim count As Integer = 0 : Dim MaxID As String = String.Empty
    Dim CalcType As String = String.Empty
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 1 : Dim Offset As Integer = 0
    Dim el As New Aadhat.ErrorLogger
    Dim opbal As String = "" : Dim ClBal As String = ""
    Dim ServerTag As Integer : Dim ItemPer As String
    Dim whatsappSender As New WhatsAppSender()

    Private Sub Super_Sale_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
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
    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub

    Private Sub mskEntryDate_Leave(sender As Object, e As EventArgs) Handles mskEntryDate.Leave
        ExpSettings()
    End Sub
    Private Sub ExpSettings()
        If BtnSave.Text <> "&Save" Then Exit Sub
        Dg2.Rows.Clear()
        Dim dt As New DataTable
        Dim sql As String = "Select * From ExpControl Where ApplyOn='Super Sale'"
        dt = clsFun.ExecDataTable(sql)
        Try
            For i = 0 To dt.Rows.Count - 1
                Dg2.Rows.Add()
                With Dg2.Rows(i)
                    Dim CalcType As String = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & dt.Rows(i)("ChargesID").ToString() & "'")
                    Dim PlusMinus As String = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & dt.Rows(i)("ChargesID").ToString() & "'")
                    .Cells(6).Value = Val(dt.Rows(i)("ChargesID").ToString())
                    .Cells(1).Value = dt.Rows(i)("ChargesName").ToString()
                    .Cells(4).Value = PlusMinus
                    If CalcType = "Aboslute" Then
                        .Cells(5).Value = dt.Rows(i)("FixAs").ToString()
                    ElseIf CalcType = "Percentage" Then
                        .Cells(3).Value = dt.Rows(i)("FixAs").ToString()
                    ElseIf CalcType = "Nug" Then
                        .Cells(3).Value = dt.Rows(i)("FixAs").ToString()
                    ElseIf CalcType = "Weight" Then
                        .Cells(3).Value = dt.Rows(i)("FixAs").ToString()
                    End If
                End With
            Next
        Catch ex As Exception

        End Try
        Dg2.ClearSelection()
    End Sub
    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub

    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
        Dim BackDateEntry As String = clsFun.ExecScalarStr("SELECT DontAllowBack FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Other'")
        If BackDateEntry <> "N" Then
            If mskEntryDate.Text < Date.Today.ToString("dd-MM-yyyy") Then MsgBox("Can't Create Bill Back Date...", MsgBoxStyle.Critical, "Denied Back Date Entries...") : mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy") : Exit Sub
        End If
    End Sub

    Private Sub Super_Sale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0 : Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        VNumber() : rowColums()
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Cbper.SelectedIndex = 0 : pnlMarka.Visible = False
        Me.KeyPreview = True : BtnDelete.Enabled = False
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
        FillSuperSale()
    End Sub

    Public Sub FillSuperSale()
        Dim ssql As String = "Select * from Controls "
        Dim dt As DataTable
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("SuperCommission").ToString().Trim() = "Percentage" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = False
            ElseIf dt.Rows(0)("SuperCommission").ToString().Trim() = "Amount" Then
                txtComAmt.TabStop = True : txtComPer.TabStop = False
            ElseIf dt.Rows(0)("SuperCommission").ToString().Trim() = "Both" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = True
            Else
                txtComPer.TabStop = False : txtComAmt.TabStop = False
            End If
            If dt.Rows(0)("SuperMandiTax").ToString().Trim() = "Percentage" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = False
            ElseIf dt.Rows(0)("SuperMandiTax").ToString().Trim() = "Amount" Then
                txtMAmt.TabStop = True : txtMPer.TabStop = False
            ElseIf dt.Rows(0)("SuperMandiTax").ToString().Trim() = "Both" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = True
            Else
                txtMPer.TabStop = False : txtMAmt.TabStop = False
            End If
            If dt.Rows(0)("SuperRDF").ToString().Trim() = "Percentage" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = False
            ElseIf dt.Rows(0)("SuperRDF").ToString().Trim() = "Amount" Then
                txtRdfAmt.TabStop = True : txtRdfPer.TabStop = False
            ElseIf dt.Rows(0)("SuperRDF").ToString().Trim() = "Both" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = True
            Else
                txtRdfPer.TabStop = False : txtRdfAmt.TabStop = False
            End If
            If dt.Rows(0)("SuperTare").ToString().Trim() = "Nug" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = False
            ElseIf dt.Rows(0)("SuperTare").ToString().Trim() = "Amount" Then
                txtTareAmt.TabStop = True : txtTare.TabStop = False
            ElseIf dt.Rows(0)("SuperTare").ToString().Trim() = "Both" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = True
            Else
                txtTare.TabStop = False : txtTareAmt.TabStop = False
            End If
            If dt.Rows(0)("SuperLabour").ToString().Trim() = "Nug" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = False
            ElseIf dt.Rows(0)("SuperLabour").ToString().Trim() = "Amount" Then
                txtLaboutAmt.TabStop = True : txtLabour.TabStop = False
            ElseIf dt.Rows(0)("SuperLabour").ToString().Trim() = "Both" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = True
            Else
                txtLabour.TabStop = False : txtLaboutAmt.TabStop = False
            End If
            If dt.Rows(0)("SuperTaxPaid").ToString().Trim() = "Yes" Then ckTaxPaid.Visible = True Else ckTaxPaid.Visible = False
            If dt.Rows(0)("SuperKaat").ToString().Trim() = "Yes" Then txtCut.TabStop = True Else txtCut.TabStop = False
            If dt.Rows(0)("SuperVehicleNo").ToString().Trim() = "Yes" Then txtVehicleNo.TabStop = True Else txtVehicleNo.TabStop = False
            If dt.Rows(0)("SuperBasic").ToString().Trim() = "Yes" Then txtBasicCustomer.TabStop = True : txtBasicCustomer.ReadOnly = False Else txtBasicCustomer.TabStop = False : txtBasicCustomer.ReadOnly = True
            'Cbper.Text = dt.Rows(0)("Per").ToString().Trim()
            ItemPer = dt.Rows(0)("Per").ToString().Trim()
            If ItemPer <> "itemWise" Then Cbper.Text = dt.Rows(0)("Per").ToString().Trim()
        End If
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 37
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
            .Columns(10).Name = "SRate" : .Columns(10).Width = 90
            .Columns(11).Name = "per" : .Columns(11).Width = 50
            .Columns(12).Name = "SallerAmount" : .Columns(12).Width = 95
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
            .Columns(26).Name = "Addweight" : .Columns(26).Width = 159
            .Columns(27).Name = "Driver Name" : .Columns(27).Width = 159
            .Columns(28).Name = "Mobile" : .Columns(28).Width = 159
            .Columns(29).Name = "Remark" : .Columns(29).Width = 159
            .Columns(30).Name = "GrNo" : .Columns(30).Width = 159
            .Columns(31).Name = "GSTN" : .Columns(31).Width = 159
            .Columns(32).Name = "Cust Mobile" : .Columns(32).Width = 159
            .Columns(33).Name = "Broker Name" : .Columns(33).Width = 159
            .Columns(34).Name = "Broker Mobile" : .Columns(34).Width = 159
            .Columns(35).Name = "TransPort" : .Columns(35).Width = 159
            .Columns(36).Name = "GRNo" : .Columns(36).Width = 159
        End With
    End Sub
    Private Sub AcBal()
        ' Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtAccountID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
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
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Cr"
            Else
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Dr"
            End If
        End If
        lblAcBal.Visible = True
        lblAcBal.Text = "Sup. Bal : " & opbal
    End Sub
    Private Sub ClosingBal()
        ClBal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtAccountID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(ClBal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(ClBal) + Val(tmpamtcr)
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(ClBal)
        If drcr = "Cr" Then
            ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
        Else
            ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
            Else
                ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
            End If
        End If
    End Sub
    Private Sub AccountComm()
        If BtnSave.Text = "&Save" Then
            Dim acccomm As Decimal = 0.0 : Dim Mper As Decimal = 0.0
            Dim RdfPer As Decimal = 0.0 : Dim TarePer As Decimal = 0.0
            Dim LabourPer As Decimal = 0.0
            acccomm = Val(clsFun.ExecScalarStr("Select CommPer From Accounts Where ID= '" & Val(txtcustomerID.Text) & "'"))
            Mper = Val(clsFun.ExecScalarStr("Select Mper From Accounts Where ID= '" & Val(txtcustomerID.Text) & "'"))
            RdfPer = Val(clsFun.ExecScalarStr("Select RdfPer From Accounts Where ID= '" & Val(txtcustomerID.Text) & "'"))
            TarePer = Val(clsFun.ExecScalarStr("Select TarePer From Accounts Where ID= '" & Val(txtcustomerID.Text) & "'"))
            LabourPer = Val(clsFun.ExecScalarStr("Select LabourPer From Accounts Where ID= '" & Val(txtcustomerID.Text) & "'"))
            If acccomm > 0 And Val(txtComPer.Text) <> 0 Then txtComPer.Text = acccomm
            If Mper > 0 And Val(txtMPer.Text) <> 0 Then txtMPer.Text = Mper
            If RdfPer > 0 And Val(txtRdfPer.Text) <> 0 Then txtRdfPer.Text = RdfPer
            If TarePer > 0 And Val(txtTare.Text) <> 0 Then txtTare.Text = TarePer
            If LabourPer > 0 And Val(txtLabour.Text) <> 0 Then txtLabour.Text = LabourPer
        End If
    End Sub
    Private Sub AcCustBal()
        ' Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtcustomerID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtcustomerID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtcustomerID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtcustomerID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Cr"
        Else
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtcustomerID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtcustomerID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Cr"
            Else
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Dr"
            End If
        End If
        lblCustBal.Visible = True
        lblCustBal.Text = "Cust. Bal : " & opbal
        AccountComm()
    End Sub
    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        If dgCustomer.ColumnCount = 0 Then CustoemerRowColums()
        If dgCustomer.Rows.Count = 0 Then RetriveCustomer()
        If dgCustomer.SelectedRows.Count = 0 Then dgCustomer.Visible = True
        If txtCustomer.Text.Trim() <> "" Then
            RetriveCustomer(" And upper(AccountName) Like upper('" & txtCustomer.Text.Trim() & "%')")
        Else
            RetriveCustomer()
        End If
        txtcustomerID.Text = dgCustomer.SelectedRows(0).Cells(0).Value
        txtCustomer.Text = dgCustomer.SelectedRows(0).Cells(1).Value
        dgCustomer.Visible = False : AcCustBal()
        If lblCrate.Text = "Y" Then
            If txtcustomerID.Text = 7 Then
                cbAccountName.Enabled = True
                If dg1.SelectedRows.Count = 0 Then
                    clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33,11) order by AccountName", "AccountName", "ID", "--N./A.--")
                    cbAccountName.SelectedIndex = 0
                End If
            Else
                ' cbCrateMarka.SelectedIndex = 0 : cbCrateMarka.Enabled = True : txtCrateQty.Enabled = True
                cbAccountName.Enabled = False : cbAccountName.SelectedIndex = -1
                txtCrateQty.Text = Val(txtNug.Text)
            End If
            pnlMarka.Visible = True
            pnlMarka.BringToFront()
        Else
            pnlMarka.Visible = False
        End If
    End Sub

    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave
        If lblCrate.Text = "N" Then pnlMarka.Visible = False : Exit Sub
        txtNug.SelectionStart = 0
        If txtNug.Text = "" Then txtNug.Text = Val(0)
        txtCrateQty.Text = txtNug.Text
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

    Private Sub rowColums()
        '   Application.DoEvents()
        dg1.ColumnCount = 32
        dg1.Columns(0).HeaderText = "ID" : dg1.Columns(0).Name = "ColID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).HeaderText = "Item Name" : dg1.Columns(1).Name = "ColItem" : dg1.Columns(1).Width = 144
        dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).HeaderText = "Party Name" : dg1.Columns(2).Name = "ColParty" : dg1.Columns(2).Width = 184
        dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).HeaderText = "Cut" : dg1.Columns(3).Name = "ColCut" : dg1.Columns(3).Width = 99
        dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).HeaderText = "Nug" : dg1.Columns(4).Name = "colNug" : dg1.Columns(4).Width = 99
        dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).HeaderText = "Weight" : dg1.Columns(5).Name = "ColWeight" : dg1.Columns(5).Width = 99
        dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).HeaderText = "P Rate" : dg1.Columns(6).Name = "ColPartyRate" : dg1.Columns(6).Width = 99
        dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).HeaderText = "S Rate" : dg1.Columns(7).Name = "ColSallerRate" : dg1.Columns(7).Width = 99
        dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).HeaderText = "Per" : dg1.Columns(8).Name = "ColPer" : dg1.Columns(8).Width = 92
        dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(9).HeaderText = "Amount" : dg1.Columns(9).Name = "ColBasicAmount" : dg1.Columns(9).Width = 129
        dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).HeaderText = "Total" : dg1.Columns(10).Name = "ColTotal" : dg1.Columns(10).Width = 129
        dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).HeaderText = "Charges" : dg1.Columns(11).Name = "ColCharges" : dg1.Columns(11).Visible = False
        dg1.Columns(12).HeaderText = "SallerPerItem" : dg1.Columns(12).Name = "ColSallerPerItem" : dg1.Columns(12).Visible = False
        dg1.Columns(13).HeaderText = "Comm Per" : dg1.Columns(13).Name = "ColCommPer" : dg1.Columns(13).Visible = False
        dg1.Columns(14).HeaderText = "Comm Amt" : dg1.Columns(14).Name = "ColCommAmt" : dg1.Columns(14).Visible = False
        dg1.Columns(15).HeaderText = "M Per" : dg1.Columns(15).Name = "ColMPer" : dg1.Columns(15).Visible = False
        dg1.Columns(16).HeaderText = "M Amt" : dg1.Columns(16).Name = "ColMAmt" : dg1.Columns(16).Visible = False
        dg1.Columns(17).HeaderText = "RdfPer" : dg1.Columns(17).Name = "ColRdfPer" : dg1.Columns(17).Visible = False
        dg1.Columns(18).HeaderText = "RdfAmt" : dg1.Columns(18).Name = "ColRdfAmt" : dg1.Columns(18).Visible = False
        dg1.Columns(19).HeaderText = "TarePer" : dg1.Columns(19).Name = "ColTarePer" : dg1.Columns(19).Visible = False
        dg1.Columns(20).HeaderText = "TareAmt" : dg1.Columns(20).Name = "ColTareAmt" : dg1.Columns(20).Visible = False
        dg1.Columns(21).HeaderText = "LabourPer" : dg1.Columns(21).Name = "ColLabourPer" : dg1.Columns(21).Visible = False
        dg1.Columns(22).HeaderText = "LabourAmt" : dg1.Columns(22).Name = "ColLabourAmt" : dg1.Columns(22).Visible = False
        dg1.Columns(23).HeaderText = "Crate Marka" : dg1.Columns(23).Name = "ColCrateMarka" : dg1.Columns(23).Visible = False
        dg1.Columns(24).HeaderText = "CrateID" : dg1.Columns(24).Name = "ColCrateID" : dg1.Columns(24).Visible = False
        dg1.Columns(25).HeaderText = "Crate Qty" : dg1.Columns(25).Name = "ColCrateQty" : dg1.Columns(25).Visible = False
        dg1.Columns(26).HeaderText = "ItemID" : dg1.Columns(26).Name = "ColItemID" : dg1.Columns(26).Visible = False
        dg1.Columns(27).HeaderText = "CustomerID" : dg1.Columns(27).Name = "ColCustomerID" : dg1.Columns(27).Visible = False
        dg1.Columns(28).HeaderText = "Maintain Crate" : dg1.Columns(28).Name = "ColMaintainCrate" : dg1.Columns(28).Visible = False
        dg1.Columns(29).HeaderText = "CrateAccountID" : dg1.Columns(29).Name = "CrateAccountID" : dg1.Columns(29).Visible = False
        dg1.Columns(30).HeaderText = "CrateAccountName" : dg1.Columns(30).Name = "CrateAccountName" : dg1.Columns(30).Visible = False
        dg1.Columns(31).HeaderText = "AddWeight" : dg1.Columns(31).Name = "AddWeight" : dg1.Columns(31).Visible = False


        Dg2.ColumnCount = 7
        Dg2.Columns(0).HeaderText = "ID" : Dg2.Columns(0).Name = "ColID" : Dg2.Columns(0).Visible = False
        Dg2.Columns(1).HeaderText = "Charge Name" : Dg2.Columns(1).Name = "ColChargeName" : Dg2.Columns(1).Width = 259
        Dg2.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Dg2.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        Dg2.Columns(2).HeaderText = "On Value" : Dg2.Columns(2).Name = "ColOnValue" : Dg2.Columns(2).Width = 113
        Dg2.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(3).HeaderText = "@" : Dg2.Columns(3).Name = "ColCalculation" : Dg2.Columns(3).Width = 114
        Dg2.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(4).HeaderText = "+/-" : Dg2.Columns(4).Name = "ColPlusMinus" : Dg2.Columns(4).Width = 114
        Dg2.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dg2.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dg2.Columns(5).HeaderText = "Amount" : Dg2.Columns(5).Name = "ColChargeAmount" : Dg2.Columns(5).Width = 114
        Dg2.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(6).HeaderText = "CustomerID" : Dg2.Columns(6).Name = "ColChargeID" : Dg2.Columns(6).Visible = True
    End Sub

    Sub calc()
        txtTotalNug.Text = Format(0, "0.00") : txttotalWeight.Text = Format(0, "0.00")
        txtbasicTotal.Text = Format(0, "0.00") : txttotalCharges.Text = Format(0, "0.00")
        txtTotalNetAmount.Text = Format(0, "0.00") : txtSallerBasicTotal.Text = Format(0, "0.00")
        If dg1.Rows.Count > 9 Then dg1.Columns(10).Width = 109 Else dg1.Columns(10).Width = 129
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNug.Text = Format(Val(txtTotalNug.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txttotalWeight.Text = Format(Val(txttotalWeight.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtbasicTotal.Text = Format(Val(txtbasicTotal.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txttotalCharges.Text = Format(Val(txttotalCharges.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            txtTotalNetAmount.Text = Format(Val(txtTotalNetAmount.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            txtSallerBasicTotal.Text = Format(Val(txtSallerBasicTotal.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
        Next
        txtSallerCharges.Text = Format(0, "0.00")
        For i = 0 To Dg2.Rows.Count - 1
            Dim CalcType As String = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(6).Value) & "'")
            Dim PlusMinus As String = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(6).Value) & "'")
            If Dg2.Rows(i).Cells(4).Value = "-" Then
                txtSallerCharges.Text = Format(Val(txtSallerCharges.Text) - Val(Dg2.Rows(i).Cells(5).Value), "0.00")
            Else
                txtSallerCharges.Text = Format(Val(txtSallerCharges.Text) + Val(Dg2.Rows(i).Cells(5).Value), "0.00")
            End If
        Next
        lblcount.Text = "#: " & Val(dg1.RowCount)
        Dim tmpCustAmount As Decimal = 0.0
        Dim tmpSalleramount As Double = CDbl(Val(txtSallerBasicTotal.Text) + Val(txtSallerCharges.Text))
        ' txtsallerTotal.Text = Math.Round(Val(tmpSalleramount), 0)
        txtsallerTotal.Text = Math.Round(Val(tmpSalleramount), 0, MidpointRounding.AwayFromZero)
        txtRoff.Text = Format(Val(txtsallerTotal.Text) - Val(tmpSalleramount), "0.00")
        txtsallerTotal.Text = Format(Val(txtsallerTotal.Text), "0.00")
        Dim temproundoff As String = tmpCustAmount
        txtroundoff.Text = Format(Val(txtTotalNetAmount.Text) - (Val(txtbasicTotal.Text) + Val(txttotalCharges.Text)), "0.00")
        txtTotalNetAmount.Text = Format(Val(txtTotalNetAmount.Text), "0.00")
        If dg1.RowCount = 0 Then Exit Sub
        dg1.FirstDisplayedScrollingRowIndex = Val(dg1.RowCount - 1)
    End Sub

    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus, txtItem.Click
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
        txtCustomer.SelectAll()
    End Sub

    Private Sub txtCustomer_GotFocus(sender As Object, e As EventArgs) Handles txtCustomer.GotFocus, txtCustomer.Click
        If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
        If dgItemSearch.Rows.Count = 0 Then retriveItems()
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus() : Exit Sub
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        dgItemSearch.Visible = False : itemfill() : txtCustomer.SelectAll()
    End Sub

    Private Sub txtNug_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNug.KeyUp, txtTare.KeyUp, txtSallerRate.KeyUp, txtSallerAmout.KeyUp, txtRdfPer.KeyUp, txtOnValue.KeyUp, txtNetRate.KeyUp, txtMPer.KeyUp, txtLabour.KeyUp, txtWeight.KeyUp, txtItem.KeyUp, txtCut.KeyUp, txtCustomerRate.KeyUp, txtCrateQty.KeyUp, txtComPer.KeyUp, txtchargesAmount.KeyUp
        If BtnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) > 0 Then
                txtWeight.Text = Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) * Val(txtNug.Text)
            End If
        End If
    End Sub

    Private Sub txtKg_GotFocus(sender As Object, e As EventArgs) Handles txtWeight.GotFocus, txtWeight.Click
        txtWeight.SelectAll()
        If txtAddWeight.Text = "" Then lblAddWeight.Text = "" Else lblAddWeight.Text = txtAddWeight.Text
        lblAddWeight.Visible = True
    End Sub

    Private Sub txtCustomerRate_GotFocus(sender As Object, e As EventArgs) Handles txtCustomerRate.GotFocus, txtCustomerRate.Click
        If ckTaxPaid.Checked = True Then pnlNetRate.Visible = True : txtNetRate.Focus()
        txtCustomerRate.SelectionStart = 0 : txtCustomerRate.SelectionLength = Len(txtCustomerRate.Text)
    End Sub

    Private Sub txtSallerRate_GotFocus(sender As Object, e As EventArgs) Handles txtSallerRate.GotFocus, txtSallerRate.Click
        txtSallerRate.SelectionStart = 0 : txtSallerRate.SelectionLength = Len(txtSallerRate.Text)
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
        If dgItemSearch.RowCount = 0 Then retriveItems()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        txtAccount.SelectAll()
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.BackColor = Color.LightGray
        mskEntryDate.SelectAll()
    End Sub

    Private Sub txtVehicleNo_GotFocus(sender As Object, e As EventArgs) Handles txtVehicleNo.GotFocus,
     txtVoucherNo.GotFocus, txtAccount.GotFocus, txtItem.GotFocus, txtCustomer.GotFocus, txtCut.GotFocus, txtNug.GotFocus, txtWeight.GotFocus,
     txtCustomerRate.GotFocus, txtSallerRate.GotFocus, txtComPer.GotFocus, txtMPer.GotFocus, txtRdfPer.GotFocus, txtTare.GotFocus,
     txtLabour.GotFocus, txtComAmt.GotFocus, txtMAmt.GotFocus, txtRdfAmt.GotFocus, txtTareAmt.GotFocus, txtLaboutAmt.GotFocus,
     txtNetRate.GotFocus, txtBasicCustomer.GotFocus, txtTotAmount.GotFocus, txtCrateQty.GotFocus
        If txtCut.Focused Then itemfill()
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub

    Private Sub txtVehicleNo_LostFocus(sender As Object, e As EventArgs) Handles txtVehicleNo.LostFocus,
     txtVoucherNo.LostFocus, txtAccount.LostFocus, txtItem.LostFocus, txtCustomer.LostFocus, txtCut.LostFocus, txtNug.LostFocus, txtWeight.LostFocus,
     txtCustomerRate.LostFocus, txtSallerRate.LostFocus, txtComPer.LostFocus, txtMPer.LostFocus, txtRdfPer.LostFocus, txtTare.LostFocus,
     txtLabour.LostFocus, txtComAmt.LostFocus, txtMAmt.LostFocus, txtRdfAmt.LostFocus, txtTareAmt.LostFocus, txtLaboutAmt.LostFocus,
     txtNetRate.LostFocus, txtBasicCustomer.LostFocus, txtTotAmount.LostFocus, txtCrateQty.LostFocus
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
    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVehicleNo.KeyDown,
        txtVoucherNo.KeyDown, txtAccount.KeyDown, txtItem.KeyDown, txtCustomer.KeyDown, txtCut.KeyDown, txtNug.KeyDown, txtWeight.KeyDown,
        txtCustomerRate.KeyDown, txtSallerRate.KeyDown, Cbper.KeyDown, cbAccountName.KeyDown, cbCrateMarka.KeyDown, ckTaxPaid.KeyDown,
        txtComPer.KeyDown, txtMPer.KeyDown, txtRdfPer.KeyDown, txtTare.KeyDown, txtLabour.KeyDown, txtComAmt.KeyDown, txtMAmt.KeyDown,
        txtRdfAmt.KeyDown, txtTareAmt.KeyDown, txtLaboutAmt.KeyDown, txtBasicCustomer.KeyDown
        If txtVoucherNo.Focused Then
            If e.KeyCode = Keys.F2 Then
                pnlInvoiceID.Visible = True
                txtInvoiceID.Focus()
                e.SuppressKeyPress = True
            End If
        End If
        If txtVehicleNo.Focused Then
            If e.KeyCode = Keys.F3 Then
                pnlSendingDetails.Visible = True
                pnlSendingDetails.BringToFront()
                txtCustMobile.Focus()
            End If
        End If
        If DgAccountSearch.Visible = False And dgItemSearch.Visible = False And dgCustomer.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or Cbper.Focused = True Or cbCrateMarka.Focused = True Or cbAccountName.Focused = True Or txtCrateQty.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If

        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.Down Then
                If dgItemSearch.Visible = True Then dgItemSearch.Focus() : Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
            If e.KeyCode = Keys.F3 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                Item_form.Opener = Me
                Item_form.OpenedFromItems = True
                If Not Item_form Is Nothing Then
                    Item_form.BringToFront()
                End If
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
        End If
        If txtWeight.Focused Then
            If e.KeyCode = Keys.F3 Then
                pnlAddWeight.Visible = True
                txtAddWeight.Focus()
            End If
        End If
        If cbCrateMarka.Focused Then
            If e.KeyCode = Keys.F3 Then
                CrateForm.MdiParent = MainScreenForm
                CrateForm.Show()
                If Not CrateForm Is Nothing Then
                    CrateForm.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.F1 Then
                Dim tmpMarkaID As String = cbCrateMarka.SelectedValue
                CrateForm.MdiParent = MainScreenForm
                CrateForm.Show()
                CrateForm.FillControls(tmpMarkaID)
                If Not CrateForm Is Nothing Then
                    CrateForm.BringToFront()
                End If
            End If
        End If
        If txtCustomer.Focused Then
            If e.KeyCode = Keys.Down Then
                If dgCustomer.Visible = True Then dgCustomer.Focus() : Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show() : CreateAccount.Opener = Me
                CreateAccount.OpenedFromItems = True
                CreateAccount.TextBoxSender = txtCustomer.Name
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID in(16,32) Order by ID Desc ", "GroupName", "ID", "")
                CreateAccount.BringToFront()
            End If
            If e.KeyCode = Keys.Down Then dgCustomer.Focus()
            If e.KeyCode = Keys.F1 Then
                If dgCustomer.SelectedRows.Count = 0 Then Exit Sub
                Dim tmpID As String = Val(dgCustomer.SelectedRows(0).Cells(0).Value)
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show() : CreateAccount.Opener = Me
                CreateAccount.TextBoxSender = txtCustomer.Name
                CreateAccount.OpenedFromItems = True
                CreateAccount.FillContros(tmpID)
                CreateAccount.BringToFront()
            End If
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then
                If DgAccountSearch.Visible = True Then DgAccountSearch.Focus() : Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show() : CreateAccount.Opener = Me
                CreateAccount.OpenedFromItems = True
                CreateAccount.TextBoxSender = txtAccount.Name
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID in(17,33) Order by ID Desc ", "GroupName", "ID", "")
                CreateAccount.BringToFront()
            End If
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
            If e.KeyCode = Keys.F1 Then
                If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
                Dim tmpID As String = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show() : CreateAccount.Opener = Me
                CreateAccount.TextBoxSender = txtAccount.Name
                CreateAccount.OpenedFromItems = True
                CreateAccount.FillContros(tmpID)
                CreateAccount.BringToFront()
            End If
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                txtAdvancePay.Focus()
        End Select
        Select Case e.KeyCode
            Case Keys.PageDown
                e.Handled = True
                txtCharges.Focus()
        End Select
    End Sub
    Private Sub ckTaxPaid_GotFocus(sender As Object, e As EventArgs) Handles ckTaxPaid.GotFocus
        ckTaxPaid.ForeColor = Color.GhostWhite
        ckTaxPaid.BackColor = Color.Green
    End Sub

    Private Sub ckTaxPaid_LostFocus(sender As Object, e As EventArgs) Handles ckTaxPaid.LostFocus
        ckTaxPaid.ForeColor = Color.Black
        ckTaxPaid.BackColor = Color.FromArgb(247, 220, 111)
    End Sub

    Private Sub SuperCalculation()
        If ckTaxPaid.Checked = True Then
            Dim TotTaxPer As Decimal = Format(Val(txtComPer.Text) + Val(txtMPer.Text) + Val(txtRdfPer.Text) + 100, "0.00")
            Dim incRate As Decimal = Val(txtNetRate.Text) * (100 / TotTaxPer)
            txtCustomerRate.Text = Format(Val(incRate), "0.00")
            If Cbper.SelectedIndex = 0 Then
                txtBasicCustomer.Text = Format(Val(txtNug.Text) * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 1 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 2 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 5 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 5 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 3 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 10 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 10 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 4 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 20 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 20 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 5 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 40 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 40 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 6 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 41 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 41 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 7 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 50 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 50 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 8 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 51 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 51 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 9 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 51.7 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 51.7 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 10 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 52.3 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 52.3 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 11 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 53 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 53 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 12 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 80 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 80 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 13 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 100 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 100 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 14 Then
                txtBasicCustomer.Text = Format(Val(txtNug.Text) * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            End If
        Else
            If Cbper.SelectedIndex = 0 Then
                txtBasicCustomer.Text = Format(Val(txtNug.Text) * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 1 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 2 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 5 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 5 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 3 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 10 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 10 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 4 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 20 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 20 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 5 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 40 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 40 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 6 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 41 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 41 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 7 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 50 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 50 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 8 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 51 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 51 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 9 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 51.7 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 51.7 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 10 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 52.3 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 52.3 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 11 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 53 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 53 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 12 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 80 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 80 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 13 Then
                txtBasicCustomer.Text = Format(Val(txtWeight.Text) / 100 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtWeight.Text) / 100 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 14 Then
                txtBasicCustomer.Text = Format(Val(txtNug.Text) * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            End If
        End If

        txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtBasicCustomer.Text) / 100, "0.00")
        txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtBasicCustomer.Text) / 100, "0.00")
        If Val(txtRdfPer.Text) > 0 Then
            txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtBasicCustomer.Text) / 100, "0.00")
        Else
            txtRdfAmt.Text = Format(Val(txtRdfAmt.Text), "0.00")
        End If
        If clsFun.ExecScalarStr("Select ApplyCommWeight From Controls") = "Yes" Then
            txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtWeight.Text), "0.00")
        Else
            Dim crateRate As String = clsFun.ExecScalarStr("Select CrateBardana From Controls")
            If crateRate = "Yes" And lblCrate.Text = "Y" Then txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtCrateQty.Text), "0.00") Else txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
        End If
        txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
        lbltotCharges.Text = Format(Val(Val(txtComAmt.Text)) + Val(Val(txtMAmt.Text)) + Val(Val(txtRdfAmt.Text)) + Val(Val(txtTareAmt.Text)) + Val(Val(txtLaboutAmt.Text)), "0.00")
        txtTotAmount.Text = Format(Val(Val(lbltotCharges.Text)) + Val(Val(txtBasicCustomer.Text)), 0)
        txtTotAmount.Text = Math.Round(Val(txtTotAmount.Text), 0, MidpointRounding.AwayFromZero)
    End Sub
    Private Sub txtNetRate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNetRate.KeyDown
        If e.KeyCode = Keys.Enter Then
            If ckTaxPaid.Checked = True Then
                pnlNetRate.Visible = False
                txtSallerRate.Focus()
                e.SuppressKeyPress = True
                txtSallerRate.Text = txtCustomerRate.Text
            Else
                txtNetRate.Text = ""
            End If

        End If
    End Sub

    Private Sub txtTotAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotAmount.KeyDown
        If Val(txtItemID.Text) = 0 Then txtItem.Focus() : Exit Sub
        If Cbper.SelectedIndex = 0 Then
            If Val(txtNug.Text) = 0 Then MsgBox("please fill Nug...", MsgBoxStyle.Critical, "Access Denied") : txtNug.Focus() : Exit Sub
        ElseIf Cbper.SelectedIndex = 14 Then
            If lblCrate.Text = "Y" Then
                pnlMarka.Visible = True : pnlMarka.BringToFront()
                If Val(txtCrateQty.Text) = 0 Then MsgBox("please fill Crate Qty...", MsgBoxStyle.Critical, "Access Denied") : txtCrateQty.Focus() : Exit Sub
            End If
        Else
            If Val(txtWeight.Text) = 0 Then MsgBox("please fill Weight...", MsgBoxStyle.Critical, "Access Denied") : txtWeight.Focus() : Exit Sub
        End If
        If e.KeyCode = Keys.Enter Then
            If txtSallerCharges.Text = "" Then txtSallerCharges.Text = "0.00"
            If txtNug.Text = "" Then txtNug.Text = "0.00"
            If txtWeight.Text = "" Then txtWeight.Text = "0.00"
            If txtCustomerRate.Text = "" Then txtCustomerRate.Text = "0.00"
            If txtCut.Text = "" Then txtCut.Text = "0.00"
            If txtSallerRate.Text = "" Then txtSallerRate.Text = "0.00"
            If Val(txtNug.Text) = 0 And Val(txtWeight.Text) = 0 Then
                MsgBox("please fill Nug or Weight", MsgBoxStyle.Critical, "Empty nug or Weight...")
                txtNug.Focus()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(1).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(2).Value = txtCustomer.Text
                dg1.SelectedRows(0).Cells(3).Value = IIf(Val(txtCut.Text) > 0, Format(Val(txtCut.Text), "0.00"), "")
                dg1.SelectedRows(0).Cells(4).Value = txtNug.Text
                dg1.SelectedRows(0).Cells(5).Value = txtWeight.Text
                dg1.SelectedRows(0).Cells(6).Value = txtCustomerRate.Text
                dg1.SelectedRows(0).Cells(7).Value = txtSallerRate.Text
                dg1.SelectedRows(0).Cells(8).Value = Cbper.Text
                dg1.SelectedRows(0).Cells(9).Value = txtBasicCustomer.Text
                dg1.SelectedRows(0).Cells(10).Value = txtTotAmount.Text
                dg1.SelectedRows(0).Cells(11).Value = lbltotCharges.Text
                dg1.SelectedRows(0).Cells(12).Value = txtSallerAmout.Text
                dg1.SelectedRows(0).Cells(13).Value = txtComPer.Text
                dg1.SelectedRows(0).Cells(14).Value = txtComAmt.Text
                dg1.SelectedRows(0).Cells(15).Value = txtMPer.Text
                dg1.SelectedRows(0).Cells(16).Value = txtMAmt.Text
                dg1.SelectedRows(0).Cells(17).Value = txtRdfPer.Text
                dg1.SelectedRows(0).Cells(18).Value = txtRdfAmt.Text
                dg1.SelectedRows(0).Cells(19).Value = txtTare.Text
                dg1.SelectedRows(0).Cells(20).Value = txtTareAmt.Text
                dg1.SelectedRows(0).Cells(21).Value = txtLabour.Text
                dg1.SelectedRows(0).Cells(22).Value = txtLaboutAmt.Text
                dg1.SelectedRows(0).Cells(23).Value = cbCrateMarka.Text
                dg1.SelectedRows(0).Cells(24).Value = cbCrateMarka.SelectedValue
                dg1.SelectedRows(0).Cells(25).Value = txtCrateQty.Text
                dg1.SelectedRows(0).Cells(26).Value = txtItemID.Text
                dg1.SelectedRows(0).Cells(27).Value = txtcustomerID.Text
                dg1.SelectedRows(0).Cells(28).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(29).Value = cbAccountName.SelectedValue
                dg1.SelectedRows(0).Cells(30).Value = cbAccountName.Text
                dg1.SelectedRows(0).Cells(31).Value = txtAddWeight.Text
                txtItem.Focus() : calc() : dg1.ClearSelection() : cleartxt()
            Else
                If lblCrate.Text = "Y" Then
                    If cbAccountName.SelectedIndex <> 0 Then
                        dg1.Rows.Add("", txtItem.Text, txtCustomer.Text, IIf(Val(txtCut.Text) > 0, Format(Val(txtCut.Text), "0.00"), ""), Format(Val(txtNug.Text), "0.00"), Format(Val(txtWeight.Text), "0.00"),
              Format(Val(txtCustomerRate.Text), "0.00"), Format(Val(txtSallerRate.Text), "0.00"),
                  Cbper.Text, Format(Val(txtBasicCustomer.Text), "0.00"), Format(Val(txtTotAmount.Text), "0.00"), lbltotCharges.Text, txtSallerAmout.Text,
                  txtComPer.Text, txtComAmt.Text, txtMPer.Text, txtMAmt.Text, txtRdfPer.Text, txtRdfAmt.Text, txtTare.Text, txtTareAmt.Text,
                  txtLabour.Text, txtLaboutAmt.Text, cbCrateMarka.Text, cbCrateMarka.SelectedValue, txtCrateQty.Text, txtItemID.Text,
                  txtcustomerID.Text, lblCrate.Text, Val(cbAccountName.SelectedValue), cbAccountName.Text, txtAddWeight.Text)
                    Else
                        dg1.Rows.Add("", txtItem.Text, txtCustomer.Text, IIf(Val(txtCut.Text) > 0, Format(Val(txtCut.Text), "0.00"), ""), Format(Val(txtNug.Text), "0.00"), Format(Val(txtWeight.Text), "0.00"),
                        Format(Val(txtCustomerRate.Text), "0.00"), Format(Val(txtSallerRate.Text), "0.00"),
                        Cbper.Text, Format(Val(txtBasicCustomer.Text), "0.00"), Format(Val(txtTotAmount.Text), "0.00"), lbltotCharges.Text, txtSallerAmout.Text,
                        txtComPer.Text, txtComAmt.Text, txtMPer.Text, txtMAmt.Text, txtRdfPer.Text, txtRdfAmt.Text, txtTare.Text, txtTareAmt.Text,
                        txtLabour.Text, txtLaboutAmt.Text, IIf(cbAccountName.SelectedIndex = 0, "", cbCrateMarka.Text),
                        IIf(cbAccountName.SelectedIndex = 0, "", cbCrateMarka.SelectedValue), IIf(cbAccountName.SelectedIndex = 0, "", txtCrateQty.Text), txtItemID.Text,
                        txtcustomerID.Text, lblCrate.Text, Val(cbAccountName.SelectedValue), cbAccountName.Text, txtAddWeight.Text)
                    End If
                Else
                    dg1.Rows.Add("", txtItem.Text, txtCustomer.Text, Val(txtCut.Text), Format(Val(txtNug.Text), "0.00"), Format(Val(txtWeight.Text), "0.00"),
                            Format(Val(txtCustomerRate.Text), "0.00"), Format(Val(txtSallerRate.Text), "0.00"),
                                Cbper.Text, Format(Val(txtBasicCustomer.Text), "0.00"), Format(Val(txtTotAmount.Text), "0.00"), lbltotCharges.Text,
                                txtSallerAmout.Text, txtComPer.Text, txtComAmt.Text, txtMPer.Text, txtMAmt.Text, txtRdfPer.Text, txtRdfAmt.Text,
                                txtTare.Text, txtTareAmt.Text, txtLabour.Text, txtLaboutAmt.Text, "", 0, 0, txtItemID.Text, txtcustomerID.Text, lblCrate.Text, "", "", txtAddWeight.Text)
                End If
                calc() : cleartxt()
                txtItem.Focus()
                dg1.ClearSelection()
            End If
        End If
        '  calc()
    End Sub
    Private Sub txtCrateQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCrateQty.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
            pnlMarka.Visible = False
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus() : pnlMarka.Visible = False
        End Select
    End Sub
    Private Sub cleartxt()
        txtCut.Text = "" : txtNug.Text = ""
        txtWeight.Text = "" : txtTotAmount.Text = ""
        txtCrateQty.Clear() : txtBasicCustomer.Text = ""
        txtAddWeight.Text = "" : lblAddWeight.Visible = False
        txtComPer.Text = 0 : txtComAmt.Text = 0
        txtMPer.Text = 0 : txtMAmt.Text = 0
        txtTare.Text = 0 : txtTareAmt.Text = 0
        txtLabour.Text = 0 : txtLaboutAmt.Text = 0
        lblCrate.Text = "N" : lblAddWeight.Visible = False
        cbAccountName.SelectedIndex = -1
        pnlMarka.Visible = False
    End Sub
    Private Sub FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        If dg1.Rows.Count > 9 Then dg1.Columns(10).Width = 109 Else dg1.Columns(10).Width = 129
        txtAccountID.Text = "" : txtSallerBasicTotal.Clear()
        txtAccount.Text = "" : txtTotalNug.Text = ""
        txtbasicTotal.Text = "" : txttotalWeight.Text = ""
        txtTotalNetAmount.Text = "" : txttotalCharges.Text = ""
        txtroundoff.Text = "" : txtVehicleNo.Text = ""
        txtCustomerRate.Text = "" : VNumber()
        txtAdvancePay.Text = "" : MainScreenPicture.retrive()
        txtSallerAmout.Clear() : txtSallerCharges.Clear()
        txtsallerTotal.Clear() : txtRoff.Clear()
        BtnSave.Text = "&Save" : BtnDelete.Text = "&Delete"
    End Sub
    Private Sub cleartxtCharges()
        txtOnValue.Text = "" : txtCalculatePer.Text = ""
        txtPlusMinus.Text = "" : txtchargesAmount.Text = ""
    End Sub
    Private Sub ChargesCalculation()
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
    End Sub
    Private Sub Cbper_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbper.SelectedIndexChanged
        SuperCalculation()
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
    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtWeight.KeyPress, txtSallerRate.KeyPress,
        txtCustomerRate.KeyPress, txtComPer.KeyPress, txtbasicTotal.KeyPress, txtchargesAmount.KeyPress, txtComPer.KeyPress, txtCrateQty.KeyPress,
        txtCustomerRate.KeyPress, txtLabour.KeyPress, txtCustomerRate.KeyPress, txtMPer.KeyPress, txtBasicCustomer.KeyPress, txtTotAmount.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub
    Private Sub txtCustomerRate_Leave(sender As Object, e As EventArgs) Handles txtComAmt.Leave

    End Sub
    Private Sub txtComPer_Leave(sender As Object, e As EventArgs) Handles txtComAmt.Leave, txtRdfAmt.Leave, txtMAmt.Leave, txtLaboutAmt.Leave, txtTareAmt.Leave
        If txtComPer.Text = "" Then txtComPer.Text = "0.00"
        SuperCalculation()
    End Sub
    Private Sub txtMPer_Leave(sender As Object, e As EventArgs) Handles txtMPer.Leave
        If txtMPer.Text = "" Then txtMPer.Text = "0.00"
    End Sub
    Private Sub txtRdfAmt_Leave(sender As Object, e As EventArgs) Handles txtRdfAmt.Leave
        If txtRdfPer.Text = "" Then txtRdfPer.Text = "0.00"
    End Sub
    Private Sub txtTare_Leave(sender As Object, e As EventArgs) Handles txtTare.Leave
        If txtTare.Text = "" Then txtTare.Text = "0.00"
    End Sub
    Private Sub txtLabour_Leave(sender As Object, e As EventArgs) Handles txtLabour.Leave
        If txtLabour.Text = "" Then txtLabour.Text = "0.00"
    End Sub
    Private Sub txtOnValue_GotFocus(sender As Object, e As EventArgs) Handles txtOnValue.GotFocus
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        dgCharges.Visible = False : FillCharges()
    End Sub

    Private Sub txtRdfPer_Leave(sender As Object, e As EventArgs) Handles txtRdfPer.Leave
        If txtRdfPer.Text = "" Then txtRdfPer.Text = "0"
    End Sub
    Private Sub txtNug_KeyUp(sender As Object, e As EventArgs) Handles txtNug.KeyUp, txtItem.KeyUp, txtSallerRate.KeyUp,
            txtSallerAmout.KeyUp, txtOnValue.KeyUp, txtchargesAmount.KeyUp, txtWeight.KeyUp, txtCut.KeyUp, txtCustomerRate.KeyUp,
            txtComPer.KeyUp, txtMPer.KeyUp, txtRdfPer.KeyUp, txtLabour.KeyUp, txtTare.KeyUp, txtCrateQty.KeyUp, txtNetRate.KeyUp, txtCrateQty.KeyUp
        ChargesCalculation() : SuperCalculation()
        Try
            lblInword.Text = AmtInWord(txtsallerTotal.Text)
        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
    End Sub

    'Private Sub txtNug_TextChanged(sender As Object, e As EventArgs) Handles txtNug.TextChanged, txtItem.TextChanged, txtSallerRate.TextChanged,
    '    txtSallerAmout.TextChanged, txtOnValue.TextChanged, txtchargesAmount.TextChanged, txtKg.TextChanged, txtCut.TextChanged, txtCustomerRate.TextChanged,
    '    txtComPer.TextChanged, txtMPer.TextChanged, txtRdfPer.TextChanged, txtLabour.TextChanged, txtTare.TextChanged, txtCrateQty.TextChanged, txtNetRate.TextChanged, txtCrateQty.TextChanged
    '    ChargesCalculation() : SuperCalculation()
    '    Try
    '        lblInword.Text = AmtInWord(txtsallerTotal.Text)
    '    Catch ex As Exception
    '        lblInword.Text = ex.ToString
    '    End Try
    'End Sub
    Private Sub RemoveDuplicateInvoice()
        Dim dt As New DataTable
        Dim sql As String = "Select ID,billNo,InvoiceID,COUNT(ID) c FROM Vouchers Where TransType='" & Me.Text & "' GROUP BY billNo HAVING c > 1"
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    If i > 0 Then
                        Dim tmpbillNo As Integer = Val(dt.Rows(i)("BillNo").ToString())
                        tmpbillNo = tmpbillNo + 1
                        Dim tempInvoiceID As Integer = Val(dt.Rows(i)("InvoiceID").ToString())
                        tempInvoiceID = tempInvoiceID + 1
                        clsFun.ExecScalarStr("Update Vouchers Set BillNo='" & Val(tmpbillNo) & "',InvoiceID='" & Val(tempInvoiceID) & "' Where ID=" & Val(dt.Rows(i)("ID").ToString()) & "")
                        txtid.Text = Val(dt.Rows(i)("InvoiceID").ToString())
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Save()
        Dim SqliteEntryDate As String = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        dg1.ClearSelection() ': Dim device As Devices.ComputerInfo
        Dim cmd As SQLite.SQLiteCommand
        Dim UserID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "insert into Vouchers(TransType,BillNo,VehicleNo, Entrydate,SallerID, SallerName, Nug, kg,BasicAmount, TotalAmount,TotalCharges, " _
                                    & "N1,N2,InvoiceID,UserID,EntryTime,T1,T2,T3,T4,T5,T6,T7,T8,T9,T10)" _
                                    & "values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10,@11,@12,@13,@14,@15,@16,@17, @18, @19, @20, @21, @22, @23, @24, @25, @26)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@3", txtVehicleNo.Text)
            cmd.Parameters.AddWithValue("@4", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@5", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@6", txtAccount.Text)
            cmd.Parameters.AddWithValue("@7", Val(txtTotalNug.Text))
            cmd.Parameters.AddWithValue("@8", Val(txttotalWeight.Text))
            cmd.Parameters.AddWithValue("@9", Val(txtSallerBasicTotal.Text))
            cmd.Parameters.AddWithValue("@10", Val(txtsallerTotal.Text))
            cmd.Parameters.AddWithValue("@11", Val(txtSallerCharges.Text))
            cmd.Parameters.AddWithValue("@12", Val(0))
            cmd.Parameters.AddWithValue("@13", Val(txtAdvancePay.Text))
            cmd.Parameters.AddWithValue("@14", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@15", Val(UserID))
            cmd.Parameters.AddWithValue("@16", Now.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@17", txtDriverName.Text)
            cmd.Parameters.AddWithValue("@18", txtDriverMobile.Text)
            cmd.Parameters.AddWithValue("@19", txtRemark.Text)
            cmd.Parameters.AddWithValue("@20", txtStateName.Text)
            cmd.Parameters.AddWithValue("@21", txtGSTN.Text)
            cmd.Parameters.AddWithValue("@22", txtCustMobile.Text)
            cmd.Parameters.AddWithValue("@23", txtBrokerName.Text)
            cmd.Parameters.AddWithValue("@24", txtBrokerMob.Text)
            cmd.Parameters.AddWithValue("@25", txtTransPort.Text)
            cmd.Parameters.AddWithValue("@26", txtGrNo.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                el.WriteToErrorLog(sql, Constants.compname, "Super Sale Saved")
                txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
                clsFun.CloseConnection()
            End If
            ServerTag = 1 : Dg1Record() : dg2Record()
            LegerSeller() : Ledgercharges() : LedgerCustomers() : LedgerCrate()
            ServerLedgerSeller() : ServerLedgerCustomers() : ServerchargesLedger() : ServerLedgerCrate()
            '''' ServerLedger Recrods
            If Val(txtAdvancePay.Text) > 0 Then
                PaymentInfo()
            End If
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")

            'RemoveDuplicateInvoice()
            mskEntryDate.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        ' retrive2()
    End Sub
    Private Sub PaymentInfo()
        Dim RemarkHindi As String = String.Empty
        Dim Remark2 As String = String.Empty
        Dim PaymentNo As Integer = clsFun.ExecScalarInt("SELECT InvoiceID AS NumberOfProducts FROM Vouchers WHERE id=(SELECT max(id) FROM Vouchers Where TransType='Payment')")
        '   vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")

        Dim ssql As String = "insert into Vouchers(EntryDate,TransType,SallerID,sallerName,AccountID,AccountName,BasicAmount,DiscountAmount,TotalAmount,Remark, " &
        "billNo,InvoiceID,PaymentID,PaymentAmount) values ('" & CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd") & "', 'Payment', " & Val(7) & ", " &
         "'" & clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=7") & "','" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "', " &
         "'" & Val(txtAdvancePay.Text) & "','" & Val(0) & "','" & Val(txtAdvancePay.Text) & "','','" & Val(PaymentNo + 1) & "','" & Val(PaymentNo + 1) & "','" & Val(txtid.Text) & "','" & Val(txtAdvancePay.Text) & "')"
        clsFun.ExecNonQuery(ssql)
        Dim vchid As Integer = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
        clsFun.ExecScalarStr("Update Vouchers Set PaymentID=" & Val(vchid) & ", PaymentAmount='" & Val(txtAdvancePay.Text) & "' Where ID='" & Val(txtid.Text) & "'")
        Dim FastQuery As String = String.Empty
        If Val(txtAccountID.Text) > 0 Then ''Party Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(vchid) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','Payment'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtAdvancePay.Text) & ",'D','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & RemarkHindi & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'," & Val(7) & ",'Cash'"
        End If
        If Val(txtAdvancePay.Text) > 0 Then ''Total Amout
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(vchid) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','Payment'," & Val(7) & ",'Cash'," & Val(txtAdvancePay.Text) & ",'C','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & RemarkHindi & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'"
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastReceipt(FastQuery)
        clsFun.ExecScalarStr("Update Vouchers Set PaymentID=" & Val(txtid.Text) & ", PaymentAmount='" & Val(txtAdvancePay.Text) & "' Where ID='" & Val(vchid) & "'")
    End Sub
    Private Sub LegerSeller()
        Dim FastQuery As String = String.Empty
        Dim RemarkHindi As String = String.Empty
        Dim sql As String = String.Empty
        Dim PostingID As Integer = clsFun.ExecScalarInt("Select PostingID From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
        Dim PostingName As String = clsFun.ExecScalarStr("Select PostingAcName From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
        If Val(txtSallerBasicTotal.Text) > 0 Then ''Manual Beejak Account Fixed
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 46 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Val(txtSallerBasicTotal.Text) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','','' "
        End If
        If Val(clsFun.ExecScalarStr("Select PostingID From Accounts Where ID='" & Val(txtAccountID.Text) & "'")) <> 0 Then
            If Val(txtsallerTotal.Text) > 0 Then ''Account 
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(PostingID) & ",'" & PostingName & "','" & Val(txtsallerTotal.Text) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','','' "
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(PostingID) & ",'" & PostingName & "','" & Math.Abs(Val(txtsallerTotal.Text)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','','' "
            End If
        Else
            If Val(txtsallerTotal.Text) > 0 Then ''Account 
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Val(txtsallerTotal.Text) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','','' "
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(txtsallerTotal.Text)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','','' "
            End If
        End If

        If Val(txtRoff.Text) <> 0 Then ''Account 
            If Val(txtRoff.Text) < 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Math.Abs(Val(txtRoff.Text)) & ",'C' ,'" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','" & txtAccount.Text & "','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "' "
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Val(txtRoff.Text) & ",'D' ,'" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','" & txtAccount.Text & "','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "' "
            End If
        End If
        Dim isDiff As String = clsFun.ExecScalarStr("Select sendDiff From Controls")
        If isDiff = "Yes" Then
            Dim diff As Decimal = Val(txtSallerBasicTotal.Text) - Val(txtbasicTotal.Text)
            If diff = 0 Then Exit Sub
            If Val(diff) < 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 56 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "'," & Math.Abs(Val(diff)) & ",'D' ,'" & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & "','" & txtAccount.Text & "','" & RemarkHindi & "' "
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 38 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "'," & Math.Abs(Val(diff)) & ",'C' ,'" & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & "','" & txtAccount.Text & "','" & RemarkHindi & "' "
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 56 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "'," & Math.Abs(Val(diff)) & ",'C' ,'" & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & "','" & txtAccount.Text & "','" & RemarkHindi & "' "
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 38 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "'," & Math.Abs(Val(diff)) & ",'D' ,'" & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & "','" & txtAccount.Text & "','" & RemarkHindi & "' "
            End If
        End If
        If FastQuery = "" Then Exit Sub
        clsFun.FastLedger(FastQuery)
        clsFun.ExecNonQuery("Update Ledger SET POstingID=" & Val(txtAccountID.Text) & ",PostingAccount='" & txtAccount.Text & "' Where VourchersID=" & Val(txtid.Text) & " and AccountID=" & Val(PostingID) & " ")
    End Sub

    Private Sub ServerLedgerSeller()
        Dim RemarkHindi As String = String.Empty
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        If Val(txtSallerBasicTotal.Text) > 0 Then ''Manual Beejak Account Fixed
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 46 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=46") & "','" & Val(txtSallerBasicTotal.Text) & "','D','" & Val(ServerTag) & "','" & Val(OrgID) & "', '" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','','' "
        End If
        If Val(txtsallerTotal.Text) > 0 Then ''Account 
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Val(txtsallerTotal.Text) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "', '" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','','' "
        End If
        If Val(txtRoff.Text) <> 0 Then ''Account 
            If Val(txtRoff.Text) < 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Math.Abs(Val(txtRoff.Text)) & ",'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','" & txtAccount.Text & "','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "' "
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Val(txtRoff.Text) & ",'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "','" & txtAccount.Text & "','" & "Voucher No.:" & txtVoucherNo.Text & " : " & txtAccount.Text & " Value : " & txtsallerTotal.Text & "' "
            End If
        End If
        Dim isDiff As String = clsFun.ExecScalarStr("Select sendDiff From Controls")
        If isDiff = "Yes" Then
            Dim diff As Decimal = Val(txtSallerBasicTotal.Text) - Val(txtbasicTotal.Text)
            If diff <> 0 Then Exit Sub
            If Val(diff) < 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 56 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "'," & Math.Abs(Val(diff)) & ",'D','" & Val(ServerTag) & "','" & Val(OrgID) & "'," & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & ",'" & txtAccount.Text & "','" & RemarkHindi & "' "
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 38 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "'," & Math.Abs(Val(diff)) & ",'C' ,'" & Val(ServerTag) & "','" & Val(OrgID) & "'," & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & ",'" & txtAccount.Text & "','" & RemarkHindi & "' "

            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 56 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=56") & "'," & Math.Abs(Val(diff)) & ",'C','" & Val(ServerTag) & "','" & Val(OrgID) & "'," & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & ",'" & txtAccount.Text & "','" & RemarkHindi & "' "
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 38 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=38") & "'," & Math.Abs(Val(diff)) & ",'D','" & Val(ServerTag) & "','" & Val(OrgID) & "'," & "Voucher No.:" & txtVoucherNo.Text & " : Sellout Mannual Value : " & diff & ",'" & txtAccount.Text & "','" & RemarkHindi & "' "
            End If
        End If
        If FastQuery = "" Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub
    Private Sub Dg1Record()

        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells("ColItem").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd") & "'," & Val(txtid.Text) & ", '" & Me.Text & "'," &
                             "'" & .Cells("ColCustomerID").Value & "','" & .Cells("ColParty").Value & "'," & Val(.Cells("ColItemID").Value) & "," &
                             "'" & .Cells("ColItem").Value & "'," & Val(.Cells("ColCut").Value) & ", " &
                             " '" & .Cells("ColNug").Value & "','" & Val(.Cells("ColWeight").Value) & "','" & Val(.Cells("ColPartyRate").Value) & "'," &
                             " '" & .Cells("ColSallerRate").Value & "','" & .Cells("Colper").Value & "','" & Val(.Cells("ColBasicAmount").Value) & "'," &
                             "'" & Val(.Cells("ColCharges").Value) & "'," &
                             "'" & .Cells("ColTotal").Value & "','" & Val(.Cells("ColSallerPerItem").Value) & "','" & Val(.Cells("ColCommPer").Value) & "'," &
                             " '" & .Cells("ColCommAmt").Value & "','" & Val(.Cells("ColMPer").Value) & "','" & Val(.Cells("ColMAmt").Value) & "'," &
                             " '" & .Cells("ColRdfPer").Value & "','" & Val(.Cells("ColRdfamt").Value) & "','" & Val(.Cells("ColTarePer").Value) & "'," &
                             " '" & .Cells("ColTareAmt").Value & "','" & Val(.Cells("ColLabourPer").Value) & "','" & Val(.Cells("ColLabourAmt").Value) & "'," &
                             " '" & .Cells("ColCrateID").Value & "','" & .Cells("ColCrateMarka").Value & "','" & Val(.Cells("ColCrateQty").Value) & "'," &
                             "'" & .Cells("ColMaintainCrate").Value & "','" & Val(.Cells("CrateAccountID").Value) & "','" & .Cells("CrateAccountName").Value & "', '" & .Cells("AddWeight").Value & "'"
                End If
            End With
        Next
        If FastQuery = "" Then Exit Sub
        Sql = "insert into Transaction2(EntryDate,VoucherID,TransType,AccountID,AccountName,ItemID,ItemName,Cut,Nug,Weight, " _
                        & " Rate,SRate, Per,Amount,Charges,TotalAmount,SallerAmt,CommPer,CommAmt,MPer,MAmt,RdfPer,RdfAmt," _
                        & "Tare,TareAmt,Labour, LabourAmt,CrateID,Cratemarka,CrateQty, MaintainCrate,CrateAccountID,CrateAccountName,OnWeight) " & FastQuery & ""
        Try
            clsFun.ExecNonQuery(Sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        clsFun.CloseConnection()
    End Sub

    Private Sub ServerDg1Record()
        If OrgID = 0 Then Exit Sub
        Dim sql As String = String.Empty
        Dim cmd As SQLite.SQLiteCommand
        Dim SqliteEntryDate As String = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells("ColItem").Value <> "" Then
                    sql = sql & "insert into Transaction2(EntryDate,VoucherID,TransType,AccountID,AccountName,ItemID,ItemName,Cut,Nug,Weight, " _
                             & " Rate,SRate, Per,Amount,Charges,TotalAmount,SallerAmt,CommPer,CommAmt,MPer,MAmt,RdfPer,RdfAmt," _
                             & "Tare,TareAmt,Labour, LabourAmt,CrateID,Cratemarka,CrateQty, MaintainCrate,CrateAccountID,CrateAccountName,OnWeight,ServerTag,OrgID) " &
                             " values('" & SqliteEntryDate & "'," & Val(txtid.Text) & ", '" & Me.Text & "'," &
                             "'" & .Cells("ColCustomerID").Value & "','" & .Cells("ColParty").Value & "'," & Val(.Cells("ColItemID").Value) & "," &
                             "'" & .Cells("ColItem").Value & "'," & Val(.Cells("ColCut").Value) & ", " &
                             " '" & .Cells("ColNug").Value & "','" & Val(.Cells("ColWeight").Value) & "','" & Val(.Cells("ColPartyRate").Value) & "'," &
                             " '" & .Cells("ColSallerRate").Value & "','" & .Cells("Colper").Value & "','" & Val(.Cells("ColBasicAmount").Value) & "'," &
                             "'" & Val(.Cells("ColCharges").Value) & "'," &
                             " '" & .Cells("ColTotal").Value & "','" & Val(.Cells("ColSallerPerItem").Value) & "','" & Val(.Cells("ColCommPer").Value) & "'," &
                             " '" & .Cells("ColCommAmt").Value & "','" & Val(.Cells("ColMPer").Value) & "','" & Val(.Cells("ColMAmt").Value) & "'," &
                             " '" & .Cells("ColRdfPer").Value & "','" & Val(.Cells("ColRdfamt").Value) & "','" & Val(.Cells("ColTarePer").Value) & "'," &
                             " '" & .Cells("ColTareAmt").Value & "','" & Val(.Cells("ColLabourPer").Value) & "','" & Val(.Cells("ColLabourAmt").Value) & "'," &
                             " '" & .Cells("ColCrateID").Value & "','" & .Cells("ColCrateMarka").Value & "','" & Val(.Cells("ColCrateQty").Value) & "'," &
                             "'" & .Cells("ColMaintainCrate").Value & "','" & Val(.Cells("CrateAccountID").Value) & "','" & .Cells("CrateAccountName").Value & "', " &
                             "'" & .Cells("AddWeight").Value & "',1,'" & Val(OrgID) & "');"
                End If
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunserver.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then el.WriteToErrorLog(sql, Constants.compname, "Super Sale Record") : count = +1
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunserver.CloseConnection()
        End Try
        ClsFunserver.CloseConnection()
    End Sub

    Private Sub dg2Record()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("ColChargeName").Value <> "" Then
                    If row.Index <> 0 Then FastQuery = FastQuery & " UNION ALL SELECT "
                    FastQuery = FastQuery & Val(txtid.Text) & "," &
                        "'" & Val(.Cells("ColChargeID").Value) & "','" & .Cells("ColChargeName").Value & "','" & .Cells("ColOnValue").Value & "'," &
                        "'" & .Cells("ColCalculation").Value & "','" & .Cells("ColPlusMinus").Value & "','" & .Cells("ColChargeAmount").Value & "'"
                End If
            End With
        Next
        If FastQuery = "" Then Exit Sub
        sql = "insert into ChargesTrans(VoucherID, ChargesID, ChargeName, OnValue, Calculate, ChargeType, Amount) SELECT " & FastQuery & ""
        Try
            clsFun.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try

        clsFun.CloseConnection()
    End Sub

    Private Sub UpdateRecord()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        VchId = Val(txtid.Text)
        VchId2 = Val(txtPaymentID.Text)
        Dim count As Integer = 0
        dg1.ClearSelection()
        ' Dim cmd As SQLite.SQLiteCommand
        Dim ModifyByID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicleNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  SallerID=" & Val(txtAccountID.Text) & ", SallerName='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtSallerBasicTotal.Text & "', TotalAmount='" & txtsallerTotal.Text & "', RoundOFF='" & txtRoff.Text & "',TotalCharges='" & txtSallerCharges.Text & "', " _
                                & "N1='" & Val(txtPaymentID.Text) & "',N2='" & Val(txtAdvancePay.Text) & "',InvoiceID='" & Val(txtInvoiceID.Text) & "',ModifiedByID='" & Val(ModifyByID) & "', " _
                                & "ModifiedTime='" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "',T1= '" & txtDriverName.Text & "',T2= '" & txtDriverMobile.Text & "',T3= '" & txtRemark.Text & "',T4= '" & txtStateName.Text & "', " _
                                & "T5= '" & txtGSTN.Text & "',T6= '" & txtCustMobile.Text & "',T7= '" & txtBrokerName.Text & "',T8= '" & txtBrokerMob.Text & "',T9= '" & txtTransPort.Text & "',T10= '" & txtGrNo.Text & "' where ID =" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                el.WriteToErrorLog(sql, Constants.compname, "Updated")
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                              " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                UpdateCrate()
                clsFun.ExecNonQuery("DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                    "DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";" &
                                    "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";" &
                                    "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ";")
                Dg1Record() : dg2Record() : ServerTag = 1
                LegerSeller() : Ledgercharges() : LedgerCustomers() : LedgerCrate()
                ServerLedgerSeller() : ServerLedgerCustomers() : ServerchargesLedger() : ServerLedgerCrate()
                If Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) <> Val(txtid.Text) Then
                    clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) & "")
                    clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) & "")
                    If Val(txtAdvancePay.Text) > 0 Then
                        PaymentInfo()
                    End If
                End If
            End If
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Update") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            mskEntryDate.Focus()

            'mskEntryDate.Focus()
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
    Public Sub MultiUpdate()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        VchId = Val(txtid.Text)
        VchId2 = Val(txtPaymentID.Text)
        Dim count As Integer = 0
        dg1.ClearSelection()
        '  Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "' ,VehicleNo='" & txtVehicleNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                & "  SallerID=" & Val(txtAccountID.Text) & ", SallerName='" & txtAccount.Text & "', Nug='" & txtTotalNug.Text & "', kg='" & txttotalWeight.Text & "'," _
                                & " BasicAmount='" & txtSallerBasicTotal.Text & "', TotalAmount='" & txtsallerTotal.Text & "',TotalCharges='" & txtSallerCharges.Text & "', N1='" & Val(txtPaymentID.Text) & "',N2='" & Val(txtAdvancePay.Text) & "' where ID =" & VchId & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                el.WriteToErrorLog(sql, Constants.compname, "Updated")
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                UpdateCrate()
                clsFun.ExecNonQuery("DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                    "DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";" &
                                    "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & ";" &
                                    "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ";")
                Dg1Record() : dg2Record() : ServerTag = 1
                LegerSeller() : Ledgercharges() : LedgerCustomers() : LedgerCrate()
                ServerLedgerSeller() : ServerLedgerCustomers() : ServerchargesLedger() : ServerLedgerCrate()
                If Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) <> Val(txtid.Text) Then
                    clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) & "")
                    clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) & "")
                    If Val(txtAdvancePay.Text) > 0 Then
                        PaymentInfo()
                    End If
                End If

            End If
            cleartxt()
            cleartxtCharges()
            FootertextClear()
            dg1.Rows.Clear()
            Dg2.Rows.Clear()
            BtnDelete.Enabled = False
            BtnSave.Text = "&Save"
            mskEntryDate.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Private Sub LedgerCustomers()
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim Remark2 As String = String.Empty : Dim RemarkHindi As String = String.Empty
        Dim MandiTax As Decimal = 0.0 : Dim CommAmt As Decimal = 0.0
        Dim RDFAmt As Decimal = 0.0 : Dim TareAmt As Decimal = 0.0
        Dim Labouramt As Decimal = 0.0
        For Each row As DataGridViewRow In dg1.Rows
            ' Application.DoEvents()
            With row
                If .Cells("ColParty").Value <> "" Then
                    Remark2 = .Cells(1).Value & " Nug : " & .Cells(4).Value & " Weight : " & .Cells(5).Value & " On : " & .Cells(6).Value & " Per  /- " & .Cells(8).Value & ", Charges : " & .Cells(11).Value & vbCrLf
                    RemarkHindi = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & (.Cells(26).Value) & "") & ", नग : " & .Cells(4).Value & " वजन : " & .Cells(5).Value & " भाव : " & .Cells(6).Value & "  /- " & .Cells(8).Value & ", ख़र्चे : " & .Cells(11).Value & vbCrLf
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & .Cells(27).Value & "','" & .Cells(2).Value & "','" & .Cells(10).Value & "','D','" & Remark2 & "','" & .Cells(2).Value & "','" & RemarkHindi & "'"
                    CommAmt = Val(CommAmt) + Val(.Cells(14).Value)
                    MandiTax = Val(MandiTax) + Val(.Cells(16).Value)
                    RDFAmt = Val(RDFAmt) + Val(.Cells(18).Value)
                    TareAmt = Val(TareAmt) + Val(.Cells(20).Value)
                    Labouramt = Val(Labouramt) + Val(.Cells(22).Value)
                End If
            End With
        Next
        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 29 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "','" & Val(txtbasicTotal.Text) & "','C','" & Remark2 & "','','" & RemarkHindi & "'"
        If Val(CommAmt) > 0 Then ''Tax Tax Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 10 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "','" & Val(CommAmt) & "','C','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(MandiTax) > 0 Then ''Mandi Tax Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 30 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "','" & Val(MandiTax) & "','C','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(RDFAmt) > 0 Then ''RDF Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 39 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "','" & Val(RDFAmt) & "','C','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(TareAmt) > 0 Then ''Tare Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 4 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "','" & Val(TareAmt) & "','C','" & Remark2 & "','','" & RemarkHindi & "'"

        End If
        If Val(Labouramt) > 0 Then ''Labour Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 23 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "','" & Val(Labouramt) & "','C','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) > 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','C','" & Remark2 & "','','" & RemarkHindi & "'"
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','D','" & Remark2 & "','','" & RemarkHindi & "'"
            End If
        End If
        If FastQuery = "" Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub
    Private Sub ServerLedgerCustomers()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim Remark2 As String = String.Empty : Dim RemarkHindi As String = String.Empty
        Dim MandiTax As Decimal = 0.0 : Dim CommAmt As Decimal = 0.0
        Dim RDFAmt As Decimal = 0.0 : Dim TareAmt As Decimal = 0.0
        Dim Labouramt As Decimal = 0.0
        For Each row As DataGridViewRow In dg1.Rows
            ' Application.DoEvents()
            With row
                If .Cells("ColParty").Value <> "" Then
                    Remark2 = .Cells(1).Value & " Nug : " & .Cells(4).Value & " Weight : " & .Cells(5).Value & " On : " & .Cells(6).Value & " Per  /- " & .Cells(8).Value & ", Charges : " & .Cells(11).Value & vbCrLf
                    RemarkHindi = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & (.Cells(26).Value) & "") & ", नग : " & .Cells(4).Value & " वजन : " & .Cells(5).Value & " भाव : " & .Cells(6).Value & "  /- " & .Cells(8).Value & ", ख़र्चे : " & .Cells(11).Value & vbCrLf
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & .Cells(27).Value & "','" & .Cells(2).Value & "','" & .Cells(10).Value & "','D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','" & .Cells(2).Value & "','" & RemarkHindi & "'"
                    CommAmt = Val(CommAmt) + Val(.Cells(14).Value)
                    MandiTax = Val(MandiTax) + Val(.Cells(16).Value)
                    RDFAmt = Val(RDFAmt) + Val(.Cells(18).Value)
                    TareAmt = Val(TareAmt) + Val(.Cells(20).Value)
                    Labouramt = Val(Labouramt) + Val(.Cells(22).Value)
                End If
            End With
        Next
        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 29 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "','" & Val(txtbasicTotal.Text) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"
        If Val(CommAmt) > 0 Then ''Tax Tax Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 10 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "','" & Val(CommAmt) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(MandiTax) > 0 Then ''Mandi Tax Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 30 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "','" & Val(MandiTax) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(RDFAmt) > 0 Then ''RDF Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 39 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "','" & Val(RDFAmt) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(TareAmt) > 0 Then ''Tare Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 4 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "','" & Val(TareAmt) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"

        End If
        If Val(Labouramt) > 0 Then ''Labour Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 23 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "','" & Val(Labouramt) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"
        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) > 0 Then
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"
            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "','" & 42 & "','" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "','D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "','','" & RemarkHindi & "'"
            End If
        End If
        If FastQuery = "" Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub
    Private Sub Ledgercharges()
        Dim RemarkHindi As String = String.Empty
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("ColChargeName").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(6).Value & "")
                    Dim ssql As String
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(6).Value & "")
                    Dim AccName As String = ssql
                    Dim Remark As String = "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " , Total Amount :" & txtTotalNetAmount.Text
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(6).Value & "'") = "Party Cost" Then
                        If .Cells(4).Value = "+" Then

                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "D", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " , Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "C", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " , Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(6).Value & "'") = "Our Cost" Then ''our coast
                        If .Cells(4).Value = "+" Then
                            '       clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "D", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " ,Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'D','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "C", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " ,Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'C','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub

    Private Sub ServerchargesLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim RemarkHindi As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("ColChargeName").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(6).Value & "")
                    Dim ssql As String
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(6).Value & "")
                    Dim AccName As String = ssql
                    Dim Remark As String = "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " , Total Amount :" & txtTotalNetAmount.Text
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(6).Value & "'") = "Party Cost" Then
                        If .Cells(4).Value = "+" Then

                            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "D", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " , Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "C", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " , Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells(6).Value & "'") = "Our Cost" Then ''our coast
                        If .Cells(4).Value = "+" Then
                            '       clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "D", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " ,Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        Else
                            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, AcID, AccName, .Cells(5).Value, "C", "Voucher No. :" & txtVoucherNo.Text & " , Vehicle No. :" & txtVehicleNo.Text & " , Account Name :" & txtAccount.Text & " , Basic Amount :" & txtbasicTotal.Text & " , Total Charges :" & txttotalCharges.Text & " ,Total Amount :" & txtTotalNetAmount.Text)
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(5).Value) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark & "','" & AccName & "','" & RemarkHindi & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub

    Private Sub Delete()
        Dim RemoveSale As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
        If RemoveSale <> "Y" Then MsgBox("You Don't Have Rights to Delete Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : cleartxt() : cleartxtCharges() : FootertextClear() : Exit Sub
        Try
            If MessageBox.Show("Are You Sure Want to Delete Complete Bill?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) & "")
                clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(clsFun.ExecScalarStr("Select PaymentID From Vouchers Where ID=" & Val(txtid.Text) & "")) & "")
                If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "; " &
                                       "DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "; " &
                                       "DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                       "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "; " &
                                       "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & "") > 0 Then
                    ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                               "Delete From  CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")

                    ServerTag = 0 : ServerLedgerSeller() : ServerLedgerCustomers() : ServerchargesLedger() : ServerLedgerCrate()
                End If
                MsgBox("Record Deleted Successfully.", MsgBoxStyle.Information, "Deleted")
                mskEntryDate.Focus() : cleartxt()
                cleartxtCharges() : FootertextClear()
                dg1.Rows.Clear() : Dg2.Rows.Clear()
                BtnDelete.Enabled = False
                BtnSave.Text = "&Save"
                If Application.OpenForms().OfType(Of Super_Sale_Register).Any = True Then Super_Sale_Register.btnShow.PerformClick()
                If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
                If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
                If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub retrive2()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        dt = clsFun.ExecDataTable(" Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.sallerName, Vouchers.VehicleNo,Transaction2.OnWeight, " _
                               & " Transaction2.ItemName, Transaction2.Cut, sum(Transaction2.Nug) as Transnug, Round(sum(Transaction2.Weight),5) as Weight, Transaction2.SRate," _
                               & " Transaction2.Per, sum(Transaction2.SallerAmt) as SallerAmt, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount," _
                               & " Vouchers.totalcharges, Items.OtherName as OtherItemName, Accounts.OtherName as OtherName,Vouchers.T1,Vouchers.T2,Vouchers.T3,Vouchers.T4, " _
                               & " Vouchers.T5,Vouchers.T6,Vouchers.T7,Vouchers.T8,Vouchers.T9,Vouchers.T10 FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID) " _
                               & " INNER JOIN Items ON Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID Where (Vouchers.ID=" & Val(txtid.Text) & ") Group by Transaction2.ItemName,Transaction2.SRate,Transaction2.Per " _
                               & " order by Vouchers.EntryDate,Vouchers.billNo, Transaction2.ItemName,Transaction2.SRate Desc")
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                TempRowColumn()
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("SallerName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Cut").ToString()
                    .Cells(8).Value = .Cells(8).Value & dt.Rows(i)("Transnug").ToString()
                    .Cells(9).Value = .Cells(9).Value & dt.Rows(i)("Weight").ToString()
                    .Cells(10).Value = .Cells(10).Value & dt.Rows(i)("SRate").ToString()
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = .Cells(12).Value & dt.Rows(i)("SallerAmt").ToString()
                    .Cells(18).Value = .Cells(18).Value & dt.Rows(i)("Nug").ToString()
                    .Cells(19).Value = .Cells(19).Value & dt.Rows(i)("Kg").ToString()
                    .Cells(20).Value = .Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()
                    .Cells(21).Value = .Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()
                    .Cells(22).Value = .Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherItemName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & dt.Rows(i)("OnWeight").ToString()
                    .Cells(27).Value = dt.Rows(i)("T1").ToString()
                    .Cells(28).Value = dt.Rows(i)("T2").ToString()
                    .Cells(29).Value = dt.Rows(i)("T3").ToString()
                    .Cells(30).Value = dt.Rows(i)("T4").ToString()
                    .Cells(31).Value = dt.Rows(i)("T5").ToString()
                    .Cells(32).Value = dt.Rows(i)("T6").ToString()
                    .Cells(33).Value = dt.Rows(i)("T7").ToString()
                    .Cells(34).Value = dt.Rows(i)("T8").ToString()
                    .Cells(35).Value = dt.Rows(i)("T9").ToString()
                    .Cells(36).Value = dt.Rows(i)("T10").ToString()
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
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
        dt.Clear() : dt1.Clear()
    End Sub
    Sub retrive3()
        'MsgBox(txtid.Text)
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        dt = clsFun.ExecDataTable("  Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.sallerName, Vouchers.VehicleNo,Transaction2.OnWeight, Transaction2.ItemName, Transaction2.Cut, sum(Transaction2.Nug) as Transnug, " _
                               & "  Round(sum(Transaction2.Weight),2)  as Weight , Round(avg(Transaction2.SRate),2) as SRate, Transaction2.Per, sum(Transaction2.SallerAmt) as SallerAmt , Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount," _
                               & " Vouchers.totalcharges, Items.OtherName as OtherItemName, Accounts.OtherName FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)  INNER JOIN Items ON" _
                               & " Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID " _
                               & "  Where (Vouchers.ID=" & txtid.Text & ")Group by  Transaction2.ItemName,Transaction2.SRate")
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                TempRowColumn()
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("SallerName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Cut").ToString()
                    .Cells(8).Value = Format(Val(.Cells(8).Value & dt.Rows(i)("Transnug").ToString()), "0.00")
                    .Cells(9).Value = Format(Val(.Cells(9).Value & dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = Format(Val(.Cells(10).Value & dt.Rows(i)("SRate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = Format(Val(.Cells(12).Value & dt.Rows(i)("SallerAmt").ToString()), "0.00")
                    .Cells(18).Value = Format(Val(.Cells(18).Value & dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = Format(Val(.Cells(19).Value & dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = Format(Val(.Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = Format(Val(.Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = Format(Val(.Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherItemName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & dt.Rows(i)("OnWeight").ToString()
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
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
        dt.Clear() : dt1.Clear()
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        'Application.DoEvents()
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnSave.Image = My.Resources.Edit
        BtnSave.BackColor = Color.Coral
        BtnDelete.Enabled = True
        'Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Transaction2 WHERE transtype = 'Super Sale' and   EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' Order By ID Desc")
        'TotalPages = Math.Ceiling(recordsCount / RowCount)
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers v INNER JOIN Accounts a1 ON a1.ID = v.sallerID where v.id=" & Val(id)
        Dim sql As String = "Select * from Transaction2 t INNER JOIN Accounts a ON a.ID = t.AccountID INNER JOIN Items i ON i.ID = t.ItemID where t.VoucherID=" & Val(id)
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & Val(id)
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds, "b")
        ad2.Fill(ds, "c")
        txtid.Text = Val(id)
        If ds.Tables("a").Rows.Count > 0 Then
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName1").ToString()
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtSallerBasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txtSallerCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txtRoff.Text = Format(Val(ds.Tables("a").Rows(0)("RoundOff").ToString()), "0.00")
            txtsallerTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) From Transaction2 Where VoucherID='" & Val(ds.Tables("a").Rows(0)("ID").ToString()) & "'")), "0.00")
            txttotalCharges.Text = Format(Val(clsFun.ExecScalarStr("Select sum(Charges) From Transaction2 Where VoucherID='" & Val(ds.Tables("a").Rows(0)("ID").ToString()) & "'")), "0.00")
            txtTotalNetAmount.Text = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) From Transaction2 Where VoucherID='" & Val(ds.Tables("a").Rows(0)("ID").ToString()) & "'")), "0.00")
            txtroundoff.Text = Format(Val(txtTotalNetAmount.Text) - (Val(txtbasicTotal.Text) + Val(txttotalCharges.Text)), "0.00")
            txtPaymentID.Text = ds.Tables("a").Rows(0)("N1").ToString()
            txtAdvancePay.Text = ds.Tables("a").Rows(0)("N2").ToString()
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
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
            ' If ds.Tables("a").Rows(0)("IsCanceled").ToString() = "Y" Then ckCancel.Checked = True Else ckCancel.Checked = False
        End If
        If ds.Tables("b").Rows.Count > 0 Then
            dg1.Rows.Clear()
            'With dg1
            If ds.Tables("b").Rows.Count > 9 Then dg1.Columns(10).Width = 109 Else dg1.Columns(10).Width = 129
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                ' Application.DoEvents()
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells("ColCustomerID").Value = ds.Tables("b").Rows(i)("AccountID").ToString()
                    .Cells("ColParty").Value = ds.Tables("b").Rows(i)("AccountName1").ToString()
                    .Cells("ColItemID").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                    .Cells("ColItem").Value = ds.Tables("b").Rows(i)("ItemName1").ToString()
                    If Val(ds.Tables("b").Rows(i)("Cut").ToString()) > 0 Then
                        .Cells("ColCut").Value = ds.Tables("b").Rows(i)("Cut").ToString()
                    End If
                    .Cells("ColNug").Value = Format(Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                    .Cells("ColWeight").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                    .Cells("ColPartyRate").Value = Format(Val(ds.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                    .Cells("ColSallerRate").Value = Format(Val(ds.Tables("b").Rows(i)("SRate").ToString()), "0.00")
                    .Cells("ColPer").Value = ds.Tables("b").Rows(i)("Per").ToString()
                    .Cells("ColBasicAmount").Value = Format(Val(ds.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                    .Cells("ColCharges").Value = Format(Val(ds.Tables("b").Rows(i)("Charges").ToString()), "0.00")
                    .Cells("ColTotal").Value = Format(Val(ds.Tables("b").Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells("ColSallerPerItem").Value = ds.Tables("b").Rows(i)("SallerAmt").ToString()
                    .Cells("ColCommPer").Value = Format(Val(ds.Tables("b").Rows(i)("CommPer").ToString()), "0.00")
                    .Cells("ColCommAmt").Value = Format(Val(ds.Tables("b").Rows(i)("CommAmt").ToString()), "0.00")
                    .Cells("ColMPer").Value = Format(Val(ds.Tables("b").Rows(i)("MPer").ToString()), "0.00")
                    .Cells("ColMAmt").Value = Format(Val(ds.Tables("b").Rows(i)("MAmt").ToString()), "0.00")
                    .Cells("ColRdfPer").Value = Format(Val(ds.Tables("b").Rows(i)("RdfPer").ToString()), "0.00")
                    .Cells("ColRdfAmt").Value = Format(Val(ds.Tables("b").Rows(i)("RdfAmt").ToString()), "0.00")
                    .Cells("ColTarePer").Value = Format(Val(ds.Tables("b").Rows(i)("Tare").ToString()), "0.00")
                    .Cells("ColTareAmt").Value = Format(Val(ds.Tables("b").Rows(i)("TareAmt").ToString()), "0.00")
                    .Cells("ColLabourPer").Value = Format(Val(ds.Tables("b").Rows(i)("Labour").ToString()), "0.00")
                    .Cells("ColLabourAmt").Value = Format(Val(ds.Tables("b").Rows(i)("LabourAmt").ToString()), "0.00")
                    .Cells("ColCrateID").Value = ds.Tables("b").Rows(i)("CrateID").ToString()
                    .Cells("ColCrateMarka").Value = ds.Tables("b").Rows(i)("Cratemarka").ToString()
                    .Cells("ColCrateQty").Value = ds.Tables("b").Rows(i)("CrateQty").ToString()
                    .Cells("ColMaintainCrate").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                    .Cells("CrateAccountID").Value = ds.Tables("b").Rows(i)("CrateAccountID").ToString()
                    .Cells("CrateAccountName").Value = ds.Tables("b").Rows(i)("CrateAccountName").ToString()
                    .Cells("AddWeight").Value = ds.Tables("b").Rows(i)("Onweight").ToString()
                End With

            Next
            ' End With
        End If
        If ds.Tables("C").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("ColChargeName").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    If Val(ds.Tables("c").Rows(i)("OnValue").ToString()) > 0 Then
                        .Rows(i).Cells("ColOnValue").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    End If
                    Dg2.Rows(i).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Rows(i).Cells("ColCalculation").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("ColPlusMinus").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("ColChargeAmount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ColChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        Offset = clsFun.ExecScalarInt("SELECT COUNT(*) AS RowPosition FROM Vouchers WHERE transtype ='Super Sale' AND ID < " & Val(txtid.Text) & " ORDER BY ID DESC")
        dg1.ClearSelection() : Dg2.ClearSelection()
        AcBal() : ClosingBal() : calc()
    End Sub



    Public Sub FillWithNevigation()
        FootertextClear()
        Dim id As Integer
        If BtnSave.Text = "&Save" And dg1.RowCount > 0 Then MsgBox("Save Transaction First...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnSave.Image = My.Resources.Edit
        BtnSave.BackColor = Color.Coral
        BtnDelete.Enabled = True
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Super Sale'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from  Vouchers WHERE transtype = 'Super Sale'  Order By ID LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString() + ""
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccount.Text = ds.Tables("a").Rows(0)("Sallername").ToString()
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtSallerBasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txtSallerCharges.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txtRoff.Text = Format(Val(ds.Tables("a").Rows(0)("RoundOff").ToString()), "0.00")
            txtsallerTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(clsFun.ExecScalarStr("Select sum(Amount) From Transaction2 Where VoucherID='" & Val(ds.Tables("a").Rows(0)("ID").ToString()) & "'")), "0.00")
            txttotalCharges.Text = Format(Val(clsFun.ExecScalarStr("Select sum(Charges) From Transaction2 Where VoucherID='" & Val(ds.Tables("a").Rows(0)("ID").ToString()) & "'")), "0.00")
            txtTotalNetAmount.Text = Format(Val(clsFun.ExecScalarStr("Select sum(TotalAmount) From Transaction2 Where VoucherID='" & Val(ds.Tables("a").Rows(0)("ID").ToString()) & "'")), "0.00")
            txtroundoff.Text = Format(Val(txtTotalNetAmount.Text) - (Val(txtbasicTotal.Text) + Val(txttotalCharges.Text)), "0.00")
            txtPaymentID.Text = ds.Tables("a").Rows(0)("N1").ToString()
            txtAdvancePay.Text = ds.Tables("a").Rows(0)("N2").ToString()
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
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
            '   If ds.Tables("a").Rows(0)("IsCanceled").ToString() = "Y" Then ckCancel.Checked = True Else ckCancel.Checked = False
            id = Val(txtid.Text)
        End If
        Dim sql As String = "Select * from Transaction2 t INNER JOIN Accounts a ON a.ID = t.AccountID INNER JOIN Items i ON i.ID = t.ItemID where t.VoucherID=" & Val(id)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        ad1.Fill(ds, "b")
        If ds.Tables("b").Rows.Count > 0 Then
            dg1.Rows.Clear()
            'With dg1
            If ds.Tables("b").Rows.Count > 9 Then dg1.Columns(10).Width = 109 Else dg1.Columns(10).Width = 129
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                dg1.Rows.Add()
                With dg1.Rows(i)
                    '  Threading.Thread.Sleep(1)
                    '    Application.DoEvents()
                    .Cells("ColCustomerID").Value = ds.Tables("b").Rows(i)("AccountID").ToString()
                    .Cells("ColParty").Value = ds.Tables("b").Rows(i)("AccountName1").ToString()
                    .Cells("ColItemID").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                    .Cells("ColItem").Value = ds.Tables("b").Rows(i)("ItemName1").ToString()
                    If Val(ds.Tables("b").Rows(i)("Cut").ToString()) > 0 Then
                        .Cells("ColCut").Value = ds.Tables("b").Rows(i)("Cut").ToString()
                    End If
                    .Cells("ColNug").Value = Format(Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                    .Cells("ColWeight").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                    .Cells("ColPartyRate").Value = Format(Val(ds.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                    .Cells("ColSallerRate").Value = Format(Val(ds.Tables("b").Rows(i)("SRate").ToString()), "0.00")
                    .Cells("ColPer").Value = ds.Tables("b").Rows(i)("Per").ToString()
                    .Cells("ColBasicAmount").Value = Format(Val(ds.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                    .Cells("ColCharges").Value = Format(Val(ds.Tables("b").Rows(i)("Charges").ToString()), "0.00")
                    .Cells("ColTotal").Value = Format(Val(ds.Tables("b").Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells("ColSallerPerItem").Value = ds.Tables("b").Rows(i)("SallerAmt").ToString()
                    .Cells("ColCommPer").Value = Format(Val(ds.Tables("b").Rows(i)("CommPer").ToString()), "0.00")
                    .Cells("ColCommAmt").Value = Format(Val(ds.Tables("b").Rows(i)("CommAmt").ToString()), "0.00")
                    .Cells("ColMPer").Value = Format(Val(ds.Tables("b").Rows(i)("MPer").ToString()), "0.00")
                    .Cells("ColMAmt").Value = Format(Val(ds.Tables("b").Rows(i)("MAmt").ToString()), "0.00")
                    .Cells("ColRdfPer").Value = Format(Val(ds.Tables("b").Rows(i)("RdfPer").ToString()), "0.00")
                    .Cells("ColRdfAmt").Value = Format(Val(ds.Tables("b").Rows(i)("RdfAmt").ToString()), "0.00")
                    .Cells("ColTarePer").Value = Format(Val(ds.Tables("b").Rows(i)("Tare").ToString()), "0.00")
                    .Cells("ColTareAmt").Value = Format(Val(ds.Tables("b").Rows(i)("TareAmt").ToString()), "0.00")
                    .Cells("ColLabourPer").Value = Format(Val(ds.Tables("b").Rows(i)("Labour").ToString()), "0.00")
                    .Cells("ColLabourAmt").Value = Format(Val(ds.Tables("b").Rows(i)("LabourAmt").ToString()), "0.00")
                    .Cells("ColCrateID").Value = ds.Tables("b").Rows(i)("CrateID").ToString()
                    .Cells("ColCrateMarka").Value = ds.Tables("b").Rows(i)("Cratemarka").ToString()
                    .Cells("ColCrateQty").Value = ds.Tables("b").Rows(i)("CrateQty").ToString()
                    .Cells("ColMaintainCrate").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                    .Cells("CrateAccountID").Value = ds.Tables("b").Rows(i)("CrateAccountID").ToString()
                    .Cells("CrateAccountName").Value = ds.Tables("b").Rows(i)("CrateAccountName").ToString()
                    .Cells("AddWeight").Value = ds.Tables("b").Rows(i)("Onweight").ToString()
                End With

            Next
            ' End With
        End If
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & Val(id)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        ad2.Fill(ds, "c")
        If ds.Tables("C").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("ColChargeName").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    If Val(ds.Tables("c").Rows(i)("OnValue").ToString()) > 0 Then
                        .Rows(i).Cells("ColOnValue").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    End If
                    .Rows(i).Cells("ColCalculation").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("ColPlusMinus").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("ColChargeAmount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ColChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        dg1.ClearSelection() : Dg2.ClearSelection()
        AcBal() : ClosingBal()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            txtItem.Text = dg1.SelectedRows(0).Cells(1).Value
            txtCustomer.Text = dg1.SelectedRows(0).Cells(2).Value
            txtCut.Text = dg1.SelectedRows(0).Cells(3).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(4).Value
            txtWeight.Text = dg1.SelectedRows(0).Cells(5).Value
            txtCustomerRate.Text = dg1.SelectedRows(0).Cells(6).Value
            txtSallerRate.Text = dg1.SelectedRows(0).Cells(7).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(8).Value
            txtBasicCustomer.Text = dg1.SelectedRows(0).Cells(9).Value
            txtTotAmount.Text = dg1.SelectedRows(0).Cells(10).Value
            lbltotCharges.Text = dg1.SelectedRows(0).Cells(11).Value
            txtSallerAmout.Text = dg1.SelectedRows(0).Cells(12).Value
            txtComPer.Text = dg1.SelectedRows(0).Cells(13).Value
            txtComAmt.Text = dg1.SelectedRows(0).Cells(14).Value
            txtMPer.Text = dg1.SelectedRows(0).Cells(15).Value
            txtMAmt.Text = dg1.SelectedRows(0).Cells(16).Value
            txtRdfPer.Text = dg1.SelectedRows(0).Cells(17).Value
            txtRdfAmt.Text = dg1.SelectedRows(0).Cells(18).Value
            txtTare.Text = dg1.SelectedRows(0).Cells(19).Value
            txtTareAmt.Text = dg1.SelectedRows(0).Cells(20).Value
            txtLabour.Text = dg1.SelectedRows(0).Cells(21).Value
            txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(22).Value
            cbCrateMarka.Text = dg1.SelectedRows(0).Cells(23).Value
            cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(24).Value)
            txtCrateQty.Text = dg1.SelectedRows(0).Cells(25).Value
            txtItemID.Text = dg1.SelectedRows(0).Cells(26).Value
            txtcustomerID.Text = dg1.SelectedRows(0).Cells(27).Value
            'cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(29).Value)
            'cbAccountName.Text = dg1.SelectedRows(0).Cells(30).Value
            txtAddWeight.Text = dg1.SelectedRows(0).Cells(31).Value
            txtItem.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0)) : calc()
            End If
        End If
        If e.KeyCode = Keys.Up Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Index) = 0 Then txtItem.Focus()
            dg1.ClearSelection()
        End If
        If e.KeyCode = Keys.Down Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Index) = Val(dg1.Rows.Count - 1) Then txtItem.Focus() : dg1.ClearSelection() : Exit Sub
            e.Handled = False
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItem.Text = dg1.SelectedRows(0).Cells(1).Value
        txtCustomer.Text = dg1.SelectedRows(0).Cells(2).Value
        txtCut.Text = dg1.SelectedRows(0).Cells(3).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(4).Value
        txtWeight.Text = dg1.SelectedRows(0).Cells(5).Value
        txtCustomerRate.Text = dg1.SelectedRows(0).Cells(6).Value
        txtSallerRate.Text = dg1.SelectedRows(0).Cells(7).Value
        Cbper.Text = dg1.SelectedRows(0).Cells(8).Value
        txtBasicCustomer.Text = dg1.SelectedRows(0).Cells(9).Value
        txtTotAmount.Text = dg1.SelectedRows(0).Cells(10).Value
        lbltotCharges.Text = dg1.SelectedRows(0).Cells(11).Value
        txtSallerAmout.Text = dg1.SelectedRows(0).Cells(12).Value
        txtComPer.Text = dg1.SelectedRows(0).Cells(13).Value
        txtComAmt.Text = dg1.SelectedRows(0).Cells(14).Value
        txtMPer.Text = dg1.SelectedRows(0).Cells(15).Value
        txtMAmt.Text = dg1.SelectedRows(0).Cells(16).Value
        txtRdfPer.Text = dg1.SelectedRows(0).Cells(17).Value
        txtRdfAmt.Text = dg1.SelectedRows(0).Cells(18).Value
        txtTare.Text = dg1.SelectedRows(0).Cells(19).Value
        txtTareAmt.Text = dg1.SelectedRows(0).Cells(20).Value
        txtLabour.Text = dg1.SelectedRows(0).Cells(21).Value
        txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(22).Value
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(23).Value
        cbCrateMarka.SelectedValue = Val(dg1.SelectedRows(0).Cells(24).Value)
        txtCrateQty.Text = dg1.SelectedRows(0).Cells(25).Value
        txtItemID.Text = dg1.SelectedRows(0).Cells(26).Value
        txtcustomerID.Text = dg1.SelectedRows(0).Cells(27).Value
        'cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(29).Value)
        'cbAccountName.Text = dg1.SelectedRows(0).Cells(30).Value
        txtAddWeight.Text = dg1.SelectedRows(0).Cells(31).Value
        'txtItem.Focus()
    End Sub
    Private Sub txtchargesAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtchargesAmount.KeyDown
        If txtPlusMinus.Text = "" Then txtCharges.Focus()
        If e.KeyCode = Keys.Enter Then
            If Dg2.SelectedRows.Count = 1 Then
                Dg2.SelectedRows(0).Cells(1).Value = txtCharges.Text
                Dg2.SelectedRows(0).Cells(2).Value = txtOnValue.Text
                Dg2.SelectedRows(0).Cells(3).Value = txtCalculatePer.Text
                Dg2.SelectedRows(0).Cells(4).Value = txtPlusMinus.Text
                Dg2.SelectedRows(0).Cells(5).Value = txtchargesAmount.Text
                Dg2.SelectedRows(0).Cells(6).Value = txtChargeID.Text
                txtCharges.Focus()
                Dg2.ClearSelection()
                cleartxtCharges()
            Else
                Dg2.Rows.Add("", txtCharges.Text, txtOnValue.Text, txtCalculatePer.Text, txtPlusMinus.Text, txtchargesAmount.Text, txtChargeID.Text)
                cleartxtCharges()
                txtCharges.Focus()
                Dg2.ClearSelection()
            End If
        End If
        calc()
    End Sub

    Private Sub Dg2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles Dg2.CellDoubleClick
        If Dg2.SelectedRows.Count = 0 Then Exit Sub
        txtCharges.Text = Dg2.SelectedRows(0).Cells(1).Value
        txtOnValue.Text = Dg2.SelectedRows(0).Cells(2).Value
        txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(3).Value
        txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(4).Value
        txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(5).Value
        txtChargeID.Text = Dg2.SelectedRows(0).Cells(6).Value
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
    End Sub

    Private Sub Dg2_KeyDown(sender As Object, e As KeyEventArgs) Handles Dg2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            txtCharges.Text = Dg2.SelectedRows(0).Cells(1).Value
            txtOnValue.Text = Dg2.SelectedRows(0).Cells(2).Value
            txtCalculatePer.Text = Dg2.SelectedRows(0).Cells(3).Value
            txtPlusMinus.Text = Dg2.SelectedRows(0).Cells(4).Value
            txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(5).Value
            txtChargeID.Text = Dg2.SelectedRows(0).Cells(6).Value
            'txtchargesAmount.Text = Dg2.SelectedRows(0).Cells(6).Value
            CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
            txtCharges.Focus()
        End If
        e.SuppressKeyPress = True
        If e.KeyCode = Keys.Delete Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to Remove Charges", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
                Dg2.ClearSelection() : txtCharges.Focus()
                If String.IsNullOrEmpty(txtbasicTotal.Text) OrElse String.IsNullOrEmpty(txttotalCharges.Text) Then Exit Sub
                txtsallerTotal.Text = Format(CDbl(txtSallerBasicTotal.Text) + CDbl(txtSallerCharges.Text), "0.00")
                'ClearDetails()
            End If
            calc()
        End If

    End Sub

    Private Sub LedgerCrate()
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = mskEntryDate.Text
        SqliteEntryDate = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If Val(.Cells(24).Value) > 0 Then  ''Party Account
                    If .Cells(28).Value = "Y" Then ''Party Account
                        If Val(.Cells(25).Value) <> 0 Then
                            If Val(.Cells(27).Value) = 7 Then
                                If Val(.Cells(29).Value) > 0 Then
                                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(29).Value) & ",'" & .Cells(30).Value & "','Crate Out'," & Val(.Cells(24).Value) & ",'" & .Cells(23).Value & "','" & .Cells(25).Value & "', '','','',''"
                                End If
                            Else
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(27).Value) & ",'" & .Cells(2).Value & "','Crate Out'," & Val(.Cells(24).Value) & ",'" & .Cells(23).Value & "','" & .Cells(25).Value & "',  '','','',''"
                            End If
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastCrateLedger(FastQuery)
    End Sub
    Private Sub ServerLedgerCrate()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = mskEntryDate.Text
        SqliteEntryDate = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If Val(.Cells(24).Value) > 0 Then  ''Party Account
                    If .Cells(28).Value = "Y" Then ''Party Account
                        '  If cbCrateMarka.SelectedValue > 0 Then ''Party Account
                        If Val(.Cells(25).Value) > 0 Then
                            If Val(.Cells(27).Value) = 7 Then
                                If Val(.Cells(29).Value) > 0 Then
                                    ' clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, SqliteEntryDate, Me.Text, .Cells(29).Value, .Cells(30).Value, "Crate Out", Val(.Cells(24).Value), .Cells(23).Value, .Cells(25).Value, "", "", "", "")
                                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(29).Value) & ",'" & .Cells(30).Value & "','Crate Out'," & Val(.Cells(24).Value) & ",'" & .Cells(23).Value & "','" & .Cells(25).Value & "', '','','','','" & Val(ServerTag) & "','" & Val(OrgID) & "'"

                                End If
                            Else
                                'clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, SqliteEntryDate, Me.Text, .Cells(27).Value, .Cells(2).Value, "Crate Out", .Cells(24).Value, .Cells(23).Value, .Cells(25).Value, "", "", "", "")
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(27).Value) & ",'" & .Cells(2).Value & "','Crate Out'," & Val(.Cells(24).Value) & ",'" & .Cells(23).Value & "','" & .Cells(25).Value & "',  '','','','','" & Val(ServerTag) & "','" & Val(OrgID) & "'"
                            End If
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(FastQuery)
    End Sub
    Private Sub eventLog()
        ' setup for a divide by zero error
        Dim int1 As Integer = 10
        Dim int2 As Integer = 0
        Dim intResult As Integer
        Try
            ' trip the divide by zero error
            intResult = int1 / int2
        Catch ex As Exception
            Dim el As New Aadhat.ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")

            ' MsgBox("Error logged.")

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

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        TempRowColumn() : mskEntryDate.Focus()
        If dg1.RowCount = 0 Then MsgBox("There is no record to Save / Update...", vbOKOnly, "Empty") : Exit Sub
        If BtnSave.Text = "&Save" Then
            Dim addSale As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
            If addSale <> "Y" Then MsgBox("You Don't Have Rights to Add Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : cleartxt() : cleartxtCharges() : FootertextClear() : Exit Sub
            ButtonControl() : Save() : ButtonControl()
        Else
            Dim ModifySale As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
            If ModifySale <> "Y" Then MsgBox("You Don't Have Rights to Modify Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : cleartxt() : cleartxtCharges() : FootertextClear() : Exit Sub
            ButtonControl() : UpdateRecord() : ButtonControl()
        End If
        If clsFun.ExecScalarStr("Select AskMannual from Controls ") = "Yes" Then
            Dim res = MessageBox.Show("Do you want to Print Supplier Bill...", "Print Invoice...", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = Windows.Forms.DialogResult.Yes Then
                btnPrint.Enabled = True : btnPrint.PerformClick()
            End If
        End If
        'If Application.OpenForms().OfType(Of Super_Sale_Register).Any = True Then Super_Sale_Register.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        BtnDelete.Enabled = False : BtnSave.Text = "&Save"
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ButtonControl() : Delete() : ButtonControl()
    End Sub

    Private Sub Canceled()
        Try
            If MessageBox.Show("Are You Sure Want to Cancel  Bill?", "Cancel Bill", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
                clsFun.ExecScalarStr("Update Vouchers Set IsCanceled='Y' Where ID =" & Val(txtid.Text) & "")
                clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "")
                MsgBox("Record Canceled Successfully.", vbExclamation + vbOKOnly, "Deleted")
                mskEntryDate.Focus() : cleartxt() : cleartxtCharges() : FootertextClear()
                dg1.Rows.Clear() : Dg2.Rows.Clear()
                BtnDelete.Enabled = False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub PrintRecord()
        Dim FastQuery As String = String.Empty
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = String.Empty
        Dim lastPayment = clsFun.ExecScalarStr("Select  BasicAmount FROM Vouchers where TransType='Payment' and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'  and Accountid=" & Val(txtAccountID.Text) & " ORDER BY Vouchers.Entrydate DESC limit 1 ;")

        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then

                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                                "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & Format(Val(.Cells(9).Value), "0.00") & "','" & Format(Val(.Cells(10).Value), "0.00") & "','" & .Cells(11).Value & "','" & Format(Val(.Cells(12).Value), "0.00") & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & Format(Val(.Cells(19).Value), "0.00") & "','" & Format(Val(.Cells(20).Value), "0.00") & "', " &
                                "'" & Format(Val(.Cells(21).Value), "0.00") & "','" & Format(Val(.Cells(22).Value), "0.00") & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "', " &
                                "'" & lblInword.Text & "','" & IIf(.Cells(26).Value = "", "", "(" & .Cells(26).Value & ")") & "','" & opbal & "'," &
                                "'" & ClBal & "'," & Format(Val(lastPayment), "0.00") & ""
                End If
            End With
        Next
        If FastQuery = "" Then Exit Sub
        sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " &
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                        " P21,P22,P23,P24,P25,P26)" & FastQuery & ""
        Try
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub

    Private Sub CustoemerRowColums()
        dgCustomer.ColumnCount = 3
        dgCustomer.Columns(0).Name = "ID" : dgCustomer.Columns(0).Visible = False
        dgCustomer.Columns(1).Name = "Account Name" : dgCustomer.Columns(1).Width = 332
        dgCustomer.Columns(2).Name = "City" : dgCustomer.Columns(2).Width = 150
        dgCustomer.Visible = True : dgCustomer.BringToFront()
        RetriveCustomer()
    End Sub
    Private Sub RetriveCustomer(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        If ckShowSupplier.Checked = True Then
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16,17)  or UnderGroupID in (11,16,17))" & condtion & " order by AccountName Limit 50")
        Else
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))" & condtion & " order by AccountName  Limit 50")
        End If
        Try
            If dt.Rows.Count > 0 Then
                dgCustomer.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgCustomer.Rows.Add()
                    With dgCustomer.Rows(i)
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

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If Val(txtid.Text) > 0 Then
            retrive2()
        End If
        If Val(txtid.Text) = 0 Then
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
            retrive2() : PrintRecord()
            Report_Viewer.printReport("\SuperSaleBeejak.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
            'pnlWhatsapp.Visible = True : pnlWhatsapp.BringToFront() : txtWhatsappNo.Focus()
            'If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "WhatsApp API" Then
            '    cbType.SelectedIndex = 0
            '    Exit Sub
            'Else
            '    cbType.SelectedIndex = 1
            '    Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            '    If System.IO.File.Exists(WhatsappFile) = False Then
            '        MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
            '        Exit Sub
            '    End If
            '    Dim p() As Process
            '    p = Process.GetProcessesByName("Easy Whatsapp")
            '    If p.Count = 0 Then
            '        Dim StartWhatsapp As New System.Diagnostics.Process
            '        StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            '        StartWhatsapp.Start()
            '    End If
            'End If
            'Exit Sub
        End If
        ' pnlWhatsapp.Visible = False
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()

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

    Private Sub BtnSave_GotFocus(sender As Object, e As EventArgs) Handles BtnSave.GotFocus
        TempRowColumn()
    End Sub
    Private Sub Dg2_MouseClick(sender As Object, e As MouseEventArgs) Handles Dg2.MouseClick
        Dg2.ClearSelection()
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
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            txtItem.Focus() : e.SuppressKeyPress = True
            DgAccountSearch.SendToBack() : DgAccountSearch.Visible = False
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 304
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 160
        DgAccountSearch.BringToFront() : DgAccountSearch.Visible = True
        retriveAccounts()
    End Sub

    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        AccountRowColumns()
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
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
        dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,17)  or UnderGroupID in (11,17))" & condtion & " order by AccountName Limit 50")
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
    Private Sub ItemRowColumns()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 130
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 130
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
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
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
            ' dgItemSearch.Visible = True
            retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
    End Sub

    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress, txtAccount.KeyPress, txtCustomer.KeyPress, txtCharges.KeyPress
        If txtItem.Focused = True Then dgItemSearch.Visible = True
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        itemfill() : dgItemSearch.Visible = False : txtCustomer.Focus()
        Dim CutStop As String = String.Empty
    End Sub

    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtItem.Clear()
            txtItemID.Clear()
            txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            itemfill() : dgItemSearch.Visible = False : txtCustomer.Focus()
            e.SuppressKeyPress = True
        End If

    End Sub
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
            ItemPer = row("RateAs").ToString()
            If ItemPer = "ItemWise" Then Cbper.Text = row("RateAs").ToString()
            trackStock = row("TrackStock").ToString()
        End If
        'AccountComm()
    End Sub
    'Private Sub itemfill()
    '    If dg1.SelectedRows.Count = 0 Then
    '        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")
    '        txtComPer.Text = Format(Val(clsFun.ExecScalarStr(" Select CommisionPer FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")), "0.00")
    '        txtMPer.Text = Format(Val(clsFun.ExecScalarStr(" Select UserChargesPer FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")), "0.00")
    '        txtTare.Text = Format(Val(clsFun.ExecScalarStr(" Select tare FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")), "0.00")
    '        txtLabour.Text = Format(Val(clsFun.ExecScalarStr(" Select Labour FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")), "0.00")
    '        txtRdfPer.Text = Format(Val(clsFun.ExecScalarStr(" Select rdfper FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")), "0.00")
    '        txtWeight.Text = Format(Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")), "0.00")
    '    Else
    '        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & txtItemID.Text & "'")
    '    End If
    'End Sub
    Private Sub dgCustomer_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgCustomer.CellClick
        txtCustomer.Clear()
        txtcustomerID.Clear()
        txtcustomerID.Text = dgCustomer.SelectedRows(0).Cells(0).Value
        txtCustomer.Text = dgCustomer.SelectedRows(0).Cells(1).Value
        dgCustomer.Visible = False
        If clsFun.ExecScalarStr("Select SpeedKaat From Controls") = "Y" Then txtCut.TabStop = True : txtCut.Focus() Else txtCut.TabStop = False : txtNug.Focus()
    End Sub

    Private Sub dgCustomer_KeyDown(sender As Object, e As KeyEventArgs) Handles dgCustomer.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtCustomer.Clear()
            txtcustomerID.Clear()
            txtcustomerID.Text = dgCustomer.SelectedRows(0).Cells(0).Value
            txtCustomer.Text = dgCustomer.SelectedRows(0).Cells(1).Value
            dgCustomer.Visible = False
            If clsFun.ExecScalarStr("Select SpeedKaat From Controls") = "Y" Then txtCut.TabStop = True : txtCut.Focus() Else txtCut.TabStop = False : txtNug.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtCustomer.Focus()
    End Sub
    Private Sub txtCustomer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCustomer.KeyPress
        CustoemerRowColums()
    End Sub

    Private Sub FillCharges()
        'If Dg2.SelectedRows.Count > 0 Then Exit Sub
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & Val(txtChargeID.Text) & "'")
        If CalcType = "Aboslute" Then
            txtOnValue.TabStop = False
            txtCalculatePer.TabStop = False
            txtOnValue.Text = ""
            txtchargesAmount.Focus()
        ElseIf CalcType = "Weight" Then
            txtOnValue.Text = txttotalWeight.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Percentage" Then
            txtOnValue.Text = txtSallerBasicTotal.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        ElseIf CalcType = "Nug" Then
            txtOnValue.Text = txtTotalNug.Text
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        End If
    End Sub
    Private Sub txtCustomer_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomer.KeyUp
        If txtCustomer.Text.Trim() <> "" Then
            If dgCustomer.ColumnCount = 0 Then CustoemerRowColums()
            RetriveCustomer(" And upper(accountname) Like upper('" & txtCustomer.Text.Trim() & "%')")
        End If
    End Sub
    Private Sub dgCharges_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgCharges.CellClick
        txtCharges.Clear()
        txtChargeID.Clear()
        txtChargeID.Text = dgCharges.SelectedRows(0).Cells(0).Value
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        lblClcType.Text = dgCharges.SelectedRows(0).Cells(2).Value
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
            lblClcType.Text = dgCharges.SelectedRows(0).Cells(2).Value
            dgCharges.Visible = False
            txtOnValue.Focus()
            FillCharges()
            ChargesCalculation()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtCharges.Focus()
    End Sub

    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs) Handles txtCharges.GotFocus, txtCharges.Click
        dgItemSearch.Visible = False : DgAccountSearch.Visible = False : dgCustomer.Visible = False
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
    Private Sub txtCharges_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyUp
        If txtCharges.Text.Trim() <> "" Then
            'dgCharges.Visible = True
            RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
        Else
            RetriveCharges()
        End If
        If e.KeyCode = Keys.Escape Then dgCharges.Visible = False
    End Sub

    Private Sub txtCharges_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCharges.KeyPress
        dgCharges.Visible = True
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
    End Sub


    Private Sub txtCalculatePer_TextChanged(sender As Object, e As EventArgs) Handles txtCalculatePer.TextChanged
        ChargesCalculation()
    End Sub

    Private Sub txtVoucherNo_Leave(sender As Object, e As EventArgs) Handles txtVoucherNo.Leave
        txtInvoiceID.Text = Val(txtVoucherNo.Text)
        If txtInvoiceID.Text = 0 Then txtInvoiceID.Text = 1
    End Sub

    Private Sub txtsallerTotal_TextChanged(sender As Object, e As EventArgs) Handles txtsallerTotal.TextChanged
        Try
            lblInword.Text = AmtInWord(txtsallerTotal.Text)
        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
    End Sub
    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        If dg1.Rows.Count <> 0 Then
            Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "AADHAT")
            If msgRslt = MsgBoxResult.Yes Then
                Me.Close()
            ElseIf msgRslt = MsgBoxResult.No Then
                Exit Sub
            End If
        Else
            Me.Close()
        End If

    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
    End Sub

    Private Sub btnMultiUpdate_Click(sender As Object, e As EventArgs) Handles btnMultiUpdate.Click
        MultiUpdate()
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        FillWithNevigation()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If Offset = 0 Then
            Offset = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Super Sale'  Order By ID ")
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
        Offset = (TotalPages - 1) * RowCount
        FillWithNevigation()
    End Sub


    Private Sub txtCustomerRate_Leave1(sender As Object, e As EventArgs) Handles txtCustomerRate.Leave
        txtSallerRate.Text = Val(txtCustomerRate.Text)
    End Sub


    Private Sub txtInvoiceID_Leave(sender As Object, e As EventArgs) Handles txtInvoiceID.Leave
        pnlInvoiceID.Visible = False : txtVoucherNo.Focus()
    End Sub



    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Val(txtid.Text) > 0 Then
            retrive3()
        End If
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
            PrintRecord()
            Report_Viewer.printReport("\SuperSaleBeejak.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
        pnlWhatsapp.Visible = False
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
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
        retrive3() : PrintRecord()
        Pdf_Genrate.ExportReport("\Formats\SuperSaleBeejak.rpt")
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Whatsapp\Pdfs\" & GlobalData.PdfName)
        whatsappSender.SendWhatsAppFile("91" & txtWhatsappNo.Text, "Sended By: Aadhat Software" & vbCrLf & "www.softmanagementindia.in", FilePath)
        lblStatus.Text = "PDF Sent " & whatsappSender.APIResposne
        lblStatus.Visible = True
        sql = "insert into waReport(EntryDate,AccountName,WhatsAppNo,Type,Status) SELECT '" & Date.Today.ToString("yyyy-MM-dd") & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','SUper Sale','" & lblStatus.Text & "'"
        clsFun.ExecNonQuery(sql)
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
            instance_id = ClsFunPrimary.ExecScalarStr("Select InstanceID From API")
            If instance_id = "" Then MainScreenForm.MdiParent = Me : WhatsApp_API.Show()
            UsingWhatsappAPI()
            pnlWhatsapp.Visible = False : mskEntryDate.Focus()
            cleartxt() : cleartxtCharges() : FootertextClear()
            dg1.Rows.Clear() : Dg2.Rows.Clear()
            pnlWhatsapp.Visible = False : mskEntryDate.Focus()
        Else

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
            pnlWhatsapp.Visible = False
        End If
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
        retrive3() : PrintRecord()
        Pdf_Genrate.ExportReport("\Formats\SuperSaleBeejak.rpt")
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        pnlWhatsapp.Visible = False
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Val(txtid.Text) > 0 Then
            retrive3()
        End If
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
            PrintRecord()
            Report_Viewer.printReport("\SuperSaleBeejak.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
        pnlWhatsapp.Visible = False
        cleartxt() : cleartxtCharges() : FootertextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
    End Sub

    Private Sub txtAddWeight_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAddWeight.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtAddWeight.Text = "" Then pnlAddWeight.Visible = False : txtWeight.Focus() : Exit Sub
            Dim code As String = ""
            Dim a() As String = Split(txtAddWeight.Text, "+")
            If a.Length >= 1 Then
                For i = 0 To a.Length - 1
                    code = Val(code) + Val(a(i).ToString)
                Next
            End If
            txtWeight.Text = code
            txtWeight.Focus()
            pnlAddWeight.Visible = False
        End If
    End Sub

    Private Sub txtAddWeight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAddWeight.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") Or ((e.KeyChar = "+") = -1)))
    End Sub


    Private Sub txtVehicleNo_TextChanged(sender As Object, e As EventArgs) Handles txtVehicleNo.TextChanged

    End Sub

    Private Sub txtCustMobile_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCustMobile.KeyDown, txtDriverName.KeyDown,
          txtDriverMobile.KeyDown, txtRemark.KeyDown, txtStateName.KeyDown, txtGSTN.KeyDown, txtCustMobile.KeyDown, txtBrokerName.KeyDown, txtBrokerMob.KeyDown,
        txtTransPort.KeyDown, txtGrNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub txtRemark_Leave(sender As Object, e As EventArgs) Handles txtRemark.Leave
        pnlSendingDetails.Visible = False : txtAccount.Focus()
    End Sub

    Private Sub txtAdvancePay_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAdvancePay.KeyDown
        If e.KeyCode = Keys.Enter Then BtnSave.PerformClick()
    End Sub

    Private Sub txtVoucherNo_TextChanged(sender As Object, e As EventArgs) Handles txtVoucherNo.TextChanged

    End Sub

    Private Sub txtchargesAmount_Leave(sender As Object, e As EventArgs) Handles txtchargesAmount.Leave

    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub txtCustomer_TextChanged(sender As Object, e As EventArgs) Handles txtCustomer.TextChanged

    End Sub

    Private Sub txtBasicCustomer_KeyUp(sender As Object, e As KeyEventArgs) Handles txtBasicCustomer.KeyUp
        If Cbper.SelectedIndex = 0 Then
            txtCustomerRate.Text = Val(txtBasicCustomer.Text) / Format(Val(txtNug.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtCustomerRate.Text = Val(txtBasicCustomer.Text) / Format(Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 5 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 3 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 10 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 4 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 20 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 5 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 40 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 6 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 41 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 7 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 50 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 8 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 51 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 9 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 51.7 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 10 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 52.3 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 11 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 53 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 12 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 80 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 13 Then
            txtCustomerRate.Text = Format(Val(txtBasicCustomer.Text) * 100 / Val(txtWeight.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 14 Then
            txtCustomerRate.Text = Format(Val(txtNug.Text) * Val(txtBasicCustomer.Text), "0.00")
        End If
        txtSallerAmout.Text = txtBasicCustomer.Text
        txtCustomerRate.Text = Format(Math.Round(Val(txtCustomerRate.Text), 2), "0.00")
        txtSallerRate.Text = txtCustomerRate.Text
        If txtCustomerRate.Text = "NAN" Then txtCustomerRate.Text = "" : Exit Sub
        txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtBasicCustomer.Text) / 100, "0.00")
        txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtBasicCustomer.Text) / 100, "0.00")
        If Val(txtRdfPer.Text) > 0 Then
            txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtBasicCustomer.Text) / 100, "0.00")
        Else
            txtRdfAmt.Text = Format(Val(txtRdfAmt.Text), "0.00")
        End If
        If clsFun.ExecScalarStr("Select ApplyCommWeight From Controls") = "Yes" Then
            txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtWeight.Text), "0.00")
        Else
            Dim crateRate As String = clsFun.ExecScalarStr("Select CrateBardana From Controls")
            If crateRate = "Yes" And lblCrate.Text = "Y" Then txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtCrateQty.Text), "0.00") Else txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
        End If
        txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
        lbltotCharges.Text = Format(Val(Val(txtComAmt.Text)) + Val(Val(txtMAmt.Text)) + Val(Val(txtRdfAmt.Text)) + Val(Val(txtTareAmt.Text)) + Val(Val(txtLaboutAmt.Text)), "0.00")
        txtTotAmount.Text = Format(Val(Val(lbltotCharges.Text)) + Val(Val(txtBasicCustomer.Text)), 0)
    End Sub

    Private Sub txtBasicCustomer_Leave(sender As Object, e As EventArgs) Handles txtBasicCustomer.Leave
        '  SuperCalculation()
    End Sub

    Private Sub txtBasicCustomer_TextChanged(sender As Object, e As EventArgs) Handles txtBasicCustomer.TextChanged

    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub txtSallerRate_TextChanged(sender As Object, e As EventArgs) Handles txtSallerRate.TextChanged

    End Sub
End Class