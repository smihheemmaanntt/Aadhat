Public Class Ugrahi_Report
    Dim Date1 As String
    Dim Coldate As String
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub Ugrahi_Report_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub Ugrahi_Report_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) From Transaction2")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        CbDays.SelectedIndex = 2
        rowColums(0, CDate(mskFromDate.Text))
    End Sub
    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, btnShow.KeyDown, CbDays.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub


    Private Sub retrive2()
        pnlWait.Visible = True
        Label1.Visible = True
        dg1.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        Dim bal As Decimal = 0
        Dim tmpdt As Date = CDate(mskFromDate.Text).ToString("yyyy-MM-dd")
        Date1 = CDate(tmpdt.AddDays(-Val(CbDays.Text))).ToString("yyyy-MM-dd")
        rowColums(Val(CbDays.Text), Date1)
        Dim lastval As Integer = 0
        '''''''''''''''''''''''''''''''''''''''''''''''''
        ' ssql = "Select  AccountID as ID From Transaction2 Where EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' group by AccountID order by accountname "
        ssql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " &
    "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(Date1).ToString("yyyy-MM-dd") & "')" &
    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(Date1).ToString("yyyy-MM-dd") & "')) " &
    " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(Date1).ToString("yyyy-MM-dd") & "')" &
    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(Date1).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where GroupID in(16,32)  Order by Upper(Area),upper(AccountName)  ;"
        'ssql = "Select  ac.id as id ,ac.id as id from Account_acgrp ac " & _
        '            "inner join Transaction2 t2 on ac.id=t2.accountid left join vouchers v on v.id =t2.voucherid where (ac.groupid =16 or ac.undergroupid=16)  " & _
        '            "and  t2.EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' group by Ac.AccountName order by ac.accountname "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                pb1.Minimum = 0
                pb1.Maximum = dt.Rows.Count - 1
                pb1.Value = i
                Application.DoEvents()

                If bal > 0 Then
                    opbal = Format(Val(dt.Rows(i)("Restbal").ToString()), "0.00") & " Dr"
                Else
                    opbal = Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00") & " Cr"
                End If

                ssql = "Select strftime('%d-%m', EntryDate) as EntryDate1,AccountID as ID, AccountName, sum(Amount) as Sales From Ledger Where EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and id =" & dt.Rows(i)("Id") & " and DC='D' Group by Entrydate1 "
                Dim dt1 As New DataTable
                dt1 = clsFun.ExecDataTable(ssql)
                If dt1.Rows.Count > 0 Then
                    dg1.Rows.Add()
                    lastval = lastval + 1
                    For k As Integer = 0 To dt1.Rows.Count - 1
                        dg1.ClearSelection()
                        With dg1.Rows(lastval)
                            Application.DoEvents()
                            Dim totalsales As Decimal = 0.0
                            .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight

                            .Cells(0).Value = dt1.Rows(k)("Id").ToString()
                            .Cells(1).Value = dt1.Rows(k)("AccountName").ToString()
                            .Cells(2).Value = opbal '& " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts  WHERE id = " & dt.Rows(i)("Id").ToString() & "")
                            If Val(CbDays.Text) = 0 Then
                                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                .Cells(3).Value = dt1.Rows(k)("Sales").ToString()
                                totalsales = .Cells(3).Value
                                If (opbal.ToString.Split(" ")(1)) = "Dr" Then
                                    totalsales = totalsales + Val(opbal.ToString.Split(" ")(0))
                                Else
                                    totalsales = totalsales - Val(opbal.ToString.Split(" ")(0))
                                End If
                                totrecpt = clsFun.ExecScalarStr("Select  sum(Amount) FROM Ledger where  DC='C' and Accountid=" & dt.Rows(i)("Id") & " and EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "';")
                                .Cells(4).Value = totrecpt.ToString() '' clsFun.ExecScalarStr("Select  Amount FROM Ledger where  DC='C' and Accountid=" & dt.Rows(i)("Id") & " and EntryDate='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "';")
                                '.Cells(5).Value = .Cells(3).Value

                                totalsales = totalsales - Val(totrecpt)
                                .Cells(5).Value = IIf(totalsales > 0, totalsales & " Dr", Math.Abs(totalsales) & " Cr")
                            Else
                                Dim colno As Integer = 3
                                For j As Integer = 0 To Val(CbDays.Text)
                                    'Dim dv As New DataView(dt1)
                                    .Cells(colno).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                    'dv.RowFilter = "entrydate='" & Format(CDate(dt1.Rows(j)("entrydate").ToString()), "dd/MM") & "'"
                                    Dim result As DataRow() = dt1.Select("entrydate1='" & dg1.Columns(colno).Name & "'")
                                    '' Dim dr As New DataRow(dt1, "entrydate1='" & dg1.Columns(colno).Name & "'", "Sales", DataViewRowState.CurrentRows)
                                    '' Dim dt2 As New DataTable()

                                    If result.Length = 0 Then
                                        .Cells(colno).Value = ""
                                    Else
                                        .Cells(colno).Value = Format(Val(result(0)("Sales").ToString()), "0.00")
                                    End If
                                    'If j < dt1.Rows.Count Then
                                    '    If dg1.Columns(colno).Name = Format(CDate(dt1.Rows(j)("entrydate").ToString()), "dd/MM") Then
                                    '        .Cells(colno).Value = dt1.Rows(j)("Sales").ToString()
                                    '    Else
                                    '        .Cells(colno).Value = "0"
                                    '    End If
                                    'Else
                                    '    .Cells(colno).Value = "0"
                                    'End If
                                    ''  .Cells(colno).Value = dt1.Rows(j)("Sales").ToString()
                                    totalsales = Val(.Cells(colno).Value) + totalsales
                                    colno = colno + 1
                                Next

                                '  colno = colno + optdays
                                ''colno = colno + 14
                                If bal >= 0 Then
                                    totalsales = totalsales + bal
                                Else
                                    totalsales = totalsales - bal
                                End If
                                '  Dim totrecpt As String = ""
                                totrecpt = clsFun.ExecScalarStr("Select  sum(Amount) FROM Ledger where  DC='C' and Accountid=" & dt.Rows(i)("Id") & " and EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "';")
                                .Cells(colno).Value = IIf(totrecpt = "", "", Format(Val(totrecpt.ToString()), "0.00")) '' clsFun.ExecScalarStr("Select  Amount FROM Ledger where  DC='C' and Accountid=" & dt.Rows(i)("Id") & " and EntryDate='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "';")
                                .Cells(colno).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                colno = colno + 1
                                totalsales = totalsales - Val(totrecpt)
                                .Cells(colno).Value = IIf(totalsales > 0, Format(totalsales, "0.00") & " Dr", Format(Math.Abs(totalsales), "0.00") & " Cr")
                                .Cells(colno).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                'colno = colno + 1
                                '.Cells(colno).Value = 0.0
                            End If

                        End With
                    Next
                End If


            Next i
        End If
        dg1.ClearSelection()
        Label1.Visible = False
        pnlWait.Visible = False
    End Sub


    
    Private Sub retrive()
        pnlWait.Visible = True : Label1.Visible = True
        dg1.Rows.Clear() : Dim dt As New DataTable
        Dim i As Integer : Dim count As Integer = 0
        Dim tmpdt As Date = CDate(mskFromDate.Text).ToString("yyyy-MM-dd")
        Date1 = CDate(tmpdt.AddDays(-Val(CbDays.Text) + 1)).ToString("yyyy-MM-dd")
        rowColums(Val(CbDays.Text), Date1)
        Dim RowSales As Decimal = 0 : Dim RowReceipt As Decimal = 0
        Dim RowTotal As Decimal = 0 : Dim bal As Decimal = 0
        Dim lastrow As Integer = 0
        '''''''''''''''''''''''''''''''''''''''''''''''''
        ssql = "Select  AccountID as ID From Transaction2 Where EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and AccountID<>7 group by AccountID order by accountname "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                pb1.Minimum = 0
                pb1.Maximum = dt.Rows.Count - 1
                pb1.Value = i
                opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(Date1).ToString("yyyy-MM-dd") & "')" & _
                                              "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(Date1).ToString("yyyy-MM-dd") & "')) " & _
                                              " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(Date1).ToString("yyyy-MM-dd") & "')" & _
                                              " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(Date1).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(dt.Rows(i)("Id").ToString()) & " Order by upper(AccountName);"))
                bal = opbal
                If opbal > 0 Then
                    opbal = Format(Val(opbal), "0.00") & " Dr"
                Else
                    opbal = Format(Math.Abs(Val(opbal)), "0.00") & " Cr"
                End If
                ssql = "Select strftime('%d-%m', EntryDate) as EntryDate1,AccountID as ID, AccountName, sum(Amount) as Sales From Ledger Where EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and id =" & dt.Rows(i)("Id") & " and DC='D' and AccountID<>7 Group by Entrydate1 "
                Dim dt1 As New DataTable
                dt1 = clsFun.ExecDataTable(ssql)
                If dt1.Rows.Count > 0 Then
                    dg1.Rows.Add()
                    For k As Integer = 0 To dt1.Rows.Count - 1
                        dg1.ClearSelection()
                        With dg1.Rows(lastrow)
                            Dim totalsales As Decimal = 0.0
                            .Cells(0).Value = dt1.Rows(k)("Id").ToString()
                            .Cells(1).Value = dt1.Rows(k)("AccountName").ToString()
                            .Cells(2).Value = opbal
                            If Val(CbDays.Text) = 1 Then

                                .Cells(3).Value = dt1.Rows(k)("Sales").ToString()
                                totalsales = .Cells(3).Value
                                If (opbal.ToString.Split(" ")(1)) = "Dr" Then
                                    totalsales = totalsales + Val(opbal.ToString.Split(" ")(0))
                                Else
                                    totalsales = totalsales - Val(opbal.ToString.Split(" ")(0))
                                End If
                                totrecpt = clsFun.ExecScalarStr("Select  sum(Amount) FROM Ledger where  DC='C' and Accountid=" & dt.Rows(i)("Id") & " and EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "';")
                                .Cells(4).Value = totrecpt.ToString()
                                totalsales = totalsales - Val(totrecpt)
                                .Cells(5).Value = IIf(totalsales > 0, totalsales & " Dr", Math.Abs(totalsales) & " Cr")
                            Else
                                Dim colno As Integer = 3
                                For j As Integer = 0 To Val(CbDays.Text) - 1
                                    Dim result As DataRow() = dt1.Select("entrydate1='" & dg1.Columns(colno).Name & "'")
                                    If result.Length = 0 Then
                                        .Cells(colno).Value = ""
                                    Else
                                        .Cells(colno).Value = Format(Val(result(0)("Sales").ToString()), "0.00")
                                    End If
                                    totalsales = Val(.Cells(colno).Value) + Val(totalsales)
                                    colno = colno + 1
                                Next
                                If bal >= 0 Then
                                    totalsales = totalsales + bal
                                Else
                                    totalsales = totalsales - bal
                                End If
                                '  Dim totrecpt As String = ""
                                totrecpt = clsFun.ExecScalarStr("Select  sum(Amount) FROM Ledger where  DC='C' and Accountid=" & dt.Rows(i)("Id") & " and EntryDate between '" & CDate(Date1).ToString("yyyy-MM-dd") & "' and '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "';")
                                .Cells(colno).Value = IIf(totrecpt = "", "", Format(Val(totrecpt.ToString()), "0.00")) '' clsFun.ExecScalarStr("Select  Amount FROM Ledger where  DC='C' and Accountid=" & dt.Rows(i)("Id") & " and EntryDate='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "';")
                                colno = colno + 1
                                totalsales = totalsales - Val(totrecpt)
                                .Cells(colno).Value = IIf(totalsales > 0, Format(totalsales, "0.00") & " Dr", Format(Math.Abs(totalsales), "0.00") & " Cr")
                            End If
                        End With
                    Next
                    lastrow = lastrow + 1
                End If
            Next i
        End If
        dg1.ClearSelection()
        Label1.Visible = False : pnlWait.Visible = False
    End Sub


    Private Sub rowColums(ByVal optdays As Integer, ByVal dt As Date)
        dg1.Columns.Clear()
        dg1.ColumnCount = 6
        Coldate = ""
        If optdays = 1 Then
            dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
            dg1.Columns(1).Name = "Account name" : dg1.Columns(1).Width = 250
            dg1.Columns(2).Name = "Op. Bal." : dg1.Columns(2).Width = 100
            dg1.Columns(3).Name = Format(CDate(mskFromDate.Text), "dd/MM") : dg1.Columns(3).Width = 100
            dg1.Columns(4).Name = "Receipts" : dg1.Columns(4).Width = 100
            dg1.Columns(5).Name = "Total" : dg1.Columns(5).Width = 100
            Coldate = Format(CDate(mskFromDate.Text), "dd/MM") & ","
            '            dg1.Columns(5).Name = "Closing Bal" : dg1.Columns(5).Width = 100
        Else
            Dim colno As Integer = 3
            dg1.ColumnCount = dg1.ColumnCount + Val(optdays - 1)
            dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
            dg1.Columns(1).Name = "Account name" : dg1.Columns(1).Width = 250
            dg1.Columns(2).Name = "Op. Bal." : dg1.Columns(2).Width = 100
            For i = 0 To optdays - 1
                Dim dt1 As Date = DateAdd(DateInterval.Day, i, dt)
                dg1.Columns(colno).Name = Format(CDate(dt1), "dd/MM") : dg1.Columns(colno).Width = 100
                Coldate = Coldate & Format(CDate(dt1), "dd/MM") & ","
                colno = colno + 1
            Next

            '  colno = colno + optdays
            ''colno = colno + 1
            dg1.Columns(colno).Name = "Receipts" : dg1.Columns(colno).Width = 100
            colno = colno + 1
            dg1.Columns(colno).Name = "Total" : dg1.Columns(colno).Width = 100

            'colno = colno + 1
            'dg1.Columns(colno).Name = "Closing " : dg1.Columns(colno).Width = 100
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
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
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        ButtonControl() : retrive() : ButtonControl()
    End Sub

    Private Sub CbDays_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbDays.SelectedIndexChanged
        '      rowColums(0, CDate(mskFromDate.Text))
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        ButtonControl()
        pnlWait.Visible = True
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Product.xml") Then
            My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Product.xml")
        End If

        Dim ds As New DataSet
        Try
            ' Add Table
            ds.Tables.Add("Invoices")

            ' Add Columns
            Dim col As DataColumn
            For Each dgvCol As DataGridViewColumn In dg1.Columns
                col = New DataColumn(dgvCol.Index)
                '     If ds.Tables("Invoices").Columns.Contains() Then
                ds.Tables("Invoices").Columns.Add(col)
                'End If
            Next

            'Add Rows from the datagridview
            Dim row As DataRow
            Dim colcount As Integer = dg1.Columns.Count - 1
            For i As Integer = 0 To dg1.Rows.Count - 1
                pb1.Minimum = 0
                pb1.Maximum = dg1.Columns.Count - 1
                row = ds.Tables("Invoices").Rows.Add
                For Each column As DataGridViewColumn In dg1.Columns
                    Application.DoEvents()
                    pb1.Value = IIf(Val(column.Index) < 0, 0, Val(column.Index))
                    'Dim OtherName As String = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & dg1.Rows.Item(i).Cells(0).Value & "")
                    'If dg1.Rows.Item(i).Cells(column.Index).ColumnIndex = 0 Then
                    '    row.Item(column.Index) = dg1.Rows.Item(i).Cells(column.Index + 1).Value
                    'ElseIf dg1.Rows.Item(i).Cells(column.Index).ColumnIndex = 1 Then
                    '    row.Item(column.Index) = OtherName
                    'Else
                    row.Item(column.Index) = dg1.Rows.Item(i).Cells(column.Index).Value
                    '         End If


                Next
            Next

            ds.Tables(0).TableName = "product"
            ds.WriteXml("Product.xml")
            '  clsFun.xmldata()
            If CbDays.SelectedIndex = 0 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport0.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            ElseIf CbDays.SelectedIndex = 1 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport1.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            ElseIf CbDays.SelectedIndex = 2 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport2.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            ElseIf CbDays.SelectedIndex = 3 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport3.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            End If

            Ugrahi_Viewer.MdiParent = MainScreenForm
            Ugrahi_Viewer.Show()
            If Not Registers_Viewer Is Nothing Then
                Ugrahi_Viewer.BringToFront()
            End If
            '  MsgBox("Done")
        Catch ex As Exception

            MsgBox("CRITICAL ERROR : Exception caught while converting dataGridView to DataSet (dgvtods).. " & Chr(10) & ex.Message)

        End Try
        pnlWait.Visible = False
        ButtonControl()
    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim bm As New Bitmap(Me.dg1.Width, Me.dg1.Height)
        dg1.DrawToBitmap(bm, New Rectangle(0, 0, Me.dg1.Width, Me.dg1.Height))
        e.Graphics.DrawImage(bm, 0, 0)
    End Sub

    Private Sub PrintPreviewDialog1_Load(sender As Object, e As EventArgs) Handles PrintPreviewDialog1.Load

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnPrint2.Click
        ButtonControl()
        pnlWait.Visible = True
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Product.xml") Then
            My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Product.xml")
        End If
        'Dim p As New System.Diagnostics.Process
        'p.StartInfo.FileName = Application.StartupPath & "\Product.xml"


        Dim ds As New DataSet
        Try
            ' Add Table
            ds.Tables.Add("Invoices")

            ' Add Columns
            Dim col As DataColumn
            For Each dgvCol As DataGridViewColumn In dg1.Columns
                col = New DataColumn(dgvCol.Index)
                '     If ds.Tables("Invoices").Columns.Contains() Then
                ds.Tables("Invoices").Columns.Add(col)
                'End If
            Next

            'Add Rows from the datagridview
            Dim row As DataRow
            Dim colcount As Integer = dg1.Columns.Count - 1
            For i As Integer = 0 To dg1.Rows.Count - 1
                pb1.Minimum = 0
                pb1.Maximum = dg1.Columns.Count - 1
                row = ds.Tables("Invoices").Rows.Add
                For Each column As DataGridViewColumn In dg1.Columns
                    Application.DoEvents()
                    pb1.Value = IIf(Val(column.Index) < 0, 0, Val(column.Index))
                    Dim OtherName As String = clsFun.ExecScalarStr("Select OtherName From Accounts Where ID=" & dg1.Rows.Item(i).Cells(0).Value & "")
                    If dg1.Rows.Item(i).Cells(column.Index).ColumnIndex = 1 Then
                        row.Item(column.Index) = OtherName
                    Else
                        row.Item(column.Index) = dg1.Rows.Item(i).Cells(column.Index).Value
                    End If


                Next
            Next

            ds.Tables(0).TableName = "product"
            ds.WriteXml("Product.xml")
            '  clsFun.xmldata()
            If CbDays.SelectedIndex = 0 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport0.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            ElseIf CbDays.SelectedIndex = 1 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport1.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            ElseIf CbDays.SelectedIndex = 2 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport2.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            ElseIf CbDays.SelectedIndex = 3 Then
                Ugrahi_Viewer.printReport("\Reports\UgrahiReport3.rpt", mskFromDate.Text, mskFromDate.Text, Date1, Coldate)
            End If

            Ugrahi_Viewer.MdiParent = MainScreenForm
            Ugrahi_Viewer.Show()
            If Not Registers_Viewer Is Nothing Then
                Ugrahi_Viewer.BringToFront()
            End If
            '  MsgBox("Done")
        Catch ex As Exception

            MsgBox("CRITICAL ERROR : Exception caught while converting dataGridView to DataSet (dgvtods).. " & Chr(10) & ex.Message)

        End Try
        pnlWait.Visible = False
        ButtonControl()
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub

    Private Sub dtp1_Leave(sender As Object, e As EventArgs) Handles dtp1.Leave
        mskFromDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskFromDate.Enabled = False Then Exit Sub
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub
End Class