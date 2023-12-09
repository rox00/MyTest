using log4net.Appender;
using log4net.Core;
using System;

namespace ConsoleApp3.Common
{
    internal class CRollingFileAppender : RollingFileAppender
    {
        //    protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        //    {
        //        base.Append(loggingEvent);
        //        try
        //        {
        //            Level level = loggingEvent.Level;
        //            if (level < Level.Error)
        //                return;
        //            SendAlarm send = new SendAlarm(Send);
        //            send.BeginInvoke(loggingEvent.MessageObject + "", level.Name, null, null);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.ToString());
        //        }
        //    }
        //public delegate void SendAlarm(string errorMessage, string loggingLevel);
        //public void Send(string errorMessage, string loggingLevel)
        //{
        //}
        //private string logFileName;
        //private string logFilePath;
        //private int v1;
        //private int v2;
        //private int v3;
        //private string v4;

        public CRollingFileAppender(string logFileName, LoggerConfiguration config)
        {
            this.RollingStyle = RollingMode.Size;
            this.MaxFileSize = config.MaximumFileSize*1024*1024;
            this.MaxSizeRollBackups= config.MaxSizeRollBackups;
            this.File = config.OutputFilePath + "\\" + logFileName;
            this.DatePattern = config.PatternLayout;


            //this.logFileName = logFileName;
            //this.logFilePath = logFilePath;
            //this.v1 = v1;
            //this.v2 = v2;
            //this.v3 = v3;
            //this.v4 = v4;
        }
    }
}