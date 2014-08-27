using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ColorPicker.Controls
{
    abstract class ColorViewBase
    {
        public abstract void CreateView(StackPanel InputControl);
        public abstract Color CurrentColor { get; set; }
        public event EventHandler ColorChanged;

        protected bool DontRaiseEvent = false;
        protected void OnColorChanged()
        {
            if (DontRaiseEvent)
                return;

            ColorChanged(this, EventArgs.Empty);
        }

        protected float GetValueFromNullableFloat(float? value)
        {
            return value.HasValue ? value.Value : 0;
        }
    }
}
