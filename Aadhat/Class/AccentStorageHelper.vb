Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports Newtonsoft.Json
Imports System.Management
Imports System.Linq

'=====================================================
' MODELS
'=====================================================
Public Class LicenseData
    Public Property firm_name As String
    Public Property address As String
    Public Property city As String
    Public Property state As String
    Public Property mobile1 As String
    Public Property mobile2 As String
    Public Property email As String
    Public Property license_key As String
    Public Property product_id As Integer
    Public Property pc_name As String
    Public Property board_id As String
    Public Property board_ids As String
End Class

Public Class CustomerActivationResponse
    Public Property status As String
    Public Property message As String
    Public Property customer_code As String

    Public Property firm_name As String
    Public Property city As String
    Public Property state As String
    Public Property mobile1 As String
    Public Property mobile2 As String
    Public Property email As String
    Public Property address As String

    Public Property activation_date As String
    Public Property license_effective_from As String

    ' 🔥 EXACT NAMES AS API
    Public Property license_expiry_date As String
    Public Property license_expiry As String
    Public Property expires_on As String

    Public Property license_type As String
    Public Property lan_supported As String
    Public Property user_limit As Integer
    Public Property board_ids As String
End Class


Public Class AmcActivationResponse
    Public Property status As String
    Public Property message As String
    Public Property activation_date As String
    Public Property amc_start_date As String
    Public Property amc_end_date As String
    Public Property amc_days As Integer
End Class

Public Class AmcData
    Public Property customer_code As String
    Public Property license_key As String
    Public Property product_id As Integer
    Public Property board_id As String
    Public Property activation_date As String
    Public Property amc_start As String
    Public Property amc_end As String
    Public Property amc_days As Integer
    Public Property status As String
End Class

Public Class ResponseData
    Public Property status As String
    Public Property message As String
    Public Property customer_code As String
    Public Property activation_date As String
    Public Property license_effective_from As String
    Public Property license_expiry_date As String
    Public Property license_expiry As String
    Public Property expires_on As String
    Public Property license_type As String
    Public Property lan_supported As String
    Public Property user_limit As Integer
    Public Property license_mode As String
    Public Property board_ids As String
End Class

Public Class FinalStore
    Public Property license_data As LicenseData
    Public Property response_data As ResponseData
    Public Property amc As List(Of AmcData)
    Public Property is_blocked As Boolean
End Class

