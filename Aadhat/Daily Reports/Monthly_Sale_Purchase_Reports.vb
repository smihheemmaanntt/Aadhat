Imports System.Data
Imports System.Collections.Generic
Imports System.IO
Imports System.Drawing.Printing

Friend Class MonthlyReportPeriod
    Public Name As String
    Public StartDate As Date
    Public EndDate As Date

    Public Sub New(ByVal periodName As String, ByVal fromDate As Date, ByVal toDate As Date)
        Name = periodName
        StartDate = fromDate
        EndDate = toDate
    End Sub
End Class

Friend Class MonthlyReportData
    Public Const SaleMode As String = "Sale"
    Public Const PurchaseMode As String = "Purchase"
    Public Const BeejakMode As String = "Beejak"
    Public Const SalePurchaseMode As String = "Sale + Purchase"
    Public Const PurchaseBeejakMode As String = "Purchase + Beejak"

    Public Shared Function GetModes() As String()
        Return New String() {SaleMode, PurchaseMode, BeejakMode, SalePurchaseMode, PurchaseBeejakMode}
    End Function

    Public Shared Function GetFinancialMonths() As List(Of MonthlyReportPeriod)
        Dim result As New List(Of MonthlyReportPeriod)()
        Dim firstMonth As Date = New Date(FinYearStart.Year, FinYearStart.Month, 1)
        For monthOffset As Integer = 0 To 11
            Dim monthStart As Date = firstMonth.AddMonths(monthOffset)
            Dim monthEnd As Date = monthStart.AddMonths(1).AddDays(-1)
            result.Add(New MonthlyReportPeriod(monthStart.ToString("MMMM"), monthStart, monthEnd))
        Next
        Return result
    End Function

    Public Shared Function IncludesSale(ByVal mode As String) As Boolean
        Return mode = SaleMode OrElse mode = SalePurchaseMode
    End Function

    Public Shared Function IncludesPurchase(ByVal mode As String) As Boolean
        Return mode = PurchaseMode OrElse mode = SalePurchaseMode OrElse mode = PurchaseBeejakMode
    End Function

    Public Shared Function IncludesBeejak(ByVal mode As String) As Boolean
        Return mode = BeejakMode OrElse mode = PurchaseBeejakMode
    End Function

    Private Shared Function SqlDate(ByVal value As Date) As String
        Return value.ToString("yyyy-MM-dd")
    End Function

    Public Shared Function SaleTotal(ByVal fromDate As Date, ByVal toDate As Date) As Decimal
        Dim sql As String = _
            "Select IfNull(Sum(ReportAmount),0) From (" & _
            " Select IfNull(TotalAmount,0) As ReportAmount From Vouchers" & _
            " Where TransType In ('Super Sale','Stock Sale','Standard Sale','Sale (Loose)','Loose Sale')" & _
            " And EntryDate Between '" & SqlDate(fromDate) & "' And '" & SqlDate(toDate) & "'" & _
            " Union All" & _
            " Select IfNull(T.TotalAmount,0) From Transaction2 T" & _
            " Where T.TransType='Speed Sale' And T.EntryDate Between '" & SqlDate(fromDate) & "' And '" & SqlDate(toDate) & "')"
        Return CDec(Val(clsFun.ExecScalarStr(sql)))
    End Function

    Public Shared Function PurchaseTotal(ByVal fromDate As Date, ByVal toDate As Date) As Decimal
        Dim sql As String = "Select IfNull(Sum(TotalAmount),0) From Vouchers" & _
            " Where TransType In ('Purchase','Purchase (Loose)')" & _
            " And EntryDate Between '" & SqlDate(fromDate) & "' And '" & SqlDate(toDate) & "'"
        Return CDec(Val(clsFun.ExecScalarStr(sql)))
    End Function

    Public Shared Function BeejakTotal(ByVal fromDate As Date, ByVal toDate As Date) As Decimal
        Dim sql As String = "Select IfNull(Sum(TotalAmount),0) From Vouchers" & _
            " Where TransType In ('Beejak','Auto Beejak')" & _
            " And EntryDate Between '" & SqlDate(fromDate) & "' And '" & SqlDate(toDate) & "'"
        Return CDec(Val(clsFun.ExecScalarStr(sql)))
    End Function

    Public Shared Function AccountTotals(ByVal mode As String, ByVal fromDate As Date, ByVal toDate As Date) As DataTable
        Dim parts As New List(Of String)()
        Dim dateCondition As String = " Between '" & SqlDate(fromDate) & "' And '" & SqlDate(toDate) & "'"

        If IncludesSale(mode) Then
            parts.Add("Select AccountID,AccountName,IfNull(TotalAmount,0) SaleAmount,0 PurchaseAmount,0 BeejakAmount" & _
                      " From Vouchers Where TransType In ('Super Sale','Stock Sale','Standard Sale','Sale (Loose)','Loose Sale')" & _
                      " And EntryDate" & dateCondition)
            parts.Add("Select T.AccountID,T.AccountName,IfNull(T.TotalAmount,0) SaleAmount,0 PurchaseAmount,0 BeejakAmount" & _
                      " From Transaction2 T" & _
                      " Where T.TransType='Speed Sale' And T.EntryDate" & dateCondition)
        End If
        If IncludesPurchase(mode) Then
            parts.Add("Select AccountID,AccountName,0 SaleAmount,IfNull(TotalAmount,0) PurchaseAmount,0 BeejakAmount" & _
                      " From Vouchers Where TransType In ('Purchase','Purchase (Loose)')" & _
                      " And EntryDate" & dateCondition)
        End If
        If IncludesBeejak(mode) Then
            parts.Add("Select SallerID AccountID,SallerName AccountName,0 SaleAmount,0 PurchaseAmount,IfNull(TotalAmount,0) BeejakAmount" & _
                      " From Vouchers Where TransType In ('Beejak','Auto Beejak')" & _
                      " And EntryDate" & dateCondition)
        End If

        Dim unionSql As String = String.Empty
        For Each part As String In parts
            If unionSql <> String.Empty Then unionSql &= " Union All "
            unionSql &= part
        Next

        Dim sql As String = "Select X.AccountID," & _
            " IfNull((Select AccountName From Accounts A Where A.ID=X.AccountID),Max(X.AccountName)) AccountName," & _
            " IfNull((Select City From Accounts A Where A.ID=X.AccountID),'') City," & _
            " Sum(X.SaleAmount) SaleAmount,Sum(X.PurchaseAmount) PurchaseAmount,Sum(X.BeejakAmount) BeejakAmount" & _
            " From (" & unionSql & ") X Where IfNull(X.AccountID,0)>0" & _
            " Group By X.AccountID Order By AccountName"
        Return clsFun.ExecDataTable(sql)
    End Function
