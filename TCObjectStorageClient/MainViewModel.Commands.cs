using System.Windows.Input;
using TCObjectStorageClient.Commands;

namespace TCObjectStorageClient.ViewModels
{
    public partial class MainViewModel
    {
        private float _progress;
        public float Progress => _progress;

        private void InvalidateProgress(float percentage)
        {
            OnPropertyChanged(ref _progress, percentage * 100.0f, propertyName: "Progress");
        }

        public ICommand OpenFileCommand => new DelegateCommand(() => UploadFiles(InvalidateProgress));

        public ICommand OpenDirectoryCommand => new DelegateCommand(() => UploadDirectory(InvalidateProgress));

        public ICommand DeleteFilesCommand => new DelegateCommand(() => DeleteAllFilesInContainer(InvalidateProgress));
    }
}
