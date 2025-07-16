Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Public Class Pdf_Genrate
    Dim data As String = String.Empty
    Dim reportName As String

    Private Function GetData() As DataSet
        Dim sSql As String = "vacuum;Select * from Printing"
        Dim ds As New DataSet
        ds = ClsFunPrimary.ExecDataSet(sSql, "Printing")
        GetData = ds
    End Function
    Sub printReport(ByRef reportName As String)
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables
        Try
            Dim Rpt As New ReportDocument
            If Not File.Exists(Application.StartupPath & reportName) Then MsgBox("Report : " & reportName & " Not Exists.... Please Contact to Service Provider...", MsgBoxStyle.Critical, "Not Found...") : Me.Close() : Exit Sub
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
            intCounter = Rpt.DataDefinition.ParameterFields.Count

            'For Parameter fields
            'If intCounter > 0 And Trim(pParams) <> "" Then
            '    strParamenters = pParams
            '    strParValPair = strParamenters.Split("&")
            '    For index = 0 To UBound(strParValPair)
            '        If InStr(strParValPair(index), "=") > 0 Then
            '            strVal = strParValPair(index).Split("=")
            '            ' paraValue.Value = strVal(1)
            '            currValue = Rpt.DataDefinition.ParameterFields(strVal(0)).CurrentValues
            '            currValue.Add(paraValue)
            '            Rpt.DataDefinition.ParameterFields(strVal(0)).ApplyCurrentValues(currValue)
            '        End If
            '    Next
            'End If
            CrystalReportViewer1.ReportSource = Rpt
            CrystalReportViewer1.Refresh()
            CrTables.Dispose() ': Rpt.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    'Public Sub ExportReport(ByRef reportName As String)
    '    Dim Rpt As New ReportDocument
    '    Dim cryRpt As New ReportDocument
    '    Dim CrTables As Tables
    '    Dim crtableLogoninfo As New TableLogOnInfo
    '    Dim crConnectionInfo As New ConnectionInfo
    '    If Not File.Exists(Application.StartupPath & reportName) Then MsgBox("Report : " & reportName & " Not Exists.... Please Contact to Service Provider...", MsgBoxStyle.Critical, "Not Found...") : Me.Close() : Exit Sub
    '    Rpt.Load(Application.StartupPath & reportName)
    '    '   CrystalReportViewer1.ReportSource = cryRpt
    '    CrTables = Rpt.Database.Tables
    '    For Each CrTable In CrTables
    '        crtableLogoninfo = CrTable.LogOnInfo
    '        crtableLogoninfo.ConnectionInfo = crConnectionInfo
    '        CrTable.ApplyLogOnInfo(crtableLogoninfo)
    '    Next
    '    Dim dsCustomers As DataSet = GetData()
    '    Rpt.SetDataSource(dsCustomers.Tables("Printing"))
    '    Rpt.SetParameterValue("CompName", compname)
    '    Rpt.SetParameterValue("Mob1", Mob1)
    '    Rpt.SetParameterValue("Mob2", Mob2)
    '    Rpt.SetParameterValue("Address", Address)
    '    Rpt.SetParameterValue("City", City)
    '    Rpt.SetParameterValue("State", State)
    '    Rpt.SetParameterValue("AddressHindi", AddressHindi)
    '    Rpt.SetParameterValue("CityHindi", CityHindi)
    '    Rpt.SetParameterValue("StateHindi", StateHindi)
    '    Rpt.SetParameterValue("HindiCompanyName", compnameHindi)
    '    '  Rpt.ParameterFields.Add("HindiCompanyName1")
    '    Try
    '        Dim CrExportOptions As ExportOptions
    '        Dim CrDiskFileDestinationOptions As New  _
    '        DiskFileDestinationOptions()
    '        Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
    '        '--------Pdf Name-------------------
    '        GlobalData.PdfPath = Application.StartupPath & "\Pdfs\" & GlobalData.PdfName.Replace("/", "")
    '        CrExportOptions = Rpt.ExportOptions
    '        CrDiskFileDestinationOptions.DiskFileName = GlobalData.PdfPath
    '        With CrExportOptions
    '            .ExportDestinationType = ExportDestinationType.DiskFile
    '            .ExportFormatType = ExportFormatType.PortableDocFormat
    '            .DestinationOptions = CrDiskFileDestinationOptions
    '            .FormatOptions = CrFormatTypeOptions
    '        End With
    '        Rpt.Export()
    '        CrTables.Dispose() : Rpt.Close()
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub

    Public Sub ExportReport(ByRef reportName As String)
        Try
            Dim Rpt As New ReportDocument
            If Not File.Exists(Application.StartupPath & reportName) Then
                MsgBox("Report : " & reportName & " Not Exists.... Please Contact the Service Provider...", MsgBoxStyle.Critical, "Not Found...")
                Exit Sub
            End If

            Rpt.Load(Application.StartupPath & reportName)

            ' If the database login is not needed, skip this part for speed.
            Dim CrTables As Tables = Rpt.Database.Tables
            Dim crConnectionInfo As New ConnectionInfo()
            For Each CrTable As Table In CrTables
                Dim crtableLogoninfo As TableLogOnInfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            ' Retrieve only necessary data.
            Dim dsCustomers As DataSet = GetData()
            Rpt.SetDataSource(dsCustomers.Tables("Printing"))

            ' Set all parameters.
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
            ' Export to PDF.
            Dim CrExportOptions As ExportOptions = Rpt.ExportOptions
            Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
            Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
            ' PDF Path.
            GlobalData.PdfPath = Application.StartupPath & "\Pdfs\" & GlobalData.PdfName.Replace("/", "")
            CrDiskFileDestinationOptions.DiskFileName = GlobalData.PdfPath
            With CrExportOptions
                .ExportDestinationType = ExportDestinationType.DiskFile
                .ExportFormatType = ExportFormatType.PortableDocFormat
                .DestinationOptions = CrDiskFileDestinationOptions
                .FormatOptions = CrFormatTypeOptions
            End With

            ' Export the report.
            Rpt.Export()

            ' Clean up.
            CrTables.Dispose()
            Rpt.Close()
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub

    Private Sub Report_Viewer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Private Sub Report_Viewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0
        Me.Left = 0
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
End Class