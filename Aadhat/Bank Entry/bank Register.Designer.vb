<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class bank_Register
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bank_Register))
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.TxtGrandTotal = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtTotDisCount = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtTotBasic = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.dtp1 = New System.Windows.Forms.DateTimePicker()
        Me.dtp2 = New System.Windows.Forms.DateTimePicker()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txttoDate = New System.Windows.Forms.TextBox()
        Me.txtFromDate = New System.Windows.Forms.TextBox()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(1143, 0)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(53, 49)
        Me.btnClose.TabIndex = 91115
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(384, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(444, 48)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "BANK ENTRY REGISTER"
        '
        'btnShow
        '
        Me.btnShow.BackColor = System.Drawing.Color.DarkTurquoise
        Me.btnShow.FlatAppearance.BorderSize = 0
        Me.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShow.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.btnShow.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnShow.Image = Global.Aadhat.My.Resources.Resources.Save
        Me.btnShow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShow.Location = New System.Drawing.Point(439, 92)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(93, 30)
        Me.btnShow.TabIndex = 2
        Me.btnShow.Text = "&Show"
        Me.btnShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnShow.UseVisualStyleBackColor = False
        '
        'TxtGrandTotal
        '
        Me.TxtGrandTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.TxtGrandTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtGrandTotal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TxtGrandTotal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TxtGrandTotal.ForeColor = System.Drawing.Color.Red
        Me.TxtGrandTotal.Location = New System.Drawing.Point(1064, 616)
        Me.TxtGrandTotal.Name = "TxtGrandTotal"
        Me.TxtGrandTotal.ReadOnly = True
        Me.TxtGrandTotal.Size = New System.Drawing.Size(120, 26)
        Me.TxtGrandTotal.TabIndex = 40185
        Me.TxtGrandTotal.TabStop = False
        Me.TxtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.CadetBlue
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(1011, 617)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(55, 25)
        Me.Label5.TabIndex = 40184
        Me.Label5.Text = "Total :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTotDisCount
        '
        Me.txtTotDisCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotDisCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotDisCount.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotDisCount.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotDisCount.ForeColor = System.Drawing.Color.Red
        Me.txtTotDisCount.Location = New System.Drawing.Point(900, 616)
        Me.txtTotDisCount.Name = "txtTotDisCount"
        Me.txtTotDisCount.ReadOnly = True
        Me.txtTotDisCount.Size = New System.Drawing.Size(111, 26)
        Me.txtTotDisCount.TabIndex = 40183
        Me.txtTotDisCount.TabStop = False
        Me.txtTotDisCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.CadetBlue
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(831, 616)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 26)
        Me.Label4.TabIndex = 40182
        Me.Label4.Text = "Discount :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtTotBasic
        '
        Me.txtTotBasic.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotBasic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotBasic.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTotBasic.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotBasic.ForeColor = System.Drawing.Color.Red
        Me.txtTotBasic.Location = New System.Drawing.Point(715, 616)
        Me.txtTotBasic.Name = "txtTotBasic"
        Me.txtTotBasic.ReadOnly = True
        Me.txtTotBasic.Size = New System.Drawing.Size(116, 26)
        Me.txtTotBasic.TabIndex = 40181
        Me.txtTotBasic.TabStop = False
        Me.txtTotBasic.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.CadetBlue
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Location = New System.Drawing.Point(647, 616)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(68, 26)
        Me.Label3.TabIndex = 40180
        Me.Label3.Text = "Amount :"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.dg1.ColumnHeadersHeight = 25
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.DarkTurquoise
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle8
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Crimson
        Me.dg1.Location = New System.Drawing.Point(12, 119)
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dg1.RowHeadersVisible = False
        Me.dg1.RowHeadersWidth = 42
        Me.dg1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle9
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1172, 498)
        Me.dg1.TabIndex = 4
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(328, 7)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 91139
        Me.PictureBox2.TabStop = False
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.Coral
        Me.btnPrint.FlatAppearance.BorderSize = 0
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.btnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(529, 92)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(100, 30)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.TabStop = False
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'dtp1
        '
        Me.dtp1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp1.Location = New System.Drawing.Point(126, 93)
        Me.dtp1.Name = "dtp1"
        Me.dtp1.Size = New System.Drawing.Size(102, 26)
        Me.dtp1.TabIndex = 5
        '
        'dtp2
        '
        Me.dtp2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dtp2.Location = New System.Drawing.Point(337, 93)
        Me.dtp2.Name = "dtp2"
        Me.dtp2.Size = New System.Drawing.Size(102, 26)
        Me.dtp2.TabIndex = 6
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label9.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label9.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label9.Location = New System.Drawing.Point(228, 93)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(100, 27)
        Me.Label9.TabIndex = 8
        Me.Label9.Text = "To Date :"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label2.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label2.Location = New System.Drawing.Point(12, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(101, 27)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "From Date :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txttoDate
        '
        Me.txttoDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txttoDate.Font = New System.Drawing.Font("Times New Roman", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txttoDate.Location = New System.Drawing.Point(328, 93)
        Me.txttoDate.Name = "txttoDate"
        Me.txttoDate.Size = New System.Drawing.Size(95, 27)
        Me.txttoDate.TabIndex = 1
        '
        'txtFromDate
        '
        Me.txtFromDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFromDate.Font = New System.Drawing.Font("Times New Roman", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFromDate.Location = New System.Drawing.Point(113, 93)
        Me.txtFromDate.Name = "txtFromDate"
        Me.txtFromDate.Size = New System.Drawing.Size(100, 27)
        Me.txtFromDate.TabIndex = 0
        '
        'bank_Register
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.txttoDate)
        Me.Controls.Add(Me.txtFromDate)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtGrandTotal)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtTotDisCount)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtTotBasic)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.dtp2)
        Me.Controls.Add(Me.dtp1)
        Me.Name = "bank_Register"
        Me.Text = "Bank Entry"
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents TxtGrandTotal As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtTotDisCount As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtTotBasic As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents dtp1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtp2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txttoDate As System.Windows.Forms.TextBox
    Friend WithEvents txtFromDate As System.Windows.Forms.TextBox
End Class
