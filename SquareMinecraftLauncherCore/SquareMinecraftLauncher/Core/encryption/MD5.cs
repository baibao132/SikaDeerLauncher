using System;
using System.IO;
using System.Text;

namespace MD5
{
    internal class MD5
    {
        public string GetMD5HashFromFile(string fileName)
        {
            try
            {
                var file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                var sb = new StringBuilder();

                for (int i = 0; i < retVal.Length; i++)
                    sb.Append(retVal[i].ToString("x2"));
                

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
} 