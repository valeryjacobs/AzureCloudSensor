using HAT = GHIElectronics.UWP.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;


namespace CloudSensor
{
    public static class ColorHelper
    {
        
        public static HAT.FEZHAT.Color GetHATColor(int percentage,Color from, Color to)
        {
            var osColor = ColorHelper.GetBlendedColor(percentage,  from,  to);
            return new HAT.FEZHAT.Color(osColor.R, osColor.G, osColor.B);
        }
        public static Color GetBlendedColor(int percentage, Color from, Color to)
        {
            if (percentage < 50)
                return Interpolate(from, to, percentage / 50.0);
            return Interpolate(from, to, (percentage - 50) / 50.0);
        }

        private static Color Interpolate(Color color1, Color color2, double fraction)
        {
            double r = Interpolate(color1.R, color2.R, fraction);
            double g = Interpolate(color1.G, color2.G, fraction);
            double b = Interpolate(color1.B, color2.B, fraction);
            return Color.FromArgb(255,(byte)Math.Round(r), (byte)Math.Round(g), (byte)Math.Round(b));
        }

        private static double Interpolate(double d1, double d2, double fraction)
        {
            return d1 + (d1 - d2) * fraction;
        }
    }
}
