Public Class SellOUT_Value

    Private Sub Supplier_Statement_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Supplier_Statement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums()
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
    End Sub
    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown, mskFromDate.KeyDown, MsktoDate.KeyDown
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 8
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date"
        dg1.Columns(1).Width = 130
        dg1.Columns(2).Name = "Voucher No."
        dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Vehicle No."
        dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Suppler Name"
        dg1.Columns(4).Width = 300
        dg1.Columns(5).Name = "Nugs"
        dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Weight"
        dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "SellOut Amount"
        dg1.Columns(7).Width = 200
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 2
        DgAccountSearch.Columns(0).Name = "AccountID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 440
        retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select AccountID,AccountName From Purchase Where PurchaseTypeName='Stock In' and AccountID Not in(7) " & condtion & " Group By AccountName order by AccountName")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
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
    Private Sub txtCustomer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.BringToFront()
        AccountRowColumns()
        DgAccountSearch.Visible = True
    End Sub
    Private Sub txtCustomer_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then txtAccount.Focus() : Exit Sub
        txtAccountID.text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        txtAccountID.text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : DateValidation()
        mskFromDate.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            If DgAccountSearch.RowCount = 0 Then Exit Sub
            txtAccountID.text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False : DateValidation()
            mskFromDate.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub DateValidation()
        'If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & txtAccount.Text & "'") = 0 Then
        '    MsgBox("Account Name Not Found in Database...", vbOKOnly, "Access Denied")
        '    clsFun.FillDropDownList(cbAccountName, "Select * From Account_AcGrp", "AccountName", "Id", "")
        '    cbAccountName.Focus()
        '    Exit Sub
        'End If
        Dim mindate As String : Dim maxDate As String
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) From Ledger Where AccountID=" & Val(txtAccountID.Text) & "")
        maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger Where AccountID=" & Val(txtAccountID.Text) & "")
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
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()
    End Sub
    Private Sub Retrive()
        Application.DoEvents()
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim sql As String = String.Empty
        sql = " Select ID,EntryDate,SallerName,BillNo,VehicleNo,Nug,Kg,TotalAmount From Vouchers  WHERE Transtype='Auto Beejak'  " & _
            " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & _
            " and SallerID =" & Val(txtAccountID.Text) & " "
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).tostring("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                        .Cells(4).Value = dt.Rows(i)("SallerName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("kg").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    End With
                Next
            End If
        Catch ex As Exception

        End Try
        dt = clsFun.ExecDataTable(sql)
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub calc()
        dg1.ClearSelection()
        txtBalAmt.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtBalAmt.Text = Format(Val(txtBalAmt.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
        Next
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

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click

    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
        Sellout_Auto.MdiParent = MainScreenForm
        Sellout_Auto.Show()
        Sellout_Auto.FillFromData(tmpID)
        If Not Sellout_Auto Is Nothing Then
            Sellout_Auto.BringToFront()
        End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
            Sellout_Auto.MdiParent = MainScreenForm
            Sellout_Auto.Show()
            Sellout_Auto.FillFromData(tmpID)
            If Not Sellout_Auto Is Nothing Then
                Sellout_Auto.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub
End Class