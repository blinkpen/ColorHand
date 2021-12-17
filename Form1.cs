using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ColorHand;
namespace ColorHand
{
    public partial class Form1 : Form
    {
        private int xpos;
        private int ypos;
        private Point pos = new Point();
        public bool running = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
       
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            xpos = Cursor.Position.X - this.Location.X;
            ypos = Cursor.Position.Y - this.Location.Y;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pos = MousePosition;
                pos.X = pos.X - xpos;
                pos.Y = pos.Y - ypos;
                this.Location = pos;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label3_MouseDown(object sender, MouseEventArgs e)
        {
            xpos = Cursor.Position.X - this.Location.X;
            ypos = Cursor.Position.Y - this.Location.Y;
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                pos = MousePosition;
                pos.X = pos.X - xpos;
                pos.Y = pos.Y - ypos;
                this.Location = pos;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.BackColor = colorLense2.selectedColor;
            textBox1.Text = $"{colorLense2.R}, {colorLense2.G}, {colorLense2.B}, {colorLense2.A}";
            textBox2.Text = colorLense2.hex;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(colorLense2.clock.Enabled)
            {
                colorLense2.clock.Stop();
            }
            else
            {
                colorLense2.clock.Start();                
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            colorLense2.UnHookMouse();
        }
    }
}
