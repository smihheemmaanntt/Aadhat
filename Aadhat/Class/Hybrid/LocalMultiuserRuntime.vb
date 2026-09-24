Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Windows.Forms
Imports Newtonsoft.Json

Public Class LocalMultiuserSettings
    Public Property Enabled As Boolean
    Public Property Role As String
    Public Property CustomerCode As String
    Public Property AdminPcName As String
    Public Property AdminIpAddress As String
    Public Property ServerDataPath As String
    Public Property ClientDataFile As String
    Public Property AllowedPcCount As Integer
    Public Property SavedAt As String

    Public Shared Function ConfigPath() As String
        Return Path.Combine(Application.StartupPath, "local_multiuser.json")
    End Function

    Public Shared Function Load() As LocalMultiuserSettings
        If Not File.Exists(ConfigPath()) Then Return New LocalMultiuserSettings()
        Try
            Dim json As String = File.ReadAllText(ConfigPath())
            Dim settings As LocalMultiuserSettings = JsonConvert.DeserializeObject(Of LocalMultiuserSettings)(json)
            If settings Is Nothing Then Return New LocalMultiuserSettings()
            Return settings
        Catch
            Return New LocalMultiuserSettings()
        End Try
    End Function

    Public Sub Save()
        SavedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        File.WriteAllText(ConfigPath(), JsonConvert.SerializeObject(Me, Formatting.Indented))
    End Sub
End Class

Public Class LocalMultiuserLicenseSummary
    Public Property HasLicense As Boolean
    Public Property CustomerCode As String
    Public Property LanAllowed As Boolean
    Public Property AllowedPcCount As Integer
    Public Property RegisteredPcCount As Integer
    Public Property CurrentBoardId As String
    Public Property Message As String
End Class

