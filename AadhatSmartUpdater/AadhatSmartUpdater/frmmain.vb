Imports System.Net
Imports System.IO
Imports Microsoft.Win32
Public Class frmUpdater

    Private WithEvents wc As New WebClient()
    Private DownloadUrl As String = "https://softmanagementindia.in/updates/Aadhat.exe"
    'Private DownloadUrl As String = "http://softmanagementindia.in/updates/aadhatsmartupdater.php"
    Private SaveFile As String   ' patch folder file
    Private Sub frmUpdater_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType)
        Me.Text = "Downloading..."
        prgDownload.Value = 0
        lblPercent.Text = "0%"
        lblstatus.Text = "Starting download..."

        ' Base folder = updater EXE folder
        Dim basePath As String = Application.StartupPath

        ' Patch folder
        Dim patchFolder As String = Path.Combine(basePath, "Patch")
        If Not Directory.Exists(patchFolder) Then
            Directory.CreateDirectory(patchFolder)
        End If

        ' Downloaded file path
        SaveFile = Path.Combine(patchFolder, "Aadhat.exe")

        Try
            wc.DownloadFileAsync(New Uri(DownloadUrl), SaveFile)

        Catch ex As Exception
            MsgBox("Error starting download: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try

    End Sub

    Private Sub wc_DownloadProgressChanged(sender As Object,
                                           e As DownloadProgressChangedEventArgs) _
                                           Handles wc.DownloadProgressChanged

        prgDownload.Value = e.ProgressPercentage
        lblPercent.Text = e.ProgressPercentage.ToString() & "%"
        lblstatus.Text = "Downloading... " &
                         (e.BytesReceived \ 1024) & " KB / " &
                         (e.TotalBytesToReceive \ 1024) & " KB"

    End Sub


    Private Sub wc_DownloadFileCompleted(sender As Object,
                                     e As System.ComponentModel.AsyncCompletedEventArgs) _
                                     Handles wc.DownloadFileCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Download failed: " & e.Error.Message, MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        lblStatus.Text = "Applying Update..."

        Try
            Dim basePath As String = Application.StartupPath

            Dim oldExe As String = Path.Combine(basePath, "Aadhat.exe")
            Dim backupExe As String = Path.Combine(basePath, "Aadhat.bak.exe")
            Dim newExe As String = SaveFile

            If File.Exists(oldExe) Then
                If File.Exists(backupExe) Then File.Delete(backupExe)
                File.Move(oldExe, backupExe)
            End If

            File.Copy(newExe, oldExe, True)

            lblStatus.Text = "Update completed."

            ' ============================
            '   FINAL USER CONFIRMATION
            ' ============================
            'Dim result As DialogResult = MsgBox(
            '    "The application has been downloaded and updated successfully." & vbCrLf &
            '    "Do you want to start the application now?",
            '    MsgBoxStyle.Question Or MsgBoxStyle.YesNo,
            '    "Update Completed"
            ')

            'If result = DialogResult.Yes Then
            '    Process.Start(oldExe)
            'End If
            Process.Start(oldExe)
            Me.Close()
        Catch ex As Exception
            MsgBox("Error applying update: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try
    End Sub
End Class
