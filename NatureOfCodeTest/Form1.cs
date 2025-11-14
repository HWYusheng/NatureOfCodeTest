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
            for (int i = 0; i < 5; i++)
            {
                objectList.Add(new Body(
                    this.Width, this.Height, this, 
                    new Vector2(rand.Next(1, this.Width), rand.Next(1, this.Height/2)),
                    new Vector2(0, 0), 
                    (float)(rand.Next(2, 5))
                    ));
            }
            objectA = new Body(this.Width, this.Height, this, new Vector2(this.Width / 2, this.Height / 2), new Vector2(1, 0), 1);
            objectB = new Body(this.Width, this.Height, this, new Vector2(this.Width / 2, this.Height / 2), new Vector2(-1, 0), 1);
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Vector2 wind = new Vector2(7, 0);
            Vector2 gravity = new Vector2(0, 10);

            for (int i = 0; i < objectList.Count; i++)
            {
                for (int j = 0; j < objectList.Count; j++)
                {
                    if (j != i)
                    {
                        Vector2 force = objectList[i].AttractTo(objectList[j]);
                        objectList[i].ApplyForce(force);
                        objectList[i].checkEdge();

                    }

                }
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
