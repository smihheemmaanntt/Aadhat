Imports System.Globalization
Imports System.IO
Imports System.Net
Imports Ionic.Zip
Imports System.Security.Cryptography
Imports Microsoft.Win32
Imports System.Collections.Generic
Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Public Class Backup_On_Server
    Dim zipPath As String
    Dim OringinalPath As String
    Dim ClsCommon As CommonClass = New CommonClass()
    Private isClosingForBackup As Boolean = False
    Private Const ExternalBackupRegistryKey As String = "Software\Aadhat\Backup"
    Private Const ExternalBackupDriveValue As String = "DefaultDrive"
    Private Const ExternalBackupGoogleValue As String = "BackupToGoogleDrive"
    Private Const IoctlVolumeGetVolumeDiskExtents As UInteger = &H560000UI
    Private Const IoctlStorageQueryProperty As UInteger = &H2D1400UI
    Private Const FileShareRead As UInteger = &H1UI
    Private Const FileShareWrite As UInteger = &H2UI
    Private Const OpenExisting As UInteger = 3UI
    Private externalDrivePicker As Form
    Private externalDriveCombo As ComboBox
    Private externalDriveBackupButton As Button
    Private externalDriveScanResults As List(Of DriveInfo)
    Private externalDriveScanError As String
    Private externalDriveScanId As Integer
    Private externalDriveScanInProgress As Integer
    Private externalDriveScanTimer As System.Windows.Forms.Timer
    Private externalDrivePreferredRoot As String
    Private cloudBackupButtonImage As Image
    Private isOnlineMode As Boolean
    Private lastZipBackupSucceeded As Boolean

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Private Shared Function CreateFile(ByVal fileName As String, ByVal desiredAccess As UInteger, ByVal shareMode As UInteger, ByVal securityAttributes As IntPtr, ByVal creationDisposition As UInteger, ByVal flagsAndAttributes As UInteger, ByVal templateFile As IntPtr) As SafeFileHandle
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Shared Function DeviceIoControl(ByVal device As SafeFileHandle, ByVal controlCode As UInteger, ByVal inputBuffer As Byte(), ByVal inputBufferSize As UInteger, ByVal outputBuffer As Byte(), ByVal outputBufferSize As UInteger, ByRef bytesReturned As UInteger, ByVal overlapped As IntPtr) As Boolean
    End Function

    Private Delegate Function CopyProgressRoutine(ByVal totalFileSize As Long, ByVal totalBytesTransferred As Long, ByVal streamSize As Long, ByVal streamBytesTransferred As Long, ByVal streamNumber As UInteger, ByVal callbackReason As UInteger, ByVal sourceHandle As IntPtr, ByVal destinationHandle As IntPtr, ByVal callbackData As IntPtr) As UInteger

    <DllImport("kernel32.dll", EntryPoint:="CopyFileExW", CharSet:=CharSet.Unicode, SetLastError:=True)> _
    Private Shared Function CopyFileExNative(ByVal existingFileName As String, ByVal newFileName As String, ByVal progressRoutine As CopyProgressRoutine, ByVal callbackData As IntPtr, <MarshalAs(UnmanagedType.Bool)> ByRef cancel As Boolean, ByVal copyFlags As UInteger) As Boolean
    End Function

    Private Sub ButtonControl()
        For Each b As Button In Me.Controls.OfType(Of Button)()
            If b.Enabled = True Then
                b.Enabled = False
            Else
                b.Enabled = True
            End If
        Next
    End Sub

    Private Sub Backup_On_Server_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isClosingForBackup Then Exit Sub
        isClosingForBackup = True
        ' Closing this screen must not start any backup. Backups are started only
        ' by the user's explicit backup buttons.
        Application.Exit()
    End Sub

    Private Sub Backup_On_Server_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Backup_On_Server_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Capture and apply this form's responsive layout before any work that
        ' can delay Load (such as the online update check).  This prevents the
        ' visible late right-to-left layout jump without changing other forms.
        'GenericResponsiveFormScaler.Bind(Me)
        ' System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType)
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        OringinalPath = GlobalData.ConnectionPath
        lblOriginalPath.Text = "Data Path : " & OringinalPath
        cloudBackupButtonImage = Button3.Image
        isOnlineMode = ClsCommon.IsInternetConnect()
        ApplyConnectivityMode(isOnlineMode)
        LoadBanner(isOnlineMode)
        If isOnlineMode Then CheckForUpdate()
    End Sub

    Private Sub ApplyConnectivityMode(ByVal isOnline As Boolean)
        isOnlineMode = isOnline
        Button3.Visible = True
        Label5.Visible = isOnline
        Label1.Visible = isOnline
        Label2.Visible = isOnline
        lblMsg2.Visible = False
        If isOnline Then
            Button3.Text = "Backup &&" & Environment.NewLine & "Sync Cloud"
            Button3.Image = cloudBackupButtonImage
        Else
            Button3.Text = "Create Local" & Environment.NewLine & "Backup"
            Button3.Image = Nothing
        End If
    End Sub

    Private Sub CheckForUpdate()
        If CheckInternetConnection() = False Then Exit Sub
        Try
            Dim updateUrl As String = "https://softmanagementindia.in/updates/aadhat-update.info.txt"
            ' Server version (AssemblyVersion format)
            Dim serverVersionString As String = ""
            If AccentStorageHelper.UseLegacyHttpMode() Then
                serverVersionString = WinHttpHelper.GetData(updateUrl).Trim()
            Else
                Try
                    Dim wc As New WebClient()
                    serverVersionString = wc.DownloadString(updateUrl).Trim()
                Catch
                    Throw
                End Try
            End If

            If serverVersionString = "" Then Exit Sub

            ' Local version (Auto incremented AssemblyVersion)
            Dim localVersion As New Version(GetAssemblyVersion())
            Dim serverVersion As New Version(serverVersionString)
            'If serverVersion = localVersion Then Exit Sub
            If serverVersion > localVersion Then
                btnUpdate.Visible = True
                btnUpdate.Text = "Update Aadhat (" & serverVersion.ToString() & ")"
            Else
                btnUpdate.Visible = False
            End If
        Catch ex As Exception
            btnUpdate.Visible = False
        End Try
    End Sub

    Private Function GetAssemblyVersion() As String
        Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
    End Function
    'Private Sub LoadBanner()
    '    Try
    '        Dim bannerUrl As String = "http://softmanagementindia.in/banners/banner.jpg"
    '        Using wc As New WebClient()
    '            Dim data As Byte() = wc.DownloadData(bannerUrl)
    '            Application.DoEvents()
    '            Using ms As New IO.MemoryStream(data)
    '                If pb1.Image IsNot Nothing Then pb1.Image.Dispose()
    '                pb1.Image = Image.FromStream(ms)
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        MessageBox.Show("Banner load error: " & ex.Message)
    '    End Try
    'End Sub

    Private Sub LoadBanner(ByVal isOnline As Boolean)
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Banners")
            Dim localFilePath As String = Path.Combine(folderPath, "Banner.jpg")
            Dim bannerUrl As String = "https://softmanagementindia.in/banners/banner.jpg"
            ' Ensure folder exists
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            If isOnline Then
                Application.DoEvents()
                Dim needDownload As Boolean = True
                ' Get server file last-modified time. Use GET (not HEAD): some
                ' carriers/CDNs allow the browser image GET but reject HEAD.
                Try
                    Dim request As Net.HttpWebRequest = CType(Net.WebRequest.Create(bannerUrl), Net.HttpWebRequest)
                    request.Method = "GET"
                    request.UserAgent = "Aadhat/1.0"
                    request.Accept = "image/jpeg,image/*;q=0.8,*/*;q=0.5"
                    request.Timeout = 20000
                    request.ReadWriteTimeout = 20000
                    Using response As Net.HttpWebResponse = CType(request.GetResponse(), Net.HttpWebResponse)
                        Dim serverModified As DateTime = response.LastModified
                        If File.Exists(localFilePath) Then
                            Dim localModified As DateTime = File.GetLastWriteTime(localFilePath)

                            ' Compare times
                            If localModified >= serverModified Then
                                needDownload = False
                            End If
                        End If
                    End Using
                Catch
                    If Not AccentStorageHelper.UseLegacyHttpMode() Then
                        Throw
                    End If
                End Try

                ' Download only if server image is newer
                If needDownload Then
                    Using client As New WebClient()
                        client.Headers(HttpRequestHeader.UserAgent) = "Aadhat/1.0"
                        client.Headers(HttpRequestHeader.Accept) = "image/jpeg,image/*;q=0.8,*/*;q=0.5"
                        File.WriteAllBytes(localFilePath, client.DownloadData(bannerUrl))
                    End Using
                End If

            End If

            ' Offline mode uses the banner already stored in the local Banners folder.
            If File.Exists(localFilePath) Then
                If pb1.Image IsNot Nothing Then pb1.Image.Dispose()
                pb1.Image = Image.FromFile(localFilePath)
            End If
        Catch ex As Exception
            ' Banner is optional. Keep the application usable when a carrier/CDN
            ' blocks the image request; a cached local image remains available.
            If File.Exists(Path.Combine(Application.StartupPath, "Banners\Banner.jpg")) Then
                Try
                    If pb1.Image IsNot Nothing Then pb1.Image.Dispose()
                    pb1.Image = Image.FromFile(Path.Combine(Application.StartupPath, "Banners\Banner.jpg"))
                Catch
                End Try
            End If
        End Try
    End Sub

    Private Function CheckInternetConnection() As Boolean
        Try
            Using client As New WebClient()
                Using stream = client.OpenRead("https://www.google.com/generate_204")
                    Return True
                End Using
            End Using
        Catch
            If AccentStorageHelper.UseLegacyHttpMode() Then Return True
            Return False
        End Try
    End Function

    Private Function GetFileHash(filePath As String) As String
        Using stream As FileStream = File.OpenRead(filePath)
            Using md5 As MD5 = md5.Create()
                Dim hashBytes As Byte() = md5.ComputeHash(stream)
                Return BitConverter.ToString(hashBytes).Replace("-", "").ToLower()
            End Using
        End Using
    End Function

    Private Sub btnStartUpload_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Dim updaterPath As String = Application.StartupPath & "\AadhatSmartUpdater.exe"
            If IO.File.Exists(updaterPath) = False Then
                MsgBox("AadhatSmartUpdater.exe नहीं मिला!", MsgBoxStyle.Critical)
                Exit Sub
            End If
            ' AadhatSmartUpdater.exe को बिना किसी parameter के चलाएँ
            Process.Start(updaterPath)
            ' अब main application बंद हो जाए
            Application.Exit()
        Catch ex As Exception
            MsgBox("Updater start error: " & ex.Message)
        End Try
        'MsgBox("Online Backup Option Removed. Due to Server Load.", MsgBoxStyle.Critical, "Access Denied") : MainScreenForm.OfflineBackup() : Application.Exit()
        'lastModifled() : backupOnCloud() : lastModifled() : Application.Exit()
    End Sub



    Private Sub lastModifled()
        Dim cloudFileName As String = String.Empty
        cloudFileName = compname & "_" & City & "_" & CDate(FinYearStart).Year & "-" & CDate(FinYearEnd).Year
        Dim ftpServerIP As String = "ftp://smicloud.in" & "/Aadhat/" & cloudFileName & ".zip"
        Dim request As System.Net.FtpWebRequest = CType(System.Net.WebRequest.Create(ftpServerIP), System.Net.FtpWebRequest)
        request.Credentials = New System.Net.NetworkCredential("user", "Customer@123")
        ' Dim request = CType(WebRequest.Create(URL + ZipFile), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.GetDateTimestamp
        Try
            Dim response = CType(request.GetResponse(), FtpWebResponse)
            Dim ServerDate = DateTime.ParseExact(response.StatusDescription.Substring(4, 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None)
            Dim Modify = response.LastModified
            lbllastbackup.Text = "Last Backup On : " & Modify
        Catch ex As Exception
            lbllastbackup.Text = "Last Backup On : Never Uploaded "
        End Try
        If zipPath Is Nothing Then Exit Sub
        System.IO.File.Delete(Application.StartupPath & "\ZipBackup" & zipPath)
    End Sub
    Public Sub backupOnCloud()
        Application.DoEvents()
        Dim cloudFileName As String = String.Empty
        cloudFileName = compname & "_" & City & "_" & CDate(FinYearStart).Year & "-" & CDate(FinYearEnd).Year
        Dim ftpServerIP As String = "ftp://smicloud.in" & "/Aadhat/" & cloudFileName & ".zip"
        Dim request As System.Net.FtpWebRequest = CType(System.Net.WebRequest.Create(ftpServerIP), System.Net.FtpWebRequest)
        request.Credentials = New System.Net.NetworkCredential("user", "Customer@123")
        request.Method = System.Net.WebRequestMethods.Ftp.UploadFile
        request.Timeout = 10000000
        Dim file() As Byte = System.IO.File.ReadAllBytes(Application.StartupPath & "\ZipBackup" & zipPath)
        Try
            Dim strz As System.IO.Stream = request.GetRequestStream()
            Label2.Text = "File Size : " & Format(Val(file.Length / 1024) / 1024, "0.000") & " MB"
            ProgressBar1.Visible = True
            For offset As Integer = 0 To file.Length Step 1024
                Application.DoEvents()
                ProgressBar1.Value = CType(Val(offset / 1024) * ProgressBar1.Maximum / Val(file.Length / 1024), Int64)
                Dim chunkSize As Integer = file.Length - offset
                If chunkSize > 1024 Then chunkSize = 1024
                ProgressBar1.Value = ProgressBar1.Maximum
                strz.Write(file, offset, chunkSize)
                Label1.Text = "Uploaded : " & Format(Val(offset / 1024) / 1024, "0.000") & " MB"
            Next
            strz.Close()
            strz.Dispose()
            ProgressBar1.Visible = False
            lblMsg.Text = "Backup File Uploaded on Server Sucessfully..."
            lblMsg2.Visible = True
            lblMsg2.Text = "Backup File Uploaded on Server..."
            If zipPath Is Nothing Then Exit Sub
            System.IO.File.Delete(Application.StartupPath & "\ZipBackup" & zipPath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ApplyConnectivityMode(ClsCommon.IsInternetConnect())
        ZipBackup()
        If Not lastZipBackupSucceeded Then
            MessageBox.Show("The local backup could not be created. " & lblMsg.Text, "Local Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If isOnlineMode Then
            SyncCloudBackup(True)
        Else
            MessageBox.Show("Local backup created successfully." & Environment.NewLine & lblFilepath.Text, "Local Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Public Sub ZipBackup()
        lastZipBackupSucceeded = False
        If Directory.Exists(Application.StartupPath & "\BackupGoogle") = False Then
            Directory.CreateDirectory(Application.StartupPath & "\BackupGoogle")
        End If
        Dim sourceDirectory As String = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last() & "\Data.db"
        sourceDirectory = Application.StartupPath & "\Data\" & sourceDirectory.Replace("\Data.db", "")
        Dim zipFilePath As String = IO.Path.GetDirectoryName(GlobalData.ConnectionPath).Split(Path.DirectorySeparatorChar).Last() & ""
        zipFilePath = Application.StartupPath & "\BackupGoogle\" & zipFilePath & ".zip"
        lblMsg.Visible = True
        lblMsg.Text = "Creating Zip...Please Wait"
        Try
            Using zipFile As ZipFile = New ZipFile()
                zipFile.AddDirectory(sourceDirectory) ' Add the entire folder and its contents.
                zipFile.Save(zipFilePath)
            End Using
            lastZipBackupSucceeded = True
            lblFilepath.Text = "Zip Path : " & zipFilePath
            lblMsg.Text = "Zip Backup Created Successfully..."
        Catch ex As Exception
            lblMsg.Text = "Local backup failed: " & ex.Message
            Console.WriteLine("Error creating zip folder: " & ex.Message)
        End Try
    End Sub

    Private Sub SyncCloudBackup(ByVal forceSync As Boolean)
        Try
            Dim settings As CloudBackupSettings = CloudBackupManager.Load()
            If Not settings.GoogleEnabled AndAlso Not settings.OneDriveEnabled Then Exit Sub
            If Not forceSync AndAlso (Not settings.SyncOnClose OrElse Not CloudBackupManager.IsDue(settings)) Then Exit Sub

            ProgressBar1.Visible = True
            ProgressBar1.Value = 0
            If settings.OneDriveEnabled Then
                CloudBackupManager.SyncOneDriveFolder(Path.Combine(Application.StartupPath, "BackupGoogle"), settings, AddressOf ShowCloudProgress)
            End If
            If settings.GoogleEnabled Then
                GoogleDriveBackup.UploadFolder(Path.Combine(Application.StartupPath, "BackupGoogle"), AddressOf ShowCloudProgress)
                lblMsg.Visible = True
                lblMsg.Text = "BackupGoogle uploaded to Google Drive."
            Else
                lblMsg.Visible = True
                lblMsg.Text = "BackupGoogle synced to OneDrive."
            End If
        Catch ex As Exception
            lblMsg.Visible = True
            lblMsg.Text = "Cloud sync failed: " & ex.Message
            If forceSync Then MessageBox.Show(ex.Message, "Cloud Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            ProgressBar1.Visible = False
        End Try
    End Sub

    Private Sub ShowCloudProgress(ByVal percent As Integer, ByVal message As String)
        If percent < ProgressBar1.Minimum Then percent = ProgressBar1.Minimum
        If percent > ProgressBar1.Maximum Then percent = ProgressBar1.Maximum
        ProgressBar1.Value = percent
        lblMsg.Visible = True
        lblMsg.Text = message
        Application.DoEvents()
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim sec As Integer = CDate(Format(TimeOfDay, "hh:mm:ss")).Second
        If CLng(sec) Mod 2 > 0 Then
            lblMsg.Visible = False
            lblMsg.Text = "Creating Archive Zip.... Please Wait...."
        Else
            lblMsg.Visible = True
            lblMsg.Text = "Creating Archive Zip.... Please Wait...."
        End If
        '   MainScreenPicture.Dashboard()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnExternalBackup_Click(sender As Object, e As EventArgs) Handles btnExternalBackup.Click
        Try
            ' The picker scans immediately after it opens, so it sees drives
            ' attached or removed since Backup_On_Server was displayed.
            Dim chosenDrive As DriveInfo = SelectExternalDrive(New List(Of DriveInfo)(), True)
            If chosenDrive Is Nothing Then Return
            RunExternalBackup(True, chosenDrive)
        Catch ex As Exception
            MessageBox.Show("The external backup failed: " & ex.Message, "External Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Function GetAvailableBackupDrives() As List(Of DriveInfo)
        Dim drives As New List(Of DriveInfo)()
        For Each drive As DriveInfo In DriveInfo.GetDrives()
            Try
                If drive.IsReady AndAlso Not IsProtectedLocalDrive(drive.RootDirectory.FullName) Then
                    If drive.DriveType = DriveType.Removable OrElse IsUsbPhysicalVolume(drive.RootDirectory.FullName) Then
                        drives.Add(drive)
                    End If
                End If
            Catch
                ' A drive may be removed while Windows is enumerating it.
            End Try
        Next
        Return drives
    End Function

    Private Function IsProtectedLocalDrive(ByVal driveRoot As String) As Boolean
        Dim systemDrive As String = Environment.GetEnvironmentVariable("SystemDrive")
        Dim windowsRoot As String = ""
        If Not String.IsNullOrEmpty(systemDrive) Then windowsRoot = Path.GetPathRoot(systemDrive & "\")
        If String.Equals(windowsRoot, driveRoot, StringComparison.OrdinalIgnoreCase) Then Return True
        If String.Equals(Path.GetPathRoot(Application.StartupPath), driveRoot, StringComparison.OrdinalIgnoreCase) Then Return True
        If Not String.IsNullOrEmpty(OringinalPath) AndAlso String.Equals(Path.GetPathRoot(OringinalPath), driveRoot, StringComparison.OrdinalIgnoreCase) Then Return True
        Try
            Dim builder As New System.Data.SQLite.SQLiteConnectionStringBuilder(ClsFunPrimary.con.ConnectionString)
            If Not String.IsNullOrEmpty(builder.DataSource) AndAlso String.Equals(Path.GetPathRoot(builder.DataSource), driveRoot, StringComparison.OrdinalIgnoreCase) Then Return True
        Catch
            ' The database connection may not be initialized yet.
        End Try
        Return False
    End Function

    Private Function IsUsbPhysicalVolume(ByVal driveRoot As String) As Boolean
        Try
            Dim volumePath As String = "\\.\" & driveRoot.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            Using volumeHandle As SafeFileHandle = CreateFile(volumePath, 0UI, FileShareRead Or FileShareWrite, IntPtr.Zero, OpenExisting, 0UI, IntPtr.Zero)
                If volumeHandle Is Nothing OrElse volumeHandle.IsInvalid Then Return False

                Dim extents(1023) As Byte
                Dim bytesReturned As UInteger = 0UI
                If Not DeviceIoControl(volumeHandle, IoctlVolumeGetVolumeDiskExtents, Nothing, 0UI, extents, CUInt(extents.Length), bytesReturned, IntPtr.Zero) Then Return False
                ' VOLUME_DISK_EXTENTS has NumberOfDiskExtents at offset 0;
                ' the first DISK_EXTENT begins at offset 8, where DiskNumber lives.
                If bytesReturned < 12UI Then Return False

                Dim diskNumber As Integer = BitConverter.ToInt32(extents, 8)
                Dim physicalPath As String = "\\.\PhysicalDrive" & diskNumber.ToString(CultureInfo.InvariantCulture)
                Using diskHandle As SafeFileHandle = CreateFile(physicalPath, 0UI, FileShareRead Or FileShareWrite, IntPtr.Zero, OpenExisting, 0UI, IntPtr.Zero)
                    If diskHandle Is Nothing OrElse diskHandle.IsInvalid Then Return False

                    ' STORAGE_PROPERTY_QUERY: StorageDeviceProperty (0), PropertyStandardQuery (0).
                    Dim query(11) As Byte
                    Dim descriptor(1023) As Byte
                    bytesReturned = 0UI
                    If Not DeviceIoControl(diskHandle, IoctlStorageQueryProperty, query, CUInt(query.Length), descriptor, CUInt(descriptor.Length), bytesReturned, IntPtr.Zero) Then Return False
                    If bytesReturned < 32UI Then Return False

                    ' STORAGE_DEVICE_DESCRIPTOR.BusType is a DWORD at byte offset 28; BusTypeUsb = 7.
                    Return BitConverter.ToInt32(descriptor, 28) = 7
                End Using
            End Using
        Catch
            ' If Windows cannot confirm a USB physical disk, do not show it as an external backup target.
            Return False
        End Try
    End Function

    Private Function SelectExternalDrive(ByVal drives As List(Of DriveInfo), ByVal interactive As Boolean) As DriveInfo
        If drives Is Nothing Then drives = New List(Of DriveInfo)()

        Dim savedRoot As String = ""
        Try
            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(ExternalBackupRegistryKey)
                If key IsNot Nothing Then savedRoot = Convert.ToString(key.GetValue(ExternalBackupDriveValue, ""))
            End Using
        Catch
        End Try

        If Not interactive AndAlso Not String.IsNullOrEmpty(savedRoot) Then
            For Each drive As DriveInfo In drives
                If String.Equals(drive.RootDirectory.FullName, savedRoot, StringComparison.OrdinalIgnoreCase) Then Return drive
            Next
        End If
        If Not interactive AndAlso Not String.IsNullOrEmpty(savedRoot) Then Return Nothing
        If Not interactive Then
            If Not String.IsNullOrEmpty(savedRoot) Then Return Nothing
            If drives.Count = 1 Then Return drives(0)
            Return Nothing
        End If

        Dim prompt As New Form()
        prompt.Text = "External Backup - Select Drive"
        prompt.StartPosition = FormStartPosition.CenterScreen
        prompt.FormBorderStyle = FormBorderStyle.FixedDialog
        prompt.MinimizeBox = False
        prompt.MaximizeBox = False
        prompt.ShowInTaskbar = False
        prompt.ClientSize = New Size(430, 175)

        Dim driveList As New ComboBox()
        driveList.DropDownStyle = ComboBoxStyle.DropDownList
        driveList.Location = New Point(18, 20)
        driveList.Size = New Size(394, 28)
        ' The explicit button must rescan when the picker opens. Close-time callers
        ' pass interactive=False and never reach this dialog.
        Dim scanOnOpen As Boolean = True
        driveList.Items.Add("Searching for external drives...")
        driveList.SelectedIndex = 0
        driveList.Enabled = False

        Dim saveDefault As New CheckBox()
        saveDefault.Text = "Save as Default"
        saveDefault.AutoSize = True
        saveDefault.Location = New Point(18, 62)

        Dim googleDrivePath As String = GetGoogleDriveFolder()
        Dim googleChoice As New CheckBox()
        googleChoice.Text = "Also back up to Google Drive"
        googleChoice.AutoSize = True
        googleChoice.Location = New Point(18, 91)
        googleChoice.Enabled = Not String.IsNullOrEmpty(googleDrivePath)
        googleChoice.Checked = ReadDefaultGoogleBackup() AndAlso googleChoice.Enabled

        Dim okButton As New Button()
        okButton.Text = "Backup"
        okButton.Enabled = Not scanOnOpen
        okButton.DialogResult = DialogResult.OK
        okButton.Location = New Point(238, 128)
        Dim cancelButton As New Button()
        cancelButton.Text = "Cancel"
        cancelButton.DialogResult = DialogResult.Cancel
        cancelButton.Location = New Point(328, 128)

        prompt.Controls.Add(driveList)
        prompt.Controls.Add(saveDefault)
        prompt.Controls.Add(googleChoice)
        prompt.Controls.Add(okButton)
        prompt.Controls.Add(cancelButton)
        prompt.AcceptButton = okButton
        prompt.CancelButton = cancelButton

        externalDrivePicker = prompt
        externalDriveCombo = driveList
        externalDriveBackupButton = okButton
        If scanOnOpen Then
            externalDriveScanResults = New List(Of DriveInfo)()
            externalDrivePreferredRoot = ""
        End If
        externalDriveScanError = ""
        externalDriveScanId += 1
        If scanOnOpen Then
            externalDriveScanTimer = New System.Windows.Forms.Timer()
            externalDriveScanTimer.Interval = 2000
            AddHandler externalDriveScanTimer.Tick, AddressOf StartExternalDriveScan
            AddHandler prompt.Shown, AddressOf StartExternalDriveScan
            AddHandler prompt.Shown, AddressOf StartExternalDriveMonitoring
            AddHandler prompt.FormClosed, AddressOf StopExternalDriveMonitoring
        End If

        If prompt.ShowDialog() <> DialogResult.OK Then Return Nothing
        If externalDriveScanResults Is Nothing OrElse externalDriveScanResults.Count = 0 OrElse driveList.SelectedIndex < 0 Then Return Nothing
        Dim selectedDrive As DriveInfo = externalDriveScanResults(driveList.SelectedIndex)
        ' Confirm the selected drive is still ready and still classified as an
        ' external target immediately before accepting the choice.
        Dim currentDrives As List(Of DriveInfo) = GetAvailableBackupDrives()
        Dim stillAvailable As Boolean = False
        For Each currentDrive As DriveInfo In currentDrives
            If String.Equals(currentDrive.RootDirectory.FullName, selectedDrive.RootDirectory.FullName, StringComparison.OrdinalIgnoreCase) Then
                selectedDrive = currentDrive
                stillAvailable = True
                Exit For
            End If
        Next
        If Not stillAvailable Then Return Nothing
        If saveDefault.Checked Then SaveExternalBackupDefaults(selectedDrive.RootDirectory.FullName, googleChoice.Checked)
        externalDrivePicker = Nothing
        externalDriveCombo = Nothing
        externalDriveBackupButton = Nothing
        Return selectedDrive
    End Function

    Private Sub StartExternalDriveScan(ByVal sender As Object, ByVal e As EventArgs)
        If System.Threading.Interlocked.CompareExchange(externalDriveScanInProgress, 1, 0) <> 0 Then Return
        If externalDriveCombo IsNot Nothing AndAlso externalDriveScanResults IsNot Nothing AndAlso _
            externalDriveCombo.SelectedIndex >= 0 AndAlso externalDriveCombo.SelectedIndex < externalDriveScanResults.Count Then
            externalDrivePreferredRoot = externalDriveScanResults(externalDriveCombo.SelectedIndex).RootDirectory.FullName
        End If
        System.Threading.ThreadPool.QueueUserWorkItem(AddressOf ScanExternalDrivesInBackground, externalDriveScanId)
    End Sub

    Private Sub StartExternalDriveMonitoring(ByVal sender As Object, ByVal e As EventArgs)
        If externalDriveScanTimer IsNot Nothing Then externalDriveScanTimer.Start()
    End Sub

    Private Sub StopExternalDriveMonitoring(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        If externalDriveScanTimer IsNot Nothing Then
            externalDriveScanTimer.Stop()
            externalDriveScanTimer.Dispose()
            externalDriveScanTimer = Nothing
        End If
        externalDrivePicker = Nothing
        externalDriveCombo = Nothing
        externalDriveBackupButton = Nothing
    End Sub

    Private Sub ScanExternalDrivesInBackground(ByVal state As Object)
        Dim scanId As Integer = Convert.ToInt32(state)
        Dim results As List(Of DriveInfo)
        Dim scanError As String = ""
        Try
            results = GetAvailableBackupDrives()
        Catch ex As Exception
            scanError = ex.Message
            results = New List(Of DriveInfo)()
        End Try

        If scanId <> externalDriveScanId Then
            System.Threading.Interlocked.Exchange(externalDriveScanInProgress, 0)
            Return
        End If
        externalDriveScanResults = results
        externalDriveScanError = scanError

        Try
            If externalDrivePicker IsNot Nothing AndAlso Not externalDrivePicker.IsDisposed Then
                externalDrivePicker.BeginInvoke(New MethodInvoker(AddressOf FinishExternalDriveScan))
            End If
        Catch
            ' The user may have closed the picker before scanning finished.
            System.Threading.Interlocked.Exchange(externalDriveScanInProgress, 0)
        End Try
    End Sub

    Private Sub FinishExternalDriveScan()
        System.Threading.Interlocked.Exchange(externalDriveScanInProgress, 0)
        If externalDrivePicker Is Nothing OrElse externalDrivePicker.IsDisposed OrElse externalDriveCombo Is Nothing Then Return
        If externalDriveBackupButton IsNot Nothing Then externalDriveBackupButton.Enabled = False
        externalDriveCombo.Enabled = False
        externalDriveCombo.Items.Clear()
        If Not String.IsNullOrEmpty(externalDriveScanError) Then
            externalDriveCombo.Items.Add("Drive scan failed: " & externalDriveScanError)
            externalDriveCombo.SelectedIndex = 0
            Return
        End If

        If externalDriveScanResults Is Nothing OrElse externalDriveScanResults.Count = 0 Then
            externalDriveCombo.Items.Add("No external drives detected")
            externalDriveCombo.SelectedIndex = 0
            Return
        End If

        Dim savedRoot As String = ""
        Try
            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(ExternalBackupRegistryKey)
                If key IsNot Nothing Then savedRoot = Convert.ToString(key.GetValue(ExternalBackupDriveValue, ""))
            End Using
        Catch
        End Try

        Dim defaultIndex As Integer = 0
        Dim preferredIndex As Integer = -1
        For index As Integer = 0 To externalDriveScanResults.Count - 1
            Dim drive As DriveInfo = externalDriveScanResults(index)
            Dim label As String = drive.RootDirectory.FullName
            Try
                If Not String.IsNullOrEmpty(drive.VolumeLabel) Then label &= " (" & drive.VolumeLabel & ")"
            Catch
            End Try
            externalDriveCombo.Items.Add(label)
            If String.Equals(drive.RootDirectory.FullName, externalDrivePreferredRoot, StringComparison.OrdinalIgnoreCase) Then preferredIndex = index
            If String.Equals(drive.RootDirectory.FullName, savedRoot, StringComparison.OrdinalIgnoreCase) Then defaultIndex = index
        Next
        If preferredIndex >= 0 Then defaultIndex = preferredIndex
        externalDriveCombo.SelectedIndex = defaultIndex
        externalDriveCombo.Enabled = True
        If externalDriveBackupButton IsNot Nothing Then externalDriveBackupButton.Enabled = True
    End Sub

    Private Sub SaveExternalBackupDefaults(ByVal driveRoot As String, ByVal googleBackup As Boolean)
        Using key As RegistryKey = Registry.CurrentUser.CreateSubKey(ExternalBackupRegistryKey)
            If key IsNot Nothing Then
                key.SetValue(ExternalBackupDriveValue, driveRoot, RegistryValueKind.String)
                key.SetValue(ExternalBackupGoogleValue, If(googleBackup, 1, 0), RegistryValueKind.DWord)
            End If
        End Using
    End Sub

    Private Function GetGoogleDriveFolder() As String
        Dim userProfilePath As String = Environment.GetEnvironmentVariable("USERPROFILE")
        If String.IsNullOrEmpty(userProfilePath) Then userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
        Dim candidates() As String = {Environment.GetEnvironmentVariable("GoogleDrive"), Environment.GetEnvironmentVariable("Google Drive"), Environment.GetEnvironmentVariable("GOOGLE_DRIVE"), _
            Path.Combine(userProfilePath, "Google Drive"), _
            Path.Combine(userProfilePath, "My Drive")}
        For Each candidate As String In candidates
            If Not String.IsNullOrEmpty(candidate) AndAlso Directory.Exists(candidate) Then Return candidate
        Next
        Return ""
    End Function

    Private Function ReadDefaultGoogleBackup() As Boolean
        Try
            Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(ExternalBackupRegistryKey)
                If key IsNot Nothing Then Return Val(Convert.ToString(key.GetValue(ExternalBackupGoogleValue, 0))) <> 0
            End Using
        Catch
        End Try
        Return False
    End Function

    Private Sub RunExternalBackup(ByVal interactive As Boolean, Optional ByVal chosenDrive As DriveInfo = Nothing)
        Dim drives As List(Of DriveInfo) = GetAvailableBackupDrives()
        If drives Is Nothing Then drives = New List(Of DriveInfo)()

        If Not interactive AndAlso drives.Count > 1 Then
            Dim savedRoot As String = ""
            Try
                Using key As RegistryKey = Registry.CurrentUser.OpenSubKey(ExternalBackupRegistryKey)
                    If key IsNot Nothing Then savedRoot = Convert.ToString(key.GetValue(ExternalBackupDriveValue, ""))
                End Using
            Catch
            End Try
            If String.IsNullOrEmpty(savedRoot) Then Return
        End If

        Dim selectedDrive As DriveInfo = chosenDrive
        If selectedDrive Is Nothing Then selectedDrive = SelectExternalDrive(drives, interactive)
        If selectedDrive Is Nothing Then Return

        Dim googleFolder As String = GetGoogleDriveFolder()
        Dim backupToGoogle As Boolean = False
        If Not String.IsNullOrEmpty(googleFolder) Then
            If interactive Then
                backupToGoogle = MessageBox.Show("Would you also like to back up to Google Drive?", "External Backup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes
            Else
                backupToGoogle = ReadDefaultGoogleBackup()
            End If
        End If

        Dim sourceFolder As String = Path.Combine(Application.StartupPath, "BackupGoogle")
        If Not Directory.Exists(sourceFolder) OrElse Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories).Length = 0 Then
            If interactive Then MessageBox.Show("There are no backup files available to copy from the BackupGoogle folder.", "External Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim dateFolder As String = Date.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        Dim destination As String = Path.Combine(Path.Combine(selectedDrive.RootDirectory.FullName, "AadhatBackup"), dateFolder)
        If String.Equals(Path.GetPathRoot(destination), Path.GetPathRoot(Application.StartupPath), StringComparison.OrdinalIgnoreCase) Then
            If interactive Then MessageBox.Show("The selected destination is on the Windows drive. External backup was cancelled.", "External Backup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        CopyBackupFiles(sourceFolder, destination, "External drive")

        If backupToGoogle AndAlso Not String.IsNullOrEmpty(googleFolder) Then
            Dim googleDestination As String = Path.Combine(Path.Combine(googleFolder, "AadhatBackup"), dateFolder)
            CopyBackupFiles(sourceFolder, googleDestination, "Google Drive")
        End If

        ProgressBar1.Visible = False

        If interactive Then
            Dim message As String = "Backup completed successfully: " & destination
            If backupToGoogle Then message &= Environment.NewLine & "Google Drive: " & Path.Combine(Path.Combine(googleFolder, "AadhatBackup"), dateFolder)
            MessageBox.Show(message, "External Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub CopyBackupFiles(ByVal sourceFolder As String, ByVal destinationFolder As String, ByVal destinationName As String)
        Dim files As String() = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories)
        Dim totalBytes As Long = 0
        For Each sourceFile As String In files
            totalBytes += New FileInfo(sourceFile).Length
        Next

        ProgressBar1.Visible = True
        ProgressBar1.BringToFront()
        ProgressBar1.Location = New Point(730, 420)
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 1000
        ProgressBar1.Value = 0
        lblMsg.Visible = True
        lblMsg.BringToFront()
        lblMsg.Location = New Point(730, 392)
        lblMsg.Text = "Copying backup to " & destinationName & "..."
        Application.DoEvents()

        Directory.CreateDirectory(destinationFolder)
        Dim copiedBytes As Long = 0
        Dim lastUiUpdate As Integer = Environment.TickCount
        Try
            For Each sourceFile As String In files
                Dim relativePath As String = sourceFile.Substring(sourceFolder.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                Dim destinationFile As String = Path.Combine(destinationFolder, relativePath)
                Dim parentFolder As String = Path.GetDirectoryName(destinationFile)
                If Not Directory.Exists(parentFolder) Then Directory.CreateDirectory(parentFolder)
                Dim sourceFileBytes As Long = New FileInfo(sourceFile).Length
                Dim progressCallback As CopyProgressRoutine = _
                    Function(totalFileSize As Long, totalBytesTransferred As Long, streamSize As Long, streamBytesTransferred As Long, streamNumber As UInteger, callbackReason As UInteger, sourceHandle As IntPtr, destinationHandle As IntPtr, callbackData As IntPtr) As UInteger
                        Try
                            Dim now As Integer = Environment.TickCount
                            If now - lastUiUpdate >= 250 Then
                                UpdateCopyProgress(copiedBytes + totalBytesTransferred, totalBytes, destinationName)
                                lastUiUpdate = now
                            End If
                        Catch
                        End Try
                        Return 0UI ' PROGRESS_CONTINUE
                    End Function
                Dim cancelCopy As Boolean = False
                If Not CopyFileExNative(sourceFile, destinationFile, progressCallback, IntPtr.Zero, cancelCopy, 0UI) Then
                    Throw New IOException("Could not copy file: " & sourceFile, New System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error()))
                End If
                copiedBytes += sourceFileBytes
                UpdateCopyProgress(copiedBytes, totalBytes, destinationName)
            Next
        Catch
            ProgressBar1.Visible = False
            Throw
        End Try
    End Sub

    Private Sub UpdateCopyProgress(ByVal copiedBytes As Long, ByVal totalBytes As Long, ByVal destinationName As String)
        Dim percent As Integer = 100
        If totalBytes > 0 Then percent = CInt(Math.Min(100, (copiedBytes * 100) \ totalBytes))
        ProgressBar1.Value = Math.Min(ProgressBar1.Maximum, percent * 10)
        lblMsg.Text = "Copying to " & destinationName & "... " & percent.ToString() & "% (" & _
            Format(Val(copiedBytes / 1024) / 1024, "0.0") & " / " & Format(Val(totalBytes / 1024) / 1024, "0.0") & " MB)"
        ProgressBar1.Refresh()
        lblMsg.Refresh()
    End Sub

End Class
