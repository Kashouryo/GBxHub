using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GBxHub.pages
{
    /// <summary>
    /// WriterPage.xaml 的交互逻辑
    /// </summary>
    public partial class WriterPage : Page
    {
        public WriterPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void flashButton_Click(object sender, RoutedEventArgs e)
        {
            int writeRomCartType = 0;
            if (chipTypeComboBox.Text == "insideGadgets 32KB (incl 4KB FRAM) Cart")
            {
                writeRomCartType = 1;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 512KB Cart")
            {
                writeRomCartType = 29;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 1MB 128KB SRAM Cart")
            {
                writeRomCartType = 30;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 1MB 128KB Custom Logo SRAM Cart")
            {
                writeRomCartType = 31;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 2MB 128KB SRAM Cart")
            {
                writeRomCartType = 2;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 2MB 32KB FRAM Cart")
            {
                writeRomCartType = 3;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 4MB 128KB SRAM/FRAM Cart")
            {
                writeRomCartType = 4;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 4MB 32KB FRAM MBC3 RTC Flash Cart")
            {
                writeRomCartType = 35;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 64MB 128KB SRAM Cart")
            {
                writeRomCartType = 5;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 64MB 128KB SRAM Cart (Exp)")
            {
                writeRomCartType = 6;
            }


            else if (chipTypeComboBox.Text == "32KB AM29F010B (WR)")
            {
                writeRomCartType = 102;
            }
            else if (chipTypeComboBox.Text == "32KB AM29F010B (Audio)")
            {
                writeRomCartType = 101;
            }
            else if (chipTypeComboBox.Text == "32KB SST39SF010A / AT49F040 (WR)")
            {
                writeRomCartType = 104;
            }
            else if (chipTypeComboBox.Text == "32KB SST39SF010A / AT49F040 (Audio)")
            {
                writeRomCartType = 103;
            }


            else if (chipTypeComboBox.Text == "512KB SST39SF040")
            {
                writeRomCartType = 8;
            }
            else if (chipTypeComboBox.Text == "512KB AM29LV160 with CPLD")
            {
                writeRomCartType = 32;
            }
            else if (chipTypeComboBox.Text == "1MB ES29LV160")
            {
                writeRomCartType = 9;
            }
            else if (chipTypeComboBox.Text == "1MB 29LV320 with CPLD")
            {
                writeRomCartType = 33;
            }
            else if (chipTypeComboBox.Text == "2MB BV5")
            {
                writeRomCartType = 10;
            }
            else if (chipTypeComboBox.Text == "2MB AM29LV160DB / 29LV160CTTC / 29LV160TE")
            {
                writeRomCartType = 11;
            }
            else if (chipTypeComboBox.Text == "2MB AM29F016B / 4MB AM29F032B")
            {
                writeRomCartType = 12;
            }
            else if (chipTypeComboBox.Text == "2MB AM29F016B / 4MB AM29F032B (Audio as WE)")
            {
                writeRomCartType = 34;
            }
            else if (chipTypeComboBox.Text == "2MB GB Smart 16M")
            {
                writeRomCartType = 13;
            }
            else if (chipTypeComboBox.Text == "4MB M29W640 / 29DL32BF / GL032A10BAIR4 / S29AL016M9")
            {
                writeRomCartType = 14;
            }
            else if (chipTypeComboBox.Text == "4MB AM29F032B / MBM29F033C")
            {
                writeRomCartType = 15;
            }
            else if (chipTypeComboBox.Text == "32MB (4x 8MB Banks) 256M29")
            {
                writeRomCartType = 16;
            }
            else if (chipTypeComboBox.Text == "32MB (4x 8MB Banks) M29W256 / MX29GL256")
            {
                writeRomCartType = 17;
            }



            // GBA
            else if (chipTypeComboBox.Text == "insideGadgets 32MB (512Kbit/1Mbit Flash Save) or (256Kbit FRAM) Flash Cart")
            {
                writeRomCartType = 20;
            }
            else if (chipTypeComboBox.Text == "insideGadgets 32MB 4Kbit/64Kbit EEPROM Flash Cart")
            {
                writeRomCartType = 27;
            }
            else if (chipTypeComboBox.Text == "4MB MX29LV320")
            {
                writeRomCartType = 26;
            }
            else if (chipTypeComboBox.Text == "16MB MSP55LV128 / 29LV128DTMC")
            {
                writeRomCartType = 21;
            }
            else if (chipTypeComboBox.Text == "16MB MSP55LV128M / 29GL128EHMC / MX29GL128ELT / M29W128 / S29GL128 / 32MB 256M29EWH")
            {
                writeRomCartType = 22;
            }
            else if (chipTypeComboBox.Text == "16MB M36L0R706 / 32MB 256L30B / 4455LLZBQO / 4000L0YBQ0")
            {
                writeRomCartType = 23;
            }
            else if (chipTypeComboBox.Text == "16MB M36L0R706 (2) / 32MB 256L30B (2) / 4455LLZBQO (2) / 4000L0YBQ0 (2)")
            {
                writeRomCartType = 24;
            }
            else if (chipTypeComboBox.Text == "16MB GE28F128W30")
            {
                writeRomCartType = 25;
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = "bin\\flasher.exe";

            startInfo.Arguments += " \"";
            startInfo.Arguments += romPathTextBox.Text;
            startInfo.Arguments += "\" ";
            startInfo.Arguments += writeRomCartType;

            process.StartInfo = startInfo;
            process.Start();
        }

        private void romSelectButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = this.directoryNameToolStripMenuItem.Text;
            openFileDialog1.FileName = "*gb; *.gbc; *.gba";

            if ((bool)openFileDialog1.ShowDialog())
            {
                FileInfo fileSelected = new FileInfo(openFileDialog1.FileName);
                // Store the last rom directory opened
                //this.directoryNameToolStripMenuItem.Text = Path.GetDirectoryName(openFileDialog1.FileName);
                flashButton.IsEnabled = true;
                romPathTextBox.Text = openFileDialog1.FileName;
            }
        }
    }
}
