Imports System.Data.SQLite
Imports System.IO
Imports System.Globalization
Imports System.Drawing.Printing
Imports System.Text.RegularExpressions

Public Class ReportCompany
    Public Name As String
    Public DatabasePath As String
    Public YearStart As Date
    Public YearEnd As Date
    Public Overrides Function ToString() As String
        Return Name & "  |  " & YearStart.ToString("dd-MM-yyyy") & " to " & YearEnd.ToString("dd-MM-yyyy") & "  |  " & IO.Path.GetFileName(IO.Path.GetDirectoryName(DatabasePath))
    End Function
End Class

' Uses separate read-only connections; never changes the active company connection.
Public Class MultiCompanyReportData
    Private Shared Function CleanText(ByVal value As Object) As String
        If value Is Nothing OrElse value Is DBNull.Value Then Return ""
        Return Regex.Replace(Convert.ToString(value).Trim(), "\s+", " ")
    End Function

    Private Shared Function SafeDecimal(ByVal value As Object) As Decimal
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0D
        Dim text As String = Convert.ToString(value).Trim()
        If text = "" Then Return 0D
        text = text.Replace(",", "")
        Dim amount As Decimal
        If Decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, amount) Then Return amount
        If Decimal.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, amount) Then Return amount
        Return 0D
    End Function

    Private Shared Function SafeLong(ByVal value As Object) As Long
        If value Is Nothing OrElse value Is DBNull.Value Then Return 0L
        Dim text As String = Convert.ToString(value).Trim()
        If text = "" Then Return 0L
        Dim number As Long
        If Long.TryParse(text, number) Then Return number
        Return CLng(Val(text))
    End Function

    Public Shared Function OpenCompany(ByVal path As String) As SQLiteConnection
        Dim builder As New SQLiteConnectionStringBuilder()
        builder.DataSource = path
        builder.ReadOnly = True
        builder.FailIfMissing = True
        Dim connection As New SQLiteConnection(builder.ConnectionString)
        connection.Open()
        Return connection
    End Function

    Public Shared Function ReadCompany(ByVal path As String) As ReportCompany
        Using connection As SQLiteConnection = OpenCompany(path)
            Using command As New SQLiteCommand("SELECT CompanyName,YearStart,YearEnd FROM Company LIMIT 1", connection)
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    If Not reader.Read() Then Throw New ApplicationException("Company details are missing: " & path)
                    Dim result As New ReportCompany()
                    result.Name = CleanText(reader("CompanyName"))
                    result.DatabasePath = IO.Path.GetFullPath(path)
                    result.YearStart = Convert.ToDateTime(reader("YearStart"))
                    result.YearEnd = Convert.ToDateTime(reader("YearEnd"))
                    Return result
                End Using
            End Using
        End Using
    End Function

    Public Shared Function AccountKey(ByVal name As String) As String
        Return CleanText(name).ToUpperInvariant()
    End Function

    Private Shared Function Accounts(ByVal connection As SQLiteConnection) As DataTable
        Dim result As New DataTable()
        Using adapter As New SQLiteDataAdapter("SELECT ID,AccountName,CAST(ifnull(OpBal,0) AS TEXT) AS OpBal,DC FROM Accounts ORDER BY AccountName,ID", connection)
            adapter.Fill(result)
        End Using
        Return result
    End Function

    Public Shared Function Names(ByVal companies As List(Of ReportCompany)) As List(Of String)
        Dim found As New SortedDictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        For Each company As ReportCompany In companies
            Using connection As SQLiteConnection = OpenCompany(company.DatabasePath)
                For Each account As DataRow In Accounts(connection).Rows
                    Dim name As String = CleanText(account("AccountName"))
                    If name <> "" AndAlso Not found.ContainsKey(AccountKey(name)) Then found.Add(AccountKey(name), name)
                Next
            End Using
        Next
        Return New List(Of String)(found.Values)
    End Function

    Private Shared Function Opening(ByVal account As DataRow) As Decimal
        Dim amount As Decimal = SafeDecimal(account("OpBal"))
        Dim dc As String = CleanText(account("DC"))
        Return If(String.Equals(dc, "Dr", StringComparison.OrdinalIgnoreCase) OrElse String.Equals(dc, "D", StringComparison.OrdinalIgnoreCase), amount, -amount)
    End Function

    Private Shared Function Movements(ByVal connection As SQLiteConnection, ByVal accountID As Object, ByVal fromDate As Date, ByVal toDate As Date) As DataTable
        Dim result As New DataTable()
        Using command As New SQLiteCommand("SELECT EntryDate,TransType,VourchersID,ifnull(Amount,0) AS Amount,DC,ifnull(Remark,'') AS Remark,ifnull(Narration,'') AS Narration FROM Ledger WHERE AccountID=@id AND date(EntryDate)>=@start AND date(EntryDate)<=@finish ORDER BY date(EntryDate),VourchersID", connection)
            command.Parameters.AddWithValue("@id", accountID)
            command.Parameters.AddWithValue("@start", fromDate.ToString("yyyy-MM-dd"))
            command.Parameters.AddWithValue("@finish", toDate.ToString("yyyy-MM-dd"))
            Using adapter As New SQLiteDataAdapter(command)
                adapter.Fill(result)
            End Using
        End Using
        Return result
    End Function

    Private Shared Function SignedAmount(ByVal row As DataRow) As Decimal
        Dim dc As String = CleanText(row("DC")).ToUpperInvariant()
        If dc <> "D" AndAlso dc <> "C" Then Throw New ApplicationException("Invalid Ledger DC for voucher " & Convert.ToString(row("VourchersID")))
        Return SafeDecimal(row("Amount")) * If(dc = "D", 1D, -1D)
    End Function

    Public Shared Function BalanceText(ByVal balance As Decimal) As String
        Return Math.Abs(balance).ToString("N2") & If(balance < 0, " Cr", " Dr")
    End Function

    Public Shared Function Ledger(ByVal companies As List(Of ReportCompany), ByVal name As String, ByVal fromDate As Date, ByVal toDate As Date) As DataTable
        Dim raw As New DataTable()
        raw.Columns.Add("Date", GetType(Date))
        raw.Columns.Add("Company", GetType(String))
        raw.Columns.Add("Transaction", GetType(String))
        raw.Columns.Add("Voucher", GetType(Long))
        raw.Columns.Add("Particulars", GetType(String))
        raw.Columns.Add("Debit", GetType(Decimal))
        raw.Columns.Add("Credit", GetType(Decimal))
        raw.Columns.Add("Balance", GetType(String))
        raw.Columns.Add("Order", GetType(Integer))
        For Each company As ReportCompany In companies
            If company.YearStart.Date > toDate.Date Then Continue For
            Using connection As SQLiteConnection = OpenCompany(company.DatabasePath)
                For Each account As DataRow In Accounts(connection).Rows
                    If AccountKey(CleanText(account("AccountName"))) <> AccountKey(name) Then Continue For
                    Dim balance As Decimal = Opening(account)
                    Dim transactions As DataTable = Movements(connection, account("ID"), company.YearStart.Date, toDate.Date)
                    For Each entry As DataRow In transactions.Rows
                        If Convert.ToDateTime(entry("EntryDate")).Date < fromDate.Date Then balance += SignedAmount(entry)
                    Next
                    Dim openingDate As Date = If(company.YearStart.Date > fromDate.Date, company.YearStart.Date, fromDate.Date)
                    raw.Rows.Add(openingDate, company.Name, "Opening Balance", 0L, "Account: " & CleanText(account("AccountName")), Math.Max(balance, 0D), Math.Max(-balance, 0D), "", 0)
                    For Each entry As DataRow In transactions.Rows
                        Dim dateValue As Date = Convert.ToDateTime(entry("EntryDate")).Date
                        If dateValue < fromDate.Date Then Continue For
                        Dim signed As Decimal = SignedAmount(entry)
                        raw.Rows.Add(dateValue, company.Name, CleanText(entry("TransType")), SafeLong(entry("VourchersID")), (CleanText(entry("Remark")) & " " & CleanText(entry("Narration"))).Trim(), Math.Max(signed, 0D), Math.Max(-signed, 0D), "", 1)
                    Next
                Next
            End Using
        Next
        raw.DefaultView.Sort = "Date,Order,Company,Voucher"
        Dim result As DataTable = raw.DefaultView.ToTable()
        Dim running As Decimal = 0D
        Dim debit As Decimal = 0D
        Dim credit As Decimal = 0D
        For Each row As DataRow In result.Rows
            debit += Convert.ToDecimal(row("Debit"))
            credit += Convert.ToDecimal(row("Credit"))
            running = debit - credit
            row("Balance") = BalanceText(running)
        Next
        result.Columns.Remove("Order")
        result.Rows.Add(DBNull.Value, "ALL SELECTED COMPANIES", "TOTAL / CLOSING", DBNull.Value, "Includes opening balances", debit, credit, BalanceText(running))
        Return result
    End Function

    Public Shared Function Outstanding(ByVal companies As List(Of ReportCompany), ByVal name As String, ByVal asOf As Date) As DataTable
        Dim result As New DataTable()
        result.Columns.Add("Account", GetType(String))
        result.Columns.Add("Company", GetType(String))
        result.Columns.Add("Debit", GetType(Decimal))
        result.Columns.Add("Credit", GetType(Decimal))
        result.Columns.Add("Balance", GetType(String))
        Dim details As New SortedDictionary(Of String, List(Of Object()))(StringComparer.OrdinalIgnoreCase)
        For Each company As ReportCompany In companies
            If company.YearStart.Date > asOf.Date Then Continue For
            Using connection As SQLiteConnection = OpenCompany(company.DatabasePath)
                Dim table As DataTable = Accounts(connection)
                ' Aggregate in one query per database, rather than one query per account.
                Dim balances As New Dictionary(Of Long, Decimal)()
                Using command As New SQLiteCommand("SELECT AccountID,SUM(CASE WHEN DC='D' THEN ifnull(Amount,0) WHEN DC='C' THEN -ifnull(Amount,0) ELSE 0 END) AS Net FROM Ledger WHERE date(EntryDate)>=@start AND date(EntryDate)<=@finish GROUP BY AccountID", connection)
                    command.Parameters.AddWithValue("@start", company.YearStart.ToString("yyyy-MM-dd"))
                    command.Parameters.AddWithValue("@finish", asOf.ToString("yyyy-MM-dd"))
                    Using reader As SQLiteDataReader = command.ExecuteReader()
                        While reader.Read()
                            balances(SafeLong(reader("AccountID"))) = SafeDecimal(reader("Net"))
                        End While
                    End Using
                End Using
                Dim companyTotals As New SortedDictionary(Of String, Decimal)(StringComparer.OrdinalIgnoreCase)
                For Each account As DataRow In table.Rows
                    Dim key As String = AccountKey(CleanText(account("AccountName")))
                    If name <> "" AndAlso key <> AccountKey(name) Then Continue For
                    Dim amount As Decimal = Opening(account)
                    Dim id As Long = SafeLong(account("ID"))
                    If balances.ContainsKey(id) Then amount += balances(id)
                    If Not companyTotals.ContainsKey(key) Then companyTotals.Add(key, 0D)
                    companyTotals(key) += amount
                Next
                For Each pair As KeyValuePair(Of String, Decimal) In companyTotals
                    If Not details.ContainsKey(pair.Key) Then details.Add(pair.Key, New List(Of Object())())
                    details(pair.Key).Add(New Object() {company.Name, pair.Value})
                Next
            End Using
        Next
        Dim totalDebit As Decimal = 0D
        Dim totalCredit As Decimal = 0D
        For Each group As KeyValuePair(Of String, List(Of Object())) In details
            Dim net As Decimal = 0D
            Dim hasBalance As Boolean = False
            For Each companyBalance As Object() In group.Value
                Dim amount As Decimal = Convert.ToDecimal(companyBalance(1))
                If Math.Round(amount, 2) = 0D Then Continue For
                hasBalance = True
                result.Rows.Add(group.Key, companyBalance(0), Math.Max(amount, 0D), Math.Max(-amount, 0D), BalanceText(amount))
                net += amount
            Next
            If Not hasBalance Then Continue For
            result.Rows.Add(group.Key, "COMBINED TOTAL", Math.Max(net, 0D), Math.Max(-net, 0D), BalanceText(net))
            totalDebit += Math.Max(net, 0D)
            totalCredit += Math.Max(-net, 0D)
        Next
        result.Rows.Add("GRAND TOTAL", "Combined account balances", totalDebit, totalCredit, BalanceText(totalDebit - totalCredit))
        Return result
    End Function
