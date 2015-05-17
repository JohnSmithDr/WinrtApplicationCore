using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace JohnSmithDr.Application.Behaviors
{
    public class SynchronizeSelectedItemsBehavior : DependencyObject, IBehavior
    {
        private ListViewBase _view;
        private INotifyCollectionChanged _data;
        private bool _synchronizing;

        #region public object SelectedItems

        public object SelectedItems
        {
            get { return (object)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(
                "SelectedItems",
                typeof(object),
                typeof(SynchronizeSelectedItemsBehavior),
                new PropertyMetadata(null, SelectedItemsPropertyChanged));

        private static void SelectedItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = d as SynchronizeSelectedItemsBehavior;
            obj.SelectedItemsPropertyChanged(e.OldValue, e.NewValue);
        }

        private void SelectedItemsPropertyChanged(object oldValue, object newValue)
        {
            if (_data != null)
            {
                _data.CollectionChanged -= OnSourceCollectionChanged;
                _data = null;
            }

            if (newValue is INotifyCollectionChanged)
            {
                _data = newValue as INotifyCollectionChanged;
                _data.CollectionChanged += OnSourceCollectionChanged;
            }
        }

        #endregion

        #region public bool EnableSynchronization

        public bool EnableSynchronization
        {
            get { return (bool)GetValue(EnableSynchronizationProperty); }
            set { SetValue(EnableSynchronizationProperty, value); }
        }

        public static readonly DependencyProperty EnableSynchronizationProperty =
            DependencyProperty.Register(
                "EnableSynchronization",
                typeof(bool),
                typeof(SynchronizeSelectedItemsBehavior),
                new PropertyMetadata(true));

        #endregion

        #region IBehavior

        public DependencyObject AssociatedObject
        {
            get { return _view; }
        }

        public void Attach(DependencyObject associatedObject)
        {
            if (associatedObject is ListViewBase == false)
            {
                return;
            }

            _view = associatedObject as ListViewBase;
            _view.SelectionChanged += OnListViewSelectionChanged;
        }

        public void Detach()
        {
            if (_data != null)
            {
                _data.CollectionChanged -= OnSourceCollectionChanged;
            }

            _view.SelectionChanged -= OnListViewSelectionChanged;
            _view = null;
        }

        #endregion

        private void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_synchronizing)
            {
                return;
            }

            _synchronizing = true;

            if (_data is IList)
            {
                var list = _data as IList;
                e.AddedItems.ForEach(i => list.Add(i));
                e.RemovedItems.ForEach(i => list.Remove(i));
            }

            _synchronizing = false;
        }

        private void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_synchronizing)
            {
                return;
            }

            _synchronizing = true;

            try
            {
                var action = e.Action;
                switch (action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var i in e.NewItems)
                        {
                            _view.SelectedItems.Add(i);
                        }
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var i in e.OldItems)
                        {
                            _view.SelectedItems.Remove(i);
                        }
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        if (_view.SelectedItems.Count > 0)
                        {
                            _view.SelectedItems.Clear();
                        }
                        if (_data is IEnumerable)
                        {
                            foreach (var i in _data as IEnumerable)
                            {
                                _view.SelectedItems.Add(i);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                
            }

            _synchronizing = false;
        }
    }
}
