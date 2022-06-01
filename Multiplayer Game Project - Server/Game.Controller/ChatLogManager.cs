using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Models;
using Game.Data.Enums;
using Base.Factories;
using Server.Configuration;

namespace Game.Controller
{
    public class ChatLogManager
    {
        public static MessageModel[] GetLastMessages()
        {
            IBaseController Base = ControllerFactory.GetBaseController("chat_log");
            return Base.GetModels().OrderBy(Ma => Ma.ID).Take(GameConfiguration.MaximumMessageCache).Cast<MessageModel>().ToArray();
        }
    }
}
