Public Class Supplier_Statement

    Private Sub Supplier_Statement_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Supplier_Statement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        rowColums()
        mskFromDate.Text = Date.Today.ToString("dd-MM-yyyy")
        MsktoDate.Text = Date.Today.ToString("dd-MM-yyyy")
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
    Private Sub rowColums()
        dg1.ColumnCount = 10
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date"
        dg1.Columns(1).Width = 130
        dg1.Columns(2).Name = "Type"
        dg1.Columns(2).Width = 150
        dg1.Columns(3).Name = "Account Name"
        dg1.Columns(3).Visible = False
        dg1.Columns(4).Name = "Description"
        dg1.Columns(4).Width = 545
        dg1.Columns(5).Name = "Debit"
        dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Credit"
        dg1.Columns(6).Width = 100
        dg1.Columns(7).Name = "Balance"
        dg1.Columns(7).Width = 150
        dg1.Columns(8).Name = "HindiName"
        dg1.Columns(8).Width = 100
        dg1.Columns(9).Name = "HindiItem"
        dg1.Columns(9).Width = 100
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 2
        DgAccountSearch.Columns(0).Name = "AccountID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 440
        retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select AccountID,AccountName From Purchase Where PurchaseTypeName='Stock In' and AccountID Not in(7) " & condtion & " Group By AccountName order by AccountName")
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
    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If

    End Sub
    Private Sub txtCustomer_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.BringToFront()
        AccountRowColumns()
        DgAccountSearch.Visible = True
    End Sub
    Private Sub txtCustomer_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub mskFromDate_GotFocus(sender As Object, e As EventArgs) Handles mskFromDate.GotFocus
        If DgAccountSearch.RowCount = 0 Then txtAccount.Focus() : Exit Sub
        If DgAccountSearch.SelectedRows.Count = 0 Then txtAccount.Focus() : Exit Sub
        txtAccountID.text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub mskFromDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskFromDate.Validating
        mskFromDate.Text = clsFun.convdate(mskFromDate.Text)
    End Sub

    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        txtAccountID.text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : DateValidation()
        mskFromDate.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear()
            txtAccountID.Clear()
            If DgAccountSearch.RowCount = 0 Then Exit Sub
            txtAccountID.text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False : DateValidation()
            mskFromDate.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub DateValidation()
        'If clsFun.ExecScalarInt("Select count(*)from Accounts where AccountName='" & txtAccount.Text & "'") = 0 Then
        '    MsgBox("Account Name Not Found in Database...", vbOKOnly, "Access Denied")
        '    clsFun.FillDropDownList(cbAccountName, "Select * From Account_AcGrp", "AccountName", "Id", "")
        '    cbAccountName.Focus()
        '    Exit Sub
        'End If
        mindate = clsFun.ExecScalarStr("Select min(EntryDate) From Ledger Where AccountID=" & Val(txtAccountID.Text) & "")
        maxdate = clsFun.ExecScalarStr("Select Max(EntryDate) From Ledger Where AccountID=" & Val(txtAccountID.Text) & "")
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
    Private Sub MsktoDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MsktoDate.Validating
        MsktoDate.Text = clsFun.convdate(MsktoDate.Text)
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        Retrive()
    End Sub
    Private Sub Retrive()
        Application.DoEvents()
        dg1.Rows.Clear()
        txtOpBal.Text = ""
        Dim ssql As String = String.Empty
        Dim dt As New DataTable : Dim dr As Decimal = 0
        Dim cr As Decimal = 0 : Dim tot As Decimal = 0
        Dim opbal As String = "" : Dim tmpamtdr As String = ""
        Dim tmpamtcr As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        ssql = "Select VourchersID,Entrydate, TransType,AccountName,Remark,RemarkHindi,Narration,Amount as Dr,'0' as Cr from Ledger where DC ='D' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    union all" & _
            " Select VourchersID,Entrydate,  TransType,AccountName,Remark,RemarkHindi,Narration,'0' as Dr,Amount as Cr  from Ledger where Dc='C' " & IIf(Val(txtAccountID.Text) > 0, "and AccountID=" & Val(txtAccountID.Text) & "", "") & " and EntryDate Between '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "' And '" & CDate(MsktoDate.Text).ToString("yyyy-MM-dd") & "'    "
        tmpamtdr = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        tmpamtcr = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.text) & " and EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Val(opbal) + Val(tmpamtdr)
        Else
            tmpamtcr = Val(opbal) + Val(tmpamtcr)
        End If

        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = -Val(opbal)
        End If

        'If Val(opbal) < Val(tmpamt) Then
        '    tmpamt = Math.Abs(Val(tmpamt)) + Math.Abs(Val(opbal))
        'Else
        '    tmpamt = Val(opbal) - Math.Abs(Val(tmpamt))
        'End If
        dt = clsFun.ExecDataTable(ssql)
        Dim dvData As DataView = New DataView(dt)
        'dvData.RowFilter = "EntryDate Between '" & mskFromDate.Text & "' And '" & MsktoDate.Text & "'"
        dvData.Sort = " [EntryDate],VourchersID asc"
        dt = dvData.ToTable
        dg1.Rows.Clear()

        opbal = tmpamt
        'If Val(tmpamt) > 0 Then
        '    opbal = Val(tmpamt)
        'Else
        '    opbal = Val(opbal) + Val(tmpamt)
        'End If

        Dim cnt As Integer = clsFun.ExecScalarInt("Select count(*) from LEdger where  accountID=" & Val(txtAccountID.text) & " and  EntryDate < '" & CDate(mskFromDate.Text).ToString("yyyy-MM-dd") & "'")
        If cnt = 0 Then
            txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " " & clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Cr"
            Else
                txtOpBal.Text = Format(Math.Abs(Val(opbal)), "0.00") & " Dr"
            End If
            '  txtOpBal.Text = IIf(Val(opbal) > 0, Val(opbal) & " Dr", Math.Abs(Val(opbal)) & " Cr")
        End If
        ''opbal = Val(opbal) + Val(tmpamt)
        '  
        If Val(txtOpBal.Text) > 0 Then
            drcr = txtOpBal.Text.Substring(txtOpBal.Text.Length - 2)
        End If
        ' opbal = Math.Abs(val(opbal))
        Try
            If dt.Rows.Count > 0 Then
                Application.DoEvents()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VourchersID").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("EntryDate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("TransType").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(5).Value = IIf(Val(dt.Rows(i)("Dr").ToString()) = 0, "", Format(Val(dt.Rows(i)("Dr").ToString()), "0.00"))
                        .Cells(6).Value = IIf(Val(dt.Rows(i)("Cr").ToString()) = 0, "", Format(Val(dt.Rows(i)("Cr").ToString()), "0.00"))
                        .Cells(8).Value = clsFun.ExecScalarStr(" Select OtherName FROM Accounts WHERE ID= " & txtAccountID.text & "")
                        ssql = "(Select * FROM Transaction2 AS T INNER JOIN Items AS I ON T.ItemID = I.ID)"
                        If dt.Rows(i)("TransType").ToString() = "Speed Sale" Then
                            'ssql = "Select OtherName ||' = नग  : '||(nug)||', वजन : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount as HindiRemark  From " & ssql & "  where AccountID=" & txtAccountID.text & " and TransType='" & dt.Rows(i)("TransType").ToString() & "' and EntryDate= '" & Format(dt.Rows(i)("EntryDate"), "yyyy-MM-dd") & "'"
                            .Cells(9).Value = clsFun.ExecScalarStr("Select OtherName ||' = नग  : '||(nug)||', वजन : '|| (weight) ||', भाव '||(rate)||' /- '|| Per ||' = '|| amount From " & ssql & " where VoucherID= " & dt.Rows(i)("VourchersID").ToString() & "")
                        Else
                            .Cells(9).Value = dt.Rows(i)("RemarkHindi").ToString()
                        End If
                        If i = 0 Then
                            If Val(.Cells(5).Value) > 0 Then
                                If drcr = "Dr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(5).Value)), "0.00")
                                Else
                                    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'If Val(.cells(5).value) > Val(opbal) Then
                                    '    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
