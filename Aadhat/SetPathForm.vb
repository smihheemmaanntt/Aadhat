Public Class SetPathForm

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            txtPath.Text = FolderBrowserDialog1.SelectedPath
        End If

    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If txtPath.Text <> String.Empty Then
            clsFun.ExecNonQuery("Update Path set DefaultPath='" & txtPath.Text.Trim & "'")
            MsgBox("Change Path Successfully")

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub

    Private Sub SetPathForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPath.Text = clsFun.ExecScalarStr("Select DefaultPath from Path")
    End Sub
End Class