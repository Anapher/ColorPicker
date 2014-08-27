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
    class YDbDrColorView : ColorViewBase
    {
        SingleUpDown sudY;
        SingleUpDown sudDb;
        SingleUpDown sudDr;


        public override void CreateView(StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "Y: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudY = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Db: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudDb = InstanceNewSingleUpDown());

            InputControl.Children.Add(new TextBlock() { Text = "Dr: ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(sudDr = InstanceNewSingleUpDown());
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
                return Color.FromRgb((byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (GetValueFromNullableFloat(sudDb.Value) * 0.000092303716148f) + (GetValueFromNullableFloat(sudDr.Value) * -0.5259126300661865f)) * 255)), (byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (GetValueFromNullableFloat(sudDb.Value) * -0.129132898890509f) + (GetValueFromNullableFloat(sudDr.Value) * 0.267899328207599f)) * 255)), (byte)((float)Math.Round((GetValueFromNullableFloat(sudY.Value) + (GetValueFromNullableFloat(sudDb.Value) * 0.664679059978955f) + (GetValueFromNullableFloat(sudDr.Value) * -0.000079202543533f)) * 255)));
            }
            set
            {
                DontRaiseEvent = true;
                sudY.Value = (float)Math.Round((0.299f * value.R) + (0.587f * value.G) + (0.114f * value.B), 3);
                sudDb.Value = (float)Math.Round((-0.450f * value.R) + (-0.883f * value.G) + (1.333f * value.B), 3);
                sudDr.Value = (float)Math.Round((-1.333f * value.R) + (1.116f * value.G) + (0.217f * value.B), 3);
                DontRaiseEvent = false;
            }
        }
    }
}
