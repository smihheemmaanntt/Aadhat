Public Class Trading_Account

    Private Sub Trading_Account_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Trading_Account_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.KeyPreview = True
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
        dg1.ColumnCount = 5
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "PURCHASE"
        dg1.Columns(1).Width = 400
        dg1.Columns(2).Name = "AMOUNT"
        dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "SALE"
        dg1.Columns(3).Width = 400
        dg1.Columns(4).Name = "AMOUNT"
        dg1.Columns(4).Width = 200
        ' retrive()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        Dim GPGL As Decimal = 0
        Dim dtTotalSale As New DataTable : Dim dtTotalExp As New DataTable
        Dim dtTotalIncome As New DataTable : Dim dtTotalPurchase As New DataTable
        Dim TotalOpBal As Decimal = 0.0 : Dim TotalPurchase As Decimal = 0.0
        Dim TotalSale As Decimal = 0.0 : Dim TotalExp As Decimal = 0.0
        Dim TotalIncome As Decimal = 0.0 : Dim TotalClosing As Decimal = 0.0
        Dim lastval As Integer = 10 : Dim crtotal As Decimal = 0.0
        Dim drtotal As Decimal = 0.0

        dtTotalPurchase = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(22)")
        For i As Integer = 0 To dtTotalPurchase.Rows.Count - 1
            Dim opbal As String = "" : Dim ClBal As String = ""
            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & "")
            If drcr = "Dr" Then
                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                TotalOpBal = Val(clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & ""))
            Else
                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                TotalOpBal = -Val(clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & ""))
            End If
            Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr)
            TotalPurchase = TotalPurchase + Math.Abs(Val(tmpamt))
        Next

        dtTotalSale = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(23)")
        For i As Integer = 0 To dtTotalSale.Rows.Count - 1
            Dim opbal As String = "" : Dim ClBal As String = ""
            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtTotalSale.Rows(i)("ID").ToString()) & "")
            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtTotalSale.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtTotalSale.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtTotalSale.Rows(i)("ID").ToString()) & "")
            If drcr = "Dr" Then
                tmpamtdr = Val(opbal) + Val(tmpamtdr)
            Else
                tmpamtcr = Val(opbal) + Val(tmpamtcr)
            End If
            Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr)
            TotalSale = TotalSale + Math.Abs(Val(tmpamt))
        Next
        dtTotalExp = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(24)")
        For i As Integer = 0 To dtTotalExp.Rows.Count - 1
            Dim opbal As String = "" : Dim ClBal As String = ""
            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtTotalExp.Rows(i)("ID").ToString()) & "")
            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtTotalExp.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtTotalExp.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtTotalExp.Rows(i)("ID").ToString()) & "")
            If drcr = "Dr" Then
                tmpamtdr = Val(opbal) + Val(tmpamtdr)
            Else
                tmpamtcr = Val(opbal) + Val(tmpamtcr)
            End If
            Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr)
            TotalExp = TotalExp + Math.Abs(Val(tmpamt))
        Next


        dtTotalIncome = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(26)")
        For i As Integer = 0 To dtTotalIncome.Rows.Count - 1
            Dim opbal As String = "" : Dim ClBal As String = ""
            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtTotalIncome.Rows(i)("ID").ToString()) & "")
            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtTotalIncome.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtTotalIncome.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtTotalIncome.Rows(i)("ID").ToString()) & "")
            If drcr = "Dr" Then
                tmpamtdr = Val(opbal) + Val(tmpamtdr)
            Else
                tmpamtcr = Val(opbal) + Val(tmpamtcr)
            End If
            Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr)
            TotalIncome = TotalIncome + Math.Abs(Val(tmpamt))
        Next
        dg1.Rows.Clear()
        dg1.Rows.Add()
        drtotal = TotalPurchase + TotalExp
        crtotal = TotalSale + TotalIncome
        GPGL = (TotalPurchase + TotalExp) - (TotalSale + TotalIncome)
        For i As Integer = 0 To lastval - 1
            With dg1.Rows(i)
                If i = 0 Then
                    .Cells(1).Value = "Opening Stock"
                    .Cells(2).Value = Format(Val(TotalOpBal), "0.00")
                ElseIf i = 1 Then
                    .Cells(1).Value = "Total Purchases"
                    .Cells(2).Value = Format(Val(TotalPurchase), "0.00")
                    .Cells(3).Value = "Total Sales"
                    .Cells(4).Value = Format(Val(TotalSale), "0.00")
                ElseIf i = 2 Then
                    .Cells(1).Value = "Direct Expenses"
                    .Cells(2).Value = Format(Val(TotalExp), "0.00")
                    .Cells(3).Value = "Direct Income"
                    .Cells(4).Value = Format(Val(TotalIncome), "0.00")
                ElseIf i = 3 Then
                    If GPGL < 0 Then
                        drtotal = drtotal + Math.Abs(GPGL)
                        .Cells(1).Value = "Gross Profit(B/F)"
                        .Cells(2).Value = Format(Val(Math.Abs(GPGL)), "0.00")
                        .Cells(2).Style.BackColor = Color.Green
                        .Cells(2).Style.ForeColor = Color.GhostWhite
                        .Cells(1).Style.BackColor = Color.Green
                        .Cells(1).Style.ForeColor = Color.GhostWhite
                    Else
                        .Cells(3).Value = "Gross Loss(B/F)"
                        .Cells(4).Value = Format(Val(Math.Abs(GPGL)), "0.00")
                        crtotal = crtotal + Math.Abs(GPGL)
                        .Cells(3).Style.BackColor = Color.Red
                        .Cells(3).Style.ForeColor = Color.GhostWhite
                        .Cells(4).Style.BackColor = Color.Red
                        .Cells(4).Style.ForeColor = Color.GhostWhite
                    End If
                ElseIf i = 4 Then
                    .Cells(1).Value = "Total"
                    .Cells(2).Value = Format(Val(drtotal), "0.00")
                    .Cells(3).Value = "Total"
                    .Cells(4).Value = Format(Val(crtotal), "0.00")
                    .Cells(2).Style.ForeColor = Color.Blue
                    .Cells(4).Style.ForeColor = Color.Blue
                    .Cells(1).Style.ForeColor = Color.Blue
                    .Cells(3).Style.ForeColor = Color.Blue
                End If
            End With
            dg1.Rows.Add()
        Next i

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
                sql = sql & "insert into Printing(D1,P1, P2,P3, P4) values('" & mskEntryDate.Text & "'," & _
              "'" & .Cells(1).Value & "','" & Format(Val(.Cells(2).Value), "0.00") & "', " & _
              "'" & .Cells(3).Value & "','" & Format(Val(.Cells(4).Value), "0.00") & "');"

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
        Report_Viewer.printReport("\Reports\TradingAccount.rpt")
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