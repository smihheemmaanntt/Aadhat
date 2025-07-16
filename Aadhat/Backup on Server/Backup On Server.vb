Imports System.Globalization
Imports System.IO
Imports System.Net
Imports Ionic.Zip
Imports System.Security.Cryptography

Public Class Backup_On_Server
    Dim zipPath As String
    Dim OringinalPath As String
    Dim ClsCommon As CommonClass = New CommonClass()
    Private Sub Backup_On_Server_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        '   ZipBackup()
    End Sub

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
        btnClose.Image = Nothing
        btnClose.Text = "Wait."
        ZipBackup() : Application.Exit()
    End Sub

    Private Sub Backup_On_Server_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Backup_On_Server_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        OringinalPath = GlobalData.ConnectionPath
        lblOriginalPath.Text = "Data Path : " & OringinalPath
        'If ClsCommon.IsInternetConnect() Then pb1.Image = New System.Drawing.Bitmap(New IO.MemoryStream(New System.Net.WebClient().DownloadData("http://softmanagementindia.in/images/AadhatKit.png")))
        LoadBanner()
    End Sub


    Private Sub LoadBanner()
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Banners")
            Dim localFilePath As String = Path.Combine(folderPath, "Banner.jpg")
            Dim bannerUrl As String = "http://softmanagementindia.in/banners/banner.jpg"

            ' Ensure folder exists
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            If CheckInternetConnection() Then
                Dim needDownload As Boolean = True

                ' Get server file last-modified time
                Dim request As Net.HttpWebRequest = CType(Net.WebRequest.Create(bannerUrl), Net.HttpWebRequest)
                request.Method = "HEAD"
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

                ' Download only if server image is newer
                If needDownload Then
                    Dim client As New WebClient()
                    client.DownloadFile(bannerUrl, localFilePath)
                End If

                ' Load image
                If File.Exists(localFilePath) Then
                    If pb1.Image IsNot Nothing Then pb1.Image.Dispose()
                    pb1.Image = Image.FromFile(localFilePath)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Banner load error: " & ex.Message)
        End Try
    End Sub

    Private Function CheckInternetConnection() As Boolean
        Try
            Using client As New WebClient()
                Using stream = client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using
        Catch
            Return False
        End Try
    End Function

    Private Function GetFileHash(filePath As String) As String
        Using stream As FileStream = File.OpenRead(filePath)
            Using md5 As MD5 = MD5.Create()
                Dim hashBytes As Byte() = md5.ComputeHash(stream)
                Return BitConverter.ToString(hashBytes).Replace("-", "").ToLower()
            End Using
        End Using
    End Function

    Private Sub btnStartUpload_Click(sender As Object, e As EventArgs) Handles btnStartUpload.Click
        MsgBox("Online Backup Option Removed. Due to Server Load.", MsgBoxStyle.Critical, "Access Denied") : MainScreenForm.OfflineBackup() : Application.Exit()
        lastModifled() : backupOnCloud() : lastModifled() : Application.Exit()
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
        ZipBackup() : MsgBox("Offline Zip Backup Created Successfully ....", MsgBoxStyle.Information, "Offline Backup Successful")
        Application.Exit()
    End Sub

    Public Sub ZipBackup()
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
            lblMsg.Text = "Zip Backup Created Successfully..."
        Catch ex As Exception
            Console.WriteLine("Error creating zip folder: " & ex.Message)
        End Try
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
        ZipBackup() : Application.Exit()
    End Sub
End Class