Imports System.Security.Cryptography
Imports System.Text

Public Module SecureCredentialStore
    Private Const Prefix As String = "enc:v1:"
    Private ReadOnly Entropy As Byte() = Encoding.UTF8.GetBytes("Aadhat.WhatsAppOfficialApi.v1")

    Public Function IsProtected(ByVal value As String) As Boolean
        If value Is Nothing Then Return False
        Return value.Trim().StartsWith(Prefix, StringComparison.Ordinal)
    End Function

    Public Function Protect(ByVal value As String) As String
        If value Is Nothing Then Return ""
        value = value.Trim()
        If value = "" Then Return ""
        If IsProtected(value) Then Return value
        Dim plainBytes As Byte() = Encoding.UTF8.GetBytes(value)
        Dim protectedBytes As Byte() = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser)
        Return Prefix & Convert.ToBase64String(protectedBytes)
    End Function

    Public Function Unprotect(ByVal value As String) As String
        If value Is Nothing Then Return ""
        value = value.Trim()
        If value = "" Then Return ""
        If IsProtected(value) = False Then Return value
        Dim encryptedText As String = value.Substring(Prefix.Length)
        Dim protectedBytes As Byte() = Convert.FromBase64String(encryptedText)
        Dim plainBytes As Byte() = ProtectedData.Unprotect(protectedBytes, Entropy, DataProtectionScope.CurrentUser)
        Return Encoding.UTF8.GetString(plainBytes)
    End Function

    Public Function TryUnprotect(ByVal value As String, ByRef plainText As String, ByRef errorMessage As String) As Boolean
        Try
            plainText = Unprotect(value)
            errorMessage = ""
            Return True
        Catch ex As Exception
            plainText = ""
            errorMessage = "Saved Official API credentials cannot be read on this Windows user/machine. Please enter and save again."
            Return False
        End Try
    End Function

    Public Function Mask(ByVal value As String) As String
        value = If(value, "").Trim()
        If value = "" Then Return ""
        If value.Length <= 8 Then Return "****"
        Return value.Substring(0, 4) & "..." & value.Substring(value.Length - 3)
    End Function
End Module
