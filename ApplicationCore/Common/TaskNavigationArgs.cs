namespace JohnSmithDr.ApplicationCore
{
    public class TaskNavigationArgs
    {
        public TaskNavigationArgs(object taskSource, object parameter)
        {
            this.TaskSource = taskSource;
            this.Parameter = parameter;
        }

        public object TaskSource { get; private set; }

        public object Parameter { get; private set; }
    }
}
