using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Windows.UI.Xaml
{
    public static class FrameworkElementExtensions
    {
        public static void OnLoaded(this FrameworkElement source, Action action)
        {
            Debug.Assert(action != null, "action should not be null");

            RoutedEventHandler eh = null;

            eh = new RoutedEventHandler((s, e) =>
            {
                action.Invoke();
                source.Loaded -= eh;
            });

            source.Loaded += eh;
        }

        public static void OnUnloaded(this FrameworkElement source, Action action)
        {
            Debug.Assert(action != null, "action should not be null");

            RoutedEventHandler eh = null;

            eh = new RoutedEventHandler((s, e) =>
            {
                action.Invoke();
                source.Unloaded -= eh;
            });

            source.Unloaded += eh;
        }

        public static void BeginInvoke(this FrameworkElement source, Action action)
        {
            Debug.Assert(action != null, "action should not be null");

            var r = source.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(action));
        }

        public static void BeginInvokeNow(this FrameworkElement source, Action action)
        {
            Debug.Assert(action != null, "action should not be null");

            var r = source.Dispatcher.RunAsync(CoreDispatcherPriority.High, new DispatchedHandler(action));
        }

        public static void DelayInvoke(this FrameworkElement source, TimeSpan delay, Action action)
        {
            Task.Delay(delay).
                ContinueWith(t => BeginInvoke(source, action)).
                ConfigureAwait(false);
        }
    }
}


