Public Class Sell_Out_Pending_Bills

    Private Sub Supplier_Statement_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Supplier_Statement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums()
        mindate = clsFun.ExecScalarStr("Select Min(entrydate) from Vouchers Where TransType='Purchase' ")
        maxdate = clsFun.ExecScalarStr("Select Max(entrydate) from Vouchers Where TransType='Purchase' ")
        If mindate <> "" Then mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy") Else mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        If maxdate <> "" Then MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy") Else MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 8
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 150
        dg1.Columns(2).Name = "AccountID" : dg1.Columns(2).Visible = False
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 450
        dg1.Columns(4).Name = "Vehicle No" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Purchase Nugs" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Sold Nugs" : dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "Amount" : dg1.Columns(7).Width = 150
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtAccount.Text.Trim() <> "" Then
                retrive(" and AccountName Like '" & txtAccount.Text.Trim() & "%'")
            Else
                retrive()
            End If
        End If

    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()
    End Sub

    Private Sub Retrive(Optional ByVal condtion As String = "")
        Application.DoEvents()
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim sql As String = String.Empty
        sql = " Select EntryDate,AccountID,AccountName,VoucherID,VehicleNo,sum(Nug) as Nug,ifnull((Select PurchaseID From Transaction1 Where PurchaseID=Purchase.VoucherID),0) as SaleVoucherID, " & _
              " ifnull((Select Sum(Nug) From Transaction2 Where PurchaseID=Purchase.VoucherID),0) as SoldNug ,ifnull((Select Sum(SallerAmt) From Transaction2 Where PurchaseID=Purchase.VoucherID),0) as Amount " & _
              " From Purchase WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & _
              " and PurchaseTypeName='Stock In' " & condtion & " Group By VoucherID Having SaleVoucherID=0;"
        dt = clsFun.ExecDataTable(sql)
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).tostring("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("VehicleNo").ToString()
                        .Cells(5).Value = dt.Rows(i)("Nug").ToString()
                        .Cells(6).Value = dt.Rows(i)("SoldNug").ToString()
                        .Cells(7).Value = dt.Rows(i)("Amount").ToString()
                    End With
                Next
            End If
        Catch ex As Exception

        End Try
        dt = clsFun.ExecDataTable(sql)
        dg1.ClearSelection()
    End Sub
    Private Sub PrintRecord()
        pnlWait.Visible = True
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            pb1.Minimum = 0
            pb1.Maximum = dg1.RowCount
            Application.DoEvents()
            With row
                pb1.Value = IIf(Val(row.Index) < 0, 0, Val(row.Index))
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5,P6) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
            "'" & .Cells(1).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & Format(Val(.Cells(5).Value), "0.00") & "'," & _
            "'" & Format(Val(.Cells(6).Value), "0.00") & "','" & Format(Val(.Cells(7).Value), "0.00") & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
        pnlWait.Visible = False
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        PrintRecord()
        Report_Viewer.printReport("\Reports\PendingSellout.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
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

    Private Sub btnShow_Click_1(sender As Object, e As EventArgs) Handles btnShow.Click

    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = Val(dg1.SelectedRows(0).Cells(0).Value)
        Sellout_Auto.MdiParent = MainScreenForm
        Sellout_Auto.Show()
        Sellout_Auto.mskEntryDate.Text = dg1.SelectedRows(0).Cells(1).Value
        Sellout_Auto.txtAccountID.Text = Val(dg1.SelectedRows(0).Cells(2).Value)
        Sellout_Auto.txtAccount.Text = dg1.SelectedRows(0).Cells(3).Value
        Sellout_Auto.txtVehicleID.Text = Val(dg1.SelectedRows(0).Cells(0).Value)
        Sellout_Auto.txtVehicle.Text = dg1.SelectedRows(0).Cells(4).Value
        Sellout_Auto.FillControls(tmpID)
        Sellout_Auto.txtVehicle.Focus()
        If Not Sellout_Auto Is Nothing Then Sellout_Auto.BringToFront()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = Val(dg1.SelectedRows(0).Cells(0).Value)
            Sellout_Auto.MdiParent = MainScreenForm
            Sellout_Auto.Show()
            Sellout_Auto.mskEntryDate.Text = dg1.SelectedRows(0).Cells(1).Value
            Sellout_Auto.txtAccountID.Text = Val(dg1.SelectedRows(0).Cells(2).Value)
            Sellout_Auto.txtAccount.Text = dg1.SelectedRows(0).Cells(3).Value
            Sellout_Auto.txtVehicleID.Text = Val(dg1.SelectedRows(0).Cells(0).Value)
            Sellout_Auto.txtVehicle.Text = dg1.SelectedRows(0).Cells(4).Value
            Sellout_Auto.FillControls(tmpID)
            Sellout_Auto.txtVehicle.Focus()
            If Not Sellout_Auto Is Nothing Then Sellout_Auto.BringToFront()
            e.SuppressKeyPress = True
        End If
    End Sub
End Class