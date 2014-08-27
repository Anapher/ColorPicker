using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using System.Windows.Media;

namespace ColorPicker.Controls
{
    class ColorDisplayer : StackPanel
    {
        public ColorDisplayer()
        {
            ColorType = DisplayColorType.RGB;
            CreateView();
            this.Orientation = System.Windows.Controls.Orientation.Horizontal;
            this.Height = 20;
        }

        private ColorViewBase ColorView;

        public void CreateView()
        {
            this.Children.Clear();
            switch (ColorType)
            {
                case DisplayColorType.RGB:
                    ColorView = new ColorViewer.RGBColorView();
                    break;
                case DisplayColorType.Hex:
                    ColorView = new ColorViewer.HexColorView();
                    break;
                case DisplayColorType.CIE1931:
                    ColorView = new ColorViewer.CIE1931ColorView();
                    break;
                case DisplayColorType.CMY:
                    ColorView = new ColorViewer.CMYColorView();
                    break;
                case DisplayColorType.CMYK:
                    ColorView = new ColorViewer.CMYKColorView();
                    break;
                case DisplayColorType.YCbCr:
                    ColorView = new ColorViewer.YCbCrColorView();
                    break;
                case DisplayColorType.YDbDr:
                    ColorView = new ColorViewer.YDbDrColorView();
                    break;
                case DisplayColorType.YIQ:
                    ColorView = new ColorViewer.YIQColorView();
                    break;
                case DisplayColorType.YPbPr:
                    ColorView = new ColorViewer.YPbPrColorView();
                    break;
                case DisplayColorType.YUV:
                    ColorView = new ColorViewer.YUVColorView();
                    break;
            }
            ColorView.ColorChanged += ColorView_ColorChanged;
            ColorView.CreateView(this);
        }

        void ColorView_ColorChanged(object sender, EventArgs e)
        {
            SelectedColor = ColorView.CurrentColor;
        }

        #region DependencyProperties
        public static readonly DependencyProperty ColorTypeProperty = DependencyProperty.Register("ColorType", typeof(DisplayColorType), typeof(ColorDisplayer), new FrameworkPropertyMetadata(DisplayColorType.RGB, OnColorTypePropertyCallback));

        private static void OnColorTypePropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ColorDisplayer;
            obj.CreateView();
        }

        public DisplayColorType ColorType
        {
            get { return (DisplayColorType)this.GetValue(ColorTypeProperty); }
            set
            {
                this.SetValue(ColorTypeProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorDisplayer), new FrameworkPropertyMetadata(Color.FromRgb(0, 0, 0), OnSelectedColorPropertyCallback) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged });

        private static void OnSelectedColorPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as ColorDisplayer;
            obj.ColorView.CurrentColor = obj.SelectedColor;
        }

        public Color SelectedColor
        {
            get { return (Color)this.GetValue(SelectedColorProperty); }
            set { this.SetValue(SelectedColorProperty, value); }
        }

        #endregion
    }

    public enum DisplayColorType
    {
        Hex,
        RGB,
        CIE1931,
        CMY,
        CMYK,
        YCbCr,
        YDbDr,
        YIQ,
        YPbPr,
        YUV
    }
}
