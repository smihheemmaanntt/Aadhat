Public Class Stock_Sale
    Dim vno As Integer : Dim VchId As Integer
    Dim sql As String = String.Empty : Dim count As Integer = 0
    Dim MaxID As String = String.Empty : Dim itemBal As String = String.Empty
    Dim LotBal As String = String.Empty : Dim RestBal As String = String.Empty
    Dim UpdateTmp As Integer = 0 : Dim tmpID As String = String.Empty
    Dim bal As Decimal = 0.0 : Dim curindex As Integer = 0
    Dim el As New Aadhat.ErrorLogger
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 1 : Dim Offset As Integer = 0
    Dim ServerTag As Integer : Dim ItemPer As String
    Dim crateRate As String = String.Empty : Dim trackStock As String
    Dim CalculationMethod As String : Dim CUT As Decimal = 0.0


    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        clsFun.DoubleBuffered(dg3, True)
    End Sub

    Private Sub Stock_Sale_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        ' MainScreenForm.lblItemBalance.Visible = False
    End Sub
    Private Sub Stock_Sale_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F2 Then
            If BtnSave.Text <> "&Save" Then
                If txtPurchaseType.Enabled = True Then txtPurchaseType.Enabled = False : txtStorage.Enabled = False Else txtPurchaseType.Enabled = True : txtStorage.Enabled = True
            End If
        End If
        If e.KeyCode = Keys.F10 Then
            If Val(txtid.Text) = Val(0) Then Exit Sub
            If dg3.ColumnCount = 0 Then SaleRowColums()
            retriveSale()
            PnlCustomerBill.Visible = True : PnlCustomerBill.BringToFront()
        End If
        If e.KeyCode = Keys.Escape Then
            If btnClose.Enabled = False Then Exit Sub
            If PnlCustomerBill.Visible = True Then
                PnlCustomerBill.Visible = False
                Exit Sub
            ElseIf DgAccountSearch.Visible = True Then
                DgAccountSearch.Visible = False
                Exit Sub
            ElseIf dgStore.Visible = True Then
                dgStore.Visible = False
                Exit Sub
            ElseIf dgItemSearch.Visible = True Then
                dgItemSearch.Visible = False
                Exit Sub
            ElseIf dgItemSearch.Visible = True Then
                dgItemSearch.Visible = False
                Exit Sub
                If dg1.Rows.Count = 0 Then Me.Close() : Exit Sub
            ElseIf dg1.Rows.Count = 0 Then
                Me.Close() : Exit Sub
            ElseIf dg1.Rows.Count <> 0 Then
                If MessageBox.Show("Are you Sure Want to Exit Stock Sale ???", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                End If
            End If
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.F4 Then
            If pnlInvoiceID.Visible = False Then
                pnlInvoiceID.Visible = True
            Else
                pnlInvoiceID.Visible = False
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

    Private Sub Stock_Sale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0 : Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskEntryDate.Focus() : Cbper.SelectedIndex = 0
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33,11) order by AccountName ", "AccountName", "ID", "--N./A.--")
        Me.KeyPreview = True : pnlMarka.Visible = False : clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        BtnDelete.Enabled = False : rowColums() : VNumber() : FillStockSale()
    End Sub

    Public Sub FillStockSale()
        ssql = "Select * from Controls"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("StockCommission").ToString().Trim() = "Percentage" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = False
            ElseIf dt.Rows(0)("StockCommission").ToString().Trim() = "Amount" Then
                txtComAmt.TabStop = True : txtComPer.TabStop = False
            ElseIf dt.Rows(0)("StockCommission").ToString().Trim() = "Both" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = True
            Else
                txtComPer.TabStop = False : txtComAmt.TabStop = False
            End If
            If dt.Rows(0)("StockMandiTax").ToString().Trim() = "Percentage" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = False
            ElseIf dt.Rows(0)("StockMandiTax").ToString().Trim() = "Amount" Then
                txtMAmt.TabStop = True : txtMPer.TabStop = False
            ElseIf dt.Rows(0)("StockMandiTax").ToString().Trim() = "Both" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = True
            Else
                txtMPer.TabStop = False : txtMAmt.TabStop = False
            End If
            If dt.Rows(0)("StockRDF").ToString().Trim() = "Percentage" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = False
            ElseIf dt.Rows(0)("StockRDF").ToString().Trim() = "Amount" Then
                txtRdfAmt.TabStop = True : txtRdfPer.TabStop = False
            ElseIf dt.Rows(0)("StockRDF").ToString().Trim() = "Both" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = True
            Else
                txtRdfPer.TabStop = False : txtRdfAmt.TabStop = False
            End If
            If dt.Rows(0)("StockTare").ToString().Trim() = "Nug" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = False
            ElseIf dt.Rows(0)("StockTare").ToString().Trim() = "Amount" Then
                txtTareAmt.TabStop = True : txtTare.TabStop = False
            ElseIf dt.Rows(0)("StockTare").ToString().Trim() = "Both" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = True
            Else
                txtTare.TabStop = False : txtTareAmt.TabStop = False
            End If
            If dt.Rows(0)("StockLabour").ToString().Trim() = "Nug" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = False
            ElseIf dt.Rows(0)("StockLabour").ToString().Trim() = "Amount" Then
                txtLaboutAmt.TabStop = True : txtLabour.TabStop = False
            ElseIf dt.Rows(0)("StockLabour").ToString().Trim() = "Both" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = True
            Else
                txtLabour.TabStop = False : txtLaboutAmt.TabStop = False
            End If
            If dt.Rows(0)("StockTaxPaid").ToString().Trim() = "Yes" Then ckTaxPaid.Visible = True Else ckTaxPaid.Visible = False
            If dt.Rows(0)("StockKaat").ToString().Trim() = "Kaat" Then txtCut.TabStop = True Else txtCut.TabStop = False
            If dt.Rows(0)("StockKaat").ToString().Trim() = "Nug" Then CalculationMethod = "Nug"
            ItemPer = dt.Rows(0)("Per").ToString().Trim()
            If ItemPer <> "itemWise" Then Cbper.Text = dt.Rows(0)("Per").ToString().Trim()
            If dt.Rows(0)("StopBasic").ToString().Trim() = "Yes" Then txtbasicAmt.TabStop = True : txtbasicAmt.ReadOnly = False Else txtbasicAmt.TabStop = False : txtbasicAmt.ReadOnly = True
            CrateRate = clsFun.ExecScalarStr("Select CrateBardana From Controls")
        End If
    End Sub
    Private Sub AcBal()
        '  Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtPurchaseTypeID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtPurchaseTypeID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtPurchaseTypeID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtPurchaseTypeID.Text) & "")
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
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtPurchaseTypeID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Format(Math.Abs(Val(opbal)), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtPurchaseTypeID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
            Else
                opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
            End If
        End If
        lblAcBal.Visible = True
        lblAcBal.Text = "Party Bal : (" & opbal & ")"
        AccountComm()
    End Sub
    Private Sub CustBal()
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
            opbal = Format(Math.Abs(Val(opbal)), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
            Else
                opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
            End If
        End If
        lblCustBal.Visible = True
        lblCustBal.Text = "Customer Bal : (" & opbal & ")"

    End Sub
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
    Private Sub txtNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        DgAccountSearch.Visible = False : dgItemSearch.Visible = False : dgStore.Visible = False
        If dgLot.ColumnCount = 0 Then LotCoulmns()
        If dgLot.RowCount = 0 Then RetriveLot()
        If dgLot.SelectedRows.Count = 0 Then dgLot.Visible = True : txtLot.Focus() : Exit Sub
        'If txtLot.Text.Trim() <> "" Then
        '    RetriveLot(" And upper(LotNo) like upper('" & txtLot.Text.Trim() & "%')")
        'Else
        '    If dgLot.RowCount = 0 Then RetriveLot()
        'End If
        If dgLot.SelectedRows.Count = 0 Then txtLot.Focus() : Exit Sub
        txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
        txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
        dgLot.Visible = False
        ItemBalance() : LotBalance()
        txtNug.SelectionStart = 0 : txtNug.SelectionLength = Len(txtNug.Text)
        If lblCrate.Text = "Y" Then
            If txtAccountID.Text = 7 Then
                cbAccountName.Enabled = True : If dg1.SelectedRows.Count = 0 Then cbAccountName.SelectedIndex = 0
            Else
                cbAccountName.Enabled = False : cbAccountName.SelectedIndex = -1
            End If
            pnlMarka.Visible = True
            pnlMarka.BringToFront()
        End If
        AccountComm()
    End Sub
    Public Sub RefreshAccounts()
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33) order by AccountName ", "AccountName", "ID", "--N./A.--")
    End Sub
    Private Sub cbAccountName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbAccountName.KeyDown
        If e.KeyCode = Keys.Enter Then cbCrateMarka.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(Val(cbAccountName.SelectedValue))
            ' clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
    End Sub

    Private Sub txtItem_Click(sender As Object, e As EventArgs) Handles txtItem.Click
        txtItem.SelectionStart = 0 : txtItem.SelectionLength = Len(txtItem.Text)
    End Sub
    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus
        If txtStorage.Enabled = True Then
            dgStore.SendToBack() : dgStore.Visible = False
            If dgStore.ColumnCount = 0 Then StoreColums()
            If dgStore.RowCount = 0 Then RetriveMode()
            If dgStore.SelectedRows.Count = 0 Then dgStore.Visible = True : txtStorage.Focus() : Exit Sub
            txtStorageID.Text = Val(dgStore.SelectedRows(0).Cells(0).Value)
            txtStorage.Text = dgStore.SelectedRows(0).Cells(1).Value
            dgStore.Visible = False ': AcBal()
        End If
        If dgItemSearch.ColumnCount = 0 Then StockInItemRowColums()
        If dgItemSearch.RowCount = 0 Then StockINretriveItems()
        If txtItem.Text.Trim() <> "" Then
            StockINretriveItems(" And upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            StockINretriveItems()
        End If
        txtItem.SelectionStart = 0 : txtItem.SelectionLength = Len(txtItem.Text)
        dgItemSearch.Visible = True : DgAccountSearch.Visible = False
    End Sub

    Private Sub cbLot_GotFocus(sender As Object, e As EventArgs)
        If txtAccount.Text = "" Then txtAccount.Focus()
    End Sub
    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.SelectAll()
        mskEntryDate.BackColor = Color.LightGray
    End Sub

    Private Sub txtLot_Click(sender As Object, e As EventArgs) Handles txtLot.Click, txtLot.GotFocus
        'LotCoulmns()
        'txtLot.SelectionStart = 0 : txtLot.SelectionLength = Len(txtLot.Text)
    End Sub

    Private Sub txtLot_GotFocus(sender As Object, e As EventArgs) Handles txtLot.GotFocus
        DgAccountSearch.Visible = False : dgLot.Visible = True
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        If txtAccount.Text <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "')")
        Else
            retriveAccounts()
        End If
        DgAccountSearch.Visible = False
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        CustBal()
        If dgLot.ColumnCount = 0 Then LotCoulmns()
        If dgLot.RowCount = 0 Then RetriveLot()
        If txtLot.Text.Trim() <> "" Then
            RetriveLot(" And upper(LotNo) like upper('" & txtLot.Text.Trim() & "%')")
        Else
            If dgLot.RowCount = 0 Then RetriveLot()
        End If
        txtLot.SelectionStart = 0 : txtLot.SelectionLength = Len(txtLot.Text)
    End Sub

    Private Sub txtSallerRate_GotFocus(sender As Object, e As EventArgs) Handles txtSallerRate.GotFocus
        If DgAccountSearch.Visible = True Then
            DgAccountSearch.Visible = False
        ElseIf dgItemSearch.Visible = True Then
            dgItemSearch.Visible = False
        ElseIf dgStore.Visible = True Then
            dgStore.Visible = False
        End If
        'If ckTaxPaid.Checked = True Then
        '    pnlNetRate.Visible = True
        '    txtNetRate.Focus()
        'End If
        txtSallerRate.SelectionStart = 0
        txtSallerRate.SelectionLength = Len(txtSallerRate.Text)
    End Sub

    Private Sub txtCustomerRate_GotFocus(sender As Object, e As EventArgs) Handles txtCustomerRate.GotFocus, txtCustomerRate.Click

        If DgAccountSearch.Visible = True Then
            DgAccountSearch.Visible = False
        ElseIf dgItemSearch.Visible = True Then
            dgItemSearch.Visible = False
        ElseIf dgStore.Visible = True Then
            dgStore.Visible = False
        End If
        If ckTaxPaid.Checked = True Then
            pnlNetRate.Visible = True
            txtNetRate.Focus()
        End If
        txtCustomerRate.SelectionStart = 0 : txtCustomerRate.SelectionLength = Len(txtCustomerRate.Text)
    End Sub
    Private Sub txtNug_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNug.KeyUp
        If btnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) > 0 Then
                txtKg.Text = Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) * Val(txtNug.Text)
            End If
        End If
    End Sub
    Private Sub txtKg_Click(sender As Object, e As EventArgs) Handles txtKg.Click, txtKg.GotFocus
        txtKg.SelectAll() : pnlGrossWeight.Visible = False : pnlMarka.Visible = False
        If txtAddWeight.Text = "" Then lblAddWeight.Text = "" Else lblAddWeight.Text = txtAddWeight.Text
        lblAddWeight.Visible = True
    End Sub

    Private Sub txtKg_GotFocus(sender As Object, e As EventArgs) Handles txtKg.GotFocus
        If DgAccountSearch.Visible = True Then
            DgAccountSearch.Visible = False
        ElseIf dgItemSearch.Visible = True Then
            dgItemSearch.Visible = False
        ElseIf dgStore.Visible = True Then
            dgStore.Visible = False
        End If
    End Sub

    Private Sub txtCut_GotFocus(sender As Object, e As EventArgs) Handles txtCut.GotFocus
        If DgAccountSearch.Visible = True Then
            DgAccountSearch.Visible = False
        ElseIf dgItemSearch.Visible = True Then
            dgItemSearch.Visible = False
        ElseIf dgStore.Visible = True Then
            dgStore.Visible = False
        End If
    End Sub

    Private Sub txtAccount_Click(sender As Object, e As EventArgs) Handles txtAccount.Click
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
    End Sub



    'Private Sub txtStorage_Click(sender As Object, e As EventArgs) Handles txtStorage.GotFocus
    '    dgPurchaseType.Visible = False
    '    'If dgPurchaseType.ColumnCount = 0 Then PurchaseTypeColumns()
    '    'If dgPurchaseType.RowCount = 0 Then retrivePurchaseType()
    '    If dgPurchaseType.SelectedRows.Count = 0 Then dgPurchaseType.Visible = True : txtPurchaseType.Focus() : Exit Sub
    '    txtPurchaseID.Text = Val(dgPurchaseType.SelectedRows(0).Cells(0).Value)
    '    txtPurchaseType.Text = dgPurchaseType.SelectedRows(0).Cells(1).Value
    '    dgPurchaseType.Visible = False ' AcBal()
    '    'If dgStore.ColumnCount = 0 Then StoreColums()
    '    '' If dgStore.RowCount = 0 Then RetriveMode()
    '    'If txtStorage.Text.Trim() <> "" Then
    '    '    RetriveMode(" and upper(StorageName) Like upper('" & txtStorage.Text.Trim() & "%')")
    '    'Else
    '    '    RetriveMode()
    '    'End If
    '    'txtStorage.Focus()
    '    'txtStorage.SelectionStart = 0 : txtStorage.SelectionLength = Len(txtStorage.Text)
    'End Sub

    Private Sub ckTaxPaid_GotFocus(sender As Object, e As EventArgs) Handles ckTaxPaid.GotFocus
        ckTaxPaid.ForeColor = Color.GhostWhite
        ckTaxPaid.BackColor = Color.Green
    End Sub

    Private Sub txtAddWeight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAddWeight.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") Or ((e.KeyChar = "+") = -1)))
        'e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)) Or ((e.KeyChar = "+") And (sender.Text.IndexOf(".") = -1)))

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
    Private Sub txtPurchaseType_GotFocus(sender As Object, e As EventArgs) Handles txtPurchaseType.GotFocus, txtLot.GotFocus,
     txtCut.GotFocus, txtNug.GotFocus, txtKg.GotFocus, txtCustomerRate.GotFocus, txtStorage.GotFocus, txtItem.GotFocus, txtAccount.GotFocus,
    txtSallerRate.GotFocus, txtCrateQty.GotFocus, txtbasicAmt.GotFocus, txtVoucherNo.GotFocus, txtComPer.GotFocus,
    txtMPer.GotFocus, txtRdfPer.GotFocus, txtTare.GotFocus, txtLabour.GotFocus, txtComAmt.GotFocus, txtMAmt.GotFocus, txtRdfAmt.GotFocus,
    txtTareAmt.GotFocus, txtLaboutAmt.GotFocus
        If txtPurchaseType.Focused Then
            If dgPurchaseType.ColumnCount = 0 Then PurchaseTypeColumns()
            If txtPurchaseType.Text.Trim() <> "" Then
                retrivePurchaseType(" Where upper(StockHolderName) Like upper('" & txtPurchaseType.Text.Trim() & "%')")
            Else
                retrivePurchaseType()
            End If
        End If
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub

    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles txtPurchaseType.LostFocus, txtLot.LostFocus,
     txtCut.LostFocus, txtNug.LostFocus, txtKg.LostFocus, txtCustomerRate.LostFocus, txtStorage.LostFocus, txtItem.LostFocus, txtAccount.LostFocus,
    txtSallerRate.LostFocus, txtCrateQty.LostFocus, txtbasicAmt.LostFocus, txtVoucherNo.LostFocus, txtComPer.LostFocus, txtMPer.LostFocus,
    txtRdfPer.LostFocus, txtTare.LostFocus, txtLabour.LostFocus, txtComAmt.LostFocus, txtMAmt.LostFocus, txtRdfAmt.LostFocus, txtTareAmt.LostFocus, txtLaboutAmt.LostFocus
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
    Private Sub txtGrossWt_GotFocus(sender As Object, e As EventArgs) Handles txtGrossWt.GotFocus
        txtGrossWt.SelectAll()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtPurchaseType.KeyDown, txtLot.KeyDown,
         txtCut.KeyDown, txtNug.KeyDown, txtKg.KeyDown, txtCustomerRate.KeyDown, Cbper.KeyDown, txtStorage.KeyDown, txtItem.KeyDown, txtAccount.KeyDown,
        txtSallerRate.KeyDown, cbCrateMarka.KeyDown, txtCrateQty.KeyDown, txtbasicAmt.KeyDown, ckTaxPaid.KeyDown, txtVoucherNo.KeyDown, txtGrossWt.KeyDown
        If txtVoucherNo.Focused Then
            If e.KeyCode = Keys.F3 Then
                pnlInvoiceID.Visible = True
                txtInvoiceID.Focus()
            End If
        End If
        If txtLot.Focused Then
            If e.KeyCode = Keys.Down Then dgLot.Focus() : Exit Sub
        End If
        If txtStorage.Focused Then
            If e.KeyCode = Keys.Down Then dgStore.Focus() : Exit Sub
        End If

        If e.KeyCode = Keys.Enter Then
            If txtNug.Focused AndAlso Val(CUT) > 0 AndAlso lblCrate.Text = "N" Then
                pnlGrossWeight.Visible = True : txtGrossWt.Focus()
                pnlGrossWeight.BringToFront()
                Exit Sub
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            If txtCrateQty.Focused AndAlso Val(CUT) > 0 Then
                pnlGrossWeight.Visible = True : pnlGrossWeight.BringToFront()
                txtGrossWt.Focus() : pnlMarka.Visible = False
                Exit Sub
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If txtKg.Focused Then
            If e.KeyCode = Keys.F3 Then
                pnlAddWeight.Visible = True
                txtAddWeight.Focus()
            End If
        End If
        If dgPurchaseType.Visible = True Then
            If txtPurchaseType.Focused Then
                If e.KeyCode = Keys.Down Then dgPurchaseType.Focus() : Exit Sub
            End If
        End If


        If DgAccountSearch.Visible = False And dgItemSearch.Visible = False And dgPurchaseType.Visible = False And dgStore.Visible = False And dgLot.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or Cbper.Focused = True Or cbCrateMarka.Focused = True Or cbAccountName.Focused = True Or txtCrateQty.Focused = True Or txtLot.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If DgAccountSearch.Visible = False And dgItemSearch.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or Cbper.Focused = True Or cbCrateMarka.Focused = True Or dgPurchaseType.Visible = True Or cbAccountName.Focused = True Or txtCrateQty.Focused = True Or txtLot.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus()
        End Select
        If txtAccount.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show() : CreateAccount.Opener = Me
                CreateAccount.OpenedFromItems = True
                CreateAccount.TextBoxSender = CreateAccount.Name
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID in(16,32) Order by ID Desc ", "GroupName", "ID", "")
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
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
            ' e.SuppressKeyPress = True
        End If
        If txtCustomerRate.Focused Then
            If e.KeyCode = Keys.Enter Then
                If ckTaxPaid.Checked = True Then
                    pnlNetRate.Visible = True
                    txtNetRate.Focus()
                End If
            End If

        End If

        If txtPurchaseType.Focused Then
            If e.KeyCode = Keys.Down Then dgPurchaseType.Focus()
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.Down Then dgItemSearch.Focus()
        End If

    End Sub
    Private Sub retriveSale(Optional ByVal condtion As String = "")
        dg3.Rows.Clear() : lblNugs.Text = Format(Val(0), "0.00") : lblWeight.Text = Format(Val(0), "0.00") : lblAmount.Text = Format(Val(0), "0.00")
        Dim dt As New DataTable
        Dim sql As String = String.Empty
        sql = "Select VoucherID,ItemID, ItemName,Lot,Round(sum(Nug),2) as nug,Round(sum(Weight),2) as weight,Round(Rate,2) as Rate,Per,Round(sum(Amount),2) as Amount,Round(sum(CommAmt),2) as ComAmt,Round(sum(MAmt),2) as Mamt from Transaction2 where VoucherID=" & Val(txtid.Text) & " Group By ItemId,Lot,SRate order by Rate Desc"
        dt = clsFun.ExecDataTable(sql)
        '   dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE PurchaseID=" & Val(txtid.Text) & " order by ID")
        Dim dvData As DataView = New DataView(dt)
        Try
            If dt.Rows.Count > 0 Then
                lblNugs.Text = Format(Val(dt.Compute("Sum(Nug)", "")), "0.00")
                lblWeight.Text = Format(Val(dt.Compute("Sum(Weight)", "")), "0.00")
                lblAmount.Text = Format(Val(dt.Compute("Sum(Amount)", "")), "0.00")
                lblTotComm.Text = Format(Val(dt.Compute("Sum(ComAmt)", "")), "0.00")
                lblTotMTax.Text = Format(Val(dt.Compute("Sum(Mamt)", "")), "0.00")
                dg3.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg3.Rows.Add()
                    With dg3.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(2).Value = dt.Rows(i)("Lot").ToString()
                        .Cells(3).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(4).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("ComAmt").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Mamt").ToString()), "0.00")
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next
            End If
            dg3.ClearSelection()
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg3.ClearSelection()
    End Sub
    Private Sub SaleRowColums()
        dg3.ColumnCount = 9
        dg3.Columns(0).Name = "PurcahseID" : dg3.Columns(0).Visible = False
        dg3.Columns(1).Name = "Item Name" : dg3.Columns(1).Width = 200
        dg3.Columns(2).Name = "Lot" : dg3.Columns(2).Width = 100
        dg3.Columns(3).Name = "Nug" : dg3.Columns(3).Width = 150
        dg3.Columns(4).Name = "Weight" : dg3.Columns(4).Width = 150
        dg3.Columns(5).Name = "Rate" : dg3.Columns(5).Width = 150
        dg3.Columns(6).Name = "Amount" : dg3.Columns(6).Width = 150
        dg3.Columns(7).Name = "Commission" : dg3.Columns(7).Width = 120
        dg3.Columns(8).Name = "MTax" : dg3.Columns(8).Width = 120

    End Sub

    Private Sub ItemFill()
        Dim itemID As Integer = Val(txtItemID.Text)
        If itemID = 0 Then Exit Sub ' Prevent unnecessary queries if ItemID is invalid
        ' Use a single query to fetch all required fields
        Dim query As String = "SELECT CommisionPer, UserChargesPer, Tare, Labour, RDFPer, MaintainCrate, WeightPerNug,TrackStock,RateAs,CutPerNug FROM Items WHERE ID = " & itemID
        Dim dt As DataTable = clsFun.ExecDataTable(query) ' Assume ExecDataTable returns a DataTable
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)
            txtComPer.Text = row("CommisionPer").ToString()
            txtMPer.Text = row("UserChargesPer").ToString()
            txtTare.Text = row("Tare").ToString()
            txtLabour.Text = row("Labour").ToString()
            txtRdfPer.Text = row("RDFPer").ToString()
            lblCrate.Text = row("MaintainCrate").ToString()
            CUT = row("CutPerNug").ToString()
            txtCut.Text = Val(row("CutPerNug").ToString())
            If ItemPer = "ItemWise" Then CbPer.Text = row("RateAs").ToString()
            trackStock = row("TrackStock").ToString()
        End If
        'AccountComm()
    End Sub
    Private Sub txtGrossWt_KeyUp(sender As Object, e As KeyEventArgs) Handles txtGrossWt.KeyUp
        If Val(txtGrossWt.Text) = 0 Then Exit Sub
        txtKg.Text = Format(Val(txtGrossWt.Text) - (Val(CUT) * Val(txtNug.Text)), "0.00")
    End Sub
   
    Public Sub FillControls(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.BackColor = Color.Coral
        BtnSave.Image = My.Resources.Edit
        BtnSave.Text = "&Update"
        BtnDelete.Enabled = True
        txtPurchaseType.Enabled = False
        'txtPurchaseType.Enabled = False
        txtStorage.Enabled = False
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        sSql = "Select * from Vouchers where id=" & id
        Dim sql As String = "Select * from Transaction2 where VoucherID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = Val(ds.Tables("a").Rows(0)("ID").ToString())
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtPurchaseTypeID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtPurchaseType.Text = clsFun.ExecScalarStr("Select AccountName from Accounts Where  ID='" & Val(ds.Tables("a").Rows(0)("SallerID").ToString()) & "'")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txtTotweight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtTotBasic.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txtTotCharge.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            TxtGrandTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txttotRoff.Text = Format(Val(TxtGrandTotal.Text) - Val(Val(txtTotBasic.Text) + Val(txtTotCharge.Text)), "0.00")
            txtStorage.Text = ds.Tables("a").Rows(0)("StorageName").ToString()
            txtStorageID.Text = ds.Tables("a").Rows(0)("StorageID").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
        End If
        Dim ds1 As New DataSet
        ad1.Fill(ds1, "b")
        If ds1.Tables("b").Rows.Count > 0 Then
            dg1.Rows.Clear()
            With dg1
                Dim i As Integer = 0
                For i = 0 To ds1.Tables("b").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("ItemID").Value = ds1.Tables("b").Rows(i)("ItemID").ToString()
                    .Rows(i).Cells("Item Name").Value = ds1.Tables("b").Rows(i)("ItemName").ToString()
                    .Rows(i).Cells("CustomerID").Value = ds1.Tables("b").Rows(i)("AccountID").ToString()
                    .Rows(i).Cells("Customer").Value = ds1.Tables("b").Rows(i)("AccountName").ToString()
                    .Rows(i).Cells("Lot/Veriety").Value = ds1.Tables("b").Rows(i)("Lot").ToString()
                    .Rows(i).Cells("Nug").Value = Format(Val(ds1.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                    .Rows(i).Cells("Kg").Value = Format(Val(ds1.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                    .Rows(i).Cells("Cut").Value = Format(Val(ds1.Tables("b").Rows(i)("Cut").ToString()), "0.00")
                    .Rows(i).Cells(8).Value = Format(Val(ds1.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                    .Rows(i).Cells(9).Value = Format(Val(ds1.Tables("b").Rows(i)("SRate").ToString()), "0.00")
                    .Rows(i).Cells("Per").Value = ds1.Tables("b").Rows(i)("Per").ToString()
                    .Rows(i).Cells("Basic").Value = Format(Val(ds1.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("Total").Value = Format(Val(ds1.Tables("b").Rows(i)("TotalAmount").ToString()), "0.00")
                    .Rows(i).Cells("Charges").Value = Format(Val(ds1.Tables("b").Rows(i)("Charges").ToString()), "0.00")
                    .Rows(i).Cells("Crate Y/N").Value = ds1.Tables("b").Rows(i)("MaintainCrate").ToString()
                    '.Rows(i).Cells("ColSallerPerItem").Value = ds1.Tables("b").Rows(i)("SallerAmt").ToString()
                    .Rows(i).Cells("Comm Per").Value = Format(Val(ds1.Tables("b").Rows(i)("CommPer").ToString()), "0.00")
                    .Rows(i).Cells("comm Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("CommAmt").ToString()), "0.00")
                    .Rows(i).Cells("User Charges").Value = Format(Val(ds1.Tables("b").Rows(i)("MPer").ToString()), "0.00")
                    .Rows(i).Cells("UC Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("MAmt").ToString()), "0.00")
                    .Rows(i).Cells("Rdf").Value = Format(Val(ds1.Tables("b").Rows(i)("RdfPer").ToString()), "0.00")
                    .Rows(i).Cells("Rdf Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("RdfAmt").ToString()), "0.00")
                    .Rows(i).Cells("Tare").Value = Format(Val(ds1.Tables("b").Rows(i)("Tare").ToString()), "0.00")
                    .Rows(i).Cells("Tare Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("TareAmt").ToString()), "0.00")
                    .Rows(i).Cells("Labour").Value = Format(Val(ds1.Tables("b").Rows(i)("Labour").ToString()), "0.00")
                    .Rows(i).Cells("Labour Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("LabourAmt").ToString()), "0.00")
                    .Rows(i).Cells("Crate ID").Value = ds1.Tables("b").Rows(i)("CrateID").ToString()
                    .Rows(i).Cells("Crate Name").Value = ds1.Tables("b").Rows(i)("Cratemarka").ToString()
                    .Rows(i).Cells("Crate Qty").Value = ds1.Tables("b").Rows(i)("CrateQty").ToString()
                    .Rows(i).Cells("SallerID").Value = ds1.Tables("b").Rows(i)("SallerID").ToString()
                    .Rows(i).Cells("SallerName").Value = ds1.Tables("b").Rows(i)("SallerName").ToString()
                    .Rows(i).Cells("PurchaseID").Value = ds1.Tables("b").Rows(i)("PurchaseID").ToString()
                    .Rows(i).Cells("SallerAmt").Value = ds1.Tables("b").Rows(i)("SallerAmt").ToString()
                    .Rows(i).Cells("roundoff").Value = ds1.Tables("b").Rows(i)("roundoff").ToString()
                    .Rows(i).Cells("CrateAccountID").Value = ds1.Tables("b").Rows(i)("CrateAccountID").ToString()
                    .Rows(i).Cells("CrateAccountName").Value = ds1.Tables("b").Rows(i)("CrateAccountName").ToString()
                    .Rows(i).Cells("AddWeight").Value = ds1.Tables("b").Rows(i)("Onweight").ToString()
                    .Rows(i).Cells(36).Value = Val(ds1.Tables("b").Rows(i)("GrossWeight").ToString())
                    .Rows(i).Cells(37).Value = i + 1
                    Dim SellerID As Integer = Val(clsFun.ExecScalarInt("Select StockHolderID From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                    Dim SellerName As String = clsFun.ExecScalarStr("Select StockHolderName From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                    Dim StorageID As Integer = Val(clsFun.ExecScalarInt("Select StorageID From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                    Dim StorageName As String = clsFun.ExecScalarStr("Select StorageName From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                    Dim typeac As String = IIf(SellerID = 28, "Purchase", "Stock in")
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & Val(i + 1) & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," &
                                "'" & typeac & "'," & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "," & Val(SellerID) & ",'" & SellerName & "', " &
                                "'" & Val(StorageID) & "','" & StorageName & "','" & Val(ds1.Tables("b").Rows(i)("ItemID").ToString()) & "','" & ds1.Tables("b").Rows(i)("ItemName").ToString() & "'," &
                                "'" & Val(0) & "', '" & ds1.Tables("b").Rows(i)("Lot").ToString() & "','" & Val(ds1.Tables("b").Rows(i)("Nug").ToString()) & "','" & Val(ds1.Tables("b").Rows(i)("Weight").ToString()) & "', " &
                                "'" & ds1.Tables("b").Rows(i)("Per").ToString() & "'," & Val(txtid.Text) & "," & Val(ds1.Tables("b").Rows(i)("GrossWeight").ToString()) & ""
                Next
            End With
            Try
                sql = "insert into Stock(ID,ENTRYDATE,TRANSTYPE,PURCHASETYPENAME,PurchaseID,SELLERID,SELLERNAME,STORAGEID,StorageName," _
                               & " ITEMID,ITEMNAME,CUT,LOT,NUG,WEIGHT,PER,TransID,GrossWeight) " & FastQuery & ""
                If clsFun.ExecNonQuery(sql) > 0 Then

                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
        calc() : dg1.ClearSelection()
        Offset = clsFun.ExecScalarInt("SELECT COUNT(*) AS RowPosition FROM Vouchers WHERE transtype ='Stock Sale' AND ID < " & Val(txtid.Text) & " ORDER BY ID DESC")
    End Sub


    Public Sub FillWithNevigation()
        If BtnSave.Text = "&Save" And dg1.RowCount > 0 Then MsgBox("Save Transaction First...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.BackColor = Color.Coral
        BtnSave.Image = My.Resources.Edit
        BtnSave.Text = "&Update"
        clsFun.ExecScalarStr("Delete From Stock Where TransType ='" & Me.Text & "'")
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Stock Sale'  Order By ID ")
        BtnDelete.Enabled = True
        'txtPurchaseType.Enabled = False
        'txtStorage.Enabled = False
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where transtype = 'Stock Sale'  Order By ID LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString() + ""
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)


        Dim ds As New DataSet : Dim ds1 As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtPurchaseTypeID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtPurchaseType.Text = clsFun.ExecScalarStr("Select AccountName from Accounts Where  ID='" & Val(ds.Tables("a").Rows(0)("SallerID").ToString()) & "'")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txtTotweight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtTotBasic.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txtTotCharge.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            TxtGrandTotal.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txttotRoff.Text = Format(Val(TxtGrandTotal.Text) - Val(Val(txtTotBasic.Text) + Val(txtTotCharge.Text)), "0.00")
            txtStorage.Text = ds.Tables("a").Rows(0)("StorageName").ToString()
            txtStorageID.Text = ds.Tables("a").Rows(0)("StorageID").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
        End If
        Dim sql As String = "Select * from Transaction2 where VoucherID=" & Val(txtid.Text)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        ad1.Fill(ds1, "b")
        If ds1.Tables("b").Rows.Count > 0 Then
            dg1.Rows.Clear()
            With dg1
                Dim i As Integer = 0
                For i = 0 To ds1.Tables("b").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("ItemID").Value = ds1.Tables("b").Rows(i)("ItemID").ToString()
                    .Rows(i).Cells("Item Name").Value = ds1.Tables("b").Rows(i)("ItemName").ToString()
                    .Rows(i).Cells("CustomerID").Value = ds1.Tables("b").Rows(i)("AccountID").ToString()
                    .Rows(i).Cells("Customer").Value = ds1.Tables("b").Rows(i)("AccountName").ToString()
                    .Rows(i).Cells("Lot/Veriety").Value = ds1.Tables("b").Rows(i)("Lot").ToString()
                    .Rows(i).Cells("Nug").Value = Format(Val(ds1.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                    .Rows(i).Cells("Kg").Value = Format(Val(ds1.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                    .Rows(i).Cells("Cut").Value = Format(Val(ds1.Tables("b").Rows(i)("Cut").ToString()), "0.00")
                    .Rows(i).Cells(8).Value = Format(Val(ds1.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                    .Rows(i).Cells(9).Value = Format(Val(ds1.Tables("b").Rows(i)("SRate").ToString()), "0.00")
                    .Rows(i).Cells("Per").Value = ds1.Tables("b").Rows(i)("Per").ToString()
                    .Rows(i).Cells("Basic").Value = Format(Val(ds1.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("Total").Value = Format(Val(ds1.Tables("b").Rows(i)("TotalAmount").ToString()), "0.00")
                    .Rows(i).Cells("Charges").Value = Format(Val(ds1.Tables("b").Rows(i)("Charges").ToString()), "0.00")
                    .Rows(i).Cells("Crate Y/N").Value = ds1.Tables("b").Rows(i)("MaintainCrate").ToString()
                    '.Rows(i).Cells("ColSallerPerItem").Value = ds1.Tables("b").Rows(i)("SallerAmt").ToString()
                    .Rows(i).Cells("Comm Per").Value = Format(Val(ds1.Tables("b").Rows(i)("CommPer").ToString()), "0.00")
                    .Rows(i).Cells("comm Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("CommAmt").ToString()), "0.00")
                    .Rows(i).Cells("User Charges").Value = Format(Val(ds1.Tables("b").Rows(i)("MPer").ToString()), "0.00")
                    .Rows(i).Cells("UC Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("MAmt").ToString()), "0.00")
                    .Rows(i).Cells("Rdf").Value = Format(Val(ds1.Tables("b").Rows(i)("RdfPer").ToString()), "0.00")
                    .Rows(i).Cells("Rdf Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("RdfAmt").ToString()), "0.00")
                    .Rows(i).Cells("Tare").Value = Format(Val(ds1.Tables("b").Rows(i)("Tare").ToString()), "0.00")
                    .Rows(i).Cells("Tare Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("TareAmt").ToString()), "0.00")
                    .Rows(i).Cells("Labour").Value = Format(Val(ds1.Tables("b").Rows(i)("Labour").ToString()), "0.00")
                    .Rows(i).Cells("Labour Amt").Value = Format(Val(ds1.Tables("b").Rows(i)("LabourAmt").ToString()), "0.00")
                    .Rows(i).Cells("Crate ID").Value = ds1.Tables("b").Rows(i)("CrateID").ToString()
                    .Rows(i).Cells("Crate Name").Value = ds1.Tables("b").Rows(i)("Cratemarka").ToString()
                    .Rows(i).Cells("Crate Qty").Value = ds1.Tables("b").Rows(i)("CrateQty").ToString()
                    .Rows(i).Cells("SallerID").Value = ds1.Tables("b").Rows(i)("SallerID").ToString()
                    .Rows(i).Cells("SallerName").Value = ds1.Tables("b").Rows(i)("SallerName").ToString()
                    .Rows(i).Cells("PurchaseID").Value = ds1.Tables("b").Rows(i)("PurchaseID").ToString()
                    .Rows(i).Cells("SallerAmt").Value = ds1.Tables("b").Rows(i)("SallerAmt").ToString()
                    .Rows(i).Cells("roundoff").Value = ds1.Tables("b").Rows(i)("roundoff").ToString()
                    .Rows(i).Cells("CrateAccountID").Value = ds1.Tables("b").Rows(i)("CrateAccountID").ToString()
                    .Rows(i).Cells("CrateAccountName").Value = ds1.Tables("b").Rows(i)("CrateAccountName").ToString()
                    .Rows(i).Cells("AddWeight").Value = ds1.Tables("b").Rows(i)("Onweight").ToString()
                    .Rows(i).Cells(36).Value = Val(ds1.Tables("b").Rows(i)("GrossWeight").ToString())
                    .Rows(i).Cells(37).Value = i + 1
                    Dim SellerID As Integer = Val(clsFun.ExecScalarInt("Select StockHolderID From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                    Dim SellerName As String = clsFun.ExecScalarStr("Select StockHolderName From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                    Dim StorageID As Integer = Val(clsFun.ExecScalarInt("Select StorageID From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'"))
                    Dim StorageName As String = clsFun.ExecScalarStr("Select StorageName From Purchase Where VoucherID='" & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "'")
                    Dim typeac As String = IIf(SellerID = 28, "Purchase", "Stock in")
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & Val(i + 1) & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," &
                                "'" & typeac & "'," & Val(ds1.Tables("b").Rows(i)("PurchaseID").ToString()) & "," & Val(SellerID) & ",'" & SellerName & "', " &
                                "'" & Val(StorageID) & "','" & StorageName & "','" & Val(ds1.Tables("b").Rows(i)("ItemID").ToString()) & "','" & ds1.Tables("b").Rows(i)("ItemName").ToString() & "'," &
                                "'" & Val(0) & "', '" & ds1.Tables("b").Rows(i)("Lot").ToString() & "','" & Val(ds1.Tables("b").Rows(i)("Nug").ToString()) & "','" & Val(ds1.Tables("b").Rows(i)("Weight").ToString()) & "', " &
                                "'" & ds1.Tables("b").Rows(i)("Per").ToString() & "'," & Val(txtid.Text) & "," & Val(ds1.Tables("b").Rows(i)("GrossWeight").ToString()) & ""
                Next
            End With
            Try
                sql = "insert into Stock(ID,ENTRYDATE,TRANSTYPE,PURCHASETYPENAME,PurchaseID,SELLERID,SELLERNAME,STORAGEID,StorageName," _
                               & " ITEMID,ITEMNAME,CUT,LOT,NUG,WEIGHT,PER,TransID,GrossWeight) " & FastQuery & ""
                If clsFun.ExecNonQuery(sql) > 0 Then

                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
        dg1.ClearSelection()
    End Sub

    Private Sub Delete()
        Dim RemoveSale As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
        If RemoveSale <> "Y" Then MsgBox("You Don't Have Rights to Delete Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : ClearAll() : Exit Sub
        Try
            If MessageBox.Show("Are you Sure to Delete This Bill...??", "Delete Bill", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dim sql As String = "DELETE from Ledger WHERE vourchersID=" & Val(txtid.Text) & ";DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "; " &
                                    "DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ""
                ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                             "Delete From  CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 0 : ServerStockSaleLedger() : ServerCrateLedger()
                If clsFun.ExecNonQuery(sql) > 0 Then
                    el.WriteToErrorLog(sql, Constants.compname, "Updated")
                    MsgBox("Record Deleted Successfully...", vbInformation + vbOKOnly, "Deleted") : ClearAll()
                    If Application.OpenForms().OfType(Of Stock_Sale_Register).Any = True Then Stock_Sale_Register.btnShow.PerformClick()
                    If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
                    If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
                    If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub txtTotalNetAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtTotalAmt.KeyDown
        If Val(txtPurchaseID.Text) = Val(0) Then MsgBox("Invallid Lot No.", MsgBoxStyle.Critical, "Invalid LOT No") : txtLot.Focus() : Exit Sub
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
            If Val(LotBal) < Val(txtNug.Text) Then
                MsgBox("Not Enough Nug", MsgBoxStyle.Critical, "Access Denied") : txtLot.Focus() : Exit Sub
            End If
        BalanceRecord()
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 1 Then
                dg1.SelectedRows(0).Cells(0).Value = Val(txtItemID.Text)
                dg1.SelectedRows(0).Cells(1).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(2).Value = Val(txtAccountID.Text)
                dg1.SelectedRows(0).Cells(3).Value = txtAccount.Text
                dg1.SelectedRows(0).Cells(4).Value = txtLot.Text
                dg1.SelectedRows(0).Cells(5).Value = Val(txtNug.Text)
                dg1.SelectedRows(0).Cells(6).Value = (txtKg.Text)
                dg1.SelectedRows(0).Cells(7).Value = txtCut.Text
                dg1.SelectedRows(0).Cells(8).Value = Val(txtCustomerRate.Text)
                dg1.SelectedRows(0).Cells(9).Value = Val(txtSallerRate.Text)
                dg1.SelectedRows(0).Cells(10).Value = Cbper.Text
                dg1.SelectedRows(0).Cells(11).Value = txtbasicAmt.Text
                dg1.SelectedRows(0).Cells(12).Value = txtTotalAmt.Text
                dg1.SelectedRows(0).Cells(13).Value = lbltotCharges.Text
                dg1.SelectedRows(0).Cells(14).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(15).Value = txtComPer.Text
                dg1.SelectedRows(0).Cells(16).Value = txtComAmt.Text
                dg1.SelectedRows(0).Cells(17).Value = txtMPer.Text
                dg1.SelectedRows(0).Cells(18).Value = txtMAmt.Text
                dg1.SelectedRows(0).Cells(19).Value = txtRdfPer.Text
                dg1.SelectedRows(0).Cells(20).Value = txtRdfAmt.Text
                dg1.SelectedRows(0).Cells(21).Value = txtTare.Text
                dg1.SelectedRows(0).Cells(22).Value = txtTareAmt.Text
                dg1.SelectedRows(0).Cells(23).Value = txtLabour.Text
                dg1.SelectedRows(0).Cells(24).Value = txtLaboutAmt.Text
                dg1.SelectedRows(0).Cells(25).Value = cbCrateMarka.SelectedValue
                dg1.SelectedRows(0).Cells(26).Value = cbCrateMarka.Text
                dg1.SelectedRows(0).Cells(27).Value = txtCrateQty.Text
                dg1.SelectedRows(0).Cells(28).Value = txtPurchaseTypeID.Text
                dg1.SelectedRows(0).Cells(29).Value = txtPurchaseType.Text
                dg1.SelectedRows(0).Cells(30).Value = txtPurchaseID.Text
                dg1.SelectedRows(0).Cells(31).Value = txtSallerAmout.Text
                dg1.SelectedRows(0).Cells(32).Value = lblRoundoff.Text
                dg1.SelectedRows(0).Cells(33).Value = cbAccountName.SelectedValue
                dg1.SelectedRows(0).Cells(34).Value = cbAccountName.Text
                dg1.SelectedRows(0).Cells(35).Value = txtAddWeight.Text
                dg1.SelectedRows(0).Cells(36).Value = Val(txtGrossWt.Text)
                dg1.SelectedRows(0).Cells(37).Value = Val(dg1.SelectedRows(0).Cells(37).Value)
                txtItem.Focus()
            Else
                If lblLot.Text <> "" Then
                    If lblCrate.Text = "Y" Then
                        dg1.Rows.Add(Val(txtItemID.Text), txtItem.Text, Val(txtAccountID.Text),
                                  txtAccount.Text, txtLot.Text, txtNug.Text, Val(txtKg.Text), txtCut.Text,
                                  Val(txtCustomerRate.Text), Val(txtSallerRate.Text), Cbper.Text, Val(txtbasicAmt.Text), Val(txtTotalAmt.Text),
                                  Val(lbltotCharges.Text), lblCrate.Text, Val(txtComPer.Text), Val(txtComAmt.Text), Val(txtMPer.Text), Val(txtMAmt.Text),
                                 (txtRdfPer.Text), Val(txtRdfAmt.Text), Val(txtTare.Text), Val(txtTareAmt.Text), Val(txtLabour.Text), Val(txtLaboutAmt.Text),
                                  Val(cbCrateMarka.SelectedValue), cbCrateMarka.Text, Val(txtCrateQty.Text), Val(txtPurchaseTypeID.Text), txtPurchaseType.Text,
                                  Val(txtPurchaseID.Text), Val(txtSallerAmout.Text), Val(lblRoundoff.Text), Val(cbAccountName.SelectedValue), cbAccountName.Text, txtAddWeight.Text, Val(txtGrossWt.Text), (dg1.Rows.Count) + 1)
                    Else
                        dg1.Rows.Add(Val(txtItemID.Text), txtItem.Text, Val(txtAccountID.Text),
                                          txtAccount.Text, txtLot.Text, txtNug.Text, Val(txtKg.Text), txtCut.Text,
                                          Val(txtCustomerRate.Text), Val(txtSallerRate.Text), Cbper.Text, Val(txtbasicAmt.Text), Val(txtTotalAmt.Text),
                                          Val(lbltotCharges.Text), lblCrate.Text, Val(txtComPer.Text), Val(txtComAmt.Text), Val(txtMPer.Text), Val(txtMAmt.Text),
                                         (txtRdfPer.Text), Val(txtRdfAmt.Text), Val(txtTare.Text), Val(txtTareAmt.Text), Val(txtLabour.Text), Val(txtLaboutAmt.Text),
                                         "0", "", "", Val(txtPurchaseTypeID.Text), txtPurchaseType.Text, Val(txtPurchaseID.Text), Val(txtSallerAmout.Text), Val(lblRoundoff.Text), "0", "", txtAddWeight.Text, Val(txtGrossWt.Text), (dg1.Rows.Count) + 1)
                    End If
                End If
            End If
            'ItemBalance() : LotBalance()
            lblItemBalance.Visible = False : lblLot.Visible = False
            calc() : txtclear() : dg1.ClearSelection()
        End If
    End Sub

    Private Sub txtCustomerRate_Leave(sender As Object, e As EventArgs) Handles txtCustomerRate.Leave
        If txtCustomerRate.Text = "" Then txtCustomerRate.Text = 0
        txtSallerRate.Text = txtCustomerRate.Text
        txtCustomerRate.SelectionStart = 0
    End Sub

    Private Sub txtbasicAmt_KeyDown(sender As Object, e As KeyEventArgs) Handles txtbasicAmt.KeyDown
        If e.KeyCode = Keys.F10 Then
            Dim basic As Decimal = Math.Round(Val(txtbasicAmt.Text))
            If Cbper.SelectedIndex = 0 Then
                txtSallerRate.Text = Val(basic) / Val(txtKg.Text)
                txtCustomerRate.Text = Val(basic) / Val(txtKg.Text)
                txtTotalAmt.Focus()
            ElseIf Cbper.SelectedIndex = 1 Then
                txtSallerRate.Text = Val(basic) / Val(txtKg.Text)
                txtCustomerRate.Text = Val(basic) / Val(txtKg.Text)
                txtTotalAmt.Focus()
            ElseIf Cbper.SelectedIndex = 2 Then
                txtSallerRate.Text = Val(basic) / Val(txtKg.Text)
                txtCustomerRate.Text = Val(basic) / Val(txtKg.Text)
                txtTotalAmt.Focus()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtKg.KeyPress, txtCut.KeyPress,
        txtbasicAmt.KeyPress, TxtGrandTotal.KeyPress, txtCustomerRate.KeyPress, txtSallerRate.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave
        txtNug.SelectionLength = 0
        If lblCrate.Text = "Y" Then
            txtCrateQty.Text = txtNug.Text
        Else
            txtCrateQty.Text = Val(0)
            cbCrateMarka.SelectedValue = Val(0)
            cbCrateMarka.Text = ""
        End If
        If txtNug.Text = "" Then txtNug.Text = 0
        If trackStock = "Nug" Then
            If txtNug.Text > Val(LotBal) Then
                MsgBox("Not Enough Nugs. Please Choose Another Item / Lot ", MsgBoxStyle.Critical, "Zero")
                txtNug.Text = 0 : txtLot.Focus() : Exit Sub
            End If
        End If
    

    End Sub

    Private Sub txtSallerRate_Leave(sender As Object, e As EventArgs) Handles txtSallerRate.Leave
        txtSallerRate.SelectionStart = 0
    End Sub

    Private Sub txtComPer_KeyDown(sender As Object, e As KeyEventArgs) Handles txtComPer.KeyDown, txtMPer.KeyDown, txtTare.KeyDown, txtLabour.KeyDown, txtRdfPer.KeyDown,
        txtComAmt.KeyDown, txtMAmt.KeyDown, txtTareAmt.KeyDown, txtLaboutAmt.KeyDown, txtRdfAmt.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                BtnSave.Focus()
        End Select
    End Sub

    Private Sub txtrate_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerRate.KeyUp, txtSallerRate.KeyUp, txtNug.KeyUp,
         txtKg.KeyUp, txtComPer.KeyUp, txtComAmt.KeyUp, txtMPer.KeyUp, txtMAmt.KeyUp,
        txtLabour.KeyUp, txtLaboutAmt.KeyUp, Cbper.KeyUp, txtTare.KeyUp, txtNetRate.KeyUp, txtRdfPer.KeyUp,
        txtRdfAmt.KeyUp, txtCrateQty.KeyUp
        StockCalculation()
    End Sub
    Private Sub CbPer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Cbper.SelectedIndexChanged
        StockCalculation()
    End Sub
    Private Sub rowColums()
        ' Application.DoEvents()
        dg1.ColumnCount = 38
        dg1.Columns(0).Name = "ItemID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Item Name" : dg1.Columns(1).Width = 149 : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(2).Name = "CustomerID" : dg1.Columns(2).Visible = False
        dg1.Columns(3).Name = "Customer" : dg1.Columns(3).Width = 238 : dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).Name = "Lot/Veriety" : dg1.Columns(4).Width = 109 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).Name = "Nug" : dg1.Columns(5).Width = 79 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(6).Name = "Kg" : dg1.Columns(6).Width = 79 : dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(7).Name = "Cut" : dg1.Columns(7).Width = 79 : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(8).Name = "PRate" : dg1.Columns(8).Width = 79 : dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(9).Name = "SRate" : dg1.Columns(9).Width = 79 : dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(10).Name = "Per" : dg1.Columns(10).Width = 80 : dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(11).Name = "Basic" : dg1.Columns(11).Width = 99 : dg1.Columns(11).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(12).Name = "Total" : dg1.Columns(12).Width = 99 : dg1.Columns(12).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(13).Name = "Charges" : dg1.Columns(13).Width = 150
        dg1.Columns(14).Name = "Crate Y/N" : dg1.Columns(14).Width = 150
        dg1.Columns(15).Name = "Comm Per" : dg1.Columns(15).Width = 150
        dg1.Columns(16).Name = "comm Amt" : dg1.Columns(16).Width = 150
        dg1.Columns(17).Name = "User Charges" : dg1.Columns(17).Width = 200
        dg1.Columns(18).Name = "UC Amt" : dg1.Columns(18).Width = 150
        dg1.Columns(19).Name = "Rdf" : dg1.Columns(19).Width = 150
        dg1.Columns(20).Name = "Rdf Amt" : dg1.Columns(20).Width = 150
        dg1.Columns(21).Name = "Tare" : dg1.Columns(21).Width = 150
        dg1.Columns(22).Name = "Tare Amt" : dg1.Columns(22).Width = 200
        dg1.Columns(23).Name = "Labour" : dg1.Columns(23).Width = 150
        dg1.Columns(24).Name = "Labour Amt" : dg1.Columns(24).Width = 200
        dg1.Columns(25).Name = "Crate ID" : dg1.Columns(25).Width = 200
        dg1.Columns(26).Name = "Crate Name" : dg1.Columns(26).Width = 200
        dg1.Columns(27).Name = "Crate Qty" : dg1.Columns(27).Width = 200
        dg1.Columns(28).Name = "SallerID" : dg1.Columns(28).Width = 200
        dg1.Columns(29).Name = "SallerName" : dg1.Columns(29).Width = 200
        dg1.Columns(30).Name = "PurchaseID" : dg1.Columns(30).Width = 200
        dg1.Columns(31).Name = "SallerAmt" : dg1.Columns(30).Width = 200
        dg1.Columns(32).Name = "RoundOff" : dg1.Columns(30).Width = 200
        dg1.Columns(33).Name = "CrateAccountID" : dg1.Columns(33).Width = 200
        dg1.Columns(34).Name = "CrateAccountName" : dg1.Columns(34).Width = 200
        dg1.Columns(35).Name = "AddWeight" : dg1.Columns(35).Width = 200
        dg1.Columns(36).Name = "RecordID" : dg1.Columns(36).Width = 200
        dg1.Columns(37).Name = "RecordID" : dg1.Columns(37).Width = 200

    End Sub
    Private Sub StockCalculation()
        If ckTaxPaid.Checked = True Then
            Dim TotTaxPer As Decimal = Format(Val(txtComPer.Text) + Val(txtMPer.Text) + Val(txtRdfPer.Text) + 100, "0.00")
            Dim incRate As Decimal = Val(txtNetRate.Text) * (100 / TotTaxPer)
            txtCustomerRate.Text = Format(Val(incRate), "0.00")
            If Cbper.SelectedIndex = 0 Then
                txtbasicAmt.Text = Format(Val(txtNug.Text) * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 1 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 2 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 5 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 5 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 3 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 10 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 10 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 4 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 20 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 20 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 5 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 40 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 40 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 6 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 41 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 41 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 7 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 50 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 50 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 8 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 51 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 51 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 9 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 51.7 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 51.7 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 10 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 52.2 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 52.5 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 11 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 52.3 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 52.3 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 12 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 52.5 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 52.5 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 13 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 53 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 53 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 14 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 80 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 80 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 15 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 100 * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 100 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 16 Then
                txtbasicAmt.Text = Format(Val(txtNug.Text) * Val(incRate), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            End If
            If clsFun.ExecScalarStr("Select Octroi From Controls") = "Yes" Then
                txtComAmt.Text = Format(Val(txtComPer.Text) * Val(Val(txtbasicAmt.Text) + Val(txtLaboutAmt.Text)) / 100, "0.00")
            Else
                txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            End If
            If CalculationMethod = "Nug" Then
                txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtNug.Text), "0.00")
            Else
                txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            End If
            '  txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            ' txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            If crateRate = "RDF" AndAlso lblCrate.Text = "Y" Then txtRdfAmt.Text = Format(Val(txtCrateQty.Text) * Val(txtRdfPer.Text), "0.00") Else txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            If clsFun.ExecScalarStr("Select ApplyCommWeight From Controls") = "Yes" Then
                txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtKg.Text), "0.00")
            Else
                If crateRate = "Yes" And lblCrate.Text = "Y" Then txtTareAmt.Text = Format(Val(txtCrateQty.Text) * Val(txtTare.Text), "0.00") Else txtTareAmt.Text = Format(Val(txtNug.Text) * Val(txtTare.Text), "0.00")
            End If
            txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
            lbltotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
            txtTotalAmt.Text = Format(Val(lbltotCharges.Text) + Val(txtbasicAmt.Text), "0.00")
        Else
            If Cbper.SelectedIndex = 0 Then
                txtbasicAmt.Text = Format(Val(txtNug.Text) * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 1 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 2 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 5 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 5 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 3 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 10 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 10 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 4 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 20 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 20 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 5 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 40 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 40 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 6 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 41 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 41 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 7 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 50 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 50 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 8 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 51 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 51 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 9 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 51.7 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 51.7 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 10 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 52.2 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 52.2 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 11 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 52.3 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 52.3 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 12 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 52.5 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 52.5 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 13 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 53 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 53 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 14 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 80 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 80 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 15 Then
                txtbasicAmt.Text = Format(Val(txtKg.Text) / 100 * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtKg.Text) / 100 * Val(txtSallerRate.Text), "0.00")
            ElseIf Cbper.SelectedIndex = 16 Then
                txtbasicAmt.Text = Format(Val(txtNug.Text) * Val(txtCustomerRate.Text), "0.00")
                txtSallerAmout.Text = Format(Val(txtNug.Text) * Val(txtSallerRate.Text), "0.00")
            End If
            If clsFun.ExecScalarStr("Select Octroi From Controls") = "Yes" Then
                txtComAmt.Text = Format(Val(txtComPer.Text) * Val(Val(txtbasicAmt.Text) + Val(txtLaboutAmt.Text)) / 100, "0.00")
            Else
                txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            End If
            If CalculationMethod = "Nug" Then
                txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            Else
                txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            End If
            '   txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            If crateRate = "RDF" AndAlso lblCrate.Text = "Y" Then txtRdfAmt.Text = Format(Val(txtCrateQty.Text) * Val(txtRdfPer.Text), "0.00") Else txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
            If clsFun.ExecScalarStr("Select ApplyCommWeight From Controls") = "Yes" Then
                txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtKg.Text), "0.00")
            Else
                If crateRate = "Yes" AndAlso lblCrate.Text = "Y" Then txtTareAmt.Text = Format(Val(txtCrateQty.Text) * Val(txtTare.Text), "0.00") Else txtTareAmt.Text = Format(Val(txtNug.Text) * Val(txtTare.Text), "0.00")
            End If
            txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
            lbltotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
            txtTotalAmt.Text = Format(Val(lbltotCharges.Text) + Val(txtbasicAmt.Text), "0.00")
        End If
        If clsFun.ExecScalarStr("Select ROEachItem From Controls") = "No" Then
            Dim tmpCustAmount As Double = Val(txtTotalAmt.Text)
            txtTotalAmt.Text = Math.Round(Val(tmpCustAmount), 0, MidpointRounding.AwayFromZero)
            ' txtTotalAmt.Text = Math.Round(Val(tmpCustAmount), 0)
            lblRoundoff.Text = Format(Math.Round(Val(txtTotalAmt.Text) - Val(tmpCustAmount), 2), "0.00")
            txtTotalAmt.Text = Format(Val(txtTotalAmt.Text), "0.00")
        End If

    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        DgAccountSearch.Visible = False : dgItemSearch.Visible = False
        dgStore.Visible = False : pnlMarka.Visible = False : dgLot.Visible = False
        If dg1.RowCount = 0 Or dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItemID.Text = Val(dg1.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dg1.SelectedRows(0).Cells(1).Value
        txtAccountID.Text = dg1.SelectedRows(0).Cells(2).Value
        txtAccount.Text = dg1.SelectedRows(0).Cells(3).Value
        txtLot.Text = dg1.SelectedRows(0).Cells(4).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(5).Value
        txtKg.Text = dg1.SelectedRows(0).Cells(6).Value
        txtCut.Text = dg1.SelectedRows(0).Cells(7).Value
        txtCustomerRate.Text = dg1.SelectedRows(0).Cells(8).Value
        txtSallerRate.Text = dg1.SelectedRows(0).Cells(9).Value
        Cbper.Text = dg1.SelectedRows(0).Cells(10).Value
        txtbasicAmt.Text = dg1.SelectedRows(0).Cells(11).Value
        txtTotalAmt.Text = dg1.SelectedRows(0).Cells(12).Value
        lbltotCharges.Text = dg1.SelectedRows(0).Cells(13).Value
        lblCrate.Text = dg1.SelectedRows(0).Cells(14).Value
        txtComPer.Text = dg1.SelectedRows(0).Cells(15).Value
        txtComAmt.Text = dg1.SelectedRows(0).Cells(16).Value
        txtMPer.Text = dg1.SelectedRows(0).Cells(17).Value
        txtMAmt.Text = dg1.SelectedRows(0).Cells(18).Value
        txtRdfPer.Text = dg1.SelectedRows(0).Cells(19).Value
        txtRdfAmt.Text = dg1.SelectedRows(0).Cells(20).Value
        txtTare.Text = dg1.SelectedRows(0).Cells(21).Value
        txtTareAmt.Text = dg1.SelectedRows(0).Cells(22).Value
        txtLabour.Text = dg1.SelectedRows(0).Cells(23).Value
        txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(24).Value
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(26).Value
        txtCrateQty.Text = dg1.SelectedRows(0).Cells(27).Value
        txtPurchaseTypeID.Text = Val(dg1.SelectedRows(0).Cells(28).Value)
        txtPurchaseType.Text = dg1.SelectedRows(0).Cells(29).Value
        txtPurchaseID.Text = (dg1.SelectedRows(0).Cells(30).Value)
        txtSallerAmout.Text = dg1.SelectedRows(0).Cells(31).Value
        lblRoundoff.Text = Val(dg1.SelectedRows(0).Cells(32).Value)
        cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(33).Value)
        cbAccountName.Text = dg1.SelectedRows(0).Cells(34).Value
        txtAddWeight.Text = dg1.SelectedRows(0).Cells(35).Value
        txtGrossWt.Text = dg1.SelectedRows(0).Cells(36).Value
        StockCalculation() ': txtItem.Focus()
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        DgAccountSearch.Visible = False : dgItemSearch.Visible = False
        dgStore.Visible = False : pnlMarka.Visible = False : dgLot.Visible = False
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Or dg1.SelectedRows.Count = 0 Then Exit Sub
            txtItemID.Text = dg1.SelectedRows(0).Cells(0).Value
            txtItem.Text = dg1.SelectedRows(0).Cells(1).Value
            txtAccountID.Text = dg1.SelectedRows(0).Cells(2).Value
            txtAccount.Text = dg1.SelectedRows(0).Cells(3).Value
            txtLot.Text = dg1.SelectedRows(0).Cells(4).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(5).Value
            txtKg.Text = dg1.SelectedRows(0).Cells(6).Value
            txtCut.Text = dg1.SelectedRows(0).Cells(7).Value
            txtCustomerRate.Text = dg1.SelectedRows(0).Cells(8).Value
            txtSallerRate.Text = dg1.SelectedRows(0).Cells(9).Value
            Cbper.Text = dg1.SelectedRows(0).Cells(10).Value
            txtbasicAmt.Text = dg1.SelectedRows(0).Cells(11).Value
            txtTotalAmt.Text = dg1.SelectedRows(0).Cells(12).Value
            lbltotCharges.Text = dg1.SelectedRows(0).Cells(13).Value
            lblCrate.Text = dg1.SelectedRows(0).Cells(14).Value
            txtComPer.Text = dg1.SelectedRows(0).Cells(15).Value
            txtComAmt.Text = dg1.SelectedRows(0).Cells(16).Value
            txtMPer.Text = dg1.SelectedRows(0).Cells(17).Value
            txtMAmt.Text = dg1.SelectedRows(0).Cells(18).Value
            txtRdfPer.Text = dg1.SelectedRows(0).Cells(19).Value
            txtRdfAmt.Text = dg1.SelectedRows(0).Cells(20).Value
            txtTare.Text = dg1.SelectedRows(0).Cells(21).Value
            txtTareAmt.Text = dg1.SelectedRows(0).Cells(22).Value
            txtLabour.Text = dg1.SelectedRows(0).Cells(23).Value
            txtLaboutAmt.Text = dg1.SelectedRows(0).Cells(24).Value
            cbCrateMarka.Text = dg1.SelectedRows(0).Cells(26).Value
            txtCrateQty.Text = dg1.SelectedRows(0).Cells(27).Value
            txtPurchaseTypeID.Text = Val(dg1.SelectedRows(0).Cells(28).Value)
            txtPurchaseType.Text = dg1.SelectedRows(0).Cells(29).Value
            txtPurchaseID.Text = dg1.SelectedRows(0).Cells(30).Value
            txtSallerAmout.Text = dg1.SelectedRows(0).Cells(31).Value
            lblRoundoff.Text = Val(dg1.SelectedRows(0).Cells(32).Value)
            cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(33).Value)
            cbAccountName.Text = dg1.SelectedRows(0).Cells(34).Value
            txtAddWeight.Text = dg1.SelectedRows(0).Cells(35).Value
            e.SuppressKeyPress = True : StockCalculation() : txtItem.Focus()
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
        If dg1.SelectedRows.Count = 1 Then
            If e.KeyCode = Keys.Delete Then
                If MessageBox.Show("Are you Sure to Remove Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                    ItemBalance()
                    If dg1.SelectedRows.Count = 0 Then Exit Sub
                    clsFun.ExecScalarStr("Delete From Stock Where ID='" & Val(dg1.SelectedRows(0).Cells(36).Value) & "'")
                    dg1.Rows.Remove(dg1.SelectedRows(0))
                    calc()
                    dg1.ClearSelection()
                    e.SuppressKeyPress = True
                Else
                    e.SuppressKeyPress = True : Exit Sub
                End If
            End If
        End If
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotBasic.Text = Format(0, "0.00")
        txtTotCharge.Text = Format(0, "0.00") : TxtGrandTotal.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
        Next
        txttotRoff.Text = Format(Val(TxtGrandTotal.Text) - Val(Val(txtTotBasic.Text) + Val(txtTotCharge.Text)), "0.00")
        If dg1.Rows.Count = 0 Then Exit Sub
        dg1.FirstDisplayedScrollingRowIndex = Val(dg1.RowCount - 1)
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If btnClose.Enabled = False Then Exit Sub
        If dg1.Rows.Count = 0 Then Me.Close() : Exit Sub
        If MessageBox.Show("Are you Sure Want to Exit Stock Sale ???", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Sub txtclear()
        txtItem.Focus() : txtPurchaseID.Text = ""
        txtNug.Text = "" : txtKg.Text = ""
        txtComPer.Text = "" : txtMPer.Text = ""
        txtCut.Text = "" 'txtSallerRate.Text = ""
        txtAddWeight.Text = "" : lblAddWeight.Visible = False
        txtbasicAmt.Text = "" : txtTotalAmt.Text = ""
        txtComAmt.Text = "0" : txtMAmt.Text = "0"
        txtRdfAmt.Text = "0" : txtTare.Text = "0"
        txtLabour.Text = "0" : txtCrateQty.Text = ""
        lbltotCharges.Text = "" : txtSallerAmout.Text = ""
        cbAccountName.SelectedValue = 0 : cbAccountName.Text = ""
        txtGrossWt.Text = ""
    End Sub
    Private Sub Save()
        '    VNumber()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        ' Change the format:
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        dg1.ClearSelection()
        Dim cmd As SQLite.SQLiteCommand
        Dim UserID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "insert into Vouchers(TransType,BillNo, Entrydate, " _
                                    & "SallerID, SallerName, Nug, kg,BasicAmount, TotalAmount,TotalCharges,StorageID,Storagename,InvoiceID,UserID,EntryTime)" _
                                    & "values (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10,@11,@12,@13,@14,@15)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@3", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@4", Val(txtPurchaseTypeID.Text))
            cmd.Parameters.AddWithValue("@5", txtPurchaseType.Text)
            cmd.Parameters.AddWithValue("@6", Val(txtTotNug.Text))
            cmd.Parameters.AddWithValue("@7", Val(txtTotweight.Text))
            cmd.Parameters.AddWithValue("@8", Val(txtTotBasic.Text))
            cmd.Parameters.AddWithValue("@9", Val(TxtGrandTotal.Text))
            cmd.Parameters.AddWithValue("@10", Val(txtTotCharge.Text))
            cmd.Parameters.AddWithValue("@11", Val(txtStorageID.Text))
            cmd.Parameters.AddWithValue("@12", txtStorage.Text)
            cmd.Parameters.AddWithValue("@13", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@14", Val(UserID))
            cmd.Parameters.AddWithValue("@15", Now.ToString("yyyy-MM-dd HH:mm:ss"))
            If cmd.ExecuteNonQuery() > 0 Then
                el.WriteToErrorLog(sql, Constants.compname, "Stock Sale Saved")
                clsFun.CloseConnection()
            End If
            txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
            ServerTag = 1 : Dg1Record() : StockSaleLedger() : CrateLedger() : ServerStockSaleLedger() : ServerCrateLedger()
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
            ClearAll()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub CrateLedger()
        Dim fastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                ' If Val(.Cells(33).Value) > 0 Then
                If Val(.Cells(27).Value) > 0 Then ''Party Account
                    If Val(.Cells(2).Value) = 7 Then
                        If Val(.Cells(33).Value) > 0 Then
                            ' If row.Index <> 0 Then FastQuery = FastQuery & " UNION ALL SELECT "
                            If crateRate = "RDF" Then
                                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(33).Value) & ",'" & .Cells(34).Value & "','Crate Out'," & Val(.Cells(25).Value) & ",'" & .Cells(26).Value & "','" & .Cells(27).Value & "', '','" & .Cells(19).Value & "','" & .Cells(20).Value & "', ''"
                            Else
                                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(33).Value) & ",'" & .Cells(34).Value & "','Crate Out'," & Val(.Cells(25).Value) & ",'" & .Cells(26).Value & "','" & .Cells(27).Value & "', '','" & .Cells(21).Value & "','" & .Cells(22).Value & "', ''"
                            End If

                        End If
                    Else
                        If crateRate = "RDF" Then
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(23).Value) & ",'" & .Cells(34).Value & "','Crate Out'," & Val(.Cells(25).Value) & ",'" & .Cells(26).Value & "','" & .Cells(27).Value & "', '','" & .Cells(19).Value & "','" & .Cells(20).Value & "', ''"
                        Else
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ",'" & .Cells(3).Value & "','Crate Out'," & Val(.Cells(25).Value) & ",'" & .Cells(26).Value & "','" & .Cells(27).Value & "', '','" & .Cells(21).Value & "','" & .Cells(22).Value & "', ''"
                        End If
                        'If row.Index <> 0 Then FastQuery = FastQuery & " UNION ALL SELECT "
                        '  clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, .Cells(2).Value, .Cells(3).Value, "Crate Out", Val(.Cells(25).Value), .Cells(26).Value, .Cells(27).Value, "", .Cells(21).Value, .Cells(22).Value, "")
                        '   fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ",'" & .Cells(3).Value & "','Crate Out'," & Val(.Cells(25).Value) & ",'" & .Cells(26).Value & "','" & .Cells(27).Value & "', '','" & .Cells(21).Value & "','" & .Cells(22).Value & "', ''"
                    End If
                End If
                ' End If
            End With
        Next
        If fastQuery = String.Empty Then Exit Sub
        clsFun.FastCrateLedger(fastQuery)
    End Sub
    Private Sub ServerCrateLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim fastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                ' If Val(.Cells(33).Value) > 0 Then
                If Val(.Cells(27).Value) > 0 Then ''Party Account
                    If Val(.Cells(2).Value) = 7 Then
                        If Val(.Cells(33).Value) > 0 Then
                            ' If row.Index <> 0 Then FastQuery = FastQuery & " UNION ALL SELECT "
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(33).Value) & ",'" & .Cells(34).Value & "','Crate Out'," & Val(.Cells(25).Value) & ",'" & .Cells(26).Value & "','" & .Cells(27).Value & "', '','" & .Cells(21).Value & "','" & .Cells(22).Value & "', '','" & Val(ServerTag) & "','" & Val(OrgID) & "'"
                        End If
                    Else
                        'If row.Index <> 0 Then FastQuery = FastQuery & " UNION ALL SELECT "
                        '  clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, .Cells(2).Value, .Cells(3).Value, "Crate Out", Val(.Cells(25).Value), .Cells(26).Value, .Cells(27).Value, "", .Cells(21).Value, .Cells(22).Value, "")
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ",'" & .Cells(3).Value & "','Crate Out'," & Val(.Cells(25).Value) & ",'" & .Cells(26).Value & "','" & .Cells(27).Value & "', '','" & .Cells(21).Value & "','" & .Cells(22).Value & "', '','" & Val(ServerTag) & "','" & Val(OrgID) & "'"
                    End If
                End If
                ' End If
            End With
        Next
        If fastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(fastQuery)
    End Sub

    'Private Sub Dg1Record()
    '    Dim cmd As SQLite.SQLiteCommand
    '    Dim cmd1 As SQLite.SQLiteCommand
    '    Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
    '    Dim typeac As String = IIf(txtPurchaseTypeID.Text = 28, "Purchase", "Stock in")
    '    Dim sql As String = String.Empty : Dim ssql As String = String.Empty
    '    Dim i As Integer
    '    For i = 0 To dg1.Rows.Count - 1
    '        With dg1.Rows(i)
    '            If .Cells("Item Name").Value <> "" Then
    '                sql = sql & "insert into Transaction2(EntryDate,BillNo,VoucherID,TransType,AccountID,AccountName,ItemID,ItemName,Lot,Nug,Weight, " _
    '                    & " Cut,Rate,SRate, Per,Amount,Charges,TotalAmount,CommPer,CommAmt,MPer,MAmt,RdfPer,RdfAmt," _
    '                    & "Tare,TareAmt,Labour, LabourAmt,CrateID,Cratemarka,CrateQty, MaintainCrate,PurchaseTypename,SallerID,SallerName,PurchaseID, " _
    '                    & "SallerAmt,RoundOff,CrateAccountID,CrateAccountName,StorageID,StorageName,OnWeight) " _
    '                    & "values('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & txtVoucherNo.Text & "'," & Val(txtid.Text) & ", '" & Me.Text & "'," & _
    '                         "'" & .Cells("CustomerID").Value & "','" & .Cells("Customer").Value & "'," & Val(.Cells("ItemID").Value) & "," & _
    '                         "'" & .Cells("Item Name").Value & "','" & .Cells("Lot/Veriety").Value & "', " & _
    '                         " '" & .Cells("Nug").Value & "','" & Val(.Cells("kg").Value) & "','" & .Cells("Cut").Value & "','" & Val(.Cells(8).Value) & "'," & _
    '                         " '" & .Cells(9).Value & "','" & .Cells("per").Value & "','" & Val(.Cells("Basic").Value) & "'," & _
    '                         "'" & Val(.Cells("Charges").Value) & "'," & _
    '                         " '" & .Cells("Total").Value & "','" & Val(.Cells("Comm Per").Value) & "'," & _
    '                         " '" & .Cells("comm Amt").Value & "','" & Val(.Cells("User Charges").Value) & "','" & Val(.Cells("UC Amt").Value) & "'," & _
    '                         " '" & .Cells("Rdf").Value & "','" & Val(.Cells("Rdf Amt").Value) & "','" & Val(.Cells("Tare").Value) & "'," & _
    '                         " '" & .Cells("Tare Amt").Value & "','" & Val(.Cells("Labour").Value) & "','" & Val(.Cells("Labour Amt").Value) & "'," & _
    '                         " '" & .Cells("Crate ID").Value & "','" & .Cells("Crate Name").Value & "','" & Val(.Cells("Crate Qty").Value) & "'," & _
    '                         "'" & .Cells("Crate Y/N").Value & "','" & typeac & "'," & Val(txtPurchaseTypeID.Text) & ",'" & txtPurchaseType.Text & "'," & _
    '                         "'" & Val(.Cells("PurchaseID").Value) & "','" & Val(.Cells("SallerAmt").Value) & "','" & Val(.Cells("RoundOff").Value) & "' ," & _
    '                           "'" & Val(.Cells(33).Value) & "','" & .Cells(34).Value & "','" & Val(txtStorageID.Text) & "','" & txtStorage.Text & "','" & .Cells(35).Value & "');"
    '                            End If
    '        End With
    '    Next
    '    Try
    '        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
    '        'cmd1 = New SQLite.SQLiteCommand(ssql, ClsFunserver.GetConnection())
    '        If cmd.ExecuteNonQuery() > 0 Then el.WriteToErrorLog(sql, Constants.compname, "Stock Sale Record") : count = +1
    '        ' cmd1.ExecuteNonQuery()
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '        clsFun.CloseConnection()
    '    End Try
    '    ' ProgressBar1.Visible = False
    '    clsFun.CloseConnection()
    'End Sub
    Private Sub BalanceRecord()
        If dg1.SelectedRows.Count <> 0 Then clsFun.ExecScalarStr("Delete From Stock Where ID='" & Val(dg1.SelectedRows(0).Cells(36).Value) & "'")
        Dim FastQuery As String = String.Empty
        Dim RecordCount As Integer = If(dg1.SelectedRows.Count = 1, Val(dg1.SelectedRows(0).Cells(36).Value), (dg1.Rows.Count) + 1)
        Dim Sql As String = String.Empty
        Dim typeac As String = IIf(txtPurchaseTypeID.Text = 28, "Purchase", "Stock in")
        FastQuery = " SELECT '" & Val(RecordCount) & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & _
            "'" & typeac & "'," & Val(txtPurchaseID.Text) & "," & Val(txtPurchaseTypeID.Text) & ",'" & txtPurchaseType.Text & "', " & _
            "'" & Val(txtStorageID.Text) & "','" & txtStorage.Text & "','" & Val(txtItemID.Text) & "','" & txtItem.Text & "'," & _
            "'" & Val(txtCut.Text) & "', '" & txtLot.Text & "','" & Val(txtNug.Text) & "','" & Val(txtKg.Text) & "', " & _
            "'" & Cbper.Text & "'," & Val(txtid.Text) & ""
        Try
            Sql = "insert into Stock(ID,ENTRYDATE,TRANSTYPE,PURCHASETYPENAME,PurchaseID,SELLERID,SELLERNAME,STORAGEID,StorageName," _
                           & " ITEMID,ITEMNAME,CUT,LOT,NUG,WEIGHT,PER,TransID) " & FastQuery & ""
            If clsFun.ExecNonQuery(Sql) > 0 Then el.WriteToErrorLog(Sql, Constants.compname, "Stock Sale Record") : count = +1
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        clsFun.CloseConnection()
    End Sub

    Private Sub Dg1Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim typeac As String = IIf(txtPurchaseTypeID.Text = 28, "Purchase", "Stock in")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                If .Cells("Item Name").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & txtVoucherNo.Text & "'," & Val(txtid.Text) & ", '" & Me.Text & "'," &
                                "'" & .Cells("CustomerID").Value & "','" & .Cells("Customer").Value & "'," & Val(.Cells("ItemID").Value) & "," &
                                "'" & .Cells("Item Name").Value & "','" & .Cells("Lot/Veriety").Value & "', " &
                                " '" & .Cells("Nug").Value & "','" & Val(.Cells("kg").Value) & "','" & Val(.Cells("Cut").Value) & "','" & Val(.Cells(8).Value) & "'," &
                                " '" & .Cells(9).Value & "','" & .Cells("per").Value & "','" & Val(.Cells("Basic").Value) & "'," &
                                "'" & Val(.Cells("Charges").Value) & "'," &
                                " '" & .Cells("Total").Value & "','" & Val(.Cells("Comm Per").Value) & "'," &
                                " '" & .Cells("comm Amt").Value & "','" & Val(.Cells("User Charges").Value) & "','" & Val(.Cells("UC Amt").Value) & "'," &
                                " '" & .Cells("Rdf").Value & "','" & Val(.Cells("Rdf Amt").Value) & "','" & Val(.Cells("Tare").Value) & "'," &
                                " '" & .Cells("Tare Amt").Value & "','" & Val(.Cells("Labour").Value) & "','" & Val(.Cells("Labour Amt").Value) & "'," &
                                " '" & .Cells("Crate ID").Value & "','" & .Cells("Crate Name").Value & "','" & Val(.Cells("Crate Qty").Value) & "'," &
                                "'" & .Cells("Crate Y/N").Value & "','" & typeac & "'," & Val(txtPurchaseTypeID.Text) & ",'" & txtPurchaseType.Text & "'," &
                                "'" & Val(.Cells("PurchaseID").Value) & "','" & Val(.Cells("SallerAmt").Value) & "','" & Val(.Cells("RoundOff").Value) & "' ," &
                                "'" & Val(.Cells(33).Value) & "','" & .Cells(34).Value & "','" & Val(txtStorageID.Text) & "','" & txtStorage.Text & "','" & .Cells(35).Value & "','" & Val(.Cells(36).Value) & "'"
                End If
            End With
        Next
        Try
            Sql = "insert into Transaction2(EntryDate,BillNo,VoucherID,TransType,AccountID,AccountName,ItemID,ItemName,Lot,Nug,Weight, " _
                           & " Cut,Rate,SRate, Per,Amount,Charges,TotalAmount,CommPer,CommAmt,MPer,MAmt,RdfPer,RdfAmt," _
                           & "Tare,TareAmt,Labour, LabourAmt,CrateID,Cratemarka,CrateQty, MaintainCrate,PurchaseTypename,SallerID,SallerName,PurchaseID, " _
                           & "SallerAmt,RoundOff,CrateAccountID,CrateAccountName,StorageID,StorageName,OnWeight,GrossWeight)" & FastQuery & ""
            If clsFun.ExecNonQuery(Sql) > 0 Then el.WriteToErrorLog(Sql, Constants.compname, "Stock Sale Record") : count = +1
            ' cmd1.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        ' ProgressBar1.Visible = False
        clsFun.CloseConnection()
    End Sub

    Private Sub StockSaleLedger()
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim Remark2 As String = String.Empty
        Dim RemarkHindi As String = String.Empty
        Dim MandiTax As Decimal = 0.0
        Dim CommAmt As Decimal = 0.0
        Dim RDFAmt As Decimal = 0.0
        Dim TareAmt As Decimal = 0.0
        Dim Labouramt As Decimal = 0.0
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                'Remark2 = .Cells(1).Value & " Nug : " & .Cells(5).Value & " Weight : " & .Cells(6).Value & " On : " & .Cells(8).Value & " /- " & .Cells(10).Value & " ,Amount : " & Val(.Cells(11).Value) & ",Crate : " & Val(.Cells(22).Value) & " (Charges : " & Val(.Cells(13).Value) - Val(.Cells(22).Value) & " ) "
                'RemarkHindi = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & (.Cells(0).Value) & "") & ", नग : " & .Cells(5).Value & " वजन : " & .Cells(6).Value & " भाव : " & .Cells(8).Value & " /- " & .Cells(10).Value & ",रकम : " & Val(.Cells(11).Value) & ", क्रेट रकम : " & Val(.Cells(22).Value) & " (ख़र्चे : " & Val(.Cells(13).Value) - Val(.Cells(22).Value) & " ) "
                Remark2 = .Cells(1).Value & " Nug : " & .Cells(5).Value & " Weight : " & .Cells(6).Value & " On : " & .Cells(8).Value & " /- " & .Cells(10).Value & " ,(Charges: " & Val(.Cells(13).Value) & ") "
                RemarkHindi = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & (.Cells(0).Value) & "") & ", नग : " & .Cells(5).Value & " वजन : " & .Cells(6).Value & " भाव : " & .Cells(8).Value & " /- " & .Cells(10).Value & ",(ख़र्चे: " & Val(.Cells(13).Value) & ") "
                Dim CrateRemark As String = .Cells(26).Value & ", Qty : " & .Cells(27).Value & ", Rate /- " & .Cells(21).Value & " Amount : " & .Cells(22).Value
                If .Cells("Customer").Value <> "" Then
                    If Val(.Cells(12).Value) > 0 Then
                        If Val(.Cells(2).Value) = Val(.Cells(33).Value) AndAlso Val(.Cells(33).Value) <> 0 Then
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & .Cells(12).Value & "', 'D','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                        ElseIf Val(.Cells(2).Value) <> Val(.Cells(33).Value) AndAlso Val(.Cells(33).Value) <> 0 Then
                            If clsFun.ExecScalarStr("Select TareSameAc from Controls") = "Yes" Then
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & .Cells(12).Value & "', 'D','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                            Else
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & Val(.Cells(12).Value) - Val(.Cells(22).Value) & "', 'D','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                                If Val(.Cells(20).Value) > 0 Then
                                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(33).Value) & ", '" & .Cells(34).Value & "','" & Val(.Cells(20).Value) & "', 'D','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                                ElseIf Val(.Cells(22).Value) > 0 Then
                                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(33).Value) & ", '" & .Cells(34).Value & "','" & Val(.Cells(22).Value) & "', 'D','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                                End If         
                            End If
                        Else
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & .Cells(12).Value & "', 'D','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                        End If
                    End If
                    CommAmt = Format(Val(CommAmt) + Val(.Cells(16).Value), "0.00")
                    MandiTax = Format(Val(MandiTax) + Val(.Cells(18).Value), "0.00")
                    RDFAmt = Format(Val(RDFAmt) + Val(.Cells(20).Value), "0.00")
                    TareAmt = Format(Val(TareAmt) + Val(.Cells(22).Value), "0.00")
                    Labouramt = Format(Val(Labouramt) + Val(.Cells(24).Value), "0.00")
                End If
            End With
        Next
        ''Maal Khata Account
        Remark2 = "Voucher No.:" & txtVoucherNo.Text & ",Nugs :" & txtTotNug.Text & ",Weight :" & txtTotweight.Text & ",Net :" & txtTotBasic.Text & ",Charges :" & txtTotCharge.Text & ",Total :" & txtTotalAmt.Text
        RemarkHindi = "Voucher No.:" & txtVoucherNo.Text & ",नग :" & txtTotNug.Text & ",वजन :" & txtTotweight.Text & ",नेट :" & txtTotBasic.Text & ",खर्चे :" & txtTotCharge.Text & ",कुल रकम :" & txtTotalAmt.Text
        '    clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 29, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29"), Val(txtTotBasic.Text), "C", Remark2, "", RemarkHindi)
        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 29 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "','" & Val(txtTotBasic.Text) & "', 'C','" & Remark2 & "', '','" & RemarkHindi & "'"
        If Val(CommAmt) > 0 Then ''Tax Tax Account
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 10, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10"), Val(CommAmt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 10 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "','" & Val(CommAmt) & "', 'C','" & Remark2 & "', '','" & RemarkHindi & "'"

        End If
        If Val(MandiTax) > 0 Then ''Mandi Tax Account
            '  clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 30, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30"), Val(MandiTax), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 30 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "','" & Val(MandiTax) & "', 'C','" & Remark2 & "', '','" & RemarkHindi & "'"
        End If
        If Val(RDFAmt) > 0 Then ''RDF Account
            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 39, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39"), Val(RDFAmt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 39 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "','" & Val(RDFAmt) & "', 'C','" & Remark2 & "', '','" & RemarkHindi & "'"
        End If
        If Val(TareAmt) > 0 Then ''Tare Account
            '     clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 4, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4"), Val(TareAmt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 4 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "','" & Val(TareAmt) & "', 'C','" & Remark2 & "', '','" & RemarkHindi & "'"

        End If
        If Val(Labouramt) > 0 Then ''Labour Account
            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 23, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23"), Val(Labouramt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 23 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "','" & Val(Labouramt) & "', 'C','" & Remark2 & "', '','" & RemarkHindi & "'"

        End If
        If Val(txttotRoff.Text) <> 0 Then ''Account 
            If Val(txttotRoff.Text) > 0 Then
                '   clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txttotRoff.Text)), "C", Remark2, "", RemarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txttotRoff.Text)) & "', 'C','" & Remark2 & "', '','" & RemarkHindi & "'"

            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txttotRoff.Text)) & "', 'D','" & Remark2 & "', '','" & RemarkHindi & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub

    Private Sub ServerStockSaleLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim Remark2 As String = String.Empty
        Dim RemarkHindi As String = String.Empty
        Dim MandiTax As Decimal = 0.0
        Dim CommAmt As Decimal = 0.0
        Dim RDFAmt As Decimal = 0.0
        Dim TareAmt As Decimal = 0.0
        Dim Labouramt As Decimal = 0.0
        For Each row As DataGridViewRow In dg1.Rows
            With row
                Remark2 = .Cells(1).Value & " Nug : " & .Cells(5).Value & " Weight : " & .Cells(6).Value & " On : " & .Cells(8).Value & " /- " & .Cells(10).Value & " ,(Charges : " & Val(.Cells(13).Value) & ") "
                RemarkHindi = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & (.Cells(0).Value) & "") & ", नग : " & .Cells(5).Value & " वजन : " & .Cells(6).Value & " भाव : " & .Cells(8).Value & " /- " & .Cells(10).Value & ",(ख़र्चे : " & Val(.Cells(13).Value) & ") "
                Dim CrateRemark As String = .Cells(26).Value & ", Qty : " & .Cells(27).Value & ", Rate /- " & .Cells(21).Value & " Amount : " & .Cells(22).Value
                If .Cells("Customer").Value <> "" Then
                    If Val(.Cells(12).Value) > 0 Then
                        If Val(.Cells(2).Value) = Val(.Cells(33).Value) AndAlso Val(.Cells(33).Value) <> 0 Then
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & .Cells(12).Value & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                        ElseIf Val(.Cells(2).Value) <> Val(.Cells(33).Value) AndAlso Val(.Cells(33).Value) <> 0 Then
                            If clsFun.ExecScalarStr("Select TareSameAc from Controls") = "Yes" Then
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & .Cells(12).Value & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                            Else
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & Val(.Cells(12).Value) - Val(.Cells(22).Value) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(33).Value) & ", '" & .Cells(34).Value & "','" & Val(.Cells(22).Value) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                            End If
                        Else
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(2).Value) & ", '" & .Cells(3).Value & "','" & .Cells(12).Value & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '" & .Cells(3).Value & "','" & RemarkHindi & "'"
                        End If
                    End If
                    CommAmt = Format(Val(CommAmt) + Val(.Cells(16).Value), "0.00")
                    MandiTax = Format(Val(MandiTax) + Val(.Cells(18).Value), "0.00")
                    RDFAmt = Format(Val(RDFAmt) + Val(.Cells(20).Value), "0.00")
                    TareAmt = Format(Val(TareAmt) + Val(.Cells(22).Value), "0.00")
                    Labouramt = Format(Val(Labouramt) + Val(.Cells(24).Value), "0.00")
                End If
            End With
        Next
        ''Maal Khata Account
        Remark2 = "Voucher No.:" & txtVoucherNo.Text & ",Nugs :" & txtTotNug.Text & ",Weight :" & txtTotweight.Text & ",Net :" & txtTotBasic.Text & ",Charges :" & txtTotCharge.Text & ",Total :" & txtTotalAmt.Text
        RemarkHindi = "Voucher No.:" & txtVoucherNo.Text & ",नग :" & txtTotNug.Text & ",वजन :" & txtTotweight.Text & ",नेट :" & txtTotBasic.Text & ",खर्चे :" & txtTotCharge.Text & ",कुल रकम :" & txtTotalAmt.Text
        'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 29, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29"), Val(txtTotBasic.Text), "C", Remark2, "", RemarkHindi)
        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 29 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "','" & Val(txtTotBasic.Text) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"
        If Val(CommAmt) > 0 Then ''Tax Tax Account
            'clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 10, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10"), Val(CommAmt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 10 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "','" & Val(CommAmt) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"

        End If
        If Val(MandiTax) > 0 Then ''Mandi Tax Account
            '  clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 30, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30"), Val(MandiTax), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 30 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "','" & Val(MandiTax) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"
        End If
        If Val(RDFAmt) > 0 Then ''RDF Account
            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 39, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39"), Val(RDFAmt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 39 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "','" & Val(RDFAmt) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"
        End If
        If Val(TareAmt) > 0 Then ''Tare Account
            '     clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 4, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4"), Val(TareAmt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 4 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "','" & Val(TareAmt) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"

        End If
        If Val(Labouramt) > 0 Then ''Labour Account
            ' clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 23, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23"), Val(Labouramt), "C", Remark2, "", RemarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 23 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "','" & Val(Labouramt) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"

        End If
        If Val(txttotRoff.Text) <> 0 Then ''Account 
            If Val(txttotRoff.Text) > 0 Then
                '   clsFun.Ledger(0, Val(txtid.Text), SqliteEntryDate, Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txttotRoff.Text)), "C", Remark2, "", RemarkHindi)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txttotRoff.Text)) & "', 'C','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"

            Else
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txttotRoff.Text)) & "', 'D','" & Val(ServerTag) & "','" & Val(OrgID) & "','" & Remark2 & "', '','" & RemarkHindi & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub
    Public Sub MultiUpdateStockSale()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        '  Dim cmd As SQLite.SQLiteCommand
        Dim ModifyByID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                    & " SallerID='" & Val(txtPurchaseTypeID.Text) & "', SallerName='" & txtPurchaseType.Text & "', " _
                                    & " Nug='" & Val(txtTotNug.Text) & "', kg='" & Val(txtTotweight.Text) & "',BasicAmount='" & Val(txtTotBasic.Text) & "', " _
                                    & " TotalAmount='" & Val(TxtGrandTotal.Text) & "',TotalCharges='" & Val(txtTotCharge.Text) & "'," _
                                    & " StorageID='" & Val(txtStorageID.Text) & "',Storagename='" & txtStorage.Text & "',InvoiceID='" & Val(txtInvoiceID.Text) & "', " _
                                    & " ModifiedByID='" & Val(ModifyByID) & "',ModifiedTime='" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where ID =" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                el.WriteToErrorLog(sql, Constants.compname, "Stock Sale Updated")
                clsFun.CloseConnection()
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                UpdateCrate()
                sql = "DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                  "DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";" &
                                  "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ";"
                If clsFun.ExecNonQuery(sql) > 0 Then
                    el.WriteToErrorLog(sql, Constants.compname, "Stock Sale Record Deleted")
                End If
                VchId = Val(txtid.Text)
                ServerTag = 1 : Dg1Record() : StockSaleLedger() : CrateLedger() : ServerStockSaleLedger() : ServerCrateLedger()
                ClearAll()

            End If
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
    Private Sub UpdateRecord()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim ModifyByID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "', Entrydate='" & SqliteEntryDate & "', " _
                                    & " SallerID='" & Val(txtPurchaseTypeID.Text) & "', SallerName='" & txtPurchaseType.Text & "', " _
                                    & " Nug='" & Val(txtTotNug.Text) & "', kg='" & Val(txtTotweight.Text) & "',BasicAmount='" & Val(txtTotBasic.Text) & "', " _
                                    & " TotalAmount='" & Val(TxtGrandTotal.Text) & "',TotalCharges='" & Val(txtTotCharge.Text) & "'," _
                                    & " StorageID='" & Val(txtStorageID.Text) & "',Storagename='" & txtStorage.Text & "',InvoiceID='" & Val(txtInvoiceID.Text) & "', " _
                                    & " ModifiedByID='" & Val(ModifyByID) & "',ModifiedTime='" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "' where ID =" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                el.WriteToErrorLog(sql, Constants.compname, "Stock Sale Updated") : clsFun.CloseConnection()
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                UpdateCrate()
                clsFun.ExecNonQuery(" DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                    " DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";" &
                                    " DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ";")
                ServerTag = 1 : Dg1Record() : StockSaleLedger() : CrateLedger() : ServerStockSaleLedger() : ServerCrateLedger()
                MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                ClearAll()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Private Sub ClearAll()
        txtclear() : VNumber()
        txtPurchaseType.Enabled = True
        dg1.Rows.Clear() : txttotRoff.Clear()
        cbAccountName.Text = "" : txtid.Clear()
        cbAccountName.SelectedValue = 0
        BtnSave.Text = "&Save" : BtnDelete.Enabled = False
        txtTotNug.Text = "" : txtTotBasic.Text = ""
        txtTotweight.Text = "" : TxtGrandTotal.Text = ""
        txtTotCharge.Text = "" : BtnSave.BackColor = Color.DarkTurquoise
        BtnSave.Image = My.Resources.Save : VNumber() : MainScreenPicture.retrive()
        txtPurchaseType.Enabled = True
        txtStorage.Enabled = True
        clsFun.ExecScalarStr("Delete From Stock Where Transtype ='" & Me.Text & "'")
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
        If dg1.RowCount = 0 Then MsgBox("There is No Record to Save / Update...", vbOKOnly, "Empty") : Exit Sub
        If BtnSave.Text = "&Save" Then
            Dim addSale As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
            If addSale <> "Y" Then MsgBox("You Don't Have Rights to Add Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : ClearAll() : Exit Sub
            ButtonControl() : Save() : ButtonControl()
        Else
            Dim ModifySale As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
            If ModifySale <> "Y" Then MsgBox("You Don't Have Rights to Modify Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : ClearAll() : Exit Sub
            ButtonControl() : UpdateRecord() : ButtonControl()
        End If
        mskEntryDate.Focus()
        If clsFun.ExecScalarStr("Select AutoSwitch From Controls") = "No" Then Exit Sub
        If MessageBox.Show(" Do you Want to Switch SellOut Now???", "SellOut Now", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Sellout_Auto.MdiParent = MainScreenForm
            Sellout_Auto.Show()
            Sellout_Auto.BringToFront()
        End If
        If Application.OpenForms().OfType(Of Stock_Sale_Register).Any = True Then Stock_Sale_Register.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
    End Sub

    Private Sub VNumber()
        Dim vno As Integer = 0
        If vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
            vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        Else
            vno = clsFun.ExecScalarInt("Select Max(InvoiceID)  AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        End If
        'Dim vno As Integer = 0
        'If Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
        '    vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        '    txtVoucherNo.Text = vno + 1
        '    txtInvoiceID.Text = vno + 1
        'Else
        '    vno = clsFun.ExecScalarInt("SELECT InvoiceID AS NumberOfProducts FROM Vouchers WHERE id=(SELECT max(id) FROM Vouchers Where TransType='" & Me.Text & "')")
        '    '   vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        '    txtVoucherNo.Text = vno + 1
        '    txtInvoiceID.Text = vno + 1
        'End If
    End Sub
    Private Sub dg1_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles dg1.RowStateChanged
        If BtnSave.Text = "&Save" Then
            If dg1.RowCount = 0 Then
                txtPurchaseType.Enabled = True
                txtStorage.Enabled = True
            Else
                txtPurchaseType.Enabled = False
                txtStorage.Enabled = False
            End If
        End If
    End Sub

    Private Sub cbCrateMarka_Leave(sender As Object, e As EventArgs) Handles cbCrateMarka.Leave
        If clsFun.ExecScalarInt("Select count(*)from CrateMarka") = 0 Then
            Exit Sub
        End If
        If clsFun.ExecScalarInt("Select count(*)from CrateMarka where MarkaName='" & cbCrateMarka.Text & "'") = 0 Then
            MsgBox("Item Not Found in Database...", vbOKOnly, "Access Denied")
            txtItem.Focus()
            Exit Sub
        End If
        If lblCrate.Text = "Y" Then
            Dim crateRate As String = clsFun.ExecScalarStr("Select CrateBardana From Controls")
            If crateRate = "Yes" Then txtTare.Text = clsFun.ExecScalarStr("Select Rate From CrateMarka Where ID='" & Val(cbCrateMarka.SelectedValue) & "'")

        End If
    End Sub
    Private Sub PurchaseTypeColumns()
        dgPurchaseType.ColumnCount = 3
        dgPurchaseType.Columns(0).Name = "ID" : dgPurchaseType.Columns(0).Visible = False
        dgPurchaseType.Columns(1).Name = "Account Name" : dgPurchaseType.Columns(1).Width = 300
        dgPurchaseType.Columns(2).Name = "City" : dgPurchaseType.Columns(2).Width = 74
        dgPurchaseType.Visible = True : dgPurchaseType.BringToFront() ': retrivePurchaseType()
    End Sub
    Private Sub dgPurchaseType_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgPurchaseType.CellClick
        If dgPurchaseType.Rows.Count = 0 Then Exit Sub
        txtPurchaseType.Clear()
        txtPurchaseTypeID.Clear()
        txtPurchaseTypeID.Text = dgPurchaseType.SelectedRows(0).Cells(0).Value
        txtPurchaseType.Text = dgPurchaseType.SelectedRows(0).Cells(1).Value
        txtStorage.Focus()
    End Sub
    Private Sub dgPurchaseType_KeyDown(sender As Object, e As KeyEventArgs) Handles dgPurchaseType.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dgPurchaseType.Rows.Count = 0 Then Exit Sub
            txtPurchaseType.Clear()
            txtPurchaseTypeID.Clear()
            txtPurchaseTypeID.Text = dgPurchaseType.SelectedRows(0).Cells(0).Value
            txtPurchaseType.Text = dgPurchaseType.SelectedRows(0).Cells(1).Value
            StoreColums()
            'txtStorage.Text = clsFun.ExecScalarStr("Select  StorageName From Purchase where StockHolderID=" & txtAccountID.Text & " group by  StorageName")
            'txtStorageID.Text = clsFun.ExecScalarStr("Select  StorageID From Purchase where StockHolderID=" & txtAccountID.Text & " group by  StorageID")
            ' dgPurchaseType.Visible = False
            txtStorage.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtPurchaseType.Focus()
    End Sub
    Private Sub retrivePurchaseType(Optional ByVal condtion As String = "")

        dgPurchaseType.Rows.Clear()
        Dim sql As String = String.Empty
        If BtnSave.Text = "&Save" Then
            sql = "SELECT StockHolderID, " & _
                  "(SELECT AccountName FROM Accounts WHERE ID = Purchase.StockHolderID) AS StockHolderName, " & _
                  "PurchaseTypeName, " & _
                  "(IFNULL(SUM(Nug), 0) - " & _
                  "(SELECT IFNULL(SUM(Nug), 0) FROM Transaction2 " & _
                  "WHERE TransType IN ('Stock Sale', 'Standard Sale', 'On Sale', 'Store In') AND SallerID = Purchase.StockHolderID)) AS RestNug " &
                  "FROM Purchase " & _
                  condtion & _
                  " GROUP BY StockHolderID " & _
                  "HAVING RestNug > 0 " & _
                  "ORDER BY StockHolderName;"
            '        "HAVING RestNug > 0 OR RestWeight > 0 " & _
        Else
            sql = "SELECT StockHolderID, " & _
                  "(SELECT AccountName FROM Accounts WHERE ID = Purchase.StockHolderID) AS StockHolderName, " & _
                  "PurchaseTypeName, " & _
                  "(IFNULL(SUM(Nug), 0) - " & _
                  "(SELECT IFNULL(SUM(Nug), 0) FROM Transaction2 " & _
                  "WHERE TransType IN ('Stock Sale', 'Standard Sale', 'On Sale', 'Store In') AND SallerID = Purchase.StockHolderID)) + " & _
                  "(SELECT IFNULL(SUM(Nug), 0) FROM Transaction2 " & _
                  "WHERE TransType IN ('Stock Sale', 'Standard Sale', 'On Sale', 'Store In') AND SallerID = Purchase.StockHolderID AND VoucherID = " & Val(txtid.Text) & ") AS RestNug " & _
                  "FROM Purchase " & _
                  condtion & _
                  " GROUP BY StockHolderID " & _
                  "HAVING RestNug >  " & _
                  "ORDER BY StockHolderName;"
        End If

        ' sql = "Select StockHolderID,StockholderName,PurchaseTypeName From Purchase Group By StockHolderID;"
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dgPurchaseType.Rows.Add()
                    With dgPurchaseType.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("StockHolderID").ToString()
                        .Cells(1).Value = dt.Rows(i)("StockHolderName").ToString()
                        .Cells(2).Value = dt.Rows(i)("PurchaseTypeName").ToString()
                    End With
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub txtPurchaseType_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPurchaseType.KeyPress
        dgPurchaseType.Visible = True
    End Sub

    Private Sub txtPurchaseType_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPurchaseType.KeyUp
        '  If dgPurchaseType.RowCount = 0 Then Exit Sub
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

  
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub

    Private Sub mskEntryDate_Leave(sender As Object, e As EventArgs) Handles mskEntryDate.Leave
        mskEntryDate.SelectionLength = 0
    End Sub

    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub StoreColums()
        dgStore.ColumnCount = 2
        dgStore.Columns(0).Name = "ID" : dgStore.Columns(0).Visible = False
        dgStore.Columns(1).Name = "Mode Name" : dgStore.Columns(1).Width = 250
        dgStore.Visible = True : dgStore.BringToFront() : RetriveMode()
    End Sub
    Private Sub RetriveMode(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dgStore.Rows.Clear()
        Dim sql As String = String.Empty
        'If BtnSave.Text = "&Save" Then
        '    sql = "Select  StorageID,StorageName,(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  " &
        '    " and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=Purchase.StorageID  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as RestNug " &
        '    "From Purchase where StockHolderID=" & Val(txtPurchaseTypeID.Text) & "  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & "  group by  StorageID,StorageName having RestNug<>0 order by StorageName "
        'Else
        '    sql = "Select  StorageID,StorageName,(ifnull(sum(Nug),0) -(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  " &
        '    " and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=Purchase.StorageID  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
        '    "+(Select ifnull(sum(Nug),0) From Transaction2 Where SallerID=Purchase.StockHolderID  " &
        '    " and TransType in ('Stock Sale','On Sale','Standard Sale','Store Out') and StorageID=Purchase.StorageID  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') as RestNug " &
        '    "From Purchase where StockHolderID=" & Val(txtPurchaseTypeID.Text) & "  and EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'  " & condtion & "  group by  StorageID,StorageName having RestNug<>0 order by StorageName"

        'End If

        If BtnSave.Text = "&Save" Then
            sql = "SELECT StorageID, StorageName, " & _
                  "(IFNULL(SUM(Nug), 0) - " & _
                  "(SELECT IFNULL(SUM(Nug), 0) FROM Transaction2 " & _
                  "WHERE SallerID = Purchase.StockHolderID AND TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
                  "AND StorageID = Purchase.StorageID AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) AS RestNug, " & _
                  "(IFNULL(SUM(Weight), 0) - " & _
                  "(SELECT IFNULL(SUM(Weight), 0) FROM Transaction2 " & _
                  "WHERE SallerID = Purchase.StockHolderID AND TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
                  "AND StorageID = Purchase.StorageID AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) AS RestWeight " & _
                  "FROM Purchase " & _
                  "WHERE StockHolderID = " & Val(txtPurchaseTypeID.Text) & " AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & _
                  condtion & " GROUP BY StorageID, StorageName " & _
                  "HAVING RestNug <> 0 OR RestWeight <> 0 " & _
                  "ORDER BY StorageName;"
        Else
            sql = "SELECT StorageID, StorageName, " & _
                  "(IFNULL(SUM(Nug), 0) - " & _
                  "(SELECT IFNULL(SUM(Nug), 0) FROM Transaction2 " & _
                  "WHERE SallerID = Purchase.StockHolderID AND TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
                  "AND StorageID = Purchase.StorageID AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) + " & _
                  "(SELECT IFNULL(SUM(Nug), 0) FROM Transaction2 " & _
                  "WHERE SallerID = Purchase.StockHolderID AND TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
                  "AND StorageID = Purchase.StorageID AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') AS RestNug, " & _
                  "(IFNULL(SUM(Weight), 0) - " & _
                  "(SELECT IFNULL(SUM(Weight), 0) FROM Transaction2 " & _
                  "WHERE SallerID = Purchase.StockHolderID AND TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
                  "AND StorageID = Purchase.StorageID AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) + " & _
                  "(SELECT IFNULL(SUM(Weight), 0) FROM Transaction2 " & _
                  "WHERE SallerID = Purchase.StockHolderID AND TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
                  "AND StorageID = Purchase.StorageID AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') AS RestWeight " & _
                  "FROM Purchase " & _
                  "WHERE StockHolderID = " & Val(txtPurchaseTypeID.Text) & " AND EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & _
                  condtion & " GROUP BY StorageID, StorageName " & _
                  "HAVING RestNug <> 0 OR RestWeight <> 0 " & _
                  "ORDER BY StorageName;"
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
    Private Sub txtStorage_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtStorage.KeyPress
        dgStore.Visible = True
    End Sub
    Private Sub txtStorage_KeyUp(sender As Object, e As KeyEventArgs) Handles txtStorage.KeyUp
        'StoreColums()

        If txtStorage.Text.Trim() <> "" Then
            '   dgStore.Visible = True
            RetriveMode(" and upper(StorageName) Like upper('" & txtStorage.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        If e.KeyCode = Keys.Escape Then
            If dgStore.Visible = False Then dgStore.Visible = False
        End If
    End Sub

    Private Sub txtStorage_GotFocus(sender As Object, e As EventArgs) Handles txtStorage.GotFocus
        If txtPurchaseType.Enabled <> False Then
            If dgPurchaseType.SelectedRows.Count = 0 Then txtPurchaseType.Focus() : Exit Sub
            If dgPurchaseType.ColumnCount = 0 Then PurchaseTypeColumns()
            If dgPurchaseType.Rows.Count = 0 Then retrivePurchaseType() : Exit Sub
            txtPurchaseTypeID.Text = dgPurchaseType.SelectedRows(0).Cells(0).Value
            txtPurchaseType.Text = dgPurchaseType.SelectedRows(0).Cells(1).Value
        End If
        StoreColums() : AcBal()
        dgPurchaseType.Visible = False
    End Sub
    Private Sub dgMode_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgStore.CellClick
        txtStorage.Clear()
        txtStorageID.Clear()
        txtStorageID.Text = dgStore.SelectedRows(0).Cells(0).Value
        txtStorage.Text = dgStore.SelectedRows(0).Cells(1).Value
        dgStore.Visible = False
        txtItem.Focus()
    End Sub

    Private Sub dgMode_KeyDown(sender As Object, e As KeyEventArgs) Handles dgStore.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtStorage.Clear()
            txtStorageID.Clear()
            txtStorageID.Text = dgStore.SelectedRows(0).Cells(0).Value
            txtStorage.Text = dgStore.SelectedRows(0).Cells(1).Value
            dgStore.Visible = False
            txtItem.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtStorage.Focus()
    End Sub

    Private Sub StockInItemRowColums()
        dgItemSearch.ColumnCount = 4
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 170
        dgItemSearch.Columns(2).Name = "Nug" : dgItemSearch.Columns(2).Width = 100
        dgItemSearch.Columns(3).Name = "Weight" : dgItemSearch.Columns(3).Visible = False
        StockINretriveItems()
    End Sub

    Private Sub StockINretriveItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable : dgItemSearch.Rows.Clear()
        Dim sql As String = String.Empty
        Dim Calculate As String = IIf(trackStock = "Nug", "RestNug", "RestWeight")
        'sql = "SELECT P.ItemID, P.ItemName, " & _
        '      "COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID AND T.LOT = P.LOTNO " & _
        '      "AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
        '      " + COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug, " & _
        '      "COALESCE(SUM(P.Weight), 0) - (COALESCE((SELECT SUM(T.Weight) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
        '      "AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
        '      " + COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestWeight " & _
        '      "FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & _
        '      "AND P.StorageID = " & Val(txtStorageID.Text) & " " & condtion & " GROUP BY P.ItemID, P.ItemName  HAVING RestNug >= 0 OR RestWeight >= 0 ORDER BY P.EntryDate;"
        sql = "SELECT P.ItemID,P.ItemName,(IFNULL(SUM(P.Nug), 0)" & _
            " -(SELECT IFNULL(SUM(T.Nug), 0) FROM Transaction2 T WHERE T.SallerID = P.StockHolderID  AND T.ItemID = P.ItemID AND T.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out')" & _
            " AND T.StorageID = " & Val(txtStorageID.Text) & " AND T.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.SellerID = P.StockHolderID  AND S.ITEMID = P.ITEMID))  " & _
            " - COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.SellerID = P.StockHolderID  AND S.ItemID = P.ItemID AND S.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out')), 0)) AS RestNug " & _
            " FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StorageID = " & Val(txtStorageID.Text) & " AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & condtion & " GROUP BY P.ItemID, P.ItemName HAVING RestNug >= 0 ORDER BY P.ItemName;"

        'sql = "SELECT P.ItemID, P.ItemName, " & _
        '      " (COALESCE(SUM(P.Nug), 0) " & _
        '      " - (SELECT COALESCE(SUM(T.Nug), 0) FROM Transaction2 T " & _
        '      "     WHERE T.SellerID = P.StockHolderID " & _
        '      "       AND T.ItemID = P.ItemID " & _
        '      "       AND T.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
        '      "       AND T.StorageID = " & Val(txtStorageID.Text) & " " & _
        '      "       AND T.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & _
        '      "       AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S " & _
        '      "                               WHERE S.SellerID = P.StockHolderID AND S.ItemID = P.ItemID)) " & _
        '      " - COALESCE((SELECT SUM(S.Nug) FROM STOCK S " & _
        '      "            WHERE S.SellerID = P.StockHolderID AND S.ItemID = P.ItemID " & _
        '      "              AND S.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out')), 0)) AS RestNug, " & _
        '      " (COALESCE(SUM(P.Weight), 0) " & _
        '      " - (SELECT COALESCE(SUM(T.Weight), 0) FROM Transaction2 T " & _
        '      "     WHERE T.SellerID = P.StockHolderID " & _
        '      "       AND T.ItemID = P.ItemID " & _
        '      "       AND T.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') " & _
        '      "       AND T.StorageID = " & Val(txtStorageID.Text) & " " & _
        '      "       AND T.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & _
        '      "       AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S " & _
        '      "                               WHERE S.SellerID = P.StockHolderID AND S.ItemID = P.ItemID)) " & _
        '      " - COALESCE((SELECT SUM(S.Weight) FROM STOCK S " & _
        '      "            WHERE S.SellerID = P.StockHolderID AND S.ItemID = P.ItemID " & _
        '      "              AND S.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out')), 0)) AS RestWeight " & _
        '      " FROM Purchase P " & _
        '      " WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' " & _
        '      "   AND P.StorageID = " & Val(txtStorageID.Text) & _
        '      "   AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & condtion & _
        '      " GROUP BY P.ItemID, P.ItemName " & _
        '      " HAVING RestNug >= 0 " & _
        '      " ORDER BY P.ItemName;"
        dt = clsFun.ExecDataTable(sql)

        Try
            Dim lastval As Integer = 0
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    ItemBalCheck = Val(dt.Rows(i)("RestNug").ToString())
                    SelectedItemBal = 0
                    SelectedWeightBal = 0
                    ' Check if selected row in dg1 matches
                    Dim showRecord As Boolean = ItemBalCheck > 0
                    If dg1.SelectedRows.Count <> 0 Then
                        If Val(dg1.SelectedRows(0).Cells(0).Value) = Val(dt.Rows(i)("ItemID")) Then
                            SelectedItemBal = Val(dg1.SelectedRows(0).Cells(5).Value)
                            SelectedWeightBal = Val(dg1.SelectedRows(0).Cells(6).Value)
                            showRecord = True ' Ensure record is shown if selected
                        End If
                    End If
                    ItemBalCheck += SelectedItemBal
                    ' Dim RestWeight = Val(dt.Rows(i)("RestWeight").ToString()) + SelectedWeightBal
                    If showRecord Then
                        dgItemSearch.Rows.Add()
                        With dgItemSearch.Rows(lastval)
                            .Cells(0).Value = dt.Rows(i)("ItemID").ToString()
                            .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                            .Cells(2).Value = Val(ItemBalCheck)
                            .Cells(3).Value = Val(RestWeight)
                        End With
                        lastval += 1
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub


    'Private Sub StockINretriveItems(Optional ByVal condtion As String = "")
    '      Dim dt As New DataTable : dgItemSearch.Rows.Clear()
    '      Dim sql As String = String.Empty
    '      Dim Calculate As String = IIf(trackStock = "Nug", "RestNug", "RestWeight")
    '      sql = "SELECT P.ItemID, P.ItemName, " & _
    '          "COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID AND T.LOT = P.LOTNO " & _
    '          "AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
    '          " + COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug, " & _
    '          "COALESCE(SUM(P.Weight), 0) - (COALESCE((SELECT SUM(T.Weight) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
    '          "AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
    '          " + COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestWeight " & _
    '          "FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & _
    '          "AND P.StorageID = " & Val(txtStorageID.Text) & " " & condtion & " GROUP BY P.ItemID, P.ItemName  HAVING RestNug > 0 OR RestWeight > 0 ORDER BY P.EntryDate;"
    '      dt = clsFun.ExecDataTable(sql)
    '      Try
    '          Dim lastval As Integer = 0
    '          If dt.Rows.Count > 0 Then
    '              For i = 0 To dt.Rows.Count - 1
    '                  ItemBalCheck = 0 : SelectedItemBal = 0
    '                  If dg1.SelectedRows.Count <> 0 Then
    '                      If Val(dg1.SelectedRows(0).Cells(0).Value) = (Val(txtItemID.Text) Or dt.Rows(i)("ItemID").ToString()) Then
    '                          SelectedItemBal = Val(SelectedItemBal) + Val(dg1.SelectedRows(0).Cells(5).Value)
    '                          SelectedWeightBal = Val(SelectedWeightBal) + Val(dg1.SelectedRows(0).Cells(6).Value)
    '                      End If
    '                  End If
    '                  ItemBalCheck = Val(dt.Rows(i)("RestNug").ToString()) + Val(SelectedItemBal)
    '                  Dim RestWeight = Val(dt.Rows(i)("RestWeight").ToString()) + Val(SelectedWeightBal)
    '                  dgItemSearch.Rows.Add()
    '                  With dgItemSearch.Rows(lastval)
    '                      .Cells(0).Value = dt.Rows(i)("ItemID").ToString()
    '                      .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
    '                      .Cells(2).Value = Val(ItemBalCheck)
    '                      .Cells(3).Value = Val(RestWeight) ' Weight calculation added to column 3
    '                      lastval += 1
    '                  End With
    '              Next
    '          End If
    '      Catch ex As Exception
    '          MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
    '      End Try
    '  End Sub
    Private Sub txtItemStockIn_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        ' StockInItemRowColums()
        If txtItem.Text.Trim() <> "" Then
            StockINretriveItems(" And upper(p.ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
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
        ' txtItem.TextAlign = HorizontalAlignment.Left
        ItemFill()
        ' clsFun.FillDropDownList(cbLot, "Select  LotNo From Purchase where ItemID=" & txtItemID.Text & " and StockHolderId=" & txtPurchaseTypeID.Text & " group by  LotNo", "LotNo", "", "")
        dgItemSearch.Visible = False
        txtAccount.Focus()
    End Sub
    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown

        If e.KeyCode = Keys.Enter Then
            If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
            txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            ' txtItem.TextAlign = HorizontalAlignment.Left
            ItemFill()
            '  clsFun.FillDropDownList(cbLot, "Select  LotNo From Purchase where ItemID=" & txtItemID.Text & " and StockHolderId=" & txtPurchaseTypeID.Text & " group by  LotNo", "LotNo", "", "")

            txtAccount.Focus()
            dgItemSearch.Visible = False
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then
            txtItem.Focus()

        End If
    End Sub
    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
        If dgItemSearch.ColumnCount = 0 Then StockInItemRowColums()
        If dgItemSearch.RowCount = 0 Then StockINretriveItems()
        If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus() : Exit Sub
        If txtItem.Text.Trim() <> "" Then
            StockINretriveItems(" And upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            StockINretriveItems()
        End If
        If dgItemSearch.SelectedRows.Count = 0 Then txtItem.Focus() : Exit Sub
        txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        dgStore.Visible = False ': AcBal()

        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        ItemFill() : ItemBalance() ': AccountComm()
        dgItemSearch.Visible = False ' : txtAccount.Focus()
        If Val(bal) = 0 Then
            txtItem.Focus()
        End If



    End Sub
    Private Sub ItemBalance()
        Dim sql As String = String.Empty
        If dg1.SelectedRows.Count > 0 AndAlso dg1.Rows.Count > 0 AndAlso dg1.SelectedRows(0).Cells.Count > 36 Then
            excludeCondition = "AND STOCK.ID <> " & Val(dg1.SelectedRows(0).Cells(36).Value)
        End If
        If Val(txtid.Text) <> 0 Then excludeID = "and VoucherID <>" & Val(txtid.Text)
        ' If trackStock = "Nug" Then
        'sql = "SELECT  " & _
        '    " COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID AND T.LOT = P.LOTNO " & _
        '    " AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
        '    " + COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug " & _
        '    " FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & _
        '    " AND P.StorageID = " & Val(txtStorageID.Text) & " AND p.ITEMid=" & Val(txtItemID.Text) & " " & condtion & " GROUP BY P.ItemID, P.ItemName  HAVING RestNug > 0 ORDER BY P.EntryDate;"
        sql = "SELECT (IFNULL(SUM(P.Nug), 0)" & _
" -(SELECT IFNULL(SUM(T.Nug), 0) FROM Transaction2 T WHERE T.SallerID = P.StockHolderID  AND T.ItemID = P.ItemID AND T.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out')" & _
" AND T.StorageID = " & Val(txtStorageID.Text) & " AND T.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.SellerID = P.StockHolderID  AND S.ITEMID = P.ITEMID))  " & _
" - COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.SellerID = P.StockHolderID  AND S.ItemID = P.ItemID AND S.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out')), 0)) AS RestNug " & _
" FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StorageID = " & Val(txtStorageID.Text) & " AND p.ITEMid=" & Val(txtItemID.Text) & " AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & condtion & " GROUP BY P.ItemID, P.ItemName HAVING RestNug > 0 ORDER BY P.ItemName;"
        bal = Format(Val(clsFun.ExecScalarStr(sql)), "0.00")
        If dg1.SelectedRows.Count <> 0 Then
            SelectedItemBal = 0
            If Val(dg1.SelectedRows(0).Cells(0).Value) = Val(txtItemID.Text) Then
                SelectedItemBal = Val(SelectedItemBal) + Val(dg1.SelectedRows(0).Cells(5).Value)
            End If
        End If
        bal = Val(bal) + Val(SelectedItemBal)
        lblItemBalance.Text = "Item Bal.(Nug): " & Val(bal)
        lblItemBalance.Visible = True
        'Else
        'sql = "SELECT (IFNULL(SUM(P.Weight), 0)" & _
        '        " -(SELECT IFNULL(SUM(T.Weight), 0) FROM Transaction2 T WHERE T.SallerID = P.StockHolderID  AND T.ItemID = P.ItemID AND T.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out')" & _
        '        " AND T.StorageID = " & Val(txtStorageID.Text) & " AND T.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.SellerID = P.StockHolderID  AND S.ITEMID = P.ITEMID))  " & _
        '        " - COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.SellerID = P.StockHolderID  AND S.ItemID = P.ItemID AND S.TransType IN ('Stock Sale', 'On Sale', 'Standard Sale', 'Store Out') ), 0)) AS RestWeight " & _
        '        " FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StorageID = " & Val(txtStorageID.Text) & " AND p.ITEMid=" & Val(txtItemID.Text) & " AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & condtion & " GROUP BY P.ItemID, P.ItemName HAVING RestWeight > 0 ORDER BY P.ItemName;"
        'bal = Format(Val(clsFun.ExecScalarStr(sql)), "0.00")
        'If dg1.SelectedRows.Count <> 0 Then
        '    SelectedWeightBal = 0
        '    If Val(dg1.SelectedRows(0).Cells(0).Value) = Val(txtItemID.Text) Then
        '        SelectedWeightBal = Val(SelectedWeightBal) + Val(dg1.SelectedRows(0).Cells(6).Value)
        '    End If
        'End If
        'bal = Val(bal) + Val(SelectedWeightBal)
        'lblItemBalance.Text = "Item Bal.(Weight): " & Val(bal)
        'lblItemBalance.Visible = True
        'End If
    End Sub
    'Private Sub ItemBalance()
    '    lblItemBalance.Visible = True
    '    Dim PurchaseBal As String = "" : Dim StockBal As String = ""
    '    Dim RestBal As String = "" : Dim tmpbal As String = ""
    '    Dim dt As New DataTable
    '    PurchaseBal = clsFun.ExecScalarStr("Select sum(Nug) From Purchase Where StockHolderID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " and ItemID = " & Val(txtItemID.Text) & " and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    StockBal = clsFun.ExecScalarStr("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & " and StorageID=" & Val(txtStorageID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and TransType  in ('Stock Sale','On Sale','Standard Sale','Store Out') and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    RestBal = Val(PurchaseBal) - Val(StockBal)
    '    If BtnSave.Text = "&Save" Then
    '        If dg1.SelectedRows.Count = 0 Then
    '            If Val(StockBal) = 0 Then ' if no record inserted
    '                If dg1.RowCount = 0 Then ' if no rows addred
    '                    bal = (RestBal)
    '                Else 'if rows count
    '                    For i As Integer = 0 To dg1.RowCount - 1
    '                        If Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) Then
    '                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(5).Value)
    '                        End If
    '                    Next i
    '                    tmpbal = (tmpbal)
    '                End If
    '                bal = Val(PurchaseBal) - Val(tmpbal)
    '            Else
    '                If dg1.RowCount = 0 Then ' if any Record Inserted in Database but Row not Added
    '                    bal = (RestBal)
    '                Else
    '                    For i As Integer = 0 To dg1.RowCount - 1 'if any Record Inserted in Database and Row Added
    '                        If Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) Then
    '                            tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(5).Value)
    '                        End If
    '                    Next i
    '                    bal = Val(RestBal) - Val(tmpbal)
    '                End If
    '            End If
    '        Else
    '            If Val(StockBal) = 0 Then
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) Then
    '                        tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(5).Value)
    '                    End If
    '                Next i
    '                tmpbal = Val(PurchaseBal) - Val(tmpbal)
    '                tmpbal = Val(tmpbal) + Val(dg1.SelectedRows(0).Cells(5).Value)
    '                bal = Val(tmpbal)
    '            Else
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) Then
    '                        tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(5).Value)
    '                    End If
    '                Next i
    '                tmpbal = Val(RestBal) - Val(tmpbal)
    '                tmpbal = (tmpbal) + Val(dg1.SelectedRows(0).Cells(5).Value)
    '                bal = Val(tmpbal)
    '            End If
    '        End If
    '    Else '''''''''''''''''''''''''''''for Update Stock--------------------------------------
    '        If dg1.RowCount = 0 Then ' if no rows addred
    '            bal = (RestBal)
    '        Else 'if rows count
    '            UpdateTmp = clsFun.ExecScalarInt("Select sum(Nug) From Transaction2 Where SallerID=" & Val(txtPurchaseTypeID.Text) & "  and ItemID = " & Val(txtItemID.Text) & " and StorageID=" & Val(txtStorageID.Text) & " AND VoucherID not in ('" & Val(txtid.Text) & "') and TransType  in ('Stock Sale','On Sale','Standard Sale','Store Out') and  EntryDate<='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '            If dg1.SelectedRows.Count = 0 Then
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) Then
    '                        tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(5).Value)
    '                    End If
    '                Next i
    '                tmpbal = Val(UpdateTmp) + Val(tmpbal)
    '                bal = Val(PurchaseBal) - Val(tmpbal)
    '            Else
    '                For i As Integer = 0 To dg1.RowCount - 1
    '                    If Val(dg1.Rows(i).Cells(0).Value) = Val(txtItemID.Text) Then
    '                        tmpbal = Val(tmpbal) + Val(dg1.Rows(i).Cells(5).Value)
    '                    End If
    '                Next i
    '                tmpbal = Val(UpdateTmp) + Val(tmpbal)
    '                If Val(dg1.SelectedRows(0).Cells(0).Value) = Val(txtItemID.Text) Then
    '                    '- Val(dg1.SelectedRows(0).Cells(5).Value)
    '                    tmpbal = Val(tmpbal) - Val(dg1.SelectedRows(0).Cells(5).Value)
    '                End If
    '                ' If (StockBal) = 0 Then

    '                bal = Val(PurchaseBal) - Val(tmpbal)
    '            End If
    '        End If
    '    End If
    '    If dg1.SelectedRows.Count = 0 Then
    '        lblItemBalance.Text = "Item Balance : " & Val(bal)
    '    Else
    '        lblItemBalance.Text = "Item Balance : " & Val(bal) & " (Selected Nugs Not Counting)"
    '    End If
    'End Sub
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
        Dim sql As String = String.Empty
        If dg1.SelectedRows.Count > 0 AndAlso dg1.Rows.Count > 0 AndAlso dg1.SelectedRows(0).Cells.Count > 36 Then
            excludeCondition = "AND STOCK.ID <> " & Val(dg1.SelectedRows(0).Cells(36).Value)
        End If
        If Val(txtid.Text) <> 0 Then excludeID = "and Transaction2.VoucherID <>" & Val(txtid.Text)
        'If trackStock = "Nug" Then
        sql = "SELECT  " & _
" COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID AND T.LOT = P.LOTNO " & _
" AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
" + COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug " & _
" FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " and P.VoucherID= " & Val(dgLot.SelectedRows(0).Cells(0).Value.trim) & " " & _
" AND P.StorageID = " & Val(txtStorageID.Text) & " and P.itemID = " & Val(txtItemID.Text) & "  AND p.LotNo='" & txtLot.Text & "' and P.VoucherID " & condtion & " GROUP BY P.VoucherID, P.EntryDate, P.ItemID, P.ItemName, P.AccountName, P.LotNo, P.VehicleNo " & _
" HAVING RestNug > 0 ORDER BY P.EntryDate;"
        LotBal = Format(Val(clsFun.ExecScalarStr(sql)), "0.00")
        If dg1.SelectedRows.Count <> 0 Then
            SelectedItemBal = 0
            If Val(dg1.SelectedRows(0).Cells(0).Value) = Val(txtItemID.Text) AndAlso Val(dg1.SelectedRows(0).Cells(4).Value.trim) = Val(txtLot.Text.Trim) Then
                SelectedItemBal = Val(SelectedItemBal) + Val(dg1.SelectedRows(0).Cells(5).Value)
            End If
        End If
        LotBal = Val(LotBal) + Val(SelectedItemBal)
        lblLot.Text = "Lot Bal. (Nug): " & Val(LotBal)
        lblLot.Visible = True
        '        Else
        '        sql = "SELECT " & _
        '   "COALESCE(SUM(P.Weight), 0) - (COALESCE((SELECT SUM(T.Weight) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
        '   "AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
        '   " + COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestWeight " & _
        '   "FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " and P.VoucherID= " & Val(dgLot.SelectedRows(0).Cells(0).Value.trim) & " " & _
        '   "AND P.StorageID = " & Val(txtStorageID.Text) & " and P.itemID = " & Val(txtItemID.Text) & " AND p.LotNo='" & txtLot.Text & "'  " & condtion & " GROUP BY P.VoucherID, P.EntryDate, P.ItemID, P.ItemName, P.AccountName, P.LotNo, P.VehicleNo " & _
        '" HAVING  RestWeight > 0 ORDER BY P.EntryDate;"
        '        LotBal = Format(Val(clsFun.ExecScalarStr(sql)), "0.00")
        '        If dg1.SelectedRows.Count <> 0 Then
        '            SelectedWeightBal = 0
        '            If Val(dg1.SelectedRows(0).Cells(0).Value) = Val(txtItemID.Text) AndAlso Val(dg1.SelectedRows(0).Cells(4).Value.trim) = Val(txtLot.Text.Trim) Then
        '                SelectedWeightBal = Val(SelectedWeightBal) + Val(dg1.SelectedRows(0).Cells(6).Value)
        '            End If
        '        End If
        '        LotBal = Val(LotBal) + Val(SelectedWeightBal)
        '        lblLot.Text = "Lot Bal. (Weight): " & Val(LotBal)
        '        lblLot.Visible = True
        '        End If
    End Sub

    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear() : txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        ' CustomerFill()
        DgAccountSearch.Visible = False
        txtLot.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        ' LotCoulmns()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            Dim AccountID As String = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(AccountID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False : txtLot.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    
    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress, txtItem.KeyPress
        If txtAccount.Focused = True Then DgAccountSearch.BringToFront() : DgAccountSearch.Visible = True
        If txtItem.Focused = True Then dgItemSearch.Visible = True
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
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
        DgAccountSearch.Visible = True : retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        ' dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))" & condtion & " order by AccountName")
        If ckShowSupplier.Checked Then
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16,17)  or UnderGroupID in (11,16,17))" & condtion & " order by AccountName")
        Else
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))" & condtion & " order by AccountName")
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

    Private Sub LotCoulmns()
        dgLot.ColumnCount = 7
        dgLot.Columns(0).Name = "LotID" : dgLot.Columns(0).Visible = False
        dgLot.Columns(1).Name = "Lot" : dgLot.Columns(1).Width = 100
        dgLot.Columns(2).Name = "Vehicle No." : dgLot.Columns(2).Width = 100
        dgLot.Columns(3).Name = "Date" : dgLot.Columns(3).Width = 100
        dgLot.Columns(4).Name = "Account Name" : dgLot.Columns(4).Width = 200
        dgLot.Columns(5).Name = "Nug" : dgLot.Columns(5).Width = 150
        dgLot.Columns(6).Name = "Weight" : dgLot.Columns(6).Visible = False
        dgLot.Visible = True
    End Sub

  


    Private Sub RetriveLot(Optional ByVal condtion As String = "")
        dgLot.Rows.Clear()
        Dim LotBalCheckNug As String = 0
        Dim LotBalCheckWeight As String = 0
        Dim SelectedLotBalNug As String = 0
        Dim SelectedLotBalWeight As String = 0
        Dim sql As String = String.Empty
        Dim excludeCondition As String = ""

        ' Check if a row is selected and the DataGridView is not empty
        If dg1.SelectedRows.Count > 0 AndAlso dg1.Rows.Count > 0 AndAlso dg1.SelectedRows(0).Cells.Count > 36 Then
            excludeCondition = "AND S.ID <> " & Val(dg1.SelectedRows(0).Cells(36).Value)
        End If
        If Val(txtid.Text) <> 0 Then excludeID = "and VoucherID <>" & Val(txtid.Text)
        Dim calculate As String = If(trackStock = "Nug", "RestNug", "RestWeight")
        sql = "SELECT P.VoucherID, P.EntryDate, P.ItemID, P.ItemName, P.AccountName, P.LotNo, P.VehicleNo," & _
    " COALESCE(SUM(P.Nug), 0) - (COALESCE((SELECT SUM(T.Nug) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID AND T.LOT = P.LOTNO " & _
    " AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
    " + COALESCE((SELECT SUM(S.Nug) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestNug, " & _
    " COALESCE(SUM(P.Weight), 0) - (COALESCE((SELECT SUM(T.Weight) FROM Transaction2 T WHERE T.PURCHASEID = P.VOUCHERID AND T.ItemID = P.ITEMID " & _
    " AND T.LOT = P.LOTNO AND T.VoucherID NOT IN (SELECT S.TransID FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO)), 0) " & _
    " + COALESCE((SELECT SUM(S.Weight) FROM STOCK S WHERE S.PURCHASEID = P.VOUCHERID AND S.ITEMID = P.ITEMID AND S.LOT = P.LOTNO), 0)) AS RestWeight " & _
    " FROM Purchase P WHERE P.EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' AND P.StockHolderID = " & Val(txtPurchaseTypeID.Text) & " " & _
    " AND P.StorageID = " & Val(txtStorageID.Text) & " and  P.itemID = " & Val(txtItemID.Text) & " " & condtion & " GROUP BY P.VoucherID, P.EntryDate, P.ItemID, P.ItemName, P.AccountName, P.LotNo, P.VehicleNo " & _
    " HAVING RestNug >= 0  ORDER BY P.EntryDate;"

        dt = clsFun.ExecDataTable(sql)

        Try
            Dim lastval As Integer = 0
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    LotBalCheckNug = 0 : LotBalCheckWeight = 0
                    SelectedLotBalNug = 0 : SelectedLotBalWeight = 0
                    If dg1.SelectedRows.Count <> 0 Then
                        If dg1.SelectedRows(0).Cells(4).Value = dt.Rows(i)("LotNo").ToString() AndAlso Val(dg1.SelectedRows(0).Cells(0).Value) = Val(txtItemID.Text) AndAlso Val(dg1.SelectedRows(0).Cells(30).Value) = Val(dt.Rows(i)("VoucherID").ToString()) Then
                            SelectedLotBalNug += Val(dg1.SelectedRows(0).Cells(5).Value)
                            SelectedLotBalWeight += Val(dg1.SelectedRows(0).Cells(6).Value)
                        End If
                    End If

                    ' Calculate remaining balance for Nuggets and Weight
                    Dim RestNug = Val(dt.Rows(i)("RestNug").ToString()) + SelectedLotBalNug
                    Dim RestWeight = Val(dt.Rows(i)("RestWeight").ToString()) + SelectedLotBalWeight
                    ' If balance is greater than 0, add it to the grid
                    If Val(RestNug) > 0 Then
                        dgLot.Rows.Add()
                        With dgLot.Rows(lastval)
                            .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                            .Cells(1).Value = dt.Rows(i)("LotNo").ToString()
                            .Cells(2).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(3).Value = CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy")
                            .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                            .Cells(5).Value = Val(RestNug)
                            .Cells(6).Value = Val(RestWeight) ' Rest Weight
                            lastval += 1
                        End With
                    End If
                Next
            End If
        Catch ex As Exception
            ' Log or display the exception if necessary
        End Try
    End Sub

 
    Private Sub txtLot_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLot.KeyPress
        dgLot.BringToFront() : dgLot.Visible = True
    End Sub

    Private Sub dgLot_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgLot.CellClick
        txtLot.Clear()
        txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
        txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
        dgLot.Visible = False
        txtNug.Focus()
    End Sub

    Private Sub dgLot_KeyDown(sender As Object, e As KeyEventArgs) Handles dgLot.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtLot.Clear()
            txtLot.Text = dgLot.SelectedRows(0).Cells(1).Value
            txtPurchaseID.Text = Val(dgLot.SelectedRows(0).Cells(0).Value)
            dgLot.Visible = False
            txtNug.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtLot.Focus()
    End Sub

    Private Sub dgLot_KeyUp(sender As Object, e As KeyEventArgs) Handles dgLot.KeyUp

    End Sub

    Private Sub txtLot_KeyUp(sender As Object, e As KeyEventArgs) Handles txtLot.KeyUp
        If txtLot.Text.Trim() <> "" Then
            RetriveLot(" And upper(TRIM(LotNo)) like upper('" & txtLot.Text.Trim() & "%')")
        Else
            RetriveLot()
        End If
    End Sub
    Private Sub txtPurchaseType_Leave(sender As Object, e As EventArgs) Handles txtPurchaseType.Leave
        ' txtPurchaseType.SelectionLength = 0
    End Sub

    'Private Sub txtLot_TextChanged(sender As Object, e As EventArgs) Handles txtLot.TextChanged
    '    If dgLot.RowCount = 0 Then Exit Sub
    '    If txtLot.Text.Trim() <> "" Then
    '        RetriveLot(" And upper(LotNo) Like upper('%" & txtLot.Text.Trim() & "%')")
    '    End If
    'End Sub

    Private Sub txtLot_Leave(sender As Object, e As EventArgs) Handles txtLot.Leave
        txtLot.SelectionLength = 0
    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub dgPurchaseType_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgPurchaseType.CellContentClick

    End Sub

    Private Sub DgAccountSearch_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellContentClick

    End Sub

    Private Sub dgLot_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgLot.CellContentClick

    End Sub

    Private Sub dgItemSearch_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellContentClick

    End Sub

    Private Sub txtVoucherNo_Leave(sender As Object, e As EventArgs) Handles txtVoucherNo.Leave
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarStr("Select count(*)from Vouchers where BillNo='" & txtVoucherNo.Text & "' and TransType='" & Me.Text & "'") > 1 Then
                MsgBox("Voucher Number Already Exists...", vbOKOnly, "Access Denied")
                txtVoucherNo.Focus()
            End If
        Else
            Exit Sub
        End If
    End Sub

    Private Sub txtVoucherNo_TextChanged(sender As Object, e As EventArgs) Handles txtVoucherNo.TextChanged

    End Sub

    Private Sub txtPurchaseType_TextChanged(sender As Object, e As EventArgs) Handles txtPurchaseType.TextChanged

    End Sub

    Private Sub Cbper_LostFocus(sender As Object, e As EventArgs) Handles Cbper.Leave
        If Val(txtNug.Text) <= 0 And Cbper.SelectedIndex = 0 Then
            txtNug.Focus()
            MsgBox("You Are Selected Value For Nug but There is No Nug ...", MsgBoxStyle.Critical, "Stop")
            Exit Sub
        ElseIf Val(txtKg.Text) <= 0 And Cbper.SelectedIndex = 1 Then
            txtKg.Focus()
            MsgBox("You Are Selected Value For Weight but There is No Weight", MsgBoxStyle.Critical, "Stop")
            Exit Sub
        ElseIf Val(txtKg.Text) <= 0 And Cbper.SelectedIndex = 2 Then
            txtKg.Focus()
            MsgBox("You Are Selected Value For Weight but There is No Weight", MsgBoxStyle.Critical, "Stop")
            Exit Sub
        End If
    End Sub

    Private Sub txtItem_TextChanged(sender As Object, e As EventArgs) Handles txtItem.TextChanged

    End Sub

    Private Sub txtAccount_Leave(sender As Object, e As EventArgs) Handles txtAccount.Leave
        'txtAccount.SelectionStart = 0
    End Sub

    Private Sub txtItem_Leave(sender As Object, e As EventArgs) Handles txtItem.Leave
        txtItem.SelectionStart = 0
    End Sub

    Private Sub txtCut_Leave(sender As Object, e As EventArgs) Handles txtCut.Leave
        If txtCut.Text = "" Then txtCut.Text = "0"
    End Sub

    Private Sub txtKg_Leave(sender As Object, e As EventArgs) Handles txtKg.Leave
        If txtKg.Text = "" Then txtKg.Text = "0"
    End Sub



    Private Sub txtInvoiceID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceID.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlInvoiceID.Visible = False
            txtVoucherNo.Focus()
        End If
    End Sub

    Private Sub ckTaxPaid_LostFocus(sender As Object, e As EventArgs) Handles ckTaxPaid.LostFocus
        ckTaxPaid.ForeColor = Color.Black
        ckTaxPaid.BackColor = Color.FromArgb(247, 220, 111)
    End Sub

    Private Sub txtNetRate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNetRate.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlNetRate.Visible = False
            txtSallerRate.Focus()
            e.SuppressKeyPress = True
            txtSallerRate.Text = txtCustomerRate.Text
        End If

    End Sub

    Private Sub txtLot_TextChanged(sender As Object, e As EventArgs) Handles txtLot.TextChanged

    End Sub

    Private Sub ckTaxPaid_Leave(sender As Object, e As EventArgs) Handles ckTaxPaid.Leave

    End Sub


    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        FillWithNevigation()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If Offset = 0 Then
            Offset = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Stock Sale'  Order By ID ")
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
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Stock Sale'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        Offset = (TotalPages - 1) * RowCount
        FillWithNevigation()
    End Sub

    Private Sub btnPnlHide_Click(sender As Object, e As EventArgs) Handles btnPnlHide.Click
        PnlCustomerBill.Visible = False : txtItem.Focus()
    End Sub

    Private Sub mskEntryDate_LostFocus1(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick

    End Sub

    Private Sub txtbasicAmt_KeyUp(sender As Object, e As KeyEventArgs) Handles txtbasicAmt.KeyUp
        If Cbper.SelectedIndex = 0 Then
            txtCustomerRate.Text = Val(txtbasicAmt.Text) / Format(Val(txtNug.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 1 Then
            txtCustomerRate.Text = Val(txtbasicAmt.Text) / Format(Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 2 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 5 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 3 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 10 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 4 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 20 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 5 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 40 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 6 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 41 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 7 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 50 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 8 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 51 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 9 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 51.7 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 10 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 52.3 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 11 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 53 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 12 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 80 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 13 Then
            txtCustomerRate.Text = Format(Val(txtbasicAmt.Text) * 100 / Val(txtKg.Text), "0.00")
        ElseIf Cbper.SelectedIndex = 14 Then
            txtCustomerRate.Text = Format(Val(txtNug.Text) * Val(txtbasicAmt.Text), "0.00")
        End If
        txtSallerAmout.Text = txtbasicAmt.Text
        txtCustomerRate.Text = Format(Math.Round(Val(txtCustomerRate.Text), 2), "0.00")
        txtSallerRate.Text = txtCustomerRate.Text
        If txtCustomerRate.Text = "NAN" Then txtCustomerRate.Text = "" : Exit Sub
        txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
        txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
        If Val(txtRdfPer.Text) > 0 Then
            txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtbasicAmt.Text) / 100, "0.00")
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
        lbltotCharges.Text = Format(Val(Val(txtComAmt.Text)) + Val(Val(txtMAmt.Text)) + Val(Val(txtRdfAmt.Text)) + Val(Val(txtTareAmt.Text)) + Val(Val(txtLaboutAmt.Text)), "0.00")
        txtTotalAmt.Text = Format(Val(Val(lbltotCharges.Text)) + Val(Val(txtbasicAmt.Text)), 0)
    End Sub

    Private Sub txtTotalAmt_TextChanged(sender As Object, e As EventArgs) Handles txtTotalAmt.TextChanged

    End Sub
End Class