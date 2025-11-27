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

        Dim dtDates As New DataTable
        Dim dt As New DataTable
        Dim dtOp As New DataTable

        Dim last As Integer = 0

        Dim fromDate As String = CDate(mskFromDate.Text).ToString("yyyy-MM-dd")
        Dim toDate As String = CDate(MsktoDate.Text).ToString("yyyy-MM-dd")

        Dim OPEN_DR As Double = 0
        Dim OPEN_CR As Double = 0
        Dim OPEN_BAL As Double = 0
        Dim OPEN_TYPE As String = "Dr"

        dg1.Rows.Clear()

        '------------------ 1. OPENING BALANCE (CASH GROUP 11) ------------------
        Dim s_op As String = ""
        s_op &= "SELECT "
        s_op &= " SUM(CASE WHEN L.DC='D' THEN L.Amount ELSE 0 END) AS DrAmt, "
        s_op &= " SUM(CASE WHEN L.DC='C' THEN L.Amount ELSE 0 END) AS CrAmt "
        s_op &= "FROM Ledger L "
        s_op &= "JOIN Accounts A ON A.ID=L.AccountID "
        s_op &= "WHERE A.GroupID=11 AND EntryDate < '" & fromDate & "'"

        dtOp = clsFun.ExecDataTable(s_op)

        If dtOp.Rows.Count > 0 Then
            If Not IsDBNull(dtOp.Rows(0)("DrAmt")) Then OPEN_DR = Val(dtOp.Rows(0)("DrAmt"))
            If Not IsDBNull(dtOp.Rows(0)("CrAmt")) Then OPEN_CR = Val(dtOp.Rows(0)("CrAmt"))
        End If

        OPEN_BAL = Math.Abs(OPEN_DR - OPEN_CR)
        If OPEN_DR >= OPEN_CR Then
            OPEN_TYPE = "Dr"
        Else
            OPEN_TYPE = "Cr"
        End If

        '------------------ 2. DATE LIST ------------------
        Dim sqlDates As String = ""
        sqlDates &= "SELECT DISTINCT EntryDate FROM Ledger "
        sqlDates &= "WHERE EntryDate BETWEEN '" & fromDate & "' AND '" & toDate & "' "
        sqlDates &= "ORDER BY EntryDate"

        dtDates = clsFun.ExecDataTable(sqlDates)

        If dtDates.Rows.Count = 0 Then Exit Sub


        '------------------ 3. LOOP DATEWISE ------------------
        Dim i As Integer

        For i = 0 To dtDates.Rows.Count - 1

            Dim selDate As String = CDate(dtDates.Rows(i)("EntryDate")).ToString("yyyy-MM-dd")

            dg1.Rows.Add()
            dg1.Rows(last).Cells(1).Value = "Date : " & CDate(selDate).ToString("dd-MM-yyyy")
            dg1.Rows(last).Cells(5).Value = "Date : " & CDate(selDate).ToString("dd-MM-yyyy")
            dg1.Rows(last).Cells(4).Value = "|"
            last += 1

            '------------------Opening Balance------------------
            dg1.Rows.Add()

            If OPEN_TYPE = "Dr" Then
                dg1.Rows(last).Cells(1).Style.ForeColor = Color.Blue
                dg1.Rows(last).Cells(1).Value = "Opening Balance"
                dg1.Rows(last).Cells(3).Value = Format(OPEN_BAL, "0.00")
            Else
                dg1.Rows(last).Cells(5).Style.ForeColor = Color.Blue
                dg1.Rows(last).Cells(5).Value = "Opening Balance"
                dg1.Rows(last).Cells(7).Value = Format(OPEN_BAL, "0.00")
            End If

            dg1.Rows(last).Cells(4).Value = "|"
            last += 1

            Dim DAY_DR As Double = 0
            Dim DAY_CR As Double = 0

            '------------------ Fetch Daily Ledger ------------------
            Dim sql As String = ""
            sql &= "SELECT L.*, A.GroupID FROM Ledger L "
            sql &= "JOIN Accounts A ON A.ID=L.AccountID "
            sql &= "WHERE L.EntryDate='" & selDate & "' "
            sql &= "ORDER BY L.DC, L.TransType, L.AccountID"

            dt = clsFun.ExecDataTable(sql)

            Dim r As Integer

            For r = 0 To dt.Rows.Count - 1

                Dim AC As String = dt.Rows(r)("AccountName").ToString()
                Dim rm As String = ""
                If Not IsDBNull(dt.Rows(r)("Narration")) Then REM = dt.Rows(r)("Narration").ToString()
                    Dim TR As String = dt.Rows(r)("TransType").ToString()
                    Dim DC As String = dt.Rows(r)("DC").ToString()
                    Dim AMT As Double = Val(dt.Rows(r)("Amount"))
                    Dim GRP As Integer = Val(dt.Rows(r)("GroupID"))
                    dg1.Rows.Add()
                    '------------------ CASHBOOK (GROUP 11) ------------------
                    If GRP = 11 Then

                        If DC = "D" Then
                            dg1.Rows(last).Cells(1).Value = AC & " - " & rm
                            dg1.Rows(last).Cells(2).Value = TR
                            dg1.Rows(last).Cells(3).Value = Format(AMT, "0.00")
                            DAY_DR = DAY_DR + AMT
                        Else
                            dg1.Rows(last).Cells(5).Value = AC & " - " & rm
                            dg1.Rows(last).Cells(6).Value = TR
                            dg1.Rows(last).Cells(7).Value = Format(AMT, "0.00")
                            DAY_CR = DAY_CR + AMT
                        End If

                    Else

                        '------------------ DAYBOOK (UDHAR/JOURNAL) ------------------
                        If DC = "D" Then
                            dg1.Rows(last).Cells(1).Value = AC & " - " & rm
                            dg1.Rows(last).Cells(3).Value = Format(AMT, "0.00")
                        Else
                            dg1.Rows(last).Cells(5).Value = AC & " - " & rm
                            dg1.Rows(last).Cells(7).Value = Format(AMT, "0.00")
                        End If

                    End If

                    dg1.Rows(last).Cells(4).Value = "|"
                    last += 1
                End If

            Next r

            '------------------ TOTAL ROW ------------------
            dg1.Rows.Add()
            dg1.Rows(last).Cells(2).Value = "Total"
            dg1.Rows(last).Cells(3).Value = Format(DAY_DR, "0.00")
            dg1.Rows(last).Cells(6).Value = "Total"
            dg1.Rows(last).Cells(7).Value = Format(DAY_CR, "0.00")
            dg1.Rows(last).Cells(4).Value = "|"
            last += 1

            '------------------ CLOSING BALANCE ------------------
            Dim CLOS As Double = 0
            Dim BalType As String = "Dr"

            If (OPEN_BAL + DAY_DR) > DAY_CR Then
                CLOS = (OPEN_BAL + DAY_DR) - DAY_CR
                BalType = "Dr"
            Else
                CLOS = DAY_CR - (OPEN_BAL + DAY_DR)
                BalType = "Cr"
            End If

            dg1.Rows.Add()

            If BalType = "Dr" Then
                dg1.Rows(last).Cells(1).Value = "Closing Balance"
                dg1.Rows(last).Cells(3).Value = Format(CLOS, "0.00")
            Else
                dg1.Rows(last).Cells(5).Value = "Closing Balance"
                dg1.Rows(last).Cells(7).Value = Format(CLOS, "0.00")
            End If

            dg1.Rows(last).Cells(4).Value = "|"
            last += 1

            'Next day opening
            OPEN_BAL = CLOS
            OPEN_TYPE = BalType

            '------------------ SEPARATOR ------------------
            dg1.Rows.Add()
            dg1.Rows(last).Cells(3).Value = "-----------"
            dg1.Rows(last).Cells(7).Value = "-----------"
            dg1.Rows(last).Cells(4).Value = "|"
            last += 1

        Next i

        dg1.ClearSelection()

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