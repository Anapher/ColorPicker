using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using System.Windows;

namespace ColorPicker.Controls.ColorViewer
{
    class RGBColorView : ColorViewBase
    {
        private ByteUpDown budR;
        private ByteUpDown budG;
        private ByteUpDown budB;
        public override void CreateView(System.Windows.Controls.StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "R: ", VerticalAlignment = VerticalAlignment.Center });

            budR = InstanceNewByteUpDown();
            InputControl.Children.Add(budR);

            InputControl.Children.Add(new TextBlock() { Text = "G: ", VerticalAlignment = VerticalAlignment.Center });

            budG = InstanceNewByteUpDown();
            InputControl.Children.Add(budG);

            InputControl.Children.Add(new TextBlock() { Text = "B: ", VerticalAlignment = VerticalAlignment.Center });

            budB = InstanceNewByteUpDown();
            InputControl.Children.Add(budB);
        }


        public ByteUpDown InstanceNewByteUpDown()
        {
            var b = new ByteUpDown() { Width = 50, Margin = new Thickness(0, 0, 10, 0), DefaultValue = 0, Value = 0 };
            b.ValueChanged += new RoutedPropertyChangedEventHandler<object>(OnValueChanged);
            return b;
        }

        protected void OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            OnColorChanged();
        }

        public override Color CurrentColor
        {
            get
            {
                return Color.FromRgb(budR.Value.HasValue ? budR.Value.Value : byte.MinValue, budG.Value.HasValue ? budG.Value.Value : byte.MinValue, budB.Value.HasValue ? budB.Value.Value : byte.MinValue);
            }
            set
            {
                DontRaiseEvent = true;
                budR.Value = value.R;
                budG.Value = value.G;
                budB.Value = value.B;
                DontRaiseEvent = false;
            }
        }


    }
}
