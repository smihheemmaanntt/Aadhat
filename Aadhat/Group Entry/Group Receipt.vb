Public Class Group_Receipt
    Dim vno As Integer : Dim ServerTag As Integer
    Private Sub PayMentform_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then
                DgAccountSearch.Visible = False
                Exit Sub
            ElseIf dgMode.Visible = True Then
                dgMode.Visible = False
                Exit Sub
            Else
                Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "AADHAT")
                DgAccountSearch.Visible = False : dgMode.Visible = False
                If msgRslt = MsgBoxResult.Yes Then
                    Me.Close()
                ElseIf msgRslt = MsgBoxResult.No Then
                End If
            End If
        End If
    End Sub


    Private Sub PayMentform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums() : VNumber() : FillPayment()
    End Sub
    Public Sub FillPayment()
        'Dim ssql As String = "Select * from Controls "
        'Dim dt As DataTable
        'dt = clsFun.ExecDataTable(ssql)
        '    If dt.Rows.Count > 0 Then
        'If dt.Rows(0)("PayDate").ToString().Trim() = "Y" Then mskEntryDate.TabStop = False Else mskEntryDate.TabStop = True
        'If dt.Rows(0)("PayNo").ToString().Trim() = "Y" Then txtReciptNo.TabStop = False Else txtReciptNo.TabStop = True
        'If dt.Rows(0)("PayRemark").ToString().Trim() = "Y" Then TxtRemark.TabStop = False Else TxtRemark.TabStop = True
        'End If
    End Sub
    Private Sub AcBal()
        '  Dim i, j As Integer
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
            tmpamtdr = Format(Val(opbal) + Val(tmpamtdr), "0.00")
        Else
            tmpamtcr = Format(Val(opbal) + Val(tmpamtcr), "0.00")
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Cr"
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
        lblAcBal.Text = "Bal : " & opbal
    End Sub
    Private Sub CapAcBal()
        ' Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtModeID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtModeID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtModeID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtModeID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Format(Val(opbal) + Val(tmpamtdr), "0.00")
        Else
            tmpamtcr = Format(Val(opbal) + Val(tmpamtcr), "0.00")
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Cr"
        Else
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtModeID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id=" & Val(txtModeID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Cr"
            Else
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Dr"
            End If
        End If
        lblCapAcBal.Visible = True
        lblCapAcBal.Text = "Bal : " & opbal
    End Sub
    Private Sub ModeColums()
        dgMode.ColumnCount = 2
        dgMode.Columns(0).Name = "ID" : dgMode.Columns(0).Visible = False
        dgMode.Columns(1).Name = "Mode Name" : dgMode.Columns(1).Width = 408
        RetriveMode()
    End Sub
    Private Sub RetriveMode(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Accounts where groupid in(11,12) " & condtion & " order by AccountName")
        Try
            If dt.Rows.Count > 0 Then
                dgMode.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgMode.Rows.Add()
                    With dgMode.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                    End With
                Next

            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Teller")
        End Try
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 182
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 150
        retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select ID,AccountName,City From Accounts where not groupid in(11,12) " & condtion & " order by  AccountName Limit 10")
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
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Teller")
        End Try
    End Sub
    Private Sub dgMode_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgMode.CellClick
        If dgMode.SelectedRows.Count = 0 Then Exit Sub
        txtMode.Clear() : txtModeID.Clear()
        txtModeID.Text = Val(dgMode.SelectedRows(0).Cells(0).Value)
        txtMode.Text = dgMode.SelectedRows(0).Cells(1).Value
        dgMode.Visible = False
        txtAccount.Focus()
    End Sub

    Private Sub dgMode_KeyDown(sender As Object, e As KeyEventArgs) Handles dgMode.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dgMode.SelectedRows.Count = 0 Then Exit Sub
            txtMode.Clear() : txtModeID.Clear()
            txtModeID.Text = Val(dgMode.SelectedRows(0).Cells(0).Value)
            txtMode.Text = dgMode.SelectedRows(0).Cells(1).Value
            dgMode.Visible = False
            txtAccount.Focus()
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
            If dgMode.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = dgMode.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Back Then txtMode.Focus()
    End Sub
    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        If dgMode.ColumnCount = 0 Then ModeColums()
        If dgMode.RowCount = 0 Then RetriveMode()
        If dgMode.SelectedRows.Count = 0 Then dgMode.Visible = True
        If txtMode.Text.Trim() <> "" Then
            dgMode.Visible = True
            RetriveMode(" And upper(AccountName) Like upper('" & txtMode.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        txtModeID.Text = dgMode.SelectedRows(0).Cells(0).Value
        txtMode.Text = dgMode.SelectedRows(0).Cells(1).Value
        dgMode.Visible = False : CapAcBal()
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        '   txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        '  txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
    End Sub
    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup", "GroupName", "ID", "")
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
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
        txtAccount.Clear() : txtAccountID.Clear()
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : txtReciveAmount.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup", "GroupName", "ID", "")
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
            If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False : txtReciveAmount.Focus()

            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub VNumber()
        If vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
            vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtReciptNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        Else
            vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtReciptNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        End If
    End Sub

    Private Sub txtclear()
        BtnDelete.Visible = False
        VNumber()
        txtReciveAmount.Text = ""
        btnSave.Text = "&Save"
        TxtRemark.Text = ""
        If mskEntryDate.TabStop = True Then mskEntryDate.Focus() Else txtMode.Focus()
        btnSave.BackColor = Color.DarkSlateGray
        btnSave.Image = My.Resources.icons8_save_48px
        MainScreenPicture.retrive2()
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 4
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name" : dg1.Columns(1).Width = 332
        dg1.Columns(2).Name = "Amount" : dg1.Columns(2).Width = 175
        dg1.Columns(3).Name = "Remark" : dg1.Columns(3).Width = 662
    End Sub
    Private Sub calc()
        dg1.ClearSelection()
        txtTotal.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtTotal.Text = Format(Val(txtTotal.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
        Next
    End Sub

    Public Sub FillControls(ByVal id As Integer)
        Dim ssql As String = String.Empty
        Dim primary As String = String.Empty
        BtnDelete.Visible = True
        Dim dt As New DataTable
        btnSave.BackColor = Color.Coral
        btnSave.Image = My.Resources.icons8_edit_48px
        btnSave.Text = "&Update"
        ssql = "Select * from Vouchers where id=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            mskEntryDate.Text = Format(dt.Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtModeID.Text = dt.Rows(0)("SallerID").ToString()
            txtMode.Text = dt.Rows(0)("Sallername").ToString()
            txtTotal.Text = Format(Val(dt.Rows(0)("TotalAmount").ToString()), "0.00")
            txtNaration.Text = dt.Rows(0)("Remark").ToString()
            txtReciptNo.Text = dt.Rows(0)("BillNo").ToString()
            txtID.Text = dt.Rows(0)("ID").ToString()
            txtInvoiceID.Text = dt.Rows(0)("InvoiceID").ToString()
        End If
        Dim sql As String = "Select * from Transaction3 Where VoucherID=" & Val(txtID.Text)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad1.Fill(ds, "b")
        If ds.Tables("b").Rows.Count > 0 Then
            dg1.Rows.Clear()
            'With dg1
            ' If ds.Tables("b").Rows.Count > 9 Then dg1.Columns(10).Width = 109 Else dg1.Columns(10).Width = 129
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                dg1.Rows.Add()
                With dg1.Rows(i)
                    .Cells(0).Value = ds.Tables("b").Rows(i)("AccountID").ToString()
                    .Cells(1).Value = ds.Tables("b").Rows(i)("AccountName").ToString()
                    .Cells(2).Value = ds.Tables("b").Rows(i)("Amount").ToString()
                    .Cells(3).Value = ds.Tables("b").Rows(i)("Remark").ToString()
                End With
            Next

        End If
        dg1.ClearSelection()
    End Sub
    Private Sub UpdateRecord()
        ' Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "Update Vouchers Set EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "', TransType='" & Me.Text & "', " &
            " SallerID='" & Val(txtModeID.Text) & "',sallerName='" & txtMode.Text & "',TotalAmount='" & Val(txtTotal.Text) & "',billNo='" & txtReciptNo.Text & "', " &
            " InvoiceID='" & Val(txtInvoiceID.Text) & "',Remark='" & TxtRemark.Text & "' Where ID=" & Val(txtID.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                clsFun.CloseConnection()
                clsFun.ExecNonQuery("DELETE from Transaction3 WHERE VoucherID=" & Val(txtID.Text) & "; " &
                                    "DELETE from Ledger WHERE VourchersID=" & Val(txtID.Text) & ";")
                ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & ";")
                dg1Record() : Ledger() : ServerTag = 1 : ServerLedger()
                MessageBox.Show("Record Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearControls()
            End If
        Catch ex As Exception

        End Try

    End Sub
    Private Sub save()
        Dim SqliteEntryDate As String = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim cmd As New SQLite.SQLiteCommand
        If txtMode.Text = "" Then
            MsgBox("Please Fill Mode Name... ", MsgBoxStyle.Exclamation, "Empty")
            MsgBox("Please Fill Amount... ", MsgBoxStyle.Exclamation, "Empty")
            txtMode.Focus() : Exit Sub
        End If
        Dim sql As String = "insert into Vouchers (EntryDate,TransType,SallerID,sallerName,TotalAmount,billNo,InvoiceID,Remark) values (@1, @2, @3,@4,@5,@6,@7,@8)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", SqliteEntryDate)
            cmd.Parameters.AddWithValue("@2", Me.Text)
            cmd.Parameters.AddWithValue("@3", Val(txtModeID.Text))
            cmd.Parameters.AddWithValue("@4", txtMode.Text)
            cmd.Parameters.AddWithValue("@5", Val(txtTotal.Text))
            cmd.Parameters.AddWithValue("@6", txtReciptNo.Text)
            cmd.Parameters.AddWithValue("@7", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@8", txtNaration.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                txtID.Text = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
                dg1Record() : Ledger() : ServerTag = 1 : ServerLedger()
                MessageBox.Show("Record Inserted Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearControls()
            End If
            clsFun.CloseConnection()

            'If clsFun.ExecScalarStr("Select RecPrint from Option5 ") = "A" Then
            '    Dim res = MessageBox.Show("Do you want to Print Slip", "Print", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            '    If res = Windows.Forms.DialogResult.Yes Then PrintRecord()
            'ElseIf clsFun.ExecScalarStr("Select RecPrint from Option5 ") = "D" Then
            '    PrintRecord()
            'End If
            txtclear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub Ledger()
        Dim FastQuery As String = String.Empty
        Dim Remark2 As String = String.Empty
        Dim RemarkHindi As String = String.Empty
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                Remark2 = "Receipt No. : " & txtReciptNo.Text & ", Paid Amt : " & Val(.Cells(2).Value) & ", Total Amt " & Val(.Cells(2).Value) & " "
                RemarkHindi = "रसीद नं. : " & txtReciptNo.Text & ", प्राप्त राशि : " & Val(.Cells(2).Value) & ", कुल राशि : " & Val(.Cells(2).Value) & " "
                If Val(.Cells(2).Value) > 0 Then ''Party Account
                    'clsFun.Ledger(0, Val(txtID.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(.Cells(0).Value), .Cells(1).Value, Val(.Cells(2).Value), "C", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), .Cells(1).Value, RemarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(0).Value) & ",'" & .Cells(1).Value & "'," & Val(.Cells(2).Value) & ",'C' ,'" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & .Cells(1).Value & "','" & RemarkHindi & "'"
                End If
            End With
        Next
        Remark2 = "Receipt No. : " & txtReciptNo.Text & ", Paid Amt : " & Val(txtTotal.Text) & ", Total Amt " & Val(txtTotal.Text) & " "
        RemarkHindi = "रसीद नं. : " & txtReciptNo.Text & ", प्राप्त राशि : " & Val(txtTotal.Text) & ", कुल राशि : " & Val(txtTotal.Text) & " "
        '   clsFun.Ledger(0, Val(txtID.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtModeID.Text), txtMode.Text, Val(txtTotal.Text), "D", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtMode.Text, RemarkHindi)
        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtModeID.Text) & ",'" & txtMode.Text & "'," & Val(txtTotal.Text) & ",'D' ,'" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtMode.Text & "','" & RemarkHindi & "'"
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub

    Private Sub ServerLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim Remark2 As String = String.Empty
        Dim RemarkHindi As String = String.Empty
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                Remark2 = "Receipt No. : " & txtReciptNo.Text & ", Paid Amt : " & Val(.Cells(2).Value) & ", Total Amt " & Val(.Cells(2).Value) & " "
                RemarkHindi = "रसीद नं. : " & txtReciptNo.Text & ", प्राप्त राशि : " & Val(.Cells(2).Value) & ", कुल राशि : " & Val(.Cells(2).Value) & " "
                If Val(.Cells(2).Value) > 0 Then ''Party Account
                    'clsFun.Ledger(0, Val(txtID.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(.Cells(0).Value), .Cells(1).Value, Val(.Cells(2).Value), "C", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), .Cells(1).Value, RemarkHindi)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(.Cells(0).Value) & ",'" & .Cells(1).Value & "'," & Val(.Cells(2).Value) & ",'C'," & (ServerTag) & "," & (OrgID) & ",'" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & .Cells(1).Value & "','" & RemarkHindi & "'"
                End If
            End With
        Next
        Remark2 = "Receipt No. : " & txtReciptNo.Text & ", Paid Amt : " & Val(txtTotal.Text) & ", Total Amt " & Val(txtTotal.Text) & " "
        RemarkHindi = "रसीद नं. : " & txtReciptNo.Text & ", प्राप्त राशि : " & Val(txtTotal.Text) & ", कुल राशि : " & Val(txtTotal.Text) & " "
        '   clsFun.Ledger(0, Val(txtID.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtModeID.Text), txtMode.Text, Val(txtTotal.Text), "D", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtMode.Text, RemarkHindi)
        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtModeID.Text) & ",'" & txtMode.Text & "'," & Val(txtTotal.Text) & ",'D'," & (ServerTag) & "," & (OrgID) & ",'" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtMode.Text & "','" & RemarkHindi & "'"
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub
    Private Sub dg1Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        ' Dim cmd As SQLite.SQLiteCommand
        For i As Integer = 0 To dg1.Rows.Count - 1
            With dg1.Rows(i)
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & Me.Text & "','" & Val(.Cells(0).Value) & "'," &
                "'" & .Cells(1).Value & "','" & Val(.Cells(2).Value) & "','" & .Cells(3).Value & "'"
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        Sql = "insert into Transaction3(VoucherID, TransType ,AccountID, AccountName, Amount, Remark) " & FastQuery & ""
        clsFun.ExecNonQuery(Sql)

    End Sub
    Private Sub printPR()
        PrintRecord()
        Report_Viewer.printReport("\Trans.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        sql = "insert into printing (D1,M1,M2,M3,P1,P2,P3,P4,P5,P6) values (@1, @2, @3,@4,@5,@6,@7,@8)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            cmd.Parameters.AddWithValue("@1", mskEntryDate.Text)
            cmd.Parameters.AddWithValue("@2", Me.Text)
            cmd.Parameters.AddWithValue("@3", txtMode.Text)
            cmd.Parameters.AddWithValue("@4", txtAccount.Text)
            cmd.Parameters.AddWithValue("@5", txtReciveAmount.Text)
            cmd.Parameters.AddWithValue("@6", TxtRemark.Text)
            cmd.Parameters.AddWithValue("@7", txtReciptNo.Text)
            cmd.Parameters.AddWithValue("@8", lblInword.Text)
            If cmd.ExecuteNonQuery() > 0 Then
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub ClearControls()
        dg1.Rows.Clear() : mskEntryDate.Focus() : calc() : VNumber()
        btnSave.Text = "&Save" : btnSave.BackColor = Color.SeaGreen : txtID.Text = 0
        BtnDelete.Visible = False
    End Sub
    Private Sub Delete()
        If txtID.Text = Val(0) Then Exit Sub
        Try
            If MessageBox.Show("Are You Sure Want to Delete Group Payment Entry ?", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                If clsFun.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & Val(txtID.Text) & ";DELETE from Vouchers WHERE ID=" & Val(txtID.Text) & ";DELETE from Transaction3 WHERE VoucherID=" & Val(txtID.Text) & "") > 0 Then
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & ";")
                    ServerTag = 0 : ServerLedger()
                    MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                    ClearControls()
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txtMode_GotFocus(sender As Object, e As EventArgs) Handles txtMode.GotFocus
        If dgMode.ColumnCount = 0 Then ModeColums()
        If dgMode.RowCount = 0 Then RetriveMode()
        If txtMode.Text.Trim() <> "" Then
            dgMode.Visible = True
            RetriveMode(" And upper(AccountName) Like upper('" & txtMode.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        If dgMode.SelectedRows.Count = 0 Then dgMode.Visible = True
    End Sub
    Private Sub txtInvoiceID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceID.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlInvoiceID.Visible = False
            txtReciptNo.Focus()
        End If
    End Sub

    Private Sub txtReciveAmount_GotFocus(sender As Object, e As EventArgs) Handles txtReciveAmount.GotFocus

        'If txtReciptNo.TabStop = True Then Exit Sub
        '  If txtReciptNo.TabStop = False Then VNumber()
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : AcBal()
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.SelectAll()
    End Sub
    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtMode.KeyDown, txtAccount.KeyDown,
        txtReciptNo.KeyDown, txtReciveAmount.KeyDown, TxtRemark.KeyDown
        If txtReciptNo.Focused Then
            If e.KeyCode = Keys.F2 Then
                pnlInvoiceID.Visible = True
                pnlInvoiceID.Focus()
            End If
        End If
        If txtMode.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=12 ", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.F1 Then
                Dim tmpID As String = txtModeID.Text
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                CreateAccount.FillContros(tmpID)
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.F1 Then
                Dim tmpID As String = txtAccountID.Text
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                CreateAccount.FillContros(tmpID)
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnSave.Focus()
        End Select
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count <> 0 Then
                If MessageBox.Show("Are you Sure to delete Entry", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                    dg1.Rows.Remove(dg1.SelectedRows(0))
                    txtAccount.Focus() : calc()
                End If
            End If
        End If

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        'If Val(txtModeID.Text) = 0 Then txtMode.Focus() : Exit Sub
        'If Val(txtAccountID.Text) = 0 Then txtAccount.Focus() : Exit Sub
        If btnSave.Text = "&Save" Then
            Dim AddVoucher As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Voucher'")
            If AddVoucher <> "Y" Then MsgBox("You Don't Have Rights to  Save Voucher... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            save()
        Else
            Dim ModifyVoucher As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Voucher'")
            If ModifyVoucher <> "Y" Then MsgBox("You Don't Have Rights to  Modify Voucher... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            UpdateRecord()
        End If
        MainScreenPicture.lblPaymentAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Payment' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        MainScreenPicture.lblPayment.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Payment' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
    End Sub
    Private Sub txtReciveAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtReciveAmount.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        txtAccountID.Text = dg1.SelectedRows(0).Cells(0).Value
        txtAccount.Text = dg1.SelectedRows(0).Cells(1).Value
        txtReciveAmount.Text = Format(Val(dg1.SelectedRows(0).Cells(2).Value), "0.00")
        TxtRemark.Text = dg1.SelectedRows(0).Cells(3).Value
        txtAccount.Focus()

    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            txtAccountID.Text = dg1.SelectedRows(0).Cells(0).Value
            txtAccount.Text = dg1.SelectedRows(0).Cells(1).Value
            txtReciveAmount.Text = Format(Val(dg1.SelectedRows(0).Cells(2).Value), "0.00")
            TxtRemark.Text = dg1.SelectedRows(0).Cells(3).Value
            txtAccount.Focus()
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then
            Exit Sub
        End If
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        '   FillControls(dg1.SelectedRows(0).Cells(0).Value)
    End Sub

    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
        '   If btnSave.Text = "&Save" Then VNumber()
    End Sub

    Private Sub txtMode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMode.KeyDown
        If e.KeyCode = Keys.Down Then dgMode.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=12", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If dgMode.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = dgMode.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If

    End Sub

    Private Sub txtMode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMode.KeyPress
        dgMode.Visible = True
    End Sub

    Private Sub txtMode_KeyUp(sender As Object, e As KeyEventArgs) Handles txtMode.KeyUp
        If dgMode.ColumnCount = 0 Then ModeColums()
        If dgMode.RowCount = 0 Then RetriveMode()
        If txtMode.Text.Trim() <> "" Then
            RetriveMode(" And upper(AccountName) Like upper('" & txtMode.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        If dgMode.SelectedRows.Count = 0 Then dgMode.Visible = True
    End Sub


    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        If e.KeyCode = Keys.Escape Then DgAccountSearch.Visible = False
    End Sub

    Private Sub txtReciptNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtReciptNo.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub
    Private Sub txtReciptNo_Leave(sender As Object, e As EventArgs) Handles txtReciptNo.Leave
        If btnSave.Text = "&Save" Then
            If clsFun.ExecScalarStr("Select count(*) from Vouchers where TransType='" & Me.Text & "' and  BillNo='" & Val(txtReciptNo.Text) & "'") >= 1 Then
                MsgBox("Receipt Already Exists...", vbOKOnly, "Access Denied")
                txtReciptNo.Focus() : txtReciptNo.Text = Val(txtReciptNo.Text) + 1
            End If
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If dg1.SelectedRows.Count = 1 Then
            dg1.SelectedRows(0).Cells(0).Value = Val(txtAccountID.Text)
            dg1.SelectedRows(0).Cells(1).Value = txtAccount.Text
            dg1.SelectedRows(0).Cells(2).Value = Format(Val(txtReciveAmount.Text), "0.00")
            dg1.SelectedRows(0).Cells(3).Value = TxtRemark.Text
            txtAccount.Focus() : calc() : txtReciveAmount.Clear() : TxtRemark.Clear()
        Else
            dg1.Rows.Add(Val(txtAccountID.Text), txtAccount.Text, Format(Val(txtReciveAmount.Text), "0.00"), TxtRemark.Text)
            txtAccount.Focus() : calc() : txtReciveAmount.Clear() : TxtRemark.Clear()
        End If

    End Sub

    Private Sub txtNaration_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNaration.KeyDown

    End Sub

    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub
End Class