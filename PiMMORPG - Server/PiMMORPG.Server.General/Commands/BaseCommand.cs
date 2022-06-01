using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.Server.General.Commands
{
    using Client;

    public abstract class BaseCommand : ICommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public PiBaseClient Client { get; private set; }
        protected ILogger Logger { get; private set; }

        public BaseCommand()
        {
            Logger = LoggerFactory.GetLogger(this);
        }

        public abstract bool Execute();
        public virtual bool Parse(object caller, params string[] args)
        {
            Client = caller as PiBaseClient;
            return AvailableFor(Client);
        }

        public virtual bool AvailableFor(PiBaseClient client) { return Client != null; }
    }

    public abstract class BaseCommand<TClient> : BaseCommand
        where TClient : PiBaseClient
    {
        public new TClient Client { get; private set; }

        public override bool Parse(object caller, params string[] args)
        {
            Client = caller as TClient;
            return AvailableFor(Client);
        }

        public override bool AvailableFor(PiBaseClient client) { return client is TClient; }
    }
}