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
    class YPbPrColorView : ColorViewBase
    {
        SingleUpDown sudY;
        SingleUpDown sudPb;
        SingleUpDown sudPr;

        public override void CreateView(StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "Y: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudY = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Pb: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudPb = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Pr: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudPr = InstanceNewSingleUpDown());
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
                return Color.FromRgb((byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (1.402f * GetValueFromNullableFloat(sudPr.Value))) * 255)), (byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (-0.344136f * GetValueFromNullableFloat(sudPb.Value)) + (-0.714136f * GetValueFromNullableFloat(sudPr.Value))) * 255)), (byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (1.772f * GetValueFromNullableFloat(sudPb.Value))) * 255)));
            }
            set
            {
                DontRaiseEvent = true;
                sudY.Value = (float)Math.Round((0.299f * value.R) + (0.587f * value.G) + (0.114f * value.B), 3);
                sudPb.Value = (float)Math.Round((-0.168736f * value.R) + (-0.331264f * value.G) + (0.5f * value.B), 3);
                sudPr.Value = (float)Math.Round((0.5f * value.R) + (-0.418688f * value.G) + (-0.081312f * value.B), 3);
                DontRaiseEvent = false;
            }
        }
    }
}
