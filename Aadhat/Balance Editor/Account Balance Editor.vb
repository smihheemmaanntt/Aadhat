Imports System.Data.SQLite
Imports System.IO

Public Class Account_Balance_Editor
    Dim root As String = Application.StartupPath
    ' Public Shared filepath As String = String.Empty
    Public newconnection As String = ""



    Private Sub Account_Balance_Editor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        txtPath.Text = clsFun.ExecScalarStr("Select LinkedDb From Company")
        If txtPath.Text = String.Empty Then
            btnApply.Visible = False
        Else
            lblLinkedName.Text = ClsImportBalances.ExecScalarStr("Select CompanyName From Company")
            lbllinkedfy.Text = ClsImportBalances.ExecScalarStr("SELECT strftime('%d-%m-%Y', YearStart) || ' - ' || strftime('%d-%m-%Y', YearEnd) AS FinYear FROM Company")
        End If


        rowColums() : txtSearch.Focus()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name" : dg1.Columns(1).Width = 430
        dg1.Columns(2).Name = "Group" : dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Debit" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Credit" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Other Name" : dg1.Columns(5).Width = 200
        retrive()
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Accounts ac inner join AccountGroup grp on ac.GroupID=grp.ID " & condtion & " order by UPPER(AccountName)")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Readonly = True : .Cells(1).Readonly = True
                        .Cells(2).Readonly = True
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("GroupName").ToString()
                        If dt.Rows(i)("DC").ToString() = "Dr" Then
                            .Cells(3).Value = Format(Val(dt.Rows(i)("OpBal").ToString()), "0.00")
                        Else
                            .Cells(4).Value = Format(Val(dt.Rows(i)("OpBal").ToString()), "0.00")
                        End If
                        .Cells(5).Value = dt.Rows(i)("OtherName").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.Rows(0).Cells(3).Selected = True : calc()
        LblTotal.Visible = True
        LblTotal.Text = "Total Accounts : " & dg1.Rows.Count
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
            Dim Sql As String = "Update Accounts SET Opbal='" & Val(dg1.CurrentRow.Cells(4).Value) & "',  DC='Cr' Where ID='" & Val(dg1.CurrentRow.Cells(0).Value) & "'"
            If clsFun.ExecNonQuery(Sql) > 0 Then
                lblCrateBalance.Visible = True
                lblCrateBalance.Text = dg1.CurrentRow.Cells(1).Value & ": Opening Balance : " & Format(Val(dg1.CurrentRow.Cells(4).Value), "0.00") & " Cr"
            End If
        ElseIf dg1.CurrentCell.ColumnIndex = 3 Then
            dg1.CurrentRow.Cells(4).Value = "0.00"
            Dim Sql As String = "Update Accounts SET Opbal='" & Val(dg1.CurrentRow.Cells(3).Value) & "', DC='Dr' Where ID='" & Val(dg1.CurrentRow.Cells(0).Value) & "'"
            If clsFun.ExecNonQuery(Sql) > 0 Then
                lblCrateBalance.Visible = True
                lblCrateBalance.Text = dg1.CurrentRow.Cells(1).Value & ": Opening Balance : " & Format(Val(dg1.CurrentRow.Cells(3).Value), "0.00") & " Dr"
            End If
        ElseIf dg1.CurrentCell.ColumnIndex = 5 Then
            Dim Sql As String = "Update Accounts SET OtherName='" & dg1.CurrentRow.Cells(5).Value & "' Where ID='" & Val(dg1.CurrentRow.Cells(0).Value) & "'"
            If clsFun.ExecNonQuery(Sql) > 0 Then
                lblCrateBalance.Visible = True
                lblCrateBalance.Text = dg1.CurrentRow.Cells(1).Value & ": Other Name Updated : " & dg1.CurrentRow.Cells(5).Value & ""
            End If
        End If
        calc()
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
            retrive(" and GroupName Like '" & txtSearcGroup.Text.Trim() & "%'")
        Else
            retrive()
        End If
    End Sub

    Private Sub dg1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dg1_CellContentClick_2(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            If MessageBox.Show("Are you Sure Want Import Balances From Another Company...???", "Warning (On Your Own Risk)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                BringBalances() : btnApply.Visible = False
            End If
        End If
    End Sub
    
    Private Sub BringBalances()
        Dim i As Integer
        Dim dt As New DataTable
        Dim ssql As String = "" : ProgressBar1.Visible = True
        Dim sql As String = String.Empty
        lblStatus.Visible = True
        sql = "Select ID,Accountname,DC, Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
        " Where AccountID=Account_AcGrp.ID and DC='D')-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID " &
        " and DC='C' ))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
        "  Where AccountID=Account_AcGrp.ID and DC='C') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
        " Where AccountID=Account_AcGrp.ID and DC='D'))  end),2) as  Restbal from Account_AcGrp Where ParentID Not In(22,23,24,25,26,27);"
        dt = ClsImportBalances.ExecDataTable(sql)
        For i = 0 To dt.Rows.Count - 1
            Application.DoEvents()
            ProgressBar1.Maximum = dt.Rows.Count
            lblStatus.Text = Format(Val(i + 1) * 100 / dt.Rows.Count, "0.00") & " %"
            ProgressBar1.Value = i
            If Val(dt.Rows(i)("Restbal").ToString()) = 0 Then
                ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , DC='" & dt.Rows(i)("DC").ToString() & "' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
            ElseIf Val(dt.Rows(i)("Restbal").ToString()) > 0 Then
                ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , Dc='Dr' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
            ElseIf Val(dt.Rows(i)("Restbal").ToString()) < 0 Then
                ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " , Dc='Cr' where id=" & Val(dt.Rows(i)("id").ToString()) & ";"
            End If
        Next
        Dim a As Integer = clsFun.ExecNonQuery(ssql, True)
        Dim GroupID As String = ClsImportBalances.ExecScalarStr("SELECT GROUP_CONCAT(ac.ID) AS Concatenated_IDs FROM Accounts AS ac INNER JOIN AccountGroup AS grp ON ac.Groupid = grp.ID" & _
                              " WHERE grp.UnderGroupID in(22,23,24,25,26,27) or Ac.GroupID  in (22,23,24,25,26,27);")
        clsFun.ExecNonQuery("Update Accounts Set Opbal=0 Where ID in(" & GroupID & ") ")

        ssql = String.Empty
        Dim GPGl As Decimal = ClsImportBalances.ExecScalarDec(" Select Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D')" & _
                                    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' )) " & _
                                    " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C')" & _
                                    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D'))  end) as  GroupBal from Accounts " & _
                                    " Where  GroupID  in(22,23,24,25,26,27);")

        If Val(GPGl) > 0 Then
            ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(GPGl)) & " , Dc='Dr' where id=38;"
        ElseIf Val(GPGl) < 0 Then
            ssql = ssql & "Update Accounts set opbal=" & Math.Abs(Val(GPGl)) & " , Dc='Cr' where id=38;"
        End If
        a = clsFun.ExecNonQuery(ssql, True)
        If a > 0 Then
            MsgBox("Balance Updated From Other Company")
            retrive() : dg1.Focus()
        End If
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If MessageBox.Show(" Are you Sure Want to Update Balnaces From Other Database ??", "Update Balance", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
            BringBalances()
        End If

    End Sub
End Class