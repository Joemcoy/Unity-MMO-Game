//using System.Net;
//using Nancy;
//using Newtonsoft.Json;
//using PiMMORPG.Server;
//using tFramework.Factories;

//namespace PiMMORPG.WebServer.Modules
//{
//    using Models;
//    using Server.Auth;

//    public class ServerModule : SecureModule
//    {
//        public ServerModule() : base("/server")
//        {
//            Get["/"] = p => HandleIndex(string.Empty, p);
//            Get["/start"] = HandleStart;
//            Get["/stop"] = HandleStop;
//        }
        
//        dynamic HandleIndex(string msg, dynamic p)
//        {
//            var model = new ServerModel();
//            model.Status = ComponentFactory.IsEnabled<PiAuthServer>();
//            model.Message = msg;

//            return View["default", model];
//        }

//        dynamic HandleStart(dynamic p)
//        {
//            var client = new WebClient();
//            var String = client.DownloadString(WebServer.Configuration.Url + "api/server/start");

//            var model = JsonConvert.DeserializeObject<ServerModel>(String);
//            return HandleIndex(model.Message, p);
//        }

//        dynamic HandleStop(dynamic p)
//        {
//            var client = new WebClient();
//            var String = client.DownloadString(WebServer.Configuration.Url + "api/server/stop");

//            var model = JsonConvert.DeserializeObject<ServerModel>(String);
//            return HandleIndex(model.Message, p);
//        }
//    }
//}