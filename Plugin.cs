﻿using MapGeneration;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Core.Zones;
using PluginAPI.Enums;
using UnityEngine;
using FacilityZone = MapGeneration.FacilityZone;

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

            if (player.Zone == FacilityZone.Other)
            { 
                if (isTails)
                {
                    RoomIdentifier room = PluginAPI.Core.Zones.Heavy.Rooms.HczScp049.RoomIdentifier;
                    for(int i = 0; i < HeavyZone.Rooms.Count; i++)
                    {
                        if (HeavyZone.Rooms[i].Identifier == room)
                        {
                            Vector3 vector = HeavyZone.Rooms[i].Position;
                            vector.y += 2;
                            player.Position = vector;
                        }
                    }
                    //player.Position = PocketDimension.Logic.PocketDimension.Position;
                }
                else
                {
                    
                    player.AddItem(ItemType.Coin); //Leo, es macht alles keine Sinn, Add Item funktioniert
                    //player.ClearInventory();  das wäre jetzt so meine Idee das Ganze zu lösen
                }
            }
        }
    }
}