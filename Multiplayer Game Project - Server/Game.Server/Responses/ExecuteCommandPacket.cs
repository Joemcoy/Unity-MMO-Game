using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Factories;

using Game.Data;
using Game.Client;

using Network.Data;
using Network.Data.Interfaces;
using Game.Server.Writers;
using System.IO;

namespace Game.Server.Responses
{
    public class ExecuteCommandPacket : GCResponse
    {
        public override uint ID { get { return PacketID.SendCommand; } }

        string CommandLine;
        public override bool Read(ISocketPacket Packet)
        {
            CommandLine = Packet.ReadString();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            CommandFactory<GCommand> Factory = SingletonFactory.GetInstance<CommandFactory<GCommand>>();

            var FilePath = Path.Combine(Environment.CurrentDirectory, "Logs", "Command Logs", Client.CurrentCharacter.Name + ".txt");
            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            using (var Stream = File.Open(FilePath, FileMode.OpenOrCreate))
            using (var Writer = new StreamWriter(Stream))
                Writer.WriteLine(string.Format("[{0}]: {1}", DateTime.Now, CommandLine));

            if (!Factory.ExecuteCommand(CommandLine, Factory.CreateParameter("Client", Client)))
            {
                CommandExecuteFailWriter Packet = new CommandExecuteFailWriter();
                Client.Socket.Send(Packet);
            }
        }
    }
}