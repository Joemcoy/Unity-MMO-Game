using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Factories;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class ChannelDriver : BaseDriver<Channel>
    {
        protected override void PreConnection()
        {
            base.PreConnection();
            Ignore(m => m.Connections);
        }

        protected override void OnCreateTable()
        {
            AddModel(new Channel { Name = "Beta", Port = 1369, IsPVP = true, MaximumConnections = 5 });
        }

        public static ushort CreateChannel(Channel model)
        {
            if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 3)
                return 1;
            else if (model.Port <= 0 || model.Port > 65535)
                return 2;
            else if (model.MaximumConnections <= 0 || model.MaximumConnections > 64 * 1000)
                return 3;
            else
            {
                using (var ctx = new ChannelDriver())
                {
                    if (ctx.HasModel(ctx.CreateBuilder().Where(m => m.Name).Equal(model.Name)))
                        return 4;
                    else if (ctx.HasModel(ctx.CreateBuilder().Where(m => m.Port).Equal(model.Port)))
                        return 5;
                    else
                    {
                        ctx.AddModel(model);
                        return 0;
                    }
                }
            }
        }
    }
}
