using System;
using System.Globalization;
using System.Windows.Data;

using GuessWhoResources;

using WPFLocalizeExtension.Engine;

namespace GuessWho.View {
    public class CategoryToNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                if (value is BasicCategory basicCategory) {
                    return LocalizeDictionary.Instance.GetLocalizedObject($"{ResourceType.League}_{nameof(BasicCategory)}_{basicCategory}", null, LocalizeDictionary.Instance.Culture);
                }
                if (value is CustomCategory customCategory) {
                    return LocalizeDictionary.Instance.GetLocalizedObject($"{ResourceType.CustomCategories}_{customCategory}", null, LocalizeDictionary.Instance.Culture);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}