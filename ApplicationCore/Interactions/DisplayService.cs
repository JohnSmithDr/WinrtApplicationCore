using System.Diagnostics;
using Windows.Graphics.Display;
using Windows.System.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JohnSmithDr.PlayerPlus.Interactions
{
    public static class DisplayService
    {
        #region KeepScreenOn

        public static bool GetKeepScreenOn(DependencyObject obj)
        {
            return (bool)obj.GetValue(KeepScreenOnProperty);
        }

        public static void SetKeepScreenOn(DependencyObject obj, bool value)
        {
            obj.SetValue(KeepScreenOnProperty, value);
        }

        public static readonly DependencyProperty KeepScreenOnProperty =
            DependencyProperty.RegisterAttached(
                "KeepScreenOn",
                typeof(bool),
                typeof(Page),
                new PropertyMetadata(DependencyProperty.UnsetValue, OnKeepScreenOnPropertyChanged));

        private static void OnKeepScreenOnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as Page;
            var value = (bool)e.NewValue;

            if (value)
            {
                var display = new DisplayRequest();

                display.RequestActive();

                page.OnUnloaded(() =>
                {
                    display.RequestRelease();
                });
            }
        }

        #endregion

        #region DisplayOrientation

        public static DisplayOrientations GetDisplayOrientation(DependencyObject obj)
        {
            return (DisplayOrientations)obj.GetValue(DisplayOrientationProperty);
        }

        public static void SetDisplayOrientation(DependencyObject obj, DisplayOrientations value)
        {
            obj.SetValue(DisplayOrientationProperty, value);
        }

        public static readonly DependencyProperty DisplayOrientationProperty =
            DependencyProperty.RegisterAttached(
                "DisplayOrientation",
                typeof(DisplayOrientations),
                typeof(Page),
                new PropertyMetadata(DependencyProperty.UnsetValue, OnDisplayOrientationPropertyChanged));

        private static void OnDisplayOrientationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as Page;
            var value = (DisplayOrientations)e.NewValue;

            var orientation = DisplayInformation.AutoRotationPreferences;

            DisplayInformation.AutoRotationPreferences = value;

            page.OnUnloaded(() =>
            {
                DisplayInformation.AutoRotationPreferences = orientation;
            });
        }

        #endregion

#if WINDOWS_PHONE_APP

        #region BoundsMode

        public static ApplicationViewBoundsMode GetBoundsMode(DependencyObject obj)
        {
            return (ApplicationViewBoundsMode)obj.GetValue(BoundsModeProperty);
        }

        public static void SetBoundsMode(DependencyObject obj, ApplicationViewBoundsMode value)
        {
            obj.SetValue(BoundsModeProperty, value);
        }

        public static readonly DependencyProperty BoundsModeProperty =
            DependencyProperty.RegisterAttached(
                "BoundsMode",
                typeof(ApplicationViewBoundsMode),
                typeof(Page),
                new PropertyMetadata(DependencyProperty.UnsetValue, OnBoundsModePropertyChanged));

        private static void OnBoundsModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var page = d as Page;
            var value = (ApplicationViewBoundsMode)e.NewValue;

            var view = ApplicationView.GetForCurrentView();
            var mode = view.DesiredBoundsMode;

            view.SetDesiredBoundsMode(value);

            page.OnUnloaded(() =>
            {
                view.SetDesiredBoundsMode(mode);
            });
        }

        #endregion

#endif
    }
}
