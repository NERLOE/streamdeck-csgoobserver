using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BarRaider.SdTools;
using System.Drawing;
using System.Linq;
using CSGSI;
using CSGSI.Nodes;

namespace CSGOObserverHUDPlugin.Backend
{
    class GSIManager
    {
        public class KeyInfo
        {
            public PlayerSwitch.PluginSettings Settings { get; set; }
            public SDConnection Connection { get; set; }

            public string ImageFile { get; set; }
            public string Title { get; set; }

            public KeyInfo(SDConnection connection, PlayerSwitch.PluginSettings settings)
            {
                this.Settings = settings;
                this.Connection = connection;
                this.ImageFile = "";
                this.Title = "";
            }
        }

        public static Dictionary<string, KeyInfo> keysActive = new Dictionary<string, KeyInfo> { };
        public static GameStateListener gsl;

        public static GameState latestData = null; //GameStateIntegrationPayload

        public static async void KeyWillAppear(SDConnection connection, PlayerSwitch.PluginSettings settings)
        {
            bool isFirst = keysActive.Count == 0;

            await connection.LogSDMessage("Key added");
            keysActive.Add(connection.ContextId, new KeyInfo(connection, settings));

            if (isFirst) {
                gsl = new GameStateListener(3004);
                gsl.NewGameState += new NewGameStateHandler(OnNewGameState);

                if (!gsl.Start())
                {
                     await connection.LogSDMessage("Couldn't start GSI");
                     return;
                }

                await connection.LogSDMessage("Listening for GSI game states");
            }

            UpdateKey(connection.ContextId, null);
        }

        static void OnNewGameState(GameState gs)
        {
            var keys = keysActive.Keys;
            foreach (var x in keys)
            {
                UpdateKey(x, gs);
            }
        }

        public static async void DisposeKey(string contextId)
        {
            KeyInfo keyInfo = GetKey(contextId);
            SDConnection connection = keyInfo.Connection;
            await connection.LogSDMessage("Key disappeared");

            bool isLast = keysActive.Count == 1;
            if (isLast)
            {
                await connection.LogSDMessage("No more shown keys, stopping GSI");
                gsl.NewGameState -= OnNewGameState;
                gsl.Stop();
                gsl = null;
            }

            keysActive.Remove(contextId);
        }

        public static void UpdateKeySettings(SDConnection connection, PlayerSwitch.PluginSettings settings)
        {
            keysActive.Remove(connection.ContextId);
            keysActive.Add(connection.ContextId, new KeyInfo(connection, settings));

            UpdateKey(connection.ContextId, latestData);
        }

        public static KeyInfo GetKey(string contextId)
        {
            if (keysActive.ContainsKey(contextId))
            {
                return keysActive[contextId];
            }

            return null;
        }

        public static async void UpdateKey(string contextId, GameState data) //GameStateIntegrationPayload data)
        {
            KeyInfo keyInfo = GetKey(contextId);
            if (keyInfo == null) return;

            SDConnection connection = keyInfo.Connection;
            PlayerSwitch.PluginSettings settings = keyInfo.Settings;

            latestData = data;
            if (data == null || data.AllPlayers == null || settings.ObserverSlot == -1)
            {
                await SetImage(contextId, "images/nodata.png");

                if (settings.ObserverSlot == -1) { await SetTitle(contextId, $"Slot not\nselected"); return; }
                if (data == null) { await SetTitle(contextId, "No data"); return; }

                await SetTitle(contextId, $"No server\nconnected");
                return;
            }

            PlayerNode player = null;
            foreach (var x in data.AllPlayers)
            {
                if (x.ObserverSlot == settings.ObserverSlot) player = x;
            }

            if (player == null)
            {
                await SetImage(contextId, "images/nodata.png");
                await SetTitle(contextId, "Slot\nempty");
                return;
            }

            var team = Enum.GetName(typeof(PlayerTeam), player.Team).ToLower();

            if (player.State.Health == 0)
            {
                await SetImage(contextId, "images/skull.png");
            } else if (data.Player.ObserverSlot != player.ObserverSlot && player.Weapons.Weapons.Where(x => x.Type == WeaponType.C4 && x.Name == "weapon_c4").Count() > 0) {
                await SetImage(contextId, $"images/{team}_c4.png");
            } else
            {
                var state = data.Player.ObserverSlot != -1 && data.Player.ObserverSlot == player.ObserverSlot ? "selected" : "default";
                await SetImage(contextId, $"images/{team}_{state}.png");
            }

            await SetTitle(contextId, $"{player.Name}\n{player.ObserverSlot}");
            return;
        }

        private static async Task SetTitle(string contextId, string title) {
            KeyInfo keyInfo = GetKey(contextId);
            if (keyInfo == null) return;
            if (keyInfo.Title == title) return;
            SDConnection connection = keyInfo.Connection;

            await connection.SetTitleAsync(title);

            keysActive[contextId].Title = title;
        }

        private static async Task SetImage(string contextId, string fileName)
        {
            KeyInfo keyInfo = GetKey(contextId);
            if (keyInfo == null) return;
            if (keyInfo.ImageFile == fileName) return;

            SDConnection connection = keyInfo.Connection;
            Image img = Image.FromFile(fileName);
            await connection.SetImageAsync(img);
            img.Dispose();
            keysActive[contextId].ImageFile = fileName;
        }
    }
}
