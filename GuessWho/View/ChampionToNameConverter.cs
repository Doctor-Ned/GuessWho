using System;
using System.Globalization;
using System.Windows.Data;

using WPFLocalizeExtension.Engine;

namespace GuessWho.View {
    public class ChampionToNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is string champId) {
                return LocalizeDictionary.Instance.GetLocalizedObject($"League_Champion_{champId}_Name", null, LocalizeDictionary.Instance.Culture);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}