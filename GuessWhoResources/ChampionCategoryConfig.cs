using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GuessWhoResources {
    public class ChampionCategoryConfig {
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public HashSet<BasicCategory> BasicCategories { get; set; } = new HashSet<BasicCategory>();

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public HashSet<CustomCategory> CustomCategories { get; set; } = new HashSet<CustomCategory>();
    }
}
