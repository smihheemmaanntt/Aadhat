Public Class Purchase_RegisterDetailed
    Dim strSDate As String
    Dim strEDate As String
    Dim dDate As DateTime
    Dim mskstartDate As String
    Dim mskenddate As String
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub Super_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If dg1.RowCount = 0 Then
                Me.Close()
            Else
                Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "AADHAT")
                If msgRslt = MsgBoxResult.Yes Then
                    Me.Close()
                ElseIf msgRslt = MsgBoxResult.No Then
                End If
            End If
        End If
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
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
    Private Sub Super_Sale_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from Vouchers where transtype='" & Me.Text & "'")
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
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 16
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 80 : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "No." : dg1.Columns(2).Width = 60 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "V.No." : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Saller" : dg1.Columns(4).Width = 150 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(5).Name = "Item" : dg1.Columns(5).Width = 100 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(6).Name = "Lot No" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Customer" : dg1.Columns(7).Visible = False : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(8).Name = "Nug" : dg1.Columns(8).Width = 60 : dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Kg" : dg1.Columns(9).Width = 60 : dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "Rate" : dg1.Columns(10).Width = 80 : dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).Name = "Per" : dg1.Columns(11).Width = 50 : dg1.Columns(11).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(12).Name = "Basic" : dg1.Columns(12).Width = 80 : dg1.Columns(12).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(13).Name = "Charges" : dg1.Columns(13).Width = 80 : dg1.Columns(13).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(13).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(14).Name = "R.Off" : dg1.Columns(14).Width = 80 : dg1.Columns(14).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(14).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(15).Name = "Total" : dg1.Columns(15).Width = 90 : dg1.Columns(15).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(15).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtTotRoundOff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            txtTotRoundOff.Text = Format(Val(txtTotRoundOff.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(15).Value), "0.00")
        Next
    End Sub
    Private Sub txtTotalSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtTotalSearch.KeyUp
        If txtTotalSearch.Text.Trim() <> "" Then
            retrive("And t.TotalAmount Like '" & txtTotalSearch.Text.Trim() & "%'")
        End If
        If txtTotalSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyUp
        If txtItemSearch.Text.Trim() <> "" Then
            retrive("And i.ItemName Like '" & txtItemSearch.Text.Trim() & "%'")
        End If
        If txtItemSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtCustomerSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyDown

    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            retrive("And a.AccountName Like '" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers v INNER JOIN Purchase P ON v.id = P.VoucherID INNER JOIN Accounts a1 ON a1.ID = v.sallerID INNER JOIN Accounts a ON a.ID = P.AccountID INNER JOIN Items i ON i.ID = P.ItemID Where v.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and v.transtype='" & Me.Text & "' " & condtion & "  order by  v.InvoiceID,v.EntryDate ")

        'dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' order by EntryDate")
        Dim vchid As Integer = 0
        Try
            If dt.Rows.Count > 20 Then dg1.Columns(14).Width = 60 Else dg1.Columns(14).Width = 80
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        If i = 0 Then
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("AccountName2").ToString()
                            .Cells(13).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                            .Cells(15).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                            Dim BasicAmount As Decimal = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                            .Cells(14).Value = Format(Val(.Cells(15).Value) - Val(Val(.Cells(13).Value) + Val(BasicAmount)), "0.00")
                            lblRecordCount.Visible = True
                            lblRecordCount.Text = "Record Count : " & i + 1
                        ElseIf vchid = dt.Rows(i)("Voucherid").ToString() Then
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                            .Cells(4).Value = ""
                            lblRecordCount.Text = "Record Count : " & i + 1
                        Else
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("AccountName2").ToString()
                            .Cells(13).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                            .Cells(15).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                            Dim BasicAmount As Decimal = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                            .Cells(14).Value = Format(Val(.Cells(15).Value) - Val(Val(.Cells(13).Value) + Val(BasicAmount)), "0.00")
                        End If
                        .Cells(5).Value = dt.Rows(i)("ItemName2").ToString()
                        .Cells(6).Value = dt.Rows(i)("LotNo").ToString()
                        ' .Cells(7).Value = dt.Rows(i)("AccountName3").ToString()
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Nug1").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("Rate1").ToString()), "0.00")
                        .Cells(11).Value = dt.Rows(i)("Per1").ToString()
                        .Cells(12).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
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
    Private Sub txtAccountName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccountName.KeyUp
        If txtAccountName.Text.Trim() <> "" Then
            retrive("And a1.AccountName Like '" & txtAccountName.Text.Trim() & "%'")
        End If
        If txtAccountName.Text.Trim() = "" Then
            retrive()
        End If
    End Sub


    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub
    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,M1,M2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,T1,T2,T3,T4,T5) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "'," & _
                    "'" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & .Cells(9).Value & "'," & _
                    "'" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "','" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & txtTotNug.Text & "','" & txtTotweight.Text & "'," & _
                    "'" & txtTotBasic.Text & "','" & txtTotCharge.Text & "','" & TxtGrandTotal.Text & "');"
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

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.RowCount = 0 Then btnShow.PerformClick() : Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Purchase.MdiParent = MainScreenForm
        Purchase.Show()
        Purchase.FillControls(tmpID)
        Purchase.BringToFront()

    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then btnShow.PerformClick() : Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            Purchase.MdiParent = MainScreenForm
            Purchase.Show()
            Purchase.FillControls(tmpID)
            Purchase.BringToFront()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\PurchaseRegisterDetailed.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub txtAccountName_TextChanged(sender As Object, e As EventArgs) Handles txtAccountName.TextChanged

    End Sub
End Class