End Class

Public Class MultiCompanyReports
    Inherits Form
    Private companyList As New CheckedListBox()
    Private account As New ComboBox()
    Private allAccounts As New CheckBox()
    Private accountRefresh As New System.Windows.Forms.Timer()
    Private reportType As New ComboBox()
    Private fromDate As New DateTimePicker()
    Private toDate As New DateTimePicker()
    Private results As New DataGridView()
    Private status As New Label()
    Private previewButton As New Button()
    Private printButton As New Button()
    Private report As DataTable
    Private reportHeading As String
    Private reportSources As String
    Private printRow As Integer
    Private printPage As Integer
    Private workspace As MdiClient
    Private companyTip As New ToolTip()
    Private WithEvents document As New PrintDocument()

    Public Sub New()
        Text = "Multi Company Ledger / Outstanding"
        Size = New Size(1180, 640)
        MinimumSize = Size.Empty
        Font = New Font("Segoe UI", 9)
        ForeColor = Color.FromArgb(35, 45, 55)
        BackColor = Color.FromArgb(247, 220, 111)
        FormBorderStyle = FormBorderStyle.None
        StartPosition = FormStartPosition.Manual
        KeyPreview = True
        Dim layout As New TableLayoutPanel()
        layout.Dock = DockStyle.Fill
        layout.Padding = New Padding(10, 6, 10, 6)
        layout.ColumnCount = 1
        layout.RowCount = 6
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 38))
        layout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 115))
        layout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        layout.RowStyles.Add(New RowStyle(SizeType.Absolute, 40))
        Controls.Add(layout)
        Dim heading As New Panel() With {.Dock = DockStyle.Fill, .Margin = New Padding(0)}
        Dim closeButton As New Button() With {.Text = "Close  [Esc]", .Dock = DockStyle.Right, .Width = 105, .BackColor = Color.Maroon, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat}
        AddHandler closeButton.Click, AddressOf CloseReport
        heading.Controls.Add(closeButton)
        heading.Controls.Add(New Label() With {.Text = "MULTI COMPANY  |  LEDGER / OUTSTANDING", .Font = New Font("Segoe UI", 13, FontStyle.Bold), .AutoSize = True, .Location = New Point(0, 5)})
        layout.Controls.Add(heading, 0, 0)
        Dim companiesBar As New FlowLayoutPanel()
        companiesBar.Dock = DockStyle.Fill
        companiesBar.AutoSize = True
        companiesBar.Margin = New Padding(0, 3, 0, 3)
        AddButton(companiesBar, "Reload Companies", AddressOf ReloadCompanies)
        AddButton(companiesBar, "Add Data.db...", AddressOf AddDatabase)
        AddButton(companiesBar, "Select All", AddressOf SelectAllCompanies)
        AddButton(companiesBar, "Clear Selection", AddressOf ClearCompanies)
        AddButton(companiesBar, "Load Account Names", AddressOf LoadNames)
        layout.Controls.Add(companiesBar, 0, 1)
        companyList.Dock = DockStyle.Fill
        companyList.BackColor = Color.FromArgb(255, 253, 239)
        companyList.ForeColor = Color.FromArgb(35, 45, 55)
        companyList.IntegralHeight = False
        companyList.BorderStyle = BorderStyle.FixedSingle
        companyList.Margin = New Padding(0, 0, 0, 5)
        companyList.CheckOnClick = True
        companyList.HorizontalScrollbar = True
        layout.Controls.Add(companyList, 0, 2)
        Dim filters As New FlowLayoutPanel()
        filters.Dock = DockStyle.Fill
        filters.AutoSize = True
        filters.Padding = New Padding(5, 7, 5, 7)
        filters.Margin = New Padding(0, 0, 0, 6)
        filters.BackColor = Color.FromArgb(255, 244, 195)
        filters.Controls.Add(New Label() With {.Text = "Report", .AutoSize = True})
        reportType.DropDownStyle = ComboBoxStyle.DropDownList
        reportType.Items.AddRange(New Object() {"Ledger", "Outstanding"})
        reportType.SelectedIndex = 0
        filters.Controls.Add(reportType)
        filters.Controls.Add(New Label() With {.Text = "Account", .AutoSize = True})
        account.Width = 220
        account.DropDownStyle = ComboBoxStyle.DropDown
        account.MaxDropDownItems = 15
        account.IntegralHeight = False
        account.DropDownHeight = 280
        account.DropDownWidth = 360
        account.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        account.AutoCompleteSource = AutoCompleteSource.ListItems
        filters.Controls.Add(account)
        allAccounts.Text = "All accounts"
        allAccounts.AutoSize = True
        allAccounts.Enabled = False
        allAccounts.Margin = New Padding(3, 6, 6, 0)
        filters.Controls.Add(allAccounts)
        filters.Controls.Add(New Label() With {.Text = "From", .AutoSize = True})
        fromDate.Format = DateTimePickerFormat.Custom
        fromDate.CustomFormat = "dd-MM-yyyy"
        fromDate.Width = 115
        fromDate.Value = New Date(Date.Today.Year - If(Date.Today.Month < 4, 1, 0), 4, 1)
        filters.Controls.Add(fromDate)
        filters.Controls.Add(New Label() With {.Text = "To / As of", .AutoSize = True})
        toDate.Format = DateTimePickerFormat.Custom
        toDate.CustomFormat = "dd-MM-yyyy"
        toDate.Width = 115
        filters.Controls.Add(toDate)
        AddButton(filters, "Generate", AddressOf GenerateReport)
        previewButton = AddButton(filters, "Print Preview", AddressOf PreviewReport)
        printButton = AddButton(filters, "Print...", AddressOf PrintReport)
        previewButton.Enabled = False
        printButton.Enabled = False
        For Each control As Control In filters.Controls
            If TypeOf control Is Label Then control.Margin = New Padding(3, 7, 3, 0)
        Next
        layout.Controls.Add(filters, 0, 3)
        results.Dock = DockStyle.Fill
        results.ReadOnly = True
        results.AllowUserToAddRows = False
        results.AllowUserToDeleteRows = False
        results.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        results.RowHeadersVisible = False
        results.BackgroundColor = Color.White
        results.BorderStyle = BorderStyle.FixedSingle
        results.GridColor = Color.FromArgb(220, 220, 210)
        results.EnableHeadersVisualStyles = False
        results.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(90, 78, 36)
        results.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        results.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        results.ColumnHeadersHeight = 30
        results.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        results.RowTemplate.Height = 25
        results.DefaultCellStyle.ForeColor = Color.FromArgb(35, 45, 55)
        results.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 226, 140)
        results.DefaultCellStyle.SelectionForeColor = Color.Black
        results.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 249, 241)
        results.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        results.Margin = New Padding(0)
        layout.Controls.Add(results, 0, 4)
        status.Dock = DockStyle.Fill
        status.Padding = New Padding(4, 5, 4, 0)
        status.ForeColor = Color.FromArgb(65, 55, 25)
        status.Text = "Select companies; account names load automatically. Select an account, or tick All accounts for Outstanding."
        layout.Controls.Add(status, 0, 5)
        AddHandler Load, AddressOf AttachWorkspace
        AddHandler KeyDown, AddressOf ReportKeyDown
        AddHandler companyList.SelectedIndexChanged, AddressOf CompanyTooltip
        AddHandler results.CellFormatting, AddressOf FormatReportRow
        AddHandler Shown, AddressOf ReloadCompanies
        AddHandler companyList.ItemCheck, AddressOf CompanySelectionChanged
        AddHandler account.TextChanged, AddressOf FilterChanged
        AddHandler account.Enter, AddressOf EnsureAccountNames
        AddHandler account.DropDown, AddressOf EnsureAccountNames
        AddHandler allAccounts.CheckedChanged, AddressOf AccountScopeChanged
        accountRefresh.Interval = 200
        AddHandler accountRefresh.Tick, AddressOf RefreshAccountNames
        AddHandler fromDate.ValueChanged, AddressOf FilterChanged
        AddHandler toDate.ValueChanged, AddressOf FilterChanged
        AddHandler reportType.SelectedIndexChanged, AddressOf FilterChanged
        document.DefaultPageSettings.Landscape = True
        document.DefaultPageSettings.Margins = New Margins(35, 35, 35, 35)
    End Sub

    Private Function AddButton(ByVal panel As FlowLayoutPanel, ByVal caption As String, ByVal handler As EventHandler) As Button
        Dim button As New Button()
        button.Text = caption
        button.AutoSize = True
        button.Padding = New Padding(5, 3, 5, 3)
        button.FlatStyle = FlatStyle.Flat
        button.FlatAppearance.BorderColor = Color.FromArgb(190, 170, 100)
        button.BackColor = Color.FromArgb(255, 252, 235)
        button.ForeColor = Color.FromArgb(45, 45, 35)
        button.Cursor = Cursors.Hand
        If caption = "Generate" Then
            button.BackColor = Color.FromArgb(46, 110, 72)
            button.ForeColor = Color.White
        End If
        AddHandler button.Click, handler
        panel.Controls.Add(button)
        Return button
    End Function

    Private Sub AttachWorkspace(sender As Object, e As EventArgs)
        If MdiParent IsNot Nothing Then
            For Each control As Control In MdiParent.Controls
                If TypeOf control Is MdiClient Then
                    workspace = DirectCast(control, MdiClient)
                    AddHandler workspace.ClientSizeChanged, AddressOf FitWorkspace
                    Exit For
                End If
            Next
        End If
        FitWorkspace(sender, e)
    End Sub

    Private Sub FitWorkspace(sender As Object, e As EventArgs)
        Dim available As Size = If(workspace IsNot Nothing, workspace.ClientSize, Screen.FromControl(Me).WorkingArea.Size)
        Bounds = New Rectangle(0, 0, Math.Min(1180, Math.Max(1, available.Width - 6)), Math.Max(1, available.Height - 6))
    End Sub

    Private Sub CloseReport(sender As Object, e As EventArgs)
        Close()
    End Sub

    Private Sub ReportKeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            e.SuppressKeyPress = True
            Close()
        End If
    End Sub

    Private Sub CompanyTooltip(sender As Object, e As EventArgs)
        Dim selected As ReportCompany = TryCast(companyList.SelectedItem, ReportCompany)
        companyTip.SetToolTip(companyList, If(selected Is Nothing, "", selected.DatabasePath))
    End Sub

    Private Sub FormatReportRow(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse Not results.Columns.Contains("Company") Then Return
        Dim company As String = Convert.ToString(results.Rows(e.RowIndex).Cells("Company").Value)
        If company = "COMBINED TOTAL" OrElse company = "ALL SELECTED COMPANIES" OrElse company = "Combined account balances" Then
            e.CellStyle.BackColor = Color.FromArgb(249, 231, 159)
            e.CellStyle.ForeColor = Color.FromArgb(65, 49, 10)
        End If
    End Sub

    Private Sub FilterChanged(sender As Object, e As EventArgs)
        report = Nothing
        results.DataSource = Nothing
        previewButton.Enabled = False
        printButton.Enabled = False
        fromDate.Enabled = reportType.SelectedIndex = 0
        allAccounts.Enabled = reportType.SelectedIndex = 1
        account.Enabled = Not (reportType.SelectedIndex = 1 AndAlso allAccounts.Checked)
        status.Text = "Select an account name for a combined report. For Outstanding, tick All accounts to include everyone."
    End Sub

    Private Sub CompanySelectionChanged(sender As Object, e As ItemCheckEventArgs)
        account.Items.Clear()
        FilterChanged(sender, EventArgs.Empty)
        ' ItemCheck fires before CheckedItems changes; defer loading until the selection is committed.
        accountRefresh.Stop()
        accountRefresh.Start()
    End Sub

    Private Sub RefreshAccountNames(sender As Object, e As EventArgs)
        accountRefresh.Stop()
        LoadNames(sender, e)
    End Sub

    Private Sub EnsureAccountNames(sender As Object, e As EventArgs)
        If accountRefresh.Enabled OrElse account.Items.Count = 0 Then RefreshAccountNames(sender, e)
    End Sub

    Private Sub AccountScopeChanged(sender As Object, e As EventArgs)
        FilterChanged(sender, e)
    End Sub

    Private Function SelectedCompanies() As List(Of ReportCompany)
        Dim selected As New List(Of ReportCompany)()
        For Each item As Object In companyList.CheckedItems
            selected.Add(DirectCast(item, ReportCompany))
        Next
        If selected.Count = 0 Then Throw New ApplicationException("Please select at least one company.")
        Return selected
    End Function

    Private Sub AddCompany(ByVal path As String, ByVal selected As Boolean)
        For Each item As Object In companyList.Items
            If String.Equals(DirectCast(item, ReportCompany).DatabasePath, IO.Path.GetFullPath(path), StringComparison.OrdinalIgnoreCase) Then Return
        Next
        companyList.Items.Add(MultiCompanyReportData.ReadCompany(path), selected)
    End Sub

    Private Sub ReloadCompanies(sender As Object, e As EventArgs)
        Try
            FilterChanged(sender, e)
            companyList.Items.Clear()
            account.Items.Clear()
            Dim root As String = ClsFunPrimary.ExecScalarStr("Select DefaultPath From Path")
            If String.IsNullOrEmpty(root) OrElse String.Equals(root, "Data", StringComparison.OrdinalIgnoreCase) Then root = IO.Path.Combine(Application.StartupPath, "Data")
            If Not IO.Path.IsPathRooted(root) Then root = IO.Path.Combine(Application.StartupPath, root)
            Dim failures As New List(Of String)()
            If File.Exists(GlobalData.ConnectionPath) Then AddCompany(GlobalData.ConnectionPath, True)
            If Directory.Exists(root) Then
                For Each path As String In Directory.GetFiles(root, "Data.db", SearchOption.AllDirectories)
                    Try
                        AddCompany(path, False)
                    Catch ex As Exception
                        failures.Add(path & ": " & ex.Message)
                    End Try
                Next
            End If
            status.Text = companyList.Items.Count.ToString() & " companies found. Accounts load automatically. Use All accounts for full Outstanding. Select each company/year once."
            If failures.Count > 0 Then MessageBox.Show(String.Join(Environment.NewLine, failures.ToArray()), "Some companies could not be loaded")
        Catch ex As Exception
            MessageBox.Show(ex.Message, Text)
        End Try
    End Sub

    Private Sub AddDatabase(sender As Object, e As EventArgs)
        Using dialog As New OpenFileDialog()
            dialog.Filter = "Company database (*.db)|*.db"
            dialog.Multiselect = True
            If dialog.ShowDialog(Me) <> DialogResult.OK Then Return
            For Each path As String In dialog.FileNames
                Try
                    AddCompany(path, True)
                Catch ex As Exception
                    MessageBox.Show(path & Environment.NewLine & ex.Message, Text)
                End Try
            Next
        End Using
    End Sub

    Private Sub SelectAllCompanies(sender As Object, e As EventArgs)
        For i As Integer = 0 To companyList.Items.Count - 1
            companyList.SetItemChecked(i, True)
        Next
    End Sub

    Private Sub ClearCompanies(sender As Object, e As EventArgs)
        For i As Integer = 0 To companyList.Items.Count - 1
            companyList.SetItemChecked(i, False)
        Next
    End Sub

    Private Sub LoadNames(sender As Object, e As EventArgs)
        accountRefresh.Stop()
        Dim previousText As String = account.Text
        Try
            account.BeginUpdate()
            account.Items.Clear()
            If companyList.CheckedItems.Count = 0 Then
                status.Text = "Select at least one company to load account names."
                Return
            End If
            For Each name As String In MultiCompanyReportData.Names(SelectedCompanies())
                account.Items.Add(name)
            Next
            status.Text = account.Items.Count.ToString() & " account names ready. Type a name or use the dropdown. Same names from selected companies are combined."
        Catch ex As Exception
            MessageBox.Show(ex.Message, Text)
        Finally
            account.EndUpdate()
            account.Text = previousText
        End Try
    End Sub

    Private Sub GenerateReport(sender As Object, e As EventArgs)
        Try
            FilterChanged(sender, e)
            Dim companies As List(Of ReportCompany) = SelectedCompanies()
            Dim selectedName As String = If(reportType.SelectedIndex = 1 AndAlso allAccounts.Checked, "", account.Text.Trim())
            If selectedName = "" AndAlso Not (reportType.SelectedIndex = 1 AndAlso allAccounts.Checked) Then Throw New ApplicationException("Please select an account name. For all Outstanding accounts, tick All accounts.")
            If reportType.SelectedIndex = 0 AndAlso fromDate.Value.Date > toDate.Value.Date Then Throw New ApplicationException("From date must be before To date.")
            If selectedName <> "" Then
                Dim exists As Boolean = False
                For Each name As String In MultiCompanyReportData.Names(companies)
                    If MultiCompanyReportData.AccountKey(name) = MultiCompanyReportData.AccountKey(selectedName) Then exists = True
                Next
                If Not exists Then Throw New ApplicationException("Account name was not found in the selected companies.")
            End If
            ' Repeated company names often indicate copies or linked financial years.
            Dim seen As New Dictionary(Of String, Boolean)()
            For Each company As ReportCompany In companies
                Dim key As String = MultiCompanyReportData.AccountKey(company.Name)
                If seen.ContainsKey(key) Then Throw New ApplicationException("Multiple databases have company name '" & company.Name & "'. Select one database/year for that company to avoid counting opening balances twice.")
                seen.Add(key, True)
            Next
            Cursor = Cursors.WaitCursor
            If reportType.SelectedIndex = 0 Then
                report = MultiCompanyReportData.Ledger(companies, account.Text, fromDate.Value.Date, toDate.Value.Date)
                reportHeading = "Combined Ledger - " & account.Text & " | " & fromDate.Value.ToString("dd-MM-yyyy") & " to " & toDate.Value.ToString("dd-MM-yyyy")
            Else
                report = MultiCompanyReportData.Outstanding(companies, selectedName, toDate.Value.Date)
                reportHeading = "Combined Outstanding - " & If(selectedName = "", "All accounts", selectedName) & " | As of " & toDate.Value.ToString("dd-MM-yyyy")
            End If
            Dim sourceNames As New List(Of String)()
            For Each company As ReportCompany In companies
                sourceNames.Add(company.Name & " (" & company.YearStart.ToString("yyyy") & "-" & company.YearEnd.ToString("yyyy") & ")")
            Next
            reportSources = String.Join(", ", sourceNames.ToArray())
            results.DataSource = report
            For Each column As DataGridViewColumn In results.Columns
                column.SortMode = DataGridViewColumnSortMode.NotSortable
                If column.Name = "Date" Then column.DefaultCellStyle.Format = "dd-MM-yyyy"
                If column.Name = "Debit" OrElse column.Name = "Credit" Then
                    column.DefaultCellStyle.Format = "N2"
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next
            previewButton.Enabled = True
            printButton.Enabled = True
            status.Text = reportHeading & " | " & companies.Count.ToString() & " companies. Read-only report."
        Catch ex As Exception
            report = Nothing
            MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub PreviewReport(sender As Object, e As EventArgs)
        If report Is Nothing Then Return
        Try
            Using preview As New PrintPreviewDialog()
                preview.Document = document
                preview.Width = 1100
                preview.Height = 750
                preview.ShowDialog(Me)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Print Preview", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintReport(sender As Object, e As EventArgs)
        If report Is Nothing Then Return
        Try
            Using dialog As New PrintDialog()
                dialog.Document = document
                dialog.UseEXDialog = True
                If dialog.ShowDialog(Me) = DialogResult.OK Then document.Print()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Print", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BeginReportPrint(sender As Object, e As PrintEventArgs) Handles document.BeginPrint
        printRow = 0
        printPage = 0
        If report Is Nothing Then e.Cancel = True
    End Sub

    Private Function CellText(ByVal row As DataRow, ByVal column As DataColumn) As String
        If row.IsNull(column) Then Return ""
        If column.DataType Is GetType(Date) Then Return Convert.ToDateTime(row(column)).ToString("dd-MM-yyyy")
        If column.DataType Is GetType(Decimal) Then Return Convert.ToDecimal(row(column)).ToString("N2")
        Return Convert.ToString(row(column))
    End Function

    Private Sub PrintReportPage(sender As Object, e As PrintPageEventArgs) Handles document.PrintPage
        printPage += 1
        Dim widths As Single() = If(report.Columns.Count = 8, New Single() {0.09F, 0.17F, 0.13F, 0.06F, 0.24F, 0.1F, 0.1F, 0.11F}, New Single() {0.28F, 0.28F, 0.14F, 0.14F, 0.16F})
        Using font As New Font("Segoe UI", 8), title As New Font("Segoe UI", 11, FontStyle.Bold), bold As New Font("Segoe UI", 8, FontStyle.Bold)
            Dim y As Single = e.MarginBounds.Top
            Dim headingHeight As Single = e.Graphics.MeasureString(reportHeading, title, e.MarginBounds.Width).Height + 5
            e.Graphics.DrawString(reportHeading, title, Brushes.Black, New RectangleF(e.MarginBounds.Left, y, e.MarginBounds.Width, headingHeight))
            y += headingHeight
            Dim sourcesHeight As Single = e.Graphics.MeasureString(reportSources, font, e.MarginBounds.Width).Height + 8
            e.Graphics.DrawString(reportSources, font, Brushes.Black, New RectangleF(e.MarginBounds.Left, y, e.MarginBounds.Width, sourcesHeight))
            y += sourcesHeight
            Dim x As Single = e.MarginBounds.Left
            For i As Integer = 0 To report.Columns.Count - 1
                Dim width As Single = e.MarginBounds.Width * widths(i)
                e.Graphics.FillRectangle(Brushes.LightGray, x, y, width, 24)
                e.Graphics.DrawString(report.Columns(i).ColumnName, bold, Brushes.Black, x + 3, y + 4)
                x += width
            Next
            y += 24
            Dim firstRow As Boolean = True
            While printRow < report.Rows.Count
                Dim row As DataRow = report.Rows(printRow)
                Dim height As Single = 24
                For i As Integer = 0 To report.Columns.Count - 1
                    height = Math.Max(height, e.Graphics.MeasureString(CellText(row, report.Columns(i)), font, CInt(e.MarginBounds.Width * widths(i) - 6)).Height + 8)
                Next
                If y + height > e.MarginBounds.Bottom - 25 AndAlso Not firstRow Then
                    e.HasMorePages = True
                    Exit While
                End If
                ' Refuse oversized rows rather than silently clipping report text.
                Dim availableHeight As Single = e.MarginBounds.Bottom - 25 - y
                If height > availableHeight Then Throw New ApplicationException("A report row is too tall for this paper size. Use larger paper or shorter remarks.")
                x = e.MarginBounds.Left
                For i As Integer = 0 To report.Columns.Count - 1
                    Dim width As Single = e.MarginBounds.Width * widths(i)
                    e.Graphics.DrawRectangle(Pens.LightGray, x, y, width, height)
                    e.Graphics.DrawString(CellText(row, report.Columns(i)), font, Brushes.Black, New RectangleF(x + 3, y + 4, width - 6, height - 6))
                    x += width
                Next
                y += height
                printRow += 1
                firstRow = False
            End While
            e.Graphics.DrawString("Page " & printPage.ToString() & " | " & Date.Now.ToString("dd-MM-yyyy HH:mm"), font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Bottom - 18)
        End Using
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            accountRefresh.Dispose()
            If workspace IsNot Nothing Then RemoveHandler workspace.ClientSizeChanged, AddressOf FitWorkspace
            companyTip.Dispose()
            document.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class
