using System;
using System.Windows.Input;

namespace TestsUniversal.ViewModel.Commands
{
    public sealed class UniversalCommand : ICommand
    {
        public UniversalCommand(Predicate<Object> canExecuteDelegate, Action<Object> executeDelegate)
        {
            CanExecuteDelegate = canExecuteDelegate;
            ExecuteDelegate = executeDelegate;
        }

        public UniversalCommand(Action<Object> executeDelegate)
        {
            ExecuteDelegate = executeDelegate;
        }

        public Predicate<Object> CanExecuteDelegate { get; }
        public Action<Object> ExecuteDelegate { get; }

        public Boolean CanExecute(Object parameter) => (CanExecuteDelegate != null) ? 
                                                       CanExecuteDelegate(parameter) : 
                                                       true;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(Object parameter) => ExecuteDelegate?.Invoke(parameter);
    }
}
