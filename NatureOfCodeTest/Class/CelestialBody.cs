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
        // Here, the Name would the HostName
        public double Luminosity { get; set; }
        
        public Queue<System.Numerics.Vector2> HistoryQueue { get; private set; } = new Queue<System.Numerics.Vector2>();
        public int MaxHistorySize { get; set; } = 100;

        public void RecordVelocity() 
        {
            HistoryQueue.Enqueue(Velocity);
            
            // If we exceed our maximum history, remove the oldest entry. This is for the program to run smoother over long period.
            if (HistoryQueue.Count > MaxHistorySize)
            {
                HistoryQueue.Dequeue();
            }
        }
    }

    public class Planet : CelestialBody
    {
        // However, for planets, we need a seperate hostName, beside the Name itself
        public string HostName { get; set; }
        public OrbitalElements Orbit { get; set; }
    }
}
