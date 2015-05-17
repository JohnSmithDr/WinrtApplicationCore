using System;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;

namespace JohnSmithDr.ApplicationCore.Reactive
{
    public interface IReactiveGroupedList<TKey, TElement> :
        IReactiveList<IReactiveGrouping<TKey, TElement>>
    {
        int ElementCount { get; }

        IObservable<int> ElementCountChanged { get; }

        IEnumerable<TElement> Elements { get; }

        bool IsElementEmpty { get; }

        IObservable<bool> IsElementEmptyChanged { get; }

        void AddToGroup(TKey groupKey, TElement item);

        void AddRangeToGroup(TKey groupKey, IEnumerable<TElement> items);

        IReactiveGrouping<TKey, TElement> GetGroup(TKey groupKey);

        bool RemoveElement(TElement item);

        bool RemoveFromGroup(TKey groupKey, TElement item);

        void RemoveAllFromGroup(TKey groupKey, IEnumerable<TElement> items);
    }

    public interface IReactiveGrouping<TKey, TElement> :
        IGrouping<TKey, TElement>,
        IReactiveList<TElement>
    {

    }

    public class ReactiveGroupedList<TKey, TElement> :
        ReactiveList<IReactiveGrouping<TKey, TElement>>,
        IReactiveList<IReactiveGrouping<TKey, TElement>>,
        IReactiveGroupedList<TKey, TElement>
    {
        public ReactiveGroupedList()
        {
            var ob = new Dictionary<IReactiveGrouping<TKey, TElement>, Tuple<IDisposable, IDisposable>>();

            this.ItemsAdded.Subscribe(i =>
            {
                var t1 = i.IsEmptyChanged.Subscribe(s => this.RaisePropertyChanged(x => x.IsElementEmpty));
                var t2 = i.CountChanged.Subscribe(s => this.RaisePropertyChanged(x => x.ElementCount));
                ob[i] = new Tuple<IDisposable, IDisposable>(t1, t2);

                this.RaisePropertyChanged(x => x.IsElementEmpty);
                this.RaisePropertyChanged(x => x.ElementCount);
            });

            this.ItemsRemoved.Subscribe(i =>
            {
                var t = ob[i];
                t.Item1.TryDispose();
                t.Item2.TryDispose();
                ob.Remove(i);

                this.RaisePropertyChanged(x => x.IsElementEmpty);
                this.RaisePropertyChanged(x => x.ElementCount);
            });
        }

        #region IReactiveGroupedList

        public int ElementCount
        {
            get { return this.Sum(i => i.Count); }
        }

        public IObservable<int> ElementCountChanged
        {
            get { return this.ObservableForProperty(x => x.ElementCount, s => s); }
        }

        public IEnumerable<TElement> Elements
        {
            get { return this.SelectMany(s => s.AsEnumerable()); }
        }

        public bool IsElementEmpty
        {
            get { return this.All(i => i.IsEmpty); }
        }

        public IObservable<bool> IsElementEmptyChanged
        {
            get { return this.ObservableForProperty(x => x.IsElementEmpty, s => s); }
        }

        public void AddToGroup(TKey groupKey, TElement item)
        {
            var groups = this.Where(i => i.Key.Equals(groupKey)).ToArray();

            if (groups.Length > 0)
            {
                groups.ForEach(i =>
                {
                    i.Add(item);
                });
            }
            else
            {
                var group = new ReactiveGroupItem<TKey, TElement>(groupKey);
                group.Add(item);
                this.Add(group);
            }
        }

        public void AddRangeToGroup(TKey groupKey, IEnumerable<TElement> items)
        {
            var groups = this.Where(i => i.Key.Equals(groupKey)).ToArray();

            if (groups.Length > 0)
            {
                groups.ForEach(i =>
                {
                    i.AddRange(items);
                });
            }
            else
            {
                var group = new ReactiveGroupItem<TKey, TElement>(groupKey, items);
                this.Add(group);
            }
        }

        public IReactiveGrouping<TKey, TElement> GetGroup(TKey groupKey)
        {
            return this.FirstOrDefault(i => i.Key.Equals(groupKey));
        }

        public bool RemoveElement(TElement item)
        {
            var removed = false;

            foreach (var group in this)
            {
                removed = group.Remove(item) || removed;
            }
            return removed;
        }

        public bool RemoveFromGroup(TKey groupKey, TElement item)
        {
            var removed = false;
            var groups = this.Where(i => i.Key.Equals(groupKey)).ToArray();

            if (groups.Length > 0)
            {
                groups.ForEach(i =>
                {
                    removed = i.Remove(item) || removed;
                });
            }
            return removed;
        }

        public void RemoveAllFromGroup(TKey groupKey, IEnumerable<TElement> items)
        {
            var groups = this.Where(i => i.Key.Equals(groupKey)).ToArray();

            if (groups.Length > 0)
            {
                groups.ForEach(i =>
                {
                    i.RemoveAll(items);
                });
            }
        }

        #endregion
    }

    public class ReactiveGroupItem<TKey, TElement> : ReactiveList<TElement>, IReactiveGrouping<TKey, TElement>
    {
        public ReactiveGroupItem(TKey key)
            : base()
        {
            this.Key = key;
        }

        public ReactiveGroupItem(TKey key, IEnumerable<TElement> initialContents)
            : base(initialContents)
        {
            this.Key = key;
        }

        public TKey Key { get; private set; }
    }
}
