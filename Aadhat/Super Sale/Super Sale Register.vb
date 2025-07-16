Imports System.IO

Public Class Super_Sale_Register
    Dim strSDate As String : Dim strEDate As String
    Dim dDate As DateTime : Dim mskstartDate As String
    Dim mskenddate As String
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

    Private Sub Super_Sale_Register_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isBackgroundWorkerRunning Then
            e.Cancel = True
            Me.Hide()
            Me.Top = 0 : Me.Left = 0
        End If
    End Sub

    Private Sub Super_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
      If e.KeyCode = Keys.Escape Then
            If isBackgroundWorkerRunning Then
                Me.Hide()
                Me.Top = 0 : Me.Left = 0
            Else
                If MessageBox.Show("Are You Sure want to Exit Super Sale Register ??", "Exit Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    Me.Close()
                End If
            End If
        End If
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
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
    Private Sub Super_Sale_Register_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
        If mindate <> "" Then mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy") Else mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        If maxdate <> "" Then MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy") Else MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        RadioCustomerWise.Checked = True : rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 16
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 80 : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "No." : dg1.Columns(2).Width = 60 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "V.No." : dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Seller" : dg1.Columns(4).Width = 150 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(5).Name = "Item" : dg1.Columns(5).Width = 100 : dg1.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(6).Name = "Lot" : dg1.Columns(6).Visible = False
        dg1.Columns(7).Name = "Customer" : dg1.Columns(7).Width = 200 : dg1.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft : dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(8).Name = "Nug" : dg1.Columns(8).Width = 60 : dg1.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Kg" : dg1.Columns(9).Width = 60 : dg1.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "Rate" : dg1.Columns(10).Width = 80 : dg1.Columns(10).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).Name = "Per" : dg1.Columns(11).Width = 50 : dg1.Columns(11).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(12).Name = "Basic" : dg1.Columns(12).Width = 80 : dg1.Columns(12).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(13).Name = "Charges" : dg1.Columns(13).Width = 80 : dg1.Columns(13).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(13).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(14).Name = "R.Off" : dg1.Columns(14).Width = 80 : dg1.Columns(14).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(14).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(15).Name = "Total" : dg1.Columns(15).Width = 90 : dg1.Columns(15).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(15).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight : dg1.Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtTotRoundOff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(8).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(13).Value), "0.00")
            txtTotRoundOff.Text = Format(Val(txtTotRoundOff.Text) + Val(dg1.Rows(i).Cells(14).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(15).Value), "0.00")
        Next
    End Sub
    Private Sub txtTotalSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtTotalSearch.KeyUp
        If txtTotalSearch.Text.Trim() <> "" Then
            retrive("And t.TotalAmount Like '" & txtTotalSearch.Text.Trim() & "%'")
        End If
        If txtTotalSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtItemSearch.KeyUp
        If txtItemSearch.Text.Trim() <> "" Then
            retrive("And i.ItemName Like '" & txtItemSearch.Text.Trim() & "%'")
        End If
        If txtItemSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub

    Private Sub txtCustomerSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyDown

    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            retrive("And a.AccountName Like '" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Dim recordCount As Integer = 0
    Public Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim RecordCount As Integer = 0
        dt = clsFun.ExecDataTable("Select * FROM Vouchers v INNER JOIN Transaction2 t ON v.id = t.VoucherID INNER JOIN Accounts a1 ON a1.ID = v.sallerID INNER JOIN Accounts a ON a.ID = t.AccountID INNER JOIN Items i ON i.ID = t.ItemID Where v.EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and v.transtype='" & Me.Text & "' " & condtion & "  order by  v.InvoiceID,v.EntryDate ")
        Dim vchid As Integer = 0
        Try
            If dt.Rows.Count > 20 Then dg1.Columns(14).Width = 60 Else dg1.Columns(14).Width = 80
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    '    Application.DoEvents()
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("Voucherid").ToString()
                        If i = 0 Then
                            '   pb1.Value = i : pnlWait.Visible = True
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("AccountName2").ToString()
                            lblbillCount.Visible = True
                            RecordCount = RecordCount + 1
                        ElseIf vchid = dt.Rows(i)("Voucherid").ToString() Then
                            '  pb1.Value = i
                            .Cells(1).Value = ""
                            .Cells(2).Value = ""
                            .Cells(3).Value = ""
                            .Cells(4).Value = ""
                        Else
                            '           pb1.Value = i
                            .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                            .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                            .Cells(3).Value = dt.Rows(i)("VehicleNo").ToString()
                            .Cells(4).Value = dt.Rows(i)("AccountName2").ToString()
                            RecordCount = RecordCount + 1
                        End If
                        .Cells(5).Value = dt.Rows(i)("ItemName2").ToString()
                        .Cells(6).Value = dt.Rows(i)("Lot").ToString()
                        .Cells(7).Value = dt.Rows(i)("AccountName3").ToString()
                        .Cells(8).Value = Format(Val(dt.Rows(i)("Nug1").ToString()), "0.00")
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("Rate1").ToString()), "0.00")
                        .Cells(11).Value = dt.Rows(i)("Per1").ToString()
                        .Cells(12).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(13).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        .Cells(15).Value = Format(Val(dt.Rows(i)("TotalAmount1").ToString()), "0.00")
                        .Cells(14).Value = Format(Val(.Cells(15).Value) - Val(Val(.Cells(13).Value) + Val(.Cells(12).Value)), "0.00")
                        .Cells(1).readOnly = True : .Cells(2).readOnly = True
                        .Cells(3).readOnly = True : .Cells(4).readOnly = True
                        .Cells(5).readOnly = True : .Cells(6).readOnly = True
                        .Cells(7).readOnly = True : .Cells(8).readOnly = True
                        .Cells(9).readOnly = True : .Cells(10).readOnly = True
                        .Cells(11).readOnly = True : .Cells(12).readOnly = True
                        .Cells(13).readOnly = True : .Cells(14).readOnly = True
                    End With
                    vchid = dt.Rows(i)("Voucherid").ToString()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        lblvoucherCount.Visible = True
        lblvoucherCount.Text = "Entries : " & dg1.RowCount
        lblbillCount.Text = "Bills : " & RecordCount
        calc() : dg1.ClearSelection() : pnlWait.Visible = False
    End Sub
    Private Sub txtAccountName_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccountName.KeyUp
        If txtAccountName.Text.Trim() <> "" Then
            retrive("And a1.AccountName Like '" & txtAccountName.Text.Trim() & "%'")
        End If
        If txtAccountName.Text.Trim() = "" Then
            retrive()
        End If
    End Sub


    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub
    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,M1,M2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12,P13,T1,T2,T3,T4,T5) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "'," & _
                    "'" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & .Cells(9).Value & "'," & _
                    "'" & .Cells(10).Value & "','" & .Cells(11).Value & "','" & .Cells(12).Value & "','" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & txtTotNug.Text & "','" & txtTotweight.Text & "'," & _
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

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
        Super_Sale.MdiParent = MainScreenForm
        Super_Sale.Show()
        Super_Sale.FillControls(tmpID)
        If Not Super_Sale Is Nothing Then
            Super_Sale.BringToFront()
        End If
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(0).Value
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(tmpID)
            If Not Super_Sale Is Nothing Then
                Super_Sale.BringToFront()
            End If
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\SuperSaleRegisterDetailed.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub dg1_MouseClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseClick
        dg1.ClearSelection()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub txtAccountName_TextChanged(sender As Object, e As EventArgs) Handles txtAccountName.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

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
    Private Sub SendWhatsappData()
        Dim fastQuery As String = String.Empty
        Dim count As Integer = 0
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        If Not Directory.Exists(directoryName) Then
            Directory.CreateDirectory(directoryName)
        Else
            Directory.Delete(directoryName, True)
            Directory.CreateDirectory(directoryName) ' Recreate the directory
        End If
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        Dim filteredRows As List(Of DataGridViewRow) = DgWhatsapp.Rows.Cast(Of DataGridViewRow)().
            Where(Function(r) r.Cells(0).Value = True AndAlso Not String.IsNullOrEmpty(r.Cells(3).Value.ToString())).ToList()
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(Sub() ProgressBar1.Maximum = filteredRows.Count)
        Else
            ProgressBar1.Maximum = filteredRows.Count
        End If
        UpdateProgressBarVisibility(True)
        For Each row As DataGridViewRow In filteredRows
            With row
                If .Cells(3).Value.trim() <> String.Empty Then
                    UpdateProgressBar(count)
                    GlobalData.PdfName = .Cells(4).Value.Replace("/", "") & ".pdf"
                    retrive3(.Cells(1).Value) : PrintBills()
                    Pdf_Genrate.ExportReport("\SuperSaleBeejak.rpt")
                    If selectedIndex = 0 Then
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(2).Value & "','" & .Cells(4).Value & "',91" & .Cells(3).Value & ",'" & GlobalData.PdfPath & "'"
                    Else
                        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(2).Value & "','" & .Cells(4).Value & "','" & .Cells(3).Value & "','" & whatsappSender.FilePath & "'"
                    End If
                End If
            End With
            count += 1
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
                Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                If Not System.IO.File.Exists(WhatsappFile) Then
                    MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                    Exit Sub
                End If
                ' Check if WahSoft is already running
                Dim p() As Process = Process.GetProcessesByName("Easy Whatsapp")
                If p.Count = 0 Then
                    Dim StartWhatsapp As New System.Diagnostics.Process
                    StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                    StartWhatsapp.Start()
                End If
            Else
                WABA.ExecNonQuery(sql)
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
            End If

            ' Check if WahSoft.exe exists
          
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
        Dim SSql As String = "Select * From Vouchers Where TransType='Super Sale' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' Order by Cast(BIllNo as Integer)"
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

    Private Sub TempRowColumn()
        With tmpgrid
            .ColumnCount = 37
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
            .Columns(10).Name = "SRate" : .Columns(10).Width = 90
            .Columns(11).Name = "per" : .Columns(11).Width = 50
            .Columns(12).Name = "SallerAmount" : .Columns(12).Width = 95
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
            .Columns(26).Name = "Addweight" : .Columns(26).Width = 159
            .Columns(27).Name = "Driver Name" : .Columns(27).Width = 159
            .Columns(28).Name = "Mobile" : .Columns(28).Width = 159
            .Columns(29).Name = "Remark" : .Columns(29).Width = 159
            .Columns(30).Name = "GrNo" : .Columns(30).Width = 159
            .Columns(31).Name = "GSTN" : .Columns(31).Width = 159
            .Columns(32).Name = "Cust Mobile" : .Columns(32).Width = 159
            .Columns(33).Name = "Broker Name" : .Columns(33).Width = 159
            .Columns(34).Name = "Broker Mobile" : .Columns(34).Width = 159
            .Columns(35).Name = "TransPort" : .Columns(35).Width = 159
            .Columns(36).Name = "GRNo" : .Columns(36).Width = 159
        End With
    End Sub
    Private Sub retrive2(ByVal id As String)
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        dt = clsFun.ExecDataTable(" Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.sallerName, Vouchers.VehicleNo,Transaction2.OnWeight, " _
                               & " Transaction2.ItemName, Transaction2.Cut, sum(Transaction2.Nug) as Transnug, sum(Transaction2.Weight) as Weight, Transaction2.SRate," _
                               & " Transaction2.Per, sum(Transaction2.SallerAmt) as SallerAmt, Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount," _
                               & " Vouchers.totalcharges, Items.OtherName as OtherItemName, Accounts.OtherName as OtherName,Vouchers.T1,Vouchers.T2,Vouchers.T3,Vouchers.T4, " _
                               & " Vouchers.T5,Vouchers.T6,Vouchers.T7,Vouchers.T8,Vouchers.T9,Vouchers.T10 FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID) " _
                               & " INNER JOIN Items ON Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID Where (Vouchers.ID in ('" & id & "')) Group by Transaction2.ItemName,Transaction2.SRate,Transaction2.Per")
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                TempRowColumn()
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("SallerName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Cut").ToString()
                    .Cells(8).Value = .Cells(8).Value & dt.Rows(i)("Transnug").ToString()
                    .Cells(9).Value = .Cells(9).Value & dt.Rows(i)("Weight").ToString()
                    .Cells(10).Value = .Cells(10).Value & dt.Rows(i)("SRate").ToString()
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = .Cells(12).Value & dt.Rows(i)("SallerAmt").ToString()
                    .Cells(18).Value = .Cells(18).Value & dt.Rows(i)("Nug").ToString()
                    .Cells(19).Value = .Cells(19).Value & dt.Rows(i)("Kg").ToString()
                    .Cells(20).Value = .Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()
                    .Cells(21).Value = .Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()
                    .Cells(22).Value = .Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherItemName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & dt.Rows(i)("OnWeight").ToString()
                    .Cells(27).Value = dt.Rows(i)("T1").ToString()
                    .Cells(28).Value = dt.Rows(i)("T2").ToString()
                    .Cells(29).Value = dt.Rows(i)("T3").ToString()
                    .Cells(30).Value = dt.Rows(i)("T4").ToString()
                    .Cells(31).Value = dt.Rows(i)("T5").ToString()
                    .Cells(32).Value = dt.Rows(i)("T6").ToString()
                    .Cells(33).Value = dt.Rows(i)("T7").ToString()
                    .Cells(34).Value = dt.Rows(i)("T8").ToString()
                    .Cells(35).Value = dt.Rows(i)("T9").ToString()
                    .Cells(36).Value = dt.Rows(i)("T10").ToString()
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & dt1.Rows(j)("OnValue").ToString() & vbCrLf
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
        dt.Clear() : dt1.Clear()
    End Sub
    Sub retrive3(ByVal id As String)
        'MsgBox(txtid.Text)
        Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim cnt As Integer = -1
        tmpgrid.Rows.Clear()
        dt = clsFun.ExecDataTable("  Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.sallerName, Vouchers.VehicleNo,Transaction2.OnWeight, Transaction2.ItemName, Transaction2.Cut, sum(Transaction2.Nug) as Transnug, " _
                               & "  Round(sum(Transaction2.Weight),2)  as Weight , Round(avg(Transaction2.SRate),2) as SRate, Transaction2.Per, sum(Transaction2.SallerAmt) as SallerAmt , Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount," _
                               & " Vouchers.totalcharges, Items.OtherName as OtherItemName, Accounts.OtherName FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)  INNER JOIN Items ON" _
                               & " Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID " _
                               & "  Where (Vouchers.ID in(" & id & "))Group by  Vouchers.ID,  Transaction2.ItemName,Transaction2.SRate  order by Vouchers.EntryDate,Vouchers.billNo, Transaction2.ItemName,Transaction2.SRate Desc")
        'dt = clsFun.ExecDataTable("  Select Vouchers.ID, Vouchers.Entrydate, Vouchers.billNo, Vouchers.sallerName, Vouchers.VehicleNo,Transaction2.OnWeight, Transaction2.ItemName, Transaction2.Cut, sum(Transaction2.Nug) as Transnug, " _
        '                       & "  Round(sum(Transaction2.Weight),2)  as Weight , Round(avg(Transaction2.SRate),2) as SRate, Transaction2.Per, sum(Transaction2.SallerAmt) as SallerAmt , Vouchers.Nug, Vouchers.Kg, Vouchers.BasicAmount, Vouchers.TotalAmount," _
        '                       & " Vouchers.totalcharges, Items.OtherName as OtherItemName, Accounts.OtherName FROM ((Vouchers INNER JOIN Transaction2 ON Vouchers.ID = Transaction2.VoucherID)  INNER JOIN Items ON" _
        '                       & " Transaction2.ItemID = Items.ID) INNER JOIN Accounts ON Vouchers.sallerID = Accounts.ID " _
        '                       & "  Where (Vouchers.ID in(" & id & "))Group by  Vouchers.ID, Transaction2.ItemName,Transaction2.SRate order by Transaction2.ItemName,Transaction2.SRate Desc")
        If dt.Rows.Count = 0 Then Exit Sub
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                TempRowColumn()
                tmpgrid.Rows.Add()
                cnt = cnt + 1
                With tmpgrid.Rows(cnt)
                    .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                    .Cells(2).Value = .Cells(2).Value & dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = dt.Rows(i)("SallerName").ToString()
                    .Cells(5).Value = .Cells(5).Value & dt.Rows(i)("VehicleNo").ToString()
                    .Cells(6).Value = .Cells(6).Value & dt.Rows(i)("ItemName").ToString()
                    .Cells(7).Value = .Cells(7).Value & dt.Rows(i)("Cut").ToString()
                    .Cells(8).Value = Format(Val(.Cells(8).Value & dt.Rows(i)("Transnug").ToString()), "0.00")
                    .Cells(9).Value = Format(Val(.Cells(9).Value & dt.Rows(i)("Weight").ToString()), "0.00")
                    .Cells(10).Value = Format(Val(.Cells(10).Value & dt.Rows(i)("SRate").ToString()), "0.00")
                    .Cells(11).Value = .Cells(11).Value & dt.Rows(i)("Per").ToString()
                    .Cells(12).Value = Format(Val(.Cells(12).Value & dt.Rows(i)("SallerAmt").ToString()), "0.00")
                    .Cells(18).Value = Format(Val(.Cells(18).Value & dt.Rows(i)("Nug").ToString()), "0.00")
                    .Cells(19).Value = Format(Val(.Cells(19).Value & dt.Rows(i)("Kg").ToString()), "0.00")
                    .Cells(20).Value = Format(Val(.Cells(20).Value & dt.Rows(i)("BasicAmount").ToString()), "0.00")
                    .Cells(21).Value = Format(Val(.Cells(21).Value & dt.Rows(i)("TotalAmount").ToString()), "0.00")
                    .Cells(22).Value = Format(Val(.Cells(22).Value & dt.Rows(i)("TotalCharges").ToString()), "0.00")
                    .Cells(23).Value = .Cells(23).Value & dt.Rows(i)("OtherItemName").ToString()
                    .Cells(24).Value = .Cells(24).Value & dt.Rows(i)("OtherName").ToString()
                    .Cells(26).Value = .Cells(26).Value & dt.Rows(i)("OnWeight").ToString()
                    dt1 = clsFun.ExecDataTable("Select * FROM ChargesTrans WHERE VoucherID=" & dt.Rows(i)("ID").ToString() & "")
                    '  tmpgrid.Rows.Clear()
                    If dt1.Rows.Count > 0 Then
                        For j = 0 To dt1.Rows.Count - 1
                            .Cells(13).Value = .Cells(13).Value & dt1.Rows(j)("ChargeName").ToString() & vbCrLf
                            .Cells(14).Value = .Cells(14).Value & Format(Val(dt1.Rows(j)("OnValue").ToString()), "0.00") & vbCrLf
                            .Cells(15).Value = .Cells(15).Value & Format(Val(dt1.Rows(j)("Calculate").ToString()), "0.00") & vbCrLf
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
        dt.Clear() : dt1.Clear()
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
    Private Sub btnPrintBills_Click(sender As Object, e As EventArgs) Handles btnPrintBills.Click
            If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
            pnlWhatsapp.Visible = True : ShowWhatsappContacts()
        'cbType.SelectedIndex = 0
        'Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
        'If System.IO.File.Exists(WhatsappFile) = False Then
        '    MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
        '    Exit Sub
        'End If
        'Dim p() As Process
        'p = Process.GetProcessesByName("Easy Whatsapp")
        'If p.Count = 0 Then
        '    Dim StartWhatsapp As New System.Diagnostics.Process
        '    StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
        '    StartWhatsapp.Start()
        'End If
    End Sub

    Private Sub btnPnlVisHide_Click(sender As Object, e As EventArgs) Handles btnPnlVisHide.Click
        pnlWhatsapp.Hide()
    End Sub
    Private Sub PrintBills()
        Dim FastQuery As String = String.Empty
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In tmpgrid.Rows
            With row
                If .Cells(6).Value <> "" Then
                    Dim amtInWords As String = String.Empty
                    Try
                        amtInWords = AmtInWord(.Cells(21).Value)
                    Catch ex As Exception
                        amtInWords = ex.ToString
                    End Try
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                                "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "', " &
                                "'" & Format(Val(.Cells(9).Value), "0.00") & "','" & Format(Val(.Cells(10).Value), "0.00") & "','" & .Cells(11).Value & "','" & Format(Val(.Cells(12).Value), "0.00") & "', " &
                                "'" & .Cells(13).Value & "','" & .Cells(14).Value & "','" & .Cells(15).Value & "','" & .Cells(16).Value & "', " &
                                "'" & .Cells(17).Value & "','" & .Cells(18).Value & "','" & Format(Val(.Cells(19).Value), "0.00") & "','" & Format(Val(.Cells(20).Value), "0.00") & "', " &
                                "'" & Format(Val(.Cells(21).Value), "0.00") & "','" & Format(Val(.Cells(22).Value), "0.00") & "','" & .Cells(23).Value & "','" & .Cells(24).Value & "', " &
                                "'" & amtInWords & "','" & IIf(.Cells(26).Value = "", "", "(" & .Cells(26).Value & ")") & "','" & opbal & "'," &
                                "'" & ClBal & "'," & Val(0) & ""
                End If
            End With
        Next
        If FastQuery = "" Then Exit Sub
        sql = "insert into Printing(D1, D2,M1, P1,P2, P3, P4, P5, P6,P7,P8,P9, " &
                        " P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,P20, " &
                        " P21,P22,P23,P24,P25,P26)" & FastQuery & ""
        Try
            ClsFunPrimary.ExecNonQuery(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        IDGenrate() : PrintBills()
        Report_Viewer.printReport("\SuperSaleBeejak.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        Report_Viewer.BringToFront()
    End Sub
    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub

    Private Sub WhatsAppAPI()
        Dim sql As String = String.Empty
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            With row
                ProgressBar1.Value = row.Index
                If .Cells(0).Value = True Then
                    retrive3(.Cells(1).Value) : PrintBills()
                    GlobalData.PdfName = .Cells(4).Value & ".pdf"
                    Pdf_Genrate.ExportReport("\SuperSaleBeejak.rpt")
                    whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Whatsapp\Pdfs\" & GlobalData.PdfName)
                    whatsappSender.SendWhatsAppFile("91" & .Cells(3).Value, .Cells(4).Value & vbCrLf & "Sended By: Aadhat Software" & vbCrLf & "www.softmanagementindia.in", whatsappSender.FilePath)
                    .Cells(5).Value = "PDF Sent " & whatsappSender.APIResposne
                    sql = "insert into waReport(EntryDate,AccountName,WhatsAppNo,Type,Status) SELECT '" & Date.Today.ToString("yyyy-MM-dd") & "','" & .Cells(4).Value & "','" & .Cells(3).Value & "','Receipt','" & .Cells(5).Value & "'"
                    clsFun.ExecNonQuery(sql)
                End If
            End With
        Next
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
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

    'MsgBox("Comming Soon...")
    'End Sub
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

    Private Sub txtSearchAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchAccount.KeyUp
        Dim searchValue As String = txtSearchAccount.Text.Trim().ToLower()
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            If Not row.IsNewRow Then
                Dim cellValue As String = row.Cells(4).Value.ToString().ToLower()
                If cellValue.StartsWith(searchValue) Then
                    row.Visible = True
                Else
                    row.Visible = False
                End If
            End If
        Next
    End Sub
End Class