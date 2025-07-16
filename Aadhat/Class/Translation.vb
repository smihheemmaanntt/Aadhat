Imports System.Runtime.InteropServices

Public Class Translation
    ' Windows API functions
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function LoadKeyboardLayout(pwszKLID As String, Flags As UInteger) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function ActivateKeyboardLayout(hkl As IntPtr, Flags As UInteger) As IntPtr
    End Function

    ' Constants
    Private Const KLF_ACTIVATE As UInteger = &H1

    ' Keyboard layout identifiers
    Public Const KLID_ENGLISH As String = "00000409"
    Public Const KLID_HINDI As String = "00000439"
    Public Const KLID_GUJARATI As String = "00000447"
    Public Const KLID_PUNJABI As String = "00000446"
    Public Const KLID_MARATHI As String = "0000044E"
    Public Const KLID_TAMIL As String = "00000449"
    Public Const KLID_TELUGU As String = "0000044A"
    Public Const KLID_BENGALI As String = "00000445"

    ' Function to change the keyboard layout
    Public Sub ChangeKeyboardLayout(layoutId As String)
        Dim hkl As IntPtr = LoadKeyboardLayout(layoutId, KLF_ACTIVATE)
        If hkl = IntPtr.Zero Then
            MessageBox.Show("Failed to load keyboard layout.")
        Else
            ActivateKeyboardLayout(hkl, KLF_ACTIVATE)
        End If
    End Sub
End Class
