using ConsoleApp3.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Program
    {
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int hConsoleHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);

        public static void DisbleQuickEditMode()
        {
            IntPtr hStdin = GetStdHandle(STD_INPUT_HANDLE);
            uint mode;
            GetConsoleMode(hStdin, out mode);
            mode &= ~ENABLE_QUICK_EDIT_MODE;
            SetConsoleMode(hStdin, mode);

        }

        static void Main(string[] args)
        {
            DisbleQuickEditMode();
            string name = typeof(Program).Assembly.GetName().Name;
            CRegistryConfig registryCfg = new CRegistryConfig();
            LoggerConfiguration config = new LoggerConfiguration(registryCfg);
            CAppTool.Instance.Initialize(name, config);

            int index = 0;
            new Thread(() =>
            {
                while (true)
                {
                    CAppTool.Instance.Debug($"ConsoleApp3 Debug {index}");
                    CAppTool.Instance.Info($"ConsoleApp3 Info {index}");
                    CAppTool.Instance.Error($"ConsoleApp3 Error {index}");
                    index++;
                    Thread.Sleep(500);
                }
            }).Start();
            Console.ReadLine();
        }
    }
}
