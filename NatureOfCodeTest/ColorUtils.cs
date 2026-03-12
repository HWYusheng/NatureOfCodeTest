using System;
using System.Drawing;

namespace NatureOfCodeTest
{
    public static class ColorUtils
    {
        public const double BaseWavelengthNM = 580.0; // Center of visible spectrum (Yellow-Green)
        public const double DopplerExaggeration = 2.0e9; // Increased for more vivid color changes

        public static double GetShiftedWavelength(double radialVelocityMS)
        {
            double c = 299792458.0; // Speed of light in m/s
            
            // Map the radial velocity to a wavelength shift.
            // rv > 0 (away) -> Redshift (longer wavelength)
            // rv < 0 (towards) -> Blueshift (shorter wavelength)
            double shifted = BaseWavelengthNM * (1.0 + (radialVelocityMS * DopplerExaggeration) / c);
            
            // Clamp to visible spectrum: 380nm (Purple) to 750nm (Red)
            return Math.Max(380.0, Math.Min(750.0, shifted));
        }

        public static Color WavelengthToRGB(double wavelength, int alpha = 200)
        {
            double Gamma = 0.80;
            double IntensityMax = 255;
            double factor;
            double red, green, blue;

            if ((wavelength >= 380) && (wavelength < 440))
            {
                red = -(wavelength - 440) / (440 - 380);
                green = 0.0;
                blue = 1.0;
            }
            else if ((wavelength >= 440) && (wavelength < 490))
            {
                red = 0.0;
                green = (wavelength - 440) / (490 - 440);
                blue = 1.0;
            }
            else if ((wavelength >= 490) && (wavelength < 510))
            {
                red = 0.0;
                green = 1.0;
                blue = -(wavelength - 510) / (510 - 490);
            }
            else if ((wavelength >= 510) && (wavelength < 580))
            {
                red = (wavelength - 510) / (580 - 510);
                green = 1.0;
                blue = 0.0;
            }
            else if ((wavelength >= 580) && (wavelength < 645))
            {
                red = 1.0;
                green = -(wavelength - 645) / (645 - 580);
                blue = 0.0;
            }
            else if ((wavelength >= 645) && (wavelength <= 780))
            {
                red = 1.0;
                green = 0.0;
                blue = 0.0;
            }
            else
            {
                red = 0.0;
                green = 0.0;
                blue = 0.0;
            }

            // Let the intensity fall off near the vision limits
            if ((wavelength >= 380) && (wavelength < 420))
            {
                factor = 0.3 + 0.7 * (wavelength - 380) / (420 - 380);
            }
            else if ((wavelength >= 420) && (wavelength < 701))
            {
                factor = 1.0;
            }
            else if ((wavelength >= 701) && (wavelength <= 780))
            {
                factor = 0.3 + 0.7 * (780 - wavelength) / (780 - 701);
            }
            else
            {
                factor = 0.0;
            }

            int r = red == 0.0 ? 0 : (int)Math.Round(IntensityMax * Math.Pow(red * factor, Gamma));
            int g = green == 0.0 ? 0 : (int)Math.Round(IntensityMax * Math.Pow(green * factor, Gamma));
            int b = blue == 0.0 ? 0 : (int)Math.Round(IntensityMax * Math.Pow(blue * factor, Gamma));

            return Color.FromArgb(alpha, r, g, b);
        }
    }
}
