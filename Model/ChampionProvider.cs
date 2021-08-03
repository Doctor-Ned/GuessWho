using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;

namespace GuessWho.Model {
    public static class ChampionProvider {
        private const string ICON_EXTENSION = "png", ICON_DIRECTORY = "Champions";

        public static Champion[] AllChampionsAlphabetical { get; private set; }

        private static Dictionary<Champion, string> ChampionNameDictionary { get; } = new Dictionary<Champion, string> {
            { Champion.AurelionSol, "Aurelion Sol" },
            { Champion.Chogath, "Cho'Gath" },
            { Champion.DrMundo, "Dr Mundo" },
            { Champion.JarvanIV, "Jarvan IV" },
            { Champion.Kaisa, "Kai'Sa" },
            { Champion.KogMaw, "Kog'Maw" },
            { Champion.Leblanc, "LeBlanc" },
            { Champion.LeeSin, "Lee Sin" },
            { Champion.MasterYi, "Master Yi" },
            { Champion.MissFortune, "Miss Fortune" },
            { Champion.MonkeyKing, "Wukong" },
            { Champion.RekSai, "Rek'Sai" },
            { Champion.TahmKench, "Tahm Kench" },
            { Champion.TwistedFate, "Twisted Fate" },
            { Champion.Velkoz, "Vel'Koz" },
            { Champion.XinZhao, "Xin Zhao" },
        };

        public static void Validate() {
            List<Champion> champions = Enum.GetValues(typeof(Champion)).Cast<Champion>().ToList();
            foreach (Champion champion in champions) {
                // fill remaining champion names
                if (!ChampionNameDictionary.ContainsKey(champion)) {
                    ChampionNameDictionary.Add(champion, champion.ToString());
                }
                // validate icon files
                StreamResourceInfo _ = Application.GetResourceStream(champion.GetIconUri());
            }

            AllChampionsAlphabetical = champions.OrderByChampionName().ToArray();
        }

        public static string GetName(this Champion champion) {
            return ChampionNameDictionary[champion];
        }

        public static Uri GetIconUri(this Champion champion) {
            return new Uri($"{ICON_DIRECTORY}/{champion}.{ICON_EXTENSION}", UriKind.Relative);
        }

        public static Uri GetWPFIconUri(this Champion champion) {
            return new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/{ICON_DIRECTORY}/{champion}.{ICON_EXTENSION}", UriKind.Absolute);
        }

        public static IEnumerable<Champion> OrderByChampionName(this IEnumerable<Champion> champions) {
            return champions.OrderBy(c => c.GetName());
        }
    }
}
