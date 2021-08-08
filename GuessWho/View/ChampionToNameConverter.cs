using System;
using System.Globalization;
using System.Windows.Data;

using GuessWho.Model;

namespace GuessWho.View {
    public class ChampionToNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is Champion champion) {
                return champion.GetName();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}