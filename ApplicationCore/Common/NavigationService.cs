using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JohnSmithDr.ApplicationCore
{
    public class NavigationService : IViewNavigation
    {
        readonly Stack<Frame> frameStack = new Stack<Frame>();

        public NavigationService(string viewLocation)
        {
            ViewLocation = viewLocation;
        }

        public string ViewLocation { get; private set; }

        private Frame CreateFrame()
        {
            var frame = new Frame();
            SuspensionManager.RegisterFrame(frame, "HyperFrame" + frameStack.Count);
            frame.CacheSize = 0;
            return frame;
        }

        public void DestroyFrame(Frame frame)
        {
            SuspensionManager.UnregisterFrame(frame);
        }

        private bool Navigate(Frame frame, Type sourcePageType, object parameter)
        {
            return frame.Navigate(sourcePageType, parameter);
        }

        private Task<TResult> NavigateAsync<TResult>(Type sourcePageType, object parameter)
        {
            var taskSource = new TaskCompletionSource<TResult>();
            var args = new TaskNavigationArgs(taskSource, parameter);

            frameStack.Push(CurrentFrame);

            var newFrame = CreateFrame();
            Window.Current.Content = newFrame;

            if (!Navigate(newFrame, sourcePageType, args))
            {
                taskSource.TrySetException(new Exception("Navigation failed"));
            }
            return taskSource.Task;
        }

        private Type GetViewType(string viewName)
        {
            string className = ViewLocation + "." + viewName;
            return Type.GetType(className);
        }

        private static Frame CurrentFrame
        {
            get { return Window.Current.Content as Frame; }
        }

        #region IViewLocator

        public bool CanGoBack
        {
            get
            {
                var frame = CurrentFrame;
                return (frame != null) && (frame.CanGoBack || frameStack.Count > 0);
            }
        }

        public bool GoBack()
        {
            var frame = CurrentFrame;

            if (frame != null)
            {
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                    return true;
                }
                else if (frameStack.Count > 0)
                {
                    var prevFrame = frameStack.Pop();
                    Window.Current.Content = prevFrame;
                    DestroyFrame(frame);
                    return true;
                }
            }
            return false;
        }

        public bool Navigate(string viewName)
        {
            return Navigate(CurrentFrame, GetViewType(viewName), null);
        }

        public bool Navigate(string viewName, object parameter)
        {
            return Navigate(CurrentFrame, GetViewType(viewName), parameter);
        }

        public Task<TResult> NavigateAsync<TResult>(string viewName)
        {
            return NavigateAsync<TResult>(GetViewType(viewName), null);
        }

        public Task<TResult> NavigateAsync<TResult>(string viewName, object parameter)
        {
            return NavigateAsync<TResult>(GetViewType(viewName), parameter);
        }

        public void GoBack<TResult>(ITaskContainer<TResult> taskContainer)
        {
            GoBack();

            Task.Delay(250).ContinueWith(t =>
            {
                taskContainer.TaskSource.TrySetCanceled();
            });
        }

        public void GoBack<TResult>(ITaskContainer<TResult> taskContainer, TResult result)
        {
            GoBack();

            Task.Delay(250).ContinueWith(t =>
            {
                taskContainer.TaskSource.TrySetResult(result);
            });
        }

        public void GoBack<TResult>(ITaskContainer<TResult> taskContainer, Exception error)
        {
            GoBack();

            Task.Delay(250).ContinueWith(t =>
            {
                taskContainer.TaskSource.TrySetException(error);
            });
        }

        #endregion
    }

}
