using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;

namespace JohnSmithDr.ApplicationCore
{
#if WINDOWS_PHONE_APP

    public static class StatusBarHelper
    {
        public static TimeSpan DefaultDelay
        {
            get { return TimeSpan.FromSeconds(5); }
        }

        public static Color AccentColor
        {
            get
            {
                var brush = Windows.UI.Xaml.Application.Current.Resources["PhoneAccentBrush"] as SolidColorBrush;
                return brush != null ? brush.Color : Color.FromArgb(0, 0, 0, 0);
            }
        }

        public static void Show()
        {
            Show(string.Empty, null);
        }

        public static void Show(string text, double? progress)
        {
            text = text ?? string.Empty;
            var bar = StatusBar.GetForCurrentView();
            bar.ProgressIndicator.Text = text;
            bar.ProgressIndicator.ProgressValue = progress;
            bar.BackgroundColor = null;
            bar.BackgroundOpacity = 0;
            var t = bar.ProgressIndicator.ShowAsync();
        }

        public static void Hide()
        {
            var bar = StatusBar.GetForCurrentView();
            bar.BackgroundOpacity = 0;
            bar.ProgressIndicator.ProgressValue = null;
            bar.ProgressIndicator.Text = string.Empty;
            var t = bar.ProgressIndicator.HideAsync();
        }

        public static void Post(string message, Color? color, TimeSpan delay)
        {
            PostAsync(message, color, delay).ConfigureAwait(false);
        }

        public static void Post(string message, Color? color)
        {
            Post(message, color, DefaultDelay);
        }

        public static void Post(string message, TimeSpan delay)
        {
            Post(message, AccentColor, delay);
        }

        public static void Post(string message)
        {
            Post(message, AccentColor, DefaultDelay);
        }

        public static Task PostAsync(string message, Color? color, TimeSpan delay)
        {
            var id = ++eventId;
            return PostAsync(id, message, color, delay);
        }

        private static async Task PostAsync(ulong id, string message, Color? color, TimeSpan delay)
        {
            if (!String.IsNullOrEmpty(message))
            {
                var bar = StatusBar.GetForCurrentView();
                bar.BackgroundColor = color;
                bar.BackgroundOpacity = 1;
                bar.ProgressIndicator.ProgressValue = 0;
                bar.ProgressIndicator.Text = message;
                await bar.ProgressIndicator.ShowAsync();
                await Task.Delay(delay);

                if (id == eventId)
                {
                    await bar.ProgressIndicator.HideAsync();
                    bar.BackgroundOpacity = 0;
                    bar.ProgressIndicator.ProgressValue = null;
                    bar.ProgressIndicator.Text = string.Empty;
                }
            }
        }

        static volatile uint eventId;
    }

#endif

}
