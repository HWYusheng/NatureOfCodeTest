using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    internal class Plotting
    {
        public float[] RaxisAngle(Vector2 axis, float theta)
        {
            float cosTheta = (float)Math.Cos(Convert.ToDouble(theta));
            float sinTheta = (float)Math.Sin(Convert.ToDouble(theta));
            float[] data = {
                cosTheta + axis.X * axis.X * (1-cosTheta),
                axis.X * axis.Y -sinTheta,
            };
            return data;
        }
    }
}
