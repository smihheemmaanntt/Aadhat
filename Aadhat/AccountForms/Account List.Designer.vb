<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Account_List
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Account_List))
        Me.Label41 = New System.Windows.Forms.Label()
        Me.dg1 = New System.Windows.Forms.DataGridView()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtTotal = New System.Windows.Forms.TextBox()
        Me.txtSearcGroup = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.PnlImport = New System.Windows.Forms.Panel()
        Me.btnImportClose = New System.Windows.Forms.Button()
        Me.lblMSg = New System.Windows.Forms.Label()
        Me.pbImport = New System.Windows.Forms.ProgressBar()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnimportcont = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.btnSelectFile = New System.Windows.Forms.Button()
        Me.txtpath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PnlExport = New System.Windows.Forms.Panel()
        Me.btnExportClose = New System.Windows.Forms.Button()
        Me.btnExportAccounts = New System.Windows.Forms.Button()
        Me.RadioExportCrediors = New System.Windows.Forms.RadioButton()
        Me.RadioExportDebtors = New System.Windows.Forms.RadioButton()
        Me.radioExportAll = New System.Windows.Forms.RadioButton()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lblTotalCount = New System.Windows.Forms.Label()
        Me.cbMoreSearch = New System.Windows.Forms.ComboBox()
        Me.txtMoreSearch = New System.Windows.Forms.TextBox()
        Me.ckShowAllAccounts = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.btnRetrive = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.btnImport = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.btnFirst = New System.Windows.Forms.Button()
        Me.btnLast = New System.Windows.Forms.Button()
        Me.btnNext = New System.Windows.Forms.Button()
        Me.btnPrevious = New System.Windows.Forms.Button()
        Me.lblTotalAccounts = New System.Windows.Forms.Label()
        Me.lblCurrentpage = New System.Windows.Forms.Label()
        Me.lblTotalPage = New System.Windows.Forms.Label()
        Me.txtpagebal = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlImport.SuspendLayout()
        Me.PnlExport.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Bodoni Bk BT", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ForeColor = System.Drawing.Color.Black
        Me.Label41.Location = New System.Drawing.Point(628, 11)
        Me.Label41.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(350, 59)
        Me.Label41.TabIndex = 101
        Me.Label41.Text = "ACCOUNT LIST"
        '
        'dg1
        '
        Me.dg1.AllowUserToAddRows = False
        Me.dg1.AllowUserToDeleteRows = False
        Me.dg1.AllowUserToResizeRows = False
        Me.dg1.BackgroundColor = System.Drawing.Color.GhostWhite
        Me.dg1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Crimson
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dg1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dg1.ColumnHeadersHeight = 25
        Me.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.GhostWhite
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Maroon
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dg1.DefaultCellStyle = DataGridViewCellStyle2
        Me.dg1.EnableHeadersVisualStyles = False
        Me.dg1.GridColor = System.Drawing.Color.Crimson
        Me.dg1.Location = New System.Drawing.Point(16, 124)
        Me.dg1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dg1.MultiSelect = False
        Me.dg1.Name = "dg1"
        Me.dg1.ReadOnly = True
        Me.dg1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.dg1.RowHeadersVisible = False
        Me.dg1.RowHeadersWidth = 42
        Me.dg1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.dg1.RowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dg1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dg1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dg1.Size = New System.Drawing.Size(1563, 576)
        Me.dg1.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Black
        Me.Label11.Location = New System.Drawing.Point(1181, 763)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(136, 22)
        Me.Label11.TabIndex = 40120
        Me.Label11.Text = "Total Balance : "
        '
        'txtTotal
        '
        Me.txtTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTotal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtTotal.ForeColor = System.Drawing.Color.Red
        Me.txtTotal.Location = New System.Drawing.Point(1325, 758)
        Me.txtTotal.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtTotal.Name = "txtTotal"
        Me.txtTotal.ReadOnly = True
        Me.txtTotal.Size = New System.Drawing.Size(253, 30)
        Me.txtTotal.TabIndex = 40119
        Me.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSearcGroup
        '
        Me.txtSearcGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtSearcGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearcGroup.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtSearcGroup.ForeColor = System.Drawing.Color.Red
        Me.txtSearcGroup.Location = New System.Drawing.Point(496, 94)
        Me.txtSearcGroup.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtSearcGroup.Name = "txtSearcGroup"
        Me.txtSearcGroup.Size = New System.Drawing.Size(287, 30)
        Me.txtSearcGroup.TabIndex = 40124
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.CadetBlue
        Me.Label1.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label1.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label1.Location = New System.Drawing.Point(393, 94)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(103, 32)
        Me.Label1.TabIndex = 40125
        Me.Label1.Text = "Group :"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtSearch
        '
        Me.txtSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtSearch.ForeColor = System.Drawing.Color.Red
        Me.txtSearch.Location = New System.Drawing.Point(104, 94)
        Me.txtSearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(290, 30)
        Me.txtSearch.TabIndex = 40122
        '
        'Label42
        '
        Me.Label42.BackColor = System.Drawing.Color.CadetBlue
        Me.Label42.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label42.ForeColor = System.Drawing.Color.GhostWhite
        Me.Label42.Location = New System.Drawing.Point(16, 94)
        Me.Label42.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(91, 32)
        Me.Label42.TabIndex = 40123
        Me.Label42.Text = " Name :"
        Me.Label42.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PnlImport
        '
        Me.PnlImport.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.PnlImport.Controls.Add(Me.btnImportClose)
        Me.PnlImport.Controls.Add(Me.lblMSg)
        Me.PnlImport.Controls.Add(Me.pbImport)
        Me.PnlImport.Controls.Add(Me.Label4)
        Me.PnlImport.Controls.Add(Me.Label3)
        Me.PnlImport.Controls.Add(Me.btnimportcont)
        Me.PnlImport.Controls.Add(Me.TextBox2)
        Me.PnlImport.Controls.Add(Me.btnSelectFile)
        Me.PnlImport.Controls.Add(Me.txtpath)
        Me.PnlImport.Controls.Add(Me.Label2)
        Me.PnlImport.Location = New System.Drawing.Point(480, 350)
        Me.PnlImport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PnlImport.Name = "PnlImport"
        Me.PnlImport.Size = New System.Drawing.Size(649, 261)
        Me.PnlImport.TabIndex = 40128
        Me.PnlImport.Visible = False
        '
        'btnImportClose
        '
        Me.btnImportClose.BackColor = System.Drawing.Color.Red
        Me.btnImportClose.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportClose.ForeColor = System.Drawing.Color.Teal
        Me.btnImportClose.Location = New System.Drawing.Point(0, 0)
        Me.btnImportClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnImportClose.Name = "btnImportClose"
        Me.btnImportClose.Size = New System.Drawing.Size(55, 43)
        Me.btnImportClose.TabIndex = 40138
        Me.btnImportClose.Text = "X"
        Me.btnImportClose.UseVisualStyleBackColor = False
        '
        'lblMSg
        '
        Me.lblMSg.AutoSize = True
        Me.lblMSg.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMSg.ForeColor = System.Drawing.Color.Green
        Me.lblMSg.Location = New System.Drawing.Point(55, 229)
        Me.lblMSg.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblMSg.Name = "lblMSg"
        Me.lblMSg.Size = New System.Drawing.Size(0, 22)
        Me.lblMSg.TabIndex = 40137
        Me.lblMSg.Visible = False
        '
        'pbImport
        '
        Me.pbImport.Location = New System.Drawing.Point(359, 226)
        Me.pbImport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.pbImport.Name = "pbImport"
        Me.pbImport.Size = New System.Drawing.Size(287, 28)
        Me.pbImport.TabIndex = 40136
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Green
        Me.Label4.Location = New System.Drawing.Point(49, 154)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(331, 22)
        Me.Label4.TabIndex = 40135
        Me.Label4.Text = "If want to Import Some Record type No. "
        Me.Label4.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Green
        Me.Label3.Location = New System.Drawing.Point(49, 76)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(394, 22)
        Me.Label3.TabIndex = 40134
        Me.Label3.Text = "Path Should Be Shown in Below Given Text box"
        '
        'btnimportcont
        '
        Me.btnimportcont.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnimportcont.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnimportcont.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnimportcont.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnimportcont.Location = New System.Drawing.Point(359, 182)
        Me.btnimportcont.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnimportcont.Name = "btnimportcont"
        Me.btnimportcont.Size = New System.Drawing.Size(239, 41)
        Me.btnimportcont.TabIndex = 40133
        Me.btnimportcont.Text = "&Import exports"
        Me.btnimportcont.UseVisualStyleBackColor = False
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.AliceBlue
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBox2.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.ForeColor = System.Drawing.Color.Red
        Me.TextBox2.Location = New System.Drawing.Point(55, 186)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(287, 30)
        Me.TextBox2.TabIndex = 40132
        Me.TextBox2.Visible = False
        '
        'btnSelectFile
        '
        Me.btnSelectFile.BackColor = System.Drawing.Color.Blue
        Me.btnSelectFile.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectFile.ForeColor = System.Drawing.Color.Teal
        Me.btnSelectFile.Location = New System.Drawing.Point(359, 36)
        Me.btnSelectFile.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSelectFile.Name = "btnSelectFile"
        Me.btnSelectFile.Size = New System.Drawing.Size(239, 41)
        Me.btnSelectFile.TabIndex = 40131
        Me.btnSelectFile.Text = "&Select Excel File "
        Me.btnSelectFile.UseVisualStyleBackColor = False
        '
        'txtpath
        '
        Me.txtpath.BackColor = System.Drawing.Color.AliceBlue
        Me.txtpath.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtpath.ForeColor = System.Drawing.Color.Red
        Me.txtpath.Location = New System.Drawing.Point(55, 105)
        Me.txtpath.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtpath.Name = "txtpath"
        Me.txtpath.Size = New System.Drawing.Size(541, 30)
        Me.txtpath.TabIndex = 40130
        Me.txtpath.Text = "Path Should Be Shown Here"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Monotype Corsiva", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Teal
        Me.Label2.Location = New System.Drawing.Point(48, 36)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(212, 37)
        Me.Label2.TabIndex = 40129
        Me.Label2.Text = "Import Accounts"
        '
        'PnlExport
        '
        Me.PnlExport.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.PnlExport.Controls.Add(Me.btnExportClose)
        Me.PnlExport.Controls.Add(Me.btnExportAccounts)
        Me.PnlExport.Controls.Add(Me.RadioExportCrediors)
        Me.PnlExport.Controls.Add(Me.RadioExportDebtors)
        Me.PnlExport.Controls.Add(Me.radioExportAll)
        Me.PnlExport.Controls.Add(Me.Label7)
        Me.PnlExport.Location = New System.Drawing.Point(480, 351)
        Me.PnlExport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PnlExport.Name = "PnlExport"
        Me.PnlExport.Size = New System.Drawing.Size(649, 154)
        Me.PnlExport.TabIndex = 40136
        Me.PnlExport.Visible = False
        '
        'btnExportClose
        '
        Me.btnExportClose.BackColor = System.Drawing.Color.Red
        Me.btnExportClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportClose.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportClose.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnExportClose.Location = New System.Drawing.Point(0, 0)
        Me.btnExportClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExportClose.Name = "btnExportClose"
        Me.btnExportClose.Size = New System.Drawing.Size(55, 43)
        Me.btnExportClose.TabIndex = 40139
        Me.btnExportClose.Text = "X"
        Me.btnExportClose.UseVisualStyleBackColor = False
        '
        'btnExportAccounts
        '
        Me.btnExportAccounts.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnExportAccounts.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExportAccounts.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExportAccounts.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnExportAccounts.Location = New System.Drawing.Point(359, 30)
        Me.btnExportAccounts.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExportAccounts.Name = "btnExportAccounts"
        Me.btnExportAccounts.Size = New System.Drawing.Size(239, 41)
        Me.btnExportAccounts.TabIndex = 40134
        Me.btnExportAccounts.Text = "&Export Accounts"
        Me.btnExportAccounts.UseVisualStyleBackColor = False
        '
        'RadioExportCrediors
        '
        Me.RadioExportCrediors.AutoSize = True
        Me.RadioExportCrediors.Location = New System.Drawing.Point(416, 91)
        Me.RadioExportCrediors.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioExportCrediors.Name = "RadioExportCrediors"
        Me.RadioExportCrediors.Size = New System.Drawing.Size(130, 21)
        Me.RadioExportCrediors.TabIndex = 40132
        Me.RadioExportCrediors.TabStop = True
        Me.RadioExportCrediors.Text = "Export Creditors"
        Me.RadioExportCrediors.UseVisualStyleBackColor = True
        '
        'RadioExportDebtors
        '
        Me.RadioExportDebtors.AutoSize = True
        Me.RadioExportDebtors.Location = New System.Drawing.Point(249, 91)
        Me.RadioExportDebtors.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RadioExportDebtors.Name = "RadioExportDebtors"
        Me.RadioExportDebtors.Size = New System.Drawing.Size(123, 21)
        Me.RadioExportDebtors.TabIndex = 40131
        Me.RadioExportDebtors.TabStop = True
        Me.RadioExportDebtors.Text = "Export Debtors"
        Me.RadioExportDebtors.UseVisualStyleBackColor = True
        '
        'radioExportAll
        '
        Me.radioExportAll.AutoSize = True
        Me.radioExportAll.Location = New System.Drawing.Point(56, 91)
        Me.radioExportAll.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.radioExportAll.Name = "radioExportAll"
        Me.radioExportAll.Size = New System.Drawing.Size(149, 21)
        Me.radioExportAll.TabIndex = 40130
        Me.radioExportAll.TabStop = True
        Me.radioExportAll.Text = "Export all Accounts"
        Me.radioExportAll.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Monotype Corsiva", 18.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Teal
        Me.Label7.Location = New System.Drawing.Point(48, 36)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(214, 37)
        Me.Label7.TabIndex = 40129
        Me.Label7.Text = "Export Accounts"
        '
        'lblTotalCount
        '
        Me.lblTotalCount.AutoSize = True
        Me.lblTotalCount.Font = New System.Drawing.Font("Century", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTotalCount.ForeColor = System.Drawing.Color.Black
        Me.lblTotalCount.Location = New System.Drawing.Point(1161, 703)
        Me.lblTotalCount.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalCount.Name = "lblTotalCount"
        Me.lblTotalCount.Size = New System.Drawing.Size(91, 21)
        Me.lblTotalCount.TabIndex = 40137
        Me.lblTotalCount.Text = "RowCount"
        '
        'cbMoreSearch
        '
        Me.cbMoreSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cbMoreSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cbMoreSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.cbMoreSearch.DropDownHeight = 100
        Me.cbMoreSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMoreSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cbMoreSearch.Font = New System.Drawing.Font("Times New Roman", 12.0!)
        Me.cbMoreSearch.FormattingEnabled = True
        Me.cbMoreSearch.IntegralHeight = False
        Me.cbMoreSearch.Items.AddRange(New Object() {"Area", "City", "State", "Mobile No. 1", "Mobile No. 2", "Opening Balance", "DR/Cr", "Tag"})
        Me.cbMoreSearch.Location = New System.Drawing.Point(955, 94)
        Me.cbMoreSearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cbMoreSearch.Name = "cbMoreSearch"
        Me.cbMoreSearch.Size = New System.Drawing.Size(225, 30)
        Me.cbMoreSearch.TabIndex = 40239
        '
        'txtMoreSearch
        '
        Me.txtMoreSearch.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtMoreSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMoreSearch.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtMoreSearch.ForeColor = System.Drawing.Color.Red
        Me.txtMoreSearch.Location = New System.Drawing.Point(1180, 94)
        Me.txtMoreSearch.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtMoreSearch.Name = "txtMoreSearch"
        Me.txtMoreSearch.Size = New System.Drawing.Size(399, 30)
        Me.txtMoreSearch.TabIndex = 40240
        '
        'ckShowAllAccounts
        '
        Me.ckShowAllAccounts.AutoSize = True
        Me.ckShowAllAccounts.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ckShowAllAccounts.Location = New System.Drawing.Point(1433, 60)
        Me.ckShowAllAccounts.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ckShowAllAccounts.Name = "ckShowAllAccounts"
        Me.ckShowAllAccounts.Size = New System.Drawing.Size(135, 26)
        Me.ckShowAllAccounts.TabIndex = 40241
        Me.ckShowAllAccounts.Text = "&All Accounts"
        Me.ckShowAllAccounts.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.CadetBlue
        Me.Label5.Font = New System.Drawing.Font("Times New Roman", 14.0!)
        Me.Label5.ForeColor = System.Drawing.Color.White
        Me.Label5.Location = New System.Drawing.Point(784, 94)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(169, 32)
        Me.Label5.TabIndex = 40242
        Me.Label5.Text = "Search More :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnRetrive
        '
        Me.btnRetrive.BackColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(187, Byte), Integer), CType(CType(153, Byte), Integer))
        Me.btnRetrive.FlatAppearance.BorderSize = 0
        Me.btnRetrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRetrive.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRetrive.ForeColor = System.Drawing.Color.Red
        Me.btnRetrive.Location = New System.Drawing.Point(1431, 23)
        Me.btnRetrive.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnRetrive.Name = "btnRetrive"
        Me.btnRetrive.Size = New System.Drawing.Size(71, 38)
        Me.btnRetrive.TabIndex = 91116
        Me.btnRetrive.TabStop = False
        Me.btnRetrive.Text = "Retrive"
        Me.btnRetrive.UseVisualStyleBackColor = False
        Me.btnRetrive.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(553, 11)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(50, 50)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 91115
        Me.PictureBox1.TabStop = False
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.btnClose.FlatAppearance.BorderSize = 0
        Me.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClose.Font = New System.Drawing.Font("Times New Roman", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.ForeColor = System.Drawing.Color.Red
        Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
        Me.btnClose.Location = New System.Drawing.Point(1524, 0)
        Me.btnClose.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(71, 58)
        Me.btnClose.TabIndex = 91114
        Me.btnClose.TabStop = False
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.DarkGoldenrod
        Me.btnExport.FlatAppearance.BorderSize = 0
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnExport.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnExport.Image = CType(resources.GetObject("btnExport.Image"), System.Drawing.Image)
        Me.btnExport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnExport.Location = New System.Drawing.Point(212, 722)
        Me.btnExport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(201, 60)
        Me.btnExport.TabIndex = 40127
        Me.btnExport.Text = "&Export Accounts "
        Me.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'btnImport
        '
        Me.btnImport.BackColor = System.Drawing.Color.DarkOliveGreen
        Me.btnImport.FlatAppearance.BorderSize = 0
        Me.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImport.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnImport.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnImport.Image = CType(resources.GetObject("btnImport.Image"), System.Drawing.Image)
        Me.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImport.Location = New System.Drawing.Point(16, 722)
        Me.btnImport.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnImport.Name = "btnImport"
        Me.btnImport.Size = New System.Drawing.Size(196, 60)
        Me.btnImport.TabIndex = 40126
        Me.btnImport.Text = "&Import Accounts "
        Me.btnImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnImport.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Brown
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Button1.ForeColor = System.Drawing.Color.GhostWhite
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(413, 722)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(187, 60)
        Me.Button1.TabIndex = 40121
        Me.Button1.Text = "&Create Account "
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = False
        '
        'btnPrint
        '
        Me.btnPrint.BackColor = System.Drawing.Color.Chocolate
        Me.btnPrint.FlatAppearance.BorderSize = 0
        Me.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrint.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnPrint.ForeColor = System.Drawing.Color.GhostWhite
        Me.btnPrint.Image = CType(resources.GetObject("btnPrint.Image"), System.Drawing.Image)
        Me.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPrint.Location = New System.Drawing.Point(600, 722)
        Me.btnPrint.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(113, 60)
        Me.btnPrint.TabIndex = 40053
        Me.btnPrint.Text = "&Print"
        Me.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPrint.UseVisualStyleBackColor = False
        '
        'btnFirst
        '
        Me.btnFirst.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFirst.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnFirst.ForeColor = System.Drawing.Color.Navy
        Me.btnFirst.Location = New System.Drawing.Point(861, 759)
        Me.btnFirst.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnFirst.Name = "btnFirst"
        Me.btnFirst.Size = New System.Drawing.Size(59, 28)
        Me.btnFirst.TabIndex = 91120
        Me.btnFirst.TabStop = False
        Me.btnFirst.Text = "|<"
        Me.btnFirst.UseVisualStyleBackColor = True
        '
        'btnLast
        '
        Me.btnLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLast.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnLast.ForeColor = System.Drawing.Color.Navy
        Me.btnLast.Location = New System.Drawing.Point(1061, 759)
        Me.btnLast.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnLast.Name = "btnLast"
        Me.btnLast.Size = New System.Drawing.Size(59, 28)
        Me.btnLast.TabIndex = 91119
        Me.btnLast.TabStop = False
        Me.btnLast.Text = ">|"
        Me.btnLast.UseVisualStyleBackColor = True
        '
        'btnNext
        '
        Me.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNext.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnNext.ForeColor = System.Drawing.Color.Navy
        Me.btnNext.Location = New System.Drawing.Point(995, 759)
        Me.btnNext.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(59, 28)
        Me.btnNext.TabIndex = 91118
        Me.btnNext.TabStop = False
        Me.btnNext.Text = ">>"
        Me.btnNext.UseVisualStyleBackColor = True
        '
        'btnPrevious
        '
        Me.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPrevious.Font = New System.Drawing.Font("Times New Roman", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.btnPrevious.ForeColor = System.Drawing.Color.Navy
        Me.btnPrevious.Location = New System.Drawing.Point(928, 759)
        Me.btnPrevious.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnPrevious.Name = "btnPrevious"
        Me.btnPrevious.Size = New System.Drawing.Size(59, 28)
        Me.btnPrevious.TabIndex = 91117
        Me.btnPrevious.TabStop = False
        Me.btnPrevious.Text = "<<"
        Me.btnPrevious.UseVisualStyleBackColor = True
        '
        'lblTotalAccounts
        '
        Me.lblTotalAccounts.AutoSize = True
        Me.lblTotalAccounts.Font = New System.Drawing.Font("Century", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTotalAccounts.ForeColor = System.Drawing.Color.Black
        Me.lblTotalAccounts.Location = New System.Drawing.Point(1427, 702)
        Me.lblTotalAccounts.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalAccounts.Name = "lblTotalAccounts"
        Me.lblTotalAccounts.Size = New System.Drawing.Size(99, 21)
        Me.lblTotalAccounts.TabIndex = 91121
        Me.lblTotalAccounts.Text = "TotalCount"
        Me.lblTotalAccounts.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCurrentpage
        '
        Me.lblCurrentpage.AutoSize = True
        Me.lblCurrentpage.Font = New System.Drawing.Font("Century", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblCurrentpage.ForeColor = System.Drawing.Color.Black
        Me.lblCurrentpage.Location = New System.Drawing.Point(857, 702)
        Me.lblCurrentpage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCurrentpage.Name = "lblCurrentpage"
        Me.lblCurrentpage.Size = New System.Drawing.Size(99, 21)
        Me.lblCurrentpage.TabIndex = 91122
        Me.lblCurrentpage.Text = "TotalCount"
        '
        'lblTotalPage
        '
        Me.lblTotalPage.AutoSize = True
        Me.lblTotalPage.Font = New System.Drawing.Font("Century", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.lblTotalPage.ForeColor = System.Drawing.Color.Black
        Me.lblTotalPage.Location = New System.Drawing.Point(857, 721)
        Me.lblTotalPage.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotalPage.Name = "lblTotalPage"
        Me.lblTotalPage.Size = New System.Drawing.Size(99, 21)
        Me.lblTotalPage.TabIndex = 91123
        Me.lblTotalPage.Text = "TotalCount"
        '
        'txtpagebal
        '
        Me.txtpagebal.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.txtpagebal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtpagebal.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.txtpagebal.ForeColor = System.Drawing.Color.Red
        Me.txtpagebal.Location = New System.Drawing.Point(1325, 725)
        Me.txtpagebal.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtpagebal.Name = "txtpagebal"
        Me.txtpagebal.ReadOnly = True
        Me.txtpagebal.Size = New System.Drawing.Size(253, 30)
        Me.txtpagebal.TabIndex = 91124
        Me.txtpagebal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Times New Roman", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(1187, 730)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(128, 22)
        Me.Label6.TabIndex = 91125
        Me.Label6.Text = "Page Balance :"
        '
        'Account_List
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(111, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1595, 804)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtpagebal)
        Me.Controls.Add(Me.lblTotalPage)
        Me.Controls.Add(Me.lblCurrentpage)
        Me.Controls.Add(Me.lblTotalAccounts)
        Me.Controls.Add(Me.btnFirst)
        Me.Controls.Add(Me.btnLast)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.btnPrevious)
        Me.Controls.Add(Me.btnRetrive)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.ckShowAllAccounts)
        Me.Controls.Add(Me.Label41)
        Me.Controls.Add(Me.lblTotalCount)
        Me.Controls.Add(Me.PnlExport)
        Me.Controls.Add(Me.PnlImport)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.btnImport)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.txtTotal)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.cbMoreSearch)
        Me.Controls.Add(Me.txtSearcGroup)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.txtMoreSearch)
        Me.Controls.Add(Me.dg1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label42)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "Account_List"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Account_List"
        CType(Me.dg1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlImport.ResumeLayout(False)
        Me.PnlImport.PerformLayout()
        Me.PnlExport.ResumeLayout(False)
        Me.PnlExport.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents dg1 As System.Windows.Forms.DataGridView
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtTotal As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents txtSearcGroup As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents btnImport As System.Windows.Forms.Button
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents PnlImport As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnimportcont As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents btnSelectFile As System.Windows.Forms.Button
    Friend WithEvents txtpath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PnlExport As System.Windows.Forms.Panel
    Friend WithEvents btnExportAccounts As System.Windows.Forms.Button
    Friend WithEvents RadioExportCrediors As System.Windows.Forms.RadioButton
    Friend WithEvents RadioExportDebtors As System.Windows.Forms.RadioButton
    Friend WithEvents radioExportAll As System.Windows.Forms.RadioButton
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblTotalCount As System.Windows.Forms.Label
    Friend WithEvents pbImport As System.Windows.Forms.ProgressBar
    Friend WithEvents lblMSg As System.Windows.Forms.Label
    Friend WithEvents btnImportClose As System.Windows.Forms.Button
    Friend WithEvents btnExportClose As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents cbMoreSearch As System.Windows.Forms.ComboBox
    Friend WithEvents txtMoreSearch As System.Windows.Forms.TextBox
    Friend WithEvents ckShowAllAccounts As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnRetrive As System.Windows.Forms.Button
    Friend WithEvents btnFirst As System.Windows.Forms.Button
    Friend WithEvents btnLast As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents btnPrevious As System.Windows.Forms.Button
    Friend WithEvents lblTotalAccounts As System.Windows.Forms.Label
    Friend WithEvents lblCurrentpage As System.Windows.Forms.Label
    Friend WithEvents lblTotalPage As System.Windows.Forms.Label
    Friend WithEvents txtpagebal As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class
