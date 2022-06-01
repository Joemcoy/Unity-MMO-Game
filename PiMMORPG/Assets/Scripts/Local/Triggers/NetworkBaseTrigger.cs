using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devdog.InventoryPro;
using UnityEngine;

using tFramework.Factories;
using PiMMORPG.Client;

namespace Scripts.Local.Triggers
{
    using Inventory;
    public abstract class NetworkTriggerBase : MonoBehaviour
    {
        public bool IsLocal, IsLoaded, Initalized = false;

        [NonSerialized]
        public NetworkInventoryPlayer Player;
        public PiBaseClient Client { get { return PiBaseClient.Current; } }

        public virtual void Init(bool IsLocal)
        {
            Player = GetComponent<NetworkInventoryPlayer>();

            if (Player != null && Player.characterUI != null)
                Player.characterUI.character = Player;
            this.IsLocal = IsLocal;

            Initalized = true;
        }

        public virtual void LoadEvents()
        {
            IsLoaded = true;
        }

        public virtual void UnloadEvents()
        {
            IsLoaded = false;
        }
    }
}