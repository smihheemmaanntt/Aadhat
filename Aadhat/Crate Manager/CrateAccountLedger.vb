Public Class CrateAccountLedger

    Private Sub CrateLedger_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub CrateLedger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums()
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
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
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 440
        retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select AccountID,(Select AccountName From Accounts Where ID=AccountID) as AccountName From CrateVoucher " & condtion & " Group By AccountName order by AccountName")
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
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then txtAccount.Focus() : Exit Sub
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : mskFromDate.SelectAll()
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text <> "" Then
            retriveAccounts(" Where upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If

    End Sub

    Private Sub MsktoDate_GotFocus(sender As Object, e As EventArgs) Handles MsktoDate.GotFocus
        MsktoDate.SelectAll()
    End Sub

    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown, mskFromDate.KeyDown, MsktoDate.KeyDown
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

        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" Where upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
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
        DgAccountSearch.Visible = False : DateValidation()
        mskFromDate.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            If DgAccountSearch.RowCount = 0 Then Exit Sub
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False : DateValidation()
            mskFromDate.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub


    Private Sub txtAccount_Leave(sender As Object, e As EventArgs) Handles txtAccount.Leave

    End Sub
    Private Sub DateValidation()
        Dim mindate as String = String.Empty : Dim maxdate As String = String.Empty
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) as entrydate from crateVoucher where   AccountID='" & txtAccountID.Text & "'")
        maxdate = clsFun.ExecScalarStr("Select max(EntryDate) as entrydate from crateVoucher where  AccountID='" & txtAccountID.Text & "'")
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
    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub mskFromDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskFromDate.MaskInputRejected

    End Sub

    Private Sub CrateMarka()
        lblCrateDetails.Visible = False
        lblCrate.Visible = False
        Dim cratebal As String = String.Empty
        Dim CrateQty As String = String.Empty
        Dim sql As String = "Select  ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(txtAccountID.Text) & "')" &
          "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "' and AccountID='" & Val(txtAccountID.Text) & "')) as  Restbal from Accounts   where Restbal<>0   order by AccountName ;"
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
                    If Application.OpenForms().OfType(Of Ledger).Any = False Then Exit Sub
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
        Retrive() : CrateMarka()
    End Sub
    Private Sub Retrive()
        dg1.Rows.Clear()
        txtOpBal.Text = "" : Dim ssql As String = String.Empty
        Dim dt As New DataTable : Dim dr As Decimal = 0
        Dim cr As Decimal = 0 : Dim tot As Decimal = 0
        Dim opbal As String = "" : opbal = Val(0)
        ssql = "Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,Qty as QtyIn,'0' as QtyOut from CrateVoucher where CrateType ='Crate In' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    union all" & _
            " Select VoucherID,[Entrydate],SlipNo, TransType,AccountID,CrateName,Remark,'0' as QtyOut,Qty as QtyIn from CrateVoucher where CrateType ='Crate Out' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    "
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
        dg1.Rows.Clear()
        opbal = tmpamt
        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from CrateVoucher where  accountID=" & Val(txtAccountID.Text) & " and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        If cnt = 0 Then
            txtOpBal.Text = Math.Abs(Val(opbal)) & " " & clsFun.ExecScalarStr(" Select CrateType   FROM CrateVoucher  WHERE ID= " & Val(txtAccountID.Text) & "")
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
        dg1.ClearSelection()

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
                sql = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7) values('" & mskFromDate.Text & "'," & _
                    "'" & MsktoDate.Text & "','" & txtAccount.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," & _
                    "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Crate Name").Value & "','" & .Cells("Description").Value & "'," & _
                    "'" & .Cells("Crate In").Value & "','" & .Cells("Crate Out").Value & "','" & .Cells("Balance").Value & "')"
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
End Class