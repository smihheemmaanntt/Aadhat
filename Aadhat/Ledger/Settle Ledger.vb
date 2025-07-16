Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Threading

Public Class Settle_Ledger
    Dim rs As New Resizer
    Dim strSDate As String : Dim strEDate As String
    Dim dDate As DateTime : Dim mskstartDate As String
    Dim mskenddate As String
    Dim whatsappSender As New WhatsAppSender()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub mskFromDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskFromDate.KeyDown, MsktoDate.KeyDown, cbAccountName.KeyDown

        If cbAccountName.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(cbAccountName, "Select * from Account_AcGrp where (Groupid in(16,17)  or UnderGroupID in (16,17))", "AccountName", "Id", "")
                CreateAccount.BringToFront()
                mindate = clsFun.ExecScalarStr("Select min(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
                maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
                If mindate <> "" Then
                    mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
                Else
                    mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
                End If
                If maxdate <> "" Then
                    MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
                Else
                    MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
                End If
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
            'SendKeys.Send("{TAB}")
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnShow.Focus()
        End Select

    End Sub
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub
    Private Sub Ledger_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        mskFromDate.SelectAll()
    End Sub
    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub
    Private Sub Ledger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        clsFun.FillDropDownList(cbAccountName, "Select * from Account_AcGrp where (Groupid in(16,17)  or UnderGroupID in (16,17))", "AccountName", "Id", "")
        Dim mindate = String.Empty : Dim maxdate As String = String.Empty
        mskFromDate.Text = IIf(mindate <> "", mindate, Date.Today.ToString("dd-MM-yyy"))
        MsktoDate.Text = IIf(maxdate <> "", maxdate, Date.Today.ToString("dd-MM-yyy"))
        rowColums()
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 130
        dg1.Columns(2).Name = "Type" : dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Description" : dg1.Columns(3).Width = 545
        dg1.Columns(4).Name = "Debit" : dg1.Columns(4).Width = 100
        dg1.Columns(5).Name = "Credit" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Balance" : dg1.Columns(6).Width = 150
        dg1.Columns(7).Name = "HindiItem" : dg1.Columns(7).Visible = False
        dg1.Columns(8).Name = "HindiName" : dg1.Columns(8).Visible = False
    End Sub


    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        FillBalanceGrid()
    End Sub
    Private Sub FillBalanceGrid()
        dg1.Rows.Clear()
        Dim Sql As String = String.Empty
        Sql = "Select Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
   "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')) " &
   " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "')" &
   " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <'" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where ID=" & Val(cbAccountName.SelectedValue) & " Order by upper(AccountName) ;"
        Dim opbal As Decimal = Val(clsFun.ExecScalarStr(Sql))
        Dim AccountOtherName As String = clsFun.ExecScalarStr("SELECT OtherName FROM Accounts WHERE ID =" & Val(cbAccountName.SelectedValue) & "")
        txtOpBal.Text = If(opbal >= 0, Format(opbal, "0.00") & " Dr", Format(Math.Abs(opbal), "0.00") & " Cr")
        Dim fromDate As Date = CDate(mskFromDate.Text)
        Dim toDate As Date = CDate(MsktoDate.Text)
        Dim accountId As Integer = Val(cbAccountName.SelectedValue)
        Dim GroupID As Integer = clsFun.ExecScalarInt("SELECT GroupID FROM ACCOUNT_ACGRP WHERE ID='" & Val(cbAccountName.SelectedValue) & "'")
        Dim drSideDt As DataTable = clsFun.ExecDataTable("Select VourchersID,  EntryDate,TransType,Remark, Amount from Ledger where DC ='D' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID = " & accountId & " ORDER BY EntryDate; ")
        Dim crSideDt As DataTable = clsFun.ExecDataTable("Select VourchersID,  EntryDate,TransType,Remark, Amount from Ledger where DC ='C' and EntryDate Between '" & CDate(Me.mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID = " & accountId & " ORDER BY EntryDate; ")
        'Dim drSideDt As DataTable = clsFun.ExecDataTable("SELECT VourchersID, EntryDate, TransType, Remark, Dr as Amount FROM GET_DAYBOOK " &
        '                                            "('" & fromDate.ToString("yyyy-MM-dd") & "', '" & toDate.ToString("yyyy-MM-dd") & "') " &
        '                                             "WHERE AccountID = " & accountId & " AND DR <> 0 ORDER BY EntryDate;")
        'Dim crSideDt As DataTable = clsFun.ExecDataTable("SELECT VourchersID, EntryDate, TransType, Remark, Cr as Amount FROM GET_DAYBOOK " &
        '                                             "('" & fromDate.ToString("yyyy-MM-dd") & "', '" & toDate.ToString("yyyy-MM-dd") & "') " &
        '                                             "WHERE AccountID = " & accountId & " AND CR <> 0 ORDER BY EntryDate, Cr DESC;")
        Dim drIndex As Integer = 0
        Dim crIndex As Integer = 0
        Dim balance As Decimal = opbal
        Dim crTotal As Decimal = 0
        Dim drTotal As Decimal = 0

        ' Process Cr entries first if opening balance is positive
        If GroupID = 16 Or GroupID = 32 Then
            If balance > 0 Then
                While crIndex < crSideDt.Rows.Count AndAlso balance > 0
                    Dim crAmount As Decimal = CDec(crSideDt.Rows(crIndex)("Amount"))
                    If balance >= crAmount Then
                        balance -= crAmount
                        crTotal += crAmount
                        dg1.Rows.Add(crSideDt.Rows(crIndex)("VourchersID").ToString(), CDate(crSideDt.Rows(crIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                                 crSideDt.Rows(crIndex)("TransType").ToString(), crSideDt.Rows(crIndex)("Remark").ToString(),
                                 "", Math.Abs(crAmount).ToString("0.00"), If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                        crIndex += 1
                    Else
                        Exit While
                    End If
                End While
            End If
            ' Process Dr entries
            While drIndex < drSideDt.Rows.Count OrElse crIndex < crSideDt.Rows.Count
                If drIndex < drSideDt.Rows.Count Then
                    Dim drAmount As Decimal = CDec(drSideDt.Rows(drIndex)("Amount"))
                    balance += drAmount
                    drTotal += drAmount
                    dg1.Rows.Add(drSideDt.Rows(drIndex)("VourchersID").ToString(), CDate(drSideDt.Rows(drIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                             drSideDt.Rows(drIndex)("TransType").ToString(), drSideDt.Rows(drIndex)("Remark").ToString(),
                              Math.Abs(drAmount).ToString("0.00"), "", If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                    drIndex += 1

                    ' Process credit entries until balance is >= the next credit amount
                    While crIndex < crSideDt.Rows.Count AndAlso balance > 0
                        Dim crAmount As Decimal = CDec(crSideDt.Rows(crIndex)("Amount"))
                        If balance >= crAmount Then
                            balance -= crAmount
                            crTotal += crAmount
                            dg1.Rows.Add(crSideDt.Rows(crIndex)("VourchersID").ToString(), CDate(crSideDt.Rows(crIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                                     crSideDt.Rows(crIndex)("TransType").ToString(), crSideDt.Rows(crIndex)("Remark").ToString(),
                                     "", Math.Abs(crAmount).ToString("0.00"), If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                            crIndex += 1
                        Else
                            Exit While
                        End If
                    End While
                ElseIf crIndex < crSideDt.Rows.Count Then
                    Dim crAmount As Decimal = CDec(crSideDt.Rows(crIndex)("Amount"))
                    balance -= crAmount
                    crTotal += crAmount
                    dg1.Rows.Add(crSideDt.Rows(crIndex)("VourchersID").ToString(), CDate(crSideDt.Rows(crIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                             crSideDt.Rows(crIndex)("TransType").ToString(), crSideDt.Rows(crIndex)("Remark").ToString(),
                             "", Math.Abs(crAmount).ToString("0.00"), If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                    crIndex += 1
                End If
            End While
        Else
            ' If the opening balance is Cr or zero
            If balance <= 0 Then
                ' Process Dr entries first until balance is zero or positive
                While drIndex < drSideDt.Rows.Count AndAlso balance <= 0
                    Dim drAmount As Decimal = CDec(drSideDt.Rows(drIndex)("Amount"))
                    balance += drAmount
                    drTotal += drAmount
                    dg1.Rows.Add(drSideDt.Rows(drIndex)("VourchersID").ToString(), CDate(drSideDt.Rows(drIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                                 drSideDt.Rows(drIndex)("TransType").ToString(), drSideDt.Rows(drIndex)("Remark").ToString(),
                                 Math.Abs(drAmount).ToString("0.00"), "", If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                    drIndex += 1

                    ' Ensure balance does not become positive
                    If balance > 0 Then
                        balance -= drAmount
                        drTotal -= drAmount
                        dg1.Rows.RemoveAt(dg1.Rows.Count - 1)
                        drIndex -= 1
                        Exit While
                    End If
                End While

                ' If Dr entries caused balance to be zero or positive, process remaining Cr entries
                If balance <= 0 Then
                    ' Process Cr entries
                    While crIndex < crSideDt.Rows.Count
                        Dim crAmount As Decimal = CDec(crSideDt.Rows(crIndex)("Amount"))
                        balance -= crAmount
                        crTotal += crAmount
                        dg1.Rows.Add(crSideDt.Rows(crIndex)("VourchersID").ToString(), CDate(crSideDt.Rows(crIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                                     crSideDt.Rows(crIndex)("TransType").ToString(), crSideDt.Rows(crIndex)("Remark").ToString(),
                                     "", Math.Abs(crAmount).ToString("0.00"), If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                        crIndex += 1

                        ' Process Dr entries if balance becomes negative after a credit entry
                        While drIndex < drSideDt.Rows.Count AndAlso balance < 0
                            Dim drAmount As Decimal = CDec(drSideDt.Rows(drIndex)("Amount"))
                            balance += drAmount
                            drTotal += drAmount
                            dg1.Rows.Add(drSideDt.Rows(drIndex)("VourchersID").ToString(), CDate(drSideDt.Rows(drIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                                         drSideDt.Rows(drIndex)("TransType").ToString(), drSideDt.Rows(drIndex)("Remark").ToString(),
                                         Math.Abs(drAmount).ToString("0.00"), "", If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                            drIndex += 1

                            ' Ensure balance does not become positive
                            If balance > 0 Then
                                balance -= drAmount
                                drTotal -= drAmount
                                dg1.Rows.RemoveAt(dg1.Rows.Count - 1)
                                drIndex -= 1
                                Exit While
                            End If
                        End While
                    End While
                End If
            End If

            ' Process remaining Dr entries if balance is still negative after all Cr entries
            While drIndex < drSideDt.Rows.Count
                Dim drAmount As Decimal = CDec(drSideDt.Rows(drIndex)("Amount"))
                balance += drAmount
                drTotal += drAmount
                dg1.Rows.Add(drSideDt.Rows(drIndex)("VourchersID").ToString(), CDate(drSideDt.Rows(drIndex)("EntryDate")).ToString("dd-MM-yyyy"),
                             drSideDt.Rows(drIndex)("TransType").ToString(), drSideDt.Rows(drIndex)("Remark").ToString(),
                             Math.Abs(drAmount).ToString("0.00"), "", If(balance = 0, "Nill", If(balance > 0, balance.ToString("0.00") & " Dr", Math.Abs(balance).ToString("0.00") & " Cr")))
                drIndex += 1
            End While
        End If





        drSideDt.Dispose()
        crSideDt.Dispose()
        txtDramt.Text = Format(drTotal, "0.00") & " Dr"
        txtcrAmt.Text = Format(crTotal, "0.00") & " Cr"
        txtBalAmt.Text = If(balance = 0, "NILL", If(balance > 0, Format(balance, "0.00") & " Dr", Format(Math.Abs(balance), "0.00") & " Cr"))
        dg1.ClearSelection()
    End Sub
    Private Sub cbAccountName_Leave(sender As Object, e As EventArgs) Handles cbAccountName.Leave
        If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & cbAccountName.Text & "'") = 0 Then
            MsgBox("Account Name Not Found in Database...", vbOKOnly, "Access Denied")
            clsFun.FillDropDownList(cbAccountName, "Select * from Account_AcGrp where (Groupid in(16,17)  or UnderGroupID in (16,17))", "AccountName", "Id", "")
            cbAccountName.Focus()
            Exit Sub
        End If
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
        maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger Where AccountID=" & Val(cbAccountName.SelectedValue) & "")
        If mindate <> "" Then
            mskFromDate.Text = CDate(mindate).ToString("dd-MM-yyyy")
        Else
            mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
        If maxdate <> "" Then
            MsktoDate.Text = CDate(maxdate).ToString("dd-MM-yyyy")
        Else
            MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        End If
    End Sub

    Private Sub cbAccountName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbAccountName.SelectedIndexChanged

    End Sub

    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub PrintRecord()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")

        ' Ensure cross-thread safety when accessing UI elements
        Dim cbAccountNameText As String = String.Empty
        Dim mskFromDateText As String = String.Empty
        Dim mskToDateText As String = String.Empty
        Dim txtOpBalText As String = String.Empty
        Dim txtDramtText As String = String.Empty
        Dim txtcrAmtText As String = String.Empty
        Dim txtBalAmtText As String = String.Empty
        Dim lblCrateText As String = String.Empty
        Dim lblCrateDetailsText As String = String.Empty

        ' Safely retrieve UI values on the UI thread
        If cbAccountName.InvokeRequired Then
            cbAccountName.Invoke(Sub()
                                     cbAccountNameText = cbAccountName.Text
                                     mskFromDateText = mskFromDate.Text
                                     mskToDateText = MsktoDate.Text
                                     txtOpBalText = txtOpBal.Text
                                     txtDramtText = txtDramt.Text
                                     txtcrAmtText = txtcrAmt.Text
                                     txtBalAmtText = txtBalAmt.Text
                                     lblCrateText = lblCrate.Text
                                     lblCrateDetailsText = lblCrateDetails.Text
                                 End Sub)
        Else
            cbAccountNameText = cbAccountName.Text
            mskFromDateText = mskFromDate.Text
            mskToDateText = MsktoDate.Text
            txtOpBalText = txtOpBal.Text
            txtDramtText = txtDramt.Text
            txtcrAmtText = txtcrAmt.Text
            txtBalAmtText = txtBalAmt.Text
            lblCrateText = lblCrate.Text
            lblCrateDetailsText = lblCrateDetails.Text
        End If

        If dg1.RowCount <> 0 Then
            For i As Integer = 0 To maxRowCount - 1
                FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
                For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                    With dg1.Rows(LastRecord)
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDateText & "'," &
                            "'" & mskToDateText & "','" & cbAccountNameText & "','" & txtOpBalText & "','" & txtDramtText & "','" & txtcrAmtText & "'," &
                            "'" & txtBalAmtText & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & IIf(ckPrintHindi.Checked = True, .Cells("HindiItem").Value, .Cells("Description").Value) & "'," &
                            "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','" & lblCrateText & "','" & lblCrateDetailsText & "','" & Val(LastRecord) + 1 & "'"
                    End With
                    LastRecord = Val(LastRecord + 1)
                Next
                Try
                    If FastQuery = String.Empty Then Exit Sub
                    sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2, P4, P5, P6,P7,P8,P9,P10,P11) " & FastQuery & ""
                    ClsFunPrimary.ExecNonQuery(sQL)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            Next
        Else
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDateText & "'," &
                "'" & mskToDateText & "','" & cbAccountNameText & "','" & txtOpBalText & "','" & txtDramtText & "', " &
                "'" & txtcrAmtText & "','" & txtBalAmtText & "','" & lblCrateText & "','" & lblCrateDetailsText & "'"
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5,P9,P10) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        End If
    End Sub
    Private Sub WithoutDiscriptionPrint()
        Dim AllRecord As Integer = Val(dg1.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim FastQuery As String = String.Empty
        Dim sQL As String = String.Empty
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        If dg1.RowCount <> 0 Then
            For i As Integer = 0 To maxRowCount - 1
                Application.DoEvents()
                FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
                For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                    With dg1.Rows(LastRecord)
                        Dim cbAccountNameText As String = InvokeIfRequired(Function() cbAccountName.Text)
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," &
                            "'" & MsktoDate.Text & "','" & cbAccountNameText & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," &
                            "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','',''," &
                            "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
                    End With
                    LastRecord = Val(LastRecord + 1)
                Next
                ' LastRecord = LastCount
                Try
                    If FastQuery = String.Empty Then Exit Sub
                    sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) " & FastQuery & ""
                    ClsFunPrimary.ExecNonQuery(sQL)
                Catch ex As Exception
                    MsgBox(ex.Message)
                    ClsFunPrimary.CloseConnection()
                End Try
            Next
        Else
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskFromDate.Text & "'," &
                "'" & MsktoDate.Text & "','" & cbAccountName.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "', " &
                "'" & txtcrAmt.Text & "','" & txtBalAmt.Text & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "'"
            Try
                If FastQuery = String.Empty Then Exit Sub
                sQL = "insert into Printing(D1,D2,M1,M2, M3, M4, M5,P9,P10) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(sQL)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        End If
    End Sub

    Private Function InvokeIfRequired(Of T)(func As Func(Of T)) As T
        If Me.InvokeRequired Then
            Return CType(Me.Invoke(func), T)
        Else
            Return func()
        End If
    End Function

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click
        ' clsFun.changeCompany()
        If ckJoin.Checked = True Then
            WithoutDiscriptionPrint()
        Else
            PrintRecord()
        End If
        If ckPrintHindi.Checked = True Then
            Report_Viewer.printReport("\Ledger2.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        Else
            '    PrintRecord()
            Report_Viewer.printReport("\Ledger.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If

    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim id As Integer = dg1.SelectedRows(0).Cells(0).Value
        Dim type As String = dg1.SelectedRows(0).Cells(2).Value
        If type = "Stock Sale" Then
            Stock_Sale.MdiParent = MainScreenForm
            Stock_Sale.Show()
            Stock_Sale.FillControls(id)
            If Not Stock_Sale Is Nothing Then
                Stock_Sale.BringToFront()
            End If
        ElseIf type = "Receipt" Then
            ReceiptForm.MdiParent = MainScreenForm
            ReceiptForm.Show()
            ReceiptForm.FillControls(id)
            If Not ReceiptForm Is Nothing Then
                ReceiptForm.BringToFront()
            End If
        ElseIf type = "Payment" Then
            PayMentform.MdiParent = MainScreenForm
            PayMentform.Show()
            PayMentform.FillControls(id)
            If Not PayMentform Is Nothing Then
                PayMentform.BringToFront()
            End If
        ElseIf type = "Purchase" Then
            Purchase.MdiParent = MainScreenForm
            Purchase.Show()
            Purchase.FillControls(id)
            Purchase.BringToFront()
        ElseIf type = "Purchase (Loose)" Then
            Loose_Purchase.MdiParent = MainScreenForm
            Loose_Purchase.Show()
            Loose_Purchase.FillControls(id)
            Loose_Purchase.BringToFront()
        ElseIf type = "Sale (Loose)" Then
            Loose_Sale.MdiParent = MainScreenForm
            Loose_Sale.Show()
            Loose_Sale.FillControls(id)
            Loose_Sale.BringToFront()
        ElseIf type = "Super Sale" Then
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(id)
            If Not Super_Sale Is Nothing Then
                Super_Sale.BringToFront()
            End If
        ElseIf type = "Speed Sale" Then
            SpeedSale.MdiParent = MainScreenForm
            SpeedSale.Show()
            SpeedSale.FillContros(id)
            If Not SpeedSale Is Nothing Then
                SpeedSale.BringToFront()
            End If
        ElseIf type = "Standard Sale" Then
            Standard_Sale.MdiParent = MainScreenForm
            Standard_Sale.Show()
            Standard_Sale.FillControls(id)
            If Not Standard_Sale Is Nothing Then
                Standard_Sale.BringToFront()
            End If
        ElseIf type = "Super Sale" Then
            Super_Sale.MdiParent = MainScreenForm
            Super_Sale.Show()
            Super_Sale.FillControls(id)
            If Not Super_Sale Is Nothing Then
                Super_Sale.BringToFront()
            End If
        ElseIf type = "Auto Beejak" Then
            Sellout_Auto.MdiParent = MainScreenForm
            Sellout_Auto.Show()
            Sellout_Auto.FillFromData(id)
            If Not Sellout_Auto Is Nothing Then
                Sellout_Auto.BringToFront()
            End If
        ElseIf type = "Beejak" Then
            Sellout_Mannual.MdiParent = MainScreenForm
            Sellout_Mannual.Show()
            Sellout_Mannual.FillContros(id)
            Sellout_Mannual.BringToFront()
        ElseIf type = "Crate In" Then
            Crate_IN.MdiParent = MainScreenForm
            Crate_IN.Show()
            Crate_IN.FillControls(id)
            If Not Crate_IN Is Nothing Then
                Crate_IN.BringToFront()
            End If
        ElseIf type = "Crate Out" Then
            Crate_Out.MdiParent = MainScreenForm
            Crate_Out.Show()
            Crate_Out.FillControls(id)
            If Not Crate_Out Is Nothing Then
                Crate_Out.BringToFront()
            End If
        ElseIf type = "Journal" Then
            JournalEntry.MdiParent = MainScreenForm
            JournalEntry.Show()
            JournalEntry.FillContros(id)
            If Not JournalEntry Is Nothing Then
                JournalEntry.BringToFront()
            End If
        ElseIf type = "On Sale" Then
            On_Sale.MdiParent = MainScreenForm
            On_Sale.Show()
            On_Sale.FillControl(id)
            If Not On_Sale Is Nothing Then
                On_Sale.BringToFront()
            End If
        ElseIf type = "On Sale Receipt" Then
            On_Sale_Receipt.MdiParent = MainScreenForm
            On_Sale_Receipt.Show()
            On_Sale_Receipt.FillControl(id)
            If Not On_Sale_Receipt Is Nothing Then
                On_Sale_Receipt.BringToFront()
            End If
        ElseIf type = "Net Receipt" Then
            OnSaleReceipt_Net.MdiParent = MainScreenForm
            OnSaleReceipt_Net.Show()
            OnSaleReceipt_Net.FillControls(id)
            OnSaleReceipt_Net.BringToFront()
        ElseIf type = "Group Receipt" Then
            Group_Receipt.MdiParent = MainScreenForm
            Group_Receipt.Show()
            Group_Receipt.FillControls(id)
            If Not Group_Receipt Is Nothing Then
                Group_Receipt.BringToFront()
            End If
        ElseIf type = "Group Payment" Then
            Group_Payment.MdiParent = MainScreenForm
            Group_Payment.Show()
            Group_Payment.FillControls(id)
            If Not Group_Payment Is Nothing Then
                Group_Payment.BringToFront()
            End If
        Else
            Bank_Entry.MdiParent = MainScreenForm
            Bank_Entry.Show()
            Bank_Entry.FillControls(id)
            If Not Bank_Entry Is Nothing Then
                Bank_Entry.BringToFront()
            End If
        End If
    End Sub

    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Dim id As Integer = dg1.SelectedRows(0).Cells(0).Value
            Dim type As String = dg1.SelectedRows(0).Cells(2).Value
            If type = "Stock Sale" Then
                Stock_Sale.MdiParent = MainScreenForm
                Stock_Sale.Show()
                Stock_Sale.FillControls(id)
                If Not Stock_Sale Is Nothing Then
                    Stock_Sale.BringToFront()
                End If
            ElseIf type = "Receipt" Then
                ReceiptForm.MdiParent = MainScreenForm
                ReceiptForm.Show()
                ReceiptForm.FillControls(id)
                If Not ReceiptForm Is Nothing Then
                    ReceiptForm.BringToFront()
                End If
            ElseIf type = "Payment" Then
                PayMentform.MdiParent = MainScreenForm
                PayMentform.Show()
                PayMentform.FillControls(id)
                If Not PayMentform Is Nothing Then
                    PayMentform.BringToFront()
                End If
            ElseIf type = "Purchase" Then
                Purchase.MdiParent = MainScreenForm
                Purchase.Show()
                Purchase.FillControls(id)
                Purchase.BringToFront()
            ElseIf type = "Purchase (Loose)" Then
                Loose_Purchase.MdiParent = MainScreenForm
                Loose_Purchase.Show()
                Loose_Purchase.FillControls(id)
                Loose_Purchase.BringToFront()
            ElseIf type = "Sale (Loose)" Then
                Loose_Sale.MdiParent = MainScreenForm
                Loose_Sale.Show()
                Loose_Sale.FillControls(id)
                Loose_Sale.BringToFront()
            ElseIf type = "Super Sale" Then
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.Show()
                Super_Sale.FillControls(id)
                If Not Super_Sale Is Nothing Then
                    Super_Sale.BringToFront()
                End If
            ElseIf type = "Speed Sale" Then
                SpeedSale.MdiParent = MainScreenForm
                SpeedSale.Show()
                SpeedSale.FillContros(id)
                If Not SpeedSale Is Nothing Then
                    SpeedSale.BringToFront()
                End If
            ElseIf type = "Standard Sale" Then
                Standard_Sale.MdiParent = MainScreenForm
                Standard_Sale.Show()
                Standard_Sale.FillControls(id)
                If Not Standard_Sale Is Nothing Then
                    Standard_Sale.BringToFront()
                End If
            ElseIf type = "Super Sale" Then
                Super_Sale.MdiParent = MainScreenForm
                Super_Sale.Show()
                Super_Sale.FillControls(id)
                If Not Super_Sale Is Nothing Then
                    Super_Sale.BringToFront()
                End If
            ElseIf type = "Auto Beejak" Then
                Sellout_Auto.MdiParent = MainScreenForm
                Sellout_Auto.Show()
                Sellout_Auto.FillFromData(id)
                If Not Sellout_Auto Is Nothing Then
                    Sellout_Auto.BringToFront()
                End If
            ElseIf type = "Beejak" Then
                Sellout_Mannual.MdiParent = MainScreenForm
                Sellout_Mannual.Show()
                Sellout_Mannual.FillContros(id)
                Sellout_Mannual.BringToFront()
            ElseIf type = "Crate In" Then
                Crate_IN.MdiParent = MainScreenForm
                Crate_IN.Show()
                Crate_IN.FillControls(id)
                If Not Crate_IN Is Nothing Then
                    Crate_IN.BringToFront()
                End If
            ElseIf type = "Crate Out" Then
                Crate_Out.MdiParent = MainScreenForm
                Crate_Out.Show()
                Crate_Out.FillControls(id)
                If Not Crate_Out Is Nothing Then
                    Crate_Out.BringToFront()
                End If
            ElseIf type = "Journal" Then
                JournalEntry.MdiParent = MainScreenForm
                JournalEntry.Show()
                JournalEntry.FillContros(id)
                If Not JournalEntry Is Nothing Then
                    JournalEntry.BringToFront()
                End If
            ElseIf type = "On Sale" Then
                On_Sale.MdiParent = MainScreenForm
                On_Sale.Show()
                On_Sale.FillControl(id)
                If Not On_Sale Is Nothing Then
                    On_Sale.BringToFront()
                End If
            ElseIf type = "On Sale Receipt" Then
                On_Sale_Receipt.MdiParent = MainScreenForm
                On_Sale_Receipt.Show()
                On_Sale_Receipt.FillControl(id)
                If Not On_Sale_Receipt Is Nothing Then
                    On_Sale_Receipt.BringToFront()
                End If
            ElseIf type = "Net Receipt" Then
                OnSaleReceipt_Net.MdiParent = MainScreenForm
                OnSaleReceipt_Net.Show()
                OnSaleReceipt_Net.FillControls(id)
                OnSaleReceipt_Net.BringToFront()
            ElseIf type = "Group Receipt" Then
                Group_Receipt.MdiParent = MainScreenForm
                Group_Receipt.Show()
                Group_Receipt.FillControls(id)
                If Not Group_Receipt Is Nothing Then
                    Group_Receipt.BringToFront()
                End If
            ElseIf type = "Group Payment" Then
                Group_Payment.MdiParent = MainScreenForm
                Group_Payment.Show()
                Group_Payment.FillControls(id)
                If Not Group_Payment Is Nothing Then
                    Group_Payment.BringToFront()
                End If
            Else
                Bank_Entry.MdiParent = MainScreenForm
                Bank_Entry.Show()
                Bank_Entry.FillControls(id)
                If Not Bank_Entry Is Nothing Then
                    Bank_Entry.BringToFront()
                End If
            End If
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'clsFun.changeCompany()
        PrintRecord()
        Report_Viewer.printReport("\Ledger-DayWise.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub

    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
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
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        txtWhatsappNo.Text = clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(cbAccountName.SelectedValue) & "'")
        pnlWahtsappNo.Visible = True : txtWhatsappNo.Focus()
        If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
            cbType.SelectedIndex = 0
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            End If
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
        Else
            cbType.SelectedIndex = 1
        End If
    End Sub
    Private Sub WhatsAppDesktop()
        Dim GAP1 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP1 From API")) & "000"
        Dim GAP2 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP2 From API")) & "000"
        Dim whatsappURL As String = String.Empty
        Dim sourceFilePath As String = String.Empty
        Try
            whatsappURL = "whatsapp://send?"
            Dim psi As New ProcessStartInfo(whatsappURL)
            Dim process As Process = process.Start(psi)
        Catch ex As Exception
            MsgBox(ex.Message)
            Exit Sub
        End Try
        If Directory.Exists(Application.StartupPath & "\Whatsapp\Pdfs") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\Whatsapp\Pdfs")
        End If
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        PrintRecord()
        GlobalData.PdfName = cbAccountName.Text & " (" & mskFromDate.Text & " to " & MsktoDate.Text & ").pdf"
        If ckPrintHindi.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger2.rpt")
        Else
            Pdf_Genrate.ExportReport("\Ledger.rpt")
        End If
        sourceFilePath = GlobalData.PdfPath
        whatsappURL = "whatsapp://send?phone=91" & txtWhatsappNo.Text.Trim & ""
        Dim psi1 As New ProcessStartInfo(whatsappURL)
        psi1.UseShellExecute = True
        psi1.WindowStyle = ProcessWindowStyle.Normal
        Dim process1 As Process = Process.Start(psi1)
        psi1.WindowStyle = ProcessWindowStyle.Minimized
        Thread.Sleep(GAP1)
        SendKeys.SendWait("^(+f)")
        SendKeys.SendWait("{ESCAPE}")
        Clipboard.SetData(DataFormats.FileDrop, {sourceFilePath})
        ' If i = 0 Then Thread.Sleep(1000)
        SendKeys.SendWait("^(v)")
        Thread.Sleep(GAP2)
        SendKeys.SendWait("{ENTER}")
        Thread.Sleep(GAP2)
        Dim processName As String = "WhatsApp"
        Dim proc As New ProcessStartInfo(processName)
        Dim processes() As Process = Process.GetProcessesByName(processName)
        If processes.Length > 0 Then
            ' Close each instance of the process
            For Each p As Process In processes
                Thread.Sleep(GAP2)
                p.Kill() : pnlWahtsappNo.Visible = False : cbAccountName.Focus() : cbAccountName.SelectAll()
                'proc.WindowStyle = ProcessWindowStyle.Minimized
            Next
            MsgBox("Ledger Send to " & cbAccountName.Text & " via WhatsApp Successful", MsgBoxStyle.Information, "Ledger Sent")

        Else
            ' The process was not found
            Console.WriteLine("Process not found.")
        End If
    End Sub
    Private Sub WahSoft()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.pdf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        WABA.ExecNonQuery("Delete from SendingData")
        GlobalData.PdfName = mskFromDate.Text & ".pdf"
        PrintRecord()
        If ckPrintHindi.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger2.rpt")
        Else
            Pdf_Genrate.ExportReport("\Ledger.rpt")
        End If
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(0) & "','','" & txtWhatsappNo.Text & "','" & whatsappSender.FilePath & "');Update Settings Set MinState='N'"
        WABA.ExecNonQuery(sql)
        '  MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")

        'MsgBox("Data Send to Easy WhatsApp Successfully...", vbInformation, "Sended On Easy Whatsapp")

        Dim WhatsappFile As String = Application.StartupPath & "\WahSoft\WahSoft.exe"
        If System.IO.File.Exists(WhatsappFile) = False Then
            MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
            Exit Sub
        End If
        Dim p() As Process
        p = Process.GetProcessesByName("WahSoft")
        If p.Count = 0 Then
            Dim StartWhatsapp As New System.Diagnostics.Process
            StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\WahSoft\WahSoft.exe"
            StartWhatsapp.Start()
        End If

    End Sub
    Private Sub SendWhatsappData()
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        GlobalData.PdfName = cbAccountName.Text & "-" & mskFromDate.Text & ".pdf"
        PrintRecord()
        If ckPrintHindi.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger2.rpt")
        Else
            Pdf_Genrate.ExportReport("\Ledger.rpt")
        End If
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " &
         "('" & Val(cbAccountName.SelectedValue) & "','" & cbAccountName.Text.Replace("/", "") & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        pnlWahtsappNo.Visible = False : cbAccountName.Focus()
    End Sub

    Private Sub UsingWhatsappAPI()
        If txtWhatsappNo.Text <= "" Then lblStatus.Visible = False : Exit Sub
        lblStatus.Visible = False
        Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")

        'pnlWahtsappNo.Visible = True
        'txtWhatsappNo.Focus()
        GlobalData.PdfName = cbAccountName.Text & "-" & mskFromDate.Text & ".pdf"
        PrintRecord()
        If ckPrintHindi.Checked = True Then
            Pdf_Genrate.ExportReport("\Ledger2.rpt")
        Else
            Pdf_Genrate.ExportReport("\Ledger.rpt")
        End If
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Whatsapp\Pdfs\" & GlobalData.PdfName)
        whatsappSender.SendWhatsAppFile("91" & txtWhatsappNo.Text, "Sended By: Aadhat Software" & vbCrLf & "www.softmanagementindia.in", FilePath)
        lblStatus.Text = "PDF Sent " & whatsappSender.APIResposne
        lblStatus.Visible = True
        sql = "insert into waReport(EntryDate,AccountName,WhatsAppNo,Type,Status) SELECT '" & Date.Today.ToString("yyyy-MM-dd") & "','" & cbAccountName.Text.Replace("/", "") & "','" & txtWhatsappNo.Text & "','Ledger','" & lblStatus.Text & "'"
        clsFun.ExecNonQuery(sql)
        pnlWahtsappNo.Visible = False : cbAccountName.Focus()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(cbAccountName.SelectedValue) & "'") = "" And txtWhatsappNo.Text <> "" Then
            clsFun.ExecScalarStr("Update Accounts set Mobile1='" & txtWhatsappNo.Text & "' Where ID='" & Val(cbAccountName.SelectedValue) & "'")
        Else
            If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(cbAccountName.SelectedValue) & "'") <> txtWhatsappNo.Text Then
                If MessageBox.Show("Are you Sure to Change Mobile No In PhoneBook", "Change Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    clsFun.ExecScalarStr("Update Accounts set Mobile1='" & txtWhatsappNo.Text & "' Where ID='" & Val(cbAccountName.SelectedValue) & "'")
                End If
            End If
        End If
        If cbType.SelectedIndex = 0 Then
            If txtWhatsappNo.Text <> "" Then
                StartBackgroundTask(AddressOf SendWhatsappData)
            Else
                MsgBox("Please Enter Valid Whatsapp Contact", MsgBoxStyle.Critical, "Invalid Contact") : txtWhatsappNo.Focus()
            End If
        ElseIf cbType.SelectedIndex = 1 Then
            StartBackgroundTask(AddressOf WahSoft)
        End If
    End Sub
    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
            'MsgBox("A background task is running. you can Use your Task", MsgBoxStyle.Information, "Background Task")
        Else
            MsgBox("A background task is already running.", MsgBoxStyle.Information, "Background Task")
        End If
    End Sub
    Private Sub bgWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        isBackgroundWorkerRunning = True
        Dim action As Action = CType(e.Argument, Action)
        action.Invoke()
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        isBackgroundWorkerRunning = False
        pnlWahtsappNo.Visible = False : cbAccountName.Focus()
    End Sub

End Class