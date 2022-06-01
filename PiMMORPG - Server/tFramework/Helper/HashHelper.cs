using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.IO;

namespace tFramework.Helper
{
    public static class HashHelper
    {
        public static string CalculateFileMD5(string path)
        {
            using (var stream = IOHelper.WaitForFile(path, FileMode.Open, FileAccess.Read))
            {
                byte[] computed = MD5.Create().ComputeHash(stream);
                return BitConverter.ToString(computed).Replace("-", "");
            }
        }

        public static string CalculateMD5(this string value) { return CalculateMD5(Encoding.UTF8.GetBytes(value)); }
        public static string CalculateMD5(this byte[] data)
        {
            byte[] computed = MD5.Create().ComputeHash(data);
            return BitConverter.ToString(computed).Replace("-", "");
        }
    }
}