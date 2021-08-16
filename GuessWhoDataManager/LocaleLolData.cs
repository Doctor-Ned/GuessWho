using System.Collections.Generic;

using GuessWhoResources;

namespace GuessWhoDataManager {
    internal class LocaleLolData {
        public Dictionary<string, LocaleLolChampionData> ChampionData { get; }
        public Dictionary<BasicCategory, string> BasicCategoryNames { get; }

        internal LocaleLolData(Dictionary<string, LocaleLolChampionData> championData,
            Dictionary<BasicCategory, string> basicCategoryNames) {
            ChampionData = championData;
            BasicCategoryNames = basicCategoryNames;
        }
    }
}
