Imports System.Data
Imports System.Drawing
Imports System.Windows.Forms

Public Class WhatsAppOfficialMessageLog
    Public VendorUid As String = ""
    Public AccessToken As String = ""

    Private Sub WhatsAppOfficialMessageLog_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        dtpFrom.Value = Date.Today.AddDays(-7)
        dtpTo.Value = Date.Today
        cbStatus.SelectedIndex = 0
        LoadLogs()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        LoadLogs()
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClear.Click
        txtSearch.Clear()
        cbStatus.SelectedIndex = 0
        dtpFrom.Value = Date.Today.AddDays(-7)
        dtpTo.Value = Date.Today
        LoadLogs()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            LoadLogs()
        End If
    End Sub

    Private Sub LoadLogs()
        Try
            Dim dt As DataTable = Nothing
            Dim errorMessage As String = ""
            Dim statusValue As String = SelectedStatusValue()

            If WhatsAppOfficialApi.GetMessageLogs( _
                VendorUid, _
                AccessToken, _
                dtpFrom.Value.ToString("yyyy-MM-dd"), _
                dtpTo.Value.ToString("yyyy-MM-dd"), _
                txtSearch.Text.Trim(), _
                statusValue, _
                dt, _
                errorMessage _
            ) = False Then
                lblStatus.Text = "Status: " & WhatsAppOfficialApi.FormatDisplayMessage(errorMessage)
                lblStatus.ForeColor = Color.Maroon
                dgvLogs.DataSource = New DataTable()
                Exit Sub
            End If

            dgvLogs.DataSource = dt
            ApplyGrid()
            lblStatus.Text = "Status: " & dt.Rows.Count & " log(s) loaded"
            lblStatus.ForeColor = Color.Navy
        Catch ex As Exception
            lblStatus.Text = "Status: " & ex.Message
            lblStatus.ForeColor = Color.Maroon
        End Try
    End Sub

    Private Function SelectedStatusValue() As String
        If cbStatus.SelectedItem Is Nothing Then Return "all"
        Dim value As String = cbStatus.SelectedItem.ToString().Trim().ToLower()
        If value = "sent" Then Return "accepted"
        If value = "held" Then Return "held_for_quality_assessment"
        If value = "delivered" Then Return "delivered"
        If value = "read" Then Return "read"
        If value = "failed" Then Return "failed"
        Return "all"
    End Function

    Private Sub ApplyGrid()
        dgvLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvLogs.EnableHeadersVisualStyles = False
        dgvLogs.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 240, 248)
        dgvLogs.ColumnHeadersDefaultCellStyle.ForeColor = Color.Navy
        dgvLogs.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 105, 160)
        dgvLogs.DefaultCellStyle.SelectionForeColor = Color.White

        If dgvLogs.Columns.Contains("MessageTime") Then dgvLogs.Columns("MessageTime").HeaderText = "Time"
        If dgvLogs.Columns.Contains("MobileNo") Then dgvLogs.Columns("MobileNo").HeaderText = "Mobile No"
        If dgvLogs.Columns.Contains("ApproxCharge") Then dgvLogs.Columns("ApproxCharge").HeaderText = "Approx Charge"
        If dgvLogs.Columns.Contains("WAMID") Then dgvLogs.Columns("WAMID").HeaderText = "Message ID"
        If dgvLogs.Columns.Contains("Message") Then dgvLogs.Columns("Message").Visible = False

        If dgvLogs.Columns.Contains("MessageTime") Then dgvLogs.Columns("MessageTime").FillWeight = 95
        If dgvLogs.Columns.Contains("MobileNo") Then dgvLogs.Columns("MobileNo").FillWeight = 75
        If dgvLogs.Columns.Contains("Template") Then dgvLogs.Columns("Template").FillWeight = 80
        If dgvLogs.Columns.Contains("Status") Then dgvLogs.Columns("Status").FillWeight = 60
        If dgvLogs.Columns.Contains("Response") Then dgvLogs.Columns("Response").FillWeight = 170
        If dgvLogs.Columns.Contains("ApproxCharge") Then dgvLogs.Columns("ApproxCharge").FillWeight = 125
        If dgvLogs.Columns.Contains("WAMID") Then dgvLogs.Columns("WAMID").FillWeight = 140

        If dgvLogs.Columns.Contains("Status") = False Then Exit Sub
        For Each row As DataGridViewRow In dgvLogs.Rows
            If row.IsNewRow Then Continue For
            Dim statusText As String = ""
            If row.Cells("Status").Value IsNot Nothing Then statusText = row.Cells("Status").Value.ToString().ToUpper()

            If statusText.Contains("READ") OrElse statusText.Contains("DELIVERED") OrElse statusText.Contains("SENT") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(224, 245, 232)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(0, 100, 45)
            ElseIf statusText.Contains("HELD") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 215)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(120, 85, 0)
            ElseIf statusText.Contains("FAILED") Then
                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230)
                row.DefaultCellStyle.ForeColor = Color.Maroon
            Else
                row.DefaultCellStyle.BackColor = Color.FromArgb(242, 246, 252)
                row.DefaultCellStyle.ForeColor = Color.FromArgb(45, 60, 85)
            End If
        Next
    End Sub
End Class
