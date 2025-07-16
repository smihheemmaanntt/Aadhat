Public Class OnSaleReceipt_Net

    Private Sub OnSaleReceipt_Net_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub OnSaleReceipt_Net_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.KeyPreview = True : VNumber()
    End Sub

    Private Sub VNumber()
        Dim Vno As String = String.Empty
        Dim maxInvoiceId As String = Val(clsFun.ExecScalarStr("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'"))
        If Not String.IsNullOrEmpty(maxInvoiceId) AndAlso Val(Vno) = Val(maxInvoiceId) Then
            Vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = Vno + 1
            txtInvoiceID.Text = Vno + 1
        Else
            Vno = clsFun.ExecScalarInt("Select Max(InvoiceID)  AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtVoucherNo.Text = Vno + 1
            txtInvoiceID.Text = Vno + 1
        End If
    End Sub
    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.BringToFront()
        AccountRowColumns()
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        BtnSave.Text = "&Update"
        BtnDelete.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & Val(id)
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtAccountID.Text = Val(ds.Tables("a").Rows(0)("AccountID").ToString())
            clsFun.FillDropDownList(CbChallanNo, "Select ItemID, ItemName From Vouchers Where AccountID='" & Val(txtAccountID.Text) & "' and TransType='Net Receipt'", "ItemName", "ItemID", "")
            txtAccount.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            CbChallanNo.SelectedValue = Val(ds.Tables("a").Rows(0)("ItemID").ToString())
            CbChallanNo.Text = Val(ds.Tables("a").Rows(0)("ItemName").ToString())
            txtTotalAmount.Text = Format(Val(ds.Tables("a").Rows(0)("BasicAmount").ToString()), "0.00")
            txtTotalGrand.Text = Format(Val(ds.Tables("a").Rows(0)("TotalAmount").ToString()), "0.00")
            txtTotalExp.Text = Format(Val(ds.Tables("a").Rows(0)("TotalCharges").ToString()), "0.00")
            txtFreight.Text = Format(Val(ds.Tables("a").Rows(0)("N1").ToString()), "0.00")
            txtLabour.Text = Format(Val(ds.Tables("a").Rows(0)("N2").ToString()), "0.00")
            txtOtherCharges.Text = Format(Val(ds.Tables("a").Rows(0)("N3").ToString()), "0.00")
            txtTransferOn.Text = ds.Tables("a").Rows(0)("T1").ToString()
            txtChallanNo.Text = ds.Tables("a").Rows(0)("T2").ToString()
            txtVehicleNo.Text = ds.Tables("a").Rows(0)("T3").ToString()
            txtOurCost.Text = ds.Tables("a").Rows(0)("T4").ToString()
            txtNarration.Text = ds.Tables("a").Rows(0)("Remark").ToString()
        End If
        CbChallanNo.Enabled = False
    End Sub

    Private Sub Save()
        Dim Sql As String = String.Empty
        Sql = "Insert Into Vouchers (TransType,BillNo,EntryDate,AccountID,AccountName,BasicAmount,TotalCharges,TotalAmount,N1,N2,N3,ItemID,ItemName,T1,T2,T3,T4,remark,InvoiceID) " & _
            "values ( '" & Me.Text & "','" & txtVoucherNo.Text & "','" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'," & _
            "'" & Val(txtAccountID.Text) & "','" & txtAccount.Text & "','" & Val(txtTotalAmount.Text) & "'," & _
            "'" & Val(txtTotalExp.Text) & "','" & Val(txtTotalGrand.Text) & "','" & Val(txtFreight.Text) & "','" & Val(txtLabour.Text) & "', " & _
            "'" & Val(txtOtherCharges.Text) & "'," & (CbChallanNo.SelectedValue) & ",'" & CbChallanNo.Text & "','" & txtTransferOn.Text & "','" & CbChallanNo.Text & "'," & _
            "'" & txtVehicleNo.Text & "','" & Val(txtOurCost.Text) & "','" & txtNarration.Text & "','" & Val(txtInvoiceID.Text) & "')"
        If clsFun.ExecNonQuery(Sql) > 0 Then
            txtid.Text = Val(clsFun.ExecScalarStr("Select max(ID) From Vouchers"))
            clsFun.ExecNonQuery("Update Vouchers Set ItemID='" & (txtid.Text) & "' Where ID =" & (CbChallanNo.SelectedValue) & ";")
            InsertLedger() : MsgBox("Receipt Net Saved Successfully.", MsgBoxStyle.Information, "Saved") : TxtClearAll()
        End If
    End Sub

    Private Sub UpdateRecord()
        Dim Sql As String = String.Empty
        Sql = "Update Vouchers Set TransType='" & Me.Text & "',BillNo='" & txtVoucherNo.Text & "',EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "',AccountID='" & Val(txtAccountID.Text) & "',AccountName='" & txtAccount.Text & "', " &
            "BasicAmount='" & Val(txtTotalAmount.Text) & "',TotalCharges='" & Val(txtTotalExp.Text) & "',TotalAmount='" & Val(txtTotalGrand.Text) & "', " &
            "N1='" & Val(txtFreight.Text) & "',N2='" & Val(txtLabour.Text) & "',N3='" & Val(txtOtherCharges.Text) & "',ItemID=" & Val(CbChallanNo.SelectedValue) & ",ItemName='" & CbChallanNo.Text & "', " &
            "T1='" & txtTransferOn.Text & "',T2='" & txtChallanNo.Text & "',T3='" & txtVehicleNo.Text & "',T4='" & Val(txtOurCost.Text) & "',Remark='" & txtNarration.Text & "', " &
            "InvoiceID='" & Val(txtInvoiceID.Text) & "' Where ID=" & Val(txtid.Text) & " "
        If clsFun.ExecNonQuery(Sql) > 0 Then
            '    txtid.Text = Val(clsFun.ExecScalarStr("Select max(ID) From Vouchers"))
            clsFun.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtid.Text) & "")
            clsFun.ExecNonQuery("Update Vouchers Set ItemID='" & (txtid.Text) & "' Where ID =" & (CbChallanNo.SelectedValue) & ";")
            InsertLedger()
            MsgBox("Receipt Net Updated Successfully.", MsgBoxStyle.Information, "Update")
            TxtClearAll()
        End If
    End Sub
    Private Sub TxtClearAll()
        txtAccount.Clear() : CbChallanNo.Text = ""
        txtTotalAmount.Clear() : txtFreight.Clear()
        txtLabour.Clear() : txtOtherCharges.Clear()
        txtTotalExp.Clear() : txtTotalGrand.Clear()
        txtNarration.Clear() : txtChallanNo.Clear()
        txtVoucherNo.Clear() : txtGrNo.Clear()
        txtTransferOn.Clear() : txtVehicleNo.Clear()
        txtOurCost.Clear() : CbChallanNo.Enabled = True
        txtOurCost.Clear() : BtnSave.Text = "&Save"
        VNumber() : mskEntryDate.Focus()
    End Sub
    Private Sub InsertLedger()
        Dim FastQuery As String = String.Empty
        If Val(txtTotalAmount.Text) <> 0 Then ''Manual Beejak Account Fixed
            '    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, 24, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=24"), Val(tmpamount2), "C", remark, txtAccount.Text, remarkHindi)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(24) & ", '" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=24") & "','" & Val(txtTotalGrand.Text) & "', 'C','" & txtNarration.Text & "','" & txtAccount.Text & "','" & txtNarration.Text & "'"
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ", '" & txtAccount.Text & "','" & Val(txtTotalGrand.Text) & "', 'D','" & txtNarration.Text & "','" & txtAccount.Text & "','" & txtNarration.Text & "'"
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub

    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 180
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 150
        DgAccountSearch.Visible = True : DgAccountSearch.BringToFront()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        DgAccountSearch.Rows.Clear() : Dim dt As New DataTable
        If BtnSave.Text = "&Save" Then
            dt = clsFun.ExecDataTable("Select AccountID,AccountName from Vouchers  where Transtype='On Sale' And  ifnull(ItemID,0)= 0  " & condtion & "  Group by AccountID  order by AccountName")
        Else
            dt = clsFun.ExecDataTable("Select AccountID,AccountName from Vouchers  where Transtype='On Sale' And  ifnull(ItemID,0)<>0 " & condtion & "  Group by AccountID order by AccountName")
        End If

        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        '   .Cells(2).Value = dt.Rows(i)("City").ToString()
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
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub CbChallanNo_GotFocus(sender As Object, e As EventArgs) Handles CbChallanNo.GotFocus
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then txtAccount.Focus() : Exit Sub
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        If BtnSave.Text = "&Save" Then
            'If CbChallanNo.Text <> "" Then Exit Sub
            clsFun.FillDropDownList(CbChallanNo, "Select ID, BillNo From Vouchers Where AccountID='" & Val(txtAccountID.Text) & "' and ifnull(ItemID,0)=0 and TransType='On Sale'", "BillNo", "ID", "")
        Else
            ' If CbChallanNo.Text <> "" Then Exit Sub
            clsFun.FillDropDownList(CbChallanNo, "Select ID, BillNo From Vouchers Where AccountID='" & Val(txtAccountID.Text) & "' and ifnull(ItemID,0)<>0 and ID='" & Val(txtid.Text) & "' and TransType='On Sale'", "BillNo", "ID", "")
        End If
    End Sub
    Private Sub Calculation()
        txtTotalExp.Text = Format(Val(txtLabour.Text) + Val(txtFreight.Text) + Val(txtOtherCharges.Text), "0.00")
        txtTotalGrand.Text = Format(Val(txtTotalAmount.Text) - Val(txtTotalExp.Text), "0.00")
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus
        mskEntryDate.SelectAll()
        mskEntryDate.BackColor = Color.LightCyan
    End Sub
    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown, txtAccount.KeyDown,
        CbChallanNo.KeyDown, txtTotalAmount.KeyDown, txtFreight.KeyDown, txtLabour.KeyDown, txtOtherCharges.KeyDown, txtNarration.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then
                DgAccountSearch.Focus()
            End If
        End If
    End Sub

    Private Sub mskEntryDate_Leave(sender As Object, e As EventArgs) Handles mskEntryDate.Leave
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub
    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub txtVoucherNo_GotFocus(sender As Object, e As EventArgs) Handles txtVoucherNo.GotFocus, txtAccount.GotFocus,
    txtTotalAmount.GotFocus, txtFreight.GotFocus, txtLabour.GotFocus, txtOtherCharges.GotFocus, txtNarration.GotFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightCyan
        tb.SelectAll()
    End Sub
    Private Sub txtVoucherNo_Leave(sender As Object, e As EventArgs) Handles txtVoucherNo.Leave, txtAccount.Leave,
     txtTotalAmount.Leave, txtFreight.Leave, txtLabour.Leave, txtOtherCharges.Leave, txtNarration.Leave
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.GhostWhite
    End Sub
    Private Sub txtTotalAmount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtTotalAmount.KeyUp, txtFreight.KeyUp, txtLabour.KeyUp, txtOtherCharges.KeyUp
        Calculation()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If BtnSave.Text = "&Save" Then
            Save()
        Else
            UpdateRecord()
        End If
    End Sub

    Private Sub CbChallanNo_Leave(sender As Object, e As EventArgs) Handles CbChallanNo.Leave
        RetriveChallan()
    End Sub
    Private Sub RetriveChallan()
        Dim dt As New DataTable
        Dim i As Integer = 0
        If BtnSave.Text = "&Save" Then
            dt = clsFun.ExecDataTable("Select * FROM Vouchers Where  ID=" & Val(CbChallanNo.SelectedValue) & " order by  InvoiceID,EntryDate ")
        Else
            dt = clsFun.ExecDataTable("Select * FROM Vouchers Where  ID=" & Val(CbChallanNo.SelectedValue) & " order by  InvoiceID,EntryDate ")
        End If
        
        Try
            If dt.Rows.Count > 0 Then
                txtTransferOn.Text = Format(dt.Rows(0)("EntryDate"), "dd-MM-yyyy")
                txtChallanNo.Text = dt.Rows(0)("BillNo").ToString()
                txtVehicleNo.Text = dt.Rows(0)("VehicleNo").ToString()
                txtOurCost.Text = Format(Val(dt.Rows(0)("BasicAmount").ToString()), "0.00")
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        ' calc() : dg1.ClearSelection()
    End Sub
    Private Sub CbChallanNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbChallanNo.SelectedIndexChanged

    End Sub

    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False
            CbChallanNo.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub
    Private Sub Delete()
        Try
            If MessageBox.Show("Are you Sure Want to Delete Net Receipt?", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
                If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & "") > 0 Then
                    clsFun.ExecNonQuery("DELETE from Ledger WHERE VourchersID=" & Val(txtid.Text) & ";Update Vouchers Set ItemID='" & (0) & "' Where ID =" & (CbChallanNo.SelectedValue) & ";")
                    MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                    BtnDelete.Visible = False : BtnSave.Text = "&Save" : TxtClearAll()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class