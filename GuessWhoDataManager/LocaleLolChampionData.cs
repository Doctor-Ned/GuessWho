using System.Collections.Generic;

using GuessWhoResources;

namespace GuessWhoDataManager {
    internal class LocaleLolChampionData {
        public string Name { get; }
        public string Title { get; }
        public HashSet<BasicCategory> BasicCategories { get; }
        public HashSet<CustomCategory> CustomCategories { get; }

        internal LocaleLolChampionData(string name, string title,
            HashSet<BasicCategory> basicCategories, HashSet<CustomCategory> customCategories) {
            Name = name;
            Title = title;
            BasicCategories = basicCategories;
            CustomCategories = customCategories;
        }
    }
}
