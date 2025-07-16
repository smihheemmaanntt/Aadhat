Public Class Users_Register

    Private Sub Users_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Users_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 3
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "User Name"
        dg1.Columns(1).Width = 300
        dg1.Columns(2).Name = "Password"
        dg1.Columns(2).Width = 150
        retrive()
    End Sub
    Private Sub retrive()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Users")
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("UserName").ToString()
                        ' .Cells(2).Value = dt.Rows(i)("Password").ToString("*")
                        '.cells(2).value=
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        dg1.ClearSelection()
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        UserForm.MdiParent = MainScreenForm
        UserForm.Show()
        If Not UserForm Is Nothing Then
            UserForm.BringToFront()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        UserForm.MdiParent = MainScreenForm
        UserForm.Show()
        Dim tmpid As Integer = dg1.SelectedRows(0).Cells(0).Value
        UserForm.FillControls(tmpid)
        If Not UserForm Is Nothing Then
            UserForm.BringToFront()
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            UserForm.MdiParent = MainScreenForm
            UserForm.Show()
            Dim tmpid As Integer = dg1.SelectedRows(0).Cells(0).Value
            UserForm.FillControls(tmpid)
            If Not UserForm Is Nothing Then
                UserForm.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub
End Class