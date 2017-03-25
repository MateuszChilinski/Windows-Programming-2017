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
        int[,] labels = new int[2,5]; // 0->x 1->y
        System.Windows.Forms.Button[,] buttons = new System.Windows.Forms.Button[5, 5];
        public Form1()
        {
            InitializeComponent();
            buttons[1, 1] = button1;
            buttons[2, 1] = button2;
            buttons[3, 1] = button3;
            buttons[4, 1] = button4;
            buttons[1, 2] = button5;
            buttons[2, 2] = button6;
            buttons[3, 2] = button7;
            buttons[4, 2] = button8;
            buttons[1, 3] = button9;
            buttons[2, 3] = button10;
            buttons[3, 3] = button11;
            buttons[4, 3] = button12;
            buttons[1, 4] = button13;
            buttons[2, 4] = button14;
            buttons[3, 4] = button15;
            buttons[4, 4] = button16;
        }
        private void GetXY(Button b, out int x, out int y)
        {
            x = -1;
            y = -1;
            for(int i = 1; i <= 4; i++)
            {
                for(int j = 1; j <= 4; j++)
                {
                    if(buttons[i,j] == b)
                    {
                        x = i;
                        y = j;
                        return;
                    }
                }
            }
        }
        private void button_clicked(object sender, EventArgs e)
        {
            Button currButton = (Button)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            switch (me.Button)
            {

                case MouseButtons.Left:
                    {
                        int x, y;
                        GetXY(currButton, out x, out y);
                        int sumX=0,sumY=0;
                        for(int i = 1; i <= 4; i++)
                        {
                            Button cb =  buttons[x,i];
                            if (cb.BackColor == System.Drawing.Color.Black)
                                sumX++;
                        }
                        for (int i = 1; i <= 4; i++)
                        {
                            Button cb = buttons[i,y];
                            if (cb.BackColor == System.Drawing.Color.Black)
                                sumY++;
                        }
                        int labX = labels[0,x];
                        int labY = labels[1,y];
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
            for(int i = 0; i <= 4; i++)
            {
                labels[0, i] = rnd.Next(0, 4);
                labels[1, i] = rnd.Next(0, 4);

            }
            this.label1.Text = labels[0, 1].ToString();
            this.label2.Text = labels[0, 2].ToString();
            this.label3.Text = labels[0, 3].ToString();
            this.label4.Text = labels[0, 4].ToString();
            this.label5.Text = labels[1, 1].ToString();
            this.label6.Text = labels[1, 2].ToString();
            this.label7.Text = labels[1, 3].ToString();
            this.label8.Text = labels[1, 4].ToString();
        }
        private void Form1_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            const string message =
                "Are you sure that you would like to close the game?";
            const string caption = "Close the application?";
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
