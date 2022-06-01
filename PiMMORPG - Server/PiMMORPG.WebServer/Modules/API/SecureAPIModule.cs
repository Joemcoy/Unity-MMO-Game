using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;
using Nancy.Security;

namespace PiMMORPG.WebServer.Modules.API
{
    public abstract class SecureAPIModule : APIModule
    {
        public SecureAPIModule(string path) : base(path)
        {
             Before.AddItemToEndOfPipeline(c =>
            {
                if (Session["AID"] == null)
                    return c.Response.WithStatusCode(HttpStatusCode.Unauthorized);
                else
                    return null;
            });
        }
    }
}