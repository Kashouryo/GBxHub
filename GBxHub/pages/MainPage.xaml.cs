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

namespace GBxHub
{
    /// <summary>
    /// mainPage.xaml 的交互逻辑
    /// </summary>
    public partial class mainPage : Page
    {
        public mainPage()
        {
            InitializeComponent();
        }

        private void readerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService ns = NavigationService.GetNavigationService(this);
                ns.Navigate(new Uri("pages/ReaderPage.xaml", UriKind.Relative));
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
                ns.Navigate(new Uri("pages/WriterPage.xaml", UriKind.Relative));
            }
            catch (Exception)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
