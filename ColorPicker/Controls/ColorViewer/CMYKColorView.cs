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
    class CMYKColorView : ColorViewBase
    {
        private SingleUpDown sudCyan;
        private SingleUpDown sudMagenta;
        private SingleUpDown sudYellow;
        private SingleUpDown sudKey;

        public override void CreateView(System.Windows.Controls.StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "Cyan: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudCyan = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Magenta: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudMagenta = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Yellow: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudYellow = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Key: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudKey = InstanceNewSingleUpDown());
        }

        private SingleUpDown InstanceNewSingleUpDown()
        {
            var s = new SingleUpDown() { Width = 70, Margin = new Thickness(0, 0, 10, 0), DefaultValue = 0, Value = 0, Increment = 0.001f };
            s.ValueChanged += s_ValueChanged;
            return s;
        }

        private void s_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnColorChanged();
        }


        public override System.Windows.Media.Color CurrentColor
        {
            get
            {
                return Color.FromRgb((byte)((float)Math.Round(255 * (1 - GetValueFromNullableFloat(sudCyan.Value)) * (1 - GetValueFromNullableFloat(sudKey.Value)))), (byte)((float)Math.Round(255 * (1 - GetValueFromNullableFloat(sudMagenta.Value)) * (1 - GetValueFromNullableFloat(sudKey.Value)))), (byte)((float)Math.Round(255 * (1 - GetValueFromNullableFloat(sudYellow.Value)) * (1 - GetValueFromNullableFloat(sudKey.Value)))));
            }
            set
            {
                float cyan;
                float magenta;
                float yellow;
                float key;

                cyan = 1f - ((float)value.R / 255f);
                magenta = 1f - ((float)value.G / 255f);
                yellow = 1f - ((float)value.B / 255f);
                key = 1f;

                if (cyan < key) { key = cyan; }
                if (magenta < key) { key = magenta; }
                if (yellow < key) { key = yellow; }
                if (key == 1f)
                {
                    cyan = 0f;
                    magenta = 0f;
                    yellow = 0f;
                }
                else
                {
                    cyan = (cyan - key) / (1f - key);
                    magenta = (magenta - key) / (1f - key);
                    yellow = (yellow - key) / (1f - key);
                }
                DontRaiseEvent = true;
                sudCyan.Value = (float)Math.Round(cyan, 3);
                sudMagenta.Value = (float)Math.Round(magenta, 3);
                sudYellow.Value = (float)Math.Round(yellow, 3);
                sudKey.Value = (float)Math.Round(key, 3);
                DontRaiseEvent = false;
            }
        }
    }
}
