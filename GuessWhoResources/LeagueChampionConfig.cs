using System.Collections.Generic;

using Newtonsoft.Json;

namespace GuessWhoResources {
    public class LeagueChampionConfig {
        public Dictionary<string, ChampionCategoryConfig> ChampionCategoryConfigs { get; set; } = new Dictionary<string, ChampionCategoryConfig>();

        public string ToJson() {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static LeagueChampionConfig LoadConfigFromResources() {
            return JsonConvert.DeserializeObject<LeagueChampionConfig>(Resources.LeagueChampionConfig);
        }
    }
}