Imports System.IO
Imports System.Collections.Generic

Public Class Login
    Dim el As New Aadhat.ErrorLogger
    Dim ClsCommon As CommonClass = New CommonClass()
    ' Dim rs As New Resizer
    Private ReadOnly _loginControlLayout As New Dictionary(Of Control, LoginControlLayout)
    Private _loginLayoutCaptured As Boolean
    Private _lastLoginHorizontalOffset As Integer = Integer.MinValue
    Private Const Login100PercentScale As Single = 1.15F

    Private Structure LoginControlLayout
        Public Bounds As Rectangle
        Public Font As Font
    End Structure
    Private Sub Login_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        RemoveHandler CompanyList.Resize, AddressOf CompanyGridBoundsChanged
        RemoveHandler CompanyList.dg1.Resize, AddressOf CompanyGridBoundsChanged
        CompanyList.Enabled = True
    End Sub

    Private Sub Login_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Net.ServicePointManager.SecurityProtocol = CType(768 Or 3072, System.Net.SecurityProtocolType)
        ' Capture the designer dimensions before this overlay is fitted to the
        ' CompanyList grid.  That lets its child controls scale with the grid.
        CaptureLoginControlLayout()
        GenericResponsiveFormScaler.Bind(Me)
        '  rs.FindAllControls(Me)
        CompanyList.Enabled = False
        Me.BackColor = Color.DarkTurquoise
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        FitOverCompanyGrid()
        AddHandler CompanyList.Resize, AddressOf CompanyGridBoundsChanged
        AddHandler CompanyList.dg1.Resize, AddressOf CompanyGridBoundsChanged
        clsFun.FillDropDownList(CbUserName, "Select * From Users Order By Username Desc", "UserName", "Id", "")
        Me.KeyPreview = True
        CbUserName.BackColor = Color.DarkTurquoise
        txtPassword.UseSystemPasswordChar = True
        ApplyResponsiveLoginLayout()
    End Sub

    Private Sub Login_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ' The overlay is resized after Load.  Set focus on the next UI turn so
        ' the MDI/layout repaint cannot take it away from the user selector.
        BeginInvoke(New MethodInvoker(AddressOf FocusUserName))
    End Sub

    Private Sub FocusUserName()
        If IsDisposed OrElse Not Visible OrElse Not CbUserName.Visible OrElse Not CbUserName.Enabled Then Return
        CbUserName.Select()
        CbUserName.Focus()
    End Sub

    Private Sub CompanyGridBoundsChanged(sender As Object, e As EventArgs)
        FitOverCompanyGrid()
    End Sub

    Private Sub FitOverCompanyGrid()
        Dim gridBounds As Rectangle = CompanyGridBounds()
        If gridBounds.Width < 1 OrElse gridBounds.Height < 1 Then Return
        Me.Bounds = gridBounds
        ApplyResponsiveLoginLayout()
    End Sub

    ' Set the overlay before Show so the first frame already matches CompanyList.dg1.
    Public Sub PrepareGridOverlay()
        ' MDI can otherwise paint its default title bar for one frame before
        ' Login_Load runs.  Apply the final non-client state before Show.
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Me.ControlBox = False
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.Manual
        Dim scaler As GenericResponsiveFormScaler = GenericResponsiveFormScaler.Bind(Me)
        'scaler.PrepareForFirstShow()
        FitOverCompanyGrid()
    End Sub

    Private Function CompanyGridBounds() As Rectangle
        If CompanyList Is Nothing OrElse CompanyList.IsDisposed OrElse CompanyList.dg1 Is Nothing Then Return Rectangle.Empty

        Return New Rectangle(CompanyList.Left + CompanyList.dg1.Left,
                             CompanyList.Top + CompanyList.dg1.Top,
                             CompanyList.dg1.Width,
                             CompanyList.dg1.Height)
    End Function

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
            "SELECT UserType FROM Users WHERE username='" & CbUserName.Text &
            "' AND Password='" & txtPassword.Text & "'")

        If dt.Rows.Count = 0 Then
            MsgBox("Incorrect Password !!! Try Again...", vbCritical)
            txtPassword.Focus()
            Exit Sub
        End If

        If HybridRuntime.IsEnabled Then
            Dim status As HybridCompanyStatus = HybridRuntime.CheckCompanyStatus(Val(sCompCode).ToString(), CbUserName.Text)
            If status Is Nothing OrElse status.Success = False Then
                MsgBox("Hybrid server company access failed. Local single-user mode is unchanged, but this company is marked for hybrid check." & vbCrLf & If(status Is Nothing, "", status.Message), vbCritical, "Hybrid Access")
                Exit Sub
            End If

            Select Case status.ParsedMode()
                Case HybridCompanyMode.LocalOnly
                    'Continue with the existing local SQLite flow.
                Case HybridCompanyMode.OnlineOnly
                    MsgBox("This company is marked OnlineOnly on hybrid server. Online transaction modules are not enabled in this build yet.", vbInformation, "Hybrid Mode")
                    Exit Sub
                Case HybridCompanyMode.SyncingToOnline, HybridCompanyMode.SyncingToLocal
                    MsgBox("This company is currently syncing. Please try again after sync completes.", vbInformation, "Hybrid Mode")
                    Exit Sub
                Case HybridCompanyMode.ReadOnlyArchive
                    MsgBox("This company is available as read-only archive on hybrid server.", vbInformation, "Hybrid Mode")
                    Exit Sub
            End Select
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

        Dim loggedInUserType As String = Convert.ToString(dt.Rows(0)("UserType"))
        MainScreenForm.ApplyHybridPermission(loggedInUserType)

        MainScreenForm.UsersToolStripMenuItem1.Visible = Not String.Equals(loggedInUserType, "Operator", StringComparison.OrdinalIgnoreCase)

        Me.Dispose()
        ShowCompanies.Dispose()
        lblMsg.Visible = False
    End Sub


    Private Sub ShowApplyLicense()
        ApplyLicenseKey.MdiParent = ShowCompanies
        ApplyLicenseKey.PrepareGridOverlay()
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
        ApplyResponsiveLoginLayout()
    End Sub

    Private Sub CaptureLoginControlLayout()
        If _loginLayoutCaptured Then Return
        ' Keep the logo/branding column as designed. Only the controls on the
        ' cyan login area follow the available space in the company-grid overlay.
        For Each control As Control In Me.Controls
            If Object.ReferenceEquals(control, Panel1) Then Continue For
            _loginControlLayout(control) = New LoginControlLayout With {
                .Bounds = control.Bounds,
                .Font = control.Font
            }
        Next
        _loginLayoutCaptured = True
    End Sub
    Private Sub ApplyResponsiveLoginLayout()
        If Not _loginLayoutCaptured OrElse Me.IsDisposed OrElse Panel1 Is Nothing Then Return
        ' Preserve the existing 125% layout exactly. At 100%, make the login
        ' controls modestly larger while keeping the group centered in the cyan area.
        If GetLoginDpiX() > 96.0F Then Return
        Dim targetCenterX As Single = Panel1.Width + (Me.ClientSize.Width - Panel1.Width) / 2.0F
        Dim groupLeft As Integer = Integer.MaxValue
        Dim groupRight As Integer = Integer.MinValue
        Dim groupTop As Integer = Integer.MaxValue
        Dim groupBottom As Integer = Integer.MinValue
        For Each layoutEntry As KeyValuePair(Of Control, LoginControlLayout) In _loginControlLayout
            If Object.ReferenceEquals(layoutEntry.Key, lblMsg) Then Continue For
            groupLeft = Math.Min(groupLeft, layoutEntry.Value.Bounds.Left)
            groupRight = Math.Max(groupRight, layoutEntry.Value.Bounds.Right)
            groupTop = Math.Min(groupTop, layoutEntry.Value.Bounds.Top)
            groupBottom = Math.Max(groupBottom, layoutEntry.Value.Bounds.Bottom)
        Next
        If groupLeft = Integer.MaxValue OrElse groupRight <= groupLeft OrElse groupBottom <= groupTop Then Return
        Dim groupCenterX As Single = (groupLeft + groupRight) / 2.0F
        Dim groupCenterY As Single = (groupTop + groupBottom) / 2.0F
        Dim horizontalOffset As Integer = CInt(Math.Round(targetCenterX - groupCenterX))
        If horizontalOffset = _lastLoginHorizontalOffset Then Return
        For Each layoutEntry As KeyValuePair(Of Control, LoginControlLayout) In _loginControlLayout
            Dim control As Control = layoutEntry.Key
            If control Is Nothing OrElse control.IsDisposed Then Continue For
            Dim original As LoginControlLayout = layoutEntry.Value
            Dim controlWidthScale As Single = Login100PercentScale
            Dim controlHeightScale As Single = Login100PercentScale
            If TypeOf control Is TextBox Then
                controlWidthScale *= 1.02F
                controlHeightScale *= 0.98F
            End If
            Dim scaledWidth As Integer = Math.Max(1, CInt(Math.Round(original.Bounds.Width * controlWidthScale)))
            Dim scaledHeight As Integer = Math.Max(1, CInt(Math.Round(original.Bounds.Height * controlHeightScale)))
            Dim centerX As Single = targetCenterX + (original.Bounds.Left + original.Bounds.Width / 2.0F - groupCenterX) * Login100PercentScale
            Dim centerY As Single = groupCenterY + (original.Bounds.Top + original.Bounds.Height / 2.0F - groupCenterY) * Login100PercentScale
            control.SetBounds(CInt(Math.Round(centerX - scaledWidth / 2.0F)),
                              CInt(Math.Round(centerY - scaledHeight / 2.0F)),
                              scaledWidth,
                              scaledHeight)
            Dim originalFont As Font = original.Font
            If originalFont IsNot Nothing AndAlso originalFont.Size > 0 Then
                control.Font = New Font(originalFont.FontFamily,
                                        originalFont.Size * Login100PercentScale,
                                        originalFont.Style,
                                        originalFont.Unit,
                                        originalFont.GdiCharSet,
                                        originalFont.GdiVerticalFont)
            End If
        Next
        _lastLoginHorizontalOffset = horizontalOffset
    End Sub

    Private Function GetLoginDpiX() As Single
        Try
            Using graphics As Graphics = Me.CreateGraphics()
                Return graphics.DpiX
            End Using
        Catch
            Return 96.0F
        End Try
    End Function
End Class
