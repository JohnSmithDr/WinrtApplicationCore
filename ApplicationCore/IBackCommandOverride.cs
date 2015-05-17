using System.Windows.Input;

namespace JohnSmithDr.ApplicationCore
{
    public interface IBackCommandOverride
    {
        ICommand BackCommand { get; }
    }
}
