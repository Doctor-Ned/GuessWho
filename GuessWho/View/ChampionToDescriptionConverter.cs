using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

using GuessWho.Model;

using GuessWhoResources;

namespace GuessWho.View {
    public class ChampionToDescriptionConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is string champId) {
                StringBuilder builder = new StringBuilder();
                foreach (CustomCategory category in ChampionProvider.GetCustomCategories(champId)) {
                    if (builder.Length != 0) {
                        builder.AppendLine();
                    }
                    builder.Append(ResourceProvider.GetLocalizedCategoryName(category));
                }

                foreach (BasicCategory category in ChampionProvider.GetBasicCategories(champId)) {
                    if (builder.Length != 0) {
                        builder.AppendLine();
                    }

                    builder.Append(ResourceProvider.GetLocalizedCategoryName(category));
                }

                return builder.ToString();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}