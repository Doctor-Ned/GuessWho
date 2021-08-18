using System;
using System.Globalization;
using System.Linq;

namespace GuessWhoResources {
    public static class ResourceManager {
        public const Locale DEFAULT_LOCALE = Locale.en_US;
        public const string RESOURCE_FILENAME = "Resources";
        public const string RESOURCE_DIRECTORY = "Properties";
        public const string ICON_DIRECTORY = "Champions";
        public const string ICON_EXTENSION = "png";

        public static Language[] AvailableLanguages { get; } = Enum.GetValues(typeof(Locale)).Cast<Locale>()
            .Select(l => new Language(l)).OrderBy(l => l).ToArray();

        public static CultureInfo ToCultureInfo(this Locale locale) {
            return new CultureInfo(locale.ToCultureInfoString());
        }

        public static string ToCultureInfoString(this Locale locale) {
            return locale.ToString().Replace('_', '-');
        }

        public static string GetLanguageCode(this Locale locale) {
            return locale.ToString().Substring(0, 2);
        }

        public static Locale[] GetLocalesWithSameLanguage(this Locale locale) {
            return Enum.GetValues(typeof(Locale)).Cast<Locale>()
                .Where(l => l != locale && l.GetLanguageCode() == locale.GetLanguageCode()).ToArray();
        }

        public static Locale[] GetLocalesWithLanguage(string languageCode) {
            if (languageCode.Length != 2) {
                throw new ArgumentException(nameof(languageCode));
            }
            return Enum.GetValues(typeof(Locale)).Cast<Locale>()
                .Where(l => l.GetLanguageCode() == languageCode).ToArray();
        }

        public static Locale GetClosestLocale(this CultureInfo cultureInfo) {
            if (Enum.TryParse(cultureInfo.Name.Replace('-', '_'), out Locale locale)) {
                return locale;
            }
            Locale[] localesWithSameLanguage = GetLocalesWithLanguage(cultureInfo.TwoLetterISOLanguageName);
            return localesWithSameLanguage.Length != 0 ? localesWithSameLanguage[0] : DEFAULT_LOCALE;
        }
    }
}
