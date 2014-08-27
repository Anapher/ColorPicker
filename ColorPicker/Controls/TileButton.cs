using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ColorPicker.Controls
{
    class TileButton : ContentControl
    {
        ColorAnimation coloranimation;
        Rectangle rect;
        public TileButton()
        {
            rect = new Rectangle();
            UpdateRect();
            this.Content = rect;
        }

        private Color hovercolor;
        private SolidColorBrush pressedbrush;

        private Storyboard sIn;
        protected void StartInAnimation()
        {
            if (sIn == null)
            {
                sIn = InitalizeStoryboard(BaseColor, hovercolor, TimeSpan.FromMilliseconds(200));
            }
            sIn.Begin();
        }

        private Storyboard sOut;
        protected void StartOutAnimation()
        {
            if (sOut == null)
            {
                sOut = InitalizeStoryboard(hovercolor, BaseColor, TimeSpan.FromMilliseconds(200));
            }
            sOut.Begin();
        }

        private Storyboard InitalizeStoryboard(Color startcolor, Color endcolor, TimeSpan duration)
        {
            coloranimation = new ColorAnimation();
            coloranimation.From = startcolor;
            coloranimation.To = endcolor;
            coloranimation.Duration = duration;

            Storyboard s = new Storyboard();
            s.Duration = duration;
            s.Children.Add(coloranimation);

            Storyboard.SetTarget(coloranimation, rect);
            Storyboard.SetTargetProperty(coloranimation, new PropertyPath("Fill.Color"));
            return s;
        }

        public static DependencyProperty BaseColorProperty = DependencyProperty.Register("BaseColor", typeof(Color), typeof(TileButton), new FrameworkPropertyMetadata(BaseColorChanged));
        public static DependencyProperty ColorToSetProperty = DependencyProperty.Register("ColorToSet", typeof(Color), typeof(TileButton), new FrameworkPropertyMetadata() {BindsTwoWayByDefault =true });

        public Color BaseColor
        {
            get { return (Color)this.GetValue(BaseColorProperty); }
            set { this.SetValue(BaseColorProperty, value); }
        }

        public Color ColorToSet
        {
            set
            {
                this.SetValue(ColorToSetProperty, value);
            }
        }

        private static void BaseColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TileButton)d).UpdateRect();
        }

        private void UpdateRect()
        {
            rect.Fill = new SolidColorBrush(BaseColor);
            hovercolor = BaseColor.ChangeLightness(0.1);
            pressedbrush = new SolidColorBrush(BaseColor.ChangeLightness(0.15));

            sIn = null;
            sOut = null;
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            StartInAnimation();
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            StartOutAnimation();
        }

        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            ColorToSet = BaseColor;
            rect.Fill = pressedbrush;
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            rect.Fill = new SolidColorBrush(hovercolor);
        }
    }
}
