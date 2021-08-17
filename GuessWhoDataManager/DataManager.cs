using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

using GuessWhoResources;

namespace GuessWhoDataManager {
    internal class DataManager {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        internal const string DATA_DRAGON_VERSION_KEY = "DataDragonVersion";
        internal const string MAIN_PROJECT_NAME = "GuessWho";
        internal const string DATA_MANAGER_PROJECT_NAME = "GuessWhoDataManager";
        internal const string RESOURCE_PROJECT_NAME = "GuessWhoResources";
        internal const string RESOURCES = "Resources";
        internal const Locale DEFAULT_LOCALE = Locale.en_US;

        internal static string DefaultSolutionPath {
            get { return Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName ?? throw new InvalidOperationException(), "../../.."); }
        }

        public DataManager() : this(DefaultSolutionPath) { }

        public DataManager(string solutionPath) {
            DirectoryInfo dir = new DirectoryInfo(solutionPath);
            if (!dir.Exists) {
                throw new ArgumentException($"Solution path '{solutionPath}' does not exist!");
            }

            SolutionPath = dir.FullName;
            Logger.Info($"DataManager created with {nameof(SolutionPath)} '{SolutionPath}'!");
        }

        public void ReworkResources(bool force) {
            string latestVersion = DataDragon.GetLatestVersion();
            string versionResourcePath = Path.Combine(SolutionPath, RESOURCE_PROJECT_NAME, $"{RESOURCES}.resx");
            if (!force) {
                using (ResXResourceReader versionResourceReader = new ResXResourceReader(versionResourcePath)) {
                    string currentVersion = versionResourceReader.Cast<DictionaryEntry>()
                    .First(e => e.Key is string s && s == DATA_DRAGON_VERSION_KEY).Value as string;
                    if (currentVersion == latestVersion) {
                        Logger.Info($"The resources are already up to date (current version: {latestVersion})!");
                        return;
                    }
                }
            }

            Dictionary<Locale, LocaleData> localeDatas = new Dictionary<Locale, LocaleData>();
            foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                Logger.Debug($"Preparing data for locale {locale}...");
                Dictionary<string, string> localeLabels = new Dictionary<string, string>();
                Dictionary<CustomCategory, string> customCategoryLabels = new Dictionary<CustomCategory, string>();
                using (ResXResourceReader localeReader = new ResXResourceReader(
                    Path.Combine(SolutionPath, DATA_MANAGER_PROJECT_NAME, RESOURCES,
                        ResourceType.Locale.ToString(), $"{ResourceType.Locale}.{locale.ToCultureInfoString()}.resx"))) {
                    foreach (DictionaryEntry entry in localeReader) {
                        if (entry.Value == null) {
                            Logger.Warn($"Locale {locale} entry '{entry.Key}' value is empty!");
                            localeLabels.Add(entry.Key.ToString(), "");
                        } else {
                            localeLabels.Add(entry.Key.ToString(), entry.Value.ToString());
                        }
                    }
                }
                using (ResXResourceReader customCategoryReader = new ResXResourceReader(Path.Combine(SolutionPath, DATA_MANAGER_PROJECT_NAME, RESOURCES,
                    nameof(ResourceType.CustomCategories), $"{nameof(ResourceType.CustomCategories)}.{locale.ToCultureInfoString()}.resx"))) {
                    foreach (DictionaryEntry entry in customCategoryReader) {
                        if (Enum.GetValues(typeof(CustomCategory)).Cast<CustomCategory>()
                            .Any(c => c.ToString() == entry.Key.ToString())) {
                            if (entry.Value == null) {
                                Logger.Warn($"Locale {locale} custom category '{entry.Key}' value is empty!");
                            }
                            customCategoryLabels.Add((CustomCategory)Enum.Parse(typeof(CustomCategory), entry.Key.ToString()), entry.Value == null ? "" : entry.Value.ToString());
                        } else {
                            Logger.Error($"Locale {locale} custom category resource file contains invalid key '{entry.Key}'. This key will be omitted and should be removed from the file.");
                        }
                    }
                }
                localeDatas.Add(locale, new LocaleData(locale, localeLabels, customCategoryLabels));
            }

            if (localeDatas[DEFAULT_LOCALE].CustomCategoryLabels.Keys.Count !=
                Enum.GetValues(typeof(CustomCategory)).Length) {
                throw new InvalidDataException($"Default locale {DEFAULT_LOCALE} custom categories are incomplete: fill the appropriate resource file to proceed.");
            }

            foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                if (locale != DEFAULT_LOCALE) {
                    localeDatas[locale].RefillMissingData(localeDatas, DEFAULT_LOCALE);
                }

                using (ResXResourceWriter writer = new ResXResourceWriter(Path.Combine(SolutionPath,
                    RESOURCE_PROJECT_NAME, RESOURCES, $"{ResourceType.Locale}.{locale.ToCultureInfoString()}.resx"))) {
                    Logger.Debug($"Writing {locale} resource file '{writer.BasePath}'...");
                    foreach (KeyValuePair<string, string> pair in localeDatas[locale].LocaleLabels) {
                        writer.AddResource($"{ResourceType.Locale}.{pair.Key}", pair.Value);
                    }
                    foreach (KeyValuePair<CustomCategory, string> pair in localeDatas[locale].CustomCategoryLabels) {
                        writer.AddResource($"{ResourceType.CustomCategories}.{pair.Key}", pair.Value);
                    }
                    writer.Generate();
                }
            }

            //todo: create all league-related resources!

            throw new NotImplementedException();

            using (ResXResourceWriter versionResourceWriter = new ResXResourceWriter(versionResourcePath)) {
                versionResourceWriter.AddResource(DATA_DRAGON_VERSION_KEY, latestVersion);
                versionResourceWriter.Generate();
            }
        }

        private string SolutionPath { get; }
    }
}
