using System;
using System.Collections.Generic;
using System.Linq;

using GuessWhoResources;

namespace GuessWhoDataManager {
    internal class LocaleData {
        internal LocaleData(Dictionary<string, string> localeLabels, Dictionary<CustomCategory, string> customCategoryLabels) {
            LocaleLabels = localeLabels;
            CustomCategoryLabels = customCategoryLabels;
        }

        public Tuple<int, int> RefillMissingDataFrom(LocaleData localeData) {
            int missingLocaleLabels = 0;
            int missingCustomCategoryLabels = 0;
            foreach (KeyValuePair<string, string> pair in localeData.LocaleLabels.Where(pair => !LocaleLabels.ContainsKey(pair.Key))) {
                ++missingLocaleLabels;
                LocaleLabels.Add(pair.Key, pair.Value);
            }
            foreach (KeyValuePair<CustomCategory, string> pair in localeData.CustomCategoryLabels.Where(pair => !CustomCategoryLabels.ContainsKey(pair.Key))) {
                ++missingCustomCategoryLabels;
                CustomCategoryLabels.Add(pair.Key, pair.Value);
            }
            return new Tuple<int, int>(missingLocaleLabels, missingCustomCategoryLabels);
        }

        public Dictionary<string, string> LocaleLabels { get; set; }
        public Dictionary<CustomCategory, string> CustomCategoryLabels { get; set; }
    }
}
