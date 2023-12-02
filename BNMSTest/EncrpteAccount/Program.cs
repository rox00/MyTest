using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EncrpteAccount
{

    internal class Program
    {

        static void Main(string[] args)
        {
            //RSACryptoServiceProvider oRSA = new RSACryptoServiceProvider();
            //string privatekey = oRSA.ToXmlString(true);//私钥 需要保存下来
            //string publickey = oRSA.ToXmlString(false);//公钥 需要保存下来

            Dictionary<string, string> sourceDic = new Dictionary<string, string>()
            {
                {"beadmin","0000abc!"},
            };
            JObject o = new JObject();
            for (int i = 0; i < sourceDic.Count; i++)
            {
                string key = "12345678123456781234567812345678"; //must be 32 bytes
                byte[] encrypted = EncryptAes(sourceDic.ElementAt(i).Value, key);
                string encryStr = Convert.ToBase64String(encrypted);
                o.Add(sourceDic.ElementAt(i).Key, encryStr);

                //byte[] decrypte = Convert.FromBase64String(encryStr);
                //string password = DecryptAes(decrypte, key);
            }
            using (StreamWriter stream = new StreamWriter("BNMSAccount.json"))
            {
                stream.WriteLine(o.ToString());
                stream.Close();
            }

        }

        static byte[] EncryptAes(string text, string key)
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

        // 使用 AES 对称加密算法解密数据
        static string DecryptAes(byte[] encrypted, string key)
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
            public static string RSAEncrypt(string publickey, string content)
        {
            //publickey = @"HKJC";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);

            return Convert.ToBase64String(cipherbytes);
        }
        public static string RSADecrypt(string privatekey, string content)
        {
            //privatekey = @"HKJC";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }
    }
}
