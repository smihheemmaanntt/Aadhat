Public Class SaveCrateMarkaRequest
    Dim serverType As Object
    Public Sub New()
        CrateMarkas = New List(Of CrateMarka)()
        Type = ServerType
    End Sub
    Public Property OrgId As Integer
    Public Property CrateMarkas As List(Of CrateMarka)
    Public Property Type As Integer
End Class
Public Class CrateMarka
    Public Property CrateID As Integer
    Public Property MarkaName As String
    Public Property OpQty As String
    Public Property Rate As String
    Public Property OrgID As Integer
    Public Property Type As Integer
    Public Property ServerTag As Integer
End Class
Public Class SaveCrateMarkaResponse
    Inherits Response
End Class
