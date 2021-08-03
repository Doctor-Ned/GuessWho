using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GuessWho.Model {
    public class ChampionCategory {
        public string CategoryName { get; set; }
        public bool IsSelected { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public HashSet<Champion> Champions { get; set; }
    }
}