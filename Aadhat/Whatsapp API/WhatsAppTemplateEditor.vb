Imports System
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports Newtonsoft.Json.Linq

Public Class WhatsAppTemplateEditor
    Public VendorUid As String = ""
    Public AccessToken As String = ""
    Private SelectedMediaPath As String = ""
    Private UploadedMediaFileName As String = ""
    Private SampleValuePanel As FlowLayoutPanel
    Private SampleTextBoxes As New Dictionary(Of Integer, TextBox)
    Private LastTemplateTextBox As TextBox
    Private LastTemplateSelectionStart As Integer = 0
    Private LoadingTemplateSelection As Boolean = False
    Private TemplateParameterFields As New List(Of String)()
    Private SuppressSelectAllEvent As Boolean = False
    Private BulkSelectingTemplates As Boolean = False

    Private Sub WhatsAppTemplateEditor_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        If Me.DesignMode OrElse LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Exit Sub
        WhatsAppOfficialDb.EnsureDatabase()
        FillLocalTypeCombo()
        If cbLanguage.Items.Count > 0 Then cbLanguage.SelectedIndex = 0
        If cbTemplateFormat.Items.Count > 0 Then cbTemplateFormat.SelectedIndex = 0
        If cbCategory.Items.Count > 0 Then cbCategory.SelectedIndex = 0
        If cbHeaderType.Items.Count > 0 Then cbHeaderType.SelectedIndex = 0
        ApplyButtonStyles()
        EnsureSampleValuePanel()
        LoadParameterFieldCombo()
        LoadTemplates()
        BuildSampleInputs()
    End Sub

    Private Sub FillLocalTypeCombo()
        txtTemplateType.Items.Clear()
        txtTemplateType.Items.Add("Print Bill")
        txtTemplateType.Items.Add("Receipt")
        txtTemplateType.Items.Add("Payment")
        txtTemplateType.Items.Add("Balance")
        txtTemplateType.Items.Add("Statement")
        txtTemplateType.Items.Add("Crate In")
        txtTemplateType.Items.Add("Crate Out")
        txtTemplateType.Items.Add("Ledger")
        txtTemplateType.Items.Add("Settle Ledger")
        txtTemplateType.Items.Add("Sub Ledger")
        txtTemplateType.Items.Add("Purchase")
        txtTemplateType.Items.Add("Purchase Register")
        txtTemplateType.Items.Add("Standard Sale")
        txtTemplateType.Items.Add("Standard Sale Register")
        txtTemplateType.Items.Add("Super Sale Register")
        txtTemplateType.Items.Add("Sellout Manual")
        txtTemplateType.Items.Add("Sellout Auto")
        txtTemplateType.Items.Add("Crate Ledger")
        If txtTemplateType.Items.Count > 0 Then txtTemplateType.SelectedIndex = 0
    End Sub



    Private Sub AttachEnterToNext(ByVal parent As Control)
        For Each ctl As Control In parent.Controls
            If TypeOf ctl Is TextBox OrElse TypeOf ctl Is ComboBox OrElse TypeOf ctl Is Button OrElse TypeOf ctl Is DataGridView Then
                AddHandler ctl.KeyDown, AddressOf EnterToNext_KeyDown
            End If
            If ctl.HasChildren Then AttachEnterToNext(ctl)
        Next
    End Sub

    Private Sub EnterToNext_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub LoadTemplates()
        Try
            Dim dt As DataTable = WhatsAppOfficialDb.GetTemplatesForDisplay()
            UpdateTemplateTabCounts(dt)
            dt = FilterTemplatesForSelectedTab(dt)
            AddTemplateSelectionColumn(dt)
            AddSerialNumbers(dt)
            dgvTemplates.DataSource = dt
            If dgvTemplates.Columns.Contains("SelectTemplate") Then
                dgvTemplates.Columns("SelectTemplate").HeaderText = "Tick"
                dgvTemplates.Columns("SelectTemplate").Width = 42
                dgvTemplates.Columns("SelectTemplate").ReadOnly = False
            End If
            If dgvTemplates.Columns.Contains("Description") Then dgvTemplates.Columns("Description").Visible = False
            If dgvTemplates.Columns.Contains("BodyText") Then dgvTemplates.Columns("BodyText").Visible = False
            If dgvTemplates.Columns.Contains("FooterText") Then dgvTemplates.Columns("FooterText").Visible = False
            If dgvTemplates.Columns.Contains("ButtonsJson") Then dgvTemplates.Columns("ButtonsJson").Visible = False
            If dgvTemplates.Columns.Contains("Examples") Then dgvTemplates.Columns("Examples").Visible = False
            If dgvTemplates.Columns.Contains("TemplateType") Then dgvTemplates.Columns("TemplateType").Visible = False
            If dgvTemplates.Columns.Contains("LocalTypeName") Then dgvTemplates.Columns("LocalTypeName").HeaderText = "Local Type"
            If dgvTemplates.Columns.Contains("TemplateCode") Then dgvTemplates.Columns("TemplateCode").HeaderText = "Code"
            If dgvTemplates.Columns.Contains("TemplateName") Then dgvTemplates.Columns("TemplateName").HeaderText = "Name"
            If dgvTemplates.Columns.Contains("SNo") Then dgvTemplates.Columns("SNo").HeaderText = "SNo."
            For Each col As DataGridViewColumn In dgvTemplates.Columns
                If col.Name <> "SelectTemplate" Then col.ReadOnly = True
            Next
            UpdateLocalSelectionControls()
            ApplyTemplateGridStyle()
            lblStatus.Text = "Templates loaded."
        Catch ex As Exception
            lblStatus.Text = ex.Message
        End Try
    End Sub

    Private Sub AddSerialNumbers(ByVal dt As DataTable)
        If dt Is Nothing Then Exit Sub
        If dt.Columns.Contains("SNo") = False Then dt.Columns.Add("SNo", GetType(Integer))
        For i As Integer = 0 To dt.Rows.Count - 1
            dt.Rows(i)("SNo") = i + 1
        Next
    End Sub

    Private Function FilterTemplatesForSelectedTab(ByVal source As DataTable) As DataTable
        If source Is Nothing Then Return New DataTable()
        If tabTemplates Is Nothing OrElse source.Columns.Contains("Status") = False Then Return source

        Dim filtered As DataTable = source.Clone()
        Dim wanted As String = "LOCAL"
        If tabTemplates.SelectedTab Is tabApprovedTemplates Then wanted = "APPROVED"
        If tabTemplates.SelectedTab Is tabPendingTemplates Then wanted = "PENDING"
        If tabTemplates.SelectedTab Is tabRejectedTemplates Then wanted = "REJECT"

        For Each row As DataRow In source.Rows
            Dim statusText As String = row("Status").ToString().ToUpper()
            Dim includeRow As Boolean = False
            If wanted = "LOCAL" Then
                includeRow = (statusText = "LOCAL" OrElse statusText.Trim() = "")
            ElseIf wanted = "APPROVED" Then
                includeRow = statusText.Contains("APPROVED")
            ElseIf wanted = "PENDING" Then
                includeRow = (statusText.Contains("PENDING") OrElse statusText.Contains("APPEAL"))
            ElseIf wanted = "REJECT" Then
                includeRow = (statusText.Contains("REJECT") OrElse statusText.Contains("FAILED"))
            End If
            If includeRow Then filtered.ImportRow(row)
        Next

        Return filtered
    End Function

    Private Sub UpdateTemplateTabCounts(ByVal source As DataTable)
        If tabTemplates Is Nothing OrElse source Is Nothing OrElse source.Columns.Contains("Status") = False Then Exit Sub
        Dim localCount As Integer = 0
        Dim approvedCount As Integer = 0
        Dim pendingCount As Integer = 0
        Dim rejectedCount As Integer = 0

        For Each row As DataRow In source.Rows
            Dim statusText As String = row("Status").ToString().ToUpper()
            If statusText = "LOCAL" OrElse statusText.Trim() = "" Then localCount += 1
            If statusText.Contains("APPROVED") Then approvedCount += 1
            If statusText.Contains("PENDING") OrElse statusText.Contains("APPEAL") Then pendingCount += 1
            If statusText.Contains("REJECT") OrElse statusText.Contains("FAILED") Then rejectedCount += 1
        Next

        tabLocalTemplates.Text = "Local (" & localCount.ToString() & ")"
        tabApprovedTemplates.Text = "Approved (" & approvedCount.ToString() & ")"
        tabPendingTemplates.Text = "Pending (" & pendingCount.ToString() & ")"
        tabRejectedTemplates.Text = "Rejected (" & rejectedCount.ToString() & ")"
    End Sub

    Private Sub AddTemplateSelectionColumn(ByVal dt As DataTable)
        If dt Is Nothing Then Exit Sub
        If tabTemplates Is Nothing OrElse tabTemplates.SelectedTab IsNot tabLocalTemplates Then Exit Sub
        If dt.Columns.Contains("SelectTemplate") = False Then
            dt.Columns.Add("SelectTemplate", GetType(Boolean))
            dt.Columns("SelectTemplate").SetOrdinal(0)
        End If
        For Each row As DataRow In dt.Rows
            row("SelectTemplate") = False
        Next
    End Sub

    Private Sub ApplyTemplateGridStyle()
        If dgvTemplates Is Nothing Then Exit Sub
        dgvTemplates.BorderStyle = BorderStyle.FixedSingle
        dgvTemplates.CellBorderStyle = DataGridViewCellBorderStyle.Single
        dgvTemplates.GridColor = Color.Silver
        dgvTemplates.EnableHeadersVisualStyles = False
        dgvTemplates.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
        dgvTemplates.ColumnHeadersDefaultCellStyle.ForeColor = Color.Navy
        dgvTemplates.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 240, 248)
        dgvTemplates.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 105, 160)
        dgvTemplates.DefaultCellStyle.SelectionForeColor = Color.White
        dgvTemplates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        If dgvTemplates.Columns.Contains("SelectTemplate") Then dgvTemplates.Columns("SelectTemplate").Width = 42
        If dgvTemplates.Columns.Contains("SNo") Then dgvTemplates.Columns("SNo").Width = 42
        If dgvTemplates.Columns.Contains("TemplateCode") Then dgvTemplates.Columns("TemplateCode").Width = 70
        If dgvTemplates.Columns.Contains("TemplateName") Then dgvTemplates.Columns("TemplateName").Width = 112
        If dgvTemplates.Columns.Contains("LanguageCode") Then dgvTemplates.Columns("LanguageCode").Width = 45
        If dgvTemplates.Columns.Contains("LocalTypeName") Then dgvTemplates.Columns("LocalTypeName").Width = 95
        If dgvTemplates.Columns.Contains("ParameterCount") Then dgvTemplates.Columns("ParameterCount").Width = 55
        If dgvTemplates.Columns.Contains("HeaderType") Then dgvTemplates.Columns("HeaderType").Width = 65
        If dgvTemplates.Columns.Contains("Status") Then dgvTemplates.Columns("Status").Width = 78
        If dgvTemplates.Columns.Contains("FileSupport") Then dgvTemplates.Columns("FileSupport").Width = 52
        If dgvTemplates.Columns.Contains("Category") Then dgvTemplates.Columns("Category").Width = 70

        If dgvTemplates.Columns.Contains("Status") = False Then Exit Sub
        For Each row As DataGridViewRow In dgvTemplates.Rows
            If row.IsNewRow Then Continue For
            Dim statusText As String = ""
            If row.Cells("Status").Value IsNot Nothing Then statusText = row.Cells("Status").Value.ToString().ToUpper()
            If dgvTemplates.Columns.Contains("SelectTemplate") Then
                row.Cells("SelectTemplate").Style.BackColor = Color.White
                row.Cells("SelectTemplate").Style.ForeColor = Color.Black
                row.Cells("SelectTemplate").Style.SelectionBackColor = Color.White
                row.Cells("SelectTemplate").Style.SelectionForeColor = Color.Black
            End If

            If statusText.Contains("APPROVED") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(224, 245, 232)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 100, 45)
            ElseIf statusText.Contains("PENDING") OrElse statusText.Contains("APPEAL") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 215)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 85, 0)
            ElseIf statusText.Contains("REJECT") OrElse statusText.Contains("FAILED") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230)
                row.DefaultCellStyle.ForeColor = Color.Maroon
            Else
                row.DefaultCellStyle.BackColor = Color.FromArgb(242, 246, 252)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(45, 60, 85)
            End If
        Next
    End Sub

    Private Sub tabTemplates_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tabTemplates.SelectedIndexChanged
        LoadTemplates()
    End Sub

    Private Sub dgvTemplates_CellContentClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvTemplates.CellContentClick
        If e.RowIndex >= 0 AndAlso dgvTemplates.Columns.Contains("SelectTemplate") AndAlso e.ColumnIndex = dgvTemplates.Columns("SelectTemplate").Index Then
            dgvTemplates.CommitEdit(DataGridViewDataErrorContexts.Commit)
            If BulkSelectingTemplates = False Then UpdateLocalSelectionControls()
        End If
    End Sub

    Private Sub dgvTemplates_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvTemplates.CurrentCellDirtyStateChanged
        If dgvTemplates.IsCurrentCellDirty AndAlso dgvTemplates.Columns.Contains("SelectTemplate") Then
            dgvTemplates.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Sub dgvTemplates_CellValueChanged(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvTemplates.CellValueChanged
        If e.RowIndex >= 0 AndAlso dgvTemplates.Columns.Contains("SelectTemplate") AndAlso e.ColumnIndex = dgvTemplates.Columns("SelectTemplate").Index Then
            If BulkSelectingTemplates = False Then UpdateLocalSelectionControls()
        End If
    End Sub

    Private Sub chkSelectAllLocal_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkSelectAllLocal.CheckedChanged
        If SuppressSelectAllEvent Then Exit Sub
        If tabTemplates Is Nothing OrElse tabTemplates.SelectedTab IsNot tabLocalTemplates Then Exit Sub
        If dgvTemplates.Columns.Contains("SelectTemplate") = False Then Exit Sub

        Dim targetChecked As Boolean = chkSelectAllLocal.Checked
        BulkSelectingTemplates = True
        Try
            dgvTemplates.EndEdit()
            For Each row As DataGridViewRow In dgvTemplates.Rows
                If row.IsNewRow Then Continue For
                row.Cells("SelectTemplate").Value = targetChecked
            Next
            dgvTemplates.EndEdit()
        Finally
            BulkSelectingTemplates = False
        End Try
        UpdateLocalSelectionControls()
    End Sub

    Private Function CountCheckedLocalTemplates() As Integer
        If dgvTemplates Is Nothing OrElse dgvTemplates.Columns.Contains("SelectTemplate") = False Then Return 0
        Dim selectedCount As Integer = 0
        For Each row As DataGridViewRow In dgvTemplates.Rows
            If row.IsNewRow Then Continue For
            Dim isChecked As Boolean = False
            If row.Cells("SelectTemplate").Value IsNot Nothing Then Boolean.TryParse(row.Cells("SelectTemplate").Value.ToString(), isChecked)
            If isChecked Then selectedCount += 1
        Next
        Return selectedCount
    End Function

    Private Sub UpdateLocalSelectionControls()
        Dim isLocalTab As Boolean = (tabTemplates Is Nothing OrElse tabTemplates.SelectedTab Is tabLocalTemplates)
        If chkSelectAllLocal IsNot Nothing Then chkSelectAllLocal.Visible = isLocalTab
        If btnSubmitSelectedLocal IsNot Nothing Then btnSubmitSelectedLocal.Visible = isLocalTab

        If isLocalTab = False Then Exit Sub

        Dim rowCount As Integer = 0
        If dgvTemplates IsNot Nothing Then
            For Each row As DataGridViewRow In dgvTemplates.Rows
                If row.IsNewRow = False Then rowCount += 1
            Next
        End If

        Dim selectedCount As Integer = CountCheckedLocalTemplates()
        SuppressSelectAllEvent = True
        If chkSelectAllLocal IsNot Nothing Then
            chkSelectAllLocal.Checked = (rowCount > 0 AndAlso selectedCount = rowCount)
            chkSelectAllLocal.Enabled = (rowCount > 0)
            chkSelectAllLocal.Font = New Font(chkSelectAllLocal.Font, If(selectedCount > 1, FontStyle.Bold, FontStyle.Regular))
            chkSelectAllLocal.ForeColor = If(selectedCount > 1, Color.DarkGreen, Color.Navy)
        End If
        SuppressSelectAllEvent = False

        If btnSubmitSelectedLocal IsNot Nothing Then
            btnSubmitSelectedLocal.Enabled = (selectedCount > 0)
            btnSubmitSelectedLocal.Text = If(selectedCount > 0, "Submit Selected Local (" & selectedCount.ToString() & ")", "Submit Selected Local")
            If selectedCount > 1 Then
                StyleButton(btnSubmitSelectedLocal, Color.SeaGreen)
            ElseIf selectedCount = 1 Then
                StyleButton(btnSubmitSelectedLocal, Color.SteelBlue)
            Else
                StyleButton(btnSubmitSelectedLocal, Color.Gray)
            End If
        End If
    End Sub

    Private Sub ApplyButtonStyles()
        StyleButton(btnSelectMedia, Color.SteelBlue)
        If btnUploadMedia IsNot Nothing Then btnUploadMedia.Visible = False
        StyleButton(btnSaveLocal, Color.DarkCyan)
        StyleButton(btnSubmitMeta, Color.SeaGreen)
        StyleButton(btnDeleteMeta, Color.Firebrick)
        StyleButton(btnRefresh, Color.SteelBlue)
        StyleButton(btnNewTemplate, Color.DarkSlateBlue)
        StyleButton(btnSubmitSelectedLocal, Color.Gray)
        StyleButton(btnP1, Color.FromArgb(70, 130, 180))
        btnP1.Text = "+ Add Variable"
        btnP2.Visible = False
        btnP3.Visible = False
        btnP4.Visible = False
        btnP5.Visible = False
        btnP6.Visible = False
    End Sub

    Private Sub StyleButton(ByVal btn As Button, ByVal backColor As Color)
        If btn Is Nothing Then Exit Sub
        btn.BackColor = backColor
        btn.ForeColor = Color.White
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 1
        btn.FlatAppearance.BorderColor = Color.Black
        btn.UseVisualStyleBackColor = False
    End Sub

    Private Sub EnsureSampleValuePanel()
        If SampleValuePanel IsNot Nothing Then Exit Sub
        SampleValuePanel = pnlSampleValues
        SampleValuePanel.Location = New Point(552, 493)
        SampleValuePanel.Size = New Size(518, 74)
        SampleValuePanel.BorderStyle = BorderStyle.FixedSingle
        SampleValuePanel.AutoScroll = True
        SampleValuePanel.WrapContents = True
        SampleValuePanel.FlowDirection = FlowDirection.LeftToRight
        If SampleValuePanel.Controls.Contains(txtExamples) Then SampleValuePanel.Controls.Remove(txtExamples)
        SampleValuePanel.BringToFront()
        txtExamples.Visible = False
        lblExamples.Text = "Template Parameters"
    End Sub



    Private Sub RememberTemplateCaret(ByVal box As TextBox)
        If box Is Nothing Then Exit Sub
        LastTemplateTextBox = box
        LastTemplateSelectionStart = box.SelectionStart
    End Sub

    Private Sub txtBody_CaretChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBody.Click, txtBody.KeyUp, txtBody.MouseUp, txtBody.Enter
        RememberTemplateCaret(txtBody)
    End Sub

    Private Sub txtFooter_CaretChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFooter.Click, txtFooter.KeyUp, txtFooter.MouseUp, txtFooter.Enter
        RememberTemplateCaret(txtFooter)
    End Sub

    Private Function UsedParameters() As List(Of Integer)
        Dim result As New List(Of Integer)()
        Dim source As String = txtBody.Text & " " & txtFooter.Text
        For Each m As Match In Regex.Matches(source, "\{\{(\d+)\}\}")
            Dim n As Integer = Val(m.Groups(1).Value)
            If n > 0 AndAlso result.Contains(n) = False Then result.Add(n)
        Next
        result.Sort()
        Return result
    End Function

    Private Function ParameterFieldForNumber(ByVal parameterNumber As Integer) As String
        If parameterNumber <= 0 Then Return ""
        EnsureParameterFieldsForUsedParameters()
        If parameterNumber <= TemplateParameterFields.Count Then Return TemplateParameterFields(parameterNumber - 1)

        Dim defaultFields() As String = DefaultParameterFieldsForCurrentType().Split(","c)
        If parameterNumber <= defaultFields.Length Then Return defaultFields(parameterNumber - 1).Trim()
        Return ""
    End Function

    Private Sub EnsureParameterFieldsForUsedParameters()
        Dim defaultFields() As String = DefaultParameterFieldsForCurrentType().Split(","c)
        For Each n As Integer In UsedParameters()
            While TemplateParameterFields.Count < n
                Dim defaultField As String = ""
                If TemplateParameterFields.Count < defaultFields.Length Then defaultField = defaultFields(TemplateParameterFields.Count).Trim()
                TemplateParameterFields.Add(defaultField)
            End While
            If n > 0 AndAlso n <= TemplateParameterFields.Count AndAlso TemplateParameterFields(n - 1).Trim() = "" AndAlso n <= defaultFields.Length Then
                TemplateParameterFields(n - 1) = defaultFields(n - 1).Trim()
            End If
        Next
    End Sub

    Private Function SampleValueForField(ByVal fieldName As String) As String
        Select Case NormalizeParameterFieldKey(fieldName)
            Case "company_name" : Return "Shree Balaji Traders"
            Case "account_name" : Return "Ramesh Ji"
            Case "customer_mobile_no" : Return "9876543210"
            Case "customer_city" : Return "Jaipur"
            Case "bill_no" : Return "B-1024"
            Case "bill_date", "receipt_date", "payment_date", "entry_date", "balance_date", "from_date" : Return "25-05-2026"
            Case "to_date" : Return "31-05-2026"
            Case "bill_total", "amount", "balance_amount", "total_amount" : Return "8500"
            Case "nug" : Return "25"
            Case "weight" : Return "1250.00"
            Case "basic_amount" : Return "7800"
            Case "charges" : Return "700"
            Case "pdf_link" : Return "https://msgz.in/5SACW2"
            Case "message_text" : Return "Please verify this document."
            Case "payment_mode" : Return "Cash"
            Case "dr_cr" : Return "Dr"
            Case "opening_balance" : Return "2500 Dr"
            Case "closing_balance" : Return "8500 Dr"
            Case "debit_total" : Return "12000"
            Case "credit_total" : Return "3500"
            Case "crate_qty" : Return "15"
            Case "marka" : Return "M-01"
            Case "entry_no" : Return "E-1024"
            Case "receipt_no" : Return "R-1024"
            Case "payment_no" : Return "P-1024"
            Case "total_count" : Return "10"
        End Select
        Return If(fieldName, "").Replace("_", " ")
    End Function

    Private Function ParameterFieldDisplayName(ByVal fieldKey As String) As String
        Dim key As String = NormalizeParameterFieldKey(fieldKey)
        Dim dt As DataTable = ParameterFieldOptions()
        For Each row As DataRow In dt.Rows
            If row("FieldKey").ToString().Trim().ToLower() = key Then Return row("DisplayName").ToString()
        Next
        Return If(fieldKey, "").Replace("_", " ")
    End Function

    Private Sub BuildSampleInputs(Optional ByVal existingExamples As String = "")
        EnsureSampleValuePanel()
        Dim oldValues As New Dictionary(Of Integer, String)()
        For Each pair As KeyValuePair(Of Integer, TextBox) In SampleTextBoxes
            oldValues(pair.Key) = pair.Value.Text
        Next
        If existingExamples.Trim <> "" Then
            Dim parts() As String = existingExamples.Split("|"c)
            For i As Integer = 0 To parts.Length - 1
                oldValues(i + 1) = parts(i).Trim()
            Next
        End If

        SampleValuePanel.Controls.Clear()
        SampleTextBoxes.Clear()
        For Each n As Integer In UsedParameters()
            Dim box As New TextBox()
            box.Name = "txtSampleP" & n.ToString()
            box.BorderStyle = BorderStyle.FixedSingle
            box.Width = 1
            box.Height = 1
            box.Visible = False
            box.Tag = n
            Dim fieldName As String = ParameterFieldForNumber(n)
            If oldValues.ContainsKey(n) AndAlso oldValues(n).Trim() <> "" Then
                box.Text = oldValues(n)
            Else
                box.Text = SampleValueForField(fieldName)
            End If
            AddHandler box.TextChanged, AddressOf SampleTextChanged
            AddHandler box.KeyDown, AddressOf EnterToNext_KeyDown

            Dim lbl As New Label()
            lbl.AutoSize = False
            lbl.Width = 245
            lbl.Height = 26
            lbl.TextAlign = ContentAlignment.MiddleLeft
            lbl.Text = "{{" & n.ToString() & "}} " & ParameterFieldDisplayName(fieldName)
            lbl.ForeColor = Color.Navy

            SampleValuePanel.Controls.Add(lbl)
            SampleValuePanel.Controls.Add(box)
            SampleTextBoxes(n) = box
        Next

        If SampleTextBoxes.Count = 0 Then
            Dim lbl As New Label()
            lbl.AutoSize = False
            lbl.Width = 480
            lbl.Height = 28
            lbl.TextAlign = ContentAlignment.MiddleLeft
            lbl.ForeColor = Color.Gray
            lbl.Text = "Add parameters in body or footer to enter sample values."
            SampleValuePanel.Controls.Add(lbl)
        End If
        SyncExamplesText()
    End Sub

    Private Sub SyncExamplesText()
        Dim values As New List(Of String)()
        For Each n As Integer In UsedParameters()
            If SampleTextBoxes.ContainsKey(n) Then values.Add(SampleTextBoxes(n).Text.Trim())
        Next
        txtExamples.Text = String.Join("|", values.ToArray())
    End Sub

    Private Sub SampleTextChanged(ByVal sender As Object, ByVal e As EventArgs)
        SyncExamplesText()
    End Sub

    Private Sub dgvTemplates_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvTemplates.SelectionChanged
        If dgvTemplates.CurrentRow Is Nothing Then Exit Sub
        Try
            LoadingTemplateSelection = True
            txtTemplateCode.Text = CellText("TemplateCode")
            txtTemplateTitle.Text = CellText("TemplateName")
            cbLanguage.Text = CellText("LanguageCode")
            SetLocalTypeFromCode(CellText("TemplateType"))
            SetFormatFromLanguage(CellText("LanguageCode"))
            LoadParameterFieldCombo()
            txtBody.Text = If(CellText("BodyText") <> "", CellText("BodyText"), CellText("Description"))
            txtFooter.Text = CellText("FooterText")
            LoadQuickReplyButtons(CellText("ButtonsJson"))
            txtExamples.Text = CellText("Examples")
            cbCategory.Text = If(CellText("Category") <> "", CellText("Category"), "UTILITY")
            SetHeaderTypeFromText(CellText("HeaderType"), CellText("FileSupport"))
            LoadParameterFieldsFromText(WhatsAppOfficialDb.GetTemplateParameterFields(CurrentLocalTypeCode().ToUpper(), txtTemplateCode.Text.Trim, CurrentMappingKey()))
            If TemplateParameterFields.Count = 0 Then LoadParameterFieldsFromText(DefaultParameterFieldsForCurrentType())
            UploadedMediaFileName = ""
            SelectedMediaPath = ""
            If txtMediaFile IsNot Nothing Then txtMediaFile.Text = ""
            BuildSampleInputs(CellText("Examples"))
            lblStatus.Text = "Status: " & CellText("Status")
        Catch ex As Exception
        Finally
            LoadingTemplateSelection = False
        End Try
    End Sub

    Private Function CellText(ByVal columnName As String) As String
        If dgvTemplates.CurrentRow Is Nothing Then Return ""
        If dgvTemplates.Columns.Contains(columnName) = False Then Return ""
        Dim value As Object = dgvTemplates.CurrentRow.Cells(columnName).Value
        If value Is Nothing Then Return ""
        Return value.ToString()
    End Function

    Private Sub SetHeaderTypeFromText(ByVal headerType As String, ByVal fileSupport As String)
        headerType = If(headerType, "").Trim().ToLower()
        If headerType = "" Then
            If If(fileSupport, "").Trim().ToUpper() = "YES" Then headerType = "document" Else headerType = "none"
        End If
        For i As Integer = 0 To cbHeaderType.Items.Count - 1
            If cbHeaderType.Items(i).ToString().Trim().ToLower() = headerType Then
                cbHeaderType.SelectedIndex = i
                UpdateHeaderMediaVisibility()
                Exit Sub
            End If
        Next
        If cbHeaderType.Items.Count > 0 Then cbHeaderType.SelectedIndex = 0
        UpdateHeaderMediaVisibility()
    End Sub


    Private Sub ClearTemplateEditor()
        txtTemplateCode.Text = ""
        txtTemplateTitle.Text = ""
        If cbLanguage.Items.Count > 0 Then cbLanguage.SelectedIndex = 0
        If cbTemplateFormat.Items.Count > 0 Then cbTemplateFormat.SelectedIndex = 0
        If txtTemplateType.Items.Count > 0 Then txtTemplateType.SelectedIndex = 0
        If cbCategory.Items.Count > 0 Then cbCategory.SelectedIndex = 0
        If cbHeaderType.Items.Count > 0 Then cbHeaderType.SelectedIndex = 0
        txtMediaFile.Text = ""
        txtBody.Text = ""
        txtFooter.Text = ""
        SetDefaultQuickReplyButtons()
        txtExamples.Text = ""
        LoadParameterFieldCombo()
        LoadParameterFieldsFromText(DefaultParameterFieldsForCurrentType())
        SelectedMediaPath = ""
        UploadedMediaFileName = ""
        BuildSampleInputs()
        lblStatus.Text = "New template mode. Enter details, then Save Local or Submit / Update Meta."
        txtTemplateCode.Focus()
    End Sub

    Private Sub btnNewTemplate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewTemplate.Click
        ClearTemplateEditor()
    End Sub

    Private Sub EnsureBodyHasSafeEnding()
        Dim bodyText As String = txtBody.Text
        If bodyText.Trim() = "" Then Exit Sub

        Dim fixedBody As String = bodyText.TrimEnd()
        Dim changed As Boolean = False

        If Regex.IsMatch(fixedBody, "^\s*\{\{\d+\}\}") Then
            fixedBody = "Hello " & fixedBody.TrimStart()
            changed = True
        End If

        Dim sentenceEndingPattern As String = "(\{\{\d+\}\})(\s*[\.\!\?])(?=\s*(\r?\n|$))"
        If Regex.IsMatch(fixedBody, sentenceEndingPattern) Then
            fixedBody = Regex.Replace(fixedBody, sentenceEndingPattern, "$1 as per records$2")
            changed = True
        End If

        If Regex.IsMatch(fixedBody, "\{\{\d+\}\}\s*[\.\,\;\:\!\?\)]*\s*$") Then
            fixedBody &= " Thank you."
            changed = True
        End If

        If changed Then
            txtBody.Text = fixedBody
            txtBody.SelectionStart = txtBody.TextLength
            lblStatus.Text = "Message body adjusted for Meta template rules."
        End If
    End Sub

    Private Sub EnsureParametersAreBold()
        Dim changed As Boolean = False
        Dim boldParameter As MatchEvaluator = Function(m)
                                                  Dim sourceText As String = m.Value
                                                  If sourceText.StartsWith("*") AndAlso sourceText.EndsWith("*") Then Return sourceText
                                                  changed = True
                                                  Return "*" & sourceText & "*"
                                              End Function

        txtBody.Text = Regex.Replace(txtBody.Text, "(?<!\*)\{\{\d+\}\}(?!\*)", boldParameter)
        txtFooter.Text = Regex.Replace(txtFooter.Text, "(?<!\*)\{\{\d+\}\}(?!\*)", boldParameter)
        If changed Then lblStatus.Text = "Template parameters formatted as bold."
    End Sub

    Private Sub NormalizeTemplateParameterNumbers()
        Dim bodyText As String = txtBody.Text
        If bodyText.Trim() = "" Then Exit Sub

        Dim oldToNew As New Dictionary(Of Integer, Integer)()
        Dim newFields As New List(Of String)()
        Dim changed As Boolean = False

        Dim normalizedBody As String = Regex.Replace(bodyText, "\{\{(\d+)\}\}", Function(m)
                                                                                    Dim oldNumber As Integer = Val(m.Groups(1).Value)
                                                                                    If oldNumber <= 0 Then Return m.Value
                                                                                    If oldToNew.ContainsKey(oldNumber) = False Then
                                                                                        oldToNew(oldNumber) = oldToNew.Count + 1
                                                                                        Dim fieldName As String = ParameterFieldForNumber(oldNumber)
                                                                                        If fieldName.Trim() <> "" Then newFields.Add(fieldName.Trim())
                                                                                    End If
                                                                                    Dim newNumber As Integer = oldToNew(oldNumber)
                                                                                    If newNumber <> oldNumber Then changed = True
                                                                                    Return "{{" & newNumber.ToString() & "}}"
                                                                                End Function)

        If changed Then
            txtBody.Text = normalizedBody
            LoadParameterFieldsFromText(String.Join(",", newFields.ToArray()))
            BuildSampleInputs()
            lblStatus.Text = "Template parameters normalized for Meta."
        End If
    End Sub

    Private Sub btnSaveLocal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveLocal.Click
        EnsureBodyHasSafeEnding()
        NormalizeTemplateParameterNumbers()
        EnsureParametersAreBold()
        If ValidateTemplateFields() = False Then Exit Sub
        BuildSampleInputs()
        SyncExamplesText()
        WhatsAppOfficialDb.SaveLocalTemplate(txtTemplateCode.Text.Trim, txtTemplateTitle.Text.Trim, cbLanguage.Text.Trim, CurrentLocalTypeCode(), cbHeaderType.Text.Trim, txtBody.Text.Trim, txtFooter.Text.Trim, cbCategory.Text.Trim, txtExamples.Text.Trim, CurrentButtonsJson())
        SaveTemplateMappingForSelection()
        LoadTemplates()
        lblStatus.Text = "Saved locally. Click Submit/Update Meta when you want to send it for approval."
    End Sub

    Private Sub btnSubmitMeta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmitMeta.Click
        SubmitCurrentTemplateToMeta(True, True)
    End Sub

    Private Function SubmitCurrentTemplateToMeta(ByVal showResultMessage As Boolean, ByVal reloadAfterSuccess As Boolean) As Boolean
        EnsureBodyHasSafeEnding()
        NormalizeTemplateParameterNumbers()
        EnsureParametersAreBold()
        If ValidateTemplateFields() = False Then Return False
        If VendorUid.Trim = "" OrElse AccessToken.Trim = "" Then
            MsgBox("Vendor ID / Access Token is missing. Please save or validate it on the WhatsApp API screen first.", MsgBoxStyle.Critical, "Template Editor")
            Return False
        End If
        BuildSampleInputs()
        SyncExamplesText()
        WhatsAppOfficialDb.SaveLocalTemplate(txtTemplateCode.Text.Trim, txtTemplateTitle.Text.Trim, cbLanguage.Text.Trim, CurrentLocalTypeCode(), cbHeaderType.Text.Trim, txtBody.Text.Trim, txtFooter.Text.Trim, cbCategory.Text.Trim, txtExamples.Text.Trim, CurrentButtonsJson())
        SaveTemplateMappingForSelection()
        If EnsureSampleUploaded() = False Then Return False
        Dim responseMessage As String = ""
        If WhatsAppOfficialApi.SubmitTemplate(VendorUid, AccessToken, txtTemplateCode.Text.Trim, MetaLanguageCode(), cbCategory.Text.Trim, cbHeaderType.Text.Trim, txtBody.Text.Trim, txtFooter.Text.Trim, txtExamples.Text.Trim, UploadedMediaFileName, responseMessage, CurrentButtonsJson()) Then
            WhatsAppOfficialDb.SyncMetaTemplatesFromApi(VendorUid, AccessToken, responseMessage)
            If reloadAfterSuccess Then LoadTemplates()
            If showResultMessage Then MsgBox("Template submitted or updated on Meta." & vbCrLf & responseMessage, MsgBoxStyle.Information, "Template Editor")
            Return True
        Else
            lblStatus.Text = responseMessage
            If showResultMessage Then MsgBox(responseMessage, MsgBoxStyle.Critical, "Template Editor")
            Return False
        End If
    End Function

    Private Sub btnSubmitSelectedLocal_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmitSelectedLocal.Click
        If dgvTemplates.Columns.Contains("SelectTemplate") = False Then
            MsgBox("Open the Local tab and tick templates to submit.", MsgBoxStyle.Information, "Template Editor")
            Exit Sub
        End If

        Dim templateCodes As New List(Of String)()
        For Each row As DataGridViewRow In dgvTemplates.Rows
            If row.IsNewRow Then Continue For
            Dim isChecked As Boolean = False
            If row.Cells("SelectTemplate").Value IsNot Nothing Then Boolean.TryParse(row.Cells("SelectTemplate").Value.ToString(), isChecked)
            If isChecked AndAlso row.Cells("TemplateCode").Value IsNot Nothing Then templateCodes.Add(row.Cells("TemplateCode").Value.ToString())
        Next

        If templateCodes.Count = 0 Then
            MsgBox("Please tick at least one local template.", MsgBoxStyle.Information, "Template Editor")
            Exit Sub
        End If

        If MsgBox("Submit " & templateCodes.Count.ToString() & " local template(s) to Meta for approval?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Template Editor") <> MsgBoxResult.Yes Then Exit Sub

        Dim successCount As Integer = 0
        Dim failedCount As Integer = 0
        Dim firstFailure As String = ""

        For Each templateCode As String In templateCodes
            If SelectTemplateInGrid(templateCode) = False Then
                failedCount += 1
                If firstFailure = "" Then firstFailure = templateCode & ": not found in current list."
                Continue For
            End If
            lblStatus.Text = "Submitting " & templateCode & "..."
            Application.DoEvents()
            If SubmitCurrentTemplateToMeta(False, False) Then
                successCount += 1
            Else
                failedCount += 1
                If firstFailure = "" Then firstFailure = templateCode & ": " & lblStatus.Text
            End If
        Next

        LoadTemplates()
        Dim summary As String = "Template submit complete." & vbCrLf & "Submitted: " & successCount.ToString() & vbCrLf & "Failed: " & failedCount.ToString()
        If firstFailure <> "" Then summary &= vbCrLf & "First error: " & firstFailure
        MsgBox(summary, If(failedCount > 0, MsgBoxStyle.Exclamation, MsgBoxStyle.Information), "Template Editor")
    End Sub

    Private Function SelectTemplateInGrid(ByVal templateCode As String) As Boolean
        For Each row As DataGridViewRow In dgvTemplates.Rows
            If row.IsNewRow OrElse row.Cells("TemplateCode").Value Is Nothing Then Continue For
            If row.Cells("TemplateCode").Value.ToString().Trim().ToLower() = templateCode.Trim().ToLower() Then
                dgvTemplates.ClearSelection()
                row.Selected = True
                Dim firstVisibleIndex As Integer = 0
                For Each col As DataGridViewColumn In dgvTemplates.Columns
                    If col.Visible AndAlso col.Name <> "SelectTemplate" Then
                        firstVisibleIndex = col.Index
                        Exit For
                    End If
                Next
                dgvTemplates.CurrentCell = row.Cells(firstVisibleIndex)
                dgvTemplates_SelectionChanged(dgvTemplates, EventArgs.Empty)
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub btnDeleteMeta_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteMeta.Click
        If txtTemplateCode.Text.Trim = "" Then Exit Sub
        If MsgBox("Do you want to delete this template from Meta/server?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Template Editor") <> MsgBoxResult.Yes Then Exit Sub
        Dim responseMessage As String = ""
        If WhatsAppOfficialApi.DeleteTemplate(VendorUid, AccessToken, txtTemplateCode.Text.Trim, MetaLanguageCode(), responseMessage) Then
            WhatsAppOfficialDb.SyncMetaTemplatesFromApi(VendorUid, AccessToken, responseMessage)
            LoadTemplates()
            MsgBox(responseMessage, MsgBoxStyle.Information, "Template Editor")
        Else
            MsgBox(responseMessage, MsgBoxStyle.Critical, "Template Editor")
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
        Dim responseMessage As String = ""
        If VendorUid.Trim <> "" AndAlso AccessToken.Trim <> "" Then
            WhatsAppOfficialDb.SyncMetaTemplatesFromApi(VendorUid, AccessToken, responseMessage)
        End If
        LoadTemplates()
        lblStatus.Text = responseMessage
    End Sub

    Private Function CurrentLocalTypeCode() As String
        Select Case txtTemplateType.Text.Trim
            Case "Print Bill" : Return "print_bill"
            Case "Receipt" : Return "receipt"
            Case "Payment" : Return "payment"
            Case "Balance" : Return "balance"
            Case "Statement" : Return "statement"
            Case "Crate In" : Return "crate_in"
            Case "Crate Out" : Return "crate_out"
            Case "Ledger" : Return "ledger"
            Case "Settle Ledger" : Return "settle_ledger"
            Case "Sub Ledger" : Return "sub_ledger"
            Case "Purchase" : Return "purchase"
            Case "Purchase Register" : Return "purchase_register"
            Case "Standard Sale" : Return "standard_sale"
            Case "Standard Sale Register" : Return "standard_sale_register"
            Case "Super Sale Register" : Return "super_sale_register"
            Case "Sellout Manual" : Return "sellout_manual"
            Case "Sellout Auto" : Return "sellout_auto"
            Case "Crate Ledger" : Return "crate_ledger"
        End Select
        Return txtTemplateType.Text.Trim.ToLower().Replace(" ", "_")
    End Function

    Private Sub SetLocalTypeFromCode(ByVal typeCode As String)
        Dim code As String = If(typeCode, "").Trim().ToLower()
        Dim displayText As String = ""
        Select Case code
            Case "sale_bill", "print_bill", "print_bill_pdf_only", "print_bill_pdf_message" : displayText = "Print Bill"
            Case "receipt" : displayText = "Receipt"
            Case "payment" : displayText = "Payment"
            Case "balance" : displayText = "Balance"
            Case "statement" : displayText = "Statement"
            Case "crate_in" : displayText = "Crate In"
            Case "crate_out" : displayText = "Crate Out"
            Case "ledger" : displayText = "Ledger"
            Case "settle_ledger" : displayText = "Settle Ledger"
            Case "sub_ledger" : displayText = "Sub Ledger"
            Case "purchase" : displayText = "Purchase"
            Case "purchase_register" : displayText = "Purchase Register"
            Case "standard_sale" : displayText = "Standard Sale"
            Case "standard_sale_register" : displayText = "Standard Sale Register"
            Case "super_sale_register" : displayText = "Super Sale Register"
            Case "sellout_manual" : displayText = "Sellout Manual"
            Case "sellout_auto" : displayText = "Sellout Auto"
            Case "crate_ledger" : displayText = "Crate Ledger"
        End Select
        If displayText <> "" AndAlso txtTemplateType.Items.Contains(displayText) Then txtTemplateType.Text = displayText
    End Sub

    Private Sub SetFormatFromLanguage(ByVal languageCode As String)
        Dim value As String = If(languageCode, "").Trim().ToLower()
        If value.StartsWith("hi") OrElse value.Contains("regional") Then
            cbTemplateFormat.Text = "Regional"
        Else
            cbTemplateFormat.Text = "English"
        End If
    End Sub

    Private Function ParameterFieldOptions() As DataTable
        Dim dt As New DataTable()
        dt.Columns.Add("DisplayName")
        dt.Columns.Add("FieldKey")

        AddParameterField(dt, "Firm Name", "company_name")
        AddParameterField(dt, "Account Name", "account_name")
        AddParameterField(dt, "Customer Mobile No.", "customer_mobile_no")
        AddParameterField(dt, "Customer City", "customer_city")

        Select Case CurrentLocalTypeCode()
            Case "print_bill", "purchase", "standard_sale", "sellout_manual", "sellout_auto"
                AddParameterField(dt, "Bill No.", "bill_no")
                AddParameterField(dt, "Bill Date", "bill_date")
                AddParameterField(dt, "Bill Total", "bill_total")
                AddParameterField(dt, "Nug", "nug")
                AddParameterField(dt, "Weight", "weight")
                AddParameterField(dt, "Basic Amount", "basic_amount")
                AddParameterField(dt, "Charges", "charges")
                AddParameterField(dt, "PDF Link", "pdf_link")
                AddParameterField(dt, "Message Text", "message_text")
            Case "receipt"
                AddParameterField(dt, "Receipt No.", "receipt_no")
                AddParameterField(dt, "Receipt Date", "receipt_date")
                AddParameterField(dt, "Amount", "amount")
                AddParameterField(dt, "Payment Mode", "payment_mode")
                AddParameterField(dt, "PDF Link", "pdf_link")
            Case "payment"
                AddParameterField(dt, "Payment No.", "payment_no")
                AddParameterField(dt, "Payment Date", "payment_date")
                AddParameterField(dt, "Amount", "amount")
                AddParameterField(dt, "Payment Mode", "payment_mode")
                AddParameterField(dt, "PDF Link", "pdf_link")
            Case "balance"
                AddParameterField(dt, "Balance Date", "balance_date")
                AddParameterField(dt, "Balance Amount", "balance_amount")
                AddParameterField(dt, "Dr / Cr", "dr_cr")
            Case "statement", "ledger", "settle_ledger", "sub_ledger", "crate_ledger"
                AddParameterField(dt, "From Date", "from_date")
                AddParameterField(dt, "To Date", "to_date")
                AddParameterField(dt, "Opening Balance", "opening_balance")
                AddParameterField(dt, "Closing Balance", "closing_balance")
                AddParameterField(dt, "Debit Total", "debit_total")
                AddParameterField(dt, "Credit Total", "credit_total")
                AddParameterField(dt, "PDF Link", "pdf_link")
            Case "crate_in", "crate_out"
                AddParameterField(dt, "Entry No.", "entry_no")
                AddParameterField(dt, "Entry Date", "entry_date")
                AddParameterField(dt, "Crate Qty", "crate_qty")
                AddParameterField(dt, "Marka", "marka")
                AddParameterField(dt, "PDF Link", "pdf_link")
            Case "purchase_register", "standard_sale_register", "super_sale_register"
                AddParameterField(dt, "From Date", "from_date")
                AddParameterField(dt, "To Date", "to_date")
                AddParameterField(dt, "Total Count", "total_count")
                AddParameterField(dt, "Total Amount", "bill_total")
                AddParameterField(dt, "PDF Link", "pdf_link")
        End Select

        Return dt
    End Function

    Private Sub AddParameterField(ByVal dt As DataTable, ByVal displayName As String, ByVal fieldKey As String)
        Dim key As String = NormalizeParameterFieldKey(fieldKey)
        dt.Rows.Add(key, key)
    End Sub

    Private Function NormalizeParameterFieldKey(ByVal fieldKey As String) As String
        Dim key As String = If(fieldKey, "").Trim().ToLower().Replace(" ", "_").Replace("/", "_")
        key = Regex.Replace(key, "_+", "_")
        Select Case key
            Case "firm_name"
                Return "company_name"
            Case "customer_name", "customer_account_name", "party_name"
                Return "account_name"
            Case "mobile_no", "mobile", "whatsapp_no", "customer_mobile", "account_mobile"
                Return "customer_mobile_no"
            Case "city", "account_city"
                Return "customer_city"
            Case "total_amount", "sale_total"
                Return "bill_total"
            Case "receipt_amount", "payment_amount", "balance_amount"
                Return "amount"
        End Select
        Return key
    End Function

    Private Sub LoadParameterFieldCombo()
        If cbParameterField Is Nothing Then Exit Sub
        Dim selectedKey As String = ""
        If cbParameterField.SelectedValue IsNot Nothing Then selectedKey = cbParameterField.SelectedValue.ToString()

        Dim dt As DataTable = ParameterFieldOptions()
        cbParameterField.DataSource = Nothing
        cbParameterField.Items.Clear()
        cbParameterField.DisplayMember = "DisplayName"
        cbParameterField.ValueMember = "FieldKey"
        cbParameterField.DataSource = dt
        cbParameterField.ForeColor = Color.Black
        cbParameterField.BackColor = Color.White

        If selectedKey <> "" Then
            selectedKey = NormalizeParameterFieldKey(selectedKey)
            For i As Integer = 0 To cbParameterField.Items.Count - 1
                Dim row As DataRowView = TryCast(cbParameterField.Items(i), DataRowView)
                If row IsNot Nothing AndAlso row("FieldKey").ToString() = selectedKey Then
                    cbParameterField.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
        If cbParameterField.SelectedIndex < 0 AndAlso cbParameterField.Items.Count > 0 Then cbParameterField.SelectedIndex = 0
        lblStatus.Text = "Parameter fields loaded: " & cbParameterField.Items.Count.ToString() & ". Double-click a field to add it."
    End Sub

    Private Function SelectedParameterFieldKey() As String
        If cbParameterField Is Nothing Then Return ""
        If cbParameterField.SelectedValue IsNot Nothing Then Return NormalizeParameterFieldKey(cbParameterField.SelectedValue.ToString())
        Return ""
    End Function

    Private Sub LoadParameterFieldsFromText(ByVal parameterFields As String)
        TemplateParameterFields.Clear()
        For Each part As String In If(parameterFields, "").Split(","c)
            Dim fieldName As String = NormalizeParameterFieldKey(part)
            If fieldName <> "" Then TemplateParameterFields.Add(fieldName)
        Next
    End Sub

    Private Function CurrentMappingKey() As String
        Dim moduleCode As String = CurrentLocalTypeCode().ToUpper()
        Dim langCode As String = If(CurrentLanguageCode() = "hi", "HI", "EN")
        Return moduleCode & "_" & langCode
    End Function

    Private Function CurrentLanguageCode() As String
        If cbTemplateFormat.Text.Trim = "Regional" Then Return "hi"
        Return "en"
    End Function

    Private Function CurrentParameterFields() As String
        EnsureParameterFieldsForUsedParameters()
        Dim used As List(Of Integer) = UsedParameters()
        If TemplateParameterFields.Count > 0 AndAlso used.Count > 0 Then
            Dim activeFields As New List(Of String)()
            For Each n As Integer In used
                If n > 0 AndAlso n <= TemplateParameterFields.Count AndAlso TemplateParameterFields(n - 1).Trim() <> "" Then
                    activeFields.Add(TemplateParameterFields(n - 1).Trim())
                End If
            Next
            If activeFields.Count > 0 Then Return String.Join(",", activeFields.ToArray())
        End If

        Dim storedFields As String = WhatsAppOfficialDb.GetTemplateMappingParameterFields(CurrentMappingKey())
        If storedFields.Trim() <> "" Then
            LoadParameterFieldsFromText(storedFields)
            Return storedFields
        End If

        Dim defaultFields As String = DefaultParameterFieldsForCurrentType()
        LoadParameterFieldsFromText(defaultFields)
        Return defaultFields
    End Function

    Private Function DefaultParameterFieldsForCurrentType() As String
        Select Case CurrentLocalTypeCode()
            Case "print_bill"
                Return "account_name,bill_date,company_name,bill_total,nug,weight,basic_amount,charges,customer_city,customer_mobile_no"
            Case "receipt"
                Return "company_name,account_name,receipt_date,amount"
            Case "payment"
                Return "company_name,account_name,payment_date,amount"
            Case "balance"
                Return "company_name,account_name,balance_date,balance_amount"
            Case "statement", "ledger", "settle_ledger", "sub_ledger", "crate_ledger"
                Return "company_name,account_name,from_date,to_date"
            Case "crate_in", "crate_out"
                Return "company_name,account_name,entry_date,crate_qty"
            Case "purchase", "purchase_register", "standard_sale", "standard_sale_register", "super_sale_register", "sellout_manual", "sellout_auto"
                Return "company_name,account_name,bill_date,bill_total"
        End Select
        Return "company_name,account_name,entry_date,amount"
    End Function

    Private Sub SaveTemplateMappingForSelection()
        If txtTemplateCode.Text.Trim = "" Then Exit Sub
        Dim moduleCode As String = CurrentLocalTypeCode().ToUpper()
        Dim mappingKey As String = CurrentMappingKey() & "_" & txtTemplateCode.Text.Trim.ToUpper()
        WhatsAppOfficialDb.SaveTemplateMapping(mappingKey, moduleCode, txtTemplateType.Text.Trim & " " & cbTemplateFormat.Text.Trim, txtTemplateCode.Text.Trim, CurrentLanguageCode(), "BILL", CurrentParameterFields())
    End Sub

    Private Sub SetDefaultQuickReplyButtons()
        If chkQuickReplies Is Nothing Then Exit Sub
        chkQuickReplies.Checked = True
        If CurrentLanguageCode() = "hi" Then
            txtButton1.Text = "हाँ, सही है"
            txtButton2.Text = "नहीं, गलती है"
        Else
            txtButton1.Text = "Yes, Right"
            txtButton2.Text = "No, Wrong"
        End If
        UpdateQuickReplyControls()
    End Sub

    Private Function HasDefaultQuickReplyText() As Boolean
        Dim b1 As String = If(txtButton1 Is Nothing, "", txtButton1.Text.Trim())
        Dim b2 As String = If(txtButton2 Is Nothing, "", txtButton2.Text.Trim())
        If b1 = "" AndAlso b2 = "" Then Return True
        If b1 = "Yes, Right" AndAlso b2 = "No, Wrong" Then Return True
        If b1 = "हाँ, सही है" AndAlso b2 = "नहीं, गलती है" Then Return True
        Return False
    End Function

    Private Sub LoadQuickReplyButtons(ByVal buttonsJson As String)
        If chkQuickReplies Is Nothing Then Exit Sub
        txtButton1.Text = ""
        txtButton2.Text = ""
        If If(buttonsJson, "").Trim() = "" Then
            SetDefaultQuickReplyButtons()
            Exit Sub
        End If

        Try
            Dim buttons As JArray = JArray.Parse(buttonsJson)
            Dim textValues As New List(Of String)()
            For Each token As JToken In buttons
                If token Is Nothing OrElse token.Type <> JTokenType.Object Then Continue For
                Dim item As JObject = CType(token, JObject)
                If item("text") IsNot Nothing AndAlso item("text").ToString().Trim() <> "" Then textValues.Add(item("text").ToString())
            Next
            chkQuickReplies.Checked = (textValues.Count > 0)
            If textValues.Count > 0 Then txtButton1.Text = textValues(0)
            If textValues.Count > 1 Then txtButton2.Text = textValues(1)
        Catch ex As Exception
            SetDefaultQuickReplyButtons()
        End Try
        UpdateQuickReplyControls()
    End Sub

    Private Function CurrentButtonsJson() As String
        If chkQuickReplies Is Nothing OrElse chkQuickReplies.Checked = False Then Return ""
        Dim b1 As String = txtButton1.Text.Trim()
        Dim b2 As String = txtButton2.Text.Trim()
        Dim buttons As New JArray()
        If b1 <> "" Then
            Dim item As New JObject()
            item("type") = New JValue("QUICK_REPLY")
            item("text") = New JValue(b1)
            buttons.Add(item)
        End If
        If b2 <> "" AndAlso b2 <> b1 Then
            Dim item As New JObject()
            item("type") = New JValue("QUICK_REPLY")
            item("text") = New JValue(b2)
            buttons.Add(item)
        End If
        Return If(buttons.Count > 0, buttons.ToString(Newtonsoft.Json.Formatting.None), "")
    End Function

    Private Sub UpdateQuickReplyControls()
        Dim enabledButtons As Boolean = (chkQuickReplies IsNot Nothing AndAlso chkQuickReplies.Checked)
        If txtButton1 IsNot Nothing Then txtButton1.Enabled = enabledButtons
        If txtButton2 IsNot Nothing Then txtButton2.Enabled = enabledButtons
        If lblButton1 IsNot Nothing Then lblButton1.Enabled = enabledButtons
        If lblButton2 IsNot Nothing Then lblButton2.Enabled = enabledButtons
    End Sub

    Private Sub chkQuickReplies_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkQuickReplies.CheckedChanged
        UpdateQuickReplyControls()
    End Sub

    Private Sub ApplyFormatDefaults()
        If LoadingTemplateSelection Then Exit Sub
        If cbTemplateFormat.Text.Trim = "Regional" Then
            cbLanguage.Text = "hi"
        Else
            cbLanguage.Text = "en_US"
        End If

        If CurrentLocalTypeCode() = "print_bill" Then
            If cbHeaderType.Items.Contains("document") Then cbHeaderType.Text = "document"
            If cbCategory.Items.Contains("UTILITY") Then cbCategory.Text = "UTILITY"
            If txtBody.Text.Trim = "" Then
                If CurrentLanguageCode() = "hi" Then
                    txtBody.Text = "नमस्ते *{{1}}*, आपका बिल दिनांक *{{2}}* फर्म *{{3}}* तैयार है।" & vbCrLf & "बिल की कुल रकम *{{4}}* है।" & vbCrLf & "धन्यवाद"
                Else
                    txtBody.Text = "Hello *{{1}}*, your bill dated *{{2}}* from *{{3}}* is ready for your records. Total amount is *{{4}}*. Thank you."
                End If
            End If
        End If
        If chkQuickReplies IsNot Nothing AndAlso HasDefaultQuickReplyText() Then SetDefaultQuickReplyButtons()
    End Sub

    Private Sub LocalTypeOrFormatChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtTemplateType.SelectedIndexChanged, cbTemplateFormat.SelectedIndexChanged
        LoadParameterFieldCombo()
        ApplyFormatDefaults()
    End Sub

    Private Function MetaLanguageCode() As String
        If cbLanguage.Text.Trim.ToLower() = "en" Then Return "en_US"
        Return cbLanguage.Text.Trim
    End Function



    Private Sub UpdateHeaderMediaVisibility()
        If cbHeaderType.SelectedIndex <> 0 Then
            lblMedia.Show()
            txtMediaFile.Show()
            btnSelectMedia.Show()
            lblMedia.Visible = True
            txtMediaFile.Visible = True
            btnSelectMedia.Visible = True
            lblMedia.BringToFront()
            txtMediaFile.BringToFront()
            btnSelectMedia.BringToFront()
            If btnUploadMedia IsNot Nothing Then btnUploadMedia.Hide()
        Else
            lblMedia.Show()
            txtMediaFile.Show()
            btnSelectMedia.Show()
            lblMedia.Visible = False
            txtMediaFile.Visible = False
            btnSelectMedia.Visible = False
            lblMedia.BringToFront()
            txtMediaFile.BringToFront()
            btnSelectMedia.BringToFront()
            If btnUploadMedia IsNot Nothing Then btnUploadMedia.Hide()
        End If

    End Sub

    Private Sub cbHeaderType_Changed(ByVal sender As Object, ByVal e As EventArgs) Handles cbHeaderType.SelectedIndexChanged, cbHeaderType.SelectionChangeCommitted, cbHeaderType.TextChanged, cbHeaderType.Validated, cbHeaderType.DropDownClosed, cbHeaderType.Leave
        If LoadingTemplateSelection = False AndAlso HeaderNeedsSample() = False Then
            SelectedMediaPath = ""
            UploadedMediaFileName = ""
            If txtMediaFile IsNot Nothing Then txtMediaFile.Text = ""
        End If
        UpdateHeaderMediaVisibility()
    End Sub
    Private Function HeaderNeedsSample() As Boolean
        Dim header As String = cbHeaderType.Text.Trim.ToLower()
        Return header = "document" OrElse header = "image" OrElse header = "video"
    End Function

    Private Function EnsureSampleUploaded() As Boolean
        If HeaderNeedsSample() = False Then Return True
        If UploadedMediaFileName.Trim <> "" Then Return True
        If SelectedMediaPath.Trim = "" Then
            If cbHeaderType.Text.Trim.ToLower() = "document" Then
                txtMediaFile.Text = "Default sample PDF will be used"
                lblStatus.Text = "Default sample PDF will be used from https://pdfobject.com/pdf/sample.pdf"
                Return True
            End If
            MsgBox("Please select a sample file for document/image/video header templates.", MsgBoxStyle.Critical, "Template Editor")
            Return False
        End If
        If VendorUid.Trim = "" OrElse AccessToken.Trim = "" Then
            MsgBox("Vendor ID / Access Token is missing. Please save or validate it on the WhatsApp API screen first.", MsgBoxStyle.Critical, "Template Editor")
            Return False
        End If
        Dim responseMessage As String = ""
        lblStatus.Text = "Uploading sample file..."
        Application.DoEvents()
        If WhatsAppOfficialApi.UploadTemplateSample(VendorUid, AccessToken, cbHeaderType.Text.Trim, SelectedMediaPath, UploadedMediaFileName, responseMessage) Then
            txtMediaFile.Text = System.IO.Path.GetFileName(SelectedMediaPath) & " - Uploaded"
            lblStatus.Text = "Uploaded: " & UploadedMediaFileName
            Return True
        End If
        MsgBox(responseMessage, MsgBoxStyle.Critical, "Template Editor")
        lblStatus.Text = responseMessage
        Return False
    End Function

    Private Sub btnSelectMedia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectMedia.Click
        Dim ofd As New OpenFileDialog()
        ofd.Title = "Select sample document/image/video"
        ofd.Filter = "Supported Files|*.pdf;*.doc;*.docx;*.xls;*.xlsx;*.txt;*.jpg;*.jpeg;*.png;*.mp4;*.3gp|All Files|*.*"
        If ofd.ShowDialog(Me) = DialogResult.OK Then
            SelectedMediaPath = ofd.FileName
            UploadedMediaFileName = ""
            txtMediaFile.Text = System.IO.Path.GetFileName(SelectedMediaPath) & " - Selected"
            If HeaderNeedsSample() Then
                If MsgBox("Do you want to upload this sample file now?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Template Editor") = MsgBoxResult.Yes Then
                    EnsureSampleUploaded()
                Else
                    lblStatus.Text = "Sample file selected. Upload will be required before submitting to Meta."
                End If
            End If
        End If
    End Sub

    Private Sub btnUploadMedia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUploadMedia.Click
        EnsureSampleUploaded()
    End Sub



    Private Sub TemplateEditor_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtTemplateCode.KeyDown, txtTemplateTitle.KeyDown, cbLanguage.KeyDown, cbCategory.KeyDown, cbHeaderType.KeyDown, txtTemplateType.KeyDown, cbTemplateFormat.KeyDown, txtMediaFile.KeyDown, txtBody.KeyDown, txtFooter.KeyDown, txtExamples.KeyDown, txtButton1.KeyDown, txtButton2.KeyDown, dgvTemplates.KeyDown
        If e.KeyCode = Keys.Enter Then
            If sender Is txtBody OrElse sender Is txtFooter Then
                Return
            End If
            e.SuppressKeyPress = True
            If sender Is dgvTemplates Then
                txtTemplateCode.Focus()
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Private Function ValidateTemplateFields() As Boolean
        If txtTemplateCode.Text.Trim = "" Then MsgBox("Template code/name is required.", MsgBoxStyle.Critical, "Template Editor") : txtTemplateCode.Focus() : Return False
        If txtTemplateTitle.Text.Trim = "" Then MsgBox("Template title is required.", MsgBoxStyle.Critical, "Template Editor") : txtTemplateTitle.Focus() : Return False
        If cbLanguage.Text.Trim = "" Then MsgBox("Language is required.", MsgBoxStyle.Critical, "Template Editor") : cbLanguage.Focus() : Return False
        If cbCategory.Text.Trim = "" Then MsgBox("Category is required.", MsgBoxStyle.Critical, "Template Editor") : cbCategory.Focus() : Return False
        If cbHeaderType.Text.Trim = "" Then MsgBox("Header is required. Please select None, Text, Document, Image, or Video.", MsgBoxStyle.Critical, "Template Editor") : cbHeaderType.Focus() : Return False
        If txtTemplateType.Text.Trim = "" Then MsgBox("Local type is required.", MsgBoxStyle.Critical, "Template Editor") : txtTemplateType.Focus() : Return False
        If txtBody.Text.Trim = "" Then MsgBox("Message body is required.", MsgBoxStyle.Critical, "Template Editor") : txtBody.Focus() : Return False
        If chkQuickReplies IsNot Nothing AndAlso chkQuickReplies.Checked Then
            Dim button1 As String = txtButton1.Text.Trim()
            Dim button2 As String = txtButton2.Text.Trim()
            If button1 = "" Then MsgBox("Quick reply button 1 is required.", MsgBoxStyle.Critical, "Template Editor") : txtButton1.Focus() : Return False
            If button2 = "" Then MsgBox("Quick reply button 2 is required.", MsgBoxStyle.Critical, "Template Editor") : txtButton2.Focus() : Return False
            If button1 = button2 Then MsgBox("Quick reply button labels must be different.", MsgBoxStyle.Critical, "Template Editor") : txtButton2.Focus() : Return False
            If button1.Length > 25 OrElse button2.Length > 25 Then MsgBox("Quick reply button text should be 25 characters or less.", MsgBoxStyle.Critical, "Template Editor") : Return False
        End If

        BuildSampleInputs()
        For Each n As Integer In UsedParameters()
            If SampleTextBoxes.ContainsKey(n) = False OrElse SampleTextBoxes(n).Text.Trim = "" Then
                MsgBox("Sample value is required for {{" & n.ToString() & "}}.", MsgBoxStyle.Critical, "Template Editor")
                If SampleTextBoxes.ContainsKey(n) Then SampleTextBoxes(n).Focus()
                Return False
            End If
        Next

        If HeaderNeedsSample() AndAlso UploadedMediaFileName.Trim = "" AndAlso SelectedMediaPath.Trim = "" AndAlso cbHeaderType.Text.Trim.ToLower() <> "document" Then
            MsgBox("Please select and upload a sample file for this header type.", MsgBoxStyle.Critical, "Template Editor")
            btnSelectMedia.Focus()
            Return False
        End If
        Return True
    End Function


    Private Function NextMissingParameterNumber() As Integer
        Dim used As List(Of Integer) = UsedParameters()
        For i As Integer = 1 To 99
            If used.Contains(i) = False Then Return i
        Next
        Return used.Count + 1
    End Function

    Private Sub AddNextParameter()
        Dim n As Integer = NextMissingParameterNumber()
        Dim selectedFieldKey As String = SelectedParameterFieldKey()
        If selectedFieldKey = "" Then
            MsgBox("Please select a parameter field first.", MsgBoxStyle.Critical, "Template Editor")
            If cbParameterField IsNot Nothing Then cbParameterField.Focus()
            Exit Sub
        End If
        While TemplateParameterFields.Count < n
            TemplateParameterFields.Add("")
        End While
        TemplateParameterFields(n - 1) = selectedFieldKey
        InsertParameter("*{{" & n.ToString() & "}}*")
    End Sub

    Private Sub InsertParameter(ByVal parameterText As String)
        Dim target As TextBox = If(LastTemplateTextBox IsNot Nothing, LastTemplateTextBox, txtBody)
        If target IsNot txtBody AndAlso target IsNot txtFooter Then target = txtBody

        Dim plainParameterText As String = parameterText.Replace("*", "")
        Dim alreadyUsed As Boolean = (txtBody.Text.Contains(plainParameterText) OrElse txtFooter.Text.Contains(plainParameterText))
        If alreadyUsed Then
            FocusSampleValue(plainParameterText)
            lblStatus.Text = plainParameterText & " already exists. Enter or edit its sample value."
            Exit Sub
        End If

        Dim insertAt As Integer = LastTemplateSelectionStart
        If insertAt < 0 OrElse insertAt > target.TextLength Then insertAt = target.SelectionStart
        If insertAt = target.TextLength AndAlso target.TextLength > 0 Then
            MsgBox("Meta does not allow variables at the very end. Please place the cursor before the final text, then add the variable.", MsgBoxStyle.Critical, "Template Editor")
            target.Focus()
            Exit Sub
        End If

        target.Focus()
        target.SelectionStart = insertAt
        target.SelectedText = parameterText
        LastTemplateTextBox = target
        LastTemplateSelectionStart = target.SelectionStart
        BuildSampleInputs()
    End Sub

    Private Sub FocusSampleValue(ByVal parameterText As String)
        Dim numberText As String = parameterText.Replace("{{", "").Replace("}}", "")
        Dim n As Integer = Val(numberText)
        If SampleTextBoxes.ContainsKey(n) Then
            SampleTextBoxes(n).Focus()
            SampleTextBoxes(n).SelectAll()
        End If
    End Sub

    Private Sub txtBody_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtBody.TextChanged
        BuildSampleInputs()
    End Sub

    Private Sub txtFooter_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFooter.TextChanged
        BuildSampleInputs()
    End Sub

    Private Sub btnP1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnP1.Click
        AddNextParameter()
    End Sub

    Private Sub cbParameterField_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles cbParameterField.DoubleClick
        AddNextParameter()
    End Sub

    Private Sub cbParameterField_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles cbParameterField.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            AddNextParameter()
        End If
    End Sub
    Private Sub btnP2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnP2.Click
        InsertParameter("*{{2}}*")
    End Sub
    Private Sub btnP3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnP3.Click
        InsertParameter("*{{3}}*")
    End Sub
    Private Sub btnP4_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnP4.Click
        InsertParameter("*{{4}}*")
    End Sub
    Private Sub btnP5_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnP5.Click
        InsertParameter("*{{5}}*")
    End Sub
    Private Sub btnP6_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnP6.Click
        InsertParameter("*{{6}}*")
    End Sub
End Class







