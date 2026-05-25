Public Class ComplaintRegisterRequest
    Public Property customer_code As String
    Public Property license_key As String
    Public Property mobile As String
    Public Property board_id As String
    Public Property pc_name As String
    Public Property subject As String
    Public Property description As String
    Public Property status As String
    Public Property source As String
End Class

Public Class ComplaintRegisterResponse
    Public Property status As String
    Public Property message As String
    Public Property complaint_id As Integer
    Public Property complaint_code As String
    Public Property complaint_status As String
    Public Property customer_code As String
    Public Property firm_name As String
End Class

Public Class ComplaintListRequest
    Public Property customer_code As String
    Public Property license_key As String
    Public Property mobile As String
End Class

Public Class ComplaintFeedbackItem
    Public Property feedback_subject As String
    Public Property feedback_comments As String
    Public Property feedback_date As String
    Public Property feedback_by As String
End Class

Public Class ComplaintListItem
    Public Property id As Integer
    Public Property complaint_code As String
    Public Property subject As String
    Public Property description As String
    Public Property status As String
    Public Property created_at As String
    Public Property feedbacks As List(Of ComplaintFeedbackItem)
End Class

Public Class ComplaintListResponse
    Public Property status As String
    Public Property message As String
    Public Property customer_code As String
    Public Property firm_name As String
    Public Property complaints As List(Of ComplaintListItem)
End Class
