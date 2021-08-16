using System;
using System.IO;

using GuessWhoDataManager;

using GuessWhoResources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuessWho.Tests {
    [TestClass]
    public class DataManagerTest {
        private const int CHAMP_COUNT_11_15_1 = 156;
        private const string VER_11_15_1 = "11.15.1";

        [TestMethod]
        public void DataDragon_GetAvailableVersions_NonEmpty() {
            string[] versions = DataDragon.GetAvailableVersions();
            Assert.IsNotNull(versions);
            Assert.AreNotEqual(0, versions.Length);
        }

        [TestMethod]
        public void DataDragon_GetLatestVersion_NonEmpty() {
            Assert.IsFalse(string.IsNullOrWhiteSpace(DataDragon.GetLatestVersion()));
        }

        [TestMethod]
        public void DataDragon_ContentValid() {
            DataDragon dd = new DataDragon(VER_11_15_1);
            Assert.AreEqual(CHAMP_COUNT_11_15_1, dd.ChampionIds.Length);
            foreach (Locale locale in Enum.GetValues(typeof(Locale))) {
                Assert.IsTrue(dd.Locales.ContainsKey(locale));
                LocaleLolData lolData = dd.Locales[locale];
                Assert.AreEqual(Enum.GetValues(typeof(BasicCategory)).Length, lolData.BasicCategoryNames.Keys.Count);
                Assert.IsTrue(lolData.ChampionData.ContainsKey("Akshan"));
                Assert.IsTrue(lolData.ChampionData.ContainsKey("Kaisa"));
                Assert.IsTrue(lolData.ChampionData["Kaisa"].BasicCategories.Contains(BasicCategory.Marksman));
            }
            Assert.AreEqual("Kai'Sa", dd.Locales[Locale.en_US].ChampionData["Kaisa"].Name);
            Assert.AreEqual("카이사", dd.Locales[Locale.ko_KR].ChampionData["Kaisa"].Name);
            FileInfo testFileInfo = new FileInfo("TestChampIcon.png");
            if (testFileInfo.Exists) {
                testFileInfo.Delete();
            }
            dd.DownloadChampIcon("Akshan", testFileInfo);
            testFileInfo.Refresh();
            Assert.IsTrue(testFileInfo.Exists);
        }
    }
}
