using System.Threading.Tasks;

namespace System.Collections
{
    public static class CollectionExtentions
    {
        #region ForEach

        public static void ForEach(this IEnumerable source, Action<object> action)
        {
            foreach (var item in source)
            {
                action.Invoke(item);
            }
        }

        public static async Task ForEachAsync(this IEnumerable source, Func<object, Task> taskFactory)
        {
            foreach (var item in source)
            {
                await taskFactory.Invoke(item);
            }
        } 

        #endregion
    }
}