Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class Ugrahi_Viewer

    'Dim data As String = Application.StartupPath & "\Product.xml"
    Private Function GetData() As DataSet
        Dim product As String = Application.StartupPath & "\Product.xml"
        Dim ds As New DataSet
        ds.ReadXml(product)
        'Dim sSql As String = "Select * From Product" 'ds.Tables("product").ToString
        ' ''clsFun.changeCompany()
        'ds = clsFun.ExecDataSet(sSql, "Product")
        GetData = ds
    End Function

    Private Sub Report_Viewer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'data = MainScreenForm.tssDbpath.Text
        'If data <> "" Then
        '    isCompanyOpen = True
        '    clsFun.ChangePath("Data\" & data)
        'End If
        'Me.Dispose()
    End Sub
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    Private Sub Report_Viewer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
    Sub printReport(ByRef reportname As String, Optional ByRef entrydate As String = "", Optional ByRef frmdate As String = "", Optional ByRef todate As String = "", Optional ByRef coldate As String = "")
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables
        Dim dt1 As String = ""
        Dim dt2 As String = ""
        Dim dt3 As String = ""

        Dim dt4 As String = ""
        Dim dt5 As String = ""
        Dim dt6 As String = ""

        Dim dt7 As String = ""
        Dim dt8 As String = ""
        Dim dt9 As String = ""

        Dim dt10 As String = ""
        Dim dt11 As String = ""

        If Ugrahi_Report.CbDays.SelectedIndex = 0 Then
            If coldate.Split(",")(0).ToString() <> "" Then
                dt1 = coldate.Split(",")(0).ToString()
            Else
                dt1 = ""
            End If
        ElseIf Ugrahi_Report.CbDays.SelectedIndex = 1 Then
            If coldate.Split(",")(0).ToString() <> "" Then
                dt1 = coldate.Split(",")(0).ToString()
            Else
                dt1 = ""
            End If
            If coldate.Split(",")(1).ToString() <> "" Then
                dt2 = coldate.Split(",")(1).ToString()
            Else
                dt2 = ""
            End If
        ElseIf Ugrahi_Report.CbDays.SelectedIndex = 2 Then
            If coldate.Split(",")(0).ToString() <> "" Then
                dt1 = coldate.Split(",")(0).ToString()
            Else
                dt1 = ""
            End If
            If coldate.Split(",")(1).ToString() <> "" Then
                dt2 = coldate.Split(",")(1).ToString()
            Else
                dt2 = ""
            End If
            If coldate.Split(",")(2).ToString() <> "" Then
                dt3 = coldate.Split(",")(2).ToString()
            Else
                dt3 = ""
            End If
        ElseIf Ugrahi_Report.CbDays.SelectedIndex = 3 Then
            If coldate.Split(",")(0).ToString() <> "" Then
                dt1 = coldate.Split(",")(0).ToString()
            Else
                dt1 = ""
            End If
            If coldate.Split(",")(1).ToString() <> "" Then
                dt2 = coldate.Split(",")(1).ToString()
            Else
                dt2 = ""
            End If
            If coldate.Split(",")(2).ToString() <> "" Then
                dt3 = coldate.Split(",")(2).ToString()
            Else
                dt3 = ""
            End If
            If coldate.Split(",")(3).ToString() <> "" Then
                dt4 = coldate.Split(",")(3).ToString()
            Else
                dt4 = ""
            End If
            If coldate.Split(",")(4).ToString() <> "" Then
                dt5 = coldate.Split(",")(4).ToString()
            Else
                dt5 = ""
            End If
            If coldate.Split(",")(5).ToString() <> "" Then
                dt6 = coldate.Split(",")(4).ToString()
            Else
                dt6 = ""
            End If
            If coldate.Split(",")(6).ToString() <> "" Then
                dt7 = coldate.Split(",")(6).ToString()
            Else
                dt7 = ""
            End If
        End If


        Try
            Dim Rpt As New ReportDocument
            Rpt.Load(Application.StartupPath & reportname)
            CrTables = Rpt.Database.Tables
            For Each CrTable In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next
            Dim dsCustomers As DataSet = GetData()
            Rpt.SetDataSource(dsCustomers.Tables("Product"))
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
            Rpt.SetParameterValue("EntryDate", entrydate)
            Rpt.SetParameterValue("FromDate", frmdate)
            Rpt.SetParameterValue("Todate", todate)
            Rpt.SetParameterValue("Date1", dt1)
            Rpt.SetParameterValue("Date2", dt2)
            Rpt.SetParameterValue("Date3", dt3)
            Rpt.SetParameterValue("Date4", dt4)
            Rpt.SetParameterValue("Date5", dt5)
            Rpt.SetParameterValue("Date6", dt6)
            If Ugrahi_Report.CbDays.SelectedIndex = 3 Then
                Rpt.SetParameterValue("Date7", dt7)
            End If

            CrystalReportViewer1.ReportSource = Rpt
            CrystalReportViewer1.Refresh()
            CrTables.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


  
    Private Sub Registers_Viewer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Top = 0 : Me.Left = 0
        Me.BackColor = Color.GhostWhite
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
    End Sub

  
End Class