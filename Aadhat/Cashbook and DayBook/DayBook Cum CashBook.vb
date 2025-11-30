Public Class DayBook_Cum_CashBook
    Private opbal As Decimal = 0.0
    Private Sub Cash_Bank_Book_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
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
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_KeyDown(sender As Object, e As KeyEventArgs) Handles MsktoDate.KeyDown
        If e.KeyCode = Keys.Enter Then btnShow.Focus()
    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Cash_Bank_Book_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        'clsFun.FillDropDownList(cbAccountName, "Select * From Accounts where groupid in(11,12)", "AccountName", "Id", "")
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mskFromDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyyy"))
        MsktoDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyyy"))
        rowColums() : Me.KeyPreview = True
    End Sub


    Private Sub rowColums()
        dg1.ColumnCount = 8
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Disc."
        dg1.Columns(1).Width = 299
        dg1.Columns(2).Name = "Type"
        dg1.Columns(2).Width = 110
        dg1.Columns(3).Name = "Receipt"
        dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "||"
        dg1.Columns(4).Width = 50
        dg1.Columns(5).Name = "Disc."
        dg1.Columns(5).Width = 299
        dg1.Columns(6).Name = "Type"
        dg1.Columns(6).Width = 110
        dg1.Columns(7).Name = "Payment"
        dg1.Columns(7).Width = 150
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        pnlWait.Visible = True
        RetriveOld()
        pnlWait.Visible = False
        ' Retrive()
    End Sub

    Private Sub RetriveOld()

        Dim ssql As String = ""
        Dim dtDates As New DataTable
        Dim dt As New DataTable
        Dim dtOp As New DataTable
        Dim dtAcc As New DataTable
        Dim tmpDt As New DataTable

        Dim lastval As Integer = 0

        Dim fromDate As String = CDate(mskFromDate.Text).ToString("yyyy-MM-dd")
        Dim toDate As String = CDate(MsktoDate.Text).ToString("yyyy-MM-dd")

        Dim opDr As Double = 0
        Dim opCr As Double = 0
        Dim opBal As Double = 0
        Dim opDC As String = "Dr"

        Dim DAY_DR As Double = 0
        Dim DAY_CR As Double = 0

        dg1.Rows.Clear()

        'ssql = "Select Entrydate, TransType,AccountName,Remark,Amount as Dr,'0' as Cr,Narration from Ledger where DC ='D'  and EntryDate = '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'   union all" &
        '      " Select Entrydate,  TransType,AccountName,Remark,'0' as Dr,Amount as Cr ,Narration from Ledger where Dc='C'  and EntryDate = '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'   "
        ssql = "SELECT EntryDate,TransType,AccountName,SUM(Cr) AS Cr,SUM(Cr) AS Cr,GroupID FROM (" &
       "SELECT L.EntryDate,L.TransType,A.AccountName,L.Amount AS Dr,0 AS Cr,A.GroupID FROM Ledger L INNER JOIN Accounts A ON A.ID=L.AccountID WHERE L.DC='D' AND L.EntryDate='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' UNION ALL SELECT L.EntryDate,L.TransType,A.AccountName,0 AS Dr,L.Amount AS Cr,A.GroupID FROM Ledger L INNER JOIN Accounts A ON A.ID=L.AccountID WHERE L.DC='C' AND L.EntryDate='" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') X GROUP BY EntryDate,TransType,AccountName,GroupID ORDER BY EntryDate,TransType,AccountName,GroupID"
        tmpDt = clsFun.ExecDataTable(ssql)
        If lastval > 20 Then dg1.Columns(4).Width = 30 Else dg1.Columns(4).Width = 50
        For j = 0 To tmpDt.Rows.Count - 1
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                If tmpDt.Rows(j)("Dr").ToString() <> "0" Then
                    dg1.Rows(lastval).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Cells(1).Value = tmpDt.Rows(j)("AccountName").ToString()
                    dg1.Rows(lastval).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Cells(2).Value = tmpDt.Rows(j)("TransType").ToString()
                    .Cells(3).Value = Format(Val(tmpDt.Rows(j)("Dr").ToString()), "0.00")
                    dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    drtotal = Format(Val(Val(drtotal) + Val(.Cells(3).Value)), "0.00")
                    If i = 0 And tmpopbaladd = False Then
                        drtotal = Format(Val(Val(drtotal) + Val(Val(drtotal1))), "0.00")
                        tmpopbaladd = True
                    End If
                ElseIf tmpDt.Rows(j)("Cr").ToString() <> "0" Then

                    dg1.Rows(lastval).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Cells(5).Value = tmpDt.Rows(j)("AccountName").ToString()
                    dg1.Rows(lastval).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    .Cells(6).Value = tmpDt.Rows(j)("TransType").ToString()
                    dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Cells(7).Value = Format(Val(tmpDt.Rows(j)("Cr").ToString()), "0.00")
                    crtotal = Format(Val(Val(crtotal) + Val(.Cells(7).Value)), "0.00")
                    If i = 0 And tmpopbaladd = False Then
                        crtotal = Format(Val(Val(crtotal) + Val(Val(crtotal1))), "0.00")
                        tmpopbaladd = True
                    End If
                End If
                .Cells(4).Value = "|"
                lastval = lastval + 1
            End With
            'If clsFun.ExecScalarInt("Select count(*) from Ledger where Dc='C'  and EntryDate = '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'") = 0 Then
            '    crtotal = Val(crtotal1)
            'End If
            'If clsFun.ExecScalarInt("Select count(*) from Ledger where Dc='D'  and EntryDate = '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'") = 0 Then
            '    drtotal = Val(drtotal1)

            'End If
        Next
    End Sub




    'Private Sub LoadCashBook()

    '    rowColums()
    '    dg1.Rows.Clear()

    '    Dim FromDate As Date = CDate(mskFromDate.Text)
    '    Dim ToDate As Date = CDate(MsktoDate.Text)

    '    Dim Opening As Double = 0
    '    Dim TotalReceipt As Double = 0
    '    Dim TotalPayment As Double = 0


    '    '===========================================================
    '    ' 1. OPENING
    '    '===========================================================
    '    Dim sqlOpening As String =
    '        "SELECT IFNULL(SUM(CASE WHEN DC='D' THEN OpBal ELSE -OpBal END),0) " &
    '        "FROM Accounts WHERE GroupID = 11"

    '    Opening = Val(clsFun.ExecScalarStr(sqlOpening))

    '    ' OPENING ROW
    '    If Opening >= 0 Then
    '        dg1.Rows.Add("", "Opening Balance", "", Opening.ToString("0.00"), "||", "", "", "")
    '        TotalReceipt += Opening
    '    Else
    '        dg1.Rows.Add("", "", "", "", "||", "Opening Balance", "", Math.Abs(Opening).ToString("0.00"))
    '        TotalPayment += Math.Abs(Opening)
    '    End If


    '    '===========================================================
    '    ' 2. NEW SQL BASED ON YOUR LATEST QUERY
    '    '===========================================================
    '    Dim sql As String =
    '        "SELECT EntryDate, AccountID, AccountName, DC, TransType, " &
    '        "SUM(CASE WHEN DC='D' THEN Amount ELSE 0 END) AS TotalDr, " &
    '        "SUM(CASE WHEN DC='C' THEN Amount ELSE 0 END) AS TotalCr " &
    '        "FROM Ledger " &
    '        "WHERE EntryDate >= '" & FromDate.ToString("yyyy-MM-dd") & "' " &
    '        "AND EntryDate <= '" & ToDate.ToString("yyyy-MM-dd") & "' " &
    '        "GROUP BY EntryDate, AccountID, AccountName, DC, TransType " &
    '        "ORDER BY EntryDate, AccountID, TransType, DC"

    '    Dim dt As DataTable = clsFun.ExecDataTable(sql)


    '    '===========================================================
    '    ' 3. FILL ROWS ACCORDING TO YOUR NEW RULE
    '    '===========================================================
    '    For Each dr As DataRow In dt.Rows

    '        Dim DiscLeft As String = ""
    '        Dim TypeLeft As String = ""
    '        Dim Receipt As String = ""

    '        Dim DiscRight As String = ""
    '        Dim TypeRight As String = ""
    '        Dim Payment As String = ""

    '        Dim totalDr As Double = Val(dr("TotalDr"))
    '        Dim totalCr As Double = Val(dr("TotalCr"))


    '        '------------------------------
    '        '  RULE:
    '        '  ✔ If TotalCr > 0 → LEFT SIDE
    '        '  ✔ If TotalDr > 0 → RIGHT SIDE
    '        '------------------------------

    '        If totalCr > 0 Then
    '            'credit → left side
    '            DiscLeft = dr("AccountName").ToString()
    '            TypeLeft = dr("TransType").ToString()
    '            Receipt = totalCr.ToString("0.00")
    '            TotalReceipt += totalCr
    '        End If

    '        If totalDr > 0 Then
    '            'debit → right side
    '            DiscRight = dr("AccountName").ToString()
    '            TypeRight = dr("TransType").ToString()
    '            Payment = totalDr.ToString("0.00")
    '            TotalPayment += totalDr
    '        End If


    '        dg1.Rows.Add("",
    '                     DiscLeft, TypeLeft, Receipt,
    '                     "||",
    '                     DiscRight, TypeRight, Payment)
    '    Next



    '    '===========================================================
    '    ' 4. CLOSING
    '    '===========================================================
    '    Dim Closing As Double = TotalReceipt - TotalPayment

    '    If Closing >= 0 Then
    '        dg1.Rows.Add("", "Closing Balance", "", Closing.ToString("0.00"), "||", "", "", "")
    '        TotalReceipt += Closing
    '    Else
    '        dg1.Rows.Add("", "", "", "", "||", "Closing Balance", "", Math.Abs(Closing).ToString("0.00"))
    '        TotalPayment += Math.Abs(Closing)
    '    End If


    '    '===========================================================
    '    ' 5. TOTAL ROW
    '    '===========================================================
    '    dg1.Rows.Add("", "TOTAL", "", TotalReceipt.ToString("0.00"), "||",
    '                 "TOTAL", "", TotalPayment.ToString("0.00"))

    'End Sub


    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,M1, P1, P2,P3, P4, P5, P6,P7) values('" & mskFromDate.Text & "'," & _
                    "'" & MsktoDate.Text & "',''," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," & _
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "');"

            End With
        Next
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            If cmd.ExecuteNonQuery() > 0 Then count = +1
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        '  clsFun.changeCompany()
        PrintRecord()
        Report_Viewer.printReport("\Cashbook.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub


    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles Dtp2.ValueChanged
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
End Class