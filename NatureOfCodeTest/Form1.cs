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
        Random rnd = new Random();
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
            timer.Interval = 200;
            timer.Tick += Timer_Tick;
            this.Paint += Form1_Paint;
            for (int i = 0; i < 100; i++)
            {
                Vector2 pos = new Vector2();
                //pos = RandomUnitVector() * rand.Next(300, 500);
                pos = new Vector2(rnd.Next(this.Width/2), rnd.Next(this.Height/2));
                Vector2 velo = /*new Vector2(0, 10)*/ RandomUnitVector() * rnd.Next(15, 20);
                objectList.Add(new Body(
                    this.Width, this.Height, this, 
                    pos,
                    velo, 
                    15f, 25f
                    ));
            }
            sun = new Body(this.Width, this.Height, this, 
                new Vector2(this.Width / 2, this.Height / 2), 
                new Vector2(0, 0), 
                500f, 200f);
        }
        public Vector2 RandomUnitVector()
        {
            double random = rnd.Next(0, 260); 
            return new Vector2((float)(Math.Cos(random)), (float)Math.Sin(random));
        }
        //change to database
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].AttractTo(sun);
                for (int j = 0; j < objectList.Count; j++)
                {
                    if (j != i)
                    {
                        objectList[i].AttractTo(objectList[j]);
                    }

                }

            }
            //foreach (var body in objectList)
            //{
            //    body.AttractTo(sun);
            //    foreach (var other in objectList)
            //    {
            //        if (body != other)
            //        {
            //            body.AttractTo(other);
            //        }
            //    }
            //}
            foreach (var item in objectList)
            {
                item.Update();
                item.Display(e.Graphics);
            }
            //sun.Display(e.Graphics);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
