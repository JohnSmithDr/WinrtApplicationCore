using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace JohnSmithDr.Application.Actions
{
    public class ShowFlyoutAction : DependencyObject, IAction
    {
        #region IAction

        public object Execute(object sender, object parameter)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
            return true;
        }

        #endregion
    }
}
