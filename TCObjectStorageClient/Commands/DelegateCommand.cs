using System;
using TCObjectStorageClient.ViewModels;

namespace TCObjectStorageClient.Commands
{
    public class DelegateCommand : CommandBase
    {
        private readonly Action _action;

        public DelegateCommand(Action action)
        {
            _action = action;
        }

        protected override void ExecuteInternal()
        {
            _action();
        }
    }
}
