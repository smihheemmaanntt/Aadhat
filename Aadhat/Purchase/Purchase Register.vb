Imports System.IO

Public Class Purchase_Register
    Private WhatsappCheckBox As CheckBox = New CheckBox()
    Dim whatsappSender As New WhatsAppSender()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        clsFun.DoubleBuffered(DgWhatsapp, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub Super_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If isBackgroundWorkerRunning Then
                Me.Hide()
                Me.Top = 0 : Me.Left = 0
            Else
                If MessageBox.Show("Are You Sure want to Exit Purchase Register ??", "Exit Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                End If
            End If
        End If
    End Sub
    Private Sub WhatsappCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        DgWhatsapp.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = WhatsappCheckBox.Checked
        Next
    End Sub

    Private Sub DgWhatsapp_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 1 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In DgWhatsapp.Rows
                If Convert.ToBoolean(row.Cells("chk").EditedFormattedValue) = False Then
                    isChecked = True
                    Exit For
                End If
            Next
            WhatsappCheckBox.Checked = isChecked
        End If
    End Sub
    Private Sub Purchase_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) from Vouchers where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) from Vouchers where transtype='" & Me.Text & "'")
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
        dg1.ColumnCount = 12
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 95
        dg1.Columns(2).Name = "No." : dg1.Columns(2).Width = 80
        dg1.Columns(3).Name = "V. No." : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "Type" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Account Name" : dg1.Columns(5).Width = 240
        dg1.Columns(6).Name = "Nug" : dg1.Columns(6).Width = 80
        dg1.Columns(7).Name = "Weight" : dg1.Columns(7).Width = 80
        dg1.Columns(8).Name = "Basic" : dg1.Columns(8).Width = 80
        dg1.Columns(9).Name = "Charges" : dg1.Columns(9).Width = 80
        dg1.Columns(10).Name = "R.off" : dg1.Columns(10).Width = 75
        dg1.Columns(11).Name = "Total" : dg1.Columns(11).Width = 110
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtRoundOff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtRoundOff.Text = Format(Val(txtRoundOff.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            dg1.Rows(i).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            dg1.Rows(i).Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
        Next
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and Transtype='Purchase' " & condtion & " order by EntryDate,BillNo")
        Dim dvData As DataView = New DataView(dt)
        Try
            If dt.Rows.Count > 0 Then
                If dt.Rows.Count > 20 Then dg1.Columns(10).Width = 55 Else dg1.Columns(10).Width = 75
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("billNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                        .Cells(4).Value = dt.Rows(i)("PurchaseType").ToString()
                        .Cells(5).Value = dt.Rows(i)("SallerName").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Kg").ToString()), "0.00")
                        .Cells(8).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(.Cells(11).Value) - Val(Val(.Cells(8).Value) + Val(.Cells(9).Value)), "0.00")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub txtAccountName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccountName.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtAccountName.Text.Trim() <> "" Then
                retrive("And SallerName Like '%" & txtAccountName.Text.Trim() & "%'")
            End If
            If txtAccountName.Text.Trim() = "" Then
                retrive()
            End If
            e.SuppressKeyPress = True
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
            Purchase.MdiParent = MainScreenForm
            Purchase.Show()
            Purchase.FillControls(tmpID)
            If Not Purchase Is Nothing Then
                Purchase.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Purchase.MdiParent = MainScreenForm
        Purchase.Show()
        Purchase.FillControls(tmpID)
        If Not Purchase Is Nothing Then
            Purchase.BringToFront()
        End If
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
    Sub RowColumsWhatsapp()
        DgWhatsapp.Columns.Clear() : DgWhatsapp.ColumnCount = 7
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        WhatsappCheckBox.Location = New Point(headerCellLocation.X + 10, headerCellLocation.Y + 2)
        WhatsappCheckBox.BackColor = Color.GhostWhite
        WhatsappCheckBox.Size = New Size(18, 18)
        AddHandler WhatsappCheckBox.Click, AddressOf WhatsappCheckBox_Clicked
        DgWhatsapp.Controls.Add(WhatsappCheckBox)
        Dim checkBoxColumn1 As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn1.HeaderText = "" : checkBoxColumn1.Width = 30
        checkBoxColumn1.Name = "checkBoxColumn"
        DgWhatsapp.Columns.Insert(0, checkBoxColumn1)
        DgWhatsapp.Columns(0).ReadOnly = False
        AddHandler DgWhatsapp.CellContentClick, AddressOf DgWhatsapp_CellClick
        DgWhatsapp.Columns(1).Name = "VoucherID" : DgWhatsapp.Columns(1).Visible = False
        DgWhatsapp.Columns(2).Name = "Bill No." : DgWhatsapp.Columns(2).Width = 120
        DgWhatsapp.Columns(3).Name = "WhatsApp No" : DgWhatsapp.Columns(3).Width = 150
        DgWhatsapp.Columns(4).Name = "Account Name" : DgWhatsapp.Columns(4).Width = 150
        DgWhatsapp.Columns(5).Name = "Status" : DgWhatsapp.Columns(5).Width = 300
        DgWhatsapp.Columns(6).Name = "Path" : DgWhatsapp.Columns(6).Visible = False
    End Sub

    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 26
            .Columns(0).Name = "ID" : .Columns(0).Visible = False
            .Columns(1).Name = "Date" : .Columns(1).Width = 95
            .Columns(2).Name = "VoucherNo" : .Columns(2).Width = 159
            .Columns(3).Name = "AccountName" : .Columns(3).Width = 159
            .Columns(4).Name = "ShopNo" : .Columns(4).Width = 59
            .Columns(5).Name = "VehicleNo" : .Columns(5).Width = 59
            .Columns(6).Name = "itemName" : .Columns(6).Width = 69
            .Columns(7).Name = "LotNo" : .Columns(7).Width = 76
            .Columns(8).Name = "Nug" : .Columns(8).Width = 90
            .Columns(9).Name = "Weight" : .Columns(9).Width = 86
            .Columns(10).Name = "Rate" : .Columns(10).Width = 90
            .Columns(11).Name = "Per" : .Columns(11).Width = 50
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

    Sub retrive3(ByVal id As String)
        TempRowColumn()
        Dim i, j As Integer
        Dim dt As New DataTable : Dim dt1 As New DataTable
        Dim dt2 As New DataTable : Dim cnt As Integer = -1
        tmpgrid.Rows.Clear() : Dim sql As String = String.Empty
        LineMargin = clsFun.ExecScalarInt("Select Margin From Controls")
        Dim margin As String = String.Empty
        If LineMargin = 0 Then margin = vbCrLf
        If LineMargin = 1 Then margin = vbCrLf & vbCrLf
        If LineMargin = 2 Then margin = vbCrLf & vbCrLf
        If LineMargin = 3 Then margin = vbCrLf & vbCrLf & vbCrLf
        If LineMargin = 4 Then margin = vbCrLf & vbCrLf & vbCrLf & vbCrLf
        If LineMargin = 5 Then margin = vbCrLf & vbCrLf & vbCrLf & vbCrLf & vbCrLf
        'sql = " Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.SallerName, Vouchers.VehicleNo, " _
        '                        & " Purchase.ItemName, Purchase.Lot, Purchase.Nug as TransNug, Purchase.Weight, Purchase.Rate," _
        '                        & " Purchase.Per, Purchase.Amount,Purchase.AddWeight, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, " _
        '                        & " Vouchers.T1, Vouchers.T2, Vouchers.T3, Vouchers.T4, Vouchers.T5, Vouchers.T6,Vouchers.T7, Vouchers.T8, Vouchers.T9, Vouchers.T10,Vouchers.T11, " _
        '                        & " Vouchers.TotalCharges, Items.OtherName, Accounts.OtherName FROM ((Vouchers INNER JOIN Purchase ON Vouchers.ID = Purchase.VoucherID) " _
        '                        & " INNER JOIN Items ON Purchase.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID Where (Vouchers.ID in (" & id & "))"

        sql = " Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.SallerName, Vouchers.StorageName,Vouchers.VehicleNo, " _
                             & " Purchase.ItemName, Purchase.LotNo, Purchase.Nug as TransNug, Purchase.Weight, Purchase.Rate," _
                             & " Purchase.Per, Purchase.Amount, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount, " _
                             & " Vouchers.TotalCharges, Items.OtherName, Accounts.OtherName FROM ((Vouchers INNER JOIN Purchase ON Vouchers.ID = Purchase.VoucherID) " _
                             & " INNER JOIN Items ON Purchase.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID Where (Vouchers.ID in(" & id & "))"
        dt = clsFun.ExecDataTable(sql)
        'dt2 = clsFun.ExecDataTable(sql)
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
                    .Cells(4).Value = .Cells(5).Value & dt.Rows(i)("StorageName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("LotNo").ToString()
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
                    '.Cells(25).Value = .Cells(25).Value & dt.Rows(i)("AddWeight").ToString()
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
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
    Private Sub PrintBills()
        Dim FastQuery As String = String.Empty
        Dim sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        ' clsFun.ExecNonQuery(sql)
        For Each row As DataGridViewRow In tmpgrid.Rows
            'For Each row As DataGridViewRow In .Rows
            With row
                If .Cells(6).Value <> "" Then
                    Dim amtInWords As String = String.Empty
                    Try
                        amtInWords = AmtInWord(.Cells(21).Value)
                    Catch ex As Exception
                        amtInWords = ex.ToString
                    End Try
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(1).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "','" & .Cells(4).Value & "'," &
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                    "'" & .Cells(9).Value & "','" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "', " &
                    "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                    "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & .Cells(19).Value & "','" & .Cells(20).Value & "', " &
                    "'" & .Cells(21).Value & "','" & .Cells(22).Value & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "','" & amtInWords & "'"
                End If
            End With
        Next
        Try
            If FastQuery = String.Empty Then Exit Sub
            sql = "insert into Printing(D1, D2,M1,M2,M3,M4, P1,P2, P3, P4, P5, P6,P7,P8,P9, " &
                    " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20,  P21,P22)" & FastQuery & ""
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try

    End Sub
    Private Sub IDGenrate()
        Dim checkBox As DataGridViewCheckBoxCell
        Dim id As String = String.Empty
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                id = id & row.Cells(1).Value & ","
            End If
        Next
        If id = "" Then
            retrive3(id)
        Else
            id = id.Remove(id.LastIndexOf(","))
            retrive3(id)
        End If
    End Sub
    Private Sub SendWhatsappData()
        Dim fastQuery As String = String.Empty
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        If Not Directory.Exists(directoryName) Then
            Directory.CreateDirectory(directoryName)
        Else
            Directory.Delete(directoryName, True)
            Directory.CreateDirectory(directoryName) ' Recreate the directory
        End If

        ' ProgressBar setup
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = DgWhatsapp.RowCount
        UpdateProgressBarVisibility(True)

        Dim selectedIndex As Integer
        If cbType.InvokeRequired Then
            cbType.Invoke(Sub() selectedIndex = cbType.SelectedIndex)
        Else
            selectedIndex = cbType.SelectedIndex
        End If

        ' Delete from SendingData based on selectedIndex
        If selectedIndex = 0 Then
            ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        Else
            WABA.ExecNonQuery("Delete from SendingData")
        End If

        ' Filter rows where the checkbox is checked (True) and process only those
        ' Filter rows where the checkbox is checked (True) and Cell(2) is not empty
        Dim filteredRows As List(Of DataGridViewRow) = DgWhatsapp.Rows.Cast(Of DataGridViewRow)().
            Where(Function(r) r.Cells(0).Value = True AndAlso Not String.IsNullOrEmpty(r.Cells(3).Value.ToString())).ToList()

        ' ProgressBar update for filtered rows
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(Sub() ProgressBar1.Maximum = filteredRows.Count)
        Else
            ProgressBar1.Maximum = filteredRows.Count
        End If
        For Each row As DataGridViewRow In filteredRows
            With row
                If .Cells(3).Value.trim() <> String.Empty Then
                    UpdateProgressBar(row.Index)
                    GlobalData.PdfName = .Cells(4).Value.Replace("/", "") & ".pdf"
                    retrive3(.Cells(1).Value) : PrintBills()
                    Pdf_Genrate.ExportReport("\Purchase.rpt")
                    If selectedIndex = 0 Then
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(2).Value & "','" & .Cells(4).Value & "',91" & .Cells(3).Value & ",'" & GlobalData.PdfPath & "'"
                    Else
                        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(2).Value & "','" & .Cells(4).Value & "','" & .Cells(3).Value & "','" & whatsappSender.FilePath & "'"
                    End If
                End If
            End With
        Next
        ' If no data found, exit the method
        If String.IsNullOrEmpty(fastQuery) Then
            UpdateProgressBarVisibility(False)
            Exit Sub
        End If

        Try
            Dim sql As String = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) " & fastQuery & ";Update Settings Set MinState='N'"
            If selectedIndex = 0 Then
                ClsFunWhatsapp.ExecNonQuery(sql)
            Else
                WABA.ExecNonQuery(sql)
            End If

            ' Check if WahSoft.exe exists
            Dim WhatsappFile As String = Application.StartupPath & "\WahSoft\WahSoft.exe"
            If Not System.IO.File.Exists(WhatsappFile) Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If

            ' Check if WahSoft is already running
            Dim p() As Process = Process.GetProcessesByName("wahSoft")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\WahSoft\WahSoft.exe"
                StartWhatsapp.Start()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            UpdateProgressBarVisibility(False)
            ClsFunWhatsapp.CloseConnection()
            WABA.CloseConnection()
        End Try

        UpdateProgressBarVisibility(False)
    End Sub

    Private Sub ShowWhatsappContacts(Optional ByVal condtion As String = "")
        DgWhatsapp.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        Dim SSql As String = "Select * From Vouchers Where TransType='Purchase' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(SSql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                DgWhatsapp.Rows.Add()
                With DgWhatsapp.Rows(i)
                    .Cells(1).Value = dt.Rows(i)("ID").ToString()
                    .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = clsFun.ExecScalarStr("Select MObile1 From Accounts Where ID='" & Val(dt.Rows(i)("SallerID").ToString()) & "'")
                    .Cells(4).Value = dt.Rows(i)("SaLLerName").ToString()
                    .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                    .Cells(0).Value = True
                End With
            Next i
        End If
        DgWhatsapp.ClearSelection()
    End Sub

    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
            'MsgBox("A background task is running. you can Use your Task", MsgBoxStyle.Information, "Background Task")
        Else
            MsgBox("A background task is already running.", MsgBoxStyle.Information, "Background Task")
        End If
    End Sub
    Private Sub bgWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        isBackgroundWorkerRunning = True
        Dim action As Action = CType(e.Argument, Action)
        action.Invoke()
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        isBackgroundWorkerRunning = False
    End Sub

    Private Sub UpdateProgressBar(value As Integer)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Integer)(AddressOf UpdateProgressBar), value)
        Else
            ProgressBar1.Value = value
        End If
    End Sub

    Private Sub UpdateProgressBarVisibility(visible As Boolean)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Boolean)(AddressOf UpdateProgressBarVisibility), visible)
        Else
            ProgressBar1.Visible = visible
        End If
    End Sub

    Private Sub btnPrintBills_Click(sender As Object, e As EventArgs) Handles btnPrintBills.Click
            If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
            pnlWhatsapp.Visible = True : ShowWhatsappContacts()
            cbType.SelectedIndex = 0
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If cbType.SelectedIndex = 0 Then
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
            SendWhatsappData()
        Else
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        IDGenrate() : PrintBills()
        Report_Viewer.printReport("\Purchase.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        Report_Viewer.BringToFront()
    End Sub

    Private Sub btnPnlVisHide_Click(sender As Object, e As EventArgs) Handles btnPnlVisHide.Click
        pnlWhatsapp.Hide()
    End Sub
    Private Sub DgWhatsapp_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellEndEdit
        If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(1).Value & "'") = "" And DgWhatsapp.CurrentRow.Cells(3).Value <> "" Then
            clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(1).Value) & "'")
        Else
            If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(1).Value & "'") <> DgWhatsapp.CurrentRow.Cells(3).Value Then
                If MessageBox.Show("Are you Sure to Change Mobile No in PhoneBook", "Change Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(1).Value) & "'")
                End If
            End If
        End If
    End Sub

    Private Sub txtAccountName_TextChanged(sender As Object, e As EventArgs) Handles txtAccountName.TextChanged

    End Sub
End Class