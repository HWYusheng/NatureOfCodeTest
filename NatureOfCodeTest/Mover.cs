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
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 acceleration;
        public Mover(float x, float y)
        {
            this.position = new Vector2(x, y);
        }
        public void ApplyForce(float fx, float fy)
        {

        }
        public void Update()
        {
        }
    }
}
