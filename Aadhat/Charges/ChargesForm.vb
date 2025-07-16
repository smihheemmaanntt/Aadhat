Public Class ChargesForm
    Private Sub ChargesForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub


    Private Sub ChargesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        clsFun.FillDropDownList(cbAccountName, "Select * From Accounts", "AccountName", "Id", "")
        cbApply.SelectedIndex = 0
        cbCost.SelectedIndex = 0
        ' BtnUpdate.Visible = False
        BtnDelete.Visible = False
        rowColums()
        RadioAmount.Focus() : radioPlus.Focus()
    End Sub
    Private Sub retrive()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Charges")
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("ChargeName").ToString()
                        .Cells(2).Value = dt.Rows(i)("PrintName").ToString()
                        .Cells(3).Value = dt.Rows(i)("Calculate").ToString()
                        .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = dt.Rows(i)("ApplyType").ToString()
                        .Cells(6).Value = dt.Rows(i)("ChargesType").ToString()
                        .Cells(7).Value = dt.Rows(i)("ApplyOn").ToString()
                        .Cells(8).Value = dt.Rows(i)("CostOn").ToString()
                        .Cells(9).Value = dt.Rows(i)("RoundOff").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
    End Sub
    Private Sub Save()
        Dim guid As Guid = guid.NewGuid()
        If txtCalculate.Text = "" Then txtCalculate.Text = "0"
        Dim cmd As New SQLite.SQLiteCommand
        Dim ChargesType As String = ""
        Dim ApplyType As String = ""
        Dim ISRoundOFf As String = ""
        If radioMinus.Checked = True Then ChargesType = "-"
        If radioPlus.Checked = True Then ChargesType = "+"
        If RadioAmount.Checked = True Then ApplyType = "Aboslute"
        If radioPercentage.Checked = True Then ApplyType = "Percentage"
        If radioWeight.Checked = True Then ApplyType = "Weight"
        If radioNug.Checked = True Then ApplyType = "Nug"
        If RadioCrate.Checked = True Then ApplyType = "Crate"
        If CkRoundOff.Checked Then
            ISRoundOFf = "Y"
        Else
            ISRoundOFf = "N"
        End If
        If TxtChargeName.Text.Trim = "" Then
            MsgBox("Charge Name Can't be Empty.", vbOKOnly, "Access Denied ")
            TxtChargeName.Focus()
        Else
            Dim sql As String = "insert into Charges (ChargeName,Calculate,AccountID,AccountName,ApplyType,ChargesType,ApplyOn,CostOn,RoundOff,PrintName,guid) " & _
                "values (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)"
            Try
                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                cmd.Parameters.AddWithValue("@1", TxtChargeName.Text.Trim.Trim)
                cmd.Parameters.AddWithValue("@2", txtCalculate.Text)
                cmd.Parameters.AddWithValue("@3", cbAccountName.SelectedValue)
                cmd.Parameters.AddWithValue("@4", cbAccountName.Text)
                cmd.Parameters.AddWithValue("@5", ApplyType)
                cmd.Parameters.AddWithValue("@6", ChargesType)
                cmd.Parameters.AddWithValue("@7", cbApply.Text)
                cmd.Parameters.AddWithValue("@8", cbCost.Text)
                cmd.Parameters.AddWithValue("@9", ISRoundOFf)
                cmd.Parameters.AddWithValue("@10", txtPrintName.Text)
                cmd.Parameters.AddWithValue("@11", guid.ToString)
                If cmd.ExecuteNonQuery() > 0 Then
                    MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
                    txtclear()
                End If
                clsFun.CloseConnection()
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If btnSave.Text = "&Save" Then
            If clsFun.ExecScalarInt("Select count(*) from Charges where upper(ChargeName)=upper('" & TxtChargeName.Text.Trim & "')") = 1 Then
                MsgBox("Charges Already Exists...", vbOKOnly, "Access Denied")
                TxtChargeName.Focus() : Exit Sub
            End If
            Save()
        Else
            If clsFun.ExecScalarInt("Select count(*) from Charges where upper(ChargeName)=upper('" & TxtChargeName.Text.Trim & "')") > 1 Then
                MsgBox("Charges Already Exists...", vbOKOnly, "Access Denied")
                TxtChargeName.Focus() : Exit Sub
            End If
            UpdateCharges()
        End If
        'scripForm.BtnRefresh.PerformClick()
        'Standard_Sale.BtnRefresh.PerformClick()
    End Sub
    Public Sub FillContros(ByVal id As Integer)
        btnSave.Text = "&Update"
        BtnDelete.Visible = True
        Dim ChargesType As String = ""
        Dim ApplyType As String = ""
        Dim RoundOff As String = ""
        Dim sSql As String = String.Empty
        LblCharges.Text = "MODIFY CHARGES"
        btnSave.Image = My.Resources.Edit
        btnSave.BackColor = Color.Coral
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Charges where id=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtID.Text = ds.Tables("a").Rows(0)("ID").ToString()
            TxtChargeName.Text = ds.Tables("a").Rows(0)("ChargeName").ToString().Trim
            txtPrintName.Text = ds.Tables("a").Rows(0)("PrintName").ToString().Trim
            txtCalculate.Text = ds.Tables("a").Rows(0)("Calculate").ToString()
            cbAccountName.Text = ds.Tables("a").Rows(0)("AccountName").ToString()
            ApplyType = ds.Tables("a").Rows(0)("ApplyType").ToString().Trim
            If ApplyType = "Aboslute" Then
                RadioAmount.Checked = True
            End If
            If ApplyType = "Percentage" Then
                radioPercentage.Checked = True
            End If
            If ApplyType = "Weight" Then
                radioWeight.Checked = True
            End If
            If ApplyType = "Nug" Then
                radioNug.Checked = True
            End If
            If ApplyType = "Crate" Then
                RadioCrate.Checked = True
            End If
            ChargesType = ds.Tables("a").Rows(0)("ChargesType").ToString()
            If ChargesType = "+" Then
                radioPlus.Checked = True
            End If
            If ChargesType = "-" Then
                radioMinus.Checked = True
            End If
            cbApply.Text = ds.Tables("a").Rows(0)("ApplyON").ToString()
            cbCost.Text = ds.Tables("a").Rows(0)("CostON").ToString()
            RoundOff = ds.Tables("a").Rows(0)("RoundOff").ToString()
            If RoundOff = "Y" Then CkRoundOff.Checked = True
        Else
            CkRoundOff.Checked = False
        End If

    End Sub

    Private Sub txtPrintName_GotFocus(sender As Object, e As EventArgs) Handles txtPrintName.GotFocus, txtPrintName.LostFocus
        Try
            SendKeys.Send("+%")
        Catch ex As InvalidOperationException
            ' Do nothing
        End Try
        txtPrintName.SelectAll()
    End Sub

    Private Sub TxtChargeName_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtChargeName.KeyDown, txtPrintName.KeyDown, txtCalculate.KeyDown, cbAccountName.KeyDown, RadioAmount.KeyDown,
        radioPercentage.KeyDown, radioNug.KeyDown, radioWeight.KeyDown, radioPlus.KeyDown, radioMinus.KeyDown, cbApply.KeyDown, cbCost.KeyDown, CkRoundOff.KeyDown, RadioCrate.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnSave.Focus()
        End Select
        Select Case e.KeyCode
            Case Keys.Down
                If cbAccountName.Focused Or cbApply.Focused Or cbCost.Focused Then Exit Sub
                e.Handled = True
                dg1.Focus()
        End Select
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Charge Name" : dg1.Columns(1).Width = 180
        dg1.Columns(2).Name = "Print Name" : dg1.Columns(2).Width = 180
        dg1.Columns(3).Name = "Cal.@" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Account Post" : dg1.Columns(4).Width = 250
        dg1.Columns(5).Name = "($/%)" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "(+/-)" : dg1.Columns(6).Width = 80
        dg1.Columns(7).Name = "Apply On" : dg1.Columns(7).Width = 100
        dg1.Columns(8).Name = "Cost on" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "R.off" : dg1.Columns(9).Width = 100
        retrive()
    End Sub

    Private Sub Delete()
        If clsFun.ExecScalarInt("Select count(*) from ChargesTrans where ChargesID='" & Val(txtID.Text) & "'") <> 0 Then
            MsgBox("Charges Already Used in Transactions", vbOKOnly, "Access Denied")
            Exit Sub
        Else
            Try
                '  If clsFun.ExecScalarInt("Select tag From AccountGroup  WHERE ID=" & txtid.Text & "") = 0 Then MsgBox("access denied", MsgBoxStyle.Critical) : Exit Sub
                '  If clsFun.ExecScalarInt("Select count(*) from Accounts where GroupId=" & Val(txtid.Text) & "") = 1 Then
                If MessageBox.Show("Are you Sure Want to Delete Charges??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    If clsFun.ExecNonQuery("DELETE from Charges WHERE ID=" & txtID.Text & "") > 0 Then
                        MsgBox("Successfully deleted", MsgBoxStyle.Information, "Deleted")
                        txtclear()
                    End If
                End If
                'Else
                'MsgBox("Account Group Cannot delete alreday use in Account", vbOkOnly, "Access Denied")
                'End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            FillContros(dg1.SelectedRows(0).Cells(0).Value)
            TxtChargeName.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub UpdateCharges()
        Dim cmd As New SQLite.SQLiteCommand
        If TxtChargeName.Text.Trim = "" Then
            TxtChargeName.Focus()
            MsgBox("Please fill Charge Name... ", MsgBoxStyle.Exclamation, "Access Denied")
        Else
            Dim RoundOFf As String = ""
            If CkRoundOff.CheckState = CheckState.Checked Then RoundOFf = "Y"
            If CkRoundOff.CheckState = CheckState.Unchecked Then RoundOFf = "N"
            Dim ApplyType As String = ""
            If RadioAmount.Checked = True Then ApplyType = "Aboslute"
            If radioPercentage.Checked = True Then ApplyType = "Percentage"
            If radioWeight.Checked = True Then ApplyType = "Weight"
            If radioNug.Checked = True Then ApplyType = "Nug"
            Dim ChargeType As String = ""
            If radioPlus.Checked = True Then ChargeType = "+"
            If radioMinus.Checked = True Then ChargeType = "-"
            Dim sql As String = "Update Charges SET ChargeName='" & TxtChargeName.Text.Trim.Trim & "', Calculate='" & txtCalculate.Text & "', " & _
                "AccountID=" & Val(cbAccountName.SelectedValue) & ",AccountName='" & cbAccountName.Text & "',ApplyType='" & ApplyType & "', " & _
                "ChargesType='" & ChargeType & "',ApplyOn='" & cbApply.Text & "',CostOn='" & cbCost.Text & "',Roundoff='" & RoundOFf & "', " & _
               " PrintName='" & txtPrintName.Text.Trim & "' WHERE ID=" & Val(txtID.Text) & ""
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    MsgBox("Record Updated Successfully", MsgBoxStyle.Information, "Updated")
                    txtclear()
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Private Sub txtclear()
        txtCalculate.Clear()
        TxtChargeName.Clear()
        CkRoundOff.Checked = False
        LblCharges.Text = "CHARGES ENTRY"
        retrive()
        btnSave.Image = My.Resources.Save
        btnSave.BackColor = Color.DarkTurquoise
        btnSave.Text = "&Save"
        BtnDelete.Visible = False
        TxtChargeName.Focus()
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        FillContros(dg1.SelectedRows(0).Cells(0).Value)
    End Sub

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub

    Private Sub txtCalculate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCalculate.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub txtCalculate_TextChanged(sender As Object, e As EventArgs) Handles txtCalculate.TextChanged

    End Sub

    Private Sub TxtChargeName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtChargeName.KeyPress, txtPrintName.KeyPress
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub TxtChargeName_Leave(sender As Object, e As EventArgs) Handles TxtChargeName.Leave
        If TxtChargeName.Text.Trim <> TxtChargeName.Text.Trim.ToUpper Then
            TxtChargeName.Text = Trim(StrConv(TxtChargeName.Text.Trim, VbStrConv.ProperCase))
        End If
        If btnSave.Text = "&save" Then
            If clsFun.ExecScalarInt("Select count(*) from Charges where ChargeName='" & TxtChargeName.Text.Trim & "'") = 1 Then
                MsgBox("Charges Already Exists...", vbOKOnly, "Access Denied")
                TxtChargeName.Focus() : Exit Sub
            End If
        Else
            If clsFun.ExecScalarInt("Select count(*) from Charges where ChargeName='" & TxtChargeName.Text.Trim & "'") > 1 Then
                MsgBox("Charges Already Exists...", vbOKOnly, "Access Denied")
                TxtChargeName.Focus() : Exit Sub
            End If
        End If
    End Sub

    Private Sub TxtChargeName_TextChanged(sender As Object, e As EventArgs) Handles TxtChargeName.TextChanged
        If btnSave.Text = "&Save" Then
            txtPrintName.Text = TxtChargeName.Text.Trim
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub radioWeight_CheckedChanged(sender As Object, e As EventArgs) Handles radioWeight.CheckedChanged

    End Sub

    Private Sub cbAccountName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbAccountName.SelectedIndexChanged

    End Sub

    Private Sub cbApply_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbApply.SelectedIndexChanged

    End Sub
End Class