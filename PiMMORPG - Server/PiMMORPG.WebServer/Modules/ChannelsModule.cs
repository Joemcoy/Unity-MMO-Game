//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Nancy;
//using Nancy.ModelBinding;

//using tFramework.Factories;

//using PiMMORPG.Models;
//namespace PiMMORPG.WebServer.Modules
//{
//    using Models;
//    using Server.General;
//    using Server.General.Drivers;
//    using Server.General.Interfaces;

//    public class ChannelsModule : SecureModule
//    {
//        public ChannelsModule() : base("/channels")
//        {
//            Get["/"] = ListChannels;
//            Get["/register"] = _ => View["register"];
//            Post["/register/"] = HandleRegister;

//            Get["/start/{id}"] = StartChannel;
//            Get["/stop/{id}"] = StopChannel;
//            Get["/delete/{id}"] = DeleteChannel;
//        }

//        object ListChannels(dynamic p)
//        {
//            var Model = new ChannelsModel();
//            using (var ctx = new ChannelDriver())
//            {
//                Model.Channels = ctx.GetModels();
//            }

//            return View["default", Model];
//        }

//        object HandleRegister(dynamic p)
//        {
//            var channel = this.Bind<Channel>();
//            var model = new SuccessModel();

//            if (string.IsNullOrWhiteSpace(channel.Name) || channel.Name.Length < 5)
//                model.Message = "O nome do canal precisa ter no mínimo 5 caractéres.";
//            else if (channel.Port <= 0)
//                model.Message = "O número da porta precisa ser maior que 0.";
//            else if (channel.MaximumConnections < 0)
//                model.Message = "O máximo de conexções precisa ser maior ou igual a 0.";
//            else
//            {
//                using (var ctx = new ChannelDriver())
//                {
//                    if (ctx.HasModel(ctx.CreateBuilder().Where(c => c.Name).Equal(channel.Name)))
//                        model.Message = "O nome do canal já está sendo utilizado!";
//                    if (ctx.HasModel(ctx.CreateBuilder().Where(c => c.Port).Equal(channel.Port)))
//                        model.Message = "A porta do canal já está sendo utilizada!";
//                    else
//                    {
//                        model.Success = true;
//                        model.Message = "Registrado com sucesso!";

//                        ctx.AddModel(channel);
//                    }
//                }
//            }

//            return View["register", model];
//        }

//        object StartChannel(dynamic p)
//        {
//            var id = p.id;
//            var channel = GetChannel(id);

//            if (channel != null)
//                ComponentFactory.Enable(channel);
//            return Response.AsRedirect("/channels");
//        }

//        object StopChannel(dynamic p)
//        {
//            var id = p.id;
//            var channel = GetChannel(id);

//            if (channel != null)
//                ComponentFactory.Disable(channel);
//            return Response.AsRedirect("/channels");
//        }

//        IGameServer GetChannel(int port)
//        {
//            var server = ServerControl.GetServer(port);

//            if (server == null)
//                using (var ctx = new ChannelDriver())
//                {
//                    var model = ctx.GetModel(ctx.CreateBuilder().Where(c => c.ID).Equal(port));
//                    if (ServerControl.RegisterServer(model))
//                        server = ServerControl.GetServer(port);
//                }

//            return server;
//        }

//        object DeleteChannel(dynamic p)
//        {
//            var id = p.id;
//            var model = new ChannelsModel();

//            using (var ctx = new ChannelDriver())
//            {
//                var channel = ctx.GetModel(ctx.CreateBuilder().Where(c => c.ID).Equal(id));
//                if (channel != null)
//                {
//                    model.Success = true;
//                    model.Message = "Canal removido com sucesso!";

//                    ctx.RemoveModel(channel);
//                }
//                else
//                {
//                    model.Success = false;
//                    model.Message = $"Canal de id {id} não encontrado!";
//                }

//                model.Channels = ctx.GetModels();
//            }

//            return View["default", model];
//        }
//    }
//}