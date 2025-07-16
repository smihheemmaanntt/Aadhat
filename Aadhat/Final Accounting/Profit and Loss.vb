Public Class Profit_and_Loss
    Dim tmpdt As New DataTable
    Private Sub Profit_and_Loss_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Profit_and_Loss_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        dg1.Columns(1).Name = "EXPENDITURE"
        dg1.Columns(1).Width = 400
        dg1.Columns(2).Name = "AMOUNT"
        dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "INCOME"
        dg1.Columns(3).Width = 400
        dg1.Columns(4).Name = "AMOUNT"
        dg1.Columns(4).Width = 200
        ' retrive()
        TmpColumns()
    End Sub
    Private Sub TmpColumns()
        tmpdt.Columns.Add("IDS")
        tmpdt.Columns.Add("EXPENDITURE")
        tmpdt.Columns.Add("EXAMOUNT")
        tmpdt.Columns.Add("INCOME")
        tmpdt.Columns.Add("INAMOUNT")
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Dim ssql As String = String.Empty : Dim dtloss As New DataTable
        Dim dtprofit As New DataTable : Dim GPGL As Decimal = 0
        Dim TotalOpBal As Decimal = 0.0 : Dim TotalClosing As Decimal = 0.0
        Dim dtTotalSale As New DataTable : Dim dtTotalExp As New DataTable
        Dim dtTotalIncome As New DataTable : Dim dtTotalPurchase As New DataTable
        Dim lastval As Integer = 0 : Dim crtotal As Decimal = 0.0
        Dim drtotal As Decimal = 0.0 : Dim netloss As Decimal = 0.0
        Dim netprofit As Decimal = 0.0
        Dim TotalPurchase As Decimal = 0.0 : Dim TotalSale As Decimal = 0.0
        Dim TotalExp As Decimal = 0.0 : Dim TotalIncome As Decimal = 0.0
        dtTotalPurchase = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(22)")
        For i As Integer = 0 To dtTotalPurchase.Rows.Count - 1
            Dim opbal As String = "" : Dim ClBal As String = ""
            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & "")
            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtTotalPurchase.Rows(i)("ID").ToString()) & "")
            If drcr = "Dr" Then
                tmpamtdr = Val(opbal) + Val(tmpamtdr)
            Else
                tmpamtcr = Val(opbal) + Val(tmpamtcr)
            End If
            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
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
            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
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
            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
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
            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
            TotalIncome = TotalIncome + Math.Abs(Val(tmpamt))
        Next



        TotalOpBal = 0.0
        TotalOpBal = 0.0
        dg1.Rows.Clear()
        dg1.Rows.Add()
        drtotal = 0.0
        crtotal = 0.0
        GPGL = (TotalPurchase + TotalExp) - (TotalSale + TotalIncome)
        tmpdt.Rows.Clear()
        tmpdt.Rows.Add()
        dtloss = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(25) or UnderGroupID in(25)")
        dtprofit = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(27) or UnderGroupID in(27)")
        For i As Integer = 0 To IIf(dtloss.Rows.Count > dtprofit.Rows.Count, dtloss.Rows.Count - 1, dtprofit.Rows.Count - 1)
            tmpdt.Rows.Add()
            With tmpdt
                If dtloss.Rows.Count - 1 >= i Then
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtloss.Rows(i)("ID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtloss.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtloss.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtloss.Rows(i)("ID").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If
                    tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt <> 0 Then
                        .Rows(i)(0) = dtloss.Rows(i)("ID")
                        .Rows(i)(1) = dtloss.Rows(i)("Accountname")
                        .Rows(i)(2) = Val(tmpamt)
                    End If
                End If
                If dtprofit.Rows.Count - 1 >= i Then
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtprofit.Rows(i)("ID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtprofit.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtprofit.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtprofit.Rows(i)("id").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If

                    tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt <> 0 Then
                        .Rows(i)(0) = dtprofit.Rows(i)("ID")
                        .Rows(i)(3) = dtprofit.Rows(i)("Accountname")
                        .Rows(i)(4) = Val(tmpamt)
                    End If
                End If
            End With

        Next i
        dg1.Rows.Clear()
        dg1.Rows.Add()
        With dg1.Rows(0)
            lastval = lastval + 1
            If GPGL > 0 Then
                .Cells(1).Value = "Gross Loss(B/F)"""
                .Cells(2).Value = Math.Abs(GPGL)
                netloss = Math.Abs(GPGL)
            Else
                .Cells(3).Value = "Gross Profit(B/F)"
                .Cells(4).Value = Math.Abs(GPGL)
                netprofit = Math.Abs(GPGL)
            End If
        End With
        Dim cnt As Integer = 1
        Dim jk As Integer = 1

        For j = 0 To tmpdt.Rows.Count - 1

            If tmpdt.Rows(j)("IDS").tostring() <> "" Then
                dg1.Rows.Add()
                With dg1.Rows(jk)
                    If Val(tmpdt.Rows(j)("EXAMOUNT").tostring()) > 0 And Val(tmpdt.Rows(j)("EXAMOUNT").tostring()) <> 0 Then
                        .Cells(0).Value = tmpdt.Rows(j)("IDS").tostring()
                        .Cells(1).Value = tmpdt.Rows(j)("EXPENDITURE").tostring()
                        .Cells(2).Value = Format(Math.Abs(Math.Abs(Val(tmpdt.Rows(j)("EXAMOUNT").tostring()))), "0.00")
                        netloss = Val(netloss) + Val(.Cells(2).Value)
                        drtotal = drtotal + Val(.Cells(2).Value)
                    ElseIf Val(tmpdt.Rows(j)("EXAMOUNT").tostring()) < 0 And Val(tmpdt.Rows(j)("EXAMOUNT").tostring()) <> 0 Then
                        .Cells(0).Value = tmpdt.Rows(j)("IDS").tostring()
                        .Cells(3).Value = tmpdt.Rows(j)("EXPENDITURE").tostring()
                        .Cells(4).Value = Format(Math.Abs(Math.Abs(Val(tmpdt.Rows(j)("EXAMOUNT").tostring()))), "0.00")
                        netprofit = Val(netprofit) + Val(.Cells(4).Value)
                        crtotal = crtotal + Val(.Cells(4).Value)
                    End If
                    If tmpdt.Rows(j)("IDS").tostring() = "" Then
                        If Val(tmpdt.Rows(j)("INAMOUNT").tostring()) > 0 Then
                            dg1.Rows.Add()
                            With dg1.Rows(j + 1)
                                .Cells(0).Value = tmpdt.Rows(j)("IDS").tostring()
                                .Cells(3).Value = tmpdt.Rows(j)("INCOME").tostring()
                                .Cells(4).Value = Format(Math.Abs(Math.Abs(Val(tmpdt.Rows(j)("INAMOUNT").tostring()))), "0.00")
                                netprofit = Val(netprofit) + Val(.Cells(4).Value)
                                crtotal = crtotal + Val(.Cells(4).Value)
                            End With

                        Else
                            dg1.Rows.Add()
                            With dg1.Rows(j + 1)
                                .Cells(0).Value = tmpdt.Rows(j)("IDS").tostring()
                                .Cells(1).Value = tmpdt.Rows(j)("EXPENDITURE").tostring()
                                .Cells(2).Value = Format(Math.Abs(Math.Abs(Val(tmpdt.Rows(j)("INAMOUNT").tostring()))), "0.00")
                                netloss = Val(netloss) + Val(.Cells(2).Value)
                                drtotal = drtotal + Val(.Cells(2).Value)
                            End With
                        End If
                    Else
                        If Val(tmpdt.Rows(j)("INAMOUNT").tostring()) < 0 And Val(tmpdt.Rows(j)("INAMOUNT").tostring()) <> 0 Then
                            dg1.Rows.Add()
                            With dg1.Rows(jk)
                                .Cells(0).Value = tmpdt.Rows(j)("IDS").tostring()
                                .Cells(3).Value = tmpdt.Rows(j)("INCOME").tostring()
                                .Cells(4).Value = Format(Math.Abs(Val(tmpdt.Rows(j)("INAMOUNT").tostring())), "0.00")
                                netprofit = Val(netprofit) + Val(.Cells(4).Value)
                                crtotal = crtotal + Val(.Cells(4).Value)
                            End With

                        ElseIf Val(tmpdt.Rows(j)("INAMOUNT").tostring()) > 0 And Val(tmpdt.Rows(j)("INAMOUNT").tostring()) <> 0 Then
                            dg1.Rows.Add()
                            With dg1.Rows(j + 1)
                                .Cells(0).Value = tmpdt.Rows(j)("IDS").tostring()
                                .Cells(1).Value = tmpdt.Rows(j)("INCOME").tostring()
                                .Cells(2).Value = Format(Math.Abs(Val(tmpdt.Rows(j)("INAMOUNT").tostring())), "0.00")
                                netloss = Val(netloss) + Val(.Cells(2).Value)
                                drtotal = crtotal + Val(.Cells(2).Value)
                            End With
                        End If

                    End If
                    jk = jk + 1
                    cnt = cnt + 1
                End With

            End If


            'End If
            '  dg1.Rows.Add()
        Next
        dg1.Rows.Add()
        With dg1.Rows(jk)
            If netprofit > netloss Then
                .Cells(1).Value = "Net Profit"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(Val(netprofit - netloss)), "0.00")
                ' netloss = drtotal + Math.Abs(Val(.Cells(2).Value))
            Else
                .Cells(3).Value = "Net Loss"
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Value = Format(Math.Abs(Val(netloss - netprofit)), "0.00")
                '  netprofit = crtotal + Math.Abs(Val(.Cells(4).Value))
                .Cells(3).Style.BackColor = Color.Red
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Style.ForeColor = Color.GhostWhite
            End If
        End With
        dg1.Rows.Add()
        With dg1.Rows(jk + 1)
            If netprofit > netloss Then
                .Cells(1).Value = "Total"
                .Cells(2).Value = Format(Val(netprofit), "0.00")
                .Cells(3).Value = "Total"
                .Cells(4).Value = Format(Val(netprofit), "0.00")
                .Cells(2).Style.ForeColor = Color.Blue
                .Cells(4).Style.ForeColor = Color.Blue
                .Cells(1).Style.ForeColor = Color.Blue
                .Cells(3).Style.ForeColor = Color.Blue
            Else
                .Cells(1).Value = "Total"
                .Cells(2).Value = Format(Val(netloss), "0.00")
                .Cells(3).Value = "Total"
                .Cells(4).Value = Format(Val(netloss), "0.00")
                .Cells(2).Style.ForeColor = Color.Blue
                .Cells(4).Style.ForeColor = Color.Blue
                .Cells(1).Style.ForeColor = Color.Blue
                .Cells(3).Style.ForeColor = Color.Blue
            End If
            dg1.ClearSelection()
        End With


    End Sub
    Private Sub RetrivePL()
        Dim ssql As String = String.Empty : Dim dtloss As New DataTable
        Dim dtprofit As New DataTable : Dim GPGL As Decimal = 0
        Dim TotalOpBal As Decimal = 0.0 : Dim TotalPurchase As Decimal = 0.0
        Dim TotalSale As Decimal = 0.0 : Dim TotalExp As Decimal = 0.0
        Dim TotalIncome As Decimal = 0.0 : Dim TotalClosing As Decimal = 0.0
        Dim lastval As Integer = 0 : Dim crtotal As Decimal = 0.0
        Dim drtotal As Decimal = 0.0 : Dim netloss As Decimal = 0.0
        Dim netprofit As Decimal = 0.0
        TotalPurchase = Val(clsFun.ExecScalarStr("Select Sum(Amount) as Purchase,TransType From Ledger Where  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and DC='D' and  AccountID in(Select Id from Accounts where GroupId=22)"))
        TotalPurchase = TotalPurchase - Val(clsFun.ExecScalarStr("Select Sum(Amount) as Purchase,TransType From Ledger Where  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and DC='C' and  AccountID in(Select Id from Accounts where GroupId=22)"))
        TotalSale = Val(clsFun.ExecScalarStr("Select Sum(Amount) as Sale From Ledger Where  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'and DC='C' and AccountID in(Select Id from Accounts where GroupId=23)"))
        TotalSale = TotalSale - Val(clsFun.ExecScalarStr("Select Sum(Amount) as Sale From Ledger Where  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'and DC='D' and AccountID in(Select Id from Accounts where GroupId=23)"))
        TotalExp = Val(clsFun.ExecScalarStr("Select Sum(Amount) as IndirectExpenses From Ledger Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'and DC='D' and AccountID in(Select Id from Accounts where GroupId=24)"))
        TotalExp = TotalExp - Val(clsFun.ExecScalarStr("Select Sum(Amount) as IndirectExpenses From Ledger Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'and DC='C' and AccountID in(Select Id from Accounts where GroupId=24)"))
        TotalIncome = Val(clsFun.ExecScalarStr("Select Sum(Amount) as Indirect From Ledger Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'and DC='C' and AccountID in(Select Id from Accounts where GroupId=26)"))
        TotalIncome = TotalIncome - Val(clsFun.ExecScalarStr("Select Sum(Amount) as Indirect From Ledger Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'and DC='D' and AccountID in(Select Id from Accounts where GroupId=26)"))
        dtloss = clsFun.ExecDataTable("Select Sum(Amount) as IndirectExpenses,Accountname,AccountID  From Ledger Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID in(Select Id from Accounts where GroupId=25) group by Accountname")
        dtprofit = clsFun.ExecDataTable("Select Sum(Amount) as Indirectincome,Accountname,AccountID  From Ledger Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and AccountID in(Select Id from Accounts where GroupId=27) group by Accountname")
        TotalOpBal = 0.0
        TotalOpBal = 0.0
        dg1.Rows.Clear()
        dg1.Rows.Add()
        drtotal = 0.0
        crtotal = 0.0
        GPGL = (TotalPurchase + TotalExp) - (TotalSale + TotalIncome)
        dg1.Rows.Clear()
        dg1.Rows.Add()
        With dg1.Rows(0)
            lastval = lastval + 1
            If GPGL > 0 Then
                .Cells(1).Value = "Gross Loss(B/F)"""
                .Cells(2).Value = Math.Abs(GPGL)
                netloss = Math.Abs(GPGL)
                drtotal = drtotal + netloss
            Else
                .Cells(3).Value = "Gross Profit(B/F)"
                .Cells(4).Value = Math.Abs(GPGL)
                netprofit = Math.Abs(GPGL)
                crtotal = crtotal + netprofit
            End If
        End With
        dg1.Rows.Add()
        For i As Integer = 0 To IIf(dtloss.Rows.Count > dtprofit.Rows.Count, dtloss.Rows.Count - 1, dtprofit.Rows.Count - 1)
            With dg1.Rows(i + 1)
                If dtloss.Rows.Count - 1 >= i Then
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtloss.Rows(i)("AccountID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtloss.Rows(i)("AccountID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtloss.Rows(i)("AccountID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtloss.Rows(i)("AccountID").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If
                    Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt <> 0 Then
                        .Cells(1).Value = dtloss.Rows(i)("Accountname")
                        .Cells(2).Value = Format(Math.Abs(Val(tmpamt)), "0.00")
                        netloss = Val(netloss) + Val(.Cells(2).Value)
                        drtotal = drtotal + Val(.Cells(2).Value)
                    End If

                End If
                If dtprofit.Rows.Count - 1 >= i Then
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtprofit.Rows(i)("AccountID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtprofit.Rows(i)("AccountID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtprofit.Rows(i)("AccountID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtprofit.Rows(i)("AccountID").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If
                    Dim tmpamt As String = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt <> 0 Then
                        .Cells(3).Value = dtprofit.Rows(i)("Accountname")
                        .Cells(4).Value = Format(Math.Abs(Val(tmpamt)), "0.00")
                        netprofit = Val(netprofit) + Val(.Cells(4).Value)
                        crtotal = crtotal + Val(.Cells(4).Value)
                    End If
                End If
            End With
            lastval = lastval + 1
            dg1.Rows.Add()
        Next i
        '' dg1.Rows.Add()
        With dg1.Rows(lastval)
            lastval = lastval + 1
            If netprofit > netloss Then
                .Cells(1).Value = "Net Profit"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Math.Abs(netprofit - netloss)
                netloss = Math.Abs(GPGL)
            Else
                .Cells(3).Value = "Net Loss"
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Value = Math.Abs(netprofit - netloss)
                netprofit = Math.Abs(GPGL)
                .Cells(3).Style.BackColor = Color.Red
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Style.ForeColor = Color.GhostWhite
            End If
        End With
        dg1.Rows.Add()
        With dg1.Rows(lastval)
            lastval = lastval + 1
            If netprofit > netloss Then
                .Cells(1).Value = "Total"
                .Cells(2).Value = Math.Abs(netprofit)
                .Cells(3).Value = "Total"
                .Cells(2).Style.ForeColor = Color.Blue
                .Cells(4).Style.ForeColor = Color.Blue
                .Cells(1).Style.ForeColor = Color.Blue
                .Cells(3).Style.ForeColor = Color.Blue
            Else
                .Cells(1).Value = "Total"
                .Cells(2).Value = Math.Abs(netprofit)
                .Cells(3).Value = "Total"
                .Cells(4).Value = Math.Abs(netloss)
                .Cells(2).Style.ForeColor = Color.Blue
                .Cells(4).Style.ForeColor = Color.Blue
                .Cells(1).Style.ForeColor = Color.Blue
                .Cells(3).Style.ForeColor = Color.Blue
            End If
            dg1.ClearSelection()
        End With
    End Sub
    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Panel5_Paint(sender As Object, e As PaintEventArgs)
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
        Report_Viewer.printReport("\Reports\ProfitandLoss.rpt")
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