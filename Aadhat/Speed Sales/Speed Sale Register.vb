Public Class Speed_Sale_Register
    Private headerCheckBox As CheckBox = New CheckBox()
    Dim SearchText As String = "" : Dim TotalPages As Integer = 0
    Dim PageNumber As Integer = 0 : Dim RowCount As Integer = 20
    Dim Offset As Integer = 0 : Dim totNugs As Decimal = 0
    Dim totWeight As Decimal = 0 : Dim totBasic As Decimal = 0
    Dim totCharges As Decimal = 0 : Dim totTotal As Decimal = 0
    Dim Roundoff As Decimal = 0 : Dim ID As String
    Dim Primary, Secondary, third As String
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub Speed_Sale_Register_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If pnlprint.Visible = True Then pnlprint.Visible = False : mskFromDate.Focus() : Exit Sub
            If PnlDeleteBills.Visible = True Then PnlDeleteBills.Visible = False : mskFromDate.Focus() : Exit Sub
            Me.Close()
        End If

        If dg1.Focused = False Then
            If e.KeyCode = 109 Then btnPrevious.PerformClick()
            If e.KeyCode = Keys.Add Then btnNext.PerformClick()
        End If

    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
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
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) as entrydate from transaction2 where transtype='" & Me.Text & "'")
        mskFromDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyyy"))
        MsktoDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyyy"))
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text) : MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
        rowColums()
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
    Private Sub rowColums()
        dg1.ColumnCount = 15
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        headerCheckBox.Location = New Point(headerCellLocation.X + 8, headerCellLocation.Y + 2)
        headerCheckBox.BackColor = Color.GhostWhite
        headerCheckBox.Size = New Size(18, 18)
        AddHandler headerCheckBox.Click, AddressOf HeaderCheckBox_Clicked
        dg1.Controls.Add(headerCheckBox)
        Dim checkBoxColumn As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn.HeaderText = ""
        checkBoxColumn.Width = 30
        checkBoxColumn.Name = "checkBoxColumn"
        dg1.Columns.Insert(0, checkBoxColumn)
        AddHandler dg1.CellContentClick, AddressOf dg1_CellClick
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Date" : dg1.Columns(2).Width = 95
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).Name = "Item Name" : dg1.Columns(3).Width = 150
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).Name = "Customer" : dg1.Columns(4).Width = 200
        dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(5).Name = "Nug" : dg1.Columns(5).Width = 80
        dg1.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).Name = "Kg" : dg1.Columns(6).Width = 80
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).Name = "Rate" : dg1.Columns(7).Width = 80
        dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).Name = "Per" : dg1.Columns(8).Width = 70
        dg1.Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).Name = "Net" : dg1.Columns(9).Width = 80
        dg1.Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).Name = "Charges" : dg1.Columns(10).Width = 80
        dg1.Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).Name = "R. Off" : dg1.Columns(11).Width = 80
        dg1.Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(12).Name = "Total" : dg1.Columns(12).Width = 100
        dg1.Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(13).Name = "Crate" : dg1.Columns(13).Width = 80
        dg1.Columns(13).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(14).Name = "AccountID" : dg1.Columns(14).Width = 80
        dg1.Columns(15).Name = "ItmID" : dg1.Columns(15).Width = 80

    End Sub
    Sub calc()
        txtTotNug.Text = Format(0, "0.00") : txtTotweight.Text = Format(0, "0.00")
        txtTotBasic.Text = Format(0, "0.00") : txtTotCharge.Text = Format(0, "0.00")
        TxtGrandTotal.Text = Format(0, "0.00") : txtTotalRoff.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            txtTotNug.Text = Format(Val(txtTotNug.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotweight.Text = Format(Val(txtTotweight.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotBasic.Text = Format(Val(txtTotBasic.Text) + Val(dg1.Rows(i).Cells(9).Value), "0.00")
            txtTotCharge.Text = Format(Val(txtTotCharge.Text) + Val(dg1.Rows(i).Cells(10).Value), "0.00")
            txtTotalRoff.Text = Format(Val(txtTotalRoff.Text) + Val(dg1.Rows(i).Cells(11).Value), "0.00")
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(12).Value), "0.00")
        Next
        lblRecordCount.Visible = True
        lblRecordCount.Text = "Record Count : " & dg1.RowCount

    End Sub
    Public Sub retrive(Optional ByVal condtion As String = "", Optional ByVal condtion1 As String = "", Optional ByVal condtion2 As String = "")
        dg1.Rows.Clear()
        Dim dt, dt1 As New DataTable
        '   Dim NewDate As DateTime
        ' Dim duration As TimeSpan
        Dim StartTime As DateTime = DateTime.Now
        'Call the database here and execute your SQL statement
        Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & "")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        dt = clsFun.ExecDataTable("Select * FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & " LIMIT " + RowCount.ToString() + " OFFSET " + Offset.ToString())
        lblTotalRecord.Text = "Total Pages : " & TotalPages : lblTotalRecord.Visible = True
        lblPageNumber.Text = "Page No. : " & (Offset / RowCount) + 1 : lblPageNumber.Visible = True
        totNugs = Format(clsFun.ExecScalarDec("Select ifnull(Sum(Nug),0) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lbltotNug.Visible = True
        totWeight = Format(clsFun.ExecScalarDec("Select ifnull(Sum(Weight),0) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblTotalWeight.Visible = True
        totBasic = Format(clsFun.ExecScalarDec("Select ifnull(Sum(Amount),0) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblBasic.Visible = True
        totCharges = Format(clsFun.ExecScalarDec("Select ifnull(Sum(Charges),0) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblCharges.Visible = True
        Roundoff = Format(Val(clsFun.ExecScalarStr("Select ifnull(Sum(RoundOff),0) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & "")), "0.00") : lblRounfOff.Visible = True
        totTotal = Format(clsFun.ExecScalarDec("Select ifnull(Sum(TotalAmount),0) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & ""), "0.00") : lblTotal.Visible = True
        lbltotNug.Text = "Nug  : " & Format(Val(totNugs), "0.00") : lblTotalWeight.Text = "Weight : " & Format(Val(totWeight), "0.00")
        lblBasic.Text = "Basic : " & Format(Val(totBasic), "0.00") : lblCharges.Text = "Charges : " & Format(Val(totCharges), "0.00")
        lblTotal.Text = "Total : " & Format(Val(totTotal), "0.00") : lblRounfOff.Text = "R. Off : " & Format(Val(Roundoff), "0.00")
        'Offset += dt.Rows.Count()
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        'Threading.Thread.Sleep(1)
                        .Cells(2).ReadOnly = True : .Cells(3).ReadOnly = True
                        .Cells(4).ReadOnly = True : .Cells(5).ReadOnly = True
                        .Cells(6).ReadOnly = True : .Cells(7).ReadOnly = True
                        .Cells(8).ReadOnly = True : .Cells(9).ReadOnly = True
                        .Cells(10).ReadOnly = True : .Cells(11).ReadOnly = True
                        .Cells(12).ReadOnly = True : .Cells(13).ReadOnly = True
                        .Cells(1).Value = dt.Rows(i)("Voucherid").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(3).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Per").ToString()
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                        .Cells(12).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(13).Value = dt.Rows(i)("CrateQty").ToString()
                        .Cells(14).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(15).Value = dt.Rows(i)("ItemID").ToString()
          
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
        ' lblCharges.Text = Format(Val(totCharges), "0.00")
    End Sub


    Public Sub retriveAll(Optional ByVal condtion As String = "", Optional ByVal condtion1 As String = "", Optional ByVal condtion2 As String = "")
        dg1.Rows.Clear()
        Dim dt, dt1 As New DataTable
        ' Dim recordsCount As Integer = clsFun.ExecScalarInt("Select Count(*) FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & "")
        TotalPages = Math.Ceiling(recordsCount / RowCount)
        dt = clsFun.ExecDataTable("Select Voucherid,EntryDate,ItemName,AccountName,Nug,Weight,Rate,Per,Amount,Charges,RoundOff,TotalAmount,CrateQty,AccountID,ItemID FROM Transaction2 WHERE transtype = 'Speed Sale' and  EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & condtion & "  " & condtion1 & "  " & condtion2 & "")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        '   Threading.Thread.Sleep(1)
                        .Cells(1).Value = dt.Rows(i)("Voucherid").ToString()
                        .Cells(2).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(3).Value = dt.Rows(i)("ItemName").ToString()
                        .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("Nug").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Weight").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Per").ToString()
                        .Cells(9).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(10).Value = Format(Val(dt.Rows(i)("Charges").ToString()), "0.00")
                        .Cells(11).Value = Format(Val(dt.Rows(i)("RoundOff").ToString()), "0.00")
                        .Cells(12).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(13).Value = dt.Rows(i)("CrateQty").ToString()
                        .Cells(14).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(15).Value = dt.Rows(i)("ItemID").ToString()
                        '.Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        '.Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        '.Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        '.Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                        '.Cells(9).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        '.Cells(10).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        '.Cells(11).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        '.Cells(12).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        '  lblTotalRecordCount.Text = "Total Record : " & recordsCount : lblTotalRecordCount.Visible = True
        calc() : dg1.ClearSelection()
        ' lblCharges.Text = Format(Val(totCharges), "0.00")
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If ckAll.Checked = True Then
            retriveAll()
        Else
            Primary = "" : Secondary = "" : third = ""
            txtPrimarySearch.Text = "" : txtSecondarySearch.Text = "" : txtThirdSearch.Text = ""
            retrive() ':  BackgroundWorker1.RunWorkerAsync()
        End If

    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
            SpeedSale.MdiParent = MainScreenForm
            SpeedSale.Show() : SpeedSale.FillContros(tmpID)
            SpeedSale.BringToFront() : e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.RowCount = 0 Then Exit Sub
        Dim tmpID As String = dg1.SelectedRows(0).Cells(1).Value
        SpeedSale.MdiParent = MainScreenForm
        SpeedSale.Show() : SpeedSale.FillContros(tmpID)
        SpeedSale.BringToFront()
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
                Dim OtherAccountName As String = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(.Cells(14).Value) & "")
                Dim OtherItemName As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(.Cells(15).Value) & "")
                sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,T1,T2,T3,T4,T5,T6,T7,T8) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," &
                    "'" & .Cells("Date").Value & "','" & .Cells("Item Name").Value & "','" & .Cells("Customer").Value & "','" & Format(Val(.Cells("Nug").Value), "0.00") & "'," &
                    "'" & Format(Val(.Cells("Kg").Value), "0.00") & "'," & Format(Val(.Cells("Rate").Value), "0.00") & ",'" & .Cells("Per").Value & "'," &
                    "'" & Format(Val(.Cells("Net").Value), "0.00") & "'," & Format(Val(.Cells("Charges").Value), "0.00") & ",'" & Format(Val(.Cells("Total").Value), "0.00") & "'," & Format(Val(txtTotNug.Text), "0.00") & "," &
                    " " & Format(Val(txtTotweight.Text), "0.00") & "," & Format(Val(txtTotBasic.Text), "0.00") & "," & Format(Val(txtTotCharge.Text), "0.00") & "," & Format(Val(txtTotalRoff.Text), "0.00") & ", " &
                    "" & Format(Val(TxtGrandTotal.Text), "0.00") & ",'" & OtherAccountName & "','" & OtherItemName & "')"
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
            Dim OtherAccountName As String = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & Val(dt.Rows(i)("AccountID").ToString()) & "")
            Dim OtherItemName As String = clsFun.ExecScalarStr("Select OtherName From Items Where ID=" & Val(dt.Rows(i)("ItemID").ToString()) & "")
            sql = "insert into Printing(D1,D2, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,T1,T2,T3,T4,T5,T6,T7,T8) values('" & mskFromDate.Text & "','" & MsktoDate.Text & "'," &
                    "'" & CDate(dt.Rows(i)("EntryDate")).ToString("dd-MM-yyyy") & "','" & dt.Rows(i)("ItemName").ToString() & "'," &
                    "'" & dt.Rows(i)("AccountName").ToString() & "','" & Format(Val(dt.Rows(i)("Nug").ToString()), "0.00") & "'," &
                    "'" & Format(Val(dt.Rows(i)("Weight").ToString()), "0.00") & "','" & Format(Val(dt.Rows(i)("Rate").ToString()), "0.00") & "'," &
                    "'" & dt.Rows(i)("Per").ToString() & "','" & Format(Val(dt.Rows(i)("Amount").ToString()), "0.00") & "'," &
                    "'" & Format(Val(dt.Rows(i)("Charges").ToString()), "0.00") & "','" & Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00") & "'," &
                    "'" & Format(Val(totNugs), "0.00") & "','" & Format(Val(totWeight), "0.00") & "'," &
                    "'" & Format(Val(totBasic), "0.00") & "','" & Format(Val(totCharges), "0.00") & "','" & Format(Val(Roundoff), "0.00") & "', " &
                    "'" & Format(Val(totTotal), "0.00") & "','" & OtherAccountName & "','" & OtherItemName & "')"
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

    Private Sub txtPrimarySearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrimarySearch.KeyPress, txtSecondarySearch.KeyPress, txtThirdSearch.KeyPress
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPrimarySearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If ckAll.Checked = True Then
                If txtPrimarySearch.Text.Trim() <> "" Then
                    Primary = "And AccountName Like '" & txtPrimarySearch.Text.Trim() & "%'"
                    Offset = 0
                    retriveAll(Primary, Secondary, third)
                End If
                If txtPrimarySearch.Text.Trim() = "" Then
                    Offset = 0
                    retrive()
                End If
            ElseIf txtPrimarySearch.Text.Trim() <> "" Then
                Primary = "And AccountName Like '" & txtPrimarySearch.Text.Trim() & "%'"
                Offset = 0
                retrive(Primary, Secondary, third)
            End If
            If txtPrimarySearch.Text.Trim() = "" Then
                Offset = 0
                retrive()
            End If
        End If
        e.SuppressKeyPress = True
    End Sub

    Private Sub txtItemSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSecondarySearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If ckAll.Checked = True Then
                If txtSecondarySearch.Text.Trim() <> "" Then
                    Secondary = "And ItemName Like '" & txtSecondarySearch.Text.Trim() & "%'"
                    Offset = 0
                    retriveAll(Primary, Secondary, third)
                End If
                If txtSecondarySearch.Text.Trim() = "" Then
                    Primary = "" : Secondary = "" : third = ""
                    Offset = 0
                    retriveAll()
                End If
            Else
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
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub txtTotalSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtThirdSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If ckAll.Checked = True Then
                If txtThirdSearch.Text.Trim() <> "" Then
                    third = "And TotalAmount Like '" & txtThirdSearch.Text.Trim() & "%'"
                    Offset = 0
                    retriveAll(Primary, Secondary, third)
                    PrintRecord2(SearchText)
                End If
                If txtThirdSearch.Text.Trim() = "" Then
                    Primary = "" : Secondary = "" : third = ""
                    Offset = 0
                    retriveAll()
                    PrintRecord2(Primary)
                End If
            Else
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
            End If

            e.SuppressKeyPress = True
        End If
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
        Report_Viewer.BringToFront()
        pnlprint.Visible = False
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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PnlDeleteBills.Show()
        PnlDeleteBills.BringToFront()
        txtDelete.Clear() : txtDelete.Focus()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim checkBox As DataGridViewCheckBoxCell
        ID = String.Empty
        If txtDelete.Text <> "SURE" Then MsgBox("captcha Mis Match, Unable to Delete Bills", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        For Each row As DataGridViewRow In dg1.Rows
            checkBox = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            If checkBox.Value = True Then
                ID = ID & row.Cells(1).Value & ","
            End If
        Next
        If ID <> "" Then
            ID = ID.Remove(ID.LastIndexOf(","))
        End If
        If ID = "" Then MsgBox("Please Select atleast 1 Entry to Delete", MsgBoxStyle.Critical, "Access Denied") : txtDelete.Clear() : Exit Sub
        If MessageBox.Show("Are you Sure want to Delete Selected Entries ??", "DELETE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            UpdateLedger() : UpdateCrate()
            If clsFun.ExecNonQuery("Delete From Vouchers Where ID in (" & ID & "); " &
                                    "Delete From Transaction2 Where VoucherID in (" & ID & "); " &
                                    "Delete From CrateVoucher Where VoucherID in(" & ID & "); " &
                                    "Delete From Ledger Where VourchersID in(" & ID & ");") Then
                MsgBox("Selected Bills Deleted Successfully...", MsgBoxStyle.Information, "Sucessful")
                retrive() : PnlDeleteBills.Visible = False : mskFromDate.Focus() : txtDelete.Clear() : txtDelete.Focus()
            End If
        End If
    End Sub
    Private Sub UpdateCrate()
        ClsFunserver.ExecScalarStr("Delete from CrateVoucher where VoucherID in (" & ID & ")")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from CrateVoucher where VoucherID in (" & ID & ")")
        Dim fastQuery As String = String.Empty
        Try
            If dt.Rows.Count > 0 Then
                ServerTag = 0
                For i = 0 To dt.Rows.Count - 1
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(dt.Rows(i)("VoucherID").ToString()) & ",'" & dt.Rows(i)("SlipNo").ToString() & "', " &
                        "'" & CDate(dt.Rows(i)("EntryDate").ToString()).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(dt.Rows(i)("AccountID").ToString()) & ", " &
                        "'" & dt.Rows(i)("AccountName").ToString() & "','Crate Out','" & Val(dt.Rows(i)("CrateID").ToString()) & "','" & dt.Rows(i)("CrateName").ToString() & "', " &
                        "" & Val(dt.Rows(i)("Qty").ToString()) & ",'', '" & Val(dt.Rows(i)("Rate").ToString()) & "','" & Val(dt.Rows(i)("Amount").ToString()) & "',''," & Val(ServerTag) & ", " & Val(OrgID) & ""
                Next
            End If
            If fastQuery = String.Empty Then Exit Sub
            ClsFunserver.FastCrateLedger(fastQuery)
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub UpdateLedger()
        ClsFunserver.ExecScalarStr("Delete from Ledger where VourchersID in (" & ID & ")")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Ledger where VourchersID in (" & ID & ")")
        '  sSql = "Insert into CrateVoucher(VoucherID,SlipNo, EntryDate,TransType,AccountID,AccountName,CrateType,CrateID,CrateName, Qty,Remark,Rate,Amount,CashPaid,ServerTag,OrgID)"
        Dim fastQuery As String = String.Empty
        Try
            If dt.Rows.Count > 0 Then
                ServerTag = 0
                For i = 0 To dt.Rows.Count - 1
                    fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(dt.Rows(i)("VourchersID").ToString()) & ",'" & CDate(dt.Rows(i)("EntryDate").ToString()).ToString("yyyy-MM-dd") & "'," &
                                        "'" & Me.Text & "',  " & Val(dt.Rows(i)("AccountID").ToString()) & ",'" & dt.Rows(i)("AccountName").ToString() & "','" & Val(dt.Rows(i)("Amount").ToString()) & "','DC', " & Val(ServerTag) & "," & Val(OrgID) & "," &
                                       "'" & dt.Rows(i)("Remark").ToString() & "','" & dt.Rows(i)("Narration").ToString() & "','" & dt.Rows(i)("RemarkHindi").ToString() & "'"
                Next
            End If
            If fastQuery = String.Empty Then Exit Sub
            ClsFunserver.FastLedger(fastQuery)
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub txtDelete_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDelete.KeyDown
        If e.KeyCode = Keys.Enter Then btnDelete.Focus()
    End Sub

    Private Sub MsktoDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MsktoDate.MaskInputRejected

    End Sub

    Private Sub txtPrimarySearch_TextChanged(sender As Object, e As EventArgs) Handles txtPrimarySearch.TextChanged

    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub txtThirdSearch_TextChanged(sender As Object, e As EventArgs) Handles txtThirdSearch.TextChanged

    End Sub
End Class