using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class ItemDriver : BaseDriver<Item>
    {
        protected override string TableName => "items";

        public ItemDriver()
        {
            MapDriver<ItemTypeDriver>(m => m.Type);
        }
    }
}