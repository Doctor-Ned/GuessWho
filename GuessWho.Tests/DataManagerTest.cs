using System;

using GuessWhoDataManager;

using GuessWhoResources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessWho.Tests {
    [TestClass]
    public class DataManagerTest {
        [TestMethod]
        public void InitializeChampionIds_11_15_1_Successful() {
            string url = Statics.GetDataDragonUrl("11.15.1");
            Statics.InitializeChampionIds(url);
            Assert.AreEqual(156, Statics.AllChampionIds.Length);
            Assert.IsNotNull(Statics.CustomCategoryMappings);
        }

        [TestMethod]
        public void LocaleData_11_15_1_ko_KR_ConstructionValid() {
            string url = Statics.GetDataDragonUrl("11.15.1");
            Statics.InitializeChampionIds(url);
            string localisedUrl = Statics.LocalizeUrl(url, Locale.ko_KR);
            LocaleData data = new LocaleData(Locale.ko_KR, localisedUrl);
            Assert.AreEqual(Enum.GetValues(typeof(BasicCategory)).Length, data.BasicCategoryNames.Keys.Count);
            Assert.AreEqual(156, data.ChampionData.Keys.Count);
        }
    }
}
