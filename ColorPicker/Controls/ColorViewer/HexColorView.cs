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
    class HexColorView : ColorViewBase
    {
        private TextBox HexBox;

        public override void CreateView(System.Windows.Controls.StackPanel InputControl)
        {
            InputControl.Children.Add(new TextBlock() { Text = "# ", VerticalAlignment = VerticalAlignment.Center });
            InputControl.Children.Add(HexBox = new TextBox() { MaxLength = 7 });
            HexBox.TextChanged += HexBox_TextChanged;
        }

        void HexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string newvalue = HexBox.Text.ToUpper();

            if (newvalue.StartsWith("#"))
            {
                newvalue = newvalue.Remove(0, 1);
            }

            try
            {
                ColorConverter.ConvertFromString("#" + newvalue);
            }
            catch (Exception)
            {
                newvalue = ColorToHex(Colors.Black);
            }

            HexBox.Text = newvalue;
            OnColorChanged();
        }

        public override System.Windows.Media.Color CurrentColor
        {
            get
            {
                try
                {
                    return (Color)ColorConverter.ConvertFromString("#" + HexBox.Text);
                }
                catch (FormatException)
                {
                    return Colors.Black;
                }
            }
            set
            {
                string hexstring = ColorToHex(value);
                if (hexstring != HexBox.Text)
                {
                    HexBox.Text = hexstring;
                }
            }
        }

        private string ColorToHex(Color color)
        {
            return string.Format("{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B).ToUpper();
        }
    }
}
