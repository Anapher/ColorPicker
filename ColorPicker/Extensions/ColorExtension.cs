using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

public static class ColorExtension
{
    private const int MinLightness = 1;
    private const int MaxLightness = 10;
    private const float MinLightnessCoef = 1f;
    private const float MaxLightnessCoef = 0.4f;

    public static Color ChangeLightness(this Color color, double lightness)
    {
        return Color.FromArgb(
          color.A,
          (byte)Math.Min(255, color.R + 255 * lightness),
          (byte)Math.Min(255, color.G + 255 * lightness),
          (byte)Math.Min(255, color.B + 255 * lightness));
    }
}
