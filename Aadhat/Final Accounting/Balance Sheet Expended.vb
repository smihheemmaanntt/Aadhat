Public Class Balance_Sheet_Expended
    Dim tmpdt As New DataTable

    Private Sub Balance_Sheet_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Balance_Sheet_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
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
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "LIBILITIES" : dg1.Columns(1).Width = 400
        dg1.Columns(2).Name = "AMOUNT" : dg1.Columns(2).Width = 200
        dg1.Columns(3).Name = "ASSESTS" : dg1.Columns(3).Width = 400
        dg1.Columns(4).Name = "AMOUNT" : dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "ID2" : dg1.Columns(5).Visible = False
        TmpColumns()
        ' retrive()
    End Sub
    Private Sub TmpColumns()
        tmpdt.Columns.Clear()
        tmpdt.Columns.Add("IDS")
        tmpdt.Columns.Add("EXPENDITURE")
        tmpdt.Columns.Add("EXAMOUNT")
        tmpdt.Columns.Add("INCOME")
        tmpdt.Columns.Add("INAMOUNT")
    End Sub
    Private Sub rowColumsExpended()
        dg1.Columns.Clear()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "LIBILITIES" : dg1.Columns(1).Width = 200
        dg1.Columns(2).Name = "AMOUNT" : dg1.Columns(2).Width = 120
        dg1.Columns(3).Name = "TOTAL" : dg1.Columns(3).Width = 120
        dg1.Columns(4).Name = "ASSESTS" : dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "AMOUNT" : dg1.Columns(5).Width = 120
        dg1.Columns(6).Name = "TOTAL" : dg1.Columns(6).Width = 120
        TmpColumns()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        pnlWait.Visible = True
        Retrive()
        'RetriveExpended()
        pnlWait.Visible = False
    End Sub

    Private Sub Retrive()
        Dim ssql As String = String.Empty : Dim dtLiablity As New DataTable
        Dim dtAsset As New DataTable : Dim GPGL As Decimal = 0
        Dim dtTotalSale As New DataTable : Dim dtTotalExp As New DataTable
        Dim dtTotalIncome As New DataTable : Dim dtTotalPurchase As New DataTable
        Dim TotalLiablity As Decimal = 0.0 : Dim TotalAsset As Decimal = 0.0
        Dim TotalPurchase As Decimal = 0.0 : Dim TotalSale As Decimal = 0.0
        Dim TotalExp As Decimal = 0.0 : Dim TotalIncome As Decimal = 0.0
        Dim NetIncome As Decimal = 0.0 : Dim NetExpense As Decimal = 0.0
        Dim TotalClosing As Decimal = 0.0 : Dim assetTotal As Decimal = 0.0
        Dim LiabltyTotal As Decimal = 0.0 : Dim lastval As Integer = 0.0
        Dim curassets As Decimal = 0.0 : Dim Libilities As Decimal = 0.0
        Dim tmpamt As Decimal = 0.0 : Dim id As String = 0 : Dim netprofit As Decimal = 0.00
        Dim netloss As Decimal = 0.00
        dtTotalPurchase = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(22)")
        For i = 0 To dtTotalPurchase.Rows.Count - 1
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
        dtTotalSale = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(23)")
        For i = 0 To dtTotalSale.Rows.Count - 1
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
        dtTotalExp = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(24)")
        For i = 0 To dtTotalExp.Rows.Count - 1
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
        dtTotalIncome = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(26)")
        For i = 0 To dtTotalIncome.Rows.Count - 1
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


        ssql = "Select * from undergroup where ID in (1,3,6,8,10) and dc='Cr' "
        dtLiablity = clsFun.ExecDataTable(ssql)
        ssql = "Select * from undergroup where ID in (2,4,5,7) and dc='Dr'"

        dtAsset = clsFun.ExecDataTable(ssql)
        dg1.Rows.Clear()
        dg1.Rows.Add()

        GPGL = (TotalPurchase + TotalExp) - (TotalSale + TotalIncome)
        If GPGL > 0 Then
            netloss = Format(Math.Abs(Val(GPGL)), "0.00")
        Else
            netprofit = Format(Math.Abs(Val(GPGL)), "0.00")
        End If
        dg1.Rows.Add()
        For i = 0 To IIf(dtLiablity.Rows.Count > dtAsset.Rows.Count, dtLiablity.Rows.Count - 1, dtAsset.Rows.Count - 1)
            Application.DoEvents()
            With dg1.Rows(i)
                Application.DoEvents()
                If dtLiablity.Rows.Count - 1 >= i Then
                    If "Capital Account" = dtLiablity.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname,GroupName,GroupID From Account_AcGrp Where   GroupID in (1,29) or UnderGroupID in (1,29)  order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            Libilities = Libilities + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If Libilities > 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            Libilities = -Val(Libilities)
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        ElseIf Libilities < 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        Else
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        End If
                        TotalLiablity = Format(Math.Abs(Val(TotalLiablity) + Math.Abs(Val(Libilities))), "0.00")
                        .Cells(0).Value = id
                    End If
                    id = ""
                    If "Current Liabilities" = dtLiablity.Rows(i)("UnderGroup") Then
                        Libilities = 0.0
                        ssql = "Select ID,Accountname,GroupName,GroupID From Account_AcGrp Where    GroupID in (3,17,18,19,31,33) or UnderGroupID in (3,17,18,19,31,33) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            Libilities = Libilities + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        id = id.Remove(id.LastIndexOf(","))
                        If Libilities > 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            Libilities = -Val(Libilities)
                            .Cells(2).Value = Format(Val(Libilities), "0.00")
                        ElseIf Libilities < 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        Else
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        End If
                        If Val(.Cells(2).Value) < 0 Then
                            TotalLiablity = Format(Math.Abs(Val(TotalLiablity) - Math.Abs(Val(Libilities))), "0.00")
                        Else

                            TotalLiablity = Format(Math.Abs(Val(TotalLiablity) + Math.Abs(Val(Libilities))), "0.00")
                        End If

                        .Cells(0).Value = id
                    End If
                    id = ""
                    If "Loans (Liability)" = dtLiablity.Rows(i)("UnderGroup") Then
                        Libilities = 0.0
                        ssql = "Select ID,Accountname,GroupName,GroupID From Account_AcGrp Where GroupID in (6,20,21,28) or UnderGroupID in (6,20,21,28) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            Libilities = Libilities + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If Libilities > 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            Libilities = -Val(Libilities)
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        ElseIf Libilities < 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        Else
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")

                        End If
                        TotalLiablity = Format(Math.Abs(Val(TotalLiablity) + Val(Libilities)), "0.00")
                        .Cells(0).Value = id
                    End If

                    If "Suspense Account" = dtLiablity.Rows(i)("UnderGroup") Then
                        Libilities = 0.0
                        ssql = "Select ID,Accountname,GroupName,GroupID From Account_AcGrp Where GroupID in (10) or UnderGroupID in (10) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            Libilities = Libilities + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If Libilities > 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            Libilities = -Val(Libilities)
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        ElseIf Libilities < 0 Then
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")
                        Else
                            .Cells(0).Value = dtLiablity.Rows(i)("ID")
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Format(Math.Abs(Val(Libilities)), "0.00")

                        End If
                        TotalLiablity = Format(Math.Abs(Val(TotalLiablity) + Val(.Cells(2).Value)), "0.00")
                        .Cells(0).Value = id
                    End If

                End If
                curassets = 0.0 : id = ""
                If dtAsset.Rows.Count - 1 >= i Then
                    If "Current Assets" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname,GroupID From Account_AcGrp Where    GroupID in (2,11,12,13,14,16,32) or UnderGroupID in (2,11,12,13,14,16,32) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If curassets > 0 Then

                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))
                        ElseIf curassets < 0 Then
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = -Val(curassets)
                        Else
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))

                        End If
                        TotalAsset = Format(Val(TotalAsset) + Val(.Cells(4).Value), "0.00")
                        .Cells(5).Value = id
                    End If
                    curassets = 0.0 : id = ""
                    If "Fixed Assets" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname,GroupID From Account_AcGrp Where    GroupID in (4) or UnderGroupID in (4) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If curassets > 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))
                        ElseIf curassets < 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(1).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(2).Value = Math.Abs(Val(curassets))
                        Else
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))

                        End If
                        TotalAsset = Format(Val(TotalAsset) + Val(.Cells(4).Value), "0.00")
                        .Cells(5).Value = id
                    End If
                    curassets = 0.0 : id = ""
                    If "Investments" = dtAsset.Rows(i)("UnderGroup") Then

                        ssql = "Select ID,Accountname,GroupID From Account_AcGrp Where GroupID in (5) or UnderGroupID in (5) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If curassets > 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))
                        ElseIf curassets < 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = -Val(curassets)
                        Else
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))

                        End If
                        TotalAsset = Format(Val(TotalAsset) + Val(.Cells(4).Value), "0.00")
                        .Cells(5).Value = id
                    End If
                    curassets = 0.0 : id = ""
                    If "Pre-Operative Expenses" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname,GroupID From Account_AcGrp Where GroupID in (7) or UnderGroupID in () order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If curassets > 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))
                        ElseIf curassets < 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = -Val(curassets)
                        Else
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))

                        End If
                        TotalAsset = Format(Val(TotalAsset) + Val(.Cells(4).Value), "0.00")
                        .Cells(5).Value = id
                    End If
                    curassets = 0.0 : id = ""
                    If "Suspense Account" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname,GroupID From Account_AcGrp Where GroupID in (10) or UnderGroupID in (10) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Application.DoEvents()
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            id = id & dt.Rows(j)("GroupID") & ","
                        Next
                        If id <> "" Then
                            id = id.Remove(id.LastIndexOf(","))
                        End If
                        If curassets > 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))
                        ElseIf curassets < 0 Then
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = -Val(curassets)
                        Else
                            .Cells(5).Value = dtAsset.Rows(i)("ID")
                            .Cells(3).Value = dtAsset.Rows(i)("UnderGroup")
                            .Cells(4).Value = Math.Abs(Val(curassets))

                        End If
                        TotalAsset = Format(Val(TotalAsset) + Val(.Cells(4).Value), "0.00")
                        .Cells(5).Value = id
                    End If
                End If
            End With
            lastval = lastval + 1
            dg1.Rows.Add()
        Next i
        tmpdt.Rows.Clear()
        tmpdt.Rows.Add()
        dtloss = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(25)")
        dtprofit = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(27)")
        For N As Integer = 0 To IIf(dtloss.Rows.Count > dtprofit.Rows.Count, dtloss.Rows.Count - 1, dtprofit.Rows.Count - 1)
            tmpdt.Rows.Add()
            With tmpdt
                If dtloss.Rows.Count - 1 >= N Then
                    Application.DoEvents()
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtloss.Rows(N)("ID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtloss.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtloss.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtloss.Rows(N)("ID").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If
                    tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt < 0 Then
                        .Rows(N)(0) = dtloss.Rows(N)("ID")
                        .Rows(N)(1) = dtloss.Rows(N)("Accountname")
                        .Rows(N)(2) = Val(tmpamt)
                        netloss = Val(netloss) + Val(.Rows(N)(2))
                    ElseIf tmpamt > 0 Then
                        .Rows(N)(0) = dtloss.Rows(N)("ID")
                        .Rows(N)(1) = dtloss.Rows(N)("Accountname")
                        .Rows(N)(2) = Math.Abs(Val(tmpamt))
                        netloss = Val(.Rows(N)(2)) + Val(netloss)
                    End If
                End If
                If dtprofit.Rows.Count - 1 >= N Then
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtprofit.Rows(N)("ID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtprofit.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtprofit.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtprofit.Rows(N)("id").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If

                    tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt < 0 Then
                        .Rows(N)(0) = dtprofit.Rows(N)("ID")
                        .Rows(N)(3) = dtprofit.Rows(N)("Accountname")
                        .Rows(N)(4) = Math.Abs(Val(tmpamt))
                        netprofit = Val(netprofit) + Val(.Rows(N)(4))
                    ElseIf tmpamt > 0 Then
                        .Rows(N)(0) = dtprofit.Rows(N)("ID")
                        .Rows(N)(3) = dtprofit.Rows(N)("Accountname")
                        .Rows(N)(4) = Val(tmpamt)
                        netprofit = Val(netprofit) - Val(.Rows(N)(4))
                    End If
                End If
            End With

        Next N



        With dg1.Rows(lastval)

            If netprofit > netloss Then
                lastval = lastval + 1
                .Cells(1).Value = "Net Profit"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(Val(netprofit - netloss)), "0.00")
                'netloss = Math.Abs(GPGL)
                TotalLiablity = Format(TotalLiablity + Val(.Cells(2).Value), "0.00")
            ElseIf TotalAsset < TotalLiablity Then
                lastval = lastval + 1
                .Cells(3).Value = "Net Loss"
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Value = Math.Abs(Val(netprofit + netloss))
                ' netprofit = Math.Abs(GPGL)
                .Cells(3).Style.BackColor = Color.Red
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Style.ForeColor = Color.GhostWhite
                TotalAsset = TotalAsset + Math.Abs(Val(.Cells(4).Value))
            Else
                lastval = lastval + 1
                .Cells(1).Value = "Net Profit"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(Val(netprofit - netloss)), "0.00")
                'netloss = Math.Abs(GPGL)
                TotalLiablity = Format(TotalLiablity + Val(.Cells(2).Value), "0.00")
            End If
        End With


        ' lastval = lastval + 1
        dg1.Rows.Add()
        With dg1.Rows(lastval)
            If TotalAsset > TotalLiablity Then
                lastval = lastval + 1
                .Cells(1).Value = "Difference in Opening Balance"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Val(Math.Abs(TotalAsset - TotalLiablity)), "0.00")
            ElseIf TotalAsset < TotalLiablity Then
                lastval = lastval + 1
                .Cells(3).Value = "Difference in Opening Balance"
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Value = Format(Val(Math.Abs(TotalLiablity - TotalAsset)), "0.00")
                .Cells(3).Style.BackColor = Color.Red
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Style.ForeColor = Color.GhostWhite
            End If
        End With
        With dg1.Rows(lastval)
            If TotalAsset > TotalLiablity Then
                .Cells(1).Value = "Total"
                .Cells(3).Value = "Total"
                .Cells(2).Style.BackColor = Color.ForestGreen
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.ForestGreen
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(3).Style.BackColor = Color.ForestGreen
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.ForestGreen
                .Cells(4).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(TotalAsset), "0.00")
                .Cells(4).Value = Format(Math.Abs(TotalAsset), "0.00")
            Else
                .Cells(1).Value = "Total"
                .Cells(3).Value = "Total"
                .Cells(2).Style.BackColor = Color.ForestGreen
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.ForestGreen
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(3).Style.BackColor = Color.ForestGreen
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.ForestGreen
                .Cells(4).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(TotalLiablity), "0.00")
                .Cells(4).Value = Format(Math.Abs(TotalLiablity), "0.00")

            End If
        End With
        dg1.ClearSelection()
    End Sub
    Private Sub RetriveExpended()
        Dim ssql As String = String.Empty : Dim dtLiablity As New DataTable
        Dim dtAsset As New DataTable : Dim GPGL As Decimal = 0
        Dim dtTotalSale As New DataTable : Dim dtTotalExp As New DataTable
        Dim dtTotalIncome As New DataTable : Dim dtTotalPurchase As New DataTable
        Dim TotalLiablity As Decimal = 0.0 : Dim TotalAsset As Decimal = 0.0
        Dim TotalPurchase As Decimal = 0.0 : Dim TotalSale As Decimal = 0.0
        Dim TotalExp As Decimal = 0.0 : Dim TotalIncome As Decimal = 0.0
        Dim NetIncome As Decimal = 0.0 : Dim NetExpense As Decimal = 0.0
        Dim TotalClosing As Decimal = 0.0 : Dim assetTotal As Decimal = 0.0
        Dim LiabltyTotal As Decimal = 0.0 : Dim lastval As Integer = 0.0
        Dim curassets As Decimal = 0.0 : Dim Libilities As Decimal = 0.0
        Dim tmpamt As Decimal = 0.0 : Dim netloss As Decimal = 0.0
        Dim netprofit As Decimal = 0.0
        dtTotalPurchase = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(22)")
        For i = 0 To dtTotalPurchase.Rows.Count - 1
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
        dtTotalSale = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(23)")
        For i = 0 To dtTotalSale.Rows.Count - 1
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
        dtTotalExp = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(24)")
        For i = 0 To dtTotalExp.Rows.Count - 1
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
        dtTotalIncome = clsFun.ExecDataTable("Select ID,Accountname From Account_AcGrp Where    GroupID in(26)")
        For i = 0 To dtTotalIncome.Rows.Count - 1
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


        ssql = "Select * from undergroup where ID in (1,3,6,8) and dc='Cr' "
        dtLiablity = clsFun.ExecDataTable(ssql)
        ssql = "Select * from undergroup where ID in (2,4,5,7,10) and dc='Dr'"

        dtAsset = clsFun.ExecDataTable(ssql)
        dg1.Rows.Clear()
        dg1.Rows.Add()

        GPGL = (TotalPurchase + TotalExp) - (TotalSale + TotalIncome)
        If GPGL > 0 Then
            netloss = Format(Math.Abs(Val(GPGL)), "0.00")
        Else
            netprofit = Format(Math.Abs(Val(GPGL)), "0.00")
        End If
        '  dg1.Rows.Add()
        For i = 0 To IIf(dtLiablity.Rows.Count > dtAsset.Rows.Count, dtLiablity.Rows.Count - 1, dtAsset.Rows.Count - 1)
            With dg1.Rows(i)
                If dtLiablity.Rows.Count - 1 >= i Then
                    If "Capital Account" = dtLiablity.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname,GroupName From Account_AcGrp Where    GroupID in (1,29) or UnderGroupID in (1,29)  order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            Libilities = Libilities + Val(tmpamt)
                        Next
                        If Libilities > 0 Then
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = -Val(Libilities)
                        ElseIf Libilities < 0 Then
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Math.Abs(Val(Libilities))
                        Else
                            .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                            .Cells(2).Value = Math.Abs(Val(Libilities))
                        End If
                        TotalLiablity = Format(Val(TotalLiablity) + Val(.Cells(2).Value), "0.00")
                    End If
                    If "Current Liabilities" = dtLiablity.Rows(i)("UnderGroup") Then
                        Libilities = 0.0
                        ssql = "Select ID,Accountname,GroupName From Account_AcGrp Where    GroupID in (3,17,18,19,31,33) or UnderGroupID in (3,17,18,19,31,33) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            Libilities = Libilities + Val(tmpamt)
                            lastval = lastval + 1

                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                If Libilities > 0 Then
                                    .Cells(1).Value = dt.Rows(j)("AccountName")
                                    .Cells(2).Value = -Val(tmpamt)
                                    TotalLiablity = Format(Val(TotalLiablity) + Val(.Cells(2).Value), "0.00")
                                ElseIf Libilities < 0 Then
                                    .Cells(1).Value = dt.Rows(j)("AccountName")
                                    .Cells(2).Value = Math.Abs(Val(tmpamt))
                                    TotalLiablity = Format(Val(TotalLiablity) + Val(.Cells(2).Value), "0.00")
                                Else
                                    .Cells(1).Value = dt.Rows(j)("AccountName")
                                    .Cells(2).Value = Math.Abs(Val(tmpamt))
                                    TotalLiablity = Format(Val(TotalLiablity) + Val(.Cells(2).Value), "0.00")
                                End If
                            End With

                        Next
                        .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                        .Cells(3).Value = Math.Abs(Val(TotalLiablity))
                    End If
                    If "Loans (Liability)" = dtLiablity.Rows(i)("UnderGroup") Then
                        Libilities = 0.0
                        ssql = "Select ID,Accountname,GroupName From Account_AcGrp Where    GroupID in (6,20,21,28) or UnderGroupID in (6,20,21,28) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            Libilities = Libilities + Val(tmpamt)
                            lastval = lastval + 1

                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                If Libilities > 0 Then
                                    .Cells(1).Value = dt.Rows(j)("AccountName")
                                    .Cells(2).Value = -Val(tmpamt)
                                    TotalLiablity = Format(Val(TotalLiablity) + Val(.Cells(2).Value), "0.00")
                                ElseIf Libilities < 0 Then
                                    .Cells(1).Value = dt.Rows(j)("AccountName")
                                    .Cells(2).Value = Math.Abs(Val(tmpamt))
                                    TotalLiablity = Format(Val(TotalLiablity) + Val(.Cells(2).Value), "0.00")
                                Else
                                    .Cells(1).Value = dt.Rows(j)("AccountName")
                                    .Cells(2).Value = Math.Abs(Val(tmpamt))
                                    TotalLiablity = Format(Val(TotalLiablity) + Val(.Cells(2).Value), "0.00")
                                End If
                            End With

                        Next
                        .Cells(1).Value = dtLiablity.Rows(i)("UnderGroup")
                        .Cells(3).Value = Math.Abs(Val(TotalLiablity))
                    End If
                End If
                curassets = 0.0
                If dtAsset.Rows.Count - 1 >= i Then
                    If "Current Assets" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname,GroupName,GroupID From Account_AcGrp Where    GroupID in (2,11,12,13,14,16,32) or UnderGroupID in (2,11,12,13,14,16,32) Group by GroupID "
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select Sum(OpBal) FROM Accounts WHERE GroupID in (  " & Val(dt.Rows(j)("GroupID").ToString()) & ")")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID in(Select Id from Accounts where GroupId in(" & Val(dt.Rows(j)("GroupID").ToString()) & ")) and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID in(Select Id from Accounts where GroupId in(" & Val(dt.Rows(j)("GroupID").ToString()) & ")) and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID in(Select Id from Accounts where GroupId in(" & Val(dt.Rows(j)("GroupID").ToString()) & "))")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            lastval = lastval + 1
                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                If curassets > 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = -Val(tmpamt)
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                ElseIf curassets < 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                Else
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                End If
                            End With

                        Next
                        .Cells(4).Value = dtAsset.Rows(i)("UnderGroup")
                        .Cells(5).Value = Math.Abs(Val(TotalAsset))
                    End If
                    curassets = 0.0
                    If "Fixed Assets" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname From Account_AcGrp Where    GroupID in (4) or UnderGroupID in (4) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            lastval = lastval + 1
                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                If curassets > 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = -Val(tmpamt)
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                ElseIf curassets < 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                Else
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                End If
                            End With

                        Next
                        .Cells(4).Value = dtAsset.Rows(i)("UnderGroup")
                        .Cells(5).Value = Math.Abs(Val(TotalAsset))
                    End If
                    curassets = 0.0
                    If "Investments" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname From Account_AcGrp Where GroupID in (5) or UnderGroupID in (5) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            lastval = lastval + 1
                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                If curassets > 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = -Val(tmpamt)
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                ElseIf curassets < 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                Else
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                End If
                            End With

                        Next
                        .Cells(4).Value = dtAsset.Rows(i)("UnderGroup")
                        .Cells(5).Value = Math.Abs(Val(TotalAsset))
                    End If
                    curassets = 0.0
                    If "Pre-Operative Expenses" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname From Account_AcGrp Where GroupID in (7) or UnderGroupID in () order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            lastval = lastval + 1
                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                If curassets > 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = -Val(tmpamt)
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                ElseIf curassets < 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                Else
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                End If
                            End With

                        Next
                        .Cells(4).Value = dtAsset.Rows(i)("UnderGroup")
                        .Cells(5).Value = Math.Abs(Val(TotalAsset))
                    End If


                    curassets = 0.0
                    If "Suspense Account" = dtAsset.Rows(i)("UnderGroup") Then
                        ssql = "Select ID,Accountname From Account_AcGrp Where GroupID in (10) or UnderGroupID in (10) order by AccountName"
                        Dim dt As New DataTable
                        dt = clsFun.ExecDataTable(ssql)
                        For j As Integer = 0 To dt.Rows.Count - 1
                            Dim opbal As String = "" : Dim ClBal As String = ""
                            opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dt.Rows(j)("ID").ToString()) & "")
                            Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dt.Rows(j)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                            Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(j)("ID").ToString()) & "")
                            If drcr = "Dr" Then
                                tmpamtdr = Val(opbal) + Val(tmpamtdr)
                            Else
                                tmpamtcr = Val(opbal) + Val(tmpamtcr)
                            End If
                            tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                            curassets = curassets + Val(tmpamt)
                            lastval = lastval + 1
                            dg1.Rows.Add()
                            With dg1.Rows(lastval)
                                If curassets > 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = -Val(tmpamt)
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                ElseIf curassets < 0 Then
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                Else
                                    .Cells(4).Value = dt.Rows(j)("AccountName")
                                    .Cells(5).Value = Math.Abs(Val(tmpamt))
                                    TotalAsset = Format(Val(TotalAsset) + Val(.Cells(5).Value), "0.00")
                                End If
                            End With

                        Next
                        .Cells(4).Value = dtAsset.Rows(i)("UnderGroup")
                        .Cells(5).Value = Math.Abs(Val(TotalAsset))
                    End If
                End If
            End With
            lastval = lastval + 1
            dg1.Rows.Add()
        Next i
        tmpdt.Rows.Clear()
        tmpdt.Rows.Add()
        dtloss = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(25)")
        dtprofit = clsFun.ExecDataTable("Select ID,Accountname From Accounts Where    GroupID in(27)")
        For N As Integer = 0 To IIf(dtloss.Rows.Count > dtprofit.Rows.Count, dtloss.Rows.Count - 1, dtprofit.Rows.Count - 1)
            tmpdt.Rows.Add()
            With tmpdt
                If dtloss.Rows.Count - 1 >= N Then
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtloss.Rows(N)("ID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtloss.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtloss.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtloss.Rows(N)("ID").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If
                    tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt < 0 Then
                        .Rows(N)(0) = dtloss.Rows(N)("ID")
                        .Rows(N)(1) = dtloss.Rows(N)("Accountname")
                        .Rows(N)(2) = Val(tmpamt)
                        netloss = Val(netloss) + Val(.Rows(N)(2))
                    ElseIf tmpamt > 0 Then
                        .Rows(N)(0) = dtloss.Rows(N)("ID")
                        .Rows(N)(1) = dtloss.Rows(N)("Accountname")
                        .Rows(N)(2) = Math.Abs(Val(tmpamt))
                        netloss = Val(netloss) + Val(.Rows(N)(2))
                    End If
                End If
                If dtprofit.Rows.Count - 1 >= N Then
                    Dim opbal As String = "" : Dim ClBal As String = ""
                    opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(dtprofit.Rows(N)("ID").ToString()) & "")
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(dtprofit.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(dtprofit.Rows(N)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dtprofit.Rows(N)("id").ToString()) & "")
                    If drcr = "Dr" Then
                        tmpamtdr = Val(opbal) + Val(tmpamtdr)
                    Else
                        tmpamtcr = Val(opbal) + Val(tmpamtcr)
                    End If

                    tmpamt = Val(tmpamtdr) - Val(tmpamtcr)
                    If tmpamt < 0 Then
                        .Rows(N)(0) = dtprofit.Rows(N)("ID")
                        .Rows(N)(3) = dtprofit.Rows(N)("Accountname")
                        .Rows(N)(4) = Math.Abs(Val(tmpamt))
                        netprofit = Val(netprofit) + Val(.Rows(N)(4))
                    ElseIf tmpamt > 0 Then
                        .Rows(N)(0) = dtprofit.Rows(N)("ID")
                        .Rows(N)(3) = dtprofit.Rows(N)("Accountname")
                        .Rows(N)(4) = Val(tmpamt)
                        netprofit = Val(netprofit) + Val(.Rows(N)(4))
                    End If
                End If
            End With

        Next N



        With dg1.Rows(lastval)

            If netprofit > netloss Then
                lastval = lastval + 1
                .Cells(1).Value = "Net Profit"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(Val(netprofit - netloss)), "0.00")
                'netloss = Math.Abs(GPGL)
                TotalLiablity = Format(TotalLiablity + Val(.Cells(2).Value), "0.00")
            ElseIf TotalAsset < TotalLiablity Then
                lastval = lastval + 1
                .Cells(3).Value = "Net Loss"
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Value = Math.Abs(Val(netprofit + netloss))
                ' netprofit = Math.Abs(GPGL)
                .Cells(3).Style.BackColor = Color.Red
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Style.ForeColor = Color.GhostWhite
                TotalAsset = TotalAsset + Math.Abs(Val(.Cells(4).Value))
            Else
                lastval = lastval + 1
                .Cells(1).Value = "Net Profit"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(Val(netprofit - netloss)), "0.00")
                'netloss = Math.Abs(GPGL)
                TotalLiablity = Format(TotalLiablity + Val(.Cells(2).Value), "0.00")
            End If
        End With


        ' lastval = lastval + 1
        dg1.Rows.Add()
        With dg1.Rows(lastval)

            If TotalAsset > TotalLiablity Then
                lastval = lastval + 1
                .Cells(1).Value = "Difference in Opening Balance"
                .Cells(2).Style.BackColor = Color.Green
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.Green
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Val(Math.Abs(TotalAsset - TotalLiablity)), "0.00")
            ElseIf TotalAsset < TotalLiablity Then
                lastval = lastval + 1
                .Cells(3).Value = "Difference in Opening Balance"
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Value = Format(Val(Math.Abs(TotalLiablity - TotalAsset)), "0.00")
                .Cells(3).Style.BackColor = Color.Red
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.Red
                .Cells(4).Style.ForeColor = Color.GhostWhite
            End If
        End With
        With dg1.Rows(lastval)
            If TotalAsset > TotalLiablity Then
                .Cells(1).Value = "Total"
                .Cells(3).Value = "Total"
                .Cells(2).Style.BackColor = Color.ForestGreen
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.ForestGreen
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(3).Style.BackColor = Color.ForestGreen
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.ForestGreen
                .Cells(4).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(TotalAsset), "0.00")
                .Cells(4).Value = Format(Math.Abs(TotalAsset), "0.00")
            Else
                .Cells(1).Value = "Total"
                .Cells(3).Value = "Total"
                .Cells(2).Style.BackColor = Color.ForestGreen
                .Cells(2).Style.ForeColor = Color.GhostWhite
                .Cells(1).Style.BackColor = Color.ForestGreen
                .Cells(1).Style.ForeColor = Color.GhostWhite
                .Cells(3).Style.BackColor = Color.ForestGreen
                .Cells(3).Style.ForeColor = Color.GhostWhite
                .Cells(4).Style.BackColor = Color.ForestGreen
                .Cells(4).Style.ForeColor = Color.GhostWhite
                .Cells(2).Value = Format(Math.Abs(TotalLiablity), "0.00")
                .Cells(4).Value = Format(Math.Abs(TotalLiablity), "0.00")

            End If
        End With
        dg1.ClearSelection()
    End Sub
    Private Sub RectangleShape1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.CurrentCell.ColumnIndex = 3 Or dg1.CurrentCell.ColumnIndex = 4 Then
            Dim tmpID As String = dg1.CurrentRow.Cells(5).Value
            Dim tmpdate As String = mskEntryDate.Text
            If tmpID = "" Then Exit Sub
            Group_Summary.MdiParent = MainScreenForm
            Group_Summary.Show()
            Group_Summary.retrive(tmpID, tmpdate)
            If Not Group_Summary Is Nothing Then
                Group_Summary.BringToFront()
            End If

        Else
            Dim tmpID As String = dg1.CurrentRow.Cells(0).Value
            Dim tmpdate As String = mskEntryDate.Text
            If tmpID = "" Then Exit Sub
            Group_Summary.MdiParent = MainScreenForm
            Group_Summary.Show()
            Group_Summary.retrive(tmpID, tmpdate)
            If Not Group_Summary Is Nothing Then
                Group_Summary.BringToFront()
            End If
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.CurrentCell.ColumnIndex = 3 Or dg1.CurrentCell.ColumnIndex = 4 Then
                Dim tmpID As String = dg1.CurrentRow.Cells(5).Value
                Dim tmpdate As String = mskEntryDate.Text
                If tmpID = "" Then Exit Sub
                Group_Summary.MdiParent = MainScreenForm
                Group_Summary.Show()
                Group_Summary.retrive(tmpID, tmpdate)
                If Not Group_Summary Is Nothing Then
                    Group_Summary.BringToFront()
                End If

            Else
                Dim tmpID As String = dg1.CurrentRow.Cells(0).Value
                Dim tmpdate As String = mskEntryDate.Text
                If tmpID = "" Then Exit Sub
                Group_Summary.MdiParent = MainScreenForm
                Group_Summary.Show()
                Group_Summary.retrive(tmpID, tmpdate)
                If Not Group_Summary Is Nothing Then
                    Group_Summary.BringToFront()
                End If
            End If
            e.SuppressKeyPress = True
        End If
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")> Private Sub printRecord()
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
        Report_Viewer.printReport("\Reports\BalanceSheet.rpt")
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