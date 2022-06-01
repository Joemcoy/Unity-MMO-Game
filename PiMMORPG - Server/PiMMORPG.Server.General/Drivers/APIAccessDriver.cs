using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class APIAccessDriver : BaseDriver<APIAccess>
    {
        protected override string TableName => "api_access";

        protected override void OnCreateTable()
        {
            base.OnCreateTable();
            AddModel(new APIAccess
            {
                Serial = Guid.NewGuid(),
                CanRegister = true,
                CanListAccounts = true,
                CanListChannels = true,
                CanListAccessLevels = true,
                CanListCharacters = true,
                CanListDrops = true,
                CanListTrees = true,
                CanListMaps = true,
                CanListMapSpawns = true,
                CanListItemTypes = true,
                CanListAPIAccess = true
            });
        }
    }
}