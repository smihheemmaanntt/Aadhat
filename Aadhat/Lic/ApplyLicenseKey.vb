Imports System.IO

Public Class ApplyLicenseKey
    Dim ClsCommon As New CommonClass()

    Private Sub ApplyLicenseKey_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Top = 130
        Me.Left = 84
        Me.FormBorderStyle = FormBorderStyle.None
        Me.KeyPreview = True
        Dim filePath As String = Path.Combine(Application.StartupPath, "coreaccess.smx")
        ' ======================================
        ' AMC MODE
        ' ======================================
        If File.Exists(filePath) Then
            lblMode.Text = "AMC Activation Mode"
            btnReleaseKey.Visible = True
            Dim store = AccentStorageHelper.LoadStore()
            If store IsNot Nothing Then
                ' 🔹 Customer ID (from response_data)
                If store.response_data IsNot Nothing Then
                    txtCustomerID.Text = store.response_data.customer_code
                End If

                ' 🔹 Fill details from coreaccess.smx
                If store.license_data IsNot Nothing Then
                    txtFrimName.Text = store.license_data.firm_name
                    txtfullAddress.Text = store.license_data.address
                    txtcity.Text = store.license_data.city
                    txtState.Text = store.license_data.state
                    txtMobile1.Text = store.license_data.mobile1
                    txtMobile2.Text = store.license_data.mobile2
                    txtEmail.Text = store.license_data.email
                End If
            End If

            ' 🔒 Disable ALL info fields
            txtCustomerID.Enabled = False
            txtFrimName.Enabled = False
            txtfullAddress.Enabled = False
            txtcity.Enabled = False
            txtState.Enabled = False
            txtMobile1.Enabled = False
            txtMobile2.Enabled = False
            txtEmail.Enabled = False

            ' 🔓 License / AMC key always enabled
            txtLicKey.Enabled = True

        Else
            ' ======================================
            ' FRESH LICENSE MODE
            ' ======================================
            lblMode.Text = "Fresh License Activation"
            btnReleaseKey.Visible = False
            txtCustomerID.Visible = False
            txtFrimName.Enabled = True
            txtfullAddress.Enabled = True
            txtcity.Enabled = True
            txtState.Enabled = True
            txtMobile1.Enabled = True
            txtMobile2.Enabled = True
            txtEmail.Enabled = True
            txtLicKey.Enabled = True
            ' 🔹 Auto-fill from system
            txtFrimName.Text = compname
            txtfullAddress.Text = Address
            txtcity.Text = City
            txtState.Text = State
            txtMobile1.Text = Mob1
            txtMobile2.Text = Mob2
            txtEmail.Text = Email
        End If
    End Sub

    Private Function IsCustomerCode(input As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(input, "^\d{6}$")
    End Function

    Private Function IsLicenseOrAmcKey(input As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(
            input,
            "^[A-Z0-9]{6}(-[A-Z0-9]{6}){3}$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase
        )
    End Function

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click

        Dim inputKey As String = txtLicKey.Text.Trim()

        If inputKey = "" Then
            MsgBox("Please enter License / AMC Key or Customer Code!", vbExclamation)
            Exit Sub
        End If

        Dim store = AccentStorageHelper.LoadStore()
        Dim filePath As String = Application.StartupPath & "\coreaccess.smx"

        '==============================================================
        ' SMART DECISION BASED ON INPUT FORMAT
        '==============================================================

        '-------------------------------
        ' CASE A : CUSTOMER CODE (6 DIGIT)
        '-------------------------------
        If IsCustomerCode(inputKey) Then

            If AccentStorageHelper.RetrieveLicense(inputKey) Then
                MsgBox("License retrieved and activated on this PC.", vbInformation)
                Me.Close()
            Else
                MsgBox("Unable to retrieve license. Please check customer code.", vbCritical)
            End If

            Exit Sub
        End If

        '-------------------------------
        ' CASE B : LICENSE / AMC KEY FORMAT
        '-------------------------------
        If Not IsLicenseOrAmcKey(inputKey) Then
            MsgBox("Invalid License / AMC Key format!", vbCritical)
            Exit Sub
        End If

        '==============================================================
        ' CASE 1 : FRESH LICENSE (NO coreaccess.smx)
        '==============================================================
        If Not File.Exists(filePath) Then

            Dim data As New LicenseData With {
                .license_key = inputKey,
                .product_id = 1,
                .board_id = AccentStorageHelper.GetMotherboardID(),
                .pc_name = Environment.MachineName,
                .firm_name = txtFrimName.Text,
                .address = txtfullAddress.Text,
                .city = txtcity.Text,
                .state = txtState.Text,
                .mobile1 = txtMobile1.Text,
                .mobile2 = txtMobile2.Text,
            .email = txtEmail.Text
            }

            Dim response = AccentStorageHelper.SaveLicense(data)
            Dim respObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomerActivationResponse)(response)

            MsgBox(respObj.message, If(respObj.status = "success", vbInformation, vbCritical))

            If respObj.status = "success" Then Me.Close()
            Exit Sub
        End If

        '==============================================================
        ' CASE 2 : AMC ACTIVATION (coreaccess.smx exists)
        '==============================================================
        If store Is Nothing OrElse store.response_data Is Nothing Then
            MsgBox("Base License Missing! Cannot Apply AMC.", vbCritical)
            Exit Sub
        End If

        Dim amcData As New AmcData With {
             .license_key = inputKey,
             .product_id = 1,
             .customer_code = store.response_data.customer_code,
             .board_id = AccentStorageHelper.GetMotherboardID()
        }

        Dim AMCresponse = AccentStorageHelper.SaveAmc(amcData)
        Dim amcRespObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AmcActivationResponse)(AMCresponse)

        MsgBox(amcRespObj.message, If(amcRespObj.status = "success", vbInformation, vbCritical))

        If amcRespObj.status = "success" Then Me.Close()

    End Sub

    'Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click

    '    If txtLicKey.Text.Trim = "" Then
    '        MsgBox("Please enter License / AMC Key!", vbExclamation)
    '        Exit Sub
    '    End If
    '    Dim filePath As String = Application.StartupPath & "\coreaccess.smx"
    '    '==============================================================
    '    '   CASE 1 : FRESH LICENSE
    '    '==============================================================
    '    If Not File.Exists(filePath) Then
    '        Dim data As New LicenseData With {
    '            .license_key = txtLicKey.Text.Trim,
    '            .product_id = 1,
    '            .board_id = AccentStorageHelper.GetMotherboardID(),
    '            .pc_name = Environment.MachineName,
    '            .firm_name = compname,
    '            .address = Address,
    '            .city = City,
    '            .state = State,
    '            .mobile1 = Mob1,
    '            .mobile2 = Mob2,
    '            .email = Email
    '        }
    '        Dim response = AccentStorageHelper.SaveLicense(data)
    '        MsgBox(response)
    '        Dim respObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomerActivationResponse)(response)
    '        ' 🔥 EXACT API MESSAGE SHOW HERE
    '        MsgBox(respObj.message, IIf(respObj.status = "success", vbInformation, vbCritical))
    '        If respObj.status = "success" Then Me.Close()
    '        Exit Sub
    '    End If
    '    '==============================================================
    '    '   CASE 2 : AMC ACTIVATION
    '    '==============================================================
    '    Dim store = AccentStorageHelper.LoadStore()
    '    If store Is Nothing OrElse store.response_data Is Nothing Then
    '        MsgBox("Base License Missing! Cannot Apply AMC.", vbCritical)
    '        Exit Sub
    '    End If
    '    Dim custID As String = store.response_data.customer_code
    '    Dim amcData As New AmcData With {
    '        .customer_id = custID,
    '        .license_key = txtLicKey.Text.Trim,
    '        .product_id = 1,
    '        .board_id = AccentStorageHelper.GetMotherboardID()
    '         }

    '    Dim AMCresponse = AccentStorageHelper.SaveAmc(amcData)
    '    Dim amcRespObj = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AmcActivationResponse)(AMCresponse)

    '    ' 🔥 EXACT AMC API MESSAGE SHOW HERE
    '    MsgBox(amcRespObj.message, IIf(amcRespObj.status = "success", vbInformation, vbCritical))

    '    If amcRespObj.status = "success" Then Me.Close()

    'End Sub



    Private Sub ApplyLicenseKey_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub btnReleaseKey_Click(sender As Object, e As EventArgs) Handles btnReleaseKey.Click
        Dim confirm = MsgBox(
            "This will release license from this PC." & vbCrLf &
            "You can activate it on another PC." & vbCrLf & vbCrLf &
            "Continue?",
            MsgBoxStyle.Question Or MsgBoxStyle.YesNo,
            "Transfer License"
        )

        If confirm <> MsgBoxResult.Yes Then Exit Sub

        If AccentStorageHelper.ReleaseBoardOnly() Then
            MsgBox("License released successfully." & vbCrLf &
                   "Now activate on new PC.",
                   MsgBoxStyle.Information)
            'Application.Exit()
        End If


    End Sub
End Class
