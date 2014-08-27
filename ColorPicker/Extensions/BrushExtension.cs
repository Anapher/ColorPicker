using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

static class BrushExtension
{
    private const int MinLightness = 1;
    private const int MaxLightness = 10;
    private const float MinLightnessCoef = 1f;
    private const float MaxLightnessCoef = 0.4f;

    public static SolidColorBrush ChangeLightness(this SolidColorBrush brush, double lightness)
    {
        return new SolidColorBrush(Color.FromArgb(
          brush.Color.A,
          (byte)Math.Min(255, brush.Color.R + 255 * lightness),
          (byte)Math.Min(255, brush.Color.G + 255 * lightness),
          (byte)Math.Min(255, brush.Color.B + 255 * lightness)));
    }
}
