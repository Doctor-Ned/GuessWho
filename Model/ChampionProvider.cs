using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Resources;

namespace GuessWho.Model {
    public static class ChampionProvider {
        private const string ICON_EXTENSION = "png", ICON_DIRECTORY = "Champions";

        private static Dictionary<Champion, string> Champions { get; } = new Dictionary<Champion, string> {
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
            foreach (Champion champion in Enum.GetValues(typeof(Champion))) {
                // fill remaining champion names
                if (!Champions.ContainsKey(champion)) {
                    Champions.Add(champion, champion.ToString());
                }
                // validate icon files
                StreamResourceInfo _ = Application.GetResourceStream(champion.GetIconUri());
            }
        }

        public static string GetName(this Champion champion) {
            return Champions[champion];
        }

        public static Uri GetIconUri(this Champion champion) {
            return new Uri($"{ICON_DIRECTORY}/{champion}.{ICON_EXTENSION}", UriKind.Relative);
        }

        public static Uri GetWPFIconUri(this Champion champion) {
            return new Uri($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/{ICON_DIRECTORY}/{champion}.{ICON_EXTENSION}", UriKind.Absolute);
        }

        public static Dictionary<Champion, string> GetAllChampions() {
            return new Dictionary<Champion, string>(Champions);
        }
    }
}
