using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JohnSmithDr.ApplicationCore.Tasks
{
    public abstract class AbstractTaskWithProgress<TProgress> : AbstractTaskWrapper, ITaskWrapper, ITaskWithProgress<TProgress>
    {
        #region ITaskWithProgress<TProgress>

        public TProgress Progress { get; protected set; }

        public event EventHandler<TaskProgressEventArgs<TProgress>> Progressed;

        protected void OnProgressed(TProgress progress)
        {
            Progress = progress;

            if (Progressed != null)
            {
                Progressed.Invoke(this, new TaskProgressEventArgs<TProgress> { Progress = progress });
            }
        }

        #endregion

        protected override async Task RunAsync(CancellationToken c)
        {
            await RunAsync(c, new Progress<TProgress>(g =>
            {
                OnProgressed(g);
                OnStatusChanged();
            }));

            OnStatusChanged();
        }

        protected abstract Task RunAsync(CancellationToken c, IProgress<TProgress> progress);
    }

    public abstract class AbstractTaskWithProgress<TResult, TProgress> : AbstractTaskWrapper, ITaskWrapper, ITaskWithProgress<TResult, TProgress>
    {
        #region ITaskWithProgress<TProgress>

        public TProgress Progress { get; protected set; }

        public event EventHandler<TaskProgressEventArgs<TProgress>> Progressed;

        protected void OnProgressed(TProgress progress)
        {
            Progress = progress;

            if (Progressed != null)
            {
                Progressed.Invoke(this, new TaskProgressEventArgs<TProgress> { Progress = progress });
            }
        }

        #endregion

        #region ITaskWithProgress<TResult,TProgress>

        public TResult Result { get; protected set; }

        #endregion

        protected override async Task RunAsync(CancellationToken c)
        {
            Result = await RunAsync(c, new Progress<TProgress>(g =>
            {
                OnProgressed(g);
                OnStatusChanged();
            }));

            OnStatusChanged();
        }

        protected abstract Task<TResult> RunAsync(CancellationToken c, IProgress<TProgress> progress);
    }
}
