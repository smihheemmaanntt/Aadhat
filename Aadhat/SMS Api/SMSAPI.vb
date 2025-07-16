Public Class SMSAPI

    Private Sub SmsTemplete_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub SmsTemplete_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        me.Top = 0
        Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.GhostWhite
        Me.KeyPreview = True
        '  rowColums()
        fillControls()
    End Sub
    'Private Sub rowColums()
    '    dg1.ColumnCount = 3
    '    dg1.Columns(0).Name = "ID"
    '    dg1.Columns(0).Visible = False
    '    dg1.Columns(1).Name = "SMS Apply on"
    '    dg1.Columns(1).Width = 250
    '    dg1.Columns(2).Name = "Default Templete"
    '    dg1.Columns(2).Width = 700
    'End Sub
    Private Sub fillControls()
        Dim ds As New DataSet
        Dim sSql As String = String.Empty
        sSql = "Select * from SMSAPI "
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtpart1.Text = ds.Tables("a").Rows(0)("Part1").ToString()
            txtPart2.Text = ds.Tables("a").Rows(0)("part2").ToString()
            txtpart3.Text = ds.Tables("a").Rows(0)("part3").ToString()
            '    txtDefault.Text = ds.Tables("a").Rows(0)("DefaultTemplate").ToString()
        End If

    End Sub
    Private Sub save()
        Dim sql As String = String.Empty
        Dim cmd As SQLite.SQLiteCommand
        clsFun.ExecNonQuery("Delete from SMSAPI")
        sql = "insert into SMSAPI(part1,part2, Part3,DefaultTemplate)" _
                                    & "values (@1, @2, @3,@4)"
        Try
            cmd = New SQLite.SQLiteCommand(Sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", txtpart1.Text)
            cmd.Parameters.AddWithValue("@2", txtPart2.Text)
            cmd.Parameters.AddWithValue("@3", txtpart3.Text)
            '  cmd.Parameters.AddWithValue("@4", txtDefault.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                MsgBox("SMS SMSAPI Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                clsFun.CloseConnection()
                txtpart1.Clear()
                txtPart2.Clear()
                txtpart3.Clear()
                ' txtDefault.Clear()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        save()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class