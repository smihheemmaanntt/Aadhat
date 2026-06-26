Imports System.Data
Imports System.Data.SQLite

Public Class SpeedSaleDateCorrection
    Private Sub SpeedSaleDateCorrection_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        txtEntryDate.Text = Date.Today.ToString("dd-MM-yyyy")
        txtCorrectDate.Text = Date.Today.ToString("dd-MM-yyyy")
        PrepareGrid()
        ResetCorrectionControls()
        UpdateSummary()
    End Sub

    Private Sub ResetCorrectionControls()
        txtSure.Text = ""
        lblSure.Visible = False
        txtSure.Visible = False
        lblCorrectDate.Visible = False
        txtCorrectDate.Visible = False
        btnCorrectDate.Visible = False
    End Sub

    Private Sub PrepareGrid()
        dgEntries.Columns.Clear()
        dgEntries.AutoGenerateColumns = False
        dgEntries.EnableHeadersVisualStyles = False
        dgEntries.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
        dgEntries.ColumnHeadersDefaultCellStyle.ForeColor = Color.Navy
        dgEntries.GridColor = Color.Silver

        Dim colSelect As New DataGridViewCheckBoxColumn()
        colSelect.Name = "colSelect"
        colSelect.HeaderText = "Tick"
        colSelect.Width = 42
        dgEntries.Columns.Add(colSelect)

        AddTextColumn("colID", "ID", 60)
        AddTextColumn("colBillNo", "Bill No", 70)
        AddTextColumn("colEntryDate", "Entry Date", 95)
        AddTextColumn("colEntryTime", "Entry Time", 145)
        AddTextColumn("colAccountName", "Account Name", 260)
        AddTextColumn("colItemName", "Item Name", 180)
        AddTextColumn("colNug", "Nug", 70, DataGridViewContentAlignment.MiddleRight)
        AddTextColumn("colWeight", "Weight", 80, DataGridViewContentAlignment.MiddleRight)
        AddTextColumn("colAmount", "Amount", 90, DataGridViewContentAlignment.MiddleRight)
    End Sub

    Private Sub AddTextColumn(ByVal name As String, ByVal headerText As String, ByVal width As Integer, Optional ByVal alignment As DataGridViewContentAlignment = DataGridViewContentAlignment.MiddleLeft)
        Dim col As New DataGridViewTextBoxColumn()
        col.Name = name
        col.HeaderText = headerText
        col.Width = width
        col.ReadOnly = True
        col.DefaultCellStyle.Alignment = alignment
        dgEntries.Columns.Add(col)
    End Sub

    Private Sub txtEntryDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtEntryDate.Validating
        txtEntryDate.Text = SmartDate(txtEntryDate.Text)
    End Sub

    Private Sub txtCorrectDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtCorrectDate.Validating
        txtCorrectDate.Text = SmartDate(txtCorrectDate.Text)
    End Sub

    Private Sub txtDate_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtEntryDate.GotFocus, txtCorrectDate.GotFocus
        CType(sender, TextBox).SelectAll()
    End Sub

    Private Sub btnShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShow.Click
        LoadEntries()
    End Sub

    Private Sub LoadEntries()
        dgEntries.Rows.Clear()
        chkSelectAll.Checked = False
        ResetCorrectionControls()

        If IsDate(txtEntryDate.Text) = False Then
            MsgBox("Please enter a valid EntryTime Date.", MsgBoxStyle.Critical, "Speed Sale Date Correction")
            txtEntryDate.Focus()
            Exit Sub
        End If

        Dim sqliteDate As String = CDate(txtEntryDate.Text).ToString("yyyy-MM-dd")
        Dim sql As String = "Select v.ID, v.BillNo, v.EntryDate, IfNull(v.EntryTime,'') As EntryTime, " &
                            "IfNull(t.AccountName,'') As AccountName, IfNull(t.ItemName,'') As ItemName, " &
                            "IfNull(t.Nug,0) As Nug, IfNull(t.Weight,0) As Weight, IfNull(t.TotalAmount,0) As TotalAmount " &
                            "From Vouchers v " &
                            "Left Join Transaction2 t On t.VoucherID=v.ID And t.TransType='Speed Sale' " &
                            "Where v.TransType='Speed Sale' " &
                            "And IfNull(substr(v.EntryTime,1,10),'')='" & sqliteDate & "' " &
                            "And IfNull(v.EntryDate,'')<>'" & sqliteDate & "' " &
                            "Order By IfNull(v.EntryTime,''), v.ID"

        Dim dt As DataTable = clsFun.ExecDataTable(sql)
        For Each row As DataRow In dt.Rows
            dgEntries.Rows.Add(False,
                               Val(row("ID").ToString()),
                               Val(row("BillNo").ToString()),
                               Format(CDate(row("EntryDate")), "dd-MM-yyyy"),
                               row("EntryTime").ToString(),
                               row("AccountName").ToString(),
                               row("ItemName").ToString(),
                               Format(Val(row("Nug").ToString()), "0.00"),
                               Format(Val(row("Weight").ToString()), "0.00"),
                               Format(Val(row("TotalAmount").ToString()), "0.00"))
        Next
        dt.Dispose()
        If dgEntries.Rows.Count > 0 Then
            lblSure.Visible = True
            txtSure.Visible = True
            txtCorrectDate.Text = txtEntryDate.Text
        End If
        UpdateSummary()
    End Sub

    Private Function SelectedIds() As List(Of Integer)
        Dim ids As New List(Of Integer)()
        For Each row As DataGridViewRow In dgEntries.Rows
            If row.IsNewRow Then Continue For
            Dim isChecked As Boolean = False
            If row.Cells("colSelect").Value IsNot Nothing Then Boolean.TryParse(row.Cells("colSelect").Value.ToString(), isChecked)
            If isChecked Then ids.Add(Val(row.Cells("colID").Value))
        Next
        Return ids
    End Function

    Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSelectAll.CheckedChanged
        For Each row As DataGridViewRow In dgEntries.Rows
            If row.IsNewRow Then Continue For
            row.Cells("colSelect").Value = chkSelectAll.Checked
        Next
        UpdateSummary()
    End Sub

    Private Sub dgEntries_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgEntries.CurrentCellDirtyStateChanged
        If dgEntries.IsCurrentCellDirty Then dgEntries.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub

    Private Sub dgEntries_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgEntries.CellValueChanged
        If e.RowIndex >= 0 Then UpdateSummary()
    End Sub

    Private Sub UpdateSummary()
        Dim totalRows As Integer = 0
        For Each row As DataGridViewRow In dgEntries.Rows
            If row.IsNewRow = False Then totalRows += 1
        Next
        lblSummary.Text = "Mismatch Entries: " & totalRows.ToString() & "    Selected: " & SelectedIds().Count.ToString()
    End Sub

    Private Sub txtSure_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSure.TextChanged
        Dim isSure As Boolean = (txtSure.Text.Trim().ToUpper() = "SURE")
        lblCorrectDate.Visible = isSure
        txtCorrectDate.Visible = isSure
        btnCorrectDate.Visible = isSure
        If isSure AndAlso txtCorrectDate.Text.Trim() = "" Then txtCorrectDate.Text = txtEntryDate.Text
    End Sub

    Private Sub btnCorrectDate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCorrectDate.Click
        If IsDate(txtEntryDate.Text) = False Then
            MsgBox("Please enter a valid EntryTime Date.", MsgBoxStyle.Critical, "Speed Sale Date Correction")
            txtEntryDate.Focus()
            Exit Sub
        End If
        If IsDate(txtCorrectDate.Text) = False Then
            MsgBox("Please enter a valid correct Entry Date.", MsgBoxStyle.Critical, "Speed Sale Date Correction")
            txtCorrectDate.Focus()
            Exit Sub
        End If
        If txtSure.Text.Trim().ToUpper() <> "SURE" Then
            MsgBox("Please type SURE before correction.", MsgBoxStyle.Critical, "Speed Sale Date Correction")
            txtSure.Focus()
            Exit Sub
        End If

        Dim ids As List(Of Integer) = SelectedIds()
        If ids.Count = 0 Then
            MsgBox("Please tick at least one Speed Sale entry.", MsgBoxStyle.Critical, "Speed Sale Date Correction")
            Exit Sub
        End If

        Dim newDate As String = CDate(txtCorrectDate.Text).ToString("yyyy-MM-dd")
        Dim idList As String = String.Join(",", ids.ConvertAll(Function(x) x.ToString()).ToArray())
        Dim modifyById As Integer = clsFun.ExecScalarInt("Select ID From Users Where UserName='" & MainScreenPicture.lblUser.Text & "'")
        Dim modifiedTime As String = Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim sql As String = "Begin Transaction; " &
                            "Update Vouchers Set EntryDate='" & newDate & "', ModifiedByID=" & modifyById & ", ModifiedTime='" & modifiedTime & "' " &
                            "Where TransType='Speed Sale' And ID In(" & idList & "); " &
                            "Update Transaction2 Set EntryDate='" & newDate & "' " &
                            "Where TransType='Speed Sale' And VoucherID In(" & idList & "); " &
                            "Update Ledger Set EntryDate='" & newDate & "' " &
                            "Where TransType='Speed Sale' And VourchersID In(" & idList & "); " &
                            "Update CrateVoucher Set EntryDate='" & newDate & "' " &
                            "Where VoucherID In(" & idList & ") And IfNull(TransType,'')<>'Op Bal'; " &
                            "Commit;"

        Try
            clsFun.ExecNonQuery(sql)
            MsgBox(ids.Count.ToString() & " Speed Sale entry date(s) corrected successfully.", MsgBoxStyle.Information, "Speed Sale Date Correction")
            ResetCorrectionControls()
            LoadEntries()
        Catch ex As Exception
            Try
                clsFun.ExecNonQuery("Rollback;")
            Catch
            End Try
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Speed Sale Date Correction")
        End Try
    End Sub

    Private Sub SpeedSaleDateCorrection_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
        If e.KeyCode = Keys.Enter AndAlso TypeOf Me.ActiveControl Is TextBox Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class
