using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;
    public class TreeDriver : BaseDriver<Tree>
    {
        public TreeDriver()
        {
            MapDriver<MapDriver>(m => m.Map);
        }

        protected override void OnInsert(Tree model)
        {
            model.Serial = Guid.NewGuid();
        }

        protected override void OnLoad(Tree model)
        {
            if(model.Serial == Guid.Empty)
            {
                model.Serial = Guid.NewGuid();
                UpdateModel(model);
            }
        }
    }
}