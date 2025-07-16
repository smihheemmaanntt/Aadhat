Imports FirebirdSql.Data.FirebirdClient
Public Class Cash_Book_Grouped
    Private opbal As Decimal = 0.0
    Private Sub Cash_Bank_Book_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, cbAccountName.KeyDown
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
        clsFun.FillDropDownList(cbAccountName, "Select * From Accounts where groupid in(11,12)", "AccountName", "Id", "")
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mskFromDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyyy"))
        MsktoDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyyy"))
        rowColums() : cbAccountName.Focus() : Me.KeyPreview = True
    End Sub

    Private Sub cbAccountName_Leave(sender As Object, e As EventArgs) Handles cbAccountName.Leave
        If clsFun.ExecScalarInt("Select count(*)from Accounts") = 0 Then
            Exit Sub
        End If
        If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & cbAccountName.Text & "'") = 0 Then
            MsgBox("Account Name Not Found in Database...", vbOKOnly, "Access Denied")
            cbAccountName.Focus()
            Exit Sub
        End If
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
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
        pbWait.Visible = True
        RetriveOld()
        pbWait.Visible = False
        ' Retrive()
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub RetriveOld()
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        Dim drtotal As Double = 0
        Dim crtotal As Double = 0
        Dim drtotal1 As Double = 0
        Dim crtotal1 As Double = 0
        Dim closingBal As Double = 0
        Dim grndTotal As Double = 0
        Dim j As Integer
        Dim tmpdate As String = String.Empty
        Dim tmpdate1 As String = String.Empty
        Dim lastval As Integer = 0
        Dim tmpopbaladd As Boolean = False
        Dim tmpDt As New DataTable
        Try


            ssql = "Select Entrydate from Ledger where DC ='D' " & IIf(cbAccountName.SelectedValue > 0, "and AccountID=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'   union " &
                " Select Entrydate from Ledger where Dc='C' " & IIf(cbAccountName.SelectedValue > 0, "and AccountID=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' order by Entrydate "
            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID= " & cbAccountName.SelectedValue & "")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & cbAccountName.SelectedValue & "")

            dt = clsFun.ExecDataTable(ssql)
            If dt.Rows.Count > 20 Then dg1.Columns(5).Width = 280 Else dg1.Columns(5).Width = 299
            dg1.Rows.Clear()
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    dg1.Rows(lastval).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    dg1.Rows(lastval).Cells(1).Value = "Date : " & CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                    dg1.Rows(lastval).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    dg1.Rows(lastval).Cells(5).Value = "Date : " & CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                    dg1.Rows(lastval).Cells(4).Value = "|"
                    lastval = lastval + 1
                    dg1.Rows.Add()
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(cbAccountName.SelectedValue) & " and EntryDate < '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(cbAccountName.SelectedValue) & " and EntryDate < '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'")
                    ''Dim tmpamt As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where accountID=" & Val(cbAccountName.SelectedValue) & " and EntryDate < '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If
                    Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr))

                    If drcr = "Cr" Then
                        opbal = Math.Round(Math.Abs(Val(tmpamt)), 2)
                    Else
                        opbal = Math.Round(Math.Abs(Val(tmpamt)), 2)
                    End If
                    '************For Opening Balance"***************************************************************************
                    If i = 0 Then
                        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from LEdger where  accountID=" & Val(cbAccountName.SelectedValue) & " and  EntryDate < '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'")
                        If cnt = 0 Then
                            If drcr = "Dr" Then
                                dg1.Rows(lastval).Cells(1).Style.Alignment = DataGridViewContentAlignment.BottomLeft
                                dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(1).Value = "Opening Balance"
                                dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(3).Value = Format(Val(opbal), "0.00")
                            Else
                                dg1.Rows(lastval).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                dg1.Rows(lastval).Cells(5).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(5).Value = "Opening Balance"
                                dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                dg1.Rows(lastval).Cells(7).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(7).Value = Format(Val(Val(opbal)), "0.00")
                            End If

                        Else
                            If Val(tmpamtcr) > Val(tmpamtdr) Then
                                dg1.Rows(lastval).Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                dg1.Rows(lastval).Cells(5).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(5).Value = "Opening Balance"
                                dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                dg1.Rows(lastval).Cells(7).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(7).Value = Format(Val(Val(opbal)), "0.00")
                            Else
                                dg1.Rows(lastval).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(1).Value = "Opening Balance"
                                dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                                dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.Blue
                                dg1.Rows(lastval).Cells(3).Value = Format(Val(Val(opbal)), "0.00")
                            End If

                        End If
                        'If clsFun.ExecScalarStr("Select Dc FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'").StartsWith("D") = True Then
                        '    dg1.Rows(lastval).Cells(1).Value = "Opening Balance"
                        '    dg1.Rows(lastval).Cells(3).Value = Val(tmpamt)
                        'Else
                        '    dg1.Rows(lastval).Cells(5).Value = "Opening Balance"
                        '    dg1.Rows(lastval).Cells(7).Value = Val(tmpamt)
                        'End If

                    Else
                        crtotal1 = Val(0) : drtotal1 = Val(0)
                        If drtotal < crtotal Then
                            closingBal = Math.Abs(crtotal) - Math.Abs(drtotal)
                            crtotal1 = Val(closingBal)
                            dg1.Rows(lastval).Cells(6).Style.ForeColor = Color.Blue
                            dg1.Rows(lastval).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            dg1.Rows(lastval).Cells(6).Value = "Opening Balance"
                            dg1.Rows(lastval).Cells(6).Style.ForeColor = Color.Blue
                            dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                            dg1.Rows(lastval).Cells(7).Value = Format(Val(closingBal), "0.00")

                        Else
                            closingBal = Math.Abs(drtotal) - Math.Abs(crtotal)
                            drtotal1 = Val(closingBal)
                            dg1.Rows(lastval).Cells(1).Style.ForeColor = Color.Blue
                            dg1.Rows(lastval).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                            dg1.Rows(lastval).Cells(1).Value = "Opening Balance"
                            dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.Blue
                            dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                            dg1.Rows(lastval).Cells(3).Value = Format(Val(closingBal), "0.00")

                        End If
                    End If
                    dg1.Rows(lastval).Cells(4).Value = "|"
                    lastval = lastval + 1
                    crtotal = 0 : drtotal = 0
                    '**********************************************************************************************************

                    '****************************Grid Fill Based on Dates********************************************************
                    ssql = "Select Entrydate, TransType,AccountName,Remark,sum(Amount) as Dr,'0' as Cr,Narration from Ledger where DC ='D' " & IIf(cbAccountName.SelectedValue > 0, "and AccountID=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate = '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'  Group by Transtype   union all" &
                " Select Entrydate,  TransType,AccountName,Remark,'0' as Dr,sum(Amount) as Cr ,Narration from Ledger where Dc='C' " & IIf(cbAccountName.SelectedValue > 0, "and AccountID=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate = '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'  Group by Transtype   "
                    tmpDt = clsFun.ExecDataTable(ssql)
                    If lastval > 20 Then dg1.Columns(4).Width = 30 Else dg1.Columns(4).Width = 50
                    For j = 0 To tmpDt.Rows.Count - 1
                        dg1.Rows.Add()
                        With dg1.Rows(lastval)
                            If tmpDt.Rows(j)("Dr").ToString() <> "0" Then
                                dg1.Rows(lastval).Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                                .Cells(1).Value = tmpDt.Rows(j)("TransType").ToString() & " All"
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
                                .Cells(5).Value = tmpDt.Rows(j)("TransType").ToString() & " All"
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
                        'If clsFun.ExecScalarInt("Select count(*) from Ledger where Dc='C' " & IIf(cbAccountName.SelectedValue > 0, "and AccountID=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate = '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'") = 0 Then
                        '    crtotal = Val(crtotal1)
                        'End If
                        'If clsFun.ExecScalarInt("Select count(*) from Ledger where Dc='D' " & IIf(cbAccountName.SelectedValue > 0, "and AccountID=" & Val(cbAccountName.SelectedValue) & "", "") & " and EntryDate = '" & CDate(dt.Rows(i)("Entrydate")).ToString("yyyy-MM-dd") & "'") = 0 Then
                        '    drtotal = Val(drtotal1)

                        'End If
                    Next
                    '******************************************************************************************************************
                    If i = 0 Then
                        If Val(tmpamtcr) > Val(tmpamtdr) Then
                            crtotal = Val(crtotal) + Val(opbal)

                        Else
                            drtotal = Val(drtotal) + Val(opbal)
                        End If
                    Else
                        drtotal = Val(drtotal1) + Val(drtotal)
                        crtotal = Val(crtotal1) + Val(crtotal)
                    End If

                    '*************************************************Total******************************************************************
                    dg1.Rows.Add()
                    dg1.Rows(lastval).Cells(2).Style.ForeColor = Color.DeepPink
                    dg1.Rows(lastval).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    dg1.Rows(lastval).Cells(2).Value = "Total"
                    dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.DeepPink
                    dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    dg1.Rows(lastval).Cells(3).Value = Format(Val(IIf(drtotal = 0, Val(drtotal1), drtotal)), "0.00")
                    dg1.Rows(lastval).Cells(6).Style.ForeColor = Color.DeepPink
                    dg1.Rows(lastval).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    dg1.Rows(lastval).Cells(6).Value = "Total"
                    dg1.Rows(lastval).Cells(7).Style.ForeColor = Color.DeepPink
                    dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    dg1.Rows(lastval).Cells(7).Value = Format(Val(crtotal), "0.00")
                    dg1.Rows(lastval).Cells(4).Value = "|"
                    lastval = lastval + 1

                    dg1.Rows.Add()
                    If drtotal < crtotal Then
                        closingBal = Math.Abs(crtotal) - Math.Abs(drtotal)
                        dg1.Rows(lastval).Cells(2).Style.ForeColor = Color.HotPink
                        dg1.Rows(lastval).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                        dg1.Rows(lastval).Cells(2).Value = "Closing Balance"
                        dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.HotPink
                        dg1.Rows(lastval).Cells(3).Value = Format(Val(closingBal), "0.00")
                        grndTotal = closingBal + Math.Abs(drtotal) '' + (Math.Abs(crtotal) - Math.Abs(drtotal))
                    Else
                        closingBal = Math.Abs(drtotal) - Math.Abs(crtotal)
                        dg1.Rows(lastval).Cells(6).Style.ForeColor = Color.Red
                        dg1.Rows(lastval).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                        dg1.Rows(lastval).Cells(6).Value = "Closing Balance"
                        dg1.Rows(lastval).Cells(7).Style.ForeColor = Color.Red
                        dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        dg1.Rows(lastval).Cells(7).Value = Format(Math.Round(Val(closingBal)), "0.00")
                        grndTotal = closingBal + Math.Abs(crtotal) ' Math.Abs(drtotal) - Math.Abs(crtotal)
                    End If
                    dg1.Rows(lastval).Cells(4).Value = "|"
                    '***********************************************************************************************************'
                    lastval = lastval + 1

                    dg1.Rows.Add()
                    dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    dg1.Rows(lastval).Cells(3).Value = "-----------"
                    dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    dg1.Rows(lastval).Cells(7).Value = "-----------"
                    dg1.Rows(lastval).Cells(4).Value = "|"
                    lastval = lastval + 1
                    dg1.Rows.Add()
                    dg1.Rows(lastval).Cells(2).Style.ForeColor = Color.Green
                    dg1.Rows(lastval).Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    dg1.Rows(lastval).Cells(2).Value = "Grand Total"
                    dg1.Rows(lastval).Cells(3).Style.ForeColor = Color.Green
                    dg1.Rows(lastval).Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    dg1.Rows(lastval).Cells(3).Value = Format(Val(grndTotal), "0.00")
                    dg1.Rows(lastval).Cells(6).Style.ForeColor = Color.Green
                    dg1.Rows(lastval).Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                    dg1.Rows(lastval).Cells(6).Value = "Grand Total"
                    dg1.Rows(lastval).Cells(7).Style.ForeColor = Color.Green
                    dg1.Rows(lastval).Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                    dg1.Rows(lastval).Cells(7).Value = Format(Val(grndTotal), "0.00")
                    dg1.Rows(lastval).Cells(4).Value = "|"
                    '************************************************************************************************************
                    '*****************For GridLines*****************************************************************************
                    lastval = lastval + 1
                    dg1.Rows.Add()
                    For j = 0 To 7
                        If j <> 4 Then
                            dg1.Rows(lastval).Cells(j).Value = "------------------"
                        Else
                            dg1.Rows(lastval).Cells(4).Value = "|"
                        End If
                    Next
                    lastval = lastval + 1
                    '*******************************************************************************************************
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        dg1.ClearSelection()
    End Sub

    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,M1, P1, P2,P3, P4, P5, P6,P7) values('" & mskFromDate.Text & "'," &
                    "'" & MsktoDate.Text & "','" & cbAccountName.Text & "'," &
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "'," &
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "');"

            End With
        Next
        Try
            Dim conn As New SQLite.SQLiteConnection(ClsFunPrimary.GetConnection()) : conn.Open()
            cmd = New SQLite.SQLiteCommand(sql, conn)
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
End Class