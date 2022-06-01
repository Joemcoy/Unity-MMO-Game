using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiMMORPG.WebServer.Modules
{
    public class ViewModule : SecureModule
    {
        public ViewModule() : base("/view")
        {
            Get["/{name}"] = RequestView;
        }

        object RequestView(dynamic p)
        {
			string name = p.name.ToString();
            switch (name)
            {
                case "home":
                    return View["template/home.html"];
                case "list-accounts":
                    return View["template/list-accounts.html"];
                case "create-account":
                    return View["template/create-account.html"];
                case "list-channels":
                    return View["template/list-channels.html"];
                case "create-channel":
                    return View["template/create-channel.html"];
                case "list-characters":
                    return View["template/list-characters.html"];
                case "create-character":
                    return View["template/create-character.html"];
                default:
                    return "Internal View Error!";
            }
        }
    }
}