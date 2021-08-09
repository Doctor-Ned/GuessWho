using System;

namespace GuessWhoResources {
    public class Language : IComparable<Language> {
        public Locale Locale { get; }
        public string Name { get; }

        internal Language(Locale locale, string name) {
            Locale = locale;
            Name = name;
        }

        public int CompareTo(Language other) {
            if (ReferenceEquals(this, other)) {
                return 0;
            }

            if (ReferenceEquals(null, other)) {
                return 1;
            }

            int nameComparison = string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
            if (nameComparison != 0) {
                return nameComparison;
            }

            return Locale.CompareTo(other.Locale);
        }
    }
}