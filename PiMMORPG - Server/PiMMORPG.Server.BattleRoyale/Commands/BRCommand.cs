using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.BattleRoyale.Commands
{
    using General.Commands;
    using Client.BattleRoyale;
    using PiMMORPG.Client;

    public abstract class BRCommand : BaseCommand<PiBRClient>
    {
        public override bool AvailableFor(PiBaseClient client)
        {
            return client is PiBRClient && (client as PiBRClient).RoomID != Guid.Empty;
        }
    }
}
