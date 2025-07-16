Public Class Create_marka_list

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 4
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Marka Name"
        dg1.Columns(1).Width = 200
        dg1.Columns(2).Name = "Op. Qty"
        dg1.Columns(2).Width = 66
        dg1.Columns(3).Name = "Rate"
        dg1.Columns(3).Width = 200
        retrive()
    End Sub

    Private Sub Create_marka_list_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub Create_marka_list_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.GhostWhite
        Me.KeyPreview = True
        rowColums()
    End Sub
    Private Sub retrive()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from CrateMarka Order by Upper(MarkaName)")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("MarkaName").ToString()
                        .Cells(2).Value = dt.Rows(i)("Opqty").ToString()
                        .Cells(3).Value = dt.Rows(i)("Rate").ToString()

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
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            CrateForm.FillControls(tmpID)
            If Not CrateForm Is Nothing Then
                CrateForm.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        '  If e.KeyCode = Keys.Enter Then
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        CrateForm.MdiParent = MainScreenForm
        CrateForm.Show()
        CrateForm.FillControls(tmpID)
        If Not CrateForm Is Nothing Then
            CrateForm.BringToFront()
        End If
        'e.SuppressKeyPress = True
        'End If
    End Sub

    Private Sub btnRetrive_Click(sender As Object, e As EventArgs) Handles btnRetrive.Click
        retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CrateForm.MdiParent = MainScreenForm
        CrateForm.Show()
        If Not CrateForm Is Nothing Then
            CrateForm.BringToFront()
        End If
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub
End Class