using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Network.Bases;

namespace PiMMORPG.Client
{
    public abstract class PiBaseRequest : BaseRequest<PiBaseClient, TCPAsyncClient>
    {
    }
}