using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace WwiseTool.Converters {
    public class BooleanInverseConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is not bool) return value;
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return value;
        }
    }

    public class BooleanInverseVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is not bool) return Visibility.Collapsed;
            return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return Visibility.Collapsed;
        }
    }

    public class NotNullVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is not null) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return Visibility.Collapsed;
        }
    }

    public class NullVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            if (value is null) return Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return Visibility.Collapsed;
        }
    }
}