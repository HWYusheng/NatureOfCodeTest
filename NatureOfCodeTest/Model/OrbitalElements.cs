using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest.Model
{
    public class OrbitalElements
    {
        public double SemiMajorAxis { get; set; } // a
        public double Eccentricity { get; set; }  // e
        public double Inclination { get; set; }   // i (độ)
        public double ArgumentOfPeriapsis { get; set; } // ω
        public double MeanAnomalyAtEpoch { get; set; }  // M0
        public double EpochTime { get; set; }
    }
}
