using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data
{
    public class PacketID
    {
        static uint Last = 0x0;
        static uint Offset = 0xA;

        static uint CreateID()
        {
            return Last += Offset;
        }

        public static readonly uint Ping = CreateID();
        public static readonly uint Login = CreateID();
        public static readonly uint Register = CreateID();
        public static readonly uint SendCommand = CreateID();
        public static readonly uint CommandExecuteFail = CreateID();
        public static readonly uint PrivateMessage = CreateID();

        public static readonly uint CharacterList = CreateID();
        public static readonly uint CreateCharacter = CreateID();
        public static readonly uint DeleteCharacter = CreateID();
        public static readonly uint SelectCharacter = CreateID();
        public static readonly uint SendCharacterItems = CreateID();
        public static readonly uint RequestItems = CreateID();
        public static readonly uint SetEquipState = CreateID();
        public static readonly uint ConsumeItem = CreateID();
        public static readonly uint AddItem = CreateID();
        public static readonly uint SetItemSlot = CreateID();
        public static readonly uint ChangeHotbar = CreateID();
        public static readonly uint SetItemAmount = CreateID();
        public static readonly uint RemoveItem = CreateID();
        public static readonly uint DropItem = CreateID();
        public static readonly uint UnstackItem = CreateID();
        public static readonly uint StackItem = CreateID();
        public static readonly uint RemoveDrop = CreateID();
        public static readonly uint SendDrops = CreateID();
        public static readonly uint MergeItem = CreateID();

        public static readonly uint TimeUpdate = CreateID();
        public static readonly uint PlayerList = CreateID();
        public static readonly uint SpawnPlayer = CreateID();
        public static readonly uint RemovePlayer = CreateID();
        public static readonly uint MovePlayer = CreateID();
        public static readonly uint ToggleCombatMode = CreateID();
        public static readonly uint AlreadyOnline = CreateID();
        public static readonly uint SetWeapon = CreateID();
        public static readonly uint PutItemInWorld = CreateID();
        public static readonly uint ToggleChatting = CreateID();
        public static readonly uint AttackPlayer = CreateID();
        public static readonly uint KillPlayer = CreateID();
        public static readonly uint AttackStart = CreateID();
        public static readonly uint AttackEnd = CreateID();
        public static readonly uint Revive = CreateID();
        public static readonly uint ShieldStart = CreateID();
        public static readonly uint ShieldEnd = CreateID();
        public static readonly uint SetKeyState = CreateID();
        public static readonly uint ClickMove = CreateID();
        public static readonly uint PlayMotion = CreateID();
        public static readonly uint SpawnMob = CreateID();
        public static readonly uint SpawnTree = CreateID();
        public static readonly uint SendAudio = CreateID();
        public static readonly uint CanSendAudio = CreateID();
        public static readonly uint SetPlayerPosition = CreateID();
        public static readonly uint SpawnNPC = CreateID();
        public static readonly uint ChangeMap = CreateID();

        public static readonly uint LauncherFileList = CreateID();

#if !(UNITY_BUILD || (UNITY_STANDALONE || UNITY_EDITOR))
        public static readonly uint AuthClient = CreateID();
        public static readonly uint GateType = CreateID();
        public static readonly uint SendGlobalMessage = CreateID();
        public static readonly uint UpdatePlayerCount = CreateID();
#endif
        public static readonly uint SendGates = CreateID();

        public static readonly uint SendMessage = CreateID();
        public static readonly uint SendMessages = CreateID();
        public static readonly uint SendAccount = CreateID();

#if !(UNITY_BUILD || (UNITY_STANDALONE || UNITY_EDITOR))
        public static readonly uint DataSendAccounts = CreateID();
        public static readonly uint DataSendAccountByUsername = CreateID();
        public static readonly uint DataSendAccountByID = CreateID();
        public static readonly uint DataSendLogin = CreateID();
        public static readonly uint DataSendRegister = CreateID();
        public static readonly uint DataSendCharacters = CreateID();
        public static readonly uint DataCreateCharacter = CreateID();
        public static readonly uint DataDeleteCharacter = CreateID();
        public static readonly uint DataSendMapByID = CreateID();
        public static readonly uint DataUpdateCharacterPosition = CreateID();
        public static readonly uint DataSendMessage = CreateID();
        public static readonly uint DataSendMessages = CreateID();
        public static readonly uint DataSendItensInWorld = CreateID();
        public static readonly uint DataSendItemToWorld = CreateID();
        public static readonly uint DataPutItemInWorld = CreateID();
        public static readonly uint DataSendWorldItems = CreateID();
        public static readonly uint DataLauncherFileList = CreateID();
        public static readonly uint DataSendMaps = CreateID();
        public static readonly uint DataSendCharacterItems = CreateID();
        public static readonly uint DataUpdateCharacterItem = CreateID();
        public static readonly uint DataUpdateAccount = CreateID();
        public static readonly uint DataRemoveItem = CreateID();
        public static readonly uint DataSendDrops = CreateID();
        public static readonly uint DataAddDrop = CreateID();
        public static readonly uint DataRemoveDrop = CreateID();
        public static readonly uint DataAddItem = CreateID();
        public static readonly uint DataSendEquips = CreateID();
        public static readonly uint DataSendMobs = CreateID();
        public static readonly uint DataSendTrees = CreateID();
        public static readonly uint DataBan = CreateID();        
        public static readonly uint DataSendNPCs = CreateID();
#endif
    }
}