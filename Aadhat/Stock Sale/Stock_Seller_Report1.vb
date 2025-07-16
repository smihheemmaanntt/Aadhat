Public Class Stock_Seller_Report1
    Dim Primary, Secondary As String
    Private Sub Stock_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub Stock_Sale_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) from transaction2 where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) from transaction2 where transtype='" & Me.Text & "'")
        CbSearchPrimary.SelectedIndex = 0 : cbSearchSecondary.SelectedIndex = 0
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
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 80
        dg1.Columns(2).Name = "No." : dg1.Columns(2).Width = 60
        dg1.Columns(3).Name = "Vehicle" : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Saller" : dg1.Columns(4).Width = 120
        dg1.Columns(5).Name = "Item" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Lot" : dg1.Columns(6).Width = 80
        dg1.Columns(7).Name = "Customer" : dg1.Columns(7).Width = 120
        dg1.Columns(8).Name = "Nug" : dg1.Columns(8).Width = 60
        dg1.Columns(9).Name = "Kg" : dg1.Columns(9).Width = 60
        dg1.Columns(10).Name = "Rate" : dg1.Columns(10).Width = 60
        dg1.Columns(11).Name = "Per" : dg1.Columns(11).Width = 50
        dg1.Columns(12).Name = "Basic" : dg1.Columns(12).Width = 80
        dg1.Columns(13).Name = "Charges" : dg1.Columns(13).Width = 80
        dg1.Columns(14).Name = "R.Off" : dg1.Columns(14).Width = 60
        dg1.Columns(15).Name = "Total" : dg1.Columns(15).Width = 100
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtTotROff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            txtTotROff.Text = Format(Val(txtTotROff.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(15).Value), "0.00")
        Next
    End Sub

    Private Sub retrive(Optional ByVal Primary As String = "", Optional ByVal Secondary As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Stock_Sale_Report Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' " & Primary & "" & Secondary & "  order by EntryDate,BillNo,Voucherid ")
        'dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' order by EntryDate")
        Dim vchid As Integer = 0
        Try
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
                            .Cells(4).Value = dt.Rows(i)("SallerName").ToString()
                        ElseIf vchid = dt.Rows(i)("Voucherid").ToString() Then
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                            .Cells(4).Value = ""
                        Else
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("SallerName").ToString()
                        End If
                        .Cells(5).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(6).Value = dt.Rows(i)("Lot").ToString()
                        .Cells(7).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(11).Value = dt.Rows(i)("Per").ToString()
                        .Cells(12).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(13).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        .Cells(15).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(14).Value = Format(Val(.Cells(15).Value) - Val(Val(.Cells(13).Value) + Val(.Cells(12).Value)), "0.00")
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

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        CbSearchPrimary.SelectedIndex = 0 : cbSearchSecondary.SelectedIndex = 0
        txtSearchPrimary.Text = "" : txtSearchSecondary.Text = ""
        retrive()
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                Exit Sub
            End If
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            Stock_Sale.MdiParent = MainScreenForm
            Stock_Sale.Show()
            Stock_Sale.FillControls(tmpID)
            If Not Stock_Sale Is Nothing Then
                Stock_Sale.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then
            Exit Sub
        End If
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Stock_Sale.MdiParent = MainScreenForm
        Stock_Sale.Show()
        Stock_Sale.FillControls(tmpID)
        If Not Stock_Sale Is Nothing Then
            Stock_Sale.BringToFront()
        End If
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
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
    Private Sub save()
        clsFun.GetConnection.BeginTransaction()

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
                sql = sql & "insert into Printing(D1,D2,M1,M2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,T1,T2,T3,T4,T5) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells("Date").Value & "','" & .Cells("No.").Value & "','" & .Cells("Saller").Value & "','" & .Cells("Item").Value & "','" & .Cells("Customer").Value & "'," & _
                    "'" & Val(.Cells("Nug").Value) & "','" & Val(.Cells("Kg").Value) & "'," & Val(.Cells("Rate").Value) & ",'" & .Cells("Per").Value & "'," & _
                    "'" & Val(.Cells("Basic").Value) & "'," & Val(.Cells("Charges").Value) & ",'" & .Cells("Total").Value & "','" & txtTotNug.Text & "','" & txtTotweight.Text & "'," & _
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
        Report_Viewer.printReport("\Reports\StockSaleRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_KeyDown(sender As Object, e As KeyEventArgs) Handles MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then btnShow.Focus()
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub


    Private Sub txtSearchPrimary_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchPrimary.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtSearchPrimary.Text.Trim() <> "" Then
                If CbSearchPrimary.SelectedIndex = 0 Then
                    Primary = "And SallerName Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 1 Then
                    Primary = "And AccountName Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 2 Then
                    Primary = "And ItemName Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 3 Then
                    Primary = "And upper(Lot) = upper('" & txtSearchPrimary.Text.Trim() & "')"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 4 Then
                    Primary = "And BillNo Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 5 Then
                    Primary = "And VehicleNo Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 6 Then
                    Primary = "And Nug Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 7 Then
                    Primary = "And Weight Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 8 Then
                    Primary = "And Rate Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 9 Then
                    Primary = "And Amount Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 10 Then
                    Primary = "And Charges Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf CbSearchPrimary.SelectedIndex = 11 Then
                    Primary = "And TotalAmount Like '" & txtSearchPrimary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                End If

            Else
                If txtSearchPrimary.Text.Trim() = "" Then
                    Primary = ""
                    retrive(Nothing, Secondary)
                End If
            End If
        End If
    End Sub


    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub




    Private Sub txtSearchPrimary_TextChanged(sender As Object, e As EventArgs) Handles txtSearchPrimary.TextChanged

    End Sub

    Private Sub txtSearchSecondary_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchSecondary.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtSearchSecondary.Text.Trim() <> "" Then
                If cbSearchSecondary.SelectedIndex = 0 Then
                    Secondary = "And Nug Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 1 Then
                    Secondary = "And Weight Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 2 Then
                    Secondary = "And Rate Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 3 Then
                    Secondary = "And Amount Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 4 Then
                    Secondary = "And Charges Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 5 Then
                    Secondary = "And TotalAmount Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 6 Then
                    Secondary = "And SallerName Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 7 Then
                    Secondary = "And AccountName Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 8 Then
                    Secondary = "And ItemName Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 9 Then
                    Secondary = "And upper(Lot) = upper('" & txtSearchSecondary.Text.Trim() & "')"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 10 Then
                    Secondary = "And BillNo Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                ElseIf cbSearchSecondary.SelectedIndex = 11 Then
                    Secondary = "And VehicleNo Like '" & txtSearchSecondary.Text.Trim() & "%'"
                    retrive(Primary, Secondary)
                End If
            Else
                If txtSearchSecondary.Text.Trim() = "" Then
                    Secondary = ""
                    retrive(Primary, Nothing)
                End If
            End If
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