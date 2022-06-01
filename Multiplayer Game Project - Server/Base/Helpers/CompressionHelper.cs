using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Base.Helpers
{
    public static class CompressionHelper
    {
        public const int GZipBuffer = 1024 * 4;
        public static byte[] GZipCompress(byte[] Buffer)
        {
            byte[] Output = null;

            using (var Memory = new MemoryStream())
            {
                using (var GZip = new GZipStream(Memory, CompressionMode.Compress, true))
                {
                    GZip.Write(Buffer, 0, Buffer.Length);
                }
                Output = Memory.ToArray();
            }

            return Output;
        }

        public static byte[] GZipDecompress(byte[] Buffer)
        {
            using (GZipStream GZip = new GZipStream(new MemoryStream(Buffer), CompressionMode.Decompress))
            {
                byte[] buffer = new byte[GZipBuffer];
                using (MemoryStream Memory = new MemoryStream())
                {
                    int Count = 0;
                    do
                    {
                        Count = GZip.Read(buffer, 0, GZipBuffer);
                        if (Count > 0)
                        {
                            Memory.Write(buffer, 0, Count);
                        }
                    }
                    while (Count > 0);
                    return Memory.ToArray();
                }
            }
        }
    }
}