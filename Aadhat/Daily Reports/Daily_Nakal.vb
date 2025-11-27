Public Class Daily_Nakal

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub CustomerWiseSaleSummary_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub CustomerWiseSaleSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select Max(EntryDate) from transaction2 where transtype='" & Me.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(entrydate) from transaction2 where transtype='" & Me.Text & "'")
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
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus, mskFromDate.Click
        mskFromDate.SelectionStart = 0 : mskFromDate.SelectionLength = Len(mskFromDate.Text)
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectionStart = 0 : MsktoDate.SelectionLength = Len(MsktoDate.Text)
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Expenses/Account Name" : dg1.Columns(1).Width = 400 : dg1.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(2).Name = "Total" : dg1.Columns(2).Width = 180 : dg1.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 400 : dg1.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft
        dg1.Columns(4).Name = "Total" : dg1.Columns(4).Width = 180 : dg1.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable : dg1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
    End Sub

    Private Sub retrive()
        dg1.Rows.Clear()

        Try
            ' ============================
            ' Get Charges Data (Transaction2 + Vouchers)
            ' ============================
            Dim dtCharges As New DataTable
            Dim sqlCharges As String = _
                "SELECT V.SallerName AS AccountName," & _
                " SUM(V.TotalAmount) AS ChargesAmt " & _
                " FROM Vouchers V WHERE V.ID IN (SELECT DISTINCT T2.VoucherID FROM Transaction2 T2)" & _
                " AND V.EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' " & _
                " AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & _
                " GROUP BY V.SallerName, V.SallerID " & _
                " ORDER BY V.SallerName"
            dtCharges = clsFun.ExecDataTable(sqlCharges)

            ' ============================
            ' Get Expense Data
            ' ============================
            Dim dtExp As New DataTable
            dtExp = clsFun.ExecDataTable("SELECT '' AS Commission, '' AS MandiTax, " & _
                                         "SUM(RdfAmt) AS RDF, '' AS Bardana, " & _
                                         "SUM(LabourAmt) AS Labour, (SUM(LabourAmt)+sum(RdfAmt)) AS TotalExp " & _
                                         "FROM Transaction2 WHERE EntryDate BETWEEN '" & _
                                         CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & _
                                         CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary)

            ' ============================
            ' Get Account Totals
            ' ============================
            Dim dtAcc As New DataTable
            dtAcc = clsFun.ExecDataTable("SELECT AccountName, SUM(TotalAmount) AS TotalAmount " & _
                                         "FROM Transaction2 WHERE EntryDate BETWEEN '" & _
                                         CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & _
                                         CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary & _
                                         " GROUP BY AccountName ORDER BY AccountName")

            ' ============================
            ' Fill Left (Expenses) Section
            ' ============================
            Dim totalChargesAmt As Double = 0
            Dim totalExpAmt As Double = 0
            Dim totalAccountAmt As Double = 0

            ' Add Charges (Suppliers)
            For i As Integer = 0 To dtCharges.Rows.Count - 1
                dg1.Rows.Add()
                Dim accName As String = If(IsDBNull(dtCharges.Rows(i)("AccountName")), "", dtCharges.Rows(i)("AccountName").ToString())
                Dim amt As Double = Val(If(IsDBNull(dtCharges.Rows(i)("ChargesAmt")), 0, dtCharges.Rows(i)("ChargesAmt")))
                dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = accName
                dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(amt, "0.00")
                totalChargesAmt += amt
            Next

            ' Separator
            dg1.Rows.Add()
            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "========================"
            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "===================="

            ' Suppliers Total
            dg1.Rows.Add()
            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Suppliers Total"
            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(totalChargesAmt, "0.00")

            ' Separator
            dg1.Rows.Add()
            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "========================"
            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "===================="

            ' Expenses Section
            dg1.Rows.Add()
            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Customers Expenses ----------------"
            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "----------------"

            If dtExp.Rows.Count > 0 Then
                Dim expRow = dtExp.Rows(0)
                'dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Commission" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(expRow("Commission")), "0.00")
                'dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Mandi Tax" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(expRow("MandiTax")), "0.00")
                dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "RDF" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(expRow("RDF")), "0.00")
                'dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Bardana" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(expRow("Bardana")), "0.00")
                dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Labour" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(expRow("Labour")), "0.00")
                totalExpAmt = Val(If(IsDBNull(expRow("TotalExp")), 0, expRow("TotalExp")))
                ' Total Expenses
                dg1.Rows.Add()
                dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Total Expenses"
                dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(totalExpAmt, "0.00")
            End If
            ' ============================
            ' Right Side (Accounts)
            ' ============================
            For i As Integer = 0 To dtAcc.Rows.Count - 1
                If dg1.Rows.Count <= i Then dg1.Rows.Add()
                dg1.Rows(i).Cells(3).Value = dtAcc.Rows(i)("AccountName").ToString()
                dg1.Rows(i).Cells(4).Value = Format(Val(If(IsDBNull(dtAcc.Rows(i)("TotalAmount")), 0, dtAcc.Rows(i)("TotalAmount"))), "0.00")
                totalAccountAmt += Val(If(IsDBNull(dtAcc.Rows(i)("TotalAmount")), 0, dtAcc.Rows(i)("TotalAmount")))
            Next
            ' Total of Accounts
            dg1.Rows.Add()
            dg1.Rows(dg1.Rows.Count - 1).Cells(3).Value = "Total of Accounts"
            dg1.Rows(dg1.Rows.Count - 1).Cells(4).Value = Format(totalAccountAmt, "0.00")
            dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)
            dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightYellow
            ' ============================
            ' Calculate Difference
            ' ============================
            Dim totalLeft As Double = totalChargesAmt + totalExpAmt
            Dim diff As Double = totalAccountAmt - totalLeft
            Dim finalTotal As Double = Math.Max(totalAccountAmt, totalLeft)
            dg1.Rows.Add()
            Dim r As Integer = dg1.Rows.Count - 1
            dg1.Rows(r).DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)

            If diff > 0 Then
                dg1.Rows(r).Cells(1).Value = "Difference"
                dg1.Rows(r).Cells(2).Value = Format(diff, "0.00")
                dg1.Rows(r).DefaultCellStyle.BackColor = Color.LightGreen
            ElseIf diff < 0 Then
                dg1.Rows(r).Cells(3).Value = "Difference"
                dg1.Rows(r).Cells(4).Value = Format(Math.Abs(diff), "0.00")
                dg1.Rows(r).DefaultCellStyle.BackColor = Color.LightCoral
                'Else
                '    dg1.Rows(r).Cells(1).Value = "Difference"
                '    dg1.Rows(r).Cells(2).Value = "0.00"
                '    dg1.Rows(r).DefaultCellStyle.BackColor = Color.LightGray
            End If

            ' ============================
            ' Grand Total (Both Sides Equal)
            ' ============================
            dg1.Rows.Add()
            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Grand Total"
            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(finalTotal, "0.00")
            dg1.Rows(dg1.Rows.Count - 1).Cells(3).Value = "Grand Total"
            dg1.Rows(dg1.Rows.Count - 1).Cells(4).Value = Format(finalTotal, "0.00")

            dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)
            dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightCyan

            dg1.ClearSelection()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
    End Sub


    'Private Sub retrive()
    '    dg1.Rows.Clear()
    '    ' Get Expense Data
    '    Dim dtExp As New DataTable
    '    dtExp = clsFun.ExecDataTable("Select sum(CommAmt) as Commission, sum(MAmt) as MandiTax, sum(RdfAmt) as RDF, sum(TareAmt) as Bardana, sum(LabourAmt) as Labour, sum(Charges) as Charges from Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary)
    '    ' Get Account Data
    '    Dim dtAcc As New DataTable
    '    dtAcc = clsFun.ExecDataTable("Select AccountName, sum(TotalAmount) as TotalAmount from Transaction2 Where EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary & " Group By AccountName order by AccountName")

    '    Try
    '        Dim maxRows As Integer = Math.Max(dtExp.Rows.Count, dtAcc.Rows.Count)
    '        Dim totalAccountAmt As Double = 0 ' Account column total store karne ke liye

    '        For i As Integer = 0 To maxRows - 1
    '            dg1.Rows.Add()

    '            ' Add Expense Data
    '            If i = 0 Then dg1.Rows(i).Cells(1).Value = "Customers Expenses ----------------" : dg1.Rows(i).Cells(2).Value = "----------------"
    '            If dtExp.Rows.Count > 0 Then
    '                Select Case i
    '                    Case 1 : dg1.Rows(i).Cells(1).Value = "Commission" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Commission").ToString()), "0.00")
    '                    Case 2 : dg1.Rows(i).Cells(1).Value = "Mandi Tax" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("MandiTax").ToString()), "0.00")
    '                    Case 3 : dg1.Rows(i).Cells(1).Value = "RDF" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("RDF").ToString()), "0.00")
    '                    Case 4 : dg1.Rows(i).Cells(1).Value = "Bardana" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Bardana").ToString()), "0.00")
    '                    Case 5 : dg1.Rows(i).Cells(1).Value = "Labour" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Labour").ToString()), "0.00")
    '                    Case 6 : dg1.Rows(i).Cells(1).Value = "========================" : dg1.Rows(i).Cells(2).Value = "===================="
    '                    Case 7 : dg1.Rows(i).Cells(1).Value = "Total Expenses" : dg1.Rows(i).Cells(2).Value = Format(Val(dtExp.Rows(0)("Charges").ToString()), "0.00")
    '                    Case 8 : dg1.Rows(i).Cells(1).Value = "========================" : dg1.Rows(i).Cells(2).Value = "===================="
    '                End Select
    '            End If

    '            ' Add Account Data
    '            If dtAcc.Rows.Count > i Then
    '                dg1.Rows(i).Cells(3).Value = dtAcc.Rows(i)("AccountName").ToString()
    '                dg1.Rows(i).Cells(4).Value = Format(Val(dtAcc.Rows(i)("TotalAmount").ToString()), "0.00")

    '                ' Total calculation
    '                totalAccountAmt += Val(dtAcc.Rows(i)("TotalAmount").ToString())
    '            End If
    '        Next

    '        ' Add empty separator row
    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(3).Value = "---------------------------"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(4).Value = "--------------------"

    '        ' Add Total row for column 4
    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(3).Value = "Total of Accounts"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(4).Value = Format(totalAccountAmt, "0.00")

    '        ' Bold and highlight total row (optional)
    '        Dim totalRowIndex As Integer = dg1.Rows.Count - 1
    '        dg1.Rows(totalRowIndex).DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)
    '        dg1.Rows(totalRowIndex).DefaultCellStyle.BackColor = Color.LightYellow

    '        ' Clear selection
    '        dg1.ClearSelection()

    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
    '    End Try
    'End Sub

    'Private Sub retrive()
    '    dg1.Rows.Clear()

    '    Try
    '        ' ============================
    '        ' Get Charges Data (Transaction1 + Vouchers)
    '        ' ============================
    '        Dim dtCharges As New DataTable
    '        'Dim sqlCharges As String = _
    '        '    "SELECT SallerName as AccountName, SUM(SallerAmt) AS ChargesAmt " & _
    '        '                             "FROM Transaction2  INNER JOIN Vouchers V ON V.ID = T2.VoucherID WHERE EntryDate BETWEEN '" & _
    '        '                             CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & _
    '        '                             CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary & _
    '        '                             " GROUP BY SallerID ORDER BY SallerName"
    '        Dim sqlCharges As String = _
    '            "SELECT V.SallerName AS AccountName, SUM(T2.SallerAmt) AS ChargesAmt " & _
    '            "FROM Transaction2 T2 " & _
    '            "INNER JOIN Vouchers V ON V.ID = T2.VoucherID " & _
    '            "WHERE V.EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' " & _
    '            "AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & _
    '            Primary & " " & _
    '            "GROUP BY V.SallerName, V.SallerID " & _
    '            "ORDER BY V.SallerName"
    '        dtCharges = clsFun.ExecDataTable(sqlCharges)

    '        ' ============================
    '        ' Get Expense Data (Transaction2)
    '        ' ============================
    '        Dim dtExp As New DataTable
    '        dtExp = clsFun.ExecDataTable("SELECT SUM(CommAmt) AS Commission, SUM(MAmt) AS MandiTax, " & _
    '                                     "SUM(RdfAmt) AS RDF, SUM(TareAmt) AS Bardana, " & _
    '                                     "SUM(LabourAmt) AS Labour, SUM(Charges) AS TotalExp " & _
    '                                     "FROM Transaction2 WHERE EntryDate BETWEEN '" & _
    '                                     CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & _
    '                                     CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary)

    '        ' ============================
    '        ' Get Account Totals (Transaction2)
    '        ' ============================
    '        Dim dtAcc As New DataTable
    '        dtAcc = clsFun.ExecDataTable("SELECT AccountName, SUM(TotalAmount) AS TotalAmount " & _
    '                                     "FROM Transaction2 WHERE EntryDate BETWEEN '" & _
    '                                     CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & _
    '                                     CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & Primary & _
    '                                     " GROUP BY AccountName ORDER BY AccountName")

    '        ' ============================
    '        ' START FILLING GRID
    '        ' ============================

    '        Dim totalAccountAmt As Double = 0
    '        Dim totalChargesAmt As Double = 0

    '        '' Row 1: Title Row for Charges
    '        'dg1.Rows.Add()
    '        'dg1.Rows(0).Cells(1).Value = "Charges Details ----------------"
    '        'dg1.Rows(0).Cells(2).Value = "----------------"

    '        ' Add Charges (from Transaction1 + Vouchers)
    '        Dim i As Integer
    '        For i = 0 To dtCharges.Rows.Count - 1
    '            dg1.Rows.Add()
    '            Dim accName As String = ""
    '            Dim amt As Double = 0

    '            If Not IsDBNull(dtCharges.Rows(i)("AccountName")) Then
    '                accName = dtCharges.Rows(i)("AccountName").ToString()
    '            End If
    '            If Not IsDBNull(dtCharges.Rows(i)("ChargesAmt")) Then
    '                amt = Val(dtCharges.Rows(i)("ChargesAmt"))
    '            End If

    '            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = accName
    '            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(amt, "0.00")
    '            totalChargesAmt = totalChargesAmt + amt
    '        Next

    '        ' Separator
    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "========================"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "===================="

    '        ' Total Charges
    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Suppliers Total"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(totalChargesAmt, "0.00")

    '        ' Separator
    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "========================"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "==============="


    '        ' ============================
    '        ' Add Expenses Section (Same as your original format)
    '        ' ============================
    '        Dim rowStart As Integer = dg1.Rows.Count

    '        If rowStart = 0 Then dg1.Rows.Add()

    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Customers Expenses ----------------"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "----------------"

    '        If dtExp.Rows.Count > 0 Then
    '            dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Commission" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(If(IsDBNull(dtExp.Rows(0)("Commission")), 0, dtExp.Rows(0)("Commission"))), "0.00")
    '            dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Mandi Tax" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(If(IsDBNull(dtExp.Rows(0)("MandiTax")), 0, dtExp.Rows(0)("MandiTax"))), "0.00")
    '            dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "RDF" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(If(IsDBNull(dtExp.Rows(0)("RDF")), 0, dtExp.Rows(0)("RDF"))), "0.00")
    '            dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Bardana" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(If(IsDBNull(dtExp.Rows(0)("Bardana")), 0, dtExp.Rows(0)("Bardana"))), "0.00")
    '            dg1.Rows.Add() : dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Labour" : dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(If(IsDBNull(dtExp.Rows(0)("Labour")), 0, dtExp.Rows(0)("Labour"))), "0.00")

    '            dg1.Rows.Add()
    '            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "========================"
    '            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "==============="

    '            dg1.Rows.Add()
    '            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Total Expenses"
    '            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(Val(If(IsDBNull(dtExp.Rows(0)("TotalExp")), 0, dtExp.Rows(0)("TotalExp"))), "0.00")

    '            dg1.Rows.Add()
    '            dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "========================"
    '            dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = "===================="
    '        End If

    '        ' ============================
    '        ' Add Account Data (Right Side)
    '        ' ============================
    '        Dim maxRows As Integer = Math.Max(dtExp.Rows.Count, dtAcc.Rows.Count)
    '        totalAccountAmt = 0

    '        Dim startRowIndex As Integer = 0
    '        For startRowIndex = 0 To dtAcc.Rows.Count - 1
    '            If dg1.Rows.Count <= startRowIndex Then dg1.Rows.Add()
    '            dg1.Rows(startRowIndex).Cells(3).Value = dtAcc.Rows(startRowIndex)("AccountName").ToString()
    '            dg1.Rows(startRowIndex).Cells(4).Value = Format(Val(If(IsDBNull(dtAcc.Rows(startRowIndex)("TotalAmount")), 0, dtAcc.Rows(startRowIndex)("TotalAmount"))), "0.00")
    '            totalAccountAmt += Val(If(IsDBNull(dtAcc.Rows(startRowIndex)("TotalAmount")), 0, dtAcc.Rows(startRowIndex)("TotalAmount")))
    '        Next

    '        ' Separator
    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(3).Value = "---------------------------"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(4).Value = "--------------------"

    '        ' Add Total row for Accounts
    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(3).Value = "Total of Accounts"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(4).Value = Format(totalAccountAmt, "0.00")

    '        ' Highlight total row
    '        Dim totalRowIndex As Integer = dg1.Rows.Count - 1
    '        dg1.Rows(totalRowIndex).DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)
    '        dg1.Rows(totalRowIndex).DefaultCellStyle.BackColor = Color.LightYellow

    '        ' ============================
    '        ' Difference Display
    '        ' ============================
    '        Dim diff As Double = totalChargesAmt - Val(If(IsDBNull(dtExp.Rows(0)("TotalExp")), 0, dtExp.Rows(0)("TotalExp")))

    '        dg1.Rows.Add()
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(1).Value = "Difference (Charges - Expenses)"
    '        dg1.Rows(dg1.Rows.Count - 1).Cells(2).Value = Format(diff, "0.00")

    '        If diff > 0 Then
    '            dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightGreen
    '        ElseIf diff < 0 Then
    '            dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightCoral
    '        Else
    '            dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.BackColor = Color.LightGray
    '        End If

    '        dg1.Rows(dg1.Rows.Count - 1).DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)

    '        ' Clear Selection
    '        dg1.ClearSelection()

    '    Catch ex As Exception
    '        MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
    '    End Try
    'End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        printRecord()
        Report_Viewer.printReport("\Reports\DailyNakal.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Ugrahi_Viewer.BringToFront()
        End If
    End Sub
    Private Sub PrintRecord()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            Application.DoEvents()
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With dg1.Rows(LastRecord)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                      "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "', " & _
                      "'" & .Cells(4).Value & "'"
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            ' LastRecord = LastCount
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,P1, P2,P3, P4) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try

        Next

    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
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

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub
End Class