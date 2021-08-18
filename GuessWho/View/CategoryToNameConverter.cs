using System;
using System.Globalization;
using System.Windows.Data;

using GuessWho.Model;

using GuessWhoResources;

namespace GuessWho.View {
    public class CategoryToNameConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                if (value is BasicCategory basicCategory) {
                    return ResourceProvider.GetLocalizedCategoryName(basicCategory);
                }
                if (value is CustomCategory customCategory) {
                    return ResourceProvider.GetLocalizedCategoryName(customCategory);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}