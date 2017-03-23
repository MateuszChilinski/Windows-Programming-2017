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
    
    public partial class Form1 : Form
    {
        int[] labels = new int[8];
        public Form1()
        {
            InitializeComponent();
        }
        private int getX(int id)
        {
            return id % 4;
        }
        private int getY(int id)
        {
            if (id <= 4)
                return 1;
            if (id <= 8)
                return 2;
            if (id <= 12)
                return 3;
            if (id <= 16)
                return 4;
            return -1;
        }
        private int getNum(int x, int y)
        {
            return x+4*(y-1); 
        }
        private void button_clicked(object sender, EventArgs e)
        {
            Button currButton = (Button)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            switch (me.Button)
            {

                case MouseButtons.Left:
                    {
                        int id = Int32.Parse(currButton.Name.Substring(6, currButton.Name.Length - 6));
                        int x=getX(id), y=getY(id);
                        int sumX=0,sumY=0;
                        for(int i = 1; i <= 4; i++)
                        {
                            int num = getNum(x, i);
                            Button cb =  buttons[num];
                            if (cb.BackColor == System.Drawing.Color.Black)
                                sumX++;
                        }
                        for (int i = 1; i <= 4; i++)
                        {
                            Button cb = buttons[getNum(i, y)];
                            if (cb.BackColor == System.Drawing.Color.Black)
                                sumY++;
                        }
                        int labX = labels[x];
                        int labY = labels[y+4];
                        if (sumX < labX && sumY < labY)
                            currButton.BackColor = System.Drawing.Color.Black;
                        else
                        {
                            const string message =
    "Wrong!";
                            const string caption = "Wrong";
                            var result = MessageBox.Show(message, caption,
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Question);
                        }
                    }
                    break;

                case MouseButtons.Right:
                    currButton.BackColor = System.Drawing.Color.White;
                    break;
            }
            
        }
        private void button_hover(object sender, EventArgs e)
        {
            Button currButton = (Button)sender;
            if(currButton.BackColor != System.Drawing.Color.Black)
                currButton.BackColor = System.Drawing.Color.Yellow;
        }
        private void button_unhover(object sender, EventArgs e)
        {
            Button currButton = (Button)sender;
            if (currButton.BackColor != System.Drawing.Color.Black)
                currButton.BackColor = System.Drawing.Color.White;
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            for(int i = 0; i < 8; i++)
            {
                labels[i] = rnd.Next(0, 4);
            }
            this.label1.Text = labels[0].ToString();
            this.label2.Text = labels[1].ToString();
            this.label3.Text = labels[2].ToString();
            this.label4.Text = labels[3].ToString();
            this.label5.Text = labels[4].ToString();
            this.label6.Text = labels[5].ToString();
            this.label7.Text = labels[6].ToString();
            this.label8.Text = labels[7].ToString();
        }
        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            const string message =
                "Are you sure that you would like to close the form?";
            const string caption = "Form Closing";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);
            
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
