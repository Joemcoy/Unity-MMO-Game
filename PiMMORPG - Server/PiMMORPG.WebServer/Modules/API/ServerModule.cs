using System;
using System.ComponentModel;
using System.Dynamic;
using Nancy;
using Nancy.Responses;
using tFramework.Factories;

namespace PiMMORPG.WebServer.Modules.API
{
    using Server.Auth;

    public class ServerModule : APIModule
    {
        public ServerModule() : base("server")
        {
            Post["/start"] = StartServer;
            Post["/stop"] = StopServer;
        }

        public Response StartServer(dynamic p)
        {
            var msg = "";
            var result = false;
            var server = SingletonFactory.GetSingleton<PiAuthServer>();

            if (ComponentFactory.IsEnabled<PiAuthServer>())
                msg = "Server has already started!";
            else if (ComponentFactory.Enable<PiAuthServer>())
            {
                msg = "Server has been started!";
                result = true;
            }

            return Response.AsJson(new {Success = result, Message = msg});
        }

        dynamic StopServer(dynamic p)
        {
            var msg = "Wait";
            var result = false;

            if (!ComponentFactory.IsEnabled<PiAuthServer>())
                msg = "Server has not been started!";
            else if (!ComponentFactory.Disable<PiAuthServer>())
                msg = "Failed to stop the server!";
            else
            {
                result = true;
                msg = "Server has been stoped!";
            }

            return Response.AsJson(new {Success = result, Message = msg});
        }
    }
}