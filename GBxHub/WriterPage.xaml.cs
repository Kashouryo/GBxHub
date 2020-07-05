using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;

namespace GBxHub
{
    /// <summary>
    /// WriterPage.xaml 的交互逻辑
    /// </summary>
    public partial class WriterPage
    {
        private readonly Dictionary<string, int> _cartTypes = new Dictionary<string, int>
        {
            {"insideGadgets 32KB (incl 4KB FRAM) Cart", 1},
            {"insideGadgets 512KB Cart", 29},
            {"insideGadgets 1MB 128KB SRAM Cart", 30},
            {"insideGadgets 1MB 128KB SRAM Custom Logo Cart", 31},
            {"insideGadgets 2MB 128KB SRAM Cart", 2},
            {"insideGadgets 2MB 32KB FRAM Cart", 3},
            {"insideGadgets 4MB 128KB SRAM/FRAM Cart", 4},
            {"insideGadgets 4MB 32KB FRAM MBC3 RTC Flash Cart", 35},
            {"insideGadgets 64MB 128KB SRAM Cart", 5},
            {"insideGadgets 64MB 128KB SRAM Cart (Exp)", 6},
            {"32KB AM29F010B (WR)", 102},
            {"32KB AM29F010B (Audio)", 101},
            {"32KB SST39SF010A / AT49F040 (WR)", 104},
            {"32KB SST39SF010A / AT49F040 (Audio)", 103},
            {"512KB SST39SF040", 8},
            {"512KB AM29LV160 with CPLD", 32},
            {"1MB ES29LV160", 9},
            {"1MB 29LV320 with CPLD", 33},
            {"2MB BV5", 10},
            {"2MB AM29LV160DB / 29LV160CTTC / 29LV160TE", 11},
            {"2MB AM29F016B / 4MB AM29F032B", 12},
            {"2MB AM29F016B / 4MB AM29F032B (Audio as WE)", 34},
            {"2MB GB Smart 16M", 13},
            {"4MB M29W640 / 29DL32BF / GL032A10BAIR4 / S29AL016M9", 14},
            {"4MB AM29F032B / MBM29F033C", 15},
            {"32MB (4x 8MB Banks) 256M29", 16},
            {"32MB (4x 8MB Banks) M29W256 / MX29GL256", 17},
            {"insideGadgets 32MB (512Kbit/1Mbit Flash Save) or (256Kbit FRAM) Flash Cart", 20},
            {"insideGadgets 32MB 4Kbit/64Kbit EEPROM Flash Cart", 27},
            {"4MB MX29LV320", 26},
            {"16MB MSP55LV128 / 29LV128DTMC", 21},
            {"16MB MSP55LV128M / 29GL128EHMC / MX29GL128ELT / M29W128 / S29GL128 / 32MB 256M29EWH", 22},
            {"16MB M36L0R706 / 32MB 256L30B / 4455LLZBQO / 4000L0YBQ0", 23},
            {"16MB M36L0R706 (2) / 32MB 256L30B (2) / 4455LLZBQO (2) / 4000L0YBQ0 (2)", 24},
            {"16MB GE28F128W30", 25}
        };

        public WriterPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void flashButton_Click(object sender, RoutedEventArgs e)
        {
            if (_cartTypes.TryGetValue(chipTypeComboBox.Text, out var writeRomCartType))
            {
                var process = new Process();
                var startInfo = new ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal, FileName = "bin\\flasher.exe",
                    Arguments = @" \" + romPathTextBox.Text + @"\ " + writeRomCartType

                };

                process.StartInfo = startInfo;

                try
                {
                    process.Start();
                }
                catch (Exception)
                {
                    MessageBox.Show("flasher.exe could not be found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Invalid chip type selected, please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void romSelectButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog {FileName = "*gb; *.gbc; *.gba"};

            if (!(bool)openFileDialog1.ShowDialog()) return;
            flashButton.IsEnabled = true;
            romPathTextBox.Text = openFileDialog1.FileName;
        }
    }
}
