using System;
using System.Windows.Input;
using TCObjectStorageClient.ViewModels;

namespace TCObjectStorageClient.Commands
{
    public abstract class CommandBase : ICommand
    {
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            ExecuteInternal();
        }

        public event EventHandler CanExecuteChanged;

        protected abstract void ExecuteInternal();
    }
}
