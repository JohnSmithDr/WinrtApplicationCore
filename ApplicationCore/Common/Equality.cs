using System;
using System.Collections.Generic;

namespace JohnSmithDr.ApplicationCore
{
    public static class Equality
    {
        public static IEqualityComparer<TObj> Create<TObj, TVal>(Func<TObj, TVal> keySelector)
        {
            return new CommonEqualityComparer<TObj, TVal>(keySelector);
        }

        public static IEqualityComparer<TObj> Create<TObj, TVal>(Func<TObj, TVal> keySelector, IEqualityComparer<TVal> comparer)
        {
            return new CommonEqualityComparer<TObj, TVal>(keySelector, comparer);
        }

        class CommonEqualityComparer<TObj, TVal> : IEqualityComparer<TObj>
        {
            private Func<TObj, TVal> keySelector;
            private IEqualityComparer<TVal> comparer;

            public CommonEqualityComparer(Func<TObj, TVal> keySelector, IEqualityComparer<TVal> comparer)
            {
                this.keySelector = keySelector;
                this.comparer = comparer;
            }

            public CommonEqualityComparer(Func<TObj, TVal> keySelector)
                : this(keySelector, EqualityComparer<TVal>.Default)
            {
            
            }

            public bool Equals(TObj x, TObj y)
            {
                if (x == null || y == null) return false;
                return comparer.Equals(keySelector(x), keySelector(y));
            }

            public int GetHashCode(TObj obj)
            {
                if (obj == null) return 0;
                return comparer.GetHashCode(keySelector(obj));
            }
        }
    }
}
