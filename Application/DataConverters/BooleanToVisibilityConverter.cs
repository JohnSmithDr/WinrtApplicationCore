using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JohnSmithDr.Application.DataConverters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Reverse { get; set; }

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                var bv = (bool)value;

                if (Reverse)
                {
                    return bv ? Visibility.Collapsed : Visibility.Visible;
                }
                else
                {
                    return bv ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
