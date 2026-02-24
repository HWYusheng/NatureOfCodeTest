using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest.Model
{
    public class SimulationEngine
    {
        public Star HostStar { get; set; }
        public Planet OrbitingPlanet { get; set; }
        public double CurrentTime { get; private set; }
        public double TimeStep { get; set; } = 86400; // 1 ngày (giây)

        private OrbitCalculator _orbitCalc = new OrbitCalculator();
        private RadialVelocityCalculator _rvCalc = new RadialVelocityCalculator();

        public List<SimulationSample> Samples { get; } = new List<SimulationSample>();

        public void Step()
        {
            // Reset star to origin to compute relative orbital position of the planet
            HostStar.Position = System.Numerics.Vector2.Zero;
            _orbitCalc.ComputePlanetState(OrbitingPlanet, HostStar, CurrentTime);
            
            // The position calculated is now the relative position (Planet - Star)
            System.Numerics.Vector2 relativePos = OrbitingPlanet.Position;
            double totalMass = HostStar.Mass + OrbitingPlanet.Mass;

            // Update position crosshair relative to the barycenter (located at 0,0)
            HostStar.Position = System.Numerics.Vector2.Multiply((float)(-OrbitingPlanet.Mass / totalMass), relativePos);
            OrbitingPlanet.Position = System.Numerics.Vector2.Multiply((float)(HostStar.Mass / totalMass), relativePos);

            // Update velocity (reflex motion)
            HostStar.Velocity = _orbitCalc.ComputeStarReflexMotion(OrbitingPlanet, HostStar);

            Samples.Add(new SimulationSample
            {
                Time = CurrentTime,
                StarPosition = HostStar.Position,
                PlanetPosition = OrbitingPlanet.Position,
                RadialVelocity = _rvCalc.Compute(HostStar.Velocity)
            });

            CurrentTime += TimeStep;
        }

        public void RunSteps(int n) { for (int i = 0; i < n; i++) Step(); }
    }
}
