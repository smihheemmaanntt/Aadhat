Imports System.Data.SQLite
Imports System.IO

Public Class Crate_Balance_Editor
    Dim root As String = Application.StartupPath
    ' Public Shared filepath As String = String.Empty
    Public newconnection As String = ""
    Private Sub Account_Balance_Editor_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If Pnlconfirmation.Visible = True Then Pnlconfirmation.Visible = False : Exit Sub
            Me.Close()
        End If
    End Sub

    Private Sub Account_Balance_Editor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        txtPath.Text = clsFun.ExecScalarStr("Select LinkedDb From Company")
        If txtPath.Text = String.Empty Then
            btnApply.Visible = False
        Else
            lblLinkedName.Text = ClsImportCrates.ExecScalarStr("Select CompanyName From Company")
            lbllinkedfy.Text = ClsImportCrates.ExecScalarStr("SELECT strftime('%d-%m-%Y', YearStart) || ' - ' || strftime('%d-%m-%Y', YearEnd) AS FinYear FROM Company")
            btnApply.Visible = True
        End If
        rowColums() : txtSearch.Focus()
        rowColums()
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name" : dg1.Columns(1).Width = 430
        dg1.Columns(2).Name = "Crate Name" : dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Crate In" : dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Crate Out" : dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "CrateID" : dg1.Columns(5).Width = 1
        retrive()
    End Sub

    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from CrateVoucher Where TransType='Op Bal'  " & condtion & " Group by CrateID,AccountID order by UPPER(AccountName)")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).ReadOnly = True : .Cells(1).ReadOnly = True
                        .Cells(2).ReadOnly = True
                        .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("CrateName").ToString()
                        If dt.Rows(i)("CrateType").ToString() = "Crate In" Then
                            .Cells(3).Value = Format(Val(dt.Rows(i)("Qty").ToString()), "0.00")
                        Else
                            .Cells(4).Value = Format(Val(dt.Rows(i)("Qty").ToString()), "0.00")
                        End If
                        .Cells(5).Value = dt.Rows(i)("CrateID").ToString()
                    End With
                Next
                dg1.Rows(0).Cells(3).Selected = True : calc()

            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try

    End Sub
    Private Sub calc()
        txtDramt.Text = Format(0, "0.00") : txtcrAmt.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtDramt.Text = Format(Val(txtDramt.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
            txtcrAmt.Text = Format(Val(txtcrAmt.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
        Next
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    '<System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")> <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")> Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
    '    If dg1.Rows.Count = 0 Then Exit Function
    '    Dim notlastColumn As Boolean
    '    Dim icolumn As Integer = dg1.CurrentCell.ColumnIndex
    '    Dim irow As Integer = dg1.CurrentCell.RowIndex
    '    Dim i As Integer = irow
    '    If keyData = Keys.Enter Then
    '        If icolumn = dg1.Columns.Count - 1 Then
    '            If notlastColumn = True Then
    '                dg1.CurrentCell = dg1.Rows(i).Cells(3)
    '            End If
    '            If dg1.CurrentRow.Index = Val(dg1.Rows.Count - 1) Then
    '                ' dg1.Rows.Add("0.00", "0.00")
    '                Exit Function
    '            End If
    '            dg1.CurrentCell = dg1(3, irow + 1)
    '            calc()
    '        Else
    '            dg1.CurrentCell = dg1(icolumn + 1, irow)
    '            calc()
    '        End If
    '        Return True
    '    Else
    '        Return ProcessCmdKey(msg, keyData)
    '    End If
    '    Return False
    'End Function

    Private Sub dg1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If e.ColumnIndex = 4 Then
            dg1.ReadOnly = False
        ElseIf e.ColumnIndex = 5 Then
            dg1.ReadOnly = False
        Else
            dg1.ReadOnly = True
        End If
    End Sub
    Private Sub dg1_CellValidating(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs)
        If Not dg1.Rows(e.RowIndex).Cells(4).Value Is Nothing Then
            If Not IsNumeric(dg1.Rows(e.RowIndex).Cells(4).Value) Then
                MessageBox.Show("Enter Numeric value only", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Cancel = True
            ElseIf Not dg1.Rows(e.RowIndex).Cells(5).Value Is Nothing Then
                If (dg1.Rows(e.RowIndex).Cells(5).Value) > 0 Then
                    MessageBox.Show("Only One Column Should be contain value", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    e.Cancel = True
                End If
            End If
        ElseIf Not dg1.Rows(e.RowIndex).Cells(5).Value Is Nothing Then
            If Not IsNumeric(dg1.Rows(e.RowIndex).Cells(5).Value) Then
                MessageBox.Show("Enter Numeric value only", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.Cancel = True
            ElseIf Not dg1.Rows(e.RowIndex).Cells(4).Value Is Nothing Then
                If (dg1.Rows(e.RowIndex).Cells(4).Value) > 0 Then
                    MessageBox.Show("Only One Column Should be contain value", "", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    e.Cancel = True
                End If
            End If
        End If

    End Sub
    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellEndEdit
        If dg1.CurrentCell.ColumnIndex = 4 Then
            dg1.CurrentRow.Cells(3).Value = "0.00"
            Dim Sql As String = "Update CrateVoucher SET Qty='" & Val(dg1.CurrentRow.Cells(4).Value) & "', CrateType='Crate Out' Where AccountID='" & Val(dg1.CurrentRow.Cells(0).Value) & "' and CrateID='" & Val(dg1.CurrentRow.Cells(5).Value) & "' and VoucherID=0"
            If clsFun.ExecNonQuery(Sql) > 0 Then
                lblCrateBalance.Visible = True
                lblCrateBalance.Text = dg1.CurrentRow.Cells(1).Value & ": Opening Balance : " & Format(Val(dg1.CurrentRow.Cells(4).Value), "0.00") & " Crate Out"
            End If
        ElseIf dg1.CurrentCell.ColumnIndex = 3 Then
            dg1.CurrentRow.Cells(4).Value = "0.00"
            Dim Sql As String = "Update CrateVoucher SET Qty='" & Val(dg1.CurrentRow.Cells(3).Value) & "', CrateType='Crate In' Where AccountID='" & Val(dg1.CurrentRow.Cells(0).Value) & "' and CrateID='" & Val(dg1.CurrentRow.Cells(5).Value) & "' and VoucherID=0"
            If clsFun.ExecNonQuery(Sql) > 0 Then
                lblCrateBalance.Visible = True
                lblCrateBalance.Text = dg1.CurrentRow.Cells(1).Value & ": Crate Opening Balance : " & Format(Val(dg1.CurrentRow.Cells(3).Value), "0.00") & " Crate In"
            End If
            'ElseIf dg1.CurrentCell.ColumnIndex = 5 Then
            '    Dim Sql As String = "Update Accounts SET OtherName='" & dg1.CurrentRow.Cells(5).Value & "' Where AccountID='" & Val(dg1.CurrentRow.Cells(0).Value) & "'"
            '    If clsFun.ExecNonQuery(Sql) > 0 Then
            '        lblMSg.Visible = True
            '        lblMSg.Text = dg1.CurrentRow.Cells(1).Value & ": Other Name Updated : " & dg1.CurrentRow.Cells(5).Value & ""
            '    End If
        End If

    End Sub


    Private Sub dg1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dg1.CurrentCellDirtyStateChanged
        If dg1.CurrentCell.ColumnIndex = 1 AndAlso dg1.IsCurrentCellDirty Then
            dg1.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        If txtSearch.Text.Trim() <> "" Then
            retrive(" and accountname Like '" & txtSearch.Text.Trim() & "%'")
        Else
            retrive()
        End If
    End Sub

    Private Sub txtSearcGroup_TextChanged(sender As Object, e As EventArgs) Handles txtSearcGroup.TextChanged
        If txtSearcGroup.Text.Trim() <> "" Then
            retrive(" and CrateName Like '" & txtSearcGroup.Text.Trim() & "%'")
        Else
            retrive()
        End If
    End Sub


    Private Sub dg1_CellContentClick_2(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            BringBalances()
        End If
    End Sub
    Private Sub BringBalances()
        Dim i As Integer
        Dim dt As New DataTable
        Dim ssql As String = "" : ProgressBar1.Visible = True
        Dim sql As String = String.Empty
        lblStatus.Visible = True
        Dim OpDate As String = CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("yyyy-MM-dd")
        clsFun.ExecNonQuery("Delete From CrateVoucher Where TransType='Op Bal'")
        ssql = ""
        dt = ClsImportCrates.ExecDataTable("Select CrateID,CrateName,AccountID,AccountName FROM CrateVoucher Where AccountID Not in(0,7) Group by CrateName,AccountID   order by AccountName ")
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    Application.DoEvents()
                    'If Application.OpenForms().OfType(Of Change_Financial_Year).Any = False Then Exit Sub
                    ProgressBar1.Maximum = dt.Rows.Count
                    lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
                    ProgressBar1.Value = i
                    If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
                    sql = "Select  ((Select ifnull(Sum(Qty),0) From CrateVoucher Where CrateType='Crate In'  and AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "')" &
                                        "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where CrateType='Crate Out' and  AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "')) as  Restbal " &
                                        " from CrateVoucher   where  AccountID='" & Val(dt.Rows(i)("AccountID").ToString()) & "' Group by AccountID  order by AccountName ;"
                    Dim crateTotbal As String = ClsImportCrates.ExecScalarStr(sql)
                    If Val(crateTotbal) <> 0 Then
                        Dim tmpamtdr1 As String = ClsImportCrates.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        Dim tmpamtcr1 As String = ClsImportCrates.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        tmpamt1 = Val(tmpamtdr1) - Val(tmpamtcr1) '- Val(opbal)
                        Dim tmpamtdr As String = ClsImportCrates.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        Dim tmpamtcr As String = ClsImportCrates.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(dt.Rows(i)("AccountID").ToString()) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "")
                        Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
                        If tmpamt <> 0 Then
                            If Val(tmpamtcr) > Val(tmpamtdr) Then
                                cratebal = Math.Abs(Val(tmpamt))
                                Dim AccountName As String = clsFun.ExecScalarStr("Select AccountName From Accounts Where ID='" & Val(dt.Rows(i)("AccountID").ToString()) & "'")
                                ssql = "Insert Into CrateVoucher (VoucherID,SlipNo,EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName,Qty,Remark,Rate,Amount,CashPaid) values" & _
                                    "(0," & Val(i + 1) & ",'" & OpDate & "','Op Bal'," & Val(dt.Rows(i)("AccountID").ToString()) & ", " & _
                                    " '" & AccountName & "','Crate Out','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "', " & _
                                    " '" & Val(cratebal) & "','Opening Crate Balance',0,0,'N')"
                                a1 = clsFun.ExecNonQuery(ssql, True)
                                If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
                            ElseIf Val(tmpamtcr) < Val(tmpamtdr) Then
                                cratebal = Math.Abs(Val(tmpamt))
                                ssql = "Insert Into CrateVoucher (VoucherID,SlipNo,EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName,Qty,Remark,Rate,Amount,CashPaid) values" & _
                                       "(0," & Val(i + 1) & ",'" & OpDate & "','Op Bal'," & Val(dt.Rows(i)("AccountID").ToString()) & ", " & _
                                       " '" & dt.Rows(i)("AccountName").ToString() & "','Crate In','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "', " & _
                                       " '" & Val(cratebal) & "','Opening Crate Balance',0,0,'N')"
                                a1 = clsFun.ExecNonQuery(ssql, True)
                                If lblCrateBalance.Visible = True Then lblCrateBalance.Visible = False Else lblCrateBalance.Visible = True
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
        lblCrateBalance.Visible = True
        lblCrateBalance.Text = "Crates Updated..."
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        Pnlconfirmation.Visible = True
        Pnlconfirmation.BringToFront()
        txtUpdate.Focus()
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If txtUpdate.Text <> "SURE" Then MsgBox("captcha Mis Match, Unable to Update Balances", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        If MessageBox.Show(" Are you Sure Want to Update Balnaces From Other Database ??", "Update Balance", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            BringBalances() : Pnlconfirmation.Visible = False
        End If
    End Sub
End Class