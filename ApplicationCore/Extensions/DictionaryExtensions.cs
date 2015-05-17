namespace System.Collections.Generic
{
    public static class DictionaryExtensions
    {
        public static void ForEach<TKey, TValue>(this IDictionary<TKey, TValue> dict, Action<TKey, TValue> action)
        {
            foreach (var i in dict)
            {
                action(i.Key, i.Value);
            }
        }

        public static TRet GetTypedValue<TKey, TRet>(this IDictionary<TKey, object> dict, TKey key, TRet defaultValue = default(TRet))
        {
            if(dict.ContainsKey(key))
            {
                var value = dict[key];
                if(value is TRet) return (TRet)value;
            }
            return defaultValue;
        }
    }
}
