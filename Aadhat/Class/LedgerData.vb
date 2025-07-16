Public Class LedgerRequest
    Dim serverType As Object
    Public Sub New()
        Ledgers = New List(Of LedgerData)()
        Type = ServerType
    End Sub

    Public Property Ledgers As List(Of LedgerData)
    Public Property IsFirstRow As Boolean
    Public Property Type As Integer
End Class

Public Class LedgerData
    Public Property VourchersID As Integer = 0
    Public Property EntryDate As Date
    Public Property TransType As String = ""
    Public Property AccountID As Integer = 0
    Public Property AccountName As String = ""
    Public Property Amount As Decimal
    Public Property DC As String = ""
    Public Property Remark As String = ""
    Public Property Remark2 As String = ""
    Public Property Narration As String = ""
    Public Property OrganizationId As Integer
    Public Property Type As Integer
    Public Property ServerTag As Integer
End Class

Public Class SaveLedgerResponse
    Inherits Response
End Class


