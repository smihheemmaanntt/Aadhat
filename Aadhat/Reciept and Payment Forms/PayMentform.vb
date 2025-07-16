Imports System.IO
Imports System.Threading

Public Class PayMentform
    Dim vno As Integer : Dim ServerTag As Integer
    Private WhatsappCheckBox As CheckBox = New CheckBox()
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

    Private Sub PayMentform_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isBackgroundWorkerRunning Then
            e.Cancel = True
            Me.Hide()
            ' MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Top = 0 : Me.Left = 0
        End If
    End Sub
    Private Sub PayMentform_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            If DgAccountSearch.Visible = True Then
                DgAccountSearch.Visible = False
                Exit Sub
            ElseIf dgMode.Visible = True Then
                dgMode.Visible = False
                Exit Sub
            ElseIf pnlWhatsapp.Visible = True Then
                pnlWhatsapp.Visible = False
                Exit Sub
            Else

                If isBackgroundWorkerRunning Then
                    ' e.Cancel = True
                    Me.Hide()
                    ' MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.Top = 0 : Me.Left = 0
                Else
                    Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "Aadhat")
                    If msgRslt = MsgBoxResult.Yes Then
                        Me.Close() : Exit Sub
                    ElseIf msgRslt = MsgBoxResult.No Then
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub PayMentform_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True : FillControl()
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        rowColums() : VNumber() : FillPayment() : rowColums2()
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

    Private Sub rowColums2()
        tmpgrid.ColumnCount = 9
        tmpgrid.Columns(0).Name = "ID" : tmpgrid.Columns(0).Visible = False
        tmpgrid.Columns(1).Name = "Date" : tmpgrid.Columns(1).Width = 98
        tmpgrid.Columns(2).Name = "Mode" : tmpgrid.Columns(2).Width = 171
        tmpgrid.Columns(3).Name = "Account Name" : tmpgrid.Columns(3).Width = 210
        tmpgrid.Columns(4).Name = "Rcpt No." : tmpgrid.Columns(4).Width = 99
        tmpgrid.Columns(5).Name = "Amount" : tmpgrid.Columns(5).Width = 100
        tmpgrid.Columns(6).Name = "Discount" : tmpgrid.Columns(6).Width = 108
        tmpgrid.Columns(7).Name = "Total" : tmpgrid.Columns(7).Width = 120
        tmpgrid.Columns(8).Name = "Remark" : tmpgrid.Columns(8).Width = 260
    End Sub
    Public Sub FillPayment()
        ssql = "Select * from Controls "
        dt = clsFun.ExecDataTable(ssql)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("PymtDate").ToString().Trim() = "Yes" Then mskEntryDate.TabStop = True Else mskEntryDate.TabStop = False : txtMode.Focus()
            If dt.Rows(0)("PymtSlip").ToString().Trim() = "Yes" Then txtReciptNo.TabStop = True Else txtReciptNo.TabStop = False
            If dt.Rows(0)("PymtDisc").ToString().Trim() = "Yes" Then txtDiscountAmount.TabStop = True Else txtDiscountAmount.TabStop = False
            If dt.Rows(0)("PymtTotal").ToString().Trim() = "Yes" Then txtTotalAmount.TabStop = True Else txtTotalAmount.TabStop = False
            If dt.Rows(0)("PymtRemark").ToString().Trim() = "Yes" Then TxtRemark.TabStop = True Else TxtRemark.TabStop = False
        End If
    End Sub
    Private Sub AcBal()
        'Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtAccountID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtAccountID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtAccountID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Format(Val(opbal) + Val(tmpamtdr), "0.00")
        Else
            tmpamtcr = Format(Val(opbal) + Val(tmpamtcr), "0.00")
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Cr"
        Else
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtAccountID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id= " & Val(txtAccountID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Cr"
            Else
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Dr"
            End If
        End If
        lblAcBal.Visible = True
        lblAcBal.Text = "Bal : " & opbal
    End Sub
    Private Sub CapAcBal()
        'Dim i, j As Integer
        Dim dt As New DataTable
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim opbal As String = ""
        Dim ClBal As String = ""
        opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE ID=  " & Val(txtModeID.Text) & "")
        Dim tmpamtdr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='D' and accountID=" & Val(txtModeID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        Dim tmpamtcr As String = clsFun.ExecScalarStr("Select sum(Amount) as tot from Ledger where Dc='C' and accountID=" & Val(txtModeID.Text) & " and EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        ' opbal = clsFun.ExecScalarStr(" Select (OpBal) FROM Accounts WHERE AccountName like '%" + cbAccountName.Text + "%'")
        Dim drcr As String = clsFun.ExecScalarStr(" Select Dc FROM Accounts WHERE ID= " & Val(txtModeID.Text) & "")
        If drcr = "Dr" Then
            tmpamtdr = Format(Val(opbal) + Val(tmpamtdr), "0.00")
        Else
            tmpamtcr = Format(Val(opbal) + Val(tmpamtcr), "0.00")
        End If
        Dim tmpamt As String = IIf(Val(tmpamtdr) > Val(tmpamtcr), Val(tmpamtdr) - Val(tmpamtcr), Val(tmpamtcr) - Val(tmpamtdr)) '- Val(opbal)
        If drcr = "Cr" Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Cr"
        Else
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " Dr"
        End If
        Dim cntbal As Integer = 0
        cntbal = clsFun.ExecScalarInt("Select count(*) from ledger where  accountid=" & Val(txtModeID.Text) & " and  EntryDate <= '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'")
        If cntbal = 0 Then
            opbal = Format(Val(Math.Abs(Val(opbal))), "0.00") & " " & clsFun.ExecScalarStr(" Select dc from accounts where id=" & Val(txtModeID.Text) & "")
        Else
            If Val(tmpamtcr) > Val(tmpamtdr) Then
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Cr"
            Else
                opbal = Format(Val(Math.Abs(Val(tmpamt))), "0.00") & " Dr"
            End If
        End If
        lblCapAcBal.Visible = True
        lblCapAcBal.Text = "Bal : " & opbal
    End Sub
    Private Sub ModeColums()
        dgMode.ColumnCount = 2
        dgMode.Columns(0).Name = "ID" : dgMode.Columns(0).Visible = False
        dgMode.Columns(1).Name = "Mode Name" : dgMode.Columns(1).Width = 262
        RetriveMode()
    End Sub
    Private Sub RetriveMode(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * from Accounts where groupid in(11,12) " & condtion & " order by AccountName")
        Try
            If dt.Rows.Count > 0 Then
                dgMode.Rows.Clear()
                For i = 0 To dt.Rows.Count - 1
                    dgMode.Rows.Add()
                    With dgMode.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = dt.Rows(i)("AccountName").ToString()
                    End With
                Next

            End If
            dt.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Teller")
        End Try
    End Sub
    Private Sub AccountRowColumns()
        DgAccountSearch.ColumnCount = 3
        DgAccountSearch.Columns(0).Name = "ID" : DgAccountSearch.Columns(0).Visible = False
        DgAccountSearch.Columns(1).Name = "Account Name" : DgAccountSearch.Columns(1).Width = 408
        DgAccountSearch.Columns(2).Name = "City" : DgAccountSearch.Columns(2).Width = 110
        retriveAccounts()
    End Sub
    Private Sub retriveAccounts(Optional ByVal condtion As String = "")
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select ID,AccountName,City From Accounts where not groupid in(11,12) " & condtion & " order by  AccountName Limit 20")
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
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Teller")
        End Try
    End Sub
    Private Sub dgMode_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgMode.CellClick
        txtMode.Clear()
        txtModeID.Clear()
        txtModeID.Text = dgMode.SelectedRows(0).Cells(0).Value
        txtMode.Text = dgMode.SelectedRows(0).Cells(1).Value
        dgMode.Visible = False
        txtAccount.Focus()
    End Sub

    Private Sub dgMode_KeyDown(sender As Object, e As KeyEventArgs) Handles dgMode.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtMode.Clear()
            txtModeID.Clear()
            txtModeID.Text = dgMode.SelectedRows(0).Cells(0).Value
            txtMode.Text = dgMode.SelectedRows(0).Cells(1).Value
            dgMode.Visible = False
            txtAccount.Focus()
        End If
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=12", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If dgMode.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = dgMode.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Back Then txtMode.Focus()
    End Sub
    Private Sub txtAccount_GotFocus(sender As Object, e As EventArgs) Handles txtAccount.GotFocus
        If dgMode.ColumnCount = 0 Then ModeColums()
        If dgMode.RowCount = 0 Then RetriveMode()
        If dgMode.SelectedRows.Count = 0 Then dgMode.Visible = True
        If txtMode.Text.Trim() <> "" Then
            RetriveMode(" And upper(AccountName) Like upper('" & txtMode.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        txtModeID.Text = dgMode.SelectedRows(0).Cells(0).Value
        txtMode.Text = dgMode.SelectedRows(0).Cells(1).Value
        dgMode.Visible = False : CapAcBal()
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(AccountName) Like upper('" & txtAccount.Text.Trim() & "%')")
        Else
            retriveAccounts()
        End If
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
    End Sub
    Private Sub txtAccount_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyDown
        If e.KeyCode = Keys.Down Then DgAccountSearch.Focus()
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
            Dim tmpAcID As Integer = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
    End Sub
    Private Sub DgAccountSearch_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgAccountSearch.CellClick
        txtAccount.Clear()
        txtAccountID.Clear()
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False
        If txtReciptNo.TabStop = True Then txtReciptNo.Focus() Else txtReciveAmount.Focus()
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
            Dim tmpAcID As Integer = DgAccountSearch.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            txtAccount.Clear() : txtAccountID.Clear()
            txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
            txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
            DgAccountSearch.Visible = False
            If txtReciptNo.TabStop = True Then txtReciptNo.Focus() Else txtReciveAmount.Focus()
            e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Back Then txtAccount.Focus()
    End Sub
    Private Sub VNumber()
        'If vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
        '    vno = clsFun.ExecScalarInt("Select Count(ID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        '    txtReciptNo.Text = vno + 1
        '    txtInvoiceID.Text = vno + 1
        'Else
        '    vno = clsFun.ExecScalarInt("Select Max(InvoiceID)  AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
        '    txtReciptNo.Text = vno + 1
        '    txtInvoiceID.Text = vno + 1
        'End If
        Dim vno As Integer = 0
        If vno = Val(clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")) <> 0 Then
            vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtReciptNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        Else
            vno = clsFun.ExecScalarInt("SELECT InvoiceID AS NumberOfProducts FROM Vouchers WHERE id=(SELECT max(id) FROM Vouchers Where TransType='" & Me.Text & "')")
            '   vno = clsFun.ExecScalarInt("Select Max(InvoiceID) AS NumberOfProducts FROM Vouchers Where TransType='" & Me.Text & "'")
            txtReciptNo.Text = vno + 1
            txtInvoiceID.Text = vno + 1
        End If
    End Sub


    Private Sub txtclear()
        BtnDelete.Visible = False : print()
        AcBal() : CapAcBal()
        VNumber() : txtID.Text = ""
        txtReciveAmount.Text = ""
        txtDiscountAmount.Text = ""
        txtTotalAmount.Text = ""
        btnSave.Text = "&Save"
        '  TxtRemark.Text = ""
        If mskEntryDate.TabStop = True Then mskEntryDate.Focus() Else txtMode.Focus()
        retrive()
        btnSave.BackColor = Color.DarkSlateGray
        btnSave.Image = My.Resources.icons8_save_48px
        MainScreenPicture.retrive2()
        retrive()
    End Sub

    Private Sub rowColums()
        dg1.ColumnCount = 9
        dg1.Columns(0).Name = "ID"
        dg1.Columns(0).Visible = False
        dg1.Columns(1).Name = "Date" : dg1.Columns(1).Width = 98
        dg1.Columns(2).Name = "Mode" : dg1.Columns(2).Width = 171
        dg1.Columns(3).Name = "Account Name" : dg1.Columns(3).Width = 210
        dg1.Columns(4).Name = "Rcpt No." : dg1.Columns(4).Width = 99
        dg1.Columns(5).Name = "Amount" : dg1.Columns(5).Width = 100
        dg1.Columns(6).Name = "Discount" : dg1.Columns(6).Width = 108
        dg1.Columns(7).Name = "Total" : dg1.Columns(7).Width = 120
        dg1.Columns(8).Name = "Remark" : dg1.Columns(8).Width = 260
    End Sub
    Private Sub calc()
        txttotNet.Text = Format(0, "0.00") : txtTotDisc.Text = Format(0, "0.00") : txtTotal.Text = Format(0, "0.00")
        For i = 0 To dg1.Rows.Count - 1
            txttotNet.Text = Format(Val(txttotNet.Text) + Val(dg1.Rows(i).Cells(5).Value), "0.00")
            txtTotDisc.Text = Format(Val(txtTotDisc.Text) + Val(dg1.Rows(i).Cells(6).Value), "0.00")
            txtTotal.Text = Format(Val(txtTotal.Text) + Val(dg1.Rows(i).Cells(7).Value), "0.00")
        Next
    End Sub
    Private Sub retrive()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate = '" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "'  Order by ID Desc")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        dg1.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("Sallername").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try
        calc() : dg1.ClearSelection()
    End Sub
    Private Sub retriveNext()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','+1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        dg1.Rows.Clear()
        If dt.Rows.Count = 0 Then
            Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM Vouchers WHERE EntryDate >'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' limit 1")
            If NextDate.Trim = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
            dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate ='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' Order by ID Desc")
        End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("Sallername").ToString()
                        .Cells(4).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try

        calc() : dg1.ClearSelection()
    End Sub

    Private Sub retrivePrev()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','-1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        dg1.Rows.Clear()
        If dt.Rows.Count = 0 Then
            Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM Vouchers WHERE EntryDate <'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' ORDER BY EntryDate DESC limit 1")
            If NextDate.Trim = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
            dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate ='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' Order by ID Desc")
        End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("Sallername").ToString()
                        .Cells(4).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try

        calc() : dg1.ClearSelection()
    End Sub

    Private Sub retriveFirst()
        Dim dt As New DataTable
        Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM Vouchers WHERE transtype='" & Me.Text & "' Order by EntryDate limit 1")
        If NextDate.Trim = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
        '  dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','-1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' Order By ID")
        dg1.Rows.Clear()
        'If dt.Rows.Count = 0 Then
        '    Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM Vouchers WHERE EntryDate >'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' limit 1")
        '    If NextDate = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
        '    dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate ='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' Order by ID Desc")
        'End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("Sallername").ToString()
                        .Cells(4).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try

        calc() : dg1.ClearSelection()
    End Sub

    Private Sub retriveLast()
        Dim dt As New DataTable
        Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM Vouchers WHERE transtype='" & Me.Text & "' Order by EntryDate Desc limit 1")
        If NextDate.Trim = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
        '  dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate = (Select Date('" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','-1 day')) and transtype='" & Me.Text & "' Order by ID Desc")
        dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' Order by ID Desc")
        dg1.Rows.Clear()
        'If dt.Rows.Count = 0 Then
        '    Dim NextDate As String = clsFun.ExecScalarStr("Select EntryDate FROM Vouchers WHERE EntryDate >'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' limit 1")
        '    If NextDate = "" Then MsgBox("No More Record Found", MsgBoxStyle.Critical, "Record Ended") : Exit Sub
        '    dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE EntryDate ='" & CDate(NextDate).ToString("yyyy-MM-dd") & "' and transtype='" & Me.Text & "' Order by ID Desc")
        'End If
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    dg1.Rows.Add()
                    With dg1.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("Sallername").ToString()
                        .Cells(4).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                        mskEntryDate.Text = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try

        calc() : dg1.ClearSelection()
    End Sub
    Public Sub FillControls(ByVal id As Integer)
        Dim ssql As String = String.Empty
        Dim primary As String = String.Empty
        BtnDelete.Visible = True
        Dim dt As New DataTable
        btnSave.BackColor = Color.Coral
        btnSave.Image = My.Resources.icons8_edit_48px
        btnSave.Text = "&Update"
        ssql = "Select * from Vouchers where id=" & id
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            mskEntryDate.Text = Format(dt.Rows(0)("EntryDate"), "dd-MM-yyyy")
            txtModeID.Text = dt.Rows(0)("SallerID").ToString()
            txtMode.Text = dt.Rows(0)("Sallername").ToString()
            txtAccountID.Text = dt.Rows(0)("AccountID").ToString()
            txtAccount.Text = dt.Rows(0)("AccountName").ToString()
            txtReciveAmount.Text = Format(Val(dt.Rows(0)("BasicAmount").ToString()), "0.00")
            txtDiscountAmount.Text = Format(Val(dt.Rows(0)("DiscountAmount").ToString()), "0.00")
            txtTotalAmount.Text = Format(Val(dt.Rows(0)("TotalAmount").ToString()), "0.00")
            TxtRemark.Text = dt.Rows(0)("Remark").ToString()
            txtReciptNo.Text = dt.Rows(0)("BillNo").ToString()
            txtID.Text = dt.Rows(0)("ID").ToString()
            txtInvoiceID.Text = dt.Rows(0)("InvoiceID").ToString()
        End If
    End Sub
    Private Sub save()

        SqliteEntryDate = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim cmd As New SQLite.SQLiteCommand
        If txtMode.Text = "" Then
            txtMode.Focus()
            MsgBox("Please Fill Mode Name... ", MsgBoxStyle.Exclamation, "Empty")
        ElseIf txtTotalAmount.Text = "0" Then
            MsgBox("Please Fill Amount... ", MsgBoxStyle.Exclamation, "Empty")
            txtTotalAmount.Focus()
        ElseIf txtTotalAmount.Text = "" Then
            MsgBox("Please Fill Amount... ", MsgBoxStyle.Exclamation, "Empty")
            txtTotalAmount.Focus()
        Else
            'Dim sql As String = "insert into Vouchers(EntryDate,TransType,SallerID,sallerName,AccountID,AccountName,BasicAmount,DiscountAmount,TotalAmount,Remark, " & _
            '    "billNo,InvoiceID) values (@1, @2, @3,@4,@5,@6,@7,@8,@9,@10,@11,@12)"
            Dim sql As String = "insert into Vouchers (EntryDate,TransType,SallerID,sallerName,AccountID,AccountName, " &
                                "BasicAmount,DiscountAmount,TotalAmount,Remark,billNo,InvoiceID) values (@1, @2, @3,@4,@5,@6,@7,@8,@9,@10,@11,@12)"
            Try
                cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
                cmd.Parameters.AddWithValue("@1", SqliteEntryDate)
                cmd.Parameters.AddWithValue("@2", Me.Text)
                cmd.Parameters.AddWithValue("@3", Val(txtModeID.Text))
                cmd.Parameters.AddWithValue("@4", txtMode.Text)
                cmd.Parameters.AddWithValue("@5", txtAccountID.Text)
                cmd.Parameters.AddWithValue("@6", txtAccount.Text)
                cmd.Parameters.AddWithValue("@7", Val(txtReciveAmount.Text))
                cmd.Parameters.AddWithValue("@8", Val(txtDiscountAmount.Text))
                cmd.Parameters.AddWithValue("@9", Val(txtTotalAmount.Text))
                cmd.Parameters.AddWithValue("@10", TxtRemark.Text)
                cmd.Parameters.AddWithValue("@11", txtReciptNo.Text)
                cmd.Parameters.AddWithValue("@12", Val(txtInvoiceID.Text))
                If cmd.ExecuteNonQuery() > 0 Then
                    txtID.Text = Val(clsFun.ExecScalarInt("Select Max(ID) from Vouchers"))
                    clsFun.ExecScalarStr("Update Vouchers Set PaymentID=" & Val(txtID.Text) & ", PaymentAmount='" & Val(txtTotalAmount.Text) & "' Where ID='" & Val(txtID.Text) & "'")
                    InsertLedger() : ServerTag = 1 : ServerLedger()
                    mskEntryDate.Focus()
                End If
                clsFun.CloseConnection()

                MsgBox("Payment Saved Successfully...", MsgBoxStyle.Information, "Saved")
                txtclear()
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
    End Sub
    Private Sub ServerDb()
        If OrgID = 0 Then Exit Sub
        SqliteEntryDate = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim ReceiptID As Integer = 0
        If btnSave.Text = "&Save" Then
            ReceiptID = clsFun.ExecScalarInt("Select Max(ID) from Vouchers")
        Else
            ClsFunserver.ExecScalarStr("Delete From Vouchers Where ID='" & Val(txtID.Text) & "' and OrgID='" & Val(OrgID) & "'")
            ReceiptID = Val(txtID.Text)
        End If
        Dim sql As String = "insert into Vouchers (ID,EntryDate,TransType,SallerID,sallerName,AccountID,AccountName, " &
                         "BasicAmount,DiscountAmount,TotalAmount,Remark,billNo,InvoiceID,ServerTag,ORGID) values " &
                         "(@1, @2, @3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, ClsFunserver.GetConnection())
            cmd.Parameters.AddWithValue("@1", Val(ReceiptID))
            cmd.Parameters.AddWithValue("@2", CDate(mskEntryDate.Text).ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@3", Me.Text)
            cmd.Parameters.AddWithValue("@4", Val(txtModeID.Text))
            cmd.Parameters.AddWithValue("@5", txtMode.Text)
            cmd.Parameters.AddWithValue("@6", txtAccountID.Text)
            cmd.Parameters.AddWithValue("@7", txtAccount.Text)
            cmd.Parameters.AddWithValue("@8", Val(txtReciveAmount.Text))
            cmd.Parameters.AddWithValue("@9", Val(txtDiscountAmount.Text))
            cmd.Parameters.AddWithValue("@10", Val(txtTotalAmount.Text))
            cmd.Parameters.AddWithValue("@11", TxtRemark.Text)
            cmd.Parameters.AddWithValue("@12", txtReciptNo.Text)
            cmd.Parameters.AddWithValue("@13", Val(txtInvoiceID.Text))
            cmd.Parameters.AddWithValue("@14", Val(1))
            cmd.Parameters.AddWithValue("@15", Val(OrgID))
            cmd.ExecuteNonQuery() : ServerLedger()
        Catch ex As Exception
            MsgBox(ex.Message) : ClsFunserver.CloseConnection()
        End Try
    End Sub
    Private Sub ServerLedger()
        If OrgID = 0 Then Exit Sub
        Dim FastQuery As String = String.Empty
        SqliteEntryDate = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim Remark1 As String = "(" & txtAccount.Text & ") : " & clsFun.ExecScalarStr(" Select 'Payment No. : '|| billNo  ||', Received Amt : ' ||BasicAmount ||', Dis Amt : ' ||DiscountAmount ||', Total Amt : ' ||TotalAmount  From Vouchers Where ID=" & Val(Val(txtID.Text)) & "")
        Dim Remark2 As String = "(" & txtMode.Text & ") : " & clsFun.ExecScalarStr(" Select 'Payment No. : '|| billNo  ||', Received Amt : ' ||BasicAmount ||', Dis Amt : ' ||DiscountAmount ||', Total Amt : ' ||TotalAmount  From Vouchers Where ID=" & Val(Val(txtID.Text)) & "")
        Dim RemarkHindi As String = clsFun.ExecScalarStr(" Select 'रसीद नं. : '|| billNo  ||', प्राप्त राशि : ' ||BasicAmount ||', छूट राशि : ' ||DiscountAmount ||', कुल  राशि : ' ||TotalAmount  From Vouchers Where ID=" & Val(Val(txtID.Text)) & "")

        If Val(txtAccountID.Text) > 0 Then ''Party Account
            ' clsFun.Ledger(0, Val(txtID.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Val(txtReciveAmount.Text), "C", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtAccount.Text, RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text))
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtReciveAmount.Text) & ",'D'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtAccount.Text & "','" & RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "'"
        End If
        If Val(txtDiscountAmount.Text) > 0 Then ''Discount Amount
            '    clsFun.Ledger(0, Val(TxtID.text), SqliteEntryDate, Me.Text, 17, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17"), Val(txtDiscountAmount.Text), "D", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtAccount.Text, RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text))
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(17) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17") & "'," & Val(txtDiscountAmount.Text) & ",'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtAccount.Text & "','" & RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "'"

        End If
        If Val(txtTotalAmount.Text) > 0 Then ''Total Amout
            If txtModeID.Text > 0 Then ''Party Account
                '  clsFun.Ledger(0, Val(TxtID.text), SqliteEntryDate, Me.Text, txtModeID.Text, txtMode.Text, Val(txtTotalAmount.Text), "D", Remark1 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtAccount.Text, RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text))
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtModeID.Text) & ",'" & txtMode.Text & "'," & Val(txtTotalAmount.Text) & ",'C'," & Val(ServerTag) & "," & Val(OrgID) & ",'" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtAccount.Text & "','" & RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        ClsFunserver.FastLedger(FastQuery)
    End Sub
    Private Sub InsertLedger()
        Dim FastQuery As String = String.Empty
        SqliteEntryDate = CDate(Me.mskEntryDate.Text).ToString("yyyy-MM-dd")
        Dim Remark1 As String = clsFun.ExecScalarStr(" Select 'Receipt No. : '|| billNo  ||',  Total Amt : ' ||TotalAmount  From Vouchers Where ID=" & Val(Val(txtID.Text)) & "")
        Dim Remark2 As String = clsFun.ExecScalarStr(" Select 'Receipt No. : '|| billNo  ||',  Total Amt : ' ||TotalAmount  From Vouchers Where ID=" & Val(Val(txtID.Text)) & "")
        Dim RemarkHindi As String = clsFun.ExecScalarStr(" Select 'रसीद नं. : '|| billNo  ||',  कुल  राशि : ' ||TotalAmount  From Vouchers Where ID=" & Val(Val(txtID.Text)) & "")

        If Val(txtAccountID.Text) > 0 Then ''Party Account
            ' clsFun.Ledger(0, Val(txtID.Text), SqliteEntryDate, Me.Text, txtAccountID.Text, txtAccount.Text, Val(txtReciveAmount.Text), "C", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtAccount.Text, RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text))
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'," & Val(txtReciveAmount.Text) & ",'D','" & "(" & txtMode.Text & ") :" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtAccount.Text & "','" & "(" & txtMode.Text & ") :" & RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "'," & Val(txtModeID.Text) & ",'" & txtMode.Text & "'"
        End If
        If Val(txtDiscountAmount.Text) > 0 Then ''Discount Amount
            '    clsFun.Ledger(0, Val(TxtID.text), SqliteEntryDate, Me.Text, 17, clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17"), Val(txtDiscountAmount.Text), "D", Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtAccount.Text, RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text))
            FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(17) & ",'" & clsFun.ExecScalarStr("Select AccountName from Accounts where Id=17") & "'," & Val(txtDiscountAmount.Text) & ",'C','" & "(" & txtAccount.Text & ") :" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtAccount.Text & "','" & "(" & txtAccount.Text & ") :" & RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "'," & Val(txtModeID.Text) & ",'" & txtModeID.Text & "'"

        End If
        If Val(txtTotalAmount.Text) > 0 Then ''Total Amout
            If txtModeID.Text > 0 Then ''Party Account
                '  clsFun.Ledger(0, Val(TxtID.text), SqliteEntryDate, Me.Text, txtModeID.Text, txtMode.Text, Val(txtTotalAmount.Text), "D", Remark1 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text), txtAccount.Text, RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text))
                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(txtID.Text) & ",'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "','" & Me.Text & "'," & Val(txtModeID.Text) & ",'" & txtMode.Text & "'," & Val(txtTotalAmount.Text) & ",'C','" & "(" & txtAccount.Text & ") :" & Remark2 & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "','" & txtAccount.Text & "','" & "(" & txtAccount.Text & ") :" & RemarkHindi & IIf(TxtRemark.Text = "", "", ", Remark :" & TxtRemark.Text) & "'," & Val(txtAccountID.Text) & ",'" & txtAccount.Text & "'"
            End If
        End If
        If FastQuery = String.Empty Then Exit Sub
        clsFun.FastReceipt(FastQuery)

    End Sub
    Private Sub print()
        If clsFun.ExecScalarStr("Select AskPayment from Controls ") = "Payment" Then
            If MessageBox.Show("Are You Sure want to Print Payment Slip ??", "Print Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                PrintRecord()
                Report_Viewer.printReport("\Trans2.rpt")
                Report_Viewer.MdiParent = MainScreenForm
                Report_Viewer.Show()
                Report_Viewer.BringToFront()
            End If
        ElseIf clsFun.ExecScalarStr("Select AskPayment from Controls ") = "Crate+Rec" Then
            If MessageBox.Show("Are You Sure want to Print Payment Slip ??", "Print Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                PrintRecord()
                Report_Viewer.printReport("\Trans2.rpt")
                Report_Viewer.MdiParent = MainScreenForm
                Report_Viewer.Show()
                Report_Viewer.BringToFront()
            End If

        End If


    End Sub
    Private Sub printPR()
        PrintRecord()
        Report_Viewer.printReport("\Trans.rpt")
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        If Not Report_Viewer Is Nothing Then
            Report_Viewer.BringToFront()
        End If
    End Sub
    Private Sub UpdateReceipt()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim cmd As New SQLite.SQLiteCommand
        If txtMode.Text = "" Then
            txtMode.Focus()
            MsgBox("Please Fill Mode Name... ", MsgBoxStyle.Exclamation, "Empty")
        Else
            Dim sql As String = "Update Vouchers SET EntryDate='" & SqliteEntryDate & "',TransType='" & Me.Text & "', " &
                                "SallerID=" & Val(txtModeID.Text) & ",sallerName='" & txtMode.Text & "',AccountID=" & Val(txtAccountID.Text) & "," &
                                "AccountName='" & txtAccount.Text & "',BasicAmount=" & Val(txtReciveAmount.Text) & ",DiscountAmount=" & Val(txtDiscountAmount.Text) & ", " &
                                "TotalAmount=" & Val(txtTotalAmount.Text) & ",Remark='" & TxtRemark.Text & "',billNo='" & txtReciptNo.Text & "',InvoiceID='" & Val(txtInvoiceID.Text) & "' where ID=" & Val(txtID.Text) & ""

            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    clsFun.ExecScalarStr("Update Vouchers Set PaymentID=" & Val(txtID.Text) & ", PaymentAmount='" & Val(txtTotalAmount.Text) & "' Where ID='" & Val(txtID.Text) & "'")
                    clsFun.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & Val(txtID.Text) & "")
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & "")
                    ServerTag = 1 : InsertLedger() : ServerLedger()
                End If

                MsgBox("Payment Updated Successfully...", MsgBoxStyle.Information, "Updated")
                txtclear()
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
    End Sub
    Public Sub MultiUpdateReceipt()
        Dim dt As DateTime
        dt = CDate(Me.mskEntryDate.Text)
        SqliteEntryDate = dt.ToString("yyyy-MM-dd")
        Dim cmd As New SQLite.SQLiteCommand
        If txtMode.Text = "" Then
            txtMode.Focus()
            MsgBox("Please Fill Mode Name... ", MsgBoxStyle.Exclamation, "Empty")
        Else
            Dim sql As String = "Update Vouchers SET EntryDate='" & SqliteEntryDate & "',TransType='" & Me.Text & "', " &
                                "SallerID=" & Val(txtModeID.Text) & ",sallerName='" & txtMode.Text & "',AccountID=" & Val(txtAccountID.Text) & "," &
                                "AccountName='" & txtAccount.Text & "',BasicAmount=" & Val(txtReciveAmount.Text) & ",DiscountAmount=" & Val(txtDiscountAmount.Text) & ", " &
                                "TotalAmount=" & Val(txtTotalAmount.Text) & ",Remark='" & TxtRemark.Text & "',billNo='" & txtReciptNo.Text & "',InvoiceID='" & Val(txtInvoiceID.Text) & "' where ID=" & txtID.Text & ""
            Try
                If clsFun.ExecNonQuery(sql) > 0 Then
                    clsFun.ExecScalarStr("Update Vouchers Set PaymentID=" & Val(txtID.Text) & ", PaymentAmount='" & Val(txtTotalAmount.Text) & "' Where ID='" & Val(txtID.Text) & "'")
                    clsFun.ExecNonQuery("DELETE from Ledger WHERE vourchersID=" & Val(txtID.Text) & "")
                    ClsFunserver.ExecNonQuery("Delete From Ledger Where VourchersID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & "")
                    ServerTag = 1 : InsertLedger() : ServerLedger()
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                clsFun.CloseConnection()
            End Try
        End If
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
            cmd.Parameters.AddWithValue("@3", txtMode.Text)
            cmd.Parameters.AddWithValue("@4", txtAccount.Text)
            cmd.Parameters.AddWithValue("@5", txtReciveAmount.Text)
            cmd.Parameters.AddWithValue("@6", txtDiscountAmount.Text)
            cmd.Parameters.AddWithValue("@7", txtTotalAmount.Text)
            cmd.Parameters.AddWithValue("@8", TxtRemark.Text)
            cmd.Parameters.AddWithValue("@9", txtReciptNo.Text)
            cmd.Parameters.AddWithValue("@10", lblInword.Text)
            If cmd.ExecuteNonQuery() > 0 Then
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            ClsFunPrimary.CloseConnection()
        End Try
    End Sub
    Private Sub Delete()
        If dg1.SelectedRows.Count = 0 Then MsgBox("Please Select Desire Entry to Delete....", MsgBoxStyle.Critical, "Select Desire Entry") : Exit Sub
        '  If clsFun.ExecScalarInt("Select PaymentID From Vouchers Where ID=" & Val(txtID.Text) & "") <> Val(txtID.Text) Then MsgBox("Can't Delete Entry Because Entry Passed By Super Sale Payment", MsgBoxStyle.Critical, "Access Denid") : Exit Sub
        Try
            If MessageBox.Show("Are You Sure want to Delete Receipt No.: " & txtReciptNo.Text & ", account Name : " & txtAccount.Text & ",Total Amount : " & txtTotalAmount.Text & " ??", "Delete Confirmation...", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.Yes Then
                If clsFun.ExecNonQuery("DELETE from Vouchers WHERE ID=" & Val(dg1.SelectedRows(0).Cells(0).Value) & ";  " &
                                       "DELETE from Ledger WHERE vourchersID=" & Val(dg1.SelectedRows(0).Cells(0).Value) & "") > 0 Then
                    ClsFunserver.ExecNonQuery("Delete From  Ledger Where VourchersID=" & Val(txtID.Text) & " and OrgID=" & Val(OrgID) & "")
                    ServerTag = 0 : ServerLedger()
                End If
                MsgBox("Record Deleted Successfully.", MsgBoxStyle.Information + vbOKOnly, "Deleted")
                txtclear()
                MainScreenPicture.lblTotReceiptamt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Payment' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
                MainScreenPicture.lblTotReceipt.Text = Val(clsFun.ExecScalarStr("Select Count(*) from Vouchers Where TransType='Payment' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
                MainScreenPicture.Dashboard() : If Application.OpenForms().OfType(Of Report_Viewer).Any = True Then Report_Viewer.btnPrint.Focus()
                If Application.OpenForms().OfType(Of Payment_Register).Any = True Then Payment_Register.btnShow.PerformClick()
                If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
                If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
                If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txtReciptNo_GotFocus(sender As Object, e As EventArgs) Handles txtReciptNo.GotFocus
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : AcBal()
    End Sub

    Private Sub txtInvoiceID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtInvoiceID.KeyDown
        If e.KeyCode = Keys.Enter Then
            pnlInvoiceID.Visible = False
            txtReciptNo.Focus()
        End If
    End Sub

    Private Sub txtReciveAmount_GotFocus(sender As Object, e As EventArgs) Handles txtReciveAmount.GotFocus

        If txtReciptNo.TabStop = True Then Exit Sub
        '  If txtReciptNo.TabStop = False Then VNumber()
        If DgAccountSearch.ColumnCount = 0 Then AccountRowColumns()
        If DgAccountSearch.RowCount = 0 Then retriveAccounts()
        If DgAccountSearch.SelectedRows.Count = 0 Then DgAccountSearch.Visible = True
        txtAccountID.Text = DgAccountSearch.SelectedRows(0).Cells(0).Value
        txtAccount.Text = DgAccountSearch.SelectedRows(0).Cells(1).Value
        DgAccountSearch.Visible = False : AcBal()
    End Sub
    Private Sub txtMode_GotFocus(sender As Object, e As EventArgs) Handles txtMode.GotFocus, txtAccount.GotFocus,
        txtReciptNo.GotFocus, txtReciveAmount.GotFocus, txtDiscountAmount.GotFocus, txtTotalAmount.GotFocus, TxtRemark.GotFocus
        If txtMode.Focused Then
            If dgMode.ColumnCount = 0 Then ModeColums()
            If dgMode.RowCount = 0 Then RetriveMode()
            If txtMode.Text.Trim() <> "" Then
                dgMode.Visible = True
                RetriveMode(" And upper(AccountName) Like upper('" & txtMode.Text.Trim() & "%')")
            Else
                RetriveMode()
            End If
            If dgMode.SelectedRows.Count = 0 Then dgMode.Visible = True
        End If
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.LightGray
        tb.SelectAll()
    End Sub

    Private Sub txtMode_LostFOcus(sender As Object, e As EventArgs) Handles txtMode.LostFocus, txtAccount.LostFocus,
        txtReciptNo.LostFocus, txtReciveAmount.LostFocus, txtDiscountAmount.LostFocus, txtTotalAmount.LostFocus, TxtRemark.LostFocus
        Dim tb As TextBox = CType(sender, TextBox)
        tb.BackColor = Color.GhostWhite
    End Sub

    Private Sub mskEntryDate_GotFocus(sender As Object, e As EventArgs) Handles mskEntryDate.GotFocus
        mskEntryDate.BackColor = Color.LightGray
        mskEntryDate.SelectAll()
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, txtMode.KeyDown, txtAccount.KeyDown,
     txtReciptNo.KeyDown, txtReciveAmount.KeyDown, txtDiscountAmount.KeyDown, txtTotalAmount.KeyDown, TxtRemark.KeyDown
        If txtReciptNo.Focused Then
            If e.KeyCode = Keys.F2 Then
                pnlInvoiceID.Visible = True
                txtInvoiceID.Focus()
            End If
        End If
        If txtMode.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=12 ", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.F1 Then
                If dgMode.SelectedRows.Count = 0 Then Exit Sub
                Dim tmpID As String = dgMode.SelectedRows(0).Cells(0).Value
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                CreateAccount.FillContros(tmpID)
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.F3 Then
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup", "GroupName", "ID", "")
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
            If e.KeyCode = Keys.F1 Then
                Dim tmpID As String = txtAccountID.Text
                CreateAccount.MdiParent = MainScreenForm
                CreateAccount.Show()
                CreateAccount.FillContros(tmpID)
                If Not CreateAccount Is Nothing Then
                    CreateAccount.BringToFront()
                End If
            End If
        End If
        If txtMode.Focused Then
            If e.KeyCode = Keys.Down Then
                If dgMode.Visible = True Then dgMode.Focus() : Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If txtAccount.Focused Then
            If e.KeyCode = Keys.Down Then
                If DgAccountSearch.Visible = True Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If DgAccountSearch.Visible = False And dgMode.Visible = False Then
            If e.KeyCode = Keys.Down Then
                If dg1.Rows.Count = 0 Then Exit Sub
                dg1.Rows(0).Selected = True : dg1.Focus()
            End If
        End If
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
        Select Case e.KeyCode
            Case Keys.End
                e.Handled = True
                btnSave.Focus()
        End Select
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If isBackgroundWorkerRunning Then
            ' e.Cancel = True
            Me.Hide()
            ' MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Top = 0 : Me.Left = 0
        Else
            Dim msgRslt As MsgBoxResult = MsgBox("Are you Sure Want to Close Entry?", MsgBoxStyle.YesNo, "Aadhat")
            If msgRslt = MsgBoxResult.Yes Then
                Me.Close() : Exit Sub
            ElseIf msgRslt = MsgBoxResult.No Then
            End If
        End If
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Val(txtModeID.Text) = 0 Then txtMode.Focus() : Exit Sub
        If Val(txtAccountID.Text) = 0 Then txtAccount.Focus() : Exit Sub
        If btnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr("Select count(*) from Vouchers where upper(BillNo)=upper('" & txtReciptNo.Text & "') and TransType='" & Me.Text & "'")) >= 1 And txtReciptNo.Text <> 0 Then
                MsgBox("Receipt No. Already Exists...", MsgBoxStyle.Critical, "Access Denied")
                txtReciptNo.Focus() : Exit Sub
            End If
        Else
            If Val(clsFun.ExecScalarStr("Select count(*) from Vouchers where upper(BillNo)=upper('" & txtReciptNo.Text & "') and TransType='" & Me.Text & "'")) > 1 And txtReciptNo.Text <> 0 Then
                MsgBox("Receipt No. Already Exists...", MsgBoxStyle.Critical, "Access Denied")
                txtReciptNo.Focus() : Exit Sub
            End If
        End If
        If btnSave.Text = "&Save" Then
            Dim AddVoucher As String = clsFun.ExecScalarStr("SELECT Save FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Voucher'")
            If AddVoucher <> "Y" Then MsgBox("You Don't Have Rights to  Save Voucher... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            save()
        Else
            Dim ModifyVoucher As String = clsFun.ExecScalarStr("SELECT Modify FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Voucher'")
            If ModifyVoucher <> "Y" Then MsgBox("You Don't Have Rights to  Modify Voucher... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            If clsFun.ExecScalarInt("Select PaymentID From Vouchers Where ID=" & Val(txtID.Text) & "") <> Val(txtID.Text) Then MsgBox("Can't Update Entry Because Entry Passed By Super Sale Payment", MsgBoxStyle.Critical, "Access Denid") : Exit Sub
            UpdateReceipt()
        End If
        MainScreenPicture.lblPaymentAmt.Text = Format(Val(clsFun.ExecScalarStr("Select Sum(TotalAmount) from Vouchers Where TransType='Payment' And EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'")), "0.00")
        MainScreenPicture.lblPayment.Text = Val(clsFun.ExecScalarStr("Select Count(*) FROM Vouchers Where TransType='Payment' and EntryDate='" & CDate(Date.Now).ToString("yyyy-MM-dd") & "'"))
        MainScreenPicture.Dashboard() : If Application.OpenForms().OfType(Of Report_Viewer).Any = True Then Report_Viewer.btnPrint.Focus()
        If Application.OpenForms().OfType(Of Payment_Register).Any = True Then Payment_Register.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Ledger).Any = True Then Ledger.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = True Then OutStanding_Amount_Only.btnShow.PerformClick()
        If Application.OpenForms().OfType(Of Day_book).Any = True Then Day_book.btnShow.PerformClick()
    End Sub
    Private Sub txtReciveAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtReciveAmount.KeyPress, txtDiscountAmount.KeyPress, txtTotalAmount.KeyPress
        e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    End Sub
    Private Sub calculation()
        If String.IsNullOrEmpty(txtReciveAmount.Text) OrElse String.IsNullOrEmpty(txtDiscountAmount.Text) Then Exit Sub
        txtTotalAmount.Text = CDbl(txtReciveAmount.Text) - CDbl(txtDiscountAmount.Text)
    End Sub

    Private Sub txtDiscountAmount_Leave(sender As Object, e As EventArgs) Handles txtDiscountAmount.Leave
        If txtDiscountAmount.Text = "" Then txtDiscountAmount.Text = "0"
    End Sub

    Private Sub txtReciveAmount_Leave(sender As Object, e As EventArgs) Handles txtReciveAmount.Leave
        If txtReciveAmount.Text = "" Then txtReciveAmount.Text = Format(Val(txtReciveAmount.Text), "0.00")
        If txtDiscountAmount.Text = "" Then txtDiscountAmount.Text = Format(Val(txtDiscountAmount.Text), "0.00")
    End Sub
    Private Sub txtReciveAmount_TextChanged(sender As Object, e As EventArgs) Handles txtReciveAmount.TextChanged, txtDiscountAmount.TextChanged, txtTotalAmount.TextChanged
        calculation()
        Try
            lblInword.Text = AmtInWord(txtTotalAmount.Text)
        Catch ex As Exception
            lblInword.Text = ex.ToString
        End Try
    End Sub

    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        dg1.ClearSelection()
    End Sub
    Private Sub dg1_KeyDown(sender As Object, e As KeyEventArgs) Handles dg1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If dg1.RowCount = 0 Then
                Exit Sub
            End If
            FillControls(Val(dg1.SelectedRows(0).Cells(0).Value))
            mskEntryDate.Focus() : e.SuppressKeyPress = True
        End If
        If e.KeyCode = Keys.Up Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Index) = 0 Then mskEntryDate.Focus()
            dg1.ClearSelection()
        End If
        If e.KeyCode = Keys.Down Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Index) = Val(dg1.Rows.Count - 1) Then dg1.Rows(0).Selected = True
        End If
        If e.KeyCode = Keys.Delete Then
            If dg1.SelectedRows.Count = 0 Then Exit Sub
            If Val(dg1.SelectedRows(0).Cells(0).Value) = 0 Then Exit Sub
            Dim Remove As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Voucher'")
            If Remove <> "Y" Then MsgBox("You Don't Have Rights to  Save Voucher... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
            Delete()
        End If

    End Sub

    Private Sub dg1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles dg1.MouseDoubleClick
        If dg1.SelectedRows.Count = 0 Then Exit Sub
        FillControls(Val(dg1.SelectedRows(0).Cells(0).Value))
        'mskEntryDate.Focus()
    End Sub

    Private Sub dg1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellContentClick

    End Sub

    Private Sub cbmode_Leave(sender As Object, e As EventArgs)
        If clsFun.ExecScalarInt("Select count(*)from Accounts") = 0 Then
            Exit Sub
        End If
    End Sub
    Private Sub cbAccountName_Leave(sender As Object, e As EventArgs)
        If clsFun.ExecScalarInt("Select count(*)from Accounts") = 0 Then
            Exit Sub
        End If
    End Sub

    Private Sub mskEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
        If dg1.RowCount = 0 Then
            retrive()
        End If
        '   If btnSave.Text = "&Save" Then VNumber()
    End Sub

    Private Sub cbmode_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub
    Private Sub txtMode_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMode.KeyDown
        If e.KeyCode = Keys.Down Then dgMode.Focus()
        If e.KeyCode = Keys.F3 Then
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            clsFun.FillDropDownList(CreateAccount.cbGroup, "Select ID,GroupName FROM AccountGroup Where ID=12", "GroupName", "ID", "")
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F1 Then
            If dgMode.SelectedRows.Count = 0 Then Exit Sub
            Dim tmpAcID As Integer = dgMode.SelectedRows(0).Cells(0).Value
            CreateAccount.MdiParent = MainScreenForm
            CreateAccount.Show()
            CreateAccount.FillContros(tmpAcID)
            If Not CreateAccount Is Nothing Then
                CreateAccount.BringToFront()
            End If
        End If
    End Sub

    Private Sub txtMode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMode.KeyPress
        dgMode.Visible = True
    End Sub

    Private Sub txtMode_KeyUp(sender As Object, e As KeyEventArgs) Handles txtMode.KeyUp
        If dgMode.ColumnCount = 0 Then ModeColums()
        If dgMode.RowCount = 0 Then RetriveMode()
        If txtMode.Text.Trim() <> "" Then
            RetriveMode(" And upper(AccountName) Like upper('" & txtMode.Text.Trim() & "%')")
        Else
            RetriveMode()
        End If
        If dgMode.SelectedRows.Count = 0 Then dgMode.Visible = True
    End Sub


    Private Sub txtAccount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAccount.KeyPress
        DgAccountSearch.Visible = True
    End Sub

    Private Sub txtAccount_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAccount.KeyUp
        If txtAccount.Text.Trim() <> "" Then
            retriveAccounts(" And upper(accountname) Like upper('" & txtAccount.Text.Trim() & "%')")
        End If
        If e.KeyCode = Keys.Escape Then DgAccountSearch.Visible = False
    End Sub

    'Private Sub txtReciptNo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtReciptNo.KeyPress
    '    e.Handled = Not (Char.IsDigit(e.KeyChar) Or Asc(e.KeyChar) = 8 Or ((e.KeyChar = ".") And (sender.Text.IndexOf(".") = -1)))
    'End Sub
    Private Sub txtReciptNo_Leave(sender As Object, e As EventArgs) Handles txtReciptNo.Leave
        If btnSave.Text = "&Save" Then
            If Val(clsFun.ExecScalarStr("Select count(*) from Vouchers where upper(BillNo)=upper('" & txtReciptNo.Text & "') and TransType='" & Me.Text & "'")) >= 1 And Val(txtReciptNo.Text) <> 0 Or txtReciptNo.Text = "" Then
                MsgBox("Receipt No. Already Exists...", MsgBoxStyle.Critical, "Access Denied")
                txtReciptNo.Focus() : Exit Sub
            End If
        Else
            If Val(clsFun.ExecScalarStr("Select count(*) from Vouchers where upper(BillNo)=upper('" & txtReciptNo.Text & "') and TransType='" & Me.Text & "'")) > 1 And Val(txtReciptNo.Text) <> 0 Or txtReciptNo.Text = "" Then
                MsgBox("Receipt No. Already Exists...", MsgBoxStyle.Critical, "Access Denied")
                txtReciptNo.Focus() : Exit Sub
            End If
        End If

        txtInvoiceID.Text = txtReciptNo.Text
        'If btnSave.Text = "&Save" Then
        '    If clsFun.ExecScalarStr("Select count(*)from Vouchers where TransType='Payment' and  BillNo='" & Val(txtReciptNo.Text) & "'") >= 1 Then
        '        MsgBox("Receipt Already Exists...", vbOkOnly, "Access Denied")
        '        txtReciptNo.Focus() : txtReciptNo.Text = Val(txtReciptNo.Text) + 1
        '    End If
        'End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        Dim Remove As String = clsFun.ExecScalarStr("SELECT Remove FROM UserRights AS UR INNER JOIN Users AS U ON UR.UserTypeID = U.UserTypeID Where UserName='" & MainScreenPicture.lblUser.Text & "' and EntryType='Voucher'")
        If Remove <> "Y" Then MsgBox("You Don't Have Rights to  Save Voucher... " & vbNewLine & " Please Contact to Admin...Contact to Owner...", MsgBoxStyle.Critical, "Access Denied") : Exit Sub
        Delete()
    End Sub

    Private Sub txtReciptNo_TextChanged(sender As Object, e As EventArgs) Handles txtReciptNo.TextChanged

    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        If mskEntryDate.Enabled = False Then Exit Sub
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = clsFun.convdate(mskEntryDate.Text)
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        'retriveByDate()
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

    Private Sub mskEntryDate_LostFocus(sender As Object, e As EventArgs) Handles mskEntryDate.LostFocus
        mskEntryDate.BackColor = Color.GhostWhite
    End Sub

    Private Sub ShowWhatsappContacts(Optional ByVal condtion As String = "")
        DgWhatsapp.Rows.Clear()
        Dim dt As New DataTable
        Dim i As Integer
        Dim count As Integer = 0
        Dim SSql As String = "Select * From Vouchers Where TransType='Payment' and EntryDate='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'"
        dt = clsFun.ExecDataTable(SSql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                DgWhatsapp.Rows.Add()
                With DgWhatsapp.Rows(i)
                    .Cells(1).Value = dt.Rows(i)("ID").ToString()
                    .Cells(2).Value = dt.Rows(i)("BillNo").ToString()
                    .Cells(3).Value = clsFun.ExecScalarStr("Select MObile1 From Accounts Where ID='" & Val(dt.Rows(i)("AccountId").ToString()) & "'")
                    .Cells(4).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(7).Value = dt.Rows(i)("SallerName").ToString()
                    .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                    .Cells(0).Value = True
                End With
            Next i
        End If
        DgWhatsapp.ClearSelection()
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
    Sub RowColumsWhatsapp()
        DgWhatsapp.Columns.Clear() : DgWhatsapp.ColumnCount = 8
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
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(1).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintPayments()
                            Pdf_Genrate.ExportReport("\Trans.rpt")
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & .Cells(8).Value & "','" & GlobalData.PdfPath & "');"
                        ElseIf RadioPdfOnly.Checked = True Then
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(1).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintPayments()
                            Pdf_Genrate.ExportReport("\Trans.rpt")
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & GlobalData.PdfPath & "');"
                        ElseIf RadioMsgOnly.Checked = True Then
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & .Cells(8).Value & "','');"
                        End If
                    ElseIf RadioRegional.Checked = True And .Cells(3).Value <> "" Then
                        If RadioPDFMsg.Checked = True Then
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(1).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintPayments()
                            Pdf_Genrate.ExportReport("\Trans.rpt")
                            sql = sql & "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) values  " & _
                             "('" & Val(.Cells(0).Value) & "','" & .Cells(4).Value & "','" & "91" & .Cells(3).Value & "','" & .Cells(9).Value & "','" & GlobalData.PdfPath & "');"
                        ElseIf RadioPdfOnly.Checked = True Then
                            GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(1).Value & ")-" & mskEntryDate.Text & ".pdf"
                            retrive2(.Cells(1).Value) : PrintPayments()
                            Pdf_Genrate.ExportReport("\Trans.rpt")
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

    Private Sub PrintPayments()
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
                        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & .Cells(1).Value & "','" & Me.Text & "','" & .Cells(2).Value & "','" & .Cells(3).Value & "'," &
                             "'" & .Cells(5).Value & "','" & .Cells(6).Value & "','" & .Cells(7).Value & "','" & .Cells(8).Value & "','" & .Cells(4).Value & "','" & AmtInWord(Val(.Cells(7).Value)) & "'"
                    End If
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            Try
                If FastQuery = String.Empty Then Exit Sub
                Sql = "insert into printing (D1,M1,M2,M3,P1,P2,P3,P4,P5,P6) " & FastQuery & ""
                ClsFunPrimary.ExecNonQuery(Sql)
            Catch ex As Exception
                MsgBox(ex.Message)
                ClsFunPrimary.CloseConnection()
            End Try
        Next
        ' pnlWait.Visible = False
    End Sub
    Private Sub retrive2(VoucherID As Integer)
        tmpgrid.Rows.Clear()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select * FROM Vouchers WHERE id='" & Val(VoucherID) & "'")
        'dt = clsFun.ExecDataTable("Select * from Vouchers where TransType= '" & Me.Text & "'and EntryDate='" & mskEntryDate.Text & "'")
        tmpgrid.Rows.Clear()
        Try
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    tmpgrid.Rows.Add()
                    With tmpgrid.Rows(i)
                        .Cells(0).Value = dt.Rows(i)("id").ToString()
                        .Cells(1).Value = Format(dt.Rows(i)("Entrydate"), "dd-MM-yyyy")
                        .Cells(2).Value = dt.Rows(i)("Sallername").ToString()
                        .Cells(3).Value = dt.Rows(i)("AccountName").ToString()
                        .Cells(4).Value = dt.Rows(i)("BillNo").ToString()
                        .Cells(5).Value = Format(Val(dt.Rows(i)("BasicAmount").ToString()), "0.00")
                        .Cells(6).Value = Format(Val(dt.Rows(i)("DiscountAmount").ToString()), "0.00")
                        .Cells(7).Value = Format(Val(dt.Rows(i)("TotalAmount").ToString()), "0.00")
                        .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                        .Cells(8).Value = dt.Rows(i)("Remark").ToString()
                    End With
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "AADHAT")
        End Try

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If cbType.SelectedIndex = 0 Then
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
            SendWhatsappData()
       
        End If
    End Sub
    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
            ' MsgBox("A background task is running. you can Use your Task", MsgBoxStyle.Information, "Background Task")
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
    End Sub

    Private Sub WhatsAppDesktop()
        Dim directoryName As String = Application.StartupPath & "\Pdfs"
        For Each deleteFile In Directory.GetFiles(directoryName, "*.pdf*", SearchOption.TopDirectoryOnly)
            File.Delete(deleteFile)
        Next
        ' rowColums1()
        UpdateProgressBarVisibility(True)
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim fastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        WABA.ExecNonQuery("Delete from SendingData")
        For Each row As DataGridViewRow In DgWhatsapp.Rows
            With row
                UpdateProgressBar(count)
                If .Cells(0).Value = True Then
                    If .Cells(3).Value <> "" Then
                        GlobalData.PdfName = .Cells(4).Value & "(" & .Cells(7).Value & ")-" & mskEntryDate.Text & ".pdf"
                        retrive2(.Cells(1).Value) : PrintPayments()
                        Pdf_Genrate.ExportReport("\Trans.rpt")
                        whatsappSender.FilePath = whatsappSender.UploadFile(Application.StartupPath & "\Pdfs\" & GlobalData.PdfName)
                        fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(4).Value & "','" & .Cells(3).Value & "', " &
                       "'" & .Cells(8).Value & "', '" & whatsappSender.FilePath & "'"
                    End If
                End If
            End With
            count += 1
        Next
        Try
            Sql = "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) " & fastQuery & ";Update Settings Set MinState='N'"
            WABA.ExecNonQuery(Sql)
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
        Catch ex As Exception
            MsgBox(ex.Message)
            UpdateProgressBarVisibility(False)
            WABA.CloseConnection()
        End Try
        UpdateProgressBarVisibility(False)
        'Cursor.Current = Cursors.WaitCursor
        'Dim GAP1 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP1 From API")) & "000"
        'Dim GAP2 As Integer = Val(ClsFunPrimary.ExecScalarInt("Select GAP2 From API")) & "000"
        'Dim Response As String = String.Empty
        'Dim i As Integer = 0
        'Dim whatsappURL As String = String.Empty
        'Dim sourceFilePath As String = String.Empty
        'Try
        '    whatsappURL = "whatsapp://send?"
        '    Dim psi As New ProcessStartInfo(whatsappURL)
        '    Dim process As Process = process.Start(psi)
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        '    Exit Sub
        'End Try

        'If Directory.Exists(Application.StartupPath & "\Whatsapp\Pdfs") = False Then
        '    Directory.CreateDirectory(Application.StartupPath & "\Whatsapp\Pdfs")
        'End If
        'Dim directoryName As String = Application.StartupPath & "\Whatsapp\Pdfs"
        'For Each deleteFile In Directory.GetFiles(directoryName, "*.PDf*", SearchOption.TopDirectoryOnly)
        '    File.Delete(deleteFile)
        'Next
        'For i = 0 To DgWhatsapp.Rows.Count - 1

        '    With DgWhatsapp.Rows(i)
        '        If .Cells(0).Value = True AndAlso .Cells(3).Value.ToString.Trim <> "" Then
        '            GlobalData.PdfName = .Cells(4).Value & "-" & mskEntryDate.Text & ".pdf"
        '            retrive2(.Cells(1).Value) : PrintReceipts()
        '            Pdf_Genrate.ExportReport("\Trans.rpt")
        '            sourceFilePath = GlobalData.PdfPath
        '            whatsappURL = "whatsapp://send?phone=91" & .Cells(3).Value.ToString.Trim
        '            Response = "Receipt Sent Successful"
        '        End If
        '        If .Cells(3).Value.ToString.Trim <> "" Then
        '            Dim psi As New ProcessStartInfo(whatsappURL)
        '            psi.UseShellExecute = True
        '            psi.WindowStyle = ProcessWindowStyle.Normal
        '            Dim process As Process = process.Start(psi)
        '            psi.WindowStyle = ProcessWindowStyle.Minimized
        '            Thread.Sleep(GAP1)
        '            SendKeys.SendWait("^(+f)")
        '            SendKeys.SendWait("{ESCAPE}")
        '            Clipboard.SetData(DataFormats.FileDrop, {sourceFilePath})
        '            If i = 0 Then Thread.Sleep(1000)
        '            SendKeys.SendWait("^(v)")
        '            Thread.Sleep(GAP2)
        '            SendKeys.SendWait("{ENTER}")
        '            .Cells(5).Value = Response
        '        End If
        '    End With
        'Next
        'Thread.Sleep(GAP1)
        'MsgBox("WhatsApp Send to Successful", MsgBoxStyle.Information, "Sent")
        'Dim processName As String = "WhatsApp"
        'Dim proc As New ProcessStartInfo(processName)
        'Dim processes() As Process = Process.GetProcessesByName(processName)
        'If processes.Length > 0 Then
        '    ' Close each instance of the process
        '    For Each p As Process In processes
        '        Thread.Sleep(1000)
        '        p.Kill()
        '        'proc.WindowStyle = ProcessWindowStyle.Minimized
        '    Next

        '    '    MsgBox("WhatsApp Send to Successful", MsgBoxStyle.Information, "Sent")
        '    Cursor.Current = Cursors.Default
        'Else
        '    ' The process was not found
        '    Console.WriteLine("Process not found.")
        'End If
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
    Private Sub lblInword_Click(sender As Object, e As EventArgs) Handles lblInword.Click

    End Sub

    Private Sub btnPnlVisHide_Click(sender As Object, e As EventArgs) Handles btnPnlVisHide.Click
        Me.Close()
    End Sub
End Class