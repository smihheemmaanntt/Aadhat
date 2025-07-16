Public Class Order_Register

    Private Sub Order_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub CreateTble()
        Dim sql As String = String.Empty
        sql = "CREATE TABLE if not exists OrderBook (ID INTEGER PRIMARY KEY AUTOINCREMENT,OrderNo " & _
            "TEXT,EntryDate DATE,AccountID INTEGER,AccountName TEXT,TotalNug DECIMAL,TotalWeight DECIMAL);"
        clsFun.ExecNonQuery(sql)
    End Sub
    Private Sub Order_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CreateTble()
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) From OrderBook")
        maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From OrderBook")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        rowColums()
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then Exit Sub
            Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
            Order_Book.MdiParent = MainScreenForm
            Order_Book.Show()
            Order_Book.FillControls(tmpID)
            If Not Order_Book Is Nothing Then
                Order_Book.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
        Order_Book.MdiParent = MainScreenForm
        Order_Book.Show()
        Order_Book.FillControls(tmpID)
        If Not Order_Book Is Nothing Then
            Order_Book.BringToFront()
        End If
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub


    Private Sub rowColums()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "Account Name" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Total Nugs" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Total Weight" : dg1.Columns(4).Width = 150
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        ' Dim NewDate As DateTime
        dt = clsFun.ExecDataTable("Select * FROM OrderBook WHERE  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("ID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(3).Value = Format(Val(dt.Rows(i)("TotalNug").ToString()), "0.00")
                        .Cells(4).Value = Format(Val(dt.Rows(i)("TotalWeight").ToString()), "0.00")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Sub calc()
        txtTotalNugs.Text = Format(0, "00000.00") : txtTotalWeight.Text = Format(0, "00000.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotalNugs.Text = Format(Val(txtTotalNugs.Text + Val(dg1.Rows(i).Cells(3).Value)), "0.00")
            txtTotalWeight.Text = Format(Val(txtTotalWeight.Text + Val(dg1.Rows(i).Cells(4).Value)), "0.00")
        Next
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click

    End Sub
End Class