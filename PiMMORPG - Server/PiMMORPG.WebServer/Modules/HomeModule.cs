using System.IO;
using System.Net;
using Nancy;
using Newtonsoft.Json;
using Nancy.Security;

namespace PiMMORPG.WebServer.Modules
{
    using Models;
    using Server.General.Drivers;

    public class HomeModule : NancyModule
    {
        public HomeModule() : base("/")
        {
            Get["/"] = HandleIndex;
        }

        dynamic HandleIndex(dynamic p)
        {
            /*var model = new IndexModel();

            model.Logs = WebServer.Logs.ToArray();
            using (var ctx = new AccountDriver())
                model.AccountCount = ctx.Count();

            using(var ctx = new ChannelDriver())
            model.ChannelCount = ctx.Count();

            return View["index"];*/

            if (Session["UID"] == null)
                return View["login"];
            else
                return View["master"];
        }
    }
}