using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GuessWho.View {
    public class IntToSizeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is int i) {
                return new Size(i, i);
            }

            return Size.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
