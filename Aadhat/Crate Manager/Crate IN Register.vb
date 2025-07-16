Public Class Crate_IN_Register

    Private Sub Crate_IN_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Crate_IN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from crateVoucher where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(EntryDate) as entrydate from crateVoucher where transtype='" & Me.Text & "'")
        If mindate <> "" Then mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy") Else mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        If maxdate <> "" Then MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy") Else MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 150
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 350
        dg1.Columns(4).Name = "Crate Name" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Qty In" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Estimate" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Remark" : dg1.Columns(7).Width = 250
        dg1.Columns(8).Name = "OtherName" : dg1.Columns(8).Visible = False
    End Sub
    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim id As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
        Dim type As String = dg1.SelectedRows(0).Cells(2).Value
        If type = "Op Bal" Then MsgBox("Can't Modify Opening Balance Crates", MsgBoxStyle.Critical, "Stop") : Exit Sub
        If type = "Stock Sale" Then
            Stock_Sale.MdiParent = MainScreenForm
            Stock_Sale.Show()
            Stock_Sale.FillControls(id)
            If Not Stock_Sale Is Nothing Then
                Stock_Sale.BringToFront()
            End If
        ElseIf type = "Receipt" Then
            ReceiptForm.MdiParent = MainScreenForm
            ReceiptForm.Show()
            ReceiptForm.FillControls(id)
            If Not ReceiptForm Is Nothing Then
                ReceiptForm.BringToFront()
            End If
        ElseIf type = "Payment" Then
            PayMentform.MdiParent = MainScreenForm
            PayMentform.Show()
            PayMentform.FillControls(id)
            If Not PayMentform Is Nothing Then
                PayMentform.BringToFront()
            End If
        ElseIf type = "Purchase" Then
            Purchase.MdiParent = MainScreenForm
            Purchase.Show()
            Purchase.FillControls(id)
            If Not Purchase Is Nothing Then
                Purchase.BringToFront()
            End If
        ElseIf type = "Super Sale" Then
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(id)
            If Not Super_Sale Is Nothing Then
                Super_Sale.BringToFront()
            End If
        ElseIf type = "Speed Sale" Then
            SpeedSale.MdiParent = MainScreenForm
            SpeedSale.Show()
            SpeedSale.FillContros(id)
            If Not SpeedSale Is Nothing Then
                SpeedSale.BringToFront()
            End If
        ElseIf type = "Standard Sale" Then
            Standard_Sale.MdiParent = MainScreenForm
            Standard_Sale.Show()
            Standard_Sale.FillControls(id)
            If Not Standard_Sale Is Nothing Then
                Standard_Sale.BringToFront()
            End If
        ElseIf type = "Super Sale" Then
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(id)
            If Not Super_Sale Is Nothing Then
                Super_Sale.BringToFront()
            End If
        ElseIf type = "Auto Beejak" Then
            Sellout_Auto.MdiParent = MainScreenForm
            Sellout_Auto.Show()
            Sellout_Auto.FillFromData(id)
            If Not Sellout_Auto Is Nothing Then
                Sellout_Auto.BringToFront()
            End If
        ElseIf type = "Beejak" Then
            Sellout_Mannual.MdiParent = MainScreenForm
            Sellout_Mannual.Show()
            Sellout_Mannual.FillContros(id)
            Sellout_Mannual.BringToFront()
        ElseIf type = "Crate In" Then
            Crate_IN.MdiParent = MainScreenForm
            Crate_IN.Show()
            Crate_IN.FillControls(id)
            If Not Crate_IN Is Nothing Then
                Crate_IN.BringToFront()
            End If
        ElseIf type = "Crate Out" Then
            Crate_Out.MdiParent = MainScreenForm
            Crate_Out.Show()
            Crate_Out.FillControls(id)
            If Not Crate_Out Is Nothing Then
                Crate_Out.BringToFront()
            End If
        End If

    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Dim id As Integer = Val(dg1.SelectedRows(0).Cells(0).Value)
            Dim type As String = dg1.SelectedRows(0).Cells(2).Value
            If type = "Op Bal" Then MsgBox("Can't Modify Opening Balance Crates", MsgBoxStyle.Critical, "Stop") : Exit Sub
            If type = "Stock Sale" Then
                Stock_Sale.MdiParent = MainScreenForm
                Stock_Sale.Show()
                Stock_Sale.FillControls(id)
                If Not Stock_Sale Is Nothing Then
                    Stock_Sale.BringToFront()
                End If
            ElseIf type = "Receipt" Then
                ReceiptForm.MdiParent = MainScreenForm
                ReceiptForm.Show()
                ReceiptForm.FillControls(id)
                If Not ReceiptForm Is Nothing Then
                    ReceiptForm.BringToFront()
                End If
            ElseIf type = "Payment" Then
                PayMentform.MdiParent = MainScreenForm
                PayMentform.Show()
                PayMentform.FillControls(id)
                If Not PayMentform Is Nothing Then
                    PayMentform.BringToFront()
                End If
            ElseIf type = "Purchase" Then
                Purchase.MdiParent = MainScreenForm
                Purchase.Show()
                Purchase.FillControls(id)
                If Not Purchase Is Nothing Then
                    Purchase.BringToFront()
                End If
            ElseIf type = "Super Sale" Then
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.Show()
                Super_Sale.FillControls(id)
                If Not Super_Sale Is Nothing Then
                    Super_Sale.BringToFront()
                End If
            ElseIf type = "Speed Sale" Then
                SpeedSale.MdiParent = MainScreenForm
                SpeedSale.Show()
                SpeedSale.FillContros(id)
                If Not SpeedSale Is Nothing Then
                    SpeedSale.BringToFront()
                End If
            ElseIf type = "Standard Sale" Then
                Standard_Sale.MdiParent = MainScreenForm
                Standard_Sale.Show()
                Standard_Sale.FillControls(id)
                If Not Standard_Sale Is Nothing Then
                    Standard_Sale.BringToFront()
                End If
            ElseIf type = "Super Sale" Then
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.Show()
                Super_Sale.FillControls(id)
                If Not Super_Sale Is Nothing Then
                    Super_Sale.BringToFront()
                End If
            ElseIf type = "Auto Beejak" Then
                Sellout_Auto.MdiParent = MainScreenForm
                Sellout_Auto.Show()
                Sellout_Auto.FillFromData(id)
                If Not Sellout_Auto Is Nothing Then
                    Sellout_Auto.BringToFront()
                End If
            ElseIf type = "Beejak" Then
                Sellout_Mannual.MdiParent = MainScreenForm
                Sellout_Mannual.Show()
                Sellout_Mannual.FillContros(id)
                If Not Sellout_Mannual Is Nothing Then
                    Sellout_Mannual.BringToFront()
                End If
            ElseIf type = "Crate In" Then
                Crate_IN.MdiParent = MainScreenForm
                Crate_IN.Show()
                Crate_IN.FillControls(id)
                If Not Crate_IN Is Nothing Then
                    Crate_IN.BringToFront()
                End If
            ElseIf type = "Crate Out" Then
                Crate_Out.MdiParent = MainScreenForm
                Crate_Out.Show()
                Crate_Out.FillControls(id)
                If Not Crate_Out Is Nothing Then
                    Crate_Out.BringToFront()
                End If
            End If

        End If

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Sub calc()
        TxtGrandTotal.Text = Format(0, "0.00") : txtTotEstimateOut.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotEstimateOut.Text = Format(Val(txtTotEstimateOut.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
        Next
        lblRecordCount.Visible = True
        lblRecordCount.Text = "Record Count : " & dg1.RowCount
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        ' dt = clsFun.ExecDataTable("Select * from vouchers where entrydate='" & mskFromDate.Text & "' and MsktoDate='" & mskFromDate.Text & "'and TransType='" & Me.Text & "'")
        If CkHideOpBal.Checked = True Then
            dt = clsFun.ExecDataTable("Select *,sum(qty)  as TotQty FROM CrateVoucher WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Cratetype = 'Crate In' and TransType<>'Op Bal' " & condtion & " Group by EntryDate,AccountID,CrateID Order by EntryDate,upper(AccountName),Upper(CrateName)")

        Else
            dt = clsFun.ExecDataTable("Select *,sum(qty)  as TotQty FROM CrateVoucher WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Cratetype = 'Crate In' " & condtion & " Group by EntryDate,AccountID,CrateID Order by EntryDate,upper(AccountName),Upper(CrateName)")
        End If
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).tostring("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(5).Value = dt.Rows(i)("TotQty").ToString()
                        Dim cost As Double = Format(Val(clsFun.ExecScalarStr("Select Rate From CrateMarka Where ID='" & Val(dt.Rows(i)("CrateID").ToString()) & "'")), "0.00")
                        .Cells(6).Value = Format(Val(.Cells(5).Value) * cost, "0.00")
                        .Cells(7).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(8).Value = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID='" & dt.Rows(i)("AccountID").ToString() & "' ")
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

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectAll()
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectAll()
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
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
                sql = "insert into Printing(M1,D1, P1, P2,P3, P4, P5,P6,P7,P8,P9,P10) values('" & .Cells(0).Value & "','" & mskFromDate.Text & "'," & _
                         "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                         "'" & Format(Val(.Cells(5).Value), "0.00") & "','" & .Cells(6).Value & "'," & Format(Val(TxtGrandTotal.Text), "0.00") & ", " & _
                         "'" & .Cells(7).Value & "','" & .Cells(8).Value & "'," & Format(Val(txtTotEstimateOut.Text), "0.00") & ")"
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
        Report_Viewer.printReport("\Reports\InRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
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
    Private Sub txtMarka_KeyUp(sender As Object, e As KeyEventArgs) Handles txtMarka.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtMarka.Text.Trim() <> "" Then
                retrive(" and CrateName Like '" & txtMarka.Text.Trim() & "%'")
            Else
                retrive()
            End If
        End If

    End Sub
End Class