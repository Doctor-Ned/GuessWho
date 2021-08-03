using System;
using System.Globalization;
using System.Windows.Data;

using MaterialDesignThemes.Wpf;

namespace GuessWho.View {
    public class BoolToSettingsIconConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(value is bool b)) {
                return null;
            }

            return b ? PackIconKind.SettingsOff : PackIconKind.Settings;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
