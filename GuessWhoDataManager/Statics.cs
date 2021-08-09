using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using GuessWhoResources;

using Newtonsoft.Json.Linq;

namespace GuessWhoDataManager {
    internal class Statics {
        internal static string CSPROJ_NAME = "GuessWhoResources";

        public static Dictionary<CustomCategory, string[]> CustomCategoryMappings { get; private set; }

        public static string[] AllChampionIds { get; private set; }

        internal static void InitializeChampionIds(string url) {
            JObject champObj;
            using (WebClient wc = new WebClient()) {
                champObj = JObject.Parse(wc.DownloadString($"{LocalizeUrl(url, Locale.en_US)}champion.json"))["data"].Value<JObject>();
            }

            AllChampionIds = champObj.Properties().Select(p => p.Name).OrderBy(s => s).ToArray();

            CustomCategoryMappings = new Dictionary<CustomCategory, string[]>();

            string[] men = new[] {
                "Aatrox",
                "Akshan",
                "Alistar",
                "Amumu",
                "Aphelios",
                "AurelionSol",
                "Azir",
                "Bard",
                "Blitzcrank",
                "Brand",
                "Braum",
                "Chogath",
                "Corki",
                "Darius",
                "Draven",
                "DrMundo",
                "Ekko",
                "Ezreal",
                "Fiddlesticks",
                "Fizz",
                "Galio",
                "Gangplank",
                "Garen",
                "Gnar",
                "Gragas",
                "Graves",
                "Hecarim",
                "Heimerdinger",
                "Ivern",
                "JarvanIV",
                "Jax",
                "Jayce",
                "Jhin",
                "Karthus",
                "Kassadin",
                "Kayn",
                "Kennen",
                "Khazix",
                "Kled",
                "KogMaw",
                "LeeSin",
                "Lucian",
                "Malphite",
                "Malzahar",
                "Maokai",
                "MasterYi",
                "MonkeyKing",
                "Mordekaiser",
                "Nasus",
                "Nautilus",
                "Nocturne",
                "Nunu",
                "Olaf",
                "Ornn",
                "Pantheon",
                "Pyke",
                "Rakan",
                "Rammus",
                "Renekton",
                "Rengar",
                "Rumble",
                "Ryze",
                "Sett",
                "Shaco",
                "Shen",
                "Singed",
                "Sion",
                "Skarner",
                "Swain",
                "Sylas",
                "TahmKench",
                "Talon",
                "Taric",
                "Teemo",
                "Thresh",
                "Trundle",
                "Tryndamere",
                "TwistedFate",
                "Twitch",
                "Udyr",
                "Urgot",
                "Varus",
                "Veigar",
                "Velkoz",
                "Viego",
                "Viktor",
                "Vladimir",
                "Volibear",
                "Warwick",
                "Xerath",
                "XinZhao",
                "Yasuo",
                "Yone",
                "Yorick",
                "Zac",
                "Zed",
                "Ziggs",
                "Zilean"
            };
            string[] women = AllChampionIds.Where(c => !men.Contains(c)).ToArray();
            CustomCategoryMappings.Add(CustomCategory.Man, men);
            CustomCategoryMappings.Add(CustomCategory.Woman, women);

            // other custom categories can be initialized here

            if (CustomCategoryMappings.Keys.Count != Enum.GetValues(typeof(CustomCategory)).Length) {
                throw new ApplicationException(
                    $"Custom category mapping count does not match available custom categories! ({CustomCategoryMappings.Keys.Count} != {Enum.GetValues(typeof(CustomCategory)).Length})");
            }

            foreach (KeyValuePair<CustomCategory, string[]> pair in CustomCategoryMappings) {
                List<string> wrongChamps = pair.Value.Where(c => !AllChampionIds.Contains(c)).ToList();
                if (wrongChamps.Count != 0) {
                    throw new ApplicationException($"Custom category '{pair.Key}' contains the following invalid champion IDs: '{string.Join(",", wrongChamps)}'");
                }
            }

            foreach (CustomCategory category in CustomCategoryMappings.Keys.ToList()) {
                CustomCategoryMappings[category] =
                    CustomCategoryMappings[category].Distinct().OrderBy(s => s).ToArray();
            }
        }

        public static HashSet<CustomCategory> GetCustomCategories(string champId) {
            return new HashSet<CustomCategory>(CustomCategoryMappings.Where(p => p.Value.Contains(champId)).Select(p => p.Key));
        }

        public static string GetLatestDataDragonVersion() {
            const string VERSIONS_URL = "https://ddragon.leagueoflegends.com/api/versions.json";
            using (WebClient wc = new WebClient()) {
                string versionsJson = wc.DownloadString(VERSIONS_URL);
                JToken token = JToken.Parse(versionsJson);
                return token[0].Value<string>();
            }
        }

        public static string GetDataDragonUrl(string version) {
            return $"http://ddragon.leagueoflegends.com/cdn/{version}/data/";
        }

        public static string LocalizeUrl(string url, Locale locale) {
            return $"{url}{locale}/";
        }

        public static string GetLatestDataDragonUrl() {
            return GetDataDragonUrl(GetLatestDataDragonVersion());
        }
    }
}
