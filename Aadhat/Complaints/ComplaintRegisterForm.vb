Imports System.Drawing
Imports System.Linq
Imports System.Net
Imports System.Windows.Forms
Imports Newtonsoft.Json

Partial Public Class ComplaintRegisterForm

    Private currentStore As FinalStore

    Public Sub New()
        InitializeComponent()
        If cboSubject.Items.Count > 0 Then cboSubject.SelectedIndex = 0
        LoadCustomerData()
    End Sub

    Private Sub LoadCustomerData()
        Try
            currentStore = AccentStorageHelper.LoadStore()
            If currentStore Is Nothing Then Return

            If currentStore.response_data IsNot Nothing Then
                lblCustomerCodeValue.Text = currentStore.response_data.customer_code
            End If

            If currentStore.license_data IsNot Nothing Then
                lblFirmNameValue.Text = currentStore.license_data.firm_name
                txtMobile.Text = currentStore.license_data.mobile1
            End If
        Catch
            lblStatus.Text = "Unable to load saved customer details."
        End Try
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If lblCustomerCodeValue.Text.Trim() = "" OrElse lblCustomerCodeValue.Text.Trim() = "-" Then
            MessageBox.Show("Customer code is required.", "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim mobileDigits As String = OnlyDigits(txtMobile.Text)
        If mobileDigits = "" OrElse mobileDigits.Length < 10 Then
            MessageBox.Show("Please enter mobile number before submitting complaint.", "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtMobile.Focus()
            Return
        End If

        If cboSubject.SelectedItem Is Nothing OrElse cboSubject.Text.Trim() = "" Then
            MessageBox.Show("Please select complaint subject.", "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cboSubject.Focus()
            Return
        End If

        If txtDescription.Text.Trim() = "" Then
            MessageBox.Show("Please enter complaint description.", "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtDescription.Focus()
            Return
        End If

        Try
            btnSubmit.Enabled = False
            lblStatus.ForeColor = Color.FromArgb(13, 110, 253)
            lblStatus.Text = "Submitting complaint..."

            Dim licenseKey As String = ""
            If currentStore IsNot Nothing AndAlso currentStore.license_data IsNot Nothing Then
                licenseKey = currentStore.license_data.license_key
            End If

            Dim req As New ComplaintRegisterRequest With {
                .customer_code = lblCustomerCodeValue.Text.Trim(),
                .license_key = licenseKey,
                .mobile = mobileDigits,
                .board_id = AccentStorageHelper.GetMotherboardID(),
                .pc_name = Environment.MachineName,
                .subject = cboSubject.Text.Trim(),
                .description = BuildDescriptionWithMobile(txtDescription.Text, mobileDigits),
                .status = "Open",
                .source = "vb.net"
            }

            Dim json As String = AccentStorageHelper.PostJson(ComplaintRegisterUrl, req)
            Dim resp = JsonConvert.DeserializeObject(Of ComplaintRegisterResponse)(json)

            If resp IsNot Nothing AndAlso resp.status = "success" Then
                lblStatus.ForeColor = Color.FromArgb(25, 135, 84)
                lblStatus.Text = "Complaint registered: " & resp.complaint_code
                MessageBox.Show("Complaint registered successfully." & Environment.NewLine & "Complaint No: " & resp.complaint_code, "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Information)
                txtDescription.Clear()
            Else
                Dim msg As String = If(resp IsNot Nothing AndAlso resp.message <> "", resp.message, json)
                lblStatus.ForeColor = Color.FromArgb(220, 53, 69)
                lblStatus.Text = msg
                MessageBox.Show(msg, "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As WebException
            lblStatus.ForeColor = Color.FromArgb(220, 53, 69)
            lblStatus.Text = "Server connection failed."
            MessageBox.Show("Server connection failed. Please check internet and try again.", "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            lblStatus.ForeColor = Color.FromArgb(220, 53, 69)
            lblStatus.Text = ex.Message
            MessageBox.Show(ex.Message, "Complaint", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnSubmit.Enabled = True
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Function OnlyDigits(value As String) As String
        If value Is Nothing Then Return ""
        Dim chars = value.Where(Function(ch) Char.IsDigit(ch)).ToArray()
        Return New String(chars)
    End Function

    Private Function BuildDescriptionWithMobile(description As String, mobileDigits As String) As String
        Dim cleanDescription As String = If(description, "").Trim()
        Dim mobileText As String = "(" & mobileDigits & ")"

        If cleanDescription.EndsWith(mobileText) Then
            Return cleanDescription
        End If

        Return cleanDescription & " " & mobileText
    End Function

    Private Sub lblTitle_Click(sender As Object, e As EventArgs) Handles lblTitle.Click

    End Sub

    Private Sub rootPanel_Paint(sender As Object, e As PaintEventArgs) Handles rootPanel.Paint

    End Sub

    Private Sub ComplaintRegisterForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub ComplaintRegisterForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 0 : Me.Left = 0
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True

    End Sub

    Private Sub txtMobile_KeyDown(sender As Object, e As KeyEventArgs) Handles txtMobile.KeyDown, cboSubject.KeyDown, txtDescription.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If
    End Sub
End Class