Public Class LocalMultiuserRuntime
    Public Shared Function LicenseSummary() As LocalMultiuserLicenseSummary
        Dim summary As New LocalMultiuserLicenseSummary()
        summary.CurrentBoardId = AccentStorageHelper.GetMotherboardID()
        summary.AllowedPcCount = 1
        summary.RegisteredPcCount = 0
        summary.LanAllowed = False
        summary.HasLicense = False

        Try
            Dim store As FinalStore = AccentStorageHelper.LoadStore()
            If store Is Nothing OrElse store.response_data Is Nothing Then
                summary.Message = "License not found. Apply license first."
                Return summary
            End If

            summary.HasLicense = True
            summary.CustomerCode = SafeText(store.response_data.customer_code)
            summary.AllowedPcCount = Math.Max(1, store.response_data.user_limit)
            summary.LanAllowed = IsYes(store.response_data.lan_supported) OrElse summary.AllowedPcCount > 1
            summary.RegisteredPcCount = CountBoards(If(store.response_data.board_ids, If(store.license_data Is Nothing, "", store.license_data.board_ids)))

            If Not AccentStorageHelper.IsLicenseUsable() Then
                summary.Message = "License is not usable: " & AccentStorageHelper.LastLicenseError
            ElseIf Not summary.LanAllowed Then
                summary.Message = "LAN multiuser permission is not available in this license."
            Else
                summary.Message = "LAN multiuser allowed. " & summary.RegisteredPcCount.ToString() & " of " & summary.AllowedPcCount.ToString() & " PC registered."
            End If
        Catch ex As Exception
            summary.Message = ex.Message
        End Try

        Return summary
    End Function

    Public Shared Function CurrentCompanyDataFile() As String
        Dim value As String = If(GlobalData.ConnectionPath, "").Trim()
        If value = "" OrElse value.ToUpper() = "DATA" Then
            Return Path.Combine(Path.Combine(Application.StartupPath, "Data"), "Data.db")
        End If
        If Path.IsPathRooted(value) Then Return value
        Return Path.Combine(Application.StartupPath, value)
    End Function

    Public Shared Function CurrentCompanyDataFolder() As String
        Dim filePath As String = CurrentCompanyDataFile()
        If filePath = "" Then Return ""
        Return Path.GetDirectoryName(filePath)
    End Function

    Public Shared Function LocalIpAddress() As String
        Try
            For Each address As IPAddress In Dns.GetHostEntry(Dns.GetHostName()).AddressList
                If address.AddressFamily = AddressFamily.InterNetwork AndAlso Not address.ToString().StartsWith("127.") Then
                    Return address.ToString()
                End If
            Next
        Catch
        End Try
        Return ""
    End Function

    Public Shared Function BuildOwnerInstructions(ByVal dataFolder As String) As String
        Dim pc As String = Environment.MachineName
        Dim ip As String = LocalIpAddress()
        Dim shareExample As String = "\\" & pc & "\AadhatData"
        Return "Admin PC: " & pc & Environment.NewLine & _
               "IP Address: " & ip & Environment.NewLine & _
               "Data Folder: " & dataFolder & Environment.NewLine & _
               "Suggested share path for client PCs: " & shareExample & Environment.NewLine & Environment.NewLine & _
               "On client PC, open Multiuser Setup and select Data.db from the shared folder."
    End Function

    Public Shared Sub SaveAdminSetup(ByVal dataFolder As String)
        If dataFolder = "" OrElse Not Directory.Exists(dataFolder) Then Throw New ApplicationException("Please select a valid admin data folder.")
        Dim summary As LocalMultiuserLicenseSummary = LicenseSummary()
        If Not summary.LanAllowed Then Throw New ApplicationException(summary.Message)

        Dim settings As New LocalMultiuserSettings()
        settings.Enabled = True
        settings.Role = "AdminServer"
        settings.CustomerCode = summary.CustomerCode
        settings.AdminPcName = Environment.MachineName
        settings.AdminIpAddress = LocalIpAddress()
        settings.ServerDataPath = dataFolder
        settings.AllowedPcCount = summary.AllowedPcCount
        settings.Save()
    End Sub

    Public Shared Sub SaveClientSetup(ByVal sharedDataFile As String)
        If sharedDataFile = "" OrElse Not File.Exists(sharedDataFile) Then Throw New ApplicationException("Please select shared Data.db from Admin PC.")
        Dim summary As LocalMultiuserLicenseSummary = LicenseSummary()
        If Not summary.LanAllowed Then Throw New ApplicationException(summary.Message)

        Dim folder As String = Path.GetDirectoryName(sharedDataFile)
        UpdateDefaultPath(folder)

        Dim settings As New LocalMultiuserSettings()
        settings.Enabled = True
        settings.Role = "ClientPc"
        settings.CustomerCode = summary.CustomerCode
        settings.AdminPcName = ""
        settings.AdminIpAddress = ""
        settings.ServerDataPath = folder
        settings.ClientDataFile = sharedDataFile
        settings.AllowedPcCount = summary.AllowedPcCount
        settings.Save()
    End Sub

    Public Shared Sub TurnOff()
        Dim settings As LocalMultiuserSettings = LocalMultiuserSettings.Load()
        settings.Enabled = False
        settings.Save()
    End Sub

    Private Shared Sub UpdateDefaultPath(ByVal folder As String)
        Dim safeFolder As String = SqlText(folder)
        Try
            Dim affected As Integer = ClsFunPrimary.ExecNonQuery("Update Path Set DefaultPath='" & safeFolder & "'")
            If affected <= 0 Then
                ClsFunPrimary.ExecNonQuery("Insert Into Path(DefaultPath) Values('" & safeFolder & "')")
            End If
        Catch
            ClsFunPrimary.ExecNonQuery("Create Table If Not Exists Path(DefaultPath TEXT)")
            ClsFunPrimary.ExecNonQuery("Insert Into Path(DefaultPath) Values('" & safeFolder & "')")
        End Try
    End Sub

    Private Shared Function CountBoards(ByVal boardIds As String) As Integer
        If boardIds Is Nothing OrElse boardIds.Trim() = "" Then Return 0
        Dim values() As String = boardIds.Split(","c)
        Dim count As Integer = 0
        Dim seen As New List(Of String)()
        For Each raw As String In values
            Dim value As String = raw.Trim().ToUpperInvariant()
            If value <> "" AndAlso Not seen.Contains(value) Then
                seen.Add(value)
                count += 1
            End If
        Next
        Return count
    End Function

    Private Shared Function IsYes(ByVal value As String) As Boolean
        value = SafeText(value).ToLowerInvariant()
        Return value = "yes" OrElse value = "true" OrElse value = "1" OrElse value = "y"
    End Function

    Private Shared Function SafeText(ByVal value As String) As String
        If value Is Nothing Then Return ""
        Return value.Trim()
    End Function

    Private Shared Function SqlText(ByVal value As String) As String
        If value Is Nothing Then Return ""
        Return value.Replace("'", "''")
    End Function
End Class
