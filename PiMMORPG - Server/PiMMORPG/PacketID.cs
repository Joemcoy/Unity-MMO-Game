using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG
{
    public struct PacketID
    {
        //Basic Packets
        public const ushort Login = 0x0001;
        public const ushort Chat = 0x0002;

        //Local Character management
        public const ushort SendCharacter = 0x1003;
        public const ushort CreateCharacter = 0x1004;
        public const ushort SendCharacters = 0x1005;
        public const ushort SelectCharacter = 0x1006;

        //Remote Character Management
        public const ushort SpawnCharacter = 0x2000;
        public const ushort RemoveCharacter = 0x2001;
        public const ushort RequestMapData = 0x2002;
        public const ushort SyncCharacter = 0x2003;
        public const ushort ToggleRunning = 0x2004;
        public const ushort MoveCharacter = 0x2005;
        public const ushort DoAttack = 0x2006;
        public const ushort CharacterDied = 0x2007;

        //Visible Content
        //public const ushort SendTrees = 0x3000;
        //public const ushort SendPlayers = 0x3001;
        //public const ushort SendDrops = 0x3002;
        public const ushort UpdateTime = 0x3000;
        public const ushort ElevateWater = 0x3001;
        public const ushort UpdateRoom = 0x3002;

        //Inventory
        public const ushort SetEquipState = 0x4000;
        public const ushort SetItemSlot = 0x4001;
        public const ushort SetItemQuantity = 0x4002;
        public const ushort MergeItems = 0x4003;
        public const ushort UnstackItems = 0x4004;
        public const ushort RemoveItem = 0x4005;
        public const ushort SetHotbarSlot = 0x4006;
        public const ushort DropItem = 0x4007;
        public const ushort RemoveDrop = 0x4008;
        public const ushort GiveItem = 0x4009;
    }
}
