Public Class Trading_AccountNew

    Private Sub Trading_Account_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Trading_Account_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.KeyPreview = True
        rowColums() : rowColumsLibilities() : rowColumsAssests()
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

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        If dg1.CurrentRow.Cells(1).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
        If dg1.CurrentRow.Cells(2).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
        If dg1.CurrentRow.Cells(3).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
        If dg1.CurrentRow.Cells(5).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
        If dg1.CurrentRow.Cells(6).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
        If dg1.CurrentRow.Cells(7).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Down Then
            If dg1.CurrentRow.Cells(1).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
            If dg1.CurrentRow.Cells(2).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
            If dg1.CurrentRow.Cells(3).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
            If dg1.CurrentRow.Cells(5).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
            If dg1.CurrentRow.Cells(6).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
            If dg1.CurrentRow.Cells(7).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
        End If
        If e.KeyCode = Keys.Up Then
            If dg1.CurrentRow.Cells(1).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
            If dg1.CurrentRow.Cells(2).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
            If dg1.CurrentRow.Cells(3).Selected = True Then dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(2).Selected = True : dg1.CurrentRow.Cells(3).Selected = True
            If dg1.CurrentRow.Cells(5).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
            If dg1.CurrentRow.Cells(6).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
            If dg1.CurrentRow.Cells(7).Selected = True Then dg1.CurrentRow.Cells(5).Selected = True : dg1.CurrentRow.Cells(6).Selected = True : dg1.CurrentRow.Cells(7).Selected = True
        End If
        If e.KeyCode = Keys.Enter Then
            Dim tmpID As String = String.Empty
            Dim tmpdate As String = String.Empty
            If (dg1.CurrentCell.ColumnIndex = 1 Or dg1.CurrentCell.ColumnIndex = 2 Or dg1.CurrentCell.ColumnIndex = 3) And dg1.CurrentRow.Cells(8).Value = "Groups" Then
                tmpID = dg1.CurrentRow.Cells(0).Value
                tmpdate = mskEntryDate.Text
                If tmpID = "" Then Exit Sub
                Group_Summary.MdiParent = MainScreenForm
                Group_Summary.Show()
                Group_Summary.retrive(tmpID, tmpdate)
                If Not Group_Summary Is Nothing Then
                    Group_Summary.BringToFront()
                End If

            ElseIf (dg1.CurrentCell.ColumnIndex = 5 Or dg1.CurrentCell.ColumnIndex = 6 Or dg1.CurrentCell.ColumnIndex = 7) And dg1.CurrentRow.Cells(8).Value = "Groups" Then
                tmpID = dg1.CurrentRow.Cells(4).Value
                tmpdate = mskEntryDate.Text
                If tmpID = "" Then Exit Sub
                Group_Summary.MdiParent = MainScreenForm
                Group_Summary.Show()
                Group_Summary.retrive(tmpID, tmpdate)
                If Not Group_Summary Is Nothing Then
                    Group_Summary.BringToFront()
                End If
            ElseIf (dg1.CurrentCell.ColumnIndex = 1 Or dg1.CurrentCell.ColumnIndex = 2 Or dg1.CurrentCell.ColumnIndex = 3) And dg1.CurrentRow.Cells(8).Value = "Accounts" And Val(dg1.CurrentRow.Cells(0).Value) <> 0 Then
                tmpID = dg1.CurrentRow.Cells(0).Value
                tmpdate = mskEntryDate.Text
                If tmpID = "" Then Exit Sub
                Ledger.MdiParent = MainScreenForm
                Ledger.Show()
                Ledger.cbAccountName.SelectedValue = Val(dg1.CurrentRow.Cells(0).Value)
                Ledger.BringToFront()
                Ledger.mskFromDate.Text = clsFun.convdate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
                Ledger.MsktoDate.Text = mskEntryDate.Text
                Ledger.ckMerge.Checked = True
                Ledger.btnShow.PerformClick()

            ElseIf (dg1.CurrentCell.ColumnIndex = 5 Or dg1.CurrentCell.ColumnIndex = 6 Or dg1.CurrentCell.ColumnIndex = 7) And dg1.CurrentRow.Cells(8).Value = "Accounts" And Val(dg1.CurrentRow.Cells(4).Value) <> 0 Then
                tmpID = dg1.CurrentRow.Cells(4).Value
                tmpdate = mskEntryDate.Text
                If tmpID = "" Then Exit Sub
                Ledger.MdiParent = MainScreenForm
                Ledger.Show()
                Ledger.cbAccountName.SelectedValue = Val(dg1.CurrentRow.Cells(4).Value)
                Ledger.BringToFront()
                Ledger.mskFromDate.Text = clsFun.convdate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
                Ledger.MsktoDate.Text = mskEntryDate.Text
                Ledger.ckMerge.Checked = True
                Ledger.btnShow.PerformClick()
            End If
            e.SuppressKeyPress = True
        End If


    End Sub
    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Particulars" : dg1.Columns(1).Width = 317
        dg1.Columns(2).Name = "" : dg1.Columns(2).Width = 120
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).Name = "Amount" : dg1.Columns(3).Width = 150
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).Name = "ID" : dg1.Columns(4).Visible = False
        dg1.Columns(5).Name = "Particulars" : dg1.Columns(5).Width = 317
        dg1.Columns(6).Name = "" : dg1.Columns(6).Width = 120
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).Name = "Amount" : dg1.Columns(7).Width = 150
        dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).Name = "Type" : dg1.Columns(8).Visible = False
        dg1.Columns(9).Name = "ID2" : dg1.Columns(9).Visible = False
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub TryNew()
        Dim dt As DataTable = New DataTable()
        dt.Columns.Add("DC", GetType(System.String))
        dt.Columns.Add("Ids", GetType(System.Int32))
        dt.Columns.Add("GroupName", GetType(System.String))
        dt.Columns.Add("GroupID", GetType(System.Int32))
        dt.Columns.Add("GroupBalance", GetType(System.Decimal))
        dt.Columns.Add("DC1", GetType(System.String))
        dt.Columns.Add("Ids1", GetType(System.Int32))
        dt.Columns.Add("GroupName1", GetType(System.String))
        dt.Columns.Add("GroupID1", GetType(System.Int32))
        dt.Columns.Add("GroupBalance1", GetType(System.Decimal))
        Dim row As DataRow = dt.NewRow()
        row("DC") = "Dr"
        row("Ids") = 1
        row("GroupName") = "Purchase Accounts"
        row("GroupID") = 22
        row("GroupBalance") = 5369.36
        dt.Rows.Add(row)
        row = dt.NewRow()
        row("DC") = "Cr"
        row("Ids") = 2
        row("GroupName") = "Sales Accounts"
        row("GroupID") = 23
        row("GroupBalance") = -200.35
        dt.Rows.Add(row)
        row = dt.NewRow()
        row("DC") = "Dr"
        row("Ids") = 3
        row("GroupName") = "Expenses(direct/mfg)"
        row("GroupID") = 24
        row("GroupBalance") = -597
        dt.Rows.Add(row)
        row = dt.NewRow()
        row("DC") = "Cr"
        row("Ids") = 4
        row("GroupName") = "Expenses(direct/mfg)"
        row("GroupID") = 25
        row("GroupBalance") = 14500.14
        dt.Rows.Add(row)
        row = dt.NewRow()
        row("DC") = "Dr"
        row("Ids") = 5
        row("GroupName") = "Others"
        row("GroupID") = 26
        row("GroupBalance") = -56936
        dt.Rows.Add(row)
        Dim tempCrs As DataTable = New DataTable()
        tempCrs.Columns.Add("CrDr", GetType(System.String))
        tempCrs.Columns.Add("GroupName", GetType(System.String))
        tempCrs.Columns.Add("Balance", GetType(System.Decimal))
    End Sub
    Private Sub Retrive1()
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
    Private Sub rowColumsLibilities()
        DgLibilities.ColumnCount = 8
        DgLibilities.Columns(0).Name = "ID"
        DgLibilities.Columns(0).Visible = False
        DgLibilities.Columns(1).Name = "PURCHASE"
        DgLibilities.Columns(1).Width = 300
        DgLibilities.Columns(2).Name = ""
        DgLibilities.Columns(2).Width = 200
        DgLibilities.Columns(3).Name = "AMOUNT"
        DgLibilities.Columns(3).Width = 300
        DgLibilities.Columns(4).Name = "ID"
        DgLibilities.Columns(4).Visible = False
        DgLibilities.Columns(5).Name = "SALE"
        DgLibilities.Columns(5).Width = 300
        DgLibilities.Columns(6).Name = ""
        DgLibilities.Columns(6).Width = 200
        DgLibilities.Columns(7).Name = "AMOUNT"
        DgLibilities.Columns(7).Width = 300
    End Sub
    Private Sub rowColumsAssests()
        dgAssests.ColumnCount = 8
        dgAssests.Columns(0).Name = "ID"
        dgAssests.Columns(0).Visible = False
        dgAssests.Columns(1).Name = "PURCHASE"
        dgAssests.Columns(1).Width = 340
        dgAssests.Columns(2).Name = ""
        dgAssests.Columns(2).Width = 200
        dgAssests.Columns(3).Name = "AMOUNT"
        dgAssests.Columns(3).Width = 300
        dgAssests.Columns(4).Name = "ID"
        dgAssests.Columns(4).Visible = False
        dgAssests.Columns(5).Name = "SALE"
        dgAssests.Columns(5).Width = 300
        dgAssests.Columns(6).Name = ""
        dgAssests.Columns(6).Width = 200
        dgAssests.Columns(7).Name = "AMOUNT"
        dgAssests.Columns(7).Width = 350

    End Sub
    Private Sub RetriveLibilities()
        DgLibilities.Rows.Clear() : dgAssests.Rows.Clear()
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable

        Dim lastval = 0
        Dim sql As String = " Select Primary2,DC,(SELECT GROUP_CONCAT(ID) FROM AccountGroup Where UnderGroupID=GroupID) as IDs," &
            " (Case When Primary2='Y' then GroupName Else UnderGroupName end ) as GroupName,(Case When Primary2='Y' then GroupID Else UnderGroupID end ) as GroupID," &
            "  Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
            "  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))" &
            "  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') " &
            "  +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end)" &
            "  as  GroupBal from Account_AcGrp  Where (GroupID  in(22,23,24,26)  or UnderGroupID  in(22,23,24,26))  Group BY (Case When Primary2='Y' then GroupID Else UnderGroupID end ) having Groupbal<>0 Order BY DC DESC ,GroupID ASC"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("DC").ToString() = "Dr" And dt.Rows(i)("GroupBal").ToString() > 0 Then
                DgLibilities.Rows.Add()
                With DgLibilities.Rows(lastval)
                    .Cells(0).Value = dt.Rows(i)("GroupID").ToString()
                    .Cells(1).Value = dt.Rows(i)("GroupName").ToString()
                    .Cells(3).Value = dt.Rows(i)("GroupBal").ToString()
                    lastval = lastval + 1
                End With
                sql = "Select ID,Accountname,Area,Opbal,DC,GroupID,OtherName,Mobile1,(Select GroupName From AccountGroup Where ID=Accounts.GroupID) as GroupName,  " &
  "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
  "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
  " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
  " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0  and GroupID=" & Val(dt.Rows(i)("GroupID").ToString()) & " Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    DgLibilities.Rows.Add()
                    With DgLibilities.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("Accountname").ToString()
                        .Cells(2).Value = dt2.Rows(j)("RestBal").ToString()

                        lastval = lastval + 1
                    End With
                Next
            ElseIf dt.Rows(i)("DC").ToString() = "Cr" And dt.Rows(i)("GroupBal").ToString() > 0 Then
                DgLibilities.Rows.Add()
                With DgLibilities.Rows(lastval)
                    .Cells(0).Value = dt.Rows(i)("GroupID").ToString()
                    .Cells(1).Value = dt.Rows(i)("GroupName").ToString()
                    .Cells(3).Value = dt.Rows(i)("GroupBal").ToString()
                    lastval = lastval + 1
                End With
                sql = "Select ID,Accountname,Area,Opbal,DC,GroupID,OtherName,Mobile1,(Select GroupName From AccountGroup Where ID=Accounts.GroupID) as GroupName,  " &
