using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.DataDriver.MySQL;
using tFramework.Data.Interfaces;

namespace PiMMORPG.Server.General.Drivers
{
    public abstract class BaseDriver<TModel> : MySqlDriver<TModel>
        where TModel : class, IModel, new()
    {
        public BaseDriver() : base(ServerControl.Configuration.ConnectionString)
        { }
    }
}