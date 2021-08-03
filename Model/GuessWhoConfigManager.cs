using Newtonsoft.Json;

using System.IO;
using System.Linq;

namespace GuessWho.Model {
    public class GuessWhoConfigManager {
        private string ConfigFile { get; }
        private object ConfigLock { get; } = new object();
        private bool FirstRead { get; set; } = true;

        public GuessWhoConfigManager() : this("GuessWhoConfig.json") { }

        public GuessWhoConfigManager(string configFile) {
            ConfigFile = configFile;
        }

        private GuessWhoConfig Validate(GuessWhoConfig config) {
            if (config.WindowWidth <= 0.0) {
                throw new InvalidDataException($"Window width ({config.WindowWidth}) must be positive!");
            }
            if (config.WindowHeight <= 0.0) {
                throw new InvalidDataException($"Window height ({config.WindowHeight}) must be positive!");
            }

            if (config.CategoryCheckBoxSize <= 0.0) {
                throw new InvalidDataException($"Category check box size ({config.CategoryCheckBoxSize}) must be positive!");
            }
            if (config.CategoryFontSize <= 0.0) {
                throw new InvalidDataException($"Category font size ({config.CategoryFontSize}) must be positive!");
            }
            if (config.SidePanelWidth <= 0.0) {
                throw new InvalidDataException($"Side panel width ({config.SidePanelWidth}) must be positive!");
            }
            if (config.IconSize <= 0.0) {
                throw new InvalidDataException($"Icon size ({config.IconSize}) must be positive!");
            }
            foreach (ChampionCategory category in config.Categories) {
                if (string.IsNullOrWhiteSpace(category.CategoryName)) {
                    throw new InvalidDataException("Category name must not be empty!");
                }

                if (!category.Champions.Any()) {
                    throw new InvalidDataException($"Category '{category.CategoryName}' contains no champions!");
                }
            }
            for (int i = 0; i < config.Categories.Count - 1; ++i) {
                for (int j = i + 1; j < config.Categories.Count; ++j) {
                    if (config.Categories[i].CategoryName == config.Categories[j].CategoryName) {
                        throw new InvalidDataException($"Duplicate category found ('{config.Categories[i].CategoryName}')!");
                    }
                }
            }

            return config;
        }

        public GuessWhoConfig ReadConfig() {
            lock (ConfigLock) {
                if (FirstRead && !File.Exists(ConfigFile)) {
                    FirstRead = false;
                    return Validate(GuessWhoConfig.GetDefaultConfig());
                }

                return Validate(JsonConvert.DeserializeObject<GuessWhoConfig>(File.ReadAllText(ConfigFile)));
            }
        }

        public void WriteConfig(GuessWhoConfig config) {
            lock (ConfigLock) {
                File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(Validate(config), Formatting.Indented));
            }
        }
    }
}
