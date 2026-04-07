<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Crate_Balance_Editor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Crate_Balance_Editor))
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtcrAmt = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtDramt = New System.Windows.Forms.TextBox()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.lblCrateBalance = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.txtSearcGroup = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.Pnlconfirmation = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.txtUpdate = New System.Windows.Forms.TextBox()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.lbllinkedfy = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblLinkedName = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Pnlconfirmation.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.btnClose.Size = New System.Drawing.Size(53, 47)
        Me.btnClose.TabIndex = 91117
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(977, 594)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 19)
        Me.Label8.TabIndex = 40231
        Me.Label8.Text = "Credit :"
        '
        'txtcrAmt
        '
        Me.txtcrAmt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtcrAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtcrAmt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtcrAmt.ForeColor = System.Drawing.Color.Red
        Me.txtcrAmt.Location = New System.Drawing.Point(1050, 590)
        Me.txtcrAmt.Name = "txtcrAmt"
        Me.txtcrAmt.ReadOnly = True
        Me.txtcrAmt.Size = New System.Drawing.Size(123, 26)
        Me.txtcrAmt.TabIndex = 40230
        Me.txtcrAmt.TabStop = False
        Me.txtcrAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(781, 594)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(55, 19)
        Me.Label7.TabIndex = 40229
        Me.Label7.Text = "Debit :"
        '
        'txtDramt
        '
        Me.txtDramt.BackColor = System.Drawing.Color.GhostWhite
        Me.txtDramt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDramt.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtDramt.ForeColor = System.Drawing.Color.Red
        Me.txtDramt.Location = New System.Drawing.Point(848, 590)
        Me.txtDramt.Name = "txtDramt"
        Me.txtDramt.ReadOnly = True
        Me.txtDramt.Size = New System.Drawing.Size(123, 26)
        Me.txtDramt.TabIndex = 40228
        Me.txtDramt.TabStop = False
        Me.txtDramt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle25.BackColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle25.Font = New System.Drawing.Font("Times New Roman", 10.0!)
        DataGridViewCellStyle25.ForeColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle25.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle25.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle25.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle25
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle26.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle26.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle26.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle26.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle26.SelectionForeColor = System.Drawing.Color.Maroon
        DataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle26
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Gray
        Me.dg1.Location = New System.Drawing.Point(20, 126)
        Me.dg1.MultiSelect = False
        Me.dg1.Name = "dg1"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle27.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.RowHeadersDefaultCellStyle = DataGridViewCellStyle27
        Me.dg1.RowHeadersVisible = False
        DataGridViewCellStyle28.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle28
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dg1.Size = New System.Drawing.Size(1153, 465)
        Me.dg1.TabIndex = 40337
        '
        'lblCrateBalance
        '
        Me.lblCrateBalance.AutoSize = True
        Me.lblCrateBalance.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblCrateBalance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblCrateBalance.Location = New System.Drawing.Point(16, 595)
        Me.lblCrateBalance.Name = "lblCrateBalance"
        Me.lblCrateBalance.Size = New System.Drawing.Size(39, 19)
        Me.lblCrateBalance.TabIndex = 40338
        Me.lblCrateBalance.Text = "MSg"
        Me.lblCrateBalance.Visible = False
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.GhostWhite
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtSearch.ForeColor = System.Drawing.Color.Red
        Me.txtSearch.Location = New System.Drawing.Point(96, 101)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(374, 26)
        Me.txtSearch.TabIndex = 40339
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label42.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label42.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label42.Location = New System.Drawing.Point(20, 101)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(79, 27)
        Me.Label42.TabIndex = 40340
        Me.Label42.Text = " Name :"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSearcGroup
        '
        Me.txtSearcGroup.BackColor = System.Drawing.Color.GhostWhite
        Me.txtSearcGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearcGroup.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtSearcGroup.ForeColor = System.Drawing.Color.Red
        Me.txtSearcGroup.Location = New System.Drawing.Point(561, 101)
        Me.txtSearcGroup.Name = "txtSearcGroup"
        Me.txtSearcGroup.Size = New System.Drawing.Size(289, 26)
        Me.txtSearcGroup.TabIndex = 40342
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.DarkSlateGray
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label2.Location = New System.Drawing.Point(470, 101)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(91, 27)
        Me.Label2.TabIndex = 40343
        Me.Label2.Text = "Crate Name :"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ForeColor = System.Drawing.Color.Black
        Me.Label41.Location = New System.Drawing.Point(16, 26)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(461, 48)
        Me.Label41.TabIndex = 91118
        Me.Label41.Text = "OPENING CRATE EDITOR"
        '
        'btnApply
        '
        Me.btnApply.BackColor = System.Drawing.Color.MediumSeaGreen
        Me.btnApply.FlatAppearance.BorderSize = 0
        Me.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnApply.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnApply.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnApply.Image = CType(resources.GetObject("btnApply.Image"), System.Drawing.Image)
        Me.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnApply.Location = New System.Drawing.Point(856, 98)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(317, 28)
        Me.btnApply.TabIndex = 91119
        Me.btnApply.Text = "Bring Crate Balance From Another Company"
        Me.btnApply.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnApply.UseVisualStyleBackColor = False
        Me.btnApply.Visible = False
        '
        'Pnlconfirmation
        '
        Me.Pnlconfirmation.BackColor = System.Drawing.Color.Silver
        Me.Pnlconfirmation.Controls.Add(Me.Label4)
        Me.Pnlconfirmation.Controls.Add(Me.PictureBox4)
        Me.Pnlconfirmation.Controls.Add(Me.txtUpdate)
        Me.Pnlconfirmation.Controls.Add(Me.btnUpdate)
        Me.Pnlconfirmation.Location = New System.Drawing.Point(370, 221)
        Me.Pnlconfirmation.Name = "Pnlconfirmation"
        Me.Pnlconfirmation.Size = New System.Drawing.Size(457, 210)
        Me.Pnlconfirmation.TabIndex = 91146
        Me.Pnlconfirmation.Visible = False
        '
        'Label4
        '
        Me.Label4.AllowDrop = True
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(0, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(459, 19)
        Me.Label4.TabIndex = 91374
        Me.Label4.Text = "Type [SURE] For Update Crate Balances From Another Company ?"
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.Aadhat.My.Resources.Resources.Rupee
        Me.PictureBox4.Location = New System.Drawing.Point(14, 92)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(86, 86)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 91373
        Me.PictureBox4.TabStop = False
        '
        'txtUpdate
        '
        Me.txtUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.txtUpdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUpdate.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtUpdate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtUpdate.ForeColor = System.Drawing.Color.Black
        Me.txtUpdate.Location = New System.Drawing.Point(150, 92)
        Me.txtUpdate.Name = "txtUpdate"
        Me.txtUpdate.Size = New System.Drawing.Size(278, 26)
        Me.txtUpdate.TabIndex = 91372
        Me.txtUpdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnUpdate.FlatAppearance.BorderSize = 0
        Me.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnUpdate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnUpdate.ForeColor = System.Drawing.Color.White
        Me.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnUpdate.Location = New System.Drawing.Point(150, 125)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(278, 51)
        Me.btnUpdate.TabIndex = 91269
        Me.btnUpdate.TabStop = False
        Me.btnUpdate.Text = "&Update All Crate Balances"
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblStatus.ForeColor = System.Drawing.Color.Navy
        Me.lblStatus.Location = New System.Drawing.Point(173, 594)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(67, 19)
        Me.lblStatus.TabIndex = 91147
        Me.lblStatus.Text = "MsgBox"
        Me.lblStatus.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(384, 594)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(388, 26)
        Me.ProgressBar1.TabIndex = 91150
        '
        'txtPath
        '
        Me.txtPath.BackColor = System.Drawing.Color.GhostWhite
        Me.txtPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPath.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPath.ForeColor = System.Drawing.Color.Teal
        Me.txtPath.Location = New System.Drawing.Point(384, 593)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.ReadOnly = True
        Me.txtPath.Size = New System.Drawing.Size(388, 26)
        Me.txtPath.TabIndex = 91148
        '
        'lbllinkedfy
        '
        Me.lbllinkedfy.AutoSize = True
        Me.lbllinkedfy.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lbllinkedfy.ForeColor = System.Drawing.Color.Navy
        Me.lbllinkedfy.Location = New System.Drawing.Point(723, 51)
        Me.lbllinkedfy.Name = "lbllinkedfy"
        Me.lbllinkedfy.Size = New System.Drawing.Size(97, 19)
        Me.lbllinkedfy.TabIndex = 91154
        Me.lbllinkedfy.Text = "___________"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Navy
        Me.Label3.Location = New System.Drawing.Point(610, 51)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 19)
        Me.Label3.TabIndex = 91153
        Me.Label3.Text = "Financial Year :"
        '
        'lblLinkedName
        '
        Me.lblLinkedName.AutoSize = True
        Me.lblLinkedName.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblLinkedName.ForeColor = System.Drawing.Color.Navy
        Me.lblLinkedName.Location = New System.Drawing.Point(723, 28)
        Me.lblLinkedName.Name = "lblLinkedName"
        Me.lblLinkedName.Size = New System.Drawing.Size(97, 19)
        Me.lblLinkedName.TabIndex = 91152
        Me.lblLinkedName.Text = "___________"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(545, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(170, 19)
        Me.Label1.TabIndex = 91151
        Me.Label1.Text = "Linked Company Name :"
        '
        'Crate_Balance_Editor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.lbllinkedfy)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblLinkedName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.Pnlconfirmation)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.Label41)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.txtSearcGroup)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblCrateBalance)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtcrAmt)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtDramt)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label42)
        Me.Name = "Crate_Balance_Editor"
        Me.Text = "Account_Balance_Editor"
        CType(Me.dg1,System.ComponentModel.ISupportInitialize).EndInit
        Me.Pnlconfirmation.ResumeLayout(false)
        Me.Pnlconfirmation.PerformLayout
        CType(Me.PictureBox4,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtcrAmt As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtDramt As System.Windows.Forms.TextBox
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents lblCrateBalance As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents txtSearcGroup As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents Pnlconfirmation As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents txtUpdate As System.Windows.Forms.TextBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents lbllinkedfy As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblLinkedName As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
