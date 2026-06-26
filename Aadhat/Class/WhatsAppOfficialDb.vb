Imports System.Data
Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms
Imports System.Collections.Generic

Public Class WhatsAppOfficialDb
    Private Const DefaultTemplateFooter As String = ""

    Public Shared ReadOnly Property DatabasePath() As String
        Get
            Return Path.Combine(Application.StartupPath, "msgs.db")
        End Get
    End Property

    Public Shared ReadOnly Property ConString() As String
        Get
            Return "Data Source=" & DatabasePath & ";Version=3;New=True;Compress=True;synchronous=ON;"
        End Get
    End Property

    Public Shared Sub EnsureDatabase()
        If File.Exists(DatabasePath) = False Then SQLiteConnection.CreateFile(DatabasePath)

        ExecNonQuery("Create Table If Not Exists ApiSettings (ID INTEGER PRIMARY KEY CHECK (ID = 1), VendorUid TEXT, AccessToken TEXT, SendingMethod TEXT, LanguageType TEXT, SendingType TEXT, MsgAccessToken TEXT, DefaultSim TEXT, BaseUrl TEXT, LastConnectedAt TEXT, BusinessStatus TEXT, BusinessInfoText TEXT, LastBusinessInfoAt TEXT)")
        ExecNonQuery("Create Table If Not Exists PredefinedTemplates (TemplateCode TEXT PRIMARY KEY, TemplateName TEXT, LanguageCode TEXT, TemplateType TEXT, ParameterCount INTEGER, IsDefault INTEGER DEFAULT 0, HeaderType TEXT, SupportsFile INTEGER DEFAULT 0, Description TEXT, MetaStatus TEXT, IsApproved INTEGER DEFAULT 0, IsPending INTEGER DEFAULT 0, IsRejected INTEGER DEFAULT 0, Status TEXT, BodyText TEXT, FooterText TEXT, Category TEXT, Examples TEXT)")
        ExecNonQuery("Create Table If Not Exists TemplateMappings (MappingKey TEXT PRIMARY KEY, ModuleName TEXT, DisplayName TEXT, TemplateCode TEXT, LanguageCode TEXT, MessageMode TEXT, ParameterFields TEXT, UpdatedAt TEXT)")
        EnsureColumn("ApiSettings", "BusinessStatus", "TEXT")
        EnsureColumn("ApiSettings", "BusinessInfoText", "TEXT")
        EnsureColumn("ApiSettings", "LastBusinessInfoAt", "TEXT")
        EnsureColumn("ApiSettings", "TemplateVendorUid", "TEXT")
        EnsureColumn("PredefinedTemplates", "HeaderType", "TEXT")
        EnsureColumn("PredefinedTemplates", "SupportsFile", "INTEGER DEFAULT 0")
        EnsureColumn("PredefinedTemplates", "MetaStatus", "TEXT")
        EnsureColumn("PredefinedTemplates", "IsApproved", "INTEGER DEFAULT 0")
        EnsureColumn("PredefinedTemplates", "IsPending", "INTEGER DEFAULT 0")
        EnsureColumn("PredefinedTemplates", "IsRejected", "INTEGER DEFAULT 0")
        EnsureColumn("PredefinedTemplates", "Status", "TEXT")
        EnsureColumn("PredefinedTemplates", "BodyText", "TEXT")
        EnsureColumn("PredefinedTemplates", "FooterText", "TEXT")
        EnsureColumn("PredefinedTemplates", "Category", "TEXT")
        EnsureColumn("PredefinedTemplates", "Examples", "TEXT")
        EnsureColumn("PredefinedTemplates", "ButtonsJson", "TEXT")
        EnsureColumn("TemplateMappings", "ParameterFields", "TEXT")
        ExecNonQuery("Create Table If Not Exists MessageLog (ID INTEGER PRIMARY KEY AUTOINCREMENT, MobileNo TEXT, TemplateCode TEXT, MessageMode TEXT, Status TEXT, ResponseMessage TEXT, CreatedAt TEXT)")
        MigrateProtectedCredentials()
        CleanupOfficialApiLogs()
        SeedLocalPredefinedTemplates()
        SeedDefaultTemplateMappings()
        NormalizeExistingTemplateTypes()
    End Sub

    Public Shared Function ExecDataTable(ByVal cmdText As String) As DataTable
        EnsureDatabaseLight()
        Dim ad As New SQLiteDataAdapter(cmdText, ConString)
        Dim dt As DataTable = New DataTable()
        ad.Fill(dt)
        ad.Dispose()
        Return dt
    End Function

    Public Shared Function ExecNonQuery(ByVal cmdText As String) As Integer
        EnsureDatabaseLight()
        Dim con As SQLiteConnection = New SQLiteConnection(ConString)
        con.Open()
        Dim cmd As SQLiteCommand = New SQLiteCommand(cmdText, con)
        Dim rows As Integer = cmd.ExecuteNonQuery()
        cmd.Dispose()
        con.Dispose()
        Return rows
    End Function

    Public Shared Function ExecScalarStr(ByVal cmdText As String) As String
        EnsureDatabaseLight()
        Dim con As SQLiteConnection = New SQLiteConnection(ConString)
        con.Open()
        Dim cmd As SQLiteCommand = New SQLiteCommand(cmdText, con)
        Dim obj As Object = cmd.ExecuteScalar()
        cmd.Dispose()
        con.Dispose()
        If obj Is Nothing Then Return ""
        Return obj.ToString()
    End Function

    Public Shared Sub SaveApiSettings(ByVal vendorUid As String, ByVal accessToken As String, ByVal sendingMethod As String, ByVal languageType As String, ByVal sendingType As String, ByVal msgAccessToken As String, ByVal defaultSim As String)
        EnsureDatabase()
        Dim con As SQLiteConnection = New SQLiteConnection(ConString)
        con.Open()
        Dim sql As String = "Insert Or Ignore Into ApiSettings(ID) Values(1); Update ApiSettings Set VendorUid=@VendorUid, AccessToken=@AccessToken, SendingMethod=@SendingMethod, LanguageType=@LanguageType, SendingType=@SendingType, MsgAccessToken=@MsgAccessToken, DefaultSim=@DefaultSim, BaseUrl=@BaseUrl, LastConnectedAt=datetime('now') Where ID=1"
        Dim cmd As SQLiteCommand = New SQLiteCommand(sql, con)
        cmd.Parameters.AddWithValue("@VendorUid", ProtectCredential(vendorUid))
        cmd.Parameters.AddWithValue("@AccessToken", ProtectCredential(accessToken))
        cmd.Parameters.AddWithValue("@SendingMethod", SafeValue(sendingMethod))
        cmd.Parameters.AddWithValue("@LanguageType", SafeValue(languageType))
        cmd.Parameters.AddWithValue("@SendingType", SafeValue(sendingType))
        cmd.Parameters.AddWithValue("@MsgAccessToken", SafeValue(msgAccessToken))
        cmd.Parameters.AddWithValue("@DefaultSim", SafeValue(defaultSim))
        cmd.Parameters.AddWithValue("@BaseUrl", WhatsAppOfficialApi.BaseUrl)
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        con.Dispose()
    End Sub
    Public Shared Sub SaveBusinessInfoStatus(ByVal statusText As String, ByVal infoText As String)
        EnsureDatabase()
        ExecNonQuery("Insert Or Ignore Into ApiSettings(ID) Values(1)")
        ExecNonQuery("Update ApiSettings Set BusinessStatus='" & SqlText(statusText) & "', BusinessInfoText='" & SqlText(infoText) & "', LastBusinessInfoAt=datetime('now') Where ID=1")
    End Sub

    Public Shared Function GetBusinessInfoDisplayText() As String
        EnsureDatabase()
        Return ExecScalarStr("Select BusinessInfoText From ApiSettings Where ID=1")
    End Function


    Public Shared Function GetApiSettings() As DataTable
        EnsureDatabase()
        Dim dt As DataTable = ExecDataTable("Select * From ApiSettings Where ID=1")
        If dt.Rows.Count > 0 Then
            DecryptDataTableField(dt, "VendorUid")
            DecryptDataTableField(dt, "AccessToken")
        End If
        Return dt
    End Function

    Public Shared Function GetSetting(ByVal fieldName As String) As String
        EnsureDatabase()
        If IsSafeFieldName(fieldName) = False Then Return ""
        Dim value As String = ExecScalarStr("Select " & fieldName & " From ApiSettings Where ID=1")
        If IsCredentialField(fieldName) Then Return UnprotectCredential(value)
        Return value
    End Function
    Public Shared Function GetTemplateRowCount() As Integer
        EnsureDatabase()
        Return Val(ExecScalarStr("Select Count(*) From PredefinedTemplates"))
    End Function

    Public Shared Sub ClearLocalTemplates()
        EnsureDatabase()
        ExecNonQuery("Delete From PredefinedTemplates")
        ExecNonQuery("Delete From TemplateMappings")
        ExecNonQuery("Update ApiSettings Set TemplateVendorUid='' Where ID=1")
        SeedDefaultTemplateMappings()
    End Sub

    Public Shared Sub ClearOfficialApiSettings()
        EnsureDatabase()
        ExecNonQuery("Insert Or Ignore Into ApiSettings(ID) Values(1)")
        ExecNonQuery("Update ApiSettings Set VendorUid='', AccessToken='', TemplateVendorUid='', BusinessStatus='', BusinessInfoText='', LastConnectedAt=NULL, LastBusinessInfoAt=NULL Where ID=1")
        ResetTemplateMetaState()
    End Sub

    Public Shared Sub MarkTemplatesVendor(ByVal vendorUid As String)
        EnsureDatabase()
        ExecNonQuery("Insert Or Ignore Into ApiSettings(ID) Values(1)")
        ExecNonQuery("Update ApiSettings Set TemplateVendorUid='" & SqlText(ProtectCredential(vendorUid)) & "' Where ID=1")
    End Sub

    Public Shared Sub EnsureTemplateCacheForVendor(ByVal vendorUid As String)
        EnsureDatabase()
        vendorUid = SafeValue(vendorUid)
        If vendorUid = "" Then Exit Sub

        Dim cachedVendorUid As String = GetSetting("TemplateVendorUid")
        If cachedVendorUid = vendorUid Then Exit Sub

        ResetTemplateMetaState()
        MarkTemplatesVendor(vendorUid)
    End Sub

    Private Shared Sub ResetTemplateMetaState()
        ExecNonQuery("Update PredefinedTemplates Set IsApproved=0, IsPending=0, IsRejected=0, MetaStatus='LOCAL', Status='LOCAL' Where Upper(IfNull(Status,''))<>'LOCAL' Or Upper(IfNull(MetaStatus,''))<>'LOCAL' Or IfNull(IsApproved,0)<>0 Or IfNull(IsPending,0)<>0 Or IfNull(IsRejected,0)<>0")
    End Sub

    Public Shared Function GetTemplatesForDisplay() As DataTable
        EnsureDatabase()
        Return ExecDataTable("Select 0 As SNo, TemplateCode, TemplateName, LanguageCode, TemplateType, " & _
                             "Case TemplateType " & _
                             "When 'sale_bill' Then 'Print Bill' " & _
                             "When 'print_bill' Then 'Print Bill' " & _
                             "When 'print_bill_pdf_only' Then 'Print Bill' " & _
                             "When 'print_bill_pdf_message' Then 'Print Bill' " & _
                             "When 'receipt' Then 'Receipt' " & _
                             "When 'payment' Then 'Payment' " & _
                             "When 'balance' Then 'Balance' " & _
                             "When 'statement' Then 'Statement' " & _
                             "When 'crate_in' Then 'Crate In' " & _
                             "When 'crate_out' Then 'Crate Out' " & _
                             "When 'ledger' Then 'Ledger' " & _
                             "When 'settle_ledger' Then 'Settle Ledger' " & _
                             "When 'sub_ledger' Then 'Sub Ledger' " & _
                             "When 'purchase' Then 'Purchase' " & _
                             "When 'purchase_register' Then 'Purchase Register' " & _
                             "When 'standard_sale' Then 'Standard Sale' " & _
                             "When 'standard_sale_register' Then 'Standard Sale Register' " & _
                             "When 'super_sale_register' Then 'Super Sale Register' " & _
                             "When 'sellout_manual' Then 'Sellout Manual' " & _
                             "When 'sellout_auto' Then 'Sellout Auto' " & _
                             "When 'crate_ledger' Then 'Crate Ledger' " & _
                             "Else TemplateType End As LocalTypeName, " & _
                             "ParameterCount, HeaderType, Case When Status Is Not Null And Status<>'' Then Status When IsApproved=1 Then 'APPROVED' When IsPending=1 Then 'PENDING' When IsRejected=1 Then 'REJECTED' When MetaStatus Is Null Or MetaStatus='' Then 'LOCAL' Else MetaStatus End As Status, Case When SupportsFile=1 Then 'YES' Else 'NO' End As FileSupport, Description, BodyText, FooterText, Category, Examples, ButtonsJson From PredefinedTemplates Order By LocalTypeName, LanguageCode, TemplateName")
    End Function

    Public Shared Function GetApprovedPrintBillDocumentTemplates(Optional ByVal languageCode As String = "") As DataTable
        EnsureDatabase()
        languageCode = NormalizeLanguageCode(languageCode)
        Dim languageFilter As String = "p.LanguageCode='" & SqlText(languageCode) & "'"
        Dim languageOrder As String = "Case When p.LanguageCode='" & SqlText(languageCode) & "' Then 0 Else 1 End"
        If languageCode = "hi" Then
            languageFilter = "(p.LanguageCode='hi' Or Lower(p.TemplateCode) Like '%hi%' Or Lower(p.TemplateName) Like '%hindi%' Or Lower(p.TemplateName) Like '% hi%')"
            languageOrder = "Case When p.LanguageCode='hi' Then 0 When Lower(p.TemplateCode) Like '%hi%' Or Lower(p.TemplateName) Like '%hindi%' Or Lower(p.TemplateName) Like '% hi%' Then 1 Else 2 End"
        Else
            languageFilter = "(p.LanguageCode='en' And Lower(p.TemplateCode) Not Like '%hi%' And Lower(p.TemplateName) Not Like '%hindi%' And Lower(p.TemplateName) Not Like '% hi%')"
            languageOrder = "Case When p.LanguageCode='en' Then 0 Else 1 End"
        End If
        Return ExecDataTable("Select p.TemplateCode, p.TemplateName, p.LanguageCode, p.ParameterCount, p.BodyText, " & _
                             "IfNull((Select m.ParameterFields From TemplateMappings m Where m.ModuleName='PRINT_BILL' And m.TemplateCode=p.TemplateCode And IfNull(m.ParameterFields,'')<>'' Order By m.UpdatedAt Desc Limit 1),'account_name,bill_date,company_name,bill_total') As ParameterFields " & _
                             "From PredefinedTemplates p Where p.TemplateType In ('print_bill','print_bill_pdf_only','print_bill_pdf_message','sale_bill') " & _
                             "And " & languageFilter & " " & _
                             "And (IfNull(p.HeaderType,'')='document' Or p.SupportsFile=1) " & _
                             "And (p.IsApproved=1 Or Upper(IfNull(p.Status,'')) Like '%APPROVED%' Or Upper(IfNull(p.MetaStatus,'')) Like '%APPROVED%') " & _
                             "Order By " & languageOrder & ", p.TemplateName")
    End Function

    Public Shared Function GetApprovedDocumentTemplates(ByVal templateType As String, Optional ByVal languageCode As String = "") As DataTable
        EnsureDatabase()
        templateType = NormalizeTemplateType(templateType)
        languageCode = NormalizeLanguageCode(languageCode)
        Dim moduleName As String = templateType.ToUpper()
        Dim languageFilter As String = GetTemplateLanguageFilter(languageCode)
        Dim languageOrder As String = GetTemplateLanguageOrder(languageCode)
        Return ExecDataTable("Select p.TemplateCode, p.TemplateName, p.LanguageCode, p.ParameterCount, p.BodyText, " & _
                             "IfNull((Select m.ParameterFields From TemplateMappings m Where m.ModuleName='" & SqlText(moduleName) & "' And m.TemplateCode=p.TemplateCode And IfNull(m.ParameterFields,'')<>'' Order By m.UpdatedAt Desc Limit 1),'" & SqlText(DefaultParameterFields(templateType)) & "') As ParameterFields " & _
                             "From PredefinedTemplates p Where p.TemplateType='" & SqlText(templateType) & "' " & _
                             "And " & languageFilter & " " & _
                             "And (IfNull(p.HeaderType,'')='document' Or p.SupportsFile=1) " & _
                             "And (p.IsApproved=1 Or Upper(IfNull(p.Status,'')) Like '%APPROVED%' Or Upper(IfNull(p.MetaStatus,'')) Like '%APPROVED%') " & _
                             "Order By " & languageOrder & ", p.TemplateName")
    End Function

    Public Shared Function GetApprovedTemplates(ByVal templateType As String, Optional ByVal languageCode As String = "") As DataTable
        EnsureDatabase()
        templateType = NormalizeTemplateType(templateType)
        languageCode = NormalizeLanguageCode(languageCode)
        Dim moduleName As String = templateType.ToUpper()
        Dim languageFilter As String = GetTemplateLanguageFilter(languageCode)
        Dim languageOrder As String = GetTemplateLanguageOrder(languageCode)
        Return ExecDataTable("Select p.TemplateCode, p.TemplateName, p.LanguageCode, p.ParameterCount, p.BodyText, p.HeaderType, p.SupportsFile, " & _
                             "IfNull((Select m.ParameterFields From TemplateMappings m Where m.ModuleName='" & SqlText(moduleName) & "' And m.TemplateCode=p.TemplateCode And IfNull(m.ParameterFields,'')<>'' Order By m.UpdatedAt Desc Limit 1),'" & SqlText(DefaultParameterFields(templateType)) & "') As ParameterFields " & _
                             "From PredefinedTemplates p Where p.TemplateType='" & SqlText(templateType) & "' " & _
                             "And " & languageFilter & " " & _
                             "And (p.IsApproved=1 Or Upper(IfNull(p.Status,'')) Like '%APPROVED%' Or Upper(IfNull(p.MetaStatus,'')) Like '%APPROVED%') " & _
                             "Order By " & languageOrder & ", Case When IfNull(p.HeaderType,'')='document' Or p.SupportsFile=1 Then 0 Else 1 End, p.TemplateName")
    End Function

    Public Shared Function GetTemplateByLocalTypeAndLanguage(ByVal templateType As String, ByVal languageCode As String) As DataTable
        EnsureDatabase()
        templateType = NormalizeTemplateType(templateType)
        languageCode = NormalizeLanguageCode(languageCode)
        Dim typeFilter As String = "p.TemplateType='" & SqlText(templateType) & "'"
        If templateType = "print_bill" Then typeFilter = "p.TemplateType In ('print_bill','print_bill_pdf_only','print_bill_pdf_message','sale_bill')"
        Return ExecDataTable("Select p.TemplateCode, p.TemplateName, p.LanguageCode, p.TemplateType, p.ParameterCount, p.HeaderType, p.SupportsFile, p.Description, p.BodyText, p.FooterText, p.Category, p.Examples " & _
                             "From PredefinedTemplates p Where " & typeFilter & " And p.LanguageCode='" & SqlText(languageCode) & "' " & _
                             "Order By Case When p.IsDefault=1 Then 0 Else 1 End, Case When Upper(IfNull(p.Status,'')) Like '%APPROVED%' Or Upper(IfNull(p.MetaStatus,'')) Like '%APPROVED%' Or p.IsApproved=1 Then 0 Else 1 End, p.TemplateName Limit 1")
    End Function

    Public Shared Function GetTemplateMappingParameterFields(ByVal mappingKey As String) As String
        EnsureDatabase()
        Return ExecScalarStr("Select ParameterFields From TemplateMappings Where MappingKey='" & SqlText(mappingKey) & "' And IfNull(ParameterFields,'')<>'' Limit 1")
    End Function

    Public Shared Function GetTemplateParameterFields(ByVal moduleName As String, ByVal templateCode As String, ByVal fallbackMappingKey As String) As String
        EnsureDatabase()
        Dim fields As String = ""
        If SafeValue(moduleName) <> "" AndAlso SafeValue(templateCode) <> "" Then
            fields = ExecScalarStr("Select ParameterFields From TemplateMappings Where ModuleName='" & SqlText(SafeValue(moduleName).ToUpper()) & "' And TemplateCode='" & SqlText(templateCode) & "' And IfNull(ParameterFields,'')<>'' Order By UpdatedAt Desc Limit 1")
        End If
        If fields.Trim() <> "" Then Return NormalizeParameterFieldsText(fields)
        Return GetTemplateMappingParameterFields(fallbackMappingKey)
    End Function


    Public Shared Function GetTemplateName(ByVal languageType As String, Optional ByVal templateType As String = "sale_bill") As String
        EnsureDatabase()
        Dim languageCode As String = "en"
        If SafeValue(languageType).ToLower().Contains("hindi") Or SafeValue(languageType).ToLower().Contains("regional") Then languageCode = "hi"
        Dim templateCode As String = ExecScalarStr("Select TemplateCode From PredefinedTemplates Where TemplateType='" & SqlText(templateType) & "' And LanguageCode='" & SqlText(languageCode) & "' Order By IsDefault Desc Limit 1")
        If templateCode <> "" Then Return templateCode
        If languageCode = "hi" Then Return "sb_hi"
        Return "sb_en"
    End Function

    Public Shared Function GetApprovedPrintBillTemplate(ByVal languageType As String, ByVal messageMode As String, ByRef templateCode As String, ByRef languageCode As String, ByRef parameterCount As Integer, ByRef parameterFields As String, ByRef errorMessage As String) As Boolean
        EnsureDatabase()
        languageCode = ResolveLanguageCode(languageType)

        Dim modeCode As String = ResolvePrintBillMode(messageMode)
        Dim mappedTemplateCode As String = ""
        Dim mappingKey As String = "PRINT_BILL_" & If(languageCode = "hi", "HI", "EN")
        Dim dt As DataTable = ExecDataTable("Select TemplateCode, ParameterFields From TemplateMappings Where MappingKey='" & SqlText(mappingKey) & "' Limit 1")
        If dt.Rows.Count > 0 Then
            templateCode = SafeValue(dt.Rows(0)("TemplateCode").ToString())
            mappedTemplateCode = templateCode
            parameterFields = SafeValue(dt.Rows(0)("ParameterFields").ToString())
        End If
        dt.Dispose()

        If templateCode <> "" AndAlso IsTemplateApproved(templateCode, languageCode, parameterCount) Then Return True

        mappingKey = "PRINT_BILL_" & If(languageCode = "hi", "HI", "EN") & "_" & modeCode
        dt = ExecDataTable("Select TemplateCode, ParameterFields From TemplateMappings Where MappingKey='" & SqlText(mappingKey) & "' Limit 1")
        If dt.Rows.Count > 0 Then
            templateCode = SafeValue(dt.Rows(0)("TemplateCode").ToString())
            If mappedTemplateCode = "" Then mappedTemplateCode = templateCode
            parameterFields = SafeValue(dt.Rows(0)("ParameterFields").ToString())
        End If
        dt.Dispose()

        If templateCode <> "" AndAlso IsTemplateApproved(templateCode, languageCode, parameterCount) Then Return True

        Dim fallbackType As String = If(modeCode = "PDF_ONLY", "print_bill_pdf_only", "print_bill_pdf_message")
        templateCode = ExecScalarStr("Select TemplateCode From PredefinedTemplates Where TemplateType='" & SqlText(fallbackType) & "' And LanguageCode='" & SqlText(languageCode) & "' And (IsApproved=1 Or Upper(IfNull(Status,'')) Like '%APPROVED%' Or Upper(IfNull(MetaStatus,'')) Like '%APPROVED%') Order By IsDefault Desc, TemplateName Limit 1")
        If templateCode <> "" AndAlso IsTemplateApproved(templateCode, languageCode, parameterCount) Then Return True

        templateCode = ExecScalarStr("Select TemplateCode From PredefinedTemplates Where TemplateType='print_bill' And LanguageCode='" & SqlText(languageCode) & "' And (IsApproved=1 Or Upper(IfNull(Status,'')) Like '%APPROVED%' Or Upper(IfNull(MetaStatus,'')) Like '%APPROVED%') Order By IsDefault Desc, TemplateName Limit 1")
        If templateCode <> "" AndAlso IsTemplateApproved(templateCode, languageCode, parameterCount) Then Return True

        dt = ExecDataTable("Select TemplateCode, LanguageCode, ParameterCount From PredefinedTemplates Where TemplateType='print_bill' And IfNull(HeaderType,'')='document' And SupportsFile=1 And (IsApproved=1 Or Upper(IfNull(Status,'')) Like '%APPROVED%' Or Upper(IfNull(MetaStatus,'')) Like '%APPROVED%') Order By Case When LanguageCode='" & SqlText(languageCode) & "' Then 0 Else 1 End, Case When Lower(TemplateCode) Like '%hi%' Or Lower(TemplateName) Like '%hindi%' Then 0 Else 1 End, IsDefault Desc, TemplateName Limit 1")
        If dt.Rows.Count > 0 Then
            templateCode = SafeValue(dt.Rows(0)("TemplateCode").ToString())
            languageCode = SafeValue(dt.Rows(0)("LanguageCode").ToString())
            parameterCount = Val(dt.Rows(0)("ParameterCount").ToString())
            If parameterFields = "" Then parameterFields = "account_name,bill_date,company_name,bill_total"
            dt.Dispose()
            Return True
        End If
        dt.Dispose()

        Dim displayTemplateCode As String = If(mappedTemplateCode <> "", mappedTemplateCode, templateCode)
        errorMessage = "No approved Print Bill document template found. MappingKey: " & mappingKey & ", TemplateCode: " & displayTemplateCode & ". Please approve or sync a Print Bill document template."
        templateCode = ""
        parameterCount = 0
        parameterFields = ""
        Return False
    End Function

    Public Shared Sub SaveTemplateMapping(ByVal mappingKey As String, ByVal moduleName As String, ByVal displayName As String, ByVal templateCode As String, ByVal languageCode As String, ByVal messageMode As String, ByVal parameterFields As String)
        EnsureDatabase()
        parameterFields = NormalizeParameterFieldsText(parameterFields)
        ExecNonQuery("Insert Or Replace Into TemplateMappings(MappingKey, ModuleName, DisplayName, TemplateCode, LanguageCode, MessageMode, ParameterFields, UpdatedAt) Values('" & SqlText(mappingKey) & "','" & SqlText(moduleName) & "','" & SqlText(displayName) & "','" & SqlText(templateCode) & "','" & SqlText(languageCode) & "','" & SqlText(messageMode) & "','" & SqlText(parameterFields) & "', datetime('now'))")
    End Sub

    Public Shared Function SyncTemplatesFromApi(ByVal vendorUid As String, ByVal accessToken As String, ByRef responseMessage As String) As Boolean
        Try
            EnsureDatabase()
            Dim url As String = WhatsAppOfficialApi.BaseUrl & SafeValue(vendorUid) & "/whatsapp/predefined-templates?token=" & Uri.EscapeDataString(SafeValue(accessToken))
            Dim responseString As String = WinHttpHelper.GetData(url)
            If SafeValue(responseString) = "" Then
                responseMessage = "Templates API returned a blank response."
                Return False
            End If
            If responseString.Trim().StartsWith("{") = False Then
                responseMessage = "Templates API did not return a JSON response."
                Return False
            End If
            Dim responseJson As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(responseString)
            If responseJson("result") Is Nothing OrElse responseJson("result").ToString().ToLower() <> "success" Then
                If responseJson("message") IsNot Nothing Then responseMessage = responseJson("message").ToString() Else responseMessage = responseString
                Return False
            End If
            Dim data As Newtonsoft.Json.Linq.JObject = CType(responseJson("data"), Newtonsoft.Json.Linq.JObject)
            Dim templates As Newtonsoft.Json.Linq.JArray = CType(data("templates"), Newtonsoft.Json.Linq.JArray)
            If templates Is Nothing Then
                responseMessage = "Templates data is blank."
                Return False
            End If
            ResetTemplateMetaState()
            For Each item As Newtonsoft.Json.Linq.JObject In templates
                Dim templateType As String = ResolveSyncedTemplateType(ReadJson(item, "template_code"), ReadJson(item, "template_type"))
                UpsertTemplate(ReadJson(item, "template_code"), ReadJson(item, "title"), ReadJson(item, "language_code"), templateType, Val(ReadJson(item, "parameter_count")), ReadJson(item, "header_type"), If(IsJsonTrue(ReadJson(item, "supports_file")), 1, 0), ReadJson(item, "body"), ReadJson(item, "meta_status"), If(IsJsonTrue(ReadJson(item, "is_approved")), 1, 0), If(IsJsonTrue(ReadJson(item, "is_pending")), 1, 0), If(IsJsonTrue(ReadJson(item, "is_rejected")), 1, 0), GetTemplateDisplayStatus(ReadJson(item, "meta_status"), ReadJson(item, "is_approved"), ReadJson(item, "is_pending"), ReadJson(item, "is_rejected")), ReadJson(item, "body"), ReadJson(item, "footer"), ReadJson(item, "category"), JoinJsonArray(item, "examples"), ReadJson(item, "buttons_json"))
                SaveApiTemplateMapping(templateType, ReadJson(item, "template_code"), ReadJson(item, "language_code"), ReadJson(item, "parameter_fields"))
            Next
            responseMessage = templates.Count.ToString() & " templates synced successfully."
            Return True
        Catch ex As Exception
            responseMessage = ex.Message
            Return False
        End Try
    End Function

    Public Shared Function SyncMetaTemplatesFromApi(ByVal vendorUid As String, ByVal accessToken As String, ByRef responseMessage As String) As Boolean
        Try
            EnsureDatabase()
            Dim responseString As String = ""
            Dim errorMessage As String = ""
            If WhatsAppOfficialApi.GetServerTemplates(vendorUid, accessToken, True, responseString, errorMessage) = False Then
                responseMessage = errorMessage
                Return False
            End If
            Dim responseJson As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(responseString)
            Dim data As Newtonsoft.Json.Linq.JObject = CType(responseJson("data"), Newtonsoft.Json.Linq.JObject)
            Dim templates As Newtonsoft.Json.Linq.JArray = CType(data("templates"), Newtonsoft.Json.Linq.JArray)
            If templates Is Nothing Then
                responseMessage = "Templates data is blank."
                Return False
            End If
            ResetTemplateMetaState()
            For Each item As Newtonsoft.Json.Linq.JObject In templates
                Dim templateType As String = ResolveSyncedTemplateType(ReadJson(item, "template_code"), ReadJson(item, "template_type"))
                UpsertTemplate(ReadJson(item, "template_code"), ReadJson(item, "title"), ReadJson(item, "language_code"), templateType, Val(ReadJson(item, "parameter_count")), ReadJson(item, "header_type"), If(IsJsonTrue(ReadJson(item, "supports_file")), 1, 0), ReadJson(item, "body"), ReadJson(item, "meta_status"), If(IsJsonTrue(ReadJson(item, "is_approved")), 1, 0), If(IsJsonTrue(ReadJson(item, "is_pending")), 1, 0), If(IsJsonTrue(ReadJson(item, "is_rejected")), 1, 0), GetTemplateDisplayStatus(ReadJson(item, "meta_status"), ReadJson(item, "is_approved"), ReadJson(item, "is_pending"), ReadJson(item, "is_rejected")), ReadJson(item, "body"), ReadJson(item, "footer"), ReadJson(item, "category"), JoinJsonArray(item, "examples"), ReadJson(item, "buttons_json"))
                SaveApiTemplateMapping(templateType, ReadJson(item, "template_code"), ReadJson(item, "language_code"), ReadJson(item, "parameter_fields"))
            Next
            responseMessage = templates.Count.ToString() & " templates synced from Meta/server."
            Return True
        Catch ex As Exception
            responseMessage = ex.Message
            Return False
        End Try
    End Function

    Public Shared Sub SaveLocalTemplate(ByVal templateCode As String, ByVal title As String, ByVal languageCode As String, ByVal templateType As String, ByVal headerType As String, ByVal bodyText As String, ByVal footerText As String, ByVal category As String, ByVal examples As String, Optional ByVal buttonsJson As String = "")
        EnsureDatabase()
        templateType = ResolveSyncedTemplateType(templateCode, templateType)
        Dim countText As Integer = CountBodyParameters(bodyText)
        If SafeValue(footerText).Trim() = "" Then footerText = DefaultTemplateFooter
        headerType = NormalizeHeaderType(headerType)
        Dim normalizedLanguageCode As String = NormalizeLanguageCode(languageCode)
        Dim existing As DataTable = ExecDataTable("Select MetaStatus, IsApproved, IsPending, IsRejected, Status From PredefinedTemplates Where TemplateCode='" & SqlText(templateCode) & "' And LanguageCode='" & SqlText(normalizedLanguageCode) & "' Limit 1")
        Dim metaStatus As String = "LOCAL"
        Dim statusText As String = "LOCAL"
        Dim isApproved As Integer = 0
        Dim isPending As Integer = 0
        Dim isRejected As Integer = 0

        If existing.Rows.Count > 0 Then
            metaStatus = SafeValue(existing.Rows(0)("MetaStatus").ToString())
            statusText = SafeValue(existing.Rows(0)("Status").ToString())
            isApproved = Val(existing.Rows(0)("IsApproved").ToString())
            isPending = Val(existing.Rows(0)("IsPending").ToString())
            isRejected = Val(existing.Rows(0)("IsRejected").ToString())

            If metaStatus.Trim() = "" Then metaStatus = "LOCAL"
            If statusText.Trim() = "" Then statusText = metaStatus
        End If
        existing.Dispose()

        UpsertTemplate(templateCode, title, normalizedLanguageCode, templateType, countText, headerType, If(headerType = "document" Or headerType = "image" Or headerType = "video", 1, 0), bodyText, metaStatus, isApproved, isPending, isRejected, statusText, bodyText, footerText, category, examples, buttonsJson)
    End Sub

    Private Shared Function NormalizeHeaderType(ByVal headerType As String) As String
        headerType = SafeValue(headerType).Trim().ToLower()
        If headerType = "none" Then Return ""
        Return headerType
    End Function

    Public Shared Function GetTemplateDetail(ByVal templateCode As String) As DataTable
        EnsureDatabase()
        Return ExecDataTable("Select * From PredefinedTemplates Where TemplateCode='" & SqlText(templateCode) & "'")
    End Function

    Public Shared Sub AddMessageLog(ByVal mobileNo As String, ByVal templateCode As String, ByVal messageMode As String, ByVal status As String, ByVal responseMessage As String)
        EnsureDatabase()
        Dim con As SQLiteConnection = New SQLiteConnection(ConString)
        con.Open()
        Dim sql As String = "Insert Into MessageLog(MobileNo, TemplateCode, MessageMode, Status, ResponseMessage, CreatedAt) Values(@MobileNo, @TemplateCode, @MessageMode, @Status, @ResponseMessage, datetime('now'))"
        Dim cmd As SQLiteCommand = New SQLiteCommand(sql, con)
        cmd.Parameters.AddWithValue("@MobileNo", SafeValue(mobileNo))
        cmd.Parameters.AddWithValue("@TemplateCode", SafeValue(templateCode))
        cmd.Parameters.AddWithValue("@MessageMode", SafeValue(messageMode))
        cmd.Parameters.AddWithValue("@Status", SafeValue(status))
        cmd.Parameters.AddWithValue("@ResponseMessage", SafeValue(responseMessage))
        cmd.ExecuteNonQuery()
        cmd.Dispose()
        con.Dispose()
    End Sub

    Private Shared Sub UpsertTemplate(ByVal code As String, ByVal name As String, ByVal languageCode As String, ByVal templateType As String, ByVal parameterCount As Integer, ByVal headerType As String, ByVal supportsFile As Integer, ByVal description As String, ByVal metaStatus As String, ByVal isApproved As Integer, ByVal isPending As Integer, ByVal isRejected As Integer, ByVal statusText As String, Optional ByVal bodyText As String = "", Optional ByVal footerText As String = "", Optional ByVal category As String = "UTILITY", Optional ByVal examples As String = "", Optional ByVal buttonsJson As String = "")
        If code = "" Then Exit Sub
        ExecNonQuery("Insert Or Replace Into PredefinedTemplates(TemplateCode, TemplateName, LanguageCode, TemplateType, ParameterCount, IsDefault, HeaderType, SupportsFile, Description, MetaStatus, IsApproved, IsPending, IsRejected, Status, BodyText, FooterText, Category, Examples, ButtonsJson) Values('" & SqlText(code) & "','" & SqlText(name) & "','" & SqlText(NormalizeLanguageCode(languageCode)) & "','" & SqlText(templateType) & "'," & parameterCount & "," & If(templateType = "sale_bill", 1, 0) & ",'" & SqlText(headerType) & "'," & supportsFile & ",'" & SqlText(description) & "','" & SqlText(metaStatus) & "'," & isApproved & "," & isPending & "," & isRejected & ",'" & SqlText(statusText) & "','" & SqlText(bodyText) & "','" & SqlText(footerText) & "','" & SqlText(category) & "','" & SqlText(examples) & "','" & SqlText(buttonsJson) & "')")
    End Sub

    Private Shared Sub SaveApiTemplateMapping(ByVal templateType As String, ByVal templateCode As String, ByVal languageCode As String, ByVal parameterFields As String)
        templateType = NormalizeTemplateType(templateType)
        If templateType = "" OrElse templateCode = "" Then Exit Sub
        languageCode = NormalizeLanguageCode(languageCode)
        If parameterFields.Trim() = "" Then parameterFields = DefaultParameterFields(templateType)
        parameterFields = NormalizeParameterFieldsText(parameterFields)
        Dim moduleName As String = templateType.ToUpper()
        Dim mappingKey As String = moduleName & "_" & If(languageCode = "hi", "HI", "EN") & "_" & SafeValue(templateCode).ToUpper()
        Dim existing As DataTable = ExecDataTable("Select ParameterFields, MessageMode From TemplateMappings Where MappingKey='" & SqlText(mappingKey) & "' Limit 1")
        Dim finalParameterFields As String = parameterFields
        Dim finalMessageMode As String = "AUTO"

        If existing.Rows.Count > 0 Then
            Dim existingMode As String = SafeValue(existing.Rows(0)("MessageMode").ToString()).Trim().ToUpper()
            Dim existingFields As String = NormalizeParameterFieldsText(SafeValue(existing.Rows(0)("ParameterFields").ToString()))
            If existingFields <> "" AndAlso existingMode <> "" AndAlso existingMode <> "AUTO" Then
                finalParameterFields = existingFields
                finalMessageMode = existingMode
            End If
        End If
        existing.Dispose()

        SaveTemplateMapping(mappingKey, moduleName, templateType.Replace("_", " ") & " " & If(languageCode = "hi", "Regional", "English"), templateCode, languageCode, finalMessageMode, finalParameterFields)
    End Sub

    Private Shared Function NormalizeParameterFieldsText(ByVal parameterFields As String) As String
        Dim result As New List(Of String)()
        For Each rawField As String In If(parameterFields, "").Split(","c)
            Dim key As String = NormalizeParameterFieldKey(rawField)
            If key <> "" Then result.Add(key)
        Next
        Return String.Join(",", result.ToArray())
    End Function

    Private Shared Function NormalizeParameterFieldKey(ByVal fieldKey As String) As String
        Dim key As String = SafeValue(fieldKey).ToLower().Replace(" ", "_").Replace("/", "_")
        key = System.Text.RegularExpressions.Regex.Replace(key, "_+", "_")
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

    Private Shared Sub SeedTemplates()
        If Val(ExecScalarStr("Select Count(*) From PredefinedTemplates")) > 0 Then Exit Sub
        InsertTemplate("sb_en", "Sale Bill", "en", "sale_bill", 5, 1, "Sale bill utility template")
        ExecNonQuery("Update PredefinedTemplates Set HeaderType='document', SupportsFile=1 Where TemplateCode='sb_en'")
        InsertTemplate("sb_hi", "Sale Bill Hindi", "hi", "sale_bill", 5, 1, "Sale bill Hindi utility template")
        ExecNonQuery("Update PredefinedTemplates Set HeaderType='document', SupportsFile=1 Where TemplateCode='sb_hi'")
        InsertTemplate("pay_en", "Payment Receipt", "en", "payment", 5, 0, "Payment receipt utility template")
        InsertTemplate("pay_hi", "Payment Receipt Hindi", "hi", "payment", 5, 0, "Payment receipt Hindi utility template")
        InsertTemplate("bal_en", "Balance", "en", "balance", 5, 0, "Balance utility template")
        InsertTemplate("bal_hi", "Balance Hindi", "hi", "balance", 5, 0, "Balance Hindi utility template")
        InsertTemplate("stmt_en", "Statement", "en", "statement", 5, 0, "Statement utility template")
        InsertTemplate("stmt_hi", "Statement Hindi", "hi", "statement", 5, 0, "Statement Hindi utility template")
    End Sub

    Private Shared Sub InsertTemplate(ByVal code As String, ByVal name As String, ByVal languageCode As String, ByVal templateType As String, ByVal parameterCount As Integer, ByVal isDefault As Integer, ByVal description As String)
        ExecNonQuery("Insert Or Ignore Into PredefinedTemplates(TemplateCode, TemplateName, LanguageCode, TemplateType, ParameterCount, IsDefault, HeaderType, SupportsFile, Description) Values('" & SqlText(code) & "','" & SqlText(name) & "','" & SqlText(languageCode) & "','" & SqlText(templateType) & "'," & parameterCount & "," & isDefault & ",'',0,'" & SqlText(description) & "')")
    End Sub

    Private Shared Sub SeedLocalPredefinedTemplates()
        InsertLocalTemplate("bal_en", "Balance", "en", "balance", 4, "", 0, "Hello {{2}}, your account balance with {{1}} as on {{3}} is {{4}}. Thank you")
        InsertLocalTemplate("bal_hi", "Balance Hindi", "hi", "balance", 4, "", 0, "नमस्ते *{{2}}*, फर्म *{{1}}* में दिनांक *{{3}}* को आपका खाता बैलेंस *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("bill_en", "Print Bill", "en", "print_bill", 4, "document", 1, "Hello {{2}}, your bill dated {{3}} from {{1}} is ready. Total amount is {{4}}. Thank you")
        InsertLocalTemplate("bill_hi", "Print Bill Hindi", "hi", "print_bill", 4, "document", 1, "नमस्ते *{{1}}*, आपका बिल दिनांक *{{2}}* फर्म *{{3}}* तैयार है।" & vbCrLf & "बिल की कुल रकम *{{4}}* है।" & vbCrLf & "धन्यवाद")
        InsertLocalTemplate("ledger_en", "Ledger", "en", "ledger", 4, "document", 1, "Hello {{2}}, your ledger with {{1}} from {{3}} to {{4}} is ready. Thank you")
        InsertLocalTemplate("ledger_hi", "Ledger Hindi", "hi", "ledger", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* में दिनांक *{{3}}* से *{{4}}* तक आपका लेजर तैयार है। धन्यवाद")
        InsertLocalTemplate("rec_en", "Receipt", "en", "receipt", 4, "document", 1, "Hello {{2}}, your receipt dated {{3}} from {{1}} is ready. Amount is {{4}}. Thank you")
        InsertLocalTemplate("rec_hi", "Receipt Hindi", "hi", "receipt", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* से दिनांक *{{3}}* की जमा रसीद तैयार है। रकम *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("pay_en", "Payment", "en", "payment", 4, "document", 1, "Hello {{2}}, your payment receipt dated {{3}} from {{1}} is ready. Amount is {{4}}. Thank you")
        InsertLocalTemplate("pay_hi", "Payment Hindi", "hi", "payment", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* से दिनांक *{{3}}* की भुगतान रसीद तैयार है। रकम *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("pur_en", "Purchase", "en", "purchase", 4, "document", 1, "Hello {{2}}, your purchase dated {{3}} from {{1}} is ready. Total amount is {{4}}. Thank you")
        InsertLocalTemplate("pur_hi", "Purchase Hindi", "hi", "purchase", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* से दिनांक *{{3}}* की खरीद तैयार है। कुल रकम *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("cratein_en", "Crate In", "en", "crate_in", 4, "document", 1, "Hello {{2}}, your crate in dated {{3}} from {{1}} is ready. Crate qty is {{4}}. Thank you")
        InsertLocalTemplate("cratein_hi", "Crate In Hindi", "hi", "crate_in", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* में दिनांक *{{3}}* की क्रेट इन एंट्री तैयार है। क्रेट संख्या *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("crateout_en", "Crate Out", "en", "crate_out", 4, "document", 1, "Hello {{2}}, your crate out dated {{3}} from {{1}} is ready. Crate qty is {{4}}. Thank you")
        InsertLocalTemplate("crateout_hi", "Crate Out Hindi", "hi", "crate_out", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* में दिनांक *{{3}}* की क्रेट आउट एंट्री तैयार है। क्रेट संख्या *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("setledger_en", "Settle Ledger", "en", "settle_ledger", 4, "document", 1, "Hello {{2}}, your settle ledger with {{1}} from {{3}} to {{4}} is ready. Thank you")
        InsertLocalTemplate("setledger_hi", "Settle Ledger Hindi", "hi", "settle_ledger", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* में दिनांक *{{3}}* से *{{4}}* तक आपका सेटल लेजर तैयार है। धन्यवाद")
        InsertLocalTemplate("subledger_en", "Sub Ledger", "en", "sub_ledger", 4, "document", 1, "Hello {{2}}, your sub ledger with {{1}} from {{3}} to {{4}} is ready. Thank you")
        InsertLocalTemplate("subledger_hi", "Sub Ledger Hindi", "hi", "sub_ledger", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* में दिनांक *{{3}}* से *{{4}}* तक आपका सब लेजर तैयार है। धन्यवाद")
        InsertLocalTemplate("sellman_en", "Sellout Manual", "en", "sellout_manual", 4, "document", 1, "Hello {{2}}, your sellout manual dated {{3}} from {{1}} is ready. Total amount is {{4}}. Thank you")
        InsertLocalTemplate("sellman_hi", "Sellout Manual Hindi", "hi", "sellout_manual", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* से दिनांक *{{3}}* का सेलआउट मैनुअल तैयार है। कुल रकम *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("sellauto_en", "Sellout Auto", "en", "sellout_auto", 4, "document", 1, "Hello {{2}}, your sellout auto dated {{3}} from {{1}} is ready. Total amount is {{4}}. Thank you")
        InsertLocalTemplate("sellauto_hi", "Sellout Auto Hindi", "hi", "sellout_auto", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* से दिनांक *{{3}}* का सेलआउट ऑटो तैयार है। कुल रकम *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("stdsale_en", "Standard Sale", "en", "standard_sale", 4, "document", 1, "Hello {{2}}, your standard sale dated {{3}} from {{1}} is ready. Total amount is {{4}}. Thank you")
        InsertLocalTemplate("stdsale_hi", "Standard Sale Hindi", "hi", "standard_sale", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* से दिनांक *{{3}}* की स्टैंडर्ड सेल तैयार है। कुल रकम *{{4}}* है। धन्यवाद")
        InsertLocalTemplate("crateledger_en", "Crate Ledger", "en", "crate_ledger", 4, "document", 1, "Hello {{2}}, your crate ledger with {{1}} from {{3}} to {{4}} is ready. Thank you")
        InsertLocalTemplate("crateledger_hi", "Crate Ledger Hindi", "hi", "crate_ledger", 4, "document", 1, "नमस्ते *{{2}}*, फर्म *{{1}}* में दिनांक *{{3}}* से *{{4}}* तक आपका क्रेट लेजर तैयार है। धन्यवाद")
        ExecNonQuery("Update PredefinedTemplates Set FooterText='" & SqlText(DefaultTemplateFooter) & "' Where (IfNull(Status,'')='LOCAL' Or IfNull(MetaStatus,'')='LOCAL') And IfNull(FooterText,'')=''")
        ExecNonQuery("Update PredefinedTemplates Set FooterText='' Where Upper(IfNull(FooterText,''))='SENDED BY AADHAT SOFTWARE'")
        ExecNonQuery("Update PredefinedTemplates Set FooterText='' Where Upper(IfNull(FooterText,''))='SENT BY AADHAT SOFTWARE'")
        ExecNonQuery("Update PredefinedTemplates Set HeaderType='', SupportsFile=0 Where TemplateType='balance' And IfNull(SupportsFile,0)=0 And Lower(IfNull(HeaderType,''))='text' And Upper(IfNull(Status,'')) Not Like '%APPROVED%' And Upper(IfNull(MetaStatus,'')) Not Like '%APPROVED%'")
    End Sub

    Private Shared Sub InsertLocalTemplate(ByVal code As String, ByVal name As String, ByVal languageCode As String, ByVal templateType As String, ByVal parameterCount As Integer, ByVal headerType As String, ByVal supportsFile As Integer, ByVal bodyText As String)
        Dim buttonsJson As String = DefaultQuickReplyButtonsJson(languageCode)
        ExecNonQuery("Insert Or Ignore Into PredefinedTemplates(TemplateCode, TemplateName, LanguageCode, TemplateType, ParameterCount, IsDefault, HeaderType, SupportsFile, Description, MetaStatus, IsApproved, IsPending, IsRejected, Status, BodyText, FooterText, Category, Examples, ButtonsJson) Values('" & SqlText(code) & "','" & SqlText(name) & "','" & SqlText(NormalizeLanguageCode(languageCode)) & "','" & SqlText(templateType) & "'," & parameterCount & ",0,'" & SqlText(headerType) & "'," & supportsFile & ",'Local predefined Aadhat template','LOCAL',0,0,0,'LOCAL','" & SqlText(bodyText) & "','" & SqlText(DefaultTemplateFooter) & "','UTILITY','','" & SqlText(buttonsJson) & "')")
        ExecNonQuery("Update PredefinedTemplates Set TemplateName='" & SqlText(name) & "', LanguageCode='" & SqlText(NormalizeLanguageCode(languageCode)) & "', TemplateType='" & SqlText(templateType) & "', ParameterCount=" & parameterCount & ", HeaderType='" & SqlText(headerType) & "', SupportsFile=" & supportsFile & ", Description='Local predefined Aadhat template', MetaStatus='LOCAL', Status='LOCAL', BodyText='" & SqlText(bodyText) & "', FooterText='" & SqlText(DefaultTemplateFooter) & "', Category='UTILITY', ButtonsJson='" & SqlText(buttonsJson) & "' Where TemplateCode='" & SqlText(code) & "' And IfNull(IsApproved,0)=0 And IfNull(IsPending,0)=0 And IfNull(IsRejected,0)=0 And Upper(IfNull(Status,'')) Not Like '%APPROVED%' And Upper(IfNull(Status,'')) Not Like '%PENDING%' And Upper(IfNull(Status,'')) Not Like '%REJECT%'")
    End Sub

    Private Shared Sub SeedDefaultTemplateMappings()
        InsertTemplateMapping("PRINT_BILL_EN", "PRINT_BILL", "Print Bill English", "bill_en", "en", "BILL", "account_name,bill_date,company_name,bill_total")
        InsertTemplateMapping("PRINT_BILL_HI", "PRINT_BILL", "Print Bill Regional", "bill_hi", "hi", "BILL", "account_name,bill_date,company_name,bill_total")
        InsertTemplateMapping("PRINT_BILL_EN_PDF_ONLY", "PRINT_BILL", "Bill PDF English", "bill_en", "en", "PDF_ONLY", "account_name,bill_date,company_name,bill_total")
        InsertTemplateMapping("PRINT_BILL_HI_PDF_ONLY", "PRINT_BILL", "Bill PDF Hindi", "bill_hi", "hi", "PDF_ONLY", "account_name,bill_date,company_name,bill_total")
        InsertTemplateMapping("PRINT_BILL_EN_PDF_MESSAGE", "PRINT_BILL", "Bill PDF + Message English", "bill_en", "en", "PDF_MESSAGE", "account_name,bill_date,company_name,bill_total")
        InsertTemplateMapping("PRINT_BILL_HI_PDF_MESSAGE", "PRINT_BILL", "Bill PDF + Message Hindi", "bill_hi", "hi", "PDF_MESSAGE", "account_name,bill_date,company_name,bill_total")
        ExecNonQuery("Update TemplateMappings Set TemplateCode='bill_hi', LanguageCode='hi', ParameterFields='account_name,bill_date,company_name,bill_total' Where MappingKey In ('PRINT_BILL_HI','PRINT_BILL_HI_PDF_ONLY','PRINT_BILL_HI_PDF_MESSAGE') And (TemplateCode='bill_en' Or LanguageCode='en' Or ParameterFields='company_name,account_name,bill_date,bill_total')")
        ExecNonQuery("Update TemplateMappings Set ParameterFields='account_name,bill_date,company_name,bill_total' Where ModuleName='PRINT_BILL' And LanguageCode='en' And Lower(TemplateCode) In ('bill_en','bill_en1','bill_en2') And ParameterFields='company_name,account_name,bill_date,bill_total'")
        ExecNonQuery("Update TemplateMappings Set ParameterFields='account_name,bill_date,company_name,bill_total' Where ModuleName='PRINT_BILL' And IfNull(ParameterFields,'')=''")
        ExecNonQuery("Update TemplateMappings Set ParameterFields='account_name,bill_date,company_name,bill_total' Where ModuleName='PRINT_BILL' And ParameterFields In ('account_name,bill_date,bill_total,pdf_link','account_name,bill_date,bill_total,pdf_link,message_text')")
    End Sub

    Private Shared Sub InsertTemplateMapping(ByVal mappingKey As String, ByVal moduleName As String, ByVal displayName As String, ByVal templateCode As String, ByVal languageCode As String, ByVal messageMode As String, ByVal parameterFields As String)
        ExecNonQuery("Insert Or Ignore Into TemplateMappings(MappingKey, ModuleName, DisplayName, TemplateCode, LanguageCode, MessageMode, ParameterFields, UpdatedAt) Values('" & SqlText(mappingKey) & "','" & SqlText(moduleName) & "','" & SqlText(displayName) & "','" & SqlText(templateCode) & "','" & SqlText(languageCode) & "','" & SqlText(messageMode) & "','" & SqlText(parameterFields) & "', datetime('now'))")
    End Sub

    Private Shared Sub NormalizeExistingTemplateTypes()
        ExecNonQuery("Update PredefinedTemplates Set TemplateType='print_bill' Where IfNull(TemplateType,'') In ('','sale_bill') And (Lower(TemplateCode) Like 'bill%' Or Lower(TemplateCode) Like 'sb_%' Or Lower(TemplateName) Like '%bill%')")
        ExecNonQuery("Update PredefinedTemplates Set TemplateType='receipt' Where IfNull(TemplateType,'') In ('','sale_bill') And (Lower(TemplateCode) Like 'rec%' Or Lower(TemplateName) Like '%receipt%')")
        ExecNonQuery("Update PredefinedTemplates Set TemplateType='payment' Where IfNull(TemplateType,'') In ('','sale_bill') And (Lower(TemplateCode) Like 'pay%' Or Lower(TemplateName) Like '%payment%')")
        ExecNonQuery("Update PredefinedTemplates Set TemplateType='balance' Where IfNull(TemplateType,'') In ('','sale_bill') And (Lower(TemplateCode) Like 'bal%' Or Lower(TemplateName) Like '%balance%')")
        ExecNonQuery("Update PredefinedTemplates Set TemplateType='statement' Where IfNull(TemplateType,'') In ('','sale_bill') And (Lower(TemplateCode) Like 'stmt%' Or Lower(TemplateName) Like '%statement%')")
        ExecNonQuery("Update PredefinedTemplates Set TemplateType='purchase' Where IfNull(TemplateType,'') In ('','sale_bill') And (Lower(TemplateCode) Like 'pur%' Or Lower(TemplateName) Like '%purchase%')")
        ExecNonQuery("Update PredefinedTemplates Set TemplateType='crate_ledger' Where IfNull(TemplateType,'') In ('','sale_bill','print_bill') And (Lower(TemplateCode) Like 'crate%' Or Lower(TemplateName) Like '%crate%')")
    End Sub

    Private Shared Sub EnsureDatabaseLight()
        If File.Exists(DatabasePath) = False Then SQLiteConnection.CreateFile(DatabasePath)
    End Sub

    Private Shared Sub EnsureColumn(ByVal tableName As String, ByVal columnName As String, ByVal columnDefinition As String)
        Dim exists As Boolean = False
        Dim con As SQLiteConnection = New SQLiteConnection(ConString)
        con.Open()
        Dim cmd As SQLiteCommand = New SQLiteCommand("PRAGMA table_info(" & tableName & ")", con)
        Dim reader As SQLiteDataReader = cmd.ExecuteReader()
        While reader.Read()
            If reader("name").ToString().ToLower() = columnName.ToLower() Then
                exists = True
                Exit While
            End If
        End While
        reader.Close()
        cmd.Dispose()
        con.Dispose()
        If exists = False Then ExecNonQuery("Alter Table " & tableName & " Add Column " & columnName & " " & columnDefinition)
    End Sub
    Private Shared Function GetTemplateDisplayStatus(ByVal metaStatus As String, ByVal approved As String, ByVal pending As String, ByVal rejected As String) As String
        If SafeValue(approved).ToLower() = "true" Then Return "APPROVED"
        If SafeValue(pending).ToLower() = "true" Then Return "PENDING"
        If SafeValue(rejected).ToLower() = "true" Then Return "REJECTED"
        If SafeValue(metaStatus) <> "" Then Return SafeValue(metaStatus)
        Return "LOCAL"
    End Function


    Private Shared Function ReadJson(ByVal item As Newtonsoft.Json.Linq.JObject, ByVal key As String) As String
        If item Is Nothing Then Return ""
        If item(key) Is Nothing Then Return ""
        Return item(key).ToString()
    End Function

    Private Shared Function IsJsonTrue(ByVal value As String) As Boolean
        value = SafeValue(value).Trim().ToLower()
        Return value = "true" OrElse value = "1" OrElse value = "yes" OrElse value = "y"
    End Function

    Public Shared Function DefaultQuickReplyButtonsJson(ByVal languageCode As String) As String
        If NormalizeLanguageCode(languageCode) = "hi" Then
            Return "[{""type"":""QUICK_REPLY"",""text"":""हाँ, सही है""},{""type"":""QUICK_REPLY"",""text"":""नहीं, गलती है""}]"
        End If
        Return "[{""type"":""QUICK_REPLY"",""text"":""Yes, Right""},{""type"":""QUICK_REPLY"",""text"":""No, Wrong""}]"
    End Function

    Private Shared Function GetTemplateLanguageFilter(ByVal languageCode As String) As String
        languageCode = NormalizeLanguageCode(languageCode)
        If languageCode = "hi" Then
            Return "(p.LanguageCode='hi' Or Lower(p.TemplateCode) Like '%hi%' Or Lower(p.TemplateName) Like '%hindi%' Or Lower(p.TemplateName) Like '% hi%')"
        End If
        Return "(p.LanguageCode='en' And Lower(p.TemplateCode) Not Like '%hi%' And Lower(p.TemplateName) Not Like '%hindi%' And Lower(p.TemplateName) Not Like '% hi%')"
    End Function

    Private Shared Function GetTemplateLanguageOrder(ByVal languageCode As String) As String
        languageCode = NormalizeLanguageCode(languageCode)
        If languageCode = "hi" Then
            Return "Case When p.LanguageCode='hi' Then 0 When Lower(p.TemplateCode) Like '%hi%' Or Lower(p.TemplateName) Like '%hindi%' Or Lower(p.TemplateName) Like '% hi%' Then 1 Else 2 End"
        End If
        Return "Case When p.LanguageCode='en' Then 0 Else 1 End"
    End Function

    Private Shared Function NormalizeLanguageCode(ByVal value As String) As String
        value = SafeValue(value).ToLower()
        If value.StartsWith("hi") Then Return "hi"
        Return "en"
    End Function

    Private Shared Function ResolveLanguageCode(ByVal languageType As String) As String
        Dim value As String = SafeValue(languageType).ToLower()
        If value.Contains("hindi") OrElse value.Contains("regional") OrElse value.StartsWith("hi") Then Return "hi"
        Return "en"
    End Function

    Private Shared Function NormalizeTemplateType(ByVal value As String) As String
        value = SafeValue(value).ToLower().Replace(" ", "_")
        If value = "sale_bill" OrElse value = "print_bill_pdf_only" OrElse value = "print_bill_pdf_message" Then Return "print_bill"
        Return value
    End Function

    Private Shared Function ResolvePrintBillMode(ByVal messageMode As String) As String
        Dim value As String = SafeValue(messageMode).ToLower()
        If value.Contains("only") AndAlso value.Contains("pdf") Then Return "PDF_ONLY"
        Return "PDF_MESSAGE"
    End Function

    Private Shared Function IsTemplateApproved(ByVal templateCode As String, ByVal languageCode As String, ByRef parameterCount As Integer) As Boolean
        Dim dt As DataTable = ExecDataTable("Select ParameterCount, IsApproved, Status, MetaStatus From PredefinedTemplates Where TemplateCode='" & SqlText(templateCode) & "' And LanguageCode='" & SqlText(languageCode) & "' Limit 1")
        If dt.Rows.Count = 0 Then
            dt.Dispose()
            Return False
        End If

        parameterCount = Val(dt.Rows(0)("ParameterCount").ToString())
        Dim statusText As String = SafeValue(dt.Rows(0)("Status").ToString()).ToUpper()
        Dim metaStatus As String = SafeValue(dt.Rows(0)("MetaStatus").ToString()).ToUpper()
        Dim approved As Boolean = (Val(dt.Rows(0)("IsApproved").ToString()) = 1 OrElse statusText.Contains("APPROVED") OrElse metaStatus.Contains("APPROVED"))
        dt.Dispose()
        Return approved
    End Function

    Private Shared Function GuessTemplateType(ByVal templateCode As String) As String
        templateCode = SafeValue(templateCode).ToLower()
        If templateCode.StartsWith("bill_") OrElse templateCode.StartsWith("sb_") Then Return "print_bill"
        If templateCode.StartsWith("rec_") Then Return "receipt"
        If templateCode.StartsWith("pay") Then Return "payment"
        If templateCode.StartsWith("bal") Then Return "balance"
        If templateCode.StartsWith("stmt") Then Return "statement"
        If templateCode.StartsWith("cratein") Then Return "crate_in"
        If templateCode.StartsWith("crateout") Then Return "crate_out"
        If templateCode.StartsWith("ledger") Then Return "ledger"
        If templateCode.StartsWith("setledger") Then Return "settle_ledger"
        If templateCode.StartsWith("subledger") Then Return "sub_ledger"
        If templateCode.StartsWith("purreg") Then Return "purchase_register"
        If templateCode.StartsWith("pur") Then Return "purchase"
        If templateCode.StartsWith("stdsale") Then Return "standard_sale"
        If templateCode.StartsWith("stdreg") Then Return "standard_sale_register"
        If templateCode.StartsWith("supreg") Then Return "super_sale_register"
        If templateCode.StartsWith("sellman") Then Return "sellout_manual"
        If templateCode.StartsWith("sellauto") Then Return "sellout_auto"
        If templateCode.StartsWith("crate") Then Return "crate_ledger"
        If templateCode.StartsWith("bill") Then Return "print_bill"
        Return "print_bill"
    End Function

    Private Shared Function ResolveSyncedTemplateType(ByVal templateCode As String, ByVal serverTemplateType As String) As String
        Dim guessedType As String = GuessTemplateType(templateCode)
        Dim normalizedServerType As String = NormalizeTemplateType(serverTemplateType)

        If normalizedServerType = "" Then Return guessedType
        If normalizedServerType = "print_bill" AndAlso guessedType <> "print_bill" Then Return guessedType

        Return normalizedServerType
    End Function

    Private Shared Function DefaultParameterFields(ByVal templateType As String) As String
        templateType = NormalizeTemplateType(templateType)
        Select Case templateType
            Case "print_bill"
                Return "account_name,bill_date,company_name,bill_total"
            Case "balance"
                Return "company_name,account_name,balance_date,balance_amount"
            Case "statement", "ledger", "settle_ledger", "sub_ledger", "purchase_register", "standard_sale_register", "super_sale_register", "crate_ledger"
                Return "company_name,account_name,from_date,to_date"
            Case "crate_in", "crate_out"
                Return "company_name,account_name,entry_date,crate_qty"
            Case "receipt", "payment"
                Return "company_name,account_name,entry_date,amount"
        End Select
        Return "account_name,bill_date,company_name,bill_total"
    End Function

    Private Shared Function JoinJsonArray(ByVal item As Newtonsoft.Json.Linq.JObject, ByVal key As String) As String
        If item Is Nothing OrElse item(key) Is Nothing Then Return ""
        If item(key).Type <> Newtonsoft.Json.Linq.JTokenType.Array Then Return item(key).ToString()
        Dim values As New List(Of String)()
        For Each token As Newtonsoft.Json.Linq.JToken In CType(item(key), Newtonsoft.Json.Linq.JArray)
            values.Add(token.ToString())
        Next
        Return String.Join("|", values.ToArray())
    End Function

    Private Shared Function CountBodyParameters(ByVal bodyText As String) As Integer
        Dim maxValue As Integer = 0
        For Each m As System.Text.RegularExpressions.Match In System.Text.RegularExpressions.Regex.Matches(SafeValue(bodyText), "\{\{(\d+)\}\}")
            maxValue = Math.Max(maxValue, Val(m.Groups(1).Value))
        Next
        Return maxValue
    End Function

    Public Shared Function ProtectCredential(ByVal value As String) As String
        Return SecureCredentialStore.Protect(SafeValue(value))
    End Function

    Public Shared Function UnprotectCredential(ByVal value As String) As String
        Dim plainText As String = ""
        Dim errorMessage As String = ""
        If SecureCredentialStore.TryUnprotect(SafeValue(value), plainText, errorMessage) Then Return plainText
        Return ""
    End Function

    Public Shared Function MaskCredential(ByVal value As String) As String
        Return SecureCredentialStore.Mask(UnprotectCredential(value))
    End Function

    Private Shared Function IsCredentialField(ByVal fieldName As String) As Boolean
        Return fieldName = "VendorUid" OrElse fieldName = "AccessToken" OrElse fieldName = "TemplateVendorUid"
    End Function

    Private Shared Sub DecryptDataTableField(ByVal dt As DataTable, ByVal fieldName As String)
        If dt Is Nothing OrElse dt.Columns.Contains(fieldName) = False Then Exit Sub
        For Each row As DataRow In dt.Rows
            row(fieldName) = UnprotectCredential(row(fieldName).ToString())
        Next
    End Sub

    Private Shared Sub MigrateProtectedCredentials()
        Try
            ExecNonQuery("Insert Or Ignore Into ApiSettings(ID) Values(1)")
            ProtectApiSettingsColumn("VendorUid")
            ProtectApiSettingsColumn("AccessToken")
            ProtectApiSettingsColumn("TemplateVendorUid")
        Catch ex As Exception
        End Try
    End Sub

    Private Shared Sub ProtectApiSettingsColumn(ByVal fieldName As String)
        If IsCredentialField(fieldName) = False Then Exit Sub
        Dim value As String = ExecScalarStr("Select " & fieldName & " From ApiSettings Where ID=1")
        If value.Trim() = "" OrElse SecureCredentialStore.IsProtected(value) Then Exit Sub
        ExecNonQuery("Update ApiSettings Set " & fieldName & "='" & SqlText(ProtectCredential(value)) & "' Where ID=1")
    End Sub

    Private Shared Sub CleanupOfficialApiLogs()
        Try
            Dim logDir As String = Path.Combine(Application.StartupPath, "OfficialApiLogs")
            If Directory.Exists(logDir) = False Then Exit Sub
            For Each logFile As String In Directory.GetFiles(logDir, "*.txt", SearchOption.TopDirectoryOnly)
                Dim text As String = File.ReadAllText(logFile)
                Dim cleaned As String = System.Text.RegularExpressions.Regex.Replace(text, "(?im)^VendorUid:\s*(.+)$", Function(m As System.Text.RegularExpressions.Match) "VendorUid: " & SecureCredentialStore.Mask(m.Groups(1).Value))
                cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, "(?im)^AccessToken:\s*(.+)$", "AccessToken: ***")
                cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, "(token=)[^&\s""]+", "$1***")
                If cleaned <> text Then File.WriteAllText(logFile, cleaned)
            Next
        Catch ex As Exception
        End Try
    End Sub

    Private Shared Function SafeValue(ByVal value As String) As String
        If value Is Nothing Then Return ""
        Return value.Trim()
    End Function

    Private Shared Function SqlText(ByVal value As String) As String
        Return SafeValue(value).Replace("'", "''")
    End Function

    Private Shared Function IsSafeFieldName(ByVal fieldName As String) As Boolean
        Select Case fieldName
            Case "VendorUid", "AccessToken", "SendingMethod", "LanguageType", "SendingType", "MsgAccessToken", "DefaultSim", "BaseUrl", "LastConnectedAt", "BusinessStatus", "BusinessInfoText", "LastBusinessInfoAt", "TemplateVendorUid"
                Return True
        End Select
        Return False
    End Function
End Class

