using Devdog.InventoryPro;

namespace Scripts.Local.Inventory
{
    public class NetworkInventoryPlayer : InventoryPlayer
    {
        public override void Init()
        {
            if(!isInitialized)
                base.Init();
        }
    }
}