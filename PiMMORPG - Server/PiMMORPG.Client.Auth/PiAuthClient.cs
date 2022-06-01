using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Interfaces;
using tFramework.Network;
using tFramework.Network.Bases;

namespace PiMMORPG.Client.Auth
{
    using Models;
    using tFramework.Network.Enums;
    using tFramework.Network.EventArgs;

    public class PiAuthClient : BaseClient<PiAuthClient, TCPAsyncClient>
#if UNITY_STANDALONE
        , ISingleton
#endif
    {
        public Account User { get; set; }

#if UNITY_STANDALONE
        void ISingleton.Created() { RegisterResponses(); }
        void ISingleton.Destroyed() { }

        protected override void Disconnected(DisconnectReason reason)
        {
            base.Disconnected(reason);

            if(reason != DisconnectReason.Normal)
                Scripts.Local.Application.SocketDisconnected();
        }

        protected override void ResponseExecute(ResponseCallEventArgs<TCPAsyncClient> e)
        {
            if (e.Response is PiAuthResponse && (e.Response as PiAuthResponse).ThreadSafe)
            {
                e.CancelCall = true;
                Scripts.Local.SafeInvoker.Create(e.Callback);
            }
        }
#endif
    }
}