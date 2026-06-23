Imports System.IO

Public Class Login
    Dim el As New Aadhat.ErrorLogger
    Dim ClsCommon As CommonClass = New CommonClass()
    ' Dim rs As New Resizer
    Private Sub Login_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        CompanyList.Enabled = True
    End Sub

    Private Sub Login_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Net.ServicePointManager.SecurityProtocol = CType(768 Or 3072, System.Net.SecurityProtocolType)
        '  rs.FindAllControls(Me)
        CompanyList.Enabled = False
        Me.Top = 130 : Me.Left = 84
        Me.BackColor = Color.DarkTurquoise
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        clsFun.FillDropDownList(CbUserName, "Select * From Users Order By Username Desc", "UserName", "Id", "")
        Me.KeyPreview = True
        CbUserName.BackColor = Color.DarkTurquoise
        txtPassword.UseSystemPasswordChar = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub OpenWhatsapp()
        Try
            Dim p() As Process
            p = Process.GetProcessesByName("Easy Whatsapp")
            If p.Count = 0 Then
                Dim StartWhatsapp As New System.Diagnostics.Process
                StartWhatsapp.StartInfo.FileName = Application.StartupPath & "\Whatsapp\Easy Whatsapp.exe"
                StartWhatsapp.Start()
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click

        UpdateDatabase()

        ' ---------------- USER AUTH ----------------
        Dim dt = clsFun.ExecDataTable(
            "SELECT COUNT(*) FROM Users WHERE username='" & CbUserName.Text &
            "' AND Password='" & txtPassword.Text & "'")

        If dt.Rows(0)(0) = 0 Then
            MsgBox("Incorrect Password !!! Try Again...", vbCritical)
            txtPassword.Focus()
            Exit Sub
        End If

        ' ---------------- LICENSE / TRIAL CHECK ----------------
        Dim coreAccessPath As String = Path.Combine(Application.StartupPath, "coreaccess.smx")
        Dim hasCoreAccess As Boolean = File.Exists(coreAccessPath)

        If hasCoreAccess Then
            If Not AccentStorageHelper.IsLicenseUsable() Then
                If AccentStorageHelper.LastLicenseError = "blocked" Then
                    MsgBox("Licence is blocked.Please contact your service provider...", vbCritical, "Access Denied")
                    Exit Sub
                End If
                If AccentStorageHelper.LastLicenseError = "expired" Then
                    MsgBox("License expired. please contact your service provider", vbCritical, "Access Denied")
                Else
                    MsgBox("Invalid Licence.please contact your service provider", vbCritical, "Access Denied")
                End If

                ShowApplyLicense()
                Exit Sub
            End If
        Else
            Dim isExpired As Boolean = AccentStorageHelper.CheckLicence()
            If isExpired = True Then
                ShowApplyLicense()
                Exit Sub
            End If
        End If

        ' ---------------- LOGIN SUCCESS ----------------
        lblMsg.Visible = True
        lblMsg.Text = "Login Successfully..."

        Dim daysLeft = AccentStorageHelper.GetRemainingDays()
        MainScreenForm.lblARC.Text =
            If(daysLeft > 0, "ARC Expire In Next " & daysLeft & " Days", If(AccentStorageHelper.IsTrialMode(), "Trial Mode", ""))

        MainScreenPicture.lblUser.Text = CbUserName.Text
        MainScreenForm.Show()

        If clsFun.ExecScalarStr(
            "SELECT Usertype FROM Users WHERE ID='" & Val(CbUserName.SelectedValue) & "'") = "Operator" Then
            MainScreenForm.UsersToolStripMenuItem1.Visible = False
        End If

        Me.Dispose()
        ShowCompanies.Dispose()
        lblMsg.Visible = False
    End Sub


    Private Sub ShowApplyLicense()
        ApplyLicenseKey.MdiParent = ShowCompanies
        ApplyLicenseKey.Show()
        ApplyLicenseKey.BringToFront()
    End Sub



    Private Sub UpdateDatabase()
        clsFun.ExecNonQuery("Update UnderGroup set DC='Cr' Where ID=10 and DC='Dr'")
        'clsFun.ExecNonQuery("ALTER TABLE Transaction2  ADD PurchaseID INTEGER;")
    End Sub
    Private Sub CbUserName_KeyDown(sender As Object, e As KeyEventArgs) Handles CbUserName.KeyDown, txtPassword.KeyDown, btnViewPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
            e.SuppressKeyPress = True
        End If

    End Sub

    Private Sub CbUserName_Leave(sender As Object, e As EventArgs) Handles CbUserName.Leave
        If clsFun.ExecScalarInt("Select COUNT(*) from Users  where  username='" & CbUserName.Text & "'") = 0 Then
            MsgBox("User Not Found in Database...", vbOKOnly, "Access Denied")
            CbUserName.Focus()
            Exit Sub
        End If
    End Sub



    Private Sub btnViewPassword_Click(sender As Object, e As EventArgs) Handles btnViewPassword.Click
        If txtPassword.UseSystemPasswordChar = True Then
            txtPassword.UseSystemPasswordChar = False
            txtPassword.Focus() : txtPassword.SelectAll()
        Else
            txtPassword.UseSystemPasswordChar = True
            txtPassword.Focus() : txtPassword.SelectAll()
        End If
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        btnViewPassword.Visible = True
    End Sub

    Private Sub txtPassword_Leave(sender As Object, e As EventArgs) Handles txtPassword.Leave

    End Sub

    Private Sub txtPassword_TextChanged_1(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        If txtPassword.Text = "" Then btnViewPassword.Visible = False
    End Sub

    Private Sub Login_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'rs.ResizeAllControls(Me)
    End Sub
End Class
