Public Class Storage_Register

    Private Sub Storage_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Storage_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.Top = 200 : Me.Left = 450
        Me.BackColor = Color.GhostWhite
        Me.KeyPreview = True
        rowColums()
    End Sub
    Private Sub rowColums()
        With dg1
            .ColumnCount = 2
            .Columns(0).HeaderText = "ID" : .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).HeaderText = "Storage Place" : .Columns(1).Name = "Storage Place" : .Columns(1).Width = 323
            retrive()
        End With
    End Sub
    Public Sub retrive()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Storage")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("StorageName").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            Store.MdiParent = MainScreenForm
            Store.Show()
            Store.FillControls(tmpID)
            If Not Store Is Nothing Then
                Store.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Store.MdiParent = MainScreenForm
        Store.Show()
        Store.FillControls(tmpID)
        If Not Store Is Nothing Then
            Store.BringToFront()
        End If
    End Sub

    Private Sub btnRetrive_Click(sender As Object, e As EventArgs)
        retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class