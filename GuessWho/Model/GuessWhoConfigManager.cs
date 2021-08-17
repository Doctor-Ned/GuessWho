using System.IO;
using System.Linq;

using Newtonsoft.Json;

namespace GuessWho.Model {
    public class GuessWhoConfigManager {
        public GuessWhoConfigManager() : this("GuessWhoConfig.json") { }

        public GuessWhoConfigManager(string configFile) {
            ConfigFile = configFile;
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

        #region Private members

        private string ConfigFile { get; }
        private object ConfigLock { get; } = new object();
        private bool FirstRead { get; set; } = true;

        private GuessWhoConfig Validate(GuessWhoConfig config) {
            if (config.WindowWidth <= 0.0) {
                throw new InvalidDataException($"Window width ({config.WindowWidth}) must be positive!");
            }

            if (config.WindowHeight <= 0.0) {
                throw new InvalidDataException($"Window height ({config.WindowHeight}) must be positive!");
            }

            if (config.CategoryCheckBoxSize <= 0.0) {
                throw new InvalidDataException(
                    $"Category check box size ({config.CategoryCheckBoxSize}) must be positive!");
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

            if (config.RejectedChampions.Distinct().Count() != config.RejectedChampions.Count) {
                throw new InvalidDataException($"Values of {nameof(config.RejectedChampions)} are not unique!");
            }

            if (config.RejectedBasicCategories.Distinct().Count() != config.RejectedBasicCategories.Count) {
                throw new InvalidDataException($"Values of {nameof(config.RejectedBasicCategories)} are not unique!");
            }

            if (config.RejectedCustomCategories.Distinct().Count() != config.RejectedCustomCategories.Count) {
                throw new InvalidDataException($"Values of {nameof(config.RejectedCustomCategories)} are not unique!");
            }

            return config;
        }

        #endregion

    }
}