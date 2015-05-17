using System.Linq;

namespace Windows.UI.Xaml.Controls
{
    public static class ItemsControlExtensions
    {
        /// <summary>
        /// Get the ScrollViewer from visual tree.
        /// </summary>
        public static ScrollViewer GetScrollViewer(this ItemsControl control)
        {
            return control.GetDescendants<ScrollViewer>().FirstOrDefault();
        }

        /// <summary>
        /// Get the horizontal offset of ScrollViewer.
        /// </summary>
        public static double GetScrollHorizontalOffset(this ItemsControl control)
        {
            var scroll = GetScrollViewer(control);
            return (scroll != null) ? scroll.HorizontalOffset : 0;
        }

        /// <summary>
        /// Get the vertical offset of ScrollViewer.
        /// </summary>
        public static double GetScrollVerticalOffset(this ItemsControl control)
        {
            var scroll = GetScrollViewer(control);
            return (scroll != null) ? scroll.VerticalOffset : 0;
        }

        /// <summary>
        /// Change the horizontal offset of ScrollViewer.
        /// </summary>
        public static void ScrollToHorizontalOffset(this ItemsControl control, double offset)
        {
            var scroll = GetScrollViewer(control);
            if (scroll != null) scroll.ChangeView(offset, null, null, true);
        }

        /// <summary>
        /// Change the vertical offset of ScrollViewer.
        /// </summary>
        public static void ScrollToVerticalOffset(this ItemsControl control, double offset)
        {
            var scroll = GetScrollViewer(control);
            if (scroll != null) scroll.ChangeView(null, offset, null, true);
        }
    }
}
