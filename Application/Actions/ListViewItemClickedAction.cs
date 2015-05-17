using System.Windows.Input;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JohnSmithDr.Application.Actions
{
    public class ListViewItemClickedAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            var s = sender as ListViewBase;
            var e = parameter as ItemClickEventArgs;
            var d = s.DataContext;
            var i = e.ClickedItem;

            if (s.SelectionMode == ListViewSelectionMode.Multiple)
            {
                ToggleSelection(s, i);
                return true;
            }
            else
            {
                if (this.Command != null)
                {
                    if (this.WrapDataContext)
                    {
                        var a = new ItemClickedArgs { Context = d, ClickedItem = i };
                        if (this.Command.CanExecute(a))
                        {
                            this.Command.Execute(a);
                            return true;
                        }
                    }
                    else if (this.Command.CanExecute(i))
                    {
                        this.Command.Execute(i);
                        return true;
                    }
                }
                return false;
            }
        }

        private void ToggleSelection(ListViewBase list, object item)
        {
            var selected = list.SelectedItems;
            if (selected.Contains(item)) selected.Remove(item); else selected.Add(item);
        }

        #region public ICommand Command

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                "Command",
                typeof(ICommand),
                typeof(ListViewItemClickedAction),
                new PropertyMetadata(null));

        #endregion

        #region public bool WrapDataContext

        public bool WrapDataContext
        {
            get { return (bool)GetValue(WrapDataContextProperty); }
            set { SetValue(WrapDataContextProperty, value); }
        }

        public static readonly DependencyProperty WrapDataContextProperty =
            DependencyProperty.Register(
                "WrapDataContext",
                typeof(bool),
                typeof(ListViewItemClickedAction),
                new PropertyMetadata(false));

        #endregion
    }

    public class ItemClickedArgs
    {
        public object Context { get; internal set; }

        public object ClickedItem { get; internal set; }
    }
}
