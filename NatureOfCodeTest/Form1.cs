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
        Body sun, objectB;
        List<Body> objectList = new List<Body>();
        Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;

            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            this.Paint += Form1_Paint;
            for (int i = 0; i < 20; i++)
            {
                objectList.Add(new Body(
                    this.Width, this.Height, this, 
                    new Vector2(rand.Next(this.Width/4, this.Width*3/5), rand.Next(this.Height/4, this.Height*3/5)),
                    new Vector2(0, 0), 
                    (float)(50), 50f
                    ));
            }
            sun = new Body(this.Width, this.Height, this, 
                new Vector2(this.Width / 2, this.Height / 2), 
                new Vector2(0, 0), 
                30000f, 200f);
        }
        //change to database
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            for (int i = 0; i < objectList.Count; i++)
            {
                for (int j = 0; j < objectList.Count; j++)
                {
                    if (j != i)
                    {
                        objectList[i].AttractTo(objectList[j]);

                    }

                }
                objectList[i].AttractTo(sun);
                objectList[i].Update();
                objectList[i].Display(e.Graphics);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
