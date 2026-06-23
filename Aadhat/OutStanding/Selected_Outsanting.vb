Imports System.IO

Imports System.Collections.Generic

Public Class Selected_Outsanting
    Private Const MobileSendingMethod As String = "From Mobile"
    Private WhatsappCheckBox As CheckBox = New CheckBox()
    Dim FilePath As String : Dim hostedFilePath As String
    Dim access_token As String = "649299554c995"
    Dim instance_id As String = ClsFunPrimary.ExecScalarStr("Select InstanceID From API")
    Dim APIResposne As String : Dim whatsappSender As New WhatsAppSender()
    Private WithEvents bgWorker As New System.ComponentModel.BackgroundWorker
    Private isBackgroundWorkerRunning As Boolean = False

    Public Sub New()
        InitializeComponent()
        clsFun.DoubleBuffered(dg1, True)
        bgWorker.WorkerSupportsCancellation = True
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub mskEntryDate_KeyDown(sender As Object, e As KeyEventArgs) Handles mskEntryDate.KeyDown, btnShow.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub



    Private Sub mskEntryDate_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mskEntryDate.Validating
        mskEntryDate.Text = smartDate(mskEntryDate.Text)
    End Sub

    Private Sub OutStanding_Amount_Only_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isBackgroundWorkerRunning Then
            '  e.Cancel = True
            Me.Hide()
            '    MessageBox.Show("The process is still running. The form will be hidden instead of closed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Left = 0 : Me.Top = 0
        Else
            Me.Dispose()
        End If
    End Sub

    Private Sub OutStanding_Amount_Only_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If pnlWhatsapp.Visible = True Then pnlWhatsapp.Visible = False : mskEntryDate.Focus() : Exit Sub
            If isBackgroundWorkerRunning Then
                Me.Hide()
                Me.Left = 0 : Me.Top = 0
            Else
                Me.Dispose()
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
            Ledger.txtFromDate.Text = smartDate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
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
        Ledger.txtFromDate.Text = smartDate(CDate(clsFun.ExecScalarStr("Select YearStart From Company")).ToString("dd-MM-yyyy"))
        Ledger.btnShow.PerformClick()
    End Sub

    Private Sub OutStanding_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        mskEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        RadioSundryDebtors.Checked = True
        rowColumns()
    End Sub
    Private Sub txtCustomerSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCustomerSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtCustomerSearch.Text.Trim() <> "" Then
                retrive("And AccountName  Like '" & txtCustomerSearch.Text.Trim() & "%'")
            End If
            If txtCustomerSearch.Text.Trim() = "" Then
                retrive()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub
    Private Sub txtAreaSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtAreaSearch.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtAreaSearch.Text.Trim() <> "" Then
                retrive("And Area  Like '" & txtAreaSearch.Text.Trim() & "%'")
            End If
            If txtAreaSearch.Text.Trim() = "" Then
                retrive()
            End If
            e.SuppressKeyPress = True
        End If

    End Sub
    Private Sub dtp1_GotFocus(sender As Object, e As EventArgs) Handles dtp1.GotFocus
        mskEntryDate.Focus()
    End Sub

    Private Sub dtp1_ValueChanged(sender As Object, e As EventArgs) Handles dtp1.ValueChanged
        mskEntryDate.Text = dtp1.Value.ToString("dd-MM-yyyy")
        mskEntryDate.Text = smartDate(mskEntryDate.Text)
    End Sub
    Private Sub txtGreater_KeyUp(sender As Object, e As KeyEventArgs) Handles txtGrater.KeyUp
        If e.KeyCode = Keys.Enter Then
            retrive()
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub txtLess_KeyUp(sender As Object, e As KeyEventArgs) Handles txtLess.KeyUp
        If e.KeyCode = Keys.Enter Then
            retrive()
            e.SuppressKeyPress = True
        End If
    End Sub

    ' ---------- Declarations ----------
    Private headerCheckBox As CheckBox = Nothing
    Private dt As DataTable

    ' ---------- Setup columns (call once, e.g., Form.Load) ----------
    Private Sub rowColumns()
        dg1.Columns.Clear()
        ' create columns
        dg1.ColumnCount = 8
        dg1.Columns(0).Name = "ID" : dg1.Columns(0).Visible = False
        ' replace column 1 with checkbox column
        Dim chkCol As New DataGridViewCheckBoxColumn()
        chkCol.Name = "Select"
        chkCol.HeaderText = ""
        chkCol.Width = 40
        chkCol.ReadOnly = False
        dg1.Columns.RemoveAt(1)
        dg1.Columns.Insert(1, chkCol)

        dg1.Columns(2).Name = "Account Name" : dg1.Columns(2).Width = 300
        dg1.Columns(3).Name = "Area" : dg1.Columns(3).Width = 300
        dg1.Columns(4).Name = "Mobile No." : dg1.Columns(4).Width = 170
        dg1.Columns(5).Name = "Op Bal" : dg1.Columns(5).Width = 170
        dg1.Columns(6).Name = "Balance" : dg1.Columns(6).Width = 170
        dg1.Columns(7).Name = "OtherName" : dg1.Columns(7).Visible = False

        dg1.AllowUserToAddRows = False
        dg1.ReadOnly = False              ' default readonly
        dg1.Columns("Select").ReadOnly = False
        dg1.EditMode = DataGridViewEditMode.EditOnEnter
        dg1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dg1.MultiSelect = False

        ' add header checkbox
        If headerCheckBox IsNot Nothing Then
            RemoveHandler headerCheckBox.CheckedChanged, AddressOf HeaderCheckBox_CheckedChanged
            dg1.Controls.Remove(headerCheckBox)
            headerCheckBox = Nothing
        End If
        AddHeaderCheckBox()
    End Sub

    ' ---------- Header checkbox (Select All) ----------
    Private Sub AddHeaderCheckBox()
        headerCheckBox = New CheckBox()
        headerCheckBox.Size = New Size(15, 15)
        headerCheckBox.BackColor = Color.Transparent
        headerCheckBox.ThreeState = False
        AddHandler headerCheckBox.CheckedChanged, AddressOf HeaderCheckBox_CheckedChanged

        dg1.Controls.Add(headerCheckBox)
        RepositionHeaderCheckBox()
        AddHandler dg1.Scroll, AddressOf dg1_Scroll
        AddHandler dg1.ColumnWidthChanged, AddressOf dg1_ColumnWidthChanged
        AddHandler dg1.SizeChanged, AddressOf dg1_SizeChanged
    End Sub

    Private Sub RepositionHeaderCheckBox()
        If headerCheckBox Is Nothing Then Return
        Dim rect As Rectangle = dg1.GetCellDisplayRectangle(1, -1, True)
        headerCheckBox.Location = New Point(rect.X + (rect.Width - headerCheckBox.Width) \ 2, rect.Y + (rect.Height - headerCheckBox.Height) \ 2)
    End Sub

    Private Sub dg1_Scroll(s As Object, e As ScrollEventArgs) Handles dg1.Scroll
        RepositionHeaderCheckBox()
    End Sub
    Private Sub dg1_ColumnWidthChanged(s As Object, e As DataGridViewColumnEventArgs) Handles dg1.ColumnWidthChanged
        RepositionHeaderCheckBox()
    End Sub
    Private Sub dg1_SizeChanged(s As Object, e As EventArgs) Handles dg1.SizeChanged
        RepositionHeaderCheckBox()
    End Sub

    Private Sub HeaderCheckBox_CheckedChanged(sender As Object, e As EventArgs)
        If headerCheckBox Is Nothing Then Return
        Dim isChecked As Boolean = headerCheckBox.Checked
        For Each r As DataGridViewRow In dg1.Rows
            If Not r.IsNewRow Then r.Cells("Select").Value = isChecked
        Next
        CalculateTotals()
    End Sub

    ' ---------- Main retrive (paste your SQL logic here unchanged) ----------
    Public Sub retrive(Optional ByVal condtion As String = "")
        dg1.Rows.Clear()
        txtDebitBal.Text = "0.00" : txtCreditBal.Text = "0.00" : TxtGrandTotal.Text = "0.00"
        Dim sql As String = String.Empty
        Dim filterCondition As String = ""

        ' --- your existing filter building logic (kept intact) ---
        If IsNumeric(txtGrater.Text) AndAlso IsNumeric(txtLess.Text) AndAlso
           Val(txtGrater.Text) > 0 AndAlso Val(txtLess.Text) > 0 Then
            Dim minVal As Double = Math.Min(Val(txtGrater.Text), Val(txtLess.Text))
            Dim maxVal As Double = Math.Max(Val(txtGrater.Text), Val(txtLess.Text))
            filterCondition &= " AND RestBal BETWEEN " & minVal & " AND " & maxVal
        Else
            If IsNumeric(txtGrater.Text) AndAlso Val(txtGrater.Text) > 0 Then filterCondition &= " AND RestBal >= " & Val(txtGrater.Text)
            If IsNumeric(txtLess.Text) AndAlso Val(txtLess.Text) > 0 Then filterCondition &= " AND RestBal <= " & Val(txtLess.Text)
        End If

        ' --- your existing radios / sql construction (unchanged) ---
        If ckAreaWise.Checked = True Then
            If RadioSundryDebtors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
                 "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(16,32) " & condtion & filterCondition & " Order by Upper(Area),upper(AccountName)  ;"
            ElseIf RadioSundryCreditors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
                 "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(17,33)  " & condtion & filterCondition & " Order by Upper(Area),upper(AccountName) ;"
            ElseIf RadioAll.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
                 "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 " & condtion & filterCondition & " Order by Upper(Area),upper(AccountName) ;"
            End If
        Else
            If RadioSundryDebtors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
                 "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(16,32) " & condtion & filterCondition & " Order by upper(AccountName) ;"
            ElseIf RadioSundryCreditors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
                 "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(17,33)  " & condtion & filterCondition & " Order by upper(AccountName) ;"
            ElseIf RadioAll.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
                 "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
                 " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
                 " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 " & condtion & filterCondition & " Order by upper(AccountName) ;"
            End If
        End If

        dt = clsFun.ExecDataTable(sql)
        If dt Is Nothing Then Return
        If Val(dt.Rows.Count) = Val(dg1.Rows.Count) Then Exit Sub

        If Val(dt.Rows.Count) > 20 Then dg1.Columns(6).Width = 150
        dg1.Rows.Clear()

        For i As Integer = 0 To dt.Rows.Count - 1
            lblRecordCount.Visible = True
            lblRecordCount.Text = "Total Records : " & dt.Rows.Count
            dg1.Rows.Add()
            With dg1.Rows(i)
                dg1.ClearSelection()
                ' Only Select column editable
                dg1.Columns(0).ReadOnly = True : dg1.Columns(1).ReadOnly = False
                dg1.Columns(2).ReadOnly = True : dg1.Columns(3).ReadOnly = True
                dg1.Columns(4).ReadOnly = True : dg1.Columns(5).ReadOnly = True
                dg1.Columns(6).ReadOnly = True : dg1.Columns(7).ReadOnly = True

                .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleRight

                .Cells(0).Value = dt.Rows(i)("ID").ToString()
                .Cells(1).Value = False
                .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                .Cells(3).Value = dt.Rows(i)("area").ToString()
                .Cells(4).Value = dt.Rows(i)("Mobile1").ToString()
                .Cells(5).Value = Format(Val(dt.Rows(i)("Opbal").ToString()), "0.00") & "  " & dt.Rows(i)("DC").ToString()
                .Cells(6).Value = IIf(Val(dt.Rows(i)("Restbal").ToString()) > 0, Format(Val(dt.Rows(i)("Restbal").ToString()), "0.00") & " " & "Dr", Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00") & " " & "Cr")
                .Cells(7).Value = dt.Rows(i)("OtherName").ToString()

                Dim numericRest As Double = 0
                Double.TryParse(dt.Rows(i)("Restbal").ToString(), numericRest)
                .Tag = numericRest
            End With
        Next

        dg1.EditMode = DataGridViewEditMode.EditOnEnter
        SyncHeaderCheckBox()
        CalculateTotals()
    End Sub

    ' ---------- Totals calculation (use everywhere) ----------
    Private Sub CalculateTotals()
        Dim anySelected As Boolean = False
        For Each r As DataGridViewRow In dg1.Rows
            If Not r.IsNewRow Then
                Dim sel As Boolean = False
                If r.Cells("Select").Value IsNot Nothing Then Boolean.TryParse(r.Cells("Select").Value.ToString(), sel)
                If sel Then anySelected = True : Exit For
            End If
        Next

        Dim sumDebit As Double = 0, sumCredit As Double = 0
        If anySelected Then
            For Each r As DataGridViewRow In dg1.Rows
                If Not r.IsNewRow Then
                    Dim sel As Boolean = False
                    If r.Cells("Select").Value IsNot Nothing Then Boolean.TryParse(r.Cells("Select").Value.ToString(), sel)
                    If sel Then
                        Dim v As Double = 0
                        If r.Tag IsNot Nothing Then Double.TryParse(r.Tag.ToString(), v)
                        If v > 0 Then sumDebit += v Else sumCredit += Math.Abs(v)
                    End If
                End If
            Next
        Else
            For Each r As DataGridViewRow In dg1.Rows
                If Not r.IsNewRow Then
                    Dim v As Double = 0
                    If r.Tag IsNot Nothing Then Double.TryParse(r.Tag.ToString(), v)
                    If v > 0 Then sumDebit += v Else sumCredit += Math.Abs(v)
                End If
            Next
        End If

        txtDebitBal.Text = Format(sumDebit, "0.00")
        txtCreditBal.Text = Format(sumCredit, "0.00")
        Dim grand As Double = sumDebit - sumCredit
        TxtGrandTotal.Text = If(grand > 0, Format(grand, "0.00") & " Dr", Format(Math.Abs(grand), "0.00") & " Cr")
    End Sub

    ' ---------- Sync header checkbox state ----------
    Private Sub SyncHeaderCheckBox()
        If headerCheckBox Is Nothing Then Return
        Dim allChecked As Boolean = True
        For Each r As DataGridViewRow In dg1.Rows
            If Not r.IsNewRow Then
                Dim val As Boolean = False
                If r.Cells("Select").Value IsNot Nothing Then Boolean.TryParse(r.Cells("Select").Value.ToString(), val)
                If Not val Then allChecked = False : Exit For
            End If
        Next
        RemoveHandler headerCheckBox.CheckedChanged, AddressOf HeaderCheckBox_CheckedChanged
        headerCheckBox.Checked = allChecked
        AddHandler headerCheckBox.CheckedChanged, AddressOf HeaderCheckBox_CheckedChanged
    End Sub

    ' ---------- Event handlers for realtime update ----------
    Private Sub dg1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles dg1.CurrentCellDirtyStateChanged
        If dg1.IsCurrentCellDirty Then
            dg1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            CalculateTotals()
            SyncHeaderCheckBox()
        End If
    End Sub

    Private Sub dg1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellValueChanged
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = dg1.Columns("Select").Index Then
            CalculateTotals()
            SyncHeaderCheckBox()
        End If
    End Sub

    ' optional: click on row (other cells) toggle checkbox
    Private Sub dg1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dg1.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex <> dg1.Columns("Select").Index Then
            Dim r As DataGridViewRow = dg1.Rows(e.RowIndex)
            Dim v As Boolean = False
            If r.Cells("Select").Value IsNot Nothing Then Boolean.TryParse(r.Cells("Select").Value.ToString(), v)
            r.Cells("Select").Value = Not v
            dg1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            CalculateTotals()
            SyncHeaderCheckBox()
        End If
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
        ButtonControl() : retrive() : ButtonControl()
        pnlWait.Visible = False : If dg1.Rows.Count <> 0 Then btnWhatsapp.Visible = True
    End Sub

    Private Sub PrintRecord()
        pnlWait.Visible = True
        Try
            ' collect rows to print based on Select checkbox
            Dim rowsToPrint As New List(Of DataGridViewRow)
            For Each r As DataGridViewRow In dg1.Rows
                If Not r.IsNewRow Then
                    Dim sel As Boolean = False
                    If r.Cells("Select").Value IsNot Nothing Then
                        Boolean.TryParse(r.Cells("Select").Value.ToString(), sel)
                    End If
                    If sel Then
                        rowsToPrint.Add(r)
                    End If
                End If
            Next

            ' if none selected -> use all rows
            If rowsToPrint.Count = 0 Then
                For Each r As DataGridViewRow In dg1.Rows
                    If Not r.IsNewRow Then rowsToPrint.Add(r)
                Next
            End If

            If rowsToPrint.Count = 0 Then
                pnlWait.Visible = False
                Exit Sub
            End If

            ' compute totals for the set we'll print (use .Tag numeric value if present)
            Dim sumDebit As Double = 0.0
            Dim sumCredit As Double = 0.0
            For Each r As DataGridViewRow In rowsToPrint
                Dim v As Double = 0
                If r.Tag IsNot Nothing Then Double.TryParse(r.Tag.ToString(), v)
                If v > 0 Then sumDebit += v Else sumCredit += Math.Abs(v)
            Next
            Dim grand As Double = sumDebit - sumCredit

            ' prepare printing table (clean)
            Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
            ClsFunPrimary.ExecNonQuery("Delete from printing")

            ' batching: 100 rows per insert (same as original)
            Dim totalRows As Integer = rowsToPrint.Count
            Dim batchCountDecimal As Decimal = Math.Ceiling(totalRows / 100D)
            Dim lastIndex As Integer = 0

            For batchIndex As Integer = 0 To batchCountDecimal - 1
                Application.DoEvents()
                Dim FastQuery As String = String.Empty

                Dim startIdx As Integer = batchIndex * 100
                Dim endIdx As Integer = Math.Min(startIdx + 99, totalRows - 1)

                For idx As Integer = startIdx To endIdx
                    Dim rw As DataGridViewRow = rowsToPrint(idx)

                    ' get cell values safely (use indices to avoid name mismatch)
                    Dim accName As String = If(rw.Cells(2).Value IsNot Nothing, rw.Cells(2).Value.ToString(), "")
                    Dim area As String = If(rw.Cells(3).Value IsNot Nothing, rw.Cells(3).Value.ToString(), "")
                    Dim mobile As String = If(rw.Cells(4).Value IsNot Nothing, rw.Cells(4).Value.ToString(), "")
                    Dim opBal As String = If(rw.Cells(5).Value IsNot Nothing, rw.Cells(5).Value.ToString(), "")
                    Dim balance As String = If(rw.Cells(6).Value IsNot Nothing, rw.Cells(6).Value.ToString(), "")
                    Dim otherName As String = If(rw.Cells(7).Value IsNot Nothing, rw.Cells(7).Value.ToString(), "")

                    ' escape single quotes for SQL safety
                    accName = accName.Replace("'", "''")
                    area = area.Replace("'", "''")
                    mobile = mobile.Replace("'", "''")
                    opBal = opBal.Replace("'", "''")
                    balance = balance.Replace("'", "''")
                    otherName = otherName.Replace("'", "''")
                    Dim markaEsc As String = If(marka IsNot Nothing, marka.Replace("'", "''"), "")

                    ' Use computed totals (sumDebit, sumCredit, grand) for every inserted row (same as original behaviour)
                    Dim debitStr As String = Format(sumDebit, "0.00")
                    Dim creditStr As String = Format(sumCredit, "0.00")
                    Dim grandStr As String = Format(Math.Abs(grand), "0.00")

                    ' Build SELECT fragment
                    FastQuery &= If(FastQuery <> "", " UNION ALL SELECT ", " SELECT ")
                    FastQuery &= "'" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'," & _
                                 "'" & accName & "','" & area & "','" & mobile & "'," & _
                                 "'" & balance & "','" & otherName & "','" & opBal & "'," & _
                                 "'" & debitStr & "','" & creditStr & "'," & _
                                 "'" & grandStr & "','" & markaEsc & "'"
                Next

                ' execute batch insert
                If FastQuery <> "" Then
                    Dim sQL As String = "insert into Printing(D1,P1, P2,P3, P4,P5,P6,P7,P8,P9,M10) " & FastQuery
                    Try
                        ClsFunPrimary.ExecNonQuery(sQL)
                    Catch ex As Exception
                        MsgBox("PrintRecord Error: " & ex.Message)
                        ClsFunPrimary.CloseConnection()
                    End Try
                End If
            Next

        Finally
            pnlWait.Visible = False
        End Try
    End Sub

    'Private Sub PrintRecord()
    '    Dim AllRecord As Integer = Val(dg1.Rows.Count)
    '    Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
    '    Dim FastQuery As String = String.Empty
    '    Dim sQL As String = String.Empty
    '    Dim LastCount As Integer = 0
    '    pnlWait.Visible = True
    '    Dim TotalRecord As Integer = 0
    '    Dim LastRecord As Integer = 0
    '    Dim marka As String = clsFun.ExecScalarStr("Select Marka From Company")
    '    ClsFunPrimary.ExecNonQuery("Delete from printing")
    '    For i As Integer = 0 To maxRowCount - 1
    '        Application.DoEvents()
    '        FastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
    '        For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
    '            With dg1.Rows(LastRecord)
    '                FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskEntryDate.Text & "'," & _
    '                  "'" & .Cells("Account Name").Value & "','" & .Cells("Area").Value & "','" & .Cells("Mobile No.").Value & "', " & _
    '                  "'" & .Cells("Balance").Value & "','" & .Cells("OtherName").Value & "','" & .Cells("Op bal").Value & "', " & _
    '                  "'" & Format(Val(txtDebitBal.Text), "0.00") & "','" & Format(Val(txtCreditBal.Text), "0.00") & "', " & _
    '                  "'" & Format(Val(TxtGrandTotal.Text), "0.00") & "','" & marka & "'"
    '            End With
    '            LastRecord = Val(LastRecord + 1)
    '        Next
    '        ' LastRecord = LastCount
    '        Try
    '            If FastQuery = String.Empty Then Exit Sub
    '            sQL = "insert into Printing(D1,P1, P2,P3, P4,P5,P6,P7,P8,P9,M10) " & FastQuery & ""
    '            ClsFunPrimary.ExecNonQuery(sQL)
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '            ClsFunPrimary.CloseConnection()
    '        End Try

    '    Next

    '    'LastRecord = LastCount
    '    'For Each row As DataGridViewRow In dg1.Rows
    '    '    Application.DoEvents()
    '    '    If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = False Then Exit Sub
    '    '    pb1.Minimum = 0
    '    '    pb1.Maximum = dg1.Rows.Count
    '    '    With row
    '    '        pb1.Value = IIf(Val(row.Index) < 0, 0, Val(row.Index))
    '    '        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskEntryDate.Text & "'," & _
    '    '            "'" & .Cells("Account Name").Value & "','" & .Cells("Area").Value & "','" & .Cells("Mobile No.").Value & "', " & _
    '    '            "'" & .Cells("Balance").Value & "','" & .Cells("OtherName").Value & "','" & .Cells("Op bal").Value & "', " & _
    '    '            "'" & Format(Val(txtDebitBal.Text), "0.00") & "','" & Format(Val(txtCreditBal.Text), "0.00") & "', " & _
    '    '            "'" & Format(Val(TxtGrandTotal.Text), "0.00") & "','" & marka & "'"
    '    '    End With
    '    'Next

    '    'For Each row As DataGridViewRow In dg1.Rows
    '    '    Application.DoEvents()
    '    '    If Application.OpenForms().OfType(Of OutStanding_Amount_Only).Any = False Then Exit Sub
    '    '    pb1.Minimum = 0
    '    '    pb1.Maximum = dg1.Rows.Count
    '    '    With row
    '    '        pb1.Value = IIf(Val(row.Index) < 0, 0, Val(row.Index))
    '    '        FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", " SELECT ") & "'" & mskEntryDate.Text & "'," & _
    '    '            "'" & .Cells("Account Name").Value & "','" & .Cells("Area").Value & "','" & .Cells("Mobile No.").Value & "', " & _
    '    '            "'" & .Cells("Balance").Value & "','" & .Cells("OtherName").Value & "','" & .Cells("Op bal").Value & "', " & _
    '    '            "'" & Format(Val(txtDebitBal.Text), "0.00") & "','" & Format(Val(txtCreditBal.Text), "0.00") & "', " & _
    '    '            "'" & Format(Val(TxtGrandTotal.Text), "0.00") & "','" & marka & "'"
    '    '    End With
    '    'Next
    '    'Next

    '    pnlWait.Visible = False
    'End Sub

    Private Sub BtnPrint_Click(sender As Object, e As EventArgs) Handles BtnPrint.Click

        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            ButtonControl()
            PrintRecord()
            If Application.OpenForms().OfType(Of Selected_Outsanting).Any = False Then Exit Sub
            If ckAreaWise.Checked = True Then Report_Viewer.printReport("\OutstandingAreaWise.rpt") Else Report_Viewer.printReport("\Outstanding.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
        ButtonControl()
    End Sub

    Private Sub btnPrintHindi_Click(sender As Object, e As EventArgs) Handles btnPrintHindi.Click
        'clsFun.changeCompany()

        'Print_Outstanding.MdiParent = MainScreenForm
        'Print_Outstanding.Show()
        'If Not Print_Outstanding Is Nothing Then
        '    Print_Outstanding.BringToFront()
        'End If

        '''''''
        If dg1.RowCount = 0 Then
            MsgBox("There is No record to Print...", vbOKOnly, "Empty")
            Exit Sub
        Else
            ButtonControl()
            PrintRecord()
            If Application.OpenForms().OfType(Of Selected_Outsanting).Any = False Then Exit Sub
            If ckAreaWise.Checked = True Then Report_Viewer.printReport("\OutstandingAreaWise2.rpt") Else Report_Viewer.printReport("\OutstandingHindi.rpt")
            '  Report_Viewer.printReport("\OutstandingHindi.rpt")
            ' Report_Viewer.ExportReport("\OutstandingHindi.rpt")
            Report_Viewer.MdiParent = MainScreenForm
            Report_Viewer.Show()
            If Not Report_Viewer Is Nothing Then
                Report_Viewer.BringToFront()
            End If
        End If
        ButtonControl()
    End Sub

    Private Sub ckHideze_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub ckDebtors_CheckedChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub lblRecordCount_Click(sender As Object, e As EventArgs) Handles lblRecordCount.Click

    End Sub


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If isBackgroundWorkerRunning Then
            Me.Hide()
            Me.Left = 0 : Me.Top = 0
        Else
            Me.Dispose()
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

    Private Sub DgWhatsapp_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DgWhatsapp.CellEndEdit
        If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(9).Value & "'") = "" And DgWhatsapp.CurrentRow.Cells(3).Value <> "" Then
            clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(9).Value) & "'")
        Else
            If clsFun.ExecScalarStr("Select Mobile1 From Accounts Where ID='" & DgWhatsapp.CurrentRow.Cells(9).Value & "'") <> DgWhatsapp.CurrentRow.Cells(3).Value Then
                If MessageBox.Show("Are you Sure to Change Mobile No in PhoneBook", "Change Number", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    clsFun.ExecScalarStr("Update Accounts set Mobile1='" & DgWhatsapp.CurrentRow.Cells(3).Value & "' Where ID='" & Val(DgWhatsapp.CurrentRow.Cells(9).Value) & "'")
                End If
            End If
        End If
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
        Dim sql As String = String.Empty
        Dim i As Integer
        Dim count As Integer = 0
        If ckAreaWise.Checked = True Then
            If RadioSundryDebtors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
             "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(16,32) " & condtion & " Order by Upper(Area),upper(AccountName)  ;"
            ElseIf RadioSundryCreditors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
             "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(17,33)  " & condtion & " Order by Upper(Area),upper(AccountName) ;"
            ElseIf RadioAll.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
             "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 " & condtion & " Order by Upper(Area),upper(AccountName) ;"
            End If
        Else
            If RadioSundryDebtors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
             "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(16,32) " & condtion & " Order by upper(AccountName) ;"
            ElseIf RadioSundryCreditors.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
             "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 and GroupID in(17,33)  " & condtion & " Order by upper(AccountName) ;"
            ElseIf RadioAll.Checked = True Then
                sql = "Select ID,Accountname,Area,Opbal,DC,OtherName,Mobile1, " & _
             "Round((Case When DC='Dr' then (ifnull(opbal,0)+(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             "-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')) " & _
             " else (ifnull(-(opbal),0)+-(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='C' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "')" & _
             " +(Select ifnull(Round(Sum(Amount),2),0) From Ledger Where AccountID=Accounts.ID and DC='D' and Ledger.Entrydate <='" & CDate(mskEntryDate.Text).ToString("yyyy-MM-dd") & "'))  end),2) as  Restbal from Accounts Where RestBal<>0 " & condtion & " Order by upper(AccountName) ;"
            End If

        End If
        dt = clsFun.ExecDataTable(sql)
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                DgWhatsapp.Rows.Add()
                With DgWhatsapp.Rows(i)
                    Dim Msg As String = IIf(Val(dt.Rows(i)("Restbal").ToString()) > 0, Format(Val(dt.Rows(i)("Restbal").ToString()), "0.00") & " " & "Dr", Format(Math.Abs(Val(dt.Rows(i)("Restbal").ToString())), "0.00") & " " & "Cr")
                    .Cells(1).Value = dt.Rows(i)("ID").ToString()
                    .Cells(2).Value = dt.Rows(i)("AccountName").ToString()
                    .Cells(3).Value = dt.Rows(i)("Mobile1").ToString()
                    .Cells(4).Value = "Dear " & dt.Rows(i)("AccountName").ToString() & "," & vbNewLine & " Your Due Balance is:" & Msg & "." & vbNewLine & " (_Please Deposit as Soon As Possible_) "
                    .Cells(5).Value = dt.Rows(i)("OtherName").ToString()
                    .Cells(6).Value = "प्रिय " & dt.Rows(i)("OtherName").ToString() & "," & vbNewLine & " आपकी कुल बकाया राशि :" & Msg & " हैं|" & vbNewLine & " (_कृपया समय पर जमा करवाए_|) "
                    .Cells(9).Value = dt.Rows(i)("ID").ToString()
                    .Cells(1).ReadOnly = True : .Cells(2).ReadOnly = True
                    .Cells(0).Value = True
                End With
            Next i
        End If
        DgWhatsapp.ClearSelection()
    End Sub

    Sub RowColumsWhatsapp()
        DgWhatsapp.Columns.Clear() : DgWhatsapp.ColumnCount = 9
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
        DgWhatsapp.Columns(1).Name = "ID" : DgWhatsapp.Columns(1).Visible = False
        DgWhatsapp.Columns(2).Name = "Account Name" : DgWhatsapp.Columns(2).Width = 150
        DgWhatsapp.Columns(3).Name = "Mobile No" : DgWhatsapp.Columns(3).Width = 100
        DgWhatsapp.Columns(4).Name = "message" : DgWhatsapp.Columns(4).Width = 350
        DgWhatsapp.Columns(5).Name = "Other Name" : DgWhatsapp.Columns(5).Visible = False
        DgWhatsapp.Columns(6).Name = "message2" : DgWhatsapp.Columns(6).Visible = False
        DgWhatsapp.Columns(7).Name = "Status" : DgWhatsapp.Columns(7).Width = 100
        DgWhatsapp.Columns(8).Name = "Path" : DgWhatsapp.Columns(8).Visible = False
        DgWhatsapp.Columns(9).Name = "AccountID" : DgWhatsapp.Columns(9).Visible = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnWhatsapp.Click
        FillControl() : pnlWhatsapp.BringToFront()
        If ClsFunPrimary.ExecScalarStr("Select SendingMethod From API") = "Easy WhatsApp" Then
            cbType.Visible = True : cbType.SelectedIndex = 0
            RowColumsWhatsapp() : ShowWhatsappContacts()
            pnlWhatsapp.Visible = True
            Exit Sub
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
        pnlWhatsapp.Visible = True
        If DgWhatsapp.ColumnCount = 0 Then RowColumsWhatsapp()
        ShowWhatsappContacts()
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
            'StartBackgroundTask(AddressOf SendWhatsappData)
        ElseIf cbType.Text = "WhatsApp Official API" Then
            SendOfficialOutstandingData()
        ElseIf cbType.Text = MobileSendingMethod Then
            SendMobileOutstandingData()
        End If
    End Sub
    Private Sub StartBackgroundTask(action As Action)
        If Not bgWorker.IsBusy Then
            bgWorker.RunWorkerAsync(action)
            '    MsgBox("A background task is running. you can Use your Task", MsgBoxStyle.Information, "Background Task")
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

    Private Sub UpdateProgressBar(value As Integer)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Integer)(AddressOf UpdateProgressBar), value)
        Else
            If value <= ProgressBar1.Maximum Then
                ProgressBar1.Value = value
            End If
        End If
    End Sub

    Private Sub UpdateProgressBarVisibility(visible As Boolean)
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(New Action(Of Boolean)(AddressOf UpdateProgressBarVisibility), visible)
        Else
            ProgressBar1.Visible = visible
        End If
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
                    If SendingMethod = "WhatsApp Official API" AndAlso cbType.Items.Contains("WhatsApp Official API") Then
                        cbType.SelectedItem = "WhatsApp Official API"
                    ElseIf SendingMethod = MobileSendingMethod AndAlso cbType.Items.Contains(MobileSendingMethod) Then
                        cbType.SelectedItem = MobileSendingMethod
                        RefreshMobileSimList()
                    Else
                        cbType.SelectedIndex = 0
                    End If
                    cbType.Visible = True
                    ToggleMobileControls()
                    LangugageType = dt.Rows(i)("LanguageType").ToString()
                    btnRadioEnglish.Checked = True
                    If LangugageType = "English" Then btnRadioEnglish.Checked = True Else RadioRegional.Checked = True
                Next
            End If
            dt.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly + vbInformation, "Aadhat")
        End Try
        'clsFun.CloseConnection()
    End Sub
    Private Sub SendWhatsappData()
        'ProgressBar1.Visible = True
        'ProgressBar1.Minimum = 0
        'ProgressBar1.Maximum = DgWhatsapp.RowCount
        UpdateProgressBarVisibility(True)
        Dim count As Integer = 0
        Dim cmd As New SQLite.SQLiteCommand
        Dim fastQuery As String = String.Empty
        Dim Sql As String = String.Empty
        Dim con As SQLite.SQLiteConnection = clsFun.GetConnection
        ClsFunWhatsapp.ExecNonQuery("Delete from SendingData")
        Dim filteredRows As List(Of DataGridViewRow) = DgWhatsapp.Rows.Cast(Of DataGridViewRow)().
         Where(Function(row) row.Cells(0).Value = True AndAlso Not String.IsNullOrEmpty(row.Cells(3).Value.ToString())).ToList()
        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(Sub() ProgressBar1.Maximum = filteredRows.Count)
        Else
            ProgressBar1.Maximum = filteredRows.Count
        End If
        Dim AllRecord As Integer = Val(filteredRows.Count)
        Dim maxRowCount As Decimal = Math.Ceiling(AllRecord / 100)
        Dim LastCount As Integer = 0
        Dim TotalRecord As Integer = 0
        Dim LastRecord As Integer = 0
        For i As Integer = 0 To maxRowCount - 1
            fastQuery = String.Empty : TotalRecord = (AllRecord - LastRecord)
            For LastCount = 0 To IIf(i = (maxRowCount - 1), Val(TotalRecord - 1), 99)
                With filteredRows(LastRecord)
                    UpdateProgressBar(count)
                    If .Cells(0).Value = True Then
                        If btnRadioEnglish.Checked = True And .Cells(3).Value <> "" Then
                            fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(2).Value & "','" & .Cells(3).Value & "', " &
                             "'" & .Cells(4).Value & vbNewLine & txtMsg.Text.Trim & vbNewLine & "*" & compname & "*', ''"
                        ElseIf RadioRegional.Checked = True And .Cells(3).Value <> "" Then
                            If .Cells(3).Value <> "" Then
                                fastQuery = fastQuery & IIf(fastQuery <> "", " UNION ALL SELECT ", " SELECT ") & Val(.Cells(1).Value) & ",'" & .Cells(5).Value & "','" & .Cells(3).Value & "', " &
                                           "'" & .Cells(6).Value & vbNewLine & txtMsg.Text.Trim & vbNewLine & "*" & compnameHindi & "*', ''"
                            End If
                        End If
                    End If
                End With
                LastRecord = Val(LastRecord + 1)
            Next
            Try
                Sql = "insert into SendingData(AccountID,AccountName,MobileNos,Message1,AttachedFilepath) " & fastQuery & ";"
                ClsFunWhatsapp.ExecNonQuery(Sql)
            Catch ex As Exception
                MsgBox(ex.Message)
                UpdateProgressBarVisibility(False)
                ClsFunWhatsapp.CloseConnection()
            End Try
        Next
        ClsFunWhatsapp.ExecNonQuery("Update Settings Set MinState='N'")
        MsgBox("Data Send to Easy Whatsapp Successfully...", vbInformation, "Sended On Easy Whatsapp")
        UpdateProgressBarVisibility(False)
    End Sub



    Private Function ExtractBalanceAmount(ByVal messageText As String) As String
        If messageText Is Nothing Then Return ""
        Dim match As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(messageText, "([0-9]+(?:\.[0-9]+)?\s*(?:Dr|Cr))", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        If match.Success Then Return match.Groups(1).Value.Trim()
        Return ""
    End Function

    Private Sub SendOfficialOutstandingData()
        UpdateProgressBarVisibility(True)
        Dim filteredRows As List(Of DataGridViewRow) = DgWhatsapp.Rows.Cast(Of DataGridViewRow)().
            Where(Function(row) row.Cells(0).Value = True AndAlso row.Cells(3).Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(row.Cells(3).Value.ToString())).ToList()

        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(Sub() ProgressBar1.Maximum = Math.Max(filteredRows.Count, 1))
        Else
            ProgressBar1.Maximum = Math.Max(filteredRows.Count, 1)
        End If

        Dim count As Integer = 0
        For Each row As DataGridViewRow In filteredRows
            UpdateProgressBar(count)
            Dim mobile As String = WhatsAppOfficialApi.NormalizeMobile(row.Cells(3).Value.ToString())
            If mobile = "" OrElse mobile.Length < 12 Then
                row.Cells(7).Value = "Invalid mobile no"
                row.Cells(7).Style.ForeColor = System.Drawing.Color.Red
                count += 1
                Continue For
            End If

            Dim preferredLanguage As String = If(btnRadioEnglish.Checked, "en", "hi")
            Dim accountName As String = If(btnRadioEnglish.Checked, row.Cells(2).Value.ToString(), row.Cells(5).Value.ToString())
            If accountName.Trim() = "" Then accountName = row.Cells(2).Value.ToString()
            Dim messageText As String = If(btnRadioEnglish.Checked, row.Cells(4).Value.ToString(), row.Cells(6).Value.ToString())
            Dim apiResponse As String = ""
            Dim ok As Boolean = WhatsAppOfficialSendHelper.SendAadhatText("balance", "OUTSTANDING", mobile, accountName, mskEntryDate.Text, ExtractBalanceAmount(messageText), messageText, apiResponse, preferredLanguage)

            row.Cells(7).Value = If(ok, "Sent via Official API", "Failed")
            row.Cells(7).ToolTipText = apiResponse
            row.Cells(7).Style.ForeColor = If(ok, System.Drawing.Color.Green, System.Drawing.Color.Red)
            count += 1
        Next

        UpdateProgressBarVisibility(False)
    End Sub

    Private Sub ToggleMobileControls()
        Dim showMobile As Boolean = (cbType.Text = MobileSendingMethod)
        lblSim.Visible = showMobile
        cbSim.Visible = showMobile
        Label10.Visible = Not showMobile
    End Sub

    Private Function RefreshMobileSimList() As Boolean
        cbSim.Items.Clear()
        Dim simItems As List(Of String) = PhoneMSg.GetSimDisplayList("", False)
        If simItems.Count = 0 Then
            MsgBox("No verified mobile SIM found. Please verify PhoneMSg access token in WhatsApp API Configuration.", MsgBoxStyle.Exclamation, "Mobile API")
            Return False
        End If

        For Each item As String In simItems
            cbSim.Items.Add(item)
        Next

        Dim savedSim As String = ClsFunPrimary.ExecScalarStr("Select DefaultSim From API")
        Dim savedIndex As Integer = PhoneMSg.FindSimIndexBySubscriberId(savedSim, cbSim.Items)
        If savedIndex >= 0 Then
            cbSim.SelectedIndex = savedIndex
        ElseIf cbSim.Items.Count > 0 Then
            cbSim.SelectedIndex = 0
        End If
        Return cbSim.SelectedIndex >= 0
    End Function

    Private Sub SendMobileOutstandingData()
        If RefreshMobileSimList() = False Then Exit Sub
        UpdateProgressBarVisibility(True)
        Dim filteredRows As List(Of DataGridViewRow) = DgWhatsapp.Rows.Cast(Of DataGridViewRow)().
            Where(Function(row) row.Cells(0).Value = True AndAlso row.Cells(3).Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(row.Cells(3).Value.ToString())).ToList()

        If ProgressBar1.InvokeRequired Then
            ProgressBar1.Invoke(Sub() ProgressBar1.Maximum = Math.Max(filteredRows.Count, 1))
        Else
            ProgressBar1.Maximum = Math.Max(filteredRows.Count, 1)
        End If

        Dim count As Integer = 0
        For Each row As DataGridViewRow In filteredRows
            UpdateProgressBar(count)
            Dim mobile As String = WhatsAppOfficialApi.NormalizeMobile(row.Cells(3).Value.ToString())
            Dim messageText As String = If(btnRadioEnglish.Checked, row.Cells(4).Value.ToString(), row.Cells(6).Value.ToString())
            If txtMsg.Text.Trim() <> "" Then messageText &= vbNewLine & txtMsg.Text.Trim()
            Dim apiResult As String = PhoneMSg.SendPhoneMsg(mobile, cbSim.Text, messageText)
            row.Cells(7).Value = apiResult
            row.Cells(7).ToolTipText = apiResult
            row.Cells(7).Style.ForeColor = If(apiResult.ToLower().Contains("success"), System.Drawing.Color.Green, System.Drawing.Color.Red)
            count += 1
        Next

        UpdateProgressBarVisibility(False)
    End Sub

    Private Sub cbType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbType.SelectedIndexChanged
        ToggleMobileControls()
        If cbType.Text = MobileSendingMethod AndAlso pnlWhatsapp.Visible Then RefreshMobileSimList()
    End Sub

    Private Sub btnPnlVisHide_Click(sender As Object, e As EventArgs) Handles btnPnlVisHide.Click
        pnlWhatsapp.Visible = False
    End Sub

    Private Sub txtCustomerSearch_TextChanged(sender As Object, e As EventArgs) Handles txtCustomerSearch.TextChanged

    End Sub

    Private Sub ckAreaWise_CheckedChanged(sender As Object, e As EventArgs) Handles ckAreaWise.CheckedChanged

    End Sub
End Class
