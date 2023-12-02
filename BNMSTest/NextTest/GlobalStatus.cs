using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NextTest
{
    internal class GlobalStatus
    {
        private string Aeskey = "12345678123456781234567812345678"; //must be 32 bytes
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
        private byte[] EncryptAes(string text, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                byte[] textBytes = Encoding.UTF8.GetBytes(text);
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
                }
            }
        }
        private string DecryptAes(byte[] encrypted, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }

        }
        public bool Login(string username, string password)
        {
            bool bRet = false;
            try
            {
                string filecontent = System.IO.File.ReadAllText("BNMSAccount.json");
                JObject o = JObject.Parse(filecontent);
                username = username.ToLower();
                foreach (var item in o)
                {
                    string nameTemp = item.Key;
                    nameTemp = nameTemp.ToLower();
                    if (nameTemp.Equals(username))
                    {
                        string passTemp = DecryptAes(Convert.FromBase64String((string)item.Value), Aeskey);
                        if (passTemp.Equals(password))
                        {
                            bRet = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return bRet;
        }

        public bool addStatus(string hostname, JObject o)
        {
            bool bRet = false;
            try
            {
                LogWriteLock.EnterWriteLock();
                statusObject[hostname] = o;
                bRet = true;
            }
            catch (Exception)
            {
            }
            finally
            {
                LogWriteLock.ExitWriteLock();
            }
            return bRet;
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
