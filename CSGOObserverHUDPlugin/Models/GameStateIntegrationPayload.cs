using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SDCSGOObserverHUDPlugin.Models
{
    class GameStateIntegrationPayload
    {
        [JsonProperty(PropertyName = "provider")]
        public Provider Provider { get; set; }
        [JsonProperty(PropertyName = "player")]
        public Player Player { get; set; }
        [JsonProperty(PropertyName = "allplayers")]
        public Dictionary<string, AllPlayerPlayer> AllPlayers { get; set; }

        [JsonProperty(PropertyName = "achievements")]
        public List<dynamic> Achievements { get; set; }
    }

    public class Provider
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "appid")]
        public int AppId { get; set; }

        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "steamid")]
        public string SteamId { get; set; }

        [JsonProperty(PropertyName = "timestamp")]
        public int Timestamp { get; set; }
    }
    public class Player
    {
        [JsonProperty(PropertyName = "steamid")]
        public string SteamId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "activity")]
        public string Activity { get; set; }

        // These are only accessible, when a player is being spectated. 
        [JsonProperty(PropertyName = "observer_slot")]
        public dynamic ObserverSlot { get; set; } = null;

        [JsonProperty(PropertyName = "team")]
        public string Team { get; set; }


    }

    public class AllPlayerPlayer : Player
    {
        [JsonProperty(PropertyName = "state")]
        public State State { get; set; }

        [JsonProperty(PropertyName = "match_stats")]
        public MatchStats MatchStats { get; set; }

        [JsonProperty(PropertyName = "weapons")]
        public Dictionary<string, Weapon> Weapons { get; set; }

        [JsonProperty(PropertyName = "position")]
        public string Position { get; set; }

        [JsonProperty(PropertyName = "forward")]
        public string Forward { get; set; }
    }

    public class Weapon { 
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "paintkit")]
        public string PaintKit { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "ammo_clip")]
        public int AmmoClip { get; set; }

        [JsonProperty(PropertyName = "ammo_clip_max")]
        public int AmmoClipMax { get; set; }

        [JsonProperty(PropertyName = "ammo_reserve")]
        public int AmmoReserve { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
    }

    public class MatchStats
    {

        [JsonProperty(PropertyName = "kills")]
        public int Kills { get; set; }

        [JsonProperty(PropertyName = "assists")]
        public int Assists { get; set; }

        [JsonProperty(PropertyName = "deaths")]
        public int Deaths { get; set; }

        [JsonProperty(PropertyName = "mvps")]
        public int MVPs { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }
    }

    public class State
    {
        [JsonProperty(PropertyName = "health")]
        public int Health { get; set; }

        [JsonProperty(PropertyName = "armor")]
        public int Armor { get; set; }

        [JsonProperty(PropertyName = "helmet")]
        public bool Helmet { get; set; }

        [JsonProperty(PropertyName = "flashed")]
        public int Flashed { get; set; }

        [JsonProperty(PropertyName = "burning")]
        public int Burning { get; set; }

        [JsonProperty(PropertyName = "money")]
        public int Money { get; set; }

        [JsonProperty(PropertyName = "round_kills")]
        public int RoundKills { get; set; }

        [JsonProperty(PropertyName = "round_killhs")]
        public int RoundKillHs { get; set; }

        [JsonProperty(PropertyName = "round_totaldmg")]
        public int RoundTotalDmg { get; set; }

        [JsonProperty(PropertyName = "equip_value")]
        public int EquipmentValue { get; set; }

        [JsonProperty(PropertyName = "total_adr")]
        public float TotalADR { get; set; }
    }
    public class Auth
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }


}
