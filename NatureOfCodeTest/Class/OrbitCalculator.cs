using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest.Model
{
    public class OrbitCalculator
    {
        private KeplerSolver _solver = new KeplerSolver();
        public void ComputePlanetState(Planet planet, Star star, double time)
        {
            // Logic to calculate position base on Keplerian Elements
            // 1. Calculate Mean Anomaly -> Eccentric Anomaly -> True Anomaly
            // 2. Convert to Descartes (X, Y)
            // Calculate base on Keplerian Elements: 
            // 1. Mean anomaly -> eccentric ano -> true ano
            var orbit = planet.Orbit;

            // 1. Calc Mean Motion (n) - ave angular speed (rad/s)
            // n = sqrt(G * (M_star + m_planet) / a^3)
            double totalMass = star.Mass + planet.Mass;
            double n = Math.Sqrt(PhysicalConstants.G * totalMass / Math.Pow(orbit.SemiMajorAxis, 3));

            // 2. Mean Anomaly (M) at 'time'
            // M = M0 + n * (t - t0)
            double M = orbit.MeanAnomalyAtEpoch + n * (time - orbit.EpochTime);
            M = M % (2 * Math.PI); 

            // 3. Solve Kepler eq to find Eccentric Anomaly (E)
            // M = E - e * sin(E) -> Use KeplerSolver (Newton-Raphson)
            double E = _solver.SolveEccentricAnomaly(M, orbit.Eccentricity);

            // 4. Calc position coord on Orbital Plane
            double cosE = Math.Cos(E);
            double sinE = Math.Sin(E);
            double x_orbital = orbit.SemiMajorAxis * (cosE - orbit.Eccentricity);
            double y_orbital = orbit.SemiMajorAxis * Math.Sqrt(1 - Math.Pow(orbit.Eccentricity, 2)) * sinE;

            // 5. Rotate position coord according to Argument of Periapsis (ω) and Inclination (i)
            // This is only 2D, so only need to rotate with respect to w
            double omega = orbit.ArgumentOfPeriapsis;
            double cosW = Math.Cos(omega);
            double sinW = Math.Sin(omega);

            double x_final = x_orbital * cosW - y_orbital * sinW;
            double y_final = x_orbital * sinW + y_orbital * cosW;

            // 6. Update relative position coord to the Star
            planet.Position = new Vector2((float)(star.Position.X + x_final), (float)(star.Position.Y + y_final));

            // 7. Calc instaneous Velocity - dy/dx of position
            // v = sqrt(G * M_total / a) * [-sinE, sqrt(1-e^2)*cosE] / (1 - e*cosE)
            double v_factor = Math.Sqrt(PhysicalConstants.G * totalMass / orbit.SemiMajorAxis) / (1 - orbit.Eccentricity * cosE);
            double vx_orbital = -v_factor * sinE;
            double vy_orbital = v_factor * Math.Sqrt(1 - Math.Pow(orbit.Eccentricity, 2)) * cosE;

            // Rotate speed same way as position
            planet.Velocity = new Vector2(
                (float)(vx_orbital * cosW - vy_orbital * sinW),
                (float)(vx_orbital * sinW + vy_orbital * cosW)
            );
        }

        public Vector2 ComputeStarReflexMotion(Planet p, Star s)
        {
            // Exact calculation: v_star = -v_rel * (m_p / (M_s + m_p))
            double totalMass = s.Mass + p.Mass;
            double ratio = p.Mass / totalMass;
            return Vector2.Multiply((float)(-ratio), p.Velocity);
        }
    }
}

