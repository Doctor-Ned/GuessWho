using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using GuessWhoResources;

using Newtonsoft.Json.Linq;

namespace GuessWhoDataManager {
    internal class DataDragon {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private const string DATA_DRAGON_URL = "http://ddragon.leagueoflegends.com/cdn/";
        private const string DATA_DRAGON_VERSIONS_URL = "https://ddragon.leagueoflegends.com/api/versions.json";

        public string Version { get; }

        public string[] ChampionIds { get; }

        public Dictionary<Locale, LocaleLolData> Locales { get; } = new Dictionary<Locale, LocaleLolData>();

        private Dictionary<CustomCategory, string[]> CustomCategoryMappings { get; } = new Dictionary<CustomCategory, string[]>();

        public DataDragon(string version) {
            if (!GetAvailableVersions().Contains(version)) {
                throw new ArgumentException($"Data Dragon version {version} does not exist!");
            }

            Version = version;

            Logger.Info($"Downloading DataDragon data for version '{version}'...");

            using (WebClient wc = new WebClient()) {
                JObject champObj = JObject.Parse(wc.DownloadString($"{DATA_DRAGON_URL}{Version}/data/{Locale.en_US}/champion.json"))["data"].Value<JObject>();
                ChampionIds = champObj.Properties().Select(p => p.Name).OrderBy(s => s).ToArray();
            }

            InitializeCustomCategoryMappings();
            InitializeLocales();
        }

        private void InitializeCustomCategoryMappings() {
            string[] men = {
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
            string[] women = ChampionIds.Where(c => !men.Contains(c)).ToArray();
            CustomCategoryMappings.Add(CustomCategory.Man, men);
            CustomCategoryMappings.Add(CustomCategory.Woman, women);

            // other custom categories can be initialized here

            if (CustomCategoryMappings.Keys.Count != Enum.GetValues(typeof(CustomCategory)).Length) {
                throw new ApplicationException(
                    $"Custom category mapping count does not match available custom categories! ({CustomCategoryMappings.Keys.Count} != {Enum.GetValues(typeof(CustomCategory)).Length})");
            }

            foreach (KeyValuePair<CustomCategory, string[]> pair in CustomCategoryMappings) {
                List<string> wrongChamps = pair.Value.Where(c => !ChampionIds.Contains(c)).ToList();
                if (wrongChamps.Count != 0) {
                    throw new ApplicationException($"Custom category '{pair.Key}' contains the following invalid champion IDs: '{string.Join(",", wrongChamps)}'");
                }
            }

            foreach (CustomCategory category in CustomCategoryMappings.Keys.ToList()) {
                CustomCategoryMappings[category] =
                    CustomCategoryMappings[category].Distinct().OrderBy(s => s).ToArray();
            }
        }

        private void InitializeLocales() {
            using (WebClient wc = new WebClient()) {
                foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                    Logger.Info($"Downloading locale {locale} data...");
                    JObject langObj = JObject.Parse(Encoding.UTF8.GetString(
                        wc.DownloadData($"{DATA_DRAGON_URL}{Version}/data/{locale}/language.json")))["data"].Value<JObject>();

                    Dictionary<BasicCategory, string> basicCategoryNames =
                        Enum.GetValues(typeof(BasicCategory)).Cast<BasicCategory>().
                            ToDictionary(bc => bc, bc => langObj[bc.ToString()].Value<string>());

                    JObject champsObj = JObject.Parse(Encoding.UTF8.GetString(
                        wc.DownloadData($"{DATA_DRAGON_URL}{Version}/data/{locale}/champion.json")))["data"].Value<JObject>();

                    Dictionary<string, LocaleLolChampionData> championData = new Dictionary<string, LocaleLolChampionData>();
                    foreach (JProperty prop in champsObj.Properties()) {
                        JObject champObj = prop.Value.Value<JObject>();
                        string name = champObj["name"].Value<string>();
                        string title = champObj["title"].Value<string>();
                        HashSet<BasicCategory> basicCategories = new HashSet<BasicCategory>();
                        foreach (string tag in champObj["tags"]) {
                            basicCategories.Add((BasicCategory)Enum.Parse(typeof(BasicCategory), tag, true));
                        }
                        HashSet<CustomCategory> customCategories = GetCustomCategories(champObj["id"].Value<string>());
                        championData.Add(prop.Name, new LocaleLolChampionData(name, title, basicCategories, customCategories));
                    }
                    Locales.Add(locale, new LocaleLolData(championData, basicCategoryNames));
                }
            }
        }

        public HashSet<CustomCategory> GetCustomCategories(string champId) {
            return new HashSet<CustomCategory>(CustomCategoryMappings.Where(p => p.Value.Contains(champId)).Select(p => p.Key));
        }

        public void DownloadChampIcon(string champId, FileInfo targetFileInfo) {
            if (!ChampionIds.Contains(champId)) {
                throw new ArgumentException($"Champion ID '{champId}' is invalid!");
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{DATA_DRAGON_URL}{Version}/img/champion/{champId}.png");
            request.Method = "GET";
            using (WebResponse response = request.GetResponse()) {
                using (Stream responseStream = response.GetResponseStream()) {
                    if (responseStream == null) {
                        throw new InvalidDataException("Unable to acquire ResponseStream for given champion icon.");
                    }
                    using (FileStream fileToDownload = new FileStream(targetFileInfo.FullName, FileMode.Create, FileAccess.ReadWrite)) {
                        responseStream.CopyTo(fileToDownload);
                    }
                }
            }
        }

        public static string GetLatestVersion() {
            return GetAvailableVersions()[0];
        }

        public static string[] GetAvailableVersions() {
            using (WebClient wc = new WebClient()) {
                JToken token = JToken.Parse(wc.DownloadString(DATA_DRAGON_VERSIONS_URL));
                return token.Values<string>().ToArray();
            }
        }
    }
}
