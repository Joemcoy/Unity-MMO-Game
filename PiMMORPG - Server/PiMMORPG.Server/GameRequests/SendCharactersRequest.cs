using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;
    using General.Drivers;

    public class SendCharactersRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.SendCharacters;

        public Character[] Characters { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteInt(Characters.Length);
            foreach(var character in Characters)
            {
                packet.WriteWrapper(character);
                using (var ctx = new CharacterItemDriver())
                {
                    var equips = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Character).Equal(character.ID).And(m => m.Equipped).Equal(true));
                    packet.WriteWrappers(equips);
                }
            }
            return true;
        }
    }
}
