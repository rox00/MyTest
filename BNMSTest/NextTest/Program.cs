using BNMS;
using Grpc.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextTest
{
    internal class Program
    {
        dynamic server = new BNMSStatusServer();
        static JObject testobject = new JObject();
        private static object _SyncLockObject = new object();
        static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();

        static void Main(string[] args)
        {
            //JArray array= new JArray(); 
            //JObject obj = new JObject();
            //array.Add("123");
            //array.Add("456");
            //testobject["fdasfds"] = array;
            //string str = testobject.ToString();
            //str = testobject.ToString();
            //array = (JArray)testobject["fdasfds"];
            //var obj1 = array[0];
            //var obj2 = array[1];

            //testobject["fdasfds"] = DateTime.Now;
            //var aaa = testobject["fdasfds"];
            //int bbb = (int)testobject["fdasfds"];
            //string ccc = (string)testobject["fdasfds"];
            //aaa = testobject["fdasfds"];
            //string str = DateTime.Now.ToFileTimeUtc() + "";
            //testobject.Add(str, DateTime.Now);
            //var obj = testobject[str];
            ////obj = DateTime.Now;

            //JObject testobject1 = testobject;
            //testobject1[str] = DateTime.Now;


            //var a = DateTime.Now.ToFileTimeUtc();
            //new Thread(() =>
            //{
            //    while (true)
            //    {
            //        lock (_SyncLockObject)
            //        {
            //            testobject.Add(DateTime.Now.ToFileTimeUtc() + "A", DateTime.Now);
            //        }
            //        Thread.Sleep(1);
            //    }
            //}).Start();
            //new Thread(() =>
            //{
            //    while (true)
            //    {
            //        lock (_SyncLockObject)
            //        {
            //            testobject.Add(DateTime.Now.ToFileTimeUtc() + "B", DateTime.Now);
            //        }
            //        Thread.Sleep(1);
            //    }
            //}).Start();
            //Console.ReadLine();


            var server = new Server
            {
                Services = { BNMSStatusSrv.BindService(new BNMSStatusServer()) },
                Ports = { new ServerPort("localhost", 8888, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine($"MyTestGrpc Listen to Port: 555");
            Console.ReadLine();
        }
    }

    //class Program
    //{
    //    static int LogCount = 1000;
    //    static int SumLogCount = 0;
    //    static int WritedCount = 0;
    //    static int FailedCount = 0;

    //    static void Main(string[] args)
    //    {
    //        //往线程池里添加一个任务，迭代写入N个日志
    //        SumLogCount += LogCount;
    //        ThreadPool.QueueUserWorkItem((obj) =>
    //        {
    //            Parallel.For(0, LogCount, e =>
    //            {
    //                WriteLog();
    //            });
    //        });

    //        //在新的线程里，添加N个写入日志的任务到线程池
    //        SumLogCount += LogCount;
    //        var thread1 = new Thread(() =>
    //        {
    //            Parallel.For(0, LogCount, e =>
    //            {
    //                ThreadPool.QueueUserWorkItem((subObj) =>
    //                {
    //                    WriteLog();
    //                });
    //            });
    //        });
    //        thread1.IsBackground = false;
    //        thread1.Start();

    //        //添加N个写入日志的任务到线程池
    //        SumLogCount += LogCount;
    //        Parallel.For(0, LogCount, e =>
    //        {
    //            ThreadPool.QueueUserWorkItem((obj) =>
    //            {
    //                WriteLog();
    //            });
    //        });

    //        //在新的线程里，迭代写入N个日志
    //        SumLogCount += LogCount;
    //        var thread2 = new Thread(() =>
    //        {
    //            Parallel.For(0, LogCount, e =>
    //            {
    //                WriteLog();
    //            });
    //        });
    //        thread2.IsBackground = false;
    //        thread2.Start();

    //        //在当前线程里，迭代写入N个日志
    //        SumLogCount += LogCount;
    //        Parallel.For(0, LogCount, e =>
    //        {
    //            WriteLog();
    //        });

    //        Console.WriteLine("Main Thread Processed.\r\n");
    //        while (true)
    //        {
    //            Console.WriteLine(string.Format("Sum Log Count:{0}.\t\tWrited Count:{1}.\tFailed Count:{2}.", SumLogCount.ToString(), WritedCount.ToString(), FailedCount.ToString()));
    //            Console.ReadLine();
    //        }
    //    }

    //    //读写锁，当资源处于写入模式时，其他线程写入需要等待本次写入结束之后才能继续写入
    //    static ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
    //    static void WriteLog()
    //    {
    //        try
    //        {
    //            //设置读写锁为写入模式独占资源，其他写入请求需要等待本次写入结束之后才能继续写入
    //            //注意：长时间持有读线程锁或写线程锁会使其他线程发生饥饿 (starve)。 为了得到最好的性能，需要考虑重新构造应用程序以将写访问的持续时间减少到最小。
    //            //      从性能方面考虑，请求进入写入模式应该紧跟文件操作之前，在此处进入写入模式仅是为了降低代码复杂度
    //            //      因进入与退出写入模式应在同一个try finally语句块内，所以在请求进入写入模式之前不能触发异常，否则释放次数大于请求次数将会触发异常
    //            LogWriteLock.EnterWriteLock();

    //            var logFilePath = "log.txt";
    //            var now = DateTime.Now;
    //            var logContent = string.Format("Tid: {0}{1} {2}.{3}\r\n", Thread.CurrentThread.ManagedThreadId.ToString().PadRight(4), now.ToLongDateString(), now.ToLongTimeString(), now.Millisecond.ToString());

    //            File.AppendAllText(logFilePath, logContent);
    //            WritedCount++;
    //        }
    //        catch (Exception)
    //        {
    //            FailedCount++;
    //        }
    //        finally
    //        {
    //            //退出写入模式，释放资源占用
    //            //注意：一次请求对应一次释放
    //            //      若释放次数大于请求次数将会触发异常[写入锁定未经保持即被释放]
    //            //      若请求处理完成后未释放将会触发异常[此模式不下允许以递归方式获取写入锁定]
    //            LogWriteLock.ExitWriteLock();
    //        }
    //    }
    //}
}
