using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorPicker.Model
{
    class IntelliColor
    {
        public IntelliColor()
        {

        }

        private Color basecolor;
        public Color BaseColor
        {
            get { return basecolor; }
            set
            {
                basecolor = value;
            }
        }

        public string Hex
        {
            get { return basecolor.R.ToString("X2") + basecolor.G.ToString("X2") + basecolor.B.ToString("X2"); }
            set
            {
                if (value.StartsWith("#"))
                {
                    value = value.Remove(0, 1);
                }
                BaseColor = (Color)ColorConverter.ConvertFromString(value);
            }
        }


        public byte[] RGB
        {
            get { return new byte[3] { basecolor.R, basecolor.G, basecolor.B }; }
            set
            {
                BaseColor = Color.FromRgb(value[0], value[1], value[2]);
            }
        }


    }
}
