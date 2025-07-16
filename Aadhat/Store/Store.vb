Public Class Store
    Private Sub Store_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub Store_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        '  Me.BackColor = Color.GhostWhite
        Me.KeyPreview = True
        Me.Top = 250 : Me.Left = 450
        BtnDelete.Visible = False
        txtid.Visible = False
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        BtnSave.Text = "&Update"
        Panel1.BackColor = Color.PaleVioletRed
        BtnDelete.Visible = True
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from Storage where id=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtid.Text = ds.Tables("a").Rows(0)("ID").ToString()
            txtStoreName.Text = ds.Tables("a").Rows(0)("StorageName").ToString()
        End If
    End Sub
    Private Sub UpdateRecord()
        If txtStoreName.Text.Trim = "" Then MsgBox("Please Item Name... ", MsgBoxStyle.Exclamation, "Access Denied") : txtStoreName.Focus() : Exit Sub
        If clsFun.ExecScalarInt("Select count(*)from items where upper(itemName)=upper('" & txtStoreName.Text.Trim & "')") > 1 Then
            MsgBox("Store Already Exists...", vbOKOnly, "Access Denied") : txtStoreName.Focus() : Exit Sub
        End If
        Dim sql As String = "Update Storage SET StorageName='" & txtStoreName.Text.Trim & "' WHERE ID=" & Val(txtid.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                MsgBox("Store Name Updated Successfully...", vbInformation, "Updated")
                Storage_Register.retrive()
                txtStoreName.Clear() : txtStoreName.Focus()
            End If
            'con.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            'con.Close()
        End Try
    End Sub
    Private Sub save()
        Dim guid As Guid = guid.NewGuid()
        Dim sql As String = "insert into Storage (StorageName,guid) values (@1,@2)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", txtStoreName.Text.Trim)
            cmd.Parameters.AddWithValue("@2", guid.ToString)
            If cmd.ExecuteNonQuery() > 0 Then
                MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
                Storage_Register.retrive()
                txtStoreName.Clear() : txtStoreName.Focus()
                'txtCratename.Focus()
            End If
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
        ' Purchase.BtnRefresh.PerformClick()
    End Sub

    Private Sub txtStoreName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtStoreName.KeyDown
        If e.KeyCode = Keys.Enter Then BtnSave.Focus()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If txtStoreName.Text.Trim = "" Then MsgBox("Please Fill Store Name", MsgBoxStyle.Critical, "Fill Store") : txtStoreName.Focus() : Exit Sub
        If BtnSave.Text = "&Save" Then
            If clsFun.ExecScalarStr("Select count(*) from Storage where upper(StorageName)=upper('" & txtStoreName.Text & "')") = 1 Then
                MsgBox("Account Already Exists...", vbOKOnly, "Access Denied") : txtStoreName.Focus() : Exit Sub
            End If
            save()
        Else
            If clsFun.ExecScalarStr("Select count(*) from Storage where upper(StorageName)=upper('" & txtStoreName.Text & "')") > 1 Then
                MsgBox("Account Already Exists...", vbOKOnly, "Access Denied") : txtStoreName.Focus() : Exit Sub
            End If
            UpdateRecord()
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If clsFun.ExecScalarInt("Select count(*) from Vouchers where StorageID=" & Val(txtid.Text) & ";Select count(*) from Transaction2 where StorageID=" & Val(txtid.Text) & ";Select count(*) from Purchase where StorageID=" & Val(txtid.Text) & "") <> 0 Then
            MsgBox("Storage Already Used in Transactions", vbOKOnly, "Access Denied")
            Exit Sub
        End If
        Try
            If MessageBox.Show("Are you Sure want to Delete Storage : " & txtStoreName.Text & " ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) = Windows.Forms.DialogResult.Yes Then
                If clsFun.ExecNonQuery("DELETE from Storage WHERE ID=" & Val(txtid.Text) & "") > 0 Then
                    'Me.Alert("Deleted Successful...", msgAlert.enmType.Delete)
                    MsgBox("Record Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtStoreName_Leave(sender As Object, e As EventArgs) Handles txtStoreName.Leave
        If txtStoreName.Text <> txtStoreName.Text.ToUpper Then
            txtStoreName.Text = StrConv(txtStoreName.Text, VbStrConv.ProperCase)
        End If
    End Sub
End Class