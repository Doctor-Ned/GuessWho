using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GuessWho.View {
    public class DoubleToSizeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double d) {
                return new Size(d, d);
            }

            return Size.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}