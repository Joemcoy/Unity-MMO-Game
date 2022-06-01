using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Models;
using Game.Server.Manager;
using Data.Client;

namespace Game.Server.Commands
{
    public class BanCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "ban";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if ((Client == null || Client.Account.Access == Data.Enums.AccessLevel.Administrator) && Arguments.Length > 2)
            {
                int Type;
                if (int.TryParse(Arguments[0], out Type))
                {
                    var Name = string.Join(" ", Arguments.Skip(1).ToArray());

                    if (Name != Client.CurrentCharacter.Name)
                    {
                        var Data = SingletonFactory.GetInstance<DataClient>();
                        Data.SendBanRequest(Client, Type, Name);

                        return true;
                    }
                }
            }
            return false;
        }
    }
}