End Class

Friend Class MonthlyReportOutput
    Private reportGrid As DataGridView
    Private reportTitle As String
    Private reportSubTitle As String
    Private currentRow As Integer
    Private pageNumber As Integer

    Public Sub New(ByVal grid As DataGridView, ByVal title As String, ByVal subTitle As String)
        reportGrid = grid
        reportTitle = title
        reportSubTitle = subTitle
    End Sub

    Public Shared Sub FitToMdiClient(ByVal reportForm As Form)
        reportForm.Left = 0
        reportForm.Top = 0
        If reportForm.MdiParent Is Nothing Then Exit Sub
        For Each parentControl As Control In reportForm.MdiParent.Controls
            If TypeOf parentControl Is MdiClient Then
                Dim client As MdiClient = DirectCast(parentControl, MdiClient)
                reportForm.Height = client.ClientSize.Height
                If reportForm.Width > client.ClientSize.Width Then reportForm.Width = client.ClientSize.Width
                Exit For
            End If
        Next
    End Sub

    Public Sub PrintPreview(ByVal owner As Form)
        If reportGrid.Rows.Count = 0 Then
            MsgBox("There are no records to print.", MsgBoxStyle.Information, "Aadhat")
            Exit Sub
        End If
        currentRow = 0
        pageNumber = 1
        Dim document As New PrintDocument()
        document.DefaultPageSettings.Landscape = True
        document.DocumentName = reportTitle
        AddHandler document.PrintPage, AddressOf PrintPage
        Dim preview As New PrintPreviewDialog()
        preview.Document = document
        preview.WindowState = FormWindowState.Maximized
        preview.ShowDialog(owner)
        RemoveHandler document.PrintPage, AddressOf PrintPage
        preview.Dispose()
        document.Dispose()
    End Sub

    Private Sub PrintPage(ByVal sender As Object, ByVal e As PrintPageEventArgs)
        Dim visibleColumns As New List(Of DataGridViewColumn)()
        Dim totalGridWidth As Integer = 0
        For Each column As DataGridViewColumn In reportGrid.Columns
            If column.Visible Then
                visibleColumns.Add(column)
                totalGridWidth += column.Width
            End If
        Next

        Dim titleFont As New Font("Times New Roman", 18.0!, FontStyle.Bold)
        Dim subTitleFont As New Font("Times New Roman", 10.0!, FontStyle.Regular)
        Dim headerFont As New Font("Times New Roman", 9.0!, FontStyle.Bold)
        Dim rowFont As New Font("Times New Roman", 9.0!, FontStyle.Regular)
        Dim centerFormat As New StringFormat()
        centerFormat.Alignment = StringAlignment.Center
        centerFormat.LineAlignment = StringAlignment.Center
        Dim rightFormat As New StringFormat()
        rightFormat.Alignment = StringAlignment.Far
        rightFormat.LineAlignment = StringAlignment.Center
        Dim leftFormat As New StringFormat()
        leftFormat.Alignment = StringAlignment.Near
        leftFormat.LineAlignment = StringAlignment.Center

        Dim bounds As Rectangle = e.MarginBounds
        Dim y As Single = bounds.Top
        e.Graphics.DrawString(reportTitle, titleFont, Brushes.Black, New RectangleF(bounds.Left, y, bounds.Width, 30), centerFormat)
        y += 31
        e.Graphics.DrawString(reportSubTitle, subTitleFont, Brushes.Black, New RectangleF(bounds.Left, y, bounds.Width, 22), centerFormat)
        y += 27

        Dim scale As Single = CSng(bounds.Width) / CSng(totalGridWidth)
        Dim x As Single = bounds.Left
        Dim headerHeight As Single = 28
        For Each column As DataGridViewColumn In visibleColumns
            Dim width As Single = column.Width * scale
            e.Graphics.FillRectangle(New SolidBrush(Color.FromArgb(42, 75, 79)), x, y, width, headerHeight)
            e.Graphics.DrawRectangle(Pens.Gray, x, y, width, headerHeight)
            e.Graphics.DrawString(column.HeaderText, headerFont, Brushes.White, New RectangleF(x + 2, y, width - 4, headerHeight), centerFormat)
            x += width
        Next
        y += headerHeight

        Dim rowHeight As Single = 24
        While currentRow < reportGrid.Rows.Count AndAlso y + rowHeight < bounds.Bottom - 25
            x = bounds.Left
            Dim gridRow As DataGridViewRow = reportGrid.Rows(currentRow)
            For Each column As DataGridViewColumn In visibleColumns
                Dim width As Single = column.Width * scale
                e.Graphics.DrawRectangle(Pens.LightGray, x, y, width, rowHeight)
                Dim value As String = Convert.ToString(gridRow.Cells(column.Index).FormattedValue)
                Dim format As StringFormat = If(column.Index >= 3 OrElse column.Name.EndsWith("Amount"), rightFormat, leftFormat)
                e.Graphics.DrawString(value, rowFont, Brushes.Black, New RectangleF(x + 3, y, width - 6, rowHeight), format)
                x += width
            Next
            currentRow += 1
            y += rowHeight
        End While

        e.Graphics.DrawString("Page " & pageNumber, subTitleFont, Brushes.Black, New RectangleF(bounds.Left, bounds.Bottom - 20, bounds.Width, 20), rightFormat)
        e.HasMorePages = currentRow < reportGrid.Rows.Count
        If e.HasMorePages Then pageNumber += 1

        titleFont.Dispose()
        subTitleFont.Dispose()
        headerFont.Dispose()
        rowFont.Dispose()
        centerFormat.Dispose()
        rightFormat.Dispose()
        leftFormat.Dispose()
    End Sub

    Public Shared Sub ExportExcelCsv(ByVal owner As Form, ByVal grid As DataGridView, ByVal defaultFileName As String, ByVal title As String, ByVal subTitle As String)
        If grid.Rows.Count = 0 Then
            MsgBox("There are no records to export.", MsgBoxStyle.Information, "Aadhat")
            Exit Sub
        End If
        Dim dialog As New SaveFileDialog()
        dialog.Filter = "Excel CSV File (*.csv)|*.csv"
        dialog.FileName = defaultFileName
        dialog.AddExtension = True
        If dialog.ShowDialog(owner) <> DialogResult.OK Then Exit Sub

        Try
            Using writer As New StreamWriter(dialog.FileName, False, New System.Text.UTF8Encoding(True))
                writer.WriteLine(CsvValue(title))
                writer.WriteLine(CsvValue(subTitle))
                Dim headers As New List(Of String)()
                For Each column As DataGridViewColumn In grid.Columns
                    If column.Visible Then headers.Add(CsvValue(column.HeaderText))
                Next
                writer.WriteLine(String.Join(",", headers.ToArray()))

                For Each gridRow As DataGridViewRow In grid.Rows
                    Dim values As New List(Of String)()
                    For Each column As DataGridViewColumn In grid.Columns
                        If column.Visible Then values.Add(CsvValue(Convert.ToString(gridRow.Cells(column.Index).FormattedValue)))
                    Next
                    writer.WriteLine(String.Join(",", values.ToArray()))
                Next

                Dim totals As New List(Of String)()
                Dim firstVisible As Boolean = True
                For Each column As DataGridViewColumn In grid.Columns
                    If column.Visible Then
                        If firstVisible Then
                            totals.Add(CsvValue("TOTAL"))
                            firstVisible = False
                        ElseIf column.Name.EndsWith("Amount") Then
                            Dim total As Decimal = 0D
                            For Each gridRow As DataGridViewRow In grid.Rows
                                total += CDec(Val(Convert.ToString(gridRow.Cells(column.Index).Value)))
                            Next
                            totals.Add(CsvValue(total.ToString("N2")))
                        Else
                            totals.Add(String.Empty)
                        End If
                    End If
                Next
                writer.WriteLine(String.Join(",", totals.ToArray()))
            End Using
            MsgBox("Report exported successfully.", MsgBoxStyle.Information, "Aadhat")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Aadhat")
        Finally
            dialog.Dispose()
        End Try
    End Sub

    Private Shared Function CsvValue(ByVal value As String) As String
        If value Is Nothing Then value = String.Empty
        Return Chr(34) & value.Replace(Chr(34), Chr(34) & Chr(34)) & Chr(34)
    End Function
