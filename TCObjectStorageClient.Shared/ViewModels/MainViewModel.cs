using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TCObjectStorageClient.Annotations;
using TCObjectStorageClient.Interfaces;
using TCObjectStorageClient.IO;
using TCObjectStorageClient.Models;
using TCObjectStorageClient.ObjectStorage;

namespace TCObjectStorageClient.ViewModels
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private readonly IFileImporter _fileImporter;
        private readonly IAlertDialog _alertDialog;
        private readonly IPreferences _preferences;

        private string _tenentName;
        private string _userName;
        private string _password;
        private string _token;
        private string _account;
        private string _containerName;

        public MainViewModel(IFileImporter fileImporter, IAlertDialog alertDialog, IPreferences preferences)
        {
            _fileImporter = fileImporter;
            _alertDialog = alertDialog;
            _preferences = preferences;

            TenentName = _preferences.GetString(Constants.TenentNameKey, string.Empty);
            Account = _preferences.GetString(Constants.AccountKey, string.Empty);
            UserName = _preferences.GetString(Constants.UserNameKey, string.Empty);
            ContainerName = _preferences.GetString(Constants.ContainerNameKey, string.Empty);
        }

        public string TenentName
        {
            get => _tenentName;
            set => OnPropertyChanged(ref _tenentName, value, Constants.TenentNameKey);
        }

        public string UserName
        {
            get => _userName;
            set => OnPropertyChanged(ref _userName, value, Constants.UserNameKey);
        }

        public string Password
        {
            get => _password;
            set => OnPropertyChanged(ref _password, value);
        }

        public string Token
        {
            get => _token;
            set => OnPropertyChanged(ref _token, value);
        }

        public string Account
        {
            get => _account;
            set => OnPropertyChanged(ref _account, value, Constants.AccountKey);
        }

        public string ContainerName
        {
            get => _containerName;
            set => OnPropertyChanged(ref _containerName, value, Constants.ContainerNameKey);
        }

        public void UploadFiles()
        {
            var filePathList = _fileImporter.GetFilePathList();
            if (filePathList.Count > 0)
            {
                _alertDialog.ShowAlert(filePathList.Aggregate((s1, s2) => s1 + "\n" + s2), async isOk =>
                {
                    if (isOk)
                    {
                        var isSuccess = true;

                        foreach (var filePath in filePathList)
                        {
                            TCObjectStorage client = new TCObjectStorage();
                            var token = await client.PostToken(TenentName, UserName, Password);

                            if (token == null)
                            {
                                _alertDialog.ShowAlert("Failed to get a token");
                                return;
                            }

                            var hasContainer = await client.ContainsContainer(token, Account, ContainerName);
                            if (!hasContainer)
                            {
                                _alertDialog.ShowAlert($"Invalid [{ContainerName}] container");
                                return;
                            }

                            isSuccess &= await client.UploadFile(token, Account, ContainerName, Path.GetFileName(filePath), File.ReadAllBytes(filePath));
                        }
                        _alertDialog.ShowAlert($"{(isSuccess ? "Success" : "Fail")} to upload files");
                    }
                });
            }
        }

        public async void UploadDirectory()
        {
            var dirInfo = _fileImporter.GetDirectoryInfo();
            if (dirInfo == null)
                return;

            var entity = new DirectoryEntity(dirInfo.FullName);
            var children = entity.GetAllChildren();
#if DEBUG
            foreach (var child in children)
            {
                Debug.WriteLine($"{child.pathFromBase} : {child.entity.Path}");
            }
#endif

            var isSuccess = true;
            foreach (var child in children)
            {
                TCObjectStorage client = new TCObjectStorage();
                var token = await client.PostToken(TenentName, UserName, Password);

                if (token == null)
                {
                    _alertDialog.ShowAlert("Failed to get a token");
                    return;
                }

                var hasContainer = await client.ContainsContainer(token, Account, ContainerName);
                if (!hasContainer)
                {
                    _alertDialog.ShowAlert($"Invalid [{ContainerName}] container");
                    return;
                }

                var filePath = child.pathFromBase.Replace('\\', '/');
                isSuccess &= await client.UploadFile(token, Account, ContainerName, filePath, File.ReadAllBytes(child.entity.Path));
            }
            _alertDialog.ShowAlert($"{(isSuccess ? "Success" : "Fail")} to upload files");
        }

        public async void GetFiles()
        {
            await GetFilesInContainer();
        }

        private async Task<(bool isSuccess, List<string> files)> GetFilesInContainer()
        {
            TCObjectStorage client = new TCObjectStorage();
            var token = await client.PostToken(TenentName, UserName, Password);
            var result = await client.GetFiles(token, Account, ContainerName);
            return result;
        }

        public async void DeleteAllFilesInContainer()
        {
            TCObjectStorage client = new TCObjectStorage();
            var token = await client.PostToken(TenentName, UserName, Password);

            var result = await GetFilesInContainer();
            var files = result.files;

            foreach (var file in files)
            {
                var isSuccesss = await client.DeleteFile(token, Account, ContainerName, file);
                if (!isSuccesss)
                {
                    _alertDialog.ShowAlert($"Fail to delete file : {file}");
                    return;
                }
            }
            _alertDialog.ShowAlert($"Success to delete file");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(ref string target, string value, string cacheKey = null, [CallerMemberName] string propertyName = null)
        {
            target = value;
            if (cacheKey != null)
            {
                _preferences.SetString(cacheKey, value);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
