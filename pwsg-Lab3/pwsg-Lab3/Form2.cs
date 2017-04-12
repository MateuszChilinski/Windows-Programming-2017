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
    }
}
