using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace JohnSmithDr.ApplicationCore.Interactions
{
    public static class FlyoutService
    {
        #region IsOpen

        public static bool GetIsOpen(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsOpenProperty);
        }

        public static void SetIsOpen(DependencyObject obj, bool value)
        {
            obj.SetValue(IsOpenProperty, value);
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.RegisterAttached(
                "IsOpen",
                typeof(bool),
                typeof(FlyoutService),
                new PropertyMetadata(DependencyProperty.UnsetValue, OnIsOpenPropertyChanged));

        private static void OnIsOpenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flyout = d as FlyoutBase;

            if (flyout == null)
                return;

            if ((bool)e.NewValue)
            {
                flyout.Closed += OnFlyoutClosed;
                flyout.ShowAt(GetTarget(d));
            }
            else
            {
                flyout.Closed -= OnFlyoutClosed;
                flyout.Hide();
            }
        }

        private static void OnFlyoutClosed(object sender, object e)
        {
            SetIsOpen(sender as DependencyObject, false);
        }

        #endregion

        #region Target

        public static FrameworkElement GetTarget(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(TargetProperty);
        }

        public static void SetTarget(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(TargetProperty, value);
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.RegisterAttached(
                "Target", 
                typeof(FrameworkElement), 
                typeof(FlyoutService), 
                new PropertyMetadata(null));
        
        #endregion
    }
}
