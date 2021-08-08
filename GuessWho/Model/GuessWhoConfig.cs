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
                    Champion.Corki,
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
            config.Categories.Add(new ChampionCategory {
                CategoryName = "Zabójca",
                IsSelected = true,
                Champions = new HashSet<Champion> {
                    Champion.Akali,
                    Champion.Ekko,
                    Champion.Evelynn,
                    Champion.Fizz,
                    Champion.Kassadin,
                    Champion.Katarina,
                    Champion.Khazix,
                    Champion.Leblanc,
                    Champion.Lillia,
                    Champion.MasterYi,
                    Champion.Nidalee,
                    Champion.Nocturne,
                    Champion.Qiyana,
                    Champion.Rengar,
                    Champion.Shaco,
                    Champion.Talon,
                    Champion.Yone,
                    Champion.Zed
                }
            });
            config.Categories.Add(new ChampionCategory {
                CategoryName = "Wojownik",
                IsSelected = true,
                Champions = new HashSet<Champion> {
                    Champion.Aatrox,
                    Champion.Camille,
                    Champion.Darius,
                    Champion.Diana,
                    Champion.DrMundo,
                    Champion.Fiora,
                    Champion.Gangplank,
                    Champion.Garen,
                    Champion.Gnar,
                    Champion.Gragas,
                    Champion.Gwen,
                    Champion.Hecarim,
                    Champion.Illaoi,
                    Champion.Irelia,
                    Champion.Jax,
                    Champion.Jayce,
                    Champion.Kayle,
                    Champion.Kayn,
                    Champion.Kled,
                    Champion.LeeSin,
                    Champion.Lillia,
                    Champion.Mordekaiser,
                    Champion.Nasus,
                    Champion.Olaf,
                    Champion.Pantheon,
                    Champion.RekSai,
                    Champion.Renekton,
                    Champion.Riven,
                    Champion.Rumble,
                    Champion.Sett,
                    Champion.Shyvana,
                    Champion.Skarner,
                    Champion.Trundle,
                    Champion.Tryndamere,
                    Champion.Udyr,
                    Champion.Urgot,
                    Champion.Vi,
                    Champion.Viego,
                    Champion.Volibear,
                    Champion.Warwick,
                    Champion.MonkeyKing,
                    Champion.XinZhao,
                    Champion.Yasuo,
                    Champion.Yorick
                }
            });
            config.Categories.Add(new ChampionCategory {
                CategoryName = "Mag",
                IsSelected = true,
                Champions = new HashSet<Champion> {
                    Champion.Ahri,
                    Champion.Anivia,
                    Champion.Annie,
                    Champion.AurelionSol,
                    Champion.Azir,
                    Champion.Brand,
                    Champion.Cassiopeia,
                    Champion.Elise,
                    Champion.Fiddlesticks,
                    Champion.Heimerdinger,
                    Champion.Karma,
                    Champion.Karthus,
                    Champion.Kennen,
                    Champion.Lissandra,
                    Champion.Lux,
                    Champion.Malzahar,
                    Champion.Morgana,
                    Champion.Neeko,
                    Champion.Orianna,
                    Champion.Ryze,
                    Champion.Swain,
                    Champion.Sylas,
                    Champion.Syndra,
                    Champion.Taliyah,
                    Champion.TwistedFate,
                    Champion.Veigar,
                    Champion.Velkoz,
                    Champion.Viktor,
                    Champion.Vladimir,
                    Champion.Xerath,
                    Champion.Ziggs,
                    Champion.Zoe,
                    Champion.Zyra
                }
            });
            config.Categories.Add(new ChampionCategory {
                CategoryName = "Strzelec",
                IsSelected = true,
                Champions = new HashSet<Champion> {
                    Champion.Akshan,
                    Champion.Aphelios,
                    Champion.Ashe,
                    Champion.Caitlyn,
                    Champion.Corki,
                    Champion.Draven,
                    Champion.Ezreal,
                    Champion.Graves,
                    Champion.Jhin,
                    Champion.Jinx,
                    Champion.Kaisa,
                    Champion.Kalista,
                    Champion.Kindred,
                    Champion.KogMaw,
                    Champion.Lucian,
                    Champion.MissFortune,
                    Champion.Quinn,
                    Champion.Samira,
                    Champion.Senna,
                    Champion.Sivir,
                    Champion.Teemo,
                    Champion.Tristana,
                    Champion.Twitch,
                    Champion.Varus,
                    Champion.Vayne,
                    Champion.Xayah
                }
            });
            config.Categories.Add(new ChampionCategory {
                CategoryName = "Wsparcie",
                IsSelected = true,
                Champions = new HashSet<Champion> {
                    Champion.Bard,
                    Champion.Braum,
                    Champion.Ivern,
                    Champion.Janna,
                    Champion.Lulu,
                    Champion.Nami,
                    Champion.Pyke,
                    Champion.Rakan,
                    Champion.Seraphine,
                    Champion.Sona,
                    Champion.Soraka,
                    Champion.TahmKench,
                    Champion.Taric,
                    Champion.Thresh,
                    Champion.Yuumi,
                    Champion.Zilean
                }
            });
            config.Categories.Add(new ChampionCategory {
                CategoryName = "Tank",
                IsSelected = true,
                Champions = new HashSet<Champion> {
                    Champion.Alistar,
                    Champion.Amumu,
                    Champion.Blitzcrank,
                    Champion.Chogath,
                    Champion.Galio,
                    Champion.JarvanIV,
                    Champion.Leona,
                    Champion.Malphite,
                    Champion.Maokai,
                    Champion.Nautilus,
                    Champion.Nunu,
                    Champion.Ornn,
                    Champion.Poppy,
                    Champion.Rammus,
                    Champion.Rell,
                    Champion.Sejuani,
                    Champion.Shen,
                    Champion.Singed,
                    Champion.Sion,
                    Champion.Zac
                }
            });
            return config;
        }
    }
}