using System.Collections.Generic;

using GuessWhoResources;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GuessWho.Model {
    public class GuessWhoConfig {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double CategoryCheckBoxSize { get; set; }
        public double CategoryFontSize { get; set; }
        public double SidePanelWidth { get; set; }
        public double IconSize { get; set; }
        public bool ShowTooltips { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<string> RejectedChampions { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<BasicCategory> RejectedBasicCategories { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<CustomCategory> RejectedCustomCategories { get; set; }

        public static GuessWhoConfig GetDefaultConfig() {
            GuessWhoConfig config = new GuessWhoConfig {
                WindowWidth = 1080.0,
                WindowHeight = 602.0,
                CategoryCheckBoxSize = 1.5,
                CategoryFontSize = 20.0,
                SidePanelWidth = 220.0,
                IconSize = 80.0,
                ShowTooltips = true,
                RejectedChampions = new List<string>(),
                RejectedBasicCategories = new List<BasicCategory>(),
                RejectedCustomCategories = new List<CustomCategory>()
            };
            return config;
        }
    }
}