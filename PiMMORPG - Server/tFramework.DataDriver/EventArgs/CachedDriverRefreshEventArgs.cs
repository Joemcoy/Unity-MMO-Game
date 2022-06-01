using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.DataDriver.EventArgs
{
    using Enums;
    using Interfaces;
    using Data.Interfaces;

    public class CachedDriverRefreshEventArgs<TModel> : System.EventArgs
        where TModel : IModel, new()
    {
        public ICachedDriver Driver { get; private set; }
        public DriverOperation Operation { get; private set; }
        public TModel[] Models { get; private set; }

        public CachedDriverRefreshEventArgs(ICachedDriver driver, DriverOperation operation, params TModel[] models)
        {
            this.Driver = driver;
            this.Operation = operation;
            this.Models = models;
        }
    }
}
