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
    class CIE1931ColorView : ColorViewBase
    {
        SingleUpDown sudX;
        SingleUpDown sudY;
        SingleUpDown sudZ;

        public override void CreateView(System.Windows.Controls.StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text ="X: " ,VerticalAlignment = VerticalAlignment.Center});
            InputControl.Children.Add(sudX = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Y: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudY = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Z: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudZ = InstanceNewSingleUpDown());
        }

        public SingleUpDown InstanceNewSingleUpDown()
        {
            var s = new SingleUpDown() { Width = 70, Margin = new Thickness(0, 0, 10, 0), DefaultValue = 0, Value = 0, Increment = 0.001f };
            s.ValueChanged += s_ValueChanged;
            return s;
        }

        void s_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnColorChanged();
        }

        public override System.Windows.Media.Color CurrentColor
        {
            get
            {
                float X = GetValueFromNullableFloat(sudX.Value), Y = GetValueFromNullableFloat(sudY.Value), Z = GetValueFromNullableFloat(sudZ.Value);
                return Color.FromRgb((byte)Math.Round(((3.240479f * X) + (-1.537150f * Y) + (-0.498535f * Z)) * 255), (byte)Math.Round(((-0.969256f * X) + (1.875992f * Y) + (0.041556f * Z)) * 255), (byte)Math.Round(((0.055648f * X) + (-0.204043f * Y) + (1.057311f * Z)) * 255));
            }
            set
            {
                float r = value.R / 255f;
                float g = value.G / 255f;
                float b = value.B / 255f;
                DontRaiseEvent = true;
                sudX.Value = (float)Math.Round((0.412453f * r) + (0.357580f * g) + (0.180423f * b), 3);
                sudY.Value = (float)Math.Round((0.212671f * r) + (0.715160f * g) + (0.072169f * b), 3);
                sudZ.Value = (float)Math.Round((0.019334f * r) + (0.119193f * g) + (0.950227f * b), 3);
                DontRaiseEvent = false;
            }
        }
    }
}
