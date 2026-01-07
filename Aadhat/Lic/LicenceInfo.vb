Imports System.IO
Imports System.Collections

Public Class LicenceInfo

    Private Sub LicenceInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim store = AccentStorageHelper.LoadStore()
        If store Is Nothing OrElse store.license_data Is Nothing OrElse store.response_data Is Nothing Then
            MsgBox("Invalid License File!", MsgBoxStyle.Critical)
            Exit Sub
        End If

        Dim L = store.license_data
        Dim R = store.response_data

        '================================================
        ' BASIC DETAILS
        '================================================
        txtFirmName.Text = Safe(L.firm_name)
        txtAddress.Text = Trim(Safe(L.address) & ", " & Safe(L.city) & ", " & Safe(L.state))
        txtMobile.Text = Trim(Safe(L.mobile1) & If(Safe(L.mobile2) <> "", ", " & Safe(L.mobile2), ""))
        txtEmail.Text = Safe(L.email)
        txtBoardID.Text = Safe(L.board_ids)
        txtPCName.Text = Safe(L.pc_name)

        txtCustomerID.Text = Safe(R.customer_code)
        txtLicenseType.Text = Safe(R.license_type)

        ' 🔹 License Effective From
        txtStartDate.Text = FormatDate(R.license_effective_from)

        '================================================
        ' FINAL EXPIRY (License + AMC)
        '================================================
        Dim finalExpiry As Date = Date.MinValue

        ' AMC expiry priority
        If store.amc IsNot Nothing AndAlso store.amc.Count > 0 Then
            For Each A In store.amc
                Dim d As Date
                If Date.TryParse(GetAnyProp(A, "amc_end", "amc_end_date"), d) Then
                    If d > finalExpiry Then finalExpiry = d
                End If
            Next
        End If

        ' Fallback → License expiry
        If finalExpiry = Date.MinValue Then
            Date.TryParse(R.license_expiry_date, finalExpiry)
        End If

        '================================================
        ' SHOW EXPIRY + DAYS
        '================================================
        If finalExpiry <> Date.MinValue Then
            txtExpiryDate.Text = finalExpiry.ToString("dd-MM-yyyy")
            Dim daysLeft = CInt((finalExpiry - Date.Today).TotalDays)
            If daysLeft < 0 Then daysLeft = 0
            txtRemainingDays.Text = daysLeft & " Days"
        Else
            txtExpiryDate.Text = "—"
            txtRemainingDays.Text = "—"
        End If

        '================================================
        ' GRID SETUP
        '================================================
        dgvKeys.Rows.Clear()
        dgvKeys.Columns.Clear()

        dgvKeys.Columns.Add("key", "KEY")
        dgvKeys.Columns.Add("type", "TYPE")
        dgvKeys.Columns.Add("start", "START")
        dgvKeys.Columns.Add("end", "END")
        dgvKeys.Columns.Add("days", "DAYS")

        dgvKeys.AllowUserToAddRows = False
        dgvKeys.ReadOnly = True
        dgvKeys.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvKeys.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

        '================================================
        ' LICENSE ROW (Effective → Expiry)
        '================================================
        Dim licStart As Date
        Dim licEnd As Date
        Date.TryParse(R.license_effective_from, licStart)
        Date.TryParse(R.license_expiry_date, licEnd)

        dgvKeys.Rows.Add(
            Safe(L.license_key),
            "License",
            FormatDate(R.license_effective_from),
            FormatDate(R.license_expiry_date),
            If(licStart <> Date.MinValue AndAlso licEnd <> Date.MinValue,
               DateDiff(DateInterval.Day, licStart, licEnd) & " Days",
               "")
        )

        '================================================
        ' AMC ROWS
        '================================================
        If store.amc IsNot Nothing Then
            For Each A In store.amc
                Dim st As Date
                Dim en As Date
                Date.TryParse(GetAnyProp(A, "amc_start", "amc_start_date"), st)
                Date.TryParse(GetAnyProp(A, "amc_end", "amc_end_date"), en)

                dgvKeys.Rows.Add(
                    Safe(GetAnyProp(A, "license_key")),
                    "AMC",
                    FormatDate(GetAnyProp(A, "amc_start", "amc_start_date")),
                    FormatDate(GetAnyProp(A, "amc_end", "amc_end_date")),
                    If(s <> Date.MinValue AndAlso en <> Date.MinValue,
                       DateDiff(DateInterval.Day, st, en) & " Days",
                       "")
                )
            Next
        End If

        '================================================
        ' FINAL GRID POLISH
        '================================================
        dgvKeys.AutoResizeColumns()
        dgvKeys.Columns("key").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

    End Sub

    '================================================
    ' HELPERS
    '================================================
    Private Function FormatDate(v As String) As String
        Dim d As Date
        If Date.TryParse(v, d) Then
            Return d.ToString("dd-MM-yyyy")
        End If
        Return ""
    End Function

    Private Function GetAnyProp(obj As Object, ParamArray names() As String) As String
        For Each n In names
            Dim v = GetProp(obj, n)
            If v <> "" Then Return v
        Next
        Return ""
    End Function

    Private Function GetProp(obj As Object, propName As String) As String
        If obj Is Nothing Then Return ""
        Try
            Dim t = obj.GetType()
            Dim p = t.GetProperty(propName)
            If p IsNot Nothing Then
                Dim v = p.GetValue(obj, Nothing)
                If v IsNot Nothing Then Return v.ToString()
            End If
            Dim f = t.GetField(propName)
            If f IsNot Nothing Then
                Dim v = f.GetValue(obj)
                If v IsNot Nothing Then Return v.ToString()
            End If
        Catch
        End Try
        Return ""
    End Function

    Private Function Safe(s As String) As String
        If s Is Nothing Then Return ""
        Return s.Trim()
    End Function

End Class
