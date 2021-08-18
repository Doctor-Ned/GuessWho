using GuessWhoResources;

using WPFLocalizeExtension.Engine;

namespace GuessWho.Model {
    public static class ResourceProvider {
        public static object GetLocalizedValue(string fullKey) {
            return LocalizeDictionary.Instance.GetLocalizedObject(fullKey, null, LocalizeDictionary.CurrentCulture);
        }

        public static T GetLocalizedValue<T>(string fullKey) {
            return GetLocalizedValue(fullKey) is T t ? t : default;
        }

        public static string GetLocaleString(string key) {
            return GetLocalizedValue<string>($"{ResourceType.Locale}_{key}");
        }

        public static string GetLocalizedChampionName(string champId) {
            return GetLocalizedValue<string>($"{ResourceType.League}_Champion_{champId}_Name");
        }

        public static string GetLocalizedChampionTitle(string champId) {
            return GetLocalizedValue<string>($"{ResourceType.League}_Champion_{champId}_Title");
        }

        public static string GetLocalizedCategoryName(BasicCategory basicCategory) {
            return GetLocalizedValue<string>($"{ResourceType.League}_{nameof(BasicCategory)}_{basicCategory}");
        }

        public static string GetLocalizedCategoryName(CustomCategory customCategory) {
            return GetLocalizedValue<string>($"{ResourceType.CustomCategories}_{customCategory}");
        }
    }
}
