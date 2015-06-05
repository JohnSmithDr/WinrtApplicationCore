using System;

namespace JohnSmithDr.ApplicationCore
{
    //public enum ItemsViewState
    //{
    //    ItemsViewDefault,
    //    ItemsViewSelection
    //}

    public class ItemsViewState : IViewState
    {
        static readonly IViewState _itemsViewDefault = new ViewState<String>("ItemsViewDefault");
        static readonly IViewState _itemsViewSelection = new ViewState<String>("ItemsViewSelection");

        public static IViewState ItemsViewDefault
        {
            get { return _itemsViewDefault; }
        }

        public static IViewState ItemsViewSelection
        {
            get { return _itemsViewSelection; }
        }
    }
}
