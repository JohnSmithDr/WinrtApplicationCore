using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace JohnSmithDr.ApplicationCore.Reactive
{
    public class ReactiveListViewModel<T> : ReactiveObject, IBackCommandOverride
    {
        public ReactiveListViewModel(IViewNavigation navigation)
        {
            this.Navigation = navigation;
            this.Items = new ReactiveList<T>();
            this.SelectedItems = new ReactiveList<T>();
            this.ViewState = ItemsViewState.ItemsViewDefault;

            this.Refresh = this
                .WhenAnyValue(x => x.RefreshEnabled)
                .ToCommand(RefreshOnExecute)
                .Command;

            this.Select = this
                .WhenAnyValue(x => x.IsSelectionEnabled, x => x.Items.IsEmpty, (a, b) => !(a || b))
                .ToCommand(() =>
                {
#if WINDOWS_PHONE_APP
                    IsSelectionEnabled = true;
#endif
                })
                .Command;

            this.SelectAll = this
                .WhenAnyValue(x => x.IsSelectionEnabled, x => x.Items.IsEmpty, (a, b) => a && !b)
                .ToCommand(() =>
                {
                    SelectedItems.Clear();
                    SelectedItems.AddRange(Items);
                })
                .Command;

            this.ClearSelection = this
                .WhenAnyValue(x => x.IsSelectionEnabled, x => x.SelectedItems.IsEmpty, (a, b) => a && !b)
                .ToCommand(() =>
                {
                    SelectedItems.Clear();
                })
                .Command;

            this.BackCommand = new RelayCommand(OnBackCommandExecute, OnBackCommandCanExecute);
            this.ItemClick = new RelayCommand<T>(ItemClickOnExecute);

            this.WhenAnyValue(x => x.IsSelectionEnabled)
                .Subscribe(s =>
                {
                    if (s) this.ViewState = ItemsViewState.ItemsViewSelection;
                    else this.ViewState = ItemsViewState.ItemsViewDefault;
                });

            this.WhenAnyValue(x => x.IsItemsLoaded, x => x.Items.IsEmpty, (a, b) => a && b)
                .ToProperty(this, x => x.IsItemsEmpty, out _IsItemsEmpty, false);
        }

        protected virtual void RefreshOnExecute()
        {

        }

        protected virtual void ItemClickOnExecute(T item)
        {

        }

        public ICommand Refresh { get; set; }

        public ICommand Select { get; set; }

        public ICommand SelectAll { get; set; }

        public ICommand ClearSelection { get; set; }

        public ICommand Delete { get; set; }

        public ICommand ItemClick { get; set; }

        public IReactiveList<T> Items { get; protected set; }

        public IReactiveList<T> SelectedItems { get; protected set; }

        public IViewNavigation Navigation { get; protected set; }

        #region public string Title

        public string Title
        {
            get { return _Title; }
            set { this.RaiseAndSetIfChanged(ref _Title, value); }
        }

        private string _Title;

        #endregion

        #region public string Header

        public string Header
        {
            get { return _Header; }
            set { this.RaiseAndSetIfChanged(ref _Header, value); }
        }

        private string _Header;

        #endregion

        #region public IViewState ViewState

        public IViewState ViewState
        {
            get { return _ViewState; }
            set { this.RaiseAndSetIfChanged(ref _ViewState, value); }
        }

        private IViewState _ViewState;

        #endregion

        #region public bool IsItemsLoaded

        public bool IsItemsLoaded
        {
            get { return _IsItemsLoaded; }
            protected set { this.RaiseAndSetIfChanged(ref _IsItemsLoaded, value); }
        }

        private bool _IsItemsLoaded;

        #endregion

        #region public bool IsItemsEmpty

        public bool IsItemsEmpty
        {
            get { return _IsItemsEmpty.Value; }
        }

        private ObservableAsPropertyHelper<bool> _IsItemsEmpty;

        #endregion

        #region public bool IsSelectionEnabled

        public bool IsSelectionEnabled
        {
            get { return _IsSelectionEnabled; }
            set { this.RaiseAndSetIfChanged(ref _IsSelectionEnabled, value); }
        }

        private bool _IsSelectionEnabled;

        #endregion

        #region public bool RefreshEnabled

        public bool RefreshEnabled
        {
            get { return _RefreshEnabled; }
            set { this.RaiseAndSetIfChanged(ref _RefreshEnabled, value); }
        }

        private bool _RefreshEnabled;

        #endregion

        #region IBackCommandOverride

        #region public ICommand BackCommand

        public ICommand BackCommand { get; set; }

        protected virtual void OnBackCommandExecute()
        {
            if (this.IsSelectionEnabled)
            {
                this.IsSelectionEnabled = false;
            }
            else
            {
                Navigation.GoBack();
            }
        }

        protected virtual bool OnBackCommandCanExecute()
        {
            return (IsSelectionEnabled || Navigation.CanGoBack);
        }

        #endregion

        #endregion
    }
}
