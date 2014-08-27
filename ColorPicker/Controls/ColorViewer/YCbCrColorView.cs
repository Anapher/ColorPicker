using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;

namespace ColorPicker.Controls.ColorViewer
{
    class YCbCrColorView : ColorViewBase
    {
        ByteUpDown budY;
        ByteUpDown budCb;
        ByteUpDown budCr;

        public override void CreateView(StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "Y: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(budY = InstanceNewByteUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Cb: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(budCb = InstanceNewByteUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Cr: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(budCr = InstanceNewByteUpDown());
        }

        public ByteUpDown InstanceNewByteUpDown()
        {
            var b = new ByteUpDown() { Width = 50, Margin = new Thickness(0, 0, 10, 0), DefaultValue = 0, Value = 0 };
            b.ValueChanged += b_ValueChanged;
            return b;
        }

        void b_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnColorChanged();
        }

        public override Color CurrentColor
        {
            get
            {
                return Color.FromRgb((byte)((float)Math.Round(((1.164f * (GetValueFromNullableFloat(budY.Value) - 16)) + (1.793f * (GetValueFromNullableFloat(budCr.Value) - 128))))), (byte)((float)Math.Round(((1.164f * (GetValueFromNullableFloat(budY.Value) - 16)) + (-0.213f * (GetValueFromNullableFloat(budCb.Value) - 128)) + (-0.533f * (GetValueFromNullableFloat(budCr.Value) - 128))))), (byte)((float)Math.Round(((1.164f * (GetValueFromNullableFloat(budY.Value) - 16)) + (2.112f * (GetValueFromNullableFloat(budCb.Value) - 128))))));
            }
            set
            {
                DontRaiseEvent = true;
                budY.Value = (byte)((float)Math.Round((0.183f * value.R) + (0.614f * value.G) + (0.062f * value.B) + 16));
                budCb.Value = (byte)((float)Math.Round((-0.101f * value.R) + (-0.339f * value.G) + (0.439f * value.B) + 128));
                budCr.Value = (byte)((float)Math.Round((0.439f * value.R) + (-0.399f * value.G) + (-0.040f * value.B) + 128));
                DontRaiseEvent = false;
            }
        }
    }
}
