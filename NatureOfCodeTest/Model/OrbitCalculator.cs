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
            // Logic tính toán vị trí dựa trên Keplerian Elements
            // 1. Tính Mean Anomaly -> Eccentric Anomaly -> True Anomaly
            // 2. Chuyển đổi sang tọa độ Descartes (X, Y)
            var orbit = planet.Orbit;

            // 1. Tính Mean Motion (n) - tốc độ góc trung bình (rad/s)
            // n = sqrt(G * (M_star + m_planet) / a^3)
            double totalMass = star.Mass + planet.Mass;
            double n = Math.Sqrt(PhysicalConstants.G * totalMass / Math.Pow(orbit.SemiMajorAxis, 3));

            // 2. Tính Mean Anomaly (M) tại thời điểm 'time'
            // M = M0 + n * (t - t0)
            double M = orbit.MeanAnomalyAtEpoch + n * (time - orbit.EpochTime);
            M = M % (2 * Math.PI); // Chuẩn hóa về [0, 2pi]

            // 3. Giải phương trình Kepler để tìm Eccentric Anomaly (E)
            // M = E - e * sin(E) -> Dùng KeplerSolver (Newton-Raphson)
            double E = _solver.SolveEccentricAnomaly(M, orbit.Eccentricity);

            // 4. Tính tọa độ trong mặt phẳng quỹ đạo (Orbital Plane)
            // Giả sử mặt phẳng nằm trên hệ trục X', Y' với Star tại tiêu điểm
            double cosE = Math.Cos(E);
            double sinE = Math.Sin(E);

            // Tọa độ X, Y trong mặt phẳng quỹ đạo (chu kỳ elip)
            double x_orbital = orbit.SemiMajorAxis * (cosE - orbit.Eccentricity);
            double y_orbital = orbit.SemiMajorAxis * Math.Sqrt(1 - Math.Pow(orbit.Eccentricity, 2)) * sinE;

            // 5. Xoay tọa độ theo Argument of Periapsis (ω) và Inclination (i)
            // Đối với 2D đơn giản, chúng ta chủ yếu xoay theo ω (góc lệch của cận điểm quỹ đạo)
            double omega = orbit.ArgumentOfPeriapsis;
            double cosW = Math.Cos(omega);
            double sinW = Math.Sin(omega);

            double x_final = x_orbital * cosW - y_orbital * sinW;
            double y_final = x_orbital * sinW + y_orbital * cosW;

            // 6. Cập nhật vị trí hành tinh (tương đối so với Sao chủ)
            planet.Position = new Vector2((float)(star.Position.X + x_final), (float)(star.Position.Y + y_final));

            // 7. Tính vận tốc tức thời (Velocity) - Đạo hàm của vị trí
            // v = sqrt(G * M_total / a) * [-sinE, sqrt(1-e^2)*cosE] / (1 - e*cosE)
            double v_factor = Math.Sqrt(PhysicalConstants.G * totalMass / orbit.SemiMajorAxis) / (1 - orbit.Eccentricity * cosE);
            double vx_orbital = -v_factor * sinE;
            double vy_orbital = v_factor * Math.Sqrt(1 - Math.Pow(orbit.Eccentricity, 2)) * cosE;

            // Xoay vận tốc tương tự như vị trí
            planet.Velocity = new Vector2(
                (float)(vx_orbital * cosW - vy_orbital * sinW),
                (float)(vx_orbital * sinW + vy_orbital * cosW)
            );
        }

        public Vector2 ComputeStarReflexMotion(Planet p, Star s)
        {
            // Tính toán sự rung lắc của sao chủ do lực hấp dẫn từ hành tinh
            double ratio = p.Mass / s.Mass;
            return Vector2.Multiply((float)(-ratio), p.Velocity);
        }
    }
}

}
