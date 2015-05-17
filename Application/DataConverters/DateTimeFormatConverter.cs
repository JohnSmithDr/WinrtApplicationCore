using System;
using Windows.UI.Xaml.Data;

namespace JohnSmithDr.Application.DataConverters
{
    public class DateTimeFormatConverter : IValueConverter
    {
        public string Format { get; set; }

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime)
            {
                var time = (DateTime)value;
                var localTime = time.ToLocalTime();
                return (!string.IsNullOrEmpty(this.Format)) ? localTime.ToString(this.Format) : localTime.ToString();
            }
            if (value is DateTimeOffset)
            {
                var time = (DateTimeOffset)value;
                var localTime = time.DateTime.ToLocalTime();
                return (!string.IsNullOrEmpty(this.Format)) ? localTime.ToString(this.Format) : localTime.ToString();
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
