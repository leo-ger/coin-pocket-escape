using System.Linq;
using System.Security.Policy;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Core.Items;
using PluginAPI.Enums;
using PluginAPI.Core.Items;
using PluginAPI.Core.Zones;
using PluginAPI.Core.Zones.Pocket;
using PluginAPI.Core.Zones.Pocket;
using InventorySystem.Items;

namespace coin_pocket_escape
{
    public class Plugin
    {
        public static Plugin Singleton;

        [PluginConfig] public Config Config;

        public const string Version = "0.0.1";
        
        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint("Coin-Pocket-Escape", Version,
            "Players have a chance of escaping the pocket dimension when flipping a coin", "Various")]
        void LoadPlugin()
        {
            Singleton = this;
            PluginAPI.Events.EventManager.RegisterEvents(this);
        }
        
        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public void OnPlayerCoinFlip(PluginAPI.Core.Player player, bool isTails)
        {
            Log.Info($"&rPlayer &6{player.Nickname}&r (&6{player.UserId}&r) flipped the coin. Flip result: " +
                     $"{(isTails ? "tails" : "heads")}.");

            if (player.Zone == PluginAPI.Core.Zones.UnknownZone); 
            if (isTails)
            {
                player.RemoveItem(player.CurrentItem);
            }
        }
    }
}