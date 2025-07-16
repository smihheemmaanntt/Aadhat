using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KellermanSoftware.SharpZipWrapper;

namespace SharpZipWrapperDemo
{
    public partial class Demo : Form
    {
        public Demo()
        {
            InitializeComponent();

            linkKellerman.Links.Add(0, linkKellerman.Text.Length, "http://www.kellermansoftware.com");
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSourceFile.Text = openFileDialog1.FileName;
            }

        }

        private void btnSelectZip_Click(object sender, EventArgs e)
        {
            SelectZip();
        }

        private void SelectZip()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Zip files (*.zip)|*.zip";
            saveFileDialog1.FilterIndex = 1;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtZipFile.Text = saveFileDialog1.FileName;
                txtZipDir.Text = saveFileDialog1.FileName;
                txtZipUnzip.Text = saveFileDialog1.FileName;
            }
        }

        private void btnZipFile_Click(object sender, EventArgs e)
        {
            ZipHelper oZipHelper = new ZipHelper();
            oZipHelper.AddFilesToZip(txtZipFile.Text,
                System.IO.Path.GetDirectoryName(txtSourceFile.Text),
                System.IO.Path.GetFileName(txtSourceFile.Text), false, "");
            MessageBox.Show("Finished");
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtSourceDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnSelectZipDir_Click(object sender, EventArgs e)
        {
            SelectZip();
        }

        private void btnZipDir_Click(object sender, EventArgs e)
        {
            ZipHelper oZipHelper = new ZipHelper();
            oZipHelper.AddFilesToZip(txtZipFile.Text,txtSourceDir.Text,"*.*",true,"");
            MessageBox.Show("Finished");
        }

        private void btnSelectZipUnzip_Click(object sender, EventArgs e)
        {
            SelectZip();
        }

        private void btnSelectDestDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDestDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnUnzip_Click(object sender, EventArgs e)
        {
            ZipHelper oZipHelper = new ZipHelper();
            oZipHelper.ExtractFilesFromZip(txtZipFile.Text,txtDestDir.Text,"");
            MessageBox.Show("Finished");
        }

        private void linkKellerman_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }


    }
}