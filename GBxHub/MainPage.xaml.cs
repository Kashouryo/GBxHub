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
using System.IO;
using System.Reflection;

namespace GBxHub
{
    /// <summary>
    /// mainPage.xaml 的交互逻辑
    /// </summary>
    public partial class mainPage : Page
    {
        ImageBrush gbxRead = new ImageBrush();
        ImageBrush gbxWrite = new ImageBrush();
        ImageBrush gbxUtil = new ImageBrush();
        public mainPage()
        {
            InitializeComponent();
            gbxRead.Stretch = Stretch.Uniform;
            gbxWrite.Stretch = Stretch.Uniform;
            gbxUtil.Stretch = Stretch.Uniform;
            gbxRead.ImageSource = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets/gbxread.png"), UriKind.Absolute));
            gbxWrite.ImageSource = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets/gbxwrite.png"), UriKind.Absolute));
            gbxUtil.ImageSource = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets/gbxutil.png"), UriKind.Absolute));
            readerButton.Background = gbxRead;
            writerButton.Background = gbxWrite;
            utilButton.Background = gbxUtil;
        }

        private void readerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new Uri("ReaderPage.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void writerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new Uri("WriterPage.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
