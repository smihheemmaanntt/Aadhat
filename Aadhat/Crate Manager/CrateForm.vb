Public Class CrateForm
    Private Sub CrateForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub CrateForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.GhostWhite
        Me.KeyPreview = True : BtnDelete.Visible = False
    End Sub
    Private Sub save()
        Dim guid As Guid = guid.NewGuid()
        Dim cmd As New SQLite.SQLiteCommand
        If txtCratename.Text = "" Then
            txtCratename.Focus()
            MsgBox("Please Fill Group Name... ", MsgBoxStyle.Exclamation, "Empty")
        Else
            Dim sql As String = "insert into CrateMarka (MarkaName,OpQty,Rate,guid) values (@1, @2, @3,@4)"
            Try
                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                cmd.Parameters.AddWithValue("@1", txtCratename.Text.Trim)
                cmd.Parameters.AddWithValue("@2", Val(txtopQty.Text))
                cmd.Parameters.AddWithValue("@3", Val(txtRatePerCrate.Text))
                cmd.Parameters.AddWithValue("@4", guid.ToString)
                If cmd.ExecuteNonQuery() > 0 Then
                    MessageBox.Show("Create Marka Inserted Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ServerDb() : Create_marka_list.btnRetrive.PerformClick()
                    textclear() : Purchase.crateRefresh()
                    txtCratename.Focus()
                End If
                clsFun.CloseConnection()
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
    End Sub
    Private Sub ServerDb()
        If OrgID = 0 Then Exit Sub
        Dim CrateID As Integer = 0
        If BtnSave.Text = "&Save" Then
            CrateID = clsFun.ExecScalarInt("Select Max(ID) from CrateMarka")
        Else
            ClsFunserver.ExecScalarStr("Delete From CrateMarka Where ID='" & Val(txtid.Text) & "' and OrgID='" & Val(OrgID) & "'")
            CrateID = Val(txtid.Text)
        End If
        Dim sql As String = "insert into CrateMarka (ID,MarkaName,OpQty,Rate,ServerTag,ORGID) values (@1, @2, @3,@4,@5,@6)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunserver.GetConnection())
            cmd.Parameters.AddWithValue("@1", Val(CrateID))
            cmd.Parameters.AddWithValue("@2", txtCratename.Text)
            cmd.Parameters.AddWithValue("@3", Val(txtopQty.Text))
            cmd.Parameters.AddWithValue("@4", Val(txtRatePerCrate.Text))
            cmd.Parameters.AddWithValue("@5", Val(1))
            cmd.Parameters.AddWithValue("@6", Val(OrgID))
            cmd.ExecuteNonQuery()
            ClsFunserver.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunserver.CloseConnection()
        End Try
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim ssql As String = String.Empty
        ' Dim primary As String = String.Empty
        Dim dt As New DataTable
        BtnSave.Image = My.Resources.Edit
        BtnSave.BackColor = Color.Coral
        lblgroup.Text = "Modify Crate"
        BtnDelete.Visible = True
        BtnSave.Text = "&Update"
        ssql = "Select * from CrateMarka where id=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            txtCratename.Text = dt.Rows(0)("MarkaName").ToString()
            txtopQty.Text = dt.Rows(0)("OpQty").ToString()
            txtRatePerCrate.Text = dt.Rows(0)("Rate").ToString()
            txtid.Text = dt.Rows(0)("ID").ToString()
        End If
    End Sub
    Private Sub UpdateMarka()
        Dim cmd As New SQLite.SQLiteCommand
        If txtCratename.Text = "" Then
            txtCratename.Focus()
            MsgBox("Please Fill Marka Name... ", MsgBoxStyle.Exclamation, "Empty")
        Else
            '  Dim sql As String = "Update AccountGroup SET GroupName='" & txtGroupName.Text & "',DC='" & lbldc.Text & "',UndergrpID=" & CbUnderGroup.SelectedValue & ",IsPrimary='" & primary & "',ISCHNGDEL=0 WHERE ID=" & Val(txtid.Text) & ""
            Dim sql As String = "Update CrateMarka SET MarkaName='" & txtCratename.Text.Trim & "',OpQty='" & Val(txtopQty.Text) & "',Rate='" & Val(txtRatePerCrate.Text) & "' WHERE ID=" & Val(txtid.Text) & ""
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    MessageBox.Show("Create Marka Updated Successfully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ServerDb() : textclear() : Purchase.crateRefresh()
                End If
                'con.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                'con.Close()
            End Try
        End If
    End Sub
    Private Sub textclear()
        txtCratename.Text = ""
        txtopQty.Text = ""
        txtRatePerCrate.Text = ""
        BtnSave.Image = My.Resources.Save
        BtnSave.BackColor = Color.DarkTurquoise
        BtnSave.Text = "&Save"
        txtCratename.Focus()
        BtnDelete.Visible = False

    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarInt("Select count(*) from CrateMarka where upper(MarkaName)=upper('" & txtCratename.Text.Trim & "')") = 1 Then
                MsgBox("Marka Name Already Exists...", vbOkOnly, "Access Denied")
                txtCratename.Focus() : Exit Sub
            End If
            save()
        Else
            If clsFun.ExecScalarInt("Select count(*) from CrateMarka where upper(MarkaName)=upper('" & txtCratename.Text.Trim & "')") > 1 Then
                MsgBox("Marka Name Already Exists...", vbOkOnly, "Access Denied")
                txtCratename.Focus() : Exit Sub
            End If
            UpdateMarka()
        End If
        SpeedSale.BtnRefresh.PerformClick()
        Super_Sale.BtnRefresh.PerformClick()
        Standard_Sale.BtnRefresh.PerformClick()
        Purchase.BtnRefresh.PerformClick()
        Stock_Sale.BtnRefresh.PerformClick()
    End Sub
    Private Sub Delete()

        If clsFun.ExecScalarInt("Select count(*) from CrateVoucher where CrateID=" & Val(txtid.Text) & "") <> 0 Then
            MsgBox("Crate Already Used in Transactions.", MsgBoxStyle.Critical, "Access Denied")
            Exit Sub
        Else
            Try
                If MessageBox.Show("Sure ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                    If clsFun.ExecNonQuery("DELETE from CrateMarka WHERE ID=" & Val(txtid.Text) & "") > 0 Then
                        MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                        ClsFunserver.ExecScalarStr("Update CrateMarka Set ServerTag=0 Where ID='" & Val(txtid.Text) & "' and OrgID='" & Val(OrgID) & "'")
                        Create_marka_list.btnRetrive.PerformClick()
                        Panel1.BackColor = Color.Teal : BtnDelete.Visible = False
                        BtnSave.Text = "&Save" : txtCratename.Focus()
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub
    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub

    Private Sub txtRatePerCrate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRatePerCrate.KeyDown, txtCratename.KeyDown, txtopQty.KeyDown
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

    Private Sub txtid_TextChanged(sender As Object, e As EventArgs) Handles txtid.TextChanged

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtCratename_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCratename.KeyPress
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCratename_Leave(sender As Object, e As EventArgs) Handles txtCratename.Leave
        If txtCratename.Text <> txtCratename.Text.ToUpper Then
            txtCratename.Text = StrConv(txtCratename.Text, VbStrConv.ProperCase)
        End If
        If BtnSave.Text = "&save" Then
            If clsFun.ExecScalarInt("Select count(*) from CrateMarka where MarkaName='" & txtCratename.Text & "'") = 1 Then
                MsgBox("Charges Already Exists...", vbOkOnly, "Access Denied")
                Exit Sub
                txtCratename.Focus()
            End If
            If clsFun.ExecScalarInt("Select count(*) from CrateMarka where MarkaName='" & txtCratename.Text & "'") > 1 Then
                MsgBox("Charges Already Exists...", vbOkOnly, "Access Denied")
                Exit Sub
                txtCratename.Focus()
            End If
        End If
    End Sub

    Private Sub txtCratename_TextChanged(sender As Object, e As EventArgs) Handles txtCratename.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Create_marka_list.MdiParent = MainScreenForm
        Create_marka_list.Show()
        If Not Create_marka_list Is Nothing Then
            Create_marka_list.BringToFront()
        End If
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub
End Class