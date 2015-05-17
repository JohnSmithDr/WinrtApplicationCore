using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JohnSmithDr.ApplicationCore.Tasks
{
    public interface ITaskWrapper
    {
        event EventHandler StatusChanged;

        TaskStatus Status { get; }

        bool IsCompletedStatus { get; }

        Task StartAsync();

        void Cancel();
    }

    public interface ITaskWithProgress<TProgress> : ITaskWrapper
    {
        event EventHandler<TaskProgressEventArgs<TProgress>> Progressed;

        TProgress Progress { get; }
    }

    public interface ITaskWithProgress<TResult, TProgress> : ITaskWrapper, ITaskWithProgress<TProgress>
    {
        TResult Result { get; }
    }

    public interface ITaskExecutor<TTask> where TTask : ITaskWrapper
    {
        void Post(TTask task);

        void Post(IEnumerable<TTask> tasks);

        Task WaitCompletionAsync();
    }
}
