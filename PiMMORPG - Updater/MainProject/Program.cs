using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

using PiMMORPG;

using tFramework.Helper;
using tFramework.Data.Serializer;

namespace MainProject
{
    class Program
    {
        public const string ChecksumURL = "http://launcher.4fungames.com.br/LCF.php?VL=Checksum.xml";
        public const string BaseURL = "http://launcher.4fungames.com.br/LCF.php?DL=";
        public const string ClientPath = "Client";
        public const string Executable = "Release.exe";
        public const string BasicTitle = "PiMMORPG - Alpha Basic Updater";

        static List<FileData> NonUpdatedFiles = new List<FileData>();

        // Returns the human-readable file size for an arbitrary, 64-bit file size 
        // The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
        static string GetBytesReadable(long i)
        {
            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.### ") + suffix;
        }

        static string GenerateBaseDir()
        {
            var baseDir = Environment.CurrentDirectory;
            if (!string.IsNullOrEmpty(ClientPath))
                baseDir = Path.Combine(baseDir, ClientPath);

            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            return baseDir;
        }

        static bool CheckFile(FileData file)
        {
            var path = Path.Combine(GenerateBaseDir(), file.FilePath);
            if (!File.Exists(path))
                return false;
            else if (new FileInfo(path).Length != file.Size)
                return false;
            else if (Path.GetExtension(path) == ".hash")
            {
                if (HashHelper.CalculateFileMD5(path) != file.Hash)
                    return false;
            }
            else
            {
                var hashFile = path + ".hash";
                if (!File.Exists(hashFile))
                    return false;

                var data = CryptHelper.DecryptRijndael(File.ReadAllBytes(hashFile));
                var decoded = data.Select(b => Convert.ToByte(b >= 5 ? b - 5 : b)).ToArray();

                var hash = Encoding.UTF8.GetString(decoded);
                if (hash != file.Hash)
                    return false;
            }

            return true;
        }

        static void Main(string[] args)
        {
            Console.Title = BasicTitle;

            using (var ev = new AutoResetEvent(false))
            {
                using (var pb = new ProgressBar())
                using (var wb = new WebClient())
                {
                    pb.Prefix = "Getting checksum data...";

                    var url = new Uri(ChecksumURL);
                    wb.Proxy = null;
                    wb.DownloadProgressChanged += (s, e) => ReportProgress(pb, "Checksum", e);
                    wb.DownloadDataCompleted += (s, e) => ReportComplete(pb, wb, url, e.Cancelled, e.Error);
                    wb.DownloadDataCompleted += (s, e) => ReportChecksumCompleted(ev, e);
                    wb.DownloadDataAsync(url);
                }
                ev.WaitOne();

                if (NonUpdatedFiles.Count > 0)
                {
                    Console.WriteLine("{0} files hasn't updated! Starting update tasks...", NonUpdatedFiles.Count);

                    var count = 0;
                    foreach (var file in NonUpdatedFiles)
                    {
                        Console.Title = string.Format("{0} - {1}/{2} - {3}%", BasicTitle, ++count, NonUpdatedFiles.Count, (count * 100) / NonUpdatedFiles.Count);

                        var path = Path.Combine(GenerateBaseDir(), file.FilePath);
                        var dir = Path.GetDirectoryName(path);
                        if (!Directory.Exists(dir))
                            Directory.CreateDirectory(dir);

                        using (var pb = new ProgressBar())
                        using (var wb = new WebClient())
                        {
                            pb.Prefix = Path.GetFileName(file.FilePath);
                            var url = new Uri(BaseURL + file.FilePath.Replace('/', '$').Replace('\\', '$').Replace('.', '|'));

                            wb.Proxy = null;
                            wb.DownloadProgressChanged += (s, e) => ReportProgress(pb, Path.GetFileName(file.FilePath), e);
                            wb.DownloadFileCompleted += (s, e) => ReportComplete(pb, wb, url, e.Cancelled, e.Error);
                            wb.DownloadFileCompleted += (s, e) => ReportFileCompleted(ev, e);
                            wb.DownloadFileAsync(url, path);
                            ev.WaitOne();
                        }
                    }
                }

                var client = Path.Combine(GenerateBaseDir(), Executable);
                if (!File.Exists(client))
                    Console.WriteLine("The game executable hasn't been found!");
                else
                {
                    var start = new ProcessStartInfo();
                    start.WorkingDirectory = Path.GetDirectoryName(client);
                    start.FileName = client;
                    start.UseShellExecute = true;

                    Process.Start(start);
                }
            }

            Console.WriteLine("End!");
            Console.WriteLine("Press enter to exit!");
            Console.ReadLine();
        }

        private static void ReportChecksumCompleted(AutoResetEvent ev, DownloadDataCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                using (var stream = new MemoryStream(CryptHelper.DecryptRijndael(e.Result)))
                {
                    stream.Position = 0;

                    FileData[] files = null;
                    if (XMLSerializer.Load(ref files, stream))
                    {
                        Console.WriteLine("Removing deprecated files...");
                        foreach(var toremove in Directory.GetFiles(GenerateBaseDir(), "*.*", SearchOption.AllDirectories))
                        {
                            if(Path.GetDirectoryName(toremove) != "Logs" && Path.GetDirectoryName(toremove) != "Configuration" && !files.Any(f => toremove.EndsWith(f.FilePath)))
                            {
                                Console.WriteLine("Removing {0} (Deprecated!)", Path.GetFileName(toremove));
                                File.Delete(toremove);
                            }
                        }

                        using (var pb = new ProgressBar())
                        {
                            pb.Prefix = "Checking the client..";
                            var count = 0;

                            foreach (var file in files)
                            {
                                if (!CheckFile(file))
                                    NonUpdatedFiles.Add(file);
                                pb.Report(count++ / (double)files.Length);
                            }
                        }
                        ev.Set();

                    }
                    else
                        Console.WriteLine("Failed to read the checksum data!");
                }
            }
            else
                Console.WriteLine("Please close the updater to exit!!!");
        }

        private static void ReportFileCompleted(AutoResetEvent ev, AsyncCompletedEventArgs e)
        {
            if (e.Error != null || e.Cancelled)
                Console.WriteLine("Please close the updater to exit!!!");
            else
                ev.Set();
        }

        private static void ReportComplete(ProgressBar pb, WebClient wc, Uri uri, bool cancelled, Exception error)
        {
            wc.Dispose();
            pb.Dispose();
            Console.Write("{0}  ", pb.Prefix);

            if (cancelled)
                Console.WriteLine("The download has been canceled!");
            else if (error != null)
                Console.WriteLine("Error:{0}!", error);
            else
                Console.WriteLine("Done!");
        }

        private static void ReportProgress(ProgressBar pb, string filename, DownloadProgressChangedEventArgs e)
        {
            pb.Prefix = string.Format("{0} - {1}/{2}", filename, GetBytesReadable(e.BytesReceived), GetBytesReadable(e.TotalBytesToReceive));
            pb.Report((double)e.ProgressPercentage / 100);
        }
    }
}