Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Text
Imports System.Threading

Public Class CrateCrateAccountCumMarkaLedger
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

    Private Sub CrateCrateAccountCumMarkaLedger_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isBackgroundWorkerRunning Then
            e.Cancel = True
            Me.Hide()
            ' MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Top = 0 : Me.Left = 0
        End If
    End Sub

    Private Sub CrateLedger_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "Aadhat")
            If msgRslt = MsgBoxResult.Yes Then
                Me.Close() : Exit Sub
            ElseIf msgRslt = MsgBoxResult.No Then
            End If
        End If
    End Sub

    Private Sub CrateLedger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : rowColums()
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
        CbPer.SelectedIndex = 0
        Dim APIResposne As String
        Dim FilePath As String : Dim hostedFilePath As String
        Dim access_token As String = "649299554c995"
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 100
        dg1.Columns(2).Name = "No." : dg1.Columns(2).Width = 100
        dg1.Columns(3).Name = "Type" : dg1.Columns(3).Width = 150
        dg1.Columns(4).Name = "AccountID" : dg1.Columns(4).Visible = False
        dg1.Columns(5).Name = "Crate Name" : dg1.Columns(5).Width = 200
        dg1.Columns(6).Name = "Description" : dg1.Columns(6).Width = 300
        dg1.Columns(7).Name = "Crate In" : dg1.Columns(7).Width = 110
        dg1.Columns(8).Name = "Crate Out" : dg1.Columns(8).Width = 110
        dg1.Columns(9).Name = "Balance" : dg1.Columns(9).Width = 100
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 2
        DgAccountSearch.Columns(0).Name = "AccountID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 490
        ' retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        DgAccountSearch.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select AccountID,(Select AccountName From Accounts Where ID=AccountID) as AccountName From CrateVoucher Where Accountid <> 0 " & condtion & " Group By AccountID order by AccountName Limit 15")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("AccountID").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub

    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : DateDicide()
        mskFromDate.SelectAll()
        clsFun.FillDropDownList(CbPer, "Select CrateID,(Select CrateName From CrateMarka Where ID=CrateID) as CrateName  FROM CrateVoucher Where AccountID=" & Val(txtAccountID.Text) & " Group by CrateID", "CrateName", "CrateID", "--All--")
    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : DgAccountSearch.BringToFront()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" and upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        txtAccount.SelectAll()
    End Sub

    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown, mskFromDate.KeyDown, CbPer.KeyDown, MsktoDate.KeyDown
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        Else
            Exit Sub
        End If
        e.SuppressKeyPress = True
    End Sub
    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.BringToFront()
        '  AccountRowColumns()
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" and upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        mskFromDate.Focus() : DateDicide()
    End Sub
    Private Sub DateDicide()
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) From CrateVoucher Where AccountID=" & Val(txtAccountID.Text) & "")
        maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From CrateVoucher Where AccountID=" & Val(txtAccountID.Text) & "")
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
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            If DgAccountSearch.RowCount = 0 Then Exit Sub
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False
            mskFromDate.Focus()
            DateDicide()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()

    End Sub


    Private Sub txtAccount_Leave(sender As Object, e As EventArgs) Handles txtAccount.Leave
        Dim mindate As String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) as entrydate from crateVoucher where transtype='" & Me.Text & "' And AccountID='" & txtAccountID.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(EntryDate) as entrydate from crateVoucher where transtype='" & Me.Text & "' And AccountID='" & txtAccountID.Text & "'")
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
    Private Sub CrateMarka()
        lblCrateDetails.Visible = False
        lblCrate.Visible = False
        Dim condition As String = String.Empty
        Dim cratebal As String = String.Empty
        Dim CrateQty As String = String.Empty
        Dim sql As String = "Select  ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(txtAccountID.Text) & "')" &
          "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(txtAccountID.Text) & "')) as  Restbal from Accounts   where Restbal<>0  order by AccountName ;"
        Dim crateTotbal As String = clsFun.ExecScalarStr(sql)

        'lblCrate.Visible = True
        If Val(crateTotbal) <> 0 Then lblCrate.Visible = True
        If Val(crateTotbal) < 0 Then
            lblCrate.Text = "Total Crate : " & Math.Abs(Val(crateTotbal)) & " Out"
        Else
            lblCrate.Text = "Total Crate : " & Math.Abs(Val(crateTotbal)) & " In"
        End If
        dt = clsFun.ExecDataTable("Select CrateID,CrateName FROM CrateVoucher Where EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(txtAccountID.Text) & "'  Group by CrateName,AccountID   order by AccountID ")
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    If Application.OpenForms().OfType(Of CrateCrateAccountCumMarkaLedger).Any = False Then Exit Sub
                    Application.DoEvents()
                    Dim tmpamtdr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(txtAccountID.Text) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "  and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(txtAccountID.Text) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & "  and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    tmpamt1 = Val(tmpamtdr1) - Val(tmpamtcr1) '- Val(opbal)
                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(txtAccountID.Text) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(txtAccountID.Text) & " and CrateID=" & Val(dt.Rows(i)("CrateID").ToString()) & " and EntryDate <= '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
                    If Val(tmpamtcr) > Val(tmpamtdr) Then
                        cratebal = dt.Rows(i)("Cratename").ToString() & " = " & Math.Abs(Val(tmpamt)) & " Out"
                        CrateQty = CrateQty & " , " & cratebal
                    ElseIf Val(tmpamtcr) < Val(tmpamtdr) Then
                        cratebal = dt.Rows(i)("Cratename").ToString() & " = " & Math.Abs(Val(tmpamt)) & " In"
                        CrateQty = CrateQty & " , " & cratebal
                    End If
                Next
                CrateQty = CrateQty.Trim().TrimStart(",")
                If CrateQty <> "" Then lblCrateDetails.Visible = True
                lblCrateDetails.Text = CrateQty
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        If CbPer.SelectedIndex = 0 Then
            retrive() : CrateMarka()
        Else
            retrive2() : CrateMarka()
        End If
    End Sub
    Private Sub retrive()
        dg1.Rows.Clear() : txtOpBal.Text = ""
        Dim ssql As String = String.Empty : Dim dt As New DataTable
        Dim dr As Decimal = 0 : Dim cr As Decimal = 0
        Dim tot As Decimal = 0 : Dim opbal As String = ""
        opbal = Val(0)
        If CkHideOpBal.Checked = True Then
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' and TransType<>'Op Bal' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    union all" & _
          " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' and TransType<>'Op Bal' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    "
        Else
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In'  " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    union all" & _
          " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    "

        End If
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select CrateType FROM CrateVoucher WHERE AccountID= " & Val(txtAccountID.Text) & "")
        If drcr = "Crate In" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Crate Out" Then
            opbal = -Val(opbal)
        End If
        dt = clsFun.ExecDataTable(ssql)
        Dim dvData As DataView = New DataView(dt)
        dvData.Sort = " [EntryDate] asc , VoucherID asc "
        dt = dvData.ToTable
        dg1.Rows.Clear() : opbal = Val(tmpamt)
        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from CrateVoucher where  accountID=" & Val(txtAccountID.Text) & " and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        If cnt = 0 Then
            txtOpBal.Text = Math.Abs(Val(opbal)) & " " & clsFun.ExecNonQuery(" Select CrateType   FROM CrateVoucher  WHERE ID= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                txtOpBal.Text = Math.Abs(Val(opbal)) & " Out"
            Else
                txtOpBal.Text = Math.Abs(Val(opbal)) & " In"
            End If
        End If
        If Val(txtOpBal.Text) > 0 Then
            drcr = txtOpBal.Text.Substring(txtOpBal.Text.Length - 3)
        End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).tostring("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(5).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(6).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(7).Value = IIf(Val(dt.Rows(i)("QtyIn").ToString()) = 0, "", dt.Rows(i)("QtyIn").ToString())
                        .Cells(8).Value = IIf(Val(dt.Rows(i)("QtyOut").ToString()) = 0, "", dt.Rows(i)("QtyOut").ToString())
                        If i = 0 Then
                            If Val(.Cells(7).Value) > 0 Then
                                If drcr = " In" Then
                                    tot = Val(opbal) + Val(.Cells(7).Value)
                                Else
                                    If Val(.cells(7).value) > Val(opbal) Then
                                        tot = Val(.Cells(7).Value) - Val(opbal)
                                    Else
                                        tot = Val(.Cells(7).Value) - Val(opbal)
                                    End If
                                End If
                            Else
                                If drcr = "Out" Then
                                    tot = Val(opbal) + Val(.Cells(8).Value)
                                Else
                                    If Val(.cells(8).value) > Val(opbal) Then
                                        tot = Val(.Cells(8).Value) - Val(opbal)
                                    Else
                                        tot = Val(opbal) - Val(.Cells(8).Value)
                                    End If
                                End If
                                If drcr = " In" And Val(opbal) > Val(.Cells(8).Value) Then
                                    tot = Val(tot)
                                ElseIf drcr = " In" And Val(opbal) < Val(.Cells(8).Value) Then
                                    tot = -Val(tot)
                                Else
                                    tot = -Val(tot)
                                End If
                            End If
                        Else
                            If Val(tot) < 0 Then
                                tot = Val(tot) + IIf(Val(.Cells(7).Value) > 0, Val(.Cells(7).Value), -Val(.Cells(8).Value))
                            Else
                                tot = Val(tot) + IIf(Val(.Cells(7).Value) > 0, Val(.Cells(7).Value), -Val(.Cells(8).Value))
                            End If
                        End If
                        .Cells(9).Value = IIf(tot > 0, Math.Abs(tot) & " In", Math.Abs(tot) & " Out")
                        dr = dr + Val(.Cells(7).Value)
                        cr = cr + Val(.Cells(8).Value)
                    End With
                Next
            Else
                tot = opbal
            End If
            If drcr = " In" Then
                dr = dr + Math.Abs(Val(opbal))
            Else
                cr = cr + Math.Abs(Val(opbal))
            End If
            txtDramt.Text = dr.ToString() : txtcrAmt.Text = cr.ToString()
            txtBalAmt.Text = IIf(tot > 0, tot & " In", Math.Abs(tot) & " Out")
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
    End Sub
    Private Sub retrive2()
        dg1.Rows.Clear()
        txtOpBal.Text = ""
        Dim ssql As String = String.Empty
        Dim dt As New DataTable
        Dim dr As Decimal = 0
        Dim cr As Decimal = 0
        Dim tot As Decimal = 0
        Dim opbal As String = ""
        opbal = Val(0)
        If CkHideOpBal.Checked = True Then
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' and TransType<>'Op Bal' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and CrateID='" & Val(CbPer.SelectedValue) & "' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' AND crateID=" & CbPer.SelectedValue & "   union all" & _
                      " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' and TransType<>'Op Bal' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and CrateID='" & Val(CbPer.SelectedValue) & "' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and crateID=" & CbPer.SelectedValue & "    "

        Else
            ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and CrateID='" & Val(CbPer.SelectedValue) & "' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' AND crateID=" & CbPer.SelectedValue & "   union all" & _
          " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and CrateID='" & Val(CbPer.SelectedValue) & "' and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and crateID=" & CbPer.SelectedValue & "    "

        End If
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and CrateID='" & Val(CbPer.SelectedValue) & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(txtAccountID.Text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and CrateID='" & Val(CbPer.SelectedValue) & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select CrateType FROM CrateVoucher WHERE AccountID= " & Val(txtAccountID.Text) & " and crateID=" & CbPer.SelectedValue & "")
        If drcr = "Crate In" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Crate Out" Then
            opbal = -Val(opbal)
        End If
        dt = clsFun.ExecDataTable(ssql)
        Dim dvData As DataView = New DataView(dt)
        dvData.Sort = " [EntryDate] asc , VoucherID asc "
        dt = dvData.ToTable
        dg1.Rows.Clear()
        opbal = tmpamt
        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from CrateVoucher where  accountID=" & Val(txtAccountID.Text) & " and crateID=" & CbPer.SelectedValue & "  and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' and CrateID='" & Val(CbPer.SelectedValue) & "'")
        If cnt = 0 Then
            txtOpBal.Text = Math.Abs(Val(opbal)) & " " & clsFun.ExecScalarStr(" Select CrateType  FROM CrateVoucher  WHERE ID= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                txtOpBal.Text = Math.Abs(Val(opbal)) & " Out"
            Else
                txtOpBal.Text = Math.Abs(Val(opbal)) & " In"
            End If
        End If
        If Val(txtOpBal.Text) > 0 Then
            drcr = txtOpBal.Text.Substring(txtOpBal.Text.Length - 3)
        End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        .Cells(1).Value = CDate(dt.Rows(i)("EntryDate")).tostring("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(5).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(6).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(7).Value = IIf(Val(dt.Rows(i)("QtyIn").ToString()) = 0, "", dt.Rows(i)("QtyIn").ToString())
                        .Cells(8).Value = IIf(Val(dt.Rows(i)("QtyOut").ToString()) = 0, "", dt.Rows(i)("QtyOut").ToString())
                        If i = 0 Then
                            If Val(.Cells(7).Value) > 0 Then
                                If drcr = " In" Then
                                    tot = Val(opbal) + Val(.Cells(7).Value)
                                Else
                                    If Val(.cells(7).value) > Val(opbal) Then
                                        tot = Val(.Cells(7).Value) - Val(opbal)
                                    Else
                                        tot = Val(.Cells(7).Value) - Val(opbal)
                                    End If
                                End If
                            Else
                                If drcr = "Out" Then
                                    tot = Val(opbal) + Val(.Cells(8).Value)
                                Else
                                    If Val(.cells(8).value) > Val(opbal) Then
                                        tot = Val(.Cells(8).Value) - Val(opbal)
                                    Else
                                        tot = Val(opbal) - Val(.Cells(8).Value)
                                    End If
                                End If
                                If drcr = " In" And Val(opbal) > Val(.Cells(8).Value) Then
                                    tot = Val(tot)
                                ElseIf drcr = " In" And Val(opbal) < Val(.Cells(8).Value) Then
                                    tot = -Val(tot)
                                Else
                                    tot = -Val(tot)
                                End If
                            End If
                        Else
                            If Val(tot) < 0 Then
                                tot = Val(tot) + IIf(Val(.Cells(7).Value) > 0, Val(.Cells(7).Value), -Val(.Cells(8).Value))
                            Else
                                tot = Val(tot) + IIf(Val(.Cells(7).Value) > 0, Val(.Cells(7).Value), -Val(.Cells(8).Value))
                            End If

                        End If
                        .Cells(9).Value = IIf(tot > 0, Math.Abs(tot) & " In", Math.Abs(tot) & " Out")
                        dr = dr + Val(.Cells(7).Value)
                        cr = cr + Val(.Cells(8).Value)
                    End With
                Next
            Else
                tot = Val(opbal)
            End If
            If drcr = " In" Then
                dr = dr + Math.Abs(Val(opbal))
            Else
                cr = cr + Math.Abs(Val(opbal))
            End If
            txtDramt.Text = dr.ToString() : txtcrAmt.Text = cr.ToString()
            txtBalAmt.Text = IIf(tot > 0, tot & " In", Math.Abs(tot) & " Out")
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
    End Sub

    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            With row
                sql = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8,P9,P10) values('" & mskFromDate.Text & "'," & _
                    "'" & MsktoDate.Text & "','" & txtAccount.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," & _
                    "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Crate Name").Value & "', " & _
                    "'" & .Cells("Description").Value & "'," & _
                    "'" & .Cells("Crate In").Value & "','" & .Cells("Crate Out").Value & "','" & .Cells("Balance").Value & "','" & lblCrate.Text & "','" & lblCrateDetails.Text & "','" & .Cells("No.").Value & "')"
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
        PrintRecord()
        Report_Viewer.printReport("\AccountWiseCrate.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub txtAccount_Click(sender As Object, e As EventArgs) Handles txtAccount.Click
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
    End Sub

    Private Sub CbPer_GotFocus(sender As Object, e As EventArgs) Handles CbPer.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" and upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        ' txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
        ' DateDicide()
    End Sub

    Private Sub dtp2_GotFocus(sender As Object, e As EventArgs) Handles dtp2.GotFocus
        MsktoDate.Focus()
    End Sub

    Private Sub dtp2_ValueChanged(sender As Object, e As EventArgs) Handles dtp2.ValueChanged
        MsktoDate.Text = dtp2.Value.ToString("dd-MM-yyyy")
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
        pnlWahtsappNo.Visible = True : txtWhatsappNo.Focus()
        txtWhatsappNo.Text = clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & Val(txtAccountID.Text) & "'")
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If cbType.SelectedIndex = 0 Then
            If txtWhatsappNo.Text <> "" Then
                StartBackgroundTask(AddressOf SendWhatsappData)
            Else
                MsgBox("Please Enter Valid Whatsapp Contact", MsgBoxStyle.Critical, "Invalid Contact") : txtWhatsappNo.Focus()
            End If
        Else
            StartBackgroundTask(AddressOf WhatsAppDesktop)
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
        pnlWahtsappNo.Visible = False : txtAccount.Focus()
    End Sub

    Private Sub WhatsAppDesktop()
        Dim sql As String = String.Empty
        If txtWhatsappNo.Text <= "" Then lblStatus.Visible = False : Exit Sub
        lblStatus.Visible = False
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next

        GlobalData.PdfName = txtAccount.Text & "-" & mskFromDate.Text & ".pdf"
        PrintRecord()
        GlobalData.PdfName = txtAccount.Text & " Crate Ledger" & "-" & mskFromDate.Text & ".pdf"
        PrintRecord()
        Pdf_Genrate.ExportReport("\AccountWiseCrate.rpt")
        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
        whatsappSender.SendWhatsAppFile("91" & txtWhatsappNo.Text, "Sended By: Aadhat Software" & vbCrLf & "www.softmanagementindia.in", FilePath)
        WABA.ExecNonQuery("Delete from SendingData")
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
            "('" & Val(0) & "','','" & txtWhatsappNo.Text & "','" & whatsappSender.FilePath & "');Update Settings Set MinState='N'"
        ' sql = "insert into waReport(EntryDate,AccountName,WhatsAppNo,Type,Status) SELECT '" & Date.Today.ToString("yyyy-MM-dd") & "','" & txtAccount.Text.Replace("/", "") & "','" & txtWhatsappNo.Text & "','Crate Ledger','" & lblStatus.Text & "'"
        WABA.ExecNonQuery(sql)
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
        '    pnlWahtsappNo.Visible = False : txtAccount.Focus()
    End Sub

    Private Sub SendWhatsappData()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
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
        GlobalData.PdfName = txtAccount.Text & " Crate Ledger" & "-" & mskFromDate.Text & ".pdf"
        PrintRecord()
        Pdf_Genrate.ExportReport("\AccountWiseCrate.rpt")
        sql = "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
         "('" & Val(txtAccountID.Text) & "','" & Val(txtAccount.Text) & "','" & txtWhatsappNo.Text & "','" & GlobalData.PdfPath & "')"
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
    End Sub

    Private Sub MsktoDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles MsktoDate.MaskInputRejected

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub
End Class