Imports System.IO
Imports System.Drawing.Printing
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class Report_Viewer
    Dim data As String = String.Empty
    Public RptName As String
    Public reportName As String
    Dim Rpt As New ReportDocument
    Private Function GetData() As DataSet
        Dim sSql As String = "Select * from Printing"
        Dim ds As New DataSet
        ds = ClsFunPrimary.ExecDataSet(sSql, "Printing")
        GetData = ds
    End Function
    Sub printReport(ByRef reportName As String)
        RptName = reportName
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables
        Try
            If Not File.Exists(Application.StartupPath & reportName) Then MsgBox("Report : " & reportName & " Not Exists.... Please Contact to Service Provider...", MsgBoxStyle.Critical, "Not Found...") : Me.Close() : Exit Sub
            Rpt.Load(Application.StartupPath & reportName)
            'Rpt.GetDefaultProperty()
            ' Rpt.PrintOptions.PaperOrientation = PaperOrientation.DefaultPaperOrientation
            CrTables = Rpt.Database.Tables
            For Each CrTable In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            'Report1.PrintToPrinter(1, False, 1, 1)
            Dim dsCustomers As DataSet = GetData()
            Rpt.SetDataSource(dsCustomers.Tables("Printing"))
            Rpt.SetParameterValue("CompName", compname)
            Rpt.SetParameterValue("Mob1", Mob1)
            Rpt.SetParameterValue("Mob2", Mob2)
            Rpt.SetParameterValue("Address", Address)
            Rpt.SetParameterValue("City", City)
            Rpt.SetParameterValue("State", State)
            Rpt.SetParameterValue("AddressHindi", AddressHindi)
            Rpt.SetParameterValue("CityHindi", CityHindi)
            Rpt.SetParameterValue("StateHindi", StateHindi)
            Rpt.SetParameterValue("HindiCompanyName", compnameHindi)
            CrystalReportViewer1.ReportSource = Rpt
            ' Rpt.PrintToPrinter(1, False, 1, 1)
            CrTables.Dispose() ': Rpt.Close()
        Catch ex As Exception
            If ex.Message.Contains("The type initializer for 'CrystalDecisions.ReportSource.ReportSourceFactory' threw an exception.") Then
                If MsgBox("Crystal Reports Not Install... Do you Want to Download?") = vbYes Then
                    Process.Start("https://softmanagementindia.in/release/Support.msi")
                End If
            End If
            '  MsgBox(ex.Message)
        End Try
    End Sub
    Sub PrintDirect(ByRef reportName As String)
        Dim printReportDocument As ReportDocument = Nothing
        Try
            If String.IsNullOrEmpty(reportName) OrElse reportName.Trim() = String.Empty Then
                MsgBox("Report is not ready for printing.", MsgBoxStyle.Exclamation, "Print Report")
                Exit Sub
            End If

            Dim reportPath As String = Path.Combine(Application.StartupPath, reportName.TrimStart("\"c, "/"c))
            If Not File.Exists(reportPath) Then
                MsgBox("Report file not found: " & reportPath, MsgBoxStyle.Critical, "Print Report")
                Exit Sub
            End If

            'Use a separate document for printing so the preview's report stays alive.
            printReportDocument = New ReportDocument()
            printReportDocument.Load(reportPath)

            '==== CONNECTION ====
            Dim crConnectionInfo As New ConnectionInfo()
            Dim CrTables As Tables = printReportDocument.Database.Tables

            For Each CrTable As Table In CrTables
                Dim crtableLogoninfo As TableLogOnInfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            '==== DATA ====
            Dim dsCustomers As DataSet = GetData()

            If dsCustomers Is Nothing OrElse Not dsCustomers.Tables.Contains("Printing") Then
                MsgBox("Printing Data Not Found")
                Exit Sub
            End If

            printReportDocument.SetDataSource(dsCustomers.Tables("Printing"))

            '==== PARAMETERS ====
            printReportDocument.SetParameterValue("CompName", compname)
            printReportDocument.SetParameterValue("Mob1", Mob1)
            printReportDocument.SetParameterValue("Mob2", Mob2)
            printReportDocument.SetParameterValue("Address", Address)
            printReportDocument.SetParameterValue("City", City)
            printReportDocument.SetParameterValue("State", State)
            printReportDocument.SetParameterValue("AddressHindi", AddressHindi)
            printReportDocument.SetParameterValue("CityHindi", CityHindi)
            printReportDocument.SetParameterValue("StateHindi", StateHindi)
            printReportDocument.SetParameterValue("HindiCompanyName", compnameHindi)

            '==== FONT SCALE ====
            Dim fontScaleFactor As Single = 1.0

            For Each section As Section In printReportDocument.ReportDefinition.Sections
                For Each obj As Object In section.ReportObjects
                    If TypeOf obj Is TextObject Then
                        Dim textObj As TextObject = CType(obj, TextObject)
                        textObj.ApplyFont(New Font(textObj.Font.FontFamily, textObj.Font.Size * fontScaleFactor))
                    End If
                Next
            Next

            '==== PRINT DIALOG ====
            Using dialog1 As New PrintDialog()
                dialog1.AllowSomePages = True
                dialog1.AllowPrintToFile = False

                If PrinterSettings.InstalledPrinters.Count = 0 Then
                    MsgBox("Windows me koi printer installed nahi hai. Printer install/online karke dobara try karein.", MsgBoxStyle.Exclamation, "Printer unavailable")
                    Exit Sub
                End If

                If dialog1.ShowDialog() = DialogResult.OK Then
                    If Not dialog1.PrinterSettings.IsValid Then
                        MsgBox("Selected printer available nahi hai. Printers & scanners me printer status check karein.", MsgBoxStyle.Exclamation, "Printer unavailable")
                        Exit Sub
                    End If

                    printReportDocument.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName

                    printReportDocument.PrintToPrinter(
                        dialog1.PrinterSettings.Copies,
                        dialog1.PrinterSettings.Collate,
                        dialog1.PrinterSettings.FromPage,
                        dialog1.PrinterSettings.ToPage
                    )
                End If
            End Using

            Dim PreviewClose As String = clsFun.ExecScalarStr( _
                 "SELECT IFNULL(PreviewClose,'Yes') FROM Controls LIMIT 1")
            If PreviewClose = "Yes" Then Me.Close()
        Catch ex As Exception
            MsgBox("Print failed: " & ex.Message, MsgBoxStyle.Critical, "Print Report")
        Finally
            If printReportDocument IsNot Nothing Then
                Try
                    printReportDocument.Close()
                Finally
                    printReportDocument.Dispose()
                End Try
            End If
        End Try

    End Sub
    'Sub PrintDirect(ByRef reportName As String)
    '    Dim crtableLogoninfo As New TableLogOnInfo
    '    Dim crConnectionInfo As New ConnectionInfo
    '    Dim CrTables As Tables
    '    Try
    '        '            Dim Rpt As New ReportDocument
    '        Rpt.Load(Application.StartupPath & reportName)
    '        CrTables = Rpt.Database.Tables
    '        For Each CrTable In CrTables
    '            crtableLogoninfo = CrTable.LogOnInfo
    '            crtableLogoninfo.ConnectionInfo = crConnectionInfo
    '            CrTable.ApplyLogOnInfo(crtableLogoninfo)
    '        Next
    '        Dim dsCustomers As DataSet = GetData()
    '        Rpt.SetDataSource(dsCustomers.Tables("Printing"))
    '        Rpt.SetParameterValue("CompName", compname)
    '        Rpt.SetParameterValue("Mob1", Mob1)
    '        Rpt.SetParameterValue("Mob2", Mob2)
    '        Rpt.SetParameterValue("Address", Address)
    '        Rpt.SetParameterValue("City", City)
    '        Rpt.SetParameterValue("State", State)
    '        Rpt.SetParameterValue("AddressHindi", AddressHindi)
    '        Rpt.SetParameterValue("CityHindi", CityHindi)
    '        Rpt.SetParameterValue("StateHindi", StateHindi)
    '        Rpt.SetParameterValue("HindiCompanyName", compnameHindi)
    '        'CrystalReportViewer1.ReportSource = Rpt
    '        ' CrystalReportViewer1.Refresh()
    '        '  Dim report1 As CrystalReport1 = New CrystalReport1()
    '        Dim fontScaleFactor As Single = 1.0 ' Adjust this value as needed
    '        For Each section As Section In Rpt.ReportDefinition.Sections
    '            For Each obj As Object In section.ReportObjects
    '                If TypeOf obj Is TextObject Then
    '                    Dim textObj As TextObject = DirectCast(obj, TextObject)
    '                    textObj.ApplyFont(New Font(textObj.Font.FontFamily, textObj.Font.Size * fontScaleFactor))
    '                End If
    '            Next
    '        Next

    '        Dim dialog1 As PrintDialog = New PrintDialog()
    '        dialog1.AllowSomePages = True
    '        dialog1.AllowPrintToFile = False
    '        If dialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
    '            Dim copies As Integer = dialog1.PrinterSettings.Copies
    '            Dim fromPage As Integer = dialog1.PrinterSettings.FromPage
    '            Dim toPage As Integer = dialog1.PrinterSettings.ToPage
    '            Dim collate As Boolean = dialog1.PrinterSettings.Collate
    '            Rpt.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName
    '            Rpt.PrintToPrinter(copies, collate, fromPage, toPage)
    '        End If
    '        Rpt.Dispose() : dialog1.Dispose()
    '        '  Rpt.PrintToPrinter(1, False, 0, 0)
    '        CrTables.Dispose() : Rpt.Close() : Rpt.Dispose() ': Me.Close()
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub
    Private Sub CustomPaper()
        'Dim rep = New ReportDocument()
        'Dim printerSettings = New System.Drawing.Printing.PrinterSettings()
        'Dim pSettings = New System.Drawing.Printing.PageSettings(printerSettings)
        'pSettings.PaperSize = New System.Drawing.Printing.PaperSize
        'pSettings.Margins = New System.Drawing.Printing.Margins(0, 0, 0, 0)
        '' rep.PrintOptions.DissociatePageSizeAndPrinterPaperSize = True
        'rep.PrintOptions.CopyFrom(printerSettings, pSettings)
    End Sub
    Sub ExportReport(ByRef reportName As String, Optional exportFormat As String = "PDF")
        If reportName Is Nothing OrElse reportName.Trim() = String.Empty Then
            MsgBox("Report is not ready for export.", MsgBoxStyle.Exclamation, "Export Report")
            Exit Sub
        End If

        Dim exportReport As New ReportDocument
        Dim CrTables As Tables = Nothing
        Try
            If Not File.Exists(Application.StartupPath & reportName) Then
                MsgBox("Report : " & reportName & " Not Exists.... Please Contact to Service Provider...", MsgBoxStyle.Critical, "Not Found...")
                Exit Sub
            End If

            exportReport.Load(Application.StartupPath & reportName)
            CrTables = exportReport.Database.Tables
            Dim crConnectionInfo As New ConnectionInfo
            For Each CrTable In CrTables
                Dim crtableLogoninfo As TableLogOnInfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            Dim dsCustomers As DataSet = GetData()
            exportReport.SetDataSource(dsCustomers.Tables("Printing"))
            exportReport.SetParameterValue("CompName", compname)
            exportReport.SetParameterValue("Mob1", Mob1)
            exportReport.SetParameterValue("Mob2", Mob2)
            exportReport.SetParameterValue("Address", Address)
            exportReport.SetParameterValue("City", City)
            exportReport.SetParameterValue("State", State)
            exportReport.SetParameterValue("AddressHindi", AddressHindi)
            exportReport.SetParameterValue("CityHindi", CityHindi)
            exportReport.SetParameterValue("StateHindi", StateHindi)
            exportReport.SetParameterValue("HindiCompanyName", compnameHindi)

            Dim normalizedFormat As String = exportFormat.Trim().ToUpperInvariant()
            Dim extension As String = If(normalizedFormat = "EXCEL", "xlsx", If(normalizedFormat = "CSV", "csv", "pdf"))
            Using saveDialog As New SaveFileDialog()
                saveDialog.Title = "Export Report - " & normalizedFormat
                saveDialog.Filter = If(normalizedFormat = "EXCEL", "Excel Workbook (*.xlsx)|*.xlsx", If(normalizedFormat = "CSV", "CSV File (*.csv)|*.csv", "PDF Document (*.pdf)|*.pdf"))
                saveDialog.DefaultExt = extension
                saveDialog.AddExtension = True
                saveDialog.FileName = Path.GetFileNameWithoutExtension(reportName) & "_" & DateTime.Now.ToString("yyyyMMdd_HHmm") & "." & extension
                If saveDialog.ShowDialog(Me) <> DialogResult.OK Then Exit Sub

                Dim destinationOptions As New DiskFileDestinationOptions With {.DiskFileName = saveDialog.FileName}
                With exportReport.ExportOptions
                    .ExportDestinationType = ExportDestinationType.DiskFile
                    .DestinationOptions = destinationOptions
                    If normalizedFormat = "EXCEL" Then
                        .ExportFormatType = ExportFormatType.ExcelWorkbook
                        .FormatOptions = New ExcelFormatOptions()
                    ElseIf normalizedFormat = "CSV" Then
                        .ExportFormatType = ExportFormatType.CharacterSeparatedValues
                        .FormatOptions = New CharacterSeparatedValuesFormatOptions()
                    Else
                        .ExportFormatType = ExportFormatType.PortableDocFormat
                        .FormatOptions = New PdfRtfWordFormatOptions()
                    End If
                End With
                exportReport.Export()

                If Not File.Exists(saveDialog.FileName) Then
                    MsgBox("Export unsuccessful. The file could not be created.", MsgBoxStyle.Critical, "Export Report")
                    Exit Sub
                End If

                If MsgBox(normalizedFormat & " export successful." & vbCrLf & vbCrLf &
                          saveDialog.FileName & vbCrLf & vbCrLf &
                          "Do you want to open this file now?", MsgBoxStyle.Information Or MsgBoxStyle.YesNo, "Export Report") = MsgBoxResult.Yes Then
                    Try
                        Process.Start(saveDialog.FileName)
                    Catch ex As Exception
                        MsgBox("File was exported successfully, but could not be opened." & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, "Export Report")
                    End Try
                End If
            End Using
        Catch ex As Exception
            MsgBox("Export failed: " & ex.Message, MsgBoxStyle.Critical, "Export Report")
        Finally
            If CrTables IsNot Nothing Then CrTables.Dispose()
            exportReport.Close()
            exportReport.Dispose()
        End Try
    End Sub

    Private Sub Report_Viewer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Report_Viewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.GhostWhite
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        ' printReport(reportname)
        CrystalReportViewer1.Zoom(100)
        'data = MainScreenForm.tssDbpath.Text
        'If data <> "" Then
        '    isCompanyOpen = True
        '    clsFun.ChangePath("Data\" & data)
        'End If
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        ExportReport(RptName, "PDF")
    End Sub

    Private Sub ButtonPrint_Click(sender As Object, e As EventArgs) Handles ButtonPrint.Click
        PrintDirect(RptName)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ExportReport(RptName, "EXCEL")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ExportReport(RptName, "CSV")
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs)
        '  PrintDialog1.ShowDialog()
        PrintDirect(RptName)
    End Sub

    Private Sub BtnPrintO_Click(sender As Object, e As EventArgs) Handles BtnPrintO.Click
        PrintDirect(RptName)
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        CrystalReportViewer1.Refresh()
    End Sub

    Private Sub GenericResponsiveFormScaler_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        GenericResponsiveFormScaler.Bind(Me)
    End Sub
End Class
