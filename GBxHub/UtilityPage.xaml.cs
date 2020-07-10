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
using System.Reflection;

namespace GBxHub
{
    /// <summary>
    /// UtilityPage.xaml 的交互逻辑
    /// </summary>
    public partial class UtilityPage : Page
    {
        ImageBrush appAddress = new ImageBrush();
        public UtilityPage()
        {
            InitializeComponent();
            appAddress.Stretch = Stretch.Uniform;
            appAddress.ImageSource = new BitmapImage(new Uri(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets/util_address.png"), UriKind.Absolute));
            appAddressHelperButton.Background = appAddress;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void utilButton_Click(object sender, RoutedEventArgs e)
        {
            AddressHelper addressHelper = new AddressHelper();
            addressHelper.Show();
        }
    }
}
