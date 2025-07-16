Public Class Crate_Outstanding

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.SelectAll()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, btnShow.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub OutStanding_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub OutStanding_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums() : ckHideze.Checked = True : RadioAll.Checked = True
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 6
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Account Name" : dg1.Columns(1).Width = 400
        dg1.Columns(2).Name = "Area" : dg1.Columns(2).Width = 250
        dg1.Columns(3).Name = "Mobile No." : dg1.Columns(3).Width = 250
        dg1.Columns(4).Name = "Balance" : dg1.Columns(4).Width = 280
        dg1.Columns(5).Name = "OtherName" : dg1.Columns(5).Width = 200
        ' retrive()
    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtCustomerSearch.Text.Trim() <> "" Then
                retrive(" And AccountName  Like '" & txtCustomerSearch.Text.Trim() & "%'")
                '   pnlWait.Visible = False
            Else
                retrive()
                '   pnlWait.Visible = False
            End If
            e.SuppressKeyPress = True
        End If

    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        TxtGrandTotal.Clear()
        Dim sql As String = String.Empty
        Dim dt As DataTable
        If RadioAll.Checked = True Then
            sql = "Select ID,Accountname,Area,Mobile1,OtherName, ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as  Restbal from Accounts   where Restbal<>0 " & condtion & "  order by AccountName ;"
        ElseIf RadioCustomer.Checked = True Then
            sql = "Select ID,Accountname,Area,Mobile1,OtherName, ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
"-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as  Restbal from Accounts   where Restbal<0 " & condtion & "  order by AccountName ;"
        ElseIf RadioSupplier.Checked = True Then
            sql = "Select ID,Accountname,Area,Mobile1,OtherName, ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
