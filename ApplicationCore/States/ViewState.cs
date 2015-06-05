namespace JohnSmithDr.ApplicationCore
{
    public interface IViewState
    {

    }

    public class ViewState<T> : IViewState
    {
        public ViewState(T value)
        {
            this.Value = value;
        }

        public T Value { get; protected set; }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ViewState<T>)
            {
                return this.Value.Equals((obj as ViewState<T>).Value);
            }

            return base.Equals(obj);
        }
    }
}
