Public Class Query_Maker

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Dim sql As String = String.Empty
        If RtxtQuery.Text.Trim = "" Then MsgBox("Please Insert Valid Sqlite Query ") : Exit Sub
        sql = RtxtQuery.Text
        If clsFun.ExecNonQuery(sql) > 0 Then
            MsgBox("Query Successful Updated...", MsgBoxStyle.Information, "Sucessful")
        End If
    End Sub

    Private Sub Query_Maker_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape AndAlso RtxtQuery.Text = "" Then Me.Close()
    End Sub

    Private Sub Query_Maker_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPassword.UseSystemPasswordChar = True
        txtPassword.Focus() : RtxtQuery.Enabled = False
        Me.KeyPreview = True
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        If txtPassword.Text = "MUKESH" Then pnlLock.Visible = False : RtxtQuery.Enabled = True : RtxtQuery.Focus()
    End Sub

End Class