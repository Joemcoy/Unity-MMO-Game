using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Interfaces;
using tFramework.Enums;

namespace tFramework.Network
{
    using Interfaces;
    using Factories;
    using Extensions;

    internal class IOQueue : IUpdater
    {
        class DataG
        {
            public byte Action { get; set; }
            public IDataPacket Packet { get; set; }
        }

        TCPClient _client;
        Queue<DataG> _grams;
        ILogger _logger;

        int IUpdater.Interval { get { return 75; } }
        DelayMode IUpdater.DelayMode { get { return DelayMode.DelayAfter; } }

        public IOQueue(TCPClient client)
        {
            this._client = client;
            _logger = LoggerFactory.GetLogger(this);
        }

        void IThread.Start()
        {
            _grams = new Queue<DataG>();
        }

        void IThread.End()
        {
            _logger.LogWarning("IOQueue of client {0} has been stopped!", _client.EndPoint);
        }

        bool IThread.Run()
        {
            if(_client.Connected)
            {
                if (!_client.IOEnabled) return true;
                try
                {
                    if(_grams.Count > 0)
                    {
                        var d = _grams.Dequeue();
                        switch(d.Action)
                        {
                            case 0:
                                _client.FirePacketReceived(d.Packet);
                                break;
                            case 1:
                                var header = new byte[d.Packet.HeaderLength];
                                d.Packet.CopyHeader(ref header);

                                _client.Stream.Write(header, 0, header.Length);
                                _client.Stream.Write(d.Packet.Buffer, 0, d.Packet.Length);
                                _client.FirePacketSent(d.Packet);
                                break;
                        }
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    _logger.LogFatal(ex);
                    return false;
                }
            }
            return false;
        }

        public void EnqueueReceive(IDataPacket packet)
        {
            _grams.Enqueue(new DataG { Action = 0, Packet = packet });
        }

        public void EnqueueSend(IDataPacket packet)
        {
            _grams.Enqueue(new DataG { Action = 1, Packet = packet });
        }
    }
}
