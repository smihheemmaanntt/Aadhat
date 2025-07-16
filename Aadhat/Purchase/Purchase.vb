Imports System.IO

Public Class Purchase
    Dim vno As Integer : Dim VchId As Integer
    Dim sql As String = String.Empty : Dim count As Integer = 0 : Dim MaxID As String = String.Empty
    Dim CalcType As String = String.Empty : Dim remark As String = String.Empty
    Dim remarkHindi As String = String.Empty : Dim TotalPages As Integer = 0 : Dim PageNumber As Integer = 0
    Dim RowCount As Integer = 1 : Dim Offset As Integer = 0 : Dim ServerTag As Integer
    Dim whatsappSender As New WhatsAppSender()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        clsFun.DoubleBuffered(Dg2, True)
        clsFun.DoubleBuffered(dg3, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub
    Private Sub Purchase_DoubleClick(sender As Object, e As EventArgs) Handles Me.DoubleClick
        dg1.ClearSelection()
        Dg2.ClearSelection()
    End Sub
    Private Sub txtPurchaseNug_GotFocus(sender As Object, e As EventArgs) Handles txtNug.GotFocus
        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID='" & txtItemID.Text & "'")

        If lblCrate.Text = "Y" Then
            If txtAccountID.Text = 7 Then
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
            txtCrateQty.Clear()
            cbCrateMarka.SelectedIndex = -1
            pnlMarka.Visible = False
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
    'Private Sub itemfill()
    '    If dg1.SelectedRows.Count = 0 Then
    '        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID= '" & txtItemID.Text & "'")
    '        txtWeight.Text = Format(Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")), "0.00")
    '    Else
    '        lblCrate.Text = clsFun.ExecScalarStr(" Select MaintainCrate FROM Items WHERE ID= '" & txtItemID.Text & "'")
    '    End If
    'End Sub

    Private Sub ItemFill()
        Dim itemID As Integer = Val(txtItemID.Text)
        If itemID = 0 Then Exit Sub ' Prevent unnecessary queries if ItemID is invalid
        ' Use a single query to fetch all required fields
        Dim query As String = "SELECT CommisionPer, UserChargesPer, Tare, Labour, RDFPer, MaintainCrate, WeightPerNug,TrackStock,RateAs FROM Items WHERE ID = " & itemID
        Dim dt As DataTable = clsFun.ExecDataTable(query) ' Assume ExecDataTable returns a DataTable
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)
            lblCrate.Text = row("MaintainCrate").ToString()
            CbPer.Text = row("RateAs").ToString()
            trackStock = row("TrackStock").ToString()
        End If
    End Sub

    Private Sub txtNug_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNug.KeyUp, txtWeight.KeyUp, txtRate.KeyUp, txtbasicTotal.KeyUp, cbPer.KeyUp
        If BtnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) > 0 Then
                txtWeight.Text = Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) * Val(txtNug.Text)
            End If
        End If
    End Sub
    Private Sub Purchase_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F2 Then
            If BtnSave.Text <> "&Save" Then
                If txtAccount.Enabled = True Then txtAccount.Enabled = False : txtStorage.Enabled = False Else txtAccount.Enabled = True : txtStorage.Enabled = True
            End If
        End If
        If e.KeyCode = Keys.Escape Then
            If PnlCustomerBill.Visible = True Then
                PnlCustomerBill.Visible = False
                Exit Sub
            End If
            If DgAccountSearch.Visible = True Then
                DgAccountSearch.Visible = False
                Exit Sub
            ElseIf dgItemPurchase.Visible = True Then
                dgItemPurchase.Visible = False
                Exit Sub
            ElseIf dgStore.Visible = True Then
                dgStore.Visible = False
                Exit Sub
            ElseIf dgCharges.Visible = True Then
                dgCharges.Visible = False
                Exit Sub
            Else
                Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "AADHAT")
                If msgRslt = MsgBoxResult.Yes Then
                    Me.Close()
                ElseIf msgRslt = MsgBoxResult.No Then
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

    Private Sub Purchase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskEntryDate.Focus() : VNumber()
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
        clsFun.FillDropDownList(cbAccountName, "Select ID,AccountName FROM Accounts  where GroupID in(16,17,32,33) order by AccountName ", "AccountName", "ID", "--N./A.--")
        Me.KeyPreview = True : CbpurchaseType.SelectedIndex = 0
        cbPer.SelectedIndex = 0 : fillPurchase()
        pnlMarka.Visible = False : BtnDelete.Enabled = False
        dg2RownColums() : rowColumsPurchase()
    End Sub
    Public Sub fillPurchase()
        cbPer.Text = clsFun.ExecScalarStr("Select per From Controls")
        If clsFun.ExecScalarStr("Select AskFarmer From Controls") = "No" Then
            cbFarmer.Visible = False
            lblFarmer.Visible = False
        Else
            cbFarmer.Visible = True
            lblFarmer.Visible = True
        End If
    End Sub

    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 26
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "Date" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "AccountName" : .Columns(3).Width = 159
            .Columns(4).Name = "ShopNo" : .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo" : .Columns(5).Width = 59
            .Columns(6).Name = "itemName" : .Columns(6).Width = 69
            .Columns(7).Name = "LotNo" : .Columns(7).Width = 76
            .Columns(8).Name = "Nug" : .Columns(8).Width = 90
            .Columns(9).Name = "Weight" : .Columns(9).Width = 86
            .Columns(10).Name = "Rate" : .Columns(10).Width = 90
            .Columns(11).Name = "Per" : .Columns(11).Width = 50
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
        End With
    End Sub
    Private Sub PrintOnly()
        TempRowColumn()
        Dim i, j As Integer
        'Dim dt As New DataTable
        'Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        tmpgrid.Rows.Clear()
        If dg1.Rows.Count = 0 Then Exit Sub
        If dg1.Rows.Count > 0 Then
            For i = 0 To dg1.Rows.Count - 1
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = mskEntryDate.Text
                    .Cells(2).Value = .Cells(2).Value & txtVoucherNo.Text
                    .Cells(3).Value = txtAccount.Text
                    If cbType.InvokeRequired Then
                        cbType.Invoke(Sub() selectedIndex = cbType.SelectedIndex)
                    Else
                        selectedIndex = cbType.SelectedIndex
                    End If
                    .Cells(4).Value = selectedIndex
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
                    .Cells(21).Value = .Cells(21).Value & txtTotalNetAmount.Text
                    .Cells(22).Value = .Cells(22).Value & txttotalCharges.Text
                    .Cells(23).Value = clsFun.ExecScalarStr("Select OtherName From Items Where Id='" & dg1.Rows(i).Cells(7).Value & "'")
                    .Cells(24).Value = clsFun.ExecScalarStr("Select OtherName From Accounts Where Id='" & txtAccountID.Text & "'")
                    If Dg2.Rows.Count > 0 Then
                        For j = 0 To Dg2.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & Dg2.Rows(j).Cells(0).Value & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Dg2.Rows(j).Cells(1).Value & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & Dg2.Rows(j).Cells(2).Value & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & Dg2.Rows(j).Cells(3).Value & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & Dg2.Rows(j).Cells(4).Value & vbCrLf
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
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 300
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 10
        DgAccountSearch.BringToFront() : DgAccountSearch.Visible = True
        retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        If ckShowCustomer.Checked = True Then
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,17,16)  or UnderGroupID in (11,17,16))" & condtion & " order by AccountName Limit 10")
        Else
            dt = clsFun.ExecDataTable("Select * from Account_AcGrp where (Groupid in(11,17)  or UnderGroupID in (11,17))" & condtion & " order by AccountName limit 10")
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
    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If dgItemPurchase.ColumnCount = 0 Then PurchaseItemRowColums()
        If dgItemPurchase.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        txtStorage.Focus()
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
            txtAccount.Clear()
            txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            '    CustomerFill()
            DgAccountSearch.Visible = False
            txtStorage.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub

    Private Sub CrateLedger()
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells(10).Value = "Y" Then ''Party Account
                    If txtAccountID.Text = 7 Then
                        If Val(cbAccountName.SelectedValue) > 0 Then
                            clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate In'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), CbpurchaseType.Text, cbAccountName.SelectedValue, cbAccountName.Text, "Crate In", .Cells(11).Value, .Cells(12).Value, .Cells(13).Value, "", "", "", "")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & CbpurchaseType.Text & "'," & Val(cbAccountName.SelectedValue) & ",'" & cbAccountName.Text & "','Crate In'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '" & txtVehicleNo.Text & "','','',''"
                        End If
                    Else
                        '                        clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate In'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), CbpurchaseType.Text, txtAccountID.Text, txtAccount.Text, "Crate In", .Cells(11).Value, .Cells(12).Value, .Cells(13).Value, "", "", "", "")
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & CbpurchaseType.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate In'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '" & txtVehicleNo.Text & "','','',''"
                    End If
                    ' clsFun.CrateLedger(0, VchId, txtVoucherNo.Text, SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, "Crate In", .Cells(11).Value, .Cells(12).Value, .Cells(13).Value, "")
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastCrateLedger(FastQuery)
    End Sub

    Private Sub ServerCrateLedger()
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                If .Cells(10).Value = "Y" Then ''Party Account
                    If txtAccountID.Text = 7 Then
                        If Val(cbAccountName.SelectedValue) > 0 Then
                            clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate In'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), CbpurchaseType.Text, cbAccountName.SelectedValue, cbAccountName.Text, "Crate In", .Cells(11).Value, .Cells(12).Value, .Cells(13).Value, "", "", "", "")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & CbpurchaseType.Text & "'," & Val(cbAccountName.SelectedValue) & ",'" & cbAccountName.Text & "','Crate In'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '" & txtVehicleNo.Text & "','','',''," & Val(ServerTag) & "," & Val(OrgID) & ""
                        End If
                    Else
                        '                        clsFun.CrateLedger(0, Val(txtid.Text), clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='Crate In'") + 1, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), CbpurchaseType.Text, txtAccountID.Text, txtAccount.Text, "Crate In", .Cells(11).Value, .Cells(12).Value, .Cells(13).Value, "", "", "", "")
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & "," & Val(clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & .Cells(14).Value & "'") + 1) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & CbpurchaseType.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate In'," & Val(.Cells(11).Value) & ",'" & .Cells(12).Value & "','" & .Cells(13).Value & "', '" & txtVehicleNo.Text & "','','',''," & Val(ServerTag) & "," & Val(OrgID) & ""
                    End If
                    ' clsFun.CrateLedger(0, VchId, txtVoucherNo.Text, SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, "Crate In", .Cells(11).Value, .Cells(12).Value, .Cells(13).Value, "")
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(FastQuery)
    End Sub

    Private Sub dg2Record()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            '  Application.DoEvents()
            With row
                If .Cells("Charge Name").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtid.Text) & "," &
                        "'" & .Cells("ChargeID").Value & "','" & .Cells("Charge Name").Value & "','" & .Cells("On").Value & "'," &
                        "'" & .Cells("Cal").Value & "','" & .Cells("+/-").Value & "','" & .Cells("Amount").Value & "'"
                End If
            End With
        Next
        Try
            sql = "insert into ChargesTrans(VoucherID, ChargesID, ChargeName, OnValue, Calculate, ChargeType, Amount) " & FastQuery & ""
            If FastQuery = String.Empty Then Exit Sub
            clsFun.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub ChargesLedger()
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ChargeName='" & .Cells("Charge Name").Value & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            '                            clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "C")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        Else
                            '                           clsFun.Ledger(0, VchId, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "D")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ChargeName='" & .Cells("Charge Name").Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "D")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D','" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        Else
                            ' clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "C")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C','" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
        clsFun.CloseConnection()
    End Sub

    Private Sub ServerChargesLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In Dg2.Rows
            With row
                If .Cells("Charge Name").Value <> "" Then
                    Dim AcID As Integer = clsFun.ExecScalarInt("Select AccountID from Charges where ID=" & .Cells(5).Value & "")
                    ssql = clsFun.ExecScalarStr("Select AccountName from Charges where ID=" & .Cells(5).Value & "")
                    Dim AccName As String = ssql
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ChargeName='" & .Cells("Charge Name").Value & "'") = "Our Cost" Then
                        If .Cells(3).Value = "+" Then
                            '                            clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "C")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        Else
                            '                           clsFun.Ledger(0, VchId, CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "D")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ChargeName='" & .Cells("Charge Name").Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            ' clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "D")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        Else
                            ' clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, AcID, AccName, .Cells(4).Value, "C")
                            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(AcID) & ", '" & AccName & "','" & Val(.Cells(4).Value) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & txtVoucherNo.Text & "','" & AccName & "','" & txtVoucherNo.Text & "'"
                        End If
                    End If
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
        clsFun.CloseConnection()
    End Sub

    Private Sub SellerLedger()
        Dim FastQuery As String = String.Empty
        Dim tmpamount As Decimal = Val(txtbasicTotal.Text)
        Dim tmpamount2 As Decimal = Val(txtbasicTotal.Text)
        ''Caluclate  net amt
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells("ChargeID").Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells("ChargeID").Value & "'") = "Our Cost" Then ''our coast
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
        If CbpurchaseType.SelectedIndex = 1 Then
            If txtAccountID.Text > 0 Then ''Party Account
                ' If Math.Abs(Val(txtTotalNetAmount.Text)) Then
                If txtTotalNetAmount.Text > 0 Then
                    '       clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Val(tmpamount) + Val(txtroundoff.Text), "C", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Val(tmpamount) + Val(txtroundoff.Text) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                ElseIf txtTotalNetAmount.Text < 0 Then
                    '                    clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                End If
                If Val(txtbasicTotal.Text) > 0 Then ''Maal Khata Account
                    'clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 28, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=28"), Val(tmpamount2), "D", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(28) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=28") & "','" & Math.Abs(Val(tmpamount2)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                End If
            End If
        Else
            If Math.Abs(Val(txttotalCharges.Text)) > 0 Then
                If txttotalCharges.Text > 0 Then
                    ' clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Val(tmpamount) + Val(txtroundoff.Text), "C", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

                Else
                    'clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                End If
            End If
        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) < 0 Then
                '  clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtroundoff.Text)), "C", "Voucher No.:" & txtVoucherNo.Text & ", " & txtAccount.Text)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(42) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'C', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
            Else
                '    clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Val(txtroundoff.Text), "D", "Voucher No.:" & txtVoucherNo.Text & ", " & txtAccount.Text)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(42) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'D', '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

            End If
        End If
        If FastQuery = "" Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub
    Private Sub ServerSellerLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim tmpamount As Decimal = Val(txtbasicTotal.Text)
        Dim tmpamount2 As Decimal = Val(txtbasicTotal.Text)
        ''Caluclate  net amt
        For Each row As DataGridViewRow In Dg2.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                ' Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                If .Cells("Charge Name").Value <> "" Then
                    If clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells("ChargeID").Value & "'") = "Party Cost" Then
                        If .Cells(3).Value = "+" Then
                            tmpamount = tmpamount + Val(.Cells(4).Value)
                        Else
                            tmpamount = tmpamount - Val(.Cells(4).Value)
                        End If
                    ElseIf clsFun.ExecScalarStr("Select COSTON from Charges where ID='" & .Cells("ChargeID").Value & "'") = "Our Cost" Then ''our coast
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
        If CbpurchaseType.SelectedIndex = 1 Then
            If txtAccountID.Text > 0 Then ''Party Account
                ' If Math.Abs(Val(txtTotalNetAmount.Text)) Then
                If txtTotalNetAmount.Text > 0 Then
                    '       clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Val(tmpamount) + Val(txtroundoff.Text), "C", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Val(tmpamount) + Val(txtroundoff.Text) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                ElseIf txtTotalNetAmount.Text < 0 Then
                    '                    clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                End If
                If Val(txtbasicTotal.Text) > 0 Then ''Maal Khata Account
                    'clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 28, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=28"), Val(tmpamount2), "D", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(28) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=28") & "','" & Math.Abs(Val(tmpamount2)) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                End If
            End If
        Else
            If Math.Abs(Val(txttotalCharges.Text)) > 0 Then
                If txttotalCharges.Text > 0 Then
                    ' clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Val(tmpamount) + Val(txtroundoff.Text), "C", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

                Else
                    'clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)), "D", remark, txtAccount.Text, remarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','" & Math.Abs(Val(tmpamount) + Val(txtroundoff.Text)) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
                End If
            End If
        End If
        If Val(txtroundoff.Text) <> 0 Then ''Account 
            If Val(txtroundoff.Text) < 0 Then
                '  clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Math.Abs(Val(txtroundoff.Text)), "C", "Voucher No.:" & txtVoucherNo.Text & ", " & txtAccount.Text)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(42) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'C'," & Val(ServerTag) & "," & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "
            Else
                '    clsFun.Ledger(0, Val(txtid.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 42, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42"), Val(txtroundoff.Text), "D", "Voucher No.:" & txtVoucherNo.Text & ", " & txtAccount.Text)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(42) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=42") & "','" & Math.Abs(Val(txtroundoff.Text)) & "', 'D'," & Val(ServerTag) & "," & Val(OrgID) & ", '" & "Voucher No.:" & txtVoucherNo.Text & IIf(txtVehicleNo.Text <> "", ",Vehicle No.: " & txtVehicleNo.Text, ",") & remark & "','" & txtAccount.Text & "',' " & remarkHindi & "' "

            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub
    Private Sub delete()
        Dim DeletePurchase As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Purchase'")
        If DeletePurchase <> "Y" Then MsgBox("You Don't Have Rights to Delete Purchase Bills... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : AlltextClear() : Exit Sub
        If clsFun.ExecScalarInt("Select count(*) from Transaction2 where PurchaseID='" & Val(txtid.Text) & "'") > 0 Then
            MsgBox("Transactions Already Used in Stock Sale.. Delete First Stock Sale Records", vbOKOnly, "Access Denied")
            Exit Sub
        End If
        If MessageBox.Show(" Are you Sure Want to Delete Purchase Voucher ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "; " &
                                   "DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "; " &
                                   "DELETE from Purchase WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                   "DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "") > 0 Then
                ClsFunserver.ExecNonQuery("Delete From  Ledger  Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                   "Delete From  CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 0 : ServerSellerLedger() : ServerChargesLedger() : ServerCrateLedger()
            End If
            MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Saved")
            AlltextClear() : PurchaseClear()
        End If
    End Sub

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
                    End If
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub SaveDgPurchase()
        Dim sql As String = String.Empty
        Dim cmd As SQLite.SQLiteCommand
        sql = "Insert Into Vouchers(Transtype, EntryDate,BillNo,SallerID, SallerName,VehicleNo,Nug,Kg,BasicAmount,DiscountAmount,TotalAmount,PurchaseType, " &
              "StorageID,StorageName,InvoiceID,AccountName,TotalCharges) Values (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@3", txtVoucherNo.Text)
            cmd.Parameters.AddWithValue("@4", Val(txtAccountID.Text))
            cmd.Parameters.AddWithValue("@5", txtAccount.Text)
            cmd.Parameters.AddWithValue("@6", txtVehicleNo.Text)
            cmd.Parameters.AddWithValue("@7", txtTotalNug.Text)
            cmd.Parameters.AddWithValue("@8", txttotalWeight.Text)
            cmd.Parameters.AddWithValue("@9", txtbasicTotal.Text)
            cmd.Parameters.AddWithValue("@10", Val(txttotalCharges.Text))
            cmd.Parameters.AddWithValue("@11", Val(txtTotalNetAmount.Text))
            cmd.Parameters.AddWithValue("@12", CbpurchaseType.Text)
            cmd.Parameters.AddWithValue("@13", Val(txtStorageID.Text))
            cmd.Parameters.AddWithValue("@14", txtStorage.Text)
            cmd.Parameters.AddWithValue("@15", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@16", cbFarmer.Text)
            cmd.Parameters.AddWithValue("@17", Val(txttotalCharges.Text))
            If cmd.ExecuteNonQuery() > 0 Then
                clsFun.CloseConnection()
            End If
            txtid.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
            DgPurchaseRecord() : dg2Record() : ServerTag = 1
            Dim SellOutCharges As String = clsFun.ExecScalarStr("Select ChargeEffect From Controls")
            If SellOutCharges <> "Yes" Or CbpurchaseType.Text = "Purchase" Then
                SellerLedger() : ChargesLedger() : CrateLedger()
                ServerSellerLedger() : ServerChargesLedger() : ServerCrateLedger()
            Else
                CrateLedger() : ServerCrateLedger()
            End If
            MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '  RemoveDuplicateInvoice()
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
                remark = remark & .Cells("Item Name").Value & " Lot No. : " & .Cells("Lot No").Value & ", Nug. : " & Val(.Cells("Nug").Value) & ",Weight : " & Format(Val(.Cells("Weight").Value), "0.00") & ",Rate : " & Format(Val(.Cells("Rate").Value), "0.00") & "/- " & .Cells("Per").Value & " =" & Format(Val(.Cells("Amount").Value), "0.00") & "" & vbCrLf
                Dim othername As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID='" & Val(.Cells("ItemID").Value) & "' ")
                remarkHindi = remarkHindi & othername & " Lot No. : " & .Cells("Lot No").Value & ", नग : " & Val(.Cells("Nug").Value) & ",वजन : " & Format(Val(.Cells("Weight").Value), "0.00") & ",भाव : " & Format(Val(.Cells("Rate").Value), "0.00") & "/- " & .Cells("Per").Value & "=" & Format(Val(.Cells("Amount").Value), "0.00") & "" & vbCrLf
            End With
        Next
    End Sub

    Private Sub DgPurchaseRecord()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            ' Application.DoEvents()
            With row
                If .Cells("Item Name").Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtid.Text) & ", " &
                             "'" & txtVoucherNo.Text & "','" & txtVehicleNo.Text & "','" & CbpurchaseType.Text & "'," &
                             "" & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtStorageID.Text) & ",'" & txtStorage.Text & "'," &
                             "" & Val(.Cells("ItemID").Value) & ",'" & .Cells("Item Name").Value & "','" & .Cells("Lot No").Value & "', " &
                             " " & Val(.Cells("Nug").Value) & "," & Val(.Cells("Weight").Value) & "," & Val(.Cells("Rate").Value) & ",'" & .Cells("Per").Value & "'," &
                            " " & Val(.Cells("Amount").Value) & ",'" & .Cells("CrateY/N").Value & "'," & Val(.Cells("CrateID").Value) & ",'" & .Cells("CrateName").Value & "', " &
                            " " & Val(.Cells("CrateQty").Value) & "," & IIf(CbpurchaseType.SelectedIndex = 0, Val(txtAccountID.Text), (Val(28))) & ", " &
                            "'" & IIf(CbpurchaseType.SelectedIndex = 0, txtAccount.Text, (clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=28"))) & "'"
                End If
            End With
        Next
        Try
            sql = "insert into Purchase(EntryDate,TransType,VoucherID,BillNo,VehicleNo,PurchaseTypeName,AccountID,AccountName,StorageID,StorageName, " _
                         & "ItemID,ItemName,LotNo, Nug, Weight,Rate,Per, Amount, MaintainCrate, CrateID, CrateName, CrateQty,StockHolderID,StockHolderName) " & FastQuery & ""
            If FastQuery = String.Empty Then Exit Sub
            clsFun.ExecNonQuery(sql)
            ' MsgBox("Successfully Inserted")
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try

        clsFun.CloseConnection()
    End Sub

    Private Sub txtPurchaseTotAmount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAmount.KeyDown
        If txtItem.Text = "" Or Val(txtItemID.Text) < 0 Then txtItem.Focus() : Exit Sub
        If e.KeyCode = Keys.Enter Then
            If Val(txtAccountID.Text) = 0 Then MsgBox("Please Select Proper Account", MsgBoxStyle.Critical, "Select Account") : txtAccount.Focus() : Exit Sub
            If Val(txtStorageID.Text) = 0 Then MsgBox("Please Select Proper Store", MsgBoxStyle.Critical, "Select Store") : txtStorage.Focus() : Exit Sub
            If Val(txtItemID.Text) = 0 Then MsgBox("Please Select Proper Item", MsgBoxStyle.Critical, "Select item") : txtItem.Focus() : Exit Sub
            If TxtPurchaseLot.Text = "" Then MsgBox("Lot Can't Be Empty." & vbNewLine & "Please fill Lot...", MsgBoxStyle.Critical, "Check Lot") : TxtPurchaseLot.Focus() : Exit Sub
            If dg1.SelectedRows.Count = 1 Then
                Dim SoldLot As String = clsFun.ExecScalarStr("Select Lot From Transaction2 Where PurchaseID=" & Val(txtid.Text) & " and Lot='" & dg1.SelectedRows(0).Cells(1).Value & "'")
                'If SoldLot <> "" Then
                '    If TxtPurchaseLot.Text <> SoldLot Then
                '        MsgBox("Entry Can't Modify..." & vbNewLine & "Lot No. : " & dg1.SelectedRows(0).Cells(1).Value & " Already in Sale Transaction..." & vbNewLine & "You Entered  Lot No. : " & TxtPurchaseLot.Text & "", MsgBoxStyle.Critical, "Access Denied..") : TxtPurchaseLot.Focus() : Exit Sub
                '    End If
                'End If

                dg1.SelectedRows(0).Cells(0).Value = txtItem.Text
                dg1.SelectedRows(0).Cells(1).Value = TxtPurchaseLot.Text
                dg1.SelectedRows(0).Cells(2).Value = Format(Val(txtNug.Text), "0.00")
                dg1.SelectedRows(0).Cells(3).Value = Format(Val(txtWeight.Text), "0.00")
                dg1.SelectedRows(0).Cells(4).Value = Format(Val(txtRate.Text), "0.00")
                dg1.SelectedRows(0).Cells(5).Value = cbPer.Text
                dg1.SelectedRows(0).Cells(6).Value = Format(Val(txtAmount.Text), "0.00")
                dg1.SelectedRows(0).Cells(7).Value = txtItemID.Text
                dg1.SelectedRows(0).Cells(8).Value = txtStorageID.Text
                dg1.SelectedRows(0).Cells(9).Value = txtStorage.Text
                dg1.SelectedRows(0).Cells(10).Value = lblCrate.Text
                dg1.SelectedRows(0).Cells(11).Value = Val(cbCrateMarka.SelectedValue)
                dg1.SelectedRows(0).Cells(12).Value = cbCrateMarka.Text
                dg1.SelectedRows(0).Cells(13).Value = txtCrateQty.Text
                dg1.SelectedRows(0).Cells(14).Value = IIf(CbpurchaseType.SelectedIndex = 0, Val(txtAccountID.Text), (Val(28)))
                dg1.SelectedRows(0).Cells(15).Value = IIf(CbpurchaseType.SelectedIndex = 0, txtAccount.Text, ("Mall Khata Purchase A/c"))
                txtItem.Focus() : dg1.ClearSelection()
                ' cleartxt()
                calc() : PurchaseClear()
            Else
                For i = 0 To dg1.Rows.Count - 1
                    If dg1.Rows(i).Cells(0).Value = txtItem.Text And dg1.Rows(i).Cells(1).Value = TxtPurchaseLot.Text Then
                        MsgBox("Lot Already Exist...You Entered : " & TxtPurchaseLot.Text & " " & vbNewLine & "Same Lot No. Already Exists For this Item In this Bill...", MsgBoxStyle.Critical, "Lot Exists...") : TxtPurchaseLot.Focus() : Exit Sub
                    End If
                Next
                dg1.Rows.Add(txtItem.Text, TxtPurchaseLot.Text, Format(Val(txtNug.Text), "0.00"), Format(Val(txtWeight.Text), "0.00"),
                                   Format(Val(txtRate.Text), "0.00"), cbPer.Text, Format(Val(txtAmount.Text), "0.00"),
                                    txtItemID.Text, txtStorageID.Text, txtStorage.Text, lblCrate.Text,
                                    cbCrateMarka.SelectedValue, cbCrateMarka.Text, txtCrateQty.Text,
                                    IIf(CbpurchaseType.SelectedIndex = 0, Val(txtAccountID.Text), (Val(28))),
                                    IIf(CbpurchaseType.SelectedIndex = 0, txtAccount.Text, ("Mall Khata Purchase A/c")))

                'cleartxt()
                txtItem.Focus() : dg1.ClearSelection()
                'dg1.ClearSelection()
                calc() : PurchaseClear()
            End If
        End If

    End Sub
    'Private Sub UpdateStockIn()
    '    Dim dt As DateTime
    '    dt = CDate(Me.mskEntryDate.Text)
    '    SqliteEntryDate = dt.ToString("yyyy-MM-dd")
    '    Dim sql As String = String.Empty
    '    'If Val(txtNug.Text) = 0 And Val(txtKg.Text) = 0 Then
    '    '    MsgBox("please fill Nug or Kg")
    '    '    txtNug.Focus()
    '    '    Exit Sub
    '    'End If
    '    Dim cmd As SQLite.SQLiteCommand
    '    sql = "Update Vouchers Set TransType='" & Me.Text & "', EntryDate='" & SqliteEntryDate & "',BillNo='" & txtVoucherNo.Text & "'," _
    '        & "SallerID='" & Val(txtAccountID.Text) & "', SallerName='" & txtAccount.Text & "',VehicleNo='" & txtVehicleNo.Text & "'," _
    '        & "Nug='" & Val(txtTotalNug.Text) & "',Kg='" & Val(txttotalWeight.Text) & "',BasicAmount='" & Val(txtbasicTotal.Text) & "'," _
    '        & " DiscountAmount= '" & Val(txttotalCharges.Text) & "',TotalAmount='" & Val(txtTotalNetAmount.Text) & "'," _
    '        & "PurchaseType='" & CbpurchaseType.Text & "',StorageID='" & Val(txtStorageID.Text) & "',StorageName='" & txtStorage.Text & "' Where ID=" & Val(txtid.Text) & ""
    '    Try
    '        If clsFun.ExecNonQuery(sql) > 0 Then
    '            clsFun.CloseConnection()
    '        End If
    '        If clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & txtid.Text & "") > 0 Then
    '        End If
    '        If clsFun.ExecNonQuery("DELETE from Purchase WHERE VoucherID=" & txtid.Text & "") > 0 Then
    '        End If
    '        If clsFun.ExecNonQuery("DELETE from ChargesTrans WHERE VoucherID=" & txtid.Text & "") > 0 Then
    '        End If
    '        If clsFun.ExecNonQuery("DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & txtid.Text & "") > 0 Then
    '        End If
    '        DgPurchaseRecord() : dg2Record()
    '        insertLedger()
    '        InsertCharges()
    '        UpdateCrateLedger()
    '        MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
    '        AlltextClear()
    '        'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '        clsFun.CloseConnection()
    '    End Try
    'End Sub
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
    Private Sub UpdatePurchase()
        Dim sql As String = String.Empty
        'Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "', EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "',BillNo='" & txtVoucherNo.Text & "'," _
            & "SallerID='" & Val(txtAccountID.Text) & "', SallerName='" & txtAccount.Text & "',VehicleNo='" & txtVehicleNo.Text & "'," _
            & "Nug='" & Val(txtTotalNug.Text) & "',Kg='" & Val(txttotalWeight.Text) & "',BasicAmount='" & Val(txtbasicTotal.Text) & "'," _
            & " DiscountAmount= '" & Val(txttotalCharges.Text) & "',TotalAmount='" & Val(txtTotalNetAmount.Text) & "'," _
            & "PurchaseType='" & CbpurchaseType.Text & "',StorageID='" & Val(txtStorageID.Text) & "',StorageName='" & txtStorage.Text & "'," _
            & "AccountName='" & cbFarmer.Text & "',TotalCharges='" & Val(txttotalCharges.Text) & "', InvoiceID='" & Val(txtInvoiceID.Text) & "' Where ID=" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 1 : UpdateCrate()
                clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "; " &
                                " DELETE from Purchase WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                " DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & "; " &
                                " DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "")

                DgPurchaseRecord() : dg2Record() : ServerTag = 1
                clsFun.ExecNonQuery("Update Transaction2 SET SallerID=" & IIf(CbpurchaseType.SelectedIndex = 0, Val(txtAccountID.Text), (Val(28))) & ", " & _
                             "SallerName= '" & IIf(CbpurchaseType.SelectedIndex = 0, txtAccount.Text, (clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=28"))) & "', " & _
                             "StorageID=" & Val(txtStorageID.Text) & ",StorageName='" & txtStorage.Text & "' Where PurchaseID='" & Val(txtid.Text) & "' ")
                clsFun.ExecNonQuery("Update Vouchers SET SallerID=" & IIf(CbpurchaseType.SelectedIndex = 0, Val(txtAccountID.Text), (Val(28))) & ", StorageID=" & Val(txtStorageID.Text) & ",StorageName='" & txtStorage.Text & "', " & _
                              "SallerName= '" & IIf(CbpurchaseType.SelectedIndex = 0, txtAccount.Text, (clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=28"))) & "' Where ID= " & (clsFun.ExecScalarInt("Select VoucherID From Transaction2 Where PurchaseID='" & Val(txtid.Text) & "'")) & " ")
                Dim SellOutCharges As String = clsFun.ExecScalarStr("Select ChargeEffect From Controls")
                If SellOutCharges <> "Yes" Or CbpurchaseType.Text = "Purchase" Then
                    SellerLedger() : ChargesLedger() : CrateLedger()
                    ServerSellerLedger() : ServerChargesLedger() : ServerCrateLedger()
                Else
                    CrateLedger() : ServerCrateLedger()
                End If
            End If
            MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Public Sub MultiUpdatePurchase()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        'If Val(txtNug.Text) = 0 And Val(txtKg.Text) = 0 Then
        '    MsgBox("please fill Nug or Kg")
        '    txtNug.Focus()
        '    Exit Sub
        'End If
        'Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers Set TransType='" & Me.Text & "', EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "',BillNo='" & txtVoucherNo.Text & "'," _
            & "SallerID='" & Val(txtAccountID.Text) & "', SallerName='" & txtAccount.Text & "',VehicleNo='" & txtVehicleNo.Text & "'," _
            & "Nug='" & Val(txtTotalNug.Text) & "',Kg='" & Val(txttotalWeight.Text) & "',BasicAmount='" & Val(txtbasicTotal.Text) & "'," _
            & " DiscountAmount= '" & Val(txttotalCharges.Text) & "',TotalAmount='" & Val(txtTotalNetAmount.Text) & "'," _
            & "PurchaseType='" & CbpurchaseType.Text & "',StorageID='" & Val(txtStorageID.Text) & "',StorageName='" & txtStorage.Text & "'," _
            & "AccountName='" & cbFarmer.Text & "',TotalCharges='" & Val(txttotalCharges.Text) & "', InvoiceID='" & Val(txtInvoiceID.Text) & "' Where ID=" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecScalarStr("Select Count(VoucherID) From CrateVoucher Where VoucherID='" & Val(txtid.Text) & "'") > 0 Then
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher  Where  VoucherID=" & Val(txtid.Text) & " and OrgID=" & Val(OrgID) & ";")
                ServerTag = 1 : UpdateCrate()
            End If
            clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & "; " &
                            " DELETE from Purchase WHERE VoucherID=" & Val(txtid.Text) & "; " &
                            " DELETE from ChargesTrans WHERE VoucherID=" & Val(txtid.Text) & "; " &
                            " DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtid.Text) & "")

            DgPurchaseRecord() : dg2Record() : ServerTag = 1
            Dim SellOutCharges As String = clsFun.ExecScalarStr("Select ChargeEffect From Controls")
            If SellOutCharges <> "Yes" Or CbpurchaseType.Text = "Purchase" Then
                SellerLedger() : ChargesLedger() : CrateLedger()
                ServerSellerLedger() : ServerChargesLedger() : ServerCrateLedger()
            Else
                CrateLedger() : ServerCrateLedger()
            End If
            AlltextClear()
            '   MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated") 'Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Private Sub dgPurchase_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
        TxtPurchaseLot.Text = dg1.SelectedRows(0).Cells(1).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
        txtWeight.Text = dg1.SelectedRows(0).Cells(3).Value
        txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
        cbPer.Text = dg1.SelectedRows(0).Cells(5).Value
        'txtModeID.Text = dgPurchase.SelectedRows(0).Cells(6).Value
        cbPer.Text = dg1.SelectedRows(0).Cells(5).Value
        txtAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
        txtStorage.Text = dg1.SelectedRows(0).Cells(9).Value
        lblCrate.Text = dg1.SelectedRows(0).Cells(10).Value
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(12).Value
        txtCrateQty.Text = dg1.SelectedRows(0).Cells(13).Value
        txtItem.Focus()
        'txtAccountID.Text = dgPurchase.SelectedRows(0).Cells(14).Value
        'txtAccount.Text = dgPurchase.SelectedRows(0).Cells(15).Value
    End Sub
    Private Sub dgPurchase_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
            TxtPurchaseLot.Text = dg1.SelectedRows(0).Cells(1).Value
            txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
            txtWeight.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
            cbPer.Text = dg1.SelectedRows(0).Cells(5).Value
            txtAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtItemID.Text = dg1.SelectedRows(0).Cells(7).Value
            cbPer.Text = dg1.SelectedRows(0).Cells(5).Value
            txtAmount.Text = dg1.SelectedRows(0).Cells(6).Value
            txtStorage.Text = dg1.SelectedRows(0).Cells(9).Value
            lblCrate.Text = dg1.SelectedRows(0).Cells(10).Value
            cbCrateMarka.Text = dg1.SelectedRows(0).Cells(12).Value
            txtCrateQty.Text = dg1.SelectedRows(0).Cells(13).Value
            '  txtAccountID.Text = dgPurchase.SelectedRows(0).Cells(14).Value
            'txtAccount.Text = dgPurchase.SelectedRows(0).Cells(15).Value
            txtItemID.Text = dg1.SelectedRows(0).Cells(16).Value
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

        If dg1.SelectedRows.Count = 0 Then Exit Sub
        If e.KeyCode = Keys.Delete Then
            Dim SoldLot As String = clsFun.ExecScalarStr("Select Lot From Transaction2 Where PurchaseID=" & Val(txtid.Text) & " and ItemID=" & Val(dg1.SelectedRows(0).Cells(7).Value) & " and Lot='" & dg1.SelectedRows(0).Cells(1).Value & "'")
            If SoldLot <> "" Then
                If SoldLot = TxtPurchaseLot.Text Then
                    MsgBox("Emtry Can't Remove..." & vbNewLine & "Lot No. : " & dg1.SelectedRows(0).Cells(1).Value & " Already in Sale Transaction..." & vbNewLine & "You Entered  Lot No. : " & TxtPurchaseLot.Text & "", MsgBoxStyle.Critical, "Access Denied..") : TxtPurchaseLot.Focus() : Exit Sub
                End If
            End If
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
                'ClearDetails()
            End If
        End If
        calc()
    End Sub
    Private Sub txtCrateQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCrateQty.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtWeight.Focus()
        End If
    End Sub

    Private Sub txtPurchaseNug_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNug.KeyDown
        If e.KeyCode = Keys.Enter Then
            If lblCrate.Text = "Y" Then
                If txtAccountID.Text = 7 Then
                    cbAccountName.Focus()
                Else
                    cbCrateMarka.Focus()
                End If
            Else
                txtWeight.Focus()
            End If
        End If
    End Sub

    Private Sub txtWeight_GotFocus(sender As Object, e As EventArgs) Handles txtWeight.GotFocus
        If BtnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) > 0 Then
                txtWeight.Text = Val(clsFun.ExecScalarStr(" Select WeightPerNug FROM Items WHERE ID='" & Val(txtItemID.Text) & "'")) * Val(txtNug.Text)
            End If
        End If
    End Sub

    Private Sub txtchargesAmount_GotFocus(sender As Object, e As EventArgs) Handles txtchargesAmount.GotFocus
        dgItemPurchase.Visible = False : DgAccountSearch.Visible = False
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If dgCharges.SelectedRows.Count = 0 Then txtCharges.Focus() : Exit Sub
        txtCharges.Text = dgCharges.SelectedRows(0).Cells(1).Value
        txtChargeID.Text = Val(dgCharges.SelectedRows(0).Cells(0).Value)
        dgCharges.Visible = False ' FillCharges()
        If dgCharges.SelectedRows.Count = 0 Then dgCharges.Visible = True
        txtCharges.SelectAll()
    End Sub


    Private Sub txtNug_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNug.KeyPress, txtRate.KeyPress,
        txtWeight.KeyPress, txtAmount.KeyPress, txtOnValue.KeyPress,
       txtCalculatePer.KeyPress, txtchargesAmount.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
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
    Private Sub SaleRowColums()
        dg3.ColumnCount = 11
        dg3.Columns(0).Name = "PurcahseID" : dg3.Columns(0).Visible = False
        dg3.Columns(1).Name = "Date" : dg3.Columns(1).Width = 100
        dg3.Columns(2).Name = "Invoice No" : dg3.Columns(2).Width = 150
        dg3.Columns(3).Name = "Customer Name" : dg3.Columns(3).Width = 200
        dg3.Columns(4).Name = "Item Name" : dg3.Columns(4).Width = 200
        dg3.Columns(5).Name = "Lot" : dg3.Columns(5).Width = 100
        dg3.Columns(6).Name = "Nug" : dg3.Columns(6).Width = 100
        dg3.Columns(7).Name = "Weight" : dg3.Columns(7).Width = 100
        dg3.Columns(8).Name = "Rate" : dg3.Columns(8).Width = 100
        dg3.Columns(9).Name = "Amount" : dg3.Columns(9).Width = 120
        dg3.Columns(10).Name = "Transtype" : dg3.Columns(10).Visible = False
    End Sub
    Private Sub retriveSale(Optional ByVal condtion As String = "")
        dg3.Rows.Clear() : lblNugs.Text = Format(Val(0), "0.00") : lblWeight.Text = Format(Val(0), "0.00") : lblAmount.Text = Format(Val(0), "0.00")
        Dim dt As New DataTable
        If cbBillingType.SelectedIndex = 0 Then
            dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE TransType<>'Store Out' and PurchaseID=" & Val(txtid.Text) & " order by ID")
        ElseIf cbBillingType.SelectedIndex = 1 Then
            dt = clsFun.ExecDataTable("Select VoucherID,'' as TransType,'' as BillNo,'' as EntryDate,'' as AccountName, ItemName,lot,sum(nug) as nug,sum(weight) as weight,(sum(Amount) / sum(weight)) as Rate, avg(SRate) as SRate,Per,sum(Amount)  As Amount FROM Transaction2 WHERE  TransType<>'Store Out' and PurchaseID=" & Val(txtid.Text) & " Group by ItemName,Lot order by Rate Desc")
        ElseIf cbBillingType.SelectedIndex = 2 Then
            dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE PurchaseID=" & Val(txtid.Text) & "  order by SRate Desc")
        ElseIf cbBillingType.SelectedIndex = 3 Then
            dt = clsFun.ExecDataTable("Select VoucherID,'' as TransType,'' as BillNo,'' as EntryDate,'' as AccountName, ItemName,Lot,Round(sum(Nug),2) as nug,Round(sum(Weight),2) as weight,Round(Rate,2) as Rate,Per,Round(sum(Amount),2) as Amount FROM Transaction2 WHERE TransType<>'Store Out' and PurchaseID=" & Val(txtid.Text) & " Group by ItemName,Lot order by Rate Desc")
        ElseIf cbBillingType.SelectedIndex = 4 Then
            dt = clsFun.ExecDataTable("Select VoucherID,'' as TransType,'' as BillNo,'' as EntryDate,'' as AccountName, ItemName,Lot,Round(sum(Nug),2) as nug,Round(sum(Weight),2) as weight,Round(Rate,2) as Rate,Per,Round(sum(Amount),2) as Amount FROM Transaction2 WHERE TransType<>'Store Out' and PurchaseID=" & Val(txtid.Text) & " Group By ItemName, Lot,SRate order by SRate Desc")
        End If

        Dim dvData As DataView = New DataView(dt)
        Try
            If dt.Rows.Count > 0 Then
                If dt.Rows.Count > 12 Then dg3.Columns(9).Width = 98 Else dg3.Columns(9).Width = 120
                lblNugs.Text = Format(Val(dt.Compute("Sum(Nug)", "")), "0.00")
                lblWeight.Text = Format(Val(dt.Compute("Sum(Weight)", "")), "0.00")
                lblAmount.Text = Format(Val(dt.Compute("Sum(Amount)", "")), "0.00")
                dg3.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg3.Rows.Add()
                    With dg3.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("billNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(5).Value = dt.Rows(i)("Lot").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(10).Value = dt.Rows(i)("Transtype").ToString()
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next
            End If
            dg3.ClearSelection()
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try

    End Sub

    Private Sub rowColumsPurchase()
        dg1.ColumnCount = 17
        dg1.Columns(0).Name = "Item Name" : dg1.Columns(0).Width = 353
        dg1.Columns(1).Name = "Lot No" : dg1.Columns(1).Width = 149
        dg1.Columns(2).Name = "Nug" : dg1.Columns(2).Width = 129
        dg1.Columns(3).Name = "Weight" : dg1.Columns(3).Width = 129
        dg1.Columns(4).Name = "Rate" : dg1.Columns(4).Width = 129
        dg1.Columns(5).Name = "Per" : dg1.Columns(5).Width = 117
        dg1.Columns(6).Name = "Amount" : dg1.Columns(6).Width = 160
        dg1.Columns(7).Name = "ItemID" : dg1.Columns(7).Width = 120
        dg1.Columns(8).Name = "StoreID" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "StoreName" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "CrateY/N" : dg1.Columns(10).Width = 120
        dg1.Columns(11).Name = "CrateID" : dg1.Columns(11).Width = 100
        dg1.Columns(12).Name = "CrateName" : dg1.Columns(12).Width = 100
        dg1.Columns(13).Name = "CrateQty" : dg1.Columns(13).Width = 100
        dg1.Columns(14).Name = "StockHolderID" : dg1.Columns(14).Width = 100
        dg1.Columns(15).Name = "StockHolderName" : dg1.Columns(15).Width = 100
        dg1.Columns(16).Name = "StockHolderName" : dg1.Columns(16).Width = 100
        dg1.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Private Sub dg2RownColums()
        Dg2.ColumnCount = 7
        Dg2.Columns(0).Name = "Charge Name" : Dg2.Columns(0).Width = 273
        Dg2.Columns(1).Name = "On" : Dg2.Columns(1).Width = 130
        Dg2.Columns(2).Name = "Cal" : Dg2.Columns(2).Width = 99
        Dg2.Columns(3).Name = "+/-" : Dg2.Columns(3).Width = 99
        Dg2.Columns(4).Name = "Amount" : Dg2.Columns(4).Width = 150
        Dg2.Columns(5).Name = "ChargeID" : Dg2.Columns(5).Width = 110
        Dg2.Columns(6).Name = "ID" : Dg2.Columns(6).Visible = False
        Dg2.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        Dg2.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Dg2.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dg2.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Dg2.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        Dg2.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Dg2.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

    End Sub
    Private Sub FillCharges()
        CalcType = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        txtPlusMinus.Text = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        txtCalculatePer.Text = clsFun.ExecScalarStr(" Select Calculate FROM Charges WHERE ID='" & txtChargeID.Text & "'")
        If CalcType = "Aboslute" Then
            txtOnValue.TabStop = False
            txtOnValue.Text = ""
            txtCalculatePer.Text = ""
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
            txtOnValue.Text = Val(lblCrateQty.Text)
            txtOnValue.TabStop = True
            txtCalculatePer.TabStop = True
        End If
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnSave.Image = My.Resources.Edit
        BtnSave.BackColor = Color.Coral
        BtnDelete.Enabled = True
        txtAccount.Enabled = False
        txtStorage.Enabled = False
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & id
        Dim sql As String = "Select * from Purchase where VoucherID=" & id
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        Dim ds As New DataSet
        Dim ds1 As New DataSet
        Dim ds2 As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds1, "b")
        ad2.Fill(ds2, "c")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("SallerName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("DiscountAmount").ToString()), "0.00")
            txtTotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("billNo").ToString()
            CbpurchaseType.Text = ds.Tables("a").Rows(0)("PurchaseType").ToString()
            txtStorageID.Text = ds.Tables("a").Rows(0)("StorageID").ToString()
            txtStorage.Text = ds.Tables("a").Rows(0)("StorageName").ToString()
            cbFarmer.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
        End If
        '  If ds.Tables("b").Rows.Count > 0 Then dgStockIn.Rows.Clear()
        If ds1.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds1.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells("Item Name").Value = ds1.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells("Lot No").Value = ds1.Tables("b").Rows(i)("LotNo").ToString()
                .Rows(i).Cells("Nug").Value = Format(Val(ds1.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells("Weight").Value = Format(Val(ds1.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells("ItemID").Value = ds1.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells("StoreID").Value = ds1.Tables("b").Rows(i)("StorageID").ToString()
                .Rows(i).Cells("StoreName").Value = ds1.Tables("b").Rows(i)("StorageName").ToString()
                .Rows(i).Cells("CrateY/N").Value = ds1.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells("CrateID").Value = ds1.Tables("b").Rows(i)("CrateID").ToString()
                .Rows(i).Cells("CrateName").Value = ds1.Tables("b").Rows(i)("CrateName").ToString()
                .Rows(i).Cells("CrateQty").Value = Val(ds1.Tables("b").Rows(i)("CrateQty").ToString())
                .Rows(i).Cells("Rate").Value = Format(Val(ds1.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells("per").Value = ds1.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells("Amount").Value = Format(Val(ds1.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells("StockHolderID").Value = ds1.Tables("b").Rows(i)("StockHolderID").ToString()
                .Rows(i).Cells("StockHolderName").Value = ds1.Tables("b").Rows(i)("StockHolderName").ToString()
            Next
        End With
        If ds2.Tables("c").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds2.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds2.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On").Value = Format(Val(ds2.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds2.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds2.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds2.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds2.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        Offset = clsFun.ExecScalarInt("SELECT COUNT(*) AS RowPosition FROM Vouchers WHERE transtype ='Purchase' AND ID < " & Val(txtid.Text) & " ORDER BY ID DESC")
        If dg3.ColumnCount = 0 Then SaleRowColums()
        retriveSale() : calc()
        dg1.ClearSelection()
        Dg2.ClearSelection()
        If CbpurchaseType.SelectedIndex = 0 Then
            txtRate.Enabled = False
            txtRate.BackColor = Color.Gray
        Else
            txtRate.Enabled = True
            txtRate.BackColor = Color.GhostWhite
        End If

    End Sub
    Public Sub FillWithNevigation()
        dg1.Rows.Clear() : Dg2.Rows.Clear()
        Dim id As Integer = 0
        If BtnSave.Text = "&Save" And dg1.RowCount > 0 Then MsgBox("Save Transaction First...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        BtnSave.Image = My.Resources.Edit
        BtnSave.BackColor = Color.Coral
        BtnDelete.Enabled = True
        txtAccount.Enabled = False
        txtStorage.Enabled = False
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Purchase'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers  WHERE transtype = 'Purchase'   Order By ID LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString() + ""
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtAccountID.Text = ds.Tables("a").Rows(0)("SallerID").ToString()
            txtAccount.Text = ds.Tables("a").Rows(0)("SallerName").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("VehicleNo").ToString()
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtTotalNug.Text = Format(Val(ds.Tables("a").Rows(0)("Nug").ToString()), "0.00")
            txttotalWeight.Text = Format(Val(ds.Tables("a").Rows(0)("kg").ToString()), "0.00")
            txtbasicTotal.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txttotalCharges.Text = Format(Val(ds.Tables("a").Rows(0)("DiscountAmount").ToString()), "0.00")
            txtTotalNetAmount.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("billNo").ToString()
            CbpurchaseType.Text = ds.Tables("a").Rows(0)("PurchaseType").ToString()
            txtStorageID.Text = ds.Tables("a").Rows(0)("StorageID").ToString()
            txtStorage.Text = ds.Tables("a").Rows(0)("StorageName").ToString()
            cbFarmer.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            txtInvoiceID.Text = ds.Tables("a").Rows(0)("InvoiceID").ToString()
            id = Val(txtid.Text)
        End If
        '  If ds.Tables("b").Rows.Count > 0 Then dgStockIn.Rows.Clear()
        '  
        Dim sql As String = "Select * from Purchase where VoucherID=" & Val(id)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        ad1.Fill(ds, "b")
        If ds.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells("Item Name").Value = ds.Tables("b").Rows(i)("ItemName").ToString()
                .Rows(i).Cells("Lot No").Value = ds.Tables("b").Rows(i)("LotNo").ToString()
                .Rows(i).Cells("Nug").Value = Format(Val(ds.Tables("b").Rows(i)("Nug").ToString()), "0.00")
                .Rows(i).Cells("Weight").Value = Format(Val(ds.Tables("b").Rows(i)("Weight").ToString()), "0.00")
                .Rows(i).Cells("ItemID").Value = ds.Tables("b").Rows(i)("ItemID").ToString()
                .Rows(i).Cells("StoreID").Value = ds.Tables("b").Rows(i)("StorageID").ToString()
                .Rows(i).Cells("StoreName").Value = ds.Tables("b").Rows(i)("StorageName").ToString()
                .Rows(i).Cells("CrateY/N").Value = ds.Tables("b").Rows(i)("MaintainCrate").ToString()
                .Rows(i).Cells("CrateID").Value = ds.Tables("b").Rows(i)("CrateID").ToString()
                .Rows(i).Cells("CrateName").Value = ds.Tables("b").Rows(i)("CrateName").ToString()
                .Rows(i).Cells("CrateQty").Value = Val(ds.Tables("b").Rows(i)("CrateQty").ToString())
                .Rows(i).Cells("Rate").Value = Format(Val(ds.Tables("b").Rows(i)("Rate").ToString()), "0.00")
                .Rows(i).Cells("per").Value = ds.Tables("b").Rows(i)("Per").ToString()
                .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("b").Rows(i)("Amount").ToString()), "0.00")
                .Rows(i).Cells("StockHolderID").Value = ds.Tables("b").Rows(i)("StockHolderID").ToString()
                .Rows(i).Cells("StockHolderName").Value = ds.Tables("b").Rows(i)("StockHolderName").ToString()
            Next
        End With
        Dim Ssqll As String = "Select * from ChargesTrans where VoucherID=" & Val(id)
        Dim ad2 As New SQLite.SQLiteDataAdapter(Ssqll, clsFun.GetConnection)
        ad2.Fill(ds, "c")
        If ds.Tables("c").Rows.Count > 0 Then
            Dg2.Rows.Clear()
            With Dg2
                Dim i As Integer = 0
                For i = 0 To ds.Tables("C").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("Charge Name").Value = ds.Tables("c").Rows(i)("ChargeName").ToString()
                    .Rows(i).Cells("On").Value = Format(Val(ds.Tables("c").Rows(i)("OnValue").ToString()), "0.00")
                    .Rows(i).Cells("Cal").Value = Format(Val(ds.Tables("c").Rows(i)("Calculate").ToString()), "0.00")
                    .Rows(i).Cells("+/-").Value = ds.Tables("c").Rows(i)("ChargeType").ToString()
                    .Rows(i).Cells("Amount").Value = Format(Val(ds.Tables("c").Rows(i)("Amount").ToString()), "0.00")
                    .Rows(i).Cells("ChargeID").Value = ds.Tables("c").Rows(i)("ChargesID").ToString()
                Next
            End With
        End If
        If dg3.ColumnCount = 0 Then SaleRowColums()
        retriveSale()
        calc()
        dg1.ClearSelection()
        Dg2.ClearSelection()
        If CbpurchaseType.SelectedIndex = 0 Then
            txtRate.Enabled = False
            txtRate.BackColor = Color.Gray
        Else
            txtRate.Enabled = True
            txtRate.BackColor = Color.GhostWhite
        End If
    End Sub

    Private Sub ChargesCalculation()
        If CalcType = "Percentage" Then
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text) / 100, "0.00")
        ElseIf CalcType = "Nug" Then
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Weight" Then
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text), "0.00")
        ElseIf CalcType = "Crate" Then
            txtchargesAmount.Text = Format(Val(txtOnValue.Text) * Val(txtCalculatePer.Text), "0.00")
        End If
        txtTotalNetAmount.Text = Format(Val(txtbasicTotal.Text) + Val(txttotalCharges.Text), "0.00")
    End Sub

    Private Sub txtPurchaseNug_Leave(sender As Object, e As EventArgs) Handles txtNug.Leave
        If lblCrate.Text = "N" Then Exit Sub
        txtCrateQty.Text = txtNug.Text
    End Sub
    Private Sub txtNug_KeyUp(sender As Object, e As EventArgs) Handles txtNug.KeyUp, cbPer.KeyUp,
    txtWeight.KeyUp, txtRate.KeyUp, txtbasicTotal.KeyUp
        SpeedCalculation()
    End Sub

    Private Sub txtOnValue_Leave(sender As Object, e As EventArgs) Handles txtOnValue.Leave, txtchargesAmount.Leave, txtCalculatePer.Leave
        ChargesCalculation()
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
            txtCharges.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If Dg2.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                Dg2.Rows.Remove(Dg2.SelectedRows(0))
            End If
        End If
        calc()
        If String.IsNullOrEmpty(txtbasicTotal.Text) OrElse String.IsNullOrEmpty(txttotalCharges.Text) Then Exit Sub
        txtTotalNetAmount.Text = Format(Val(txtbasicTotal.Text) + Val(txttotalCharges.Text), "0.00")
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
    End Sub

    Private Sub txtCharges_GotFocus(sender As Object, e As EventArgs) Handles txtCharges.GotFocus
        dgCharges.Visible = True : dgItemPurchase.Visible = False : DgAccountSearch.Visible = False : dgStore.Visible = False
        If dgCharges.ColumnCount = 0 Then ChargesRowColums()
        If dgCharges.RowCount = 0 Then RetriveCharges()
        If txtCharges.Text.Trim() <> "" Then
            'dgCharges.Visible = True
            RetriveCharges(" Where upper(ChargeName) Like upper('" & txtCharges.Text.Trim() & "%')")
        Else
            RetriveCharges()
        End If
    End Sub
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

    Private Sub txtMode_GotFocus(sender As Object, e As EventArgs) Handles txtStorage.GotFocus
        If txtAccount.Enabled <> False Then
            If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
            If DgAccountSearch.RowCount = 0 Then retriveAccounts()
            If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
            txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False ': AcBal()
        End If
        If dgStore.ColumnCount = 0 Then StoreCoulums()
        If dgStore.RowCount = 0 Then RetriveMode()
        If txtStorage.Text.Trim() <> "" Then
            RetriveMode(" Where upper(StorageName) Like upper('" & txtStorage.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        txtStorage.SelectionStart = 0 : txtStorage.SelectionLength = Len(txtStorage.Text)
    End Sub
    Private Sub TxtPurchaseLot_GotFocus(sender As Object, e As EventArgs) Handles TxtPurchaseLot.GotFocus
        If dgItemPurchase.ColumnCount = 0 Then PurchaseItemRowColums()
        If dgItemPurchase.Rows.Count = 0 Then retrivePurchaseItems()
        If txtItem.Text.Trim() <> "" Then
            retrivePurchaseItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            retrivePurchaseItems()
        End If
        If dgItemPurchase.SelectedRows.Count = 0 Then dgItemPurchase.Visible = True : txtItem.Focus() : Exit Sub
        If dgItemPurchase.SelectedRows.Count = 0 Then Exit Sub
        txtItemID.Text = Val(dgItemPurchase.SelectedRows(0).Cells(0).Value)
        txtItem.Text = dgItemPurchase.SelectedRows(0).Cells(1).Value
        dgItemPurchase.Visible = False : itemfill()
        TxtPurchaseLot.SelectAll()
    End Sub

    Private Sub txtItem_GotFocus1(sender As Object, e As EventArgs) Handles txtItem.GotFocus
        If txtStorage.Enabled = True Then
            If dgStore.ColumnCount = 0 Then StoreCoulums()
            If dgStore.RowCount = 0 Then RetriveMode()
            If dgStore.SelectedRows.Count = 0 Then dgStore.Visible = True : txtStorage.Focus() : Exit Sub
            If txtStorage.Text.Trim() <> "" Then
                RetriveMode(" Where upper(StorageName) Like upper('" & txtStorage.Text.Trim() & "%')")
            Else
                RetriveMode()
            End If
            txtStorageID.Text = Val(dgStore.SelectedRows(0).Cells(0).Value)
            txtStorage.Text = dgStore.SelectedRows(0).Cells(1).Value
            dgStore.Visible = False
        End If
        If dgItemPurchase.ColumnCount = 0 Then PurchaseItemRowColums()
        If dgItemPurchase.RowCount = 0 Then retrivePurchaseItems()
        If txtItem.Text.Trim() <> "" Then
            retrivePurchaseItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            retrivePurchaseItems()
        End If
        dgItemPurchase.Visible = True : txtItem.Focus()
        txtItem.SelectionStart = 0 : txtItem.SelectionLength = Len(txtItem.Text)
    End Sub

    Private Sub txtVoucherNo_GotFocus(sender As Object, e As EventArgs) Handles txtVoucherNo.GotFocus, txtVehicleNo.GotFocus,
        txtStorage.GotFocus, txtItem.GotFocus, TxtPurchaseLot.GotFocus, txtAccount.GotFocus, txtNug.GotFocus, txtWeight.GotFocus, txtRate.GotFocus, txtAmount.GotFocus
        If txtItem.Focused = True Then dgItemPurchase.Visible = True : DgAccountSearch.Visible = False : dgStore.Visible = False : dgCharges.Visible = False
        If txtAccount.Focused = True Then dgItemPurchase.Visible = False : DgAccountSearch.Visible = True : dgStore.Visible = False : dgCharges.Visible = False
        If txtStorage.Focused = True Then dgItemPurchase.Visible = False : DgAccountSearch.Visible = False : dgStore.Visible = True : dgCharges.Visible = False
        If txtNug.Focused Or TxtPurchaseLot.Focused Then
            If dgItemPurchase.SelectedRows.Count = 0 Then Exit Sub
            txtItemID.Text = Val(dgItemPurchase.SelectedRows(0).Cells(0).Value)
            txtItem.Text = dgItemPurchase.SelectedRows(0).Cells(1).Value
            dgItemPurchase.Visible = False : itemfill()
        End If
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub

    Private Sub txtVoucherNo_LostFocus(sender As Object, e As EventArgs) Handles txtVoucherNo.LostFocus, txtVehicleNo.LostFocus,
    txtStorage.LostFocus, txtItem.LostFocus, TxtPurchaseLot.LostFocus, txtAccount.LostFocus, txtNug.LostFocus, txtWeight.LostFocus, txtRate.LostFocus, txtAmount.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.GhostWhite
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.BackColor = Color.LightGray
        mskEntryDate.SelectAll()
    End Sub

    Private Sub cbFarrmer_GotFocus(sender As Object, e As EventArgs) Handles cbFarmer.GotFocus
        cbFarmer.SelectAll()
        If dgStore.ColumnCount = 0 Then StoreCoulums()
        If dgStore.RowCount = 0 Then RetriveMode()
        If dgStore.SelectedRows.Count = 0 Then dgStore.Visible = True : txtStorage.Focus() : Exit Sub
        If txtStorage.Text.Trim() <> "" Then
            RetriveMode(" Where upper(StorageName) Like upper('" & txtStorage.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        txtStorageID.Text = Val(dgStore.SelectedRows(0).Cells(0).Value)
        txtStorage.Text = dgStore.SelectedRows(0).Cells(1).Value

        dgStore.Visible = False
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown,
        txtVehicleNo.KeyDown, CbpurchaseType.KeyDown, txtStorage.KeyDown, txtItem.KeyDown,
         TxtPurchaseLot.KeyDown, txtAccount.KeyDown, cbFarmer.KeyDown,
        txtWeight.KeyDown, cbPer.KeyDown, txtRate.KeyDown
        If txtVoucherNo.Focused Then
            If e.KeyCode = Keys.F2 Then
                pnlInvoiceID.Visible = True
                txtInvoiceID.Focus()
                e.SuppressKeyPress = True
            End If
        End If

        If txtAccount.Focused Then
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
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus() : Exit Sub
        End If
        If txtStorage.Focused Then
            If e.KeyCode = Keys.F3 Then
                Store.MdiParent = MainScreenForm
                Store.Show()
                If Not Store Is Nothing Then
                    Store.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.Down Then dgStore.Focus() : Exit Sub
        End If
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
            Case Keys.PageDown
                e.Handled = True
                txtCharges.Focus()
        End Select
        If txtItem.Focused Then
            If e.KeyCode = Keys.F3 Then
                Item_form.MdiParent = MainScreenForm
                Item_form.Show()
                Item_form.Opener = Me
                Item_form.OpenedFromItems = True
                Item_form.BringToFront()
                '    Dim CutStop As String = String.Empty
            End If
            If e.KeyCode = Keys.F1 Then
                If Val(dgItemPurchase.SelectedRows(0).Cells(0).Value) = 0 Then Exit Sub
                Item_form.MdiParent = MainScreenForm
                Item_form.FillContros(Val(dgItemPurchase.SelectedRows(0).Cells(0).Value))
                Item_form.Show()
                Item_form.BringToFront()
                Item_form.Opener = Me
                Item_form.OpenedFromItems = True
                '     Dim CutStop As String = String.Empty
            End If
            If e.KeyCode = Keys.Down Then dgItemPurchase.Focus()
        End If
        If txtItem.Focused Then
            If e.KeyCode = Keys.Down Then
                If dgItemPurchase.Visible = True Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If dgItemPurchase.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If cbCrateMarka.Focused = True Or cbPer.Focused = True Or cbCrateMarka.Focused = True Or cbAccountName.Focused = True Or txtCrateQty.Focused = True Then Exit Sub
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
                ChargesForm.MdiParent = MainScreenForm
                Dim tmpCharges As String = txtChargeID.Text
                ChargesForm.Show()
                ChargesForm.FillContros(tmpCharges)
                If Not ChargesForm Is Nothing Then
                    ChargesForm.BringToFront()
                End If
            End If
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

    Sub calc()
        txtTotalNug.Text = Format(0, "0.00") : txttotalWeight.Text = Format(0, "0.00")
        txtbasicTotal.Text = Format(0, "0.00") : txtroundoff.Text = Format(0, "0.00")
        lblCrateQty.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNug.Text = Format(Val(txtTotalNug.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txttotalWeight.Text = Format(Val(txttotalWeight.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtbasicTotal.Text = Format(Val(txtbasicTotal.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            lblCrateQty.Text = Format(Val(lblCrateQty.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
        Next
        txttotalCharges.Text = Format(0, "0.00")
        For i = 0 To Dg2.Rows.Count - 1
            Dim CalcType As String = clsFun.ExecScalarStr(" Select ApplyType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
            Dim PlusMinus As String = clsFun.ExecScalarStr(" Select ChargesType FROM Charges WHERE ID='" & Val(Dg2.Rows(i).Cells(5).Value) & "'")
            If Dg2.Rows(i).Cells(3).Value = "-" Then
                txttotalCharges.Text = Format(Val(txttotalCharges.Text) - Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            Else
                txttotalCharges.Text = Format(Val(txttotalCharges.Text) + Val(Dg2.Rows(i).Cells(4).Value), "0.00")
            End If
        Next
        txtTotalNetAmount.Text = Format(Val(Val(txtbasicTotal.Text)) + Val(Val(txttotalCharges.Text)), "0.00")
        Dim tmpamount As Double = Math.Abs(Val(txtTotalNetAmount.Text))
        txtTotalNetAmount.Text = Math.Round(Val(tmpamount), 0)
        txtroundoff.Text = Format(Math.Round(Math.Abs(Val(txtTotalNetAmount.Text)) - Val(tmpamount), 2), "0.00")
        txtTotalNetAmount.Text = Format(Val(txtTotalNetAmount.Text), "0.00")
    End Sub

    Private Sub AlltextClear()
        dgCharges.Visible = False : txtAccount.Enabled = True
        txtid.Text = "" : txtroundoff.Text = ""
        txtAccount.Text = "" : txtAccountID.Text = ""
        txtItemIDStockIn.Text = ""
        txtStorage.Text = "" : txtStorageID.Text = ""
        txtTotalNug.Text = "" : txtbasicTotal.Text = ""
        txttotalWeight.Text = "" : txtTotalNetAmount.Text = ""
        txttotalCharges.Text = "" : mskEntryDate.Focus()
        BtnDelete.Enabled = False : BtnSave.Text = "&Save"
        VNumber() : ChargesClear() : cbFarmer.Text = ""
        txtVehicleNo.Text = "" : dg1.Rows.Clear()
        Dg2.Rows.Clear() : BtnSave.Image = My.Resources.Save
        BtnSave.BackColor = Color.DarkTurquoise
        CbpurchaseType.Enabled = True
        pnlMarka.Visible = False
    End Sub

    Private Sub PurchaseClear()
        txtItem.Clear()
        TxtPurchaseLot.Text = "" : txtNug.Text = ""
        txtWeight.Text = "" : txtRate.Text = ""
        txtAmount.Text = "" : cbCrateMarka.SelectedValue = 0
    End Sub

    Private Sub ChargesClear()
        txtCharges.Clear() : txtChargeID.Clear()
        txtOnValue.Clear() : txtCalculatePer.Clear()
        txtPlusMinus.Clear() : txtchargesAmount.Clear()
    End Sub
    Private Sub SpeedCalculation()
        If cbPer.SelectedIndex = 0 Then
            txtAmount.Text = Format(Math.Round(Val(txtNug.Text) * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 1 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 2 Then
            txtAmount.Text = Format(Math.Round(Val(txtRate.Text) / 5 * Val(txtWeight.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 3 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 10 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 4 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 20 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 5 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 40 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 6 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 41 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 7 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 50 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 8 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 51 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 9 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 51.7 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 10 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 52.2 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 11 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 52.3 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 12 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 52.5 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 13 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 53 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 14 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 80 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 15 Then
            txtAmount.Text = Format(Math.Round(Val(txtWeight.Text) / 100 * Val(txtRate.Text), 2), "0.00")
        ElseIf cbPer.SelectedIndex = 16 Then
            txtAmount.Text = Format(Math.Round(Val(txtNug.Text) * Val(txtRate.Text), 2), "0.00")
        End If
        txtAmount.Text = Format(Math.Round(Val(txtAmount.Text), 0), "0.00")
        'txtTotalNetAmount.Text = Format(Val(txtbasicTotal.Text) + Val(txttotalCharges.Text), "0.00")
        'txtTotalNetAmount.Text = Format(Val(txtbasicTotal.Text) + Val(txttotalCharges.Text), "0.00")

    End Sub

    Private Sub dgPurchase_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles dg1.RowStateChanged
        If dg1.RowCount > 0 Then
            CbpurchaseType.Enabled = False
        Else
            CbpurchaseType.Enabled = True
        End If
    End Sub
    Private Sub cbPurchaseItem_SelectedIndexChanged(sender As Object, e As EventArgs)
        itemfill()
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
        If dg1.RowCount = 0 Then MsgBox("There is no record to Save / Update...", vbOKOnly, "Empty") : Exit Sub
        If BtnSave.Text = "&Save" Then
            Dim AddPurchase As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Purchase'")
            If AddPurchase <> "Y" Then MsgBox("You Don't Have Rights to Add Purchase... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : AlltextClear() : Exit Sub
            ButtonControl() : SaveDgPurchase() : ButtonControl()
        Else
            Dim ModifyPurchase As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Purchase'")
            If ModifyPurchase <> "Y" Then MsgBox("You Don't Have Rights to Modify Purchase... " & vbNewLine & " Please Contact to Admin...", MsgBoxStyle.Critical, "Access Denied") : AlltextClear() : Exit Sub
            ButtonControl() : UpdatePurchase() : ButtonControl()
        End If
        Dim res = MessageBox.Show("Do you want to Print Purchase Bill", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If res = Windows.Forms.DialogResult.Yes Then
            btnPrint.Enabled = True
            btnPrint.PerformClick()
            RadioPrint1.Checked = True
            Exit Sub
        Else
            txtid.Clear()
        End If
        AlltextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
        If clsFun.ExecScalarStr("Select AutoSwitch From Controls") = "No" Then Exit Sub
        If MessageBox.Show(" Do you Want to Sale Now???", "Sale Now", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            Stock_Sale.MdiParent = MainScreenForm
            Stock_Sale.Show()
            Stock_Sale.BringToFront()
        End If

    End Sub
    Private Sub cbCrateMarka_KeyDown1(sender As Object, e As KeyEventArgs) Handles cbCrateMarka.KeyDown
        If e.KeyCode = Keys.Enter Then txtCrateQty.Focus()
        If e.KeyCode = Keys.F3 Then
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            If Not CrateForm Is Nothing Then CrateForm.BringToFront()
        End If
        If e.KeyCode = Keys.F1 Then
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            Dim tmpID As String = cbCrateMarka.SelectedValue
            CrateForm.FillControls(tmpID)
            If Not CrateForm Is Nothing Then CrateForm.BringToFront()
        End If
    End Sub

    Public Sub crateRefresh()
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
    End Sub

    Private Sub txtCrateQty_KeyDown1(sender As Object, e As KeyEventArgs) Handles txtCrateQty.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtWeight.Focus()
            pnlMarka.Visible = False
        End If
    End Sub

    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub

    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ButtonControl() : delete() : ButtonControl()
    End Sub

    Private Sub dgPurchase_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtItem.Text = dg1.SelectedRows(0).Cells(0).Value
        TxtPurchaseLot.Text = dg1.SelectedRows(0).Cells(1).Value
        txtNug.Text = dg1.SelectedRows(0).Cells(2).Value
        txtWeight.Text = dg1.SelectedRows(0).Cells(3).Value
        txtRate.Text = dg1.SelectedRows(0).Cells(4).Value
        cbPer.Text = dg1.SelectedRows(0).Cells(5).Value
        txtAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        'txtModeID.Text = dgPurchase.SelectedRows(0).Cells(6).Value
        cbPer.Text = dg1.SelectedRows(0).Cells(5).Value
        txtAmount.Text = dg1.SelectedRows(0).Cells(6).Value
        txtStorage.Text = dg1.SelectedRows(0).Cells(9).Value
        lblCrate.Text = dg1.SelectedRows(0).Cells(10).Value
        cbCrateMarka.Text = dg1.SelectedRows(0).Cells(12).Value
        txtCrateQty.Text = dg1.SelectedRows(0).Cells(13).Value
        '    txtAccountID.Text = dgPurchase.SelectedRows(0).Cells(14).Value
        '   txtAccount.Text = dgPurchase.SelectedRows(0).Cells(15).Value
        txtItemID.Text = Val(dg1.SelectedRows(0).Cells(7).Value)
    End Sub

    Private Sub dgPurchase_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
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
        End If
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=12", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If dgStore.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = dgStore.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Back Then txtStorage.Focus()
    End Sub
    Private Sub StoreCoulums()
        dgStore.ColumnCount = 2
        dgStore.Columns(0).Name = "ID" : dgStore.Columns(0).Visible = False
        dgStore.Columns(1).Name = "Mode Name" : dgStore.Columns(1).Width = 230
        dgStore.BringToFront() : dgStore.Visible = True
        RetriveMode()
    End Sub
    Private Sub RetriveMode(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Storage  " & condtion & " order by StorageName")
        Try
            If dt.Rows.Count > 0 Then
                dgStore.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgStore.Rows.Add()
                    With dgStore.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
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
        If txtStorage.Text.Trim() <> "" Then
            RetriveMode(" Where upper(StorageName) Like upper('" & txtStorage.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
    End Sub
    Private Sub PurchaseItemRowColums()
        dgItemPurchase.ColumnCount = 3
        dgItemPurchase.Columns(0).Name = "ID" : dgItemPurchase.Columns(0).Visible = False
        dgItemPurchase.Columns(1).Name = "Item Name" : dgItemPurchase.Columns(1).Width = 200
        dgItemPurchase.Columns(2).Name = "OtherName" : dgItemPurchase.Columns(2).Width = 160
        dgItemPurchase.BringToFront() : retrivePurchaseItems()
    End Sub

    Private Sub retrivePurchaseItems(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Items " & condtion & " order by ItemName")
        Try
            If dt.Rows.Count > 0 Then
                dgItemPurchase.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgItemPurchase.Rows.Add()
                    With dgItemPurchase.Rows(i)
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

    Private Sub txtItem_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtItem.KeyPress
        'dgItemPurchase.Visible = True
    End Sub
    Private Sub dgItemPurchase_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgItemPurchase.CellClick
        txtItem.Clear()
        txtItemID.Clear()
        txtItemID.Text = dgItemPurchase.SelectedRows(0).Cells(0).Value
        txtItem.Text = dgItemPurchase.SelectedRows(0).Cells(1).Value
        itemfill()
        TxtPurchaseLot.Focus()
    End Sub

    Private Sub dgItemPurchase_GotFocus(sender As Object, e As EventArgs) Handles dgItemPurchase.GotFocus

    End Sub
    Private Sub dgItemPurchase_KeyDown(sender As Object, e As KeyEventArgs) Handles dgItemPurchase.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtItem.Clear()
            txtItemID.Text = dgItemPurchase.SelectedRows(0).Cells(0).Value
            txtItem.Text = dgItemPurchase.SelectedRows(0).Cells(1).Value
            itemfill()
            e.SuppressKeyPress = True
            TxtPurchaseLot.Focus()
        End If
        If e.KeyCode = Keys.Back Then txtItem.Focus()
        If e.KeyCode = Keys.F3 Then
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If dgItemPurchase.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dgItemPurchase.SelectedRows(0).Cells(0).Value
            Item_form.MdiParent = MainScreenForm
            Item_form.Show()
            Item_form.FillContros(tmpID)
            If Not Item_form Is Nothing Then
                Item_form.BringToFront()
            End If
        End If
    End Sub
    Private Sub txtItem_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItem.KeyUp
        If txtItem.Text.Trim() <> "" Then
            retrivePurchaseItems(" Where upper(ItemName) Like upper('" & txtItem.Text.Trim() & "%')")
        Else
            retrivePurchaseItems()
        End If

    End Sub

    Private Sub lblCrate_TextChanged(sender As Object, e As EventArgs) Handles lblCrate.TextChanged
        If lblCrate.Text = "N" Then pnlMarka.Visible = False
    End Sub
    Private Sub ChargesRowColums()
        dgCharges.ColumnCount = 3
        dgCharges.Columns(0).Name = "ID" : dgCharges.Columns(0).Visible = False
        dgCharges.Columns(1).Name = "Item Name" : dgCharges.Columns(1).Width = 130
        dgCharges.Columns(2).Name = "ApplyType" : dgCharges.Columns(2).Width = 150
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
        ChargesRowColums() : dgCharges.Visible = True
    End Sub

    Private Sub txtVehicleNo_Leave(sender As Object, e As EventArgs) Handles txtVehicleNo.Leave
        If txtVehicleNo.Text = "" Then
            MsgBox("Please Insert Vehicle Number", MsgBoxStyle.Critical, "Empty Vehicle Number")
            txtVehicleNo.Focus() : Exit Sub
        End If
    End Sub

    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.Visible = True
    End Sub

    Private Sub TxtPurchaseLot_Leave(sender As Object, e As EventArgs) Handles TxtPurchaseLot.Leave
        If BtnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr("Select Count(*) From Purchase Where AccountID=" & Val(txtAccountID.Text) & " and EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and ItemID=" & Val(txtItemID.Text) & " and LotNo='" & TxtPurchaseLot.Text & "'")) >= 1 Then
                MsgBox("This Lot Number For Today Already Exists in Transaction" & vbNewLine & "Please Change Lot Lot Number. ( Like 10A,10B,10C Etc )", MsgBoxStyle.Critical, "Change Lot Number") : TxtPurchaseLot.Focus() : Exit Sub
            End If
        Else
            If Val(clsFun.ExecScalarStr("Select Count(*) From Purchase Where AccountID=" & Val(txtAccountID.Text) & " and EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and ItemID=" & Val(txtItemID.Text) & " and LotNo='" & TxtPurchaseLot.Text & "'")) > 1 Then
                MsgBox("This Lot Number For Today Already Exists in Transaction" & vbNewLine & "Please Change Lot Lot Number. ( Like 10A,10B,10C Etc )", MsgBoxStyle.Critical, "Change Lot Number") : TxtPurchaseLot.Focus() : Exit Sub
            End If
        End If
    End Sub
    
    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbAccountName_KeyDown(sender As Object, e As KeyEventArgs) Handles cbAccountName.KeyDown
        If e.KeyCode = Keys.Enter Then cbCrateMarka.Focus()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Dim PrintPurchase As String = clsFun.ExecScalarStr("SELECT Print FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Purchase'")
        If PrintPurchase <> "Y" Then MsgBox("You Don't Have Rights to Print Purchase... " & vbNewLine & " Please Contact to Admin..Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        PrintOnly()
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

    Private Sub PrintRecord()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & txtVoucherNo.Text & "','" & mskEntryDate.Text & "','" & txtVehicleNo.Text & "','" & .Cells(4).Value & "'," &
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                    "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                    "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                    "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                    "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "','" & lblInword.Text & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into Printing(D1, D2,M1,M2,M3,M4, P1,P2, P3, P4, P5, P6,P7,P8,P9, " &
                    " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20,  P21,P22)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
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
    End Sub

    Private Sub CbpurchaseType_Leave(sender As Object, e As EventArgs) Handles CbpurchaseType.Leave
        ExpSettings()
    End Sub

    Private Sub CbpurchaseType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbpurchaseType.SelectedIndexChanged
        If CbpurchaseType.SelectedIndex = 0 Then
            txtRate.Text = "0.00"
            txtRate.Enabled = False
            txtRate.BackColor = Color.Gray
        Else
            txtRate.Enabled = True
            txtRate.BackColor = Color.GhostWhite
        End If
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        clsFun.FillDropDownList(cbCrateMarka, "Select * From CrateMarka", "MarkaName", "Id", "")
    End Sub

    Private Sub btnMultiUpdate_Click(sender As Object, e As EventArgs) Handles btnMultiUpdate.Click
        MultiUpdatePurchase()
    End Sub

    Private Sub txtInvoiceID_Leave(sender As Object, e As EventArgs) Handles txtInvoiceID.Leave
        txtVoucherNo.Text = txtVoucherNo.Text
        pnlInvoiceID.Visible = False
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        FillWithNevigation()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If Offset = 0 Then
            Offset = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Purchase'  Order By ID ")
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
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Vouchers WHERE transtype = 'Purchase'  Order By ID ")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        Offset = (TotalPages - 1) * RowCount
        FillWithNevigation()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dg3.ColumnCount = 0 Then SaleRowColums()
        cbBillingType.SelectedIndex = 0 : retriveSale()
        PnlCustomerBill.Visible = True : PnlCustomerBill.BringToFront()
    End Sub

    Private Sub btnPnlHide_Click(sender As Object, e As EventArgs) Handles btnPnlHide.Click
        PnlCustomerBill.Visible = False
    End Sub

    Private Sub dg3_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg3.CellContentClick

    End Sub

    Private Sub dg3_KeyDown(sender As Object, e As KeyEventArgs) Handles dg3.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cbBillingType.SelectedIndex <> 0 Then Exit Sub
            If dg3.SelectedRows.Count = 0 Then Exit Sub
            Dim id As Integer = dg3.SelectedRows(0).Cells(0).Value
            Dim type As String = dg3.SelectedRows(0).Cells(10).Value
            If type = "Stock Sale" Then
                Stock_Sale.MdiParent = MainScreenForm
                Stock_Sale.Show()
                Stock_Sale.FillControls(id)
                If Not Stock_Sale Is Nothing Then
                    Stock_Sale.BringToFront()
                End If
            ElseIf type = "On Sale" Then
                On_Sale.MdiParent = MainScreenForm
                On_Sale.Show()
                On_Sale.FillControl(id)
                If Not On_Sale Is Nothing Then
                    On_Sale.BringToFront()
                End If
            ElseIf type = "Standard Sale" Then
                Standard_Sale.MdiParent = MainScreenForm
                Standard_Sale.Show()
                Standard_Sale.FillControls(id)
                If Not Standard_Sale Is Nothing Then
                    Standard_Sale.BringToFront()
                End If
            End If
        End If
    End Sub

    Private Sub dg3_MouseClick(sender As Object, e As MouseEventArgs) Handles dg3.MouseClick
        dg3.ClearSelection()
    End Sub

    Private Sub dg3_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg3.MouseDoubleClick
        If dg3.SelectedRows.Count = 0 Then Exit Sub
        Dim id As Integer = dg3.SelectedRows(0).Cells(0).Value
        Dim type As String = dg3.SelectedRows(0).Cells(10).Value
        If type = "Stock Sale" Then
            Stock_Sale.MdiParent = MainScreenForm
            Stock_Sale.Show()
            Stock_Sale.FillControls(id)
            If Not Stock_Sale Is Nothing Then
                Stock_Sale.BringToFront()
            End If
        ElseIf type = "On Sale" Then
            On_Sale.MdiParent = MainScreenForm
            On_Sale.Show()
            On_Sale.FillControl(id)
            If Not On_Sale Is Nothing Then
                On_Sale.BringToFront()
            End If
        ElseIf type = "Standard Sale" Then
            Standard_Sale.MdiParent = MainScreenForm
            Standard_Sale.Show()
            Standard_Sale.FillControls(id)
            If Not Standard_Sale Is Nothing Then
                Standard_Sale.BringToFront()
            End If
        End If
    End Sub

    Private Sub txtPurchaseTotAmount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAmount.KeyUp
        If CbpurchaseType.SelectedIndex = 0 Then txtAmount.Text = "0.00" : Exit Sub
        If cbPer.SelectedIndex = 0 Then
            txtRate.Text = Val(txtAmount.Text) / Format(Val(txtNug.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 1 Then
            txtRate.Text = Val(txtAmount.Text) / Format(Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 2 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 5 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 3 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 10 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 4 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 20 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 5 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 40 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 6 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 41 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 7 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 50 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 8 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 51 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 9 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 51.7 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 10 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 52.3 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 11 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 53 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 12 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 80 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 13 Then
            txtRate.Text = Format(Val(txtAmount.Text) * 100 / Val(txtWeight.Text), "0.00")
        ElseIf cbPer.SelectedIndex = 14 Then
            txtRate.Text = Format(Val(txtNug.Text) * Val(txtAmount.Text), "0.00")
        End If
        txtRate.Text = Format(Math.Round(Val(txtRate.Text), 2), "0.00")
        If txtRate.Text = "NAN" Then txtRate.Text = ""
    End Sub



    Private Sub txtCrateQty_TextChanged(sender As Object, e As EventArgs) Handles txtCrateQty.TextChanged

    End Sub

    Private Sub reportDocument1_InitReport(sender As Object, e As EventArgs)

    End Sub

    Private Sub cbBillingType_KeyDown(sender As Object, e As KeyEventArgs) Handles cbBillingType.KeyDown
        If e.KeyCode = Keys.Enter Then
            retriveSale()
        End If
    End Sub

    Private Sub ckShowCustomer_CheckedChanged(sender As Object, e As EventArgs) Handles ckShowCustomer.CheckedChanged

    End Sub

    Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs) Handles txtAmount.TextChanged

    End Sub


    Private Sub ExpSettings()
        If BtnSave.Text <> "&Save" Then Exit Sub
        Dg2.Rows.Clear() : Dim dt As New DataTable
        Dim sql As String = String.Empty
        If CbpurchaseType.SelectedIndex = 1 Then
            sql = "Select * From ExpControl Where ApplyOn='Purchase (Self)'"
        Else
            sql = "Select * From ExpControl Where ApplyOn='Purchase (Stock In)'"
        End If
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

    Private Sub txtchargesAmount_TextChanged(sender As Object, e As EventArgs) Handles txtchargesAmount.TextChanged

    End Sub

    Private Sub txtNug_TextChanged(sender As Object, e As EventArgs) Handles txtNug.TextChanged

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
        '  cleartxt() : cleartxtCharges() : FootertextClear()
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
                StartBackgroundTask(AddressOf SendWhatsappData)
            Else
                MsgBox("Please Enter Valid Whatsapp Contact", MsgBoxStyle.Critical, "Invalid Contact") : txtWhatsappNo.Focus() : Exit Sub
            End If
        Else
            StartBackgroundTask(AddressOf WahSoft)
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

        'pnlWahtsappNo.Visible = True
        'txtWhatsappNo.Focus()
        GlobalData.PdfName = txtAccount.Text & "-" & mskEntryDate.Text & ".pdf"
        PrintOnly() : PrintRecord()
        Pdf_Genrate.ExportReport("\Purchase.rpt")
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        pnlWhatsapp.Visible = False
        AlltextClear()
        dg1.Rows.Clear() : Dg2.Rows.Clear() : dg3.Rows.Clear()
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
        Pdf_Genrate.ExportReport("\Purchase.rpt")
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If dg1.RowCount = 0 Then MsgBox("There is no record to Print...", vbOKOnly, "Empty") : Exit Sub
        PrintOnly() : PrintRecord()
        Report_Viewer.printReport("\Purchase.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If dg1.RowCount = 0 Then MsgBox("There is no record to Print...", vbOKOnly, "Empty") : Exit Sub
        PrintOnly() : PrintRecord()
        Report_Viewer.printReport("\Purchase.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
End Class