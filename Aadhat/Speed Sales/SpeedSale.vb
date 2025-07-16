Public Class SpeedSale
    Dim VNo As Integer : Dim VchId As Integer
    Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 17 : Dim Offset As Integer = 0
    Dim ServerTag As Integer : Dim el As New Aadhat.ErrorLogger
    Dim Octroi As String : Dim ApplyCommWeight As String
    Dim crateRate As String : Dim RoundOff As String
    Dim CalculationMethod As String : Dim CUT As Decimal = 0.0
    Dim ItemPer As String
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.BackColor = Color.LightGray
        mskEntryDate.SelectAll()
    End Sub
    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text) : mskEntryDate.TabStop = False : mskEntryDate.Enabled = False
        Dim BackDateEntry As String = clsFun.ExecScalarStr("SELECT DontAllowBack FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Other'")
        If BackDateEntry <> "N" Then
            If mskEntryDate.Text < Date.Today.ToString("dd-MM-yyyy") Then MsgBox("Can't Create Bill Back Date...", MsgBoxStyle.Critical, "Denied Back Date Entries...") : mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy") : Exit Sub
        End If
        'If keyData = Keys.Escape Then Exit Sub
        If dg1.RowCount = 0 Then retrive()
    End Sub

    Private Sub SpeedSale_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If btnClose.Enabled = False Then Exit Sub
            If DgAccountSearch.Visible = True Then
                DgAccountSearch.Visible = False
                Exit Sub
            ElseIf dgItemSearch.Visible = True Then
                dgItemSearch.Visible = False
                Exit Sub
            ElseIf pnlMarka.Visible = True Then
                pnlMarka.Visible = False
                Exit Sub
            ElseIf MessageBox.Show("Are you Sure Want to Exit Speed Sale ???", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                Me.Close() : MainScreenPicture.retrive()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub

    Private Sub dtp1_Leave(sender As Object, e As EventArgs) Handles dtp1.Leave
        mskEntryDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub


    Private Sub SpeedSale_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0 'System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka Order by Upper(MarkaName)", "MarkaName", "Id", "")
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33,11) order by AccountName ", "AccountName", "ID", "--N./A.--")
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy") : mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
        CbPer.SelectedIndex = 0 : pnlMarka.Visible = False : pnlGrossWeight.Visible = False
        rowColums() : FillSpeedSale() : VNumber()
        lblCommAmt.Text = ChrW(&H20B9) : lblMandiAmt.Text = ChrW(&H20B9)
        lblRdfAmt.Text = ChrW(&H20B9) : lblTareAmt.Text = ChrW(&H20B9)
        Label10.Text = ChrW(&H20B9)
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
            lblAcBal.Text = "Bal : " & Format(Math.Abs(Val(Bal)), "0.00") & " Dr"
        Else
            lblAcBal.Visible = True
            lblAcBal.Text = "Bal : " & Format(Math.Abs(Val(Bal)), "0.00") & " Cr"
        End If
        AccountComm()
    End Sub


    'Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
    '    If keyData = Keys.Escape Then
    '        Return False 'Add a return statement for this code path
    '    End If
    '    Dim notlastColumn As Boolean = True
    '    If dg1.Focused = True Then
    '        If dg1.ReadOnly = False Then
    '            Dim icolumn As Integer = Val(dg1.CurrentCell.ColumnIndex)
    '            Dim irow As Integer = Val(dg1.CurrentCell.RowIndex)
    '            Dim i As Integer = irow
    '            If keyData = Keys.Enter Then
    '                If icolumn = dg1.Columns.Count - 3 Then
    '                    If notlastColumn = True Then
    '                        dg1.CurrentCell = dg1.Rows(i).Cells(1)
    '                    End If
    '                    If dg1.CurrentRow.Index = Val(dg1.Rows.Count - 1) Then
    '                        txtNug.Focus()
    '                        Return True 'Add a return statement for this code path
    '                    End If
    '                    dg1.CurrentCell = dg1(0, irow + 1)

    '                    'calcMulti()
    '                Else
    '                    dg1.CurrentCell = dg1(icolumn + 1, irow)
    '                    'calcMulti()
    '                End If
    '                Return True
    '            Else
    '                Return ProcessCmdKey(msg, keyData)
    '            End Ifk
    '        End If
    '    End If
    '    Return False 'Add a return statement for all remaining code paths
    'End Function


    Protected Overrides Sub OnFormClosed(ByVal e As FormClosedEventArgs)
        anInstance = Nothing
        MyBase.OnFormClosed(e)
    End Sub
    Private Sub txtAccount_Click(sender As Object, e As EventArgs) Handles txtAccount.Click
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
        txtAccount.SelectAll()
    End Sub



    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear() : txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        txtNug.Focus()
    End Sub

    Private Sub DgAccountSearch_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DgAccountSearch.EditingControlShowing

    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.PageUp Then
            If DgAccountSearch.CurrentRow.Index = 0 Then txtAccount.Focus()
        End If
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
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            '    CustomerFill()
            '   DgAccountSearch.Visible = False
            txtNug.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
        If e.KeyCode = Keys.Up Then
            If DgAccountSearch.CurrentRow.Index = 0 Then
                txtAccount.Focus()
            End If
        End If
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 27
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 95 : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Item Name " : dg1.Columns(2).Width = 159 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "Customer" : dg1.Columns(3).Width = 241 : dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).Name = "Nug" : dg1.Columns(4).Width = 99 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(5).Name = "Kg" : dg1.Columns(5).Width = 99 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(6).Name = "Rate" : dg1.Columns(6).Width = 99 : dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(7).Name = "Per" : dg1.Columns(7).Width = 77 : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(8).Name = "Basic" : dg1.Columns(8).Width = 99 : dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(9).Name = "Charges" : dg1.Columns(9).Width = 100 : dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(10).Name = "Total" : dg1.Columns(10).Width = 92 : dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(11).Name = "Comm Per" : dg1.Columns(11).Visible = False : dg1.Columns(12).Name = "comm Amt" : dg1.Columns(12).Visible = False
        dg1.Columns(13).Name = "User Charges" : dg1.Columns(13).Visible = False : dg1.Columns(14).Name = "UC Amt" : dg1.Columns(14).Visible = False
        dg1.Columns(15).Name = "Rdf" : dg1.Columns(15).Visible = False : dg1.Columns(16).Name = "Rdf Amt" : dg1.Columns(16).Visible = False
        dg1.Columns(17).Name = "Tare" : dg1.Columns(17).Visible = False : dg1.Columns(18).Name = "T Amt" : dg1.Columns(18).Visible = False
        dg1.Columns(19).Name = "Labour" : dg1.Columns(19).Visible = False : dg1.Columns(20).Name = "L Amt" : dg1.Columns(20).Visible = False
        dg1.Columns(21).Name = "C Name" : dg1.Columns(21).Visible = False : dg1.Columns(22).Name = "C Qty" : dg1.Columns(22).Visible = False
        dg1.Columns(23).Name = "Crate Y/N" : dg1.Columns(23).Visible = False : dg1.Columns(24).Name = "Roff" : dg1.Columns(24).Visible = False
        dg1.Columns(25).Name = "Roff" : dg1.Columns(25).Visible = False : dg1.Columns(26).Name = "GrossWeight" : dg1.Columns(26).Visible = False
    End Sub
    Private Sub ItemRowColumns()
        dgItemSearch.ColumnCount = 3
        dgItemSearch.Columns(0).Name = "ID" : dgItemSearch.Columns(0).Visible = False
        dgItemSearch.Columns(1).Name = "Item Name" : dgItemSearch.Columns(1).Width = 100
        dgItemSearch.Columns(2).Name = "OtherName" : dgItemSearch.Columns(2).Width = 100
        retriveItems()
    End Sub
    Private Sub retriveItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select ID,ItemName,OtherName from Items " & condtion & " order by ItemName  Limit 11")
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

    'Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus

    '    txtItem.SelectAll()
    'End Sub
    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        If txtItem.Text.Trim() <> "" Then
            retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            retriveItems()
        End If
        If e.KeyCode = Keys.Escape Then dgItemSearch.Visible = False
    End Sub

    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress, txtAccount.KeyPress
        If txtItem.Focused = True Then dgItemSearch.Visible = True
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub
    Private Sub dgItemSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemSearch.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
        txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
        itemfill()
        dgItemSearch.Visible = False
        txtAccount.Focus()
    End Sub

    Private Sub dgItemSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemSearch.KeyDown
        If e.KeyCode = Keys.PageUp Then
            If dgItemSearch.CurrentRow.Index = 0 Then txtItem.Focus()
        End If
        If e.KeyCode = Keys.F3 Then
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            Dim ItemID As String = dgItemSearch.SelectedRows(0).Cells(0).Value
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            Item_form.FillContros(ItemID)
            Item_form.BringToFront()
        End If
        If e.KeyCode = Keys.Enter Then
            txtItem.Clear()
            txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            itemfill() : e.SuppressKeyPress = True
            txtAccount.Focus()
        End If
        If e.KeyCode = Keys.Back Then
            txtItem.Focus()
        End If
        If e.KeyCode = Keys.Up Then
            If dgItemSearch.CurrentRow.Index = 0 Then
                txtItem.Focus()
            End If
        End If
    End Sub
    Public Sub FillContros(ByVal ID As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        btnSave.BackColor = Color.Coral
        BtnDelete.Visible = True
        btnSave.Image = My.Resources.Edit
        btnSave.Text = "&Update"
        '  Panel1.BackColor = Color.PaleVioletRed
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Transaction2 where VoucherID=" & ID
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            'Application.DoEvents()
            txtid.Text = ds.Tables("a").Rows(0)("VoucherID").ToString()
            Dim mskDate As String = clsFun.ExecScalarStr("Select EntryDate From Vouchers Where ID='" & Val(txtid.Text) & "'")
            mskEntryDate.Text = CDate(mskDate).ToString("dd-MM-yyyy")
            txtItemID.Text = Val(ds.Tables("a").Rows(0)("ItemID").ToString())
            txtItem.Text = ds.Tables("a").Rows(0)("ItemName").ToString()
            txtAccountID.Text = Val(ds.Tables("a").Rows(0)("AccountID").ToString())
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txtKg.Text = Format(Val(ds.Tables("a").Rows(0)("Weight").ToString()), "0.00")
            txtrate.Text = Format(Val(ds.Tables("a").Rows(0)("Rate").ToString()), "0.00")
            CbPer.Text = ds.Tables("a").Rows(0)("Per").ToString()
            txtComPer.Text = Format(Val(ds.Tables("a").Rows(0)("CommPer").ToString()), "0.00")
            txtMPer.Text = Format(Val(ds.Tables("a").Rows(0)("MPer").ToString()), "0.00")
            txtRdfPer.Text = Format(Val(ds.Tables("a").Rows(0)("rdfPer").ToString()), "0.00")
            txtTare.Text = Format(Val(ds.Tables("a").Rows(0)("Tare").ToString()), "0.00")
            txtLabour.Text = Format(Val(ds.Tables("a").Rows(0)("labour").ToString()), "0.00")
            txtComAmt.Text = Format(Val(ds.Tables("a").Rows(0)("CommAmt").ToString()), "0.00")
            txtMAmt.Text = Format(Val(ds.Tables("a").Rows(0)("MAmt").ToString()), "0.00")
            txtRdfAmt.Text = Format(Val(ds.Tables("a").Rows(0)("RdfAmt").ToString()), "0.00")
            txtTareAmt.Text = Format(Val(ds.Tables("a").Rows(0)("TareAmt").ToString()), "0.00")
            txtLaboutAmt.Text = Format(Val(ds.Tables("a").Rows(0)("LabourAmt").ToString()), "0.00")
            lblCrate.Text = ds.Tables("a").Rows(0)("MaintainCrate").ToString()
            cbCrateMarka.Text = ds.Tables("a").Rows(0)("CrateMarka").ToString()
            txtCrateQty.Text = Val(ds.Tables("a").Rows(0)("crateQty").ToString())
            txtSlipNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtAddWeight.Text = ds.Tables("a").Rows(0)("OnWeight").ToString()
            txtInvoiceID.Text = clsFun.ExecScalarStr("Select InvoiceID From Vouchers Where ID='" & Val(txtid.Text) & "'")
            cbAccountName.SelectedValue = Val(ds.Tables("a").Rows(0)("CrateAccountID").ToString())
            cbAccountName.Text = ds.Tables("a").Rows(0)("CrateAccountName").ToString()
            lblRoundOff.Text = Format(Val(ds.Tables("a").Rows(0)("RoundOff").ToString()), "0.00")
            txtGrossWt.Text = Format(Val(ds.Tables("a").Rows(0)("GrossWeight").ToString()), "0.00")
        End If
        SpeedCalculation()
    End Sub

    Private Sub Delete()
        'Dim marka As String = "Speed Sale"
        Dim RemoveSale As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
        If RemoveSale <> "Y" Then MsgBox("You Don't Have Rights to Delete Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : txtclear() : Exit Sub
        Try
            If Val(txtid.Text) = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure  want to delete Item??", "Delete Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dim sql As String = "DELETE from Ledger WHERE vourchersID=" & Val(txtid.Text) & ";DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "; " &
                    "DELETE from Transaction2 WHERE VoucherID=" & Val(txtid.Text) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & ""
                If clsFun.ExecNonQuery(sql) > 0 Then
                    ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                              "Delete From  CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                    ServerTag = 0 : ServerLedger() : ServerCrate()
                    el.WriteToErrorLog(sql, Constants.compname, "Speed Sale Deleted")
                    txtclear() : Me.Alert("Delete Successful...", msgAlert.enmType.Delete)
                    mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
                    mskEntryDate.Focus()
                    If Application.OpenForms().OfType(Of Speed_Sale_Register).Any = True Then If Speed_Sale_Register.ckAll.Checked = False Then Speed_Sale_Register.retriveAll() Else Speed_Sale_Register.retrive()
                    If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            FillContros(dg1.SelectedRows(0).Cells(0).Value)
            mskEntryDate.TabStop = True : mskEntryDate.Enabled = True
            mskEntryDate.Focus() : e.SuppressKeyPress = True
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
            'If Val(txtid.Text) = 0 Then Exit Sub
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            txtid.Text = Val(dg1.SelectedRows(0).Cells(0).Value)
            dg1.CurrentCell = dg1.SelectedRows(0).Cells(1)
            Delete() : e.Handled = True
            Exit Sub
        End If
    End Sub

    Private Sub txtclear()
        ' Application.DoEvents()
        BtnDelete.Visible = False : retrive()
        txtNet.Text = "" : txtTotal.Text = ""
        txtComPer.Text = "0" : txtMPer.Text = "0"
        txtRdfPer.Text = "0" : txtLabour.Text = "0"
        txtTare.Text = "0" : lblTotCharges.Text = "0.00"
        txtComAmt.Text = "0.00" : txtMAmt.Text = "0.00"
        txtRdfAmt.Text = "0.00" : txtTareAmt.Text = "0.00"
        txtLaboutAmt.Text = "0.00" : txtNet.Text = "0.00"
        cbAccountName.SelectedValue = 0 ': txtNug.Text = ""
        cbAccountName.Text = "" : txtCrateQty.Text = ""
        VNumber() : txtNetRate.Text = "0.00"
        txtKg.Clear() : txtGrossWt.Text = 0
        txtTotal.Text = "0.00" : dg1.ClearSelection()
        txtid.Text = "" : dg1.ClearSelection()
        btnSave.Text = "&Save" : btnSave.BackColor = Color.DarkTurquoise
        btnSave.Image = My.Resources.Save : pnlMarka.Visible = False
        lblAddWeight.Text = "" : txtAddWeight.Clear()
        txtItem.Focus()
    End Sub


    Private Sub retrive()
        ' Application.DoEvents()
        dg1.Rows.Clear() : mskEntryDate.TabStop = False
        Dim dt As New DataTable
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Transaction2 WHERE transtype = 'Speed Sale' and   EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' Order By  VoucherID")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        dt = clsFun.ExecDataTable("Select  * FROM Transaction2 where  EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' Order By VoucherID Desc LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString())
        If recordsCount > 15 Then
            btnFirst.Visible = True : btnLast.Visible = True
            btnNext.Visible = True : btnPrevious.Visible = True
            lblTotalRecord.Text = "Total Pages : " & TotalPages : lblTotalRecord.Visible = True
            lblPageNumber.Text = "Page No. : " & (Offset / RowCount) + 1 : lblPageNumber.Visible = True
            lbltotNug.Text = "Nug : " & Format(clsFun.ExecScalarDec("Select Sum(Nug) FROM Transaction2 WHERE transtype = 'Speed Sale' and EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' "), "0.00") : lbltotNug.Visible = True
            lblTotalWeight.Text = "Weight : " & Format(clsFun.ExecScalarDec("Select Sum(Weight) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'"), "0.00") : lblTotalWeight.Visible = True
            lblBasic.Text = "Basic : " & Format(clsFun.ExecScalarDec("Select Sum(Amount) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' "), "0.00") : lblBasic.Visible = True
            lblCharges.Text = "Charges : " & Format(clsFun.ExecScalarDec("Select Sum(Charges) FROM Transaction2 WHERE transtype = 'Speed Sale' and EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'"), "0.00") : lblCharges.Visible = True
            lblTotal.Text = "Total : " & Format(clsFun.ExecScalarDec("Select Sum(TotalAmount) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' "), "0.00") : lblTotal.Visible = True
        End If
        Try
            If dt.Rows.Count > 0 Then
                '        Application.DoEvents()
                For i = 0 To dt.Rows.Count - 1
                    'Application.DoEvents()
                    dg1.ClearSelection() : dg1.Rows.Add()
                    With dg1.Rows(i)
                        ' Application.DoEvents()
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = dt.Rows(i)("Per").ToString()
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("CommPer").ToString()), "0.00")
                        .Cells(12).Value = Format(Val(dt.Rows(i)("CommAmt").ToString()), "0.00")
                        .Cells(13).Value = Format(Val(dt.Rows(i)("Mper").ToString()), "0.00")
                        .Cells(14).Value = Format(Val(dt.Rows(i)("MAmt").ToString()), "0.00")
                        .Cells(15).Value = Format(Val(dt.Rows(i)("rdfPer").ToString()), "0.00")
                        .Cells(16).Value = Format(Val(dt.Rows(i)("RdfAmt").ToString()), "0.00")
                        .Cells(17).Value = Format(Val(dt.Rows(i)("Tare").ToString()), "0.00")
                        .Cells(18).Value = Format(Val(dt.Rows(i)("TareAmt").ToString()), "0.00")
                        .Cells(19).Value = Format(Val(dt.Rows(i)("Labour").ToString()), "0.00")
                        .Cells(20).Value = Format(Val(dt.Rows(i)("LabourAmt").ToString()), "0.00")
                        .Cells(21).Value = dt.Rows(i)("CrateMarka").ToString()
                        .Cells(22).Value = dt.Rows(i)("CrateQty").ToString()
                        .Cells(23).Value = dt.Rows(i)("MaintainCrate").ToString()
                        .Cells(24).Value = dt.Rows(i)("RoundOff").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc()
        dg1.ClearSelection()
    End Sub
    Sub calc()
        txtTotalNugs.Text = Format(0, "0.00") : txtNetAmount.Text = Format(0, "0.00")
        txtTotalWeights.Text = Format(0, "0.00") : txtTotalCharges.Text = Format(0, "0.00")
        txtTotalAmount.Text = Format(0, "0.00") : txtTotalRoff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNugs.Text = Format(Val(txtTotalNugs.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtTotalWeights.Text = Format(Val(txtTotalWeights.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotalCharges.Text = Format(Val(txtTotalCharges.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotalAmount.Text = Format(Val(txtTotalAmount.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            txtNetAmount.Text = Format(Val(txtNetAmount.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotalRoff.Text = Format(Val(txtTotalRoff.Text) + Val(dg1.Rows(i).Cells(24).Value), "0.00")
        Next
    End Sub
    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        retrive()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Offset -= RowCount
        If Offset <= 0 Then
            Offset = 0
        End If
        retrive()
    End Sub
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim currentPage As Integer = (Offset / RowCount) + 1
        If currentPage <> TotalPages Then
            Offset += RowCount
        End If
        retrive()
    End Sub
    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        Offset = (TotalPages - 1) * RowCount
        retrive()
    End Sub


    Private Sub SpeedCalculation()
        If ckTaxPaid.Checked = True Then
            Dim TotTaxPer As Decimal = Format(Val(txtComPer.Text) + Val(txtMPer.Text) + Val(txtRdfPer.Text) + 100, "0.00")
            Dim incRate As Decimal = Val(txtNetRate.Text) * (100 / TotTaxPer)
            txtrate.Text = Format(Val(incRate), "0.00")
            If CbPer.SelectedIndex = 0 Then
                txtNet.Text = Format(Val(txtNug.Text) * Val(incRate), "0.00")
            ElseIf CbPer.SelectedIndex = 1 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate), "0.00")
            ElseIf CbPer.SelectedIndex = 2 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate) / 5, "0.00")
            ElseIf CbPer.SelectedIndex = 3 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate) / 10, "0.00")
            ElseIf CbPer.SelectedIndex = 4 Then
                txtNet.Text = Format(Val(incRate) / 20 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 5 Then
                txtNet.Text = Format(Val(incRate) / 40 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 6 Then
                txtNet.Text = Format(Val(incRate) / 41 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 7 Then
                txtNet.Text = Format(Val(incRate) / 50 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 8 Then
                txtNet.Text = Format(Val(incRate) / 51 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 9 Then
                txtNet.Text = Format(Val(incRate) / 51.7 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 10 Then
                txtNet.Text = Format(Val(incRate) / 52.2 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 11 Then
                txtNet.Text = Format(Val(incRate) / 52.3 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 12 Then
                txtNet.Text = Format(Val(incRate) / 52.5 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 13 Then
                txtNet.Text = Format(Val(incRate) / 53 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 14 Then
                txtNet.Text = Format(Val(incRate) / 80 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 15 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate) / 100, "0.00")
            ElseIf CbPer.SelectedIndex = 16 Then
                txtNet.Text = Format(Val(incRate) * Val(txtNug.Text), "0.00")
            End If

        Else
            If CbPer.SelectedIndex = 0 Then
                txtNet.Text = Format(Val(txtNug.Text) * Val(txtrate.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 1 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 2 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text) / 5, "0.00")
            ElseIf CbPer.SelectedIndex = 3 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text) / 10, "0.00")
            ElseIf CbPer.SelectedIndex = 4 Then
                txtNet.Text = Format(Val(txtrate.Text) / 20 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 5 Then
                txtNet.Text = Format(Val(txtrate.Text) / 40 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 6 Then
                txtNet.Text = Format(Val(txtrate.Text) / 41 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 7 Then
                txtNet.Text = Format(Val(txtrate.Text) / 50 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 8 Then
                txtNet.Text = Format(Val(txtrate.Text) / 51 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 9 Then
                txtNet.Text = Format(Val(txtrate.Text) / 51.7 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 10 Then
                txtNet.Text = Format(Val(txtrate.Text) / 52.2 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 11 Then
                txtNet.Text = Format(Val(txtrate.Text) / 52.3 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 12 Then
                txtNet.Text = Format(Val(txtrate.Text) / 52.5 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 13 Then
                txtNet.Text = Format(Val(txtrate.Text) / 53 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 14 Then
                txtNet.Text = Format(Val(txtrate.Text) / 80 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 15 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text) / 100, "0.00")
            ElseIf CbPer.SelectedIndex = 16 Then
                txtNet.Text = Format(Val(txtrate.Text) * Val(txtNug.Text), "0.00")
            End If
        End If
        If Octroi = "Yes" Then
            txtComAmt.Text = Format(Val(txtComPer.Text) * Val(Val(txtNet.Text) + Val(txtLaboutAmt.Text)) / 100, "0.00")
        Else
            txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtNet.Text) / 100, "0.00")
        End If
        If CalculationMethod = "Nug" Then
            txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtNug.Text), "0.00")
        Else
            txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtNet.Text) / 100, "0.00")
        End If
        txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtNet.Text) / 100, "0.00")
        If ApplyCommWeight = "Yes" Then
            'txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtKg.Text), "0.00")
            If Val(txtTare.Text) > 0 Then txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
        Else
            If crateRate = "Yes" And lblCrate.Text = "Y" Then
                txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtCrateQty.Text), "0.00")
            Else
                If Val(txtTare.Text) > 0 Then txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
            End If
        End If
        ' If Val(txtLabour.Text) > 0 Then txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00") Else 
        txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
        lblTotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
        txtTotal.Text = Format(Val(lblTotCharges.Text) + Val(txtNet.Text), "0.00")
        If RoundOff = "No" Then
            Dim tmpCustAmount As Double = Val(txtTotal.Text)
            txtTotal.Text = Math.Round(Val(tmpCustAmount), 0, MidpointRounding.AwayFromZero)
            ' txtTotal.Text = Math.Round(Val(tmpCustAmount), 0)
            lblRoundOff.Text = Math.Round(Val(txtTotal.Text) - Val(tmpCustAmount), 2)
            txtTotal.Text = Format(Val(txtTotal.Text), "0.00")
        End If
    End Sub
    Private Sub txtNetRate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNetRate.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlNetRate.Visible = False
            CbPer.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub CbCustomer_GotFocus(sender As Object, e As EventArgs)
        '  CustomerFill()
        If dgItemSearch.RowCount = 0 Then txtItem.Focus() : Exit Sub
        If txtItem.Text.ToUpper <> dgItemSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtItem.Focus() : Exit Sub
        If txtItem.Text = "" Then txtItem.Focus() : Exit Sub
        If dgItemSearch.SelectedRows.Count = 0 Then Exit Sub
        If txtItem.Text.ToUpper = dgItemSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtItemID.Text = dgItemSearch.SelectedRows(0).Cells(0).Value
            itemfill()
            'BatchColums()
            'dgbatch.Visible = True
            dgItemSearch.Visible = False
        Else
            txtItem.Focus()
        End If
    End Sub
    Private Sub CbItem_KeyDown(sender As Object, e As KeyEventArgs)

    End Sub
    Private Sub CbCustomer_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=32 ", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        Dim tmpID As String = txtAccountID.Text
        If e.KeyCode = Keys.F1 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
    End Sub
    'Private Sub CbCustomer_Leave(sender As Object, e As EventArgs)
    '    If clsFun.ExecScalarInt("Select count(*)from Accounts") = 0 Then
    '        Exit Sub
    '    End If
    '    If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & CbCustomer.Text & "'") = 0 Then
    '        MsgBox("Customer Not Found in Database...", vbOkOnly, "Access Denied")
    '        CbCustomer.Focus()
    '        Exit Sub
    '    End If
    'End Sub
    'Private Sub txtLabour_KeyDown(sender As Object, e As KeyEventArgs) Handles txtLabour.KeyDown
    '    If e.KeyCode = Keys.Enter Then btnSave.Focus()
    'End Sub
    'Private Sub txtCrateQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCrateQty.KeyDown
    '    If e.KeyCode = Keys.Enter Then
    '        e.SuppressKeyPress = True
    '        SendKeys.Send("{TAB}")
    '        'If Val(clsFun.ExecScalarInt(" Select CutPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) > 0 Then
    '        '    pnlGrossWeight.Visible = True : txtGrossWt.Focus()
    '        'Else
    '        '    pnlGrossWeight.Visible = False : txtKg.Focus()
    '        'End If
    '        '    txtKg.Focus()
    '        pnlMarka.Visible = False
    '    End If
    '    Select Case e.KeyCode
    '        Case Keys.End
    '            e.Handled = True
    '            btnSave.Focus()
    '            pnlMarka.Visible = False
    '    End Select
    'End Sub

    Private Sub txtrate_GotFocus(sender As Object, e As EventArgs) Handles txtrate.GotFocus
        If ckTaxPaid.Checked = True Then
            pnlNetRate.Visible = True
            txtNetRate.Focus()
        End If
        txtrate.SelectionStart = 0 : txtrate.SelectionLength = Len(txtrate.Text)
    End Sub



    Private Sub txtnug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtComPer.KeyPress, txtKg.KeyPress,
        txtrate.KeyPress, txtNet.KeyPress, txtCrateQty.KeyPress, txtTotal.KeyPress, txtMPer.KeyPress, txtRdfPer.KeyPress, txtLabour.KeyPress, txtTare.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub


    Private Sub CbPer_Leave(sender As Object, e As EventArgs) Handles CbPer.Leave
        If txtNug.Text = "" Then txtNug.Text = Val(0)
        SpeedCalculation()
    End Sub


    Private Sub txtNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave
        If lblCrate.Text = "N" Then
            pnlMarka.Visible = False : Exit Sub
        End If
        txtNug.SelectionStart = 0
        If txtNug.Text = "" Then txtNug.Text = Val(0)
        txtCrateQty.Text = txtNug.Text
  
    End Sub

    Private Sub txtLabour_Leave(sender As Object, e As EventArgs) Handles txtLabour.Leave
        If txtLabour.Text = "" Then txtLabour.Text = "0"
    End Sub

    Private Sub txtCrateQty_Leave(sender As Object, e As EventArgs) Handles txtCrateQty.Leave
        'If Val(CUT) > 0 Then pnlGrossWeight.Visible = True : pnlGrossWeight.BringToFront() : txtGrossWt.Focus() : pnlMarka.Visible = False
    End Sub

    Private Sub txtrate_TKeyUp(sender As Object, e As KeyEventArgs) Handles txtrate.KeyUp, txtNug.KeyUp,
         txtKg.KeyUp, txtComPer.KeyUp, txtComAmt.KeyUp, txtMPer.KeyUp, txtMAmt.KeyUp, txtRdfPer.KeyUp,
         txtRdfAmt.KeyUp, txtTare.KeyUp, txtTareAmt.KeyUp, txtLabour.KeyUp, txtLaboutAmt.KeyUp,
         CbPer.KeyUp, txtCrateQty.KeyUp, txtNetRate.KeyUp
        SpeedCalculation()

    End Sub
    'Private Sub txtrate_TextChanged(sender As Object, e As EventArgs) Handles txtrate.TextChanged, txtNug.TextChanged,
    '  txtKg.TextChanged, txtNet.TextChanged, txtComPer.TextChanged, txtComAmt.TextChanged,
    ' txtMPer.TextChanged, txtMAmt.TextChanged, txtRdfPer.TextChanged, txtRdfAmt.TextChanged, txtTare.TextChanged, txtTareAmt.TextChanged,
    ' txtLabour.TextChanged, txtLaboutAmt.TextChanged, CbPer.TextChanged, txtCrateQty.TextChanged, txtNetRate.TextChanged
    '    SpeedCalculation()
    'End Sub
    Private Sub CbPer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbPer.SelectedIndexChanged
        SpeedCalculation()
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        Me.Refresh()
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
    End Sub

    Private Sub RemoveDuplicateInvoice()
        Dim dt As New DataTable
        Dim sql As String = "Select ID,billNo,InvoiceID,COUNT(ID) c FROM Vouchers Where TransType='" & Me.Text & "' GROUP BY billNo HAVING c > 1"
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    If i > 0 Then
                        '  Application.DoEvents()
                        Dim tmpbillNo As Integer = Val(dt.Rows(i)("BillNo").ToString())
                        tmpbillNo = tmpbillNo + 1
                        Dim tempInvoiceID As Integer = Val(dt.Rows(i)("InvoiceID").ToString())
                        tempInvoiceID = tempInvoiceID + 1
                        clsFun.ExecScalarStr("Update Vouchers Set BillNo='" & Val(tmpbillNo) & "',InvoiceID='" & Val(tempInvoiceID) & "' Where ID=" & Val(dt.Rows(i)("ID").ToString()) & "")
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Save()
        ' BackgroundWorker1.RunWorkerAsync()
        'Application.DoEvents()
        Dim sql As String = String.Empty

        If CbPer.SelectedIndex = 0 Then
            If Val(txtNug.Text) = 0 Then MsgBox("please fill Nug...", MsgBoxStyle.Critical, "Access Denied") : txtNug.Focus() : Exit Sub
        ElseIf CbPer.SelectedIndex = 14 Then
            If lblCrate.Text = "Y" Then
                pnlMarka.Visible = True : pnlMarka.BringToFront()
                If Val(txtCrateQty.Text) = 0 Then MsgBox("please fill Crate Qty...", MsgBoxStyle.Critical, "Access Denied") : txtCrateQty.Focus() : Exit Sub
            End If
        Else
            If Val(txtKg.Text) = 0 Then MsgBox("please fill Weight...", MsgBoxStyle.Critical, "Access Denied") : txtKg.Focus() : Exit Sub
        End If
        Dim cmd As SQLite.SQLiteCommand
        Dim UserID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "Insert Into Vouchers(Transtype, EntryDate,billNo,InvoiceID,UserID,EntryTime) Values (@1,@2,@3,@4,@5,@6)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@3", Val(txtSlipNo.Text))
            cmd.Parameters.AddWithValue("@4", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@5", Val(UserID))

            cmd.Parameters.AddWithValue("@6", Now.ToString("yyyy-MM-dd HH:mm:ss"))
            If cmd.ExecuteNonQuery() > 0 Then
                el.WriteToErrorLog(sql, Constants.compname, "Saved")
                txtid.Text = Val(clsFun.ExecScalarInt("Select max(ID) ID from Vouchers;"))
                InsertTrans() : ServerTag = 1
                InsertLedger() : CrateLedger()
                MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
                ServerLedger() : ServerCrate()
                txtclear()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub txtKg_GotFocus(sender As Object, e As EventArgs) Handles txtKg.GotFocus, txtKg.Click
        txtKg.SelectAll() : pnlGrossWeight.Visible = False : pnlMarka.Visible = False
        If txtAddWeight.Text = "" Then lblAddWeight.Text = "" Else lblAddWeight.Text = txtAddWeight.Text
        lblAddWeight.Visible = True
    End Sub
    Private Sub txtAddWeight_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAddWeight.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") Or ((e.KeyChar = "+") = -1)))
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
    Private Sub InsertTrans()
        Dim sql As String = String.Empty
        sql = "insert into Transaction2(TransType, Entrydate, ItemID, ItemName, AccountID, AccountName, Nug, Weight, Rate, Per," _
                        & " Amount, TotalAmount, CommPer, CommAmt, MPer, MAmt, RdfPer, RdfAmt," _
                        & " Tare, TareAmt, Labour, LabourAmt, Charges, MaintainCrate, CrateMarka, " _
                        & " CrateQty,CrateID, BillNo,VoucherID,CrateAccountID,CrateAccountName,RoundOff,Cut,OnWeight,GrossWeight)" _
                        & " Select  '" & Me.Text & "', '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "', " & Val(txtItemID.Text) & ", " _
                        & "'" & txtItem.Text & "', " & Val(txtAccountID.Text) & ", '" & txtAccount.Text & "', " & Val(txtNug.Text) & ", " & Val(txtKg.Text) & ", " _
                        & "" & Val(txtrate.Text) & ", '" & CbPer.Text & "'," & Val(txtNet.Text) & ", " & Val(txtTotal.Text) & ", " & Val(txtComPer.Text) & ", " _
                        & "" & Val(txtComAmt.Text) & ", " & Val(txtMPer.Text) & ", " & Val(txtMAmt.Text) & ", " & Val(txtRdfPer.Text) & ", " & Val(txtRdfAmt.Text) & ", " _
                        & "" & Val(txtTare.Text) & ", " & Val(txtTareAmt.Text) & "," & Val(txtLabour.Text) & ", " & Val(txtLaboutAmt.Text) & "," _
                        & "" & Val(lblTotCharges.Text) & ", '" & lblCrate.Text & "', '" & cbCrateMarka.Text & "'," & Val(txtCrateQty.Text) & ", " _
                        & "" & If(cbCrateMarka.Text <> "Y", Val(0), Val(cbCrateMarka.SelectedValue)) & ",'" & txtSlipNo.Text & "', " _
                        & "" & Val(txtid.Text) & "," & Val(cbAccountName.SelectedValue) & ",'" & cbAccountName.Text & "'," & Val(lblRoundOff.Text) & "," & Val(txtGrossWt.Text) & ",'" & txtAddWeight.Text & "'," & Val(txtGrossWt.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql, True) > 0 Then
                ' el.WriteToErrorLog(sql, "", "Speed Record")
                'clsFun.CloseConnection()
                '  MsgBox("SAVED")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub InsertLedger()
        '   Application.DoEvents()
        Dim HindiItemName As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID=(Select ItemID From TransAction2 Where VoucherID='" & Val(txtid.Text) & "')")
        Dim Remark As String = clsFun.ExecScalarStr("Select  ItemName ||', Nug : '||(nug)||', Weight : '|| (weight) ||', On '||(rate)||' /- '|| Per  From Transaction2 where VoucherID=" & Val(txtid.Text) & " and TransType='Speed Sale'")
        Dim RemarkHindi As String = HindiItemName & clsFun.ExecScalarStr(" Select ', नग : '||(nug)||', वज़न : '|| (weight) ||', भाव '||(rate)||' /- '|| Per  From Transaction2 where VoucherID=" & Val(txtid.Text) & " and TransType='Speed Sale'")
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim fastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        If Val(txtAccountID.Text) > 0 Then ''Party Account
            If txtAccountID.Text = cbAccountName.SelectedValue AndAlso cbAccountName.SelectedValue <> 0 Then
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtTotal.Text) - Val(txtTareAmt.Text) & ",'D' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            ElseIf Val(txtAccountID.Text) <> cbAccountName.SelectedValue AndAlso cbAccountName.SelectedValue <> 0 Then
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtTotal.Text) - Val(txtTareAmt.Text) & ",'D' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & cbAccountName.SelectedValue & ",'" & cbAccountName.Text & "'," & Val(txtTareAmt.Text) & ",'D' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            ElseIf Val(txtTotal.Text) > 0 Then
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtTotal.Text) & ",'D' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            End If

        End If
        If Val(txtNet.Text) > 0 Then ''Maal Khata Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 29 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "'," & Val(txtNet.Text) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtComAmt.Text) > 0 Then ''Commmision Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 10 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "'," & Val(txtComAmt.Text) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtMAmt.Text) > 0 Then ''Mandi Tax Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 30 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "'," & Val(txtMAmt.Text) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtRdfAmt.Text) > 0 Then ''RDF Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 39 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "'," & Val(txtRdfAmt.Text) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtTareAmt.Text) > 0 Then ''Tare Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 4 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "'," & Val(txtTareAmt.Text) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtLaboutAmt.Text) > 0 Then ''Labour Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 23 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "'," & Val(txtLaboutAmt.Text) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        ''**********************************************************************************
        If clsFun.ExecScalarStr("Select SaleRO From Controls") = "No" Then
            If Val(lblRoundOff.Text) <> 0 Then ''Account 
                If Val(lblRoundOff.Text) < 0 Then
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Math.Abs(Val(lblRoundOff.Text)) & ",'D' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                Else
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Val(lblRoundOff.Text) & ",'C' ,'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                End If
            End If
        End If
        If fastQuery = "" Then Exit Sub
        clsFun.FastLedger(fastQuery)
    End Sub

    Private Sub ServerLedger()
        ' Application.DoEvents()
        If OrgID = 0 Then Exit Sub
        '   Application.DoEvents()
        Dim HindiItemName As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID=(Select ItemID From TransAction2 Where VoucherID='" & Val(txtid.Text) & "')")
        Dim Remark As String = clsFun.ExecScalarStr("Select  ItemName ||', Nug : '||(nug)||', Weight : '|| (weight) ||', On '||(rate)||' /- '|| Per ||' = '|| amount From Transaction2 where VoucherID=" & Val(txtid.Text) & " and TransType='Speed Sale'")
        Dim RemarkHindi As String = HindiItemName & clsFun.ExecScalarStr(" Select ', नग : '||(nug)||', वज़न : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount From Transaction2 where VoucherID=" & Val(txtid.Text) & " and TransType='Speed Sale'")
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim fastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        If Val(txtAccountID.Text) > 0 Then ''Party Account
            If txtAccountID.Text = cbAccountName.SelectedValue AndAlso cbAccountName.SelectedValue <> 0 Then
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtTotal.Text) - Val(txtTareAmt.Text) & ",'D', " & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            ElseIf Val(txtAccountID.Text) <> cbAccountName.SelectedValue AndAlso cbAccountName.SelectedValue <> 0 Then
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtTotal.Text) - Val(txtTareAmt.Text) & ",'D' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & cbAccountName.SelectedValue & ",'" & cbAccountName.Text & "'," & Val(txtTareAmt.Text) & ",'D' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            ElseIf Val(txtTotal.Text) > 0 Then
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtTotal.Text) & ",'D' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
            End If

        End If
        If Val(txtNet.Text) > 0 Then ''Maal Khata Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 29 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=29") & "'," & Val(txtNet.Text) & ",'C' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtComAmt.Text) > 0 Then ''Commmision Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 10 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=10") & "'," & Val(txtComAmt.Text) & ",'C' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtMAmt.Text) > 0 Then ''Mandi Tax Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 30 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=30") & "'," & Val(txtMAmt.Text) & ",'C' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtRdfAmt.Text) > 0 Then ''RDF Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 39 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=39") & "'," & Val(txtRdfAmt.Text) & ",'C' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtTareAmt.Text) > 0 Then ''Tare Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 4 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=4") & "'," & Val(txtTareAmt.Text) & ",'C' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        If Val(txtLaboutAmt.Text) > 0 Then ''Labour Account
            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 23 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=23") & "'," & Val(txtLaboutAmt.Text) & ",'C' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
        End If
        ''**********************************************************************************
        If clsFun.ExecScalarStr("Select SaleRO From Controls") = "No" Then
            If Val(lblRoundOff.Text) <> 0 Then ''Account 
                If Val(lblRoundOff.Text) < 0 Then
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Math.Abs(Val(lblRoundOff.Text)) & ",'D' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                Else
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & 42 & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "'," & Val(lblRoundOff.Text) & ",'C' ," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark & "','" & txtAccount.Text & "','" & RemarkHindi & "'"
                End If
            End If
        End If
        If fastQuery = "" Then Exit Sub
        'Sql = "Insert into Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,ServerTag,ORGID,Remark,Narration,RemarkHindi) SELECT " & fastQuery & ";"
        ''  Sql = "Insert into Ledger(VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,ServerTag,ORGID,Remark,Narration,RemarkHindi)values" & fastQuery.Remove(fastQuery.LastIndexOf(","))
        'ClsFunserver.ExecNonQuery(Sql)
        ClsFunserver.FastLedger(fastQuery)
    End Sub

    Private Sub ServerCrate()
        Dim fastQuery As String = String.Empty
        If OrgID = 0 Then Exit Sub
        If lblCrate.Text = "Y" Then ''Party Account
            If Val(txtAccountID.Text) = 7 Then
                If cbAccountName.SelectedValue > 0 Then
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(cbAccountName.SelectedValue) & ",'" & cbAccountName.Text & "','Crate Out'," & Val(cbCrateMarka.SelectedValue) & ",'" & cbCrateMarka.Text & "','" & Val(txtCrateQty.Text) & "', '','" & txtTare.Text & "','" & Val(txtTareAmt.Text) & "', ''," & Val(ServerTag) & "," & Val(OrgID) & ""
                End If
            Else
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(cbCrateMarka.SelectedValue) & ",'" & cbCrateMarka.Text & "','" & Val(txtCrateQty.Text) & "', '','" & txtTare.Text & "','" & Val(txtTareAmt.Text) & "', ''," & Val(ServerTag) & "," & Val(OrgID) & ""
            End If
        End If
        If fastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(fastQuery)

    End Sub

    Private Sub VNumber()
        If VNo = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
            VNo = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtSlipNo.Text = VNo + 1
            txtInvoiceID.Text = VNo + 1
        Else
            VNo = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtSlipNo.Text = VNo + 1
            txtInvoiceID.Text = VNo + 1
        End If
    End Sub
    Private Sub CrateLedger()
        Dim fastQuery As String = String.Empty
        If lblCrate.Text = "Y" Then ''Party Account
            If Val(txtAccountID.Text) = 7 Then
                If cbAccountName.SelectedValue > 0 Then
                    '               clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(cbAccountName.SelectedValue), cbAccountName.Text, "Crate Out", Val(cbCrateMarka.SelectedValue), cbCrateMarka.Text, Val(txtCrateQty.Text), "", Val(txtTare.Text), Val(txtTareAmt.Text), "")
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(cbAccountName.SelectedValue) & ",'" & cbAccountName.Text & "','Crate Out'," & Val(cbCrateMarka.SelectedValue) & ",'" & cbCrateMarka.Text & "','" & Val(txtCrateQty.Text) & "', '','" & txtTare.Text & "','" & Val(txtTareAmt.Text) & "', ''"

                End If
            Else
                '                clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, "Crate Out", Val(cbCrateMarka.SelectedValue), cbCrateMarka.Text, Val(txtCrateQty.Text), "", Val(txtTare.Text), Val(txtTareAmt.Text), "")
                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate Out'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(cbCrateMarka.SelectedValue) & ",'" & cbCrateMarka.Text & "','" & Val(txtCrateQty.Text) & "', '','" & txtTare.Text & "','" & Val(txtTareAmt.Text) & "', ''"
            End If
        End If
        If fastQuery = String.Empty Then Exit Sub
        clsFun.FastCrateLedger(fastQuery)
    End Sub
    Private Sub UpdateRecord()
        ' Application.DoEvents()
        Dim SqliteEntryDate As String = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        If CbPer.SelectedIndex = 0 Then
            If Val(txtNug.Text) = 0 Then MsgBox("please fill Nug or Kg", MsgBoxStyle.Critical, "Access Denied") : txtNug.Focus() : Exit Sub
        ElseIf CbPer.SelectedIndex = 1 Then
            If Val(txtKg.Text) = 0 Then MsgBox("please fill Weight...", MsgBoxStyle.Critical, "Access Denied") : txtKg.Focus() : Exit Sub
        End If
        Dim cmd As SQLite.SQLiteCommand
        Dim ModifyByID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "Update Vouchers SET TransType='" & Me.Text & "',Entrydate='" & SqliteEntryDate & "',InvoiceID='" & Val(txtInvoiceID.Text) & "', " &
              " billNo='" & Val(txtSlipNo.Text) & "',ModifiedByID='" & Val(ModifyByID) & "',ModifiedTime='" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "' " &
              " where ID =" & Val(txtid.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                ServerTag = 1
                el.WriteToErrorLog(sql, Constants.compname, "Speed Sale Updated")
                If clsFun.ExecScalarStr("Select Count(VoucherID) From CrateVoucher Where VoucherID='" & Val(txtid.Text) & "'") > 0 And lblCrate.Text = "Y" Then
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                              " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                Else
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where  VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                              " Delete From CrateVoucher Where   VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                    UpdateCrate()
                End If
                clsFun.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & Val(txtid.Text) & "; " &
                       "Delete From CrateVoucher Where TransType<>'Op bal' and VoucherID= " & Val(txtid.Text) & "")
                sql = "Update Transaction2 SET TransType='" & Me.Text & "', BillNo='" & txtSlipNo.Text & "',Entrydate='" & SqliteEntryDate & "', ItemID=" & Val(txtItemID.Text) & ", ItemName= '" & txtItem.Text & "', AccountID=" & Val(txtAccountID.Text) & ", AccountName='" & txtAccount.Text & "', Nug=" & Val(txtNug.Text) & ", Weight=" & Val(txtKg.Text) & ", Rate=" & Val(txtrate.Text) & ", Per='" & CbPer.Text & "'," _
                          & " Amount=" & Val(txtNet.Text) & ", TotalAmount=" & Val(txtTotal.Text) & ", CommPer=" & Val(txtComPer.Text) & ", CommAmt= " & Val(txtComAmt.Text) & ", MPer=" & Val(txtMPer.Text) & ", MAmt= " & Val(txtMAmt.Text) & " , RdfPer= " & Val(txtRdfPer.Text) & ", RdfAmt= " & Val(txtRdfAmt.Text) & "," _
                          & " Tare=" & Val(txtTare.Text) & ", TareAmt= " & Val(txtTareAmt.Text) & ", Labour= " & Val(txtLabour.Text) & ", LabourAmt=" & Val(txtLaboutAmt.Text) & ", Charges=" & Val(lblTotCharges.Text) & ", MaintainCrate='" & lblCrate.Text & "', CrateID= " & Val(cbCrateMarka.SelectedValue) & ", " _
                          & " CrateMarka= '" & cbCrateMarka.Text & "',CrateQty=" & Val(txtCrateQty.Text) & ",CrateAccountID=" & Val(cbAccountName.SelectedValue) & ",CrateAccountName='" & cbAccountName.Text & "',RoundOff='" & Val(lblRoundOff.Text) & "',cut=0,OnWeight='" & txtAddWeight.Text & "' where VoucherID =" & Val(txtid.Text) & ""
                clsFun.ExecNonQuery(sql)
                ServerTag = 1 : InsertLedger() : CrateLedger() : ServerLedger() : ServerCrate()
                MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                txtclear()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Public Sub Alert(ByVal msg As String, ByVal type As msgAlert.enmType)
        Dim frm As msgAlert = New msgAlert()
        frm.showAlert(msg, type)
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

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Val(txtItemID.Text) = 0 Then txtItem.Focus() : Exit Sub
        If Val(txtAccountID.Text) = 0 Then txtAccount.Focus() : Exit Sub
        If txtLabour.Text = "" Then txtLabour.Text = "0"
        If txtTare.Text = "" Then txtTare.Text = "0"
        If txtRdfPer.Text = "" Then txtRdfPer.Text = "0"
        If txtMPer.Text = "" Then txtMPer.Text = "0"
        If txtComPer.Text = "" Then txtComPer.Text = "0"
        If btnSave.Text = "&Save" Then
            Dim addSale As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
            If addSale <> "Y" Then MsgBox("You Don't Have Rights to Add Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : txtclear() : Exit Sub
            ButtonControl() : Save() : ButtonControl()
            If Application.OpenForms().OfType(Of Speed_Sale_Register).Any = True Then If Speed_Sale_Register.ckAll.Checked = False Then Speed_Sale_Register.retriveAll() Else Speed_Sale_Register.retrive()
        Else
            Dim ModifySale As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Sale'")
            If ModifySale <> "Y" Then MsgBox("You Don't Have Rights to Modify Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : txtclear() : Exit Sub
            ButtonControl() : UpdateRecord() : ButtonControl()
        End If
        If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
        calc()
    End Sub

    Private Sub txtItem_GotFocus(sender As Object, e As EventArgs) Handles txtItem.GotFocus, txtAccount.GotFocus,
           txtNug.GotFocus, txtKg.GotFocus, txtrate.GotFocus, txtComPer.GotFocus, txtLaboutAmt.GotFocus,
           txtComAmt.GotFocus, txtMPer.GotFocus, txtMAmt.GotFocus, txtRdfPer.GotFocus, txtRdfAmt.GotFocus, txtTare.GotFocus, txtTareAmt.GotFocus, txtLabour.GotFocus
        If txtItem.Focused Then
            If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
            If dgItemSearch.RowCount = 0 Then retriveItems()
            If txtItem.Text.Trim() <> "" Then
                retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
            Else
                retriveItems()
            End If
            If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus()
            txtItem.SelectAll()
        End If

        If txtAccount.Focused Then
            If dgItemSearch.ColumnCount = 0 Then ItemRowColumns()
            If dgItemSearch.Rows.Count = 0 Then retriveItems()
            If txtItem.Text.Trim() <> "" Then
                retriveItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
            Else
                retriveItems()
            End If
            If dgItemSearch.SelectedRows.Count = 0 Then dgItemSearch.Visible = True : txtItem.Focus() : Exit Sub
            txtItemID.Text = Val(dgItemSearch.SelectedRows(0).Cells(0).Value)
            txtItem.Text = dgItemSearch.SelectedRows(0).Cells(1).Value
            dgItemSearch.Visible = False : itemfill()
            If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
            If DgAccountSearch.RowCount = 0 Then retriveAccounts()
            If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
            If txtAccount.Text.Trim() <> "" Then
                retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
            Else
                retriveAccounts()
            End If
            If lblCrate.Text = "N" Then pnlMarka.Visible = False
            txtAccount.SelectAll()
        End If
        If txtNug.Focused Then
            If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
            If DgAccountSearch.RowCount = 0 Then retriveAccounts()
            If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
            txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False : AcBal()
            If lblCrate.Text = "Y" Then
                If Val(txtAccountID.Text) = 7 Then
                    cbAccountName.Enabled = True : If dg1.SelectedRows.Count = 0 Then cbAccountName.SelectedIndex = 0
                Else
                    cbAccountName.Enabled = False : cbAccountName.SelectedIndex = -1
                End If
                pnlMarka.Visible = True : pnlMarka.BringToFront()
            Else
                pnlMarka.Visible = False
                txtCrateQty.Text = 0
            End If
        End If
        If previouslyFocusedTextBox IsNot Nothing Then
            previouslyFocusedTextBox.BackColor = Color.GhostWhite
        End If
        'If txtComPer.Focused Then txtComPer.BackColor = Color.FromArgb(247, 200, 111) : Exit Sub
        If txtComPer.Focused Then txtComPer.BackColor = Color.LightGray : Exit Sub
        If txtComAmt.Focused Then txtComAmt.BackColor = Color.LightGray : Exit Sub
        If txtMPer.Focused Then txtMPer.BackColor = Color.LightGray : Exit Sub
        If txtMAmt.Focused Then txtMAmt.BackColor = Color.LightGray : Exit Sub
        If txtRdfPer.Focused Then txtRdfPer.BackColor = Color.LightGray : Exit Sub
        If txtRdfAmt.Focused Then txtRdfAmt.BackColor = Color.LightGray : Exit Sub
        If txtLabour.Focused Then txtLabour.BackColor = Color.LightGray : Exit Sub
        If txtLaboutAmt.Focused Then txtLaboutAmt.BackColor = Color.LightGray : Exit Sub
        If txtTare.Focused Then txtTare.BackColor = Color.LightGray : Exit Sub
        If txtTareAmt.Focused Then txtTareAmt.BackColor = Color.LightGray : Exit Sub
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.ForeColor = Color.Maroon
        'tb.BackColor = Color.GhostWhite
        tb.SelectAll() : If btnSave.Text = "&Save" Then dg1.ClearSelection()
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
            Dim AccountID As String = DgAccountSearch.SelectedRows(0).Cells(0).Value
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
    'Private Sub CbPer_KeyDown(sender As Object, e As KeyEventArgs) Handles CbPer.KeyDown
    '    'If e.KeyCode = Keys.Enter Then btnSave.Focus()
    'End Sub

    Private Sub cbCrateMarka_GotFocus(sender As Object, e As EventArgs) Handles cbCrateMarka.GotFocus
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper <> DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then txtAccount.Focus() : Exit Sub
        If txtAccount.Text.ToUpper = DgAccountSearch.SelectedRows(0).Cells(1).Value.ToString.ToUpper Then
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            AcBal()
            txtNug.SelectionStart = 0 : txtNug.SelectionLength = Len(txtNug.Text)
            DgAccountSearch.Visible = False
        Else
            txtAccount.Focus() : Exit Sub
        End If
    End Sub
    Public Sub RefreshAccounts()
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33,11) order by AccountName ", "AccountName", "ID", "--N./A.--")
    End Sub
    Private Sub cbAccountName_GotFocus(sender As Object, e As EventArgs) Handles cbAccountName.GotFocus
        If lblCrate.Text = "Y" Then
            If txtAccountID.Text = 7 Then
                txtCrateQty.Text = txtNug.Text
                pnlMarka.Visible = True
                pnlMarka.BringToFront()
                cbAccountName.Focus()
            Else
                txtCrateQty.Text = txtNug.Text
                pnlMarka.Visible = True
                pnlMarka.BringToFront()
                cbCrateMarka.Focus()
            End If
        Else
            pnlMarka.SendToBack()
            pnlMarka.Visible = False
        End If
    End Sub

    Private Sub cbAccount_keydown(sender As Object, e As KeyEventArgs) Handles cbAccountName.KeyDown
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
            CreateAccount.BringToFront()
        End If
    End Sub

    Private Sub txtGrossWt_GotFocus(sender As Object, e As EventArgs) Handles txtGrossWt.GotFocus
        txtGrossWt.SelectAll()
    End Sub
    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtItem.KeyDown, txtAccount.KeyDown, txtNet.KeyDown, txtGrossWt.KeyDown,
        txtNug.KeyDown, txtKg.KeyDown, txtrate.KeyDown, cbCrateMarka.KeyDown, cbAccountName.KeyDown, CbPer.KeyDown, txtComPer.KeyDown, txtLaboutAmt.KeyDown, txtCrateQty.KeyDown,
        txtComAmt.KeyDown, txtMPer.KeyDown, txtMAmt.KeyDown, txtRdfPer.KeyDown, txtRdfAmt.KeyDown, txtTare.KeyDown, txtTareAmt.KeyDown, txtLabour.KeyDown, ckTaxPaid.KeyDown
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
                txtAddWeight.Focus() : txtAddWeight.SelectAll()
            End If
            ' e.SuppressKeyPress = True
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnSave.Focus()
        End Select
        If e.KeyCode = Keys.Control AndAlso Keys.L Then
            MsgBox("Ledger Pressed")
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.Down Then
                If dgItemSearch.Visible = True Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then
                If DgAccountSearch.Visible = True Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If DgAccountSearch.Visible = False And dgItemSearch.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or CbPer.Focused = True Or cbCrateMarka.Focused = True Or cbAccountName.Focused = True Or txtCrateQty.Focused = True Then Exit Sub
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
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
    End Sub

    Public Sub FillSpeedSale()
        ssql = "Select * from Controls"
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("SpeedCommission").ToString().Trim() = "Percentage" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = False
            ElseIf dt.Rows(0)("SpeedCommission").ToString().Trim() = "Amount" Then
                txtComAmt.TabStop = True : txtComPer.TabStop = False
            ElseIf dt.Rows(0)("SpeedCommission").ToString().Trim() = "Both" Then
                txtComPer.TabStop = True : txtComAmt.TabStop = True
            Else
                txtComPer.TabStop = False : txtComAmt.TabStop = False
            End If
            If dt.Rows(0)("SpeedMandiTax").ToString().Trim() = "Percentage" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = False
            ElseIf dt.Rows(0)("SpeedMandiTax").ToString().Trim() = "Amount" Then
                txtMAmt.TabStop = True : txtMPer.TabStop = False
            ElseIf dt.Rows(0)("SpeedMandiTax").ToString().Trim() = "Both" Then
                txtMPer.TabStop = True : txtMAmt.TabStop = True
            Else
                txtMPer.TabStop = False : txtMAmt.TabStop = False
            End If
            If dt.Rows(0)("SpeedRDF").ToString().Trim() = "Percentage" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = False
            ElseIf dt.Rows(0)("SpeedRDF").ToString().Trim() = "Amount" Then
                txtRdfAmt.TabStop = True : txtRdfPer.TabStop = False
            ElseIf dt.Rows(0)("SpeedRDF").ToString().Trim() = "Both" Then
                txtRdfPer.TabStop = True : txtRdfAmt.TabStop = True
            Else
                txtRdfPer.TabStop = False : txtRdfAmt.TabStop = False
            End If
            If dt.Rows(0)("SpeedTare").ToString().Trim() = "Nug" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = False
            ElseIf dt.Rows(0)("SpeedTare").ToString().Trim() = "Amount" Then
                txtTareAmt.TabStop = True : txtTare.TabStop = False
            ElseIf dt.Rows(0)("SpeedTare").ToString().Trim() = "Both" Then
                txtTare.TabStop = True : txtTareAmt.TabStop = True
            Else
                txtTare.TabStop = False : txtTareAmt.TabStop = False
            End If
            If dt.Rows(0)("SpeedLabour").ToString().Trim() = "Nug" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = False
            ElseIf dt.Rows(0)("SpeedLabour").ToString().Trim() = "Amount" Then
                txtLaboutAmt.TabStop = True : txtLabour.TabStop = False
            ElseIf dt.Rows(0)("SpeedLabour").ToString().Trim() = "Both" Then
                txtLabour.TabStop = True : txtLaboutAmt.TabStop = True
            Else
                txtLabour.TabStop = False : txtLaboutAmt.TabStop = False
            End If
            If dt.Rows(0)("SpeedTaxPaid").ToString().Trim() = "Yes" Then ckTaxPaid.Visible = True Else ckTaxPaid.Visible = False
            If dt.Rows(0)("StopBasic").ToString().Trim() = "Yes" Then txtNet.TabStop = True : txtNet.ReadOnly = False Else txtNet.TabStop = False
            If dt.Rows(0)("SpeedTaxPaid").ToString().Trim() = "Yes" Then ckTaxPaid.Visible = True Else ckTaxPaid.Visible = False
            Octroi = dt.Rows(0)("Octroi").ToString().Trim()
            CalculationMethod = dt.Rows(0)("SpeedKaat").ToString().Trim()
            crateRate = dt.Rows(0)("CrateBardana").ToString().Trim()
            RoundOff = dt.Rows(0)("SaleRO").ToString().Trim()
            ItemPer = dt.Rows(0)("Per").ToString().Trim()
            If ItemPer <> "itemWise" Then CbPer.Text = dt.Rows(0)("Per").ToString().Trim()
        End If
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        mskEntryDate.Enabled = True : mskEntryDate.TabStop = True
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        FillContros(dg1.SelectedRows(0).Cells(0).Value)
        'mskEntryDate.Focus()
    End Sub

    Private Sub cbCrateMarka_KeyDown(sender As Object, e As KeyEventArgs) Handles cbCrateMarka.KeyDown
        If e.KeyCode = Keys.F3 Then
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            If Not CrateForm Is Nothing Then
                CrateForm.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            Dim tmpMarkaID As Integer = cbCrateMarka.SelectedValue
            If tmpMarkaID = 0 Then Exit Sub
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            CrateForm.FillControls(tmpMarkaID)
            If Not CrateForm Is Nothing Then
                CrateForm.BringToFront()
            End If
        End If
    End Sub
    Private Sub ckTaxPaid_GotFocus(sender As Object, e As EventArgs) Handles ckTaxPaid.GotFocus
        ckTaxPaid.ForeColor = Color.GhostWhite
        ckTaxPaid.BackColor = Color.Green
    End Sub

    Private Sub ckTaxPaid_LostFocus(sender As Object, e As EventArgs) Handles ckTaxPaid.LostFocus
        ckTaxPaid.ForeColor = Color.Black
        ckTaxPaid.BackColor = Color.FromArgb(247, 220, 111)
    End Sub
    Private Sub cbCrateMarka_Leave(sender As Object, e As EventArgs) Handles cbCrateMarka.Leave
        If lblCrate.Text = "Y" Then
            If clsFun.ExecScalarInt("Select count(*)from CrateMarka") = 0 Then
                CrateForm.MdiParent = MainScreenForm
                CrateForm.Show()
                If Not CrateForm Is Nothing Then
                    CrateForm.BringToFront()
                    CrateForm.txtCratename.Focus()
                End If
                Exit Sub
            End If
        End If
        If clsFun.ExecScalarInt("Select count(*)from CrateMarka where MarkaName='" & cbCrateMarka.Text & "'") = 0 Then
            MsgBox("Crate Not Found in Database...", vbOKOnly, "Access Denied")
            cbCrateMarka.Focus()
            Exit Sub
        End If
        Dim crateRate As String = clsFun.ExecScalarStr("Select CrateBardana From Controls")
        If crateRate = "Yes" Then txtTare.Text = clsFun.ExecScalarStr("Select Rate From CrateMarka Where ID='" & Val(cbCrateMarka.SelectedValue) & "'")
    End Sub
    
    Private Sub AccountComm()
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
            If ItemPer = "ItemWise" Then CbPer.Text = row("RateAs").ToString()
            trackStock = row("TrackStock").ToString()
        End If
        'AccountComm()
    End Sub
    
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 180
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 150
        retriveAccounts()
    End Sub

    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        '  AccountRowColumns()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub

    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        If ckShowSupplier.Checked Then
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16,17)  or UnderGroupID in (11,16,17))" & condtion & " order by AccountName Limit 11")
        Else
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,16)  or UnderGroupID in (11,16))" & condtion & " order by AccountName  Limit 11")
        End If

        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = Val(dt.Rows(i)("id").ToString())
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


    Private Sub dg1_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles dg1.RowStateChanged
        ' If btnSave.Text = "&Save" Then
        If dg1.RowCount > 0 Then
            mskEntryDate.Enabled = False
            mskEntryDate.TabStop = False
        Else
            mskEntryDate.Enabled = True
            mskEntryDate.TabStop = True
        End If
        'Else
        'mskEntryDate.Enabled = True
        'End If
    End Sub
    Private Sub txtItem_LostFocus(sender As Object, e As EventArgs) Handles txtItem.LostFocus, txtAccount.LostFocus, txtNug.LostFocus, txtKg.LostFocus, txtrate.LostFocus, txtComPer.LostFocus, txtLaboutAmt.LostFocus, txtComAmt.LostFocus, txtMPer.LostFocus, txtMAmt.LostFocus, txtRdfPer.LostFocus, txtRdfAmt.LostFocus, txtTare.LostFocus, txtTareAmt.LostFocus, txtLabour.LostFocus
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
            tb.ForeColor = Color.Black
        End If
    End Sub

    Private Sub lblCrate_TextChanged(sender As Object, e As EventArgs) Handles lblCrate.TextChanged
        If lblCrate.Text = "N" Then lblCrate.Visible = False : txtCrateQty.Clear()
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        If btnClose.Enabled = False Then Exit Sub
        Me.Close() : MainScreenPicture.retrive()
    End Sub

    Private Sub mskEntryDate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles mskEntryDate.KeyPress
        If mskEntryDate.SelectionLength = Len(mskEntryDate.Text) Then
            mskEntryDate.Clear()
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ButtonControl() : Delete() : ButtonControl()
    End Sub

    Private Sub txtSlipNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSlipNo.KeyDown
        If e.KeyCode = Keys.F2 Then
            pnlInvoiceID.Visible = True
            e.SuppressKeyPress = True
        End If
    End Sub

    Public Sub MultiUpdateSpeed()
        ButtonControl()
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        If CbPer.SelectedIndex = 0 Then
            If Val(txtNug.Text) = 0 Then MsgBox("please fill Nug or Kg", MsgBoxStyle.Critical, "Access Denied") : txtNug.Focus() : Exit Sub
        ElseIf CbPer.SelectedIndex = 1 Then
            If Val(txtKg.Text) = 0 Then MsgBox("please fill Weight...", MsgBoxStyle.Critical, "Access Denied") : txtKg.Focus() : Exit Sub
        End If
        Dim cmd As SQLite.SQLiteCommand
        Dim ModifyByID As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        sql = "Update Vouchers SET TransType='" & Me.Text & "',Entrydate='" & SqliteEntryDate & "',InvoiceID='" & Val(txtInvoiceID.Text) & "', " & _
              " billNo='" & Val(txtSlipNo.Text) & "',ModifiedByID='" & Val(ModifyByID) & "',ModifiedTime='" & Now.ToString("yyyy-MM-dd HH:mm:ss") & "' " & _
              " where ID =" & Val(txtid.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                ServerTag = 1
                el.WriteToErrorLog(sql, Constants.compname, "Speed Sale Updated")
                If clsFun.ExecScalarStr("Select Count(VoucherID) From CrateVoucher Where VoucherID='" & Val(txtid.Text) & "'") > 0 And lblCrate.Text = "Y" Then
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " & _
                                              " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                Else
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where  VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " & _
                                              " Delete From CrateVoucher Where   VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                    UpdateCrate()
                End If
                clsFun.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & Val(txtid.Text) & "; " & _
                       "Delete From CrateVoucher Where VoucherID= " & Val(txtid.Text) & "")
                sql = "Update Transaction2 SET TransType='" & Me.Text & "', BillNo='" & txtSlipNo.Text & "',Entrydate='" & SqliteEntryDate & "', ItemID=" & Val(txtItemID.Text) & ", ItemName= '" & txtItem.Text & "', AccountID=" & Val(txtAccountID.Text) & ", AccountName='" & txtAccount.Text & "', Nug=" & Val(txtNug.Text) & ", Weight=" & Val(txtKg.Text) & ", Rate=" & Val(txtrate.Text) & ", Per='" & CbPer.Text & "'," _
                          & " Amount=" & Val(txtNet.Text) & ", TotalAmount=" & Val(txtTotal.Text) & ", CommPer=" & Val(txtComPer.Text) & ", CommAmt= " & Val(txtComAmt.Text) & ", MPer=" & Val(txtMPer.Text) & ", MAmt= " & Val(txtMAmt.Text) & " , RdfPer= " & Val(txtRdfPer.Text) & ", RdfAmt= " & Val(txtRdfAmt.Text) & "," _
                          & " Tare=" & Val(txtTare.Text) & ", TareAmt= " & Val(txtTareAmt.Text) & ", Labour= " & Val(txtLabour.Text) & ", LabourAmt=" & Val(txtLaboutAmt.Text) & ", Charges=" & Val(lblTotCharges.Text) & ", MaintainCrate='" & lblCrate.Text & "', CrateID= " & Val(cbCrateMarka.SelectedValue) & ", " _
                          & " CrateMarka= '" & cbCrateMarka.Text & "',CrateQty=" & Val(txtCrateQty.Text) & ",CrateAccountID=" & Val(cbAccountName.SelectedValue) & ",CrateAccountName='" & cbAccountName.Text & "',RoundOff='" & Val(lblRoundOff.Text) & "',cut=0,OnWeight='" & txtAddWeight.Text & "' where VoucherID =" & Val(txtid.Text) & ""
                clsFun.ExecNonQuery(sql)

                '   Me.Alert("Success Alert", msgAlert.enmType.Update)
                ServerTag = 1 : InsertLedger() : CrateLedger() : ServerLedger() : ServerCrate() : retrive()
                'MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                txtclear()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        ButtonControl()
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
            ClsFunserver.FastCrateLedger(FastQuery)
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub btnMultiUpdate_Click(sender As Object, e As EventArgs) Handles btnMultiUpdate.Click
        MultiUpdateSpeed()
    End Sub

    Private Sub txtSlipNo_TextChanged(sender As Object, e As EventArgs) Handles txtSlipNo.TextChanged

    End Sub

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub txtNug_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNug.KeyUp
        If btnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) > 0 Then
                txtKg.Text = Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) * Val(txtNug.Text)
            End If
        End If
    End Sub
    Private Sub SpeedCalculation2()
        If ckTaxPaid.Checked = True Then
            Dim TotTaxPer As Decimal = Format(Val(txtComPer.Text) + Val(txtMPer.Text) + Val(txtRdfPer.Text) + 100, "0.00")
            Dim incRate As Decimal = Val(txtNetRate.Text) * (100 / TotTaxPer)
            txtrate.Text = Format(Val(incRate), "0.00")
            If CbPer.SelectedIndex = 0 Then
                txtNet.Text = Format(Val(txtNug.Text) * Val(incRate), "0.00")
            ElseIf CbPer.SelectedIndex = 1 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate), "0.00")
            ElseIf CbPer.SelectedIndex = 2 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate) / 5, "0.00")
            ElseIf CbPer.SelectedIndex = 3 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate) / 10, "0.00")
            ElseIf CbPer.SelectedIndex = 4 Then
                txtNet.Text = Format(Val(incRate) / 20 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 5 Then
                txtNet.Text = Format(Val(incRate) / 40 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 6 Then
                txtNet.Text = Format(Val(incRate) / 41 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 7 Then
                txtNet.Text = Format(Val(incRate) / 50 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 8 Then
                txtNet.Text = Format(Val(incRate) / 51 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 9 Then
                txtNet.Text = Format(Val(incRate) / 51.7 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 10 Then
                txtNet.Text = Format(Val(incRate) / 52.2 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 11 Then
                txtNet.Text = Format(Val(incRate) / 52.3 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 12 Then
                txtNet.Text = Format(Val(incRate) / 52.5 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 13 Then
                txtNet.Text = Format(Val(incRate) / 53 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 14 Then
                txtNet.Text = Format(Val(incRate) / 80 * Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 15 Then
                txtNet.Text = Format(Val(txtKg.Text) * Val(incRate) / 100, "0.00")
            ElseIf CbPer.SelectedIndex = 16 Then
                txtNet.Text = Format(Val(incRate) * Val(txtNug.Text), "0.00")
            End If

        Else
            If CbPer.SelectedIndex = 0 Then
                txtrate.Text = Format(Val(txtNet.Text) / Val(txtNug.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 1 Then
                '                txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 2 Then
                '          txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text) / 5, "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 5 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 3 Then
                '      txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text) / 10, "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 10 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 4 Then
                '  txtNet.Text = Format(Val(txtrate.Text) / 20 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 20 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 5 Then
                ' txtNet.Text = Format(Val(txtrate.Text) / 40 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 40 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 6 Then
                '        txtNet.Text = Format(Val(txtrate.Text) / 41 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 41 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 7 Then
                'txtNet.Text = Format(Val(txtrate.Text) / 50 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 50 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 8 Then
                '  txtNet.Text = Format(Val(txtrate.Text) / 50 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 51 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 9 Then
                'txtNet.Text = Format(Val(txtrate.Text) / 51.7 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 51.7 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 10 Then
                'txtNet.Text = Format(Val(txtrate.Text) / 52.2 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 52.2 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 11 Then
                'txtNet.Text = Format(Val(txtrate.Text) / 52.3 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 52.3 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 12 Then
                'txtNet.Text = Format(Val(txtrate.Text) / 52.5 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 52.5 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 13 Then
                'txtNet.Text = Format(Val(txtrate.Text) / 53 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 53 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 14 Then
                'txtNet.Text = Format(Val(txtrate.Text) / 80 * Val(txtKg.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 80 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 15 Then
                'txtNet.Text = Format(Val(txtKg.Text) * Val(txtrate.Text) / 100, "0.00")
                txtrate.Text = Format(Val(txtNet.Text) * 100 / Val(txtKg.Text), "0.00")
            ElseIf CbPer.SelectedIndex = 16 Then
                '    txtNet.Text = Format(Val(txtrate.Text) * Val(txtCrateQty.Text), "0.00")
                txtrate.Text = Format(Val(txtNet.Text) / Val(txtCrateQty.Text), "0.00")
            End If

        End If
        If Octroi = "Yes" Then
            txtComAmt.Text = Format(Val(txtComPer.Text) * Val(Val(txtNet.Text) + Val(txtLaboutAmt.Text)) / 100, "0.00")
        Else
            txtComAmt.Text = Format(Val(txtComPer.Text) * Val(txtNet.Text) / 100, "0.00")
        End If
        If CalculationMethod = "Nug" Then
            txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtNet.Text) , "0.00")
        Else
            txtMAmt.Text = Format(Val(txtMPer.Text) * Val(txtNet.Text) / 100, "0.00")
        End If
        txtRdfAmt.Text = Format(Val(txtRdfPer.Text) * Val(txtNet.Text) / 100, "0.00")
        If ApplyCommWeight = "Yes" Then
            txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtKg.Text), "0.00")
        Else
            If crateRate = "Yes" And lblCrate.Text = "Y" Then
                txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtCrateQty.Text), "0.00")
            Else
                txtTareAmt.Text = Format(Val(txtTare.Text) * Val(txtNug.Text), "0.00")
            End If
        End If
        ' If Val(txtLabour.Text) > 0 Then txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
        txtLaboutAmt.Text = Format(Val(txtLabour.Text) * Val(txtNug.Text), "0.00")
        lblTotCharges.Text = Format(Val(txtComAmt.Text) + Val(txtMAmt.Text) + Val(txtRdfAmt.Text) + Val(txtTareAmt.Text) + Val(txtLaboutAmt.Text), "0.00")
        txtTotal.Text = Format(Val(lblTotCharges.Text) + Val(txtNet.Text), "0.00")
        If RoundOff = "No" Then
            Dim tmpCustAmount As Double = Val(txtTotal.Text)
            txtTotal.Text = Math.Round(Val(tmpCustAmount), 0)
            lblRoundOff.Text = Math.Round(Val(txtTotal.Text) - Val(tmpCustAmount), 2)
            txtTotal.Text = Format(Val(txtTotal.Text), "0.00")
        End If
    End Sub
    Private Sub txtNet_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNet.KeyUp
        SpeedCalculation2()
    End Sub

    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub


    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub

    Private Sub txtItem_TextChanged(sender As Object, e As EventArgs) Handles txtItem.TextChanged

    End Sub

    Private Sub txtNug_TextChanged(sender As Object, e As EventArgs) Handles txtNug.TextChanged

    End Sub

    Private Sub txtCrateQty_TextChanged(sender As Object, e As EventArgs) Handles txtCrateQty.TextChanged

    End Sub

    Private Sub txtGrossWt_KeyUp(sender As Object, e As KeyEventArgs) Handles txtGrossWt.KeyUp
        If Val(txtGrossWt.Text) = 0 Then Exit Sub
        txtKg.Text = Format(Val(txtGrossWt.Text) - (Val(CUT) * Val(txtNug.Text)), "0.00")
    End Sub

    Private Sub txtTotal_TextChanged(sender As Object, e As EventArgs) Handles txtTotal.TextChanged

    End Sub
End Class