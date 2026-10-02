Public NotInheritable Class PerUnitCalculator
    Private Sub New()
    End Sub

    'Compatibility overload for existing callers using the standard CbPer order.
    Public Shared Function KgDivisor(ByVal selectedIndex As Integer) As Decimal
        Return KgDivisor(selectedIndex, 18)
    End Function

    Public Shared Function KgDivisor(ByVal selectedIndex As Integer, ByVal itemCount As Integer) As Decimal
        'Entry form CbPer lists share one ordered set of units. itemCount is
        'kept in the signature for compatibility, but indices are common now.
        Select Case selectedIndex
            Case 1 : Return 1D
            Case 2 : Return 5D
            Case 3 : Return 10D
            Case 4 : Return 20D
            Case 5 : Return 40D
            Case 6 : Return 41D
            Case 7 : Return 42D
            Case 8 : Return 50D
            Case 9 : Return 51D
            Case 10 : Return 51.7D
            Case 11 : Return 52.2D
            Case 12 : Return 52.3D
            Case 13 : Return 52.5D
            Case 14 : Return 53D
            Case 15 : Return 80D
            Case 16 : Return 100D
        End Select
        Return 0D
    End Function
End Class
