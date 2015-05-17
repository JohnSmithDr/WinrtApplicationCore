///
/// This code requires Microsoft TPL Dataflow library
///

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace JohnSmithDr.ApplicationCore.Tasks
{
    public class TaskExecuter<TTask> : ITaskExecutor<TTask> where TTask : ITaskWrapper
    {
        readonly BufferBlock<TTask> bufferBlock;
        readonly ActionBlock<TTask> executeBlock;

        public TaskExecuter(TimeSpan delay)
        {
            bufferBlock = new BufferBlock<TTask>();
            executeBlock = new ActionBlock<TTask>(async t =>
            {
                await t.StartAsync();
                await Task.Delay(delay);
            });

            bufferBlock.LinkTo(executeBlock);
        }

        public void Post(TTask task)
        {
            this.bufferBlock.Post(task);
        }

        public void Post(IEnumerable<TTask> tasks)
        {
            foreach (var t in tasks)
            {
                this.bufferBlock.Post(t);
            }
        }

        public Task WaitCompletionAsync()
        {
            return executeBlock.Completion;
        }
    }
}
