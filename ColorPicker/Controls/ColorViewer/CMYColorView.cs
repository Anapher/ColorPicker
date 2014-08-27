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
    class CMYColorView : ColorViewBase
    {
        private SingleUpDown sudCyan;
        private SingleUpDown sudMagenta;
        private SingleUpDown sudYellow;

        public override void CreateView(System.Windows.Controls.StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "Cyan: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudCyan = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Magenta: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudMagenta = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Yellow: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudYellow = InstanceNewSingleUpDown());
        }

        private SingleUpDown InstanceNewSingleUpDown()
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
                return Color.FromRgb((byte)(Math.Round(1 - GetValueFromNullableFloat(sudCyan.Value)) * 255), (byte)(Math.Round((1 - GetValueFromNullableFloat(sudMagenta.Value)) * 255)), (byte)(Math.Round((1 - GetValueFromNullableFloat(sudYellow.Value)) * 255)));
            }
            set
            {
                DontRaiseEvent = true;
                sudCyan.Value = (float)Math.Round(1 - (value.R / 255f), 3);
                sudMagenta.Value = (float)Math.Round(1 - (value.G / 255f), 3);
                sudYellow.Value = (float)Math.Round(1 - (value.B / 255f), 3);
                DontRaiseEvent = false;
            }
        }
    }
}
