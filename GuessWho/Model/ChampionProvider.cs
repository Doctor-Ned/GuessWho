using System;
using System.Collections.Generic;
using System.Linq;

using GuessWhoResources;

using WPFLocalizeExtension.Engine;

namespace GuessWho.Model {
    public static class ChampionProvider {
        private static LeagueChampionConfig LeagueChampionConfig { get; }

        private static Dictionary<string, BasicCategory[]> BasicCategories { get; } = new Dictionary<string, BasicCategory[]>();

        private static Dictionary<string, CustomCategory[]> CustomCategories { get; } = new Dictionary<string, CustomCategory[]>();

        private static Dictionary<BasicCategory, string[]> BasicCategoryChampions { get; } = new Dictionary<BasicCategory, string[]>();

        private static Dictionary<CustomCategory, string[]> CustomCategoryChampions { get; } = new Dictionary<CustomCategory, string[]>();

        public static string[] AllChampionIds { get; }

        static ChampionProvider() {
            LeagueChampionConfig = LeagueChampionConfig.LoadConfigFromResources();
            AllChampionIds = LeagueChampionConfig.ChampionCategoryConfigs.Keys.OrderBy(k => k).ToArray();
            foreach (BasicCategory category in Enum.GetValues(typeof(BasicCategory))) {
                BasicCategoryChampions.Add(category, AllChampionIds.Where(s => LeagueChampionConfig.ChampionCategoryConfigs[s].BasicCategories.Contains(category)).ToArray());
            }
            foreach (CustomCategory category in Enum.GetValues(typeof(CustomCategory))) {
                CustomCategoryChampions.Add(category, AllChampionIds.Where(s => LeagueChampionConfig.ChampionCategoryConfigs[s].CustomCategories.Contains(category)).ToArray());
            }
            foreach (string champId in AllChampionIds) {
                BasicCategories.Add(champId, LeagueChampionConfig.ChampionCategoryConfigs[champId].BasicCategories.OrderBy(c => c).ToArray());
                CustomCategories.Add(champId, LeagueChampionConfig.ChampionCategoryConfigs[champId].CustomCategories.OrderBy(c => c).ToArray());
            }
        }

        public static Uri GetWPFIconUri(string championId) {
            return new Uri(
                $"pack://application:,,,/{typeof(ResourceManager).Assembly.GetName().Name};component/{ResourceManager.ICON_DIRECTORY}/{championId}.{ResourceManager.ICON_EXTENSION}",
                UriKind.Absolute);
        }

        public static BasicCategory[] GetBasicCategories(string champId) {
            return BasicCategories[champId];
        }

        public static CustomCategory[] GetCustomCategories(string champId) {
            return CustomCategories[champId];
        }

        public static string[] GetChampIds(BasicCategory basicCategory) {
            return BasicCategoryChampions[basicCategory];
        }

        public static string[] GetChampIds(CustomCategory customCategory) {
            return CustomCategoryChampions[customCategory];
        }

        public static string GetLocalizedChampionName(string champId) {
            return LocalizeDictionary.Instance.GetLocalizedObject($"{ResourceType.League}_Champion_{champId}_Name", null, LocalizeDictionary.Instance.Culture) as string;
        }

        public static string GetLocalizedChampionTitle(string champId) {
            return LocalizeDictionary.Instance.GetLocalizedObject($"{ResourceType.League}_Champion_{champId}_Title", null, LocalizeDictionary.Instance.Culture) as string;
        }

        public static string GetLocalizedCategoryName(BasicCategory basicCategory) {
            return LocalizeDictionary.Instance.GetLocalizedObject($"{ResourceType.League}_{nameof(BasicCategory)}_{basicCategory}", null, LocalizeDictionary.Instance.Culture) as string;
        }

        public static string GetLocalizedCategoryName(CustomCategory customCategory) {
            return LocalizeDictionary.Instance.GetLocalizedObject($"{ResourceType.CustomCategories}_{customCategory}", null, LocalizeDictionary.Instance.Culture) as string;
        }
    }
}