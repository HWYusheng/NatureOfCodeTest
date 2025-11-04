using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NatureOfCodeTest
{
    internal class Mover
    {
        static Random rnd = new Random();
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        Vector2 mouse = new Vector2();
        SolidBrush brushesColor;
        int formWidth, formHeight;
        float topSpeed;
        Form frm;
        public float mass;
        public Mover(int width, int height, Form theForm)
        {
            formHeight = height;
            formWidth = width;
            position = new Vector2();
            velocity = new Vector2(0, 0);
            acceleration = new Vector2(0, 0);
            brushesColor = new SolidBrush(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            frm = theForm;
            topSpeed = 15;
        }
        public void ApplyForce(Vector2 force)
        {
            this.acceleration += force / mass;
            this.Update();
        }
        public void Update()
        {
            this.velocity += this.acceleration;
            this.position += this.velocity;
            checkEdge();
            this.acceleration *= 0;
        }
        public void Display(Graphics e)
        {
            e.FillEllipse(brushesColor, position.X, position.Y, 70, 70);
        }
        private void checkEdge()
        {
            if (this.position.X > formWidth)
            {
                this.position.X = formWidth;
                this.velocity.X *= -1;
            }
            else if (this.position.X < 0)
            {
                this.velocity.X *= -1;
                this.position.X = 0;
            }
            if (this.position.Y > formHeight)
            {
                this.velocity.Y *= -1;
                this.position.Y = formHeight;
            }
        }
    }
}
