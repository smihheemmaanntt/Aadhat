Imports System.Runtime.InteropServices

Public Class Absent_Account_List
    <DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)>
    Private Shared Function CheckRemoteDebuggerPresent(ByVal hProcess As IntPtr, ByRef isDebuggerPresent As Boolean) As Boolean
    End Function
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
        DetectDebugger.Main()
        Me.Top = 0 : Me.Left = 0
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
        dg1.Columns(1).Width = 300
        dg1.Columns(2).Name = "Last Receipt"
        dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Due Days"
        dg1.Columns(3).Width = 200
        dg1.Columns(4).Name = "Op Bal"
        dg1.Columns(4).Width = 200
        dg1.Columns(5).Name = "Balance"
        dg1.Columns(5).Width = 170
        dg1.Columns(6).Name = "OtherName"
        dg1.Columns(6).Visible = False
        ' retrive()
    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If txtCustomerSearch.Text.Trim() <> "" Then
            retrive("And aAccountName  Like '" & txtCustomerSearch.Text.Trim() & "%'")
        End If
        If txtCustomerSearch.Text.Trim() = "" Then
            retrive()
        End If
    End Sub
    Private Sub retrive(Optional ByVal condtion As String = "")
        '  pnlWait.Visible = True
        Dim dt As DataTable
        dg1.Rows.Clear()
        txtDebitBal.Text = "0.00" : txtCreditBal.Text = "0.00" : TxtGrandTotal.Text = "0.00"
        Dim sql As String = String.Empty
        If RadioSundryDebtors.Checked = True Then
            sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
         "(Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
         " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
         " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end) as  Restbal from Accounts Where RestBal<>0 and GroupID in(16,32) " & condtion & " Order by AccountName ;"
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
            'Application.DoEvents()
            lblRecordCount.Visible = True
            lblRecordCount.Text = "Total Records : " & dt.Rows.Count
            dg1.Rows.Add()
            With dg1.Rows(i)
                ' Application.DoEvents()
                dg1.ClearSelection()
                .Cells(0).Value = dt.Rows(i)("ID").ToString()
                .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                Dim lastdate As String = clsFun.ExecScalarStr("Select EntryDate From Ledger Where DC='C' and AccountID='" & Val(dt.Rows(i)("ID").ToString()) & "' And Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' Order by EntryDate Desc Limit 1 ")
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
        retrive()
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
            MsgBox("There is No record to Print...", vbOkOnly, "Empty")
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
            MsgBox("There is No record to Print...", vbOkOnly, "Empty")
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
End Class