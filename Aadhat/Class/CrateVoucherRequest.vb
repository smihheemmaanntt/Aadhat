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
    'The server contract is Int32; this makes JSON emit a number rather than a string.
    Public Property SlipNo As Integer = 0
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
    Public Shared Function ParseSlipNo(ByVal value As Object, Optional ByVal recordId As Object = Nothing) As Integer
        Dim rawValue As String = If(value Is Nothing OrElse value Is DBNull.Value, String.Empty, Convert.ToString(value, Globalization.CultureInfo.InvariantCulture).Trim())
        Dim slipNumber As Decimal
        If rawValue Is Nothing OrElse rawValue.Trim().Length = 0 OrElse
           Not Decimal.TryParse(rawValue, Globalization.NumberStyles.Number, Globalization.CultureInfo.InvariantCulture, slipNumber) OrElse
           Decimal.Truncate(slipNumber) <> slipNumber OrElse
           slipNumber < Integer.MinValue OrElse slipNumber > Integer.MaxValue Then
            Dim rowDescription As String = If(recordId Is Nothing OrElse recordId Is DBNull.Value, String.Empty, " in CrateVoucher row " & Convert.ToString(recordId, Globalization.CultureInfo.InvariantCulture))
            Throw New InvalidOperationException("SlipNo" & rowDescription & " must be a whole number within the Int32 range. Found: '" & rawValue & "'.")
        End If
        Return Decimal.ToInt32(slipNumber)
    End Function
End Class
Public Class SaveCrateVoucherResponse
    Inherits Response
End Class

