using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class ItemTypeDriver : BaseDriver<ItemType>
    {
        protected override string TableName => "item_type";
    }
}