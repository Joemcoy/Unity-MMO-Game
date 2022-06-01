using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;
using Nancy.Security;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Basic;

using tFramework.Helper;
using PiMMORPG.WebServer.Bases;

namespace PiMMORPG.WebServer
{
    using Models;
    using Server.General.Drivers;

    public class APIDatabase : BaseDatabase<APIDatabase, APIAccessDriver, APIAccess, AuthenticatedAPI>
    {
        public override string SessionFileName { get { return "api-sessions.cfg"; } }
    }
}