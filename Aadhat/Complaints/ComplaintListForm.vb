Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports Newtonsoft.Json

Public Partial Class ComplaintListForm

    Private currentStore As FinalStore
    Private complaintItems As List(Of ComplaintListItem)

    Public Sub New()
        InitializeComponent()
        LoadCustomerData()
        LoadComplaints()
    End Sub

    Private Sub LoadCustomerData()
        Try
            currentStore = AccentStorageHelper.LoadStore()

            If currentStore IsNot Nothing AndAlso currentStore.response_data IsNot Nothing Then
                lblCustomerCodeValue.Text = currentStore.response_data.customer_code
            End If

            If currentStore IsNot Nothing AndAlso currentStore.license_data IsNot Nothing Then
                lblFirmNameValue.Text = currentStore.license_data.firm_name
                txtMobile.Text = currentStore.license_data.mobile1
            End If
        Catch
            lblStatus.Text = "Unable to load saved customer details."
        End Try
    End Sub

    Private Sub LoadComplaints()
        Try
            lblStatus.ForeColor = Color.FromArgb(13, 110, 253)
            lblStatus.Text = "Loading complaints..."
            btnRefresh.Enabled = False

            Dim licenseKey As String = ""
            If currentStore IsNot Nothing AndAlso currentStore.license_data IsNot Nothing Then
                licenseKey = currentStore.license_data.license_key
            End If

            Dim req As New ComplaintListRequest With {
                .customer_code = If(lblCustomerCodeValue.Text = "-", "", lblCustomerCodeValue.Text.Trim()),
                .license_key = licenseKey,
                .mobile = txtMobile.Text.Trim()
            }

            Dim json As String = AccentStorageHelper.PostJson(ComplaintListUrl, req)
            Dim resp = JsonConvert.DeserializeObject(Of ComplaintListResponse)(json)

            If resp Is Nothing OrElse resp.status <> "success" Then
                Dim msg As String = If(resp IsNot Nothing AndAlso resp.message <> "", resp.message, json)
                lblStatus.ForeColor = Color.FromArgb(220, 53, 69)
                lblStatus.Text = msg
                dgvComplaints.Rows.Clear()
                txtDetails.Clear()
                Return
            End If

            lblCustomerCodeValue.Text = resp.customer_code
            lblFirmNameValue.Text = resp.firm_name
            If resp.complaints Is Nothing Then
                complaintItems = New List(Of ComplaintListItem)()
            Else
                complaintItems = resp.complaints
            End If
            FillGrid()

            lblStatus.ForeColor = Color.FromArgb(25, 135, 84)
            lblStatus.Text = complaintItems.Count.ToString() & " complaint(s) loaded."
        Catch ex As Exception
            lblStatus.ForeColor = Color.FromArgb(220, 53, 69)
            lblStatus.Text = "Unable to load complaints."
            MessageBox.Show(ex.Message, "Complaint Status", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnRefresh.Enabled = True
        End Try
    End Sub

    Private Sub FillGrid()
        dgvComplaints.Rows.Clear()

        For Each item In complaintItems
            Dim latestFeedback As String = ""
            Dim feedbackDate As String = ""

            If item.feedbacks IsNot Nothing AndAlso item.feedbacks.Count > 0 Then
                latestFeedback = item.feedbacks(0).feedback_subject
                feedbackDate = FormatDateTimeText(item.feedbacks(0).feedback_date)
            End If

            Dim rowIndex = dgvComplaints.Rows.Add(
                item.complaint_code,
                item.status,
                item.subject,
                FormatDateTimeText(item.created_at),
                latestFeedback,
                feedbackDate
            )
            dgvComplaints.Rows(rowIndex).Tag = item
        Next

        If dgvComplaints.Rows.Count > 0 Then
            dgvComplaints.Rows(0).Selected = True
            ShowComplaintDetails(CType(dgvComplaints.Rows(0).Tag, ComplaintListItem))
        Else
            txtDetails.Text = "No complaints found."
        End If
    End Sub

    Private Function FormatDateTimeText(value As String) As String
        Dim dt As DateTime
        If DateTime.TryParse(value, dt) Then
            Return dt.ToString("dd-MM-yyyy hh:mm tt")
        End If
        Return value
    End Function

    Private Sub dgvComplaints_SelectionChanged(sender As Object, e As EventArgs) Handles dgvComplaints.SelectionChanged
        If dgvComplaints.SelectedRows.Count = 0 Then Return
        Dim item = TryCast(dgvComplaints.SelectedRows(0).Tag, ComplaintListItem)
        If item IsNot Nothing Then ShowComplaintDetails(item)
    End Sub

    Private Sub ShowComplaintDetails(item As ComplaintListItem)
        Dim sb As New StringBuilder()
        sb.AppendLine("Complaint No: " & item.complaint_code)
        sb.AppendLine("Status: " & item.status)
        sb.AppendLine("Date: " & FormatDateTimeText(item.created_at))
        sb.AppendLine("Subject: " & item.subject)
        sb.AppendLine()
        sb.AppendLine("Description:")
        sb.AppendLine(item.description)
        sb.AppendLine()
        sb.AppendLine("Feedback / Work Done:")

        If item.feedbacks IsNot Nothing AndAlso item.feedbacks.Count > 0 Then
            For Each fb In item.feedbacks
                sb.AppendLine("----------------------------------------")
                sb.AppendLine(FormatDateTimeText(fb.feedback_date) & " - " & fb.feedback_subject)
                If fb.feedback_comments IsNot Nothing AndAlso fb.feedback_comments.Trim() <> "" Then
                    sb.AppendLine(fb.feedback_comments)
                End If
            Next
        Else
            sb.AppendLine("No feedback added yet.")
        End If

        txtDetails.Text = sb.ToString()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadComplaints()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub rootPanel_Paint(sender As Object, e As PaintEventArgs) Handles rootPanel.Paint

    End Sub

    Private Sub ComplaintListForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub ComplaintListForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
    End Sub

    Private Sub lblStatus_Click(sender As Object, e As EventArgs) Handles lblStatus.Click

    End Sub
End Class
