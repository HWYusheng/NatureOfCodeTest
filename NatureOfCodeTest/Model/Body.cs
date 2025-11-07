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
    internal class Body
    {
        static Random rnd = new Random();
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;
        Vector2 mouse = new Vector2();
        SolidBrush brushesColor;
        int formWidth, formHeight;
        double topSpeed;
        Form frm;
        float mass, radius = 70;
        public Body(int width, int height, Form theForm, Vector2 pos, Vector2 velo, float m)
        {
            formHeight = height;
            formWidth = width;
            position = pos;
            velocity = velo;
            acceleration = new Vector2(0, 0);
            mass = m;
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
            this.acceleration *= 0;
        }
        public void Display(Graphics e)
        {
            e.FillEllipse(brushesColor, position.X, position.Y, radius, radius);
        }
        public void checkEdge()
        {
            if (this.position.X > formWidth)
            {
                this.position.X = formWidth;
                this.velocity.X *= -1;
            }
            else if (this.position.X < radius)
            {
                this.velocity.X *= -1;
                this.position.X = 0;
            }
            if (this.position.Y > formHeight-radius)
            {
                this.velocity.Y *= -1;
                this.position.Y = formHeight-radius;
            }

        }
    }
}
