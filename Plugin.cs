using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interactables.Interobjects;
using InventorySystem;
using MapGeneration;
using MapGeneration.Distributors;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Core.Zones;
using PluginAPI.Enums;
using UnityEngine;
using System.Collections.Generic;
using CommandSystem.Commands.RemoteAdmin.Cleanup;
using InventorySystem.Items;
using InventorySystem.Items.Coin;
using InventorySystem.Items.Keycards;
using InventorySystem.Items.Radio;
using PluginAPI.Core.Items;
using RelativePositioning;
using FacilityZone = MapGeneration.FacilityZone;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace coin_pocket_escape
{
    public class Plugin
    {
        public static Plugin Singleton;

        [PluginConfig] public Config Config;

        public const string Version = "1.1.0";
        
        [PluginPriority(LoadPriority.Highest)]
        [PluginEntryPoint("Coin-Pocket-Escape", Version,
            "Players have a chance of escaping the pocket dimension when flipping a coin.", "David-Floy,"+
            " josqu-john, leo-ger")]
        void LoadPlugin()
        {
            Singleton = this;
            PluginAPI.Events.EventManager.RegisterEvents(this);
        }
        
        [PluginEvent(ServerEventType.PlayerCoinFlip)]
        public async void OnPlayerCoinFlip(Player player, bool isTails)
        {
            var spawnp = Object.FindObjectsOfType<LockerChamber>();
            var locklenght = Object.FindObjectsOfType<Locker>().Length;
            for (int i = 0; i < locklenght; i++)
            {
                int j = new Random().Next(locklenght);
                Vector3 lockerposi = spawnp[j].transform.position;
                ItemPickup coin = ItemPickup.Create(ItemType.Coin,lockerposi , Quaternion.identity);
                coin.Spawn();
                
                Log.Info($"&rLocker: &6{Object.FindObjectsOfType<Locker>()[i].transform.position}&r");
            }
            
            var playerInPocket = (player.Zone == FacilityZone.Other);
            Log.Info($"&rPlayer &6{player.Nickname}&r (&6{player.UserId}&r) flipped the coin. Flip result: " +
                     $"{(isTails ? "tails" : "heads")}. Player is {(playerInPocket ? "in" : "not in")} pocket.");
            // If the player is not in the pocket, do nothing
            if (!playerInPocket)
            {
                return;
            }

            var success = isTails;
            
            // If CoinRandom is enabled, switch up the result randomly
            if (Config.CoinRandom)
            {
                int i = new Random().Next(2);
                if (i == 1)
                {
                    success = !isTails;
                }
            }
            
            player.ReceiveHint("The coin will decide your fate!", 2F);
            
            // Wait to let the coin-flip animation play
            await Task.Delay(TimeSpan.FromMilliseconds(Config.WaitingTime));
            
            // If isTails and nuke not detonated, the player gets teleported to heavy zone and the coin gets removed
            // Only if the player still has a coin in his hand
            if (success && !AlphaWarheadController.Detonated && player.CurrentItem.ItemTypeId == ItemType.Coin)
            {
                /*
                 Selecting a room in the HeavyZone to teleport the player.
                 To prevent teleports to weird rooms, that are way off the map, only rooms with names are possible.
                */
                int i = new Random().Next(HeavyZone.Rooms.Count);
                while (HeavyZone.Rooms[i].Identifier.Name == RoomName.Unnamed || 
                       HeavyZone.Rooms[i].Identifier.Name == RoomName.HczTesla ||
                       HeavyZone.Rooms[i].Identifier.Name == RoomName.HczTestroom)
                {
                    i = new Random().Next(HeavyZone.Rooms.Count);
                }
                Vector3 vector = HeavyZone.Rooms[i].Position;
                vector.y += 1;
                
                Log.Info($"&rPlayer gets teleported to Room &6{HeavyZone.Rooms[i].Identifier.Name}&r, " +
                         $"Position: &6{vector.x}&r, &6{vector.y}&r, &6{vector.z}&r.");
                player.Position = vector;
                player.ReceiveHint("You were lucky!", 2F);
                player.ReferenceHub.inventory.ServerRemoveItem(player.CurrentItem.ItemSerial, null);
            }

            // If isTails and nuke detonated, the player gets teleported to surface zone and the coin gets removed
            // Only if the player still has a coin in his hand.
            else if (success && player.CurrentItem.ItemTypeId == ItemType.Coin)
            {
                int i = new Random().Next(SurfaceZone.Rooms.Count);
                while (SurfaceZone.Rooms[i].Identifier.Name == RoomName.Unnamed ||
                       HeavyZone.Rooms[i].Identifier.Name == RoomName.HczTesla ||
                       HeavyZone.Rooms[i].Identifier.Name == RoomName.HczTestroom)
                {
                    i = new Random().Next(SurfaceZone.Rooms.Count);
                }
                Vector3 vector = SurfaceZone.Rooms[i].Position;
                vector.y += 1;
                
                Log.Info($"&rPlayer gets teleported to Room &6{HeavyZone.Rooms[i].Identifier.Name}&r, " +
                         $"Position: &6{vector.x}&r, &6{vector.y}&r, &6{vector.z}&r.");
                player.Position = vector;
                player.ReferenceHub.inventory.ServerRemoveItem(player.CurrentItem.ItemSerial, null);
            }
            
            // If the coin is heads, the coin just gets removed and the player stays in the pocket.
            // Only if the player still has a coin in his hand.
            else if (player.CurrentItem.ItemTypeId == ItemType.Coin)
            {
                player.ReceiveHint("Looks like you didn't have luck.", 1F);
                player.ReferenceHub.inventory.ServerRemoveItem(player.CurrentItem.ItemSerial, null);
            }

            // If the player doesn't have a coin in his hand anymore, the event is cancelled.
            else
            {
                player.ReceiveHint("Coin flip cancelled!", 1F);
            }
        }
    }
}