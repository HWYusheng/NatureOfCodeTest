using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NatureOfCodeTest
{
    internal class Mover
    {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 acceleration;
        public float mass;
        public Mover()
        {
            position = new Vector2();
            velocity = new Vector2();
            acceleration = new Vector2();
        }
        public void ApplyForce(string forceName)
        {

        }
        public void Update()
        {
        }
    }
}
