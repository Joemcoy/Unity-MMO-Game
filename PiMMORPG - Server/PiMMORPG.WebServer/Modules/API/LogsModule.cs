using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.WebServer.Modules.API
{
    public class LogsModule : SecureAPIModule
    {
        public LogsModule() : base("logs")
        {
            Post["/"] = ListLogs;
        }

        object ListLogs(dynamic p)
        {
            return Response.AsNJson(new
            {
                LastUpdate = WebServer.LogUpdated,
                Logs = WebServer.Logs
            });
        }
    }
}