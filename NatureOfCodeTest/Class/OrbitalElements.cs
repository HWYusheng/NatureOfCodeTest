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
        public double Inclination { get; set; }   // i (degrees)
        public double ArgumentOfPeriapsis { get; set; } // ω
        public double MeanAnomalyAtEpoch { get; set; }  // M0, basiclly the starting point, should be set to 0. Combine with epochtime in keplersolver to determine the position of the planet in the orbit
        public double EpochTime { get; set; } // in case we want the initial position to be some time later
    }
}
