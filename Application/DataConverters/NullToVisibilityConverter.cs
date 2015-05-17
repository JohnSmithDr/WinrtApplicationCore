using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JohnSmithDr.Application.DataConverters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public bool Reverse { get; set; }

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (Reverse)
            {
                return (value == null) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return (value == null) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
