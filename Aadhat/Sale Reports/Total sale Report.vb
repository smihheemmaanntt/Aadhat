Public Class Total_sale_Report
    Dim SearchText As String = "" : Dim TotalPages As Integer = 0
    Dim PageNumber As Integer = 0 : Dim RowCount As Integer = 20
    Dim Offset As Integer = 0 : Dim totNugs As Integer = 0
    Dim totWeight As Integer = 0 : Dim totBasic As Integer = 0
    Dim totCharges As Integer = 0 : Dim totTotal As Integer = 0
    Dim Roundoff As Integer = 0
    Dim Primary, Secondary, third As String
    Private Sub Speed_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
        If dg1.Focused = False Then
            If e.KeyCode = 109 Then btnPrevious.PerformClick()
            If e.KeyCode = Keys.Add Then btnNext.PerformClick()
        End If

    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.Select(0, 0)
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub
    Private Sub Speed_Sale_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Control.CheckForIllegalCrossThreadCalls = False
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
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
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text) : MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 13
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 95
        dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Item Name" : dg1.Columns(2).Width = 150
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "Customer" : dg1.Columns(3).Width = 200
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).Name = "Nug" : dg1.Columns(4).Width = 80
        dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).Name = "Kg" : dg1.Columns(5).Width = 80
        dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).Name = "Rate" : dg1.Columns(6).Width = 80
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).Name = "Per" : dg1.Columns(7).Width = 70
        dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(8).Name = "Net" : dg1.Columns(8).Width = 80
        dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Charges" : dg1.Columns(9).Width = 80
        dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "R. Off" : dg1.Columns(10).Width = 80
        dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).Name = "Total" : dg1.Columns(11).Width = 100
        dg1.Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(12).Name = "Crate" : dg1.Columns(12).Width = 80
        dg1.Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtTotalRoff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotalRoff.Text = Format(Val(txtTotalRoff.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
        Next
        lblRecordCount.Visible = True
        lblRecordCount.Text = "Record Count : " & dg1.RowCount

    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "", Optional ByVal condtion1 As String = "", Optional ByVal condtion2 As String = "")
        dg1.Rows.Clear()
        Dim dt, dt1 As New DataTable
        '  Dim NewDate As DateTime
        ' Dim duration As TimeSpan
        Dim StartTime As DateTime = DateTime.Now
        'Call the database here and execute your SQL statement
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & "")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & " LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString())
        lblTotalRecord.Text = "Total Pages : " & TotalPages : lblTotalRecord.Visible = True
        lblPageNumber.Text = "Page No. : " & (Offset / RowCount) + 1 : lblPageNumber.Visible = True
        totNugs = Format(clsFun.ExecScalarInt("Select Sum(Nug) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lbltotNug.Visible = True
        totWeight = Format(clsFun.ExecScalarInt("Select Sum(Weight) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblTotalWeight.Visible = True
        totBasic = Format(clsFun.ExecScalarInt("Select Sum(Amount) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblBasic.Visible = True
        totCharges = Format(clsFun.ExecScalarInt("Select Sum(Charges) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblCharges.Visible = True
        Roundoff = Format(clsFun.ExecScalarInt("Select Sum(RoundOff) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblTotal.Visible = True
        totTotal = Format(clsFun.ExecScalarInt("Select Sum(TotalAmount) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblTotal.Visible = True
        lbltotNug.Text = "Nug  : " & Format(totNugs, "0.00") : lblTotalWeight.Text = "Weight : " & Format(totWeight, "0.00")
        lblBasic.Text = "Basic : " & Format(totBasic, "0.00") : lblCharges.Text = "Charges : " & Format(totCharges, "0.00")
        lblTotal.Text = "Total : " & Format(totTotal, "0.00") : lblRounfOff.Text = "R. Off : " & Format(Roundoff, "0.00")
        'Offset += dt.Rows.Count()
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        ' Threading.Thread.Sleep(1)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = dt.Rows(i)("Per").ToString()
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(12).Value = dt.Rows(i)("CrateQty").ToString()
                        .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        'Dim percentage As Double = (i / dt.Rows.Count) * 100
                        'ProgressBar1.Value = Int32.Parse(Math.Truncate(percentage).ToString())
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        lblTotalRecordCount.Text = "Total Record : " & recordsCount : lblTotalRecordCount.Visible = True
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Primary = "" : Secondary = "" : third = ""
        txtPrimarySearch.Text = "" : txtSecondarySearch.Text = "" : txtThirdSearch.Text = ""
        retrive() ':  BackgroundWorker1.RunWorkerAsync() 
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            SpeedSale.MdiParent = MainScreenForm
            SpeedSale.Show()
            SpeedSale.FillContros(tmpID)
            If Not SpeedSale Is Nothing Then
                SpeedSale.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        SpeedSale.MdiParent = MainScreenForm
        SpeedSale.Show()
        SpeedSale.FillContros(tmpID)
        If Not SpeedSale Is Nothing Then
            SpeedSale.BringToFront()
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
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,T1,T2,T3,T4,T5,T6) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells("Date").Value & "','" & .Cells("Item Name").Value & "','" & .Cells("Customer").Value & "','" & Format(Val(.Cells("Nug").Value), "0.00") & "'," & _
                    "'" & Format(Val(.Cells("Kg").Value), "0.00") & "'," & Format(Val(.Cells("Rate").Value), "0.00") & ",'" & .Cells("Per").Value & "'," & _
                    "'" & Format(Val(.Cells("Net").Value), "0.00") & "'," & Format(Val(.Cells("Charges").Value), "0.00") & ",'" & Format(Val(.Cells("Total").Value), "0.00") & "'," & Format(Val(txtTotNug.Text), "0.00") & "," & _
                    " " & Format(Val(txtTotweight.Text), "0.00") & "," & Format(Val(txtTotBasic.Text), "0.00") & "," & Format(Val(txtTotCharge.Text), "0.00") & "," & Format(Val(txtTotalRoff.Text), "0.00") & "," & Format(Val(TxtGrandTotal.Text), "0.00") & ")"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub
    Private Sub PrintRecord2(Optional ByVal condtion As String = "")
        Dim i As Integer = 0
        Dim dt As New DataTable
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & SearchText & "")
        ProgressBar1.Maximum = dt.Rows.Count - 1
        ProgressBar1.Visible = True
        For i = 0 To dt.Rows.Count - 1
            sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,T1,T2,T3,T4,T5,T6) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy") & "','" & dt.Rows(i)("ItemName").ToString() & "'," & _
                    "'" & dt.Rows(i)("AccountName").ToString() & "','" & Format(Val(dt.Rows(i)("Nug").ToString()), "0.00") & "'," & _
                    "'" & Format(Val(dt.Rows(i)("Weight").ToString()), "0.00") & "','" & Format(Val(dt.Rows(i)("Rate").ToString()), "0.00") & "'," & _
                    "'" & dt.Rows(i)("Per").ToString() & "','" & Format(Val(dt.Rows(i)("Amount").ToString()), "0.00") & "'," & _
                    "'" & Format(Val(dt.Rows(i)("Charges").ToString()), "0.00") & "','" & Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00") & "'," & _
                    "'" & Format(Val(totNugs), "0.00") & "','" & Format(Val(totWeight), "0.00") & "'," & _
                    "'" & Format(Val(totBasic), "0.00") & "','" & Format(Val(totCharges), "0.00") & "','" & Format(Val(Roundoff), "0.00") & "','" & Format(Val(totTotal), "0.00") & "')"
            ClsFunPrimary.ExecNonQuery(sql)
            ProgressBar1.Value = i
        Next
        ProgressBar1.Value = False
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        pnlprint.Visible = True : radioCurrent.Focus()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        ' mskFromDate.Clear()
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        '   MsktoDate.Clear()
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub txtCustomerSearch_GotFocus(sender As Object, e As EventArgs) Handles txtPrimarySearch.GotFocus

    End Sub

    Private Sub txtCustomerSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPrimarySearch.KeyDown

    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPrimarySearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtPrimarySearch.Text.Trim() <> "" Then
                Primary = "And AccountName Like '" & txtPrimarySearch.Text.Trim() & "%'"
                Offset = 0
                retrive(Primary, Secondary, third)
            End If
            If txtPrimarySearch.Text.Trim() = "" Then
                Offset = 0
                retrive()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub txtItemSearch_GotFocus(sender As Object, e As EventArgs) Handles txtSecondarySearch.GotFocus

    End Sub

    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSecondarySearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtSecondarySearch.Text.Trim() <> "" Then
                Secondary = "And ItemName Like '" & txtSecondarySearch.Text.Trim() & "%'"
                Offset = 0
                retrive(Primary, Secondary, third)
            End If
            If txtSecondarySearch.Text.Trim() = "" Then
                Primary = "" : Secondary = "" : third = ""
                Offset = 0
                retrive()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub



    Private Sub txtTotalSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtThirdSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtThirdSearch.Text.Trim() <> "" Then
                third = "And TotalAmount Like '" & txtThirdSearch.Text.Trim() & "%'"
                Offset = 0
                retrive(Primary, Secondary, third)
                PrintRecord2(SearchText)
            End If
            If txtThirdSearch.Text.Trim() = "" Then
                Primary = "" : Secondary = "" : third = ""
                Offset = 0
                retrive()
                PrintRecord2(Primary)
            End If
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub txtTotalSearch_Leave(sender As Object, e As EventArgs) Handles txtThirdSearch.Leave

    End Sub

    Private Sub txtCustomerSearch_Leave(sender As Object, e As EventArgs) Handles txtPrimarySearch.Leave

    End Sub

    Private Sub txtItemSearch_Leave(sender As Object, e As EventArgs) Handles txtSecondarySearch.Leave

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        Offset = 0
        retrive(Primary, Secondary, third)
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Offset -= RowCount
        If Offset <= 0 Then
            Offset = 0
        End If
        retrive(Primary, Secondary, third)
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Dim currentPage As Integer = (Offset / RowCount) + 1
        If currentPage <> TotalPages Then
            Offset += RowCount
        End If
        retrive(Primary, Secondary, third)
    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        Offset = (TotalPages - 1) * RowCount
        retrive(Primary, Secondary, third)
    End Sub

    Private Sub radioCurrent_CheckedChanged(sender As Object, e As EventArgs) Handles radioCurrent.CheckedChanged

    End Sub

    Private Sub btnPrintNew_Click(sender As Object, e As EventArgs) Handles btnPrintNew.Click
        If radioCurrent.Checked = True Then PrintRecord()
        If radioAll.Checked = True Then PrintRecord2()
        'PrintRecord()
        Report_Viewer.printReport("\Reports\SpeedSaleRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Registers_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
        pnlprint.Visible = False
    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub Panel4_Paint(sender As Object, e As PaintEventArgs)

    End Sub

    Private Sub mskFromDate_Leave(sender As Object, e As EventArgs) Handles mskFromDate.Leave

    End Sub

    Private Sub dg1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub

    Private Sub txtItemSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSecondarySearch.TextChanged

    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtPrimarySearch.TextChanged

    End Sub
End Class