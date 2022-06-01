using Base.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Crypt.Tool
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void lvItems_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link;
        }

        private void lvItems_DragDrop(object sender, DragEventArgs e)
        {
            foreach (string FilePath in (e.Data.GetData(DataFormats.FileDrop) as string[]))
            {
                ListViewItem Item = new ListViewItem();
                Item.Text = FilePath;

                lvItems.Items.Add(Item);
            }
        }

        private void lvItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Delete && lvItems.SelectedItems.Count > 0)
            {
                foreach (int Index in lvItems.SelectedIndices)
                    lvItems.Items.RemoveAt(Index);
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in lvItems.SelectedItems)
            {
                byte[] Data = File.ReadAllBytes(Item.Text);
                byte[] CData = RijndaelHelper.Encrypt(Data);

                File.Move(Item.Text, Item.Text + $".bak_{DateTime.Now}".Replace(':', '_').Replace('\\', '_').Replace('/', '_'));

                using (FileStream Stream = File.Create(Item.Text))
                    Stream.Write(CData, 0, CData.Length);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in lvItems.SelectedItems)
            {
                byte[] Data = File.ReadAllBytes(Item.Text);
                byte[] CData = RijndaelHelper.Decrypt(Data);

                File.Move(Item.Text, Item.Text + $".bak_{DateTime.Now}".Replace(':', '_').Replace('\\', '_').Replace('/', '_'));

                using (FileStream Stream = File.Create(Item.Text))
                    Stream.Write(CData, 0, CData.Length);
            }
        }
    }
}