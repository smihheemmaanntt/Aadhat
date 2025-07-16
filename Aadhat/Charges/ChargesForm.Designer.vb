<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ChargesForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChargesForm))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtChargeName = New System.Windows.Forms.TextBox()
        Me.txtCalculate = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbAccountName = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RadioCrate = New System.Windows.Forms.RadioButton()
        Me.radioWeight = New System.Windows.Forms.RadioButton()
        Me.radioNug = New System.Windows.Forms.RadioButton()
        Me.radioPercentage = New System.Windows.Forms.RadioButton()
        Me.RadioAmount = New System.Windows.Forms.RadioButton()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.radioMinus = New System.Windows.Forms.RadioButton()
        Me.radioPlus = New System.Windows.Forms.RadioButton()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.LblCharges = New System.Windows.Forms.Label()
        Me.btnRetrive = New System.Windows.Forms.Button()
        Me.CkRoundOff = New System.Windows.Forms.CheckBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.cbApply = New System.Windows.Forms.ComboBox()
        Me.cbCost = New System.Windows.Forms.ComboBox()
        Me.txtID = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtPrintName = New System.Windows.Forms.TextBox()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtChargeName
        '
        Me.TxtChargeName.BackColor = System.Drawing.Color.GhostWhite
        Me.TxtChargeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TxtChargeName.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.TxtChargeName.Location = New System.Drawing.Point(14, 136)
        Me.TxtChargeName.Name = "TxtChargeName"
        Me.TxtChargeName.Size = New System.Drawing.Size(244, 26)
        Me.TxtChargeName.TabIndex = 0
        '
        'txtCalculate
        '
        Me.txtCalculate.BackColor = System.Drawing.Color.GhostWhite
        Me.txtCalculate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCalculate.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtCalculate.Location = New System.Drawing.Point(500, 136)
        Me.txtCalculate.Name = "txtCalculate"
        Me.txtCalculate.Size = New System.Drawing.Size(82, 26)
        Me.txtCalculate.TabIndex = 2
        Me.txtCalculate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(12, 115)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 19)
        Me.Label2.TabIndex = 109
        Me.Label2.Text = "Charge Name"
        '
        'cbAccountName
        '
        Me.cbAccountName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbAccountName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbAccountName.BackColor = System.Drawing.Color.GhostWhite
        Me.cbAccountName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAccountName.DropDownWidth = 200
        Me.cbAccountName.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbAccountName.Font = New System.Drawing.Font("Times New Roman", 10.25!)
        Me.cbAccountName.ForeColor = System.Drawing.Color.Black
        Me.cbAccountName.FormattingEnabled = True
        Me.cbAccountName.Location = New System.Drawing.Point(582, 136)
        Me.cbAccountName.Name = "cbAccountName"
        Me.cbAccountName.Size = New System.Drawing.Size(210, 24)
        Me.cbAccountName.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(498, 115)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 19)
        Me.Label1.TabIndex = 110
        Me.Label1.Text = "Calculate"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(585, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 19)
        Me.Label3.TabIndex = 111
        Me.Label3.Text = "Account to be "
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Peru
        Me.Panel1.Controls.Add(Me.RadioCrate)
        Me.Panel1.Controls.Add(Me.radioWeight)
        Me.Panel1.Controls.Add(Me.radioNug)
        Me.Panel1.Controls.Add(Me.radioPercentage)
        Me.Panel1.Controls.Add(Me.RadioAmount)
        Me.Panel1.Location = New System.Drawing.Point(14, 190)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(617, 33)
        Me.Panel1.TabIndex = 6
        '
        'RadioCrate
        '
        Me.RadioCrate.AutoSize = True
        Me.RadioCrate.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioCrate.ForeColor = System.Drawing.Color.GhostWhite
        Me.RadioCrate.Location = New System.Drawing.Point(519, 6)
        Me.RadioCrate.Name = "RadioCrate"
        Me.RadioCrate.Size = New System.Drawing.Size(96, 25)
        Me.RadioCrate.TabIndex = 4
        Me.RadioCrate.TabStop = True
        Me.RadioCrate.Text = "On Crate"
        Me.RadioCrate.UseVisualStyleBackColor = True
        '
        'radioWeight
        '
        Me.radioWeight.AutoSize = True
        Me.radioWeight.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioWeight.ForeColor = System.Drawing.Color.GhostWhite
        Me.radioWeight.Location = New System.Drawing.Point(381, 6)
        Me.radioWeight.Name = "radioWeight"
        Me.radioWeight.Size = New System.Drawing.Size(107, 25)
        Me.radioWeight.TabIndex = 3
        Me.radioWeight.TabStop = True
        Me.radioWeight.Text = "On Weight"
        Me.radioWeight.UseVisualStyleBackColor = True
        '
        'radioNug
        '
        Me.radioNug.AutoSize = True
        Me.radioNug.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioNug.ForeColor = System.Drawing.Color.GhostWhite
        Me.radioNug.Location = New System.Drawing.Point(279, 5)
        Me.radioNug.Name = "radioNug"
        Me.radioNug.Size = New System.Drawing.Size(87, 25)
        Me.radioNug.TabIndex = 2
        Me.radioNug.TabStop = True
        Me.radioNug.Text = "On Nug"
        Me.radioNug.UseVisualStyleBackColor = True
        '
        'radioPercentage
        '
        Me.radioPercentage.AutoSize = True
        Me.radioPercentage.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioPercentage.ForeColor = System.Drawing.Color.GhostWhite
        Me.radioPercentage.Location = New System.Drawing.Point(135, 5)
        Me.radioPercentage.Name = "radioPercentage"
        Me.radioPercentage.Size = New System.Drawing.Size(109, 25)
        Me.radioPercentage.TabIndex = 1
        Me.radioPercentage.TabStop = True
        Me.radioPercentage.Text = "Percentage"
        Me.radioPercentage.UseVisualStyleBackColor = True
        '
        'RadioAmount
        '
        Me.RadioAmount.AutoSize = True
        Me.RadioAmount.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioAmount.ForeColor = System.Drawing.Color.GhostWhite
        Me.RadioAmount.Location = New System.Drawing.Point(5, 4)
        Me.RadioAmount.Name = "RadioAmount"
        Me.RadioAmount.Size = New System.Drawing.Size(115, 25)
        Me.RadioAmount.TabIndex = 0
        Me.RadioAmount.TabStop = True
        Me.RadioAmount.Text = "On Amount"
        Me.RadioAmount.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Tan
        Me.Panel2.Controls.Add(Me.radioMinus)
        Me.Panel2.Controls.Add(Me.radioPlus)
        Me.Panel2.Location = New System.Drawing.Point(630, 190)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(325, 33)
        Me.Panel2.TabIndex = 7
        '
        'radioMinus
        '
        Me.radioMinus.AutoSize = True
        Me.radioMinus.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioMinus.ForeColor = System.Drawing.Color.Black
        Me.radioMinus.Location = New System.Drawing.Point(191, 4)
        Me.radioMinus.Name = "radioMinus"
        Me.radioMinus.Size = New System.Drawing.Size(97, 25)
        Me.radioMinus.TabIndex = 1
        Me.radioMinus.TabStop = True
        Me.radioMinus.Text = "Minus (-)"
        Me.radioMinus.UseVisualStyleBackColor = True
        '
        'radioPlus
        '
        Me.radioPlus.AutoSize = True
        Me.radioPlus.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.radioPlus.ForeColor = System.Drawing.Color.Black
        Me.radioPlus.Location = New System.Drawing.Point(39, 4)
        Me.radioPlus.Name = "radioPlus"
        Me.radioPlus.Size = New System.Drawing.Size(88, 25)
        Me.radioPlus.TabIndex = 0
        Me.radioPlus.TabStop = True
        Me.radioPlus.Text = "Plus (+)"
        Me.radioPlus.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(788, 115)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 19)
        Me.Label4.TabIndex = 115
        Me.Label4.Text = "Apply on"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(931, 115)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 19)
        Me.Label5.TabIndex = 117
        Me.Label5.Text = "Cost on"
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
        Me.btnClose.TabIndex = 91118
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'LblCharges
        '
        Me.LblCharges.AutoSize = True
        Me.LblCharges.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCharges.ForeColor = System.Drawing.Color.Black
        Me.LblCharges.Location = New System.Drawing.Point(440, 9)
        Me.LblCharges.Name = "LblCharges"
        Me.LblCharges.Size = New System.Drawing.Size(326, 48)
        Me.LblCharges.TabIndex = 101
        Me.LblCharges.Text = "CHARGES ENTRY"
        '
        'btnRetrive
        '
        Me.btnRetrive.BackColor = System.Drawing.Color.Black
        Me.btnRetrive.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRetrive.ForeColor = System.Drawing.Color.Red
        Me.btnRetrive.Location = New System.Drawing.Point(1143, 0)
        Me.btnRetrive.Name = "btnRetrive"
        Me.btnRetrive.Size = New System.Drawing.Size(51, 43)
        Me.btnRetrive.TabIndex = 91112
        Me.btnRetrive.TabStop = False
        Me.btnRetrive.Text = "R"
        Me.btnRetrive.UseVisualStyleBackColor = False
        '
        'CkRoundOff
        '
        Me.CkRoundOff.AutoSize = True
        Me.CkRoundOff.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.CkRoundOff.ForeColor = System.Drawing.Color.Red
        Me.CkRoundOff.Location = New System.Drawing.Point(969, 195)
        Me.CkRoundOff.Name = "CkRoundOff"
        Me.CkRoundOff.Size = New System.Drawing.Size(88, 23)
        Me.CkRoundOff.TabIndex = 6
        Me.CkRoundOff.Text = "&Round off"
        Me.CkRoundOff.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.DarkTurquoise
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnSave.Image = Global.Aadhat.My.Resources.Resources.Save
        Me.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSave.Location = New System.Drawing.Point(1068, 172)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(116, 51)
        Me.btnSave.TabIndex = 7
        Me.btnSave.Text = "&Save"
        Me.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'BtnDelete
        '
        Me.BtnDelete.BackColor = System.Drawing.Color.DarkRed
        Me.BtnDelete.FlatAppearance.BorderSize = 0
        Me.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDelete.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.BtnDelete.ForeColor = System.Drawing.Color.GhostWhite
        Me.BtnDelete.Image = CType(resources.GetObject("BtnDelete.Image"), System.Drawing.Image)
        Me.BtnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.BtnDelete.Location = New System.Drawing.Point(1068, 109)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(116, 55)
        Me.BtnDelete.TabIndex = 8
        Me.BtnDelete.TabStop = False
        Me.BtnDelete.Text = "&Delete"
        Me.BtnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.BtnDelete.UseVisualStyleBackColor = False
        '
        'cbApply
        '
        Me.cbApply.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbApply.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbApply.BackColor = System.Drawing.Color.GhostWhite
        Me.cbApply.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbApply.Font = New System.Drawing.Font("Times New Roman", 10.75!)
        Me.cbApply.ForeColor = System.Drawing.Color.Black
        Me.cbApply.FormattingEnabled = True
        Me.cbApply.Items.AddRange(New Object() {"Basic Amount", "Item Total", "Total Amount"})
        Me.cbApply.Location = New System.Drawing.Point(792, 136)
        Me.cbApply.Name = "cbApply"
        Me.cbApply.Size = New System.Drawing.Size(144, 24)
        Me.cbApply.TabIndex = 4
        '
        'cbCost
        '
        Me.cbCost.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbCost.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbCost.BackColor = System.Drawing.Color.GhostWhite
        Me.cbCost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCost.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbCost.Font = New System.Drawing.Font("Times New Roman", 10.75!)
        Me.cbCost.ForeColor = System.Drawing.Color.Black
        Me.cbCost.FormattingEnabled = True
        Me.cbCost.Items.AddRange(New Object() {"Party Cost", "Our Cost", "N.A."})
        Me.cbCost.Location = New System.Drawing.Point(935, 136)
        Me.cbCost.Name = "cbCost"
        Me.cbCost.Size = New System.Drawing.Size(117, 24)
        Me.cbCost.TabIndex = 5
        '
        'txtID
        '
        Me.txtID.BackColor = System.Drawing.Color.AliceBlue
        Me.txtID.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.txtID.Location = New System.Drawing.Point(1085, 77)
        Me.txtID.Name = "txtID"
        Me.txtID.Size = New System.Drawing.Size(99, 26)
        Me.txtID.TabIndex = 40029
        Me.txtID.TabStop = False
        Me.txtID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtID.Visible = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(213, 169)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(58, 14)
        Me.Label6.TabIndex = 40030
        Me.Label6.Text = "Apply Type"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.Label7.Font = New System.Drawing.Font("Times New Roman", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(768, 169)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 14)
        Me.Label7.TabIndex = 40031
        Me.Label7.Text = "Charge Type"
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dg1.ColumnHeadersHeight = 28
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Maroon
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle2
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Crimson
        Me.dg1.Location = New System.Drawing.Point(12, 229)
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Maroon
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dg1.RowHeadersVisible = False
        Me.dg1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Maroon
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle4
        Me.dg1.RowTemplate.DefaultCellStyle.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.dg1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1172, 374)
        Me.dg1.TabIndex = 40220
        Me.dg1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(384, 7)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 91139
        Me.PictureBox2.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(256, 115)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(78, 19)
        Me.Label8.TabIndex = 91141
        Me.Label8.Text = "Print Name"
        '
        'txtPrintName
        '
        Me.txtPrintName.BackColor = System.Drawing.Color.GhostWhite
        Me.txtPrintName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPrintName.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtPrintName.Location = New System.Drawing.Point(257, 136)
        Me.txtPrintName.Name = "txtPrintName"
        Me.txtPrintName.Size = New System.Drawing.Size(245, 26)
        Me.txtPrintName.TabIndex = 1
        '
        'ChargesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.GhostWhite
        Me.ClientSize = New System.Drawing.Size(1196, 653)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtPrintName)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.LblCharges)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.btnRetrive)
        Me.Controls.Add(Me.txtID)
        Me.Controls.Add(Me.cbCost)
        Me.Controls.Add(Me.cbApply)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.CkRoundOff)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cbAccountName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtCalculate)
        Me.Controls.Add(Me.TxtChargeName)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.ForeColor = System.Drawing.Color.GhostWhite
        Me.KeyPreview = True
        Me.Name = "ChargesForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ChargesForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtChargeName As System.Windows.Forms.TextBox
    Friend WithEvents txtCalculate As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbAccountName As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents radioWeight As System.Windows.Forms.RadioButton
    Friend WithEvents radioNug As System.Windows.Forms.RadioButton
    Friend WithEvents radioPercentage As System.Windows.Forms.RadioButton
    Friend WithEvents RadioAmount As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents radioMinus As System.Windows.Forms.RadioButton
    Friend WithEvents radioPlus As System.Windows.Forms.RadioButton
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LblCharges As System.Windows.Forms.Label
    Friend WithEvents btnRetrive As System.Windows.Forms.Button
    Friend WithEvents CkRoundOff As System.Windows.Forms.CheckBox
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents cbApply As System.Windows.Forms.ComboBox
    Friend WithEvents cbCost As System.Windows.Forms.ComboBox
    Friend WithEvents txtID As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents RadioCrate As System.Windows.Forms.RadioButton
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPrintName As System.Windows.Forms.TextBox
End Class
