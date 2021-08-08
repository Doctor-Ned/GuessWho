using System;
using System.Globalization;
using System.Windows.Data;

namespace GuessWho.View {
    public class BoolToSettingsToolTipConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(value is bool b)) {
                return null;
            }

            return b ? "Schowaj panel ustawień" : "Pokaż panel ustawień";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}