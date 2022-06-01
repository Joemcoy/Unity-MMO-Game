using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Base.Helpers
{
    public class HashHelper
    {
        readonly static Encoding Default = Encoding.UTF8;

        public static string StringToMD5(string Value)
        {
            byte[] Buffer = Default.GetBytes(Value);
            using (MemoryStream Memory = new MemoryStream(Buffer))
            {
                using (MD5 Hash = MD5.Create())
                {
                    Buffer = Hash.ComputeHash(Memory);
                }
            }
            return BitConverter.ToString(Buffer).Replace("-", "");
        }

        public static string FileToMD5(string FilePath)
        {
            using (FileStream Stream = File.Open(FilePath, FileMode.Open))
            {
                using (MD5 Hash = MD5.Create())
                {
                    return BitConverter.ToString(Hash.ComputeHash(Stream)).Replace("-", "");
                }
            }
        }
    }
}
