using System;
using TCObjectStorageClient.ViewModels;

namespace TCObjectStorageClient.Commands
{
    public class DelegateCommand : CommandBase
    {
        private readonly Action _action;

        public DelegateCommand(MainViewModel viewModel, Action action) : base(viewModel)
        {
            _action = action;
        }

        protected override void ExecuteInternal()
        {
            _action();
        }
    }
}
