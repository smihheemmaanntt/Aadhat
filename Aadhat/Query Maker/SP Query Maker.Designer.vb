<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SP_Query_Maker
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
        Me.RtxtQuery = New System.Windows.Forms.RichTextBox()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.pnlLock = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.pnlLock.SuspendLayout()
        Me.SuspendLayout()
        '
        'RtxtQuery
        '
        Me.RtxtQuery.Location = New System.Drawing.Point(36, 27)
        Me.RtxtQuery.Name = "RtxtQuery"
        Me.RtxtQuery.Size = New System.Drawing.Size(509, 299)
        Me.RtxtQuery.TabIndex = 2
        Me.RtxtQuery.Text = ""
        '
        'BtnSave
        '
        Me.BtnSave.BackColor = System.Drawing.Color.Teal
        Me.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSave.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSave.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnSave.Location = New System.Drawing.Point(390, 332)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(155, 37)
        Me.BtnSave.TabIndex = 3
        Me.BtnSave.Text = "&Run Query"
        Me.BtnSave.UseVisualStyleBackColor = False
        '
        'pnlLock
        '
        Me.pnlLock.Controls.Add(Me.Label3)
        Me.pnlLock.Controls.Add(Me.txtPassword)
        Me.pnlLock.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlLock.Location = New System.Drawing.Point(0, 0)
        Me.pnlLock.Name = "pnlLock"
        Me.pnlLock.Size = New System.Drawing.Size(631, 408)
        Me.pnlLock.TabIndex = 0
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Teal
        Me.Label3.Location = New System.Drawing.Point(80, 194)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(146, 19)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Authorised Password :"
        '
        'txtPassword
        '
        Me.txtPassword.BackColor = System.Drawing.Color.GhostWhite
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPassword.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtPassword.ForeColor = System.Drawing.Color.Black
        Me.txtPassword.Location = New System.Drawing.Point(232, 191)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(273, 26)
        Me.txtPassword.TabIndex = 0
        Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Query_Maker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(631, 408)
        Me.Controls.Add(Me.pnlLock)
        Me.Controls.Add(Me.RtxtQuery)
        Me.Controls.Add(Me.BtnSave)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Query_Maker"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Query Maker"
        Me.pnlLock.ResumeLayout(False)
        Me.pnlLock.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RtxtQuery As System.Windows.Forms.RichTextBox
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents pnlLock As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
End Class
