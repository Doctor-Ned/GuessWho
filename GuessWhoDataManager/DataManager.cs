using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Xml;

using GuessWhoResources;

using ResourceManager = GuessWhoResources.ResourceManager;

namespace GuessWhoDataManager {
    internal class DataManager {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        internal const string DATA_DRAGON_VERSION_KEY = "DataDragonVersion";
        internal const string LEAGUE_CHAMPION_CONFIG_KEY = "LeagueChampionConfig";
        internal const string XML_RESOURCE_NODE = "Resource";
        internal const string XML_ITEMGROUP_NODE = "ItemGroup";
        internal const string XML_INCLUDE_ATTRIBUTE = "Include";
        internal const Locale DEFAULT_LOCALE = Locale.en_US;

        internal static string DefaultSolutionPath {
            get { return Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName ?? throw new InvalidOperationException(), "../../.."); }
        }

        internal static string DataManagerProjectName {
            get { return Assembly.GetExecutingAssembly().GetName().Name; }
        }

        internal static string ResourcesProjectName {
            get { return typeof(ResourceManager).Assembly.GetName().Name; }
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

        public void ReworkResources(string version) {
            // todo: this method could be split into multiple smaller ones for improved readability
            Dictionary<Locale, LocaleData> localeDatas = new Dictionary<Locale, LocaleData>();
            foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                Logger.Debug($"Preparing data for locale {locale}...");
                Dictionary<string, string> localeLabels = new Dictionary<string, string>();
                Dictionary<CustomCategory, string> customCategoryLabels = new Dictionary<CustomCategory, string>();
                using (ResXResourceReader localeReader = new ResXResourceReader(GetDataManagerLocaleResourcePath(locale))) {
                    foreach (DictionaryEntry entry in localeReader) {
                        if (entry.Value == null) {
                            Logger.Warn($"Locale {locale} entry '{entry.Key}' value is empty!");
                            localeLabels.Add(entry.Key.ToString(), "");
                        } else {
                            localeLabels.Add(entry.Key.ToString(), entry.Value.ToString());
                        }
                    }
                }
                using (ResXResourceReader customCategoryReader = new ResXResourceReader(GetDataManagerCustomCategoriesResourcePath(locale))) {
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

            Logger.Info("Verifying resource data...");

            if (localeDatas[DEFAULT_LOCALE].CustomCategoryLabels.Keys.Count !=
                Enum.GetValues(typeof(CustomCategory)).Length) {
                throw new InvalidDataException($"Default locale {DEFAULT_LOCALE} custom categories are incomplete: fill the appropriate resource file to proceed.");
            }

            foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                if (locale != DEFAULT_LOCALE) {
                    localeDatas[locale].RefillMissingFromSimilarLanguages(localeDatas, DEFAULT_LOCALE);
                }
            }

            foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                if (locale != DEFAULT_LOCALE) {
                    localeDatas[locale].RefillMissingFromDefaultLocale(localeDatas[DEFAULT_LOCALE]);
                }
            }

            Logger.Info("Processing DataDragon...");

            DataDragon dataDragon = new DataDragon(version);
            LeagueChampionConfig config = new LeagueChampionConfig();
            foreach (KeyValuePair<string, LocaleLolChampionData> pair in dataDragon.Locales[DEFAULT_LOCALE].ChampionData) {
                config.ChampionCategoryConfigs.Add(pair.Key, new ChampionCategoryConfig {
                    BasicCategories = pair.Value.BasicCategories,
                    CustomCategories = pair.Value.CustomCategories
                });
            }

            foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                LocaleLolData lolData = dataDragon.Locales[locale];
                using (ResXResourceWriter writer = new ResXResourceWriter(GetOutputResourcePath(locale))) {
                    Logger.Debug($"Writing {locale} resource file '{writer.BasePath}'...");
                    foreach (KeyValuePair<string, string> pair in localeDatas[locale].LocaleLabels) {
                        writer.AddResource($"{ResourceType.Locale}_{pair.Key}", pair.Value);
                    }
                    foreach (KeyValuePair<CustomCategory, string> pair in localeDatas[locale].CustomCategoryLabels) {
                        writer.AddResource($"{ResourceType.CustomCategories}_{pair.Key}", pair.Value);
                    }
                    foreach (KeyValuePair<BasicCategory, string> pair in lolData.BasicCategoryNames) {
                        writer.AddResource($"{ResourceType.League}_{nameof(BasicCategory)}_{pair.Key}", pair.Value);
                    }
                    foreach (KeyValuePair<string, LocaleLolChampionData> pair in lolData.ChampionData) {
                        writer.AddResource($"{ResourceType.League}_Champion_{pair.Key}_Name", pair.Value.Name);
                        writer.AddResource($"{ResourceType.League}_Champion_{pair.Key}_Title", pair.Value.Title);
                    }
                    writer.Generate();
                }
            }

