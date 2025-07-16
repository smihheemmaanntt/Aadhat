Public Class Sellout_Mannual_Detialed_Register
    Dim strSDate As String
    Dim strEDate As String
    Dim dDate As DateTime
    Dim mskstartDate As String
    Dim mskenddate As String
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub

    Private Sub Scrip_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
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
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs)
        dg1.ClearSelection()
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Scrip_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) as EntryDate From Vouchers where TransType='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(EntryDate) as EntryDate From Vouchers where TransType='" & Me.Text & "'")
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
        rowColums1()
    End Sub

    Private Sub rowColums1()
        dg1.ColumnCount = 15
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "No" : dg1.Columns(2).Width = 50
        dg1.Columns(3).Name = "Veh. No" : dg1.Columns(3).Width = 80
        dg1.Columns(4).Name = "Account Name" : dg1.Columns(4).Width = 170
        dg1.Columns(5).Name = "Item Name" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Nug" : dg1.Columns(6).Width = 80
        dg1.Columns(7).Name = "Kg" : dg1.Columns(7).Width = 80
        dg1.Columns(8).Name = "Rate" : dg1.Columns(8).Width = 80
        dg1.Columns(9).Name = "Per" : dg1.Columns(9).Width = 60
        dg1.Columns(10).Name = "Net" : dg1.Columns(10).Width = 100
        dg1.Columns(11).Name = "Charges" : dg1.Columns(11).Width = 100
        dg1.Columns(12).Name = "ROff" : dg1.Columns(12).Visible = False
        dg1.Columns(13).Name = "Total" : dg1.Columns(13).Width = 120
        dg1.Columns(14).Name = "OtherHindiName" : dg1.Columns(14).Visible = False
    End Sub
    Public Sub retrive(Optional ByVal Primary As String = "", Optional ByVal Secondary As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        ' dt = clsFun.ExecDataTable("Select * FROM Stock_Sale_Report Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' " & Primary & "" & Secondary & "  order by EntryDate,BillNo,Voucherid ")
        dt = clsFun.ExecDataTable("Select * FROM Vouchers v   INNER JOIN   Transaction1 t ON v.id = t.VoucherID LEFT JOIN Accounts a ON t.AccountID = a.ID  WHERE V.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and V.transtype='" & Me.Text & "' " & Primary & "" & Secondary & "  order by V.EntryDate")
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
                            .Cells(11).Value = IIf(Val(dt.Rows(i)("TotalCharges").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00"))
                            .Cells(12).Value = "" ' IIf(Val(dt.Rows(i)("TotalCharges").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00"))
                            .Cells(13).Value = IIf(Val(dt.Rows(i)("TotalAmount").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00"))
                        ElseIf vchid = dt.Rows(i)("Voucherid").ToString() Then
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                            .Cells(4).Value = ""
                            .Cells(14).Value = ""
                            '  .Cells(15).Value = ""
                        Else
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("SallerName").ToString()
                            .Cells(11).Value = IIf(Val(dt.Rows(i)("TotalCharges").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00"))
                            .Cells(13).Value = IIf(Val(dt.Rows(i)("TotalAmount").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00"))
                        End If
                        .Cells(5).Value = dt.Rows(i)("ItemName1").ToString()
                        .Cells(6).Value = IIf(Val(dt.Rows(i)("Nug1").ToString()) = 0, "", Format(Val(dt.Rows(i)("Nug1").ToString()), "0.00"))
                        .Cells(7).Value = IIf(Val(dt.Rows(i)("Weight").ToString()) = 0, "", Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")) 'Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Rate1").ToString()), "0.00")
                        .Cells(9).Value = dt.Rows(i)("Per1").ToString()
                        .Cells(10).Value = IIf(Val(dt.Rows(i)("Amount").ToString()) = 0, "", Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")) 'Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
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
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtRoundoff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            txtRoundoff.Text = Format(Val(txtRoundoff.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
        Next
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                btnShow.PerformClick()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID = Val(dg1.SelectedRows(0).Cells(0).Value)
            Sellout_Mannual.MdiParent = MainScreenForm
            Sellout_Mannual.Show()
            Sellout_Mannual.FillContros(tmpID)
            Sellout_Mannual.BringToFront()
            e.SuppressKeyPress = True
        End If

    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Sellout_Mannual.MdiParent = MainScreenForm
        Sellout_Mannual.Show()
        Sellout_Mannual.FillContros(tmpID)
        Sellout_Mannual.BringToFront()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PrintRecordList()
        Report_Viewer.printReport("\Reports\MannualSelloutDltRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Registers_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub PrintRecordList()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                Application.DoEvents()
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "'," & _
                    "'" & .Cells(7).Value & "'," & .Cells(8).Value & ",'" & .Cells(9).Value & "'," & _
                    "'" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & txtTotNug.Text & "','" & txtTotweight.Text & "'," & _
                    "'" & txtTotBasic.Text & "','" & txtTotCharge.Text & "','" & txtRoundoff.Text & "','" & TxtGrandTotal.Text & "', '" & .Cells(12).Value & "', '" & .Cells(13).Value & "', '" & .Cells(14).Value & "','" & .Cells(1).Value & "');"
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


    Private Sub txtAccountSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccountSearch.KeyUp
        If txtAccountSearch.Text.Trim() <> "" Then
            txtVoucherSearch.Clear()
            retrive(" And SallerName Like '" & txtAccountSearch.Text.Trim() & "%'")
        End If
        If txtAccountSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtAccountSearch_TextChanged(sender As Object, e As EventArgs) Handles txtAccountSearch.TextChanged

    End Sub

    Private Sub txtVoucherSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtVoucherSearch.KeyUp
        If txtVoucherSearch.Text.Trim() <> "" Then
            txtAccountSearch.Clear()
            retrive(" And BillNo Like '" & txtVoucherSearch.Text.Trim() & "'")
        End If
        If txtVoucherSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub txtVoucherSearch_TextChanged(sender As Object, e As EventArgs) Handles txtVoucherSearch.TextChanged

    End Sub

    Private Sub txtVehicleNo_KeyUp(sender As Object, e As KeyEventArgs) Handles txtVehicleNo.KeyUp
        If txtVehicleNo.Text.Trim() <> "" Then
            txtAccountSearch.Clear() : txtVoucherSearch.Clear()
            retrive(" And VehicleNo Like '" & txtVehicleNo.Text.Trim() & "%'")
        End If
        If txtVehicleNo.Text.Trim() = "" Then
            retrive()
        End If
    End Sub


    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub txtitemName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtitemName.KeyUp
        If txtitemName.Text.Trim() <> "" Then
            txtVoucherSearch.Clear()
            retrive(" And t.ItemName Like '" & txtitemName.Text.Trim() & "%'")
        End If
        If txtitemName.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtitemName.TextChanged

    End Sub
End Class