Imports System.IO

Public Class Auto_Scrip_Register
    Dim strSDate As String : Dim strEDate As String
    Dim dDate As DateTime : Dim mskstartDate As String
    Dim mskenddate As String
    Private headerCheckBox As CheckBox = New CheckBox()
    Private Sub Auto_Scrip_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub HeaderCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        'Necessary to end the edit mode of the Cell.
        dg1.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In dg1.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = headerCheckBox.Checked
        Next
    End Sub
    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        'Check to ensure that the row CheckBox is clicked.
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In dg1.Rows
                If Convert.ToBoolean(row.Cells("checkBoxColumn").EditedFormattedValue) = False Then
                    isChecked = False
                    Exit For
                End If
            Next
            headerCheckBox.Checked = isChecked
        End If
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub

    Private Sub MsktoDate_KeyDown(sender As Object, e As KeyEventArgs) Handles MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnShow.Focus()
        End If
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
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
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 12
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        headerCheckBox.Location = New Point(headerCellLocation.X + 8, headerCellLocation.Y + 2)
        headerCheckBox.BackColor = Color.Crimson
        headerCheckBox.Size = New Size(18, 18)
        AddHandler headerCheckBox.Click, AddressOf HeaderCheckBox_Clicked
        dg1.Controls.Add(headerCheckBox)
        Dim checkBoxColumn As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = ""
        checkBoxColumn.Width = 30
        checkBoxColumn.Name = "checkBoxColumn"
        dg1.Columns.Insert(0, checkBoxColumn)
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Date" : dg1.Columns(2).Width = 100 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "No" : dg1.Columns(3).Width = 100 : dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).Name = "Vehicle No" : dg1.Columns(4).Width = 135 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(5).Name = "Account Name" : dg1.Columns(5).Width = 270 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(6).Name = "Item" : dg1.Columns(6).Visible = False : dg1.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(7).Name = "Nug" : dg1.Columns(7).Width = 80 : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).Name = "Kg" : dg1.Columns(8).Width = 80 : dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Net" : dg1.Columns(9).Width = 100 : dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "Charges" : dg1.Columns(10).Width = 100 : dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).Name = "R.off" : dg1.Columns(11).Width = 50 : dg1.Columns(11).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(12).Name = "Total" : dg1.Columns(12).Width = 125 : dg1.Columns(12).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtRoundOff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            txtRoundOff.Text = Format(Val(txtRoundOff.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
        Next
        lblRecordCount.Visible = True
        lblRecordCount.Text = "Record Count : " & dg1.RowCount
    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            retrive("And SallerName Like '" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * From Vouchers WHERE EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype = '" & Me.Text & "'" & condtion & "  order by EntryDate,BillNo")
        If dt.Rows.Count > 20 Then dg1.Columns(12).Width = 105 Else dg1.Columns(12).Width = 125
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(1).readonly = True : .Cells(2).readonly = True
                        .Cells(3).readonly = True : .Cells(4).readonly = True
                        .Cells(5).readonly = True : .Cells(6).readonly = True
                        .Cells(7).readonly = True : .Cells(8).readonly = True
                        .Cells(9).readonly = True : .Cells(10).readonly = True
                        .Cells(11).readonly = True : .cells(12).readonly = True
                        .Cells(1).Value = dt.Rows(i)("ID").ToString()
                        .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(3).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(4).Value = dt.Rows(i)("VehicleNo").ToString()
                        .Cells(5).Value = dt.Rows(i)("SallerName").ToString()
                        '.Cells(5).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                        .Cells(12).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(.Cells(12).Value) - Val(Val(.Cells(9).Value) + Val(.Cells(10).Value)), "0.00")
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(12).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
            Sellout_Auto.MdiParent = MainScreenForm
            Sellout_Auto.Show()
            Sellout_Auto.FillFromData(tmpID)
            If Not Sellout_Auto Is Nothing Then
                Sellout_Auto.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick

    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive() : TempRowColumn()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        If dg1.CurrentCell.ColumnIndex = 0 Then Exit Sub
        Dim tmpID As String = Val(dg1.SelectedRows(0).Cells(1).Value)
        Sellout_Auto.MdiParent = MainScreenForm
        Sellout_Auto.Show()
        Sellout_Auto.FillFromData(tmpID)
        If Not Sellout_Auto Is Nothing Then
            Sellout_Auto.BringToFront()
        End If
    End Sub
    Private Sub IDGentate()
        Dim checkBox As DataGridViewCheckBoxCell
        Dim id As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            retrive2(id, " And upper(Vouchers.accountname) Like upper('" & txtCustomerSearch.Text.Trim() & "%')")
        Else
            id = id.Remove(id.LastIndexOf(","))
            retrive2(id, " And upper(Vouchers.accountname) Like upper('" & txtCustomerSearch.Text.Trim() & "%')")
        End If
    End Sub
    Private Sub txtNugs_KeyUp(sender As Object, e As KeyEventArgs) Handles txtNugs.KeyUp
        If txtNugs.Text.Trim() <> "" Then
            retrive("And Nug Like '" & txtNugs.Text.Trim() & "%'")
        End If
        If txtNugs.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    'Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyUp
    '    If txtItemSearch.Text.Trim() <> "" Then
    '        retrive("And ItemName Like '%" & txtItemSearch.Text.Trim() & "%'")
    '    End If
    '    If txtItemSearch.Text.Trim() = "" Then
    '        retrive()
    '    End If
    'End Sub

    Private Sub txtItemSearch_TextChanged(sender As Object, e As EventArgs) Handles txtNugs.TextChanged

    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub

    Private Sub txtWeight_KeyUp(sender As Object, e As KeyEventArgs) Handles txtWeight.KeyUp
        If txtWeight.Text.Trim() <> "" Then
            retrive("And Kg Like '" & txtWeight.Text.Trim() & "%'")
        End If
        If txtWeight.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtWeight_TextChanged(sender As Object, e As EventArgs) Handles txtWeight.TextChanged

    End Sub

    Private Sub txtCharges_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCharges.KeyUp
        If txtCharges.Text.Trim() <> "" Then
            retrive("And TotalCharges Like '" & txtCharges.Text.Trim() & "%'")
        End If
        If txtCharges.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub lblRecordCount_Click(sender As Object, e As EventArgs) Handles lblRecordCount.Click

    End Sub
    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")>
    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," &
                    "'" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "'," &
                    "'" & Format(Val(.Cells(7).Value), "0.00") & "','" & Format(Val(.Cells(8).Value), "0.00") & "'," & Format(Val(.Cells(9).Value), "0.00") & ",'" & Format(Val(.Cells(10).Value), "0.00") & "'," &
                    "'" & Format(Val(.Cells(11).Value), "0.00") & "'," & Format(Val(.Cells(12).Value), "0.00") & ",'" & Format(Val(txtTotNug.Text), "0.00") & "','" & Format(Val(txtTotweight.Text), "0.00") & "'," &
                    "'" & Format(Val(txtTotBasic.Text), "0.00") & "','" & Format(Val(txtTotCharge.Text), "0.00") & "','" & Format(Val(txtRoundOff.Text), "0.00") & "','" & Format(Val(TxtGrandTotal.Text), "0.00") & "');"
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
        If File.Exists(Application.StartupPath & "\Reports\AutoSelloutRegister.rpt") Then
            Report_Viewer.printReport("\Reports\AutoSelloutRegister.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Ugrahi_Viewer.BringToFront()
            End If
        Else
            MsgBox("Report File Not Exists.... Please Contact to Service Provider...", MsgBoxStyle.Critical, "Not Found...")
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
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 28
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "Date" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "AccountName" : .Columns(3).Width = 159
            .Columns(4).Name = "BillingType" : .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo" : .Columns(5).Width = 59
            .Columns(6).Name = "itemName" : .Columns(6).Width = 69
            .Columns(7).Name = "LotNo" : .Columns(7).Width = 76
            .Columns(8).Name = "Nug" : .Columns(8).Width = 90
            .Columns(9).Name = "Weight" : .Columns(9).Width = 86
            .Columns(10).Name = "Rate" : .Columns(10).Width = 90
            .Columns(11).Name = "per" : .Columns(11).Width = 50
            .Columns(12).Name = "Amount" : .Columns(12).Width = 95
            .Columns(13).Name = "ChargeName" : .Columns(13).Width = 159
            .Columns(14).Name = "onValue" : .Columns(14).Width = 159
            .Columns(15).Name = "@" : .Columns(15).Width = 59
            .Columns(16).Name = "=/-" : .Columns(16).Width = 59
            .Columns(17).Name = "ChargeAmount" : .Columns(17).Width = 69
            .Columns(18).Name = "TotalNug" : .Columns(18).Width = 76
            .Columns(19).Name = "TotalWeight" : .Columns(19).Width = 90
            .Columns(20).Name = "TotalBasicAmount" : .Columns(20).Width = 86
            .Columns(21).Name = "RoundOff" : .Columns(21).Width = 90
            .Columns(22).Name = "TotalAmount" : .Columns(22).Width = 90
            .Columns(23).Name = "OtherItemName" : .Columns(23).Width = 95
            .Columns(24).Name = "OtherAccountName" : .Columns(24).Width = 159
            .Columns(25).Name = "AmountInWords" : .Columns(25).Width = 159
            .Columns(26).Name = "Opbal" : .Columns(24).Width = 159
            .Columns(27).Name = "Cl Bal" : .Columns(25).Width = 159
        End With
    End Sub
    Sub retrive2(ByVal id As String, Optional ByVal condtion As String = "")
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        If id <> "" Then
            id = " Where Vouchers.id in (" & id & ")"
        End If
        dt = clsFun.ExecDataTable(" Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.SallerName, Vouchers.VehicleNo, " _
                                & " Transaction1.ItemName, Transaction1.Lot, Transaction1.Nug as TransNug, Transaction1.Weight, Transaction1.Rate," _
                                & "   Transaction1.Per, Transaction1.Amount, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, " _
                                & "  Vouchers.TotalCharges, Items.OtherName, Accounts.OtherName FROM ((Vouchers INNER JOIN Transaction1 ON Vouchers.ID = Transaction1.VoucherID) " _
                                & "    INNER JOIN Items ON Transaction1.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID " & id & "")
        'dt = clsFun.ExecDataTable(" Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.SallerName, Vouchers.VehicleNo, " _
        '                        & " Transaction1.ItemName, Transaction1.Lot, Transaction1.Nug as TransNug, Transaction1.Weight, Transaction1.Rate," _
        '                        & " Transaction1.Per, Transaction1.Amount, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, " _
        '                        & " Vouchers.TotalCharges, Items.OtherName, Accounts.OtherName FROM ((Vouchers INNER JOIN Transaction1 ON Vouchers.ID = Transaction1.VoucherID) " _
        '                        & " INNER JOIN Items ON Transaction1.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID Where (Vouchers.ID in(" & id & "))")
        tmpgrid.Rows.Clear()
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("SallerName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Lot").ToString()
                    .Cells(8).Value = Format(Val(.Cells(8).Value & dt.Rows(i)("TransNug").ToString()), "0.00")
                    .Cells(9).Value = Format(Val(.Cells(9).Value & dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = Format(Val(.Cells(10).Value & dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = Format(Val(.Cells(12).Value & dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(18).Value = Format(Val(.Cells(18).Value & dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = Format(Val(.Cells(19).Value & dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = Format(Val(.Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = Format(Val(.Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = Format(Val(.Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("OtherName1").ToString()
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("OnValue").ToString()), "0.00") & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & Format(Val(dt1.Rows(j)("Amount").ToString()), "0.00") & vbCrLf
                        Next
                    Else
                        .Cells(13).Value = ""
                        .Cells(14).Value = ""
                        .Cells(15).Value = ""
                        .Cells(16).Value = ""
                        .Cells(17).Value = ""
                    End If

                End With
                '  End If
            Next
        End If
        dt.Clear()
        dt1.Clear()
    End Sub
    'Private Sub AcBal()
    '    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtAccountID.Text) & "")
    '    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
    '    If drcr = "Dr" Then
    '        tmpamtdr = Val(opbal) + Val(tmpamtdr)
    '    Else
    '        tmpamtcr = Val(opbal) + Val(tmpamtcr)
    '    End If
    '    Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
    '    If drcr = "Cr" Then
    '        opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
    '    Else
    '        opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
    '    End If
    '    Dim cntbal As Integer = 0
    '    cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate < '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    If cntbal = 0 Then
    '        opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
    '    Else
    '        If Val(tmpamtcr) > Val(tmpamtdr) Then
    '            opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
    '        Else
    '            opbal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
    '        End If
    '    End If
    'End Sub
    'Private Sub ClosingBal()
    '    ClBal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtAccountID.Text) & "")
    '    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
    '    If drcr = "Dr" Then
    '        tmpamtdr = Val(ClBal) + Val(tmpamtdr)
    '    Else
    '        tmpamtcr = Val(ClBal) + Val(tmpamtcr)
    '    End If
    '    Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(ClBal)
    '    If drcr = "Cr" Then
    '        ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
    '    Else
    '        ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
    '    End If
    '    Dim cntbal As Integer = 0
    '    cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '    If cntbal = 0 Then
    '        ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
    '    Else
    '        If Val(tmpamtcr) > Val(tmpamtdr) Then
    '            ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Cr"
    '        Else
    '            ClBal = Format(Math.Abs(Val(tmpamt)), "0.00") & " Dr"
    '        End If
    '    End If
    'End Sub

    Private Sub btnPrintBills_Click(sender As Object, e As EventArgs) Handles btnPrintBills.Click
        IDGentate() ': AcBal() : ClosingBal() 
        printRecord()
        Report_Viewer.printReport("\AutoBeejakAll.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
    Private Sub PrintRecord2()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then
                    sql = sql & "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " & _
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " & _
                        " P21,P22)" & _
                        "  values('" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "','');"
                End If
            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try

    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        IDGentate()
        'clsFun.CloseConnection()
        'clsFun.changeCompany()
        PrintRecord2()
        Report_Viewer.printReport("\AutoSellOutNakal.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
End Class