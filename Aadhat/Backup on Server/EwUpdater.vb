Imports System.Net
Imports System.IO

Public Class frmUpdater

    Private WithEvents wc As New WebClient()

    ' 👉 Proxy URL use karo
    Private DownloadUrl As String = "http://softmanagementindia.in/updates/EWSmartUpdater.exe"
    Private SaveFile As String
    Private Sub frmUpdater_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Downloading Update..."
        prgDownload.Value = 0
        lblPercent.Text = "0%"
        lblstatus.Text = "Starting download..."
        Dim basePath As String = Application.StartupPath
        Dim whatsappFolder As String = Path.Combine(basePath, "Whatsapp")
        If Not Directory.Exists(whatsappFolder) Then
            Directory.CreateDirectory(whatsappFolder)
        End If
        SaveFile = Path.Combine(whatsappFolder, "EWSmartUpdater.exe")
        Try
            wc.DownloadFileAsync(New Uri(DownloadUrl), SaveFile)
        Catch ex As Exception
            MsgBox("Error starting download: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try

    End Sub

    Private Sub wc_DownloadProgressChanged(
        sender As Object,
        e As DownloadProgressChangedEventArgs
    ) Handles wc.DownloadProgressChanged

        prgDownload.Value = e.ProgressPercentage
        lblPercent.Text = e.ProgressPercentage & "%"
        lblstatus.Text =
            "Downloading... " &
            (e.BytesReceived \ 1024) & " KB / " &
            (e.TotalBytesToReceive \ 1024) & " KB"

    End Sub

    Private Sub wc_DownloadFileCompleted(
        sender As Object,
        e As System.ComponentModel.AsyncCompletedEventArgs
    ) Handles wc.DownloadFileCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Download failed: " & e.Error.Message, MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        lblstatus.Text = "Starting Updater..."

        Try
            ' 👉 No re-download (IMPORTANT FIX)
            ClsFunPrimary.ExecScalarStr("Update API SET SendingMethod='Easy WhatsApp'")
            Process.Start(SaveFile)
            Me.Close()

        Catch ex As Exception
            MsgBox("Error running update: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try

    End Sub

End Class