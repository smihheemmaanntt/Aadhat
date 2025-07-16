Public Class PartnerDetails

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveDetails()
    End Sub
    Private Sub SaveDetails()

        Dim sql As String
        Dim cmd As SQLite.SQLiteCommand
        sql = "Delete From Channel;Insert Into Channel(D1, D2,D3,D4,YN) Values (@1,@2,@3,@4,@5)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            cmd.Parameters.AddWithValue("@1", txtFirmName.Text)
            cmd.Parameters.AddWithValue("@2", txtPartnerName.Text)
            cmd.Parameters.AddWithValue("@3", txtMobileNo.Text)
            cmd.Parameters.AddWithValue("@4", txtMail.Text)
            cmd.Parameters.AddWithValue("@5", IIf(ckEnable.Checked = True, "Y", "N"))
            If cmd.ExecuteNonQuery() > 0 Then
                MsgBox("Partner Details Updated", MsgBoxStyle.Information, "Successful") : MainScreenPicture.PartnerDetails()
                ClsFunPrimary.CloseConnection()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub

    Private Sub PartnerDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim sql As String = String.Empty
        sql = "create table if not exists Channel(D1 TEXT,D2 TEXT,D3 TEXT,D4 TEXT,YN TEXT);"
        ClsFunPrimary.ExecNonQuery(sql)
        sql = "Select * from Channel"
        Dim ad As New SQLite.SQLiteDataAdapter(sql, ClsFunPrimary.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            txtFirmName.Text = ds.Tables("a").Rows(0)("D1").ToString()
            txtPartnerName.Text = ds.Tables("a").Rows(0)("D2").ToString()
            txtMobileNo.Text = ds.Tables("a").Rows(0)("D3").ToString()
            txtMail.Text = ds.Tables("a").Rows(0)("D4").ToString()
            If ds.Tables("a").Rows(0)("YN").ToString() = "Y" Then ckEnable.Checked = True Else ckEnable.Checked = False
        End If
    End Sub
End Class