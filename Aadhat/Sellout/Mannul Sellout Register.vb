Public Class Scrip_Register
    Dim strSDate As String
    Dim strEDate As String
    Dim dDate As DateTime
    Dim mskstartDate As String
    Dim mskenddate As String
    Private headerCheckBox As CheckBox = New CheckBox()
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
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
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
        headerCheckBox.BackColor = Color.WhiteSmoke
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
        dg1.Columns(3).Name = "Voucher No" : dg1.Columns(3).Width = 120
        dg1.Columns(4).Name = "Vehicle No" : dg1.Columns(4).Width = 120
        dg1.Columns(5).Name = "Account Name" : dg1.Columns(5).Width = 200
        dg1.Columns(6).Name = "Nug" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Kg" : dg1.Columns(7).Width = 100
        dg1.Columns(8).Name = "Net" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "Charges" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "Round Off" : dg1.Columns(10).Width = 100
        dg1.Columns(11).Name = "Total" : dg1.Columns(11).Width = 120
        dg1.Columns(12).Name = "OtherHindiName" : dg1.Columns(12).Width = 120
    End Sub

    Private Sub rowColums1()
        dg1.ColumnCount = 15
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "No" : dg1.Columns(2).Width = 50
        dg1.Columns(3).Name = "Veh. No" : dg1.Columns(3).Width = 80
        dg1.Columns(4).Name = "Account Name" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Item Name" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Nug" : dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Kg" : dg1.Columns(7).Width = 100
        dg1.Columns(8).Name = "Rate" : dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "Per" : dg1.Columns(9).Width = 100
        dg1.Columns(10).Name = "Net" : dg1.Columns(10).Width = 100
        dg1.Columns(11).Name = "Charges" : dg1.Columns(11).Width = 100
        dg1.Columns(12).Name = "Round Off" : dg1.Columns(12).Width = 100
        dg1.Columns(13).Name = "Total" : dg1.Columns(13).Width = 120
        dg1.Columns(14).Name = "OtherHindiName" : dg1.Columns(14).Visible = False
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
    Private Sub retrive(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        ' dt = clsFun.ExecDataTable("Select * from vouchers where entrydate='" & mskFromDate.Text & "' and MsktoDate='" & mskFromDate.Text & "'and TransType='" & Me.Text & "'")
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' " & condtion & " order by EntryDate,cast(BillNo AS INTEGER)")
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).readonly = False : .Cells(2).readonly = True
                        .Cells(3).readonly = True : .Cells(4).readonly = True
                        .Cells(5).readonly = True : .Cells(6).readonly = True
                        .Cells(7).readonly = True : .Cells(8).readonly = True
                        .Cells(9).readonly = True : .Cells(10).readonly = True
                        .Cells(11).readonly = True : .Cells(1).readonly = True
                        .Cells(1).Value = dt.Rows(i)("id").ToString()
                        .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(3).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(4).Value = dt.Rows(i)("VehicleNo").ToString()
                        .Cells(5).Value = dt.Rows(i)("SallerName").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(.cells(11).value) - Val(Val(.cells(8).value) + Val(.cells(9).value)), "0.00")
                        .Cells(12).Value = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID =" & Val(dt.Rows(i)("SallerID").ToString()) & "")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Public Sub retrive1(Optional ByVal Primary As String = "", Optional ByVal Secondary As String = "")
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
                            .Cells(12).Value = 0 ' IIf(Val(dt.Rows(i)("TotalCharges").ToString()) = 0, "", Format(Val(dt.Rows(i)("TotalCharges").ToString()), "0.00"))
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
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtRoundoff.Text = Format(Val(txtRoundoff.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
        Next
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                btnShow.PerformClick()
                Exit Sub
            End If
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As Integer = Val(dg1.SelectedRows(0).Cells(1).Value)

            Sellout_Mannual.MdiParent = MainScreenForm
            Sellout_Mannual.Show()
            Sellout_Mannual.FillContros(tmpID)
            Sellout_Mannual.BringToFront()
            e.SuppressKeyPress = True
        End If

    End Sub
    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 26
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
        End With
    End Sub
    Private Sub IDGentate()
        '  Dim checkBox As DataGridViewCheckBoxCell
        Dim id As String = String.Empty
        For Each row As DataGridViewRow In dg1.Rows
            If row.Cells(0).Value = True Then
                id = id & row.Cells(1).Value & ","
            End If

        Next
        If id = "" Then Exit Sub
        id = id.Remove(id.LastIndexOf(","))
        retrive2(id)
    End Sub
    Sub retrive2(ByVal id As String)
        TempRowColumn()
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        '  dt = clsFun.ExecDataTable("Select * From ScripPrint Where (ID=" & txtid.Text & ")")
        dt = clsFun.ExecDataTable(" Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.SallerName, Vouchers.VehicleNo, " _
                                & " Transaction1.ItemName, Transaction1.Lot, Transaction1.Nug as TransNug, Transaction1.Weight, Transaction1.Rate," _
                                & " Transaction1.Per, Transaction1.Amount, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, " _
                                & " Vouchers.TotalCharges, Items.OtherName, Accounts.OtherName FROM ((Vouchers INNER JOIN Transaction1 ON Vouchers.ID = Transaction1.VoucherID) " _
                                & " INNER JOIN Items ON Transaction1.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID Where (Vouchers.ID in(" & id & "))")
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
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
        Sellout_Mannual.MdiParent = MainScreenForm
        Sellout_Mannual.Show()
        Sellout_Mannual.FillContros(tmpID)
        Sellout_Mannual.BringToFront()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        IDGentate()
        'clsFun.CloseConnection()
        'clsFun.changeCompany()
        PrintRecord()
        Report_Viewer.printReport("\MannualBeejak.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PrintRecordList()
        Report_Viewer.printReport("\Reports\MannualSelloutRegister.rpt")
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
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(6).Value & "'," & _
                    "'" & Format(Val(.Cells(7).Value), "0.00") & "'," & Format(Val(.Cells(8).Value), "0.00") & ",'" & Format(.Cells(9).Value, "0.00") & "'," & _
                    "'" & Format(Val(.Cells(10).Value), "0.00") & "'," & Format(Val(.Cells(11).Value), "0.00") & ",'" & Format(Val(txtTotNug.Text), "0.00") & "','" & Format(Val(txtTotweight.Text), "0.00") & "'," & _
                    "'" & Format(Val(txtTotBasic.Text), "0.00") & "','" & Format(Val(txtTotCharge.Text), "0.00") & "','" & Format(Val(txtRoundoff.Text), "0.00") & "','" & Format(Val(TxtGrandTotal.Text), "0.00") & "', '" & .Cells(12).Value & "');"
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
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        IDGentate()
        'clsFun.CloseConnection()
        'clsFun.changeCompany()
        PrintRecord()
        Report_Viewer.printReport("\SellOutNakal.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
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


End Class