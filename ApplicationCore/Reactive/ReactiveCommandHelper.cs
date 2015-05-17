using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace JohnSmithDr.ApplicationCore.Reactive
{
    public class ReactiveCommandHelper : IDisposable
    {
        public static ReactiveCommandHelper Create(Action execute, IObservable<bool> canExecute = null, Action<Exception> onError = null)
        {
            var cmd = ReactiveCommand.Create(canExecute);

            IDisposable ex = cmd.Subscribe(a =>
            {
                execute.Invoke();
            });

            IDisposable eh = null;
            if (onError != null)
            {
                eh = cmd.ThrownExceptions.Subscribe(e => onError.Invoke(e));
            }

            return new ReactiveCommandHelper(cmd, ex, eh);
        }

        public static ReactiveCommandHelper Create<T>(Action<T> execute, IObservable<bool> canExecute = null, Action<Exception> onError = null)
        {
            var cmd = ReactiveCommand.Create(canExecute);

            IDisposable ex = cmd.Where(a => a is T).Select(a => (T)a).Subscribe(a =>
            {
                execute.Invoke(a);
            });

            IDisposable eh = null;
            if (onError != null)
            {
                eh = cmd.ThrownExceptions.Subscribe(e => onError.Invoke(e));
            }

            return new ReactiveCommandHelper(cmd, ex, eh);
        }

        public static ReactiveCommandHelper CreateAsync(Func<Task> executeAsync, IObservable<bool> canExecute = null, Action<Exception> onError = null)
        {
            var cmd = ReactiveCommand.CreateAsyncTask(
                canExecute,
                async s =>
                {
                    await executeAsync.Invoke();
                });

            IDisposable eh = null;
            if (onError != null)
            {
                eh = cmd.ThrownExceptions.Subscribe(e => onError.Invoke(e));
            }

            return new ReactiveCommandHelper(cmd, null, eh);
        }

        public static ReactiveCommandHelper CreateAsync<T>(Func<T, Task> executeAsync, IObservable<bool> canExecute = null, Action<Exception> onError = null)
        {
            var cmd = ReactiveCommand.CreateAsyncTask(
                canExecute,
                async s =>
                {
                    if (s is T)
                    {
                        await executeAsync.Invoke((T)s);
                    }
                });

            IDisposable eh = null;
            if (onError != null)
            {
                eh = cmd.ThrownExceptions.Subscribe(e => onError.Invoke(e));
            }

            return new ReactiveCommandHelper(cmd, null, eh);
        }

        IReactiveCommand _Command;
        IDisposable _CommandExecuter;
        IDisposable _ExceptionHandler;

        private ReactiveCommandHelper(IReactiveCommand command, IDisposable executor = null, IDisposable exceptionHandler = null)
        {
            _Command = command;
            _CommandExecuter = executor;
            _ExceptionHandler = exceptionHandler;
        }

        public ICommand Command
        {
            get { return _Command; }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ExceptionHandler.TryDispose();
                _CommandExecuter.TryDispose();
                _Command.TryDispose();
            }
        }

        #endregion
    }
}
