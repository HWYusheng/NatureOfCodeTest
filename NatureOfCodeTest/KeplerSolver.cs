using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest.Model
{
    internal class KeplerSolver
    {
        public double SolveEccentricAnomaly(double M, double e)
        {
            double E = M; // Khởi tạo ban đầu
            for (int i = 0; i < 10; i++) // Newton-Raphson
            {
                E = E - (E - e * Math.Sin(E) - M) / (1 - e * Math.Cos(E));
            }
            return E;
        }
    }
}
