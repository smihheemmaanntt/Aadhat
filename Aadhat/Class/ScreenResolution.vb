'Option Strict Off
'Option Explicit On
'Module ScreenResolution

'    Public Const EWX_LOGOFF As Short = 0
'    Public Const EWX_SHUTDOWN As Short = 1
'    Public Const EWX_REBOOT As Short = 2
'    Public Const EWX_FORCE As Short = 4
'    Public Const CCDEVICENAME As Short = 32
'    Public Const CCFORMNAME As Short = 32
'    Public Const DM_BITSPERPEL As Integer = &H40000
'    Public Const DM_PELSWIDTH As Integer = &H80000
'    Public Const DM_PELSHEIGHT As Integer = &H100000
'    Public Const CDS_UPDATEREGISTRY As Short = &H1S
'    Public Const CDS_TEST As Short = &H4S
'    Public Const DISP_CHANGE_SUCCESSFUL As Short = 0
'    Public Const DISP_CHANGE_RESTART As Short = 1

'    Structure DEVMODE
'        <VBFixedString(CCDEVICENAME), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=CCDEVICENAME)> Public dmDeviceName As String
'        Dim dmSpecVersion As Short
'        Dim dmDriverVersion As Short
'        Dim dmSize As Short
'        Dim dmDriverExtra As Short
'        Dim dmFields As Integer
'        Dim dmOrientation As Short
'        Dim dmPaperSize As Short
'        Dim dmPaperLength As Short
'        Dim dmPaperWidth As Short
'        Dim dmScale As Short
'        Dim dmCopies As Short
'        Dim dmDefaultSource As Short
'        Dim dmPrintQuality As Short
'        Dim dmColor As Short
'        Dim dmDuplex As Short
'        Dim dmYResolution As Short
'        Dim dmTTOption As Short
'        Dim dmCollate As Short
'        <VBFixedString(CCFORMNAME), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=CCFORMNAME)> Public dmFormName As String
'        Dim dmUnusedPadding As Short
'        Dim dmBitsPerPel As Short
'        Dim dmPelsWidth As Integer
'        Dim dmPelsHeight As Integer
'        Dim dmDisplayFlags As Integer
'        Dim dmDisplayFrequency As Integer
'    End Structure
'    Declare Function EnumDisplaySettings Lib "user32" Alias "EnumDisplaySettingsA" (ByVal lpszDeviceName As Integer, ByVal iModeNum As Integer, ByRef lpDevMode As DEVMODE) As Boolean
'    Declare Function ChangeDisplaySettings Lib "user32" Alias "ChangeDisplaySettingsA" (ByRef lpDevMode As DEVMODE, ByVal dwFlags As Integer) As Integer
'    Declare Function ExitWindowsEx Lib "user32" (ByVal uFlags As Integer, ByVal dwReserved As Integer) As Integer
'    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'    'And the form code is 
'Option Strict Off
'Option Explicit On 
'    Friend Class Form1
'        Inherits System.Windows.Forms.Form
'#Region "Windows Form Designer generated code "
'        Public Sub New()
'            MyBase.New()

