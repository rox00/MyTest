using ConsoleApp3.Common;
using log4net.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

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
            Test testclass = new Test();
            Task t = new Task(() =>
            {
                Console.WriteLine("任务开始工作……");
                Thread.Sleep(5000);  //模拟工作过程
            });
            t.Start();
            t.ContinueWith(task =>
            {
                Console.WriteLine("任务完成，完成时候的状态为：");
                Console.WriteLine("IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2}",
                                  task.IsCanceled, task.IsCompleted, task.IsFaulted);

            });
            /***********************************************************************************/
            var tasks = new List<Task>();
            ConcurrentDictionary<int,string> retDictionary = new ConcurrentDictionary<int,string>();    
            // Define a delegate that prints and returns the system tick count
            Action<object> action = (object obj) =>
            {
                string retStr = "";
                int i = (int)obj;
                try
                {
                    // Make each thread sleep a different time in order to return a different tick count
                    Thread.Sleep(i * 100);

                    // The tasks that receive an argument between 2 and 5 throw exceptions
                    if (2 <= i && i <= 5)
                    {
                        throw new InvalidOperationException("SIMULATED EXCEPTION");
                    }

                    int tickCount = Environment.TickCount;
                    Console.WriteLine("Task={0}, i={1}, TickCount={2}, Thread={3}", Task.CurrentId, i, tickCount, Thread.CurrentThread.ManagedThreadId);
                    retStr = "success";
                }
                catch (Exception ex)
                {
                    retStr = ex.Message;
                }

                retDictionary[i] = retStr;
            };

            // Construct started tasks
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(action, i));
            }

            try
            {
                // Wait for all the tasks to finish.
                Task.WaitAll(tasks.ToArray());
                string jsonstr = JsonConvert.SerializeObject(retDictionary);
                var objFromJson = JsonConvert.DeserializeObject<ConcurrentDictionary<int, string>>(jsonstr);

                Dictionary<string,object> dic = new Dictionary<string,object>();
                dic["statuscode"] = ErrorCode.GenericFailure;
                dic["message"] = retDictionary;
                string jsonstr1 = JsonConvert.SerializeObject(dic);

                //{
                //    "statuscode": 1,
                //    "message": {
                //        "0": "success",
                //        "1": "success",
                //        "2": "SIMULATED EXCEPTION",
                //        "3": "SIMULATED EXCEPTION",
                //        "4": "SIMULATED EXCEPTION",
                //        "5": "SIMULATED EXCEPTION",
                //        "6": "success",
                //        "7": "success",
                //        "8": "success",
                //        "9": "success"
                //    }
                //}

                // We should never get to this point
                Console.WriteLine("WaitAll() has not thrown exceptions. THIS WAS NOT EXPECTED.");
            }
            catch (AggregateException e)
            {
                Console.WriteLine("\nThe following exceptions have been thrown by WaitAll(): (THIS WAS EXPECTED)");
                for (int j = 0; j < e.InnerExceptions.Count; j++)
                {
                    Console.WriteLine("\n-------------------------------------------------\n{0}", e.InnerExceptions[j].ToString());
                }
            }





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
