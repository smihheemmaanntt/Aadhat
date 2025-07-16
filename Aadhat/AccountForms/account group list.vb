Public Class account_group_list

    Private Sub account_group_list_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub account_group_list_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        rowColums() : txtSearch.Focus()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Group Name" : dg1.Columns(1).Width = 450
        dg1.Columns(2).Name = "Under Group" : dg1.Columns(2).Width = 250
        dg1.Columns(3).Name = "DR/CR" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Primary" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Tag" : dg1.Columns(5).Width = 150
        retrive()
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable : dg1.Rows.Clear()
        dt = clsFun.ExecDataTable("Select * from AccountGroup " & condtion & "")
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("GroupName").ToString()
                        .Cells(2).Value = dt.Rows(i)("UnderGroupName").ToString()
                        .Cells(3).Value = dt.Rows(i)("DC").ToString()
                        .Cells(4).Value = dt.Rows(i)("Primary2").ToString()
                        .Cells(5).Value = dt.Rows(i)("Tag").ToString()
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
    Private Sub txtSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearch.KeyPress
        If txtSearch.Text.Trim() <> "" Then
            retrive(" where GroupName Like '%" & txtSearch.Text.Trim() & "%'")
        End If
    End Sub
    Private Sub txtSearchGroup_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearchGroup.KeyPress
        If txtSearchGroup.Text.Trim() <> "" Then
            retrive(" where UnderGroupName Like '%" & txtSearchGroup.Text.Trim() & "%'")
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                '   btnShow.PerformClick()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            Account_Group.MdiParent = MainScreenForm
            Account_Group.Show()
            Account_Group.FillControls(tmpID)
            If Not Account_Group Is Nothing Then
                Account_Group.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Account_Group.MdiParent = MainScreenForm
        Account_Group.Show()
        Account_Group.FillControls(tmpID)
        If Not Account_Group Is Nothing Then
            Account_Group.BringToFront()
        End If
    End Sub

    Private Sub btnRetrive_Click(sender As Object, e As EventArgs) Handles btnRetrive.Click
        retrive()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub
End Class