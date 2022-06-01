using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Nancy;
namespace PiMMORPG.WebServer.Modules.API
{
    using Server.General.Drivers;
    public class CharactersModule : SecureAPIModule
    {
        public CharactersModule() : base("characters")
        {
            Post["/count"] = DoCount;
        }

        object DoCount(dynamic p)
        {
            using (var ctx = new CharacterDriver())
                return Response.AsNJson(ctx.Count());
        }
    }
}