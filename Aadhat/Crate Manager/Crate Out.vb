Imports System.IO

Public Class Crate_Out
    Dim vno As Integer
    Dim VchId As Integer = 0
    Dim sql As String = String.Empty
    Dim bal As String = ""
    Dim ServerTag As Integer
    Private WhatsappCheckBox As CheckBox = New CheckBox()
    Private Sub Crate_IN_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Crate_IN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums() : VNumber() : mskEntryDate.Focus()
    End Sub
    Public Sub FillControl()
        Dim SendingMethod As String
        Dim LangugageType As String
        Dim MsgType As String
        Dim Sql As String = "Select * From API"
        Dim dt As New DataTable
        dt = ClsFunPrimary.ExecDataTable(Sql)
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    SendingMethod = dt.Rows(i)("SendingMethod").ToString()
                    cbType.SelectedIndex = 0
                    If SendingMethod = "Easy WhatsApp" Then cbType.SelectedIndex = 0 Else cbType.SelectedIndex = 0 : cbType.Visible = True
                    LangugageType = dt.Rows(i)("LanguageType").ToString()
                    btnRadioEnglish.Checked = True
                    If LangugageType = "English" Then btnRadioEnglish.Checked = True Else RadioRegional.Checked = True
                    MsgType = dt.Rows(i)("SendingType").ToString()
                    RadioPDFMsg.Checked = True
                    If MsgType = "Pdf + Msg" Then RadioPDFMsg.Checked = True
                    If MsgType = "Pdf Only" Then RadioPdfOnly.Checked = True
                    If MsgType = "Msg Only" Then RadioMsgOnly.Checked = True
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        'clsFun.CloseConnection()
    End Sub
    Private Sub AcBal()
        '  Dim j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = Val(0)
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
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
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from CrateVoucher where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Math.Abs(Val(opbal)) & " " & clsFun.ExecScalarStr(" Select CrateType from CrateVoucher where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Math.Abs(Val(tmpamt)) & " Out"
            Else
                opbal = Math.Abs(Val(tmpamt)) & " In"
            End If
            bal = Math.Abs(Val(tmpamt))
        End If
        lblcrateBal.Visible = True
        lblcrateBal.Text = "Crate Bal : " & Val(bal)
    End Sub

    Private Sub VNumber()
        If dg1.SelectedRows.Count <> 0 Then Exit Sub
        'vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & Me.Text & "'")
        'txtSlipNo.Text = vno + 1
        Dim vno As Integer = 0
        If vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'AND id= (SELECT max(id) FROM Vouchers Where TransType='" & Me.Text & "')")) <> 0 Then
            vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "' AND id=(SELECT max(id) FROM Vouchers Where TransType='" & Me.Text & "')")
            txtSlipNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        Else
            ' vno = clsFun.ExecScalarInt("SELECT InvoiceID AS NumberOfProducts FROM Vouchers WHERE id=(SELECT max(id) FROM Vouchers Where TransType='" & Me.Text & "')")
            vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "' AND id= (SELECT max(id) FROM Vouchers Where TransType='" & Me.Text & "')")
            txtSlipNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        End If
    End Sub

    'Private Sub VNumber()
    '    If dg1.SelectedRows.Count <> 0 Then Exit Sub
    '    'vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM CrateVoucher Where TransType='" & Me.Text & "'")
    '    'txtSlipNo.Text = vno + 1
    '    Dim vno As Integer = 0
    '    If vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "' AND id= (Select mAX(ID) from Vouchers)")) <> 0 Then
    '        vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "' AND id= (Select mAX(ID) from Vouchers)")
    '        txtSlipNo.Text = vno + 1
    '        txtInvoiceID.Text = vno + 1
    '    Else
    '        vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "' AND id= (Select mAX(ID) from Vouchers)")
    '        txtSlipNo.Text = vno + 1
    '        txtInvoiceID.Text = vno + 1
    '    End If
    'End Sub


    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus, mskEntryDate.Click
        mskEntryDate.SelectAll()
    End Sub

    Private Sub mskEntryDate_Leave(sender As Object, e As EventArgs) Handles mskEntryDate.Leave
        'retrive()
    End Sub
    Private Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date"
        dg1.Columns(1).Width = 112
        dg1.Columns(2).Name = "No"
        dg1.Columns(2).Width = 76
        dg1.Columns(3).Name = "Account Name"
        dg1.Columns(3).Width = 201
        dg1.Columns(4).Name = "Crate Name"
        dg1.Columns(4).Width = 201
        dg1.Columns(5).Name = "Qty"
        dg1.Columns(5).Width = 82
        dg1.Columns(6).Name = "Rate"
        dg1.Columns(6).Width = 99
        dg1.Columns(7).Name = "Amount"
        dg1.Columns(7).Width = 143
        dg1.Columns(8).Name = "Remark"
        dg1.Columns(8).Width = 270
    End Sub
    Private Sub CrateLedger()
        Dim FastQuery As String = String.Empty
        Dim CashPaid As String = ""
        If ckCash.Checked = True Then CashPaid = "Y"
        If ckCash.Checked = False Then CashPaid = "N"
        Dim SqliteEntryDate As String = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        If Val(txtModeID.Text) > 0 Then ''Party Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & "," & txtSlipNo.Text & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(txtModeID.Text) & ",'" & txtCrateMarka.Text & "','" & Val(txtCrateQty.Text) & "', '" & txtRemark.Text & "','" & Val(txtCrateRate.Text) & "','" & Val(txtCrateAmt.Text) & "','" & CashPaid & "'"
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastCrateLedger(FastQuery)
        ''**********************************************************************************
    End Sub

    Private Sub ServerCrateLedger()
        If Val(OrgID) = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        Dim CashPaid As String = ""
        If ckCash.Checked = True Then CashPaid = "Y"
        If ckCash.Checked = False Then CashPaid = "N"
        Dim SqliteEntryDate As String = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        'Dim VchId As Integer = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
        If Val(txtModeID.Text) > 0 Then ''Party Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & "," & txtSlipNo.Text & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(txtModeID.Text) & ",'" & txtCrateMarka.Text & "','" & Val(txtCrateQty.Text) & "', '" & txtRemark.Text & "','" & Val(txtCrateRate.Text) & "','" & Val(txtCrateAmt.Text) & "','" & CashPaid & "', " & Val(ServerTag) & "," & Val(OrgID) & ""
            'clsFun.CrateLedger(0, txtID.Text, txtSlipNo.Text, CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, Val(txtAccountID.Text), txtAccount.Text, "Crate Out", txtModeID.Text, txtCrateMarka.Text, txtCrateQty.Text, txtRemark.Text, txtCrateRate.Text, txtCrateAmt.Text, CashPaid)
            '    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & "," & txtSlipNo.Text & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "','Crate Out'," & Val(txtModeID.Text) & ",'" & txtCrateMarka.Text & "','" & Val(txtCrateQty.Text) & "', '" & txtRemark.Text & "','" & txtCrateAmt.Text & "','" & CashPaid & "', ''," & Val(ServerTag) & "," & Val(OrgID) & ""
        End If
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastCrateLedger(FastQuery)
        ''**********************************************************************************
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Val(txtCrateQty.Text) <= CDbl(Val(0)) Then
            MsgBox("There is No Crate Quantity to Save", MsgBoxStyle.Critical, "Empty Quantity")
            Exit Sub
        End If
        If btnSave.Text = "&Save" Then
            save()
        Else
            UpdateRecord()
        End If
        If Application.OpenForms().OfType(Of Report_Viewer).Any = True Then Report_Viewer.btnPrint.Focus()
    End Sub
    Private Sub PrintRecord()
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        sql = "insert into printing (D1,M1,M2,M3,P1,P2,P3,P4,P5,P6) values (@1, @2, @3,@4,@5,@6,@7,@8,@9,@10)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunPrimary.GetConnection())
            cmd.Parameters.AddWithValue("@1", mskEntryDate.Text)
            cmd.Parameters.AddWithValue("@2", Me.Text)
            cmd.Parameters.AddWithValue("@3", txtCrateMarka.Text)
            cmd.Parameters.AddWithValue("@4", txtAccount.Text)
            cmd.Parameters.AddWithValue("@5", txtCrateQty.Text)
            cmd.Parameters.AddWithValue("@6", txtCrateRate.Text)
            cmd.Parameters.AddWithValue("@7", txtCrateAmt.Text)
            cmd.Parameters.AddWithValue("@8", txtRemark.Text)
            cmd.Parameters.AddWithValue("@9", txtSlipNo.Text)
            cmd.Parameters.AddWithValue("@10", ckCash.Text)
            If cmd.ExecuteNonQuery() > 0 Then

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub print()
        If clsFun.ExecScalarStr("Select AskPayment from Controls") = "Crate" Then
            If MessageBox.Show("Are You Sure want to Print Crate Out Slip ??", "Print Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                PrintRecord()
                Report_Viewer.printReport("\TransCrate.rpt")
                Report_Viewer.MdiParent = MainScreenForm
                Report_Viewer.Show()
                Report_Viewer.BringToFront()
            End If
        ElseIf clsFun.ExecScalarStr("Select AskPayment from Controls") = "Crate+Rec" Then
            If MessageBox.Show("Are You Sure want to Print Crate Out Slip ??", "Print Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                PrintRecord()
                Report_Viewer.printReport("\TransCrate.rpt")
                Report_Viewer.MdiParent = MainScreenForm
                Report_Viewer.Show()
                Report_Viewer.BringToFront()
            End If
        End If
    End Sub

    Private Sub save()
        Dim CashPaid As String = ""
        If ckCash.Checked = True Then CashPaid = "Y"
        If ckCash.Checked = False Then CashPaid = "N"
        If txtAccount.Text = "" Then txtAccount.Focus() : Exit Sub
        If txtCrateMarka.Text = "" Then txtCrateMarka.Focus() : Exit Sub
        If txtCrateQty.Text = "" Then
            MsgBox("There is No Crate Quantity to Save", MsgBoxStyle.Critical, "Empty Quantity")
            txtCrateQty.Focus() : Exit Sub
        End If
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        sql = "insert into Vouchers (TransType,EntryDate,BillNo,InvoiceID) values (@1,@2,@3,@4)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", Me.Text)
            cmd.Parameters.AddWithValue("@2", CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@3", txtSlipNo.Text)
            cmd.Parameters.AddWithValue("@4", Val(txtInvoiceID.Text))
            If cmd.ExecuteNonQuery() > 0 Then
                txtID.Text = Val(clsFun.ExecScalarInt("Select max(ID) ID from Vouchers;"))
                CrateLedger() : ledger() : ServerTag = 1 : ServerLedger() : ServerCrateLedger()
                MsgBox("Record Saved Successfully.", vbInformation + vbOKOnly, "Saved")
                Txtclear()
            End If
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try

        ' Purchase.BtnRefresh.PerformClick()
    End Sub

    Private Sub UpdateRecord()
        Dim SqliteEntryDate As String = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim cmd As SQLite.SQLiteCommand
        sql = "Update Vouchers SET TransType='" & Me.Text & "',Entrydate='" & SqliteEntryDate & "',BillNo='" & txtSlipNo.Text & "',InvoiceID='" & Val(txtInvoiceID.Text) & "' where ID =" & Val(txtID.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        If clsFun.ExecNonQuery(sql) > 0 Then
            If clsFun.ExecNonQuery("DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(txtID.Text) & ";DELETE from Ledger WHERE vourchersID=" & Val(txtID.Text) & "") > 0 Then
                ClsFunserver.ExecNonQuery("Delete From Ledger Where  VourchersID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & "; " &
                                          " Delete From CrateVoucher Where   VoucherID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & ";")
                CrateLedger() : ledger() : ServerTag = 1 : ServerLedger() : ServerCrateLedger()
                MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
                Txtclear()
            End If
        End If
    End Sub

    Private Sub ledger()
        Dim FastQuery As String = String.Empty
        Dim Remark2 As String = txtCrateMarka.Text & ", Qty : " & txtCrateQty.Text & ", Rate /- " & txtCrateRate.Text & " ( " & IIf(ckCash.Checked = True, "Cash Recived", "Bardana Issued") & " )"
        If Val(txtCrateAmt.Text) > 0 Then ''Party Account
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtCrateAmt.Text) & ",'D','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'"
        End If
        If Val(txtCrateAmt.Text) > 0 Then ''Total Amout
            If txtModeID.Text > 0 Then ''Party Account
                If ckCash.Checked = True Then
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(7) & ",'" & clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=7") & "'," & Val(txtCrateAmt.Text) & ",'C','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'"
                Else
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(4) & ",'" & clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=4") & "'," & Val(txtCrateAmt.Text) & ",'C','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'"
                End If
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastLedger(FastQuery)
    End Sub
    Private Sub ServerLedger()
        If Val(OrgID) = 0 Then Exit Sub
        ' " & Val(ServerTag) & "," & Val(OrgID) & ",
        Dim FastQuery As String = String.Empty
        Dim Remark2 As String = txtCrateMarka.Text & ", Qty : " & txtCrateQty.Text & ", Rate /- " & txtCrateRate.Text & " ( " & IIf(ckCash.Checked = True, "Cash Recived", "Bardana Issued") & " )"
        If Val(txtCrateAmt.Text) > 0 Then ''Party Account
            '   clsFun.Ledger(0, Val(txtID.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, txtAccountID.Text, txtAccount.Text, Val(txtCrateAmt.Text), "C", Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text), txtAccount.Text)
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtCrateAmt.Text) & ",'D'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'"
        End If
        If Val(txtCrateAmt.Text) > 0 Then ''Total Amout
            If Val(txtModeID.Text) > 0 Then ''Party Account
                If ckCash.Checked = True Then
                    'clsFun.Ledger(0, Val(txtID.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 7, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=7"), Val(txtCrateAmt.Text), "D", Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text), txtAccount.Text)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(7) & ",'" & clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=7") & "'," & Val(txtCrateAmt.Text) & ",'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'"
                Else
                    '    clsFun.Ledger(0, Val(txtID.Text), CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"), Me.Text, 4, clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=4"), Val(txtCrateAmt.Text), "D", Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text), txtAccount.Text)
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(4) & ",'" & clsFun.ExecScalarStr("Select AccountName From Accounts Where ID=4") & "'," & Val(txtCrateAmt.Text) & ",'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "','" & txtAccount.Text & "','" & Remark2 & IIf(txtRemark.Text = "", "", ", Remark :" & txtRemark.Text) & "'"
                End If
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub

    Sub calc()
        TxtGrandTotal.Text = Format(0, "0.00")
        txtTotAmount.Text = Format(0, "0.00")
        Dim i As Integer
        For i = 0 To dg1.Rows.Count - 1
            TxtGrandTotal.Text = Format(Val(TxtGrandTotal.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotAmount.Text = Format(Val(txtTotAmount.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
        Next
        lblRecordCount.Visible = True
        lblRecordCount.Text = "Record Count : " & dg1.RowCount
    End Sub
    Private Sub retrive()
        dg1.Rows.Clear()
        Dim dt As New DataTable
        ' dt = clsFun.ExecDataTable("Select * from vouchers where entrydate='" & mskFromDate.Text & "' and MsktoDate='" & mskFromDate.Text & "'and TransType='" & Me.Text & "'")
        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType = 'Crate Out' Order by VoucherID Desc")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(5).Value = dt.Rows(i)("Qty").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub txtSlipNo_Leave(sender As Object, e As EventArgs) Handles txtSlipNo.Leave
        txtInvoiceID.Text = txtSlipNo.Text
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim CashPaid As String = ""
        Dim ssql As String = String.Empty
        Dim primary As String = String.Empty
        Dim dt As New DataTable
        btnSave.BackColor = Color.Coral
        btnSave.Image = My.Resources.Edit
        'lblgroup.Text = "Modify Group"
        btnSave.Text = "&Update"
        'ssql = "Select * FROM Vouchers AS V INNER JOIN CrateVoucher AS CV ON CV.VoucherID = V.ID"
        ssql = "Select * from CrateVoucher where crateType='Crate Out' and Voucherid=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            txtID.Text = id
            mskEntryDate.Text = CDate(dt.Rows(0)("EntryDate")).ToString("dd-MM-yyyy")
            txtSlipNo.Text = dt.Rows(0)("SlipNo").ToString()
            txtAccountID.Text = dt.Rows(0)("AccountID").ToString()
            txtAccount.Text = dt.Rows(0)("AccountName").ToString()
            txtModeID.Text = dt.Rows(0)("CrateID").ToString()
            txtCrateMarka.Text = dt.Rows(0)("CrateName").ToString()
            txtCrateQty.Text = dt.Rows(0)("Qty").ToString()
            txtRemark.Text = dt.Rows(0)("Remark").ToString()
            txtCrateRate.Text = Format(Val(dt.Rows(0)("Rate").ToString()), "0.00")
            txtCrateAmt.Text = Format(Val(dt.Rows(0)("Amount").ToString()), "0.00")
            CashPaid = dt.Rows(0)("CashPaid").ToString()
            If CashPaid = "Y" Then ckCash.Checked = True
            If CashPaid = "N" Then ckCash.Checked = False
        End If
    End Sub
    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
        If dg1.RowCount = 0 Then
            retrive()
        End If
        VNumber()
    End Sub

    Private Sub dg1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellDoubleClick
        FillControls(dg1.SelectedRows(0).Cells(0).Value)
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                Exit Sub
            End If
            FillControls(dg1.SelectedRows(0).Cells(0).Value)
            mskEntryDate.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            Delete()
        End If
    End Sub
    Private Sub Delete()
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        Try
            If MessageBox.Show("Are you Sure want to Delete Crate??", "Delete Entry", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(dg1.SelectedRows(0).Cells(0).Value) & ";DELETE from CrateVoucher WHERE TransType<>'Op Bal' and  VoucherID=" & Val(dg1.SelectedRows(0).Cells(0).Value) & ";DELETE from LEDGER WHERE VouRcherSID=" & Val(dg1.SelectedRows(0).Cells(0).Value) & "") > 0 Then
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where  VourchersID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & "; " &
                      " Delete From CrateVoucher Where   VoucherID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & ";")
                    ServerTag = 0 : ServerLedger() : ServerCrateLedger()
                End If
                MsgBox("Crate Out Entry Deleted Successfully...", MsgBoxStyle.Information, "Crate Out Deleted")
                Txtclear()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub txtCrateQty_GotFocus(sender As Object, e As EventArgs) Handles txtCrateQty.GotFocus
        'AcBal()
        If dgCrate.ColumnCount = 0 Then ModeColums()
        If dgCrate.Rows.Count = 0 Then RetriveMode()
        If dgCrate.SelectedRows.Count = 0 Then dgCrate.Visible = True : txtCrateMarka.Focus() : Exit Sub
        txtModeID.Text = Val(dgCrate.SelectedRows(0).Cells(0).Value)
        txtCrateMarka.Text = dgCrate.SelectedRows(0).Cells(1).Value
        txtCrateRate.Text = dgCrate.SelectedRows(0).Cells(3).Value
        dgCrate.Visible = False
    End Sub

    Private Sub txtMode_GotFocus(sender As Object, e As EventArgs) Handles txtCrateMarka.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True : txtAccount.Focus() : Exit Sub
        txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : AcBal()
        If dgCrate.ColumnCount = 0 Then ModeColums()
        If dgCrate.RowCount = 0 Then RetriveMode()
        If txtCrateMarka.Text.Trim() <> "" Then
            RetriveMode(" Where upper(MarkaName) Like upper('" & txtCrateMarka.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        If dgCrate.SelectedRows.Count = 0 Then dgCrate.Visible = True : txtCrateMarka.Focus()
    End Sub
    Private Sub Txtclear()
        print()
        btnSave.Text = "&Save"
        txtSlipNo.Clear()
        '  txtAccountID.Clear() : txtAccount.Clear()
        ' txtCrateMarka.Clear() : txtModeID.Clear()
        txtCrateQty.Clear() : VNumber()
        txtRemark.Clear() : mskEntryDate.Focus()
        btnSave.BackColor = Color.DarkTurquoise
        btnSave.Image = My.Resources.Save
        retrive()
    End Sub

    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        txtAccount.SelectionStart = 0 : txtAccount.SelectionLength = Len(txtAccount.Text)
    End Sub
    'Private Sub txtRemark_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRemark.KeyDown
    '    If e.KeyCode = Keys.Enter Then ckCash.Focus()
    'End Sub


    Private Sub txtCrateQty_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCrateQty.KeyDown, mskEntryDate.KeyDown,
        txtCrateMarka.KeyDown, txtSlipNo.KeyDown, txtAccount.KeyDown, txtCrateRate.KeyDown, txtCrateAmt.KeyDown, txtRemark.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.F1 Then
                If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
                Dim tmpAcID As Integer = DgAccountSearch.SelectedRows(0).Cells(0).Value
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                CreateAccount.FillContros(tmpAcID)
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
                e.SuppressKeyPress = True
            End If
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
                e.SuppressKeyPress = True
            End If
            If e.KeyCode = Keys.Down Then
                DgAccountSearch.Focus() : e.SuppressKeyPress = True
            End If
        End If
        If txtCrateMarka.Focused Then
            If e.KeyCode = Keys.F3 Then
                CrateForm.MdiParent = MainScreenForm
                CrateForm.Show()
                If Not CrateForm Is Nothing Then
                    CrateForm.BringToFront()
                End If
                e.SuppressKeyPress = True
            End If
            If e.KeyCode = Keys.F1 Then
                If dgCrate.SelectedRows.Count = 0 Then Exit Sub
                Dim tmpCrateID As Integer = dgCrate.SelectedRows(0).Cells(0).Value
                CrateForm.MdiParent = MainScreenForm
                CrateForm.Show()
                CrateForm.FillControls(tmpCrateID)
                If Not CrateForm Is Nothing Then
                    CrateForm.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.Down Then
                dgCrate.Focus()
                e.SuppressKeyPress = True
            End If
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnSave.Focus()
        End Select
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 240
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 100
        retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        ' If DgAccountSearch.Columns.Count = 0 Then AccountRowColumns()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * From Account_AcGrp where (groupid in(11,16,17) or UnderGroupID in (11,16,17)) " & condtion & " order by AccountName Limit 20")
        Try
            If dt.Rows.Count > 0 Then
                DgAccountSearch.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    DgAccountSearch.Rows.Add()
                    With DgAccountSearch.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(2).Value = dt.Rows(i)("City").ToString()
                    End With
                    ' Dg1.Rows.Add()
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If DgAccountSearch.RowCount = 0 Then Exit Sub
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear() : txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : txtCrateMarka.Focus()
    End Sub
    Private Sub DgAccountSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles DgAccountSearch.KeyDown
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If DgAccountSearch.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = Val(DgAccountSearch.SelectedRows(0).Cells(0).Value)
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False : txtCrateMarka.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub ModeColums()
        dgCrate.ColumnCount = 4
        dgCrate.Columns(0).Name = "ID" : dgCrate.Columns(0).Visible = False
        dgCrate.Columns(1).Name = "Mode Name" : dgCrate.Columns(1).Width = 150
        dgCrate.Columns(2).Name = "Qty" : dgCrate.Columns(2).Width = 150
        dgCrate.Columns(3).Name = "Rate" : dgCrate.Columns(2).Width = 150
        RetriveMode()
    End Sub
    Private Sub RetriveMode(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        Dim totalOpOutCrate As Decimal
        Dim Opbal As String
        Dim oldbal As Decimal
        Dim totalOpInCrate As Integer = 0
        dt = clsFun.ExecDataTable("Select ID,MarkaName,Rate from CrateMarka  " & condtion & " order by MarkaName")
        Try
            If dt.Rows.Count > 0 Then
                dgCrate.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    'Dim tmpamtdr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    'Dim tmpamtcr1 As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    'tmpamt1 = Val(tmpamtdr1) - Val(tmpamtcr1) '- Val(opbal)

                    Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate In' and accountID=" & Val(txtAccountID.Text) & " and CrateID=" & Val(dt.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Qty) as tot from CrateVoucher where CrateType ='Crate Out'  and accountID=" & Val(txtAccountID.Text) & " and CrateID=" & Val(dt.Rows(i)("ID").ToString()) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
                    Dim tmpamt As Integer = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
                    If Val(tmpamtcr) > Val(tmpamtdr) Then
                        totalOpOutCrate = Val(totalOpOutCrate) + Val(tmpamt)
                        oldbal = Val(tmpamt)
                        Opbal = Math.Abs(Val(tmpamt)) & " Out"
                    Else
                        oldbal = tmpamt
                        totalOpInCrate = totalOpInCrate + tmpamt
                        Opbal = Math.Abs(Val(tmpamt)) & " In"
                    End If
                    dgCrate.Rows.Add()
                    With dgCrate.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("MarkaName").ToString()
                        .Cells(2).Value = Opbal
                        .Cells(3).Value = dt.Rows(i)("Rate").ToString()
                    End With
                Next

            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
    End Sub
    Private Sub txtMode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCrateMarka.KeyPress, txtAccount.KeyPress
        If txtCrateMarka.Focused = True Then dgCrate.Visible = True
        If e.KeyChar = "'"c Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtMode_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCrateMarka.KeyUp
        If txtCrateMarka.Text.Trim() <> "" Then
            dgCrate.Visible = True
            RetriveMode(" Where upper(MarkaName) Like upper('" & txtCrateMarka.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        If e.KeyCode = Keys.Escape Then dgCrate.Visible = False
    End Sub
    Private Sub dgMode_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgCrate.CellClick
        txtCrateMarka.Clear()
        txtModeID.Clear()
        txtModeID.Text = dgCrate.SelectedRows(0).Cells(0).Value
        txtCrateMarka.Text = dgCrate.SelectedRows(0).Cells(1).Value
        txtCrateRate.Text = Format(Val(dgCrate.SelectedRows(0).Cells(2).Value), "0.00")
        dgCrate.Visible = False
        txtCrateQty.Focus()
    End Sub

    Private Sub dgMode_KeyDown(sender As Object, e As KeyEventArgs) Handles dgCrate.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtCrateMarka.Clear()
            txtModeID.Clear()
            txtModeID.Text = dgCrate.SelectedRows(0).Cells(0).Value
            txtCrateMarka.Text = dgCrate.SelectedRows(0).Cells(1).Value
            txtCrateRate.Text = Format(Val(dgCrate.SelectedRows(0).Cells(2).Value), "0.00")
            dgCrate.Visible = False
            txtCrateQty.Focus()
        End If
        If e.KeyCode = Keys.F3 Then
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            If Not CrateForm Is Nothing Then
                CrateForm.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If dgCrate.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = dgCrate.SelectedRows(0).Cells(0).Value
            CrateForm.MdiParent = MainScreenForm
            CrateForm.Show()
            CrateForm.FillControls(tmpAcID)
            If Not CrateForm Is Nothing Then
                CrateForm.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Back Then txtCrateMarka.Focus()
    End Sub


    Private Sub txtCrateQty_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCrateQty.KeyPress, txtCrateAmt.KeyPress, txtCrateRate.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub


    Private Sub btnClose_Click_1(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtAccount_Leave(sender As Object, e As EventArgs) Handles txtAccount.Leave
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        End If
    End Sub

    Private Sub txtCrateRate_TextChanged(sender As Object, e As EventArgs) Handles txtCrateRate.TextChanged, txtCrateQty.TextChanged
        txtCrateAmt.Text = Format(Val(Val(txtCrateQty.Text) * Val(txtCrateRate.Text)), "0.00")
    End Sub

    Private Sub ckCash_KeyDown(sender As Object, e As KeyEventArgs) Handles ckCash.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSave.Focus()
            e.SuppressKeyPress = True
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

    Private Sub txtAccount_TextChanged(sender As Object, e As EventArgs) Handles txtAccount.TextChanged

    End Sub

    Private Sub txtInvoiceID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceID.KeyDown
        If e.KeyCode = Keys.Enter Then pnlInvoiceID.Visible = False : txtAccount.Focus()
    End Sub

    Private Sub Label23_Click(sender As Object, e As EventArgs) Handles Label23.Click

    End Sub

    Private Sub mskEntryDate_MaskInputRejected(sender As Object, e As MaskInputRejectedEventArgs) Handles mskEntryDate.MaskInputRejected

    End Sub
    Private Sub retriveNext()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','+1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        dg1.Rows.Clear()
        If dt.Rows.Count = 0 Then
            Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM CrateVoucher WHERE EntryDate >'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' limit 1")
            If NextDate = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
            dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate ='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' Order by ID Desc")
        End If
        '        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType = 'Crate Out' Order by VoucherID Desc")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(5).Value = dt.Rows(i)("Qty").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub retriveFirst()
        Dim dt As New DataTable
        Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM CrateVoucher WHERE transtype='" & Me.Text & "' Order by EntryDate limit 1")
        '  dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','-1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        dt = clsFun.ExecDataTable("Select * from CrateVoucher where TransType= '" & Me.Text & "'and EntryDate='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' Order By ID")
        dg1.Rows.Clear()
        '        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType = 'Crate Out' Order by VoucherID Desc")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(5).Value = dt.Rows(i)("Qty").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub retrivePrev()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','-1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        dg1.Rows.Clear()
        If dt.Rows.Count = 0 Then
            Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM CrateVoucher WHERE EntryDate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'  and transtype='" & Me.Text & "' ORDER BY EntryDate DESC limit 1")
            If NextDate = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
            dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate ='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' Order by ID Desc")
        End If
        '        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType = 'Crate Out' Order by VoucherID Desc")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(5).Value = dt.Rows(i)("Qty").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        calc() : dg1.ClearSelection()
    End Sub

    Private Sub retriveLast()
        Dim dt As New DataTable
        Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM CrateVoucher WHERE transtype='" & Me.Text & "' Order by EntryDate Desc limit 1")
        '  dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','-1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        dt = clsFun.ExecDataTable("Select * from CrateVoucher where TransType= '" & Me.Text & "'and EntryDate='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' Order by ID Desc")
        dg1.Rows.Clear()

        '        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE EntryDate= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and TransType = 'Crate Out' Order by VoucherID Desc")
        Try
            If dt.Rows.Count > 0 Then
                dg1.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(5).Value = dt.Rows(i)("Qty").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        retriveNext()
    End Sub

    Private Sub btnLast_Click(sender As Object, e As EventArgs) Handles btnLast.Click
        retriveLast()
    End Sub

    Private Sub btnFirst_Click(sender As Object, e As EventArgs) Handles btnFirst.Click
        retriveFirst()
    End Sub

    Private Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        retrivePrev()
    End Sub

    'Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
    '    If ClsFunPrimary.ExecScalarStr("Select InstanceID From API") <> "" AndAlso ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "WhatsApp API" Then
    '        cbType.Visible = True : cbType.SelectedIndex = 0
    '        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
    '        ShowWhatsappContacts() : pnlWhatsapp.Visible = True
    '        pnlWhatsapp.BringToFront()
    '        Exit Sub
    '    ElseIf ClsFunPrimary.ExecScalarStr("Select InstanceID From API") <> "" AndAlso ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
    '        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
    '        pnlWhatsapp.Visible = True : ShowWhatsappContacts()
    '        pnlWhatsapp.BringToFront() : cbType.SelectedIndex = 1 : Exit Sub
    '        Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
    '        If System.IO.File.Exists(WhatsappFile) = False Then
    '            MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
    '            Exit Sub
    '        End If
    '        Dim p() As Process
    '        p = Process.GetProcessesByName("Easy Whatsapp")
    '        If p.Count = 0 Then
    '            Dim StartWhatsapp As New System.Diagnostics.Process
    '            StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
    '            StartWhatsapp.Start()
    '        End If
    '        pnlWhatsapp.Visible = True
    '        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
    '        ShowWhatsappContacts()
    '    Else
    '        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
    '        pnlWhatsapp.Visible = True : ShowWhatsappContacts()
    '        pnlWhatsapp.BringToFront() : cbType.SelectedIndex = 2
    '    End If
    'End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        FillControl()
        If ClsFunPrimary.ExecScalarStr("Select InstanceID From API") <> "" AndAlso ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
            If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
            pnlWhatsapp.Visible = True : ShowWhatsappContacts()
            pnlWhatsapp.BringToFront() : cbType.SelectedIndex = 0 : Exit Sub
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
            pnlWhatsapp.Visible = True
            If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
            ShowWhatsappContacts()
        Else
            If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
            pnlWhatsapp.Visible = True : ShowWhatsappContacts()
            pnlWhatsapp.BringToFront() : cbType.SelectedIndex = 0
        End If
    End Sub
    Sub RowColumsWhatsapp()
        DgWhatsapp.Columns.Clear() : DgWhatsapp.ColumnCount = 10
        Dim headerCellLocation As Point = Me.dg1.GetCellDisplayRectangle(0, -1, True).Location
        'Place the Header CheckBox in the Location of the Header Cell.
        WhatsappCheckBox.Location = New Point(headerCellLocation.X + 10, headerCellLocation.Y + 2)
        WhatsappCheckBox.BackColor = Color.GhostWhite
        WhatsappCheckBox.Size = New Size(18, 18)
        AddHandler WhatsappCheckBox.Click, AddressOf WhatsappCheckBox_Clicked
        DgWhatsapp.Controls.Add(WhatsappCheckBox)
        Dim checkBoxColumn1 As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
        checkBoxColumn1.HeaderText = "" : checkBoxColumn1.Width = 30
        checkBoxColumn1.Name = "checkBoxColumn"
        DgWhatsapp.Columns.Insert(0, checkBoxColumn1)
        DgWhatsapp.Columns(0).ReadOnly = False
        AddHandler DgWhatsapp.CellContentClick, AddressOf DgWhatsapp_CellClick
        DgWhatsapp.Columns(1).Name = "VoucherID" : DgWhatsapp.Columns(1).Visible = False
        DgWhatsapp.Columns(2).Name = "Receipt No." : DgWhatsapp.Columns(2).Width = 120
        DgWhatsapp.Columns(3).Name = "WhatsApp No" : DgWhatsapp.Columns(3).Width = 150
        DgWhatsapp.Columns(4).Name = "Account Name" : DgWhatsapp.Columns(4).Width = 150
        DgWhatsapp.Columns(5).Name = "Status" : DgWhatsapp.Columns(5).Width = 300
        DgWhatsapp.Columns(6).Name = "Path" : DgWhatsapp.Columns(6).Visible = False
        DgWhatsapp.Columns(7).Name = "Mode" : DgWhatsapp.Columns(7).Visible = False
        DgWhatsapp.Columns(8).Name = "Msg1" : DgWhatsapp.Columns(9).Visible = False
        DgWhatsapp.Columns(9).Name = "Msg2" : DgWhatsapp.Columns(9).Visible = False
    End Sub

    Private Sub WhatsappCheckBox_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        DgWhatsapp.EndEdit()
        'Loop and check and uncheck all row CheckBoxes based on Header Cell CheckBox.
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            Dim checkBox As DataGridViewCheckBoxCell = (TryCast(row.Cells("checkBoxColumn"), DataGridViewCheckBoxCell))
            checkBox.Value = WhatsappCheckBox.Checked
        Next
    End Sub

    Private Sub DgWhatsapp_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 1 Then
            'Loop to verify whether all row CheckBoxes are checked or not.
            Dim isChecked As Boolean = True
            For Each row As DataGridViewRow In DgWhatsapp.Rows
                If Convert.ToBoolean(row.Cells("chk").EditedFormattedValue) = False Then
                    isChecked = True
                    Exit For
                End If
            Next
            WhatsappCheckBox.Checked = isChecked
        End If
    End Sub

    Private Sub ShowWhatsappContacts(Optional ByVal condtion As String = "")
        DgWhatsapp.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        Dim SSql As String = "Select * From CrateVoucher Where TransType='Crate Out' and EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(SSql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                DgWhatsapp.Rows.Add()
                With DgWhatsapp.Rows(i)

                    .Cells(1).Value = dt.Rows(i)("VoucherID").ToString()
                    .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                    .Cells(3).Value = clsFun.ExecScalarStr("Select MObile1 From Accounts Where ID='" & Val(dt.Rows(i)("AccountId").ToString()) & "'")
                    .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    '.Cells(7).Value = dt.Rows(i)("SallerName").ToString()
                    Dim OpSql As String = "Select ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                          "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as  Restbal from Accounts   where ID='" & Val(dt.Rows(i)("AccountId").ToString()) & "' and Restbal<>0  ;"
                    Dim OpBal As String = clsFun.ExecScalarStr(OpSql)
                    If Val(OpBal) >= 0 Then
                        OpBal = Format(Math.Abs(Val(OpBal)), "0") & " In"
                    Else
                        OpBal = Format(Math.Abs(Val(OpBal)), "0") & " Out"
                    End If
                    Dim ClSql As String = "Select ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                          "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as  Restbal from Accounts   where ID='" & Val(dt.Rows(i)("AccountId").ToString()) & "' and Restbal<>0   ;"
                    Dim ClBal As String = clsFun.ExecScalarStr(ClSql)
                    If Val(ClBal) >= 0 Then
                        ClBal = Format(Math.Abs(Val(ClBal)), "0.00") & " In"
                    Else
                        ClBal = Format(Math.Abs(Val(ClBal)), "0.00") & " Ont"
                    End If
                    Dim msg As String = "Dear " & .Cells(4).Value & ", " & vbCrLf & " Thank you for your *Crate Out of Qty " & dt.Rows(i)("Qty").ToString() & "* Received today(" & mskEntryDate.Text & ") to *" & compname & "*. Your previous Total Crate balance Was  *₹ " & OpBal & "*. After todays Deposits, your new *total Crate outstanding balance is ₹ " & ClBal & "*."
                    Dim msg2 As String = "प्रिय " & .Cells(4).Value & ", " & vbCrLf & " आज दिनांक (" & mskEntryDate.Text & ") *" & compnameHindi & "* को क्रैट " & dt.Rows(i)("Qty").ToString() & " जमा* कराने के लिए आपका धन्यवाद।  आपका *पुराना क्रैट बकाया  ₹ " & OpBal & "* था। आज के जमा के बाद, आपका नया *कुल बकाया क्रैट " & ClBal & "* है। " & vbCrLf & " *धन्यवाद। " & vbCrLf & " सादर: *" & compnameHindi & "*"

                    .Cells(8).Value = msg
                    .Cells(9).Value = msg2
                    .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                    .Cells(0).Value = True
                End With
            Next i
        End If
        DgWhatsapp.ClearSelection()
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If cbType.SelectedIndex = 0 Then
            Dim WhatsappFile As String = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
            If System.IO.File.Exists(WhatsappFile) = False Then
                MsgBox("Please Contact to Support Officer...", MsgBoxStyle.Critical, "Access Denied")
                Exit Sub
            Else
                Dim p() As Process
                p = Process.GetProcessesByName("Easy Whatsapp")
                If p.Count = 0 Then
                    Dim StartWhatsapp As New System.Diagnostics.Process
                    StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                    StartWhatsapp.Start()
                End If
                SendWhatsappData()
            End If
        End If
    End Sub
    Private Sub SendWhatsappData()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        UpdateProgressBarVisibility(True)
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim sql As String = ""
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            With row
                UpdateProgressBar(row.Index)
                If .Cells(0).Value = True Then
                    If btnRadioEnglish.Checked = True And .Cells(3).Value <> "" Then
                        If RadioPDFMsg.Checked = True Then
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(7).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintReceipts()
                            Pdf_Genrate.ExportReport("\transCrate2.rpt")
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & .Cells(8).Value & "','" & GlobalData.PdfPath & "');"
                        ElseIf RadioPdfOnly.Checked = True Then
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(7).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintReceipts()
                            Pdf_Genrate.ExportReport("\transCrate2.rpt")
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & GlobalData.PdfPath & "');"
                        ElseIf RadioMsgOnly.Checked = True Then
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & .Cells(8).Value & "','');"
                        End If
                    ElseIf RadioRegional.Checked = True And .Cells(3).Value <> "" Then
                        If RadioPDFMsg.Checked = True Then
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(7).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintReceipts()
                            Pdf_Genrate.ExportReport("\transCrate2.rpt")
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & .Cells(9).Value & "','" & GlobalData.PdfPath & "');"
                        ElseIf RadioPdfOnly.Checked = True Then
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(7).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintReceipts()
                            Pdf_Genrate.ExportReport("\transCrate.rpt")
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & GlobalData.PdfPath & "');"
                        ElseIf RadioMsgOnly.Checked = True Then
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & .Cells(9).Value & "','');"
                        End If
                    End If
                End If
            End With
        Next
        If ClsFunWhatsapp.ExecNonQuery(sql) > 0 Then
            sql = "Update Settings Set MinState='N'"
            ClsFunWhatsapp.ExecScalarStr(sql)
            MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        End If
        UpdateProgressBarVisibility(False)
    End Sub
    Private Sub rowColums2()
        tmpgrid.ColumnCount = 10
        tmpgrid.Columns(0).Name = "ID" : tmpgrid.Columns(0).Visible = False
        tmpgrid.Columns(1).Name = "Date" : tmpgrid.Columns(1).Width = 98
        tmpgrid.Columns(2).Name = "Mode" : tmpgrid.Columns(2).Width = 171
        tmpgrid.Columns(3).Name = "Account Name" : tmpgrid.Columns(3).Width = 210
        tmpgrid.Columns(4).Name = "Rcpt No." : tmpgrid.Columns(4).Width = 99
        tmpgrid.Columns(5).Name = "Amount" : tmpgrid.Columns(5).Width = 100
        tmpgrid.Columns(6).Name = "Discount" : tmpgrid.Columns(6).Width = 108
        tmpgrid.Columns(7).Name = "Total" : tmpgrid.Columns(7).Width = 120
        tmpgrid.Columns(8).Name = "Remark" : tmpgrid.Columns(8).Width = 260
        tmpgrid.Columns(9).Name = "AccountID" : tmpgrid.Columns(9).Width = 260
    End Sub

    Private Sub retrive2(VoucherID As Integer)
        tmpgrid.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM CrateVoucher WHERE VoucherID='" & Val(VoucherID) & "'")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        tmpgrid.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                tmpgrid.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    tmpgrid.Rows.Add()
                    With tmpgrid.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("VoucherID").ToString()
                        '  .Cells(0).Value = i + 1
                        .Cells(1).Value = CDate(dt.Rows(i)("Entrydate")).ToString("dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("SlipNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("CrateName").ToString()
                        .Cells(5).Value = dt.Rows(i)("Qty").ToString()
                        .Cells(6).Value = Format(Val(dt.Rows(i)("Rate").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("Amount").ToString()), "0.00")
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        .Cells(9).Value = dt.Rows(i)("AccountID").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
    End Sub
    Private Sub UpdateProgressBar(value As Integer)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Integer)(AddressOf UpdateProgressBar), value)
        Else
            ProgressBar1.Value = value
        End If
    End Sub

    Private Sub UpdateProgressBarVisibility(visible As Boolean)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Boolean)(AddressOf UpdateProgressBarVisibility), visible)
        Else
            ProgressBar1.Visible = visible
        End If
    End Sub
    Private Sub PrintReceipts()
        Dim AllRecord As Integer = Val(tmpgrid.Rows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        Dim count As Integer = 0 : Dim cmd As New SQLite.SQLiteCommand
        Dim FastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        ClsFunPrimary.ExecNonQuery("Delete from printing")
        For i As Integer = 0 To maxRowCount - 1
            FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With tmpgrid.Rows(LastRecord)
                    If .Cells(2).Value <> "" Then
                        Dim OpSql As String = "Select ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                                          "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as  Restbal from Accounts   where ID='" & Val(.Cells(9).Value) & "' and Restbal<>0  ;"
                        Dim OpBal As String = clsFun.ExecScalarStr(OpSql)
                        If Val(OpBal) >= 0 Then
                            OpBal = Format(Math.Abs(Val(OpBal)), "0") & " In"
                        Else
                            OpBal = Format(Math.Abs(Val(OpBal)), "0") & " Out"
                        End If
                        Dim ClSql As String = "Select ((Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate In' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                                              "-(Select ifnull(Sum(Qty),0) From CrateVoucher Where AccountID=Accounts.ID and CrateType='Crate Out' and CrateVoucher.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) as  Restbal from Accounts   where ID='" & Val(.Cells(9).Value) & "' and Restbal<>0   ;"
                        Dim ClBal As String = clsFun.ExecScalarStr(ClSql)
                        If Val(ClBal) >= 0 Then
                            ClBal = Format(Math.Abs(Val(ClBal)), "0.00") & " In"
                        Else
                            ClBal = Format(Math.Abs(Val(ClBal)), "0.00") & " Ont"
                        End If
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & Me.Text & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & .Cells(4).Value & "','" & AmtInWord(Val(.Cells(7).Value)) & "','" & OpBal & "','" & ClBal & "'"
                    End If
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            Try
                MsgBox("Inserting")
                MsgBox(FastQuery)
                If FastQuery = String.Empty Then Exit Sub
                MsgBox(FastQuery)
                Sql = "insert into printing (D1,M1,M2,M3,P1,P2,P3,P4,P5,P6,P7,P8) " & FastQuery & ""
                MsgBox(Sql)
                ClsFunPrimary.ExecNonQuery(Sql)
                MsgBox("Success")
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        Next
        ' pnlWait.Visible = False
    End Sub

End Class