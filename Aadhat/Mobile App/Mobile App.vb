Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports Aadhat.HttpService


Public Class Mobile_App
    Dim ClsCommon As CommonClass = New CommonClass()
    Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
    Private Sub BtnIDGenrate_Click(sender As Object, e As EventArgs) Handles BtnIDGenrate.Click
        If My.Computer.Network.IsAvailable = False Then MsgBox("Please Check Your Internet Connection", MsgBoxStyle.Critical, "No Internet Access") : Exit Sub
        '  If Not File.Exists(fileName) Then MsgBox("In DEMO MODE Mobile App Feature Not Activated..." & vbNewLine & "If Do you want to See Mobile App Feature " & vbNewLine & " Please Use OrganizationID=123456 and Password Is 123456 In Mobile App", MsgBoxStyle.Exclamation, "Thank You...") : Exit Sub
        BtnIDGenrate.BackColor = Color.DarkTurquoise
        ClsCommon.GenrateID()
    End Sub

    Private Sub saveUpdateInfo()
        btnCustom.Visible = False
        Application.DoEvents()
        If ClsCommon.IsInternetConnect Then
            UpdateRecord()
            ' ClsCommon.UpdateCompanyInfo("ADD")
            Dim httpservice As HttpService = New HttpService()
            ''''''
            ''''Company Data
            ''''
            '           httpservice.Authenticate("123456", "123456")
            Dim OrgID As Integer = 0
            Dim ComPSql As String = "Select * From Company"
            Dim compdt As DataTable = New DataTable()
            compdt = clsFun.ExecDataTable(ComPSql)
            Dim companyRequest As CompanyRequest = New CompanyRequest()
            For Each item As DataRow In compdt.Rows
                Application.DoEvents()
                lblProgress.Text = "Company Data..."
                companyRequest.Address = item.Field(Of String)("Address")
                companyRequest.City = item.Field(Of String)("City")
                companyRequest.CompanyID = 0
                companyRequest.CompanyName = item.Field(Of String)("CompanyName")
                companyRequest.CompData = item.Field(Of String)("CompData")
                companyRequest.DealsIN = item.Field(Of String)("DealsIN")
                companyRequest.EmailID = item.Field(Of String)("EmailID")
                companyRequest.FaxNo = item.Field(Of String)("FaxNo")
                companyRequest.GSTN = item.Field(Of String)("GSTN")
                companyRequest.IsActive = True
                companyRequest.Marka = item.Field(Of String)("Marka")
                companyRequest.MobileNo1 = item.Field(Of String)("MobileNo1")
                companyRequest.MobileNo2 = item.Field(Of String)("MobileNo2")
                companyRequest.OrganizationID = item.Field(Of Int64)("OrganizationID")
                companyRequest.Other = item.Field(Of String)("Other")
                companyRequest.PanNo = item.Field(Of String)("PanNO")
                companyRequest.Password = item.Field(Of String)("Password")
                companyRequest.PhoneNo = item.Field(Of String)("PhoneNo")
                companyRequest.PrintOtheraddress = Utf8ToUnicode(item.Field(Of String)("PrintOtheraddress"))
                companyRequest.PrintOtherCity = Utf8ToUnicode(item.Field(Of String)("PrintOtherCity"))
                companyRequest.PrintOtherName = Utf8ToUnicode(item.Field(Of String)("PrintOtherName"))
                companyRequest.PrintOtherState = Utf8ToUnicode(item.Field(Of String)("PrintOtherState"))
                companyRequest.RegistrationNo = item.Field(Of String)("RegistrationNo")
                companyRequest.State = item.Field(Of String)("State")
                companyRequest.tag = item.Field(Of String)("tag")
                companyRequest.Website = item.Field(Of String)("Website")
                companyRequest.YearStart = CDate(item.Field(Of Date)("YearStart")).ToString("yyyy-MM-dd")
                companyRequest.Yearend = CDate(item.Field(Of Date)("Yearend")).ToString("yyyy-MM-dd")
                OrgID = item.Field(Of Int64)("OrganizationID")
            Next
            Dim companyResp As CompanyResponse = httpservice.SendCompany(companyRequest)
            ' MsgBox("Passed")
            Dim loginResp As LoginResponse = httpservice.Authenticate(txtCompanyID.Text, txtPassword.Text)
            'MsgBox(loginResp.Token)

            '''' Account Table Insert
            Dim accSql As String = "Select * FROM Accounts"
            Dim accDt As DataTable = New DataTable()
            accDt = clsFun.ExecDataTable(accSql)
            Dim accRqst As AddAccountRequest = New AddAccountRequest()
            For Each item As DataRow In accDt.Rows
                Application.DoEvents()
                lblProgress.Text = "Accounts Data..."
                Dim acc As AccountRequest = New AccountRequest()
                acc.AccountId = item.Field(Of Int64)("ID")
                acc.AccountName = item.Field(Of String)("AccountName").Trim
                acc.GroupID = item.Field(Of Int64)("GroupID")
                'If item.Field(Of Int64)("GroupID") = 32 Then MsgBox("a")
                acc.DC = item.Field(Of String)("DC")
                acc.Tag = item.Field(Of String)("Tag")
                acc.OpBal = item.Field(Of Decimal)("OpBal")
                acc.OtherName = Utf8ToUnicode(item.Field(Of String)("OtherName"))
                acc.Address = item.Field(Of String)("Address")
                acc.LFNo = item.Field(Of String)("LFNo")
                acc.Area = item.Field(Of String)("Area")
                acc.City = item.Field(Of String)("City")
                acc.AccNo = item.Field(Of String)("State")
                acc.Phone = item.Field(Of String)("Phone")
                acc.Contact = item.Field(Of String)("Contact")
                acc.Mobile1 = item.Field(Of String)("Mobile1")
                acc.Mobile2 = item.Field(Of String)("Mobile2")
                acc.MailID = item.Field(Of String)("MailID")
                acc.BankName = item.Field(Of String)("BankName")
                acc.AccNo = item.Field(Of String)("AccNo")
                acc.IFSC = item.Field(Of String)("IFSC")
                acc.GName = item.Field(Of String)("GName")
                acc.Gmobile1 = item.Field(Of String)("Gmobile1")
                acc.Gmobile2 = item.Field(Of String)("Gmobile2")
                acc.Gaddress = item.Field(Of String)("Gaddress")
                acc.GCity = item.Field(Of String)("GCity")
                acc.Gstate = item.Field(Of String)("Gstate")
                acc.Limit = item.Field(Of String)("Limit")
                acc.AccountPhoto = ""
                acc.Gphoto = ""
                acc.OrganizationId = OrgID
                accRqst.Accounts.Add(acc)
            Next
            Dim httppservice As HttpService = New HttpService()
            Dim accResp As SaveAccountResponse = httpservice.SendAccountData(accRqst, loginResp.Token)


            ' '''' Account Table Insert
            '' Account Table Insert
            'Dim accSql As String = "SELECT COUNT(*) FROM Accounts"
            'Dim accountCount As Integer = clsFun.ExecScalarInt(accSql) ' Total records count
            'Dim progressCount As Integer = 0
            'Dim maxRowCount As Integer = Math.Ceiling(accountCount / 1000) ' Total batches

            'dataProgress.Minimum = 0
            'dataProgress.Maximum = maxRowCount
            'dataProgress.Visible = True
            'lblProgress.Visible = True

            'Dim processedRecords As Integer = 0
            'Dim batchSize As Integer = 500

            'Dim httppservice As HttpService = New HttpService()

            'For i As Integer = 0 To maxRowCount - 1
            '    Application.DoEvents()

            '    ' Batch-wise SQL Query
            '    Dim batchSql As String = "SELECT * FROM Accounts ORDER BY AccountName LIMIT 500 OFFSET " & processedRecords.ToString()
            '    Dim accDt As DataTable = clsFun.ExecDataTable(batchSql)

            '    progressCount += 1
            '    If accDt.Rows.Count > 0 Then
            '        processedRecords += accDt.Rows.Count
            '        Dim accRqst As New AddAccountRequest()

            '        'If i = 0 Then
            '        '    accRqst.IsFirstRow = True
            '        'Else
            '        '    accRqst.IsFirstRow = False
            '        'End If

            '        dataProgress.Step = 1
            '        dataProgress.Value = progressCount

            '        For Each item As DataRow In accDt.Rows
            '            Application.DoEvents()
            '            lblProgress.Text = "Processing Accounts... " & processedRecords & ""
            '            Dim acc As New AccountRequest()
            '            acc.AccountId = item.Field(Of Int64)("ID")
            '            acc.AccountName = item.Field(Of String)("AccountName").Trim
            '            acc.GroupID = item.Field(Of Int64)("GroupID")
            '            acc.DC = item.Field(Of String)("DC")
            '            acc.Tag = item.Field(Of String)("Tag")
            '            acc.OpBal = item.Field(Of Decimal)("OpBal")
            '            acc.OtherName = Utf8ToUnicode(item.Field(Of String)("OtherName")).Trim
            '            acc.Address = item.Field(Of String)("Address")
            '            acc.LFNo = item.Field(Of String)("LFNo")
            '            acc.Area = item.Field(Of String)("Area")
            '            acc.City = item.Field(Of String)("City")
            '            acc.AccNo = item.Field(Of String)("State")
            '            acc.Phone = item.Field(Of String)("Phone")
            '            acc.Contact = item.Field(Of String)("Contact")
            '            acc.Mobile1 = item.Field(Of String)("Mobile1")
            '            acc.Mobile2 = item.Field(Of String)("Mobile2")
            '            acc.MailID = item.Field(Of String)("MailID")
            '            acc.BankName = item.Field(Of String)("BankName")
            '            acc.AccNo = item.Field(Of String)("AccNo")
            '            acc.IFSC = item.Field(Of String)("IFSC")
            '            acc.GName = item.Field(Of String)("GName")
            '            acc.Gmobile1 = item.Field(Of String)("Gmobile1")
            '            acc.Gmobile2 = item.Field(Of String)("Gmobile2")
            '            acc.Gaddress = item.Field(Of String)("Gaddress")
            '            acc.GCity = item.Field(Of String)("GCity")
            '            acc.Gstate = item.Field(Of String)("Gstate")
            '            acc.Limit = item.Field(Of String)("Limit")
            '            acc.AccountPhoto = ""
            '            acc.Gphoto = ""
            '            acc.OrganizationId = OrgID
            '            'acc.ServerTag = 0
            '            accRqst.Accounts.Add(acc)
            '        Next

            '        ' API Call to Send Data
            '        Dim accResp As SaveAccountResponse = httppservice.SendAccountData(accRqst, loginResp.Token)

            '        ' Optional Delay to Prevent Overloading API
            '        ' System.Threading.Thread.Sleep(500)
            '    End If
            '    dataProgress.Value = 0
            'Next

            'lblProgress.Text = "All accounts processed successfully!"


            Dim AccGrpSql As String = "Select * FROM AccountGroup"
            Dim AccgrpDt As DataTable = New DataTable()
            AccgrpDt = clsFun.ExecDataTable(AccGrpSql)
            Dim AccgrpRqst As AddAccountGroupRequest = New AddAccountGroupRequest()
            For Each item As DataRow In AccgrpDt.Rows
                Application.DoEvents()
                lblProgress.Text = "Groups Data..."
                Dim accgrp As AccountGroupRequest = New AccountGroupRequest()
                accgrp.GroupId = item.Field(Of Int64)("ID")
                accgrp.GroupName = item.Field(Of String)("GroupName").Trim
                accgrp.UnderGroupID = item.Field(Of Int64)("UnderGroupID")
                accgrp.UnderGroupName = item.Field(Of String)("UnderGroupName").Trim
                accgrp.DC = item.Field(Of String)("DC")
                accgrp.Primary2 = item.Field(Of String)("Primary2")
                accgrp.Tag = item.Field(Of String)("Tag")
                accgrp.OrganizationId = OrgID 'item.Field(Of Int64)("OrganizationId")
                AccgrpRqst.AccountGroups.Add(accgrp)
            Next
            Dim AccGrpResp As AccountGroupResponse = New HttpService().SendAccountGroup(AccgrpRqst, loginResp.Token)
            'Dim AccGrpResp As AccountGroupResponse = New HttpService().SendAccountGroup(AccgrpRqst)
            Dim Cratesql As String = "Select * FROM CrateMarka"
            Dim crateDt As DataTable = New DataTable()
            crateDt = clsFun.ExecDataTable(Cratesql)
            Dim CrateRqst As SaveCrateMarkaRequest = New SaveCrateMarkaRequest()
            CrateRqst.OrgId = Val(txtCompanyID.Text)
            For Each item As DataRow In crateDt.Rows
                Application.DoEvents()
                lblProgress.Text = "Marka Data..."
                Dim Crtgrp As CrateMarka = New CrateMarka()
                Crtgrp.CrateID = item.Field(Of Int64)("ID")
                Crtgrp.MarkaName = item.Field(Of String)("MarkaName").Trim
                Crtgrp.OpQty = item.Field(Of Decimal)("OpQty")
                Crtgrp.Rate = item.Field(Of Decimal)("Rate")
                Crtgrp.OrgID = OrgID
                CrateRqst.CrateMarkas.Add(Crtgrp)
            Next
            Dim crateRsp As SaveCrateMarkaResponse = New HttpService().sendcratemarka(CrateRqst, loginResp.Token)
            'Dim crateRsp As SaveCrateMarkaResponse = New HttpService().sendcratemarka(CrateRqst)
            '''' Ledger Talbe Insert

            'Dim totLegCount As Integer = 0

            Dim sql As String = "Select COUNT(*) FROM Ledger"
            Dim ledgerCount As Integer = clsFun.ExecScalarInt(sql)
            Dim progesssCount As Integer = 0
            Dim maxRowCount1 As Decimal = 0
            maxRowCount1 = Math.Ceiling(ledgerCount / 2000)
            dataProgress.Minimum = progesssCount
            dataProgress.Maximum = maxRowCount1
            dataProgress.Visible = True
            lblProgress.Visible = True
            Dim legCount As Integer = 0

            For i As Integer = 0 To maxRowCount1 - 1
                Application.DoEvents()
                Dim ledSql As String = "Select VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi" +
                                    " FROM Ledger LIMIT 2000 OFFSET " + legCount.ToString()
                '  If Val(legCount) = 28700 Then MsgBox("a")
                Dim ledDt As DataTable = New DataTable()
                ledDt = clsFun.ExecDataTable(ledSql)
                progesssCount = progesssCount + 1
                If ledDt.Rows.Count() > 0 Then
                    legCount = legCount + ledDt.Rows.Count()
                    Dim ledgerRqst As LedgerRequest = New LedgerRequest()
                    If i = 0 Then
                        ledgerRqst.IsFirstRow = True
                    Else
                        ledgerRqst.IsFirstRow = False
                    End If
                    'dataProgress.Maximum = maxRowCount 'Set Max Lenght
                    dataProgress.Step = 1 'Set Step
                    dataProgress.Value = progesssCount
                    For Each item As DataRow In ledDt.Rows
                        Application.DoEvents()
                        lblProgress.Text = "Ledger Data... " & legCount & ""
                        dataProgress.Value = i
                        Dim ledger As LedgerData = New LedgerData()
                        ledger.AccountID = item.Field(Of Int64)("AccountID")
                        ledger.VourchersID = item.Field(Of Int64)("VourchersID")
                        ledger.EntryDate = item.Field(Of Date)("EntryDate")
                        ledger.TransType = item.Field(Of String)("TransType")
                        ledger.AccountName = item.Field(Of String)("AccountName").Trim
                        ledger.Amount = item.Field(Of Decimal)("Amount")
                        ledger.DC = item.Field(Of String)("DC")
                        ledger.Remark = Utf82Hebrew(item.Field(Of String)("Remark")).Trim
                        ledger.Remark2 = Utf82Hebrew(IIf(item.Field(Of String)("RemarkHindi") = Nothing, "", item.Field(Of String)("RemarkHindi"))).Trim
                        ' ledger.Remark2 = IIf(item.Field(Of String)("RemarkHindi") Is Nothing, "", item.Field(Of String)("RemarkHindi")).Trim()
                        ledger.Narration = Utf8ToUnicode(item.Field(Of String)("Narration")).Trim
                        ledger.OrganizationId = OrgID 'item.Field(Of Int64)("OrganizationId")

                        ledgerRqst.Ledgers.Add(ledger)
                    Next
                    Dim ledgerResp As SaveLedgerResponse = New HttpService().SendLedgerData(ledgerRqst, loginResp.Token)
                    'Dim ledgerResp As SaveLedgerResponse = New HttpService().SendLedgerData(ledgerRqst)

                End If
                dataProgress.Value = 0
            Next

            '''' Crate Talbe Insert

            'Dim totLegCount As Integer = 0


            Dim Ssql As String = "Select COUNT(*) FROM CrateVoucher"
            Dim CrateCount As Integer = clsFun.ExecScalarInt(Ssql)
            Dim CrateprogesssCount As Integer = 0
            Dim CratemaxRowCount As Decimal = 0
            CratemaxRowCount = Math.Ceiling(CrateCount / 2000)
            dataProgress.Minimum = CrateprogesssCount
            dataProgress.Maximum = CratemaxRowCount
            dataProgress.Visible = True
            lblProgress.Visible = True
            Dim CrateVCount As Integer = 0
            For i As Integer = 0 To CratemaxRowCount - 1

                Dim CrateVSql As String = "Select * " +
                                    " FROM CrateVoucher LIMIT 2000 OFFSET " + CrateVCount.ToString()
                Dim CrateVDt As DataTable = New DataTable()
                CrateVDt = clsFun.ExecDataTable(CrateVSql)
                'dataProgress.Maximum = maxRowCount 'Set Max Lenght
                dataProgress.Step = 1 'Set Step
                progesssCount = progesssCount + 1
                If CrateVDt.Rows.Count() > 0 Then
                    CrateVCount = CrateVCount + CrateVDt.Rows.Count()
                    Dim CratevReqst As CrateVoucherRequest = New CrateVoucherRequest()
                    If i = 0 Then
                        CratevReqst.IsFirstRow = True
                    Else
                        CratevReqst.IsFirstRow = False
                    End If
                    dataProgress.Maximum = CratemaxRowCount 'Set Max Lenght
                    dataProgress.Step = 1 'Set Step
                    ' dataProgress.Value = progesssCount
                    CratevReqst.OrgId = Val(txtCompanyID.Text)
                    For Each item As DataRow In CrateVDt.Rows
                        Application.DoEvents()
                        lblProgress.Text = "Crate Voucher Data... " & CrateVCount & ""
                        dataProgress.Value = i
                        Dim CrateV As SaveCrateVoucherRequest = New SaveCrateVoucherRequest()
                        CrateV.OrganizationId = txtCompanyID.Text
                        CrateV.SlipNo = item.Field(Of String)("SlipNo")
                        CrateV.VoucherID = item.Field(Of Int64)("VoucherID")
                        CrateV.EntryDate = item.Field(Of Date)("EntryDate")
                        CrateV.TransType = item.Field(Of String)("TransType")
                        CrateV.AccountID = Val(item.Field(Of Int64)("AccountID"))
                        CrateV.AccountName = item.Field(Of String)("AccountName").Trim
                        CrateV.CrateType = item.Field(Of String)("CrateType")
                        CrateV.CrateID = item.Field(Of Int64)("CrateID")
                        CrateV.CrateName = item.Field(Of String)("CrateName").Trim
                        CrateV.Qty = item.Field(Of Int64)("Qty")
                        CrateV.Remark = Utf8ToUnicode(item.Field(Of String)("Remark")).Trim
                        ' CrateV.Rate = Convert.ToDecimal(Format(CrateV.Rate, "0.00"))
                        CrateV.Rate = item.Field(Of Decimal)("Rate")
                        'CrateV.Amount = Convert.ToDecimal(Format(CrateV.Amount, "0.00"))
                        CrateV.Amount = item.Field(Of Decimal)("Amount")
                        CrateV.CashPaid = item.Field(Of String)("CashPaid")
                        'item.Field(Of Int64)("OrganizationId")
                        CratevReqst.CrateVouchers.Add(CrateV)
                    Next
                    Dim CrateVResp As SaveCrateVoucherResponse = New HttpService().sendcrateVoucher(CratevReqst, loginResp.Token)
                    'Dim CrateVResp As SaveCrateVoucherResponse = New HttpService().sendcrateVoucher(CratevReqst)
                End If
                dataProgress.Value = 0
            Next
            ' Update last Sync
            Dim syncDateTimeResp As UpdateLastDataSyncDateTimeResponse = httpservice.UpdateLastDataSyncDateTime(loginResp.Token)
            ' Dim syncDateTimeResp As UpdateLastDataSyncDateTimeResponse = httpservice.UpdateLastDataSyncDateTime(Val(txtCompanyID.Text))
            lblProgress.Visible = False
            dataProgress.Visible = False
            MsgBox("Record Updated on Server Successfully", vbInformation, "Sucessful")
            clsFun.ExecScalarStr("Update  Company Set SyncDate='" & DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") & "'")
            lblLastUpdate.Text = clsFun.ExecScalarStr("Select SyncDate From Company")
            ClsFunserver.ExecScalarStr("Delete From AccountGroup Where OrgID='" & Val(OrgID) & "'; " &
                                  "Delete From Accounts Where OrgID='" & Val(OrgID) & "'; " &
                                  "Delete From CrateMarka Where OrgID='" & Val(OrgID) & "'; " &
                                  "Delete From CrateVoucher Where OrgID='" & Val(OrgID) & "'; " &
                                   "Delete From Transaction2 Where OrgID='" & Val(OrgID) & "'; " &
                                  "Delete From Ledger Where OrgID='" & Val(OrgID) & "'; ")
            clsFun.ExecScalarStr("Update Company Set Autosync='Y'")
            If clsFun.ExecScalarStr("Select Autosync From Company") = "Y" Then
                btnCustom.Visible = True : btnCustom.BringToFront()
            Else
                btnCustom.Visible = True : btnCustom.BringToFront()
            End If
        Else
            MessageBox.Show("Sorry... Your System is not Connected to Internet. Please check internet connection...", "Internet Required...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Exit Sub
        End If
        'Else
        'If ClsCommon.IsInternetConnect Then
        '    UpdateRecord : ClsCommon.UpdateCutomerInfo("EDIT")
        'Else
        '    MessageBox.Show("Sorry... Your System is not Connected to Internet. Please check internet connection...", "Internet Required...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Exit Sub
        'End If
        'End If
        btnCustom.Visible = True
    End Sub


    Private Sub CustomUpdate()
        ckFullSync.Visible = False
        btnCustom.Visible = False
        Application.DoEvents()
        If ClsCommon.IsInternetConnect Then
            UpdateRecord()
            ' ClsCommon.UpdateCompanyInfo("ADD")
            Dim httpservice As HttpService = New HttpService()
            ''''''
            ''''Company Data
            ''''
            Dim OrgID As Integer = Val(OrgID)
            Dim ComPSql As String = "Select * FROM Company"
            Dim compdt As DataTable = New DataTable()
            compdt = clsFun.ExecDataTable(ComPSql)
            Dim companyRequest As CompanyRequest = New CompanyRequest()
            For Each item As DataRow In compdt.Rows
                Application.DoEvents()
                lblProgress.Text = "Company Data..."
                companyRequest.Address = item.Field(Of String)("Address")
                companyRequest.City = item.Field(Of String)("City")
                companyRequest.CompanyID = 0
                companyRequest.CompanyName = item.Field(Of String)("CompanyName").Trim
                companyRequest.CompData = item.Field(Of String)("CompData")
                companyRequest.DealsIN = item.Field(Of String)("DealsIN")
                companyRequest.EmailID = item.Field(Of String)("EmailID")
                companyRequest.FaxNo = item.Field(Of String)("FaxNo")
                companyRequest.GSTN = item.Field(Of String)("GSTN")
                companyRequest.IsActive = True
                companyRequest.Marka = item.Field(Of String)("Marka")
                companyRequest.MobileNo1 = item.Field(Of String)("MobileNo1")
                companyRequest.MobileNo2 = item.Field(Of String)("MobileNo2")
                companyRequest.OrganizationID = item.Field(Of Int64)("OrganizationID")
                companyRequest.Other = item.Field(Of String)("Other")
                companyRequest.PanNo = item.Field(Of String)("PanNO")
                companyRequest.Password = item.Field(Of String)("Password")
                companyRequest.PhoneNo = item.Field(Of String)("PhoneNo")
                companyRequest.PrintOtheraddress = Utf8ToUnicode(item.Field(Of String)("PrintOtheraddress"))
                companyRequest.PrintOtherCity = Utf8ToUnicode(item.Field(Of String)("PrintOtherCity"))
                companyRequest.PrintOtherName = Utf8ToUnicode(item.Field(Of String)("PrintOtherName"))
                companyRequest.PrintOtherState = Utf8ToUnicode(item.Field(Of String)("PrintOtherState"))
                companyRequest.RegistrationNo = item.Field(Of String)("RegistrationNo")
                companyRequest.State = item.Field(Of String)("State")
                companyRequest.tag = item.Field(Of String)("tag")
                companyRequest.Website = item.Field(Of String)("Website")
                companyRequest.YearStart = CDate(item.Field(Of Date)("YearStart")).ToString("yyyy-MM-dd")
                companyRequest.Yearend = CDate(item.Field(Of Date)("Yearend")).ToString("yyyy-MM-dd")
                OrgID = item.Field(Of Int64)("OrganizationID")
            Next
            Dim companyResp As CompanyResponse = httpservice.SendCompany(companyRequest)
            Dim loginResp As LoginResponse = httpservice.Authenticate(txtCompanyID.Text, txtPassword.Text)
            'Dim companyResp As CompanyResponse = httpservice.SendCompany(companyRequest)

            ' '''' Account Table Insert
            'ServerType = 1
            'Dim accSql As String = "Select * FROM Accounts Where OrgID=" & Val(OrgID) & ""
            'Dim accDt As DataTable = New DataTable()
            'accDt = ClsFunserver.ExecDataTable(accSql)
            'Dim accRqst As AddAccountRequest = New AddAccountRequest()
            'For Each item As DataRow In accDt.Rows
            '    Application.DoEvents()
            '    lblProgress.Text = "Accounts Data..."
            '    Dim acc As AccountRequest = New AccountRequest()
            '    acc.AccountId = item.Field(Of Int64)("ID")
            '    acc.AccountName = item.Field(Of String)("AccountName")
            '    acc.GroupID = item.Field(Of Int64)("GroupID")
            '    If item.Field(Of Int64)("GroupID") = 32 Then MsgBox("a")
            '    acc.DC = item.Field(Of String)("DC")
            '    acc.Tag = item.Field(Of String)("Tag")
            '    acc.OpBal = item.Field(Of Decimal)("OpBal")
            '    acc.OtherName = Utf8ToUnicode(item.Field(Of String)("OtherName"))
            '    acc.Address = item.Field(Of String)("Address")
            '    acc.LFNo = item.Field(Of String)("LFNo")
            '    acc.Area = item.Field(Of String)("Area")
            '    acc.City = item.Field(Of String)("City")
            '    acc.AccNo = item.Field(Of String)("State")
            '    acc.Phone = item.Field(Of String)("Phone")
            '    acc.Contact = item.Field(Of String)("Contact")
            '    acc.Mobile1 = item.Field(Of String)("Mobile1")
            '    acc.Mobile2 = item.Field(Of String)("Mobile2")
            '    acc.MailID = item.Field(Of String)("MailID")
            '    acc.BankName = item.Field(Of String)("BankName")
            '    acc.AccNo = item.Field(Of String)("AccNo")
            '    acc.IFSC = item.Field(Of String)("IFSC")
            '    acc.GName = item.Field(Of String)("GName")
            '    acc.Gmobile1 = item.Field(Of String)("Gmobile1")
            '    acc.Gmobile2 = item.Field(Of String)("Gmobile2")
            '    acc.Gaddress = item.Field(Of String)("Gaddress")
            '    acc.GCity = item.Field(Of String)("GCity")
            '    acc.Gstate = item.Field(Of String)("Gstate")
            '    acc.Limit = item.Field(Of String)("Limit")
            '    acc.AccountPhoto = ""
            '    acc.Gphoto = ""
            '    acc.Type = 1
            '    acc.ServerTag = item.Field(Of Int64)("ServerTag")
            '    acc.OrganizationId = OrgID
            '    accRqst.Accounts.Add(acc)
            'Next
            'Dim httppservice As HttpService = New HttpService()
            'If accDt.Rows.Count <> 0 Then
            '    accRqst.Type = 1
            '    Dim accResp As SaveAccountResponse = httpservice.SendAccountData(accRqst)
            'End If


            '''' Account Table Insert
            Dim accSql As String = "Select * FROM Accounts"
            Dim accDt As DataTable = New DataTable()
            accDt = clsFun.ExecDataTable(accSql)
            Dim accRqst As AddAccountRequest = New AddAccountRequest()
            For Each item As DataRow In accDt.Rows
                Application.DoEvents()
                lblProgress.Text = "Accounts Data..."
                Dim acc As AccountRequest = New AccountRequest()
                acc.AccountId = item.Field(Of Int64)("ID")
                acc.AccountName = item.Field(Of String)("AccountName").Trim
                acc.GroupID = item.Field(Of Int64)("GroupID")
                'If item.Field(Of Int64)("GroupID") = 32 Then MsgBox("a")
                acc.DC = item.Field(Of String)("DC")
                acc.Tag = item.Field(Of String)("Tag")
                acc.OpBal = item.Field(Of Decimal)("OpBal")
                acc.OtherName = Utf8ToUnicode(item.Field(Of String)("OtherName")).Trim
                acc.Address = item.Field(Of String)("Address")
                acc.LFNo = item.Field(Of String)("LFNo")
                acc.Area = item.Field(Of String)("Area")
                acc.City = item.Field(Of String)("City")
                acc.AccNo = item.Field(Of String)("State")
                acc.Phone = item.Field(Of String)("Phone")
                acc.Contact = item.Field(Of String)("Contact")
                acc.Mobile1 = item.Field(Of String)("Mobile1")
                acc.Mobile2 = item.Field(Of String)("Mobile2")
                acc.MailID = item.Field(Of String)("MailID")
                acc.BankName = item.Field(Of String)("BankName")
                acc.AccNo = item.Field(Of String)("AccNo")
                acc.IFSC = item.Field(Of String)("IFSC")
                acc.GName = item.Field(Of String)("GName")
                acc.Gmobile1 = item.Field(Of String)("Gmobile1")
                acc.Gmobile2 = item.Field(Of String)("Gmobile2")
                acc.Gaddress = item.Field(Of String)("Gaddress")
                acc.GCity = item.Field(Of String)("GCity")
                acc.Gstate = item.Field(Of String)("Gstate")
                acc.Limit = item.Field(Of String)("Limit")
                acc.AccountPhoto = ""
                acc.Gphoto = ""
                acc.OrganizationId = OrgID
                accRqst.Accounts.Add(acc)
            Next
            Dim httppservice As HttpService = New HttpService()
            Dim accResp As SaveAccountResponse = httpservice.SendAccountData(accRqst, loginResp.Token)

            Dim AccGrpSql As String = "Select * FROM AccountGroup "
            Dim AccgrpDt As DataTable = New DataTable()
            AccgrpDt = clsFun.ExecDataTable(AccGrpSql)
            Dim AccgrpRqst As AddAccountGroupRequest = New AddAccountGroupRequest()
            For Each item As DataRow In AccgrpDt.Rows
                Application.DoEvents()
                lblProgress.Text = "Groups Data..."
                Dim accgrp As AccountGroupRequest = New AccountGroupRequest()
                accgrp.GroupId = item.Field(Of Int64)("ID")
                accgrp.GroupName = item.Field(Of String)("GroupName").Trim
                accgrp.UnderGroupID = item.Field(Of Int64)("UnderGroupID")
                accgrp.UnderGroupName = item.Field(Of String)("UnderGroupName").Trim
                accgrp.DC = item.Field(Of String)("DC")
                accgrp.Primary2 = item.Field(Of String)("Primary2")
                accgrp.Tag = item.Field(Of String)("Tag")
                accgrp.OrganizationId = OrgID 'item.Field(Of Int64)("OrganizationId")

                ' accgrp.ServerTag = item.Field(Of Int64)("ServrTag")
                AccgrpRqst.AccountGroups.Add(accgrp)

            Next
            AccgrpRqst.Type = 0
            Dim AccGrpResp As AccountGroupResponse = New HttpService().SendAccountGroup(AccgrpRqst, loginResp.Token)

            Dim Cratesql As String = "Select * FROM CrateMarka Where OrgID='" & Val(OrgID) & "'"
            Dim crateDt As DataTable = New DataTable()
            crateDt = ClsFunserver.ExecDataTable(Cratesql)
            Dim CrateRqst As SaveCrateMarkaRequest = New SaveCrateMarkaRequest()
            CrateRqst.OrgId = Val(txtCompanyID.Text)
            For Each item As DataRow In crateDt.Rows
                Application.DoEvents()
                lblProgress.Text = "Marka Data..."
                Dim Crtgrp As CrateMarka = New CrateMarka()
                Crtgrp.CrateID = item.Field(Of Int64)("ID")
                Crtgrp.MarkaName = item.Field(Of String)("MarkaName").Trim
                Crtgrp.OpQty = item.Field(Of Decimal)("OpQty")
                Crtgrp.Rate = item.Field(Of Decimal)("Rate")
                Crtgrp.OrgID = OrgID
                Crtgrp.Type = 1
                Crtgrp.ServerTag = item.Field(Of Int64)("ServerTag")

                CrateRqst.CrateMarkas.Add(Crtgrp)
            Next
            If crateDt.Rows.Count <> 0 Then
                CrateRqst.Type = 1
                Dim crateRsp As SaveCrateMarkaResponse = New HttpService().sendcratemarka(CrateRqst, loginResp.Token)
            End If

            '''' Ledger Talbe Insert

            Dim totLegCount As Integer = 0
            Dim sql As String = "Select COUNT(*) FROM Ledger Where OrgID='" & Val(OrgID) & "'"
            Dim ledgerCount As Integer = ClsFunserver.ExecScalarInt(sql)
            Dim progesssCount As Integer = 0
            Dim maxRowCount As Decimal = 0
            maxRowCount = Math.Ceiling(ledgerCount / 2000)
            dataProgress.Minimum = progesssCount
            dataProgress.Maximum = maxRowCount
            dataProgress.Visible = True
            lblProgress.Visible = True
            Dim legCount As Integer = 0

            For i As Integer = 0 To maxRowCount - 1
                Application.DoEvents()
                'Dim ledSql As String = "Select VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi,ServerTag" +
                '                    " FROM Ledger Where OrgID='" & Val(OrgID) & "'  LIMIT 100 OFFSET " + legCount.ToString()
                Dim ledSql As String = "Select VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi,ServerTag" +
                    " FROM Ledger Where OrgID='" & Val(OrgID) & "'  "
                Dim ledDt As DataTable = New DataTable()
                ledDt = ClsFunserver.ExecDataTable(ledSql)
                progesssCount = progesssCount + 1
                If ledDt.Rows.Count() > 0 Then
                    legCount = legCount + ledDt.Rows.Count()
                    Dim ledgerRqst As LedgerRequest = New LedgerRequest()
                    If i = 0 Then
                        ledgerRqst.IsFirstRow = True
                    Else
                        ledgerRqst.IsFirstRow = False
                    End If
                    'dataProgress.Maximum = maxRowCount 'Set Max Lenght
                    dataProgress.Step = 1 'Set Step
                    dataProgress.Value = progesssCount
                    For Each item As DataRow In ledDt.Rows
                        Application.DoEvents()
                        lblProgress.Text = "Ledger Data... " & legCount & ""
                        dataProgress.Value = i
                        Dim ledger As LedgerData = New LedgerData()
                        ledger.AccountID = item.Field(Of Int64)("AccountID")
                        ledger.VourchersID = item.Field(Of Int64)("VourchersID")
                        ledger.EntryDate = item.Field(Of Date)("EntryDate")
                        ledger.TransType = item.Field(Of String)("TransType")
                        ledger.AccountName = item.Field(Of String)("AccountName")
                        ledger.Amount = item.Field(Of Decimal)("Amount")
                        ledger.DC = item.Field(Of String)("DC")
                        ledger.Remark = Utf82Hebrew(item.Field(Of String)("Remark")).Trim
                        ledger.Remark2 = Utf82Hebrew(IIf(item.Field(Of String)("RemarkHindi") = Nothing, "", item.Field(Of String)("RemarkHindi")))
                        ledger.Narration = item.Field(Of String)("Narration")
                        ledger.OrganizationId = OrgID 'item.Field(Of Int64)("OrganizationId")
                        ledger.ServerTag = item.Field(Of Int64)("ServerTag")
                        ledgerRqst.Ledgers.Add(ledger)
                        ledgerRqst.Type = 1
                    Next
                    If ledDt.Rows.Count <> 0 Then
                        ledgerRqst.Type = 1
                        Dim ledgerResp As SaveLedgerResponse = New HttpService().SendLedgerData(ledgerRqst, loginResp.Token)
                    End If
                End If
                dataProgress.Value = 0
            Next



            '' '''' Crate Talbe Insert

            ' ''Dim totLegCount As Integer = 0


            Dim Ssql As String = "Select COUNT(*) FROM CrateVoucher where OrgID='" & Val(OrgID) & "'"
            Dim CrateCount As Integer = ClsFunserver.ExecScalarInt(Ssql)
            Dim CrateprogesssCount As Integer = 0
            Dim CratemaxRowCount As Decimal = 0
            CratemaxRowCount = Math.Ceiling(CrateCount / 2000)
            dataProgress.Minimum = CrateprogesssCount
            dataProgress.Maximum = CratemaxRowCount
            dataProgress.Visible = True
            lblProgress.Visible = True
            Dim CrateVCount As Integer = 0
            For i As Integer = 0 To CratemaxRowCount - 1

                'Dim CrateVSql As String = "Select * " +
                '                    " FROM CrateVoucher Where OrgID='" & Val(OrgID) & "' LIMIT 100 OFFSET " + CrateVCount.ToString()
                Dim CrateVSql As String = "Select * FROM CrateVoucher Where OrgID='" & Val(OrgID) & "'"
                Dim CrateVDt As DataTable = New DataTable()
                CrateVDt = ClsFunserver.ExecDataTable(CrateVSql)
                'dataProgress.Maximum = maxRowCount 'Set Max Lenght
                dataProgress.Step = 1 'Set Step
                progesssCount = progesssCount + 1
                If CrateVDt.Rows.Count() > 0 Then
                    CrateVCount = CrateVCount + CrateVDt.Rows.Count()
                    Dim CratevReqst As CrateVoucherRequest = New CrateVoucherRequest()
                    If i = 0 Then
                        CratevReqst.IsFirstRow = True
                    Else
                        CratevReqst.IsFirstRow = False
                    End If
                    dataProgress.Maximum = CratemaxRowCount 'Set Max Lenght
                    dataProgress.Step = 1 'Set Step
                    ' dataProgress.Value = progesssCount
                    CratevReqst.OrgId = Val(txtCompanyID.Text)
                    For Each item As DataRow In CrateVDt.Rows
                        Application.DoEvents()
                        lblProgress.Text = "Crate Voucher Data... " & CrateVCount & ""
                        dataProgress.Value = i
                        Dim CrateV As SaveCrateVoucherRequest = New SaveCrateVoucherRequest()
                        CrateV.OrganizationId = txtCompanyID.Text
                        CrateV.SlipNo = item.Field(Of String)("SlipNo")
                        CrateV.EntryDate = item.Field(Of Date)("EntryDate")
                        CrateV.VoucherID = item.Field(Of Int64)("VoucherID")
                        CrateV.TransType = item.Field(Of String)("TransType")
                        CrateV.AccountID = item.Field(Of Int64)("AccountID")
                        CrateV.AccountName = item.Field(Of String)("AccountName")
                        CrateV.CrateType = item.Field(Of String)("CrateType")
                        CrateV.CrateID = item.Field(Of Int64)("CrateID")
                        CrateV.CrateName = item.Field(Of String)("CrateName")
                        CrateV.Qty = item.Field(Of Int64)("Qty")
                        CrateV.Remark = Utf82Hebrew(item.Field(Of String)("Remark")).Trim
                        ' CrateV.Rate = Convert.ToDecimal(Format(CrateV.Rate, "0.00"))
                        CrateV.Rate = item.Field(Of Decimal)("Rate")
                        'CrateV.Amount = Convert.ToDecimal(Format(CrateV.Amount, "0.00"))
                        CrateV.Amount = item.Field(Of Decimal)("Amount")
                        CrateV.CashPaid = item.Field(Of String)("CashPaid")
                        CrateV.ServerTag = item.Field(Of Int64)("ServerTag")
                        'item.Field(Of Int64)("OrganizationId")
                        CratevReqst.CrateVouchers.Add(CrateV)
                    Next
                    If CrateVDt.Rows.Count <> 0 Then
                        CratevReqst.Type = 1
                        Dim CrateVResp As SaveCrateVoucherResponse = New HttpService().sendcrateVoucher(CratevReqst, loginResp.Token)
                    End If
                End If
                dataProgress.Value = 0
            Next
            Dim syncDateTimeResp As UpdateLastDataSyncDateTimeResponse = httpservice.UpdateLastDataSyncDateTime(loginResp.Token)
            lblProgress.Visible = False
            dataProgress.Visible = False
            MsgBox("Record Updated on Server Successfully", vbInformation, "Sucessful")
            ClsFunserver.ExecScalarStr("Delete From AccountGroup Where OrgID='" & Val(OrgID) & "'; " &
                                       "Delete From Accounts Where OrgID='" & Val(OrgID) & "'; " &
                                       "Delete From CrateMarka Where OrgID='" & Val(OrgID) & "'; " &
                                       "Delete From CrateVoucher Where OrgID='" & Val(OrgID) & "'; " &
                                        "Delete From Transaction2 Where OrgID='" & Val(OrgID) & "'; " &
                                       "Delete From Ledger Where OrgID='" & Val(OrgID) & "'; ")
            clsFun.ExecScalarStr("Update Company Set Autosync='Y'")
            ClsFunserver.ExecScalarStr("Vacuum;")
            clsFun.ExecScalarStr("Update  Company Set SyncDate='" & DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") & "'")
            lblLastUpdate.Text = clsFun.ExecScalarStr("Select SyncDate From Company")
            If clsFun.ExecScalarStr("Select Autosync From Company") = "Y" Then
                ckFullSync.Checked = True : btnCustom.Visible = True : btnCustom.BringToFront()
            Else
                ckFullSync.Checked = False : btnCustom.Visible = True : btnCustom.BringToFront()
            End If
        Else
            MessageBox.Show("Sorry... Your System is not Connected to Internet. Please check internet connection...", "Internet Required...", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) : Exit Sub
        End If
    End Sub
    Private Sub eventLog()
        ' setup for a divide by zero error
        Dim int1 As Integer = 10
        Dim int2 As Integer = 0
        Dim intResult As Integer
        Try
            ' trip the divide by zero error
            intResult = int1 / int2
        Catch ex As Exception
            Dim el As New Aadhat.ErrorLogger
            el.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")

            ' MsgBox("Error logged.")

        End Try
    End Sub
    Private Sub Save()
        Dim cmd As SQLite.SQLiteCommand
        Dim sql As String = "insert into Company(OrganizationID, Password)values (@1, @2)"
        Try
            cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
            cmd.Parameters.AddWithValue("@1", txtCompanyID.Text)
            cmd.Parameters.AddWithValue("@2", txtPassword.Text)
            If cmd.ExecuteNonQuery() > 0 Then
                MessageBox.Show("Record Insert Successfully", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            clsFun.CloseConnection()
        Catch ex As Exception
            MsgBox(ex.Message)
            clsFun.CloseConnection()
        End Try
    End Sub
    Private Sub UpdateRecord()
        Dim sql As String = "Update Company SET OrganizationID='" & txtCompanyID.Text & "',Password='" & txtPassword.Text & "' WHERE ID=" & Val(txtID.Text) & ""
        cmd = New SQLite.SQLiteCommand(sql, clsFun.GetConnection())
        Try
            If clsFun.ExecNonQuery(sql) > 0 Then
                '  MsgBox("Record Updated Successfully.", vbInformation + vbOKOnly, "Updated")
            End If
            'con.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
            'con.Close()
        End Try
    End Sub

    Private Sub Mobile_App_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If dataProgress.Visible = True Then Exit Sub
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Mobile_App_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        txtCompanyID.TextAlign = HorizontalAlignment.Center
        FillControls() : txtPassword.UseSystemPasswordChar = True
        If Not File.Exists(fileName) = True Then
            pnlLock.Visible = True : TxtAuthPathPassword.UseSystemPasswordChar = True
            TxtAuthPathPassword.Focus()
            '  clsFun.ExecNonQuery("Update Company SET OrganizationID='123456', Password='123456'")
        End If
        If Val(txtID.Text) <> 0 Then ckFullSync.Visible = True
        lblLastUpdate.Text = clsFun.ExecScalarStr("Select SyncDate From Company")
        'If ClsCommon.IsInternetConnect() = False Then lblLink.Text = "Server Down or No Internet"
    End Sub

    'Private Sub setcultre()

    '    Dim culture = CultureInfo.CreateSpecificCulture("HI-IN")
    '    If culture IsNot Nothing Then
    '        System.Threading.Thread.CurrentThread.CurrentUICulture = culture
    '    End If
    'End Sub
    Public Function Utf82Hebrew(ByVal Str As String) As String
        Dim ascii As System.Text.Encoding = System.Text.Encoding.GetEncoding("windows-1252")
        Dim unicode As System.Text.Encoding = System.Text.Encoding.UTF8

        ' Convert the string into a byte array. 
        Dim unicodeBytes As Byte() = unicode.GetBytes(Str)

        ' Perform the conversion from one encoding to the other. 
        Dim asciiBytes As Byte() = System.Text.Encoding.Convert(unicode, Encoding.UTF8, unicodeBytes)

        ' Convert the new byte array into a char array and then into a string. 
        Dim asciiString As String = ascii.GetString(asciiBytes)

        Utf82Hebrew = asciiString
    End Function
    Public Function Utf8ToUnicode(ByVal str As String) As String
        Dim utf8 As System.Text.Encoding = System.Text.Encoding.UTF8
        Dim unicode As System.Text.Encoding = System.Text.Encoding.Unicode

        Dim utf8Bytes As Byte() = utf8.GetBytes(str)
        Dim unicodeBytes As Byte() = System.Text.Encoding.Convert(utf8, unicode, utf8Bytes)

        Return unicode.GetString(unicodeBytes)
    End Function

    Private Sub btnSync_Click(sender As Object, e As EventArgs)

        If txtCompanyID.Text = "" Or Val(txtCompanyID.Text) = 0 Then MsgBox("Please Check Your Organization ID...", MsgBoxStyle.Exclamation, "Organization ID") : BtnIDGenrate.BackColor = Color.Red : BtnIDGenrate.Focus() : Exit Sub
        If txtPassword.Text = "" Then MsgBox("Please Re Check Your Password...", MsgBoxStyle.Exclamation, "Password") : txtPassword.Focus() : Exit Sub
        If MessageBox.Show("Are Your Sure Want to Fully Sync on Server... " & vbNewLine & " It Can Take Time More than 5-10 minites, Be Patience.", "Sure", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            BtnIDGenrate.Visible = False
            OrgID = txtCompanyID.Text
            saveUpdateInfo()
        End If

    End Sub
    Private Const LOCALE_SYSTEM_DEFAULT = &H800

    Private Declare Function SetLocaleInfo Lib "kernel32" Alias "SetLocaleInfoA" (ByVal Locale As Integer, ByVal LCType As Integer, ByVal lpLCData As String) As Boolean
    Private Declare Function GetSystemDefaultLCID Lib "kernel32" () As Integer


    Public Sub SetDateTimeFormat()
        Dim dwLCID As Integer = GetSystemDefaultLCID()
        Try
            SetLocaleInfo(dwLCID, LOCALE_SYSTEM_DEFAULT, "Hindi (India)")
            MsgBox("done")

        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try

    End Sub

    Public Sub FillControls()
        Dim ssql As String = String.Empty
        ' Dim primary As String = String.Empty
        ' BtnIDGenrate.Visible = False
        Dim dt As New DataTable
        ssql = "Select * from Company "
        dt = clsFun.ExecDataTable(ssql) ' where id=" & id & "")
        If dt.Rows.Count > 0 Then
            txtID.Text = Val(dt.Rows(0)("ID").ToString())
            txtCompanyID.Text = Val(dt.Rows(0)("OrganizationID").ToString())
            txtPassword.Text = dt.Rows(0)("Password").ToString()
        End If
        If txtCompanyID.Text > 0 Then
            BtnIDGenrate.Visible = False
        Else
            txtCompanyID.Text = ""
        End If

    End Sub

    Private Sub txtCompanyID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCompanyID.KeyDown, BtnIDGenrate.KeyDown, txtPassword.KeyDown, CkShowPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub CkShowPassword_CheckedChanged(sender As Object, e As EventArgs) Handles CkShowPassword.CheckedChanged
        If CkShowPassword.Checked = True Then
            txtPassword.UseSystemPasswordChar = False
        Else
            txtPassword.UseSystemPasswordChar = True
        End If
    End Sub

    Public Function GetNewConnection() As SqlConnection
        Dim conn As SqlConnection
        conn = New SqlConnection("Data Source=103.160.144.174;Initial Catalog=smicloud_Aadhat;Persist Security Info=True;User ID=as;Password=admin@123")
        Return conn
    End Function
    Public Function UpdateCompany()
        Dim dts As New DataTable
        Dim conn As SqlConnection
        ' clsFun.changeCompany()
        dts = clsFun.ExecDataTable("Select * From Accounts")
        Try
            If dts.Rows.Count > 0 Then
                Dim i As Integer = 0
                For i = 0 To dts.Rows.Count - 1
                    conn = GetNewConnection()
                    conn.Open()
                    Dim cmd As New SqlCommand("Sp_Account_Master_Save", conn)
                    Try
                        cmd.Parameters("@OrganizationId").Value = txtCompanyID.Text
                        cmd.Parameters.Add("@OrganizationId", SqlDbType.Int)
                        cmd.Parameters.Add("@AccountID", SqlDbType.VarChar)
                        cmd.Parameters("@AccountID").Value = dts.Rows(i)("ID").ToString()
                        cmd.Parameters.Add("@AccountName", SqlDbType.NVarChar)
                        cmd.Parameters("@AccountName").Value = dts.Rows(i)("AccountName").ToString()
                        cmd.Parameters.Add("@GroupID", SqlDbType.VarChar)
                        cmd.Parameters("@GroupID").Value = dts.Rows(i)("GroupID").ToString()
                        cmd.Parameters.Add("@DC", SqlDbType.VarChar)
                        cmd.Parameters("@DC").Value = dts.Rows(i)("DC").ToString()
                        cmd.Parameters.Add("@Tag", SqlDbType.VarChar)
                        cmd.Parameters("@Tag").Value = dts.Rows(i)("Tag").ToString()
                        cmd.Parameters.Add("@OpBal", SqlDbType.VarChar)
                        cmd.Parameters("@OpBal").Value = dts.Rows(i)("OpBal").ToString()
                        cmd.Parameters.Add("@Address", SqlDbType.VarChar)
                        cmd.Parameters("@Address").Value = dts.Rows(i)("Address").ToString()
                        cmd.Parameters.Add("@OtherName", SqlDbType.NVarChar)
                        cmd.Parameters("@OtherName").Value = dts.Rows(i)("OtherName").ToString()
                        cmd.Parameters.Add("@LFNo", SqlDbType.NVarChar)
                        cmd.Parameters("@LFNo").Value = dts.Rows(i)("LFNo").ToString()
                        cmd.Parameters.Add("@Area", SqlDbType.NVarChar)
                        cmd.Parameters("@Area").Value = dts.Rows(i)("Area").ToString()
                        cmd.Parameters.Add("@City", SqlDbType.NVarChar)
                        cmd.Parameters("@City").Value = dts.Rows(i)("City").ToString()
                        cmd.Parameters.Add("@State", SqlDbType.NVarChar)
                        cmd.Parameters("@State").Value = dts.Rows(i)("State").ToString()
                        cmd.Parameters.Add("@Phone", SqlDbType.NVarChar)
                        cmd.Parameters("@Phone").Value = dts.Rows(i)("Phone").ToString()
                        cmd.Parameters.Add("@Contact", SqlDbType.NVarChar)
                        cmd.Parameters("@Contact").Value = dts.Rows(i)("Contact").ToString()
                        cmd.Parameters.Add("@Mobile1", SqlDbType.NVarChar)
                        cmd.Parameters("@Mobile1").Value = dts.Rows(i)("Mobile1").ToString()
                        cmd.Parameters.Add("@Mobile2", SqlDbType.NVarChar)
                        cmd.Parameters("@Mobile2").Value = dts.Rows(i)("Mobile2").ToString()
                        cmd.Parameters.Add("@MailID", SqlDbType.NVarChar)
                        cmd.Parameters("@MailID").Value = dts.Rows(i)("MailID").ToString()
                        cmd.Parameters.Add("@BankName", SqlDbType.NVarChar)
                        cmd.Parameters("@BankName").Value = dts.Rows(i)("BankName").ToString()
                        cmd.Parameters.Add("@AccNo", SqlDbType.NVarChar)
                        cmd.Parameters("@AccNo").Value = dts.Rows(i)("AccNo").ToString()
                        cmd.Parameters.Add("@IFSC", SqlDbType.NVarChar)
                        cmd.Parameters("@IFSC").Value = dts.Rows(i)("IFSC").ToString()
                        cmd.Parameters.Add("@GName", SqlDbType.NVarChar)
                        cmd.Parameters("@GName").Value = dts.Rows(i)("GName").ToString()
                        cmd.Parameters.Add("@GMobile1", SqlDbType.NVarChar)
                        cmd.Parameters("@GMobile1").Value = dts.Rows(i)("GMobile1").ToString()
                        cmd.Parameters.Add("@GMobile2", SqlDbType.NVarChar)
                        cmd.Parameters("@GMobile2").Value = dts.Rows(i)("GMobile2").ToString()
                        cmd.Parameters.Add("@Gaddress", SqlDbType.NVarChar)
                        cmd.Parameters("@Gaddress").Value = dts.Rows(i)("Gaddress").ToString()
                        cmd.Parameters.Add("@GCity", SqlDbType.NVarChar)
                        cmd.Parameters("@GCity").Value = dts.Rows(i)("GCity").ToString()
                        cmd.Parameters.Add("@GState", SqlDbType.NVarChar)
                        cmd.Parameters("@GState").Value = dts.Rows(i)("GState").ToString()

                        'cmd.Parameters.Add("@Limit", SqlDbType.NVarChar)
                        'cmd.Parameters("@Limit").Value = dts.Rows(i)("Limit").ToString()
                        'cmd.Parameters.Add("@GState", SqlDbType.NVarChar)
                        'cmd.Parameters("@GState").Value = dts.Rows(i)("GState").ToString
                        'cmd.Parameters.Add("@AccountPhoto", SqlDbType.NVarChar)
                        'cmd.Parameters("@AccountPhoto").Value = dts.Rows(i)("AccountPhoto").ToString()
                        'cmd.Parameters.Add("@Gphoto", SqlDbType.NVarChar)
                        'cmd.Parameters("@Gphoto").Value = dts.Rows(i)("Gphoto").ToString

                        If cmd.ExecuteNonQuery() > 0 Then
                            MsgBox(i + 1)
                        End If
                    Finally
                        If cmd IsNot Nothing Then cmd.Dispose()
                        If conn IsNot Nothing AndAlso conn.State <> ConnectionState.Closed Then conn.Close()
                    End Try
                    dts.Dispose()
                Next
            End If
        Catch ex As Exception
            MsgBox("Something went wrong", MsgBoxStyle.Critical, "Access Denied")
        End Try
        Return ""
    End Function

    Private Sub btnCustom_Click(sender As Object, e As EventArgs) Handles btnCustom.Click

        ''''Company Data
        ''''
        If ckFullSync.Checked = True Then
            If txtCompanyID.Text = "" Or Val(txtCompanyID.Text) = 0 Then MsgBox("Please Check Your Organization ID...", MsgBoxStyle.Critical, "Organization ID") : BtnIDGenrate.BackColor = Color.Red : BtnIDGenrate.Focus() : Exit Sub
            If txtPassword.Text = "" Then MsgBox("Please Re Check Your Password...", MsgBoxStyle.Critical, "Password") : txtPassword.Focus() : Exit Sub
            If MessageBox.Show("Are Your Sure Want to Fully Sync on Server... " & vbNewLine & " It Can Take Time More than 5-10 minites, Be Patience.", "Sure", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                BtnIDGenrate.Visible = False
                OrgID = txtCompanyID.Text
                ckFullSync.Visible = False
                saveUpdateInfo()
                ckFullSync.Visible = True
                ckFullSync.Checked = False
            End If
        Else
            ckFullSync.Visible = False
            CustomUpdate()
            ckFullSync.Visible = True
            ckFullSync.Checked = False
        End If
    End Sub
    Private Sub testingMobileApp()
        Dim sql As String = "Select COUNT(*) FROM Ledger"
        Dim ledgerCount As Integer = clsFun.ExecScalarInt(sql)
        Dim progesssCount As Integer = 0
        Dim maxRowCount As Decimal = 0
        maxRowCount = Math.Ceiling(ledgerCount / 100)
        dataProgress.Minimum = progesssCount
        dataProgress.Maximum = maxRowCount
        dataProgress.Visible = True
        lblProgress.Visible = True
        Dim legCount As Integer = 0
        clsfunSql.ExecScalarStr("Delete From Ledger Where OrganizationID=" & Val(OrgID) & "")
        For i As Integer = 0 To maxRowCount - 1
            Application.DoEvents()
            Dim FastQuery As String = String.Empty
            Dim Ssql As String = String.Empty
            Dim ledSql As String = "Select VourchersID,EntryDate,TransType,AccountID,AccountName,Amount,DC,Remark,Narration,RemarkHindi" +
                                " FROM Ledger LIMIT 100 OFFSET " + legCount.ToString()
            'Dim ledDt As DataTable = New DataTable()
            ledDt = clsFun.ExecDataTable(ledSql)
            progesssCount = progesssCount + 1
            If ledDt.Rows.Count() > 0 Then
                legCount = legCount + ledDt.Rows.Count()
                Dim ledgerRqst As LedgerRequest = New LedgerRequest()
                If i = 0 Then
                    ledgerRqst.IsFirstRow = True
                Else
                    ledgerRqst.IsFirstRow = False
                End If
                'dataProgress.Maximum = maxRowCount 'Set Max Lenght
                dataProgress.Step = 1 'Set Step
                dataProgress.Value = progesssCount
                For Each item As DataRow In ledDt.Rows
                    Application.DoEvents()
                    lblProgress.Text = "Ledger Data... " & legCount & ""
                    dataProgress.Value = i
                    FastQuery = FastQuery & IIf(FastQuery <> "", " UNION ALL SELECT ", "") & item.Field(Of Int64)("AccountID") & "," & item.Field(Of Int64)("VourchersID") & "," &
                                "'" & item.Field(Of Date)("EntryDate") & "','" & item.Field(Of String)("TransType") & "','" & item.Field(Of String)("AccountName") & "', " &
                                "'" & item.Field(Of Decimal)("Amount") & "','" & item.Field(Of String)("DC") & "','" & item.Field(Of String)("Remark") & "'," &
                                "'" & Utf8ToUnicode(IIf(item.Field(Of String)("RemarkHindi") = Nothing, "", item.Field(Of String)("RemarkHindi"))) & "','" & item.Field(Of String)("Remark") & "'," & Val(OrgID) & ""
                Next
                Ssql = "Insert Into Ledger (AccountID,VourchersID,EntryDate,TransType,AccountName,Amount,DC,Remark,Remark2,Narration,OrganizationID) SELECT " & FastQuery & ""
                clsfunSql.ExecScalarStr(Ssql)
            End If

            dataProgress.Value = 0
        Next
    End Sub

    Private Sub TxtAuthPathPassword_TextChanged(sender As Object, e As EventArgs) Handles TxtAuthPathPassword.TextChanged
        If TxtAuthPathPassword.Text = "SMI" Then pnlLock.Visible = False : If BtnIDGenrate.Visible = False Then btnCustom.Focus() Else BtnIDGenrate.Focus()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click

    End Sub

    Private Sub pnlLock_Paint(sender As Object, e As PaintEventArgs) Handles pnlLock.Paint

    End Sub
End Class