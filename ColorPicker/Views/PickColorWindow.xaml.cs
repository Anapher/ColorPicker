using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace ColorPicker.Views
{
    /// <summary>
    /// Interaktionslogik für PickColorWindow.xaml
    /// </summary>
    public partial class PickColorWindow : Window
    {
        System.Drawing.Bitmap bmp;

        public PickColorWindow()
        {
            InitializeComponent();
            bmp = new System.Drawing.Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);

            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            img.Source = bitmapImage;
        }

        public Color SelectedColor { get; set; }

        private void img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(img);
            System.Drawing.Color scolor = bmp.GetPixel((int)p.X, (int)p.Y);
            SelectedColor = Color.FromRgb(scolor.R, scolor.G, scolor.B);
            this.Close();
        }
    }
}
