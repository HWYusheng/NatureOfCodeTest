using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest.Model
{
    public class RadialVelocityCalculator
    {
        public Vector2 LineOfSight { get; set; } = new Vector2(0, 1); // Mặc định nhìn theo trục Y
        public double Dot(Vector2 other, Vector2 prime) { 
           return prime.X * other.X + prime.Y * other.Y; 
        }
        public double Compute(Vector2 starVelocity)
        {
            return Dot(LineOfSight, starVelocity);
        }
    }
}
