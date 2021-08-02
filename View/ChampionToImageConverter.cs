using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

using GuessWho.Model;

namespace GuessWho.View {
    public class ChampionToImageConverter : IValueConverter {
        private Dictionary<Champion, BitmapImage> ChampionIconDictionary { get; } = new Dictionary<Champion, BitmapImage>();

        public ChampionToImageConverter() {
            foreach (Champion champ in Enum.GetValues(typeof(Champion))) {
                ChampionIconDictionary.Add(champ, new BitmapImage(champ.GetWPFIconUri()));
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null && value is Champion champion) {
                return ChampionIconDictionary[champion];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
