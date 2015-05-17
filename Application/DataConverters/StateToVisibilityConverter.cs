using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace JohnSmithDr.Application.DataConverters
{
    public class StateToVisibilityConverter : IValueConverter
    {
        public StateComparationMode ComparationMode { get; set; }

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                var valueStr = value.ToString();
                var stateStr = parameter.ToString();
                var equals = valueStr.Equals(stateStr, StringComparison.Ordinal);

                if (ComparationMode == StateComparationMode.Equals)
                {
                    return equals ? Visibility.Visible : Visibility.Collapsed;
                }
                else if (ComparationMode == StateComparationMode.NotEquals)
                {
                    return equals ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public enum StateComparationMode
    {
        Equals,
        NotEquals
    }
}
