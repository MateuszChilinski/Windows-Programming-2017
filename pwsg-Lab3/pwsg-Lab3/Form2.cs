using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pwsg_Lab3
{
    public partial class Form2 : Form
    {
        bool saved = false;
        string filePath;
        LibraryManager myLib;
        Bitmap myBitmap;
        public Form2(LibraryManager lib, Bitmap b)
        {
            myLib = lib;
            InitializeComponent();
            Size = new Size(b.Width, b.Height);
            myBitmap = b;
            pictureBox1.Image = b;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            filePath = "image"+LibraryManager.currentImage;
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = filePath;
            save.DefaultExt = ".bmp";
            save.Filter = "Bitmap (*.bmp)|*.bmp";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                myBitmap.Save(save.FileName);
                LibraryManager.currentImage++;
                saved = true;
            }
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if(!saved)
            {
                MessageBox.Show("You need to save the picture before adding it to the library.", "Error!",
         MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                myLib.addNewImage(myBitmap, filePath);
            }
        }
    }
}
