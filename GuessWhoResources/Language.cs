using System;
using System.Globalization;

namespace GuessWhoResources {
    public class Language : IComparable<Language> {
        public Locale Locale { get; }

        public CultureInfo CultureInfo { get; }

        public string DisplayName { get; }

        public string EnglishName { get; }

        public string LanguageCode { get; }

        internal Language(Locale locale) {
            Locale = locale;
            CultureInfo = locale.ToCultureInfo();
            DisplayName = CultureInfo.DisplayName;
            EnglishName = CultureInfo.EnglishName;
            LanguageCode = locale.GetLanguageCode();
        }

        public int CompareTo(Language other) {
            if (ReferenceEquals(this, other)) {
                return 0;
            }

            if (ReferenceEquals(null, other)) {
                return 1;
            }

            int nameComparison = string.Compare(DisplayName, other.DisplayName, StringComparison.OrdinalIgnoreCase);
            if (nameComparison != 0) {
                return nameComparison;
            }

            return Locale.CompareTo(other.Locale);
        }
    }
}