'            'This call is required by the Windows Form Designer.
'            InitializeComponent()
'        End Sub
'        'Form overrides dispose to clean up the component list.
'        Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
'            If Disposing Then
'                If Not components Is Nothing Then
'                    components.Dispose()
'                End If
'            End If
'            MyBase.Dispose(Disposing)
'        End Sub
'        'Required by the Windows Form Designer
'        Private components As System.ComponentModel.IContainer
'        Public ToolTip1 As System.Windows.Forms.ToolTip
'        Public WithEvents _optRes_2 As System.Windows.Forms.RadioButton
'        Public WithEvents _optRes_1 As System.Windows.Forms.RadioButton
'        Public WithEvents _optRes_0 As System.Windows.Forms.RadioButton
'        Public WithEvents cmdChange As System.Windows.Forms.Button
'        Public WithEvents optRes As Microsoft.VisualBasic.Compatibility.Vb6.RadioButtonArray
'        Public WithEvents optShut As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
'        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
'            Me.components = New System.ComponentModel.Container
'            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
'            Me._optRes_2 = New System.Windows.Forms.RadioButton
'            Me._optRes_1 = New System.Windows.Forms.RadioButton
'            Me._optRes_0 = New System.Windows.Forms.RadioButton
'            Me.cmdChange = New System.Windows.Forms.Button
'            Me.optRes = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
'            Me.optShut = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
'            CType(Me.optRes, System.ComponentModel.ISupportInitialize).BeginInit()
'            CType(Me.optShut, System.ComponentModel.ISupportInitialize).BeginInit()
'            Me.SuspendLayout()
'            '
'            '_optRes_2
'            '
'            Me._optRes_2.BackColor = System.Drawing.SystemColors.Control
'            Me._optRes_2.Cursor = System.Windows.Forms.Cursors.Default
'            Me._optRes_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'            Me._optRes_2.ForeColor = System.Drawing.SystemColors.ControlText
'            Me.optRes.SetIndex(Me._optRes_2, CType(2, Short))
'            Me._optRes_2.Location = New System.Drawing.Point(53, 64)
'            Me._optRes_2.Name = "_optRes_2"
'            Me._optRes_2.RightToLeft = System.Windows.Forms.RightToLeft.No
'            Me._optRes_2.Size = New System.Drawing.Size(81, 13)
'            Me._optRes_2.TabIndex = 4
'            Me._optRes_2.TabStop = True
'            Me._optRes_2.Text = "1024 x 768"
'            '
'            '_optRes_1
'            '
'            Me._optRes_1.BackColor = System.Drawing.SystemColors.Control
'            Me._optRes_1.Cursor = System.Windows.Forms.Cursors.Default
'            Me._optRes_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'            Me._optRes_1.ForeColor = System.Drawing.SystemColors.ControlText
'            Me.optRes.SetIndex(Me._optRes_1, CType(1, Short))
'            Me._optRes_1.Location = New System.Drawing.Point(53, 40)
'            Me._optRes_1.Name = "_optRes_1"
'            Me._optRes_1.RightToLeft = System.Windows.Forms.RightToLeft.No
'            Me._optRes_1.Size = New System.Drawing.Size(89, 13)
'            Me._optRes_1.TabIndex = 3
'            Me._optRes_1.TabStop = True
'            Me._optRes_1.Text = "800 x 600"
'            '
'            '_optRes_0
'            '
'            Me._optRes_0.BackColor = System.Drawing.SystemColors.Control
'            Me._optRes_0.Cursor = System.Windows.Forms.Cursors.Default
'            Me._optRes_0.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'            Me._optRes_0.ForeColor = System.Drawing.SystemColors.ControlText
'            Me.optRes.SetIndex(Me._optRes_0, CType(0, Short))
'            Me._optRes_0.Location = New System.Drawing.Point(53, 16)
'            Me._optRes_0.Name = "_optRes_0"
'            Me._optRes_0.RightToLeft = System.Windows.Forms.RightToLeft.No
'            Me._optRes_0.Size = New System.Drawing.Size(81, 13)
'            Me._optRes_0.TabIndex = 2
'            Me._optRes_0.TabStop = True
'            Me._optRes_0.Text = "640 x 480"
'            '
'            'cmdChange
'            '
'            Me.cmdChange.BackColor = System.Drawing.SystemColors.Control
'            Me.cmdChange.Cursor = System.Windows.Forms.Cursors.Default
'            Me.cmdChange.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'            Me.cmdChange.ForeColor = System.Drawing.SystemColors.ControlText
'            Me.cmdChange.Location = New System.Drawing.Point(53, 87)
'            Me.cmdChange.Name = "cmdChange"
'            Me.cmdChange.RightToLeft = System.Windows.Forms.RightToLeft.No
'            Me.cmdChange.Size = New System.Drawing.Size(113, 25)
'            Me.cmdChange.TabIndex = 0
'            Me.cmdChange.Text = "&Change Resolution"
'            '
'            'Form1
'            '
'            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
'            Me.BackColor = System.Drawing.SystemColors.Control
'            Me.ClientSize = New System.Drawing.Size(241, 125)
'            Me.Controls.Add(Me._optRes_2)
'            Me.Controls.Add(Me._optRes_1)
'            Me.Controls.Add(Me._optRes_0)
'            Me.Controls.Add(Me.cmdChange)
'            Me.Cursor = System.Windows.Forms.Cursors.Default
'            Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
'            Me.Location = New System.Drawing.Point(355, 151)
'            Me.MaximizeBox = False
'            Me.MinimizeBox = False
'            Me.Name = "Form1"
'            Me.RightToLeft = System.Windows.Forms.RightToLeft.No
'            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
'            Me.Text = "Change the Screen Resolution"
'            CType(Me.optRes, System.ComponentModel.ISupportInitialize).EndInit()
'            CType(Me.optShut, System.ComponentModel.ISupportInitialize).EndInit()
'            Me.ResumeLayout(False)
'        End Sub
'#End Region
'        Private Sub cmdChange_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChange.Click
'            Dim x As DEVMODE
'            Dim y As Integer
'            Dim z As Short

'            y = EnumDisplaySettings(0, 0, x)
'            With x
'                .dmFields = DM_PELSWIDTH Or DM_PELSHEIGHT
'                If optRes(0).Checked Then
'                    .dmPelsWidth = 640
'                    .dmPelsHeight = 480
'                ElseIf optRes(1).Checked Then
'                    .dmPelsWidth = 800
'                    .dmPelsHeight = 600
'                Else
'                    .dmPelsWidth = 1024
'                    .dmPelsHeight = 768
'                End If

'            End With

'            y = ChangeDisplaySettings(x, CDS_TEST)

'            Select Case y
'                Case DISP_CHANGE_RESTART
'                    z = MsgBox("You must restart your computer to apply these changes." & vbCrLf & vbCrLf & "Do you want to restart now?", MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, "Screen Resolution")
'                    If z = MsgBoxResult.Yes Then Call ExitWindowsEx(EWX_REBOOT, 0)
'                Case DISP_CHANGE_SUCCESSFUL
'                    Call ChangeDisplaySettings(x, CDS_UPDATEREGISTRY)
'                    MsgBox("Screen resolution changed", MsgBoxStyle.Information, "Resolution Changed")
'                Case Else
'                    MsgBox("Mode not supported", MsgBoxStyle.SystemModal, "Error")
'            End Select
'        End Sub
'    End Class
'End Module
