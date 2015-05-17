using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace System
{
    public static class SystemExtentions
    {
        public static int CompareTo<TObj, TRet>(this TObj source, TObj obj, Expression<Func<TObj, TRet>> property)
            where TRet : IComparable<TRet>
        {
            var func = property.Compile();
            var ap = func(source);
            var bp = func(source);
            return ap.CompareTo(bp);
        }

        public static void ShouldNotBeNull(this object obj, string key, bool throwException = true)
        {
            Debug.Assert(obj != null, string.Format("{0} should not be null", key));

            if (obj == null && throwException)
            {
                throw new ArgumentNullException(key);
            }
        }

        public static void TryDispose(this IDisposable obj)
        {
            if (obj != null)
            {
                try
                {
                    obj.Dispose();
                }
                catch
                {
                    // just ignore exception
                }
            }
        }

        public static void TryReport<T>(this IProgress<T> progress, T value)
        {
            if (progress != null)
            {
                progress.Report(value);
            }
        }
    }
}
