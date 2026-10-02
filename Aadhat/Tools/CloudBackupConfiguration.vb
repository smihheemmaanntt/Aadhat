Imports System.Windows.Forms
Imports System.Diagnostics

Public Class CloudBackupConfiguration
    Private settings As CloudBackupSettings

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub CloudBackupConfiguration_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        settings = CloudBackupManager.Load()
        chkGoogle.Checked = settings.GoogleEnabled
        chkOneDrive.Checked = settings.OneDriveEnabled
        txtGoogleEmail.Text = settings.GoogleEmail
        txtOneDrivePath.Text = settings.OneDrivePath
        If String.IsNullOrEmpty(txtOneDrivePath.Text) Then txtOneDrivePath.Text = CloudBackupManager.DetectOneDriveFolder()
        cbSchedule.Text = settings.Schedule
        chkSyncOnClose.Checked = settings.SyncOnClose
        lblLastGoogle.Text = "Google Drive: " & If(String.IsNullOrEmpty(settings.LastGoogleBackup), "Never", settings.LastGoogleBackup)
        lblLastOneDrive.Text = "OneDrive: " & If(String.IsNullOrEmpty(settings.LastOneDriveBackup), "Never", settings.LastOneDriveBackup)
        UpdateGoogleStatus()
    End Sub

    Private Sub CloudBackupConfiguration_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            e.Handled = True
            e.SuppressKeyPress = True
            Me.Close()
        End If
    End Sub

    Private Sub btnDetectOneDrive_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDetectOneDrive.Click
        Dim folder As String = CloudBackupManager.DetectOneDriveFolder()
        If String.IsNullOrEmpty(folder) Then
            MessageBox.Show("OneDrive is not signed in. Please sign in to OneDrive first; the application will not mark a local copy as a cloud backup.", "OneDrive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            txtOneDrivePath.Text = folder
            lblStatus.Text = "OneDrive folder detected."
        End If
    End Sub

    Private Sub btnBrowseOneDrive_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowseOneDrive.Click
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then txtOneDrivePath.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub btnGoogleConnect_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGoogleConnect.Click
        Process.Start("https://console.cloud.google.com/auth/clients")
        lblGoogleStatus.Text = "Create a Desktop app OAuth client, download JSON, then Import JSON."
    End Sub

    Private Sub btnEnableDriveApi_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnableDriveApi.Click
        Process.Start("https://console.cloud.google.com/apis/library/drive.googleapis.com")
        lblGoogleStatus.Text = "Google Drive API page opened. Select your project and click Enable."
    End Sub

    Private Sub btnImportGoogleJson_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnImportGoogleJson.Click
        Using picker As New OpenFileDialog()
            picker.Filter = "Google OAuth JSON|*.json"
            picker.Title = "Select downloaded Google OAuth JSON"
            If picker.ShowDialog() = DialogResult.OK Then
                Try
                    GoogleDriveBackup.ImportClientCredentials(picker.FileName)
                    lblGoogleStatus.Text = "OAuth app imported. Connect Google account next."
                    lblStatus.Text = "OAuth client saved securely for this Windows user."
                Catch ex As Exception
                    lblGoogleStatus.ForeColor = Drawing.Color.Firebrick
                    MessageBox.Show(ex.Message, "Google Drive setup", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End Try
            End If
        End Using
    End Sub

    Private Sub btnGoogleConnectAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGoogleConnectAccount.Click
        Try
            GoogleDriveBackup.ConnectAccount(txtGoogleEmail.Text.Trim())
            lblGoogleStatus.Text = "Google account connected. Ready to upload backups."
            lblGoogleStatus.ForeColor = Drawing.Color.DarkGreen
            lblStatus.Text = "Google Drive authorization completed."
        Catch ex As Exception
            lblGoogleStatus.Text = "Google connection failed."
            lblGoogleStatus.ForeColor = Drawing.Color.Firebrick
            MessageBox.Show(ex.Message, "Google Drive connection", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub btnOneDriveLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOneDriveLogin.Click
        Process.Start("https://login.live.com/")
        lblStatus.Text = "OneDrive sign-in page opened. Sign in to the OneDrive desktop app, then use Detect OneDrive."
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnUserManual_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUserManual.Click
        MessageBox.Show("Google Drive: complete these steps once" & vbCrLf & vbCrLf & _
                        "1. Click Open OAuth Setup and select or create a project in Google Cloud." & vbCrLf & _
                        "2. Click Enable Drive API and enable it in the same project." & vbCrLf & _
                        "3. Create a Desktop OAuth client and download its JSON file." & vbCrLf & _
                        "4. Enter the backup Gmail address, then click Import OAuth JSON and select the downloaded file." & vbCrLf & _
                        "5. Click Connect Google account. In the browser, choose that Gmail account and allow access." & vbCrLf & _
                        "6. Select Enable Google Drive backup and click Save Configuration." & vbCrLf & _
                        "7. Use Backup in Backup On Server for an immediate upload. Scheduled uploads sync when due." & vbCrLf & _
                        "OAuth credentials are encrypted for this Windows user. You do not need to import the JSON again for each backup." & vbCrLf & vbCrLf & _
                        "OneDrive: Sign in to the OneDrive desktop app, click Detect OneDrive, and confirm the sync folder path. Select Enable OneDrive backup and save. Use Browse to choose the folder manually." & vbCrLf & vbCrLf & _
                        "Google Drive folder: My Drive > AadhatBackup > Daily Backup > YYYYMMDD", "Cloud Backup Help", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If chkOneDrive.Checked AndAlso String.IsNullOrEmpty(txtOneDrivePath.Text.Trim()) Then
            MessageBox.Show("Select or detect a OneDrive backup folder first.", "OneDrive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If chkGoogle.Checked AndAlso String.IsNullOrEmpty(txtGoogleEmail.Text.Trim()) Then
            MessageBox.Show("Enter the backup Gmail address first.", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If chkGoogle.Checked AndAlso Not GoogleDriveBackup.IsConnected() Then
            MessageBox.Show("Import the OAuth JSON and connect the Google account before enabling Google Drive backup.", "Google Drive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        settings.GoogleEnabled = chkGoogle.Checked
        settings.OneDriveEnabled = chkOneDrive.Checked
        settings.GoogleEmail = txtGoogleEmail.Text.Trim()
        settings.OneDrivePath = txtOneDrivePath.Text.Trim()
        settings.Schedule = cbSchedule.Text
        settings.SyncOnClose = chkSyncOnClose.Checked
        CloudBackupManager.Save(settings)
        lblStatus.Text = "Cloud backup configuration saved."
        UpdateGoogleStatus()
    End Sub

    Private Sub UpdateGoogleStatus()
        If chkGoogle.Checked Then
            lblGoogleStatus.Text = If(GoogleDriveBackup.IsConnected(), "Google account connected.", "Gmail saved. OAuth connection pending.")
            lblGoogleStatus.ForeColor = If(GoogleDriveBackup.IsConnected(), Drawing.Color.DarkGreen, Drawing.Color.DarkOrange)
        Else
            lblGoogleStatus.Text = "Google Drive backup is disabled."
            lblGoogleStatus.ForeColor = Drawing.Color.DimGray
        End If
    End Sub

    Private Sub lblGoogleStatus_Click(sender As Object, e As EventArgs) Handles lblGoogleStatus.Click

    End Sub
End Class
