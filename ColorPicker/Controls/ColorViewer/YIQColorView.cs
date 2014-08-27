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
    class YIQColorView : ColorViewBase
    {
        SingleUpDown sudY;
        SingleUpDown sudI;
        SingleUpDown sudQ;

        public override void CreateView(StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "Y: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudY = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "I: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudI = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Q: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudQ = InstanceNewSingleUpDown());
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

        public override Color CurrentColor
        {
            get
            {
                return Color.FromRgb((byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (0.9563f * GetValueFromNullableFloat(sudI.Value)) + (0.6210f * GetValueFromNullableFloat(sudQ.Value))) * 255)), (byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (-0.2721f * GetValueFromNullableFloat(sudI.Value)) + (-0.6474f * GetValueFromNullableFloat(sudQ.Value))) * 255)), (byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (-1.01070f * GetValueFromNullableFloat(sudI.Value)) + (1.7046f * GetValueFromNullableFloat(sudQ.Value))) * 255)));
            }
            set
            {
                DontRaiseEvent = true;
                float r = value.R / 255f;
                float g = value.G / 255f;
                float b = value.B / 255f;

                sudY.Value = (float)Math.Round((0.299f * r) + (0.587f * g) + (0.114f * b), 3);
                sudI.Value = (float)Math.Round((0.595716f * r) + (-0.274453f * g) + (-0.321263f * b), 3);
                sudQ.Value = (float)Math.Round((0.211456f * r) + (-0.522591f * g) + (0.311135f * b), 3);
                DontRaiseEvent = false;
            }
        }
    }
}
