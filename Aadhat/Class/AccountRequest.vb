Public Class AddAccountRequest
    Dim serverType As Object
    Public Sub New()
        Accounts = New List(Of AccountRequest)
        Type = ServerType
    End Sub
    Public Property Accounts As List(Of AccountRequest)
    Public Property Type As Integer
End Class

Public Class AccountRequest
    Public Property AccountId As Integer
    Public Property AccountName As String
    Public Property GroupID As Integer
    Public Property DC As String
    Public Property Tag As Integer
    Public Property OpBal As Decimal
    Public Property OtherName As String
    Public Property Address As String
    Public Property LFNo As String
    Public Property Area As String
    Public Property City As String
    Public Property State As String
    Public Property Phone As String
    Public Property Contact As String
    Public Property Mobile1 As String
    Public Property Mobile2 As String
    Public Property MailID As String
    Public Property BankName As String
    Public Property AccNo As String
    Public Property IFSC As String
    Public Property GName As String
    Public Property Gmobile1 As String
    Public Property Gmobile2 As String
    Public Property Gaddress As String
    Public Property GCity As String
    Public Property Gstate As String
    Public Property Limit As String
    Public Property AccountPhoto As String
    Public Property Gphoto As String
    Public Property OrganizationId As Integer
    Public Property Type As Integer
    Public Property ServerTag As Integer
End Class

Public Class Response
    Public Sub New()
        Message = ""
    End Sub

    Public Property IsValid As Boolean
    Public Property Status As Boolean
    Public Property Message As String
End Class

Public Class SaveAccountResponse
    Inherits Response
End Class