"(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
"-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
" else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
" +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0  and GroupID=" & Val(dt.Rows(i)("GroupID").ToString()) & " Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    DgLibilities.Rows.Add()
                    With DgLibilities.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("Accountname").ToString()
                        .Cells(2).Value = dt2.Rows(j)("RestBal").ToString()

                        lastval = lastval + 1
                    End With
                Next
            End If
        Next
        lastval = 0
        sql = ""
        sql = " Select Primary2,DC,(SELECT GROUP_CONCAT(ID) FROM AccountGroup Where UnderGroupID=GroupID) as IDs," &
            " (Case When Primary2='Y' then GroupName Else UnderGroupName end ) as GroupName,(Case When Primary2='Y' then GroupID Else UnderGroupID end ) as GroupID," &
            "  Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
            "  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))" &
            "  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "') " &
            "  +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end)" &
            "  as  GroupBal from Account_AcGrp  Where (GroupID  in(22,23,24,26)  or UnderGroupID  in(22,23,24,26))  Group BY (Case When Primary2='Y' then GroupID Else UnderGroupID end ) having Groupbal<>0 Order BY DC "
        dt1 = clsFun.ExecDataTable(sql)
        For j As Integer = 0 To dt1.Rows.Count - 1
            If dt1.Rows(j)("DC").ToString() = "Dr" And dt1.Rows(j)("GroupBal").ToString() < 0 Then
                If dgAssests.Rows.Count = 0 Then rowColumsAssests()
                dgAssests.Rows.Add()
                With dgAssests.Rows(lastval)
                    .Cells(0).Value = dt1.Rows(j)("GroupID").ToString()
                    .Cells(1).Value = dt1.Rows(j)("GroupName").ToString()
                    .Cells(3).Value = dt1.Rows(j)("GroupBal").ToString()
                    lastval = lastval + 1
                End With
                sql = "Select ID,Accountname,Area,Opbal,DC,GroupID,OtherName,Mobile1,(Select GroupName From AccountGroup Where ID=Accounts.GroupID) as GroupName,  " &
