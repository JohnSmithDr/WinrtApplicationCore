using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ReactiveUI;

namespace JohnSmithDr.ApplicationCore.Reactive
{
    public static class ReactiveExtensions
    {
        public static void ShowProgressIndicator(this IReactiveObject obj)
        {
            MessageBus.Current.SendMessage(
                ProgressIndicatorState.ShowIndicator, "ProgressIndicatorState");
        }

        public static void HideProgressIndicator(this IReactiveObject obj)
        {
            MessageBus.Current.SendMessage(
                ProgressIndicatorState.HideIndicator, "ProgressIndicatorState");
        }

        public static void ToastMessage(this IReactiveObject obj, string message)
        {
            MessageBus.Current.SendMessage(message, "ToastMessage");
        }

        public static void NotifyUserError(this IReactiveObject obj, UserError error)
        {
            MessageBus.Current.SendMessage(error, "NotifyUserError");
        } 

        public static TRet RaiseAndSet<TObj, TRet>(this TObj source, ref TRet field, TRet newValue, [CallerMemberName] string propertyName = null)
            where TObj : IReactiveObject
        {
            field = newValue;
            source.RaisePropertyChanged(propertyName);
            return newValue;
        }

        public static void RaisePropertyChanged<TObj, TRet>(this TObj source, Expression<Func<TObj, TRet>> property)
            where TObj : IReactiveObject
        {
            var member = property.Body.GetMemberInfo();
            if (member.Name != null)
            {
                source.RaisePropertyChanged<TObj>(member.Name);
            }
        }

        public static AsyncPropertyHelper<T> CreateAsyncProperty<TObj, T>(
            this TObj source, Expression<Func<TObj, T>> property, Func<Task<T>> valueFactory, T initialValue = default(T), int millisecondsDelay = 0)
            where TObj : IReactiveObject
        {
            var helper = new AsyncPropertyHelper<T>(
                valueFactory,
                t => RaisePropertyChanged(source, property),
                initialValue,
                millisecondsDelay);

            return helper;
        }

        public static ReactiveCommandHelper ToCommand(this IObservable<bool> canExecute, Action execute)
        {
            return ReactiveCommandHelper.Create(execute, canExecute);
        }

        public static ReactiveCommandHelper ToCommand<T>(this IObservable<bool> canExecute, Action<T> execute)
        {
            return ReactiveCommandHelper.Create(execute, canExecute);
        }

        public static ReactiveCommandHelper ToAsyncCommand(this IObservable<bool> canExecute, Func<Task> executeAsync)
        {
            return ReactiveCommandHelper.CreateAsync(executeAsync, canExecute);
        }

        public static ReactiveCommandHelper ToAsyncCommand<T>(this IObservable<bool> canExecute, Func<T, Task> executeAsync)
        {
            return ReactiveCommandHelper.CreateAsync(executeAsync, canExecute);
        }
    }
}
