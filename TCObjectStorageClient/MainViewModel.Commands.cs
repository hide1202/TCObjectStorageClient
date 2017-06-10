using System.Windows.Input;
using TCObjectStorageClient.Commands;

namespace TCObjectStorageClient
{
    namespace ViewModels
    {
        public partial class MainViewModel
        {
            public ICommand OpenFileCommand => new DelegateCommand(this, UploadFiles);

            public ICommand OpenDirectoryCommand => new DelegateCommand(this, UploadDirectory);

            public ICommand DeleteFilesCommand => new DelegateCommand(this, DeleteAllFilesInContainer);
        }
    }
}
