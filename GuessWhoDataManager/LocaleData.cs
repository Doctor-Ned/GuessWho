using System.Collections.Generic;

using GuessWhoResources;

namespace GuessWhoDataManager {
    internal class LocaleData {
        public Dictionary<string, LocaleChampionData> ChampionData { get; }
        public Dictionary<BasicCategory, string> BasicCategoryNames { get; }

        internal LocaleData(Dictionary<string, LocaleChampionData> championData,
            Dictionary<BasicCategory, string> basicCategoryNames) {
            ChampionData = championData;
            BasicCategoryNames = basicCategoryNames;
        }
    }
}
