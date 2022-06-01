using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.WebServer.Modules.API
{
    using Server.General.Drivers;

    public class APIAccessModule : SecureAPIModule
    {
        public APIAccessModule() : base("api-access")
        {
            Post["/count"] = DoCount;
        }

        object DoCount(dynamic p)
        {
            using (var ctx = new APIAccessDriver())
                return Response.AsNJson(ctx.Count());
        }
    }
}