using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class CharacterDriver : BaseDriver<Character>
    {
        protected override string TableName => "character";

        public CharacterDriver()
        {
            MapDriver<AccountDriver>(m => m.Account);
            MapDriver<CharacterStyleDriver>(m => m.Style);
            MapDriver<CharacterPositionDriver>(m => m.Position);
            MapDriver<MapDriver>(m => m.Map);
        }

        protected override void PreConnection()
        {
            base.PreConnection();
            Ignore(m => m.Items);
        }

        protected override void OnInsert(Character model)
        {
            base.OnInsert(model);

            using (var ctx = new MapDriver())
                model.Position = ctx.GetModel().Spawn;

            using (var ctx = new CharacterItemDriver())
            {
                using (var ictx = new ItemDriver())
                {
                    uint slot = 0;
                    foreach(var item in ictx.GetModels(ictx.CreateBuilder().Where(m => m.IsEquip).Equal(true)))
                    {
                        var citem = new CharacterItem
                        {
                            Character = model.ID,

                            Equipped = false,
                            Slot = slot++,
                            Info = item,
                            Quantity = 1
                        };
                        ctx.AddModel(citem);
                    }
                }
            }
        }

        protected override void OnUpdate(Character model)
        {
            base.OnUpdate(model);
            if (model.Items != null)
            {
                using (var ctx = new CharacterItemDriver())
                    foreach (var item in model.Items)
                        ctx.UpdateModel(item);
            }
        }
    }
}