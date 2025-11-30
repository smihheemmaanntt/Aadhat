Imports System.Net
Imports System.IO

Public Class frmUpdater

    Private WithEvents wc As New WebClient()
    Private DownloadUrl As String = "http://softmanagementindia.in/updates/Aadhat.exe"
    Private SaveFile As String   ' patch folder file

    Private Sub frmUpdater_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = "Downloading..."
        prgDownload.Value = 0
        lblPercent.Text = "0%"
        lblStatus.Text = "Starting download..."

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
        lblStatus.Text = "Downloading... " &
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

        ' ---------------------------
        '   UPDATE APPLY LOGIC
        ' ---------------------------

        Try
            Dim basePath As String = Application.StartupPath

            ' OLD Aadhat.exe location (main software folder)
            Dim oldExe As String = Path.Combine(basePath, "Aadhat.exe")

            ' BACKUP file name
            Dim backupExe As String = Path.Combine(basePath, "Aadhat.bak.exe")

            ' PATCH folder new EXE (already downloaded)
            Dim newExe As String = SaveFile   ' Patch\Aadhat.exe

            ' 1. अगर पुराना Aadhat.exe मिलता है तो उसे backup कर दो
            If File.Exists(oldExe) Then
                If File.Exists(backupExe) Then File.Delete(backupExe)
                File.Move(oldExe, backupExe)
            End If

            ' 2. Now new EXE replace to main folder
            File.Copy(newExe, oldExe, True)

            lblStatus.Text = "Update applied. Starting Aadhat..."

            ' 3. RUN the new updated exe
            Process.Start(oldExe)
            'MsgBox("Update successfully applied!", MsgBoxStyle.Information)
            Me.Close()

        Catch ex As Exception
            MsgBox("Error applying update: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try

    End Sub

End Class
