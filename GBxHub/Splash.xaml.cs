using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace GBxHub
{
    /// <summary>
    /// Splash.xaml 的交互逻辑
    /// </summary>
    public partial class Splash : Window
    {
        private DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private int seq = 0;
        public Splash()
        {
            InitializeComponent();
            if (File.Exists(".\\assets\\gbxlogo.png"))
            {
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri(".\\assets\\gbxlogo.png", UriKind.Relative));
                gbxImageLogo.Fill = brush;
            }
            else
            {
                logoLabel.Visibility = Visibility.Visible;
            }
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            switch (seq)
            {
                case 0:
                    seq++;
                    loadingMessage.Text = "Loading: Please wait";
                    break;
                case 1:
                    seq++;
                    dispatcherTimer.Stop();
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                    break;
                default:
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
