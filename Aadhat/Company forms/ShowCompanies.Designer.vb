<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShowCompanies
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ShowCompanies))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FinacleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateCompanyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModifyCompanyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeFinacialYearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteCompanyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SystemInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SystemInfoToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.RunSqliteQueryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RemoteSupportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 587)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1095, 22)
        Me.StatusStrip1.TabIndex = 7
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 24)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1095, 25)
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FinacleToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1095, 24)
        Me.MenuStrip1.TabIndex = 5
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FinacleToolStripMenuItem
        '
        Me.FinacleToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CreateCompanyToolStripMenuItem, Me.ModifyCompanyToolStripMenuItem, Me.ChangeFinacialYearToolStripMenuItem, Me.DeleteCompanyToolStripMenuItem, Me.BackupToolStripMenuItem, Me.RestoreToolStripMenuItem, Me.SystemInfoToolStripMenuItem, Me.SystemInfoToolStripMenuItem1, Me.RunSqliteQueryToolStripMenuItem, Me.RemoteSupportToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.FinacleToolStripMenuItem.Name = "FinacleToolStripMenuItem"
        Me.FinacleToolStripMenuItem.Size = New System.Drawing.Size(71, 20)
        Me.FinacleToolStripMenuItem.Text = "&Company"
        '
        'CreateCompanyToolStripMenuItem
        '
        Me.CreateCompanyToolStripMenuItem.Name = "CreateCompanyToolStripMenuItem"
        Me.CreateCompanyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3
        Me.CreateCompanyToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.CreateCompanyToolStripMenuItem.Text = "Create &Company"
        '
        'ModifyCompanyToolStripMenuItem
        '
        Me.ModifyCompanyToolStripMenuItem.Name = "ModifyCompanyToolStripMenuItem"
        Me.ModifyCompanyToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.ModifyCompanyToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ModifyCompanyToolStripMenuItem.Text = "Modify Company"
        '
        'ChangeFinacialYearToolStripMenuItem
        '
        Me.ChangeFinacialYearToolStripMenuItem.Name = "ChangeFinacialYearToolStripMenuItem"
        Me.ChangeFinacialYearToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10
        Me.ChangeFinacialYearToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ChangeFinacialYearToolStripMenuItem.Text = "Change Finacial Year"
        '
        'DeleteCompanyToolStripMenuItem
        '
        Me.DeleteCompanyToolStripMenuItem.Name = "DeleteCompanyToolStripMenuItem"
        Me.DeleteCompanyToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.DeleteCompanyToolStripMenuItem.Text = "Delete Company"
        '
        'BackupToolStripMenuItem
        '
        Me.BackupToolStripMenuItem.Name = "BackupToolStripMenuItem"
        Me.BackupToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.BackupToolStripMenuItem.Text = "Backup"
        '
        'RestoreToolStripMenuItem
        '
        Me.RestoreToolStripMenuItem.Name = "RestoreToolStripMenuItem"
        Me.RestoreToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.RestoreToolStripMenuItem.Text = "Restore"
        '
        'SystemInfoToolStripMenuItem
        '
        Me.SystemInfoToolStripMenuItem.Name = "SystemInfoToolStripMenuItem"
        Me.SystemInfoToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.SystemInfoToolStripMenuItem.Text = "System info"
        '
        'SystemInfoToolStripMenuItem1
        '
        Me.SystemInfoToolStripMenuItem1.Name = "SystemInfoToolStripMenuItem1"
        Me.SystemInfoToolStripMenuItem1.Size = New System.Drawing.Size(208, 22)
        Me.SystemInfoToolStripMenuItem1.Text = "Licence Info"
        '
        'RunSqliteQueryToolStripMenuItem
        '
        Me.RunSqliteQueryToolStripMenuItem.Name = "RunSqliteQueryToolStripMenuItem"
        Me.RunSqliteQueryToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.RunSqliteQueryToolStripMenuItem.Text = "Run Sqlite Query"
        '
        'RemoteSupportToolStripMenuItem
        '
        Me.RemoteSupportToolStripMenuItem.Name = "RemoteSupportToolStripMenuItem"
        Me.RemoteSupportToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F11
        Me.RemoteSupportToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.RemoteSupportToolStripMenuItem.Text = "Remote Support"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(208, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'ShowCompanies
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1095, 609)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Name = "ShowCompanies"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TransparencyKey = System.Drawing.Color.Teal
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FinacleToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreateCompanyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModifyCompanyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents BackupToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RestoreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeFinacialYearToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteCompanyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoteSupportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SystemInfoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SystemInfoToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RunSqliteQueryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