End Class

Public Class Monthly_Sale_Purchase_Report
    Inherits Form

    Private WithEvents cbReportType As New ComboBox()
    Private WithEvents btnShow As New Button()
    Private WithEvents btnPrint As New Button()
    Private WithEvents btnExport As New Button()
    Private WithEvents btnClose As New Button()
    Private dgReport As New DataGridView()
    Private lblPeriod As New Label()
    Private txtSaleTotal As New TextBox()
    Private txtPurchaseTotal As New TextBox()
    Private txtBeejakTotal As New TextBox()
    Private txtGrandTotal As New TextBox()

    Public Sub New()
        BuildForm("MONTHLY SALE / PURCHASE REPORT")
    End Sub

    Private Sub BuildForm(ByVal title As String)
        Me.Text = title
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        Me.StartPosition = FormStartPosition.Manual
        Me.Size = New Size(1196, 700)
        Me.Top = 0
        Me.Left = 0

        Dim header As New Panel()
        header.Dock = DockStyle.Top
        header.Height = 72
        header.BackColor = Color.FromArgb(247, 220, 111)
        Dim heading As New Label()
        heading.Text = title
        heading.Font = New Font("Times New Roman", 25.0!, FontStyle.Regular)
        heading.TextAlign = ContentAlignment.MiddleCenter
        heading.Dock = DockStyle.Fill
        btnClose.Text = "CLOSE"
        btnClose.Font = New Font("Consolas", 10.0!, FontStyle.Bold)
        btnClose.BackColor = Color.LightCoral
        btnClose.ForeColor = Color.White
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.Size = New Size(52, 28)
        btnClose.Location = New Point(Me.Width - 60, 22)
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        header.Controls.Add(heading)
        header.Controls.Add(btnClose)
        btnClose.BringToFront()

        Dim filters As New Panel()
        filters.Dock = DockStyle.Top
        filters.Height = 48
        filters.BackColor = Color.FromArgb(42, 75, 79)
        Dim typeLabel As New Label()
        typeLabel.Text = "Report Type :"
        typeLabel.ForeColor = Color.White
        typeLabel.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        typeLabel.AutoSize = True
        typeLabel.Location = New Point(18, 14)
        cbReportType.DropDownStyle = ComboBoxStyle.DropDownList
        cbReportType.Font = New Font("Times New Roman", 12.0!)
        cbReportType.Location = New Point(125, 10)
        cbReportType.Size = New Size(230, 28)
        cbReportType.Items.AddRange(MonthlyReportData.GetModes())
        cbReportType.SelectedIndex = 0
        btnShow.Text = "Show"
        btnShow.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        btnShow.BackColor = Color.FromArgb(25, 190, 190)
        btnShow.ForeColor = Color.White
        btnShow.FlatStyle = FlatStyle.Flat
        btnShow.Location = New Point(365, 8)
        btnShow.Size = New Size(90, 32)
        ConfigureActionButton(btnPrint, "Print", 465)
        ConfigureActionButton(btnExport, "Export", 565)
        ConfigureActionButton(btnClose, "Close", 665)
        btnClose.BackColor = Color.LightCoral
        lblPeriod.ForeColor = Color.White
        lblPeriod.Font = New Font("Times New Roman", 11.0!, FontStyle.Bold)
        lblPeriod.AutoSize = True
        lblPeriod.Location = New Point(770, 14)
        filters.Controls.AddRange(New Control() {typeLabel, cbReportType, btnShow, btnPrint, btnExport, lblPeriod})
        filters.Controls.Add(btnClose)

        ConfigureGrid()
        Dim totals As Panel = BuildTotalsPanel()
        Me.Controls.Add(dgReport)
        Me.Controls.Add(totals)
        Me.Controls.Add(filters)
        Me.Controls.Add(header)
        AddHandler Me.Load, AddressOf Report_Load
        AddHandler Me.Shown, AddressOf Report_Shown
        AddHandler Me.KeyDown, AddressOf Report_KeyDown
    End Sub

    Private Sub ConfigureActionButton(ByVal button As Button, ByVal caption As String, ByVal left As Integer)
        button.Text = caption
        button.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        button.BackColor = Color.FromArgb(25, 190, 190)
        button.ForeColor = Color.White
        button.FlatStyle = FlatStyle.Flat
        button.Location = New Point(left, 8)
        button.Size = New Size(90, 32)
    End Sub

    Private Sub ConfigureGrid()
        dgReport.Dock = DockStyle.Fill
        dgReport.AllowUserToAddRows = False
        dgReport.AllowUserToDeleteRows = False
        dgReport.AllowUserToResizeRows = False
        dgReport.ReadOnly = True
        dgReport.RowHeadersVisible = False
        dgReport.BackgroundColor = Color.FromArgb(245, 245, 252)
        dgReport.BorderStyle = BorderStyle.FixedSingle
        dgReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgReport.ColumnHeadersHeight = 32
        dgReport.RowTemplate.Height = 28
        dgReport.DefaultCellStyle.Font = New Font("Times New Roman", 12.0!)
        dgReport.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        dgReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(42, 75, 79)
        dgReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgReport.EnableHeadersVisualStyles = False
        dgReport.Columns.Add("MonthName", "Month Name")
        dgReport.Columns.Add("SaleAmount", "Sale Amount")
        dgReport.Columns.Add("PurchaseAmount", "Purchase Amount")
        dgReport.Columns.Add("BeejakAmount", "Beejak Amount")
        dgReport.Columns.Add("TotalAmount", "Total")
        For columnIndex As Integer = 1 To dgReport.Columns.Count - 1
            dgReport.Columns(columnIndex).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgReport.Columns(columnIndex).DefaultCellStyle.Format = "N2"
        Next
    End Sub

    Private Function BuildTotalsPanel() As Panel
        Dim panel As New Panel()
        panel.Dock = DockStyle.Bottom
        panel.Height = 48
        panel.BackColor = Color.FromArgb(42, 75, 79)
        AddTotalBox(panel, "Sale Total :", txtSaleTotal, 18)
        AddTotalBox(panel, "Purchase Total :", txtPurchaseTotal, 300)
        AddTotalBox(panel, "Beejak Total :", txtBeejakTotal, 600)
        AddTotalBox(panel, "Grand Total :", txtGrandTotal, 890)
        Return panel
    End Function

    Private Sub AddTotalBox(ByVal panel As Panel, ByVal caption As String, ByVal valueBox As TextBox, ByVal left As Integer)
        Dim label As New Label()
        label.Text = caption
        label.ForeColor = Color.White
        label.Font = New Font("Times New Roman", 11.0!, FontStyle.Bold)
        label.AutoSize = True
        label.Location = New Point(left, 15)
        valueBox.ReadOnly = True
        valueBox.TextAlign = HorizontalAlignment.Right
        valueBox.Font = New Font("Times New Roman", 11.0!, FontStyle.Bold)
        valueBox.Location = New Point(left + 105, 10)
        valueBox.Size = New Size(150, 28)
        panel.Controls.Add(label)
        panel.Controls.Add(valueBox)
    End Sub

    Private Sub Report_Load(ByVal sender As Object, ByVal e As EventArgs)
        MonthlyReportOutput.FitToMdiClient(Me)
        lblPeriod.Text = "Financial Year: " & FinYearStart.ToString("dd-MM-yyyy") & " to " & FinYearEnd.ToString("dd-MM-yyyy")
        LoadReport()
    End Sub

    Private Sub Report_Shown(ByVal sender As Object, ByVal e As EventArgs)
        MonthlyReportOutput.FitToMdiClient(Me)
    End Sub

    Private Sub btnShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShow.Click
        LoadReport()
    End Sub

    Private Sub LoadReport()
        Try
            dgReport.Rows.Clear()
            Dim mode As String = cbReportType.Text
            Dim saleGrand As Decimal = 0D
            Dim purchaseGrand As Decimal = 0D
            Dim beejakGrand As Decimal = 0D
            Dim months As List(Of MonthlyReportPeriod) = MonthlyReportData.GetFinancialMonths()

            For Each period As MonthlyReportPeriod In months
                Dim sale As Decimal = 0D
                Dim purchase As Decimal = 0D
                Dim beejak As Decimal = 0D
                If MonthlyReportData.IncludesSale(mode) Then sale = MonthlyReportData.SaleTotal(period.StartDate, period.EndDate)
                If MonthlyReportData.IncludesPurchase(mode) Then purchase = MonthlyReportData.PurchaseTotal(period.StartDate, period.EndDate)
                If MonthlyReportData.IncludesBeejak(mode) Then beejak = MonthlyReportData.BeejakTotal(period.StartDate, period.EndDate)
                dgReport.Rows.Add(period.Name, sale, purchase, beejak, sale + purchase + beejak)
                saleGrand += sale
                purchaseGrand += purchase
                beejakGrand += beejak
            Next

            dgReport.Columns("SaleAmount").Visible = MonthlyReportData.IncludesSale(mode)
            dgReport.Columns("PurchaseAmount").Visible = MonthlyReportData.IncludesPurchase(mode)
            dgReport.Columns("BeejakAmount").Visible = MonthlyReportData.IncludesBeejak(mode)
            txtSaleTotal.Text = saleGrand.ToString("N2")
            txtPurchaseTotal.Text = purchaseGrand.ToString("N2")
            txtBeejakTotal.Text = beejakGrand.ToString("N2")
            txtGrandTotal.Text = (saleGrand + purchaseGrand + beejakGrand).ToString("N2")
            dgReport.ClearSelection()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Aadhat")
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        If dgReport.Rows.Count = 0 Then MsgBox("There are no records to print.", MsgBoxStyle.Information, "Aadhat") : Exit Sub
        Dim reportPath As String = "\Reports\MonthlySalePurchase.rpt"
        If Not File.Exists(Application.StartupPath & reportPath) Then MsgBox("Please create Crystal Report: " & reportPath, MsgBoxStyle.Information, "Report Not Found") : Exit Sub
        PrepareCrystalData()
        Report_Viewer.printReport(reportPath)
        Report_Viewer.reportName = reportPath
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        Report_Viewer.Left = 0
        Report_Viewer.Top = 0
        Report_Viewer.BringToFront()
    End Sub

    Private Sub PrepareCrystalData()
        ClsFunPrimary.ExecNonQuery("Delete From Printing")
        Dim mode As String = cbReportType.Text
        Dim financialYear As String = FinYearStart.ToString("dd-MM-yyyy") & " to " & FinYearEnd.ToString("dd-MM-yyyy")
        For Each row As DataGridViewRow In dgReport.Rows
            Dim sql As String = "Insert Into Printing(M1,M2,P1,P2,P3,P4,P5,P6,P7,P8,P9) Values(" & _
                SqlText(mode) & "," & SqlText(financialYear) & "," & SqlText(Convert.ToString(row.Cells("MonthName").Value)) & "," & _
                SqlText(Convert.ToString(row.Cells("SaleAmount").Value)) & "," & SqlText(Convert.ToString(row.Cells("PurchaseAmount").Value)) & "," & _
                SqlText(Convert.ToString(row.Cells("BeejakAmount").Value)) & "," & SqlText(Convert.ToString(row.Cells("TotalAmount").Value)) & "," & _
                SqlText(txtSaleTotal.Text) & "," & SqlText(txtPurchaseTotal.Text) & "," & SqlText(txtBeejakTotal.Text) & "," & SqlText(txtGrandTotal.Text) & ")"
            ClsFunPrimary.ExecNonQuery(sql)
        Next
    End Sub

    Private Function SqlText(ByVal value As String) As String
        If value Is Nothing Then value = String.Empty
        Return "'" & value.Replace("'", "''") & "'"
    End Function

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        MonthlyReportOutput.ExportExcelCsv(Me, dgReport, "Monthly_Sale_Purchase.csv", Me.Text, lblPeriod.Text & " | " & cbReportType.Text)
    End Sub

    Private Sub Report_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
