using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorPicker.Controls
{
    class ColorChangedEventArgs : EventArgs
    {
        public Color color { get; set; }
    }
}
