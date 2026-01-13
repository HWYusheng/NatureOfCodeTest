using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    internal class Plotting
    {
        //    def bin_spectrum(wl, fl, lmin: int = 10, lmax: int = 30_000):
        //bwl = linspace(lmin, lmax, lmax - lmin + 1)
        //bfl = zeros(bwl.size)
        //istart, iend = 0, 0
        //for ibin in range(bfl.size) :
        //    cwl = bwl[ibin]
        //    n = 0
        //    while wl[istart] < cwl - 0.5:
        //        istart += 1
        //    iend = istart
        //    while wl[iend] < cwl + 0.5 and iend<wl.size - 1:
        //        bfl[ibin] += fl[istart]
        //        n += 1
        //        iend += 1
        //    istart = iend
        //    if n > 0:
        //        bfl[ibin] /= n
        //return bwl, bfl
        //https://github.com/hpparvi/PyTransit/blob/master/pytransit/stars/btsettl.py
        public float[] RaxisAngle(Vector2 axis, float theta)
        {
            float cosTheta = (float)Math.Cos(Convert.ToDouble(theta));
            float sinTheta = (float)Math.Sin(Convert.ToDouble(theta));
            float[] data = {
                cosTheta + axis.X * axis.X * (1-cosTheta),
                axis.X * axis.Y -sinTheta,
            };
            return data;
        }
        public (float[], float[]) binSpectrum(float[] wl, float[] fl, int lmin = 10, int lmax = 30000)
        {
            float[] bwl = linspace(lmin, lmax, lmax - lmin + 1);
            float[] bfl = zeros(bwl.size);
            int isStart = 0, isEnd = 0;
            for (int bin = 0; bin < bfl.size; bin++)
            {
                float cwl = bwl[bin];
                int n = 0;
                while (wl[isEnd]< cwl+0.5 && isEnd<wl.size -1)
                {
                    bfl[bin] += fl[isStart];
                    n++;
                    isEnd++;
                    if (n>0)
                    {
                        bfl[bin] /= n;
                    }
                }
            }
            return (bwl, bfl);
        }
        public (float, float) readSpectrum(string fname)
        {
            Body() df = new Body();
            float wl = DecoderFallback 
            float fl = 
        }
        public Int32 GetEff()
        {
            return Convert.ToInt32(1e2);
        }
    }
}
