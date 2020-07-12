using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GBxHub
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        const string dllLocation = ".\\bin\\GBxHub.RS232.interop.dll";

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern int RS232_OpenComport(int comport_number, int baudrate, string mode);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern int read_cartridge_mode();

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void RS232_CloseComport(int comport_number);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int check_if_file_exists();

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr read_gb_header(ref Int32 length);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr read_gba_header(ref Int32 length);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void read_rom(int cartMode, ref UInt32 length, ref int cancelOperation);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void read_ram(bool saveAsNewFile, ref UInt32 length, ref int cancelOperation);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void write_ram(string writeSaveFileName, ref UInt32 length, ref int cancelOperation);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void set_mode(char command);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern int read_config(int type);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void update_config(int comport, Int32 baudrate, int alwaysAddDateTimeToSave, int promptForRestoreSaveFile, int reReadCartHeader);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gb_specify_rom_size(int size);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gb_specify_mbc_type(int mbcType);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gba_specify_rom_size(int size);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gb_specify_ram_size(int size);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern void gba_specify_ram_size(int memoryType, int flashType, int size);

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern int read_firmware_version();

        [DllImport(dllLocation, CallingConvention = CallingConvention.Cdecl)]
        public static extern int request_value(char command);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void write_rom(string fileName, int flashCartType, UInt32 fileSize, ref UInt32 length, ref int cancelOperation);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void erase_rom(int cartType, ref UInt32 length, ref int cancelOperation);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void erase_ram(ref UInt32 length, ref int cancelOperation);

        [DllImport(dllLocation, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void update_current_folder(string folderName);

        public const float version = 1.1f;
        public const bool beta = true;
        public const string additionalVersion = "";

        private static App application = new App();

        [STAThread]
        public static void Main()
        {
            application.InitializeComponent();
            application.Run();
        }
        public static void DoEvents()
        {
            var frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(
                    delegate (object f)
                    {
                        ((DispatcherFrame)f).Continue = false;
                        return null;
                    }), frame);
            Dispatcher.PushFrame(frame);
        }
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString());
        }
    }
}