"-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as  Restbal from Accounts   where Restbal>0 " & condtion & "  order by AccountName ;"

        End If
     
        dt = clsFun.ExecDataTable(sql)
        If dt.rows.count > 20 Then dg1.Columns(4).Width = 250 Else dg1.Columns(4).Width = 280
        'If Val(dt.Rows.Count) = Val(dg1.Rows.Count) Then Exit Sub
        dg1.Rows.Clear()
        For i = 0 To dt.Rows.Count - 1
            'Application.DoEvents()
            lblRecordCount.Visible = True
            lblRecordCount.Text = "Total Records : " & dt.Rows.Count
            dg1.Rows.Add()
            With dg1.Rows(i)
                '  Application.DoEvents()
                dg1.ClearSelection()
                .Cells(0).Value = dt.Rows(i)("ID").ToString()
                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                .Cells(2).Value = dt.Rows(i)("area").ToString()
                .Cells(3).Value = dt.Rows(i)("Mobile1").ToString()
                .Cells(4).Value = IIf(Val(dt.Rows(i)("Restbal").ToString()) > 0, dt.Rows(i)("Restbal").ToString() & " " & "In", Math.Abs(Val(dt.Rows(i)("Restbal").ToString())) & " " & "Out")
                .Cells(5).Value = dt.Rows(i)("OtherName").ToString()
                If Val(dt.Rows(i)("Restbal").ToString()) > 0 Then
                    TxtGrandTotal.Text = Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(4).Value)
                Else
                    TxtGrandTotal.Text = Val(TxtGrandTotal.Text) - Val(dg1.Rows(i).Cells(4).Value)
                End If
                dg1.ClearSelection()
            End With
        Next
        If Val(TxtGrandTotal.Text) > 0 Then TxtGrandTotal.Text = TxtGrandTotal.Text & " In" Else TxtGrandTotal.Text = Math.Abs(Val(TxtGrandTotal.Text)) & " Out"
        pbWait.Visible = False

        'Dim tmpcreditbalance As Decimal = 0.0
        'Dim tmpDebitBalance As Decimal = 0.0
        'Dim tmpTotalBalance As Decimal = 0.0
        'Dim tmpval As Integer = 0
        'Dim amt As Decimal = 0
        'dg1.Rows.Clear()
        'Dim dt As New DataTable
        'dt = clsFun.ExecDataTable("Select AccountID,AccountName FROM CrateVoucher Where EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'" & condtion & " Group by AccountID order by AccountName")

        'If dt.Rows.Count > 0 Then
        '    Application.DoEvents()
        '    For i = 0 To dt.Rows.Count - 1
        '        ' If Val(dt.Rows(i)("AccountId")) = 1141 Then MsgBox("a")
        '        Dim opbal As String = ""
        '        Dim ClBal As String = ""
        '        opbal = Val(0)
        '        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType='Crate In' and accountID=" & Val(dt.Rows(i)("AccountId")) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        '        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType='Crate Out' and accountID=" & Val(dt.Rows(i)("AccountId")) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        '        Dim drcr As String = clsFun.ExecScalarStr(" Select TransType From CrateVoucher Where AccountID=" & Val(dt.Rows(i)("AccountId")) & "")
        '        If drcr = "Crate In" Then
        '            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        '        Else
        '            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        '        End If
        '        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        '        If drcr = "Crate Out" Then
        '            opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " Out"
        '        Else
        '            opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " In"
        '        End If
        '        Dim cntbal As Integer = 0
        '        cntbal = clsFun.ExecScalarInt("Select count(*) from CrateVoucher where  accountid=" & Val(dt.Rows(i)("AccountId")) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        '        If cntbal = 0 Then
        '            opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " " & clsFun.ExecScalarStr(" Select TransType From CrateVoucher Where AccountID=" & Val(dt.Rows(i)("AccountId")) & "")
        '            If drcr = "Dr" Then
        '                tmpDebitBalance = tmpDebitBalance + Format(Val(tmpamt), "0.00")
        '            Else
        '                tmpcreditbalance = tmpcreditbalance + Format(Val(tmpamt), "0.00")

        '            End If
        '        Else
        '            If Val(tmpamtcr) > Val(tmpamtdr) Then
        '                opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " Out"
        '                tmpcreditbalance = tmpcreditbalance + Format(Val(tmpamt), "0.00")
        '            Else
        '                opbal = Math.Round(Math.Abs(Val(tmpamt)), 2) & " In"
        '                tmpDebitBalance = tmpDebitBalance + Format(Val(tmpamt), "0.00")
        '            End If
        '        End If
        '        If ckHideze.Checked = True Then
        '            If tmpamt > 0 Then
        '                dg1.Rows.Add()
        '                With dg1.Rows(tmpval)
        '                    Application.DoEvents()
        '                    .Cells(0).Value = dt.Rows(i)("AccountId").ToString()
        '                    .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
        '                    .Cells(4).Value = opbal
        '                    amt = 0
        '                    tmpval = tmpval + 1
        '                    .Cells(5).Value = clsFun.ExecScalarStr("Select othername  from Accounts where ID=" & dt.Rows(i)("AccountID").ToString() & "")
        '                End With
        '            End If
        '        Else
        '            dg1.Rows.Add()
        '            With dg1.Rows(i)
        '                Application.DoEvents()
        '                .Cells(0).Value = dt.Rows(i)("AccountId").ToString()
        '                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
        '                .Cells(4).Value = opbal
        '                .Cells(5).Value = clsFun.ExecScalarStr("Select othername  from Accounts where ID=" & dt.Rows(i)("AccountID").ToString() & "")
        '            End With
        '        End If
        '    Next i
        'End If
        '' txtDebitBal.Text = Format(Math.Abs(Val(tmpDebitBalance)), "0.00")
        ''txtCreditBal.Text = Format(Math.Abs(Val(tmpcreditbalance)), "0.00")
        'TxtGrandTotal.Text = Format(Math.Abs(Val(tmpDebitBalance - tmpcreditbalance)), "0.00")
        '  calc() : dg1.ClearSelection()
    End Sub
    Sub calc()
        TxtGrandTotal.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(4).Value), "0.00")
        Next
        'lblRecordCount.Visible = True
        'lblRecordCount.Text = "Total Accounts : " & dg1.RowCount
    End Sub
    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        pbWait.Visible = True
        retrive()
        pbWait.Visible = False
    End Sub
    'Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOkOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            Report_Viewer.printReport("\Reports\CrateOutTotal.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(D1,P1, P2,P3,P4) values('" & mskEntryDate.Text & "'," & _
                    "'" & .Cells(1).Value & "','" & .Cells(4).Value & "','" & .Cells(5).Value & "', '" & TxtGrandTotal.Text & "')"

                Try
                    ClsFunPrimary.ExecNonQuery(sql)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            End With
        Next
    End Sub

    'End Sub

    Private Sub btnPrintHindi_Click(sender As Object, e As EventArgs) Handles btnPrintHindi.Click

        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOkOnly, "Empty")
            Exit Sub
        Else
            PrintRecord()
            Report_Viewer.printReport("\Reports\CrateOutTotalHindi.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
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

    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub
End Class