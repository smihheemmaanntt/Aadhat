Public Class CopyBackup
    'Private Sub DriveInsertedEventHandler(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Get the drive letter of the inserted removable drive
    '    Dim driveLetter As String = e.Drive.ToString()

    '    ' Ask the user for the folder they want to copy as a ZIP file
    '    Dim folderPath As String = ""
    '    Using folderDialog As New FolderBrowserDialog()
    '        folderDialog.Description = "Select the folder you want to copy as a ZIP file"
    '        Dim result As DialogResult = folderDialog.ShowDialog()
    '        If result = DialogResult.OK Then
    '            folderPath = folderDialog.SelectedPath
    '        End If
    '    End Using

    '    ' Create a ZIP file with the current date in the filename
    '    Dim currentDate As String = DateTime.Now.ToString("yyyyMMdd")
    '    Dim zipFilePath As String = driveLetter & "\Data_" & currentDate & ".zip"
    '    ZipFile.CreateFromDirectory(folderPath, zipFilePath)
    'End Sub

    'Private Sub CopyBackup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    AddHandler Microsoft.Win32.SystemEvents.DeviceInserted, AddressOf DriveInsertedEventHandler
    'End Sub
End Class