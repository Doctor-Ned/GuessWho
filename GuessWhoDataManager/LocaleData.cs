﻿using System;
using System.Collections.Generic;
using System.Net;

using GuessWhoResources;

using Newtonsoft.Json.Linq;

namespace GuessWhoDataManager {
    internal class LocaleData {
        public Dictionary<string, LocaleChampionData> ChampionData { get; }
        public Dictionary<BasicCategory, string> BasicCategoryNames { get; }

        internal LocaleData(Locale locale, string localisedUrl) {
            JObject langObj, champObj;
            using (WebClient wc = new WebClient()) {
                wc.Headers["Accept-Language"] = locale.ToString();
                langObj = JObject.Parse(wc.DownloadString($"{localisedUrl}language.json"))["data"].Value<JObject>();
                champObj = JObject.Parse(wc.DownloadString($"{localisedUrl}champion.json"))["data"].Value<JObject>();
            }
            BasicCategoryNames = new Dictionary<BasicCategory, string>();
            foreach (BasicCategory bc in Enum.GetValues(typeof(BasicCategory))) {
                BasicCategoryNames.Add(bc, langObj[bc.ToString()].Value<string>());
            }

            ChampionData = new Dictionary<string, LocaleChampionData>();
            foreach (JProperty prop in champObj.Properties()) {
                ChampionData.Add(prop.Name, new LocaleChampionData(prop.Value.Value<JObject>()));
            }
        }
    }
}
