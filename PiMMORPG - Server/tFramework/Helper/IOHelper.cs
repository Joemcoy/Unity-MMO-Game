using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using tFramework.Factories;
using System.Threading;

namespace tFramework.Helper
{
    public static class IOHelper
    {
        public const byte Maximum = 10;

        public static bool IsLocked(string filename, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None)
        {
            if (!File.Exists(filename) || new FileInfo(filename).Length == 0)
                return false;

            using (var stream = new FileStream(filename, mode, access, share))
            {
                return stream.Length <= 0;
            }
        }

        public static FileStream WaitForFile(string filename, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite, FileShare share = FileShare.None)
        {
            int tries = 0;
            while (IsLocked(filename, mode, access, share))
            {
                if (tries++ > 10)
                    throw new IOException(string.Format("Cannot wait for file {0}! Tried {1}..", filename, tries));
                 Thread.Sleep(1000);
            }
            return File.Open(filename, mode, access, share);
        }
    }
}