using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace TestsUniversal.ViewModel.Commands
{
    public sealed class CommandActionWithEArgs : TriggerAction<UIElement>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            nameof(Command), typeof(ICommand), typeof(CommandActionWithEArgs), null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);           
            set => SetValue(CommandProperty, value);
        }

        protected override void Invoke(Object parameter)
        {
            if (AssociatedObject != null)
            {
                ICommand command = Command;
                if ((command != null) && command.CanExecute(parameter))
                    command.Execute(parameter);
            }
        }
    }
}
