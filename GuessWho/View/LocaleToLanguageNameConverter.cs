using System;
using System.Globalization;
using System.Windows.Data;

using GuessWhoResources;

namespace GuessWho.View {
    public class LocaleToLanguageNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is Locale locale) {
                return locale.ToCultureInfo().NativeName;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}