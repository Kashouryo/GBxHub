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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace GBxHub
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var logoImg = new ImageBrush();
            logoImg.ImageSource = new BitmapImage(new Uri(".\\assets\\igcartsmall.png", UriKind.Relative));
            logoImage.Fill = logoImg;
            if (File.Exists(".\\assets\\background.png"))
            {
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(".\\assets\\background.png", UriKind.Relative));
                bgRectangle.Fill = brush;
            }
            if (File.Exists(".\\assets\\gbxlogo.png"))
            {
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(".\\assets\\gbxlogo.png", UriKind.Relative));
                gbxImageLogo.Fill = brush;
            }
            else {
                logoLabel.Visibility = Visibility.Visible;
            }
            versionLabel.Content = App.version.ToString(".0") + " " + (App.beta ? "beta" : "stable");
            mainFrame.NavigationService.Navigate(new mainPage());
        }

    }
}
