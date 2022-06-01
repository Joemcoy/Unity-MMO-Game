using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Security.Cryptography;

namespace Base.Helpers
{
    public class RijndaelHelper
    {
        static byte BufferTransform(byte ChunkFlag, byte B)
        {
            return ChunkFlag >= B ? Convert.ToByte(ChunkFlag - B) : B;
        }

        public static byte[] Encrypt(byte[] Data, byte ChunkFlag = 0xCA)
        {
            byte[] Key = new byte[32], IV = new byte[16];

            byte[] Out = null;
            using (RijndaelManaged Rijndael = new RijndaelManaged())
            {
                Rijndael.GenerateKey();
                Rijndael.GenerateIV();

                Key = Rijndael.Key;
                IV = Rijndael.IV;

                using (MemoryStream Memory = new MemoryStream())
                {
                    using (CryptoStream Stream = new CryptoStream(Memory, Rijndael.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        Stream.Write(Data, 0, Data.Length);
                        Stream.FlushFinalBlock();
                    }
                    Out = Memory.ToArray();
                }
            }

            return Key.Concat(Out).Concat(IV).Select(B => BufferTransform(ChunkFlag, B)).Reverse().ToArray();
        }

        public static byte[] Decrypt(byte[] Data, byte ChunkFlag = 0xCA)
        {
            Data = Data.Reverse().Select(B => BufferTransform(ChunkFlag, B)).ToArray();
            byte[] Key = Data.Take(32).ToArray();
            byte[] Buffer = Data.Skip(32).Take(Data.Length - 48).ToArray();
            byte[] IV = Data.Skip(Data.Length - 16).ToArray();

            byte[] Out = null;
            using (RijndaelManaged Rijndael = new RijndaelManaged())
            {
                using (MemoryStream Memory = new MemoryStream())
                {
                    using (CryptoStream Stream = new CryptoStream(Memory, Rijndael.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        Stream.Write(Buffer, 0, Buffer.Length);
                        Stream.Flush();
                    }
                    Out = Memory.ToArray();
                }
            }
            return Out;
        }
    }
}
