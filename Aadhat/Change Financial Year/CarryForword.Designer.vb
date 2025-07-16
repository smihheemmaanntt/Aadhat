<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CarryForword
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CarryForword))
        Me.lblAcBalance = New System.Windows.Forms.Label()
        Me.btnChangeYear = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'lblAcBalance
        '
        Me.lblAcBalance.AutoSize = True
        Me.lblAcBalance.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblAcBalance.ForeColor = System.Drawing.Color.Navy
        Me.lblAcBalance.Location = New System.Drawing.Point(50, 24)
        Me.lblAcBalance.Name = "lblAcBalance"
        Me.lblAcBalance.Size = New System.Drawing.Size(562, 19)
        Me.lblAcBalance.TabIndex = 40246
        Me.lblAcBalance.Text = "Do You want to Carry Forword Masters and Balances From Previous Fiancial Year?"
        '
        'btnChangeYear
        '
        Me.btnChangeYear.BackColor = System.Drawing.Color.MediumAquamarine
        Me.btnChangeYear.FlatAppearance.BorderSize = 0
        Me.btnChangeYear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnChangeYear.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.btnChangeYear.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnChangeYear.Image = CType(resources.GetObject("btnChangeYear.Image"), System.Drawing.Image)
        Me.btnChangeYear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeYear.Location = New System.Drawing.Point(351, 115)
        Me.btnChangeYear.Name = "btnChangeYear"
        Me.btnChangeYear.Size = New System.Drawing.Size(77, 49)
        Me.btnChangeYear.TabIndex = 40247
        Me.btnChangeYear.Text = "Yes"
        Me.btnChangeYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnChangeYear.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.OrangeRed
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.Button1.ForeColor = System.Drawing.Color.GhostWhite
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(428, 115)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(77, 49)
        Me.Button1.TabIndex = 40248
        Me.Button1.Text = "No"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Gray
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.Button2.ForeColor = System.Drawing.Color.GhostWhite
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(505, 115)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(107, 49)
        Me.Button2.TabIndex = 40249
        Me.Button2.Text = "Cancel"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(54, 64)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(558, 23)
        Me.ProgressBar1.TabIndex = 40250
        Me.ProgressBar1.Visible = False
        '
        'CarryForword
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(642, 187)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnChangeYear)
        Me.Controls.Add(Me.lblAcBalance)
        Me.Name = "CarryForword"
        Me.Text = "Carry Forword"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblAcBalance As System.Windows.Forms.Label
    Friend WithEvents btnChangeYear As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
End Class
