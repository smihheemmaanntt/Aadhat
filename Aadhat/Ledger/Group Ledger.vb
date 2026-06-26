Public Class Group_Ledger
    Private headerCheckBox As CheckBox = New CheckBox()
    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub

    Private Sub txtFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles cbAccountName.KeyDown, txtFromDate.KeyDown, txttoDate.KeyDown
        If cbAccountName.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(cbAccountName, "Select * from Account_AcGrp where (Groupid in(16,17)  or UnderGroupID in (16,17))", "AccountName", "Id", "")
                CreateAccount.BringToFront()
                mindate = clsFun.ExecScalarStr("Select min(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
                maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
                If mindate <> "" Then
                    txtFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
                Else
                    txtFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
                End If
                If maxdate <> "" Then
                    txttoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
                Else
                    txttoDate.Text = Date.Today.ToString("dd-MM-yyyy")
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
        If e.KeyCode = Keys.Escape Then
            If pnlPrint.Visible = True Then pnlPrint.Visible = False : Exit Sub
            Me.Close()
        End If

    End Sub
    Private Sub txtFromDate_GotFocus(sender As Object, e As EventArgs) Handles txtFromDate.GotFocus
        txtFromDate.SelectAll()
    End Sub
    Private Sub txtToDate_GotFocus(sender As Object, e As EventArgs) Handles txttoDate.GotFocus
        txttoDate.SelectAll()
    End Sub
    Private Sub Groupped_Ledger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        clsFun.FillDropDownList(cbAccountName, "Select GroupID,GroupName from Account_AcGrp Group by GroupID ", "GroupName", "GroupID", "")
        cbAccountName.SelectedValue = 32
        Dim mindate As Date = FinYearStart
        Dim maxdate As String = String.Empty
        txtFromDate.Text = If(mindate <> Date.MinValue,
                              mindate.ToString("dd-MM-yyyy"),
                             Date.Today.ToString("dd-MM-yyyy"))
        txttoDate.Text = If(maxdate <> "",
                            CDate(maxdate).ToString("dd-MM-yyyy"),
                            Date.Today.ToString("dd-MM-yyyy"))

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

    Private Sub dg1_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles dg1.CellBeginEdit
        ' Sirf checkbox column allow
        If e.ColumnIndex <> 0 Then
            e.Cancel = True
        End If
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
    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            dg1.CommitEdit(DataGridViewDataErrorContexts.Commit)
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
        dg1.ColumnCount = 9
        dg1.Columns(1).Name = "ID" : dg1.Columns(1).Visible = False
        dg1.Columns(2).Name = "Account Name" : dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Group Name" : dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Op Bal" : dg1.Columns(4).Width = 150
        dg1.Columns(5).Name = "Debit" : dg1.Columns(5).Width = 150
        dg1.Columns(6).Name = "Credit" : dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "Balance" : dg1.Columns(7).Width = 200
        dg1.Columns(8).Name = "OtherName" : dg1.Columns(8).Visible = False
    End Sub
    Private Sub RetriveGroupLedger()
        dg1.Rows.Clear()
        txtOpbal.Text = ""
        txtBalAmt.Text = ""
        txtDramt.Text = "0.00"
        txtcrAmt.Text = "0.00"

        Dim fromDate As Date = CDate(txtFromDate.Text)
        Dim toDate As Date = CDate(txttoDate.Text)

        '*** Debtors O/S की तरह – सिर्फ वही party जिनका RestBal <> 0 है
        ' अगर किसी खास Group (Debtors) के लिए चाहिए तो यहां GroupID की condition जोड़ लेना
        ' जैसे: "Where RestBal <> 0 And GroupID = 1"
        Dim accSql As String = "Select ID, AccountName, DC, ifnull(OtherName,'') as OtherName " &
                               "From Accounts Order By upper(AccountName)"

        Dim dtAcc As DataTable = clsFun.ExecDataTable(accSql)

        Dim grandDr As Decimal = 0D
        Dim grandCr As Decimal = 0D

        For Each accRow As DataRow In dtAcc.Rows

            Dim accId As Integer = CInt(accRow("ID"))
            Dim accName As String = accRow("AccountName").ToString()
            Dim drcr As String = accRow("DC").ToString()
            Dim otherName As String = accRow("OtherName").ToString()

            '************ Opening Balance (आपके पुराने formula जैसा ही) ************
            Dim opbalSql As String =
                "Select Round((Case When DC='Dr' then " &
                " (ifnull(opbal,0) + (Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
                "   Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate < '" & fromDate.ToString("yyyy-MM-dd") & "')" &
                " - (Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
                "   Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate < '" & fromDate.ToString("yyyy-MM-dd") & "')" &
                " ) else " &
                " (ifnull(-(opbal),0) + -(Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
                "   Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate < '" & fromDate.ToString("yyyy-MM-dd") & "')" &
                " + (Select ifnull(Round(Sum(Amount),2),0) From Ledger " &
                "   Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate < '" & fromDate.ToString("yyyy-MM-dd") & "')" &
                " ) end),2) as Restbal " &
                "From Accounts Where ID=" & accId

            Dim opBal As Decimal = Val(clsFun.ExecScalarStr(opbalSql))

            '************ उस party के ledger transactions ************
            Dim tranSql As String =
                "Select VourchersID, Entrydate, TransType, AccountName, Remark, RemarkHindi, " &
                " round(Case When DC='D' then Amount else 0 end,2) as Dr, " &
                " round(Case When DC='C' then Amount else 0 end,2) as Cr " &
                "From Ledger " &
                "Where AccountID=" & accId &
                "  And EntryDate Between '" & fromDate.ToString("yyyy-MM-dd") &
                "' And '" & toDate.ToString("yyyy-MM-dd") & "'" &
                "Order By EntryDate, VourchersID"

            Dim dtLed As DataTable = clsFun.ExecDataTable(tranSql)

            ' अगर ना opening है, ना ही period में कोई transaction, तो skip
            If opBal = 0D AndAlso dtLed.Rows.Count = 0 Then
                Continue For
            End If

            Dim partyDr As Decimal = 0D
            Dim partyCr As Decimal = 0D
            Dim bal As Decimal = opBal

            '************ Party Header row  (जैसे PDF में "Party : XYZ") ************
            Dim rIndex As Integer = dg1.Rows.Add()
            With dg1.Rows(rIndex)
                .Cells(1).Value = "Party : " & accName
                .Cells(3).Value = accName
                '.Cells(8).Value = otherName
                .DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)
            End With

            '************ Opening Balance row (01/04/25 Op.Bal. जैसा) ************
            If opBal <> 0D Then
                rIndex = dg1.Rows.Add()
                With dg1.Rows(rIndex)
                    .Cells(1).Value = fromDate.ToString("dd-MM-yyyy")
                    .Cells(2).Value = "Op.Bal."
                    If opBal >= 0D Then
                        .Cells(5).Value = Format(Math.Abs(opBal), "0.00")  'Dr
                        .Cells(6).Value = ""                               'Cr
                    Else
                        .Cells(5).Value = ""
                        .Cells(6).Value = Format(Math.Abs(opBal), "0.00")  'Cr
                    End If
                    .Cells(7).Value = If(bal >= 0D,
                                         Format(Math.Abs(bal), "0.00") & " Dr",
                                         Format(Math.Abs(bal), "0.00") & " Cr")
                End With

                If opBal >= 0D Then
                    partyDr += Math.Abs(opBal)
                Else
                    partyCr += Math.Abs(opBal)
                End If
            End If

            '************ Ledger की सारी entries ************
            For Each tRow As DataRow In dtLed.Rows

                Dim drAmt As Decimal = Val(tRow("Dr").ToString())
                Dim crAmt As Decimal = Val(tRow("Cr").ToString())

                ' Running balance
                bal += drAmt
                bal -= crAmt

                partyDr += drAmt
                partyCr += crAmt

                rIndex = dg1.Rows.Add()
                With dg1.Rows(rIndex)
                    .Cells(0).Value = tRow("VourchersID").ToString()
                    .Cells(1).Value = CDate(tRow("EntryDate")).ToString("dd-MM-yyyy")
                    .Cells(2).Value = tRow("TransType").ToString()
                    .Cells(3).Value = accName
                    .Cells(4).Value = tRow("Remark").ToString()
                    .Cells(5).Value = If(drAmt = 0D, "", Format(drAmt, "0.00"))
                    .Cells(6).Value = If(crAmt = 0D, "", Format(crAmt, "0.00"))
                    .Cells(7).Value = If(bal >= 0D,
                                         Format(Math.Abs(bal), "0.00") & " Dr",
                                         Format(Math.Abs(bal), "0.00") & " Cr")
                    .Cells(8).Value = otherName
                    .Cells(9).Value = tRow("RemarkHindi").ToString()
                End With

            Next

            '************ Party Total row (जैसे PDF में "Party Total") ************
            rIndex = dg1.Rows.Add()
            With dg1.Rows(rIndex)
                .Cells(3).Value = "Party Total"
                .Cells(5).Value = If(partyDr = 0D, "", Format(partyDr, "0.00"))
                .Cells(6).Value = If(partyCr = 0D, "", Format(partyCr, "0.00"))
                .DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)
            End With

            '************ Party Balance row (जैसे PDF में "Party Balance") ************
            rIndex = dg1.Rows.Add()
            With dg1.Rows(rIndex)
                .Cells(3).Value = "Party Balance"
                .Cells(7).Value = If(bal >= 0D,
                                     Format(Math.Abs(bal), "0.00") & " Dr",
                                     Format(Math.Abs(bal), "0.00") & " Cr")
                .DefaultCellStyle.Font = New Font(dg1.Font, FontStyle.Bold)
            End With

            grandDr += partyDr
            grandCr += partyCr

        Next

        '***** Grand Total Dr / Cr textboxes में *****
        txtDramt.Text = Format(grandDr, "0.00")
        txtcrAmt.Text = Format(grandCr, "0.00")

        dg1.ClearSelection()

    End Sub

    Public Sub retrive(Optional ByVal condtion As String = "")
        Dim sql As String = String.Empty
        Dim OpTotal As Decimal = 0
        Dim Drtotal As Decimal = 0
        Dim CrTotal As Decimal = 0
        Dim CloseTotal As Decimal = 0
        If ckShowAll.Checked = True Then
            sql = "SELECT A.ID as ID, A.AccountName as AccountName, A.GroupName as GroupName, " & _
     " ROUND(CASE WHEN A.DC='Dr' THEN IFNULL(A.Opbal,0)+(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "')" & _
      " -(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') ELSE -IFNULL(A.Opbal,0)- " & _
      " (SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "')+(SELECT IFNULL(SUM(Amount),0) FROM Ledger " & _
      " WHERE AccountID=A.ID AND DC='D' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') END,2) AS TotalOpbal," & _
      " ROUND((SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate BETWEEN '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "'),2) AS TotalDr, " & _
      " ROUND((SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate BETWEEN '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "'),2) AS TotalCr,  " & _
      " ROUND(CASE WHEN A.DC='Dr' THEN IFNULL(A.Opbal,0)+(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "')" & _
      " -(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "') ELSE -IFNULL(A.Opbal,0)- " & _
      " (SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "')+(SELECT IFNULL(SUM(Amount),0) FROM Ledger " & _
      " WHERE AccountID=A.ID AND DC='D' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "') END,2) AS TotalRestbal FROM Account_AcGrp A WHERE A.GroupID=" & Val(cbAccountName.SelectedValue) & "  " & _
      " ORDER BY UPPER(A.AccountName);"
        Else
            sql = "SELECT A.ID as ID, A.AccountName as AccountName, A.GroupName as GroupName, " & _
                 " ROUND(CASE WHEN A.DC='Dr' THEN IFNULL(A.Opbal,0)+(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "')" & _
                  " -(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') ELSE -IFNULL(A.Opbal,0)- " & _
                  " (SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "')+(SELECT IFNULL(SUM(Amount),0) FROM Ledger " & _
                  " WHERE AccountID=A.ID AND DC='D' AND EntryDate<'" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') END,2) AS TotalOpbal," & _
                  " ROUND((SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate BETWEEN '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "'),2) AS TotalDr, " & _
                  " ROUND((SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate BETWEEN '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "'),2) AS TotalCr,  " & _
                  " ROUND(CASE WHEN A.DC='Dr' THEN IFNULL(A.Opbal,0)+(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='D' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                  " -(SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "') ELSE -IFNULL(A.Opbal,0)- " & _
                  " (SELECT IFNULL(SUM(Amount),0) FROM Ledger WHERE AccountID=A.ID AND DC='C' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "')+(SELECT IFNULL(SUM(Amount),0) FROM Ledger " & _
                  " WHERE AccountID=A.ID AND DC='D' AND EntryDate<='" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "') END,2) AS TotalRestbal FROM Account_AcGrp A WHERE A.GroupID=" & Val(cbAccountName.SelectedValue) & " and TotalRestbal<>0 " & _
                  " ORDER BY UPPER(A.AccountName);"
        End If
        dt = clsFun.ExecDataTable(sql)
        If Val(dt.Rows.Count) > 20 Then dg1.Columns(5).Width = 150
        dg1.Rows.Clear()
        For i = 0 To dt.Rows.Count - 1
            lblRecordCount.Visible = True
            lblRecordCount.Text = "Total Records : " & dt.Rows.Count
            dg1.Rows.Add()
            With dg1.Rows(i)
                .Cells(1).Value = dt.Rows(i)("ID").ToString()
                .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                .Cells(3).Value = dt.Rows(i)("GroupName").ToString()
                .Cells(4).Value = IIf(Val(dt.Rows(i)("TotalOpbal").ToString()) > 0, Format(Math.Abs(Val(dt.Rows(i)("TotalOpbal").ToString())), "0.00") & " " & " Dr", Format(Math.Abs(Val(dt.Rows(i)("TotalOpbal").ToString())), "0.00") & " " & " Cr")
                .Cells(5).Value = Format(Val(dt.Rows(i)("TotalDr").ToString()), "0.00")
                .Cells(6).Value = Format(Val(dt.Rows(i)("TotalCr").ToString()), "0.00")
                .Cells(7).Value = IIf(Val(dt.Rows(i)("TotalRestbal").ToString()) > 0, Format(Math.Abs(Val(dt.Rows(i)("TotalRestbal").ToString())), "0.00") & " Dr", Format(Math.Abs(Val(dt.Rows(i)("TotalRestbal").ToString())), "0.00") & " Cr")
                OpTotal += Val(dt.Rows(i)("TotalOpbal").ToString())
                Drtotal += Val(dt.Rows(i)("TotalDr").ToString())
                CrTotal += Val(dt.Rows(i)("TotalCr").ToString())
                CloseTotal += Val(dt.Rows(i)("TotalRestbal").ToString())
            End With
        Next
        txtOpbal.Text = IIf(Val(CloseTotal) > 0, Format(Math.Abs(Val(OpTotal)), "0.00") & " " & " Dr", Format(Math.Abs(Val(OpTotal)), "0.00") & " " & " Cr")
        txtBalAmt.Text = IIf(Val(CloseTotal) > 0, Format(Math.Abs(Val(CloseTotal)), "0.00") & " " & " Dr", Format(Math.Abs(Val(CloseTotal)), "0.00") & " " & " Cr")
        txtDramt.Text = Format(Math.Abs(Val(Drtotal)), "0.00")
        txtcrAmt.Text = Format(Math.Abs(Val(CrTotal)), "0.00")
        dg1.ClearSelection()
    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        retrive()
        headerCheckBox.Checked = True
        HeaderCheckBox_Clicked(headerCheckBox, EventArgs.Empty)
        'RetriveGroupLedger()
    End Sub
    Private Sub rowColumsTemp()
        tmpgrid.ColumnCount = 16
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
        tmpgrid.Columns(15).Name = "DayCount" : tmpgrid.Columns(15).Width = 100
    End Sub
    Private Sub RetriveLedger()
        Dim i As Integer = 0
        tmpgrid.Rows.Clear()

        Dim sql As String = String.Empty
        Dim dt As DataTable

        ' ProgressBar Settings
        pb1.Visible = True
        pb1.Minimum = 0
        pb1.Maximum = dg1.Rows.Count
        pb1.Value = 0

        For i = 0 To dg1.Rows.Count - 1
            If dg1.Rows(i).Cells("checkBoxColumn").Value = False Then
                Continue For
            End If
            Dim runningBalance As Decimal = 0
            Dim Drtotal As Decimal = 0
            Dim CrTotal As Decimal = 0
            Dim isFirstRow As Boolean = True
            Dim lastRunningBalance As Decimal = 0
            Dim accountId As Integer = Val(dg1.Rows(i).Cells(1).Value)
            Dim RowCount As Integer = 0

            '---------------------------------------------------
            '      SQL QUERY – SAME AS YOUR OLD QUERY
            '---------------------------------------------------
            sql = "SELECT A.AccountName,A.OtherName, " &
                  "ROUND(CASE WHEN A.DC = 'Dr' THEN IFNULL(A.Opbal,0) + " &
                  "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') - " &
                  "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') " &
                  "ELSE -IFNULL(A.Opbal,0) - (SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') + " &
                  "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') END, 2) AS OpeningBalance, " &
                  "L.EntryDate AS EntryDate, L.VourchersID, L.TransType, L.Remark, L.RemarkHindi, L.Narration, " &
                  "CASE WHEN L.DC = 'D' THEN ROUND(L.Amount, 2) ELSE 0 END AS Dr, " &
                  "CASE WHEN L.DC = 'C' THEN ROUND(L.Amount, 2) ELSE 0 END AS Cr " &
                  "FROM Account_AcGrp A LEFT JOIN Ledger L ON L.AccountID = A.ID " &
                  "WHERE A.ID = " & accountId &
                  " AND L.EntryDate BETWEEN '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") &
                  "' AND '" & CDate(txttoDate.Text).ToString("yyyy-MM-dd") & "' " &
                  "ORDER BY L.VourchersID, L.EntryDate"

            dt = clsFun.ExecDataTable(sql)


            '---------------------------------------------------
            '        MAIN LOOP (Row by Row Processing)
            '---------------------------------------------------
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


                '---------------------------------------------------
                '       ⭐ NEW : Calculate DAYS
                '---------------------------------------------------
                Dim entryDate As Date = CDate(row("EntryDate"))
                Dim daysDiff As Integer = DateDiff(DateInterval.Day, CDate(txtFromDate.Text), entryDate)
                ' If daysDiff < 0 Then daysDiff = 0 'No negative days

                '---------------------------------------------------
                '      ADD ROW TO TMPGRID
                '---------------------------------------------------
                tmpgrid.Rows.Add(
                    row("L.VourchersID"),
                    CDate(row("EntryDate")).ToString("dd-MM-yyyy"),
                    row("L.TransType"),
                    row("A.AccountName"),
                    row("L.Remark"),
                    Format(dr, "0.00"),
                    Format(cr, "0.00"),
                    IIf(runningBalance > 0,
                        Format(Math.Abs(runningBalance), "0.00") & " Dr",
                        Format(Math.Abs(runningBalance), "0.00") & " Cr"),
                    row("A.OtherName"),
                    row("L.RemarkHindi"),
                    Drtotal,
                    CrTotal,
                    IIf(openingBalance > 0,
                        Format(Math.Abs(openingBalance), "0.00") & " Dr",
                        Format(Math.Abs(openingBalance), "0.00") & " Cr"),
                    "",
                    RowCount,
                    daysDiff)

                lastRunningBalance = runningBalance

            Next



            '---------------------------------------------------
            '       SET FINAL BALANCE FOR ALL ROWS OF THIS ACCOUNT
            '---------------------------------------------------
            Dim lastRowCount As Integer = dt.Rows.Count + 1
            If lastRowCount > 0 Then
                For j As Integer = tmpgrid.Rows.Count - lastRowCount To tmpgrid.Rows.Count - 1

                    tmpgrid.Rows(j).Cells("CalbalTotal").Value =
                        IIf(lastRunningBalance > 0,
                            Format(lastRunningBalance, "0.00") & " Dr",
                            Format(Math.Abs(lastRunningBalance), "0.00") & " Cr")

                Next
            End If


            ' ProgressBar
            pb1.Value = i + 1
            Application.DoEvents()

        Next

        pb1.Value = 0
        pb1.Visible = False

    End Sub
    Private Sub RetriveLedgerMerged()
        tmpgrid.Rows.Clear()

        Dim sql As String = ""
        Dim dt As DataTable

        pb1.Visible = True
        pb1.Minimum = 0
        pb1.Maximum = dg1.Rows.Count
        pb1.Value = 0

        For i As Integer = 0 To dg1.Rows.Count - 1

            If dg1.Rows(i).Cells("checkBoxColumn").Value = False Then Continue For

            Dim accountId As Integer = Val(dg1.Rows(i).Cells(1).Value)

            '==============================
            ' OPENING BALANCE
            '==============================
            Dim opbal As Decimal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & Format(CDate(txtFromDate.Text), "yyyy-MM-dd") & "')" &
                                    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & Format(CDate(txtFromDate.Text), "yyyy-MM-dd") & "')) " &
                                    " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & Format(CDate(txtFromDate.Text), "yyyy-MM-dd") & "')" &
                                    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & Format(CDate(txtFromDate.Text), "yyyy-MM-dd") & "'))  end),2) from Accounts Where ID=" & accountId))

            '==============================
            ' CLOSING BALANCE
            '==============================
            Dim clbal As Decimal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & Format(CDate(txttoDate.Text), "yyyy-MM-dd") & "')" &
                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & Format(CDate(txttoDate.Text), "yyyy-MM-dd") & "')) " &
                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & Format(CDate(txttoDate.Text), "yyyy-MM-dd") & "')" &
                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & Format(CDate(txttoDate.Text), "yyyy-MM-dd") & "'))  end),2) from Accounts Where ID=" & accountId))

            '==============================
            ' MAIN QUERY
            '==============================
            sql = ""
            sql &= "SELECT EntryDate, TransType, "
            sql &= "SUM(CASE WHEN DC='D' THEN Amount ELSE 0 END) AS Dr, "
            sql &= "SUM(CASE WHEN DC='C' THEN Amount ELSE 0 END) AS Cr "
            sql &= "FROM Ledger "
            sql &= "WHERE AccountID=" & accountId
            sql &= " AND EntryDate BETWEEN '" & Format(CDate(txtFromDate.Text), "yyyy-MM-dd") & "'"
            sql &= " AND '" & Format(CDate(txttoDate.Text), "yyyy-MM-dd") & "' "
            sql &= "GROUP BY EntryDate, TransType "
            sql &= "ORDER BY EntryDate"

            dt = clsFun.ExecDataTable(sql)

            Dim runningBalance As Decimal = opbal
            Dim totalDr As Decimal = 0
            Dim totalCr As Decimal = 0

            '==============================
            ' 🔹 OPENING ROW (Always Show)
            '==============================
            tmpgrid.Rows.Add("", "", "Opening", dg1.Rows(i).Cells(2).Value, "",
                             "", "",
                             Format(Math.Abs(opbal), "0.00") & If(opbal >= 0, " Dr", " Cr"),
                             "", "", "", "", Format(opbal, "0.00"), "", "")

            '==============================
            ' TRANSACTIONS
            '==============================
            For Each r As DataRow In dt.Rows

                Dim entryDate As Date = CDate(r("EntryDate"))
                Dim transType As String = r("TransType").ToString()
                Dim dr As Decimal = Val(r("Dr"))
                Dim cr As Decimal = Val(r("Cr"))

                totalDr += dr
                totalCr += cr

                ' Remark
                Dim remarkSql As String = "SELECT Remark FROM Ledger WHERE AccountID=" & accountId &
                                         " AND EntryDate='" & Format(entryDate, "yyyy-MM-dd") & "'" &
                                         " AND TransType='" & transType.Replace("'", "''") & "'"

                Dim dtRemark As DataTable = clsFun.ExecDataTable(remarkSql)

                Dim finalRemark As String = ""
                For Each rr As DataRow In dtRemark.Rows
                    finalRemark &= rr("Remark").ToString() & vbCrLf
                Next

                runningBalance += dr - cr

                tmpgrid.Rows.Add(
                    "",
                    Format(entryDate, "dd-MM-yyyy"),
                    transType,
                    dg1.Rows(i).Cells(2).Value,
                    finalRemark,
                    If(dr = 0, "", Format(dr, "0.00")),
                    If(cr = 0, "", Format(cr, "0.00")),
                    Format(Math.Abs(runningBalance), "0.00") & If(runningBalance >= 0, " Dr", " Cr"),
                    "",
                    "",
                    Format(totalDr, "0.00"),
                    Format(totalCr, "0.00"),
                    "",
                    "",
                    ""
                )
            Next

            '==============================
            ' 🔹 CLOSING ROW (Always Show)
            '==============================
            tmpgrid.Rows.Add("", "", "Closing", dg1.Rows(i).Cells(2).Value, "",
                             "", "",
                             Format(Math.Abs(clbal), "0.00") & If(clbal >= 0, " Dr", " Cr"),
                             "", "",
                             Format(totalDr, "0.00"),
                             Format(totalCr, "0.00"),
                             "",
                             Format(clbal, "0.00"),
                             "")

            pb1.Value = i + 1
            Application.DoEvents()

        Next

        pb1.Visible = False

    End Sub
    'Private Sub RetriveLedgerMerged()
    '        tmpgrid.Rows.Clear()

    '        Dim sql As String = ""
    '        Dim dt As DataTable

    '        pb1.Visible = True
    '        pb1.Minimum = 0
    '        pb1.Maximum = dg1.Rows.Count
    '        pb1.Value = 0

    '        For i As Integer = 0 To dg1.Rows.Count - 1

    '            If dg1.Rows(i).Cells("checkBoxColumn").Value = False Then Continue For

    '            Dim accountId As Integer = Val(dg1.Rows(i).Cells(1).Value)

    '            '=========================================
    '            ' MAIN QUERY (SUM BY DATE + TRANSTYPE)
    '            '=========================================
    '            opbal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(txtFromDate.Text.ToString("yyyy-MM-dd")) & "')" &
    '                                "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(txtFromDate.Text.ToString("yyyy-MM-dd")) & "')) " &
    '                                " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(txtFromDate.Text.ToString("yyyy-MM-dd")) & "')" &
    '                                " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(txtFromDate.Text.ToString("yyyy-MM-dd")) & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(AcID) & " Order by upper(AccountName) ;"))
    '            Dim ClBal As Decimal = Val(clsFun.ExecScalarStr("Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(txttoDate.Text.ToString("yyyy-MM-dd")) & "')" &
    '                                     "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(txttoDate.Text.ToString("yyyy-MM-dd")) & "')) " &
    '                                     " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(txttoDate.Text.ToString("yyyy-MM-dd")) & "')" &
    '                                     " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(txttoDate.Text.ToString("yyyy-MM-dd")) & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and ID=" & Val(AcID) & " Order by upper(AccountName) ;"))

    '            sql = ""
    '            sql &= "SELECT EntryDate, TransType, "
    '            sql &= "SUM(CASE WHEN DC='D' THEN Amount ELSE 0 END) AS Dr, "
    '            sql &= "SUM(CASE WHEN DC='C' THEN Amount ELSE 0 END) AS Cr "
    '            sql &= "FROM Ledger "
    '            sql &= "WHERE AccountID=" & accountId
    '            sql &= " AND EntryDate BETWEEN '" & Format(CDate(txtFromDate.Text), "yyyy-MM-dd") & "'"
    '            sql &= " AND '" & Format(CDate(txttoDate.Text), "yyyy-MM-dd") & "' "
    '            sql &= "GROUP BY EntryDate, TransType "
    '            sql &= "ORDER BY EntryDate"

    '            dt = clsFun.ExecDataTable(sql)

    '            '=========================================
    '            ' RUNNING BALANCE VARIABLES
    '            '=========================================
    '            Dim runningBalance As Decimal = 0
    '            Dim isFirst As Boolean = True

    '            For Each r As DataRow In dt.Rows

    '                Dim entryDate As Date = CDate(r("EntryDate"))
    '                Dim transType As String = r("TransType").ToString()
    '                Dim dr As Decimal = Val(r("Dr"))
    '                Dim cr As Decimal = Val(r("Cr"))

    '                '=========================================
    '                ' 🔥 REMARK FETCH (SEPARATE QUERY - SAME AS YOUR STYLE)
    '                '=========================================
    '                Dim remarkSql As String = ""
    '                remarkSql &= "SELECT Remark FROM Ledger "
    '                remarkSql &= "WHERE AccountID=" & accountId
    '                remarkSql &= " AND EntryDate='" & Format(entryDate, "yyyy-MM-dd") & "'"
    '                remarkSql &= " AND TransType='" & transType.Replace("'", "''") & "'"

    '                Dim dtRemark As DataTable = clsFun.ExecDataTable(remarkSql)

    '                Dim finalRemark As String = ""

    '                If dtRemark.Rows.Count > 0 Then
    '                    For j As Integer = 0 To dtRemark.Rows.Count - 1
    '                        finalRemark &= dtRemark.Rows(j)("Remark").ToString() & vbCrLf
    '                    Next
    '                End If

    '                '=========================================
    '                ' RUNNING BALANCE
    '                '=========================================
    '                If isFirst Then
    '                    runningBalance = 0
    '                    isFirst = False
    '                End If

    '                runningBalance += dr
    '                runningBalance -= cr

    '                '=========================================
    '                ' ADD ROW
    '                '=========================================
    '                tmpgrid.Rows.Add(
    '                    "",
    '                    Format(entryDate, "dd-MM-yyyy"),
    '                    transType,
    '                    dg1.Rows(i).Cells(2).Value,
    '                    finalRemark,
    '                    IIf(dr = 0, "", Format(dr, "0.00")),
    '                    IIf(cr = 0, "", Format(cr, "0.00")),
    '                    IIf(runningBalance >= 0,
    '                        Format(runningBalance, "0.00") & " Dr",
    '                        Format(Math.Abs(runningBalance), "0.00") & " Cr"),
    '                    "",
    '                    "",
    '                    "",
    '                    "",
    '                    "",
    '                    "",
    '                    ""
    '                )

    '            Next

    '            pb1.Value = i + 1
    '            Application.DoEvents()

    '        Next

    '        pb1.Visible = False

    '    End Sub





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
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & txtFromDate.Text & "'," &
                            "'" & txttoDate.Text & "','" & .Cells("Account Name").Value & "','" & .Cells("OpBalTotal").Value & "','" & .Cells("DrTotal").Value & "','" & .Cells("CrTotal").Value & "'," &
                            "'" & .Cells("CalbalTotal").Value & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "','" & IIf(ckPrintHindi.Checked = True, .Cells("HindiItem").Value, .Cells("Description").Value) & "'," &
                            "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','" & .Cells("CalbalTotal").Value & "',''," & Val(.Cells("RowCount").Value) & ",'" & .Cells("DayCount").Value & "'"
                    End With
                    LastRecord = Val(LastRecord + 1)
                Next
                ' LastRecord = LastCount
                Try
                    If FastQuery = String.Empty Then Exit Sub
                    sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10,P11,P12) " & FastQuery & ""
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
    '              "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') - " & _
    '              "(SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') " & _
    '              "ELSE -IFNULL(A.Opbal,0) - (SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'C' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') " & _
    '              "+ (SELECT IFNULL(SUM(Amount), 0) FROM Ledger L1 WHERE L1.AccountID = A.ID AND L1.DC = 'D' AND L1.EntryDate < '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "') END, 2) AS OpeningBalance, " & _
    '              "L.EntryDate, L.VourchersID, L.TransType, L.Remark,L.RemarkHindi, L.Narration, " & _
    '              "CASE WHEN L.DC = 'D' THEN ROUND(L.Amount, 2) ELSE 0 END AS Dr, " & _
    '              "CASE WHEN L.DC = 'C' THEN ROUND(L.Amount, 2) ELSE 0 END AS Cr " & _
    '              "FROM Account_AcGrp A LEFT JOIN Ledger L ON L.AccountID = A.ID " & _
    '              "WHERE A.ID = " & accountId & "  AND L.EntryDate BETWEEN '" & CDate(txtFromDate.Text).ToString("yyyy-MM-dd") & "' AND '" & CDate(txtToDate.Text).ToString("yyyy-MM-dd") & "' " & _
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
        txttoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles Dtp2.ValueChanged
        txttoDate.Text = Dtp2.Value.ToString("dd-MM-yyyy")
        txttoDate.Text = SmartDate(txttoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        txtFromDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        txtFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        txtFromDate.Text = SmartDate(txtFromDate.Text)
    End Sub

    Private Sub txtFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtFromDate.Validating
        txtFromDate.Text = SmartDate(txtFromDate.Text)
    End Sub

    Private Sub txtToDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txttoDate.Validating
        txttoDate.Text = SmartDate(txttoDate.Text, True, 2)
    End Sub

    Private Sub btnLedgerMerged_Click(sender As Object, e As EventArgs) Handles btnLedgerMerged.Click
        pnlWait.Visible = True : pnlPrint.Visible = False : rowColumsTemp() : RetriveLedgerMerged() : PrintRecord()
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
    Private Sub PrintOutstanding()
        Dim sql As String = ""
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        If dg1.Rows.Count = 0 Then Exit Sub
        For Each row As DataGridViewRow In dg1.Rows
            If row.IsNewRow Then Continue For
            Application.DoEvents()
            With row
                Try
                    sql = "insert into Printing(D1,P1, P2,P3, P4,P5,P6,P7,P8,P9,M10) values('" & txttoDate.Text & "'," & _
                    "'" & .Cells(2).Value & "','" & .Cells(3).Value & "',''," & _
                    "'" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & .Cells(4).Value & "'," & _
                    "'" & Format(Val(txtDramt.Text), "0.00") & "','" & Format(Val(txtcrAmt.Text), "0.00") & "','" & Format(Val(txtBalAmt.Text), "0.00") & "','" & marka & "')"
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub
    Private Sub btnPrintOutstanding_Click(sender As Object, e As EventArgs) Handles btnPrintOutstanding.Click
        PrintOutstanding()
        If ckPrintHindi.Checked = True Then
            Report_Viewer.printReport("\OutstandingHindi.rpt")
        Else
            Report_Viewer.printReport("\Outstanding.rpt")
        End If
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        Report_Viewer.BringToFront()
    End Sub

    Private Sub dg1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Space Then
            'If dg1.CurrentCell.ColumnIndex = 0 Then
            Dim chk As DataGridViewCheckBoxCell = dg1.CurrentRow.Cells(0)
            chk.Value = Not Convert.ToBoolean(chk.Value)
            e.Handled = True
            'End If
        End If
    End Sub
End Class
