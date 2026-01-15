using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest.Model
{
    internal class SimulationEngine
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
            _orbitCalc.ComputePlanetState(OrbitingPlanet, HostStar, CurrentTime);
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
