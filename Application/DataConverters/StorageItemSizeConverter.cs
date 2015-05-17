using System;
using Windows.UI.Xaml.Data;

namespace JohnSmithDr.Application.DataConverters
{
    public class StorageItemSizeConverter : IValueConverter
    {
        public static string ToFileSize(double size)
        {
            const double u = 1024d;
            double s = (double)size;
            if (s < u) return string.Format("{0:0} B", s);
            if ((s /= u) < u) return string.Format("{0:0} KB", s);
            if ((s /= u) < u) return string.Format("{0:0.00} MB", s);
            if ((s /= u) < u) return string.Format("{0:0.00} GB", s);
            if ((s /= u) < u) return string.Format("{0:0.00} TB", s);
            return string.Format("{0:0.00} PB", s /= u);
        }

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return " ";
            if (value is double) return ToFileSize((double)value);
            else if (value is ulong) return ToFileSize((ulong)value);
            else if (value is long) return ToFileSize((long)value);
            else if (value is uint) return ToFileSize((uint)value);
            else if (value is int) return ToFileSize((int)value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
