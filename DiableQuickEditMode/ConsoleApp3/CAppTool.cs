using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    using System;
    using System.Text;
    using System.Diagnostics;

    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Layout;
    using log4net.Repository.Hierarchy;

    using System.Linq;
    using System.Reflection.Emit;
    using static log4net.Appender.RollingFileAppender;

    namespace Common
    {
        public class CAppTool
        {
            private LoggerConfiguration config;
            private ILog rootLogger, debugLogger;
            private PatternLayout patternLayout;
            private static CAppTool instance = null;

            private Level getLogLevel(string level)
            {
                if ("OFF".Equals(level.ToUpper()) || "NONE".Equals(level.ToUpper()))
                    return Level.Off;
                if ("INFO".Equals(level.ToUpper()))
                    return Level.Info;
                if ("ERROR".Equals(level.ToUpper()))
                    return Level.Error;
                if ("WARN".Equals(level.ToUpper()))
                    return Level.Warn;
                if ("DEBUG".Equals(level.ToUpper()) || "ALL".Equals(level.ToUpper()))
                    return Level.Debug;
                return Level.Info;
            }

            // Setup the default appender when startup
            private CAppTool()
            {
                
            }

            public void SetEventSource(string sourceName, int lvl) //for ctcBT.dll in NGBT
            {
                Level lv = null;
                switch (lvl)
                {
                    case 1://Debug
                        lv = Level.Debug;
                        break;
                    case 2://Infomation
                        lv = Level.Info;
                        break;
                    case 3://Warning
                        lv = Level.Warn;
                        break;
                    case 4://Error
                        lv = Level.Error;
                        break;
                    default:
                        lv = Level.Info;
                        break;
                }

                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
                foreach (IAppender appender in hierarchy.GetAppenders())
                {
                    if (appender is EventLogAppender)
                    {
                        ((EventLogAppender)appender).ApplicationName = sourceName;
                        ((EventLogAppender)appender).ActivateOptions();
                        ((EventLogAppender)appender).Threshold = lv;
                    }
                }
            }

            public static CAppTool Instance
            {
                get
                {
                    if (instance == null)
                        instance = new CAppTool();
                    return instance;
                }
            }

            public void Initialize(string ServiceName, LoggerConfiguration config)
            {
                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
                hierarchy.Configured = true;
                hierarchy.Root.Level = getLogLevel(config.LogFileLevel);

                //config pattern
                patternLayout = new PatternLayout();
                if (!string.IsNullOrEmpty(config.PatternLayout))
                {
                    patternLayout.ConversionPattern =
                        "%d{yyyy-MM-dd HH:mm:ss.fff} [%type{1}].[%5M] [%5p] - %m%n";
                    //patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss.fff} [%15type{1}].[%-15M] [%5p] - %m%n";
                    //patternLayout.ConversionPattern = config.PatternLayout;
                }
                else
                {
                    patternLayout.ConversionPattern = "%d{yyyy-MM-dd HH:mm:ss.fff} [%15type{1}].[%-15M] [%5p] - %m%n";
                }
                patternLayout.ActivateOptions();

                //config eventAppender
                EventLogAppender eventlogAppender = new EventLogAppender();
                eventlogAppender.ApplicationName = ServiceName;
                eventlogAppender.Layout = patternLayout;
                eventlogAppender.Threshold = getLogLevel(config.EventLogLevel);
                eventlogAppender.ActivateOptions();
                hierarchy.Root.AddAppender(eventlogAppender);

                // config consoleAppender only load the ConsoleAppender in console mode
                if (Environment.UserInteractive)
                {
                    ConsoleAppender consoleAppender = new ConsoleAppender();
                    consoleAppender.Layout = patternLayout;
                    consoleAppender.Threshold = Level.All;
                    consoleAppender.ActivateOptions();
                    hierarchy.Root.AddAppender(consoleAppender);
                }
                rootLogger = LogManager.GetLogger(typeof(CAppTool));

                //config fileAppender
                if (!"".Equals(config.LogFile))
                {
                    RollingFileAppender fileAppender = new RollingFileAppender();
                    fileAppender.Layout = patternLayout;
                    fileAppender.Threshold = string.IsNullOrEmpty(config.DebugLogFile) ? getLogLevel(config.LogFileLevel) : Level.Info;

                    fileAppender.Encoding = Encoding.UTF8;
                    fileAppender.RollingStyle = RollingMode.Size;
                    fileAppender.MaxFileSize = config.MaximumFileSize * 1024 * 1024;
                    fileAppender.MaxSizeRollBackups = config.MaxSizeRollBackups;
                    fileAppender.File = config.OutputFilePath + "\\" + config.LogFile;
                    fileAppender.DatePattern = config.PatternLayout;
                    fileAppender.ActivateOptions();
                    hierarchy.Root.AddAppender(fileAppender);
                }

                // config debugAppender
                if (!string.IsNullOrEmpty(config.DebugLogFile))
                {
                    CRollingFileAppender debugFileAppender = new CRollingFileAppender(config.DebugLogFile, config);
                    debugFileAppender.Layout = patternLayout;
                    debugFileAppender.Threshold = Level.Debug;
                    debugFileAppender.ActivateOptions();

                    debugLogger = LogManager.GetLogger("DebugLogger");
                    ((Logger)debugLogger.Logger).AddAppender(debugFileAppender);
                }
                this.config = config;
            }

            //public void createDashboardLogger(string logFileName, string logFilePath)
            //{
            //    PatternLayout dashboardLayout = new PatternLayout();
            //    dashboardLayout.ConversionPattern = "%m%n";
            //    dashboardLayout.ActivateOptions();

            //    CRollingFileAppender dashboardFileAppender = new CRollingFileAppender(logFileName, logFilePath, 1, 7, 10, "yyyyMMdd");
            //    dashboardFileAppender.Layout = dashboardLayout;
            //    dashboardFileAppender.Threshold = Level.Info;
            //    dashboardFileAppender.ActivateOptions();

            //    dashboardLogger = LogManager.GetLogger("DashboardLogger");
            //    ((Logger)dashboardLogger.Logger).AddAppender(dashboardFileAppender);
            //}

            public string GetVersion(string strFile = null)
            {
                if (strFile == null)
                    strFile = Process.GetCurrentProcess().MainModule.FileName;
                FileVersionInfo vInfo = FileVersionInfo.GetVersionInfo(strFile);
                char privatePart = (vInfo.FilePrivatePart == 0) ? ' ' : (char)(vInfo.FilePrivatePart + 96);
                string strVersion = "L" + vInfo.FileMajorPart + "." + vInfo.FileMinorPart + "R" + vInfo.FileBuildPart + privatePart;
                return strVersion.TrimEnd();
            }

            public void Error(string message)
            {
                rootLogger.Error(message);
            }

            public void Error(string message, Exception ex)
            {
                rootLogger.Error(message, ex);
            }

            public void Error(Exception ex)
            {
                rootLogger.Error(ex);
            }

            public void Warn(string message)
            {
                rootLogger.Warn(message);
            }

            public void Warn(string message, Exception ex)
            {
                rootLogger.Warn(message, ex);
            }

            public void Warn(Exception ex)
            {
                rootLogger.Warn(ex);
            }

            public void Info(string message)
            {
                rootLogger.Info(message);
            }

            public void Info(string message, Exception ex)
            {
                rootLogger.Info(message, ex);
            }

            public void Info(Exception ex)
            {
                rootLogger.Info(ex);
            }

            public void Debug(string message)
            {
                if (debugLogger != null)
                    debugLogger.Debug(message);
                else
                    rootLogger.Debug(message);
            }

            public void Debug(string message, Exception ex)
            {
                if (debugLogger != null)
                    debugLogger.Debug(message, ex);
                else
                    rootLogger.Debug(message, ex);
            }

            public void Debug(Exception ex)
            {
                if (debugLogger != null)
                    debugLogger.Debug(ex);
                else
                    rootLogger.Debug(ex);
            }

            public void All(string message)
            {
                if (config != null && "ALL".Equals(config.LogFileLevel.ToUpper()))
                    rootLogger.Debug(message);
            }

            public void All(string message, Exception ex)
            {
                if (config != null && "ALL".Equals(config.LogFileLevel.ToUpper()))
                    rootLogger.Debug(message, ex);
            }

            public void All(Exception ex)
            {
                if (config != null && "ALL".Equals(config.LogFileLevel.ToUpper()))
                    rootLogger.Debug(ex);
            }

            public bool isDebugEnabled()
            {
                //return (debugLogger != null) ? true : logger.IsDebugEnabled;

                //the logger.IsDebugEnabled, is alway true by default, there is no other place to set this. so ignore this 
                //juse use "DebugLogFile" in registry to control "isDebugEnabled", if have path in this field, it mean enabled.
                return (debugLogger != null) ? true : false;
            }
        }
    }
}
