Imports System.IO
Public Class Whats_New

    Private Sub Whats_New_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        If File.Exists(Application.StartupPath & "\WhatsNew.txt") = True Then
            Dim reader As TextReader = New StreamReader(Application.StartupPath & "\WhatsNew.txt")
            RichTextBox1.Text = reader.ReadToEnd()
            reader.Close()
        Else
            Exit Sub
        End If
    End Sub
End Class