using System;
using System.IO;
using System.Linq;
using System.Text;

using Nancy;
using tFramework.Helper;
using tFramework.Factories;

namespace PiMMORPG.WebServer.Modules
{
    using Server.General;
    using tFramework.Data.Manager;

    public class ChecksumModule : NancyModule
    {
        public ChecksumModule() : base("/Checksum")
        {
            Get["/"] = GetChecksum;
        }

        object GetChecksum(dynamic p)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Checksum.xml");
            if (!File.Exists(path))
                return new NotFoundResponse();
            else
            {
                var md5 = HashHelper.CalculateFileMD5(path);
                if(md5 != ServerControl.Configuration.ChecksumMD5)
                {
                    LoggerFactory.GetLogger(this).LogWarning("Checksum file has been changed, updating hash...");

                    ServerControl.Configuration.ChecksumMD5 = md5;
                    ConfigurationManager.Save(ServerControl.Configuration);
                }

                var stream = File.OpenRead(path);
                stream.Seek(0, SeekOrigin.Begin);
                return Response.FromStream(stream, "text/xml");
            }
        }
    }
}