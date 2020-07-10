using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace GBxHub
{
    /// <summary>
    /// ReaderPage.xaml 的交互逻辑
    /// </summary>
    public partial class ReaderPage : Page
    {
        public const int GB_CART = 1;
        public const int GBA_CART = 2;
        const char CART_MODE = 'C';

        public const char VOLTAGE_3_3V = '3';
        public const char VOLTAGE_5V = '5';

        const int READROM = 1;
        public const int SAVERAM = 2;
        const int WRITERAM = 3;
        const int READHEADER = 4;
        const int WRITEROM = 5;
        const int ERASERAM = 6;
        const int ERASEROM = 7;

        public const int PCB_1_0 = 1;
        public const int PCB_1_1 = 2;
        public const int PCB_1_2 = 3;
        public const int PCB_1_3 = 4;
        public const int GBXMAS = 90;
        public const int MINI_PCB_1_0 = 100;
        public const char READ_PCB_VERSION = 'h';

        bool formMinimised = false;
        bool comConnected = false;
        public static int commandReceived = 0;
        bool headerRead = false;
        int cancelOperation = 0;
        UInt32 progress = 0;
        UInt32 progressPrevious = 0;
        int progressPreviousCounter = 0;
        int progressStalled = 0;

        public static string[] headerTokens = { "", "", "", "", "" };
        string writeRomFileName;
        bool writeRomSelected = false;
        public static bool saveAsNewFile = false;
        string writeSaveFileName;
        int alwaysAddDateTimeToSave = 0;
        int promptForRestoreSaveFile = 0;
        int reReadCartHeader = 0;
        public static int writeRomCartType = 0;
        public static UInt32 writeRomCartSize = 0;
        public static UInt32 writeRomSize = 0;
        public static int cartMode = GBA_CART;
        public static int gbxcartPcbVersion = 0;
        public static int chipEraseSelected = 1;

        private BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker2 = new BackgroundWorker();

        private ImageBrush pendingBrush = new ImageBrush();
        private ImageBrush connectedBrush = new ImageBrush();

        private Color goodColour = Color.FromRgb(116, 247, 134);
        private Color badColour = Color.FromRgb(164, 0, 0);
        public ReaderPage()
        {
            InitializeComponent();

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerAsync();

            backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker2_DoWork);
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.RunWorkerAsync();
        }

        // Progress bar
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                double progress_percent = progress;

                if (progress_percent >= 100)
                {
                    progress_percent = 100;
                }

                backgroundWorker1.ReportProgress(Convert.ToInt32(progress_percent));
                System.Threading.Thread.Sleep(100);
                //Console.WriteLine(progress);

                // Check if progress has stalled after 30 seconds
                if (commandReceived != 0 && comConnected == true)
                {
                    if (progressPreviousCounter >= 300)
                    {
                        if (progress != progressPrevious)
                        {
                            progressPrevious = progress;
                            progressPreviousCounter = 0;
                        }
                        else if (progressStalled == 0)
                        {
                            statuslabel.Dispatcher.Invoke(new Action(() => {
                                statuslabel.Content = statuslabel.Content + " Stalled";
                            }));
                            progressStalled = 1;
                            //Functions.MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                            //TopMost = true;
                            //System.Windows.Forms.MessageBox.Show("Progress has stalled. Please unplug GBxCart RW, re-seat your game cartridge and try again.");

                            MessageBox.Show("Progress has stalled. Please unplug GBxCart RW, re-seat your game cartridge and try again. This program will close when you press Ok.", "GBxCart RW Stalled");
                            //MessageBox.Show("Progress has stalled. Please unplug GBxCart RW, re-seat your game cartridge and try again.", "GBxCart RW Stalled", MessageBoxButtons.OK, MessageBoxIcon.None,
                            // MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);  // MB_TOPMOST
                            Application.Current.Shutdown();
                        }
                    }
                    progressPreviousCounter++;
                }
                else
                {
                    progressPrevious = 0;
                    progressPreviousCounter = 0;
                    progressStalled = 0;
                }
            }
        }

        // Update the progress bar
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // The progress percentage is a property of e
            progressBar1.Value = e.ProgressPercentage;

