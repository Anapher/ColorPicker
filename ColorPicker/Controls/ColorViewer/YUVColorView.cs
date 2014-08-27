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
    class YUVColorView : ColorViewBase
    {
        SingleUpDown sudY;
        SingleUpDown sudU;
        SingleUpDown sudV;

        public override void CreateView(StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "Y: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudY = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "U: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudU = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "V: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudV = InstanceNewSingleUpDown());
        }

        public SingleUpDown InstanceNewSingleUpDown()
        {
            var s = new SingleUpDown() { Width = 70, Margin = new Thickness(0, 0, 10, 0), DefaultValue = 0, Value = 0, Increment = 0.001f };
            s.ValueChanged += s_ValueChanged;
            return s;
        }

        private void s_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnColorChanged();
        }

        public override Color CurrentColor
        {
            get
            {
                return Color.FromRgb((byte)Math.Round((GetValueFromNullableFloat(sudY.Value) + (1.13983f * GetValueFromNullableFloat(sudV.Value))) * 255), (byte)Math.Round((GetValueFromNullableFloat(sudY.Value) + (-0.39465f * GetValueFromNullableFloat(sudU.Value)) + (-0.58060f * GetValueFromNullableFloat(sudV.Value))) * 255), (byte)Math.Round((GetValueFromNullableFloat(sudY.Value) + (2.03211f * GetValueFromNullableFloat(sudU.Value))) * 255));
            }
            set
            {
                float r = value.R / 255f;
                float g = value.G / 255f;
                float b = value.B / 255f;
                DontRaiseEvent = true;
                sudY.Value = (float)Math.Round((0.299f * r) + (0.587f * g) + (0.114f * b), 3);
                sudU.Value = (float)Math.Round((-0.14713f * r) + (-0.28886f * g) + (0.436f * b), 3);
                sudV.Value = (float)Math.Round((0.615f * r) + (-0.51499f * g) + (-0.10001f * b), 3);
                DontRaiseEvent = false;
            }
        }
    }
}
