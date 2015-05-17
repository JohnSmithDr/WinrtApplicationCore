using System;
using Windows.ApplicationModel.Resources;

namespace JohnSmithDr.ApplicationCore
{
    public class LocalizedStrings : ILocalizedStrings
    {
        public LocalizedStrings(string resource)
        {
            Resource = ResourceLoader.GetForViewIndependentUse(resource);
        }

        public ResourceLoader Resource { get; private set; }

        public string GetString(string key)
        {
            return Resource.GetString(key);
        }

        public string GetFormatString(string key, params object[] args)
        {
            var str = GetString(key);

            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            else
            {
                return string.Format(str, args);
            }
        }

        public string GetState<TEnum>(TEnum value)
        {
            var key = value.GetType().Name + value.ToString();
            var state = GetString(key);
            return state;
        }

        #region Singleton

        public static LocalizedStrings Default
        {
            get { return _Default.Value; }
        }

        private static readonly Lazy<LocalizedStrings> _Default =
            new Lazy<LocalizedStrings>(
              valueFactory: () => new LocalizedStrings("Resources"),
              isThreadSafe: true);

        #endregion
    }
}
