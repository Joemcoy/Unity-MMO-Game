using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using Models;
    using Requests;
    using Drivers;

    public class CreateCharacterResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.CreateCharacter;

        string Name;
        bool IsFemale;
        CharacterStyle Style;

        public override bool Read(IDataPacket packet)
        {
            Name = packet.ReadString();
            IsFemale = packet.ReadBool();
            (Style = new CharacterStyle()).ReadPacket(packet);

            return true;
        }

        public override void Execute()
        {
            using (var ctx = new CharacterDriver())
            {
                using (var mctx = new MapDriver())
                {
                    var packet = new CreateCharacterRequest();
                    if (!ctx.HasModel(ctx.CreateBuilder().Where(m => m.Name).Equal(Name)))
                    {
                        var character = new Character
                        {
                            Name = Name,
                            IsFemale = IsFemale,
                            Style = Style,
                            Account = Client.Account,
                            Map = mctx.GetModel()
                        };

                        ctx.AddModel(character);

                        packet.Result = true;
                        packet.Characters = Client.Characters = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Account).Equal(Client.Account));
                    }
                    Socket.Send(packet);
                }
            }
        }
    }
}