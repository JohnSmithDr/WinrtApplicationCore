using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JohnSmithDr.Application.DataConverters
{
    public class DoubleToPercentageConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is double) ? string.Format("{0:0}%", value) : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