'=====================================================
' MAIN HELPER
'=====================================================
Public Class AccentStorageHelper

    Private Shared storePath As String = Path.Combine(Application.StartupPath, "coreaccess.smx")
    Private Shared AES_KEY As String = "12345678901234567890123456789012"
    Private Shared AES_IV As String = "1234567890123456"
    Public Shared LastLicenseError As String = ""

    '=====================================================
    ' INTERNET CHECK (PING / HTTP)
    '=====================================================
    Public Shared Function IsInternetAvailable() As Boolean
        Try
            Dim pingUrl As String = BASE_URL.Replace("/api/", "/ping.txt")
            Dim req = CType(WebRequest.Create(pingUrl), HttpWebRequest)
            req.Method = "GET"
            req.Timeout = 4000
            req.ReadWriteTimeout = 4000
            req.UserAgent = "AccoBook"

            Using resp = CType(req.GetResponse(), HttpWebResponse)
                Return (resp.StatusCode = HttpStatusCode.OK)
            End Using

        Catch
            Return False
        End Try
    End Function


    '=====================================================
    ' SERVER BLOCK STATUS
    '=====================================================
    Public Shared Function CheckOnlineBlock(customerCode As String) As Boolean?
        Try
            If Not IsInternetAvailable() Then Return Nothing

            Dim url As String = BlockStatusUrl & _
                "?customer_code=" & Uri.EscapeDataString(customerCode) & _
                "&board_id=" & Uri.EscapeDataString(GetMotherboardID())

            Dim json = New WebClient().DownloadString(url)
            Dim obj = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(json)

            If obj Is Nothing OrElse Not obj.ContainsKey("status") OrElse Convert.ToString(obj("status")).ToLower() <> "success" Then
                Return Nothing
            End If

            If obj.ContainsKey("is_blocked") Then
                Dim apiBlocked As Boolean = Convert.ToBoolean(obj("is_blocked"))
                SetLocalBlock(apiBlocked)
                Return apiBlocked
            End If

            Return Nothing
        Catch
            Return Nothing
        End Try
    End Function

    '=====================================================
    ' SET LOCAL BLOCK
    '=====================================================
    Public Shared Sub SetLocalBlock(status As Boolean)
        Dim store = LoadStore()
        store.is_blocked = status
        SaveStore(store)
    End Sub

    Public Shared Function IsLocallyBlocked() As Boolean
        Try
            Dim store = LoadStore()
            If store Is Nothing Then Return False
            Return store.is_blocked
        Catch
            Return False
        End Try
    End Function

    '=====================================================
    ' AES ENCRYPT / DECRYPT
    '=====================================================
    Private Shared Function Encrypt(plain As String) As String
        Using aes As New AesManaged()
            aes.Key = Encoding.ASCII.GetBytes(AES_KEY)
            aes.IV = Encoding.ASCII.GetBytes(AES_IV)
            aes.Mode = CipherMode.CBC
            aes.Padding = PaddingMode.PKCS7
            Dim enc = aes.CreateEncryptor()
            Dim b = Encoding.UTF8.GetBytes(plain)
            Return Convert.ToBase64String(enc.TransformFinalBlock(b, 0, b.Length))
        End Using
    End Function

    Private Shared Function Decrypt(cipher As String) As String
        Try
            Using aes As New AesManaged()
                aes.Key = Encoding.ASCII.GetBytes(AES_KEY)
                aes.IV = Encoding.ASCII.GetBytes(AES_IV)
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7
                Dim dec = aes.CreateDecryptor()
                Dim b = Convert.FromBase64String(cipher)
                Return Encoding.UTF8.GetString(dec.TransformFinalBlock(b, 0, b.Length))
            End Using
        Catch
            Return ""
        End Try
    End Function

    '=====================================================
    ' LOAD / SAVE STORE
    '=====================================================
    Public Shared Function LoadStore() As FinalStore
        If Not File.Exists(storePath) Then
            Return New FinalStore With {.amc = New List(Of AmcData)}
        End If
        Dim json = Decrypt(File.ReadAllText(storePath))
        If json = "" Then Return New FinalStore With {.amc = New List(Of AmcData)}
        Return JsonConvert.DeserializeObject(Of FinalStore)(json)
    End Function

    Public Shared Sub SaveStore(store As FinalStore)
        File.WriteAllText(storePath, Encrypt(JsonConvert.SerializeObject(store, Formatting.Indented)))
    End Sub


    Public Shared Event LicenceStatusChanged(ByVal IsExpired As Boolean)

    Public Shared Function CheckLicence() As Boolean
        Dim IsExpired As Boolean = False

        Dim count As Integer = clsFun.ExecScalarInt("Select OpenTime from Licence")

        If count > 30 Then
            IsExpired = True

        Else
            If count = 0 Then
                clsFun.ExecNonQuery("Insert into Licence(OpenTime,InstallDate) values(" & count + 1 & ",'" & clsFun.GetServerDate & "')")
                IsExpired = False

            Else
                Dim instDate As Date = clsFun.ExecScalarStr("Select InstallDate from Licence")

                If clsFun.ExecScalarInt("Select count(*) from ledger") > 30 Then
                    Dim Expdate As Date = clsFun.ExecScalarStr("Select Max(entrydate) from ledger limit 1")
                    Dim difference As TimeSpan = Expdate.Subtract(instDate)

                    If difference.TotalDays > 7 Then
                        IsExpired = True
                    End If

                Else
                    Dim compdate As Date = clsFun.GetServerDate
                    Dim dif As TimeSpan = compdate.Subtract(instDate)

                    If dif.TotalDays > 7 Then
                        IsExpired = True
                    End If
                End If

                clsFun.ExecNonQuery("Update Licence set OpenTime = OpenTime + 1")
            End If
        End If

        ' ✅ Event fire karo
        RaiseEvent LicenceStatusChanged(IsExpired)

        Return IsExpired
    End Function

    '=====================================================
    ' POST JSON
    '=====================================================
    Public Shared Function PostJson(url As String, obj As Object) As String
        Using wc As New WebClient()
            wc.Headers(HttpRequestHeader.ContentType) = "application/json"
            Return wc.UploadString(url, "POST", JsonConvert.SerializeObject(obj))
        End Using
    End Function

    '    Public Shared Function PostJson(
    '    url As String,
    '    obj As Object
    ') As String

    '        Return WinHttpHelper.PostJson(url, obj)

    '    End Function

    '=====================================================
    ' SAVE LICENSE (FRESH + RESPONSE DRIVEN STORE)
    '=====================================================
    Public Shared Function SaveLicense(data As LicenseData) As String

        Dim res = PostJson(ValidateLicenseUrl, data)
        Dim resp = JsonConvert.DeserializeObject(Of CustomerActivationResponse)(res)

        If resp Is Nothing OrElse resp.status <> "success" Then
            Return res
        End If

        Dim store = LoadStore()

        ' 🔒 MINIMUM MACHINE BINDING (NO BOARDS HERE)
        store.license_data = New LicenseData With {
            .license_key = data.license_key,
            .product_id = data.product_id,
            .pc_name = data.pc_name,
            .board_id = data.board_id
        }

        ' 🔥 CUSTOMER PROFILE – SERVER AUTHORITY
        store.license_data.firm_name = resp.firm_name
        store.license_data.address = resp.address
        store.license_data.city = resp.city
        store.license_data.state = resp.state
        store.license_data.mobile1 = resp.mobile1
        store.license_data.mobile2 = resp.mobile2
        store.license_data.email = resp.email

        ' 🔥 BOARD LIST – SERVER ONLY
        store.license_data.board_ids = resp.board_ids

        ' 🔥 RESPONSE MASTER DATA
        store.response_data = New ResponseData With {
            .status = resp.status,
            .message = resp.message,
            .customer_code = resp.customer_code,
            .activation_date = resp.activation_date,
            .license_effective_from = If(String.IsNullOrEmpty(resp.license_effective_from), resp.activation_date, resp.license_effective_from),
            .license_expiry_date = resp.license_expiry_date,
            .license_expiry = resp.license_expiry,
            .expires_on = resp.expires_on,
            .license_type = resp.license_type,
            .lan_supported = resp.lan_supported,
            .user_limit = resp.user_limit,
            .license_mode = If(resp.user_limit > 1, "Multi User", "Single User"),
            .board_ids = resp.board_ids
        }

        SaveStore(store)
        Return res
    End Function


    '=====================================================
    ' SAVE AMC
    '=====================================================
    Public Shared Function SaveAmc(data As AmcData) As String
        Dim res = PostJson(ActivateAmcUrl, data)
        Dim resp = JsonConvert.DeserializeObject(Of AmcActivationResponse)(res)
        If resp.status <> "success" Then Return res

        Dim store = LoadStore()
        store.response_data.expires_on = resp.amc_end_date
        If store.amc Is Nothing Then store.amc = New List(Of AmcData)
        store.amc.Add(New AmcData With {
            .customer_code = store.response_data.customer_code,
            .product_id = data.product_id,
            .license_key = data.license_key,
            .activation_date = resp.activation_date,
            .amc_start = resp.amc_start_date,
            .amc_end = resp.amc_end_date,
            .amc_days = resp.amc_days,
            .board_id = data.board_id,
            .status = "AMC Activated"
        })
        SaveStore(store)
        Return res
    End Function

    '=====================================================
    ' RELEASE BOARD ONLY
    '=====================================================
    Public Shared Function ReleaseBoardOnly() As Boolean
        Try
            Dim store = LoadStore()
            Dim payload = New With {
                .customer_code = store.response_data.customer_code,
                .board_id = GetMotherboardID(),
                .action = "revoke_board",
                .reason = "Released from VB app"
            }
            Dim res = PostJson(ReleaseLicenseUrl, payload)
            Return res.ToLower().Contains("success")
        Catch
            Return False
        End Try
    End Function

    '=====================================================
    ' RETRIEVE LICENSE
    '=====================================================
    Public Shared Function RetrieveLicense(customerCode As String) As Boolean
        Try
            If Not IsInternetAvailable() Then Return False
            Dim url As String = RetrieveLicenseUrl & _
                "?customer_code=" & Uri.EscapeDataString(customerCode) & _
                "&board_id=" & Uri.EscapeDataString(AccentStorageHelper.GetMotherboardID()) & _
                "&pc_name=" & Uri.EscapeDataString(Environment.MachineName)

            Using wc As New WebClient()
                wc.Headers(HttpRequestHeader.ContentType) = "application/octet-stream"
                Dim data = wc.DownloadData(url)
                If data Is Nothing OrElse data.Length < 50 Then Return False
                File.WriteAllBytes(storePath, data)
                Return True
            End Using
        Catch
            Return False
        End Try
    End Function

    '=====================================================
    ' CHECK LICENSE
    '=====================================================
    Public Shared Function CheckLicense() As Boolean
        Dim store = LoadStore()
        If store Is Nothing OrElse store.license_data Is Nothing Then Return False
        If store.is_blocked Then Return False

        Dim finalExpiry As Date = Date.MinValue

        If store.amc IsNot Nothing AndAlso store.amc.Count > 0 Then
            Date.TryParse(store.amc(store.amc.Count - 1).amc_end, finalExpiry)
        Else
            Date.TryParse(store.response_data.expires_on, finalExpiry)
        End If

        If finalExpiry <> Date.MinValue AndAlso finalExpiry < Date.Today Then
            Return False
        End If

        Return True
    End Function


    '=====================================================
    ' MOTHERBOARD ID
    '=====================================================
    Public Shared Function GetMotherboardID() As String
        Try
            For Each mo As ManagementObject In
                New ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard").Get()
                Return NormalizeBoardId(mo("SerialNumber").ToString())
            Next
        Catch
        End Try
        Return ""
    End Function

    Private Shared Function NormalizeBoardId(value As String) As String
        If value Is Nothing Then Return ""
        Return value.Trim().ToUpperInvariant()
    End Function

    Private Shared Function SplitBoardIds(boardIds As String) As List(Of String)
        If String.IsNullOrEmpty(boardIds) Then Return New List(Of String)

        Return boardIds.Split(","c).
            Select(Function(x) NormalizeBoardId(x)).
            Where(Function(x) x <> "").
            Distinct().
            ToList()
    End Function

    '=====================================================
    ' CHECK CURRENT BOARD AUTHORIZATION (MULTI BOARD SAFE)
    '=====================================================
    Public Shared Function EnsureBoardAuthorized() As Boolean
        Dim store = LoadStore()
        If store Is Nothing OrElse store.response_data Is Nothing Then Return False

        Dim currentBoard As String = NormalizeBoardId(GetMotherboardID())
        If String.IsNullOrEmpty(currentBoard) Then Return False

        Dim boardIds As String = store.response_data.board_ids
        If String.IsNullOrEmpty(boardIds) AndAlso store.license_data IsNot Nothing Then
            boardIds = store.license_data.board_ids
        End If

        If String.IsNullOrEmpty(boardIds) AndAlso store.license_data IsNot Nothing Then
            boardIds = store.license_data.board_id
        End If

        If String.IsNullOrEmpty(boardIds) Then Return False

        Dim boards = SplitBoardIds(boardIds)
        Return boards.Contains(currentBoard)
        'Return boards.Contains("123")
    End Function
    '=====================================================
    ' COMPLETE LICENSE GATE CHECK (LOGIN SAFE)
    '=====================================================
    Public Shared Function IsLicenseUsable() As Boolean

        LastLicenseError = ""

        ' License file + structure
        Dim store = LoadStore()
        If store Is Nothing OrElse store.license_data Is Nothing OrElse store.response_data Is Nothing Then
            LastLicenseError = "missing"
            Return False
        End If

        ' Online hone par sirf block status check hota hai.
        Dim onlineBlock As Boolean? = CheckOnlineBlock(store.response_data.customer_code)
        If onlineBlock.HasValue AndAlso onlineBlock.Value = True Then
            LastLicenseError = "blocked"
            Return False
        End If

        ' Local block
        If IsLocallyBlocked() Then
            LastLicenseError = "blocked"
            Return False
        End If

        ' Board authorization hamesha local coreaccess.smx file se hota hai.
        If Not EnsureBoardAuthorized() Then
            If LastLicenseError = "" Then LastLicenseError = "not_authorized"
            Return False
        End If

        ' Final expiry check (license / AMC)
        If Not CheckLicense() Then
            If LastLicenseError = "" Then LastLicenseError = "expired"
            Return False
        End If

        LastLicenseError = ""
        Return True
    End Function
    '=====================================================
    ' GET REMAINING LICENSE / AMC DAYS
    '=====================================================
    Public Shared Function GetRemainingDays() As Integer
        Try
            Dim store = LoadStore()
            If store Is Nothing OrElse store.response_data Is Nothing Then Return 0

            Dim finalExpiry As Date = Date.MinValue

            If store.amc IsNot Nothing AndAlso store.amc.Count > 0 Then
                For Each a In store.amc
                    Dim d As Date
                    If Date.TryParse(a.amc_end, d) AndAlso d > finalExpiry Then
                        finalExpiry = d
                    End If
                Next
            End If

            If finalExpiry = Date.MinValue Then
                Date.TryParse(store.response_data.expires_on, finalExpiry)
            End If

            If finalExpiry = Date.MinValue Then Return 0

            Dim days = CInt((finalExpiry - Date.Today).TotalDays)
            If days < 0 Then days = 0
            Return days

        Catch
            Return 0
        End Try
    End Function

End Class
