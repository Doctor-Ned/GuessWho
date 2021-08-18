using System;
using System.Globalization;
using System.Windows.Data;

using GuessWho.Model;

namespace GuessWho.View {
    public class ChampionToNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is string champId) {
                return ResourceProvider.GetLocalizedChampionName(champId);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}