Public Class Group_Payment_Register

    Private Sub RcptRegisterForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub RcptRegisterForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
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
        mskFromDate.Focus()
        rowColums()
    End Sub
    Private Sub MsktoDate_KeyDown(sender As Object, e As KeyEventArgs) Handles MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then btnShow.Focus()
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown
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
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectAll()
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectAll()
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 98
        dg1.Columns(2).Name = "Mode" : dg1.Columns(2).Width = 171
        dg1.Columns(3).Name = "Rcpt No." : dg1.Columns(3).Width = 99
        dg1.Columns(4).Name = "Account Name" : dg1.Columns(4).Width = 210
        dg1.Columns(5).Name = "Remark" : dg1.Columns(5).Width = 487
        dg1.Columns(6).Name = "Amount" : dg1.Columns(6).Width = 100

    End Sub
    Sub calc()
        Dim i As Integer
        txtTotBasic.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
        Next
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable : Dim vchid As Integer
        dt = clsFun.ExecDataTable("Select * FROM Vouchers v INNER JOIN Transaction3 t ON v.id = t.VoucherID  WHERE v.EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & " and v.transtype='" & Me.Text & "'Order By v.EntryDate")
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        If i = 0 Then
                            .Cells(0).Value = dt.Rows(i)("id").ToString()
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("SallerName").ToString()
                            .Cells(3).Value = dt.Rows(i)("BillNo").ToString()
                        ElseIf vchid = dt.Rows(i)("Voucherid").ToString() Then
                            .Cells(0).Value = dt.Rows(i)("id").ToString()
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                        Else
                            .Cells(0).Value = dt.Rows(i)("id").ToString()
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("SallerName").ToString()
                            .Cells(3).Value = dt.Rows(i)("BillNo").ToString()
                        End If
                        .Cells(4).Value = dt.Rows(i)("AccountName1").ToString()
                        .Cells(5).Value = dt.Rows(i)("Remark1").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                    vchid = dt.Rows(i)("Voucherid").ToString()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub txtCustomerSearch_GotFocus(sender As Object, e As EventArgs) Handles txtCustomerSearch.GotFocus
        txtCustomerSearch.Clear() : txtCustomerSearch.ForeColor = Color.Teal
        txtDiscSearch.Text = "Search Here..." : txtDiscSearch.ForeColor = Color.Gray
        txtNetSearch.Text = "Search Here..." : txtNetSearch.ForeColor = Color.Gray
        txtTotalSearch.Text = "Search Here..." : txtTotalSearch.ForeColor = Color.Gray

    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            retrive(" And AccountName Like '%" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtNetSearch_GotFocus(sender As Object, e As EventArgs) Handles txtNetSearch.GotFocus
        txtNetSearch.Clear() : txtNetSearch.ForeColor = Color.Teal
        txtDiscSearch.Text = "Search Here..." : txtDiscSearch.ForeColor = Color.Gray
        txtCustomerSearch.Text = "Search Here..." : txtCustomerSearch.ForeColor = Color.Gray
        txtTotalSearch.Text = "Search Here..." : txtTotalSearch.ForeColor = Color.Gray
    End Sub
    Private Sub txtNetSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNetSearch.KeyUp
        If txtNetSearch.Text.Trim() <> "" Then
            retrive(" And BasicAmount Like '%" & txtNetSearch.Text.Trim() & "%'")
        End If
        If txtNetSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtDiscSearch_GotFocus(sender As Object, e As EventArgs) Handles txtDiscSearch.GotFocus
        txtDiscSearch.Clear() : txtDiscSearch.ForeColor = Color.Teal
        txtNetSearch.Text = "Search Here..." : txtNetSearch.ForeColor = Color.Gray
        txtCustomerSearch.Text = "Search Here..." : txtCustomerSearch.ForeColor = Color.Gray
        txtTotalSearch.Text = "Search Here..." : txtTotalSearch.ForeColor = Color.Gray
    End Sub
    Private Sub txtDiscSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDiscSearch.KeyUp
        If txtDiscSearch.Text.Trim() <> "" Then
            retrive(" And DiscountAmount Like '%" & txtDiscSearch.Text.Trim() & "%'")
        End If
        If txtDiscSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtTotalSearch_GotFocus(sender As Object, e As EventArgs) Handles txtTotalSearch.GotFocus
        txtTotalSearch.Clear() : txtTotalSearch.ForeColor = Color.Teal
        txtNetSearch.Text = "Search Here..." : txtNetSearch.ForeColor = Color.Gray
        txtCustomerSearch.Text = "Search Here..." : txtCustomerSearch.ForeColor = Color.Gray
        txtDiscSearch.Text = "Search Here..." : txtDiscSearch.ForeColor = Color.Gray
    End Sub
    Private Sub txtTotalSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtTotalSearch.KeyUp
        If txtTotalSearch.Text.Trim() <> "" Then
            retrive(" And TotalAmount Like '%" & txtTotalSearch.Text.Trim() & "%'")
        End If
        If txtTotalSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then Exit Sub
            Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
            Group_Payment.MdiParent = MainScreenForm
            Group_Payment.Show()
            Group_Payment.FillControls(tmpID)
            Group_Payment.BringToFront()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
        Group_Payment.MdiParent = MainScreenForm
        Group_Payment.Show()
        Group_Payment.FillControls(tmpID)
        Group_Payment.BringToFront()
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,T1) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells("Date").Value & "','" & .Cells("Mode").Value & "','" & .Cells("Rcpt No.").Value & "','" & .Cells("Account Name").Value & "'," & _
                    "'" & Format(Val(.Cells("Amount").Value), "0.00") & "'," & Format(Val(.Cells("Discount").Value), "0.00") & ",'" & Format(Val(.Cells("Total").Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells("Remark").Value), "0.00") & "'," & Format(Val(txtTotBasic.Text), "0.00") & ")"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        'clsFun.changeCompany()
        PrintRecord()
        Report_Viewer.printReport("\Reports\ReceiptRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub txtCustomerSearch_Leave(sender As Object, e As EventArgs) Handles txtCustomerSearch.Leave
        If txtCustomerSearch.Text = "" Then
            txtCustomerSearch.Text = "Search Here..."
            txtCustomerSearch.ForeColor = Color.Gray
        End If

    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub

    Private Sub txtNetSearch_Leave(sender As Object, e As EventArgs) Handles txtNetSearch.Leave
        If txtNetSearch.Text = "" Then
            txtNetSearch.Text = "Search Here..."
            txtNetSearch.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub txtDiscSearch_Leave(sender As Object, e As EventArgs) Handles txtDiscSearch.Leave
        If txtDiscSearch.Text = "" Then
            txtDiscSearch.Text = "Search Here..."
            txtDiscSearch.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub txtTotalSearch_Leave(sender As Object, e As EventArgs) Handles txtTotalSearch.Leave
        If txtTotalSearch.Text = "" Then
            txtTotalSearch.Text = "Search Here..."
            txtTotalSearch.ForeColor = Color.Gray
        End If
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub lblName_Click(sender As Object, e As EventArgs) Handles lblName.Click

    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class