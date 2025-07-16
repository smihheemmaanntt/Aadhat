namespace SharpZipWrapperDemo
{
    partial class Demo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.btnZipFile = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnSelectZip = new System.Windows.Forms.Button();
            this.txtZipFile = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSelectDir = new System.Windows.Forms.Button();
            this.txtZipDir = new System.Windows.Forms.TextBox();
            this.txtSourceDir = new System.Windows.Forms.TextBox();
            this.btnSelectZipDir = new System.Windows.Forms.Button();
            this.btnZipDir = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSelectDestDir = new System.Windows.Forms.Button();
            this.txtDestDir = new System.Windows.Forms.TextBox();
            this.txtZipUnzip = new System.Windows.Forms.TextBox();
            this.btnSelectZipUnzip = new System.Windows.Forms.Button();
            this.btnUnzip = new System.Windows.Forms.Button();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.saveDialog = new System.Windows.Forms.SaveFileDialog();
            this.linkKellerman = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSourceFile
            // 
            this.txtSourceFile.Location = new System.Drawing.Point(69, 19);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.Size = new System.Drawing.Size(379, 20);
            this.txtSourceFile.TabIndex = 0;
            // 
            // btnZipFile
            // 
            this.btnZipFile.Location = new System.Drawing.Point(69, 71);
            this.btnZipFile.Name = "btnZipFile";
            this.btnZipFile.Size = new System.Drawing.Size(75, 23);
            this.btnZipFile.TabIndex = 1;
            this.btnZipFile.Text = "Zip File";
            this.btnZipFile.UseVisualStyleBackColor = true;
            this.btnZipFile.Click += new System.EventHandler(this.btnZipFile_Click);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(454, 16);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select...";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // btnSelectZip
            // 
            this.btnSelectZip.Location = new System.Drawing.Point(454, 45);
            this.btnSelectZip.Name = "btnSelectZip";
            this.btnSelectZip.Size = new System.Drawing.Size(75, 23);
            this.btnSelectZip.TabIndex = 3;
            this.btnSelectZip.Text = "Select Zip";
            this.btnSelectZip.UseVisualStyleBackColor = true;
            this.btnSelectZip.Click += new System.EventHandler(this.btnSelectZip_Click);
            // 
            // txtZipFile
            // 
            this.txtZipFile.Location = new System.Drawing.Point(69, 45);
            this.txtZipFile.Name = "txtZipFile";
            this.txtZipFile.Size = new System.Drawing.Size(379, 20);
            this.txtZipFile.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSelectFile);
            this.groupBox1.Controls.Add(this.txtZipFile);
            this.groupBox1.Controls.Add(this.txtSourceFile);
            this.groupBox1.Controls.Add(this.btnSelectZip);
            this.groupBox1.Controls.Add(this.btnZipFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(539, 102);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add File to Zip";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Zip File";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Source File";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnSelectDir);
            this.groupBox2.Controls.Add(this.txtZipDir);
            this.groupBox2.Controls.Add(this.txtSourceDir);
            this.groupBox2.Controls.Add(this.btnSelectZipDir);
            this.groupBox2.Controls.Add(this.btnZipDir);
            this.groupBox2.Location = new System.Drawing.Point(12, 137);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(539, 102);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Add Directory to Zip";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Zip File";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Source Dir";
            // 
            // btnSelectDir
            // 
            this.btnSelectDir.Location = new System.Drawing.Point(454, 16);
            this.btnSelectDir.Name = "btnSelectDir";
            this.btnSelectDir.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDir.TabIndex = 2;
            this.btnSelectDir.Text = "Select...";
            this.btnSelectDir.UseVisualStyleBackColor = true;
            this.btnSelectDir.Click += new System.EventHandler(this.btnSelectDir_Click);
            // 
            // txtZipDir
            // 
            this.txtZipDir.Location = new System.Drawing.Point(69, 45);
            this.txtZipDir.Name = "txtZipDir";
            this.txtZipDir.Size = new System.Drawing.Size(379, 20);
            this.txtZipDir.TabIndex = 4;
            // 
            // txtSourceDir
            // 
            this.txtSourceDir.Location = new System.Drawing.Point(69, 19);
            this.txtSourceDir.Name = "txtSourceDir";
            this.txtSourceDir.Size = new System.Drawing.Size(379, 20);
            this.txtSourceDir.TabIndex = 0;
            // 
            // btnSelectZipDir
            // 
            this.btnSelectZipDir.Location = new System.Drawing.Point(454, 45);
            this.btnSelectZipDir.Name = "btnSelectZipDir";
            this.btnSelectZipDir.Size = new System.Drawing.Size(75, 23);
            this.btnSelectZipDir.TabIndex = 3;
            this.btnSelectZipDir.Text = "Select Zip";
            this.btnSelectZipDir.UseVisualStyleBackColor = true;
            this.btnSelectZipDir.Click += new System.EventHandler(this.btnSelectZipDir_Click);
            // 
            // btnZipDir
            // 
            this.btnZipDir.Location = new System.Drawing.Point(69, 71);
            this.btnZipDir.Name = "btnZipDir";
            this.btnZipDir.Size = new System.Drawing.Size(75, 23);
            this.btnZipDir.TabIndex = 1;
            this.btnZipDir.Text = "Zip Dir";
            this.btnZipDir.UseVisualStyleBackColor = true;
            this.btnZipDir.Click += new System.EventHandler(this.btnZipDir_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.btnSelectDestDir);
            this.groupBox3.Controls.Add(this.txtDestDir);
            this.groupBox3.Controls.Add(this.txtZipUnzip);
            this.groupBox3.Controls.Add(this.btnSelectZipUnzip);
            this.groupBox3.Controls.Add(this.btnUnzip);
            this.groupBox3.Location = new System.Drawing.Point(12, 260);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(539, 102);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Unzip";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Zip File";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Dest Dir";
            // 
            // btnSelectDestDir
            // 
            this.btnSelectDestDir.Location = new System.Drawing.Point(454, 46);
            this.btnSelectDestDir.Name = "btnSelectDestDir";
            this.btnSelectDestDir.Size = new System.Drawing.Size(75, 23);
            this.btnSelectDestDir.TabIndex = 2;
            this.btnSelectDestDir.Text = "Select...";
            this.btnSelectDestDir.UseVisualStyleBackColor = true;
            this.btnSelectDestDir.Click += new System.EventHandler(this.btnSelectDestDir_Click);
            // 
            // txtDestDir
            // 
            this.txtDestDir.Location = new System.Drawing.Point(69, 45);
            this.txtDestDir.Name = "txtDestDir";
            this.txtDestDir.Size = new System.Drawing.Size(379, 20);
            this.txtDestDir.TabIndex = 4;
            // 
            // txtZipUnzip
            // 
            this.txtZipUnzip.Location = new System.Drawing.Point(69, 19);
            this.txtZipUnzip.Name = "txtZipUnzip";
            this.txtZipUnzip.Size = new System.Drawing.Size(379, 20);
            this.txtZipUnzip.TabIndex = 0;
            // 
            // btnSelectZipUnzip
            // 
            this.btnSelectZipUnzip.Location = new System.Drawing.Point(454, 17);
            this.btnSelectZipUnzip.Name = "btnSelectZipUnzip";
            this.btnSelectZipUnzip.Size = new System.Drawing.Size(75, 23);
            this.btnSelectZipUnzip.TabIndex = 3;
            this.btnSelectZipUnzip.Text = "Select Zip";
            this.btnSelectZipUnzip.UseVisualStyleBackColor = true;
            this.btnSelectZipUnzip.Click += new System.EventHandler(this.btnSelectZipUnzip_Click);
            // 
            // btnUnzip
            // 
            this.btnUnzip.Location = new System.Drawing.Point(69, 71);
            this.btnUnzip.Name = "btnUnzip";
            this.btnUnzip.Size = new System.Drawing.Size(75, 23);
            this.btnUnzip.TabIndex = 1;
            this.btnUnzip.Text = "Unzip";
            this.btnUnzip.UseVisualStyleBackColor = true;
            this.btnUnzip.Click += new System.EventHandler(this.btnUnzip_Click);
            // 
            // linkKellerman
            // 
            this.linkKellerman.AutoSize = true;
            this.linkKellerman.Location = new System.Drawing.Point(13, 373);
            this.linkKellerman.Name = "linkKellerman";
            this.linkKellerman.Size = new System.Drawing.Size(145, 13);
            this.linkKellerman.TabIndex = 9;
            this.linkKellerman.TabStop = true;
            this.linkKellerman.Text = "Free from Kellerman Software";
            this.linkKellerman.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkKellerman_LinkClicked);
            // 
            // Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 398);
            this.Controls.Add(this.linkKellerman);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Demo";
            this.Text = "Sharp Zip Wrapper Demo";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.Button btnZipFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnSelectZip;
        private System.Windows.Forms.TextBox txtZipFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelectDir;
        private System.Windows.Forms.TextBox txtZipDir;
        private System.Windows.Forms.TextBox txtSourceDir;
        private System.Windows.Forms.Button btnSelectZipDir;
        private System.Windows.Forms.Button btnZipDir;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSelectDestDir;
        private System.Windows.Forms.TextBox txtDestDir;
        private System.Windows.Forms.TextBox txtZipUnzip;
        private System.Windows.Forms.Button btnSelectZipUnzip;
        private System.Windows.Forms.Button btnUnzip;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.SaveFileDialog saveDialog;
        private System.Windows.Forms.LinkLabel linkKellerman;
    }
}

