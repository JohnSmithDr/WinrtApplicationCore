
namespace Windows.UI.Xaml
{
    public static class ApplicationResourceExtensions
    {
        public static TResult GetFromResource<TResult>(this Application app, string key, TResult fallback = default(TResult))
        {
            if (app.Resources.ContainsKey(key))
            {
                var res = app.Resources[key];
                if (res is TResult) return (TResult)res;
            }
            return fallback;
        }
    }
}
