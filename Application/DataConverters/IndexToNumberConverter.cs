using System;
using Windows.UI.Xaml.Data;

namespace JohnSmithDr.Application.DataConverters
{
    public class IndexToNumberConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int)value - 1;
        }

        #endregion
    }
}
