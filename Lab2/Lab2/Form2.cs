using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public int Life
        {
            get
            {
                return (int) this.numericUpDown1.Value;
            }
            set
            {
                this.numericUpDown1.Value = value;
            }
        }
        public int Time
        {
            get
            {
                return (int) this.numericUpDown2.Value;
            }
            set
            {
                this.numericUpDown2.Value = value;
            }
        }
    }
}
