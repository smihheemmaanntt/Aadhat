Imports System.Net
Imports System.IO

Public Class frmUpdater

    Private WithEvents wc As New WebClient()

    Private DownloadUrl As String = "http://softmanagementindia.in/updates/EWSmartUpdater.exe"
    Private SaveFile As String

    Private Sub frmUpdater_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Net.ServicePointManager.SecurityProtocol = CType(3072, System.Net.SecurityProtocolType)
        Me.Text = "Downloading Update..."
        prgDownload.Value = 0
        lblPercent.Text = "0%"
        lblstatus.Text = "Starting download..."

        ' ===== Base Path =====
        Dim basePath As String = Application.StartupPath

        ' ===== Whatsapp Folder =====
        Dim whatsappFolder As String = Path.Combine(basePath, "Whatsapp")
        If Not Directory.Exists(whatsappFolder) Then
            Directory.CreateDirectory(whatsappFolder)
        End If

        ' ===== Downloaded EXE Path =====
        SaveFile = Path.Combine(whatsappFolder, "EWSmartUpdater.exe")

        Try
            wc.DownloadFileAsync(New Uri(DownloadUrl), SaveFile)
        Catch ex As Exception
            MsgBox("Error starting download: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try

    End Sub

    ' ================= PROGRESS =================
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

    ' ================= COMPLETED =================
    Private Sub wc_DownloadFileCompleted(
        sender As Object,
        e As System.ComponentModel.AsyncCompletedEventArgs
    ) Handles wc.DownloadFileCompleted

        If e.Error IsNot Nothing Then
            MsgBox("Download failed: " & e.Error.Message, MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        lblstatus.Text = "Preparing to run update..."

        Try
            ' ===== Backup if file already exists =====
            Dim bakFile As String = SaveFile & ".bak"

            If File.Exists(SaveFile) Then
                If File.Exists(bakFile) Then File.Delete(bakFile)
                File.Move(SaveFile, bakFile)
            End If

            ' ===== Download again fresh (overwrite logic safe) =====
            wc.DownloadFile(New Uri(DownloadUrl), SaveFile)

            lblstatus.Text = "Starting Updater..."
            ' ===== RUN DOWNLOADED EXE =====
            ClsFunPrimary.ExecScalarStr("Update API SET SendingMethod='Easy WhatsApp'")
            Process.Start(SaveFile)
            Me.Close()

        Catch ex As Exception
            MsgBox("Error running update: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try

    End Sub

End Class
