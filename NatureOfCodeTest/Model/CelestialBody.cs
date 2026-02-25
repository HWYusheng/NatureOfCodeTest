using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace NatureOfCodeTest.Model
{
    [Serializable]
    public abstract class CelestialBody
    {
        [JsonProperty("name")]
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
