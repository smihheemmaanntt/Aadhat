Imports System.IO
Imports System.Data
Imports System.Collections.Generic
Imports Microsoft.Win32

Public Class CloudBackupSettings
    Public GoogleEnabled As Boolean
    Public OneDriveEnabled As Boolean
    Public GoogleEmail As String = ""
    Public OneDrivePath As String = ""
    Public Schedule As String = "Daily"
    Public SyncOnClose As Boolean = True
    Public LastGoogleBackup As String = ""
    Public LastOneDriveBackup As String = ""
End Class

Public Delegate Sub CloudBackupProgress(ByVal percent As Integer, ByVal message As String)

Public NotInheritable Class CloudBackupManager
    Public Const CloudBackupRootFolderName As String = "AadhatBackup"
    Public Const CloudBackupDailyFolderName As String = "Daily Backup"

    Private Sub New()
    End Sub

    Public Shared Sub EnsureTable()
        ClsFunPrimary.ExecNonQuery("CREATE TABLE IF NOT EXISTS CloudBackupSettings (Id INTEGER PRIMARY KEY, GoogleEnabled INTEGER, OneDriveEnabled INTEGER, GoogleEmail TEXT, OneDrivePath TEXT, Schedule TEXT, SyncOnClose INTEGER, LastGoogleBackup TEXT, LastOneDriveBackup TEXT)")
        If ClsFunPrimary.ExecScalarInt("SELECT Count(*) FROM CloudBackupSettings WHERE Id=1") = 0 Then
            ' Migrate settings once from the active company database. Keep the
            ' original company row intact as a safety copy.
            Dim legacy As CloudBackupSettings = LoadLegacyCompanySettings()
            If legacy Is Nothing Then
                ClsFunPrimary.ExecNonQuery("INSERT INTO CloudBackupSettings (Id, GoogleEnabled, OneDriveEnabled, GoogleEmail, OneDrivePath, Schedule, SyncOnClose, LastGoogleBackup, LastOneDriveBackup) VALUES (1, 0, 0, '', '', 'Daily', 1, '', '')")
            Else
                ClsFunPrimary.ExecNonQuery("INSERT INTO CloudBackupSettings (Id, GoogleEnabled, OneDriveEnabled, GoogleEmail, OneDrivePath, Schedule, SyncOnClose, LastGoogleBackup, LastOneDriveBackup) VALUES (1, " & _
                    If(legacy.GoogleEnabled, 1, 0) & ", " & If(legacy.OneDriveEnabled, 1, 0) & ", " & SqlText(legacy.GoogleEmail) & ", " & SqlText(legacy.OneDrivePath) & ", " & _
                    SqlText(legacy.Schedule) & ", " & If(legacy.SyncOnClose, 1, 0) & ", " & SqlText(legacy.LastGoogleBackup) & ", " & SqlText(legacy.LastOneDriveBackup) & ")")
            End If
        End If
    End Sub

    Private Shared Function LoadLegacyCompanySettings() As CloudBackupSettings
        Try
            Dim tableCheck As DataTable = clsFun.ExecDataTable("SELECT name FROM sqlite_master WHERE type='table' AND name='CloudBackupSettings'")
            If tableCheck.Rows.Count = 0 Then Return Nothing

            Dim dt As DataTable = clsFun.ExecDataTable("SELECT * FROM CloudBackupSettings WHERE Id=1")
            If dt.Rows.Count = 0 Then Return Nothing

            Dim row As DataRow = dt.Rows(0)
            Dim result As New CloudBackupSettings()
            result.GoogleEnabled = Val(row("GoogleEnabled").ToString()) = 1
            result.OneDriveEnabled = Val(row("OneDriveEnabled").ToString()) = 1
            result.GoogleEmail = row("GoogleEmail").ToString()
            result.OneDrivePath = row("OneDrivePath").ToString()
            result.Schedule = row("Schedule").ToString()
            result.SyncOnClose = Val(row("SyncOnClose").ToString()) = 1
            result.LastGoogleBackup = row("LastGoogleBackup").ToString()
            result.LastOneDriveBackup = row("LastOneDriveBackup").ToString()
            If String.IsNullOrEmpty(result.Schedule) Then result.Schedule = "Daily"
            Return result
        Catch
            ' The selected company database may not be available yet. The
            ' shared database will still receive a safe default row.
            Return Nothing
        End Try
    End Function

    Private Shared Function SqlText(ByVal value As String) As String
        Return "'" & If(value, "").Replace("'", "''") & "'"
    End Function

    Public Shared Function Load() As CloudBackupSettings
        EnsureTable()
        Dim result As New CloudBackupSettings()
        Dim dt As DataTable = ClsFunPrimary.ExecDataTable("SELECT * FROM CloudBackupSettings WHERE Id=1")
        If dt.Rows.Count > 0 Then
            Dim row As DataRow = dt.Rows(0)
            result.GoogleEnabled = Val(row("GoogleEnabled").ToString()) = 1
            result.OneDriveEnabled = Val(row("OneDriveEnabled").ToString()) = 1
            result.GoogleEmail = row("GoogleEmail").ToString()
            result.OneDrivePath = row("OneDrivePath").ToString()
            result.Schedule = row("Schedule").ToString()
            result.SyncOnClose = Val(row("SyncOnClose").ToString()) = 1
            result.LastGoogleBackup = row("LastGoogleBackup").ToString()
            result.LastOneDriveBackup = row("LastOneDriveBackup").ToString()
        End If
        If String.IsNullOrEmpty(result.Schedule) Then result.Schedule = "Daily"
        Return result
    End Function

    Public Shared Sub Save(ByVal settings As CloudBackupSettings)
        EnsureTable()
        ClsFunPrimary.ExecNonQuery("UPDATE CloudBackupSettings SET GoogleEnabled=" & If(settings.GoogleEnabled, 1, 0) & ", OneDriveEnabled=" & If(settings.OneDriveEnabled, 1, 0) & ", GoogleEmail=" & SqlText(settings.GoogleEmail) & ", OneDrivePath=" & SqlText(settings.OneDrivePath) & ", Schedule=" & SqlText(settings.Schedule) & ", SyncOnClose=" & If(settings.SyncOnClose, 1, 0) & " WHERE Id=1")
    End Sub

    Public Shared Function DetectOneDriveFolder() As String
        If Not IsOneDriveSignedIn() Then Return ""
        Dim candidates() As String = {Environment.GetEnvironmentVariable("OneDrive"), Environment.GetEnvironmentVariable("OneDriveConsumer"), Environment.GetEnvironmentVariable("OneDriveCommercial")}
        For Each folder As String In candidates
            If Not String.IsNullOrEmpty(folder) AndAlso Directory.Exists(folder) Then Return Path.Combine(folder, "Aadhat Backups")
        Next
        Return ""
    End Function

    Public Shared Function IsOneDriveSignedIn() As Boolean
        Try
            Using accountsKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\OneDrive\Accounts")
                If accountsKey Is Nothing Then Return False
                For Each accountName As String In accountsKey.GetSubKeyNames()
                    Using accountKey As RegistryKey = accountsKey.OpenSubKey(accountName)
                        If accountKey Is Nothing Then Continue For
                        Dim userEmail As String = Convert.ToString(accountKey.GetValue("UserEmail", "")).Trim()
                        'Cid can remain after sign-out, so it must not be treated as proof of an active account.
                        If userEmail.Length > 0 Then Return True
                    End Using
                Next
            End Using
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Public Shared Function IsDue(ByVal settings As CloudBackupSettings) As Boolean
        Dim lastBackup As String = If(settings.OneDriveEnabled, settings.LastOneDriveBackup, settings.LastGoogleBackup)
        If String.IsNullOrEmpty(lastBackup) Then Return True
        Dim dateValue As Date
        If Not Date.TryParse(lastBackup, dateValue) Then Return True
        If String.Equals(settings.Schedule, "Weekly", StringComparison.OrdinalIgnoreCase) Then Return dateValue.AddDays(7) <= Date.Now
        Return dateValue.AddDays(1) <= Date.Now
    End Function

    Public Shared Sub MarkGoogleBackupComplete()
        EnsureTable()
        ClsFunPrimary.ExecNonQuery("UPDATE CloudBackupSettings SET LastGoogleBackup=" & SqlText(Date.Now.ToString("yyyy-MM-dd HH:mm:ss")) & " WHERE Id=1")
    End Sub

    Public Shared Function SyncOneDrive(ByVal zipFile As String, ByVal settings As CloudBackupSettings, ByVal progress As CloudBackupProgress) As Boolean
        If String.IsNullOrEmpty(zipFile) Then Throw New ArgumentException("Local backup path is empty.", "zipFile")
        Return SyncOneDriveFolder(Path.GetDirectoryName(zipFile), settings, progress)
    End Function

    'Copies the complete local BackupGoogle folder.  Existing files are replaced,
    'but files that exist only in OneDrive are deliberately never deleted.
    Public Shared Function SyncOneDriveFolder(ByVal sourceFolder As String, ByVal settings As CloudBackupSettings, ByVal progress As CloudBackupProgress) As Boolean
        If Not settings.OneDriveEnabled Then Return True
        If Not IsOneDriveSignedIn() Then Throw New InvalidOperationException("OneDrive is not signed in. No cloud upload was started.")
        If String.IsNullOrEmpty(settings.OneDrivePath) Then Throw New InvalidOperationException("OneDrive backup folder is not configured.")
        If String.IsNullOrEmpty(sourceFolder) OrElse Not Directory.Exists(sourceFolder) Then Throw New DirectoryNotFoundException("Local BackupGoogle folder was not found.")

        Dim allFiles() As String = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories)
        Dim validFiles As New List(Of String)()
        For Each candidate As String In allFiles
            'Never put incomplete archive/work files into a cloud backup.
            If Not String.Equals(Path.GetExtension(candidate), ".tmp", StringComparison.OrdinalIgnoreCase) Then validFiles.Add(candidate)
        Next
        Dim files() As String = validFiles.ToArray()
        Dim backupDate As String = Date.Now.ToString("yyyyMMdd")
        Dim destinationRoot As String = Path.Combine( _
            Path.Combine(Path.Combine(settings.OneDrivePath, CloudBackupRootFolderName), CloudBackupDailyFolderName), _
            backupDate)
        Directory.CreateDirectory(destinationRoot)

        If files.Length = 0 Then
            If progress IsNot Nothing Then progress(100, "OneDrive folder ready: " & CloudBackupRootFolderName & " > " & CloudBackupDailyFolderName & " > " & backupDate)
        Else
            For index As Integer = 0 To files.Length - 1
                Dim sourceFile As String = files(index)
                Dim relativePath As String = sourceFile.Substring(sourceFolder.Length)
                If relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()) OrElse relativePath.StartsWith(Path.AltDirectorySeparatorChar.ToString()) Then relativePath = relativePath.Substring(1)
                Dim destinationFile As String = Path.Combine(destinationRoot, relativePath)
                Dim destinationDirectory As String = Path.GetDirectoryName(destinationFile)
                If Not Directory.Exists(destinationDirectory) Then Directory.CreateDirectory(destinationDirectory)
                File.Copy(sourceFile, destinationFile, True)
                If progress IsNot Nothing Then progress(CInt((index + 1) * 100 / files.Length), "Syncing to OneDrive > " & CloudBackupRootFolderName & " > " & CloudBackupDailyFolderName & " > " & backupDate & ": " & Path.GetFileName(sourceFile))
            Next
        End If
        EnsureTable()
        ClsFunPrimary.ExecNonQuery("UPDATE CloudBackupSettings SET LastOneDriveBackup=" & SqlText(Date.Now.ToString("yyyy-MM-dd HH:mm:ss")) & " WHERE Id=1")
        If progress IsNot Nothing Then progress(100, "Backup saved to OneDrive sync folder: " & CloudBackupRootFolderName & " > " & CloudBackupDailyFolderName & " > " & backupDate)
        Return True
    End Function
End Class
