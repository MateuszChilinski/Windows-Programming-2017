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
        public Form2(LibraryManager lib, Bitmap b)
        {
            myLib = lib;
            InitializeComponent();
            Size = new Size(b.Width, b.Height);
            pictureBox1.Image = b;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            filePath = "sth";
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if(!saved)
            {
                //error lol displaybox
            }
            else
            {
                myLib.addNewImage(new Bitmap(pictureBox1.Image), filePath);
            }
        }
    }
}
