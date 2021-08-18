using System;
using System.Globalization;
using System.Windows.Data;

namespace GuessWho.View {
    public class BoolToMessageConverter : IValueConverter {
        public string TrueMessage { get; set; }
        public string FalseMessage { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (!(value is bool b)) {
                return null;
            }

            return b ? TrueMessage : FalseMessage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}