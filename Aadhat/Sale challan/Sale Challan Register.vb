

Public Class Sale_Challan_Register
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
        'Assign Click event to the DataGridView Cell.
        AddHandler dg1.CellContentClick, AddressOf dg1_CellClick
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Date" : dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "V. No." : dg1.Columns(3).Width = 100
        dg1.Columns(4).Name = "Account Name" : dg1.Columns(4).Width = 300
        dg1.Columns(5).Name = "Nug" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Weight" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Rate" : dg1.Columns(7).Visible = False
        dg1.Columns(8).Name = "Net" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "Charges" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "Oth.exp." : dg1.Columns(10).Width = 100
        dg1.Columns(11).Name = "R.O." : dg1.Columns(11).Width = 50
        dg1.Columns(12).Name = "Total" : dg1.Columns(12).Width = 100
        dg1.Columns(0).ReadOnly = False
        ' dg1.Rows(0).ReadOnly = False
    End Sub

    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtRoundoff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(10).Value) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtRoundoff.Text = Format(Val(txtRoundoff.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
        Next
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "'  " & condtion & " order by EntryDate")
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
                        .Cells(11).Value = Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                        .Cells(12).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
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
                    Dim amtInWords As String = String.Empty
                    Try
                        amtInWords = AmtInWord(.Cells(37).Value)
                    Catch ex As Exception
                        amtInWords = ex.ToString
                    End Try
                    sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " & _
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " & _
                        " P21,P22,P23,P24,P25,P26,P27,P28,P29,P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,P40,P41,P42)" & _
                        "  values('" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " & _
                                "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " & _
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " & _
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " & _
                                "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "'," & _
                                "'" & amtInWords & "','" & .Cells(36).Value & "','" & .Cells(37).Value & "','" & .Cells(38).Value & "'," & _
                                "'" & .Cells(39).Value & "','" & .Cells(40).Value & "','" & .Cells(41).Value & "','" & .Cells(42).Value & "','" & .Cells(43).Value & "'," & _
                                "'" & .Cells(44).Value & "','" & .Cells(45).Value & "','" & .Cells(46).Value & "','" & .Cells(47).Value & "','" & .Cells(48).Value & "'," & _
                                "'" & .Cells(49).Value & "','" & .Cells(50).Value & "','" & .Cells(51).Value & "','" & .Cells(52).Value & "','" & .Cells(53).Value & "'," & _
                                "'" & .Cells(54).Value & "','" & .Cells(55).Value & "')"
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
    Private Sub PrintRegister()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            con.BeginTransaction(IsolationLevel.ReadCommitted)
            With row
                sql = sql & "insert into Printing(D1,D2,M1,M2, P1, P2,P3, P4, P5,P6,P7,T1,T2,T3,T4,T5) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "'," & _
                    "'" & Val(.Cells(6).Value) & "','" & Val(.Cells(7).Value) & "'," & _
                    "'" & Val(.Cells(8).Value) & "'," & Val(.Cells(9).Value) & ",'" & txtTotNug.Text & "','" & txtTotweight.Text & "'," & _
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
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        PrintRegister()
        Report_Viewer.printReport("\Reports\StandardRegister.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Registers_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
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
            Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(1).Value)
            Standard_Sale.MdiParent = MainScreenForm
            Standard_Sale.Show()
            Standard_Sale.FillControls(tmpID)
            If Not Standard_Sale Is Nothing Then
                Standard_Sale.BringToFront()
            End If
        End If
        e.SuppressKeyPress = True
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
        Standard_Sale.MdiParent = MainScreenForm
        Standard_Sale.Show()
        Standard_Sale.FillControls(tmpID)
        If Not Standard_Sale Is Nothing Then
            Standard_Sale.BringToFront()
        End If
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
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            retrive2(id, " And upper(Vouchers.accountname) Like upper('" & txtPrimarySearch.Text.Trim() & "%')")
        Else
            id = id.Remove(id.LastIndexOf(","))
            retrive2(id, " And upper(Vouchers.accountname) Like upper('" & txtPrimarySearch.Text.Trim() & "%')")
        End If
    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 56
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
            .Columns(38).Name = "TotalNugs" : .Columns(38).Width = 159
            .Columns(39).Name = "TotalNugs" : .Columns(39).Width = 159
            .Columns(40).Name = "TotalWeight" : .Columns(40).Width = 159
            .Columns(41).Name = "TotalCommission" : .Columns(41).Width = 159
            .Columns(42).Name = "TotalMarketFees" : .Columns(42).Width = 159
            .Columns(43).Name = "TotalRDF" : .Columns(43).Width = 159
            .Columns(44).Name = "TotalTare" : .Columns(44).Width = 159
            .Columns(45).Name = "TotalLabour" : .Columns(45).Width = 159
            .Columns(46).Name = "Driver Name" : .Columns(46).Width = 159
            .Columns(47).Name = "Mobile" : .Columns(47).Width = 159
            .Columns(48).Name = "Remark" : .Columns(48).Width = 159
            .Columns(49).Name = "GrNo" : .Columns(47).Width = 159
            .Columns(50).Name = "GSTN" : .Columns(48).Width = 159
            .Columns(51).Name = "Cust Mobile" : .Columns(51).Width = 159
            .Columns(52).Name = "Broker Name" : .Columns(52).Width = 159
            .Columns(53).Name = "Broker Mobile" : .Columns(53).Width = 159
            .Columns(54).Name = "TransPort" : .Columns(54).Width = 159
            .Columns(55).Name = "GRNo" : .Columns(55).Width = 159
        End With
    End Sub
    Sub retrive2(ByVal id As String, Optional ByVal condtion As String = "")
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        If id <> "" Then
            id = " and Vouchers.id in (" & id & ")"
        End If
        dt = clsFun.ExecDataTable("Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.AccountName, Vouchers.VehicleNo," _
                                  & "Transaction2.ItemName, Transaction2.Lot, Transaction2.Nug as TransNug, Transaction2.Weight, Transaction2.Rate," _
                                  & "Transaction2.Per, Transaction2.Amount,Transaction2.CommPer,Transaction2.CommAmt,Transaction2.MPer,Transaction2.MAmt," _
                                  & "Transaction2.RdfPer,Transaction2.RdfAmt,Transaction2.Tare,Transaction2.TareAmt,Transaction2.labour,Transaction2.LabourAmt," _
                                  & "Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, Vouchers.DiscountAmount, Vouchers.TotalCharges, vouchers.SubTotal, " _
                                  & "Vouchers.RoundOff,Vouchers.T1,Vouchers.T2,Vouchers.T3,Vouchers.T4,Vouchers.T5,Vouchers.T6,Vouchers.T7,Vouchers.T8,Vouchers.T9,Vouchers.T10, " _
                                  & "Items.OtherName, Accounts.OtherName as AccountOtherName FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)" _
                                  & "INNER JOIN Items ON Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.AccountID = Accounts.ID  Where  Vouchers.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & id & " " & condtion & "")
        'dt = clsFun.ExecDataTable("Select Vouchers.Entrydate, Vouchers.billNo, Vouchers.AccountName, Vouchers.VehicleNo, Transaction2.ItemName, Transaction2.Lot, " _
        '                         & " Transaction2.Nug, Transaction2.Weight, Transaction2.Rate, Transaction2.Per, Transaction2.Amount, Vouchers.Nug, Vouchers.Kg, " _
        '                         & " Vouchers.BasicAmount, Vouchers.TotalAmount, Vouchers.TotalCharges, Accounts.OtherName as AccountOtherName, Items.OtherName, " _
        '                         & "Transaction2.CommPer, Transaction2.CommAmt, Transaction2.MPer, Transaction2.MAmt, Transaction2.RdfPer, Transaction2.RdfAmt, " _
        '                         & "Transaction2.Tare, Transaction2.Tareamt, Transaction2.labour, Transaction2.LabourAmt,Vouchers.ID,Vouchers.DiscountAmount, Vouchers.SubTotal FROM Items INNER JOIN " _
        '                         & "(Accounts INNER JOIN (Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID) ON " _
        '                         & " Accounts.ID = Vouchers.AccountID) ON Items.ID = Transaction2.ItemID  Where (Vouchers.ID=" & Val(txtid.Text) & ")")
        '  tmpgrid.Rows.Clear()
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
                    .Cells(8).Value = .Cells(8).Value & Format(Val(dt.Rows(i)("TransNug").ToString()), "0.00")
                    .Cells(9).Value = .Cells(9).Value & Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = .Cells(10).Value & Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = .Cells(12).Value & Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                    .Cells(18).Value = .Cells(18).Value & Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = .Cells(19).Value & Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = .Cells(20).Value & Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = .Cells(21).Value & Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = .Cells(22).Value & Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("AccountOtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & Format(Val(dt.Rows(i)("CommPer").ToString()), "0.00") & vbCrLf
                    .Cells(27).Value = .Cells(27).Value & Format(Val(dt.Rows(i)("CommAmt").ToString()), "0.00") & vbCrLf
                    .Cells(28).Value = .Cells(28).Value & Format(Val(dt.Rows(i)("Mper").ToString()), "0.00") & vbCrLf
                    .Cells(29).Value = .Cells(29).Value & Format(Val(dt.Rows(i)("Mamt").ToString()), "0.00") & vbCrLf
                    .Cells(30).Value = .Cells(30).Value & Format(Val(dt.Rows(i)("RdfPer").ToString()), "0.00") & vbCrLf
                    .Cells(31).Value = .Cells(31).Value & Format(Val(dt.Rows(i)("rdfAmt").ToString()), "0.00") & vbCrLf
                    .Cells(32).Value = .Cells(32).Value & Format(Val(dt.Rows(i)("Tare").ToString()), "0.00") & vbCrLf
                    .Cells(33).Value = .Cells(33).Value & Format(Val(dt.Rows(i)("tareamt").ToString()), "0.00") & vbCrLf
                    .Cells(34).Value = .Cells(34).Value & Format(Val(dt.Rows(i)("Labour").ToString()), "0.00") & vbCrLf
                    .Cells(35).Value = .Cells(35).Value & Format(Val(dt.Rows(i)("Labouramt").ToString()), "0.00") & vbCrLf
                    .Cells(36).Value = .Cells(36).Value & Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                    .Cells(37).Value = .Cells(37).Value & Format(Val(dt.Rows(i)("SubTotal").ToString()), "0.00")
                    .Cells(38).Value = .Cells(38).Value & Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                    .Cells(39).Value = Format(Val(dt.Compute("Sum(TransNug)", "")), "0.00")
                    .Cells(40).Value = Format(Val(dt.Compute("Sum(Weight)", "")), "0.00")
                    .Cells(41).Value = Format(Val(dt.Compute("Sum(CommAmt)", "")), "0.00")
                    .Cells(42).Value = Format(Val(dt.Compute("Sum(Mamt)", "")), "0.00")
                    .Cells(43).Value = Format(Val(dt.Compute("Sum(rdfAmt)", "")), "0.00")
                    .Cells(44).Value = Format(Val(dt.Compute("Sum(tareamt)", "")), "0.00")
                    .Cells(45).Value = Format(Val(dt.Compute("Sum(Labouramt)", "")), "0.00")
                    .Cells(46).Value = dt.Rows(i)("T1").ToString()
                    .Cells(47).Value = dt.Rows(i)("T2").ToString()
                    .Cells(48).Value = dt.Rows(i)("T3").ToString()
                    .Cells(49).Value = dt.Rows(i)("T4").ToString()
                    .Cells(50).Value = dt.Rows(i)("T5").ToString()
                    .Cells(51).Value = dt.Rows(i)("T6").ToString()
                    .Cells(52).Value = dt.Rows(i)("T7").ToString()
                    .Cells(53).Value = dt.Rows(i)("T8").ToString()
                    .Cells(54).Value = dt.Rows(i)("T9").ToString()
                    .Cells(55).Value = dt.Rows(i)("T10").ToString()

                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
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
        IDGentate()
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOkOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
            Report_Viewer.printReport("\SaleChallan All.rpt")
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

    Private Sub txtPrimarySearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPrimarySearch.KeyUp
        If dg1.RowCount = 0 Then Exit Sub
        If txtPrimarySearch.Text.Trim() <> "" Then
            retrive(" And upper(accountname) Like upper('" & txtPrimarySearch.Text.Trim() & "%')")
        Else
            retrive()
        End If
    End Sub

    Private Sub btnPrint2_Click(sender As Object, e As EventArgs) Handles btnPrint2.Click
        IDGentate()
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOkOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
            Report_Viewer.printReport("\SaleChallan2 All.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        IDGentate()
        If dg1.RowCount = 0 Then
            MsgBox("There is no record to Print...", vbOkOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Standard_Sale_Register).Any = False Then Exit Sub
            Report_Viewer.printReport("\BillofSupplyAll.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class