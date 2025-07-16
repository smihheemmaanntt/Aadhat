Public Class CrateVoucherRequest
    Dim serverType As Object
    Public Sub New()
        CrateVouchers = New List(Of SaveCrateVoucherRequest)()
        Type = ServerType
    End Sub

    Public Property CrateVouchers As List(Of SaveCrateVoucherRequest)
    Public Property OrgId As Integer
    Public Property IsFirstRow As Boolean
    Public Property Type As Integer
End Class
Public Class SaveCrateVoucherRequest
    Public Property SlipNo As String = ""
    Public Property EntryDate As Date
    Public Property TransType As String = ""
    Public Property AccountID As Integer = 0
    Public Property VoucherID As Integer = 0
    Public Property AccountName As String = ""
    Public Property CrateType As String = ""
    Public Property CrateID As Integer = 0
    Public Property CrateName As String = ""
    Public Property Qty As Integer = 0
    Public Property Remark As String = ""
    Public Property Rate As String = ""
    Public Property Amount As String = ""
    Public Property CashPaid As String = ""
    Public Property OrganizationId As Integer
    Public Property Type As Integer
    Public Property ServerTag As Integer
End Class
Public Class SaveCrateVoucherResponse
    Inherits Response
End Class

