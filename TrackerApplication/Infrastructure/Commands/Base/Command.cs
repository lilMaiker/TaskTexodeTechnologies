using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace TrackerApplication.Infrastructure.Commands.Base
{
    internal abstract class Command : ICommand
    {
        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
