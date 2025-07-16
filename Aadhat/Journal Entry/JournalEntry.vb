Public Class JournalEntry
    Dim rs As New Resizer : Dim vno As Integer : Dim ServerTag As Integer

    Private Sub JournalEntry_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub JournalEntry_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        'rs.FindAllControls(Me)
        Me.BackColor = Color.FromArgb(247, 220, 111)
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.FormBorderStyle = FormBorderStyle.None
        clsFun.FillDropDownList(CbAccountName, "Select * From Accounts", "AccountName", "Id", "")
        CbDrCr.SelectedIndex = 0
        BtnDelete.Enabled = False
        rowColums()
        txtclear()
    End Sub
    Private Sub AcBalance()
        If clsFun.ExecScalarInt("Select ParentID From Account_AcGrp Where ID=" & Val(CbAccountName.SelectedValue) & "") = 8 Then
            Dim GPGl As Decimal = clsFun.ExecScalarDec(" Select Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                                "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                                " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                                " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  GroupBal from Accounts " & _
                                                " Where  GroupID  in(22,23,24,25,26,27);")
            Dim transferedPL As Decimal = clsFun.ExecScalarDec("Select Sum(Case When DC='Dr' then ((Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                    " else (-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  GroupBal from Account_AcGrp " & _
                                    " Where  ParentID  in(8);")
            If GPGl <> 0 Then lblCapAcBal.Visible = True
            lblCapAcBal.Text = IIf(GPGl > 0, Format(Math.Abs(GPGl) - Val(transferedPL), "0.00") & " Cr", Format(Math.Abs(GPGl) - Val(transferedPL), "0.00") & " Dr")
            txtAmount.Text = Math.Abs(GPGl) - Val(transferedPL)
        Else
            lblCapAcBal.Visible = True
            Dim Sql As String = String.Empty
            Sql = "Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
                  "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
                  " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
                  " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where ID=" & Val(CbAccountName.SelectedValue) & " Order by upper(AccountName) ;"
            Dim Bal As String = clsFun.ExecScalarStr(Sql)
            If Val(Bal) >= 0 Then
                lblCapAcBal.Visible = True
                lblCapAcBal.Text = "Bal : " & Format(Math.Abs(Val(Bal)), "0.00") & " Dr"
            Else
                lblCapAcBal.Visible = True
                lblCapAcBal.Text = "Bal : " & Format(Math.Abs(Val(Bal)), "0.00") & " Cr"
            End If
        End If
    End Sub
    Private Sub VNumber()
        Vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        txtVoucherNo.Text = Vno + 1
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "DC"
        dg1.Columns(0).Width = 95
        dg1.Columns(1).Name = "Account Name"
        dg1.Columns(1).Width = 380
        dg1.Columns(2).Name = "Debit"
        dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "Credit"
        dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Remark"
        dg1.Columns(4).Width = 500
        dg1.Columns(5).Name = "AcId"
        dg1.Columns(5).Visible = False
    End Sub
    Private Sub JournalEntry_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'rs.ResizeAllControls(Me)
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtVoucherNo.KeyDown,
        CbDrCr.KeyDown, CbAccountName.KeyDown, txtAmount.KeyDown
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

    Private Sub CbAccountName_Leave(sender As Object, e As EventArgs) Handles CbAccountName.Leave
        If clsFun.ExecScalarInt("Select count(*)from Accounts") = 0 Then
            Exit Sub
        End If
        If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & CbAccountName.Text & "'") = 0 Then
            MsgBox("Account Not Found in Database...", vbOKOnly, "Access Denied")
            CbAccountName.Focus()
            Exit Sub
        Else
            AcBalance()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub txtRemark_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRemark.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 1 Then
                If CbDrCr.Text = "D" Then
                    dg1.SelectedRows(0).Cells(0).Value = CbDrCr.Text
                    dg1.SelectedRows(0).Cells(1).Value = CbAccountName.Text
                    dg1.SelectedRows(0).Cells(2).Value = txtAmount.Text
                    dg1.SelectedRows(0).Cells(3).Value = ""
                    dg1.SelectedRows(0).Cells(4).Value = txtRemark.Text
                    dg1.SelectedRows(0).Cells(5).Value = Val(CbAccountName.SelectedValue)
                    CbDrCr.Focus()
                    dg1.ClearSelection()
                    calc()
                Else
                    dg1.SelectedRows(0).Cells(0).Value = CbDrCr.Text
                    dg1.SelectedRows(0).Cells(1).Value = CbAccountName.Text
                    dg1.SelectedRows(0).Cells(2).Value = ""
                    dg1.SelectedRows(0).Cells(3).Value = txtAmount.Text
                    dg1.SelectedRows(0).Cells(4).Value = txtRemark.Text
                    dg1.SelectedRows(0).Cells(5).Value = Val(CbAccountName.SelectedValue)
                    CbDrCr.Focus()
                    dg1.ClearSelection()
                    calc()
                End If
            Else 'mid
                If dg1.Rows.Count > 0 Then
                    For i = 0 To dg1.RowCount - 1
                        If dg1.Rows(i).Cells(1).Value = CbAccountName.Text Then
                            MsgBox("Same Account can't be Debited & Credited in a single Voucher")
                            CbAccountName.Focus()
                            Exit Sub
                        End If
                    Next
                End If
                If CbDrCr.Text = "D" Then
                    dg1.Rows.Add(CbDrCr.Text, CbAccountName.Text, txtAmount.Text, 0, txtRemark.Text, CbAccountName.SelectedValue)
                    calc()
                Else
                    dg1.Rows.Add(CbDrCr.Text, CbAccountName.Text, 0, txtAmount.Text, txtRemark.Text, CbAccountName.SelectedValue)
                    calc()
                End If
                calc()
                'cleartxtCharges()
                CbDrCr.Focus()
                dg1.ClearSelection()
            End If
            AcBalance()
        End If
    End Sub
    Sub calc()
        TxtTotalDebit.Text = Format(0, "0.00") : TxtTotalCredit.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            TxtTotalDebit.Text = Format(Val(TxtTotalDebit.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            TxtTotalCredit.Text = Format(Val(TxtTotalCredit.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
        Next
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows(0).Cells(0).Value = "D" Then
                CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
                CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
                txtAmount.Text = dg1.SelectedRows(0).Cells(2).Value
                txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
            Else
                CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
                CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
                txtAmount.Text = dg1.SelectedRows(0).Cells(3).Value
                txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
            End If
        End If
        e.SuppressKeyPress = True
        If e.KeyCode = Keys.Delete Then
            dg1.Rows.Remove(dg1.SelectedRows(0).Cells(0).Value)
        End If
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        If dg1.SelectedRows(0).Cells(0).Value = "D" Then
            CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
            CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
            txtAmount.Text = dg1.SelectedRows(0).Cells(2).Value
            txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
        Else
            CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
            CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
            txtAmount.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
        End If
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If BtnSave.Text = "&Save" Then
            save()
        Else
            UpdateRecord()
        End If
    End Sub
    Private Sub save()
        Dim SqliteEntryDate As String = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim sql As String = String.Empty
        If dg1.RowCount = 0 Then MsgBox("There is nothing to Save...", MsgBoxStyle.Critical, "Not Saved") : Exit Sub
        If TxtTotalDebit.Text <> TxtTotalCredit.Text Then
            MsgBox("Debit and Credit Balance Not Same")
            txtAmount.Focus()
            Exit Sub
        End If
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        sql = "insert into Vouchers (EntryDate,TransType,BillNo) values (@1, @2, @3)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@2", Me.Text)
            cmd.Parameters.AddWithValue("@3", txtVoucherNo.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                txtid.Text = Val(clsFun.ExecScalarInt("Select max(ID) ID from Vouchers;"))
                Ledger() : ServerTag = 1 : ServerLedger()
                MessageBox.Show("Record Inserted Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtclear()
            End If
            clsFun.CloseConnection()

            txtclear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub UpdateRecord()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        If dg1.RowCount = 0 Then MsgBox("There is nothing to Save...", MsgBoxStyle.Critical, "Not Saved") : Exit Sub
        If TxtTotalDebit.Text <> TxtTotalCredit.Text Then
            MsgBox("Debit and Credit Balance Are not Same", MsgBoxStyle.Exclamation, "Invalid")
            txtAmount.Focus()
            Exit Sub
        End If
        Dim sql As String = "Update Vouchers SET EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "',TransType='" & Me.Text & "', " &
                           "BillNo='" & txtVoucherNo.Text & "' Where ID=" & Val(txtid.Text) & ""
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                ClsFunserver.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & (txtid.Text) & " and OrgID=" & Val(OrgID) & "")
                clsFun.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & (txtid.Text) & "")
                Ledger() : ServerTag = 1 : ServerLedger()
                MessageBox.Show("Record Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            txtclear()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub Ledger()
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells("DC").Value <> "" Then
                    '    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, Val(.Cells(5).Value), .Cells(1).Value, IIf(Val(.Cells(2).Value) > 0, Val(.Cells(2).Value), .Cells(3).Value), .Cells("DC").Value, .Cells(4).Value, .Cells(1).Value)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "', " & _
                        "'" & Me.Text & "'," & Val(.Cells(5).Value) & ",'" & .Cells(1).Value & "'," & IIf(Val(.Cells(2).Value) > 0, Val(.Cells(2).Value), .Cells(3).Value) & ", " & _
                        "'" & .Cells("DC").Value & "' ,'" & .Cells(4).Value & "','" & .Cells(1).Value & "','" & .Cells(4).Value & "'"
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub
    Private Sub ServerLedger()
        If Val(OrgID) > 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells("DC").Value <> "" Then
                    '    clsFun.Ledger(0, VchId, SqliteEntryDate, Me.Text, Val(.Cells(5).Value), .Cells(1).Value, IIf(Val(.Cells(2).Value) > 0, Val(.Cells(2).Value), .Cells(3).Value), .Cells("DC").Value, .Cells(4).Value, .Cells(1).Value)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtid.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "', " & _
                        "'" & Me.Text & "'," & Val(.Cells(5).Value) & ",'" & .Cells(1).Value & "'," & IIf(Val(.Cells(2).Value) > 0, Val(.Cells(2).Value), .Cells(3).Value) & ", " & _
                        "'" & .Cells("DC").Value & "'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & .Cells(4).Value & "','" & .Cells(1).Value & "','" & .Cells(4).Value & "'"
                End If
            End With
        Next
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub

    Private Sub Delete()
        Try
            If MessageBox.Show("Sure ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(txtid.Text) & ";DELETE from Ledger WHERE vourchersID=" & Val(txtid.Text) & "") > 0 Then
                End If
                ClsFunserver.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & (txtid.Text) & " and OrgID=" & Val(OrgID) & "")
                ServerTag = 0 : ServerLedger()
                MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                txtclear()
            Else

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub txtclear()
        VNumber()
        txtRemark.Text = "" : TxtTotalCredit.Text = ""
        TxtTotalDebit.Text = "" : txtAmount.Text = ""
        BtnSave.Text = "&Save" : BtnDelete.Enabled = False
        dg1.Rows.Clear() : mskEntryDate.Focus()
    End Sub

    Public Sub FillContros(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        BtnSave.Text = "&Update"
        '  BtnUpdate.Visible = True
        BtnDelete.Enabled = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Vouchers where id=" & id
        'Dim sql As String = "Select EntryDate,TransType,AccountName,Amount as Dr,'' as Cr,Remark from Ledger where DC ='D' and VourchersID=" & id " & _
        '                    "UNION ALL Select VourchersID, EntryDate,TransType,AccountName,'' as Dr,Amount as Cr,Remark  from Ledger where Dc='C' ;"
        Dim sql As String = "Select * from Ledger where VourchersID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)

        Dim ds As New DataSet
        ad.Fill(ds, "a")
        ad1.Fill(ds, "b")
        If ds.Tables("a").Rows.Count > 0 Then
            mskEntryDate.Text = Format(ds.Tables("a").Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtVoucherNo.Text = ds.Tables("a").Rows(0)("BillNo").ToString()
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
        End If
        If ds.Tables("b").Rows.Count > 0 Then
            dg1.Rows.Clear()
            ' If dg1.Rows.Count = 0 Then dg1.Rows.Add()
            With dg1
                Dim i As Integer = 0
                For i = 0 To ds.Tables("b").Rows.Count - 1
                    .Rows.Add()
                    .Rows(i).Cells("DC").Value = ds.Tables("b").Rows(i)("Dc").ToString()
                    .Rows(i).Cells("Account Name").Value = ds.Tables("b").Rows(i)("AccountName").ToString()
                    .Rows(i).Cells("AcId").Value = clsFun.ExecScalarInt("Select ID From Accounts Where AccountName='" & ds.Tables("b").Rows(i)("AccountName").ToString() & "'")
                    If dg1.Rows(i).Cells(0).Value = "D" Then
                        .Rows(i).Cells("Debit").Value = ds.Tables("b").Rows(i)("Amount").ToString()
                    Else
                        .Rows(i).Cells("Credit").Value = ds.Tables("b").Rows(i)("Amount").ToString()
                    End If
                    .Rows(i).Cells("Remark").Value = ds.Tables("b").Rows(i)("Remark").ToString()
                Next
            End With
        End If
        calc() : dg1.ClearSelection()
        BtnDelete.Enabled = True
    End Sub

    Private Sub dg1_KeyDown1(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown

        If e.KeyCode = Keys.Enter Then

            If dg1.SelectedRows(0).Cells(0).Value = "D" Then
                CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
                CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
                txtAmount.Text = dg1.SelectedRows(0).Cells(2).Value
                txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
            Else
                CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
                CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
                txtAmount.Text = dg1.SelectedRows(0).Cells(3).Value
                txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
            End If
        End If
        e.SuppressKeyPress = True
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to delete Item", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
                calc()
                'ClearDetails()
            End If
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick1(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows(0).Cells(0).Value = "D" Then
            CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
            CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
            txtAmount.Text = dg1.SelectedRows(0).Cells(2).Value
            txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
        Else
            CbDrCr.Text = dg1.SelectedRows(0).Cells(0).Value
            CbAccountName.Text = dg1.SelectedRows(0).Cells(1).Value
            txtAmount.Text = dg1.SelectedRows(0).Cells(3).Value
            txtRemark.Text = dg1.SelectedRows(0).Cells(4).Value
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub
    Private Sub txtAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAmount.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtRemark_Leave(sender As Object, e As EventArgs) Handles txtRemark.Leave

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub txtRemark_TextChanged(sender As Object, e As EventArgs) Handles txtRemark.TextChanged

    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub lblCapAcBal_Click(sender As Object, e As EventArgs) Handles lblCapAcBal.Click

    End Sub
End Class