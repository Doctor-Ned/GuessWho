using System;
using System.Globalization;
using System.Linq;

namespace GuessWhoResources {
    public static class ResourceManager {
        public const string RESOURCE_DIRECTORY = "Resources";
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
    }
}
