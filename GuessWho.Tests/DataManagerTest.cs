using System;

using GuessWhoDataManager;

using GuessWhoResources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessWho.Tests {
    [TestClass]
    public class DataManagerTest {
        private const int CHAMP_COUNT_11_15_1 = 156;
        private const string VER_11_15_1 = "11.15.1";

        [DataTestMethod]
        [DataRow(VER_11_15_1, CHAMP_COUNT_11_15_1)]
        public void InitializeChampionIds_SuccessfulAndChampCountValid(string version, int champCount) {
            string url = Statics.GetDataDragonUrl(version);
            Statics.InitializeChampionIds(url);
            Assert.AreEqual(champCount, Statics.AllChampionIds.Length);
            Assert.IsNotNull(Statics.CustomCategoryMappings);
        }

        [DataTestMethod]
        [DataRow(VER_11_15_1, Locale.en_US, CHAMP_COUNT_11_15_1, "Kaisa", "Kai'Sa", BasicCategory.Marksman)]
        [DataRow(VER_11_15_1, Locale.ko_KR, CHAMP_COUNT_11_15_1, "Kaisa", "카이사", BasicCategory.Marksman)]
        public void LocaleData_ConstructionAndContentValid(string version, Locale locale, int champCount, string testChampId, string testChampName, BasicCategory basicCategory) {
            string url = Statics.GetDataDragonUrl(version);
            Statics.InitializeChampionIds(url);
            string localisedUrl = Statics.LocalizeUrl(url, locale);
            LocaleData data = new LocaleData(locale, localisedUrl);
            Assert.AreEqual(Enum.GetValues(typeof(BasicCategory)).Length, data.BasicCategoryNames.Keys.Count);
            Assert.AreEqual(champCount, data.ChampionData.Keys.Count);
            Assert.IsTrue(data.ChampionData.ContainsKey(testChampId));
            LocaleChampionData testChamp = data.ChampionData[testChampId];
            Assert.AreEqual(testChampName, testChamp.Name);
            Assert.IsTrue(testChamp.BasicCategories.Contains(basicCategory));
        }
    }
}