            Logger.Info("Updating champion icons...");
            XmlDocument doc = new XmlDocument();
            bool xmlDocModified = false;
            string projectFilePath =
                Path.Combine(SolutionPath, ResourcesProjectName, $"{ResourcesProjectName}.csproj");
            doc.Load(projectFilePath);
            if (doc.DocumentElement == null) {
                throw new InvalidOperationException("Provided resource project file is invalid!");
            }

            string xmlns = doc.DocumentElement.Attributes["xmlns"].Value;
            XmlNodeList resourceNodes = doc.GetElementsByTagName(XML_RESOURCE_NODE);
            foreach (string id in dataDragon.ChampionIds) {
                string resourceIncludeAttribute = $"{ResourceManager.ICON_DIRECTORY}\\{id}.{ResourceManager.ICON_EXTENSION}";
                FileInfo iconFile = new FileInfo(Path.Combine(SolutionPath, ResourcesProjectName, resourceIncludeAttribute));
                if (!iconFile.Exists) {
                    dataDragon.DownloadChampIcon(id, iconFile);
                }

                bool resourceFound = false;
                foreach (XmlNode node in resourceNodes.Cast<XmlNode>()) {
                    XmlAttribute includeAttribute = node.Attributes?[XML_INCLUDE_ATTRIBUTE];
                    if (includeAttribute != null && includeAttribute.Value == resourceIncludeAttribute) {
                        resourceFound = true;
                        break;
                    }
                }

                if (!resourceFound) {
                    XmlElement itemGroupElement = doc.CreateElement(XML_ITEMGROUP_NODE, xmlns);
                    XmlElement resourceElement = doc.CreateElement(XML_RESOURCE_NODE, xmlns);
                    resourceElement.SetAttribute(XML_INCLUDE_ATTRIBUTE, resourceIncludeAttribute);
                    itemGroupElement.AppendChild(resourceElement);
                    doc.DocumentElement?.AppendChild(itemGroupElement);
                    xmlDocModified = true;
                }
            }

            if (xmlDocModified) {
                Logger.Info($"Overwriting project file '{projectFilePath}'...");
                doc.Save(projectFilePath);
            }

            using (ResXResourceWriter versionResourceWriter = new ResXResourceWriter(VersionResourcePath)) {
                versionResourceWriter.AddResource(DATA_DRAGON_VERSION_KEY, version);
                versionResourceWriter.AddResource(LEAGUE_CHAMPION_CONFIG_KEY, config.ToJson());
                versionResourceWriter.Generate();
            }
        }

        public void ReworkResources(bool force) {
            string latestVersion = DataDragon.GetLatestVersion();
            if (!force) {
                using (ResXResourceReader versionResourceReader = new ResXResourceReader(VersionResourcePath)) {
                    string currentVersion = versionResourceReader.Cast<DictionaryEntry>()
                    .First(e => e.Key is string s && s == DATA_DRAGON_VERSION_KEY).Value as string;
                    if (currentVersion == latestVersion) {
                        Logger.Info($"The resources are already up to date (current version: {latestVersion})!");
                        return;
                    }
                }
            }
            ReworkResources(latestVersion);
        }

        private string SolutionPath { get; }

        public string VersionResourcePath {
            get { return Path.Combine(SolutionPath, ResourcesProjectName, "ResourceData.resx"); }
        }

        private string GetDataManagerLocaleResourcePath(Locale locale) {
            return Path.Combine(SolutionPath, DataManagerProjectName, ResourceManager.RESOURCE_DIRECTORY,
                ResourceType.Locale.ToString(), $"{ResourceType.Locale}.{locale.ToCultureInfoString()}.resx");
        }

        private string GetDataManagerCustomCategoriesResourcePath(Locale locale) {
            return Path.Combine(SolutionPath, DataManagerProjectName, ResourceManager.RESOURCE_DIRECTORY,
                ResourceType.CustomCategories.ToString(), $"{ResourceType.CustomCategories}.{locale.ToCultureInfoString()}.resx");
        }

        private string GetOutputResourcePath(Locale locale) {
            return Path.Combine(SolutionPath, ResourcesProjectName, ResourceManager.RESOURCE_DIRECTORY,
                $"{ResourceManager.RESOURCE_FILENAME}.{locale.ToCultureInfoString()}.resx");
        }
    }
}
