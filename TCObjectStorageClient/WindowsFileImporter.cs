using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using TCObjectStorageClient.Annotations;
using TCObjectStorageClient.Interfaces;
using TCObjectStorageClient.IO;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace TCObjectStorageClient
{
    public class WindowsFileImporter : IFileImporter, INotifyPropertyChanged
    {
        private const string PathPreferenceKey = "orgViewpointFileImporterCurrentPath";

        private readonly IPreferences _preferences;
        private string _currentPath;

        public string CurrentPath
        {
            get => _currentPath;
            set
            {
                _currentPath = value;
                _preferences.SetString(PathPreferenceKey, value);

                OnPropertyChanged();
            }
        }

        public WindowsFileImporter(IPreferences preferences)
        {
            _preferences = preferences;

            CurrentPath = _preferences.GetString(PathPreferenceKey, null);
            if (string.IsNullOrEmpty(CurrentPath))
                CurrentPath = Environment.CurrentDirectory;
        }

        public IList<string> GetFilePathList()
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            var isShow = dialog.ShowDialog();
            if (isShow.HasValue && isShow.Value)
            {
                return dialog.FileNames;
            }
            return new List<string>();
        }

        public DirectoryInfo GetDirectoryInfo()
        {
            var dialog = new FolderBrowserDialog();
            dialog.SelectedPath = CurrentPath;
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                CurrentPath = dialog.SelectedPath;
                return new DirectoryInfo(dialog.SelectedPath);
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
