<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Merge_Accounts
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.cbSelectAccount = New System.Windows.Forms.ComboBox()
        Me.cbDestination = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(48, 112)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(132, 21)
        Me.Label4.TabIndex = 40112
        Me.Label4.Text = "Select Account"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(48, 165)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(173, 21)
        Me.Label1.TabIndex = 40115
        Me.Label1.Text = "Destination Account"
        '
        'BtnSave
        '
        Me.BtnSave.BackColor = System.Drawing.Color.DarkTurquoise
        Me.BtnSave.FlatAppearance.BorderSize = 0
        Me.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSave.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BtnSave.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnSave.Image = Global.Aadhat.My.Resources.Resources.Save
        Me.BtnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnSave.Location = New System.Drawing.Point(508, 221)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(105, 38)
        Me.BtnSave.TabIndex = 40117
        Me.BtnSave.TabStop = False
        Me.BtnSave.Text = "&Save"
        Me.BtnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnSave.UseVisualStyleBackColor = False
        '
        'cbSelectAccount
        '
        Me.cbSelectAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbSelectAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbSelectAccount.BackColor = System.Drawing.Color.GhostWhite
        Me.cbSelectAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSelectAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbSelectAccount.Font = New System.Drawing.Font("Times New Roman", 11.0!)
        Me.cbSelectAccount.ForeColor = System.Drawing.Color.Black
        Me.cbSelectAccount.FormattingEnabled = True
        Me.cbSelectAccount.Items.AddRange(New Object() {"Nug", "Kg", "40 Kg"})
        Me.cbSelectAccount.Location = New System.Drawing.Point(52, 136)
        Me.cbSelectAccount.Name = "cbSelectAccount"
        Me.cbSelectAccount.Size = New System.Drawing.Size(561, 26)
        Me.cbSelectAccount.TabIndex = 40118
        '
        'cbDestination
        '
        Me.cbDestination.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbDestination.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbDestination.BackColor = System.Drawing.Color.GhostWhite
        Me.cbDestination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDestination.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbDestination.Font = New System.Drawing.Font("Times New Roman", 11.0!)
        Me.cbDestination.ForeColor = System.Drawing.Color.Black
        Me.cbDestination.FormattingEnabled = True
        Me.cbDestination.Items.AddRange(New Object() {"Nug", "Kg", "40 Kg"})
        Me.cbDestination.Location = New System.Drawing.Point(52, 189)
        Me.cbDestination.Name = "cbDestination"
        Me.cbDestination.Size = New System.Drawing.Size(561, 26)
        Me.cbDestination.TabIndex = 40119
        '
        'Merge_Accounts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(766, 451)
        Me.Controls.Add(Me.cbDestination)
        Me.Controls.Add(Me.cbSelectAccount)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Name = "Merge_Accounts"
        Me.Text = "Merge_Accounts"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents cbSelectAccount As System.Windows.Forms.ComboBox
    Friend WithEvents cbDestination As System.Windows.Forms.ComboBox
End Class
