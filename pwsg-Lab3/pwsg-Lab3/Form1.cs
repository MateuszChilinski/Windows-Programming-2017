using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pwsg_Lab3
{
    public partial class Form1 : Form
    {
        bool firstDone = false;
        bool secondDone = false;
        bool firstBar = false;
        bool secondBar = false;
        LibraryManager myLib;
        Bitmap b1;
        Bitmap b2;
        PictureBoxExtra selectedBox;
        bool isSelected = false;
        public Form1()
        {
            myLib = new LibraryManager("imgLibrary.xml");
            InitializeComponent();
            myLib.initializePictures(flowLayoutPanel1);
        }
        private void ActualBlend(int bar)
        {
            double alfa = 0.5;
            Bitmap b1_t=null, b2_t=null;
            this.Invoke((MethodInvoker)delegate {
                alfa = trackBar1.Value / 11.0;
                b1_t = (Bitmap) b1.Clone();
                b2_t = (Bitmap) b2.Clone();
            });
            Bitmap b3 = new Bitmap(Math.Min(b1_t.Width, b2_t.Width), Math.Min(b1_t.Height, b2_t.Height));
            for(int i = 0; i < b3.Width; i++)
            {
                this.Invoke((MethodInvoker)delegate {
                    if(bar == 1)
                        progressBar1.Value = (int) (i*1.0 / (b3.Width-1)*1.0 * 100);
                    else
                        progressBar2.Value = (int)(i * 1.0 / (b3.Width-1) * 1.0 * 100);
                });
                for(int j = 0; j < b3.Height; j++)
                {
                    byte newR = (byte)(alfa * b1_t.GetPixel(i, j).R + (1 - alfa) * b2_t.GetPixel(i, j).R);
                    byte newG = (byte)(alfa * b1_t.GetPixel(i, j).G + (1 - alfa) * b2_t.GetPixel(i, j).G);
                    byte newB = (byte)(alfa * b1_t.GetPixel(i, j).B + (1 - alfa) * b2_t.GetPixel(i, j).B);
                    Color newC = Color.FromArgb(newR, newG, newB);
                    b3.SetPixel(i,j,newC);
                }
            }
            this.Invoke((MethodInvoker)delegate {
                var myForm = new Form2(myLib, b3);
                myForm.Show();
            });

        }
        private void Blend()
        {
            int b = -1;
            this.Invoke((MethodInvoker)delegate
            {
                if (progressBar1.Value == 0 && firstBar == true)
                    b = 1;
                else
                    b = 2;
            });
            ActualBlend(b);
            this.Invoke((MethodInvoker)delegate {
                if (progressBar1.Value == 100)
                {
                    firstBar = false;
                    if (!firstBar && !secondBar)
                        label1.Visible = false;
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    button1.Enabled = true;
                }
                else
                {
                    secondBar = false;
                    if(!firstBar && !secondBar)
                        label1.Visible = false;
                    progressBar2.Visible = false;
                    progressBar2.Value = 0;
                    button1.Enabled = true;
                }
            });
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(firstBar && secondBar)
            {
                return;
            }
            if (firstBar || secondBar)
            {
                button1.Enabled = false;
                if (firstBar)
                {
                    progressBar2.Visible = true;
                    secondBar = true;
                }
                else
                {
                    progressBar1.Visible = true;
                    firstBar = true;
                }
            }
            else
            {
                progressBar1.Visible = true;
                firstBar = true;
            }
            label1.Visible = true;
            Thread thread = new Thread(Blend);
            thread.Start();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            using (var selectFileDialog = new OpenFileDialog())
            {
                if (selectFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(selectFileDialog.FileName);
                    firstDone = true;
                    if (firstDone && secondDone)
                        button1.Enabled = true;

                    b1 = new Bitmap(selectFileDialog.FileName);
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F12))
            {
                Graphics graph = null;
                try
                {
                    Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    graph = Graphics.FromImage(bmp);
                    graph.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                    if (!firstDone)
                    {
                        b1 = bmp;
                        pictureBox1.Image = bmp;
                    }
                    else
                    {
                        b2 = bmp;
                        secondDone = true;
                        pictureBox2.Image = bmp;
                    }
                    firstDone = true;
                    if (firstDone && secondDone)
                        button1.Enabled = true;
                }
                finally
                {
                    graph.Dispose();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            using (var selectFileDialog = new OpenFileDialog())
            {
                if (selectFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox2.Image = new Bitmap(selectFileDialog.FileName);
                    b2 = new Bitmap(selectFileDialog.FileName);
                    secondDone = true;
                    if (firstDone && secondDone)
                        button1.Enabled = true;
                }
            }
        }
    }
}
