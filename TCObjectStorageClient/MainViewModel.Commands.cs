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

        public ICommand OpenFileCommand => new DelegateCommand(this, () => UploadFiles(InvalidateProgress));

        public ICommand OpenDirectoryCommand => new DelegateCommand(this, () => UploadDirectory(InvalidateProgress));

        public ICommand DeleteFilesCommand => new DelegateCommand(this, () => DeleteAllFilesInContainer(InvalidateProgress));
    }
}
