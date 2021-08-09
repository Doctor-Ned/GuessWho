using System;

namespace GuessWhoResources {
    public static class ResourceManager {
        private static Language[] _AvailableLanguages;

        public static Language[] AvailableLanguages {
            get {
                if (_AvailableLanguages != null) {
                    return _AvailableLanguages;
                }

                _AvailableLanguages = new[] {
                    new Language(Locale.en_US, "English (United States)"),
                    new Language(Locale.cs_CZ, "Czech (Czech Republic)"),
                    new Language(Locale.de_DE, "German (Germany)"),
                    new Language(Locale.el_GR, "Greek (Greece)"),
                    new Language(Locale.en_AU, "English (Australia)"),
                    new Language(Locale.en_GB, "English (United Kingdom)"),
                    new Language(Locale.en_PH, "English (Republic of the Philippines)"),
                    new Language(Locale.en_SG, "English (Singapore)"),
                    new Language(Locale.es_AR, "Spanish (Argentina)"),
                    new Language(Locale.es_ES, "Spanish (Spain)"),
                    new Language(Locale.es_MX, "Spanish (Mexico)"),
                    new Language(Locale.fr_FR, "French (France)"),
                    new Language(Locale.hu_HU, "Hungarian (Hungary)"),
                    new Language(Locale.id_ID, "Indonesian (Indonesia)"),
                    new Language(Locale.it_IT, "Italian (Italy)"),
                    new Language(Locale.ja_JP, "Japanese (Japan)"),
                    new Language(Locale.ko_KR, "Korean (Korea)"),
                    new Language(Locale.pl_PL, "Polish (Poland)"),
                    new Language(Locale.pt_BR, "Portuguese (Brazil)"),
                    new Language(Locale.ro_RO, "Romanian (Romania)"),
                    new Language(Locale.ru_RU, "Russian (Russia)"),
                    new Language(Locale.th_TH, "Thai (Thailand)"),
                    new Language(Locale.tr_TR, "Turkish (Turkey)"),
                    new Language(Locale.vn_VN, "Vietnamese (Viet Nam)"),
                    new Language(Locale.zh_CN, "Chinese (China)"),
                    new Language(Locale.zh_MY, "Chinese (Malaysia)"),
                    new Language(Locale.zh_TW, "Chinese (Taiwan)")
                };
                if (_AvailableLanguages.Length != Enum.GetValues(typeof(Locale)).Length) {
                    throw new ApplicationException(
                        $"Language count does not match available locales! ({_AvailableLanguages.Length} != {Enum.GetValues(typeof(Locale)).Length})");
                }

                Array.Sort(_AvailableLanguages);

                return _AvailableLanguages;
            }
        }
    }
}
