using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest.Model
{
    internal class SimulationSample
    {
        public double Time { get; set; }
        public Vector2 StarPosition { get; set; }
        public Vector2 PlanetPosition { get; set; }
        public double RadialVelocity { get; set; }
    }
}
