Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class Report_Viewer
    Dim data As String = String.Empty
    Public RptName As String
    Public reportName As String
    Dim Rpt As New ReportDocument
    Private Function GetData() As DataSet
        Dim sSql As String = "vacuum;Select * from Printing"
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
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables
        Try
            '            Dim Rpt As New ReportDocument
            Rpt.Load(Application.StartupPath & reportName)
            CrTables = Rpt.Database.Tables
            For Each CrTable In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next
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
            'CrystalReportViewer1.ReportSource = Rpt
            ' CrystalReportViewer1.Refresh()
            '  Dim report1 As CrystalReport1 = New CrystalReport1()
            Dim fontScaleFactor As Single = 1.0 ' Adjust this value as needed
            For Each section As Section In Rpt.ReportDefinition.Sections
                For Each obj As Object In section.ReportObjects
                    If TypeOf obj Is TextObject Then
                        Dim textObj As TextObject = DirectCast(obj, TextObject)
                        textObj.ApplyFont(New Font(textObj.Font.FontFamily, textObj.Font.Size * fontScaleFactor))
                    End If
                Next
            Next

            Dim dialog1 As PrintDialog = New PrintDialog()
            dialog1.AllowSomePages = True
            dialog1.AllowPrintToFile = False
            If dialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                Dim copies As Integer = dialog1.PrinterSettings.Copies
                Dim fromPage As Integer = dialog1.PrinterSettings.FromPage
                Dim toPage As Integer = dialog1.PrinterSettings.ToPage
                Dim collate As Boolean = dialog1.PrinterSettings.Collate
                Rpt.PrintOptions.PrinterName = dialog1.PrinterSettings.PrinterName
                Rpt.PrintToPrinter(copies, collate, fromPage, toPage)
            End If
            Rpt.Dispose() : dialog1.Dispose()
            '  Rpt.PrintToPrinter(1, False, 0, 0)
            CrTables.Dispose() : Rpt.Close() : Rpt.Dispose() : Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub CustomPaper()
        'Dim rep = New ReportDocument()
        'Dim printerSettings = New System.Drawing.Printing.PrinterSettings()
        'Dim pSettings = New System.Drawing.Printing.PageSettings(printerSettings)
        'pSettings.PaperSize = New System.Drawing.Printing.PaperSize
        'pSettings.Margins = New System.Drawing.Printing.Margins(0, 0, 0, 0)
        '' rep.PrintOptions.DissociatePageSizeAndPrinterPaperSize = True
        'rep.PrintOptions.CopyFrom(printerSettings, pSettings)
    End Sub
    Sub ExportReport(ByRef reportName As String)
        '       Dim Rpt As New ReportDocument
        Dim cryRpt As New ReportDocument
        Dim CrTables As Tables
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        If Not File.Exists(Application.StartupPath & reportName) Then MsgBox("Report : " & reportName & " Not Exists.... Please Contact to Service Provider...", MsgBoxStyle.Critical, "Not Found...") : Me.Close() : Exit Sub
        Rpt.Load(Application.StartupPath & reportName)
        CrystalReportViewer1.ReportSource = cryRpt
        CrTables = Rpt.Database.Tables
        For Each CrTable In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next
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
        '  Rpt.ParameterFields.Add("HindiCompanyName1")
        Try
            Dim CrExportOptions As ExportOptions
            Dim CrDiskFileDestinationOptions As New  _
            DiskFileDestinationOptions()
            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
            CrDiskFileDestinationOptions.DiskFileName =
                                        "D:\crystalExport.pdf"
            CrExportOptions = Rpt.ExportOptions

            Dim dialog1 As SaveFileDialog = New SaveFileDialog()
            dialog1.Title = "Select PDFFile"
            dialog1.Filter = "PDF(*.pdf)|*.pdf"
            If dialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            End If
            With CrExportOptions
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
                .DestinationOptions = CrDiskFileDestinationOptions
                .FormatOptions = CrFormatTypeOptions
            End With
            Rpt.Export()
        Catch ex As Exception
            MsgBox(ex.ToString)
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

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ExportReport(reportName)
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        ExportReport(reportName)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        '  PrintDialog1.ShowDialog()
        PrintDirect(RptName)
    End Sub

    Private Sub BtnPrintO_Click(sender As Object, e As EventArgs) Handles BtnPrintO.Click
        PrintDirect(RptName)
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        CrystalReportViewer1.Refresh()
    End Sub
End Class