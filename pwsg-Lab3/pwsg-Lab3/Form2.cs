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
        public Form2(Bitmap b)
        {
            InitializeComponent();
            Size = new Size(b.Width, b.Height);
            pictureBox1.Image = b;
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {

        }

        private void menuItem2_Click(object sender, EventArgs e)
        {

        }
    }
}
