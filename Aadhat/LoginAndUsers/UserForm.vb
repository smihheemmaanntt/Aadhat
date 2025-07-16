Public Class UserForm

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If txtPassword.Text <> txtConfirm.Text Then
            MsgBox("Password Did Not Matched... Please Re Check Your Password...", MsgBoxStyle.Exclamation, "Unmatched")
            txtConfirm.Focus() : Exit Sub
        End If

        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarInt("Select count(*) from users where upper(userName)=upper('" & TxtUsername.Text & "')") = 1 Then
                MsgBox("UserName Name Already Exists...", vbOKOnly, "Access Denied")
                TxtUsername.Focus() : Exit Sub
            End If
            Save()
        Else
            If clsFun.ExecScalarInt("Select count(*) from users where upper(userName)=upper('" & TxtUsername.Text & "')") > 1 Then
                MsgBox("UserName Name Already Exists...", vbOKOnly, "Access Denied")
                TxtUsername.Focus() : Exit Sub
            End If
            UpdateRecord()
        End If
    End Sub

    Private Sub Save()
        Dim InActive As String = String.Empty
        If ckActive.Checked = True Then InActive = "Y" Else InActive = "N"
        Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "insert into Users(UserName, Password,Tag,UserType,FullName,InActive,UserTypeID)values (@1, @2,@3,@4, @5,@6,@7)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", TxtUsername.Text)
            cmd.Parameters.AddWithValue("@2", txtPassword.Text)
            cmd.Parameters.AddWithValue("@3", 1)
            cmd.Parameters.AddWithValue("@4", CbUserType.Text)
            cmd.Parameters.AddWithValue("@5", txtFullName.Text)
            cmd.Parameters.AddWithValue("@6", InActive)
            cmd.Parameters.AddWithValue("@7", Val(CbUserType.SelectedValue))
            If cmd.ExecuteNonQuery() > 0 Then
                MessageBox.Show("Record Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtClear()
            End If
            ' MsgBox("Successfully Inserted")
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Private Sub UserForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub UserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : CbUserType.SelectedIndex = 0
        clsFun.FillDropDownList(CbUserType, "Select UserTypeID,UserTypeName FROM userRights Group by UserTypeID ", "UserTypeName", "UserTypeID", "")
    End Sub
    Private Sub UpdateRecord()
        Dim InActive As String = String.Empty
        If ckActive.Checked = True Then InActive = "Y" Else InActive = "N"
        Dim sql As String = "Update Users SET UserTypeID='" & Val(CbUserType.SelectedValue) & "',UserType='" & CbUserType.Text & "',FullName='" & txtFullName.Text & "',  " &
                            "userName='" & TxtUsername.Text & "',Password='" & txtPassword.Text & "', " &
                            " InActive='" & InActive & "' WHERE ID=" & Val(txtid.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                txtClear()
            End If
            'con.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            'con.Close()
        End Try
    End Sub
    Private Sub txtClear()
        txtFullName.Clear() : TxtUsername.Clear()
        txtPassword.Clear() : ckActive.Checked = False
        CbUserType.Focus() : CbUserType.Enabled = True
        txtConfirm.Clear() : ckActive.Visible = True
        account_group_list.btnRetrive.PerformClick()
        lblgroup.Text = "USER CREATION"
        Panel1.BackColor = Color.Teal
        BtnSave.Text = "&Save"
        btnDelete.Visible = False
    End Sub
    Private Sub TxtUsername_KeyDown(sender As Object, e As KeyEventArgs) Handles CbUserType.KeyDown, TxtUsername.KeyDown, txtFullName.KeyDown,
                                                                                 txtPassword.KeyDown, txtConfirm.KeyDown, ckActive.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        Panel1.BackColor = Color.PaleVioletRed
        lblgroup.Text = "Modify User"
        btnDelete.Visible = True
        BtnSave.Text = "&Update"
        Dim InActive As String = String.Empty
        Dim Tag As String = String.Empty
        ssql = "Select * from Users where id=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            txtid.Text = dt.Rows(0)("ID").ToString()
            TxtUsername.Text = dt.Rows(0)("UserName").ToString()
            txtPassword.Text = dt.Rows(0)("Password").ToString()
            CbUserType.SelectedValue = Val(dt.Rows(0)("UserTypeID").ToString())
            CbUserType.Text = dt.Rows(0)("UserType").ToString()
            txtFullName.Text = dt.Rows(0)("FullName").ToString()
            Tag = dt.Rows(0)("Tag").ToString()
            If Tag = 0 Then CbUserType.Enabled = False Else CbUserType.Enabled = True
            If Tag = 0 Then ckActive.Visible = False Else ckActive.Visible = True
            InActive = dt.Rows(0)("InActive").ToString()
            If InActive = "Y" Then ckActive.Checked = True Else ckActive.Checked = False
        End If
    End Sub

    Private Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        Users_Register.MdiParent = MainScreenForm
        Users_Register.Show()
        If Not Users_Register Is Nothing Then
            Users_Register.BringToFront()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        delete()
    End Sub
    Private Sub delete()
        If clsFun.ExecScalarInt("Select tag From Users  WHERE ID=" & txtid.Text & "") = 0 Then MsgBox("Pre-Define Master. You Can't Delete it.", vbOKOnly, "Access Denied") : Exit Sub
        If clsFun.ExecScalarInt("Select count(*) from Vouchers where UserID=" & Val(txtid.Text) & " or ModifiedByID=" & Val(txtid.Text) & "") <> 0 Then
            MsgBox("User Already have Some Work...", vbOKOnly, "Access Denied")
            Exit Sub
        End If
        If MessageBox.Show("are you Sure want to Delete User... ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            If clsFun.ExecNonQuery("DELETE from Users WHERE ID=" & txtid.Text & "") > 0 Then
                MsgBox("User Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                txtClear()
            End If
        End If
    End Sub

    Private Sub TxtUsername_Leave(sender As Object, e As EventArgs) Handles TxtUsername.Leave
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarInt("Select count(*) from users where upper(userName)=upper('" & TxtUsername.Text & "')") = 1 Then
                MsgBox("UserName Name Already Exists...", vbOKOnly, "Access Denied")
                TxtUsername.Focus() : Exit Sub
            End If
        Else
            If clsFun.ExecScalarInt("Select count(*) from users where upper(userName)=upper('" & TxtUsername.Text & "')") > 1 Then
                MsgBox("UserName Name Already Exists...", vbOKOnly, "Access Denied")
                TxtUsername.Focus() : Exit Sub
            End If
        End If
    End Sub

    Private Sub ckActive_CheckedChanged(sender As Object, e As EventArgs) Handles ckActive.CheckedChanged
        If ckActive.Checked = True Then
            ckActive.Text = "Inactived"
        Else
            ckActive.Text = "Inactive User"
        End If
    End Sub
End Class