using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class MapDriver : BaseDriver<Map>
    {
        protected override string TableName => "map";

        public MapDriver()
        {
            MapDriver<MapSpawnDriver>(m => m.Spawn);
        }

        protected override void OnCreateTable()
        {
            base.OnCreateTable();
            using (var ctx = new MapSpawnDriver())
            {
                var query = ctx.CreateBuilder().Where(m => m.ID).Equal(1);
                Position spawn = null;

                if (!ctx.HasModel(query))
                {
                    spawn = new Position
                    {
                        PositionX = 100,
                        PositionZ = 50
                    };
                    ctx.AddModel(spawn);
                }
                spawn = ctx.GetModel(query);

                var map = new Map
                {
                    Name = "Beta Map",
                    Message = "The map for testers!",
                    SceneName = "beta",
                    Spawn = spawn
                };
                AddModel(map);
            }
        }
    }
}