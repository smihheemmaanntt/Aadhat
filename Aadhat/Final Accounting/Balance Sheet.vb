Public Class Balance_Sheet

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
    End Sub
    Private Sub Balance_Sheet_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Balance_Sheet_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        MskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        Me.KeyPreview = True
        rowColums() : rowColumsLibilities() : rowColumsAssests()
        dgAssests.Visible = False : DgLibilities.Visible = False
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Liabilities" : dg1.Columns(1).Width = 317
        dg1.Columns(2).Name = "" : dg1.Columns(2).Width = 120
        dg1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).Name = "Amount" : dg1.Columns(3).Width = 150
        dg1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(4).Name = "ID" : dg1.Columns(4).Visible = False
        dg1.Columns(5).Name = "Assets" : dg1.Columns(5).Width = 317
        dg1.Columns(6).Name = "" : dg1.Columns(6).Width = 120
        dg1.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).Name = "Amount" : dg1.Columns(7).Width = 150
        dg1.Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dg1.Columns(8).Name = "Primary" : dg1.Columns(8).Visible = False
        dg1.Columns(9).Name = "Primary2" : dg1.Columns(9).Visible = False
        ' retrive()
    End Sub
    Private Sub rowColumsLibilities()
        DgLibilities.ColumnCount = 5
        DgLibilities.Columns(0).Name = "ID"
        DgLibilities.Columns(0).Visible = False
        DgLibilities.Columns(1).Name = "PURCHASE"
        DgLibilities.Columns(1).Width = 300
        DgLibilities.Columns(2).Name = ""
        DgLibilities.Columns(2).Width = 200
        DgLibilities.Columns(3).Name = "AMOUNT"
        DgLibilities.Columns(3).Width = 300
        DgLibilities.Columns(4).Name = "Primary"
        DgLibilities.Columns(4).Width = 300
        ' TmpColumns()
        ' retrive()
    End Sub
    Private Sub rowColumsAssests()
        dgAssests.ColumnCount = 5
        dgAssests.Columns(0).Name = "ID"
        dgAssests.Columns(0).Visible = False
        dgAssests.Columns(1).Name = "PURCHASE"
        dgAssests.Columns(1).Width = 340
        dgAssests.Columns(2).Name = ""
        dgAssests.Columns(2).Width = 200
        dgAssests.Columns(3).Name = "AMOUNT"
        dgAssests.Columns(3).Width = 300
        dgAssests.Columns(4).Name = "Primary"
        dgAssests.Columns(4).Width = 300
        ' TmpColumns()
        ' retrive()
    End Sub

    Private Sub MskFromDate_GotFocus(sender As Object, e As EventArgs) Handles MskFromDate.GotFocus, MskFromDate.Click
        MskFromDate.SelectAll()
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus, MsktoDate.Click
        MsktoDate.SelectAll()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles MskFromDate.KeyDown, MsktoDate.KeyDown
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

    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MskFromDate.Validating
        MskFromDate.Text = clsFun.convdate(MskFromDate.Text)
    End Sub
    Private Sub MsktoDateValidating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        MskFromDate.Focus()
    End Sub
    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If MskFromDate.Enabled = False Then Exit Sub
        MskFromDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        MskFromDate.Text = clsFun.convdate(MskFromDate.Text)
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles Dtp2.GotFocus
        MsktoDate.Focus()
    End Sub
    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles Dtp2.ValueChanged
        If MsktoDate.Enabled = False Then Exit Sub
        MsktoDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub RetriveLibilities()
        DgLibilities.Rows.Clear() : dgAssests.Rows.Clear()
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        '''''''''''''''''''GPGL'''''''''''''''''''''''''''''
        Dim GPGl As Decimal = clsFun.ExecScalarDec("  " & _
                                                    " Select Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                                    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                                    " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                                    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  GroupBal from Accounts " & _
                                                    " Where  GroupID  in(22,23,24,26);")

        ''''''''''''''''''''''''''Credit Side/Libilities Side''''''''''''''''''''''''''''
        Dim lastval = 0
        Dim sql As String = "Select Primary2,DC,(Select GroupName From AccountGroup Where ParentID=Account_AcGrp.ParentID) as GroupName,ParentID as GroupID," & _
            "Round(Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0)  From Ledger Where AccountID=Account_AcGrp.ID and DC='D'   " & _
            "and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C'" & _
            "and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
            "Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')+(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
            "Where AccountID=Account_AcGrp.ID  and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end),2)  as  GroupBal from Account_AcGrp " & _
        "Where ParentID  in(1,2,3,4,5,6,7,9,10) Group BY ParentID having Groupbal<>0 Order BY DC,ParentID ASC;"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("DC").ToString() = "Cr" And dt.Rows(i)("GroupBal").ToString() < 0 Then
                DgLibilities.Rows.Add()
                With DgLibilities.Rows(lastval)
                    .Cells(0).Value = dt.Rows(i)("GroupID").ToString()
                    .Cells(1).Value = dt.Rows(i)("GroupName").ToString()
                    .Cells(3).Value = dt.Rows(i)("GroupBal").ToString()
                    lastval = lastval + 1
                End With

                'If dt.Rows(i)("GroupID").ToString() = 3 Then MsgBox("a")
                sql = " Select Primary2,DC,  GroupName,GroupID, Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) " & _
                      " From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
                      " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
                      " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')   +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID " & _
                      " and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end)  as  GroupBal from Account_AcGrp  Where ParentID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") and Primary2<>'Y' Group by GroupID having Groupbal<>0 Order BY DC DESC ,GroupID ASC"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    DgLibilities.Rows.Add()
                    With DgLibilities.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("GroupID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("GroupName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Groupbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next
                sql = " Select ID,Primary2,Accountname,DC,GroupID,(Select GroupName From AccountGroup Where ID=Account_AcGrp.GroupID) as GroupName, " & _
"   (Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where " & _
"  AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
"  Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Account_AcGrp  Where RestBal<>0 and" & _
"  (GroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") or UndergroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ")) and Primary2='Y'   Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    DgLibilities.Rows.Add()
                    With DgLibilities.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("AccountName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Restbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next

            ElseIf dt.Rows(i)("DC").ToString() = "Dr" And dt.Rows(i)("GroupBal").ToString() < 0 Then
                DgLibilities.Rows.Add()
                With DgLibilities.Rows(lastval)
                    .Cells(0).Value = dt.Rows(i)("GroupID").ToString()
                    .Cells(1).Value = dt.Rows(i)("GroupName").ToString()
                    .Cells(3).Value = dt.Rows(i)("GroupBal").ToString()
                    lastval = lastval + 1
                End With
                sql = " Select Primary2,DC,  GroupName,GroupID, Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) " & _
                      " From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
                      " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
                      " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')   +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID " & _
                      " and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end)  as  GroupBal from Account_AcGrp  Where ParentID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") and Primary2<>'Y' Group by GroupID having Groupbal<>0 Order BY DC DESC ,GroupID ASC"
                '             sql = " Select Primary2,DC,  GroupName,GroupID, Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) " & _
                '" From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
                '" Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
                '" Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')   +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID " & _
                '" and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end)  as  GroupBal from Account_AcGrp  Where (GroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") or UndergroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") ) and Primary2<>'Y' Group by GroupID having Groupbal<>0 Order BY DC DESC ,GroupID ASC"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    DgLibilities.Rows.Add()
                    With DgLibilities.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("GroupID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("GroupName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Groupbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next

                sql = " Select ID,Primary2,Accountname,DC,GroupID,(Select GroupName From AccountGroup Where ID=Account_AcGrp.GroupID) as GroupName, " & _
"   (Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where " & _
"  AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
"  Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Account_AcGrp  Where RestBal<>0 and" & _
"  (GroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") or UndergroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ")) and Primary2='Y'   Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    DgLibilities.Rows.Add()
                    With DgLibilities.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("AccountName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Restbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next
            End If
        Next


        ''''''''''''''''''''''''''Debit Side/Assets Side''''''''''''''''''''''''''''
        lastval = 0
        sql = "Select Primary2,DC,(Select GroupName From AccountGroup Where ParentID=Account_AcGrp.ParentID) as GroupName,ParentID as GroupID," & _
            "Round(Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0)  From Ledger Where AccountID=Account_AcGrp.ID and DC='D'   " & _
            "and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C'" & _
            "and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
            "Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')+(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
            "Where AccountID=Account_AcGrp.ID  and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end),2)  as  GroupBal from Account_AcGrp " & _
        "Where ParentID  in(1,2,3,4,5,6,7,9,10) Group BY ParentID having Groupbal<>0 Order BY  DC DESC ,ParentID,Primary2 Desc;"
        dt = clsFun.ExecDataTable(sql)
        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("DC").ToString() = "Dr" And dt.Rows(i)("GroupBal").ToString() > 0 Then
                dgAssests.Rows.Add()
                With dgAssests.Rows(lastval)
                    .Cells(0).Value = dt.Rows(i)("GroupID").ToString()
                    .Cells(1).Value = dt.Rows(i)("GroupName").ToString()
                    .Cells(3).Value = dt.Rows(i)("GroupBal").ToString()
                    lastval = lastval + 1
                End With
                sql = " Select Primary2,DC,  GroupName,GroupID, Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) " & _
                    " From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
                    " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
                    " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')   +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID " & _
                    " and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end)  as  GroupBal from Account_AcGrp  Where ParentID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") and Primary2<>'Y' Group by GroupID having Groupbal<>0 Order BY DC DESC ,GroupID ASC"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    dgAssests.Rows.Add()
                    With dgAssests.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("GroupID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("GroupName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Groupbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next

                sql = " Select ID,Primary2,Accountname,DC,GroupID,(Select GroupName From AccountGroup Where ID=Account_AcGrp.GroupID) as GroupName, " & _
"   (Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where " & _
"  AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
"  Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Account_AcGrp  Where RestBal<>0 and" & _
"  (GroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") or UndergroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ")) and Primary2='Y'   Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    dgAssests.Rows.Add()
                    With dgAssests.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("AccountName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Restbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next

            ElseIf dt.Rows(i)("DC").ToString() = "Cr" And dt.Rows(i)("GroupBal").ToString() > 0 Then
                dgAssests.Rows.Add()
                With dgAssests.Rows(lastval)
                    .Cells(0).Value = dt.Rows(i)("GroupID").ToString()
                    .Cells(1).Value = dt.Rows(i)("GroupName").ToString()
                    .Cells(3).Value = dt.Rows(i)("GroupBal").ToString()
                    lastval = lastval + 1
                End With
                sql = " Select Primary2,DC,  GroupName,GroupID, Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) " & _
             " From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')  -(Select ifnull(Round(Sum(Amount),2),0) From Ledger  " & _
             " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
             " Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')   +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID " & _
             " and DC='D' and Ledger.Entrydate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end)  as  GroupBal from Account_AcGrp  Where ParentID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") and Primary2<>'Y' Group by GroupID having Groupbal<>0 Order BY DC DESC ,GroupID ASC"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    dgAssests.Rows.Add()
                    With dgAssests.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("GroupID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("GroupName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Groupbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next

                sql = " Select ID,Primary2,Accountname,DC,GroupID,(Select GroupName From AccountGroup Where ID=Account_AcGrp.GroupID) as GroupName, " & _
"   (Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' " & _
"  and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where " & _
"  AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "') +(Select ifnull(Round(Sum(Amount),2),0) From Ledger " & _
"  Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Account_AcGrp  Where RestBal<>0 and" & _
"  (GroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ") or UndergroupID in(" & Val(dt.Rows(i)("GroupID").ToString()) & ")) and Primary2='Y'   Order by GroupName,AccountName ;"
                dt2 = clsFun.ExecDataTable(sql)
                For j As Integer = 0 To dt2.Rows.Count - 1
                    dgAssests.Rows.Add()
                    With dgAssests.Rows(lastval)
                        .Cells(0).Value = dt2.Rows(j)("ID").ToString()
                        .Cells(1).Value = "       " & dt2.Rows(j)("AccountName").ToString()
                        .Cells(2).Value = dt2.Rows(j)("Restbal").ToString()
                        .Cells(4).Value = dt2.Rows(j)("Primary2").ToString()
                        lastval = lastval + 1
                    End With
                Next
            End If
        Next
    End Sub
    Private Sub Retrive()
        Dim CrAmount As Decimal
        Dim DrAmount As Decimal
        Dim drtotal As Decimal
        dg1.Rows.Clear()
        '  Dim GPGL As Decimal
        Dim lastval As Integer = 0
        Dim diff As Decimal
        Dim oPNetPL As Decimal = Format(Val(clsFun.ExecScalarStr("  " & _
                                                    " Select (Case When DC='Dr' then ifnull(opbal,0) Else ifnull(-opbal,0) End) from Account_AcGrp " & _
                                                    " Where  ParentID  in(8);")), "0.00")

        Dim NetPL As Decimal = clsFun.ExecScalarDec("  " & _
                                                    " Select Sum(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                                    "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                                    " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                                    " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  GroupBal from Accounts " & _
                                                    " Where  GroupID  in(22,23,24,25,26,27);")

        'Dim NetPL As Decimal = clsFun.ExecScalarDec("  " & _
        '                                            " Select Sum(Case When DC='Dr' then ((Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
        '                                            "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " & _
        '                                            " else (-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
        '                                            " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  GroupBal from Account_AcGrp " & _
        '                                            " Where  ParentID  in(22,23,24,25,26,27);")
        Dim transferedPL As Decimal = clsFun.ExecScalarDec("Select Sum(Case When DC='Dr' then ((Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                            "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                                            " else (-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='C' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                            " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Account_AcGrp.ID and DC='D' and Ledger.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  GroupBal from Account_AcGrp " & _
                                            " Where  ParentID  in(8);")

        For i As Integer = 0 To IIf(DgLibilities.Rows.Count > dgAssests.Rows.Count, DgLibilities.Rows.Count, dgAssests.Rows.Count) - 1
            dg1.Rows.Add()
            If DgLibilities.Rows.Count > i Then
                'dg1.Rows.Add()
                dg1.Rows(i).Cells(0).Value = DgLibilities.Rows(i).Cells(0).Value
                dg1.Rows(i).Cells(1).Value = DgLibilities.Rows(i).Cells(1).Value
                '  If Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)) = 5500 Then MsgBox("a")
                If Val(DgLibilities.Rows(i).Cells(2).Value) < 0 Then
                    dg1.Rows(i).Cells(2).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)) > 0, Format(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)), "0.00"), "")
                Else
                    dg1.Rows(i).Cells(2).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)) > 0, "-" & Format(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)), "0.00"), "")
                End If
                'dg1.Rows(i).Cells(2).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)) > 0, Format(Math.Abs(Val(DgLibilities.Rows(i).Cells(2).Value)), "0.00"), "")
                dg1.Rows(i).Cells(3).Value = IIf(Math.Abs(Val(DgLibilities.Rows(i).Cells(3).Value)) > 0, Format(Math.Abs(Val(DgLibilities.Rows(i).Cells(3).Value)), "0.00"), "")
                dg1.Rows(i).Cells(8).Value = DgLibilities.Rows(i).Cells(4).Value
                dg1.Rows(i).Cells(1).Style.Font = IIf(DgLibilities.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(3).Style.Font = IIf(DgLibilities.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(1).Style.Font = IIf(DgLibilities.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Italic), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(2).Style.Font = IIf(DgLibilities.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Underline), New Font("Times New Roman", 9.75, FontStyle.Regular))
                DrAmount = DrAmount + Val(dg1.Rows(i).Cells(3).Value)
            End If
            If dgAssests.Rows.Count > i Then
                dg1.Rows(i).Cells(4).Value = dgAssests.Rows(i).Cells(0).Value
                dg1.Rows(i).Cells(5).Value = dgAssests.Rows(i).Cells(1).Value
                If Val(dgAssests.Rows(i).Cells(2).Value) < 0 Then
                    dg1.Rows(i).Cells(6).Value = IIf(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)) > 0, "-" & Format(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)), "0.00"), "")
                Else
                    dg1.Rows(i).Cells(6).Value = IIf(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)) > 0, Format(Math.Abs(Val(dgAssests.Rows(i).Cells(2).Value)), "0.00"), "")
                End If
                dg1.Rows(i).Cells(9).Value = dgAssests.Rows(i).Cells(4).Value
                dg1.Rows(i).Cells(7).Value = IIf(Math.Abs(Val(dgAssests.Rows(i).Cells(3).Value)) > 0, Format(Math.Abs(Val(dgAssests.Rows(i).Cells(3).Value)), "0.00"), "")
                dg1.Rows(i).Cells(5).Style.Font = IIf(dgAssests.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(7).Style.Font = IIf(dgAssests.Rows(i).Cells(3).Value = True, New Font("Times New Roman", 10, FontStyle.Bold), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(5).Style.Font = IIf(dgAssests.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Italic), New Font("Times New Roman", 9.75, FontStyle.Regular))
                dg1.Rows(i).Cells(6).Style.Font = IIf(dgAssests.Rows(i).Cells(2).Value = True, New Font("Times New Roman", 10, FontStyle.Underline), New Font("Times New Roman", 9.75, FontStyle.Regular))
                CrAmount = CrAmount + Val(dg1.Rows(i).Cells(7).Value)
            End If
            lastval = lastval + 1
        Next
        GPGL = Val(DrAmount) - Val(CrAmount)

        If oPNetPL <> 0 Then
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                If oPNetPL < 0 Then
                    drtotal = drtotal + Math.Abs(GPGL)
                    .Cells(1).Value = "Net Profit (Opening)"
                    .Cells(3).Value = Format(Val(Math.Abs(oPNetPL)), "0.00")
                    DrAmount = DrAmount + Math.Abs(oPNetPL)
                    .Cells(1).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(3).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(1).Style.ForeColor = Color.Green
                    .Cells(3).Style.ForeColor = Color.Green
                    '.Cells(2).Style.BackColor = Color.Green
                    '.Cells(2).Style.ForeColor = Color.GhostWhite
                    '.Cells(1).Style.BackColor = Color.Green
                    '.Cells(1).Style.ForeColor = Color.GhostWhite
                    '.Cells(3).Style.BackColor = Color.Green
                    '.Cells(3).Style.ForeColor = Color.GhostWhite
                Else
                    .Cells(5).Value = "Net Loss (Opening)"
                    .Cells(7).Value = Format(Val(Math.Abs(oPNetPL)), "0.00")
                    CrAmount = CrAmount + Math.Abs(GPGL)
                    .Cells(5).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(7).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(5).Style.ForeColor = Color.Red
                    .Cells(7).Style.ForeColor = Color.Red
                    '.Cells(5).Style.BackColor = Color.Red
                    '.Cells(5).Style.ForeColor = Color.GhostWhite
                    '.Cells(7).Style.BackColor = Color.Red
                    '.Cells(7).Style.ForeColor = Color.GhostWhite
                    '.Cells(6).Style.BackColor = Color.Red
                    '.Cells(6).Style.ForeColor = Color.GhostWhite
                End If
                lastval = lastval + 1
            End With
        End If
        If lastval >= 20 Then dg1.Columns(7).Width = 125

        If NetPL <> 0 Then
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                If NetPL < 0 Then
                    drtotal = drtotal + Math.Abs(GPGL)
                    .Cells(1).Value = "Net Profit (Current Year)"
                    .Cells(3).Value = Format(Val(Math.Abs(NetPL) - Val(transferedPL)), "0.00")
                    DrAmount = DrAmount + Math.Abs(NetPL)
                    .Cells(1).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(3).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(1).Style.ForeColor = Color.Green
                    .Cells(3).Style.ForeColor = Color.Green
                    '.Cells(2).Style.BackColor = Color.Green
                    '.Cells(2).Style.ForeColor = Color.GhostWhite
                    '.Cells(1).Style.BackColor = Color.Green
                    '.Cells(1).Style.ForeColor = Color.GhostWhite
                    '.Cells(3).Style.BackColor = Color.Green
                    '.Cells(3).Style.ForeColor = Color.GhostWhite
                Else
                    .Cells(5).Value = "Net Loss (Current Year)"
                    .Cells(7).Value = Format(Val(Math.Abs(NetPL)), "0.00")
                    CrAmount = CrAmount + Math.Abs(NetPL)
                    .Cells(5).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(7).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(5).Style.ForeColor = Color.Red
                    .Cells(7).Style.ForeColor = Color.Red
                    '.Cells(5).Style.BackColor = Color.Red
                    '.Cells(5).Style.ForeColor = Color.GhostWhite
                    '.Cells(7).Style.BackColor = Color.Red
                    '.Cells(7).Style.ForeColor = Color.GhostWhite
                    '.Cells(6).Style.BackColor = Color.Red
                    '.Cells(6).Style.ForeColor = Color.GhostWhite
                End If
                lastval = lastval + 1
            End With
        End If

        If transferedPL <> 0 Then
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                If transferedPL > 0 Then
                    drtotal = drtotal + Math.Abs(transferedPL)
                    .Cells(1).Value = "Less: Profit transferred"
                    .Cells(2).Value = Format(Val(transferedPL), "0.00")
                    DrAmount = DrAmount - Math.Abs(transferedPL)
                    .Cells(1).Style.Font = New Font("Times New Roman", 10, FontStyle.Italic)
                    .Cells(2).Style.Font = New Font("Times New Roman", 10, FontStyle.Underline)
                    .Cells(1).Style.ForeColor = Color.Green
                    .Cells(2).Style.ForeColor = Color.Green
                    '.Cells(2).Style.BackColor = Color.Green
                    '.Cells(2).Style.ForeColor = Color.GhostWhite
                    '.Cells(1).Style.BackColor = Color.Green
                    '.Cells(1).Style.ForeColor = Color.GhostWhite
                    '.Cells(3).Style.BackColor = Color.Green
                    '.Cells(3).Style.ForeColor = Color.GhostWhite
                Else
                    .Cells(5).Value = "Add: Loss transferred"
                    .Cells(6).Value = Format(Val(Math.Abs(transferedPL)), "0.00")
                    CrAmount = CrAmount - Math.Abs(transferedPL)
                    .Cells(5).Style.Font = New Font("Times New Roman", 10, FontStyle.Italic)
                    .Cells(6).Style.Font = New Font("Times New Roman", 10, FontStyle.Underline)
                    .Cells(5).Style.ForeColor = Color.Red
                    .Cells(6).Style.ForeColor = Color.Red
                    '.Cells(5).Style.BackColor = Color.Red
                    '.Cells(5).Style.ForeColor = Color.GhostWhite
                    '.Cells(7).Style.BackColor = Color.Red
                    '.Cells(7).Style.ForeColor = Color.GhostWhite
                    '.Cells(6).Style.BackColor = Color.Red
                    '.Cells(6).Style.ForeColor = Color.GhostWhite
                End If
                lastval = lastval + 1
            End With
        End If

        diff = Val(DrAmount) - Val(CrAmount)
        If diff <> 0 Then
            dg1.Rows.Add()
            With dg1.Rows(lastval)
                If diff < 0 Then
                    drtotal = drtotal + Math.Abs(GPGL)
                    .Cells(1).Value = "Diffrence in Opening Balance"
                    .Cells(3).Value = Format(Val(Math.Abs(diff)), "0.00")
                    DrAmount = DrAmount + Math.Abs(diff)
                    .Cells(1).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(3).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(1).Style.ForeColor = Color.Gray
                    .Cells(3).Style.ForeColor = Color.Gray
                    '.Cells(2).Style.BackColor = Color.Green
                    '.Cells(2).Style.ForeColor = Color.GhostWhite
                    '.Cells(1).Style.BackColor = Color.Green
                    '.Cells(1).Style.ForeColor = Color.GhostWhite
                    '.Cells(3).Style.BackColor = Color.Green
                    '.Cells(3).Style.ForeColor = Color.GhostWhite
                Else
                    .Cells(5).Value = "Diffrence in Opening Balance"
                    .Cells(7).Value = Format(Val(Math.Abs(diff)), "0.00")
                    CrAmount = CrAmount + Math.Abs(diff)
                    .Cells(5).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(7).Style.Font = New Font("Times New Roman", 10, FontStyle.Bold)
                    .Cells(5).Style.ForeColor = Color.Gray
                    .Cells(7).Style.ForeColor = Color.Gray
                    '.Cells(5).Style.BackColor = Color.Red
                    '.Cells(5).Style.ForeColor = Color.GhostWhite
                    '.Cells(7).Style.BackColor = Color.Red
                    '.Cells(7).Style.ForeColor = Color.GhostWhite
                    '.Cells(6).Style.BackColor = Color.Red
                    '.Cells(6).Style.ForeColor = Color.GhostWhite
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
        'Retrive()
    End Sub

  

    Private Sub OpenBS(rowIndex As Integer, checkColumnIndex As Integer)
        Dim cell As DataGridViewCell = dg1.Rows(rowIndex).Cells(checkColumnIndex)
        If cell.Value Is Nothing Then
            Exit Sub
        End If

        Dim cellValue As String = cell.Value.ToString()
        ' Check the value and display appropriate message
        Select Case cellValue
            Case "Y"
                Ledger.MdiParent = MainScreenForm
                Ledger.Show()
                Ledger.BringToFront()
                Ledger.cbAccountName.SelectedValue = If(checkColumnIndex = 9, dg1.Rows(rowIndex).Cells(4).Value.ToString(), dg1.Rows(rowIndex).Cells(0).Value.ToString())
                Ledger.mskFromDate.Text = clsFun.convdate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
                Ledger.btnShow.PerformClick()
            Case "N"
                Dim tmpID As String = If(checkColumnIndex = 9, dg1.Rows(rowIndex).Cells(4).Value.ToString(), dg1.Rows(rowIndex).Cells(0).Value.ToString())
                Dim tmpdate As String = MsktoDate.Text
                Group_Summary.MdiParent = MainScreenForm
                Group_Summary.Show() : Group_Summary.BringToFront()
                Group_Summary.retrive(tmpID, tmpdate)
            Case ""
                Exit Sub
        End Select
    End Sub
    Private Sub ProcessCell(rowIndex As Integer, columnIndex As Integer)
        ' Get the value for tmpID based on the column index
        ' Check the cell value in the specified column
        If columnIndex >= 0 And columnIndex <= 3 Then
            ' Call the OpenBS function and pass the row index and column index to check cell 8
            OpenBS(rowIndex, 8)
        ElseIf columnIndex >= 5 And columnIndex <= 8 Then
            ' Call the OpenBS function and pass the row index and column index to check cell 9
            OpenBS(rowIndex, 9)
        End If
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If e.ColumnIndex >= 0 And e.ColumnIndex <= 3 Then
            ' Call the OpenBS function and pass the row index and column index to check cell 8
            OpenBS(e.RowIndex, 8)
        End If

        ' Check if the clicked cell is within the next four columns (5 to 8)
        If e.ColumnIndex >= 5 And e.ColumnIndex <= 7 Then
            ' Call the OpenBS function and pass the row index and column index to check cell 9
            OpenBS(e.RowIndex, 9)
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
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
                sql = sql & "insert into Printing(D1,D2,P1, P2,P3, P4,P5,P6) values('" & MskFromDate.Text & "','" & MsktoDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & IIf(Val(.Cells(2).Value) <> 0, Format(Val(.Cells(2).Value), "0.00"), "") & "', '" & IIf(Val(.Cells(3).Value) <> 0, Format(Val(.Cells(3).Value), "0.00"), "") & "'," & _
                    "'" & .Cells(5).Value & "','" & IIf(Val(.Cells(6).Value) <> 0, Format(Val(.Cells(6).Value), "0.00"), "") & "','" & IIf(Val(.Cells(7).Value) <> 0, Format(Val(.Cells(7).Value), "0.00"), "") & "');"
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


    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            ' Get the current selected cell's row and column indices
            Dim currentRowIndex As Integer = dg1.CurrentCell.RowIndex
            Dim currentColumnIndex As Integer = dg1.CurrentCell.ColumnIndex
            ' Process the cell
            ProcessCell(currentRowIndex, currentColumnIndex)
            ' Prevent the default Enter key behavior
            e.Handled = True
        End If
    End Sub
End Class