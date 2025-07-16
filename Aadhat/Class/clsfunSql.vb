Imports MySql.Data.MySqlClient

Public Class clsfunSql
    Public Function GetConnectionstr() As String
        Dim conn As String
        conn = "Server=147.93.107.113;Database=smicloud_aadhat;Uid=smicloud;Pwd=Hemant@2025;;"
        Return conn
    End Function

    Public Function GetConnectionstring() As MySqlConnection
        Dim conn As MySqlConnection
        conn = New MySqlConnection("Server=147.93.107.113;Database=smicloud_aadhat;Uid=smicloud;Pwd=Hemant@2025;")
        Return conn
    End Function

    Public Shared Function GetConnection() As MySqlConnection
        Dim cs As String = "Server=147.93.107.113;Database=smicloud_aadhat;Uid=smicloud;Pwd=Hemant@2025;"

        Dim con As New MySqlConnection(cs)
        con.Open()
        Return con
    End Function

    Public Shared Function ExecDataTable(ByVal cmdText As String) As DataTable
        Dim ad As New MySqlDataAdapter(cmdText, GetConnection())
        Dim dt As DataTable = New DataTable()
        ad.Fill(dt)
        ad.Dispose()
        Return dt
    End Function

    Public Shared Function ToInt(ByVal Val As Object) As Integer
        Dim Result As Integer = 0
        Try
            Result = Convert.ToInt32(Convert.ToDecimal(Val))
        Catch ex As Exception
            Result = 0
        End Try
        Return Result
    End Function

    Public Shared Function ExecScalarInt(ByVal cmdText As String) As Integer
        Return ToInt(ExecScalarStr(cmdText))
    End Function

    Public Shared Function ExecScalarStr(ByVal cmdText As String) As String
        Dim Result As String = String.Empty
        Dim Con As MySqlConnection = GetConnection()
        Dim cmd As MySqlCommand = New MySqlCommand(cmdText, Con)
        Dim obj As Object = cmd.ExecuteScalar()
        cmd.Dispose()
        Con.Dispose()
        Return ToStr(obj)
    End Function

    Public Shared Function ToStr(ByVal Val As Object) As String
        Dim Result As String = String.Empty
        Try
            Result = Val.ToString()
        Catch ex As Exception
            Result = String.Empty
        End Try
        Return Result
    End Function
End Class
