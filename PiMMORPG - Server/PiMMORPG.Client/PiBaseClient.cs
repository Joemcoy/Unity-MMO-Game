using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Network.Bases;
using tFramework.Network.Enums;
using tFramework.Network.EventArgs;

namespace PiMMORPG.Client
{
    using Models;
    using Interfaces;

    public abstract class PiBaseClient: BaseClient<PiBaseClient, TCPAsyncClient>, IGameClient
    {
#if UNITY_STANDALONE
        protected override void ResponseExecute(ResponseCallEventArgs<TCPAsyncClient> e)
        {
            if (e.Response is PiBaseResponse && (e.Response as PiBaseResponse).ThreadSafe)
            {
                e.CancelCall = true;
                Scripts.Local.SafeInvoker.Create(e.Callback);
            }
        }
        
        protected override void Connected()
        {
            base.Connected();
            Current = this;
        }

        protected override void Disconnected(DisconnectReason reason)
        {
            base.Disconnected(reason);
            Scripts.Local.Application.SocketDisconnected();
        }

        public static PiBaseClient Current { get; private set; }
        public static bool IsLoaded { get; set; }
#else
        public bool SwitchingMap { get; set; } = false;
        public virtual bool CanSpawn(IGameClient other, bool allowSwitching)
        {
            return
                other != null &&
                Character != null &&
                !other.Equals(this) &&
                other.Character != null &&
                other.Character.Map.ID == Character.Map.ID;// && 
                                                           //(!allowSwitching || SwitchingMap && !other.SwitchingMap);
        }
#endif
        public Account Account { get; set; }
        public Character[] Characters { get; set; }
        public Character Character { get; set; }

        public int HitPoints { get; set; }
    }
}