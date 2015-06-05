using System;
using System.Threading.Tasks;

namespace JohnSmithDr.ApplicationCore
{
    public interface IViewNavigation
    {
        bool CanGoBack { get; }

        bool GoBack();

        bool Navigate(string viewName);

        bool Navigate(string viewName, object parameter);

        Task<TResult> NavigateAsync<TResult>(string viewName);

        Task<TResult> NavigateAsync<TResult>(string viewName, object parameter);

        void GoBack<TResult>(ITaskContainer<TResult> taskContainer, TResult result);

#if WINDOWS_PHONE_APP

        Task<bool> ShowContentDialogAsync(string viewName, object parameter);

        Task<TResult> ShowContentDialogAsync<TParam, TResult>(string viewName, TParam parameter, Func<bool, TParam, TResult> resultSelector);
#endif
    }

    public interface IViewHost
    {
        IViewNavigation ViewNavigation { get; }
    }
}
