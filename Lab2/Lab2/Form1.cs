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
        enum status {
            LOST,
            NORMAL,
            EDIT
        };
        int maxSec = 10;
        int currSec = 10;
        int score = 0;
        int lifes = 3;
        int maxLifes = 3;
        int guessedButtons = 0;
        int activeButtons = 0;
        bool[,] active = new bool[5,5];
        status gameMode;
        System.Windows.Forms.Label[,] labels = new System.Windows.Forms.Label[2,5]; // 0->x 1->y
        System.Windows.Forms.Button[,] buttons = new System.Windows.Forms.Button[5, 5];
        private void refresheButtons()
        {
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    buttons[i, j].BackColor = System.Drawing.Color.RoyalBlue;
                    buttons[i, j].Text = "?";
                }
            }
        }
        private void initializeButtons()
        {
            refresheButtons();
            for (int i = 1; i <= 4; i++)
            {
                for(int j = 1; j <= 4; j++)
                {
                    buttons[i, j].Font = new System.Drawing.Font("Arial", 16F);
                    buttons[i, j].Dock = System.Windows.Forms.DockStyle.Fill;
                    buttons[i, j].UseVisualStyleBackColor = false;
                    buttons[i, j].MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_clicked);
                    buttons[i, j].MouseEnter += new System.EventHandler(this.button_hover);
                    buttons[i, j].MouseLeave += new System.EventHandler(this.button_unhover);
                }
            }
        }
        private void endGame()
        {
            gameMode = status.LOST;
            string message =
"Your final score is " + score.ToString() + ".";
            const string caption = "End of game";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Question);
        }
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
            labels[0, 1] = this.label1;
            labels[0, 2] = this.label2;
            labels[0, 3] = this.label3;
            labels[0, 4] = this.label4;
            labels[1, 1] = this.label5;
            labels[1, 2] = this.label6;
            labels[1, 3] = this.label7;
            labels[1, 4] = this.label8;
            initializeButtons();
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
            if ((labels[0, 1].Text == "" && gameMode != status.EDIT) || gameMode == status.LOST)
                return;
            Button currButton = (Button)sender;
            MouseEventArgs me = (MouseEventArgs)e;
            int x, y;
            GetXY(currButton, out x, out y);
            switch (me.Button)
            {
                case MouseButtons.Left:
                    {
                        if(gameMode == status.EDIT)
                        {
                            active[x, y] = true;
                            currButton.BackColor = System.Drawing.Color.Black;
                            return;
                        }
                        if (active[x, y] && currButton.BackColor != System.Drawing.Color.Black)
                        {
                            score += 50;
                            if(++guessedButtons == activeButtons)
                            {
                                score += 500;
                                label10.Text = "Score: " + score.ToString();
                                newGame();
                                return;
                            }
                            label10.Text = "Score: " + score.ToString();
                            currButton.BackColor = System.Drawing.Color.Black;
                        }
                        else if(!active[x, y])
                        {
                            currButton.BackColor = System.Drawing.Color.Red;
                            lifes--;
                            label9.Text = "Lifes: " + lifes.ToString();
                            if(lifes == 0)
                            {
                                endGame();
                            }
                        }
                    }
                    break;

                case MouseButtons.Right:
                    if (gameMode == status.EDIT)
                    {
                        active[x, y] = true;
                        currButton.BackColor = System.Drawing.Color.White;
                        return;
                    }
                    if (currButton.BackColor != System.Drawing.Color.Black)
                    currButton.BackColor = System.Drawing.Color.White;
                    break;
            }
            
        }
        private void button_hover(object sender, EventArgs e)
        {
            Button currButton = (Button)sender;
            if (currButton.BackColor == System.Drawing.Color.RoyalBlue)
            {
                currButton.Text = "";
                currButton.BackColor = System.Drawing.Color.Yellow;
            }
        }
        private void button_unhover(object sender, EventArgs e)
        {
            Button currButton = (Button)sender;
            if (currButton.BackColor == System.Drawing.Color.Yellow)
            {
                currButton.Text = "?";
                currButton.BackColor = System.Drawing.Color.RoyalBlue;
            }
        }
        private void newGame(bool loaded=false)
        {
            refresheButtons();
            activeButtons = 0;
            guessedButtons = 0;
            lifes = maxLifes;
            Random rnd = new Random();
            for(int i = 1; i<=4; i++)
            {
                for (int j = 1; j <= 4 && !loaded; j++)
                    active[i, j] = false;
                labels[0, i].Text = (0).ToString();
                labels[1, i].Text = (0).ToString();
            }
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                        if (!loaded && rnd.Next(1, 4) == 1 || (loaded && active[i, j]))
                        {
                            if (!loaded)
                                active[i, j] = true;
                            activeButtons++;
                            labels[0, i].Text = (1 + (Int32.Parse(labels[0, i].Text))).ToString();
                            labels[1, j].Text = (1 + (Int32.Parse(labels[1, j].Text))).ToString();
                        }
                }
            }

        }
        private void edit()
        {
            activeButtons = 0;
            guessedButtons = 0;
            lifes = maxLifes;
            for (int i = 1; i <= 4; i++)
            {
                labels[0, i].Text = "";
                labels[1, i].Text = "";
            }
            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    active[i, j] = false;
                    buttons[i, j].BackColor = System.Drawing.Color.White;
                    buttons[i, j].Text = "";
                }
            }
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameMode = status.NORMAL;
            newGame();
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Form2 dialbox = new Form2())
            {
                dialbox.Show();
            }
            // somehow handle the DisplayResult, google
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameMode = status.EDIT;
            edit();
        }

        private void gameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gameMode = status.NORMAL;
            newGame(true);
        }
        private void timerDisplay()
        {
            if(currSec == 0)
            {
                endGame();
            }
        }
    }
}
