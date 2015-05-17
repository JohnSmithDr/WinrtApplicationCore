using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JohnSmithDr.ApplicationCore.Tasks
{
    public abstract class AbstractTaskWrapper : ITaskWrapper
    {
        #region ITaskWrapper

        public event EventHandler StatusChanged;

        protected void OnStatusChanged()
        {
            if (StatusChanged != null)
                StatusChanged.Invoke(this, EventArgs.Empty);
        }

        public TaskStatus Status
        {
            get
            {
                if (_cancelBeforeStarted) return TaskStatus.Canceled;
                return _task != null ? _task.Status : TaskStatus.Created;
            }
        }

        public async Task StartAsync()
        {
            if (_cancelBeforeStarted)
            {
                OnStatusChanged();
                return;
            }

            if (_task == null)
            {
                using (_cts = new CancellationTokenSource())
                {
                    _task = RunAsync(_cts.Token);
                    OnStatusChanged();
                    await _task;
                    OnStatusChanged();
                }
                _cts = null;
            }
            else
            {
                throw new InvalidOperationException("Task has been executed");
            }
        }

        public void Cancel()
        {
            if (_task == null)
            {
                _cancelBeforeStarted = true;
                OnStatusChanged();
            }

            if (_cts != null && _cts.IsCancellationRequested == false)
            {
                OnCancel();
                _cts.Cancel();
                OnStatusChanged();
            }
        }

        protected abstract Task RunAsync(CancellationToken c);

        protected virtual void OnCancel()
        {

        }

        Task _task;
        CancellationTokenSource _cts;
        bool _cancelBeforeStarted;

        #endregion

        public bool IsCompletedStatus
        {
            get
            {
                return
                    this.Status == TaskStatus.RanToCompletion ||
                    this.Status == TaskStatus.Canceled ||
                    this.Status == TaskStatus.Faulted;
            }
        }
    }
}
