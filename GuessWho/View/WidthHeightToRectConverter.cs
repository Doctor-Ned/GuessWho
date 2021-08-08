using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GuessWho.View {
    public class WidthHeightToRectConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length == 2 && values[0] is double width && values[1] is double height && width > 0.0 && height > 0.0) {
                return new Rect(new Size(width, height));
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}