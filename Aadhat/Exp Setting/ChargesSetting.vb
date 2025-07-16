Public Class ChargesSetting
    Private Sub ChargesForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub ChargesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        ' BtnUpdate.Visible = False
        BtnDelete.Enabled = False
        rowColums() : Me.KeyPreview = True
        clsFun.FillDropDownList(cbCharges, "Select ID,ChargeName From Charges", "ChargeName", "Id", "")
        cbChargesOn.SelectedIndex = 0
    End Sub

    Private Sub txtCalculate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCalculate.KeyDown, cbChargesOn.KeyDown,
        cbCharges.KeyDown, txtSrNo.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnadd.Focus()
        End Select
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If MessageBox.Show("Are you Sure to Delete Charges ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                dg1.Rows.Remove(dg1.SelectedRows(0))
                txtSrNo.Focus() : dg1.ClearSelection()
            End If
        End If
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 4
        dg1.Columns(0).Name = "Sr No." : dg1.Columns(0).Width = 130
        dg1.Columns(1).Name = "ChargesID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Charge Name" : dg1.Columns(2).Width = 648
        dg1.Columns(3).Name = "Calculate @" : dg1.Columns(3).Width = 245
        'retrive()
    End Sub

    Private Sub txtCalculate_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCalculate.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub cbChargesOn_Leave(sender As Object, e As EventArgs) Handles cbChargesOn.Leave
        FillControls(cbChargesOn.SelectedIndex)
    End Sub



    Private Sub Save()
        Dim sql As String = String.Empty
        ' Dim cmd As SQLite.SQLiteCommand
        sql = "insert into ExpVouchers (EntryDate,ApplyID,ApplyOn) values ('" & Date.Today.ToString("dd-MM-yyyy") & "', " & _
            "'" & Val(cbChargesOn.SelectedIndex) & "','" & cbChargesOn.Text & "')"
        If clsFun.ExecNonQuery(sql) Then
            txtID.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from ExpVouchers"))
            Dg1Record()
            MsgBox("Setting Saved For " & cbChargesOn.Text & " Successfully.", vbInformation + vbOKOnly, "Saved")
            cbChargesOn.Focus() : dg1.Rows.Clear() : txtID.Clear()
        End If
    End Sub
    Private Sub UpdateRecord()
        Dim sql As String = String.Empty
        '  Dim cmd As SQLite.SQLiteCommand
        sql = "Update  ExpVouchers Set EntryDate='" & Date.Today.ToString("dd-MM-yyyy") & "',ApplyID='" & Val(cbChargesOn.SelectedIndex) & "', " &
             "ApplyOn='" & cbChargesOn.Text & "' Where ID=" & Val(txtID.Text) & ""
        If clsFun.ExecNonQuery(sql) Then
            clsFun.ExecNonQuery("Delete From ExpControl Where VoucherID=" & Val(txtID.Text) & "")
            Dg1Record()
            MsgBox("Setting Updated For " & cbChargesOn.Text & " Successfully.", vbInformation + vbOKOnly, "Updated")
            cbChargesOn.Focus() : dg1.Rows.Clear() : txtID.Clear()
        End If
    End Sub
    Private Sub Dg1Record()
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            With row
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "" & Val(txtID.Text) & "," &
                "'" & .Cells(0).Value & "','" & Val(.Cells(1).Value) & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "'," &
                "'" & cbChargesOn.Text & "'"
            End With
        Next
        Sql = "Insert Into ExpControl (VoucherID,SRNo,ChargesID,ChargesName,FixAs,ApplyOn) " & FastQuery & ""
        If FastQuery = String.Empty Then Exit Sub
        clsFun.ExecNonQuery(Sql)
    End Sub

    Public Sub FillControls(ByVal id As Integer)
        Dim sSql As String = String.Empty
        Dim Crate As String = String.Empty
        If clsFun.GetConnection.State = ConnectionState.Open Then clsFun.CloseConnection()
        sSql = "Select * from ExpVouchers where ApplyID=" & id
        clsFun.con.Open()
        Dim ad As New SQLite.SQLiteDataAdapter(sSql, clsFun.GetConnection)
        Dim ds As New DataSet
        ad.Fill(ds, "a")
        If ds.Tables("a").Rows.Count > 0 Then
            btnSave.Text = "&Update" : BtnDelete.Enabled = True : btnSave.BackColor = Color.Coral
            cbChargesOn.Text = ds.Tables("a").Rows(0)("ApplyOn").ToString()
            cbChargesOn.SelectedIndex = ds.Tables("a").Rows(0)("ApplyID").ToString()
            txtID.Text = ds.Tables("a").Rows(0)("ID").ToString()
        Else
            btnSave.Text = "&Save" : BtnDelete.Enabled = False
            dg1.Rows.Clear() : txtID.Clear() : btnSave.BackColor = Color.DarkTurquoise
        End If
        Dim sql As String = "Select * from ExpControl where VoucherID=" & Val(txtID.Text)
        Dim ad1 As New SQLite.SQLiteDataAdapter(sql, clsFun.GetConnection)
        ad1.Fill(ds, "b")
        If ds.Tables("b").Rows.Count > 0 Then dg1.Rows.Clear()
        With dg1
            Dim i As Integer = 0
            For i = 0 To ds.Tables("b").Rows.Count - 1
                .Rows.Add()
                .Rows(i).Cells(0).Value = ds.Tables("b").Rows(i)("SRNo").ToString()
                .Rows(i).Cells(1).Value = ds.Tables("b").Rows(i)("ChargesID").ToString()
                .Rows(i).Cells(2).Value = ds.Tables("b").Rows(i)("ChargesName").ToString()
                .Rows(i).Cells(3).Value = ds.Tables("b").Rows(i)("FixAs").ToString()
            Next
        End With
        dg1.ClearSelection()
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnadd.Click
        If dg1.SelectedRows.Count = 1 Then
            dg1.SelectedRows(0).Cells(0).Value = txtSrNo.Text
            dg1.SelectedRows(0).Cells(1).Value = Val(cbCharges.SelectedValue)
            dg1.SelectedRows(0).Cells(2).Value = cbCharges.Text
            dg1.SelectedRows(0).Cells(3).Value = txtCalculate.Text
        Else
            dg1.Rows.Add(txtSrNo.Text, Val(cbCharges.SelectedValue), cbCharges.Text, txtCalculate.Text)
        End If
        txtSrNo.Focus() : dg1.ClearSelection()
    End Sub

    Private Sub btnSave_Click_1(sender As Object, e As EventArgs) Handles btnSave.Click
        If dg1.Rows.Count = 0 Then MsgBox("There is No Charges for Save/Update", MsgBoxStyle.Critical, "Add Charges") : Exit Sub
        If btnSave.Text = "&Save" Then
            Save()
        Else
            UpdateRecord()
        End If

    End Sub

    Private Sub dg1_Click(sender As Object, e As EventArgs) Handles dg1.Click
        If dg1.SelectedRows.Count <> 0 Then dg1.ClearSelection()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 1 Then
                txtSrNo.Text = dg1.SelectedRows(0).Cells(0).Value
                cbCharges.Text = dg1.SelectedRows(0).Cells(2).Value
                txtCalculate.Text = dg1.SelectedRows(0).Cells(3).Value
                txtSrNo.Focus()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 1 Then
            txtSrNo.Text = dg1.SelectedRows(0).Cells(0).Value
            cbCharges.Text = dg1.SelectedRows(0).Cells(2).Value
            txtCalculate.Text = dg1.SelectedRows(0).Cells(3).Value
            txtSrNo.Focus()
        End If
    End Sub
    Private Sub Delete()
        If MessageBox.Show("Are you Sure want to Delete Charges Settings For " & cbChargesOn.Text & "??", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then

            clsFun.ExecNonQuery("Delete From ExpVouchers Where ID=" & Val(txtID.Text) & "; " & _
                           "Delete From ExpControl Where VoucherID=" & Val(txtID.Text) & "")
            MsgBox("Setting Deleted For " & cbChargesOn.Text & " Successfully.", vbInformation + vbOKOnly, "Deelted")
            cbChargesOn.Focus()
            btnSave.Text = "&Save" : BtnDelete.Enabled = False
            dg1.Rows.Clear() : txtID.Clear() : btnSave.BackColor = Color.DarkTurquoise
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Delete()
    End Sub
End Class