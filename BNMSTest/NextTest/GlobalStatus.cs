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
        /// <summary>
        /// by Eric 20230214
        /// Encrpyt a text by using the 32 bytes key.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// <summary>
        /// by Eric 20230214
        /// Decrypt a text by using the 32 bytes key.
        /// </summary>
        /// <param name="encrypted"></param>
        /// <param name="key"></param>
        /// <returns></returns>
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
        /// <summary>
        /// by Eric 20230214
        /// for user login to BNMS on BNMSConsole side.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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

        /// <summary>
        /// by Eric 20230214
        /// receive Json status from ntbc,cbgw,ngbt(maybe),kiosk(maybe) and save these status to Json Object.
        /// using the hostname as a unique key for one machine.
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="o"></param>
        /// <returns></returns>
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

        /// <summary>
        /// by Eric 20230214
        /// BNMS Console can use subkey(ex: must be 997 884 cbgw00 AGP01) to query one machine
        /// or one branch's status.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
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
