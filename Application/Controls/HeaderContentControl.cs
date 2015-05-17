using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JohnSmithDr.Application.Controls
{
    public class HeaderContentControl : ContentControl
    {
        #region public object Header

        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(object),
                typeof(HeaderContentControl),
                new PropertyMetadata(null));

        #endregion

        #region public DataTemplate HeaderTemplate

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                "HeaderTemplate",
                typeof(DataTemplate),
                typeof(HeaderContentControl),
                new PropertyMetadata(null));

        #endregion
    }
}
