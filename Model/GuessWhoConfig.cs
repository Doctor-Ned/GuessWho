using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GuessWho.Model {
    public class GuessWhoConfig {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double CategoryCheckBoxSize { get; set; }
        public double CategoryFontSize { get; set; }
        public double SidePanelWidth { get; set; }
        public double IconSize { get; set; }
        public bool ShowTooltips { get; set; }

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public HashSet<Champion> RejectedChampions { get; set; }

        public List<ChampionCategory> Categories { get; set; }

        public static GuessWhoConfig GetDefaultConfig() {
            GuessWhoConfig config = new GuessWhoConfig {
                WindowWidth = 1080.0,
                WindowHeight = 602.0,
                CategoryCheckBoxSize = 1.5,
                CategoryFontSize = 20.0,
                SidePanelWidth = 220.0,
                IconSize = 80.0,
                ShowTooltips = true,
                RejectedChampions = new HashSet<Champion>(),
                Categories = new List<ChampionCategory>()
            };
            ChampionCategory men = new ChampionCategory {
                CategoryName = "Mężczyzna",
                IsSelected = true,
                Champions = new HashSet<Champion> {
                    Champion.Aatrox,
                    Champion.Akshan,
                    Champion.Alistar,
                    Champion.Amumu,
                    Champion.Aphelios,
                    Champion.AurelionSol,
                    Champion.Azir,
                    Champion.Bard,
                    Champion.Blitzcrank,
                    Champion.Brand,
                    Champion.Braum,
                    Champion.Chogath,
                    Champion.Darius,
                    Champion.Draven,
                    Champion.DrMundo,
                    Champion.Ekko,
                    Champion.Ezreal,
                    Champion.Fiddlesticks,
                    Champion.Fizz,
                    Champion.Galio,
                    Champion.Gangplank,
                    Champion.Garen,
                    Champion.Gnar,
                    Champion.Gragas,
                    Champion.Graves,
                    Champion.Hecarim,
                    Champion.Heimerdinger,
                    Champion.Ivern,
                    Champion.JarvanIV,
                    Champion.Jax,
                    Champion.Jayce,
                    Champion.Jhin,
                    Champion.Karthus,
                    Champion.Kassadin,
                    Champion.Kayn,
                    Champion.Kennen,
                    Champion.Khazix,
                    Champion.Kled,
                    Champion.KogMaw,
                    Champion.LeeSin,
                    Champion.Lucian,
                    Champion.Malphite,
                    Champion.Malzahar,
                    Champion.Maokai,
                    Champion.MasterYi,
                    Champion.MonkeyKing,
                    Champion.Mordekaiser,
                    Champion.Nasus,
                    Champion.Nautilus,
                    Champion.Nocturne,
                    Champion.Nunu,
                    Champion.Olaf,
                    Champion.Ornn,
                    Champion.Pantheon,
                    Champion.Pyke,
                    Champion.Rakan,
                    Champion.Rammus,
                    Champion.Renekton,
                    Champion.Rengar,
                    Champion.Rumble,
                    Champion.Ryze,
                    Champion.Sett,
                    Champion.Shaco,
                    Champion.Shen,
                    Champion.Singed,
                    Champion.Sion,
                    Champion.Skarner,
                    Champion.Swain,
                    Champion.Sylas,
                    Champion.TahmKench,
                    Champion.Talon,
                    Champion.Taric,
                    Champion.Teemo,
                    Champion.Thresh,
                    Champion.Trundle,
                    Champion.Tryndamere,
                    Champion.TwistedFate,
                    Champion.Twitch,
                    Champion.Udyr,
                    Champion.Urgot,
                    Champion.Varus,
                    Champion.Veigar,
                    Champion.Velkoz,
                    Champion.Viego,
                    Champion.Viktor,
                    Champion.Vladimir,
                    Champion.Volibear,
                    Champion.Warwick,
                    Champion.Xerath,
                    Champion.XinZhao,
                    Champion.Yasuo,
                    Champion.Yone,
                    Champion.Yorick,
                    Champion.Zac,
                    Champion.Zed,
                    Champion.Ziggs,
                    Champion.Zilean
                }
            };
            config.Categories.Add(men);
            config.Categories.Add(new ChampionCategory {
                CategoryName = "Kobieta",
                IsSelected = true,
                Champions =
                    new HashSet<Champion>(Enum.GetValues(typeof(Champion)).Cast<Champion>()
                        .Where(c => !men.Champions.Contains(c)))
            });
            return config;
        }
    }
}