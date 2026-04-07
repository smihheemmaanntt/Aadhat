Option Strict Off
Option Explicit On
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.IO

Friend Class ShowCompanies
    Inherits System.Windows.Forms.Form
    Dim rs As New Resizer
    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function LoadKeyboardLayout(pwszKLID As String, Flags As UInteger) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function ActivateKeyboardLayout(hkl As IntPtr, Flags As UInteger) As IntPtr
    End Function
    ' Constants
    Private Const KLF_ACTIVATE As UInteger = &H1
    ' Keyboard layout identifiers
    Private Const KLID_ENGLISH_INDIA As String = "00000409"
    Private Const KLID_ENGLISH_US As String = "00004009"
    Private Const KLID_HINDI As String = "00000439"
    Private Const KLID_GUJARATI As String = "00000447"

    Private Sub ChangeKeyboardLayout(layoutId As String)
        Dim hkl As IntPtr = LoadKeyboardLayout(layoutId, KLF_ACTIVATE)
        If hkl = IntPtr.Zero Then
            MessageBox.Show("Failed to load keyboard layout.")
        Else
            ActivateKeyboardLayout(hkl, KLF_ACTIVATE)
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.F1
                ChangeKeyboardLayout(KLID_ENGLISH_INDIA)
            Case Keys.F2
                ChangeKeyboardLayout(KLID_ENGLISH_US)
            Case Keys.F6
                ChangeKeyboardLayout(KLID_HINDI)
            Case Keys.F4
                ChangeKeyboardLayout(KLID_GUJARATI)
        End Select
    End Sub

    Private Sub ShowCompanies_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'clsFun.changeCompany()
        Application.Exit()
    End Sub
    Private Sub ShowCompanies_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F3 Then
            Create_Company.MdiParent = Me
            Create_Company.Show()
            If Not Create_Company Is Nothing Then
                Create_Company.BringToFront()
            End If
        End If
        If e.KeyCode = Keys.F12 Then
            If CompanyList.txtPath.Visible = True Then
                CompanyList.txtPath.Visible = False
            ElseIf CompanyList.txtPath.Visible = False Then
                CompanyList.txtPath.Visible = True
            End If
        End If
        If e.KeyCode = Keys.F5 Then
            CompanyList.rowColums() : CompanyList.getCompanies()
            MsgBox("Company List Refreshed Successfully...", MsgBoxStyle.Information, "Refreshed")
        End If
        If e.KeyCode = Keys.F2 Then
            If CompanyList.btnPath.Visible = True Then
                CompanyList.btnPath.Visible = False
                CompanyList.txtMainPath.Visible = False
            Else
                CompanyList.btnPath.Visible = True
                CompanyList.txtMainPath.Visible = True
            End If
        End If
    End Sub
    'Private Sub ShowCompanies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    '    Dim fecha As Date = IO.File.GetCreationTime(Assembly.GetExecutingAssembly().Location)
    '    Dim version As Version = Assembly.GetExecutingAssembly().GetName().Version
    '    AccentStorageHelper.LoadStore()
    '    Me.Text = "Aadhat #" & version.ToString() & " " & CDate(fecha).ToString("yyMMdd")
    '    rs.FindAllControls(Me)
    '    CompanyList.MdiParent = Me
    '    CompanyList.Show()
    '    Me.Top = 50 : Me.Left = 50
    '    ' Me.WindowState = FormWindowState.Maximized
    '    Me.KeyPreview = True
    '    Dim screenWidth As Integer = Screen.PrimaryScreen.WorkingArea.Width
    '    Dim screenHeight As Integer = Screen.PrimaryScreen.WorkingArea.Height + 48
    'End Sub
    Private Sub ShowCompanies_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim fecha As Date = IO.File.GetCreationTime(Assembly.GetExecutingAssembly().Location)
        Dim version As Version = Assembly.GetExecutingAssembly().GetName().Version
        Dim storePath As String = Path.Combine(Application.StartupPath, "coreaccess.smx")
        Dim customerCode As String = ""
        ' 👉 Load Store
        If IO.File.Exists(storePath) Then
            Dim store = AccentStorageHelper.LoadStore()
            If store IsNot Nothing AndAlso store.response_data IsNot Nothing Then
                customerCode = Safe(store.response_data.customer_code)
            Else
                customerCode = " Trail "
            End If
        Else
            customerCode = " Trail "
        End If
 
        ' 👉 Title me Customer Code [ ] me show
        Me.Text = "[Aadhat #" & customerCode & "] " & version.ToString() & " [" & CDate(fecha).ToString("yyMMddhhmm") & "]"
        rs.FindAllControls(Me)
        CompanyList.MdiParent = Me
        CompanyList.Show()

        Me.Top = 50 : Me.Left = 50
        Me.KeyPreview = True

        'Dim screenWidth As Integer = Screen.PrimaryScreen.WorkingArea.Width
        'Dim screenHeight As Integer = Screen.PrimaryScreen.WorkingArea.Height + 48
    End Sub
    Private Function Safe(s As String) As String
        If s Is Nothing Then Return ""
        Return s.Trim()
    End Function

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ShowCompanies_MaximizedBoundsChanged(sender As Object, e As EventArgs) Handles Me.MaximizedBoundsChanged
        Dim screenWidth As Integer = Screen.PrimaryScreen.WorkingArea.Width
    End Sub
    Private Sub ShowCompanies_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        rs.ResizeAllControls(Me)
    End Sub
    Private Sub CreateCompanyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateCompanyToolStripMenuItem.Click
        TermsAndConditions.MdiParent = Me
        TermsAndConditions.Show()
        TermsAndConditions.BringToFront()
    End Sub

    Private Sub ModifyCompanyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifyCompanyToolStripMenuItem.Click
        Create_Company.MdiParent = Me
        Create_Company.Show()
        If CompanyList.dg1.SelectedRows.Count = 0 Then Exit Sub
        Dim tmpid As Integer = Val(CompanyList.dg1.SelectedRows(0).Cells(0).Value)
        Create_Company.FillContros(tmpid)
        Create_Company.BringToFront()
    End Sub

    Private Sub ChangeFinacialYearToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeFinacialYearToolStripMenuItem.Click
        Change_Financial_Year.MdiParent = Me
        Change_Financial_Year.Show()
        Change_Financial_Year.BringToFront()

    End Sub

    Private Sub DeleteCompanyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteCompanyToolStripMenuItem.Click
        Delete()
    End Sub
    Private Sub Delete()
        'Dim marka As String = "Speed Sale"
        Try
            If MessageBox.Show(" Are You Sure want to delete comapny it can't be Reverse ??", "Delete Company At Own Risk", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                If clsFun.ExecNonQuery("DELETE from Company WHERE ID=" & CompanyList.dg1.SelectedRows(0).Cells(0).Value & "") > 0 Then

                End If
                MsgBox("Company Deleted Successfully.", vbInformation + vbOKOnly, "Deleted")
                ' CompanyList.retrive()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub SystemInfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SystemInfoToolStripMenuItem.Click
        SystemInfo.MdiParent = Me
        SystemInfo.Show()
        SystemInfo.BringToFront()
    End Sub

    Private Sub SystemInfoToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles SystemInfoToolStripMenuItem1.Click
        LicenceInfo.MdiParent = Me
        LicenceInfo.Show()
        LicenceInfo.BringToFront()
    End Sub

    Private Sub RunSqliteQueryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunSqliteQueryToolStripMenuItem.Click
        Query_Maker.MdiParent = Me
        Query_Maker.Show()
        Query_Maker.BringToFront()
    End Sub

    Private Sub UpdaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdaToolStripMenuItem.Click
        frmUpdater.MdiParent = Me
        frmUpdater.Show()
        frmUpdater.BringToFront()
    End Sub
End Class
