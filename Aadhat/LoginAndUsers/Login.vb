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


    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        UpdateDatabase()
        Dim dt As New DataTable
        dt = clsFun.ExecDataTable("Select COUNT(*) from Users  where  username='" & CbUserName.Text & "' and Password='" & txtPassword.Text & "'")
        Dim fileName As String = AppDomain.CurrentDomain.BaseDirectory & "accent.dll"
        If dt.Rows(0)(0) > 0 Then
            'Application.DoEvents()
            If clsFun.CheckLicence = False Then
                lblMsg.Visible = True
                lblMsg.Text = "Login Successfuly..."
                If File.Exists(fileName) = True Then MainScreenForm.lblARC.Text = "ARC Extended"
                MainScreenPicture.lblUser.Text = CbUserName.Text
                MainScreenForm.Show() ' : OpenWhatsapp()
                Dim OperatorType As String = clsFun.ExecScalarStr("Select Usertype From Users Where ID='" & Val(CbUserName.SelectedValue) & "'")
                If OperatorType = "Operator" Then MainScreenForm.UsersToolStripMenuItem1.Visible = False
                Me.Dispose() : ShowCompanies.Dispose()
                el.WriteToErrorLog("Login Successfuly..." & CbUserName.Text, Constants.compname, "Login Successfuly...")
                lblMsg.Visible = False ': OpenWhatsapp()
            Else
                If clsFun.CheckLicence = True Then
                    If Not File.Exists(fileName) Then
                        ApplyLicenseKey.MdiParent = ShowCompanies
                        ApplyLicenseKey.Show()
                        If Not ApplyLicenseKey Is Nothing Then
                            ApplyLicenseKey.BringToFront()
                        End If
                    Else
                        Dim LicCheck As Boolean = ClsCommon.LicenseCheck(fileName)
                        If LicCheck = True Then
                            If ClsCommon.IsLicenseBlocked() Then
                                MsgBox("License is Blocked !!! Please Contact to Software Vendor ", vbOKOnly, "Blocked") : Exit Sub
                                el.WriteToErrorLog("License is Blocked !!!" & CbUserName.Text, Constants.compname, "License is Blocked !!!")
                                ' clsFun.ChangePath(Data)
                                Exit Sub
                            Else
                                Data = CompanyList.dg1.SelectedRows(0).Cells(7).Value
                                MainScreenPicture.lblUser.Text = CbUserName.Text
                                MainScreenForm.Show()
                                Dim OperatorType As String = clsFun.ExecScalarStr("Select Usertype From Users Where ID='" & Val(CbUserName.SelectedValue) & "'")
                                If OperatorType = "Operator" Then MainScreenForm.UsersToolStripMenuItem1.Visible = False
                                lblMsg.Visible = True
                                lblMsg.Text = "Login Successfuly..."
                                el.WriteToErrorLog("Login Successfuly..." & CbUserName.Text, Constants.compname, "Login Successfuly...")
                                Me.Dispose() : ShowCompanies.Dispose() ': OpenWhatsapp()
                                lblMsg.Visible = False
                            End If
                        End If
                    End If
                End If
            End If
        Else
            MsgBox("Incorrect Password !!! Try Again... ", vbOKOnly, "invalid Password")
            el.WriteToErrorLog("Incorrect Password !!!" & CbUserName.Text, Constants.compname, "Login Successfuly...")
            txtPassword.Focus()
        End If
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