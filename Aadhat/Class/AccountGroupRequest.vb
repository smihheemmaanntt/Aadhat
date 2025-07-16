Public Class AddAccountGroupRequest
    Dim servertype As Object
    Public Sub New()
        AccountGroups = New List(Of AccountGroupRequest)
        Type = ServerType
    End Sub
    Public Property AccountGroups As List(Of AccountGroupRequest)
    Public Property Type As Integer
End Class

Public Class AccountGroupRequest
    Public Property GroupId As Integer = 0
    Public Property GroupName As String = ""
    Public Property UnderGroupID As Integer = 0
    Public Property UnderGroupName As String = ""
    Public Property DC As String = ""
    Public Property Primary2 As String = ""
    Public Property Tag As String = ""
    Public Property OrganizationId As Integer
    Public Property Type As Integer
    Public Property ServerTag As Integer
End Class

Public Class AccountGroupResponse
    Inherits Response
End Class

