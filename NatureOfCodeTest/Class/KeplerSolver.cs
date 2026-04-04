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
            // Use recursion for numerical approximation of orbit.
            // Formula of Recursive Successive Substitution: E_new = M + e * sin(E_old)
            return SolveRecursive(M, e, M, 0);
        }

        private double SolveRecursive(double M, double e, double E, int depth)
        {
            // Base case: Terminate after 10 recursions for stability.
            if (depth >= 10) return E;

            double nextE = M + e * Math.Sin(E);
            return SolveRecursive(M, e, nextE, depth + 1);
        }
    }
}
