using System.Collections.Generic;
using System.Linq;

using GuessWhoResources;

namespace GuessWhoDataManager {
    internal class LocaleData {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        internal LocaleData(Locale locale, Dictionary<string, string> localeLabels, Dictionary<CustomCategory, string> customCategoryLabels) {
            Locale = locale;
            LocaleLabels = localeLabels;
            CustomCategoryLabels = customCategoryLabels;
        }

        public void RefillMissingData(Dictionary<Locale, LocaleData> otherData, Locale defaultLocale) {
            Locale[] localesWithSameLanguage = Locale.GetLocalesWithSameLanguage();
            LocaleData defaultLocaleData = otherData[defaultLocale];
            int missingLabels = 0;
            foreach (KeyValuePair<string, string> pair in defaultLocaleData.LocaleLabels.Where(pair => !LocaleLabels.ContainsKey(pair.Key))) {
                bool replaced = false;
                foreach (Locale locale in localesWithSameLanguage) {
                    if (otherData[locale].LocaleLabels.ContainsKey(pair.Key)) {
                        Logger.Info($"Label '{pair.Key}' missing from {Locale} was filled from {locale}!");
                        LocaleLabels.Add(pair.Key, otherData[locale].LocaleLabels[pair.Key]);
                        replaced = true;
                        break;
                    }
                }

                if (!replaced) {
                    ++missingLabels;
                    LocaleLabels.Add(pair.Key, pair.Value);
                }
            }
            if (missingLabels != 0) {
                Logger.Warn($"Locale {Locale} is incomplete: filled {missingLabels} missing labels from default locale {defaultLocale}!");
                missingLabels = 0;
            }

            foreach (KeyValuePair<CustomCategory, string> pair in defaultLocaleData.CustomCategoryLabels.Where(pair => !CustomCategoryLabels.ContainsKey(pair.Key))) {
                bool replaced = false;
                foreach (Locale locale in localesWithSameLanguage) {
                    if (otherData[locale].CustomCategoryLabels.ContainsKey(pair.Key)) {
                        Logger.Info($"CustomCategory '{pair.Key}' missing from {Locale} was filled from {locale}!");
                        CustomCategoryLabels.Add(pair.Key, otherData[locale].CustomCategoryLabels[pair.Key]);
                        replaced = true;
                        break;
                    }
                }

                if (!replaced) {
                    ++missingLabels;
                    CustomCategoryLabels.Add(pair.Key, pair.Value);
                }
            }
            if (missingLabels != 0) {
                Logger.Warn($"Locale {Locale} is incomplete: filled {missingLabels} missing custom categories from default locale {defaultLocale}!");
            }
        }

        public Locale Locale { get; }
        public Dictionary<string, string> LocaleLabels { get; }
        public Dictionary<CustomCategory, string> CustomCategoryLabels { get; }
    }
}
