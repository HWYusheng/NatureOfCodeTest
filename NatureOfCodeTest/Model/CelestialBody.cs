using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static NatureOfCodeTest.Model.CelestialBody;

namespace NatureOfCodeTest.Model
{

        public abstract class CelestialBody
        {
            public const double G = 6.67430e-11;
            public const double SpeedOfLight = 299792458;
            public const double SolarMass = 1.989e30;
            public const double AU = 1.496e11;
            public string Name { get; set; }
            public double Mass { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 Velocity { get; set; }

            public virtual void UpdatePosition(double dt)
            {
                Position += Vector2.Multiply((float)dt, Velocity);
            }
        }

        public class Star : CelestialBody
        {
            public double Luminosity { get; set; }
            public List<Vector2> VelocityHistory { get; private set; } = new List<Vector2>();

            public void RecordVelocity() => VelocityHistory.Add(Velocity);
        }
        public class Planet : CelestialBody
        {
            public OrbitalElements Orbit { get; set; }
        }
}
