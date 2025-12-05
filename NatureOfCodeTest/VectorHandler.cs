using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace NatureOfCodeTest
{
    internal class VectorHandler
    {
        Random rand = new Random();
        public Vector2 RandomUnitVector()
        {
            double random = rand.Next(0, 260);
            return new Vector2((float)(Math.Cos(random)), (float)Math.Sin(random));
        }
    }

    
}
