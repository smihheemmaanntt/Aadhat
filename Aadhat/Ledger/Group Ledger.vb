Public Class Group_Ledger
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown, cbAccountName.KeyDown

        If cbAccountName.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(cbAccountName, "Select * from Account_AcGrp where (Groupid in(16,17)  or UnderGroupID in (16,17))", "AccountName", "Id", "")
                CreateAccount.BringToFront()
                mindate = clsFun.ExecScalarStr("Select min(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
                maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
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
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
            'SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select

    End Sub

    Private Sub Ledger_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        mskFromDate.SelectAll()
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub
    Private Sub Groupped_Ledger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        clsFun.FillDropDownList(cbAccountName, "Select GroupID,GroupName from Account_AcGrp Group by GroupID ", "GroupName", "GroupID", "--All--")
        Dim mindate = String.Empty : Dim maxdate As String = String.Empty
        mskFromDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyy"))
        MsktoDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyy"))
        rowColums()
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name" : dg1.Columns(1).Width = 400
        dg1.Columns(2).Name = "Group Name" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "Op Bal" : dg1.Columns(2).Width = 200
        dg1.Columns(4).Name = "Debit" : dg1.Columns(3).Width = 150
        dg1.Columns(5).Name = "Credit" : dg1.Columns(4).Width = 150
        dg1.Columns(6).Name = "Balance" : dg1.Columns(5).Width = 200
    End Sub

    Public Sub retrive(Optional ByVal condtion As String = "")
        Dim sql As String = String.Empty
        Dim OpTotal As Decimal = 0
        Dim Drtotal As Decimal = 0
        Dim CrTotal As Decimal = 0
        Dim CloseTotal As Decimal = 0
        sql = "SELECT A.ID as ID, A.AccountName as AccountName, A.GroupName as GroupName, " & _
             " ROUND(CASE WHEN A.DC='Dr' THEN IFNULL(A.Opbal,0)+(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate<'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" & _
              " -(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') ELSE -IFNULL(A.Opbal,0)- " & _
              " (SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')+(SELECT IFNULL(SUM(Amount),0) FROM Ledger " & _
              " WHERE AccountID=A.ID AND DC='D' AND EntryDate<'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') END,2) AS TotalOpbal," & _
              " ROUND((SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'),2) AS TotalDr, " & _
              " ROUND((SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'),2) AS TotalCr,  " & _
              " ROUND(CASE WHEN A.DC='Dr' THEN IFNULL(A.Opbal,0)+(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate<='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
              " -(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') ELSE -IFNULL(A.Opbal,0)- " & _
              " (SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')+(SELECT IFNULL(SUM(Amount),0) FROM Ledger " & _
              " WHERE AccountID=A.ID AND DC='D' AND EntryDate<='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') END,2) AS TotalRestbal FROM Account_AcGrp A WHERE A.GroupID=" & Val(cbAccountName.SelectedValue) & " and TotalRestbal<>0 " & _
              " ORDER BY UPPER(A.AccountName);"
        dt = clsFun.ExecDataTable(Sql)
        If Val(dt.Rows.Count) > 20 Then dg1.Columns(5).Width = 150
        dg1.Rows.Clear()
        For i = 0 To dt.Rows.Count - 1
            lblRecordCount.Visible = True
            lblRecordCount.Text = "Total Records : " & dt.Rows.Count
            dg1.Rows.Add()
            With dg1.Rows(i)
                .Cells(0).Value = dt.Rows(i)("ID").ToString()
                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                .Cells(2).Value = dt.Rows(i)("GroupName").ToString()
                .Cells(3).Value = IIf(Val(dt.Rows(i)("TotalOpbal").ToString()) > 0, Format(Math.Abs(Val(dt.Rows(i)("TotalOpbal").ToString())), "0.00") & " " & "Dr", Format(Math.Abs(Val(dt.Rows(i)("TotalOpbal").ToString())), "0.00") & " " & "Cr")
                .Cells(4).Value = Format(Val(dt.Rows(i)("TotalDr").ToString()), "0.00")
                .Cells(5).Value = Format(Val(dt.Rows(i)("TotalCr").ToString()), "0.00")
                .Cells(6).Value = IIf(Val(dt.Rows(i)("TotalRestbal").ToString()) > 0, Format(Math.Abs(Val(dt.Rows(i)("TotalRestbal").ToString())), "0.00") & "Dr", Format(Math.Abs(Val(dt.Rows(i)("TotalRestbal").ToString())), "0.00") & "Cr")
                OpTotal += Val(dt.Rows(i)("TotalOpbal").ToString())
                Drtotal += Val(dt.Rows(i)("TotalDr").ToString())
                CrTotal += Val(dt.Rows(i)("TotalCr").ToString())
                CloseTotal += Val(dt.Rows(i)("TotalRestbal").ToString())
            End With
        Next
        txtOpbal.Text = IIf(Val(CloseTotal) > 0, Format(Math.Abs(Val(OpTotal)), "0.00") & " " & "Dr", Format(Math.Abs(Val(OpTotal)), "0.00") & " " & "Cr")
        txtBalAmt.Text = IIf(Val(CloseTotal) > 0, Format(Math.Abs(Val(CloseTotal)), "0.00") & " " & "Dr", Format(Math.Abs(Val(CloseTotal)), "0.00") & " " & "Cr")
        txtDramt.Text = Format(Math.Abs(Val(Drtotal)), "0.00")
        txtcrAmt.Text = Format(Math.Abs(Val(CrTotal)), "0.00")
        dg1.ClearSelection()
    End Sub
   

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
    End Sub
    Private Sub rowColumsTemp()
        tmpgrid.ColumnCount = 15
        tmpgrid.Columns(0).Name = "ID" : tmpgrid.Columns(0).Visible = False
        tmpgrid.Columns(1).Name = "Date" : tmpgrid.Columns(1).Width = 130
        tmpgrid.Columns(2).Name = "Type" : tmpgrid.Columns(2).Width = 150
        tmpgrid.Columns(3).Name = "Account Name" : tmpgrid.Columns(3).Width = 100
        tmpgrid.Columns(4).Name = "Description" : tmpgrid.Columns(4).Width = 545
        tmpgrid.Columns(5).Name = "Debit" : tmpgrid.Columns(5).Width = 100
        tmpgrid.Columns(6).Name = "Credit" : tmpgrid.Columns(6).Width = 100
        tmpgrid.Columns(7).Name = "Balance" : tmpgrid.Columns(7).Width = 150
        tmpgrid.Columns(8).Name = "HindiName" : tmpgrid.Columns(8).Width = 100
        tmpgrid.Columns(9).Name = "HindiItem" : tmpgrid.Columns(9).Width = 100
        tmpgrid.Columns(10).Name = "DrTotal" : tmpgrid.Columns(10).Width = 100
        tmpgrid.Columns(11).Name = "CrTotal" : tmpgrid.Columns(11).Width = 100
        tmpgrid.Columns(12).Name = "OpBalTotal" : tmpgrid.Columns(12).Width = 100
        tmpgrid.Columns(13).Name = "CalbalTotal" : tmpgrid.Columns(13).Width = 100
        tmpgrid.Columns(14).Name = "RowCount" : tmpgrid.Columns(14).Width = 100
    End Sub

    Private Sub RetriveLedger()
        Dim i As Integer = 0
        tmpgrid.Rows.Clear()
        Dim sql As String = String.Empty
        Dim dt As DataTable
        ' Set ProgressBar initial settings
        pb1.Visible = True
        pb1.Minimum = 0
        pb1.Maximum = dg1.Rows.Count
        pb1.Value = 0

        ' Step 1: Temporary list to store last balance row index
        Dim lastRowIndexList As New List(Of Integer)

        For i = 0 To dg1.Rows.Count - 1
            Dim runningBalance As Decimal = 0
            Dim Drtotal As Decimal = 0
            Dim CrTotal As Decimal = 0
            Dim isFirstRow As Boolean = True
            Dim lastRunningBalance As Decimal = 0
            Dim accountId As Integer = Val(dg1.Rows(i).Cells(0).Value)
            Dim RowCount As Integer = 0
            sql = "SELECT  A.AccountName,A.OtherName,  " & _
                  "ROUND(CASE WHEN A.DC = 'Dr' THEN IFNULL(A.Opbal,0) + " & _
                  "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') - " & _
                  "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') " & _
                  "ELSE -IFNULL(A.Opbal,0) - (SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') " & _
                  "+ (SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') END, 2) AS OpeningBalance, " & _
                  "L.EntryDate, L.VourchersID, L.TransType, L.Remark,L.RemarkHindi, L.Narration, " & _
                  "CASE WHEN L.DC = 'D' THEN ROUND(L.Amount, 2) ELSE 0 END AS Dr, " & _
                  "CASE WHEN L.DC = 'C' THEN ROUND(L.Amount, 2) ELSE 0 END AS Cr " & _
                  "FROM Account_AcGrp A LEFT JOIN Ledger L ON L.AccountID = A.ID " & _
                  "WHERE A.ID = " & accountId & "  AND L.EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & _
                  "ORDER BY L.VourchersID, L.EntryDate"
            dt = clsFun.ExecDataTable(sql)
            For Each row As DataRow In dt.Rows
                Dim dr As Decimal = Val(row("Dr"))
                Dim cr As Decimal = Val(row("Cr"))
                Dim openingBalance As Decimal = Val(row("OpeningBalance"))
                If isFirstRow Then
                    runningBalance = openingBalance
                    isFirstRow = False
                End If
                runningBalance += dr
                runningBalance -= cr
                Drtotal += dr
                CrTotal += cr
                RowCount += 1
                ' Add row to grid
                tmpgrid.Rows.Add(row("L.VourchersID"),
                  CDate(row("L.EntryDate")).ToString("dd-MM-yyyy"),
                  row("L.TransType"),
                  row("A.AccountName"),
                  row("L.Remark"),
                  Format(dr, "0.00"),
                  Format(cr, "0.00"),
                  IIf(runningBalance > 0, Format(Math.Abs(runningBalance), "0.00") & " Dr", Format(Math.Abs(runningBalance), "0.00") & " Cr"),
                  row("A.OtherName"),
                  row("L.RemarkHindi"), Drtotal, CrTotal,
                  IIf(openingBalance > 0, Format(Math.Abs(openingBalance), "0.00") & " Dr", Format(Math.Abs(openingBalance), "0.00") & " Cr"),
                  "", RowCount)
                lastRunningBalance = runningBalance
            Next

            ' ---- Set Final Balance in Each Row of this Account Block ----
            Dim lastRowCount As Integer = dt.Rows.Count + 1 ' +1 for opening row
            If lastRowCount > 0 Then
                For j As Integer = tmpgrid.Rows.Count - lastRowCount To tmpgrid.Rows.Count - 1
                    tmpgrid.Rows(j).Cells("CalbalTotal").Value = IIf(lastRunningBalance > 0, Format(lastRunningBalance, "0.00") & " Dr", Format(Math.Abs(lastRunningBalance), "0.00") & " Cr")
                Next
            End If

            ' Update ProgressBar
            pb1.Value = i + 1
            Application.DoEvents()
        Next
        ' Hide ProgressBar after completion
        pb1.Value = 0
        pb1.Visible = False
    End Sub
    Private Sub PrintRecord()
        Dim AllRecord As Integer = Val(tmpgrid.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        If tmpgrid.RowCount <> 0 Then
            For i As Integer = 0 To maxRowCount - 1
                'Application.DoEvents()
                FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
                For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                    With tmpgrid.Rows(LastRecord)
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," &
                            "'" & MsktoDate.Text & "','" & .Cells("Account Name").Value & "','" & .Cells("OpBalTotal").Value & "','" & .Cells("DrTotal").Value & "','" & .Cells("CrTotal").Value & "'," &
                            "'" & .Cells("CalbalTotal").Value & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "','" & IIf(ckPrintHindi.Checked = True, .Cells("HindiItem").Value, .Cells("Description").Value) & "'," &
                            "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','',''," & Val(.Cells("RowCount").Value) & ""
                    End With
                    LastRecord = Val(LastRecord + 1)
                Next
                ' LastRecord = LastCount
                Try
                    If FastQuery = String.Empty Then Exit Sub
                    sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11) " & FastQuery & ""
                    ClsFunPrimary.ExecNonQuery(sQL)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            Next
        End If
    End Sub


    'Private Sub RetriveLedger()
    '    Dim i As Integer = 0
    '    tmpgrid.Rows.Clear()
    '    Dim sql As String = String.Empty
    '    Dim dt As DataTable

    '    ' Step 1: Temporary list to store last balance row index
    '    Dim lastRowIndexList As New List(Of Integer)

    '    For i = 0 To dg1.Rows.Count - 1
    '        Dim runningBalance As Decimal = 0
    '        Dim Drtotal As Decimal = 0
    '        Dim CrTotal As Decimal = 0
    '        Dim isFirstRow As Boolean = True
    '        Dim lastRunningBalance As Decimal = 0
    '        Dim accountId As Integer = Val(dg1.Rows(i).Cells(0).Value)

    '        sql = "SELECT  A.AccountName,A.OtherName,  " & _
    '              "ROUND(CASE WHEN A.DC = 'Dr' THEN IFNULL(A.Opbal,0) + " & _
    '              "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') - " & _
    '              "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') " & _
    '              "ELSE -IFNULL(A.Opbal,0) - (SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') " & _
    '              "+ (SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "') END, 2) AS OpeningBalance, " & _
    '              "L.EntryDate, L.VourchersID, L.TransType, L.Remark,L.RemarkHindi, L.Narration, " & _
    '              "CASE WHEN L.DC = 'D' THEN ROUND(L.Amount, 2) ELSE 0 END AS Dr, " & _
    '              "CASE WHEN L.DC = 'C' THEN ROUND(L.Amount, 2) ELSE 0 END AS Cr " & _
    '              "FROM Account_AcGrp A LEFT JOIN Ledger L ON L.AccountID = A.ID " & _
    '              "WHERE A.ID = " & accountId & "  AND L.EntryDate BETWEEN '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' " & _
    '              "ORDER BY L.VourchersID, L.EntryDate"

    '        dt = clsFun.ExecDataTable(sql)
    '        For Each row As DataRow In dt.Rows
    '            Dim dr As Decimal = Val(row("Dr"))
    '            Dim cr As Decimal = Val(row("Cr"))
    '            Dim openingBalance As Decimal = Val(row("OpeningBalance"))

    '            If isFirstRow Then
    '                runningBalance = openingBalance
    '                isFirstRow = False
    '            End If

    '            runningBalance += dr
    '            runningBalance -= cr
    '            Drtotal += dr
    '            CrTotal += cr

    '            ' Add row to grid
    '            tmpgrid.Rows.Add(row("L.VourchersID"),
    '              CDate(row("L.EntryDate")).ToString("dd-MM-yyyy"),
    '              row("L.TransType"),
    '              row("A.AccountName"),
    '              row("L.Remark"),
    '              Format(dr, "0.00"),
    '              Format(cr, "0.00"),
    '              IIf(runningBalance > 0, Format(Math.Abs(runningBalance), "0.00") & " Dr", Format(Math.Abs(runningBalance), "0.00") & " Cr"),
    '              row("A.OtherName"),
    '              "", Drtotal, CrTotal,
    '              IIf(openingBalance > 0, Format(Math.Abs(openingBalance), "0.00") & " Dr", Format(Math.Abs(openingBalance), "0.00") & " Cr"),
    '              "") ' Last column "FinalBal" as blank initially
    '            lastRunningBalance = runningBalance
    '        Next
    '        ' Add lastRunningBalance to recent rows just added for this account
    '        ' ---- Set Final Balance in Each Row of this Account Block ----
    '        Dim lastRowCount As Integer = dt.Rows.Count + 1 ' +1 for opening row
    '        If lastRowCount > 0 Then
    '            For j As Integer = tmpgrid.Rows.Count - lastRowCount To tmpgrid.Rows.Count - 1
    '                tmpgrid.Rows(j).Cells("CalbalTotal").Value = IIf(lastRunningBalance > 0, Format(lastRunningBalance, "0.00") & " Dr", Format(Math.Abs(lastRunningBalance), "0.00") & " Cr")
    '            Next
    '        End If
    '    Next
    'End Sub

  
  
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If pnlPrint.Visible = False Then pnlPrint.Visible = True : btnPrintOutstanding.Focus()
    End Sub

    Private Sub btnPrintLedger_Click(sender As Object, e As EventArgs) Handles btnPrintLedger.Click
        pnlWait.Visible = True : pnlPrint.Visible = False : rowColumsTemp() : RetriveLedger() : PrintRecord()
        'If ckJoin.Checked = True Then
        '    WithoutDiscriptionPrint()
        'Else
        '    PrintRecord()
        'End If
        If ckPrintHindi.Checked = True Then
            Report_Viewer.printReport("\Reports\GroupLedger2.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            Report_Viewer.BringToFront()
        Else
            Report_Viewer.printReport("\Reports\GroupLedger.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            Report_Viewer.BringToFront()
        End If
        pnlWait.Visible = False
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles Dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles Dtp2.ValueChanged
        MsktoDate.Text = Dtp2.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
End Class