using System;

namespace JohnSmithDr.ApplicationCore.Tasks
{
    public class TaskProgressEventArgs<TProgress> : EventArgs
    {
        public TProgress Progress { get; internal set; }
    }
}
