using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using GuessWho.Model;

namespace GuessWho.View {
    public class ChampionToImageConverter : IValueConverter {
        public ChampionToImageConverter() {
            foreach (string champId in ChampionProvider.AllChampionIds) {
                ChampionIconDictionary.Add(champId, new BitmapImage(ChampionProvider.GetWPFIconUri(champId)));
            }
        }

        private Dictionary<string, BitmapImage> ChampionIconDictionary { get; } =
            new Dictionary<string, BitmapImage>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is string champId) {
                return ChampionIconDictionary[champId];
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}