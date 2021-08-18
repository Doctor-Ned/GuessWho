using System;
using System.Windows;
using System.Windows.Data;

namespace GuessWho.View {
    public class DoubleGridLengthConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value != null && value is double d) {
                return new GridLength(d);
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (value != null && value is GridLength gl) {
                return gl.Value;
            }
            return null;
        }
    }
}
