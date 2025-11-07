using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NatureOfCodeTest
{
    public partial class Form1 : Form
    {
        Mover objectA;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;

            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            this.Paint += Form1_Paint;
            objectA = new Mover(this.Width, this.Height, this, new Vector2(this.Width / 2, this.Height / 2), new Vector2(0, 0), 1);
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Vector2 wind = new Vector2(7, 0);
            Vector2 gravity = new Vector2(0, 2);
            objectA.ApplyForce(wind);
            objectA.Display(e.Graphics);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