"(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
"-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
" else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
" +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0  and GroupID=" & Val(dt1.Rows(j)("GroupID").ToString()) & " Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For k As Integer = 0 To dt2.Rows.Count - 1
                    dgAssests.Rows.Add()
                    With dgAssests.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(k)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(k)("Accountname").ToString()
                        .Cells(2).Value = dt2.Rows(k)("RestBal").ToString()
                        lastval = lastval + 1
                    End With
                Next
            ElseIf dt1.Rows(j)("DC").ToString() = "Cr" And dt1.Rows(j)("GroupBal").ToString() < 0 Then
                If dgAssests.Rows.Count = 0 Then rowColumsAssests()
                dgAssests.Rows.Add()
                With dgAssests.Rows(lastval)
                    .Cells(0).Value = dt1.Rows(j)("GroupID").ToString()
                    .Cells(1).Value = dt1.Rows(j)("GroupName").ToString()
                    .Cells(3).Value = dt1.Rows(j)("GroupBal").ToString()
                    lastval = lastval + 1
                End With
                sql = "Select ID,Accountname,Area,Opbal,DC,GroupID,OtherName,Mobile1,(Select GroupName From AccountGroup Where ID=Accounts.GroupID) as GroupName,  " &
"(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
"-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " &
" else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" &
" +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0  and GroupID=" & Val(dt1.Rows(j)("GroupID").ToString()) & " Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For k As Integer = 0 To dt2.Rows.Count - 1
                    dgAssests.Rows.Add()
                    With dgAssests.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(k)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(k)("Accountname").ToString()
                        .Cells(2).Value = dt2.Rows(k)("RestBal").ToString()
                        lastval = lastval + 1
                    End With
                Next
            End If
        Next
    End Sub
    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        ButtonControl()
        RetriveLibilities() : Retrive()
        ButtonControl()
    End Sub
    Private Sub Retrive()
        Dim CrAmount As Decimal
        Dim DrAmount As Decimal
        Dim GPGL As Decimal
        Dim lastval As Integer = 0
        Dim drtotal As Decimal
        dg1.Rows.Clear()
        For i As Integer = 0 To IIf(DgLibilities.Rows.Count > dgAssests.Rows.Count, DgLibilities.Rows.Count, dgAssests.Rows.Count) - 1
            dg1.Rows.Add()
            If DgLibilities.Rows.Count > i Then
                dg1.Rows(i).Cells(0).Value = DgLibilities.Rows(i).Cells(0).Value
                dg1.Rows(i).Cells(1).Value = DgLibilities.Rows(i).Cells(1).Value
                If Val(DgLibilities.Rows(i).Cells(2).Value) > 0 Then
                    dg1.Rows(i).Cells(2).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)) > 0, Format(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)), "0.00"), "")
                Else
                    dg1.Rows(i).Cells(2).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)) > 0, "-" & Format(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)), "0.00"), "")
                End If


                dg1.Rows(i).Cells(8).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(3).Value)) > 0, "Groups", "Accounts")
                dg1.Rows(i).Cells(3).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(3).Value)) > 0, Format(Math.Abs(Val(DgLibilities.Rows(i).Cells(3).Value)), "0.00"), "")
                dg1.Rows(i).Cells(1).Style.Font = IIf(DgLibilities.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(3).Style.Font = IIf(DgLibilities.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(1).Style.Font = IIf(DgLibilities.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Italic), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(2).Style.Font = IIf(DgLibilities.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Underline), New Font("Times New Roman", 9.75, FontStyle.Regular))
                DrAmount = DrAmount + Val(dg1.Rows(i).Cells(3).Value)
            End If
            If dgAssests.Rows.Count > i Then
                dg1.Rows(i).Cells(8).Value = IIf(Math.Abs(Val(dgAssests.Rows(i).Cells(3).Value)) > 0, "Groups", "Accounts")
                dg1.Rows(i).Cells(4).Value = dgAssests.Rows(i).Cells(0).Value
                dg1.Rows(i).Cells(5).Value = dgAssests.Rows(i).Cells(1).Value
                If Val(dgAssests.Rows(i).Cells(2).Value) > 0 Then
                    dg1.Rows(i).Cells(6).Value = IIf(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)) > 0, "-" & Format(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)), "0.00"), "")
                Else

                    dg1.Rows(i).Cells(6).Value = IIf(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)) > 0, Format(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)), "0.00"), "")
                End If

                dg1.Rows(i).Cells(7).Value = IIf(Math.Abs(Val(dgAssests.Rows(i).Cells(3).Value)) > 0, Format(Math.Abs(Val(dgAssests.Rows(i).Cells(3).Value)), "0.00"), "")
                dg1.Rows(i).Cells(5).Style.Font = IIf(dgAssests.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(7).Style.Font = IIf(dgAssests.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(5).Style.Font = IIf(dgAssests.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Italic), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(6).Style.Font = IIf(dgAssests.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Underline), New Font("Times New Roman", 9.75, FontStyle.Regular))
                CrAmount = CrAmount + Val(dg1.Rows(i).Cells(7).Value)
            End If
            lastval = lastval + 1
        Next
        GPGL = Val(DrAmount) - (CrAmount)
        If GPGL <> 0 Then
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                If GPGL < 0 Then
                    drtotal = drtotal + Math.Abs(GPGL)
                    .Cells(1).Value = "Gross Profit (B/F)"
                    .Cells(3).Value = Format(Val(Math.Abs(GPGL)), "0.00")
                    DrAmount = DrAmount + Math.Abs(GPGL)
                    .Cells(1).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(3).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(1).Style.ForeColor = Color.Green
                    .Cells(3).Style.ForeColor = Color.Green
                Else
                    .Cells(5).Value = "Gross Loss (B/F)"
                    .Cells(7).Value = Format(Val(Math.Abs(GPGL)), "0.00")
                    CrAmount = CrAmount + Math.Abs(GPGL)
                    .Cells(5).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(7).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(5).Style.ForeColor = Color.Red
                    .Cells(7).Style.ForeColor = Color.Red
                End If
                lastval = lastval + 1
            End With
        End If
        dg1.Rows.Add()
        With dg1.Rows(lastval)
            .Cells(1).Value = "Total"
            .Cells(3).Value = Format(Val(DrAmount), "0.00")
            .Cells(5).Value = "Total"
            .Cells(7).Value = Format(Val(CrAmount), "0.00")
            .Cells(1).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
            .Cells(3).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
            .Cells(5).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
            .Cells(7).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
            .Cells(1).Style.ForeColor = Color.Blue
            .Cells(2).Style.ForeColor = Color.Blue
            .Cells(3).Style.ForeColor = Color.Blue
            .Cells(4).Style.ForeColor = Color.Blue
            .Cells(5).Style.ForeColor = Color.Blue
            .Cells(6).Style.ForeColor = Color.Blue
            .Cells(7).Style.ForeColor = Color.Blue
        End With
        dg1.ClearSelection()
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")> Private Sub printRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4,P5,P6) values('" & mskEntryDate.Text & "',''," &
                    "'" & .Cells(1).Value & "','" & IIf(Val(.Cells(2).Value) <> 0, Format(Val(.Cells(2).Value), "0.00"), "") & "', '" & IIf(Val(.Cells(3).Value) <> 0, Format(Val(.Cells(3).Value), "0.00"), "") & "'," &
                    "'" & .Cells(5).Value & "','" & IIf(Val(.Cells(6).Value) <> 0, Format(Val(.Cells(6).Value), "0.00"), "") & "','" & IIf(Val(.Cells(7).Value) <> 0, Format(Val(.Cells(7).Value), "0.00"), "") & "');"
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