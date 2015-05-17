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

        void GoBack<TResult>(ITaskContainer<TResult> taskContainer);

        void GoBack<TResult>(ITaskContainer<TResult> taskContainer, TResult result);

        void GoBack<TResult>(ITaskContainer<TResult> taskContainer, Exception error);
    }
}
