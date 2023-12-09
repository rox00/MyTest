using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class CRegistryConfig
    {
        RegistryKey baseKey;

        public const string BNMS_REGISTRY_ROOT = @"SOFTWARE\HKJC\CBN\BNMS\ConsoleApp3";

        private object GetRegistryValue(string strKeyPath, string strKeyName)
        {
            RegistryKey keyPath = baseKey.OpenSubKey(strKeyPath);
            return keyPath.GetValue(strKeyName);
        }

        private object GetUserRegistryValue(string strKeyPath, string strKeyName)
        {
            RegistryKey userRegistryKey = null;
            try
            {
                userRegistryKey = Registry.CurrentUser.OpenSubKey(strKeyPath);
                return userRegistryKey.GetValue(strKeyName);
            }
            finally
            {
                if (userRegistryKey != null)
                    userRegistryKey.Close();
            }
        }

        private void SetUserRegistryValue(string strKeyPath, string strKeyName, object value, RegistryValueKind valueKind)
        {
            RegistryKey userRegistryKey = null;
            try
            {
                userRegistryKey = Registry.CurrentUser.CreateSubKey(strKeyPath);
                userRegistryKey.SetValue(strKeyName, value, valueKind);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (userRegistryKey != null)
                    userRegistryKey.Close();
            }
        }

        public CRegistryConfig()
        {
            // initiate registry 
            //RegistryView registryView = (Environment.Is64BitOperatingSystem) ? RegistryView.Registry64 : RegistryView.Registry32;
            baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            //RollingStyle = 0;
        }

        public void CloseRegistryKey()
        {
            baseKey.Close();
        }

        public void WriteUserRegistryValue(string strKeyPath, string strKeyName, long value)
        {
            SetUserRegistryValue(strKeyPath, strKeyName, value, RegistryValueKind.DWord);
        }

        public void WriteUserRegistryValue(string strKeyPath, string strKeyName, string value)
        {
            SetUserRegistryValue(strKeyPath, strKeyName, value, RegistryValueKind.String);
        }

        public void ReadUserRegistryValue(string strKeyPath, string strKeyName, out string value)
        {
            value = "";
            try
            {
                value = GetUserRegistryValue(strKeyPath, strKeyName).ToString();
            }
            catch (Exception ex)
            {
            }
        }

        public void ReadUserRegistryValue(string strKeyPath, string strKeyName, string defaultValue, out string value)
        {
            try
            {
                value = GetUserRegistryValue(strKeyPath, strKeyName).ToString();
            }
            catch (Exception)
            {
                value = defaultValue;
            }
        }

        public void ReadUserRegistryValue(string strKeyPath, string strKeyName, out long value)
        {
            value = 0;
            try
            {
                value = Convert.ToInt64(GetUserRegistryValue(strKeyPath, strKeyName).ToString());
            }
            catch (Exception ex)
            {
            }
        }

        public void ReadUserRegistryValue(string strKeyPath, string strKeyName, long defaultValue, out long value)
        {
            try
            {
                value = Convert.ToInt64(GetUserRegistryValue(strKeyPath, strKeyName).ToString());
            }
            catch (Exception)
            {
                value = defaultValue;
            }
        }

        public void GetRegistryValue(string strKeyPath, string strKeyName, out string[] value)
        {
            value = null;
            try
            {
                value = (string[])GetRegistryValue(strKeyPath, strKeyName);
            }
            catch (Exception ex)
            {
            }
        }

        public void GetRegistryValue(string strKeyPath, string strKeyName, out string value)
        {
            value = null;
            try
            {
                value = GetRegistryValue(strKeyPath, strKeyName).ToString();
            }
            catch (Exception ex)
            {
            }
        }

        public void GetRegistryValue(string strKeyPath, string strKeyName, string defaultValue, out string value)
        {
            try
            {
                value = GetRegistryValue(strKeyPath, strKeyName).ToString();
            }
            catch (Exception)
            {
                value = defaultValue;
            }
        }

        public void GetRegistryValue(string strKeyPath, string strKeyName, out long value)
        {
            value = 0;
            try
            {
                value = Convert.ToInt64(GetRegistryValue(strKeyPath, strKeyName).ToString());
            }
            catch (Exception ex)
            {
            }
        }

        public void GetRegistryValue(string strKeyPath, string strKeyName, long defaultValue, out long value)
        {
            try
            {
                value = Convert.ToInt64(GetRegistryValue(strKeyPath, strKeyName).ToString());
            }
            catch (Exception)
            {
                value = defaultValue;
            }
        }

        public void GetRegistryValue(string strKeyPath, string strKeyName, out int value)
        {
            value= 0;
            try
            {
                value = Convert.ToInt32(GetRegistryValue(strKeyPath, strKeyName).ToString());
            }
            catch (Exception ex)
            {
            }
        }

        public void GetRegistryValue(string strKeyPath, string strKeyName, int defaultValue, out int value)
        {
            try
            {
                value = Convert.ToInt32(GetRegistryValue(strKeyPath, strKeyName).ToString());
            }
            catch (Exception)
            {
                value = defaultValue;
            }
        }

        //public int RollingStyle { set; get; }
        //public int MaximumFileSize { set; get; }
        //public int MaxSizeRollBackups { set; get; }

        //public string LogFile { set; get; }
        //public string DebugLogFile { set; get; }
        //public string OutputFilePath { set; get; }

        //public string PatternLayout { set; get; }

        //public string LogFileLevel { set; get; }
        //public string EventLogLevel { set; get; }
    }

    public class LoggerConfiguration
    {
        public int RollingStyle, MaximumFileSize, MaxSizeRollBackups;
        public string LogFile, DebugLogFile, OutputFilePath, LogFileLevel, EventLogLevel, PatternLayout;

        public LoggerConfiguration(CRegistryConfig config)
        {
            string LoggerPath = CRegistryConfig.BNMS_REGISTRY_ROOT + @"\Logger";
            config.GetRegistryValue(LoggerPath, "LogFile", "", out LogFile);
            config.GetRegistryValue(LoggerPath, "DebugLogFile", "", out DebugLogFile);
            config.GetRegistryValue(LoggerPath, "OutputFilePath", @".\LOG\", out OutputFilePath);
            config.GetRegistryValue(LoggerPath, "LogFileLevel", "INFO", out LogFileLevel);
            config.GetRegistryValue(LoggerPath, "EventLogLevel", "INFO", out EventLogLevel);
            config.GetRegistryValue(LoggerPath, "PatternLayout", "", out PatternLayout);

            config.GetRegistryValue(LoggerPath, "RollingStyle", 0, out RollingStyle);
            config.GetRegistryValue(LoggerPath, "MaximumFileSize", 100, out MaximumFileSize);
            config.GetRegistryValue(LoggerPath, "MaxSizeRollBackups", 100, out MaxSizeRollBackups);
        }

        public void saveConfiguration(CRegistryConfig config)
        {
            //config.LogFile = LogFile;
            //config.DebugLogFile = DebugLogFile;
            //config.OutputFilePath = OutputFilePath;
            //config.PatternLayout = PatternLayout;

            //config.LogFileLevel = LogFileLevel;
            //config.EventLogLevel = EventLogLevel;

            //config.RollingStyle = RollingStyle;
            //config.MaximumFileSize = MaximumFileSize;
            //config.MaxSizeRollBackups = MaxSizeRollBackups;
        }
    }
}
