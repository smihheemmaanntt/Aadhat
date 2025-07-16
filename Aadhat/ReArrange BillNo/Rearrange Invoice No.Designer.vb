<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Rearrange_Invoice_No
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
        Me.Cbper = New System.Windows.Forms.ComboBox()
        Me.Pb1 = New System.Windows.Forms.ProgressBar()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.RadioInvoiceID = New System.Windows.Forms.RadioButton()
        Me.RadioBillNo = New System.Windows.Forms.RadioButton()
        Me.RadioBoth = New System.Windows.Forms.RadioButton()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.RadioAccountID = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'Cbper
        '
        Me.Cbper.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.Cbper.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.Cbper.BackColor = System.Drawing.Color.AliceBlue
        Me.Cbper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cbper.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Cbper.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Cbper.FormattingEnabled = True
        Me.Cbper.Items.AddRange(New Object() {"Nug", "Kg", "40 Kg"})
        Me.Cbper.Location = New System.Drawing.Point(174, 65)
        Me.Cbper.Name = "Cbper"
        Me.Cbper.Size = New System.Drawing.Size(269, 29)
        Me.Cbper.TabIndex = 4
        '
        'Pb1
        '
        Me.Pb1.Location = New System.Drawing.Point(16, 135)
        Me.Pb1.Name = "Pb1"
        Me.Pb1.Size = New System.Drawing.Size(427, 23)
        Me.Pb1.TabIndex = 5
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.Navy
        Me.btnShow.FlatAppearance.BorderSize = 0
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnShow.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnShow.Location = New System.Drawing.Point(291, 173)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(152, 30)
        Me.btnShow.TabIndex = 6
        Me.btnShow.Text = "&Rearrange"
        Me.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'RadioInvoiceID
        '
        Me.RadioInvoiceID.AutoSize = True
        Me.RadioInvoiceID.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.RadioInvoiceID.ForeColor = System.Drawing.Color.Black
        Me.RadioInvoiceID.Location = New System.Drawing.Point(18, 108)
        Me.RadioInvoiceID.Name = "RadioInvoiceID"
        Me.RadioInvoiceID.Size = New System.Drawing.Size(90, 21)
        Me.RadioInvoiceID.TabIndex = 7
        Me.RadioInvoiceID.TabStop = True
        Me.RadioInvoiceID.Text = "Invoice ID"
        Me.RadioInvoiceID.UseVisualStyleBackColor = True
        '
        'RadioBillNo
        '
        Me.RadioBillNo.AutoSize = True
        Me.RadioBillNo.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.RadioBillNo.ForeColor = System.Drawing.Color.Black
        Me.RadioBillNo.Location = New System.Drawing.Point(135, 108)
        Me.RadioBillNo.Name = "RadioBillNo"
        Me.RadioBillNo.Size = New System.Drawing.Size(65, 21)
        Me.RadioBillNo.TabIndex = 8
        Me.RadioBillNo.TabStop = True
        Me.RadioBillNo.Text = "Bill No"
        Me.RadioBillNo.UseVisualStyleBackColor = True
        '
        'RadioBoth
        '
        Me.RadioBoth.AutoSize = True
        Me.RadioBoth.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.RadioBoth.ForeColor = System.Drawing.Color.Black
        Me.RadioBoth.Location = New System.Drawing.Point(219, 108)
        Me.RadioBoth.Name = "RadioBoth"
        Me.RadioBoth.Size = New System.Drawing.Size(205, 21)
        Me.RadioBoth.TabIndex = 9
        Me.RadioBoth.TabStop = True
        Me.RadioBoth.Text = "Both (Invoice ID and Bill No)"
        Me.RadioBoth.UseVisualStyleBackColor = True
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label24.ForeColor = System.Drawing.Color.Black
        Me.Label24.Location = New System.Drawing.Point(12, 68)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(150, 21)
        Me.Label24.TabIndex = 30
        Me.Label24.Text = "Transaction Type :"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Bodoni Bk BT", 20.0!)
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(67, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(317, 32)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "RESET INVOICE NUMBER"
        '
        'RadioAccountID
        '
        Me.RadioAccountID.AutoSize = True
        Me.RadioAccountID.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.RadioAccountID.ForeColor = System.Drawing.Color.Black
        Me.RadioAccountID.Location = New System.Drawing.Point(18, 180)
        Me.RadioAccountID.Name = "RadioAccountID"
        Me.RadioAccountID.Size = New System.Drawing.Size(99, 21)
        Me.RadioAccountID.TabIndex = 32
        Me.RadioAccountID.TabStop = True
        Me.RadioAccountID.Text = "AccountIDs"
        Me.RadioAccountID.UseVisualStyleBackColor = True
        Me.RadioAccountID.Visible = False
        '
        'Rearrange_Invoice_No
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(455, 226)
        Me.Controls.Add(Me.RadioAccountID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.RadioBoth)
        Me.Controls.Add(Me.RadioBillNo)
        Me.Controls.Add(Me.RadioInvoiceID)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.Pb1)
        Me.Controls.Add(Me.Cbper)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Rearrange_Invoice_No"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reset Invoice No"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Cbper As System.Windows.Forms.ComboBox
    Friend WithEvents Pb1 As System.Windows.Forms.ProgressBar
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents RadioInvoiceID As System.Windows.Forms.RadioButton
    Friend WithEvents RadioBillNo As System.Windows.Forms.RadioButton
    Friend WithEvents RadioBoth As System.Windows.Forms.RadioButton
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents RadioAccountID As System.Windows.Forms.RadioButton
End Class
