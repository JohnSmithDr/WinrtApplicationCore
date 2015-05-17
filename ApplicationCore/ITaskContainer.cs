using System.Threading.Tasks;

namespace JohnSmithDr.ApplicationCore
{
    public interface ITaskContainer<TResult>
    {
        TaskCompletionSource<TResult> TaskSource { get; }
    }
}
