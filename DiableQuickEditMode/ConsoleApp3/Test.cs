using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public enum BCStatusKeyEnum
    {//just using the enum name as stringKey
        appl_state,
        line0,
        line1,
        line2,
        line3,
        LINE4,
    }
    internal class Test
    {
        ConcurrentDictionary<string, string> cur = new ConcurrentDictionary<string, string>();
        public Test()
        {
            //cur[BCStatusKeyEnum.appl_state.ToString()] = "-1";
            //cur[BCStatusKeyEnum.appl_state.ToString()] = "-1";
            //cur[BCStatusKeyEnum.appl_state.ToString()] = "-1";
            //foreach (var item in Enum.GetValues(typeof(BCStatusKeyEnum)))
            //{
            //    cur[item.ToString()] = "-1";
            //}
            ////send
            //string ret = JsonConvert.SerializeObject(cur);

            //Thread backThread = new Thread(() =>
            //{
            //    try
            //    {
            //        while (true)
            //        {
            //            string filepath = "AllBCStatus.json";
            //            if (File.Exists(filepath) == false)
            //            {
            //                var writer = File.CreateText(filepath);
            //                string json = JsonConvert.SerializeObject(cur);
            //                writer.Write(json);
            //                writer.Close();
            //            }
            //            Thread.Sleep(10000);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        //for logs;
            //    }
            //});
            //backThread.IsBackground = true;
            //backThread.Start();



            //Console.ReadLine();
        }



        public void ReadShareMemory()
        {
            // 指定共享内存文件路径及名称
            string sharedMemName = "MySharedMemory";

            try
            {
                using (var mmf = MemoryMappedFile.OpenExisting(sharedMemName))
                {
                    // 获取共享内存视图
                    var accessor = mmf.CreateViewAccessor();

                    // 设置数据起始位置（字节索引）
                    long offset = 0;

                    // 读取共享内存中的数据
                    byte[] buffer = new byte[128]; // 根据实际情况调整大小
                    int bytesRead = accessor.ReadArray<byte>(offset, buffer, 0, buffer.Length);

                    Console.WriteLine("从共享内存中读取到的数据为：");
                    for (int i = 0; i < bytesRead; ++i)
                    {
                        Console.Write((char)(buffer[i]) + " ");
                    }
                    Console.WriteLine("\n共享内存中的数据长度为：" + bytesRead);
                }

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误：" + ex.Message);
                Console.ReadLine();
            }
        }

    }
}