0:                                  'Else
                                    '    tot = Format(Val(Val(.Cells(5).Value) - Val(opbal)), "0.00")
                                    'End If
                                End If
                            Else
                                If drcr = "Cr" Then
                                    tot = Format(Val(Val(opbal) + Val(.Cells(6).Value)), "0.00")
                                Else
                                    If Val(.cells(6).value) > Val(opbal) Then
                                        tot = Format(Val(Val(.Cells(6).Value) - Val(opbal)), "0.00")
                                    Else
                                        tot = Format(Val(Val(opbal) - Val(.Cells(6).Value)), "0.00")
                                    End If
                                End If
                                If drcr = "Dr" And Val(opbal) > Val(.Cells(6).Value) Then
                                    tot = Math.Round(Val(tot), 2)
                                Else
                                    tot = -Val(tot)
                                End If
                            End If
                        Else
                            tot = tot + IIf(Val(.Cells(5).Value) > 0, Val(.Cells(5).Value), -Val(.Cells(6).Value))
                        End If
                        .Cells(7).Value = IIf(tot > 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(tot), "0.00") & " Cr")
                        dr = dr + Val(.Cells(5).Value)
                        cr = cr + Val(.Cells(6).Value)
                    End With
                Next
            Else
                tot = Format(Val(opbal), "0.00")

            End If
            If drcr = "Dr" Then
                dr = Format(Val(dr) + Math.Abs(Val(opbal)), "0.00")
            Else
                cr = Format(Val(cr) + Math.Abs(Val(opbal)), "0.00")
            End If
            txtDramt.Text = dr.ToString() : txtcrAmt.Text = cr.ToString()
            If dt.Rows.Count = 0 Then
                txtBalAmt.Text = txtOpBal.Text
            Else
                txtBalAmt.Text = IIf(Format(Val(tot), "0.00") >= 0, Format(Val(tot), "0.00") & " Dr", Format(Math.Abs(Val(tot)), "0.00") & " Cr")
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        dg1.ClearSelection()
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
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For Each row As DataGridViewRow In dg1.Rows
            Application.DoEvents()
            With row
                sql = "insert into Printing(D1,D2,M1,M2, M3, M4, M5, P1, P2,P3, P4, P5, P6,P7,P8) values('" & mskFromDate.Text & "'," & _
                    "'" & MsktoDate.Text & "','" & txtAccount.Text & "','" & txtOpBal.Text & "','" & txtDramt.Text & "','" & txtcrAmt.Text & "'," & _
                    "'" & txtBalAmt.Text & "','" & .Cells("Date").Value & "','" & .Cells("Type").Value & "','" & .Cells("Account Name").Value & "','" & .Cells("Description").Value & "'," & _
                    "'" & .Cells("Debit").Value & "','" & .Cells("Credit").Value & "','" & .Cells("Balance").Value & "','" & .Cells("HindiName").Value & "')"
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
        Report_Viewer.printReport("\Ledger.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
End Class