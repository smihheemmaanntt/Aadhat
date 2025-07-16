Public Class Account_Group
    Dim tmpid As Integer = 0
    Public bUpdate As Boolean
    Private Sub Account_Group_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub Account_Group_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Top = 179 : Me.Left = 450 : Me.BackColor = Color.GhostWhite
        Me.KeyPreview = True
        'clsFun.FillDropDownList(CbUnderGroup, "Select ID, GroupName   FROM AccountGroup where UnderGroupID not in (0) UNION all Select ID,UnderGroup   FROM UnderGroup ", "GroupName", "Id", "")
        clsFun.FillDropDownList(CbUnderGroup, "Select ID, GroupName   FROM AccountGroup ", "GroupName", "Id", "")
        BtnDelete.Visible = False
    End Sub
    Private Sub save()
        Dim guid As Guid = guid.NewGuid()
        Dim ParentID As Integer = Val(clsFun.ExecScalarInt("Select ParentID From AccountGroup Where ID=" & Val(CbUnderGroup.SelectedValue) & " "))
        Dim cmd As New SQLite.SQLiteCommand
        If txtGroupName.Text = "" Then
            txtGroupName.Focus()
            MsgBox("Please Fill Group Name... ", MsgBoxStyle.Exclamation, "Empty")
        Else
            If ckPrimary.Checked Then Isprimary = "Y" Else Isprimary = "N"
            Dim sql As String = "insert into AccountGroup (GroupName,UnderGroupID,UnderGroupName,DC,Primary2,tag,ParentID,guid ) values (@1,@2,@3,@4,@5,@6,@7,@8)"
            Try
                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                If ckPrimary.Checked Then
                    cmd.Parameters.AddWithValue("@1", txtGroupName.Text)
                    cmd.Parameters.AddWithValue("@2", Val(0))
                    cmd.Parameters.AddWithValue("@3", "")
                    cmd.Parameters.AddWithValue("@4", lbldc.Text)
                    cmd.Parameters.AddWithValue("@5", Isprimary)
                    cmd.Parameters.AddWithValue("@6", 1)
                    cmd.Parameters.AddWithValue("@7", ParentID)
                    cmd.Parameters.AddWithValue("@8", guid.ToString)
                Else
                    cmd.Parameters.AddWithValue("@1", txtGroupName.Text)
                    cmd.Parameters.AddWithValue("@2", Val(CbUnderGroup.SelectedValue))
                    cmd.Parameters.AddWithValue("@3", CbUnderGroup.Text)
                    cmd.Parameters.AddWithValue("@4", lbldc.Text)
                    cmd.Parameters.AddWithValue("@5", Isprimary)
                    cmd.Parameters.AddWithValue("@6", 1)
                    cmd.Parameters.AddWithValue("@7", ParentID)
                    cmd.Parameters.AddWithValue("@8", guid.ToString)
                End If

                If cmd.ExecuteNonQuery() > 0 Then
                    ServerDbAdd()
                    MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
                    account_group_list.btnRetrive.PerformClick()
                    '  Textclear()
                    txtGroupName.Focus()
                End If
                clsFun.CloseConnection()
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
    End Sub
    Private Sub ServerDbAdd()
        If OrgID = 0 Then Exit Sub
        Dim GroupID As Integer = 0
        If BtnSave.Text = "&Save" Then
            GroupID = clsFun.ExecScalarInt("Select Max(ID) from AccountGroup")
        Else
            ClsFunserver.ExecScalarStr("Delete From AccountGroup Where ID='" & Val(txtid.Text) & "' and OrgID='" & Val(OrgID) & "'")
            GroupID = Val(txtid.Text)
        End If
        Dim ParentID As Integer = Val(clsFun.ExecScalarInt("Select ParentID From AccountGroup Where ID=" & Val(CbUnderGroup.SelectedValue) & " "))
        Dim Isprimary As String = ""
        If ckPrimary.Checked Then Isprimary = "Y" Else Isprimary = "N"
        Dim sql As String = "insert into AccountGroup (GroupName,UnderGroupID,UnderGroupName,DC,Primary2,tag,ParentID,ServerTag,ORGID,ID) values (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunserver.GetConnection())
            If ckPrimary.Checked Then
                cmd.Parameters.AddWithValue("@1", txtGroupName.Text)
                cmd.Parameters.AddWithValue("@2", Val(0))
                cmd.Parameters.AddWithValue("@3", "")
                cmd.Parameters.AddWithValue("@4", lbldc.Text)
                cmd.Parameters.AddWithValue("@5", Isprimary)
                cmd.Parameters.AddWithValue("@6", 1)
                cmd.Parameters.AddWithValue("@7", ParentID)
                cmd.Parameters.AddWithValue("@8", 1)
                cmd.Parameters.AddWithValue("@9", OrgID)
                cmd.Parameters.AddWithValue("@10", GroupID)

            Else
                cmd.Parameters.AddWithValue("@1", txtGroupName.Text)
                cmd.Parameters.AddWithValue("@2", Val(CbUnderGroup.SelectedValue))
                cmd.Parameters.AddWithValue("@3", CbUnderGroup.Text)
                cmd.Parameters.AddWithValue("@4", lbldc.Text)
                cmd.Parameters.AddWithValue("@5", Isprimary)
                cmd.Parameters.AddWithValue("@6", 1)
                cmd.Parameters.AddWithValue("@7", ParentID)
                cmd.Parameters.AddWithValue("@8", 1)
                cmd.Parameters.AddWithValue("@9", OrgID)
                cmd.Parameters.AddWithValue("@10", GroupID)
            End If

            If cmd.ExecuteNonQuery() > 0 Then
            End If
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Public Sub FillControls(ByVal id As Integer)
        Dim ssql As String = String.Empty
        Dim primary As String = String.Empty
        Dim dt As New DataTable
        lblgroup.Text = "Modify Group"
        BtnDelete.Visible = True
        BtnSave.Text = "&Update"
        ssql = "Select * from AccountGroup where id=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            txtid.Text = dt.Rows(0)("ID").ToString()
            txttag.Text = dt.Rows(0)("tag").ToString()
            txtGroupName.Text = dt.Rows(0)("GroupName").ToString()
            lbldc.Text = dt.Rows(0)("DC").ToString()
            primary = dt.Rows(0)("Primary2").ToString().Trim()
            If primary = "Y" Then
                clsFun.FillDropDownList(CbUnderGroup, "", "", "", "")
                ckPrimary.CheckState = CheckState.Checked
            Else
                ckPrimary.CheckState = CheckState.Unchecked
                CbUnderGroup.Text = dt.Rows(0)("UnderGroupName").ToString()
            End If
        End If
    End Sub
    Private Sub UpdateGroup(name As String)
        Dim ParentID As Integer = Val(clsFun.ExecScalarInt("Select ParentID From AccountGroup Where ID=" & Val(CbUnderGroup.SelectedValue) & " "))
        ' Dim cmd As New SQLite.SQLiteCommand
        If txtGroupName.Text = "" Then
            txtGroupName.Focus()
            MsgBox("Please Fill Group Name... ", MsgBoxStyle.Exclamation, "Empty")
        Else
            Dim primary As String = ""
            If ckPrimary.CheckState = CheckState.Checked Then primary = "Y"
            If ckPrimary.CheckState = CheckState.Unchecked Then primary = "N"
            '  Dim sql As String = "Update AccountGroup SET GroupName='" & txtGroupName.Text & "',DC='" & lbldc.Text & "',UndergrpID=" & CbUnderGroup.SelectedValue & ",IsPrimary='" & primary & "',ISCHNGDEL=0 WHERE ID=" & Val(txtid.Text) & ""

            Dim sql As String = "Update AccountGroup SET GroupName='" & txtGroupName.Text & "',DC='" & lbldc.Text & "',UnderGroupID=" & Val(CbUnderGroup.SelectedValue) & " ,UnderGroupName='" & CbUnderGroup.Text & "',Primary2='" & primary & "',ParentID=" & ParentID & " WHERE ID=" & Val(txtid.Text) & ""
            Dim cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    ServerDbAdd()
                    MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                    account_group_list.btnRetrive.PerformClick()
                    lblgroup.Text = "CREATE GROUP"
                    Panel1.BackColor = Color.Teal
                    ' Textclear()
                    BtnSave.Text = "&Save"
                    BtnDelete.Visible = False
                    txtGroupName.Focus()
                    Me.Close()
                End If
                'con.Close()
            Catch ex As Exception
                MsgBox(ex.Message)
                'con.Close()
            End Try
        End If
    End Sub
    Private Sub Delete()
        Try
            If clsFun.ExecScalarInt("Select tag From AccountGroup  WHERE ID=" & txtid.Text & "") = 0 Then MsgBox("Primary Groups Can't be Deleted", vbOKOnly, "Access Denied") : Exit Sub
            If clsFun.ExecScalarInt("Select count(*) from Accounts where GroupId='" & Val(txtid.Text) & "'") = 0 Then
                If MessageBox.Show("Sure ??", "DELETE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
                    If clsFun.ExecNonQuery("DELETE from AccountGroup WHERE ID=" & txtid.Text & "") > 0 Then
                        ClsFunserver.ExecScalarStr("Update AccountGroup Set ServerTag=0 Where ID='" & Val(txtid.Text) & "' and OrgID='" & Val(OrgID) & "'")
                        MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "deleted")
                        lblgroup.Text = "CREATE GROUP"
                        account_group_list.btnRetrive.PerformClick()
                        Panel1.BackColor = Color.Teal
                        BtnDelete.Visible = False
                        BtnSave.Text = "&Save"
                        txtGroupName.Focus()
                    End If
                End If
            Else
                MsgBox("Account Group Cannot delete alreday use in Account", vbOKOnly, "Access Denied")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub ckPrimary_CheckedChanged(sender As Object, e As EventArgs) Handles ckPrimary.CheckedChanged
        If ckPrimary.Checked Then
            clsFun.FillDropDownList(CbUnderGroup, "Select * From AccountGroup Where Primary2='Y'", "GroupName", "Id", "")
            '    CbUnderGroup.Enabled = False
        Else
            clsFun.FillDropDownList(CbUnderGroup, "Select * From AccountGroup", "GroupName", "Id", "")
            '   CbUnderGroup.Enabled = True
        End If
    End Sub
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If txtGroupName.Text = "" Then MsgBox("Can't Save Without Group Name", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        If BtnSave.Text = "&Save" Then
            save()
        Else
            UpdateGroup(Name)
        End If
    End Sub

    Private Sub CbUnderGroup_KeyDown(sender As Object, e As KeyEventArgs) Handles CbUnderGroup.KeyDown, txtGroupName.KeyDown, ckPrimary.KeyDown
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

    Private Sub CbUnderGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbUnderGroup.SelectedIndexChanged
        'ShowDetails()
        ShowDc()
    End Sub
    Private Sub ShowDc()
        '   lbldc.Text = clsFun.ExecScalarStr(" Select DC FROM UnderGroup WHERE UnderGroup like '%" + CbUnderGroup.Text + "%'")
        lbldc.Text = clsFun.ExecScalarStr(" Select DC FROM AccountGroup WHERE GroupName like '%" + CbUnderGroup.Text + "%'")
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub

    Private Function sql() As Object
        Throw New NotImplementedException
    End Function

    Private Sub txttag_TextChanged(sender As Object, e As EventArgs) Handles txttag.TextChanged

    End Sub

    Private Sub txtGroupName_Leave(sender As Object, e As EventArgs) Handles txtGroupName.Leave
        If BtnSave.Text = "&save" Then
            If clsFun.ExecScalarInt("Select count(*)from AccountGroup where GroupName='" & txtGroupName.Text & "'") = 1 Then
                MsgBox("Account Group Already  Exists...", vbOKOnly, "Access Denied")
                txtGroupName.Focus()
                Exit Sub
            End If
        Else
        End If
    End Sub

    Private Sub txtGroupName_TextChanged(sender As Object, e As EventArgs) Handles txtGroupName.TextChanged

    End Sub
End Class