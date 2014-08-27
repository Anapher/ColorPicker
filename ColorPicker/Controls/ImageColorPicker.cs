using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ColorPicker.ViewModelBase;

namespace ColorPicker.Controls
{
    /// <summary>
    /// Image element with the ability to pick out a pixel color value.
    /// </summary>
    /// <remarks>
    /// <see cref="ImageColorPicker"/> element adorns the <see cref="System.Windows.Controls.Image"/>
    /// it's derived from with the facility to pick the image pixel color value at the position 
    /// specified by the selector visual.
    /// </remarks>
    public class ImageColorPicker : Image
    {
        #region SelectedColor
        /// <summary>
        /// SelectedColor property backing ReadOnly DependencyProperty.
        /// </summary>
        private static readonly DependencyPropertyKey SelectedColorPropertyKey
            = DependencyProperty.RegisterReadOnly("SelectedColor", typeof(Color), typeof(ImageColorPicker)
            , new FrameworkPropertyMetadata(Colors.Transparent));
        /// <summary>
        /// Gets or sets the color selected.
        /// </summary>
        /// <value>The color selected.</value>
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorPropertyKey.DependencyProperty); }
        }
        #endregion SelectedColor

        #region Selector
        /// <summary>
        /// Selector property backing DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty SelectorProperty
            = DependencyProperty.Register("Selector", typeof(Drawing), typeof(ImageColorPicker)
            , new FrameworkPropertyMetadata(new GeometryDrawing(Brushes.White, new Pen(Brushes.Black, 1)
                    , new EllipseGeometry(new Point(), 5, 5))
                , FrameworkPropertyMetadataOptions.AffectsRender)
                , ValidateSelector);

        public event EventHandler ColorChanged;

        /// <summary>
        /// Validates the suggested selector drawing value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if suggested value isn't null; otherwise <c>false</c>.</returns>
        static bool ValidateSelector(object value)
        {
            return value == null ? false : true;
        }
        /// <summary>
        /// Gets or sets the selector drawing.
        /// </summary>
        /// <value>The selector drawing. Must not be null</value>
        /// <remarks>
        /// <see cref="SelectorDrawing"/> is the <see cref="System.Windows.Media.Drawing"/> using
        /// to mark the position where the color is selected.
        /// <para>The <see cref="ImageColorPicker"/> desn't pose any restrictions on the size of
        /// this drawing. It's the user duty to choose it resonably.</para>
        /// </remarks>
        public Drawing Selector
        {
            get { return (Drawing)GetValue(SelectorProperty); }
            set { SetValue(SelectorProperty, value); }
        }
        #endregion Selector

        /// <summary>
        /// Renders the contents of an <see cref="T:System.Windows.Controls.Image"/> and 
        /// the SelectorDrawing.
        /// </summary>
        /// <param name="dc">An instance of <see cref="T:System.Windows.Media.DrawingContext"/> 
        /// used to render the control.</param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ActualWidth == 0 || ActualHeight == 0)
                return;

            // Render the SelectorDrawing
            dc.PushTransform(new TranslateTransform(Position.X, Position.Y));
            dc.DrawDrawing(Selector);
            dc.Pop();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.SizeChanged"/> event, 
        /// using the specified information as part of the eventual event data.
        /// </summary>
        /// <param name="sizeInfo">Details of the old and new size involved in the change.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            cachedTargetBitmap = null; // TargetBitmap cache isn't valid anymore.
            // Adjust the selector position proportionally to size change.
            if (sizeInfo.PreviousSize.Width > 0 && sizeInfo.PreviousSize.Height > 0)
                Position = new Point(Position.X * sizeInfo.NewSize.Width / sizeInfo.PreviousSize.Width
                    , Position.Y * sizeInfo.NewSize.Height / sizeInfo.PreviousSize.Height);
        }

        /// <summary>
        /// Invoked whenever the effective value of any dependency property on this 
        /// <see cref="T:System.Windows.FrameworkElement"/> has been updated. 
        /// The specific dependency property that changed is reported in the arguments parameter. 
        /// Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)"/>.
        /// </summary>
        /// <param name="e">The event data that describes the property that changed, 
        /// as well as old and new values.</param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "Source")
            {
                cachedTargetBitmap = null; // TargetBitmap cache isn't valid anymore.
                Position = new Point(); // Move the selector to the top-left corner.
            }
            base.OnPropertyChanged(e);
        }

        #region Position
        Point position = new Point();
        /// <summary>
        /// Gets or sets the Selector Position.
        /// </summary>
        /// <value>The position.</value>
        Point Position
        {
            get { return position; }
            set
            {
                Point newPos = RestrictedPosition(value);
                if (position != newPos)
                {
                    position = newPos;
                    Color color = PickColor(position.X, position.Y);
                    InvalidateVisual();
                    SetValue(SelectedColorPropertyKey, color);
                    ColorChanged(this, new ColorChangedEventArgs() { color = color });
                }
            }
        }

        /// <summary>
        /// Get the position restricted by the element bounds.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns></returns>
        Point RestrictedPosition(Point point)
        {
            double x = point.X, y = point.Y;

            if (x < 0)
                x = 0;
            else if (x > ActualWidth)
                x = ActualWidth;

            if (y < 0)
                y = 0;
            else if (y > ActualHeight)
                y = ActualHeight;

            return new Point(x, y);
        }

        /// <summary>
        /// Sets the <paramref name="pt"/> as the new position if the point falls 
        /// into the element bounds.
        /// </summary>
        /// <param name="pt">The point.</param>
        void SetPositionIfInBounds(Point pt)
        {
            if (pt.X >= 0 && pt.X <= ActualWidth && pt.Y >= 0 && pt.Y <= ActualHeight)
                Position = pt;

        }

        #endregion Position

        #region TargetBitmap
        RenderTargetBitmap cachedTargetBitmap;
        /// <summary>
        /// Gets the target bitmap for the DrawingImage image Source.
        /// </summary>
        /// <value>The target bitmap.</value>
        RenderTargetBitmap TargetBitmap
        {
            get
            {
                if (cachedTargetBitmap == null)
                {
                    DrawingImage drawingImage = Source as DrawingImage;
                    if (drawingImage != null)
                    {
                        DrawingVisual drawingVisual = new DrawingVisual();
                        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                        {
                            drawingContext.DrawDrawing(drawingImage.Drawing);
                        }

                        // Scale the DrawingVisual.
                        Rect dvRect = drawingVisual.ContentBounds;
                        drawingVisual.Transform = new ScaleTransform(ActualWidth / dvRect.Width
                            , ActualHeight / dvRect.Height);

                        cachedTargetBitmap = new RenderTargetBitmap((int)ActualWidth
                            , (int)ActualHeight, 96, 96, PixelFormats.Pbgra32);
                        cachedTargetBitmap.Render(drawingVisual);
                    }
                }
                return cachedTargetBitmap;
            }
        }
        #endregion TargetBitmap

        #region Mouse gesture handling
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            SetPositionIfInBounds(e.GetPosition(this));
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            SetPositionIfInBounds(e.GetPosition(this));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
                SetPositionIfInBounds(e.GetPosition(this));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePoint = e.GetPosition(this);
                Position = new Point(mousePoint.X, mousePoint.Y);
            }
        }
        #endregion Mouse gesture handling

        /// <summary>
        /// Picks the color at the position specified.
        /// </summary>
        /// <param name="x">The x coordinate in WPF pixels.</param>
        /// <param name="y">The y coordinate in WPF pixels.</param>
        /// <returns>The image pixel color at x,y position.</returns>
        /// <remarks>
        /// Input coordinates are scaled according to the underlying image resolution,
        /// so this method doesn't expect exceptions thrown by the 
        /// <see cref="M:System.Windows.Media.Imaging.BitmapSource.CopyPixels"/> method.
        /// <para>Color can be picked not only from the 
        /// <see cref="T:System.Windows.Media.Imaging.BitmapSource"/>, but also from the
        /// <see cref="T:System.Windows.Media.DrawingImage"/>.</para>
        /// </remarks>
        Color PickColor(double x, double y)
        {
            if (Source == null)
                throw new InvalidOperationException("Image Source not set");

            BitmapSource bitmapSource = Source as BitmapSource;
            if (bitmapSource != null)
            { // Get color from bitmap pixel.
                // Convert coopdinates from WPF pixels to Bitmap pixels and restrict them by the Bitmap bounds.
                x *= bitmapSource.PixelWidth / ActualWidth;
                if ((int)x > bitmapSource.PixelWidth - 1)
                    x = bitmapSource.PixelWidth - 1;
                else if (x < 0)
                    x = 0;
                y *= bitmapSource.PixelHeight / ActualHeight;
                if ((int)y > bitmapSource.PixelHeight - 1)
                    y = bitmapSource.PixelHeight - 1;
                else if (y < 0)
                    y = 0;

                // Lee Brimelow approach (http://thewpfblog.com/?p=62).
                //byte[] pixels = new byte[4];
                //CroppedBitmap cb = new CroppedBitmap(bitmapSource, new Int32Rect((int)x, (int)y, 1, 1));
                //cb.CopyPixels(pixels, 4, 0);
                //return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);

                // Alternative approach
                if (bitmapSource.Format == PixelFormats.Indexed4)
                {
                    byte[] pixels = new byte[1];
                    int stride = (bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel + 3) / 4;
                    bitmapSource.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), pixels, stride, 0);

                    Debug.Assert(bitmapSource.Palette != null, "bitmapSource.Palette != null");
                    Debug.Assert(bitmapSource.Palette.Colors.Count == 16, "bitmapSource.Palette.Colors.Count == 16");
                    return bitmapSource.Palette.Colors[pixels[0] >> 4];
                }
                else if (bitmapSource.Format == PixelFormats.Indexed8)
                {
                    byte[] pixels = new byte[1];
                    int stride = (bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel + 7) / 8;
                    bitmapSource.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), pixels, stride, 0);

                    Debug.Assert(bitmapSource.Palette != null, "bitmapSource.Palette != null");
                    Debug.Assert(bitmapSource.Palette.Colors.Count == 256, "bitmapSource.Palette.Colors.Count == 256");
                    return bitmapSource.Palette.Colors[pixels[0]];
                }
                else
                {
                    byte[] pixels = new byte[4];
                    int stride = (bitmapSource.PixelWidth * bitmapSource.Format.BitsPerPixel + 7) / 8;
                    bitmapSource.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), pixels, stride, 0);

                    return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
                }
                // TODO There are other PixelFormats which processing should be added if desired.
            }

            DrawingImage drawingImage = Source as DrawingImage;
            if (drawingImage != null)
            { // Get color from drawing pixel.
                RenderTargetBitmap targetBitmap = TargetBitmap;
                Debug.Assert(targetBitmap != null, "targetBitmap != null");

                // Convert coopdinates from WPF pixels to Bitmap pixels and restrict them by the Bitmap bounds.
                x *= targetBitmap.PixelWidth / ActualWidth;
                if ((int)x > targetBitmap.PixelWidth - 1)
                    x = targetBitmap.PixelWidth - 1;
                else if (x < 0)
                    x = 0;
                y *= targetBitmap.PixelHeight / ActualHeight;
                if ((int)y > targetBitmap.PixelHeight - 1)
                    y = targetBitmap.PixelHeight - 1;
                else if (y < 0)
                    y = 0;

                // TargetBitmap is always in PixelFormats.Pbgra32 format.
                // Pbgra32 is a sRGB format with 32 bits per pixel (BPP). Each channel (blue, green, red, and alpha)
                // is allocated 8 bits per pixel (BPP). Each color channel is pre-multiplied by the alpha value. 
                byte[] pixels = new byte[4];
                int stride = (targetBitmap.PixelWidth * targetBitmap.Format.BitsPerPixel + 7) / 8;
                targetBitmap.CopyPixels(new Int32Rect((int)x, (int)y, 1, 1), pixels, stride, 0);
                return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
            }

            throw new InvalidOperationException("Unsupported Image Source Type");
        }
    }
}
