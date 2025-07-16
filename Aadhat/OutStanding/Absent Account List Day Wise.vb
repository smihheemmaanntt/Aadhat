Public Class Absent_Account_List_Day_Wise

    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.SelectionStart = 0 : mskEntryDate.SelectionLength = Len(mskEntryDate.Text)
    End Sub
    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub

    Private Sub OutStanding_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub OutStanding_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        RadioSundryDebtors.Checked = True
        rowColums()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 7
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name"
        dg1.Columns(1).Width = 500
        dg1.Columns(2).Name = "Last Nill Date"
        dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Due Days"
        dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Op Bal"
        dg1.Columns(4).Visible = False
        dg1.Columns(5).Name = "Balance"
        dg1.Columns(5).Width = 370
        dg1.Columns(6).Name = "OtherName"
        dg1.Columns(6).Visible = False
        ' retrive()
    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            retrive("And AccountName  Like '" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        '  pnlWait.Visible = True
        dg1.Rows.Clear()
        Dim dt As DataTable : Dim dt1 As DataTable
        Dim BalDate As String
        ': Dim DebitBal As String
        '  Dim Creditbal As String
        ' Dim totbal As String
        Dim tot As Decimal = 0
        Dim lastdate As String
        Dim ssql As String
        txtDebitBal.Text = "0.00" : txtCreditBal.Text = "0.00" : TxtGrandTotal.Text = "0.00"
        Dim sql As String = String.Empty
        If RadioSundryDebtors.Checked = True Then
            sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
         "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0 AND ID=6247 and GroupID in(16,32) " & condtion & " Order by AccountName ;"
        ElseIf RadioSundryCreditors.Checked = True Then
            sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
         "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0 and GroupID in(17,33)  " & condtion & " Order by AccountName ;"
        ElseIf RadioAll.Checked = True Then
            sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
         "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0 " & condtion & " Order by AccountName ;"
        End If
        dt = clsFun.ExecDataTable(sql)
        If Val(dt.Rows.Count) = Val(dg1.Rows.Count) Then Exit Sub
        If Val(dt.Rows.Count) > 20 Then dg1.Columns(5).Width = 150
        dg1.Rows.Clear()
        For i = 0 To dt.Rows.Count - 1
            Application.DoEvents()
            If Application.OpenForms().OfType(Of Absent_Account_List_Day_Wise).Any = False Then Exit Sub
            lblRecordCount.Visible = True
            lblRecordCount.Text = "Total Records : " & dt.Rows.Count
            dg1.Rows.Add()
            tot = 0
            With dg1.Rows(i)
                lastdate = ""
                BalDate = ""
                pb1.Maximum = dt.Rows.Count - 1
                pb1.Value = i
                Application.DoEvents()
                If Application.OpenForms().OfType(Of Absent_Account_List_Day_Wise).Any = False Then Exit Sub
                dg1.ClearSelection()
                .Cells(0).Value = dt.Rows(i)("ID").ToString()
                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()

                ssql = "Select AccountID,VourchersID,Entrydate, TransType,AccountName,sum(Amount) as Dr,'0' as Cr from Ledger where DC ='D'" & _
                       " and AccountID='" & dt.Rows(i)("ID").ToString() & "'  and EntryDate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' Group By EntryDate"
                dt1 = clsFun.ExecDataTable(ssql)

                Try
                    If dt1.Rows.Count > 0 Then
                        For J = 0 To dt1.Rows.Count - 1
                            Dim bal As String = Val(clsFun.ExecScalarStr("select Amount as Cr  from Ledger where Dc='C' and AccountID=" & Val(dt.Rows(i)("ID").ToString()) & " " & _
                                                                          "and Cr='" & dt1.Rows(j)("Dr").ToString() & "'  and EntryDate >='" & CDate(dt1.Rows(j)("EntryDate")).ToString("yyyy-MM-dd") & "'  Order by VourchersID Limit 1;"))
                            If bal <> 0 Then
                                lastdate = dt1.Rows(j + 1)("Entrydate").ToString()
                            Else
                                BalDate = dt1.Rows(0)("Entrydate").ToString()
                            End If
                        Next
                    End If
                Catch ex As Exception

                End Try
                Dim Opbal As String
                ssql = ""
                opbal = clsFun.ExecScalarStr(" Select Round(OpBal,2) FROM Accounts WHERE ID= " & Val(dt.Rows(i)("ID").ToString()) & "")
                ssql = "Select VourchersID,Entrydate,round(Amount,2) as Dr,'0' as Cr from Ledger where DC ='D' and AccountID=" & Val(dt.Rows(i)("ID").ToString()) & "   union all" & _
                       " Select VourchersID,Entrydate,'0' as Dr,round(Amount,2) as Cr  from Ledger where Dc='C' and AccountID=" & Val(dt.Rows(i)("ID").ToString()) & ""
                Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(dt.Rows(i)("ID").ToString()) & "")

                If drcr = "Dr" Then opbal = Val(opbal) Else opbal = -Val(opbal)
                dt1 = clsFun.ExecDataTable(ssql)
                Dim dvData As DataView = New DataView(dt1)
                'dvData.RowFilter = "EntryDate Between '" & mskFromDate.Text & "' And '" & MsktoDate.Text & "'"
                dvData.Sort = " [EntryDate],VourchersID asc"
                dt1 = dvData.ToTable
                Try

                    If dt1.Rows.Count > 0 Then
                        For k = 0 To dt1.Rows.Count - 1
                            If Application.OpenForms().OfType(Of Absent_Account_List_Day_Wise).Any = False Then Exit Sub
                            ' BalDate = dt1.Rows(k)("EntryDate").ToString()
                            If k = 0 Then
                                If Val(dt1.Rows(k)("Dr").ToString()) > 0 Then
                                    If drcr = "Dr" Then
                                        tot = opbal + dt1.Rows(k)("Dr").ToString()
                                    Else
                                        tot = dt1.Rows(k)("Cr").ToString() + opbal
                                    End If
                                Else
                                    If drcr = "Cr" Then
                                        tot = opbal + Val(dt1.Rows(k)("Cr").ToString())
                                    Else
                                        If Val(dt1.Rows(k)("Cr").ToString()) > Val(opbal) Then
                                            tot = Format(Val(Val(dt1.Rows(k)("Cr").ToString()) - Val(opbal)), "0.00")
                                        Else
                                            tot = Format(Val(Val(opbal) - Val(dt1.Rows(k)("Cr").ToString())), "0.00")
                                        End If

                                    End If
                                    If drcr = "Dr" And Val(opbal) > Val(dt1.Rows(k)("Cr").ToString()) Then
                                        tot = Math.Round(Val(tot), 2)
                                    Else
                                        tot = -Val(tot)
                                    End If
                                End If
                            Else
                                tot = tot + IIf(Val(dt1.Rows(k)("Dr").ToString()) > 0, Val(dt1.Rows(k)("Dr").ToString()), -Val(dt1.Rows(k)("Cr").ToString()))
                            End If
                            ' lastdate = dt1.Rows(k)("EntryDate").ToString()
                            If tot = 0 Then
                                lastdate = clsFun.ExecScalarStr("select EntryDate   from Ledger where Dc='D' and AccountID=" & Val(dt.Rows(i)("ID").ToString()) & " " & _
                                                                         " and EntryDate >='" & CDate(dt1.Rows(K)("EntryDate")).ToString("yyyy-MM-dd") & "'  Order by VourchersID Limit 1;")
                                ' lastdate = dt1.Rows(k)("EntryDate").ToString()
                            End If
                            '  BalDate = dt1.Rows(0)("EntryDate").ToString()
                        Next
                    End If
                Catch ex As Exception

                End Try
                lastdate = IIf(lastdate = "", BalDate, lastdate)
                'Dim lastdate As String = clsFun.ExecScalarStr("Select EntryDate From Ledger Where DC='C' and AccountID='" & Val(dt.Rows(i)("ID").ToString()) & "' And Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' Order by EntryDate Desc Limit 1 ")
                If lastdate <> "" Then
                    .Cells(2).Value = CDate(lastdate).ToString("dd-MM-yyyy")
                    Dim LastDate1 As DateTime = CDate(lastdate).ToString("yyyy-MM-dd")
                    Dim CurrDate As DateTime = CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")
                    ' = Date.Parse(CDate(mskEntryDate.Text).ToString("yyyy-MM-dd")).Day - Date.Parse(CDate(lastdate).ToString("yyyy-MM-dd")).Day
                    .Cells(3).Value = DateDiff(DateInterval.Day, LastDate1, CurrDate)
                End If
                .Cells(4).Value = Format(Val(dt.Rows(i)("Opbal").ToString()), "0.00") & "  " & dt.Rows(i)("DC").ToString()
                .Cells(5).Value = IIf(Val(dt.Rows(i)("Restbal").ToString()) > 0, Format(Val(dt.Rows(i)("Restbal").ToString()), "0.00") & " " & "Dr", Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00") & " " & "Cr")
                .Cells(6).Value = dt.Rows(i)("OtherName").ToString()
                If Val(dt.Rows(i)("Restbal").ToString()) > 0 Then
                    txtDebitBal.Text = Format(Val(txtDebitBal.Text) + Val(dt.Rows(i)("Restbal").ToString()), "0.00")
                Else
                    txtCreditBal.Text = Format(Val(txtCreditBal.Text) + Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00")
                End If
                .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
            End With
        Next
        TxtGrandTotal.Text = Val(txtDebitBal.Text) - Val(txtCreditBal.Text)
        TxtGrandTotal.Text = IIf(Val(TxtGrandTotal.Text) > 0, Format(Val(TxtGrandTotal.Text), "0.00") & " " & "Dr", Format(Math.Abs(Val(TxtGrandTotal.Text)), "0.00") & " " & "Cr")
        ' pnlWait.Visible = False

    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        pnlWait.Visible = True
        retrive()
        pnlWait.Visible = False
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            Application.DoEvents()
            If Application.OpenForms().OfType(Of Absent_Account_List).Any = False Then Exit Sub
            With row
                sql = "insert into Printing(D1,P1, P2,P3, P4,P5,P6,P7,P8,P9) values('" & mskEntryDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "','" & .Cells(4).Value & "', " & _
                    "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & Format(Val(txtDebitBal.Text), "0.00") & "'," & _
                    "'" & Format(Val(txtCreditBal.Text), "0.00") & "','" & Format(Val(TxtGrandTotal.Text), "0.00") & "')"
                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Absent_Account_List).Any = False Then Exit Sub
            Report_Viewer.printReport("\AbsentAccounts.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Ledger.MdiParent = MainScreenForm
            Ledger.Show()
            Ledger.ckMerge.Checked = True
            Ledger.cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(0).Value)
            Ledger.BringToFront()
            Ledger.mskFromDate.Text = clsFun.convdate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
            Ledger.btnShow.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Ledger.MdiParent = MainScreenForm
        Ledger.Show()
        Ledger.ckMerge.Checked = True
        Ledger.cbAccountName.SelectedValue = Val(dg1.SelectedRows(0).Cells(0).Value)
        Ledger.BringToFront()
        Ledger.mskFromDate.Text = clsFun.convdate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
        Ledger.btnShow.PerformClick()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPrintHindi_Click(sender As Object, e As EventArgs) Handles btnPrintHindi.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            If Application.OpenForms().OfType(Of Absent_Account_List).Any = False Then Exit Sub
            Report_Viewer.printReport("\AbsentAccountsHindi.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub


    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub
End Class