End Class

Public Class Monthly_Account_Wise_Sale_Purchase
    Inherits Form

    Private WithEvents cbMonth As New ComboBox()
    Private WithEvents cbReportType As New ComboBox()
    Private WithEvents btnShow As New Button()
    Private WithEvents btnPrint As New Button()
    Private WithEvents btnExport As New Button()
    Private WithEvents btnClose As New Button()
    Private dgReport As New DataGridView()
    Private periods As List(Of MonthlyReportPeriod)
    Private txtSaleTotal As New TextBox()
    Private txtPurchaseTotal As New TextBox()
    Private txtBeejakTotal As New TextBox()
    Private txtGrandTotal As New TextBox()

    Public Sub New()
        BuildForm()
    End Sub

    Private Sub BuildForm()
        Me.Text = "MONTHLY ACCOUNT-WISE SALE / PURCHASE"
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackColor = Color.FromArgb(247, 220, 111)
        Me.KeyPreview = True
        Me.StartPosition = FormStartPosition.Manual
        Me.Size = New Size(1196, 700)
        Me.Top = 0
        Me.Left = 0

        Dim header As New Panel()
        header.Dock = DockStyle.Top
        header.Height = 72
        Dim heading As New Label()
        heading.Text = Me.Text
        heading.Font = New Font("Times New Roman", 25.0!)
        heading.TextAlign = ContentAlignment.MiddleCenter
        heading.Dock = DockStyle.Fill
        btnClose.Text = "CLOSE"
        btnClose.Font = New Font("Consolas", 10.0!, FontStyle.Bold)
        btnClose.BackColor = Color.LightCoral
        btnClose.ForeColor = Color.White
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.Size = New Size(52, 28)
        btnClose.Location = New Point(Me.Width - 60, 22)
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        header.Controls.Add(heading)
        header.Controls.Add(btnClose)
        btnClose.BringToFront()

        Dim filters As New Panel()
        filters.Dock = DockStyle.Top
        filters.Height = 48
        filters.BackColor = Color.FromArgb(42, 75, 79)
        Dim monthLabel As New Label()
        monthLabel.Text = "Month :"
        monthLabel.ForeColor = Color.White
        monthLabel.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        monthLabel.AutoSize = True
        monthLabel.Location = New Point(18, 14)
        cbMonth.DropDownStyle = ComboBoxStyle.DropDownList
        cbMonth.Font = New Font("Times New Roman", 12.0!)
        cbMonth.Location = New Point(85, 10)
        cbMonth.Size = New Size(160, 28)
        Dim typeLabel As New Label()
        typeLabel.Text = "Report Type :"
        typeLabel.ForeColor = Color.White
        typeLabel.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        typeLabel.AutoSize = True
        typeLabel.Location = New Point(275, 14)
        cbReportType.DropDownStyle = ComboBoxStyle.DropDownList
        cbReportType.Font = New Font("Times New Roman", 12.0!)
        cbReportType.Location = New Point(382, 10)
        cbReportType.Size = New Size(225, 28)
        cbReportType.Items.AddRange(MonthlyReportData.GetModes())
        cbReportType.SelectedIndex = 0
        btnShow.Text = "Show"
        btnShow.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        btnShow.BackColor = Color.FromArgb(25, 190, 190)
        btnShow.ForeColor = Color.White
        btnShow.FlatStyle = FlatStyle.Flat
        btnShow.Location = New Point(622, 8)
        btnShow.Size = New Size(90, 32)
        ConfigureActionButton(btnPrint, "Print", 722)
        ConfigureActionButton(btnExport, "Export", 822)
        ConfigureActionButton(btnClose, "Close", 922)
        btnClose.BackColor = Color.LightCoral
        filters.Controls.AddRange(New Control() {monthLabel, cbMonth, typeLabel, cbReportType, btnShow, btnPrint, btnExport})
        filters.Controls.Add(btnClose)

        ConfigureGrid()
        Dim totals As Panel = BuildTotalsPanel()
        Me.Controls.Add(dgReport)
        Me.Controls.Add(totals)
        Me.Controls.Add(filters)
        Me.Controls.Add(header)
        AddHandler Me.Load, AddressOf Report_Load
        AddHandler Me.Shown, AddressOf Report_Shown
        AddHandler Me.KeyDown, AddressOf Report_KeyDown
    End Sub

    Private Sub ConfigureActionButton(ByVal button As Button, ByVal caption As String, ByVal left As Integer)
        button.Text = caption
        button.Font = New Font("Times New Roman", 12.0!, FontStyle.Bold)
        button.BackColor = Color.FromArgb(25, 190, 190)
        button.ForeColor = Color.White
        button.FlatStyle = FlatStyle.Flat
        button.Location = New Point(left, 8)
        button.Size = New Size(90, 32)
    End Sub

    Private Sub ConfigureGrid()
        dgReport.Dock = DockStyle.Fill
        dgReport.AllowUserToAddRows = False
        dgReport.AllowUserToDeleteRows = False
        dgReport.AllowUserToResizeRows = False
        dgReport.ReadOnly = True
        dgReport.RowHeadersVisible = False
        dgReport.BackgroundColor = Color.FromArgb(245, 245, 252)
        dgReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgReport.RowTemplate.Height = 27
        dgReport.ColumnHeadersHeight = 32
        dgReport.DefaultCellStyle.Font = New Font("Times New Roman", 11.0!)
        dgReport.ColumnHeadersDefaultCellStyle.Font = New Font("Times New Roman", 11.0!, FontStyle.Bold)
        dgReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(42, 75, 79)
        dgReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgReport.EnableHeadersVisualStyles = False
        dgReport.Columns.Add("AccountID", "Account ID")
        dgReport.Columns("AccountID").Visible = False
        dgReport.Columns.Add("AccountName", "Account Name")
        dgReport.Columns.Add("City", "City")
        dgReport.Columns.Add("SaleAmount", "Sale Amount")
        dgReport.Columns.Add("PurchaseAmount", "Purchase Amount")
        dgReport.Columns.Add("BeejakAmount", "Beejak Amount")
        dgReport.Columns.Add("TotalAmount", "Total")
        dgReport.Columns("AccountName").FillWeight = 180
        For columnIndex As Integer = 3 To dgReport.Columns.Count - 1
            dgReport.Columns(columnIndex).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgReport.Columns(columnIndex).DefaultCellStyle.Format = "N2"
        Next
    End Sub

    Private Function BuildTotalsPanel() As Panel
        Dim panel As New Panel()
        panel.Dock = DockStyle.Bottom
        panel.Height = 48
        panel.BackColor = Color.FromArgb(42, 75, 79)
        AddTotalBox(panel, "Sale Total :", txtSaleTotal, 18)
        AddTotalBox(panel, "Purchase Total :", txtPurchaseTotal, 300)
        AddTotalBox(panel, "Beejak Total :", txtBeejakTotal, 600)
        AddTotalBox(panel, "Grand Total :", txtGrandTotal, 890)
        Return panel
    End Function

    Private Sub AddTotalBox(ByVal panel As Panel, ByVal caption As String, ByVal valueBox As TextBox, ByVal left As Integer)
        Dim label As New Label()
        label.Text = caption
        label.ForeColor = Color.White
        label.Font = New Font("Times New Roman", 11.0!, FontStyle.Bold)
        label.AutoSize = True
        label.Location = New Point(left, 15)
        valueBox.ReadOnly = True
        valueBox.TextAlign = HorizontalAlignment.Right
        valueBox.Font = New Font("Times New Roman", 11.0!, FontStyle.Bold)
        valueBox.Location = New Point(left + 105, 10)
        valueBox.Size = New Size(150, 28)
        panel.Controls.Add(label)
        panel.Controls.Add(valueBox)
    End Sub

    Private Sub Report_Load(ByVal sender As Object, ByVal e As EventArgs)
        MonthlyReportOutput.FitToMdiClient(Me)
        periods = MonthlyReportData.GetFinancialMonths()
        cbMonth.Items.Clear()
        For Each period As MonthlyReportPeriod In periods
            cbMonth.Items.Add(period.Name)
        Next
        cbMonth.SelectedIndex = 0
        LoadReport()
    End Sub

    Private Sub Report_Shown(ByVal sender As Object, ByVal e As EventArgs)
        MonthlyReportOutput.FitToMdiClient(Me)
    End Sub

    Private Sub btnShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShow.Click
        LoadReport()
    End Sub

    Private Sub LoadReport()
        If cbMonth.SelectedIndex < 0 OrElse periods Is Nothing Then Exit Sub
        Try
            dgReport.Rows.Clear()
            Dim period As MonthlyReportPeriod = periods(cbMonth.SelectedIndex)
            Dim mode As String = cbReportType.Text
            Dim data As DataTable = MonthlyReportData.AccountTotals(mode, period.StartDate, period.EndDate)
            Dim saleGrand As Decimal = 0D
            Dim purchaseGrand As Decimal = 0D
            Dim beejakGrand As Decimal = 0D

            For Each row As DataRow In data.Rows
                Dim sale As Decimal = CDec(Val(row("SaleAmount").ToString()))
                Dim purchase As Decimal = CDec(Val(row("PurchaseAmount").ToString()))
                Dim beejak As Decimal = CDec(Val(row("BeejakAmount").ToString()))
                dgReport.Rows.Add(row("AccountID"), row("AccountName"), row("City"), sale, purchase, beejak, sale + purchase + beejak)
                saleGrand += sale
                purchaseGrand += purchase
                beejakGrand += beejak
            Next

            dgReport.Columns("SaleAmount").Visible = MonthlyReportData.IncludesSale(mode)
            dgReport.Columns("PurchaseAmount").Visible = MonthlyReportData.IncludesPurchase(mode)
            dgReport.Columns("BeejakAmount").Visible = MonthlyReportData.IncludesBeejak(mode)
            txtSaleTotal.Text = saleGrand.ToString("N2")
            txtPurchaseTotal.Text = purchaseGrand.ToString("N2")
            txtBeejakTotal.Text = beejakGrand.ToString("N2")
            txtGrandTotal.Text = (saleGrand + purchaseGrand + beejakGrand).ToString("N2")
            dgReport.ClearSelection()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "Aadhat")
        End Try
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
        If dgReport.Rows.Count = 0 Then MsgBox("There are no records to print.", MsgBoxStyle.Information, "Aadhat") : Exit Sub
        Dim reportPath As String = "\Reports\MonthlyAccountWiseSalePurchase.rpt"
        If Not File.Exists(Application.StartupPath & reportPath) Then MsgBox("Please create Crystal Report: " & reportPath, MsgBoxStyle.Information, "Report Not Found") : Exit Sub
        PrepareCrystalData()
        Report_Viewer.printReport(reportPath)
        Report_Viewer.reportName = reportPath
        Report_Viewer.MdiParent = MainScreenForm
        Report_Viewer.Show()
        Report_Viewer.Left = 0
        Report_Viewer.Top = 0
        Report_Viewer.BringToFront()
    End Sub

    Private Sub PrepareCrystalData()
        ClsFunPrimary.ExecNonQuery("Delete From Printing")
        Dim periodName As String = cbMonth.Text & " " & periods(cbMonth.SelectedIndex).StartDate.Year.ToString()
        For Each row As DataGridViewRow In dgReport.Rows
            Dim sql As String = "Insert Into Printing(M1,M2,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10) Values(" & _
                SqlText(cbReportType.Text) & "," & SqlText(periodName) & "," & SqlText(Convert.ToString(row.Cells("AccountName").Value)) & "," & _
                SqlText(Convert.ToString(row.Cells("City").Value)) & "," & SqlText(Convert.ToString(row.Cells("SaleAmount").Value)) & "," & _
                SqlText(Convert.ToString(row.Cells("PurchaseAmount").Value)) & "," & SqlText(Convert.ToString(row.Cells("BeejakAmount").Value)) & "," & _
                SqlText(Convert.ToString(row.Cells("TotalAmount").Value)) & "," & SqlText(txtSaleTotal.Text) & "," & SqlText(txtPurchaseTotal.Text) & "," & _
                SqlText(txtBeejakTotal.Text) & "," & SqlText(txtGrandTotal.Text) & ")"
            ClsFunPrimary.ExecNonQuery(sql)
        Next
    End Sub

    Private Function SqlText(ByVal value As String) As String
        If value Is Nothing Then value = String.Empty
        Return "'" & value.Replace("'", "''") & "'"
    End Function

    Private Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
        Dim details As String = cbMonth.Text & " " & FinYearStart.Year.ToString() & " | " & cbReportType.Text
        MonthlyReportOutput.ExportExcelCsv(Me, dgReport, "Monthly_Account_Wise_Sale_Purchase.csv", Me.Text, details)
    End Sub

    Private Sub Report_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
End Class
