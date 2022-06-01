using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
namespace PiMMORPG.Server.RPG
{
    using Client.RPG;
    using General;
    using General.Bases;

    public class PiRPGServer : GameServerBase<PiRPGServer, PiRPGClient>
    {
        public PiRPGServer() : base()
        {
            RegisterResponses(typeof(ServerControl).Assembly);
            RegisterResponses();
            RegisterResponses<PiRPGResponse>();
        }
    }
}