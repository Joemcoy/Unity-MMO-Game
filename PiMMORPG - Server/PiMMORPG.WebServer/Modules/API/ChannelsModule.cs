using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;
using Nancy.ModelBinding;
using tFramework.Factories;
using RazorEngine.Compilation.ImpromptuInterface;

namespace PiMMORPG.WebServer.Modules.API
{
    using Models;
    using Server.General;
    using Server.General.Drivers;
    using Server.General.Interfaces;

    public class ChannelsModule : SecureAPIModule
    {
        public ChannelsModule() : base("channels")
        {
            Post["/"] = GetChannels;
            Post["/count"] = CountChannels;
            Post["/create"] = CreateChannel;
            Post["/open"] = OpenChannel;
            Post["/close"] = CloseChannel;
            Post["/status"] = StatusChannel;
        }

        object GetChannels(dynamic p)
        {
            var servers = ServerControl.Servers.Select(s => s.Channel);
            using (var ctx = new ChannelDriver())
                servers = servers.Concat(ctx.GetModels(ctx.CreateBuilder().Where(m => m.ID).NotIn(servers.Select(m => m.ID).ToArray())));

            return Response.AsNJson(servers);
        }

        object CountChannels(dynamic p)
        {
            using (var ctx = new ChannelDriver())
            {
                var c = ctx.Count();
                return Response.AsNJson(c);
            }
        }

        object CreateChannel(dynamic p)
        {
            var channel = this.Bind<Channel>();
            return Response.AsNJson(ChannelDriver.CreateChannel(channel) as object);
        }

        object OpenChannel(dynamic p)
        {
            var id = (int)Request.Form.id;
            var channel = GetChannel(id);
            var result = -1;

            if (channel != null)
                if (ComponentFactory.IsEnabled(channel))
                    return 1;
                else
                    result = ComponentFactory.Enable(channel) ? 0 : 2;
            return Response.AsNJson(result);
        }

        object CloseChannel(dynamic p)
        {
            var id = (int)Request.Form.id;
            var channel = GetChannel(id);
            var result = -1;

            if (channel != null)
                if (ComponentFactory.IsEnabled(channel))
                    result = ComponentFactory.Disable(channel) ? 0 : 2;
                else
                    result = 1;
            return Response.AsNJson(result);
        }

        object StatusChannel(dynamic p)
        {
            var id = (int)Request.Form.id;
            var channel = GetChannel(id);
            var result = -1;

            if (channel != null)
                result = ComponentFactory.IsEnabled(channel) ? 1 : 0;
            return Response.AsNJson(result);
        }

        IGameServer GetChannel(int port)
        {
            var server = ServerControl.GetServer(port);

            if (server == null)
                using (var ctx = new ChannelDriver())
                {
                    var model = ctx.GetModel(ctx.CreateBuilder().Where(c => c.Port).Equal(port));
                    if (ServerControl.RegisterServer(model))
                        server = ServerControl.GetServer(port);
                }

            return server;
        }
    }
}