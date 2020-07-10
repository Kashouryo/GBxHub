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
            string description = "No information available";
            bool wrEnabled = wrCheckBox.IsChecked ?? false;
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
            string addrStr = adrNumber.ToString("X");
            addressBox.Text = addrStr;
            addressDecBox.Text = adrNumber.ToString();

            if (noMapperRadioButton.IsChecked ?? false)
            {
                if (adrNumber <= 32767)
                {
                    int bankNum = (adrNumber <= 16383) ? 0 : 1;
                    description = "Gameboy can read data from ROM address " + adrNumber.ToString("X") + ".";
                    description += $"Which is in bank {bankNum}.";
                }
                else
                {
                    description = "Flash ROM disabled.";
                }

            }
            else if (mbc1RadioButton.IsChecked ?? false)
            {
                if (adrNumber <= 32767)
                {
                    if (wrEnabled)
                    {
                        //0000-1FFF - ROM Bank Number (Write Only)
                        if (adrNumber >= 0 && adrNumber <= 8191)
                        {
                            description = "Gameboy can write to here to enable or disable RAM:\n00h - Disable RAM (default)\n0Ah = Enable RAM";
                        }
                        //2000-3FFF - ROM Bank Number (Write Only)
                        else if(adrNumber >= 8192 && adrNumber <= 16383)
                        {
                            description = "Gameboy can write additional address to MBC with data line";
                        }
                        //4000-5FFF - RAM Bank Number - or - Upper Bits of ROM Bank Number (Write Only)
                        else if (adrNumber >= 16384 && adrNumber <= 24575)
                        {
                            description = "This 2bit register can be used to select a RAM Bank in range from 00-03h, or to specify the upper two bits (Bit 5-6) of the ROM Bank number, depending on the current ROM/RAM Mode.";
                        }
                        //6000-7FFF - ROM/RAM Mode Select (Write Only)
                        else if (adrNumber >= 24576 && adrNumber <= 32767)
                        {
                            description = "Gameboy can write to here to change ROM/RAM mode:\n00h - ROM Banking Mode (up to 8KByte RAM, 2MByte ROM) (default)\n01h - RAM Banking Mode (up to 32KByte RAM, 512KByte ROM)";
                        }
                    }
                    //0000-3FFF - ROM Bank 00 (Read Only)
                    else if (adrNumber <= 16383)
                    {
                        description = "Gameboy can read data from ROM address " + adrNumber.ToString("X") + ". Which is in bank 0.";
                    }
                    //4000-7FFF - ROM Bank 01-7F (Read Only)
                    else
                    {
                        description = "Gameboy can read data from ROM address which controlled by the MBC1";
                    }
                }
                //A000-BFFF - RAM Bank 00-03, if any (Read/Write)
                else if (adrNumber >= 40960 && adrNumber <= 49151)
                {
                    if (wrEnabled)
                    {
                        description = $"Gameboy can write data from RAM(if any) address {adrNumber.ToString("X")}.";
                    }
                    else
                    {
                        description = $"Gameboy can read data from RAM(if any) address {adrNumber.ToString("X")}.";
                    }
                }

            }
        meaningTextBlock.Text = description;
        }
    }
}
