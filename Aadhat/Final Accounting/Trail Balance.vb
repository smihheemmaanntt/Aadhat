Public Class Trial_Balance

    Private Sub Trail_Balance_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Trail_Balance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.KeyPreview = True
        RadioClosing.Checked = True
        rowColums()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown
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
    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 4
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name"
        dg1.Columns(1).Width = 600
        dg1.Columns(2).Name = "Debit"
        dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Credit"
        dg1.Columns(3).Width = 300
        ' retrive()
    End Sub
    Private Sub ClosingTrial()
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        ' Dim i, j As Integer
        Dim amt As Decimal = 0 : Dim opbaltot As Decimal = 0.0
        Dim drtot As Decimal = 0.0 : Dim crtot As Decimal = 0.0
        Dim lastval As Integer = 0 : Dim sql As String = String.Empty
        Dim tmpval As Integer = 0
        dg1.Rows.Clear()
        sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " &
        "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
        "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
        " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
        " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0  Order by UPPER(AccountName) ;"
        dt = clsFun.ExecDataTable(sql)
        dg1.Rows.Clear()
        For i = 0 To dt.Rows.Count - 1
            dg1.Rows.Add()
            With dg1.Rows(i)
                .Cells(0).Value = dt.Rows(i)("ID").ToString()
                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                If Val(dt.Rows(i)("Restbal").ToString()) > 0 Then
                    .Cells(2).Value = Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00")
                    drtot = drtot + .Cells(2).Value
                Else
                    .Cells(3).Value = Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00")
                    crtot = crtot + .Cells(3).Value
                End If
                tmpval = tmpval + 1
            End With
        Next
        If dg1.RowCount = 0 Then calc() : Exit Sub
        If crtot > drtot Then
            dg1.Rows.Add()
            dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
            dg1.Rows(tmpval).Cells(2).Value = Format(Val(crtot - drtot), "0.00")
        ElseIf crtot < drtot Then
            dg1.Rows.Add()
            dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
            dg1.Rows(tmpval).Cells(3).Value = Format(Val(drtot - crtot), "0.00")
        End If
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub OpeningTrial()
        dg1.Rows.Clear()
        Dim opbaltot As Decimal = 0.0
        Dim drtot As Decimal = 0.0
        Dim crtot As Decimal = 0.0
        Dim lastval As Integer = 0
        Dim tmpval As Integer = 0
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Accounts ac inner join AccountGroup grp on ac.GroupID=grp.ID Where ifnull(OPbal,0)<>0 and  opbal<>''   order by UPPER(AccountName)")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Readonly = True : .Cells(1).Readonly = True
                        .Cells(2).Readonly = True
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        If dt.Rows(i)("DC").ToString() = "Dr" Then
                            .Cells(2).Value = Format(Val(dt.Rows(i)("OpBal").ToString()), "0.00")
                            drtot = drtot + .Cells(2).Value
                        Else
                            .Cells(3).Value = Format(Val(dt.Rows(i)("OpBal").ToString()), "0.00")
                            crtot = crtot + .Cells(3).Value
                        End If
                        tmpval = tmpval + 1
                    End With
                Next
                If dg1.RowCount = 0 Then calc() : Exit Sub
                If crtot > drtot Then
                    dg1.Rows.Add()
                    dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
                    dg1.Rows(tmpval).Cells(2).Value = Format(Val(crtot - drtot), "0.00")
                ElseIf crtot < drtot Then
                    dg1.Rows.Add()
                    dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
                    dg1.Rows(tmpval).Cells(3).Value = Format(Val(drtot - crtot), "0.00")
                End If
                calc() : dg1.ClearSelection()
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.Rows(0).Cells(3).Selected = True : calc()

    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If RadioClosing.Checked = True Then
            ClosingTrial()
        Else
            OpeningTrial()
        End If

    End Sub
    'Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
    '    Dim ssql As String = String.Empty
    '    Dim dt As New DataTable
    '    Dim i, j As Integer
    '    Dim amt As Decimal = 0
    '    Dim opbaltot As Decimal = 0.0
    '    Dim drtot As Decimal = 0.0
    '    Dim crtot As Decimal = 0.0
    '    Dim lastval As Integer = 0
    '    'ssql = "Select ac.AccountName as AccountName,GroupID, Ledger.AccountID as AccountId,Mobile1,City FROM Ledger INNER JOIN Account_acgrp ac ON Ledger.AccountID = ac.ID WHERE " & _
    '    '    "EntryDate<= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'"
    '    ssql = "Select * From Accounts ORDER BY AccountName"
    '    dt = clsFun.ExecDataTable(ssql)
    '    dg1.Rows.Clear()
    '    If dt.Rows.Count > 0 Then
    '        For i = 0 To dt.Rows.Count - 1
    '            Dim opbal As String = "" : Dim ClBal As String = ""
    '            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(i)("ID").ToString()) & "")
    '            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
    '            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(i)("ID").ToString()) & "")
    '            If drcr = "Dr" Then
    '                tmpamtdr = Val(opbal) + Val(tmpamtdr)
    '            Else
    '                tmpamtcr = Val(opbal) + Val(tmpamtcr)
    '            End If
    '            Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr) '- Val(opbal)
    '            If tmpamt > 0 Then
    '                dg1.Rows.Add()
    '                With dg1.Rows(tmpval)
    '                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
    '                    .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
    '                    .Cells(4).Value = Format(Math.Abs(Val(tmpamt)), "0.00")
    '                    drtot = drtot + .Cells(4).Value
    '                    tmpval = tmpval + 1
    '                End With
    '            ElseIf tmpamt < 0 Then
    '                dg1.Rows.Add()
    '                With dg1.Rows(tmpval)
    '                    .Cells(0).Value = dt.Rows(i)("ID").ToString()
    '                    .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
    '                    .Cells(5).Value = Format(Math.Abs(Val(tmpamt)), "0.00")
    '                    crtot = crtot + .Cells(5).Value
    '                    tmpval = tmpval + 1
    '                End With
    '            End If
    '        Next
    '    End If


    '    If dg1.RowCount = 0 Then calc() : Exit Sub
    '    If crtot > drtot Then
    '        dg1.Rows.Add()
    '        dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
    '        dg1.Rows(tmpval).Cells(4).Value = Format(Val(crtot - drtot), "0.00")
    '    ElseIf crtot < drtot Then
    '        dg1.Rows.Add()
    '        dg1.Rows(tmpval).Cells(1).Value = "Diffrence in Balance"
    '        dg1.Rows(tmpval).Cells(5).Value = Format(Val(drtot - crtot), "0.00")
    '    End If
    '    calc() : dg1.ClearSelection()
    'End Sub
    Private Sub calc()
        txtDramt.Text = Format(0, "0.00") : txtcrAmt.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txtDramt.Text = Format(Val(txtDramt.Text) + Val(dg1.Rows(i).Cells(2).Value), "0.00")
            txtcrAmt.Text = Format(Val(txtcrAmt.Text) + Val(dg1.Rows(i).Cells(3).Value), "0.00")
        Next
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,P1, P2,P3, P4,P5) values('" & mskEntryDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & IIf(.Cells(2).Value = "", "", Format(Val(.Cells(2).Value), "0.00")) & "', " & _
                    "'" & IIf(.Cells(3).Value = "", "", Format(Val(.Cells(3).Value), "0.00")) & "','" & Format(Val(txtDramt.Text), "0.00") & "','" & Format(Val(txtcrAmt.Text), "0.00") & "');"
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
        Report_Viewer.printReport("\Reports\TrailBalance.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
End Class