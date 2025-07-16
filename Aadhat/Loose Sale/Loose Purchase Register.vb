Public Class Loose_Purchase_Register

    Private Sub Purchase_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Purchase_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) from PurchaseLoose where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) from PurchaseLoose where transtype='" & Me.Text & "'")
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
        mskFromDate.Focus() : rowColums()
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0
        mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0
        MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown, btnShow.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub mskFromDate_Leave(sender As Object, e As EventArgs) Handles mskFromDate.Leave
        mskFromDate.SelectionStart = 0
        ' mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Leave(sender As Object, e As EventArgs) Handles MsktoDate.Leave
        MsktoDate.SelectionStart = 0
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 15
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "No." : dg1.Columns(2).Width = 80
        dg1.Columns(3).Name = "Vehic.No." : dg1.Columns(3).Width = 80
        dg1.Columns(4).Name = "Item" : dg1.Columns(4).Width = 80
        dg1.Columns(5).Name = "Account" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Nug" : dg1.Columns(6).Width = 80
        dg1.Columns(7).Name = "Weight" : dg1.Columns(7).Width = 80
        dg1.Columns(8).Name = "Rate" : dg1.Columns(8).Width = 80
        dg1.Columns(9).Name = "Per" : dg1.Columns(9).Width = 80
        dg1.Columns(10).Name = "Amount" : dg1.Columns(10).Width = 80
        dg1.Columns(11).Name = "Basic" : dg1.Columns(11).Width = 80
        dg1.Columns(12).Name = "Charges" : dg1.Columns(12).Width = 80
        dg1.Columns(13).Name = "Roff" : dg1.Columns(13).Width = 50
        dg1.Columns(14).Name = "Total" : dg1.Columns(14).Width = 80
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtRoundOff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            txtRoundOff.Text = Format(Val(txtRoundOff.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
            dg1.Rows(i).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            '    dg1.Rows(i).Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear() : Dim vchid As Integer = 0
        Dim dt As New DataTable
        ' dt = clsFun.ExecDataTable("Select * FROM PurchaseLoose WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Transtype='Purchase (Loose)' " & condtion & " order by EntryDate,BillNo")
        dt = clsFun.ExecDataTable("Select * FROM Vouchers v INNER JOIN PurchaseLoose p ON v.id = p.VoucherID INNER JOIN Accounts a1 ON a1.ID = P.AccountID INNER JOIN Items i ON i.ID = p.ItemID Where v.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and v.transtype='" & Me.Text & "' " & condtion & "  order by  V.InvoiceID,v.EntryDate ")
        Dim dvData As DataView = New DataView(dt)
        Try
            If dt.Rows.Count > 0 Then
                '  If dt.Rows.Count > 20 Then dg1.Columns(10).Width = 55 Else dg1.Columns(10).Width = 75
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        If i = 0 Then
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("billNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("ItemName").ToString()
                            .Cells(11).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                            .Cells(12).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                            .Cells(14).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                            .Cells(13).Value = Format(Val(.Cells(14).Value) - Val(Val(.Cells(11).Value) + Val(.Cells(12).Value)), "0.00")
                            lblRecordCount.Visible = True
                            lblRecordCount.Text = "# : " & i + 1
                        ElseIf vchid = dt.Rows(i)("id").ToString() Then
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                            .Cells(4).Value = ""
                            .Cells(11).Value = ""
                            .Cells(12).Value = ""
                            .Cells(14).Value = ""
                        Else
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("billNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("ItemName").ToString()
                            .Cells(11).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                            .Cells(12).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                            .Cells(14).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                            .Cells(13).Value = Format(Val(.Cells(14).Value) - Val(Val(.Cells(11).Value) + Val(.Cells(12).Value)), "0.00")
                            lblRecordCount.Text = "# : " & i + 1
                        End If
                        '  .Cells(0).Value = i + 1
                        .Cells(5).Value = dt.Rows(i)("AccountName1").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Nug1").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Rate1").ToString()), "0.00")
                        .Cells(9).Value = dt.Rows(i)("Per1").ToString()
                        .Cells(10).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    End With
                    vchid = dt.Rows(i)("ID").ToString()
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
            retrive("And SallerName Like '%" & txtAccountName.Text.Trim() & "%'")
        End If
        If txtAccountName.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            Loose_Purchase.MdiParent = MainScreenForm
            Loose_Purchase.Show()
            Loose_Purchase.FillControls(tmpID)
            Loose_Purchase.BringToFront()
        End If
        e.SuppressKeyPress = True

    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Loose_Purchase.MdiParent = MainScreenForm
        Loose_Purchase.Show()
        Loose_Purchase.FillControls(tmpID)
        Loose_Purchase.BringToFront()
    End Sub

    Private Sub txtVechileSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtVechileSearch.KeyUp
        If txtVechileSearch.Text.Trim() <> "" Then
            retrive("And VehicleNo Like '%" & txtVechileSearch.Text.Trim() & "%'")
        End If
        If txtVechileSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub txttype_KeyUp(sender As Object, e As KeyEventArgs) Handles txttype.KeyUp
        If txttype.Text.Trim() <> "" Then
            retrive("And PurchaseType Like '%" & txttype.Text.Trim() & "%'")
        End If
        If txttype.Text.Trim() = "" Then
            retrive()
        End If
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
                sql = sql & "insert into Printing(D1,D2,M1,M2, P1, P2,P3, P4, P5, P6,P7,P8,T1,T2,T3,T4,T5) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(5).Value & "'," & _
                    "'" & Val(.Cells(6).Value) & "','" & Val(.Cells(7).Value) & "'," & _
                    "'" & Val(.Cells(8).Value) & "'," & Val(.Cells(9).Value) & ",'" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & txtTotNug.Text & "','" & txtTotweight.Text & "'," & _
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
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\PurchaseRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Registers_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
End Class