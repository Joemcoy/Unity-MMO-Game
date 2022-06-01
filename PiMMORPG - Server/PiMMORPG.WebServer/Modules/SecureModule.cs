using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Security;

namespace PiMMORPG.WebServer.Modules
{
    public abstract class SecureModule : NancyModule
    {
        public SecureModule(string Path = "") : base(Path)
        {
            Before.AddItemToEndOfPipeline(c =>
            {
                if (Session["UID"] == null)
                    return c.Response.WithStatusCode(HttpStatusCode.Unauthorized);
                else
                    return null;
            });
        }
    }
}