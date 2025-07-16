Public Class TempChallanRegister
    Private headerCheckBox As CheckBox = New CheckBox()
    Private Sub Standard_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub


    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown, btnShow.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
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
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub Standard_Sale_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskFromDate.Focus()
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
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text) : MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
        pnlSearch.Visible = False
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 11
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
        'Assign Click event to the DataGridView Cell.
        AddHandler dg1.CellContentClick, AddressOf dg1_CellClick
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Date" : dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "V. No." : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Account Name" : dg1.Columns(4).Width = 300
        dg1.Columns(5).Name = "Nug" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Kg" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Rate" : dg1.Columns(7).Visible = False
        dg1.Columns(8).Name = "Net" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "Charges" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "Other Charges" : dg1.Columns(10).Width = 150
        dg1.Columns(11).Name = "Total" : dg1.Columns(11).Width = 100
        dg1.Columns(0).ReadOnly = False
        ' dg1.Rows(0).ReadOnly = False
    End Sub
    Private Sub AccountSearch()
        dgAccounts.ColumnCount = 4
        Dim chk As New DataGridViewCheckBoxColumn()
        chk.HeaderText = ChrW(&H2714)
        chk.Name = "chk"
        dgAccounts.Columns.Insert(0, chk)
        dgAccounts.Columns(0).Width = 30
        dgAccounts.Columns(1).Name = "ID"
        dgAccounts.Columns(1).Visible = False
        dgAccounts.Columns(2).Name = "Account Name"
        dgAccounts.Columns(2).Width = 200
        dgAccounts.Columns(3).Name = "Total"
        dgAccounts.Columns(3).Width = 130
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(10).Value) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
        Next
    End Sub
    Private Sub retrive()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' order by EntryDate")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(1).readonly = True : .Cells(2).readonly = True
                        .Cells(3).readonly = True : .Cells(4).readonly = True
                        .Cells(5).readonly = True : .Cells(6).readonly = True
                        .Cells(7).readonly = True : .Cells(8).readonly = True
                        .Cells(9).readonly = True : .Cells(10).readonly = True
                        .Cells(11).readonly = True
                        .Cells(1).Value = dt.Rows(i)("id").ToString()
                        .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(3).Value = dt.Rows(i)("billNo").ToString()
                        .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
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
        TempRowColumn()
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then
                    sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " & _
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " & _
                        " P21,P22,P23,P24)" & _
                        "  values('" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," & _
                                "'" & .Cells(35).Value & "','" & .Cells(36).Value & "','" & .Cells(37).Value & "')"
                    Try
                        cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
                        ClsFunPrimary.ExecNonQuery(sql)
                    Catch ex As Exception
                        MsgBox(ex.Message)
                        ClsFunPrimary.CloseConnection()
                    End Try
                End If
            End With
        Next
        ' clsFun.ExecNonQuery(sql)
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click

    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick
        '    If dg1.Rows.Count > 0 AndAlso dg1.ColumnCount >= 1 Then
        '  dg1.Rows(e.RowIndex).Cells(0).ReadOnly = False
        'End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                btnShow.PerformClick()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
            TempChallan.MdiParent = MainScreenForm
            TempChallan.Show()
            TempChallan.FillControls(tmpID)
            If Not TempChallan Is Nothing Then
                TempChallan.BringToFront()
            End If
        End If
        e.SuppressKeyPress = True

    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
        TempChallan.MdiParent = MainScreenForm
        TempChallan.Show()
        TempChallan.FillControls(tmpID)
        If Not TempChallan Is Nothing Then
            TempChallan.BringToFront()
        End If
    End Sub

    Private Sub btnSelectAccount_Click(sender As Object, e As EventArgs) Handles btnSelectAccount.Click
        pnlSearch.Visible = True
        AccountSearch()
        ShowAccounts()
    End Sub
    Private Sub ShowAccounts(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        ssql = "Select AccountID, AccountName,TotalAmount from Vouchers where Transtype='Standard Sale' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' group by AccountID order by AccountID" & condtion & " "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                dgAccounts.Rows.Add()
                With dgAccounts.Rows(i)
                    .Cells(1).Value = dt.Rows(i)("AccountId").ToString()
                    .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(3).Value = dt.Rows(i)("TotalAmount").ToString()
                End With
            Next i
        End If
    End Sub

    Private Sub txtSearchAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearchAccount.KeyPress
        If txtSearchAccount.Text.Trim() <> "" Then
            ShowAccounts(" where AccountName Like '" & txtSearchAccount.Text.Trim() & "%'")
        End If
    End Sub

    Private Sub txtSearchAccount_TextChanged(sender As Object, e As EventArgs) Handles txtSearchAccount.TextChanged

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
    Private Sub IDGentate()
        Dim checkBox As DataGridViewCheckBoxCell
        Dim id As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            id = id & row.Cells(1).Value & ","
        Next
        id = id.Remove(id.LastIndexOf(","))
        retrive2(id)
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 38
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "EntryDate" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "SallerName" : .Columns(3).Width = 59
            .Columns(4).Name = "BillingType" : .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo" : .Columns(5).Width = 159
            .Columns(6).Name = "itemName" : .Columns(6).Width = 69
            .Columns(7).Name = "Cut" : .Columns(7).Width = 76
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
            .Columns(26).Name = "CommPer" : .Columns(26).Width = 90
            .Columns(27).Name = "CommAmt" : .Columns(27).Width = 86
            .Columns(28).Name = "UCPer" : .Columns(28).Width = 90
            .Columns(29).Name = "UcAmt" : .Columns(29).Width = 90
            .Columns(30).Name = "RdfPer" : .Columns(30).Width = 95
            .Columns(31).Name = "RdfAmt" : .Columns(31).Width = 159
            .Columns(32).Name = "Bardanaper" : .Columns(32).Width = 159
            .Columns(33).Name = "BardanaAmt" : .Columns(33).Width = 90
            .Columns(34).Name = "labourPer" : .Columns(34).Width = 90
            .Columns(35).Name = "LabourAmt" : .Columns(35).Width = 95
            .Columns(36).Name = "TotalCharges" : .Columns(36).Width = 159
            .Columns(37).Name = "GrossAmount" : .Columns(37).Width = 159
        End With
    End Sub
    Sub retrive2(ByVal id As String)
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        dt = clsFun.ExecDataTable("Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.AccountName, Vouchers.VehicleNo," _
                                  & "Transaction2.ItemName, Transaction2.Lot, Transaction2.Nug as TransNug, Transaction2.Weight, Transaction2.Rate," _
                                  & "Transaction2.Per, Transaction2.Amount,Transaction2.CommPer,Transaction2.CommAmt,Transaction2.MPer,Transaction2.MAmt," _
                                  & "Transaction2.RdfPer,Transaction2.RdfAmt,Transaction2.Tare,Transaction2.TareAmt,Transaction2.labour,Transaction2.LabourAmt," _
                                  & "Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, Vouchers.DiscountAmount, Vouchers.TotalCharges, vouchers.SubTotal, " _
                                  & "Items.OtherName, Accounts.OtherName as AccountOtherName FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)" _
                                  & "INNER JOIN Items ON Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.AccountID = Accounts.ID  Where (Vouchers.ID in(" & id & "))")
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Lot").ToString()
                    .Cells(8).Value = .Cells(8).Value & dt.Rows(i)("TransNug").ToString()
                    .Cells(9).Value = .Cells(9).Value & dt.Rows(i)("Weight").ToString()
                    .Cells(10).Value = .Cells(10).Value & dt.Rows(i)("Rate").ToString()
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = .Cells(12).Value & dt.Rows(i)("Amount").ToString()
                    .Cells(18).Value = .Cells(18).Value & dt.Rows(i)("Nug").ToString()
                    .Cells(19).Value = .Cells(19).Value & dt.Rows(i)("Kg").ToString()
                    .Cells(20).Value = .Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()
                    .Cells(21).Value = .Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()
                    .Cells(22).Value = .Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("AccountOtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & dt.Rows(i)("CommPer").ToString() & vbCrLf
                    .Cells(27).Value = .Cells(27).Value & dt.Rows(i)("CommAmt").ToString() & vbCrLf
                    .Cells(28).Value = .Cells(28).Value & dt.Rows(i)("Mper").ToString() & vbCrLf
                    .Cells(29).Value = .Cells(29).Value & dt.Rows(i)("Mamt").ToString() & vbCrLf
                    .Cells(30).Value = .Cells(30).Value & dt.Rows(i)("RdfPer").ToString() & vbCrLf
                    .Cells(31).Value = .Cells(31).Value & dt.Rows(i)("rdfAmt").ToString() & vbCrLf
                    .Cells(32).Value = .Cells(32).Value & dt.Rows(i)("Tare").ToString() & vbCrLf
                    .Cells(33).Value = .Cells(33).Value & dt.Rows(i)("tareamt").ToString() & vbCrLf
                    .Cells(34).Value = .Cells(34).Value & dt.Rows(i)("Labour").ToString() & vbCrLf
                    .Cells(35).Value = .Cells(35).Value & dt.Rows(i)("ID").ToString() & vbCrLf
                    .Cells(36).Value = .Cells(36).Value & dt.Rows(i)("DiscountAmount").ToString()
                    .Cells(37).Value = .Cells(37).Value & dt.Rows(i)("SubTotal").ToString()
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                            .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                            .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Amount").ToString() & vbCrLf
                        Next
                    Else
                        .Cells(13).Value = ""
                        .Cells(14).Value = ""
                        .Cells(15).Value = ""
                        .Cells(16).Value = ""
                        .Cells(17).Value = ""
                    End If
                    'dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    'If dt1.Rows.Count > 0 Then
                    '    For j = 0 To dt1.Rows.Count - 1
                    '        .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                    '        .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
                    '        .Cells(15).Value = .Cells(15).Value & dt1.Rows(j)("Calculate").ToString() & vbCrLf
                    '        .Cells(16).Value = .Cells(16).Value & dt1.Rows(j)("ChargeType").ToString() & vbCrLf
                    '        .Cells(17).Value = .Cells(17).Value & dt1.Rows(j)("Amount").ToString() & vbCrLf
                    '    Next
                    'Else
                    '    .Cells(13).Value = ""
                    '    .Cells(14).Value = ""
                    '    .Cells(15).Value = ""
                    '    .Cells(16).Value = ""
                    '    .Cells(17).Value = ""
                    'End If
                End With
                '  End If
            Next
        End If
        dt.Clear()
        dt1.Clear()
    End Sub

    Private Sub btnPrintBills_Click(sender As Object, e As EventArgs) Handles btnPrintBills.Click
        ''   retrive2(id)
        IDGentate()
        'MsgBox("If you want to Print. Save First Record.", vbOkOnly, "Save First")
        'Dim res = MessageBox.Show("Do you want to Save Bill", "Save First?", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        'If res = Windows.Forms.DialogResult.Yes Then
        '    BtnSave.PerformClick()
        'Else
        ' End If
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOkOnly, "Empty")
            Exit Sub
        Else
            clsFun.CloseConnection()
            'clsFun.changeCompany()
            PrintRecord()
            Report_Viewer.printReport("\ChallanAll.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Private Sub dg1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentDoubleClick

    End Sub

    Private Sub dg1_ColumnHeaderMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dg1.ColumnHeaderMouseDoubleClick
        dg1.ClearSelection()
    End Sub

    Private Sub txtTotNug_TextChanged(sender As Object, e As EventArgs) Handles txtTotNug.TextChanged

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtTotweight_TextChanged(sender As Object, e As EventArgs) Handles txtTotweight.TextChanged

    End Sub
End Class