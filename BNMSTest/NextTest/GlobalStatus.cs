using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextTest
{
    internal class GlobalStatus
    {
        private JObject statusObject = new JObject();
        private ReaderWriterLockSlim LogWriteLock = new ReaderWriterLockSlim();
        private static GlobalStatus intance = null;
        public static GlobalStatus Intance
        {
            get
            {
                if (intance is null)    
                {
                    intance = new GlobalStatus();
                }
                return intance;
            }
        }

        public void addStatus(string hostname, JObject o)
        {
            try
            {
                LogWriteLock.EnterWriteLock();
                statusObject[hostname] = o;
            }
            catch (Exception)
            {
            }
            finally
            {
                LogWriteLock.ExitWriteLock();
            }

        }

        public string getStatus(JArray array)
        {
            string ret = "";
            try
            {
                LogWriteLock.EnterReadLock();
                JObject oTemp = new JObject();
                for (int i = 0; i < array.Count; i++) { 
                    string key = array[i].ToString();
                    if (key.Length < 3) //ex: must be 997 884 cbgw00 agp01
                        break;

                    foreach (var item in statusObject)
                    {
                        string hostname = item.Key;
                        if (hostname.Contains(key))
                        {
                            oTemp[hostname] = statusObject[hostname];
                            break;
                        }
                    }

                }
                ret = oTemp.ToString();
            }
            catch (Exception)
            {
            }
            finally
            {
                LogWriteLock.ExitReadLock();
            }
            return ret;
        }
    }
}
