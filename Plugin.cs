using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
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
        public void OnPlayerCoinFlip(MyPlayer player, bool isTails)
        {
            Log.Info($"&rPlayer &6{player.Nickname}&r (&6{player.UserId}&r) flipped the coin. Flip result: " +
                     $"{(isTails ? "tails" : "heads")}.");

            if (isTails)
            {
                
            }
        }
    }
}