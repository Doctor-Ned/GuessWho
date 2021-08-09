using System;
using System.Collections.Generic;

using GuessWhoResources;

using Newtonsoft.Json.Linq;

namespace GuessWhoDataManager {
    internal class LocaleChampionData {
        public HashSet<CustomCategory> CustomCategories { get; }
        public HashSet<BasicCategory> BasicCategories { get; }
        public string Name { get; }
        public string Title { get; }

        internal LocaleChampionData(JObject champ) {
            CustomCategories = Statics.GetCustomCategories(champ["id"].Value<string>());
            BasicCategories = new HashSet<BasicCategory>();
            foreach (string tag in champ["tags"]) {
                BasicCategories.Add((BasicCategory)Enum.Parse(typeof(BasicCategory), tag, true));
            }
            Name = champ["name"].Value<string>();
            Title = champ["title"].Value<string>();
        }
    }
}
