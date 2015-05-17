using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace JohnSmithDr.ApplicationCore.Reactive
{
    public class AsyncPropertyHelper<T> : IObservable<T>, IDisposable
    {
        private Func<Task<T>> valueFactory;
        private Action<T> onChanged;
        private T value;
        private bool eval;
        private Subject<T> valueSubject;
        private IDisposable valueChangedObserver;
        private int delay;

        public AsyncPropertyHelper(Func<Task<T>> valueFactory, Action<T> onChanged, T initialValue = default(T), int millisecondsDelay = 0)
        {
            this.delay = millisecondsDelay;
            this.onChanged = onChanged;
            this.valueFactory = valueFactory;
            this.value = initialValue;
            this.valueSubject = new Subject<T>();

            this.valueChangedObserver = Observable.FromEventPattern(this, "ValueChanged")
                .ObserveOnDispatcher()
                .Subscribe(s =>
                {
                    if (this.onChanged != null)
                    {
                        this.onChanged.Invoke(value);
                    }
                });
        }

        public T Value
        {
            get
            {
                if (!eval)
                {
                    Invalid();
                }
                return value;
            }
        }

        public void Invalid()
        {
            var run = Task.Run(async () =>
            {
                if (delay > 0)
                {
                    await Task.Delay(delay);
                }

                value = await Task.Run(valueFactory);
                eval = true;
                valueSubject.OnNext(value);

                if (ValueChanged != null)
                {
                    ValueChanged.Invoke(this, EventArgs.Empty);
                }
            });
        }

        #region public event ValueChanged

        public event EventHandler ValueChanged;

        private void OnValueChanged()
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion

        #region IObservable<T>

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return valueSubject.Subscribe(observer);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                valueChangedObserver.TryDispose();
                valueSubject.TryDispose();
            }
        }

        #endregion
    }
}