#if WINDOWS_7_BUILD
            try {
                TaskbarProgress.SetValue(this.Handle, e.ProgressPercentage, 100);

                if (progressStalled == 1) {
                    TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Error);
                }
                else {
                    TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Normal);
                }
            }
            catch (ObjectDisposedException a) {
                Console.WriteLine("Caught: {0}", a.Message);
            }
#endif
        }

        // Perform the reading/writing/header read in the background
        void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {

                // Before doing any operations, check if we are still connected and read the mode we are in
                if (commandReceived != 0)
                {
                    int checkCom = App.read_firmware_version();
                    int checkComCounter = 0;
                    progressStalled = 0;
                    //Console.Write(checkCom);

                    // Read which mode we are in
                    /*if (checkCom >= 1) {
                        if (cartMode == GB_CART) {
                            modeTextBox.Invoke(new Action(() => {
                                modeTextBox.Text = "GB/GBC";
                            }));
                        }
                        else if (cartMode == GBA_CART) {
                            modeTextBox.Invoke(new Action(() => {
                                modeTextBox.Text = "GBA";
                            }));
                        }
                    }*/

                    while (checkCom == 0)
                    { // Not connected, check it a few times
                        checkCom = App.read_firmware_version();
                        System.Threading.Thread.Sleep(100);
                        checkComCounter++;
                        //Console.Write(checkCom);

                        if (checkComCounter >= 3)
                        { // Close the COM port
                            commandReceived = 0;

                            int comPortInt = Convert.ToInt32(comPortTextBox.Text);
                            comPortInt--;

                            App.RS232_CloseComport(comPortInt);
                            comConnected = false;

                            statuslabel.Dispatcher.Invoke(new Action(() => {
                                statuslabel.Content = "Device disconnected";
                            }));

                            // Change icon/buttons
                            statusRect.Dispatcher.Invoke(new Action(() => {
                                statusRect.Fill = new SolidColorBrush(badColour);
                            }));
                            connectButton.Dispatcher.Invoke(new Action(() => {
                                connectButton.Visibility = Visibility.Visible;
                            }));
                            disconnectButton.Dispatcher.Invoke(new Action(() => {
                                disconnectButton.Visibility = Visibility.Hidden;
                            }));

                            break;
                        }
                    }
                }

                if (commandReceived == READROM)
                { // Read ROM
                    App.read_rom(cartMode, ref progress, ref cancelOperation);
                    commandReceived = 0;
                    System.Threading.Thread.Sleep(500);

                    if (cancelOperation == 0)
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Reading ROM... Finished";
                        }));
                    }
                    else
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Reading ROM... Cancelled";
                        }));
                    }
                }
                else if (commandReceived == SAVERAM)
                { // Save RAM
                    App.read_ram(saveAsNewFile, ref progress, ref cancelOperation);
                    commandReceived = 0;
                    System.Threading.Thread.Sleep(500);

                    if (cancelOperation == 0)
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Backing up Save... Finished";
                        }));
                    }
                    else
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Backing up Save... Cancelled";
                        }));
                    }
                }
                else if (commandReceived == WRITERAM)
                { // Write RAM
                    App.write_ram(writeSaveFileName, ref progress, ref cancelOperation);
                    commandReceived = 0;
                    System.Threading.Thread.Sleep(500);

                    if (cancelOperation == 0)
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Restoring Save... Finished";
                        }));
                    }
                    else
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Restoring Save... Cancelled";
                        }));
                    }
                }
                else if (commandReceived == WRITEROM)
                { // Write ROM

                    // First erase the flash chip if needed or if selected
                    if (chipEraseSelected == 1)
                    {
                        if (writeRomCartType == 2 || writeRomCartType == 4 || writeRomCartType == 5 || writeRomCartType == 6 || writeRomCartType == 7 || writeRomCartType == 8 || writeRomCartType == 9 || writeRomCartType == 10 || writeRomCartType == 14 || writeRomCartType == 17 || writeRomCartType == 19)
                        {
                            App.erase_rom(writeRomCartType, ref progress, ref cancelOperation);
                            Console.WriteLine("ROm");
                            if (cancelOperation == 1)
                            {
                                commandReceived = 0;
                                System.Threading.Thread.Sleep(500);

                                statuslabel.Dispatcher.Invoke(new Action(() => {
                                    statuslabel.Content = "Erasing Flash... Cancelled";
                                }));
                            }
                            else
                            {
                                statuslabel.Dispatcher.Invoke(new Action(() => {
                                    statuslabel.Content = "Writing ROM...";
                                }));

                                progress = 0;
                                backgroundWorker1.ReportProgress(0);
                            }
                        }
                    }

                    // Program the flash chip
                    if (cancelOperation != 1)
                    {
                        progressPrevious = 0;
                        progressPreviousCounter = 0;

                        App.write_rom(writeRomFileName, writeRomCartType, writeRomSize, ref progress, ref cancelOperation);
                        commandReceived = 0;
                        System.Threading.Thread.Sleep(500);

                        if (cancelOperation == 0)
                        {
                            statuslabel.Dispatcher.Invoke(new Action(() => {
                                statuslabel.Content = "Writing ROM... Finished";
                            }));
                        }
                        else if (cancelOperation == 1)
                        {
                            statuslabel.Dispatcher.Invoke(new Action(() => {
                                statuslabel.Content = "Writing ROM... Cancelled";
                            }));
                        }
                        else if (cancelOperation == 2)
                        {
                            statuslabel.Dispatcher.Invoke(new Action(() => {
                                statuslabel.Content = "Writing ROM... Stalled";
                            }));
                            progressStalled = 1;
                        }
                    }
                }
                else if (commandReceived == ERASERAM)
                { // Erase RAM
                    App.erase_ram(ref progress, ref cancelOperation);
                    commandReceived = 0;
                    System.Threading.Thread.Sleep(500);

                    if (cancelOperation == 0)
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Erasing Save... Finished";
                        }));
                    }
                    else
                    {
                        statuslabel.Dispatcher.Invoke(new Action(() => {
                            statuslabel.Content = "Erasing Save... Cancelled";
                        }));
                    }
                }
                else if (commandReceived == READHEADER)
                { // Read Header
                    headerRead = true;
                    Int32 textLength = 0;
                    IntPtr headerPointer;
                    string headerText = "";

                    if (cartMode == GB_CART)
                    {
                        headerPointer = App.read_gb_header(ref textLength);
                        headerText = Marshal.PtrToStringAnsi(headerPointer, textLength);
                        headerTokens = headerText.Split('\n');
                    }
                    else if (cartMode == GBA_CART)
                    {
                        headerPointer = App.read_gba_header(ref textLength);
                        headerText = Marshal.PtrToStringAnsi(headerPointer, textLength);
                        headerTokens = headerText.Split('\n');
                    }
                    commandReceived = 0;

                    headerTextBox.Dispatcher.Invoke(new Action(() => {
                        //headerTextBox.Text = headerText;
                        headerTextBox.Text = headerTokens[0] + "\n" + headerTokens[1] + "\n" + headerTokens[2] + "\n" + headerTokens[3] + "\n" + headerTokens[4];
                    }));
                }

                cancelOperation = 0;
                System.Threading.Thread.Sleep(100);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (comConnected == true)
            {
                int comPortInt = Convert.ToInt32(comPortTextBox.Text);
                comPortInt--;

                App.RS232_CloseComport(comPortInt);
                comConnected = false;
            }
            NavigationService.GoBack();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            int comPortInt = Convert.ToInt32(comPortTextBox.Text);
            Int32 baudInt = Convert.ToInt32(baudTextBox.Text);
            comPortInt--;
            headerTextBox.Text = "";
            frimwareLabel.Content = "Error";

            // If port open, close it, we will open it again below
            if (comConnected == true)
            {
                App.RS232_CloseComport(comPortInt);
                comConnected = false;
            }
            else
            {
                // Try opening the port
                if (App.RS232_OpenComport(comPortInt, baudInt, "8N1") == 0)
                { // Port opened
                    App.update_config(comPortInt, baudInt, alwaysAddDateTimeToSave, promptForRestoreSaveFile, reReadCartHeader);

                    // See if device responds correctly
                    App.set_mode('0');
                    int cartridgeMode = App.request_value(CART_MODE);

                    // Responded correctly
                    if (cartridgeMode == GB_CART || cartridgeMode == GBA_CART)
                    {
                        comConnected = true;
                    }
                    App.RS232_CloseComport(comPortInt);
                }else
                {
                    frimwareLabel.Content = "Error";
                }

                if (comConnected == false)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        if (App.RS232_OpenComport(x, baudInt, "8N1") == 0)
                        { // Port opened
                            comPortInt = x;
                            App.update_config(comPortInt, baudInt, alwaysAddDateTimeToSave, promptForRestoreSaveFile, reReadCartHeader);

                            // See if device responds correctly
                            App.set_mode('0');
                            int cartridgeMode = App.request_value(CART_MODE);
                            App.RS232_CloseComport(comPortInt);

                            if (cartridgeMode == GB_CART || cartridgeMode == GBA_CART)
                            {
                                comPortTextBox.Text = Convert.ToString(comPortInt + 1);
                                break;
                            }
                        }
                    }
                }
                // Successful open
                if (App.RS232_OpenComport(comPortInt, baudInt, "8N1") == 0)
                {
                    comConnected = true;
                    commandReceived = 0;

                    headerRead = false;
                    progress = 0;
                    backgroundWorker1.ReportProgress(0);

                    // Update config
                    App.update_config(comPortInt, baudInt, alwaysAddDateTimeToSave, promptForRestoreSaveFile, reReadCartHeader);

                    // Read PCB version
                    gbxcartPcbVersion = App.request_value(READ_PCB_VERSION);

                    // If v1.3, set the voltage
                    if (gbxcartPcbVersion == PCB_1_3 || gbxcartPcbVersion == GBXMAS)
                    {
                        if (cartMode == GB_CART)
                        {
                            App.set_mode(VOLTAGE_5V);
                        }
                        else
                        {
                            App.set_mode(VOLTAGE_3_3V);
                        }
                    }

                    // Read which mode we are in (for v1.2 PCB and below)
                    if (gbxcartPcbVersion <= PCB_1_2)
                    {
                        cartMode = App.read_cartridge_mode();
                        if (cartMode == GB_CART)
                        {
                            gbMode.Dispatcher.Invoke(new Action(() =>
                            {
                                gbMode.IsChecked = true;
                            }));
                        }
                        else if (cartMode == GBA_CART)
                        {
                            gbaMode.Dispatcher.Invoke(new Action(() =>
                            {
                                gbaMode.IsChecked = true;
                            }));
                        }
                    }

                    // If GBxCart Mini, switch to GB mode if in GBA mode
                    if (gbxcartPcbVersion == MINI_PCB_1_0)
                    {
                        gbMode.Dispatcher.Invoke(new Action(() =>
                        {
                            gbMode.IsChecked = true;
                        }));
                        gbaMode.Dispatcher.Invoke(new Action(() =>
                        {
                            gbaMode.IsChecked = false;
                        }));
                    }

                    // Read device firmware
                    int firmwareVersion = App.read_firmware_version();
                    if (firmwareVersion >= 1)
                    {
                        frimwareLabel.Content = "Firmware: R" + firmwareVersion;
                    }

                    // Change icon/buttons
                    statusRect.Fill= new SolidColorBrush(goodColour);
                    connectButton.Visibility = Visibility.Hidden;
                    disconnectButton.Visibility = Visibility.Visible;
                }

            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            cartMode = GB_CART;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            cartMode = GBA_CART;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (comConnected == true)
            {
                int comPortInt = Convert.ToInt32(comPortTextBox.Text);
                comPortInt--;
                headerTextBox.Text = "";
                statuslabel.Content = "";

                App.RS232_CloseComport(comPortInt);
                comConnected = false;
            }

            headerRead = false;
            progress = 0;
            backgroundWorker1.ReportProgress(0);

            statusRect.Fill = new SolidColorBrush(badColour);
            connectButton.Visibility = Visibility.Visible;
            disconnectButton.Visibility = Visibility.Hidden;
            frimwareLabel.Content = "Disconnected";
        }

        private void readInfoButton_Click(object sender, RoutedEventArgs e)
        {
            if (comConnected == true && commandReceived == 0)
            {
                progress = 0;
                backgroundWorker1.ReportProgress(0);

                commandReceived = READHEADER;
                statuslabel.Content = "";
            }
        }

        private void readRomButton_Click(object sender, RoutedEventArgs e)
        {
            if (comConnected == true && headerRead == true && commandReceived == 0)
            {
                progress = 0;
                backgroundWorker1.ReportProgress(0);

                if (cartMode == GB_CART && headerTokens[4] != "Header Checksum: OK")
                {
                    MessageBox.Show("Header Checksum has failed. Can't proceed as corruption could occur.");
                }
                else
                {
                    commandReceived = READROM;
                    statuslabel.Content = "Reading ROM...";
                }
            }
        }

        private void readRamButton_Click(object sender, RoutedEventArgs e)
        {
            if (comConnected == true && commandReceived == 0 && reReadCartHeader == 1)
            {
                progress = 0;
                backgroundWorker1.ReportProgress(0);

                commandReceived = READHEADER;
                statuslabel.Content = "";

                while (commandReceived == READHEADER)
                {
                    System.Threading.Thread.Sleep(200);
                }
                headerRead = true;
            }
            if (comConnected == true && headerRead == true && commandReceived == 0)
            {
                progress = 0;
                backgroundWorker1.ReportProgress(0);

                if (cartMode == GB_CART && headerTokens[4] != "Header Checksum: OK")
                {
                    MessageBox.Show("Header Checksum has failed. Can't proceed as corruption could occur.");
                }
                else
                {
                    if (alwaysAddDateTimeToSave == 1)
                    {
                        saveAsNewFile = true;
                        commandReceived = SAVERAM;
                        statuslabel.Content = "Backing up Save...";
                    }
                    else
                    {
                        saveAsNewFile = false;
                        //if (App.check_if_file_exists() == 1)
                        //{
                        //    SaveOptions SaveOptionsForm = new SaveOptions();
                        //    SaveOptionsForm.saveNameExisting.Text = headerTokens[0];
                        //    SaveOptionsForm.ShowDialog();

                        //    if (commandReceived == SAVERAM)
                        //    {
                        //        statuslabel.Text = "Backing up Save...";
                        //    }
                        //    else
                        //    {
                        //        statuslabel.Text = "Backing up Save cancelled";
                        //    }
                        //}
                        //else
                        //{
                            commandReceived = SAVERAM;
                            statuslabel.Content = "Backing up Save...";
                        //}
                    }
                }
            }
        }

        private void writeRamButton_Click(object sender, RoutedEventArgs e)
        {
            if (comConnected == true && headerRead == true && commandReceived == 0)
            {
                progress = 0;
                backgroundWorker1.ReportProgress(0);

                if (cartMode == GB_CART && headerTokens[4] != "Header Checksum: OK")
                {
                    System.Windows.MessageBox.Show("Header Checksum has failed. Can't proceed as corruption could occur.");
                }
                else
                {
                    bool promptToRestore = false;
                    if (promptForRestoreSaveFile == 1)
                    { // Select a save file
                        OpenFileDialog openFileDialog2 = new OpenFileDialog();
                        if ((bool)openFileDialog2.ShowDialog())
                        {
                            FileInfo fileSelected = new FileInfo(openFileDialog2.FileName);
                            writeSaveFileName = openFileDialog2.FileName;
                            promptToRestore = true;
                        }
                    }
                    else
                    { // Use the default save file name
                        writeSaveFileName = "";
                        if (App.check_if_file_exists() == 1)
                        {
                            promptToRestore = true;
                        }
                        else
                        {
                            statuslabel.Content = "Save file not found.";
                        }
                    }

                    // Prompt to overwrite save
                    //if (promptToRestore == true)
                    //{
                    //    Functions.MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    //    MessageBoxResult dialogResult = MessageBox.Show("This will erase the save game from your Gameboy / Gameboy Advance cart\nPress Yes to continue or No to abort.", "Confirm Write", MessageBoxButtons.YesNo);
                    //    if (dialogResult == MessageBoxResult.Yes)
                    //    {
                            commandReceived = WRITERAM;
                    //        statuslabel.Text = "Restoring Save...";
                    //    }
                    //    else if (dialogResult == DialogResult.No)
                    //    {
                    //        statuslabel.Text = "Restoring Save cancelled";
                    //    }
                    //}
                }
            }
        }
    }
    }
