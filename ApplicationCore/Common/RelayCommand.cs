using System;
using System.Windows.Input;

namespace JohnSmithDr.ApplicationCore
{
    public class RelayCommand : ICommand, IRelayCommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

    public class RelayCommand<TParameter> : ICommand, IRelayCommand
    {
        private readonly Action<TParameter> _execute;
        private readonly Func<TParameter, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<TParameter> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (parameter is TParameter)
                return _canExecute == null ? true : _canExecute((TParameter)parameter);
            return false;
        }

        public void Execute(object parameter)
        {
            if (parameter is TParameter)
                _execute((TParameter)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }

    public interface IRelayCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}