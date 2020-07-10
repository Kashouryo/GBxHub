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

namespace GBxHub
{
    /// <summary>
    /// AddressHelper.xaml 的交互逻辑
    /// </summary>
    public partial class AddressHelper : Window
    {
        public AddressHelper()
        {
            InitializeComponent();
        }

        private void addressCheckBox_Click(object sender, RoutedEventArgs e)
        {
            int adrNumber = 0;
            adrNumber += Convert.ToInt32(a0CheckBox.IsChecked ?? false) * 1;
            adrNumber += Convert.ToInt32(a1CheckBox.IsChecked ?? false) * 2;
            adrNumber += Convert.ToInt32(a2CheckBox.IsChecked ?? false) * 4;
            adrNumber += Convert.ToInt32(a3CheckBox.IsChecked ?? false) * 8;
            adrNumber += Convert.ToInt32(a4CheckBox.IsChecked ?? false) * 16;
            adrNumber += Convert.ToInt32(a5CheckBox.IsChecked ?? false) * 32;
            adrNumber += Convert.ToInt32(a6CheckBox.IsChecked ?? false) * 64;
            adrNumber += Convert.ToInt32(a7CheckBox.IsChecked ?? false) * 128;
            adrNumber += Convert.ToInt32(a8CheckBox.IsChecked ?? false) * 256;
            adrNumber += Convert.ToInt32(a9CheckBox.IsChecked ?? false) * 512;
            adrNumber += Convert.ToInt32(a10CheckBox.IsChecked ?? false) * 1024;
            adrNumber += Convert.ToInt32(a11CheckBox.IsChecked ?? false) * 2048;
            adrNumber += Convert.ToInt32(a12CheckBox.IsChecked ?? false) * 4096;
            adrNumber += Convert.ToInt32(a13CheckBox.IsChecked ?? false) * 8192;
            adrNumber += Convert.ToInt32(a14CheckBox.IsChecked ?? false) * 16384;
            adrNumber += Convert.ToInt32(a15CheckBox.IsChecked ?? false) * 32768;
            addressBox.Text = adrNumber.ToString("X");
            addressDecBox.Text = adrNumber.ToString();
        }
    }
}
