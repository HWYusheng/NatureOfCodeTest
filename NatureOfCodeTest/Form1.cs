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
        Body objectA, objectB;
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
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
            this.Paint += Form1_Paint;
            for (int i = 0; i < 3; i++)
            {
                objectList.Add(new Body(
                    this.Width, this.Height, this, 
                    new Vector2(rand.Next(1, this.Width), rand.Next(1, this.Height/2)),
                    new Vector2(30, -20), 
                    (float)(rand.Next(10, 20)), 70f
                    ));
            }
            objectA = new Body(this.Width, this.Height, this, 
                new Vector2(this.Width / 2, this.Height / 2), 
                new Vector2(0, 0), 
                300, 200f);
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            for (int i = 0; i < objectList.Count; i++)
            {
                for (int j = 0; j < objectList.Count; j++)
                {
                    if (j != i)
                    {
                        Vector2 force = objectList[i].AttractTo(objectList[j]);
                        objectList[i].ApplyForce(force);

                    }

                }
                Vector2 mforce = objectList[i].AttractTo(objectA);
                objectList[i].ApplyForce(mforce);
                objectList[i].Update();
                objectList[i].Display(e.Graphics);
            }
            objectA.Display(e.Graphics);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
