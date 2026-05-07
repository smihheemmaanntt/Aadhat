Public Class Sellout_Item_Summary

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub Market_Tax_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub txtFromDate_GotFocus(sender As Object, e As EventArgs) Handles txtFromDate.GotFocus, txtFromDate.Click
        txtFromDate.SelectAll()
    End Sub
    Private Sub txttoDate_GotFocus(sender As Object, e As EventArgs) Handles txttoDate.GotFocus, txttoDate.Click
        txttoDate.SelectAll()
    End Sub

    Private Sub txtFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFromDate.KeyDown, txttoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub txtFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtFromDate.Validating
        txtFromDate.Text = SmartDate(txtFromDate.Text)
    End Sub

    Private Sub txttoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txttoDate.Validating
        txttoDate.Text = SmartDate(txttoDate.Text, True, 2)
    End Sub
    Private Sub Market_Tax_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from transaction2 ")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from transaction2")
        If mindate <> "" Then
            txtFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            txtFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            txttoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            txttoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 4
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Item Name" : dg1.Columns(1).Width = 800
        dg1.Columns(2).Name = "Nug" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Weight" : dg1.Columns(3).Width = 200
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
        Next
    End Sub
    Private Sub retrive()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("SELECT T1.ItemID,T1.ItemName,Sum(t1.Nug) as Nug,Round(Sum(t1.Weight),2) as Weight FROM Vouchers AS V INNER JOIN Transaction1 AS T1 ON T1.VoucherID = V.ID " & _
                                  "Where EntryDate Between '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' Group by T1.ItemID Order by Upper(T1.ItemName);")
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(1).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(2).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(3).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")

                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub


    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        txttoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        txttoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        txttoDate.Text = smartDate(txttoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        txtFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        txtFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        txtFromDate.Text = smartDate(txtFromDate.Text)
    End Sub

    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                sql = sql & "insert into Printing(D1,D2,P1,P2, P3, P4,P5) values('" & txtFromDate.Text & "','" & txttoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & Val(txtTotNug.Text) & "','" & Val(txtTotweight.Text) & "');"
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
            'clsFun.ExecNonQuery("COMMIT;")
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\SelloutItemSummary.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
    End Sub
End Class