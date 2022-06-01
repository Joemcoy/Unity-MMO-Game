using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class AccessLevelDriver : BaseDriver<AccessLevel>
    {
        protected override void OnCreateTable()
        {
            base.OnCreateTable();
            AddModel(new AccessLevel
            {
                Name = "General",
                LevelColor = "#0074D4",
                PanelAccess = false
            });
            AddModel(new AccessLevel
            {
                Name = "Administrator",
                LevelColor = "#D45100",
                PanelAccess = true
            });
        }
    }
}