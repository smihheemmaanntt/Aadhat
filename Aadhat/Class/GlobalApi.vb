Imports System.Net
Module GlobalApi
    Sub New()
        ' TLS 1.2 enable for .NET 3.5
        ServicePointManager.SecurityProtocol =
            CType(3072, SecurityProtocolType)
    End Sub
    ' 🔥 ये Base URL सिर्फ एक बार बदलना है
    'Public Const BASE_URL As String = "https://crm.softmanagementindia.in/api/"
    Public Const BASE_URL As String = "http://localhost/HRM_Project_Modified/api/"
    ' 🔥 अब हर API यहाँ से Auto बनेगी
    Public ReadOnly ValidateLicenseUrl As String = BASE_URL & "validate_license.php"
    Public ReadOnly ActivateAmcUrl As String = BASE_URL & "activate_amc.php"
    Public ReadOnly BlockStatusUrl As String = BASE_URL & "check_block_status.php"
    Public ReadOnly ReleaseLicenseUrl As String = BASE_URL & "release_license.php"
    Public ReadOnly RetrieveLicenseUrl As String = BASE_URL & "retrieve_license_PC.php"

End Module
