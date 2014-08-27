using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ColorPicker.ViewModelBase;
using Ookii.Dialogs.Wpf;
using System.Windows.Media.Imaging;

namespace ColorPicker.ViewModels
{
    class MainViewModel : PropertyChangedBase
    {
        #region "Singleton & Constructor"
        private static MainViewModel _instance;
        public static MainViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainViewModel();
                return _instance;
            }
        }


        private MainViewModel()
        {
            SelectedColor = Colors.White;
        }
        #endregion


        private Color selectedcolor;
        public Color SelectedColor
        {
            get { return selectedcolor; }
            set
            {
                SetProperty(value, ref selectedcolor);
            }
        }

        private BitmapImage selectedimage;
        public BitmapImage SelectedImage
        {
            get { return selectedimage; }
            set
            {
                SetProperty(value, ref selectedimage);
            }
        }

        private RelayCommand openfilecommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                if (openfilecommand == null)
                    openfilecommand = new RelayCommand((object parameter) =>
                    {
                        VistaOpenFileDialog ofd = new VistaOpenFileDialog();
                        ofd.CheckFileExists = true;
                        ofd.Filter = "Bilddateien (JPG, PNG, BMP, GIF)|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Alle Dateien|*.*";
                        if (ofd.ShowDialog() == true)
                        {
                            try
                            {
                                SelectedImage = new BitmapImage(new Uri(ofd.FileName, UriKind.Absolute));
                            }
                            catch (Exception)
                            {
                                System.Windows.MessageBox.Show("Das Format konnte nicht erkannt werden oder wird nicht unterstüzt", "Fehler", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                            }
                        }
                    });
                return openfilecommand;
            }
        }

        
        private System.Windows.WindowState windowstate;
        public System.Windows.WindowState WindowState
        {
            get { return windowstate; }
            set
            {
                SetProperty(value, ref windowstate);
            }
        }

        private RelayCommand fromscreenshot;
        public RelayCommand FromScreenshot
        {
            get
            {
                if (fromscreenshot == null)
                    fromscreenshot = new RelayCommand((object parameter) =>
                    {
                        WindowState = System.Windows.WindowState.Minimized;
                        System.Threading.Thread.Sleep(500); //wait for the minimized animation
                        Views.PickColorWindow window = new Views.PickColorWindow();
                        window.Show();
                        window.BringIntoView();
                        window.Closed += (s,e) =>
                        {
                            WindowState = System.Windows.WindowState.Normal;
                            SelectedColor = window.SelectedColor;
                        };
                    });
                return fromscreenshot;
            }
        }

        private RelayCommand makelighter;
        public RelayCommand MakeLighter
        {
            get
            {
                if (makelighter == null)
                    makelighter = new RelayCommand((object parameter) => { SelectedColor = SelectedColor.ChangeLightness(0.2); });
                return makelighter;
            }
        }

        public List<ColorItem> SystemColors
        {
            get
            {
                List<ColorItem> ColorList = new List<ColorItem>() { new ColorItem("ActiveBorderColor", System.Windows.SystemColors.ActiveBorderColor),
                new ColorItem("ActiveCaptionColor", System.Windows.SystemColors.ActiveCaptionColor),
                new ColorItem("ActiveCaptionTextColor", System.Windows.SystemColors.ActiveCaptionTextColor),
                new ColorItem("AppWorkspaceColor", System.Windows.SystemColors.AppWorkspaceColor),
                new ColorItem("ControlColor", System.Windows.SystemColors.ControlColor),
                new ColorItem("ControlDarkColor", System.Windows.SystemColors.ControlDarkColor),
                new ColorItem("ControlDarkDarkColor", System.Windows.SystemColors.ControlDarkDarkColor),
                new ColorItem("ControlLightColor", System.Windows.SystemColors.ControlLightColor),
                new ColorItem("ControlLightLightColor", System.Windows.SystemColors.ControlLightLightColor),
                new ColorItem("ControlTextColor", System.Windows.SystemColors.ControlTextColor),
                new ColorItem("DesktopColor", System.Windows.SystemColors.DesktopColor),
                new ColorItem("GradientActiveCaptionColor", System.Windows.SystemColors.GradientActiveCaptionColor),
                new ColorItem("GradientInactiveCaptionColor", System.Windows.SystemColors.GradientInactiveCaptionColor),
                new ColorItem("GrayTextColor", System.Windows.SystemColors.GrayTextColor),
                new ColorItem("HighlightColor", System.Windows.SystemColors.HighlightColor),
                new ColorItem("HighlightTextColor", System.Windows.SystemColors.HighlightTextColor),
                new ColorItem("HotTrackColor", System.Windows.SystemColors.HotTrackColor),
                new ColorItem("InactiveBorderColor", System.Windows.SystemColors.InactiveBorderColor),
                new ColorItem("InactiveCaptionColor", System.Windows.SystemColors.InactiveCaptionColor),
                new ColorItem("InactiveCaptionTextColor", System.Windows.SystemColors.InactiveCaptionTextColor),
                new ColorItem("InfoColor", System.Windows.SystemColors.InfoColor),
                new ColorItem("InfoTextColor", System.Windows.SystemColors.InfoTextColor),
                new ColorItem("MenuBarColor", System.Windows.SystemColors.MenuBarColor),
                new ColorItem("MenuColor", System.Windows.SystemColors.MenuColor),
                new ColorItem("MenuHighlightColor", System.Windows.SystemColors.MenuHighlightColor),
                new ColorItem("MenuTextColor", System.Windows.SystemColors.MenuTextColor),
                new ColorItem("ScrollBarColor", System.Windows.SystemColors.ScrollBarColor),
                new ColorItem("WindowColor", System.Windows.SystemColors.WindowColor),
                new ColorItem("WindowFrameColor", System.Windows.SystemColors.WindowFrameColor),
                new ColorItem("WindowTextColor", System.Windows.SystemColors.WindowTextColor)};
                foreach(ColorItem item in ColorList)
                    item.WantToSet += item_WantToSet;
                return ColorList;
            }
        }

        void item_WantToSet(object sender, EventArgs e)
        {
            this.SelectedColor = ((ColorItem)sender).BaseColor;
        }

        public class ColorItem
        {
            public string Name { get; set; }
            public Color BaseColor { get; set; }
            public event EventHandler WantToSet;

            public Brush BaseBrush
            {
                get
                {
                    return new SolidColorBrush(BaseColor);
                }
            }

            private RelayCommand setcolor;
            public RelayCommand SetColor
            {
                get
                {
                    if (setcolor == null)
                        setcolor = new RelayCommand((object parameter) => { if (WantToSet != null) WantToSet(this, EventArgs.Empty); });
                    return setcolor;
                }
            }

            public ColorItem(string name, Color color)
            {
                this.Name = name;
                this.BaseColor = color;
            }
        }
    }
}
