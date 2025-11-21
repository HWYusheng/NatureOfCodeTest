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
        public Vector2 position { get; private set; } = new Vector2(); 
        public Vector2 velocity { get; private set; }
        public Vector2 acceleration { get; private set; }
        Vector2 mouse = new Vector2();
        SolidBrush brushesColor;
        int formWidth, formHeight;
        double topSpeed;
        Form frm;
        float mass, radius, G = 1f;
        static Random rnd = new Random();
        public Body(int width, int height, Form theForm, Vector2 pos, Vector2 velo, float m, float dia)
        {
            formHeight = height;
            formWidth = width;
            radius = dia / 2;
            //pos.X += (float)(Math.Sqrt(2)*radius);
            //pos.Y += (float)(Math.Sqrt(2)*radius);
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
        public void AttractTo(Body AnotherBody)
        {
            Vector2 origin = new Vector2(0, 0);
            Vector2 gforce = -this.position + AnotherBody.position;
            float distanceSq = Mag(gforce);
            
            float strength = (G * this.mass * AnotherBody.mass) / (distanceSq*distanceSq);
            gforce = Vector2.Normalize(gforce) * strength;
            this.ApplyForce(gforce);
        }
        public void Update()
        {
            this.velocity += this.acceleration;
            this.position += this.velocity;
            this.acceleration *= 0;
        }
        public void Display(Graphics e)
        {
            e.FillEllipse(brushesColor, this.position.X - radius, this.position.Y - radius, 2*radius, 2*radius);
        }
        public void checkEdge()
        {
            Vector2 pos = this.position;
            Vector2 velo = this.velocity;
            if (pos.X > formWidth - radius)
            {
                pos.X = formWidth - radius;
                velo.X *= -1;
            }
            else if (pos.X < 0)
            {
                pos.X = 0;                
                velo.X *= -1;
                
            }
            if (this.position.Y > formHeight - radius)
            {
                pos.Y = formHeight - radius;
                velo.Y *= -1;                
            }
            this.position = pos;
            this.velocity = velo;
        }
        float Mag(Vector2 theVector)
        {
            return (float)Math.Sqrt(theVector.X * theVector.X + theVector.Y * theVector.Y);
        }

        public Vector2 Limit(Vector2 theVector, float lowerLimit, float upperLimit)
        {

            if (Mag(theVector) > upperLimit)
            {

                theVector.X = theVector.X * upperLimit / Mag(theVector);
                theVector.Y = theVector.Y * upperLimit / Mag(theVector);
            }
            if (Mag(theVector) < lowerLimit)
            {
                theVector.X = theVector.X * Mag(theVector) / lowerLimit;
                theVector.Y = theVector.Y * Mag(theVector) / lowerLimit;
            }
            return theVector;
        }
        public bool contactEdge()
        {
            if (this.position.X > formWidth - radius || this.position.X < 0 || this.position.Y > formHeight - radius)
            {
                return true;
            }
            return false;
        }
    }
}
