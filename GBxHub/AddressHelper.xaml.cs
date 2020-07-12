using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            var bNum = 0;
            var description = "No information available";
            var wrEnabled = wrCheckBox.IsChecked ?? false;
            var adrNumber = checkBoxGrid.Children.OfType<CheckBox>().Sum(ctrl => Convert.ToInt32(ctrl.IsChecked ?? false) * (1 << bNum++));
            var addrStr = adrNumber.ToString("X");

            addressBox.Text = addrStr;
            addressDecBox.Text = adrNumber.ToString();

            if (noMapperRadioButton.IsChecked ?? false)
            {
                if (adrNumber <= 0x7FFF)
                {
                    var bankNum = (adrNumber <= 0x3FFF) ? 0 : 1;
                    description = $"Gameboy can read data from ROM address {adrNumber:X}.";
                    description += $"Which is in bank {bankNum}.";
                }
                else
                    description = "Flash ROM disabled.";
            }
            else if (mbc1RadioButton.IsChecked ?? false)
            {
                if (adrNumber <= 0x7FFF)
                {
                    if (wrEnabled)
                    {
                        //0000-1FFF - ROM Bank Number (Write Only)
                        if (Enumerable.Range(0x0, 0x1FFF).Contains(adrNumber))
                            description = "Gameboy can write to here to enable or disable RAM:\n00h - Disable RAM (default)\n0Ah = Enable RAM";
                        //2000-3FFF - ROM Bank Number (Write Only)
                        else if (Enumerable.Range(0x2000, 0x3FFF).Contains(adrNumber))
                            description = "Gameboy can write additional address to MBC with data line";
                        //4000-5FFF - RAM Bank Number - or - Upper Bits of ROM Bank Number (Write Only)
                        else if (Enumerable.Range(0x4000, 0x5FFF).Contains(adrNumber))
                            description = "This 2bit register can be used to select a RAM Bank in range from 00-03h, or to specify the upper two bits (Bit 5-6) of the ROM Bank number, depending on the current ROM/RAM Mode.";
                        //6000-7FFF - ROM/RAM Mode Select (Write Only)
                        else if (Enumerable.Range(0x6000, 0x7FFF).Contains(adrNumber))
                            description = "Gameboy can write to here to change ROM/RAM mode:\n00h - ROM Banking Mode (up to 8KByte RAM, 2MByte ROM) (default)\n01h - RAM Banking Mode (up to 32KByte RAM, 512KByte ROM)";
                    }
                    //0000-3FFF - ROM Bank 00 (Read Only)
                    else if (adrNumber <= 0x3FFF)
                        description = $"Gameboy can read data from ROM address {adrNumber:X}. Which is in bank 0.";
                    //4000-7FFF - ROM Bank 01-7F (Read Only)
                    else
                        description = "Gameboy can read data from ROM address which controlled by the MBC1";
                }
                //A000-BFFF - RAM Bank 00-03, if any (Read/Write)
                else if (Enumerable.Range(0xA000, 0xBFFF).Contains(adrNumber))
                    description = wrEnabled ? $"Gameboy can write data from RAM(if any) address {adrNumber:X}." : $"Gameboy can read data from RAM(if any) address {adrNumber:X}.";
            }
            meaningTextBlock.Text = description;
        }
    }
}