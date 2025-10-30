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
            velocity = new Vector2();
            acceleration = new Vector2();
            brushesColor = new SolidBrush(Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
            frm = theForm;
            topSpeed = 15;
        }
        public void ApplyForce(string forceName)
        {

        }
        public void Update()
        {

            Point cp = frm.PointToClient(Cursor.Position);
            mouse.X = cp.X;
            mouse.Y = cp.Y;
            Vector2 dir = Vector2.Subtract(mouse, position);
            dir = Vector2.Normalize(dir);
            dir = Vector2.Multiply(0.4f, dir);
            acceleration = dir;
            velocity = Vector2.Add(velocity, acceleration);
            position = Vector2.Add(position, velocity);
            if (position.X < 2 || position.X > this.formWidth - 70)
            {

                velocity.X = velocity.X * -1;
            }
            if (position.Y < 2 || position.Y > this.formHeight - 70)
            {
                velocity.Y = velocity.Y * -1;
            }

        }
        public void Display(Graphics e)
        {
            e.FillEllipse(brushesColor, position.X, position.Y, 70, 70);
        }
    }
}
